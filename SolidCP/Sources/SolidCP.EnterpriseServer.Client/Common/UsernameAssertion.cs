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
using System.Data;
using System.Configuration;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

using WSE = Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;

namespace SolidCP.EnterpriseServer
{
    public class UsernameAssertion : SecurityPolicyAssertion
    {
        #region Public properties

        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; }
        }


        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        #endregion

        public UsernameAssertion()
        {
        }

        public UsernameAssertion(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public override SoapFilter CreateServiceInputFilter(FilterCreationContext context)
        {
            return null;
        }

        public override SoapFilter CreateServiceOutputFilter(FilterCreationContext context)
        {
            return null;
        }

        public override SoapFilter CreateClientInputFilter(FilterCreationContext context)
        {
            return null;
        }

        public override SoapFilter CreateClientOutputFilter(FilterCreationContext context)
        {
            return new ClientOutputFilter(this, context);
        }

        #region ClientOutputFilter
        public class ClientOutputFilter : SendSecurityFilter
        {
            UsernameAssertion parentAssertion;
            FilterCreationContext filterContext;

            public ClientOutputFilter(UsernameAssertion parentAssertion, FilterCreationContext filterContext)
                : base(parentAssertion.ServiceActor, false, parentAssertion.ClientActor)
            {
                this.parentAssertion = parentAssertion;
                this.filterContext = filterContext;
            }

            public override void SecureMessage(SoapEnvelope envelope, WSE.Security security)
            {
                // create username token
                UsernameToken userToken = new UsernameToken(parentAssertion.Username, parentAssertion.Password,
                            PasswordOption.SendNone);

                // Add the token to the SOAP header.
                security.Tokens.Add(userToken);

                // Sign the SOAP message by using the UsernameToken.
                MessageSignature sig = new MessageSignature(userToken);
                security.Elements.Add(sig);

                // Encrypt SOAP message
                EncryptedData data = new EncryptedData(userToken);
                security.Elements.Add(data);
            }
        }
        #endregion
    }
}
