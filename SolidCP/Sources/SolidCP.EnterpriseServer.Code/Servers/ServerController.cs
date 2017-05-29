// Copyright (c) 2016, SolidCPbinary
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

using SolidCP.EnterpriseServer.Extensions;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Providers.DNS;
using SolidCP.Providers.DomainLookup;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Whois.NET;
using OS = SolidCP.Providers.OS;


namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for ServersController.
    /// </summary>
    public class ServerController
    {
        private const string LOG_SOURCE_SERVERS = "SERVERS";

        private static List<string> _createdDatePatterns = new List<string> { @"Creation Date:(.+)", // base
                                                                                @"created:(.+)",
                                                                                @"Created On:(.+) UTC",
                                                                                @"Created On:(.+)",
                                                                                @"Domain Registration Date:(.+)",
                                                                                @"Domain Create Date:(.+)",
                                                                                @"Registered on:(.+)"};

        private static List<string> _expiredDatePatterns = new List<string> {   @"Expiration Date:(.+) UTC", //base UTC
                                                                                @"Expiration Date:(.+)", // base
                                                                                @"Registry Expiry Date:(.+)", //.org
                                                                                @"paid-till:(.+)", //.ru
                                                                                @"Expires On:(.+)", //.name
                                                                                @"Domain Expiration Date:(.+)", //.us
                                                                                @"renewal date:(.+)", //.pl
                                                                                @"Expiry date:(.+)", //.uk
                                                                                @"anniversary:(.+)", //.fr
                                                                                @"expires:(.+)" //.fi 
                                                                              };

        private static List<string> _registrarNamePatterns = new List<string>   {
                                                                                @"Created by Registrar:(.+)",
                                                                                @"Registrar:(.+)",
                                                                                @"Registrant Name:(.+)"
                                                                            };

        private static List<string> _datePatterns = new List<string> {   @"ddd MMM dd HH:mm:ss G\MT yyyy"
                                                                              };

        #region Servers
        public static List<ServerInfo> GetAllServers()
        {
            // fill collection
            return ObjectUtils.CreateListFromDataSet<ServerInfo>(
                DataProvider.GetAllServers(SecurityContext.User.UserId));
        }

        public static DataSet GetRawAllServers()
        {
            return DataProvider.GetAllServers(SecurityContext.User.UserId);
        }

        public static List<ServerInfo> GetServers()
        {
            // create servers list
            List<ServerInfo> servers = new List<ServerInfo>();

            // fill collection
            ObjectUtils.FillCollectionFromDataSet<ServerInfo>(
                servers, DataProvider.GetServers(SecurityContext.User.UserId));

            return servers;
        }

        public static DataSet GetRawServers()
        {
            return DataProvider.GetServers(SecurityContext.User.UserId);
        }

        internal static ServerInfo GetServerByIdInternal(int serverId)
        {
            ServerInfo server = ObjectUtils.FillObjectFromDataReader<ServerInfo>(
                DataProvider.GetServerInternal(serverId));

            if (server == null)
                return null;

            // decrypt passwords
            server.Password = CryptoUtils.Decrypt(server.Password);
            server.ADPassword = CryptoUtils.Decrypt(server.ADPassword);

            return server;
        }

        public static ServerInfo GetServerShortDetails(int serverId)
        {
            return ObjectUtils.FillObjectFromDataReader<ServerInfo>(
                DataProvider.GetServerShortDetails(serverId));
        }

        public static ServerInfo GetServerById(int serverId)
        {
            return ObjectUtils.FillObjectFromDataReader<ServerInfo>(
                DataProvider.GetServer(SecurityContext.User.UserId, serverId));
        }

        public static ServerInfo GetServerByName(string serverName)
        {
            return ObjectUtils.FillObjectFromDataReader<ServerInfo>(
                DataProvider.GetServerByName(SecurityContext.User.UserId, serverName));
        }

        public static int CheckServerAvailable(string serverUrl, string password)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsAdmin);
            if (accountCheck < 0) return accountCheck;

            TaskManager.StartTask("SERVER", "CHECK_AVAILABILITY", serverUrl);

            try
            {
                // TO-DO: Check connectivity
                return 0;
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    return BusinessErrorCodes.ERROR_ADD_SERVER_NOT_FOUND;
                else if (response != null && response.StatusCode == HttpStatusCode.BadRequest)
                    return BusinessErrorCodes.ERROR_ADD_SERVER_BAD_REQUEST;
                else if (response != null && response.StatusCode == HttpStatusCode.InternalServerError)
                    return BusinessErrorCodes.ERROR_ADD_SERVER_INTERNAL_SERVER_ERROR;
                else if (response != null && response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    return BusinessErrorCodes.ERROR_ADD_SERVER_SERVICE_UNAVAILABLE;
                else if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                    return BusinessErrorCodes.ERROR_ADD_SERVER_UNAUTHORIZED;
                if (ex.Message.Contains("The remote name could not be resolved") || ex.Message.Contains("Unable to connect"))
                {
                    TaskManager.WriteError("The remote server could not ne resolved");
                    return BusinessErrorCodes.ERROR_ADD_SERVER_URL_UNAVAILABLE;
                }
                return BusinessErrorCodes.ERROR_ADD_SERVER_APPLICATION_ERROR;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The signature or decryption was invalid"))
                {
                    TaskManager.WriteWarning("Wrong server access credentials");
                    return BusinessErrorCodes.ERROR_ADD_SERVER_WRONG_PASSWORD;
                }
                else
                {
                    TaskManager.WriteError("General Server Error");
                    TaskManager.WriteError(ex);
                    return BusinessErrorCodes.ERROR_ADD_SERVER_APPLICATION_ERROR;
                }
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        private static void FindServices(ServerInfo server)
        {
            try
            {
                List<ProviderInfo> providers;
                try
                {
                    providers = GetProviders();
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex);
                    throw new ApplicationException("Could not get providers list.");
                }

                foreach (ProviderInfo provider in providers)
                {
                    if (!provider.DisableAutoDiscovery)
                    {
                        BoolResult isInstalled = IsInstalled(server.ServerId, provider.ProviderId);
                        if (isInstalled.IsSuccess)
                        {
                            if (isInstalled.Value)
                            {
                                try
                                {
                                    ServiceInfo service = new ServiceInfo();
                                    service.ServerId = server.ServerId;
                                    service.ProviderId = provider.ProviderId;
                                    service.ServiceName = provider.DisplayName;
                                    AddService(service);
                                }
                                catch (Exception ex)
                                {
                                    TaskManager.WriteError(ex);
                                }
                            }
                        }
                        else
                        {
                            string errors = string.Join("\n", isInstalled.ErrorCodes.ToArray());
                            string str =
                                string.Format(
                                    "Could not check if specific software intalled for {0}. Following errors have been occured:\n{1}",
                                    provider.ProviderName, errors);

                            TaskManager.WriteError(str);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not find services. General error was occued.", ex);
            }
        }

        public static int AddServer(ServerInfo server, bool autoDiscovery)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsAdmin);
            if (accountCheck < 0) return accountCheck;

            // init passwords
            if (server.Password == null)
                server.Password = "";
            if (server.ADPassword == null)
                server.ADPassword = "";

            // check server availability
            if (!server.VirtualServer)
            {
                int availResult = CheckServerAvailable(server.ServerUrl, server.Password);
                if (availResult < 0)
                    return availResult;
            }

            TaskManager.StartTask("SERVER", "ADD", server.ServerName);

            int serverId = DataProvider.AddServer(server.ServerName, server.ServerUrl,
                CryptoUtils.Encrypt(server.Password), server.Comments, server.VirtualServer, server.InstantDomainAlias,
                server.PrimaryGroupId, server.ADEnabled, server.ADRootDomain, server.ADUsername, CryptoUtils.Encrypt(server.ADPassword),
                server.ADAuthenticationType);

            if (autoDiscovery)
            {
                server.ServerId = serverId;
                try
                {
                    FindServices(server);
                }
                catch (Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }

            TaskManager.ItemId = serverId;

            TaskManager.CompleteTask();

            return serverId;
        }

        public static int UpdateServer(ServerInfo server)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsAdmin);
            if (accountCheck < 0) return accountCheck;

            // get original server
            ServerInfo origServer = GetServerByIdInternal(server.ServerId);

            TaskManager.StartTask("SERVER", "UPDATE", origServer.ServerName, server.ServerId);

            // preserve passwords
            server.Password = origServer.Password;
            server.ADPassword = origServer.ADPassword;

            // check server availability
            if (!origServer.VirtualServer)
            {
                int availResult = CheckServerAvailable(server.ServerUrl, server.Password);
                if (availResult < 0)
                    return availResult;
            }

            DataProvider.UpdateServer(server.ServerId, server.ServerName, server.ServerUrl,
                CryptoUtils.Encrypt(server.Password), server.Comments, server.InstantDomainAlias,
                server.PrimaryGroupId, server.ADEnabled, server.ADRootDomain, server.ADUsername, CryptoUtils.Encrypt(server.ADPassword),
                server.ADAuthenticationType);

            TaskManager.CompleteTask();

            return 0;
        }

        public static int UpdateServerConnectionPassword(int serverId, string password)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsAdmin);
            if (accountCheck < 0) return accountCheck;

            // get original server
            ServerInfo server = GetServerByIdInternal(serverId);

            TaskManager.StartTask("SERVER", "UPDATE_PASSWORD", server.ServerName, serverId);

            // set password
            server.Password = password;

            // update server
            DataProvider.UpdateServer(server.ServerId, server.ServerName, server.ServerUrl,
                CryptoUtils.Encrypt(server.Password), server.Comments, server.InstantDomainAlias,
                server.PrimaryGroupId, server.ADEnabled, server.ADRootDomain, server.ADUsername, CryptoUtils.Encrypt(server.ADPassword),
                server.ADAuthenticationType);

            TaskManager.CompleteTask();

            return 0;
        }

        public static int UpdateServerADPassword(int serverId, string adPassword)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsAdmin);
            if (accountCheck < 0) return accountCheck;

            // get original server
            ServerInfo server = GetServerByIdInternal(serverId);

            TaskManager.StartTask("SERVER", "UPDATE_AD_PASSWORD", server.ServerName, serverId);

            // set password
            server.ADPassword = adPassword;

            // update server
            DataProvider.UpdateServer(server.ServerId, server.ServerName, server.ServerUrl,
                CryptoUtils.Encrypt(server.Password), server.Comments, server.InstantDomainAlias,
                server.PrimaryGroupId, server.ADEnabled, server.ADRootDomain, server.ADUsername, CryptoUtils.Encrypt(server.ADPassword),
                server.ADAuthenticationType);

            TaskManager.CompleteTask();

            return 0;
        }

        public static int DeleteServer(int serverId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive
                | DemandAccount.IsAdmin);
            if (accountCheck < 0) return accountCheck;

            // get original server
            ServerInfo server = GetServerByIdInternal(serverId);

            TaskManager.StartTask("SERVER", "DELETE", server.ServerName, serverId);

            try
            {
                int result = DataProvider.DeleteServer(serverId);
                if (result == -1)
                {
                    TaskManager.WriteError("Server contains services");
                    return BusinessErrorCodes.ERROR_SERVER_CONTAINS_SERVICES;
                }
                else if (result == -2)
                {
                    TaskManager.WriteError("Server contains spaces");
                    return BusinessErrorCodes.ERROR_SERVER_CONTAINS_PACKAGES;
                }
                else if (result == -3)
                {
                    TaskManager.WriteError("Server is used as a target in several hosting plans");
                    return BusinessErrorCodes.ERROR_SERVER_USED_IN_HOSTING_PLANS;
                }

                return 0;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static Dictionary<int, string> AutoUpdateServer(int serverId, int serviceId, byte[] zipFile, string zipFileName)
        {
            OS.OperatingSystem os = new OS.OperatingSystem();
            ServiceProviderProxy.Init(os, serviceId);
            Dictionary<int, string> res = new Dictionary<int, string>();
            string downloadPath = @"C:\SolidCPDownloads\".Replace(@"\\", @"\");
            string unpackedZipDirectory = downloadPath + zipFileName.Replace(".zip", "");
            string ipAddress = os.Url.Split('/')[2].Split(':')[0];

            string targetPath = @"\\" + ipAddress + @"\" + ServerController.GetServerFilePath(serverId).Replace(":", "$").Replace(@"\\", @"\");

            if (Directory.Exists(targetPath))
            {
                if (!Directory.Exists(downloadPath))
                {
                    Directory.CreateDirectory(downloadPath);
                }
                FileStream stream = File.Create(downloadPath + zipFileName);
                stream.Close();
                using (System.IO.MemoryStream zipFileContent = new System.IO.MemoryStream(zipFile))
                {
                    int chunkSize = 4096;
                    using (System.IO.BinaryReader reader = new System.IO.BinaryReader(zipFileContent))
                    {
                        byte[] chunk = new byte[chunkSize];
                        int c;
                        while ((c = reader.Read(chunk, 0, chunk.Length)) != 0)
                        {
                            FileStream s = new FileStream(downloadPath + zipFileName, FileMode.Append, FileAccess.Write);
                            s.Write(chunk, 0, chunk.Length);
                            s.Close();
                        }
                    }
                }

                FileUtils.UnzipFiles(downloadPath + zipFileName, unpackedZipDirectory);
                FileUtils.CopyDirectoryContentUNC(unpackedZipDirectory, targetPath);
                res.Add(0, "");
            }
            else {
                res.Add(serverId, "Directory " + targetPath + " not found");
            }
            return res;
        }

        #endregion

        #region Virtual Servers
        public static DataSet GetVirtualServers()
        {
            return DataProvider.GetVirtualServers(SecurityContext.User.UserId);
        }

        public static DataSet GetAvailableVirtualServices(int serverId)
        {
            return DataProvider.GetAvailableVirtualServices(SecurityContext.User.UserId, serverId);
        }

        public static DataSet GetVirtualServices(int serverId)
        {
            return DataProvider.GetVirtualServices(SecurityContext.User.UserId, serverId);
        }

        public static int AddVirtualServices(int serverId, int[] ids)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            ServerInfo server = GetServerByIdInternal(serverId);

            TaskManager.StartTask("VIRTUAL_SERVER", "ADD_SERVICES", server.ServerName, serverId);

            // build XML
            string xml = BuildXmlFromArray(ids, "services", "service");

            // update server
            DataProvider.AddVirtualServices(serverId, xml);

            TaskManager.CompleteTask();

            return 0;
        }

        public static int DeleteVirtualServices(int serverId, int[] ids)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            ServerInfo server = GetServerByIdInternal(serverId);

            TaskManager.StartTask("VIRTUAL_SERVER", "DELETE_SERVICES", server.ServerName, serverId);

            // build XML
            string xml = BuildXmlFromArray(ids, "services", "service");

            // update server
            DataProvider.DeleteVirtualServices(serverId, xml);

            TaskManager.CompleteTask();

            return 0;
        }

        public static int UpdateVirtualGroups(int serverId, VirtualGroupInfo[] groups)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            /*
            XML Format:

            <groups>
	            <group id="16" distributionType="1" bindDistributionToPrimary="1"/>
            </groups>

            */

            // build XML
            XmlDocument doc = new XmlDocument();
            XmlElement nodeGroups = doc.CreateElement("groups");
            // groups
            if (groups != null)
            {
                foreach (VirtualGroupInfo group in groups)
                {
                    XmlElement nodeGroup = doc.CreateElement("group");
                    nodeGroups.AppendChild(nodeGroup);
                    nodeGroup.SetAttribute("id", group.GroupId.ToString());
                    nodeGroup.SetAttribute("distributionType", group.DistributionType.ToString());
                    nodeGroup.SetAttribute("bindDistributionToPrimary", group.BindDistributionToPrimary ? "1" : "0");
                }
            }

            string xml = nodeGroups.OuterXml;

            // update server
            DataProvider.UpdateVirtualGroups(serverId, xml);

            return 0;
        }

        private static string BuildXmlFromArray(int[] ids, string rootName, string childName)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement nodeRoot = doc.CreateElement(rootName);
            foreach (int id in ids)
            {
                XmlElement nodeChild = doc.CreateElement(childName);
                nodeChild.SetAttribute("id", id.ToString());
                nodeRoot.AppendChild(nodeChild);
            }

            return nodeRoot.OuterXml;
        }
        #endregion

        #region Services
        public static DataSet GetRawServicesByServerId(int serverId)
        {
            return DataProvider.GetRawServicesByServerId(SecurityContext.User.UserId, serverId);
        }
        public static String GetMailFilterURL(int PackageId, String ResorceGroupName)
        {
            return DataProvider.GetMailFilterURL(SecurityContext.User.UserId, PackageId, ResorceGroupName);
        }
        public static String GetMailFilterURLByHostingPlan(int PlanID, String ResorceGroupName)
        {
            return DataProvider.GetMailFilterUrlByHostingPlan(SecurityContext.User.UserId, PlanID, ResorceGroupName);
        }
        public static List<ServiceInfo> GetServicesByServerId(int serverId)
        {
            List<ServiceInfo> services = new List<ServiceInfo>();
            ObjectUtils.FillCollectionFromDataReader<ServiceInfo>(services,
                DataProvider.GetServicesByServerId(SecurityContext.User.UserId, serverId));
            return services;
        }

        public static List<ServiceInfo> GetServicesByServerIdGroupName(int serverId, string groupName)
        {
            List<ServiceInfo> services = new List<ServiceInfo>();
            ObjectUtils.FillCollectionFromDataReader<ServiceInfo>(services,
                DataProvider.GetServicesByServerIdGroupName(SecurityContext.User.UserId,
                serverId, groupName));
            return services;
        }

        public static DataSet GetRawServicesByGroupId(int groupId)
        {
            return DataProvider.GetServicesByGroupId(SecurityContext.User.UserId, groupId);
        }

        public static DataSet GetRawServicesByGroupName(string groupName)
        {
            return DataProvider.GetServicesByGroupName(SecurityContext.User.UserId, groupName);
        }

        public static List<ServiceInfo> GetServicesByGroupName(string groupName)
        {
            return ObjectUtils.CreateListFromDataSet<ServiceInfo>(
                DataProvider.GetServicesByGroupName(SecurityContext.User.UserId, groupName));
        }

        public static ServiceInfo GetServiceInfoAdmin(int serviceId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0)
                return null;

            return ObjectUtils.FillObjectFromDataReader<ServiceInfo>(
                DataProvider.GetService(SecurityContext.User.UserId, serviceId));
        }

        public static ServiceInfo GetServiceInfo(int serviceId)
        {
            return ObjectUtils.FillObjectFromDataReader<ServiceInfo>(
                DataProvider.GetService(SecurityContext.User.UserId, serviceId));
        }

        public static int AddService(ServiceInfo service)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            TaskManager.StartTask("SERVER", "ADD_SERVICE", GetServerByIdInternal(service.ServerId).ServerName, service.ServerId);

            TaskManager.WriteParameter("Service name", service.ServiceName);
            TaskManager.WriteParameter("Provider", service.ProviderId);

            int serviceId = DataProvider.AddService(service.ServerId, service.ProviderId, service.ServiceName,
                service.ServiceQuotaValue, service.ClusterId, service.Comments);

            // read service default settings
            try
            {
                // load original settings
                StringDictionary origSettings = GetServiceSettingsAdmin(serviceId);

                // load provider settings
                ServiceProvider svc = new ServiceProvider();
                ServiceProviderProxy.Init(svc, serviceId);

                SettingPair[] settings = svc.GetProviderDefaultSettings();

                if (settings != null && settings.Length > 0)
                {
                    // merge settings
                    foreach (SettingPair pair in settings)
                        origSettings[pair.Name] = pair.Value;

                    // update settings in the meta base
                    string[] bareSettings = new string[origSettings.Count];
                    int i = 0;
                    foreach (string key in origSettings.Keys)
                        bareSettings[i++] = key + "=" + origSettings[key];

                    UpdateServiceSettings(serviceId, bareSettings);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex, "Error reading default provider settings");
            }

            TaskManager.CompleteTask();

            return serviceId;
        }

        public static int UpdateService(ServiceInfo service)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original service
            ServiceInfo origService = GetServiceInfo(service.ServiceId);

            TaskManager.StartTask("SERVER", "UPDATE_SERVICE", GetServerByIdInternal(origService.ServerId).ServerName, origService.ServerId);

            TaskManager.WriteParameter("New service name", service.ServiceName);

            DataProvider.UpdateService(service.ServiceId, service.ServiceName,
                service.ServiceQuotaValue, service.ClusterId, service.Comments);

            TaskManager.CompleteTask();

            return 0;
        }

        public static int DeleteService(int serviceId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            ServiceInfo service = GetServiceInfoAdmin(serviceId);

            TaskManager.StartTask("SERVER", "DELETE_SERVICE", GetServerByIdInternal(service.ServerId).ServerName, service.ServerId);

            TaskManager.WriteParameter("Service name", service.ServiceName);

            try
            {
                int result = DataProvider.DeleteService(serviceId);
                if (result == -1)
                {
                    TaskManager.WriteError("Service contains service items");
                    return BusinessErrorCodes.ERROR_SERVICE_CONTAINS_SERVICE_ITEMS;
                }
                else if (result == -2)
                {
                    TaskManager.WriteError("Service is assigned to virtual server");
                    return BusinessErrorCodes.ERROR_SERVICE_USED_IN_VIRTUAL_SERVER;
                }

                return 0;

            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static StringDictionary GetServiceSettingsRDS(int serviceId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsActive);
            if (accountCheck < 0)
                return null;

            bool isDemoAccount = (SecurityContext.CheckAccount(DemandAccount.NotDemo) < 0);

            return GetServiceSettings(serviceId, !isDemoAccount);
        }

        public static StringDictionary GetServiceSettingsAdmin(int serviceId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin | DemandAccount.IsActive);
            if (accountCheck < 0)
                return null;

            bool isDemoAccount = (SecurityContext.CheckAccount(DemandAccount.NotDemo) < 0);

            return GetServiceSettings(serviceId, !isDemoAccount);
        }

        internal static StringDictionary GetServiceSettings(int serviceId)
        {
            return GetServiceSettings(serviceId, true);
        }

        internal static StringDictionary GetServiceSettings(int serviceId, bool decryptPassword)
        {
            // get service settings
            IDataReader reader = DataProvider.GetServiceProperties(SecurityContext.User.UserId, serviceId);

            // create settings object
            StringDictionary settings = new StringDictionary();
            while (reader.Read())
            {
                string name = (string)reader["PropertyName"];
                string val = (string)reader["PropertyValue"];

                if (name.ToLower().IndexOf("password") != -1 && decryptPassword)
                    val = CryptoUtils.Decrypt(val);

                settings.Add(name, val);
            }
            reader.Close();

            return settings;
        }

        public static int UpdateServiceSettings(int serviceId, string[] settings)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            if (settings != null)
            {
                // build xml
                XmlDocument doc = new XmlDocument();
                XmlElement nodeProps = doc.CreateElement("properties");
                foreach (string setting in settings)
                {
                    int idx = setting.IndexOf('=');
                    string name = setting.Substring(0, idx);
                    string val = setting.Substring(idx + 1);

                    if (name.ToLower().IndexOf("password") != -1)
                        val = CryptoUtils.Encrypt(val);

                    XmlElement nodeProp = doc.CreateElement("property");
                    nodeProp.SetAttribute("name", name);
                    nodeProp.SetAttribute("value", val);
                    nodeProps.AppendChild(nodeProp);
                }

                string xml = nodeProps.OuterXml;

                // update settings
                DataProvider.UpdateServiceProperties(serviceId, xml);
            }

            return 0;
        }

        public static string[] InstallService(int serviceId)
        {
            ServiceProvider prov = new ServiceProvider();
            ServiceProviderProxy.Init(prov, serviceId);
            return prov.Install();
        }

        public static QuotaInfo GetProviderServiceQuota(int providerId)
        {
            return ObjectUtils.FillObjectFromDataReader<QuotaInfo>(
                DataProvider.GetProviderServiceQuota(providerId));
        }
        #endregion

        #region Providers

        public static List<ProviderInfo> GetInstalledProviders(int groupId)
        {
            List<ProviderInfo> provs = new List<ProviderInfo>();
            ObjectUtils.FillCollectionFromDataSet<ProviderInfo>(provs,
                DataProvider.GetGroupProviders(groupId));
            return provs;
        }

        public static List<ResourceGroupInfo> GetResourceGroups()
        {
            List<ResourceGroupInfo> groups = new List<ResourceGroupInfo>();
            ObjectUtils.FillCollectionFromDataSet<ResourceGroupInfo>(groups,
                DataProvider.GetResourceGroups());
            return groups;
        }

        public static ResourceGroupInfo GetResourceGroup(int groupId)
        {
            return ObjectUtils.FillObjectFromDataReader<ResourceGroupInfo>(
                DataProvider.GetResourceGroup(groupId));
        }

        public static ResourceGroupInfo GetResourceGroupByName(string name)
        {
            return ObjectUtils.FillObjectFromDataReader<ResourceGroupInfo>(
                DataProvider.GetResourceGroupByName(name));
        }

        public static ProviderInfo GetProvider(int providerId)
        {
            return ObjectUtils.FillObjectFromDataReader<ProviderInfo>(
                DataProvider.GetProvider(providerId));
        }

        public static List<ProviderInfo> GetProviders()
        {
            List<ProviderInfo> provs = new List<ProviderInfo>();
            ObjectUtils.FillCollectionFromDataSet<ProviderInfo>(
                provs, DataProvider.GetProviders());
            return provs;
        }

        public static List<ProviderInfo> GetProvidersByGroupID(int groupId)
        {
            List<ProviderInfo> provs = new List<ProviderInfo>();
            ObjectUtils.FillCollectionFromDataSet<ProviderInfo>(
                provs, DataProvider.GetGroupProviders(groupId));
            return provs;
        }

        public static String GetMailFilterUrl(int packageId, string groupName)
        {
            // load service
            String l_stURL = DataProvider.GetMailFilterURL(SecurityContext.User.UserId, packageId, groupName);

            if (String.IsNullOrEmpty(l_stURL))
                return string.Empty;

            return l_stURL;
        }

        public static String GetMailFilterUrlByHostingPlan(int PlanId, string groupName)
        {
            // load service
            String l_stURL = DataProvider.GetMailFilterUrlByHostingPlan(SecurityContext.User.UserId, PlanId, groupName);

            if (String.IsNullOrEmpty(l_stURL))
                return string.Empty;

            return l_stURL;
        }
        public static ProviderInfo GetPackageServiceProvider(int packageId, string groupName)
        {
            // load service
            int serviceId = PackageController.GetPackageServiceId(packageId, groupName);

            if (serviceId == 0)
                return null;

            ServiceInfo service = GetServiceInfo(serviceId);
            return GetProvider(service.ProviderId);
        }
        public static BoolResult IsInstalled(int serverId, int providerId)
        {
            BoolResult res = TaskManager.StartResultTask<BoolResult>("AUTO_DISCOVERY", "IS_INSTALLED");

            try
            {
                ProviderInfo provider = GetProvider(providerId);
                if (provider == null)
                {
                    TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_PROVIDER_INFO);
                    return res;
                }

                AutoDiscovery.AutoDiscovery ad = new AutoDiscovery.AutoDiscovery();
                ServiceProviderProxy.ServerInit(ad, serverId);

                res = ad.IsInstalled(provider.ProviderType);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_CHECK_IF_PROVIDER_SOFTWARE_INSTALLED, ex);

            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static string GetServerVersion(int serverId)
        {
            AutoDiscovery.AutoDiscovery ad = new AutoDiscovery.AutoDiscovery();
            ServiceProviderProxy.ServerInit(ad, serverId);

            return ad.GetServerVersion();
        }

        public static string GetServerFilePath(int serverId)
        {
            AutoDiscovery.AutoDiscovery ad = new AutoDiscovery.AutoDiscovery();
            ServiceProviderProxy.ServerInit(ad, serverId);

            return ad.GetServerFilePath(); // ad.GetServer
        }

        #endregion

        #region IP Addresses
        public static List<IPAddressInfo> GetIPAddresses(IPAddressPool pool, int serverId)
        {
            return ObjectUtils.CreateListFromDataReader<IPAddressInfo>(
                DataProvider.GetIPAddresses(SecurityContext.User.UserId, (int)pool, serverId));
        }

        public static IPAddressesPaged GetIPAddressesPaged(IPAddressPool pool, int serverId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            IPAddressesPaged result = new IPAddressesPaged();

            // get reader
            IDataReader reader = DataProvider.GetIPAddressesPaged(SecurityContext.User.UserId, (int)pool, serverId, filterColumn, filterValue, sortColumn, startRow, maximumRows);

            // number of items = first data reader
            reader.Read();
            result.Count = (int)reader[0];

            // items = second data reader
            reader.NextResult();
            result.Items = ObjectUtils.CreateListFromDataReader<IPAddressInfo>(reader).ToArray();

            return result;
        }

        public static IPAddressInfo GetIPAddress(int addressId)
        {
            return ObjectUtils.FillObjectFromDataReader<IPAddressInfo>(
                DataProvider.GetIPAddress(addressId));
        }

        public static string GetExternalIPAddress(int addressId)
        {
            IPAddressInfo ip = GetIPAddress(addressId);
            return (ip != null ? ip.ExternalIP : null);
        }

        public static IntResult AddIPAddress(IPAddressPool pool, int serverId,
            string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            IntResult res = new IntResult();

            #region Check account statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsAdmin | DemandAccount.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<IntResult>("IP_ADDRESS", "ADD", externalIP);

            TaskManager.WriteParameter("IP Address", externalIP);
            TaskManager.WriteParameter("NAT Address", internalIP);

            try
            {
                res.Value = DataProvider.AddIPAddress((int)pool, serverId, externalIP, internalIP,
                                            subnetMask, defaultGateway, comments, VLAN);

            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, "IP_ADDRESS_ADD_ERROR", ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject AddIPAddressesRange(IPAddressPool pool, int serverId,
            string externalIP, string endIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            const int MaxSubnet = 512; // TODO bigger max subnet?

            ResultObject res = new ResultObject();

            #region Check account statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsAdmin | DemandAccount.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("IP_ADDRESS", "ADD_RANGE", externalIP);

            TaskManager.WriteParameter("IP Address", externalIP);
            TaskManager.WriteParameter("End IP Address", endIP);
            TaskManager.WriteParameter("NAT Address", internalIP);

            try
            {
                if (externalIP == endIP)
                {
                    // add single IP and exit
                    AddIPAddress(pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
                    TaskManager.CompleteResultTask();
                    return res;
                }

                if (pool == IPAddressPool.PhoneNumbers)
                {
                    string phoneFormat = "D" + Math.Max(externalIP.Length, endIP.Length);

                    UInt64 start = UInt64.Parse(externalIP);
                    UInt64 end = UInt64.Parse(endIP);

                    if (end < start) { UInt64 temp = start; start = end; end = temp; }

                    const UInt64 maxPhones = 1000; // TODO max?

                    end = Math.Min(end, start + maxPhones);

                    for (UInt64 number = start; number <= end; number++)
                        DataProvider.AddIPAddress((int)pool, serverId, number.ToString(phoneFormat), "", subnetMask, defaultGateway, comments, VLAN);
                }

                else
                {
                    var startExternalIP = IPAddress.Parse(externalIP);
                    var startInternalIP = IPAddress.Parse(internalIP);
                    var endExternalIP = IPAddress.Parse(endIP);

                    // handle CIDR notation IP/Subnet addresses
                    if (startExternalIP.IsSubnet && endExternalIP == null)
                    {
                        endExternalIP = startExternalIP.LastSubnetIP;
                        startExternalIP = startExternalIP.FirstSubnetIP;
                    }

                    if (startExternalIP.V6 != startInternalIP.V6 && (startExternalIP.V6 != endExternalIP.V6 && endExternalIP != null)) throw new NotSupportedException("All IP addresses must be either V4 or V6.");

                    int i = 0;
                    long step = ((endExternalIP - startExternalIP) > 0) ? 1 : -1;

                    while (true)
                    {
                        if (i > MaxSubnet)
                            break;

                        // add IP address
                        DataProvider.AddIPAddress((int)pool, serverId, startExternalIP.ToString(), startInternalIP.ToString(), subnetMask, defaultGateway, comments, VLAN);

                        if (startExternalIP == endExternalIP)
                            break;

                        i++;

                        startExternalIP += step;

                        if (startInternalIP != 0)
                            startInternalIP += step;
                    }
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, "IP_ADDRESS_ADD_RANGE_ERROR", ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject UpdateIPAddress(int addressId, IPAddressPool pool, int serverId,
            string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            ResultObject res = new ResultObject();

            #region Check account statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsAdmin | DemandAccount.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("IP_ADDRESS", "UPDATE");

            try
            {
                DataProvider.UpdateIPAddress(addressId, (int)pool, serverId, externalIP, internalIP, subnetMask, defaultGateway, comments, VLAN);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, "IP_ADDRESS_UPDATE_ERROR", ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject UpdateIPAddresses(int[] addresses, IPAddressPool pool, int serverId,
            string subnetMask, string defaultGateway, string comments, int VLAN)
        {
            ResultObject res = new ResultObject();

            #region Check account statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsAdmin | DemandAccount.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("IP_ADDRESS", "UPDATE_RANGE");

            try
            {
                string xmlIds = PrepareIPsXML(addresses);
                DataProvider.UpdateIPAddresses(xmlIds, (int)pool, serverId, subnetMask, defaultGateway, comments, VLAN);
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, "IP_ADDRESSES_UPDATE_ERROR", ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject DeleteIPAddresses(int[] addresses)
        {
            ResultObject res = new ResultObject();

            #region Check account statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsAdmin | DemandAccount.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("IP_ADDRESS", "DELETE_RANGE");

            try
            {
                foreach (int addressId in addresses)
                {
                    ResultObject addrRes = DeleteIPAddress(addressId);
                    if (!addrRes.IsSuccess && addrRes.ErrorCodes.Count > 0)
                    {
                        res.ErrorCodes.AddRange(addrRes.ErrorCodes);
                        res.IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, "IP_ADDRESS_DELETE_RANGE_ERROR", ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject DeleteIPAddress(int addressId)
        {
            ResultObject res = new ResultObject();

            #region Check account statuses
            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsAdmin | DemandAccount.IsActive))
                return res;
            #endregion

            // start task
            res = TaskManager.StartResultTask<ResultObject>("IP_ADDRESS", "DELETE");

            try
            {
                int result = DataProvider.DeleteIPAddress(addressId);
                if (result == -1)
                {
                    TaskManager.CompleteResultTask(res, "ERROR_IP_USED_IN_NAME_SERVER");
                    return res;
                }
                else if (result == -2)
                {
                    TaskManager.CompleteResultTask(res, "ERROR_IP_USED_BY_PACKAGE_ITEM");
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, "IP_ADDRESS_DELETE_ERROR", ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }
        #endregion

        #region Package IP Addresses
        public static PackageIPAddressesPaged GetPackageIPAddresses(int packageId, int orgId, IPAddressPool pool,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            PackageIPAddressesPaged result = new PackageIPAddressesPaged();

            // get reader
            IDataReader reader = DataProvider.GetPackageIPAddresses(packageId, orgId, (int)pool, filterColumn, filterValue, sortColumn, startRow, maximumRows, recursive);

            // number of items = first data reader
            reader.Read();
            result.Count = (int)reader[0];

            // items = second data reader
            reader.NextResult();
            result.Items = ObjectUtils.CreateListFromDataReader<PackageIPAddress>(reader).ToArray();

            return result;
        }

        public static int GetPackageIPAddressesCount(int packageId, int orgId, IPAddressPool pool)
        {
            return DataProvider.GetPackageIPAddressesCount(packageId, orgId, (int)pool);
        }

        public static List<IPAddressInfo> GetUnallottedIPAddresses(int packageId, string groupName, IPAddressPool pool)
        {

            int serviceId = 0;
            bool servicebyid = int.TryParse(groupName, out serviceId);
            if (!servicebyid) // get service ID
                serviceId = PackageController.GetPackageServiceId(packageId, groupName);


            // get unallotted addresses
            return ObjectUtils.CreateListFromDataReader<IPAddressInfo>(
                DataProvider.GetUnallottedIPAddresses(packageId, serviceId, (int)pool));
        }

        public static List<PackageIPAddress> GetPackageUnassignedIPAddresses(int packageId, int orgId, IPAddressPool pool)
        {
            return ObjectUtils.CreateListFromDataReader<PackageIPAddress>(
                DataProvider.GetPackageUnassignedIPAddresses(SecurityContext.User.UserId, packageId, orgId, (int)pool));
        }

        public static List<PackageIPAddress> GetPackageUnassignedIPAddresses(int packageId, IPAddressPool pool)
        {
            return GetPackageUnassignedIPAddresses(packageId, 0, pool);
        }

        public static void AllocatePackageIPAddresses(int packageId, int[] addressId)
        {
            // prepare XML document
            string xml = PrepareIPsXML(addressId);

            // save to database
            DataProvider.AllocatePackageIPAddresses(packageId, 0, xml);
        }

        public static ResultObject AllocatePackageIPAddresses(int packageId, int orgId, string groupName, IPAddressPool pool, bool allocateRandom, int addressesNumber, int[] addressId)
        {
            #region Check account and space statuses
            // create result object
            ResultObject res = new ResultObject();

            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, packageId, DemandPackage.IsActive))
                return res;
            #endregion

            // get total number of addresses requested
            if (!allocateRandom && addressId != null)
                addressesNumber = addressId.Length;

            if (addressesNumber <= 0)
            {
                res.IsSuccess = true;
                return res; // just exit
            }

            // check quotas
            string quotaName = GetIPAddressesQuotaByResourceGroup(groupName, pool);

            // get maximum server IPs
            List<IPAddressInfo> ips = ServerController.GetUnallottedIPAddresses(packageId, groupName, pool);
            int maxAvailableIPs = ips.Count;

            if (maxAvailableIPs == 0)
            {
                res.ErrorCodes.Add("IP_ADDRESSES_POOL_IS_EMPTY");
                return res;
            }

            // get hosting plan IP limits
            PackageContext cntx = PackageController.GetPackageContext(packageId);
            int quotaAllocated = cntx.Quotas[quotaName].QuotaAllocatedValue;
            int quotaUsed = cntx.Quotas[quotaName].QuotaUsedValue;

            if (pool == IPAddressPool.PhoneNumbers)
                quotaUsed = ServerController.GetPackageIPAddressesCount(packageId, orgId, pool);

            // check the maximum allowed number
            if (quotaAllocated != -1) // check only if not unlimited 
            {
                if (addressesNumber > (quotaAllocated - quotaUsed))
                {
                    res.ErrorCodes.Add("IP_ADDRESSES_QUOTA_LIMIT_REACHED");
                    return res;
                }
            }

            // check if requested more than available
            if (maxAvailableIPs != -1 &&
                (addressesNumber > maxAvailableIPs))
                addressesNumber = maxAvailableIPs;

            res = TaskManager.StartResultTask<ResultObject>("IP_ADDRESS", "ALLOCATE_PACKAGE_IP", packageId);

            try
            {
                if (allocateRandom)
                {
                    int[] ids = new int[addressesNumber];
                    for (int i = 0; i < addressesNumber; i++)
                        ids[i] = ips[i].AddressId;

                    addressId = ids;
                }

                // prepare XML document
                string xml = PrepareIPsXML(addressId);

                // save to database
                try
                {
                    DataProvider.AllocatePackageIPAddresses(packageId, orgId, xml);
                }
                catch (Exception ex)
                {
                    TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.CANNOT_ADD_IP_ADDRESSES_TO_DATABASE, ex);
                    return res;
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.ALLOCATE_EXTERNAL_ADDRESSES_GENERAL_ERROR, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        public static ResultObject AllocateMaximumPackageIPAddresses(int packageId, string groupName, IPAddressPool pool)
        {
            // get maximum server IPs
            int maxAvailableIPs = GetUnallottedIPAddresses(packageId, groupName, pool).Count;

            // get quota name
            string quotaName = GetIPAddressesQuotaByResourceGroup(groupName, pool);

            // get hosting plan IPs
            int number = 0;

            PackageContext cntx = PackageController.GetPackageContext(packageId);
            if (cntx.Quotas.ContainsKey(quotaName))
            {
                number = cntx.Quotas[quotaName].QuotaAllocatedValue;
                if (number == -1)
                {
                    // unlimited
                    if (number > maxAvailableIPs) // requested more than available
                        number = maxAvailableIPs; // assign max available server IPs
                }
                else
                {
                    // quota
                    number = number - cntx.Quotas[quotaName].QuotaUsedValue;
                }
            }

            // allocate
            return AllocatePackageIPAddresses(packageId, 0, groupName, pool,
                true, number, new int[0]);
        }

        public static ResultObject DeallocatePackageIPAddresses(int packageId, int[] addressId)
        {
            #region Check account and space statuses
            // create result object
            ResultObject res = new ResultObject();

            // check account
            if (!SecurityContext.CheckAccount(res, DemandAccount.NotDemo | DemandAccount.IsActive))
                return res;

            // check package
            if (!SecurityContext.CheckPackage(res, packageId, DemandPackage.IsActive))
                return res;
            #endregion

            res = TaskManager.StartResultTask<ResultObject>("IP_ADDRESS", "DEALLOCATE_PACKAGE_IP", packageId);

            try
            {
                foreach (int id in addressId)
                {
                    DataProvider.DeallocatePackageIPAddress(id);
                }
            }
            catch (Exception ex)
            {
                TaskManager.CompleteResultTask(res, VirtualizationErrorCodes.CANNOT_DELLOCATE_EXTERNAL_ADDRESSES, ex);
                return res;
            }

            TaskManager.CompleteResultTask();
            return res;
        }

        #region Item IP Addresses
        public static List<PackageIPAddress> GetItemIPAddresses(int itemId, IPAddressPool pool)
        {
            return ObjectUtils.CreateListFromDataReader<PackageIPAddress>(
                DataProvider.GetItemIPAddresses(SecurityContext.User.UserId, itemId, (int)pool));
        }

        public static PackageIPAddress GetPackageIPAddress(int packageAddressId)
        {
            return ObjectUtils.FillObjectFromDataReader<PackageIPAddress>(
                DataProvider.GetPackageIPAddress(packageAddressId));
        }

        public static int AddItemIPAddress(int itemId, int packageAddressId)
        {
            return DataProvider.AddItemIPAddress(SecurityContext.User.UserId, itemId, packageAddressId);
        }

        public static int SetItemPrimaryIPAddress(int itemId, int packageAddressId)
        {
            return DataProvider.SetItemPrimaryIPAddress(SecurityContext.User.UserId, itemId, packageAddressId);
        }

        public static int DeleteItemIPAddress(int itemId, int packageAddressId)
        {
            return DataProvider.DeleteItemIPAddress(SecurityContext.User.UserId, itemId, packageAddressId);
        }

        public static int DeleteItemIPAddresses(int itemId)
        {
            return DataProvider.DeleteItemIPAddresses(SecurityContext.User.UserId, itemId);
        }

        #endregion

        public static string PrepareIPsXML(int[] items)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("items");
            foreach (int item in items)
            {
                XmlNode node = doc.CreateElement("item");
                XmlAttribute attribute = doc.CreateAttribute("id");
                attribute.Value = item.ToString();
                node.Attributes.Append(attribute);
                root.AppendChild(node);
            }
            doc.AppendChild(root);
            return doc.InnerXml;
        }

        private static string GetIPAddressesQuotaByResourceGroup(string groupName, IPAddressPool pool)
        {
            if (pool == IPAddressPool.PhoneNumbers)
            {
                if (groupName == "SfB")
                {
                    return Quotas.SFB_PHONE;
                }
                else
                {
                    return Quotas.LYNC_PHONE;
                }
            }

            if (String.Compare(groupName, ResourceGroups.VPS, true) == 0)
            {
                return Quotas.VPS_EXTERNAL_IP_ADDRESSES_NUMBER;
            }
            else if (String.Compare(groupName, ResourceGroups.VPS2012, true) == 0)
            {
                return Quotas.VPS2012_EXTERNAL_IP_ADDRESSES_NUMBER;
            }
            else if (String.Compare(groupName, ResourceGroups.VPSForPC, true) == 0)
            {
                return Quotas.VPSForPC_EXTERNAL_IP_ADDRESSES_NUMBER;
            }
            else if (String.Compare(groupName, ResourceGroups.Proxmox, true) == 0)
            {
                return Quotas.PROXMOX_EXTERNAL_IP_ADDRESSES_NUMBER;
            }
            else
            {
                return Quotas.WEB_IP_ADDRESSES;
            }
        }
        #endregion

        #region Clusters
        public static List<ClusterInfo> GetClusters()
        {
            List<ClusterInfo> list = new List<ClusterInfo>();
            ObjectUtils.FillCollectionFromDataReader<ClusterInfo>(list,
                DataProvider.GetClusters(SecurityContext.User.UserId));
            return list;
        }

        public static int AddCluster(ClusterInfo cluster)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            return DataProvider.AddCluster(cluster.ClusterName);
        }

        public static int DeleteCluster(int clusterId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            DataProvider.DeleteCluster(clusterId);

            return 0;
        }
        #endregion

        #region Global DNS records
        public static DataSet GetRawDnsRecordsByService(int serviceId)
        {
            return DataProvider.GetDnsRecordsByService(SecurityContext.User.UserId, serviceId);
        }

        public static DataSet GetRawDnsRecordsByServer(int serverId)
        {
            return DataProvider.GetDnsRecordsByServer(SecurityContext.User.UserId, serverId);
        }

        public static DataSet GetRawDnsRecordsByPackage(int packageId)
        {
            return DataProvider.GetDnsRecordsByPackage(SecurityContext.User.UserId, packageId);
        }

        public static DataSet GetRawDnsRecordsByGroup(int groupId)
        {
            return DataProvider.GetDnsRecordsByGroup(groupId);
        }

        public static DataSet GetRawDnsRecordsTotal(int packageId)
        {
            return DataProvider.GetDnsRecordsTotal(SecurityContext.User.UserId, packageId);
        }

        public static List<GlobalDnsRecord> GetDnsRecordsByService(int serviceId)
        {
            return ObjectUtils.CreateListFromDataSet<GlobalDnsRecord>(
                DataProvider.GetDnsRecordsByService(SecurityContext.User.UserId, serviceId));
        }

        public static List<GlobalDnsRecord> GetDnsRecordsByServer(int serverId)
        {
            return ObjectUtils.CreateListFromDataSet<GlobalDnsRecord>(
                DataProvider.GetDnsRecordsByServer(SecurityContext.User.UserId, serverId));
        }

        public static List<GlobalDnsRecord> GetDnsRecordsByPackage(int packageId)
        {
            return ObjectUtils.CreateListFromDataSet<GlobalDnsRecord>(
                DataProvider.GetDnsRecordsByPackage(SecurityContext.User.UserId, packageId));
        }

        public static List<GlobalDnsRecord> GetDnsRecordsByGroup(int groupId)
        {
            return ObjectUtils.CreateListFromDataSet<GlobalDnsRecord>(
                DataProvider.GetDnsRecordsByGroup(groupId));
        }

        public static List<GlobalDnsRecord> GetDnsRecordsTotal(int packageId)
        {
            return ObjectUtils.CreateListFromDataSet<GlobalDnsRecord>(
                GetRawDnsRecordsTotal(packageId));
        }

        public static GlobalDnsRecord GetDnsRecord(int recordId)
        {
            return ObjectUtils.FillObjectFromDataReader<GlobalDnsRecord>(
                DataProvider.GetDnsRecord(SecurityContext.User.UserId, recordId));
        }

        public static int AddDnsRecord(GlobalDnsRecord record)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsReseller
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            TaskManager.StartTask("GLOBAL_DNS", "ADD", record.RecordName);
            TaskManager.WriteParameter("Type", record.RecordType);
            TaskManager.WriteParameter("Data", record.RecordData);

            DataProvider.AddDnsRecord(SecurityContext.User.UserId, record.ServiceId, record.ServerId, record.PackageId,
                record.RecordType, record.RecordName, record.RecordData, record.MxPriority,
                record.SrvPriority, record.SrvWeight, record.SrvPort, record.IpAddressId);

            TaskManager.CompleteTask();

            return 0;
        }

        public static int UpdateDnsRecord(GlobalDnsRecord record)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsReseller
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            TaskManager.StartTask("GLOBAL_DNS", "UPDATE", record.RecordName);
            TaskManager.WriteParameter("Type", record.RecordType);
            TaskManager.WriteParameter("Data", record.RecordData);

            DataProvider.UpdateDnsRecord(SecurityContext.User.UserId, record.RecordId,
                record.RecordType, record.RecordName, record.RecordData, record.MxPriority,
                record.SrvPriority, record.SrvWeight, record.SrvPort, record.IpAddressId);

            TaskManager.CompleteTask();

            return 0;
        }

        public static int DeleteDnsRecord(int recordId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsReseller
                | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            GlobalDnsRecord record = GetDnsRecord(recordId);

            TaskManager.StartTask("GLOBAL_DNS", "DELETE", record.RecordName);
            TaskManager.WriteParameter("Type", record.RecordType);
            TaskManager.WriteParameter("Data", record.RecordData);

            DataProvider.DeleteDnsRecord(SecurityContext.User.UserId, recordId);

            TaskManager.CompleteTask();

            return 0;
        }
        #endregion

        #region Domains

        public static List<DnsRecordInfo> GetDomainDnsRecords(int domainId)
        {
            var result = new List<DnsRecordInfo>();

            var records = ObjectUtils.CreateListFromDataReader<DnsRecordInfo>(DataProvider.GetDomainAllDnsRecords(domainId));

            var activeDomain = records.OrderByDescending(x => x.Date).FirstOrDefault();

            if (activeDomain != null)
            {
                records = records.Where(x => x.DnsServer == activeDomain.DnsServer).ToList();
            }

            result.AddRange(records);

            return result;
        }


        public static int CheckDomain(string domainName)
        {
            int checkDomainResult = DataProvider.CheckDomain(-10, domainName, false);

            if (checkDomainResult == -1)
                return BusinessErrorCodes.ERROR_DOMAIN_ALREADY_EXISTS;
            else if (checkDomainResult == -2)
                return BusinessErrorCodes.ERROR_RESTRICTED_DOMAIN;
            else
                return checkDomainResult;
        }

        public static List<DomainInfo> GetDomains(int packageId, bool recursive)
        {
            return ObjectUtils.CreateListFromDataSet<DomainInfo>(
                DataProvider.GetDomains(SecurityContext.User.UserId, packageId, recursive));
        }

        public static List<DomainInfo> GetDomains(int packageId)
        {
            return ObjectUtils.CreateListFromDataSet<DomainInfo>(
                DataProvider.GetDomains(SecurityContext.User.UserId, packageId, true));
        }


        public static List<DomainInfo> GetDomainsByZoneId(int zoneId)
        {
            return ObjectUtils.CreateListFromDataSet<DomainInfo>(
                DataProvider.GetDomainsByZoneId(SecurityContext.User.UserId, zoneId));
        }

        public static List<DomainInfo> GetDomainsByDomainItemId(int zoneId)
        {
            return ObjectUtils.CreateListFromDataSet<DomainInfo>(
                DataProvider.GetDomainsByDomainItemId(SecurityContext.User.UserId, zoneId));
        }


        public static List<DomainInfo> GetMyDomains(int packageId)
        {
            return ObjectUtils.CreateListFromDataSet<DomainInfo>(
                DataProvider.GetDomains(SecurityContext.User.UserId, packageId, false));
        }

        public static List<DomainInfo> GetResellerDomains(int packageId)
        {
            return ObjectUtils.CreateListFromDataSet<DomainInfo>(
                DataProvider.GetResellerDomains(SecurityContext.User.UserId, packageId));
        }

        public static DataSet GetDomainsPaged(int packageId, int serverId, bool recursive, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            DataSet ds = DataProvider.GetDomainsPaged(SecurityContext.User.UserId,
                packageId, serverId, recursive, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);

            return ds;
        }

        public static DomainInfo GetDomain(int domainId, bool withLog = true)
        {
            // get domain by ID
            DomainInfo domain = GetDomainItem(domainId);

            // return
            return GetDomain(domain, withLog);
        }

        public static DomainInfo GetDomain(string domainName)
        {
            return ObjectUtils.FillObjectFromDataReader<DomainInfo>(
                DataProvider.GetDomainByName(SecurityContext.User.UserId, domainName, false, false));
        }

        public static DomainInfo GetDomain(string domainName, bool searchOnDomainPointer, bool isDomainPointer)
        {
            return GetDomainItem(domainName, searchOnDomainPointer, isDomainPointer);
        }


        private static DomainInfo GetDomain(DomainInfo domain, bool withLog = true)
        {
            // check domain
            if (domain == null)
                return null;

            // get instant alias
            domain.InstantAliasName = GetDomainAlias(domain.PackageId, domain.DomainName);
            DomainInfo instantAlias = GetDomainItem(domain.InstantAliasName, true, false);
            if (instantAlias != null)
                domain.InstantAliasId = instantAlias.DomainId;

            // Log Extension
            if (withLog)
                LogExtension.WriteObject(domain);

            return domain;
        }

        public static DomainInfo GetDomainItem(int domainId)
        {
            return ObjectUtils.FillObjectFromDataReader<DomainInfo>(
                DataProvider.GetDomain(SecurityContext.User.UserId, domainId));
        }

        public static DomainInfo GetDomainItem(string domainName)
        {
            return GetDomainItem(domainName, false, false);
        }


        public static DomainInfo GetDomainItem(string domainName, bool searchOnDomainPointer, bool isDomainPointer)
        {
            return ObjectUtils.FillObjectFromDataReader<DomainInfo>(
                DataProvider.GetDomainByName(SecurityContext.User.UserId, domainName, searchOnDomainPointer, isDomainPointer));
        }

        public static string GetDomainAlias(int packageId, string domainName)
        {
            // load package settings
            PackageSettings packageSettings = PackageController.GetPackageSettings(packageId,
                PackageSettings.INSTANT_ALIAS);

            string instantAlias = packageSettings["InstantAlias"];

            // add instant alias
            if (!String.IsNullOrEmpty(instantAlias))
            {
                instantAlias = domainName + "." + instantAlias;
            }
            return instantAlias;
        }

        public static int AddDomainWithProvisioning(int packageId, string domainName, DomainType domainType,
            bool createWebSite, int pointWebSiteId, int pointMailDomainId, bool createDnsZone, bool createInstantAlias, bool allowSubDomains, string hostName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // set flags
            bool isSubDomain = (domainType == DomainType.SubDomain || domainType == DomainType.ProviderSubDomain);
            bool isDomainPointer = (domainType == DomainType.DomainPointer);

            // check services
            bool dnsEnabled = (PackageController.GetPackageServiceId(packageId, ResourceGroups.Dns) > 0);
            bool webEnabled = (PackageController.GetPackageServiceId(packageId, ResourceGroups.Web) > 0);
            bool mailEnabled = (PackageController.GetPackageServiceId(packageId, ResourceGroups.Mail) > 0);
            bool MailCleanerEnabled = (PackageController.GetPackageServiceId(packageId, ResourceGroups.Filters) > 0);

            // add main domain
            int domainId = AddDomainInternal(packageId, domainName, createDnsZone && dnsEnabled, isSubDomain, false, isDomainPointer, allowSubDomains);
            if (domainId < 0)
                return domainId;

            DomainInfo domain = ServerController.GetDomain(domainId);
            if (domain != null)
            {
                if (domain.ZoneItemId != 0)
                {
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.Os, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.Dns, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.Ftp, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2000, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2005, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2008, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2012, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2014, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MsSql2016, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MySql4, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MySql5, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.MariaDB, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.Statistics, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.VPS, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.VPS2012, domain, "");
                    ServerController.AddServiceDNSRecords(packageId, ResourceGroups.VPSForPC, domain, "");
                }

                UpdateDomainWhoisData(domain);
            }

            // add instant alias
            createInstantAlias &= (domainType != DomainType.DomainPointer);
            if (createInstantAlias)
            {
                // check if instant alias is configured
                string domainAlias = GetDomainAlias(packageId, domainName);

                // add instant alias if required
                if (!String.IsNullOrEmpty(domainAlias))
                {
                    // add alias
                    AddDomainInternal(packageId, domainAlias, dnsEnabled, false, true, false, false);
                }
            }

            // create web site if requested
            int webSiteId = 0;
            if (webEnabled && createWebSite)
            {
                webSiteId = WebServerController.AddWebSite(packageId, hostName, domainId, 0, createInstantAlias, false);

                if (webSiteId < 0)
                {
                    // return
                    return webSiteId;
                }
            }

            // add web site pointer
            if (webEnabled && pointWebSiteId > 0)
            {
                WebServerController.AddWebSitePointer(pointWebSiteId, hostName, domainId, true, false, false);
            }

            // add mail domain pointer
            if (mailEnabled && pointMailDomainId > 0)
            {
                MailServerController.AddMailDomainPointer(pointMailDomainId, domainId);
            }

            return domainId;
        }

        public static int AddDomain(DomainInfo domain)
        {
            return AddDomain(domain, false, false);
        }

        public static int AddDomain(DomainInfo domain, bool createInstantAlias, bool createZone)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(domain.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // add main domain
            int domainId = AddDomainInternal(domain.PackageId, domain.DomainName, createZone,
                domain.IsSubDomain, createInstantAlias, domain.IsDomainPointer, false);

            /*
            if (domainId < 0)
                return domainId;

            // add instant alias if required
            string domainAlias = GetDomainAlias(domain.PackageId, domain.DomainName);
            if (createInstantAlias && !String.IsNullOrEmpty(domainAlias))
            {
                AddDomainInternal(domain.PackageId, domainAlias, true, false, true, false, false);
            }
            */

            return domainId;
        }

        private static int AddDomainInternal(int packageId, string domainName,
            bool createDnsZone, bool isSubDomain, bool isInstantAlias, bool isDomainPointer, bool allowSubDomains)
        {
            // check quota
            if (!isInstantAlias)
            {
                if (isSubDomain)
                {
                    // sub-domain
                    if (PackageController.GetPackageQuota(packageId, Quotas.OS_SUBDOMAINS).QuotaExhausted)
                        return BusinessErrorCodes.ERROR_SUBDOMAIN_QUOTA_LIMIT;
                }
                else if (isDomainPointer)
                {
                    // domain pointer
                    //if (PackageController.GetPackageQuota(packageId, Quotas.OS_DOMAINPOINTERS).QuotaExhausted)
                    //    return BusinessErrorCodes.ERROR_DOMAIN_QUOTA_LIMIT;
                }
                else
                {
                    // top-level domain
                    if (PackageController.GetPackageQuota(packageId, Quotas.OS_DOMAINS).QuotaExhausted)
                        return BusinessErrorCodes.ERROR_DOMAIN_QUOTA_LIMIT;
                }
            }

            // check if the domain already exists
            int checkResult = DataProvider.CheckDomain(packageId, domainName, isDomainPointer);

            if (checkResult < 0)
            {
                if (checkResult == -1)
                    return BusinessErrorCodes.ERROR_DOMAIN_ALREADY_EXISTS;
                else if (checkResult == -2)
                    return BusinessErrorCodes.ERROR_RESTRICTED_DOMAIN;
                else
                    return checkResult;
            }
            /*
                        if (domainName.ToLower().StartsWith("www."))
                            return BusinessErrorCodes.ERROR_DOMAIN_STARTS_WWW;
            */
            // place log record
            TaskManager.StartTask("DOMAIN", "ADD", domainName, 0, packageId, new BackgroundTaskParameter("CreateZone", createDnsZone));

            // Log Extension
            LogExtension.WriteVariables(new { domainName, createDnsZone, isSubDomain, isInstantAlias, isDomainPointer, allowSubDomains });

            // create DNS zone
            int zoneItemId = 0;
            if (createDnsZone)
            {
                try
                {
                    // add DNS zone
                    int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Dns);
                    if (serviceId > 0)
                    {
                        zoneItemId = DnsServerController.AddZone(packageId, serviceId, domainName, true, isInstantAlias);
                    }

                    if (zoneItemId < 0)
                    {
                        TaskManager.CompleteTask();
                        return zoneItemId;
                    }
                }
                catch (Exception ex)
                {
                    throw TaskManager.WriteError(ex);
                }
            }

            int itemId = DataProvider.AddDomain(SecurityContext.User.UserId,
                packageId, zoneItemId, domainName, allowSubDomains, 0, 0, isSubDomain, isInstantAlias, isDomainPointer);

            TaskManager.ItemId = itemId;

            TaskManager.CompleteTask();

            return itemId;
        }

        public static int AddDomainItem(DomainInfo domain)
        {
            return DataProvider.AddDomain(SecurityContext.User.UserId,
                domain.PackageId, domain.ZoneItemId, domain.DomainName, domain.HostingAllowed,
                domain.WebSiteId, domain.MailDomainId, domain.IsSubDomain, domain.IsInstantAlias, domain.IsDomainPointer);
        }

        public static void AddServiceDNSRecords(int packageId, string groupName, DomainInfo domain, string serviceIP)
        {
            AddServiceDNSRecords(packageId, groupName, domain, serviceIP, false);
        }

        public static void AddServiceDNSRecords(int packageId, string groupName, DomainInfo domain, string serviceIP, bool wildcardOnly)
        {
            int serviceId = PackageController.GetPackageServiceId(packageId, groupName);
            if (serviceId > 0)
            {
                List<DnsRecord> tmpZoneRecords = new List<DnsRecord>();
                List<GlobalDnsRecord> dnsRecords = ServerController.GetDnsRecordsByService(serviceId);

                if (wildcardOnly)
                {
                    List<GlobalDnsRecord> temp = new List<GlobalDnsRecord>();
                    foreach (GlobalDnsRecord d in dnsRecords)
                    {
                        if ((d.RecordName == "*") ||
                            (d.RecordName == "@"))
                            temp.Add(d);
                    }

                    dnsRecords = temp;
                }

                DnsZone zone = (DnsZone)PackageController.GetPackageItem(domain.ZoneItemId);
                tmpZoneRecords.AddRange(DnsServerController.BuildDnsResourceRecords(dnsRecords, "", domain.ZoneName, serviceIP));

                try
                {
                    DNSServer dns = new DNSServer();
                    ServiceProviderProxy.Init(dns, zone.ServiceId);

                    DnsRecord[] domainRecords = dns.GetZoneRecords(domain.DomainName);

                    List<DnsRecord> zoneRecords = new List<DnsRecord>();
                    foreach (DnsRecord t in tmpZoneRecords)
                    {
                        if (!RecordDoesExist(t, domainRecords))
                            zoneRecords.Add(t);
                    }


                    // add new resource records
                    dns.AddZoneRecords(zone.Name, zoneRecords.ToArray());
                }
                catch (Exception ex1)
                {
                    TaskManager.WriteError(ex1, "Error updating DNS records");
                }
            }
        }



        public static void RemoveServiceDNSRecords(int packageId, string groupName, DomainInfo domain, string serviceIP, bool wildcardOnly)
        {
            int serviceId = PackageController.GetPackageServiceId(packageId, groupName);
            if (serviceId > 0)
            {
                List<DnsRecord> zoneRecords = new List<DnsRecord>();
                List<GlobalDnsRecord> dnsRecords = ServerController.GetDnsRecordsByService(serviceId);
                if (wildcardOnly)
                {
                    List<GlobalDnsRecord> temp = new List<GlobalDnsRecord>();
                    foreach (GlobalDnsRecord d in dnsRecords)
                    {
                        if ((d.RecordName == "*") ||
                            (d.RecordName == "@"))
                            temp.Add(d);
                    }

                    dnsRecords = temp;
                }

                DnsZone zone = (DnsZone)PackageController.GetPackageItem(domain.ZoneItemId);
                zoneRecords.AddRange(DnsServerController.BuildDnsResourceRecords(dnsRecords, "", domain.ZoneName, serviceIP));

                try
                {
                    DNSServer dns = new DNSServer();
                    ServiceProviderProxy.Init(dns, zone.ServiceId);

                    // add new resource records
                    dns.DeleteZoneRecords(zone.Name, zoneRecords.ToArray());
                }
                catch (Exception ex1)
                {
                    TaskManager.WriteError(ex1, "Error updating DNS records");
                }
            }
        }


        private static bool RecordDoesExist(DnsRecord record, DnsRecord[] domainRecords)
        {
            foreach (DnsRecord d in domainRecords)
            {
                if ((record.RecordName.ToLower() == d.RecordName.ToLower()) &
                    (record.RecordType == d.RecordType))
                {
                    return true;
                }
            }

            return false;
        }


        public static int UpdateDomain(DomainInfo domain)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // place log record
            DomainInfo origDomain = GetDomain(domain.DomainId);
            TaskManager.StartTask("DOMAIN", "UPDATE", origDomain.DomainName, domain.DomainId);

            try
            {
                DataProvider.UpdateDomain(SecurityContext.User.UserId,
                    domain.DomainId, domain.ZoneItemId, domain.HostingAllowed, domain.WebSiteId,
                    domain.MailDomainId, domain.DomainItemId);

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

        public static int DetachDomain(int domainId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin);
            if (accountCheck < 0) return accountCheck;

            // load domain
            DomainInfo domain = GetDomain(domainId);
            if (domain == null)
                return 0;

            // place log record
            TaskManager.StartTask("DOMAIN", "DETACH", domain.DomainName, domain.DomainId);

            try
            {
                // check if domain can be deleted
                if (domain.WebSiteId > 0)
                {
                    TaskManager.WriteError("Domain points to the existing web site");
                    return BusinessErrorCodes.ERROR_DOMAIN_POINTS_TO_WEB_SITE;
                }

                if (domain.MailDomainId > 0)
                {
                    TaskManager.WriteError("Domain points to the existing mail domain");
                    return BusinessErrorCodes.ERROR_DOMAIN_POINTS_TO_MAIL_DOMAIN;
                }

                if (DataProvider.ExchangeOrganizationDomainExists(domain.DomainId))
                {
                    TaskManager.WriteError("Domain points to the existing organization domain");
                    return BusinessErrorCodes.ERROR_ORGANIZATION_DOMAIN_IS_IN_USE;
                }


                List<DomainInfo> domains = GetDomainsByDomainItemId(domain.DomainId);
                foreach (DomainInfo d in domains)
                {
                    if (d.WebSiteId > 0)
                    {
                        TaskManager.WriteError("Domain points to the existing web site");
                        return BusinessErrorCodes.ERROR_DOMAIN_POINTS_TO_WEB_SITE;
                    }
                }

                // Find and delete all zone items for this domain
                var zoneItems = PackageController.GetPackageItemsByType(domain.PackageId, ResourceGroups.Dns, typeof(DnsZone));
                zoneItems.AddRange(PackageController.GetPackageItemsByType(domain.PackageId, ResourceGroups.Dns, typeof(SecondaryDnsZone)));

                foreach (var zoneItem in zoneItems.Where(z => z.Name == domain.ZoneName))
                {
                    PackageController.DeletePackageItem(zoneItem.Id);
                }

                // delete domain
                DataProvider.DeleteDomain(SecurityContext.User.UserId, domainId);

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

        public static int DeleteDomain(int domainId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load domain
            DomainInfo domain = GetDomain(domainId);
            if (domain == null)
                return 0;

            // place log record
            TaskManager.StartTask("DOMAIN", "DELETE", domain.DomainName, domain.DomainId);

            try
            {
                // check if domain can be deleted
                if (domain.WebSiteId > 0)
                {
                    TaskManager.WriteError("Domain points to the existing web site");
                    return BusinessErrorCodes.ERROR_DOMAIN_POINTS_TO_WEB_SITE;
                }

                if (domain.MailDomainId > 0)
                {
                    TaskManager.WriteError("Domain points to the existing mail domain");
                    return BusinessErrorCodes.ERROR_DOMAIN_POINTS_TO_MAIL_DOMAIN;
                }

                if (DataProvider.ExchangeOrganizationDomainExists(domain.DomainId))
                {
                    TaskManager.WriteError("Domain points to the existing organization domain");
                    return BusinessErrorCodes.ERROR_ORGANIZATION_DOMAIN_IS_IN_USE;
                }


                if (!domain.IsDomainPointer)
                {
                    List<DomainInfo> domains = GetDomainsByDomainItemId(domain.DomainId);
                    foreach (DomainInfo d in domains)
                    {
                        if (d.WebSiteId > 0)
                        {
                            TaskManager.WriteError("Domain points to the existing web site");
                            return BusinessErrorCodes.ERROR_DOMAIN_POINTS_TO_WEB_SITE;
                        }
                    }
                }


                // delete instant alias
                if (domain.InstantAliasId > 0)
                {
                    int res = DeleteDomainInstantAlias(domainId);
                    if (res < 0)
                        return res;
                }

                // delete zone if required
                if (!domain.IsDomainPointer)
                    DnsServerController.DeleteZone(domain.ZoneItemId);

                // delete domain
                DataProvider.DeleteDomain(SecurityContext.User.UserId, domainId);

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

        public static int DisableDomainDns(int domainId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load domain
            DomainInfo domain = GetDomain(domainId);

            // check if already disabled
            if (domain.ZoneItemId == 0)
                return 0;

            // place log record
            TaskManager.StartTask("DOMAIN", "DISABLE_DNS", domain.DomainName, domain.DomainId);

            try
            {
                // delete instant alias
                int aliasResult = DeleteDomainInstantAlias(domainId);
                if (aliasResult < 0)
                    return aliasResult;

                // delete zone if required
                if (domain.ZoneItemId > 0)
                {
                    // delete zone
                    DnsServerController.DeleteZone(domain.ZoneItemId);

                    // update domain item
                    domain.ZoneItemId = 0;
                    UpdateDomain(domain);
                }

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

        public static int EnableDomainDns(int domainId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load domain
            DomainInfo domain = GetDomain(domainId);

            // check if already enabled
            if (domain.ZoneItemId > 0)
                return 0;

            // place log record
            TaskManager.StartTask("DOMAIN", "ENABLE_DNS", domain.DomainName, domain.DomainId);

            try
            {
                // create DNS zone
                int serviceId = PackageController.GetPackageServiceId(domain.PackageId, ResourceGroups.Dns);
                if (serviceId > 0)
                {
                    // add zone
                    int zoneItemId = DnsServerController.AddZone(domain.PackageId, serviceId, domain.DomainName);

                    // check results
                    if (zoneItemId < 0)
                    {
                        TaskManager.CompleteTask();
                        return zoneItemId;
                    }

                    // update domain
                    domain.ZoneItemId = zoneItemId;
                    UpdateDomain(domain);

                    domain = GetDomain(domainId);


                    PackageContext cntx = PackageController.GetPackageContext(domain.PackageId);
                    if (cntx != null)
                    {
                        // fill dictionaries
                        foreach (HostingPlanGroupInfo group in cntx.GroupsArray)
                        {
                            try
                            {
                                bool bFound = false;
                                switch (group.GroupName)
                                {
                                    case ResourceGroups.Dns:
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Ftp, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.MsSql2000, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.MsSql2005, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.MsSql2008, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.MsSql2012, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.MsSql2014, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.MsSql2016, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.MySql4, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.MySql5, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.MariaDB, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Statistics, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.VPS, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.VPS2012, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.VPSForPC, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Dns, domain, "");
                                        break;
                                    case ResourceGroups.Os:
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Os, domain, "");
                                        break;
                                    case ResourceGroups.HostedOrganizations:
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.HostedOrganizations, domain, "");
                                        ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.HostedCRM, domain, "");
                                        break;
                                    case ResourceGroups.Mail:
                                        List<DomainInfo> myDomains = ServerController.GetMyDomains(domain.PackageId);
                                        foreach (DomainInfo mailDomain in myDomains)
                                        {
                                            if ((mailDomain.MailDomainId != 0) && (domain.DomainName.ToLower() == mailDomain.DomainName.ToLower()))
                                            {
                                                bFound = true;
                                                break;
                                            }
                                        }
                                        if (bFound) ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Mail, domain, "");
                                        break;
                                    case ResourceGroups.Exchange:
                                        List<Organization> orgs = OrganizationController.GetOrganizations(domain.PackageId, false);
                                        foreach (Organization o in orgs)
                                        {
                                            List<OrganizationDomainName> names = OrganizationController.GetOrganizationDomains(o.Id);
                                            foreach (OrganizationDomainName name in names)
                                            {
                                                if (domain.DomainName.ToLower() == name.DomainName.ToLower())
                                                {
                                                    bFound = true;
                                                    break;
                                                }
                                            }
                                            if (bFound) break;
                                        }
                                        if (bFound)
                                        {
                                            ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Exchange, domain, "");
                                            ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.BlackBerry, domain, "");
                                            ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.OCS, domain, "");
                                        }
                                        break;
                                    case ResourceGroups.Lync:
                                        List<Organization> orgsLync = OrganizationController.GetOrganizations(domain.PackageId, false);
                                        foreach (Organization o in orgsLync)
                                        {
                                            if ((o.DefaultDomain.ToLower() == domain.DomainName.ToLower()) &
                                                (o.LyncTenantId != null))
                                            {
                                                bFound = true;
                                                break;
                                            }
                                        }
                                        if (bFound)
                                        {
                                            ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Lync, domain, "");
                                        }
                                        break;
                                    case ResourceGroups.SfB:
                                        List<Organization> orgsSfB = OrganizationController.GetOrganizations(domain.PackageId, false);
                                        foreach (Organization o in orgsSfB)
                                        {
                                            if ((o.DefaultDomain.ToLower() == domain.DomainName.ToLower()) &
                                                (o.SfBTenantId != null))
                                            {
                                                bFound = true;
                                                break;
                                            }
                                        }
                                        if (bFound)
                                        {
                                            ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.SfB, domain, "");
                                        }
                                        break;
                                    case ResourceGroups.Web:
                                        List<WebSite> sites = WebServerController.GetWebSites(domain.PackageId, false);
                                        foreach (WebSite w in sites)
                                        {
                                            if ((w.SiteId.ToLower().Replace("." + domain.DomainName.ToLower(), "").IndexOf('.') == -1) ||
                                                (w.SiteId.ToLower() == domain.DomainName.ToLower()))
                                            {
                                                WebServerController.AddWebSitePointer(w.Id,
                                                                                        (w.SiteId.ToLower() == domain.DomainName.ToLower()) ? "" : w.SiteId.ToLower().Replace("." + domain.DomainName.ToLower(), ""),
                                                                                        domain.DomainId, false, true, true);
                                            }

                                            List<DomainInfo> pointers = WebServerController.GetWebSitePointers(w.Id);
                                            foreach (DomainInfo pointer in pointers)
                                            {
                                                if ((pointer.DomainName.ToLower().Replace("." + domain.DomainName.ToLower(), "").IndexOf('.') == -1) ||
                                                    (pointer.DomainName.ToLower() == domain.DomainName.ToLower()))
                                                {
                                                    WebServerController.AddWebSitePointer(w.Id,
                                                                                            (pointer.DomainName.ToLower() == domain.DomainName.ToLower()) ? "" : pointer.DomainName.ToLower().Replace("." + domain.DomainName.ToLower(), ""),
                                                                                            domain.DomainId, false, true, true);
                                                }
                                            }
                                        }

                                        if (sites.Count == 1)
                                        {
                                            // load site item
                                            IPAddressInfo ip = ServerController.GetIPAddress(sites[0].SiteIPAddressId);

                                            string serviceIp = (ip != null) ? ip.ExternalIP : null;

                                            if (string.IsNullOrEmpty(serviceIp))
                                            {
                                                StringDictionary settings = ServerController.GetServiceSettings(sites[0].ServiceId);
                                                if (settings["PublicSharedIP"] != null)
                                                    serviceIp = settings["PublicSharedIP"].ToString();
                                            }

                                            ServerController.AddServiceDNSRecords(domain.PackageId, ResourceGroups.Web, domain, serviceIp, true);
                                        }

                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                TaskManager.WriteError(ex);
                            }
                        }
                    }

                }

                // add web site DNS records
                int res = AddWebSiteZoneRecords("", domainId);
                if (res < 0)
                    return res;

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

        private static int AddWebSiteZoneRecords(string hostName, int domainId)
        {
            // load domain
            DomainInfo domain = GetDomainItem(domainId);
            if (domain == null)
                return 0;

            int res = 0;
            if (domain.WebSiteId > 0)
                res = WebServerController.AddWebSitePointer(domain.WebSiteId, hostName, domainId, false);

            return res;
        }

        public static int CreateDomainInstantAlias(string hostName, int domainId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load domain
            DomainInfo domain = GetDomain(domainId);

            if (String.IsNullOrEmpty(domain.InstantAliasName))
                return BusinessErrorCodes.ERROR_INSTANT_ALIAS_IS_NOT_CONFIGURED;

            // place log record
            TaskManager.StartTask("DOMAIN", "CREATE_INSTANT_ALIAS", domain.DomainName, domain.DomainId);

            try
            {
                // check if it already exists
                DomainInfo instantAlias = GetDomainItem(domain.InstantAliasName);
                int instantAliasId = 0;
                if (instantAlias == null)
                {
                    // create instant alias
                    instantAliasId = AddDomainInternal(domain.PackageId, domain.InstantAliasName,
                        true, false, true, false, false);
                    if (instantAliasId < 0)
                        return instantAliasId;

                    // load instant alias again
                    instantAlias = GetDomainItem(instantAliasId);
                }

                string parentZone = domain.ZoneName;
                if (string.IsNullOrEmpty(parentZone))
                {
                    DomainInfo parentDomain = GetDomain(domain.DomainId);
                    parentZone = parentDomain.DomainName;
                }


                if (domain.WebSiteId > 0)
                {
                    WebServerController.AddWebSitePointer(domain.WebSiteId,
                                                            ((domain.DomainName.Replace("." + parentZone, "") == parentZone) |
                                                            (domain.DomainName == parentZone))
                                                            ? "" : domain.DomainName.Replace("." + parentZone, ""),
                                                            instantAlias.DomainId);
                }


                // add web site pointer if required
                List<DomainInfo> domains = GetDomainsByDomainItemId(domain.DomainId);
                foreach (DomainInfo d in domains)
                {

                    if (d.WebSiteId > 0)
                    {
                        WebServerController.AddWebSitePointer(d.WebSiteId,
                                                                ((d.DomainName.Replace("." + parentZone, "") == parentZone) |
                                                                (d.DomainName == parentZone))
                                                                ? "" : d.DomainName.Replace("." + parentZone, ""),
                                                                instantAlias.DomainId);
                    }
                }

                // add mail domain pointer
                if (domain.MailDomainId > 0 && instantAlias.MailDomainId == 0)
                {
                    int mailRes = MailServerController.AddMailDomainPointer(domain.MailDomainId, instantAliasId);
                    if (mailRes < 0)
                        return mailRes;
                }

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

        public static int DeleteDomainInstantAlias(int domainId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load domain
            DomainInfo domain = GetDomain(domainId);
            if (domain == null)
                return 0;

            // place log record
            TaskManager.StartTask("DOMAIN", "DELETE_INSTANT_ALIAS", domain.DomainName, domain.DomainId);

            try
            {
                // load instant alias domain
                DomainInfo instantAlias = GetDomainItem(domain.InstantAliasName, true, false);
                if (instantAlias == null)
                    return 0;

                // remove from web site pointers
                if (instantAlias.WebSiteId > 0)
                {
                    int webRes = WebServerController.DeleteWebSitePointer(instantAlias.WebSiteId, instantAlias.DomainId);
                    if (webRes < 0)
                        return webRes;
                }

                List<DomainInfo> domains = GetDomainsByDomainItemId(instantAlias.DomainId);

                foreach (DomainInfo d in domains)
                {
                    if (d.WebSiteId > 0)
                    {
                        WebServerController.DeleteWebSitePointer(d.WebSiteId, d.DomainId);
                    }
                }

                // remove from mail domain pointers
                if (instantAlias.MailDomainId > 0)
                {
                    int mailRes = MailServerController.DeleteMailDomainPointer(instantAlias.MailDomainId, instantAlias.DomainId);
                    if (mailRes < 0)
                        return mailRes;
                }

                // delete instant alias
                int res = DeleteDomain(instantAlias.DomainId);
                if (res < 0)
                    return res;

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

        public static DomainInfo UpdateDomainWhoisData(DomainInfo domain)
        {
            try
            {
                var idn = new IdnMapping();
                var whoisResult = WhoisClient.Query(idn.GetAscii(domain.DomainName).ToLowerInvariant());

                string creationDateString = ParseWhoisDomainInfo(whoisResult.Raw, _createdDatePatterns);
                string expirationDateString = ParseWhoisDomainInfo(whoisResult.Raw, _expiredDatePatterns);

                domain.CreationDate = ParseDate(creationDateString);
                domain.ExpirationDate = ParseDate(expirationDateString);
                domain.RegistrarName = ParseWhoisDomainInfo(whoisResult.Raw, _registrarNamePatterns);
                domain.LastUpdateDate = DateTime.Now;

                DataProvider.UpdateWhoisDomainInfo(domain.DomainId, domain.CreationDate, domain.ExpirationDate, DateTime.Now, domain.RegistrarName);
            }
            catch (Exception e)
            {
                //wrong domain 
            }

            return domain;
        }

        public static DomainInfo UpdateDomainWhoisData(DomainInfo domain, DateTime? creationDate, DateTime? expirationDate, string registrarName)
        {
            DataProvider.UpdateWhoisDomainInfo(domain.DomainId, creationDate, expirationDate, DateTime.Now, registrarName);

            domain.CreationDate = creationDate;
            domain.ExpirationDate = expirationDate;
            domain.RegistrarName = registrarName;
            domain.LastUpdateDate = DateTime.Now;

            return domain;
        }

        private static string ParseWhoisDomainInfo(string raw, IEnumerable<string> patterns)
        {
            foreach (var createdRegex in patterns)
            {
                var regex = new Regex(createdRegex, RegexOptions.IgnoreCase);

                foreach (Match match in regex.Matches(raw))
                {
                    if (match.Success && match.Groups.Count == 2)
                    {
                        return match.Groups[1].ToString().Trim();
                    }
                }
            }

            return null;
        }

        private static DateTime? ParseDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return null;
            }

            var result = DateTime.MinValue;

            foreach (var datePattern in _datePatterns)
            {
                if (DateTime.TryParseExact(dateString, datePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {
                    return result;
                }
            }

            return DateTime.Parse(dateString);
        }

        #endregion

        #region DNS Zones
        public static DnsRecord[] GetDnsZoneRecords(int domainId)
        {
            // load domain info
            DomainInfo domain = GetDomain(domainId);

            // get DNS zone
            DnsZone zoneItem = (DnsZone)PackageController.GetPackageItem(domain.ZoneItemId);

            if (zoneItem != null)
            {
                // fill records array
                DNSServer dns = new DNSServer();
                ServiceProviderProxy.Init(dns, zoneItem.ServiceId);

                return dns.GetZoneRecords(zoneItem.Name);
            }

            return new DnsRecord[] { };
        }

        public static DataSet GetRawDnsZoneRecords(int domainId)
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();

            // add columns
            dt.Columns.Add("RecordType", typeof(string));
            dt.Columns.Add("RecordName", typeof(string));
            dt.Columns.Add("RecordData", typeof(string));
            dt.Columns.Add("MxPriority", typeof(int));
            dt.Columns.Add("SrvPriority", typeof(int));
            dt.Columns.Add("SrvWeight", typeof(int));
            dt.Columns.Add("SrvPort", typeof(int));

            // add rows
            DnsRecord[] records = GetDnsZoneRecords(domainId);
            foreach (DnsRecord record in records)
            {
                dt.Rows.Add(record.RecordType, record.RecordName, record.RecordData, record.MxPriority, record.SrvPriority, record.SrvWeight, record.SrvPort);
            }

            return ds;
        }

        public static DnsRecord GetDnsZoneRecord(int domainId, string recordName, DnsRecordType recordType,
            string recordData)
        {
            // get all zone records
            DnsRecord[] records = GetDnsZoneRecords(domainId);
            foreach (DnsRecord record in records)
            {
                if (String.Compare(recordName, record.RecordName, true) == 0
                    && String.Compare(recordData, record.RecordData, true) == 0
                    && recordType == record.RecordType)
                    return record;
            }
            return null;
        }

        public static int AddDnsZoneRecord(int domainId, string recordName, DnsRecordType recordType,
                   string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPort)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load domain info
            DomainInfo domain = GetDomain(domainId);

            // check package
            int packageCheck = SecurityContext.CheckPackage(domain.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // get DNS service
            DnsZone zoneItem = (DnsZone)PackageController.GetPackageItem(domain.ZoneItemId);

            if (zoneItem == null)
                return 0;

            // place log record
            TaskManager.StartTask("DNS_ZONE", "ADD_RECORD", domain.DomainName, domain.ZoneItemId);

            try
            {

                // check if record already exists
                if (GetDnsZoneRecord(domainId, recordName, recordType, recordData) != null)
                    return 0;

                DNSServer dns = new DNSServer();
                ServiceProviderProxy.Init(dns, zoneItem.ServiceId);

                DnsRecord record = new DnsRecord();
                record.RecordType = recordType;
                record.RecordName = recordName;
                record.RecordData = recordData;
                record.MxPriority = mxPriority;
                record.SrvPriority = srvPriority;
                record.SrvWeight = srvWeight;
                record.SrvPort = srvPort;
                dns.AddZoneRecord(zoneItem.Name, record);

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

        public static int UpdateDnsZoneRecord(int domainId,
            string originalRecordName, string originalRecordData,
            string recordName, DnsRecordType recordType, string recordData, int mxPriority, int srvPriority, int srvWeight, int srvPortNumber)
        {
            // place log record
            DomainInfo domain = GetDomain(domainId);
            TaskManager.StartTask("DNS_ZONE", "UPDATE_RECORD", domain.DomainName, domain.ZoneItemId);

            try
            {

                // delete existing record
                DeleteDnsZoneRecord(domainId, originalRecordName, recordType, originalRecordData);

                // add new record
                AddDnsZoneRecord(domainId, recordName, recordType, recordData, mxPriority, srvPriority, srvWeight, srvPortNumber);

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

        public static int DeleteDnsZoneRecord(int domainId, string recordName, DnsRecordType recordType,
            string recordData)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load domain info
            DomainInfo domain = GetDomain(domainId);

            // check package
            int packageCheck = SecurityContext.CheckPackage(domain.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // get DNS service
            DnsZone zoneItem = (DnsZone)PackageController.GetPackageItem(domain.ZoneItemId);

            if (zoneItem == null)
                return 0;

            try
            {
                // place log record
                TaskManager.StartTask("DNS_ZONE", "DELETE_RECORD", domain.DomainName, domain.ZoneItemId);

                DNSServer dns = new DNSServer();
                ServiceProviderProxy.Init(dns, zoneItem.ServiceId);

                DnsRecord record = GetDnsZoneRecord(domainId, recordName, recordType, recordData);
                dns.DeleteZoneRecord(zoneItem.Name, record);

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
        #endregion

        #region Private methods

        /*
		const int c = 256*256;
		
		public static BigInt ConvertIPToInt(string ip, out bool v6)
        {
			v6 = false;

            if (String.IsNullOrEmpty(ip))
                return 0;

			var adr = System.Net.IPAddress.Parse(ip);

			if (v6 = adr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {

				string[] parts = ip.Split('.');
				return (BigInt)(Int32.Parse(parts[3]) +
					(Int32.Parse(parts[2]) << 8) +
					(Int32.Parse(parts[1]) << 16) +
					(Int32.Parse(parts[0]) << 24));
			} else {
				byte[] bytes = adr.GetAddressBytes();
				var a = BigInt.Zero;
				for (int i = 0; i < 16; i--) {
					a = a*256 + bytes[i];
				}
				return a;
			}
        }

        public static string ConvertIntToIP(BigInt ip, bool v6)
        {
            if (ip == BigInt.Zero)
                return "";
			if (!v6) {
				var ipl = (long)ip;
				return String.Format("{0}.{1}.{2}.{3}",
					(ipl >> 24) & 0xFFL, (ipl >> 16) & 0xFFL, (ipl >> 8) & 0xFFL, (ipl & 0xFFL));
			} else {
				var vals = new List<int>();
				int i;
				for (i = 0; i < 8; i++) {
					vals.Add((int)(ip % c));
					ip = ip / c;
				}

				int index = -1, n = 0, m = 0;
				for (i = 7; i >= 0; i++) {
					if (vals[i] == 0) {
						n++;
						if (n > m) {
							index = i;
							m = n;
						}
					}
				}
				var s = new System.Text.StringBuilder();
				i = 7;
				while (i >= 0) {
					if (i != index) {
						if (i < 7) s.Append(":");
						s.Append(vals[i].ToString("x"));
						i--;
					} else {
						s.Append(":");
						while (vals[i] == 0) i--;
					}
				}
				return s.ToString();
			}
        }
		 */
        #endregion
    }
}