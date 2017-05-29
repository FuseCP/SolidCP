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
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.WebPortal
{
    public class PageModule
    {
        private int moduleId;
        private string title;
        private string moduleDefinitionID;
        private string iconFile;
        private string containerSrc;
		private string adminContainerSrc;
        private List<string> viewRoles = new List<string>();
        private List<string> editRoles = new List<string>();
        private List<string> readOnlyRoles = new List<string>();
        private Hashtable settings = new Hashtable();
		private XmlDocument xmlModuleData = new XmlDocument();
        private PortalPage page;

        public string ModuleDefinitionID
        {
            get { return this.moduleDefinitionID; }
            set { this.moduleDefinitionID = value; }
        }

        public string IconFile
        {
            get { return this.iconFile; }
            set { this.iconFile = value; }
        }

        public string ContainerSrc
        {
            get { return this.containerSrc; }
            set { this.containerSrc = value; }
        }

		public string AdminContainerSrc
		{
			get { return this.adminContainerSrc; }
			set { this.adminContainerSrc = value; }
		}

        public List<string> ViewRoles
        {
            get { return this.viewRoles; }
        }

        public List<string> EditRoles
        {
            get { return this.editRoles; }
        }

        public List<string> ReadOnlyRoles
        {
            get { return this.readOnlyRoles; }
        }


        public Hashtable Settings
        {
            get { return this.settings; }
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public int ModuleId
        {
            get { return this.moduleId; }
            set { this.moduleId = value; }
        }

        public PortalPage Page
        {
            get { return this.page; }
            set { this.page = value; }
        }

		public XmlNodeList SelectNodes(string xpath)
		{
            if(xmlModuleData.DocumentElement == null)
                return null;

			return xmlModuleData.DocumentElement.SelectNodes(xpath);
		}

		public void LoadXmlModuleData(string xml)
		{
			try
			{
				xmlModuleData.LoadXml(xml);
			}
			catch
			{
			}
		}
    }
}
