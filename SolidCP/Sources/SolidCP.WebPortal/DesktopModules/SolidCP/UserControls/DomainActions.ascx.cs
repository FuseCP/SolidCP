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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Web.Services3.Referral;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Portal.UserControls;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal
{
    public enum DomainActionTypes
    {
        None = 0,
        EnableDns = 1,
        DisableDns = 2,
        CreateInstantAlias = 3,
        DeleteInstantAlias = 4,
    }

    public partial class DomainActions : ActionListControlBase<DomainActionTypes>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Remove DNS items if current Hosting plan does not allow it
            if (!PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId).Groups.ContainsKey(ResourceGroups.Dns))
            {
                RemoveActionItem(DomainActionTypes.EnableDns);
                RemoveActionItem(DomainActionTypes.DisableDns);
            }

            // Remove Instant Alias items if current Hosting plan does not allow it
            PackageSettings packageSettings = ES.Services.Packages.GetPackageSettings(PanelSecurity.PackageId, PackageSettings.INSTANT_ALIAS);
            if (packageSettings == null || String.IsNullOrEmpty(packageSettings["InstantAlias"]))
            {
                RemoveActionItem(DomainActionTypes.CreateInstantAlias);
                RemoveActionItem(DomainActionTypes.DeleteInstantAlias);
            }

            // hide control if no actions allowed
            if (ActionsList.Items.Count <= 1)
            {
                Visible = false;
            }
        }

        protected override DropDownList ActionsList
        {
            get { return ddlDomainActions; }
        }

        protected override int DoAction(List<int> ids)
        {
            switch (SelectedAction)
            {
                case DomainActionTypes.EnableDns:
                    return EnableDns(true, ids);
                case DomainActionTypes.DisableDns:
                    return EnableDns(false, ids);
                case DomainActionTypes.CreateInstantAlias:
                    return CreateInstantAlias(true, ids);
                case DomainActionTypes.DeleteInstantAlias:
                    return CreateInstantAlias(false, ids);
            }

            return 0;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            switch (SelectedAction)
            {
                case DomainActionTypes.EnableDns:
                case DomainActionTypes.DisableDns:
                case DomainActionTypes.CreateInstantAlias:
                case DomainActionTypes.DeleteInstantAlias:
                    FireExecuteAction();
                    break;
            }
        }

        private int EnableDns(bool enable, List<int> ids)
        {
            foreach (var id in ids)
            {
                // load domain
                DomainInfo domain = ES.Services.Servers.GetDomain(id);
                if (domain == null)
                    continue;

                // load package context
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(domain.PackageId);
                bool dnsEnabled = cntx.Groups.ContainsKey(ResourceGroups.Dns);
                if (!dnsEnabled)
                    continue;

                int result;
                
                if (enable)
                    result = ES.Services.Servers.EnableDomainDns(id);
                else 
                    result = ES.Services.Servers.DisableDomainDns(id);


                if (result < 0)
                    return result;
            }

            return 0;
        }

        private int CreateInstantAlias(bool enable, List<int> ids)
        {
            foreach (var id in ids)
            {
                 // load domain
                DomainInfo domain = ES.Services.Servers.GetDomain(id);
                if (domain == null)
                    continue;

                // instant alias
                bool instantAliasAllowed = !String.IsNullOrEmpty(domain.InstantAliasName);
                if (!instantAliasAllowed || domain.IsDomainPointer || domain.IsInstantAlias)
                    continue;

                int result;

                if (enable)
                    result = ES.Services.Servers.CreateDomainInstantAlias("", id);
                else
                    result = ES.Services.Servers.DeleteDomainInstantAlias(id);

                if (result < 0)
                    return result;
            }

            return 0;
        }
    }
}
