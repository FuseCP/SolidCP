#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IWebServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IWebServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeSiteState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeSiteStateResponse")]
        void ChangeSiteState(string siteId, SolidCP.Providers.ServerState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeSiteState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeSiteStateResponse")]
        System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, SolidCP.Providers.ServerState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSiteState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteStateResponse")]
        SolidCP.Providers.ServerState GetSiteState(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSiteState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ServerState> GetSiteStateAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSiteId", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteIdResponse")]
        string GetSiteId(string siteName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSiteId", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteIdResponse")]
        System.Threading.Tasks.Task<string> GetSiteIdAsync(string siteName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSitesAccounts", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSitesAccountsResponse")]
        string[] GetSitesAccounts(string[] siteIds);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSitesAccounts", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSitesAccountsResponse")]
        System.Threading.Tasks.Task<string[]> GetSitesAccountsAsync(string[] siteIds);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SiteExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SiteExistsResponse")]
        bool SiteExists(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SiteExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SiteExistsResponse")]
        System.Threading.Tasks.Task<bool> SiteExistsAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSites", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSitesResponse")]
        string[] GetSites();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSites", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSitesResponse")]
        System.Threading.Tasks.Task<string[]> GetSitesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteResponse")]
        SolidCP.Providers.Web.WebSite GetSite(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebSite> GetSiteAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSiteBindings", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteBindingsResponse")]
        SolidCP.Providers.ServerBinding[] GetSiteBindings(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSiteBindings", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteBindingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ServerBinding[]> GetSiteBindingsAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateSiteResponse")]
        string CreateSite(SolidCP.Providers.Web.WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateSiteResponse")]
        System.Threading.Tasks.Task<string> CreateSiteAsync(SolidCP.Providers.Web.WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteResponse")]
        void UpdateSite(SolidCP.Providers.Web.WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteResponse")]
        System.Threading.Tasks.Task UpdateSiteAsync(SolidCP.Providers.Web.WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteBindings", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteBindingsResponse")]
        void UpdateSiteBindings(string siteId, SolidCP.Providers.ServerBinding[] bindings, bool emptyBindingsAllowed);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteBindings", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteBindingsResponse")]
        System.Threading.Tasks.Task UpdateSiteBindingsAsync(string siteId, SolidCP.Providers.ServerBinding[] bindings, bool emptyBindingsAllowed);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteSiteResponse")]
        void DeleteSite(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteSiteResponse")]
        System.Threading.Tasks.Task DeleteSiteAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeAppPoolState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeAppPoolStateResponse")]
        void ChangeAppPoolState(string siteId, SolidCP.Providers.AppPoolState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeAppPoolState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeAppPoolStateResponse")]
        System.Threading.Tasks.Task ChangeAppPoolStateAsync(string siteId, SolidCP.Providers.AppPoolState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppPoolState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppPoolStateResponse")]
        SolidCP.Providers.AppPoolState GetAppPoolState(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppPoolState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppPoolStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.AppPoolState> GetAppPoolStateAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/VirtualDirectoryExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/VirtualDirectoryExistsResponse")]
        bool VirtualDirectoryExists(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/VirtualDirectoryExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/VirtualDirectoryExistsResponse")]
        System.Threading.Tasks.Task<bool> VirtualDirectoryExistsAsync(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectoriesResponse")]
        SolidCP.Providers.Web.WebVirtualDirectory[] GetVirtualDirectories(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectoriesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory[]> GetVirtualDirectoriesAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectoryResponse")]
        SolidCP.Providers.Web.WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory> GetVirtualDirectoryAsync(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateVirtualDirectoryResponse")]
        void CreateVirtualDirectory(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateVirtualDirectoryResponse")]
        System.Threading.Tasks.Task CreateVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateVirtualDirectoryResponse")]
        void UpdateVirtualDirectory(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateVirtualDirectoryResponse")]
        System.Threading.Tasks.Task UpdateVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteVirtualDirectoryResponse")]
        void DeleteVirtualDirectory(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteVirtualDirectoryResponse")]
        System.Threading.Tasks.Task DeleteVirtualDirectoryAsync(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/AppVirtualDirectoryExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/AppVirtualDirectoryExistsResponse")]
        bool AppVirtualDirectoryExists(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/AppVirtualDirectoryExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/AppVirtualDirectoryExistsResponse")]
        System.Threading.Tasks.Task<bool> AppVirtualDirectoryExistsAsync(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectoriesResponse")]
        SolidCP.Providers.Web.WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectoriesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetAppVirtualDirectoriesAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectoryResponse")]
        SolidCP.Providers.Web.WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory> GetAppVirtualDirectoryAsync(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateAppVirtualDirectoryResponse")]
        void CreateAppVirtualDirectory(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task CreateAppVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateEnterpriseStorageAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateEnterpriseStorageAppVirtualDirectoryResponse")]
        void CreateEnterpriseStorageAppVirtualDirectory(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateEnterpriseStorageAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateEnterpriseStorageAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task CreateEnterpriseStorageAppVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateAppVirtualDirectoryResponse")]
        void UpdateAppVirtualDirectory(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task UpdateAppVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteAppVirtualDirectoryResponse")]
        void DeleteAppVirtualDirectory(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task DeleteAppVirtualDirectoryAsync(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/IsFrontPageSystemInstalled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/IsFrontPageSystemInstalledResponse")]
        bool IsFrontPageSystemInstalled();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/IsFrontPageSystemInstalled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/IsFrontPageSystemInstalledResponse")]
        System.Threading.Tasks.Task<bool> IsFrontPageSystemInstalledAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/IsFrontPageInstalled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/IsFrontPageInstalledResponse")]
        bool IsFrontPageInstalled(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/IsFrontPageInstalled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/IsFrontPageInstalledResponse")]
        System.Threading.Tasks.Task<bool> IsFrontPageInstalledAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallFrontPage", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallFrontPageResponse")]
        bool InstallFrontPage(string siteId, string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallFrontPage", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallFrontPageResponse")]
        System.Threading.Tasks.Task<bool> InstallFrontPageAsync(string siteId, string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UninstallFrontPage", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UninstallFrontPageResponse")]
        void UninstallFrontPage(string siteId, string username);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UninstallFrontPage", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UninstallFrontPageResponse")]
        System.Threading.Tasks.Task UninstallFrontPageAsync(string siteId, string username);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeFrontPagePassword", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeFrontPagePasswordResponse")]
        void ChangeFrontPagePassword(string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeFrontPagePassword", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeFrontPagePasswordResponse")]
        System.Threading.Tasks.Task ChangeFrontPagePasswordAsync(string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/IsColdFusionSystemInstalled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/IsColdFusionSystemInstalledResponse")]
        bool IsColdFusionSystemInstalled();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/IsColdFusionSystemInstalled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/IsColdFusionSystemInstalledResponse")]
        System.Threading.Tasks.Task<bool> IsColdFusionSystemInstalledAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GrantWebSiteAccess", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GrantWebSiteAccessResponse")]
        void GrantWebSiteAccess(string path, string siteId, SolidCP.Providers.NTFSPermission permission);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GrantWebSiteAccess", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GrantWebSiteAccessResponse")]
        System.Threading.Tasks.Task GrantWebSiteAccessAsync(string path, string siteId, SolidCP.Providers.NTFSPermission permission);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallSecuredFoldersResponse")]
        void InstallSecuredFolders(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallSecuredFoldersResponse")]
        System.Threading.Tasks.Task InstallSecuredFoldersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UninstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UninstallSecuredFoldersResponse")]
        void UninstallSecuredFolders(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UninstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UninstallSecuredFoldersResponse")]
        System.Threading.Tasks.Task UninstallSecuredFoldersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetFoldersResponse")]
        SolidCP.Providers.Web.WebFolder[] /*List*/ GetFolders(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetFoldersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder[]> GetFoldersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetFolderResponse")]
        SolidCP.Providers.Web.WebFolder GetFolder(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder> GetFolderAsync(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateFolderResponse")]
        void UpdateFolder(string siteId, SolidCP.Providers.Web.WebFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateFolderResponse")]
        System.Threading.Tasks.Task UpdateFolderAsync(string siteId, SolidCP.Providers.Web.WebFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteFolderResponse")]
        void DeleteFolder(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteFolderResponse")]
        System.Threading.Tasks.Task DeleteFolderAsync(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetUsers", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetUsersResponse")]
        SolidCP.Providers.Web.WebUser[] /*List*/ GetUsers(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetUsers", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetUsersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser[]> GetUsersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetUserResponse")]
        SolidCP.Providers.Web.WebUser GetUser(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser> GetUserAsync(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateUserResponse")]
        void UpdateUser(string siteId, SolidCP.Providers.Web.WebUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateUserResponse")]
        System.Threading.Tasks.Task UpdateUserAsync(string siteId, SolidCP.Providers.Web.WebUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteUserResponse")]
        void DeleteUser(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteUserResponse")]
        System.Threading.Tasks.Task DeleteUserAsync(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGroups", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGroupsResponse")]
        SolidCP.Providers.Web.WebGroup[] /*List*/ GetGroups(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGroups", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetGroupsAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGroupResponse")]
        SolidCP.Providers.Web.WebGroup GetGroup(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGroupResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetGroupAsync(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateGroupResponse")]
        void UpdateGroup(string siteId, SolidCP.Providers.Web.WebGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateGroupResponse")]
        System.Threading.Tasks.Task UpdateGroupAsync(string siteId, SolidCP.Providers.Web.WebGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteGroupResponse")]
        void DeleteGroup(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteGroupResponse")]
        System.Threading.Tasks.Task DeleteGroupAsync(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeStatus", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeStatusResponse")]
        SolidCP.Providers.ResultObjects.HeliconApeStatus GetHeliconApeStatus(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeStatus", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeStatusResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.HeliconApeStatus> GetHeliconApeStatusAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallHeliconApe", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallHeliconApeResponse")]
        void InstallHeliconApe(string ServiceId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallHeliconApe", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallHeliconApeResponse")]
        System.Threading.Tasks.Task InstallHeliconApeAsync(string ServiceId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/EnableHeliconApe", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/EnableHeliconApeResponse")]
        void EnableHeliconApe(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/EnableHeliconApe", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/EnableHeliconApeResponse")]
        System.Threading.Tasks.Task EnableHeliconApeAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DisableHeliconApe", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DisableHeliconApeResponse")]
        void DisableHeliconApe(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DisableHeliconApe", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DisableHeliconApeResponse")]
        System.Threading.Tasks.Task DisableHeliconApeAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFoldersResponse")]
        SolidCP.Providers.Web.HtaccessFolder[] /*List*/ GetHeliconApeFolders(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFoldersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder[]> GetHeliconApeFoldersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeHttpdFolderResponse")]
        SolidCP.Providers.Web.HtaccessFolder GetHeliconApeHttpdFolder();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeHttpdFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeHttpdFolderAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFolderResponse")]
        SolidCP.Providers.Web.HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeFolderAsync(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeFolderResponse")]
        void UpdateHeliconApeFolder(string siteId, SolidCP.Providers.Web.HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeFolderResponse")]
        System.Threading.Tasks.Task UpdateHeliconApeFolderAsync(string siteId, SolidCP.Providers.Web.HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeHttpdFolderResponse")]
        void UpdateHeliconApeHttpdFolder(SolidCP.Providers.Web.HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeHttpdFolderResponse")]
        System.Threading.Tasks.Task UpdateHeliconApeHttpdFolderAsync(SolidCP.Providers.Web.HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeFolderResponse")]
        void DeleteHeliconApeFolder(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeFolderResponse")]
        System.Threading.Tasks.Task DeleteHeliconApeFolderAsync(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUsers", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUsersResponse")]
        SolidCP.Providers.Web.HtaccessUser[] /*List*/ GetHeliconApeUsers(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUsers", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUsersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser[]> GetHeliconApeUsersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUserResponse")]
        SolidCP.Providers.Web.HtaccessUser GetHeliconApeUser(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser> GetHeliconApeUserAsync(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeUserResponse")]
        void UpdateHeliconApeUser(string siteId, SolidCP.Providers.Web.HtaccessUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeUserResponse")]
        System.Threading.Tasks.Task UpdateHeliconApeUserAsync(string siteId, SolidCP.Providers.Web.HtaccessUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeUserResponse")]
        void DeleteHeliconApeUser(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeUserResponse")]
        System.Threading.Tasks.Task DeleteHeliconApeUserAsync(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroups", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroupsResponse")]
        SolidCP.Providers.Web.WebGroup[] /*List*/ GetHeliconApeGroups(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroups", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetHeliconApeGroupsAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroupResponse")]
        SolidCP.Providers.Web.WebGroup GetHeliconApeGroup(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroupResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetHeliconApeGroupAsync(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeGroupResponse")]
        void UpdateHeliconApeGroup(string siteId, SolidCP.Providers.Web.WebGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeGroupResponse")]
        System.Threading.Tasks.Task UpdateHeliconApeGroupAsync(string siteId, SolidCP.Providers.Web.WebGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GrantWebDeployPublishingAccess", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GrantWebDeployPublishingAccessResponse")]
        void GrantWebDeployPublishingAccess(string siteId, string accountName, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GrantWebDeployPublishingAccess", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GrantWebDeployPublishingAccessResponse")]
        System.Threading.Tasks.Task GrantWebDeployPublishingAccessAsync(string siteId, string accountName, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/RevokeWebDeployPublishingAccess", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/RevokeWebDeployPublishingAccessResponse")]
        void RevokeWebDeployPublishingAccess(string siteId, string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/RevokeWebDeployPublishingAccess", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/RevokeWebDeployPublishingAccessResponse")]
        System.Threading.Tasks.Task RevokeWebDeployPublishingAccessAsync(string siteId, string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeGroupResponse")]
        void DeleteHeliconApeGroup(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeGroupResponse")]
        System.Threading.Tasks.Task DeleteHeliconApeGroupAsync(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetZooApplications", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetZooApplicationsResponse")]
        SolidCP.Providers.Web.WebAppVirtualDirectory[] GetZooApplications(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetZooApplications", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetZooApplicationsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetZooApplicationsAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooEnvironmentVariable", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooEnvironmentVariableResponse")]
        SolidCP.Providers.ResultObjects.StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooEnvironmentVariable", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooEnvironmentVariableResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooEnvironmentVariableAsync(string siteId, string appName, string envName, string envValue);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleEnabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleEnabledResponse")]
        SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleEnabled(string siteId, string appName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleEnabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleEnabledResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleEnabledAsync(string siteId, string appName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleDisabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleDisabledResponse")]
        SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleDisabled(string siteId, string appName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleDisabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleDisabledResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleDisabledAsync(string siteId, string appName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckLoadUserProfile", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckLoadUserProfileResponse")]
        bool CheckLoadUserProfile();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckLoadUserProfile", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckLoadUserProfileResponse")]
        System.Threading.Tasks.Task<bool> CheckLoadUserProfileAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/EnableLoadUserProfile", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/EnableLoadUserProfileResponse")]
        void EnableLoadUserProfile();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/EnableLoadUserProfile", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/EnableLoadUserProfileResponse")]
        System.Threading.Tasks.Task EnableLoadUserProfileAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InitFeeds", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InitFeedsResponse")]
        void InitFeeds(int UserId, string[] feeds);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InitFeeds", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InitFeedsResponse")]
        System.Threading.Tasks.Task InitFeedsAsync(int UserId, string[] feeds);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetResourceLanguage", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetResourceLanguageResponse")]
        void SetResourceLanguage(int UserId, string resourceLanguage);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetResourceLanguage", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetResourceLanguageResponse")]
        System.Threading.Tasks.Task SetResourceLanguageAsync(int UserId, string resourceLanguage);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryLanguages", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryLanguagesResponse")]
        SolidCP.Providers.ResultObjects.GalleryLanguagesResult GetGalleryLanguages(int UserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryLanguages", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryLanguagesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryLanguagesResult> GetGalleryLanguagesAsync(int UserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryCategories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryCategoriesResponse")]
        SolidCP.Providers.ResultObjects.GalleryCategoriesResult GetGalleryCategories(int UserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryCategories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryCategoriesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryCategoriesResult> GetGalleryCategoriesAsync(int UserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplications", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsResponse")]
        SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplications", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsAsync(int UserId, string categoryId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsFiltered", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsFilteredResponse")]
        SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsFiltered", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsFilteredResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsFilteredAsync(int UserId, string pattern);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/IsMsDeployInstalled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/IsMsDeployInstalledResponse")]
        bool IsMsDeployInstalled();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/IsMsDeployInstalled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/IsMsDeployInstalledResponse")]
        System.Threading.Tasks.Task<bool> IsMsDeployInstalledAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationResponse")]
        SolidCP.Providers.ResultObjects.GalleryApplicationResult GetGalleryApplication(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationResult> GetGalleryApplicationAsync(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationStatus", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationStatusResponse")]
        SolidCP.Providers.WebAppGallery.GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationStatus", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationStatusResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> GetGalleryApplicationStatusAsync(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DownloadGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DownloadGalleryApplicationResponse")]
        SolidCP.Providers.WebAppGallery.GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DownloadGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DownloadGalleryApplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> DownloadGalleryApplicationAsync(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationParameters", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationParametersResponse")]
        SolidCP.Providers.ResultObjects.DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationParameters", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationParametersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.DeploymentParametersResult> GetGalleryApplicationParametersAsync(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallGalleryApplicationResponse")]
        SolidCP.Providers.ResultObjects.StringResultObject InstallGalleryApplication(int UserId, string id, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ updatedValues, string languageId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallGalleryApplicationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> InstallGalleryApplicationAsync(int UserId, string id, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ updatedValues, string languageId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementAccountExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementAccountExistsResponse")]
        bool CheckWebManagementAccountExists(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementAccountExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementAccountExistsResponse")]
        System.Threading.Tasks.Task<bool> CheckWebManagementAccountExistsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementPasswordComplexity", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementPasswordComplexityResponse")]
        SolidCP.Providers.Common.ResultObject CheckWebManagementPasswordComplexity(string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementPasswordComplexity", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementPasswordComplexityResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CheckWebManagementPasswordComplexityAsync(string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GrantWebManagementAccess", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GrantWebManagementAccessResponse")]
        void GrantWebManagementAccess(string siteId, string accountName, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GrantWebManagementAccess", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GrantWebManagementAccessResponse")]
        System.Threading.Tasks.Task GrantWebManagementAccessAsync(string siteId, string accountName, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/RevokeWebManagementAccess", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/RevokeWebManagementAccessResponse")]
        void RevokeWebManagementAccess(string siteId, string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/RevokeWebManagementAccess", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/RevokeWebManagementAccessResponse")]
        System.Threading.Tasks.Task RevokeWebManagementAccessAsync(string siteId, string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeWebManagementAccessPassword", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeWebManagementAccessPasswordResponse")]
        void ChangeWebManagementAccessPassword(string accountName, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeWebManagementAccessPassword", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeWebManagementAccessPasswordResponse")]
        System.Threading.Tasks.Task ChangeWebManagementAccessPasswordAsync(string accountName, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/generateCSR", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/generateCSRResponse")]
        SolidCP.Providers.Web.SSLCertificate generateCSR(SolidCP.Providers.Web.SSLCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/generateCSR", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/generateCSRResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> generateCSRAsync(SolidCP.Providers.Web.SSLCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/generateRenewalCSR", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/generateRenewalCSRResponse")]
        SolidCP.Providers.Web.SSLCertificate generateRenewalCSR(SolidCP.Providers.Web.SSLCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/generateRenewalCSR", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/generateRenewalCSRResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> generateRenewalCSRAsync(SolidCP.Providers.Web.SSLCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/getCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/getCertificateResponse")]
        SolidCP.Providers.Web.SSLCertificate getCertificate(SolidCP.Providers.Web.WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/getCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/getCertificateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> getCertificateAsync(SolidCP.Providers.Web.WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/installCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/installCertificateResponse")]
        SolidCP.Providers.Web.SSLCertificate installCertificate(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/installCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/installCertificateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> installCertificateAsync(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/LEinstallCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/LEinstallCertificateResponse")]
        System.String LEinstallCertificate(SolidCP.Providers.Web.WebSite website, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/LEinstallCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/LEinstallCertificateResponse")]
        System.Threading.Tasks.Task<System.String> LEinstallCertificateAsync(SolidCP.Providers.Web.WebSite website, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/installPFX", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/installPFXResponse")]
        SolidCP.Providers.Web.SSLCertificate installPFX(byte[] certificate, string password, SolidCP.Providers.Web.WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/installPFX", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/installPFXResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> installPFXAsync(byte[] certificate, string password, SolidCP.Providers.Web.WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/exportCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/exportCertificateResponse")]
        byte[] exportCertificate(string serialNumber, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/exportCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/exportCertificateResponse")]
        System.Threading.Tasks.Task<byte[]> exportCertificateAsync(string serialNumber, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/getServerCertificates", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/getServerCertificatesResponse")]
        SolidCP.Providers.Web.SSLCertificate[] /*List*/ getServerCertificates();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/getServerCertificates", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/getServerCertificatesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate[]> getServerCertificatesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteCertificateResponse")]
        SolidCP.Providers.Common.ResultObject DeleteCertificate(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteCertificateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCertificateAsync(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ImportCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ImportCertificateResponse")]
        SolidCP.Providers.Web.SSLCertificate ImportCertificate(SolidCP.Providers.Web.WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ImportCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ImportCertificateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> ImportCertificateAsync(SolidCP.Providers.Web.WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckCertificateResponse")]
        bool CheckCertificate(SolidCP.Providers.Web.WebSite webSite);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckCertificateResponse")]
        System.Threading.Tasks.Task<bool> CheckCertificateAsync(SolidCP.Providers.Web.WebSite webSite);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetDirectoryBrowseEnabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetDirectoryBrowseEnabledResponse")]
        bool GetDirectoryBrowseEnabled(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetDirectoryBrowseEnabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetDirectoryBrowseEnabledResponse")]
        System.Threading.Tasks.Task<bool> GetDirectoryBrowseEnabledAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetDirectoryBrowseEnabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetDirectoryBrowseEnabledResponse")]
        void SetDirectoryBrowseEnabled(string siteId, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetDirectoryBrowseEnabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetDirectoryBrowseEnabledResponse")]
        System.Threading.Tasks.Task SetDirectoryBrowseEnabledAsync(string siteId, bool enabled);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class WebServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IWebServer
    {
        public void ChangeSiteState(string siteId, SolidCP.Providers.ServerState state)
        {
            Invoke("SolidCP.Server.WebServer", "ChangeSiteState", siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, SolidCP.Providers.ServerState state)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "ChangeSiteState", siteId, state);
        }

        public SolidCP.Providers.ServerState GetSiteState(string siteId)
        {
            return Invoke<SolidCP.Providers.ServerState>("SolidCP.Server.WebServer", "GetSiteState", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ServerState> GetSiteStateAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.ServerState>("SolidCP.Server.WebServer", "GetSiteState", siteId);
        }

        public string GetSiteId(string siteName)
        {
            return Invoke<string>("SolidCP.Server.WebServer", "GetSiteId", siteName);
        }

        public async System.Threading.Tasks.Task<string> GetSiteIdAsync(string siteName)
        {
            return await InvokeAsync<string>("SolidCP.Server.WebServer", "GetSiteId", siteName);
        }

        public string[] GetSitesAccounts(string[] siteIds)
        {
            return Invoke<string[]>("SolidCP.Server.WebServer", "GetSitesAccounts", siteIds);
        }

        public async System.Threading.Tasks.Task<string[]> GetSitesAccountsAsync(string[] siteIds)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.WebServer", "GetSitesAccounts", siteIds);
        }

        public bool SiteExists(string siteId)
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "SiteExists", siteId);
        }

        public async System.Threading.Tasks.Task<bool> SiteExistsAsync(string siteId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "SiteExists", siteId);
        }

        public string[] GetSites()
        {
            return Invoke<string[]>("SolidCP.Server.WebServer", "GetSites");
        }

        public async System.Threading.Tasks.Task<string[]> GetSitesAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.WebServer", "GetSites");
        }

        public SolidCP.Providers.Web.WebSite GetSite(string siteId)
        {
            return Invoke<SolidCP.Providers.Web.WebSite>("SolidCP.Server.WebServer", "GetSite", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebSite> GetSiteAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebSite>("SolidCP.Server.WebServer", "GetSite", siteId);
        }

        public SolidCP.Providers.ServerBinding[] GetSiteBindings(string siteId)
        {
            return Invoke<SolidCP.Providers.ServerBinding[]>("SolidCP.Server.WebServer", "GetSiteBindings", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ServerBinding[]> GetSiteBindingsAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.ServerBinding[]>("SolidCP.Server.WebServer", "GetSiteBindings", siteId);
        }

        public string CreateSite(SolidCP.Providers.Web.WebSite site)
        {
            return Invoke<string>("SolidCP.Server.WebServer", "CreateSite", site);
        }

        public async System.Threading.Tasks.Task<string> CreateSiteAsync(SolidCP.Providers.Web.WebSite site)
        {
            return await InvokeAsync<string>("SolidCP.Server.WebServer", "CreateSite", site);
        }

        public void UpdateSite(SolidCP.Providers.Web.WebSite site)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateSite", site);
        }

        public async System.Threading.Tasks.Task UpdateSiteAsync(SolidCP.Providers.Web.WebSite site)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateSite", site);
        }

        public void UpdateSiteBindings(string siteId, SolidCP.Providers.ServerBinding[] bindings, bool emptyBindingsAllowed)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateSiteBindings", siteId, bindings, emptyBindingsAllowed);
        }

        public async System.Threading.Tasks.Task UpdateSiteBindingsAsync(string siteId, SolidCP.Providers.ServerBinding[] bindings, bool emptyBindingsAllowed)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateSiteBindings", siteId, bindings, emptyBindingsAllowed);
        }

        public void DeleteSite(string siteId)
        {
            Invoke("SolidCP.Server.WebServer", "DeleteSite", siteId);
        }

        public async System.Threading.Tasks.Task DeleteSiteAsync(string siteId)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "DeleteSite", siteId);
        }

        public void ChangeAppPoolState(string siteId, SolidCP.Providers.AppPoolState state)
        {
            Invoke("SolidCP.Server.WebServer", "ChangeAppPoolState", siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeAppPoolStateAsync(string siteId, SolidCP.Providers.AppPoolState state)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "ChangeAppPoolState", siteId, state);
        }

        public SolidCP.Providers.AppPoolState GetAppPoolState(string siteId)
        {
            return Invoke<SolidCP.Providers.AppPoolState>("SolidCP.Server.WebServer", "GetAppPoolState", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.AppPoolState> GetAppPoolStateAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.AppPoolState>("SolidCP.Server.WebServer", "GetAppPoolState", siteId);
        }

        public bool VirtualDirectoryExists(string siteId, string directoryName)
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "VirtualDirectoryExists", siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<bool> VirtualDirectoryExistsAsync(string siteId, string directoryName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "VirtualDirectoryExists", siteId, directoryName);
        }

        public SolidCP.Providers.Web.WebVirtualDirectory[] GetVirtualDirectories(string siteId)
        {
            return Invoke<SolidCP.Providers.Web.WebVirtualDirectory[]>("SolidCP.Server.WebServer", "GetVirtualDirectories", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory[]> GetVirtualDirectoriesAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebVirtualDirectory[]>("SolidCP.Server.WebServer", "GetVirtualDirectories", siteId);
        }

        public SolidCP.Providers.Web.WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
        {
            return Invoke<SolidCP.Providers.Web.WebVirtualDirectory>("SolidCP.Server.WebServer", "GetVirtualDirectory", siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory> GetVirtualDirectoryAsync(string siteId, string directoryName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebVirtualDirectory>("SolidCP.Server.WebServer", "GetVirtualDirectory", siteId, directoryName);
        }

        public void CreateVirtualDirectory(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory)
        {
            Invoke("SolidCP.Server.WebServer", "CreateVirtualDirectory", siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "CreateVirtualDirectory", siteId, directory);
        }

        public void UpdateVirtualDirectory(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateVirtualDirectory", siteId, directory);
        }

        public async System.Threading.Tasks.Task UpdateVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateVirtualDirectory", siteId, directory);
        }

        public void DeleteVirtualDirectory(string siteId, string directoryName)
        {
            Invoke("SolidCP.Server.WebServer", "DeleteVirtualDirectory", siteId, directoryName);
        }

        public async System.Threading.Tasks.Task DeleteVirtualDirectoryAsync(string siteId, string directoryName)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "DeleteVirtualDirectory", siteId, directoryName);
        }

        public bool AppVirtualDirectoryExists(string siteId, string directoryName)
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "AppVirtualDirectoryExists", siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<bool> AppVirtualDirectoryExistsAsync(string siteId, string directoryName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "AppVirtualDirectoryExists", siteId, directoryName);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId)
        {
            return Invoke<SolidCP.Providers.Web.WebAppVirtualDirectory[]>("SolidCP.Server.WebServer", "GetAppVirtualDirectories", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetAppVirtualDirectoriesAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebAppVirtualDirectory[]>("SolidCP.Server.WebServer", "GetAppVirtualDirectories", siteId);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName)
        {
            return Invoke<SolidCP.Providers.Web.WebAppVirtualDirectory>("SolidCP.Server.WebServer", "GetAppVirtualDirectory", siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory> GetAppVirtualDirectoryAsync(string siteId, string directoryName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebAppVirtualDirectory>("SolidCP.Server.WebServer", "GetAppVirtualDirectory", siteId, directoryName);
        }

        public void CreateAppVirtualDirectory(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            Invoke("SolidCP.Server.WebServer", "CreateAppVirtualDirectory", siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateAppVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "CreateAppVirtualDirectory", siteId, directory);
        }

        public void CreateEnterpriseStorageAppVirtualDirectory(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            Invoke("SolidCP.Server.WebServer", "CreateEnterpriseStorageAppVirtualDirectory", siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateEnterpriseStorageAppVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "CreateEnterpriseStorageAppVirtualDirectory", siteId, directory);
        }

        public void UpdateAppVirtualDirectory(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateAppVirtualDirectory", siteId, directory);
        }

        public async System.Threading.Tasks.Task UpdateAppVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateAppVirtualDirectory", siteId, directory);
        }

        public void DeleteAppVirtualDirectory(string siteId, string directoryName)
        {
            Invoke("SolidCP.Server.WebServer", "DeleteAppVirtualDirectory", siteId, directoryName);
        }

        public async System.Threading.Tasks.Task DeleteAppVirtualDirectoryAsync(string siteId, string directoryName)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "DeleteAppVirtualDirectory", siteId, directoryName);
        }

        public bool IsFrontPageSystemInstalled()
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "IsFrontPageSystemInstalled");
        }

        public async System.Threading.Tasks.Task<bool> IsFrontPageSystemInstalledAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "IsFrontPageSystemInstalled");
        }

        public bool IsFrontPageInstalled(string siteId)
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "IsFrontPageInstalled", siteId);
        }

        public async System.Threading.Tasks.Task<bool> IsFrontPageInstalledAsync(string siteId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "IsFrontPageInstalled", siteId);
        }

        public bool InstallFrontPage(string siteId, string username, string password)
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "InstallFrontPage", siteId, username, password);
        }

        public async System.Threading.Tasks.Task<bool> InstallFrontPageAsync(string siteId, string username, string password)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "InstallFrontPage", siteId, username, password);
        }

        public void UninstallFrontPage(string siteId, string username)
        {
            Invoke("SolidCP.Server.WebServer", "UninstallFrontPage", siteId, username);
        }

        public async System.Threading.Tasks.Task UninstallFrontPageAsync(string siteId, string username)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UninstallFrontPage", siteId, username);
        }

        public void ChangeFrontPagePassword(string username, string password)
        {
            Invoke("SolidCP.Server.WebServer", "ChangeFrontPagePassword", username, password);
        }

        public async System.Threading.Tasks.Task ChangeFrontPagePasswordAsync(string username, string password)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "ChangeFrontPagePassword", username, password);
        }

        public bool IsColdFusionSystemInstalled()
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "IsColdFusionSystemInstalled");
        }

        public async System.Threading.Tasks.Task<bool> IsColdFusionSystemInstalledAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "IsColdFusionSystemInstalled");
        }

        public void GrantWebSiteAccess(string path, string siteId, SolidCP.Providers.NTFSPermission permission)
        {
            Invoke("SolidCP.Server.WebServer", "GrantWebSiteAccess", path, siteId, permission);
        }

        public async System.Threading.Tasks.Task GrantWebSiteAccessAsync(string path, string siteId, SolidCP.Providers.NTFSPermission permission)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "GrantWebSiteAccess", path, siteId, permission);
        }

        public void InstallSecuredFolders(string siteId)
        {
            Invoke("SolidCP.Server.WebServer", "InstallSecuredFolders", siteId);
        }

        public async System.Threading.Tasks.Task InstallSecuredFoldersAsync(string siteId)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "InstallSecuredFolders", siteId);
        }

        public void UninstallSecuredFolders(string siteId)
        {
            Invoke("SolidCP.Server.WebServer", "UninstallSecuredFolders", siteId);
        }

        public async System.Threading.Tasks.Task UninstallSecuredFoldersAsync(string siteId)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UninstallSecuredFolders", siteId);
        }

        public SolidCP.Providers.Web.WebFolder[] /*List*/ GetFolders(string siteId)
        {
            return Invoke<SolidCP.Providers.Web.WebFolder[], SolidCP.Providers.Web.WebFolder>("SolidCP.Server.WebServer", "GetFolders", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder[]> GetFoldersAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebFolder[], SolidCP.Providers.Web.WebFolder>("SolidCP.Server.WebServer", "GetFolders", siteId);
        }

        public SolidCP.Providers.Web.WebFolder GetFolder(string siteId, string folderPath)
        {
            return Invoke<SolidCP.Providers.Web.WebFolder>("SolidCP.Server.WebServer", "GetFolder", siteId, folderPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder> GetFolderAsync(string siteId, string folderPath)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebFolder>("SolidCP.Server.WebServer", "GetFolder", siteId, folderPath);
        }

        public void UpdateFolder(string siteId, SolidCP.Providers.Web.WebFolder folder)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateFolder", siteId, folder);
        }

        public async System.Threading.Tasks.Task UpdateFolderAsync(string siteId, SolidCP.Providers.Web.WebFolder folder)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateFolder", siteId, folder);
        }

        public void DeleteFolder(string siteId, string folderPath)
        {
            Invoke("SolidCP.Server.WebServer", "DeleteFolder", siteId, folderPath);
        }

        public async System.Threading.Tasks.Task DeleteFolderAsync(string siteId, string folderPath)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "DeleteFolder", siteId, folderPath);
        }

        public SolidCP.Providers.Web.WebUser[] /*List*/ GetUsers(string siteId)
        {
            return Invoke<SolidCP.Providers.Web.WebUser[], SolidCP.Providers.Web.WebUser>("SolidCP.Server.WebServer", "GetUsers", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser[]> GetUsersAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebUser[], SolidCP.Providers.Web.WebUser>("SolidCP.Server.WebServer", "GetUsers", siteId);
        }

        public SolidCP.Providers.Web.WebUser GetUser(string siteId, string userName)
        {
            return Invoke<SolidCP.Providers.Web.WebUser>("SolidCP.Server.WebServer", "GetUser", siteId, userName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser> GetUserAsync(string siteId, string userName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebUser>("SolidCP.Server.WebServer", "GetUser", siteId, userName);
        }

        public void UpdateUser(string siteId, SolidCP.Providers.Web.WebUser user)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateUser", siteId, user);
        }

        public async System.Threading.Tasks.Task UpdateUserAsync(string siteId, SolidCP.Providers.Web.WebUser user)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateUser", siteId, user);
        }

        public void DeleteUser(string siteId, string userName)
        {
            Invoke("SolidCP.Server.WebServer", "DeleteUser", siteId, userName);
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(string siteId, string userName)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "DeleteUser", siteId, userName);
        }

        public SolidCP.Providers.Web.WebGroup[] /*List*/ GetGroups(string siteId)
        {
            return Invoke<SolidCP.Providers.Web.WebGroup[], SolidCP.Providers.Web.WebGroup>("SolidCP.Server.WebServer", "GetGroups", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetGroupsAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebGroup[], SolidCP.Providers.Web.WebGroup>("SolidCP.Server.WebServer", "GetGroups", siteId);
        }

        public SolidCP.Providers.Web.WebGroup GetGroup(string siteId, string groupName)
        {
            return Invoke<SolidCP.Providers.Web.WebGroup>("SolidCP.Server.WebServer", "GetGroup", siteId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetGroupAsync(string siteId, string groupName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebGroup>("SolidCP.Server.WebServer", "GetGroup", siteId, groupName);
        }

        public void UpdateGroup(string siteId, SolidCP.Providers.Web.WebGroup group)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateGroup", siteId, group);
        }

        public async System.Threading.Tasks.Task UpdateGroupAsync(string siteId, SolidCP.Providers.Web.WebGroup group)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateGroup", siteId, group);
        }

        public void DeleteGroup(string siteId, string groupName)
        {
            Invoke("SolidCP.Server.WebServer", "DeleteGroup", siteId, groupName);
        }

        public async System.Threading.Tasks.Task DeleteGroupAsync(string siteId, string groupName)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "DeleteGroup", siteId, groupName);
        }

        public SolidCP.Providers.ResultObjects.HeliconApeStatus GetHeliconApeStatus(string siteId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.HeliconApeStatus>("SolidCP.Server.WebServer", "GetHeliconApeStatus", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.HeliconApeStatus> GetHeliconApeStatusAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.HeliconApeStatus>("SolidCP.Server.WebServer", "GetHeliconApeStatus", siteId);
        }

        public void InstallHeliconApe(string ServiceId)
        {
            Invoke("SolidCP.Server.WebServer", "InstallHeliconApe", ServiceId);
        }

        public async System.Threading.Tasks.Task InstallHeliconApeAsync(string ServiceId)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "InstallHeliconApe", ServiceId);
        }

        public void EnableHeliconApe(string siteId)
        {
            Invoke("SolidCP.Server.WebServer", "EnableHeliconApe", siteId);
        }

        public async System.Threading.Tasks.Task EnableHeliconApeAsync(string siteId)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "EnableHeliconApe", siteId);
        }

        public void DisableHeliconApe(string siteId)
        {
            Invoke("SolidCP.Server.WebServer", "DisableHeliconApe", siteId);
        }

        public async System.Threading.Tasks.Task DisableHeliconApeAsync(string siteId)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "DisableHeliconApe", siteId);
        }

        public SolidCP.Providers.Web.HtaccessFolder[] /*List*/ GetHeliconApeFolders(string siteId)
        {
            return Invoke<SolidCP.Providers.Web.HtaccessFolder[], SolidCP.Providers.Web.HtaccessFolder>("SolidCP.Server.WebServer", "GetHeliconApeFolders", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder[]> GetHeliconApeFoldersAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.HtaccessFolder[], SolidCP.Providers.Web.HtaccessFolder>("SolidCP.Server.WebServer", "GetHeliconApeFolders", siteId);
        }

        public SolidCP.Providers.Web.HtaccessFolder GetHeliconApeHttpdFolder()
        {
            return Invoke<SolidCP.Providers.Web.HtaccessFolder>("SolidCP.Server.WebServer", "GetHeliconApeHttpdFolder");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeHttpdFolderAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Web.HtaccessFolder>("SolidCP.Server.WebServer", "GetHeliconApeHttpdFolder");
        }

        public SolidCP.Providers.Web.HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath)
        {
            return Invoke<SolidCP.Providers.Web.HtaccessFolder>("SolidCP.Server.WebServer", "GetHeliconApeFolder", siteId, folderPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeFolderAsync(string siteId, string folderPath)
        {
            return await InvokeAsync<SolidCP.Providers.Web.HtaccessFolder>("SolidCP.Server.WebServer", "GetHeliconApeFolder", siteId, folderPath);
        }

        public void UpdateHeliconApeFolder(string siteId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateHeliconApeFolder", siteId, folder);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeFolderAsync(string siteId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateHeliconApeFolder", siteId, folder);
        }

        public void UpdateHeliconApeHttpdFolder(SolidCP.Providers.Web.HtaccessFolder folder)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateHeliconApeHttpdFolder", folder);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeHttpdFolderAsync(SolidCP.Providers.Web.HtaccessFolder folder)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateHeliconApeHttpdFolder", folder);
        }

        public void DeleteHeliconApeFolder(string siteId, string folderPath)
        {
            Invoke("SolidCP.Server.WebServer", "DeleteHeliconApeFolder", siteId, folderPath);
        }

        public async System.Threading.Tasks.Task DeleteHeliconApeFolderAsync(string siteId, string folderPath)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "DeleteHeliconApeFolder", siteId, folderPath);
        }

        public SolidCP.Providers.Web.HtaccessUser[] /*List*/ GetHeliconApeUsers(string siteId)
        {
            return Invoke<SolidCP.Providers.Web.HtaccessUser[], SolidCP.Providers.Web.HtaccessUser>("SolidCP.Server.WebServer", "GetHeliconApeUsers", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser[]> GetHeliconApeUsersAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.HtaccessUser[], SolidCP.Providers.Web.HtaccessUser>("SolidCP.Server.WebServer", "GetHeliconApeUsers", siteId);
        }

        public SolidCP.Providers.Web.HtaccessUser GetHeliconApeUser(string siteId, string userName)
        {
            return Invoke<SolidCP.Providers.Web.HtaccessUser>("SolidCP.Server.WebServer", "GetHeliconApeUser", siteId, userName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser> GetHeliconApeUserAsync(string siteId, string userName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.HtaccessUser>("SolidCP.Server.WebServer", "GetHeliconApeUser", siteId, userName);
        }

        public void UpdateHeliconApeUser(string siteId, SolidCP.Providers.Web.HtaccessUser user)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateHeliconApeUser", siteId, user);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeUserAsync(string siteId, SolidCP.Providers.Web.HtaccessUser user)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateHeliconApeUser", siteId, user);
        }

        public void DeleteHeliconApeUser(string siteId, string userName)
        {
            Invoke("SolidCP.Server.WebServer", "DeleteHeliconApeUser", siteId, userName);
        }

        public async System.Threading.Tasks.Task DeleteHeliconApeUserAsync(string siteId, string userName)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "DeleteHeliconApeUser", siteId, userName);
        }

        public SolidCP.Providers.Web.WebGroup[] /*List*/ GetHeliconApeGroups(string siteId)
        {
            return Invoke<SolidCP.Providers.Web.WebGroup[], SolidCP.Providers.Web.WebGroup>("SolidCP.Server.WebServer", "GetHeliconApeGroups", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetHeliconApeGroupsAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebGroup[], SolidCP.Providers.Web.WebGroup>("SolidCP.Server.WebServer", "GetHeliconApeGroups", siteId);
        }

        public SolidCP.Providers.Web.WebGroup GetHeliconApeGroup(string siteId, string groupName)
        {
            return Invoke<SolidCP.Providers.Web.WebGroup>("SolidCP.Server.WebServer", "GetHeliconApeGroup", siteId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetHeliconApeGroupAsync(string siteId, string groupName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebGroup>("SolidCP.Server.WebServer", "GetHeliconApeGroup", siteId, groupName);
        }

        public void UpdateHeliconApeGroup(string siteId, SolidCP.Providers.Web.WebGroup group)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateHeliconApeGroup", siteId, group);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeGroupAsync(string siteId, SolidCP.Providers.Web.WebGroup group)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateHeliconApeGroup", siteId, group);
        }

        public void GrantWebDeployPublishingAccess(string siteId, string accountName, string accountPassword)
        {
            Invoke("SolidCP.Server.WebServer", "GrantWebDeployPublishingAccess", siteId, accountName, accountPassword);
        }

        public async System.Threading.Tasks.Task GrantWebDeployPublishingAccessAsync(string siteId, string accountName, string accountPassword)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "GrantWebDeployPublishingAccess", siteId, accountName, accountPassword);
        }

        public void RevokeWebDeployPublishingAccess(string siteId, string accountName)
        {
            Invoke("SolidCP.Server.WebServer", "RevokeWebDeployPublishingAccess", siteId, accountName);
        }

        public async System.Threading.Tasks.Task RevokeWebDeployPublishingAccessAsync(string siteId, string accountName)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "RevokeWebDeployPublishingAccess", siteId, accountName);
        }

        public void DeleteHeliconApeGroup(string siteId, string groupName)
        {
            Invoke("SolidCP.Server.WebServer", "DeleteHeliconApeGroup", siteId, groupName);
        }

        public async System.Threading.Tasks.Task DeleteHeliconApeGroupAsync(string siteId, string groupName)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "DeleteHeliconApeGroup", siteId, groupName);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory[] GetZooApplications(string siteId)
        {
            return Invoke<SolidCP.Providers.Web.WebAppVirtualDirectory[]>("SolidCP.Server.WebServer", "GetZooApplications", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetZooApplicationsAsync(string siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebAppVirtualDirectory[]>("SolidCP.Server.WebServer", "GetZooApplications", siteId);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue)
        {
            return Invoke<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.Server.WebServer", "SetZooEnvironmentVariable", siteId, appName, envName, envValue);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooEnvironmentVariableAsync(string siteId, string appName, string envName, string envValue)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.Server.WebServer", "SetZooEnvironmentVariable", siteId, appName, envName, envValue);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleEnabled(string siteId, string appName)
        {
            return Invoke<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.Server.WebServer", "SetZooConsoleEnabled", siteId, appName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleEnabledAsync(string siteId, string appName)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.Server.WebServer", "SetZooConsoleEnabled", siteId, appName);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleDisabled(string siteId, string appName)
        {
            return Invoke<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.Server.WebServer", "SetZooConsoleDisabled", siteId, appName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleDisabledAsync(string siteId, string appName)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.Server.WebServer", "SetZooConsoleDisabled", siteId, appName);
        }

        public bool CheckLoadUserProfile()
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "CheckLoadUserProfile");
        }

        public async System.Threading.Tasks.Task<bool> CheckLoadUserProfileAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "CheckLoadUserProfile");
        }

        public void EnableLoadUserProfile()
        {
            Invoke("SolidCP.Server.WebServer", "EnableLoadUserProfile");
        }

        public async System.Threading.Tasks.Task EnableLoadUserProfileAsync()
        {
            await InvokeAsync("SolidCP.Server.WebServer", "EnableLoadUserProfile");
        }

        public void InitFeeds(int UserId, string[] feeds)
        {
            Invoke("SolidCP.Server.WebServer", "InitFeeds", UserId, feeds);
        }

        public async System.Threading.Tasks.Task InitFeedsAsync(int UserId, string[] feeds)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "InitFeeds", UserId, feeds);
        }

        public void SetResourceLanguage(int UserId, string resourceLanguage)
        {
            Invoke("SolidCP.Server.WebServer", "SetResourceLanguage", UserId, resourceLanguage);
        }

        public async System.Threading.Tasks.Task SetResourceLanguageAsync(int UserId, string resourceLanguage)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "SetResourceLanguage", UserId, resourceLanguage);
        }

        public SolidCP.Providers.ResultObjects.GalleryLanguagesResult GetGalleryLanguages(int UserId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryLanguagesResult>("SolidCP.Server.WebServer", "GetGalleryLanguages", UserId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryLanguagesResult> GetGalleryLanguagesAsync(int UserId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryLanguagesResult>("SolidCP.Server.WebServer", "GetGalleryLanguages", UserId);
        }

        public SolidCP.Providers.ResultObjects.GalleryCategoriesResult GetGalleryCategories(int UserId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryCategoriesResult>("SolidCP.Server.WebServer", "GetGalleryCategories", UserId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryCategoriesResult> GetGalleryCategoriesAsync(int UserId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryCategoriesResult>("SolidCP.Server.WebServer", "GetGalleryCategories", UserId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.Server.WebServer", "GetGalleryApplications", UserId, categoryId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsAsync(int UserId, string categoryId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.Server.WebServer", "GetGalleryApplications", UserId, categoryId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.Server.WebServer", "GetGalleryApplicationsFiltered", UserId, pattern);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsFilteredAsync(int UserId, string pattern)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryApplicationsResult>("SolidCP.Server.WebServer", "GetGalleryApplicationsFiltered", UserId, pattern);
        }

        public bool IsMsDeployInstalled()
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "IsMsDeployInstalled");
        }

        public async System.Threading.Tasks.Task<bool> IsMsDeployInstalledAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "IsMsDeployInstalled");
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationResult GetGalleryApplication(int UserId, string id)
        {
            return Invoke<SolidCP.Providers.ResultObjects.GalleryApplicationResult>("SolidCP.Server.WebServer", "GetGalleryApplication", UserId, id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationResult> GetGalleryApplicationAsync(int UserId, string id)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.GalleryApplicationResult>("SolidCP.Server.WebServer", "GetGalleryApplication", UserId, id);
        }

        public SolidCP.Providers.WebAppGallery.GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id)
        {
            return Invoke<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus>("SolidCP.Server.WebServer", "GetGalleryApplicationStatus", UserId, id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> GetGalleryApplicationStatusAsync(int UserId, string id)
        {
            return await InvokeAsync<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus>("SolidCP.Server.WebServer", "GetGalleryApplicationStatus", UserId, id);
        }

        public SolidCP.Providers.WebAppGallery.GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id)
        {
            return Invoke<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus>("SolidCP.Server.WebServer", "DownloadGalleryApplication", UserId, id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> DownloadGalleryApplicationAsync(int UserId, string id)
        {
            return await InvokeAsync<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus>("SolidCP.Server.WebServer", "DownloadGalleryApplication", UserId, id);
        }

        public SolidCP.Providers.ResultObjects.DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id)
        {
            return Invoke<SolidCP.Providers.ResultObjects.DeploymentParametersResult>("SolidCP.Server.WebServer", "GetGalleryApplicationParameters", UserId, id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.DeploymentParametersResult> GetGalleryApplicationParametersAsync(int UserId, string id)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.DeploymentParametersResult>("SolidCP.Server.WebServer", "GetGalleryApplicationParameters", UserId, id);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject InstallGalleryApplication(int UserId, string id, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ updatedValues, string languageId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.Server.WebServer", "InstallGalleryApplication", UserId, id, updatedValues.ToList(), languageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> InstallGalleryApplicationAsync(int UserId, string id, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ updatedValues, string languageId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.Server.WebServer", "InstallGalleryApplication", UserId, id, updatedValues, languageId);
        }

        public bool CheckWebManagementAccountExists(string accountName)
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "CheckWebManagementAccountExists", accountName);
        }

        public async System.Threading.Tasks.Task<bool> CheckWebManagementAccountExistsAsync(string accountName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "CheckWebManagementAccountExists", accountName);
        }

        public SolidCP.Providers.Common.ResultObject CheckWebManagementPasswordComplexity(string accountPassword)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.WebServer", "CheckWebManagementPasswordComplexity", accountPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CheckWebManagementPasswordComplexityAsync(string accountPassword)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.WebServer", "CheckWebManagementPasswordComplexity", accountPassword);
        }

        public void GrantWebManagementAccess(string siteId, string accountName, string accountPassword)
        {
            Invoke("SolidCP.Server.WebServer", "GrantWebManagementAccess", siteId, accountName, accountPassword);
        }

        public async System.Threading.Tasks.Task GrantWebManagementAccessAsync(string siteId, string accountName, string accountPassword)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "GrantWebManagementAccess", siteId, accountName, accountPassword);
        }

        public void RevokeWebManagementAccess(string siteId, string accountName)
        {
            Invoke("SolidCP.Server.WebServer", "RevokeWebManagementAccess", siteId, accountName);
        }

        public async System.Threading.Tasks.Task RevokeWebManagementAccessAsync(string siteId, string accountName)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "RevokeWebManagementAccess", siteId, accountName);
        }

        public void ChangeWebManagementAccessPassword(string accountName, string accountPassword)
        {
            Invoke("SolidCP.Server.WebServer", "ChangeWebManagementAccessPassword", accountName, accountPassword);
        }

        public async System.Threading.Tasks.Task ChangeWebManagementAccessPasswordAsync(string accountName, string accountPassword)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "ChangeWebManagementAccessPassword", accountName, accountPassword);
        }

        public SolidCP.Providers.Web.SSLCertificate generateCSR(SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "generateCSR", certificate);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> generateCSRAsync(SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "generateCSR", certificate);
        }

        public SolidCP.Providers.Web.SSLCertificate generateRenewalCSR(SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "generateRenewalCSR", certificate);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> generateRenewalCSRAsync(SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "generateRenewalCSR", certificate);
        }

        public SolidCP.Providers.Web.SSLCertificate getCertificate(SolidCP.Providers.Web.WebSite site)
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "getCertificate", site);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> getCertificateAsync(SolidCP.Providers.Web.WebSite site)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "getCertificate", site);
        }

        public SolidCP.Providers.Web.SSLCertificate installCertificate(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website)
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "installCertificate", certificate, website);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> installCertificateAsync(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "installCertificate", certificate, website);
        }

        public System.String LEinstallCertificate(SolidCP.Providers.Web.WebSite website, string email)
        {
            return Invoke<System.String>("SolidCP.Server.WebServer", "LEinstallCertificate", website, email);
        }

        public async System.Threading.Tasks.Task<System.String> LEinstallCertificateAsync(SolidCP.Providers.Web.WebSite website, string email)
        {
            return await InvokeAsync<System.String>("SolidCP.Server.WebServer", "LEinstallCertificate", website, email);
        }

        public SolidCP.Providers.Web.SSLCertificate installPFX(byte[] certificate, string password, SolidCP.Providers.Web.WebSite website)
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "installPFX", certificate, password, website);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> installPFXAsync(byte[] certificate, string password, SolidCP.Providers.Web.WebSite website)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "installPFX", certificate, password, website);
        }

        public byte[] exportCertificate(string serialNumber, string password)
        {
            return Invoke<byte[]>("SolidCP.Server.WebServer", "exportCertificate", serialNumber, password);
        }

        public async System.Threading.Tasks.Task<byte[]> exportCertificateAsync(string serialNumber, string password)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.WebServer", "exportCertificate", serialNumber, password);
        }

        public SolidCP.Providers.Web.SSLCertificate[] /*List*/ getServerCertificates()
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate[], SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "getServerCertificates");
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate[]> getServerCertificatesAsync()
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate[], SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "getServerCertificates");
        }

        public SolidCP.Providers.Common.ResultObject DeleteCertificate(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.WebServer", "DeleteCertificate", certificate, website);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCertificateAsync(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.WebServer", "DeleteCertificate", certificate, website);
        }

        public SolidCP.Providers.Web.SSLCertificate ImportCertificate(SolidCP.Providers.Web.WebSite website)
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "ImportCertificate", website);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> ImportCertificateAsync(SolidCP.Providers.Web.WebSite website)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate>("SolidCP.Server.WebServer", "ImportCertificate", website);
        }

        public bool CheckCertificate(SolidCP.Providers.Web.WebSite webSite)
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "CheckCertificate", webSite);
        }

        public async System.Threading.Tasks.Task<bool> CheckCertificateAsync(SolidCP.Providers.Web.WebSite webSite)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "CheckCertificate", webSite);
        }

        public bool GetDirectoryBrowseEnabled(string siteId)
        {
            return Invoke<bool>("SolidCP.Server.WebServer", "GetDirectoryBrowseEnabled", siteId);
        }

        public async System.Threading.Tasks.Task<bool> GetDirectoryBrowseEnabledAsync(string siteId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "GetDirectoryBrowseEnabled", siteId);
        }

        public void SetDirectoryBrowseEnabled(string siteId, bool enabled)
        {
            Invoke("SolidCP.Server.WebServer", "SetDirectoryBrowseEnabled", siteId, enabled);
        }

        public async System.Threading.Tasks.Task SetDirectoryBrowseEnabledAsync(string siteId, bool enabled)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "SetDirectoryBrowseEnabled", siteId, enabled);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class WebServer : SolidCP.Web.Client.ClientBase<IWebServer, WebServerAssemblyClient>, IWebServer
    {
        public void ChangeSiteState(string siteId, SolidCP.Providers.ServerState state)
        {
            base.Client.ChangeSiteState(siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, SolidCP.Providers.ServerState state)
        {
            await base.Client.ChangeSiteStateAsync(siteId, state);
        }

        public SolidCP.Providers.ServerState GetSiteState(string siteId)
        {
            return base.Client.GetSiteState(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ServerState> GetSiteStateAsync(string siteId)
        {
            return await base.Client.GetSiteStateAsync(siteId);
        }

        public string GetSiteId(string siteName)
        {
            return base.Client.GetSiteId(siteName);
        }

        public async System.Threading.Tasks.Task<string> GetSiteIdAsync(string siteName)
        {
            return await base.Client.GetSiteIdAsync(siteName);
        }

        public string[] GetSitesAccounts(string[] siteIds)
        {
            return base.Client.GetSitesAccounts(siteIds);
        }

        public async System.Threading.Tasks.Task<string[]> GetSitesAccountsAsync(string[] siteIds)
        {
            return await base.Client.GetSitesAccountsAsync(siteIds);
        }

        public bool SiteExists(string siteId)
        {
            return base.Client.SiteExists(siteId);
        }

        public async System.Threading.Tasks.Task<bool> SiteExistsAsync(string siteId)
        {
            return await base.Client.SiteExistsAsync(siteId);
        }

        public string[] GetSites()
        {
            return base.Client.GetSites();
        }

        public async System.Threading.Tasks.Task<string[]> GetSitesAsync()
        {
            return await base.Client.GetSitesAsync();
        }

        public SolidCP.Providers.Web.WebSite GetSite(string siteId)
        {
            return base.Client.GetSite(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebSite> GetSiteAsync(string siteId)
        {
            return await base.Client.GetSiteAsync(siteId);
        }

        public SolidCP.Providers.ServerBinding[] GetSiteBindings(string siteId)
        {
            return base.Client.GetSiteBindings(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ServerBinding[]> GetSiteBindingsAsync(string siteId)
        {
            return await base.Client.GetSiteBindingsAsync(siteId);
        }

        public string CreateSite(SolidCP.Providers.Web.WebSite site)
        {
            return base.Client.CreateSite(site);
        }

        public async System.Threading.Tasks.Task<string> CreateSiteAsync(SolidCP.Providers.Web.WebSite site)
        {
            return await base.Client.CreateSiteAsync(site);
        }

        public void UpdateSite(SolidCP.Providers.Web.WebSite site)
        {
            base.Client.UpdateSite(site);
        }

        public async System.Threading.Tasks.Task UpdateSiteAsync(SolidCP.Providers.Web.WebSite site)
        {
            await base.Client.UpdateSiteAsync(site);
        }

        public void UpdateSiteBindings(string siteId, SolidCP.Providers.ServerBinding[] bindings, bool emptyBindingsAllowed)
        {
            base.Client.UpdateSiteBindings(siteId, bindings, emptyBindingsAllowed);
        }

        public async System.Threading.Tasks.Task UpdateSiteBindingsAsync(string siteId, SolidCP.Providers.ServerBinding[] bindings, bool emptyBindingsAllowed)
        {
            await base.Client.UpdateSiteBindingsAsync(siteId, bindings, emptyBindingsAllowed);
        }

        public void DeleteSite(string siteId)
        {
            base.Client.DeleteSite(siteId);
        }

        public async System.Threading.Tasks.Task DeleteSiteAsync(string siteId)
        {
            await base.Client.DeleteSiteAsync(siteId);
        }

        public void ChangeAppPoolState(string siteId, SolidCP.Providers.AppPoolState state)
        {
            base.Client.ChangeAppPoolState(siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeAppPoolStateAsync(string siteId, SolidCP.Providers.AppPoolState state)
        {
            await base.Client.ChangeAppPoolStateAsync(siteId, state);
        }

        public SolidCP.Providers.AppPoolState GetAppPoolState(string siteId)
        {
            return base.Client.GetAppPoolState(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.AppPoolState> GetAppPoolStateAsync(string siteId)
        {
            return await base.Client.GetAppPoolStateAsync(siteId);
        }

        public bool VirtualDirectoryExists(string siteId, string directoryName)
        {
            return base.Client.VirtualDirectoryExists(siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<bool> VirtualDirectoryExistsAsync(string siteId, string directoryName)
        {
            return await base.Client.VirtualDirectoryExistsAsync(siteId, directoryName);
        }

        public SolidCP.Providers.Web.WebVirtualDirectory[] GetVirtualDirectories(string siteId)
        {
            return base.Client.GetVirtualDirectories(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory[]> GetVirtualDirectoriesAsync(string siteId)
        {
            return await base.Client.GetVirtualDirectoriesAsync(siteId);
        }

        public SolidCP.Providers.Web.WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
        {
            return base.Client.GetVirtualDirectory(siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory> GetVirtualDirectoryAsync(string siteId, string directoryName)
        {
            return await base.Client.GetVirtualDirectoryAsync(siteId, directoryName);
        }

        public void CreateVirtualDirectory(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory)
        {
            base.Client.CreateVirtualDirectory(siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory)
        {
            await base.Client.CreateVirtualDirectoryAsync(siteId, directory);
        }

        public void UpdateVirtualDirectory(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory)
        {
            base.Client.UpdateVirtualDirectory(siteId, directory);
        }

        public async System.Threading.Tasks.Task UpdateVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebVirtualDirectory directory)
        {
            await base.Client.UpdateVirtualDirectoryAsync(siteId, directory);
        }

        public void DeleteVirtualDirectory(string siteId, string directoryName)
        {
            base.Client.DeleteVirtualDirectory(siteId, directoryName);
        }

        public async System.Threading.Tasks.Task DeleteVirtualDirectoryAsync(string siteId, string directoryName)
        {
            await base.Client.DeleteVirtualDirectoryAsync(siteId, directoryName);
        }

        public bool AppVirtualDirectoryExists(string siteId, string directoryName)
        {
            return base.Client.AppVirtualDirectoryExists(siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<bool> AppVirtualDirectoryExistsAsync(string siteId, string directoryName)
        {
            return await base.Client.AppVirtualDirectoryExistsAsync(siteId, directoryName);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId)
        {
            return base.Client.GetAppVirtualDirectories(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetAppVirtualDirectoriesAsync(string siteId)
        {
            return await base.Client.GetAppVirtualDirectoriesAsync(siteId);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName)
        {
            return base.Client.GetAppVirtualDirectory(siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory> GetAppVirtualDirectoryAsync(string siteId, string directoryName)
        {
            return await base.Client.GetAppVirtualDirectoryAsync(siteId, directoryName);
        }

        public void CreateAppVirtualDirectory(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            base.Client.CreateAppVirtualDirectory(siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateAppVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            await base.Client.CreateAppVirtualDirectoryAsync(siteId, directory);
        }

        public void CreateEnterpriseStorageAppVirtualDirectory(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            base.Client.CreateEnterpriseStorageAppVirtualDirectory(siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateEnterpriseStorageAppVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            await base.Client.CreateEnterpriseStorageAppVirtualDirectoryAsync(siteId, directory);
        }

        public void UpdateAppVirtualDirectory(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            base.Client.UpdateAppVirtualDirectory(siteId, directory);
        }

        public async System.Threading.Tasks.Task UpdateAppVirtualDirectoryAsync(string siteId, SolidCP.Providers.Web.WebAppVirtualDirectory directory)
        {
            await base.Client.UpdateAppVirtualDirectoryAsync(siteId, directory);
        }

        public void DeleteAppVirtualDirectory(string siteId, string directoryName)
        {
            base.Client.DeleteAppVirtualDirectory(siteId, directoryName);
        }

        public async System.Threading.Tasks.Task DeleteAppVirtualDirectoryAsync(string siteId, string directoryName)
        {
            await base.Client.DeleteAppVirtualDirectoryAsync(siteId, directoryName);
        }

        public bool IsFrontPageSystemInstalled()
        {
            return base.Client.IsFrontPageSystemInstalled();
        }

        public async System.Threading.Tasks.Task<bool> IsFrontPageSystemInstalledAsync()
        {
            return await base.Client.IsFrontPageSystemInstalledAsync();
        }

        public bool IsFrontPageInstalled(string siteId)
        {
            return base.Client.IsFrontPageInstalled(siteId);
        }

        public async System.Threading.Tasks.Task<bool> IsFrontPageInstalledAsync(string siteId)
        {
            return await base.Client.IsFrontPageInstalledAsync(siteId);
        }

        public bool InstallFrontPage(string siteId, string username, string password)
        {
            return base.Client.InstallFrontPage(siteId, username, password);
        }

        public async System.Threading.Tasks.Task<bool> InstallFrontPageAsync(string siteId, string username, string password)
        {
            return await base.Client.InstallFrontPageAsync(siteId, username, password);
        }

        public void UninstallFrontPage(string siteId, string username)
        {
            base.Client.UninstallFrontPage(siteId, username);
        }

        public async System.Threading.Tasks.Task UninstallFrontPageAsync(string siteId, string username)
        {
            await base.Client.UninstallFrontPageAsync(siteId, username);
        }

        public void ChangeFrontPagePassword(string username, string password)
        {
            base.Client.ChangeFrontPagePassword(username, password);
        }

        public async System.Threading.Tasks.Task ChangeFrontPagePasswordAsync(string username, string password)
        {
            await base.Client.ChangeFrontPagePasswordAsync(username, password);
        }

        public bool IsColdFusionSystemInstalled()
        {
            return base.Client.IsColdFusionSystemInstalled();
        }

        public async System.Threading.Tasks.Task<bool> IsColdFusionSystemInstalledAsync()
        {
            return await base.Client.IsColdFusionSystemInstalledAsync();
        }

        public void GrantWebSiteAccess(string path, string siteId, SolidCP.Providers.NTFSPermission permission)
        {
            base.Client.GrantWebSiteAccess(path, siteId, permission);
        }

        public async System.Threading.Tasks.Task GrantWebSiteAccessAsync(string path, string siteId, SolidCP.Providers.NTFSPermission permission)
        {
            await base.Client.GrantWebSiteAccessAsync(path, siteId, permission);
        }

        public void InstallSecuredFolders(string siteId)
        {
            base.Client.InstallSecuredFolders(siteId);
        }

        public async System.Threading.Tasks.Task InstallSecuredFoldersAsync(string siteId)
        {
            await base.Client.InstallSecuredFoldersAsync(siteId);
        }

        public void UninstallSecuredFolders(string siteId)
        {
            base.Client.UninstallSecuredFolders(siteId);
        }

        public async System.Threading.Tasks.Task UninstallSecuredFoldersAsync(string siteId)
        {
            await base.Client.UninstallSecuredFoldersAsync(siteId);
        }

        public SolidCP.Providers.Web.WebFolder[] /*List*/ GetFolders(string siteId)
        {
            return base.Client.GetFolders(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder[]> GetFoldersAsync(string siteId)
        {
            return await base.Client.GetFoldersAsync(siteId);
        }

        public SolidCP.Providers.Web.WebFolder GetFolder(string siteId, string folderPath)
        {
            return base.Client.GetFolder(siteId, folderPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder> GetFolderAsync(string siteId, string folderPath)
        {
            return await base.Client.GetFolderAsync(siteId, folderPath);
        }

        public void UpdateFolder(string siteId, SolidCP.Providers.Web.WebFolder folder)
        {
            base.Client.UpdateFolder(siteId, folder);
        }

        public async System.Threading.Tasks.Task UpdateFolderAsync(string siteId, SolidCP.Providers.Web.WebFolder folder)
        {
            await base.Client.UpdateFolderAsync(siteId, folder);
        }

        public void DeleteFolder(string siteId, string folderPath)
        {
            base.Client.DeleteFolder(siteId, folderPath);
        }

        public async System.Threading.Tasks.Task DeleteFolderAsync(string siteId, string folderPath)
        {
            await base.Client.DeleteFolderAsync(siteId, folderPath);
        }

        public SolidCP.Providers.Web.WebUser[] /*List*/ GetUsers(string siteId)
        {
            return base.Client.GetUsers(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser[]> GetUsersAsync(string siteId)
        {
            return await base.Client.GetUsersAsync(siteId);
        }

        public SolidCP.Providers.Web.WebUser GetUser(string siteId, string userName)
        {
            return base.Client.GetUser(siteId, userName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser> GetUserAsync(string siteId, string userName)
        {
            return await base.Client.GetUserAsync(siteId, userName);
        }

        public void UpdateUser(string siteId, SolidCP.Providers.Web.WebUser user)
        {
            base.Client.UpdateUser(siteId, user);
        }

        public async System.Threading.Tasks.Task UpdateUserAsync(string siteId, SolidCP.Providers.Web.WebUser user)
        {
            await base.Client.UpdateUserAsync(siteId, user);
        }

        public void DeleteUser(string siteId, string userName)
        {
            base.Client.DeleteUser(siteId, userName);
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(string siteId, string userName)
        {
            await base.Client.DeleteUserAsync(siteId, userName);
        }

        public SolidCP.Providers.Web.WebGroup[] /*List*/ GetGroups(string siteId)
        {
            return base.Client.GetGroups(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetGroupsAsync(string siteId)
        {
            return await base.Client.GetGroupsAsync(siteId);
        }

        public SolidCP.Providers.Web.WebGroup GetGroup(string siteId, string groupName)
        {
            return base.Client.GetGroup(siteId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetGroupAsync(string siteId, string groupName)
        {
            return await base.Client.GetGroupAsync(siteId, groupName);
        }

        public void UpdateGroup(string siteId, SolidCP.Providers.Web.WebGroup group)
        {
            base.Client.UpdateGroup(siteId, group);
        }

        public async System.Threading.Tasks.Task UpdateGroupAsync(string siteId, SolidCP.Providers.Web.WebGroup group)
        {
            await base.Client.UpdateGroupAsync(siteId, group);
        }

        public void DeleteGroup(string siteId, string groupName)
        {
            base.Client.DeleteGroup(siteId, groupName);
        }

        public async System.Threading.Tasks.Task DeleteGroupAsync(string siteId, string groupName)
        {
            await base.Client.DeleteGroupAsync(siteId, groupName);
        }

        public SolidCP.Providers.ResultObjects.HeliconApeStatus GetHeliconApeStatus(string siteId)
        {
            return base.Client.GetHeliconApeStatus(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.HeliconApeStatus> GetHeliconApeStatusAsync(string siteId)
        {
            return await base.Client.GetHeliconApeStatusAsync(siteId);
        }

        public void InstallHeliconApe(string ServiceId)
        {
            base.Client.InstallHeliconApe(ServiceId);
        }

        public async System.Threading.Tasks.Task InstallHeliconApeAsync(string ServiceId)
        {
            await base.Client.InstallHeliconApeAsync(ServiceId);
        }

        public void EnableHeliconApe(string siteId)
        {
            base.Client.EnableHeliconApe(siteId);
        }

        public async System.Threading.Tasks.Task EnableHeliconApeAsync(string siteId)
        {
            await base.Client.EnableHeliconApeAsync(siteId);
        }

        public void DisableHeliconApe(string siteId)
        {
            base.Client.DisableHeliconApe(siteId);
        }

        public async System.Threading.Tasks.Task DisableHeliconApeAsync(string siteId)
        {
            await base.Client.DisableHeliconApeAsync(siteId);
        }

        public SolidCP.Providers.Web.HtaccessFolder[] /*List*/ GetHeliconApeFolders(string siteId)
        {
            return base.Client.GetHeliconApeFolders(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder[]> GetHeliconApeFoldersAsync(string siteId)
        {
            return await base.Client.GetHeliconApeFoldersAsync(siteId);
        }

        public SolidCP.Providers.Web.HtaccessFolder GetHeliconApeHttpdFolder()
        {
            return base.Client.GetHeliconApeHttpdFolder();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeHttpdFolderAsync()
        {
            return await base.Client.GetHeliconApeHttpdFolderAsync();
        }

        public SolidCP.Providers.Web.HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath)
        {
            return base.Client.GetHeliconApeFolder(siteId, folderPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeFolderAsync(string siteId, string folderPath)
        {
            return await base.Client.GetHeliconApeFolderAsync(siteId, folderPath);
        }

        public void UpdateHeliconApeFolder(string siteId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            base.Client.UpdateHeliconApeFolder(siteId, folder);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeFolderAsync(string siteId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            await base.Client.UpdateHeliconApeFolderAsync(siteId, folder);
        }

        public void UpdateHeliconApeHttpdFolder(SolidCP.Providers.Web.HtaccessFolder folder)
        {
            base.Client.UpdateHeliconApeHttpdFolder(folder);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeHttpdFolderAsync(SolidCP.Providers.Web.HtaccessFolder folder)
        {
            await base.Client.UpdateHeliconApeHttpdFolderAsync(folder);
        }

        public void DeleteHeliconApeFolder(string siteId, string folderPath)
        {
            base.Client.DeleteHeliconApeFolder(siteId, folderPath);
        }

        public async System.Threading.Tasks.Task DeleteHeliconApeFolderAsync(string siteId, string folderPath)
        {
            await base.Client.DeleteHeliconApeFolderAsync(siteId, folderPath);
        }

        public SolidCP.Providers.Web.HtaccessUser[] /*List*/ GetHeliconApeUsers(string siteId)
        {
            return base.Client.GetHeliconApeUsers(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser[]> GetHeliconApeUsersAsync(string siteId)
        {
            return await base.Client.GetHeliconApeUsersAsync(siteId);
        }

        public SolidCP.Providers.Web.HtaccessUser GetHeliconApeUser(string siteId, string userName)
        {
            return base.Client.GetHeliconApeUser(siteId, userName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser> GetHeliconApeUserAsync(string siteId, string userName)
        {
            return await base.Client.GetHeliconApeUserAsync(siteId, userName);
        }

        public void UpdateHeliconApeUser(string siteId, SolidCP.Providers.Web.HtaccessUser user)
        {
            base.Client.UpdateHeliconApeUser(siteId, user);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeUserAsync(string siteId, SolidCP.Providers.Web.HtaccessUser user)
        {
            await base.Client.UpdateHeliconApeUserAsync(siteId, user);
        }

        public void DeleteHeliconApeUser(string siteId, string userName)
        {
            base.Client.DeleteHeliconApeUser(siteId, userName);
        }

        public async System.Threading.Tasks.Task DeleteHeliconApeUserAsync(string siteId, string userName)
        {
            await base.Client.DeleteHeliconApeUserAsync(siteId, userName);
        }

        public SolidCP.Providers.Web.WebGroup[] /*List*/ GetHeliconApeGroups(string siteId)
        {
            return base.Client.GetHeliconApeGroups(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetHeliconApeGroupsAsync(string siteId)
        {
            return await base.Client.GetHeliconApeGroupsAsync(siteId);
        }

        public SolidCP.Providers.Web.WebGroup GetHeliconApeGroup(string siteId, string groupName)
        {
            return base.Client.GetHeliconApeGroup(siteId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetHeliconApeGroupAsync(string siteId, string groupName)
        {
            return await base.Client.GetHeliconApeGroupAsync(siteId, groupName);
        }

        public void UpdateHeliconApeGroup(string siteId, SolidCP.Providers.Web.WebGroup group)
        {
            base.Client.UpdateHeliconApeGroup(siteId, group);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeGroupAsync(string siteId, SolidCP.Providers.Web.WebGroup group)
        {
            await base.Client.UpdateHeliconApeGroupAsync(siteId, group);
        }

        public void GrantWebDeployPublishingAccess(string siteId, string accountName, string accountPassword)
        {
            base.Client.GrantWebDeployPublishingAccess(siteId, accountName, accountPassword);
        }

        public async System.Threading.Tasks.Task GrantWebDeployPublishingAccessAsync(string siteId, string accountName, string accountPassword)
        {
            await base.Client.GrantWebDeployPublishingAccessAsync(siteId, accountName, accountPassword);
        }

        public void RevokeWebDeployPublishingAccess(string siteId, string accountName)
        {
            base.Client.RevokeWebDeployPublishingAccess(siteId, accountName);
        }

        public async System.Threading.Tasks.Task RevokeWebDeployPublishingAccessAsync(string siteId, string accountName)
        {
            await base.Client.RevokeWebDeployPublishingAccessAsync(siteId, accountName);
        }

        public void DeleteHeliconApeGroup(string siteId, string groupName)
        {
            base.Client.DeleteHeliconApeGroup(siteId, groupName);
        }

        public async System.Threading.Tasks.Task DeleteHeliconApeGroupAsync(string siteId, string groupName)
        {
            await base.Client.DeleteHeliconApeGroupAsync(siteId, groupName);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory[] GetZooApplications(string siteId)
        {
            return base.Client.GetZooApplications(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetZooApplicationsAsync(string siteId)
        {
            return await base.Client.GetZooApplicationsAsync(siteId);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue)
        {
            return base.Client.SetZooEnvironmentVariable(siteId, appName, envName, envValue);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooEnvironmentVariableAsync(string siteId, string appName, string envName, string envValue)
        {
            return await base.Client.SetZooEnvironmentVariableAsync(siteId, appName, envName, envValue);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleEnabled(string siteId, string appName)
        {
            return base.Client.SetZooConsoleEnabled(siteId, appName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleEnabledAsync(string siteId, string appName)
        {
            return await base.Client.SetZooConsoleEnabledAsync(siteId, appName);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleDisabled(string siteId, string appName)
        {
            return base.Client.SetZooConsoleDisabled(siteId, appName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleDisabledAsync(string siteId, string appName)
        {
            return await base.Client.SetZooConsoleDisabledAsync(siteId, appName);
        }

        public bool CheckLoadUserProfile()
        {
            return base.Client.CheckLoadUserProfile();
        }

        public async System.Threading.Tasks.Task<bool> CheckLoadUserProfileAsync()
        {
            return await base.Client.CheckLoadUserProfileAsync();
        }

        public void EnableLoadUserProfile()
        {
            base.Client.EnableLoadUserProfile();
        }

        public async System.Threading.Tasks.Task EnableLoadUserProfileAsync()
        {
            await base.Client.EnableLoadUserProfileAsync();
        }

        public void InitFeeds(int UserId, string[] feeds)
        {
            base.Client.InitFeeds(UserId, feeds);
        }

        public async System.Threading.Tasks.Task InitFeedsAsync(int UserId, string[] feeds)
        {
            await base.Client.InitFeedsAsync(UserId, feeds);
        }

        public void SetResourceLanguage(int UserId, string resourceLanguage)
        {
            base.Client.SetResourceLanguage(UserId, resourceLanguage);
        }

        public async System.Threading.Tasks.Task SetResourceLanguageAsync(int UserId, string resourceLanguage)
        {
            await base.Client.SetResourceLanguageAsync(UserId, resourceLanguage);
        }

        public SolidCP.Providers.ResultObjects.GalleryLanguagesResult GetGalleryLanguages(int UserId)
        {
            return base.Client.GetGalleryLanguages(UserId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryLanguagesResult> GetGalleryLanguagesAsync(int UserId)
        {
            return await base.Client.GetGalleryLanguagesAsync(UserId);
        }

        public SolidCP.Providers.ResultObjects.GalleryCategoriesResult GetGalleryCategories(int UserId)
        {
            return base.Client.GetGalleryCategories(UserId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryCategoriesResult> GetGalleryCategoriesAsync(int UserId)
        {
            return await base.Client.GetGalleryCategoriesAsync(UserId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId)
        {
            return base.Client.GetGalleryApplications(UserId, categoryId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsAsync(int UserId, string categoryId)
        {
            return await base.Client.GetGalleryApplicationsAsync(UserId, categoryId);
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern)
        {
            return base.Client.GetGalleryApplicationsFiltered(UserId, pattern);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationsResult> GetGalleryApplicationsFilteredAsync(int UserId, string pattern)
        {
            return await base.Client.GetGalleryApplicationsFilteredAsync(UserId, pattern);
        }

        public bool IsMsDeployInstalled()
        {
            return base.Client.IsMsDeployInstalled();
        }

        public async System.Threading.Tasks.Task<bool> IsMsDeployInstalledAsync()
        {
            return await base.Client.IsMsDeployInstalledAsync();
        }

        public SolidCP.Providers.ResultObjects.GalleryApplicationResult GetGalleryApplication(int UserId, string id)
        {
            return base.Client.GetGalleryApplication(UserId, id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.GalleryApplicationResult> GetGalleryApplicationAsync(int UserId, string id)
        {
            return await base.Client.GetGalleryApplicationAsync(UserId, id);
        }

        public SolidCP.Providers.WebAppGallery.GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id)
        {
            return base.Client.GetGalleryApplicationStatus(UserId, id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> GetGalleryApplicationStatusAsync(int UserId, string id)
        {
            return await base.Client.GetGalleryApplicationStatusAsync(UserId, id);
        }

        public SolidCP.Providers.WebAppGallery.GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id)
        {
            return base.Client.DownloadGalleryApplication(UserId, id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.WebAppGallery.GalleryWebAppStatus> DownloadGalleryApplicationAsync(int UserId, string id)
        {
            return await base.Client.DownloadGalleryApplicationAsync(UserId, id);
        }

        public SolidCP.Providers.ResultObjects.DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id)
        {
            return base.Client.GetGalleryApplicationParameters(UserId, id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.DeploymentParametersResult> GetGalleryApplicationParametersAsync(int UserId, string id)
        {
            return await base.Client.GetGalleryApplicationParametersAsync(UserId, id);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject InstallGalleryApplication(int UserId, string id, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ updatedValues, string languageId)
        {
            return base.Client.InstallGalleryApplication(UserId, id, updatedValues, languageId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> InstallGalleryApplicationAsync(int UserId, string id, SolidCP.Providers.WebAppGallery.DeploymentParameter[] /*List*/ updatedValues, string languageId)
        {
            return await base.Client.InstallGalleryApplicationAsync(UserId, id, updatedValues, languageId);
        }

        public bool CheckWebManagementAccountExists(string accountName)
        {
            return base.Client.CheckWebManagementAccountExists(accountName);
        }

        public async System.Threading.Tasks.Task<bool> CheckWebManagementAccountExistsAsync(string accountName)
        {
            return await base.Client.CheckWebManagementAccountExistsAsync(accountName);
        }

        public SolidCP.Providers.Common.ResultObject CheckWebManagementPasswordComplexity(string accountPassword)
        {
            return base.Client.CheckWebManagementPasswordComplexity(accountPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CheckWebManagementPasswordComplexityAsync(string accountPassword)
        {
            return await base.Client.CheckWebManagementPasswordComplexityAsync(accountPassword);
        }

        public void GrantWebManagementAccess(string siteId, string accountName, string accountPassword)
        {
            base.Client.GrantWebManagementAccess(siteId, accountName, accountPassword);
        }

        public async System.Threading.Tasks.Task GrantWebManagementAccessAsync(string siteId, string accountName, string accountPassword)
        {
            await base.Client.GrantWebManagementAccessAsync(siteId, accountName, accountPassword);
        }

        public void RevokeWebManagementAccess(string siteId, string accountName)
        {
            base.Client.RevokeWebManagementAccess(siteId, accountName);
        }

        public async System.Threading.Tasks.Task RevokeWebManagementAccessAsync(string siteId, string accountName)
        {
            await base.Client.RevokeWebManagementAccessAsync(siteId, accountName);
        }

        public void ChangeWebManagementAccessPassword(string accountName, string accountPassword)
        {
            base.Client.ChangeWebManagementAccessPassword(accountName, accountPassword);
        }

        public async System.Threading.Tasks.Task ChangeWebManagementAccessPasswordAsync(string accountName, string accountPassword)
        {
            await base.Client.ChangeWebManagementAccessPasswordAsync(accountName, accountPassword);
        }

        public SolidCP.Providers.Web.SSLCertificate generateCSR(SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return base.Client.generateCSR(certificate);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> generateCSRAsync(SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return await base.Client.generateCSRAsync(certificate);
        }

        public SolidCP.Providers.Web.SSLCertificate generateRenewalCSR(SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return base.Client.generateRenewalCSR(certificate);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> generateRenewalCSRAsync(SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return await base.Client.generateRenewalCSRAsync(certificate);
        }

        public SolidCP.Providers.Web.SSLCertificate getCertificate(SolidCP.Providers.Web.WebSite site)
        {
            return base.Client.getCertificate(site);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> getCertificateAsync(SolidCP.Providers.Web.WebSite site)
        {
            return await base.Client.getCertificateAsync(site);
        }

        public SolidCP.Providers.Web.SSLCertificate installCertificate(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website)
        {
            return base.Client.installCertificate(certificate, website);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> installCertificateAsync(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website)
        {
            return await base.Client.installCertificateAsync(certificate, website);
        }

        public System.String LEinstallCertificate(SolidCP.Providers.Web.WebSite website, string email)
        {
            return base.Client.LEinstallCertificate(website, email);
        }

        public async System.Threading.Tasks.Task<System.String> LEinstallCertificateAsync(SolidCP.Providers.Web.WebSite website, string email)
        {
            return await base.Client.LEinstallCertificateAsync(website, email);
        }

        public SolidCP.Providers.Web.SSLCertificate installPFX(byte[] certificate, string password, SolidCP.Providers.Web.WebSite website)
        {
            return base.Client.installPFX(certificate, password, website);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> installPFXAsync(byte[] certificate, string password, SolidCP.Providers.Web.WebSite website)
        {
            return await base.Client.installPFXAsync(certificate, password, website);
        }

        public byte[] exportCertificate(string serialNumber, string password)
        {
            return base.Client.exportCertificate(serialNumber, password);
        }

        public async System.Threading.Tasks.Task<byte[]> exportCertificateAsync(string serialNumber, string password)
        {
            return await base.Client.exportCertificateAsync(serialNumber, password);
        }

        public SolidCP.Providers.Web.SSLCertificate[] /*List*/ getServerCertificates()
        {
            return base.Client.getServerCertificates();
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate[]> getServerCertificatesAsync()
        {
            return await base.Client.getServerCertificatesAsync();
        }

        public SolidCP.Providers.Common.ResultObject DeleteCertificate(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website)
        {
            return base.Client.DeleteCertificate(certificate, website);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCertificateAsync(SolidCP.Providers.Web.SSLCertificate certificate, SolidCP.Providers.Web.WebSite website)
        {
            return await base.Client.DeleteCertificateAsync(certificate, website);
        }

        public SolidCP.Providers.Web.SSLCertificate ImportCertificate(SolidCP.Providers.Web.WebSite website)
        {
            return base.Client.ImportCertificate(website);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> ImportCertificateAsync(SolidCP.Providers.Web.WebSite website)
        {
            return await base.Client.ImportCertificateAsync(website);
        }

        public bool CheckCertificate(SolidCP.Providers.Web.WebSite webSite)
        {
            return base.Client.CheckCertificate(webSite);
        }

        public async System.Threading.Tasks.Task<bool> CheckCertificateAsync(SolidCP.Providers.Web.WebSite webSite)
        {
            return await base.Client.CheckCertificateAsync(webSite);
        }

        public bool GetDirectoryBrowseEnabled(string siteId)
        {
            return base.Client.GetDirectoryBrowseEnabled(siteId);
        }

        public async System.Threading.Tasks.Task<bool> GetDirectoryBrowseEnabledAsync(string siteId)
        {
            return await base.Client.GetDirectoryBrowseEnabledAsync(siteId);
        }

        public void SetDirectoryBrowseEnabled(string siteId, bool enabled)
        {
            base.Client.SetDirectoryBrowseEnabled(siteId, enabled);
        }

        public async System.Threading.Tasks.Task SetDirectoryBrowseEnabledAsync(string siteId, bool enabled)
        {
            await base.Client.SetDirectoryBrowseEnabledAsync(siteId, enabled);
        }
    }
}
#endif