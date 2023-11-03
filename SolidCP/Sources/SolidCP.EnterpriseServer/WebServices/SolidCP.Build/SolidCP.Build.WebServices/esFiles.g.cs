#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers.OS;
using SolidCP.EnterpriseServer;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.EnterpriseServer.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesFiles
    {
        [WebMethod]
        [OperationContract]
        SystemSettings GetFileManagerSettings();
        [WebMethod]
        [OperationContract]
        string GetHomeFolder(int packageId);
        [WebMethod]
        [OperationContract]
        List<SystemFile> GetFiles(int packageId, string path, bool includeFiles);
        [WebMethod]
        [OperationContract]
        List<SystemFile> GetFilesByMask(int packageId, string path, string filesMask);
        [WebMethod]
        [OperationContract]
        byte[] GetFileBinaryContent(int packageId, string path);
        [WebMethod]
        [OperationContract]
        byte[] GetFileBinaryContentUsingEncoding(int packageId, string path, string encoding);
        [WebMethod]
        [OperationContract]
        int UpdateFileBinaryContent(int packageId, string path, byte[] content);
        [WebMethod]
        [OperationContract]
        int UpdateFileBinaryContentUsingEncoding(int packageId, string path, byte[] content, string encoding);
        [WebMethod]
        [OperationContract]
        byte[] GetFileBinaryChunk(int packageId, string path, int offset, int length);
        [WebMethod]
        [OperationContract]
        int AppendFileBinaryChunk(int packageId, string path, byte[] chunk);
        [WebMethod]
        [OperationContract]
        int DeleteFiles(int packageId, string[] files);
        [WebMethod]
        [OperationContract]
        int CreateFile(int packageId, string path);
        [WebMethod]
        [OperationContract]
        int CreateFolder(int packageId, string path);
        [WebMethod]
        [OperationContract]
        int CopyFiles(int packageId, string[] files, string destFolder);
        [WebMethod]
        [OperationContract]
        int MoveFiles(int packageId, string[] files, string destFolder);
        [WebMethod]
        [OperationContract]
        int RenameFile(int packageId, string oldPath, string newPath);
        [WebMethod]
        [OperationContract]
        void UnzipFiles(int packageId, string[] files);
        [WebMethod]
        [OperationContract]
        int ZipFiles(int packageId, string[] files, string archivePath);
        [WebMethod]
        [OperationContract]
        int ZipRemoteFiles(int packageId, string rootFolder, string[] files, string archivePath);
        [WebMethod]
        [OperationContract]
        int CreateAccessDatabase(int packageId, string dbPath);
        [WebMethod]
        [OperationContract]
        int CalculatePackageDiskspace(int packageId);
        [WebMethod]
        [OperationContract]
        UserPermission[] GetFilePermissions(int packageId, string path);
        [WebMethod]
        [OperationContract]
        int SetFilePermissions(int packageId, string path, UserPermission[] users, bool resetChildPermissions);
        [WebMethod]
        [OperationContract]
        FolderGraph GetFolderGraph(int packageId, string path);
        [WebMethod]
        [OperationContract]
        void ExecuteSyncActions(int packageId, FileSyncAction[] actions);
        [WebMethod]
        [OperationContract]
        int ApplyEnableHardQuotaFeature(int packageId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esFiles : SolidCP.EnterpriseServer.esFiles, IesFiles
    {
    }
}
#endif