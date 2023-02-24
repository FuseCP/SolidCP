#if Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.Mail;
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
    /// Summary description for MailServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IMailServer
    {

        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool DomainExists(string domainName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        MailDomain GetDomain(string domainName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetDomains();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateDomain(MailDomain domain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateDomain(MailDomain domain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteDomain(string domainName);


        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool DomainAliasExists(string domainName, string aliasName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetDomainAliases(string domainName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void AddDomainAlias(string domainName, string aliasName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteDomainAlias(string domainName, string aliasName);


        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool AccountExists(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        MailAccount[] GetAccounts(string domainName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        MailAccount GetAccount(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateAccount(MailAccount account);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateAccount(MailAccount account);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteAccount(string accountName);


        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool MailAliasExists(string mailAliasName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        MailAlias[] GetMailAliases(string domainName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        MailAlias GetMailAlias(string mailAliasName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateMailAlias(MailAlias mailAlias);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateMailAlias(MailAlias mailAlias);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteMailAlias(string mailAliasName);


        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool GroupExists(string groupName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        MailGroup[] GetGroups(string domainName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        MailGroup GetGroup(string groupName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateGroup(MailGroup group);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateGroup(MailGroup group);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteGroup(string groupName);


        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool ListExists(string listName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        MailList[] GetLists(string domainName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        MailList GetList(string listName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateList(MailList list);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateList(MailList list);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteList(string listName);
    }

    public class MailServer
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