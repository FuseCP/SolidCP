using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
#if NETCOREAPP
using System.Runtime.Loader;
#endif

namespace SolidCP.UniversalInstaller;

public class AssemblyLoader
{
	[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
	public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, uint dwFlags);
	public static bool IsCore => !(IsNetFX || IsNetNative);
	public static bool IsNetFX => RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);
	public static bool IsNetNative => RuntimeInformation.FrameworkDescription.StartsWith(".NET Native", StringComparison.OrdinalIgnoreCase);
	public static bool IsMono => Type.GetType("Mono.Runtime") != null;
	public static bool IsWindows => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
	public static bool IsLinuxMusl
	{
		get
		{
			if (!IsLinux) return false;

			var info = new ProcessStartInfo("ldd");
			info.Arguments = "/bin/ls";
			info.RedirectStandardOutput = true;
			info.UseShellExecute = false;
			var p = Process.Start(info);
			return p.StandardOutput.ReadToEnd().Contains("musl");
		}
	}
	public static bool IsLinux => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
	public static bool IsMac => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);

	protected object AssemblyLoadContext { get; set; } = null;
	public static AssemblyLoader Init()
	{
		var mainAssembly = Assembly.GetExecutingAssembly();
		object assemblyLoadContext = null;
		Type loaderType = null;
		var loaderGenericType = typeof(AssemblyLoader<>);
		if (IsCore)
		{
			var ctxType = Type.GetType("System.Runtime.Loader.AssemblyLoadContext, System.Runtime.Loader");
			var getContext = ctxType.GetMethod("GetAssemblyLoadContext", BindingFlags.Public | BindingFlags.Static);
			assemblyLoadContext = getContext.Invoke(null, new[] { mainAssembly });
			loaderType = loaderGenericType.MakeGenericType(ctxType);
		} else
		{
			loaderType = loaderGenericType.MakeGenericType(typeof(object));
		}
		var init = loaderType.GetMethod("Init", BindingFlags.Static | BindingFlags.Public);
		return init.Invoke(null, new[] { assemblyLoadContext, mainAssembly }) as AssemblyLoader;
	}

	public void Unload()
	{
		if (AssemblyLoadContext != null)
		{
			var unload = AssemblyLoadContext.GetType().GetMethod("Unload");
			unload.Invoke(AssemblyLoadContext, new object[0]);
		}
	}

	public void CosturaInit()
	{
		CosturaUtility.Initialize();
	}
}

public class AssemblyLoader<T>: AssemblyLoader where T: class
{
	public const string NativeDllsFolder = "NativeDlls";

	protected string assembliesPath = null;

	T assemblyLoadContext = null;
	public static AssemblyLoader<T> Init(T assemblyLoadContext, Assembly mainAssembly)
	{
		var guid = Guid.NewGuid();
		var path = Path.Combine(Path.GetTempPath(), guid.ToString("D"));
		var loader = new AssemblyLoader<T>() { assembliesPath = path, assemblyLoadContext = assemblyLoadContext };
		if (!Directory.Exists(path)) Directory.CreateDirectory(path);
		AppDomain.CurrentDomain.DomainUnload += (sender, args) => Directory.Delete(path, true);
		loader.SaveAssembliesToDisk();

#if NETCOREAPP
		var ctx = assemblyLoadContext as AssemblyLoadContext;
		ctx.Resolving += (ctx, name) => loader.ResolveAssembly(ctx as T, name);
#elif NETSTANDARD
		if (IsCore)
		{
			var type = typeof(T);
			var funcGeneric = typeof(Func<,,>);
			var func = funcGeneric.MakeGenericType(type, typeof(AssemblyName), typeof(Assembly));
			var resolve = loader.GetType().GetMethod("ResolveAssembly");
			var resolving = type.GetEvent("Resolving");
			var add = resolving.GetAddMethod();
			add.Invoke(assemblyLoadContext, new object[] { Convert.ChangeType(resolve, func) });
		}
		else
		{
			AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => loader.ResolveAssembly(null, new AssemblyName(args.Name));
		}
#else
		AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => loader.ResolveAssembly(null, new AssemblyName(args.Name));
#endif
		return loader;
	}

