#if !Client
using SolidCP.Web.Services;
using SolidCP.EnterpriseServer.Code.HostedSolution;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using System.Collections.Generic;
using SolidCP.EnterpriseServer;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.EnterpriseServer.Services
{
    // wcf service contract
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [Policy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IesLync
    {
        [WebMethod]
        [OperationContract]
        LyncUserResult CreateLyncUser(int itemId, int accountId, int lyncUserPlanId);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteLyncUser(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        LyncUsersPagedResult GetLyncUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<LyncUser> GetLyncUsersByPlanId(int itemId, int planId);
        [WebMethod]
        [OperationContract]
        IntResult GetLyncUserCount(int itemId);
        [WebMethod]
        [OperationContract]
        List<LyncUserPlan> GetLyncUserPlans(int itemId);
        [WebMethod]
        [OperationContract]
        LyncUserPlan GetLyncUserPlan(int itemId, int lyncUserPlanId);
        [WebMethod]
        [OperationContract]
        int AddLyncUserPlan(int itemId, LyncUserPlan lyncUserPlan);
        [WebMethod]
        [OperationContract]
        int UpdateLyncUserPlan(int itemId, LyncUserPlan lyncUserPlan);
        [WebMethod]
        [OperationContract]
        int DeleteLyncUserPlan(int itemId, int lyncUserPlanId);
        [WebMethod]
        [OperationContract]
        int SetOrganizationDefaultLyncUserPlan(int itemId, int lyncUserPlanId);
        [WebMethod]
        [OperationContract]
        LyncUser GetLyncUserGeneralSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        LyncUserResult SetLyncUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri);
        [WebMethod]
        [OperationContract]
        LyncUserResult SetUserLyncPlan(int itemId, int accountId, int lyncUserPlanId);
        [WebMethod]
        [OperationContract]
        LyncFederationDomain[] GetFederationDomains(int itemId);
        [WebMethod]
        [OperationContract]
        LyncUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn);
        [WebMethod]
        [OperationContract]
        LyncUserResult RemoveFederationDomain(int itemId, string domainName);
        [WebMethod]
        [OperationContract]
        string[] GetPolicyList(int itemId, LyncPolicyType type, string name);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esLync : SolidCP.EnterpriseServer.esLync, IesLync
    {
    }
}
#endif