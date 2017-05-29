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
using System.Web;
using System.Web.Caching;
using SolidCP.EnterpriseServer;
using AspNetSecurity = System.Web.Security;

namespace SolidCP.Portal
{
    /// <summary>
    /// Summary description for UsersDB.
    /// </summary>
    public class UsersHelper
    {
        private const int USER_CACHE_TIMEOUT = 30; // minutes
        private const int USER_SETTINGS_CACHE_TIMEOUT = 1; // minutes

        private const string DEFAULT_ADMIN_ROLE = "SolidCP Administrators";
        private const string DEFAULT_RESELLER_ROLE = "SolidCP Resellers";
        private const string DEFAULT_CSR_ROLE = "CSR";
        private const string DEFAULT_USER_ROLE = "SolidCP Users";

        #region Users ODS Methods (for Selected User)
        DataSet dsUsersPaged;

        public int GetUsersPagedCount(int userId, string filterColumn, string filterValue,
            int statusId, int roleId)
        {
            return (int)dsUsersPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetUsersPaged(int maximumRows, int startRowIndex, string sortColumn,
            int userId, string filterColumn, string filterValue, int statusId, int roleId)
        {
            dsUsersPaged = ES.Services.Users.GetUsersPaged(userId, filterColumn, filterValue,
                statusId, roleId, sortColumn, startRowIndex, maximumRows);
            return dsUsersPaged.Tables[1];
        }

        public int GetUsersPagedRecursiveCount(string filterColumn, string filterValue)
        {
            return (int)dsUsersPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetUsersPagedRecursive(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsUsersPaged = ES.Services.Users.GetUsersPagedRecursive(PanelSecurity.EffectiveUserId,
                filterColumn, "%" + filterValue + "%",
                0, 0, sortColumn, startRowIndex, maximumRows);
            return dsUsersPaged.Tables[1];
        }
        #endregion

        #region Users ODS Methods (for Logged User)
        DataSet dsLoggedUsersPaged;

        public int GetLoggedUsersPagedCount(string filterColumn, string filterValue)
        {
            return (int)dsLoggedUsersPaged.Tables[0].Rows[0][0];
        }

        public DataTable GetLoggedUsersPaged(int maximumRows, int startRowIndex, string sortColumn,
            string filterColumn, string filterValue)
        {
            dsLoggedUsersPaged = ES.Services.Users.GetUsersPagedRecursive(PanelSecurity.EffectiveUserId, filterColumn, filterValue,
                0, 0, sortColumn, startRowIndex, maximumRows);
            return dsLoggedUsersPaged.Tables[1];
        }
        #endregion

        #region Peers ODS Methods
        public DataSet GetUserPeers()
        {
            return ES.Services.Users.GetRawUserPeers(PanelSecurity.SelectedUserId);
        }
        #endregion

        public static UserInfo GetCachedUser(int userId)
        {
            string key = "CachedUser" + userId.ToString();
            UserInfo user = (UserInfo)HttpContext.Current.Cache[key];
            if (user == null)
            {
                // get remote user
                user = ES.Services.Users.GetUserById(userId);

                // place to cache
                if (user != null)
                    HttpContext.Current.Cache.Insert(key, user, null, DateTime.Now.AddMinutes(USER_CACHE_TIMEOUT), Cache.NoSlidingExpiration);
            }
            return user;
        }

        public static UserSettings GetCachedUserSettings(int userId, string settingsName)
        {
            string key = "CachedUserSettings" + userId.ToString() + "_" + settingsName;
            UserSettings settings = (UserSettings)HttpContext.Current.Cache[key];
            if (settings == null)
            {
                // get user settings
                settings = ES.Services.Users.GetUserSettings(userId, settingsName);

                // place to cache
                if (settings != null)
                    HttpContext.Current.Cache.Insert(key, settings, null, DateTime.Now.AddMinutes(USER_SETTINGS_CACHE_TIMEOUT), Cache.NoSlidingExpiration);
            }
            return settings;
        }

        #region Display preferences
        private const int ITEMS_PER_PAGE_DEFAULT = 10;
        private const string ITEMS_PER_PAGE_SESSION = "ItemsPerPage";
        public static int GetDisplayItemsPerPage()
        {
            int itemsNumber = ITEMS_PER_PAGE_DEFAULT; // default value
            if (HttpContext.Current.Session[ITEMS_PER_PAGE_SESSION] != null)
            {
                itemsNumber = Utils.ParseInt(HttpContext.Current.Session[ITEMS_PER_PAGE_SESSION].ToString(), ITEMS_PER_PAGE_DEFAULT);
            }
            else
            {
                int userId = PanelSecurity.SelectedUserId;
                UserSettings settings = ES.Services.Users.GetUserSettings(userId, UserSettings.DISPLAY_PREFS);
                
                if (settings != null)
                {                     
                    itemsNumber = Utils.ParseInt(settings[UserSettings.GRID_ITEMS], ITEMS_PER_PAGE_DEFAULT);
                    HttpContext.Current.Session[ITEMS_PER_PAGE_SESSION] = itemsNumber;
                                        
                }
            }
                        
            if (itemsNumber <= 0)
            {
                itemsNumber = ITEMS_PER_PAGE_DEFAULT;
                HttpContext.Current.Session[ITEMS_PER_PAGE_SESSION] = itemsNumber;
            }
                            
            return itemsNumber;
        }

        public static void SetDisplayItemsPerPage(int itemsNumber)
        {
            UserSettings settings = new UserSettings();
            settings.UserId = PanelSecurity.SelectedUserId;
            settings.SettingsName = UserSettings.DISPLAY_PREFS;
            settings[UserSettings.GRID_ITEMS] = itemsNumber.ToString();
            int res =  ES.Services.Users.UpdateUserSettings(settings);
            
            HttpContext.Current.Session[ITEMS_PER_PAGE_SESSION] = itemsNumber;
        }
        #endregion

        public static UserInfo GetUser(int userId)
        {
            // get remote user
            return ES.Services.Users.GetUserById(userId);
        }

        public static void InvalidateCachedUser(int userId)
        {
            string key = "CachedUser" + userId.ToString();
            HttpContext.Current.Cache.Remove(key);
        }

        public static DataSet GetUsers(int ownerId, bool recursive)
        {
            DataSet dsUsers = ES.Services.Users.GetRawUsers(ownerId, recursive);
            DataTable dtUsers = dsUsers.Tables[0];

            // add "RoleName", "StatusName" columns
            dtUsers.Columns.Add("RoleName", typeof(string));
            dtUsers.Columns.Add("StatusName", typeof(string));
            foreach (DataRow dr in dtUsers.Rows)
            {
                dr["RoleName"] = PanelFormatter.GetUserRoleName((int)dr["RoleID"]);
                dr["StatusName"] = PanelFormatter.GetAccountStatusName((int)dr["StatusID"]);
            }

            return dsUsers;
        }

        public static int AddUser(List<string> log, int portalId, UserInfo user, bool sendLetter, string password)
        {
            // add user to SolidCP server
            return ES.Services.Users.AddUser(user, sendLetter, password);
        }

        public static void AddUserVLan(int userId, UserVlan vLan)
        {
            ES.Services.Users.AddUserVLan(userId, vLan);
        }

        public static void DeleteUserVlan(int userId, ushort vLanId)
        {
            ES.Services.Users.DeleteUserVLan(userId, vLanId);
        }

        public static int DeleteUser(int portalId, int userId)
        {
            // delete SolidCP user
            return ES.Services.Users.DeleteUser(userId);
        }

        public static int UpdateUser(int portalId, UserInfo user)
        {
            // update user in SolidCP
            int result = ES.Services.Users.UpdateUser(user);

            if (result < 0)
                return result;

            return 0;
        }

        public static int ChangeUserPassword(int portalId, int userId, string newPassword)
        {
			return PortalUtils.ChangeUserPassword(userId, newPassword);
        }

        public static int ChangeUserStatus(int portalId, int userId, UserStatus status)
        {
            // load user account
            return ES.Services.Users.ChangeUserStatus(userId, status);
        }
    }
}
