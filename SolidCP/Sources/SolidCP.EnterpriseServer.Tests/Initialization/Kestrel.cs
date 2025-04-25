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

			var startInfo = new ProcessStartInfo(exe)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = workingDir,
				Arguments = $"\"{dll}\" --urls \"{HttpUrl};{HttpsUrl}\"",
				EnvironmentVariables = {
					{ "ASPNETCORE_ENVIRONMENT", "Development" },
					//{ "ASPNETCORE_URLS", $"{HttpUrl};{HttpsUrl}" },
					//{ "ASPNETCORE_Kestrel__Certificates__Default__Path", pfx },
					//{ "ASPNETCORE_Kestrel__Certificates__Default__Password", Certificate.Password },
					//{ "ServerCertificate__File", pfx },
					//{ "ServerCertificate__Password", Certificate.Password },
				}
			};
			// redirect output to console
			startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardError = true;
			process = Process.Start(startInfo);
			process.ErrorDataReceived += (sender, arg) =>
			{
				var mainColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Kestrel>{arg.Data}");
				Debug.WriteLine($"Kestrel>{arg.Data}");
				Console.ForegroundColor = mainColor;
			};
			process.OutputDataReceived += (sender, arg) =>
			{
				Debug.WriteLine($"Kestrel>{arg.Data}");
				Console.WriteLine($"Kestrel>{arg.Data}");
			};
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

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
            if (process != null && !process.HasExited) process.Kill();
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
