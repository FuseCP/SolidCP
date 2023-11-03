#if !Client
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SolidCP.Web.Services;
using SolidCP.EnterpriseServer.Base.HostedSolution;
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
    [ToolboxItem(false)]
    [Policy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IesOrganizations
    {
        [WebMethod]
        [OperationContract]
        bool CheckPhoneNumberIsInUse(int itemId, string phoneNumber, string userSamAccountName = null);
        [WebMethod]
        [OperationContract]
        void DeletePasswordresetAccessToken(Guid accessToken);
        [WebMethod]
        [OperationContract]
        void SetAccessTokenResponse(Guid accessToken, string response);
        [WebMethod]
        [OperationContract]
        AccessToken GetPasswordresetAccessToken(Guid token);
        [WebMethod]
        [OperationContract]
        void UpdateOrganizationGeneralSettings(int itemId, OrganizationGeneralSettings settings);
        [WebMethod]
        [OperationContract]
        OrganizationGeneralSettings GetOrganizationGeneralSettings(int itemId);
        [WebMethod]
        [OperationContract]
        void UpdateOrganizationPasswordSettings(int itemId, OrganizationPasswordSettings settings);
        [WebMethod]
        [OperationContract]
        SystemSettings GetWebDavSystemSettings();
        [WebMethod]
        [OperationContract]
        OrganizationPasswordSettings GetOrganizationPasswordSettings(int itemId);
        [WebMethod]
        [OperationContract]
        bool CheckOrgIdExists(string orgId);
        [WebMethod]
        [OperationContract]
        int CreateOrganization(int packageId, string organizationID, string organizationName, string domainName);
        [WebMethod]
        [OperationContract]
        DataSet GetRawOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<Organization> GetOrganizations(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        Organization GetOrganizationById(string organizationId);
        [WebMethod]
        [OperationContract]
        string GetOrganizationUserSummuryLetter(int itemId, int accountId, bool pmm, bool emailMode, bool signup);
        [WebMethod]
        [OperationContract]
        int SendOrganizationUserSummuryLetter(int itemId, int accountId, bool signup, string to, string cc);
        [WebMethod]
        [OperationContract]
        int DeleteOrganization(int itemId);
        [WebMethod]
        [OperationContract]
        OrganizationStatistics GetOrganizationStatistics(int itemId);
        [WebMethod]
        [OperationContract]
        OrganizationStatistics GetOrganizationStatisticsByOrganization(int itemId);
        [WebMethod]
        [OperationContract]
        Organization GetOrganization(int itemId);
        [WebMethod]
        [OperationContract]
        int GetAccountIdByUserPrincipalName(int itemId, string userPrincipalName);
        [WebMethod]
        [OperationContract]
        void SetDefaultOrganization(int newDefaultOrganizationId, int currentDefaultOrganizationId);
        [WebMethod]
        [OperationContract]
        OrganizationUser GetUserGeneralSettingsWithExtraData(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ResultObject SendResetUserPasswordLinkSms(int itemId, int accountId, string reason, string phoneTo = null);
        [WebMethod]
        [OperationContract]
        ResultObject SendResetUserPasswordPincodeSms(Guid token, string phoneTo = null);
        [WebMethod]
        [OperationContract]
        ResultObject SendResetUserPasswordPincodeEmail(Guid token, string mailTo = null);
        [WebMethod]
        [OperationContract]
        ResultObject SendUserPasswordRequestSms(int itemId, int accountId, string reason, string phoneTo);
        [WebMethod]
        [OperationContract]
        void SendUserPasswordRequestEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep);
        [WebMethod]
        [OperationContract]
        int AddOrganizationDomain(int itemId, string domainName);
        [WebMethod]
        [OperationContract]
        int ChangeOrganizationDomainType(int itemId, int domainId, ExchangeAcceptedDomainType newDomainType);
        [WebMethod]
        [OperationContract]
        List<OrganizationDomainName> GetOrganizationDomains(int itemId);
        [WebMethod]
        [OperationContract]
        int DeleteOrganizationDomain(int itemId, int domainId);
        [WebMethod]
        [OperationContract]
        int SetOrganizationDefaultDomain(int itemId, int domainId);
        [WebMethod]
        [OperationContract]
        DataSet GetOrganizationObjectsByDomain(int itemId, string domainName);
        [WebMethod]
        [OperationContract]
        bool CheckDomainUsedByHostedOrganization(int itemId, int domainId);
        [WebMethod]
        [OperationContract]
        int CreateUser(int itemId, string displayName, string name, string domain, string password, string subscriberNumber, bool sendNotification, string to);
        [WebMethod]
        [OperationContract]
        int ImportUser(int itemId, string accountName, string displayName, string name, string domain, string password, string subscriberNumber);
        [WebMethod]
        [OperationContract]
        OrganizationDeletedUsersPaged GetOrganizationDeletedUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        OrganizationUsersPaged GetOrganizationUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        OrganizationUser GetUserGeneralSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetUserGeneralSettings(int itemId, int accountId, string displayName, string password, bool hideAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, string subscriberNumber, int levelId, bool isVIP, bool userMustChangePassword);
        [WebMethod]
        [OperationContract]
        int SetUserPrincipalName(int itemId, int accountId, string userPrincipalName, bool inherit);
        [WebMethod]
        [OperationContract]
        int SetUserPassword(int itemId, int accountId, string password);
        [WebMethod]
        [OperationContract]
        List<OrganizationUser> SearchAccounts(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeMailboxes);
        [WebMethod]
        [OperationContract]
        int SetDeletedUser(int itemId, int accountId, bool enableForceArchive);
        [WebMethod]
        [OperationContract]
        byte[] GetArchiveFileBinaryChunk(int packageId, int itemId, int deleteAccountId, int offset, int length);
        [WebMethod]
        [OperationContract]
        int DeleteUser(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        PasswordPolicyResult GetPasswordPolicy(int itemId);
        [WebMethod]
        [OperationContract]
        void SendResetUserPasswordEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep);
        [WebMethod]
        [OperationContract]
        AccessToken CreatePasswordResetAccessToken(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int CreateSecurityGroup(int itemId, string displayName);
        [WebMethod]
        [OperationContract]
        OrganizationSecurityGroup GetSecurityGroupGeneralSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int DeleteSecurityGroup(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetSecurityGroupGeneralSettings(int itemId, int accountId, string displayName, string[] memberAccounts, string notes);
        [WebMethod]
        [OperationContract]
        ExchangeAccountsPaged GetOrganizationSecurityGroupsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        int AddObjectToSecurityGroup(int itemId, int accountId, string groupName);
        [WebMethod]
        [OperationContract]
        int DeleteObjectFromSecurityGroup(int itemId, int accountId, string groupName);
        [WebMethod]
        [OperationContract]
        ExchangeAccount[] GetSecurityGroupsByMember(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        List<ExchangeAccount> SearchOrganizationAccounts(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeOnlySecurityGroups);
        [WebMethod]
        [OperationContract]
        ExchangeAccount[] GetUserGroups(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        List<AdditionalGroup> GetAdditionalGroups(int userId);
        [WebMethod]
        [OperationContract]
        void UpdateAdditionalGroup(int groupId, string groupName);
        [WebMethod]
        [OperationContract]
        void DeleteAdditionalGroup(int groupId);
        [WebMethod]
        [OperationContract]
        int AddAdditionalGroup(int userId, string groupName);
        [WebMethod]
        [OperationContract]
        ServiceLevel[] GetSupportServiceLevels();
        [WebMethod]
        [OperationContract]
        void UpdateSupportServiceLevel(int levelID, string levelName, string levelDescription);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteSupportServiceLevel(int levelId);
        [WebMethod]
        [OperationContract]
        int AddSupportServiceLevel(string levelName, string levelDescription);
        [WebMethod]
        [OperationContract]
        ServiceLevel GetSupportServiceLevel(int levelID);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esOrganizations : SolidCP.EnterpriseServer.esOrganizations, IesOrganizations
    {
    }
}
#endif