using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using SyslogNet.Client;
using SyslogNet.Client.Serialization;
using SyslogNet.Client.Transport;

namespace SolidCP.Providers.OS
{
    public class SyslogTraceListener : TraceListener
    {
        public const string SyslogAppName = "SolidCP";
        public TraceLevel Level { get; set; }

        SyslogLocalMessageSerializer serializer = new SyslogLocalMessageSerializer();
        SyslogLocalSender sender = new SyslogLocalSender();
        Severity? severity;
        TraceListener windowsListener = null;
        TraceListener WindowsListener => windowsListener != null ? windowsListener :
            new EventLogTraceListener(SyslogAppName);

        public override void Write(string message)
        {
            if (Level == TraceLevel.Off) return;

            if (!OSInfo.IsWindows)
            {
                Severity sev = Severity.Informational;
                if (severity.HasValue) sev = severity.Value;
                else
                {
                    if (message.IndexOf("critical", StringComparison.OrdinalIgnoreCase) >= 0) sev = Severity.Critical;
                    else if (message.IndexOf("error", StringComparison.OrdinalIgnoreCase) >= 0) sev = Severity.Error;
                    else if (message.IndexOf("warning", StringComparison.OrdinalIgnoreCase) >= 0) sev = Severity.Warning;
                }
                var msg = new SyslogMessage(Severity.Informational, SyslogAppName, message)
                {
                    DateTimeOffset = DateTimeOffset.Now,
                    HostName = Environment.MachineName,
                    Facility = Facility.UserLevelMessages
                };
                sender.Send(msg, serializer);
            }
            else
            {
                WindowsListener.Write(message);
            }
        }
        public override void WriteLine(string message)
        {
            if (Level == TraceLevel.Off) return;

            if (OSInfo.IsWindows) Write($"{message}\n");
            else WindowsListener.Write(message);
        }
        public override void Fail(string message)
        {
            if (Level == TraceLevel.Off) return;

            if (!OSInfo.IsWindows)
            {
                lock (this)
                {
                    severity = Severity.Warning;
                    base.Fail(message);
                    severity = null;
                }
            } else WindowsListener.Fail(message);
        }

        public override void Fail(string message, string detailMessage)
        {
            if (Level == TraceLevel.Off) return;

            if (!OSInfo.IsWindows)
            {
                lock (this)
                {
                    severity = Severity.Warning;
                    base.Fail(message, detailMessage);
                    severity = null;
                }
            }
            else WindowsListener.Fail(message, detailMessage);
        }

        public void SetSeverity(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Warning: severity = Severity.Warning; break;
                case TraceEventType.Error: severity = Severity.Error; break;
                case TraceEventType.Information: severity = Severity.Informational; break;
                case TraceEventType.Verbose: severity = Severity.Debug; break;
                case TraceEventType.Critical: severity = Severity.Critical; break;
                default: severity = Severity.Informational; break;
            }
        }
        public bool CheckLevel(TraceEventType eventType)
        {
            switch (Level)
            {
                case TraceLevel.Verbose: return true;
                case TraceLevel.Info: return eventType <= TraceEventType.Information;
                case TraceLevel.Warning: return eventType <= TraceEventType.Warning;
                case TraceLevel.Error: return eventType <= TraceEventType.Error;
                case TraceLevel.Off:
                default: return false;
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (!CheckLevel(eventType)) return;

            if (!OSInfo.IsWindows)
            {
                lock (this)
                {
                    SetSeverity(eventType);
                    base.TraceData(eventCache, source, eventType, id, data);
                }
            }
            else WindowsListener.TraceData(eventCache, source, eventType, id, data);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if (!CheckLevel(eventType)) return;

            if (!OSInfo.IsWindows)
            {
                lock (this)
                {
                    SetSeverity(eventType);
                    base.TraceData(eventCache, source, eventType, id, data);
                }
            } else WindowsListener.TraceData(eventCache, source, eventType, id, data); 
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            if (!CheckLevel(eventType)) return;

            if (!OSInfo.IsWindows)
            {
                lock (this)
                {
                    SetSeverity(eventType);
                    base.TraceEvent(eventCache, source, eventType, id);
                }
            }
            else WindowsListener.TraceEvent(eventCache, source, eventType, id);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (CheckLevel(eventType)) return;

            if (OSInfo.IsWindows)
            {
                lock (this)
                {
                    SetSeverity(eventType);
                    base.TraceEvent(eventCache, source, eventType, id, format, args);
                }
            }
            else WindowsListener.TraceEvent(eventCache, source, eventType, id, format, args);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (!CheckLevel(eventType)) return;

            if (!OSInfo.IsWindows)
            {
                lock (this)
                {
                    SetSeverity(eventType);
                    base.TraceEvent(eventCache, source, eventType, id, message);
                }
            }
            else WindowsListener.TraceEvent(eventCache, source, eventType, id, message);
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            if (CheckLevel(TraceEventType.Transfer)) return;

            if (OSInfo.IsWindows)
            {
                lock (this)
                {
                    severity = Severity.Informational;
                    base.TraceTransfer(eventCache, source, id, message, relatedActivityId);
                }
            }
            else WindowsListener.TraceTransfer(eventCache, source, id, message, relatedActivityId);
        }
    }
}
