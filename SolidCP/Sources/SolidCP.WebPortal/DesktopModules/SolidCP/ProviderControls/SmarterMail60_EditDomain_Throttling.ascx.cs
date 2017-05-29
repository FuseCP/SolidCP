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
using SolidCP.Providers.Mail;

namespace SolidCP.Portal.ProviderControls
{
    public partial class SmarterMail60_EditDomain_Throttling : SolidCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitValidators();
        }

        public void SaveItem(MailDomain item)
        {
            item[MailDomain.SMARTERMAIL5_MESSAGES_PER_HOUR] = txtMessagesPerHour.Text;
            item[MailDomain.SMARTERMAIL5_MESSAGES_PER_HOUR_ENABLED] = cbMessagesPerHour.Checked.ToString();
            item[MailDomain.SMARTERMAIL5_BANDWIDTH_PER_HOUR] = txtBandwidthPerHour.Text;
            item[MailDomain.SMARTERMAIL5_BANDWIDTH_PER_HOUR_ENABLED] = cbBandwidthPerHour.Checked.ToString();
            item[MailDomain.SMARTERMAIL5_BOUNCES_PER_HOUR] = txtBouncesPerHour.Text;
            item[MailDomain.SMARTERMAIL5_BOUNCES_PER_HOUR_ENABLED] = cbBouncesPerHour.Checked.ToString();
        }

        public void BindItem(MailDomain item)
        {
            txtMessagesPerHour.Text = item[MailDomain.SMARTERMAIL5_MESSAGES_PER_HOUR];
            cbMessagesPerHour.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL5_MESSAGES_PER_HOUR_ENABLED]);
            txtBandwidthPerHour.Text = item[MailDomain.SMARTERMAIL5_BANDWIDTH_PER_HOUR];
            cbBandwidthPerHour.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL5_BANDWIDTH_PER_HOUR_ENABLED]);
            txtBouncesPerHour.Text = item[MailDomain.SMARTERMAIL5_BOUNCES_PER_HOUR];
            cbBouncesPerHour.Checked = Convert.ToBoolean(item[MailDomain.SMARTERMAIL5_BOUNCES_PER_HOUR_ENABLED]);
        }

        public void InitValidators()
        {
            string message = "*";

            reqValMessagesPerHour.ErrorMessage = message;
            valMessagesPerHour.ErrorMessage = message;
            valMessagesPerHour.MaximumValue = int.MaxValue.ToString();

            reqValBandwidth.ErrorMessage = message;
            valBandwidthPerHour.ErrorMessage = message;
            valBandwidthPerHour.MaximumValue = int.MaxValue.ToString();

            reqValBouncesPerHour.ErrorMessage = message;
            valBouncesPerHour.ErrorMessage = message;
            valBouncesPerHour.MaximumValue = int.MaxValue.ToString();
        }
    }

}
