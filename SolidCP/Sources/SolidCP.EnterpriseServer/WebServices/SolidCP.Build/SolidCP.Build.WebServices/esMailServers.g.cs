#if !Client
using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers.Mail;
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
    public interface IesMailServers
    {
        [WebMethod]
        [OperationContract]
        DataSet GetRawMailAccountsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<MailAccount> GetMailAccounts(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        MailAccount GetMailAccount(int itemId);
        [WebMethod]
        [OperationContract]
        int AddMailAccount(MailAccount item);
        [WebMethod]
        [OperationContract]
        int UpdateMailAccount(MailAccount item);
        [WebMethod]
        [OperationContract]
        int DeleteMailAccount(int itemId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawMailForwardingsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<MailAlias> GetMailForwardings(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        MailAlias GetMailForwarding(int itemId);
        [WebMethod]
        [OperationContract]
        int AddMailForwarding(MailAlias item);
        [WebMethod]
        [OperationContract]
        int UpdateMailForwarding(MailAlias item);
        [WebMethod]
        [OperationContract]
        int DeleteMailForwarding(int itemId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawMailGroupsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<MailGroup> GetMailGroups(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        MailGroup GetMailGroup(int itemId);
        [WebMethod]
        [OperationContract]
        int AddMailGroup(MailGroup item);
        [WebMethod]
        [OperationContract]
        int UpdateMailGroup(MailGroup item);
        [WebMethod]
        [OperationContract]
        int DeleteMailGroup(int itemId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawMailListsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<MailList> GetMailLists(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        MailList GetMailList(int itemId);
        [WebMethod]
        [OperationContract]
        int AddMailList(MailList item);
        [WebMethod]
        [OperationContract]
        int UpdateMailList(MailList item);
        [WebMethod]
        [OperationContract]
        int DeleteMailList(int itemId);
        [WebMethod]
        [OperationContract]
        DataSet GetRawMailDomainsPaged(int packageId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<MailDomain> GetMailDomains(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        List<DomainInfo> GetMailDomainPointers(int itemId);
        [WebMethod]
        [OperationContract]
        MailDomain GetMailDomain(int itemId);
        [WebMethod]
        [OperationContract]
        int AddMailDomain(MailDomain item);
        [WebMethod]
        [OperationContract]
        int UpdateMailDomain(MailDomain item);
        [WebMethod]
        [OperationContract]
        int DeleteMailDomain(int itemId);
        [WebMethod]
        [OperationContract]
        int AddMailDomainPointer(int itemId, int domainId);
        [WebMethod]
        [OperationContract]
        int DeleteMailDomainPointer(int itemId, int domainId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esMailServers : SolidCP.EnterpriseServer.esMailServers, IesMailServers
    {
    }
}
#endif