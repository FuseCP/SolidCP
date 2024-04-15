using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using SyslogNet.Client;
using SyslogNet.Client.Serialization;
using SyslogNet.Client.Transport;
using Renci.SshNet.Messages;

namespace SolidCP.Providers.OS
{
    public class SyslogTraceListener : TraceListener
    {
        public const string SyslogAppName = "SolidCP";

        SyslogLocalMessageSerializer serializer = new SyslogLocalMessageSerializer();
        SyslogLocalSender sender = new SyslogLocalSender();
        Severity? Severity = null;
        bool IsClone = false;

        public SyslogTraceListener Clone
        {
            get
            {
                var clone = Activator.CreateInstance(GetType()) as SyslogTraceListener;
                clone.IsClone = true;
                clone.serializer = serializer;
                clone.sender = sender;
                clone.Severity = Severity;
                return clone;
            }
        }

        protected void SetSeverity(string message)
        {
            if (!Severity.HasValue)
            {
                if (message.IndexOf("critical", StringComparison.OrdinalIgnoreCase) >= 0) Severity = SyslogNet.Client.Severity.Critical;
                else if (message.IndexOf("error", StringComparison.OrdinalIgnoreCase) >= 0) Severity = SyslogNet.Client.Severity.Error;
                else if (message.IndexOf("warning", StringComparison.OrdinalIgnoreCase) >= 0) Severity = SyslogNet.Client.Severity.Warning;
                else Severity = SyslogNet.Client.Severity.Informational;
            }
        }

        public override void Write(string message)
        {
            if (IsClone)
            {
                var msg = new SyslogMessage(Severity.Value, SyslogAppName, message)
                {
                    DateTimeOffset = DateTimeOffset.Now,
                    HostName = Environment.MachineName,
                    Facility = Facility.UserLevelMessages
                };
                sender.Send(msg, serializer);
            } else
            {
                var clone = Clone;
                clone.SetSeverity(message);
                clone.Write(message);
            }
        }
        public override void WriteLine(string message)
        {
            Write($"{message}\n");
        }
        public override void Fail(string message)
        {
            if (IsClone) base.Fail(message);
            else
            {
                var clone = Clone;
                clone.SetSeverity(message);
                clone.Fail(message);
            }
        }

        public override void Fail(string message, string detailMessage)
        {
            if (IsClone) base.Fail(message, detailMessage);
            else
            {
                var clone = Clone;
                clone.SetSeverity(message+detailMessage);
                clone.Fail(message, detailMessage);
            }
        }

        public void SetSeverity(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Warning: Severity = SyslogNet.Client.Severity.Warning; break;
                case TraceEventType.Error: Severity = SyslogNet.Client.Severity.Error; break;
                case TraceEventType.Information: Severity = SyslogNet.Client.Severity.Informational; break;
                case TraceEventType.Verbose: Severity = SyslogNet.Client.Severity.Debug; break;
                case TraceEventType.Critical: Severity = SyslogNet.Client.Severity.Critical; break;
                default: Severity = SyslogNet.Client.Severity.Informational; break;
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (IsClone) base.TraceData(eventCache, source, eventType, id, data);
            else {
                var clone = Clone;
                clone.SetSeverity(eventType);
                clone.TraceData(eventCache, source, eventType, id, data);
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if (IsClone) base.TraceData(eventCache, source, eventType, id, data);
            else {
                var clone = Clone;
                clone.SetSeverity(eventType);
                clone.TraceData(eventCache, source, eventType, id, data);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            if (IsClone) base.TraceEvent(eventCache, source, eventType, id);
            else
            {
                var clone = Clone;
                clone.SetSeverity(eventType);
                clone.TraceEvent(eventCache, source, eventType, id);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (IsClone) base.TraceEvent(eventCache, source, eventType, id, format, args);
            else {
                var clone = Clone;
                clone.SetSeverity(eventType);
                clone.TraceEvent(eventCache, source, eventType, id, format, args);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (IsClone) base.TraceEvent(eventCache, source, eventType, id, message);
            else {
                var clone = Clone;
                clone.SetSeverity(eventType);
                clone.TraceEvent(eventCache, source, eventType, id, message);
            } 
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            if (IsClone) base.TraceTransfer(eventCache, source, id, message, relatedActivityId);
            else {
                var clone = Clone;
                clone.SetSeverity(message);
                clone.TraceTransfer(eventCache, source, id, message, relatedActivityId);
            }
        }
    }
}
