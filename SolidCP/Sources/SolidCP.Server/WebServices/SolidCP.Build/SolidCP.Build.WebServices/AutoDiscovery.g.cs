#if !Client
using System.ComponentModel;
using SolidCP.Providers.Common;
using SolidCP.Server.Code;
using SolidCP.Web.Services;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

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
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class AutoDiscovery : SolidCP.Server.AutoDiscovery, IAutoDiscovery
    {
    }
}
#endif