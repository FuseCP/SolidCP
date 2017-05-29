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

namespace SolidCP.Server.Client
{
    public class ServerUsernameAssertion : SecurityPolicyAssertion
    {
        #region Public properties
        private bool signRequest = true;
        public bool SignRequest
        {
            get { return signRequest; }
            set { signRequest = value; }
        }

        private bool encryptRequest = true;
        public bool EncryptRequest
        {
            get { return encryptRequest; }
            set { encryptRequest = value; }
        }

        private int serverId = 0;
        public int ServerId
        {
            get { return serverId; }
            set { serverId = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        #endregion

        public ServerUsernameAssertion()
        {
        }

        public ServerUsernameAssertion(int serverId, string password)
        {
            this.serverId = serverId;
            this.password = password;
        }

        public override SoapFilter CreateServiceInputFilter(FilterCreationContext context)
        {
            return new ServiceInputFilter(this, context);
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

        public override void ReadXml(XmlReader reader, IDictionary<string, Type> extensions)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            if (extensions == null)
                throw new ArgumentNullException("extensions");

            // find the current extension
            string tagName = null;
            foreach (string extName in extensions.Keys)
            {
                if (extensions[extName] == typeof(ServerUsernameAssertion))
                {
                    tagName = extName;
                    break;
                }
            }

            // read the first element (maybe empty)
            reader.ReadStartElement(tagName);
        }

        public override void WriteXml(XmlWriter writer)
        {
            // Typically this is not needed for custom policies
        }

        #region ServiceInputFilter
        public class ServiceInputFilter : ReceiveSecurityFilter
        {
            ServerUsernameAssertion parentAssertion;
            FilterCreationContext filterContext;

            public ServiceInputFilter(ServerUsernameAssertion parentAssertion, FilterCreationContext filterContext)
                : base(parentAssertion.ServiceActor, false, parentAssertion.ClientActor)
            {
                this.parentAssertion = parentAssertion;
                this.filterContext = filterContext;
            }

            public override void ValidateMessageSecurity(SoapEnvelope envelope, WSE.Security security)
            {
                // by default we consider that SOAP messages is not signed
                bool IsSigned = false;

                // if security element is null
                // the call is made not from WSE-enabled client
                if (security != null)
                {
                    foreach (ISecurityElement element in security.Elements)
                    {
                        if (element is MessageSignature)
                        {
                            // The given context contains a Signature element.
                            MessageSignature sign = element as MessageSignature;

                            if (CheckSignature(envelope, security, sign))
                            {
                                // The SOAP message is signed.
                                if (sign.SigningToken is UsernameToken)
                                {
                                    UsernameToken token = sign.SigningToken as UsernameToken;

                                    // The SOAP message is signed 
                                    // with a UsernameToken.
                                    IsSigned = true;
                                }
                            }
                        }
                    }
                }

                // throw an exception if the message did not pass all the tests
                if (!IsSigned)
                    throw new SecurityFault("SOAP response should be signed.");

                // check encryption
                bool IsEncrypted = false;
                foreach (ISecurityElement element in security.Elements)
                {
                    if (element is EncryptedData)
                    {
                        EncryptedData encryptedData = element as EncryptedData;
                        System.Xml.XmlElement targetElement = encryptedData.TargetElement;

                        if (SoapHelper.IsBodyElement(targetElement))
                        {
                            // The given SOAP message has the Body element Encrypted.
                            IsEncrypted = true;
                        }
                    }
                }

                if (!IsEncrypted)
                    throw new SecurityFault("SOAP response should be encrypted.");

            }

            private bool CheckSignature(SoapEnvelope envelope, WSE.Security security, MessageSignature signature)
            {
                //
                // Now verify which parts of the message were actually signed.
                //
                SignatureOptions actualOptions = signature.SignatureOptions;
                SignatureOptions expectedOptions = SignatureOptions.IncludeSoapBody;

                if (security != null && security.Timestamp != null)
                    expectedOptions |= SignatureOptions.IncludeTimestamp;

                //
                // The <Action> and <To> are required addressing elements.
                //
                expectedOptions |= SignatureOptions.IncludeAction;
                expectedOptions |= SignatureOptions.IncludeTo;

                if (envelope.Context.Addressing.FaultTo != null && envelope.Context.Addressing.FaultTo.TargetElement != null)
                    expectedOptions |= SignatureOptions.IncludeFaultTo;

                if (envelope.Context.Addressing.From != null && envelope.Context.Addressing.From.TargetElement != null)
                    expectedOptions |= SignatureOptions.IncludeFrom;

                if (envelope.Context.Addressing.MessageID != null && envelope.Context.Addressing.MessageID.TargetElement != null)
                    expectedOptions |= SignatureOptions.IncludeMessageId;

                if (envelope.Context.Addressing.RelatesTo != null && envelope.Context.Addressing.RelatesTo.TargetElement != null)
                    expectedOptions |= SignatureOptions.IncludeRelatesTo;

                if (envelope.Context.Addressing.ReplyTo != null && envelope.Context.Addressing.ReplyTo.TargetElement != null)
                    expectedOptions |= SignatureOptions.IncludeReplyTo;
                //
                // Check if the all the expected options are the present.
                //
                return ((expectedOptions & actualOptions) == expectedOptions);

            }
        }
        #endregion

        #region ClientOutputFilter
        public class ClientOutputFilter : SendSecurityFilter
        {
            ServerUsernameAssertion parentAssertion;
            FilterCreationContext filterContext;

            public ClientOutputFilter(ServerUsernameAssertion parentAssertion, FilterCreationContext filterContext)
                : base(parentAssertion.ServiceActor, false, parentAssertion.ClientActor)
            {
                this.parentAssertion = parentAssertion;
                this.filterContext = filterContext;
            }

            public override void SecureMessage(SoapEnvelope envelope, WSE.Security security)
            {
                // get server password from database 
                string password = parentAssertion.Password;

                if (password == null)
                    return;

                // hash password
                password = SHA1(password);

                // create username token
                UsernameToken userToken = new UsernameToken(parentAssertion.ServerId.ToString(), password,
                            PasswordOption.SendNone);

                if (parentAssertion.signRequest || parentAssertion.encryptRequest)
                {
                    // Add the token to the SOAP header.
                    security.Tokens.Add(userToken);
                }

                if (parentAssertion.signRequest)
                {
                    // Sign the SOAP message by using the UsernameToken.
                    MessageSignature sig = new MessageSignature(userToken);
                    security.Elements.Add(sig);
                }

                if (parentAssertion.encryptRequest)
                {
                    // we don't return any custom SOAP headers
                    // so, just encrypt a message Body
                    EncryptedData data = new EncryptedData(userToken);

                    // encrypt custom headers
                    for (int index = 0; index < envelope.Header.ChildNodes.Count; index++)
                    {
                        XmlElement child = envelope.Header.ChildNodes[index] as XmlElement;

                        // find all SecureSoapHeader headers marked with a special attribute
                        if (child != null && child.NamespaceURI == "http://smbsaas/solidcp/server/")
                        {
                            // create ID attribute for referencing purposes
                            string id = Guid.NewGuid().ToString();
                            child.SetAttribute("Id", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", id);

                            // Create an encryption reference for the custom SOAP header.
                            data.AddReference(new EncryptionReference("#" + id));
                        }
                    }

                    security.Elements.Add(data);
                }
            }

            private string SHA1(string plainText)
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                System.Security.Cryptography.HashAlgorithm hash = new System.Security.Cryptography.SHA1Managed(); ;
                byte[] hashBytes = hash.ComputeHash(plainTextBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
        #endregion
    }
}
