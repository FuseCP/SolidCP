#if !Client
using System;
using System.ComponentModel;
using SolidCP.Web.Services;
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
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("EnterpriseServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesCRM
    {
        [WebMethod]
        [OperationContract]
        OrganizationResult CreateOrganization(int organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string regionName, int userId, string collation, int baseLanguageCode);
        [WebMethod]
        [OperationContract]
        StringArrayResultObject GetCollation(int packageId);
        [WebMethod]
        [OperationContract]
        StringArrayResultObject GetCollationByServiceId(int serviceId);
        [WebMethod]
        [OperationContract]
        CurrencyArrayResultObject GetCurrency(int packageId);
        [WebMethod]
        [OperationContract]
        CurrencyArrayResultObject GetCurrencyByServiceId(int serviceId);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteCRMOrganization(int organizationId);
        [WebMethod]
        [OperationContract]
        OrganizationUsersPagedResult GetCRMUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        IntResult GetCRMUserCount(int itemId, string name, string email, int CALType);
        [WebMethod]
        [OperationContract]
        UserResult CreateCRMUser(OrganizationUser user, int packageId, int itemId, Guid businessUnitOrgId, int CALType);
        [WebMethod]
        [OperationContract]
        CRMBusinessUnitsResult GetBusinessUnits(int itemId, int packageId);
        [WebMethod]
        [OperationContract]
        CrmRolesResult GetCrmRoles(int itemId, int accountId, int packageId);
        [WebMethod]
        [OperationContract]
        ResultObject SetUserRoles(int itemId, int accountId, int packageId, Guid[] roles);
        [WebMethod]
        [OperationContract]
        ResultObject SetUserCALType(int itemId, int accountId, int packageId, int CALType);
        [WebMethod]
        [OperationContract]
        ResultObject ChangeUserState(int itemId, int accountId, bool disable);
        [WebMethod]
        [OperationContract]
        CrmUserResult GetCrmUser(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ResultObject SetMaxDBSize(int itemId, int packageId, long maxSize);
        [WebMethod]
        [OperationContract]
        long GetDBSize(int itemId, int packageId);
        [WebMethod]
        [OperationContract]
        long GetMaxDBSize(int itemId, int packageId);
        [WebMethod]
        [OperationContract]
        int[] GetInstalledLanguagePacks(int packageId);
        [WebMethod]
        [OperationContract]
        int[] GetInstalledLanguagePacksByServiceId(int serviceId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esCRM : SolidCP.EnterpriseServer.esCRM, IesCRM
    {
    }
}
#endif