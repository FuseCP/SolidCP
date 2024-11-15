using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Threading;
using System.Runtime.InteropServices;
using System.Configuration.Assemblies;


#if NETFRAMEWORK
using System.Configuration;
#endif

using SolidCP.Providers.OS;

namespace SolidCP.Web.Clients
{
    public class AssemblyLoader
    {

        public const bool CreateShadowCopies = true;

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

        static Guid guid = Guid.NewGuid();
        static string ShadowDir = null;
        public static string TempFile(string file)
        {
            lock (disposeLock)
            {
                if (ShadowDir == null)
                {
                    const int MaxTries = 7;
                    int n = 0;
                    while (n < MaxTries)
                    {
                        var dir = Path.Combine(ShadowCopyFolder, guid.ToString());
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                            ShadowDir = dir;
                            break;
                        }
                        guid = new Guid();
                        n++;
                    }
                }
                if (ShadowDir == null) throw new Exception("Cannot create ShadowCopy folder");

                return Path.Combine(ShadowDir, Path.GetFileName(file));
            }
        }

        static object disposeLock = new object();
        public static void Dispose()
        {
            lock (disposeLock)
            {
                if (shadowCopyFolder != null && Directory.Exists(shadowCopyFolder))
                {
                    var excludeDir = Path.Combine(ShadowCopyFolder, guid.ToString());

                    var files = Directory.EnumerateFiles(ShadowCopyFolder, "*.*", SearchOption.AllDirectories)
                        .Where(file => !file.StartsWith(excludeDir))
                        .ToList();

                    var directories = new List<string>();
                    var exclude = new List<string>();
                    for (int i = 0; i < files.Count; i++)
                    {
                        try
                        {
                            File.Delete(files[i]);
                            directories.Add(Path.GetDirectoryName(files[i]));
                        }
                        catch
                        {
                            // remove extension
                            var file = Regex.Replace(files[i], @"\.[^.]*$", "", RegexOptions.Singleline);
                            // don't delete associated files like .pdb
                            while (i < files.Count - 1 && files[i + 1].StartsWith(file))
                            {
                                files.RemoveAt(i + 1);
                            }
                            exclude.Add(Path.GetDirectoryName(files[i]));
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
            timer?.Dispose();
            timer = null;
        }

        public static bool Initialized = false;
        public static Timer timer = new Timer(arg => Dispose(), null, 10000, Timeout.Infinite);
        public static void Init(string probingPaths = null, string exposeWebServices = null, bool loadEnterpriseServer = true)
        {
            if (Initialized) return;
            Initialized = true;

#if NETFRAMEWORK
			ProbingPaths = ConfigurationManager.AppSettings["ExternalProbingPaths"];
#else
            ProbingPaths = probingPaths;
#endif
            AppDomain.CurrentDomain.AssemblyLoad += (sender, args) => LoadNativeDlls(args.LoadedAssembly);

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
                            // init password validator
                            var validatorType = eserver.GetType("SolidCP.EnterpriseServer.UsernamePasswordValidator");
                            var init = validatorType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
                            init.Invoke(null, new object[0]);
                        }
                    }
                }
                catch (Exception ex) { }

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
                catch (Exception ex) { }

#if NETFRAMEWORK
				exposeWebServices = exposeWebServices ?? ConfigurationManager.AppSettings["ExposeWebServices"];
#endif
                var exposeAnyWebServices = string.IsNullOrEmpty(exposeWebServices) ||
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
#else
#endif
        }

		static void AddEnvironmentPaths(IEnumerable<string> paths)
		{
			var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

			paths = paths.Where(p => !string.IsNullOrEmpty(p));

			string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(paths));

			Environment.SetEnvironmentVariable("PATH", newPath);
		}

