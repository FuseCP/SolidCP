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
using System.ServiceModel.Activation;

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
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SpamExperts : SolidCP.Server.SpamExperts, ISpamExperts
    {
        public new SpamExpertsResult AddDomainFilter(string domain, string password, string email, string[] destinations)
        {
            return base.AddDomainFilter(domain, password, email, destinations);
        }

        public new SpamExpertsResult AddEmailFilter(string name, string domain, string password)
        {
            return base.AddEmailFilter(name, domain, password);
        }

        public new SpamExpertsResult DeleteDomainFilter(string domain)
        {
            return base.DeleteDomainFilter(domain);
        }

        public new SpamExpertsResult DeleteEmailFilter(string email)
        {
            return base.DeleteEmailFilter(email);
        }

        public new SpamExpertsResult SetDomainFilterDestinations(string name, string[] destinations)
        {
            return base.SetDomainFilterDestinations(name, destinations);
        }

        public new SpamExpertsResult SetDomainFilterUser(string domain, string password, string email)
        {
            return base.SetDomainFilterUser(domain, password, email);
        }

        public new SpamExpertsResult SetDomainFilterUserPassword(string name, string password)
        {
            return base.SetDomainFilterUserPassword(name, password);
        }

        public new SpamExpertsResult SetEmailFilterUserPassword(string email, string password)
        {
            return base.SetEmailFilterUserPassword(email, password);
        }

        public new SpamExpertsResult AddDomainFilterAlias(string domain, string alias)
        {
            return base.AddDomainFilterAlias(domain, alias);
        }

        public new SpamExpertsResult DeleteDomainFilterAlias(string domain, string alias)
        {
            return base.DeleteDomainFilterAlias(domain, alias);
        }
    }
}
#endif