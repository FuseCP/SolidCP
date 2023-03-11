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
    public interface IOCSEdgeServer
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void AddDomain(string domainName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteDomain(string domainName);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OCSEdgeServer : SolidCP.Server.OCSEdgeServer, IOCSEdgeServer
    {
        public new void AddDomain(string domainName)
        {
            base.AddDomain(domainName);
        }

        public new void DeleteDomain(string domainName)
        {
            base.DeleteDomain(domainName);
        }
    }
}
#endif