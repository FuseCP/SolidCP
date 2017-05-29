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
    public partial class MailEnable_EditList : SolidCPControlBase, IMailEditListControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           HeaderFooterSection.Visible = pHeaderFooter.Visible = (PanelRequest.ItemID > 0);
        }

        public void BindItem(MailList item)
        {
            Utils.SelectListItem(ddlReplyTo, item.ReplyToMode);
            Utils.SelectListItem(ddlPostingMode, item.PostingMode);
            Utils.SelectListItem(ddlPrefixOption, item.PrefixOption);
            txtSubjectPrefix.Text = item.SubjectPrefix;
            chkModerationEnabled.Checked = item.Moderated;
            txtModeratorEmail.Text = item.ModeratorAddress;
            mailEditItems.Items = item.Members;
            if (!String.IsNullOrEmpty(item.Password))
            {
                txtPassword.Attributes["value"] = txtPassword.Text = item.Password;
            }
            else
            {
                txtPassword.Text = "";          
            }
            txtDescription.Text = item.Description;
      
            switch (item.AttachHeader)
            {
                case 1:
                    cbHeader.Checked = true;
                    break;
                case 0:
                    cbHeader.Checked = false;
                    break;
            }
            switch (item.AttachFooter)
            {
                case 1:
                    cbFooter.Checked = true;
                    break;
                case 0:
                    cbFooter.Checked = false;
                    break;
            }
            txtHeaderText.Text = item.TextHeader;
            txtFooterText.Text = item.TextFooter;
            txtHeaderHtml.Text = item.HtmlHeader;
            txtFooterHtml.Text = item.HtmlFooter;
        }



        public void SaveItem(MailList item)
        {
            item.ReplyToMode = (ReplyTo)Enum.Parse(typeof(ReplyTo), ddlReplyTo.SelectedValue, true);
            item.PostingMode = (PostingMode)Enum.Parse(typeof(PostingMode), ddlPostingMode.SelectedValue, true);
            item.PrefixOption = (PrefixOption) Enum.Parse(typeof (PrefixOption), ddlPrefixOption.SelectedValue, true);
            item.SubjectPrefix = txtSubjectPrefix.Text;
            item.Description = txtDescription.Text;
            // save password
            item.Password = (txtPassword.Text.Length > 0) ? txtPassword.Text : (string)ViewState["PWD"];
            item.Moderated = chkModerationEnabled.Checked;
            item.ModeratorAddress = txtModeratorEmail.Text;
            item.Members = mailEditItems.Items;
            item.AttachHeader = (cbHeader.Checked) ? 1 : 0;
            item.AttachFooter = (cbFooter.Checked) ? 1 : 0;
            item.TextHeader = txtHeaderText.Text;
            item.TextFooter = txtFooterText.Text;
            item.HtmlHeader = txtHeaderHtml.Text;
            item.HtmlFooter = txtFooterHtml.Text;
        }
    }
}
