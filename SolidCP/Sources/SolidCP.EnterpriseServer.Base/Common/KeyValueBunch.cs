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
using System.Collections.Specialized;
using System.Text;
using System.Xml.Serialization;

namespace SolidCP.EnterpriseServer
{
    public interface IKeyValueBunch
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        [XmlIgnore]
        string this[string settingName] { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string[] GetAllKeys();
    }

    [Serializable]
    public class KeyValueBunch : IKeyValueBunch
    {
        private NameValueCollection _settings;
        private bool hasPendingChanges;

        public string[][] KeyValueArray;

        [XmlIgnore]
        public bool IsEmpty
        {
            get { return (KeyValueArray == null || KeyValueArray.Length == 0); }
        }

        [XmlIgnore]
        public bool HasPendingChanges
        {
            get { return hasPendingChanges; }
        }

        [XmlIgnore]
        NameValueCollection Settings
        {
            get
            {
                //
                SyncCollectionsState(true);
                //
                hasPendingChanges = false;
                //
                return _settings;
            }
        }

        public string this[string settingName]
        {
            get
            {
                return Settings[settingName];
            }
            set
            {
                // check whether changes are really made
                if (!String.Equals(Settings[settingName], value))
                    hasPendingChanges = true;
                // set setting
                Settings[settingName] = value;
                //
                SyncCollectionsState(false);
            }
        }

        private void SyncCollectionsState(bool inputSync)
        {
            if (inputSync)
            {
                if (_settings == null)
                {
                    // create new dictionary
                    _settings = new NameValueCollection();

                    // fill dictionary
                    if (KeyValueArray != null)
                    {
                        foreach (string[] pair in KeyValueArray)
                            _settings.Add(pair[0], pair[1]);
                    }
                }
            }
            else
            {
                // rebuild array
                KeyValueArray = new string[Settings.Count][];
                //
                for (int i = 0; i < Settings.Count; i++)
                {
                    KeyValueArray[i] = new string[] { Settings.Keys[i], Settings[Settings.Keys[i]] };
                }
            }
        }

        public string[] GetAllKeys()
        {
            if (Settings != null)
                return Settings.AllKeys;

            return null;
        }

        public void RemoveKey(string name)
        {
            Settings.Remove(name);
            //
            SyncCollectionsState(false);
        }
    }

    /*public class CommonSettings
    {
        public const string INTERACTIVE = "interactive";
        public const string LIVE_MODE = "live_mode";
        public const string USERNAME = "username";
        public const string PASSWORD = "password";
    }*/

    public class ToCheckoutSettings
    {
        public const string FIXED_CART = "fixed_cart";
        public const string SECRET_WORD = "secret_word";
        public const string ACCOUNT_SID = "account_sid";
        public const string CURRENCY = "2co_currency";
        public const string LIVE_MODE = "live_mode";
        public const string INTERACTIVE = "interactive";
        public const string CONTINUE_SHOPPING_URL = "continue_shopping_url";
    }

    public class AuthNetSettings
    {
        public const string MD5_HASH = "md5_hash";
        public const string TRANSACTION_KEY = "trans_key";
        public const string DEMO_ACCOUNT = "demo_account";
        public const string SEND_CONFIRMATION = "send_confirm";
        public const string MERCHANT_EMAIL = "merchant_email";
        public const string INTERACTIVE = "interactive";
        public const string LIVE_MODE = "live_mode";
        public const string USERNAME = "username";
    }

    public class DirectiSettings
    {
        public const string PARENT_ID = "parent_id";
        public const string LIVE_MODE = "live_mode";
        public const string USERNAME = "username";
        public const string PASSWORD = "password";
        public const string SECURE_CHANNEL = "secure_channel";
    }

    public class EnomSettings
    {
        public const string LIVE_MODE = "live_mode";
        public const string USERNAME = "username";
        public const string PASSWORD = "password";
    }

    public class OffPaymentSettings
    {
        public const string AUTO_APPROVE = "auto_approve";
        public const string TRANSACTION_NUMBER_FORMAT = "transaction_number_format";
    }

    public class PayPalProSettings
    {
        public const string INTERACTIVE = "interactive";
        public const string USERNAME = "username";
        public const string PASSWORD = "password";
        public const string SIGNATURE = "signature";
        public const string LIVE_MODE = "live_mode";
    }

    public class PayPalStdSettings
    {
        public const string BUSINESS = "business";
        public const string RETURN_URL = "return_url";
        public const string CANCEL_RETURN_URL = "cancel_return_url";
        public const string LIVE_MODE = "live_mode";
        public const string INTERACTIVE = "interactive";
    }
}
