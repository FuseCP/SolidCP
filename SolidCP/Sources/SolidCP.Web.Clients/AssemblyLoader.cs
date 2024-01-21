using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections.Concurrent;
#if NETFRAMEWORK
using System.Configuration;
#endif

using SolidCP.Providers.OS;

namespace SolidCP.Web.Client
{
	public class AssemblyLoader
	{
		static string shadowCopyFolder = null;
		public static string ShadowCopyFolder
		{
			get
			{
				if (shadowCopyFolder == null)
				{
					shadowCopyFolder = Path.Combine(Path.GetTempPath(), "SolidCPShadowCopies");
					if (!Directory.Exists(shadowCopyFolder)) Directory.CreateDirectory(shadowCopyFolder);
				}
				return shadowCopyFolder;
			}
		}

		public static string TempFile(string file)
		{
			return Path.Combine(ShadowCopyFolder, $"{Path.GetFileNameWithoutExtension(file)}.{Guid.NewGuid()}.dll");
		}

		public static void Dispose()
		{
			if (shadowCopyFolder != null && Directory.Exists(shadowCopyFolder))
			{
				var files = Directory.EnumerateFiles(ShadowCopyFolder);
				foreach (var file in files)
				{
					try
					{
						File.Delete(file);
					}
					catch { }
				}
			}
		}

		public static bool Initialized = false;
		public static void Init(string probingPaths = null, string exposeWebServices = null, bool loadEnterpriseServer = true)
		{
			if (Initialized) return;
			Initialized = true;

#if NETFRAMEWORK
			ProbingPaths = ConfigurationManager.AppSettings["ExternalProbingPaths"];
#else
			ProbingPaths = probingPaths;
#endif

			if (!string.IsNullOrEmpty(ProbingPaths))
			{
				AppDomain.CurrentDomain.AssemblyResolve += Resolve;

				try
				{
					if (loadEnterpriseServer)
					{
						var eserver = Assembly.Load("SolidCP.EnterpriseServer");
						if (eserver != null)
						{
							var validatorType = eserver.GetType("SolidCP.EnterpriseServer.UsernamePasswordValidator");
							var init = validatorType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
							init.Invoke(null, new object[0]);
						}
					}
				}
				catch { }

				try
				{
					var server = Assembly.Load("SolidCP.Server");
					if (server != null)
					{
						var validatorType = server.GetType("SolidCP.Server.PasswordValidator");
						var init = validatorType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
						init.Invoke(null, new object[0]);
					}
				}
				catch { }

#if NETFRAMEWORK
				exposeWebServices = ConfigurationManager.AppSettings["ExposeWebServices"];
#endif
				var exposeAnyWebServices = string.IsNullOrEmpty(exposeWebServices) &&
				!string.Equals(exposeWebServices, "none", StringComparison.OrdinalIgnoreCase);
				if (exposeAnyWebServices) StartWebServices();
			}
		}

		static void StartWebServices()
		{
#if NETFRAMEWORK
			var assembly = Assembly.Load("SolidCP.Web.Services");
			var StartupNetFX = assembly?.GetType("SolidCP.Web.Services.StartupNetFX");
			var method = StartupNetFX?.GetMethod("Start", BindingFlags.Public | BindingFlags.Static);
			method?.Invoke(null, new object[0]);
#endif
		}

		static readonly string exepath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
		static string[] paths = null;
		static string ProbingPaths = null;
		static string[] Paths => paths != null ? paths : paths =
			(string.IsNullOrEmpty(ProbingPaths) ? new string[0] :
				ProbingPaths.Replace('\\', Path.DirectorySeparatorChar)
				.Split(';'));

		static Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();
		static ConcurrentDictionary<string, object> Locks = new ConcurrentDictionary<string, object>();

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

				var dlls = Paths
				.Select(p =>
				{
					var relativename = Path.Combine(p, $"{name}.dll");
					return new
					{
						FullName = new DirectoryInfo(Path.Combine(exepath, relativename)).FullName,
						Name = relativename
					};
				})
				.Where(p => File.Exists(p.FullName));

				foreach (var p in dlls)
				{
					string file;
					// Create shadow copy
					if (OSInfo.IsWindows)
					{
						file = TempFile(p.FullName);
						File.Copy(p.FullName, file);
						var pdb = Path.ChangeExtension(p.FullName, ".pdb");
						if (File.Exists(pdb)) File.Copy(pdb, Path.ChangeExtension(file, ".pdb"));
					}
					else file = p.FullName;
					// Load assembly
					var a = Assembly.LoadFrom(file);
					if (a != null)
					{
						LoadedAssemblies.Add(name, a);

						var msg = $"Loaded assembly {p.Name}";
						Console.WriteLine(msg);
						//if (Debugger.IsAttached) Debugger.Log(1, "info", $"{msg}\r\n");
					}
					return a;
				};

				return null;
			}
		}
	}
}