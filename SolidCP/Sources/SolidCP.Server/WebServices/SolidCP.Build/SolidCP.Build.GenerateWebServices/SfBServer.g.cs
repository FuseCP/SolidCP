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

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
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
    public class SfBServerService : SfBServer, ISfBServer
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

        public new bool CreateUser(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return base.CreateUser(organizationId, userUpn, plan);
        }

        public new SfBUser GetSfBUserGeneralSettings(string organizationId, string userUpn)
        {
            return base.GetSfBUserGeneralSettings(organizationId, userUpn);
        }

        public new bool SetSfBUserGeneralSettings(string organizationId, string userUpn, SfBUser sfbUser)
        {
            return base.SetSfBUserGeneralSettings(organizationId, userUpn, sfbUser);
        }

        public new bool SetSfBUserPlan(string organizationId, string userUpn, SfBUserPlan plan)
        {
            return base.SetSfBUserPlan(organizationId, userUpn, plan);
        }

        public new bool DeleteUser(string userUpn)
        {
            return base.DeleteUser(userUpn);
        }

        public new SfBFederationDomain[] GetFederationDomains(string organizationId)
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

        public new string[] GetPolicyList(SfBPolicyType type, string name)
        {
            return base.GetPolicyList(type, name);
        }
    }
}
#endif