#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesRemoteDesktopServices", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesRemoteDesktopServices
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsCollection GetRdsCollection(int collectionId, bool quick);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollection> GetRdsCollectionAsync(int collectionId, bool quick);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionSettingsResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsCollectionSettings GetRdsCollectionSettings(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollectionSettings> GetRdsCollectionSettingsAsync(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsCollections", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsCollectionsResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsCollection[] /*List*/ GetOrganizationRdsCollections(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsCollections", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsCollectionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollection[]> GetOrganizationRdsCollectionsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsCollectionResponse")]
        int AddRdsCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsCollectionResponse")]
        System.Threading.Tasks.Task<int> AddRdsCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/EditRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/EditRdsCollectionResponse")]
        SolidCP.Providers.Common.ResultObject EditRdsCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/EditRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/EditRdsCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EditRdsCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/EditRdsCollectionSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/EditRdsCollectionSettingsResponse")]
        SolidCP.Providers.Common.ResultObject EditRdsCollectionSettings(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/EditRdsCollectionSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/EditRdsCollectionSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EditRdsCollectionSettingsAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionsPagedResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsCollectionPaged GetRdsCollectionsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionsPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollectionPaged> GetRdsCollectionsPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsCollectionResponse")]
        SolidCP.Providers.Common.ResultObject RemoveRdsCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServersPagedResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetRdsServersPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetRdsServersPagedAsync(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetFreeRdsServersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetFreeRdsServersPagedResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetFreeRdsServersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string ServiceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetFreeRdsServersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetFreeRdsServersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetFreeRdsServersPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string ServiceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServersPagedResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetOrganizationRdsServersPaged(int itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetOrganizationRdsServersPagedAsync(int itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationFreeRdsServersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationFreeRdsServersPagedResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetOrganizationFreeRdsServersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationFreeRdsServersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationFreeRdsServersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetOrganizationFreeRdsServersPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsServer GetRdsServer(int rdsSeverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServer> GetRdsServerAsync(int rdsSeverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetRDServerNewConnectionAllowed", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetRDServerNewConnectionAllowedResponse")]
        SolidCP.Providers.Common.ResultObject SetRDServerNewConnectionAllowed(int itemId, string newConnectionAllowed, int rdsSeverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetRDServerNewConnectionAllowed", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetRDServerNewConnectionAllowedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRDServerNewConnectionAllowedAsync(int itemId, string newConnectionAllowed, int rdsSeverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetCollectionRdsServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetCollectionRdsServersResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsServer[] /*List*/ GetCollectionRdsServers(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetCollectionRdsServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetCollectionRdsServersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServer[]> GetCollectionRdsServersAsync(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServersResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsServer[] /*List*/ GetOrganizationRdsServers(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServer[]> GetOrganizationRdsServersAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServerResponse")]
        SolidCP.Providers.Common.ResultObject AddRdsServer(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, string rdsControllerServiceID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsServerAsync(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, string rdsControllerServiceID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServerToCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServerToCollectionResponse")]
        SolidCP.Providers.Common.ResultObject AddRdsServerToCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServerToCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServerToCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsServerToCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServerToOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServerToOrganizationResponse")]
        SolidCP.Providers.Common.ResultObject AddRdsServerToOrganization(int itemId, int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServerToOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsServerToOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsServerToOrganizationAsync(int itemId, int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServerResponse")]
        SolidCP.Providers.Common.ResultObject RemoveRdsServer(int rdsServerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsServerAsync(int rdsServerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServerFromCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServerFromCollectionResponse")]
        SolidCP.Providers.Common.ResultObject RemoveRdsServerFromCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServerFromCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServerFromCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsServerFromCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServerFromOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServerFromOrganizationResponse")]
        SolidCP.Providers.Common.ResultObject RemoveRdsServerFromOrganization(int itemId, int rdsServerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServerFromOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRdsServerFromOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsServerFromOrganizationAsync(int itemId, int rdsServerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/UpdateRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/UpdateRdsServerResponse")]
        SolidCP.Providers.Common.ResultObject UpdateRdsServer(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/UpdateRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/UpdateRdsServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateRdsServerAsync(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionUsersResponse")]
        SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ GetRdsCollectionUsers(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionUsersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetRdsCollectionUsersAsync(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetUsersToRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetUsersToRdsCollectionResponse")]
        SolidCP.Providers.Common.ResultObject SetUsersToRdsCollection(int itemId, int collectionId, SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ users);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetUsersToRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetUsersToRdsCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUsersToRdsCollectionAsync(int itemId, int collectionId, SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ users);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetCollectionRemoteApplications", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetCollectionRemoteApplicationsResponse")]
        SolidCP.Providers.RemoteDesktopServices.RemoteApplication[] /*List*/ GetCollectionRemoteApplications(int itemId, string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetCollectionRemoteApplications", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetCollectionRemoteApplicationsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RemoteApplication[]> GetCollectionRemoteApplicationsAsync(int itemId, string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetAvailableRemoteApplications", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetAvailableRemoteApplicationsResponse")]
        SolidCP.Providers.RemoteDesktopServices.StartMenuApp[] /*List*/ GetAvailableRemoteApplications(int itemId, string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetAvailableRemoteApplications", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetAvailableRemoteApplicationsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.StartMenuApp[]> GetAvailableRemoteApplicationsAsync(int itemId, string collectionName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRemoteApplicationToCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRemoteApplicationToCollectionResponse")]
        SolidCP.Providers.Common.ResultObject AddRemoteApplicationToCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRemoteApplicationToCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRemoteApplicationToCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRemoteApplicationToCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRemoteApplicationFromCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRemoteApplicationFromCollectionResponse")]
        SolidCP.Providers.Common.ResultObject RemoveRemoteApplicationFromCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRemoteApplicationFromCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RemoveRemoteApplicationFromCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRemoteApplicationFromCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetRemoteApplicationsToRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetRemoteApplicationsToRdsCollectionResponse")]
        SolidCP.Providers.Common.ResultObject SetRemoteApplicationsToRdsCollection(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication[] /*List*/ remoteApps);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetRemoteApplicationsToRdsCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetRemoteApplicationsToRdsCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRemoteApplicationsToRdsCollectionAsync(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication[] /*List*/ remoteApps);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsUsersCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsUsersCountResponse")]
        int GetOrganizationRdsUsersCount(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsUsersCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsUsersCountResponse")]
        System.Threading.Tasks.Task<int> GetOrganizationRdsUsersCountAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServersCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServersCountResponse")]
        int GetOrganizationRdsServersCount(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServersCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsServersCountResponse")]
        System.Threading.Tasks.Task<int> GetOrganizationRdsServersCountAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsCollectionsCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsCollectionsCountResponse")]
        int GetOrganizationRdsCollectionsCount(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsCollectionsCount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetOrganizationRdsCollectionsCountResponse")]
        System.Threading.Tasks.Task<int> GetOrganizationRdsCollectionsCountAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetApplicationUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetApplicationUsersResponse")]
        string[] /*List*/ GetApplicationUsers(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetApplicationUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetApplicationUsersResponse")]
        System.Threading.Tasks.Task<string[]> GetApplicationUsersAsync(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetApplicationUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetApplicationUsersResponse")]
        SolidCP.Providers.Common.ResultObject SetApplicationUsers(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp, string[] /*List*/ users);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetApplicationUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SetApplicationUsersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetApplicationUsersAsync(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp, string[] /*List*/ users);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsUserSessions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsUserSessionsResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsUserSession[] /*List*/ GetRdsUserSessions(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsUserSessions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsUserSessionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsUserSession[]> GetRdsUserSessionsAsync(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/LogOffRdsUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/LogOffRdsUserResponse")]
        SolidCP.Providers.Common.ResultObject LogOffRdsUser(int itemId, string unifiedSessionId, string hostServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/LogOffRdsUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/LogOffRdsUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> LogOffRdsUserAsync(int itemId, string unifiedSessionId, string hostServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionSessionHosts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionSessionHostsResponse")]
        string[] /*List*/ GetRdsCollectionSessionHosts(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionSessionHosts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionSessionHostsResponse")]
        System.Threading.Tasks.Task<string[]> GetRdsCollectionSessionHostsAsync(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerInfoResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsServerInfo GetRdsServerInfo(int? itemId, string fqdnName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerInfo", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerInfoResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServerInfo> GetRdsServerInfoAsync(int? itemId, string fqdnName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerStatusResponse")]
        string GetRdsServerStatus(int? itemId, string fqdnName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerStatusResponse")]
        System.Threading.Tasks.Task<string> GetRdsServerStatusAsync(int? itemId, string fqdnName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ShutDownRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ShutDownRdsServerResponse")]
        SolidCP.Providers.Common.ResultObject ShutDownRdsServer(int? itemId, string fqdnName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ShutDownRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ShutDownRdsServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ShutDownRdsServerAsync(int? itemId, string fqdnName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RestartRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RestartRdsServerResponse")]
        SolidCP.Providers.Common.ResultObject RestartRdsServer(int? itemId, string fqdnName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RestartRdsServer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/RestartRdsServerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RestartRdsServerAsync(int? itemId, string fqdnName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionLocalAdmins", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionLocalAdminsResponse")]
        SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ GetRdsCollectionLocalAdmins(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionLocalAdmins", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCollectionLocalAdminsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetRdsCollectionLocalAdminsAsync(int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SaveRdsCollectionLocalAdmins", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SaveRdsCollectionLocalAdminsResponse")]
        SolidCP.Providers.Common.ResultObject SaveRdsCollectionLocalAdmins(SolidCP.Providers.HostedSolution.OrganizationUser[] users, int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SaveRdsCollectionLocalAdmins", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SaveRdsCollectionLocalAdminsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SaveRdsCollectionLocalAdminsAsync(SolidCP.Providers.HostedSolution.OrganizationUser[] users, int collectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/InstallSessionHostsCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/InstallSessionHostsCertificateResponse")]
        SolidCP.Providers.Common.ResultObject InstallSessionHostsCertificate(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/InstallSessionHostsCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/InstallSessionHostsCertificateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InstallSessionHostsCertificateAsync(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCertificateByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCertificateByServiceIdResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsCertificate GetRdsCertificateByServiceId(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCertificateByServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCertificateByServiceIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCertificate> GetRdsCertificateByServiceIdAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCertificateByItemId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCertificateByItemIdResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsCertificate GetRdsCertificateByItemId(int? itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCertificateByItemId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsCertificateByItemIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCertificate> GetRdsCertificateByItemIdAsync(int? itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsCertificateResponse")]
        SolidCP.Providers.Common.ResultObject AddRdsCertificate(SolidCP.Providers.RemoteDesktopServices.RdsCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/AddRdsCertificateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsCertificateAsync(SolidCP.Providers.RemoteDesktopServices.RdsCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServicesResponse")]
        SolidCP.EnterpriseServer.ServiceInfo[] /*List*/ GetRdsServices();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServicesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo[]> GetRdsServicesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsSetupLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsSetupLetterResponse")]
        string GetRdsSetupLetter(int itemId, int? accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsSetupLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsSetupLetterResponse")]
        System.Threading.Tasks.Task<string> GetRdsSetupLetterAsync(int itemId, int? accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SendRdsSetupLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SendRdsSetupLetterResponse")]
        int SendRdsSetupLetter(int itemId, int? accountId, string to, string cc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SendRdsSetupLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SendRdsSetupLetterResponse")]
        System.Threading.Tasks.Task<int> SendRdsSetupLetterAsync(int itemId, int? accountId, string to, string cc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerSettingsResponse")]
        SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings GetRdsServerSettings(int serverId, string settingsName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsServerSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings> GetRdsServerSettingsAsync(int serverId, string settingsName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/UpdateRdsServerSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/UpdateRdsServerSettingsResponse")]
        int UpdateRdsServerSettings(int serverId, string settingsName, SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/UpdateRdsServerSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/UpdateRdsServerSettingsResponse")]
        System.Threading.Tasks.Task<int> UpdateRdsServerSettingsAsync(int serverId, string settingsName, SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ShadowSession", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ShadowSessionResponse")]
        SolidCP.Providers.Common.ResultObject ShadowSession(int itemId, string sessionId, bool control, string fqdName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ShadowSession", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ShadowSessionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ShadowSessionAsync(int itemId, string sessionId, bool control, string fqdName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ImportCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ImportCollectionResponse")]
        SolidCP.Providers.Common.ResultObject ImportCollection(int itemId, string collectionName, string rdsControllerServiceID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ImportCollection", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/ImportCollectionResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ImportCollectionAsync(int itemId, string collectionName, string rdsControllerServiceID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRemoteDesktopServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRemoteDesktopServiceIdResponse")]
        int GetRemoteDesktopServiceId(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRemoteDesktopServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRemoteDesktopServiceIdResponse")]
        System.Threading.Tasks.Task<int> GetRemoteDesktopServiceIdAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SendMessage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SendMessageResponse")]
        SolidCP.Providers.Common.ResultObject SendMessage(SolidCP.Providers.RemoteDesktopServices.RdsMessageRecipient[] recipients, string text, int itemId, int rdsCollectionId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SendMessage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/SendMessageResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendMessageAsync(SolidCP.Providers.RemoteDesktopServices.RdsMessageRecipient[] recipients, string text, int itemId, int rdsCollectionId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsMessagesByCollectionId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsMessagesByCollectionIdResponse")]
        SolidCP.Providers.RemoteDesktopServices.RdsMessage[] /*List*/ GetRdsMessagesByCollectionId(int rdsCollectionId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsMessagesByCollectionId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesRemoteDesktopServices/GetRdsMessagesByCollectionIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsMessage[]> GetRdsMessagesByCollectionIdAsync(int rdsCollectionId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esRemoteDesktopServicesAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesRemoteDesktopServices
    {
        public SolidCP.Providers.RemoteDesktopServices.RdsCollection GetRdsCollection(int collectionId, bool quick)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsCollection>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollection", collectionId, quick);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollection> GetRdsCollectionAsync(int collectionId, bool quick)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsCollection>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollection", collectionId, quick);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsCollectionSettings GetRdsCollectionSettings(int collectionId)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsCollectionSettings>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollectionSettings", collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollectionSettings> GetRdsCollectionSettingsAsync(int collectionId)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsCollectionSettings>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollectionSettings", collectionId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsCollection[] /*List*/ GetOrganizationRdsCollections(int itemId)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsCollection[], SolidCP.Providers.RemoteDesktopServices.RdsCollection>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsCollections", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollection[]> GetOrganizationRdsCollectionsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsCollection[], SolidCP.Providers.RemoteDesktopServices.RdsCollection>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsCollections", itemId);
        }

        public int AddRdsCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRdsCollection", itemId, collection);
        }

        public async System.Threading.Tasks.Task<int> AddRdsCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRdsCollection", itemId, collection);
        }

        public SolidCP.Providers.Common.ResultObject EditRdsCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "EditRdsCollection", itemId, collection);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EditRdsCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "EditRdsCollection", itemId, collection);
        }

        public SolidCP.Providers.Common.ResultObject EditRdsCollectionSettings(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "EditRdsCollectionSettings", itemId, collection);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EditRdsCollectionSettingsAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "EditRdsCollectionSettings", itemId, collection);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsCollectionPaged GetRdsCollectionsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsCollectionPaged>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollectionsPaged", itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollectionPaged> GetRdsCollectionsPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsCollectionPaged>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollectionsPaged", itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRdsCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RemoveRdsCollection", itemId, collection);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RemoveRdsCollection", itemId, collection);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetRdsServersPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServersPaged", filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetRdsServersPagedAsync(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServersPaged", filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetFreeRdsServersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string ServiceId)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetFreeRdsServersPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, ServiceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetFreeRdsServersPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string ServiceId)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetFreeRdsServersPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, ServiceId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetOrganizationRdsServersPaged(int itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsServersPaged", itemId, collectionId, filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetOrganizationRdsServersPagedAsync(int itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsServersPaged", itemId, collectionId, filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetOrganizationFreeRdsServersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationFreeRdsServersPaged", itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetOrganizationFreeRdsServersPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationFreeRdsServersPaged", itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServer GetRdsServer(int rdsSeverId)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsServer>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServer", rdsSeverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServer> GetRdsServerAsync(int rdsSeverId)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsServer>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServer", rdsSeverId);
        }

        public SolidCP.Providers.Common.ResultObject SetRDServerNewConnectionAllowed(int itemId, string newConnectionAllowed, int rdsSeverId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SetRDServerNewConnectionAllowed", itemId, newConnectionAllowed, rdsSeverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRDServerNewConnectionAllowedAsync(int itemId, string newConnectionAllowed, int rdsSeverId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SetRDServerNewConnectionAllowed", itemId, newConnectionAllowed, rdsSeverId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServer[] /*List*/ GetCollectionRdsServers(int collectionId)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsServer[], SolidCP.Providers.RemoteDesktopServices.RdsServer>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetCollectionRdsServers", collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServer[]> GetCollectionRdsServersAsync(int collectionId)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsServer[], SolidCP.Providers.RemoteDesktopServices.RdsServer>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetCollectionRdsServers", collectionId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServer[] /*List*/ GetOrganizationRdsServers(int itemId)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsServer[], SolidCP.Providers.RemoteDesktopServices.RdsServer>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsServers", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServer[]> GetOrganizationRdsServersAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsServer[], SolidCP.Providers.RemoteDesktopServices.RdsServer>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsServers", itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddRdsServer(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, string rdsControllerServiceID)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRdsServer", rdsServer, rdsControllerServiceID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsServerAsync(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, string rdsControllerServiceID)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRdsServer", rdsServer, rdsControllerServiceID);
        }

        public SolidCP.Providers.Common.ResultObject AddRdsServerToCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRdsServerToCollection", itemId, rdsServer, rdsCollection);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsServerToCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRdsServerToCollection", itemId, rdsServer, rdsCollection);
        }

        public SolidCP.Providers.Common.ResultObject AddRdsServerToOrganization(int itemId, int serverId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRdsServerToOrganization", itemId, serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsServerToOrganizationAsync(int itemId, int serverId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRdsServerToOrganization", itemId, serverId);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRdsServer(int rdsServerId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RemoveRdsServer", rdsServerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsServerAsync(int rdsServerId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RemoveRdsServer", rdsServerId);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRdsServerFromCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RemoveRdsServerFromCollection", itemId, rdsServer, rdsCollection);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsServerFromCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RemoveRdsServerFromCollection", itemId, rdsServer, rdsCollection);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRdsServerFromOrganization(int itemId, int rdsServerId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RemoveRdsServerFromOrganization", itemId, rdsServerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsServerFromOrganizationAsync(int itemId, int rdsServerId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RemoveRdsServerFromOrganization", itemId, rdsServerId);
        }

        public SolidCP.Providers.Common.ResultObject UpdateRdsServer(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "UpdateRdsServer", rdsServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateRdsServerAsync(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "UpdateRdsServer", rdsServer);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ GetRdsCollectionUsers(int collectionId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationUser[], SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollectionUsers", collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetRdsCollectionUsersAsync(int collectionId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationUser[], SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollectionUsers", collectionId);
        }

        public SolidCP.Providers.Common.ResultObject SetUsersToRdsCollection(int itemId, int collectionId, SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ users)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SetUsersToRdsCollection", itemId, collectionId, users.ToList());
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUsersToRdsCollectionAsync(int itemId, int collectionId, SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ users)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SetUsersToRdsCollection", itemId, collectionId, users);
        }

        public SolidCP.Providers.RemoteDesktopServices.RemoteApplication[] /*List*/ GetCollectionRemoteApplications(int itemId, string collectionName)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RemoteApplication[], SolidCP.Providers.RemoteDesktopServices.RemoteApplication>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetCollectionRemoteApplications", itemId, collectionName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RemoteApplication[]> GetCollectionRemoteApplicationsAsync(int itemId, string collectionName)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RemoteApplication[], SolidCP.Providers.RemoteDesktopServices.RemoteApplication>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetCollectionRemoteApplications", itemId, collectionName);
        }

        public SolidCP.Providers.RemoteDesktopServices.StartMenuApp[] /*List*/ GetAvailableRemoteApplications(int itemId, string collectionName)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.StartMenuApp[], SolidCP.Providers.RemoteDesktopServices.StartMenuApp>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetAvailableRemoteApplications", itemId, collectionName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.StartMenuApp[]> GetAvailableRemoteApplicationsAsync(int itemId, string collectionName)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.StartMenuApp[], SolidCP.Providers.RemoteDesktopServices.StartMenuApp>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetAvailableRemoteApplications", itemId, collectionName);
        }

        public SolidCP.Providers.Common.ResultObject AddRemoteApplicationToCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRemoteApplicationToCollection", itemId, collection, application);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRemoteApplicationToCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRemoteApplicationToCollection", itemId, collection, application);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRemoteApplicationFromCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RemoveRemoteApplicationFromCollection", itemId, collection, application);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRemoteApplicationFromCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RemoveRemoteApplicationFromCollection", itemId, collection, application);
        }

        public SolidCP.Providers.Common.ResultObject SetRemoteApplicationsToRdsCollection(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication[] /*List*/ remoteApps)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SetRemoteApplicationsToRdsCollection", itemId, collectionId, remoteApps.ToList());
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRemoteApplicationsToRdsCollectionAsync(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication[] /*List*/ remoteApps)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SetRemoteApplicationsToRdsCollection", itemId, collectionId, remoteApps);
        }

        public int GetOrganizationRdsUsersCount(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsUsersCount", itemId);
        }

        public async System.Threading.Tasks.Task<int> GetOrganizationRdsUsersCountAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsUsersCount", itemId);
        }

        public int GetOrganizationRdsServersCount(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsServersCount", itemId);
        }

        public async System.Threading.Tasks.Task<int> GetOrganizationRdsServersCountAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsServersCount", itemId);
        }

        public int GetOrganizationRdsCollectionsCount(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsCollectionsCount", itemId);
        }

        public async System.Threading.Tasks.Task<int> GetOrganizationRdsCollectionsCountAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetOrganizationRdsCollectionsCount", itemId);
        }

        public string[] /*List*/ GetApplicationUsers(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp)
        {
            return Invoke<string[], string>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetApplicationUsers", itemId, collectionId, remoteApp);
        }

        public async System.Threading.Tasks.Task<string[]> GetApplicationUsersAsync(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp)
        {
            return await InvokeAsync<string[], string>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetApplicationUsers", itemId, collectionId, remoteApp);
        }

        public SolidCP.Providers.Common.ResultObject SetApplicationUsers(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp, string[] /*List*/ users)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SetApplicationUsers", itemId, collectionId, remoteApp, users.ToList());
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetApplicationUsersAsync(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp, string[] /*List*/ users)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SetApplicationUsers", itemId, collectionId, remoteApp, users);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsUserSession[] /*List*/ GetRdsUserSessions(int collectionId)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsUserSession[], SolidCP.Providers.RemoteDesktopServices.RdsUserSession>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsUserSessions", collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsUserSession[]> GetRdsUserSessionsAsync(int collectionId)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsUserSession[], SolidCP.Providers.RemoteDesktopServices.RdsUserSession>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsUserSessions", collectionId);
        }

        public SolidCP.Providers.Common.ResultObject LogOffRdsUser(int itemId, string unifiedSessionId, string hostServer)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "LogOffRdsUser", itemId, unifiedSessionId, hostServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> LogOffRdsUserAsync(int itemId, string unifiedSessionId, string hostServer)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "LogOffRdsUser", itemId, unifiedSessionId, hostServer);
        }

        public string[] /*List*/ GetRdsCollectionSessionHosts(int collectionId)
        {
            return Invoke<string[], string>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollectionSessionHosts", collectionId);
        }

        public async System.Threading.Tasks.Task<string[]> GetRdsCollectionSessionHostsAsync(int collectionId)
        {
            return await InvokeAsync<string[], string>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollectionSessionHosts", collectionId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServerInfo GetRdsServerInfo(int? itemId, string fqdnName)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsServerInfo>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServerInfo", itemId, fqdnName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServerInfo> GetRdsServerInfoAsync(int? itemId, string fqdnName)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsServerInfo>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServerInfo", itemId, fqdnName);
        }

        public string GetRdsServerStatus(int? itemId, string fqdnName)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServerStatus", itemId, fqdnName);
        }

        public async System.Threading.Tasks.Task<string> GetRdsServerStatusAsync(int? itemId, string fqdnName)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServerStatus", itemId, fqdnName);
        }

        public SolidCP.Providers.Common.ResultObject ShutDownRdsServer(int? itemId, string fqdnName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "ShutDownRdsServer", itemId, fqdnName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ShutDownRdsServerAsync(int? itemId, string fqdnName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "ShutDownRdsServer", itemId, fqdnName);
        }

        public SolidCP.Providers.Common.ResultObject RestartRdsServer(int? itemId, string fqdnName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RestartRdsServer", itemId, fqdnName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RestartRdsServerAsync(int? itemId, string fqdnName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "RestartRdsServer", itemId, fqdnName);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ GetRdsCollectionLocalAdmins(int collectionId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationUser[], SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollectionLocalAdmins", collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetRdsCollectionLocalAdminsAsync(int collectionId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationUser[], SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCollectionLocalAdmins", collectionId);
        }

        public SolidCP.Providers.Common.ResultObject SaveRdsCollectionLocalAdmins(SolidCP.Providers.HostedSolution.OrganizationUser[] users, int collectionId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SaveRdsCollectionLocalAdmins", users, collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SaveRdsCollectionLocalAdminsAsync(SolidCP.Providers.HostedSolution.OrganizationUser[] users, int collectionId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SaveRdsCollectionLocalAdmins", users, collectionId);
        }

        public SolidCP.Providers.Common.ResultObject InstallSessionHostsCertificate(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "InstallSessionHostsCertificate", rdsServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InstallSessionHostsCertificateAsync(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "InstallSessionHostsCertificate", rdsServer);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsCertificate GetRdsCertificateByServiceId(int serviceId)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsCertificate>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCertificateByServiceId", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCertificate> GetRdsCertificateByServiceIdAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsCertificate>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCertificateByServiceId", serviceId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsCertificate GetRdsCertificateByItemId(int? itemId)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsCertificate>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCertificateByItemId", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCertificate> GetRdsCertificateByItemIdAsync(int? itemId)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsCertificate>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsCertificateByItemId", itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddRdsCertificate(SolidCP.Providers.RemoteDesktopServices.RdsCertificate certificate)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRdsCertificate", certificate);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsCertificateAsync(SolidCP.Providers.RemoteDesktopServices.RdsCertificate certificate)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "AddRdsCertificate", certificate);
        }

        public SolidCP.EnterpriseServer.ServiceInfo[] /*List*/ GetRdsServices()
        {
            return Invoke<SolidCP.EnterpriseServer.ServiceInfo[], SolidCP.EnterpriseServer.ServiceInfo>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServices");
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo[]> GetRdsServicesAsync()
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ServiceInfo[], SolidCP.EnterpriseServer.ServiceInfo>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServices");
        }

        public string GetRdsSetupLetter(int itemId, int? accountId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsSetupLetter", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<string> GetRdsSetupLetterAsync(int itemId, int? accountId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsSetupLetter", itemId, accountId);
        }

        public int SendRdsSetupLetter(int itemId, int? accountId, string to, string cc)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SendRdsSetupLetter", itemId, accountId, to, cc);
        }

        public async System.Threading.Tasks.Task<int> SendRdsSetupLetterAsync(int itemId, int? accountId, string to, string cc)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SendRdsSetupLetter", itemId, accountId, to, cc);
        }

        public SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings GetRdsServerSettings(int serverId, string settingsName)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServerSettings", serverId, settingsName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings> GetRdsServerSettingsAsync(int serverId, string settingsName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsServerSettings", serverId, settingsName);
        }

        public int UpdateRdsServerSettings(int serverId, string settingsName, SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings settings)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "UpdateRdsServerSettings", serverId, settingsName, settings);
        }

        public async System.Threading.Tasks.Task<int> UpdateRdsServerSettingsAsync(int serverId, string settingsName, SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings settings)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "UpdateRdsServerSettings", serverId, settingsName, settings);
        }

        public SolidCP.Providers.Common.ResultObject ShadowSession(int itemId, string sessionId, bool control, string fqdName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "ShadowSession", itemId, sessionId, control, fqdName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ShadowSessionAsync(int itemId, string sessionId, bool control, string fqdName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "ShadowSession", itemId, sessionId, control, fqdName);
        }

        public SolidCP.Providers.Common.ResultObject ImportCollection(int itemId, string collectionName, string rdsControllerServiceID)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "ImportCollection", itemId, collectionName, rdsControllerServiceID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ImportCollectionAsync(int itemId, string collectionName, string rdsControllerServiceID)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "ImportCollection", itemId, collectionName, rdsControllerServiceID);
        }

        public int GetRemoteDesktopServiceId(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRemoteDesktopServiceId", itemId);
        }

        public async System.Threading.Tasks.Task<int> GetRemoteDesktopServiceIdAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRemoteDesktopServiceId", itemId);
        }

        public SolidCP.Providers.Common.ResultObject SendMessage(SolidCP.Providers.RemoteDesktopServices.RdsMessageRecipient[] recipients, string text, int itemId, int rdsCollectionId, string userName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SendMessage", recipients, text, itemId, rdsCollectionId, userName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendMessageAsync(SolidCP.Providers.RemoteDesktopServices.RdsMessageRecipient[] recipients, string text, int itemId, int rdsCollectionId, string userName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "SendMessage", recipients, text, itemId, rdsCollectionId, userName);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsMessage[] /*List*/ GetRdsMessagesByCollectionId(int rdsCollectionId)
        {
            return Invoke<SolidCP.Providers.RemoteDesktopServices.RdsMessage[], SolidCP.Providers.RemoteDesktopServices.RdsMessage>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsMessagesByCollectionId", rdsCollectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsMessage[]> GetRdsMessagesByCollectionIdAsync(int rdsCollectionId)
        {
            return await InvokeAsync<SolidCP.Providers.RemoteDesktopServices.RdsMessage[], SolidCP.Providers.RemoteDesktopServices.RdsMessage>("SolidCP.EnterpriseServer.esRemoteDesktopServices", "GetRdsMessagesByCollectionId", rdsCollectionId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esRemoteDesktopServices : SolidCP.Web.Client.ClientBase<IesRemoteDesktopServices, esRemoteDesktopServicesAssemblyClient>, IesRemoteDesktopServices
    {
        public SolidCP.Providers.RemoteDesktopServices.RdsCollection GetRdsCollection(int collectionId, bool quick)
        {
            return base.Client.GetRdsCollection(collectionId, quick);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollection> GetRdsCollectionAsync(int collectionId, bool quick)
        {
            return await base.Client.GetRdsCollectionAsync(collectionId, quick);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsCollectionSettings GetRdsCollectionSettings(int collectionId)
        {
            return base.Client.GetRdsCollectionSettings(collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollectionSettings> GetRdsCollectionSettingsAsync(int collectionId)
        {
            return await base.Client.GetRdsCollectionSettingsAsync(collectionId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsCollection[] /*List*/ GetOrganizationRdsCollections(int itemId)
        {
            return base.Client.GetOrganizationRdsCollections(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollection[]> GetOrganizationRdsCollectionsAsync(int itemId)
        {
            return await base.Client.GetOrganizationRdsCollectionsAsync(itemId);
        }

        public int AddRdsCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return base.Client.AddRdsCollection(itemId, collection);
        }

        public async System.Threading.Tasks.Task<int> AddRdsCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return await base.Client.AddRdsCollectionAsync(itemId, collection);
        }

        public SolidCP.Providers.Common.ResultObject EditRdsCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return base.Client.EditRdsCollection(itemId, collection);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EditRdsCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return await base.Client.EditRdsCollectionAsync(itemId, collection);
        }

        public SolidCP.Providers.Common.ResultObject EditRdsCollectionSettings(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return base.Client.EditRdsCollectionSettings(itemId, collection);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> EditRdsCollectionSettingsAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return await base.Client.EditRdsCollectionSettingsAsync(itemId, collection);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsCollectionPaged GetRdsCollectionsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRdsCollectionsPaged(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCollectionPaged> GetRdsCollectionsPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRdsCollectionsPagedAsync(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRdsCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return base.Client.RemoveRdsCollection(itemId, collection);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection)
        {
            return await base.Client.RemoveRdsCollectionAsync(itemId, collection);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetRdsServersPaged(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return base.Client.GetRdsServersPaged(filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetRdsServersPagedAsync(string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return await base.Client.GetRdsServersPagedAsync(filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetFreeRdsServersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string ServiceId)
        {
            return base.Client.GetFreeRdsServersPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, ServiceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetFreeRdsServersPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string ServiceId)
        {
            return await base.Client.GetFreeRdsServersPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows, ServiceId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetOrganizationRdsServersPaged(int itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return base.Client.GetOrganizationRdsServersPaged(itemId, collectionId, filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetOrganizationRdsServersPagedAsync(int itemId, int? collectionId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return await base.Client.GetOrganizationRdsServersPagedAsync(itemId, collectionId, filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServersPaged GetOrganizationFreeRdsServersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return base.Client.GetOrganizationFreeRdsServersPaged(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServersPaged> GetOrganizationFreeRdsServersPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, string rdsControllerServiceID)
        {
            return await base.Client.GetOrganizationFreeRdsServersPagedAsync(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows, rdsControllerServiceID);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServer GetRdsServer(int rdsSeverId)
        {
            return base.Client.GetRdsServer(rdsSeverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServer> GetRdsServerAsync(int rdsSeverId)
        {
            return await base.Client.GetRdsServerAsync(rdsSeverId);
        }

        public SolidCP.Providers.Common.ResultObject SetRDServerNewConnectionAllowed(int itemId, string newConnectionAllowed, int rdsSeverId)
        {
            return base.Client.SetRDServerNewConnectionAllowed(itemId, newConnectionAllowed, rdsSeverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRDServerNewConnectionAllowedAsync(int itemId, string newConnectionAllowed, int rdsSeverId)
        {
            return await base.Client.SetRDServerNewConnectionAllowedAsync(itemId, newConnectionAllowed, rdsSeverId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServer[] /*List*/ GetCollectionRdsServers(int collectionId)
        {
            return base.Client.GetCollectionRdsServers(collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServer[]> GetCollectionRdsServersAsync(int collectionId)
        {
            return await base.Client.GetCollectionRdsServersAsync(collectionId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServer[] /*List*/ GetOrganizationRdsServers(int itemId)
        {
            return base.Client.GetOrganizationRdsServers(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServer[]> GetOrganizationRdsServersAsync(int itemId)
        {
            return await base.Client.GetOrganizationRdsServersAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddRdsServer(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, string rdsControllerServiceID)
        {
            return base.Client.AddRdsServer(rdsServer, rdsControllerServiceID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsServerAsync(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, string rdsControllerServiceID)
        {
            return await base.Client.AddRdsServerAsync(rdsServer, rdsControllerServiceID);
        }

        public SolidCP.Providers.Common.ResultObject AddRdsServerToCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection)
        {
            return base.Client.AddRdsServerToCollection(itemId, rdsServer, rdsCollection);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsServerToCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection)
        {
            return await base.Client.AddRdsServerToCollectionAsync(itemId, rdsServer, rdsCollection);
        }

        public SolidCP.Providers.Common.ResultObject AddRdsServerToOrganization(int itemId, int serverId)
        {
            return base.Client.AddRdsServerToOrganization(itemId, serverId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsServerToOrganizationAsync(int itemId, int serverId)
        {
            return await base.Client.AddRdsServerToOrganizationAsync(itemId, serverId);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRdsServer(int rdsServerId)
        {
            return base.Client.RemoveRdsServer(rdsServerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsServerAsync(int rdsServerId)
        {
            return await base.Client.RemoveRdsServerAsync(rdsServerId);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRdsServerFromCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection)
        {
            return base.Client.RemoveRdsServerFromCollection(itemId, rdsServer, rdsCollection);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsServerFromCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer, SolidCP.Providers.RemoteDesktopServices.RdsCollection rdsCollection)
        {
            return await base.Client.RemoveRdsServerFromCollectionAsync(itemId, rdsServer, rdsCollection);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRdsServerFromOrganization(int itemId, int rdsServerId)
        {
            return base.Client.RemoveRdsServerFromOrganization(itemId, rdsServerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRdsServerFromOrganizationAsync(int itemId, int rdsServerId)
        {
            return await base.Client.RemoveRdsServerFromOrganizationAsync(itemId, rdsServerId);
        }

        public SolidCP.Providers.Common.ResultObject UpdateRdsServer(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer)
        {
            return base.Client.UpdateRdsServer(rdsServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateRdsServerAsync(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer)
        {
            return await base.Client.UpdateRdsServerAsync(rdsServer);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ GetRdsCollectionUsers(int collectionId)
        {
            return base.Client.GetRdsCollectionUsers(collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetRdsCollectionUsersAsync(int collectionId)
        {
            return await base.Client.GetRdsCollectionUsersAsync(collectionId);
        }

        public SolidCP.Providers.Common.ResultObject SetUsersToRdsCollection(int itemId, int collectionId, SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ users)
        {
            return base.Client.SetUsersToRdsCollection(itemId, collectionId, users);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetUsersToRdsCollectionAsync(int itemId, int collectionId, SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ users)
        {
            return await base.Client.SetUsersToRdsCollectionAsync(itemId, collectionId, users);
        }

        public SolidCP.Providers.RemoteDesktopServices.RemoteApplication[] /*List*/ GetCollectionRemoteApplications(int itemId, string collectionName)
        {
            return base.Client.GetCollectionRemoteApplications(itemId, collectionName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RemoteApplication[]> GetCollectionRemoteApplicationsAsync(int itemId, string collectionName)
        {
            return await base.Client.GetCollectionRemoteApplicationsAsync(itemId, collectionName);
        }

        public SolidCP.Providers.RemoteDesktopServices.StartMenuApp[] /*List*/ GetAvailableRemoteApplications(int itemId, string collectionName)
        {
            return base.Client.GetAvailableRemoteApplications(itemId, collectionName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.StartMenuApp[]> GetAvailableRemoteApplicationsAsync(int itemId, string collectionName)
        {
            return await base.Client.GetAvailableRemoteApplicationsAsync(itemId, collectionName);
        }

        public SolidCP.Providers.Common.ResultObject AddRemoteApplicationToCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application)
        {
            return base.Client.AddRemoteApplicationToCollection(itemId, collection, application);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRemoteApplicationToCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application)
        {
            return await base.Client.AddRemoteApplicationToCollectionAsync(itemId, collection, application);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRemoteApplicationFromCollection(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application)
        {
            return base.Client.RemoveRemoteApplicationFromCollection(itemId, collection, application);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRemoteApplicationFromCollectionAsync(int itemId, SolidCP.Providers.RemoteDesktopServices.RdsCollection collection, SolidCP.Providers.RemoteDesktopServices.RemoteApplication application)
        {
            return await base.Client.RemoveRemoteApplicationFromCollectionAsync(itemId, collection, application);
        }

        public SolidCP.Providers.Common.ResultObject SetRemoteApplicationsToRdsCollection(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication[] /*List*/ remoteApps)
        {
            return base.Client.SetRemoteApplicationsToRdsCollection(itemId, collectionId, remoteApps);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRemoteApplicationsToRdsCollectionAsync(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication[] /*List*/ remoteApps)
        {
            return await base.Client.SetRemoteApplicationsToRdsCollectionAsync(itemId, collectionId, remoteApps);
        }

        public int GetOrganizationRdsUsersCount(int itemId)
        {
            return base.Client.GetOrganizationRdsUsersCount(itemId);
        }

        public async System.Threading.Tasks.Task<int> GetOrganizationRdsUsersCountAsync(int itemId)
        {
            return await base.Client.GetOrganizationRdsUsersCountAsync(itemId);
        }

        public int GetOrganizationRdsServersCount(int itemId)
        {
            return base.Client.GetOrganizationRdsServersCount(itemId);
        }

        public async System.Threading.Tasks.Task<int> GetOrganizationRdsServersCountAsync(int itemId)
        {
            return await base.Client.GetOrganizationRdsServersCountAsync(itemId);
        }

        public int GetOrganizationRdsCollectionsCount(int itemId)
        {
            return base.Client.GetOrganizationRdsCollectionsCount(itemId);
        }

        public async System.Threading.Tasks.Task<int> GetOrganizationRdsCollectionsCountAsync(int itemId)
        {
            return await base.Client.GetOrganizationRdsCollectionsCountAsync(itemId);
        }

        public string[] /*List*/ GetApplicationUsers(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp)
        {
            return base.Client.GetApplicationUsers(itemId, collectionId, remoteApp);
        }

        public async System.Threading.Tasks.Task<string[]> GetApplicationUsersAsync(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp)
        {
            return await base.Client.GetApplicationUsersAsync(itemId, collectionId, remoteApp);
        }

        public SolidCP.Providers.Common.ResultObject SetApplicationUsers(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp, string[] /*List*/ users)
        {
            return base.Client.SetApplicationUsers(itemId, collectionId, remoteApp, users);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetApplicationUsersAsync(int itemId, int collectionId, SolidCP.Providers.RemoteDesktopServices.RemoteApplication remoteApp, string[] /*List*/ users)
        {
            return await base.Client.SetApplicationUsersAsync(itemId, collectionId, remoteApp, users);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsUserSession[] /*List*/ GetRdsUserSessions(int collectionId)
        {
            return base.Client.GetRdsUserSessions(collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsUserSession[]> GetRdsUserSessionsAsync(int collectionId)
        {
            return await base.Client.GetRdsUserSessionsAsync(collectionId);
        }

        public SolidCP.Providers.Common.ResultObject LogOffRdsUser(int itemId, string unifiedSessionId, string hostServer)
        {
            return base.Client.LogOffRdsUser(itemId, unifiedSessionId, hostServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> LogOffRdsUserAsync(int itemId, string unifiedSessionId, string hostServer)
        {
            return await base.Client.LogOffRdsUserAsync(itemId, unifiedSessionId, hostServer);
        }

        public string[] /*List*/ GetRdsCollectionSessionHosts(int collectionId)
        {
            return base.Client.GetRdsCollectionSessionHosts(collectionId);
        }

        public async System.Threading.Tasks.Task<string[]> GetRdsCollectionSessionHostsAsync(int collectionId)
        {
            return await base.Client.GetRdsCollectionSessionHostsAsync(collectionId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsServerInfo GetRdsServerInfo(int? itemId, string fqdnName)
        {
            return base.Client.GetRdsServerInfo(itemId, fqdnName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsServerInfo> GetRdsServerInfoAsync(int? itemId, string fqdnName)
        {
            return await base.Client.GetRdsServerInfoAsync(itemId, fqdnName);
        }

        public string GetRdsServerStatus(int? itemId, string fqdnName)
        {
            return base.Client.GetRdsServerStatus(itemId, fqdnName);
        }

        public async System.Threading.Tasks.Task<string> GetRdsServerStatusAsync(int? itemId, string fqdnName)
        {
            return await base.Client.GetRdsServerStatusAsync(itemId, fqdnName);
        }

        public SolidCP.Providers.Common.ResultObject ShutDownRdsServer(int? itemId, string fqdnName)
        {
            return base.Client.ShutDownRdsServer(itemId, fqdnName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ShutDownRdsServerAsync(int? itemId, string fqdnName)
        {
            return await base.Client.ShutDownRdsServerAsync(itemId, fqdnName);
        }

        public SolidCP.Providers.Common.ResultObject RestartRdsServer(int? itemId, string fqdnName)
        {
            return base.Client.RestartRdsServer(itemId, fqdnName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RestartRdsServerAsync(int? itemId, string fqdnName)
        {
            return await base.Client.RestartRdsServerAsync(itemId, fqdnName);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ GetRdsCollectionLocalAdmins(int collectionId)
        {
            return base.Client.GetRdsCollectionLocalAdmins(collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetRdsCollectionLocalAdminsAsync(int collectionId)
        {
            return await base.Client.GetRdsCollectionLocalAdminsAsync(collectionId);
        }

        public SolidCP.Providers.Common.ResultObject SaveRdsCollectionLocalAdmins(SolidCP.Providers.HostedSolution.OrganizationUser[] users, int collectionId)
        {
            return base.Client.SaveRdsCollectionLocalAdmins(users, collectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SaveRdsCollectionLocalAdminsAsync(SolidCP.Providers.HostedSolution.OrganizationUser[] users, int collectionId)
        {
            return await base.Client.SaveRdsCollectionLocalAdminsAsync(users, collectionId);
        }

        public SolidCP.Providers.Common.ResultObject InstallSessionHostsCertificate(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer)
        {
            return base.Client.InstallSessionHostsCertificate(rdsServer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InstallSessionHostsCertificateAsync(SolidCP.Providers.RemoteDesktopServices.RdsServer rdsServer)
        {
            return await base.Client.InstallSessionHostsCertificateAsync(rdsServer);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsCertificate GetRdsCertificateByServiceId(int serviceId)
        {
            return base.Client.GetRdsCertificateByServiceId(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCertificate> GetRdsCertificateByServiceIdAsync(int serviceId)
        {
            return await base.Client.GetRdsCertificateByServiceIdAsync(serviceId);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsCertificate GetRdsCertificateByItemId(int? itemId)
        {
            return base.Client.GetRdsCertificateByItemId(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsCertificate> GetRdsCertificateByItemIdAsync(int? itemId)
        {
            return await base.Client.GetRdsCertificateByItemIdAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject AddRdsCertificate(SolidCP.Providers.RemoteDesktopServices.RdsCertificate certificate)
        {
            return base.Client.AddRdsCertificate(certificate);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> AddRdsCertificateAsync(SolidCP.Providers.RemoteDesktopServices.RdsCertificate certificate)
        {
            return await base.Client.AddRdsCertificateAsync(certificate);
        }

        public SolidCP.EnterpriseServer.ServiceInfo[] /*List*/ GetRdsServices()
        {
            return base.Client.GetRdsServices();
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ServiceInfo[]> GetRdsServicesAsync()
        {
            return await base.Client.GetRdsServicesAsync();
        }

        public string GetRdsSetupLetter(int itemId, int? accountId)
        {
            return base.Client.GetRdsSetupLetter(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<string> GetRdsSetupLetterAsync(int itemId, int? accountId)
        {
            return await base.Client.GetRdsSetupLetterAsync(itemId, accountId);
        }

        public int SendRdsSetupLetter(int itemId, int? accountId, string to, string cc)
        {
            return base.Client.SendRdsSetupLetter(itemId, accountId, to, cc);
        }

        public async System.Threading.Tasks.Task<int> SendRdsSetupLetterAsync(int itemId, int? accountId, string to, string cc)
        {
            return await base.Client.SendRdsSetupLetterAsync(itemId, accountId, to, cc);
        }

        public SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings GetRdsServerSettings(int serverId, string settingsName)
        {
            return base.Client.GetRdsServerSettings(serverId, settingsName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings> GetRdsServerSettingsAsync(int serverId, string settingsName)
        {
            return await base.Client.GetRdsServerSettingsAsync(serverId, settingsName);
        }

        public int UpdateRdsServerSettings(int serverId, string settingsName, SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings settings)
        {
            return base.Client.UpdateRdsServerSettings(serverId, settingsName, settings);
        }

        public async System.Threading.Tasks.Task<int> UpdateRdsServerSettingsAsync(int serverId, string settingsName, SolidCP.EnterpriseServer.Base.RDS.RdsServerSettings settings)
        {
            return await base.Client.UpdateRdsServerSettingsAsync(serverId, settingsName, settings);
        }

        public SolidCP.Providers.Common.ResultObject ShadowSession(int itemId, string sessionId, bool control, string fqdName)
        {
            return base.Client.ShadowSession(itemId, sessionId, control, fqdName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ShadowSessionAsync(int itemId, string sessionId, bool control, string fqdName)
        {
            return await base.Client.ShadowSessionAsync(itemId, sessionId, control, fqdName);
        }

        public SolidCP.Providers.Common.ResultObject ImportCollection(int itemId, string collectionName, string rdsControllerServiceID)
        {
            return base.Client.ImportCollection(itemId, collectionName, rdsControllerServiceID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ImportCollectionAsync(int itemId, string collectionName, string rdsControllerServiceID)
        {
            return await base.Client.ImportCollectionAsync(itemId, collectionName, rdsControllerServiceID);
        }

        public int GetRemoteDesktopServiceId(int itemId)
        {
            return base.Client.GetRemoteDesktopServiceId(itemId);
        }

        public async System.Threading.Tasks.Task<int> GetRemoteDesktopServiceIdAsync(int itemId)
        {
            return await base.Client.GetRemoteDesktopServiceIdAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject SendMessage(SolidCP.Providers.RemoteDesktopServices.RdsMessageRecipient[] recipients, string text, int itemId, int rdsCollectionId, string userName)
        {
            return base.Client.SendMessage(recipients, text, itemId, rdsCollectionId, userName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendMessageAsync(SolidCP.Providers.RemoteDesktopServices.RdsMessageRecipient[] recipients, string text, int itemId, int rdsCollectionId, string userName)
        {
            return await base.Client.SendMessageAsync(recipients, text, itemId, rdsCollectionId, userName);
        }

        public SolidCP.Providers.RemoteDesktopServices.RdsMessage[] /*List*/ GetRdsMessagesByCollectionId(int rdsCollectionId)
        {
            return base.Client.GetRdsMessagesByCollectionId(rdsCollectionId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.RemoteDesktopServices.RdsMessage[]> GetRdsMessagesByCollectionIdAsync(int rdsCollectionId)
        {
            return await base.Client.GetRdsMessagesByCollectionIdAsync(rdsCollectionId);
        }
    }
}
#endif