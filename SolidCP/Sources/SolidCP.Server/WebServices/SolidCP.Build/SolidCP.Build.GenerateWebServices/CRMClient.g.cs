#if Client
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

namespace SolidCP.Server.Client
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
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
}
#endif