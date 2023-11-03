#if !Client
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using SolidCP.Web.Services;
using SolidCP.EnterpriseServer.Base;
using SolidCP.Providers.Filters;
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
    public interface IesSpamExperts
    {
        [WebMethod]
        [OperationContract]
        SpamExpertsResult AddDomainFilter(SpamExpertsRoute route);
        [WebMethod]
        [OperationContract]
        void DeleteDomainFilter(DomainInfo id);
        [WebMethod]
        [OperationContract]
        SpamExpertsResult AddDomainFilterAlias(DomainInfo domain, string alias);
        [WebMethod]
        [OperationContract]
        void DeleteDomainFilterAlias(DomainInfo domain, string alias);
        [WebMethod]
        [OperationContract]
        SpamExpertsResult AddEmailFilter(int packageId, string username, string password, string domain);
        [WebMethod]
        [OperationContract]
        void DeleteEmailFilter(int packageId, string email);
        [WebMethod]
        [OperationContract]
        void SetEmailFilterPassword(int packageId, string email, string password);
        [WebMethod]
        [OperationContract]
        bool IsSpamExpertsEnabled(int packageId, string group);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esSpamExperts : SolidCP.EnterpriseServer.esSpamExperts, IesSpamExperts
    {
    }
}
#endif