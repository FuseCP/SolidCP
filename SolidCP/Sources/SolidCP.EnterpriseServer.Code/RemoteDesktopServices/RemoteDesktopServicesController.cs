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
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using SolidCP.Providers.Common;
using SolidCP.Providers.EnterpriseStorage;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.OS;
using SolidCP.Providers.RemoteDesktopServices;
using SolidCP.Providers.Web;
using System.Net.Mail;
using System.Collections;
using SolidCP.EnterpriseServer.Base.RDS;

namespace SolidCP.EnterpriseServer
{
    public class RemoteDesktopServicesController
    {
        private RemoteDesktopServicesController()
        {

        }

        public static int GetRemoteDesktopServiceId(int itemId)
        {
            return GetRdsServiceId(itemId);
        }

        public static RdsCollection GetRdsCollection(int collectionId)
        {
            return GetRdsCollectionInternal(collectionId);
        }

        public static RdsCollectionSettings GetRdsCollectionSettings(int collectionId)
        {
            return GetRdsCollectionSettingsInternal(collectionId);
        }

        public static List<RdsCollection> GetOrganizationRdsCollections(int itemId)
        {
            return GetOrganizationRdsCollectionsInternal(itemId);
        }

        public static int AddRdsCollection(int itemId, RdsCollection collection)
        {
            return AddRdsCollectionInternal(itemId, collection);
        }

        public static ResultObject EditRdsCollection(int itemId, RdsCollection collection)
        {
            return EditRdsCollectionInternal(itemId, collection);
        }

        public static ResultObject EditRdsCollectionSettings(int itemId, RdsCollection collection)
        {
            return EditRdsCollectionSettingsInternal(itemId, collection);
        }

