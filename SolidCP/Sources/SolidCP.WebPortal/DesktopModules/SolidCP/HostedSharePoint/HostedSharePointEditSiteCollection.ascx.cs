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
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.DNS;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.SharePoint;


namespace SolidCP.Portal
{
    public partial class HostedSharePointEditSiteCollection : SolidCPModuleBase
    {
        SharePointSiteCollection item = null;

        private int OrganizationId
        {
            get
            {
                return PanelRequest.GetInt("ItemID");
            }
        }

        private int SiteCollectionId
        {
            get
            {
                return PanelRequest.GetInt("SiteCollectionID");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            domain.PackageId = PanelSecurity.PackageId;

            warningStorage.UnlimitedText = GetLocalizedString("WarningUnlimitedValue");
            editWarningStorage.UnlimitedText = GetLocalizedString("WarningUnlimitedValue");

            bool newItem = (this.SiteCollectionId == 0);

            tblEditItem.Visible = newItem;
            tblViewItem.Visible = !newItem;

            //btnUpdate.Visible = newItem;
            btnDelete.Visible = !newItem;
            btnUpdate.Text = newItem ? GetLocalizedString("Text.Add") : GetLocalizedString("Text.Update");
            btnUpdate.OnClientClick = newItem ? GetLocalizedString("btnCreate.OnClientClick") : GetLocalizedString("btnUpdate.OnClientClick");

            btnBackup.Enabled = btnRestore.Enabled = !newItem;

            // bind item
            BindItem();

        }

        private void BindItem()
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!this.IsDnsServiceAvailable())
                    {
                        localMessageBox.ShowWarningMessage("HOSTEDSHAREPOINT_NO_DNS");
                    }

                    // load item if required
                    if (this.SiteCollectionId > 0)
                    {
                        // existing item
                        item = ES.Services.HostedSharePointServers.GetSiteCollection(this.SiteCollectionId);
                        if (item != null)
                        {
                            // save package info
                            ViewState["PackageId"] = item.PackageId;
                        }
                        else
                            RedirectToBrowsePage();
                    }
                    else
                    {
                        // new item
                        ViewState["PackageId"] = PanelSecurity.PackageId;
                        if (UseSharedSLL(PanelSecurity.PackageId))
                        {

                            rowUrl.Visible = false;
                            valRequireHostName.Enabled = false;
                            valRequireCorrectHostName.Enabled = false;
                        }
                    }

                    //this.gvUsers.DataBind();

                    List<CultureInfo> cultures = new List<CultureInfo>();
                    foreach (int localeId in ES.Services.HostedSharePointServers.GetSupportedLanguages(PanelSecurity.PackageId))
                    {
                        cultures.Add(new CultureInfo(localeId, false));
                    }

                    this.ddlLocaleID.DataSource = cultures;
                    this.ddlLocaleID.DataBind();
                }

                if (!IsPostBack)
                {
                    // bind item to controls
                    if (item != null)
                    {
                        // bind item to controls
                        lnkUrl.Text = item.PhysicalAddress;
                        lnkUrl.NavigateUrl = item.PhysicalAddress;
                        litSiteCollectionOwner.Text = String.Format("{0} ({1})", item.OwnerName, item.OwnerEmail);
                        litLocaleID.Text = new CultureInfo(item.LocaleId, false).DisplayName;
                        litTitle.Text = item.Title;
                        litDescription.Text = item.Description;
                        editWarningStorage.QuotaValue = (int)item.WarningStorage;
                        editMaxStorage.QuotaValue = (int)item.MaxSiteStorage;
                    }

                    Organization org = ES.Services.Organizations.GetOrganization(OrganizationId);

                    if (org != null)
                    {
                        SetStorageQuotas(org, item);
                    }
                }
                //OrganizationDomainName[] domains = ES.Services.Organizations.GetOrganizationDomains(PanelRequest.ItemID);

                //DomainInfo[] domains = ES.Services.Servers.GetMyDomains(PanelSecurity.PackageId);

                EnterpriseServer.DomainInfo[] domains = ES.Services.Servers.GetDomains(PanelSecurity.PackageId);

