#if !Client
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
//using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.EnterpriseStorage;
using SolidCP.Providers.OS;
using SolidCP.Server.Utils;
using SolidCP.Providers.Web;
using SolidCP.Server;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IEnterpriseStorage
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile[] GetFolders(string organizationId, WebDavSetting[] settings);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile[] GetFoldersWithoutFrsm(string organizationId, WebDavSetting[] settings);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile GetFolder(string organizationId, string folder, WebDavSetting setting);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateFolder(string organizationId, string folder, WebDavSetting setting);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteFolder(string organizationId, string folder, WebDavSetting setting);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool SetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting, WebDavFolderRule[] rules);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckFileServicesInstallation();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, WebDavSetting setting);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemFile[] GetQuotasForOrganization(SystemFile[] folders);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void MoveFolder(string oldPath, string newPath);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class EnterpriseStorage : SolidCP.Server.EnterpriseStorage, IEnterpriseStorage
    {
        public new SystemFile[] GetFolders(string organizationId, WebDavSetting[] settings)
        {
            return base.GetFolders(organizationId, settings);
        }

        public new SystemFile[] GetFoldersWithoutFrsm(string organizationId, WebDavSetting[] settings)
        {
            return base.GetFoldersWithoutFrsm(organizationId, settings);
        }

        public new SystemFile GetFolder(string organizationId, string folder, WebDavSetting setting)
        {
            return base.GetFolder(organizationId, folder, setting);
        }

        public new void CreateFolder(string organizationId, string folder, WebDavSetting setting)
        {
            base.CreateFolder(organizationId, folder, setting);
        }

        public new void DeleteFolder(string organizationId, string folder, WebDavSetting setting)
        {
            base.DeleteFolder(organizationId, folder, setting);
        }

        public new bool SetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting, WebDavFolderRule[] rules)
        {
            return base.SetFolderWebDavRules(organizationId, folder, setting, rules);
        }

        public new WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting)
        {
            return base.GetFolderWebDavRules(organizationId, folder, setting);
        }

        public new bool CheckFileServicesInstallation()
        {
            return base.CheckFileServicesInstallation();
        }

        public new SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            return base.Search(organizationId, searchPaths, searchText, userPrincipalName, recursive);
        }

        public new SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, WebDavSetting setting)
        {
            return base.RenameFolder(organizationId, originalFolder, newFolder, setting);
        }

        public new SystemFile[] GetQuotasForOrganization(SystemFile[] folders)
        {
            return base.GetQuotasForOrganization(folders);
        }

        public new void MoveFolder(string oldPath, string newPath)
        {
            base.MoveFolder(oldPath, newPath);
        }
    }
}
#endif