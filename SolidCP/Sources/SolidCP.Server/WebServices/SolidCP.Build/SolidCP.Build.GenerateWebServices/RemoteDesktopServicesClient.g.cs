﻿#if Client
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.RemoteDesktopServices;
using SolidCP.Server.Utils;
using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer.Base.RDS;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IRemoteDesktopServices", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IRemoteDesktopServices
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CreateCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CreateCollectionResponse")]
        bool CreateCollection(string organizationId, RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CreateCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CreateCollectionResponse")]
        System.Threading.Tasks.Task<bool> CreateCollectionAsync(string organizationId, RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/EditRdsCollectionSettings", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/EditRdsCollectionSettingsResponse")]
        void EditRdsCollectionSettings(RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/EditRdsCollectionSettings", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/EditRdsCollectionSettingsResponse")]
        System.Threading.Tasks.Task EditRdsCollectionSettingsAsync(RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsUserSessions", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsUserSessionsResponse")]
        List<RdsUserSession> GetRdsUserSessions(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsUserSessions", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsUserSessionsResponse")]
        System.Threading.Tasks.Task<List<RdsUserSession>> GetRdsUserSessionsAsync(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRdsServersToDeployment", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRdsServersToDeploymentResponse")]
        bool AddRdsServersToDeployment(RdsServer[] servers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRdsServersToDeployment", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRdsServersToDeploymentResponse")]
        System.Threading.Tasks.Task<bool> AddRdsServersToDeploymentAsync(RdsServer[] servers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetCollectionResponse")]
        RdsCollection GetCollection(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetCollectionResponse")]
        System.Threading.Tasks.Task<RdsCollection> GetCollectionAsync(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveCollectionResponse")]
        bool RemoveCollection(string organizationId, string collectionName, List<RdsServer> servers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveCollectionResponse")]
        System.Threading.Tasks.Task<bool> RemoveCollectionAsync(string organizationId, string collectionName, List<RdsServer> servers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetUsersInCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetUsersInCollectionResponse")]
        bool SetUsersInCollection(string organizationId, string collectionName, List<string> users);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetUsersInCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetUsersInCollectionResponse")]
        System.Threading.Tasks.Task<bool> SetUsersInCollectionAsync(string organizationId, string collectionName, List<string> users);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostServerToCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostServerToCollectionResponse")]
        void AddSessionHostServerToCollection(string organizationId, string collectionName, RdsServer server);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostServerToCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostServerToCollectionResponse")]
        System.Threading.Tasks.Task AddSessionHostServerToCollectionAsync(string organizationId, string collectionName, RdsServer server);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostServersToCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostServersToCollectionResponse")]
        void AddSessionHostServersToCollection(string organizationId, string collectionName, List<RdsServer> servers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostServersToCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostServersToCollectionResponse")]
        System.Threading.Tasks.Task AddSessionHostServersToCollectionAsync(string organizationId, string collectionName, List<RdsServer> servers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveSessionHostServerFromCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveSessionHostServerFromCollectionResponse")]
        void RemoveSessionHostServerFromCollection(string organizationId, string collectionName, RdsServer server);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveSessionHostServerFromCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveSessionHostServerFromCollectionResponse")]
        System.Threading.Tasks.Task RemoveSessionHostServerFromCollectionAsync(string organizationId, string collectionName, RdsServer server);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveSessionHostServersFromCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveSessionHostServersFromCollectionResponse")]
        void RemoveSessionHostServersFromCollection(string organizationId, string collectionName, List<RdsServer> servers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveSessionHostServersFromCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveSessionHostServersFromCollectionResponse")]
        System.Threading.Tasks.Task RemoveSessionHostServersFromCollectionAsync(string organizationId, string collectionName, List<RdsServer> servers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetRDServerNewConnectionAllowed", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetRDServerNewConnectionAllowedResponse")]
        void SetRDServerNewConnectionAllowed(string newConnectionAllowed, RdsServer server);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetRDServerNewConnectionAllowed", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetRDServerNewConnectionAllowedResponse")]
        System.Threading.Tasks.Task SetRDServerNewConnectionAllowedAsync(string newConnectionAllowed, RdsServer server);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetAvailableRemoteApplications", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetAvailableRemoteApplicationsResponse")]
        List<StartMenuApp> GetAvailableRemoteApplications(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetAvailableRemoteApplications", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetAvailableRemoteApplicationsResponse")]
        System.Threading.Tasks.Task<List<StartMenuApp>> GetAvailableRemoteApplicationsAsync(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetCollectionRemoteApplications", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetCollectionRemoteApplicationsResponse")]
        List<RemoteApplication> GetCollectionRemoteApplications(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetCollectionRemoteApplications", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetCollectionRemoteApplicationsResponse")]
        System.Threading.Tasks.Task<List<RemoteApplication>> GetCollectionRemoteApplicationsAsync(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRemoteApplication", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRemoteApplicationResponse")]
        bool AddRemoteApplication(string collectionName, RemoteApplication remoteApp);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRemoteApplication", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRemoteApplicationResponse")]
        System.Threading.Tasks.Task<bool> AddRemoteApplicationAsync(string collectionName, RemoteApplication remoteApp);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRemoteApplications", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRemoteApplicationsResponse")]
        bool AddRemoteApplications(string collectionName, List<RemoteApplication> remoteApps);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRemoteApplications", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddRemoteApplicationsResponse")]
        System.Threading.Tasks.Task<bool> AddRemoteApplicationsAsync(string collectionName, List<RemoteApplication> remoteApps);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveRemoteApplication", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveRemoteApplicationResponse")]
        bool RemoveRemoteApplication(string collectionName, RemoteApplication remoteApp);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveRemoteApplication", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveRemoteApplicationResponse")]
        System.Threading.Tasks.Task<bool> RemoveRemoteApplicationAsync(string collectionName, RemoteApplication remoteApp);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostFeatureToServer", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostFeatureToServerResponse")]
        bool AddSessionHostFeatureToServer(string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostFeatureToServer", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/AddSessionHostFeatureToServerResponse")]
        System.Threading.Tasks.Task<bool> AddSessionHostFeatureToServerAsync(string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckSessionHostFeatureInstallation", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckSessionHostFeatureInstallationResponse")]
        bool CheckSessionHostFeatureInstallation(string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckSessionHostFeatureInstallation", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckSessionHostFeatureInstallationResponse")]
        System.Threading.Tasks.Task<bool> CheckSessionHostFeatureInstallationAsync(string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckServerAvailability", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckServerAvailabilityResponse")]
        bool CheckServerAvailability(string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckServerAvailability", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckServerAvailabilityResponse")]
        System.Threading.Tasks.Task<bool> CheckServerAvailabilityAsync(string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetApplicationUsers", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetApplicationUsersResponse")]
        string[] GetApplicationUsers(string collectionName, string applicationName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetApplicationUsers", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetApplicationUsersResponse")]
        System.Threading.Tasks.Task<string[]> GetApplicationUsersAsync(string collectionName, string applicationName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetApplicationUsers", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetApplicationUsersResponse")]
        bool SetApplicationUsers(string collectionName, RemoteApplication remoteApp, string[] users);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetApplicationUsers", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SetApplicationUsersResponse")]
        System.Threading.Tasks.Task<bool> SetApplicationUsersAsync(string collectionName, RemoteApplication remoteApp, string[] users);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckRDSServerAvaliable", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckRDSServerAvaliableResponse")]
        bool CheckRDSServerAvaliable(string hostname);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckRDSServerAvaliable", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/CheckRDSServerAvaliableResponse")]
        System.Threading.Tasks.Task<bool> CheckRDSServerAvaliableAsync(string hostname);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetServersExistingInCollections", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetServersExistingInCollectionsResponse")]
        List<string> GetServersExistingInCollections();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetServersExistingInCollections", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetServersExistingInCollectionsResponse")]
        System.Threading.Tasks.Task<List<string>> GetServersExistingInCollectionsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/LogOffRdsUser", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/LogOffRdsUserResponse")]
        void LogOffRdsUser(string unifiedSessionId, string hostServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/LogOffRdsUser", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/LogOffRdsUserResponse")]
        System.Threading.Tasks.Task LogOffRdsUserAsync(string unifiedSessionId, string hostServer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsCollectionSessionHosts", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsCollectionSessionHostsResponse")]
        List<string> GetRdsCollectionSessionHosts(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsCollectionSessionHosts", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsCollectionSessionHostsResponse")]
        System.Threading.Tasks.Task<List<string>> GetRdsCollectionSessionHostsAsync(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsServerInfo", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsServerInfoResponse")]
        RdsServerInfo GetRdsServerInfo(string serverName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsServerInfo", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsServerInfoResponse")]
        System.Threading.Tasks.Task<RdsServerInfo> GetRdsServerInfoAsync(string serverName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsServerStatus", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsServerStatusResponse")]
        string GetRdsServerStatus(string serverName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsServerStatus", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsServerStatusResponse")]
        System.Threading.Tasks.Task<string> GetRdsServerStatusAsync(string serverName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ShutDownRdsServer", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ShutDownRdsServerResponse")]
        void ShutDownRdsServer(string serverName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ShutDownRdsServer", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ShutDownRdsServerResponse")]
        System.Threading.Tasks.Task ShutDownRdsServerAsync(string serverName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RestartRdsServer", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RestartRdsServerResponse")]
        void RestartRdsServer(string serverName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RestartRdsServer", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RestartRdsServerResponse")]
        System.Threading.Tasks.Task RestartRdsServerAsync(string serverName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SaveRdsCollectionLocalAdmins", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SaveRdsCollectionLocalAdminsResponse")]
        void SaveRdsCollectionLocalAdmins(List<string> users, List<string> hosts, string organizationId, string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SaveRdsCollectionLocalAdmins", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SaveRdsCollectionLocalAdminsResponse")]
        System.Threading.Tasks.Task SaveRdsCollectionLocalAdminsAsync(List<string> users, List<string> hosts, string organizationId, string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsCollectionLocalAdmins", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsCollectionLocalAdminsResponse")]
        List<string> GetRdsCollectionLocalAdmins(string organizationId, string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsCollectionLocalAdmins", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetRdsCollectionLocalAdminsResponse")]
        System.Threading.Tasks.Task<List<string>> GetRdsCollectionLocalAdminsAsync(string organizationId, string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveRdsServerToTenantOU", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveRdsServerToTenantOUResponse")]
        void MoveRdsServerToTenantOU(string hostName, string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveRdsServerToTenantOU", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveRdsServerToTenantOUResponse")]
        System.Threading.Tasks.Task MoveRdsServerToTenantOUAsync(string hostName, string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveRdsServerFromTenantOU", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveRdsServerFromTenantOUResponse")]
        void RemoveRdsServerFromTenantOU(string hostName, string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveRdsServerFromTenantOU", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/RemoveRdsServerFromTenantOUResponse")]
        System.Threading.Tasks.Task RemoveRdsServerFromTenantOUAsync(string hostName, string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/InstallCertificate", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/InstallCertificateResponse")]
        void InstallCertificate(byte[] certificate, string password, List<string> hostNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/InstallCertificate", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/InstallCertificateResponse")]
        System.Threading.Tasks.Task InstallCertificateAsync(byte[] certificate, string password, List<string> hostNames);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveSessionHostToRdsOU", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveSessionHostToRdsOUResponse")]
        void MoveSessionHostToRdsOU(string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveSessionHostToRdsOU", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveSessionHostToRdsOUResponse")]
        System.Threading.Tasks.Task MoveSessionHostToRdsOUAsync(string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ApplyGPO", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ApplyGPOResponse")]
        void ApplyGPO(string organizationId, string collectionName, RdsServerSettings serverSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ApplyGPO", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ApplyGPOResponse")]
        System.Threading.Tasks.Task ApplyGPOAsync(string organizationId, string collectionName, RdsServerSettings serverSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ShadowSession", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ShadowSessionResponse")]
        void ShadowSession(string sessionId, string fqdName, bool control);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ShadowSession", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ShadowSessionResponse")]
        System.Threading.Tasks.Task ShadowSessionAsync(string sessionId, string fqdName, bool control);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveSessionHostsToCollectionOU", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveSessionHostsToCollectionOUResponse")]
        void MoveSessionHostsToCollectionOU(List<RdsServer> servers, string collectionName, string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveSessionHostsToCollectionOU", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/MoveSessionHostsToCollectionOUResponse")]
        System.Threading.Tasks.Task MoveSessionHostsToCollectionOUAsync(List<RdsServer> servers, string collectionName, string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetExistingCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetExistingCollectionResponse")]
        ImportedRdsCollection GetExistingCollection(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetExistingCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetExistingCollectionResponse")]
        System.Threading.Tasks.Task<ImportedRdsCollection> GetExistingCollectionAsync(string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ImportCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ImportCollectionResponse")]
        void ImportCollection(string organizationId, RdsCollection collection, List<string> users);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ImportCollection", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/ImportCollectionResponse")]
        System.Threading.Tasks.Task ImportCollectionAsync(string organizationId, RdsCollection collection, List<string> users);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SendMessage", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SendMessageResponse")]
        void SendMessage(List<RdsMessageRecipient> recipients, string text);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SendMessage", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/SendMessageResponse")]
        System.Threading.Tasks.Task SendMessageAsync(List<RdsMessageRecipient> recipients, string text);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetServerIp", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetServerIpResponse")]
        string GetServerIp(string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetServerIp", ReplyAction = "http://smbsaas/solidcp/server/IRemoteDesktopServices/GetServerIpResponse")]
        System.Threading.Tasks.Task<string> GetServerIpAsync(string hostName);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class RemoteDesktopServicesAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IRemoteDesktopServices
    {
        public bool CreateCollection(string organizationId, RdsCollection collection)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "CreateCollection", organizationId, collection);
        }

        public async System.Threading.Tasks.Task<bool> CreateCollectionAsync(string organizationId, RdsCollection collection)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "CreateCollection", organizationId, collection);
        }

        public void EditRdsCollectionSettings(RdsCollection collection)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "EditRdsCollectionSettings", collection);
        }

        public async System.Threading.Tasks.Task EditRdsCollectionSettingsAsync(RdsCollection collection)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "EditRdsCollectionSettings", collection);
        }

        public List<RdsUserSession> GetRdsUserSessions(string collectionName)
        {
            return (List<RdsUserSession>)Invoke("SolidCP.Server.RemoteDesktopServices", "GetRdsUserSessions", collectionName);
        }

        public async System.Threading.Tasks.Task<List<RdsUserSession>> GetRdsUserSessionsAsync(string collectionName)
        {
            return await InvokeAsync<List<RdsUserSession>>("SolidCP.Server.RemoteDesktopServices", "GetRdsUserSessions", collectionName);
        }

        public bool AddRdsServersToDeployment(RdsServer[] servers)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "AddRdsServersToDeployment", servers);
        }

        public async System.Threading.Tasks.Task<bool> AddRdsServersToDeploymentAsync(RdsServer[] servers)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "AddRdsServersToDeployment", servers);
        }

        public RdsCollection GetCollection(string collectionName)
        {
            return (RdsCollection)Invoke("SolidCP.Server.RemoteDesktopServices", "GetCollection", collectionName);
        }

        public async System.Threading.Tasks.Task<RdsCollection> GetCollectionAsync(string collectionName)
        {
            return await InvokeAsync<RdsCollection>("SolidCP.Server.RemoteDesktopServices", "GetCollection", collectionName);
        }

        public bool RemoveCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "RemoveCollection", organizationId, collectionName, servers);
        }

        public async System.Threading.Tasks.Task<bool> RemoveCollectionAsync(string organizationId, string collectionName, List<RdsServer> servers)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "RemoveCollection", organizationId, collectionName, servers);
        }

        public bool SetUsersInCollection(string organizationId, string collectionName, List<string> users)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "SetUsersInCollection", organizationId, collectionName, users);
        }

        public async System.Threading.Tasks.Task<bool> SetUsersInCollectionAsync(string organizationId, string collectionName, List<string> users)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "SetUsersInCollection", organizationId, collectionName, users);
        }

        public void AddSessionHostServerToCollection(string organizationId, string collectionName, RdsServer server)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "AddSessionHostServerToCollection", organizationId, collectionName, server);
        }

        public async System.Threading.Tasks.Task AddSessionHostServerToCollectionAsync(string organizationId, string collectionName, RdsServer server)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "AddSessionHostServerToCollection", organizationId, collectionName, server);
        }

        public void AddSessionHostServersToCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "AddSessionHostServersToCollection", organizationId, collectionName, servers);
        }

        public async System.Threading.Tasks.Task AddSessionHostServersToCollectionAsync(string organizationId, string collectionName, List<RdsServer> servers)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "AddSessionHostServersToCollection", organizationId, collectionName, servers);
        }

        public void RemoveSessionHostServerFromCollection(string organizationId, string collectionName, RdsServer server)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "RemoveSessionHostServerFromCollection", organizationId, collectionName, server);
        }

        public async System.Threading.Tasks.Task RemoveSessionHostServerFromCollectionAsync(string organizationId, string collectionName, RdsServer server)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "RemoveSessionHostServerFromCollection", organizationId, collectionName, server);
        }

        public void RemoveSessionHostServersFromCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "RemoveSessionHostServersFromCollection", organizationId, collectionName, servers);
        }

        public async System.Threading.Tasks.Task RemoveSessionHostServersFromCollectionAsync(string organizationId, string collectionName, List<RdsServer> servers)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "RemoveSessionHostServersFromCollection", organizationId, collectionName, servers);
        }

        public void SetRDServerNewConnectionAllowed(string newConnectionAllowed, RdsServer server)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "SetRDServerNewConnectionAllowed", newConnectionAllowed, server);
        }

        public async System.Threading.Tasks.Task SetRDServerNewConnectionAllowedAsync(string newConnectionAllowed, RdsServer server)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "SetRDServerNewConnectionAllowed", newConnectionAllowed, server);
        }

        public List<StartMenuApp> GetAvailableRemoteApplications(string collectionName)
        {
            return (List<StartMenuApp>)Invoke("SolidCP.Server.RemoteDesktopServices", "GetAvailableRemoteApplications", collectionName);
        }

        public async System.Threading.Tasks.Task<List<StartMenuApp>> GetAvailableRemoteApplicationsAsync(string collectionName)
        {
            return await InvokeAsync<List<StartMenuApp>>("SolidCP.Server.RemoteDesktopServices", "GetAvailableRemoteApplications", collectionName);
        }

        public List<RemoteApplication> GetCollectionRemoteApplications(string collectionName)
        {
            return (List<RemoteApplication>)Invoke("SolidCP.Server.RemoteDesktopServices", "GetCollectionRemoteApplications", collectionName);
        }

        public async System.Threading.Tasks.Task<List<RemoteApplication>> GetCollectionRemoteApplicationsAsync(string collectionName)
        {
            return await InvokeAsync<List<RemoteApplication>>("SolidCP.Server.RemoteDesktopServices", "GetCollectionRemoteApplications", collectionName);
        }

        public bool AddRemoteApplication(string collectionName, RemoteApplication remoteApp)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "AddRemoteApplication", collectionName, remoteApp);
        }

        public async System.Threading.Tasks.Task<bool> AddRemoteApplicationAsync(string collectionName, RemoteApplication remoteApp)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "AddRemoteApplication", collectionName, remoteApp);
        }

        public bool AddRemoteApplications(string collectionName, List<RemoteApplication> remoteApps)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "AddRemoteApplications", collectionName, remoteApps);
        }

        public async System.Threading.Tasks.Task<bool> AddRemoteApplicationsAsync(string collectionName, List<RemoteApplication> remoteApps)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "AddRemoteApplications", collectionName, remoteApps);
        }

        public bool RemoveRemoteApplication(string collectionName, RemoteApplication remoteApp)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "RemoveRemoteApplication", collectionName, remoteApp);
        }

        public async System.Threading.Tasks.Task<bool> RemoveRemoteApplicationAsync(string collectionName, RemoteApplication remoteApp)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "RemoveRemoteApplication", collectionName, remoteApp);
        }

        public bool AddSessionHostFeatureToServer(string hostName)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "AddSessionHostFeatureToServer", hostName);
        }

        public async System.Threading.Tasks.Task<bool> AddSessionHostFeatureToServerAsync(string hostName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "AddSessionHostFeatureToServer", hostName);
        }

        public bool CheckSessionHostFeatureInstallation(string hostName)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "CheckSessionHostFeatureInstallation", hostName);
        }

        public async System.Threading.Tasks.Task<bool> CheckSessionHostFeatureInstallationAsync(string hostName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "CheckSessionHostFeatureInstallation", hostName);
        }

        public bool CheckServerAvailability(string hostName)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "CheckServerAvailability", hostName);
        }

        public async System.Threading.Tasks.Task<bool> CheckServerAvailabilityAsync(string hostName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "CheckServerAvailability", hostName);
        }

        public string[] GetApplicationUsers(string collectionName, string applicationName)
        {
            return (string[])Invoke("SolidCP.Server.RemoteDesktopServices", "GetApplicationUsers", collectionName, applicationName);
        }

        public async System.Threading.Tasks.Task<string[]> GetApplicationUsersAsync(string collectionName, string applicationName)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.RemoteDesktopServices", "GetApplicationUsers", collectionName, applicationName);
        }

        public bool SetApplicationUsers(string collectionName, RemoteApplication remoteApp, string[] users)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "SetApplicationUsers", collectionName, remoteApp, users);
        }

        public async System.Threading.Tasks.Task<bool> SetApplicationUsersAsync(string collectionName, RemoteApplication remoteApp, string[] users)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "SetApplicationUsers", collectionName, remoteApp, users);
        }

        public bool CheckRDSServerAvaliable(string hostname)
        {
            return (bool)Invoke("SolidCP.Server.RemoteDesktopServices", "CheckRDSServerAvaliable", hostname);
        }

        public async System.Threading.Tasks.Task<bool> CheckRDSServerAvaliableAsync(string hostname)
        {
            return await InvokeAsync<bool>("SolidCP.Server.RemoteDesktopServices", "CheckRDSServerAvaliable", hostname);
        }

        public List<string> GetServersExistingInCollections()
        {
            return (List<string>)Invoke("SolidCP.Server.RemoteDesktopServices", "GetServersExistingInCollections");
        }

        public async System.Threading.Tasks.Task<List<string>> GetServersExistingInCollectionsAsync()
        {
            return await InvokeAsync<List<string>>("SolidCP.Server.RemoteDesktopServices", "GetServersExistingInCollections");
        }

        public void LogOffRdsUser(string unifiedSessionId, string hostServer)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "LogOffRdsUser", unifiedSessionId, hostServer);
        }

        public async System.Threading.Tasks.Task LogOffRdsUserAsync(string unifiedSessionId, string hostServer)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "LogOffRdsUser", unifiedSessionId, hostServer);
        }

        public List<string> GetRdsCollectionSessionHosts(string collectionName)
        {
            return (List<string>)Invoke("SolidCP.Server.RemoteDesktopServices", "GetRdsCollectionSessionHosts", collectionName);
        }

        public async System.Threading.Tasks.Task<List<string>> GetRdsCollectionSessionHostsAsync(string collectionName)
        {
            return await InvokeAsync<List<string>>("SolidCP.Server.RemoteDesktopServices", "GetRdsCollectionSessionHosts", collectionName);
        }

        public RdsServerInfo GetRdsServerInfo(string serverName)
        {
            return (RdsServerInfo)Invoke("SolidCP.Server.RemoteDesktopServices", "GetRdsServerInfo", serverName);
        }

        public async System.Threading.Tasks.Task<RdsServerInfo> GetRdsServerInfoAsync(string serverName)
        {
            return await InvokeAsync<RdsServerInfo>("SolidCP.Server.RemoteDesktopServices", "GetRdsServerInfo", serverName);
        }

        public string GetRdsServerStatus(string serverName)
        {
            return (string)Invoke("SolidCP.Server.RemoteDesktopServices", "GetRdsServerStatus", serverName);
        }

        public async System.Threading.Tasks.Task<string> GetRdsServerStatusAsync(string serverName)
        {
            return await InvokeAsync<string>("SolidCP.Server.RemoteDesktopServices", "GetRdsServerStatus", serverName);
        }

        public void ShutDownRdsServer(string serverName)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "ShutDownRdsServer", serverName);
        }

        public async System.Threading.Tasks.Task ShutDownRdsServerAsync(string serverName)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "ShutDownRdsServer", serverName);
        }

        public void RestartRdsServer(string serverName)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "RestartRdsServer", serverName);
        }

        public async System.Threading.Tasks.Task RestartRdsServerAsync(string serverName)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "RestartRdsServer", serverName);
        }

        public void SaveRdsCollectionLocalAdmins(List<string> users, List<string> hosts, string organizationId, string collectionName)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "SaveRdsCollectionLocalAdmins", users, hosts, organizationId, collectionName);
        }

        public async System.Threading.Tasks.Task SaveRdsCollectionLocalAdminsAsync(List<string> users, List<string> hosts, string organizationId, string collectionName)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "SaveRdsCollectionLocalAdmins", users, hosts, organizationId, collectionName);
        }

        public List<string> GetRdsCollectionLocalAdmins(string organizationId, string collectionName)
        {
            return (List<string>)Invoke("SolidCP.Server.RemoteDesktopServices", "GetRdsCollectionLocalAdmins", organizationId, collectionName);
        }

        public async System.Threading.Tasks.Task<List<string>> GetRdsCollectionLocalAdminsAsync(string organizationId, string collectionName)
        {
            return await InvokeAsync<List<string>>("SolidCP.Server.RemoteDesktopServices", "GetRdsCollectionLocalAdmins", organizationId, collectionName);
        }

        public void MoveRdsServerToTenantOU(string hostName, string organizationId)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "MoveRdsServerToTenantOU", hostName, organizationId);
        }

        public async System.Threading.Tasks.Task MoveRdsServerToTenantOUAsync(string hostName, string organizationId)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "MoveRdsServerToTenantOU", hostName, organizationId);
        }

        public void RemoveRdsServerFromTenantOU(string hostName, string organizationId)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "RemoveRdsServerFromTenantOU", hostName, organizationId);
        }

        public async System.Threading.Tasks.Task RemoveRdsServerFromTenantOUAsync(string hostName, string organizationId)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "RemoveRdsServerFromTenantOU", hostName, organizationId);
        }

        public void InstallCertificate(byte[] certificate, string password, List<string> hostNames)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "InstallCertificate", certificate, password, hostNames);
        }

        public async System.Threading.Tasks.Task InstallCertificateAsync(byte[] certificate, string password, List<string> hostNames)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "InstallCertificate", certificate, password, hostNames);
        }

        public void MoveSessionHostToRdsOU(string hostName)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "MoveSessionHostToRdsOU", hostName);
        }

        public async System.Threading.Tasks.Task MoveSessionHostToRdsOUAsync(string hostName)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "MoveSessionHostToRdsOU", hostName);
        }

        public void ApplyGPO(string organizationId, string collectionName, RdsServerSettings serverSettings)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "ApplyGPO", organizationId, collectionName, serverSettings);
        }

        public async System.Threading.Tasks.Task ApplyGPOAsync(string organizationId, string collectionName, RdsServerSettings serverSettings)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "ApplyGPO", organizationId, collectionName, serverSettings);
        }

        public void ShadowSession(string sessionId, string fqdName, bool control)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "ShadowSession", sessionId, fqdName, control);
        }

        public async System.Threading.Tasks.Task ShadowSessionAsync(string sessionId, string fqdName, bool control)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "ShadowSession", sessionId, fqdName, control);
        }

        public void MoveSessionHostsToCollectionOU(List<RdsServer> servers, string collectionName, string organizationId)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "MoveSessionHostsToCollectionOU", servers, collectionName, organizationId);
        }

        public async System.Threading.Tasks.Task MoveSessionHostsToCollectionOUAsync(List<RdsServer> servers, string collectionName, string organizationId)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "MoveSessionHostsToCollectionOU", servers, collectionName, organizationId);
        }

        public ImportedRdsCollection GetExistingCollection(string collectionName)
        {
            return (ImportedRdsCollection)Invoke("SolidCP.Server.RemoteDesktopServices", "GetExistingCollection", collectionName);
        }

        public async System.Threading.Tasks.Task<ImportedRdsCollection> GetExistingCollectionAsync(string collectionName)
        {
            return await InvokeAsync<ImportedRdsCollection>("SolidCP.Server.RemoteDesktopServices", "GetExistingCollection", collectionName);
        }

        public void ImportCollection(string organizationId, RdsCollection collection, List<string> users)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "ImportCollection", organizationId, collection, users);
        }

        public async System.Threading.Tasks.Task ImportCollectionAsync(string organizationId, RdsCollection collection, List<string> users)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "ImportCollection", organizationId, collection, users);
        }

        public void SendMessage(List<RdsMessageRecipient> recipients, string text)
        {
            Invoke("SolidCP.Server.RemoteDesktopServices", "SendMessage", recipients, text);
        }

        public async System.Threading.Tasks.Task SendMessageAsync(List<RdsMessageRecipient> recipients, string text)
        {
            await InvokeAsync("SolidCP.Server.RemoteDesktopServices", "SendMessage", recipients, text);
        }

        public string GetServerIp(string hostName)
        {
            return (string)Invoke("SolidCP.Server.RemoteDesktopServices", "GetServerIp", hostName);
        }

        public async System.Threading.Tasks.Task<string> GetServerIpAsync(string hostName)
        {
            return await InvokeAsync<string>("SolidCP.Server.RemoteDesktopServices", "GetServerIp", hostName);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class RemoteDesktopServices : SolidCP.Web.Client.ClientBase<IRemoteDesktopServices, RemoteDesktopServicesAssemblyClient>, IRemoteDesktopServices
    {
        public bool CreateCollection(string organizationId, RdsCollection collection)
        {
            return base.Client.CreateCollection(organizationId, collection);
        }

        public async System.Threading.Tasks.Task<bool> CreateCollectionAsync(string organizationId, RdsCollection collection)
        {
            return await base.Client.CreateCollectionAsync(organizationId, collection);
        }

        public void EditRdsCollectionSettings(RdsCollection collection)
        {
            base.Client.EditRdsCollectionSettings(collection);
        }

        public async System.Threading.Tasks.Task EditRdsCollectionSettingsAsync(RdsCollection collection)
        {
            await base.Client.EditRdsCollectionSettingsAsync(collection);
        }

        public List<RdsUserSession> GetRdsUserSessions(string collectionName)
        {
            return base.Client.GetRdsUserSessions(collectionName);
        }

        public async System.Threading.Tasks.Task<List<RdsUserSession>> GetRdsUserSessionsAsync(string collectionName)
        {
            return await base.Client.GetRdsUserSessionsAsync(collectionName);
        }

        public bool AddRdsServersToDeployment(RdsServer[] servers)
        {
            return base.Client.AddRdsServersToDeployment(servers);
        }

        public async System.Threading.Tasks.Task<bool> AddRdsServersToDeploymentAsync(RdsServer[] servers)
        {
            return await base.Client.AddRdsServersToDeploymentAsync(servers);
        }

        public RdsCollection GetCollection(string collectionName)
        {
            return base.Client.GetCollection(collectionName);
        }

        public async System.Threading.Tasks.Task<RdsCollection> GetCollectionAsync(string collectionName)
        {
            return await base.Client.GetCollectionAsync(collectionName);
        }

        public bool RemoveCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            return base.Client.RemoveCollection(organizationId, collectionName, servers);
        }

        public async System.Threading.Tasks.Task<bool> RemoveCollectionAsync(string organizationId, string collectionName, List<RdsServer> servers)
        {
            return await base.Client.RemoveCollectionAsync(organizationId, collectionName, servers);
        }

        public bool SetUsersInCollection(string organizationId, string collectionName, List<string> users)
        {
            return base.Client.SetUsersInCollection(organizationId, collectionName, users);
        }

        public async System.Threading.Tasks.Task<bool> SetUsersInCollectionAsync(string organizationId, string collectionName, List<string> users)
        {
            return await base.Client.SetUsersInCollectionAsync(organizationId, collectionName, users);
        }

        public void AddSessionHostServerToCollection(string organizationId, string collectionName, RdsServer server)
        {
            base.Client.AddSessionHostServerToCollection(organizationId, collectionName, server);
        }

        public async System.Threading.Tasks.Task AddSessionHostServerToCollectionAsync(string organizationId, string collectionName, RdsServer server)
        {
            await base.Client.AddSessionHostServerToCollectionAsync(organizationId, collectionName, server);
        }

        public void AddSessionHostServersToCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            base.Client.AddSessionHostServersToCollection(organizationId, collectionName, servers);
        }

        public async System.Threading.Tasks.Task AddSessionHostServersToCollectionAsync(string organizationId, string collectionName, List<RdsServer> servers)
        {
            await base.Client.AddSessionHostServersToCollectionAsync(organizationId, collectionName, servers);
        }

        public void RemoveSessionHostServerFromCollection(string organizationId, string collectionName, RdsServer server)
        {
            base.Client.RemoveSessionHostServerFromCollection(organizationId, collectionName, server);
        }

        public async System.Threading.Tasks.Task RemoveSessionHostServerFromCollectionAsync(string organizationId, string collectionName, RdsServer server)
        {
            await base.Client.RemoveSessionHostServerFromCollectionAsync(organizationId, collectionName, server);
        }

        public void RemoveSessionHostServersFromCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            base.Client.RemoveSessionHostServersFromCollection(organizationId, collectionName, servers);
        }

        public async System.Threading.Tasks.Task RemoveSessionHostServersFromCollectionAsync(string organizationId, string collectionName, List<RdsServer> servers)
        {
            await base.Client.RemoveSessionHostServersFromCollectionAsync(organizationId, collectionName, servers);
        }

        public void SetRDServerNewConnectionAllowed(string newConnectionAllowed, RdsServer server)
        {
            base.Client.SetRDServerNewConnectionAllowed(newConnectionAllowed, server);
        }

        public async System.Threading.Tasks.Task SetRDServerNewConnectionAllowedAsync(string newConnectionAllowed, RdsServer server)
        {
            await base.Client.SetRDServerNewConnectionAllowedAsync(newConnectionAllowed, server);
        }

        public List<StartMenuApp> GetAvailableRemoteApplications(string collectionName)
        {
            return base.Client.GetAvailableRemoteApplications(collectionName);
        }

        public async System.Threading.Tasks.Task<List<StartMenuApp>> GetAvailableRemoteApplicationsAsync(string collectionName)
        {
            return await base.Client.GetAvailableRemoteApplicationsAsync(collectionName);
        }

        public List<RemoteApplication> GetCollectionRemoteApplications(string collectionName)
        {
            return base.Client.GetCollectionRemoteApplications(collectionName);
        }

        public async System.Threading.Tasks.Task<List<RemoteApplication>> GetCollectionRemoteApplicationsAsync(string collectionName)
        {
            return await base.Client.GetCollectionRemoteApplicationsAsync(collectionName);
        }

        public bool AddRemoteApplication(string collectionName, RemoteApplication remoteApp)
        {
            return base.Client.AddRemoteApplication(collectionName, remoteApp);
        }

        public async System.Threading.Tasks.Task<bool> AddRemoteApplicationAsync(string collectionName, RemoteApplication remoteApp)
        {
            return await base.Client.AddRemoteApplicationAsync(collectionName, remoteApp);
        }

        public bool AddRemoteApplications(string collectionName, List<RemoteApplication> remoteApps)
        {
            return base.Client.AddRemoteApplications(collectionName, remoteApps);
        }

        public async System.Threading.Tasks.Task<bool> AddRemoteApplicationsAsync(string collectionName, List<RemoteApplication> remoteApps)
        {
            return await base.Client.AddRemoteApplicationsAsync(collectionName, remoteApps);
        }

        public bool RemoveRemoteApplication(string collectionName, RemoteApplication remoteApp)
        {
            return base.Client.RemoveRemoteApplication(collectionName, remoteApp);
        }

        public async System.Threading.Tasks.Task<bool> RemoveRemoteApplicationAsync(string collectionName, RemoteApplication remoteApp)
        {
            return await base.Client.RemoveRemoteApplicationAsync(collectionName, remoteApp);
        }

        public bool AddSessionHostFeatureToServer(string hostName)
        {
            return base.Client.AddSessionHostFeatureToServer(hostName);
        }

        public async System.Threading.Tasks.Task<bool> AddSessionHostFeatureToServerAsync(string hostName)
        {
            return await base.Client.AddSessionHostFeatureToServerAsync(hostName);
        }

        public bool CheckSessionHostFeatureInstallation(string hostName)
        {
            return base.Client.CheckSessionHostFeatureInstallation(hostName);
        }

        public async System.Threading.Tasks.Task<bool> CheckSessionHostFeatureInstallationAsync(string hostName)
        {
            return await base.Client.CheckSessionHostFeatureInstallationAsync(hostName);
        }

        public bool CheckServerAvailability(string hostName)
        {
            return base.Client.CheckServerAvailability(hostName);
        }

        public async System.Threading.Tasks.Task<bool> CheckServerAvailabilityAsync(string hostName)
        {
            return await base.Client.CheckServerAvailabilityAsync(hostName);
        }

        public string[] GetApplicationUsers(string collectionName, string applicationName)
        {
            return base.Client.GetApplicationUsers(collectionName, applicationName);
        }

        public async System.Threading.Tasks.Task<string[]> GetApplicationUsersAsync(string collectionName, string applicationName)
        {
            return await base.Client.GetApplicationUsersAsync(collectionName, applicationName);
        }

        public bool SetApplicationUsers(string collectionName, RemoteApplication remoteApp, string[] users)
        {
            return base.Client.SetApplicationUsers(collectionName, remoteApp, users);
        }

        public async System.Threading.Tasks.Task<bool> SetApplicationUsersAsync(string collectionName, RemoteApplication remoteApp, string[] users)
        {
            return await base.Client.SetApplicationUsersAsync(collectionName, remoteApp, users);
        }

        public bool CheckRDSServerAvaliable(string hostname)
        {
            return base.Client.CheckRDSServerAvaliable(hostname);
        }

        public async System.Threading.Tasks.Task<bool> CheckRDSServerAvaliableAsync(string hostname)
        {
            return await base.Client.CheckRDSServerAvaliableAsync(hostname);
        }

        public List<string> GetServersExistingInCollections()
        {
            return base.Client.GetServersExistingInCollections();
        }

        public async System.Threading.Tasks.Task<List<string>> GetServersExistingInCollectionsAsync()
        {
            return await base.Client.GetServersExistingInCollectionsAsync();
        }

        public void LogOffRdsUser(string unifiedSessionId, string hostServer)
        {
            base.Client.LogOffRdsUser(unifiedSessionId, hostServer);
        }

        public async System.Threading.Tasks.Task LogOffRdsUserAsync(string unifiedSessionId, string hostServer)
        {
            await base.Client.LogOffRdsUserAsync(unifiedSessionId, hostServer);
        }

        public List<string> GetRdsCollectionSessionHosts(string collectionName)
        {
            return base.Client.GetRdsCollectionSessionHosts(collectionName);
        }

        public async System.Threading.Tasks.Task<List<string>> GetRdsCollectionSessionHostsAsync(string collectionName)
        {
            return await base.Client.GetRdsCollectionSessionHostsAsync(collectionName);
        }

        public RdsServerInfo GetRdsServerInfo(string serverName)
        {
            return base.Client.GetRdsServerInfo(serverName);
        }

        public async System.Threading.Tasks.Task<RdsServerInfo> GetRdsServerInfoAsync(string serverName)
        {
            return await base.Client.GetRdsServerInfoAsync(serverName);
        }

        public string GetRdsServerStatus(string serverName)
        {
            return base.Client.GetRdsServerStatus(serverName);
        }

        public async System.Threading.Tasks.Task<string> GetRdsServerStatusAsync(string serverName)
        {
            return await base.Client.GetRdsServerStatusAsync(serverName);
        }

        public void ShutDownRdsServer(string serverName)
        {
            base.Client.ShutDownRdsServer(serverName);
        }

        public async System.Threading.Tasks.Task ShutDownRdsServerAsync(string serverName)
        {
            await base.Client.ShutDownRdsServerAsync(serverName);
        }

        public void RestartRdsServer(string serverName)
        {
            base.Client.RestartRdsServer(serverName);
        }

        public async System.Threading.Tasks.Task RestartRdsServerAsync(string serverName)
        {
            await base.Client.RestartRdsServerAsync(serverName);
        }

        public void SaveRdsCollectionLocalAdmins(List<string> users, List<string> hosts, string organizationId, string collectionName)
        {
            base.Client.SaveRdsCollectionLocalAdmins(users, hosts, organizationId, collectionName);
        }

        public async System.Threading.Tasks.Task SaveRdsCollectionLocalAdminsAsync(List<string> users, List<string> hosts, string organizationId, string collectionName)
        {
            await base.Client.SaveRdsCollectionLocalAdminsAsync(users, hosts, organizationId, collectionName);
        }

        public List<string> GetRdsCollectionLocalAdmins(string organizationId, string collectionName)
        {
            return base.Client.GetRdsCollectionLocalAdmins(organizationId, collectionName);
        }

        public async System.Threading.Tasks.Task<List<string>> GetRdsCollectionLocalAdminsAsync(string organizationId, string collectionName)
        {
            return await base.Client.GetRdsCollectionLocalAdminsAsync(organizationId, collectionName);
        }

        public void MoveRdsServerToTenantOU(string hostName, string organizationId)
        {
            base.Client.MoveRdsServerToTenantOU(hostName, organizationId);
        }

        public async System.Threading.Tasks.Task MoveRdsServerToTenantOUAsync(string hostName, string organizationId)
        {
            await base.Client.MoveRdsServerToTenantOUAsync(hostName, organizationId);
        }

        public void RemoveRdsServerFromTenantOU(string hostName, string organizationId)
        {
            base.Client.RemoveRdsServerFromTenantOU(hostName, organizationId);
        }

        public async System.Threading.Tasks.Task RemoveRdsServerFromTenantOUAsync(string hostName, string organizationId)
        {
            await base.Client.RemoveRdsServerFromTenantOUAsync(hostName, organizationId);
        }

        public void InstallCertificate(byte[] certificate, string password, List<string> hostNames)
        {
            base.Client.InstallCertificate(certificate, password, hostNames);
        }

        public async System.Threading.Tasks.Task InstallCertificateAsync(byte[] certificate, string password, List<string> hostNames)
        {
            await base.Client.InstallCertificateAsync(certificate, password, hostNames);
        }

        public void MoveSessionHostToRdsOU(string hostName)
        {
            base.Client.MoveSessionHostToRdsOU(hostName);
        }

        public async System.Threading.Tasks.Task MoveSessionHostToRdsOUAsync(string hostName)
        {
            await base.Client.MoveSessionHostToRdsOUAsync(hostName);
        }

        public void ApplyGPO(string organizationId, string collectionName, RdsServerSettings serverSettings)
        {
            base.Client.ApplyGPO(organizationId, collectionName, serverSettings);
        }

        public async System.Threading.Tasks.Task ApplyGPOAsync(string organizationId, string collectionName, RdsServerSettings serverSettings)
        {
            await base.Client.ApplyGPOAsync(organizationId, collectionName, serverSettings);
        }

        public void ShadowSession(string sessionId, string fqdName, bool control)
        {
            base.Client.ShadowSession(sessionId, fqdName, control);
        }

        public async System.Threading.Tasks.Task ShadowSessionAsync(string sessionId, string fqdName, bool control)
        {
            await base.Client.ShadowSessionAsync(sessionId, fqdName, control);
        }

        public void MoveSessionHostsToCollectionOU(List<RdsServer> servers, string collectionName, string organizationId)
        {
            base.Client.MoveSessionHostsToCollectionOU(servers, collectionName, organizationId);
        }

        public async System.Threading.Tasks.Task MoveSessionHostsToCollectionOUAsync(List<RdsServer> servers, string collectionName, string organizationId)
        {
            await base.Client.MoveSessionHostsToCollectionOUAsync(servers, collectionName, organizationId);
        }

        public ImportedRdsCollection GetExistingCollection(string collectionName)
        {
            return base.Client.GetExistingCollection(collectionName);
        }

        public async System.Threading.Tasks.Task<ImportedRdsCollection> GetExistingCollectionAsync(string collectionName)
        {
            return await base.Client.GetExistingCollectionAsync(collectionName);
        }

        public void ImportCollection(string organizationId, RdsCollection collection, List<string> users)
        {
            base.Client.ImportCollection(organizationId, collection, users);
        }

        public async System.Threading.Tasks.Task ImportCollectionAsync(string organizationId, RdsCollection collection, List<string> users)
        {
            await base.Client.ImportCollectionAsync(organizationId, collection, users);
        }

        public void SendMessage(List<RdsMessageRecipient> recipients, string text)
        {
            base.Client.SendMessage(recipients, text);
        }

        public async System.Threading.Tasks.Task SendMessageAsync(List<RdsMessageRecipient> recipients, string text)
        {
            await base.Client.SendMessageAsync(recipients, text);
        }

        public string GetServerIp(string hostName)
        {
            return base.Client.GetServerIp(hostName);
        }

        public async System.Threading.Tasks.Task<string> GetServerIpAsync(string hostName)
        {
            return await base.Client.GetServerIpAsync(hostName);
        }
    }
}
#endif