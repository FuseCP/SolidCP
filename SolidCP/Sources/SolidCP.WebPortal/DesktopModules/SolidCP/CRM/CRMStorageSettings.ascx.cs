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
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.Portal
{
    public partial class CRMStorageSettings : SolidCPModuleBase
    {
        public string SizeValueToString(long val)
        {
            return (val == -1) ? GetSharedLocalizedString("Text.Unlimited") : val.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            warningValue.UnlimitedText = GetLocalizedString("WarningUnlimitedValue");
            if (!IsPostBack)
            {
                Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
                if (org.CrmOrganizationId == Guid.Empty)
                {
                    messageBox.ShowErrorMessage("NOT_CRM_ORGANIZATION");
                    StorageLimits.Enabled = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    BindValues();
                }
            }
        }

        private void BindValues()
        {
            Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

            string quotaName = "";
            if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013)) quotaName = Quotas.CRM2013_MAXDATABASESIZE;
            else if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM)) quotaName = Quotas.CRM_MAXDATABASESIZE;

            int limitDBSize = cntx.Quotas[quotaName].QuotaAllocatedValue;
            //maxStorageSettingsValue.ParentQuotaValue = limitDBSize;
            maxStorageSettingsValue.ParentQuotaValue = -1;

            long maxDBSize = ES.Services.CRM.GetMaxDBSize(PanelRequest.ItemID, PanelSecurity.PackageId);
            long DBSize = ES.Services.CRM.GetDBSize(PanelRequest.ItemID, PanelSecurity.PackageId);

            DBSize = DBSize > 0 ? DBSize = DBSize / (1024 * 1024) : DBSize;
            maxDBSize = maxDBSize > 0 ? maxDBSize = maxDBSize / (1024 * 1024) : maxDBSize;

            maxStorageSettingsValue.QuotaValue = Convert.ToInt32(maxDBSize);

            lblDBSize.Text = SizeValueToString(DBSize);
            lblMAXDBSize.Text = SizeValueToString(maxDBSize);

            lblLimitDBSize.Text = SizeValueToString(limitDBSize);
        }

        private void Save()
        {            
            try
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                string quotaName = "";
                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM2013)) quotaName = Quotas.CRM2013_MAXDATABASESIZE;
                else if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM)) quotaName = Quotas.CRM_MAXDATABASESIZE;

                long limitSize = cntx.Quotas[quotaName].QuotaAllocatedValue;

                long maxSize = maxStorageSettingsValue.QuotaValue;

                if (limitSize != -1)
                {
                    if (maxSize == -1) maxSize = limitSize;
                    if (maxSize > limitSize) maxSize = limitSize;
                }

                if (maxSize > 0)
                {
                    maxSize = maxSize * 1024 * 1024;
                }

                ES.Services.CRM.SetMaxDBSize(PanelRequest.ItemID, PanelSecurity.PackageId, maxSize);

                messageBox.ShowSuccessMessage("HOSTED_SHAREPOINT_UPDATE_QUOTAS");

                BindValues();
            }
            catch (Exception)
            {
                messageBox.ShowErrorMessage("HOSTED_SHAREPOINT_UPDATE_QUOTAS");
            }

        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();            
        }

    }
}
