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
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public class SolidCPModuleBase : SolidCPControlBase
    {
        private IMessageBoxControl messageBox;

        public SolidCPModuleBase()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            // add message box control
            messageBox = (IMessageBoxControl)this.LoadControl(
                PanelGlobals.SolidCPRootPath + "UserControls/MessageBox.ascx");
            this.Controls.AddAt(0, (Control)messageBox);
            ((Control)messageBox).Visible = false;

            // call base handler
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            //Page.MaintainScrollPositionOnPostBack = true;

            // call base handler
            base.OnLoad(e);
        }

        public void SwitchUser(object arg)
        {
            //PanelSecurity.SelectedUserId = Utils.ParseInt(arg.ToString(), PanelSecurity.EffectiveUserId);
            RedirectToBrowsePage();
        }

        public void SwitchPackage(object arg)
        {
            string[] args = arg.ToString().Split(',');

            //PanelSecurity.SelectedUserId = Utils.ParseInt(args[0], PanelSecurity.EffectiveUserId);
            //PanelSecurity.PackageId = Utils.ParseInt(args[1], 0);
            RedirectToBrowsePage();
        }

        public void LoadProviderControl(int packageId, string groupName, PlaceHolder container, string controlName)
        {
            string ctrlPath = null;
           //
			ProviderInfo provider = ES.Services.Servers.GetPackageServiceProvider(packageId, groupName);

            // try to locate suitable control
            string currPath = this.AppRelativeVirtualPath;
            currPath = currPath.Substring(0, currPath.LastIndexOf("/"));

            ctrlPath = currPath + "/ProviderControls/" + provider.EditorControl + "_" + controlName;

            Control ctrl = Page.LoadControl(ctrlPath);

            // add control to the placeholder
            container.Controls.Add(ctrl);
        }

        public void HideServiceColumns(GridView gv)
        {
            try
            {
                gv.Columns[gv.Columns.Count - 1].Visible =
                    (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
            }
            catch
            {
            }
        }

        #region Error Messages Processing
        public void ProcessException(Exception ex)
        {
            string authError = "The security token could not be authenticated or authorized";
            if (ex.Message.Contains(authError) ||
                (ex.InnerException != null &&
                ex.InnerException.Message.Contains(authError)))
            {
                ShowWarningMessage("ES_CONNECT");
            }
            else
            {
                ShowErrorMessage("MODULE_LOAD", ex);
            }

        }

		public virtual void ShowResultMessage(int resultCode)
		{
			ShowResultMessage(Utils.ModuleName, resultCode, false);
		}

		public virtual void ShowResultMessageWithContactForm(int resultCode)
		{
			ShowResultMessage(Utils.ModuleName, resultCode, true);
		}

		public void ShowResultMessage(string moduleName, int resultCode, params object[] formatArgs)
		{
			ShowResultMessage(moduleName, resultCode, false, formatArgs);
		}

        public void ShowResultMessage(string moduleName, int resultCode, bool showcf, params object[] formatArgs)
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
				else
				{
					if (formatArgs != null && formatArgs.Length > 0)
						localizedMessage = String.Format(localizedMessage, formatArgs);
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
			Exception fake_ex = null;
			// Contact form is requested to be shown
			if (showcf)
				fake_ex = new Exception();
			//
            messageBox.RenderMessage(messageType, localizedMessage, localizedDescription, fake_ex);
        }

		public virtual void ShowSuccessMessage(string messageKey)
		{
			ShowSuccessMessage(Utils.ModuleName, messageKey, null);
		}

        public void ShowSuccessMessage(string moduleName, string messageKey)
        {
            ShowSuccessMessage(moduleName, messageKey, null);
        }

        public virtual void ShowSuccessMessage(string moduleName, string messageKey, params string[] formatArgs)
        {
            string localizedMessage = GetSharedLocalizedString(moduleName, "Success." + messageKey);
            string localizedDescription = GetSharedLocalizedString(moduleName, "SuccessDescription." + messageKey);
            if (localizedMessage == null)
            {
                localizedMessage = messageKey;
            }
            else
            {
                //Format message string with args
                if (formatArgs != null && formatArgs.Length > 0)
                {
                    localizedMessage = String.Format(localizedMessage, formatArgs);
                }
            }
            // render message
            messageBox.RenderMessage(MessageBoxType.Information, localizedMessage, localizedDescription, null);
        }

		public virtual void ShowWarningMessage(string messageKey)
		{
			ShowWarningMessage(Utils.ModuleName, messageKey);
		}

        public void ShowWarningMessage(string moduleName, string messageKey)
        {
			string localizedMessage = GetSharedLocalizedString(moduleName, "Warning." + messageKey);
			string localizedDescription = GetSharedLocalizedString(moduleName, "WarningDescription." + messageKey);
            if (localizedMessage == null)
                localizedMessage = messageKey;

            // render message
            messageBox.RenderMessage(MessageBoxType.Warning, localizedMessage, localizedDescription, null);
        }

        public void ShowErrorMessage(string messageKey, params string[] additionalParameters)
        {
            ShowErrorMessage(messageKey, null, additionalParameters);
        }

		public virtual void ShowErrorMessage(string messageKey, Exception ex, params string[] additionalParameters)
		{
			ShowErrorMessage(Utils.ModuleName, messageKey, ex, additionalParameters);
		}

        public void ShowErrorMessage(string moduleName, string messageKey, Exception ex, params string[] additionalParameters)
        {
            string exceptionKey = null;
			//
            if (ex != null)
            {
				if (!String.IsNullOrEmpty(ex.Message) && ex.Message.Contains("SolidCP_ERROR"))
				{
					string[] messageParts = ex.Message.Split(new char[] { '@' });
					if (messageParts.Length > 1)
					{
						exceptionKey = messageParts[1].TrimStart(new char[] { ' ' });
					}
				}
            }
            string localizedMessage = GetSharedLocalizedString(moduleName, "Error." + exceptionKey);
			string localizedDescription = GetSharedLocalizedString(moduleName, "ErrorDescription." + exceptionKey);

            if (localizedMessage == null)
            {
                localizedMessage = GetSharedLocalizedString(moduleName, "Error." + messageKey);
                localizedDescription = GetSharedLocalizedString(moduleName, messageKey);
                if (localizedMessage == null)
                    localizedMessage = messageKey;
            }
            else
            {
                //render localized exception message without stack trace
                messageBox.RenderMessage(MessageBoxType.Error, localizedMessage, localizedDescription, null);
                return;
            }

            // render message
            messageBox.RenderMessage(MessageBoxType.Error, localizedMessage, localizedDescription, ex);
        }

       #endregion
    }
}
