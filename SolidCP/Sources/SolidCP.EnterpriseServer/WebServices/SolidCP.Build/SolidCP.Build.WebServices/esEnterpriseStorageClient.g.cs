#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesEnterpriseStorage", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesEnterpriseStorage
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/AddWebDavAccessToken", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/AddWebDavAccessTokenResponse")]
        int AddWebDavAccessToken(SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken accessToken);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/AddWebDavAccessToken", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/AddWebDavAccessTokenResponse")]
        System.Threading.Tasks.Task<int> AddWebDavAccessTokenAsync(SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken accessToken);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteExpiredWebDavAccessTokens", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteExpiredWebDavAccessTokensResponse")]
        void DeleteExpiredWebDavAccessTokens();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteExpiredWebDavAccessTokens", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteExpiredWebDavAccessTokensResponse")]
        System.Threading.Tasks.Task DeleteExpiredWebDavAccessTokensAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavAccessTokenById", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavAccessTokenByIdResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken GetWebDavAccessTokenById(int id);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavAccessTokenById", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavAccessTokenByIdResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken> GetWebDavAccessTokenByIdAsync(int id);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavAccessTokenByAccessToken", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavAccessTokenByAccessTokenResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken GetWebDavAccessTokenByAccessToken(System.Guid accessToken);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavAccessTokenByAccessToken", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavAccessTokenByAccessTokenResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken> GetWebDavAccessTokenByAccessTokenAsync(System.Guid accessToken);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckFileServicesInstallation", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckFileServicesInstallationResponse")]
        bool CheckFileServicesInstallation(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckFileServicesInstallation", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckFileServicesInstallationResponse")]
        System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFoldersResponse")]
        SolidCP.Providers.OS.SystemFile[] GetEnterpriseFolders(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFoldersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetEnterpriseFoldersAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUserRootFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUserRootFoldersResponse")]
        SolidCP.Providers.OS.SystemFile[] GetUserRootFolders(int itemId, int accountId, string userName, string displayName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUserRootFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUserRootFoldersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetUserRootFoldersAsync(int itemId, int accountId, string userName, string displayName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolderResponse")]
        SolidCP.Providers.OS.SystemFile GetEnterpriseFolder(int itemId, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetEnterpriseFolderAsync(int itemId, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolderWithExtraData", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolderWithExtraDataResponse")]
        SolidCP.Providers.OS.SystemFile GetEnterpriseFolderWithExtraData(int itemId, string folderName, bool loadDriveMapInfo);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolderWithExtraData", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolderWithExtraDataResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetEnterpriseFolderWithExtraDataAsync(int itemId, string folderName, bool loadDriveMapInfo);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseFolderResponse")]
        SolidCP.Providers.Common.ResultObject CreateEnterpriseFolder(int itemId, string folderName, int quota, SolidCP.Providers.OS.QuotaType quotaType, bool addDefaultGroup);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateEnterpriseFolderAsync(int itemId, string folderName, int quota, SolidCP.Providers.OS.QuotaType quotaType, bool addDefaultGroup);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseSubFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseSubFolderResponse")]
        SolidCP.Providers.Common.ResultObject CreateEnterpriseSubFolder(int itemId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseSubFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseSubFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateEnterpriseSubFolderAsync(int itemId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteEnterpriseFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteEnterpriseFolderResponse")]
        SolidCP.Providers.Common.ResultObject DeleteEnterpriseFolder(int itemId, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteEnterpriseFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteEnterpriseFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteEnterpriseFolderAsync(int itemId, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolderPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolderPermissionsResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] GetEnterpriseFolderPermissions(int itemId, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolderPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFolderPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[]> GetEnterpriseFolderPermissionsAsync(int itemId, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderPermissionsResponse")]
        SolidCP.Providers.Common.ResultObject SetEnterpriseFolderPermissions(int itemId, string folderName, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permission);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetEnterpriseFolderPermissionsAsync(int itemId, string folderName, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permission);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SearchESAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SearchESAccountsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchESAccounts(int itemId, string filterColumn, string filterValue, string sortColumn);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SearchESAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SearchESAccountsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchESAccountsAsync(int itemId, string filterColumn, string filterValue, string sortColumn);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFoldersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFoldersPagedResponse")]
        SolidCP.Providers.OS.SystemFilesPaged GetEnterpriseFoldersPaged(int itemId, bool loadUsagesData, bool loadWebdavRules, bool loadMappedDrives, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFoldersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseFoldersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFilesPaged> GetEnterpriseFoldersPagedAsync(int itemId, bool loadUsagesData, bool loadWebdavRules, bool loadMappedDrives, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/RenameEnterpriseFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/RenameEnterpriseFolderResponse")]
        SolidCP.Providers.OS.SystemFile RenameEnterpriseFolder(int itemId, string oldName, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/RenameEnterpriseFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/RenameEnterpriseFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> RenameEnterpriseFolderAsync(int itemId, string oldName, string newName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseStorage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseStorageResponse")]
        SolidCP.Providers.Common.ResultObject CreateEnterpriseStorage(int packageId, int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseStorage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateEnterpriseStorageResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateEnterpriseStorageAsync(int packageId, int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckEnterpriseStorageInitialization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckEnterpriseStorageInitializationResponse")]
        bool CheckEnterpriseStorageInitialization(int packageId, int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckEnterpriseStorageInitialization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckEnterpriseStorageInitializationResponse")]
        System.Threading.Tasks.Task<bool> CheckEnterpriseStorageInitializationAsync(int packageId, int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckUsersDomainExists", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckUsersDomainExistsResponse")]
        bool CheckUsersDomainExists(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckUsersDomainExists", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CheckUsersDomainExistsResponse")]
        System.Threading.Tasks.Task<bool> CheckUsersDomainExistsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavPortalUserSettingsByAccountId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavPortalUserSettingsByAccountIdResponse")]
        string GetWebDavPortalUserSettingsByAccountId(int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavPortalUserSettingsByAccountId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetWebDavPortalUserSettingsByAccountIdResponse")]
        System.Threading.Tasks.Task<string> GetWebDavPortalUserSettingsByAccountIdAsync(int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/UpdateWebDavPortalUserSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/UpdateWebDavPortalUserSettingsResponse")]
        void UpdateWebDavPortalUserSettings(int accountId, string settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/UpdateWebDavPortalUserSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/UpdateWebDavPortalUserSettingsResponse")]
        System.Threading.Tasks.Task UpdateWebDavPortalUserSettingsAsync(int accountId, string settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SearchFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SearchFilesResponse")]
        SolidCP.Providers.OS.SystemFile[] SearchFiles(int itemId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SearchFiles", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SearchFilesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> SearchFilesAsync(int itemId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseStorageServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseStorageServiceIdResponse")]
        int GetEnterpriseStorageServiceId(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseStorageServiceId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetEnterpriseStorageServiceIdResponse")]
        System.Threading.Tasks.Task<int> GetEnterpriseStorageServiceIdAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEsFolderShareSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEsFolderShareSettingsResponse")]
        void SetEsFolderShareSettings(int itemId, string folderName, bool abeIsEnabled, bool edaIsEnabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEsFolderShareSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEsFolderShareSettingsResponse")]
        System.Threading.Tasks.Task SetEsFolderShareSettingsAsync(int itemId, string folderName, bool abeIsEnabled, bool edaIsEnabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetDirectoryBrowseEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetDirectoryBrowseEnabledResponse")]
        bool GetDirectoryBrowseEnabled(int itemId, string site);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetDirectoryBrowseEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetDirectoryBrowseEnabledResponse")]
        System.Threading.Tasks.Task<bool> GetDirectoryBrowseEnabledAsync(int itemId, string site);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetDirectoryBrowseEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetDirectoryBrowseEnabledResponse")]
        void SetDirectoryBrowseEnabled(int itemId, string site, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetDirectoryBrowseEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetDirectoryBrowseEnabledResponse")]
        System.Threading.Tasks.Task SetDirectoryBrowseEnabledAsync(int itemId, string site, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderSettingsResponse")]
        void SetEnterpriseFolderSettings(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderSettingsResponse")]
        System.Threading.Tasks.Task SetEnterpriseFolderSettingsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderGeneralSettingsResponse")]
        void SetEnterpriseFolderGeneralSettings(int itemId, SolidCP.Providers.OS.SystemFile folder, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderGeneralSettingsResponse")]
        System.Threading.Tasks.Task SetEnterpriseFolderGeneralSettingsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderPermissionSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderPermissionSettingsResponse")]
        void SetEnterpriseFolderPermissionSettings(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderPermissionSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetEnterpriseFolderPermissionSettingsResponse")]
        System.Threading.Tasks.Task SetEnterpriseFolderPermissionSettingsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetFolderOwaAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetFolderOwaAccountsResponse")]
        SolidCP.Providers.HostedSolution.OrganizationUser[] GetFolderOwaAccounts(int itemId, SolidCP.Providers.OS.SystemFile folder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetFolderOwaAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetFolderOwaAccountsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetFolderOwaAccountsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetFolderOwaAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetFolderOwaAccountsResponse")]
        void SetFolderOwaAccounts(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.Providers.HostedSolution.OrganizationUser[] users);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetFolderOwaAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/SetFolderOwaAccountsResponse")]
        System.Threading.Tasks.Task SetFolderOwaAccountsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.Providers.HostedSolution.OrganizationUser[] users);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUserEnterpriseFolderWithOwaEditPermission", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUserEnterpriseFolderWithOwaEditPermissionResponse")]
        string[] /*List*/ GetUserEnterpriseFolderWithOwaEditPermission(int itemId, int[] /*List*/ accountIds);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUserEnterpriseFolderWithOwaEditPermission", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUserEnterpriseFolderWithOwaEditPermissionResponse")]
        System.Threading.Tasks.Task<string[]> GetUserEnterpriseFolderWithOwaEditPermissionAsync(int itemId, int[] /*List*/ accountIds);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/MoveToStorageSpace", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/MoveToStorageSpaceResponse")]
        SolidCP.Providers.Common.ResultObject MoveToStorageSpace(int itemId, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/MoveToStorageSpace", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/MoveToStorageSpaceResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> MoveToStorageSpaceAsync(int itemId, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetStatistics", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetStatisticsResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetStatistics(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetStatistics", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetStatisticsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetStatisticsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetStatisticsByOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetStatisticsByOrganizationResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetStatisticsByOrganization(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetStatisticsByOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetStatisticsByOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetStatisticsByOrganizationAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateMappedDrive", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateMappedDriveResponse")]
        SolidCP.Providers.Common.ResultObject CreateMappedDrive(int packageId, int itemId, string driveLetter, string labelAs, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateMappedDrive", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/CreateMappedDriveResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateMappedDriveAsync(int packageId, int itemId, string driveLetter, string labelAs, string folderName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteMappedDrive", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteMappedDriveResponse")]
        SolidCP.Providers.Common.ResultObject DeleteMappedDrive(int itemId, string driveLetter);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteMappedDrive", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/DeleteMappedDriveResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteMappedDriveAsync(int itemId, string driveLetter);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetDriveMapsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetDriveMapsPagedResponse")]
        SolidCP.Providers.OS.MappedDrivesPaged GetDriveMapsPaged(int itemId, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetDriveMapsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetDriveMapsPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.MappedDrivesPaged> GetDriveMapsPagedAsync(int itemId, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUsedDriveLetters", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUsedDriveLettersResponse")]
        string[] GetUsedDriveLetters(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUsedDriveLetters", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetUsedDriveLettersResponse")]
        System.Threading.Tasks.Task<string[]> GetUsedDriveLettersAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetNotMappedEnterpriseFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetNotMappedEnterpriseFoldersResponse")]
        SolidCP.Providers.OS.SystemFile[] GetNotMappedEnterpriseFolders(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetNotMappedEnterpriseFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesEnterpriseStorage/GetNotMappedEnterpriseFoldersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetNotMappedEnterpriseFoldersAsync(int itemId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esEnterpriseStorageAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesEnterpriseStorage
    {
        public int AddWebDavAccessToken(SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken accessToken)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esEnterpriseStorage", "AddWebDavAccessToken", accessToken);
        }

        public async System.Threading.Tasks.Task<int> AddWebDavAccessTokenAsync(SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken accessToken)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esEnterpriseStorage", "AddWebDavAccessToken", accessToken);
        }

        public void DeleteExpiredWebDavAccessTokens()
        {
            Invoke("SolidCP.EnterpriseServer.esEnterpriseStorage", "DeleteExpiredWebDavAccessTokens");
        }

        public async System.Threading.Tasks.Task DeleteExpiredWebDavAccessTokensAsync()
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esEnterpriseStorage", "DeleteExpiredWebDavAccessTokens");
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken GetWebDavAccessTokenById(int id)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetWebDavAccessTokenById", id);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken> GetWebDavAccessTokenByIdAsync(int id)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetWebDavAccessTokenById", id);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken GetWebDavAccessTokenByAccessToken(System.Guid accessToken)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetWebDavAccessTokenByAccessToken", accessToken);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken> GetWebDavAccessTokenByAccessTokenAsync(System.Guid accessToken)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetWebDavAccessTokenByAccessToken", accessToken);
        }

        public bool CheckFileServicesInstallation(int serviceId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CheckFileServicesInstallation", serviceId);
        }

        public async System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync(int serviceId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CheckFileServicesInstallation", serviceId);
        }

        public SolidCP.Providers.OS.SystemFile[] GetEnterpriseFolders(int itemId)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseFolders", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetEnterpriseFoldersAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseFolders", itemId);
        }

        public SolidCP.Providers.OS.SystemFile[] GetUserRootFolders(int itemId, int accountId, string userName, string displayName)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetUserRootFolders", itemId, accountId, userName, displayName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetUserRootFoldersAsync(int itemId, int accountId, string userName, string displayName)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetUserRootFolders", itemId, accountId, userName, displayName);
        }

        public SolidCP.Providers.OS.SystemFile GetEnterpriseFolder(int itemId, string folderName)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseFolder", itemId, folderName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetEnterpriseFolderAsync(int itemId, string folderName)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseFolder", itemId, folderName);
        }

        public SolidCP.Providers.OS.SystemFile GetEnterpriseFolderWithExtraData(int itemId, string folderName, bool loadDriveMapInfo)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseFolderWithExtraData", itemId, folderName, loadDriveMapInfo);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetEnterpriseFolderWithExtraDataAsync(int itemId, string folderName, bool loadDriveMapInfo)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseFolderWithExtraData", itemId, folderName, loadDriveMapInfo);
        }

        public SolidCP.Providers.Common.ResultObject CreateEnterpriseFolder(int itemId, string folderName, int quota, SolidCP.Providers.OS.QuotaType quotaType, bool addDefaultGroup)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CreateEnterpriseFolder", itemId, folderName, quota, quotaType, addDefaultGroup);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateEnterpriseFolderAsync(int itemId, string folderName, int quota, SolidCP.Providers.OS.QuotaType quotaType, bool addDefaultGroup)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CreateEnterpriseFolder", itemId, folderName, quota, quotaType, addDefaultGroup);
        }

        public SolidCP.Providers.Common.ResultObject CreateEnterpriseSubFolder(int itemId, string folderPath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CreateEnterpriseSubFolder", itemId, folderPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateEnterpriseSubFolderAsync(int itemId, string folderPath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CreateEnterpriseSubFolder", itemId, folderPath);
        }

        public SolidCP.Providers.Common.ResultObject DeleteEnterpriseFolder(int itemId, string folderName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "DeleteEnterpriseFolder", itemId, folderName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteEnterpriseFolderAsync(int itemId, string folderName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "DeleteEnterpriseFolder", itemId, folderName);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] GetEnterpriseFolderPermissions(int itemId, string folderName)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseFolderPermissions", itemId, folderName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[]> GetEnterpriseFolderPermissionsAsync(int itemId, string folderName)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseFolderPermissions", itemId, folderName);
        }

        public SolidCP.Providers.Common.ResultObject SetEnterpriseFolderPermissions(int itemId, string folderName, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permission)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetEnterpriseFolderPermissions", itemId, folderName, permission);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetEnterpriseFolderPermissionsAsync(int itemId, string folderName, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permission)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetEnterpriseFolderPermissions", itemId, folderName, permission);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchESAccounts(int itemId, string filterColumn, string filterValue, string sortColumn)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esEnterpriseStorage", "SearchESAccounts", itemId, filterColumn, filterValue, sortColumn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchESAccountsAsync(int itemId, string filterColumn, string filterValue, string sortColumn)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esEnterpriseStorage", "SearchESAccounts", itemId, filterColumn, filterValue, sortColumn);
        }

        public SolidCP.Providers.OS.SystemFilesPaged GetEnterpriseFoldersPaged(int itemId, bool loadUsagesData, bool loadWebdavRules, bool loadMappedDrives, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.OS.SystemFilesPaged>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseFoldersPaged", itemId, loadUsagesData, loadWebdavRules, loadMappedDrives, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFilesPaged> GetEnterpriseFoldersPagedAsync(int itemId, bool loadUsagesData, bool loadWebdavRules, bool loadMappedDrives, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFilesPaged>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseFoldersPaged", itemId, loadUsagesData, loadWebdavRules, loadMappedDrives, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.OS.SystemFile RenameEnterpriseFolder(int itemId, string oldName, string newName)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile>("SolidCP.EnterpriseServer.esEnterpriseStorage", "RenameEnterpriseFolder", itemId, oldName, newName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> RenameEnterpriseFolderAsync(int itemId, string oldName, string newName)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile>("SolidCP.EnterpriseServer.esEnterpriseStorage", "RenameEnterpriseFolder", itemId, oldName, newName);
        }

        public SolidCP.Providers.Common.ResultObject CreateEnterpriseStorage(int packageId, int itemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CreateEnterpriseStorage", packageId, itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateEnterpriseStorageAsync(int packageId, int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CreateEnterpriseStorage", packageId, itemId);
        }

        public bool CheckEnterpriseStorageInitialization(int packageId, int itemId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CheckEnterpriseStorageInitialization", packageId, itemId);
        }

        public async System.Threading.Tasks.Task<bool> CheckEnterpriseStorageInitializationAsync(int packageId, int itemId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CheckEnterpriseStorageInitialization", packageId, itemId);
        }

        public bool CheckUsersDomainExists(int itemId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CheckUsersDomainExists", itemId);
        }

        public async System.Threading.Tasks.Task<bool> CheckUsersDomainExistsAsync(int itemId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CheckUsersDomainExists", itemId);
        }

        public string GetWebDavPortalUserSettingsByAccountId(int accountId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetWebDavPortalUserSettingsByAccountId", accountId);
        }

        public async System.Threading.Tasks.Task<string> GetWebDavPortalUserSettingsByAccountIdAsync(int accountId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetWebDavPortalUserSettingsByAccountId", accountId);
        }

        public void UpdateWebDavPortalUserSettings(int accountId, string settings)
        {
            Invoke("SolidCP.EnterpriseServer.esEnterpriseStorage", "UpdateWebDavPortalUserSettings", accountId, settings);
        }

        public async System.Threading.Tasks.Task UpdateWebDavPortalUserSettingsAsync(int accountId, string settings)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esEnterpriseStorage", "UpdateWebDavPortalUserSettings", accountId, settings);
        }

        public SolidCP.Providers.OS.SystemFile[] SearchFiles(int itemId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "SearchFiles", itemId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> SearchFilesAsync(int itemId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "SearchFiles", itemId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public int GetEnterpriseStorageServiceId(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseStorageServiceId", itemId);
        }

        public async System.Threading.Tasks.Task<int> GetEnterpriseStorageServiceIdAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetEnterpriseStorageServiceId", itemId);
        }

        public void SetEsFolderShareSettings(int itemId, string folderName, bool abeIsEnabled, bool edaIsEnabled)
        {
            Invoke("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetEsFolderShareSettings", itemId, folderName, abeIsEnabled, edaIsEnabled);
        }

        public async System.Threading.Tasks.Task SetEsFolderShareSettingsAsync(int itemId, string folderName, bool abeIsEnabled, bool edaIsEnabled)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetEsFolderShareSettings", itemId, folderName, abeIsEnabled, edaIsEnabled);
        }

        public bool GetDirectoryBrowseEnabled(int itemId, string site)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetDirectoryBrowseEnabled", itemId, site);
        }

        public async System.Threading.Tasks.Task<bool> GetDirectoryBrowseEnabledAsync(int itemId, string site)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetDirectoryBrowseEnabled", itemId, site);
        }

        public void SetDirectoryBrowseEnabled(int itemId, string site, bool enabled)
        {
            Invoke("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetDirectoryBrowseEnabled", itemId, site, enabled);
        }

        public async System.Threading.Tasks.Task SetDirectoryBrowseEnabledAsync(int itemId, string site, bool enabled)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetDirectoryBrowseEnabled", itemId, site, enabled);
        }

        public void SetEnterpriseFolderSettings(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType)
        {
            Invoke("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetEnterpriseFolderSettings", itemId, folder, permissions, directoyBrowsingEnabled, quota, quotaType);
        }

        public async System.Threading.Tasks.Task SetEnterpriseFolderSettingsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetEnterpriseFolderSettings", itemId, folder, permissions, directoyBrowsingEnabled, quota, quotaType);
        }

        public void SetEnterpriseFolderGeneralSettings(int itemId, SolidCP.Providers.OS.SystemFile folder, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType)
        {
            Invoke("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetEnterpriseFolderGeneralSettings", itemId, folder, directoyBrowsingEnabled, quota, quotaType);
        }

        public async System.Threading.Tasks.Task SetEnterpriseFolderGeneralSettingsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetEnterpriseFolderGeneralSettings", itemId, folder, directoyBrowsingEnabled, quota, quotaType);
        }

        public void SetEnterpriseFolderPermissionSettings(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions)
        {
            Invoke("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetEnterpriseFolderPermissionSettings", itemId, folder, permissions);
        }

        public async System.Threading.Tasks.Task SetEnterpriseFolderPermissionSettingsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetEnterpriseFolderPermissionSettings", itemId, folder, permissions);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser[] GetFolderOwaAccounts(int itemId, SolidCP.Providers.OS.SystemFile folder)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationUser[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetFolderOwaAccounts", itemId, folder);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetFolderOwaAccountsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationUser[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetFolderOwaAccounts", itemId, folder);
        }

        public void SetFolderOwaAccounts(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.Providers.HostedSolution.OrganizationUser[] users)
        {
            Invoke("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetFolderOwaAccounts", itemId, folder, users);
        }

        public async System.Threading.Tasks.Task SetFolderOwaAccountsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.Providers.HostedSolution.OrganizationUser[] users)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esEnterpriseStorage", "SetFolderOwaAccounts", itemId, folder, users);
        }

        public string[] /*List*/ GetUserEnterpriseFolderWithOwaEditPermission(int itemId, int[] /*List*/ accountIds)
        {
            return Invoke<string[], string>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetUserEnterpriseFolderWithOwaEditPermission", itemId, accountIds.ToList());
        }

        public async System.Threading.Tasks.Task<string[]> GetUserEnterpriseFolderWithOwaEditPermissionAsync(int itemId, int[] /*List*/ accountIds)
        {
            return await InvokeAsync<string[], string>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetUserEnterpriseFolderWithOwaEditPermission", itemId, accountIds);
        }

        public SolidCP.Providers.Common.ResultObject MoveToStorageSpace(int itemId, string folderName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "MoveToStorageSpace", itemId, folderName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> MoveToStorageSpaceAsync(int itemId, string folderName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "MoveToStorageSpace", itemId, folderName);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetStatistics(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetStatistics", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetStatisticsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetStatistics", itemId);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetStatisticsByOrganization(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetStatisticsByOrganization", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetStatisticsByOrganizationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetStatisticsByOrganization", itemId);
        }

        public SolidCP.Providers.Common.ResultObject CreateMappedDrive(int packageId, int itemId, string driveLetter, string labelAs, string folderName)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CreateMappedDrive", packageId, itemId, driveLetter, labelAs, folderName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateMappedDriveAsync(int packageId, int itemId, string driveLetter, string labelAs, string folderName)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "CreateMappedDrive", packageId, itemId, driveLetter, labelAs, folderName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteMappedDrive(int itemId, string driveLetter)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "DeleteMappedDrive", itemId, driveLetter);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteMappedDriveAsync(int itemId, string driveLetter)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esEnterpriseStorage", "DeleteMappedDrive", itemId, driveLetter);
        }

        public SolidCP.Providers.OS.MappedDrivesPaged GetDriveMapsPaged(int itemId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.OS.MappedDrivesPaged>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetDriveMapsPaged", itemId, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.MappedDrivesPaged> GetDriveMapsPagedAsync(int itemId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.OS.MappedDrivesPaged>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetDriveMapsPaged", itemId, filterValue, sortColumn, startRow, maximumRows);
        }

        public string[] GetUsedDriveLetters(int itemId)
        {
            return Invoke<string[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetUsedDriveLetters", itemId);
        }

        public async System.Threading.Tasks.Task<string[]> GetUsedDriveLettersAsync(int itemId)
        {
            return await InvokeAsync<string[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetUsedDriveLetters", itemId);
        }

        public SolidCP.Providers.OS.SystemFile[] GetNotMappedEnterpriseFolders(int itemId)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetNotMappedEnterpriseFolders", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetNotMappedEnterpriseFoldersAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.EnterpriseServer.esEnterpriseStorage", "GetNotMappedEnterpriseFolders", itemId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esEnterpriseStorage : SolidCP.Web.Client.ClientBase<IesEnterpriseStorage, esEnterpriseStorageAssemblyClient>, IesEnterpriseStorage
    {
        public int AddWebDavAccessToken(SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken accessToken)
        {
            return base.Client.AddWebDavAccessToken(accessToken);
        }

        public async System.Threading.Tasks.Task<int> AddWebDavAccessTokenAsync(SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken accessToken)
        {
            return await base.Client.AddWebDavAccessTokenAsync(accessToken);
        }

        public void DeleteExpiredWebDavAccessTokens()
        {
            base.Client.DeleteExpiredWebDavAccessTokens();
        }

        public async System.Threading.Tasks.Task DeleteExpiredWebDavAccessTokensAsync()
        {
            await base.Client.DeleteExpiredWebDavAccessTokensAsync();
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken GetWebDavAccessTokenById(int id)
        {
            return base.Client.GetWebDavAccessTokenById(id);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken> GetWebDavAccessTokenByIdAsync(int id)
        {
            return await base.Client.GetWebDavAccessTokenByIdAsync(id);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken GetWebDavAccessTokenByAccessToken(System.Guid accessToken)
        {
            return base.Client.GetWebDavAccessTokenByAccessToken(accessToken);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.WebDavAccessToken> GetWebDavAccessTokenByAccessTokenAsync(System.Guid accessToken)
        {
            return await base.Client.GetWebDavAccessTokenByAccessTokenAsync(accessToken);
        }

        public bool CheckFileServicesInstallation(int serviceId)
        {
            return base.Client.CheckFileServicesInstallation(serviceId);
        }

        public async System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync(int serviceId)
        {
            return await base.Client.CheckFileServicesInstallationAsync(serviceId);
        }

        public SolidCP.Providers.OS.SystemFile[] GetEnterpriseFolders(int itemId)
        {
            return base.Client.GetEnterpriseFolders(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetEnterpriseFoldersAsync(int itemId)
        {
            return await base.Client.GetEnterpriseFoldersAsync(itemId);
        }

        public SolidCP.Providers.OS.SystemFile[] GetUserRootFolders(int itemId, int accountId, string userName, string displayName)
        {
            return base.Client.GetUserRootFolders(itemId, accountId, userName, displayName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetUserRootFoldersAsync(int itemId, int accountId, string userName, string displayName)
        {
            return await base.Client.GetUserRootFoldersAsync(itemId, accountId, userName, displayName);
        }

        public SolidCP.Providers.OS.SystemFile GetEnterpriseFolder(int itemId, string folderName)
        {
            return base.Client.GetEnterpriseFolder(itemId, folderName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetEnterpriseFolderAsync(int itemId, string folderName)
        {
            return await base.Client.GetEnterpriseFolderAsync(itemId, folderName);
        }

        public SolidCP.Providers.OS.SystemFile GetEnterpriseFolderWithExtraData(int itemId, string folderName, bool loadDriveMapInfo)
        {
            return base.Client.GetEnterpriseFolderWithExtraData(itemId, folderName, loadDriveMapInfo);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetEnterpriseFolderWithExtraDataAsync(int itemId, string folderName, bool loadDriveMapInfo)
        {
            return await base.Client.GetEnterpriseFolderWithExtraDataAsync(itemId, folderName, loadDriveMapInfo);
        }

        public SolidCP.Providers.Common.ResultObject CreateEnterpriseFolder(int itemId, string folderName, int quota, SolidCP.Providers.OS.QuotaType quotaType, bool addDefaultGroup)
        {
            return base.Client.CreateEnterpriseFolder(itemId, folderName, quota, quotaType, addDefaultGroup);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateEnterpriseFolderAsync(int itemId, string folderName, int quota, SolidCP.Providers.OS.QuotaType quotaType, bool addDefaultGroup)
        {
            return await base.Client.CreateEnterpriseFolderAsync(itemId, folderName, quota, quotaType, addDefaultGroup);
        }

        public SolidCP.Providers.Common.ResultObject CreateEnterpriseSubFolder(int itemId, string folderPath)
        {
            return base.Client.CreateEnterpriseSubFolder(itemId, folderPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateEnterpriseSubFolderAsync(int itemId, string folderPath)
        {
            return await base.Client.CreateEnterpriseSubFolderAsync(itemId, folderPath);
        }

        public SolidCP.Providers.Common.ResultObject DeleteEnterpriseFolder(int itemId, string folderName)
        {
            return base.Client.DeleteEnterpriseFolder(itemId, folderName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteEnterpriseFolderAsync(int itemId, string folderName)
        {
            return await base.Client.DeleteEnterpriseFolderAsync(itemId, folderName);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] GetEnterpriseFolderPermissions(int itemId, string folderName)
        {
            return base.Client.GetEnterpriseFolderPermissions(itemId, folderName);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[]> GetEnterpriseFolderPermissionsAsync(int itemId, string folderName)
        {
            return await base.Client.GetEnterpriseFolderPermissionsAsync(itemId, folderName);
        }

        public SolidCP.Providers.Common.ResultObject SetEnterpriseFolderPermissions(int itemId, string folderName, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permission)
        {
            return base.Client.SetEnterpriseFolderPermissions(itemId, folderName, permission);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetEnterpriseFolderPermissionsAsync(int itemId, string folderName, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permission)
        {
            return await base.Client.SetEnterpriseFolderPermissionsAsync(itemId, folderName, permission);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchESAccounts(int itemId, string filterColumn, string filterValue, string sortColumn)
        {
            return base.Client.SearchESAccounts(itemId, filterColumn, filterValue, sortColumn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchESAccountsAsync(int itemId, string filterColumn, string filterValue, string sortColumn)
        {
            return await base.Client.SearchESAccountsAsync(itemId, filterColumn, filterValue, sortColumn);
        }

        public SolidCP.Providers.OS.SystemFilesPaged GetEnterpriseFoldersPaged(int itemId, bool loadUsagesData, bool loadWebdavRules, bool loadMappedDrives, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetEnterpriseFoldersPaged(itemId, loadUsagesData, loadWebdavRules, loadMappedDrives, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFilesPaged> GetEnterpriseFoldersPagedAsync(int itemId, bool loadUsagesData, bool loadWebdavRules, bool loadMappedDrives, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetEnterpriseFoldersPagedAsync(itemId, loadUsagesData, loadWebdavRules, loadMappedDrives, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.OS.SystemFile RenameEnterpriseFolder(int itemId, string oldName, string newName)
        {
            return base.Client.RenameEnterpriseFolder(itemId, oldName, newName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> RenameEnterpriseFolderAsync(int itemId, string oldName, string newName)
        {
            return await base.Client.RenameEnterpriseFolderAsync(itemId, oldName, newName);
        }

        public SolidCP.Providers.Common.ResultObject CreateEnterpriseStorage(int packageId, int itemId)
        {
            return base.Client.CreateEnterpriseStorage(packageId, itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateEnterpriseStorageAsync(int packageId, int itemId)
        {
            return await base.Client.CreateEnterpriseStorageAsync(packageId, itemId);
        }

        public bool CheckEnterpriseStorageInitialization(int packageId, int itemId)
        {
            return base.Client.CheckEnterpriseStorageInitialization(packageId, itemId);
        }

        public async System.Threading.Tasks.Task<bool> CheckEnterpriseStorageInitializationAsync(int packageId, int itemId)
        {
            return await base.Client.CheckEnterpriseStorageInitializationAsync(packageId, itemId);
        }

        public bool CheckUsersDomainExists(int itemId)
        {
            return base.Client.CheckUsersDomainExists(itemId);
        }

        public async System.Threading.Tasks.Task<bool> CheckUsersDomainExistsAsync(int itemId)
        {
            return await base.Client.CheckUsersDomainExistsAsync(itemId);
        }

        public string GetWebDavPortalUserSettingsByAccountId(int accountId)
        {
            return base.Client.GetWebDavPortalUserSettingsByAccountId(accountId);
        }

        public async System.Threading.Tasks.Task<string> GetWebDavPortalUserSettingsByAccountIdAsync(int accountId)
        {
            return await base.Client.GetWebDavPortalUserSettingsByAccountIdAsync(accountId);
        }

        public void UpdateWebDavPortalUserSettings(int accountId, string settings)
        {
            base.Client.UpdateWebDavPortalUserSettings(accountId, settings);
        }

        public async System.Threading.Tasks.Task UpdateWebDavPortalUserSettingsAsync(int accountId, string settings)
        {
            await base.Client.UpdateWebDavPortalUserSettingsAsync(accountId, settings);
        }

        public SolidCP.Providers.OS.SystemFile[] SearchFiles(int itemId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return base.Client.SearchFiles(itemId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> SearchFilesAsync(int itemId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return await base.Client.SearchFilesAsync(itemId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public int GetEnterpriseStorageServiceId(int itemId)
        {
            return base.Client.GetEnterpriseStorageServiceId(itemId);
        }

        public async System.Threading.Tasks.Task<int> GetEnterpriseStorageServiceIdAsync(int itemId)
        {
            return await base.Client.GetEnterpriseStorageServiceIdAsync(itemId);
        }

        public void SetEsFolderShareSettings(int itemId, string folderName, bool abeIsEnabled, bool edaIsEnabled)
        {
            base.Client.SetEsFolderShareSettings(itemId, folderName, abeIsEnabled, edaIsEnabled);
        }

        public async System.Threading.Tasks.Task SetEsFolderShareSettingsAsync(int itemId, string folderName, bool abeIsEnabled, bool edaIsEnabled)
        {
            await base.Client.SetEsFolderShareSettingsAsync(itemId, folderName, abeIsEnabled, edaIsEnabled);
        }

        public bool GetDirectoryBrowseEnabled(int itemId, string site)
        {
            return base.Client.GetDirectoryBrowseEnabled(itemId, site);
        }

        public async System.Threading.Tasks.Task<bool> GetDirectoryBrowseEnabledAsync(int itemId, string site)
        {
            return await base.Client.GetDirectoryBrowseEnabledAsync(itemId, site);
        }

        public void SetDirectoryBrowseEnabled(int itemId, string site, bool enabled)
        {
            base.Client.SetDirectoryBrowseEnabled(itemId, site, enabled);
        }

        public async System.Threading.Tasks.Task SetDirectoryBrowseEnabledAsync(int itemId, string site, bool enabled)
        {
            await base.Client.SetDirectoryBrowseEnabledAsync(itemId, site, enabled);
        }

        public void SetEnterpriseFolderSettings(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType)
        {
            base.Client.SetEnterpriseFolderSettings(itemId, folder, permissions, directoyBrowsingEnabled, quota, quotaType);
        }

        public async System.Threading.Tasks.Task SetEnterpriseFolderSettingsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType)
        {
            await base.Client.SetEnterpriseFolderSettingsAsync(itemId, folder, permissions, directoyBrowsingEnabled, quota, quotaType);
        }

        public void SetEnterpriseFolderGeneralSettings(int itemId, SolidCP.Providers.OS.SystemFile folder, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType)
        {
            base.Client.SetEnterpriseFolderGeneralSettings(itemId, folder, directoyBrowsingEnabled, quota, quotaType);
        }

        public async System.Threading.Tasks.Task SetEnterpriseFolderGeneralSettingsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, bool directoyBrowsingEnabled, int quota, SolidCP.Providers.OS.QuotaType quotaType)
        {
            await base.Client.SetEnterpriseFolderGeneralSettingsAsync(itemId, folder, directoyBrowsingEnabled, quota, quotaType);
        }

        public void SetEnterpriseFolderPermissionSettings(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions)
        {
            base.Client.SetEnterpriseFolderPermissionSettings(itemId, folder, permissions);
        }

        public async System.Threading.Tasks.Task SetEnterpriseFolderPermissionSettingsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.EnterpriseServer.Base.HostedSolution.ESPermission[] permissions)
        {
            await base.Client.SetEnterpriseFolderPermissionSettingsAsync(itemId, folder, permissions);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser[] GetFolderOwaAccounts(int itemId, SolidCP.Providers.OS.SystemFile folder)
        {
            return base.Client.GetFolderOwaAccounts(itemId, folder);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> GetFolderOwaAccountsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder)
        {
            return await base.Client.GetFolderOwaAccountsAsync(itemId, folder);
        }

        public void SetFolderOwaAccounts(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.Providers.HostedSolution.OrganizationUser[] users)
        {
            base.Client.SetFolderOwaAccounts(itemId, folder, users);
        }

        public async System.Threading.Tasks.Task SetFolderOwaAccountsAsync(int itemId, SolidCP.Providers.OS.SystemFile folder, SolidCP.Providers.HostedSolution.OrganizationUser[] users)
        {
            await base.Client.SetFolderOwaAccountsAsync(itemId, folder, users);
        }

        public string[] /*List*/ GetUserEnterpriseFolderWithOwaEditPermission(int itemId, int[] /*List*/ accountIds)
        {
            return base.Client.GetUserEnterpriseFolderWithOwaEditPermission(itemId, accountIds);
        }

        public async System.Threading.Tasks.Task<string[]> GetUserEnterpriseFolderWithOwaEditPermissionAsync(int itemId, int[] /*List*/ accountIds)
        {
            return await base.Client.GetUserEnterpriseFolderWithOwaEditPermissionAsync(itemId, accountIds);
        }

        public SolidCP.Providers.Common.ResultObject MoveToStorageSpace(int itemId, string folderName)
        {
            return base.Client.MoveToStorageSpace(itemId, folderName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> MoveToStorageSpaceAsync(int itemId, string folderName)
        {
            return await base.Client.MoveToStorageSpaceAsync(itemId, folderName);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetStatistics(int itemId)
        {
            return base.Client.GetStatistics(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetStatisticsAsync(int itemId)
        {
            return await base.Client.GetStatisticsAsync(itemId);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetStatisticsByOrganization(int itemId)
        {
            return base.Client.GetStatisticsByOrganization(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetStatisticsByOrganizationAsync(int itemId)
        {
            return await base.Client.GetStatisticsByOrganizationAsync(itemId);
        }

        public SolidCP.Providers.Common.ResultObject CreateMappedDrive(int packageId, int itemId, string driveLetter, string labelAs, string folderName)
        {
            return base.Client.CreateMappedDrive(packageId, itemId, driveLetter, labelAs, folderName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CreateMappedDriveAsync(int packageId, int itemId, string driveLetter, string labelAs, string folderName)
        {
            return await base.Client.CreateMappedDriveAsync(packageId, itemId, driveLetter, labelAs, folderName);
        }

        public SolidCP.Providers.Common.ResultObject DeleteMappedDrive(int itemId, string driveLetter)
        {
            return base.Client.DeleteMappedDrive(itemId, driveLetter);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteMappedDriveAsync(int itemId, string driveLetter)
        {
            return await base.Client.DeleteMappedDriveAsync(itemId, driveLetter);
        }

        public SolidCP.Providers.OS.MappedDrivesPaged GetDriveMapsPaged(int itemId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetDriveMapsPaged(itemId, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.MappedDrivesPaged> GetDriveMapsPagedAsync(int itemId, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetDriveMapsPagedAsync(itemId, filterValue, sortColumn, startRow, maximumRows);
        }

        public string[] GetUsedDriveLetters(int itemId)
        {
            return base.Client.GetUsedDriveLetters(itemId);
        }

        public async System.Threading.Tasks.Task<string[]> GetUsedDriveLettersAsync(int itemId)
        {
            return await base.Client.GetUsedDriveLettersAsync(itemId);
        }

        public SolidCP.Providers.OS.SystemFile[] GetNotMappedEnterpriseFolders(int itemId)
        {
            return base.Client.GetNotMappedEnterpriseFolders(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetNotMappedEnterpriseFoldersAsync(int itemId)
        {
            return await base.Client.GetNotMappedEnterpriseFoldersAsync(itemId);
        }
    }
}
#endif