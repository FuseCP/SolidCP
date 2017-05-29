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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SolidCP.EnterpriseServer;
using SolidCP.Portal.Code.Framework;
using SolidCP.Portal.UserControls.ScheduleTaskView;

namespace SolidCP.Portal.ScheduleTaskControls
{
	public partial class CheckWebsite : EmptyView
	{
		private static readonly string UrlParameter = "URL";
		private static readonly string AccessUsernameParameter = "USERNAME";
		private static readonly string AccessPasswordParameter = "PASSWORD";
		private static readonly string UseSendMessageIfResponseStatusParameter = "USE_RESPONSE_STATUS";
		private static readonly string UseSendMessageIfResponseContainsParameter = "USE_RESPONSE_CONTAIN";
		private static readonly string UseSendMessageIfResponseDoesntContainParameter = "USE_RESPONSE_DOESNT_CONTAIN";
		private static readonly string SendMessageIfResponseStatusParameter = "RESPONSE_STATUS";
		private static readonly string SendMessageIfResponseContainsParameter = "RESPONSE_CONTAIN";
		private static readonly string SendMessageIfResponseDoesntContainParameter = "RESPONSE_DOESNT_CONTAIN";
		private static readonly string MailFromParameter = "MAIL_FROM";
		private static readonly string MailToParameter = "MAIL_TO";
		private static readonly string MailSubjectParameter = "MAIL_SUBJECT";
		private static readonly string MailBodyParameter = "MAIL_BODY";

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

			this.SetParameter(this.txtUrl, UrlParameter);
			this.SetParameter(this.txtAccessUsername, AccessUsernameParameter);
			this.SetParameter(this.txtAccessPassword, AccessPasswordParameter);
			this.SetParameter(this.cbxResponseStatus, UseSendMessageIfResponseStatusParameter);
			this.SetParameter(this.cbxResponseContains, UseSendMessageIfResponseContainsParameter);
			this.SetParameter(this.cbxResponseDoesntContain, UseSendMessageIfResponseDoesntContainParameter);
			this.SetParameter(this.txtResponseStatus, SendMessageIfResponseStatusParameter);
			this.SetParameter(this.txtResponseContains, SendMessageIfResponseContainsParameter);
			this.SetParameter(this.txtResponseDoesntContain, SendMessageIfResponseDoesntContainParameter);
			this.SetParameter(this.txtMailFrom, MailFromParameter);
			this.SetParameter(this.txtMailTo, MailToParameter);
			this.SetParameter(this.txtMailSubject, MailSubjectParameter);
			this.SetParameter(this.txtMailBody, MailBodyParameter);
		}

		/// <summary>
		/// Gets scheduler task parameters from view.
		/// </summary>
		/// <returns>Parameters list filled  from view.</returns>
		public override ScheduleTaskParameterInfo[] GetParameters()
		{
			ScheduleTaskParameterInfo url = this.GetParameter(this.txtUrl, UrlParameter);
			ScheduleTaskParameterInfo accessUsername = this.GetParameter(this.txtAccessUsername, AccessUsernameParameter);
			ScheduleTaskParameterInfo accessPassword = this.GetParameter(this.txtAccessPassword, AccessPasswordParameter);
			ScheduleTaskParameterInfo useSendMessageIfResponseStatus = this.GetParameter(this.cbxResponseStatus, UseSendMessageIfResponseStatusParameter);
			ScheduleTaskParameterInfo useSendMessageIfResponseContains = this.GetParameter(this.cbxResponseContains, UseSendMessageIfResponseContainsParameter);
			ScheduleTaskParameterInfo useSendMessageIfResponseDoesntContain = this.GetParameter(this.cbxResponseDoesntContain, UseSendMessageIfResponseDoesntContainParameter);
			ScheduleTaskParameterInfo sendMessageIfResponseStatus = this.GetParameter(this.txtResponseStatus, SendMessageIfResponseStatusParameter);
			ScheduleTaskParameterInfo sendMessageIfResponseContains = this.GetParameter(this.txtResponseContains, SendMessageIfResponseContainsParameter);
			ScheduleTaskParameterInfo sendMessageIfResponseDoesntContain = this.GetParameter(this.txtResponseDoesntContain, SendMessageIfResponseDoesntContainParameter);
			ScheduleTaskParameterInfo mailFrom = this.GetParameter(this.txtMailFrom, MailFromParameter);
			ScheduleTaskParameterInfo mailTo = this.GetParameter(this.txtMailTo, MailToParameter);
			ScheduleTaskParameterInfo mailSubject = this.GetParameter(this.txtMailSubject, MailSubjectParameter);
			ScheduleTaskParameterInfo mailBody = this.GetParameter(this.txtMailBody, MailBodyParameter);

			return new ScheduleTaskParameterInfo[13] { url, accessUsername, accessPassword, sendMessageIfResponseStatus, sendMessageIfResponseContains, sendMessageIfResponseDoesntContain, useSendMessageIfResponseStatus, useSendMessageIfResponseContains, useSendMessageIfResponseDoesntContain, mailFrom, mailTo, mailSubject, mailBody };

		}
	}
}
