#if !Client
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SolidCP.Web.Services;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using System.Collections.Specialized;
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
    public interface IesPackages
    {
        [WebMethod]
        [OperationContract]
        DataSet GetHostingPlans(int userId);
        [WebMethod]
        [OperationContract]
        DataSet GetHostingAddons(int userId);
        [WebMethod]
        [OperationContract]
        HostingPlanInfo GetHostingPlan(int planId);
        [WebMethod]
        [OperationContract]
        DataSet GetHostingPlanQuotas(int packageId, int planId, int serverId);
        [WebMethod]
        [OperationContract]
        HostingPlanContext GetHostingPlanContext(int planId);
        [WebMethod]
        [OperationContract]
        List<HostingPlanInfo> GetUserAvailableHostingPlans(int userId);
        [WebMethod]
        [OperationContract]
        List<HostingPlanInfo> GetUserAvailableHostingAddons(int userId);
        [WebMethod]
        [OperationContract]
        int AddHostingPlan(HostingPlanInfo plan);
        [WebMethod]
        [OperationContract]
        PackageResult UpdateHostingPlan(HostingPlanInfo plan);
        [WebMethod]
        [OperationContract]
        int DeleteHostingPlan(int planId);
        [WebMethod]
        [OperationContract]
        List<PackageInfo> GetPackages(int userId);
        [WebMethod]
        [OperationContract]
        DataSet GetNestedPackagesSummary(int packageId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawPackages(int userId);
        [WebMethod]
        [OperationContract]
        DataSet SearchServiceItemsPaged(int userId, int itemTypeId, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        DataSet GetSearchObject(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int startRow, int maximumRows, string colType, string fullType);
        [WebMethod]
        [OperationContract]
        DataSet GetSearchObjectQuickFind(int userId, string filterColumn, string filterValue, int statusId, int roleId, string sortColumn, int maximumRows, string colType, string fullType);
        [WebMethod]
        [OperationContract]
        DataSet GetSearchTableByColumns(string PagedStored, string FilterValue, int MaximumRows, bool Recursive, int PoolID, int ServerID, int StatusID, int PlanID, int OrgID, string ItemTypeName, string GroupName, int PackageID, string VPSType, int RoleID, int UserID, string FilterColumns);
        [WebMethod]
        [OperationContract]
        DataSet GetPackagesPaged(int userId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        DataSet GetNestedPackagesPaged(int packageId, string filterColumn, string filterValue, int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<PackageInfo> GetPackagePackages(int packageId);
        [WebMethod]
        [OperationContract]
        List<PackageInfo> GetMyPackages(int userId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawMyPackages(int userId);
        [WebMethod]
        [OperationContract]
        PackageInfo GetPackage(int packageId);
        [WebMethod]
        [OperationContract]
        PackageContext GetPackageContext(int packageId);
        [WebMethod]
        [OperationContract]
        DataSet GetPackageQuotas(int packageId);
        [WebMethod]
        [OperationContract]
        DataSet GetPackageQuotasForEdit(int packageId);
        [WebMethod]
        [OperationContract]
        PackageResult AddPackage(int userId, int planId, string packageName, string packageComments, int statusId, DateTime purchaseDate);
        [WebMethod]
        [OperationContract]
        PackageResult UpdatePackage(PackageInfo package);
        [WebMethod]
        [OperationContract]
        PackageResult UpdatePackageLiteral(int packageId, int statusId, int planId, DateTime purchaseDate, string packageName, string packageComments);
        [WebMethod]
        [OperationContract]
        int ChangePackageUser(int packageId, int UserId);
        [WebMethod]
        [OperationContract]
        int UpdatePackageName(int packageId, string packageName, string packageComments);
        [WebMethod]
        [OperationContract]
        int DeletePackage(int packageId);
        [WebMethod]
        [OperationContract]
        int ChangePackageStatus(int packageId, PackageStatus status);
        [WebMethod]
        [OperationContract]
        string EvaluateUserPackageTempate(int userId, int packageId, string template);
        [WebMethod]
        [OperationContract]
        PackageSettings GetPackageSettings(int packageId, string settingsName);
        [WebMethod]
        [OperationContract]
        int UpdatePackageSettings(PackageSettings settings);
        [WebMethod]
        [OperationContract]
        bool SetDefaultTopPackage(int userId, int packageId);
        [WebMethod]
        [OperationContract]
        DataSet GetPackageAddons(int packageId);
        [WebMethod]
        [OperationContract]
        PackageAddonInfo GetPackageAddon(int packageAddonId);
        [WebMethod]
        [OperationContract]
        PackageResult AddPackageAddonById(int packageId, int addonPlanId, int quantity);
        [WebMethod]
        [OperationContract]
        PackageResult AddPackageAddon(PackageAddonInfo addon);
        [WebMethod]
        [OperationContract]
        PackageResult AddPackageAddonLiteral(int packageId, int planId, int quantity, int statusId, DateTime purchaseDate, string comments);
        [WebMethod]
        [OperationContract]
        PackageResult UpdatePackageAddon(PackageAddonInfo addon);
        [WebMethod]
        [OperationContract]
        PackageResult UpdatePackageAddonLiteral(int packageAddonId, int planId, int quantity, int statusId, DateTime purchaseDate, string comments);
        [WebMethod]
        [OperationContract]
        int DeletePackageAddon(int packageAddonId);
        [WebMethod]
        [OperationContract]
        DataSet GetSearchableServiceItemTypes();
        [WebMethod]
        [OperationContract]
        DataSet GetRawPackageItemsByType(int packageId, string itemTypeName, bool recursive);
        [WebMethod]
        [OperationContract]
        DataSet GetRawPackageItemsPaged(int packageId, string groupName, string typeName, int serverId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        DataSet GetRawPackageItems(int packageId);
        [WebMethod]
        [OperationContract]
        int DetachPackageItem(int itemId);
        [WebMethod]
        [OperationContract]
        int MovePackageItem(int itemId, int destinationServiceId);
        [WebMethod]
        [OperationContract]
        QuotaValueInfo GetPackageQuota(int packageId, string quotaName);
        [WebMethod]
        [OperationContract]
        int SendAccountSummaryLetter(int userId, string to, string cc);
        [WebMethod]
        [OperationContract]
        int SendPackageSummaryLetter(int packageId, string to, string cc);
        [WebMethod]
        [OperationContract]
        string GetEvaluatedPackageTemplateBody(int packageId);
        [WebMethod]
        [OperationContract]
        string GetEvaluatedAccountTemplateBody(int userId);
        [WebMethod]
        [OperationContract]
        PackageResult AddPackageWithResources(int userId, int planId, string spaceName, int statusId, bool sendLetter, bool createResources, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName);
        [WebMethod]
        [OperationContract]
        int CreateUserWizard(int parentPackageId, string username, string password, int roleId, string firstName, string lastName, string email, string secondaryEmail, bool htmlMail, bool sendAccountLetter, bool createPackage, int planId, bool sendPackageLetter, string domainName, bool tempDomain, bool createWebSite, bool createFtpAccount, string ftpAccountName, bool createMailAccount, string hostName, bool createZoneRecord);
        [WebMethod]
        [OperationContract]
        DataSet GetPackagesBandwidthPaged(int userId, int packageId, DateTime startDate, DateTime endDate, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        DataSet GetPackagesDiskspacePaged(int userId, int packageId, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        DataSet GetPackageBandwidth(int packageId, DateTime startDate, DateTime endDate);
        [WebMethod]
        [OperationContract]
        DataSet GetPackageDiskspace(int packageId);
        [WebMethod]
        [OperationContract]
        DataSet GetOverusageSummaryReport(int userId, int packageId, DateTime startDate, DateTime endDate);
        [WebMethod]
        [OperationContract]
        DataSet GetDiskspaceOverusageDetailsReport(int userId, int packageId);
        [WebMethod]
        [OperationContract]
        DataSet GetBandwidthOverusageDetailsReport(int userId, int packageId, DateTime startDate, DateTime endDate);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esPackages : SolidCP.EnterpriseServer.esPackages, IesPackages
    {
    }
}
#endif