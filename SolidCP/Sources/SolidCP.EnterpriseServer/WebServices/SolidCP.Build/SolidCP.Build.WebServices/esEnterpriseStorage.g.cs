#if !Client
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.EnterpriseStorage;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.OS;
using SolidCP.Providers.Web;
using SolidCP.EnterpriseServer.Base.HostedSolution;
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
    public interface IesEnterpriseStorage
    {
        [WebMethod]
        [OperationContract]
        int AddWebDavAccessToken(WebDavAccessToken accessToken);
        [WebMethod]
        [OperationContract]
        void DeleteExpiredWebDavAccessTokens();
        [WebMethod]
        [OperationContract]
        WebDavAccessToken GetWebDavAccessTokenById(int id);
        [WebMethod]
        [OperationContract]
        WebDavAccessToken GetWebDavAccessTokenByAccessToken(Guid accessToken);
        [WebMethod]
        [OperationContract]
        bool CheckFileServicesInstallation(int serviceId);
        [WebMethod]
        [OperationContract]
        SystemFile[] GetEnterpriseFolders(int itemId);
        [WebMethod]
        [OperationContract]
        SystemFile[] GetUserRootFolders(int itemId, int accountId, string userName, string displayName);
        [WebMethod]
        [OperationContract]
        SystemFile GetEnterpriseFolder(int itemId, string folderName);
        [WebMethod]
        [OperationContract]
        SystemFile GetEnterpriseFolderWithExtraData(int itemId, string folderName, bool loadDriveMapInfo);
        [WebMethod]
        [OperationContract]
        ResultObject CreateEnterpriseFolder(int itemId, string folderName, int quota, QuotaType quotaType, bool addDefaultGroup);
        [WebMethod]
        [OperationContract]
        ResultObject CreateEnterpriseSubFolder(int itemId, string folderPath);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteEnterpriseFolder(int itemId, string folderName);
        [WebMethod]
        [OperationContract]
        ESPermission[] GetEnterpriseFolderPermissions(int itemId, string folderName);
        [WebMethod]
        [OperationContract]
        ResultObject SetEnterpriseFolderPermissions(int itemId, string folderName, ESPermission[] permission);
        [WebMethod]
        [OperationContract]
        List<ExchangeAccount> SearchESAccounts(int itemId, string filterColumn, string filterValue, string sortColumn);
        [WebMethod]
        [OperationContract]
        SystemFilesPaged GetEnterpriseFoldersPaged(int itemId, bool loadUsagesData, bool loadWebdavRules, bool loadMappedDrives, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        SystemFile RenameEnterpriseFolder(int itemId, string oldName, string newName);
        [WebMethod]
        [OperationContract]
        ResultObject CreateEnterpriseStorage(int packageId, int itemId);
        [WebMethod]
        [OperationContract]
        bool CheckEnterpriseStorageInitialization(int packageId, int itemId);
        [WebMethod]
        [OperationContract]
        bool CheckUsersDomainExists(int itemId);
        [WebMethod]
        [OperationContract]
        string GetWebDavPortalUserSettingsByAccountId(int accountId);
        [WebMethod]
        [OperationContract]
        void UpdateWebDavPortalUserSettings(int accountId, string settings);
        [WebMethod]
        [OperationContract]
        SystemFile[] SearchFiles(int itemId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive);
        [WebMethod]
        [OperationContract]
        int GetEnterpriseStorageServiceId(int itemId);
        [WebMethod]
        [OperationContract]
        void SetEsFolderShareSettings(int itemId, string folderName, bool abeIsEnabled, bool edaIsEnabled);
        [WebMethod]
        [OperationContract]
        bool GetDirectoryBrowseEnabled(int itemId, string site);
        [WebMethod]
        [OperationContract]
        void SetDirectoryBrowseEnabled(int itemId, string site, bool enabled);
        [WebMethod]
        [OperationContract]
        void SetEnterpriseFolderSettings(int itemId, SystemFile folder, ESPermission[] permissions, bool directoyBrowsingEnabled, int quota, QuotaType quotaType);
        [WebMethod]
        [OperationContract]
        void SetEnterpriseFolderGeneralSettings(int itemId, SystemFile folder, bool directoyBrowsingEnabled, int quota, QuotaType quotaType);
        [WebMethod]
        [OperationContract]
        void SetEnterpriseFolderPermissionSettings(int itemId, SystemFile folder, ESPermission[] permissions);
        [WebMethod]
        [OperationContract]
        OrganizationUser[] GetFolderOwaAccounts(int itemId, SystemFile folder);
        [WebMethod]
        [OperationContract]
        void SetFolderOwaAccounts(int itemId, SystemFile folder, OrganizationUser[] users);
        [WebMethod]
        [OperationContract]
        List<string> GetUserEnterpriseFolderWithOwaEditPermission(int itemId, List<int> accountIds);
        [WebMethod]
        [OperationContract]
        ResultObject MoveToStorageSpace(int itemId, string folderName);
        [WebMethod]
        [OperationContract]
        OrganizationStatistics GetStatistics(int itemId);
        [WebMethod]
        [OperationContract]
        OrganizationStatistics GetStatisticsByOrganization(int itemId);
        [WebMethod]
        [OperationContract]
        ResultObject CreateMappedDrive(int packageId, int itemId, string driveLetter, string labelAs, string folderName);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteMappedDrive(int itemId, string driveLetter);
        [WebMethod]
        [OperationContract]
        MappedDrivesPaged GetDriveMapsPaged(int itemId, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        string[] GetUsedDriveLetters(int itemId);
        [WebMethod]
        [OperationContract]
        SystemFile[] GetNotMappedEnterpriseFolders(int itemId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esEnterpriseStorage : SolidCP.EnterpriseServer.esEnterpriseStorage, IesEnterpriseStorage
    {
    }
}
#endif