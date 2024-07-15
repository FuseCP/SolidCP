using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;

namespace SolidCP.Tests
{
    public class IISExpress: IDisposable
    {
        Process? process = null;
    
        public IISExpress()
        {
            var iisExprPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "IIS Express");
            var appcmd = Path.Combine(iisExprPath, "AppCmd.exe");
            var iisexpress = Path.Combine(iisExprPath, "iisexpress.exe");
            var server = TestWebSite.Path;
            
            // setup iis express
            Process.Start(appcmd, "delete site enterprise.tests").WaitForExit();
            Process.Start(appcmd, $"add site /name:enterprise.tests /physicalPath:\"{server}\" /bindings:http/*:9033:localhost,https/*:44332:localhost").WaitForExit();
            
            // start iis express
            var startInfo = new ProcessStartInfo(iisexpress)
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = Path.GetDirectoryName(server),
                Arguments = "/site:enterprise.tests"
            };
            process = Process.Start(startInfo);

            // wait for the server to be ready
            bool done = false;
            do
            {
                try
                {
                    var client = new HttpClient();
                    var response = client.GetAsync("https://localhost:44332").Result;
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

        public static void Start() { }

        public static readonly IISExpress Current = new IISExpress();

    }
}
