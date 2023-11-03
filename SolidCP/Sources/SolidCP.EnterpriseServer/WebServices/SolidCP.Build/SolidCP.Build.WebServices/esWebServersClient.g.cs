#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesWebServers", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesWebServers
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetRawWebSitesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetRawWebSitesPagedResponse")]
        System.Data.DataSet GetRawWebSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetRawWebSitesPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetRawWebSitesPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawWebSitesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSites", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSitesResponse")]
        SolidCP.Providers.Web.WebSite[] /*List*/ GetWebSites(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSites", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSitesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebSite[]> GetWebSitesAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSiteResponse")]
        SolidCP.Providers.Web.WebSite GetWebSite(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSiteResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebSite> GetWebSiteAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSitePointers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSitePointersResponse")]
        SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetWebSitePointers(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSitePointers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebSitePointersResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetWebSitePointersAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddWebSitePointer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddWebSitePointerResponse")]
        int AddWebSitePointer(int siteItemId, string hostName, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddWebSitePointer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddWebSitePointerResponse")]
        System.Threading.Tasks.Task<int> AddWebSitePointerAsync(int siteItemId, string hostName, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteWebSitePointer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteWebSitePointerResponse")]
        int DeleteWebSitePointer(int siteItemId, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteWebSitePointer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteWebSitePointerResponse")]
        System.Threading.Tasks.Task<int> DeleteWebSitePointerAsync(int siteItemId, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddWebSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddWebSiteResponse")]
        int AddWebSite(int packageId, string hostName, int domainId, int ipAddressId, bool ignoreGlobalDNSZone);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddWebSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddWebSiteResponse")]
        System.Threading.Tasks.Task<int> AddWebSiteAsync(int packageId, string hostName, int domainId, int ipAddressId, bool ignoreGlobalDNSZone);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateWebSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateWebSiteResponse")]
        int UpdateWebSite(SolidCP.Providers.Web.WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateWebSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateWebSiteResponse")]
        System.Threading.Tasks.Task<int> UpdateWebSiteAsync(SolidCP.Providers.Web.WebSite site);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallFrontPage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallFrontPageResponse")]
        int InstallFrontPage(int siteItemId, string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallFrontPage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallFrontPageResponse")]
        System.Threading.Tasks.Task<int> InstallFrontPageAsync(int siteItemId, string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UninstallFrontPage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UninstallFrontPageResponse")]
        int UninstallFrontPage(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UninstallFrontPage", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UninstallFrontPageResponse")]
        System.Threading.Tasks.Task<int> UninstallFrontPageAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeFrontPagePassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeFrontPagePasswordResponse")]
        int ChangeFrontPagePassword(int siteItemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeFrontPagePassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeFrontPagePasswordResponse")]
        System.Threading.Tasks.Task<int> ChangeFrontPagePasswordAsync(int siteItemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RepairWebSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RepairWebSiteResponse")]
        int RepairWebSite(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RepairWebSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RepairWebSiteResponse")]
        System.Threading.Tasks.Task<int> RepairWebSiteAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteWebSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteWebSiteResponse")]
        int DeleteWebSite(int siteItemId, bool deleteWebsiteDirectory);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteWebSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteWebSiteResponse")]
        System.Threading.Tasks.Task<int> DeleteWebSiteAsync(int siteItemId, bool deleteWebsiteDirectory);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SwitchWebSiteToDedicatedIP", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SwitchWebSiteToDedicatedIPResponse")]
        int SwitchWebSiteToDedicatedIP(int siteItemId, int ipAddressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SwitchWebSiteToDedicatedIP", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SwitchWebSiteToDedicatedIPResponse")]
        System.Threading.Tasks.Task<int> SwitchWebSiteToDedicatedIPAsync(int siteItemId, int ipAddressId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SwitchWebSiteToSharedIP", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SwitchWebSiteToSharedIPResponse")]
        int SwitchWebSiteToSharedIP(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SwitchWebSiteToSharedIP", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SwitchWebSiteToSharedIPResponse")]
        System.Threading.Tasks.Task<int> SwitchWebSiteToSharedIPAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddVirtualDirectoryResponse")]
        int AddVirtualDirectory(int siteItemId, string vdirName, string vdirPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<int> AddVirtualDirectoryAsync(int siteItemId, string vdirName, string vdirPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetVirtualDirectoriesResponse")]
        SolidCP.Providers.Web.WebVirtualDirectory[] /*List*/ GetVirtualDirectories(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetVirtualDirectoriesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory[]> GetVirtualDirectoriesAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetVirtualDirectoryResponse")]
        SolidCP.Providers.Web.WebVirtualDirectory GetVirtualDirectory(int siteItemId, string vdirName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory> GetVirtualDirectoryAsync(int siteItemId, string vdirName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateVirtualDirectoryResponse")]
        int UpdateVirtualDirectory(int siteItemId, SolidCP.Providers.Web.WebVirtualDirectory vdir);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<int> UpdateVirtualDirectoryAsync(int siteItemId, SolidCP.Providers.Web.WebVirtualDirectory vdir);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteVirtualDirectoryResponse")]
        int DeleteVirtualDirectory(int siteItemId, string vdirName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<int> DeleteVirtualDirectoryAsync(int siteItemId, string vdirName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddAppVirtualDirectoryResponse")]
        int AddAppVirtualDirectory(int siteItemId, string vdirName, string vdirPath, string aspNetVersion);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<int> AddAppVirtualDirectoryAsync(int siteItemId, string vdirName, string vdirPath, string aspNetVersion);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppVirtualDirectoriesResponse")]
        SolidCP.Providers.Web.WebAppVirtualDirectory[] /*List*/ GetAppVirtualDirectories(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppVirtualDirectories", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppVirtualDirectoriesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetAppVirtualDirectoriesAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppVirtualDirectoryResponse")]
        SolidCP.Providers.Web.WebAppVirtualDirectory GetAppVirtualDirectory(int siteItemId, string vdirName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory> GetAppVirtualDirectoryAsync(int siteItemId, string vdirName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateAppVirtualDirectoryResponse")]
        int UpdateAppVirtualDirectory(int siteItemId, SolidCP.Providers.Web.WebAppVirtualDirectory vdir);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<int> UpdateAppVirtualDirectoryAsync(int siteItemId, SolidCP.Providers.Web.WebAppVirtualDirectory vdir);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteAppVirtualDirectoryResponse")]
        int DeleteAppVirtualDirectory(int siteItemId, string vdirName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteAppVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteAppVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<int> DeleteAppVirtualDirectoryAsync(int siteItemId, string vdirName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeSiteState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeSiteStateResponse")]
        int ChangeSiteState(int siteItemId, SolidCP.Providers.ServerState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeSiteState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeSiteStateResponse")]
        System.Threading.Tasks.Task<int> ChangeSiteStateAsync(int siteItemId, SolidCP.Providers.ServerState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeAppPoolState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeAppPoolStateResponse")]
        int ChangeAppPoolState(int siteItemId, SolidCP.Providers.AppPoolState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeAppPoolState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeAppPoolStateResponse")]
        System.Threading.Tasks.Task<int> ChangeAppPoolStateAsync(int siteItemId, SolidCP.Providers.AppPoolState state);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppPoolState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppPoolStateResponse")]
        SolidCP.Providers.AppPoolState GetAppPoolState(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppPoolState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetAppPoolStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.AppPoolState> GetAppPoolStateAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSiteState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSiteStateResponse")]
        SolidCP.Providers.ServerState GetSiteState(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSiteState", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSiteStateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ServerState> GetSiteStateAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLDomains", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLDomainsResponse")]
        string[] /*List*/ GetSharedSSLDomains(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLDomains", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLDomainsResponse")]
        System.Threading.Tasks.Task<string[]> GetSharedSSLDomainsAsync(int packageId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetRawSSLFoldersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetRawSSLFoldersPagedResponse")]
        System.Data.DataSet GetRawSSLFoldersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetRawSSLFoldersPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetRawSSLFoldersPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawSSLFoldersPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLFoldersResponse")]
        SolidCP.Providers.Web.SharedSSLFolder[] /*List*/ GetSharedSSLFolders(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLFoldersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SharedSSLFolder[]> GetSharedSSLFoldersAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLFolderResponse")]
        SolidCP.Providers.Web.SharedSSLFolder GetSharedSSLFolder(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSharedSSLFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SharedSSLFolder> GetSharedSSLFolderAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddSharedSSLFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddSharedSSLFolderResponse")]
        int AddSharedSSLFolder(int packageId, string sslDomain, int siteId, string vdirName, string vdirPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddSharedSSLFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/AddSharedSSLFolderResponse")]
        System.Threading.Tasks.Task<int> AddSharedSSLFolderAsync(int packageId, string sslDomain, int siteId, string vdirName, string vdirPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSharedSSLFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSharedSSLFolderResponse")]
        int UpdateSharedSSLFolder(SolidCP.Providers.Web.SharedSSLFolder vdir);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSharedSSLFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSharedSSLFolderResponse")]
        System.Threading.Tasks.Task<int> UpdateSharedSSLFolderAsync(SolidCP.Providers.Web.SharedSSLFolder vdir);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSharedSSLFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSharedSSLFolderResponse")]
        int DeleteSharedSSLFolder(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSharedSSLFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSharedSSLFolderResponse")]
        System.Threading.Tasks.Task<int> DeleteSharedSSLFolderAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallSecuredFoldersResponse")]
        int InstallSecuredFolders(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallSecuredFoldersResponse")]
        System.Threading.Tasks.Task<int> InstallSecuredFoldersAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UninstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UninstallSecuredFoldersResponse")]
        int UninstallSecuredFolders(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UninstallSecuredFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UninstallSecuredFoldersResponse")]
        System.Threading.Tasks.Task<int> UninstallSecuredFoldersAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredFoldersResponse")]
        SolidCP.Providers.Web.WebFolder[] GetSecuredFolders(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredFoldersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder[]> GetSecuredFoldersAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredFolderResponse")]
        SolidCP.Providers.Web.WebFolder GetSecuredFolder(int siteItemId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder> GetSecuredFolderAsync(int siteItemId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredFolderResponse")]
        int UpdateSecuredFolder(int siteItemId, SolidCP.Providers.Web.WebFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredFolderResponse")]
        System.Threading.Tasks.Task<int> UpdateSecuredFolderAsync(int siteItemId, SolidCP.Providers.Web.WebFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredFolderResponse")]
        int DeleteSecuredFolder(int siteItemId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredFolderResponse")]
        System.Threading.Tasks.Task<int> DeleteSecuredFolderAsync(int siteItemId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredUsersResponse")]
        SolidCP.Providers.Web.WebUser[] GetSecuredUsers(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredUsersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser[]> GetSecuredUsersAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredUserResponse")]
        SolidCP.Providers.Web.WebUser GetSecuredUser(int siteItemId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser> GetSecuredUserAsync(int siteItemId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredUserResponse")]
        int UpdateSecuredUser(int siteItemId, SolidCP.Providers.Web.WebUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredUserResponse")]
        System.Threading.Tasks.Task<int> UpdateSecuredUserAsync(int siteItemId, SolidCP.Providers.Web.WebUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredUserResponse")]
        int DeleteSecuredUser(int siteItemId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredUserResponse")]
        System.Threading.Tasks.Task<int> DeleteSecuredUserAsync(int siteItemId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredGroupsResponse")]
        SolidCP.Providers.Web.WebGroup[] GetSecuredGroups(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetSecuredGroupsAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredGroupResponse")]
        SolidCP.Providers.Web.WebGroup GetSecuredGroup(int siteItemId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSecuredGroupResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetSecuredGroupAsync(int siteItemId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredGroupResponse")]
        int UpdateSecuredGroup(int siteItemId, SolidCP.Providers.Web.WebGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateSecuredGroupResponse")]
        System.Threading.Tasks.Task<int> UpdateSecuredGroupAsync(int siteItemId, SolidCP.Providers.Web.WebGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredGroupResponse")]
        int DeleteSecuredGroup(int siteItemId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteSecuredGroupResponse")]
        System.Threading.Tasks.Task<int> DeleteSecuredGroupAsync(int siteItemId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GrantWebDeployPublishingAccess", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GrantWebDeployPublishingAccessResponse")]
        SolidCP.Providers.Common.ResultObject GrantWebDeployPublishingAccess(int siteItemId, string accountName, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GrantWebDeployPublishingAccess", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GrantWebDeployPublishingAccessResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> GrantWebDeployPublishingAccessAsync(int siteItemId, string accountName, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SaveWebDeployPublishingProfile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SaveWebDeployPublishingProfileResponse")]
        SolidCP.Providers.Common.ResultObject SaveWebDeployPublishingProfile(int siteItemId, int[] serviceItemIds);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SaveWebDeployPublishingProfile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SaveWebDeployPublishingProfileResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SaveWebDeployPublishingProfileAsync(int siteItemId, int[] serviceItemIds);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RevokeWebDeployPublishingAccess", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RevokeWebDeployPublishingAccessResponse")]
        void RevokeWebDeployPublishingAccess(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RevokeWebDeployPublishingAccess", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RevokeWebDeployPublishingAccessResponse")]
        System.Threading.Tasks.Task RevokeWebDeployPublishingAccessAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebDeployPublishingProfile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebDeployPublishingProfileResponse")]
        SolidCP.Providers.ResultObjects.BytesResult GetWebDeployPublishingProfile(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebDeployPublishingProfile", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetWebDeployPublishingProfileResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BytesResult> GetWebDeployPublishingProfileAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeWebDeployPublishingPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeWebDeployPublishingPasswordResponse")]
        SolidCP.Providers.Common.ResultObject ChangeWebDeployPublishingPassword(int siteItemId, string newAccountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeWebDeployPublishingPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeWebDeployPublishingPasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeWebDeployPublishingPasswordAsync(int siteItemId, string newAccountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeStatusResponse")]
        SolidCP.Providers.ResultObjects.HeliconApeStatus GetHeliconApeStatus(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeStatus", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeStatusResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.HeliconApeStatus> GetHeliconApeStatusAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallHeliconApe", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallHeliconApeResponse")]
        void InstallHeliconApe(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallHeliconApe", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallHeliconApeResponse")]
        System.Threading.Tasks.Task InstallHeliconApeAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/EnableHeliconApe", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/EnableHeliconApeResponse")]
        int EnableHeliconApe(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/EnableHeliconApe", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/EnableHeliconApeResponse")]
        System.Threading.Tasks.Task<int> EnableHeliconApeAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DisableHeliconApe", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DisableHeliconApeResponse")]
        int DisableHeliconApe(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DisableHeliconApe", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DisableHeliconApeResponse")]
        System.Threading.Tasks.Task<int> DisableHeliconApeAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/EnableHeliconApeGlobally", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/EnableHeliconApeGloballyResponse")]
        int EnableHeliconApeGlobally(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/EnableHeliconApeGlobally", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/EnableHeliconApeGloballyResponse")]
        System.Threading.Tasks.Task<int> EnableHeliconApeGloballyAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DisableHeliconApeGlobally", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DisableHeliconApeGloballyResponse")]
        int DisableHeliconApeGlobally(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DisableHeliconApeGlobally", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DisableHeliconApeGloballyResponse")]
        System.Threading.Tasks.Task<int> DisableHeliconApeGloballyAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeFoldersResponse")]
        SolidCP.Providers.Web.HtaccessFolder[] GetHeliconApeFolders(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeFoldersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder[]> GetHeliconApeFoldersAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeHttpdFolderResponse")]
        SolidCP.Providers.Web.HtaccessFolder GetHeliconApeHttpdFolder(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeHttpdFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeHttpdFolderAsync(int serviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeFolderResponse")]
        SolidCP.Providers.Web.HtaccessFolder GetHeliconApeFolder(int siteItemId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeFolderAsync(int siteItemId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeFolderResponse")]
        int UpdateHeliconApeFolder(int siteItemId, SolidCP.Providers.Web.HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeFolderResponse")]
        System.Threading.Tasks.Task<int> UpdateHeliconApeFolderAsync(int siteItemId, SolidCP.Providers.Web.HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeHttpdFolderResponse")]
        int UpdateHeliconApeHttpdFolder(int serviceId, SolidCP.Providers.Web.HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeHttpdFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeHttpdFolderResponse")]
        System.Threading.Tasks.Task<int> UpdateHeliconApeHttpdFolderAsync(int serviceId, SolidCP.Providers.Web.HtaccessFolder folder);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeFolderResponse")]
        int DeleteHeliconApeFolder(int siteItemId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeFolderResponse")]
        System.Threading.Tasks.Task<int> DeleteHeliconApeFolderAsync(int siteItemId, string folderPath);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeUsersResponse")]
        SolidCP.Providers.Web.HtaccessUser[] GetHeliconApeUsers(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeUsers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeUsersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser[]> GetHeliconApeUsersAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeUserResponse")]
        SolidCP.Providers.Web.HtaccessUser GetHeliconApeUser(int siteItemId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeUserResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser> GetHeliconApeUserAsync(int siteItemId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeUserResponse")]
        int UpdateHeliconApeUser(int siteItemId, SolidCP.Providers.Web.HtaccessUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeUserResponse")]
        System.Threading.Tasks.Task<int> UpdateHeliconApeUserAsync(int siteItemId, SolidCP.Providers.Web.HtaccessUser user);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeUserResponse")]
        int DeleteHeliconApeUser(int siteItemId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeUser", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeUserResponse")]
        System.Threading.Tasks.Task<int> DeleteHeliconApeUserAsync(int siteItemId, string userName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeGroupsResponse")]
        SolidCP.Providers.Web.WebGroup[] GetHeliconApeGroups(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeGroups", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetHeliconApeGroupsAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeGroupResponse")]
        SolidCP.Providers.Web.WebGroup GetHeliconApeGroup(int siteItemId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetHeliconApeGroupResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetHeliconApeGroupAsync(int siteItemId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeGroupResponse")]
        int UpdateHeliconApeGroup(int siteItemId, SolidCP.Providers.Web.WebGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/UpdateHeliconApeGroupResponse")]
        System.Threading.Tasks.Task<int> UpdateHeliconApeGroupAsync(int siteItemId, SolidCP.Providers.Web.WebGroup group);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeGroupResponse")]
        int DeleteHeliconApeGroup(int siteItemId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeGroup", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteHeliconApeGroupResponse")]
        System.Threading.Tasks.Task<int> DeleteHeliconApeGroupAsync(int siteItemId, string groupName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetZooApplications", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetZooApplicationsResponse")]
        SolidCP.Providers.Web.WebAppVirtualDirectory[] /*List*/ GetZooApplications(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetZooApplications", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetZooApplicationsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetZooApplicationsAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooEnvironmentVariable", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooEnvironmentVariableResponse")]
        SolidCP.Providers.ResultObjects.StringResultObject SetZooEnvironmentVariable(int siteItemId, string appName, string envName, string envValue);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooEnvironmentVariable", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooEnvironmentVariableResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooEnvironmentVariableAsync(int siteItemId, string appName, string envName, string envValue);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooConsoleEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooConsoleEnabledResponse")]
        SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleEnabled(int siteItemId, string appName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooConsoleEnabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooConsoleEnabledResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleEnabledAsync(int siteItemId, string appName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooConsoleDisabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooConsoleDisabledResponse")]
        SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleDisabled(int siteItemId, string appName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooConsoleDisabled", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/SetZooConsoleDisabledResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleDisabledAsync(int siteItemId, string appName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GrantWebManagementAccess", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GrantWebManagementAccessResponse")]
        SolidCP.Providers.Common.ResultObject GrantWebManagementAccess(int siteItemId, string accountName, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GrantWebManagementAccess", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GrantWebManagementAccessResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> GrantWebManagementAccessAsync(int siteItemId, string accountName, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RevokeWebManagementAccess", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RevokeWebManagementAccessResponse")]
        void RevokeWebManagementAccess(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RevokeWebManagementAccess", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/RevokeWebManagementAccessResponse")]
        System.Threading.Tasks.Task RevokeWebManagementAccessAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeWebManagementAccessPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeWebManagementAccessPasswordResponse")]
        SolidCP.Providers.Common.ResultObject ChangeWebManagementAccessPassword(int siteItemId, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeWebManagementAccessPassword", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ChangeWebManagementAccessPasswordResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeWebManagementAccessPasswordAsync(int siteItemId, string accountPassword);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CertificateRequest", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CertificateRequestResponse")]
        SolidCP.Providers.Web.SSLCertificate CertificateRequest(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CertificateRequest", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CertificateRequestResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> CertificateRequestAsync(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallCertificateResponse")]
        SolidCP.Providers.Common.ResultObject InstallCertificate(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallCertificateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InstallCertificateAsync(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/LEInstallCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/LEInstallCertificateResponse")]
        SolidCP.Providers.Common.ResultObject LEInstallCertificate(int siteItemId, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/LEInstallCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/LEInstallCertificateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> LEInstallCertificateAsync(int siteItemId, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallPfx", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallPfxResponse")]
        SolidCP.Providers.Common.ResultObject InstallPfx(byte[] certificate, int siteItemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallPfx", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/InstallPfxResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InstallPfxAsync(byte[] certificate, int siteItemId, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetPendingCertificates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetPendingCertificatesResponse")]
        SolidCP.Providers.Web.SSLCertificate[] /*List*/ GetPendingCertificates(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetPendingCertificates", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetPendingCertificatesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate[]> GetPendingCertificatesAsync(int siteItemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSSLCertificateByID", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSSLCertificateByIDResponse")]
        SolidCP.Providers.Web.SSLCertificate GetSSLCertificateByID(int Id);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSSLCertificateByID", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSSLCertificateByIDResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> GetSSLCertificateByIDAsync(int Id);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSiteCert", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSiteCertResponse")]
        SolidCP.Providers.Web.SSLCertificate GetSiteCert(int siteID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSiteCert", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetSiteCertResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> GetSiteCertAsync(int siteID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckSSLForWebsite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckSSLForWebsiteResponse")]
        int CheckSSLForWebsite(int siteID, bool renewal);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckSSLForWebsite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckSSLForWebsiteResponse")]
        System.Threading.Tasks.Task<int> CheckSSLForWebsiteAsync(int siteID, bool renewal);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckSSLForDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckSSLForDomainResponse")]
        SolidCP.Providers.Common.ResultObject CheckSSLForDomain(string domain, int siteID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckSSLForDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckSSLForDomainResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CheckSSLForDomainAsync(string domain, int siteID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ExportCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ExportCertificateResponse")]
        byte[] ExportCertificate(int siteId, string serialNumber, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ExportCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ExportCertificateResponse")]
        System.Threading.Tasks.Task<byte[]> ExportCertificateAsync(int siteId, string serialNumber, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetCertificatesForSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetCertificatesForSiteResponse")]
        SolidCP.Providers.Web.SSLCertificate[] /*List*/ GetCertificatesForSite(int siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetCertificatesForSite", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/GetCertificatesForSiteResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate[]> GetCertificatesForSiteAsync(int siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteCertificateResponse")]
        SolidCP.Providers.Common.ResultObject DeleteCertificate(int siteId, SolidCP.Providers.Web.SSLCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteCertificateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCertificateAsync(int siteId, SolidCP.Providers.Web.SSLCertificate certificate);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ImportCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ImportCertificateResponse")]
        SolidCP.Providers.Common.ResultObject ImportCertificate(int siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ImportCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/ImportCertificateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ImportCertificateAsync(int siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckCertificateResponse")]
        SolidCP.Providers.Common.ResultObject CheckCertificate(int siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckCertificate", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/CheckCertificateResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CheckCertificateAsync(int siteId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteCertificateRequest", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteCertificateRequestResponse")]
        SolidCP.Providers.Common.ResultObject DeleteCertificateRequest(int siteId, int csrID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteCertificateRequest", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesWebServers/DeleteCertificateRequestResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCertificateRequestAsync(int siteId, int csrID);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esWebServersAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesWebServers
    {
        public System.Data.DataSet GetRawWebSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esWebServers", "GetRawWebSitesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawWebSitesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esWebServers", "GetRawWebSitesPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Web.WebSite[] /*List*/ GetWebSites(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.Web.WebSite[], SolidCP.Providers.Web.WebSite>("SolidCP.EnterpriseServer.esWebServers", "GetWebSites", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebSite[]> GetWebSitesAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebSite[], SolidCP.Providers.Web.WebSite>("SolidCP.EnterpriseServer.esWebServers", "GetWebSites", packageId, recursive);
        }

        public SolidCP.Providers.Web.WebSite GetWebSite(int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.WebSite>("SolidCP.EnterpriseServer.esWebServers", "GetWebSite", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebSite> GetWebSiteAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebSite>("SolidCP.EnterpriseServer.esWebServers", "GetWebSite", siteItemId);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetWebSitePointers(int siteItemId)
        {
            return Invoke<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esWebServers", "GetWebSitePointers", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetWebSitePointersAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.DomainInfo[], SolidCP.EnterpriseServer.DomainInfo>("SolidCP.EnterpriseServer.esWebServers", "GetWebSitePointers", siteItemId);
        }

        public int AddWebSitePointer(int siteItemId, string hostName, int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "AddWebSitePointer", siteItemId, hostName, domainId);
        }

        public async System.Threading.Tasks.Task<int> AddWebSitePointerAsync(int siteItemId, string hostName, int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "AddWebSitePointer", siteItemId, hostName, domainId);
        }

        public int DeleteWebSitePointer(int siteItemId, int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteWebSitePointer", siteItemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteWebSitePointerAsync(int siteItemId, int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteWebSitePointer", siteItemId, domainId);
        }

        public int AddWebSite(int packageId, string hostName, int domainId, int ipAddressId, bool ignoreGlobalDNSZone)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "AddWebSite", packageId, hostName, domainId, ipAddressId, ignoreGlobalDNSZone);
        }

        public async System.Threading.Tasks.Task<int> AddWebSiteAsync(int packageId, string hostName, int domainId, int ipAddressId, bool ignoreGlobalDNSZone)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "AddWebSite", packageId, hostName, domainId, ipAddressId, ignoreGlobalDNSZone);
        }

        public int UpdateWebSite(SolidCP.Providers.Web.WebSite site)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateWebSite", site);
        }

        public async System.Threading.Tasks.Task<int> UpdateWebSiteAsync(SolidCP.Providers.Web.WebSite site)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateWebSite", site);
        }

        public int InstallFrontPage(int siteItemId, string username, string password)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "InstallFrontPage", siteItemId, username, password);
        }

        public async System.Threading.Tasks.Task<int> InstallFrontPageAsync(int siteItemId, string username, string password)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "InstallFrontPage", siteItemId, username, password);
        }

        public int UninstallFrontPage(int siteItemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UninstallFrontPage", siteItemId);
        }

        public async System.Threading.Tasks.Task<int> UninstallFrontPageAsync(int siteItemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UninstallFrontPage", siteItemId);
        }

        public int ChangeFrontPagePassword(int siteItemId, string password)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "ChangeFrontPagePassword", siteItemId, password);
        }

        public async System.Threading.Tasks.Task<int> ChangeFrontPagePasswordAsync(int siteItemId, string password)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "ChangeFrontPagePassword", siteItemId, password);
        }

        public int RepairWebSite(int siteItemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "RepairWebSite", siteItemId);
        }

        public async System.Threading.Tasks.Task<int> RepairWebSiteAsync(int siteItemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "RepairWebSite", siteItemId);
        }

        public int DeleteWebSite(int siteItemId, bool deleteWebsiteDirectory)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteWebSite", siteItemId, deleteWebsiteDirectory);
        }

        public async System.Threading.Tasks.Task<int> DeleteWebSiteAsync(int siteItemId, bool deleteWebsiteDirectory)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteWebSite", siteItemId, deleteWebsiteDirectory);
        }

        public int SwitchWebSiteToDedicatedIP(int siteItemId, int ipAddressId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "SwitchWebSiteToDedicatedIP", siteItemId, ipAddressId);
        }

        public async System.Threading.Tasks.Task<int> SwitchWebSiteToDedicatedIPAsync(int siteItemId, int ipAddressId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "SwitchWebSiteToDedicatedIP", siteItemId, ipAddressId);
        }

        public int SwitchWebSiteToSharedIP(int siteItemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "SwitchWebSiteToSharedIP", siteItemId);
        }

        public async System.Threading.Tasks.Task<int> SwitchWebSiteToSharedIPAsync(int siteItemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "SwitchWebSiteToSharedIP", siteItemId);
        }

        public int AddVirtualDirectory(int siteItemId, string vdirName, string vdirPath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "AddVirtualDirectory", siteItemId, vdirName, vdirPath);
        }

        public async System.Threading.Tasks.Task<int> AddVirtualDirectoryAsync(int siteItemId, string vdirName, string vdirPath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "AddVirtualDirectory", siteItemId, vdirName, vdirPath);
        }

        public SolidCP.Providers.Web.WebVirtualDirectory[] /*List*/ GetVirtualDirectories(int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.WebVirtualDirectory[], SolidCP.Providers.Web.WebVirtualDirectory>("SolidCP.EnterpriseServer.esWebServers", "GetVirtualDirectories", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory[]> GetVirtualDirectoriesAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebVirtualDirectory[], SolidCP.Providers.Web.WebVirtualDirectory>("SolidCP.EnterpriseServer.esWebServers", "GetVirtualDirectories", siteItemId);
        }

        public SolidCP.Providers.Web.WebVirtualDirectory GetVirtualDirectory(int siteItemId, string vdirName)
        {
            return Invoke<SolidCP.Providers.Web.WebVirtualDirectory>("SolidCP.EnterpriseServer.esWebServers", "GetVirtualDirectory", siteItemId, vdirName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory> GetVirtualDirectoryAsync(int siteItemId, string vdirName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebVirtualDirectory>("SolidCP.EnterpriseServer.esWebServers", "GetVirtualDirectory", siteItemId, vdirName);
        }

        public int UpdateVirtualDirectory(int siteItemId, SolidCP.Providers.Web.WebVirtualDirectory vdir)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateVirtualDirectory", siteItemId, vdir);
        }

        public async System.Threading.Tasks.Task<int> UpdateVirtualDirectoryAsync(int siteItemId, SolidCP.Providers.Web.WebVirtualDirectory vdir)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateVirtualDirectory", siteItemId, vdir);
        }

        public int DeleteVirtualDirectory(int siteItemId, string vdirName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteVirtualDirectory", siteItemId, vdirName);
        }

        public async System.Threading.Tasks.Task<int> DeleteVirtualDirectoryAsync(int siteItemId, string vdirName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteVirtualDirectory", siteItemId, vdirName);
        }

        public int AddAppVirtualDirectory(int siteItemId, string vdirName, string vdirPath, string aspNetVersion)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "AddAppVirtualDirectory", siteItemId, vdirName, vdirPath, aspNetVersion);
        }

        public async System.Threading.Tasks.Task<int> AddAppVirtualDirectoryAsync(int siteItemId, string vdirName, string vdirPath, string aspNetVersion)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "AddAppVirtualDirectory", siteItemId, vdirName, vdirPath, aspNetVersion);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory[] /*List*/ GetAppVirtualDirectories(int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.WebAppVirtualDirectory[], SolidCP.Providers.Web.WebAppVirtualDirectory>("SolidCP.EnterpriseServer.esWebServers", "GetAppVirtualDirectories", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetAppVirtualDirectoriesAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebAppVirtualDirectory[], SolidCP.Providers.Web.WebAppVirtualDirectory>("SolidCP.EnterpriseServer.esWebServers", "GetAppVirtualDirectories", siteItemId);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory GetAppVirtualDirectory(int siteItemId, string vdirName)
        {
            return Invoke<SolidCP.Providers.Web.WebAppVirtualDirectory>("SolidCP.EnterpriseServer.esWebServers", "GetAppVirtualDirectory", siteItemId, vdirName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory> GetAppVirtualDirectoryAsync(int siteItemId, string vdirName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebAppVirtualDirectory>("SolidCP.EnterpriseServer.esWebServers", "GetAppVirtualDirectory", siteItemId, vdirName);
        }

        public int UpdateAppVirtualDirectory(int siteItemId, SolidCP.Providers.Web.WebAppVirtualDirectory vdir)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateAppVirtualDirectory", siteItemId, vdir);
        }

        public async System.Threading.Tasks.Task<int> UpdateAppVirtualDirectoryAsync(int siteItemId, SolidCP.Providers.Web.WebAppVirtualDirectory vdir)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateAppVirtualDirectory", siteItemId, vdir);
        }

        public int DeleteAppVirtualDirectory(int siteItemId, string vdirName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteAppVirtualDirectory", siteItemId, vdirName);
        }

        public async System.Threading.Tasks.Task<int> DeleteAppVirtualDirectoryAsync(int siteItemId, string vdirName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteAppVirtualDirectory", siteItemId, vdirName);
        }

        public int ChangeSiteState(int siteItemId, SolidCP.Providers.ServerState state)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "ChangeSiteState", siteItemId, state);
        }

        public async System.Threading.Tasks.Task<int> ChangeSiteStateAsync(int siteItemId, SolidCP.Providers.ServerState state)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "ChangeSiteState", siteItemId, state);
        }

        public int ChangeAppPoolState(int siteItemId, SolidCP.Providers.AppPoolState state)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "ChangeAppPoolState", siteItemId, state);
        }

        public async System.Threading.Tasks.Task<int> ChangeAppPoolStateAsync(int siteItemId, SolidCP.Providers.AppPoolState state)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "ChangeAppPoolState", siteItemId, state);
        }

        public SolidCP.Providers.AppPoolState GetAppPoolState(int siteItemId)
        {
            return Invoke<SolidCP.Providers.AppPoolState>("SolidCP.EnterpriseServer.esWebServers", "GetAppPoolState", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.AppPoolState> GetAppPoolStateAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.AppPoolState>("SolidCP.EnterpriseServer.esWebServers", "GetAppPoolState", siteItemId);
        }

        public SolidCP.Providers.ServerState GetSiteState(int siteItemId)
        {
            return Invoke<SolidCP.Providers.ServerState>("SolidCP.EnterpriseServer.esWebServers", "GetSiteState", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ServerState> GetSiteStateAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.ServerState>("SolidCP.EnterpriseServer.esWebServers", "GetSiteState", siteItemId);
        }

        public string[] /*List*/ GetSharedSSLDomains(int packageId)
        {
            return Invoke<string[], string>("SolidCP.EnterpriseServer.esWebServers", "GetSharedSSLDomains", packageId);
        }

        public async System.Threading.Tasks.Task<string[]> GetSharedSSLDomainsAsync(int packageId)
        {
            return await InvokeAsync<string[], string>("SolidCP.EnterpriseServer.esWebServers", "GetSharedSSLDomains", packageId);
        }

        public System.Data.DataSet GetRawSSLFoldersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esWebServers", "GetRawSSLFoldersPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSSLFoldersPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esWebServers", "GetRawSSLFoldersPaged", packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Web.SharedSSLFolder[] /*List*/ GetSharedSSLFolders(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.Web.SharedSSLFolder[], SolidCP.Providers.Web.SharedSSLFolder>("SolidCP.EnterpriseServer.esWebServers", "GetSharedSSLFolders", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SharedSSLFolder[]> GetSharedSSLFoldersAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SharedSSLFolder[], SolidCP.Providers.Web.SharedSSLFolder>("SolidCP.EnterpriseServer.esWebServers", "GetSharedSSLFolders", packageId, recursive);
        }

        public SolidCP.Providers.Web.SharedSSLFolder GetSharedSSLFolder(int itemId)
        {
            return Invoke<SolidCP.Providers.Web.SharedSSLFolder>("SolidCP.EnterpriseServer.esWebServers", "GetSharedSSLFolder", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SharedSSLFolder> GetSharedSSLFolderAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SharedSSLFolder>("SolidCP.EnterpriseServer.esWebServers", "GetSharedSSLFolder", itemId);
        }

        public int AddSharedSSLFolder(int packageId, string sslDomain, int siteId, string vdirName, string vdirPath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "AddSharedSSLFolder", packageId, sslDomain, siteId, vdirName, vdirPath);
        }

        public async System.Threading.Tasks.Task<int> AddSharedSSLFolderAsync(int packageId, string sslDomain, int siteId, string vdirName, string vdirPath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "AddSharedSSLFolder", packageId, sslDomain, siteId, vdirName, vdirPath);
        }

        public int UpdateSharedSSLFolder(SolidCP.Providers.Web.SharedSSLFolder vdir)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateSharedSSLFolder", vdir);
        }

        public async System.Threading.Tasks.Task<int> UpdateSharedSSLFolderAsync(SolidCP.Providers.Web.SharedSSLFolder vdir)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateSharedSSLFolder", vdir);
        }

        public int DeleteSharedSSLFolder(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteSharedSSLFolder", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSharedSSLFolderAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteSharedSSLFolder", itemId);
        }

        public int InstallSecuredFolders(int siteItemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "InstallSecuredFolders", siteItemId);
        }

        public async System.Threading.Tasks.Task<int> InstallSecuredFoldersAsync(int siteItemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "InstallSecuredFolders", siteItemId);
        }

        public int UninstallSecuredFolders(int siteItemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UninstallSecuredFolders", siteItemId);
        }

        public async System.Threading.Tasks.Task<int> UninstallSecuredFoldersAsync(int siteItemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UninstallSecuredFolders", siteItemId);
        }

        public SolidCP.Providers.Web.WebFolder[] GetSecuredFolders(int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.WebFolder[]>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredFolders", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder[]> GetSecuredFoldersAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebFolder[]>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredFolders", siteItemId);
        }

        public SolidCP.Providers.Web.WebFolder GetSecuredFolder(int siteItemId, string folderPath)
        {
            return Invoke<SolidCP.Providers.Web.WebFolder>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredFolder", siteItemId, folderPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder> GetSecuredFolderAsync(int siteItemId, string folderPath)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebFolder>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredFolder", siteItemId, folderPath);
        }

        public int UpdateSecuredFolder(int siteItemId, SolidCP.Providers.Web.WebFolder folder)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateSecuredFolder", siteItemId, folder);
        }

        public async System.Threading.Tasks.Task<int> UpdateSecuredFolderAsync(int siteItemId, SolidCP.Providers.Web.WebFolder folder)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateSecuredFolder", siteItemId, folder);
        }

        public int DeleteSecuredFolder(int siteItemId, string folderPath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteSecuredFolder", siteItemId, folderPath);
        }

        public async System.Threading.Tasks.Task<int> DeleteSecuredFolderAsync(int siteItemId, string folderPath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteSecuredFolder", siteItemId, folderPath);
        }

        public SolidCP.Providers.Web.WebUser[] GetSecuredUsers(int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.WebUser[]>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredUsers", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser[]> GetSecuredUsersAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebUser[]>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredUsers", siteItemId);
        }

        public SolidCP.Providers.Web.WebUser GetSecuredUser(int siteItemId, string userName)
        {
            return Invoke<SolidCP.Providers.Web.WebUser>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredUser", siteItemId, userName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser> GetSecuredUserAsync(int siteItemId, string userName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebUser>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredUser", siteItemId, userName);
        }

        public int UpdateSecuredUser(int siteItemId, SolidCP.Providers.Web.WebUser user)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateSecuredUser", siteItemId, user);
        }

        public async System.Threading.Tasks.Task<int> UpdateSecuredUserAsync(int siteItemId, SolidCP.Providers.Web.WebUser user)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateSecuredUser", siteItemId, user);
        }

        public int DeleteSecuredUser(int siteItemId, string userName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteSecuredUser", siteItemId, userName);
        }

        public async System.Threading.Tasks.Task<int> DeleteSecuredUserAsync(int siteItemId, string userName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteSecuredUser", siteItemId, userName);
        }

        public SolidCP.Providers.Web.WebGroup[] GetSecuredGroups(int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.WebGroup[]>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredGroups", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetSecuredGroupsAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebGroup[]>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredGroups", siteItemId);
        }

        public SolidCP.Providers.Web.WebGroup GetSecuredGroup(int siteItemId, string groupName)
        {
            return Invoke<SolidCP.Providers.Web.WebGroup>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredGroup", siteItemId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetSecuredGroupAsync(int siteItemId, string groupName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebGroup>("SolidCP.EnterpriseServer.esWebServers", "GetSecuredGroup", siteItemId, groupName);
        }

        public int UpdateSecuredGroup(int siteItemId, SolidCP.Providers.Web.WebGroup group)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateSecuredGroup", siteItemId, group);
        }

        public async System.Threading.Tasks.Task<int> UpdateSecuredGroupAsync(int siteItemId, SolidCP.Providers.Web.WebGroup group)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateSecuredGroup", siteItemId, group);
        }

        public int DeleteSecuredGroup(int siteItemId, string groupName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteSecuredGroup", siteItemId, groupName);
        }

        public async System.Threading.Tasks.Task<int> DeleteSecuredGroupAsync(int siteItemId, string groupName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteSecuredGroup", siteItemId, groupName);
        }

        public SolidCP.Providers.Common.ResultObject GrantWebDeployPublishingAccess(int siteItemId, string accountName, string accountPassword)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "GrantWebDeployPublishingAccess", siteItemId, accountName, accountPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> GrantWebDeployPublishingAccessAsync(int siteItemId, string accountName, string accountPassword)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "GrantWebDeployPublishingAccess", siteItemId, accountName, accountPassword);
        }

        public SolidCP.Providers.Common.ResultObject SaveWebDeployPublishingProfile(int siteItemId, int[] serviceItemIds)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "SaveWebDeployPublishingProfile", siteItemId, serviceItemIds);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SaveWebDeployPublishingProfileAsync(int siteItemId, int[] serviceItemIds)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "SaveWebDeployPublishingProfile", siteItemId, serviceItemIds);
        }

        public void RevokeWebDeployPublishingAccess(int siteItemId)
        {
            Invoke("SolidCP.EnterpriseServer.esWebServers", "RevokeWebDeployPublishingAccess", siteItemId);
        }

        public async System.Threading.Tasks.Task RevokeWebDeployPublishingAccessAsync(int siteItemId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esWebServers", "RevokeWebDeployPublishingAccess", siteItemId);
        }

        public SolidCP.Providers.ResultObjects.BytesResult GetWebDeployPublishingProfile(int siteItemId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.BytesResult>("SolidCP.EnterpriseServer.esWebServers", "GetWebDeployPublishingProfile", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BytesResult> GetWebDeployPublishingProfileAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.BytesResult>("SolidCP.EnterpriseServer.esWebServers", "GetWebDeployPublishingProfile", siteItemId);
        }

        public SolidCP.Providers.Common.ResultObject ChangeWebDeployPublishingPassword(int siteItemId, string newAccountPassword)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "ChangeWebDeployPublishingPassword", siteItemId, newAccountPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeWebDeployPublishingPasswordAsync(int siteItemId, string newAccountPassword)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "ChangeWebDeployPublishingPassword", siteItemId, newAccountPassword);
        }

        public SolidCP.Providers.ResultObjects.HeliconApeStatus GetHeliconApeStatus(int siteItemId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.HeliconApeStatus>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeStatus", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.HeliconApeStatus> GetHeliconApeStatusAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.HeliconApeStatus>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeStatus", siteItemId);
        }

        public void InstallHeliconApe(int siteItemId)
        {
            Invoke("SolidCP.EnterpriseServer.esWebServers", "InstallHeliconApe", siteItemId);
        }

        public async System.Threading.Tasks.Task InstallHeliconApeAsync(int siteItemId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esWebServers", "InstallHeliconApe", siteItemId);
        }

        public int EnableHeliconApe(int siteItemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "EnableHeliconApe", siteItemId);
        }

        public async System.Threading.Tasks.Task<int> EnableHeliconApeAsync(int siteItemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "EnableHeliconApe", siteItemId);
        }

        public int DisableHeliconApe(int siteItemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DisableHeliconApe", siteItemId);
        }

        public async System.Threading.Tasks.Task<int> DisableHeliconApeAsync(int siteItemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DisableHeliconApe", siteItemId);
        }

        public int EnableHeliconApeGlobally(int serviceId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "EnableHeliconApeGlobally", serviceId);
        }

        public async System.Threading.Tasks.Task<int> EnableHeliconApeGloballyAsync(int serviceId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "EnableHeliconApeGlobally", serviceId);
        }

        public int DisableHeliconApeGlobally(int serviceId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DisableHeliconApeGlobally", serviceId);
        }

        public async System.Threading.Tasks.Task<int> DisableHeliconApeGloballyAsync(int serviceId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DisableHeliconApeGlobally", serviceId);
        }

        public SolidCP.Providers.Web.HtaccessFolder[] GetHeliconApeFolders(int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.HtaccessFolder[]>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeFolders", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder[]> GetHeliconApeFoldersAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.HtaccessFolder[]>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeFolders", siteItemId);
        }

        public SolidCP.Providers.Web.HtaccessFolder GetHeliconApeHttpdFolder(int serviceId)
        {
            return Invoke<SolidCP.Providers.Web.HtaccessFolder>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeHttpdFolder", serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeHttpdFolderAsync(int serviceId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.HtaccessFolder>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeHttpdFolder", serviceId);
        }

        public SolidCP.Providers.Web.HtaccessFolder GetHeliconApeFolder(int siteItemId, string folderPath)
        {
            return Invoke<SolidCP.Providers.Web.HtaccessFolder>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeFolder", siteItemId, folderPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeFolderAsync(int siteItemId, string folderPath)
        {
            return await InvokeAsync<SolidCP.Providers.Web.HtaccessFolder>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeFolder", siteItemId, folderPath);
        }

        public int UpdateHeliconApeFolder(int siteItemId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateHeliconApeFolder", siteItemId, folder);
        }

        public async System.Threading.Tasks.Task<int> UpdateHeliconApeFolderAsync(int siteItemId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateHeliconApeFolder", siteItemId, folder);
        }

        public int UpdateHeliconApeHttpdFolder(int serviceId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateHeliconApeHttpdFolder", serviceId, folder);
        }

        public async System.Threading.Tasks.Task<int> UpdateHeliconApeHttpdFolderAsync(int serviceId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateHeliconApeHttpdFolder", serviceId, folder);
        }

        public int DeleteHeliconApeFolder(int siteItemId, string folderPath)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteHeliconApeFolder", siteItemId, folderPath);
        }

        public async System.Threading.Tasks.Task<int> DeleteHeliconApeFolderAsync(int siteItemId, string folderPath)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteHeliconApeFolder", siteItemId, folderPath);
        }

        public SolidCP.Providers.Web.HtaccessUser[] GetHeliconApeUsers(int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.HtaccessUser[]>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeUsers", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser[]> GetHeliconApeUsersAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.HtaccessUser[]>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeUsers", siteItemId);
        }

        public SolidCP.Providers.Web.HtaccessUser GetHeliconApeUser(int siteItemId, string userName)
        {
            return Invoke<SolidCP.Providers.Web.HtaccessUser>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeUser", siteItemId, userName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser> GetHeliconApeUserAsync(int siteItemId, string userName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.HtaccessUser>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeUser", siteItemId, userName);
        }

        public int UpdateHeliconApeUser(int siteItemId, SolidCP.Providers.Web.HtaccessUser user)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateHeliconApeUser", siteItemId, user);
        }

        public async System.Threading.Tasks.Task<int> UpdateHeliconApeUserAsync(int siteItemId, SolidCP.Providers.Web.HtaccessUser user)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateHeliconApeUser", siteItemId, user);
        }

        public int DeleteHeliconApeUser(int siteItemId, string userName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteHeliconApeUser", siteItemId, userName);
        }

        public async System.Threading.Tasks.Task<int> DeleteHeliconApeUserAsync(int siteItemId, string userName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteHeliconApeUser", siteItemId, userName);
        }

        public SolidCP.Providers.Web.WebGroup[] GetHeliconApeGroups(int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.WebGroup[]>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeGroups", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetHeliconApeGroupsAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebGroup[]>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeGroups", siteItemId);
        }

        public SolidCP.Providers.Web.WebGroup GetHeliconApeGroup(int siteItemId, string groupName)
        {
            return Invoke<SolidCP.Providers.Web.WebGroup>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeGroup", siteItemId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetHeliconApeGroupAsync(int siteItemId, string groupName)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebGroup>("SolidCP.EnterpriseServer.esWebServers", "GetHeliconApeGroup", siteItemId, groupName);
        }

        public int UpdateHeliconApeGroup(int siteItemId, SolidCP.Providers.Web.WebGroup group)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateHeliconApeGroup", siteItemId, group);
        }

        public async System.Threading.Tasks.Task<int> UpdateHeliconApeGroupAsync(int siteItemId, SolidCP.Providers.Web.WebGroup group)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "UpdateHeliconApeGroup", siteItemId, group);
        }

        public int DeleteHeliconApeGroup(int siteItemId, string groupName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteHeliconApeGroup", siteItemId, groupName);
        }

        public async System.Threading.Tasks.Task<int> DeleteHeliconApeGroupAsync(int siteItemId, string groupName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "DeleteHeliconApeGroup", siteItemId, groupName);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory[] /*List*/ GetZooApplications(int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.WebAppVirtualDirectory[], SolidCP.Providers.Web.WebAppVirtualDirectory>("SolidCP.EnterpriseServer.esWebServers", "GetZooApplications", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetZooApplicationsAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebAppVirtualDirectory[], SolidCP.Providers.Web.WebAppVirtualDirectory>("SolidCP.EnterpriseServer.esWebServers", "GetZooApplications", siteItemId);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooEnvironmentVariable(int siteItemId, string appName, string envName, string envValue)
        {
            return Invoke<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.EnterpriseServer.esWebServers", "SetZooEnvironmentVariable", siteItemId, appName, envName, envValue);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooEnvironmentVariableAsync(int siteItemId, string appName, string envName, string envValue)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.EnterpriseServer.esWebServers", "SetZooEnvironmentVariable", siteItemId, appName, envName, envValue);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleEnabled(int siteItemId, string appName)
        {
            return Invoke<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.EnterpriseServer.esWebServers", "SetZooConsoleEnabled", siteItemId, appName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleEnabledAsync(int siteItemId, string appName)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.EnterpriseServer.esWebServers", "SetZooConsoleEnabled", siteItemId, appName);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleDisabled(int siteItemId, string appName)
        {
            return Invoke<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.EnterpriseServer.esWebServers", "SetZooConsoleDisabled", siteItemId, appName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleDisabledAsync(int siteItemId, string appName)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.StringResultObject>("SolidCP.EnterpriseServer.esWebServers", "SetZooConsoleDisabled", siteItemId, appName);
        }

        public SolidCP.Providers.Common.ResultObject GrantWebManagementAccess(int siteItemId, string accountName, string accountPassword)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "GrantWebManagementAccess", siteItemId, accountName, accountPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> GrantWebManagementAccessAsync(int siteItemId, string accountName, string accountPassword)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "GrantWebManagementAccess", siteItemId, accountName, accountPassword);
        }

        public void RevokeWebManagementAccess(int siteItemId)
        {
            Invoke("SolidCP.EnterpriseServer.esWebServers", "RevokeWebManagementAccess", siteItemId);
        }

        public async System.Threading.Tasks.Task RevokeWebManagementAccessAsync(int siteItemId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esWebServers", "RevokeWebManagementAccess", siteItemId);
        }

        public SolidCP.Providers.Common.ResultObject ChangeWebManagementAccessPassword(int siteItemId, string accountPassword)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "ChangeWebManagementAccessPassword", siteItemId, accountPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeWebManagementAccessPasswordAsync(int siteItemId, string accountPassword)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "ChangeWebManagementAccessPassword", siteItemId, accountPassword);
        }

        public SolidCP.Providers.Web.SSLCertificate CertificateRequest(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate>("SolidCP.EnterpriseServer.esWebServers", "CertificateRequest", certificate, siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> CertificateRequestAsync(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate>("SolidCP.EnterpriseServer.esWebServers", "CertificateRequest", certificate, siteItemId);
        }

        public SolidCP.Providers.Common.ResultObject InstallCertificate(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "InstallCertificate", certificate, siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InstallCertificateAsync(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "InstallCertificate", certificate, siteItemId);
        }

        public SolidCP.Providers.Common.ResultObject LEInstallCertificate(int siteItemId, string email)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "LEInstallCertificate", siteItemId, email);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> LEInstallCertificateAsync(int siteItemId, string email)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "LEInstallCertificate", siteItemId, email);
        }

        public SolidCP.Providers.Common.ResultObject InstallPfx(byte[] certificate, int siteItemId, string password)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "InstallPfx", certificate, siteItemId, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InstallPfxAsync(byte[] certificate, int siteItemId, string password)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "InstallPfx", certificate, siteItemId, password);
        }

        public SolidCP.Providers.Web.SSLCertificate[] /*List*/ GetPendingCertificates(int siteItemId)
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate[], SolidCP.Providers.Web.SSLCertificate>("SolidCP.EnterpriseServer.esWebServers", "GetPendingCertificates", siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate[]> GetPendingCertificatesAsync(int siteItemId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate[], SolidCP.Providers.Web.SSLCertificate>("SolidCP.EnterpriseServer.esWebServers", "GetPendingCertificates", siteItemId);
        }

        public SolidCP.Providers.Web.SSLCertificate GetSSLCertificateByID(int Id)
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate>("SolidCP.EnterpriseServer.esWebServers", "GetSSLCertificateByID", Id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> GetSSLCertificateByIDAsync(int Id)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate>("SolidCP.EnterpriseServer.esWebServers", "GetSSLCertificateByID", Id);
        }

        public SolidCP.Providers.Web.SSLCertificate GetSiteCert(int siteID)
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate>("SolidCP.EnterpriseServer.esWebServers", "GetSiteCert", siteID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> GetSiteCertAsync(int siteID)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate>("SolidCP.EnterpriseServer.esWebServers", "GetSiteCert", siteID);
        }

        public int CheckSSLForWebsite(int siteID, bool renewal)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esWebServers", "CheckSSLForWebsite", siteID, renewal);
        }

        public async System.Threading.Tasks.Task<int> CheckSSLForWebsiteAsync(int siteID, bool renewal)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esWebServers", "CheckSSLForWebsite", siteID, renewal);
        }

        public SolidCP.Providers.Common.ResultObject CheckSSLForDomain(string domain, int siteID)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "CheckSSLForDomain", domain, siteID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CheckSSLForDomainAsync(string domain, int siteID)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "CheckSSLForDomain", domain, siteID);
        }

        public byte[] ExportCertificate(int siteId, string serialNumber, string password)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esWebServers", "ExportCertificate", siteId, serialNumber, password);
        }

        public async System.Threading.Tasks.Task<byte[]> ExportCertificateAsync(int siteId, string serialNumber, string password)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esWebServers", "ExportCertificate", siteId, serialNumber, password);
        }

        public SolidCP.Providers.Web.SSLCertificate[] /*List*/ GetCertificatesForSite(int siteId)
        {
            return Invoke<SolidCP.Providers.Web.SSLCertificate[], SolidCP.Providers.Web.SSLCertificate>("SolidCP.EnterpriseServer.esWebServers", "GetCertificatesForSite", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate[]> GetCertificatesForSiteAsync(int siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Web.SSLCertificate[], SolidCP.Providers.Web.SSLCertificate>("SolidCP.EnterpriseServer.esWebServers", "GetCertificatesForSite", siteId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteCertificate(int siteId, SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "DeleteCertificate", siteId, certificate);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCertificateAsync(int siteId, SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "DeleteCertificate", siteId, certificate);
        }

        public SolidCP.Providers.Common.ResultObject ImportCertificate(int siteId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "ImportCertificate", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ImportCertificateAsync(int siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "ImportCertificate", siteId);
        }

        public SolidCP.Providers.Common.ResultObject CheckCertificate(int siteId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "CheckCertificate", siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CheckCertificateAsync(int siteId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "CheckCertificate", siteId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteCertificateRequest(int siteId, int csrID)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "DeleteCertificateRequest", siteId, csrID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCertificateRequestAsync(int siteId, int csrID)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esWebServers", "DeleteCertificateRequest", siteId, csrID);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esWebServers : SolidCP.Web.Client.ClientBase<IesWebServers, esWebServersAssemblyClient>, IesWebServers
    {
        public System.Data.DataSet GetRawWebSitesPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawWebSitesPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawWebSitesPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawWebSitesPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Web.WebSite[] /*List*/ GetWebSites(int packageId, bool recursive)
        {
            return base.Client.GetWebSites(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebSite[]> GetWebSitesAsync(int packageId, bool recursive)
        {
            return await base.Client.GetWebSitesAsync(packageId, recursive);
        }

        public SolidCP.Providers.Web.WebSite GetWebSite(int siteItemId)
        {
            return base.Client.GetWebSite(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebSite> GetWebSiteAsync(int siteItemId)
        {
            return await base.Client.GetWebSiteAsync(siteItemId);
        }

        public SolidCP.EnterpriseServer.DomainInfo[] /*List*/ GetWebSitePointers(int siteItemId)
        {
            return base.Client.GetWebSitePointers(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.DomainInfo[]> GetWebSitePointersAsync(int siteItemId)
        {
            return await base.Client.GetWebSitePointersAsync(siteItemId);
        }

        public int AddWebSitePointer(int siteItemId, string hostName, int domainId)
        {
            return base.Client.AddWebSitePointer(siteItemId, hostName, domainId);
        }

        public async System.Threading.Tasks.Task<int> AddWebSitePointerAsync(int siteItemId, string hostName, int domainId)
        {
            return await base.Client.AddWebSitePointerAsync(siteItemId, hostName, domainId);
        }

        public int DeleteWebSitePointer(int siteItemId, int domainId)
        {
            return base.Client.DeleteWebSitePointer(siteItemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteWebSitePointerAsync(int siteItemId, int domainId)
        {
            return await base.Client.DeleteWebSitePointerAsync(siteItemId, domainId);
        }

        public int AddWebSite(int packageId, string hostName, int domainId, int ipAddressId, bool ignoreGlobalDNSZone)
        {
            return base.Client.AddWebSite(packageId, hostName, domainId, ipAddressId, ignoreGlobalDNSZone);
        }

        public async System.Threading.Tasks.Task<int> AddWebSiteAsync(int packageId, string hostName, int domainId, int ipAddressId, bool ignoreGlobalDNSZone)
        {
            return await base.Client.AddWebSiteAsync(packageId, hostName, domainId, ipAddressId, ignoreGlobalDNSZone);
        }

        public int UpdateWebSite(SolidCP.Providers.Web.WebSite site)
        {
            return base.Client.UpdateWebSite(site);
        }

        public async System.Threading.Tasks.Task<int> UpdateWebSiteAsync(SolidCP.Providers.Web.WebSite site)
        {
            return await base.Client.UpdateWebSiteAsync(site);
        }

        public int InstallFrontPage(int siteItemId, string username, string password)
        {
            return base.Client.InstallFrontPage(siteItemId, username, password);
        }

        public async System.Threading.Tasks.Task<int> InstallFrontPageAsync(int siteItemId, string username, string password)
        {
            return await base.Client.InstallFrontPageAsync(siteItemId, username, password);
        }

        public int UninstallFrontPage(int siteItemId)
        {
            return base.Client.UninstallFrontPage(siteItemId);
        }

        public async System.Threading.Tasks.Task<int> UninstallFrontPageAsync(int siteItemId)
        {
            return await base.Client.UninstallFrontPageAsync(siteItemId);
        }

        public int ChangeFrontPagePassword(int siteItemId, string password)
        {
            return base.Client.ChangeFrontPagePassword(siteItemId, password);
        }

        public async System.Threading.Tasks.Task<int> ChangeFrontPagePasswordAsync(int siteItemId, string password)
        {
            return await base.Client.ChangeFrontPagePasswordAsync(siteItemId, password);
        }

        public int RepairWebSite(int siteItemId)
        {
            return base.Client.RepairWebSite(siteItemId);
        }

        public async System.Threading.Tasks.Task<int> RepairWebSiteAsync(int siteItemId)
        {
            return await base.Client.RepairWebSiteAsync(siteItemId);
        }

        public int DeleteWebSite(int siteItemId, bool deleteWebsiteDirectory)
        {
            return base.Client.DeleteWebSite(siteItemId, deleteWebsiteDirectory);
        }

        public async System.Threading.Tasks.Task<int> DeleteWebSiteAsync(int siteItemId, bool deleteWebsiteDirectory)
        {
            return await base.Client.DeleteWebSiteAsync(siteItemId, deleteWebsiteDirectory);
        }

        public int SwitchWebSiteToDedicatedIP(int siteItemId, int ipAddressId)
        {
            return base.Client.SwitchWebSiteToDedicatedIP(siteItemId, ipAddressId);
        }

        public async System.Threading.Tasks.Task<int> SwitchWebSiteToDedicatedIPAsync(int siteItemId, int ipAddressId)
        {
            return await base.Client.SwitchWebSiteToDedicatedIPAsync(siteItemId, ipAddressId);
        }

        public int SwitchWebSiteToSharedIP(int siteItemId)
        {
            return base.Client.SwitchWebSiteToSharedIP(siteItemId);
        }

        public async System.Threading.Tasks.Task<int> SwitchWebSiteToSharedIPAsync(int siteItemId)
        {
            return await base.Client.SwitchWebSiteToSharedIPAsync(siteItemId);
        }

        public int AddVirtualDirectory(int siteItemId, string vdirName, string vdirPath)
        {
            return base.Client.AddVirtualDirectory(siteItemId, vdirName, vdirPath);
        }

        public async System.Threading.Tasks.Task<int> AddVirtualDirectoryAsync(int siteItemId, string vdirName, string vdirPath)
        {
            return await base.Client.AddVirtualDirectoryAsync(siteItemId, vdirName, vdirPath);
        }

        public SolidCP.Providers.Web.WebVirtualDirectory[] /*List*/ GetVirtualDirectories(int siteItemId)
        {
            return base.Client.GetVirtualDirectories(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory[]> GetVirtualDirectoriesAsync(int siteItemId)
        {
            return await base.Client.GetVirtualDirectoriesAsync(siteItemId);
        }

        public SolidCP.Providers.Web.WebVirtualDirectory GetVirtualDirectory(int siteItemId, string vdirName)
        {
            return base.Client.GetVirtualDirectory(siteItemId, vdirName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebVirtualDirectory> GetVirtualDirectoryAsync(int siteItemId, string vdirName)
        {
            return await base.Client.GetVirtualDirectoryAsync(siteItemId, vdirName);
        }

        public int UpdateVirtualDirectory(int siteItemId, SolidCP.Providers.Web.WebVirtualDirectory vdir)
        {
            return base.Client.UpdateVirtualDirectory(siteItemId, vdir);
        }

        public async System.Threading.Tasks.Task<int> UpdateVirtualDirectoryAsync(int siteItemId, SolidCP.Providers.Web.WebVirtualDirectory vdir)
        {
            return await base.Client.UpdateVirtualDirectoryAsync(siteItemId, vdir);
        }

        public int DeleteVirtualDirectory(int siteItemId, string vdirName)
        {
            return base.Client.DeleteVirtualDirectory(siteItemId, vdirName);
        }

        public async System.Threading.Tasks.Task<int> DeleteVirtualDirectoryAsync(int siteItemId, string vdirName)
        {
            return await base.Client.DeleteVirtualDirectoryAsync(siteItemId, vdirName);
        }

        public int AddAppVirtualDirectory(int siteItemId, string vdirName, string vdirPath, string aspNetVersion)
        {
            return base.Client.AddAppVirtualDirectory(siteItemId, vdirName, vdirPath, aspNetVersion);
        }

        public async System.Threading.Tasks.Task<int> AddAppVirtualDirectoryAsync(int siteItemId, string vdirName, string vdirPath, string aspNetVersion)
        {
            return await base.Client.AddAppVirtualDirectoryAsync(siteItemId, vdirName, vdirPath, aspNetVersion);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory[] /*List*/ GetAppVirtualDirectories(int siteItemId)
        {
            return base.Client.GetAppVirtualDirectories(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetAppVirtualDirectoriesAsync(int siteItemId)
        {
            return await base.Client.GetAppVirtualDirectoriesAsync(siteItemId);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory GetAppVirtualDirectory(int siteItemId, string vdirName)
        {
            return base.Client.GetAppVirtualDirectory(siteItemId, vdirName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory> GetAppVirtualDirectoryAsync(int siteItemId, string vdirName)
        {
            return await base.Client.GetAppVirtualDirectoryAsync(siteItemId, vdirName);
        }

        public int UpdateAppVirtualDirectory(int siteItemId, SolidCP.Providers.Web.WebAppVirtualDirectory vdir)
        {
            return base.Client.UpdateAppVirtualDirectory(siteItemId, vdir);
        }

        public async System.Threading.Tasks.Task<int> UpdateAppVirtualDirectoryAsync(int siteItemId, SolidCP.Providers.Web.WebAppVirtualDirectory vdir)
        {
            return await base.Client.UpdateAppVirtualDirectoryAsync(siteItemId, vdir);
        }

        public int DeleteAppVirtualDirectory(int siteItemId, string vdirName)
        {
            return base.Client.DeleteAppVirtualDirectory(siteItemId, vdirName);
        }

        public async System.Threading.Tasks.Task<int> DeleteAppVirtualDirectoryAsync(int siteItemId, string vdirName)
        {
            return await base.Client.DeleteAppVirtualDirectoryAsync(siteItemId, vdirName);
        }

        public int ChangeSiteState(int siteItemId, SolidCP.Providers.ServerState state)
        {
            return base.Client.ChangeSiteState(siteItemId, state);
        }

        public async System.Threading.Tasks.Task<int> ChangeSiteStateAsync(int siteItemId, SolidCP.Providers.ServerState state)
        {
            return await base.Client.ChangeSiteStateAsync(siteItemId, state);
        }

        public int ChangeAppPoolState(int siteItemId, SolidCP.Providers.AppPoolState state)
        {
            return base.Client.ChangeAppPoolState(siteItemId, state);
        }

        public async System.Threading.Tasks.Task<int> ChangeAppPoolStateAsync(int siteItemId, SolidCP.Providers.AppPoolState state)
        {
            return await base.Client.ChangeAppPoolStateAsync(siteItemId, state);
        }

        public SolidCP.Providers.AppPoolState GetAppPoolState(int siteItemId)
        {
            return base.Client.GetAppPoolState(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.AppPoolState> GetAppPoolStateAsync(int siteItemId)
        {
            return await base.Client.GetAppPoolStateAsync(siteItemId);
        }

        public SolidCP.Providers.ServerState GetSiteState(int siteItemId)
        {
            return base.Client.GetSiteState(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ServerState> GetSiteStateAsync(int siteItemId)
        {
            return await base.Client.GetSiteStateAsync(siteItemId);
        }

        public string[] /*List*/ GetSharedSSLDomains(int packageId)
        {
            return base.Client.GetSharedSSLDomains(packageId);
        }

        public async System.Threading.Tasks.Task<string[]> GetSharedSSLDomainsAsync(int packageId)
        {
            return await base.Client.GetSharedSSLDomainsAsync(packageId);
        }

        public System.Data.DataSet GetRawSSLFoldersPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawSSLFoldersPaged(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawSSLFoldersPagedAsync(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawSSLFoldersPagedAsync(packageId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.Web.SharedSSLFolder[] /*List*/ GetSharedSSLFolders(int packageId, bool recursive)
        {
            return base.Client.GetSharedSSLFolders(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SharedSSLFolder[]> GetSharedSSLFoldersAsync(int packageId, bool recursive)
        {
            return await base.Client.GetSharedSSLFoldersAsync(packageId, recursive);
        }

        public SolidCP.Providers.Web.SharedSSLFolder GetSharedSSLFolder(int itemId)
        {
            return base.Client.GetSharedSSLFolder(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SharedSSLFolder> GetSharedSSLFolderAsync(int itemId)
        {
            return await base.Client.GetSharedSSLFolderAsync(itemId);
        }

        public int AddSharedSSLFolder(int packageId, string sslDomain, int siteId, string vdirName, string vdirPath)
        {
            return base.Client.AddSharedSSLFolder(packageId, sslDomain, siteId, vdirName, vdirPath);
        }

        public async System.Threading.Tasks.Task<int> AddSharedSSLFolderAsync(int packageId, string sslDomain, int siteId, string vdirName, string vdirPath)
        {
            return await base.Client.AddSharedSSLFolderAsync(packageId, sslDomain, siteId, vdirName, vdirPath);
        }

        public int UpdateSharedSSLFolder(SolidCP.Providers.Web.SharedSSLFolder vdir)
        {
            return base.Client.UpdateSharedSSLFolder(vdir);
        }

        public async System.Threading.Tasks.Task<int> UpdateSharedSSLFolderAsync(SolidCP.Providers.Web.SharedSSLFolder vdir)
        {
            return await base.Client.UpdateSharedSSLFolderAsync(vdir);
        }

        public int DeleteSharedSSLFolder(int itemId)
        {
            return base.Client.DeleteSharedSSLFolder(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSharedSSLFolderAsync(int itemId)
        {
            return await base.Client.DeleteSharedSSLFolderAsync(itemId);
        }

        public int InstallSecuredFolders(int siteItemId)
        {
            return base.Client.InstallSecuredFolders(siteItemId);
        }

        public async System.Threading.Tasks.Task<int> InstallSecuredFoldersAsync(int siteItemId)
        {
            return await base.Client.InstallSecuredFoldersAsync(siteItemId);
        }

        public int UninstallSecuredFolders(int siteItemId)
        {
            return base.Client.UninstallSecuredFolders(siteItemId);
        }

        public async System.Threading.Tasks.Task<int> UninstallSecuredFoldersAsync(int siteItemId)
        {
            return await base.Client.UninstallSecuredFoldersAsync(siteItemId);
        }

        public SolidCP.Providers.Web.WebFolder[] GetSecuredFolders(int siteItemId)
        {
            return base.Client.GetSecuredFolders(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder[]> GetSecuredFoldersAsync(int siteItemId)
        {
            return await base.Client.GetSecuredFoldersAsync(siteItemId);
        }

        public SolidCP.Providers.Web.WebFolder GetSecuredFolder(int siteItemId, string folderPath)
        {
            return base.Client.GetSecuredFolder(siteItemId, folderPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebFolder> GetSecuredFolderAsync(int siteItemId, string folderPath)
        {
            return await base.Client.GetSecuredFolderAsync(siteItemId, folderPath);
        }

        public int UpdateSecuredFolder(int siteItemId, SolidCP.Providers.Web.WebFolder folder)
        {
            return base.Client.UpdateSecuredFolder(siteItemId, folder);
        }

        public async System.Threading.Tasks.Task<int> UpdateSecuredFolderAsync(int siteItemId, SolidCP.Providers.Web.WebFolder folder)
        {
            return await base.Client.UpdateSecuredFolderAsync(siteItemId, folder);
        }

        public int DeleteSecuredFolder(int siteItemId, string folderPath)
        {
            return base.Client.DeleteSecuredFolder(siteItemId, folderPath);
        }

        public async System.Threading.Tasks.Task<int> DeleteSecuredFolderAsync(int siteItemId, string folderPath)
        {
            return await base.Client.DeleteSecuredFolderAsync(siteItemId, folderPath);
        }

        public SolidCP.Providers.Web.WebUser[] GetSecuredUsers(int siteItemId)
        {
            return base.Client.GetSecuredUsers(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser[]> GetSecuredUsersAsync(int siteItemId)
        {
            return await base.Client.GetSecuredUsersAsync(siteItemId);
        }

        public SolidCP.Providers.Web.WebUser GetSecuredUser(int siteItemId, string userName)
        {
            return base.Client.GetSecuredUser(siteItemId, userName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebUser> GetSecuredUserAsync(int siteItemId, string userName)
        {
            return await base.Client.GetSecuredUserAsync(siteItemId, userName);
        }

        public int UpdateSecuredUser(int siteItemId, SolidCP.Providers.Web.WebUser user)
        {
            return base.Client.UpdateSecuredUser(siteItemId, user);
        }

        public async System.Threading.Tasks.Task<int> UpdateSecuredUserAsync(int siteItemId, SolidCP.Providers.Web.WebUser user)
        {
            return await base.Client.UpdateSecuredUserAsync(siteItemId, user);
        }

        public int DeleteSecuredUser(int siteItemId, string userName)
        {
            return base.Client.DeleteSecuredUser(siteItemId, userName);
        }

        public async System.Threading.Tasks.Task<int> DeleteSecuredUserAsync(int siteItemId, string userName)
        {
            return await base.Client.DeleteSecuredUserAsync(siteItemId, userName);
        }

        public SolidCP.Providers.Web.WebGroup[] GetSecuredGroups(int siteItemId)
        {
            return base.Client.GetSecuredGroups(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetSecuredGroupsAsync(int siteItemId)
        {
            return await base.Client.GetSecuredGroupsAsync(siteItemId);
        }

        public SolidCP.Providers.Web.WebGroup GetSecuredGroup(int siteItemId, string groupName)
        {
            return base.Client.GetSecuredGroup(siteItemId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetSecuredGroupAsync(int siteItemId, string groupName)
        {
            return await base.Client.GetSecuredGroupAsync(siteItemId, groupName);
        }

        public int UpdateSecuredGroup(int siteItemId, SolidCP.Providers.Web.WebGroup group)
        {
            return base.Client.UpdateSecuredGroup(siteItemId, group);
        }

        public async System.Threading.Tasks.Task<int> UpdateSecuredGroupAsync(int siteItemId, SolidCP.Providers.Web.WebGroup group)
        {
            return await base.Client.UpdateSecuredGroupAsync(siteItemId, group);
        }

        public int DeleteSecuredGroup(int siteItemId, string groupName)
        {
            return base.Client.DeleteSecuredGroup(siteItemId, groupName);
        }

        public async System.Threading.Tasks.Task<int> DeleteSecuredGroupAsync(int siteItemId, string groupName)
        {
            return await base.Client.DeleteSecuredGroupAsync(siteItemId, groupName);
        }

        public SolidCP.Providers.Common.ResultObject GrantWebDeployPublishingAccess(int siteItemId, string accountName, string accountPassword)
        {
            return base.Client.GrantWebDeployPublishingAccess(siteItemId, accountName, accountPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> GrantWebDeployPublishingAccessAsync(int siteItemId, string accountName, string accountPassword)
        {
            return await base.Client.GrantWebDeployPublishingAccessAsync(siteItemId, accountName, accountPassword);
        }

        public SolidCP.Providers.Common.ResultObject SaveWebDeployPublishingProfile(int siteItemId, int[] serviceItemIds)
        {
            return base.Client.SaveWebDeployPublishingProfile(siteItemId, serviceItemIds);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SaveWebDeployPublishingProfileAsync(int siteItemId, int[] serviceItemIds)
        {
            return await base.Client.SaveWebDeployPublishingProfileAsync(siteItemId, serviceItemIds);
        }

        public void RevokeWebDeployPublishingAccess(int siteItemId)
        {
            base.Client.RevokeWebDeployPublishingAccess(siteItemId);
        }

        public async System.Threading.Tasks.Task RevokeWebDeployPublishingAccessAsync(int siteItemId)
        {
            await base.Client.RevokeWebDeployPublishingAccessAsync(siteItemId);
        }

        public SolidCP.Providers.ResultObjects.BytesResult GetWebDeployPublishingProfile(int siteItemId)
        {
            return base.Client.GetWebDeployPublishingProfile(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BytesResult> GetWebDeployPublishingProfileAsync(int siteItemId)
        {
            return await base.Client.GetWebDeployPublishingProfileAsync(siteItemId);
        }

        public SolidCP.Providers.Common.ResultObject ChangeWebDeployPublishingPassword(int siteItemId, string newAccountPassword)
        {
            return base.Client.ChangeWebDeployPublishingPassword(siteItemId, newAccountPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeWebDeployPublishingPasswordAsync(int siteItemId, string newAccountPassword)
        {
            return await base.Client.ChangeWebDeployPublishingPasswordAsync(siteItemId, newAccountPassword);
        }

        public SolidCP.Providers.ResultObjects.HeliconApeStatus GetHeliconApeStatus(int siteItemId)
        {
            return base.Client.GetHeliconApeStatus(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.HeliconApeStatus> GetHeliconApeStatusAsync(int siteItemId)
        {
            return await base.Client.GetHeliconApeStatusAsync(siteItemId);
        }

        public void InstallHeliconApe(int siteItemId)
        {
            base.Client.InstallHeliconApe(siteItemId);
        }

        public async System.Threading.Tasks.Task InstallHeliconApeAsync(int siteItemId)
        {
            await base.Client.InstallHeliconApeAsync(siteItemId);
        }

        public int EnableHeliconApe(int siteItemId)
        {
            return base.Client.EnableHeliconApe(siteItemId);
        }

        public async System.Threading.Tasks.Task<int> EnableHeliconApeAsync(int siteItemId)
        {
            return await base.Client.EnableHeliconApeAsync(siteItemId);
        }

        public int DisableHeliconApe(int siteItemId)
        {
            return base.Client.DisableHeliconApe(siteItemId);
        }

        public async System.Threading.Tasks.Task<int> DisableHeliconApeAsync(int siteItemId)
        {
            return await base.Client.DisableHeliconApeAsync(siteItemId);
        }

        public int EnableHeliconApeGlobally(int serviceId)
        {
            return base.Client.EnableHeliconApeGlobally(serviceId);
        }

        public async System.Threading.Tasks.Task<int> EnableHeliconApeGloballyAsync(int serviceId)
        {
            return await base.Client.EnableHeliconApeGloballyAsync(serviceId);
        }

        public int DisableHeliconApeGlobally(int serviceId)
        {
            return base.Client.DisableHeliconApeGlobally(serviceId);
        }

        public async System.Threading.Tasks.Task<int> DisableHeliconApeGloballyAsync(int serviceId)
        {
            return await base.Client.DisableHeliconApeGloballyAsync(serviceId);
        }

        public SolidCP.Providers.Web.HtaccessFolder[] GetHeliconApeFolders(int siteItemId)
        {
            return base.Client.GetHeliconApeFolders(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder[]> GetHeliconApeFoldersAsync(int siteItemId)
        {
            return await base.Client.GetHeliconApeFoldersAsync(siteItemId);
        }

        public SolidCP.Providers.Web.HtaccessFolder GetHeliconApeHttpdFolder(int serviceId)
        {
            return base.Client.GetHeliconApeHttpdFolder(serviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeHttpdFolderAsync(int serviceId)
        {
            return await base.Client.GetHeliconApeHttpdFolderAsync(serviceId);
        }

        public SolidCP.Providers.Web.HtaccessFolder GetHeliconApeFolder(int siteItemId, string folderPath)
        {
            return base.Client.GetHeliconApeFolder(siteItemId, folderPath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessFolder> GetHeliconApeFolderAsync(int siteItemId, string folderPath)
        {
            return await base.Client.GetHeliconApeFolderAsync(siteItemId, folderPath);
        }

        public int UpdateHeliconApeFolder(int siteItemId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            return base.Client.UpdateHeliconApeFolder(siteItemId, folder);
        }

        public async System.Threading.Tasks.Task<int> UpdateHeliconApeFolderAsync(int siteItemId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            return await base.Client.UpdateHeliconApeFolderAsync(siteItemId, folder);
        }

        public int UpdateHeliconApeHttpdFolder(int serviceId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            return base.Client.UpdateHeliconApeHttpdFolder(serviceId, folder);
        }

        public async System.Threading.Tasks.Task<int> UpdateHeliconApeHttpdFolderAsync(int serviceId, SolidCP.Providers.Web.HtaccessFolder folder)
        {
            return await base.Client.UpdateHeliconApeHttpdFolderAsync(serviceId, folder);
        }

        public int DeleteHeliconApeFolder(int siteItemId, string folderPath)
        {
            return base.Client.DeleteHeliconApeFolder(siteItemId, folderPath);
        }

        public async System.Threading.Tasks.Task<int> DeleteHeliconApeFolderAsync(int siteItemId, string folderPath)
        {
            return await base.Client.DeleteHeliconApeFolderAsync(siteItemId, folderPath);
        }

        public SolidCP.Providers.Web.HtaccessUser[] GetHeliconApeUsers(int siteItemId)
        {
            return base.Client.GetHeliconApeUsers(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser[]> GetHeliconApeUsersAsync(int siteItemId)
        {
            return await base.Client.GetHeliconApeUsersAsync(siteItemId);
        }

        public SolidCP.Providers.Web.HtaccessUser GetHeliconApeUser(int siteItemId, string userName)
        {
            return base.Client.GetHeliconApeUser(siteItemId, userName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.HtaccessUser> GetHeliconApeUserAsync(int siteItemId, string userName)
        {
            return await base.Client.GetHeliconApeUserAsync(siteItemId, userName);
        }

        public int UpdateHeliconApeUser(int siteItemId, SolidCP.Providers.Web.HtaccessUser user)
        {
            return base.Client.UpdateHeliconApeUser(siteItemId, user);
        }

        public async System.Threading.Tasks.Task<int> UpdateHeliconApeUserAsync(int siteItemId, SolidCP.Providers.Web.HtaccessUser user)
        {
            return await base.Client.UpdateHeliconApeUserAsync(siteItemId, user);
        }

        public int DeleteHeliconApeUser(int siteItemId, string userName)
        {
            return base.Client.DeleteHeliconApeUser(siteItemId, userName);
        }

        public async System.Threading.Tasks.Task<int> DeleteHeliconApeUserAsync(int siteItemId, string userName)
        {
            return await base.Client.DeleteHeliconApeUserAsync(siteItemId, userName);
        }

        public SolidCP.Providers.Web.WebGroup[] GetHeliconApeGroups(int siteItemId)
        {
            return base.Client.GetHeliconApeGroups(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup[]> GetHeliconApeGroupsAsync(int siteItemId)
        {
            return await base.Client.GetHeliconApeGroupsAsync(siteItemId);
        }

        public SolidCP.Providers.Web.WebGroup GetHeliconApeGroup(int siteItemId, string groupName)
        {
            return base.Client.GetHeliconApeGroup(siteItemId, groupName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebGroup> GetHeliconApeGroupAsync(int siteItemId, string groupName)
        {
            return await base.Client.GetHeliconApeGroupAsync(siteItemId, groupName);
        }

        public int UpdateHeliconApeGroup(int siteItemId, SolidCP.Providers.Web.WebGroup group)
        {
            return base.Client.UpdateHeliconApeGroup(siteItemId, group);
        }

        public async System.Threading.Tasks.Task<int> UpdateHeliconApeGroupAsync(int siteItemId, SolidCP.Providers.Web.WebGroup group)
        {
            return await base.Client.UpdateHeliconApeGroupAsync(siteItemId, group);
        }

        public int DeleteHeliconApeGroup(int siteItemId, string groupName)
        {
            return base.Client.DeleteHeliconApeGroup(siteItemId, groupName);
        }

        public async System.Threading.Tasks.Task<int> DeleteHeliconApeGroupAsync(int siteItemId, string groupName)
        {
            return await base.Client.DeleteHeliconApeGroupAsync(siteItemId, groupName);
        }

        public SolidCP.Providers.Web.WebAppVirtualDirectory[] /*List*/ GetZooApplications(int siteItemId)
        {
            return base.Client.GetZooApplications(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebAppVirtualDirectory[]> GetZooApplicationsAsync(int siteItemId)
        {
            return await base.Client.GetZooApplicationsAsync(siteItemId);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooEnvironmentVariable(int siteItemId, string appName, string envName, string envValue)
        {
            return base.Client.SetZooEnvironmentVariable(siteItemId, appName, envName, envValue);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooEnvironmentVariableAsync(int siteItemId, string appName, string envName, string envValue)
        {
            return await base.Client.SetZooEnvironmentVariableAsync(siteItemId, appName, envName, envValue);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleEnabled(int siteItemId, string appName)
        {
            return base.Client.SetZooConsoleEnabled(siteItemId, appName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleEnabledAsync(int siteItemId, string appName)
        {
            return await base.Client.SetZooConsoleEnabledAsync(siteItemId, appName);
        }

        public SolidCP.Providers.ResultObjects.StringResultObject SetZooConsoleDisabled(int siteItemId, string appName)
        {
            return base.Client.SetZooConsoleDisabled(siteItemId, appName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.StringResultObject> SetZooConsoleDisabledAsync(int siteItemId, string appName)
        {
            return await base.Client.SetZooConsoleDisabledAsync(siteItemId, appName);
        }

        public SolidCP.Providers.Common.ResultObject GrantWebManagementAccess(int siteItemId, string accountName, string accountPassword)
        {
            return base.Client.GrantWebManagementAccess(siteItemId, accountName, accountPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> GrantWebManagementAccessAsync(int siteItemId, string accountName, string accountPassword)
        {
            return await base.Client.GrantWebManagementAccessAsync(siteItemId, accountName, accountPassword);
        }

        public void RevokeWebManagementAccess(int siteItemId)
        {
            base.Client.RevokeWebManagementAccess(siteItemId);
        }

        public async System.Threading.Tasks.Task RevokeWebManagementAccessAsync(int siteItemId)
        {
            await base.Client.RevokeWebManagementAccessAsync(siteItemId);
        }

        public SolidCP.Providers.Common.ResultObject ChangeWebManagementAccessPassword(int siteItemId, string accountPassword)
        {
            return base.Client.ChangeWebManagementAccessPassword(siteItemId, accountPassword);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ChangeWebManagementAccessPasswordAsync(int siteItemId, string accountPassword)
        {
            return await base.Client.ChangeWebManagementAccessPasswordAsync(siteItemId, accountPassword);
        }

        public SolidCP.Providers.Web.SSLCertificate CertificateRequest(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId)
        {
            return base.Client.CertificateRequest(certificate, siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> CertificateRequestAsync(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId)
        {
            return await base.Client.CertificateRequestAsync(certificate, siteItemId);
        }

        public SolidCP.Providers.Common.ResultObject InstallCertificate(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId)
        {
            return base.Client.InstallCertificate(certificate, siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InstallCertificateAsync(SolidCP.Providers.Web.SSLCertificate certificate, int siteItemId)
        {
            return await base.Client.InstallCertificateAsync(certificate, siteItemId);
        }

        public SolidCP.Providers.Common.ResultObject LEInstallCertificate(int siteItemId, string email)
        {
            return base.Client.LEInstallCertificate(siteItemId, email);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> LEInstallCertificateAsync(int siteItemId, string email)
        {
            return await base.Client.LEInstallCertificateAsync(siteItemId, email);
        }

        public SolidCP.Providers.Common.ResultObject InstallPfx(byte[] certificate, int siteItemId, string password)
        {
            return base.Client.InstallPfx(certificate, siteItemId, password);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> InstallPfxAsync(byte[] certificate, int siteItemId, string password)
        {
            return await base.Client.InstallPfxAsync(certificate, siteItemId, password);
        }

        public SolidCP.Providers.Web.SSLCertificate[] /*List*/ GetPendingCertificates(int siteItemId)
        {
            return base.Client.GetPendingCertificates(siteItemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate[]> GetPendingCertificatesAsync(int siteItemId)
        {
            return await base.Client.GetPendingCertificatesAsync(siteItemId);
        }

        public SolidCP.Providers.Web.SSLCertificate GetSSLCertificateByID(int Id)
        {
            return base.Client.GetSSLCertificateByID(Id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> GetSSLCertificateByIDAsync(int Id)
        {
            return await base.Client.GetSSLCertificateByIDAsync(Id);
        }

        public SolidCP.Providers.Web.SSLCertificate GetSiteCert(int siteID)
        {
            return base.Client.GetSiteCert(siteID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate> GetSiteCertAsync(int siteID)
        {
            return await base.Client.GetSiteCertAsync(siteID);
        }

        public int CheckSSLForWebsite(int siteID, bool renewal)
        {
            return base.Client.CheckSSLForWebsite(siteID, renewal);
        }

        public async System.Threading.Tasks.Task<int> CheckSSLForWebsiteAsync(int siteID, bool renewal)
        {
            return await base.Client.CheckSSLForWebsiteAsync(siteID, renewal);
        }

        public SolidCP.Providers.Common.ResultObject CheckSSLForDomain(string domain, int siteID)
        {
            return base.Client.CheckSSLForDomain(domain, siteID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CheckSSLForDomainAsync(string domain, int siteID)
        {
            return await base.Client.CheckSSLForDomainAsync(domain, siteID);
        }

        public byte[] ExportCertificate(int siteId, string serialNumber, string password)
        {
            return base.Client.ExportCertificate(siteId, serialNumber, password);
        }

        public async System.Threading.Tasks.Task<byte[]> ExportCertificateAsync(int siteId, string serialNumber, string password)
        {
            return await base.Client.ExportCertificateAsync(siteId, serialNumber, password);
        }

        public SolidCP.Providers.Web.SSLCertificate[] /*List*/ GetCertificatesForSite(int siteId)
        {
            return base.Client.GetCertificatesForSite(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.SSLCertificate[]> GetCertificatesForSiteAsync(int siteId)
        {
            return await base.Client.GetCertificatesForSiteAsync(siteId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteCertificate(int siteId, SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return base.Client.DeleteCertificate(siteId, certificate);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCertificateAsync(int siteId, SolidCP.Providers.Web.SSLCertificate certificate)
        {
            return await base.Client.DeleteCertificateAsync(siteId, certificate);
        }

        public SolidCP.Providers.Common.ResultObject ImportCertificate(int siteId)
        {
            return base.Client.ImportCertificate(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ImportCertificateAsync(int siteId)
        {
            return await base.Client.ImportCertificateAsync(siteId);
        }

        public SolidCP.Providers.Common.ResultObject CheckCertificate(int siteId)
        {
            return base.Client.CheckCertificate(siteId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> CheckCertificateAsync(int siteId)
        {
            return await base.Client.CheckCertificateAsync(siteId);
        }

        public SolidCP.Providers.Common.ResultObject DeleteCertificateRequest(int siteId, int csrID)
        {
            return base.Client.DeleteCertificateRequest(siteId, csrID);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteCertificateRequestAsync(int siteId, int csrID)
        {
            return await base.Client.DeleteCertificateRequestAsync(siteId, csrID);
        }
    }
}
#endif