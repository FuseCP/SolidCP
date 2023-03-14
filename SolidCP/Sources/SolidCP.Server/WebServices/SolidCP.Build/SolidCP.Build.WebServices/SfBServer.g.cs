#if !Client
using System;
using System.ComponentModel;
using System.Web.Services;
//using System.Web.Services.Protocols;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Server.Utils;
using Microsoft.Web.Services3;
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
    public interface ISfBServer
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string GetOrganizationTenantId(string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool DeleteOrganization(string organizationId, string sipDomain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CreateUser(string organizationId, string userUpn, SfBUserPlan plan);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SfBUser GetSfBUserGeneralSettings(string organizationId, string userUpn);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool SetSfBUserGeneralSettings(string organizationId, string userUpn, SfBUser sfbUser);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool SetSfBUserPlan(string organizationId, string userUpn, SfBUserPlan plan);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool DeleteUser(string userUpn);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        SfBFederationDomain[] GetFederationDomains(string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool RemoveFederationDomain(string organizationId, string domainName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ReloadConfiguration();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetPolicyList(SfBPolicyType type, string name);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SfBServer : SolidCP.Server.SfBServer, ISfBServer
    {
    }
}
#endif