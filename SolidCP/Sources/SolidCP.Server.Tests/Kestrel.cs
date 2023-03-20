using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SolidCP.Server.Tests
{
    public class Kestrel: IDisposable
    {
        Process process;
        public Kestrel()
        {
            var exe = new FileInfo(@"..\SolidCP.Server\bin\net.core\SolidCP.Server.exe").FullName;
            process = Process.Start(exe);
        }

        public void Dispose()
        {
            if (process != null) process.Kill();
            process = null;
        }
    }
}
