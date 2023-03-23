using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;

namespace SolidCP.Server.Tests
{
    public class Kestrel: IDisposable
    {
        Process? process = null;
        public Kestrel()
        {
            var exe = new FileInfo(@"..\..\..\..\SolidCP.Server\bin\net.core\SolidCP.Server.exe").FullName;
            var startInfo = new ProcessStartInfo(exe)
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = new DirectoryInfo(@"..\..\..\..\SolidCP.Server\bin\net.core").FullName
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
    }
}
