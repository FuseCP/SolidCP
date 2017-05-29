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

    internal class SessionElement : ConfigurationElement
    {
        private ConfigurationMethod _terminateMethod;

        public void Terminate()
        {
            if (this._terminateMethod == null)
            {
                this._terminateMethod = base.Methods["Terminate"];
            }
            this._terminateMethod.CreateInstance().Execute();
        }

        public long BytesReceived
        {
            get
            {
                return (long) base["bytesReceived"];
            }
            set
            {
                base["bytesReceived"] = value;
            }
        }

        public long BytesSent
        {
            get
            {
                return (long) base["bytesSent"];
            }
            set
            {
                base["bytesSent"] = value;
            }
        }

        public string CommandStartTime
        {
            get
            {
                return (string) base["commandStartTime"];
            }
            set
            {
                base["commandStartTime"] = value;
            }
        }

        public string CurrentCommand
        {
            get
            {
                return (string) base["currentCommand"];
            }
            set
            {
                base["currentCommand"] = value;
            }
        }

        public long LastErrorStatus
        {
            get
            {
                return (long) base["lastErrorStatus"];
            }
            set
            {
                base["lastErrorStatus"] = value;
            }
        }

        public string PreviousCommand
        {
            get
            {
                return (string) base["previousCommand"];
            }
            set
            {
                base["previousCommand"] = value;
            }
        }

        public long SessionId
        {
            get
            {
                return (long) base["sessionId"];
            }
            set
            {
                base["sessionId"] = value;
            }
        }

        public string SessionStartTime
        {
            get
            {
                return (string) base["sessionStartTime"];
            }
            set
            {
                base["sessionStartTime"] = value;
            }
        }

        public string UserName
        {
            get
            {
                return (string) base["userName"];
            }
            set
            {
                base["userName"] = value;
            }
        }
    }
}

