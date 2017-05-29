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
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.RDS.UserControls
{
    public partial class RDSCollectionUsers : SolidCPControlBase
	{
        public const string DirectionString = "DirectionString";
        public event EventHandler OnRefreshClicked;

        public bool ButtonAddEnabled
        {
            get
            {
                return btnAdd.Enabled;
            }
            set
            {
                btnAdd.Enabled = value;
            }
        }

		protected enum SelectedState
		{
			All,
			Selected,
			Unselected
		}

        public void SetUsers(OrganizationUser[] users)
		{
            BindAccounts(users, false);
		}

        public OrganizationUser[] GetUsers()
		{
			return GetGridViewUsers(SelectedState.All).ToArray();
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
			// bind all accounts
			BindPopupAccounts();

			// show modal
			AddAccountsModal.Show();
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
            List<string> lockedUsers = new List<string>();

            if (PanelRequest.Ctl == "rds_collection_edit_users")
            {
                lockedUsers = CheckDeletedUsers();

                if (!lockedUsers.Any())
                {
                    List<OrganizationUser> selectedAccounts = GetGridViewUsers(SelectedState.Unselected);
                    BindAccounts(selectedAccounts.ToArray(), false);
                }                
            }
            else
            {
                List<OrganizationUser> selectedAccounts = GetGridViewUsers(SelectedState.Unselected);
                BindAccounts(selectedAccounts.ToArray(), false);
            }

            if (OnRefreshClicked != null)
            {
                OnRefreshClicked(lockedUsers, new EventArgs());
            }
		}

		protected void btnAddSelected_Click(object sender, EventArgs e)
		{
            List<OrganizationUser> selectedAccounts = GetGridViewAccounts();

            BindAccounts(selectedAccounts.ToArray(), true);

		}

        public string GetAccountImage(int accountTypeId)
        {
            string imgName = string.Empty;

            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;
            switch (accountType)
            {
                case ExchangeAccountType.Room:
                    imgName = "room_16.gif";
                    break;
                case ExchangeAccountType.Equipment:
                    imgName = "equipment_16.gif";
                    break;
                default:
                    imgName = "admin_16.png";
                    break;
            }

            return GetThemedImage("Exchange/" + imgName);
        }

        public List<string> CheckDeletedUsers()
        {            
            var rdsUsers = GetGridViewUsers(SelectedState.Selected);
            var collectionUsers = ES.Services.RDS.GetRdsCollectionUsers(PanelRequest.CollectionID);

            if (rdsUsers.All(r => !collectionUsers.Select(c => c.AccountName.ToLower()).Contains(r.AccountName.ToLower())))
            {
                return new List<string>();
            }

            var localAdmins = ES.Services.RDS.GetRdsCollectionLocalAdmins(PanelRequest.CollectionID);
            var organizationUsers = ES.Services.Organizations.GetOrganizationUsersPaged(PanelRequest.ItemID, null, null, null, 0, Int32.MaxValue).PageUsers;
            var applicationUsers = ES.Services.RDS.GetApplicationUsers(PanelRequest.ItemID, PanelRequest.CollectionID, null);
            var remoteAppUsers = organizationUsers.Where(x => applicationUsers.Select(a => a.Split('\\').Last().ToLower()).Contains(x.SamAccountName.Split('\\').Last().ToLower()));

            var deletedUsers = new List<OrganizationUser>();

            deletedUsers.AddRange(rdsUsers.Where(r => localAdmins.Select(l => l.AccountName.ToLower()).Contains(r.AccountName.ToLower())));
            remoteAppUsers = remoteAppUsers.Where(r => !localAdmins.Select(l => l.AccountName.ToLower()).Contains(r.AccountName.ToLower()));
            deletedUsers.AddRange(rdsUsers.Where(r => remoteAppUsers.Select(l => l.AccountName.ToLower()).Contains(r.AccountName.ToLower())));
            deletedUsers = deletedUsers.Distinct().ToList();            

            return deletedUsers.Select(d => d.DisplayName).ToList();
        }

        public void BindUsers()
        {
            var collectionUsers = ES.Services.RDS.GetRdsCollectionUsers(PanelRequest.CollectionID);
            var collection = ES.Services.RDS.GetRdsCollection(PanelRequest.CollectionID);
            var localAdmins = ES.Services.RDS.GetRdsCollectionLocalAdmins(PanelRequest.CollectionID);

            foreach (var user in collectionUsers)
            {
                if (localAdmins.Select(l => l.AccountName).Contains(user.AccountName))
                {
                    user.IsVIP = true;
                }
                else
                {
                    user.IsVIP = false;
                }
            }
            
            SetUsers(collectionUsers);
        }

		protected void BindPopupAccounts()
		{
            OrganizationUser[] accounts;

            if (PanelRequest.Ctl == "rds_collection_edit_users")
            {
                accounts = ES.Services.Organizations.GetOrganizationUsersPaged(PanelRequest.ItemID, null, null, null, 0, Int32.MaxValue).PageUsers;
            }
            else
            {
                accounts = ES.Services.RDS.GetRdsCollectionUsers(PanelRequest.CollectionID);
            }

            var localAdmins = ES.Services.RDS.GetRdsCollectionLocalAdmins(PanelRequest.CollectionID);

            foreach (var user in accounts)
            {
                if (localAdmins.Select(l => l.AccountName).Contains(user.AccountName))
                {
                    user.IsVIP = true;
                }
                else
                {
                    user.IsVIP = false;
                }                
            }

            accounts = accounts.Where(x => !GetUsers().Select(p => p.AccountName).Contains(x.AccountName)).ToArray();
            Array.Sort(accounts, CompareAccount);            

            gvPopupAccounts.DataSource = accounts;
            gvPopupAccounts.DataBind();
		}

        protected void BindAccounts(OrganizationUser[] newUsers, bool preserveExisting)
		{
			// get binded addresses
            List<OrganizationUser> users = new List<OrganizationUser>();
			if(preserveExisting)
                users.AddRange(GetGridViewUsers(SelectedState.All));

			// add new accounts
            if (newUsers != null)
			{
                foreach (OrganizationUser newUser in newUsers)
				{
					// check if exists
					bool exists = false;
                    foreach (OrganizationUser user in users)
					{
                        if (String.Compare(user.AccountName, newUser.AccountName, true) == 0)
						{
							exists = true;
							break;
						}
					}

					if (exists)
						continue;

                    users.Add(newUser);
				}
			}

            gvUsers.DataSource = users.OrderBy(u => u.DisplayName);
            gvUsers.DataBind();
		}

        protected List<OrganizationUser> GetGridViewUsers(SelectedState state)
        {
            List<OrganizationUser> users = new List<OrganizationUser>();
            for (int i = 0; i < gvUsers.Rows.Count; i++)
            {
                GridViewRow row = gvUsers.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                OrganizationUser user = new OrganizationUser();
                user.AccountName = (string)gvUsers.DataKeys[i][0];
                user.DisplayName = ((Literal)row.FindControl("litAccount")).Text;
                user.SamAccountName = ((HiddenField)row.FindControl("hdnSamAccountName")).Value;

                if (state == SelectedState.All ||
                    (state == SelectedState.Selected && chkSelect.Checked) ||
                    (state == SelectedState.Unselected && !chkSelect.Checked))
                    users.Add(user);
            }

            return users;
        }

        protected List<OrganizationUser> GetGridViewAccounts()
        {
            List<OrganizationUser> accounts = new List<OrganizationUser>();
            for (int i = 0; i < gvPopupAccounts.Rows.Count; i++)
            {
                GridViewRow row = gvPopupAccounts.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                if (chkSelect.Checked)
                {
                    accounts.Add(new OrganizationUser
                    {
                        AccountName = (string)gvPopupAccounts.DataKeys[i][0],
                        DisplayName = ((Literal)row.FindControl("litDisplayName")).Text,
                        SamAccountName = ((HiddenField)row.FindControl("hdnSamName")).Value,
                        IsVIP = Convert.ToBoolean(((HiddenField)row.FindControl("hdnLocalAdmin")).Value)
                    });
                }
            }

            return accounts;

        }

		protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
		{
			BindPopupAccounts();
		}

        protected SortDirection Direction
        {
            get { return ViewState[DirectionString] == null ? SortDirection.Descending : (SortDirection)ViewState[DirectionString]; }
            set { ViewState[DirectionString] = value; }
        }

        protected static int CompareAccount(OrganizationUser user1, OrganizationUser user2)
        {
            return string.Compare(user1.DisplayName, user2.DisplayName);
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SetupInstructions")
            {
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "rds_setup_letter", "CollectionID=" + PanelRequest.CollectionID, "ItemID=" + PanelRequest.ItemID, "AccountID=" + e.CommandArgument.ToString()));
            }
        }
	}
}
