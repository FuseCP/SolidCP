#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.RemoteDesktopServices;
using SolidCP.EnterpriseServer.Base.RDS;
using SolidCP.EnterpriseServer;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.EnterpriseServer.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesRemoteDesktopServices
    {
        [WebMethod]
        [OperationContract]
        RdsCollection GetRdsCollection(int collectionId, bool quick);
        [WebMethod]
        [OperationContract]
        RdsCollectionSettings GetRdsCollectionSettings(int collectionId);
        [WebMethod]
        [OperationContract]
        List<RdsCollection> GetOrganizationRdsCollections(int itemId);
        [WebMethod]
        [OperationContract]
        int AddRdsCollection(int itemId, RdsCollection collection);
        [WebMethod]
        [OperationContract]
        ResultObject EditRdsCollection(int itemId, RdsCollection collection);
        [WebMethod]
        [OperationContract]
        ResultObject EditRdsCollectionSettings(int itemId, RdsCollection collection);
        [WebMethod]
        [OperationContract]
        RdsCollectionPaged GetRdsCollectionsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        ResultObject RemoveRdsCollection(int itemId, RdsCollection collection);
        [WebMethod]
        [OperationContract]
        RdsServersPaged GetRdsServersPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID);
        [WebMethod]
        [OperationContract]
        RdsServersPaged GetFreeRdsServersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string ServiceId);
        [WebMethod]
        [OperationContract]
        RdsServersPaged GetOrganizationRdsServersPaged(int itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID);
        [WebMethod]
        [OperationContract]
        RdsServersPaged GetOrganizationFreeRdsServersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID);
        [WebMethod]
        [OperationContract]
        RdsServer GetRdsServer(int rdsSeverId);
        [WebMethod]
        [OperationContract]
        ResultObject SetRDServerNewConnectionAllowed(int itemId, string newConnectionAllowed, int rdsSeverId);
        [WebMethod]
        [OperationContract]
        List<RdsServer> GetCollectionRdsServers(int collectionId);
        [WebMethod]
        [OperationContract]
        List<RdsServer> GetOrganizationRdsServers(int itemId);
        [WebMethod]
        [OperationContract]
        ResultObject AddRdsServer(RdsServer rdsServer, string rdsControllerServiceID);
        [WebMethod]
        [OperationContract]
        ResultObject AddRdsServerToCollection(int itemId, RdsServer rdsServer, RdsCollection rdsCollection);
        [WebMethod]
        [OperationContract]
        ResultObject AddRdsServerToOrganization(int itemId, int serverId);
        [WebMethod]
        [OperationContract]
        ResultObject RemoveRdsServer(int rdsServerId);
        [WebMethod]
        [OperationContract]
        ResultObject RemoveRdsServerFromCollection(int itemId, RdsServer rdsServer, RdsCollection rdsCollection);
        [WebMethod]
        [OperationContract]
        ResultObject RemoveRdsServerFromOrganization(int itemId, int rdsServerId);
        [WebMethod]
        [OperationContract]
        ResultObject UpdateRdsServer(RdsServer rdsServer);
        [WebMethod]
        [OperationContract]
        List<OrganizationUser> GetRdsCollectionUsers(int collectionId);
        [WebMethod]
        [OperationContract]
        ResultObject SetUsersToRdsCollection(int itemId, int collectionId, List<OrganizationUser> users);
        [WebMethod]
        [OperationContract]
        List<RemoteApplication> GetCollectionRemoteApplications(int itemId, string collectionName);
        [WebMethod]
        [OperationContract]
        List<StartMenuApp> GetAvailableRemoteApplications(int itemId, string collectionName);
        [WebMethod]
        [OperationContract]
        ResultObject AddRemoteApplicationToCollection(int itemId, RdsCollection collection, RemoteApplication application);
        [WebMethod]
        [OperationContract]
        ResultObject RemoveRemoteApplicationFromCollection(int itemId, RdsCollection collection, RemoteApplication application);
        [WebMethod]
        [OperationContract]
        ResultObject SetRemoteApplicationsToRdsCollection(int itemId, int collectionId, List<RemoteApplication> remoteApps);
        [WebMethod]
        [OperationContract]
        int GetOrganizationRdsUsersCount(int itemId);
        [WebMethod]
        [OperationContract]
        int GetOrganizationRdsServersCount(int itemId);
        [WebMethod]
        [OperationContract]
        int GetOrganizationRdsCollectionsCount(int itemId);
        [WebMethod]
        [OperationContract]
        List<string> GetApplicationUsers(int itemId, int collectionId, RemoteApplication remoteApp);
        [WebMethod]
        [OperationContract]
        ResultObject SetApplicationUsers(int itemId, int collectionId, RemoteApplication remoteApp, List<string> users);
        [WebMethod]
        [OperationContract]
        List<RdsUserSession> GetRdsUserSessions(int collectionId);
        [WebMethod]
        [OperationContract]
        ResultObject LogOffRdsUser(int itemId, string unifiedSessionId, string hostServer);
        [WebMethod]
        [OperationContract]
        List<string> GetRdsCollectionSessionHosts(int collectionId);
        [WebMethod]
        [OperationContract]
        RdsServerInfo GetRdsServerInfo(int? itemId, string fqdnName);
        [WebMethod]
        [OperationContract]
        string GetRdsServerStatus(int? itemId, string fqdnName);
        [WebMethod]
        [OperationContract]
        ResultObject ShutDownRdsServer(int? itemId, string fqdnName);
        [WebMethod]
        [OperationContract]
        ResultObject RestartRdsServer(int? itemId, string fqdnName);
        [WebMethod]
        [OperationContract]
        List<OrganizationUser> GetRdsCollectionLocalAdmins(int collectionId);
        [WebMethod]
        [OperationContract]
        ResultObject SaveRdsCollectionLocalAdmins(OrganizationUser[] users, int collectionId);
        [WebMethod]
        [OperationContract]
        ResultObject InstallSessionHostsCertificate(RdsServer rdsServer);
        [WebMethod]
        [OperationContract]
        RdsCertificate GetRdsCertificateByServiceId(int serviceId);
        [WebMethod]
        [OperationContract]
        RdsCertificate GetRdsCertificateByItemId(int? itemId);
        [WebMethod]
        [OperationContract]
        ResultObject AddRdsCertificate(RdsCertificate certificate);
        [WebMethod]
        [OperationContract]
        List<ServiceInfo> GetRdsServices();
        [WebMethod]
        [OperationContract]
        string GetRdsSetupLetter(int itemId, int? accountId);
        [WebMethod]
        [OperationContract]
        int SendRdsSetupLetter(int itemId, int? accountId, string to, string cc);
        [WebMethod]
        [OperationContract]
        RdsServerSettings GetRdsServerSettings(int serverId, string settingsName);
        [WebMethod]
        [OperationContract]
        int UpdateRdsServerSettings(int serverId, string settingsName, RdsServerSettings settings);
        [WebMethod]
        [OperationContract]
        ResultObject ShadowSession(int itemId, string sessionId, bool control, string fqdName);
        [WebMethod]
        [OperationContract]
        ResultObject ImportCollection(int itemId, string collectionName, string rdsControllerServiceID);
        [WebMethod]
        [OperationContract]
        int GetRemoteDesktopServiceId(int itemId);
        [WebMethod]
        [OperationContract]
        ResultObject SendMessage(RdsMessageRecipient[] recipients, string text, int itemId, int rdsCollectionId, string userName);
        [WebMethod]
        [OperationContract]
        List<RdsMessage> GetRdsMessagesByCollectionId(int rdsCollectionId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esRemoteDesktopServices : SolidCP.EnterpriseServer.esRemoteDesktopServices, IesRemoteDesktopServices
    {
    }
}
#endif