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
using System.Text;
using System.Web;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    /// <summary>
    /// Summary description for PanelSecurity.
    /// </summary>
    public class PanelSecurity
    {
        public static int PackageId
        {
            get
            {
                HttpRequest request = HttpContext.Current.Request;
                string sSpaceId = request[PortalUtils.SPACE_ID_PARAM];
                if (!String.IsNullOrEmpty(sSpaceId))
                {
                    return Utils.ParseInt(sSpaceId, 0);
                }
                return 0;
            }
        }

        #region Recently Switched Users
        public static UserInfo[] GetRecentlySwitchedUsers()
        {
            return GetRecentlySwitchedUsersInternal().ToArray();
        }

        private static List<UserInfo> GetRecentlySwitchedUsersInternal()
        {
            List<UserInfo> users = new List<UserInfo>();

            // get existing list
            string[] pairs = GetRecentlySwitchedUsersArray();
            foreach (string pair in pairs)
            {
                string[] parts = pair.Split('=');
                UserInfo user = new UserInfo();
                user.UserId = Utils.ParseInt(parts[0], 0);
                user.Username = parts[1];
                users.Add(user);
            }

            return users;
        }

        public static void AddRecentlySwitchedUser(int userId)
        {
            // get existing list
            List<UserInfo> users = GetRecentlySwitchedUsersInternal();

            // check if the user exists
            UserInfo existUser = null;
            foreach (UserInfo user in users)
            {
                if (user.UserId == userId)
                {
                    existUser = user;
                    break;
                }
            }

            if (existUser != null)
            {
                // move user to the top of the list
                users.Remove(existUser);
                users.Insert(0, existUser);
            }
            else
            {
                // read new user
                UserInfo newUser = ES.Services.Users.GetUserById(userId);
                if (newUser == null)
                    return;

                if (users.Count == 10)
                {
                    // remove last user
                    users.RemoveAt(9);
                }

                // insert new
                users.Insert(0, newUser);
            }

            // save results
            List<string> pairs = new List<string>();
            foreach (UserInfo user in users)
            {
                pairs.Add(user.UserId.ToString() + "=" + user.Username);
            }
            string s = String.Join("*", pairs.ToArray());

            string key = "RecentlySwitchedUsers" + LoggedUserId;
            HttpContext.Current.Items[key] = s;

            HttpCookie cookie = new HttpCookie(key, s);
            cookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Remove(key);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private static string[] GetRecentlySwitchedUsersArray()
        {
            string[] users = new string[] { };

            string key = "RecentlySwitchedUsers" + LoggedUserId;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (HttpContext.Current.Items[key] != null)
                users = ((string)HttpContext.Current.Items[key]).Split('*');
            else if (cookie != null)
                users = cookie.Value.Split('*');

            return users;
        }
        #endregion

        #region Selected user
        public static int SelectedUserId
        {
            get
            {
                HttpRequest request = HttpContext.Current.Request;
                string sUserId = request[PortalUtils.USER_ID_PARAM];
                if (!String.IsNullOrEmpty(sUserId))
                {
                    return Utils.ParseInt(sUserId, 0);
                }
                else
                {
                    // try to get from current space
                    int spaceId = PackageId;
                    if (spaceId > 0)
                    {
                        // load space
                        // check context
                        PackageInfo space = (PackageInfo)HttpContext.Current.Items["SolidCPSelectedSpace"];
                        if (space != null)
                        {
                            return space.UserId;
                        }
                        else
                        {
                            space = ES.Services.Packages.GetPackage(spaceId);
                            if (space != null)
                            {
                                // place to cache
                                HttpContext.Current.Items["SolidCPSelectedSpace"] = space;

                                // return
                                return space.UserId;
                            }
                        }
                    }
                    else
                    {
                        return EffectiveUserId;
                    }
                }
                return 0;
            }
        }

        public static UserInfo SelectedUser
        {
            get
            {
                UserInfo user = (UserInfo)HttpContext.Current.Items["SolidCPSelectedUser"];
                if (user == null)
                {
                    try
                    {
                        user = ES.Services.Users.GetUserById(SelectedUserId);
                    }
                    catch { }

                    // create <empty> user
                    if (user == null)
                    {
                        user = new UserInfo();
                        user.UserId = -1;
                        user.FirstName = "Unknown";
                        user.LastName = "User";
                        user.Role = UserRole.User;
                        user.IsDemo = true;
                        user.Email = "";
                        user.Username = "Unknown";
                    }

                    // add to context
                    HttpContext.Current.Items["SolidCPSelectedUser"] = user;
                }
                return user;
            }
        }
        #endregion

        #region Logged user
        public static int LoggedUserId
        {
            get
            {
                return (LoggedUser != null) ? LoggedUser.UserId : 0;
            }
        }

        public static UserInfo LoggedUser
        {
            get
            {
                UserInfo user = (UserInfo)HttpContext.Current.Items["SolidCPLoggedUser"];

                if (user == null)
                {
                    // load ES settings
					try
					{
						user = PortalUtils.GetCurrentUser();
					}
					catch { }

					if (user != null)
					{
						// add to context
						HttpContext.Current.Items["SolidCPLoggedUser"] = user;
					}
                }
                return user;
            }
        }
        #endregion

        #region Effective user
        public static int EffectiveUserId
        {
            get
            {
                return (LoggedUser != null && LoggedUser.IsPeer) ? LoggedUser.OwnerId : LoggedUserId;
            }
        }

        public static UserInfo EffectiveUser
        {
            get
            {
                UserInfo user = (UserInfo)HttpContext.Current.Items["SolidCPEffectiveUser"];
                if (user == null)
                {
                    user = ES.Services.Users.GetUserById(EffectiveUserId);

                    // add to context
                    HttpContext.Current.Items["SolidCPEffectiveUser"] = user;
                }
                return user;
            }
        }
        #endregion
    }
}
