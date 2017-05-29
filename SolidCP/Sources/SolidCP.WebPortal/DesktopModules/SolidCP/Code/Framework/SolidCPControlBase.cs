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
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    /// <summary>
    /// Summary description for SolidCPControlBase
    /// </summary>
	public class SolidCPControlBase : WebPortalControlBase
    {
        // disable controls flag
        protected bool DisableControls = false;
        protected List<Control> ExcludeDisableControls = new List<Control>();

        private Control hostModule = null;
        public SolidCPModuleBase HostModule
        {
            get
            {
                if (hostModule == null)
                {
                    // find nearest parent
                    hostModule = this.Parent;
                    while (true)
                    {
                        if (hostModule == null || hostModule is SolidCPModuleBase)
                            break;

                        hostModule = hostModule.Parent;
                    }
                }
                return (SolidCPModuleBase)hostModule;
            }
        }

        public string TaskID
        {
            get { return (string)Context.Items["SolidCPAtlasTaskID"]; }
        }

        public string AsyncTaskID
        {
            get { return (string)Context.Items["SolidCPAtlasAsyncTaskID"]; }
            set { Context.Items["SolidCPAtlasAsyncTaskID"] = value; }
        }

		public string AsyncTaskTitle
		{
			get { return (string)Context.Items["SolidCPAtlasAsyncTaskTitle"]; }
			set { Context.Items["SolidCPAtlasAsyncTaskTitle"] = value; }
		}

        public virtual void RedirectToBrowsePage()
        {
            Response.Redirect(NavigateURL(), true);
        }

        public void RedirectAccountHomePage(int userId)
        {
            Response.Redirect(NavigateURL(PortalUtils.USER_ID_PARAM, userId.ToString()));
        }

        public void RedirectAccountHomePage()
        {
            RedirectAccountHomePage(PanelSecurity.SelectedUserId);
        }

        public void RedirectSpaceHomePage()
        {
            Response.Redirect(NavigateURL(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString()));
        }

        public void LocalizeGridView(GridView grid)
        {
            foreach (DataControlField column in grid.Columns)
            {
                // localize header text
                string key = column.HeaderText;
                if (key != null && key != "")
                {
					string localizedText = GetLocalizedString(key + ".Header");
                    if (localizedText != null)
                        column.HeaderText = localizedText;
                }
            }

            // localize empty message
            string etKey = grid.EmptyDataText;
            if (etKey != null && etKey != "")
            {
				string localizedText = GetLocalizedString(etKey + ".Empty");
                if (localizedText != null)
                    grid.EmptyDataText = localizedText;
            }
        }

        public void LocalizeGridViews(Control ctrl)
        {
            // localize control
            if (ctrl is GridView)
            {
                GridView gv = ctrl as GridView;
                LocalizeGridView(gv);
            }
            else
            {
                // localize children
                foreach (Control childCtrl in ctrl.Controls)
                    LocalizeGridViews(childCtrl);
            }
        }

        public void LocalizeMenuItems(string key, MenuItemCollection items)
        {
            foreach (MenuItem item in items)
            {
                string itemText = item.Text;
                string localizedText = GetLocalizedString(key + "." + itemText);
                if (localizedText != null)
                    item.Text = localizedText;

                // localize tool tip
                string localizedTooltip = GetLocalizedString(key + "." + itemText + ".ToolTip");
                if (localizedTooltip != null)
                    item.ToolTip = localizedTooltip;

                // localize child items
                LocalizeMenuItems(key, item.ChildItems);
            }
        }

        public void LocalizeListItems(string key, ListItemCollection items)
        {
            foreach (ListItem item in items)
            {
                string localizedText = GetLocalizedString(key + "Item." + item.Text);
                if (localizedText != null)
                    item.Text = localizedText;
            }
        }

        public void LocalizeModuleControls(Control ctrl)
        {
            // localize control
            if (ctrl is Button)
            {
                Button btn = ctrl as Button;
                string key = btn.Attributes["resourcekey"];
                if (key != null && key != "")
                {
					string localizedOnClientClick = GetLocalizedString(key + ".OnClientClick");
                    if (localizedOnClientClick != null)
                        btn.OnClientClick = localizedOnClientClick;
                }
            }
            else if (ctrl is Label)
            {
                Label lbl = ctrl as Label;
                string key = lbl.Attributes["resourcekey"];
                if (key != null && key != "")
                {
					string localizedText = GetLocalizedString(key + ".Text");
                    if (localizedText != null)
                        lbl.Text = localizedText;
                }
            }
            else if (ctrl is ImageButton)
            {
                ImageButton btn = ctrl as ImageButton;
                string key = btn.Attributes["resourcekey"];
                if (key != null && key != "")
                {
					string localizedOnClientClick = GetLocalizedString(key + ".OnClientClick");
                    if (localizedOnClientClick != null)
                        btn.OnClientClick = localizedOnClientClick;

					string localizedAlternateText = GetLocalizedString(key + ".AlternateText");
                    if (localizedAlternateText != null)
                        btn.AlternateText = localizedAlternateText;
                }
            }
            else if (ctrl is Menu)
            {
                Menu menu = ctrl as Menu;

                string key = menu.Attributes["resourcekey"];
                if (key != null && key != "")
                {
                    LocalizeMenuItems(key, menu.Items);
                }
            }
            else if (ctrl is DropDownList)
            {
                DropDownList ddl = ctrl as DropDownList;
                string key = ddl.Attributes["resourcekey"];
                if (key != null && key != "")
                {
                    LocalizeListItems(key, ddl.Items);
                }
            }
            else if (ctrl is RadioButtonList)
            {
                RadioButtonList rbl = ctrl as RadioButtonList;
                string key = rbl.Attributes["resourcekey"];
                if (key != null && key != "")
                {
                    LocalizeListItems(key, rbl.Items);
                }
            }
            else
            {
                // localize children
                foreach (Control childCtrl in ctrl.Controls)
                    LocalizeModuleControls(childCtrl);
            }
        }

        public void DisableFormControls(Control ctrl, params Control[] excludeControls)
        {
            if (excludeControls != null)
            {
                foreach (Control exCtrl in excludeControls)
                {
                    if (ctrl == exCtrl)
                        return;
                }
            }

            if (ctrl is ImageButton)
            {
                ((ImageButton)ctrl).Enabled = false;
            }
            else if (ctrl is Button)
            {
                ((Button)ctrl).Enabled = false;
            }
            else if (ctrl is CheckBox)
            {
                ((CheckBox)ctrl).Enabled = false;
            }
            else if (ctrl is TextBox)
            {
                ((TextBox)ctrl).Enabled = false;
            }
            else if (ctrl is DropDownList)
            {
                ((DropDownList)ctrl).Enabled = false;
            }
            else if (ctrl is Menu)
            {
                ((Menu)ctrl).Enabled = false;
            }
            else
            {
                // disable children
                foreach (Control childCtrl in ctrl.Controls)
                    DisableFormControls(childCtrl, excludeControls);
            }
        }

        public void FillDatabaseVersions(int packageId, ListItemCollection items, List<string> versions)
        {
            if (versions == null)
                return;

            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);

            items.Clear();
            AddDatabaseVersion(cntx, ResourceGroups.MsSql2000, items, versions);
            AddDatabaseVersion(cntx, ResourceGroups.MsSql2005, items, versions);
            AddDatabaseVersion(cntx, ResourceGroups.MsSql2008, items, versions);
            AddDatabaseVersion(cntx, ResourceGroups.MsSql2012, items, versions);
            AddDatabaseVersion(cntx, ResourceGroups.MsSql2014, items, versions);
            AddDatabaseVersion(cntx, ResourceGroups.MsSql2016, items, versions);
            AddDatabaseVersion(cntx, ResourceGroups.MySql4, items, versions);
            AddDatabaseVersion(cntx, ResourceGroups.MySql5, items, versions);
            AddDatabaseVersion(cntx, ResourceGroups.MariaDB, items, versions);
        }

        private void AddDatabaseVersion(PackageContext cntx, string groupName, ListItemCollection items, List<string> versions)
        {
            if(cntx.Groups.ContainsKey(groupName) && versions.Contains(groupName))
                items.Add(new ListItem(GetSharedLocalizedString(Utils.ModuleName, "ResourceGroup." + groupName), groupName));
        }

        protected override void OnInit(EventArgs e)
        {   
            //base.LocalResourceFile = LocalResourceFile;

            // call base handler
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            // localize grid viewes
            LocalizeGridViews(this);

            //if (PanelSecurity.LoggedUser != null)
                //if (PanelSecurity.LoggedUser.Role == UserRole.CSR) DisableControls = false;

            // call base handler
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            // localize all controls
            LocalizeModuleControls(this);


            string role = string.Empty;
            if (PanelSecurity.LoggedUser != null)
            {
                switch (PanelSecurity.LoggedUser.Role)
                {
                    case UserRole.Administrator:
                        role = "Administrator";
                        break;
                    case UserRole.Reseller:
                        role = "Reseller";
                        break;
                    case UserRole.User:
                        role = "User";
                        break;
                    case UserRole.PlatformCSR:
                        role = "PlatformCSR";
                        break;
                    case UserRole.PlatformHelpdesk:
                        role = "PlatformHelpdesk";
                        break;
                    case UserRole.ResellerCSR:
                        role = "ResellerCSR";
                        break;
                    case UserRole.ResellerHelpdesk:
                        role = "ResellerHelpdesk";
                        break;
                }
            }

            if (Module != null)
            {
                if (Module.ReadOnlyRoles != null)
                    if (Module.ReadOnlyRoles.Contains(role))
                        DisableControls = true;
            }

            // disable controls (if required)
            if (DisableControls)
                DisableFormControls(this, ExcludeDisableControls.ToArray());

            // call base handler
            base.OnPreRender(e);
        }

		public string GetSharedLocalizedString(string resourceKey)
		{
			return base.GetSharedLocalizedString(Utils.ModuleName, resourceKey);
		}

        #region Localization routines
        public string GetExceedingQuotasMessage(DataSet ds)
        {
            if (ds == null || ds.Tables[0].Rows.Count == 0)
                return "";

            List<string> quotas = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                quotas.Add(String.Format("{0} ({1})",
                    GetSharedLocalizedString(Utils.ModuleName, "Quota." + dr["QuotaName"].ToString()),
                    dr["QuotaValue"]));
            }
            return GetSharedLocalizedString(Utils.ModuleName, "Text.ExceedingQuotas") + String.Join(", ", quotas.ToArray());
        }

        public string GetAuditLogRecordSeverityName(int severityId)
        {
            string typeName = "Information";
            if (severityId == 1)
                typeName = "Warning";
            else if (severityId == 2)
                typeName = "Error";

            string localizedType = GetSharedLocalizedString(Utils.ModuleName, "AuditRecordSeverity." + typeName);
            return (localizedType != null) ? localizedType : typeName;
        }

        public string GetAuditLogSourceName(string sourceName)
        {
            string localizedText = GetSharedLocalizedString(Utils.ModuleName, "AuditLogSource." + sourceName);
            return (localizedText != null) ? localizedText : sourceName;
        }

        public string GetAuditLogTaskName(string sourceName, string taskName)
        {
            string localizedText = GetSharedLocalizedString(Utils.ModuleName,
                "AuditLogTask." + sourceName + "_" + taskName);
            return (localizedText != null) ? localizedText : taskName;
        }
        #endregion
    }
}
