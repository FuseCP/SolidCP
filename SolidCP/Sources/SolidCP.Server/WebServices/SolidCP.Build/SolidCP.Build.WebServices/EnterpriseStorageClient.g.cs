#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IEnterpriseStorage", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IEnterpriseStorage
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolders", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersResponse")]
        SolidCP.Providers.OS.SystemFile[] GetFolders(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolders", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFoldersAsync(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersWithoutFrsm", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersWithoutFrsmResponse")]
        SolidCP.Providers.OS.SystemFile[] GetFoldersWithoutFrsm(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersWithoutFrsm", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersWithoutFrsmResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFoldersWithoutFrsmAsync(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderResponse")]
        SolidCP.Providers.OS.SystemFile GetFolder(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetFolderAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/CreateFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/CreateFolderResponse")]
        void CreateFolder(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/CreateFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/CreateFolderResponse")]
        System.Threading.Tasks.Task CreateFolderAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/DeleteFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/DeleteFolderResponse")]
        void DeleteFolder(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/DeleteFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/DeleteFolderResponse")]
        System.Threading.Tasks.Task DeleteFolderAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/SetFolderWebDavRules", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/SetFolderWebDavRulesResponse")]
        bool SetFolderWebDavRules(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting, SolidCP.Providers.Web.WebDavFolderRule[] rules);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/SetFolderWebDavRules", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/SetFolderWebDavRulesResponse")]
        System.Threading.Tasks.Task<bool> SetFolderWebDavRulesAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting, SolidCP.Providers.Web.WebDavFolderRule[] rules);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderWebDavRules", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderWebDavRulesResponse")]
        SolidCP.Providers.Web.WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderWebDavRules", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderWebDavRulesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Web.WebDavFolderRule[]> GetFolderWebDavRulesAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/CheckFileServicesInstallation", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/CheckFileServicesInstallationResponse")]
        bool CheckFileServicesInstallation();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/CheckFileServicesInstallation", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/CheckFileServicesInstallationResponse")]
        System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/Search", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/SearchResponse")]
        SolidCP.Providers.OS.SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/Search", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/SearchResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> SearchAsync(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/RenameFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/RenameFolderResponse")]
        SolidCP.Providers.OS.SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, SolidCP.Providers.Web.WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/RenameFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/RenameFolderResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> RenameFolderAsync(string organizationId, string originalFolder, string newFolder, SolidCP.Providers.Web.WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetQuotasForOrganization", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetQuotasForOrganizationResponse")]
        SolidCP.Providers.OS.SystemFile[] GetQuotasForOrganization(SolidCP.Providers.OS.SystemFile[] folders);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetQuotasForOrganization", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetQuotasForOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetQuotasForOrganizationAsync(SolidCP.Providers.OS.SystemFile[] folders);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/MoveFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/MoveFolderResponse")]
        void MoveFolder(string oldPath, string newPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/MoveFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/MoveFolderResponse")]
        System.Threading.Tasks.Task MoveFolderAsync(string oldPath, string newPath);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class EnterpriseStorageAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IEnterpriseStorage
    {
        public SolidCP.Providers.OS.SystemFile[] GetFolders(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.EnterpriseStorage", "GetFolders", organizationId, settings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFoldersAsync(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.EnterpriseStorage", "GetFolders", organizationId, settings);
        }

        public SolidCP.Providers.OS.SystemFile[] GetFoldersWithoutFrsm(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.EnterpriseStorage", "GetFoldersWithoutFrsm", organizationId, settings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFoldersWithoutFrsmAsync(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.EnterpriseStorage", "GetFoldersWithoutFrsm", organizationId, settings);
        }

        public SolidCP.Providers.OS.SystemFile GetFolder(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile>("SolidCP.Server.EnterpriseStorage", "GetFolder", organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetFolderAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile>("SolidCP.Server.EnterpriseStorage", "GetFolder", organizationId, folder, setting);
        }

        public void CreateFolder(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            Invoke("SolidCP.Server.EnterpriseStorage", "CreateFolder", organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task CreateFolderAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            await InvokeAsync("SolidCP.Server.EnterpriseStorage", "CreateFolder", organizationId, folder, setting);
        }

        public void DeleteFolder(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            Invoke("SolidCP.Server.EnterpriseStorage", "DeleteFolder", organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task DeleteFolderAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            await InvokeAsync("SolidCP.Server.EnterpriseStorage", "DeleteFolder", organizationId, folder, setting);
        }

        public bool SetFolderWebDavRules(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting, SolidCP.Providers.Web.WebDavFolderRule[] rules)
        {
            return Invoke<bool>("SolidCP.Server.EnterpriseStorage", "SetFolderWebDavRules", organizationId, folder, setting, rules);
        }

        public async System.Threading.Tasks.Task<bool> SetFolderWebDavRulesAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting, SolidCP.Providers.Web.WebDavFolderRule[] rules)
        {
            return await InvokeAsync<bool>("SolidCP.Server.EnterpriseStorage", "SetFolderWebDavRules", organizationId, folder, setting, rules);
        }

        public SolidCP.Providers.Web.WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return Invoke<SolidCP.Providers.Web.WebDavFolderRule[]>("SolidCP.Server.EnterpriseStorage", "GetFolderWebDavRules", organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebDavFolderRule[]> GetFolderWebDavRulesAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return await InvokeAsync<SolidCP.Providers.Web.WebDavFolderRule[]>("SolidCP.Server.EnterpriseStorage", "GetFolderWebDavRules", organizationId, folder, setting);
        }

        public bool CheckFileServicesInstallation()
        {
            return Invoke<bool>("SolidCP.Server.EnterpriseStorage", "CheckFileServicesInstallation");
        }

        public async System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.EnterpriseStorage", "CheckFileServicesInstallation");
        }

        public SolidCP.Providers.OS.SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.EnterpriseStorage", "Search", organizationId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> SearchAsync(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.EnterpriseStorage", "Search", organizationId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public SolidCP.Providers.OS.SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile>("SolidCP.Server.EnterpriseStorage", "RenameFolder", organizationId, originalFolder, newFolder, setting);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> RenameFolderAsync(string organizationId, string originalFolder, string newFolder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile>("SolidCP.Server.EnterpriseStorage", "RenameFolder", organizationId, originalFolder, newFolder, setting);
        }

        public SolidCP.Providers.OS.SystemFile[] GetQuotasForOrganization(SolidCP.Providers.OS.SystemFile[] folders)
        {
            return Invoke<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.EnterpriseStorage", "GetQuotasForOrganization", folders);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetQuotasForOrganizationAsync(SolidCP.Providers.OS.SystemFile[] folders)
        {
            return await InvokeAsync<SolidCP.Providers.OS.SystemFile[]>("SolidCP.Server.EnterpriseStorage", "GetQuotasForOrganization", folders);
        }

        public void MoveFolder(string oldPath, string newPath)
        {
            Invoke("SolidCP.Server.EnterpriseStorage", "MoveFolder", oldPath, newPath);
        }

        public async System.Threading.Tasks.Task MoveFolderAsync(string oldPath, string newPath)
        {
            await InvokeAsync("SolidCP.Server.EnterpriseStorage", "MoveFolder", oldPath, newPath);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class EnterpriseStorage : SolidCP.Web.Client.ClientBase<IEnterpriseStorage, EnterpriseStorageAssemblyClient>, IEnterpriseStorage
    {
        public SolidCP.Providers.OS.SystemFile[] GetFolders(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings)
        {
            return base.Client.GetFolders(organizationId, settings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFoldersAsync(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings)
        {
            return await base.Client.GetFoldersAsync(organizationId, settings);
        }

        public SolidCP.Providers.OS.SystemFile[] GetFoldersWithoutFrsm(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings)
        {
            return base.Client.GetFoldersWithoutFrsm(organizationId, settings);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetFoldersWithoutFrsmAsync(string organizationId, SolidCP.Providers.Web.WebDavSetting[] settings)
        {
            return await base.Client.GetFoldersWithoutFrsmAsync(organizationId, settings);
        }

        public SolidCP.Providers.OS.SystemFile GetFolder(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return base.Client.GetFolder(organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> GetFolderAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return await base.Client.GetFolderAsync(organizationId, folder, setting);
        }

        public void CreateFolder(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            base.Client.CreateFolder(organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task CreateFolderAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            await base.Client.CreateFolderAsync(organizationId, folder, setting);
        }

        public void DeleteFolder(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            base.Client.DeleteFolder(organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task DeleteFolderAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            await base.Client.DeleteFolderAsync(organizationId, folder, setting);
        }

        public bool SetFolderWebDavRules(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting, SolidCP.Providers.Web.WebDavFolderRule[] rules)
        {
            return base.Client.SetFolderWebDavRules(organizationId, folder, setting, rules);
        }

        public async System.Threading.Tasks.Task<bool> SetFolderWebDavRulesAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting, SolidCP.Providers.Web.WebDavFolderRule[] rules)
        {
            return await base.Client.SetFolderWebDavRulesAsync(organizationId, folder, setting, rules);
        }

        public SolidCP.Providers.Web.WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return base.Client.GetFolderWebDavRules(organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Web.WebDavFolderRule[]> GetFolderWebDavRulesAsync(string organizationId, string folder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return await base.Client.GetFolderWebDavRulesAsync(organizationId, folder, setting);
        }

        public bool CheckFileServicesInstallation()
        {
            return base.Client.CheckFileServicesInstallation();
        }

        public async System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync()
        {
            return await base.Client.CheckFileServicesInstallationAsync();
        }

        public SolidCP.Providers.OS.SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return base.Client.Search(organizationId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> SearchAsync(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return await base.Client.SearchAsync(organizationId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public SolidCP.Providers.OS.SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return base.Client.RenameFolder(organizationId, originalFolder, newFolder, setting);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile> RenameFolderAsync(string organizationId, string originalFolder, string newFolder, SolidCP.Providers.Web.WebDavSetting setting)
        {
            return await base.Client.RenameFolderAsync(organizationId, originalFolder, newFolder, setting);
        }

        public SolidCP.Providers.OS.SystemFile[] GetQuotasForOrganization(SolidCP.Providers.OS.SystemFile[] folders)
        {
            return base.Client.GetQuotasForOrganization(folders);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.OS.SystemFile[]> GetQuotasForOrganizationAsync(SolidCP.Providers.OS.SystemFile[] folders)
        {
            return await base.Client.GetQuotasForOrganizationAsync(folders);
        }

        public void MoveFolder(string oldPath, string newPath)
        {
            base.Client.MoveFolder(oldPath, newPath);
        }

        public async System.Threading.Tasks.Task MoveFolderAsync(string oldPath, string newPath)
        {
            await base.Client.MoveFolderAsync(oldPath, newPath);
        }
    }
}
#endif