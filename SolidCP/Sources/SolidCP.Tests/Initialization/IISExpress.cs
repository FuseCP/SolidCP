using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using SolidCP.Providers.OS;
using System.Reflection;
using System.IO;

namespace SolidCP.Tests
{
    public class IISExpress: IDisposable
    {
        Process? process = null;

		public (Scheme Protocol, string Url)[] Urls;
		public string HttpUrl => Urls.FirstOrDefault(u => u.Protocol == Scheme.Http).Url;
		public string HttpsUrl => Urls.FirstOrDefault(u => u.Protocol == Scheme.Https).Url;
		public string NetTcpUrl => Urls.FirstOrDefault(u => u.Protocol == Scheme.NetTcp).Url;
		public static IISExpress Current { get; private set; } = null;
		public int HttpPort => new Uri(HttpUrl).Port;
		public int HttpsPort => new Uri(HttpsUrl).Port;
		public int NetTcpPort => new Uri(NetTcpUrl).Port;
		public IISExpress(Component component, (Scheme, string)[] urls = null)
        {
			Urls = urls;
			var testprojpath = Paths.Test;
			var iisExprPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express");
			var appcmd = Path.Combine(iisExprPath, "AppCmd.exe");
			var admincmd = Path.Combine(iisExprPath, "IisExpressAdminCmd.exe");
			var iisexpress = Path.Combine(iisExprPath, "iisexpress.exe");
			var serverPath = Paths.Path(component);

			// setup iis express
			var shell = Shell.Standard.Clone;
			shell.Log += msg =>
			{
				if (Debugger.IsAttached) Debug.Write($"IIS Express>{msg}");
				Console.Write($"IIS Express>{msg}");
			};
			shell.LogError += msg =>
			{
				if (Debugger.IsAttached) Debug.Write($"IIS Express>{msg}");
				var mainColor = Console.ForegroundColor;
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write($"IIS Express>{msg}");
				Console.ForegroundColor = mainColor;
			};
			shell.Exec($"\"{admincmd}\" setupSslUrl -url:{HttpsUrl} -UseSelfSigned");
			shell.Exec($"\"{appcmd}\" delete site {component}.Tests");
            shell.Exec($"\"{appcmd}\" add site /name:{component}.Tests /physicalPath:\"{serverPath}\" /bindings:http/*:{HttpPort}:localhost,https/*:{HttpsPort}:localhost");

			// start iis express
			shell.WorkingDirectory = serverPath;
			process = shell.ExecAsync($"\"{iisexpress}\" /site:{component}.Tests").Process;

			//if (process.HasExited) throw new Exception($"IIS Express exited with code {process.ExitCode}");

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
                if (process.HasExited) done = true; // throw new Exception("Server has terminated.");
				if (n++ >= max) done = true;
            } while (!done);
        }

		public void Dispose()
		{
			if (process != null && !process.HasExited) process.Kill();
			process = null;
        }
	}
}
