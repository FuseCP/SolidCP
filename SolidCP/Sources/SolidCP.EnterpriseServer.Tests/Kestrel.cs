using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace SolidCP.Tests
{
    public class Kestrel: IDisposable
    {
        Process? process = null;
        public Kestrel()
        {
            var exe = Path.Combine(TestWebSite.Path, "bin", "bin_dotnet", "SolidCP.EnterpriseServer.exe");
            var workingDir = Path.GetFileName(exe);
            var startInfo = new ProcessStartInfo(exe)
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = workingDir
            };
            process = Process.Start(startInfo);
            
            // wait for the server to be ready
            bool done = false;
            do
            {
                try
                {
                    var client = new HttpClient();
                    var response = client.GetAsync("https://localhost:9007").Result;
                    done = true;
                }
                catch (Exception ex) { }

                if (!done) Thread.Sleep(2000);
                if (process.HasExited) done = true; // throw new Exception("Server has terminated.");
    
            } while (!done) ;
        }

        public void Dispose()
        {
            if (process != null && !process.HasExited) process.Kill();
            process = null;
        }
		public static void Start() { }

		public static readonly Kestrel Current = new Kestrel();
	}
}
