#if !Client
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
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
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BlackBerry : SolidCP.Server.BlackBerry, IBlackBerry
    {
        public new ResultObject CreateBlackBerryUser(string primaryEmailAddress)
        {
            return base.CreateBlackBerryUser(primaryEmailAddress);
        }

        public new ResultObject DeleteBlackBerryUser(string primaryEmailAddress)
        {
            return base.DeleteBlackBerryUser(primaryEmailAddress);
        }

        public new BlackBerryUserStatsResult GetBlackBerryUserStats(string primaryEmailAddress)
        {
            return base.GetBlackBerryUserStats(primaryEmailAddress);
        }

        public new ResultObject SetActivationPasswordWithExpirationTime(string primaryEmailAddress, string password, int time)
        {
            return base.SetActivationPasswordWithExpirationTime(primaryEmailAddress, password, time);
        }

        public new ResultObject SetEmailActivationPassword(string primaryEmailAddress)
        {
            return base.SetEmailActivationPassword(primaryEmailAddress);
        }

        public new ResultObject DeleteDataFromBlackBerryDevice(string primaryEmailAddress)
        {
            return base.DeleteDataFromBlackBerryDevice(primaryEmailAddress);
        }
    }
}
#endif