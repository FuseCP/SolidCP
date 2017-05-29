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

namespace SolidCP.Providers.FTP.IIs70.Config
{
    using Microsoft.Web.Administration;
	using System;
	using System.Runtime.InteropServices;
	using System.Threading;

    internal class FtpSite : ConfigurationElement
    {
        private ConnectionsElement _connections;
        private DataChannelSecurityElement _dataChannelSecurity;
        private DirectoryBrowseElement _directoryBrowse;
        private FileHandlingElement _fileHandling;
        private FirewallElement _firewall;
        private ConfigurationMethod _flushLogMethod;
		private LogFileElement _logFile;
        private MessagesElement _messages;
        private SecurityElement _security;
        private SessionCollection _sessions;
        private ConfigurationMethod _startMethod;
        private ConfigurationMethod _stopMethod;
        private UserIsolationElement _userIsolation;
		public const int E_NOT_FOUND = -2147023728;
		public const int E_OBJECT_NOT_EXIST = -2147020584;
		private const uint ERROR_ALREADY_EXISTS = 2147942583;
		private string siteServiceId;

		public string SiteServiceId
		{
			get { return siteServiceId; }
			set { siteServiceId = value; }
		}

        public void FlushLog()
        {
            if (this._flushLogMethod == null)
            {
                this._flushLogMethod = base.Methods["FlushLog"];
            }
            this._flushLogMethod.CreateInstance().Execute();
        }

        public void Start()
        {
            if (this._startMethod == null)
            {
                this._startMethod = base.Methods["Start"];
            }
            this._startMethod.CreateInstance().Execute();
        }

        public void Stop()
        {
            if (this._stopMethod == null)
            {
                this._stopMethod = base.Methods["Stop"];
            }
            this._stopMethod.CreateInstance().Execute();
        }

        public bool AllowUTF8
        {
            get
            {
                return (bool) base["allowUTF8"];
            }
            set
            {
                base["allowUTF8"] = value;
            }
        }

        public ConnectionsElement Connections
        {
            get
            {
                if (this._connections == null)
                {
                    this._connections = (ConnectionsElement) base.GetChildElement("connections", typeof(ConnectionsElement));
                }
                return this._connections;
            }
        }

        public DataChannelSecurityElement DataChannelSecurity
        {
            get
            {
                if (this._dataChannelSecurity == null)
                {
                    this._dataChannelSecurity = (DataChannelSecurityElement) base.GetChildElement("dataChannelSecurity", typeof(DataChannelSecurityElement));
                }
                return this._dataChannelSecurity;
            }
        }

        public DirectoryBrowseElement DirectoryBrowse
        {
            get
            {
                if (this._directoryBrowse == null)
                {
                    this._directoryBrowse = (DirectoryBrowseElement) base.GetChildElement("directoryBrowse", typeof(DirectoryBrowseElement));
                }
                return this._directoryBrowse;
            }
        }

        public FileHandlingElement FileHandling
        {
            get
            {
                if (this._fileHandling == null)
                {
                    this._fileHandling = (FileHandlingElement) base.GetChildElement("fileHandling", typeof(FileHandlingElement));
                }
                return this._fileHandling;
            }
        }

		public FirewallElement FirewallSupport
        {
            get
            {
                if (this._firewall == null)
                {
					this._firewall = (FirewallElement)base.GetChildElement("firewallSupport", typeof(FirewallElement));
                }
                return this._firewall;
            }
        }

        public uint LastStartupStatus
        {
            get
            {
                return (uint) base["lastStartupStatus"];
            }
            set
            {
                base["lastStartupStatus"] = value;
            }
        }

		public LogFileElement LogFile
		{
			get
			{
				if (this._logFile == null)
				{
					this._logFile = (LogFileElement)base.GetChildElement("logFile", typeof(LogFileElement));
				}
				return this._logFile;
			}
		}

        public MessagesElement Messages
        {
            get
            {
                if (this._messages == null)
                {
                    this._messages = (MessagesElement) base.GetChildElement("messages", typeof(MessagesElement));
                }
                return this._messages;
            }
        }

        public SecurityElement Security
        {
            get
            {
                if (this._security == null)
                {
                    this._security = (SecurityElement) base.GetChildElement("security", typeof(SecurityElement));
                }
                return this._security;
            }
        }

        public bool ServerAutoStart
        {
            get
            {
                return (bool) base["serverAutoStart"];
            }
            set
            {
                base["serverAutoStart"] = value;
            }
        }

        public SessionCollection Sessions
        {
            get
            {
                if (this._sessions == null)
                {
                    this._sessions = (SessionCollection) base.GetCollection("sessions", typeof(SessionCollection));
                }
                return this._sessions;
            }
        }

        public SiteState State
        {
            get
            {
				SiteState unknown = SiteState.Unknown;
				int num = 0;
				bool flag = false;
				while (!flag && (++num < 10))
				{
					try
					{
						unknown = (SiteState)base["state"];
						flag = true;
						continue;
					}
					catch (COMException exception)
					{
						if (exception.ErrorCode != -2147020584)
						{
							return unknown;
						}
						Thread.Sleep(100);
						continue;
					}
				}
				return unknown;
            }
            set
            {
                base["state"] = (int) value;
            }
        }

        public UserIsolationElement UserIsolation
        {
            get
            {
                if (this._userIsolation == null)
                {
                    this._userIsolation = (UserIsolationElement) base.GetChildElement("userIsolation", typeof(UserIsolationElement));
                }
                return this._userIsolation;
            }
        }
    }
}