	public string ResolveAssemblyName(string assemblyName, CultureInfo culture)
	{
		var name = new AssemblyName(assemblyName).Name + ".dll";
		var file = Path.Combine(assembliesPath, name);
		if (File.Exists(file)) return file;
		if (culture != null && !culture.IsNeutralCulture)
		{
			file = Path.Combine(assembliesPath, culture.TwoLetterISOLanguageName.ToLower(), name);
			if (File.Exists(file)) return file;
			file = Path.Combine(assembliesPath, culture.Name.ToLower(), name);
			if (File.Exists(file)) return file;
		}
		return null;
	}

	public Assembly ResolveAssembly(T loadContext, AssemblyName name)
	{
		if (loadContext == null) loadContext = assemblyLoadContext;
		var file = ResolveAssemblyName(name.FullName, name.CultureInfo);
		if (file != null)
		{
			if (loadContext == null) return Assembly.LoadFrom(file);
			else
			{
#if NETCOREAPP
				var ctx = loadContext as AssemblyLoadContext;
				return ctx.LoadFromAssemblyPath(file);
#elif NETSTANDARD
				var loadFromMethod = loadContext.GetType().GetMethod("LoadFromAssemblyPath");
				return loadFromMethod.Invoke(assemblyLoadContext, new[] { file }) as Assembly;
#else
				return Assembly.LoadFrom(file);
#endif
			}
		}
		return null;
	}
	public void SaveToFile(Stream src, string filename)
	{
		var dir = Path.GetDirectoryName(filename);
		if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

		using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write))
			src.CopyTo(file);
	}
	public void SaveAssembliesToDisk()
	{
		var resourceNames = new HashSet<string>();
		var asm = typeof(AssemblyLoader).Assembly;
		using (StreamReader reader = new StreamReader(asm.GetManifestResourceStream("costura.metadata")))
		{
			var line = reader.ReadLine();
			while (line != null)
			{
				var tokens = line.Split('|');
				var resname = tokens[0];
				resourceNames.Add(resname);
				var filename = tokens[3].Replace('/', Path.DirectorySeparatorChar);
				filename = Path.Combine(assembliesPath, filename);
				using (var resStream = asm.GetManifestResourceStream(resname))
				{
					if (resname.EndsWith(".compressed"))
					{
						using (var src = new DeflateStream(resStream, CompressionMode.Decompress))
							SaveToFile(src, filename);
					}
					else SaveToFile(resStream, filename);
				}
				line = reader.ReadLine();
			}
		}

		var arch = RuntimeInformation.ProcessArchitecture.ToString().ToLower();
		if (arch == "x64" && IsLinuxMusl) arch = "musl-x64";

		foreach (var resname in asm.GetManifestResourceNames())
		{
			if (resname.StartsWith($"{NativeDllsFolder}/"))
			{
				var name = resname.Substring($"{NativeDllsFolder}/".Length);
				if ((name.StartsWith($"{arch}/") || !name.Contains("/")) &&
					IsWindows && name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ||
					IsMac && name.EndsWith(".dylib", StringComparison.OrdinalIgnoreCase) ||
					IsLinux && name.EndsWith(".so", StringComparison.OrdinalIgnoreCase))
				{
					var filename = Path.Combine(assembliesPath, name.Replace('/', Path.DirectorySeparatorChar));
					using (var src = asm.GetManifestResourceStream(resname))
					using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write))
						src.CopyTo(file);
					if (IsCore)
					{
						//NativeLibrary.Load(filename);
						var nativeLibType = Type.GetType("System.Runtime.InteropServices.NativeLinbrary, System.Runtime.InteropServices");
						var load = nativeLibType.GetMethod("Load", BindingFlags.Static | BindingFlags.Public);
						load.Invoke(null, new[] { filename });
					}
				}
			}
		}

		if (IsNetFX) AddEnvironmentPaths(assembliesPath, Path.Combine(assembliesPath, arch));
	}

	private void AddEnvironmentPaths(params IEnumerable<string> newpaths)
	{
		var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

		var newpath = string.Join(Path.PathSeparator.ToString(), path.Concat(newpaths));

		Environment.SetEnvironmentVariable("PATH", newpath);
	}
}
