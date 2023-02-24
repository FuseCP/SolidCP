#if Client
using System;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Server.Utils;
using Microsoft.Web.Services3;
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
    /// OCS Web Service
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
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

    public class LyncServer
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