#if !Client
using SolidCP.Web.Services;
using SolidCP.EnterpriseServer.Code.HostedSolution;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.EnterpriseServer;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.EnterpriseServer.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesBlackBerry
    {
        [WebMethod]
        [OperationContract]
        ResultObject CreateBlackBerryUser(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteBlackBerryUser(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        OrganizationUsersPagedResult GetBlackBerryUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        IntResult GetBlackBerryUserCount(int itemId, string name, string email);
        [WebMethod]
        [OperationContract]
        BlackBerryUserStatsResult GetBlackBerryUserStats(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteDataFromBlackBerryDevice(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ResultObject SetEmailActivationPassword(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ResultObject SetActivationPasswordWithExpirationTime(int itemId, int accountId, string password, int time);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esBlackBerry : SolidCP.EnterpriseServer.esBlackBerry, IesBlackBerry
    {
    }
}
#endif