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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.Providers.Mail;

namespace SolidCP.Portal.ProviderControls
{
	public partial class hMailServer5_EditAccount : SolidCPControlBase, IMailEditAccountControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public void BindItem(MailAccount item)
		{
            chkEnabled.Checked = item.Enabled;
            lblSizeInfo.Text = item.Size.ToString() + " MB";
            lblQuotaUsedInfo.Text = item.QuotaUsed.ToString() + " %";
            lblLastLoginDateInfo.Text = item.LastLogonTime;
            chkResponderEnabled.Checked = item.ResponderEnabled;
            chkResponderExpires.Checked = item.ResponderExpires;
            txtResponderExireDate.Text = item.ResponderExpirationDate;
			txtSubject.Text = item.ResponderSubject;
			txtMessage.Text = item.ResponderMessage;
            chkForwardingEnabled.Checked = item.ForwardingEnabled;
			txtForward.Text = item.ForwardingAddresses[0];
            chkOriginalMessage.Checked = item.RetainLocalCopy;
            txtFirstName.Text = item.FirstName;
            txtLastName.Text = item.LastName;
            cbSignatureEnabled.Checked = item.SignatureEnabled;
            txtPlainSignature.Text = item.Signature;
            txtHtmlSignature.Text = item.SignatureHTML;
            secStatusInfo.IsCollapsed = false;            
		}

		public void SaveItem(MailAccount item)
		{
            item.Enabled = chkEnabled.Checked;
			item.ResponderEnabled = chkResponderEnabled.Checked;
			item.ResponderSubject = txtSubject.Text;
			item.ResponderMessage = txtMessage.Text;
            item.ResponderExpires = chkResponderExpires.Checked;
            if (txtResponderExireDate.Text.Trim().Length >= 10) {
                item.ResponderExpirationDate = txtResponderExireDate.Text.Trim().Substring(0, 10);}
            item.ForwardingEnabled = chkForwardingEnabled.Checked;
            if (txtForward.Text.Trim().Length > 0) {
                item.ForwardingAddresses = new string[] { txtForward.Text.Trim() };}
            item.RetainLocalCopy = chkOriginalMessage.Checked;
            item.FirstName = txtFirstName.Text;
            item.LastName = txtLastName.Text;
            item.SignatureEnabled = cbSignatureEnabled.Checked;
            item.Signature = txtPlainSignature.Text;
            item.SignatureHTML = txtHtmlSignature.Text;
		}
	}
}
