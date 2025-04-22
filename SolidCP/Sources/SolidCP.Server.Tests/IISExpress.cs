using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.Net;

namespace SolidCP.Server.Tests
{
    public class IISExpress: IDisposable
    {
        Process? process = null;

        public const string HttpUrl = "http://localhost:9052";
        public const string HttpsUrl = "https://localhost:44301";
        public IISExpress()
        {
			// Always trust certificates
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
			
            var iisExprPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express");
            var appcmd = Path.Combine(iisExprPath, "AppCmd.exe");
            var iisexpress = Path.Combine(iisExprPath, "iisexpress.exe");
			var testdllpath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			var testprojpath = Path.GetFullPath(Path.Combine(testdllpath, "..", "..", ".."));
			var workingDir = Path.GetFullPath(Path.Combine(testprojpath, "..", "SolidCP.Server", "bin_dotnet"));
            var server = Path.GetFullPath(Path.Combine(testprojpath, "..", "SolidCP.Server"));
            // setup iis express
            Process.Start(appcmd, "delete site solidcp.server.tests").WaitForExit();
            Process.Start(appcmd, $"add site /name:solidcp.server.tests /physicalPath:\"{server}\" /bindings:http/*:9052:localhost,https/*:44301:localhost").WaitForExit();
            
			// start iis express
			var startInfo = new ProcessStartInfo(iisexpress)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = workingDir,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				Arguments = "/site:solidcp.server.tests"
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

			//if (process.HasExited) done = true; // throw new Exception($"IIS Express exited with code {process.ExitCode}");

			// wait for the server to be ready
			bool done = false;
            do
            {
                try
                {
					var handler = new HttpClientHandler();
					handler.ClientCertificateOptions = ClientCertificateOption.Manual;
					handler.ServerCertificateCustomValidationCallback =
						(httpRequestMessage, cert, cetChain, policyErrors) =>
						{
							return true;
						};
					var client = new HttpClient(handler);
					var response = client.GetAsync(HttpsUrl).Result;
                    done = true;
                }
                catch (Exception ex) { }

                if (!done) Thread.Sleep(2000);
                if (process.HasExited) done = true; // throw new Exception("Server has terminated.");

            } while (!done);
        }

        public void Dispose()
        {
            if (process != null && !process.HasExited) process.Kill();
            process = null;
        }

    }
}