        public static RdsCollectionPaged GetRdsCollectionsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetRdsCollectionsPagedInternal(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static ResultObject RemoveRdsCollection(int itemId, RdsCollection collection)
        {
            return RemoveRdsCollectionInternal(itemId, collection);
        }

        public static List<StartMenuApp> GetAvailableRemoteApplications(int itemId, string collectionName)
        {
            return GetAvailableRemoteApplicationsInternal(itemId, collectionName);
        }

        public static RdsServersPaged GetRdsServersPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetRdsServersPagedInternal(filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static List<RdsUserSession> GetRdsUserSessions(int collectionId)
        {
            return GetRdsUserSessionsInternal(collectionId);
        }

        public static RdsServersPaged GetFreeRdsServersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetFreeRdsServersPagedInternal(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static RdsServersPaged GetOrganizationRdsServersPaged(int itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetOrganizationRdsServersPagedInternal(itemId, collectionId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static RdsServersPaged GetOrganizationFreeRdsServersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return GetOrganizationFreeRdsServersPagedInternal(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static RdsServer GetRdsServer(int rdsSeverId)
        {
            return GetRdsServerInternal(rdsSeverId);
        }

        public static ResultObject SetRDServerNewConnectionAllowed(int itemId, string newConnectionAllowed, int rdsSeverId)
        {
            return SetRDServerNewConnectionAllowedInternal(itemId, newConnectionAllowed, rdsSeverId);
        }

        public static List<RdsServer> GetCollectionRdsServers(int collectionId)
        {            
            return GetCollectionRdsServersInternal(collectionId);
        }

        public static List<RdsServer> GetOrganizationRdsServers(int itemId)
        {
            return GetOrganizationRdsServersInternal(itemId);
        }

        public static ResultObject AddRdsServer(RdsServer rdsServer)
        {
            return AddRdsServerInternal(rdsServer);
        }

        public static ResultObject AddRdsServerToCollection(int itemId, RdsServer rdsServer, RdsCollection rdsCollection)
        {
            return AddRdsServerToCollectionInternal(itemId, rdsServer, rdsCollection);
        }

        public static ResultObject AddRdsServerToOrganization(int itemId, int serverId)
        {
            return AddRdsServerToOrganizationInternal(itemId, serverId);
        }

        public static ResultObject RemoveRdsServer(int rdsServerId)
        {
            return RemoveRdsServerInternal(rdsServerId);
        }

        public static ResultObject RemoveRdsServerFromCollection(int itemId, RdsServer rdsServer, RdsCollection rdsCollection)
        {
            return RemoveRdsServerFromCollectionInternal(itemId, rdsServer, rdsCollection);
        }

        public static ResultObject RemoveRdsServerFromOrganization(int itemId, int rdsServerId)
        {
            return RemoveRdsServerFromOrganizationInternal(itemId, rdsServerId);
        }

        public static ResultObject UpdateRdsServer(RdsServer rdsServer)
        {
            return UpdateRdsServerInternal(rdsServer);
        }

        public static List<OrganizationUser> GetRdsCollectionUsers(int collectionId)
        {
            return GetRdsCollectionUsersInternal(collectionId);
        }

        public static ResultObject SetUsersToRdsCollection(int itemId, int collectionId, List<OrganizationUser> users)
        {
            return SetUsersToRdsCollectionInternal(itemId, collectionId, users);
        }

        public static ResultObject AddRemoteApplicationToCollection(int itemId, RdsCollection collection, RemoteApplication application)
        {
            return AddRemoteApplicationToCollectionInternal(itemId, collection, application);
        }

        public static List<RemoteApplication> GetCollectionRemoteApplications(int itemId, string collectionName)
        {
            return GetCollectionRemoteApplicationsInternal(itemId, collectionName);
        }

        public static ResultObject RemoveRemoteApplicationFromCollection(int itemId, RdsCollection collection, RemoteApplication application)
        {
            return RemoveRemoteApplicationFromCollectionInternal(itemId, collection, application);
        }

        public static ResultObject SetRemoteApplicationsToRdsCollection(int itemId, int collectionId, List<RemoteApplication> remoteApps)
        {
            return SetRemoteApplicationsToRdsCollectionInternal(itemId, collectionId, remoteApps);
        }

        public static ResultObject DeleteRemoteDesktopService(int itemId)
        {
            return DeleteRemoteDesktopServiceInternal(itemId);
        }

        public static int GetOrganizationRdsUsersCount(int itemId)
        {
            return GetOrganizationRdsUsersCountInternal(itemId);
        }

        public static int GetOrganizationRdsServersCount(int itemId)
        {
            return GetOrganizationRdsServersCountInternal(itemId);
        }

        public static int GetOrganizationRdsCollectionsCount(int itemId)
        {
            return GetOrganizationRdsCollectionsCountInternal(itemId);
        }

        public static List<string> GetApplicationUsers(int itemId, int collectionId, RemoteApplication remoteApp)
        {
            return GetApplicationUsersInternal(itemId, collectionId, remoteApp);
        }

        public static ResultObject SetApplicationUsers(int itemId, int collectionId, RemoteApplication remoteApp, List<string> users)
        {
            return SetApplicationUsersInternal(itemId, collectionId, remoteApp, users);
        }

        public static ResultObject LogOffRdsUser(int itemId, string unifiedSessionId, string hostServer)
        {
            return LogOffRdsUserInternal(itemId, unifiedSessionId, hostServer);
        }

        public static List<string> GetRdsCollectionSessionHosts(int collectionId)
        {
            return GetRdsCollectionSessionHostsInternal(collectionId);
        }

        public static RdsServerInfo GetRdsServerInfo(int? itemId, string fqdnName)
        {
            return GetRdsServerInfoInternal(itemId, fqdnName);
        }

        public static string GetRdsServerStatus(int? itemId, string fqdnName)
        {
            return GetRdsServerStatusInternal(itemId, fqdnName);
        }

        public static ResultObject ShutDownRdsServer(int? itemId, string fqdnName)
        {
            return ShutDownRdsServerInternal(itemId, fqdnName);
        }

        public static ResultObject RestartRdsServer(int? itemId, string fqdnName)
        {
            return RestartRdsServerInternal(itemId, fqdnName);
        }

        public static List<OrganizationUser> GetRdsCollectionLocalAdmins(int collectionId)
        {
            return GetRdsCollectionLocalAdminsInternal(collectionId);
        }

        public static ResultObject SaveRdsCollectionLocalAdmins(OrganizationUser[] users, int collectionId)
        {
            return SaveRdsCollectionLocalAdminsInternal(users, collectionId);
        }

        public static ResultObject InstallSessionHostsCertificate(RdsServer rdsServer)
        {
            return InstallSessionHostsCertificateInternal(rdsServer);
        }

        public static RdsCertificate GetRdsCertificateByServiceId(int serviceId)
        {
            return GetRdsCertificateByServiceIdInternal(serviceId);
        }

        public static RdsCertificate GetRdsCertificateByItemId(int? itemId)
        {
            return GetRdsCertificateByItemIdInternal(itemId);
        }

        public static ResultObject AddRdsCertificate(RdsCertificate certificate)
        {
            return AddRdsCertificateInternal(certificate);
        }

        public static List<ServiceInfo> GetRdsServices()
        {
            return GetRdsServicesInternal();
        }        

        public static string GetRdsSetupLetter(int itemId, int? accountId)
        {
            return GetRdsSetupLetterInternal(itemId, accountId);
        }

        public static int SendRdsSetupLetter(int itemId, int? accountId, string to, string cc)
        {
            return SendRdsSetupLetterInternal(itemId, accountId, to, cc);
        }

        public static RdsServerSettings GetRdsServerSettings(int serverId, string settingsName)
        {
            return GetRdsServerSettingsInternal(serverId, settingsName);
        }              

        public static int UpdateRdsServerSettings(int serverId, string settingsName, RdsServerSettings settings)
        {
            return UpdateRdsServerSettingsInternal(serverId, settingsName, settings);
        }

        public static ResultObject ShadowSession(int itemId, string sessionId, bool control, string fqdName)
        {
            return ShadowSessionInternal(itemId, sessionId, control, fqdName);
        }

        public static ResultObject ImportCollection(int itemId, string collectionName)
        {
            return ImportCollectionInternal(itemId, collectionName);
        }

        public static ResultObject SendMessage(RdsMessageRecipient[] recipients, string text, int itemId, int rdsCollectionId, string userName)
        {
            return SendMessageInternal(recipients, text, itemId, rdsCollectionId, userName);
        }

        public static List<RdsMessage> GetRdsMessagesByCollectionId(int rdsCollectionId)
        {
            return GetRdsMessagesByCollectionIdInternal(rdsCollectionId);
        }

        private static ResultObject SendMessageInternal(RdsMessageRecipient[] recipients, string text, int itemId, int rdsCollectionId, string userName)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "SEND_MESSAGE");

            try
            {
                Organization org = OrganizationController.GetOrganization(itemId);

                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("SEND_MESSAGE", new NullReferenceException("Organization not found"));                    

                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
                rds.SendMessage(recipients, text);
                DataProvider.AddRDSMessage(rdsCollectionId, text, userName);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_SHADOW_RDS_SESSION", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }        

        private static List<RdsMessage> GetRdsMessagesByCollectionIdInternal(int rdsCollectionId)
        {
            return ObjectUtils.CreateListFromDataSet<RdsMessage>(DataProvider.GetRDSMessagesByCollectionId(rdsCollectionId));
        }

        private static ResultObject ImportCollectionInternal(int itemId, string collectionName)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "IMPORT_RDS_COLLECTION");

            try
            {
                Organization org = OrganizationController.GetOrganization(itemId);

                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("IMPORT_RDS_COLLECTION", new NullReferenceException("Organization not found"));

                    return result;
                }

                var existingCollections = GetRdsCollectionsPaged(itemId, "", "", "", 0, Int32.MaxValue).Collections;

                if (existingCollections.Select(e => e.Name.ToLower()).Contains(collectionName.ToLower()))
                {
                    result.IsSuccess = false;
                    throw new InvalidOperationException(string.Format("Collection {0} already exists in database", collectionName));
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
                var collection = rds.GetExistingCollection(collectionName);
                var newCollection = new RdsCollection
                {
                    Name = collection.CollectionName,
                    Description = collection.Description,
                    DisplayName = collection.CollectionName
                };

                newCollection.Id = DataProvider.AddRDSCollection(itemId, newCollection.Name, newCollection.Description, newCollection.DisplayName);
                newCollection.Settings = RemoteDesktopServicesHelpers.ParseCollectionSettings(collection.CollectionSettings);
                newCollection.Settings.RdsCollectionId = newCollection.Id;
                newCollection.Settings.Id = DataProvider.AddRdsCollectionSettings(newCollection.Settings);
                var existingSessionHosts = GetRdsServersPagedInternal("", "", "", 1, 1000).Servers;
                RemoteDesktopServicesHelpers.FillSessionHosts(collection.SessionHosts, existingSessionHosts, newCollection.Id, itemId);
                newCollection.Servers = ObjectUtils.CreateListFromDataReader<RdsServer>(DataProvider.GetRDSServersByCollectionId(newCollection.Id)).ToList();
                UserInfo user = PackageController.GetPackageOwner(org.PackageId);
                var organizationUsers = OrganizationController.GetOrganizationUsersPaged(itemId, null, null, null, 0, Int32.MaxValue).PageUsers.Select(u => u.SamAccountName.Split('\\').Last().ToLower());
                var newUsers = organizationUsers.Where(x => collection.UserGroups.Select(a => a.PropertyValue.ToString().Split('\\').Last().ToLower()).Contains(x));

                rds.ImportCollection(org.OrganizationId, newCollection, newUsers.ToArray());

                var emptySettings = RemoteDesktopServicesHelpers.GetEmptyGpoSettings();
                string xml = RemoteDesktopServicesHelpers.GetSettingsXml(emptySettings);
                DataProvider.UpdateRdsServerSettings(newCollection.Id, string.Format("Collection-{0}-Settings", newCollection.Id), xml);
            }            
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject ShadowSessionInternal(int itemId, string sessionId, bool control, string fqdName)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "SHADOW_RDS_SESSION");

            try
            {
                Organization org = OrganizationController.GetOrganization(itemId);

                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("SHADOW_RDS_SESSION", new NullReferenceException("Organization not found"));

                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
                rds.ShadowSession(sessionId, fqdName, control);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_SHADOW_RDS_SESSION", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static RdsServerSettings GetRdsServerSettingsInternal(int serverId, string settingsName)
        {
            IDataReader reader = DataProvider.GetRdsServerSettings(serverId, settingsName);

            var settings = new RdsServerSettings();
            settings.ServerId = serverId;
            settings.SettingsName = settingsName;

            while (reader.Read())
            {
                settings.Settings.Add(new RdsServerSetting
                {
                    PropertyName = (string)reader["PropertyName"],
                    PropertyValue = (string)reader["PropertyValue"],
                    ApplyAdministrators = Convert.ToBoolean(reader["ApplyAdministrators"]),
                    ApplyUsers = Convert.ToBoolean(reader["ApplyUsers"])
                });                
            }

            reader.Close();

            return settings;
        }  

        private static int UpdateRdsServerSettingsInternal(int serverId, string settingsName, RdsServerSettings settings)
        {
            TaskManager.StartTask("REMOTE_DESKTOP_SERVICES", "UPDATE_SETTINGS");

            try
            {                
                var collection = ObjectUtils.FillObjectFromDataReader<RdsCollection>(DataProvider.GetRDSCollectionById(serverId));
                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(GetRdsServiceId(collection.ItemId));
                Organization org = OrganizationController.GetOrganization(collection.ItemId);
                rds.ApplyGPO(org.OrganizationId, collection.Name, settings);
                string xml = RemoteDesktopServicesHelpers.GetSettingsXml(settings);

                DataProvider.UpdateRdsServerSettings(serverId, settingsName, xml);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        private static string GetRdsSetupLetterInternal(int itemId, int? accountId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);

            if (org == null)
            {
                return null;
            }
            
            UserInfo user = PackageController.GetPackageOwner(org.PackageId);
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.RDS_SETUP_LETTER);
            string settingName = user.HtmlMail ? "HtmlBody" : "TextBody";
            string body = settings[settingName];

            if (String.IsNullOrEmpty(body))
            {
                return null;
            }

            string result = RemoteDesktopServicesHelpers.EvaluateMailboxTemplate(body, org, accountId, itemId);

            return user.HtmlMail ? result : result.Replace("\n", "<br/>");
        }

        private static int SendRdsSetupLetterInternal(int itemId, int? accountId, string to, string cc)
        {            
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);

            if (accountCheck < 0)
            {
                return accountCheck;
            }
            
            Organization org = OrganizationController.GetOrganization(itemId);

            if (org == null)
            {
                return -1;
            }
                        
            UserInfo user = PackageController.GetPackageOwner(org.PackageId);            
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.RDS_SETUP_LETTER);
            string from = settings["From"];

            if (cc == null)
            {
                cc = settings["CC"];
            }

            string subject = settings["Subject"];
            string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
            bool isHtml = user.HtmlMail;
            MailPriority priority = MailPriority.Normal;

            if (!String.IsNullOrEmpty(settings["Priority"]))
            {
                priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);
            }

            if (String.IsNullOrEmpty(body))
            {
                return 0;
            }
            
            if (to == null)
            {
                to = user.Email;
            }

            subject = RemoteDesktopServicesHelpers.EvaluateMailboxTemplate(subject, org, accountId, itemId);
            body = RemoteDesktopServicesHelpers.EvaluateMailboxTemplate(body, org, accountId, itemId);
            
            return MailHelper.SendMessage(from, to, cc, subject, body, priority, isHtml);
        }

        private static ResultObject InstallSessionHostsCertificateInternal(RdsServer rdsServer)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "INSTALL_CERTIFICATE");

            try
            {                                
                int serviceId = GetRdsServiceId(rdsServer.ItemId);
                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(serviceId);
                var certificate = GetRdsCertificateByServiceIdInternal(serviceId);
                
                var array = Convert.FromBase64String(certificate.Hash);
                char[] chars = new char[array.Length / sizeof(char)];
                System.Buffer.BlockCopy(array, 0, chars, 0, array.Length);
                string password = new string(chars);
                byte[] content = Convert.FromBase64String(certificate.Content);

                rds.InstallCertificate(content, password, new string[] {rdsServer.FqdName});
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static RdsCertificate GetRdsCertificateByServiceIdInternal(int serviceId)
        {
            var result = ObjectUtils.FillObjectFromDataReader<RdsCertificate>(DataProvider.GetRdsCertificateByServiceId(serviceId));

            return result;
        }

        private static RdsCertificate GetRdsCertificateByItemIdInternal(int? itemId)
        {            
            int serviceId = GetRdsServiceId(itemId);
            var result = ObjectUtils.FillObjectFromDataReader<RdsCertificate>(DataProvider.GetRdsCertificateByServiceId(serviceId));

            return result;
        }

        private static ResultObject AddRdsCertificateInternal(RdsCertificate certificate)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "ADD_RDS_SERVER");

            try
            {
                byte[] hash = new byte[certificate.Hash.Length * sizeof(char)];
                System.Buffer.BlockCopy(certificate.Hash.ToCharArray(), 0, hash, 0, hash.Length);
                certificate.Id = DataProvider.AddRdsCertificate(certificate.ServiceId, certificate.Content, hash, certificate.FileName, certificate.ValidFrom, certificate.ExpiryDate);                
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    result.AddError("Unable to add RDS Certificate", ex.InnerException);
                }
                else
                {
                    result.AddError("Unable to add RDS Certificate", ex);
                }
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static RdsCollection GetRdsCollectionInternal(int collectionId)
        {
            var collection = ObjectUtils.FillObjectFromDataReader<RdsCollection>(DataProvider.GetRDSCollectionById(collectionId));
            var collectionSettings = ObjectUtils.FillObjectFromDataReader<RdsCollectionSettings>(DataProvider.GetRdsCollectionSettingsByCollectionId(collectionId));
            collection.Settings = collectionSettings;

            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "GET_RDS_COLLECTION");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(collection.ItemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

                rds.GetCollection(collection.Name);
                FillRdsCollection(collection);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_ADD_RDS_COLLECTION", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }            

            return collection;
        }

        private static List<OrganizationUser> GetRdsCollectionLocalAdminsInternal(int collectionId)
        {
            var result = new List<OrganizationUser>();
            var collection = ObjectUtils.FillObjectFromDataReader<RdsCollection>(DataProvider.GetRDSCollectionById(collectionId));
            var servers = ObjectUtils.CreateListFromDataReader<RdsServer>(DataProvider.GetRDSServersByCollectionId(collection.Id)).ToList();
            Organization org = OrganizationController.GetOrganization(collection.ItemId);
            
            if (org == null)
            {
                return result;
            }

            var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));            

