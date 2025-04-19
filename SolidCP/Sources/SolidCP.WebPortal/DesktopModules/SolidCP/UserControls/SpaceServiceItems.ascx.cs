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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;
using System.Text;

namespace SolidCP.Portal.UserControls
{
    public partial class SpaceServiceItems : SolidCPControlBase
    {
        public string GroupName
        {
            get { return litGroupName.Text; }
            set { litGroupName.Text = value; }
        }

        public string TypeName
        {
            get { return litTypeName.Text; }
            set { litTypeName.Text = value; }
        }

        public string QuotaName
        {
            get { return (string)ViewState["QuotaName"]; }
            set { ViewState["QuotaName"] = value; }
        }

        public string CreateControlID
        {
            get { return ViewState["CreateControlID"] == null ? "edit" : (string)ViewState["CreateControlID"]; }
            set { ViewState["CreateControlID"] = value; }
        }

        public string ViewLinkText
        {
            get { return ViewState["ViewLinkText"] == null ? null : (string)ViewState["ViewLinkText"]; }
            set { ViewState["ViewLinkText"] = value; }
        }

        public string CreateButtonText
        {
            get { return btnAddItem.Text; }
            set { btnAddItem.Text = value; }
        }

        public bool ShowCreateButton
        {
            get { EnsureChildControls(); return btnAddItembtn.Visible; }
            set { EnsureChildControls(); btnAddItembtn.Visible = value; }
        }

        public bool ShowQuota
        {
            get { EnsureChildControls(); return QuotasPanel.Visible; }
            set { EnsureChildControls(); QuotasPanel.Visible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));

            //HideServiceColumns(gvWebSites);

            // set display preferences
            gvItems.PageSize = UsersHelper.GetDisplayItemsPerPage();

            itemsQuota.QuotaName = QuotaName;
            lblQuotaName.Text = GetSharedLocalizedString("Quota." + QuotaName) + ":";

            // edit button
            string localizedButtonText = HostModule.GetLocalizedString(btnAddItem.Text + ".Text");
            if (localizedButtonText != null)
                btnAddItem.Text = localizedButtonText;

            // visibility
            chkRecursive.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);
            gvItems.Columns[2].Visible = !String.IsNullOrEmpty(ViewLinkText);
            gvItems.Columns[3].Visible = gvItems.Columns[4].Visible =
                                         (PanelSecurity.SelectedUser.Role != UserRole.User) && chkRecursive.Checked;
            gvItems.Columns[5].Visible = (PanelSecurity.SelectedUser.Role == UserRole.Administrator);
            gvItems.Columns[6].Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);

            ShowActionList();

            if (!IsPostBack)
            {
                // toggle controls
                btnAddItembtn.Enabled = PackagesHelper.CheckGroupQuotaEnabled(
                    PanelSecurity.PackageId, GroupName, QuotaName);

                searchBox.AddCriteria("ItemName", GetLocalizedString("SearchField.ItemName"));
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
                searchBox.AddCriteria("FullName", GetLocalizedString("SearchField.FullName"));
                searchBox.AddCriteria("Email", GetLocalizedString("SearchField.EMail"));
            }
            searchBox.AjaxData = this.GetSearchBoxAjaxData();
        }

        public string GetUrl(object param1, object param2)
        {
            string url = GetItemEditUrl(param1, param2) + "&Mode=View";
            url = "http://localhost:8080/Portal" + url.Remove(0, 1);
            string encodedUrl = System.Web.HttpUtility.UrlPathEncode(url);
            return string.Format("javascript:void(window.open('{0}','{1}','{2}'))", encodedUrl, "window", "width=400,height=300,scrollbars,resizable");
        }

        public string GetUrlNormalized(object param1, object param2)
        {
            string url = GetItemEditUrl(param1, param2) + "&Mode=View";
            return System.Web.HttpUtility.UrlPathEncode(url);
        }

        public string GetItemEditUrl(object packageId, object itemId)
        {
            return HostModule.EditUrl("ItemID", itemId.ToString(), "edit_item",
                 PortalUtils.SPACE_ID_PARAM + "=" + packageId.ToString());
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, spaceId.ToString());
        }

        public string GetItemsPageUrl(string parameterName, string parameterValue)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(),
                parameterName + "=" + parameterValue);
        }

        protected void odsItemsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                HostModule.ProcessException(e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(HostModule.EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(),
                CreateControlID));
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detach")
            {
                // remove item from meta base
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                int result = ES.Services.Packages.DetachPackageItem(itemId);
                if (result < 0)
                {
                    HostModule.ShowResultMessage(result);
                    return;
                }

                // refresh the list
                gvItems.DataBind();
            }
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HyperLink lnkView = (HyperLink)e.Row.FindControl("lnkView");
            if (lnkView == null)
                return;

            string localizedLinkText = HostModule.GetLocalizedString(ViewLinkText + ".Text");
            lnkView.Text = localizedLinkText != null ? localizedLinkText : ViewLinkText;
        }

        private void ShowActionList()
        {
            var checkboxColumn = gvItems.Columns[0];
            websiteActions.Visible = false;
            mailActions.Visible = false;
            checkboxColumn.Visible = false;

            switch (QuotaName)
            {
                case "Web.Sites":
                    websiteActions.Visible = true;
                    checkboxColumn.Visible = true;
                    break;
                case "Mail.Accounts":
                    ProviderInfo provider = ES.Services.Servers.GetPackageServiceProvider(PanelSecurity.PackageId, "Mail");
                    if (provider.EditorControl == "SmarterMail100")
                    {
                        mailActions.Visible = true;
                        checkboxColumn.Visible = true;
                    }
                    break;
                case "Mail.AllowAccessControls":
                    gvItems.Columns[6].Visible = false;
                    break;

            }
        }

        public string GetSearchBoxAjaxData()
        {
            String spaceId = (String.IsNullOrEmpty(Request["SpaceID"]) ? "-1" : Request["SpaceID"]);
            StringBuilder res = new StringBuilder();
            res.Append("PagedStored: 'ServiceItems'");
            res.Append(", RedirectUrl: '" + GetItemEditUrl(spaceId, "{0}").Substring(2) + "'");
            res.Append(", PackageID: " + spaceId);
            res.Append(", ItemTypeName: $('#" + litTypeName.ClientID + "').val()");
            res.Append(", GroupName: $('#" + litGroupName.ClientID + "').val()");
            res.Append(", ServerID: " + (String.IsNullOrEmpty(Request["ServerID"]) ? "0" : Request["ServerID"]));
            res.Append(", Recursive: ($('#" + chkRecursive.ClientID + "').val() == 'on')");
            return res.ToString();
        }

    }
}
