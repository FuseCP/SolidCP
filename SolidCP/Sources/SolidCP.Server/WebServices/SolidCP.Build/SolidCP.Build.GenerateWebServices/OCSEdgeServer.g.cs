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
    public class OCSEdgeServerService : OCSEdgeServer, IOCSEdgeServer
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