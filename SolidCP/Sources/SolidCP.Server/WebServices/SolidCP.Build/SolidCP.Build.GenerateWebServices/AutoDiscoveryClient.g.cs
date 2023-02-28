#if Client
using System.ComponentModel;
using System.Web.Services;
using SolidCP.Providers.Common;
using SolidCP.Server.Code;
using SolidCP.Server;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf service contract
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ServiceContract]
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
}
#endif