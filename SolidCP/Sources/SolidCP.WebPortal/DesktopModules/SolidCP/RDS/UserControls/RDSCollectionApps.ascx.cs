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
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.HostedSolution;
using System.Linq;
using SolidCP.Providers.Web;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.RemoteDesktopServices;

namespace SolidCP.Portal.RDS.UserControls
{
    public partial class RDSCollectionApps : SolidCPControlBase
	{
        public const string DirectionString = "DirectionString";

		protected enum SelectedState
		{
			All,
			Selected,
			Unselected
		}

        public void SetApps(RemoteApplication[] apps)
		{            
            BindApps(apps, false);
		}

        public void SetApps(RemoteApplication[] apps, WebPortal.PageModule module)
        {
            Module = module;
            BindApps(apps, false);            
        }

        public RemoteApplication[] GetApps()
        {
            return GetGridViewApps(SelectedState.All).ToArray();
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			// register javascript
			if (!Page.ClientScript.IsClientScriptBlockRegistered("SelectAllCheckboxes"))
			{
				string script = @"    function SelectAllCheckboxes(box)
                {
		            var state = box.checked;
                    var elm = box.parentElement.parentElement.parentElement.parentElement.getElementsByTagName(""INPUT"");
                    for(i = 0; i < elm.length; i++)
                        if(elm[i].type == ""checkbox"" && elm[i].id != box.id && elm[i].checked != state && !elm[i].disabled)
		                    elm[i].checked = state;
                }";
                Page.ClientScript.RegisterClientScriptBlock(typeof(RDSCollectionUsers), "SelectAllCheckboxes",
					script, true);
			}
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			// bind all servers
			BindPopupApps();

			// show modal
			AddAppsModal.Show();
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
            List<RemoteApplication> selectedApps = GetGridViewApps(SelectedState.Unselected);

            BindApps(selectedApps.ToArray(), false);
		}

		protected void btnAddSelected_Click(object sender, EventArgs e)
		{
            List<RemoteApplication> selectedApps = GetPopUpGridViewApps();

            BindApps(selectedApps.ToArray(), true);
		}        

        protected void BindPopupApps()
		{
            RdsCollection collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);
            List<StartMenuApp> apps = ES.Services.RDS.GetAvailableRemoteApplications(PanelRequest.ItemID, collection.Name).ToList();
            var sessionHosts = ES.Services.RDS.GetRdsCollectionSessionHosts(PanelRequest.CollectionID);            

            var addedApplications = GetApps();
            var aliases = addedApplications.Select(p => p.Alias);
            apps = apps.Where(x => !aliases.Contains(x.Alias)).ToList();          

            if (Direction == SortDirection.Ascending)
            {
                apps = apps.OrderBy(a => a.DisplayName).ToList();
                Direction = SortDirection.Descending;
            }
            else
            {
                apps = apps.OrderByDescending(a => a.DisplayName).ToList();
                Direction = SortDirection.Ascending;
            }

            var requiredParams = addedApplications.Select(a => a.RequiredCommandLine.ToLower());

            foreach (var host in sessionHosts)
            {
                if (!requiredParams.Contains(string.Format("/v:{0}", host.ToLower())))
                {                    
                    var fullRemote = new StartMenuApp
                    {
                        DisplayName = string.Format("Full Desktop - {0}", host.ToLower()),
                        FilePath = "c:\\windows\\system32\\mstsc.exe",                        
                        RequiredCommandLine = string.Format("/v:{0}", host.ToLower())
                    };

                    var sessionHost = collection.Servers.Where(s => s.FqdName.Equals(host, StringComparison.CurrentCultureIgnoreCase)).First();

                    if (sessionHost != null)
                    {
                        fullRemote.DisplayName = string.Format("Full Desktop - {0}", sessionHost.Name.ToLower());
                    }

                    fullRemote.Alias = fullRemote.DisplayName.Replace(" ", "");

                    if (apps.Count > 0)
                    {
                        apps.Insert(0, fullRemote);
                    }
                    else
                    {
                        apps.Add(fullRemote);
                    }
                }
            }

            gvPopupApps.DataSource = apps;
            gvPopupApps.DataBind();
		}

        protected void BindApps(RemoteApplication[] newApps, bool preserveExisting)
		{            
			// get binded addresses
            List<RemoteApplication> apps = new List<RemoteApplication>();
			if(preserveExisting)
                apps.AddRange(GetGridViewApps(SelectedState.All));

            // add new servers
            if (newApps != null)
			{
                foreach (RemoteApplication newApp in newApps)
				{
					// check if exists
					bool exists = false;
                    foreach (RemoteApplication app in apps)
					{
                        if (app.DisplayName == newApp.DisplayName)
						{
							exists = true;
							break;
						}
					}

					if (exists)
						continue;

                    apps.Add(newApp);
				}
			}            

            gvApps.DataSource = apps;
            gvApps.DataBind();
		}

        protected List<RemoteApplication> GetGridViewApps(SelectedState state)
        {
            List<RemoteApplication> apps = new List<RemoteApplication>();
            for (int i = 0; i < gvApps.Rows.Count; i++)
            {
                GridViewRow row = gvApps.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                RemoteApplication app = new RemoteApplication();
                app.Alias = (string)gvApps.DataKeys[i][0];
                app.DisplayName = ((LinkButton)row.FindControl("lnkDisplayName")).Text;
                app.FilePath = ((HiddenField)row.FindControl("hfFilePath")).Value;
                app.RequiredCommandLine = ((HiddenField)row.FindControl("hfRequiredCommandLine")).Value;
                var users = ((HiddenField)row.FindControl("hfUsers")).Value;

                if (!string.IsNullOrEmpty(users))
                {
                    app.Users = new string[]{"New"};
                }


                if (state == SelectedState.All ||
                    (state == SelectedState.Selected && chkSelect.Checked) ||
                    (state == SelectedState.Unselected && !chkSelect.Checked))
                    apps.Add(app);
            }

            return apps;
        }

        protected List<RemoteApplication> GetPopUpGridViewApps()
        {
            List<RemoteApplication> apps = new List<RemoteApplication>();
            for (int i = 0; i < gvPopupApps.Rows.Count; i++)
            {
                GridViewRow row = gvPopupApps.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                if (chkSelect.Checked)
                {
                    apps.Add(new RemoteApplication
                    {
                        Alias = (string)gvPopupApps.DataKeys[i][0],
                        DisplayName = ((Literal)row.FindControl("litName")).Text,
                        FilePath = ((HiddenField)row.FindControl("hfFilePathPopup")).Value,
                        RequiredCommandLine = ((HiddenField)row.FindControl("hfRequiredCommandLinePopup")).Value
                    });
                }
            }

            return apps;

        }

		protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
		{
			BindPopupApps();
		}

        protected void gvApps_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditApplication")
            {
                Response.Redirect(GetCollectionUsersEditUrl(e.CommandArgument.ToString()));
            }
        }

        protected SortDirection Direction
        {
            get { return ViewState[DirectionString] == null ? SortDirection.Descending : (SortDirection)ViewState[DirectionString]; }
            set { ViewState[DirectionString] = value; }
        }

        protected static int CompareAccount(StartMenuApp app1, StartMenuApp app2)
        {
            return string.Compare(app1.DisplayName, app2.DisplayName);
        }

        public string GetCollectionUsersEditUrl(string appId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "rds_application_edit_users",
                    "CollectionId=" + PanelRequest.CollectionID, "ItemID=" + PanelRequest.ItemID, "ApplicationID=" + appId);
        }
	}
}
