#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.SharePoint;
using SolidCP.Server.Utils;
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
    public interface ISharePointServer
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ExtendVirtualServer(SharePointSite site);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UnextendVirtualServer(string url, bool deleteContent);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string BackupVirtualServer(string url, string fileName, bool zipBackup);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RestoreVirtualServer(string url, string fileName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        byte[] GetTempFileBinaryChunk(string path, int offset, int length);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetInstalledWebParts(string url);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void InstallWebPartsPackage(string url, string packageName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteWebPartsPackage(string url, string packageName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool UserExists(string username);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetUsers();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemUser GetUser(string username);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateUser(SystemUser user);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateUser(SystemUser user);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeUserPassword(string username, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteUser(string username);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool GroupExists(string groupName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetGroups();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SystemGroup GetGroup(string groupName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateGroup(SystemGroup group);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateGroup(SystemGroup group);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteGroup(string groupName);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class SharePointServer : SolidCP.Server.SharePointServer, ISharePointServer
    {
    }
}
#endif