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
using SolidCP.Providers.Mail;
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

    // wcf service
    public class MailServerService : MailServer, IMailServer
    {
        public new bool DomainExists(string domainName)
        {
            return base.DomainExists(domainName);
        }

        public new MailDomain GetDomain(string domainName)
        {
            return base.GetDomain(domainName);
        }

        public new string[] GetDomains()
        {
            return base.GetDomains();
        }

        public new void CreateDomain(MailDomain domain)
        {
            base.CreateDomain(domain);
        }

        public new void UpdateDomain(MailDomain domain)
        {
            base.UpdateDomain(domain);
        }

        public new void DeleteDomain(string domainName)
        {
            base.DeleteDomain(domainName);
        }

        public new bool DomainAliasExists(string domainName, string aliasName)
        {
            return base.DomainAliasExists(domainName, aliasName);
        }

        public new string[] GetDomainAliases(string domainName)
        {
            return base.GetDomainAliases(domainName);
        }

        public new void AddDomainAlias(string domainName, string aliasName)
        {
            base.AddDomainAlias(domainName, aliasName);
        }

        public new void DeleteDomainAlias(string domainName, string aliasName)
        {
            base.DeleteDomainAlias(domainName, aliasName);
        }

        public new bool AccountExists(string accountName)
        {
            return base.AccountExists(accountName);
        }

        public new MailAccount[] GetAccounts(string domainName)
        {
            return base.GetAccounts(domainName);
        }

        public new MailAccount GetAccount(string accountName)
        {
            return base.GetAccount(accountName);
        }

        public new void CreateAccount(MailAccount account)
        {
            base.CreateAccount(account);
        }

        public new void UpdateAccount(MailAccount account)
        {
            base.UpdateAccount(account);
        }

        public new void DeleteAccount(string accountName)
        {
            base.DeleteAccount(accountName);
        }

        public new bool MailAliasExists(string mailAliasName)
        {
            return base.MailAliasExists(mailAliasName);
        }

        public new MailAlias[] GetMailAliases(string domainName)
        {
            return base.GetMailAliases(domainName);
        }

        public new MailAlias GetMailAlias(string mailAliasName)
        {
            return base.GetMailAlias(mailAliasName);
        }

        public new void CreateMailAlias(MailAlias mailAlias)
        {
            base.CreateMailAlias(mailAlias);
        }

        public new void UpdateMailAlias(MailAlias mailAlias)
        {
            base.UpdateMailAlias(mailAlias);
        }

        public new void DeleteMailAlias(string mailAliasName)
        {
            base.DeleteMailAlias(mailAliasName);
        }

        public new bool GroupExists(string groupName)
        {
            return base.GroupExists(groupName);
        }

        public new MailGroup[] GetGroups(string domainName)
        {
            return base.GetGroups(domainName);
        }

        public new MailGroup GetGroup(string groupName)
        {
            return base.GetGroup(groupName);
        }

        public new void CreateGroup(MailGroup group)
        {
            base.CreateGroup(group);
        }

        public new void UpdateGroup(MailGroup group)
        {
            base.UpdateGroup(group);
        }

        public new void DeleteGroup(string groupName)
        {
            base.DeleteGroup(groupName);
        }

        public new bool ListExists(string listName)
        {
            return base.ListExists(listName);
        }

        public new MailList[] GetLists(string domainName)
        {
            return base.GetLists(domainName);
        }

        public new MailList GetList(string listName)
        {
            return base.GetList(listName);
        }

        public new void CreateList(MailList list)
        {
            base.CreateList(list);
        }

        public new void UpdateList(MailList list)
        {
            base.UpdateList(list);
        }

        public new void DeleteList(string listName)
        {
            base.DeleteList(listName);
        }
    }
}
#endif