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

namespace SolidCP.Portal.ExchangeServer.UserControls
{
    public partial class EnterpriseStoragePermissions : SolidCPControlBase
	{
        public const string DirectionString = "DirectionString";

		protected enum SelectedState
		{
			All,
			Selected,
			Unselected
		}

        public void SetPermissions(ESPermission[] permissions)
		{
			BindAccounts(permissions, false);
		}

		public ESPermission[] GetPemissions()
		{
			return GetGridViewPermissions(SelectedState.All).ToArray();
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
                Page.ClientScript.RegisterClientScriptBlock(typeof(EnterpriseStoragePermissions), "SelectAllCheckboxes",
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
            List<ESPermission> selectedAccounts = GetGridViewPermissions(SelectedState.Unselected);

			BindAccounts(selectedAccounts.ToArray(), false);
		}

		protected void btnAddSelected_Click(object sender, EventArgs e)
		{
            List<ExchangeAccount> selectedAccounts = GetGridViewAccounts();

            List<ESPermission> permissions = new List<ESPermission>();
            foreach (ExchangeAccount account in selectedAccounts)
            {
                permissions.Add(new ESPermission
                {
                    Account = account.AccountName,
                    DisplayName = account.DisplayName,
                    Access = "Read-Only",
                });
            }

            BindAccounts(permissions.ToArray(), true);

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
                case ExchangeAccountType.SecurityGroup:
                    imgName = "dlist_16.gif";
                    break;
                case ExchangeAccountType.DefaultSecurityGroup:
                    imgName = "dlist_16.gif";
                    break;
                default:
                    imgName = "admin_16.png";
                    break;
            }

            return GetThemedImage("Exchange/" + imgName);
        }

		protected void BindPopupAccounts()
		{
			ExchangeAccount[] accounts = ES.Services.EnterpriseStorage.SearchESAccounts(PanelRequest.ItemID,
				ddlSearchColumn.SelectedValue, txtSearchValue.Text + "%", "");

            accounts = accounts.Where(x => !GetPemissions().Select(p => p.Account).Contains(x.AccountName)).ToArray();
            Array.Sort(accounts, CompareAccount);
            if (Direction == SortDirection.Ascending)
            {
                Array.Reverse(accounts);
                Direction = SortDirection.Descending;
            }
            else
                Direction = SortDirection.Ascending;

            gvPopupAccounts.DataSource = accounts;
            gvPopupAccounts.DataBind();
		}

        protected void BindAccounts(ESPermission[] newPermissions, bool preserveExisting)
		{
			// get binded addresses
            List<ESPermission> permissions = new List<ESPermission>();
			if(preserveExisting)
                permissions.AddRange(GetGridViewPermissions(SelectedState.All));

			// add new accounts
            if (newPermissions != null)
			{
                foreach (ESPermission newPermission in newPermissions)
				{
					// check if exists
					bool exists = false;
                    foreach (ESPermission permission in permissions)
					{
						if (String.Compare(newPermission.Account, permission.Account, true) == 0)
						{
							exists = true;
							break;
						}
					}

					if (exists)
						continue;

                    permissions.Add(newPermission);
				}
			}

            gvPermissions.DataSource = permissions;
            gvPermissions.DataBind();
		}

        protected List<ESPermission> GetGridViewPermissions(SelectedState state)
        {
            List<ESPermission> permissions = new List<ESPermission>();
            for (int i = 0; i < gvPermissions.Rows.Count; i++)
            {
                GridViewRow row = gvPermissions.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                ESPermission permission = new ESPermission();
                permission.Account = (string)gvPermissions.DataKeys[i][0];
                permission.Access = ((Literal)row.FindControl("litAccess")).Text;
                permission.DisplayName = ((Literal)row.FindControl("litAccount")).Text;

                if (state == SelectedState.All ||
                    (state == SelectedState.Selected && chkSelect.Checked) ||
                    (state == SelectedState.Unselected && !chkSelect.Checked))
                    permissions.Add(permission);
            }
            
            return permissions;
        }

        protected List<ExchangeAccount> GetGridViewAccounts()
        {
            List<ExchangeAccount> accounts = new List<ExchangeAccount>();
            for (int i = 0; i < gvPopupAccounts.Rows.Count; i++)
            {
                GridViewRow row = gvPopupAccounts.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                    continue;

                if (chkSelect.Checked)
                {
                    accounts.Add(new ExchangeAccount
                    {
                        AccountName = (string)gvPopupAccounts.DataKeys[i][0],
                        DisplayName = ((Literal)row.FindControl("litDisplayName")).Text
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

        protected static int CompareAccount(ExchangeAccount user1, ExchangeAccount user2)
        {
            return string.Compare(user1.DisplayName, user2.DisplayName);
        }

        protected void btn_UpdateAccess(object sender, EventArgs e)
        {
            if (gvPermissions.HeaderRow != null)
            {
                CheckBox chkAllSelect = (CheckBox)gvPermissions.HeaderRow.FindControl("chkSelectAll");
                if (chkAllSelect != null)
                {
                    chkAllSelect.Checked = false;
                }
            }

            for (int i = 0; i < gvPermissions.Rows.Count; i++)
            {
                GridViewRow row = gvPermissions.Rows[i];

                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                Literal litAccess = (Literal)row.FindControl("litAccess");

                if (chkSelect == null || litAccess == null)
                    continue;

                if (chkSelect.Checked)
                {
                    chkSelect.Checked = false;
                    litAccess.Text = ((Button)sender).CommandArgument;
                }
            }
        }
	}
}
