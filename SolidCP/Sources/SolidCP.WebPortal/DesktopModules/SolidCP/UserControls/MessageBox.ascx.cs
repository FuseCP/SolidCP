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
using System.Text;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Web.Services.Protocols;

namespace SolidCP.Portal
{
    public partial class MessageBox : SolidCPControlBase, IMessageBoxControl, INamingContainer
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Visible = false;
            if (ViewState["ShowNextTime"] != null)
            {
                this.Visible = true;
                ViewState["ShowNextTime"] = null;
            }
        }

        public void RenderMessage(MessageBoxType messageType, string message, string description,
            Exception ex, params string[] additionalParameters)
        {
            this.Visible = true; // show message

            // set icon and styles
            string boxStyle = "MessageBox Green";
            if (messageType == MessageBoxType.Warning)
                boxStyle = "MessageBox Yellow";
            else if (messageType == MessageBoxType.Error)
                boxStyle = "MessageBox Red";

            tblMessageBox.Attributes["class"] = boxStyle;

            // set texts
            litMessage.Text = message;
            litDescription.Text = !String.IsNullOrEmpty(description)
                ? String.Format("<br/><span class=\"description\">{0}</span>", description) : "";

            // show exception
            if (ex != null)
            {
                // show error
                try
                {
                    // technical details
                    litPageUrl.Text = PortalAntiXSS.Encode(Request.Url.ToString());
                    litLoggedUser.Text = PanelSecurity.LoggedUser.Username;
                    litSelectedUser.Text = PanelSecurity.SelectedUser.Username;
                    litPackageName.Text = PanelSecurity.PackageId.ToString();
                    litStackTrace.Text = ex.ToString().Replace("\n", "<br/>");

                    // send form
                    litSendFrom.Text = PanelSecurity.LoggedUser.Email;

                    if (!String.IsNullOrEmpty(PortalUtils.FromEmail))
                        litSendFrom.Text = PortalUtils.FromEmail;

                    //litSendTo.Text = this.PortalSettings.Email;
                    litSendTo.Text = PortalUtils.AdminEmail;
                    litSendCC.Text = PanelSecurity.LoggedUser.Email;
                    litSendSubject.Text = GetLocalizedString("Text.Subject");
                }
                catch { /* skip */ }
            }
            else
            {
                rowTechnicalDetails.Visible = false;
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            EnableViewState = true;
            ViewState["ShowNextTime"] = true;

            StringBuilder sb = new StringBuilder();
            sb.Append("Page URL: ").Append(litPageUrl.Text).Append("\n\n");
            sb.Append("Logged User: ").Append(litLoggedUser.Text).Append("\n\n");
            sb.Append("Selected User: ").Append(litSelectedUser.Text).Append("\n\n");
            sb.Append("Package ID: ").Append(litPackageName.Text).Append("\n\n");
            sb.Append("Stack Trace: ").Append(litStackTrace.Text.Replace("<br/>", "\n")).Append("\n\n");
            sb.Append("Personal Comments: ").Append(txtSendComments.Text).Append("\n\n");

            try
            {
                btnSend.Visible = false;
                lblSentMessage.Visible = true;

                // send mail
                PortalUtils.SendMail(litSendFrom.Text, litSendTo.Text, litSendFrom.Text,
                    litSendSubject.Text, sb.ToString());

                lblSentMessage.Text = GetLocalizedString("Text.MessageSent");
            }
            catch
            {
                lblSentMessage.Text = GetLocalizedString("Text.MessageSentError");
            }
        }
    }
}
