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

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
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
    public class BlackBerryService : BlackBerry, IBlackBerry
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