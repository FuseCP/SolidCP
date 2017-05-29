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

namespace SolidCP.EnterpriseServer
{
    public class UsernamePolicy
    {
        bool enabled = false;
        string allowedSymbols = "a-zA-Z0-9\\.\\_";
        int minLength = -1;
        int maxLength = -1;
        string prefix = null;
        string suffix = null;

        public UsernamePolicy(string policyValue)
        {
            if (String.IsNullOrEmpty(policyValue))
                return;

            try
            {
                // parse settings
                string[] parts = policyValue.Split(';');

                enabled = Boolean.Parse(parts[0]);
                allowedSymbols += parts[1];
                minLength = Int32.Parse(parts[2]);
                maxLength = Int32.Parse(parts[3]);
                prefix = parts[4];
                suffix = parts[5];
            }
            catch { /* skip */ }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        public string AllowedSymbols
        {
            get { return this.allowedSymbols; }
            set { this.allowedSymbols = value; }
        }

        public int MinLength
        {
            get { return this.minLength; }
            set { this.minLength = value; }
        }

        public int MaxLength
        {
            get { return this.maxLength; }
            set { this.maxLength = value; }
        }

        public string Prefix
        {
            get { return this.prefix; }
            set { this.prefix = value; }
        }

        public string Suffix
        {
            get { return this.suffix; }
            set { this.suffix = value; }
        }
    }
}