                if (domains.Length == 0)
                {
                    localMessageBox.ShowWarningMessage("HOSTEDSHAREPOINT_NO_DOMAINS");
                    DisableFormControls(this, btnCancel);
                    return;
                }
                //if (this.gvUsers.Rows.Count == 0)
                //{
                //    localMessageBox.ShowWarningMessage("HOSTEDSHAREPOINT_NO_USERS");
                //    DisableFormControls(this, btnCancel);
                //    return;
                //}
            }
            catch
            {

                localMessageBox.ShowWarningMessage("INIT_SERVICE_ITEM_FORM");

                DisableFormControls(this, btnCancel);
                return;
            }
        }

        /// <summary> Checks and sets disk quotas values.</summary>
        /// <param name="organization"> The organization.</param>
        /// <param name="collection"> The site collection.</param>
        private void SetStorageQuotas(Organization organization, SharePointSiteCollection collection)
        {
            var quotaValue = organization.MaxSharePointStorage;

            if (quotaValue != -1)
            {
                var spaceResrved = GetReservedDiskStorageSpace();

                if (spaceResrved > quotaValue)
                {
                    quotaValue = 0;
                }
                else
                {
                    quotaValue -= spaceResrved;
                }

                if (collection != null)
                {
                    quotaValue += (int)collection.MaxSiteStorage;
                }
            }
            
            maxStorage.ParentQuotaValue = quotaValue;
            maxStorage.QuotaValue = quotaValue;
            editMaxStorage.ParentQuotaValue = quotaValue;
            warningStorage.ParentQuotaValue = quotaValue;
            warningStorage.QuotaValue = quotaValue;
            editWarningStorage.ParentQuotaValue = quotaValue;

            btnUpdate.Enabled = quotaValue != 0;
        }

        /// <summary> Gets disk space reserved by existing site collections.</summary>
        /// <returns> Reserved disk space vallue.</returns>
        private int GetReservedDiskStorageSpace()
        {
            var existingCollections = ES.Services.HostedSharePointServers.GetSiteCollections(PanelSecurity.PackageId, false);

            return (int)existingCollections.Sum(siteCollection => siteCollection.MaxSiteStorage);
        }

        private void SaveItem()
        {
            if (!Page.IsValid)
            {
                return;
            }


            if (this.SiteCollectionId == 0)
            {
                if (this.userSelector.GetAccount() == null)
                {
                    localMessageBox.ShowWarningMessage("HOSTEDSHAREPOINT_NO_USERS");
                    return;
                }


                // new item
                try
                {
                    item = new SharePointSiteCollection();

                       if (!UseSharedSLL(PanelSecurity.PackageId))
                    {                        
                        SharePointSiteCollectionListPaged existentSiteCollections = ES.Services.HostedSharePointServers.GetSiteCollectionsPaged(PanelSecurity.PackageId, this.OrganizationId, "ItemName", String.Format("%{0}", this.domain.DomainName), String.Empty, 0, Int32.MaxValue);
                        foreach (SharePointSiteCollection existentSiteCollection in existentSiteCollections.SiteCollections)
                        {
                            Uri existentSiteCollectionUri = new Uri(existentSiteCollection.Name);
                            if (existentSiteCollection.Name == String.Format("{0}://{1}", existentSiteCollectionUri.Scheme, this.txtHostName.Text.ToLower() + "." + this.domain.DomainName))
                            {
                                localMessageBox.ShowWarningMessage("HOSTEDSHAREPOINT_DOMAIN_IN_USE");
                                return;
                            }
                        }

                        item.Name = this.txtHostName.Text.ToLower() + "." + this.domain.DomainName;
                    }
                    else
                        item.Name = string.Empty;

                    // get form data

                    item.OrganizationId = this.OrganizationId;
                    item.Id = this.SiteCollectionId;
                    item.PackageId = PanelSecurity.PackageId;

                    item.LocaleId = Int32.Parse(this.ddlLocaleID.SelectedValue);
                    item.OwnerLogin = this.userSelector.GetSAMAccountName();
                    item.OwnerEmail = this.userSelector.GetPrimaryEmailAddress();
                    item.OwnerName = this.userSelector.GetDisplayName();
                    item.Title = txtTitle.Text;
                    item.Description = txtDescription.Text;


                    item.MaxSiteStorage = maxStorage.QuotaValue;
                    item.WarningStorage = warningStorage.QuotaValue;                    

                    int result = ES.Services.HostedSharePointServers.AddSiteCollection(item);
                    if (result < 0)
                    {
                        localMessageBox.ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    localMessageBox.ShowErrorMessage("HOSTEDSHAREPOINT_ADD_SITECOLLECTION", ex);
                    return;
                }
            }
            else
            {
                ES.Services.HostedSharePointServers.UpdateQuota(PanelRequest.ItemID, SiteCollectionId, editMaxStorage.QuotaValue, editWarningStorage.QuotaValue);
            }

            // return
            RedirectToSiteCollectionsList();
        }

        private void AddDnsRecord(int domainId, string recordName, string recordData)
        {
            int result = ES.Services.Servers.AddDnsZoneRecord(domainId, recordName, DnsRecordType.A, recordData, 0, 0, 0, 0);
            if (result < 0)
            {
                ShowResultMessage(result);
            }
        }

        private bool IsDnsServiceAvailable()
        {
            ProviderInfo dnsProvider = ES.Services.Servers.GetPackageServiceProvider(PanelSecurity.PackageId, ResourceGroups.Dns);
            return dnsProvider != null;
        }

        private void DeleteItem()
        {
            // delete
            try
            {
                int result = ES.Services.HostedSharePointServers.DeleteSiteCollection(this.SiteCollectionId);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                localMessageBox.ShowErrorMessage("HOSTEDSHAREPOINT_DELETE_SITECOLLECTION", ex);
                return;
            }

            // return
            RedirectToSiteCollectionsList();
        }

        protected void odsAccountsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                localMessageBox.ShowErrorMessage("ORGANIZATION_GET_USERS", e.Exception);
                e.ExceptionHandled = true;
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // return
            RedirectToSiteCollectionsList();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveItem();
        }

        protected void btnBackup_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "sharepoint_backup_sitecollection", "SiteCollectionID=" + this.SiteCollectionId, "ItemID=" + PanelRequest.ItemID.ToString()));
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "sharepoint_restore_sitecollection", "SiteCollectionID=" + this.SiteCollectionId, "ItemID=" + PanelRequest.ItemID.ToString()));
        }



        private void RedirectToSiteCollectionsList()
        {
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "sharepoint_sitecollections", "ItemID=" + PanelRequest.ItemID.ToString()));
        }

        private bool UseSharedSLL(int packageID)
        {
            PackageContext cntx = ES.Services.Packages.GetPackageContext(PanelSecurity.PackageId);
            if (cntx != null)
            {
                foreach (QuotaValueInfo quota in cntx.QuotasArray)
                {
                    switch (quota.QuotaId)
                    {
                        case 400:
                            if (Convert.ToBoolean(quota.QuotaAllocatedValue))
                            {
                                return true;
                            }

                            break;
                    }
                }
            }

            return false;
        }


        //private void RegisterOwnerSelector()
        //{
        //    // Define the name and type of the client scripts on the page.
        //    String csname = "OwnerSelectorScript";
        //    Type cstype = this.GetType();

        //    // Get a ClientScriptManager reference from the Page class.
        //    ClientScriptManager cs = Page.ClientScript;

        //    // Check to see if the client script is already registered.
        //    if (!cs.IsClientScriptBlockRegistered(cstype, csname))
        //    {
        //        StringBuilder ownerSelector = new StringBuilder();
        //        ownerSelector.Append("<script type=text/javascript> function DoSelectOwner(ownerId, ownerDisplayName, email) {");
        //        ownerSelector.AppendFormat("{0}.{1}.value=ownerId;", this.Page.Form.ID, this.hdnSiteCollectionOwner.ClientID);
        //        ownerSelector.AppendFormat("{0}.{1}.value=ownerDisplayName;", this.Page.Form.ID, this.txtSiteCollectionOwner.ClientID);
        //        ownerSelector.AppendFormat("{0}.{1}.value=email;", this.Page.Form.ID, this.hdnSiteCollectionOwnerEmail.ClientID);
        //        ownerSelector.Append("} </script>");
        //        cs.RegisterClientScriptBlock(cstype, csname, ownerSelector.ToString(), false);
        //    }

        //}

        //private StringDictionary ConvertArrayToDictionary(string[] settings)
        //{
        //    StringDictionary r = new StringDictionary();
        //    foreach (string setting in settings)
        //    {
        //        int idx = setting.IndexOf('=');
        //        r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
        //    }
        //    return r;
        //}
    }
}
