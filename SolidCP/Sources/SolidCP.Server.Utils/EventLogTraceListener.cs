// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace SolidCP.Server.Utils
{
    public class EventLogTraceListener : TraceListener
    {
        private EventLog eventLog;
        private bool nameSet;

        public EventLog EventLog
        {
            get
            {
                return this.eventLog;
            }
            set
            {
                this.eventLog = value;
            }
        }

        public override string Name
        {
            get
            {
                if (!this.nameSet && (this.eventLog != null))
                {
                    this.nameSet = true;
                    base.Name = this.eventLog.Source;
                }
                return base.Name;
            }
            set
            {
                this.nameSet = true;
                base.Name = value;
            }
        }

        public EventLogTraceListener(EventLog eventLog)
            : base((eventLog != null) ? eventLog.Source : string.Empty)
        {
            this.eventLog = eventLog;
        }

        public EventLogTraceListener(string source)
        {
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, source);
            }

            this.eventLog = new EventLog();
            this.eventLog.Source = source;
            this.eventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
        }

        private EventInstance CreateEventInstance(TraceEventType severity, int id)
        {
            if (id > 0xffff)
            {
                id = 0xffff;
            }
            if (id < 0)
            {
                id = 0;
            }
            EventInstance instance1 = new EventInstance((long)id, 0);
            if ((severity == TraceEventType.Error) || (severity == TraceEventType.Critical))
            {
                instance1.EntryType = EventLogEntryType.Error;
                return instance1;
            }
            if (severity == TraceEventType.Warning)
            {
                instance1.EntryType = EventLogEntryType.Warning;
                return instance1;
            }
            instance1.EntryType = EventLogEntryType.Information;
            return instance1;
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, params object[] data)
        {
            if ((base.Filter == null) || base.Filter.ShouldTrace(eventCache, source, severity, id, null, null, null, data))
            {
                EventInstance instance1 = this.CreateEventInstance(severity, id);
                StringBuilder builder1 = new StringBuilder();
                if (data != null)
                {
                    for (int num1 = 0; num1 < data.Length; num1++)
                    {
                        if (num1 != 0)
                        {
                            builder1.Append(", ");
                        }
                        if (data[num1] != null)
                        {
                            builder1.Append(data[num1].ToString());
                        }
                    }
                }
                this.eventLog.WriteEvent(instance1, new object[] { builder1.ToString() });
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType severity, int id, object data)
        {
            if ((base.Filter == null) || base.Filter.ShouldTrace(eventCache, source, severity, id, null, null, data, null))
            {
                EventInstance instance1 = this.CreateEventInstance(severity, id);
                this.eventLog.WriteEvent(instance1, new object[] { data });
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string message)
        {
            if ((base.Filter == null) || base.Filter.ShouldTrace(eventCache, source, severity, id, message, null, null, null))
            {
                EventInstance instance1 = this.CreateEventInstance(severity, id);
                this.eventLog.WriteEvent(instance1, new object[] { message });
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType severity, int id, string format, params object[] args)
        {
            if ((base.Filter == null) || base.Filter.ShouldTrace(eventCache, source, severity, id, format, args, null, null))
            {
                EventInstance instance1 = this.CreateEventInstance(severity, id);
                if (args == null)
                {
                    this.eventLog.WriteEvent(instance1, new object[] { format });
                }
                else if (string.IsNullOrEmpty(format))
                {
                    string[] textArray1 = new string[args.Length];
                    for (int num1 = 0; num1 < args.Length; num1++)
                    {
                        textArray1[num1] = args[num1].ToString();
                    }
                    this.eventLog.WriteEvent(instance1, textArray1);
                }
                else
                {
                    this.eventLog.WriteEvent(instance1, new object[] { string.Format(CultureInfo.InvariantCulture, format, args) });
                }
            }
        }

        public override void Write(string message)
        {
            if (this.eventLog != null)
            {
                this.eventLog.WriteEntry(message);
            }
        }

        public override void WriteLine(string message)
        {
            this.Write(message);
        }

        public override void Close()
        {
            if (this.eventLog != null)
            {
                this.eventLog.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
            }
        }
    }
}
