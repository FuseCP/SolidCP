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
using System.IO;

using WSE = Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;

namespace SolidCP.EnterpriseServer
{
    class RestrictedTraceAssertion : PolicyAssertion
    {
        string inputfile = "input.xml";
        string outputfile = "output.xml";
        bool bEnabled = false;

        public RestrictedTraceAssertion()
            : base()
        {
        }

        public override SoapFilter CreateClientOutputFilter(FilterCreationContext context)
        {
            return bEnabled ? new CustomTraceFilter(outputfile) : null;
        }

        public override SoapFilter CreateClientInputFilter(FilterCreationContext context)
        {
            return bEnabled ? new CustomTraceFilter(inputfile) : null;
        }

        public override SoapFilter CreateServiceInputFilter(FilterCreationContext context)
        {
            return bEnabled ? new CustomTraceFilter(inputfile) : null;
        }

        public override SoapFilter CreateServiceOutputFilter(FilterCreationContext context)
        {
            return bEnabled ? new CustomTraceFilter(outputfile) : null;
        }

        public override void ReadXml(XmlReader reader, IDictionary<string, Type> extensions)
        {
            bool isEmpty = reader.IsEmptyElement;

            string input = reader.GetAttribute("input");
            string output = reader.GetAttribute("output");
            string enabled = reader.GetAttribute("enabled");
            if ((enabled != null) && (enabled.ToLower() == "true"))
                bEnabled = true;

            if (input != null)
                inputfile = input;

            if (output != null)
                outputfile = output;

            reader.ReadStartElement("restrictedTraceAssertion");
            if (!isEmpty)
                reader.ReadEndElement();
        }

        public override IEnumerable<KeyValuePair<string, Type>> GetExtensions()
        {
            return new KeyValuePair<string, Type>[] { new KeyValuePair<string, Type>("RestrictedTraceAssertion", this.GetType()) };
        }
    }

    class CustomTraceFilter : SoapFilter
    {
        string filename = null;

        public CustomTraceFilter(String file)
            : base()
        {
            filename = file;
        }

        void stripContent(XmlElement el, String tagName)
        {
            XmlNodeList passwords = el.GetElementsByTagName(tagName);
            for (int i = 0; i < passwords.Count; ++i)
            {
                XmlNode node = passwords.Item(i);
                node.InnerXml = "*****";
            }
        }
        public override SoapFilterResult ProcessMessage(SoapEnvelope envelope)
        {
            XmlDocument dom = null;
            DateTime timeStamp = DateTime.Now;
            XmlNode rootNode = null;

            dom = new XmlDocument();

            if (!File.Exists(filename))
            {
                XmlDeclaration xmlDecl = dom.CreateXmlDeclaration("1.0", "utf-8", null);

                dom.InsertBefore(xmlDecl, dom.DocumentElement);

                rootNode = dom.CreateNode(XmlNodeType.Element, "log", String.Empty);
                dom.AppendChild(rootNode);
                dom.Save(filename);
            }
            else
            {
                dom.Load(filename);
                rootNode = dom.DocumentElement;
            }

            XmlNode newNode = dom.ImportNode(envelope.DocumentElement, true);
            XmlElement el = newNode as XmlElement;
            stripContent(el, "password");
            stripContent(el, "Password");
            stripContent(el, "AnonymousUserPassword");
/*            XmlNodeList passwords = (newNode as XmlElement).GetElementsByTagName("password");
            for (int i = 0; i < passwords.Count; ++i)
            {
                XmlNode node = passwords.Item(i);
                node.InnerXml = "*****";
            } */

            rootNode.AppendChild(newNode);

            dom.Save(filename);

            return SoapFilterResult.Continue;
        }
    }
}
