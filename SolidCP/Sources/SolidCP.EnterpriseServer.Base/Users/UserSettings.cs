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
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for ServiceProviderSettings.
    /// </summary>
    public class UserSettings
    {
        public const string ACCOUNT_SUMMARY_LETTER = "AccountSummaryLetter";
        public const string PACKAGE_SUMMARY_LETTER = "PackageSummaryLetter";
        public const string PASSWORD_REMINDER_LETTER = "PasswordReminderLetter";
        public const string EXCHANGE_MAILBOX_SETUP_LETTER = "ExchangeMailboxSetupLetter";
        public const string EXCHANGE_HOSTED_EDITION_ORGANIZATION_SUMMARY = "ExchangeHostedEditionOrganizationSummary";
        public const string HOSTED_SOLUTION_REPORT = "HostedSoluitonReportSummaryLetter";
        public const string ORGANIZATION_USER_SUMMARY_LETTER = "OrganizationUserSummaryLetter";
        public const string VPS_SUMMARY_LETTER = "VpsSummaryLetter";
        public const string DOMAIN_EXPIRATION_LETTER = "DomainExpirationLetter";
        public const string DOMAIN_LOOKUP_LETTER = "DomainLookupLetter";
        public const string VERIFICATION_CODE_LETTER = "VerificationCodeLetter";
        public const string WEB_POLICY = "WebPolicy";
        public const string FTP_POLICY = "FtpPolicy";
        public const string MAIL_POLICY = "MailPolicy";
        public const string MSSQL_POLICY = "MsSqlPolicy";
        public const string MYSQL_POLICY = "MySqlPolicy";
        public const string MARIADB_POLICY = "MariaDBPolicy";
        public const string SHAREPOINT_POLICY = "SharePointPolicy";
        public const string OS_POLICY = "OsPolicy";
        public const string EXCHANGE_POLICY = "ExchangePolicy";
        public const string EXCHANGE_HOSTED_EDITION_POLICY = "ExchangeHostedEditionPolicy";
        public const string SolidCP_POLICY = "SolidCPPolicy";
        public const string VPS_POLICY = "VpsPolicy";
        public const string DISPLAY_PREFS = "DisplayPreferences";
        public const string GRID_ITEMS = "GridItems";

        public const string DEFAULT_MAILBOXPLANS = "DefaultMailboxPlans";
        public const string DEFAULT_LYNCUSERPLANS = "DefaultLyncUserPlans";
        public const string DEFAULT_SFBUSERPLANS = "DefaultSfBUserPlans";
        public const string RDS_SETUP_LETTER = "RDSSetupLetter";
        public const string RDS_POLICY = "RdsPolicy";
        public const string USER_PASSWORD_EXPIRATION_LETTER = "UserPasswordExpirationLetter";
        public const string USER_PASSWORD_RESET_LETTER = "UserPasswordResetLetter";
        public const string USER_PASSWORD_REQUEST_LETTER = "OrganizationUserPasswordRequestLetter";
        public const string USER_PASSWORD_RESET_PINCODE_LETTER = "UserPasswordResetPincodeLetter";
        public const string HOSTED_ORGANIZATION_PASSWORD_POLICY = "MailboxPasswordPolicy";


        public int UserId;
        public string SettingsName;

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

        public int GetInt(string settingName)
        {
            return Int32.Parse(Settings[settingName]);
        }

        public long GetLong(string settingName)
        {
            return Int64.Parse(Settings[settingName]);
        }

        public bool GetBool(string settingName)
        {
            return Boolean.Parse(Settings[settingName]);
        }
    }
}
