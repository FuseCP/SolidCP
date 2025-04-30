using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Reflection;
using System.Net.Http;
using SolidCP.Providers.OS;

namespace SolidCP.Tests
{
    public class Kestrel: IDisposable
    {
        Process? process = null;
		string pidfile = null;

		public (Scheme Protocol, string Url)[] Urls;
		public string HttpUrl => Urls.FirstOrDefault(u => u.Protocol == Scheme.Http).Url;
		public string HttpsUrl => Urls.FirstOrDefault(u => u.Protocol == Scheme.Https).Url;
		public string NetTcpUrl => Urls.FirstOrDefault(u => u.Protocol == Scheme.NetTcp).Url;
		Component component;
		WSLShell.WSLDistro WslDistro;
		static List<Kestrel> instances = new List<Kestrel>();
		public Kestrel(Component component, (Scheme, string)[] urls = null, WSLShell.WSLDistro wslDistro = null)
		{
			Urls = urls;
			this.component = component;
			this.WslDistro = wslDistro;
			instances.Add(this);
			var apppath = Paths.Path(component);
			var testdllpath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			var testprojpath = Path.GetFullPath(Path.Combine(testdllpath, "..", "..", ".."));
			var workingDir = Path.Combine(apppath, "bin_dotnet");
			var log = Path.GetFullPath(Path.Combine(Paths.Test, "TestResults", $"Kestrel.log"));
			if (!Directory.Exists(Path.GetDirectoryName(log))) Directory.CreateDirectory(Path.GetDirectoryName(log));
			var dll = Path.Combine(workingDir, $"{Paths.App}.{component}.dll");
			var pfx = Certificate.CertFilePath;

			Shell shell;
			if (wslDistro == null) shell = Shell.Standard.Clone;
			else
			{
				shell = new WSLShell(wslDistro);

				testdllpath = Paths.Wsl(testdllpath);
				testprojpath = Paths.Wsl(testprojpath);
				//workingDir = Paths.Wsl(workingDir);
				dll = Paths.Wsl(dll);
				pfx = Paths.Wsl(pfx);
			}
			var distro = (wslDistro?.ToString() ?? "Windows");

			var exe = shell.Find("dotnet");
			shell.LogFile = log;
			shell.LogCommand += msg =>
			{
				if (Debugger.IsAttached) Debug.Write($"{distro}>{msg}");
				Console.Write($"{distro}>{msg}");
			};
			shell.Log += msg =>
				{
					if (Debugger.IsAttached) Debug.Write($"{distro}/Kestrel>{msg}");
					Console.Write($"{distro}/Kestrel>{msg}");
				};
			shell.LogError += msg =>
			{
				if (Debugger.IsAttached) Debug.Write($"{distro}/Kestrel>{msg}");
				var mainColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($"{distro}/Kestrel>{msg}");
				Console.ForegroundColor = mainColor;
			};
			shell.Environment = new Dictionary<string, string>()
			{
				{ "ASPNETCORE_ENVIRONMENT", "Development" },
				//{ "ASPNETCORE_URLS", $"{HttpUrl};{HttpsUrl}" },
				//{ "ASPNETCORE_Kestrel__Certificates__Default__Path", pfx },
				//{ "ASPNETCORE_Kestrel__Certificates__Default__Password", Certificate.Password },
				//{ "ServerCertificate__File", pfx },
				//{ "ServerCertificate__Password", Certificate.Password },
			};
			shell.WorkingDirectory = workingDir;
			process = shell.ExecAsync($"\"{exe}\" \"{dll}\" --urls \"{HttpUrl};{HttpsUrl}\"").Process;

			if (WslDistro == null && process.HasExited) throw new Exception($"Kestrel exited with code {process.ExitCode}");

			// wait for the server to be ready
			bool done = false;
			int n = 0;
			const int max = 20;
            do
            {
                try
                {
					var response = Servers.HttpClient.GetAsync(HttpsUrl).Result;
                    done = true;
                }
                catch (Exception ex) { }

                if (!done) Thread.Sleep(2000);
				if (process.HasExited) done = true; //throw new Exception("Server has terminated.");
				if (n++ >= max) done = true;

            } while (!done) ;
        }

		public void Dispose()
		{
			instances.Remove(this);

			if (process != null && !process.HasExited) process.Kill();
			process = null;

			if (WslDistro != null)
			{
				if (!instances.Any(k => k.WslDistro == WslDistro))
				{
					WSLShell.Default.Terminate(WslDistro);
				} 
				if (!instances.Any(k => k.WslDistro != null)) {
					WSLShell.Default.ShutdownAll();
				}
			}
        }
	}
}
