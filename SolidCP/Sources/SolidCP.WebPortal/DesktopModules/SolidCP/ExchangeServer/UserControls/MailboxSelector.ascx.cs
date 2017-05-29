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
using System.Text.RegularExpressions;

namespace SolidCP.Portal.ExchangeServer.UserControls
{
    public partial class MailboxSelector : SolidCPControlBase
    {
        public const string DirectionString = "DirectionString";

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

        public bool ShowOnlyMailboxes
        {
            get { return ViewState["ShowOnlyMailboxes"] != null ? (bool)ViewState["ShowOnlyMailboxes"] : false; }
            set { ViewState["ShowOnlyMailboxes"] = value; }
        }

        public int ExcludeAccountId
        {
            get { return PanelRequest.AccountID; }
        }

        public void SetAccount(ExchangeAccount account)
        {
            BindSelectedAccount(account);
        }

        public string GetAccount()
        {
            return (string)ViewState["AccountName"];
        }
        public int GetAccountId()
        {
            return Utils.ParseInt(ViewState["AccountId"], 0);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // toggle controls
            if (!IsPostBack)
            {
                chkIncludeMailboxes.Visible = MailboxesEnabled;

                chkIncludeRooms.Visible = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeEquipment.Visible = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeSharedMailbox.Visible = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeMailboxes.Checked = MailboxesEnabled;

                chkIncludeRooms.Checked = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeEquipment.Checked = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeSharedMailbox.Checked = MailboxesEnabled && !ShowOnlyMailboxes;

                chkIncludeContacts.Visible = ContactsEnabled;
                chkIncludeContacts.Checked = ContactsEnabled;
                chkIncludeLists.Visible = DistributionListsEnabled;
                chkIncludeLists.Checked = DistributionListsEnabled;
            }

            // increase timeout
            ScriptManager scriptMngr = ScriptManager.GetCurrent(this.Page);
            scriptMngr.AsyncPostBackTimeout = 300;
        }

        private void BindSelectedAccount(ExchangeAccount account)
        {
            if (account != null)
            {
                txtDisplayName.Text = account.DisplayName;
                ViewState["AccountName"] = account.AccountName;
                ViewState["PrimaryEmailAddress"] = account.PrimaryEmailAddress;
                ViewState["AccountId"] = account.AccountId;
            }
            else
            {
                txtDisplayName.Text = "";
                ViewState["AccountName"] = null;
                ViewState["PrimaryEmailAddress"] = null;
                ViewState["AccountId"] = null;
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
            else if (accountType == ExchangeAccountType.SharedMailbox)
                imgName = "shared_16.gif";

            return GetThemedImage("Exchange/" + imgName);
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

        protected void chkIncludeMailboxes_CheckedChanged(object sender, EventArgs e)
        {
            BindPopupAccounts();
        }

        protected void cmdSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindPopupAccounts();
        }

        protected void cmdClear_Click(object sender, EventArgs e)
        {
            BindSelectedAccount(null);
        }

        protected void ImageButton1_Click(object sender, EventArgs e)
        {
            // bind all accounts
            BindPopupAccounts();

            // show modal
            SelectAccountsModal.Show();
        }

        protected void gvPopupAccounts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectAccount")
            {

                string[] parts = e.CommandArgument.ToString().Split('^');
                ExchangeAccount account = new ExchangeAccount();
                account.AccountName = parts[0];
                account.DisplayName = parts[1];
                account.PrimaryEmailAddress = parts[2];
                account.AccountId = Utils.ParseInt(parts[3]);


                // set account
                BindSelectedAccount(account);

                // hide popup
                SelectAccountsModal.Hide();

                // update parent panel
                MainUpdatePanel.Update();
            }
        }

        protected void OnSorting(object sender, GridViewSortEventArgs e)
        {

            BindPopupAccounts();

        }
    }
}
