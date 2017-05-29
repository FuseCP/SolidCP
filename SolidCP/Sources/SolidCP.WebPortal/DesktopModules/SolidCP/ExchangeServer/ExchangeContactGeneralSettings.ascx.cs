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
using System.Web.UI.WebControls;
using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeContactGeneralSettings : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindMapiRichTextFormat();
                BindSettings();
            }

            
        }

        private void BindMapiRichTextFormat()
        {
            ddlMAPIRichTextFormat.Items.Clear();


            ddlMAPIRichTextFormat.Items.Add(new ListItem(GetLocalizedString("MAPIRichTextFormat.Always"), "1"));
            ddlMAPIRichTextFormat.Items.Add(new ListItem(GetLocalizedString("MAPIRichTextFormat.Never"), "0"));
            ddlMAPIRichTextFormat.Items.Add(new ListItem(GetLocalizedString("MAPIRichTextFormat.Default"), "2"));
        }
        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangeContact contact = ES.Services.ExchangeServer.GetContactGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

                litDisplayName.Text = PortalAntiXSS.Encode(contact.DisplayName);

                // bind form
                txtDisplayName.Text = contact.DisplayName;
                txtEmail.Text = contact.EmailAddress;
                chkHideAddressBook.Checked = contact.HideFromAddressBook;

                txtFirstName.Text = contact.FirstName;
                txtInitials.Text = contact.Initials;
                txtLastName.Text = contact.LastName;

                txtJobTitle.Text = contact.JobTitle;
                txtCompany.Text = contact.Company;
                txtDepartment.Text = contact.Department;
                txtOffice.Text = contact.Office;
                manager.SetAccount(contact.ManagerAccount);

                txtBusinessPhone.Text = contact.BusinessPhone;
                txtFax.Text = contact.Fax;
                txtHomePhone.Text = contact.HomePhone;
                txtMobilePhone.Text = contact.MobilePhone;
                txtPager.Text = contact.Pager;
                txtWebPage.Text = contact.WebPage;

                txtAddress.Text = contact.Address;
                txtCity.Text = contact.City;
                txtState.Text = contact.State;
                txtZip.Text = contact.Zip;
                country.Country = contact.Country;
                txtNotes.Text = contact.Notes;
                ddlMAPIRichTextFormat.SelectedValue = contact.UseMapiRichTextFormat.ToString();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_CONTACT_SETTINGS", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                int result = ES.Services.ExchangeServer.SetContactGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID,
                    txtDisplayName.Text,
                    txtEmail.Text,
                    chkHideAddressBook.Checked,

                    txtFirstName.Text,
                    txtInitials.Text,
                    txtLastName.Text,

                    txtAddress.Text,
                    txtCity.Text,
                    txtState.Text,
                    txtZip.Text,
                    country.Country,

                    txtJobTitle.Text,
                    txtCompany.Text,
                    txtDepartment.Text,
                    txtOffice.Text,
                    manager.GetAccount(),

                    txtBusinessPhone.Text,
                    txtFax.Text,
                    txtHomePhone.Text,
                    txtMobilePhone.Text,
                    txtPager.Text,
                    txtWebPage.Text,
                    txtNotes.Text,
                    Utils.ParseInt(ddlMAPIRichTextFormat.SelectedValue, 2/*  UseDefaultSettings */));

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                litDisplayName.Text = PortalAntiXSS.Encode(txtDisplayName.Text);

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_CONTACT_SETTINGS");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_CONTACT_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
