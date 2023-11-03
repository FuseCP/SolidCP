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
    public interface IesSfB
    {
        [WebMethod]
        [OperationContract]
        SfBUserResult CreateSfBUser(int itemId, int accountId, int sfbUserPlanId);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteSfBUser(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        SfBUsersPagedResult GetSfBUsersPaged(int itemId, string sortColumn, string sortDirection, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<SfBUser> GetSfBUsersByPlanId(int itemId, int planId);
        [WebMethod]
        [OperationContract]
        IntResult GetSfBUserCount(int itemId);
        [WebMethod]
        [OperationContract]
        List<SfBUserPlan> GetSfBUserPlans(int itemId);
        [WebMethod]
        [OperationContract]
        SfBUserPlan GetSfBUserPlan(int itemId, int sfbUserPlanId);
        [WebMethod]
        [OperationContract]
        int AddSfBUserPlan(int itemId, SfBUserPlan sfbUserPlan);
        [WebMethod]
        [OperationContract]
        int UpdateSfBUserPlan(int itemId, SfBUserPlan sfbUserPlan);
        [WebMethod]
        [OperationContract]
        int DeleteSfBUserPlan(int itemId, int sfbUserPlanId);
        [WebMethod]
        [OperationContract]
        int SetOrganizationDefaultSfBUserPlan(int itemId, int sfbUserPlanId);
        [WebMethod]
        [OperationContract]
        SfBUser GetSfBUserGeneralSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        SfBUserResult SetSfBUserGeneralSettings(int itemId, int accountId, string sipAddress, string lineUri);
        [WebMethod]
        [OperationContract]
        SfBUserResult SetUserSfBPlan(int itemId, int accountId, int sfbUserPlanId);
        [WebMethod]
        [OperationContract]
        SfBFederationDomain[] GetFederationDomains(int itemId);
        [WebMethod]
        [OperationContract]
        SfBUserResult AddFederationDomain(int itemId, string domainName, string proxyFqdn);
        [WebMethod]
        [OperationContract]
        SfBUserResult RemoveFederationDomain(int itemId, string domainName);
        [WebMethod]
        [OperationContract]
        string[] GetPolicyList(int itemId, SfBPolicyType type, string name);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esSfB : SolidCP.EnterpriseServer.esSfB, IesSfB
    {
    }
}
#endif