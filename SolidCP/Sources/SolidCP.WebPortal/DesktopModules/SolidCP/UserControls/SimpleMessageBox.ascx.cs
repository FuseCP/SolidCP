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
using System.Text;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;

namespace SolidCP.Portal.UserControls
{
    public partial class SimpleMessageBox : SolidCPControlBase
    {        

        protected void Page_Load(object sender, EventArgs e)
        {
            if(ViewState["JustRendered"] == null)
                divMessageBox.Visible = false;

            // reset flag to hide message next time
            ViewState["JustRendered"] = null;
        }

		public void ShowResultMessage(int resultCode)
		{
			ShowResultMessage(Utils.ModuleName, resultCode);
		}

		public void ShowResultMessage(string moduleName, int resultCode)
		{
			MessageBoxType messageType = MessageBoxType.Warning;

			// try to get warning
			string sCode = Convert.ToString(resultCode * -1);
			string localizedMessage = GetSharedLocalizedString(moduleName, "Warning." + sCode);
			string localizedDescription = GetSharedLocalizedString(moduleName, "WarningDescription." + sCode);

			if (localizedMessage == null)
			{
				messageType = MessageBoxType.Error;

				// try to get error
				localizedMessage = GetSharedLocalizedString(moduleName, "Error." + sCode);
				localizedDescription = GetSharedLocalizedString(moduleName, "ErrorDescription." + sCode);

				if (localizedMessage == null)
				{
					localizedMessage = GetSharedLocalizedString(moduleName, "Message.Generic") + " " + resultCode.ToString();
				}
			}

			// check if this is a "demo" message and it is overriden
			if (resultCode == BusinessErrorCodes.ERROR_USER_ACCOUNT_DEMO)
			{
				UserSettings scpSettings = UsersHelper.GetCachedUserSettings(
					PanelSecurity.EffectiveUserId, UserSettings.SolidCP_POLICY);
				if (!String.IsNullOrEmpty(scpSettings["DemoMessage"]))
				{
					localizedDescription = scpSettings["DemoMessage"];
				}
			}

			// render message
			RenderMessage(messageType, localizedMessage, localizedDescription, null);
		}

        public void ShowErrorMessage(string messageKey)
        {
			ShowErrorMessage(messageKey, null);
        }

        public void ShowErrorMessage(string messageKey, Exception ex)
        {
			RenderMessage(MessageBoxType.Error,
				GetLocalizedMessage("Error.", messageKey),
				GetLocalizedMessage("ErrorDescription.", messageKey), ex);
        }

        public void ShowWarningMessage(string messageKey)
        {
			RenderMessage(MessageBoxType.Warning,
				GetLocalizedMessage("Warning.", messageKey),
				GetLocalizedMessage("WarningDescription.", messageKey), null);
        }

        public void ShowSuccessMessage(string messageKey)
        {
			RenderMessage(MessageBoxType.Information,
				GetLocalizedMessage("Success.", messageKey),
				GetLocalizedMessage("SuccessDescription.", messageKey), null);
        }


        public void ShowMessage(ResultObject resultObject, string messageKey, string errorMessageKeyPrefix)
        {
            if (resultObject.IsSuccess)
            {
                if (resultObject.ErrorCodes.Count == 0)
                    ShowSuccessMessage(messageKey);
                else 
                    RenderMessage(resultObject.ErrorCodes.ToArray(), MessageBoxType.Warning, messageKey, errorMessageKeyPrefix );
            }
            else
            {
                RenderMessage(resultObject.ErrorCodes.ToArray(), MessageBoxType.Error, messageKey, errorMessageKeyPrefix);
            }

        }

		private string GetLocalizedMessage(string prefix, string messageKey)
		{
			string localizedText = GetSharedLocalizedString(prefix + messageKey);
			return localizedText == null ? "" : localizedText;
		}

		public void RenderMessage(string[] messages,MessageBoxType messageType, string messageKey, string  errorMessageKeyPrefix )
		{
            divMessageBox.Visible = true;
            ViewState["JustRendered"] = true;

            
            string prefix = "Success.";
            // set icon and styles
            string boxStyle = "MessageBox Green";

            if (messageType == MessageBoxType.Warning)
            {
                boxStyle = "MessageBox Yellow";
                prefix = "Warning.";
            }
            else if (messageType == MessageBoxType.Error)
            {
                boxStyle = "MessageBox Red";
                prefix = "Error.";
            }

            divMessageBox.Attributes["class"] = boxStyle;

            string localizedMsg = GetSharedLocalizedString(prefix + messageKey);
            if(String.IsNullOrEmpty(localizedMsg))
                localizedMsg = messageKey;

            litMessage.Text = localizedMsg;

            // detailed messages
            StringBuilder sb = new StringBuilder();
            foreach (string str in messages)
		    {
                string key = str;
                string[] parts = null;
                int idx = str.IndexOf(":");
                if (idx != -1)
                {
                    parts = new string[] { str.Substring(0, idx), str.Substring(idx + 1) };
                    key = parts[0];
                }

                // first attempt
                string localizedStr = GetSharedLocalizedString(string.Format("{0}{1}", prefix, key));

                if(localizedStr == null)
                    localizedStr = GetSharedLocalizedString(string.Format("{0}{1}", "Warning.", key));

                // second attempt
                if(localizedStr == null)
                    localizedStr = GetSharedLocalizedString(string.Format("{0}.{1}", errorMessageKeyPrefix, key));

                if (parts != null && localizedStr != null)
                    localizedStr = String.Format(localizedStr, parts);

                if (String.IsNullOrEmpty(localizedStr))
                    localizedStr = str;

		        sb.Append("- ");
                sb.Append(localizedStr);
		        sb.Append("<br/>");
		    }

            string description = sb.ToString();
            litDescription.Text = !String.IsNullOrEmpty(description)
                ? String.Format("<br/><span class=\"description\">{0}</span>", description) : "";
		}
        
        public void RenderMessage(MessageBoxType messageType, string message, string description, Exception ex)
		{
			divMessageBox.Visible = true;
            ViewState["JustRendered"] = true;

			// set icon and styles
			string boxStyle = "MessageBox Green";

			if (messageType == MessageBoxType.Warning)
			{
                boxStyle = "MessageBox Yellow";
			}
			else if (messageType == MessageBoxType.Error)
			{
                boxStyle = "MessageBox Red";
			}

			divMessageBox.Attributes["class"] = boxStyle;

			// set texts
			litMessage.Text = message;

            // error
            if (ex != null)
            {
                description += "<br><br>" + ex.Message;
            }

            litDescription.Text = !String.IsNullOrEmpty(description)
                ? String.Format("<br/><span class=\"description\">{0}</span>", description) : "";
		}
    }
}
