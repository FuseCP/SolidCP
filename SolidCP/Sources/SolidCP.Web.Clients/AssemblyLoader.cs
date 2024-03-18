using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections.Concurrent;
using System.Security.Cryptography;
#if NETFRAMEWORK
using System.Configuration;
#endif

using SolidCP.Providers.OS;

namespace SolidCP.Web.Clients
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

		static char ToChar(byte b)
		{
			b = (byte)(b & 0x1F);
			if (b < 10) return (char)(b + (byte)'0');
			else return (char)(b - 10 + (byte)'a');
		}
		public static IEnumerable<char> ToName(IEnumerable<byte> bytes)
		{
			uint carry = 0;

			foreach (var b in bytes)
			{
				carry = (carry << 3) + (uint)(b & 0x7);
				if (carry >= 0x1F)
				{
					yield return ToChar((byte)(carry & 0x1F));
					carry = carry >> 5;
				}
				yield return ToChar((byte)((uint)b >> 3));
			}
			if (carry > 0) yield return ToChar((byte)(carry & 0x1F));

		}
		public static string SHA1Name(string plainText)
		{
			// Convert plain text into a byte array.
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			HashAlgorithm hash = new SHA1Managed();

			// Compute hash value of our plain text with appended salt.
			byte[] hashBytes = hash.ComputeHash(plainTextBytes);

			// Return the result.
			return new string(ToName(hashBytes).ToArray());
		}

		public static string TempFile(string file)
		{
			var dir = Path.Combine(ShadowCopyFolder, SHA1Name(Path.GetDirectoryName(file)));
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			return Path.Combine(dir, Path.GetFileName(file));
		}

		public static void Dispose()
		{
			if (shadowCopyFolder != null && Directory.Exists(shadowCopyFolder))
			{
				var files = Directory.EnumerateFiles(ShadowCopyFolder, "*.*", SearchOption.AllDirectories);
				var directories = new List<string>();
				var exclude = new List<string>();
				foreach (var file in files)
				{
					try
					{
						File.Delete(file);
						directories.Add(Path.GetDirectoryName(file));
					}
					catch {
						exclude.Add(Path.GetDirectoryName(file));
					}
				}
				foreach (var dir in directories
					.Distinct()
					.Except(exclude)
					.OrderByDescending(d => d))
				{
					try
					{
						Directory.Delete(dir);
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
						try
						{
							File.Copy(p.FullName, file, true);
							var pdb = Path.ChangeExtension(p.FullName, ".pdb");
							if (File.Exists(pdb)) File.Copy(pdb, Path.ChangeExtension(file, ".pdb"), true);
						}
						catch { }
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