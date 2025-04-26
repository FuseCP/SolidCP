using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Net.Http;
using SolidCP.Providers.OS;

namespace SolidCP.Tests
{
    public class Kestrel: IDisposable
    {
        Process? process = null;
		string pidfile = null;

		public const string HttpUrl = "http://localhost:9047";
        public const string HttpsUrl = "https://localhost:9048";
		public const string NetTcpUrl = "net.tcp://localhost:9067";
		public static Kestrel Current { get; private set; } = null;
		public Kestrel()
		{
			var apppath = EnterpriseServer.Path;
			var testdllpath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			var testprojpath = Path.GetFullPath(Path.Combine(testdllpath, "..", "..", ".."));
			var workingDir = Path.Combine(apppath, "bin_dotnet");
			var dll = Path.Combine(workingDir, "SolidCP.EnterpriseServer.dll");
			var exe = Shell.Standard.Find("dotnet");
			var pfx = Certificate.CertFilePath;

			// kill old processes
			pidfile = Path.Combine(testprojpath, "Data", "kestrelpids.txt");
			if (!Directory.Exists(Path.GetDirectoryName(pidfile)))
				Directory.CreateDirectory(Path.GetDirectoryName(pidfile));
			var pids = File.Exists(pidfile) ? File.ReadAllLines(pidfile)
				.Select(line => int.Parse(line)) :
				Enumerable.Empty<int>();
			foreach (var pid in pids)
			{
				try
				{
					var proc = Process.GetProcessById(pid);
					if (proc != null && !proc.HasExited)
					{
						proc.Kill();
						proc.WaitForExit(5000);
					}
				}
				catch (Exception ex) { }
			}

			var shell = Shell.Standard.Clone;
			shell.Log += msg =>
			{
				if (Debugger.IsAttached) Debug.Write($"Kestrel>{msg}");
				Console.Write($"Kestrel>{msg}");
			};
			shell.LogError += msg =>
			{
				if (Debugger.IsAttached) Debug.Write($"Kestrel>{msg}");
				var mainColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($"Kestrel>{msg}");
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
			File.AppendAllLines(pidfile, new[] { process.Id.ToString() });

			if (process.HasExited) throw new Exception($"Kestrel exited with code {process.ExitCode}");

			// wait for the server to be ready
			bool done = false;
			int n = 0;
			const int max = 20;
            do
            {
                try
                {
					var client = new HttpClient();
					var response = client.GetAsync(HttpsUrl).Result;
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
			if (process != null && !process.HasExited)
			{
				var pid = process.Id;
				process.Kill();
				var pids = File.ReadAllLines(pidfile)
					.Where(line => line != pid.ToString())
					.ToArray();
				if (pids.Any()) File.WriteAllLines(pidfile, pids);
				else File.Delete(pidfile);
			}

            process = null;
        }
		
		public static void Start()
		{
			if (Current == null)
			{
				Current = new Kestrel();
			}
		}

		public static void Stop()
		{
			if (Current != null)
			{
				Current.Dispose();
				Current = null;
			}
		}
	}
}
