using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SolidCP.Server.Tests
{
    public class IISExpress: IDisposable
    {
        Process? process = null;

        public const string HttpUrl = "http://localhost:9022";
        public const string HttpsUrl = "https://localhost/server";
        public IISExpress()
        {
            var iisExprPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express");
            var appcmd = Path.Combine(iisExprPath, "AppCmd.exe");
            var iisexpress = Path.Combine(iisExprPath, "iisexpress.exe");
            var server = new DirectoryInfo(@"..\..\..\..\SolidCP.Server").FullName;
            // setup iis express
            Process.Start(appcmd, "delete site solidcp.server.tests").WaitForExit();
            Process.Start(appcmd, $"add site /name:solidcp.server.tests /physicalPath:\"{server}\" /bindings:http/*:9022:localhost,https/*:44301:localhost").WaitForExit();
            
            // start iis express
            var startInfo = new ProcessStartInfo(iisexpress)
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = new DirectoryInfo(@"..\..\..\..\SolidCP.Server\bin\net.core").FullName,
                Arguments = "/site:solidcp.server.tests"
            };
            process = Process.Start(startInfo);

            // wait for the server to be ready
            bool done = false;
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

            } while (!done);
        }

        public void Dispose()
        {
            if (process != null && !process.HasExited) process.Kill();
            process = null;
        }

    }
}
