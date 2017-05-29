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
using System.Xml;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.EnterpriseServer
{
	public class SystemController
	{
		private SystemController()
		{
		}

		public static SystemSettings GetSystemSettings(string settingsName)
		{
			// check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin | DemandAccount.IsActive);
			if (accountCheck < 0)
				return null;

			bool isDemoAccount = (SecurityContext.CheckAccount(DemandAccount.NotDemo) < 0);

			return GetSystemSettingsInternal(settingsName, !isDemoAccount);
		}

        public static SystemSettings GetSystemSettingsActive(string settingsName, bool decrypt)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsActive);
            if (accountCheck < 0)
                return null;

            bool isDemoAccount = (SecurityContext.CheckAccount(DemandAccount.NotDemo) < 0);

            return GetSystemSettingsInternal(settingsName, decrypt && isDemoAccount);
        }

		internal static SystemSettings GetSystemSettingsInternal(string settingsName, bool decryptPassword)
		{
			// create settings object
			SystemSettings settings = new SystemSettings();

			// get service settings
			IDataReader reader = null;

			try
			{
				// get service settings
				reader = DataProvider.GetSystemSettings(settingsName);

				while (reader.Read())
				{
					string name = (string)reader["PropertyName"];
					string val = (string)reader["PropertyValue"];

					if (name.ToLower().IndexOf("password") != -1 && decryptPassword)
						val = CryptoUtils.Decrypt(val);

					settings[name] = val;
				}


			}
			finally
			{
				if (reader != null && !reader.IsClosed)
					reader.Close();
			}

			return settings;
		}

		public static int SetSystemSettings(string settingsName, SystemSettings settings)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
				| DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			XmlDocument xmldoc = new XmlDocument();
			XmlElement root = xmldoc.CreateElement("properties");

			foreach (string[] pair in settings.SettingsArray)
			{
				string name = pair[0];
				string val = pair[1];

				if (name.ToLower().IndexOf("password") != -1)
					val = CryptoUtils.Encrypt(val);

				XmlElement property = xmldoc.CreateElement("property");

				property.SetAttribute("name", name);
				property.SetAttribute("value", val);

				root.AppendChild(property);
			}

			DataProvider.SetSystemSettings(settingsName, root.OuterXml);

			return 0;
		}

		public static bool GetSystemSetupMode()
		{
			var scpaSystemSettings = GetSystemSettings(SystemSettings.SETUP_SETTINGS);
			// Flag either not found or empty
			if (String.IsNullOrEmpty(scpaSystemSettings["EnabledSCPA"]))
			{
				return false;
			}
			//
			return true;
		}

		public static int SetupControlPanelAccounts(string passwordA, string passwordB, string ip)
		{
			try
			{
				TaskManager.StartTask("SYSTEM", "COMPLETE_SCPA");
				//
				TaskManager.WriteParameter("Password A", passwordA);
				TaskManager.WriteParameter("Password B", passwordB);
				TaskManager.WriteParameter("IP Address", ip);
				//
				var enabledScpaMode = GetSystemSetupMode();
				//
				if (enabledScpaMode == false)
				{
					//
					TaskManager.WriteWarning("Attempt to execute SCPA procedure for an uknown reason");
					//
					return BusinessErrorCodes.FAILED_EXECUTE_SERVICE_OPERATION;
				}

				// Entering the security context into Supervisor mode
				SecurityContext.SetThreadSupervisorPrincipal();
				//
				var accountA = UserController.GetUserInternally("serveradmin");
				var accountB = UserController.GetUserInternally("admin");
				//
				var resultCodeA = UserController.ChangeUserPassword(accountA.UserId, passwordA);
				//
				if (resultCodeA < 0)
				{
					TaskManager.WriteParameter("Result Code A", resultCodeA);
					//
					return resultCodeA;
				}
				//
				var resultCodeB = UserController.ChangeUserPassword(accountB.UserId, passwordB);
				//
				if (resultCodeB < 0)
				{
					TaskManager.WriteParameter("Result Code B", resultCodeB);
					//
					return resultCodeB;
				}
				// Disable SCPA mode
				SetSystemSettings(SystemSettings.SETUP_SETTINGS, SystemSettings.Empty);
				// Operation has succeeded
				return 0;
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				//
				return BusinessErrorCodes.FAILED_EXECUTE_SERVICE_OPERATION;
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

        public static bool CheckIsTwilioEnabled()
        {
            var settings = SystemController.GetSystemSettingsActive(SystemSettings.TWILIO_SETTINGS, false);

            return settings != null
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(SystemSettings.TWILIO_ACCOUNTSID_KEY, string.Empty))
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(SystemSettings.TWILIO_AUTHTOKEN_KEY, string.Empty))
                && !string.IsNullOrEmpty(settings.GetValueOrDefault(SystemSettings.TWILIO_PHONEFROM_KEY, string.Empty));
        }
	}
}
