using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.Net;
using SolidCP.Providers.OS;

namespace SolidCP.Server.Tests
{
    public class IISExpress: IDisposable
    {
        Process? process = null;

        public const string HttpUrl = "http://localhost:9052";
        public const string HttpsUrl = "https://localhost:44301";
		public const string NetTcpUrl = "net.tcp://localhost:9066";
		public static IISExpress Current { get; private set; } = null;
		public int HttpPort => new Uri(HttpUrl).Port;
		public int HttpsPort => new Uri(HttpsUrl).Port;
		public int NetTcpPort => new Uri(NetTcpUrl).Port;

		public IISExpress()
		{
			// Always trust certificates
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

			var iisExprPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express");
			var appcmdex = Path.Combine(iisExprPath, "AppCmd.exe");
			var admincmd = Path.Combine(iisExprPath, "IisExpressAdminCmd.exe");
			var iisexpress = Path.Combine(iisExprPath, "iisexpress.exe");
			var testdllpath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			var testprojpath = Path.GetFullPath(Path.Combine(testdllpath, "..", "..", ".."));
			var workingDir = Path.GetFullPath(Path.Combine(testprojpath, "..", "SolidCP.Server", "bin_dotnet"));
			var server = Path.GetFullPath(Path.Combine(testprojpath, "..", "SolidCP.Server"));
			// setup iis express
			var shell = Shell.Standard.Clone;
			shell.Log += msg =>
			{
				if (Debugger.IsAttached) Debug.WriteLine($"IIS Express>{msg}");
				Console.WriteLine($"IIS Express>{msg}");
			};
			shell.LogError += msg =>
			{
				if (Debugger.IsAttached) Debug.WriteLine($"IIS Express>{msg}");
				var mainColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"IIS Express>{msg}");
				Console.ForegroundColor = mainColor;
			};
			shell.CreateNoWindow = true;
			shell.WindowStyle = ProcessWindowStyle.Minimized;
			shell.Exec($"\"{admincmd}\" setupSslUrl -url:https://localhost:{HttpsPort} -UseSelfSigned").Wait();
			shell.Exec($"\"{appcmdex}\" delete site solidcp.server.tests").Wait();
			shell.Exec($"\"{appcmdex}\" add site /name:solidcp.server.tests /physicalPath:\"{server}\" /bindings:http/*:{HttpPort}:localhost,https/*:{HttpsPort}:localhost").Wait();
            
			// start iis express
			shell.ExecAsync($"\"{iisexpress}\" /site:solidcp.server.tests");
			process = shell.Process;

			//if (process.HasExited) done = true; // throw new Exception($"IIS Express exited with code {process.ExitCode}");

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
		public static void Start()
		{
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
