#if Client
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
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Client
{
    /// <summary>
    /// Summary description for RemoteDesktopServices
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
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

    public class RemoteDesktopServices
    {
        ChannelFactory<T> _Factory { get; set; }

        public Credentials Credentials { get; set; }

        public object SoapHeader { get; set; }

        void Test()
        {
            try
            {
                var client = _Factory.CreateChannel();
                client.MyServiceOperation();
                ((ICommunicationObject)client).Close();
                _Factory.Close();
            }
            catch
            {
                (client as ICommunicationObject)?.Abort();
            }
        }
    }
}
#endif