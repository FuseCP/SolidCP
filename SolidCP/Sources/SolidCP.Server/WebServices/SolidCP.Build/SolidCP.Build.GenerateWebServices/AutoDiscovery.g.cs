#if !Client
using System.ComponentModel;
using System.Web.Services;
using SolidCP.Providers.Common;
using SolidCP.Server.Code;
using SolidCP.Server;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
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

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AutoDiscovery : SolidCP.Server.AutoDiscovery, IAutoDiscovery
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