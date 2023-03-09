#if Client
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.EnterpriseStorage;
using SolidCP.Providers.OS;
using SolidCP.Server.Utils;
using SolidCP.Providers.Web;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IEnterpriseStorage", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IEnterpriseStorage
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolders", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersResponse")]
        SystemFile[] GetFolders(string organizationId, WebDavSetting[] settings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolders", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersResponse")]
        System.Threading.Tasks.Task<SystemFile[]> GetFoldersAsync(string organizationId, WebDavSetting[] settings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersWithoutFrsm", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersWithoutFrsmResponse")]
        SystemFile[] GetFoldersWithoutFrsm(string organizationId, WebDavSetting[] settings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersWithoutFrsm", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFoldersWithoutFrsmResponse")]
        System.Threading.Tasks.Task<SystemFile[]> GetFoldersWithoutFrsmAsync(string organizationId, WebDavSetting[] settings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderResponse")]
        SystemFile GetFolder(string organizationId, string folder, WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderResponse")]
        System.Threading.Tasks.Task<SystemFile> GetFolderAsync(string organizationId, string folder, WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/CreateFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/CreateFolderResponse")]
        void CreateFolder(string organizationId, string folder, WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/CreateFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/CreateFolderResponse")]
        System.Threading.Tasks.Task CreateFolderAsync(string organizationId, string folder, WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/DeleteFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/DeleteFolderResponse")]
        void DeleteFolder(string organizationId, string folder, WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/DeleteFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/DeleteFolderResponse")]
        System.Threading.Tasks.Task DeleteFolderAsync(string organizationId, string folder, WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/SetFolderWebDavRules", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/SetFolderWebDavRulesResponse")]
        bool SetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting, WebDavFolderRule[] rules);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/SetFolderWebDavRules", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/SetFolderWebDavRulesResponse")]
        System.Threading.Tasks.Task<bool> SetFolderWebDavRulesAsync(string organizationId, string folder, WebDavSetting setting, WebDavFolderRule[] rules);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderWebDavRules", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderWebDavRulesResponse")]
        WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderWebDavRules", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetFolderWebDavRulesResponse")]
        System.Threading.Tasks.Task<WebDavFolderRule[]> GetFolderWebDavRulesAsync(string organizationId, string folder, WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/CheckFileServicesInstallation", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/CheckFileServicesInstallationResponse")]
        bool CheckFileServicesInstallation();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/CheckFileServicesInstallation", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/CheckFileServicesInstallationResponse")]
        System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/Search", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/SearchResponse")]
        SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/Search", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/SearchResponse")]
        System.Threading.Tasks.Task<SystemFile[]> SearchAsync(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/RenameFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/RenameFolderResponse")]
        SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/RenameFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/RenameFolderResponse")]
        System.Threading.Tasks.Task<SystemFile> RenameFolderAsync(string organizationId, string originalFolder, string newFolder, WebDavSetting setting);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetQuotasForOrganization", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetQuotasForOrganizationResponse")]
        SystemFile[] GetQuotasForOrganization(SystemFile[] folders);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetQuotasForOrganization", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/GetQuotasForOrganizationResponse")]
        System.Threading.Tasks.Task<SystemFile[]> GetQuotasForOrganizationAsync(SystemFile[] folders);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/MoveFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/MoveFolderResponse")]
        void MoveFolder(string oldPath, string newPath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IEnterpriseStorage/MoveFolder", ReplyAction = "http://smbsaas/solidcp/server/IEnterpriseStorage/MoveFolderResponse")]
        System.Threading.Tasks.Task MoveFolderAsync(string oldPath, string newPath);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class EnterpriseStorageAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IEnterpriseStorage
    {
        public SystemFile[] GetFolders(string organizationId, WebDavSetting[] settings)
        {
            return (SystemFile[])Invoke("SolidCP.Server.EnterpriseStorage", "GetFolders", organizationId, settings);
        }

        public async System.Threading.Tasks.Task<SystemFile[]> GetFoldersAsync(string organizationId, WebDavSetting[] settings)
        {
            return await InvokeAsync<SystemFile[]>("SolidCP.Server.EnterpriseStorage", "GetFolders", organizationId, settings);
        }

        public SystemFile[] GetFoldersWithoutFrsm(string organizationId, WebDavSetting[] settings)
        {
            return (SystemFile[])Invoke("SolidCP.Server.EnterpriseStorage", "GetFoldersWithoutFrsm", organizationId, settings);
        }

        public async System.Threading.Tasks.Task<SystemFile[]> GetFoldersWithoutFrsmAsync(string organizationId, WebDavSetting[] settings)
        {
            return await InvokeAsync<SystemFile[]>("SolidCP.Server.EnterpriseStorage", "GetFoldersWithoutFrsm", organizationId, settings);
        }

        public SystemFile GetFolder(string organizationId, string folder, WebDavSetting setting)
        {
            return (SystemFile)Invoke("SolidCP.Server.EnterpriseStorage", "GetFolder", organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task<SystemFile> GetFolderAsync(string organizationId, string folder, WebDavSetting setting)
        {
            return await InvokeAsync<SystemFile>("SolidCP.Server.EnterpriseStorage", "GetFolder", organizationId, folder, setting);
        }

        public void CreateFolder(string organizationId, string folder, WebDavSetting setting)
        {
            Invoke("SolidCP.Server.EnterpriseStorage", "CreateFolder", organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task CreateFolderAsync(string organizationId, string folder, WebDavSetting setting)
        {
            await InvokeAsync("SolidCP.Server.EnterpriseStorage", "CreateFolder", organizationId, folder, setting);
        }

        public void DeleteFolder(string organizationId, string folder, WebDavSetting setting)
        {
            Invoke("SolidCP.Server.EnterpriseStorage", "DeleteFolder", organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task DeleteFolderAsync(string organizationId, string folder, WebDavSetting setting)
        {
            await InvokeAsync("SolidCP.Server.EnterpriseStorage", "DeleteFolder", organizationId, folder, setting);
        }

        public bool SetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting, WebDavFolderRule[] rules)
        {
            return (bool)Invoke("SolidCP.Server.EnterpriseStorage", "SetFolderWebDavRules", organizationId, folder, setting, rules);
        }

        public async System.Threading.Tasks.Task<bool> SetFolderWebDavRulesAsync(string organizationId, string folder, WebDavSetting setting, WebDavFolderRule[] rules)
        {
            return await InvokeAsync<bool>("SolidCP.Server.EnterpriseStorage", "SetFolderWebDavRules", organizationId, folder, setting, rules);
        }

        public WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting)
        {
            return (WebDavFolderRule[])Invoke("SolidCP.Server.EnterpriseStorage", "GetFolderWebDavRules", organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task<WebDavFolderRule[]> GetFolderWebDavRulesAsync(string organizationId, string folder, WebDavSetting setting)
        {
            return await InvokeAsync<WebDavFolderRule[]>("SolidCP.Server.EnterpriseStorage", "GetFolderWebDavRules", organizationId, folder, setting);
        }

        public bool CheckFileServicesInstallation()
        {
            return (bool)Invoke("SolidCP.Server.EnterpriseStorage", "CheckFileServicesInstallation");
        }

        public async System.Threading.Tasks.Task<bool> CheckFileServicesInstallationAsync()
        {
            return await InvokeAsync<bool>("SolidCP.Server.EnterpriseStorage", "CheckFileServicesInstallation");
        }

        public SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return (SystemFile[])Invoke("SolidCP.Server.EnterpriseStorage", "Search", organizationId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public async System.Threading.Tasks.Task<SystemFile[]> SearchAsync(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return await InvokeAsync<SystemFile[]>("SolidCP.Server.EnterpriseStorage", "Search", organizationId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, WebDavSetting setting)
        {
            return (SystemFile)Invoke("SolidCP.Server.EnterpriseStorage", "RenameFolder", organizationId, originalFolder, newFolder, setting);
        }

        public async System.Threading.Tasks.Task<SystemFile> RenameFolderAsync(string organizationId, string originalFolder, string newFolder, WebDavSetting setting)
        {
            return await InvokeAsync<SystemFile>("SolidCP.Server.EnterpriseStorage", "RenameFolder", organizationId, originalFolder, newFolder, setting);
        }

        public SystemFile[] GetQuotasForOrganization(SystemFile[] folders)
        {
            return (SystemFile[])Invoke("SolidCP.Server.EnterpriseStorage", "GetQuotasForOrganization", folders);
        }

        public async System.Threading.Tasks.Task<SystemFile[]> GetQuotasForOrganizationAsync(SystemFile[] folders)
        {
            return await InvokeAsync<SystemFile[]>("SolidCP.Server.EnterpriseStorage", "GetQuotasForOrganization", folders);
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
        public SystemFile[] GetFolders(string organizationId, WebDavSetting[] settings)
        {
            return base.Client.GetFolders(organizationId, settings);
        }

        public async System.Threading.Tasks.Task<SystemFile[]> GetFoldersAsync(string organizationId, WebDavSetting[] settings)
        {
            return await base.Client.GetFoldersAsync(organizationId, settings);
        }

        public SystemFile[] GetFoldersWithoutFrsm(string organizationId, WebDavSetting[] settings)
        {
            return base.Client.GetFoldersWithoutFrsm(organizationId, settings);
        }

        public async System.Threading.Tasks.Task<SystemFile[]> GetFoldersWithoutFrsmAsync(string organizationId, WebDavSetting[] settings)
        {
            return await base.Client.GetFoldersWithoutFrsmAsync(organizationId, settings);
        }

        public SystemFile GetFolder(string organizationId, string folder, WebDavSetting setting)
        {
            return base.Client.GetFolder(organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task<SystemFile> GetFolderAsync(string organizationId, string folder, WebDavSetting setting)
        {
            return await base.Client.GetFolderAsync(organizationId, folder, setting);
        }

        public void CreateFolder(string organizationId, string folder, WebDavSetting setting)
        {
            base.Client.CreateFolder(organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task CreateFolderAsync(string organizationId, string folder, WebDavSetting setting)
        {
            await base.Client.CreateFolderAsync(organizationId, folder, setting);
        }

        public void DeleteFolder(string organizationId, string folder, WebDavSetting setting)
        {
            base.Client.DeleteFolder(organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task DeleteFolderAsync(string organizationId, string folder, WebDavSetting setting)
        {
            await base.Client.DeleteFolderAsync(organizationId, folder, setting);
        }

        public bool SetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting, WebDavFolderRule[] rules)
        {
            return base.Client.SetFolderWebDavRules(organizationId, folder, setting, rules);
        }

        public async System.Threading.Tasks.Task<bool> SetFolderWebDavRulesAsync(string organizationId, string folder, WebDavSetting setting, WebDavFolderRule[] rules)
        {
            return await base.Client.SetFolderWebDavRulesAsync(organizationId, folder, setting, rules);
        }

        public WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting)
        {
            return base.Client.GetFolderWebDavRules(organizationId, folder, setting);
        }

        public async System.Threading.Tasks.Task<WebDavFolderRule[]> GetFolderWebDavRulesAsync(string organizationId, string folder, WebDavSetting setting)
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

        public SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return base.Client.Search(organizationId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public async System.Threading.Tasks.Task<SystemFile[]> SearchAsync(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return await base.Client.SearchAsync(organizationId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, WebDavSetting setting)
        {
            return base.Client.RenameFolder(organizationId, originalFolder, newFolder, setting);
        }

        public async System.Threading.Tasks.Task<SystemFile> RenameFolderAsync(string organizationId, string originalFolder, string newFolder, WebDavSetting setting)
        {
            return await base.Client.RenameFolderAsync(organizationId, originalFolder, newFolder, setting);
        }

        public SystemFile[] GetQuotasForOrganization(SystemFile[] folders)
        {
            return base.Client.GetQuotasForOrganization(folders);
        }

        public async System.Threading.Tasks.Task<SystemFile[]> GetQuotasForOrganizationAsync(SystemFile[] folders)
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