using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SolidCP.Providers.OS;
using System.Runtime.InteropServices;

namespace SolidCP.Setup
{
	public class ResourceAssemblyLoader
	{

		public static byte[] BytesFromStream(Stream input)
		{
			using (var m = new MemoryStream((int)input.Length))
			{
				input.CopyTo(m);
				return m.ToArray();
			}
		}

		static Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();
		static ConcurrentDictionary<string, object> Locks = new ConcurrentDictionary<string, object>();


		static void AddEnvironmentPaths(IEnumerable<string> paths)
		{
			var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

			paths = paths.Where(p => !string.IsNullOrEmpty(p));

			string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(paths));

			Environment.SetEnvironmentVariable("PATH", newPath);
		}

		static void LoadNativeDll(Assembly a, string dllName)
		{
			var arch = RuntimeInformation.ProcessArchitecture;

			string file;
			if (dllName.Contains(".arm64.") || dllName.Contains(".x64.") || dllName.Contains(".x86.") || dllName.Contains(".arm."))
			{
				if (!dllName.ToLowerInvariant().Contains($".{Enum.GetName(typeof(Architecture), arch).ToLowerInvariant()}.")) return;
				file = dllName;
			}
			else
			{
				if (arch == Architecture.X64)
				{
					file = $"x64.{dllName}";
				}
				else if (arch == Architecture.X86)
				{
					file = $"x86.{dllName}";
				} else if (arch == Architecture.Arm64)
				{
					file = $"arm64.{dllName}"; 
				}
				else throw new NotSupportedException($"Architecture {arch} not supported.");
			}
			var executingAssembly = Assembly.GetExecutingAssembly();
			var resourceName = executingAssembly.GetManifestResourceNames()
				.FirstOrDefault(r => r.EndsWith(file));

			var tmpPath = Path.Combine(Path.GetTempPath(), "SolidCP.Installer", arch.ToString());
			var tmpFile = Path.Combine(tmpPath, dllName);
			if (!Directory.Exists(tmpPath)) Directory.CreateDirectory(tmpPath);

			try
			{
				using (var stream = executingAssembly.GetManifestResourceStream(resourceName))
				using (var fileStream = new FileStream(tmpFile, FileMode.Create, FileAccess.Write))
				{
					stream.CopyTo(fileStream);
				}
			}
			catch { }

			AddEnvironmentPaths(new[] { tmpPath });
		}

		static void LoadNativeDlls(Assembly a)
		{
			var name = a.GetName().Name;

			if (name == "System.Data.SQLite") LoadNativeDll(a, "SQLite.Interop.dll");
			else if (name == "SkiaSharp") LoadNativeDll(a, "libSkiaSharp.dll");
			else if (name == "Microsoft.Data.SqlClient")
			{
				LoadNativeDll(a, "Microsoft.Data.SqlClient.SNI.arm64.dll");
				LoadNativeDll(a, "Microsoft.Data.SqlClient.SNI.x64.dll");
				LoadNativeDll(a, "Microsoft.Data.SqlClient.SNI.x86.dll");
			}
		}

		public static Assembly Resolve(object sender, ResolveEventArgs args)
		{
			var name = new AssemblyName(args.Name).Name;

			var lockobj = Locks.GetOrAdd(name, new object());

			lock (lockobj)
			{
				if (LoadedAssemblies.ContainsKey(name)) return LoadedAssemblies[name];

				var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
				if (loadedAssembly != null)
				{
					LoadedAssemblies.Add(name, loadedAssembly);
					return loadedAssembly;
				}

				var host = Assembly.GetExecutingAssembly();
				var resources = host.GetManifestResourceNames();
				var assName = resources.FirstOrDefault(res => res.EndsWith($"{name}.dll", StringComparison.OrdinalIgnoreCase));
				string pdbName = null;
				if (assName != null)
				{
					using (var assStream = host.GetManifestResourceStream(assName))
					{
						if (Debugger.IsAttached)
						{
							pdbName = resources.FirstOrDefault(res => res.EndsWith($"{name}.pdb", StringComparison.OrdinalIgnoreCase));
							if (pdbName != null)
							{
								using (var pdbStream = host.GetManifestResourceStream(pdbName))
								{
									if (assStream != null && pdbStream != null)
									{
										var assembly = Assembly.Load(BytesFromStream(assStream), BytesFromStream(pdbStream));
										if (assembly != null) LoadedAssemblies.Add(name, assembly);
										return assembly;
									}
								}
							}
							else
							{
								var assembly = Assembly.Load(BytesFromStream(assStream));
								if (assembly != null) LoadedAssemblies.Add(name, assembly);
								return assembly;
							}
						}
						else
						{
							var assembly = Assembly.Load(BytesFromStream(assStream));
							if (assembly != null) LoadedAssemblies.Add(name, assembly);
							return assembly;
						}
					}
				}
				return null;
			}
		}

		public static void Init()
		{
			//AppDomain.CurrentDomain.AssemblyLoad += (sender, args) => LoadNativeDlls(args.LoadedAssembly);
			//AppDomain.CurrentDomain.AssemblyResolve += Resolve;
		}
	}
}
