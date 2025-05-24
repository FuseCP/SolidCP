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
using SolidCP.EnterpriseServer;
using SolidCP.Portal.UserControls.ScheduleTaskView;
using SCP = SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ScheduleTaskControls
{
    public partial class CheckWebsitesSslView : EmptyView
    {
        private static readonly string sendMailToCustomerParameter = "SEND_MAIL_TO_CUSTOMER";
        private static readonly string sendBccParameter = "SEND_BCC";
        private static readonly string bccMailParameter = "BCC_MAIL";
        private static readonly string expirationMailSubjectParameter = "EXPIRATION_MAIL_SUBJECT";
        private static readonly string expirationMailBodyParameter = "EXPIRATION_MAIL_BODY";
        private static readonly string send30DaysBeforeExpirationParameter = "SEND_30_DAYS_BEFORE_EXPIRATION";
        private static readonly string send14DaysBeforeExpirationParameter = "SEND_14_DAYS_BEFORE_EXPIRATION";
        private static readonly string sendTodayExpiredParameter = "SEND_TODAY_EXPIRED";
        private static readonly string sendSslErrorParameter = "SEND_SSL_ERROR";
        private static readonly string errorMailSubjectParameter = "ERROR_MAIL_SUBJECT";
        private static readonly string errorMailBodyParameter = "ERROR_MAIL_BODY";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Sets scheduler task parameters on view.
        /// </summary>
        /// <param name="parameters">Parameters list to be set on view.</param>
        public override void SetParameters(ScheduleTaskParameterInfo[] parameters)
        {
            base.SetParameters(parameters);

            SCP.SystemSettings settings = ES.Services.System.GetSystemSettingsActive(SCP.SystemSettings.SMTP_SETTINGS, false);
            if (settings != null)
            {
                txtMailFrom.Text = settings["SmtpUsername"];
            }
            if (String.IsNullOrEmpty(txtMailFrom.Text))
            {
                txtMailFrom.Text = GetLocalizedString("SMTPWarning.Text");
            }

            SetParameter(cbMailToCustomer, sendMailToCustomerParameter);
            SetParameter(cbSendBcc, sendBccParameter);
            SetParameter(txtBccMail, bccMailParameter);
            SetParameter(txtExpirationMailSubject, expirationMailSubjectParameter);
            SetParameter(txtExpirationMailBody, expirationMailBodyParameter);
            SetParameter(cbSend30DaysBeforeExpiration, send30DaysBeforeExpirationParameter);
            SetParameter(cbSend14DaysBeforeExpiration, send14DaysBeforeExpirationParameter);
            SetParameter(cbSendTodayExpired, sendTodayExpiredParameter);
            SetParameter(cbSendSslError, sendSslErrorParameter);
            SetParameter(txtErrorMailSubject, errorMailSubjectParameter);
            SetParameter(txtErrorMailBody, errorMailBodyParameter);
        }

        /// <summary>
        /// Gets scheduler task parameters from view.
        /// </summary>
        /// <returns>Parameters list filled  from view.</returns>
        public override ScheduleTaskParameterInfo[] GetParameters()
        {
            ScheduleTaskParameterInfo mailToCustomer = GetParameter(cbMailToCustomer, sendMailToCustomerParameter);
            ScheduleTaskParameterInfo sendBcc = GetParameter(cbSendBcc, sendBccParameter);
            ScheduleTaskParameterInfo bccMail = GetParameter(txtBccMail, bccMailParameter);
            ScheduleTaskParameterInfo expirationMailSubject = GetParameter(txtExpirationMailSubject, expirationMailSubjectParameter);
            ScheduleTaskParameterInfo expirationMailBody = GetParameter(txtExpirationMailBody, expirationMailBodyParameter);
            ScheduleTaskParameterInfo send30DaysBeforeExpiration = GetParameter(cbSend30DaysBeforeExpiration, send30DaysBeforeExpirationParameter);
            ScheduleTaskParameterInfo send14DaysBeforeExpiration = GetParameter(cbSend14DaysBeforeExpiration, send14DaysBeforeExpirationParameter);
            ScheduleTaskParameterInfo sendTodayExpired = GetParameter(cbSendTodayExpired, sendTodayExpiredParameter);
            ScheduleTaskParameterInfo sendSslError = GetParameter(cbSendSslError, sendSslErrorParameter);
            ScheduleTaskParameterInfo errorMailSubject = GetParameter(txtErrorMailSubject, errorMailSubjectParameter);
            ScheduleTaskParameterInfo errorMailBody = GetParameter(txtErrorMailBody, errorMailBodyParameter);

            return new ScheduleTaskParameterInfo[11] { mailToCustomer, sendBcc, bccMail, expirationMailSubject, expirationMailBody, send30DaysBeforeExpiration,
                send14DaysBeforeExpiration, sendTodayExpired, sendSslError, errorMailSubject, errorMailBody};

        }

        private string GetLocalizedString(string resourceKey)
        {
            return (string)GetLocalResourceObject(resourceKey);
        }
    }
}