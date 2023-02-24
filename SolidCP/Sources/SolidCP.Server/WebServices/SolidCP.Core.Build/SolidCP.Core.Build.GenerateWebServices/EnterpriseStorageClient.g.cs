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
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Client
{
    /// <summary>
    /// Summary description for EnterpriseStorage
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
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

    public class EnterpriseStorage
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