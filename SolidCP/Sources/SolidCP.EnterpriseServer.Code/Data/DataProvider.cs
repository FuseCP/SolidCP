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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using SolidCP.EnterpriseServer.Base.HostedSolution;
using SolidCP.Providers.HostedSolution;
using Microsoft.ApplicationBlocks.Data;
using System.Collections.Generic;
using Microsoft.Win32;
using SolidCP.Providers.OS;
using SolidCP.Providers.RemoteDesktopServices;
using SolidCP.Providers.DNS;
using SolidCP.Providers.DomainLookup;
using SolidCP.Providers.StorageSpaces;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for DataProvider.
    /// </summary>
    public static class DataProvider
    {

        static string EnterpriseServerRegistryPath = "SOFTWARE\\SolidCP\\EnterpriseServer";

        private static string ConnectionString
        {
            get
            {
                string ConnectionKey = ConfigurationManager.AppSettings["SolidCP.AltConnectionString"];
                string value = string.Empty;

                if (!string.IsNullOrEmpty(ConnectionKey))
                {
                    RegistryKey root = Registry.LocalMachine;
                    RegistryKey rk = root.OpenSubKey(EnterpriseServerRegistryPath);
                    if (rk != null)
                    {
                        value = (string)rk.GetValue(ConnectionKey, null);
                        rk.Close();
                    }
                }

                if (!string.IsNullOrEmpty(value))
                    return value;
                else
                    return ConfigurationManager.ConnectionStrings["EnterpriseServer"].ConnectionString;
            }
        }

        private static string ObjectQualifier
        {
            get
            {
                return "";
            }
        }

        #region System Settings

        public static IDataReader GetSystemSettings(string settingsName)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetSystemSettings",
                new SqlParameter("@SettingsName", settingsName)
            );
        }

        public static void SetSystemSettings(string settingsName, string xml)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "SetSystemSettings",
                new SqlParameter("@SettingsName", settingsName),
                new SqlParameter("@Xml", xml)
            );
        }

        #endregion

        #region Theme Settings

        public static DataSet GetThemes()
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetThemes"
            );
        }

        public static DataSet GetThemeSettings(int ThmemeID)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetThemeSettings",
                new SqlParameter("@ThemeID", ThmemeID));
        }

        public static DataSet GetThemeSetting(int ThmemeID, string SettingsName)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetThemeSetting",
                new SqlParameter("@ThemeID", ThmemeID),
                new SqlParameter("@SettingsName", SettingsName));
        }

        public static DataSet GetUserThemeSettings(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserSettings",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@SettingsName", "Theme"));
        }

        public static void UpdateUserThemeSetting(int actorId, int userId, string PropertyName, string PropertyValue)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateUserThemeSetting",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@PropertyName", PropertyName), 
                new SqlParameter("@PropertyValue", PropertyValue));
        }

        public static void DeleteUserThemeSetting(int actorId, int userId, string PropertyName)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteUserThemeSetting",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@PropertyName", PropertyName));
        }

        #endregion

        #region Users
        public static bool CheckUserExists(string username)
        {
            SqlParameter prmExists = new SqlParameter("@Exists", SqlDbType.Bit);
            prmExists.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckUserExists",
                prmExists,
                new SqlParameter("@username", username));

            return Convert.ToBoolean(prmExists.Value);
        }

        public static DataSet GetUsersPaged(int actorId, int userId, string filterColumn, string filterValue,
            int statusId, int roleId, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUsersPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@roleId", roleId),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows),
                new SqlParameter("@recursive", recursive));
        }

        //TODO START
        public static DataSet GetSearchObject(int actorId, int userId, string filterColumn, string filterValue,
           int statusId, int roleId, string sortColumn, int startRow, int maximumRows, string colType, string fullType, 
           bool recursive, bool onlyFind)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSearchObject",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@StatusId", statusId),
                new SqlParameter("@RoleId", roleId),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@StartRow", startRow),
                new SqlParameter("@MaximumRows", maximumRows),
                new SqlParameter("@Recursive", recursive),
                new SqlParameter("@ColType", colType),
                new SqlParameter("@FullType", fullType),
                new SqlParameter("@OnlyFind", onlyFind));
        }
        public static DataSet GetSearchTableByColumns(string PagedStored, string FilterValue, int MaximumRows,
            bool Recursive, int PoolID, int ServerID, int ActorID, int StatusID, int PlanID, int OrgID,
            string ItemTypeName, string GroupName, int PackageID, string VPSType, int RoleID, int UserID,
            string FilterColumns)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSearchTableByColumns",
                new SqlParameter("@PagedStored", PagedStored),
                new SqlParameter("@FilterValue", FilterValue),
                new SqlParameter("@MaximumRows", MaximumRows),
                new SqlParameter("@Recursive", Recursive),
                new SqlParameter("@PoolID", PoolID),
                new SqlParameter("@ServerID", ServerID),
                new SqlParameter("@ActorID", ActorID),
                new SqlParameter("@StatusID", StatusID),
                new SqlParameter("@PlanID", PlanID),
                new SqlParameter("@OrgID", OrgID),
                new SqlParameter("@ItemTypeName", ItemTypeName),
                new SqlParameter("@GroupName", GroupName),
                new SqlParameter("@PackageID", PackageID),
                new SqlParameter("@VPSType", VPSType),
                new SqlParameter("@RoleID", RoleID),
                new SqlParameter("@UserID", UserID),
                new SqlParameter("@FilterColumns", FilterColumns));
        }

        //TODO END

        public static DataSet GetUsersSummary(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUsersSummary",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static DataSet GetUserDomainsPaged(int actorId, int userId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserDomainsPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@filterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@filterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetUsers(int actorId, int ownerId, bool recursive)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUsers",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@OwnerID", ownerId),
                new SqlParameter("@Recursive", recursive));
        }

        public static DataSet GetUserParents(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserParents",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static DataSet GetUserPeers(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserPeers",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@userId", userId));
        }

        public static IDataReader GetUserByExchangeOrganizationIdInternally(int itemId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserByExchangeOrganizationIdInternally",
                new SqlParameter("@ItemID", itemId));
        }



        public static IDataReader GetUserByIdInternally(int userId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserByIdInternally",
                new SqlParameter("@UserID", userId));
        }

        public static IDataReader GetUserByUsernameInternally(string username)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserByUsernameInternally",
                new SqlParameter("@Username", username));
        }

        public static IDataReader GetUserById(int actorId, int userId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserById",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static IDataReader GetUserByUsername(int actorId, string username)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserByUsername",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@Username", username));
        }

        public static int AddUser(int actorId, int ownerId, int roleId, int statusId, string subscriberNumber, int loginStatusId, bool isDemo,
            bool isPeer, string comments, string username, string password,
            string firstName, string lastName, string email, string secondaryEmail,
            string address, string city, string country, string state, string zip,
            string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail,
            string companyName, bool ecommerceEnabled)
        {
            SqlParameter prmUserId = new SqlParameter("@UserID", SqlDbType.Int);
            prmUserId.Direction = ParameterDirection.Output;

            // add user to SolidCP Users table
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddUser",
                prmUserId,
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@OwnerID", ownerId),
                new SqlParameter("@RoleID", roleId),
                new SqlParameter("@StatusId", statusId),
                new SqlParameter("@SubscriberNumber", subscriberNumber),
                new SqlParameter("@LoginStatusId", loginStatusId),
                new SqlParameter("@IsDemo", isDemo),
                new SqlParameter("@IsPeer", isPeer),
                new SqlParameter("@Comments", comments),
                new SqlParameter("@username", username),
                new SqlParameter("@password", password),
                new SqlParameter("@firstName", firstName),
                new SqlParameter("@lastName", lastName),
                new SqlParameter("@email", email),
                new SqlParameter("@secondaryEmail", secondaryEmail),
                new SqlParameter("@address", address),
                new SqlParameter("@city", city),
                new SqlParameter("@country", country),
                new SqlParameter("@state", state),
                new SqlParameter("@zip", zip),
                new SqlParameter("@primaryPhone", primaryPhone),
                new SqlParameter("@secondaryPhone", secondaryPhone),
                new SqlParameter("@fax", fax),
                new SqlParameter("@instantMessenger", instantMessenger),
                new SqlParameter("@htmlMail", htmlMail),
                new SqlParameter("@CompanyName", companyName),
                new SqlParameter("@EcommerceEnabled", ecommerceEnabled));

            return Convert.ToInt32(prmUserId.Value);
        }

        public static void UpdateUser(int actorId, int userId, int roleId, int statusId, string subscriberNumber, int loginStatusId, bool isDemo,
            bool isPeer, string comments, string firstName, string lastName, string email, string secondaryEmail,
            string address, string city, string country, string state, string zip,
            string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail,
            string companyName, bool ecommerceEnabled, string additionalParams)
        {
            // update user
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateUser",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@RoleID", roleId),
                new SqlParameter("@StatusId", statusId),
                new SqlParameter("@SubscriberNumber", subscriberNumber),
                new SqlParameter("@LoginStatusId", loginStatusId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@IsDemo", isDemo),
                new SqlParameter("@IsPeer", isPeer),
                new SqlParameter("@Comments", comments),
                new SqlParameter("@firstName", firstName),
                new SqlParameter("@lastName", lastName),
                new SqlParameter("@email", email),
                new SqlParameter("@secondaryEmail", secondaryEmail),
                new SqlParameter("@address", address),
                new SqlParameter("@city", city),
                new SqlParameter("@country", country),
                new SqlParameter("@state", state),
                new SqlParameter("@zip", zip),
                new SqlParameter("@primaryPhone", primaryPhone),
                new SqlParameter("@secondaryPhone", secondaryPhone),
                new SqlParameter("@fax", fax),
                new SqlParameter("@instantMessenger", instantMessenger),
                new SqlParameter("@htmlMail", htmlMail),
                new SqlParameter("@CompanyName", companyName),
                new SqlParameter("@EcommerceEnabled", ecommerceEnabled),
                new SqlParameter("@AdditionalParams", additionalParams));
        }

        public static void UpdateUserFailedLoginAttempt(int userId, int lockOut, bool reset)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateUserFailedLoginAttempt",
                new SqlParameter("@UserID", userId),
                new SqlParameter("@LockOut", lockOut),
                new SqlParameter("@Reset", reset));
        }

        public static void DeleteUser(int actorId, int userId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteUser",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static void ChangeUserPassword(int actorId, int userId, string password)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "ChangeUserPassword",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@password", password));
        }

        public static void SetUserOneTimePassword(int userId, string password, int auths)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "SetUserOneTimePassword",
                new SqlParameter("@UserID", userId),
                new SqlParameter("@Password", password),
                new SqlParameter("@OneTimePasswordState", auths));
        }

        public static void UpdateUserPinSecret(int actorId, int userId, string pinSecret)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateUserPinSecret",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@PinSecret", pinSecret)
                );
        }

        public static void UpdateUserMfaMode(int actorId, int userId, int mfaMode)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateUserMfaMode",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@MfaMode", mfaMode));
        }

        public static bool CanUserChangeMfa(int callerId, int changeUserId, bool canPeerChangeMfa)
        {
            SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Bit);
            prmResult.Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CanChangeMfa",
                new SqlParameter("@CallerID", callerId),
                new SqlParameter("@ChangeUserID", changeUserId),
                new SqlParameter("@CanPeerChangeMfa", canPeerChangeMfa ? 1 : 0),
                prmResult
                );

            return Convert.ToBoolean(prmResult.Value);
        }

        #endregion

        #region User Settings
        public static IDataReader GetUserSettings(int actorId, int userId, string settingsName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserSettings",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@SettingsName", settingsName));
        }
        public static void UpdateUserSettings(int actorId, int userId, string settingsName, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateUserSettings",
                new SqlParameter("@UserID", userId),
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@SettingsName", settingsName),
                new SqlParameter("@Xml", xml));
        }
        #endregion

        #region Servers
        public static DataSet GetAllServers(int actorId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAllServers",
                new SqlParameter("@actorId", actorId));
        }
        public static DataSet GetServers(int actorId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServers",
                new SqlParameter("@actorId", actorId));
        }

        public static IDataReader GetServer(int actorId, int serverId, bool forAutodiscover)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServer",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@forAutodiscover", forAutodiscover));
        }

        public static IDataReader GetServerShortDetails(int serverId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServerShortDetails",
                new SqlParameter("@ServerID", serverId));
        }

        public static IDataReader GetServerByName(int actorId, string serverName)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServerByName",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerName", serverName));
        }

        public static IDataReader GetServerInternal(int serverId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServerInternal",
                new SqlParameter("@ServerID", serverId));
        }

        public static int AddServer(string serverName, string serverUrl,
            string password, string comments, bool virtualServer, string instantDomainAlias,
            int primaryGroupId, bool adEnabled, string adRootDomain, string adUsername, string adPassword,
            string adAuthenticationType)
        {
            SqlParameter prmServerId = new SqlParameter("@ServerID", SqlDbType.Int);
            prmServerId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddServer",
                prmServerId,
                new SqlParameter("@ServerName", serverName),
                new SqlParameter("@ServerUrl", serverUrl),
                new SqlParameter("@Password", password),
                new SqlParameter("@Comments", comments),
                new SqlParameter("@VirtualServer", virtualServer),
                new SqlParameter("@InstantDomainAlias", instantDomainAlias),
                new SqlParameter("@PrimaryGroupId", primaryGroupId),
                new SqlParameter("@AdEnabled", adEnabled),
                new SqlParameter("@AdRootDomain", adRootDomain),
                new SqlParameter("@AdUsername", adUsername),
                new SqlParameter("@AdPassword", adPassword),
                new SqlParameter("@AdAuthenticationType", adAuthenticationType));

            return Convert.ToInt32(prmServerId.Value);
        }

        public static void UpdateServer(int serverId, string serverName, string serverUrl,
            string password, string comments, string instantDomainAlias,
            int primaryGroupId, bool adEnabled, string adRootDomain, string adUsername, string adPassword,
            string adAuthenticationType, string adParentDomain, String adParentDomainController)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateServer",
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@ServerName", serverName),
                new SqlParameter("@ServerUrl", serverUrl),
                new SqlParameter("@Password", password),
                new SqlParameter("@Comments", comments),
                new SqlParameter("@InstantDomainAlias", instantDomainAlias),
                new SqlParameter("@PrimaryGroupId", primaryGroupId),
                new SqlParameter("@AdEnabled", adEnabled),
                new SqlParameter("@AdRootDomain", adRootDomain),
                new SqlParameter("@AdUsername", adUsername),
                new SqlParameter("@AdPassword", adPassword),
                new SqlParameter("@AdAuthenticationType", adAuthenticationType),
                new SqlParameter("@AdParentDomain", adParentDomain),
                new SqlParameter("@AdParentDomainController", adParentDomainController));

        }

        public static int DeleteServer(int serverId)
        {
            SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
            prmResult.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteServer",
                prmResult,
                new SqlParameter("@ServerID", serverId));

            return Convert.ToInt32(prmResult.Value);
        }
        #endregion

        #region Virtual Servers
        public static DataSet GetVirtualServers(int actorId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetVirtualServers",
                new SqlParameter("@actorId", actorId));
        }

        public static DataSet GetAvailableVirtualServices(int actorId, int serverId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAvailableVirtualServices",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerID", serverId));
        }

        public static DataSet GetVirtualServices(int actorId, int serverId, bool forAutodiscover)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetVirtualServices",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@forAutodiscover", forAutodiscover));
        }

        public static void AddVirtualServices(int serverId, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddVirtualServices",
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@xml", xml));
        }

        public static void DeleteVirtualServices(int serverId, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteVirtualServices",
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@xml", xml));
        }

        public static void UpdateVirtualGroups(int serverId, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateVirtualGroups",
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@xml", xml));
        }
        #endregion

        #region Providers

        // Providers methods

        public static DataSet GetProviders()
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetProviders");
        }

        public static DataSet GetGroupProviders(int groupId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetGroupProviders",
                new SqlParameter("@groupId", groupId));
        }

        public static IDataReader GetProvider(int providerId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetProvider",
                new SqlParameter("@ProviderID", providerId));
        }

        public static IDataReader GetProviderByServiceID(int serviceId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetProviderByServiceID",
                new SqlParameter("@ServiceID", serviceId));
        }

        #endregion

        #region Private Network VLANs
        public static int AddPrivateNetworkVLAN(int serverId, int vlan, string comments)
        {
            SqlParameter prmAddresId = new SqlParameter("@VlanID", SqlDbType.Int);
            prmAddresId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddPrivateNetworkVlan",
                prmAddresId,
                new SqlParameter("@Vlan", vlan),
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@Comments", comments));

            return Convert.ToInt32(prmAddresId.Value);
        }

        public static int DeletePrivateNetworkVLAN(int vlanId)
        {
            SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
            prmResult.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeletePrivateNetworkVLAN",
                prmResult,
                new SqlParameter("@VlanID", vlanId));

            return Convert.ToInt32(prmResult.Value);
        }

        public static IDataReader GetPrivateNetworVLANsPaged(int actorId, int serverId,
            string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetPrivateNetworVLANsPaged",
                                        new SqlParameter("@ActorId", actorId),
                                        new SqlParameter("@ServerId", serverId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@startRow", startRow),
                                        new SqlParameter("@maximumRows", maximumRows));
            return reader;
        }

        public static IDataReader GetPrivateNetworVLAN(int vlanId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPrivateNetworVLAN",
                new SqlParameter("@VlanID", vlanId));
        }

        public static void UpdatePrivateNetworVLAN(int vlanId, int serverId, int vlan, string comments)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdatePrivateNetworVLAN",
                new SqlParameter("@VlanID", vlanId),
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@Vlan", vlan),
                new SqlParameter("@Comments", comments));
        }

        public static IDataReader GetPackagePrivateNetworkVLANs(int packageId, string sortColumn, int startRow, int maximumRows)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetPackagePrivateNetworkVLANs",
                                        new SqlParameter("@PackageID", packageId),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@startRow", startRow),
                                        new SqlParameter("@maximumRows", maximumRows));
            return reader;
        }

        public static void DeallocatePackageVLAN(int id)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeallocatePackageVLAN",
                                      new SqlParameter("@PackageVlanID", id));
        }

        public static IDataReader GetUnallottedVLANs(int packageId, int serviceId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetUnallottedVLANs",
                                        new SqlParameter("@PackageId", packageId),
                                        new SqlParameter("@ServiceId", serviceId));
        }

        public static void AllocatePackageVLANs(int packageId, string xml)
        {
            SqlParameter[] param = new[]
                                       {
                                           new SqlParameter("@PackageID", packageId),
                                           new SqlParameter("@xml", xml)
                                       };

            ExecuteLongNonQuery("AllocatePackageVLANs", param);
        }
        #endregion

        #region IPAddresses
        public static IDataReader GetIPAddress(int ipAddressId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetIPAddress",
                new SqlParameter("@AddressID", ipAddressId));
        }

        public static IDataReader GetIPAddresses(int actorId, int poolId, int serverId)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetIPAddresses",
                                        new SqlParameter("@ActorId", actorId),
                                        new SqlParameter("@PoolId", poolId),
                                        new SqlParameter("@ServerId", serverId));
            return reader;
        }

        public static IDataReader GetIPAddressesPaged(int actorId, int poolId, int serverId,
            string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetIPAddressesPaged",
                                        new SqlParameter("@ActorId", actorId),
                                        new SqlParameter("@PoolId", poolId),
                                        new SqlParameter("@ServerId", serverId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@startRow", startRow),
                                        new SqlParameter("@maximumRows", maximumRows));
            return reader;
        }

        public static int AddIPAddress(int poolId, int serverId, string externalIP, string internalIP,
            string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            SqlParameter prmAddresId = new SqlParameter("@AddressID", SqlDbType.Int);
            prmAddresId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddIPAddress",
                prmAddresId,
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@externalIP", externalIP),
                new SqlParameter("@internalIP", internalIP),
                new SqlParameter("@PoolId", poolId),
                new SqlParameter("@SubnetMask", subnetMask),
                new SqlParameter("@DefaultGateway", defaultGateway),
                new SqlParameter("@Comments", comments),
                new SqlParameter("@VLAN", VLAN));

            return Convert.ToInt32(prmAddresId.Value);
        }

        public static void UpdateIPAddress(int addressId, int poolId, int serverId,
            string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateIPAddress",
                new SqlParameter("@AddressID", addressId),
                new SqlParameter("@externalIP", externalIP),
                new SqlParameter("@internalIP", internalIP),
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@PoolId", poolId),
                new SqlParameter("@SubnetMask", subnetMask),
                new SqlParameter("@DefaultGateway", defaultGateway),
                new SqlParameter("@Comments", comments),
                new SqlParameter("@VLAN", VLAN));
        }

        public static void UpdateIPAddresses(string xmlIds, int poolId, int serverId,
            string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateIPAddresses",
                new SqlParameter("@Xml", xmlIds),
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@PoolId", poolId),
                new SqlParameter("@SubnetMask", subnetMask),
                new SqlParameter("@DefaultGateway", defaultGateway),
                new SqlParameter("@Comments", comments),
                new SqlParameter("@VLAN", VLAN));
        }

        public static int DeleteIPAddress(int ipAddressId)
        {
            SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
            prmResult.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteIPAddress",
                prmResult,
                new SqlParameter("@AddressID", ipAddressId));

            return Convert.ToInt32(prmResult.Value);
        }



        #endregion

        #region Clusters
        public static IDataReader GetClusters(int actorId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetClusters",
                new SqlParameter("@actorId", actorId));
        }

        public static int AddCluster(string clusterName)
        {
            SqlParameter prmId = new SqlParameter("@ClusterID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddCluster",
                prmId,
                new SqlParameter("@ClusterName", clusterName));

            return Convert.ToInt32(prmId.Value);
        }

        public static void DeleteCluster(int clusterId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteCluster",
                new SqlParameter("@ClusterId", clusterId));
        }

        #endregion

        #region Global DNS records
        public static DataSet GetDnsRecordsByService(int actorId, int serviceId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecordsByService",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServiceId", serviceId));
        }

        public static DataSet GetDnsRecordsByServer(int actorId, int serverId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecordsByServer",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerId", serverId));
        }

        public static DataSet GetDnsRecordsByPackage(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecordsByPackage",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageId", packageId));
        }

        public static DataSet GetDnsRecordsByGroup(int groupId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecordsByGroup",
                new SqlParameter("@GroupId", groupId));
        }

        public static DataSet GetDnsRecordsTotal(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecordsTotal",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId));
        }

        public static IDataReader GetDnsRecord(int actorId, int recordId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecord",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@RecordId", recordId));
        }

        public static void AddDnsRecord(int actorId, int serviceId, int serverId, int packageId, string recordType,
            string recordName, string recordData, int mxPriority, int SrvPriority, int SrvWeight, int SrvPort, int ipAddressId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddDnsRecord",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServiceId", serviceId),
                new SqlParameter("@ServerId", serverId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@RecordType", recordType),
                new SqlParameter("@RecordName", recordName),
                new SqlParameter("@RecordData", recordData),
                new SqlParameter("@MXPriority", mxPriority),
                new SqlParameter("@SrvPriority", SrvPriority),
                new SqlParameter("@SrvWeight", SrvWeight),
                new SqlParameter("@SrvPort", SrvPort),
                new SqlParameter("@IpAddressId", ipAddressId));
        }

        public static void UpdateDnsRecord(int actorId, int recordId, string recordType,
            string recordName, string recordData, int mxPriority, int SrvPriority, int SrvWeight, int SrvPort, int ipAddressId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateDnsRecord",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@RecordId", recordId),
                new SqlParameter("@RecordType", recordType),
                new SqlParameter("@RecordName", recordName),
                new SqlParameter("@RecordData", recordData),
                new SqlParameter("@MXPriority", mxPriority),
                new SqlParameter("@SrvPriority", SrvPriority),
                new SqlParameter("@SrvWeight", SrvWeight),
                new SqlParameter("@SrvPort", SrvPort),
                new SqlParameter("@IpAddressId", ipAddressId));
        }


        public static void DeleteDnsRecord(int actorId, int recordId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteDnsRecord",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@RecordId", recordId));
        }
        #endregion

        #region Domains
        public static DataSet GetDomains(int actorId, int packageId, bool recursive)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDomains",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@Recursive", recursive));
        }

        public static DataSet GetResellerDomains(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetResellerDomains",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId));
        }

        public static DataSet GetDomainsPaged(int actorId, int packageId, int serverId, bool recursive, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDomainsPaged",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@serverId", serverId),
                new SqlParameter("@recursive", recursive),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@StartRow", startRow),
                new SqlParameter("@MaximumRows", maximumRows));
        }

        public static IDataReader GetDomain(int actorId, int domainId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDomain",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@domainId", domainId));
        }

        public static IDataReader GetDomainByName(int actorId, string domainName, bool searchOnDomainPointer, bool isDomainPointer)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDomainByName",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@domainName", domainName),
                new SqlParameter("@SearchOnDomainPointer", searchOnDomainPointer),
                new SqlParameter("@IsDomainPointer", isDomainPointer));
        }


        public static DataSet GetDomainsByZoneId(int actorId, int zoneId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDomainsByZoneID",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@ZoneID", zoneId));
        }

        public static DataSet GetDomainsByDomainItemId(int actorId, int domainId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDomainsByDomainItemId",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@DomainID", domainId));
        }



        public static int CheckDomain(int packageId, string domainName, bool isDomainPointer)
        {
            SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckDomain",
                prmId,
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@domainName", domainName),
                new SqlParameter("@isDomainPointer", isDomainPointer));

            return Convert.ToInt32(prmId.Value);
        }



        public static int CheckDomainUsedByHostedOrganization(string domainName)
        {
            SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckDomainUsedByHostedOrganization",
                prmId,
                new SqlParameter("@domainName", domainName));

            return Convert.ToInt32(prmId.Value);
        }


        public static int AddDomain(int actorId, int packageId, int zoneItemId, string domainName,
            bool hostingAllowed, int webSiteId, int mailDomainId, bool isSubDomain, bool isPreviewDomain, bool isDomainPointer)
        {
            SqlParameter prmId = new SqlParameter("@DomainID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddDomain",
                prmId,
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@ZoneItemId", zoneItemId),
                new SqlParameter("@DomainName", domainName),
                new SqlParameter("@HostingAllowed", hostingAllowed),
                new SqlParameter("@WebSiteId", webSiteId),
                new SqlParameter("@MailDomainId", mailDomainId),
                new SqlParameter("@IsSubDomain", isSubDomain),
                new SqlParameter("@IsPreviewDomain", isPreviewDomain),
                new SqlParameter("@IsDomainPointer", isDomainPointer));

            return Convert.ToInt32(prmId.Value);
        }

        public static void UpdateDomain(int actorId, int domainId, int zoneItemId,
            bool hostingAllowed, int webSiteId, int mailDomainId, int domainItemId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateDomain",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@DomainId", domainId),
                new SqlParameter("@ZoneItemId", zoneItemId),
                new SqlParameter("@HostingAllowed", hostingAllowed),
                new SqlParameter("@WebSiteId", webSiteId),
                new SqlParameter("@MailDomainId", mailDomainId),
                new SqlParameter("@DomainItemId", domainItemId));
        }

        public static void DeleteDomain(int actorId, int domainId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteDomain",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@DomainId", domainId));
        }
        #endregion

        #region Services
        public static IDataReader GetServicesByServerId(int actorId, int serverId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServicesByServerID",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerID", serverId));
        }

        public static IDataReader GetServicesByServerIdGroupName(int actorId, int serverId, string groupName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServicesByServerIdGroupName",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@GroupName", groupName));
        }

        public static DataSet GetRawServicesByServerId(int actorId, int serverId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetRawServicesByServerID",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerID", serverId));
        }

        public static DataSet GetServicesByGroupId(int actorId, int groupId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServicesByGroupID",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@groupId", groupId));
        }

        public static DataSet GetServicesByGroupName(int actorId, string groupName, bool forAutodiscover)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServicesByGroupName",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@GroupName", groupName),
                new SqlParameter("@forAutodiscover", forAutodiscover));
        }

        public static IDataReader GetService(int actorId, int serviceId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
                CommandType.StoredProcedure,
                ObjectQualifier + "GetService",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServiceID", serviceId));
        }

        public static int AddService(int serverId, int providerId, string serviceName, int serviceQuotaValue,
            int clusterId, string comments)
        {
            SqlParameter prmServiceId = new SqlParameter("@ServiceID", SqlDbType.Int);
            prmServiceId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddService",
                prmServiceId,
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@ProviderID", providerId),
                new SqlParameter("@ServiceName", serviceName),
                new SqlParameter("@ServiceQuotaValue", serviceQuotaValue),
                new SqlParameter("@ClusterId", clusterId),
                new SqlParameter("@comments", comments));

            UpdateServerPackageServices(serverId);

            return Convert.ToInt32(prmServiceId.Value);
        }
        public static void UpdateServiceFully(int serviceId, int providerId, string serviceName, int serviceQuotaValue,
            int clusterId, string comments)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateServiceFully",
                new SqlParameter("@ProviderID", providerId),
                new SqlParameter("@ServiceName", serviceName),
                new SqlParameter("@ServiceID", serviceId),
                new SqlParameter("@ServiceQuotaValue", serviceQuotaValue),
                new SqlParameter("@ClusterId", clusterId),
                new SqlParameter("@Comments", comments));
        }

        public static void UpdateService(int serviceId, string serviceName, int serviceQuotaValue,
            int clusterId, string comments)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateService",
                new SqlParameter("@ServiceName", serviceName),
                new SqlParameter("@ServiceID", serviceId),
                new SqlParameter("@ServiceQuotaValue", serviceQuotaValue),
                new SqlParameter("@ClusterId", clusterId),
                new SqlParameter("@Comments", comments));
        }

        public static int DeleteService(int serviceId)
        {
            SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
            prmResult.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteService",
                prmResult,
                new SqlParameter("@ServiceID", serviceId));

            return Convert.ToInt32(prmResult.Value);
        }

        public static IDataReader GetServiceProperties(int actorId, int serviceId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
                CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceProperties",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServiceID", serviceId));
        }

        public static void UpdateServiceProperties(int serviceId, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateServiceProperties",
                new SqlParameter("@ServiceId", serviceId),
                new SqlParameter("@Xml", xml));
        }

        public static IDataReader GetResourceGroup(int groupId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                CommandType.StoredProcedure,
                ObjectQualifier + "GetResourceGroup",
                new SqlParameter("@groupId", groupId));
        }

        public static DataSet GetResourceGroups()
        {
            return SqlHelper.ExecuteDataset(ConnectionString,
                CommandType.StoredProcedure,
                ObjectQualifier + "GetResourceGroups");
        }

        public static IDataReader GetResourceGroupByName(string groupName)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                CommandType.StoredProcedure,
                ObjectQualifier + "GetResourceGroupByName",
                new SqlParameter("@groupName", groupName));
        }

        #endregion

        #region Service Items
        public static DataSet GetServiceItems(int actorId, int packageId, string groupName, string itemTypeName, bool recursive)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItems",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@GroupName", groupName),
                new SqlParameter("@ItemTypeName", itemTypeName),
                new SqlParameter("@Recursive", recursive));

        }

        public static DataSet GetServiceItemsPaged(int actorId, int packageId, string groupName, string itemTypeName,
            int serverId, bool recursive, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@groupName", groupName),
                new SqlParameter("@serverId", serverId),
                new SqlParameter("@itemTypeName", itemTypeName),
                new SqlParameter("@recursive", recursive),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetSearchableServiceItemTypes()
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSearchableServiceItemTypes");
        }

        public static DataSet GetServiceItemsByService(int actorId, int serviceId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsByService",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServiceID", serviceId));
        }

        public static int GetServiceItemsCount(string typeName, string groupName, int serviceId)
        {
            SqlParameter prmTotalNumber = new SqlParameter("@TotalNumber", SqlDbType.Int);
            prmTotalNumber.Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsCount",
                prmTotalNumber,
                new SqlParameter("@itemTypeName", typeName),
                new SqlParameter("@groupName", groupName),
                new SqlParameter("@serviceId", serviceId));

            // read identity
            return Convert.ToInt32(prmTotalNumber.Value);
        }

        public static DataSet GetServiceItemsForStatistics(int actorId, int serviceId, int packageId,
            bool calculateDiskspace, bool calculateBandwidth, bool suspendable, bool disposable)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsForStatistics",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@ServiceID", serviceId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@calculateDiskspace", calculateDiskspace),
                new SqlParameter("@calculateBandwidth", calculateBandwidth),
                new SqlParameter("@suspendable", suspendable),
                new SqlParameter("@disposable", disposable));
        }

        public static DataSet GetServiceItemsByPackage(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsByPackage",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static DataSet GetServiceItem(int actorId, int itemId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItem",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@actorId", actorId));
        }

        public static bool CheckServiceItemExists(int serviceId, string itemName, string itemTypeName)
        {
            SqlParameter prmExists = new SqlParameter("@Exists", SqlDbType.Bit);
            prmExists.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckServiceItemExistsInService",
                prmExists,
                new SqlParameter("@serviceId", serviceId),
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@itemTypeName", itemTypeName));

            return Convert.ToBoolean(prmExists.Value);
        }

        public static bool CheckServiceItemExists(string itemName, string groupName, string itemTypeName)
        {
            SqlParameter prmExists = new SqlParameter("@Exists", SqlDbType.Bit);
            prmExists.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckServiceItemExists",
                prmExists,
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@groupName", groupName),
                new SqlParameter("@itemTypeName", itemTypeName));

            return Convert.ToBoolean(prmExists.Value);
        }

        public static DataSet GetServiceItemByName(int actorId, int packageId, string groupName,
            string itemName, string itemTypeName)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemByName",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@itemTypeName", itemTypeName),
                new SqlParameter("@groupName", groupName));
        }

        public static DataSet GetServiceItemsByName(int actorId, int packageId, string itemName)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsByName",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@itemName", itemName));
        }

        public static int GetServiceItemsCountByNameAndServiceId(int actorId, int serviceId, string groupName,
            string itemName, string itemTypeName)
        {
            int res = 0;

            object obj =  SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsCountByNameAndServiceId",
               new SqlParameter("@ActorID", actorId),
                new SqlParameter("@ServiceId", serviceId),
                new SqlParameter("@ItemName", itemName),
                new SqlParameter("@GroupName", groupName),
                new SqlParameter("@ItemTypeName", itemTypeName));

            if (!int.TryParse(obj.ToString(), out res)) return -1;

            return res;
        }

        public static int AddServiceItem(int actorId, int serviceId, int packageId, string itemName,
            string itemTypeName, string xmlProperties)
        {
            // add item
            SqlParameter prmItemId = new SqlParameter("@ItemID", SqlDbType.Int);
            prmItemId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddServiceItem",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@ServiceID", serviceId),
                new SqlParameter("@ItemName", itemName),
                new SqlParameter("@ItemTypeName", itemTypeName),
                new SqlParameter("@xmlProperties", xmlProperties),
                new SqlParameter("@CreatedDate", DateTime.Now),
                prmItemId);

            return Convert.ToInt32(prmItemId.Value);
        }

        public static void UpdateServiceItem(int actorId, int itemId, string itemName, string xmlProperties)
        {
            // update item
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateServiceItem",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ItemName", itemName),
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@XmlProperties", xmlProperties));
        }

        public static void DeleteServiceItem(int actorId, int itemId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteServiceItem",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ItemID", itemId));
        }

        public static void MoveServiceItem(int actorId, int itemId, int destinationServiceId, bool forAutodiscover)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "MoveServiceItem",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@DestinationServiceID", destinationServiceId),
                new SqlParameter("@forAutodiscover", forAutodiscover));
        }

        public static int GetPackageServiceId(int actorId, int packageId, string groupName)
        {
            SqlParameter prmServiceId = new SqlParameter("@ServiceID", SqlDbType.Int);
            prmServiceId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageServiceID",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@groupName", groupName),
                prmServiceId);

            return Convert.ToInt32(prmServiceId.Value);
        }

        public static string GetMailFilterURL(int actorId, int packageId, string groupName)
        {
            SqlParameter prmFilterUrl = new SqlParameter("@FilterUrl", SqlDbType.NVarChar,200);
            prmFilterUrl.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetFilterURL",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@GroupName", groupName),
                prmFilterUrl);

            return Convert.ToString(prmFilterUrl.Value);
        }

        public static string GetMailFilterUrlByHostingPlan(int actorId, int PlanID, string groupName)
        { 
               SqlParameter prmFilterUrl = new SqlParameter("@FilterUrl", SqlDbType.NVarChar, 200);
            prmFilterUrl.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetFilterURLByHostingPlan",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@PlanID", PlanID),
                new SqlParameter("@GroupName", groupName),
                prmFilterUrl);

            return Convert.ToString(prmFilterUrl.Value);
        }
        

        public static void UpdatePackageDiskSpace(int packageId, string xml)
        {
            ExecuteLongNonQuery(
                ObjectQualifier + "UpdatePackageDiskSpace",
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@xml", xml));
        }

        public static void UpdatePackageBandwidth(int packageId, string xml)
        {
            ExecuteLongNonQuery(
                ObjectQualifier + "UpdatePackageBandwidth",
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@xml", xml));
        }

        public static DateTime GetPackageBandwidthUpdate(int packageId)
        {
            SqlParameter prmUpdateDate = new SqlParameter("@UpdateDate", SqlDbType.DateTime);
            prmUpdateDate.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageBandwidthUpdate",
                prmUpdateDate,
                new SqlParameter("@packageId", packageId));

            return (prmUpdateDate.Value != DBNull.Value) ? Convert.ToDateTime(prmUpdateDate.Value) : DateTime.MinValue;
        }

        public static void UpdatePackageBandwidthUpdate(int packageId, DateTime updateDate)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdatePackageBandwidthUpdate",
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@updateDate", updateDate));
        }

        public static IDataReader GetServiceItemType(int itemTypeId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetServiceItemType",
                new SqlParameter("@ItemTypeID", itemTypeId)
            );
        }

        public static IDataReader GetServiceItemTypes()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetServiceItemTypes"
            );
        }
        #endregion

        #region Plans
        // Plans methods
        public static DataSet GetHostingPlans(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetHostingPlans",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId));
        }

        public static DataSet GetHostingAddons(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetHostingAddons",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId));
        }

        public static DataSet GetUserAvailableHostingPlans(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserAvailableHostingPlans",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId));
        }

        public static DataSet GetUserAvailableHostingAddons(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserAvailableHostingAddons",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId));
        }

        public static IDataReader GetHostingPlan(int actorId, int planId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetHostingPlan",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PlanId", planId));
        }

        public static DataSet GetHostingPlanQuotas(int actorId, int packageId, int planId, int serverId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetHostingPlanQuotas",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@serverId", serverId));
        }

        public static int AddHostingPlan(int actorId, int userId, int packageId, string planName,
            string planDescription, bool available, int serverId, decimal setupPrice, decimal recurringPrice,
            int recurrenceUnit, int recurrenceLength, bool isAddon, string quotasXml)
        {
            SqlParameter prmPlanId = new SqlParameter("@PlanID", SqlDbType.Int);
            prmPlanId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddHostingPlan",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@planName", planName),
                new SqlParameter("@planDescription", planDescription),
                new SqlParameter("@available", available),
                new SqlParameter("@serverId", serverId),
                new SqlParameter("@setupPrice", setupPrice),
                new SqlParameter("@recurringPrice", recurringPrice),
                new SqlParameter("@recurrenceUnit", recurrenceUnit),
                new SqlParameter("@recurrenceLength", recurrenceLength),
                new SqlParameter("@isAddon", isAddon),
                new SqlParameter("@quotasXml", quotasXml),
                prmPlanId);

            // read identity
            return Convert.ToInt32(prmPlanId.Value);
        }

        public static DataSet UpdateHostingPlan(int actorId, int planId, int packageId, int serverId, string planName,
            string planDescription, bool available, decimal setupPrice, decimal recurringPrice,
            int recurrenceUnit, int recurrenceLength, string quotasXml)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateHostingPlan",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@serverId", serverId),
                new SqlParameter("@planName", planName),
                new SqlParameter("@planDescription", planDescription),
                new SqlParameter("@available", available),
                new SqlParameter("@setupPrice", setupPrice),
                new SqlParameter("@recurringPrice", recurringPrice),
                new SqlParameter("@recurrenceUnit", recurrenceUnit),
                new SqlParameter("@recurrenceLength", recurrenceLength),
                new SqlParameter("@quotasXml", quotasXml));
        }

        public static int CopyHostingPlan(int planId, int userId, int packageId)
        {
            SqlParameter prmPlanId = new SqlParameter("@DestinationPlanID", SqlDbType.Int);
            prmPlanId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CopyHostingPlan",
                new SqlParameter("@SourcePlanID", planId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@PackageID", packageId),
                prmPlanId);

            return Convert.ToInt32(prmPlanId.Value);
        }

        public static int DeleteHostingPlan(int actorId, int planId)
        {
            SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
            prmResult.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteHostingPlan",
                prmResult,
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PlanId", planId));

            return Convert.ToInt32(prmResult.Value);
        }
        #endregion

        #region Packages

        // Packages
        public static DataSet GetMyPackages(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetMyPackages",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static DataSet GetPackages(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackages",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static DataSet GetNestedPackagesSummary(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetNestedPackagesSummary",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static DataSet SearchServiceItemsPaged(int actorId, int userId, int itemTypeId, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "SearchServiceItemsPaged",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@itemTypeId", itemTypeId),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetPackagesPaged(int actorId, int userId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackagesPaged",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetNestedPackagesPaged(int actorId, int packageId, string filterColumn, string filterValue,
            int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetNestedPackagesPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@serverId", serverId),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetPackagePackages(int actorId, int packageId, bool recursive)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackagePackages",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@recursive", recursive));
        }

        public static IDataReader GetPackage(int actorId, int packageId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackage",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static DataSet GetPackageQuotas(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageQuotas",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static DataSet GetParentPackageQuotas(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetParentPackageQuotas",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static DataSet GetPackageQuotasForEdit(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageQuotasForEdit",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static DataSet AddPackage(int actorId, out int packageId, int userId, int planId, string packageName,
            string packageComments, int statusId, DateTime purchaseDate)
        {
            SqlParameter prmPackageId = new SqlParameter("@PackageID", SqlDbType.Int);
            prmPackageId.Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddPackage",
                prmPackageId,
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@packageName", packageName),
                new SqlParameter("@packageComments", packageComments),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@purchaseDate", purchaseDate));

            // read identity
            packageId = Convert.ToInt32(prmPackageId.Value);

            DistributePackageServices(actorId, packageId);

            return ds;
        }

        public static DataSet UpdatePackage(int actorId, int packageId, int planId, string packageName,
            string packageComments, int statusId, DateTime purchaseDate,
            bool overrideQuotas, string quotasXml, bool defaultTopPackage)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdatePackage",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@packageName", packageName),
                new SqlParameter("@packageComments", packageComments),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@purchaseDate", purchaseDate),
                new SqlParameter("@overrideQuotas", overrideQuotas),
                new SqlParameter("@quotasXml", quotasXml),
                new SqlParameter("@defaultTopPackage", defaultTopPackage));
        }
        public static void ChangePackageUser(int actorId, int packageId, int userId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "ChangePackageUser",                
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserId", userId));
        }

        public static void UpdatePackageName(int actorId, int packageId, string packageName,
            string packageComments)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdatePackageName",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@packageName", packageName),
                new SqlParameter("@packageComments", packageComments));
        }

        public static void DeletePackage(int actorId, int packageId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeletePackage",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        // Package Add-ons
        public static DataSet GetPackageAddons(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageAddons",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static IDataReader GetPackageAddon(int actorId, int packageAddonId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageAddon",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageAddonID", packageAddonId));
        }

        public static DataSet AddPackageAddon(int actorId, out int addonId, int packageId, int planId, int quantity,
            int statusId, DateTime purchaseDate, string comments)
        {
            SqlParameter prmPackageAddonId = new SqlParameter("@PackageAddonID", SqlDbType.Int);
            prmPackageAddonId.Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddPackageAddon",
                prmPackageAddonId,
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@PurchaseDate", purchaseDate),
                new SqlParameter("@Comments", comments));

            // read identity
            addonId = Convert.ToInt32(prmPackageAddonId.Value);

            return ds;
        }

        public static DataSet UpdatePackageAddon(int actorId, int packageAddonId, int planId, int quantity,
            int statusId, DateTime purchaseDate, string comments)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdatePackageAddon",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageAddonID", packageAddonId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@PurchaseDate", purchaseDate),
                new SqlParameter("@Comments", comments));
        }

        public static void DeletePackageAddon(int actorId, int packageAddonId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeletePackageAddon",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageAddonID", packageAddonId));
        }

        public static void UpdateServerPackageServices(int serverId)
        {
            // FIXME
            int defaultActorID = 1;

            // get server packages
            IDataReader packagesReader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
                @"SELECT PackageID FROM Packages WHERE ServerID = @ServerID",
                new SqlParameter("@ServerID", serverId)
            );

            // call DistributePackageServices for all packages on this server
            while (packagesReader.Read())
            {
                int packageId = (int) packagesReader["PackageID"];
                DistributePackageServices(defaultActorID, packageId);
            }
        }

        public static void DistributePackageServices(int actorId, int packageId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DistributePackageServices",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        #endregion

        #region Packages Settings
        public static IDataReader GetPackageSettings(int actorId, int packageId, string settingsName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageSettings",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@SettingsName", settingsName));
        }
        public static void UpdatePackageSettings(int actorId, int packageId, string settingsName, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdatePackageSettings",
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@SettingsName", settingsName),
                new SqlParameter("@Xml", xml));
        }
        #endregion

        #region Quotas
        public static IDataReader GetProviderServiceQuota(int providerId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetProviderServiceQuota",
                new SqlParameter("@providerId", providerId));
        }

        public static IDataReader GetPackageQuota(int actorId, int packageId, string quotaName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageQuota",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@QuotaName", quotaName));
        }
        #endregion

        #region Log
        public static void AddAuditLogRecord(string recordId, int severityId,
            int userId, string username, int packageId, int itemId, string itemName, DateTime startDate, DateTime finishDate, string sourceName,
            string taskName, string executionLog)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddAuditLogRecord",
                new SqlParameter("@recordId", recordId),
                new SqlParameter("@severityId", severityId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@username", username),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@startDate", startDate),
                new SqlParameter("@finishDate", finishDate),
                new SqlParameter("@sourceName", sourceName),
                new SqlParameter("@taskName", taskName),
                new SqlParameter("@executionLog", executionLog));
        }

        public static DataSet GetAuditLogRecordsPaged(int actorId, int userId, int packageId, int itemId, string itemName, DateTime startDate, DateTime endDate,
            int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAuditLogRecordsPaged",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@itemId", itemId),
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate),
                new SqlParameter("@severityId", severityId),
                new SqlParameter("@sourceName", sourceName),
                new SqlParameter("@taskName", taskName),
                new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetAuditLogSources()
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAuditLogSources");
        }

        public static DataSet GetAuditLogTasks(string sourceName)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAuditLogTasks",
                new SqlParameter("@sourceName", sourceName));
        }

        public static IDataReader GetAuditLogRecord(string recordId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAuditLogRecord",
                new SqlParameter("@recordId", recordId));
        }

        public static void DeleteAuditLogRecords(int actorId, int userId, int itemId, string itemName, DateTime startDate, DateTime endDate,
            int severityId, string sourceName, string taskName)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteAuditLogRecords",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@itemId", itemId),
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@startDate", startDate),
                new SqlParameter("@endDate", endDate),
                new SqlParameter("@severityId", severityId),
                new SqlParameter("@sourceName", sourceName),
                new SqlParameter("@taskName", taskName));
        }

        public static void DeleteAuditLogRecordsComplete()
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteAuditLogRecordsComplete");
        }

        #endregion

        #region Reports
        public static DataSet GetPackagesBandwidthPaged(int actorId, int userId, int packageId,
            DateTime startDate, DateTime endDate, string sortColumn,
            int startRow, int maximumRows)
        {
            return ExecuteLongDataSet(
                ObjectQualifier + "GetPackagesBandwidthPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate),
                new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetPackagesDiskspacePaged(int actorId, int userId, int packageId, string sortColumn,
            int startRow, int maximumRows)
        {
            return ExecuteLongDataSet(
                ObjectQualifier + "GetPackagesDiskspacePaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetPackageBandwidth(int actorId, int packageId, DateTime startDate, DateTime endDate)
        {
            return ExecuteLongDataSet(
                ObjectQualifier + "GetPackageBandwidth",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate));
        }

        public static DataSet GetPackageDiskspace(int actorId, int packageId)
        {
            return ExecuteLongDataSet(
                ObjectQualifier + "GetPackageDiskspace",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageId", packageId));
        }

        #endregion

        #region Scheduler

        public static IDataReader GetBackgroundTask(string taskId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                           ObjectQualifier + "GetBackgroundTask",
                                           new SqlParameter("@taskId", taskId));
        }

        public static IDataReader GetScheduleBackgroundTasks(int scheduleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                           ObjectQualifier + "GetScheduleBackgroundTasks",
                                           new SqlParameter("@scheduleId", scheduleId));
        }

        public static IDataReader GetBackgroundTasks(int actorId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                           ObjectQualifier + "GetBackgroundTasks",
                                           new SqlParameter("@actorId", actorId));
        }

        public static IDataReader GetBackgroundTasks(Guid guid)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                           ObjectQualifier + "GetThreadBackgroundTasks",
                                           new SqlParameter("@guid", guid));
        }

        public static IDataReader GetProcessBackgroundTasks(BackgroundTaskStatus status)
        {
                return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                               ObjectQualifier + "GetProcessBackgroundTasks",
                                               new SqlParameter("@status", (int)status));
        }

        public static IDataReader GetBackgroundTopTask(Guid guid)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                           ObjectQualifier + "GetBackgroundTopTask",
                                           new SqlParameter("@guid", guid));
        }

        public static int AddBackgroundTask(Guid guid, string taskId, int scheduleId, int packageId, int userId,
            int effectiveUserId, string taskName, int itemId, string itemName, DateTime startDate,
            int indicatorCurrent, int indicatorMaximum, int maximumExecutionTime, string source,
            int severity, bool completed, bool notifyOnComplete, BackgroundTaskStatus status)
        {
            SqlParameter prmId = new SqlParameter("@BackgroundTaskID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                      ObjectQualifier + "AddBackgroundTask",
                                      prmId,
                                      new SqlParameter("@guid", guid),
                                      new SqlParameter("@taskId", taskId),
                                      new SqlParameter("@scheduleId", scheduleId),
                                      new SqlParameter("@packageId", packageId),
                                      new SqlParameter("@userId", userId),
                                      new SqlParameter("@effectiveUserId", effectiveUserId),
                                      new SqlParameter("@taskName", taskName),
                                      new SqlParameter("@itemId", itemId),
                                      new SqlParameter("@itemName", itemName),
                                      new SqlParameter("@startDate", startDate),
                                      new SqlParameter("@indicatorCurrent", indicatorCurrent),
                                      new SqlParameter("@indicatorMaximum", indicatorMaximum),
                                      new SqlParameter("@maximumExecutionTime", maximumExecutionTime),
                                      new SqlParameter("@source", source),
                                      new SqlParameter("@severity", severity),
                                      new SqlParameter("@completed", completed),
                                      new SqlParameter("@notifyOnComplete", notifyOnComplete),
                                      new SqlParameter("@status", status));

            // read identity
            return Convert.ToInt32(prmId.Value);
        }

        public static void AddBackgroundTaskLog(int taskId, DateTime date, string exceptionStackTrace,
            bool innerTaskStart, int severity, string text, int textIdent, string xmlParameters)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                      ObjectQualifier + "AddBackgroundTaskLog",
                                      new SqlParameter("@taskId", taskId),
                                      new SqlParameter("@date", date),
                                      new SqlParameter("@exceptionStackTrace", exceptionStackTrace),
                                      new SqlParameter("@innerTaskStart", innerTaskStart),
                                      new SqlParameter("@severity", severity),
                                      new SqlParameter("@text", text),
                                      new SqlParameter("@textIdent", textIdent),
                                      new SqlParameter("@xmlParameters", xmlParameters));
        }

        public static IDataReader GetBackgroundTaskLogs(int taskId, DateTime startLogTime)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                           ObjectQualifier + "GetBackgroundTaskLogs",
                                           new SqlParameter("@taskId", taskId),
                                           new SqlParameter("@startLogTime", startLogTime));
        }

        public static void UpdateBackgroundTask(Guid guid, int taskId, int scheduleId, int packageId, string taskName, int itemId,
            string itemName, DateTime finishDate, int indicatorCurrent, int indicatorMaximum, int maximumExecutionTime,
            string source, int severity, bool completed, bool notifyOnComplete, BackgroundTaskStatus status)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                      ObjectQualifier + "UpdateBackgroundTask",
                                      new SqlParameter("@Guid", guid),
                                      new SqlParameter("@taskId", taskId),
                                      new SqlParameter("@scheduleId", scheduleId),
                                      new SqlParameter("@packageId", packageId),
                                      new SqlParameter("@taskName", taskName),
                                      new SqlParameter("@itemId", itemId),
                                      new SqlParameter("@itemName", itemName),
                                      new SqlParameter("@finishDate",
                                                       finishDate == DateTime.MinValue
                                                           ? DBNull.Value
                                                           : (object)finishDate),
                                      new SqlParameter("@indicatorCurrent", indicatorCurrent),
                                      new SqlParameter("@indicatorMaximum", indicatorMaximum),
                                      new SqlParameter("@maximumExecutionTime", maximumExecutionTime),
                                      new SqlParameter("@source", source),
                                      new SqlParameter("@severity", severity),
                                      new SqlParameter("@completed", completed),
                                      new SqlParameter("@notifyOnComplete", notifyOnComplete),
                                      new SqlParameter("@status", (int)status));

        }

        public static IDataReader GetBackgroundTaskParams(int taskId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                           ObjectQualifier + "GetBackgroundTaskParams",
                                           new SqlParameter("@taskId", taskId));
        }

        public static void AddBackgroundTaskParam(int taskId, string name, string value, string typeName)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                      ObjectQualifier + "AddBackgroundTaskParam",
                                      new SqlParameter("@taskId", taskId),
                                      new SqlParameter("@name", name),
                                      new SqlParameter("@value", value),
                                      new SqlParameter("@typeName", typeName));
        }

        public static void DeleteBackgroundTaskParams(int taskId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                      ObjectQualifier + "DeleteBackgroundTaskParams",
                                      new SqlParameter("@taskId", taskId));
        }

        public static void AddBackgroundTaskStack(int taskId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                      ObjectQualifier + "AddBackgroundTaskStack",
                                      new SqlParameter("@taskId", taskId));
        }

        public static void DeleteBackgroundTasks(Guid guid)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                      ObjectQualifier + "DeleteBackgroundTasks",
                                      new SqlParameter("@guid", guid));
        }

        public static void DeleteBackgroundTask(int taskId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                      ObjectQualifier + "DeleteBackgroundTask",
                                      new SqlParameter("@id", taskId));
        }

        public static IDataReader GetScheduleTasks(int actorId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleTasks",
                new SqlParameter("@actorId", actorId));
        }

        public static IDataReader GetScheduleTask(int actorId, string taskId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleTask",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@taskId", taskId));
        }

        public static DataSet GetSchedules(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSchedules",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@recursive", true));
        }

        public static DataSet GetSchedulesPaged(int actorId, int packageId, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSchedulesPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@recursive", recursive),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetSchedule(int actorId, int scheduleId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSchedule",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId));
        }
        public static IDataReader GetScheduleInternal(int scheduleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleInternal",
                new SqlParameter("@scheduleId", scheduleId));
        }
        public static DataSet GetNextSchedule()
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetNextSchedule");
        }
        public static IDataReader GetScheduleParameters(int actorId, string taskId, int scheduleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleParameters",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@taskId", taskId),
                new SqlParameter("@scheduleId", scheduleId));
        }

        /// <summary>
        /// Loads view configuration for the task with specified id.
        /// </summary>
        /// <param name="taskId">Task id which points to task for which view configuration will be loaded.</param>
        /// <returns>View configuration for the task with supplied id.</returns>
        public static IDataReader GetScheduleTaskViewConfigurations(string taskId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                           ObjectQualifier + "GetScheduleTaskViewConfigurations",
                                           new SqlParameter("@taskId", taskId));
        }

        public static int AddSchedule(int actorId, string taskId, int packageId,
            string scheduleName, string scheduleTypeId, int interval,
            DateTime fromTime, DateTime toTime, DateTime startTime,
            DateTime nextRun, bool enabled, string priorityId, int historiesNumber,
            int maxExecutionTime, int weekMonthDay, string xmlParameters)
        {
            SqlParameter prmId = new SqlParameter("@ScheduleID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddSchedule",
                prmId,
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@taskId", taskId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@scheduleName", scheduleName),
                new SqlParameter("@scheduleTypeId", scheduleTypeId),
                new SqlParameter("@interval", interval),
                new SqlParameter("@fromTime", fromTime),
                new SqlParameter("@toTime", toTime),
                new SqlParameter("@startTime", startTime),
                new SqlParameter("@nextRun", nextRun),
                new SqlParameter("@enabled", enabled),
                new SqlParameter("@priorityId", priorityId),
                new SqlParameter("@historiesNumber", historiesNumber),
                new SqlParameter("@maxExecutionTime", maxExecutionTime),
                new SqlParameter("@weekMonthDay", weekMonthDay),
                new SqlParameter("@xmlParameters", xmlParameters));

            // read identity
            return Convert.ToInt32(prmId.Value);
        }

        public static void UpdateSchedule(int actorId, int scheduleId, string taskId,
            string scheduleName, string scheduleTypeId, int interval,
            DateTime fromTime, DateTime toTime, DateTime startTime,
            DateTime lastRun, DateTime nextRun, bool enabled, string priorityId,
            int historiesNumber, int maxExecutionTime, int weekMonthDay, string xmlParameters)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateSchedule",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId),
                new SqlParameter("@taskId", taskId),
                new SqlParameter("@scheduleName", scheduleName),
                new SqlParameter("@scheduleTypeId", scheduleTypeId),
                new SqlParameter("@interval", interval),
                new SqlParameter("@fromTime", fromTime),
                new SqlParameter("@toTime", toTime),
                new SqlParameter("@startTime", startTime),
                new SqlParameter("@lastRun", (lastRun == DateTime.MinValue) ? DBNull.Value : (object)lastRun),
                new SqlParameter("@nextRun", nextRun),
                new SqlParameter("@enabled", enabled),
                new SqlParameter("@priorityId", priorityId),
                new SqlParameter("@historiesNumber", historiesNumber),
                new SqlParameter("@maxExecutionTime", maxExecutionTime),
                new SqlParameter("@weekMonthDay", weekMonthDay),
                new SqlParameter("@xmlParameters", xmlParameters));
        }

        public static void DeleteSchedule(int actorId, int scheduleId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteSchedule",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId));
        }

        public static DataSet GetScheduleHistories(int actorId, int scheduleId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleHistories",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId));
        }
        public static IDataReader GetScheduleHistory(int actorId, int scheduleHistoryId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleHistory",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleHistoryId", scheduleHistoryId));
        }
        public static int AddScheduleHistory(int actorId, int scheduleId,
            DateTime startTime, DateTime finishTime, string statusId, string executionLog)
        {
            SqlParameter prmId = new SqlParameter("@ScheduleHistoryID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddScheduleHistory",
                prmId,
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId),
                new SqlParameter("@startTime", (startTime == DateTime.MinValue) ? DBNull.Value : (object)startTime),
                new SqlParameter("@finishTime", (finishTime == DateTime.MinValue) ? DBNull.Value : (object)finishTime),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@executionLog", executionLog));

            // read identity
            return Convert.ToInt32(prmId.Value);
        }
        public static void UpdateScheduleHistory(int actorId, int scheduleHistoryId,
            DateTime startTime, DateTime finishTime, string statusId, string executionLog)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateScheduleHistory",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleHistoryId", scheduleHistoryId),
                new SqlParameter("@startTime", (startTime == DateTime.MinValue) ? DBNull.Value : (object)startTime),
                new SqlParameter("@finishTime", (finishTime == DateTime.MinValue) ? DBNull.Value : (object)finishTime),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@executionLog", executionLog));
        }
        public static void DeleteScheduleHistories(int actorId, int scheduleId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteScheduleHistories",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId));
        }
        #endregion

        #region Comments
        public static DataSet GetComments(int actorId, int userId, string itemTypeId, int itemId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetComments",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@itemTypeId", itemTypeId),
                new SqlParameter("@itemId", itemId));
        }

        public static void AddComment(int actorId, string itemTypeId, int itemId,
            string commentText, int severityId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddComment",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@itemTypeId", itemTypeId),
                new SqlParameter("@itemId", itemId),
                new SqlParameter("@commentText", commentText),
                new SqlParameter("@severityId", severityId));
        }

        public static void DeleteComment(int actorId, int commentId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteComment",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@commentId", commentId));
        }
        #endregion

        #region Helper Methods
        private static string VerifyColumnName(string str)
        {
            if (str == null)
                str = "";
            return Regex.Replace(str, @"[^\w\. ]", "");
        }

        private static string VerifyColumnValue(string str)
        {
            return String.IsNullOrEmpty(str) ? str : str.Replace("'", "''");
        }

        private static DataSet ExecuteLongDataSet(string spName, params SqlParameter[] parameters)
        {
            return ExecuteLongDataSet(spName, CommandType.StoredProcedure, parameters);
        }

        private static DataSet ExecuteLongQueryDataSet(string spName, params SqlParameter[] parameters)
        {
            return ExecuteLongDataSet(spName, CommandType.Text, parameters);
        }

        private static DataSet ExecuteLongDataSet(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(commandText, conn);
            cmd.CommandType = commandType;
            cmd.CommandTimeout = 300;

            if (parameters != null)
            {
                foreach (SqlParameter prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }
            }

            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return ds;
        }

        private static void ExecuteLongNonQuery(string spName, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(spName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            if (parameters != null)
            {
                foreach (SqlParameter prm in parameters)
                {
                    cmd.Parameters.Add(prm);
                }
            }

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        #endregion

        #region Exchange Server

        public static int AddExchangeAccount(int itemId, int accountType, string accountName,
            string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
            string mailboxManagerActions, string samAccountName, int mailboxPlanId, string subscriberNumber)
        {
            SqlParameter outParam = new SqlParameter("@AccountID", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddExchangeAccount",
                outParam,
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@AccountType", accountType),
                new SqlParameter("@AccountName", accountName),
                new SqlParameter("@DisplayName", displayName),
                new SqlParameter("@PrimaryEmailAddress", primaryEmailAddress),
                new SqlParameter("@MailEnabledPublicFolder", mailEnabledPublicFolder),
                new SqlParameter("@MailboxManagerActions", mailboxManagerActions),
                new SqlParameter("@SamAccountName", samAccountName),
                new SqlParameter("@MailboxPlanId", (mailboxPlanId == 0) ? (object)DBNull.Value : (object)mailboxPlanId),
                new SqlParameter("@SubscriberNumber", (string.IsNullOrEmpty(subscriberNumber) ? (object)DBNull.Value : (object)subscriberNumber))
            );

            return Convert.ToInt32(outParam.Value);
        }

        public static void AddExchangeAccountEmailAddress(int accountId, string emailAddress)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddExchangeAccountEmailAddress",
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@EmailAddress", emailAddress)
            );
        }

        public static void AddExchangeOrganization(int itemId, string organizationId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddExchangeOrganization",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@OrganizationID", organizationId)
            );
        }

        public static void AddExchangeOrganizationDomain(int itemId, int domainId, bool isHost)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddExchangeOrganizationDomain",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@DomainID", domainId),
                new SqlParameter("@IsHost", isHost)
            );
        }

        public static void ChangeExchangeAcceptedDomainType(int itemId, int domainId, int domainTypeId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "ChangeExchangeAcceptedDomainType",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@DomainID", domainId),
                new SqlParameter("@DomainTypeID", domainTypeId)
            );
        }

        public static IDataReader GetExchangeOrganizationStatistics(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeOrganizationStatistics",
                new SqlParameter("@ItemID", itemId)
            );
        }

        public static void DeleteUserEmailAddresses(int accountId, string primaryAddress)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteUserEmailAddresses",
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@PrimaryEmailAddress", primaryAddress)
            );
        }

        public static void DeleteExchangeAccount(int itemId, int accountId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteExchangeAccount",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@AccountID", accountId)
            );
        }


        public static void DeleteExchangeAccountEmailAddress(int accountId, string emailAddress)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteExchangeAccountEmailAddress",
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@EmailAddress", emailAddress)
            );
        }

        public static void DeleteExchangeOrganization(int itemId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteExchangeOrganization",
                new SqlParameter("@ItemID", itemId)
            );
        }

        public static void DeleteExchangeOrganizationDomain(int itemId, int domainId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteExchangeOrganizationDomain",
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@DomainID", domainId)
            );
        }

        public static bool ExchangeAccountEmailAddressExists(string emailAddress)
        {
            SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "ExchangeAccountEmailAddressExists",
                new SqlParameter("@EmailAddress", emailAddress),
                outParam
            );

            return Convert.ToBoolean(outParam.Value);
        }

        public static bool ExchangeOrganizationDomainExists(int domainId)
        {
            SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "ExchangeOrganizationDomainExists",
                new SqlParameter("@DomainID", domainId),
                outParam
            );

            return Convert.ToBoolean(outParam.Value);
        }

        public static bool ExchangeOrganizationExists(string organizationId)
        {
            SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "ExchangeOrganizationExists",
                new SqlParameter("@OrganizationID", organizationId),
                outParam
            );

            return Convert.ToBoolean(outParam.Value);
        }

        public static bool ExchangeAccountExists(string accountName)
        {
            SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "ExchangeAccountExists",
                new SqlParameter("@AccountName", accountName),
                outParam
            );

            return Convert.ToBoolean(outParam.Value);
        }

        public static void UpdateExchangeAccount(int accountId, string accountName, ExchangeAccountType accountType,
            string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
            string mailboxManagerActions, string samAccountName, int mailboxPlanId, int archivePlanId, string subscriberNumber,
            bool EnableArchiving)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateExchangeAccount",
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@AccountName", accountName),
                new SqlParameter("@DisplayName", displayName),
                new SqlParameter("@AccountType", (int)accountType),
                new SqlParameter("@PrimaryEmailAddress", primaryEmailAddress),
                new SqlParameter("@MailEnabledPublicFolder", mailEnabledPublicFolder),
                new SqlParameter("@MailboxManagerActions", mailboxManagerActions),
                new SqlParameter("@SamAccountName", samAccountName),
                new SqlParameter("@MailboxPlanId", (mailboxPlanId == 0) ? (object)DBNull.Value : (object)mailboxPlanId),
                new SqlParameter("@ArchivingMailboxPlanId", (archivePlanId < 1) ? (object)DBNull.Value : (object)archivePlanId),
                new SqlParameter("@SubscriberNumber", (string.IsNullOrEmpty(subscriberNumber) ? (object)DBNull.Value : (object)subscriberNumber)),
                new SqlParameter("@EnableArchiving", EnableArchiving)
            );
        }

        public static void UpdateExchangeAccountServiceLevelSettings(int accountId, int levelId, bool isVIP)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateExchangeAccountSLSettings",
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@LevelID", (levelId == 0) ? (object)DBNull.Value : (object)levelId),
                new SqlParameter("@IsVIP", isVIP)
            );
        }

        public static void UpdateExchangeAccountUserPrincipalName(int accountId, string userPrincipalName)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateExchangeAccountUserPrincipalName",
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@UserPrincipalName", userPrincipalName));
        }

        public static IDataReader GetExchangeAccount(int itemId, int accountId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeAccount",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@AccountID", accountId)
            );
        }

        public static IDataReader GetExchangeAccountByAccountName(int itemId, string accountName)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeAccountByAccountName",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@AccountName", accountName)
            );
        }

        public static IDataReader GetExchangeAccountByMailboxPlanId(int itemId, int MailboxPlanId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeAccountByMailboxPlanId",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@MailboxPlanId", MailboxPlanId)
            );
        }


        public static IDataReader GetExchangeAccountEmailAddresses(int accountId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeAccountEmailAddresses",
                new SqlParameter("@AccountID", accountId)
            );
        }

        public static IDataReader GetExchangeOrganizationDomains(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeOrganizationDomains",
                new SqlParameter("@ItemID", itemId)
            );
        }


        public static IDataReader GetExchangeAccounts(int itemId, int accountType)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeAccounts",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@AccountType", accountType)
            );
        }

        public static IDataReader GetExchangeAccountByAccountNameWithoutItemId(string userPrincipalName)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeAccountByAccountNameWithoutItemId",
                new SqlParameter("@UserPrincipalName", userPrincipalName)
            );
        }


        public static IDataReader GetExchangeMailboxes(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeMailboxes",
                new SqlParameter("@ItemID", itemId)
            );
        }

        public static IDataReader SearchExchangeAccountsByTypes(int actorId, int itemId, string accountTypes,
            string filterColumn, string filterValue, string sortColumn)
        {
            // check input parameters
            string[] types = accountTypes.Split(',');
            for (int i = 0; i < types.Length; i++)
            {
                try
                {
                    int type = Int32.Parse(types[i]);
                }
                catch
                {
                    throw new ArgumentException("Wrong patameter", "accountTypes");
                }
            }

            string searchTypes = String.Join(",", types);

            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "SearchExchangeAccountsByTypes",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@AccountTypes", searchTypes),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn))
            );
        }

        public static DataSet GetExchangeAccountsPaged(int actorId, int itemId, string accountTypes,
                string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool archiving)
        {
            // check input parameters
            string[] types = accountTypes.Split(',');
            for (int i = 0; i < types.Length; i++)
            {
                try
                {
                    int type = Int32.Parse(types[i]);
                }
                catch
                {
                    throw new ArgumentException("Wrong patameter", "accountTypes");
                }
            }

            string searchTypes = String.Join(",", types);

            return SqlHelper.ExecuteDataset(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeAccountsPaged",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@AccountTypes", searchTypes),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@StartRow", startRow),
                new SqlParameter("@MaximumRows", maximumRows),
                new SqlParameter("@Archiving", archiving)
            );
        }

        public static IDataReader SearchExchangeAccounts(int actorId, int itemId, bool includeMailboxes,
                bool includeContacts, bool includeDistributionLists, bool includeRooms, bool includeEquipment, bool IncludeSharedMailbox,
                bool includeSecurityGroups, string filterColumn, string filterValue, string sortColumn)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "SearchExchangeAccounts",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@IncludeMailboxes", includeMailboxes),
                new SqlParameter("@IncludeContacts", includeContacts),
                new SqlParameter("@IncludeDistributionLists", includeDistributionLists),
                new SqlParameter("@IncludeRooms", includeRooms),
                new SqlParameter("@IncludeEquipment", includeEquipment),
                new SqlParameter("@IncludeSharedMailbox", IncludeSharedMailbox),
                new SqlParameter("@IncludeSecurityGroups", includeSecurityGroups),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn))
            );
        }

        public static IDataReader SearchExchangeAccount(int actorId, int accountType, string primaryEmailAddress)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "SearchExchangeAccount",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@AccountType", accountType),
                new SqlParameter("@PrimaryEmailAddress", primaryEmailAddress)
            );
        }

        #endregion

        #region Exchange Mailbox Plans
        public static int AddExchangeMailboxPlan(int itemID, string mailboxPlan, bool enableActiveSync, bool enableIMAP, bool enableMAPI, bool enableOWA, bool enablePOP, bool enableAutoReply,
                                                    bool isDefault, int issueWarningPct, int keepDeletedItemsDays, int mailboxSizeMB, int maxReceiveMessageSizeKB, int maxRecipients,
                                                    int maxSendMessageSizeKB, int prohibitSendPct, int prohibitSendReceivePct, bool hideFromAddressBook, int mailboxPlanType,
                                                    bool enabledLitigationHold, int recoverabelItemsSpace, int recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg,
                                                    bool archiving, bool EnableArchiving, int ArchiveSizeMB, int ArchiveWarningPct, bool enableForceArchiveDeletion, bool isForJournaling)
        {
            SqlParameter outParam = new SqlParameter("@MailboxPlanId", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddExchangeMailboxPlan",
                outParam,
                new SqlParameter("@ItemID", itemID),
                new SqlParameter("@MailboxPlan", mailboxPlan),
                new SqlParameter("@EnableActiveSync", enableActiveSync),
                new SqlParameter("@EnableIMAP", enableIMAP),
                new SqlParameter("@EnableMAPI", enableMAPI),
                new SqlParameter("@EnableOWA", enableOWA),
                new SqlParameter("@EnablePOP", enablePOP),
                new SqlParameter("@EnableAutoReply", enableAutoReply),
                new SqlParameter("@IsDefault", isDefault),
                new SqlParameter("@IssueWarningPct", issueWarningPct),
                new SqlParameter("@KeepDeletedItemsDays", keepDeletedItemsDays),
                new SqlParameter("@MailboxSizeMB", mailboxSizeMB),
                new SqlParameter("@MaxReceiveMessageSizeKB", maxReceiveMessageSizeKB),
                new SqlParameter("@MaxRecipients", maxRecipients),
                new SqlParameter("@MaxSendMessageSizeKB", maxSendMessageSizeKB),
                new SqlParameter("@ProhibitSendPct", prohibitSendPct),
                new SqlParameter("@ProhibitSendReceivePct", prohibitSendReceivePct),
                new SqlParameter("@HideFromAddressBook", hideFromAddressBook),
                new SqlParameter("@MailboxPlanType", mailboxPlanType),
                new SqlParameter("@AllowLitigationHold", enabledLitigationHold),
                new SqlParameter("@RecoverableItemsWarningPct", recoverabelItemsWarning),
                new SqlParameter("@RecoverableItemsSpace", recoverabelItemsSpace),
                new SqlParameter("@LitigationHoldUrl", litigationHoldUrl),
                new SqlParameter("@LitigationHoldMsg", litigationHoldMsg),
                new SqlParameter("@Archiving", archiving),
                new SqlParameter("@EnableArchiving", EnableArchiving),
                new SqlParameter("@ArchiveSizeMB", ArchiveSizeMB),
                new SqlParameter("@ArchiveWarningPct", ArchiveWarningPct),
                new SqlParameter("@EnableForceArchiveDeletion", enableForceArchiveDeletion),
                new SqlParameter("@IsForJournaling", isForJournaling)
            );

            return Convert.ToInt32(outParam.Value);
        }



        public static void UpdateExchangeMailboxPlan(int mailboxPlanID, string mailboxPlan, bool enableActiveSync, bool enableIMAP, bool enableMAPI, bool enableOWA, bool enablePOP, bool enableAutoReply,
                                            bool isDefault, int issueWarningPct, int keepDeletedItemsDays, int mailboxSizeMB, int maxReceiveMessageSizeKB, int maxRecipients,
                                            int maxSendMessageSizeKB, int prohibitSendPct, int prohibitSendReceivePct, bool hideFromAddressBook, int mailboxPlanType,
                                            bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg,
                                            bool Archiving, bool EnableArchiving, int ArchiveSizeMB, int ArchiveWarningPct, bool enableForceArchiveDeletion, bool isForJournaling)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateExchangeMailboxPlan",
                new SqlParameter("@MailboxPlanID", mailboxPlanID),
                new SqlParameter("@MailboxPlan", mailboxPlan),
                new SqlParameter("@EnableActiveSync", enableActiveSync),
                new SqlParameter("@EnableIMAP", enableIMAP),
                new SqlParameter("@EnableMAPI", enableMAPI),
                new SqlParameter("@EnableOWA", enableOWA),
                new SqlParameter("@EnablePOP", enablePOP),
                new SqlParameter("@EnableAutoReply", enableAutoReply),
                new SqlParameter("@IsDefault", isDefault),
                new SqlParameter("@IssueWarningPct", issueWarningPct),
                new SqlParameter("@KeepDeletedItemsDays", keepDeletedItemsDays),
                new SqlParameter("@MailboxSizeMB", mailboxSizeMB),
                new SqlParameter("@MaxReceiveMessageSizeKB", maxReceiveMessageSizeKB),
                new SqlParameter("@MaxRecipients", maxRecipients),
                new SqlParameter("@MaxSendMessageSizeKB", maxSendMessageSizeKB),
                new SqlParameter("@ProhibitSendPct", prohibitSendPct),
                new SqlParameter("@ProhibitSendReceivePct", prohibitSendReceivePct),
                new SqlParameter("@HideFromAddressBook", hideFromAddressBook),
                new SqlParameter("@MailboxPlanType", mailboxPlanType),
                new SqlParameter("@AllowLitigationHold", enabledLitigationHold),
                new SqlParameter("@RecoverableItemsWarningPct", recoverabelItemsWarning),
                new SqlParameter("@RecoverableItemsSpace", recoverabelItemsSpace),
                new SqlParameter("@LitigationHoldUrl", litigationHoldUrl),
                new SqlParameter("@LitigationHoldMsg", litigationHoldMsg),
                new SqlParameter("@Archiving", Archiving),
	            new SqlParameter("@EnableArchiving", EnableArchiving),
                new SqlParameter("@ArchiveSizeMB", ArchiveSizeMB),
                new SqlParameter("@ArchiveWarningPct", ArchiveWarningPct),
                new SqlParameter("@EnableForceArchiveDeletion", enableForceArchiveDeletion),
                new SqlParameter("@IsForJournaling", isForJournaling)
            );
        }



        public static void DeleteExchangeMailboxPlan(int mailboxPlanId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteExchangeMailboxPlan",
                new SqlParameter("@MailboxPlanId", mailboxPlanId)
            );
        }


        public static IDataReader GetExchangeMailboxPlan(int mailboxPlanId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeMailboxPlan",
                new SqlParameter("@MailboxPlanId", mailboxPlanId)
            );
        }

        public static IDataReader GetExchangeMailboxPlans(int itemId, bool archiving)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeMailboxPlans",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@Archiving", archiving)
            );
        }


        public static IDataReader GetExchangeOrganization(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeOrganization",
                new SqlParameter("@ItemID", itemId)
            );
        }


        public static void SetOrganizationDefaultExchangeMailboxPlan(int itemId, int mailboxPlanId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "SetOrganizationDefaultExchangeMailboxPlan",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@MailboxPlanId", mailboxPlanId)
            );
        }

        public static void SetExchangeAccountMailboxPlan(int accountId, int mailboxPlanId, int archivePlanId, bool EnableArchiving)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "SetExchangeAccountMailboxplan",
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@MailboxPlanId", (mailboxPlanId == 0) ? (object)DBNull.Value : (object)mailboxPlanId),
                new SqlParameter("@ArchivingMailboxPlanId", (archivePlanId < 1) ? (object)DBNull.Value : (object)archivePlanId),
                new SqlParameter("@EnableArchiving", EnableArchiving)
            );
        }

        #endregion

        #region Exchange Retention Policy Tags
        public static int AddExchangeRetentionPolicyTag(int ItemID, string TagName, int TagType, int AgeLimitForRetention, int RetentionAction)
        {
            SqlParameter outParam = new SqlParameter("@TagID", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddExchangeRetentionPolicyTag",
                outParam,
                new SqlParameter("@ItemID", ItemID),
                new SqlParameter("@TagName", TagName),
                new SqlParameter("@TagType", TagType),
                new SqlParameter("@AgeLimitForRetention", AgeLimitForRetention),
                new SqlParameter("@RetentionAction", RetentionAction)
            );

            return Convert.ToInt32(outParam.Value);
        }

        public static void UpdateExchangeRetentionPolicyTag(int TagID, int ItemID, string TagName, int TagType, int AgeLimitForRetention, int RetentionAction)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateExchangeRetentionPolicyTag",
                new SqlParameter("@TagID", TagID),
                new SqlParameter("@ItemID", ItemID),
                new SqlParameter("@TagName", TagName),
                new SqlParameter("@TagType", TagType),
                new SqlParameter("@AgeLimitForRetention", AgeLimitForRetention),
                new SqlParameter("@RetentionAction", RetentionAction)
            );
        }

        public static void DeleteExchangeRetentionPolicyTag(int TagID)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteExchangeRetentionPolicyTag",
                new SqlParameter("@TagID", TagID)
            );
        }
        
        public static IDataReader GetExchangeRetentionPolicyTag(int TagID)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeRetentionPolicyTag",
                new SqlParameter("@TagID", TagID)
            );
        }

        public static IDataReader GetExchangeRetentionPolicyTags(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeRetentionPolicyTags",
                new SqlParameter("@ItemID", itemId)
            );
        }

        public static int AddExchangeMailboxPlanRetentionPolicyTag(int TagID, int MailboxPlanId)
        {
            SqlParameter outParam = new SqlParameter("@PlanTagID", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddExchangeMailboxPlanRetentionPolicyTag",
                outParam,
                new SqlParameter("@TagID", TagID),
                new SqlParameter("@MailboxPlanId", MailboxPlanId)
            );

            return Convert.ToInt32(outParam.Value);
        }

        public static void DeleteExchangeMailboxPlanRetentionPolicyTag(int PlanTagID)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteExchangeMailboxPlanRetentionPolicyTag",
                new SqlParameter("@PlanTagID", PlanTagID)
            );
        }

        public static IDataReader GetExchangeMailboxPlanRetentionPolicyTags(int MailboxPlanId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeMailboxPlanRetentionPolicyTags",
                new SqlParameter("@MailboxPlanId", MailboxPlanId)
            );
        }

                
        #endregion

        #region Exchange Disclaimers
        public static int AddExchangeDisclaimer(int itemID, ExchangeDisclaimer disclaimer)
        {
            SqlParameter outParam = new SqlParameter("@ExchangeDisclaimerId", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddExchangeDisclaimer",
                outParam,

                new SqlParameter("@ItemID", itemID),
                new SqlParameter("@DisclaimerName", disclaimer.DisclaimerName),
                new SqlParameter("@DisclaimerText", disclaimer.DisclaimerText)
            );

            return Convert.ToInt32(outParam.Value);
        }

        public static void UpdateExchangeDisclaimer(int itemID, ExchangeDisclaimer disclaimer)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateExchangeDisclaimer",
                new SqlParameter("@ExchangeDisclaimerId", disclaimer.ExchangeDisclaimerId),
                new SqlParameter("@DisclaimerName", disclaimer.DisclaimerName),
                new SqlParameter("@DisclaimerText", disclaimer.DisclaimerText)
            );
        }

        public static void DeleteExchangeDisclaimer(int exchangeDisclaimerId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteExchangeDisclaimer",
                new SqlParameter("@ExchangeDisclaimerId", exchangeDisclaimerId)
            );
        }

        public static IDataReader GetExchangeDisclaimer(int exchangeDisclaimerId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeDisclaimer",
                new SqlParameter("@ExchangeDisclaimerId", exchangeDisclaimerId)
            );
        }

        public static IDataReader GetExchangeDisclaimers(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeDisclaimers",
                new SqlParameter("@ItemID", itemId)
            );
        }

        public static void SetExchangeAccountDisclaimerId(int AccountID, int ExchangeDisclaimerId)
        {
            object id = null;
            if (ExchangeDisclaimerId != -1) id = ExchangeDisclaimerId;

            SqlHelper.ExecuteNonQuery(
                            ConnectionString,
                            CommandType.StoredProcedure,
                            "SetExchangeAccountDisclaimerId",
                            new SqlParameter("@AccountID", AccountID),
                            new SqlParameter("@ExchangeDisclaimerId", id)
                        );
        }

        public static int GetExchangeAccountDisclaimerId(int AccountID)
        {
            object objReturn = SqlHelper.ExecuteScalar(
                            ConnectionString,
                            CommandType.StoredProcedure,
                            "GetExchangeAccountDisclaimerId",
                            new SqlParameter("@AccountID", AccountID)
                        );

            int ret;
            if (!int.TryParse(objReturn.ToString(), out ret)) return -1;
            return ret;
        }

        #endregion

        #region Organizations

        public static int AddAccessToken(AccessToken token)
        {
            return AddAccessToken(token.AccessTokenGuid, token.AccountId, token.ItemId, token.ExpirationDate, token.TokenType);
        }

        public static int AddAccessToken(Guid accessToken, int accountId, int itemId, DateTime expirationDate, AccessTokenTypes type)
        {
            SqlParameter prmId = new SqlParameter("@TokenID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddAccessToken",
                prmId,
                new SqlParameter("@AccessToken", accessToken),
                new SqlParameter("@ExpirationDate", expirationDate),
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@TokenType", (int)type)
            );

            // read identity
            return Convert.ToInt32(prmId.Value);
        }

        public static void SetAccessTokenResponseMessage(Guid accessToken, string response)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "SetAccessTokenSmsResponse",
                new SqlParameter("@AccessToken", accessToken),
                new SqlParameter("@SmsResponse", response)
            );
        }

        public static void DeleteExpiredAccessTokens()
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteExpiredAccessTokenTokens"
            );
        }

        public static IDataReader GetAccessTokenByAccessToken(Guid accessToken, AccessTokenTypes type)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetAccessTokenByAccessToken",
                new SqlParameter("@AccessToken", accessToken),
                new SqlParameter("@TokenType", type)
            );
        }

        public static void DeleteAccessToken(Guid accessToken, AccessTokenTypes type)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteAccessToken",
                new SqlParameter("@AccessToken", accessToken),
                new SqlParameter("@TokenType", type)
            );
        }

        public static void UpdateOrganizationSettings(int itemId, string settingsName, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateExchangeOrganizationSettings",
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@SettingsName", settingsName),
                new SqlParameter("@Xml", xml));
        }

        public static IDataReader GetOrganizationSettings(int itemId, string settingsName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetExchangeOrganizationSettings",
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@SettingsName", settingsName));
        }

        public static int AddOrganizationDeletedUser(int accountId, int originAT, string storagePath, string folderName, string fileName, DateTime expirationDate)
        {
            SqlParameter outParam = new SqlParameter("@ID", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddOrganizationDeletedUser",
                outParam,
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@OriginAT", originAT),
                new SqlParameter("@StoragePath", storagePath),
                new SqlParameter("@FolderName", folderName),
                new SqlParameter("@FileName", fileName),
                new SqlParameter("@ExpirationDate", expirationDate)
            );

            return Convert.ToInt32(outParam.Value);
        }

        public static void DeleteOrganizationDeletedUser(int id)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                CommandType.StoredProcedure,
                "DeleteOrganizationDeletedUser",
                new SqlParameter("@ID", id));
        }

        public static IDataReader GetOrganizationDeletedUser(int accountId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetOrganizationDeletedUser",
                new SqlParameter("@AccountID", accountId)
            );
        }        

        public static IDataReader GetAdditionalGroups(int userId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetAdditionalGroups",
                new SqlParameter("@UserID", userId)
            );
        }


        public static int AddAdditionalGroup(int userId, string groupName)
        {
            SqlParameter prmId = new SqlParameter("@GroupID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddAdditionalGroup",
                prmId,
                new SqlParameter("@UserID", userId),
                new SqlParameter("@GroupName", groupName));

            // read identity
            return Convert.ToInt32(prmId.Value);
        }

        public static void DeleteAdditionalGroup(int groupId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                CommandType.StoredProcedure,
                "DeleteAdditionalGroup",
                new SqlParameter("@GroupID", groupId));
        }

        public static void UpdateAdditionalGroup(int groupId, string groupName)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateAdditionalGroup",
                new SqlParameter("@GroupID", groupId),
                new SqlParameter("@GroupName", groupName)
            );
        }

        public static void DeleteOrganizationUser(int itemId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeleteOrganizationUsers", new SqlParameter("@ItemID", itemId));
        }

        public static int GetItemIdByOrganizationId(string id)
        {
            object obj = SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetItemIdByOrganizationId",
                                    new SqlParameter("@OrganizationId", id));

            return (obj == null || DBNull.Value == obj) ? 0 : (int)obj;

        }

        public static IDataReader GetOrganizationStatistics(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetOrganizationStatistics",
                new SqlParameter("@ItemID", itemId)
            );
        }

        public static IDataReader GetOrganizationGroupsByDisplayName(int itemId, string displayName)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetOrganizationGroupsByDisplayName",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@DisplayName", displayName)
            );
        }

        public static IDataReader SearchOrganizationAccounts(int actorId, int itemId,
                string filterColumn, string filterValue, string sortColumn, bool includeMailboxes)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "SearchOrganizationAccounts",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@IncludeMailboxes", includeMailboxes)
            );
        }

        public static DataSet GetOrganizationObjectsByDomain(int itemId, string domainName)
        {
            return SqlHelper.ExecuteDataset(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetOrganizationObjectsByDomain",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@DomainName", domainName)
            );
        }


        #endregion

        #region CRM

        public static int GetCRMUsersCount(int itemId, string name, string email, int CALType)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ItemID", itemId),
                    GetFilterSqlParam("@Name", name),
                    GetFilterSqlParam("@Email", email),
                    new SqlParameter("@CALType", CALType)
                };

            return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetCRMUsersCount", sqlParams);

        }

        private static SqlParameter GetFilterSqlParam(string paramName, string value)
        {
            if (string.IsNullOrEmpty(value))
                return new SqlParameter(paramName, DBNull.Value);

            return new SqlParameter(paramName, value);
        }

        public static IDataReader GetCrmUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ItemID", itemId),
                    new SqlParameter("@SortColumn", sortColumn),
                    new SqlParameter("@SortDirection", sortDirection),                    
                    GetFilterSqlParam("@Name", name),
                    GetFilterSqlParam("@Email", email),                    
                    new SqlParameter("@StartRow", startRow),
                    new SqlParameter("Count", count)
                };


            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetCRMUsers", sqlParams);
        }

        public static IDataReader GetCRMOrganizationUsers(int itemId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "GetCRMOrganizationUsers",
                                           new SqlParameter[] { new SqlParameter("@ItemID", itemId) });
        }

        public static void CreateCRMUser(int itemId, Guid crmId, Guid businessUnitId, int CALType)
        {
            SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "InsertCRMUser",
                                    new SqlParameter[]
                                        {
                                            new SqlParameter("@ItemID", itemId),
                                            new SqlParameter("@CrmUserID", crmId),
                                            new SqlParameter("@BusinessUnitId", businessUnitId),
                                            new SqlParameter("@CALType", CALType)
                                        });

        }

        public static void UpdateCRMUser(int itemId, int CALType)
        {
            SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "UpdateCRMUser",
                                    new SqlParameter[]
                                        {
                                            new SqlParameter("@ItemID", itemId),
                                            new SqlParameter("@CALType", CALType)
                                        });

        }

        public static IDataReader GetCrmUser(int itemId)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "GetCRMUser",
                                    new SqlParameter[]
                                        {
                                            new SqlParameter("@AccountID", itemId)
                                        });
            return reader;

        }

        public static int GetCrmUserCount(int itemId)
        {
            return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetOrganizationCRMUserCount",
                new SqlParameter[] { new SqlParameter("@ItemID", itemId) });
        }

        public static void DeleteCrmOrganization(int organizationId)
        {
            SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "DeleteCRMOrganization",
                                    new SqlParameter[] { new SqlParameter("@ItemID", organizationId) });
        }

        #endregion

        #region VPS - Virtual Private Servers

        public static IDataReader GetVirtualMachinesPaged(int actorId, int packageId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetVirtualMachinesPaged",
                                        new SqlParameter("@ActorID", actorId),
                                        new SqlParameter("@PackageID", packageId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@StartRow", startRow),
                                        new SqlParameter("@MaximumRows", maximumRows),
                                        new SqlParameter("@Recursive", recursive));
            return reader;
        }
        public static IDataReader GetVirtualMachinesPaged2012(int actorId, int packageId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetVirtualMachinesPaged2012",
                                        new SqlParameter("@ActorID", actorId),
                                        new SqlParameter("@PackageID", packageId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@StartRow", startRow),
                                        new SqlParameter("@MaximumRows", maximumRows),
                                        new SqlParameter("@Recursive", recursive));
            return reader;
        }

        public static IDataReader GetVirtualMachinesPagedProxmox(int actorId, int packageId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetVirtualMachinesPagedProxmox",
                                        new SqlParameter("@ActorID", actorId),
                                        new SqlParameter("@PackageID", packageId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@StartRow", startRow),
                                        new SqlParameter("@MaximumRows", maximumRows),
                                        new SqlParameter("@Recursive", recursive));
            return reader;
        }

        #endregion

        public static IDataReader GetVirtualMachinesForPCPaged(int actorId, int packageId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetVirtualMachinesPagedForPC",
                                        new SqlParameter("@ActorID", actorId),
                                        new SqlParameter("@PackageID", packageId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@StartRow", startRow),
                                        new SqlParameter("@MaximumRows", maximumRows),
                                        new SqlParameter("@Recursive", recursive));
            return reader;
        }


        #region VPS - External Network

        public static IDataReader GetUnallottedIPAddresses(int packageId, int serviceId, int poolId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetUnallottedIPAddresses",
                                        new SqlParameter("@PackageId", packageId),
                                        new SqlParameter("@ServiceId", serviceId),
                                        new SqlParameter("@PoolId", poolId));
        }


        public static void AllocatePackageIPAddresses(int packageId, int orgId, string xml)
        {
            SqlParameter[] param = new[]
                                       {
                                           new SqlParameter("@PackageID", packageId),
                                           new SqlParameter("@OrgID", orgId),
                                           new SqlParameter("@xml", xml)
                                       };

            ExecuteLongNonQuery("AllocatePackageIPAddresses", param);
        }

        public static IDataReader GetPackageIPAddresses(int packageId, int orgId, int poolId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetPackageIPAddresses",
                                        new SqlParameter("@PackageID", packageId),
                                        new SqlParameter("@OrgID", orgId),
                                        new SqlParameter("@PoolId", poolId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@startRow", startRow),
                                        new SqlParameter("@maximumRows", maximumRows),
                                        new SqlParameter("@Recursive", recursive));
            return reader;
        }

        public static int GetPackageIPAddressesCount(int packageId, int orgId, int poolId)
        {
            object obj = SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure,
                                     "GetPackageIPAddressesCount",
                                        new SqlParameter("@PackageID", packageId),
                                        new SqlParameter("@OrgID", orgId),
                                        new SqlParameter("@PoolId", poolId));
            int res = 0;
            int.TryParse(obj.ToString(), out res);
            return res;
        }

        public static void DeallocatePackageIPAddress(int id)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeallocatePackageIPAddress",
                                      new SqlParameter("@PackageAddressID", id));
        }
        #endregion

        #region VPS - Private Network

        public static IDataReader GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetPackagePrivateIPAddressesPaged",
                                        new SqlParameter("@PackageID", packageId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@startRow", startRow),
                                        new SqlParameter("@maximumRows", maximumRows));
            return reader;
        }

        public static IDataReader GetPackagePrivateIPAddresses(int packageId)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetPackagePrivateIPAddresses",
                                        new SqlParameter("@PackageID", packageId));
            return reader;
        }
        #endregion

        #region VPS - External Network Adapter
        public static IDataReader GetPackageUnassignedIPAddresses(int actorId, int packageId, int orgId, int poolId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                "GetPackageUnassignedIPAddresses",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@PackageID", packageId),
                                new SqlParameter("@OrgID", orgId),
                                new SqlParameter("@PoolId", poolId));
        }

        public static IDataReader GetPackageIPAddress(int packageAddressId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                "GetPackageIPAddress",
                                new SqlParameter("@PackageAddressId", packageAddressId));
        }

        public static IDataReader GetItemIPAddresses(int actorId, int itemId, int poolId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                "GetItemIPAddresses",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PoolID", poolId));
        }

        public static int AddItemIPAddress(int actorId, int itemId, int packageAddressId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "AddItemIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PackageAddressID", packageAddressId));
        }

        public static int SetItemPrimaryIPAddress(int actorId, int itemId, int packageAddressId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "SetItemPrimaryIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PackageAddressID", packageAddressId));
        }

        public static int DeleteItemIPAddress(int actorId, int itemId, int packageAddressId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "DeleteItemIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PackageAddressID", packageAddressId));
        }

        public static int DeleteItemIPAddresses(int actorId, int itemId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "DeleteItemIPAddresses",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId));
        }
        #endregion

        #region VPS - Private Network Adapter
        public static IDataReader GetItemPrivateIPAddresses(int actorId, int itemId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                "GetItemPrivateIPAddresses",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId));
        }

        public static int AddItemPrivateIPAddress(int actorId, int itemId, string ipAddress)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "AddItemPrivateIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@IPAddress", ipAddress));
        }

        public static int SetItemPrivatePrimaryIPAddress(int actorId, int itemId, int privateAddressId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "SetItemPrivatePrimaryIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PrivateAddressID", privateAddressId));
        }

        public static int DeleteItemPrivateIPAddress(int actorId, int itemId, int privateAddressId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "DeleteItemPrivateIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PrivateAddressID", privateAddressId));
        }

        public static int DeleteItemPrivateIPAddresses(int actorId, int itemId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "DeleteItemPrivateIPAddresses",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId));
        }
        #endregion

        #region BlackBerry

        public static void AddBlackBerryUser(int accountId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "AddBlackBerryUser",
                                      new[]
                                          {                                              
                                              new SqlParameter("@AccountID", accountId)
                                          });
        }


        public static bool CheckBlackBerryUserExists(int accountId)
        {
            int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckBlackBerryUserExists",
                                    new SqlParameter("@AccountID", accountId));
            return res > 0;
        }


        public static IDataReader GetBlackBerryUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ItemID", itemId),
                    new SqlParameter("@SortColumn", sortColumn),
                    new SqlParameter("@SortDirection", sortDirection),                    
                    GetFilterSqlParam("@Name", name),
                    GetFilterSqlParam("@Email", email),                    
                    new SqlParameter("@StartRow", startRow),
                    new SqlParameter("Count", count)
                };


            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetBlackBerryUsers", sqlParams);
        }

        public static int GetBlackBerryUsersCount(int itemId, string name, string email)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                                           {
                                               new SqlParameter("@ItemID", itemId),
                                               GetFilterSqlParam("@Name", name),
                                               GetFilterSqlParam("@Email", email),
                                           };

            return
                (int)
                SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetBlackBerryUsersCount", sqlParams);
        }

        public static void DeleteBlackBerryUser(int accountId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "DeleteBlackBerryUser",
                                      new[]
                                          {                                              
                                              new SqlParameter("@AccountID", accountId)
                                          });

        }

        #endregion

        #region OCS

        public static void AddOCSUser(int accountId, string instanceId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "AddOCSUser",
                                      new[]
                                          {                                              
                                              new SqlParameter("@AccountID", accountId),
                                              new SqlParameter("@InstanceID", instanceId)
                                          });
        }


        public static bool CheckOCSUserExists(int accountId)
        {
            int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckOCSUserExists",
                                    new SqlParameter("@AccountID", accountId));
            return res > 0;
        }


        public static IDataReader GetOCSUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ItemID", itemId),
                    new SqlParameter("@SortColumn", sortColumn),
                    new SqlParameter("@SortDirection", sortDirection),                    
                    GetFilterSqlParam("@Name", name),
                    GetFilterSqlParam("@Email", email),                    
                    new SqlParameter("@StartRow", startRow),
                    new SqlParameter("Count", count)
                };


            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetOCSUsers", sqlParams);
        }

        public static int GetOCSUsersCount(int itemId, string name, string email)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                                           {
                                               new SqlParameter("@ItemID", itemId),
                                               GetFilterSqlParam("@Name", name),
                                               GetFilterSqlParam("@Email", email),
                                           };

            return
                (int)
                SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetOCSUsersCount", sqlParams);
        }

        public static void DeleteOCSUser(string instanceId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "DeleteOCSUser",
                                      new[]
                                          {                                              
                                              new SqlParameter("@InstanceId", instanceId)
                                          });

        }

        public static string GetOCSUserInstanceID(int accountId)
        {
            return (string)SqlHelper.ExecuteScalar(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "GetInstanceID",
                                      new[]
                                          {                                              
                                              new SqlParameter("@AccountID", accountId)
                                          });
        }

        #endregion

        #region SSL
        public static int AddSSLRequest(int actorId, int packageId, int siteID, int userID, string friendlyname, string hostname, string csr, int csrLength, string distinguishedName, bool isRenewal, int previousID)
        {
            SqlParameter prmId = new SqlParameter("@SSLID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddSSLRequest", prmId,
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@UserID", userID),
                new SqlParameter("@WebSiteID", siteID),
                new SqlParameter("@FriendlyName", friendlyname),
                new SqlParameter("@HostName", hostname),
                new SqlParameter("@CSR", csr),
                new SqlParameter("@CSRLength", csrLength),
                new SqlParameter("@DistinguishedName", distinguishedName),
                new SqlParameter("@IsRenewal", isRenewal),
                new SqlParameter("@PreviousId", previousID)
                );
            return Convert.ToInt32(prmId.Value);

        }

        public static void CompleteSSLRequest(int actorId, int packageId, int id, string certificate, string distinguishedName, string serialNumber, byte[] hash, DateTime validFrom, DateTime expiryDate)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CompleteSSLRequest",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@ID", id),
                new SqlParameter("@DistinguishedName", distinguishedName),
                new SqlParameter("@Certificate", certificate),
                new SqlParameter("@SerialNumber", serialNumber),
                new SqlParameter("@Hash", Convert.ToBase64String(hash)),
                new SqlParameter("@ValidFrom", validFrom),
                new SqlParameter("@ExpiryDate", expiryDate));

        }

        public static void AddPFX(int actorId, int packageId, int siteID, int userID, string hostname, string friendlyName, string distinguishedName, int csrLength, string serialNumber, DateTime validFrom, DateTime expiryDate)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddPFX",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@UserID", userID),
                new SqlParameter("@WebSiteID", siteID),
                new SqlParameter("@FriendlyName", friendlyName),
                new SqlParameter("@HostName", hostname),
                new SqlParameter("@CSRLength", csrLength),
                new SqlParameter("@DistinguishedName", distinguishedName),
                new SqlParameter("@SerialNumber", serialNumber),
                new SqlParameter("@ValidFrom", validFrom),
                new SqlParameter("@ExpiryDate", expiryDate));

        }

        public static DataSet GetSSL(int actorId, int packageId, int id)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSSL",
                new SqlParameter("@SSLID", id));

        }

        public static DataSet GetCertificatesForSite(int actorId, int packageId, int siteId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetCertificatesForSite",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@websiteid", siteId));

        }

        public static DataSet GetPendingCertificates(int actorId, int packageId, int id, bool recursive)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPendingSSLForWebsite",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@websiteid", id),
                new SqlParameter("@Recursive", recursive));

        }

        public static IDataReader GetSSLCertificateByID(int actorId, int id)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSSLCertificateByID",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@ID", id));
        }

        public static int CheckSSL(int siteID, bool renewal)
        {
            SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckSSL",
                prmId,
                new SqlParameter("@siteID", siteID),
                new SqlParameter("@Renewal", renewal));

            return Convert.ToInt32(prmId.Value);
        }

        public static IDataReader GetSiteCert(int actorId, int siteID)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSSLCertificateByID",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@ID", siteID));
        }

        public static void DeleteCertificate(int actorId, int packageId, int id)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteCertificate",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@id", id));
        }

        public static bool CheckSSLExistsForWebsite(int siteId)
        {
            SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Bit);
            prmId.Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckSSLExistsForWebsite", prmId,
                new SqlParameter("@siteID", siteId),
                new SqlParameter("@SerialNumber", ""));
            return Convert.ToBoolean(prmId.Value);
        }
        #endregion

        #region Lync

        public static void AddLyncUser(int accountId, int lyncUserPlanId, string sipAddress)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "AddLyncUser",
                                      new[]
                                          {                                              
                                              new SqlParameter("@AccountID", accountId),
                                              new SqlParameter("@LyncUserPlanID", lyncUserPlanId),
                                              new SqlParameter("@SipAddress", sipAddress)
                                          });
        }

        public static void UpdateLyncUser(int accountId, string sipAddress)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "UpdateLyncUser",
                                      new[]
                                          {                                              
                                              new SqlParameter("@AccountID", accountId),
                                              new SqlParameter("@SipAddress", sipAddress)
                                          });
        }


        public static bool CheckLyncUserExists(int accountId)
        {
            int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckLyncUserExists",
                                    new SqlParameter("@AccountID", accountId));
            return res > 0;
        }

        public static bool LyncUserExists(int accountId, string sipAddress)
        {
            SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "LyncUserExists",
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@SipAddress", sipAddress),
                outParam
            );

            return Convert.ToBoolean(outParam.Value);
        }



        public static IDataReader GetLyncUsers(int itemId, string sortColumn, string sortDirection, int startRow, int count)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ItemID", itemId),
                    new SqlParameter("@SortColumn", sortColumn),
                    new SqlParameter("@SortDirection", sortDirection),                    
                    new SqlParameter("@StartRow", startRow),
                    new SqlParameter("Count", count)
                };


            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetLyncUsers", sqlParams);
        }


        public static IDataReader GetLyncUsersByPlanId(int itemId, int planId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetLyncUsersByPlanId",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@PlanId", planId)
            );
        }

        public static int GetLyncUsersCount(int itemId)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                                           {
                                               new SqlParameter("@ItemID", itemId)
                                           };

            return
                (int)
                SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetLyncUsersCount", sqlParams);
        }

        public static void DeleteLyncUser(int accountId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "DeleteLyncUser",
                                      new[]
                                          {                                              
                                              new SqlParameter("@AccountId", accountId)
                                          });

        }

        public static int AddLyncUserPlan(int itemID, LyncUserPlan lyncUserPlan)
        {
            SqlParameter outParam = new SqlParameter("@LyncUserPlanId", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddLyncUserPlan",
                outParam,

                new SqlParameter("@ItemID", itemID),
                new SqlParameter("@LyncUserPlanName", lyncUserPlan.LyncUserPlanName),
                new SqlParameter("@LyncUserPlanType", lyncUserPlan.LyncUserPlanType),
                new SqlParameter("@IM", lyncUserPlan.IM),
                new SqlParameter("@Mobility", lyncUserPlan.Mobility),
                new SqlParameter("@MobilityEnableOutsideVoice", lyncUserPlan.MobilityEnableOutsideVoice),
                new SqlParameter("@Federation", lyncUserPlan.Federation),
                new SqlParameter("@Conferencing", lyncUserPlan.Conferencing),
                new SqlParameter("@EnterpriseVoice", lyncUserPlan.EnterpriseVoice),
                new SqlParameter("@VoicePolicy", lyncUserPlan.VoicePolicy),
                new SqlParameter("@IsDefault", lyncUserPlan.IsDefault),

                new SqlParameter("@RemoteUserAccess", lyncUserPlan.RemoteUserAccess),
                new SqlParameter("@PublicIMConnectivity", lyncUserPlan.PublicIMConnectivity),

                new SqlParameter("@AllowOrganizeMeetingsWithExternalAnonymous", lyncUserPlan.AllowOrganizeMeetingsWithExternalAnonymous),

                new SqlParameter("@Telephony", lyncUserPlan.Telephony),

                new SqlParameter("@ServerURI", lyncUserPlan.ServerURI),

                new SqlParameter("@ArchivePolicy", lyncUserPlan.ArchivePolicy),
                new SqlParameter("@TelephonyDialPlanPolicy", lyncUserPlan.TelephonyDialPlanPolicy),
                new SqlParameter("@TelephonyVoicePolicy", lyncUserPlan.TelephonyVoicePolicy)
            );

            return Convert.ToInt32(outParam.Value);
        }


        public static void UpdateLyncUserPlan(int itemID, LyncUserPlan lyncUserPlan)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateLyncUserPlan",
                new SqlParameter("@LyncUserPlanId", lyncUserPlan.LyncUserPlanId),
                new SqlParameter("@LyncUserPlanName", lyncUserPlan.LyncUserPlanName),
                new SqlParameter("@LyncUserPlanType", lyncUserPlan.LyncUserPlanType),
                new SqlParameter("@IM", lyncUserPlan.IM),
                new SqlParameter("@Mobility", lyncUserPlan.Mobility),
                new SqlParameter("@MobilityEnableOutsideVoice", lyncUserPlan.MobilityEnableOutsideVoice),
                new SqlParameter("@Federation", lyncUserPlan.Federation),
                new SqlParameter("@Conferencing", lyncUserPlan.Conferencing),
                new SqlParameter("@EnterpriseVoice", lyncUserPlan.EnterpriseVoice),
                new SqlParameter("@VoicePolicy", lyncUserPlan.VoicePolicy),
                new SqlParameter("@IsDefault", lyncUserPlan.IsDefault),

                new SqlParameter("@RemoteUserAccess", lyncUserPlan.RemoteUserAccess),
                new SqlParameter("@PublicIMConnectivity", lyncUserPlan.PublicIMConnectivity),

                new SqlParameter("@AllowOrganizeMeetingsWithExternalAnonymous", lyncUserPlan.AllowOrganizeMeetingsWithExternalAnonymous),

                new SqlParameter("@Telephony", lyncUserPlan.Telephony),

                new SqlParameter("@ServerURI", lyncUserPlan.ServerURI),

                new SqlParameter("@ArchivePolicy", lyncUserPlan.ArchivePolicy),
                new SqlParameter("@TelephonyDialPlanPolicy", lyncUserPlan.TelephonyDialPlanPolicy),
                new SqlParameter("@TelephonyVoicePolicy", lyncUserPlan.TelephonyVoicePolicy)
            );
        }

        public static void DeleteLyncUserPlan(int lyncUserPlanId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteLyncUserPlan",
                new SqlParameter("@LyncUserPlanId", lyncUserPlanId)
            );
        }

        public static IDataReader GetLyncUserPlan(int lyncUserPlanId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetLyncUserPlan",
                new SqlParameter("@LyncUserPlanId", lyncUserPlanId)
            );
        }


        public static IDataReader GetLyncUserPlans(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetLyncUserPlans",
                new SqlParameter("@ItemID", itemId)
            );
        }


        public static void SetOrganizationDefaultLyncUserPlan(int itemId, int lyncUserPlanId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "SetOrganizationDefaultLyncUserPlan",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@LyncUserPlanId", lyncUserPlanId)
            );
        }

        public static IDataReader GetLyncUserPlanByAccountId(int AccountId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetLyncUserPlanByAccountId",
                new SqlParameter("@AccountID", AccountId)
            );
        }


        public static void SetLyncUserLyncUserplan(int accountId, int lyncUserPlanId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "SetLyncUserLyncUserplan",
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@LyncUserPlanId", (lyncUserPlanId == 0) ? (object)DBNull.Value : (object)lyncUserPlanId)
            );
        }


        #endregion

        #region SfB

        public static void AddSfBUser(int accountId, int sfbUserPlanId, string sipAddress)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "AddSfBUser",
                                      new[]
                                          {
                                              new SqlParameter("@AccountID", accountId),
                                              new SqlParameter("@SfBUserPlanID", sfbUserPlanId),
                                              new SqlParameter("@SipAddress", sipAddress)
                                          });
        }

        public static void UpdateSfBUser(int accountId, string sipAddress)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "UpdateSfBUser",
                                      new[]
                                          {
                                              new SqlParameter("@AccountID", accountId),
                                              new SqlParameter("@SipAddress", sipAddress)
                                          });
        }


        public static bool CheckSfBUserExists(int accountId)
        {
            int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckSfBUserExists",
                                    new SqlParameter("@AccountID", accountId));
            return res > 0;
        }

        public static bool SfBUserExists(int accountId, string sipAddress)
        {
            SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "SfBUserExists",
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@SipAddress", sipAddress),
                outParam
            );

            return Convert.ToBoolean(outParam.Value);
        }



        public static IDataReader GetSfBUsers(int itemId, string sortColumn, string sortDirection, int startRow, int count)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ItemID", itemId),
                    new SqlParameter("@SortColumn", sortColumn),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@StartRow", startRow),
                    new SqlParameter("Count", count)
                };


            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetSfBUsers", sqlParams);
        }


        public static IDataReader GetSfBUsersByPlanId(int itemId, int planId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetSfBUsersByPlanId",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@PlanId", planId)
            );
        }

        public static int GetSfBUsersCount(int itemId)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                                           {
                                               new SqlParameter("@ItemID", itemId)
                                           };

            return
                (int)
                SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetSfBUsersCount", sqlParams);
        }

        public static void DeleteSfBUser(int accountId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "DeleteSfBUser",
                                      new[]
                                          {
                                              new SqlParameter("@AccountId", accountId)
                                          });

        }

        public static int AddSfBUserPlan(int itemID, SfBUserPlan sfbUserPlan)
        {
            SqlParameter outParam = new SqlParameter("@SfBUserPlanId", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddSfBUserPlan",
                outParam,

                new SqlParameter("@ItemID", itemID),
                new SqlParameter("@SfBUserPlanName", sfbUserPlan.SfBUserPlanName),
                new SqlParameter("@SfBUserPlanType", sfbUserPlan.SfBUserPlanType),
                new SqlParameter("@IM", sfbUserPlan.IM),
                new SqlParameter("@Mobility", sfbUserPlan.Mobility),
                new SqlParameter("@MobilityEnableOutsideVoice", sfbUserPlan.MobilityEnableOutsideVoice),
                new SqlParameter("@Federation", sfbUserPlan.Federation),
                new SqlParameter("@Conferencing", sfbUserPlan.Conferencing),
                new SqlParameter("@EnterpriseVoice", sfbUserPlan.EnterpriseVoice),
                new SqlParameter("@VoicePolicy", sfbUserPlan.VoicePolicy),
                new SqlParameter("@IsDefault", sfbUserPlan.IsDefault),

                new SqlParameter("@RemoteUserAccess", sfbUserPlan.RemoteUserAccess),
                new SqlParameter("@PublicIMConnectivity", sfbUserPlan.PublicIMConnectivity),

                new SqlParameter("@AllowOrganizeMeetingsWithExternalAnonymous", sfbUserPlan.AllowOrganizeMeetingsWithExternalAnonymous),

                new SqlParameter("@Telephony", sfbUserPlan.Telephony),

                new SqlParameter("@ServerURI", sfbUserPlan.ServerURI),

                new SqlParameter("@ArchivePolicy", sfbUserPlan.ArchivePolicy),
                new SqlParameter("@TelephonyDialPlanPolicy", sfbUserPlan.TelephonyDialPlanPolicy),
                new SqlParameter("@TelephonyVoicePolicy", sfbUserPlan.TelephonyVoicePolicy)
            );

            return Convert.ToInt32(outParam.Value);
        }


        public static void UpdateSfBUserPlan(int itemID, SfBUserPlan sfbUserPlan)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateSfBUserPlan",
                new SqlParameter("@SfBUserPlanId", sfbUserPlan.SfBUserPlanId),
                new SqlParameter("@SfBUserPlanName", sfbUserPlan.SfBUserPlanName),
                new SqlParameter("@SfBUserPlanType", sfbUserPlan.SfBUserPlanType),
                new SqlParameter("@IM", sfbUserPlan.IM),
                new SqlParameter("@Mobility", sfbUserPlan.Mobility),
                new SqlParameter("@MobilityEnableOutsideVoice", sfbUserPlan.MobilityEnableOutsideVoice),
                new SqlParameter("@Federation", sfbUserPlan.Federation),
                new SqlParameter("@Conferencing", sfbUserPlan.Conferencing),
                new SqlParameter("@EnterpriseVoice", sfbUserPlan.EnterpriseVoice),
                new SqlParameter("@VoicePolicy", sfbUserPlan.VoicePolicy),
                new SqlParameter("@IsDefault", sfbUserPlan.IsDefault),

                new SqlParameter("@RemoteUserAccess", sfbUserPlan.RemoteUserAccess),
                new SqlParameter("@PublicIMConnectivity", sfbUserPlan.PublicIMConnectivity),

                new SqlParameter("@AllowOrganizeMeetingsWithExternalAnonymous", sfbUserPlan.AllowOrganizeMeetingsWithExternalAnonymous),

                new SqlParameter("@Telephony", sfbUserPlan.Telephony),

                new SqlParameter("@ServerURI", sfbUserPlan.ServerURI),

                new SqlParameter("@ArchivePolicy", sfbUserPlan.ArchivePolicy),
                new SqlParameter("@TelephonyDialPlanPolicy", sfbUserPlan.TelephonyDialPlanPolicy),
                new SqlParameter("@TelephonyVoicePolicy", sfbUserPlan.TelephonyVoicePolicy)
            );
        }

        public static void DeleteSfBUserPlan(int sfbUserPlanId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteSfBUserPlan",
                new SqlParameter("@SfBUserPlanId", sfbUserPlanId)
            );
        }

        public static IDataReader GetSfBUserPlan(int sfbUserPlanId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetSfBUserPlan",
                new SqlParameter("@SfBUserPlanId", sfbUserPlanId)
            );
        }


        public static IDataReader GetSfBUserPlans(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetSfBUserPlans",
                new SqlParameter("@ItemID", itemId)
            );
        }


        public static void SetOrganizationDefaultSfBUserPlan(int itemId, int sfbUserPlanId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "SetOrganizationDefaultSfBUserPlan",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@SfBUserPlanId", sfbUserPlanId)
            );
        }

        public static IDataReader GetSfBUserPlanByAccountId(int AccountId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetSfBUserPlanByAccountId",
                new SqlParameter("@AccountID", AccountId)
            );
        }


        public static void SetSfBUserSfBUserplan(int accountId, int sfbUserPlanId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "SetSfBUserSfBUserplan",
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@SfBUserPlanId", (sfbUserPlanId == 0) ? (object)DBNull.Value : (object)sfbUserPlanId)
            );
        }


        #endregion

        public static int GetPackageIdByName(string Name)
        {
            int packageId = -1;
            List<ProviderInfo> providers = ServerController.GetProviders();
            foreach (ProviderInfo providerInfo in providers)
            {
                if (string.Equals(Name, providerInfo.ProviderName, StringComparison.OrdinalIgnoreCase))
                {
                    packageId = providerInfo.ProviderId;
                    break;
                }
            }

            //if (-1 == packageId)
            //{
            //    throw new Exception("Provider not found");
            //}

            return packageId;
        }

        public static int GetServiceIdByProviderForServer(int providerId, int packageId)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
                @"SELECT TOP 1 
                    PackageServices.ServiceID
                  FROM PackageServices
                  LEFT JOIN Services ON Services.ServiceID = PackageServices.ServiceID
                  WHERE PackageServices.PackageID = @PackageID AND Services.ProviderID = @ProviderID",
                new SqlParameter("@ProviderID", providerId),
                new SqlParameter("@PackageID", packageId));

            if (reader.Read())
            {
                return (int)reader["ServiceID"];
            }

            return -1;
        }

        #region Helicon Zoo

        public static void GetHeliconZooProviderAndGroup(string providerName, out int providerId, out int groupId)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
                @"SELECT TOP 1 
                    ProviderID, GroupID
                  FROM Providers
                  WHERE ProviderName = @ProviderName",
                new SqlParameter("@ProviderName", providerName));

            reader.Read();

            providerId = (int)reader["ProviderID"];
            groupId = (int)reader["GroupID"];

        }

        public static IDataReader GetHeliconZooQuotas(int providerId)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
                @"SELECT
	                Q.QuotaID,
	                Q.GroupID,
	                Q.QuotaName,
	                Q.QuotaDescription,
	                Q.QuotaTypeID,
	                Q.ServiceQuota
                FROM Providers AS P
                INNER JOIN Quotas AS Q ON P.GroupID = Q.GroupID
                WHERE P.ProviderID = @ProviderID",
                new SqlParameter("@ProviderID", providerId));

            return reader;
        }

        public static void RemoveHeliconZooQuota(int groupId, string engineName)
        {
            int quotaId;

            // find quota id
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
                @"SELECT TOP 1 
                    QuotaID
                  FROM Quotas
                  WHERE QuotaName = @QuotaName AND GroupID = @GroupID",
                new SqlParameter("@QuotaName", engineName),
                new SqlParameter("@GroupID", groupId));

            reader.Read();
            quotaId = (int)reader["QuotaID"];

            // delete references from HostingPlanQuotas
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text,
                "DELETE FROM HostingPlanQuotas WHERE QuotaID = @QuotaID",
                new SqlParameter("@QuotaID", quotaId)
            );

            // delete from Quotas
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text,
                "DELETE FROM Quotas WHERE QuotaID = @QuotaID",
                new SqlParameter("@QuotaID", quotaId)
            );

        }

        public static void AddHeliconZooQuota(int groupId, int quotaId, string engineName, string engineDescription, int quotaOrder)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text,
                    @"INSERT INTO Quotas (QuotaID, GroupID, QuotaOrder, QuotaName, QuotaDescription, QuotaTypeID, ServiceQuota)
                    VALUES (@QuotaID, @GroupID, @QuotaOrder, @QuotaName, @QuotaDescription, 1, 0)",
                    new SqlParameter("@QuotaID", quotaId),
                    new SqlParameter("@GroupID", groupId),
                    new SqlParameter("@QuotaOrder", quotaOrder),
                    new SqlParameter("@QuotaName", engineName),
                    new SqlParameter("@QuotaDescription", engineDescription)
                );
        }

        public static IDataReader GetEnabledHeliconZooQuotasForPackage(int packageId)
        {
            int providerId, groupId;

            GetHeliconZooProviderAndGroup("HeliconZoo", out providerId, out groupId);

            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
                @"SELECT     HostingPlanQuotas.QuotaID, Quotas.QuotaName, Quotas.QuotaDescription
                FROM         HostingPlanQuotas 
                    INNER JOIN Packages ON HostingPlanQuotas.PlanID = Packages.PlanID 
                        INNER JOIN Quotas ON HostingPlanQuotas.QuotaID = Quotas.QuotaID
                WHERE     
                    (Packages.PackageID = @PackageID) AND (Quotas.GroupID = @GroupID) AND (HostingPlanQuotas.QuotaValue = 1)",
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@GroupID", groupId)
            );

            return reader;
        }

        public static int GetServiceIdForProviderIdAndPackageId(int providerId, int packageId)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
                @"SELECT PackageServices.ServiceID 
                FROM PackageServices
                INNER JOIN Services ON PackageServices.ServiceID = Services.ServiceID
                WHERE Services.ProviderID = @ProviderID and PackageID = @PackageID",
                new SqlParameter("@ProviderID", providerId),
                new SqlParameter("@PackageID", packageId)
            );

            if (reader.Read())
            {
                return (int)reader["ServiceID"];
            }

            return -1;

        }

        public static int GetServerIdForPackage(int packageId)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text,
                @"SELECT TOP 1 
                    ServerID
                  FROM Packages
                  WHERE PackageID = @PackageID",
                new SqlParameter("@PackageID", packageId)
            );

            if (reader.Read())
            {
                return (int)reader["ServerID"];
            }

            return -1;
        }

        #endregion

        #region Enterprise Storage

        public static int AddWebDavAccessToken(Base.HostedSolution.WebDavAccessToken accessToken)
        {
            SqlParameter prmId = new SqlParameter("@TokenID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddWebDavAccessToken",
                prmId,
                new SqlParameter("@AccessToken", accessToken.AccessToken),
                new SqlParameter("@FilePath", accessToken.FilePath),
                new SqlParameter("@AuthData", accessToken.AuthData),
                new SqlParameter("@ExpirationDate", accessToken.ExpirationDate),
                new SqlParameter("@AccountID", accessToken.AccountId),
                new SqlParameter("@ItemId", accessToken.ItemId)
            );

            // read identity
            return Convert.ToInt32(prmId.Value);
        }

        public static void DeleteExpiredWebDavAccessTokens()
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteExpiredWebDavAccessTokens"
            );
        }

        public static IDataReader GetWebDavAccessTokenById(int id)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetWebDavAccessTokenById",
                new SqlParameter("@Id", id)
            );
        }

        public static IDataReader GetWebDavAccessTokenByAccessToken(Guid accessToken)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetWebDavAccessTokenByAccessToken",
                new SqlParameter("@AccessToken", accessToken)
            );
        }

        public static int AddEntepriseFolder(int itemId, string folderName, int folderQuota, string locationDrive, string homeFolder, string domain, int? storageSpaceFolderId)
        {
            SqlParameter prmId = new SqlParameter("@FolderID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddEnterpriseFolder",
                prmId,
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@FolderName", folderName),
                new SqlParameter("@FolderQuota", folderQuota),
                new SqlParameter("@LocationDrive", locationDrive),
                new SqlParameter("@HomeFolder", homeFolder),
                new SqlParameter("@Domain", domain),
                new SqlParameter("@StorageSpaceFolderId", storageSpaceFolderId)
            );

            // read identity
            return Convert.ToInt32(prmId.Value);
        }

        public static void UpdateEntepriseFolderStorageSpaceFolder(int itemId, string folderName, int? storageSpaceFolderId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateEntepriseFolderStorageSpaceFolder",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@FolderName", folderName),
                new SqlParameter("@StorageSpaceFolderId", storageSpaceFolderId)
            );
        }

        public static void DeleteEnterpriseFolder(int itemId, string folderName)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteEnterpriseFolder",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@FolderName", folderName));
        }

        public static void UpdateEnterpriseFolder(int itemId, string folderID, string folderName, int folderQuota)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateEnterpriseFolder",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@FolderID", folderID),
                new SqlParameter("@FolderName", folderName),
                new SqlParameter("@FolderQuota", folderQuota));
        }

        public static IDataReader GetEnterpriseFolders(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetEnterpriseFolders",
                new SqlParameter("@ItemID", itemId)
            );
        }

        public static DataSet GetEnterpriseFoldersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetEnterpriseFoldersPaged",
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows)
            );
        }

        public static IDataReader GetEnterpriseFolder(int itemId, string folderName)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetEnterpriseFolder",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@FolderName", folderName)
            );
        }

        public static IDataReader GetWebDavPortalUserSettingsByAccountId(int accountId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetWebDavPortalUsersSettingsByAccountId",
                new SqlParameter("@AccountId", accountId)
            );
        }

        public static int AddWebDavPortalUsersSettings(int accountId, string settings)
        {
            SqlParameter settingsId = new SqlParameter("@WebDavPortalUsersSettingsId", SqlDbType.Int);
            settingsId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddWebDavPortalUsersSettings",
                settingsId,
                new SqlParameter("@AccountId", accountId),
                new SqlParameter("@Settings", settings)
            );

            // read identity
            return Convert.ToInt32(settingsId.Value);
        }

        public static void UpdateWebDavPortalUsersSettings(int accountId, string settings)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateWebDavPortalUsersSettings",
                new SqlParameter("@AccountId", accountId),
                new SqlParameter("@Settings", settings)
            );
        }

        public static void DeleteAllEnterpriseFolderOwaUsers(int itemId, int folderId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteAllEnterpriseFolderOwaUsers",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@FolderID", folderId)
            );
        }

        public static int AddEnterpriseFolderOwaUser(int itemId, int folderId, int accountId)
        {
            SqlParameter id = new SqlParameter("@ESOwsaUserId", SqlDbType.Int);
            id.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddEnterpriseFolderOwaUser",
                id,
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@FolderID", folderId),
                new SqlParameter("@AccountId", accountId)
            );

            // read identity
            return Convert.ToInt32(id.Value);
        }

        public static IDataReader GetEnterpriseFolderOwaUsers(int itemId, int folderId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetEnterpriseFolderOwaUsers",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@FolderID", folderId)
            );
        }

        public static IDataReader GetEnterpriseFolderId(int itemId, string folderName)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetEnterpriseFolderId",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@FolderName", folderName)
            );
        }

        public static IDataReader GetUserEnterpriseFolderWithOwaEditPermission(int itemId, int accountId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetUserEnterpriseFolderWithOwaEditPermission",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@AccountID", accountId)
            );
        }

        #endregion

        #region Support Service Levels

        public static IDataReader GetSupportServiceLevels()
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSupportServiceLevels");
        }

        public static int AddSupportServiceLevel(string levelName, string levelDescription)
        {
            SqlParameter outParam = new SqlParameter("@LevelID", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddSupportServiceLevel",
                outParam,
                new SqlParameter("@LevelName", levelName),
                new SqlParameter("@LevelDescription", levelDescription)
            );

            return Convert.ToInt32(outParam.Value);
        }

        public static void UpdateSupportServiceLevel(int levelID, string levelName, string levelDescription)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateSupportServiceLevel",
                new SqlParameter("@LevelID", levelID),
                new SqlParameter("@LevelName", levelName),
                new SqlParameter("@LevelDescription", levelDescription)
            );
        }

        public static void DeleteSupportServiceLevel(int levelID)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteSupportServiceLevel",
                new SqlParameter("@LevelID", levelID)
            );
        }

        public static IDataReader GetSupportServiceLevel(int levelID)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetSupportServiceLevel",
                new SqlParameter("@LevelID", levelID)
            );
        }

        public static bool CheckServiceLevelUsage(int levelID)
        {
            int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckServiceLevelUsage",
                                    new SqlParameter("@LevelID", levelID));
            return res > 0;
        }

        #endregion

        #region Storage Spaces 

        public static DataSet GetStorageSpaceLevelsPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetStorageSpaceLevelsPaged",
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows)
            );
        }

        public static IDataReader GetStorageSpaceLevelById(int id)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetStorageSpaceLevelById",
                new SqlParameter("@ID", id)
            );
        }

        public static int UpdateStorageSpaceLevel(StorageSpaceLevel level)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateStorageSpaceLevel",
                new SqlParameter("@ID", level.Id),
                new SqlParameter("@Name", level.Name),
                new SqlParameter("@Description", level.Description)
            );

            return level.Id;
        }

        public static int InsertStorageSpaceLevel(StorageSpaceLevel level)
        {
            SqlParameter id = new SqlParameter("@ID", SqlDbType.Int);
            id.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "InsertStorageSpaceLevel",
                id,
                new SqlParameter("@Name", level.Name),
                new SqlParameter("@Description", level.Description)
            );

            // read identity
            return Convert.ToInt32(id.Value);
        }

        public static void RemoveStorageSpaceLevel(int id)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "RemoveStorageSpaceLevel",
                new SqlParameter("@ID", id)
            );
        }

        public static IDataReader GetStorageSpaceLevelResourceGroups(int levelId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetLevelResourceGroups",
                new SqlParameter("@LevelId", levelId)
            );
        }

        public static void RemoveStorageSpaceLevelResourceGroups(int levelId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteLevelResourceGroups",
                new SqlParameter("@LevelId", levelId)
            );
        }

        public static void AddStorageSpaceLevelResourceGroup(int levelId, int groupId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddLevelResourceGroups",
                new SqlParameter("@LevelId", levelId),
                new SqlParameter("@GroupId", groupId)
            );
        }

        public static DataSet GetStorageSpacesPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetStorageSpacesPaged",
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows)
            );
        }

        public static IDataReader GetStorageSpaceById(int id)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetStorageSpaceById",
                new SqlParameter("@ID", id)
            );
        }

        public static IDataReader GetStorageSpaceByServiceAndPath(int serverId, string path)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetStorageSpaceByServiceAndPath",
                new SqlParameter("@ServerId", serverId),
                new SqlParameter("@Path", path)

            );
        }

        public static int UpdateStorageSpace(StorageSpace space)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateStorageSpace",
                new SqlParameter("@ID", space.Id),
                new SqlParameter("@Name", space.Name),
                new SqlParameter("@ServiceId", space.ServiceId),
                new SqlParameter("@ServerId", space.ServerId),
                new SqlParameter("@LevelId", space.LevelId),
                new SqlParameter("@Path", space.Path),
                new SqlParameter("@FsrmQuotaType", space.FsrmQuotaType),
                new SqlParameter("@FsrmQuotaSizeBytes", space.FsrmQuotaSizeBytes),
                new SqlParameter("@IsShared", space.IsShared),
                new SqlParameter("@IsDisabled", space.IsDisabled),
                new SqlParameter("@UncPath", space.UncPath)
            );

            return space.Id;
        }

        public static int InsertStorageSpace(StorageSpace space)
        {
            SqlParameter id = new SqlParameter("@ID", SqlDbType.Int);
            id.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "InsertStorageSpace",
                id,
                new SqlParameter("@Name", space.Name),
                new SqlParameter("@ServiceId", space.ServiceId),
                new SqlParameter("@ServerId", space.ServerId),
                new SqlParameter("@LevelId", space.LevelId),
                new SqlParameter("@Path", space.Path),
                new SqlParameter("@FsrmQuotaType", space.FsrmQuotaType),
                new SqlParameter("@FsrmQuotaSizeBytes", space.FsrmQuotaSizeBytes),
                new SqlParameter("@IsShared", space.IsShared),
                new SqlParameter("@IsDisabled", space.IsDisabled),
                new SqlParameter("@UncPath", space.UncPath)
            );

            // read identity
            return Convert.ToInt32(id.Value);
        }

        public static void RemoveStorageSpace(int id)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "RemoveStorageSpace",
                new SqlParameter("@ID", id)
            );
        }

        public static DataSet GetStorageSpacesByLevelId(int levelId)
        {
            return SqlHelper.ExecuteDataset(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetStorageSpacesByLevelId",
                new SqlParameter("@LevelId", levelId)
            );
        }

        public static IDataReader GetStorageSpacesByResourceGroupName(string groupName)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetStorageSpacesByResourceGroupName",
                new SqlParameter("@ResourceGroupName", groupName)
            );
        }

        public static int CreateStorageSpaceFolder(StorageSpaceFolder folder)
        {
            folder.Id = CreateStorageSpaceFolder(folder.Name, folder.StorageSpaceId, folder.Path, folder.UncPath, folder.IsShared, folder.FsrmQuotaType, folder.FsrmQuotaSizeBytes);

            return folder.Id;
        }

        public static int CreateStorageSpaceFolder(string name, int storageSpaceId, string path, string uncPath, bool isShared, QuotaType quotaType, long fsrmQuotaSizeBytes)
        {
            SqlParameter id = new SqlParameter("@ID", SqlDbType.Int);
            id.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "CreateStorageSpaceFolder",
                id,
                new SqlParameter("@Name", name),
                new SqlParameter("@StorageSpaceId", storageSpaceId),
                new SqlParameter("@Path", path),
                new SqlParameter("@UncPath", uncPath),
                new SqlParameter("@IsShared", isShared),
                new SqlParameter("@FsrmQuotaType", quotaType),
                new SqlParameter("@FsrmQuotaSizeBytes", fsrmQuotaSizeBytes)
            );

            // read identity
            return Convert.ToInt32(id.Value);
        }

        public static int UpdateStorageSpaceFolder(StorageSpaceFolder folder)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateStorageSpaceFolder",
                new SqlParameter("@ID", folder.Id),
                new SqlParameter("@Name", folder.Name),
                new SqlParameter("@StorageSpaceId", folder.StorageSpaceId),
                new SqlParameter("@Path", folder.Path),
                new SqlParameter("@UncPath", folder.UncPath),
                new SqlParameter("@IsShared", folder.IsShared),
                new SqlParameter("@FsrmQuotaType", folder.FsrmQuotaType),
                new SqlParameter("@FsrmQuotaSizeBytes", folder.FsrmQuotaSizeBytes)
            );

            return folder.Id;
        }

        public static int UpdateStorageSpaceFolder(int id, string folderName, int storageSpaceId, string path, string uncPath, bool isShared, QuotaType type, long fsrmQuotaSizeBytes)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateStorageSpaceFolder",
                new SqlParameter("@ID", id),
                new SqlParameter("@Name", folderName),
                new SqlParameter("@StorageSpaceId", storageSpaceId),
                new SqlParameter("@Path", path),
                new SqlParameter("@UncPath", uncPath),
                new SqlParameter("@IsShared", isShared),
                new SqlParameter("@FsrmQuotaType", type),
                new SqlParameter("@FsrmQuotaSizeBytes", fsrmQuotaSizeBytes)
            );

            return id;
        }

        public static IDataReader GetStorageSpaceFoldersByStorageSpaceId(int id)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetStorageSpaceFoldersByStorageSpaceId",
                new SqlParameter("@StorageSpaceId", id)
            );
        }

        public static IDataReader GetStorageSpaceFolderById(int id)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetStorageSpaceFolderById",
                new SqlParameter("@ID", id)
            );
        }

        public static void RemoveStorageSpaceFolder(int id)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "RemoveStorageSpaceFolder",
                new SqlParameter("@ID", id)
            );
        }

        #endregion

        #region RDS

        public static int CheckRDSServer(string ServerFQDN)
        {
            SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckRDSServer",
                prmId,
                new SqlParameter("@ServerFQDN", ServerFQDN));

            return Convert.ToInt32(prmId.Value);
        }

        public static IDataReader GetRdsServerSettings(int serverId, string settingsName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetRDSServerSettings",                
                new SqlParameter("@ServerId", serverId),
                new SqlParameter("@SettingsName", settingsName));
        }

        public static void UpdateRdsServerSettings(int serverId, string settingsName, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateRDSServerSettings",
                new SqlParameter("@ServerId", serverId),                
                new SqlParameter("@SettingsName", settingsName),
                new SqlParameter("@Xml", xml));
        }

        public static int AddRdsCertificate(int serviceId, string content, byte[] hash, string fileName, DateTime? validFrom, DateTime? expiryDate)
        {
            SqlParameter rdsCertificateId = new SqlParameter("@RDSCertificateID", SqlDbType.Int);
            rdsCertificateId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddRDSCertificate",
                rdsCertificateId,
                new SqlParameter("@ServiceId", serviceId),
                new SqlParameter("@Content", content),
                new SqlParameter("@Hash", Convert.ToBase64String(hash)),
                new SqlParameter("@FileName", fileName),
                new SqlParameter("@ValidFrom", validFrom),
                new SqlParameter("@ExpiryDate", expiryDate)
            );

            return Convert.ToInt32(rdsCertificateId.Value);
        }

        public static IDataReader GetRdsCertificateByServiceId(int serviceId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSCertificateByServiceId",
                new SqlParameter("@ServiceId", serviceId)
            );
        }

        public static IDataReader GetRdsCollectionSettingsByCollectionId(int collectionId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSCollectionSettingsByCollectionId",
                new SqlParameter("@RDSCollectionID", collectionId)
            );
        }

        public static int AddRdsCollectionSettings(RdsCollectionSettings settings)
        {
            return AddRdsCollectionSettings(settings.RdsCollectionId, settings.DisconnectedSessionLimitMin, settings.ActiveSessionLimitMin, settings.IdleSessionLimitMin, settings.BrokenConnectionAction,
                settings.AutomaticReconnectionEnabled, settings.TemporaryFoldersDeletedOnExit, settings.TemporaryFoldersPerSession, settings.ClientDeviceRedirectionOptions, settings.ClientPrinterRedirected,
                settings.ClientPrinterAsDefault, settings.RDEasyPrintDriverEnabled, settings.MaxRedirectedMonitors, settings.SecurityLayer, settings.EncryptionLevel, settings.AuthenticateUsingNLA);
        }

        private static int AddRdsCollectionSettings(int rdsCollectionId, int disconnectedSessionLimitMin, int activeSessionLimitMin, int idleSessionLimitMin, string brokenConnectionAction,
            bool automaticReconnectionEnabled, bool temporaryFoldersDeletedOnExit, bool temporaryFoldersPerSession, string clientDeviceRedirectionOptions, bool ClientPrinterRedirected,
            bool clientPrinterAsDefault, bool rdEasyPrintDriverEnabled, int maxRedirectedMonitors, string SecurityLayer, string EncryptionLevel, bool AuthenticateUsingNLA)
        {
            SqlParameter rdsCollectionSettingsId = new SqlParameter("@RDSCollectionSettingsID", SqlDbType.Int);
            rdsCollectionSettingsId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddRDSCollectionSettings",
                rdsCollectionSettingsId,
                new SqlParameter("@RdsCollectionId", rdsCollectionId),
                new SqlParameter("@DisconnectedSessionLimitMin", disconnectedSessionLimitMin),
                new SqlParameter("@ActiveSessionLimitMin", activeSessionLimitMin),
                new SqlParameter("@IdleSessionLimitMin", idleSessionLimitMin),
                new SqlParameter("@BrokenConnectionAction", brokenConnectionAction),
                new SqlParameter("@AutomaticReconnectionEnabled", automaticReconnectionEnabled),
                new SqlParameter("@TemporaryFoldersDeletedOnExit", temporaryFoldersDeletedOnExit),
                new SqlParameter("@TemporaryFoldersPerSession", temporaryFoldersPerSession),
                new SqlParameter("@ClientDeviceRedirectionOptions", clientDeviceRedirectionOptions),
                new SqlParameter("@ClientPrinterRedirected", ClientPrinterRedirected),
                new SqlParameter("@ClientPrinterAsDefault", clientPrinterAsDefault),
                new SqlParameter("@RDEasyPrintDriverEnabled", rdEasyPrintDriverEnabled),
                new SqlParameter("@MaxRedirectedMonitors", maxRedirectedMonitors),
                new SqlParameter("@SecurityLayer", SecurityLayer),
                new SqlParameter("@EncryptionLevel", EncryptionLevel),
                new SqlParameter("@AuthenticateUsingNLA", AuthenticateUsingNLA)
            );
            
            return Convert.ToInt32(rdsCollectionSettingsId.Value);
        }

        public static void UpdateRDSCollectionSettings(RdsCollectionSettings settings)
        {
            UpdateRDSCollectionSettings(settings.Id, settings.RdsCollectionId, settings.DisconnectedSessionLimitMin, settings.ActiveSessionLimitMin, settings.IdleSessionLimitMin, settings.BrokenConnectionAction,
                settings.AutomaticReconnectionEnabled, settings.TemporaryFoldersDeletedOnExit, settings.TemporaryFoldersPerSession, settings.ClientDeviceRedirectionOptions, settings.ClientPrinterRedirected,
                settings.ClientPrinterAsDefault, settings.RDEasyPrintDriverEnabled, settings.MaxRedirectedMonitors, settings.SecurityLayer, settings.EncryptionLevel, settings.AuthenticateUsingNLA);
        }

        public static void UpdateRDSCollectionSettings(int id, int rdsCollectionId, int disconnectedSessionLimitMin, int activeSessionLimitMin, int idleSessionLimitMin, string brokenConnectionAction,
            bool automaticReconnectionEnabled, bool temporaryFoldersDeletedOnExit, bool temporaryFoldersPerSession, string clientDeviceRedirectionOptions, bool ClientPrinterRedirected,
            bool clientPrinterAsDefault, bool rdEasyPrintDriverEnabled, int maxRedirectedMonitors, string SecurityLayer, string EncryptionLevel, bool AuthenticateUsingNLA)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateRDSCollectionSettings",
                new SqlParameter("@Id", id),
                new SqlParameter("@RdsCollectionId", rdsCollectionId),
                new SqlParameter("@DisconnectedSessionLimitMin", disconnectedSessionLimitMin),
                new SqlParameter("@ActiveSessionLimitMin", activeSessionLimitMin),
                new SqlParameter("@IdleSessionLimitMin", idleSessionLimitMin),
                new SqlParameter("@BrokenConnectionAction", brokenConnectionAction),
                new SqlParameter("@AutomaticReconnectionEnabled", automaticReconnectionEnabled),
                new SqlParameter("@TemporaryFoldersDeletedOnExit", temporaryFoldersDeletedOnExit),
                new SqlParameter("@TemporaryFoldersPerSession", temporaryFoldersPerSession),
                new SqlParameter("@ClientDeviceRedirectionOptions", clientDeviceRedirectionOptions),
                new SqlParameter("@ClientPrinterRedirected", ClientPrinterRedirected),
                new SqlParameter("@ClientPrinterAsDefault", clientPrinterAsDefault),
                new SqlParameter("@RDEasyPrintDriverEnabled", rdEasyPrintDriverEnabled),
                new SqlParameter("@MaxRedirectedMonitors", maxRedirectedMonitors),
                new SqlParameter("@SecurityLayer", SecurityLayer),
                new SqlParameter("@EncryptionLevel", EncryptionLevel),
                new SqlParameter("@AuthenticateUsingNLA", AuthenticateUsingNLA)
            );
        }

        public static void DeleteRDSCollectionSettings(int id)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteRDSCollectionSettings",
                new SqlParameter("@Id", id)
            );
        }

        public static IDataReader GetRDSCollectionsByItemId(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSCollectionsByItemId",
                new SqlParameter("@ItemID", itemId)
            );
        }

        public static IDataReader GetRDSCollectionByName(string name)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSCollectionByName",
                new SqlParameter("@Name", name)
            );
        }

        public static IDataReader GetRDSCollectionById(int id)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSCollectionById",
                new SqlParameter("@ID", id)
            );
        }

        public static DataSet GetRDSCollectionsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSCollectionsPaged",
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@itemId", itemId),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows)
            );
        }

        public static int AddRDSCollection(int itemId, string name, string description, string displayName)
        {
            SqlParameter rdsCollectionId = new SqlParameter("@RDSCollectionID", SqlDbType.Int);
            rdsCollectionId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddRDSCollection",
                rdsCollectionId,
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@Name", name),
                new SqlParameter("@Description", description),
                new SqlParameter("@DisplayName", displayName)
            );

            // read identity
            return Convert.ToInt32(rdsCollectionId.Value);
        }

        public static int GetOrganizationRdsUsersCount(int itemId)
        {
            SqlParameter count = new SqlParameter("@TotalNumber", SqlDbType.Int);
            count.Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetOrganizationRdsUsersCount",
                count,
                new SqlParameter("@ItemId", itemId));

            // read identity
            return Convert.ToInt32(count.Value);
        }

        public static int GetOrganizationRdsCollectionsCount(int itemId)
        {
            SqlParameter count = new SqlParameter("@TotalNumber", SqlDbType.Int);
            count.Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetOrganizationRdsCollectionsCount",
                count,
                new SqlParameter("@ItemId", itemId));

            // read identity
            return Convert.ToInt32(count.Value);
        }

        public static int GetOrganizationRdsServersCount(int itemId)
        {
            SqlParameter count = new SqlParameter("@TotalNumber", SqlDbType.Int);
            count.Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetOrganizationRdsServersCount",
                count,
                new SqlParameter("@ItemId", itemId));

            // read identity
            return Convert.ToInt32(count.Value);
        }

        public static void UpdateRDSCollection(RdsCollection collection)
        {
            UpdateRDSCollection(collection.Id, collection.ItemId, collection.Name, collection.Description, collection.DisplayName);
        }

        public static void UpdateRDSCollection(int id, int itemId, string name, string description, string displayName)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateRDSCollection",
                new SqlParameter("@Id", id),
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@Name", name),
                new SqlParameter("@Description", description),
                new SqlParameter("@DisplayName", displayName)
            );
        }

        public static void DeleteRDSServerSettings(int serverId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteRDSServerSettings",
                new SqlParameter("@ServerId", serverId)
            );
        }

        public static void DeleteRDSCollection(int id)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteRDSCollection",
                new SqlParameter("@Id", id)
            );
        }

        public static int AddRDSServer(string name, string fqdName, string description, string controller)
        {
            SqlParameter rdsServerId = new SqlParameter("@RDSServerID", SqlDbType.Int);
            rdsServerId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddRDSServer",
                rdsServerId,
                new SqlParameter("@FqdName", fqdName),
                new SqlParameter("@Name", name),
                new SqlParameter("@Description", description),
                new SqlParameter("@Controller", controller)
            );

            // read identity
            return Convert.ToInt32(rdsServerId.Value);
        }

        public static IDataReader GetRDSServersByItemId(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSServersByItemId",
                new SqlParameter("@ItemID", itemId)
            );
        }

        public static DataSet GetRDSServersPaged(int? itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string controller, bool ignoreItemId = false, bool ignoreRdsCollectionId = false)
        {
            return SqlHelper.ExecuteDataset(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSServersPaged",
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@RdsCollectionId", collectionId),
                new SqlParameter("@IgnoreItemId", ignoreItemId),
                new SqlParameter("@IgnoreRdsCollectionId", ignoreRdsCollectionId),
                new SqlParameter("@maximumRows", maximumRows),
                new SqlParameter("@Controller", controller)
            );
        }

        public static IDataReader GetRDSServerById(int id)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSServerById",
                new SqlParameter("@ID", id)
            );
        }

        public static IDataReader GetRDSServersByCollectionId(int collectionId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSServersByCollectionId",
                new SqlParameter("@RdsCollectionId", collectionId)
            );
        }

        public static void DeleteRDSServer(int id)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteRDSServer",
                new SqlParameter("@Id", id)
            );
        }

        public static void UpdateRDSServer(RdsServer server)
        {
            UpdateRDSServer(server.Id, server.ItemId, server.Name, server.FqdName, server.Description,
                server.RdsCollectionId, server.ConnectionEnabled);
        }

        public static void UpdateRDSServer(int id, int? itemId, string name, string fqdName, string description, int? rdsCollectionId, string connectionEnabled)
        {
            byte connEnabled = 1;
            if (!String.IsNullOrEmpty(connectionEnabled))
            {
                if (connectionEnabled.Equals("false") || connectionEnabled.Equals("no") || connectionEnabled.Equals("0")) connEnabled = 0;
            }
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateRDSServer",
                new SqlParameter("@Id", id),
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@Name", name),
                new SqlParameter("@FqdName", fqdName),
                new SqlParameter("@Description", description),
                new SqlParameter("@RDSCollectionId", rdsCollectionId),
                new SqlParameter("@ConnectionEnabled", connEnabled)
            );
        }

        public static void AddRDSServerToCollection(int serverId, int rdsCollectionId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddRDSServerToCollection",
                new SqlParameter("@Id", serverId),
                new SqlParameter("@RDSCollectionId", rdsCollectionId)
            );
        }

        public static void AddRDSServerToOrganization(int itemId, int serverId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddRDSServerToOrganization",
                new SqlParameter("@Id", serverId),
                new SqlParameter("@ItemID", itemId)
            );
        }

        public static void RemoveRDSServerFromOrganization(int serverId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "RemoveRDSServerFromOrganization",
                new SqlParameter("@Id", serverId)
            );
        }

        public static void RemoveRDSServerFromCollection(int serverId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "RemoveRDSServerFromCollection",
                new SqlParameter("@Id", serverId)
            );
        }

        public static IDataReader GetRDSCollectionUsersByRDSCollectionId(int id)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSCollectionUsersByRDSCollectionId",
                new SqlParameter("@id", id)
            );
        }

        public static void AddRDSUserToRDSCollection(int rdsCollectionId, int accountId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddUserToRDSCollection",
                new SqlParameter("@RDSCollectionId", rdsCollectionId),
                new SqlParameter("@AccountID", accountId)
            );
        }

        public static void RemoveRDSUserFromRDSCollection(int rdsCollectionId, int accountId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "RemoveRDSUserFromRDSCollection",
                new SqlParameter("@RDSCollectionId", rdsCollectionId),
                new SqlParameter("@AccountID", accountId)
            );
        }

        public static int GetRDSControllerServiceIDbyFQDN(string fqdnName)
        {
            SqlParameter prmController = new SqlParameter("@Controller", SqlDbType.Int);
            prmController.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetRDSControllerServiceIDbyFQDN",
                new SqlParameter("@RdsfqdnName", fqdnName),
                prmController);

            return Convert.ToInt32(prmController.Value);
        }

        #endregion

        #region MX|NX Services

        public static IDataReader GetAllPackages()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetAllPackages"
            );
        }

        public static IDataReader GetDomainDnsRecords(int domainId, DnsRecordType recordType)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetDomainDnsRecords",
                new SqlParameter("@DomainId", domainId),
                new SqlParameter("@RecordType", recordType)
            );
        }

        public static IDataReader GetDomainAllDnsRecords(int domainId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetDomainAllDnsRecords",
                new SqlParameter("@DomainId", domainId)
            );
        }

        public static void AddDomainDnsRecord(DnsRecordInfo domainDnsRecord)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddDomainDnsRecord",
                new SqlParameter("@DomainId", domainDnsRecord.DomainId),
                new SqlParameter("@RecordType", domainDnsRecord.RecordType),
                new SqlParameter("@DnsServer", domainDnsRecord.DnsServer),
                new SqlParameter("@Value", domainDnsRecord.Value),
                new SqlParameter("@Date", domainDnsRecord.Date)
            );
        }

        public static IDataReader GetScheduleTaskEmailTemplate(string taskId)
        {
            return SqlHelper.ExecuteReader(
                    ConnectionString,
                    CommandType.StoredProcedure,
                    "GetScheduleTaskEmailTemplate",
                    new SqlParameter("@taskId", taskId)
                );
        }

        public static void DeleteDomainDnsRecord(int id)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteDomainDnsRecord",
                new SqlParameter("@Id", id)
            );
        }

        public static void UpdateDomainCreationDate(int domainId, DateTime date)
        {
            UpdateDomainDate(domainId, "UpdateDomainCreationDate", date);
        }

        public static void UpdateDomainExpirationDate(int domainId, DateTime date)
        {
            UpdateDomainDate(domainId, "UpdateDomainExpirationDate", date);
        }

        public static void UpdateDomainLastUpdateDate(int domainId, DateTime date)
        {
            UpdateDomainDate(domainId, "UpdateDomainLastUpdateDate", date);
        }

        private static void UpdateDomainDate(int domainId, string stroredProcedure, DateTime date)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                stroredProcedure,
                new SqlParameter("@DomainId", domainId),
                new SqlParameter("@Date", date)
            );
        }

        public static void UpdateDomainDates(int domainId, DateTime? domainCreationDate, DateTime? domainExpirationDate, DateTime? domainLastUpdateDate)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateDomainDates",
                new SqlParameter("@DomainId", domainId),
                new SqlParameter("@DomainCreationDate", domainCreationDate),
                new SqlParameter("@DomainExpirationDate", domainExpirationDate),
                new SqlParameter("@DomainLastUpdateDate", domainLastUpdateDate)
            );
        }

        public static void UpdateWhoisDomainInfo(int domainId, DateTime? domainCreationDate, DateTime? domainExpirationDate, DateTime? domainLastUpdateDate, string registrarName)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "UpdateWhoisDomainInfo",
                new SqlParameter("@DomainId", domainId),
                new SqlParameter("@DomainCreationDate", domainCreationDate),
                new SqlParameter("@DomainExpirationDate", domainExpirationDate),
                new SqlParameter("@DomainLastUpdateDate", domainLastUpdateDate),
                new SqlParameter("@DomainRegistrarName", registrarName)
            );
        }

        #endregion

        #region Organization Storage Space Folders
        public static IDataReader GetOrganizationStoragSpaceFolders(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetOrganizationStoragSpaceFolders",
                new SqlParameter("@ItemId", itemId)
            );
        }

        public static IDataReader GetOrganizationStoragSpacesFolderByType(int itemId, string type)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetOrganizationStoragSpacesFolderByType",
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@Type", type)
            );
        }

        public static void DeleteOrganizationStoragSpacesFolder(int id)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                CommandType.StoredProcedure,
                "DeleteOrganizationStoragSpacesFolder",
                new SqlParameter("@ID", id));
        }

        public static int AddOrganizationStoragSpacesFolder(int itemId, string type, int storageSpaceFolderId)
        {
            SqlParameter outParam = new SqlParameter("@ID", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddOrganizationStoragSpacesFolder",
                outParam,
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@Type", type),
                new SqlParameter("@StorageSpaceFolderId", storageSpaceFolderId)
            );

            return Convert.ToInt32(outParam.Value);
        }

        public static IDataReader GetOrganizationStorageSpacesFolderById(int itemId, int folderId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetOrganizationStorageSpacesFolderById",
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@ID", folderId)
            );
        }
        #endregion

	    #region RDS Messages        

        public static DataSet GetRDSMessagesByCollectionId(int rdsCollectionId)
        {
            return SqlHelper.ExecuteDataset(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetRDSMessages",                
                new SqlParameter("@RDSCollectionId", rdsCollectionId)
            );
        }

        public static int AddRDSMessage(int rdsCollectionId, string messageText, string userName)
        {
            SqlParameter rdsMessageId = new SqlParameter("@RDSMessageID", SqlDbType.Int);
            rdsMessageId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "AddRDSMessage",
                rdsMessageId,
                new SqlParameter("@RDSCollectionId", rdsCollectionId),
                new SqlParameter("@MessageText", messageText),
                new SqlParameter("@UserName", userName),
                new SqlParameter("@Date", DateTime.Now)
            );
            
            return Convert.ToInt32(rdsMessageId.Value);
        }

        #endregion

    }
}
