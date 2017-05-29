using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;

namespace SolidCP.WIXInstaller.Common
{
    public class WiXLogFileListener : TraceListener
    {
        public const uint FileFlushSize = 4096;
        public const string DefaultLogFile = "SCPInstallation.log.txt";
        public static string LogFile { get; private set; }
        private StringBuilder m_Ctx;
        static WiXLogFileListener()
        {
            LogFile = Path.Combine(Path.GetTempPath() + DefaultLogFile);
        }
        public WiXLogFileListener(string LogFileName = DefaultLogFile)
            : base("WiXLogFileListener")
        {
            m_Ctx = new StringBuilder();
        }
        ~WiXLogFileListener()
        {
            Dispose(false);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Flush(true);
        }
        public override void Write(string Value)
        {
            m_Ctx.Append(Value);
            Flush();
        }
        public override void WriteLine(string Value)
        {
            m_Ctx.AppendLine(Value);
            Flush();
        }
        private void Flush(bool Force = false)
        {
            if(m_Ctx.Length >= FileFlushSize || Force)
            {
                using (var FileCtx = new StreamWriter(LogFile, true))
                {
                    FileCtx.Write(m_Ctx.ToString());
                }
                m_Ctx.Clear();
            }
        }
    }
}
