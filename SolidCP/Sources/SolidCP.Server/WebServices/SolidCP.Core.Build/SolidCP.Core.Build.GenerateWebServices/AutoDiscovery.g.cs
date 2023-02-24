#if !Client
using System.ComponentModel;
using System.Web.Services;
using SolidCP.Providers.Common;
using SolidCP.Server.Code;
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
    /// Summary description for AutoDiscovery
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IAutoDiscovery
    {
        [WebMethod]
        [OperationContract]
        BoolResult IsInstalled(string providerName);
        [WebMethod]
        [OperationContract]
        string GetServerFilePath();
        [WebMethod]
        [OperationContract]
        string GetServerVersion();
    }

    public class AutoDiscoveryService : SolidCP.Server.AutoDiscovery, IAutoDiscovery
    {
        public new BoolResult IsInstalled(string providerName)
        {
            return base.IsInstalled(providerName);
        }

        public new string GetServerFilePath()
        {
            return base.GetServerFilePath();
        }

        public new string GetServerVersion()
        {
            return base.GetServerVersion();
        }
    }
}
#endif