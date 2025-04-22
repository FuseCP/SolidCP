using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using SolidCP.Providers.OS;

namespace SolidCP.Server.Tests
{
    public class Kestrel: IDisposable
    {
        Process? process = null;

		public const string HttpUrl = "http://localhost:9055";
		public const string HttpsUrl = "https://localhost:9056";
		public Kestrel()
        {
			var testdllpath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			var testprojpath = Path.GetFullPath(Path.Combine(testdllpath, "..", "..", ".."));
			var workingDir = Path.GetFullPath(Path.Combine(testdllpath, "..", "..", "..", "..", "SolidCP.Server", "bin_dotnet"));
			var server = Path.GetFullPath(Path.Combine(testdllpath, "..", "..", "..", "..", "SolidCP.Server"));

            var exe = Shell.Standard.Find("dotnet");
            var dll = Path.Combine(workingDir, "SolidCP.Server.dll");

			var startInfo = new ProcessStartInfo(exe)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = workingDir,
                Arguments = $"\"{dll}\" --urls \"http://localhost:9055;https://localhost:9056\"",
                EnvironmentVariables = {
					{ "ASPNETCORE_ENVIRONMENT", "Development" },
					{ "ASPNETCORE_URLS", "http://localhost:9055;https://localhost:9056" },
					{ "ASPNETCORE_Kestrel__Certificates__Default__Path", Path.Combine(testprojpath, "localhost.pfx") },
					{ "ASPNETCORE_Kestrel__Certificates__Default__Password", "123456" },
                    { "ASPNETCORE_ServerCertificate__File", Path.Combine(testprojpath, "localhost.pfx") },
					{ "ASPNETCORE_ServerCertificate__Password", "123456" },
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
                if (process.HasExited) throw new Exception("Server has terminated.");
    
            } while (!done) ;
        }

        public void Dispose()
        {
            if (process != null && !process.HasExited) process.Kill();
            process = null;
        }
    }
}
