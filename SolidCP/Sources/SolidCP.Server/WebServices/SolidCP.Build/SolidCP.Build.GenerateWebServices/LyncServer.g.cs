#if !Client
using System;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
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
    public interface ILyncServer
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
        bool CreateUser(string organizationId, string userUpn, LyncUserPlan plan);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        LyncUser GetLyncUserGeneralSettings(string organizationId, string userUpn);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool SetLyncUserGeneralSettings(string organizationId, string userUpn, LyncUser lyncUser);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool SetLyncUserPlan(string organizationId, string userUpn, LyncUserPlan plan);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool DeleteUser(string userUpn);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        LyncFederationDomain[] GetFederationDomains(string organizationId);
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
        string[] GetPolicyList(LyncPolicyType type, string name);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LyncServer : SolidCP.Server.LyncServer, ILyncServer
    {
        public new string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            return base.CreateOrganization(organizationId, sipDomain, enableConferencing, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
        }

        public new string GetOrganizationTenantId(string organizationId)
        {
            return base.GetOrganizationTenantId(organizationId);
        }

        public new bool DeleteOrganization(string organizationId, string sipDomain)
        {
            return base.DeleteOrganization(organizationId, sipDomain);
        }

        public new bool CreateUser(string organizationId, string userUpn, LyncUserPlan plan)
        {
            return base.CreateUser(organizationId, userUpn, plan);
        }

        public new LyncUser GetLyncUserGeneralSettings(string organizationId, string userUpn)
        {
            return base.GetLyncUserGeneralSettings(organizationId, userUpn);
        }

        public new bool SetLyncUserGeneralSettings(string organizationId, string userUpn, LyncUser lyncUser)
        {
            return base.SetLyncUserGeneralSettings(organizationId, userUpn, lyncUser);
        }

        public new bool SetLyncUserPlan(string organizationId, string userUpn, LyncUserPlan plan)
        {
            return base.SetLyncUserPlan(organizationId, userUpn, plan);
        }

        public new bool DeleteUser(string userUpn)
        {
            return base.DeleteUser(userUpn);
        }

        public new LyncFederationDomain[] GetFederationDomains(string organizationId)
        {
            return base.GetFederationDomains(organizationId);
        }

        public new bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn)
        {
            return base.AddFederationDomain(organizationId, domainName, proxyFqdn);
        }

        public new bool RemoveFederationDomain(string organizationId, string domainName)
        {
            return base.RemoveFederationDomain(organizationId, domainName);
        }

        public new void ReloadConfiguration()
        {
            base.ReloadConfiguration();
        }

        public new string[] GetPolicyList(LyncPolicyType type, string name)
        {
            return base.GetPolicyList(type, name);
        }
    }
}
#endif