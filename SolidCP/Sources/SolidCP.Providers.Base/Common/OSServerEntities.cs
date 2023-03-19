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
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.OS
{
    public class TerminalSession
    {
        private int sessionId;
        private string username;
        private string status;

        public int SessionId
        {
            get { return this.sessionId; }
            set { this.sessionId = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public string Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
    }

    public class OSProcess
    {
        private int pid;
        private string name;
        private string username;
        private long cpuUsage;
        private long memUsage;

        public int Pid
        {
            get { return this.pid; }
            set { this.pid = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public long CpuUsage
        {
            get { return this.cpuUsage; }
            set { this.cpuUsage = value; }
        }

        public long MemUsage
        {
            get { return this.memUsage; }
            set { this.memUsage = value; }
        }
    }

    public class OSService
    {
        private string id;
        private string name;
        private OSServiceStatus status;
        private bool canStop;
        private bool canPauseAndContinue;

        public OSService()
        {
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public OSServiceStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        public bool CanStop
        {
            get { return this.canStop; }
            set { this.canStop = value; }
        }

        public bool CanPauseAndContinue
        {
            get { return this.canPauseAndContinue; }
            set { this.canPauseAndContinue = value; }
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
    }

    public enum OSServiceStatus
    {
        ContinuePending = 1,
        Paused = 2,
        PausePending = 3,
        Running = 4,
        StartPending = 5,
        Stopped = 6,
        StopPending = 7
    }

    public class SystemLogEntriesPaged
    {
        private int count;
        private SystemLogEntry[] entries;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public SystemLogEntry[] Entries
        {
            get { return entries; }
            set { entries = value; }
        }
    }

    public class SystemLogEntry
    {
        private SystemLogEntryType entryType;
        private DateTime created;
        private string source;
        private string category;
        private long eventId;
        private string userName;
        private string machineName;
        private string message;

        public SystemLogEntryType EntryType
        {
            get { return entryType; }
            set { entryType = value; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string MachineName
        {
            get { return machineName; }
            set { machineName = value; }
        }

        public long EventID
        {
            get { return eventId; }
            set { eventId = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

    public enum SystemLogEntryType
    {
        Information,
        Warning,
        Error,
        SuccessAudit,
        FailureAudit
    }

    public class Memory
    {
        public ulong FreePhysicalMemoryKB { get; set; }
        public ulong FreeVirtualMemoryKB { get; set; }
        public ulong TotalVirtualMemorySizeKB { get; set; }
        public ulong TotalVisibleMemorySizeKB { get; set; }
    }
}
