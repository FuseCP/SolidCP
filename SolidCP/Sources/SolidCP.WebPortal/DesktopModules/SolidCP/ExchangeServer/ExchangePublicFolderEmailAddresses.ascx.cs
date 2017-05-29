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
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer
{
	public partial class ExchangePublicFolderEmailAddresses : SolidCPModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				BindEmails();
			}
		}

        private void BindEmails()
        {
			ExchangeEmailAddress[] emails = ES.Services.ExchangeServer.GetPublicFolderEmailAddresses(
				PanelRequest.ItemID, PanelRequest.AccountID);

			gvEmails.DataSource = emails;
			gvEmails.DataBind();

			lblTotal.Text = emails.Length.ToString();

			// form title
			litDisplayName.Text = ES.Services.ExchangeServer.GetAccount(
				PanelRequest.ItemID, PanelRequest.AccountID).DisplayName;
        }

        protected void btnAddEmail_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                int result = ES.Services.ExchangeServer.AddPublicFolderEmailAddress(
					PanelRequest.ItemID, PanelRequest.AccountID, email.Email);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

				// rebind
				BindEmails();
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_PFOLDER_ADD_EMAIL", ex);
            }

			// clear field
			email.AccountName = "";
        }

        protected void btnSetAsPrimary_Click(object sender, EventArgs e)
        {
            try
            {
                string email = null;

                for (int i = 0; i < gvEmails.Rows.Count; i++)
                {
                    GridViewRow row = gvEmails.Rows[i];
                    CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        email = gvEmails.DataKeys[i].Value.ToString();
                        break;
                    }
                }

                if (email == null)
                    return;

                int result = ES.Services.ExchangeServer.SetPublicFolderPrimaryEmailAddress(
                    PanelRequest.ItemID, PanelRequest.AccountID, email);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

				// rebind
				BindEmails();
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_PFOLDER_SET_DEFAULT_EMAIL", ex);
            }
        }

        protected void btnDeleteAddresses_Click(object sender, EventArgs e)
        {
            // get selected e-mail addresses
            List<string> emails = new List<string>();

            for (int i = 0; i < gvEmails.Rows.Count; i++)
            {
                GridViewRow row = gvEmails.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect.Checked)
                    emails.Add(gvEmails.DataKeys[i].Value.ToString());
            }

            try
            {
                int result = ES.Services.ExchangeServer.DeletePublicFolderEmailAddresses(
                    PanelRequest.ItemID, PanelRequest.AccountID, emails.ToArray());

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

				// rebind
				BindEmails();
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_PFOLDER_DELETE_EMAILS", ex);
            }
        }
	}
}
