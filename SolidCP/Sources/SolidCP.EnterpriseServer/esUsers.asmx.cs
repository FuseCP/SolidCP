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
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Microsoft.Web.Services3;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esUsers : System.Web.Services.WebService
    {
        [WebMethod(Description = "Checks if the account with the specified username exists.")]
        public bool UserExists(string username)
        {
            return UserController.UserExists(username);
        }

        [WebMethod]
        public UserInfo GetUserById(int userId)
        {
            UserInfoInternal uinfo = UserController.GetUser(userId);
            return (uinfo != null) ? new UserInfo(uinfo) : null;
        }

        [WebMethod]
        public UserInfo GetUserByUsername(string username)
        {
            UserInfoInternal uinfo = UserController.GetUser(username);
            return (uinfo != null) ? new UserInfo(uinfo) : null;
        }

        [WebMethod]
        public List<UserInfo> GetUsers(int ownerId, bool recursive)
        {
            return UserController.GetUsers(ownerId, recursive);
        }

        [WebMethod]
        public void AddUserVLan(int userId, UserVlan vLan)
        {
            UserController.AddUserVLan(userId, vLan);
        }

        [WebMethod]
        public void DeleteUserVLan(int userId, ushort vLanId)
        {
            UserController.DeleteUserVLan(userId, vLanId);
        }

        [WebMethod]
        public DataSet GetRawUsers(int ownerId, bool recursive)
        {
            return UserController.GetRawUsers(ownerId, recursive);
        }

        [WebMethod]
        public DataSet GetUsersPaged(int userId, string filterColumn, string filterValue,
            int statusId, int roleId, string sortColumn, int startRow, int maximumRows)
        {
            return UserController.GetUsersPaged(userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public DataSet GetUsersPagedRecursive(int userId, string filterColumn, string filterValue,
            int statusId, int roleId, string sortColumn, int startRow, int maximumRows)
        {
            return UserController.GetUsersPagedRecursive(userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public DataSet GetUsersSummary(int userId)
        {
            return UserController.GetUsersSummary(userId);
        }

        [WebMethod]
        public DataSet GetUserDomainsPaged(int userId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return UserController.GetUserDomainsPaged(userId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public DataSet GetRawUserPeers(int userId)
        {
            return UserController.GetRawUserPeers(userId);
        }

        [WebMethod]
        public List<UserInfo> GetUserPeers(int userId)
        {
            return UserController.GetUserPeers(userId);
        }

        [WebMethod]
        public List<UserInfo> GetUserParents(int userId)
        {
            return UserController.GetUserParents(userId);
        }

        [WebMethod]
        public int AddUser(UserInfo user, bool sendLetter, string password, string[] notes)
        {
            return UserController.AddUser(user, sendLetter, password, notes);
        }

        [WebMethod]
        public int AddUserLiteral(
		    int ownerId,
		    int roleId,
		    int statusId,
		    bool isPeer,
		    bool isDemo,
		    string username,
		    string password,
		    string firstName,
		    string lastName,
		    string email,
		    string secondaryEmail,
		    string address,
		    string city,
		    string country,
		    string state,
		    string zip,
		    string primaryPhone,
		    string secondaryPhone,
		    string fax,
		    string instantMessenger,
            bool htmlMail,
		    string companyName,
		    bool ecommerceEnabled,
            bool sendLetter)
        {
            UserInfo user = new UserInfo();
            user.OwnerId = ownerId;
            user.RoleId = roleId;
            user.StatusId = statusId;
            user.IsPeer = isPeer;
            user.IsDemo = isDemo;
            user.Username = username;
//            user.Password = password;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.SecondaryEmail = secondaryEmail;
            user.Address = address;
            user.City = city;
            user.Country = country;
            user.State = state;
            user.Zip = zip;
            user.PrimaryPhone = primaryPhone;
            user.SecondaryPhone = secondaryPhone;
            user.Fax = fax;
            user.InstantMessenger = instantMessenger;
            user.HtmlMail = htmlMail;
            user.CompanyName = companyName;
            user.EcommerceEnabled = ecommerceEnabled;
            return UserController.AddUser(user, sendLetter, password);
        }

        [WebMethod]
        public int UpdateUserTask(string taskId, UserInfo user)
        {
            return UserController.UpdateUser(taskId, user);
        }

        [WebMethod]
        public int UpdateUserTaskAsynchronously(string taskId, UserInfo user)
        {
            return UserController.UpdateUserAsync(taskId, user);
        }

        [WebMethod]
        public int UpdateUser(UserInfo user)
        {
            return UserController.UpdateUser(user);
        }

        [WebMethod]
        public int UpdateUserLiteral(int userId,
            int roleId,
            int statusId,
            bool isPeer,
            bool isDemo,
            string firstName,
            string lastName,
            string email,
            string secondaryEmail,
            string address,
            string city,
            string country,
            string state,
            string zip,
            string primaryPhone,
            string secondaryPhone,
            string fax,
            string instantMessenger,
            bool htmlMail,
            string companyName,
            bool ecommerceEnabled)
        {
            UserInfo user = new UserInfo();
            user.UserId = userId;
            user.RoleId = roleId;
            user.StatusId = statusId;
            user.IsPeer = isPeer;
            user.IsDemo = isDemo;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.SecondaryEmail = secondaryEmail;
            user.Address = address;
            user.City = city;
            user.Country = country;
            user.State = state;
            user.Zip = zip;
            user.PrimaryPhone = primaryPhone;
            user.SecondaryPhone = secondaryPhone;
            user.Fax = fax;
            user.InstantMessenger = instantMessenger;
            user.HtmlMail = htmlMail;
            user.CompanyName = companyName;
            user.EcommerceEnabled = ecommerceEnabled;

            return UserController.UpdateUser(user);
        }

        [WebMethod]
        public int DeleteUser(int userId)
        {
            return UserController.DeleteUser(userId);
        }

        [WebMethod]
        public int ChangeUserPassword(int userId, string password)
        {
            int res = UserController.ChangeUserPassword(userId, password);
            return res;
        }

        [WebMethod]
        public bool UpdateUserMfa(string username, bool activate)
        {
            return UserController.UpdateUserMfaSecret(username, activate);
        }

        [WebMethod]
        public bool CanUserChangeMfa(int changeUserId)
        {
            return UserController.CanUserChangeMfa(changeUserId);
        }

        [WebMethod]
        public string[] GetUserMfaQrCodeData(string username)
        {
            return UserController.GetUserMfaQrCodeData(username);
        }

        [WebMethod]
        public bool ActivateUserMfaQrCode(string username, string pin)
        {
            return UserController.ActivateUserMfaQrCode(username, pin);
        }

        [WebMethod]
        public int ChangeUserStatus(int userId, UserStatus status)
        {
            return UserController.ChangeUserStatus(userId, status);
        }

        #region User Settings
        [WebMethod]
        public UserSettings GetUserSettings(int userId, string settingsName)
        {
            return UserController.GetUserSettings(userId, settingsName);
        }

        [WebMethod]
        public int UpdateUserSettings(UserSettings settings)
        {
            return UserController.UpdateUserSettings(settings);
        }

        [WebMethod]
        public DataSet GetUserThemeSettings(int userId)
        {
            return UserController.GetUserThemeSettings(userId);
        }

        [WebMethod]
        public void UpdateUserThemeSetting(int userId, string PropertyName, string PropertyValue)
        {
            UserController.UpdateUserThemeSetting(userId, PropertyName, PropertyValue);
        }

        [WebMethod]
        public void DeleteUserThemeSetting(int userId, string PropertyName)
        {
            UserController.DeleteUserThemeSetting(userId, PropertyName);
        }

        #endregion
    }
}
