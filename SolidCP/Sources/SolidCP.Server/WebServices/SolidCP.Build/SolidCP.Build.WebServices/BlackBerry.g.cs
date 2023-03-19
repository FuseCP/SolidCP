#if !Client
using System.ComponentModel;
using SolidCP.Web.Services;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IBlackBerry
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject CreateBlackBerryUser(string primaryEmailAddress);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject DeleteBlackBerryUser(string primaryEmailAddress);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        BlackBerryUserStatsResult GetBlackBerryUserStats(string primaryEmailAddress);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject SetActivationPasswordWithExpirationTime(string primaryEmailAddress, string password, int time);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject SetEmailActivationPassword(string primaryEmailAddress);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject DeleteDataFromBlackBerryDevice(string primaryEmailAddress);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class BlackBerry : SolidCP.Server.BlackBerry, IBlackBerry
    {
    }
}
#endif