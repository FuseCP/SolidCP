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
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.Web;

namespace SolidCP.Portal
{
    public partial class DomainsSelectDomainControl : SolidCPControlBase
    {
        public bool HideIdnDomains
        {
            get { return (ViewState["HideIdnDomains"] != null) && (bool)ViewState["HideIdnDomains"]; }
            set { ViewState["HideIdnDomains"] = value; }
        }

        public bool HideWebSites
        {
            get { return (ViewState["HideWebSites"] != null) ? (bool)ViewState["HideWebSites"] : false; }
            set { ViewState["HideWebSites"] = value; }
        }

        public bool HideInstantAlias
        {
            get { return (ViewState["HideInstantAlias"] != null) ? (bool)ViewState["HideInstantAlias"] : false; }
            set { ViewState["HideInstantAlias"] = value; }
        }

        public bool HideMailDomains
        {
            get { return (ViewState["HideMailDomains"] != null) ? (bool)ViewState["HideMailDomains"] : false; }
            set { ViewState["HideMailDomains"] = value; }
        }

        public bool HideMailDomainPointers
        {
            get { return (ViewState["HideMailDomainPointers"] != null) ? (bool)ViewState["HideMailDomainPointers"] : false; }
            set { ViewState["HideMailDomainPointers"] = value; }
        }


        public bool HideDomainPointers
        {
            get { return (ViewState["HideDomainPointers"] != null) ? (bool)ViewState["HideDomainPointers"] : false; }
            set { ViewState["HideDomainPointers"] = value; }
        }

        public bool HideDomainsSubDomains
        {
            get { return (ViewState["HideDomainsSubDomains"] != null) ? (bool)ViewState["HideDomainsSubDomains"] : false; }
            set { ViewState["HideDomainsSubDomains"] = value; }
        }

        public int PackageId
        {
            get { return (ViewState["PackageId"] != null) ? (int)ViewState["PackageId"] : 0; }
            set { ViewState["PackageId"] = value; }
        }

        public int DomainId
        {
            get
            {
                return Utils.ParseInt(ddlDomains.SelectedValue, 0);
            }
        }

        public string DomainName
        {
            get
            {
                return ddlDomains.SelectedItem.Text.ToLower();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDomains();
            }

        }

        private void BindDomains()
        {
            DomainInfo[] domains = ES.Services.Servers.GetMyDomains(PackageId);

            if (HideIdnDomains)
            {
                domains = domains.Where(d => !Utils.IsIdnDomain(d.DomainName)).ToArray();
            }

            WebSite[] sites = null;
            Hashtable htSites = new Hashtable();
            Hashtable htMailDomainPointers = new Hashtable();
            if (HideWebSites)
            {
                sites = ES.Services.WebServers.GetWebSites(PackageId, false);

                foreach (WebSite w in sites)
                {
                    if (htSites[w.Name.ToLower()] == null) htSites.Add(w.Name.ToLower(), 1);

                    DomainInfo[] pointers = ES.Services.WebServers.GetWebSitePointers(w.Id);
                    foreach (DomainInfo p in pointers)
                    {
                        if (htSites[p.DomainName.ToLower()] == null) htSites.Add(p.DomainName.ToLower(), 1);
                    }
                }
            }

            if (HideMailDomainPointers)
            {
                Providers.Mail.MailDomain[] mailDomains = ES.Services.MailServers.GetMailDomains(PackageId, false);

                foreach (Providers.Mail.MailDomain mailDomain in mailDomains)
                {
                    DomainInfo[] pointers = ES.Services.MailServers.GetMailDomainPointers(mailDomain.Id);
                    if (pointers != null)
                    {
                        foreach (DomainInfo p in pointers)
                        {
                            if (htMailDomainPointers[p.DomainName.ToLower()] == null) htMailDomainPointers.Add(p.DomainName.ToLower(), 1);

                        }
                    }
                }
            }


            ddlDomains.Items.Clear();

            // add "select" item
            ddlDomains.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectDomain"), ""));

            foreach (DomainInfo domain in domains)
            {
                if (HideWebSites)
                {
                    if (domain.WebSiteId > 0)
                    {
                        continue;
                    }
                    else
                    {
                        if (htSites != null)
                        {
                            if (htSites[domain.DomainName.ToLower()] != null) continue;
                        }
                    }
                }


                if (HideMailDomainPointers)
                {
                    if (htMailDomainPointers[domain.DomainName.ToLower()] != null) continue;
                }

                
                if (HideInstantAlias && domain.IsInstantAlias)
                    continue;
                else if (HideMailDomains && domain.MailDomainId > 0)
                    continue;
                else if (HideDomainPointers && (domain.IsDomainPointer))
                    continue;
                else if (HideDomainsSubDomains && !(domain.IsDomainPointer))
                    continue;

                ddlDomains.Items.Add(new ListItem(domain.DomainName.ToLower(), domain.DomainId.ToString()));
            }

            if (Request.Cookies["CreatedDomainId"] != null)
                Utils.SelectListItem(ddlDomains, Request.Cookies["CreatedDomainId"].Value);
        }
    }
}
