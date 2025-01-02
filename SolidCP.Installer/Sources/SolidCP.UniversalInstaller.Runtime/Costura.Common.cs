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
using SolidCP.Providers.OS;

public class Loader
{
    static int initialized = 0;

    AssemblyLoadContext
    public static void Init()
    {
        if (Interlocked.Exchange(ref initialized, 1) == 0)
        {
            var guid = Guid.NewGuid();
            var path = Path.Combine(Path.GetTempPath(), guid.ToString("X"));
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            AppDomain.CurrentDomain.DomainUnload += (sender, args) => Directory.Delete(path, true);
        }
    }

    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, uint dwFlags);

    [Conditional("DEBUG")]
    public void Log(string format, params object[] args)
    {
        //#if DEBUG
        //        Console.WriteLine("=== COSTURA === " + string.Format(format, args));
        //#else
        //        // Should this be trace?
        //        Debug.WriteLine("=== COSTURA === " + string.Format(format, args));
        //#endif

        // Should this be trace?
        Debug.WriteLine("=== COSTURA === " + string.Format(format, args));
        Console.WriteLine("=== COSTURA === " + string.Format(format, args));
    }

    private void CopyTo(Stream source, Stream destination)
    {
        var array = new byte[81920];
        int count;
        while ((count = source.Read(array, 0, array.Length)) != 0)
        {
            destination.Write(array, 0, count);
        }
    }

    private void CreateDirectory(string tempBasePath)
    {
        if (!Directory.Exists(tempBasePath))
        {
            Directory.CreateDirectory(tempBasePath);
        }
    }

    private byte[] ReadStream(Stream stream)
    {
        var data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        return data;
    }

    public string CalculateChecksum(string filename)
    {
        using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
        using (var bs = new BufferedStream(fs))
        using (var sha1 = SHA1.Create())
        {
            var hash = sha1.ComputeHash(bs);
            var formatted = new StringBuilder(2 * hash.Length);
            foreach (var b in hash)
            {
                formatted.AppendFormat("{0:X2}", b);
            }
            return formatted.ToString();
        }
    }

    public Assembly ReadExistingAssembly(AssemblyName name)
    {
        var currentDomain = AppDomain.CurrentDomain;
        var assemblies = currentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var currentName = assembly.GetName();
            if (string.Equals(currentName.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(CultureToString(currentName.CultureInfo), CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
            {
                Log("Assembly '{0}' already loaded, returning existing assembly", assembly.FullName);

                return assembly;
            }
        }
        return null;
    }

    private string CultureToString(CultureInfo culture)
    {
        if (culture is null)
        {
            return string.Empty;
        }

        return culture.Name;
    }

	public static bool IsLinuxMusl
	{
		get
		{
			if (!OSInfo.IsLinux) return false;
			return Shell.Standard.Exec("ldd /bin/ls").OutputAndError().Result.Contains("musl");
		}
	}

	public static void SaveDiskCache(string tempBasePath)
    {
        var resourceNames = new HashSet<string>();
        var asm = typeof(Loader).Assembly;
        using (StreamReader reader = new StreamReader(asm.GetManifestResourceStream("custora.metadata")))
        {
            var line = reader.ReadLine();
            while (line != null)
            {
                var tokens = line.Split('|');
                var resname = tokens[0];
                resourceNames.Add(resname);
                var filename = tokens[3];
                filename = Path.Combine(tempBasePath, filename);
                using (var resStream = asm.GetManifestResourceStream(resname))
                using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write))
                    resStream.CopyTo(file);
            }
        }

		var arch = RuntimeInformation.ProcessArchitecture.ToString().ToLower();
		if (arch == "x64" && IsLinuxMusl) arch = "musl-x64";

        foreach (var resname in asm.GetManifestResourceNames())
        {
            if (resname.StartsWith("NativeDlls/"))
            {
                var name = resname.Substring("NativeDlls/".Length);
                if ((name.StartsWith($"{arch}/") || !name.Contains("/")) &&
                    OSInfo.IsWindows && name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ||
                    OSInfo.IsMac && name.EndsWith(".dylib", StringComparison.OrdinalIgnoreCase) ||
                    OSInfo.IsLinux && name.EndsWith(".so", StringComparison.OrdinalIgnoreCase))
                {
                    var filename = Path.Combine(tempBasePath, name.Replace('/', Path.DirectorySeparatorChar));
                    using (var src = asm.GetManifestResourceStream(resname))
                    using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write))
                        src.CopyTo(file);
#if NETCOREAPP
                    NativeLibrary.Load(filename);
#endif
                }
            }
        }