        static void LoadNativeDll(Assembly a, string assemblyPath, Architecture? arch, string dllName, bool archExtension = false)
        {
            string file;
            if (arch == Architecture.X64)
            {
                if (archExtension) file = Path.Combine(assemblyPath, Path.ChangeExtension(dllName, $"x64.dll"));
                else file = Path.Combine(assemblyPath, "x64", dllName);
            }
            else if (arch == Architecture.X86)
            {
				if (archExtension) file = Path.Combine(assemblyPath, Path.ChangeExtension(dllName, $"x86.dll"));
				else file = Path.Combine(assemblyPath, "x86", dllName);
				file = Path.Combine(assemblyPath, "x86", dllName);
            }
            else throw new NotSupportedException($"Architecture {arch} not supported.");

            // Create shadow copy
            if (OSInfo.IsWindows && CreateShadowCopies)
            {
                var temp = TempFile(file);
                File.Copy(file, temp, true);
                file = temp;
            }

            AddEnvironmentPaths(new[] { Path.GetDirectoryName(file) });
        }

        static ConcurrentDictionary<string, string> OriginalFiles = new ConcurrentDictionary<string, string>();
	    static void LoadNativeDlls(Assembly a)
        {
            if (a.IsDynamic) return;

            var file = new Uri(a.CodeBase).LocalPath;
            string originalFile = null;
            if (!OriginalFiles.TryGetValue(file, out originalFile)) originalFile = file;

            var arch = RuntimeInformation.ProcessArchitecture;
            var assemblyPath = Path.GetDirectoryName(originalFile);

            if (a.GetName().Name == "System.Data.SQLite") LoadNativeDll(a, assemblyPath, arch, "SQLite.Interop.dll");
            if (a.GetName().Name == "SkiaSharp") LoadNativeDll(a, assemblyPath, arch, "libSkiaSharp.dll");
            if (a.GetName().Name == "Microsoft.Data.SqlClient") LoadNativeDll(a, assemblyPath, arch, "Microsoft.Data.SqlClient.SNI.dll", true);
        }

        static string exepath = null;
        public static string ExePath
        {
            get
            {
                if (exepath != null) return exepath;

                var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
                if (path.EndsWith($"{Path.DirectorySeparatorChar}bin") || path.EndsWith($"{Path.DirectorySeparatorChar}bin_dotnet"))
                {
                    path = Path.GetDirectoryName(path);
                }
                return exepath = path;
            }
        }

        static string[] paths = null;
        static string ProbingPaths = null;
        public static string[] Paths => paths != null ? paths : paths =
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
                    var fullName = new DirectoryInfo(Path.Combine(ExePath, relativename)).FullName;
                    return new
                    {
                        FullName = fullName,
                        Name = relativename
                    };
                })
                .Where(p => File.Exists(p.FullName));

                foreach (var p in dlls)
                {
                    string file = null;
                    string originalFile = null;
                    // Create shadow copy
                    if (OSInfo.IsWindows && CreateShadowCopies)
                    {

                        originalFile = p.FullName;
                        var temp = TempFile(originalFile);
                        try
                        {
                            File.Copy(p.FullName, temp, true);
                            var pdb = Path.ChangeExtension(p.FullName, ".pdb");
                            if (File.Exists(pdb)) File.Copy(pdb, Path.ChangeExtension(temp, $".pdb"), true);
                            file = temp;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Cannot load assembly {temp} because it's used by another process: {ex}");
                        }
                    }
                    else file = originalFile = p.FullName;

                    OriginalFiles.AddOrUpdate(file, originalFile, (fileName, originalFileName) => originalFile);

                    // Load assembly
                    var a = Assembly.LoadFrom(file);
                    if (a != null)
                    {
                        LoadedAssemblies.Add(name, a);

                        var msg = $"Loaded assembly {p.Name}";
                        if (OSInfo.IsWindows) msg += $" from {file}";
                        Console.WriteLine(msg);
                        if (Debugger.IsAttached) Debugger.Log(1, "info", $"{msg}\r\n");
                    }
                    return a;
                };

                return null;
            }
        }
    }
}