#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesPackages", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesPackages
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlans", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlansResponse")]
        System.Data.DataSet GetHostingPlans(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlans", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlansResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetHostingPlansAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingAddons", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingAddonsResponse")]
        System.Data.DataSet GetHostingAddons(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingAddons", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingAddonsResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetHostingAddonsAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlanResponse")]
        SolidCP.EnterpriseServer.HostingPlanInfo GetHostingPlan(int planId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlanResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanInfo> GetHostingPlanAsync(int planId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlanQuotas", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlanQuotasResponse")]
        System.Data.DataSet GetHostingPlanQuotas(int packageId, int planId, int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlanQuotas", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlanQuotasResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetHostingPlanQuotasAsync(int packageId, int planId, int serverId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlanContext", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlanContextResponse")]
        SolidCP.EnterpriseServer.HostingPlanContext GetHostingPlanContext(int planId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlanContext", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetHostingPlanContextResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanContext> GetHostingPlanContextAsync(int planId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetUserAvailableHostingPlans", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetUserAvailableHostingPlansResponse")]
        SolidCP.EnterpriseServer.HostingPlanInfo[] /*List*/ GetUserAvailableHostingPlans(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetUserAvailableHostingPlans", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetUserAvailableHostingPlansResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanInfo[]> GetUserAvailableHostingPlansAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetUserAvailableHostingAddons", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetUserAvailableHostingAddonsResponse")]
        SolidCP.EnterpriseServer.HostingPlanInfo[] /*List*/ GetUserAvailableHostingAddons(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetUserAvailableHostingAddons", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetUserAvailableHostingAddonsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanInfo[]> GetUserAvailableHostingAddonsAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddHostingPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddHostingPlanResponse")]
        int AddHostingPlan(SolidCP.EnterpriseServer.HostingPlanInfo plan);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddHostingPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddHostingPlanResponse")]
        System.Threading.Tasks.Task<int> AddHostingPlanAsync(SolidCP.EnterpriseServer.HostingPlanInfo plan);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdateHostingPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdateHostingPlanResponse")]
        SolidCP.EnterpriseServer.PackageResult UpdateHostingPlan(SolidCP.EnterpriseServer.HostingPlanInfo plan);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdateHostingPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdateHostingPlanResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdateHostingPlanAsync(SolidCP.EnterpriseServer.HostingPlanInfo plan);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeleteHostingPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeleteHostingPlanResponse")]
        int DeleteHostingPlan(int planId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeleteHostingPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeleteHostingPlanResponse")]
        System.Threading.Tasks.Task<int> DeleteHostingPlanAsync(int planId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesResponse")]
        SolidCP.EnterpriseServer.PackageInfo[] /*List*/ GetPackages(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo[]> GetPackagesAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetNestedPackagesSummary", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetNestedPackagesSummaryResponse")]
        System.Data.DataSet GetNestedPackagesSummary(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetNestedPackagesSummary", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetNestedPackagesSummaryResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetNestedPackagesSummaryAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackagesResponse")]
        System.Data.DataSet GetRawPackages(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackagesResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackagesAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SearchServiceItemsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SearchServiceItemsPagedResponse")]
        System.Data.DataSet SearchServiceItemsPaged(int userId, int itemTypeId, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SearchServiceItemsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SearchServiceItemsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> SearchServiceItemsPagedAsync(int userId, int itemTypeId, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchObject", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchObjectResponse")]
        System.Data.DataSet GetSearchObject(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows, string colType, string fullType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchObject", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchObjectResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetSearchObjectAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows, string colType, string fullType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchObjectQuickFind", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchObjectQuickFindResponse")]
        System.Data.DataSet GetSearchObjectQuickFind(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int maximumRows, string colType, string fullType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchObjectQuickFind", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchObjectQuickFindResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetSearchObjectQuickFindAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int maximumRows, string colType, string fullType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchTableByColumns", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchTableByColumnsResponse")]
        System.Data.DataSet GetSearchTableByColumns(string PagedStored, string FilterValue, int MaximumRows, bool Recursive, int PoolID, int ServerID, int StatusID, int PlanID, int OrgID, string ItemTypeName, string GroupName, int PackageID, string VPSType, int RoleID, int UserID, string FilterColumns);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchTableByColumns", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchTableByColumnsResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetSearchTableByColumnsAsync(string PagedStored, string FilterValue, int MaximumRows, bool Recursive, int PoolID, int ServerID, int StatusID, int PlanID, int OrgID, string ItemTypeName, string GroupName, int PackageID, string VPSType, int RoleID, int UserID, string FilterColumns);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesPagedResponse")]
        System.Data.DataSet GetPackagesPaged(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetPackagesPagedAsync(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetNestedPackagesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetNestedPackagesPagedResponse")]
        System.Data.DataSet GetNestedPackagesPaged(int packageId, string filterColumn, string filterValue, int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetNestedPackagesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetNestedPackagesPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetNestedPackagesPagedAsync(int packageId, string filterColumn, string filterValue, int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagePackages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagePackagesResponse")]
        SolidCP.EnterpriseServer.PackageInfo[] /*List*/ GetPackagePackages(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagePackages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagePackagesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo[]> GetPackagePackagesAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetMyPackages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetMyPackagesResponse")]
        SolidCP.EnterpriseServer.PackageInfo[] /*List*/ GetMyPackages(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetMyPackages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetMyPackagesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo[]> GetMyPackagesAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawMyPackages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawMyPackagesResponse")]
        System.Data.DataSet GetRawMyPackages(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawMyPackages", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawMyPackagesResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawMyPackagesAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageResponse")]
        SolidCP.EnterpriseServer.PackageInfo GetPackage(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo> GetPackageAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageContext", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageContextResponse")]
        SolidCP.EnterpriseServer.PackageContext GetPackageContext(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageContext", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageContextResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageContext> GetPackageContextAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuotas", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuotasResponse")]
        System.Data.DataSet GetPackageQuotas(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuotas", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuotasResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetPackageQuotasAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuotasForEdit", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuotasForEditResponse")]
        System.Data.DataSet GetPackageQuotasForEdit(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuotasForEdit", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuotasForEditResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetPackageQuotasForEditAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageResponse")]
        SolidCP.EnterpriseServer.PackageResult AddPackage(int userId, int planId, string packageName, string packageComments, int statusId, System.DateTime purchaseDate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAsync(int userId, int planId, string packageName, string packageComments, int statusId, System.DateTime purchaseDate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageResponse")]
        SolidCP.EnterpriseServer.PackageResult UpdatePackage(SolidCP.EnterpriseServer.PackageInfo package);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageAsync(SolidCP.EnterpriseServer.PackageInfo package);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageLiteral", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageLiteralResponse")]
        SolidCP.EnterpriseServer.PackageResult UpdatePackageLiteral(int packageId, int statusId, int planId, System.DateTime purchaseDate, string packageName, string packageComments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageLiteral", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageLiteralResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageLiteralAsync(int packageId, int statusId, int planId, System.DateTime purchaseDate, string packageName, string packageComments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/ChangePackageUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/ChangePackageUserResponse")]
        int ChangePackageUser(int packageId, int UserId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/ChangePackageUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/ChangePackageUserResponse")]
        System.Threading.Tasks.Task<int> ChangePackageUserAsync(int packageId, int UserId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageNameResponse")]
        int UpdatePackageName(int packageId, string packageName, string packageComments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageName", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageNameResponse")]
        System.Threading.Tasks.Task<int> UpdatePackageNameAsync(int packageId, string packageName, string packageComments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeletePackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeletePackageResponse")]
        int DeletePackage(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeletePackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeletePackageResponse")]
        System.Threading.Tasks.Task<int> DeletePackageAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/ChangePackageStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/ChangePackageStatusResponse")]
        int ChangePackageStatus(int packageId, SolidCP.EnterpriseServer.PackageStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/ChangePackageStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/ChangePackageStatusResponse")]
        System.Threading.Tasks.Task<int> ChangePackageStatusAsync(int packageId, SolidCP.EnterpriseServer.PackageStatus status);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/EvaluateUserPackageTempate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/EvaluateUserPackageTempateResponse")]
        string EvaluateUserPackageTempate(int userId, int packageId, string template);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/EvaluateUserPackageTempate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/EvaluateUserPackageTempateResponse")]
        System.Threading.Tasks.Task<string> EvaluateUserPackageTempateAsync(int userId, int packageId, string template);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageSettingsResponse")]
        SolidCP.EnterpriseServer.PackageSettings GetPackageSettings(int packageId, string settingsName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageSettings> GetPackageSettingsAsync(int packageId, string settingsName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageSettingsResponse")]
        int UpdatePackageSettings(SolidCP.EnterpriseServer.PackageSettings settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageSettingsResponse")]
        System.Threading.Tasks.Task<int> UpdatePackageSettingsAsync(SolidCP.EnterpriseServer.PackageSettings settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SetDefaultTopPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SetDefaultTopPackageResponse")]
        bool SetDefaultTopPackage(int userId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SetDefaultTopPackage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SetDefaultTopPackageResponse")]
        System.Threading.Tasks.Task<bool> SetDefaultTopPackageAsync(int userId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageAddons", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageAddonsResponse")]
        System.Data.DataSet GetPackageAddons(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageAddons", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageAddonsResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetPackageAddonsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageAddon", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageAddonResponse")]
        SolidCP.EnterpriseServer.PackageAddonInfo GetPackageAddon(int packageAddonId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageAddon", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageAddonResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageAddonInfo> GetPackageAddonAsync(int packageAddonId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddonById", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddonByIdResponse")]
        SolidCP.EnterpriseServer.PackageResult AddPackageAddonById(int packageId, int addonPlanId, int quantity);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddonById", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddonByIdResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAddonByIdAsync(int packageId, int addonPlanId, int quantity);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddon", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddonResponse")]
        SolidCP.EnterpriseServer.PackageResult AddPackageAddon(SolidCP.EnterpriseServer.PackageAddonInfo addon);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddon", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddonResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAddonAsync(SolidCP.EnterpriseServer.PackageAddonInfo addon);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddonLiteral", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddonLiteralResponse")]
        SolidCP.EnterpriseServer.PackageResult AddPackageAddonLiteral(int packageId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddonLiteral", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageAddonLiteralResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAddonLiteralAsync(int packageId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageAddon", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageAddonResponse")]
        SolidCP.EnterpriseServer.PackageResult UpdatePackageAddon(SolidCP.EnterpriseServer.PackageAddonInfo addon);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageAddon", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageAddonResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageAddonAsync(SolidCP.EnterpriseServer.PackageAddonInfo addon);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageAddonLiteral", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageAddonLiteralResponse")]
        SolidCP.EnterpriseServer.PackageResult UpdatePackageAddonLiteral(int packageAddonId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageAddonLiteral", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/UpdatePackageAddonLiteralResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageAddonLiteralAsync(int packageAddonId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeletePackageAddon", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeletePackageAddonResponse")]
        int DeletePackageAddon(int packageAddonId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeletePackageAddon", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DeletePackageAddonResponse")]
        System.Threading.Tasks.Task<int> DeletePackageAddonAsync(int packageAddonId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchableServiceItemTypes", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchableServiceItemTypesResponse")]
        System.Data.DataSet GetSearchableServiceItemTypes();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchableServiceItemTypes", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetSearchableServiceItemTypesResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetSearchableServiceItemTypesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItemsByType", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItemsByTypeResponse")]
        System.Data.DataSet GetRawPackageItemsByType(int packageId, string itemTypeName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItemsByType", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItemsByTypeResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackageItemsByTypeAsync(int packageId, string itemTypeName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItemsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItemsPagedResponse")]
        System.Data.DataSet GetRawPackageItemsPaged(int packageId, string groupName, string typeName, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItemsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItemsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackageItemsPagedAsync(int packageId, string groupName, string typeName, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItems", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItemsResponse")]
        System.Data.DataSet GetRawPackageItems(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItems", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetRawPackageItemsResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackageItemsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DetachPackageItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DetachPackageItemResponse")]
        int DetachPackageItem(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DetachPackageItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/DetachPackageItemResponse")]
        System.Threading.Tasks.Task<int> DetachPackageItemAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/MovePackageItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/MovePackageItemResponse")]
        int MovePackageItem(int itemId, int destinationServiceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/MovePackageItem", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/MovePackageItemResponse")]
        System.Threading.Tasks.Task<int> MovePackageItemAsync(int itemId, int destinationServiceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuota", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuotaResponse")]
        SolidCP.EnterpriseServer.QuotaValueInfo GetPackageQuota(int packageId, string quotaName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuota", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageQuotaResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.QuotaValueInfo> GetPackageQuotaAsync(int packageId, string quotaName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SendAccountSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SendAccountSummaryLetterResponse")]
        int SendAccountSummaryLetter(int userId, string to, string cc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SendAccountSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SendAccountSummaryLetterResponse")]
        System.Threading.Tasks.Task<int> SendAccountSummaryLetterAsync(int userId, string to, string cc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SendPackageSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SendPackageSummaryLetterResponse")]
        int SendPackageSummaryLetter(int packageId, string to, string cc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SendPackageSummaryLetter", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/SendPackageSummaryLetterResponse")]
        System.Threading.Tasks.Task<int> SendPackageSummaryLetterAsync(int packageId, string to, string cc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetEvaluatedPackageTemplateBody", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetEvaluatedPackageTemplateBodyResponse")]
        string GetEvaluatedPackageTemplateBody(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetEvaluatedPackageTemplateBody", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetEvaluatedPackageTemplateBodyResponse")]
        System.Threading.Tasks.Task<string> GetEvaluatedPackageTemplateBodyAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetEvaluatedAccountTemplateBody", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetEvaluatedAccountTemplateBodyResponse")]
        string GetEvaluatedAccountTemplateBody(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetEvaluatedAccountTemplateBody", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetEvaluatedAccountTemplateBodyResponse")]
        System.Threading.Tasks.Task<string> GetEvaluatedAccountTemplateBodyAsync(int userId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageWithResources", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageWithResourcesResponse")]
        SolidCP.EnterpriseServer.PackageResult AddPackageWithResources(int userId, int planId, string spaceName, int statusId, bool sendLetter, bool createResources, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageWithResources", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/AddPackageWithResourcesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageWithResourcesAsync(int userId, int planId, string spaceName, int statusId, bool sendLetter, bool createResources, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/CreateUserWizard", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/CreateUserWizardResponse")]
        int CreateUserWizard(int parentPackageId, string username, string password, int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail, bool sendAccountLetter, bool createPackage, int planId, bool sendPackageLetter, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/CreateUserWizard", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/CreateUserWizardResponse")]
        System.Threading.Tasks.Task<int> CreateUserWizardAsync(int parentPackageId, string username, string password, int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail, bool sendAccountLetter, bool createPackage, int planId, bool sendPackageLetter, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesBandwidthPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesBandwidthPagedResponse")]
        System.Data.DataSet GetPackagesBandwidthPaged(int userId, int packageId, System.DateTime startDate, System.DateTime endDate, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesBandwidthPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesBandwidthPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetPackagesBandwidthPagedAsync(int userId, int packageId, System.DateTime startDate, System.DateTime endDate, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesDiskspacePaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesDiskspacePagedResponse")]
        System.Data.DataSet GetPackagesDiskspacePaged(int userId, int packageId, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesDiskspacePaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackagesDiskspacePagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetPackagesDiskspacePagedAsync(int userId, int packageId, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageBandwidth", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageBandwidthResponse")]
        System.Data.DataSet GetPackageBandwidth(int packageId, System.DateTime startDate, System.DateTime endDate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageBandwidth", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageBandwidthResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetPackageBandwidthAsync(int packageId, System.DateTime startDate, System.DateTime endDate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageDiskspace", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageDiskspaceResponse")]
        System.Data.DataSet GetPackageDiskspace(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageDiskspace", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetPackageDiskspaceResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetPackageDiskspaceAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetOverusageSummaryReport", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetOverusageSummaryReportResponse")]
        System.Data.DataSet GetOverusageSummaryReport(int userId, int packageId, System.DateTime startDate, System.DateTime endDate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetOverusageSummaryReport", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetOverusageSummaryReportResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetOverusageSummaryReportAsync(int userId, int packageId, System.DateTime startDate, System.DateTime endDate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetDiskspaceOverusageDetailsReport", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetDiskspaceOverusageDetailsReportResponse")]
        System.Data.DataSet GetDiskspaceOverusageDetailsReport(int userId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetDiskspaceOverusageDetailsReport", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetDiskspaceOverusageDetailsReportResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetDiskspaceOverusageDetailsReportAsync(int userId, int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetBandwidthOverusageDetailsReport", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetBandwidthOverusageDetailsReportResponse")]
        System.Data.DataSet GetBandwidthOverusageDetailsReport(int userId, int packageId, System.DateTime startDate, System.DateTime endDate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetBandwidthOverusageDetailsReport", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesPackages/GetBandwidthOverusageDetailsReportResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetBandwidthOverusageDetailsReportAsync(int userId, int packageId, System.DateTime startDate, System.DateTime endDate);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esPackagesAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesPackages
    {
        public System.Data.DataSet GetHostingPlans(int userId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetHostingPlans", userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetHostingPlansAsync(int userId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetHostingPlans", userId);
        }

        public System.Data.DataSet GetHostingAddons(int userId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetHostingAddons", userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetHostingAddonsAsync(int userId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetHostingAddons", userId);
        }

        public SolidCP.EnterpriseServer.HostingPlanInfo GetHostingPlan(int planId)
        {
            return Invoke<SolidCP.EnterpriseServer.HostingPlanInfo>("SolidCP.EnterpriseServer.esPackages", "GetHostingPlan", planId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanInfo> GetHostingPlanAsync(int planId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.HostingPlanInfo>("SolidCP.EnterpriseServer.esPackages", "GetHostingPlan", planId);
        }

        public System.Data.DataSet GetHostingPlanQuotas(int packageId, int planId, int serverId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetHostingPlanQuotas", packageId, planId, serverId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetHostingPlanQuotasAsync(int packageId, int planId, int serverId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetHostingPlanQuotas", packageId, planId, serverId);
        }

        public SolidCP.EnterpriseServer.HostingPlanContext GetHostingPlanContext(int planId)
        {
            return Invoke<SolidCP.EnterpriseServer.HostingPlanContext>("SolidCP.EnterpriseServer.esPackages", "GetHostingPlanContext", planId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanContext> GetHostingPlanContextAsync(int planId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.HostingPlanContext>("SolidCP.EnterpriseServer.esPackages", "GetHostingPlanContext", planId);
        }

        public SolidCP.EnterpriseServer.HostingPlanInfo[] /*List*/ GetUserAvailableHostingPlans(int userId)
        {
            return Invoke<SolidCP.EnterpriseServer.HostingPlanInfo[], SolidCP.EnterpriseServer.HostingPlanInfo>("SolidCP.EnterpriseServer.esPackages", "GetUserAvailableHostingPlans", userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanInfo[]> GetUserAvailableHostingPlansAsync(int userId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.HostingPlanInfo[], SolidCP.EnterpriseServer.HostingPlanInfo>("SolidCP.EnterpriseServer.esPackages", "GetUserAvailableHostingPlans", userId);
        }

        public SolidCP.EnterpriseServer.HostingPlanInfo[] /*List*/ GetUserAvailableHostingAddons(int userId)
        {
            return Invoke<SolidCP.EnterpriseServer.HostingPlanInfo[], SolidCP.EnterpriseServer.HostingPlanInfo>("SolidCP.EnterpriseServer.esPackages", "GetUserAvailableHostingAddons", userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanInfo[]> GetUserAvailableHostingAddonsAsync(int userId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.HostingPlanInfo[], SolidCP.EnterpriseServer.HostingPlanInfo>("SolidCP.EnterpriseServer.esPackages", "GetUserAvailableHostingAddons", userId);
        }

        public int AddHostingPlan(SolidCP.EnterpriseServer.HostingPlanInfo plan)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "AddHostingPlan", plan);
        }

        public async System.Threading.Tasks.Task<int> AddHostingPlanAsync(SolidCP.EnterpriseServer.HostingPlanInfo plan)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "AddHostingPlan", plan);
        }

        public SolidCP.EnterpriseServer.PackageResult UpdateHostingPlan(SolidCP.EnterpriseServer.HostingPlanInfo plan)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "UpdateHostingPlan", plan);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdateHostingPlanAsync(SolidCP.EnterpriseServer.HostingPlanInfo plan)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "UpdateHostingPlan", plan);
        }

        public int DeleteHostingPlan(int planId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "DeleteHostingPlan", planId);
        }

        public async System.Threading.Tasks.Task<int> DeleteHostingPlanAsync(int planId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "DeleteHostingPlan", planId);
        }

        public SolidCP.EnterpriseServer.PackageInfo[] /*List*/ GetPackages(int userId)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageInfo[], SolidCP.EnterpriseServer.PackageInfo>("SolidCP.EnterpriseServer.esPackages", "GetPackages", userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo[]> GetPackagesAsync(int userId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageInfo[], SolidCP.EnterpriseServer.PackageInfo>("SolidCP.EnterpriseServer.esPackages", "GetPackages", userId);
        }

        public System.Data.DataSet GetNestedPackagesSummary(int packageId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetNestedPackagesSummary", packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetNestedPackagesSummaryAsync(int packageId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetNestedPackagesSummary", packageId);
        }

        public System.Data.DataSet GetRawPackages(int userId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetRawPackages", userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackagesAsync(int userId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetRawPackages", userId);
        }

        public System.Data.DataSet SearchServiceItemsPaged(int userId, int itemTypeId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "SearchServiceItemsPaged", userId, itemTypeId, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> SearchServiceItemsPagedAsync(int userId, int itemTypeId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "SearchServiceItemsPaged", userId, itemTypeId, filterValue, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetSearchObject(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows, string colType, string fullType)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetSearchObject", userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows, colType, fullType);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSearchObjectAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows, string colType, string fullType)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetSearchObject", userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows, colType, fullType);
        }

        public System.Data.DataSet GetSearchObjectQuickFind(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int maximumRows, string colType, string fullType)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetSearchObjectQuickFind", userId, filterColumn, filterValue, statusId, roleId, sortColumn, maximumRows, colType, fullType);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSearchObjectQuickFindAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int maximumRows, string colType, string fullType)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetSearchObjectQuickFind", userId, filterColumn, filterValue, statusId, roleId, sortColumn, maximumRows, colType, fullType);
        }

        public System.Data.DataSet GetSearchTableByColumns(string PagedStored, string FilterValue, int MaximumRows, bool Recursive, int PoolID, int ServerID, int StatusID, int PlanID, int OrgID, string ItemTypeName, string GroupName, int PackageID, string VPSType, int RoleID, int UserID, string FilterColumns)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetSearchTableByColumns", PagedStored, FilterValue, MaximumRows, Recursive, PoolID, ServerID, StatusID, PlanID, OrgID, ItemTypeName, GroupName, PackageID, VPSType, RoleID, UserID, FilterColumns);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSearchTableByColumnsAsync(string PagedStored, string FilterValue, int MaximumRows, bool Recursive, int PoolID, int ServerID, int StatusID, int PlanID, int OrgID, string ItemTypeName, string GroupName, int PackageID, string VPSType, int RoleID, int UserID, string FilterColumns)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetSearchTableByColumns", PagedStored, FilterValue, MaximumRows, Recursive, PoolID, ServerID, StatusID, PlanID, OrgID, ItemTypeName, GroupName, PackageID, VPSType, RoleID, UserID, FilterColumns);
        }

        public System.Data.DataSet GetPackagesPaged(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackagesPaged", userId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackagesPagedAsync(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackagesPaged", userId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetNestedPackagesPaged(int packageId, string filterColumn, string filterValue, int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetNestedPackagesPaged", packageId, filterColumn, filterValue, statusId, planId, serverId, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetNestedPackagesPagedAsync(int packageId, string filterColumn, string filterValue, int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetNestedPackagesPaged", packageId, filterColumn, filterValue, statusId, planId, serverId, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.PackageInfo[] /*List*/ GetPackagePackages(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageInfo[], SolidCP.EnterpriseServer.PackageInfo>("SolidCP.EnterpriseServer.esPackages", "GetPackagePackages", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo[]> GetPackagePackagesAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageInfo[], SolidCP.EnterpriseServer.PackageInfo>("SolidCP.EnterpriseServer.esPackages", "GetPackagePackages", packageId);
        }

        public SolidCP.EnterpriseServer.PackageInfo[] /*List*/ GetMyPackages(int userId)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageInfo[], SolidCP.EnterpriseServer.PackageInfo>("SolidCP.EnterpriseServer.esPackages", "GetMyPackages", userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo[]> GetMyPackagesAsync(int userId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageInfo[], SolidCP.EnterpriseServer.PackageInfo>("SolidCP.EnterpriseServer.esPackages", "GetMyPackages", userId);
        }

        public System.Data.DataSet GetRawMyPackages(int userId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetRawMyPackages", userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMyPackagesAsync(int userId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetRawMyPackages", userId);
        }

        public SolidCP.EnterpriseServer.PackageInfo GetPackage(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageInfo>("SolidCP.EnterpriseServer.esPackages", "GetPackage", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo> GetPackageAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageInfo>("SolidCP.EnterpriseServer.esPackages", "GetPackage", packageId);
        }

        public SolidCP.EnterpriseServer.PackageContext GetPackageContext(int packageId)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageContext>("SolidCP.EnterpriseServer.esPackages", "GetPackageContext", packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageContext> GetPackageContextAsync(int packageId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageContext>("SolidCP.EnterpriseServer.esPackages", "GetPackageContext", packageId);
        }

        public System.Data.DataSet GetPackageQuotas(int packageId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackageQuotas", packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackageQuotasAsync(int packageId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackageQuotas", packageId);
        }

        public System.Data.DataSet GetPackageQuotasForEdit(int packageId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackageQuotasForEdit", packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackageQuotasForEditAsync(int packageId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackageQuotasForEdit", packageId);
        }

        public SolidCP.EnterpriseServer.PackageResult AddPackage(int userId, int planId, string packageName, string packageComments, int statusId, System.DateTime purchaseDate)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "AddPackage", userId, planId, packageName, packageComments, statusId, purchaseDate);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAsync(int userId, int planId, string packageName, string packageComments, int statusId, System.DateTime purchaseDate)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "AddPackage", userId, planId, packageName, packageComments, statusId, purchaseDate);
        }

        public SolidCP.EnterpriseServer.PackageResult UpdatePackage(SolidCP.EnterpriseServer.PackageInfo package)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "UpdatePackage", package);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageAsync(SolidCP.EnterpriseServer.PackageInfo package)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "UpdatePackage", package);
        }

        public SolidCP.EnterpriseServer.PackageResult UpdatePackageLiteral(int packageId, int statusId, int planId, System.DateTime purchaseDate, string packageName, string packageComments)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "UpdatePackageLiteral", packageId, statusId, planId, purchaseDate, packageName, packageComments);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageLiteralAsync(int packageId, int statusId, int planId, System.DateTime purchaseDate, string packageName, string packageComments)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "UpdatePackageLiteral", packageId, statusId, planId, purchaseDate, packageName, packageComments);
        }

        public int ChangePackageUser(int packageId, int UserId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "ChangePackageUser", packageId, UserId);
        }

        public async System.Threading.Tasks.Task<int> ChangePackageUserAsync(int packageId, int UserId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "ChangePackageUser", packageId, UserId);
        }

        public int UpdatePackageName(int packageId, string packageName, string packageComments)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "UpdatePackageName", packageId, packageName, packageComments);
        }

        public async System.Threading.Tasks.Task<int> UpdatePackageNameAsync(int packageId, string packageName, string packageComments)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "UpdatePackageName", packageId, packageName, packageComments);
        }

        public int DeletePackage(int packageId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "DeletePackage", packageId);
        }

        public async System.Threading.Tasks.Task<int> DeletePackageAsync(int packageId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "DeletePackage", packageId);
        }

        public int ChangePackageStatus(int packageId, SolidCP.EnterpriseServer.PackageStatus status)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "ChangePackageStatus", packageId, status);
        }

        public async System.Threading.Tasks.Task<int> ChangePackageStatusAsync(int packageId, SolidCP.EnterpriseServer.PackageStatus status)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "ChangePackageStatus", packageId, status);
        }

        public string EvaluateUserPackageTempate(int userId, int packageId, string template)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esPackages", "EvaluateUserPackageTempate", userId, packageId, template);
        }

        public async System.Threading.Tasks.Task<string> EvaluateUserPackageTempateAsync(int userId, int packageId, string template)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esPackages", "EvaluateUserPackageTempate", userId, packageId, template);
        }

        public SolidCP.EnterpriseServer.PackageSettings GetPackageSettings(int packageId, string settingsName)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageSettings>("SolidCP.EnterpriseServer.esPackages", "GetPackageSettings", packageId, settingsName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageSettings> GetPackageSettingsAsync(int packageId, string settingsName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageSettings>("SolidCP.EnterpriseServer.esPackages", "GetPackageSettings", packageId, settingsName);
        }

        public int UpdatePackageSettings(SolidCP.EnterpriseServer.PackageSettings settings)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "UpdatePackageSettings", settings);
        }

        public async System.Threading.Tasks.Task<int> UpdatePackageSettingsAsync(SolidCP.EnterpriseServer.PackageSettings settings)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "UpdatePackageSettings", settings);
        }

        public bool SetDefaultTopPackage(int userId, int packageId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esPackages", "SetDefaultTopPackage", userId, packageId);
        }

        public async System.Threading.Tasks.Task<bool> SetDefaultTopPackageAsync(int userId, int packageId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esPackages", "SetDefaultTopPackage", userId, packageId);
        }

        public System.Data.DataSet GetPackageAddons(int packageId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackageAddons", packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackageAddonsAsync(int packageId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackageAddons", packageId);
        }

        public SolidCP.EnterpriseServer.PackageAddonInfo GetPackageAddon(int packageAddonId)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageAddonInfo>("SolidCP.EnterpriseServer.esPackages", "GetPackageAddon", packageAddonId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageAddonInfo> GetPackageAddonAsync(int packageAddonId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageAddonInfo>("SolidCP.EnterpriseServer.esPackages", "GetPackageAddon", packageAddonId);
        }

        public SolidCP.EnterpriseServer.PackageResult AddPackageAddonById(int packageId, int addonPlanId, int quantity)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "AddPackageAddonById", packageId, addonPlanId, quantity);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAddonByIdAsync(int packageId, int addonPlanId, int quantity)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "AddPackageAddonById", packageId, addonPlanId, quantity);
        }

        public SolidCP.EnterpriseServer.PackageResult AddPackageAddon(SolidCP.EnterpriseServer.PackageAddonInfo addon)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "AddPackageAddon", addon);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAddonAsync(SolidCP.EnterpriseServer.PackageAddonInfo addon)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "AddPackageAddon", addon);
        }

        public SolidCP.EnterpriseServer.PackageResult AddPackageAddonLiteral(int packageId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "AddPackageAddonLiteral", packageId, planId, quantity, statusId, purchaseDate, comments);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAddonLiteralAsync(int packageId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "AddPackageAddonLiteral", packageId, planId, quantity, statusId, purchaseDate, comments);
        }

        public SolidCP.EnterpriseServer.PackageResult UpdatePackageAddon(SolidCP.EnterpriseServer.PackageAddonInfo addon)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "UpdatePackageAddon", addon);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageAddonAsync(SolidCP.EnterpriseServer.PackageAddonInfo addon)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "UpdatePackageAddon", addon);
        }

        public SolidCP.EnterpriseServer.PackageResult UpdatePackageAddonLiteral(int packageAddonId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "UpdatePackageAddonLiteral", packageAddonId, planId, quantity, statusId, purchaseDate, comments);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageAddonLiteralAsync(int packageAddonId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "UpdatePackageAddonLiteral", packageAddonId, planId, quantity, statusId, purchaseDate, comments);
        }

        public int DeletePackageAddon(int packageAddonId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "DeletePackageAddon", packageAddonId);
        }

        public async System.Threading.Tasks.Task<int> DeletePackageAddonAsync(int packageAddonId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "DeletePackageAddon", packageAddonId);
        }

        public System.Data.DataSet GetSearchableServiceItemTypes()
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetSearchableServiceItemTypes");
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSearchableServiceItemTypesAsync()
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetSearchableServiceItemTypes");
        }

        public System.Data.DataSet GetRawPackageItemsByType(int packageId, string itemTypeName, bool recursive)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetRawPackageItemsByType", packageId, itemTypeName, recursive);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackageItemsByTypeAsync(int packageId, string itemTypeName, bool recursive)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetRawPackageItemsByType", packageId, itemTypeName, recursive);
        }

        public System.Data.DataSet GetRawPackageItemsPaged(int packageId, string groupName, string typeName, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetRawPackageItemsPaged", packageId, groupName, typeName, serverId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackageItemsPagedAsync(int packageId, string groupName, string typeName, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetRawPackageItemsPaged", packageId, groupName, typeName, serverId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetRawPackageItems(int packageId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetRawPackageItems", packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackageItemsAsync(int packageId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetRawPackageItems", packageId);
        }

        public int DetachPackageItem(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "DetachPackageItem", itemId);
        }

        public async System.Threading.Tasks.Task<int> DetachPackageItemAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "DetachPackageItem", itemId);
        }

        public int MovePackageItem(int itemId, int destinationServiceId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "MovePackageItem", itemId, destinationServiceId);
        }

        public async System.Threading.Tasks.Task<int> MovePackageItemAsync(int itemId, int destinationServiceId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "MovePackageItem", itemId, destinationServiceId);
        }

        public SolidCP.EnterpriseServer.QuotaValueInfo GetPackageQuota(int packageId, string quotaName)
        {
            return Invoke<SolidCP.EnterpriseServer.QuotaValueInfo>("SolidCP.EnterpriseServer.esPackages", "GetPackageQuota", packageId, quotaName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.QuotaValueInfo> GetPackageQuotaAsync(int packageId, string quotaName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.QuotaValueInfo>("SolidCP.EnterpriseServer.esPackages", "GetPackageQuota", packageId, quotaName);
        }

        public int SendAccountSummaryLetter(int userId, string to, string cc)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "SendAccountSummaryLetter", userId, to, cc);
        }

        public async System.Threading.Tasks.Task<int> SendAccountSummaryLetterAsync(int userId, string to, string cc)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "SendAccountSummaryLetter", userId, to, cc);
        }

        public int SendPackageSummaryLetter(int packageId, string to, string cc)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "SendPackageSummaryLetter", packageId, to, cc);
        }

        public async System.Threading.Tasks.Task<int> SendPackageSummaryLetterAsync(int packageId, string to, string cc)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "SendPackageSummaryLetter", packageId, to, cc);
        }

        public string GetEvaluatedPackageTemplateBody(int packageId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esPackages", "GetEvaluatedPackageTemplateBody", packageId);
        }

        public async System.Threading.Tasks.Task<string> GetEvaluatedPackageTemplateBodyAsync(int packageId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esPackages", "GetEvaluatedPackageTemplateBody", packageId);
        }

        public string GetEvaluatedAccountTemplateBody(int userId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esPackages", "GetEvaluatedAccountTemplateBody", userId);
        }

        public async System.Threading.Tasks.Task<string> GetEvaluatedAccountTemplateBodyAsync(int userId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esPackages", "GetEvaluatedAccountTemplateBody", userId);
        }

        public SolidCP.EnterpriseServer.PackageResult AddPackageWithResources(int userId, int planId, string spaceName, int statusId, bool sendLetter, bool createResources, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName)
        {
            return Invoke<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "AddPackageWithResources", userId, planId, spaceName, statusId, sendLetter, createResources, domainName, tempDomain, createWebSite, createFtpAccount, ftpAccountName, createMailAccount, hostName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageWithResourcesAsync(int userId, int planId, string spaceName, int statusId, bool sendLetter, bool createResources, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.PackageResult>("SolidCP.EnterpriseServer.esPackages", "AddPackageWithResources", userId, planId, spaceName, statusId, sendLetter, createResources, domainName, tempDomain, createWebSite, createFtpAccount, ftpAccountName, createMailAccount, hostName);
        }

        public int CreateUserWizard(int parentPackageId, string username, string password, int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail, bool sendAccountLetter, bool createPackage, int planId, bool sendPackageLetter, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esPackages", "CreateUserWizard", parentPackageId, username, password, roleId, firstName, lastName, email, secondaryEmail, htmlMail, sendAccountLetter, createPackage, planId, sendPackageLetter, domainName, tempDomain, createWebSite, createFtpAccount, ftpAccountName, createMailAccount, hostName, createZoneRecord);
        }

        public async System.Threading.Tasks.Task<int> CreateUserWizardAsync(int parentPackageId, string username, string password, int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail, bool sendAccountLetter, bool createPackage, int planId, bool sendPackageLetter, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esPackages", "CreateUserWizard", parentPackageId, username, password, roleId, firstName, lastName, email, secondaryEmail, htmlMail, sendAccountLetter, createPackage, planId, sendPackageLetter, domainName, tempDomain, createWebSite, createFtpAccount, ftpAccountName, createMailAccount, hostName, createZoneRecord);
        }

        public System.Data.DataSet GetPackagesBandwidthPaged(int userId, int packageId, System.DateTime startDate, System.DateTime endDate, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackagesBandwidthPaged", userId, packageId, startDate, endDate, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackagesBandwidthPagedAsync(int userId, int packageId, System.DateTime startDate, System.DateTime endDate, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackagesBandwidthPaged", userId, packageId, startDate, endDate, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetPackagesDiskspacePaged(int userId, int packageId, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackagesDiskspacePaged", userId, packageId, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackagesDiskspacePagedAsync(int userId, int packageId, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackagesDiskspacePaged", userId, packageId, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetPackageBandwidth(int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackageBandwidth", packageId, startDate, endDate);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackageBandwidthAsync(int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackageBandwidth", packageId, startDate, endDate);
        }

        public System.Data.DataSet GetPackageDiskspace(int packageId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackageDiskspace", packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackageDiskspaceAsync(int packageId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetPackageDiskspace", packageId);
        }

        public System.Data.DataSet GetOverusageSummaryReport(int userId, int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetOverusageSummaryReport", userId, packageId, startDate, endDate);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetOverusageSummaryReportAsync(int userId, int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetOverusageSummaryReport", userId, packageId, startDate, endDate);
        }

        public System.Data.DataSet GetDiskspaceOverusageDetailsReport(int userId, int packageId)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetDiskspaceOverusageDetailsReport", userId, packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetDiskspaceOverusageDetailsReportAsync(int userId, int packageId)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetDiskspaceOverusageDetailsReport", userId, packageId);
        }

        public System.Data.DataSet GetBandwidthOverusageDetailsReport(int userId, int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetBandwidthOverusageDetailsReport", userId, packageId, startDate, endDate);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetBandwidthOverusageDetailsReportAsync(int userId, int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esPackages", "GetBandwidthOverusageDetailsReport", userId, packageId, startDate, endDate);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esPackages : SolidCP.Web.Client.ClientBase<IesPackages, esPackagesAssemblyClient>, IesPackages
    {
        public System.Data.DataSet GetHostingPlans(int userId)
        {
            return base.Client.GetHostingPlans(userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetHostingPlansAsync(int userId)
        {
            return await base.Client.GetHostingPlansAsync(userId);
        }

        public System.Data.DataSet GetHostingAddons(int userId)
        {
            return base.Client.GetHostingAddons(userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetHostingAddonsAsync(int userId)
        {
            return await base.Client.GetHostingAddonsAsync(userId);
        }

        public SolidCP.EnterpriseServer.HostingPlanInfo GetHostingPlan(int planId)
        {
            return base.Client.GetHostingPlan(planId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanInfo> GetHostingPlanAsync(int planId)
        {
            return await base.Client.GetHostingPlanAsync(planId);
        }

        public System.Data.DataSet GetHostingPlanQuotas(int packageId, int planId, int serverId)
        {
            return base.Client.GetHostingPlanQuotas(packageId, planId, serverId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetHostingPlanQuotasAsync(int packageId, int planId, int serverId)
        {
            return await base.Client.GetHostingPlanQuotasAsync(packageId, planId, serverId);
        }

        public SolidCP.EnterpriseServer.HostingPlanContext GetHostingPlanContext(int planId)
        {
            return base.Client.GetHostingPlanContext(planId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanContext> GetHostingPlanContextAsync(int planId)
        {
            return await base.Client.GetHostingPlanContextAsync(planId);
        }

        public SolidCP.EnterpriseServer.HostingPlanInfo[] /*List*/ GetUserAvailableHostingPlans(int userId)
        {
            return base.Client.GetUserAvailableHostingPlans(userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanInfo[]> GetUserAvailableHostingPlansAsync(int userId)
        {
            return await base.Client.GetUserAvailableHostingPlansAsync(userId);
        }

        public SolidCP.EnterpriseServer.HostingPlanInfo[] /*List*/ GetUserAvailableHostingAddons(int userId)
        {
            return base.Client.GetUserAvailableHostingAddons(userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.HostingPlanInfo[]> GetUserAvailableHostingAddonsAsync(int userId)
        {
            return await base.Client.GetUserAvailableHostingAddonsAsync(userId);
        }

        public int AddHostingPlan(SolidCP.EnterpriseServer.HostingPlanInfo plan)
        {
            return base.Client.AddHostingPlan(plan);
        }

        public async System.Threading.Tasks.Task<int> AddHostingPlanAsync(SolidCP.EnterpriseServer.HostingPlanInfo plan)
        {
            return await base.Client.AddHostingPlanAsync(plan);
        }

        public SolidCP.EnterpriseServer.PackageResult UpdateHostingPlan(SolidCP.EnterpriseServer.HostingPlanInfo plan)
        {
            return base.Client.UpdateHostingPlan(plan);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdateHostingPlanAsync(SolidCP.EnterpriseServer.HostingPlanInfo plan)
        {
            return await base.Client.UpdateHostingPlanAsync(plan);
        }

        public int DeleteHostingPlan(int planId)
        {
            return base.Client.DeleteHostingPlan(planId);
        }

        public async System.Threading.Tasks.Task<int> DeleteHostingPlanAsync(int planId)
        {
            return await base.Client.DeleteHostingPlanAsync(planId);
        }

        public SolidCP.EnterpriseServer.PackageInfo[] /*List*/ GetPackages(int userId)
        {
            return base.Client.GetPackages(userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo[]> GetPackagesAsync(int userId)
        {
            return await base.Client.GetPackagesAsync(userId);
        }

        public System.Data.DataSet GetNestedPackagesSummary(int packageId)
        {
            return base.Client.GetNestedPackagesSummary(packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetNestedPackagesSummaryAsync(int packageId)
        {
            return await base.Client.GetNestedPackagesSummaryAsync(packageId);
        }

        public System.Data.DataSet GetRawPackages(int userId)
        {
            return base.Client.GetRawPackages(userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackagesAsync(int userId)
        {
            return await base.Client.GetRawPackagesAsync(userId);
        }

        public System.Data.DataSet SearchServiceItemsPaged(int userId, int itemTypeId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.SearchServiceItemsPaged(userId, itemTypeId, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> SearchServiceItemsPagedAsync(int userId, int itemTypeId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.SearchServiceItemsPagedAsync(userId, itemTypeId, filterValue, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetSearchObject(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows, string colType, string fullType)
        {
            return base.Client.GetSearchObject(userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows, colType, fullType);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSearchObjectAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows, string colType, string fullType)
        {
            return await base.Client.GetSearchObjectAsync(userId, filterColumn, filterValue, statusId, roleId, sortColumn, startRow, maximumRows, colType, fullType);
        }

        public System.Data.DataSet GetSearchObjectQuickFind(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int maximumRows, string colType, string fullType)
        {
            return base.Client.GetSearchObjectQuickFind(userId, filterColumn, filterValue, statusId, roleId, sortColumn, maximumRows, colType, fullType);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSearchObjectQuickFindAsync(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int maximumRows, string colType, string fullType)
        {
            return await base.Client.GetSearchObjectQuickFindAsync(userId, filterColumn, filterValue, statusId, roleId, sortColumn, maximumRows, colType, fullType);
        }

        public System.Data.DataSet GetSearchTableByColumns(string PagedStored, string FilterValue, int MaximumRows, bool Recursive, int PoolID, int ServerID, int StatusID, int PlanID, int OrgID, string ItemTypeName, string GroupName, int PackageID, string VPSType, int RoleID, int UserID, string FilterColumns)
        {
            return base.Client.GetSearchTableByColumns(PagedStored, FilterValue, MaximumRows, Recursive, PoolID, ServerID, StatusID, PlanID, OrgID, ItemTypeName, GroupName, PackageID, VPSType, RoleID, UserID, FilterColumns);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSearchTableByColumnsAsync(string PagedStored, string FilterValue, int MaximumRows, bool Recursive, int PoolID, int ServerID, int StatusID, int PlanID, int OrgID, string ItemTypeName, string GroupName, int PackageID, string VPSType, int RoleID, int UserID, string FilterColumns)
        {
            return await base.Client.GetSearchTableByColumnsAsync(PagedStored, FilterValue, MaximumRows, Recursive, PoolID, ServerID, StatusID, PlanID, OrgID, ItemTypeName, GroupName, PackageID, VPSType, RoleID, UserID, FilterColumns);
        }

        public System.Data.DataSet GetPackagesPaged(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetPackagesPaged(userId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackagesPagedAsync(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetPackagesPagedAsync(userId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetNestedPackagesPaged(int packageId, string filterColumn, string filterValue, int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetNestedPackagesPaged(packageId, filterColumn, filterValue, statusId, planId, serverId, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetNestedPackagesPagedAsync(int packageId, string filterColumn, string filterValue, int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetNestedPackagesPagedAsync(packageId, filterColumn, filterValue, statusId, planId, serverId, sortColumn, startRow, maximumRows);
        }

        public SolidCP.EnterpriseServer.PackageInfo[] /*List*/ GetPackagePackages(int packageId)
        {
            return base.Client.GetPackagePackages(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo[]> GetPackagePackagesAsync(int packageId)
        {
            return await base.Client.GetPackagePackagesAsync(packageId);
        }

        public SolidCP.EnterpriseServer.PackageInfo[] /*List*/ GetMyPackages(int userId)
        {
            return base.Client.GetMyPackages(userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo[]> GetMyPackagesAsync(int userId)
        {
            return await base.Client.GetMyPackagesAsync(userId);
        }

        public System.Data.DataSet GetRawMyPackages(int userId)
        {
            return base.Client.GetRawMyPackages(userId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawMyPackagesAsync(int userId)
        {
            return await base.Client.GetRawMyPackagesAsync(userId);
        }

        public SolidCP.EnterpriseServer.PackageInfo GetPackage(int packageId)
        {
            return base.Client.GetPackage(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageInfo> GetPackageAsync(int packageId)
        {
            return await base.Client.GetPackageAsync(packageId);
        }

        public SolidCP.EnterpriseServer.PackageContext GetPackageContext(int packageId)
        {
            return base.Client.GetPackageContext(packageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageContext> GetPackageContextAsync(int packageId)
        {
            return await base.Client.GetPackageContextAsync(packageId);
        }

        public System.Data.DataSet GetPackageQuotas(int packageId)
        {
            return base.Client.GetPackageQuotas(packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackageQuotasAsync(int packageId)
        {
            return await base.Client.GetPackageQuotasAsync(packageId);
        }

        public System.Data.DataSet GetPackageQuotasForEdit(int packageId)
        {
            return base.Client.GetPackageQuotasForEdit(packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackageQuotasForEditAsync(int packageId)
        {
            return await base.Client.GetPackageQuotasForEditAsync(packageId);
        }

        public SolidCP.EnterpriseServer.PackageResult AddPackage(int userId, int planId, string packageName, string packageComments, int statusId, System.DateTime purchaseDate)
        {
            return base.Client.AddPackage(userId, planId, packageName, packageComments, statusId, purchaseDate);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAsync(int userId, int planId, string packageName, string packageComments, int statusId, System.DateTime purchaseDate)
        {
            return await base.Client.AddPackageAsync(userId, planId, packageName, packageComments, statusId, purchaseDate);
        }

        public SolidCP.EnterpriseServer.PackageResult UpdatePackage(SolidCP.EnterpriseServer.PackageInfo package)
        {
            return base.Client.UpdatePackage(package);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageAsync(SolidCP.EnterpriseServer.PackageInfo package)
        {
            return await base.Client.UpdatePackageAsync(package);
        }

        public SolidCP.EnterpriseServer.PackageResult UpdatePackageLiteral(int packageId, int statusId, int planId, System.DateTime purchaseDate, string packageName, string packageComments)
        {
            return base.Client.UpdatePackageLiteral(packageId, statusId, planId, purchaseDate, packageName, packageComments);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageLiteralAsync(int packageId, int statusId, int planId, System.DateTime purchaseDate, string packageName, string packageComments)
        {
            return await base.Client.UpdatePackageLiteralAsync(packageId, statusId, planId, purchaseDate, packageName, packageComments);
        }

        public int ChangePackageUser(int packageId, int UserId)
        {
            return base.Client.ChangePackageUser(packageId, UserId);
        }

        public async System.Threading.Tasks.Task<int> ChangePackageUserAsync(int packageId, int UserId)
        {
            return await base.Client.ChangePackageUserAsync(packageId, UserId);
        }

        public int UpdatePackageName(int packageId, string packageName, string packageComments)
        {
            return base.Client.UpdatePackageName(packageId, packageName, packageComments);
        }

        public async System.Threading.Tasks.Task<int> UpdatePackageNameAsync(int packageId, string packageName, string packageComments)
        {
            return await base.Client.UpdatePackageNameAsync(packageId, packageName, packageComments);
        }

        public int DeletePackage(int packageId)
        {
            return base.Client.DeletePackage(packageId);
        }

        public async System.Threading.Tasks.Task<int> DeletePackageAsync(int packageId)
        {
            return await base.Client.DeletePackageAsync(packageId);
        }

        public int ChangePackageStatus(int packageId, SolidCP.EnterpriseServer.PackageStatus status)
        {
            return base.Client.ChangePackageStatus(packageId, status);
        }

        public async System.Threading.Tasks.Task<int> ChangePackageStatusAsync(int packageId, SolidCP.EnterpriseServer.PackageStatus status)
        {
            return await base.Client.ChangePackageStatusAsync(packageId, status);
        }

        public string EvaluateUserPackageTempate(int userId, int packageId, string template)
        {
            return base.Client.EvaluateUserPackageTempate(userId, packageId, template);
        }

        public async System.Threading.Tasks.Task<string> EvaluateUserPackageTempateAsync(int userId, int packageId, string template)
        {
            return await base.Client.EvaluateUserPackageTempateAsync(userId, packageId, template);
        }

        public SolidCP.EnterpriseServer.PackageSettings GetPackageSettings(int packageId, string settingsName)
        {
            return base.Client.GetPackageSettings(packageId, settingsName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageSettings> GetPackageSettingsAsync(int packageId, string settingsName)
        {
            return await base.Client.GetPackageSettingsAsync(packageId, settingsName);
        }

        public int UpdatePackageSettings(SolidCP.EnterpriseServer.PackageSettings settings)
        {
            return base.Client.UpdatePackageSettings(settings);
        }

        public async System.Threading.Tasks.Task<int> UpdatePackageSettingsAsync(SolidCP.EnterpriseServer.PackageSettings settings)
        {
            return await base.Client.UpdatePackageSettingsAsync(settings);
        }

        public bool SetDefaultTopPackage(int userId, int packageId)
        {
            return base.Client.SetDefaultTopPackage(userId, packageId);
        }

        public async System.Threading.Tasks.Task<bool> SetDefaultTopPackageAsync(int userId, int packageId)
        {
            return await base.Client.SetDefaultTopPackageAsync(userId, packageId);
        }

        public System.Data.DataSet GetPackageAddons(int packageId)
        {
            return base.Client.GetPackageAddons(packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackageAddonsAsync(int packageId)
        {
            return await base.Client.GetPackageAddonsAsync(packageId);
        }

        public SolidCP.EnterpriseServer.PackageAddonInfo GetPackageAddon(int packageAddonId)
        {
            return base.Client.GetPackageAddon(packageAddonId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageAddonInfo> GetPackageAddonAsync(int packageAddonId)
        {
            return await base.Client.GetPackageAddonAsync(packageAddonId);
        }

        public SolidCP.EnterpriseServer.PackageResult AddPackageAddonById(int packageId, int addonPlanId, int quantity)
        {
            return base.Client.AddPackageAddonById(packageId, addonPlanId, quantity);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAddonByIdAsync(int packageId, int addonPlanId, int quantity)
        {
            return await base.Client.AddPackageAddonByIdAsync(packageId, addonPlanId, quantity);
        }

        public SolidCP.EnterpriseServer.PackageResult AddPackageAddon(SolidCP.EnterpriseServer.PackageAddonInfo addon)
        {
            return base.Client.AddPackageAddon(addon);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAddonAsync(SolidCP.EnterpriseServer.PackageAddonInfo addon)
        {
            return await base.Client.AddPackageAddonAsync(addon);
        }

        public SolidCP.EnterpriseServer.PackageResult AddPackageAddonLiteral(int packageId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments)
        {
            return base.Client.AddPackageAddonLiteral(packageId, planId, quantity, statusId, purchaseDate, comments);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageAddonLiteralAsync(int packageId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments)
        {
            return await base.Client.AddPackageAddonLiteralAsync(packageId, planId, quantity, statusId, purchaseDate, comments);
        }

        public SolidCP.EnterpriseServer.PackageResult UpdatePackageAddon(SolidCP.EnterpriseServer.PackageAddonInfo addon)
        {
            return base.Client.UpdatePackageAddon(addon);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageAddonAsync(SolidCP.EnterpriseServer.PackageAddonInfo addon)
        {
            return await base.Client.UpdatePackageAddonAsync(addon);
        }

        public SolidCP.EnterpriseServer.PackageResult UpdatePackageAddonLiteral(int packageAddonId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments)
        {
            return base.Client.UpdatePackageAddonLiteral(packageAddonId, planId, quantity, statusId, purchaseDate, comments);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> UpdatePackageAddonLiteralAsync(int packageAddonId, int planId, int quantity, int statusId, System.DateTime purchaseDate, string comments)
        {
            return await base.Client.UpdatePackageAddonLiteralAsync(packageAddonId, planId, quantity, statusId, purchaseDate, comments);
        }

        public int DeletePackageAddon(int packageAddonId)
        {
            return base.Client.DeletePackageAddon(packageAddonId);
        }

        public async System.Threading.Tasks.Task<int> DeletePackageAddonAsync(int packageAddonId)
        {
            return await base.Client.DeletePackageAddonAsync(packageAddonId);
        }

        public System.Data.DataSet GetSearchableServiceItemTypes()
        {
            return base.Client.GetSearchableServiceItemTypes();
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetSearchableServiceItemTypesAsync()
        {
            return await base.Client.GetSearchableServiceItemTypesAsync();
        }

        public System.Data.DataSet GetRawPackageItemsByType(int packageId, string itemTypeName, bool recursive)
        {
            return base.Client.GetRawPackageItemsByType(packageId, itemTypeName, recursive);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackageItemsByTypeAsync(int packageId, string itemTypeName, bool recursive)
        {
            return await base.Client.GetRawPackageItemsByTypeAsync(packageId, itemTypeName, recursive);
        }

        public System.Data.DataSet GetRawPackageItemsPaged(int packageId, string groupName, string typeName, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawPackageItemsPaged(packageId, groupName, typeName, serverId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackageItemsPagedAsync(int packageId, string groupName, string typeName, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawPackageItemsPagedAsync(packageId, groupName, typeName, serverId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetRawPackageItems(int packageId)
        {
            return base.Client.GetRawPackageItems(packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawPackageItemsAsync(int packageId)
        {
            return await base.Client.GetRawPackageItemsAsync(packageId);
        }

        public int DetachPackageItem(int itemId)
        {
            return base.Client.DetachPackageItem(itemId);
        }

        public async System.Threading.Tasks.Task<int> DetachPackageItemAsync(int itemId)
        {
            return await base.Client.DetachPackageItemAsync(itemId);
        }

        public int MovePackageItem(int itemId, int destinationServiceId)
        {
            return base.Client.MovePackageItem(itemId, destinationServiceId);
        }

        public async System.Threading.Tasks.Task<int> MovePackageItemAsync(int itemId, int destinationServiceId)
        {
            return await base.Client.MovePackageItemAsync(itemId, destinationServiceId);
        }

        public SolidCP.EnterpriseServer.QuotaValueInfo GetPackageQuota(int packageId, string quotaName)
        {
            return base.Client.GetPackageQuota(packageId, quotaName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.QuotaValueInfo> GetPackageQuotaAsync(int packageId, string quotaName)
        {
            return await base.Client.GetPackageQuotaAsync(packageId, quotaName);
        }

        public int SendAccountSummaryLetter(int userId, string to, string cc)
        {
            return base.Client.SendAccountSummaryLetter(userId, to, cc);
        }

        public async System.Threading.Tasks.Task<int> SendAccountSummaryLetterAsync(int userId, string to, string cc)
        {
            return await base.Client.SendAccountSummaryLetterAsync(userId, to, cc);
        }

        public int SendPackageSummaryLetter(int packageId, string to, string cc)
        {
            return base.Client.SendPackageSummaryLetter(packageId, to, cc);
        }

        public async System.Threading.Tasks.Task<int> SendPackageSummaryLetterAsync(int packageId, string to, string cc)
        {
            return await base.Client.SendPackageSummaryLetterAsync(packageId, to, cc);
        }

        public string GetEvaluatedPackageTemplateBody(int packageId)
        {
            return base.Client.GetEvaluatedPackageTemplateBody(packageId);
        }

        public async System.Threading.Tasks.Task<string> GetEvaluatedPackageTemplateBodyAsync(int packageId)
        {
            return await base.Client.GetEvaluatedPackageTemplateBodyAsync(packageId);
        }

        public string GetEvaluatedAccountTemplateBody(int userId)
        {
            return base.Client.GetEvaluatedAccountTemplateBody(userId);
        }

        public async System.Threading.Tasks.Task<string> GetEvaluatedAccountTemplateBodyAsync(int userId)
        {
            return await base.Client.GetEvaluatedAccountTemplateBodyAsync(userId);
        }

        public SolidCP.EnterpriseServer.PackageResult AddPackageWithResources(int userId, int planId, string spaceName, int statusId, bool sendLetter, bool createResources, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName)
        {
            return base.Client.AddPackageWithResources(userId, planId, spaceName, statusId, sendLetter, createResources, domainName, tempDomain, createWebSite, createFtpAccount, ftpAccountName, createMailAccount, hostName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.PackageResult> AddPackageWithResourcesAsync(int userId, int planId, string spaceName, int statusId, bool sendLetter, bool createResources, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName)
        {
            return await base.Client.AddPackageWithResourcesAsync(userId, planId, spaceName, statusId, sendLetter, createResources, domainName, tempDomain, createWebSite, createFtpAccount, ftpAccountName, createMailAccount, hostName);
        }

        public int CreateUserWizard(int parentPackageId, string username, string password, int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail, bool sendAccountLetter, bool createPackage, int planId, bool sendPackageLetter, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord)
        {
            return base.Client.CreateUserWizard(parentPackageId, username, password, roleId, firstName, lastName, email, secondaryEmail, htmlMail, sendAccountLetter, createPackage, planId, sendPackageLetter, domainName, tempDomain, createWebSite, createFtpAccount, ftpAccountName, createMailAccount, hostName, createZoneRecord);
        }

        public async System.Threading.Tasks.Task<int> CreateUserWizardAsync(int parentPackageId, string username, string password, int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail, bool sendAccountLetter, bool createPackage, int planId, bool sendPackageLetter, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord)
        {
            return await base.Client.CreateUserWizardAsync(parentPackageId, username, password, roleId, firstName, lastName, email, secondaryEmail, htmlMail, sendAccountLetter, createPackage, planId, sendPackageLetter, domainName, tempDomain, createWebSite, createFtpAccount, ftpAccountName, createMailAccount, hostName, createZoneRecord);
        }

        public System.Data.DataSet GetPackagesBandwidthPaged(int userId, int packageId, System.DateTime startDate, System.DateTime endDate, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetPackagesBandwidthPaged(userId, packageId, startDate, endDate, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackagesBandwidthPagedAsync(int userId, int packageId, System.DateTime startDate, System.DateTime endDate, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetPackagesBandwidthPagedAsync(userId, packageId, startDate, endDate, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetPackagesDiskspacePaged(int userId, int packageId, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetPackagesDiskspacePaged(userId, packageId, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackagesDiskspacePagedAsync(int userId, int packageId, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetPackagesDiskspacePagedAsync(userId, packageId, sortColumn, startRow, maximumRows);
        }

        public System.Data.DataSet GetPackageBandwidth(int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return base.Client.GetPackageBandwidth(packageId, startDate, endDate);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackageBandwidthAsync(int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return await base.Client.GetPackageBandwidthAsync(packageId, startDate, endDate);
        }

        public System.Data.DataSet GetPackageDiskspace(int packageId)
        {
            return base.Client.GetPackageDiskspace(packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetPackageDiskspaceAsync(int packageId)
        {
            return await base.Client.GetPackageDiskspaceAsync(packageId);
        }

        public System.Data.DataSet GetOverusageSummaryReport(int userId, int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return base.Client.GetOverusageSummaryReport(userId, packageId, startDate, endDate);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetOverusageSummaryReportAsync(int userId, int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return await base.Client.GetOverusageSummaryReportAsync(userId, packageId, startDate, endDate);
        }

        public System.Data.DataSet GetDiskspaceOverusageDetailsReport(int userId, int packageId)
        {
            return base.Client.GetDiskspaceOverusageDetailsReport(userId, packageId);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetDiskspaceOverusageDetailsReportAsync(int userId, int packageId)
        {
            return await base.Client.GetDiskspaceOverusageDetailsReportAsync(userId, packageId);
        }

        public System.Data.DataSet GetBandwidthOverusageDetailsReport(int userId, int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return base.Client.GetBandwidthOverusageDetailsReport(userId, packageId, startDate, endDate);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetBandwidthOverusageDetailsReportAsync(int userId, int packageId, System.DateTime startDate, System.DateTime endDate)
        {
            return await base.Client.GetBandwidthOverusageDetailsReportAsync(userId, packageId, startDate, endDate);
        }
    }
}
#endif