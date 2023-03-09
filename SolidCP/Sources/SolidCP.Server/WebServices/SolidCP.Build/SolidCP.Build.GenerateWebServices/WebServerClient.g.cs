#if Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Server.Utils;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Providers.Common;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IWebServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IWebServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeSiteState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeSiteStateResponse")]
        void ChangeSiteState(string siteId, ServerState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeSiteState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeSiteStateResponse")]
        System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, ServerState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSiteState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteStateResponse")]
        ServerState GetSiteState(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSiteState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteStateResponse")]
        System.Threading.Tasks.Task<ServerState> GetSiteStateAsync(string siteId);
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
        WebSite GetSite(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteResponse")]
        System.Threading.Tasks.Task<WebSite> GetSiteAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSiteBindings", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteBindingsResponse")]
        ServerBinding[] GetSiteBindings(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetSiteBindings", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetSiteBindingsResponse")]
        System.Threading.Tasks.Task<ServerBinding[]> GetSiteBindingsAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateSiteResponse")]
        string CreateSite(WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateSiteResponse")]
        System.Threading.Tasks.Task<string> CreateSiteAsync(WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteResponse")]
        void UpdateSite(WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteResponse")]
        System.Threading.Tasks.Task UpdateSiteAsync(WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteBindings", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteBindingsResponse")]
        void UpdateSiteBindings(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteBindings", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateSiteBindingsResponse")]
        System.Threading.Tasks.Task UpdateSiteBindingsAsync(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteSiteResponse")]
        void DeleteSite(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteSite", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteSiteResponse")]
        System.Threading.Tasks.Task DeleteSiteAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeAppPoolState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeAppPoolStateResponse")]
        void ChangeAppPoolState(string siteId, AppPoolState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ChangeAppPoolState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ChangeAppPoolStateResponse")]
        System.Threading.Tasks.Task ChangeAppPoolStateAsync(string siteId, AppPoolState state);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppPoolState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppPoolStateResponse")]
        AppPoolState GetAppPoolState(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppPoolState", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppPoolStateResponse")]
        System.Threading.Tasks.Task<AppPoolState> GetAppPoolStateAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/VirtualDirectoryExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/VirtualDirectoryExistsResponse")]
        bool VirtualDirectoryExists(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/VirtualDirectoryExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/VirtualDirectoryExistsResponse")]
        System.Threading.Tasks.Task<bool> VirtualDirectoryExistsAsync(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectoriesResponse")]
        WebVirtualDirectory[] GetVirtualDirectories(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectoriesResponse")]
        System.Threading.Tasks.Task<WebVirtualDirectory[]> GetVirtualDirectoriesAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectoryResponse")]
        WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<WebVirtualDirectory> GetVirtualDirectoryAsync(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateVirtualDirectoryResponse")]
        void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateVirtualDirectoryResponse")]
        System.Threading.Tasks.Task CreateVirtualDirectoryAsync(string siteId, WebVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateVirtualDirectoryResponse")]
        void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateVirtualDirectoryResponse")]
        System.Threading.Tasks.Task UpdateVirtualDirectoryAsync(string siteId, WebVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteVirtualDirectoryResponse")]
        void DeleteVirtualDirectory(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteVirtualDirectoryResponse")]
        System.Threading.Tasks.Task DeleteVirtualDirectoryAsync(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/AppVirtualDirectoryExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/AppVirtualDirectoryExistsResponse")]
        bool AppVirtualDirectoryExists(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/AppVirtualDirectoryExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/AppVirtualDirectoryExistsResponse")]
        System.Threading.Tasks.Task<bool> AppVirtualDirectoryExistsAsync(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectoriesResponse")]
        WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectoriesResponse")]
        System.Threading.Tasks.Task<WebAppVirtualDirectory[]> GetAppVirtualDirectoriesAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectoryResponse")]
        WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<WebAppVirtualDirectory> GetAppVirtualDirectoryAsync(string siteId, string directoryName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateAppVirtualDirectoryResponse")]
        void CreateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task CreateAppVirtualDirectoryAsync(string siteId, WebAppVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateEnterpriseStorageAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateEnterpriseStorageAppVirtualDirectoryResponse")]
        void CreateEnterpriseStorageAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CreateEnterpriseStorageAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CreateEnterpriseStorageAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task CreateEnterpriseStorageAppVirtualDirectoryAsync(string siteId, WebAppVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateAppVirtualDirectoryResponse")]
        void UpdateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task UpdateAppVirtualDirectoryAsync(string siteId, WebAppVirtualDirectory directory);
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
        void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GrantWebSiteAccess", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GrantWebSiteAccessResponse")]
        System.Threading.Tasks.Task GrantWebSiteAccessAsync(string path, string siteId, NTFSPermission permission);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallSecuredFoldersResponse")]
        void InstallSecuredFolders(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallSecuredFoldersResponse")]
        System.Threading.Tasks.Task InstallSecuredFoldersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UninstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UninstallSecuredFoldersResponse")]
        void UninstallSecuredFolders(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UninstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UninstallSecuredFoldersResponse")]
        System.Threading.Tasks.Task UninstallSecuredFoldersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetFoldersResponse")]
        List<WebFolder> GetFolders(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetFoldersResponse")]
        System.Threading.Tasks.Task<List<WebFolder>> GetFoldersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetFolderResponse")]
        WebFolder GetFolder(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetFolderResponse")]
        System.Threading.Tasks.Task<WebFolder> GetFolderAsync(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateFolderResponse")]
        void UpdateFolder(string siteId, WebFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateFolderResponse")]
        System.Threading.Tasks.Task UpdateFolderAsync(string siteId, WebFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteFolderResponse")]
        void DeleteFolder(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteFolderResponse")]
        System.Threading.Tasks.Task DeleteFolderAsync(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetUsers", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetUsersResponse")]
        List<WebUser> GetUsers(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetUsers", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetUsersResponse")]
        System.Threading.Tasks.Task<List<WebUser>> GetUsersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetUserResponse")]
        WebUser GetUser(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetUserResponse")]
        System.Threading.Tasks.Task<WebUser> GetUserAsync(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateUserResponse")]
        void UpdateUser(string siteId, WebUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateUserResponse")]
        System.Threading.Tasks.Task UpdateUserAsync(string siteId, WebUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteUserResponse")]
        void DeleteUser(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteUserResponse")]
        System.Threading.Tasks.Task DeleteUserAsync(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGroups", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGroupsResponse")]
        List<WebGroup> GetGroups(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGroups", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGroupsResponse")]
        System.Threading.Tasks.Task<List<WebGroup>> GetGroupsAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGroupResponse")]
        WebGroup GetGroup(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGroupResponse")]
        System.Threading.Tasks.Task<WebGroup> GetGroupAsync(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateGroupResponse")]
        void UpdateGroup(string siteId, WebGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateGroupResponse")]
        System.Threading.Tasks.Task UpdateGroupAsync(string siteId, WebGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteGroupResponse")]
        void DeleteGroup(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteGroupResponse")]
        System.Threading.Tasks.Task DeleteGroupAsync(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeStatus", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeStatusResponse")]
        HeliconApeStatus GetHeliconApeStatus(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeStatus", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeStatusResponse")]
        System.Threading.Tasks.Task<HeliconApeStatus> GetHeliconApeStatusAsync(string siteId);
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
        List<HtaccessFolder> GetHeliconApeFolders(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFolders", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFoldersResponse")]
        System.Threading.Tasks.Task<List<HtaccessFolder>> GetHeliconApeFoldersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeHttpdFolderResponse")]
        HtaccessFolder GetHeliconApeHttpdFolder();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeHttpdFolderResponse")]
        System.Threading.Tasks.Task<HtaccessFolder> GetHeliconApeHttpdFolderAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFolderResponse")]
        HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeFolderResponse")]
        System.Threading.Tasks.Task<HtaccessFolder> GetHeliconApeFolderAsync(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeFolderResponse")]
        void UpdateHeliconApeFolder(string siteId, HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeFolderResponse")]
        System.Threading.Tasks.Task UpdateHeliconApeFolderAsync(string siteId, HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeHttpdFolderResponse")]
        void UpdateHeliconApeHttpdFolder(HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeHttpdFolderResponse")]
        System.Threading.Tasks.Task UpdateHeliconApeHttpdFolderAsync(HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeFolderResponse")]
        void DeleteHeliconApeFolder(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeFolderResponse")]
        System.Threading.Tasks.Task DeleteHeliconApeFolderAsync(string siteId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUsers", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUsersResponse")]
        List<HtaccessUser> GetHeliconApeUsers(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUsers", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUsersResponse")]
        System.Threading.Tasks.Task<List<HtaccessUser>> GetHeliconApeUsersAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUserResponse")]
        HtaccessUser GetHeliconApeUser(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeUserResponse")]
        System.Threading.Tasks.Task<HtaccessUser> GetHeliconApeUserAsync(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeUserResponse")]
        void UpdateHeliconApeUser(string siteId, HtaccessUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeUserResponse")]
        System.Threading.Tasks.Task UpdateHeliconApeUserAsync(string siteId, HtaccessUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeUserResponse")]
        void DeleteHeliconApeUser(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteHeliconApeUserResponse")]
        System.Threading.Tasks.Task DeleteHeliconApeUserAsync(string siteId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroups", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroupsResponse")]
        List<WebGroup> GetHeliconApeGroups(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroups", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroupsResponse")]
        System.Threading.Tasks.Task<List<WebGroup>> GetHeliconApeGroupsAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroupResponse")]
        WebGroup GetHeliconApeGroup(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetHeliconApeGroupResponse")]
        System.Threading.Tasks.Task<WebGroup> GetHeliconApeGroupAsync(string siteId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeGroupResponse")]
        void UpdateHeliconApeGroup(string siteId, WebGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/UpdateHeliconApeGroupResponse")]
        System.Threading.Tasks.Task UpdateHeliconApeGroupAsync(string siteId, WebGroup group);
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
        WebAppVirtualDirectory[] GetZooApplications(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetZooApplications", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetZooApplicationsResponse")]
        System.Threading.Tasks.Task<WebAppVirtualDirectory[]> GetZooApplicationsAsync(string siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooEnvironmentVariable", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooEnvironmentVariableResponse")]
        StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooEnvironmentVariable", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooEnvironmentVariableResponse")]
        System.Threading.Tasks.Task<StringResultObject> SetZooEnvironmentVariableAsync(string siteId, string appName, string envName, string envValue);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleEnabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleEnabledResponse")]
        StringResultObject SetZooConsoleEnabled(string siteId, string appName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleEnabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleEnabledResponse")]
        System.Threading.Tasks.Task<StringResultObject> SetZooConsoleEnabledAsync(string siteId, string appName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleDisabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleDisabledResponse")]
        StringResultObject SetZooConsoleDisabled(string siteId, string appName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleDisabled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/SetZooConsoleDisabledResponse")]
        System.Threading.Tasks.Task<StringResultObject> SetZooConsoleDisabledAsync(string siteId, string appName);
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
        GalleryLanguagesResult GetGalleryLanguages(int UserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryLanguages", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryLanguagesResponse")]
        System.Threading.Tasks.Task<GalleryLanguagesResult> GetGalleryLanguagesAsync(int UserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryCategories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryCategoriesResponse")]
        GalleryCategoriesResult GetGalleryCategories(int UserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryCategories", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryCategoriesResponse")]
        System.Threading.Tasks.Task<GalleryCategoriesResult> GetGalleryCategoriesAsync(int UserId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplications", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsResponse")]
        GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplications", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsResponse")]
        System.Threading.Tasks.Task<GalleryApplicationsResult> GetGalleryApplicationsAsync(int UserId, string categoryId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsFiltered", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsFilteredResponse")]
        GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsFiltered", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationsFilteredResponse")]
        System.Threading.Tasks.Task<GalleryApplicationsResult> GetGalleryApplicationsFilteredAsync(int UserId, string pattern);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/IsMsDeployInstalled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/IsMsDeployInstalledResponse")]
        bool IsMsDeployInstalled();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/IsMsDeployInstalled", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/IsMsDeployInstalledResponse")]
        System.Threading.Tasks.Task<bool> IsMsDeployInstalledAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationResponse")]
        GalleryApplicationResult GetGalleryApplication(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationResponse")]
        System.Threading.Tasks.Task<GalleryApplicationResult> GetGalleryApplicationAsync(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationStatus", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationStatusResponse")]
        GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationStatus", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationStatusResponse")]
        System.Threading.Tasks.Task<GalleryWebAppStatus> GetGalleryApplicationStatusAsync(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DownloadGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DownloadGalleryApplicationResponse")]
        GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DownloadGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DownloadGalleryApplicationResponse")]
        System.Threading.Tasks.Task<GalleryWebAppStatus> DownloadGalleryApplicationAsync(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationParameters", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationParametersResponse")]
        DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationParameters", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/GetGalleryApplicationParametersResponse")]
        System.Threading.Tasks.Task<DeploymentParametersResult> GetGalleryApplicationParametersAsync(int UserId, string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallGalleryApplicationResponse")]
        StringResultObject InstallGalleryApplication(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/InstallGalleryApplication", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/InstallGalleryApplicationResponse")]
        System.Threading.Tasks.Task<StringResultObject> InstallGalleryApplicationAsync(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementAccountExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementAccountExistsResponse")]
        bool CheckWebManagementAccountExists(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementAccountExists", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementAccountExistsResponse")]
        System.Threading.Tasks.Task<bool> CheckWebManagementAccountExistsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementPasswordComplexity", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementPasswordComplexityResponse")]
        ResultObject CheckWebManagementPasswordComplexity(string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementPasswordComplexity", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckWebManagementPasswordComplexityResponse")]
        System.Threading.Tasks.Task<ResultObject> CheckWebManagementPasswordComplexityAsync(string accountPassword);
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
        SSLCertificate generateCSR(SSLCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/generateCSR", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/generateCSRResponse")]
        System.Threading.Tasks.Task<SSLCertificate> generateCSRAsync(SSLCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/generateRenewalCSR", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/generateRenewalCSRResponse")]
        SSLCertificate generateRenewalCSR(SSLCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/generateRenewalCSR", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/generateRenewalCSRResponse")]
        System.Threading.Tasks.Task<SSLCertificate> generateRenewalCSRAsync(SSLCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/getCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/getCertificateResponse")]
        SSLCertificate getCertificate(WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/getCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/getCertificateResponse")]
        System.Threading.Tasks.Task<SSLCertificate> getCertificateAsync(WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/installCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/installCertificateResponse")]
        SSLCertificate installCertificate(SSLCertificate certificate, WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/installCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/installCertificateResponse")]
        System.Threading.Tasks.Task<SSLCertificate> installCertificateAsync(SSLCertificate certificate, WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/LEinstallCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/LEinstallCertificateResponse")]
        String LEinstallCertificate(WebSite website, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/LEinstallCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/LEinstallCertificateResponse")]
        System.Threading.Tasks.Task<String> LEinstallCertificateAsync(WebSite website, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/installPFX", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/installPFXResponse")]
        SSLCertificate installPFX(byte[] certificate, string password, WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/installPFX", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/installPFXResponse")]
        System.Threading.Tasks.Task<SSLCertificate> installPFXAsync(byte[] certificate, string password, WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/exportCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/exportCertificateResponse")]
        byte[] exportCertificate(string serialNumber, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/exportCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/exportCertificateResponse")]
        System.Threading.Tasks.Task<byte[]> exportCertificateAsync(string serialNumber, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/getServerCertificates", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/getServerCertificatesResponse")]
        List<SSLCertificate> getServerCertificates();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/getServerCertificates", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/getServerCertificatesResponse")]
        System.Threading.Tasks.Task<List<SSLCertificate>> getServerCertificatesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteCertificateResponse")]
        ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/DeleteCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/DeleteCertificateResponse")]
        System.Threading.Tasks.Task<ResultObject> DeleteCertificateAsync(SSLCertificate certificate, WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ImportCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ImportCertificateResponse")]
        SSLCertificate ImportCertificate(WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/ImportCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/ImportCertificateResponse")]
        System.Threading.Tasks.Task<SSLCertificate> ImportCertificateAsync(WebSite website);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckCertificateResponse")]
        bool CheckCertificate(WebSite webSite);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IWebServer/CheckCertificate", ReplyAction = "http://smbsaas/solidcp/server/IWebServer/CheckCertificateResponse")]
        System.Threading.Tasks.Task<bool> CheckCertificateAsync(WebSite webSite);
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
        public void ChangeSiteState(string siteId, ServerState state)
        {
            Invoke("SolidCP.Server.WebServer", "ChangeSiteState", siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, ServerState state)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "ChangeSiteState", siteId, state);
        }

        public ServerState GetSiteState(string siteId)
        {
            return (ServerState)Invoke("SolidCP.Server.WebServer", "GetSiteState", siteId);
        }

        public async System.Threading.Tasks.Task<ServerState> GetSiteStateAsync(string siteId)
        {
            return await InvokeAsync<ServerState>("SolidCP.Server.WebServer", "GetSiteState", siteId);
        }

        public string GetSiteId(string siteName)
        {
            return (string)Invoke("SolidCP.Server.WebServer", "GetSiteId", siteName);
        }

        public async System.Threading.Tasks.Task<string> GetSiteIdAsync(string siteName)
        {
            return await InvokeAsync<string>("SolidCP.Server.WebServer", "GetSiteId", siteName);
        }

        public string[] GetSitesAccounts(string[] siteIds)
        {
            return (string[])Invoke("SolidCP.Server.WebServer", "GetSitesAccounts", siteIds);
        }

        public async System.Threading.Tasks.Task<string[]> GetSitesAccountsAsync(string[] siteIds)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.WebServer", "GetSitesAccounts", siteIds);
        }

        public bool SiteExists(string siteId)
        {
            return (bool)Invoke("SolidCP.Server.WebServer", "SiteExists", siteId);
        }

        public async System.Threading.Tasks.Task<bool> SiteExistsAsync(string siteId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "SiteExists", siteId);
        }

        public string[] GetSites()
        {
            return (string[])Invoke("SolidCP.Server.WebServer", "GetSites");
        }

        public async System.Threading.Tasks.Task<string[]> GetSitesAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.WebServer", "GetSites");
        }

        public WebSite GetSite(string siteId)
        {
            return (WebSite)Invoke("SolidCP.Server.WebServer", "GetSite", siteId);
        }

        public async System.Threading.Tasks.Task<WebSite> GetSiteAsync(string siteId)
        {
            return await InvokeAsync<WebSite>("SolidCP.Server.WebServer", "GetSite", siteId);
        }

        public ServerBinding[] GetSiteBindings(string siteId)
        {
            return (ServerBinding[])Invoke("SolidCP.Server.WebServer", "GetSiteBindings", siteId);
        }

        public async System.Threading.Tasks.Task<ServerBinding[]> GetSiteBindingsAsync(string siteId)
        {
            return await InvokeAsync<ServerBinding[]>("SolidCP.Server.WebServer", "GetSiteBindings", siteId);
        }

        public string CreateSite(WebSite site)
        {
            return (string)Invoke("SolidCP.Server.WebServer", "CreateSite", site);
        }

        public async System.Threading.Tasks.Task<string> CreateSiteAsync(WebSite site)
        {
            return await InvokeAsync<string>("SolidCP.Server.WebServer", "CreateSite", site);
        }

        public void UpdateSite(WebSite site)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateSite", site);
        }

        public async System.Threading.Tasks.Task UpdateSiteAsync(WebSite site)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateSite", site);
        }

        public void UpdateSiteBindings(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateSiteBindings", siteId, bindings, emptyBindingsAllowed);
        }

        public async System.Threading.Tasks.Task UpdateSiteBindingsAsync(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed)
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

        public void ChangeAppPoolState(string siteId, AppPoolState state)
        {
            Invoke("SolidCP.Server.WebServer", "ChangeAppPoolState", siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeAppPoolStateAsync(string siteId, AppPoolState state)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "ChangeAppPoolState", siteId, state);
        }

        public AppPoolState GetAppPoolState(string siteId)
        {
            return (AppPoolState)Invoke("SolidCP.Server.WebServer", "GetAppPoolState", siteId);
        }

        public async System.Threading.Tasks.Task<AppPoolState> GetAppPoolStateAsync(string siteId)
        {
            return await InvokeAsync<AppPoolState>("SolidCP.Server.WebServer", "GetAppPoolState", siteId);
        }

        public bool VirtualDirectoryExists(string siteId, string directoryName)
        {
            return (bool)Invoke("SolidCP.Server.WebServer", "VirtualDirectoryExists", siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<bool> VirtualDirectoryExistsAsync(string siteId, string directoryName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "VirtualDirectoryExists", siteId, directoryName);
        }

        public WebVirtualDirectory[] GetVirtualDirectories(string siteId)
        {
            return (WebVirtualDirectory[])Invoke("SolidCP.Server.WebServer", "GetVirtualDirectories", siteId);
        }

        public async System.Threading.Tasks.Task<WebVirtualDirectory[]> GetVirtualDirectoriesAsync(string siteId)
        {
            return await InvokeAsync<WebVirtualDirectory[]>("SolidCP.Server.WebServer", "GetVirtualDirectories", siteId);
        }

        public WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
        {
            return (WebVirtualDirectory)Invoke("SolidCP.Server.WebServer", "GetVirtualDirectory", siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<WebVirtualDirectory> GetVirtualDirectoryAsync(string siteId, string directoryName)
        {
            return await InvokeAsync<WebVirtualDirectory>("SolidCP.Server.WebServer", "GetVirtualDirectory", siteId, directoryName);
        }

        public void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory)
        {
            Invoke("SolidCP.Server.WebServer", "CreateVirtualDirectory", siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateVirtualDirectoryAsync(string siteId, WebVirtualDirectory directory)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "CreateVirtualDirectory", siteId, directory);
        }

        public void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateVirtualDirectory", siteId, directory);
        }

        public async System.Threading.Tasks.Task UpdateVirtualDirectoryAsync(string siteId, WebVirtualDirectory directory)
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
            return (bool)Invoke("SolidCP.Server.WebServer", "AppVirtualDirectoryExists", siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<bool> AppVirtualDirectoryExistsAsync(string siteId, string directoryName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "AppVirtualDirectoryExists", siteId, directoryName);
        }

        public WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId)
        {
            return (WebAppVirtualDirectory[])Invoke("SolidCP.Server.WebServer", "GetAppVirtualDirectories", siteId);
        }

        public async System.Threading.Tasks.Task<WebAppVirtualDirectory[]> GetAppVirtualDirectoriesAsync(string siteId)
        {
            return await InvokeAsync<WebAppVirtualDirectory[]>("SolidCP.Server.WebServer", "GetAppVirtualDirectories", siteId);
        }

        public WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName)
        {
            return (WebAppVirtualDirectory)Invoke("SolidCP.Server.WebServer", "GetAppVirtualDirectory", siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<WebAppVirtualDirectory> GetAppVirtualDirectoryAsync(string siteId, string directoryName)
        {
            return await InvokeAsync<WebAppVirtualDirectory>("SolidCP.Server.WebServer", "GetAppVirtualDirectory", siteId, directoryName);
        }

        public void CreateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            Invoke("SolidCP.Server.WebServer", "CreateAppVirtualDirectory", siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateAppVirtualDirectoryAsync(string siteId, WebAppVirtualDirectory directory)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "CreateAppVirtualDirectory", siteId, directory);
        }

        public void CreateEnterpriseStorageAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            Invoke("SolidCP.Server.WebServer", "CreateEnterpriseStorageAppVirtualDirectory", siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateEnterpriseStorageAppVirtualDirectoryAsync(string siteId, WebAppVirtualDirectory directory)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "CreateEnterpriseStorageAppVirtualDirectory", siteId, directory);
        }

        public void UpdateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateAppVirtualDirectory", siteId, directory);
        }

        public async System.Threading.Tasks.Task UpdateAppVirtualDirectoryAsync(string siteId, WebAppVirtualDirectory directory)
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
            return (bool)Invoke("SolidCP.Server.WebServer", "IsFrontPageSystemInstalled");
        }

        public async System.Threading.Tasks.Task<bool> IsFrontPageSystemInstalledAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "IsFrontPageSystemInstalled");
        }

        public bool IsFrontPageInstalled(string siteId)
        {
            return (bool)Invoke("SolidCP.Server.WebServer", "IsFrontPageInstalled", siteId);
        }

        public async System.Threading.Tasks.Task<bool> IsFrontPageInstalledAsync(string siteId)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "IsFrontPageInstalled", siteId);
        }

        public bool InstallFrontPage(string siteId, string username, string password)
        {
            return (bool)Invoke("SolidCP.Server.WebServer", "InstallFrontPage", siteId, username, password);
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
            return (bool)Invoke("SolidCP.Server.WebServer", "IsColdFusionSystemInstalled");
        }

        public async System.Threading.Tasks.Task<bool> IsColdFusionSystemInstalledAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "IsColdFusionSystemInstalled");
        }

        public void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission)
        {
            Invoke("SolidCP.Server.WebServer", "GrantWebSiteAccess", path, siteId, permission);
        }

        public async System.Threading.Tasks.Task GrantWebSiteAccessAsync(string path, string siteId, NTFSPermission permission)
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

        public List<WebFolder> GetFolders(string siteId)
        {
            return (List<WebFolder>)Invoke("SolidCP.Server.WebServer", "GetFolders", siteId);
        }

        public async System.Threading.Tasks.Task<List<WebFolder>> GetFoldersAsync(string siteId)
        {
            return await InvokeAsync<List<WebFolder>>("SolidCP.Server.WebServer", "GetFolders", siteId);
        }

        public WebFolder GetFolder(string siteId, string folderPath)
        {
            return (WebFolder)Invoke("SolidCP.Server.WebServer", "GetFolder", siteId, folderPath);
        }

        public async System.Threading.Tasks.Task<WebFolder> GetFolderAsync(string siteId, string folderPath)
        {
            return await InvokeAsync<WebFolder>("SolidCP.Server.WebServer", "GetFolder", siteId, folderPath);
        }

        public void UpdateFolder(string siteId, WebFolder folder)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateFolder", siteId, folder);
        }

        public async System.Threading.Tasks.Task UpdateFolderAsync(string siteId, WebFolder folder)
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

        public List<WebUser> GetUsers(string siteId)
        {
            return (List<WebUser>)Invoke("SolidCP.Server.WebServer", "GetUsers", siteId);
        }

        public async System.Threading.Tasks.Task<List<WebUser>> GetUsersAsync(string siteId)
        {
            return await InvokeAsync<List<WebUser>>("SolidCP.Server.WebServer", "GetUsers", siteId);
        }

        public WebUser GetUser(string siteId, string userName)
        {
            return (WebUser)Invoke("SolidCP.Server.WebServer", "GetUser", siteId, userName);
        }

        public async System.Threading.Tasks.Task<WebUser> GetUserAsync(string siteId, string userName)
        {
            return await InvokeAsync<WebUser>("SolidCP.Server.WebServer", "GetUser", siteId, userName);
        }

        public void UpdateUser(string siteId, WebUser user)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateUser", siteId, user);
        }

        public async System.Threading.Tasks.Task UpdateUserAsync(string siteId, WebUser user)
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

        public List<WebGroup> GetGroups(string siteId)
        {
            return (List<WebGroup>)Invoke("SolidCP.Server.WebServer", "GetGroups", siteId);
        }

        public async System.Threading.Tasks.Task<List<WebGroup>> GetGroupsAsync(string siteId)
        {
            return await InvokeAsync<List<WebGroup>>("SolidCP.Server.WebServer", "GetGroups", siteId);
        }

        public WebGroup GetGroup(string siteId, string groupName)
        {
            return (WebGroup)Invoke("SolidCP.Server.WebServer", "GetGroup", siteId, groupName);
        }

        public async System.Threading.Tasks.Task<WebGroup> GetGroupAsync(string siteId, string groupName)
        {
            return await InvokeAsync<WebGroup>("SolidCP.Server.WebServer", "GetGroup", siteId, groupName);
        }

        public void UpdateGroup(string siteId, WebGroup group)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateGroup", siteId, group);
        }

        public async System.Threading.Tasks.Task UpdateGroupAsync(string siteId, WebGroup group)
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

        public HeliconApeStatus GetHeliconApeStatus(string siteId)
        {
            return (HeliconApeStatus)Invoke("SolidCP.Server.WebServer", "GetHeliconApeStatus", siteId);
        }

        public async System.Threading.Tasks.Task<HeliconApeStatus> GetHeliconApeStatusAsync(string siteId)
        {
            return await InvokeAsync<HeliconApeStatus>("SolidCP.Server.WebServer", "GetHeliconApeStatus", siteId);
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

        public List<HtaccessFolder> GetHeliconApeFolders(string siteId)
        {
            return (List<HtaccessFolder>)Invoke("SolidCP.Server.WebServer", "GetHeliconApeFolders", siteId);
        }

        public async System.Threading.Tasks.Task<List<HtaccessFolder>> GetHeliconApeFoldersAsync(string siteId)
        {
            return await InvokeAsync<List<HtaccessFolder>>("SolidCP.Server.WebServer", "GetHeliconApeFolders", siteId);
        }

        public HtaccessFolder GetHeliconApeHttpdFolder()
        {
            return (HtaccessFolder)Invoke("SolidCP.Server.WebServer", "GetHeliconApeHttpdFolder");
        }

        public async System.Threading.Tasks.Task<HtaccessFolder> GetHeliconApeHttpdFolderAsync()
        {
            return await InvokeAsync<HtaccessFolder>("SolidCP.Server.WebServer", "GetHeliconApeHttpdFolder");
        }

        public HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath)
        {
            return (HtaccessFolder)Invoke("SolidCP.Server.WebServer", "GetHeliconApeFolder", siteId, folderPath);
        }

        public async System.Threading.Tasks.Task<HtaccessFolder> GetHeliconApeFolderAsync(string siteId, string folderPath)
        {
            return await InvokeAsync<HtaccessFolder>("SolidCP.Server.WebServer", "GetHeliconApeFolder", siteId, folderPath);
        }

        public void UpdateHeliconApeFolder(string siteId, HtaccessFolder folder)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateHeliconApeFolder", siteId, folder);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeFolderAsync(string siteId, HtaccessFolder folder)
        {
            await InvokeAsync("SolidCP.Server.WebServer", "UpdateHeliconApeFolder", siteId, folder);
        }

        public void UpdateHeliconApeHttpdFolder(HtaccessFolder folder)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateHeliconApeHttpdFolder", folder);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeHttpdFolderAsync(HtaccessFolder folder)
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

        public List<HtaccessUser> GetHeliconApeUsers(string siteId)
        {
            return (List<HtaccessUser>)Invoke("SolidCP.Server.WebServer", "GetHeliconApeUsers", siteId);
        }

        public async System.Threading.Tasks.Task<List<HtaccessUser>> GetHeliconApeUsersAsync(string siteId)
        {
            return await InvokeAsync<List<HtaccessUser>>("SolidCP.Server.WebServer", "GetHeliconApeUsers", siteId);
        }

        public HtaccessUser GetHeliconApeUser(string siteId, string userName)
        {
            return (HtaccessUser)Invoke("SolidCP.Server.WebServer", "GetHeliconApeUser", siteId, userName);
        }

        public async System.Threading.Tasks.Task<HtaccessUser> GetHeliconApeUserAsync(string siteId, string userName)
        {
            return await InvokeAsync<HtaccessUser>("SolidCP.Server.WebServer", "GetHeliconApeUser", siteId, userName);
        }

        public void UpdateHeliconApeUser(string siteId, HtaccessUser user)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateHeliconApeUser", siteId, user);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeUserAsync(string siteId, HtaccessUser user)
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

        public List<WebGroup> GetHeliconApeGroups(string siteId)
        {
            return (List<WebGroup>)Invoke("SolidCP.Server.WebServer", "GetHeliconApeGroups", siteId);
        }

        public async System.Threading.Tasks.Task<List<WebGroup>> GetHeliconApeGroupsAsync(string siteId)
        {
            return await InvokeAsync<List<WebGroup>>("SolidCP.Server.WebServer", "GetHeliconApeGroups", siteId);
        }

        public WebGroup GetHeliconApeGroup(string siteId, string groupName)
        {
            return (WebGroup)Invoke("SolidCP.Server.WebServer", "GetHeliconApeGroup", siteId, groupName);
        }

        public async System.Threading.Tasks.Task<WebGroup> GetHeliconApeGroupAsync(string siteId, string groupName)
        {
            return await InvokeAsync<WebGroup>("SolidCP.Server.WebServer", "GetHeliconApeGroup", siteId, groupName);
        }

        public void UpdateHeliconApeGroup(string siteId, WebGroup group)
        {
            Invoke("SolidCP.Server.WebServer", "UpdateHeliconApeGroup", siteId, group);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeGroupAsync(string siteId, WebGroup group)
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

        public WebAppVirtualDirectory[] GetZooApplications(string siteId)
        {
            return (WebAppVirtualDirectory[])Invoke("SolidCP.Server.WebServer", "GetZooApplications", siteId);
        }

        public async System.Threading.Tasks.Task<WebAppVirtualDirectory[]> GetZooApplicationsAsync(string siteId)
        {
            return await InvokeAsync<WebAppVirtualDirectory[]>("SolidCP.Server.WebServer", "GetZooApplications", siteId);
        }

        public StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue)
        {
            return (StringResultObject)Invoke("SolidCP.Server.WebServer", "SetZooEnvironmentVariable", siteId, appName, envName, envValue);
        }

        public async System.Threading.Tasks.Task<StringResultObject> SetZooEnvironmentVariableAsync(string siteId, string appName, string envName, string envValue)
        {
            return await InvokeAsync<StringResultObject>("SolidCP.Server.WebServer", "SetZooEnvironmentVariable", siteId, appName, envName, envValue);
        }

        public StringResultObject SetZooConsoleEnabled(string siteId, string appName)
        {
            return (StringResultObject)Invoke("SolidCP.Server.WebServer", "SetZooConsoleEnabled", siteId, appName);
        }

        public async System.Threading.Tasks.Task<StringResultObject> SetZooConsoleEnabledAsync(string siteId, string appName)
        {
            return await InvokeAsync<StringResultObject>("SolidCP.Server.WebServer", "SetZooConsoleEnabled", siteId, appName);
        }

        public StringResultObject SetZooConsoleDisabled(string siteId, string appName)
        {
            return (StringResultObject)Invoke("SolidCP.Server.WebServer", "SetZooConsoleDisabled", siteId, appName);
        }

        public async System.Threading.Tasks.Task<StringResultObject> SetZooConsoleDisabledAsync(string siteId, string appName)
        {
            return await InvokeAsync<StringResultObject>("SolidCP.Server.WebServer", "SetZooConsoleDisabled", siteId, appName);
        }

        public bool CheckLoadUserProfile()
        {
            return (bool)Invoke("SolidCP.Server.WebServer", "CheckLoadUserProfile");
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

        public GalleryLanguagesResult GetGalleryLanguages(int UserId)
        {
            return (GalleryLanguagesResult)Invoke("SolidCP.Server.WebServer", "GetGalleryLanguages", UserId);
        }

        public async System.Threading.Tasks.Task<GalleryLanguagesResult> GetGalleryLanguagesAsync(int UserId)
        {
            return await InvokeAsync<GalleryLanguagesResult>("SolidCP.Server.WebServer", "GetGalleryLanguages", UserId);
        }

        public GalleryCategoriesResult GetGalleryCategories(int UserId)
        {
            return (GalleryCategoriesResult)Invoke("SolidCP.Server.WebServer", "GetGalleryCategories", UserId);
        }

        public async System.Threading.Tasks.Task<GalleryCategoriesResult> GetGalleryCategoriesAsync(int UserId)
        {
            return await InvokeAsync<GalleryCategoriesResult>("SolidCP.Server.WebServer", "GetGalleryCategories", UserId);
        }

        public GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId)
        {
            return (GalleryApplicationsResult)Invoke("SolidCP.Server.WebServer", "GetGalleryApplications", UserId, categoryId);
        }

        public async System.Threading.Tasks.Task<GalleryApplicationsResult> GetGalleryApplicationsAsync(int UserId, string categoryId)
        {
            return await InvokeAsync<GalleryApplicationsResult>("SolidCP.Server.WebServer", "GetGalleryApplications", UserId, categoryId);
        }

        public GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern)
        {
            return (GalleryApplicationsResult)Invoke("SolidCP.Server.WebServer", "GetGalleryApplicationsFiltered", UserId, pattern);
        }

        public async System.Threading.Tasks.Task<GalleryApplicationsResult> GetGalleryApplicationsFilteredAsync(int UserId, string pattern)
        {
            return await InvokeAsync<GalleryApplicationsResult>("SolidCP.Server.WebServer", "GetGalleryApplicationsFiltered", UserId, pattern);
        }

        public bool IsMsDeployInstalled()
        {
            return (bool)Invoke("SolidCP.Server.WebServer", "IsMsDeployInstalled");
        }

        public async System.Threading.Tasks.Task<bool> IsMsDeployInstalledAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "IsMsDeployInstalled");
        }

        public GalleryApplicationResult GetGalleryApplication(int UserId, string id)
        {
            return (GalleryApplicationResult)Invoke("SolidCP.Server.WebServer", "GetGalleryApplication", UserId, id);
        }

        public async System.Threading.Tasks.Task<GalleryApplicationResult> GetGalleryApplicationAsync(int UserId, string id)
        {
            return await InvokeAsync<GalleryApplicationResult>("SolidCP.Server.WebServer", "GetGalleryApplication", UserId, id);
        }

        public GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id)
        {
            return (GalleryWebAppStatus)Invoke("SolidCP.Server.WebServer", "GetGalleryApplicationStatus", UserId, id);
        }

        public async System.Threading.Tasks.Task<GalleryWebAppStatus> GetGalleryApplicationStatusAsync(int UserId, string id)
        {
            return await InvokeAsync<GalleryWebAppStatus>("SolidCP.Server.WebServer", "GetGalleryApplicationStatus", UserId, id);
        }

        public GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id)
        {
            return (GalleryWebAppStatus)Invoke("SolidCP.Server.WebServer", "DownloadGalleryApplication", UserId, id);
        }

        public async System.Threading.Tasks.Task<GalleryWebAppStatus> DownloadGalleryApplicationAsync(int UserId, string id)
        {
            return await InvokeAsync<GalleryWebAppStatus>("SolidCP.Server.WebServer", "DownloadGalleryApplication", UserId, id);
        }

        public DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id)
        {
            return (DeploymentParametersResult)Invoke("SolidCP.Server.WebServer", "GetGalleryApplicationParameters", UserId, id);
        }

        public async System.Threading.Tasks.Task<DeploymentParametersResult> GetGalleryApplicationParametersAsync(int UserId, string id)
        {
            return await InvokeAsync<DeploymentParametersResult>("SolidCP.Server.WebServer", "GetGalleryApplicationParameters", UserId, id);
        }

        public StringResultObject InstallGalleryApplication(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId)
        {
            return (StringResultObject)Invoke("SolidCP.Server.WebServer", "InstallGalleryApplication", UserId, id, updatedValues, languageId);
        }

        public async System.Threading.Tasks.Task<StringResultObject> InstallGalleryApplicationAsync(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId)
        {
            return await InvokeAsync<StringResultObject>("SolidCP.Server.WebServer", "InstallGalleryApplication", UserId, id, updatedValues, languageId);
        }

        public bool CheckWebManagementAccountExists(string accountName)
        {
            return (bool)Invoke("SolidCP.Server.WebServer", "CheckWebManagementAccountExists", accountName);
        }

        public async System.Threading.Tasks.Task<bool> CheckWebManagementAccountExistsAsync(string accountName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "CheckWebManagementAccountExists", accountName);
        }

        public ResultObject CheckWebManagementPasswordComplexity(string accountPassword)
        {
            return (ResultObject)Invoke("SolidCP.Server.WebServer", "CheckWebManagementPasswordComplexity", accountPassword);
        }

        public async System.Threading.Tasks.Task<ResultObject> CheckWebManagementPasswordComplexityAsync(string accountPassword)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.WebServer", "CheckWebManagementPasswordComplexity", accountPassword);
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

        public SSLCertificate generateCSR(SSLCertificate certificate)
        {
            return (SSLCertificate)Invoke("SolidCP.Server.WebServer", "generateCSR", certificate);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> generateCSRAsync(SSLCertificate certificate)
        {
            return await InvokeAsync<SSLCertificate>("SolidCP.Server.WebServer", "generateCSR", certificate);
        }

        public SSLCertificate generateRenewalCSR(SSLCertificate certificate)
        {
            return (SSLCertificate)Invoke("SolidCP.Server.WebServer", "generateRenewalCSR", certificate);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> generateRenewalCSRAsync(SSLCertificate certificate)
        {
            return await InvokeAsync<SSLCertificate>("SolidCP.Server.WebServer", "generateRenewalCSR", certificate);
        }

        public SSLCertificate getCertificate(WebSite site)
        {
            return (SSLCertificate)Invoke("SolidCP.Server.WebServer", "getCertificate", site);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> getCertificateAsync(WebSite site)
        {
            return await InvokeAsync<SSLCertificate>("SolidCP.Server.WebServer", "getCertificate", site);
        }

        public SSLCertificate installCertificate(SSLCertificate certificate, WebSite website)
        {
            return (SSLCertificate)Invoke("SolidCP.Server.WebServer", "installCertificate", certificate, website);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> installCertificateAsync(SSLCertificate certificate, WebSite website)
        {
            return await InvokeAsync<SSLCertificate>("SolidCP.Server.WebServer", "installCertificate", certificate, website);
        }

        public String LEinstallCertificate(WebSite website, string email)
        {
            return (String)Invoke("SolidCP.Server.WebServer", "LEinstallCertificate", website, email);
        }

        public async System.Threading.Tasks.Task<String> LEinstallCertificateAsync(WebSite website, string email)
        {
            return await InvokeAsync<String>("SolidCP.Server.WebServer", "LEinstallCertificate", website, email);
        }

        public SSLCertificate installPFX(byte[] certificate, string password, WebSite website)
        {
            return (SSLCertificate)Invoke("SolidCP.Server.WebServer", "installPFX", certificate, password, website);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> installPFXAsync(byte[] certificate, string password, WebSite website)
        {
            return await InvokeAsync<SSLCertificate>("SolidCP.Server.WebServer", "installPFX", certificate, password, website);
        }

        public byte[] exportCertificate(string serialNumber, string password)
        {
            return (byte[])Invoke("SolidCP.Server.WebServer", "exportCertificate", serialNumber, password);
        }

        public async System.Threading.Tasks.Task<byte[]> exportCertificateAsync(string serialNumber, string password)
        {
            return await InvokeAsync<byte[]>("SolidCP.Server.WebServer", "exportCertificate", serialNumber, password);
        }

        public List<SSLCertificate> getServerCertificates()
        {
            return (List<SSLCertificate>)Invoke("SolidCP.Server.WebServer", "getServerCertificates");
        }

        public async System.Threading.Tasks.Task<List<SSLCertificate>> getServerCertificatesAsync()
        {
            return await InvokeAsync<List<SSLCertificate>>("SolidCP.Server.WebServer", "getServerCertificates");
        }

        public ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website)
        {
            return (ResultObject)Invoke("SolidCP.Server.WebServer", "DeleteCertificate", certificate, website);
        }

        public async System.Threading.Tasks.Task<ResultObject> DeleteCertificateAsync(SSLCertificate certificate, WebSite website)
        {
            return await InvokeAsync<ResultObject>("SolidCP.Server.WebServer", "DeleteCertificate", certificate, website);
        }

        public SSLCertificate ImportCertificate(WebSite website)
        {
            return (SSLCertificate)Invoke("SolidCP.Server.WebServer", "ImportCertificate", website);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> ImportCertificateAsync(WebSite website)
        {
            return await InvokeAsync<SSLCertificate>("SolidCP.Server.WebServer", "ImportCertificate", website);
        }

        public bool CheckCertificate(WebSite webSite)
        {
            return (bool)Invoke("SolidCP.Server.WebServer", "CheckCertificate", webSite);
        }

        public async System.Threading.Tasks.Task<bool> CheckCertificateAsync(WebSite webSite)
        {
            return await InvokeAsync<bool>("SolidCP.Server.WebServer", "CheckCertificate", webSite);
        }

        public bool GetDirectoryBrowseEnabled(string siteId)
        {
            return (bool)Invoke("SolidCP.Server.WebServer", "GetDirectoryBrowseEnabled", siteId);
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
        public void ChangeSiteState(string siteId, ServerState state)
        {
            base.Client.ChangeSiteState(siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeSiteStateAsync(string siteId, ServerState state)
        {
            await base.Client.ChangeSiteStateAsync(siteId, state);
        }

        public ServerState GetSiteState(string siteId)
        {
            return base.Client.GetSiteState(siteId);
        }

        public async System.Threading.Tasks.Task<ServerState> GetSiteStateAsync(string siteId)
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

        public WebSite GetSite(string siteId)
        {
            return base.Client.GetSite(siteId);
        }

        public async System.Threading.Tasks.Task<WebSite> GetSiteAsync(string siteId)
        {
            return await base.Client.GetSiteAsync(siteId);
        }

        public ServerBinding[] GetSiteBindings(string siteId)
        {
            return base.Client.GetSiteBindings(siteId);
        }

        public async System.Threading.Tasks.Task<ServerBinding[]> GetSiteBindingsAsync(string siteId)
        {
            return await base.Client.GetSiteBindingsAsync(siteId);
        }

        public string CreateSite(WebSite site)
        {
            return base.Client.CreateSite(site);
        }

        public async System.Threading.Tasks.Task<string> CreateSiteAsync(WebSite site)
        {
            return await base.Client.CreateSiteAsync(site);
        }

        public void UpdateSite(WebSite site)
        {
            base.Client.UpdateSite(site);
        }

        public async System.Threading.Tasks.Task UpdateSiteAsync(WebSite site)
        {
            await base.Client.UpdateSiteAsync(site);
        }

        public void UpdateSiteBindings(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed)
        {
            base.Client.UpdateSiteBindings(siteId, bindings, emptyBindingsAllowed);
        }

        public async System.Threading.Tasks.Task UpdateSiteBindingsAsync(string siteId, ServerBinding[] bindings, bool emptyBindingsAllowed)
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

        public void ChangeAppPoolState(string siteId, AppPoolState state)
        {
            base.Client.ChangeAppPoolState(siteId, state);
        }

        public async System.Threading.Tasks.Task ChangeAppPoolStateAsync(string siteId, AppPoolState state)
        {
            await base.Client.ChangeAppPoolStateAsync(siteId, state);
        }

        public AppPoolState GetAppPoolState(string siteId)
        {
            return base.Client.GetAppPoolState(siteId);
        }

        public async System.Threading.Tasks.Task<AppPoolState> GetAppPoolStateAsync(string siteId)
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

        public WebVirtualDirectory[] GetVirtualDirectories(string siteId)
        {
            return base.Client.GetVirtualDirectories(siteId);
        }

        public async System.Threading.Tasks.Task<WebVirtualDirectory[]> GetVirtualDirectoriesAsync(string siteId)
        {
            return await base.Client.GetVirtualDirectoriesAsync(siteId);
        }

        public WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
        {
            return base.Client.GetVirtualDirectory(siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<WebVirtualDirectory> GetVirtualDirectoryAsync(string siteId, string directoryName)
        {
            return await base.Client.GetVirtualDirectoryAsync(siteId, directoryName);
        }

        public void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory)
        {
            base.Client.CreateVirtualDirectory(siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateVirtualDirectoryAsync(string siteId, WebVirtualDirectory directory)
        {
            await base.Client.CreateVirtualDirectoryAsync(siteId, directory);
        }

        public void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory)
        {
            base.Client.UpdateVirtualDirectory(siteId, directory);
        }

        public async System.Threading.Tasks.Task UpdateVirtualDirectoryAsync(string siteId, WebVirtualDirectory directory)
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

        public WebAppVirtualDirectory[] GetAppVirtualDirectories(string siteId)
        {
            return base.Client.GetAppVirtualDirectories(siteId);
        }

        public async System.Threading.Tasks.Task<WebAppVirtualDirectory[]> GetAppVirtualDirectoriesAsync(string siteId)
        {
            return await base.Client.GetAppVirtualDirectoriesAsync(siteId);
        }

        public WebAppVirtualDirectory GetAppVirtualDirectory(string siteId, string directoryName)
        {
            return base.Client.GetAppVirtualDirectory(siteId, directoryName);
        }

        public async System.Threading.Tasks.Task<WebAppVirtualDirectory> GetAppVirtualDirectoryAsync(string siteId, string directoryName)
        {
            return await base.Client.GetAppVirtualDirectoryAsync(siteId, directoryName);
        }

        public void CreateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            base.Client.CreateAppVirtualDirectory(siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateAppVirtualDirectoryAsync(string siteId, WebAppVirtualDirectory directory)
        {
            await base.Client.CreateAppVirtualDirectoryAsync(siteId, directory);
        }

        public void CreateEnterpriseStorageAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            base.Client.CreateEnterpriseStorageAppVirtualDirectory(siteId, directory);
        }

        public async System.Threading.Tasks.Task CreateEnterpriseStorageAppVirtualDirectoryAsync(string siteId, WebAppVirtualDirectory directory)
        {
            await base.Client.CreateEnterpriseStorageAppVirtualDirectoryAsync(siteId, directory);
        }

        public void UpdateAppVirtualDirectory(string siteId, WebAppVirtualDirectory directory)
        {
            base.Client.UpdateAppVirtualDirectory(siteId, directory);
        }

        public async System.Threading.Tasks.Task UpdateAppVirtualDirectoryAsync(string siteId, WebAppVirtualDirectory directory)
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

        public void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission)
        {
            base.Client.GrantWebSiteAccess(path, siteId, permission);
        }

        public async System.Threading.Tasks.Task GrantWebSiteAccessAsync(string path, string siteId, NTFSPermission permission)
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

        public List<WebFolder> GetFolders(string siteId)
        {
            return base.Client.GetFolders(siteId);
        }

        public async System.Threading.Tasks.Task<List<WebFolder>> GetFoldersAsync(string siteId)
        {
            return await base.Client.GetFoldersAsync(siteId);
        }

        public WebFolder GetFolder(string siteId, string folderPath)
        {
            return base.Client.GetFolder(siteId, folderPath);
        }

        public async System.Threading.Tasks.Task<WebFolder> GetFolderAsync(string siteId, string folderPath)
        {
            return await base.Client.GetFolderAsync(siteId, folderPath);
        }

        public void UpdateFolder(string siteId, WebFolder folder)
        {
            base.Client.UpdateFolder(siteId, folder);
        }

        public async System.Threading.Tasks.Task UpdateFolderAsync(string siteId, WebFolder folder)
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

        public List<WebUser> GetUsers(string siteId)
        {
            return base.Client.GetUsers(siteId);
        }

        public async System.Threading.Tasks.Task<List<WebUser>> GetUsersAsync(string siteId)
        {
            return await base.Client.GetUsersAsync(siteId);
        }

        public WebUser GetUser(string siteId, string userName)
        {
            return base.Client.GetUser(siteId, userName);
        }

        public async System.Threading.Tasks.Task<WebUser> GetUserAsync(string siteId, string userName)
        {
            return await base.Client.GetUserAsync(siteId, userName);
        }

        public void UpdateUser(string siteId, WebUser user)
        {
            base.Client.UpdateUser(siteId, user);
        }

        public async System.Threading.Tasks.Task UpdateUserAsync(string siteId, WebUser user)
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

        public List<WebGroup> GetGroups(string siteId)
        {
            return base.Client.GetGroups(siteId);
        }

        public async System.Threading.Tasks.Task<List<WebGroup>> GetGroupsAsync(string siteId)
        {
            return await base.Client.GetGroupsAsync(siteId);
        }

        public WebGroup GetGroup(string siteId, string groupName)
        {
            return base.Client.GetGroup(siteId, groupName);
        }

        public async System.Threading.Tasks.Task<WebGroup> GetGroupAsync(string siteId, string groupName)
        {
            return await base.Client.GetGroupAsync(siteId, groupName);
        }

        public void UpdateGroup(string siteId, WebGroup group)
        {
            base.Client.UpdateGroup(siteId, group);
        }

        public async System.Threading.Tasks.Task UpdateGroupAsync(string siteId, WebGroup group)
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

        public HeliconApeStatus GetHeliconApeStatus(string siteId)
        {
            return base.Client.GetHeliconApeStatus(siteId);
        }

        public async System.Threading.Tasks.Task<HeliconApeStatus> GetHeliconApeStatusAsync(string siteId)
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

        public List<HtaccessFolder> GetHeliconApeFolders(string siteId)
        {
            return base.Client.GetHeliconApeFolders(siteId);
        }

        public async System.Threading.Tasks.Task<List<HtaccessFolder>> GetHeliconApeFoldersAsync(string siteId)
        {
            return await base.Client.GetHeliconApeFoldersAsync(siteId);
        }

        public HtaccessFolder GetHeliconApeHttpdFolder()
        {
            return base.Client.GetHeliconApeHttpdFolder();
        }

        public async System.Threading.Tasks.Task<HtaccessFolder> GetHeliconApeHttpdFolderAsync()
        {
            return await base.Client.GetHeliconApeHttpdFolderAsync();
        }

        public HtaccessFolder GetHeliconApeFolder(string siteId, string folderPath)
        {
            return base.Client.GetHeliconApeFolder(siteId, folderPath);
        }

        public async System.Threading.Tasks.Task<HtaccessFolder> GetHeliconApeFolderAsync(string siteId, string folderPath)
        {
            return await base.Client.GetHeliconApeFolderAsync(siteId, folderPath);
        }

        public void UpdateHeliconApeFolder(string siteId, HtaccessFolder folder)
        {
            base.Client.UpdateHeliconApeFolder(siteId, folder);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeFolderAsync(string siteId, HtaccessFolder folder)
        {
            await base.Client.UpdateHeliconApeFolderAsync(siteId, folder);
        }

        public void UpdateHeliconApeHttpdFolder(HtaccessFolder folder)
        {
            base.Client.UpdateHeliconApeHttpdFolder(folder);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeHttpdFolderAsync(HtaccessFolder folder)
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

        public List<HtaccessUser> GetHeliconApeUsers(string siteId)
        {
            return base.Client.GetHeliconApeUsers(siteId);
        }

        public async System.Threading.Tasks.Task<List<HtaccessUser>> GetHeliconApeUsersAsync(string siteId)
        {
            return await base.Client.GetHeliconApeUsersAsync(siteId);
        }

        public HtaccessUser GetHeliconApeUser(string siteId, string userName)
        {
            return base.Client.GetHeliconApeUser(siteId, userName);
        }

        public async System.Threading.Tasks.Task<HtaccessUser> GetHeliconApeUserAsync(string siteId, string userName)
        {
            return await base.Client.GetHeliconApeUserAsync(siteId, userName);
        }

        public void UpdateHeliconApeUser(string siteId, HtaccessUser user)
        {
            base.Client.UpdateHeliconApeUser(siteId, user);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeUserAsync(string siteId, HtaccessUser user)
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

        public List<WebGroup> GetHeliconApeGroups(string siteId)
        {
            return base.Client.GetHeliconApeGroups(siteId);
        }

        public async System.Threading.Tasks.Task<List<WebGroup>> GetHeliconApeGroupsAsync(string siteId)
        {
            return await base.Client.GetHeliconApeGroupsAsync(siteId);
        }

        public WebGroup GetHeliconApeGroup(string siteId, string groupName)
        {
            return base.Client.GetHeliconApeGroup(siteId, groupName);
        }

        public async System.Threading.Tasks.Task<WebGroup> GetHeliconApeGroupAsync(string siteId, string groupName)
        {
            return await base.Client.GetHeliconApeGroupAsync(siteId, groupName);
        }

        public void UpdateHeliconApeGroup(string siteId, WebGroup group)
        {
            base.Client.UpdateHeliconApeGroup(siteId, group);
        }

        public async System.Threading.Tasks.Task UpdateHeliconApeGroupAsync(string siteId, WebGroup group)
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

        public WebAppVirtualDirectory[] GetZooApplications(string siteId)
        {
            return base.Client.GetZooApplications(siteId);
        }

        public async System.Threading.Tasks.Task<WebAppVirtualDirectory[]> GetZooApplicationsAsync(string siteId)
        {
            return await base.Client.GetZooApplicationsAsync(siteId);
        }

        public StringResultObject SetZooEnvironmentVariable(string siteId, string appName, string envName, string envValue)
        {
            return base.Client.SetZooEnvironmentVariable(siteId, appName, envName, envValue);
        }

        public async System.Threading.Tasks.Task<StringResultObject> SetZooEnvironmentVariableAsync(string siteId, string appName, string envName, string envValue)
        {
            return await base.Client.SetZooEnvironmentVariableAsync(siteId, appName, envName, envValue);
        }

        public StringResultObject SetZooConsoleEnabled(string siteId, string appName)
        {
            return base.Client.SetZooConsoleEnabled(siteId, appName);
        }

        public async System.Threading.Tasks.Task<StringResultObject> SetZooConsoleEnabledAsync(string siteId, string appName)
        {
            return await base.Client.SetZooConsoleEnabledAsync(siteId, appName);
        }

        public StringResultObject SetZooConsoleDisabled(string siteId, string appName)
        {
            return base.Client.SetZooConsoleDisabled(siteId, appName);
        }

        public async System.Threading.Tasks.Task<StringResultObject> SetZooConsoleDisabledAsync(string siteId, string appName)
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

        public GalleryLanguagesResult GetGalleryLanguages(int UserId)
        {
            return base.Client.GetGalleryLanguages(UserId);
        }

        public async System.Threading.Tasks.Task<GalleryLanguagesResult> GetGalleryLanguagesAsync(int UserId)
        {
            return await base.Client.GetGalleryLanguagesAsync(UserId);
        }

        public GalleryCategoriesResult GetGalleryCategories(int UserId)
        {
            return base.Client.GetGalleryCategories(UserId);
        }

        public async System.Threading.Tasks.Task<GalleryCategoriesResult> GetGalleryCategoriesAsync(int UserId)
        {
            return await base.Client.GetGalleryCategoriesAsync(UserId);
        }

        public GalleryApplicationsResult GetGalleryApplications(int UserId, string categoryId)
        {
            return base.Client.GetGalleryApplications(UserId, categoryId);
        }

        public async System.Threading.Tasks.Task<GalleryApplicationsResult> GetGalleryApplicationsAsync(int UserId, string categoryId)
        {
            return await base.Client.GetGalleryApplicationsAsync(UserId, categoryId);
        }

        public GalleryApplicationsResult GetGalleryApplicationsFiltered(int UserId, string pattern)
        {
            return base.Client.GetGalleryApplicationsFiltered(UserId, pattern);
        }

        public async System.Threading.Tasks.Task<GalleryApplicationsResult> GetGalleryApplicationsFilteredAsync(int UserId, string pattern)
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

        public GalleryApplicationResult GetGalleryApplication(int UserId, string id)
        {
            return base.Client.GetGalleryApplication(UserId, id);
        }

        public async System.Threading.Tasks.Task<GalleryApplicationResult> GetGalleryApplicationAsync(int UserId, string id)
        {
            return await base.Client.GetGalleryApplicationAsync(UserId, id);
        }

        public GalleryWebAppStatus GetGalleryApplicationStatus(int UserId, string id)
        {
            return base.Client.GetGalleryApplicationStatus(UserId, id);
        }

        public async System.Threading.Tasks.Task<GalleryWebAppStatus> GetGalleryApplicationStatusAsync(int UserId, string id)
        {
            return await base.Client.GetGalleryApplicationStatusAsync(UserId, id);
        }

        public GalleryWebAppStatus DownloadGalleryApplication(int UserId, string id)
        {
            return base.Client.DownloadGalleryApplication(UserId, id);
        }

        public async System.Threading.Tasks.Task<GalleryWebAppStatus> DownloadGalleryApplicationAsync(int UserId, string id)
        {
            return await base.Client.DownloadGalleryApplicationAsync(UserId, id);
        }

        public DeploymentParametersResult GetGalleryApplicationParameters(int UserId, string id)
        {
            return base.Client.GetGalleryApplicationParameters(UserId, id);
        }

        public async System.Threading.Tasks.Task<DeploymentParametersResult> GetGalleryApplicationParametersAsync(int UserId, string id)
        {
            return await base.Client.GetGalleryApplicationParametersAsync(UserId, id);
        }

        public StringResultObject InstallGalleryApplication(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId)
        {
            return base.Client.InstallGalleryApplication(UserId, id, updatedValues, languageId);
        }

        public async System.Threading.Tasks.Task<StringResultObject> InstallGalleryApplicationAsync(int UserId, string id, List<DeploymentParameter> updatedValues, string languageId)
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

        public ResultObject CheckWebManagementPasswordComplexity(string accountPassword)
        {
            return base.Client.CheckWebManagementPasswordComplexity(accountPassword);
        }

        public async System.Threading.Tasks.Task<ResultObject> CheckWebManagementPasswordComplexityAsync(string accountPassword)
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

        public SSLCertificate generateCSR(SSLCertificate certificate)
        {
            return base.Client.generateCSR(certificate);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> generateCSRAsync(SSLCertificate certificate)
        {
            return await base.Client.generateCSRAsync(certificate);
        }

        public SSLCertificate generateRenewalCSR(SSLCertificate certificate)
        {
            return base.Client.generateRenewalCSR(certificate);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> generateRenewalCSRAsync(SSLCertificate certificate)
        {
            return await base.Client.generateRenewalCSRAsync(certificate);
        }

        public SSLCertificate getCertificate(WebSite site)
        {
            return base.Client.getCertificate(site);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> getCertificateAsync(WebSite site)
        {
            return await base.Client.getCertificateAsync(site);
        }

        public SSLCertificate installCertificate(SSLCertificate certificate, WebSite website)
        {
            return base.Client.installCertificate(certificate, website);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> installCertificateAsync(SSLCertificate certificate, WebSite website)
        {
            return await base.Client.installCertificateAsync(certificate, website);
        }

        public String LEinstallCertificate(WebSite website, string email)
        {
            return base.Client.LEinstallCertificate(website, email);
        }

        public async System.Threading.Tasks.Task<String> LEinstallCertificateAsync(WebSite website, string email)
        {
            return await base.Client.LEinstallCertificateAsync(website, email);
        }

        public SSLCertificate installPFX(byte[] certificate, string password, WebSite website)
        {
            return base.Client.installPFX(certificate, password, website);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> installPFXAsync(byte[] certificate, string password, WebSite website)
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

        public List<SSLCertificate> getServerCertificates()
        {
            return base.Client.getServerCertificates();
        }

        public async System.Threading.Tasks.Task<List<SSLCertificate>> getServerCertificatesAsync()
        {
            return await base.Client.getServerCertificatesAsync();
        }

        public ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website)
        {
            return base.Client.DeleteCertificate(certificate, website);
        }

        public async System.Threading.Tasks.Task<ResultObject> DeleteCertificateAsync(SSLCertificate certificate, WebSite website)
        {
            return await base.Client.DeleteCertificateAsync(certificate, website);
        }

        public SSLCertificate ImportCertificate(WebSite website)
        {
            return base.Client.ImportCertificate(website);
        }

        public async System.Threading.Tasks.Task<SSLCertificate> ImportCertificateAsync(WebSite website)
        {
            return await base.Client.ImportCertificateAsync(website);
        }

        public bool CheckCertificate(WebSite webSite)
        {
            return base.Client.CheckCertificate(webSite);
        }

        public async System.Threading.Tasks.Task<bool> CheckCertificateAsync(WebSite webSite)
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