#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesOrganizations", Namespace = "http://tempuri.org/")]
    public interface IesOrganizations
    {
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CheckPhoneNumberIsInUse", ReplyAction = "http://tempuri.org/IesOrganizations/CheckPhoneNumberIsInUseResponse")]
        bool CheckPhoneNumberIsInUse(int itemId, string phoneNumber, string userSamAccountName = null);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CheckPhoneNumberIsInUse", ReplyAction = "http://tempuri.org/IesOrganizations/CheckPhoneNumberIsInUseResponse")]
        System.Threading.Tasks.Task<bool> CheckPhoneNumberIsInUseAsync(int itemId, string phoneNumber, string userSamAccountName = null);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeletePasswordresetAccessToken", ReplyAction = "http://tempuri.org/IesOrganizations/DeletePasswordresetAccessTokenResponse")]
        void DeletePasswordresetAccessToken(System.Guid accessToken);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeletePasswordresetAccessToken", ReplyAction = "http://tempuri.org/IesOrganizations/DeletePasswordresetAccessTokenResponse")]
        System.Threading.Tasks.Task DeletePasswordresetAccessTokenAsync(System.Guid accessToken);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetAccessTokenResponse", ReplyAction = "http://tempuri.org/IesOrganizations/SetAccessTokenResponseResponse")]
        void SetAccessTokenResponse(System.Guid accessToken, string response);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetAccessTokenResponse", ReplyAction = "http://tempuri.org/IesOrganizations/SetAccessTokenResponseResponse")]
        System.Threading.Tasks.Task SetAccessTokenResponseAsync(System.Guid accessToken, string response);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetPasswordresetAccessToken", ReplyAction = "http://tempuri.org/IesOrganizations/GetPasswordresetAccessTokenResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken GetPasswordresetAccessToken(System.Guid token);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetPasswordresetAccessToken", ReplyAction = "http://tempuri.org/IesOrganizations/GetPasswordresetAccessTokenResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken> GetPasswordresetAccessTokenAsync(System.Guid token);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/UpdateOrganizationGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/UpdateOrganizationGeneralSettingsResponse")]
        void UpdateOrganizationGeneralSettings(int itemId, SolidCP.Providers.HostedSolution.OrganizationGeneralSettings settings);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/UpdateOrganizationGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/UpdateOrganizationGeneralSettingsResponse")]
        System.Threading.Tasks.Task UpdateOrganizationGeneralSettingsAsync(int itemId, SolidCP.Providers.HostedSolution.OrganizationGeneralSettings settings);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.OrganizationGeneralSettings GetOrganizationGeneralSettings(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationGeneralSettings> GetOrganizationGeneralSettingsAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/UpdateOrganizationPasswordSettings", ReplyAction = "http://tempuri.org/IesOrganizations/UpdateOrganizationPasswordSettingsResponse")]
        void UpdateOrganizationPasswordSettings(int itemId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings settings);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/UpdateOrganizationPasswordSettings", ReplyAction = "http://tempuri.org/IesOrganizations/UpdateOrganizationPasswordSettingsResponse")]
        System.Threading.Tasks.Task UpdateOrganizationPasswordSettingsAsync(int itemId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings settings);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetWebDavSystemSettings", ReplyAction = "http://tempuri.org/IesOrganizations/GetWebDavSystemSettingsResponse")]
        SolidCP.EnterpriseServer.SystemSettings GetWebDavSystemSettings();
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetWebDavSystemSettings", ReplyAction = "http://tempuri.org/IesOrganizations/GetWebDavSystemSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetWebDavSystemSettingsAsync();
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationPasswordSettings", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationPasswordSettingsResponse")]
        SolidCP.Providers.HostedSolution.OrganizationPasswordSettings GetOrganizationPasswordSettings(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationPasswordSettings", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationPasswordSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationPasswordSettings> GetOrganizationPasswordSettingsAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CheckOrgIdExists", ReplyAction = "http://tempuri.org/IesOrganizations/CheckOrgIdExistsResponse")]
        bool CheckOrgIdExists(string orgId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CheckOrgIdExists", ReplyAction = "http://tempuri.org/IesOrganizations/CheckOrgIdExistsResponse")]
        System.Threading.Tasks.Task<bool> CheckOrgIdExistsAsync(string orgId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CreateOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/CreateOrganizationResponse")]
        int CreateOrganization(int packageId, string organizationID, string organizationName, string domainName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CreateOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/CreateOrganizationResponse")]
        System.Threading.Tasks.Task<int> CreateOrganizationAsync(int packageId, string organizationID, string organizationName, string domainName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetRawOrganizationsPaged", ReplyAction = "http://tempuri.org/IesOrganizations/GetRawOrganizationsPagedResponse")]
        System.Data.DataSet GetRawOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetRawOrganizationsPaged", ReplyAction = "http://tempuri.org/IesOrganizations/GetRawOrganizationsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawOrganizationsPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizations", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationsResponse")]
        SolidCP.Providers.HostedSolution.Organization[] /*List*/ GetOrganizations(int packageId, bool recursive);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizations", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization[]> GetOrganizationsAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationById", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationByIdResponse")]
        SolidCP.Providers.HostedSolution.Organization GetOrganizationById(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationById", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationByIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationByIdAsync(string organizationId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationUserSummuryLetter", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationUserSummuryLetterResponse")]
        string GetOrganizationUserSummuryLetter(int itemId, int accountId, bool pmm, bool emailMode, bool signup);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationUserSummuryLetter", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationUserSummuryLetterResponse")]
        System.Threading.Tasks.Task<string> GetOrganizationUserSummuryLetterAsync(int itemId, int accountId, bool pmm, bool emailMode, bool signup);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendOrganizationUserSummuryLetter", ReplyAction = "http://tempuri.org/IesOrganizations/SendOrganizationUserSummuryLetterResponse")]
        int SendOrganizationUserSummuryLetter(int itemId, int accountId, bool signup, string to, string cc);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendOrganizationUserSummuryLetter", ReplyAction = "http://tempuri.org/IesOrganizations/SendOrganizationUserSummuryLetterResponse")]
        System.Threading.Tasks.Task<int> SendOrganizationUserSummuryLetterAsync(int itemId, int accountId, bool signup, string to, string cc);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteOrganizationResponse")]
        int DeleteOrganization(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteOrganizationResponse")]
        System.Threading.Tasks.Task<int> DeleteOrganizationAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationStatistics", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationStatisticsResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetOrganizationStatistics(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationStatistics", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationStatisticsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetOrganizationStatisticsAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationStatisticsByOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationStatisticsByOrganizationResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetOrganizationStatisticsByOrganization(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationStatisticsByOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationStatisticsByOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetOrganizationStatisticsByOrganizationAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationResponse")]
        SolidCP.Providers.HostedSolution.Organization GetOrganization(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetAccountIdByUserPrincipalName", ReplyAction = "http://tempuri.org/IesOrganizations/GetAccountIdByUserPrincipalNameResponse")]
        int GetAccountIdByUserPrincipalName(int itemId, string userPrincipalName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetAccountIdByUserPrincipalName", ReplyAction = "http://tempuri.org/IesOrganizations/GetAccountIdByUserPrincipalNameResponse")]
        System.Threading.Tasks.Task<int> GetAccountIdByUserPrincipalNameAsync(int itemId, string userPrincipalName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetDefaultOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/SetDefaultOrganizationResponse")]
        void SetDefaultOrganization(int newDefaultOrganizationId, int currentDefaultOrganizationId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetDefaultOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/SetDefaultOrganizationResponse")]
        System.Threading.Tasks.Task SetDefaultOrganizationAsync(int newDefaultOrganizationId, int currentDefaultOrganizationId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetUserGeneralSettingsWithExtraData", ReplyAction = "http://tempuri.org/IesOrganizations/GetUserGeneralSettingsWithExtraDataResponse")]
        SolidCP.Providers.HostedSolution.OrganizationUser GetUserGeneralSettingsWithExtraData(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetUserGeneralSettingsWithExtraData", ReplyAction = "http://tempuri.org/IesOrganizations/GetUserGeneralSettingsWithExtraDataResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetUserGeneralSettingsWithExtraDataAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendResetUserPasswordLinkSms", ReplyAction = "http://tempuri.org/IesOrganizations/SendResetUserPasswordLinkSmsResponse")]
        SolidCP.Providers.Common.ResultObject SendResetUserPasswordLinkSms(int itemId, int accountId, string reason, string phoneTo = null);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendResetUserPasswordLinkSms", ReplyAction = "http://tempuri.org/IesOrganizations/SendResetUserPasswordLinkSmsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendResetUserPasswordLinkSmsAsync(int itemId, int accountId, string reason, string phoneTo = null);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendResetUserPasswordPincodeSms", ReplyAction = "http://tempuri.org/IesOrganizations/SendResetUserPasswordPincodeSmsResponse")]
        SolidCP.Providers.Common.ResultObject SendResetUserPasswordPincodeSms(System.Guid token, string phoneTo = null);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendResetUserPasswordPincodeSms", ReplyAction = "http://tempuri.org/IesOrganizations/SendResetUserPasswordPincodeSmsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendResetUserPasswordPincodeSmsAsync(System.Guid token, string phoneTo = null);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendResetUserPasswordPincodeEmail", ReplyAction = "http://tempuri.org/IesOrganizations/SendResetUserPasswordPincodeEmailResponse")]
        SolidCP.Providers.Common.ResultObject SendResetUserPasswordPincodeEmail(System.Guid token, string mailTo = null);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendResetUserPasswordPincodeEmail", ReplyAction = "http://tempuri.org/IesOrganizations/SendResetUserPasswordPincodeEmailResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendResetUserPasswordPincodeEmailAsync(System.Guid token, string mailTo = null);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendUserPasswordRequestSms", ReplyAction = "http://tempuri.org/IesOrganizations/SendUserPasswordRequestSmsResponse")]
        SolidCP.Providers.Common.ResultObject SendUserPasswordRequestSms(int itemId, int accountId, string reason, string phoneTo);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendUserPasswordRequestSms", ReplyAction = "http://tempuri.org/IesOrganizations/SendUserPasswordRequestSmsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendUserPasswordRequestSmsAsync(int itemId, int accountId, string reason, string phoneTo);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendUserPasswordRequestEmail", ReplyAction = "http://tempuri.org/IesOrganizations/SendUserPasswordRequestEmailResponse")]
        void SendUserPasswordRequestEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendUserPasswordRequestEmail", ReplyAction = "http://tempuri.org/IesOrganizations/SendUserPasswordRequestEmailResponse")]
        System.Threading.Tasks.Task SendUserPasswordRequestEmailAsync(int itemId, int accountId, string reason, string mailTo, bool finalStep);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/AddOrganizationDomain", ReplyAction = "http://tempuri.org/IesOrganizations/AddOrganizationDomainResponse")]
        int AddOrganizationDomain(int itemId, string domainName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/AddOrganizationDomain", ReplyAction = "http://tempuri.org/IesOrganizations/AddOrganizationDomainResponse")]
        System.Threading.Tasks.Task<int> AddOrganizationDomainAsync(int itemId, string domainName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/ChangeOrganizationDomainType", ReplyAction = "http://tempuri.org/IesOrganizations/ChangeOrganizationDomainTypeResponse")]
        int ChangeOrganizationDomainType(int itemId, int domainId, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType newDomainType);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/ChangeOrganizationDomainType", ReplyAction = "http://tempuri.org/IesOrganizations/ChangeOrganizationDomainTypeResponse")]
        System.Threading.Tasks.Task<int> ChangeOrganizationDomainTypeAsync(int itemId, int domainId, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType newDomainType);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationDomains", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationDomainsResponse")]
        SolidCP.Providers.HostedSolution.OrganizationDomainName[] /*List*/ GetOrganizationDomains(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationDomains", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationDomainsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationDomainName[]> GetOrganizationDomainsAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteOrganizationDomain", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteOrganizationDomainResponse")]
        int DeleteOrganizationDomain(int itemId, int domainId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteOrganizationDomain", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteOrganizationDomainResponse")]
        System.Threading.Tasks.Task<int> DeleteOrganizationDomainAsync(int itemId, int domainId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetOrganizationDefaultDomain", ReplyAction = "http://tempuri.org/IesOrganizations/SetOrganizationDefaultDomainResponse")]
        int SetOrganizationDefaultDomain(int itemId, int domainId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetOrganizationDefaultDomain", ReplyAction = "http://tempuri.org/IesOrganizations/SetOrganizationDefaultDomainResponse")]
        System.Threading.Tasks.Task<int> SetOrganizationDefaultDomainAsync(int itemId, int domainId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationObjectsByDomain", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationObjectsByDomainResponse")]
        System.Data.DataSet GetOrganizationObjectsByDomain(int itemId, string domainName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationObjectsByDomain", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationObjectsByDomainResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetOrganizationObjectsByDomainAsync(int itemId, string domainName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CheckDomainUsedByHostedOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/CheckDomainUsedByHostedOrganizationResponse")]
        bool CheckDomainUsedByHostedOrganization(int itemId, int domainId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CheckDomainUsedByHostedOrganization", ReplyAction = "http://tempuri.org/IesOrganizations/CheckDomainUsedByHostedOrganizationResponse")]
        System.Threading.Tasks.Task<bool> CheckDomainUsedByHostedOrganizationAsync(int itemId, int domainId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CreateUser", ReplyAction = "http://tempuri.org/IesOrganizations/CreateUserResponse")]
        int CreateUser(int itemId, string displayName, string name, string domain, string password, string subscriberNumber, bool sendNotification, string to);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CreateUser", ReplyAction = "http://tempuri.org/IesOrganizations/CreateUserResponse")]
        System.Threading.Tasks.Task<int> CreateUserAsync(int itemId, string displayName, string name, string domain, string password, string subscriberNumber, bool sendNotification, string to);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/ImportUser", ReplyAction = "http://tempuri.org/IesOrganizations/ImportUserResponse")]
        int ImportUser(int itemId, string accountName, string displayName, string name, string domain, string password, string subscriberNumber);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/ImportUser", ReplyAction = "http://tempuri.org/IesOrganizations/ImportUserResponse")]
        System.Threading.Tasks.Task<int> ImportUserAsync(int itemId, string accountName, string displayName, string name, string domain, string password, string subscriberNumber);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationDeletedUsersPaged", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationDeletedUsersPagedResponse")]
        SolidCP.Providers.HostedSolution.OrganizationDeletedUsersPaged GetOrganizationDeletedUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationDeletedUsersPaged", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationDeletedUsersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationDeletedUsersPaged> GetOrganizationDeletedUsersPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationUsersPaged", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationUsersPagedResponse")]
        SolidCP.Providers.HostedSolution.OrganizationUsersPaged GetOrganizationUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationUsersPaged", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationUsersPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUsersPaged> GetOrganizationUsersPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetUserGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/GetUserGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.OrganizationUser GetUserGeneralSettings(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetUserGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/GetUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetUserGeneralSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetUserGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/SetUserGeneralSettingsResponse")]
        int SetUserGeneralSettings(int itemId, int accountId, string displayName, string password, bool hideAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, string subscriberNumber, int levelId, bool isVIP, bool userMustChangePassword);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetUserGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/SetUserGeneralSettingsResponse")]
        System.Threading.Tasks.Task<int> SetUserGeneralSettingsAsync(int itemId, int accountId, string displayName, string password, bool hideAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, string subscriberNumber, int levelId, bool isVIP, bool userMustChangePassword);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetUserPrincipalName", ReplyAction = "http://tempuri.org/IesOrganizations/SetUserPrincipalNameResponse")]
        int SetUserPrincipalName(int itemId, int accountId, string userPrincipalName, bool inherit);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetUserPrincipalName", ReplyAction = "http://tempuri.org/IesOrganizations/SetUserPrincipalNameResponse")]
        System.Threading.Tasks.Task<int> SetUserPrincipalNameAsync(int itemId, int accountId, string userPrincipalName, bool inherit);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetUserPassword", ReplyAction = "http://tempuri.org/IesOrganizations/SetUserPasswordResponse")]
        int SetUserPassword(int itemId, int accountId, string password);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetUserPassword", ReplyAction = "http://tempuri.org/IesOrganizations/SetUserPasswordResponse")]
        System.Threading.Tasks.Task<int> SetUserPasswordAsync(int itemId, int accountId, string password);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SearchAccounts", ReplyAction = "http://tempuri.org/IesOrganizations/SearchAccountsResponse")]
        SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ SearchAccounts(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeMailboxes);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SearchAccounts", ReplyAction = "http://tempuri.org/IesOrganizations/SearchAccountsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> SearchAccountsAsync(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeMailboxes);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetDeletedUser", ReplyAction = "http://tempuri.org/IesOrganizations/SetDeletedUserResponse")]
        int SetDeletedUser(int itemId, int accountId, bool enableForceArchive);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetDeletedUser", ReplyAction = "http://tempuri.org/IesOrganizations/SetDeletedUserResponse")]
        System.Threading.Tasks.Task<int> SetDeletedUserAsync(int itemId, int accountId, bool enableForceArchive);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetArchiveFileBinaryChunk", ReplyAction = "http://tempuri.org/IesOrganizations/GetArchiveFileBinaryChunkResponse")]
        byte[] GetArchiveFileBinaryChunk(int packageId, int itemId, int deleteAccountId, int offset, int length);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetArchiveFileBinaryChunk", ReplyAction = "http://tempuri.org/IesOrganizations/GetArchiveFileBinaryChunkResponse")]
        System.Threading.Tasks.Task<byte[]> GetArchiveFileBinaryChunkAsync(int packageId, int itemId, int deleteAccountId, int offset, int length);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteUser", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteUserResponse")]
        int DeleteUser(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteUser", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteUserResponse")]
        System.Threading.Tasks.Task<int> DeleteUserAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetPasswordPolicy", ReplyAction = "http://tempuri.org/IesOrganizations/GetPasswordPolicyResponse")]
        SolidCP.Providers.ResultObjects.PasswordPolicyResult GetPasswordPolicy(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetPasswordPolicy", ReplyAction = "http://tempuri.org/IesOrganizations/GetPasswordPolicyResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.PasswordPolicyResult> GetPasswordPolicyAsync(int itemId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendResetUserPasswordEmail", ReplyAction = "http://tempuri.org/IesOrganizations/SendResetUserPasswordEmailResponse")]
        void SendResetUserPasswordEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SendResetUserPasswordEmail", ReplyAction = "http://tempuri.org/IesOrganizations/SendResetUserPasswordEmailResponse")]
        System.Threading.Tasks.Task SendResetUserPasswordEmailAsync(int itemId, int accountId, string reason, string mailTo, bool finalStep);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CreatePasswordResetAccessToken", ReplyAction = "http://tempuri.org/IesOrganizations/CreatePasswordResetAccessTokenResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken CreatePasswordResetAccessToken(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CreatePasswordResetAccessToken", ReplyAction = "http://tempuri.org/IesOrganizations/CreatePasswordResetAccessTokenResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken> CreatePasswordResetAccessTokenAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CreateSecurityGroup", ReplyAction = "http://tempuri.org/IesOrganizations/CreateSecurityGroupResponse")]
        int CreateSecurityGroup(int itemId, string displayName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/CreateSecurityGroup", ReplyAction = "http://tempuri.org/IesOrganizations/CreateSecurityGroupResponse")]
        System.Threading.Tasks.Task<int> CreateSecurityGroupAsync(int itemId, string displayName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetSecurityGroupGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/GetSecurityGroupGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.OrganizationSecurityGroup GetSecurityGroupGeneralSettings(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetSecurityGroupGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/GetSecurityGroupGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationSecurityGroup> GetSecurityGroupGeneralSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteSecurityGroup", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteSecurityGroupResponse")]
        int DeleteSecurityGroup(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteSecurityGroup", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteSecurityGroupResponse")]
        System.Threading.Tasks.Task<int> DeleteSecurityGroupAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetSecurityGroupGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/SetSecurityGroupGeneralSettingsResponse")]
        int SetSecurityGroupGeneralSettings(int itemId, int accountId, string displayName, string[] memberAccounts, string notes);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SetSecurityGroupGeneralSettings", ReplyAction = "http://tempuri.org/IesOrganizations/SetSecurityGroupGeneralSettingsResponse")]
        System.Threading.Tasks.Task<int> SetSecurityGroupGeneralSettingsAsync(int itemId, int accountId, string displayName, string[] memberAccounts, string notes);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationSecurityGroupsPaged", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationSecurityGroupsPagedResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccountsPaged GetOrganizationSecurityGroupsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetOrganizationSecurityGroupsPaged", ReplyAction = "http://tempuri.org/IesOrganizations/GetOrganizationSecurityGroupsPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccountsPaged> GetOrganizationSecurityGroupsPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/AddObjectToSecurityGroup", ReplyAction = "http://tempuri.org/IesOrganizations/AddObjectToSecurityGroupResponse")]
        int AddObjectToSecurityGroup(int itemId, int accountId, string groupName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/AddObjectToSecurityGroup", ReplyAction = "http://tempuri.org/IesOrganizations/AddObjectToSecurityGroupResponse")]
        System.Threading.Tasks.Task<int> AddObjectToSecurityGroupAsync(int itemId, int accountId, string groupName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteObjectFromSecurityGroup", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteObjectFromSecurityGroupResponse")]
        int DeleteObjectFromSecurityGroup(int itemId, int accountId, string groupName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteObjectFromSecurityGroup", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteObjectFromSecurityGroupResponse")]
        System.Threading.Tasks.Task<int> DeleteObjectFromSecurityGroupAsync(int itemId, int accountId, string groupName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetSecurityGroupsByMember", ReplyAction = "http://tempuri.org/IesOrganizations/GetSecurityGroupsByMemberResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount[] GetSecurityGroupsByMember(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetSecurityGroupsByMember", ReplyAction = "http://tempuri.org/IesOrganizations/GetSecurityGroupsByMemberResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetSecurityGroupsByMemberAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SearchOrganizationAccounts", ReplyAction = "http://tempuri.org/IesOrganizations/SearchOrganizationAccountsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchOrganizationAccounts(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeOnlySecurityGroups);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/SearchOrganizationAccounts", ReplyAction = "http://tempuri.org/IesOrganizations/SearchOrganizationAccountsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchOrganizationAccountsAsync(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeOnlySecurityGroups);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetUserGroups", ReplyAction = "http://tempuri.org/IesOrganizations/GetUserGroupsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount[] GetUserGroups(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetUserGroups", ReplyAction = "http://tempuri.org/IesOrganizations/GetUserGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetUserGroupsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetAdditionalGroups", ReplyAction = "http://tempuri.org/IesOrganizations/GetAdditionalGroupsResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.AdditionalGroup[] /*List*/ GetAdditionalGroups(int userId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetAdditionalGroups", ReplyAction = "http://tempuri.org/IesOrganizations/GetAdditionalGroupsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.AdditionalGroup[]> GetAdditionalGroupsAsync(int userId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/UpdateAdditionalGroup", ReplyAction = "http://tempuri.org/IesOrganizations/UpdateAdditionalGroupResponse")]
        void UpdateAdditionalGroup(int groupId, string groupName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/UpdateAdditionalGroup", ReplyAction = "http://tempuri.org/IesOrganizations/UpdateAdditionalGroupResponse")]
        System.Threading.Tasks.Task UpdateAdditionalGroupAsync(int groupId, string groupName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteAdditionalGroup", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteAdditionalGroupResponse")]
        void DeleteAdditionalGroup(int groupId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteAdditionalGroup", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteAdditionalGroupResponse")]
        System.Threading.Tasks.Task DeleteAdditionalGroupAsync(int groupId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/AddAdditionalGroup", ReplyAction = "http://tempuri.org/IesOrganizations/AddAdditionalGroupResponse")]
        int AddAdditionalGroup(int userId, string groupName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/AddAdditionalGroup", ReplyAction = "http://tempuri.org/IesOrganizations/AddAdditionalGroupResponse")]
        System.Threading.Tasks.Task<int> AddAdditionalGroupAsync(int userId, string groupName);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetSupportServiceLevels", ReplyAction = "http://tempuri.org/IesOrganizations/GetSupportServiceLevelsResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel[] GetSupportServiceLevels();
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetSupportServiceLevels", ReplyAction = "http://tempuri.org/IesOrganizations/GetSupportServiceLevelsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel[]> GetSupportServiceLevelsAsync();
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/UpdateSupportServiceLevel", ReplyAction = "http://tempuri.org/IesOrganizations/UpdateSupportServiceLevelResponse")]
        void UpdateSupportServiceLevel(int levelID, string levelName, string levelDescription);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/UpdateSupportServiceLevel", ReplyAction = "http://tempuri.org/IesOrganizations/UpdateSupportServiceLevelResponse")]
        System.Threading.Tasks.Task UpdateSupportServiceLevelAsync(int levelID, string levelName, string levelDescription);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteSupportServiceLevel", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteSupportServiceLevelResponse")]
        SolidCP.Providers.Common.ResultObject DeleteSupportServiceLevel(int levelId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/DeleteSupportServiceLevel", ReplyAction = "http://tempuri.org/IesOrganizations/DeleteSupportServiceLevelResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSupportServiceLevelAsync(int levelId);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/AddSupportServiceLevel", ReplyAction = "http://tempuri.org/IesOrganizations/AddSupportServiceLevelResponse")]
        int AddSupportServiceLevel(string levelName, string levelDescription);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/AddSupportServiceLevel", ReplyAction = "http://tempuri.org/IesOrganizations/AddSupportServiceLevelResponse")]
        System.Threading.Tasks.Task<int> AddSupportServiceLevelAsync(string levelName, string levelDescription);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetSupportServiceLevel", ReplyAction = "http://tempuri.org/IesOrganizations/GetSupportServiceLevelResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel GetSupportServiceLevel(int levelID);
        [OperationContract(Action = "http://tempuri.org/IesOrganizations/GetSupportServiceLevel", ReplyAction = "http://tempuri.org/IesOrganizations/GetSupportServiceLevelResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel> GetSupportServiceLevelAsync(int levelID);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esOrganizationsAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesOrganizations
    {
        public bool CheckPhoneNumberIsInUse(int itemId, string phoneNumber, string userSamAccountName = null)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esOrganizations", "CheckPhoneNumberIsInUse", itemId, phoneNumber, userSamAccountName);
        }

        public async System.Threading.Tasks.Task<bool> CheckPhoneNumberIsInUseAsync(int itemId, string phoneNumber, string userSamAccountName = null)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esOrganizations", "CheckPhoneNumberIsInUse", itemId, phoneNumber, userSamAccountName);
        }

        public void DeletePasswordresetAccessToken(System.Guid accessToken)
        {
            Invoke("SolidCP.EnterpriseServer.esOrganizations", "DeletePasswordresetAccessToken", accessToken);
        }

        public async System.Threading.Tasks.Task DeletePasswordresetAccessTokenAsync(System.Guid accessToken)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esOrganizations", "DeletePasswordresetAccessToken", accessToken);
        }

        public void SetAccessTokenResponse(System.Guid accessToken, string response)
        {
            Invoke("SolidCP.EnterpriseServer.esOrganizations", "SetAccessTokenResponse", accessToken, response);
        }

        public async System.Threading.Tasks.Task SetAccessTokenResponseAsync(System.Guid accessToken, string response)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esOrganizations", "SetAccessTokenResponse", accessToken, response);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken GetPasswordresetAccessToken(System.Guid token)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken>("SolidCP.EnterpriseServer.esOrganizations", "GetPasswordresetAccessToken", token);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken> GetPasswordresetAccessTokenAsync(System.Guid token)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken>("SolidCP.EnterpriseServer.esOrganizations", "GetPasswordresetAccessToken", token);
        }

        public void UpdateOrganizationGeneralSettings(int itemId, SolidCP.Providers.HostedSolution.OrganizationGeneralSettings settings)
        {
            Invoke("SolidCP.EnterpriseServer.esOrganizations", "UpdateOrganizationGeneralSettings", itemId, settings);
        }

        public async System.Threading.Tasks.Task UpdateOrganizationGeneralSettingsAsync(int itemId, SolidCP.Providers.HostedSolution.OrganizationGeneralSettings settings)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esOrganizations", "UpdateOrganizationGeneralSettings", itemId, settings);
        }

        public SolidCP.Providers.HostedSolution.OrganizationGeneralSettings GetOrganizationGeneralSettings(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationGeneralSettings>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationGeneralSettings", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationGeneralSettings> GetOrganizationGeneralSettingsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationGeneralSettings>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationGeneralSettings", itemId);
        }

        public void UpdateOrganizationPasswordSettings(int itemId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings settings)
        {
            Invoke("SolidCP.EnterpriseServer.esOrganizations", "UpdateOrganizationPasswordSettings", itemId, settings);
        }

        public async System.Threading.Tasks.Task UpdateOrganizationPasswordSettingsAsync(int itemId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings settings)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esOrganizations", "UpdateOrganizationPasswordSettings", itemId, settings);
        }

        public SolidCP.EnterpriseServer.SystemSettings GetWebDavSystemSettings()
        {
            return Invoke<SolidCP.EnterpriseServer.SystemSettings>("SolidCP.EnterpriseServer.esOrganizations", "GetWebDavSystemSettings");
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetWebDavSystemSettingsAsync()
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.SystemSettings>("SolidCP.EnterpriseServer.esOrganizations", "GetWebDavSystemSettings");
        }

        public SolidCP.Providers.HostedSolution.OrganizationPasswordSettings GetOrganizationPasswordSettings(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationPasswordSettings>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationPasswordSettings", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationPasswordSettings> GetOrganizationPasswordSettingsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationPasswordSettings>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationPasswordSettings", itemId);
        }

        public bool CheckOrgIdExists(string orgId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esOrganizations", "CheckOrgIdExists", orgId);
        }

        public async System.Threading.Tasks.Task<bool> CheckOrgIdExistsAsync(string orgId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esOrganizations", "CheckOrgIdExists", orgId);
        }

        public int CreateOrganization(int packageId, string organizationID, string organizationName, string domainName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "CreateOrganization", packageId, organizationID, organizationName, domainName);
        }

        public async System.Threading.Tasks.Task<int> CreateOrganizationAsync(int packageId, string organizationID, string organizationName, string domainName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "CreateOrganization", packageId, organizationID, organizationName, domainName);
        }

        public System.Data.DataSet GetRawOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esOrganizations", "GetRawOrganizationsPaged", packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawOrganizationsPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esOrganizations", "GetRawOrganizationsPaged", packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.Organization[] /*List*/ GetOrganizations(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.HostedSolution.Organization[], SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizations", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization[]> GetOrganizationsAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.Organization[], SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizations", packageId, recursive);
        }

        public SolidCP.Providers.HostedSolution.Organization GetOrganizationById(string organizationId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationById", organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationByIdAsync(string organizationId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationById", organizationId);
        }

        public string GetOrganizationUserSummuryLetter(int itemId, int accountId, bool pmm, bool emailMode, bool signup)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationUserSummuryLetter", itemId, accountId, pmm, emailMode, signup);
        }

        public async System.Threading.Tasks.Task<string> GetOrganizationUserSummuryLetterAsync(int itemId, int accountId, bool pmm, bool emailMode, bool signup)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationUserSummuryLetter", itemId, accountId, pmm, emailMode, signup);
        }

        public int SendOrganizationUserSummuryLetter(int itemId, int accountId, bool signup, string to, string cc)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "SendOrganizationUserSummuryLetter", itemId, accountId, signup, to, cc);
        }

        public async System.Threading.Tasks.Task<int> SendOrganizationUserSummuryLetterAsync(int itemId, int accountId, bool signup, string to, string cc)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "SendOrganizationUserSummuryLetter", itemId, accountId, signup, to, cc);
        }

        public int DeleteOrganization(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "DeleteOrganization", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteOrganizationAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "DeleteOrganization", itemId);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetOrganizationStatistics(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationStatistics", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetOrganizationStatisticsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationStatistics", itemId);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetOrganizationStatisticsByOrganization(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationStatisticsByOrganization", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetOrganizationStatisticsByOrganizationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationStatisticsByOrganization", itemId);
        }

        public SolidCP.Providers.HostedSolution.Organization GetOrganization(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganization", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganization", itemId);
        }

        public int GetAccountIdByUserPrincipalName(int itemId, string userPrincipalName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "GetAccountIdByUserPrincipalName", itemId, userPrincipalName);
        }

        public async System.Threading.Tasks.Task<int> GetAccountIdByUserPrincipalNameAsync(int itemId, string userPrincipalName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "GetAccountIdByUserPrincipalName", itemId, userPrincipalName);
        }

        public void SetDefaultOrganization(int newDefaultOrganizationId, int currentDefaultOrganizationId)
        {
            Invoke("SolidCP.EnterpriseServer.esOrganizations", "SetDefaultOrganization", newDefaultOrganizationId, currentDefaultOrganizationId);
        }

        public async System.Threading.Tasks.Task SetDefaultOrganizationAsync(int newDefaultOrganizationId, int currentDefaultOrganizationId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esOrganizations", "SetDefaultOrganization", newDefaultOrganizationId, currentDefaultOrganizationId);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser GetUserGeneralSettingsWithExtraData(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.EnterpriseServer.esOrganizations", "GetUserGeneralSettingsWithExtraData", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetUserGeneralSettingsWithExtraDataAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.EnterpriseServer.esOrganizations", "GetUserGeneralSettingsWithExtraData", itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject SendResetUserPasswordLinkSms(int itemId, int accountId, string reason, string phoneTo = null)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOrganizations", "SendResetUserPasswordLinkSms", itemId, accountId, reason, phoneTo);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendResetUserPasswordLinkSmsAsync(int itemId, int accountId, string reason, string phoneTo = null)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOrganizations", "SendResetUserPasswordLinkSms", itemId, accountId, reason, phoneTo);
        }

        public SolidCP.Providers.Common.ResultObject SendResetUserPasswordPincodeSms(System.Guid token, string phoneTo = null)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOrganizations", "SendResetUserPasswordPincodeSms", token, phoneTo);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendResetUserPasswordPincodeSmsAsync(System.Guid token, string phoneTo = null)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOrganizations", "SendResetUserPasswordPincodeSms", token, phoneTo);
        }

        public SolidCP.Providers.Common.ResultObject SendResetUserPasswordPincodeEmail(System.Guid token, string mailTo = null)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOrganizations", "SendResetUserPasswordPincodeEmail", token, mailTo);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendResetUserPasswordPincodeEmailAsync(System.Guid token, string mailTo = null)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOrganizations", "SendResetUserPasswordPincodeEmail", token, mailTo);
        }

        public SolidCP.Providers.Common.ResultObject SendUserPasswordRequestSms(int itemId, int accountId, string reason, string phoneTo)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOrganizations", "SendUserPasswordRequestSms", itemId, accountId, reason, phoneTo);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendUserPasswordRequestSmsAsync(int itemId, int accountId, string reason, string phoneTo)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOrganizations", "SendUserPasswordRequestSms", itemId, accountId, reason, phoneTo);
        }

        public void SendUserPasswordRequestEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            Invoke("SolidCP.EnterpriseServer.esOrganizations", "SendUserPasswordRequestEmail", itemId, accountId, reason, mailTo, finalStep);
        }

        public async System.Threading.Tasks.Task SendUserPasswordRequestEmailAsync(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esOrganizations", "SendUserPasswordRequestEmail", itemId, accountId, reason, mailTo, finalStep);
        }

        public int AddOrganizationDomain(int itemId, string domainName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "AddOrganizationDomain", itemId, domainName);
        }

        public async System.Threading.Tasks.Task<int> AddOrganizationDomainAsync(int itemId, string domainName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "AddOrganizationDomain", itemId, domainName);
        }

        public int ChangeOrganizationDomainType(int itemId, int domainId, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType newDomainType)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "ChangeOrganizationDomainType", itemId, domainId, newDomainType);
        }

        public async System.Threading.Tasks.Task<int> ChangeOrganizationDomainTypeAsync(int itemId, int domainId, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType newDomainType)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "ChangeOrganizationDomainType", itemId, domainId, newDomainType);
        }

        public SolidCP.Providers.HostedSolution.OrganizationDomainName[] /*List*/ GetOrganizationDomains(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationDomainName[], SolidCP.Providers.HostedSolution.OrganizationDomainName>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationDomains", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationDomainName[]> GetOrganizationDomainsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationDomainName[], SolidCP.Providers.HostedSolution.OrganizationDomainName>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationDomains", itemId);
        }

        public int DeleteOrganizationDomain(int itemId, int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "DeleteOrganizationDomain", itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteOrganizationDomainAsync(int itemId, int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "DeleteOrganizationDomain", itemId, domainId);
        }

        public int SetOrganizationDefaultDomain(int itemId, int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "SetOrganizationDefaultDomain", itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> SetOrganizationDefaultDomainAsync(int itemId, int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "SetOrganizationDefaultDomain", itemId, domainId);
        }

        public System.Data.DataSet GetOrganizationObjectsByDomain(int itemId, string domainName)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationObjectsByDomain", itemId, domainName);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetOrganizationObjectsByDomainAsync(int itemId, string domainName)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationObjectsByDomain", itemId, domainName);
        }

        public bool CheckDomainUsedByHostedOrganization(int itemId, int domainId)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esOrganizations", "CheckDomainUsedByHostedOrganization", itemId, domainId);
        }

        public async System.Threading.Tasks.Task<bool> CheckDomainUsedByHostedOrganizationAsync(int itemId, int domainId)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esOrganizations", "CheckDomainUsedByHostedOrganization", itemId, domainId);
        }

        public int CreateUser(int itemId, string displayName, string name, string domain, string password, string subscriberNumber, bool sendNotification, string to)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "CreateUser", itemId, displayName, name, domain, password, subscriberNumber, sendNotification, to);
        }

        public async System.Threading.Tasks.Task<int> CreateUserAsync(int itemId, string displayName, string name, string domain, string password, string subscriberNumber, bool sendNotification, string to)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "CreateUser", itemId, displayName, name, domain, password, subscriberNumber, sendNotification, to);
        }

        public int ImportUser(int itemId, string accountName, string displayName, string name, string domain, string password, string subscriberNumber)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "ImportUser", itemId, accountName, displayName, name, domain, password, subscriberNumber);
        }

        public async System.Threading.Tasks.Task<int> ImportUserAsync(int itemId, string accountName, string displayName, string name, string domain, string password, string subscriberNumber)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "ImportUser", itemId, accountName, displayName, name, domain, password, subscriberNumber);
        }

        public SolidCP.Providers.HostedSolution.OrganizationDeletedUsersPaged GetOrganizationDeletedUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationDeletedUsersPaged>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationDeletedUsersPaged", itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationDeletedUsersPaged> GetOrganizationDeletedUsersPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationDeletedUsersPaged>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationDeletedUsersPaged", itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUsersPaged GetOrganizationUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationUsersPaged>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationUsersPaged", itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUsersPaged> GetOrganizationUsersPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationUsersPaged>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationUsersPaged", itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser GetUserGeneralSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.EnterpriseServer.esOrganizations", "GetUserGeneralSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetUserGeneralSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.EnterpriseServer.esOrganizations", "GetUserGeneralSettings", itemId, accountId);
        }

        public int SetUserGeneralSettings(int itemId, int accountId, string displayName, string password, bool hideAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, string subscriberNumber, int levelId, bool isVIP, bool userMustChangePassword)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "SetUserGeneralSettings", itemId, accountId, displayName, password, hideAddressBook, disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, externalEmail, subscriberNumber, levelId, isVIP, userMustChangePassword);
        }

        public async System.Threading.Tasks.Task<int> SetUserGeneralSettingsAsync(int itemId, int accountId, string displayName, string password, bool hideAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, string subscriberNumber, int levelId, bool isVIP, bool userMustChangePassword)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "SetUserGeneralSettings", itemId, accountId, displayName, password, hideAddressBook, disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, externalEmail, subscriberNumber, levelId, isVIP, userMustChangePassword);
        }

        public int SetUserPrincipalName(int itemId, int accountId, string userPrincipalName, bool inherit)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "SetUserPrincipalName", itemId, accountId, userPrincipalName, inherit);
        }

        public async System.Threading.Tasks.Task<int> SetUserPrincipalNameAsync(int itemId, int accountId, string userPrincipalName, bool inherit)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "SetUserPrincipalName", itemId, accountId, userPrincipalName, inherit);
        }

        public int SetUserPassword(int itemId, int accountId, string password)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "SetUserPassword", itemId, accountId, password);
        }

        public async System.Threading.Tasks.Task<int> SetUserPasswordAsync(int itemId, int accountId, string password)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "SetUserPassword", itemId, accountId, password);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ SearchAccounts(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeMailboxes)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationUser[], SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.EnterpriseServer.esOrganizations", "SearchAccounts", itemId, filterColumn, filterValue, sortColumn, includeMailboxes);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> SearchAccountsAsync(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeMailboxes)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationUser[], SolidCP.Providers.HostedSolution.OrganizationUser>("SolidCP.EnterpriseServer.esOrganizations", "SearchAccounts", itemId, filterColumn, filterValue, sortColumn, includeMailboxes);
        }

        public int SetDeletedUser(int itemId, int accountId, bool enableForceArchive)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "SetDeletedUser", itemId, accountId, enableForceArchive);
        }

        public async System.Threading.Tasks.Task<int> SetDeletedUserAsync(int itemId, int accountId, bool enableForceArchive)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "SetDeletedUser", itemId, accountId, enableForceArchive);
        }

        public byte[] GetArchiveFileBinaryChunk(int packageId, int itemId, int deleteAccountId, int offset, int length)
        {
            return Invoke<byte[]>("SolidCP.EnterpriseServer.esOrganizations", "GetArchiveFileBinaryChunk", packageId, itemId, deleteAccountId, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetArchiveFileBinaryChunkAsync(int packageId, int itemId, int deleteAccountId, int offset, int length)
        {
            return await InvokeAsync<byte[]>("SolidCP.EnterpriseServer.esOrganizations", "GetArchiveFileBinaryChunk", packageId, itemId, deleteAccountId, offset, length);
        }

        public int DeleteUser(int itemId, int accountId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "DeleteUser", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeleteUserAsync(int itemId, int accountId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "DeleteUser", itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.PasswordPolicyResult GetPasswordPolicy(int itemId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.PasswordPolicyResult>("SolidCP.EnterpriseServer.esOrganizations", "GetPasswordPolicy", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.PasswordPolicyResult> GetPasswordPolicyAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.PasswordPolicyResult>("SolidCP.EnterpriseServer.esOrganizations", "GetPasswordPolicy", itemId);
        }

        public void SendResetUserPasswordEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            Invoke("SolidCP.EnterpriseServer.esOrganizations", "SendResetUserPasswordEmail", itemId, accountId, reason, mailTo, finalStep);
        }

        public async System.Threading.Tasks.Task SendResetUserPasswordEmailAsync(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esOrganizations", "SendResetUserPasswordEmail", itemId, accountId, reason, mailTo, finalStep);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken CreatePasswordResetAccessToken(int itemId, int accountId)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken>("SolidCP.EnterpriseServer.esOrganizations", "CreatePasswordResetAccessToken", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken> CreatePasswordResetAccessTokenAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken>("SolidCP.EnterpriseServer.esOrganizations", "CreatePasswordResetAccessToken", itemId, accountId);
        }

        public int CreateSecurityGroup(int itemId, string displayName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "CreateSecurityGroup", itemId, displayName);
        }

        public async System.Threading.Tasks.Task<int> CreateSecurityGroupAsync(int itemId, string displayName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "CreateSecurityGroup", itemId, displayName);
        }

        public SolidCP.Providers.HostedSolution.OrganizationSecurityGroup GetSecurityGroupGeneralSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationSecurityGroup>("SolidCP.EnterpriseServer.esOrganizations", "GetSecurityGroupGeneralSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationSecurityGroup> GetSecurityGroupGeneralSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationSecurityGroup>("SolidCP.EnterpriseServer.esOrganizations", "GetSecurityGroupGeneralSettings", itemId, accountId);
        }

        public int DeleteSecurityGroup(int itemId, int accountId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "DeleteSecurityGroup", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSecurityGroupAsync(int itemId, int accountId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "DeleteSecurityGroup", itemId, accountId);
        }

        public int SetSecurityGroupGeneralSettings(int itemId, int accountId, string displayName, string[] memberAccounts, string notes)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "SetSecurityGroupGeneralSettings", itemId, accountId, displayName, memberAccounts, notes);
        }

        public async System.Threading.Tasks.Task<int> SetSecurityGroupGeneralSettingsAsync(int itemId, int accountId, string displayName, string[] memberAccounts, string notes)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "SetSecurityGroupGeneralSettings", itemId, accountId, displayName, memberAccounts, notes);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccountsPaged GetOrganizationSecurityGroupsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccountsPaged>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationSecurityGroupsPaged", itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccountsPaged> GetOrganizationSecurityGroupsPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccountsPaged>("SolidCP.EnterpriseServer.esOrganizations", "GetOrganizationSecurityGroupsPaged", itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public int AddObjectToSecurityGroup(int itemId, int accountId, string groupName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "AddObjectToSecurityGroup", itemId, accountId, groupName);
        }

        public async System.Threading.Tasks.Task<int> AddObjectToSecurityGroupAsync(int itemId, int accountId, string groupName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "AddObjectToSecurityGroup", itemId, accountId, groupName);
        }

        public int DeleteObjectFromSecurityGroup(int itemId, int accountId, string groupName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "DeleteObjectFromSecurityGroup", itemId, accountId, groupName);
        }

        public async System.Threading.Tasks.Task<int> DeleteObjectFromSecurityGroupAsync(int itemId, int accountId, string groupName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "DeleteObjectFromSecurityGroup", itemId, accountId, groupName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] GetSecurityGroupsByMember(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount[]>("SolidCP.EnterpriseServer.esOrganizations", "GetSecurityGroupsByMember", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetSecurityGroupsByMemberAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount[]>("SolidCP.EnterpriseServer.esOrganizations", "GetSecurityGroupsByMember", itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchOrganizationAccounts(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeOnlySecurityGroups)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esOrganizations", "SearchOrganizationAccounts", itemId, filterColumn, filterValue, sortColumn, includeOnlySecurityGroups);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchOrganizationAccountsAsync(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeOnlySecurityGroups)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esOrganizations", "SearchOrganizationAccounts", itemId, filterColumn, filterValue, sortColumn, includeOnlySecurityGroups);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] GetUserGroups(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount[]>("SolidCP.EnterpriseServer.esOrganizations", "GetUserGroups", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetUserGroupsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount[]>("SolidCP.EnterpriseServer.esOrganizations", "GetUserGroups", itemId, accountId);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.AdditionalGroup[] /*List*/ GetAdditionalGroups(int userId)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.AdditionalGroup[], SolidCP.EnterpriseServer.Base.HostedSolution.AdditionalGroup>("SolidCP.EnterpriseServer.esOrganizations", "GetAdditionalGroups", userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.AdditionalGroup[]> GetAdditionalGroupsAsync(int userId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.AdditionalGroup[], SolidCP.EnterpriseServer.Base.HostedSolution.AdditionalGroup>("SolidCP.EnterpriseServer.esOrganizations", "GetAdditionalGroups", userId);
        }

        public void UpdateAdditionalGroup(int groupId, string groupName)
        {
            Invoke("SolidCP.EnterpriseServer.esOrganizations", "UpdateAdditionalGroup", groupId, groupName);
        }

        public async System.Threading.Tasks.Task UpdateAdditionalGroupAsync(int groupId, string groupName)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esOrganizations", "UpdateAdditionalGroup", groupId, groupName);
        }

        public void DeleteAdditionalGroup(int groupId)
        {
            Invoke("SolidCP.EnterpriseServer.esOrganizations", "DeleteAdditionalGroup", groupId);
        }

        public async System.Threading.Tasks.Task DeleteAdditionalGroupAsync(int groupId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esOrganizations", "DeleteAdditionalGroup", groupId);
        }

        public int AddAdditionalGroup(int userId, string groupName)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "AddAdditionalGroup", userId, groupName);
        }

        public async System.Threading.Tasks.Task<int> AddAdditionalGroupAsync(int userId, string groupName)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "AddAdditionalGroup", userId, groupName);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel[] GetSupportServiceLevels()
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel[]>("SolidCP.EnterpriseServer.esOrganizations", "GetSupportServiceLevels");
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel[]> GetSupportServiceLevelsAsync()
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel[]>("SolidCP.EnterpriseServer.esOrganizations", "GetSupportServiceLevels");
        }

        public void UpdateSupportServiceLevel(int levelID, string levelName, string levelDescription)
        {
            Invoke("SolidCP.EnterpriseServer.esOrganizations", "UpdateSupportServiceLevel", levelID, levelName, levelDescription);
        }

        public async System.Threading.Tasks.Task UpdateSupportServiceLevelAsync(int levelID, string levelName, string levelDescription)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esOrganizations", "UpdateSupportServiceLevel", levelID, levelName, levelDescription);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSupportServiceLevel(int levelId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOrganizations", "DeleteSupportServiceLevel", levelId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSupportServiceLevelAsync(int levelId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esOrganizations", "DeleteSupportServiceLevel", levelId);
        }

        public int AddSupportServiceLevel(string levelName, string levelDescription)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esOrganizations", "AddSupportServiceLevel", levelName, levelDescription);
        }

        public async System.Threading.Tasks.Task<int> AddSupportServiceLevelAsync(string levelName, string levelDescription)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esOrganizations", "AddSupportServiceLevel", levelName, levelDescription);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel GetSupportServiceLevel(int levelID)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel>("SolidCP.EnterpriseServer.esOrganizations", "GetSupportServiceLevel", levelID);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel> GetSupportServiceLevelAsync(int levelID)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel>("SolidCP.EnterpriseServer.esOrganizations", "GetSupportServiceLevel", levelID);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esOrganizations : SolidCP.Web.Client.ClientBase<IesOrganizations, esOrganizationsAssemblyClient>, IesOrganizations
    {
        public bool CheckPhoneNumberIsInUse(int itemId, string phoneNumber, string userSamAccountName = null)
        {
            return base.Client.CheckPhoneNumberIsInUse(itemId, phoneNumber, userSamAccountName);
        }

        public async System.Threading.Tasks.Task<bool> CheckPhoneNumberIsInUseAsync(int itemId, string phoneNumber, string userSamAccountName = null)
        {
            return await base.Client.CheckPhoneNumberIsInUseAsync(itemId, phoneNumber, userSamAccountName);
        }

        public void DeletePasswordresetAccessToken(System.Guid accessToken)
        {
            base.Client.DeletePasswordresetAccessToken(accessToken);
        }

        public async System.Threading.Tasks.Task DeletePasswordresetAccessTokenAsync(System.Guid accessToken)
        {
            await base.Client.DeletePasswordresetAccessTokenAsync(accessToken);
        }

        public void SetAccessTokenResponse(System.Guid accessToken, string response)
        {
            base.Client.SetAccessTokenResponse(accessToken, response);
        }

        public async System.Threading.Tasks.Task SetAccessTokenResponseAsync(System.Guid accessToken, string response)
        {
            await base.Client.SetAccessTokenResponseAsync(accessToken, response);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken GetPasswordresetAccessToken(System.Guid token)
        {
            return base.Client.GetPasswordresetAccessToken(token);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken> GetPasswordresetAccessTokenAsync(System.Guid token)
        {
            return await base.Client.GetPasswordresetAccessTokenAsync(token);
        }

        public void UpdateOrganizationGeneralSettings(int itemId, SolidCP.Providers.HostedSolution.OrganizationGeneralSettings settings)
        {
            base.Client.UpdateOrganizationGeneralSettings(itemId, settings);
        }

        public async System.Threading.Tasks.Task UpdateOrganizationGeneralSettingsAsync(int itemId, SolidCP.Providers.HostedSolution.OrganizationGeneralSettings settings)
        {
            await base.Client.UpdateOrganizationGeneralSettingsAsync(itemId, settings);
        }

        public SolidCP.Providers.HostedSolution.OrganizationGeneralSettings GetOrganizationGeneralSettings(int itemId)
        {
            return base.Client.GetOrganizationGeneralSettings(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationGeneralSettings> GetOrganizationGeneralSettingsAsync(int itemId)
        {
            return await base.Client.GetOrganizationGeneralSettingsAsync(itemId);
        }

        public void UpdateOrganizationPasswordSettings(int itemId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings settings)
        {
            base.Client.UpdateOrganizationPasswordSettings(itemId, settings);
        }

        public async System.Threading.Tasks.Task UpdateOrganizationPasswordSettingsAsync(int itemId, SolidCP.Providers.HostedSolution.OrganizationPasswordSettings settings)
        {
            await base.Client.UpdateOrganizationPasswordSettingsAsync(itemId, settings);
        }

        public SolidCP.EnterpriseServer.SystemSettings GetWebDavSystemSettings()
        {
            return base.Client.GetWebDavSystemSettings();
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.SystemSettings> GetWebDavSystemSettingsAsync()
        {
            return await base.Client.GetWebDavSystemSettingsAsync();
        }

        public SolidCP.Providers.HostedSolution.OrganizationPasswordSettings GetOrganizationPasswordSettings(int itemId)
        {
            return base.Client.GetOrganizationPasswordSettings(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationPasswordSettings> GetOrganizationPasswordSettingsAsync(int itemId)
        {
            return await base.Client.GetOrganizationPasswordSettingsAsync(itemId);
        }

        public bool CheckOrgIdExists(string orgId)
        {
            return base.Client.CheckOrgIdExists(orgId);
        }

        public async System.Threading.Tasks.Task<bool> CheckOrgIdExistsAsync(string orgId)
        {
            return await base.Client.CheckOrgIdExistsAsync(orgId);
        }

        public int CreateOrganization(int packageId, string organizationID, string organizationName, string domainName)
        {
            return base.Client.CreateOrganization(packageId, organizationID, organizationName, domainName);
        }

        public async System.Threading.Tasks.Task<int> CreateOrganizationAsync(int packageId, string organizationID, string organizationName, string domainName)
        {
            return await base.Client.CreateOrganizationAsync(packageId, organizationID, organizationName, domainName);
        }

        public System.Data.DataSet GetRawOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawOrganizationsPaged(packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawOrganizationsPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawOrganizationsPagedAsync(packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.Organization[] /*List*/ GetOrganizations(int packageId, bool recursive)
        {
            return base.Client.GetOrganizations(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization[]> GetOrganizationsAsync(int packageId, bool recursive)
        {
            return await base.Client.GetOrganizationsAsync(packageId, recursive);
        }

        public SolidCP.Providers.HostedSolution.Organization GetOrganizationById(string organizationId)
        {
            return base.Client.GetOrganizationById(organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationByIdAsync(string organizationId)
        {
            return await base.Client.GetOrganizationByIdAsync(organizationId);
        }

        public string GetOrganizationUserSummuryLetter(int itemId, int accountId, bool pmm, bool emailMode, bool signup)
        {
            return base.Client.GetOrganizationUserSummuryLetter(itemId, accountId, pmm, emailMode, signup);
        }

        public async System.Threading.Tasks.Task<string> GetOrganizationUserSummuryLetterAsync(int itemId, int accountId, bool pmm, bool emailMode, bool signup)
        {
            return await base.Client.GetOrganizationUserSummuryLetterAsync(itemId, accountId, pmm, emailMode, signup);
        }

        public int SendOrganizationUserSummuryLetter(int itemId, int accountId, bool signup, string to, string cc)
        {
            return base.Client.SendOrganizationUserSummuryLetter(itemId, accountId, signup, to, cc);
        }

        public async System.Threading.Tasks.Task<int> SendOrganizationUserSummuryLetterAsync(int itemId, int accountId, bool signup, string to, string cc)
        {
            return await base.Client.SendOrganizationUserSummuryLetterAsync(itemId, accountId, signup, to, cc);
        }

        public int DeleteOrganization(int itemId)
        {
            return base.Client.DeleteOrganization(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteOrganizationAsync(int itemId)
        {
            return await base.Client.DeleteOrganizationAsync(itemId);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetOrganizationStatistics(int itemId)
        {
            return base.Client.GetOrganizationStatistics(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetOrganizationStatisticsAsync(int itemId)
        {
            return await base.Client.GetOrganizationStatisticsAsync(itemId);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetOrganizationStatisticsByOrganization(int itemId)
        {
            return base.Client.GetOrganizationStatisticsByOrganization(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetOrganizationStatisticsByOrganizationAsync(int itemId)
        {
            return await base.Client.GetOrganizationStatisticsByOrganizationAsync(itemId);
        }

        public SolidCP.Providers.HostedSolution.Organization GetOrganization(int itemId)
        {
            return base.Client.GetOrganization(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationAsync(int itemId)
        {
            return await base.Client.GetOrganizationAsync(itemId);
        }

        public int GetAccountIdByUserPrincipalName(int itemId, string userPrincipalName)
        {
            return base.Client.GetAccountIdByUserPrincipalName(itemId, userPrincipalName);
        }

        public async System.Threading.Tasks.Task<int> GetAccountIdByUserPrincipalNameAsync(int itemId, string userPrincipalName)
        {
            return await base.Client.GetAccountIdByUserPrincipalNameAsync(itemId, userPrincipalName);
        }

        public void SetDefaultOrganization(int newDefaultOrganizationId, int currentDefaultOrganizationId)
        {
            base.Client.SetDefaultOrganization(newDefaultOrganizationId, currentDefaultOrganizationId);
        }

        public async System.Threading.Tasks.Task SetDefaultOrganizationAsync(int newDefaultOrganizationId, int currentDefaultOrganizationId)
        {
            await base.Client.SetDefaultOrganizationAsync(newDefaultOrganizationId, currentDefaultOrganizationId);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser GetUserGeneralSettingsWithExtraData(int itemId, int accountId)
        {
            return base.Client.GetUserGeneralSettingsWithExtraData(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetUserGeneralSettingsWithExtraDataAsync(int itemId, int accountId)
        {
            return await base.Client.GetUserGeneralSettingsWithExtraDataAsync(itemId, accountId);
        }

        public SolidCP.Providers.Common.ResultObject SendResetUserPasswordLinkSms(int itemId, int accountId, string reason, string phoneTo = null)
        {
            return base.Client.SendResetUserPasswordLinkSms(itemId, accountId, reason, phoneTo);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendResetUserPasswordLinkSmsAsync(int itemId, int accountId, string reason, string phoneTo = null)
        {
            return await base.Client.SendResetUserPasswordLinkSmsAsync(itemId, accountId, reason, phoneTo);
        }

        public SolidCP.Providers.Common.ResultObject SendResetUserPasswordPincodeSms(System.Guid token, string phoneTo = null)
        {
            return base.Client.SendResetUserPasswordPincodeSms(token, phoneTo);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendResetUserPasswordPincodeSmsAsync(System.Guid token, string phoneTo = null)
        {
            return await base.Client.SendResetUserPasswordPincodeSmsAsync(token, phoneTo);
        }

        public SolidCP.Providers.Common.ResultObject SendResetUserPasswordPincodeEmail(System.Guid token, string mailTo = null)
        {
            return base.Client.SendResetUserPasswordPincodeEmail(token, mailTo);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendResetUserPasswordPincodeEmailAsync(System.Guid token, string mailTo = null)
        {
            return await base.Client.SendResetUserPasswordPincodeEmailAsync(token, mailTo);
        }

        public SolidCP.Providers.Common.ResultObject SendUserPasswordRequestSms(int itemId, int accountId, string reason, string phoneTo)
        {
            return base.Client.SendUserPasswordRequestSms(itemId, accountId, reason, phoneTo);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SendUserPasswordRequestSmsAsync(int itemId, int accountId, string reason, string phoneTo)
        {
            return await base.Client.SendUserPasswordRequestSmsAsync(itemId, accountId, reason, phoneTo);
        }

        public void SendUserPasswordRequestEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            base.Client.SendUserPasswordRequestEmail(itemId, accountId, reason, mailTo, finalStep);
        }

        public async System.Threading.Tasks.Task SendUserPasswordRequestEmailAsync(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            await base.Client.SendUserPasswordRequestEmailAsync(itemId, accountId, reason, mailTo, finalStep);
        }

        public int AddOrganizationDomain(int itemId, string domainName)
        {
            return base.Client.AddOrganizationDomain(itemId, domainName);
        }

        public async System.Threading.Tasks.Task<int> AddOrganizationDomainAsync(int itemId, string domainName)
        {
            return await base.Client.AddOrganizationDomainAsync(itemId, domainName);
        }

        public int ChangeOrganizationDomainType(int itemId, int domainId, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType newDomainType)
        {
            return base.Client.ChangeOrganizationDomainType(itemId, domainId, newDomainType);
        }

        public async System.Threading.Tasks.Task<int> ChangeOrganizationDomainTypeAsync(int itemId, int domainId, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType newDomainType)
        {
            return await base.Client.ChangeOrganizationDomainTypeAsync(itemId, domainId, newDomainType);
        }

        public SolidCP.Providers.HostedSolution.OrganizationDomainName[] /*List*/ GetOrganizationDomains(int itemId)
        {
            return base.Client.GetOrganizationDomains(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationDomainName[]> GetOrganizationDomainsAsync(int itemId)
        {
            return await base.Client.GetOrganizationDomainsAsync(itemId);
        }

        public int DeleteOrganizationDomain(int itemId, int domainId)
        {
            return base.Client.DeleteOrganizationDomain(itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteOrganizationDomainAsync(int itemId, int domainId)
        {
            return await base.Client.DeleteOrganizationDomainAsync(itemId, domainId);
        }

        public int SetOrganizationDefaultDomain(int itemId, int domainId)
        {
            return base.Client.SetOrganizationDefaultDomain(itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> SetOrganizationDefaultDomainAsync(int itemId, int domainId)
        {
            return await base.Client.SetOrganizationDefaultDomainAsync(itemId, domainId);
        }

        public System.Data.DataSet GetOrganizationObjectsByDomain(int itemId, string domainName)
        {
            return base.Client.GetOrganizationObjectsByDomain(itemId, domainName);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetOrganizationObjectsByDomainAsync(int itemId, string domainName)
        {
            return await base.Client.GetOrganizationObjectsByDomainAsync(itemId, domainName);
        }

        public bool CheckDomainUsedByHostedOrganization(int itemId, int domainId)
        {
            return base.Client.CheckDomainUsedByHostedOrganization(itemId, domainId);
        }

        public async System.Threading.Tasks.Task<bool> CheckDomainUsedByHostedOrganizationAsync(int itemId, int domainId)
        {
            return await base.Client.CheckDomainUsedByHostedOrganizationAsync(itemId, domainId);
        }

        public int CreateUser(int itemId, string displayName, string name, string domain, string password, string subscriberNumber, bool sendNotification, string to)
        {
            return base.Client.CreateUser(itemId, displayName, name, domain, password, subscriberNumber, sendNotification, to);
        }

        public async System.Threading.Tasks.Task<int> CreateUserAsync(int itemId, string displayName, string name, string domain, string password, string subscriberNumber, bool sendNotification, string to)
        {
            return await base.Client.CreateUserAsync(itemId, displayName, name, domain, password, subscriberNumber, sendNotification, to);
        }

        public int ImportUser(int itemId, string accountName, string displayName, string name, string domain, string password, string subscriberNumber)
        {
            return base.Client.ImportUser(itemId, accountName, displayName, name, domain, password, subscriberNumber);
        }

        public async System.Threading.Tasks.Task<int> ImportUserAsync(int itemId, string accountName, string displayName, string name, string domain, string password, string subscriberNumber)
        {
            return await base.Client.ImportUserAsync(itemId, accountName, displayName, name, domain, password, subscriberNumber);
        }

        public SolidCP.Providers.HostedSolution.OrganizationDeletedUsersPaged GetOrganizationDeletedUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetOrganizationDeletedUsersPaged(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationDeletedUsersPaged> GetOrganizationDeletedUsersPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetOrganizationDeletedUsersPagedAsync(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUsersPaged GetOrganizationUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetOrganizationUsersPaged(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUsersPaged> GetOrganizationUsersPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetOrganizationUsersPagedAsync(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser GetUserGeneralSettings(int itemId, int accountId)
        {
            return base.Client.GetUserGeneralSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser> GetUserGeneralSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetUserGeneralSettingsAsync(itemId, accountId);
        }

        public int SetUserGeneralSettings(int itemId, int accountId, string displayName, string password, bool hideAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, string subscriberNumber, int levelId, bool isVIP, bool userMustChangePassword)
        {
            return base.Client.SetUserGeneralSettings(itemId, accountId, displayName, password, hideAddressBook, disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, externalEmail, subscriberNumber, levelId, isVIP, userMustChangePassword);
        }

        public async System.Threading.Tasks.Task<int> SetUserGeneralSettingsAsync(int itemId, int accountId, string displayName, string password, bool hideAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, string externalEmail, string subscriberNumber, int levelId, bool isVIP, bool userMustChangePassword)
        {
            return await base.Client.SetUserGeneralSettingsAsync(itemId, accountId, displayName, password, hideAddressBook, disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, externalEmail, subscriberNumber, levelId, isVIP, userMustChangePassword);
        }

        public int SetUserPrincipalName(int itemId, int accountId, string userPrincipalName, bool inherit)
        {
            return base.Client.SetUserPrincipalName(itemId, accountId, userPrincipalName, inherit);
        }

        public async System.Threading.Tasks.Task<int> SetUserPrincipalNameAsync(int itemId, int accountId, string userPrincipalName, bool inherit)
        {
            return await base.Client.SetUserPrincipalNameAsync(itemId, accountId, userPrincipalName, inherit);
        }

        public int SetUserPassword(int itemId, int accountId, string password)
        {
            return base.Client.SetUserPassword(itemId, accountId, password);
        }

        public async System.Threading.Tasks.Task<int> SetUserPasswordAsync(int itemId, int accountId, string password)
        {
            return await base.Client.SetUserPasswordAsync(itemId, accountId, password);
        }

        public SolidCP.Providers.HostedSolution.OrganizationUser[] /*List*/ SearchAccounts(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeMailboxes)
        {
            return base.Client.SearchAccounts(itemId, filterColumn, filterValue, sortColumn, includeMailboxes);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationUser[]> SearchAccountsAsync(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeMailboxes)
        {
            return await base.Client.SearchAccountsAsync(itemId, filterColumn, filterValue, sortColumn, includeMailboxes);
        }

        public int SetDeletedUser(int itemId, int accountId, bool enableForceArchive)
        {
            return base.Client.SetDeletedUser(itemId, accountId, enableForceArchive);
        }

        public async System.Threading.Tasks.Task<int> SetDeletedUserAsync(int itemId, int accountId, bool enableForceArchive)
        {
            return await base.Client.SetDeletedUserAsync(itemId, accountId, enableForceArchive);
        }

        public byte[] GetArchiveFileBinaryChunk(int packageId, int itemId, int deleteAccountId, int offset, int length)
        {
            return base.Client.GetArchiveFileBinaryChunk(packageId, itemId, deleteAccountId, offset, length);
        }

        public async System.Threading.Tasks.Task<byte[]> GetArchiveFileBinaryChunkAsync(int packageId, int itemId, int deleteAccountId, int offset, int length)
        {
            return await base.Client.GetArchiveFileBinaryChunkAsync(packageId, itemId, deleteAccountId, offset, length);
        }

        public int DeleteUser(int itemId, int accountId)
        {
            return base.Client.DeleteUser(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeleteUserAsync(int itemId, int accountId)
        {
            return await base.Client.DeleteUserAsync(itemId, accountId);
        }

        public SolidCP.Providers.ResultObjects.PasswordPolicyResult GetPasswordPolicy(int itemId)
        {
            return base.Client.GetPasswordPolicy(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.PasswordPolicyResult> GetPasswordPolicyAsync(int itemId)
        {
            return await base.Client.GetPasswordPolicyAsync(itemId);
        }

        public void SendResetUserPasswordEmail(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            base.Client.SendResetUserPasswordEmail(itemId, accountId, reason, mailTo, finalStep);
        }

        public async System.Threading.Tasks.Task SendResetUserPasswordEmailAsync(int itemId, int accountId, string reason, string mailTo, bool finalStep)
        {
            await base.Client.SendResetUserPasswordEmailAsync(itemId, accountId, reason, mailTo, finalStep);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken CreatePasswordResetAccessToken(int itemId, int accountId)
        {
            return base.Client.CreatePasswordResetAccessToken(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.AccessToken> CreatePasswordResetAccessTokenAsync(int itemId, int accountId)
        {
            return await base.Client.CreatePasswordResetAccessTokenAsync(itemId, accountId);
        }

        public int CreateSecurityGroup(int itemId, string displayName)
        {
            return base.Client.CreateSecurityGroup(itemId, displayName);
        }

        public async System.Threading.Tasks.Task<int> CreateSecurityGroupAsync(int itemId, string displayName)
        {
            return await base.Client.CreateSecurityGroupAsync(itemId, displayName);
        }

        public SolidCP.Providers.HostedSolution.OrganizationSecurityGroup GetSecurityGroupGeneralSettings(int itemId, int accountId)
        {
            return base.Client.GetSecurityGroupGeneralSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationSecurityGroup> GetSecurityGroupGeneralSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetSecurityGroupGeneralSettingsAsync(itemId, accountId);
        }

        public int DeleteSecurityGroup(int itemId, int accountId)
        {
            return base.Client.DeleteSecurityGroup(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeleteSecurityGroupAsync(int itemId, int accountId)
        {
            return await base.Client.DeleteSecurityGroupAsync(itemId, accountId);
        }

        public int SetSecurityGroupGeneralSettings(int itemId, int accountId, string displayName, string[] memberAccounts, string notes)
        {
            return base.Client.SetSecurityGroupGeneralSettings(itemId, accountId, displayName, memberAccounts, notes);
        }

        public async System.Threading.Tasks.Task<int> SetSecurityGroupGeneralSettingsAsync(int itemId, int accountId, string displayName, string[] memberAccounts, string notes)
        {
            return await base.Client.SetSecurityGroupGeneralSettingsAsync(itemId, accountId, displayName, memberAccounts, notes);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccountsPaged GetOrganizationSecurityGroupsPaged(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetOrganizationSecurityGroupsPaged(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccountsPaged> GetOrganizationSecurityGroupsPagedAsync(int itemId, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetOrganizationSecurityGroupsPagedAsync(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public int AddObjectToSecurityGroup(int itemId, int accountId, string groupName)
        {
            return base.Client.AddObjectToSecurityGroup(itemId, accountId, groupName);
        }

        public async System.Threading.Tasks.Task<int> AddObjectToSecurityGroupAsync(int itemId, int accountId, string groupName)
        {
            return await base.Client.AddObjectToSecurityGroupAsync(itemId, accountId, groupName);
        }

        public int DeleteObjectFromSecurityGroup(int itemId, int accountId, string groupName)
        {
            return base.Client.DeleteObjectFromSecurityGroup(itemId, accountId, groupName);
        }

        public async System.Threading.Tasks.Task<int> DeleteObjectFromSecurityGroupAsync(int itemId, int accountId, string groupName)
        {
            return await base.Client.DeleteObjectFromSecurityGroupAsync(itemId, accountId, groupName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] GetSecurityGroupsByMember(int itemId, int accountId)
        {
            return base.Client.GetSecurityGroupsByMember(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetSecurityGroupsByMemberAsync(int itemId, int accountId)
        {
            return await base.Client.GetSecurityGroupsByMemberAsync(itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchOrganizationAccounts(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeOnlySecurityGroups)
        {
            return base.Client.SearchOrganizationAccounts(itemId, filterColumn, filterValue, sortColumn, includeOnlySecurityGroups);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchOrganizationAccountsAsync(int itemId, string filterColumn, string filterValue, string sortColumn, bool includeOnlySecurityGroups)
        {
            return await base.Client.SearchOrganizationAccountsAsync(itemId, filterColumn, filterValue, sortColumn, includeOnlySecurityGroups);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] GetUserGroups(int itemId, int accountId)
        {
            return base.Client.GetUserGroups(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetUserGroupsAsync(int itemId, int accountId)
        {
            return await base.Client.GetUserGroupsAsync(itemId, accountId);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.AdditionalGroup[] /*List*/ GetAdditionalGroups(int userId)
        {
            return base.Client.GetAdditionalGroups(userId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.AdditionalGroup[]> GetAdditionalGroupsAsync(int userId)
        {
            return await base.Client.GetAdditionalGroupsAsync(userId);
        }

        public void UpdateAdditionalGroup(int groupId, string groupName)
        {
            base.Client.UpdateAdditionalGroup(groupId, groupName);
        }

        public async System.Threading.Tasks.Task UpdateAdditionalGroupAsync(int groupId, string groupName)
        {
            await base.Client.UpdateAdditionalGroupAsync(groupId, groupName);
        }

        public void DeleteAdditionalGroup(int groupId)
        {
            base.Client.DeleteAdditionalGroup(groupId);
        }

        public async System.Threading.Tasks.Task DeleteAdditionalGroupAsync(int groupId)
        {
            await base.Client.DeleteAdditionalGroupAsync(groupId);
        }

        public int AddAdditionalGroup(int userId, string groupName)
        {
            return base.Client.AddAdditionalGroup(userId, groupName);
        }

        public async System.Threading.Tasks.Task<int> AddAdditionalGroupAsync(int userId, string groupName)
        {
            return await base.Client.AddAdditionalGroupAsync(userId, groupName);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel[] GetSupportServiceLevels()
        {
            return base.Client.GetSupportServiceLevels();
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel[]> GetSupportServiceLevelsAsync()
        {
            return await base.Client.GetSupportServiceLevelsAsync();
        }

        public void UpdateSupportServiceLevel(int levelID, string levelName, string levelDescription)
        {
            base.Client.UpdateSupportServiceLevel(levelID, levelName, levelDescription);
        }

        public async System.Threading.Tasks.Task UpdateSupportServiceLevelAsync(int levelID, string levelName, string levelDescription)
        {
            await base.Client.UpdateSupportServiceLevelAsync(levelID, levelName, levelDescription);
        }

        public SolidCP.Providers.Common.ResultObject DeleteSupportServiceLevel(int levelId)
        {
            return base.Client.DeleteSupportServiceLevel(levelId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteSupportServiceLevelAsync(int levelId)
        {
            return await base.Client.DeleteSupportServiceLevelAsync(levelId);
        }

        public int AddSupportServiceLevel(string levelName, string levelDescription)
        {
            return base.Client.AddSupportServiceLevel(levelName, levelDescription);
        }

        public async System.Threading.Tasks.Task<int> AddSupportServiceLevelAsync(string levelName, string levelDescription)
        {
            return await base.Client.AddSupportServiceLevelAsync(levelName, levelDescription);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel GetSupportServiceLevel(int levelID)
        {
            return base.Client.GetSupportServiceLevel(levelID);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.ServiceLevel> GetSupportServiceLevelAsync(int levelID)
        {
            return await base.Client.GetSupportServiceLevelAsync(levelID);
        }
    }
}
#endif