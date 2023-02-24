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
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Services
{
    /// <summary>
    /// OCS Web Service
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
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

    public class OCSEdgeServerService : SolidCP.Server.OCSEdgeServer, IOCSEdgeServer
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