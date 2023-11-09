#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.EnterpriseServer.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("EnterpriseServerPolicy")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IesExchangeServer", Namespace = "http://smbsaas/solidcp/enterpriseserver/")]
    public interface IesExchangeServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetRawExchangeOrganizationsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetRawExchangeOrganizationsPagedResponse")]
        System.Data.DataSet GetRawExchangeOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetRawExchangeOrganizationsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetRawExchangeOrganizationsPagedResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetRawExchangeOrganizationsPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeOrganizationsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeOrganizationsPagedResponse")]
        SolidCP.Providers.HostedSolution.OrganizationsPaged GetExchangeOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeOrganizationsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeOrganizationsPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationsPaged> GetExchangeOrganizationsPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeOrganizations", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeOrganizationsResponse")]
        SolidCP.Providers.HostedSolution.Organization[] /*List*/ GetExchangeOrganizations(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeOrganizations", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeOrganizationsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization[]> GetExchangeOrganizationsAsync(int packageId, bool recursive);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationResponse")]
        SolidCP.Providers.HostedSolution.Organization GetOrganization(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStatistics", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStatisticsResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetOrganizationStatistics(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStatistics", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStatisticsResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetOrganizationStatisticsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStatisticsByOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStatisticsByOrganizationResponse")]
        SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetOrganizationStatisticsByOrganization(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStatisticsByOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStatisticsByOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetOrganizationStatisticsByOrganizationAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeServiceID", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeServiceIDResponse")]
        int GetExchangeServiceID(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeServiceID", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeServiceIDResponse")]
        System.Threading.Tasks.Task<int> GetExchangeServiceIDAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteOrganizationResponse")]
        int DeleteOrganization(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteOrganizationResponse")]
        System.Threading.Tasks.Task<int> DeleteOrganizationAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStorageLimits", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStorageLimitsResponse")]
        SolidCP.Providers.HostedSolution.Organization GetOrganizationStorageLimits(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStorageLimits", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetOrganizationStorageLimitsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationStorageLimitsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetOrganizationStorageLimits", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetOrganizationStorageLimitsResponse")]
        int SetOrganizationStorageLimits(int itemId, int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays, bool applyToMailboxes);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetOrganizationStorageLimits", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetOrganizationStorageLimitsResponse")]
        System.Threading.Tasks.Task<int> SetOrganizationStorageLimitsAsync(int itemId, int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays, bool applyToMailboxes);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxesStatistics", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxesStatisticsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeItemStatistics[] GetMailboxesStatistics(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxesStatistics", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxesStatisticsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]> GetMailboxesStatisticsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxStatistics", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxStatisticsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics GetMailboxStatistics(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxStatistics", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxStatisticsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics> GetMailboxStatisticsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CalculateOrganizationDiskspace", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CalculateOrganizationDiskspaceResponse")]
        int CalculateOrganizationDiskspace(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CalculateOrganizationDiskspace", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CalculateOrganizationDiskspaceResponse")]
        System.Threading.Tasks.Task<int> CalculateOrganizationDiskspaceAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetActiveSyncPolicy", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetActiveSyncPolicyResponse")]
        SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy GetActiveSyncPolicy(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetActiveSyncPolicy", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetActiveSyncPolicyResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy> GetActiveSyncPolicyAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetActiveSyncPolicy", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetActiveSyncPolicyResponse")]
        int SetActiveSyncPolicy(int itemId, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInteval);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetActiveSyncPolicy", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetActiveSyncPolicyResponse")]
        System.Threading.Tasks.Task<int> SetActiveSyncPolicyAsync(int itemId, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInteval);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddAuthoritativeDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddAuthoritativeDomainResponse")]
        int AddAuthoritativeDomain(int itemId, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddAuthoritativeDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddAuthoritativeDomainResponse")]
        System.Threading.Tasks.Task<int> AddAuthoritativeDomainAsync(int itemId, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteAuthoritativeDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteAuthoritativeDomainResponse")]
        int DeleteAuthoritativeDomain(int itemId, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteAuthoritativeDomain", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteAuthoritativeDomainResponse")]
        System.Threading.Tasks.Task<int> DeleteAuthoritativeDomainAsync(int itemId, int domainId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountsPagedResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccountsPaged GetAccountsPaged(int itemId, string accountTypes, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool archiving);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountsPaged", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountsPagedResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccountsPaged> GetAccountsPagedAsync(int itemId, string accountTypes, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool archiving);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ GetAccounts(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetAccountsAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeAccountByMailboxPlanId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeAccountByMailboxPlanIdResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ GetExchangeAccountByMailboxPlanId(int itemId, int mailboxPlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeAccountByMailboxPlanId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeAccountByMailboxPlanIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetExchangeAccountByMailboxPlanIdAsync(int itemId, int mailboxPlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccountsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchAccounts(int itemId, bool includeMailboxes, bool includeContacts, bool includeDistributionLists, bool includeRooms, bool includeEquipment, bool IncludeSharedMailbox, bool includeSecurityGroups, string filterColumn, string filterValue, string sortColumn);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccounts", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccountsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchAccountsAsync(int itemId, bool includeMailboxes, bool includeContacts, bool includeDistributionLists, bool includeRooms, bool includeEquipment, bool IncludeSharedMailbox, bool includeSecurityGroups, string filterColumn, string filterValue, string sortColumn);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccountsByTypes", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccountsByTypesResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchAccountsByTypes(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType[] types, string filterColumn, string filterValue, string sortColumn);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccountsByTypes", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccountsByTypesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchAccountsByTypesAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType[] types, string filterColumn, string filterValue, string sortColumn);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount GetAccount(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount> GetAccountAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountByAccountNameWithoutItemId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountByAccountNameWithoutItemIdResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount GetAccountByAccountNameWithoutItemId(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountByAccountNameWithoutItemId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetAccountByAccountNameWithoutItemIdResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount> GetAccountByAccountNameWithoutItemIdAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccountResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount SearchAccount(SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccount", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SearchAccountResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount> SearchAccountAsync(SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string primaryEmailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CheckAccountCredentials", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CheckAccountCredentialsResponse")]
        bool CheckAccountCredentials(int itemId, string email, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CheckAccountCredentials", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CheckAccountCredentialsResponse")]
        System.Threading.Tasks.Task<bool> CheckAccountCredentialsAsync(int itemId, string email, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateMailbox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateMailboxResponse")]
        int CreateMailbox(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string accountName, string displayName, string name, string domain, string password, bool sendSetupInstructions, string setupInstructionMailAddress, int mailboxPlanId, int archivedPlanId, string subscriberNumber, bool EnableArchiving);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateMailbox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateMailboxResponse")]
        System.Threading.Tasks.Task<int> CreateMailboxAsync(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string accountName, string displayName, string name, string domain, string password, bool sendSetupInstructions, string setupInstructionMailAddress, int mailboxPlanId, int archivedPlanId, string subscriberNumber, bool EnableArchiving);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateJournalRule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateJournalRuleResponse")]
        string CreateJournalRule(int itemId, string journalEmail, string scope, string recipientEmail, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateJournalRule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateJournalRuleResponse")]
        System.Threading.Tasks.Task<string> CreateJournalRuleAsync(int itemId, string journalEmail, string scope, string recipientEmail, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetJournalRule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetJournalRuleResponse")]
        SolidCP.Providers.HostedSolution.ExchangeJournalRule GetJournalRule(int itemId, string journalEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetJournalRule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetJournalRuleResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeJournalRule> GetJournalRuleAsync(int itemId, string journalEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetJournalRule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetJournalRuleResponse")]
        int SetJournalRule(int itemId, SolidCP.Providers.HostedSolution.ExchangeJournalRule rule);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetJournalRule", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetJournalRuleResponse")]
        System.Threading.Tasks.Task<int> SetJournalRuleAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeJournalRule rule);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteMailbox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteMailboxResponse")]
        int DeleteMailbox(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteMailbox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteMailboxResponse")]
        System.Threading.Tasks.Task<int> DeleteMailboxAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DisableMailbox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DisableMailboxResponse")]
        int DisableMailbox(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DisableMailbox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DisableMailboxResponse")]
        System.Threading.Tasks.Task<int> DisableMailboxAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxAdvancedSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxAdvancedSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxAdvancedSettings(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxAdvancedSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxAdvancedSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxAdvancedSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxAutoReplySettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxAutoReplySettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings GetMailboxAutoReplySettings(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxAutoReplySettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxAutoReplySettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings> GetMailboxAutoReplySettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxAutoReplySettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxAutoReplySettingsResponse")]
        int SetMailboxAutoReplySettings(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxAutoReplySettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxAutoReplySettingsResponse")]
        System.Threading.Tasks.Task<int> SetMailboxAutoReplySettingsAsync(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxGeneralSettings(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxGeneralSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxGeneralSettingsResponse")]
        int SetMailboxGeneralSettings(int itemId, int accountId, bool hideAddressBook, bool disabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxGeneralSettingsResponse")]
        System.Threading.Tasks.Task<int> SetMailboxGeneralSettingsAsync(int itemId, int accountId, bool hideAddressBook, bool disabled);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetResourceMailboxSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetResourceMailboxSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings GetResourceMailboxSettings(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetResourceMailboxSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetResourceMailboxSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings> GetResourceMailboxSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetResourceMailboxSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetResourceMailboxSettingsResponse")]
        int SetResourceMailboxSettings(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings resourceSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetResourceMailboxSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetResourceMailboxSettingsResponse")]
        System.Threading.Tasks.Task<int> SetResourceMailboxSettingsAsync(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings resourceSettings);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxEmailAddressesResponse")]
        SolidCP.EnterpriseServer.ExchangeEmailAddress[] GetMailboxEmailAddresses(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxEmailAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ExchangeEmailAddress[]> GetMailboxEmailAddressesAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddMailboxEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddMailboxEmailAddressResponse")]
        int AddMailboxEmailAddress(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddMailboxEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddMailboxEmailAddressResponse")]
        System.Threading.Tasks.Task<int> AddMailboxEmailAddressAsync(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxPrimaryEmailAddressResponse")]
        int SetMailboxPrimaryEmailAddress(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxPrimaryEmailAddressResponse")]
        System.Threading.Tasks.Task<int> SetMailboxPrimaryEmailAddressAsync(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteMailboxEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteMailboxEmailAddressesResponse")]
        int DeleteMailboxEmailAddresses(int itemId, int accountId, string[] emailAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteMailboxEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteMailboxEmailAddressesResponse")]
        System.Threading.Tasks.Task<int> DeleteMailboxEmailAddressesAsync(int itemId, int accountId, string[] emailAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxMailFlowSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxMailFlowSettings(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxMailFlowSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxMailFlowSettingsResponse")]
        int SetMailboxMailFlowSettings(int itemId, int accountId, bool enableForwarding, int SaveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<int> SetMailboxMailFlowSettingsAsync(int itemId, int accountId, bool enableForwarding, int SaveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetExchangeMailboxPlanResponse")]
        int SetExchangeMailboxPlan(int itemId, int accountId, int mailboxPlanId, int archivePlanId, bool EnableArchiving);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetExchangeMailboxPlanResponse")]
        System.Threading.Tasks.Task<int> SetExchangeMailboxPlanAsync(int itemId, int accountId, int mailboxPlanId, int archivePlanId, bool EnableArchiving);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxSetupInstructions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxSetupInstructionsResponse")]
        string GetMailboxSetupInstructions(int itemId, int accountId, bool pmm, bool emailMode, bool signup, string passwordResetUrl);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxSetupInstructions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxSetupInstructionsResponse")]
        System.Threading.Tasks.Task<string> GetMailboxSetupInstructionsAsync(int itemId, int accountId, bool pmm, bool emailMode, bool signup, string passwordResetUrl);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SendMailboxSetupInstructions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SendMailboxSetupInstructionsResponse")]
        int SendMailboxSetupInstructions(int itemId, int accountId, bool signup, string to, string cc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SendMailboxSetupInstructions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SendMailboxSetupInstructionsResponse")]
        System.Threading.Tasks.Task<int> SendMailboxSetupInstructionsAsync(int itemId, int accountId, bool signup, string to, string cc);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxManagerSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxManagerSettingsResponse")]
        int SetMailboxManagerSettings(int itemId, int accountId, bool pmmAllowed, SolidCP.Providers.HostedSolution.MailboxManagerActions action);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxManagerSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxManagerSettingsResponse")]
        System.Threading.Tasks.Task<int> SetMailboxManagerSettingsAsync(int itemId, int accountId, bool pmmAllowed, SolidCP.Providers.HostedSolution.MailboxManagerActions action);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxPermissionsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxPermissions(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMailboxPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxPermissionsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxPermissionsResponse")]
        int SetMailboxPermissions(int itemId, int accountId, string[] sendAsaccounts, string[] fullAccessAcounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetMailboxPermissionsResponse")]
        System.Threading.Tasks.Task<int> SetMailboxPermissionsAsync(int itemId, int accountId, string[] sendAsaccounts, string[] fullAccessAcounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/ExportMailBox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/ExportMailBoxResponse")]
        int ExportMailBox(int itemId, int accountId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/ExportMailBox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/ExportMailBoxResponse")]
        System.Threading.Tasks.Task<int> ExportMailBoxAsync(int itemId, int accountId, string path);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDeletedMailbox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDeletedMailboxResponse")]
        int SetDeletedMailbox(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDeletedMailbox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDeletedMailboxResponse")]
        System.Threading.Tasks.Task<int> SetDeletedMailboxAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateContact", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateContactResponse")]
        int CreateContact(int itemId, string displayName, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateContact", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateContactResponse")]
        System.Threading.Tasks.Task<int> CreateContactAsync(int itemId, string displayName, string email);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteContact", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteContactResponse")]
        int DeleteContact(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteContact", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteContactResponse")]
        System.Threading.Tasks.Task<int> DeleteContactAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetContactGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetContactGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeContact GetContactGeneralSettings(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetContactGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetContactGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactGeneralSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetContactGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetContactGeneralSettingsResponse")]
        int SetContactGeneralSettings(int itemId, int accountId, string displayName, string emailAddress, bool hideAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetContactGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetContactGeneralSettingsResponse")]
        System.Threading.Tasks.Task<int> SetContactGeneralSettingsAsync(int itemId, int accountId, string displayName, string emailAddress, bool hideAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetContactMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetContactMailFlowSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeContact GetContactMailFlowSettings(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetContactMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetContactMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactMailFlowSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetContactMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetContactMailFlowSettingsResponse")]
        int SetContactMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetContactMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetContactMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<int> SetContactMailFlowSettingsAsync(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateDistributionList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateDistributionListResponse")]
        int CreateDistributionList(int itemId, string displayName, string name, string domain, int managerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateDistributionList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreateDistributionListResponse")]
        System.Threading.Tasks.Task<int> CreateDistributionListAsync(int itemId, string displayName, string name, string domain, int managerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionListResponse")]
        int DeleteDistributionList(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionList", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionListResponse")]
        System.Threading.Tasks.Task<int> DeleteDistributionListAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListGeneralSettings(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListGeneralSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListGeneralSettingsResponse")]
        int SetDistributionListGeneralSettings(int itemId, int accountId, string displayName, bool hideAddressBook, string managerAccount, string[] memberAccounts, string notes);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListGeneralSettingsResponse")]
        System.Threading.Tasks.Task<int> SetDistributionListGeneralSettingsAsync(int itemId, int accountId, string displayName, bool hideAddressBook, string managerAccount, string[] memberAccounts, string notes);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListMailFlowSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListMailFlowSettings(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListMailFlowSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListMailFlowSettingsResponse")]
        int SetDistributionListMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<int> SetDistributionListMailFlowSettingsAsync(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListEmailAddressesResponse")]
        SolidCP.EnterpriseServer.ExchangeEmailAddress[] GetDistributionListEmailAddresses(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListEmailAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ExchangeEmailAddress[]> GetDistributionListEmailAddressesAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddDistributionListEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddDistributionListEmailAddressResponse")]
        int AddDistributionListEmailAddress(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddDistributionListEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddDistributionListEmailAddressResponse")]
        System.Threading.Tasks.Task<int> AddDistributionListEmailAddressAsync(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListPrimaryEmailAddressResponse")]
        int SetDistributionListPrimaryEmailAddress(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListPrimaryEmailAddressResponse")]
        System.Threading.Tasks.Task<int> SetDistributionListPrimaryEmailAddressAsync(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionListEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionListEmailAddressesResponse")]
        int DeleteDistributionListEmailAddresses(int itemId, int accountId, string[] emailAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionListEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionListEmailAddressesResponse")]
        System.Threading.Tasks.Task<int> DeleteDistributionListEmailAddressesAsync(int itemId, int accountId, string[] emailAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListPermissionsResponse")]
        SolidCP.Providers.Common.ResultObject SetDistributionListPermissions(int itemId, int accountId, string[] sendAsAccounts, string[] sendOnBehalfAccounts);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDistributionListPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetDistributionListPermissionsAsync(int itemId, int accountId, string[] sendAsAccounts, string[] sendOnBehalfAccounts);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListPermissionsResponse")]
        SolidCP.Providers.ResultObjects.ExchangeDistributionListResult GetDistributionListPermissions(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListPermissions", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.ExchangeDistributionListResult> GetDistributionListPermissionsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListsByMember", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListsByMemberResponse")]
        SolidCP.Providers.HostedSolution.ExchangeAccount[] GetDistributionListsByMember(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListsByMember", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetDistributionListsByMemberResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetDistributionListsByMemberAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddDistributionListMember", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddDistributionListMemberResponse")]
        int AddDistributionListMember(int itemId, string distributionListName, int memberId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddDistributionListMember", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddDistributionListMemberResponse")]
        System.Threading.Tasks.Task<int> AddDistributionListMemberAsync(int itemId, string distributionListName, int memberId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionListMember", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionListMemberResponse")]
        int DeleteDistributionListMember(int itemId, string distributionListName, int memberId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionListMember", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteDistributionListMemberResponse")]
        System.Threading.Tasks.Task<int> DeleteDistributionListMemberAsync(int itemId, string distributionListName, int memberId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMobileDevices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMobileDevicesResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMobileDevice[] GetMobileDevices(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMobileDevices", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMobileDevicesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice[]> GetMobileDevicesAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMobileDevice", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMobileDeviceResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMobileDevice GetMobileDevice(int itemId, string deviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMobileDevice", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetMobileDeviceResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice> GetMobileDeviceAsync(int itemId, string deviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/WipeDataFromDevice", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/WipeDataFromDeviceResponse")]
        void WipeDataFromDevice(int itemId, string deviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/WipeDataFromDevice", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/WipeDataFromDeviceResponse")]
        System.Threading.Tasks.Task WipeDataFromDeviceAsync(int itemId, string deviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CancelRemoteWipeRequest", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CancelRemoteWipeRequestResponse")]
        void CancelRemoteWipeRequest(int itemId, string deviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CancelRemoteWipeRequest", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CancelRemoteWipeRequestResponse")]
        System.Threading.Tasks.Task CancelRemoteWipeRequestAsync(int itemId, string deviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/RemoveDevice", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/RemoveDeviceResponse")]
        void RemoveDevice(int itemId, string deviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/RemoveDevice", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/RemoveDeviceResponse")]
        System.Threading.Tasks.Task RemoveDeviceAsync(int itemId, string deviceId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlans", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlansResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailboxPlan[] /*List*/ GetExchangeMailboxPlans(int itemId, bool archiving);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlans", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlansResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxPlan[]> GetExchangeMailboxPlansAsync(int itemId, bool archiving);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlanResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailboxPlan GetExchangeMailboxPlan(int itemId, int mailboxPlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlanResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxPlan> GetExchangeMailboxPlanAsync(int itemId, int mailboxPlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeMailboxPlanResponse")]
        int AddExchangeMailboxPlan(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeMailboxPlanResponse")]
        System.Threading.Tasks.Task<int> AddExchangeMailboxPlanAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeMailboxPlanResponse")]
        int UpdateExchangeMailboxPlan(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeMailboxPlanResponse")]
        System.Threading.Tasks.Task<int> UpdateExchangeMailboxPlanAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeMailboxPlanResponse")]
        int DeleteExchangeMailboxPlan(int itemId, int mailboxPlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeMailboxPlanResponse")]
        System.Threading.Tasks.Task<int> DeleteExchangeMailboxPlanAsync(int itemId, int mailboxPlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetOrganizationDefaultExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetOrganizationDefaultExchangeMailboxPlanResponse")]
        void SetOrganizationDefaultExchangeMailboxPlan(int itemId, int mailboxPlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetOrganizationDefaultExchangeMailboxPlan", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetOrganizationDefaultExchangeMailboxPlanResponse")]
        System.Threading.Tasks.Task SetOrganizationDefaultExchangeMailboxPlanAsync(int itemId, int mailboxPlanId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeRetentionPolicyTags", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeRetentionPolicyTagsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag[] /*List*/ GetExchangeRetentionPolicyTags(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeRetentionPolicyTags", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeRetentionPolicyTagsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag[]> GetExchangeRetentionPolicyTagsAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeRetentionPolicyTagResponse")]
        SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag GetExchangeRetentionPolicyTag(int itemId, int tagId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeRetentionPolicyTagResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag> GetExchangeRetentionPolicyTagAsync(int itemId, int tagId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeRetentionPolicyTagResponse")]
        SolidCP.Providers.ResultObjects.IntResult AddExchangeRetentionPolicyTag(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeRetentionPolicyTagResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddExchangeRetentionPolicyTagAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeRetentionPolicyTagResponse")]
        SolidCP.Providers.Common.ResultObject UpdateExchangeRetentionPolicyTag(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeRetentionPolicyTagResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateExchangeRetentionPolicyTagAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeRetentionPolicyTagResponse")]
        SolidCP.Providers.Common.ResultObject DeleteExchangeRetentionPolicyTag(int itemId, int tagId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeRetentionPolicyTagResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteExchangeRetentionPolicyTagAsync(int itemId, int tagId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlanRetentionPolicyTags", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlanRetentionPolicyTagsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag[] /*List*/ GetExchangeMailboxPlanRetentionPolicyTags(int policyId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlanRetentionPolicyTags", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeMailboxPlanRetentionPolicyTagsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag[]> GetExchangeMailboxPlanRetentionPolicyTagsAsync(int policyId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeMailboxPlanRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeMailboxPlanRetentionPolicyTagResponse")]
        SolidCP.Providers.ResultObjects.IntResult AddExchangeMailboxPlanRetentionPolicyTag(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag planTag);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeMailboxPlanRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeMailboxPlanRetentionPolicyTagResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddExchangeMailboxPlanRetentionPolicyTagAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag planTag);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeMailboxPlanRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeMailboxPlanRetentionPolicyTagResponse")]
        SolidCP.Providers.Common.ResultObject DeleteExchangeMailboxPlanRetentionPolicyTag(int itemID, int policyId, int planTagId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeMailboxPlanRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeMailboxPlanRetentionPolicyTagResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteExchangeMailboxPlanRetentionPolicyTagAsync(int itemID, int policyId, int planTagId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreatePublicFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreatePublicFolderResponse")]
        int CreatePublicFolder(int itemId, string parentFolder, string folderName, bool mailEnabled, string accountName, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreatePublicFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/CreatePublicFolderResponse")]
        System.Threading.Tasks.Task<int> CreatePublicFolderAsync(int itemId, string parentFolder, string folderName, bool mailEnabled, string accountName, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFoldersResponse")]
        int DeletePublicFolders(int itemId, int[] accountIds);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFolders", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFoldersResponse")]
        System.Threading.Tasks.Task<int> DeletePublicFoldersAsync(int itemId, int[] accountIds);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFolderResponse")]
        int DeletePublicFolder(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFolderResponse")]
        System.Threading.Tasks.Task<int> DeletePublicFolderAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/EnableMailPublicFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/EnableMailPublicFolderResponse")]
        int EnableMailPublicFolder(int itemId, int accountId, string name, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/EnableMailPublicFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/EnableMailPublicFolderResponse")]
        System.Threading.Tasks.Task<int> EnableMailPublicFolderAsync(int itemId, int accountId, string name, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DisableMailPublicFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DisableMailPublicFolderResponse")]
        int DisableMailPublicFolder(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DisableMailPublicFolder", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DisableMailPublicFolderResponse")]
        System.Threading.Tasks.Task<int> DisableMailPublicFolderAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderGeneralSettings(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderGeneralSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderGeneralSettingsResponse")]
        int SetPublicFolderGeneralSettings(int itemId, int accountId, string newName, bool hideAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderGeneralSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderGeneralSettingsResponse")]
        System.Threading.Tasks.Task<int> SetPublicFolderGeneralSettingsAsync(int itemId, int accountId, string newName, bool hideAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderMailFlowSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderMailFlowSettings(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderMailFlowSettingsAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderMailFlowSettingsResponse")]
        int SetPublicFolderMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<int> SetPublicFolderMailFlowSettingsAsync(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderEmailAddressesResponse")]
        SolidCP.EnterpriseServer.ExchangeEmailAddress[] GetPublicFolderEmailAddresses(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPublicFolderEmailAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ExchangeEmailAddress[]> GetPublicFolderEmailAddressesAsync(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddPublicFolderEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddPublicFolderEmailAddressResponse")]
        int AddPublicFolderEmailAddress(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddPublicFolderEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddPublicFolderEmailAddressResponse")]
        System.Threading.Tasks.Task<int> AddPublicFolderEmailAddressAsync(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderPrimaryEmailAddressResponse")]
        int SetPublicFolderPrimaryEmailAddress(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPublicFolderPrimaryEmailAddressResponse")]
        System.Threading.Tasks.Task<int> SetPublicFolderPrimaryEmailAddressAsync(int itemId, int accountId, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFolderEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFolderEmailAddressesResponse")]
        int DeletePublicFolderEmailAddresses(int itemId, int accountId, string[] emailAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFolderEmailAddresses", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeletePublicFolderEmailAddressesResponse")]
        System.Threading.Tasks.Task<int> DeletePublicFolderEmailAddressesAsync(int itemId, int accountId, string[] emailAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDefaultPublicFolderMailbox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDefaultPublicFolderMailboxResponse")]
        string SetDefaultPublicFolderMailbox(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDefaultPublicFolderMailbox", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetDefaultPublicFolderMailboxResponse")]
        System.Threading.Tasks.Task<string> SetDefaultPublicFolderMailboxAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeDisclaimer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeDisclaimerResponse")]
        int AddExchangeDisclaimer(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeDisclaimer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/AddExchangeDisclaimerResponse")]
        System.Threading.Tasks.Task<int> AddExchangeDisclaimerAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeDisclaimer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeDisclaimerResponse")]
        int UpdateExchangeDisclaimer(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeDisclaimer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/UpdateExchangeDisclaimerResponse")]
        System.Threading.Tasks.Task<int> UpdateExchangeDisclaimerAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeDisclaimer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeDisclaimerResponse")]
        int DeleteExchangeDisclaimer(int itemId, int exchangeDisclaimerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeDisclaimer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/DeleteExchangeDisclaimerResponse")]
        System.Threading.Tasks.Task<int> DeleteExchangeDisclaimerAsync(int itemId, int exchangeDisclaimerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeDisclaimer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeDisclaimerResponse")]
        SolidCP.Providers.HostedSolution.ExchangeDisclaimer GetExchangeDisclaimer(int itemId, int exchangeDisclaimerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeDisclaimer", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeDisclaimerResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDisclaimer> GetExchangeDisclaimerAsync(int itemId, int exchangeDisclaimerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeDisclaimers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeDisclaimersResponse")]
        SolidCP.Providers.HostedSolution.ExchangeDisclaimer[] /*List*/ GetExchangeDisclaimers(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeDisclaimers", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeDisclaimersResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDisclaimer[]> GetExchangeDisclaimersAsync(int itemId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetExchangeAccountDisclaimerId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetExchangeAccountDisclaimerIdResponse")]
        int SetExchangeAccountDisclaimerId(int itemId, int AccountID, int ExchangeDisclaimerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetExchangeAccountDisclaimerId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetExchangeAccountDisclaimerIdResponse")]
        System.Threading.Tasks.Task<int> SetExchangeAccountDisclaimerIdAsync(int itemId, int AccountID, int ExchangeDisclaimerId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeAccountDisclaimerId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeAccountDisclaimerIdResponse")]
        int GetExchangeAccountDisclaimerId(int itemId, int AccountID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeAccountDisclaimerId", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetExchangeAccountDisclaimerIdResponse")]
        System.Threading.Tasks.Task<int> GetExchangeAccountDisclaimerIdAsync(int itemId, int AccountID);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPicture", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPictureResponse")]
        SolidCP.Providers.Common.ResultObject SetPicture(int itemId, int accountId, byte[] picture);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPicture", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/SetPictureResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetPictureAsync(int itemId, int accountId, byte[] picture);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPicture", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPictureResponse")]
        SolidCP.Providers.ResultObjects.BytesResult GetPicture(int itemId, int accountId);
        [OperationContract(Action = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPicture", ReplyAction = "http://smbsaas/solidcp/enterpriseserver/IesExchangeServer/GetPictureResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BytesResult> GetPictureAsync(int itemId, int accountId);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esExchangeServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IesExchangeServer
    {
        public System.Data.DataSet GetRawExchangeOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<System.Data.DataSet>("SolidCP.EnterpriseServer.esExchangeServer", "GetRawExchangeOrganizationsPaged", packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawExchangeOrganizationsPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<System.Data.DataSet>("SolidCP.EnterpriseServer.esExchangeServer", "GetRawExchangeOrganizationsPaged", packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.OrganizationsPaged GetExchangeOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return Invoke<SolidCP.Providers.HostedSolution.OrganizationsPaged>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeOrganizationsPaged", packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationsPaged> GetExchangeOrganizationsPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.OrganizationsPaged>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeOrganizationsPaged", packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.Organization[] /*List*/ GetExchangeOrganizations(int packageId, bool recursive)
        {
            return Invoke<SolidCP.Providers.HostedSolution.Organization[], SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeOrganizations", packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization[]> GetExchangeOrganizationsAsync(int packageId, bool recursive)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.Organization[], SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeOrganizations", packageId, recursive);
        }

        public SolidCP.Providers.HostedSolution.Organization GetOrganization(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esExchangeServer", "GetOrganization", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esExchangeServer", "GetOrganization", itemId);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetOrganizationStatistics(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esExchangeServer", "GetOrganizationStatistics", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetOrganizationStatisticsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esExchangeServer", "GetOrganizationStatistics", itemId);
        }

        public SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics GetOrganizationStatisticsByOrganization(int itemId)
        {
            return Invoke<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esExchangeServer", "GetOrganizationStatisticsByOrganization", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics> GetOrganizationStatisticsByOrganizationAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.Base.HostedSolution.OrganizationStatistics>("SolidCP.EnterpriseServer.esExchangeServer", "GetOrganizationStatisticsByOrganization", itemId);
        }

        public int GetExchangeServiceID(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeServiceID", itemId);
        }

        public async System.Threading.Tasks.Task<int> GetExchangeServiceIDAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeServiceID", itemId);
        }

        public int DeleteOrganization(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteOrganization", itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteOrganizationAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteOrganization", itemId);
        }

        public SolidCP.Providers.HostedSolution.Organization GetOrganizationStorageLimits(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esExchangeServer", "GetOrganizationStorageLimits", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationStorageLimitsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.Organization>("SolidCP.EnterpriseServer.esExchangeServer", "GetOrganizationStorageLimits", itemId);
        }

        public int SetOrganizationStorageLimits(int itemId, int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays, bool applyToMailboxes)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetOrganizationStorageLimits", itemId, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, applyToMailboxes);
        }

        public async System.Threading.Tasks.Task<int> SetOrganizationStorageLimitsAsync(int itemId, int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays, bool applyToMailboxes)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetOrganizationStorageLimits", itemId, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, applyToMailboxes);
        }

        public SolidCP.Providers.HostedSolution.ExchangeItemStatistics[] GetMailboxesStatistics(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxesStatistics", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]> GetMailboxesStatisticsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxesStatistics", itemId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics GetMailboxStatistics(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxStatistics", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics> GetMailboxStatisticsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxStatistics", itemId, accountId);
        }

        public int CalculateOrganizationDiskspace(int itemId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "CalculateOrganizationDiskspace", itemId);
        }

        public async System.Threading.Tasks.Task<int> CalculateOrganizationDiskspaceAsync(int itemId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "CalculateOrganizationDiskspace", itemId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy GetActiveSyncPolicy(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy>("SolidCP.EnterpriseServer.esExchangeServer", "GetActiveSyncPolicy", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy> GetActiveSyncPolicyAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy>("SolidCP.EnterpriseServer.esExchangeServer", "GetActiveSyncPolicy", itemId);
        }

        public int SetActiveSyncPolicy(int itemId, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInteval)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetActiveSyncPolicy", itemId, allowNonProvisionableDevices, attachmentsEnabled, maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired, passwordRecoveryEnabled, deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts, minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInteval);
        }

        public async System.Threading.Tasks.Task<int> SetActiveSyncPolicyAsync(int itemId, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInteval)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetActiveSyncPolicy", itemId, allowNonProvisionableDevices, attachmentsEnabled, maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired, passwordRecoveryEnabled, deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts, minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInteval);
        }

        public int AddAuthoritativeDomain(int itemId, int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddAuthoritativeDomain", itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> AddAuthoritativeDomainAsync(int itemId, int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddAuthoritativeDomain", itemId, domainId);
        }

        public int DeleteAuthoritativeDomain(int itemId, int domainId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteAuthoritativeDomain", itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteAuthoritativeDomainAsync(int itemId, int domainId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteAuthoritativeDomain", itemId, domainId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccountsPaged GetAccountsPaged(int itemId, string accountTypes, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool archiving)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccountsPaged>("SolidCP.EnterpriseServer.esExchangeServer", "GetAccountsPaged", itemId, accountTypes, filterColumn, filterValue, sortColumn, startRow, maximumRows, archiving);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccountsPaged> GetAccountsPagedAsync(int itemId, string accountTypes, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool archiving)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccountsPaged>("SolidCP.EnterpriseServer.esExchangeServer", "GetAccountsPaged", itemId, accountTypes, filterColumn, filterValue, sortColumn, startRow, maximumRows, archiving);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ GetAccounts(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "GetAccounts", itemId, accountType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetAccountsAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "GetAccounts", itemId, accountType);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ GetExchangeAccountByMailboxPlanId(int itemId, int mailboxPlanId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeAccountByMailboxPlanId", itemId, mailboxPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetExchangeAccountByMailboxPlanIdAsync(int itemId, int mailboxPlanId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeAccountByMailboxPlanId", itemId, mailboxPlanId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchAccounts(int itemId, bool includeMailboxes, bool includeContacts, bool includeDistributionLists, bool includeRooms, bool includeEquipment, bool IncludeSharedMailbox, bool includeSecurityGroups, string filterColumn, string filterValue, string sortColumn)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "SearchAccounts", itemId, includeMailboxes, includeContacts, includeDistributionLists, includeRooms, includeEquipment, IncludeSharedMailbox, includeSecurityGroups, filterColumn, filterValue, sortColumn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchAccountsAsync(int itemId, bool includeMailboxes, bool includeContacts, bool includeDistributionLists, bool includeRooms, bool includeEquipment, bool IncludeSharedMailbox, bool includeSecurityGroups, string filterColumn, string filterValue, string sortColumn)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "SearchAccounts", itemId, includeMailboxes, includeContacts, includeDistributionLists, includeRooms, includeEquipment, IncludeSharedMailbox, includeSecurityGroups, filterColumn, filterValue, sortColumn);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchAccountsByTypes(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType[] types, string filterColumn, string filterValue, string sortColumn)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "SearchAccountsByTypes", itemId, types, filterColumn, filterValue, sortColumn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchAccountsByTypesAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType[] types, string filterColumn, string filterValue, string sortColumn)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount[], SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "SearchAccountsByTypes", itemId, types, filterColumn, filterValue, sortColumn);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount GetAccount(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "GetAccount", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount> GetAccountAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "GetAccount", itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount GetAccountByAccountNameWithoutItemId(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "GetAccountByAccountNameWithoutItemId", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount> GetAccountByAccountNameWithoutItemIdAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "GetAccountByAccountNameWithoutItemId", accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount SearchAccount(SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string primaryEmailAddress)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "SearchAccount", accountType, primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount> SearchAccountAsync(SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string primaryEmailAddress)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount>("SolidCP.EnterpriseServer.esExchangeServer", "SearchAccount", accountType, primaryEmailAddress);
        }

        public bool CheckAccountCredentials(int itemId, string email, string password)
        {
            return Invoke<bool>("SolidCP.EnterpriseServer.esExchangeServer", "CheckAccountCredentials", itemId, email, password);
        }

        public async System.Threading.Tasks.Task<bool> CheckAccountCredentialsAsync(int itemId, string email, string password)
        {
            return await InvokeAsync<bool>("SolidCP.EnterpriseServer.esExchangeServer", "CheckAccountCredentials", itemId, email, password);
        }

        public int CreateMailbox(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string accountName, string displayName, string name, string domain, string password, bool sendSetupInstructions, string setupInstructionMailAddress, int mailboxPlanId, int archivedPlanId, string subscriberNumber, bool EnableArchiving)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "CreateMailbox", itemId, accountId, accountType, accountName, displayName, name, domain, password, sendSetupInstructions, setupInstructionMailAddress, mailboxPlanId, archivedPlanId, subscriberNumber, EnableArchiving);
        }

        public async System.Threading.Tasks.Task<int> CreateMailboxAsync(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string accountName, string displayName, string name, string domain, string password, bool sendSetupInstructions, string setupInstructionMailAddress, int mailboxPlanId, int archivedPlanId, string subscriberNumber, bool EnableArchiving)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "CreateMailbox", itemId, accountId, accountType, accountName, displayName, name, domain, password, sendSetupInstructions, setupInstructionMailAddress, mailboxPlanId, archivedPlanId, subscriberNumber, EnableArchiving);
        }

        public string CreateJournalRule(int itemId, string journalEmail, string scope, string recipientEmail, bool enabled)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esExchangeServer", "CreateJournalRule", itemId, journalEmail, scope, recipientEmail, enabled);
        }

        public async System.Threading.Tasks.Task<string> CreateJournalRuleAsync(int itemId, string journalEmail, string scope, string recipientEmail, bool enabled)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esExchangeServer", "CreateJournalRule", itemId, journalEmail, scope, recipientEmail, enabled);
        }

        public SolidCP.Providers.HostedSolution.ExchangeJournalRule GetJournalRule(int itemId, string journalEmail)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeJournalRule>("SolidCP.EnterpriseServer.esExchangeServer", "GetJournalRule", itemId, journalEmail);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeJournalRule> GetJournalRuleAsync(int itemId, string journalEmail)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeJournalRule>("SolidCP.EnterpriseServer.esExchangeServer", "GetJournalRule", itemId, journalEmail);
        }

        public int SetJournalRule(int itemId, SolidCP.Providers.HostedSolution.ExchangeJournalRule rule)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetJournalRule", itemId, rule);
        }

        public async System.Threading.Tasks.Task<int> SetJournalRuleAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeJournalRule rule)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetJournalRule", itemId, rule);
        }

        public int DeleteMailbox(int itemId, int accountId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteMailbox", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailboxAsync(int itemId, int accountId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteMailbox", itemId, accountId);
        }

        public int DisableMailbox(int itemId, int accountId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DisableMailbox", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DisableMailboxAsync(int itemId, int accountId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DisableMailbox", itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxAdvancedSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxAdvancedSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxAdvancedSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxAdvancedSettings", itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings GetMailboxAutoReplySettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxAutoReplySettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings> GetMailboxAutoReplySettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxAutoReplySettings", itemId, accountId);
        }

        public int SetMailboxAutoReplySettings(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxAutoReplySettings", itemId, accountId, settings);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxAutoReplySettingsAsync(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxAutoReplySettings", itemId, accountId, settings);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxGeneralSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxGeneralSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxGeneralSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxGeneralSettings", itemId, accountId);
        }

        public int SetMailboxGeneralSettings(int itemId, int accountId, bool hideAddressBook, bool disabled)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxGeneralSettings", itemId, accountId, hideAddressBook, disabled);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxGeneralSettingsAsync(int itemId, int accountId, bool hideAddressBook, bool disabled)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxGeneralSettings", itemId, accountId, hideAddressBook, disabled);
        }

        public SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings GetResourceMailboxSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings>("SolidCP.EnterpriseServer.esExchangeServer", "GetResourceMailboxSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings> GetResourceMailboxSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings>("SolidCP.EnterpriseServer.esExchangeServer", "GetResourceMailboxSettings", itemId, accountId);
        }

        public int SetResourceMailboxSettings(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings resourceSettings)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetResourceMailboxSettings", itemId, accountId, resourceSettings);
        }

        public async System.Threading.Tasks.Task<int> SetResourceMailboxSettingsAsync(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings resourceSettings)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetResourceMailboxSettings", itemId, accountId, resourceSettings);
        }

        public SolidCP.EnterpriseServer.ExchangeEmailAddress[] GetMailboxEmailAddresses(int itemId, int accountId)
        {
            return Invoke<SolidCP.EnterpriseServer.ExchangeEmailAddress[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxEmailAddresses", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ExchangeEmailAddress[]> GetMailboxEmailAddressesAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ExchangeEmailAddress[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxEmailAddresses", itemId, accountId);
        }

        public int AddMailboxEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddMailboxEmailAddress", itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> AddMailboxEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddMailboxEmailAddress", itemId, accountId, emailAddress);
        }

        public int SetMailboxPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxPrimaryEmailAddress", itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxPrimaryEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxPrimaryEmailAddress", itemId, accountId, emailAddress);
        }

        public int DeleteMailboxEmailAddresses(int itemId, int accountId, string[] emailAddresses)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteMailboxEmailAddresses", itemId, accountId, emailAddresses);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailboxEmailAddressesAsync(int itemId, int accountId, string[] emailAddresses)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteMailboxEmailAddresses", itemId, accountId, emailAddresses);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxMailFlowSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxMailFlowSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxMailFlowSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxMailFlowSettings", itemId, accountId);
        }

        public int SetMailboxMailFlowSettings(int itemId, int accountId, bool enableForwarding, int SaveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxMailFlowSettings", itemId, accountId, enableForwarding, SaveSentItems, forwardingAccountName, forwardToBoth, sendOnBehalfAccounts, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxMailFlowSettingsAsync(int itemId, int accountId, bool enableForwarding, int SaveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxMailFlowSettings", itemId, accountId, enableForwarding, SaveSentItems, forwardingAccountName, forwardToBoth, sendOnBehalfAccounts, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public int SetExchangeMailboxPlan(int itemId, int accountId, int mailboxPlanId, int archivePlanId, bool EnableArchiving)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetExchangeMailboxPlan", itemId, accountId, mailboxPlanId, archivePlanId, EnableArchiving);
        }

        public async System.Threading.Tasks.Task<int> SetExchangeMailboxPlanAsync(int itemId, int accountId, int mailboxPlanId, int archivePlanId, bool EnableArchiving)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetExchangeMailboxPlan", itemId, accountId, mailboxPlanId, archivePlanId, EnableArchiving);
        }

        public string GetMailboxSetupInstructions(int itemId, int accountId, bool pmm, bool emailMode, bool signup, string passwordResetUrl)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxSetupInstructions", itemId, accountId, pmm, emailMode, signup, passwordResetUrl);
        }

        public async System.Threading.Tasks.Task<string> GetMailboxSetupInstructionsAsync(int itemId, int accountId, bool pmm, bool emailMode, bool signup, string passwordResetUrl)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxSetupInstructions", itemId, accountId, pmm, emailMode, signup, passwordResetUrl);
        }

        public int SendMailboxSetupInstructions(int itemId, int accountId, bool signup, string to, string cc)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SendMailboxSetupInstructions", itemId, accountId, signup, to, cc);
        }

        public async System.Threading.Tasks.Task<int> SendMailboxSetupInstructionsAsync(int itemId, int accountId, bool signup, string to, string cc)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SendMailboxSetupInstructions", itemId, accountId, signup, to, cc);
        }

        public int SetMailboxManagerSettings(int itemId, int accountId, bool pmmAllowed, SolidCP.Providers.HostedSolution.MailboxManagerActions action)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxManagerSettings", itemId, accountId, pmmAllowed, action);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxManagerSettingsAsync(int itemId, int accountId, bool pmmAllowed, SolidCP.Providers.HostedSolution.MailboxManagerActions action)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxManagerSettings", itemId, accountId, pmmAllowed, action);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxPermissions(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxPermissions", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxPermissionsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.EnterpriseServer.esExchangeServer", "GetMailboxPermissions", itemId, accountId);
        }

        public int SetMailboxPermissions(int itemId, int accountId, string[] sendAsaccounts, string[] fullAccessAcounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxPermissions", itemId, accountId, sendAsaccounts, fullAccessAcounts, onBehalfOfAccounts, calendarAccounts, contactAccounts);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxPermissionsAsync(int itemId, int accountId, string[] sendAsaccounts, string[] fullAccessAcounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetMailboxPermissions", itemId, accountId, sendAsaccounts, fullAccessAcounts, onBehalfOfAccounts, calendarAccounts, contactAccounts);
        }

        public int ExportMailBox(int itemId, int accountId, string path)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "ExportMailBox", itemId, accountId, path);
        }

        public async System.Threading.Tasks.Task<int> ExportMailBoxAsync(int itemId, int accountId, string path)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "ExportMailBox", itemId, accountId, path);
        }

        public int SetDeletedMailbox(int itemId, int accountId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetDeletedMailbox", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> SetDeletedMailboxAsync(int itemId, int accountId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetDeletedMailbox", itemId, accountId);
        }

        public int CreateContact(int itemId, string displayName, string email)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "CreateContact", itemId, displayName, email);
        }

        public async System.Threading.Tasks.Task<int> CreateContactAsync(int itemId, string displayName, string email)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "CreateContact", itemId, displayName, email);
        }

        public int DeleteContact(int itemId, int accountId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteContact", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeleteContactAsync(int itemId, int accountId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteContact", itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeContact GetContactGeneralSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeContact>("SolidCP.EnterpriseServer.esExchangeServer", "GetContactGeneralSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactGeneralSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeContact>("SolidCP.EnterpriseServer.esExchangeServer", "GetContactGeneralSettings", itemId, accountId);
        }

        public int SetContactGeneralSettings(int itemId, int accountId, string displayName, string emailAddress, bool hideAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetContactGeneralSettings", itemId, accountId, displayName, emailAddress, hideAddressBook, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, useMapiRichTextFormat);
        }

        public async System.Threading.Tasks.Task<int> SetContactGeneralSettingsAsync(int itemId, int accountId, string displayName, string emailAddress, bool hideAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetContactGeneralSettings", itemId, accountId, displayName, emailAddress, hideAddressBook, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, useMapiRichTextFormat);
        }

        public SolidCP.Providers.HostedSolution.ExchangeContact GetContactMailFlowSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeContact>("SolidCP.EnterpriseServer.esExchangeServer", "GetContactMailFlowSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactMailFlowSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeContact>("SolidCP.EnterpriseServer.esExchangeServer", "GetContactMailFlowSettings", itemId, accountId);
        }

        public int SetContactMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetContactMailFlowSettings", itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task<int> SetContactMailFlowSettingsAsync(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetContactMailFlowSettings", itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public int CreateDistributionList(int itemId, string displayName, string name, string domain, int managerId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "CreateDistributionList", itemId, displayName, name, domain, managerId);
        }

        public async System.Threading.Tasks.Task<int> CreateDistributionListAsync(int itemId, string displayName, string name, string domain, int managerId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "CreateDistributionList", itemId, displayName, name, domain, managerId);
        }

        public int DeleteDistributionList(int itemId, int accountId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteDistributionList", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeleteDistributionListAsync(int itemId, int accountId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteDistributionList", itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListGeneralSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeDistributionList>("SolidCP.EnterpriseServer.esExchangeServer", "GetDistributionListGeneralSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListGeneralSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeDistributionList>("SolidCP.EnterpriseServer.esExchangeServer", "GetDistributionListGeneralSettings", itemId, accountId);
        }

        public int SetDistributionListGeneralSettings(int itemId, int accountId, string displayName, bool hideAddressBook, string managerAccount, string[] memberAccounts, string notes)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetDistributionListGeneralSettings", itemId, accountId, displayName, hideAddressBook, managerAccount, memberAccounts, notes);
        }

        public async System.Threading.Tasks.Task<int> SetDistributionListGeneralSettingsAsync(int itemId, int accountId, string displayName, bool hideAddressBook, string managerAccount, string[] memberAccounts, string notes)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetDistributionListGeneralSettings", itemId, accountId, displayName, hideAddressBook, managerAccount, memberAccounts, notes);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListMailFlowSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeDistributionList>("SolidCP.EnterpriseServer.esExchangeServer", "GetDistributionListMailFlowSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListMailFlowSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeDistributionList>("SolidCP.EnterpriseServer.esExchangeServer", "GetDistributionListMailFlowSettings", itemId, accountId);
        }

        public int SetDistributionListMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetDistributionListMailFlowSettings", itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task<int> SetDistributionListMailFlowSettingsAsync(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetDistributionListMailFlowSettings", itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public SolidCP.EnterpriseServer.ExchangeEmailAddress[] GetDistributionListEmailAddresses(int itemId, int accountId)
        {
            return Invoke<SolidCP.EnterpriseServer.ExchangeEmailAddress[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetDistributionListEmailAddresses", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ExchangeEmailAddress[]> GetDistributionListEmailAddressesAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ExchangeEmailAddress[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetDistributionListEmailAddresses", itemId, accountId);
        }

        public int AddDistributionListEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddDistributionListEmailAddress", itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> AddDistributionListEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddDistributionListEmailAddress", itemId, accountId, emailAddress);
        }

        public int SetDistributionListPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetDistributionListPrimaryEmailAddress", itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> SetDistributionListPrimaryEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetDistributionListPrimaryEmailAddress", itemId, accountId, emailAddress);
        }

        public int DeleteDistributionListEmailAddresses(int itemId, int accountId, string[] emailAddresses)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteDistributionListEmailAddresses", itemId, accountId, emailAddresses);
        }

        public async System.Threading.Tasks.Task<int> DeleteDistributionListEmailAddressesAsync(int itemId, int accountId, string[] emailAddresses)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteDistributionListEmailAddresses", itemId, accountId, emailAddresses);
        }

        public SolidCP.Providers.Common.ResultObject SetDistributionListPermissions(int itemId, int accountId, string[] sendAsAccounts, string[] sendOnBehalfAccounts)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esExchangeServer", "SetDistributionListPermissions", itemId, accountId, sendAsAccounts, sendOnBehalfAccounts);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetDistributionListPermissionsAsync(int itemId, int accountId, string[] sendAsAccounts, string[] sendOnBehalfAccounts)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esExchangeServer", "SetDistributionListPermissions", itemId, accountId, sendAsAccounts, sendOnBehalfAccounts);
        }

        public SolidCP.Providers.ResultObjects.ExchangeDistributionListResult GetDistributionListPermissions(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.ExchangeDistributionListResult>("SolidCP.EnterpriseServer.esExchangeServer", "GetDistributionListPermissions", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.ExchangeDistributionListResult> GetDistributionListPermissionsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.ExchangeDistributionListResult>("SolidCP.EnterpriseServer.esExchangeServer", "GetDistributionListPermissions", itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] GetDistributionListsByMember(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeAccount[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetDistributionListsByMember", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetDistributionListsByMemberAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeAccount[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetDistributionListsByMember", itemId, accountId);
        }

        public int AddDistributionListMember(int itemId, string distributionListName, int memberId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddDistributionListMember", itemId, distributionListName, memberId);
        }

        public async System.Threading.Tasks.Task<int> AddDistributionListMemberAsync(int itemId, string distributionListName, int memberId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddDistributionListMember", itemId, distributionListName, memberId);
        }

        public int DeleteDistributionListMember(int itemId, string distributionListName, int memberId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteDistributionListMember", itemId, distributionListName, memberId);
        }

        public async System.Threading.Tasks.Task<int> DeleteDistributionListMemberAsync(int itemId, string distributionListName, int memberId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteDistributionListMember", itemId, distributionListName, memberId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMobileDevice[] GetMobileDevices(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMobileDevice[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetMobileDevices", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice[]> GetMobileDevicesAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMobileDevice[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetMobileDevices", itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMobileDevice GetMobileDevice(int itemId, string deviceId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMobileDevice>("SolidCP.EnterpriseServer.esExchangeServer", "GetMobileDevice", itemId, deviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice> GetMobileDeviceAsync(int itemId, string deviceId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMobileDevice>("SolidCP.EnterpriseServer.esExchangeServer", "GetMobileDevice", itemId, deviceId);
        }

        public void WipeDataFromDevice(int itemId, string deviceId)
        {
            Invoke("SolidCP.EnterpriseServer.esExchangeServer", "WipeDataFromDevice", itemId, deviceId);
        }

        public async System.Threading.Tasks.Task WipeDataFromDeviceAsync(int itemId, string deviceId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esExchangeServer", "WipeDataFromDevice", itemId, deviceId);
        }

        public void CancelRemoteWipeRequest(int itemId, string deviceId)
        {
            Invoke("SolidCP.EnterpriseServer.esExchangeServer", "CancelRemoteWipeRequest", itemId, deviceId);
        }

        public async System.Threading.Tasks.Task CancelRemoteWipeRequestAsync(int itemId, string deviceId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esExchangeServer", "CancelRemoteWipeRequest", itemId, deviceId);
        }

        public void RemoveDevice(int itemId, string deviceId)
        {
            Invoke("SolidCP.EnterpriseServer.esExchangeServer", "RemoveDevice", itemId, deviceId);
        }

        public async System.Threading.Tasks.Task RemoveDeviceAsync(int itemId, string deviceId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esExchangeServer", "RemoveDevice", itemId, deviceId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxPlan[] /*List*/ GetExchangeMailboxPlans(int itemId, bool archiving)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailboxPlan[], SolidCP.Providers.HostedSolution.ExchangeMailboxPlan>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeMailboxPlans", itemId, archiving);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxPlan[]> GetExchangeMailboxPlansAsync(int itemId, bool archiving)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailboxPlan[], SolidCP.Providers.HostedSolution.ExchangeMailboxPlan>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeMailboxPlans", itemId, archiving);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxPlan GetExchangeMailboxPlan(int itemId, int mailboxPlanId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailboxPlan>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeMailboxPlan", itemId, mailboxPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxPlan> GetExchangeMailboxPlanAsync(int itemId, int mailboxPlanId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailboxPlan>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeMailboxPlan", itemId, mailboxPlanId);
        }

        public int AddExchangeMailboxPlan(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddExchangeMailboxPlan", itemId, mailboxPlan);
        }

        public async System.Threading.Tasks.Task<int> AddExchangeMailboxPlanAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddExchangeMailboxPlan", itemId, mailboxPlan);
        }

        public int UpdateExchangeMailboxPlan(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "UpdateExchangeMailboxPlan", itemId, mailboxPlan);
        }

        public async System.Threading.Tasks.Task<int> UpdateExchangeMailboxPlanAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "UpdateExchangeMailboxPlan", itemId, mailboxPlan);
        }

        public int DeleteExchangeMailboxPlan(int itemId, int mailboxPlanId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteExchangeMailboxPlan", itemId, mailboxPlanId);
        }

        public async System.Threading.Tasks.Task<int> DeleteExchangeMailboxPlanAsync(int itemId, int mailboxPlanId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteExchangeMailboxPlan", itemId, mailboxPlanId);
        }

        public void SetOrganizationDefaultExchangeMailboxPlan(int itemId, int mailboxPlanId)
        {
            Invoke("SolidCP.EnterpriseServer.esExchangeServer", "SetOrganizationDefaultExchangeMailboxPlan", itemId, mailboxPlanId);
        }

        public async System.Threading.Tasks.Task SetOrganizationDefaultExchangeMailboxPlanAsync(int itemId, int mailboxPlanId)
        {
            await InvokeAsync("SolidCP.EnterpriseServer.esExchangeServer", "SetOrganizationDefaultExchangeMailboxPlan", itemId, mailboxPlanId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag[] /*List*/ GetExchangeRetentionPolicyTags(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag[], SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeRetentionPolicyTags", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag[]> GetExchangeRetentionPolicyTagsAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag[], SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeRetentionPolicyTags", itemId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag GetExchangeRetentionPolicyTag(int itemId, int tagId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeRetentionPolicyTag", itemId, tagId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag> GetExchangeRetentionPolicyTagAsync(int itemId, int tagId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeRetentionPolicyTag", itemId, tagId);
        }

        public SolidCP.Providers.ResultObjects.IntResult AddExchangeRetentionPolicyTag(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esExchangeServer", "AddExchangeRetentionPolicyTag", itemId, tag);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddExchangeRetentionPolicyTagAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esExchangeServer", "AddExchangeRetentionPolicyTag", itemId, tag);
        }

        public SolidCP.Providers.Common.ResultObject UpdateExchangeRetentionPolicyTag(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esExchangeServer", "UpdateExchangeRetentionPolicyTag", itemId, tag);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateExchangeRetentionPolicyTagAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esExchangeServer", "UpdateExchangeRetentionPolicyTag", itemId, tag);
        }

        public SolidCP.Providers.Common.ResultObject DeleteExchangeRetentionPolicyTag(int itemId, int tagId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteExchangeRetentionPolicyTag", itemId, tagId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteExchangeRetentionPolicyTagAsync(int itemId, int tagId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteExchangeRetentionPolicyTag", itemId, tagId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag[] /*List*/ GetExchangeMailboxPlanRetentionPolicyTags(int policyId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag[], SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeMailboxPlanRetentionPolicyTags", policyId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag[]> GetExchangeMailboxPlanRetentionPolicyTagsAsync(int policyId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag[], SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeMailboxPlanRetentionPolicyTags", policyId);
        }

        public SolidCP.Providers.ResultObjects.IntResult AddExchangeMailboxPlanRetentionPolicyTag(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag planTag)
        {
            return Invoke<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esExchangeServer", "AddExchangeMailboxPlanRetentionPolicyTag", itemId, planTag);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddExchangeMailboxPlanRetentionPolicyTagAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag planTag)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.IntResult>("SolidCP.EnterpriseServer.esExchangeServer", "AddExchangeMailboxPlanRetentionPolicyTag", itemId, planTag);
        }

        public SolidCP.Providers.Common.ResultObject DeleteExchangeMailboxPlanRetentionPolicyTag(int itemID, int policyId, int planTagId)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteExchangeMailboxPlanRetentionPolicyTag", itemID, policyId, planTagId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteExchangeMailboxPlanRetentionPolicyTagAsync(int itemID, int policyId, int planTagId)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteExchangeMailboxPlanRetentionPolicyTag", itemID, policyId, planTagId);
        }

        public int CreatePublicFolder(int itemId, string parentFolder, string folderName, bool mailEnabled, string accountName, string domain)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "CreatePublicFolder", itemId, parentFolder, folderName, mailEnabled, accountName, domain);
        }

        public async System.Threading.Tasks.Task<int> CreatePublicFolderAsync(int itemId, string parentFolder, string folderName, bool mailEnabled, string accountName, string domain)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "CreatePublicFolder", itemId, parentFolder, folderName, mailEnabled, accountName, domain);
        }

        public int DeletePublicFolders(int itemId, int[] accountIds)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeletePublicFolders", itemId, accountIds);
        }

        public async System.Threading.Tasks.Task<int> DeletePublicFoldersAsync(int itemId, int[] accountIds)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeletePublicFolders", itemId, accountIds);
        }

        public int DeletePublicFolder(int itemId, int accountId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeletePublicFolder", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeletePublicFolderAsync(int itemId, int accountId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeletePublicFolder", itemId, accountId);
        }

        public int EnableMailPublicFolder(int itemId, int accountId, string name, string domain)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "EnableMailPublicFolder", itemId, accountId, name, domain);
        }

        public async System.Threading.Tasks.Task<int> EnableMailPublicFolderAsync(int itemId, int accountId, string name, string domain)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "EnableMailPublicFolder", itemId, accountId, name, domain);
        }

        public int DisableMailPublicFolder(int itemId, int accountId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DisableMailPublicFolder", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DisableMailPublicFolderAsync(int itemId, int accountId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DisableMailPublicFolder", itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderGeneralSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangePublicFolder>("SolidCP.EnterpriseServer.esExchangeServer", "GetPublicFolderGeneralSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderGeneralSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangePublicFolder>("SolidCP.EnterpriseServer.esExchangeServer", "GetPublicFolderGeneralSettings", itemId, accountId);
        }

        public int SetPublicFolderGeneralSettings(int itemId, int accountId, string newName, bool hideAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetPublicFolderGeneralSettings", itemId, accountId, newName, hideAddressBook, accounts);
        }

        public async System.Threading.Tasks.Task<int> SetPublicFolderGeneralSettingsAsync(int itemId, int accountId, string newName, bool hideAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetPublicFolderGeneralSettings", itemId, accountId, newName, hideAddressBook, accounts);
        }

        public SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderMailFlowSettings(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangePublicFolder>("SolidCP.EnterpriseServer.esExchangeServer", "GetPublicFolderMailFlowSettings", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderMailFlowSettingsAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangePublicFolder>("SolidCP.EnterpriseServer.esExchangeServer", "GetPublicFolderMailFlowSettings", itemId, accountId);
        }

        public int SetPublicFolderMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetPublicFolderMailFlowSettings", itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task<int> SetPublicFolderMailFlowSettingsAsync(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetPublicFolderMailFlowSettings", itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public SolidCP.EnterpriseServer.ExchangeEmailAddress[] GetPublicFolderEmailAddresses(int itemId, int accountId)
        {
            return Invoke<SolidCP.EnterpriseServer.ExchangeEmailAddress[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetPublicFolderEmailAddresses", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ExchangeEmailAddress[]> GetPublicFolderEmailAddressesAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.EnterpriseServer.ExchangeEmailAddress[]>("SolidCP.EnterpriseServer.esExchangeServer", "GetPublicFolderEmailAddresses", itemId, accountId);
        }

        public int AddPublicFolderEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddPublicFolderEmailAddress", itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> AddPublicFolderEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddPublicFolderEmailAddress", itemId, accountId, emailAddress);
        }

        public int SetPublicFolderPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetPublicFolderPrimaryEmailAddress", itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> SetPublicFolderPrimaryEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetPublicFolderPrimaryEmailAddress", itemId, accountId, emailAddress);
        }

        public int DeletePublicFolderEmailAddresses(int itemId, int accountId, string[] emailAddresses)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeletePublicFolderEmailAddresses", itemId, accountId, emailAddresses);
        }

        public async System.Threading.Tasks.Task<int> DeletePublicFolderEmailAddressesAsync(int itemId, int accountId, string[] emailAddresses)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeletePublicFolderEmailAddresses", itemId, accountId, emailAddresses);
        }

        public string SetDefaultPublicFolderMailbox(int itemId)
        {
            return Invoke<string>("SolidCP.EnterpriseServer.esExchangeServer", "SetDefaultPublicFolderMailbox", itemId);
        }

        public async System.Threading.Tasks.Task<string> SetDefaultPublicFolderMailboxAsync(int itemId)
        {
            return await InvokeAsync<string>("SolidCP.EnterpriseServer.esExchangeServer", "SetDefaultPublicFolderMailbox", itemId);
        }

        public int AddExchangeDisclaimer(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddExchangeDisclaimer", itemId, disclaimer);
        }

        public async System.Threading.Tasks.Task<int> AddExchangeDisclaimerAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "AddExchangeDisclaimer", itemId, disclaimer);
        }

        public int UpdateExchangeDisclaimer(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "UpdateExchangeDisclaimer", itemId, disclaimer);
        }

        public async System.Threading.Tasks.Task<int> UpdateExchangeDisclaimerAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "UpdateExchangeDisclaimer", itemId, disclaimer);
        }

        public int DeleteExchangeDisclaimer(int itemId, int exchangeDisclaimerId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteExchangeDisclaimer", itemId, exchangeDisclaimerId);
        }

        public async System.Threading.Tasks.Task<int> DeleteExchangeDisclaimerAsync(int itemId, int exchangeDisclaimerId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "DeleteExchangeDisclaimer", itemId, exchangeDisclaimerId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDisclaimer GetExchangeDisclaimer(int itemId, int exchangeDisclaimerId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeDisclaimer>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeDisclaimer", itemId, exchangeDisclaimerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDisclaimer> GetExchangeDisclaimerAsync(int itemId, int exchangeDisclaimerId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeDisclaimer>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeDisclaimer", itemId, exchangeDisclaimerId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDisclaimer[] /*List*/ GetExchangeDisclaimers(int itemId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeDisclaimer[], SolidCP.Providers.HostedSolution.ExchangeDisclaimer>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeDisclaimers", itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDisclaimer[]> GetExchangeDisclaimersAsync(int itemId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeDisclaimer[], SolidCP.Providers.HostedSolution.ExchangeDisclaimer>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeDisclaimers", itemId);
        }

        public int SetExchangeAccountDisclaimerId(int itemId, int AccountID, int ExchangeDisclaimerId)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetExchangeAccountDisclaimerId", itemId, AccountID, ExchangeDisclaimerId);
        }

        public async System.Threading.Tasks.Task<int> SetExchangeAccountDisclaimerIdAsync(int itemId, int AccountID, int ExchangeDisclaimerId)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "SetExchangeAccountDisclaimerId", itemId, AccountID, ExchangeDisclaimerId);
        }

        public int GetExchangeAccountDisclaimerId(int itemId, int AccountID)
        {
            return Invoke<int>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeAccountDisclaimerId", itemId, AccountID);
        }

        public async System.Threading.Tasks.Task<int> GetExchangeAccountDisclaimerIdAsync(int itemId, int AccountID)
        {
            return await InvokeAsync<int>("SolidCP.EnterpriseServer.esExchangeServer", "GetExchangeAccountDisclaimerId", itemId, AccountID);
        }

        public SolidCP.Providers.Common.ResultObject SetPicture(int itemId, int accountId, byte[] picture)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esExchangeServer", "SetPicture", itemId, accountId, picture);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetPictureAsync(int itemId, int accountId, byte[] picture)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.EnterpriseServer.esExchangeServer", "SetPicture", itemId, accountId, picture);
        }

        public SolidCP.Providers.ResultObjects.BytesResult GetPicture(int itemId, int accountId)
        {
            return Invoke<SolidCP.Providers.ResultObjects.BytesResult>("SolidCP.EnterpriseServer.esExchangeServer", "GetPicture", itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BytesResult> GetPictureAsync(int itemId, int accountId)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.BytesResult>("SolidCP.EnterpriseServer.esExchangeServer", "GetPicture", itemId, accountId);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class esExchangeServer : SolidCP.Web.Client.ClientBase<IesExchangeServer, esExchangeServerAssemblyClient>, IesExchangeServer
    {
        public System.Data.DataSet GetRawExchangeOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetRawExchangeOrganizationsPaged(packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<System.Data.DataSet> GetRawExchangeOrganizationsPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetRawExchangeOrganizationsPagedAsync(packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.OrganizationsPaged GetExchangeOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return base.Client.GetExchangeOrganizationsPaged(packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.OrganizationsPaged> GetExchangeOrganizationsPagedAsync(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return await base.Client.GetExchangeOrganizationsPagedAsync(packageId, recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public SolidCP.Providers.HostedSolution.Organization[] /*List*/ GetExchangeOrganizations(int packageId, bool recursive)
        {
            return base.Client.GetExchangeOrganizations(packageId, recursive);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization[]> GetExchangeOrganizationsAsync(int packageId, bool recursive)
        {
            return await base.Client.GetExchangeOrganizationsAsync(packageId, recursive);
        }

        public SolidCP.Providers.HostedSolution.Organization GetOrganization(int itemId)
        {
            return base.Client.GetOrganization(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationAsync(int itemId)
        {
            return await base.Client.GetOrganizationAsync(itemId);
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

        public int GetExchangeServiceID(int itemId)
        {
            return base.Client.GetExchangeServiceID(itemId);
        }

        public async System.Threading.Tasks.Task<int> GetExchangeServiceIDAsync(int itemId)
        {
            return await base.Client.GetExchangeServiceIDAsync(itemId);
        }

        public int DeleteOrganization(int itemId)
        {
            return base.Client.DeleteOrganization(itemId);
        }

        public async System.Threading.Tasks.Task<int> DeleteOrganizationAsync(int itemId)
        {
            return await base.Client.DeleteOrganizationAsync(itemId);
        }

        public SolidCP.Providers.HostedSolution.Organization GetOrganizationStorageLimits(int itemId)
        {
            return base.Client.GetOrganizationStorageLimits(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> GetOrganizationStorageLimitsAsync(int itemId)
        {
            return await base.Client.GetOrganizationStorageLimitsAsync(itemId);
        }

        public int SetOrganizationStorageLimits(int itemId, int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays, bool applyToMailboxes)
        {
            return base.Client.SetOrganizationStorageLimits(itemId, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, applyToMailboxes);
        }

        public async System.Threading.Tasks.Task<int> SetOrganizationStorageLimitsAsync(int itemId, int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays, bool applyToMailboxes)
        {
            return await base.Client.SetOrganizationStorageLimitsAsync(itemId, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, applyToMailboxes);
        }

        public SolidCP.Providers.HostedSolution.ExchangeItemStatistics[] GetMailboxesStatistics(int itemId)
        {
            return base.Client.GetMailboxesStatistics(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]> GetMailboxesStatisticsAsync(int itemId)
        {
            return await base.Client.GetMailboxesStatisticsAsync(itemId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics GetMailboxStatistics(int itemId, int accountId)
        {
            return base.Client.GetMailboxStatistics(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics> GetMailboxStatisticsAsync(int itemId, int accountId)
        {
            return await base.Client.GetMailboxStatisticsAsync(itemId, accountId);
        }

        public int CalculateOrganizationDiskspace(int itemId)
        {
            return base.Client.CalculateOrganizationDiskspace(itemId);
        }

        public async System.Threading.Tasks.Task<int> CalculateOrganizationDiskspaceAsync(int itemId)
        {
            return await base.Client.CalculateOrganizationDiskspaceAsync(itemId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy GetActiveSyncPolicy(int itemId)
        {
            return base.Client.GetActiveSyncPolicy(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy> GetActiveSyncPolicyAsync(int itemId)
        {
            return await base.Client.GetActiveSyncPolicyAsync(itemId);
        }

        public int SetActiveSyncPolicy(int itemId, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInteval)
        {
            return base.Client.SetActiveSyncPolicy(itemId, allowNonProvisionableDevices, attachmentsEnabled, maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired, passwordRecoveryEnabled, deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts, minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInteval);
        }

        public async System.Threading.Tasks.Task<int> SetActiveSyncPolicyAsync(int itemId, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInteval)
        {
            return await base.Client.SetActiveSyncPolicyAsync(itemId, allowNonProvisionableDevices, attachmentsEnabled, maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired, passwordRecoveryEnabled, deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts, minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInteval);
        }

        public int AddAuthoritativeDomain(int itemId, int domainId)
        {
            return base.Client.AddAuthoritativeDomain(itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> AddAuthoritativeDomainAsync(int itemId, int domainId)
        {
            return await base.Client.AddAuthoritativeDomainAsync(itemId, domainId);
        }

        public int DeleteAuthoritativeDomain(int itemId, int domainId)
        {
            return base.Client.DeleteAuthoritativeDomain(itemId, domainId);
        }

        public async System.Threading.Tasks.Task<int> DeleteAuthoritativeDomainAsync(int itemId, int domainId)
        {
            return await base.Client.DeleteAuthoritativeDomainAsync(itemId, domainId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccountsPaged GetAccountsPaged(int itemId, string accountTypes, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool archiving)
        {
            return base.Client.GetAccountsPaged(itemId, accountTypes, filterColumn, filterValue, sortColumn, startRow, maximumRows, archiving);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccountsPaged> GetAccountsPagedAsync(int itemId, string accountTypes, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool archiving)
        {
            return await base.Client.GetAccountsPagedAsync(itemId, accountTypes, filterColumn, filterValue, sortColumn, startRow, maximumRows, archiving);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ GetAccounts(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType)
        {
            return base.Client.GetAccounts(itemId, accountType);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetAccountsAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType)
        {
            return await base.Client.GetAccountsAsync(itemId, accountType);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ GetExchangeAccountByMailboxPlanId(int itemId, int mailboxPlanId)
        {
            return base.Client.GetExchangeAccountByMailboxPlanId(itemId, mailboxPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetExchangeAccountByMailboxPlanIdAsync(int itemId, int mailboxPlanId)
        {
            return await base.Client.GetExchangeAccountByMailboxPlanIdAsync(itemId, mailboxPlanId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchAccounts(int itemId, bool includeMailboxes, bool includeContacts, bool includeDistributionLists, bool includeRooms, bool includeEquipment, bool IncludeSharedMailbox, bool includeSecurityGroups, string filterColumn, string filterValue, string sortColumn)
        {
            return base.Client.SearchAccounts(itemId, includeMailboxes, includeContacts, includeDistributionLists, includeRooms, includeEquipment, IncludeSharedMailbox, includeSecurityGroups, filterColumn, filterValue, sortColumn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchAccountsAsync(int itemId, bool includeMailboxes, bool includeContacts, bool includeDistributionLists, bool includeRooms, bool includeEquipment, bool IncludeSharedMailbox, bool includeSecurityGroups, string filterColumn, string filterValue, string sortColumn)
        {
            return await base.Client.SearchAccountsAsync(itemId, includeMailboxes, includeContacts, includeDistributionLists, includeRooms, includeEquipment, IncludeSharedMailbox, includeSecurityGroups, filterColumn, filterValue, sortColumn);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] /*List*/ SearchAccountsByTypes(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType[] types, string filterColumn, string filterValue, string sortColumn)
        {
            return base.Client.SearchAccountsByTypes(itemId, types, filterColumn, filterValue, sortColumn);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> SearchAccountsByTypesAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeAccountType[] types, string filterColumn, string filterValue, string sortColumn)
        {
            return await base.Client.SearchAccountsByTypesAsync(itemId, types, filterColumn, filterValue, sortColumn);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount GetAccount(int itemId, int accountId)
        {
            return base.Client.GetAccount(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount> GetAccountAsync(int itemId, int accountId)
        {
            return await base.Client.GetAccountAsync(itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount GetAccountByAccountNameWithoutItemId(string accountName)
        {
            return base.Client.GetAccountByAccountNameWithoutItemId(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount> GetAccountByAccountNameWithoutItemIdAsync(string accountName)
        {
            return await base.Client.GetAccountByAccountNameWithoutItemIdAsync(accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount SearchAccount(SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string primaryEmailAddress)
        {
            return base.Client.SearchAccount(accountType, primaryEmailAddress);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount> SearchAccountAsync(SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string primaryEmailAddress)
        {
            return await base.Client.SearchAccountAsync(accountType, primaryEmailAddress);
        }

        public bool CheckAccountCredentials(int itemId, string email, string password)
        {
            return base.Client.CheckAccountCredentials(itemId, email, password);
        }

        public async System.Threading.Tasks.Task<bool> CheckAccountCredentialsAsync(int itemId, string email, string password)
        {
            return await base.Client.CheckAccountCredentialsAsync(itemId, email, password);
        }

        public int CreateMailbox(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string accountName, string displayName, string name, string domain, string password, bool sendSetupInstructions, string setupInstructionMailAddress, int mailboxPlanId, int archivedPlanId, string subscriberNumber, bool EnableArchiving)
        {
            return base.Client.CreateMailbox(itemId, accountId, accountType, accountName, displayName, name, domain, password, sendSetupInstructions, setupInstructionMailAddress, mailboxPlanId, archivedPlanId, subscriberNumber, EnableArchiving);
        }

        public async System.Threading.Tasks.Task<int> CreateMailboxAsync(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string accountName, string displayName, string name, string domain, string password, bool sendSetupInstructions, string setupInstructionMailAddress, int mailboxPlanId, int archivedPlanId, string subscriberNumber, bool EnableArchiving)
        {
            return await base.Client.CreateMailboxAsync(itemId, accountId, accountType, accountName, displayName, name, domain, password, sendSetupInstructions, setupInstructionMailAddress, mailboxPlanId, archivedPlanId, subscriberNumber, EnableArchiving);
        }

        public string CreateJournalRule(int itemId, string journalEmail, string scope, string recipientEmail, bool enabled)
        {
            return base.Client.CreateJournalRule(itemId, journalEmail, scope, recipientEmail, enabled);
        }

        public async System.Threading.Tasks.Task<string> CreateJournalRuleAsync(int itemId, string journalEmail, string scope, string recipientEmail, bool enabled)
        {
            return await base.Client.CreateJournalRuleAsync(itemId, journalEmail, scope, recipientEmail, enabled);
        }

        public SolidCP.Providers.HostedSolution.ExchangeJournalRule GetJournalRule(int itemId, string journalEmail)
        {
            return base.Client.GetJournalRule(itemId, journalEmail);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeJournalRule> GetJournalRuleAsync(int itemId, string journalEmail)
        {
            return await base.Client.GetJournalRuleAsync(itemId, journalEmail);
        }

        public int SetJournalRule(int itemId, SolidCP.Providers.HostedSolution.ExchangeJournalRule rule)
        {
            return base.Client.SetJournalRule(itemId, rule);
        }

        public async System.Threading.Tasks.Task<int> SetJournalRuleAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeJournalRule rule)
        {
            return await base.Client.SetJournalRuleAsync(itemId, rule);
        }

        public int DeleteMailbox(int itemId, int accountId)
        {
            return base.Client.DeleteMailbox(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailboxAsync(int itemId, int accountId)
        {
            return await base.Client.DeleteMailboxAsync(itemId, accountId);
        }

        public int DisableMailbox(int itemId, int accountId)
        {
            return base.Client.DisableMailbox(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DisableMailboxAsync(int itemId, int accountId)
        {
            return await base.Client.DisableMailboxAsync(itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxAdvancedSettings(int itemId, int accountId)
        {
            return base.Client.GetMailboxAdvancedSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxAdvancedSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetMailboxAdvancedSettingsAsync(itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings GetMailboxAutoReplySettings(int itemId, int accountId)
        {
            return base.Client.GetMailboxAutoReplySettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings> GetMailboxAutoReplySettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetMailboxAutoReplySettingsAsync(itemId, accountId);
        }

        public int SetMailboxAutoReplySettings(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings)
        {
            return base.Client.SetMailboxAutoReplySettings(itemId, accountId, settings);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxAutoReplySettingsAsync(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings)
        {
            return await base.Client.SetMailboxAutoReplySettingsAsync(itemId, accountId, settings);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxGeneralSettings(int itemId, int accountId)
        {
            return base.Client.GetMailboxGeneralSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxGeneralSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetMailboxGeneralSettingsAsync(itemId, accountId);
        }

        public int SetMailboxGeneralSettings(int itemId, int accountId, bool hideAddressBook, bool disabled)
        {
            return base.Client.SetMailboxGeneralSettings(itemId, accountId, hideAddressBook, disabled);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxGeneralSettingsAsync(int itemId, int accountId, bool hideAddressBook, bool disabled)
        {
            return await base.Client.SetMailboxGeneralSettingsAsync(itemId, accountId, hideAddressBook, disabled);
        }

        public SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings GetResourceMailboxSettings(int itemId, int accountId)
        {
            return base.Client.GetResourceMailboxSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings> GetResourceMailboxSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetResourceMailboxSettingsAsync(itemId, accountId);
        }

        public int SetResourceMailboxSettings(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings resourceSettings)
        {
            return base.Client.SetResourceMailboxSettings(itemId, accountId, resourceSettings);
        }

        public async System.Threading.Tasks.Task<int> SetResourceMailboxSettingsAsync(int itemId, int accountId, SolidCP.Providers.HostedSolution.ExchangeResourceMailboxSettings resourceSettings)
        {
            return await base.Client.SetResourceMailboxSettingsAsync(itemId, accountId, resourceSettings);
        }

        public SolidCP.EnterpriseServer.ExchangeEmailAddress[] GetMailboxEmailAddresses(int itemId, int accountId)
        {
            return base.Client.GetMailboxEmailAddresses(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ExchangeEmailAddress[]> GetMailboxEmailAddressesAsync(int itemId, int accountId)
        {
            return await base.Client.GetMailboxEmailAddressesAsync(itemId, accountId);
        }

        public int AddMailboxEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return base.Client.AddMailboxEmailAddress(itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> AddMailboxEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await base.Client.AddMailboxEmailAddressAsync(itemId, accountId, emailAddress);
        }

        public int SetMailboxPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return base.Client.SetMailboxPrimaryEmailAddress(itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxPrimaryEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await base.Client.SetMailboxPrimaryEmailAddressAsync(itemId, accountId, emailAddress);
        }

        public int DeleteMailboxEmailAddresses(int itemId, int accountId, string[] emailAddresses)
        {
            return base.Client.DeleteMailboxEmailAddresses(itemId, accountId, emailAddresses);
        }

        public async System.Threading.Tasks.Task<int> DeleteMailboxEmailAddressesAsync(int itemId, int accountId, string[] emailAddresses)
        {
            return await base.Client.DeleteMailboxEmailAddressesAsync(itemId, accountId, emailAddresses);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxMailFlowSettings(int itemId, int accountId)
        {
            return base.Client.GetMailboxMailFlowSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxMailFlowSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetMailboxMailFlowSettingsAsync(itemId, accountId);
        }

        public int SetMailboxMailFlowSettings(int itemId, int accountId, bool enableForwarding, int SaveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return base.Client.SetMailboxMailFlowSettings(itemId, accountId, enableForwarding, SaveSentItems, forwardingAccountName, forwardToBoth, sendOnBehalfAccounts, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxMailFlowSettingsAsync(int itemId, int accountId, bool enableForwarding, int SaveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return await base.Client.SetMailboxMailFlowSettingsAsync(itemId, accountId, enableForwarding, SaveSentItems, forwardingAccountName, forwardToBoth, sendOnBehalfAccounts, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public int SetExchangeMailboxPlan(int itemId, int accountId, int mailboxPlanId, int archivePlanId, bool EnableArchiving)
        {
            return base.Client.SetExchangeMailboxPlan(itemId, accountId, mailboxPlanId, archivePlanId, EnableArchiving);
        }

        public async System.Threading.Tasks.Task<int> SetExchangeMailboxPlanAsync(int itemId, int accountId, int mailboxPlanId, int archivePlanId, bool EnableArchiving)
        {
            return await base.Client.SetExchangeMailboxPlanAsync(itemId, accountId, mailboxPlanId, archivePlanId, EnableArchiving);
        }

        public string GetMailboxSetupInstructions(int itemId, int accountId, bool pmm, bool emailMode, bool signup, string passwordResetUrl)
        {
            return base.Client.GetMailboxSetupInstructions(itemId, accountId, pmm, emailMode, signup, passwordResetUrl);
        }

        public async System.Threading.Tasks.Task<string> GetMailboxSetupInstructionsAsync(int itemId, int accountId, bool pmm, bool emailMode, bool signup, string passwordResetUrl)
        {
            return await base.Client.GetMailboxSetupInstructionsAsync(itemId, accountId, pmm, emailMode, signup, passwordResetUrl);
        }

        public int SendMailboxSetupInstructions(int itemId, int accountId, bool signup, string to, string cc)
        {
            return base.Client.SendMailboxSetupInstructions(itemId, accountId, signup, to, cc);
        }

        public async System.Threading.Tasks.Task<int> SendMailboxSetupInstructionsAsync(int itemId, int accountId, bool signup, string to, string cc)
        {
            return await base.Client.SendMailboxSetupInstructionsAsync(itemId, accountId, signup, to, cc);
        }

        public int SetMailboxManagerSettings(int itemId, int accountId, bool pmmAllowed, SolidCP.Providers.HostedSolution.MailboxManagerActions action)
        {
            return base.Client.SetMailboxManagerSettings(itemId, accountId, pmmAllowed, action);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxManagerSettingsAsync(int itemId, int accountId, bool pmmAllowed, SolidCP.Providers.HostedSolution.MailboxManagerActions action)
        {
            return await base.Client.SetMailboxManagerSettingsAsync(itemId, accountId, pmmAllowed, action);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxPermissions(int itemId, int accountId)
        {
            return base.Client.GetMailboxPermissions(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxPermissionsAsync(int itemId, int accountId)
        {
            return await base.Client.GetMailboxPermissionsAsync(itemId, accountId);
        }

        public int SetMailboxPermissions(int itemId, int accountId, string[] sendAsaccounts, string[] fullAccessAcounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts)
        {
            return base.Client.SetMailboxPermissions(itemId, accountId, sendAsaccounts, fullAccessAcounts, onBehalfOfAccounts, calendarAccounts, contactAccounts);
        }

        public async System.Threading.Tasks.Task<int> SetMailboxPermissionsAsync(int itemId, int accountId, string[] sendAsaccounts, string[] fullAccessAcounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts)
        {
            return await base.Client.SetMailboxPermissionsAsync(itemId, accountId, sendAsaccounts, fullAccessAcounts, onBehalfOfAccounts, calendarAccounts, contactAccounts);
        }

        public int ExportMailBox(int itemId, int accountId, string path)
        {
            return base.Client.ExportMailBox(itemId, accountId, path);
        }

        public async System.Threading.Tasks.Task<int> ExportMailBoxAsync(int itemId, int accountId, string path)
        {
            return await base.Client.ExportMailBoxAsync(itemId, accountId, path);
        }

        public int SetDeletedMailbox(int itemId, int accountId)
        {
            return base.Client.SetDeletedMailbox(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> SetDeletedMailboxAsync(int itemId, int accountId)
        {
            return await base.Client.SetDeletedMailboxAsync(itemId, accountId);
        }

        public int CreateContact(int itemId, string displayName, string email)
        {
            return base.Client.CreateContact(itemId, displayName, email);
        }

        public async System.Threading.Tasks.Task<int> CreateContactAsync(int itemId, string displayName, string email)
        {
            return await base.Client.CreateContactAsync(itemId, displayName, email);
        }

        public int DeleteContact(int itemId, int accountId)
        {
            return base.Client.DeleteContact(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeleteContactAsync(int itemId, int accountId)
        {
            return await base.Client.DeleteContactAsync(itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeContact GetContactGeneralSettings(int itemId, int accountId)
        {
            return base.Client.GetContactGeneralSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactGeneralSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetContactGeneralSettingsAsync(itemId, accountId);
        }

        public int SetContactGeneralSettings(int itemId, int accountId, string displayName, string emailAddress, bool hideAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat)
        {
            return base.Client.SetContactGeneralSettings(itemId, accountId, displayName, emailAddress, hideAddressBook, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, useMapiRichTextFormat);
        }

        public async System.Threading.Tasks.Task<int> SetContactGeneralSettingsAsync(int itemId, int accountId, string displayName, string emailAddress, bool hideAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat)
        {
            return await base.Client.SetContactGeneralSettingsAsync(itemId, accountId, displayName, emailAddress, hideAddressBook, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, useMapiRichTextFormat);
        }

        public SolidCP.Providers.HostedSolution.ExchangeContact GetContactMailFlowSettings(int itemId, int accountId)
        {
            return base.Client.GetContactMailFlowSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactMailFlowSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetContactMailFlowSettingsAsync(itemId, accountId);
        }

        public int SetContactMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return base.Client.SetContactMailFlowSettings(itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task<int> SetContactMailFlowSettingsAsync(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return await base.Client.SetContactMailFlowSettingsAsync(itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public int CreateDistributionList(int itemId, string displayName, string name, string domain, int managerId)
        {
            return base.Client.CreateDistributionList(itemId, displayName, name, domain, managerId);
        }

        public async System.Threading.Tasks.Task<int> CreateDistributionListAsync(int itemId, string displayName, string name, string domain, int managerId)
        {
            return await base.Client.CreateDistributionListAsync(itemId, displayName, name, domain, managerId);
        }

        public int DeleteDistributionList(int itemId, int accountId)
        {
            return base.Client.DeleteDistributionList(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeleteDistributionListAsync(int itemId, int accountId)
        {
            return await base.Client.DeleteDistributionListAsync(itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListGeneralSettings(int itemId, int accountId)
        {
            return base.Client.GetDistributionListGeneralSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListGeneralSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetDistributionListGeneralSettingsAsync(itemId, accountId);
        }

        public int SetDistributionListGeneralSettings(int itemId, int accountId, string displayName, bool hideAddressBook, string managerAccount, string[] memberAccounts, string notes)
        {
            return base.Client.SetDistributionListGeneralSettings(itemId, accountId, displayName, hideAddressBook, managerAccount, memberAccounts, notes);
        }

        public async System.Threading.Tasks.Task<int> SetDistributionListGeneralSettingsAsync(int itemId, int accountId, string displayName, bool hideAddressBook, string managerAccount, string[] memberAccounts, string notes)
        {
            return await base.Client.SetDistributionListGeneralSettingsAsync(itemId, accountId, displayName, hideAddressBook, managerAccount, memberAccounts, notes);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListMailFlowSettings(int itemId, int accountId)
        {
            return base.Client.GetDistributionListMailFlowSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListMailFlowSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetDistributionListMailFlowSettingsAsync(itemId, accountId);
        }

        public int SetDistributionListMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return base.Client.SetDistributionListMailFlowSettings(itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task<int> SetDistributionListMailFlowSettingsAsync(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return await base.Client.SetDistributionListMailFlowSettingsAsync(itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public SolidCP.EnterpriseServer.ExchangeEmailAddress[] GetDistributionListEmailAddresses(int itemId, int accountId)
        {
            return base.Client.GetDistributionListEmailAddresses(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ExchangeEmailAddress[]> GetDistributionListEmailAddressesAsync(int itemId, int accountId)
        {
            return await base.Client.GetDistributionListEmailAddressesAsync(itemId, accountId);
        }

        public int AddDistributionListEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return base.Client.AddDistributionListEmailAddress(itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> AddDistributionListEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await base.Client.AddDistributionListEmailAddressAsync(itemId, accountId, emailAddress);
        }

        public int SetDistributionListPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return base.Client.SetDistributionListPrimaryEmailAddress(itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> SetDistributionListPrimaryEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await base.Client.SetDistributionListPrimaryEmailAddressAsync(itemId, accountId, emailAddress);
        }

        public int DeleteDistributionListEmailAddresses(int itemId, int accountId, string[] emailAddresses)
        {
            return base.Client.DeleteDistributionListEmailAddresses(itemId, accountId, emailAddresses);
        }

        public async System.Threading.Tasks.Task<int> DeleteDistributionListEmailAddressesAsync(int itemId, int accountId, string[] emailAddresses)
        {
            return await base.Client.DeleteDistributionListEmailAddressesAsync(itemId, accountId, emailAddresses);
        }

        public SolidCP.Providers.Common.ResultObject SetDistributionListPermissions(int itemId, int accountId, string[] sendAsAccounts, string[] sendOnBehalfAccounts)
        {
            return base.Client.SetDistributionListPermissions(itemId, accountId, sendAsAccounts, sendOnBehalfAccounts);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetDistributionListPermissionsAsync(int itemId, int accountId, string[] sendAsAccounts, string[] sendOnBehalfAccounts)
        {
            return await base.Client.SetDistributionListPermissionsAsync(itemId, accountId, sendAsAccounts, sendOnBehalfAccounts);
        }

        public SolidCP.Providers.ResultObjects.ExchangeDistributionListResult GetDistributionListPermissions(int itemId, int accountId)
        {
            return base.Client.GetDistributionListPermissions(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.ExchangeDistributionListResult> GetDistributionListPermissionsAsync(int itemId, int accountId)
        {
            return await base.Client.GetDistributionListPermissionsAsync(itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeAccount[] GetDistributionListsByMember(int itemId, int accountId)
        {
            return base.Client.GetDistributionListsByMember(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeAccount[]> GetDistributionListsByMemberAsync(int itemId, int accountId)
        {
            return await base.Client.GetDistributionListsByMemberAsync(itemId, accountId);
        }

        public int AddDistributionListMember(int itemId, string distributionListName, int memberId)
        {
            return base.Client.AddDistributionListMember(itemId, distributionListName, memberId);
        }

        public async System.Threading.Tasks.Task<int> AddDistributionListMemberAsync(int itemId, string distributionListName, int memberId)
        {
            return await base.Client.AddDistributionListMemberAsync(itemId, distributionListName, memberId);
        }

        public int DeleteDistributionListMember(int itemId, string distributionListName, int memberId)
        {
            return base.Client.DeleteDistributionListMember(itemId, distributionListName, memberId);
        }

        public async System.Threading.Tasks.Task<int> DeleteDistributionListMemberAsync(int itemId, string distributionListName, int memberId)
        {
            return await base.Client.DeleteDistributionListMemberAsync(itemId, distributionListName, memberId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMobileDevice[] GetMobileDevices(int itemId, int accountId)
        {
            return base.Client.GetMobileDevices(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice[]> GetMobileDevicesAsync(int itemId, int accountId)
        {
            return await base.Client.GetMobileDevicesAsync(itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMobileDevice GetMobileDevice(int itemId, string deviceId)
        {
            return base.Client.GetMobileDevice(itemId, deviceId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice> GetMobileDeviceAsync(int itemId, string deviceId)
        {
            return await base.Client.GetMobileDeviceAsync(itemId, deviceId);
        }

        public void WipeDataFromDevice(int itemId, string deviceId)
        {
            base.Client.WipeDataFromDevice(itemId, deviceId);
        }

        public async System.Threading.Tasks.Task WipeDataFromDeviceAsync(int itemId, string deviceId)
        {
            await base.Client.WipeDataFromDeviceAsync(itemId, deviceId);
        }

        public void CancelRemoteWipeRequest(int itemId, string deviceId)
        {
            base.Client.CancelRemoteWipeRequest(itemId, deviceId);
        }

        public async System.Threading.Tasks.Task CancelRemoteWipeRequestAsync(int itemId, string deviceId)
        {
            await base.Client.CancelRemoteWipeRequestAsync(itemId, deviceId);
        }

        public void RemoveDevice(int itemId, string deviceId)
        {
            base.Client.RemoveDevice(itemId, deviceId);
        }

        public async System.Threading.Tasks.Task RemoveDeviceAsync(int itemId, string deviceId)
        {
            await base.Client.RemoveDeviceAsync(itemId, deviceId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxPlan[] /*List*/ GetExchangeMailboxPlans(int itemId, bool archiving)
        {
            return base.Client.GetExchangeMailboxPlans(itemId, archiving);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxPlan[]> GetExchangeMailboxPlansAsync(int itemId, bool archiving)
        {
            return await base.Client.GetExchangeMailboxPlansAsync(itemId, archiving);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxPlan GetExchangeMailboxPlan(int itemId, int mailboxPlanId)
        {
            return base.Client.GetExchangeMailboxPlan(itemId, mailboxPlanId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxPlan> GetExchangeMailboxPlanAsync(int itemId, int mailboxPlanId)
        {
            return await base.Client.GetExchangeMailboxPlanAsync(itemId, mailboxPlanId);
        }

        public int AddExchangeMailboxPlan(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan)
        {
            return base.Client.AddExchangeMailboxPlan(itemId, mailboxPlan);
        }

        public async System.Threading.Tasks.Task<int> AddExchangeMailboxPlanAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan)
        {
            return await base.Client.AddExchangeMailboxPlanAsync(itemId, mailboxPlan);
        }

        public int UpdateExchangeMailboxPlan(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan)
        {
            return base.Client.UpdateExchangeMailboxPlan(itemId, mailboxPlan);
        }

        public async System.Threading.Tasks.Task<int> UpdateExchangeMailboxPlanAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlan mailboxPlan)
        {
            return await base.Client.UpdateExchangeMailboxPlanAsync(itemId, mailboxPlan);
        }

        public int DeleteExchangeMailboxPlan(int itemId, int mailboxPlanId)
        {
            return base.Client.DeleteExchangeMailboxPlan(itemId, mailboxPlanId);
        }

        public async System.Threading.Tasks.Task<int> DeleteExchangeMailboxPlanAsync(int itemId, int mailboxPlanId)
        {
            return await base.Client.DeleteExchangeMailboxPlanAsync(itemId, mailboxPlanId);
        }

        public void SetOrganizationDefaultExchangeMailboxPlan(int itemId, int mailboxPlanId)
        {
            base.Client.SetOrganizationDefaultExchangeMailboxPlan(itemId, mailboxPlanId);
        }

        public async System.Threading.Tasks.Task SetOrganizationDefaultExchangeMailboxPlanAsync(int itemId, int mailboxPlanId)
        {
            await base.Client.SetOrganizationDefaultExchangeMailboxPlanAsync(itemId, mailboxPlanId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag[] /*List*/ GetExchangeRetentionPolicyTags(int itemId)
        {
            return base.Client.GetExchangeRetentionPolicyTags(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag[]> GetExchangeRetentionPolicyTagsAsync(int itemId)
        {
            return await base.Client.GetExchangeRetentionPolicyTagsAsync(itemId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag GetExchangeRetentionPolicyTag(int itemId, int tagId)
        {
            return base.Client.GetExchangeRetentionPolicyTag(itemId, tagId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag> GetExchangeRetentionPolicyTagAsync(int itemId, int tagId)
        {
            return await base.Client.GetExchangeRetentionPolicyTagAsync(itemId, tagId);
        }

        public SolidCP.Providers.ResultObjects.IntResult AddExchangeRetentionPolicyTag(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag)
        {
            return base.Client.AddExchangeRetentionPolicyTag(itemId, tag);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddExchangeRetentionPolicyTagAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag)
        {
            return await base.Client.AddExchangeRetentionPolicyTagAsync(itemId, tag);
        }

        public SolidCP.Providers.Common.ResultObject UpdateExchangeRetentionPolicyTag(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag)
        {
            return base.Client.UpdateExchangeRetentionPolicyTag(itemId, tag);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> UpdateExchangeRetentionPolicyTagAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTag tag)
        {
            return await base.Client.UpdateExchangeRetentionPolicyTagAsync(itemId, tag);
        }

        public SolidCP.Providers.Common.ResultObject DeleteExchangeRetentionPolicyTag(int itemId, int tagId)
        {
            return base.Client.DeleteExchangeRetentionPolicyTag(itemId, tagId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteExchangeRetentionPolicyTagAsync(int itemId, int tagId)
        {
            return await base.Client.DeleteExchangeRetentionPolicyTagAsync(itemId, tagId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag[] /*List*/ GetExchangeMailboxPlanRetentionPolicyTags(int policyId)
        {
            return base.Client.GetExchangeMailboxPlanRetentionPolicyTags(policyId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag[]> GetExchangeMailboxPlanRetentionPolicyTagsAsync(int policyId)
        {
            return await base.Client.GetExchangeMailboxPlanRetentionPolicyTagsAsync(policyId);
        }

        public SolidCP.Providers.ResultObjects.IntResult AddExchangeMailboxPlanRetentionPolicyTag(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag planTag)
        {
            return base.Client.AddExchangeMailboxPlanRetentionPolicyTag(itemId, planTag);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.IntResult> AddExchangeMailboxPlanRetentionPolicyTagAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeMailboxPlanRetentionPolicyTag planTag)
        {
            return await base.Client.AddExchangeMailboxPlanRetentionPolicyTagAsync(itemId, planTag);
        }

        public SolidCP.Providers.Common.ResultObject DeleteExchangeMailboxPlanRetentionPolicyTag(int itemID, int policyId, int planTagId)
        {
            return base.Client.DeleteExchangeMailboxPlanRetentionPolicyTag(itemID, policyId, planTagId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> DeleteExchangeMailboxPlanRetentionPolicyTagAsync(int itemID, int policyId, int planTagId)
        {
            return await base.Client.DeleteExchangeMailboxPlanRetentionPolicyTagAsync(itemID, policyId, planTagId);
        }

        public int CreatePublicFolder(int itemId, string parentFolder, string folderName, bool mailEnabled, string accountName, string domain)
        {
            return base.Client.CreatePublicFolder(itemId, parentFolder, folderName, mailEnabled, accountName, domain);
        }

        public async System.Threading.Tasks.Task<int> CreatePublicFolderAsync(int itemId, string parentFolder, string folderName, bool mailEnabled, string accountName, string domain)
        {
            return await base.Client.CreatePublicFolderAsync(itemId, parentFolder, folderName, mailEnabled, accountName, domain);
        }

        public int DeletePublicFolders(int itemId, int[] accountIds)
        {
            return base.Client.DeletePublicFolders(itemId, accountIds);
        }

        public async System.Threading.Tasks.Task<int> DeletePublicFoldersAsync(int itemId, int[] accountIds)
        {
            return await base.Client.DeletePublicFoldersAsync(itemId, accountIds);
        }

        public int DeletePublicFolder(int itemId, int accountId)
        {
            return base.Client.DeletePublicFolder(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DeletePublicFolderAsync(int itemId, int accountId)
        {
            return await base.Client.DeletePublicFolderAsync(itemId, accountId);
        }

        public int EnableMailPublicFolder(int itemId, int accountId, string name, string domain)
        {
            return base.Client.EnableMailPublicFolder(itemId, accountId, name, domain);
        }

        public async System.Threading.Tasks.Task<int> EnableMailPublicFolderAsync(int itemId, int accountId, string name, string domain)
        {
            return await base.Client.EnableMailPublicFolderAsync(itemId, accountId, name, domain);
        }

        public int DisableMailPublicFolder(int itemId, int accountId)
        {
            return base.Client.DisableMailPublicFolder(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<int> DisableMailPublicFolderAsync(int itemId, int accountId)
        {
            return await base.Client.DisableMailPublicFolderAsync(itemId, accountId);
        }

        public SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderGeneralSettings(int itemId, int accountId)
        {
            return base.Client.GetPublicFolderGeneralSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderGeneralSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetPublicFolderGeneralSettingsAsync(itemId, accountId);
        }

        public int SetPublicFolderGeneralSettings(int itemId, int accountId, string newName, bool hideAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts)
        {
            return base.Client.SetPublicFolderGeneralSettings(itemId, accountId, newName, hideAddressBook, accounts);
        }

        public async System.Threading.Tasks.Task<int> SetPublicFolderGeneralSettingsAsync(int itemId, int accountId, string newName, bool hideAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts)
        {
            return await base.Client.SetPublicFolderGeneralSettingsAsync(itemId, accountId, newName, hideAddressBook, accounts);
        }

        public SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderMailFlowSettings(int itemId, int accountId)
        {
            return base.Client.GetPublicFolderMailFlowSettings(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderMailFlowSettingsAsync(int itemId, int accountId)
        {
            return await base.Client.GetPublicFolderMailFlowSettingsAsync(itemId, accountId);
        }

        public int SetPublicFolderMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return base.Client.SetPublicFolderMailFlowSettings(itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task<int> SetPublicFolderMailFlowSettingsAsync(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            return await base.Client.SetPublicFolderMailFlowSettingsAsync(itemId, accountId, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public SolidCP.EnterpriseServer.ExchangeEmailAddress[] GetPublicFolderEmailAddresses(int itemId, int accountId)
        {
            return base.Client.GetPublicFolderEmailAddresses(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.EnterpriseServer.ExchangeEmailAddress[]> GetPublicFolderEmailAddressesAsync(int itemId, int accountId)
        {
            return await base.Client.GetPublicFolderEmailAddressesAsync(itemId, accountId);
        }

        public int AddPublicFolderEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return base.Client.AddPublicFolderEmailAddress(itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> AddPublicFolderEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await base.Client.AddPublicFolderEmailAddressAsync(itemId, accountId, emailAddress);
        }

        public int SetPublicFolderPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
        {
            return base.Client.SetPublicFolderPrimaryEmailAddress(itemId, accountId, emailAddress);
        }

        public async System.Threading.Tasks.Task<int> SetPublicFolderPrimaryEmailAddressAsync(int itemId, int accountId, string emailAddress)
        {
            return await base.Client.SetPublicFolderPrimaryEmailAddressAsync(itemId, accountId, emailAddress);
        }

        public int DeletePublicFolderEmailAddresses(int itemId, int accountId, string[] emailAddresses)
        {
            return base.Client.DeletePublicFolderEmailAddresses(itemId, accountId, emailAddresses);
        }

        public async System.Threading.Tasks.Task<int> DeletePublicFolderEmailAddressesAsync(int itemId, int accountId, string[] emailAddresses)
        {
            return await base.Client.DeletePublicFolderEmailAddressesAsync(itemId, accountId, emailAddresses);
        }

        public string SetDefaultPublicFolderMailbox(int itemId)
        {
            return base.Client.SetDefaultPublicFolderMailbox(itemId);
        }

        public async System.Threading.Tasks.Task<string> SetDefaultPublicFolderMailboxAsync(int itemId)
        {
            return await base.Client.SetDefaultPublicFolderMailboxAsync(itemId);
        }

        public int AddExchangeDisclaimer(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer)
        {
            return base.Client.AddExchangeDisclaimer(itemId, disclaimer);
        }

        public async System.Threading.Tasks.Task<int> AddExchangeDisclaimerAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer)
        {
            return await base.Client.AddExchangeDisclaimerAsync(itemId, disclaimer);
        }

        public int UpdateExchangeDisclaimer(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer)
        {
            return base.Client.UpdateExchangeDisclaimer(itemId, disclaimer);
        }

        public async System.Threading.Tasks.Task<int> UpdateExchangeDisclaimerAsync(int itemId, SolidCP.Providers.HostedSolution.ExchangeDisclaimer disclaimer)
        {
            return await base.Client.UpdateExchangeDisclaimerAsync(itemId, disclaimer);
        }

        public int DeleteExchangeDisclaimer(int itemId, int exchangeDisclaimerId)
        {
            return base.Client.DeleteExchangeDisclaimer(itemId, exchangeDisclaimerId);
        }

        public async System.Threading.Tasks.Task<int> DeleteExchangeDisclaimerAsync(int itemId, int exchangeDisclaimerId)
        {
            return await base.Client.DeleteExchangeDisclaimerAsync(itemId, exchangeDisclaimerId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDisclaimer GetExchangeDisclaimer(int itemId, int exchangeDisclaimerId)
        {
            return base.Client.GetExchangeDisclaimer(itemId, exchangeDisclaimerId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDisclaimer> GetExchangeDisclaimerAsync(int itemId, int exchangeDisclaimerId)
        {
            return await base.Client.GetExchangeDisclaimerAsync(itemId, exchangeDisclaimerId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDisclaimer[] /*List*/ GetExchangeDisclaimers(int itemId)
        {
            return base.Client.GetExchangeDisclaimers(itemId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDisclaimer[]> GetExchangeDisclaimersAsync(int itemId)
        {
            return await base.Client.GetExchangeDisclaimersAsync(itemId);
        }

        public int SetExchangeAccountDisclaimerId(int itemId, int AccountID, int ExchangeDisclaimerId)
        {
            return base.Client.SetExchangeAccountDisclaimerId(itemId, AccountID, ExchangeDisclaimerId);
        }

        public async System.Threading.Tasks.Task<int> SetExchangeAccountDisclaimerIdAsync(int itemId, int AccountID, int ExchangeDisclaimerId)
        {
            return await base.Client.SetExchangeAccountDisclaimerIdAsync(itemId, AccountID, ExchangeDisclaimerId);
        }

        public int GetExchangeAccountDisclaimerId(int itemId, int AccountID)
        {
            return base.Client.GetExchangeAccountDisclaimerId(itemId, AccountID);
        }

        public async System.Threading.Tasks.Task<int> GetExchangeAccountDisclaimerIdAsync(int itemId, int AccountID)
        {
            return await base.Client.GetExchangeAccountDisclaimerIdAsync(itemId, AccountID);
        }

        public SolidCP.Providers.Common.ResultObject SetPicture(int itemId, int accountId, byte[] picture)
        {
            return base.Client.SetPicture(itemId, accountId, picture);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetPictureAsync(int itemId, int accountId, byte[] picture)
        {
            return await base.Client.SetPictureAsync(itemId, accountId, picture);
        }

        public SolidCP.Providers.ResultObjects.BytesResult GetPicture(int itemId, int accountId)
        {
            return base.Client.GetPicture(itemId, accountId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BytesResult> GetPictureAsync(int itemId, int accountId)
        {
            return await base.Client.GetPictureAsync(itemId, accountId);
        }
    }
}
#endif