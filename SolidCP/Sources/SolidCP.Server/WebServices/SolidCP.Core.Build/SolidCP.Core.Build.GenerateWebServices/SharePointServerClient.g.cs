﻿#if Client
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
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Client
{
    /// <summary>
    /// Summary description for SharePointServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
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

    public class SharePointServer
    {
        ChannelFactory<T> _Factory { get; set; }

        public Credentials Credentials { get; set; }

        public object SoapHeader { get; set; }

        void Test()
        {
            try
            {
                var client = _Factory.CreateChannel();
                client.MyServiceOperation();
                ((ICommunicationObject)client).Close();
                _Factory.Close();
            }
            catch
            {
                (client as ICommunicationObject)?.Abort();
            }
        }
    }
}
#endif