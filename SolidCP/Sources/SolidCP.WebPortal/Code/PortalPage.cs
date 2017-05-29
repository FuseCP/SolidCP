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

namespace SolidCP.WebPortal
{
    public class PortalPage
    {
        private string name;
        private bool enabled;
        private bool hidden;
		private string adminSkinSrc;
        private string skinSrc;
        private List<string> roles = new List<string>();
        private List<PortalPage> pages = new List<PortalPage>();
        private PortalPage parentPage;
        private Dictionary<string, ContentPane> contentPanes = new Dictionary<string, ContentPane>();
		private string url;
        private string target;
        private string align;

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public List<string> Roles
        {
            get { return this.roles; }
        }

        public List<PortalPage> Pages
        {
            get { return this.pages; }
        }

        public PortalPage ParentPage
        {
            get { return this.parentPage; }
            set { this.parentPage = value; }
        }

        public string SkinSrc
        {
            get { return this.skinSrc; }
            set { this.skinSrc = value; }
        }

		public string AdminSkinSrc
		{
			get { return this.adminSkinSrc; }
			set { this.adminSkinSrc = value; }
		}

        public Dictionary<string, ContentPane> ContentPanes
        {
            get { return this.contentPanes; }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        public bool Hidden
        {
            get { return this.hidden; }
            set { this.hidden = value; }
        }

		public string Url
		{
			get { return this.url; }
			set { this.url = value; }
		}

        public string Target
        {
            get { return this.target; }
            set { this.target = value; }
        }

        public string Align
        {
            get { return this.align; }
            set { this.align = value; }
        }
    }
}
