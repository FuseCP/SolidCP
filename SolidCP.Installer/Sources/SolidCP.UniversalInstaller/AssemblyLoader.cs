using MySqlX.XDevAPI.Common;
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
using System.Text;
using System.Threading;
#if NETCOREAPP
using System.Runtime.Loader;
#endif

namespace SolidCP.UniversalInstaller;

public class AssemblyLoader
{
	public const string NativeDllsFolder = "NativeDlls";
	public string AssembliesPath { get; set; } = null;
	public object AssemblyLoadContext { get; set; } = null;
	public object LoaderRuntime { get; set; } = null;
	public Assembly MainAssembly { get; set; }

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

	public static AssemblyLoader Init()
	{
		var mainAssembly = Assembly.GetExecutingAssembly();

#if Costura
		var guid = Guid.NewGuid();
		var path = Path.Combine(Path.GetTempPath(), guid.ToString("D"));
		if (!Directory.Exists(path)) Directory.CreateDirectory(path);
		AppDomain.CurrentDomain.DomainUnload += (sender, args) => Directory.Delete(path, true);
#else
		var path = Path.Combine(Path.GetDirectoryName(mainAssembly.Location));
#endif

		var loader = new AssemblyLoader() { AssembliesPath = path, MainAssembly = Assembly.GetExecutingAssembly() };
#if Costura
		loader.SaveAssembliesToDisk();
#endif
		if (IsCore)
		{
			var ctxType = Type.GetType("System.Runtime.Loader.AssemblyLoadContext, System.Runtime.Loader");
			var getContext = ctxType.GetMethod("GetLoadContext", BindingFlags.Public | BindingFlags.Static);
			var loadContext = getContext.Invoke(null, new[] { mainAssembly });
			var defaultProp = ctxType.GetProperty("Default", BindingFlags.Public | BindingFlags.Static);
			var defaultCtx = defaultProp.GetValue(null);
			if (loadContext != defaultCtx) loader.AssemblyLoadContext = loadContext;

			loader.ResolveAssembly(loader.AssemblyLoadContext, new AssemblyName("SolidCP.UniversalInstaller.Core"));
			var runtime = loader.ResolveAssembly(loader.AssemblyLoadContext, new AssemblyName($"SolidCP.UniversalInstaller.Runtime.{(IsCore ? "NetCore" : "NetFX")}"));
			var loaderRuntimeType = runtime.GetType("SolidCP.UniversalInstaller.AssemblyLoader");
			loader.LoaderRuntime = Activator.CreateInstance(loaderRuntimeType);
			var assCtx = loaderRuntimeType.GetMethod("InitAssemblyLoadContext");
			assCtx.Invoke(loader.LoaderRuntime, new object[] { loader.AssembliesPath, loader.MainAssembly });
		} else
		{
			AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => loader.ResolveAssembly(null, new AssemblyName(args.Name));
		}

		return loader;
	}

#if NETCOREAPP
	public void InitAssemblyLoadContext(string assembliesPath, Assembly mainAssembly)
	{
		AssembliesPath = assembliesPath;
		MainAssembly = mainAssembly;
		var ctx = System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(mainAssembly);
		ctx.Resolving += (loadContext, name) => ResolveAssembly(loadContext, name);
		AssemblyLoadContext = ctx;
	}
#endif

	public string ResolveAssemblyName(string assemblyName, CultureInfo culture)
	{
		var name = new AssemblyName(assemblyName).Name + ".dll";
		var file = Path.Combine(AssembliesPath, name);
		if (File.Exists(file)) return file;
		if (culture != null && !culture.IsNeutralCulture)
		{
			file = Path.Combine(AssembliesPath, culture.TwoLetterISOLanguageName.ToLower(), name);
			if (File.Exists(file)) return file;
			file = Path.Combine(AssembliesPath, culture.Name.ToLower(), name);
			if (File.Exists(file)) return file;
		}
		return null;
	}
	public Assembly ResolveAssembly(object loadContext, AssemblyName name)
	{
		if (loadContext == null) loadContext = AssemblyLoadContext;
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
				return loadFromMethod.Invoke(AssemblyLoadContext, new[] { file }) as Assembly;
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
				filename = Path.Combine(AssembliesPath, filename);
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
					var filename = Path.Combine(AssembliesPath, name.Replace('/', Path.DirectorySeparatorChar));
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

		if (IsNetFX) AddEnvironmentPaths(AssembliesPath, Path.Combine(AssembliesPath, arch));
	}

	private void AddEnvironmentPaths(params IEnumerable<string> newpaths)
	{
		var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

		var newpath = string.Join(Path.PathSeparator.ToString(), path.Concat(newpaths));

		Environment.SetEnvironmentVariable("PATH", newpath);
	}

	public void Unload()
	{
		if (AssemblyLoadContext != null)
		{
			try
			{
				var unload = AssemblyLoadContext.GetType().GetMethod("Unload");
				unload.Invoke(AssemblyLoadContext, new object[0]);
			}
			catch { }
		}
	}

	public void CosturaInit()
	{
#if Costura
		CosturaUtility.Initialize();
#endif
	}
}
