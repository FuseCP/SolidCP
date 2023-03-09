#if !Client
using System;
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
    public interface ICRM
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        OrganizationResult CreateOrganization(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetSupportedCollationNames();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        Currency[] GetCurrencyList();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int[] GetInstalledLanguagePacks();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject DeleteOrganization(Guid orgId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        UserResult CreateCRMUser(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        CRMBusinessUnitsResult GetOrganizationBusinessUnits(Guid organizationId, string orgName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        CrmRolesResult GetAllCrmRoles(string orgName, Guid businessUnitId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        CrmRolesResult GetCrmUserRoles(string orgName, Guid userId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject SetUserRoles(string orgName, Guid userId, Guid[] roles);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject SetUserCALType(string orgName, Guid userId, int CALType);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        CrmUserResult GetCrmUserByDomainName(string domainName, string orgName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        CrmUserResult GetCrmUserById(Guid crmUserId, string orgName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject ChangeUserState(bool disable, string orgName, Guid crmUserId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        long GetDBSize(Guid organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        long GetMaxDBSize(Guid organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject SetMaxDBSize(Guid organizationId, long maxSize);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CRM : SolidCP.Server.CRM, ICRM
    {
        public new OrganizationResult CreateOrganization(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
        {
            return base.CreateOrganization(organizationId, organizationUniqueName, organizationFriendlyName, baseLanguageCode, ou, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, organizationCollation, maxSize);
        }

        public new string[] GetSupportedCollationNames()
        {
            return base.GetSupportedCollationNames();
        }

        public new Currency[] GetCurrencyList()
        {
            return base.GetCurrencyList();
        }

        public new int[] GetInstalledLanguagePacks()
        {
            return base.GetInstalledLanguagePacks();
        }

        public new ResultObject DeleteOrganization(Guid orgId)
        {
            return base.DeleteOrganization(orgId);
        }

        public new UserResult CreateCRMUser(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType)
        {
            return base.CreateCRMUser(user, orgName, organizationId, baseUnitId, CALType);
        }

        public new CRMBusinessUnitsResult GetOrganizationBusinessUnits(Guid organizationId, string orgName)
        {
            return base.GetOrganizationBusinessUnits(organizationId, orgName);
        }

        public new CrmRolesResult GetAllCrmRoles(string orgName, Guid businessUnitId)
        {
            return base.GetAllCrmRoles(orgName, businessUnitId);
        }

        public new CrmRolesResult GetCrmUserRoles(string orgName, Guid userId)
        {
            return base.GetCrmUserRoles(orgName, userId);
        }

        public new ResultObject SetUserRoles(string orgName, Guid userId, Guid[] roles)
        {
            return base.SetUserRoles(orgName, userId, roles);
        }

        public new ResultObject SetUserCALType(string orgName, Guid userId, int CALType)
        {
            return base.SetUserCALType(orgName, userId, CALType);
        }

        public new CrmUserResult GetCrmUserByDomainName(string domainName, string orgName)
        {
            return base.GetCrmUserByDomainName(domainName, orgName);
        }

        public new CrmUserResult GetCrmUserById(Guid crmUserId, string orgName)
        {
            return base.GetCrmUserById(crmUserId, orgName);
        }

        public new ResultObject ChangeUserState(bool disable, string orgName, Guid crmUserId)
        {
            return base.ChangeUserState(disable, orgName, crmUserId);
        }

        public new long GetDBSize(Guid organizationId)
        {
            return base.GetDBSize(organizationId);
        }

        public new long GetMaxDBSize(Guid organizationId)
        {
            return base.GetMaxDBSize(organizationId);
        }

        public new ResultObject SetMaxDBSize(Guid organizationId, long maxSize)
        {
            return base.SetMaxDBSize(organizationId, maxSize);
        }
    }
}
#endif