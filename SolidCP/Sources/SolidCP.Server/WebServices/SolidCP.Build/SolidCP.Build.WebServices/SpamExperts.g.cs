#if !Client
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
//using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;
using SolidCP.Providers;
using SolidCP.Providers.Filters;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface ISpamExperts
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult AddEmailFilter(string name, string domain, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult DeleteDomainFilter(string domain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult DeleteEmailFilter(string email);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult SetDomainFilterUser(string domain, string password, string email);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult SetDomainFilterUserPassword(string name, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult SetEmailFilterUserPassword(string email, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult AddDomainFilterAlias(string domain, string alias);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServcieModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class SpamExperts : SolidCP.Server.SpamExperts, ISpamExperts
    {
    }
}
#endif