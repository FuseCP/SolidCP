#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.SharePoint;
using SolidCP.Server.Utils;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
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
    public class SharePointServerService : SharePointServer, ISharePointServer
    {
        public new void ExtendVirtualServer(SharePointSite site)
        {
            base.ExtendVirtualServer(site);
        }

        public new void UnextendVirtualServer(string url, bool deleteContent)
        {
            base.UnextendVirtualServer(url, deleteContent);
        }

        public new string BackupVirtualServer(string url, string fileName, bool zipBackup)
        {
            return base.BackupVirtualServer(url, fileName, zipBackup);
        }

        public new void RestoreVirtualServer(string url, string fileName)
        {
            base.RestoreVirtualServer(url, fileName);
        }

        public new byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            return base.GetTempFileBinaryChunk(path, offset, length);
        }

        public new string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            return base.AppendTempFileBinaryChunk(fileName, path, chunk);
        }

        public new string[] GetInstalledWebParts(string url)
        {
            return base.GetInstalledWebParts(url);
        }

        public new void InstallWebPartsPackage(string url, string packageName)
        {
            base.InstallWebPartsPackage(url, packageName);
        }

        public new void DeleteWebPartsPackage(string url, string packageName)
        {
            base.DeleteWebPartsPackage(url, packageName);
        }

        public new bool UserExists(string username)
        {
            return base.UserExists(username);
        }

        public new string[] GetUsers()
        {
            return base.GetUsers();
        }

        public new SystemUser GetUser(string username)
        {
            return base.GetUser(username);
        }

        public new void CreateUser(SystemUser user)
        {
            base.CreateUser(user);
        }

        public new void UpdateUser(SystemUser user)
        {
            base.UpdateUser(user);
        }

        public new void ChangeUserPassword(string username, string password)
        {
            base.ChangeUserPassword(username, password);
        }

        public new void DeleteUser(string username)
        {
            base.DeleteUser(username);
        }

        public new bool GroupExists(string groupName)
        {
            return base.GroupExists(groupName);
        }

        public new string[] GetGroups()
        {
            return base.GetGroups();
        }

        public new SystemGroup GetGroup(string groupName)
        {
            return base.GetGroup(groupName);
        }

        public new void CreateGroup(SystemGroup group)
        {
            base.CreateGroup(group);
        }

        public new void UpdateGroup(SystemGroup group)
        {
            base.UpdateGroup(group);
        }

        public new void DeleteGroup(string groupName)
        {
            base.DeleteGroup(groupName);
        }
    }
}
#endif