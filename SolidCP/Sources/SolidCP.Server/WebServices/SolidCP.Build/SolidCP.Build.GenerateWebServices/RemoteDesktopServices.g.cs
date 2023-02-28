#if !Client
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

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
    public interface IRemoteDesktopServices
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CreateCollection(string organizationId, RdsCollection collection);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void EditRdsCollectionSettings(RdsCollection collection);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<RdsUserSession> GetRdsUserSessions(string collectionName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool AddRdsServersToDeployment(RdsServer[] servers);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        RdsCollection GetCollection(string collectionName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool RemoveCollection(string organizationId, string collectionName, List<RdsServer> servers);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool SetUsersInCollection(string organizationId, string collectionName, List<string> users);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void AddSessionHostServerToCollection(string organizationId, string collectionName, RdsServer server);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void AddSessionHostServersToCollection(string organizationId, string collectionName, List<RdsServer> servers);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RemoveSessionHostServerFromCollection(string organizationId, string collectionName, RdsServer server);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RemoveSessionHostServersFromCollection(string organizationId, string collectionName, List<RdsServer> servers);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetRDServerNewConnectionAllowed(string newConnectionAllowed, RdsServer server);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<StartMenuApp> GetAvailableRemoteApplications(string collectionName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<RemoteApplication> GetCollectionRemoteApplications(string collectionName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool AddRemoteApplication(string collectionName, RemoteApplication remoteApp);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool AddRemoteApplications(string collectionName, List<RemoteApplication> remoteApps);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool RemoveRemoteApplication(string collectionName, RemoteApplication remoteApp);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool AddSessionHostFeatureToServer(string hostName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckSessionHostFeatureInstallation(string hostName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckServerAvailability(string hostName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetApplicationUsers(string collectionName, string applicationName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool SetApplicationUsers(string collectionName, RemoteApplication remoteApp, string[] users);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckRDSServerAvaliable(string hostname);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<string> GetServersExistingInCollections();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void LogOffRdsUser(string unifiedSessionId, string hostServer);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<string> GetRdsCollectionSessionHosts(string collectionName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        RdsServerInfo GetRdsServerInfo(string serverName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string GetRdsServerStatus(string serverName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ShutDownRdsServer(string serverName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RestartRdsServer(string serverName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SaveRdsCollectionLocalAdmins(List<string> users, List<string> hosts, string organizationId, string collectionName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        List<string> GetRdsCollectionLocalAdmins(string organizationId, string collectionName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void MoveRdsServerToTenantOU(string hostName, string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RemoveRdsServerFromTenantOU(string hostName, string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void InstallCertificate(byte[] certificate, string password, List<string> hostNames);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void MoveSessionHostToRdsOU(string hostName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ApplyGPO(string organizationId, string collectionName, RdsServerSettings serverSettings);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ShadowSession(string sessionId, string fqdName, bool control);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void MoveSessionHostsToCollectionOU(List<RdsServer> servers, string collectionName, string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ImportedRdsCollection GetExistingCollection(string collectionName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ImportCollection(string organizationId, RdsCollection collection, List<string> users);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SendMessage(List<RdsMessageRecipient> recipients, string text);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string GetServerIp(string hostName);
    }

    // wcf service
    public class RemoteDesktopServicesService : RemoteDesktopServices, IRemoteDesktopServices
    {
        public new bool CreateCollection(string organizationId, RdsCollection collection)
        {
            return base.CreateCollection(organizationId, collection);
        }

        public new void EditRdsCollectionSettings(RdsCollection collection)
        {
            base.EditRdsCollectionSettings(collection);
        }

        public new List<RdsUserSession> GetRdsUserSessions(string collectionName)
        {
            return base.GetRdsUserSessions(collectionName);
        }

        public new bool AddRdsServersToDeployment(RdsServer[] servers)
        {
            return base.AddRdsServersToDeployment(servers);
        }

        public new RdsCollection GetCollection(string collectionName)
        {
            return base.GetCollection(collectionName);
        }

        public new bool RemoveCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            return base.RemoveCollection(organizationId, collectionName, servers);
        }

        public new bool SetUsersInCollection(string organizationId, string collectionName, List<string> users)
        {
            return base.SetUsersInCollection(organizationId, collectionName, users);
        }

        public new void AddSessionHostServerToCollection(string organizationId, string collectionName, RdsServer server)
        {
            base.AddSessionHostServerToCollection(organizationId, collectionName, server);
        }

        public new void AddSessionHostServersToCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            base.AddSessionHostServersToCollection(organizationId, collectionName, servers);
        }

        public new void RemoveSessionHostServerFromCollection(string organizationId, string collectionName, RdsServer server)
        {
            base.RemoveSessionHostServerFromCollection(organizationId, collectionName, server);
        }

        public new void RemoveSessionHostServersFromCollection(string organizationId, string collectionName, List<RdsServer> servers)
        {
            base.RemoveSessionHostServersFromCollection(organizationId, collectionName, servers);
        }

        public new void SetRDServerNewConnectionAllowed(string newConnectionAllowed, RdsServer server)
        {
            base.SetRDServerNewConnectionAllowed(newConnectionAllowed, server);
        }

        public new List<StartMenuApp> GetAvailableRemoteApplications(string collectionName)
        {
            return base.GetAvailableRemoteApplications(collectionName);
        }

        public new List<RemoteApplication> GetCollectionRemoteApplications(string collectionName)
        {
            return base.GetCollectionRemoteApplications(collectionName);
        }

        public new bool AddRemoteApplication(string collectionName, RemoteApplication remoteApp)
        {
            return base.AddRemoteApplication(collectionName, remoteApp);
        }

        public new bool AddRemoteApplications(string collectionName, List<RemoteApplication> remoteApps)
        {
            return base.AddRemoteApplications(collectionName, remoteApps);
        }

        public new bool RemoveRemoteApplication(string collectionName, RemoteApplication remoteApp)
        {
            return base.RemoveRemoteApplication(collectionName, remoteApp);
        }

        public new bool AddSessionHostFeatureToServer(string hostName)
        {
            return base.AddSessionHostFeatureToServer(hostName);
        }

        public new bool CheckSessionHostFeatureInstallation(string hostName)
        {
            return base.CheckSessionHostFeatureInstallation(hostName);
        }

        public new bool CheckServerAvailability(string hostName)
        {
            return base.CheckServerAvailability(hostName);
        }

        public new string[] GetApplicationUsers(string collectionName, string applicationName)
        {
            return base.GetApplicationUsers(collectionName, applicationName);
        }

        public new bool SetApplicationUsers(string collectionName, RemoteApplication remoteApp, string[] users)
        {
            return base.SetApplicationUsers(collectionName, remoteApp, users);
        }

        public new bool CheckRDSServerAvaliable(string hostname)
        {
            return base.CheckRDSServerAvaliable(hostname);
        }

        public new List<string> GetServersExistingInCollections()
        {
            return base.GetServersExistingInCollections();
        }

        public new void LogOffRdsUser(string unifiedSessionId, string hostServer)
        {
            base.LogOffRdsUser(unifiedSessionId, hostServer);
        }

        public new List<string> GetRdsCollectionSessionHosts(string collectionName)
        {
            return base.GetRdsCollectionSessionHosts(collectionName);
        }

        public new RdsServerInfo GetRdsServerInfo(string serverName)
        {
            return base.GetRdsServerInfo(serverName);
        }

        public new string GetRdsServerStatus(string serverName)
        {
            return base.GetRdsServerStatus(serverName);
        }

        public new void ShutDownRdsServer(string serverName)
        {
            base.ShutDownRdsServer(serverName);
        }

        public new void RestartRdsServer(string serverName)
        {
            base.RestartRdsServer(serverName);
        }

        public new void SaveRdsCollectionLocalAdmins(List<string> users, List<string> hosts, string organizationId, string collectionName)
        {
            base.SaveRdsCollectionLocalAdmins(users, hosts, organizationId, collectionName);
        }

        public new List<string> GetRdsCollectionLocalAdmins(string organizationId, string collectionName)
        {
            return base.GetRdsCollectionLocalAdmins(organizationId, collectionName);
        }

        public new void MoveRdsServerToTenantOU(string hostName, string organizationId)
        {
            base.MoveRdsServerToTenantOU(hostName, organizationId);
        }

        public new void RemoveRdsServerFromTenantOU(string hostName, string organizationId)
        {
            base.RemoveRdsServerFromTenantOU(hostName, organizationId);
        }

        public new void InstallCertificate(byte[] certificate, string password, List<string> hostNames)
        {
            base.InstallCertificate(certificate, password, hostNames);
        }

        public new void MoveSessionHostToRdsOU(string hostName)
        {
            base.MoveSessionHostToRdsOU(hostName);
        }

        public new void ApplyGPO(string organizationId, string collectionName, RdsServerSettings serverSettings)
        {
            base.ApplyGPO(organizationId, collectionName, serverSettings);
        }

        public new void ShadowSession(string sessionId, string fqdName, bool control)
        {
            base.ShadowSession(sessionId, fqdName, control);
        }

        public new void MoveSessionHostsToCollectionOU(List<RdsServer> servers, string collectionName, string organizationId)
        {
            base.MoveSessionHostsToCollectionOU(servers, collectionName, organizationId);
        }

        public new ImportedRdsCollection GetExistingCollection(string collectionName)
        {
            return base.GetExistingCollection(collectionName);
        }

        public new void ImportCollection(string organizationId, RdsCollection collection, List<string> users)
        {
            base.ImportCollection(organizationId, collection, users);
        }

        public new void SendMessage(List<RdsMessageRecipient> recipients, string text)
        {
            base.SendMessage(recipients, text);
        }

        public new string GetServerIp(string hostName)
        {
            return base.GetServerIp(hostName);
        }
    }
}
#endif