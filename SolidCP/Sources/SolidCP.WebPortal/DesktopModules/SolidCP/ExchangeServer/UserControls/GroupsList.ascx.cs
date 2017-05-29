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

namespace SolidCP.Portal.ExchangeServer.UserControls
{
	public partial class GroupsList : SolidCPControlBase
	{
		private enum SelectedState
		{
			All,
			Selected,
			Unselected
		}

		public void SetAccounts(ExchangeAccount[] accounts)
		{
			BindAccounts(accounts, false);
		}

		public string[] GetAccounts()
		{
			// get selected accounts
			List<ExchangeAccount> selectedAccounts = GetGridViewAccounts(gvGroups, SelectedState.All);

			List<string> accountNames = new List<string>();
			foreach (ExchangeAccount account in selectedAccounts)
				accountNames.Add(account.AccountName);

			return accountNames.ToArray();
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
				Page.ClientScript.RegisterClientScriptBlock(typeof(AccountsList), "SelectAllCheckboxes",
					script, true);
			}
		}

		public string GetAccountImage(int accountTypeId)
		{
			ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;
            string imgName = "dlist_16.gif";

			return GetThemedImage("Exchange/" + imgName);
		}

		protected void btnAdd_Click(object sender, EventArgs e)
		{
			// bind all accounts
			BindPopupAccounts();

			// show modal
			AddGroupsModal.Show();
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
			// get selected accounts
			List<ExchangeAccount> selectedAccounts = GetGridViewAccounts(gvGroups, SelectedState.Unselected);

			// add to the main list
			BindAccounts(selectedAccounts.ToArray(), false);
		}

		protected void btnAddSelected_Click(object sender, EventArgs e)
		{
			// get selected accounts
			List<ExchangeAccount> selectedAccounts = GetGridViewAccounts(gvPopupGroups, SelectedState.Selected);
			
			// add to the main list
			BindAccounts(selectedAccounts.ToArray(), true);
		}

		private void BindPopupAccounts()
		{
			ExchangeAccount[] accounts = ES.Services.Organizations.SearchOrganizationAccounts(PanelRequest.ItemID,			
				ddlSearchColumn.SelectedValue, txtSearchValue.Text + "%", "", true);

            accounts = accounts.Where(x => !GetAccounts().Contains(x.AccountName)).ToArray();

			gvPopupGroups.DataSource = accounts;
			gvPopupGroups.DataBind();
		}

		private void BindAccounts(ExchangeAccount[] newAccounts, bool preserveExisting)
		{
			// get binded addresses
			List<ExchangeAccount> accounts = new List<ExchangeAccount>();
			if(preserveExisting)
				accounts.AddRange(GetGridViewAccounts(gvGroups, SelectedState.All));

			// add new accounts
			if (newAccounts != null)
			{
				foreach (ExchangeAccount newAccount in newAccounts)
				{
					// check if exists
					bool exists = false;
					foreach (ExchangeAccount account in accounts)
					{
						if (String.Compare(newAccount.AccountName, account.AccountName, true) == 0)
						{
							exists = true;
							break;
						}
					}

					if (exists)
						continue;

					accounts.Add(newAccount);
				}
			}

			gvGroups.DataSource = accounts;
			gvGroups.DataBind();

            UpdateGridViewAccounts(gvGroups);

            btnDelete.Visible = gvGroups.Rows.Count > 0;
		}

		private List<ExchangeAccount> GetGridViewAccounts(GridView gv, SelectedState state)
		{
			List<ExchangeAccount> accounts = new List<ExchangeAccount>();
			for (int i = 0; i < gv.Rows.Count; i++)
			{
				GridViewRow row = gv.Rows[i];
				CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
				if (chkSelect == null)
					continue;

				ExchangeAccount account = new ExchangeAccount();
				account.AccountType = (ExchangeAccountType)Enum.Parse(typeof(ExchangeAccountType), ((Literal)row.FindControl("litAccountType")).Text);
				account.AccountName = (string)gv.DataKeys[i][0];
				account.DisplayName = ((Literal)row.FindControl("litDisplayName")).Text;

				if(state == SelectedState.All || 
					(state == SelectedState.Selected && chkSelect.Checked) ||
					(state == SelectedState.Unselected && !chkSelect.Checked))
					accounts.Add(account);
			}
			return accounts;
		}

        private void UpdateGridViewAccounts(GridView gv)
        {
            CheckBox chkSelectAll = (CheckBox)gv.HeaderRow.FindControl("chkSelectAll");

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow row = gv.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect == null)
                {
                    continue;
                }

                ExchangeAccountType exAccountType = (ExchangeAccountType)Enum.Parse(typeof(ExchangeAccountType), ((Literal)row.FindControl("litAccountType")).Text);

                if (exAccountType != ExchangeAccountType.DefaultSecurityGroup)
                {
                    chkSelectAll = null;
                    chkSelect.Enabled = true;
                }
                else
                {
                    chkSelect.Enabled = false;
                }
            }

            if (chkSelectAll != null)
            {
                chkSelectAll.Enabled = false;
            }
        }

		protected void chkIncludeMailboxes_CheckedChanged(object sender, EventArgs e)
		{
			BindPopupAccounts();
		}

		protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
		{
			BindPopupAccounts();
		}
	}
}
