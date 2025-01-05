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

#if NETCOREAPP
public class SetupAssemblyLoadContext : AssemblyLoadContext
{
	public AssemblyLoader Loader { get; set; } = null;

	public SetupAssemblyLoadContext() : base("Setup Context", true) { }
	protected override Assembly Load(AssemblyName name)
	{
		if (name.Name == "netstandard" || name.Name == "System.Runtime" || name.Name == "System.Runtime.Loader") return null;

		lock (this)
		{
			if (Loader == null) Loader = AssemblyLoader.Init(Assemblies.Single());
		}
	
		
		var loadedAssembly = Assemblies.FirstOrDefault(a =>
		{
			var lname = a.GetName();
			return lname.Name == name.Name && lname.Version >= name.Version;
		});
		if (loadedAssembly != null)
			return loadedAssembly;
		
		return Loader?.ResolveAssembly(this, name);
	}
}
#endif

	// This class exists in SolidCP.UniversalInstaller, SolidCP.Setup and in
	// SolidCP.UniversalInstaller.Runtime. It is responsible for loading the assemblies in the
	// EmbeddedResources. The version in SolidCP.Setup is for NET Standard and relies on the
	// version in SolidCP.UniversalInstaller.Runtime that is NetFX & NetCore specific.
	public class AssemblyLoader
{
	public const string NativeDllsFolder = "NativeDlls";
	public const string NativeDllsNetFXFolder = "NativeNetFXDlls";
	public const string NativeDllsNetCoreFolder = "NativeNetCoreDlls";
	public const string DllsFolder = "Dlls";
	public const string DllsNetFXFolder = "DllsNetFX";
	public const string DllsNetCoreFolder = "DllsNetCore";
	public static AssemblyLoader Current { get; private set; } = null;
	public string AssembliesPath { get; set; } = null;
	public object AssemblyLoadContext { get; set; } = null;
	public object LoaderRuntime { get; set; } = null;
	public bool IsDefault { get; set; } = false;
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

	public static AssemblyLoader Init(Assembly mainAssembly = null)
	{
		bool isSetup = mainAssembly != null;
		mainAssembly ??= Assembly.GetExecutingAssembly();

#if Costura
		var guid = Guid.NewGuid();
		var path = Path.Combine(Path.GetTempPath(), guid.ToString("D"));
		if (!Directory.Exists(path)) Directory.CreateDirectory(path);
		AppDomain.CurrentDomain.DomainUnload += (sender, args) => Directory.Delete(path, true);
#else
		string path;
		if (isSetup)
		{
			var guid = Guid.NewGuid();
			path = Path.Combine(Path.GetTempPath(), guid.ToString("D"));
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			AppDomain.CurrentDomain.DomainUnload += (sender, args) => Directory.Delete(path, true);
		} else path = Path.Combine(Path.GetDirectoryName(mainAssembly.Location));
#endif

		var loader = Current = new AssemblyLoader() { AssembliesPath = path, MainAssembly = mainAssembly };

#if Costura
		loader.SaveAssembliesToDisk();
#else
		if (isSetup) loader.SaveAssembliesToDisk();
#endif
		if (IsCore)
		{
			var ctxType = Type.GetType("System.Runtime.Loader.AssemblyLoadContext, System.Runtime.Loader");
			var getContext = ctxType.GetMethod("GetLoadContext", BindingFlags.Public | BindingFlags.Static);
			var loadContext = getContext.Invoke(null, new[] { mainAssembly });
			loader.AssemblyLoadContext = loadContext;

			var core = loader.ResolveAssembly(null, new AssemblyName("SolidCP.UniversalInstaller.Core"));
			var runtime = loader.ResolveAssembly(null, new AssemblyName($"SolidCP.UniversalInstaller.Runtime.{(IsCore ? "NetCore" : "NetFX")}"));
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
		Current = this;
		AssembliesPath = assembliesPath;
		MainAssembly = mainAssembly;
		var ctx = System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(mainAssembly);
		IsDefault = ctx == System.Runtime.Loader.AssemblyLoadContext.Default;
		if (IsDefault) {
			ctx.Resolving += (loadContext, name) => ResolveAssembly(loadContext, name);
		}
		else if (ctx is SetupAssemblyLoadContext setupCtx)
		{
			setupCtx.Loader = this;
		}
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
		Assembly assembly;
		if (loadContext == null) loadContext = AssemblyLoadContext;
		var file = ResolveAssemblyName(name.FullName, name.CultureInfo);
		if (file != null)
		{
			if (loadContext == null) assembly = Assembly.LoadFrom(file);
			else
			{
#if NETCOREAPP
				var ctx = loadContext as AssemblyLoadContext;
				assembly = ctx.LoadFromAssemblyPath(file);
#elif NETSTANDARD
				var loadFromMethod = loadContext.GetType().GetMethod("LoadFromAssemblyPath");
				assembly = loadFromMethod.Invoke(AssemblyLoadContext, new[] { file }) as Assembly;
				
#else
				assembly = Assembly.LoadFrom(file);
#endif
			}
			if (assembly != null) Console.WriteLine($"Loaded assembly {file}.");
			return assembly;
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
		var asm = MainAssembly;
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
			if (resname.StartsWith(NativeDllsFolder + "/") ||
				IsNetFX && resname.StartsWith(NativeDllsNetFXFolder + "/") ||
				IsCore && resname.StartsWith(NativeDllsNetCoreFolder + "/"))
			{
				var slash = resname.IndexOf('/');
				var name = resname.Substring(slash + 1)
					.Replace('/', Path.DirectorySeparatorChar)
					.Replace('\\', Path.DirectorySeparatorChar);
				if (/*(name.StartsWith($"{arch}/") || !name.Contains("/")) && */
					IsWindows && name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ||
					IsMac && name.EndsWith(".dylib", StringComparison.OrdinalIgnoreCase) ||
					IsLinux && name.EndsWith(".so", StringComparison.OrdinalIgnoreCase))
				{
					var filename = Path.Combine(AssembliesPath, name);
					using (var src = asm.GetManifestResourceStream(resname))
						SaveToFile(src, filename);
					/*if (IsCore)
					{
						//NativeLibrary.Load(filename);
						var nativeLibType = Type.GetType("System.Runtime.InteropServices.NativeLinbrary, System.Runtime.InteropServices");
						var load = nativeLibType.GetMethod("Load", BindingFlags.Static | BindingFlags.Public);
						load.Invoke(null, new[] { filename });
					}*/
				}
			} else if (resname.StartsWith(DllsFolder + "/") ||
				IsNetFX && resname.StartsWith(DllsNetFXFolder + "/") ||
				IsCore && resname.StartsWith(DllsNetCoreFolder + "/"))
			{
				var slash = resname.IndexOf('/');
				var name = resname.Substring(slash + 1)
					.Replace('/', Path.DirectorySeparatorChar)
					.Replace('\\', Path.DirectorySeparatorChar);
				var filename = Path.Combine(AssembliesPath, name);
				using (var src = asm.GetManifestResourceStream(resname))
					SaveToFile(src, filename);
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
		if (AssemblyLoadContext != null && !IsDefault)
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
