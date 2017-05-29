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
using System.Security;
using System.Security.Principal;

namespace SolidCP.EnterpriseServer
{
    public class EnterpriseServerPrincipal : IPrincipal
    {
        private int userId;
        private int ownerId;
        private bool isPeer;
        private bool isDemo;
        private UserStatus status;

        private List<string> roles = new List<string>();
        private IIdentity identity;

        public EnterpriseServerPrincipal(IIdentity identity, string[] roles)
        {
            this.identity = identity;
            this.roles.AddRange(roles);
        }

        #region IPrincipal Members

        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string role)
        {
            return roles.Contains(role);
        }

        #endregion

        #region Public properties
        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        public int OwnerId
        {
            get { return this.ownerId; }
            set { this.ownerId = value; }
        }

        public bool IsPeer
        {
            get { return this.isPeer; }
            set { this.isPeer = value; }
        }

        public bool IsDemo
        {
            get { return this.isDemo; }
            set { this.isDemo = value; }
        }

        public UserStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }
        #endregion
    }
}
