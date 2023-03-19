#if !Client
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Collections;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.EnterpriseStorage;
using SolidCP.Providers.OS;
using SolidCP.Server.Utils;
using SolidCP.Providers.Web;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

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
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class EnterpriseStorage : SolidCP.Server.EnterpriseStorage, IEnterpriseStorage
    {
    }
}
#endif