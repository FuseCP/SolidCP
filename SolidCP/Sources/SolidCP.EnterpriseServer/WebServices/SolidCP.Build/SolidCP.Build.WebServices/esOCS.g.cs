#if !Client
using SolidCP.Web.Services;
using SolidCP.EnterpriseServer.Code.HostedSolution;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
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
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [Policy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IesOCS
    {
        [WebMethod]
        [OperationContract]
        OCSUserResult CreateOCSUser(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteOCSUser(int itemId, string instanceId);
        [WebMethod]
        [OperationContract]
        OCSUsersPagedResult GetOCSUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        IntResult GetOCSUserCount(int itemId, string name, string email);
        [WebMethod]
        [OperationContract]
        OCSUser GetUserGeneralSettings(int itemId, string instanceId);
        [WebMethod]
        [OperationContract]
        void SetUserGeneralSettings(int itemId, string instanceId, bool enabledForFederation, bool enabledForPublicIMConnectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esOCS : SolidCP.EnterpriseServer.esOCS, IesOCS
    {
    }
}
#endif