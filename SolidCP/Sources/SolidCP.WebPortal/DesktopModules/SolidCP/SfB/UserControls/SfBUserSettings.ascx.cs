// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
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
// - Neither  the  name  of  SolidCP  nor   the   names  of  its
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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Portal.SfB.UserControls
{
    public partial class SfBUserSettings : SolidCPControlBase
    {

        private string sipAddressToSelect;

        public string sipAddress
        {
                        
            get 
            {
                if (ddlSipAddresses.Visible)
                {
                    if ((ddlSipAddresses != null) && (ddlSipAddresses.SelectedItem != null))
                        return ddlSipAddresses.SelectedItem.Value;
                    else
                        return string.Empty;
                }
                else
                {
                    return email.Email;
                }
            }
            set
            {
                sipAddressToSelect = value;

                if (ddlSipAddresses.Visible)
                {
                    if ((ddlSipAddresses != null) && (ddlSipAddresses.Items != null))
                    {
                        foreach (ListItem li in ddlSipAddresses.Items)
                        {
                            if (li.Value == value)
                            {
                                ddlSipAddresses.ClearSelection();
                                li.Selected = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        string[] Tmp = value.Split('@');
                        email.AccountName = Tmp[0];

                        if (Tmp.Length > 1)
                        {
                            email.DomainName = Tmp[1];
                        }
                    }
                }
            }
        }

        public int plansCount
		{
			get
			{
                return this.ddlSipAddresses.Items.Count;
			}
		}


        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
                BindAddresses();
			}
        }

        private void BindAddresses()
		{

            OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

            if (user == null)
                return;

            if (user.AccountType == ExchangeAccountType.Mailbox)
            {
                email.Visible = false;
                ddlSipAddresses.Visible = true;

                SolidCP.EnterpriseServer.ExchangeEmailAddress[] emails = ES.Services.ExchangeServer.GetMailboxEmailAddresses(PanelRequest.ItemID, PanelRequest.AccountID);

                foreach (SolidCP.EnterpriseServer.ExchangeEmailAddress mail in emails)
                {
                    ListItem li = new ListItem();
                    li.Text = mail.EmailAddress;
                    li.Value = mail.EmailAddress;
                    li.Selected = mail.IsPrimary;
                    ddlSipAddresses.Items.Add(li);
                }

                foreach (ListItem li in ddlSipAddresses.Items)
                {
                    if (li.Value == sipAddressToSelect)
                    {
                        ddlSipAddresses.ClearSelection();
                        li.Selected = true;
                        break;
                    }
                }
            }
            else
            {
                email.Visible = true;
                ddlSipAddresses.Visible = false;

                if (!string.IsNullOrEmpty(sipAddressToSelect))
                {
                    string[] Tmp = sipAddressToSelect.Split('@');
                    email.AccountName = Tmp[0];

                    if (Tmp.Length > 1)
                    {
                        email.DomainName = Tmp[1];
                    }
                }


            }

		}
    }
}
