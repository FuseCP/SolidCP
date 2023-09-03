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
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace SolidCP.EnterpriseServer
{
	[Serializable]
	public class SystemSettings
	{
		public const string SMTP_SETTINGS = "SmtpSettings";
		public const string BACKUP_SETTINGS = "BackupSettings";
		public const string SETUP_SETTINGS = "SetupSettings";
        public const string WPI_SETTINGS = "WpiSettings";
        public const string FILEMANAGER_SETTINGS = "FileManagerSettings";
        public const string PACKAGE_DISPLAY_SETTINGS = "PackageDisplaySettings";
        public const string RDS_SETTINGS = "RdsSettings";
        public const string WEBDAV_PORTAL_SETTINGS = "WebdavPortalSettings";
        public const string TWILIO_SETTINGS = "TwilioSettings";
        public const string ACCESS_IP_SETTINGS = "AccessIpsSettings";
		public const string AUTHENTICATION_SETTINGS = "AuthenticationSettings";

		//Keys
		public const string TWILIO_ACTIVE_KEY = "TwilioActive";
        public const string TWILIO_ACCOUNTSID_KEY = "TwilioAccountSid";
        public const string TWILIO_AUTHTOKEN_KEY = "TwilioAuthToken";
        public const string TWILIO_PHONEFROM_KEY = "TwilioPhoneFrom";

        public const string WEBDAV_PASSWORD_RESET_ENABLED_KEY = "WebdavPswResetEnabled";
        public const string WEBDAV_PASSWORD_RESET_LINK_LIFE_SPAN = "WebdavPswdResetLinkLifeSpan";
        public const string WEBDAV_OWA_ENABLED_KEY = "WebdavOwaEnabled";
        public const string WEBDAV_OWA_URL = "WebdavOwaUrl";

        // key to access to wpi main & custom feed in wpi settings
        public const string WPI_MAIN_FEED_KEY = "WpiMainFeedUrl";
        public const string FEED_ULS_KEY = "FeedUrls";

		//Mfa token app display name
		public const string MFA_TOKEN_APP_DISPLAY_NAME = "MfaTokenAppDisplayName";
		public const string MFA_CAN_PEER_CHANGE_MFA = "CanPeerChangeMfa";

		// Constant for IPAccess
		public const string ACCESS_IPs = "AccessIps";

        // Constants for Reporting Transforms
        public const string BANDWIDTH_TRANSFORM = "BandwidthXLST";
        public const string DISKSPACE_TRANSFORM = "DiskspaceXLST";

        public static readonly SystemSettings Empty = new SystemSettings { SettingsArray = new string[][] {} };

		private NameValueCollection settingsHash = null;
		public string[][] SettingsArray;

		[XmlIgnore]
		NameValueCollection Settings
		{
			get
			{
				if (settingsHash == null)
				{
					// create new dictionary
					settingsHash = new NameValueCollection();

					// fill dictionary
					if (SettingsArray != null)
					{
						foreach (string[] pair in SettingsArray)
							settingsHash.Add(pair[0], pair[1]);
					}
				}
				return settingsHash;
			}
		}

		[XmlIgnore]
		public string this[string settingName]
		{
			get
			{
				return Settings[settingName];
			}
			set
			{
				// set setting
				Settings[settingName] = value;

				// rebuild array
				SettingsArray = new string[Settings.Count][];
				for (int i = 0; i < Settings.Count; i++)
				{
					SettingsArray[i] = new string[] { Settings.Keys[i], Settings[Settings.Keys[i]] };
				}
			}
		}

	    public bool Contains(string settingName)
	    {
	        return Settings.AllKeys.Any(x => x.ToLowerInvariant() == (settingName ?? string.Empty).ToLowerInvariant());
	    }

	    public T GetValueOrDefault<T>(string settingName, T defaultValue)
	    {
	        try
	        {
                return (T)Convert.ChangeType(Settings[settingName], typeof(T));
	        }
	        catch
	        {
	        }

	        return defaultValue;
	    }

	    public int GetInt(string settingName)
		{
			return Int32.Parse(Settings[settingName]);
		}
	}
}