            var organizationUsers = OrganizationController.GetOrganizationUsersPaged(collection.ItemId, null, null, null, 0, Int32.MaxValue).PageUsers;
            var organizationAdmins = rds.GetRdsCollectionLocalAdmins(org.OrganizationId, collection.Name);

            return organizationUsers.Where(o => organizationAdmins.Select(a => a.ToLower()).Contains(o.SamAccountName.ToLower())).ToList();
        }

        private static ResultObject SaveRdsCollectionLocalAdminsInternal(OrganizationUser[] users, int collectionId)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "SAVE_LOCAL_ADMINS");

            try
            {
                var collection = ObjectUtils.FillObjectFromDataReader<RdsCollection>(DataProvider.GetRDSCollectionById(collectionId));
                Organization org = OrganizationController.GetOrganization(collection.ItemId);

                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
                var servers = ObjectUtils.CreateListFromDataReader<RdsServer>(DataProvider.GetRDSServersByCollectionId(collection.Id)).ToList();                

                rds.SaveRdsCollectionLocalAdmins(users.Select(u => u.AccountName).ToArray(), servers.Select(s => s.FqdName).ToArray(), org.OrganizationId, collection.Name);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static RdsCollectionSettings GetRdsCollectionSettingsInternal(int collectionId)
        {
            var collection = ObjectUtils.FillObjectFromDataReader<RdsCollection>(DataProvider.GetRDSCollectionById(collectionId));            
            var settings = ObjectUtils.FillObjectFromDataReader<RdsCollectionSettings>(DataProvider.GetRdsCollectionSettingsByCollectionId(collectionId));

            if (settings != null)
            {
                if (settings.SecurityLayer == null)
                {
                    settings.SecurityLayer = SecurityLayerValues.Negotiate.ToString();
                }

                if (settings.EncryptionLevel == null)
                {
                    settings.EncryptionLevel = EncryptionLevel.ClientCompatible.ToString();
                }

                if (settings.AuthenticateUsingNLA == null)
                {
                    settings.AuthenticateUsingNLA = true;
                }
            }

            return settings;
        }

        private static List<RdsCollection> GetOrganizationRdsCollectionsInternal(int itemId)
        {
            var collections = ObjectUtils.CreateListFromDataReader<RdsCollection>(DataProvider.GetRDSCollectionsByItemId(itemId));

            foreach (var rdsCollection in collections)
            {
                FillRdsCollection(rdsCollection);
            }

            return collections;
        }

        private static int AddRdsCollectionInternal(int itemId, RdsCollection collection)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "ADD_RDS_COLLECTION");
            var domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;

            try
            {
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    return -1;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

                foreach(var server in collection.Servers)
                {                    
                    if (!server.FqdName.EndsWith(domainName, StringComparison.CurrentCultureIgnoreCase))
                    {                       
                        throw TaskManager.WriteError(new Exception("Fully Qualified Domain Name not valid."));
                    }

                    if (!rds.CheckRDSServerAvaliable(server.FqdName))
                    {
                        throw TaskManager.WriteError(new Exception(string.Format("Unable to connect to {0} server.", server.FqdName)));
                    }
                }

                collection.Name = RemoteDesktopServicesHelpers.GetFormattedCollectionName(collection.DisplayName, org.OrganizationId);
                collection.Settings = RemoteDesktopServicesHelpers.GetDefaultCollectionSettings();

                rds.CreateCollection(org.OrganizationId, collection);
                var defaultGpoSettings = RemoteDesktopServicesHelpers.GetDefaultGpoSettings();                
                rds.ApplyGPO(org.OrganizationId, collection.Name, defaultGpoSettings);                
                collection.Id = DataProvider.AddRDSCollection(itemId, collection.Name, collection.Description, collection.DisplayName);
                string xml = RemoteDesktopServicesHelpers.GetSettingsXml(defaultGpoSettings);
                DataProvider.UpdateRdsServerSettings(collection.Id, string.Format("Collection-{0}-Settings", collection.Id), xml);
                
                collection.Settings.RdsCollectionId = collection.Id;
                int settingsId = DataProvider.AddRdsCollectionSettings(collection.Settings);
                collection.Settings.Id = settingsId;                

                foreach (var server in collection.Servers)
                {
                    DataProvider.AddRDSServerToCollection(server.Id, collection.Id);
                }
            }
            catch (Exception ex)
            {                
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return collection.Id;
        }

        private static ResultObject EditRdsCollectionInternal(int itemId, RdsCollection collection)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "EDIT_RDS_COLLECTION");

            try
            {                
                Organization org = OrganizationController.GetOrganization(itemId);

                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
                var existingServers =
                    ObjectUtils.CreateListFromDataReader<RdsServer>(DataProvider.GetRDSServersByCollectionId(collection.Id)).ToList();
                var removedServers = existingServers.Where(x => !collection.Servers.Select(y => y.Id).Contains(x.Id));
                var newServers = collection.Servers.Where(x => !existingServers.Select(y => y.Id).Contains(x.Id));

                foreach(var server in removedServers)
                {
                    DataProvider.RemoveRDSServerFromCollection(server.Id);
                }

                rds.AddSessionHostServersToCollection(org.OrganizationId, collection.Name, newServers.ToArray());
                rds.MoveSessionHostsToCollectionOU(collection.Servers.ToArray(), collection.Name, org.OrganizationId);

                foreach (var server in newServers)
                {                    
                    DataProvider.AddRDSServerToCollection(server.Id, collection.Id);
                }
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_ADD_RDS_COLLECTION", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject EditRdsCollectionSettingsInternal(int itemId, RdsCollection collection)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "EDIT_RDS_COLLECTION_SETTINGS");

            try
            {
                Organization org = OrganizationController.GetOrganization(itemId);

                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
                rds.EditRdsCollectionSettings(collection);
                var collectionSettings = ObjectUtils.FillObjectFromDataReader<RdsCollectionSettings>(DataProvider.GetRdsCollectionSettingsByCollectionId(collection.Id));

                if (collectionSettings == null)
                {
                    DataProvider.AddRdsCollectionSettings(collection.Settings);
                }
                else
                {
                    DataProvider.UpdateRDSCollectionSettings(collection.Settings);
                }
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_ADD_RDS_COLLECTION", ex);
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static RdsCollectionPaged GetRdsCollectionsPagedInternal(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            DataSet ds = DataProvider.GetRDSCollectionsPaged(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);

            var result = new RdsCollectionPaged
            {
                RecordsCount = (int)ds.Tables[0].Rows[0][0]
            };

            List<RdsCollection> tmpCollections = new List<RdsCollection>();

            ObjectUtils.FillCollectionFromDataView(tmpCollections, ds.Tables[1].DefaultView);

            foreach (var collection in tmpCollections)
            {
                collection.Servers = GetCollectionRdsServersInternal(collection.Id);
            }

            result.Collections = tmpCollections.ToArray();

            return result;
        }

        private static ResultObject RemoveRdsCollectionInternal(int itemId, RdsCollection collection)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "REMOVE_RDS_COLLECTION");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
                var servers = ObjectUtils.CreateListFromDataReader<RdsServer>(DataProvider.GetRDSServersByCollectionId(collection.Id)).ToArray();
                rds.RemoveCollection(org.OrganizationId, collection.Name, servers);

                DataProvider.DeleteRDSServerSettings(collection.Id);
                DataProvider.DeleteRDSCollection(collection.Id);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_REMOVE_RDS_COLLECTION", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static List<StartMenuApp> GetAvailableRemoteApplicationsInternal(int itemId, string collectionName)
        {
            var result = new List<StartMenuApp>();

            var taskResult = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES",
                "GET_AVAILABLE_REMOTE_APPLICATIOBNS");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);

                if (org == null)
                {
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

                result.AddRange(rds.GetAvailableRemoteApplications(collectionName));
            }
            catch (Exception ex)
            {
                taskResult.AddError("REMOTE_DESKTOP_SERVICES_ADD_RDS_COLLECTION", ex);
            }
            finally
            {
                if (!taskResult.IsSuccess)
                {
                    TaskManager.CompleteResultTask(taskResult);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static RdsServersPaged GetRdsServersPagedInternal(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            DataSet ds = DataProvider.GetRDSServersPaged(null, null, filterColumn, filterValue, sortColumn, startRow, maximumRows, true, true);

            RdsServersPaged result = new RdsServersPaged();
            result.RecordsCount = (int)ds.Tables[0].Rows[0][0];

            List<RdsServer> tmpServers = new List<RdsServer>();

            ObjectUtils.FillCollectionFromDataView(tmpServers, ds.Tables[1].DefaultView);

            foreach (var tmpServer in tmpServers)
            {
                RemoteDesktopServicesHelpers.FillRdsServerData(tmpServer);                
            }

            result.Servers = tmpServers.ToArray();            

            return result;
        }

        private static List<RdsUserSession> GetRdsUserSessionsInternal(int collectionId)
        {
            var result = new List<RdsUserSession>();
            var collection = ObjectUtils.FillObjectFromDataReader<RdsCollection>(DataProvider.GetRDSCollectionById(collectionId));
            var organization = OrganizationController.GetOrganization(collection.ItemId);

            if (organization == null)
            {
                return result;
            }

            var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(organization.PackageId));
            var userSessions = rds.GetRdsUserSessions(collection.Name).ToList();
            var organizationUsers = OrganizationController.GetOrganizationUsersPaged(collection.ItemId, null, null, null, 0, Int32.MaxValue).PageUsers;            

            foreach(var userSession in userSessions)
            {
                var organizationUser = organizationUsers.FirstOrDefault(o => o.SamAccountName.Equals(userSession.SamAccountName, StringComparison.CurrentCultureIgnoreCase));

                if (organizationUser != null)
                {
                    userSession.IsVip = organizationUser.IsVIP;
                    result.Add(userSession);
                }
            }

            return result;
        }

        private static RdsServersPaged GetFreeRdsServersPagedInternal(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            RdsServersPaged result = new RdsServersPaged();
            Organization org = OrganizationController.GetOrganization(itemId);

            if (org == null)
            {
                return result;
            }

            var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
            var existingServers = rds.GetServersExistingInCollections();

            DataSet ds = DataProvider.GetRDSServersPaged(null, null, filterColumn, filterValue, sortColumn, startRow, maximumRows);            
            result.RecordsCount = (int)ds.Tables[0].Rows[0][0];

            List<RdsServer> tmpServers = new List<RdsServer>();

            ObjectUtils.FillCollectionFromDataView(tmpServers, ds.Tables[1].DefaultView);
            tmpServers = tmpServers.Where(x => !existingServers.Select(y => y.ToUpper()).Contains(x.FqdName.ToUpper())).ToList();
            result.Servers = tmpServers.ToArray();

            return result;
        }

        private static List<string> GetRdsCollectionSessionHostsInternal(int collectionId)
        {
            var result = new List<string>();
            var collection = ObjectUtils.FillObjectFromDataReader<RdsCollection>(DataProvider.GetRDSCollectionById(collectionId));
            Organization org = OrganizationController.GetOrganization(collection.ItemId);

            if (org == null)
            {
                return result;
            }

            var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
            result = rds.GetRdsCollectionSessionHosts(collection.Name).ToList();

            return result;
        }

        private static RdsServersPaged GetOrganizationRdsServersPagedInternal(int itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            DataSet ds = DataProvider.GetRDSServersPaged(itemId, collectionId, filterColumn, filterValue, sortColumn, startRow, maximumRows, ignoreRdsCollectionId: !collectionId.HasValue);

            RdsServersPaged result = new RdsServersPaged();
            result.RecordsCount = (int)ds.Tables[0].Rows[0][0];

            List<RdsServer> tmpServers = new List<RdsServer>();

            ObjectUtils.FillCollectionFromDataView(tmpServers, ds.Tables[1].DefaultView);

            result.Servers = tmpServers.ToArray();

            return result;
        }

        private static RdsServersPaged GetOrganizationFreeRdsServersPagedInternal(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            DataSet ds = DataProvider.GetRDSServersPaged(itemId, null, filterColumn, filterValue, sortColumn, startRow, maximumRows);

            RdsServersPaged result = new RdsServersPaged();
            result.RecordsCount = (int)ds.Tables[0].Rows[0][0];

            List<RdsServer> tmpServers = new List<RdsServer>();

            ObjectUtils.FillCollectionFromDataView(tmpServers, ds.Tables[1].DefaultView);

            result.Servers = tmpServers.ToArray();

            return result;
        }

        private static RdsServer GetRdsServerInternal(int rdsSeverId)
        {
            return ObjectUtils.FillObjectFromDataReader<RdsServer>(DataProvider.GetRDSServerById(rdsSeverId));
        }

        private static ResultObject SetRDServerNewConnectionAllowedInternal(int itemId, string newConnectionAllowed, int rdsSeverId)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "SET_RDS_SERVER_NEW_CONNECTIONS_ALLOWED"); ;
            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

                var rdsServer = GetRdsServer(rdsSeverId);                

                if (rdsServer == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("RDS Server not found"));
                    return result;
                }

                rds.SetRDServerNewConnectionAllowed(newConnectionAllowed, rdsServer);
                rdsServer.ConnectionEnabled = "newConnectionAllowed";
                DataProvider.UpdateRDSServer(rdsServer);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_SET_RDS_SERVER_NEW_CONNECTIONS_ALLOWED", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }        

        private static ResultObject AddRdsServerInternal(RdsServer rdsServer)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "ADD_RDS_SERVER");

            try
            {
                int serviceId = GetRdsMainServiceId();
                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(serviceId);

                if (rds.CheckRDSServerAvaliable(rdsServer.FqdName))
                {
                    var domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;

                    if (rdsServer.FqdName.EndsWith(domainName, StringComparison.CurrentCultureIgnoreCase))
                    {                        
                        rds.AddSessionHostFeatureToServer(rdsServer.FqdName);
                        rds.MoveSessionHostToRdsOU(rdsServer.Name);
                        rdsServer.Id = DataProvider.AddRDSServer(rdsServer.Name, rdsServer.FqdName, rdsServer.Description);
                    }
                    else
                    {
                        throw TaskManager.WriteError(new Exception("Fully Qualified Domain Name not valid."));
                    }
                }
                else
                {
                    throw TaskManager.WriteError(new Exception(string.Format("Unable to connect to {0} server. Please double check Server Full Name setting and retry.", rdsServer.FqdName)));
                }
            }            
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject AddRdsServerToCollectionInternal(int itemId, RdsServer rdsServer, RdsCollection rdsCollection)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "ADD_RDS_SERVER_TO_COLLECTION");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

                if (!rds.CheckSessionHostFeatureInstallation(rdsServer.FqdName))
                {
                    rds.AddSessionHostFeatureToServer(rdsServer.FqdName);
                }

                rds.AddSessionHostServerToCollection(org.OrganizationId, rdsCollection.Name, rdsServer);

                DataProvider.AddRDSServerToCollection(rdsServer.Id, rdsCollection.Id);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_ADD_RDS_SERVER_TO_COLLECTION", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject RemoveRdsServerFromCollectionInternal(int itemId, RdsServer rdsServer, RdsCollection rdsCollection)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "REMOVE_RDS_SERVER_FROM_COLLECTION");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

                rds.RemoveSessionHostServerFromCollection(org.OrganizationId, rdsCollection.Name, rdsServer);

                DataProvider.RemoveRDSServerFromCollection(rdsServer.Id);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_REMOVE_RDS_SERVER_FROM_COLLECTION", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject UpdateRdsServerInternal(RdsServer rdsServer)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "UPDATE_RDS_SERVER");

            try
            {
                DataProvider.UpdateRDSServer(rdsServer);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_UPDATE_RDS_SERVER", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject AddRdsServerToOrganizationInternal(int itemId, int serverId)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "ADD_RDS_SERVER_TO_ORGANIZATION");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

                RdsServer rdsServer = GetRdsServer(serverId);                
                rds.MoveRdsServerToTenantOU(rdsServer.FqdName, org.OrganizationId);
                DataProvider.AddRDSServerToOrganization(itemId, serverId);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject RemoveRdsServerFromOrganizationInternal(int itemId, int rdsServerId)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "REMOVE_RDS_SERVER_FROM_ORGANIZATION");            

            try
            {
                Organization org = OrganizationController.GetOrganization(itemId);

                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rdsServer = ObjectUtils.FillObjectFromDataReader<RdsServer>(DataProvider.GetRDSServerById(rdsServerId));
                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
                rds.RemoveRdsServerFromTenantOU(rdsServer.FqdName, org.OrganizationId);
                DataProvider.RemoveRDSServerFromOrganization(rdsServerId);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject RemoveRdsServerInternal(int rdsServerId)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "REMOVE_RDS_SERVER");

            try
            {
                DataProvider.DeleteRDSServer(rdsServerId);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_REMOVE_RDS_SERVER", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static List<OrganizationUser> GetRdsCollectionUsersInternal(int collectionId)
        {
            return ObjectUtils.CreateListFromDataReader<OrganizationUser>(DataProvider.GetRDSCollectionUsersByRDSCollectionId(collectionId));
        }        

        private static ResultObject SetUsersToRdsCollectionInternal(int itemId, int collectionId, List<OrganizationUser> users)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "ADD_USER_TO_RDS_COLLECTION");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

                var collection = GetRdsCollection(collectionId);

                if (collection == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Collection not found"));
                    return result;
                }

                foreach(var user in users)
                {
                    var account = OrganizationController.GetAccountByAccountName(itemId, user.AccountName);

                    user.AccountId = account.AccountId;
                }

                var usersInDb = GetRdsCollectionUsers(collectionId);

                var accountNames = users.Select(x => x.AccountName).ToList();

                //Set on server
                rds.SetUsersInCollection(org.OrganizationId, collection.Name, users.Select(x => x.AccountName).ToArray());

                //Remove from db
                foreach (var userInDb in usersInDb)
                {
                    if (!accountNames.Contains(userInDb.AccountName))
                    {
                        DataProvider.RemoveRDSUserFromRDSCollection(collectionId, userInDb.AccountId);
                    }
                }

                //Add to db
                foreach (var user in users)
                {
                    if (!usersInDb.Select(x => x.AccountName).Contains(user.AccountName))
                    {
                        DataProvider.AddRDSUserToRDSCollection(collectionId, user.AccountId);
                    }
                }

            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_ADD_USER_TO_RDS_COLLECTION", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static List<string> GetApplicationUsersInternal(int itemId, int collectionId, RemoteApplication remoteApp)
        {
            var result = new List<string>();
            Organization org = OrganizationController.GetOrganization(itemId);

            if (org == null)
            {
                return result;
            }

            var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
            var collection = ObjectUtils.FillObjectFromDataReader<RdsCollection>(DataProvider.GetRDSCollectionById(collectionId));
            string alias = "";

            if (remoteApp != null)
            {
                alias = remoteApp.Alias;
            }

            var users = rds.GetApplicationUsers(collection.Name, alias);
            result.AddRange(users);

            return result;
        }

        private static ResultObject SetApplicationUsersInternal(int itemId, int collectionId, RemoteApplication remoteApp, List<string> users)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "SET_REMOTE_APP_USERS");

            try
            {                
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var collection = ObjectUtils.FillObjectFromDataReader<RdsCollection>(DataProvider.GetRDSCollectionById(collectionId));
                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
                rds.SetApplicationUsers(collection.Name, remoteApp, users.ToArray());
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_SET_REMOTE_APP_USERS", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject LogOffRdsUserInternal(int itemId, string unifiedSessionId, string hostServer)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "LOG_OFF_RDS_USER");

            try
            {                
                Organization org = OrganizationController.GetOrganization(itemId);

                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("LOG_OFF_RDS_USER", new NullReferenceException("Organization not found"));

                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));
                rds.LogOffRdsUser(unifiedSessionId, hostServer);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_LOG_OFF_RDS_USER", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject AddRemoteApplicationToCollectionInternal(int itemId, RdsCollection collection, RemoteApplication remoteApp)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "ADD_REMOTE_APP_TO_COLLECTION");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

                if (!string.IsNullOrEmpty(remoteApp.Alias))
                {
                    remoteApp.Alias = remoteApp.DisplayName;
                }

                remoteApp.ShowInWebAccess = true;
                rds.AddRemoteApplication(collection.Name, remoteApp);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_ADD_REMOTE_APP_TO_COLLECTION", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }          

        private static ResultObject ShutDownRdsServerInternal(int? itemId, string fqdnName)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "SHUTDOWN_RDS_SERVER");

            try
            {
                int serviceId = GetRdsServiceId(itemId);

                if (serviceId != -1)
                {
                    var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(serviceId);
                    rds.ShutDownRdsServer(fqdnName);
                }
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_SHUTDOWN_RDS_SERVER", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject RestartRdsServerInternal(int? itemId, string fqdnName)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "RESTART_RDS_SERVER");            

            try
            {
                int serviceId = GetRdsServiceId(itemId);

                if (serviceId != -1)
                {
                    var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(serviceId);
                    rds.RestartRdsServer(fqdnName);
                }
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_RESTART_RDS_SERVER", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static List<RemoteApplication> GetCollectionRemoteApplicationsInternal(int itemId, string collectionName)
        {
            var result = new List<RemoteApplication>();

            // load organization
            Organization org = OrganizationController.GetOrganization(itemId);

            if (org == null)
            {
                return result;
            }

            var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

            result.AddRange(rds.GetCollectionRemoteApplications(collectionName));

            return result;
        }

        private static ResultObject RemoveRemoteApplicationFromCollectionInternal(int itemId, RdsCollection collection, RemoteApplication application)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "REMOVE_REMOTE_APP_FROM_COLLECTION");

            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

                rds.RemoveRemoteApplication(collection.Name, application);
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_REMOVE_REMOTE_APP_FROM_COLLECTION", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static ResultObject SetRemoteApplicationsToRdsCollectionInternal(int itemId, int collectionId, List<RemoteApplication> remoteApps)
        {
            ResultObject result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "SET_APPS_TO_RDS_COLLECTION"); ;
            try
            {
                // load organization
                Organization org = OrganizationController.GetOrganization(itemId);
                if (org == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Organization not found"));
                    return result;
                }

                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId));

                var collection = GetRdsCollection(collectionId);

                if (collection == null)
                {
                    result.IsSuccess = false;
                    result.AddError("", new NullReferenceException("Collection not found"));
                    return result;
                }

                List<RemoteApplication> existingCollectionApps = GetCollectionRemoteApplications(itemId, collection.Name);
                List<RemoteApplication> remoteAppsToAdd = remoteApps.Where(x => !existingCollectionApps.Select(p => p.Alias).Contains(x.Alias)).ToList();
                foreach (var app in remoteAppsToAdd)
                {
                    app.ShowInWebAccess = true;
                    AddRemoteApplicationToCollection(itemId, collection, app);
                }

                List<RemoteApplication> remoteAppsToRemove = existingCollectionApps.Where(x => !remoteApps.Select(p => p.Alias).Contains(x.Alias)).ToList();
                foreach (var app in remoteAppsToRemove)
                {
                    RemoveRemoteApplicationFromCollection(itemId, collection, app);
                }
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_SET_APPS_TO_RDS_COLLECTION", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }              

        private static ResultObject DeleteRemoteDesktopServiceInternal(int itemId)
        {
            var result = TaskManager.StartResultTask<ResultObject>("REMOTE_DESKTOP_SERVICES", "CLEANUP");

            try
            {
                var collections = GetOrganizationRdsCollections(itemId);

                foreach (var collection in collections)
                {
                    RemoveRdsCollection(itemId, collection);
                }

                var servers = GetOrganizationRdsServers(itemId);

                foreach (var server in servers)
                {
                    RemoveRdsServerFromOrganization(itemId, server.Id);
                }
            }
            catch (Exception ex)
            {
                result.AddError("REMOTE_DESKTOP_SERVICES_CLEANUP", ex);
            }
            finally
            {
                if (!result.IsSuccess)
                {
                    TaskManager.CompleteResultTask(result);
                }
                else
                {
                    TaskManager.CompleteResultTask();
                }
            }

            return result;
        }

        private static int GetOrganizationRdsUsersCountInternal(int itemId)
        {
            return DataProvider.GetOrganizationRdsUsersCount(itemId);
        }

        private static int GetOrganizationRdsServersCountInternal(int itemId)
        {
            return DataProvider.GetOrganizationRdsServersCount(itemId);
        }

        private static int GetOrganizationRdsCollectionsCountInternal(int itemId)
        {
            return DataProvider.GetOrganizationRdsCollectionsCount(itemId);
        }

        private static List<RdsServer> GetCollectionRdsServersInternal(int collectionId)
        {
            return ObjectUtils.CreateListFromDataReader<RdsServer>(DataProvider.GetRDSServersByCollectionId(collectionId)).ToList();
        }

        private static List<RdsServer> GetOrganizationRdsServersInternal(int itemId)
        {
            return ObjectUtils.CreateListFromDataReader<RdsServer>(DataProvider.GetRDSServersByItemId(itemId)).ToList();
        }

        private static List<ServiceInfo> GetRdsServicesInternal()
        {
            return ObjectUtils.CreateListFromDataSet<ServiceInfo>(DataProvider.GetServicesByGroupName(SecurityContext.User.UserId, ResourceGroups.RDS));
        }

        protected static int GetRdsMainServiceId()
        {
            var settings = SystemController.GetSystemSettings(SolidCP.EnterpriseServer.SystemSettings.RDS_SETTINGS);

            if (!string.IsNullOrEmpty(settings["RdsMainController"]))
            {
                return Convert.ToInt32(settings["RdsMainController"]);
            }

            var rdsServices = GetRdsServicesInternal();

            if (rdsServices.Any())
            {
                return rdsServices.First().ServiceId;
            }

            return -1;
        }

        protected static RdsCollection FillRdsCollection(RdsCollection collection)
        {
            collection.Servers = GetCollectionRdsServersInternal(collection.Id) ?? new List<RdsServer>();

            return collection;
        }

        protected static int GetRdsServiceId(int? itemId)
        {
            int serviceId = -1;

            if (itemId.HasValue)
            {
                Organization org = OrganizationController.GetOrganization(itemId.Value);

                if (org == null)
                {
                    return serviceId;
                }

                serviceId = RemoteDesktopServicesHelpers.GetRemoteDesktopServiceID(org.PackageId);
            }
            else
            {
                serviceId = GetRdsMainServiceId();
            }

            return serviceId;
        }

        private static RdsServerInfo GetRdsServerInfoInternal(int? itemId, string fqdnName)
        {
            int serviceId = GetRdsServiceId(itemId);
            var result = new RdsServerInfo();

            if (serviceId != -1)
            {
                var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(serviceId);
                result = rds.GetRdsServerInfo(fqdnName);
            }

            return result;
        }

        private static string GetRdsServerStatusInternal(int? itemId, string fqdnName)
        {            
            var result = "Unavailable";
            var serviceId = GetRdsServiceId(itemId);

            try
            {
                if (serviceId != -1)
                {
                    var rds = RemoteDesktopServicesHelpers.GetRemoteDesktopServices(serviceId);
                    result = rds.GetRdsServerStatus(fqdnName);
                }
            }
            catch
            {
            }

            return result;
        } 
    }
}