#if NETFRAMEWORK
        AddEnvironmentPaths(tempBasePath, Path.Combine(tempBasePath, arch));
#endif
    }

    public Assembly ReadFromDiskCache(string tempBasePath, AssemblyName requestedAssemblyName)
    {
        var name = GetAssemblyResourceName(requestedAssemblyName);

        var platformName = GetPlatformName();

        var assemblyTempFilePath = Path.Combine(tempBasePath, string.Concat(name, ".dll"));
        if (File.Exists(assemblyTempFilePath))
        {
            return Assembly.LoadFile(assemblyTempFilePath);
        }

        assemblyTempFilePath = Path.ChangeExtension(assemblyTempFilePath, "exe");
        if (File.Exists(assemblyTempFilePath))
        {
            return Assembly.LoadFile(assemblyTempFilePath);
        }

        assemblyTempFilePath = Path.Combine(Path.Combine(tempBasePath, platformName), string.Concat(name, ".dll"));
        if (File.Exists(assemblyTempFilePath))
        {
            return Assembly.LoadFile(assemblyTempFilePath);
        }

        assemblyTempFilePath = Path.ChangeExtension(assemblyTempFilePath, "exe");
        if (File.Exists(assemblyTempFilePath))
        {
            return Assembly.LoadFile(assemblyTempFilePath);
        }

        return null;
    }

    public Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
    {
        var name = GetAssemblyResourceName(requestedAssemblyName);

        byte[] assemblyData;
        using (var assemblyStream = LoadStream(assemblyNames, name))
        {
            if (assemblyStream is null)
            {
                return null;
            }
            assemblyData = ReadStream(assemblyStream);
        }

        using (var pdbStream = LoadStream(symbolNames, name))
        {
            if (pdbStream is not null)
            {
                var pdbData = ReadStream(pdbStream);
                return Assembly.Load(assemblyData, pdbData);
            }
        }

        return Assembly.Load(assemblyData);
    }

    private string GetAssemblyResourceName(AssemblyName requestedAssemblyName)
    {
        var name = requestedAssemblyName.Name.ToLowerInvariant();

        if (requestedAssemblyName.CultureInfo is not null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
        {
            name = $"{CultureToString(requestedAssemblyName.CultureInfo)}.{name}".ToLowerInvariant();
        }

        return name;
    }

    private Stream LoadStream(Dictionary<string, string> resourceNames, string name)
    {
        if (resourceNames.TryGetValue(name, out var value))
        {
            return LoadStream(value);
        }

        return null;
    }

    private Stream LoadStream(string fullName)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        if (fullName.EndsWith(".compressed"))
        {
            using (var stream = executingAssembly.GetManifestResourceStream(fullName))
            using (var compressStream = new DeflateStream(stream, CompressionMode.Decompress))
            {
                var memStream = new MemoryStream();
                CopyTo(compressStream, memStream);
                memStream.Position = 0;
                return memStream;
            }
        }

        return executingAssembly.GetManifestResourceStream(fullName);
    }

    public void PreloadUnmanagedLibraries(string hash, string tempBasePath, List<string> libs, Dictionary<string, string> checksums)
    {
        // since tempBasePath is per user, the mutex can be per user
        var mutexId = $"Costura{hash}";

        using (var mutex = new Mutex(false, mutexId))
        {
            var hasHandle = false;
            try
            {
                try
                {
                    hasHandle = mutex.WaitOne(60000, false);
                    if (hasHandle == false)
                    {
                        throw new TimeoutException("Timeout waiting for exclusive access");
                    }
                }
                catch (AbandonedMutexException)
                {
                    hasHandle = true;
                }

                var platformName = GetPlatformName();

                var path = Path.Combine(tempBasePath, platformName);

                Log("Preloading unmanaged libraries to '{0}'", path);

                CreateDirectory(path);
                InternalPreloadUnmanagedLibraries(tempBasePath, libs, checksums);
            }
            finally
            {
                if (hasHandle)
                {
                    mutex.ReleaseMutex();
                }
            }
        }
    }

    private void InternalPreloadUnmanagedLibraries(string tempBasePath, IList<string> libs, Dictionary<string, string> checksums)
    {
        string name;

        foreach (var lib in libs)
        {
            name = ResourceNameToPath(lib);

            var assemblyTempFilePath = Path.Combine(tempBasePath, name);

            Log("Preloading unmanaged library '{0}' to '{1}'", name, assemblyTempFilePath);

            if (File.Exists(assemblyTempFilePath))
            {
                var checksum = CalculateChecksum(assemblyTempFilePath);
                if (checksum != checksums[lib])
                {
                    File.Delete(assemblyTempFilePath);
                }
            }

            if (!File.Exists(assemblyTempFilePath))
            {
                using (var copyStream = LoadStream(lib))
                using (var assemblyTempFile = File.OpenWrite(assemblyTempFilePath))
                {
                    CopyTo(copyStream, assemblyTempFile);
                }
            }
        }

        // prevent system-generated error message when LoadLibrary is called on a dll with an unmet dependency
        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms680621(v=vs.85).aspx
        //
        // SEM_FAILCRITICALERRORS - The system does not display the critical-error-handler message box. Instead, the system sends the error to the calling process.
        // SEM_NOGPFAULTERRORBOX  - The system does not display the Windows Error Reporting dialog.
        // SEM_NOOPENFILEERRORBOX - The OpenFile function does not display a message box when it fails to find a file. Instead, the error is returned to the caller.
        //
        // return value is the previous state of the error-mode bit flags.
        // ErrorModes.SEM_FAILCRITICALERRORS | ErrorModes.SEM_NOGPFAULTERRORBOX | ErrorModes.SEM_NOOPENFILEERRORBOX;
        if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            uint errorModes = 32771;
            var originalErrorMode = SetErrorMode(errorModes);

            foreach (var lib in libs)
            {
                name = ResourceNameToPath(lib);

                if (name.EndsWith(".dll"))
                {
                    var assemblyTempFilePath = Path.Combine(tempBasePath, name);

                    // LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
                    LoadLibraryEx(assemblyTempFilePath, IntPtr.Zero, 0x00000008);
                }
            }

            // restore to previous state
            SetErrorMode(originalErrorMode);
        }
        else
        {
            AddEnvironmentPaths(tempBasePath);
        }
    }

    [DllImport("kernel32.dll")]
    private static extern uint SetErrorMode(uint uMode);

    private string ResourceNameToPath(string lib)
    {
        var platformName = GetPlatformName();
        var name = lib;

        // _ instead of - since '-' is not supported in resource names
        var platformPrefix = string.Concat("costura-", platformName, ".")
            .Replace("-", "_");
        var costuraPrefix = "costura.";

        if (lib.StartsWith(platformPrefix))
        {
            name = Path.Combine(platformName, lib.Substring(platformPrefix.Length));
        }
        else if (lib.StartsWith(costuraPrefix))
        {
            name = lib.Substring(costuraPrefix.Length);
        }

        if (name.EndsWith(".compressed"))
        {
            name = name.Substring(0, name.Length - ".compressed".Length);
        }

        return name;
    }

    private static string GetPlatformName()
    {
        var os = "win";

        if (!RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {
                os = "osx";
            }
            else if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                os = "linux";
            }
        }

        var processorArchitecture = RuntimeInformation.ProcessArchitecture;

        return $"{os}-{Enum.GetName(typeof(Architecture), processorArchitecture).ToLowerInvariant()}";
    }

    private static void AddEnvironmentPaths(params IEnumerable<string> newpaths)
    {
        var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

        var newpath = string.Join(Path.PathSeparator.ToString(), path.Concat(newpaths));

        Environment.SetEnvironmentVariable("PATH", newpath);
    }
}
