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
using System.Web;

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

        public UsernameAssertion()
        {
        }

        public UsernameAssertion(int serverId, string password)
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
                if (extensions[extName] == typeof(UsernameAssertion))
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
            UsernameAssertion parentAssertion;
            FilterCreationContext filterContext;

            public ServiceInputFilter(UsernameAssertion parentAssertion, FilterCreationContext filterContext)
                : base(parentAssertion.ServiceActor, false, parentAssertion.ClientActor)
            {
                this.parentAssertion = parentAssertion;
                this.filterContext = filterContext;
            }

            public override void ValidateMessageSecurity(SoapEnvelope envelope, WSE.Security security)
            {
                if (security != null)
                    ProcessWSERequest(envelope, security);
                //else if (envelope.Header != null)
                //    ProcessSoapRequest(envelope);
                else// if (HttpContext.Current.Request.Headers["Authorization"] != null)
                    ProcessBasicAuthRequest();
            }

            private void ProcessBasicAuthRequest()
            {
                string authStr = HttpContext.Current.Request.Headers["Authorization"];

                if (authStr == null || authStr.Length == 0)
                {
                    // No credentials; anonymous request
                    DenyAccess();
                    return;
                }

                authStr = authStr.Trim();
                if (authStr.IndexOf("Basic", 0) != 0)
                {
                    // Don't understand this header...we'll pass it along and 
                    // assume someone else will handle it
                    DenyAccess();
                    return;
                }

                string encodedCredentials = authStr.Substring(6);

                byte[] decodedBytes = Convert.FromBase64String(encodedCredentials);
                string s = new ASCIIEncoding().GetString(decodedBytes);

                string[] userPass = s.Split(new char[] { ':' });
                string username = userPass[0];
                string password = userPass[1];

                UserInfo user = UserController.GetUserByUsernamePassword(
                                    username, password, System.Web.HttpContext.Current.Request.UserHostAddress);

                if (user == null)
                {
                    // Invalid credentials; deny access
                    DenyAccess();
                    return;

                    //throw new Exception("Wrong BASIC credentials have been supplied");
                }

                SecurityContext.SetThreadPrincipal(user);
            }

            private void DenyAccess()
            {
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.StatusCode = 401;
                response.StatusDescription = "Access Denied";
                response.Write("401 Access Denied");
                string realm = "SolidCP Enterprise Server";
                string val = String.Format("Basic Realm=\"{0}\"", realm);
                response.AppendHeader("WWW-Authenticate", val);
                response.End();
            }

            private void ProcessSoapRequest(SoapEnvelope envelope)
            {
                XmlNode authNode = envelope.Header.SelectSingleNode("Authentication");

                if (authNode == null)
                    throw new Exception("Couldn't find authentication token specified");

                XmlNode userNode = authNode.SelectSingleNode("Username");
                XmlNode passwordNode = authNode.SelectSingleNode("Password");

                if (userNode == null || passwordNode == null)
                    throw new Exception("Authentication token is invalid or broken");

                UserInfo user = UserController.GetUserByUsernamePassword(
                    userNode.InnerText,
                    passwordNode.InnerText,
                    System.Web.HttpContext.Current.Request.UserHostAddress
                );

                if (user == null)
                    throw new Exception("Authentication token is invalid or broken");

                SecurityContext.SetThreadPrincipal(user);
            }

            private void ProcessWSERequest(SoapEnvelope envelope, WSE.Security security)
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
                // get server password from database 
                string password = parentAssertion.Password;

                if (password == null)
                    return;

                // hash password
                password = CryptoUtils.SHA1(password);

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
                        if (child != null && child.NamespaceURI == "http://com/SolidCP/server/")
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
        }
        #endregion
    }
}
