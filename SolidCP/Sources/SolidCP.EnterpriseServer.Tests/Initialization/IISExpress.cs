using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using System.Net;

namespace SolidCP.Tests
{
    public class IISExpress: IDisposable
    {
        Process? process = null;

		public const string HttpUrl = "http://localhost:9053";
		public const string HttpsUrl = "https://localhost:44332";
		public const string NetTcpUrl = "net.tcp://localhost:9068";
		public static IISExpress Current { get; private set; } = null;
		public int HttpPort => new Uri(HttpUrl).Port;
		public int HttpsPort => new Uri(HttpsUrl).Port;
		public int NetTcpPort => new Uri(NetTcpUrl).Port;
		public IISExpress()
        {
			// Always trust certificates
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
			
            var iisExprPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express");
            var appcmd = Path.Combine(iisExprPath, "AppCmd.exe");
            var iisexpress = Path.Combine(iisExprPath, "iisexpress.exe");
            var server = EnterpriseServer.Path;
            
            // setup iis express
            Process.Start(appcmd, "delete site enterprise.tests").WaitForExit();
            Process.Start(appcmd, $"add site /name:enterprise.tests /physicalPath:\"{server}\" /bindings:http/*:{HttpPort}:localhost,https/*:{HttpsPort}:localhost").WaitForExit();
            
            // start iis express
            var startInfo = new ProcessStartInfo(iisexpress)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = Path.GetDirectoryName(server),
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				Arguments = "/site:enterprise.tests"
            };
			// redirect output to console
			startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardError = true;
			process = Process.Start(startInfo);
			process.ErrorDataReceived += (sender, arg) =>
			{
				var mainColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"IIS Express>{arg.Data}");
				Debug.WriteLine($"IIS Express>{arg.Data}");
				Console.ForegroundColor = mainColor;
			};
			process.OutputDataReceived += (sender, arg) =>
			{
				Debug.WriteLine($"IIS Express>{arg.Data}");
				Console.WriteLine($"IIS Express>{arg.Data}");
			};
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

			//if (process.HasExited) throw new Exception($"IIS Express exited with code {process.ExitCode}");

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
                if (process.HasExited) done = true; // throw new Exception("Server has terminated.");
				if (n++ >= max) done = true;
            } while (!done);
        }

        public void Dispose()
        {
            if (process != null && !process.HasExited) process.Kill();
            process = null;
        }

		public static void Start() {
			if (Current == null)
			{
				Current = new IISExpress();
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
