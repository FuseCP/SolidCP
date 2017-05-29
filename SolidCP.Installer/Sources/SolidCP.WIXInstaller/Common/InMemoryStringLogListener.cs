using System;
using System.Diagnostics;
using System.Text;

namespace SolidCP.WIXInstaller.Common
{
    class InMemoryStringLogListener : TraceListener
    {
        private const string Format = "[{0}] Message: {1}";
        static private StringBuilder m_Ctx;
        string m_id;
        static InMemoryStringLogListener()
        {
            m_Ctx = new StringBuilder();
        }
        public InMemoryStringLogListener(string InstanceID)
            : base(InstanceID)
        {
            m_id = InstanceID;
        }
        public override void Write(string Value)
        {
            WriteLog(Value);
        }
        public override void WriteLine(string Value)
        {
            WriteLog(Value + Environment.NewLine);
        }
        [Conditional("DEBUG")]
        private void WriteLog(string Value)
        {
            m_Ctx.Append(string.Format(Format, m_id, Value));
        }
    }
}
