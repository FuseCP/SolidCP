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
using System.Linq;
using System.Xml.Linq;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// User account.
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        private int userId;
        private int ownerId;
        private int roleId;
        private int statusId;
        private int loginStatusId;
        private int failedLogins;
        private DateTime created;
        private DateTime changed;
        private bool isPeer;
        private bool isDemo;
        private string comments;
        private string username;
//        private string password;
        private string firstName;
        private string lastName;
        private string email;
        private string secondaryEmail;
        private string address;
        private string city;
        private string country;
        private string state;
        private string zip;
        private string primaryPhone;
        private string secondaryPhone;
        private string fax;
        private string instantMessenger;
        private bool htmlMail;
        private string companyName;
        private bool ecommerceEnabled;
        private string subscriberNumber;


        /// <summary>
        /// Creates a new instance of UserInfo class.
        /// </summary>
        public UserInfo()
        {
        }

        public UserInfo(UserInfo src)
        {
            userId = src.userId;
            ownerId = src.ownerId;
            roleId = src.roleId;
            statusId = src.statusId;
            loginStatusId = src.loginStatusId;
            failedLogins = src.failedLogins;
            created = src.created;
            changed = src.changed;
            isPeer = src.isPeer;
            isDemo = src.isDemo;
            comments = src.comments;
            username = src.username;
            firstName = src.firstName;
            lastName = src.lastName;
            email = src.email;
            secondaryEmail = src.secondaryEmail;
            address = src.address;
            city = src.city;
            country = src.country;
            state = src.state;
            zip = src.zip;
            primaryPhone = src.primaryPhone;
            secondaryPhone = src.secondaryPhone;
            fax = src.fax;
            instantMessenger = src.instantMessenger;
            htmlMail = src.htmlMail;
            companyName = src.companyName;
            ecommerceEnabled = src.ecommerceEnabled;
            subscriberNumber = src.subscriberNumber;
            MfaMode = src.MfaMode;
        }

        /// <summary>
        /// User role ID:
        /// 		Administrator = 1,
        /// 		Reseller = 2,
        /// 		User = 3
        /// </summary>
        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }


        /// <summary>
        /// User role.
        /// </summary>
        public UserRole Role
        {
            get { return (UserRole)roleId; }
            set { roleId = (int)value; }
        }

        /// <summary>
        /// User account status:
        /// Active = 1,
        /// Suspended = 2,
        /// Cancelled = 3,
        /// Pending = 4
        /// </summary>
        public int StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }

        /// <summary>
        /// User account status.
        /// </summary>
        public UserStatus Status
        {
            get { return (UserStatus)statusId; }
            set { statusId = (int)value; }
        }


        public int LoginStatusId
        {
            get { return loginStatusId; }
            set { loginStatusId = value; }
        }

        public UserLoginStatus LoginStatus
        {
            get { return (UserLoginStatus)loginStatusId; }
            set { loginStatusId = (int)value; }
        }

        public int FailedLogins
        {
            get { return failedLogins; }
            set { failedLogins = value; }
        }



        /// <summary>
        /// User account unique identifier.
        /// </summary>
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public int OwnerId
        {
            get { return ownerId; }
            set { ownerId = value; }
        }

        public bool IsPeer
        {
            get { return isPeer; }
            set { isPeer = value; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public DateTime Changed
        {
            get { return changed; }
            set { changed = value; }
        }

        public bool IsDemo
        {
            get { return isDemo; }
            set { isDemo = value; }
        }

        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public string LastName
        {
            get { return this.lastName; }
            set { this.lastName = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

//        public string Password
//        {
//            get { return this.password; }
//            set { this.password = value; }
//        }

        public string FirstName
        {
            get { return this.firstName; }
            set { this.firstName = value; }
        }

        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }

        public string PrimaryPhone
        {
            get { return this.primaryPhone; }
            set { this.primaryPhone = value; }
        }

        public string Zip
        {
            get { return this.zip; }
            set { this.zip = value; }
        }

        public string InstantMessenger
        {
            get { return this.instantMessenger; }
            set { this.instantMessenger = value; }
        }

        public string Fax
        {
            get { return this.fax; }
            set { this.fax = value; }
        }

        public string SecondaryPhone
        {
            get { return this.secondaryPhone; }
            set { this.secondaryPhone = value; }
        }

        public string SecondaryEmail
        {
            get { return this.secondaryEmail; }
            set { this.secondaryEmail = value; }
        }

        public string Country
        {
            get { return this.country; }
            set { this.country = value; }
        }

        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }

        public string City
        {
            get { return this.city; }
            set { this.city = value; }
        }

        public string State
        {
            get { return this.state; }
            set { this.state = value; }
        }

        public bool HtmlMail
        {
            get { return this.htmlMail; }
            set { this.htmlMail = value; }
        }

        public string CompanyName
        {
            get { return this.companyName; }
            set { this.companyName = value; }
        }

        public bool EcommerceEnabled
        {
            get { return this.ecommerceEnabled; }
            set { this.ecommerceEnabled = value; }
        }

        public string SubscriberNumber
        {
            get { return this.subscriberNumber; }
            set { this.subscriberNumber = value; }
        }

        /// <summary>
        /// Multi-Factor Authentication Mode
        /// 0 - Off
        /// 1 - EMail
        /// 2 - Token-App
        /// </summary>
        public int MfaMode { get; set; }

        public string AdditionalParams { get; set; }

        public List<UserVlan> Vlans
        {
            get
            {
                List<UserVlan> result = new List<UserVlan>();
                try
                {

                    if (AdditionalParams != null)
                    {
                        XDocument doc = XDocument.Parse(AdditionalParams);
                        if (doc != null && doc.Root != null)
                        {
                            XElement vLansElement = doc.Root.Element("VLans");
                            if (vLansElement != null)
                            {
                                foreach (var item in vLansElement.Elements("VLan"))
                                    result.Add(new UserVlan
                                    {
                                        VLanID = item.Attribute("VLanID") != null ? ushort.Parse(item.Attribute("VLanID").Value) : (ushort)0,
                                        Comment = item.Attribute("Comment") != null ? item.Attribute("Comment").Value : null
                                    });
                            }
                        }
                        return result;
                    }
                }
                catch { }
                return result;
            }
        }
    }

    /// <summary>
    /// User's VLans
    /// </summary>
    [Serializable]
    public class UserVlan
    {
        public ushort VLanID { get; set; }
        public string Comment { get; set; }
    };

    public class UserInfoInternal : UserInfo
    {
        private string password;
        private string oneTimePassword;
        private OneTimePasswordStates oneTimePasswordState;

        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        public OneTimePasswordStates OneTimePasswordState
        {
            get { return oneTimePasswordState; }
            set { oneTimePasswordState = value; }
        }

        public string PinSecret { get; set; }
    };
}