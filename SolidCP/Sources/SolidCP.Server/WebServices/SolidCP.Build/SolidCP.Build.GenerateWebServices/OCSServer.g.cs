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
    public interface IOCSServer
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string CreateUser(string userUpn, string userDistinguishedName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        OCSUser GetUserGeneralSettings(string instanceId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetUserGeneralSettings(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteUser(string instanceId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetUserPrimaryUri(string instanceId, string userUpn);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OCSServer : SolidCP.Server.OCSServer, IOCSServer
    {
        public new string CreateUser(string userUpn, string userDistinguishedName)
        {
            return base.CreateUser(userUpn, userDistinguishedName);
        }

        public new OCSUser GetUserGeneralSettings(string instanceId)
        {
            return base.GetUserGeneralSettings(instanceId);
        }

        public new void SetUserGeneralSettings(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
        {
            base.SetUserGeneralSettings(instanceId, enabledForFederation, enabledForPublicIMConectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
        }

        public new void DeleteUser(string instanceId)
        {
            base.DeleteUser(instanceId);
        }

        public new void SetUserPrimaryUri(string instanceId, string userUpn)
        {
            base.SetUserPrimaryUri(instanceId, userUpn);
        }
    }
}
#endif