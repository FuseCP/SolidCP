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
using System.Configuration;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Web;

using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;
using WebSecurity=Microsoft.Web.Services3.Security.Security;

namespace SolidCP.Server
{
    public class UsernameAssertion : SecurityPolicyAssertion
    {
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
            // this assertion is intended to work only on service side
            return null;
        }

        public override SoapFilter CreateClientOutputFilter(FilterCreationContext context)
        {
            // this assertion is intended to work only on service side
            return null;
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

            public override void ValidateMessageSecurity(SoapEnvelope envelope, WebSecurity security)
            {
                if (!ServerConfiguration.Security.SecurityEnabled)
                    return;

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
                    throw new SecurityFault("Message did not meet security requirements.");
            }

            private bool CheckSignature(SoapEnvelope envelope, WebSecurity security, MessageSignature signature)
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
    }
}
