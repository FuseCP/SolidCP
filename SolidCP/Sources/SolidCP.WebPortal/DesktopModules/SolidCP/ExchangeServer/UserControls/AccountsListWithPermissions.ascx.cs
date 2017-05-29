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
using System.Data;

namespace SolidCP.Portal.ExchangeServer.UserControls
{
	public partial class AccountsListWithPermissions : SolidCPControlBase
	{
		private enum SelectedState
		{
			All,
			Selected,
			Unselected
		}

	    public bool EnableMailboxOnly
	    {
            get {return ViewState["EnableMailboxOnly"] != null ? (bool)ViewState["EnableMailboxOnly"]: false; }
            set { ViewState["EnableMailboxOnly"] = value; }
	    }
        
        public bool MailboxesEnabled
		{
			get { return ViewState["MailboxesEnabled"] != null ? (bool)ViewState["MailboxesEnabled"] : false; }
			set { ViewState["MailboxesEnabled"] = value; }
		}

		public bool ContactsEnabled
		{
			get { return ViewState["ContactsEnabled"] != null ? (bool)ViewState["ContactsEnabled"] : false; }
			set { ViewState["ContactsEnabled"] = value; }
		}

		public bool DistributionListsEnabled
		{
			get { return ViewState["DistributionListsEnabled"] != null ? (bool)ViewState["DistributionListsEnabled"] : false; }
			set { ViewState["DistributionListsEnabled"] = value; }
		}

		public int ExcludeAccountId
		{
			get { return PanelRequest.AccountID; }
		}

		public void SetAccounts(ExchangeAccount[] accounts)
		{
			BindAccounts(accounts, false);
		}

		public ExchangeAccount[] GetAccounts()
		{
			// get selected accounts
			List<ExchangeAccount> selectedAccounts = GetGridViewAccounts(gvAccounts, SelectedState.All);

            List<ExchangeAccount> accountNames = new List<ExchangeAccount>();
            foreach (ExchangeAccount account in selectedAccounts)
            { 
                accountNames.Add(account); 
            }
            ExchangeAccount[] accounts = accountNames.ToArray();
            return accounts;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			
			// toggle controls
			if (!IsPostBack)
			{
				chkIncludeMailboxes.Visible = chkIncludeRooms.Visible = chkIncludeEquipment.Visible = chkIncludeSharedMailbox.Visible = MailboxesEnabled;
                chkIncludeMailboxes.Checked = chkIncludeRooms.Checked = chkIncludeEquipment.Checked = chkIncludeSharedMailbox.Checked = MailboxesEnabled;
				
                if (EnableMailboxOnly)
				{
				    chkIncludeRooms.Checked = false;
				    chkIncludeRooms.Visible = false;
				    chkIncludeEquipment.Checked = false;
				    chkIncludeEquipment.Visible = false;
                    chkIncludeSharedMailbox.Checked = false;
                    chkIncludeSharedMailbox.Visible = false;
                }
                
                chkIncludeContacts.Visible = ContactsEnabled;
				chkIncludeContacts.Checked = ContactsEnabled;
				chkIncludeLists.Visible = DistributionListsEnabled;
				chkIncludeLists.Checked = DistributionListsEnabled;
			}

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
			string imgName = "mailbox_16.gif";
			if (accountType == ExchangeAccountType.Contact)
				imgName = "contact_16.gif";
			else if (accountType == ExchangeAccountType.DistributionList)
				imgName = "dlist_16.gif";
            else if (accountType == ExchangeAccountType.Room)
                imgName = "room_16.gif";
            else if (accountType == ExchangeAccountType.Equipment)
                imgName = "equipment_16.gif";

			return GetThemedImage("Exchange/" + imgName);
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
            List<ExchangeAccount> selectedAccountsWithPermissions = new List<ExchangeAccount>();
            foreach (ExchangeAccount account in selectedAccounts)
            {
                if (account.AccountId != ExcludeAccountId)
                    account.PublicFolderPermission = "Reviewer";
                selectedAccountsWithPermissions.Add(account);
            }
            // add to the main list
            BindAccounts(selectedAccountsWithPermissions.ToArray(), true);
		}

		private void BindPopupAccounts()
		{
			ExchangeAccount[] accounts = ES.Services.ExchangeServer.SearchAccounts(PanelRequest.ItemID,
				chkIncludeMailboxes.Checked, chkIncludeContacts.Checked, chkIncludeLists.Checked,
                chkIncludeRooms.Checked, chkIncludeEquipment.Checked, chkIncludeSharedMailbox.Checked, false,
				ddlSearchColumn.SelectedValue, txtSearchValue.Text + "%", "");

			if (ExcludeAccountId > 0)
			{
				List<ExchangeAccount> updatedAccounts = new List<ExchangeAccount>();
                foreach (ExchangeAccount account in accounts)
                {
                    if (account.AccountId != ExcludeAccountId)
                        account.PublicFolderPermission = "Reviewer";
                        updatedAccounts.Add(account);
                }
				accounts = updatedAccounts.ToArray();
			}

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
                if (gv != gvPopupAccounts)
                {
                    DropDownList ddlPermissions = (DropDownList)row.FindControl("ddlPermissions");
                    //HiddenField PermissionLabel = (HiddenField)row.FindControl("PermissionLabel");
                    //account.PublicFolderPermission = PermissionLabel.Value;
                    account.PublicFolderPermission = ddlPermissions.SelectedValue;
                }
				if(state == SelectedState.All || 
					(state == SelectedState.Selected && chkSelect.Checked) ||
					(state == SelectedState.Unselected && !chkSelect.Checked))
					accounts.Add(account);
			}
			return accounts;
		}

		protected void chkIncludeMailboxes_CheckedChanged(object sender, EventArgs e)
		{
			BindPopupAccounts();
		}

		protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
		{
			BindPopupAccounts();
		}

        protected void gvAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DropDownList ddlPermissions = (DropDownList)e.Row.FindControl("ddlPermissions");
                HiddenField PermissionLabel = (HiddenField)e.Row.FindControl("PermissionLabel");

                ExchangeAccount dRow = (ExchangeAccount)e.Row.DataItem as ExchangeAccount;
                ListItem itm = new ListItem(dRow.PublicFolderPermission, dRow.PublicFolderPermission);

                if (ddlPermissions.Items.Contains(itm))
                {
                    PermissionLabel.Value = dRow.PublicFolderPermission;
                    ddlPermissions.SelectedValue = dRow.PublicFolderPermission;
                }
                else
                {
                    PermissionLabel.Value = dRow.PublicFolderPermission;
                    ddlPermissions.Items.Add(itm);
                    ddlPermissions.SelectedValue = dRow.PublicFolderPermission;
                    ddlPermissions.DataBind();
                }

            }
        }

        protected void ddlPermissions_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCurrentDropDownList = (DropDownList)sender;
            GridViewRow grdrDropDownRow = ((GridViewRow)ddlCurrentDropDownList.Parent.Parent);
            
            ExchangeAccount ex = (ExchangeAccount)grdrDropDownRow.DataItem as ExchangeAccount;
            if (ex != null)
                ex.PublicFolderPermission = ddlCurrentDropDownList.SelectedValue;
        }					
	}
}
