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
    public partial class UsersList : SolidCPControlBase
	{
        public const string DirectionString = "DirectionString";

        private bool _enabled = true;

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

		private enum SelectedState
		{
			All,
			Selected,
			Unselected
		}

	    public int ExcludeAccountId
		{
			get { return PanelRequest.AccountID; }
		}

        public void SetAccounts(ExchangeAccount[] accounts)
		{
			BindAccounts(accounts, false);
		}

		public string[] GetAccounts()
		{
			// get selected accounts
			List<ExchangeAccount> selectedAccounts = GetGridViewAccounts(gvAccounts, SelectedState.All);

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

            btnAdd.Visible = Enabled;
            btnDelete.Visible = Enabled;

            gvAccounts.Columns[0].Visible = Enabled;
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
			// get selected accounts
            List<ExchangeAccount> selectedAccounts = GetGridViewAccounts(gvAccounts, SelectedState.Unselected);

			// add to the main list
			BindAccounts(selectedAccounts.ToArray(), false);
		}

		protected void btnAddSelected_Click(object sender, EventArgs e)
		{
			// get selected accounts
            List<ExchangeAccount> selectedAccounts = GetGridViewAccounts(gvPopupAccounts, SelectedState.Selected);
			
			// add to the main list
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
                case ExchangeAccountType.DistributionList:
                    imgName = "dlist_16.gif";
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

        public string GetType(int accountTypeId)
        {
            ExchangeAccountType accountType = (ExchangeAccountType)accountTypeId;

            switch (accountType)
            {
                case ExchangeAccountType.DistributionList:
                    return "Distribution";
                case ExchangeAccountType.SecurityGroup:
                    return "Security";
                case ExchangeAccountType.DefaultSecurityGroup:
                    return "Default Group";
                default:
                    return accountType.ToString();
            }
        }

		private void BindPopupAccounts()
		{
			ExchangeAccount[] accounts = ES.Services.Organizations.SearchOrganizationAccounts(PanelRequest.ItemID,
				ddlSearchColumn.SelectedValue, txtSearchValue.Text + "%", "", false);

            List<ExchangeAccount> newAccounts = new List<ExchangeAccount>();

            accounts = accounts.Where(x => !GetAccounts().Contains(x.AccountName)).ToArray();

			if (ExcludeAccountId > 0)
			{
                List<ExchangeAccount> updatedAccounts = new List<ExchangeAccount>();
                foreach (ExchangeAccount account in accounts)
					if (account.AccountId != ExcludeAccountId)
						updatedAccounts.Add(account);

				accounts = updatedAccounts.ToArray();
			}

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

        private void BindAccounts(ExchangeAccount[] newAccounts, bool preserveExisting)
		{
			// get binded addresses
            List<ExchangeAccount> accounts = new List<ExchangeAccount>();
			if(preserveExisting)
				accounts.AddRange(GetGridViewAccounts(gvAccounts, SelectedState.All));

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

			gvAccounts.DataSource = accounts;
			gvAccounts.DataBind();

            btnDelete.Visible = gvAccounts.Rows.Count > 0;
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
				account.PrimaryEmailAddress = ((Literal)row.FindControl("litPrimaryEmailAddress")).Text;

				if(state == SelectedState.All || 
					(state == SelectedState.Selected && chkSelect.Checked) ||
					(state == SelectedState.Unselected && !chkSelect.Checked))
					accounts.Add(account);
			}
			return accounts;
		}

		protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
		{
			BindPopupAccounts();
		}

        private SortDirection Direction
        {
            get { return ViewState[DirectionString] == null ? SortDirection.Descending : (SortDirection)ViewState[DirectionString]; }
            set { ViewState[DirectionString] = value; }
        }

        private static int CompareAccount(ExchangeAccount user1, ExchangeAccount user2)
        {
            return string.Compare(user1.DisplayName, user2.DisplayName);
        }
	}
}
