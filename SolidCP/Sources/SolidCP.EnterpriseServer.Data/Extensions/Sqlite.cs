﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SolidCP.Providers.OS
{
    public class Sqlite
    {
        public bool IsLinuxMusl
        {
            get
            {
                if (!OSInfo.IsLinux) return false;
                return OS.Shell.Default.Exec("ldd /bin/ls").OutputAndError().Result.Contains("musl");
            }
        }

		static void AddEnvironmentPaths(IEnumerable<string> paths)
		{
			var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

            paths = paths.Where(p => !string.IsNullOrEmpty(p));

			string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(paths));

			Environment.SetEnvironmentVariable("PATH", newPath);
		}

		static bool nativeSQLiteDllLoaded = false;
        public static void LoadNativeDlls()
        {
#if NETFRAMEWORK
            var arch = RuntimeInformation.ProcessArchitecture;
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyFileName = Path.GetFileName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            var ExePath = AppDomain.CurrentDomain.BaseDirectory;

            var assemblyLoaderType = Type.GetType("SolidCP.Web.Clients.AssemblyLoader, SolidCP.Web.Clients");
            var pathsProperty = assemblyLoaderType.GetProperty("Paths", BindingFlags.Static | BindingFlags.Public);
            var paths = pathsProperty.GetValue(null) as string[];
            var binPaths = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath.Split(Path.PathSeparator);
			var dll = binPaths
                .Concat(paths)
                .Select(p =>
                {
	                var relativename = Path.Combine(p, assemblyFileName);
	                var fullName = new DirectoryInfo(Path.Combine(ExePath, relativename)).FullName;
	                return new
	                {
		                FullName = fullName,
		                Name = relativename
	                };
                })
                .Where(p => File.Exists(p.FullName))
                .Select(p => p.FullName)
                .FirstOrDefault();

			var assemblyPath = Path.GetDirectoryName(dll ?? new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            if (Environment.Is64BitProcess)
            {
                if (arch == Architecture.X64 || arch == Architecture.X86)
                {
					var dllPath1 = Path.Combine(assemblyPath, "x64");
					var dllPath2 = Path.Combine(assemblyPath, "x64");

					AddEnvironmentPaths(new[] { dllPath1 });
				}
				else if (arch == Architecture.Arm64)
                {
                    throw new NotSupportedException();
                }
            } else
            {
                if (arch == Architecture.X64 || arch == Architecture.X86)
                {
                    var dllPath1 = Path.Combine(assemblyPath, "x86");
					AddEnvironmentPaths(new[] { dllPath1 });
				}
				else if (arch == Architecture.Arm64)
                {
                    throw new NotSupportedException();
                }
            }

#endif
		}
    }
}