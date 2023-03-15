#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IExchangeServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IExchangeServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CheckAccountCredentials", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CheckAccountCredentialsResponse")]
        bool CheckAccountCredentials(string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CheckAccountCredentials", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CheckAccountCredentialsResponse")]
        System.Threading.Tasks.Task<bool> CheckAccountCredentialsAsync(string username, string password);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/ExtendToExchangeOrganization", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/ExtendToExchangeOrganizationResponse")]
        SolidCP.Providers.HostedSolution.Organization ExtendToExchangeOrganization(string organizationId, string securityGroup, bool IsConsumer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/ExtendToExchangeOrganization", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/ExtendToExchangeOrganizationResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> ExtendToExchangeOrganizationAsync(string organizationId, string securityGroup, bool IsConsumer);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateMailEnableUser", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateMailEnableUserResponse")]
        string CreateMailEnableUser(string upn, string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string mailboxDatabase, string offlineAddressBook, string addressBookPolicy, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool hideFromAddressBook, bool isConsumer, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateMailEnableUser", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateMailEnableUserResponse")]
        System.Threading.Tasks.Task<string> CreateMailEnableUserAsync(string upn, string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string mailboxDatabase, string offlineAddressBook, string addressBookPolicy, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool hideFromAddressBook, bool isConsumer, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationOfflineAddressBook", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationOfflineAddressBookResponse")]
        SolidCP.Providers.HostedSolution.Organization CreateOrganizationOfflineAddressBook(string organizationId, string securityGroup, string oabVirtualDir);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationOfflineAddressBook", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationOfflineAddressBookResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> CreateOrganizationOfflineAddressBookAsync(string organizationId, string securityGroup, string oabVirtualDir);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/UpdateOrganizationOfflineAddressBook", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/UpdateOrganizationOfflineAddressBookResponse")]
        void UpdateOrganizationOfflineAddressBook(string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/UpdateOrganizationOfflineAddressBook", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/UpdateOrganizationOfflineAddressBookResponse")]
        System.Threading.Tasks.Task UpdateOrganizationOfflineAddressBookAsync(string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetOABVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetOABVirtualDirectoryResponse")]
        string GetOABVirtualDirectory();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetOABVirtualDirectory", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetOABVirtualDirectoryResponse")]
        System.Threading.Tasks.Task<string> GetOABVirtualDirectoryAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationAddressBookPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationAddressBookPolicyResponse")]
        SolidCP.Providers.HostedSolution.Organization CreateOrganizationAddressBookPolicy(string organizationId, string gal, string addressBook, string roomList, string oab);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationAddressBookPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationAddressBookPolicyResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> CreateOrganizationAddressBookPolicyAsync(string organizationId, string gal, string addressBook, string roomList, string oab);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeleteOrganizationResponse")]
        bool DeleteOrganization(string organizationId, string distinguishedName, string globalAddressList, string addressList, string roomList, string offlineAddressBook, string securityGroup, string addressBookPolicy, SolidCP.Providers.HostedSolution.ExchangeDomainName[] /*List*/ acceptedDomains);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeleteOrganization", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeleteOrganizationResponse")]
        System.Threading.Tasks.Task<bool> DeleteOrganizationAsync(string organizationId, string distinguishedName, string globalAddressList, string addressList, string roomList, string offlineAddressBook, string securityGroup, string addressBookPolicy, SolidCP.Providers.HostedSolution.ExchangeDomainName[] /*List*/ acceptedDomains);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetOrganizationStorageLimits", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetOrganizationStorageLimitsResponse")]
        void SetOrganizationStorageLimits(string organizationDistinguishedName, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetOrganizationStorageLimits", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetOrganizationStorageLimitsResponse")]
        System.Threading.Tasks.Task SetOrganizationStorageLimitsAsync(string organizationDistinguishedName, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxesStatistics", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxesStatisticsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeItemStatistics[] GetMailboxesStatistics(string organizationDistinguishedName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxesStatistics", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxesStatisticsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]> GetMailboxesStatisticsAsync(string organizationDistinguishedName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/AddAuthoritativeDomain", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/AddAuthoritativeDomainResponse")]
        void AddAuthoritativeDomain(string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/AddAuthoritativeDomain", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/AddAuthoritativeDomainResponse")]
        System.Threading.Tasks.Task AddAuthoritativeDomainAsync(string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/ChangeAcceptedDomainType", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/ChangeAcceptedDomainTypeResponse")]
        void ChangeAcceptedDomainType(string domain, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType domainType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/ChangeAcceptedDomainType", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/ChangeAcceptedDomainTypeResponse")]
        System.Threading.Tasks.Task ChangeAcceptedDomainTypeAsync(string domain, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType domainType);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetAuthoritativeDomains", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetAuthoritativeDomainsResponse")]
        string[] GetAuthoritativeDomains();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetAuthoritativeDomains", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetAuthoritativeDomainsResponse")]
        System.Threading.Tasks.Task<string[]> GetAuthoritativeDomainsAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeleteAuthoritativeDomain", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeleteAuthoritativeDomainResponse")]
        void DeleteAuthoritativeDomain(string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeleteAuthoritativeDomain", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeleteAuthoritativeDomainResponse")]
        System.Threading.Tasks.Task DeleteAuthoritativeDomainAsync(string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeleteMailbox", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeleteMailboxResponse")]
        void DeleteMailbox(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeleteMailbox", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeleteMailboxResponse")]
        System.Threading.Tasks.Task DeleteMailboxAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DisableMailbox", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DisableMailboxResponse")]
        void DisableMailbox(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DisableMailbox", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DisableMailboxResponse")]
        System.Threading.Tasks.Task DisableMailboxAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxAutoReplySettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxAutoReplySettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings GetMailboxAutoReplySettings(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxAutoReplySettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxAutoReplySettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings> GetMailboxAutoReplySettingsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxAutoReplySettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxAutoReplySettingsResponse")]
        void SetMailboxAutoReplySettings(string accountName, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxAutoReplySettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxAutoReplySettingsResponse")]
        System.Threading.Tasks.Task SetMailboxAutoReplySettingsAsync(string accountName, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxGeneralSettings(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxGeneralSettingsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxGeneralSettingsResponse")]
        void SetMailboxGeneralSettings(string accountName, bool hideFromAddressBook, bool disabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxGeneralSettingsResponse")]
        System.Threading.Tasks.Task SetMailboxGeneralSettingsAsync(string accountName, bool hideFromAddressBook, bool disabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxMailFlowSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxMailFlowSettings(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxMailFlowSettingsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxMailFlowSettingsResponse")]
        void SetMailboxMailFlowSettings(string accountName, bool enableForwarding, int saveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxMailFlowSettingsResponse")]
        System.Threading.Tasks.Task SetMailboxMailFlowSettingsAsync(string accountName, bool enableForwarding, int saveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxAdvancedSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxAdvancedSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxAdvancedSettings(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxAdvancedSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxAdvancedSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxAdvancedSettingsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxAdvancedSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxAdvancedSettingsResponse")]
        void SetMailboxAdvancedSettings(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxAdvancedSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxAdvancedSettingsResponse")]
        System.Threading.Tasks.Task SetMailboxAdvancedSettingsAsync(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxEmailAddressesResponse")]
        SolidCP.Providers.HostedSolution.ExchangeEmailAddress[] GetMailboxEmailAddresses(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxEmailAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]> GetMailboxEmailAddressesAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxEmailAddressesResponse")]
        void SetMailboxEmailAddresses(string accountName, string[] emailAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxEmailAddressesResponse")]
        System.Threading.Tasks.Task SetMailboxEmailAddressesAsync(string accountName, string[] emailAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxPrimaryEmailAddressResponse")]
        void SetMailboxPrimaryEmailAddress(string accountName, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxPrimaryEmailAddressResponse")]
        System.Threading.Tasks.Task SetMailboxPrimaryEmailAddressAsync(string accountName, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxPermissions", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxPermissionsResponse")]
        void SetMailboxPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxPermissions", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailboxPermissionsResponse")]
        System.Threading.Tasks.Task SetMailboxPermissionsAsync(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxPermissions", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxPermissionsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxPermissions(string organizationId, string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxPermissions", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxPermissionsAsync(string organizationId, string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxStatistics", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxStatisticsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics GetMailboxStatistics(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxStatistics", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMailboxStatisticsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics> GetMailboxStatisticsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDefaultPublicFolderMailbox", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDefaultPublicFolderMailboxResponse")]
        string[] SetDefaultPublicFolderMailbox(string id, string organizationId, string organizationDistinguishedName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDefaultPublicFolderMailbox", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDefaultPublicFolderMailboxResponse")]
        System.Threading.Tasks.Task<string[]> SetDefaultPublicFolderMailboxAsync(string id, string organizationId, string organizationDistinguishedName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateJournalRule", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateJournalRuleResponse")]
        string CreateJournalRule(string journalEmail, string scope, string recipientEmail, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateJournalRule", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateJournalRuleResponse")]
        System.Threading.Tasks.Task<string> CreateJournalRuleAsync(string journalEmail, string scope, string recipientEmail, bool enabled);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetJournalRule", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetJournalRuleResponse")]
        SolidCP.Providers.HostedSolution.ExchangeJournalRule GetJournalRule(string journalEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetJournalRule", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetJournalRuleResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeJournalRule> GetJournalRuleAsync(string journalEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetJournalRule", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetJournalRuleResponse")]
        void SetJournalRule(SolidCP.Providers.HostedSolution.ExchangeJournalRule rule);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetJournalRule", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetJournalRuleResponse")]
        System.Threading.Tasks.Task SetJournalRuleAsync(SolidCP.Providers.HostedSolution.ExchangeJournalRule rule);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveJournalRule", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveJournalRuleResponse")]
        void RemoveJournalRule(string journalEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveJournalRule", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveJournalRuleResponse")]
        System.Threading.Tasks.Task RemoveJournalRuleAsync(string journalEmail);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateContact", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateContactResponse")]
        void CreateContact(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateContact", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateContactResponse")]
        System.Threading.Tasks.Task CreateContactAsync(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeleteContact", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeleteContactResponse")]
        void DeleteContact(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeleteContact", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeleteContactResponse")]
        System.Threading.Tasks.Task DeleteContactAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetContactGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetContactGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeContact GetContactGeneralSettings(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetContactGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetContactGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactGeneralSettingsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetContactGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetContactGeneralSettingsResponse")]
        void SetContactGeneralSettings(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetContactGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetContactGeneralSettingsResponse")]
        System.Threading.Tasks.Task SetContactGeneralSettingsAsync(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetContactMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetContactMailFlowSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeContact GetContactMailFlowSettings(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetContactMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetContactMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactMailFlowSettingsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetContactMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetContactMailFlowSettingsResponse")]
        void SetContactMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetContactMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetContactMailFlowSettingsResponse")]
        System.Threading.Tasks.Task SetContactMailFlowSettingsAsync(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateDistributionList", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateDistributionListResponse")]
        void CreateDistributionList(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateDistributionList", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateDistributionListResponse")]
        System.Threading.Tasks.Task CreateDistributionListAsync(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeleteDistributionList", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeleteDistributionListResponse")]
        void DeleteDistributionList(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeleteDistributionList", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeleteDistributionListResponse")]
        System.Threading.Tasks.Task DeleteDistributionListAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListGeneralSettings(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListGeneralSettingsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListGeneralSettingsResponse")]
        void SetDistributionListGeneralSettings(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] members, string notes, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListGeneralSettingsResponse")]
        System.Threading.Tasks.Task SetDistributionListGeneralSettingsAsync(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] members, string notes, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListMailFlowSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListMailFlowSettings(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListMailFlowSettingsAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListMailFlowSettingsResponse")]
        void SetDistributionListMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListMailFlowSettingsResponse")]
        System.Threading.Tasks.Task SetDistributionListMailFlowSettingsAsync(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListEmailAddressesResponse")]
        SolidCP.Providers.HostedSolution.ExchangeEmailAddress[] GetDistributionListEmailAddresses(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListEmailAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]> GetDistributionListEmailAddressesAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListEmailAddressesResponse")]
        void SetDistributionListEmailAddresses(string accountName, string[] emailAddresses, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListEmailAddressesResponse")]
        System.Threading.Tasks.Task SetDistributionListEmailAddressesAsync(string accountName, string[] emailAddresses, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListPrimaryEmailAddressResponse")]
        void SetDistributionListPrimaryEmailAddress(string accountName, string emailAddress, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListPrimaryEmailAddressResponse")]
        System.Threading.Tasks.Task SetDistributionListPrimaryEmailAddressAsync(string accountName, string emailAddress, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListPermissions", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListPermissionsResponse")]
        void SetDistributionListPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListPermissions", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDistributionListPermissionsResponse")]
        System.Threading.Tasks.Task SetDistributionListPermissionsAsync(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts, string[] addressLists);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListPermissions", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListPermissionsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListPermissions(string organizationId, string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListPermissions", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetDistributionListPermissionsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListPermissionsAsync(string organizationId, string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDisclaimer", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDisclaimerResponse")]
        int SetDisclaimer(string name, string text);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetDisclaimer", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetDisclaimerResponse")]
        System.Threading.Tasks.Task<int> SetDisclaimerAsync(string name, string text);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDisclaimer", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDisclaimerResponse")]
        int RemoveDisclaimer(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDisclaimer", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDisclaimerResponse")]
        System.Threading.Tasks.Task<int> RemoveDisclaimerAsync(string name);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/AddDisclamerMember", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/AddDisclamerMemberResponse")]
        int AddDisclamerMember(string name, string member);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/AddDisclamerMember", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/AddDisclamerMemberResponse")]
        System.Threading.Tasks.Task<int> AddDisclamerMemberAsync(string name, string member);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDisclamerMember", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDisclamerMemberResponse")]
        int RemoveDisclamerMember(string name, string member);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDisclamerMember", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDisclamerMemberResponse")]
        System.Threading.Tasks.Task<int> RemoveDisclamerMemberAsync(string name, string member);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreatePublicFolder", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreatePublicFolderResponse")]
        void CreatePublicFolder(string organizationDistinguishedName, string organizationId, string securityGroup, string parentFolder, string folderName, bool mailEnabled, string accountName, string name, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreatePublicFolder", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreatePublicFolderResponse")]
        System.Threading.Tasks.Task CreatePublicFolderAsync(string organizationDistinguishedName, string organizationId, string securityGroup, string parentFolder, string folderName, bool mailEnabled, string accountName, string name, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeletePublicFolder", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeletePublicFolderResponse")]
        void DeletePublicFolder(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DeletePublicFolder", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DeletePublicFolderResponse")]
        System.Threading.Tasks.Task DeletePublicFolderAsync(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/EnableMailPublicFolder", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/EnableMailPublicFolderResponse")]
        void EnableMailPublicFolder(string organizationId, string folder, string accountName, string name, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/EnableMailPublicFolder", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/EnableMailPublicFolderResponse")]
        System.Threading.Tasks.Task EnableMailPublicFolderAsync(string organizationId, string folder, string accountName, string name, string domain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DisableMailPublicFolder", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DisableMailPublicFolderResponse")]
        void DisableMailPublicFolder(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/DisableMailPublicFolder", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/DisableMailPublicFolderResponse")]
        System.Threading.Tasks.Task DisableMailPublicFolderAsync(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderGeneralSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderGeneralSettings(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderGeneralSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderGeneralSettingsAsync(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderGeneralSettingsResponse")]
        void SetPublicFolderGeneralSettings(string organizationId, string folder, string newFolderName, bool hideFromAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderGeneralSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderGeneralSettingsResponse")]
        System.Threading.Tasks.Task SetPublicFolderGeneralSettingsAsync(string organizationId, string folder, string newFolderName, bool hideFromAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderMailFlowSettingsResponse")]
        SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderMailFlowSettings(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderMailFlowSettingsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderMailFlowSettingsAsync(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderMailFlowSettingsResponse")]
        void SetPublicFolderMailFlowSettings(string organizationId, string folder, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderMailFlowSettings", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderMailFlowSettingsResponse")]
        System.Threading.Tasks.Task SetPublicFolderMailFlowSettingsAsync(string organizationId, string folder, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderEmailAddressesResponse")]
        SolidCP.Providers.HostedSolution.ExchangeEmailAddress[] GetPublicFolderEmailAddresses(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderEmailAddressesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]> GetPublicFolderEmailAddressesAsync(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderEmailAddressesResponse")]
        void SetPublicFolderEmailAddresses(string organizationId, string folder, string[] emailAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderEmailAddresses", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderEmailAddressesResponse")]
        System.Threading.Tasks.Task SetPublicFolderEmailAddressesAsync(string organizationId, string folder, string[] emailAddresses);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderPrimaryEmailAddressResponse")]
        void SetPublicFolderPrimaryEmailAddress(string organizationId, string folder, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderPrimaryEmailAddress", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetPublicFolderPrimaryEmailAddressResponse")]
        System.Threading.Tasks.Task SetPublicFolderPrimaryEmailAddressAsync(string organizationId, string folder, string emailAddress);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFoldersStatistics", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFoldersStatisticsResponse")]
        SolidCP.Providers.HostedSolution.ExchangeItemStatistics[] GetPublicFoldersStatistics(string organizationId, string[] folders);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFoldersStatistics", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFoldersStatisticsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]> GetPublicFoldersStatisticsAsync(string organizationId, string[] folders);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFoldersRecursive", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFoldersRecursiveResponse")]
        string[] GetPublicFoldersRecursive(string organizationId, string parent);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFoldersRecursive", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFoldersRecursiveResponse")]
        System.Threading.Tasks.Task<string[]> GetPublicFoldersRecursiveAsync(string organizationId, string parent);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderSize", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderSizeResponse")]
        long GetPublicFolderSize(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderSize", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPublicFolderSizeResponse")]
        System.Threading.Tasks.Task<long> GetPublicFolderSizeAsync(string organizationId, string folder);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationRootPublicFolder", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationRootPublicFolderResponse")]
        string CreateOrganizationRootPublicFolder(string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationRootPublicFolder", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationRootPublicFolderResponse")]
        System.Threading.Tasks.Task<string> CreateOrganizationRootPublicFolderAsync(string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationActiveSyncPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationActiveSyncPolicyResponse")]
        void CreateOrganizationActiveSyncPolicy(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationActiveSyncPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CreateOrganizationActiveSyncPolicyResponse")]
        System.Threading.Tasks.Task CreateOrganizationActiveSyncPolicyAsync(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetActiveSyncPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetActiveSyncPolicyResponse")]
        SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy GetActiveSyncPolicy(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetActiveSyncPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetActiveSyncPolicyResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy> GetActiveSyncPolicyAsync(string organizationId);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetActiveSyncPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetActiveSyncPolicyResponse")]
        void SetActiveSyncPolicy(string id, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInterval);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetActiveSyncPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetActiveSyncPolicyResponse")]
        System.Threading.Tasks.Task SetActiveSyncPolicyAsync(string id, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInterval);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMobileDevices", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMobileDevicesResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMobileDevice[] GetMobileDevices(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMobileDevices", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMobileDevicesResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice[]> GetMobileDevicesAsync(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMobileDevice", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMobileDeviceResponse")]
        SolidCP.Providers.HostedSolution.ExchangeMobileDevice GetMobileDevice(string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetMobileDevice", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetMobileDeviceResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice> GetMobileDeviceAsync(string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/WipeDataFromDevice", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/WipeDataFromDeviceResponse")]
        void WipeDataFromDevice(string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/WipeDataFromDevice", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/WipeDataFromDeviceResponse")]
        System.Threading.Tasks.Task WipeDataFromDeviceAsync(string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CancelRemoteWipeRequest", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CancelRemoteWipeRequestResponse")]
        void CancelRemoteWipeRequest(string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/CancelRemoteWipeRequest", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/CancelRemoteWipeRequestResponse")]
        System.Threading.Tasks.Task CancelRemoteWipeRequestAsync(string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDevice", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDeviceResponse")]
        void RemoveDevice(string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDevice", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveDeviceResponse")]
        System.Threading.Tasks.Task RemoveDeviceAsync(string id);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/ExportMailBox", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/ExportMailBoxResponse")]
        SolidCP.Providers.Common.ResultObject ExportMailBox(string organizationId, string accountName, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/ExportMailBox", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/ExportMailBoxResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ExportMailBoxAsync(string organizationId, string accountName, string storagePath);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailBoxArchiving", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailBoxArchivingResponse")]
        SolidCP.Providers.Common.ResultObject SetMailBoxArchiving(string organizationId, string accountName, bool archive, long archiveQuotaKB, long archiveWarningQuotaKB, string RetentionPolicy);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetMailBoxArchiving", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetMailBoxArchivingResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetMailBoxArchivingAsync(string organizationId, string accountName, bool archive, long archiveQuotaKB, long archiveWarningQuotaKB, string RetentionPolicy);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetRetentionPolicyTagResponse")]
        SolidCP.Providers.Common.ResultObject SetRetentionPolicyTag(string Identity, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagType Type, int AgeLimitForRetention, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagAction RetentionAction);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetRetentionPolicyTagResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRetentionPolicyTagAsync(string Identity, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagType Type, int AgeLimitForRetention, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagAction RetentionAction);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveRetentionPolicyTagResponse")]
        SolidCP.Providers.Common.ResultObject RemoveRetentionPolicyTag(string Identity);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveRetentionPolicyTag", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveRetentionPolicyTagResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRetentionPolicyTagAsync(string Identity);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetRetentionPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetRetentionPolicyResponse")]
        SolidCP.Providers.Common.ResultObject SetRetentionPolicy(string Identity, string[] RetentionPolicyTagLinks);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetRetentionPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetRetentionPolicyResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRetentionPolicyAsync(string Identity, string[] RetentionPolicyTagLinks);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveRetentionPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveRetentionPolicyResponse")]
        SolidCP.Providers.Common.ResultObject RemoveRetentionPolicy(string Identity);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/RemoveRetentionPolicy", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/RemoveRetentionPolicyResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRetentionPolicyAsync(string Identity);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetPicture", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetPictureResponse")]
        SolidCP.Providers.Common.ResultObject SetPicture(string accountName, byte[] picture);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/SetPicture", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/SetPictureResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetPictureAsync(string accountName, byte[] picture);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPicture", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPictureResponse")]
        SolidCP.Providers.ResultObjects.BytesResult GetPicture(string accountName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IExchangeServer/GetPicture", ReplyAction = "http://smbsaas/solidcp/server/IExchangeServer/GetPictureResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BytesResult> GetPictureAsync(string accountName);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class ExchangeServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IExchangeServer
    {
        public bool CheckAccountCredentials(string username, string password)
        {
            return Invoke<bool>("SolidCP.Server.ExchangeServer", "CheckAccountCredentials", username, password);
        }

        public async System.Threading.Tasks.Task<bool> CheckAccountCredentialsAsync(string username, string password)
        {
            return await InvokeAsync<bool>("SolidCP.Server.ExchangeServer", "CheckAccountCredentials", username, password);
        }

        public SolidCP.Providers.HostedSolution.Organization ExtendToExchangeOrganization(string organizationId, string securityGroup, bool IsConsumer)
        {
            return Invoke<SolidCP.Providers.HostedSolution.Organization>("SolidCP.Server.ExchangeServer", "ExtendToExchangeOrganization", organizationId, securityGroup, IsConsumer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> ExtendToExchangeOrganizationAsync(string organizationId, string securityGroup, bool IsConsumer)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.Organization>("SolidCP.Server.ExchangeServer", "ExtendToExchangeOrganization", organizationId, securityGroup, IsConsumer);
        }

        public string CreateMailEnableUser(string upn, string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string mailboxDatabase, string offlineAddressBook, string addressBookPolicy, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool hideFromAddressBook, bool isConsumer, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning)
        {
            return Invoke<string>("SolidCP.Server.ExchangeServer", "CreateMailEnableUser", upn, organizationId, organizationDistinguishedName, securityGroup, organizationDomain, accountType, mailboxDatabase, offlineAddressBook, addressBookPolicy, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, hideFromAddressBook, isConsumer, enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning);
        }

        public async System.Threading.Tasks.Task<string> CreateMailEnableUserAsync(string upn, string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string mailboxDatabase, string offlineAddressBook, string addressBookPolicy, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool hideFromAddressBook, bool isConsumer, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning)
        {
            return await InvokeAsync<string>("SolidCP.Server.ExchangeServer", "CreateMailEnableUser", upn, organizationId, organizationDistinguishedName, securityGroup, organizationDomain, accountType, mailboxDatabase, offlineAddressBook, addressBookPolicy, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, hideFromAddressBook, isConsumer, enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning);
        }

        public SolidCP.Providers.HostedSolution.Organization CreateOrganizationOfflineAddressBook(string organizationId, string securityGroup, string oabVirtualDir)
        {
            return Invoke<SolidCP.Providers.HostedSolution.Organization>("SolidCP.Server.ExchangeServer", "CreateOrganizationOfflineAddressBook", organizationId, securityGroup, oabVirtualDir);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> CreateOrganizationOfflineAddressBookAsync(string organizationId, string securityGroup, string oabVirtualDir)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.Organization>("SolidCP.Server.ExchangeServer", "CreateOrganizationOfflineAddressBook", organizationId, securityGroup, oabVirtualDir);
        }

        public void UpdateOrganizationOfflineAddressBook(string id)
        {
            Invoke("SolidCP.Server.ExchangeServer", "UpdateOrganizationOfflineAddressBook", id);
        }

        public async System.Threading.Tasks.Task UpdateOrganizationOfflineAddressBookAsync(string id)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "UpdateOrganizationOfflineAddressBook", id);
        }

        public string GetOABVirtualDirectory()
        {
            return Invoke<string>("SolidCP.Server.ExchangeServer", "GetOABVirtualDirectory");
        }

        public async System.Threading.Tasks.Task<string> GetOABVirtualDirectoryAsync()
        {
            return await InvokeAsync<string>("SolidCP.Server.ExchangeServer", "GetOABVirtualDirectory");
        }

        public SolidCP.Providers.HostedSolution.Organization CreateOrganizationAddressBookPolicy(string organizationId, string gal, string addressBook, string roomList, string oab)
        {
            return Invoke<SolidCP.Providers.HostedSolution.Organization>("SolidCP.Server.ExchangeServer", "CreateOrganizationAddressBookPolicy", organizationId, gal, addressBook, roomList, oab);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> CreateOrganizationAddressBookPolicyAsync(string organizationId, string gal, string addressBook, string roomList, string oab)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.Organization>("SolidCP.Server.ExchangeServer", "CreateOrganizationAddressBookPolicy", organizationId, gal, addressBook, roomList, oab);
        }

        public bool DeleteOrganization(string organizationId, string distinguishedName, string globalAddressList, string addressList, string roomList, string offlineAddressBook, string securityGroup, string addressBookPolicy, SolidCP.Providers.HostedSolution.ExchangeDomainName[] /*List*/ acceptedDomains)
        {
            return Invoke<bool>("SolidCP.Server.ExchangeServer", "DeleteOrganization", organizationId, distinguishedName, globalAddressList, addressList, roomList, offlineAddressBook, securityGroup, addressBookPolicy, acceptedDomains.ToList());
        }

        public async System.Threading.Tasks.Task<bool> DeleteOrganizationAsync(string organizationId, string distinguishedName, string globalAddressList, string addressList, string roomList, string offlineAddressBook, string securityGroup, string addressBookPolicy, SolidCP.Providers.HostedSolution.ExchangeDomainName[] /*List*/ acceptedDomains)
        {
            return await InvokeAsync<bool>("SolidCP.Server.ExchangeServer", "DeleteOrganization", organizationId, distinguishedName, globalAddressList, addressList, roomList, offlineAddressBook, securityGroup, addressBookPolicy, acceptedDomains);
        }

        public void SetOrganizationStorageLimits(string organizationDistinguishedName, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetOrganizationStorageLimits", organizationDistinguishedName, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays);
        }

        public async System.Threading.Tasks.Task SetOrganizationStorageLimitsAsync(string organizationDistinguishedName, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetOrganizationStorageLimits", organizationDistinguishedName, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays);
        }

        public SolidCP.Providers.HostedSolution.ExchangeItemStatistics[] GetMailboxesStatistics(string organizationDistinguishedName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]>("SolidCP.Server.ExchangeServer", "GetMailboxesStatistics", organizationDistinguishedName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]> GetMailboxesStatisticsAsync(string organizationDistinguishedName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]>("SolidCP.Server.ExchangeServer", "GetMailboxesStatistics", organizationDistinguishedName);
        }

        public void AddAuthoritativeDomain(string domain)
        {
            Invoke("SolidCP.Server.ExchangeServer", "AddAuthoritativeDomain", domain);
        }

        public async System.Threading.Tasks.Task AddAuthoritativeDomainAsync(string domain)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "AddAuthoritativeDomain", domain);
        }

        public void ChangeAcceptedDomainType(string domain, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType domainType)
        {
            Invoke("SolidCP.Server.ExchangeServer", "ChangeAcceptedDomainType", domain, domainType);
        }

        public async System.Threading.Tasks.Task ChangeAcceptedDomainTypeAsync(string domain, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType domainType)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "ChangeAcceptedDomainType", domain, domainType);
        }

        public string[] GetAuthoritativeDomains()
        {
            return Invoke<string[]>("SolidCP.Server.ExchangeServer", "GetAuthoritativeDomains");
        }

        public async System.Threading.Tasks.Task<string[]> GetAuthoritativeDomainsAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.ExchangeServer", "GetAuthoritativeDomains");
        }

        public void DeleteAuthoritativeDomain(string domain)
        {
            Invoke("SolidCP.Server.ExchangeServer", "DeleteAuthoritativeDomain", domain);
        }

        public async System.Threading.Tasks.Task DeleteAuthoritativeDomainAsync(string domain)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "DeleteAuthoritativeDomain", domain);
        }

        public void DeleteMailbox(string accountName)
        {
            Invoke("SolidCP.Server.ExchangeServer", "DeleteMailbox", accountName);
        }

        public async System.Threading.Tasks.Task DeleteMailboxAsync(string accountName)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "DeleteMailbox", accountName);
        }

        public void DisableMailbox(string accountName)
        {
            Invoke("SolidCP.Server.ExchangeServer", "DisableMailbox", accountName);
        }

        public async System.Threading.Tasks.Task DisableMailboxAsync(string accountName)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "DisableMailbox", accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings GetMailboxAutoReplySettings(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings>("SolidCP.Server.ExchangeServer", "GetMailboxAutoReplySettings", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings> GetMailboxAutoReplySettingsAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings>("SolidCP.Server.ExchangeServer", "GetMailboxAutoReplySettings", accountName);
        }

        public void SetMailboxAutoReplySettings(string accountName, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetMailboxAutoReplySettings", accountName, settings);
        }

        public async System.Threading.Tasks.Task SetMailboxAutoReplySettingsAsync(string accountName, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetMailboxAutoReplySettings", accountName, settings);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxGeneralSettings(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.Server.ExchangeServer", "GetMailboxGeneralSettings", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxGeneralSettingsAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.Server.ExchangeServer", "GetMailboxGeneralSettings", accountName);
        }

        public void SetMailboxGeneralSettings(string accountName, bool hideFromAddressBook, bool disabled)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetMailboxGeneralSettings", accountName, hideFromAddressBook, disabled);
        }

        public async System.Threading.Tasks.Task SetMailboxGeneralSettingsAsync(string accountName, bool hideFromAddressBook, bool disabled)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetMailboxGeneralSettings", accountName, hideFromAddressBook, disabled);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxMailFlowSettings(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.Server.ExchangeServer", "GetMailboxMailFlowSettings", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxMailFlowSettingsAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.Server.ExchangeServer", "GetMailboxMailFlowSettings", accountName);
        }

        public void SetMailboxMailFlowSettings(string accountName, bool enableForwarding, int saveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetMailboxMailFlowSettings", accountName, enableForwarding, saveSentItems, forwardingAccountName, forwardToBoth, sendOnBehalfAccounts, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task SetMailboxMailFlowSettingsAsync(string accountName, bool enableForwarding, int saveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetMailboxMailFlowSettings", accountName, enableForwarding, saveSentItems, forwardingAccountName, forwardToBoth, sendOnBehalfAccounts, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxAdvancedSettings(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.Server.ExchangeServer", "GetMailboxAdvancedSettings", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxAdvancedSettingsAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.Server.ExchangeServer", "GetMailboxAdvancedSettings", accountName);
        }

        public void SetMailboxAdvancedSettings(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetMailboxAdvancedSettings", organizationId, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning, litigationHoldUrl, litigationHoldMsg);
        }

        public async System.Threading.Tasks.Task SetMailboxAdvancedSettingsAsync(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetMailboxAdvancedSettings", organizationId, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning, litigationHoldUrl, litigationHoldMsg);
        }

        public SolidCP.Providers.HostedSolution.ExchangeEmailAddress[] GetMailboxEmailAddresses(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]>("SolidCP.Server.ExchangeServer", "GetMailboxEmailAddresses", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]> GetMailboxEmailAddressesAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]>("SolidCP.Server.ExchangeServer", "GetMailboxEmailAddresses", accountName);
        }

        public void SetMailboxEmailAddresses(string accountName, string[] emailAddresses)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetMailboxEmailAddresses", accountName, emailAddresses);
        }

        public async System.Threading.Tasks.Task SetMailboxEmailAddressesAsync(string accountName, string[] emailAddresses)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetMailboxEmailAddresses", accountName, emailAddresses);
        }

        public void SetMailboxPrimaryEmailAddress(string accountName, string emailAddress)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetMailboxPrimaryEmailAddress", accountName, emailAddress);
        }

        public async System.Threading.Tasks.Task SetMailboxPrimaryEmailAddressAsync(string accountName, string emailAddress)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetMailboxPrimaryEmailAddress", accountName, emailAddress);
        }

        public void SetMailboxPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetMailboxPermissions", organizationId, accountName, sendAsAccounts, fullAccessAccounts, onBehalfOfAccounts, calendarAccounts, contactAccounts);
        }

        public async System.Threading.Tasks.Task SetMailboxPermissionsAsync(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetMailboxPermissions", organizationId, accountName, sendAsAccounts, fullAccessAccounts, onBehalfOfAccounts, calendarAccounts, contactAccounts);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxPermissions(string organizationId, string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.Server.ExchangeServer", "GetMailboxPermissions", organizationId, accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxPermissionsAsync(string organizationId, string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailbox>("SolidCP.Server.ExchangeServer", "GetMailboxPermissions", organizationId, accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics GetMailboxStatistics(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics>("SolidCP.Server.ExchangeServer", "GetMailboxStatistics", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics> GetMailboxStatisticsAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics>("SolidCP.Server.ExchangeServer", "GetMailboxStatistics", accountName);
        }

        public string[] SetDefaultPublicFolderMailbox(string id, string organizationId, string organizationDistinguishedName)
        {
            return Invoke<string[]>("SolidCP.Server.ExchangeServer", "SetDefaultPublicFolderMailbox", id, organizationId, organizationDistinguishedName);
        }

        public async System.Threading.Tasks.Task<string[]> SetDefaultPublicFolderMailboxAsync(string id, string organizationId, string organizationDistinguishedName)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.ExchangeServer", "SetDefaultPublicFolderMailbox", id, organizationId, organizationDistinguishedName);
        }

        public string CreateJournalRule(string journalEmail, string scope, string recipientEmail, bool enabled)
        {
            return Invoke<string>("SolidCP.Server.ExchangeServer", "CreateJournalRule", journalEmail, scope, recipientEmail, enabled);
        }

        public async System.Threading.Tasks.Task<string> CreateJournalRuleAsync(string journalEmail, string scope, string recipientEmail, bool enabled)
        {
            return await InvokeAsync<string>("SolidCP.Server.ExchangeServer", "CreateJournalRule", journalEmail, scope, recipientEmail, enabled);
        }

        public SolidCP.Providers.HostedSolution.ExchangeJournalRule GetJournalRule(string journalEmail)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeJournalRule>("SolidCP.Server.ExchangeServer", "GetJournalRule", journalEmail);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeJournalRule> GetJournalRuleAsync(string journalEmail)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeJournalRule>("SolidCP.Server.ExchangeServer", "GetJournalRule", journalEmail);
        }

        public void SetJournalRule(SolidCP.Providers.HostedSolution.ExchangeJournalRule rule)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetJournalRule", rule);
        }

        public async System.Threading.Tasks.Task SetJournalRuleAsync(SolidCP.Providers.HostedSolution.ExchangeJournalRule rule)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetJournalRule", rule);
        }

        public void RemoveJournalRule(string journalEmail)
        {
            Invoke("SolidCP.Server.ExchangeServer", "RemoveJournalRule", journalEmail);
        }

        public async System.Threading.Tasks.Task RemoveJournalRuleAsync(string journalEmail)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "RemoveJournalRule", journalEmail);
        }

        public void CreateContact(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain)
        {
            Invoke("SolidCP.Server.ExchangeServer", "CreateContact", organizationId, organizationDistinguishedName, contactDisplayName, contactAccountName, contactEmail, defaultOrganizationDomain);
        }

        public async System.Threading.Tasks.Task CreateContactAsync(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "CreateContact", organizationId, organizationDistinguishedName, contactDisplayName, contactAccountName, contactEmail, defaultOrganizationDomain);
        }

        public void DeleteContact(string accountName)
        {
            Invoke("SolidCP.Server.ExchangeServer", "DeleteContact", accountName);
        }

        public async System.Threading.Tasks.Task DeleteContactAsync(string accountName)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "DeleteContact", accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeContact GetContactGeneralSettings(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeContact>("SolidCP.Server.ExchangeServer", "GetContactGeneralSettings", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactGeneralSettingsAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeContact>("SolidCP.Server.ExchangeServer", "GetContactGeneralSettings", accountName);
        }

        public void SetContactGeneralSettings(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetContactGeneralSettings", accountName, displayName, email, hideFromAddressBook, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, useMapiRichTextFormat, defaultDomain);
        }

        public async System.Threading.Tasks.Task SetContactGeneralSettingsAsync(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetContactGeneralSettings", accountName, displayName, email, hideFromAddressBook, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, useMapiRichTextFormat, defaultDomain);
        }

        public SolidCP.Providers.HostedSolution.ExchangeContact GetContactMailFlowSettings(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeContact>("SolidCP.Server.ExchangeServer", "GetContactMailFlowSettings", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactMailFlowSettingsAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeContact>("SolidCP.Server.ExchangeServer", "GetContactMailFlowSettings", accountName);
        }

        public void SetContactMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetContactMailFlowSettings", accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task SetContactMailFlowSettingsAsync(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetContactMailFlowSettings", accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public void CreateDistributionList(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy, string[] addressLists)
        {
            Invoke("SolidCP.Server.ExchangeServer", "CreateDistributionList", organizationId, organizationDistinguishedName, displayName, accountName, name, domain, managedBy, addressLists);
        }

        public async System.Threading.Tasks.Task CreateDistributionListAsync(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy, string[] addressLists)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "CreateDistributionList", organizationId, organizationDistinguishedName, displayName, accountName, name, domain, managedBy, addressLists);
        }

        public void DeleteDistributionList(string accountName)
        {
            Invoke("SolidCP.Server.ExchangeServer", "DeleteDistributionList", accountName);
        }

        public async System.Threading.Tasks.Task DeleteDistributionListAsync(string accountName)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "DeleteDistributionList", accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListGeneralSettings(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeDistributionList>("SolidCP.Server.ExchangeServer", "GetDistributionListGeneralSettings", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListGeneralSettingsAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeDistributionList>("SolidCP.Server.ExchangeServer", "GetDistributionListGeneralSettings", accountName);
        }

        public void SetDistributionListGeneralSettings(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] members, string notes, string[] addressLists)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetDistributionListGeneralSettings", accountName, displayName, hideFromAddressBook, managedBy, members, notes, addressLists);
        }

        public async System.Threading.Tasks.Task SetDistributionListGeneralSettingsAsync(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] members, string notes, string[] addressLists)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetDistributionListGeneralSettings", accountName, displayName, hideFromAddressBook, managedBy, members, notes, addressLists);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListMailFlowSettings(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeDistributionList>("SolidCP.Server.ExchangeServer", "GetDistributionListMailFlowSettings", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListMailFlowSettingsAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeDistributionList>("SolidCP.Server.ExchangeServer", "GetDistributionListMailFlowSettings", accountName);
        }

        public void SetDistributionListMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication, string[] addressLists)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetDistributionListMailFlowSettings", accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication, addressLists);
        }

        public async System.Threading.Tasks.Task SetDistributionListMailFlowSettingsAsync(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication, string[] addressLists)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetDistributionListMailFlowSettings", accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication, addressLists);
        }

        public SolidCP.Providers.HostedSolution.ExchangeEmailAddress[] GetDistributionListEmailAddresses(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]>("SolidCP.Server.ExchangeServer", "GetDistributionListEmailAddresses", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]> GetDistributionListEmailAddressesAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]>("SolidCP.Server.ExchangeServer", "GetDistributionListEmailAddresses", accountName);
        }

        public void SetDistributionListEmailAddresses(string accountName, string[] emailAddresses, string[] addressLists)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetDistributionListEmailAddresses", accountName, emailAddresses, addressLists);
        }

        public async System.Threading.Tasks.Task SetDistributionListEmailAddressesAsync(string accountName, string[] emailAddresses, string[] addressLists)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetDistributionListEmailAddresses", accountName, emailAddresses, addressLists);
        }

        public void SetDistributionListPrimaryEmailAddress(string accountName, string emailAddress, string[] addressLists)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetDistributionListPrimaryEmailAddress", accountName, emailAddress, addressLists);
        }

        public async System.Threading.Tasks.Task SetDistributionListPrimaryEmailAddressAsync(string accountName, string emailAddress, string[] addressLists)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetDistributionListPrimaryEmailAddress", accountName, emailAddress, addressLists);
        }

        public void SetDistributionListPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts, string[] addressLists)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetDistributionListPermissions", organizationId, accountName, sendAsAccounts, sendOnBehalfAccounts, addressLists);
        }

        public async System.Threading.Tasks.Task SetDistributionListPermissionsAsync(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts, string[] addressLists)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetDistributionListPermissions", organizationId, accountName, sendAsAccounts, sendOnBehalfAccounts, addressLists);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListPermissions(string organizationId, string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeDistributionList>("SolidCP.Server.ExchangeServer", "GetDistributionListPermissions", organizationId, accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListPermissionsAsync(string organizationId, string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeDistributionList>("SolidCP.Server.ExchangeServer", "GetDistributionListPermissions", organizationId, accountName);
        }

        public int SetDisclaimer(string name, string text)
        {
            return Invoke<int>("SolidCP.Server.ExchangeServer", "SetDisclaimer", name, text);
        }

        public async System.Threading.Tasks.Task<int> SetDisclaimerAsync(string name, string text)
        {
            return await InvokeAsync<int>("SolidCP.Server.ExchangeServer", "SetDisclaimer", name, text);
        }

        public int RemoveDisclaimer(string name)
        {
            return Invoke<int>("SolidCP.Server.ExchangeServer", "RemoveDisclaimer", name);
        }

        public async System.Threading.Tasks.Task<int> RemoveDisclaimerAsync(string name)
        {
            return await InvokeAsync<int>("SolidCP.Server.ExchangeServer", "RemoveDisclaimer", name);
        }

        public int AddDisclamerMember(string name, string member)
        {
            return Invoke<int>("SolidCP.Server.ExchangeServer", "AddDisclamerMember", name, member);
        }

        public async System.Threading.Tasks.Task<int> AddDisclamerMemberAsync(string name, string member)
        {
            return await InvokeAsync<int>("SolidCP.Server.ExchangeServer", "AddDisclamerMember", name, member);
        }

        public int RemoveDisclamerMember(string name, string member)
        {
            return Invoke<int>("SolidCP.Server.ExchangeServer", "RemoveDisclamerMember", name, member);
        }

        public async System.Threading.Tasks.Task<int> RemoveDisclamerMemberAsync(string name, string member)
        {
            return await InvokeAsync<int>("SolidCP.Server.ExchangeServer", "RemoveDisclamerMember", name, member);
        }

        public void CreatePublicFolder(string organizationDistinguishedName, string organizationId, string securityGroup, string parentFolder, string folderName, bool mailEnabled, string accountName, string name, string domain)
        {
            Invoke("SolidCP.Server.ExchangeServer", "CreatePublicFolder", organizationDistinguishedName, organizationId, securityGroup, parentFolder, folderName, mailEnabled, accountName, name, domain);
        }

        public async System.Threading.Tasks.Task CreatePublicFolderAsync(string organizationDistinguishedName, string organizationId, string securityGroup, string parentFolder, string folderName, bool mailEnabled, string accountName, string name, string domain)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "CreatePublicFolder", organizationDistinguishedName, organizationId, securityGroup, parentFolder, folderName, mailEnabled, accountName, name, domain);
        }

        public void DeletePublicFolder(string organizationId, string folder)
        {
            Invoke("SolidCP.Server.ExchangeServer", "DeletePublicFolder", organizationId, folder);
        }

        public async System.Threading.Tasks.Task DeletePublicFolderAsync(string organizationId, string folder)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "DeletePublicFolder", organizationId, folder);
        }

        public void EnableMailPublicFolder(string organizationId, string folder, string accountName, string name, string domain)
        {
            Invoke("SolidCP.Server.ExchangeServer", "EnableMailPublicFolder", organizationId, folder, accountName, name, domain);
        }

        public async System.Threading.Tasks.Task EnableMailPublicFolderAsync(string organizationId, string folder, string accountName, string name, string domain)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "EnableMailPublicFolder", organizationId, folder, accountName, name, domain);
        }

        public void DisableMailPublicFolder(string organizationId, string folder)
        {
            Invoke("SolidCP.Server.ExchangeServer", "DisableMailPublicFolder", organizationId, folder);
        }

        public async System.Threading.Tasks.Task DisableMailPublicFolderAsync(string organizationId, string folder)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "DisableMailPublicFolder", organizationId, folder);
        }

        public SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderGeneralSettings(string organizationId, string folder)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangePublicFolder>("SolidCP.Server.ExchangeServer", "GetPublicFolderGeneralSettings", organizationId, folder);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderGeneralSettingsAsync(string organizationId, string folder)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangePublicFolder>("SolidCP.Server.ExchangeServer", "GetPublicFolderGeneralSettings", organizationId, folder);
        }

        public void SetPublicFolderGeneralSettings(string organizationId, string folder, string newFolderName, bool hideFromAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetPublicFolderGeneralSettings", organizationId, folder, newFolderName, hideFromAddressBook, accounts);
        }

        public async System.Threading.Tasks.Task SetPublicFolderGeneralSettingsAsync(string organizationId, string folder, string newFolderName, bool hideFromAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetPublicFolderGeneralSettings", organizationId, folder, newFolderName, hideFromAddressBook, accounts);
        }

        public SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderMailFlowSettings(string organizationId, string folder)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangePublicFolder>("SolidCP.Server.ExchangeServer", "GetPublicFolderMailFlowSettings", organizationId, folder);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderMailFlowSettingsAsync(string organizationId, string folder)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangePublicFolder>("SolidCP.Server.ExchangeServer", "GetPublicFolderMailFlowSettings", organizationId, folder);
        }

        public void SetPublicFolderMailFlowSettings(string organizationId, string folder, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetPublicFolderMailFlowSettings", organizationId, folder, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task SetPublicFolderMailFlowSettingsAsync(string organizationId, string folder, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetPublicFolderMailFlowSettings", organizationId, folder, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public SolidCP.Providers.HostedSolution.ExchangeEmailAddress[] GetPublicFolderEmailAddresses(string organizationId, string folder)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]>("SolidCP.Server.ExchangeServer", "GetPublicFolderEmailAddresses", organizationId, folder);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]> GetPublicFolderEmailAddressesAsync(string organizationId, string folder)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]>("SolidCP.Server.ExchangeServer", "GetPublicFolderEmailAddresses", organizationId, folder);
        }

        public void SetPublicFolderEmailAddresses(string organizationId, string folder, string[] emailAddresses)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetPublicFolderEmailAddresses", organizationId, folder, emailAddresses);
        }

        public async System.Threading.Tasks.Task SetPublicFolderEmailAddressesAsync(string organizationId, string folder, string[] emailAddresses)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetPublicFolderEmailAddresses", organizationId, folder, emailAddresses);
        }

        public void SetPublicFolderPrimaryEmailAddress(string organizationId, string folder, string emailAddress)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetPublicFolderPrimaryEmailAddress", organizationId, folder, emailAddress);
        }

        public async System.Threading.Tasks.Task SetPublicFolderPrimaryEmailAddressAsync(string organizationId, string folder, string emailAddress)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetPublicFolderPrimaryEmailAddress", organizationId, folder, emailAddress);
        }

        public SolidCP.Providers.HostedSolution.ExchangeItemStatistics[] GetPublicFoldersStatistics(string organizationId, string[] folders)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]>("SolidCP.Server.ExchangeServer", "GetPublicFoldersStatistics", organizationId, folders);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]> GetPublicFoldersStatisticsAsync(string organizationId, string[] folders)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]>("SolidCP.Server.ExchangeServer", "GetPublicFoldersStatistics", organizationId, folders);
        }

        public string[] GetPublicFoldersRecursive(string organizationId, string parent)
        {
            return Invoke<string[]>("SolidCP.Server.ExchangeServer", "GetPublicFoldersRecursive", organizationId, parent);
        }

        public async System.Threading.Tasks.Task<string[]> GetPublicFoldersRecursiveAsync(string organizationId, string parent)
        {
            return await InvokeAsync<string[]>("SolidCP.Server.ExchangeServer", "GetPublicFoldersRecursive", organizationId, parent);
        }

        public long GetPublicFolderSize(string organizationId, string folder)
        {
            return Invoke<long>("SolidCP.Server.ExchangeServer", "GetPublicFolderSize", organizationId, folder);
        }

        public async System.Threading.Tasks.Task<long> GetPublicFolderSizeAsync(string organizationId, string folder)
        {
            return await InvokeAsync<long>("SolidCP.Server.ExchangeServer", "GetPublicFolderSize", organizationId, folder);
        }

        public string CreateOrganizationRootPublicFolder(string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain)
        {
            return Invoke<string>("SolidCP.Server.ExchangeServer", "CreateOrganizationRootPublicFolder", organizationId, organizationDistinguishedName, securityGroup, organizationDomain);
        }

        public async System.Threading.Tasks.Task<string> CreateOrganizationRootPublicFolderAsync(string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain)
        {
            return await InvokeAsync<string>("SolidCP.Server.ExchangeServer", "CreateOrganizationRootPublicFolder", organizationId, organizationDistinguishedName, securityGroup, organizationDomain);
        }

        public void CreateOrganizationActiveSyncPolicy(string organizationId)
        {
            Invoke("SolidCP.Server.ExchangeServer", "CreateOrganizationActiveSyncPolicy", organizationId);
        }

        public async System.Threading.Tasks.Task CreateOrganizationActiveSyncPolicyAsync(string organizationId)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "CreateOrganizationActiveSyncPolicy", organizationId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy GetActiveSyncPolicy(string organizationId)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy>("SolidCP.Server.ExchangeServer", "GetActiveSyncPolicy", organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy> GetActiveSyncPolicyAsync(string organizationId)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy>("SolidCP.Server.ExchangeServer", "GetActiveSyncPolicy", organizationId);
        }

        public void SetActiveSyncPolicy(string id, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInterval)
        {
            Invoke("SolidCP.Server.ExchangeServer", "SetActiveSyncPolicy", id, allowNonProvisionableDevices, attachmentsEnabled, maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired, passwordRecoveryEnabled, deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts, minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInterval);
        }

        public async System.Threading.Tasks.Task SetActiveSyncPolicyAsync(string id, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInterval)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "SetActiveSyncPolicy", id, allowNonProvisionableDevices, attachmentsEnabled, maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired, passwordRecoveryEnabled, deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts, minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInterval);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMobileDevice[] GetMobileDevices(string accountName)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMobileDevice[]>("SolidCP.Server.ExchangeServer", "GetMobileDevices", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice[]> GetMobileDevicesAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMobileDevice[]>("SolidCP.Server.ExchangeServer", "GetMobileDevices", accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMobileDevice GetMobileDevice(string id)
        {
            return Invoke<SolidCP.Providers.HostedSolution.ExchangeMobileDevice>("SolidCP.Server.ExchangeServer", "GetMobileDevice", id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice> GetMobileDeviceAsync(string id)
        {
            return await InvokeAsync<SolidCP.Providers.HostedSolution.ExchangeMobileDevice>("SolidCP.Server.ExchangeServer", "GetMobileDevice", id);
        }

        public void WipeDataFromDevice(string id)
        {
            Invoke("SolidCP.Server.ExchangeServer", "WipeDataFromDevice", id);
        }

        public async System.Threading.Tasks.Task WipeDataFromDeviceAsync(string id)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "WipeDataFromDevice", id);
        }

        public void CancelRemoteWipeRequest(string id)
        {
            Invoke("SolidCP.Server.ExchangeServer", "CancelRemoteWipeRequest", id);
        }

        public async System.Threading.Tasks.Task CancelRemoteWipeRequestAsync(string id)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "CancelRemoteWipeRequest", id);
        }

        public void RemoveDevice(string id)
        {
            Invoke("SolidCP.Server.ExchangeServer", "RemoveDevice", id);
        }

        public async System.Threading.Tasks.Task RemoveDeviceAsync(string id)
        {
            await InvokeAsync("SolidCP.Server.ExchangeServer", "RemoveDevice", id);
        }

        public SolidCP.Providers.Common.ResultObject ExportMailBox(string organizationId, string accountName, string storagePath)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "ExportMailBox", organizationId, accountName, storagePath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ExportMailBoxAsync(string organizationId, string accountName, string storagePath)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "ExportMailBox", organizationId, accountName, storagePath);
        }

        public SolidCP.Providers.Common.ResultObject SetMailBoxArchiving(string organizationId, string accountName, bool archive, long archiveQuotaKB, long archiveWarningQuotaKB, string RetentionPolicy)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "SetMailBoxArchiving", organizationId, accountName, archive, archiveQuotaKB, archiveWarningQuotaKB, RetentionPolicy);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetMailBoxArchivingAsync(string organizationId, string accountName, bool archive, long archiveQuotaKB, long archiveWarningQuotaKB, string RetentionPolicy)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "SetMailBoxArchiving", organizationId, accountName, archive, archiveQuotaKB, archiveWarningQuotaKB, RetentionPolicy);
        }

        public SolidCP.Providers.Common.ResultObject SetRetentionPolicyTag(string Identity, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagType Type, int AgeLimitForRetention, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagAction RetentionAction)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "SetRetentionPolicyTag", Identity, Type, AgeLimitForRetention, RetentionAction);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRetentionPolicyTagAsync(string Identity, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagType Type, int AgeLimitForRetention, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagAction RetentionAction)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "SetRetentionPolicyTag", Identity, Type, AgeLimitForRetention, RetentionAction);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRetentionPolicyTag(string Identity)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "RemoveRetentionPolicyTag", Identity);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRetentionPolicyTagAsync(string Identity)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "RemoveRetentionPolicyTag", Identity);
        }

        public SolidCP.Providers.Common.ResultObject SetRetentionPolicy(string Identity, string[] RetentionPolicyTagLinks)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "SetRetentionPolicy", Identity, RetentionPolicyTagLinks);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRetentionPolicyAsync(string Identity, string[] RetentionPolicyTagLinks)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "SetRetentionPolicy", Identity, RetentionPolicyTagLinks);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRetentionPolicy(string Identity)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "RemoveRetentionPolicy", Identity);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRetentionPolicyAsync(string Identity)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "RemoveRetentionPolicy", Identity);
        }

        public SolidCP.Providers.Common.ResultObject SetPicture(string accountName, byte[] picture)
        {
            return Invoke<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "SetPicture", accountName, picture);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetPictureAsync(string accountName, byte[] picture)
        {
            return await InvokeAsync<SolidCP.Providers.Common.ResultObject>("SolidCP.Server.ExchangeServer", "SetPicture", accountName, picture);
        }

        public SolidCP.Providers.ResultObjects.BytesResult GetPicture(string accountName)
        {
            return Invoke<SolidCP.Providers.ResultObjects.BytesResult>("SolidCP.Server.ExchangeServer", "GetPicture", accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BytesResult> GetPictureAsync(string accountName)
        {
            return await InvokeAsync<SolidCP.Providers.ResultObjects.BytesResult>("SolidCP.Server.ExchangeServer", "GetPicture", accountName);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class ExchangeServer : SolidCP.Web.Client.ClientBase<IExchangeServer, ExchangeServerAssemblyClient>, IExchangeServer
    {
        public bool CheckAccountCredentials(string username, string password)
        {
            return base.Client.CheckAccountCredentials(username, password);
        }

        public async System.Threading.Tasks.Task<bool> CheckAccountCredentialsAsync(string username, string password)
        {
            return await base.Client.CheckAccountCredentialsAsync(username, password);
        }

        public SolidCP.Providers.HostedSolution.Organization ExtendToExchangeOrganization(string organizationId, string securityGroup, bool IsConsumer)
        {
            return base.Client.ExtendToExchangeOrganization(organizationId, securityGroup, IsConsumer);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> ExtendToExchangeOrganizationAsync(string organizationId, string securityGroup, bool IsConsumer)
        {
            return await base.Client.ExtendToExchangeOrganizationAsync(organizationId, securityGroup, IsConsumer);
        }

        public string CreateMailEnableUser(string upn, string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string mailboxDatabase, string offlineAddressBook, string addressBookPolicy, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool hideFromAddressBook, bool isConsumer, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning)
        {
            return base.Client.CreateMailEnableUser(upn, organizationId, organizationDistinguishedName, securityGroup, organizationDomain, accountType, mailboxDatabase, offlineAddressBook, addressBookPolicy, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, hideFromAddressBook, isConsumer, enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning);
        }

        public async System.Threading.Tasks.Task<string> CreateMailEnableUserAsync(string upn, string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain, SolidCP.Providers.HostedSolution.ExchangeAccountType accountType, string mailboxDatabase, string offlineAddressBook, string addressBookPolicy, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool hideFromAddressBook, bool isConsumer, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning)
        {
            return await base.Client.CreateMailEnableUserAsync(upn, organizationId, organizationDistinguishedName, securityGroup, organizationDomain, accountType, mailboxDatabase, offlineAddressBook, addressBookPolicy, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, hideFromAddressBook, isConsumer, enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning);
        }

        public SolidCP.Providers.HostedSolution.Organization CreateOrganizationOfflineAddressBook(string organizationId, string securityGroup, string oabVirtualDir)
        {
            return base.Client.CreateOrganizationOfflineAddressBook(organizationId, securityGroup, oabVirtualDir);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> CreateOrganizationOfflineAddressBookAsync(string organizationId, string securityGroup, string oabVirtualDir)
        {
            return await base.Client.CreateOrganizationOfflineAddressBookAsync(organizationId, securityGroup, oabVirtualDir);
        }

        public void UpdateOrganizationOfflineAddressBook(string id)
        {
            base.Client.UpdateOrganizationOfflineAddressBook(id);
        }

        public async System.Threading.Tasks.Task UpdateOrganizationOfflineAddressBookAsync(string id)
        {
            await base.Client.UpdateOrganizationOfflineAddressBookAsync(id);
        }

        public string GetOABVirtualDirectory()
        {
            return base.Client.GetOABVirtualDirectory();
        }

        public async System.Threading.Tasks.Task<string> GetOABVirtualDirectoryAsync()
        {
            return await base.Client.GetOABVirtualDirectoryAsync();
        }

        public SolidCP.Providers.HostedSolution.Organization CreateOrganizationAddressBookPolicy(string organizationId, string gal, string addressBook, string roomList, string oab)
        {
            return base.Client.CreateOrganizationAddressBookPolicy(organizationId, gal, addressBook, roomList, oab);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.Organization> CreateOrganizationAddressBookPolicyAsync(string organizationId, string gal, string addressBook, string roomList, string oab)
        {
            return await base.Client.CreateOrganizationAddressBookPolicyAsync(organizationId, gal, addressBook, roomList, oab);
        }

        public bool DeleteOrganization(string organizationId, string distinguishedName, string globalAddressList, string addressList, string roomList, string offlineAddressBook, string securityGroup, string addressBookPolicy, SolidCP.Providers.HostedSolution.ExchangeDomainName[] /*List*/ acceptedDomains)
        {
            return base.Client.DeleteOrganization(organizationId, distinguishedName, globalAddressList, addressList, roomList, offlineAddressBook, securityGroup, addressBookPolicy, acceptedDomains);
        }

        public async System.Threading.Tasks.Task<bool> DeleteOrganizationAsync(string organizationId, string distinguishedName, string globalAddressList, string addressList, string roomList, string offlineAddressBook, string securityGroup, string addressBookPolicy, SolidCP.Providers.HostedSolution.ExchangeDomainName[] /*List*/ acceptedDomains)
        {
            return await base.Client.DeleteOrganizationAsync(organizationId, distinguishedName, globalAddressList, addressList, roomList, offlineAddressBook, securityGroup, addressBookPolicy, acceptedDomains);
        }

        public void SetOrganizationStorageLimits(string organizationDistinguishedName, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays)
        {
            base.Client.SetOrganizationStorageLimits(organizationDistinguishedName, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays);
        }

        public async System.Threading.Tasks.Task SetOrganizationStorageLimitsAsync(string organizationDistinguishedName, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays)
        {
            await base.Client.SetOrganizationStorageLimitsAsync(organizationDistinguishedName, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays);
        }

        public SolidCP.Providers.HostedSolution.ExchangeItemStatistics[] GetMailboxesStatistics(string organizationDistinguishedName)
        {
            return base.Client.GetMailboxesStatistics(organizationDistinguishedName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]> GetMailboxesStatisticsAsync(string organizationDistinguishedName)
        {
            return await base.Client.GetMailboxesStatisticsAsync(organizationDistinguishedName);
        }

        public void AddAuthoritativeDomain(string domain)
        {
            base.Client.AddAuthoritativeDomain(domain);
        }

        public async System.Threading.Tasks.Task AddAuthoritativeDomainAsync(string domain)
        {
            await base.Client.AddAuthoritativeDomainAsync(domain);
        }

        public void ChangeAcceptedDomainType(string domain, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType domainType)
        {
            base.Client.ChangeAcceptedDomainType(domain, domainType);
        }

        public async System.Threading.Tasks.Task ChangeAcceptedDomainTypeAsync(string domain, SolidCP.Providers.HostedSolution.ExchangeAcceptedDomainType domainType)
        {
            await base.Client.ChangeAcceptedDomainTypeAsync(domain, domainType);
        }

        public string[] GetAuthoritativeDomains()
        {
            return base.Client.GetAuthoritativeDomains();
        }

        public async System.Threading.Tasks.Task<string[]> GetAuthoritativeDomainsAsync()
        {
            return await base.Client.GetAuthoritativeDomainsAsync();
        }

        public void DeleteAuthoritativeDomain(string domain)
        {
            base.Client.DeleteAuthoritativeDomain(domain);
        }

        public async System.Threading.Tasks.Task DeleteAuthoritativeDomainAsync(string domain)
        {
            await base.Client.DeleteAuthoritativeDomainAsync(domain);
        }

        public void DeleteMailbox(string accountName)
        {
            base.Client.DeleteMailbox(accountName);
        }

        public async System.Threading.Tasks.Task DeleteMailboxAsync(string accountName)
        {
            await base.Client.DeleteMailboxAsync(accountName);
        }

        public void DisableMailbox(string accountName)
        {
            base.Client.DisableMailbox(accountName);
        }

        public async System.Threading.Tasks.Task DisableMailboxAsync(string accountName)
        {
            await base.Client.DisableMailboxAsync(accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings GetMailboxAutoReplySettings(string accountName)
        {
            return base.Client.GetMailboxAutoReplySettings(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings> GetMailboxAutoReplySettingsAsync(string accountName)
        {
            return await base.Client.GetMailboxAutoReplySettingsAsync(accountName);
        }

        public void SetMailboxAutoReplySettings(string accountName, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings)
        {
            base.Client.SetMailboxAutoReplySettings(accountName, settings);
        }

        public async System.Threading.Tasks.Task SetMailboxAutoReplySettingsAsync(string accountName, SolidCP.Providers.HostedSolution.ExchangeMailboxAutoReplySettings settings)
        {
            await base.Client.SetMailboxAutoReplySettingsAsync(accountName, settings);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxGeneralSettings(string accountName)
        {
            return base.Client.GetMailboxGeneralSettings(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxGeneralSettingsAsync(string accountName)
        {
            return await base.Client.GetMailboxGeneralSettingsAsync(accountName);
        }

        public void SetMailboxGeneralSettings(string accountName, bool hideFromAddressBook, bool disabled)
        {
            base.Client.SetMailboxGeneralSettings(accountName, hideFromAddressBook, disabled);
        }

        public async System.Threading.Tasks.Task SetMailboxGeneralSettingsAsync(string accountName, bool hideFromAddressBook, bool disabled)
        {
            await base.Client.SetMailboxGeneralSettingsAsync(accountName, hideFromAddressBook, disabled);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxMailFlowSettings(string accountName)
        {
            return base.Client.GetMailboxMailFlowSettings(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxMailFlowSettingsAsync(string accountName)
        {
            return await base.Client.GetMailboxMailFlowSettingsAsync(accountName);
        }

        public void SetMailboxMailFlowSettings(string accountName, bool enableForwarding, int saveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            base.Client.SetMailboxMailFlowSettings(accountName, enableForwarding, saveSentItems, forwardingAccountName, forwardToBoth, sendOnBehalfAccounts, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task SetMailboxMailFlowSettingsAsync(string accountName, bool enableForwarding, int saveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            await base.Client.SetMailboxMailFlowSettingsAsync(accountName, enableForwarding, saveSentItems, forwardingAccountName, forwardToBoth, sendOnBehalfAccounts, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxAdvancedSettings(string accountName)
        {
            return base.Client.GetMailboxAdvancedSettings(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxAdvancedSettingsAsync(string accountName)
        {
            return await base.Client.GetMailboxAdvancedSettingsAsync(accountName);
        }

        public void SetMailboxAdvancedSettings(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg)
        {
            base.Client.SetMailboxAdvancedSettings(organizationId, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning, litigationHoldUrl, litigationHoldMsg);
        }

        public async System.Threading.Tasks.Task SetMailboxAdvancedSettingsAsync(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg)
        {
            await base.Client.SetMailboxAdvancedSettingsAsync(organizationId, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning, litigationHoldUrl, litigationHoldMsg);
        }

        public SolidCP.Providers.HostedSolution.ExchangeEmailAddress[] GetMailboxEmailAddresses(string accountName)
        {
            return base.Client.GetMailboxEmailAddresses(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]> GetMailboxEmailAddressesAsync(string accountName)
        {
            return await base.Client.GetMailboxEmailAddressesAsync(accountName);
        }

        public void SetMailboxEmailAddresses(string accountName, string[] emailAddresses)
        {
            base.Client.SetMailboxEmailAddresses(accountName, emailAddresses);
        }

        public async System.Threading.Tasks.Task SetMailboxEmailAddressesAsync(string accountName, string[] emailAddresses)
        {
            await base.Client.SetMailboxEmailAddressesAsync(accountName, emailAddresses);
        }

        public void SetMailboxPrimaryEmailAddress(string accountName, string emailAddress)
        {
            base.Client.SetMailboxPrimaryEmailAddress(accountName, emailAddress);
        }

        public async System.Threading.Tasks.Task SetMailboxPrimaryEmailAddressAsync(string accountName, string emailAddress)
        {
            await base.Client.SetMailboxPrimaryEmailAddressAsync(accountName, emailAddress);
        }

        public void SetMailboxPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts)
        {
            base.Client.SetMailboxPermissions(organizationId, accountName, sendAsAccounts, fullAccessAccounts, onBehalfOfAccounts, calendarAccounts, contactAccounts);
        }

        public async System.Threading.Tasks.Task SetMailboxPermissionsAsync(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts, string[] onBehalfOfAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] calendarAccounts, SolidCP.Providers.HostedSolution.ExchangeAccount[] contactAccounts)
        {
            await base.Client.SetMailboxPermissionsAsync(organizationId, accountName, sendAsAccounts, fullAccessAccounts, onBehalfOfAccounts, calendarAccounts, contactAccounts);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailbox GetMailboxPermissions(string organizationId, string accountName)
        {
            return base.Client.GetMailboxPermissions(organizationId, accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailbox> GetMailboxPermissionsAsync(string organizationId, string accountName)
        {
            return await base.Client.GetMailboxPermissionsAsync(organizationId, accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics GetMailboxStatistics(string accountName)
        {
            return base.Client.GetMailboxStatistics(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMailboxStatistics> GetMailboxStatisticsAsync(string accountName)
        {
            return await base.Client.GetMailboxStatisticsAsync(accountName);
        }

        public string[] SetDefaultPublicFolderMailbox(string id, string organizationId, string organizationDistinguishedName)
        {
            return base.Client.SetDefaultPublicFolderMailbox(id, organizationId, organizationDistinguishedName);
        }

        public async System.Threading.Tasks.Task<string[]> SetDefaultPublicFolderMailboxAsync(string id, string organizationId, string organizationDistinguishedName)
        {
            return await base.Client.SetDefaultPublicFolderMailboxAsync(id, organizationId, organizationDistinguishedName);
        }

        public string CreateJournalRule(string journalEmail, string scope, string recipientEmail, bool enabled)
        {
            return base.Client.CreateJournalRule(journalEmail, scope, recipientEmail, enabled);
        }

        public async System.Threading.Tasks.Task<string> CreateJournalRuleAsync(string journalEmail, string scope, string recipientEmail, bool enabled)
        {
            return await base.Client.CreateJournalRuleAsync(journalEmail, scope, recipientEmail, enabled);
        }

        public SolidCP.Providers.HostedSolution.ExchangeJournalRule GetJournalRule(string journalEmail)
        {
            return base.Client.GetJournalRule(journalEmail);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeJournalRule> GetJournalRuleAsync(string journalEmail)
        {
            return await base.Client.GetJournalRuleAsync(journalEmail);
        }

        public void SetJournalRule(SolidCP.Providers.HostedSolution.ExchangeJournalRule rule)
        {
            base.Client.SetJournalRule(rule);
        }

        public async System.Threading.Tasks.Task SetJournalRuleAsync(SolidCP.Providers.HostedSolution.ExchangeJournalRule rule)
        {
            await base.Client.SetJournalRuleAsync(rule);
        }

        public void RemoveJournalRule(string journalEmail)
        {
            base.Client.RemoveJournalRule(journalEmail);
        }

        public async System.Threading.Tasks.Task RemoveJournalRuleAsync(string journalEmail)
        {
            await base.Client.RemoveJournalRuleAsync(journalEmail);
        }

        public void CreateContact(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain)
        {
            base.Client.CreateContact(organizationId, organizationDistinguishedName, contactDisplayName, contactAccountName, contactEmail, defaultOrganizationDomain);
        }

        public async System.Threading.Tasks.Task CreateContactAsync(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain)
        {
            await base.Client.CreateContactAsync(organizationId, organizationDistinguishedName, contactDisplayName, contactAccountName, contactEmail, defaultOrganizationDomain);
        }

        public void DeleteContact(string accountName)
        {
            base.Client.DeleteContact(accountName);
        }

        public async System.Threading.Tasks.Task DeleteContactAsync(string accountName)
        {
            await base.Client.DeleteContactAsync(accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeContact GetContactGeneralSettings(string accountName)
        {
            return base.Client.GetContactGeneralSettings(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactGeneralSettingsAsync(string accountName)
        {
            return await base.Client.GetContactGeneralSettingsAsync(accountName);
        }

        public void SetContactGeneralSettings(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain)
        {
            base.Client.SetContactGeneralSettings(accountName, displayName, email, hideFromAddressBook, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, useMapiRichTextFormat, defaultDomain);
        }

        public async System.Threading.Tasks.Task SetContactGeneralSettingsAsync(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain)
        {
            await base.Client.SetContactGeneralSettingsAsync(accountName, displayName, email, hideFromAddressBook, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, useMapiRichTextFormat, defaultDomain);
        }

        public SolidCP.Providers.HostedSolution.ExchangeContact GetContactMailFlowSettings(string accountName)
        {
            return base.Client.GetContactMailFlowSettings(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeContact> GetContactMailFlowSettingsAsync(string accountName)
        {
            return await base.Client.GetContactMailFlowSettingsAsync(accountName);
        }

        public void SetContactMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            base.Client.SetContactMailFlowSettings(accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task SetContactMailFlowSettingsAsync(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            await base.Client.SetContactMailFlowSettingsAsync(accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public void CreateDistributionList(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy, string[] addressLists)
        {
            base.Client.CreateDistributionList(organizationId, organizationDistinguishedName, displayName, accountName, name, domain, managedBy, addressLists);
        }

        public async System.Threading.Tasks.Task CreateDistributionListAsync(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy, string[] addressLists)
        {
            await base.Client.CreateDistributionListAsync(organizationId, organizationDistinguishedName, displayName, accountName, name, domain, managedBy, addressLists);
        }

        public void DeleteDistributionList(string accountName)
        {
            base.Client.DeleteDistributionList(accountName);
        }

        public async System.Threading.Tasks.Task DeleteDistributionListAsync(string accountName)
        {
            await base.Client.DeleteDistributionListAsync(accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListGeneralSettings(string accountName)
        {
            return base.Client.GetDistributionListGeneralSettings(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListGeneralSettingsAsync(string accountName)
        {
            return await base.Client.GetDistributionListGeneralSettingsAsync(accountName);
        }

        public void SetDistributionListGeneralSettings(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] members, string notes, string[] addressLists)
        {
            base.Client.SetDistributionListGeneralSettings(accountName, displayName, hideFromAddressBook, managedBy, members, notes, addressLists);
        }

        public async System.Threading.Tasks.Task SetDistributionListGeneralSettingsAsync(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] members, string notes, string[] addressLists)
        {
            await base.Client.SetDistributionListGeneralSettingsAsync(accountName, displayName, hideFromAddressBook, managedBy, members, notes, addressLists);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListMailFlowSettings(string accountName)
        {
            return base.Client.GetDistributionListMailFlowSettings(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListMailFlowSettingsAsync(string accountName)
        {
            return await base.Client.GetDistributionListMailFlowSettingsAsync(accountName);
        }

        public void SetDistributionListMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication, string[] addressLists)
        {
            base.Client.SetDistributionListMailFlowSettings(accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication, addressLists);
        }

        public async System.Threading.Tasks.Task SetDistributionListMailFlowSettingsAsync(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication, string[] addressLists)
        {
            await base.Client.SetDistributionListMailFlowSettingsAsync(accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication, addressLists);
        }

        public SolidCP.Providers.HostedSolution.ExchangeEmailAddress[] GetDistributionListEmailAddresses(string accountName)
        {
            return base.Client.GetDistributionListEmailAddresses(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]> GetDistributionListEmailAddressesAsync(string accountName)
        {
            return await base.Client.GetDistributionListEmailAddressesAsync(accountName);
        }

        public void SetDistributionListEmailAddresses(string accountName, string[] emailAddresses, string[] addressLists)
        {
            base.Client.SetDistributionListEmailAddresses(accountName, emailAddresses, addressLists);
        }

        public async System.Threading.Tasks.Task SetDistributionListEmailAddressesAsync(string accountName, string[] emailAddresses, string[] addressLists)
        {
            await base.Client.SetDistributionListEmailAddressesAsync(accountName, emailAddresses, addressLists);
        }

        public void SetDistributionListPrimaryEmailAddress(string accountName, string emailAddress, string[] addressLists)
        {
            base.Client.SetDistributionListPrimaryEmailAddress(accountName, emailAddress, addressLists);
        }

        public async System.Threading.Tasks.Task SetDistributionListPrimaryEmailAddressAsync(string accountName, string emailAddress, string[] addressLists)
        {
            await base.Client.SetDistributionListPrimaryEmailAddressAsync(accountName, emailAddress, addressLists);
        }

        public void SetDistributionListPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts, string[] addressLists)
        {
            base.Client.SetDistributionListPermissions(organizationId, accountName, sendAsAccounts, sendOnBehalfAccounts, addressLists);
        }

        public async System.Threading.Tasks.Task SetDistributionListPermissionsAsync(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts, string[] addressLists)
        {
            await base.Client.SetDistributionListPermissionsAsync(organizationId, accountName, sendAsAccounts, sendOnBehalfAccounts, addressLists);
        }

        public SolidCP.Providers.HostedSolution.ExchangeDistributionList GetDistributionListPermissions(string organizationId, string accountName)
        {
            return base.Client.GetDistributionListPermissions(organizationId, accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeDistributionList> GetDistributionListPermissionsAsync(string organizationId, string accountName)
        {
            return await base.Client.GetDistributionListPermissionsAsync(organizationId, accountName);
        }

        public int SetDisclaimer(string name, string text)
        {
            return base.Client.SetDisclaimer(name, text);
        }

        public async System.Threading.Tasks.Task<int> SetDisclaimerAsync(string name, string text)
        {
            return await base.Client.SetDisclaimerAsync(name, text);
        }

        public int RemoveDisclaimer(string name)
        {
            return base.Client.RemoveDisclaimer(name);
        }

        public async System.Threading.Tasks.Task<int> RemoveDisclaimerAsync(string name)
        {
            return await base.Client.RemoveDisclaimerAsync(name);
        }

        public int AddDisclamerMember(string name, string member)
        {
            return base.Client.AddDisclamerMember(name, member);
        }

        public async System.Threading.Tasks.Task<int> AddDisclamerMemberAsync(string name, string member)
        {
            return await base.Client.AddDisclamerMemberAsync(name, member);
        }

        public int RemoveDisclamerMember(string name, string member)
        {
            return base.Client.RemoveDisclamerMember(name, member);
        }

        public async System.Threading.Tasks.Task<int> RemoveDisclamerMemberAsync(string name, string member)
        {
            return await base.Client.RemoveDisclamerMemberAsync(name, member);
        }

        public void CreatePublicFolder(string organizationDistinguishedName, string organizationId, string securityGroup, string parentFolder, string folderName, bool mailEnabled, string accountName, string name, string domain)
        {
            base.Client.CreatePublicFolder(organizationDistinguishedName, organizationId, securityGroup, parentFolder, folderName, mailEnabled, accountName, name, domain);
        }

        public async System.Threading.Tasks.Task CreatePublicFolderAsync(string organizationDistinguishedName, string organizationId, string securityGroup, string parentFolder, string folderName, bool mailEnabled, string accountName, string name, string domain)
        {
            await base.Client.CreatePublicFolderAsync(organizationDistinguishedName, organizationId, securityGroup, parentFolder, folderName, mailEnabled, accountName, name, domain);
        }

        public void DeletePublicFolder(string organizationId, string folder)
        {
            base.Client.DeletePublicFolder(organizationId, folder);
        }

        public async System.Threading.Tasks.Task DeletePublicFolderAsync(string organizationId, string folder)
        {
            await base.Client.DeletePublicFolderAsync(organizationId, folder);
        }

        public void EnableMailPublicFolder(string organizationId, string folder, string accountName, string name, string domain)
        {
            base.Client.EnableMailPublicFolder(organizationId, folder, accountName, name, domain);
        }

        public async System.Threading.Tasks.Task EnableMailPublicFolderAsync(string organizationId, string folder, string accountName, string name, string domain)
        {
            await base.Client.EnableMailPublicFolderAsync(organizationId, folder, accountName, name, domain);
        }

        public void DisableMailPublicFolder(string organizationId, string folder)
        {
            base.Client.DisableMailPublicFolder(organizationId, folder);
        }

        public async System.Threading.Tasks.Task DisableMailPublicFolderAsync(string organizationId, string folder)
        {
            await base.Client.DisableMailPublicFolderAsync(organizationId, folder);
        }

        public SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderGeneralSettings(string organizationId, string folder)
        {
            return base.Client.GetPublicFolderGeneralSettings(organizationId, folder);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderGeneralSettingsAsync(string organizationId, string folder)
        {
            return await base.Client.GetPublicFolderGeneralSettingsAsync(organizationId, folder);
        }

        public void SetPublicFolderGeneralSettings(string organizationId, string folder, string newFolderName, bool hideFromAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts)
        {
            base.Client.SetPublicFolderGeneralSettings(organizationId, folder, newFolderName, hideFromAddressBook, accounts);
        }

        public async System.Threading.Tasks.Task SetPublicFolderGeneralSettingsAsync(string organizationId, string folder, string newFolderName, bool hideFromAddressBook, SolidCP.Providers.HostedSolution.ExchangeAccount[] accounts)
        {
            await base.Client.SetPublicFolderGeneralSettingsAsync(organizationId, folder, newFolderName, hideFromAddressBook, accounts);
        }

        public SolidCP.Providers.HostedSolution.ExchangePublicFolder GetPublicFolderMailFlowSettings(string organizationId, string folder)
        {
            return base.Client.GetPublicFolderMailFlowSettings(organizationId, folder);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangePublicFolder> GetPublicFolderMailFlowSettingsAsync(string organizationId, string folder)
        {
            return await base.Client.GetPublicFolderMailFlowSettingsAsync(organizationId, folder);
        }

        public void SetPublicFolderMailFlowSettings(string organizationId, string folder, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            base.Client.SetPublicFolderMailFlowSettings(organizationId, folder, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public async System.Threading.Tasks.Task SetPublicFolderMailFlowSettingsAsync(string organizationId, string folder, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            await base.Client.SetPublicFolderMailFlowSettingsAsync(organizationId, folder, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public SolidCP.Providers.HostedSolution.ExchangeEmailAddress[] GetPublicFolderEmailAddresses(string organizationId, string folder)
        {
            return base.Client.GetPublicFolderEmailAddresses(organizationId, folder);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeEmailAddress[]> GetPublicFolderEmailAddressesAsync(string organizationId, string folder)
        {
            return await base.Client.GetPublicFolderEmailAddressesAsync(organizationId, folder);
        }

        public void SetPublicFolderEmailAddresses(string organizationId, string folder, string[] emailAddresses)
        {
            base.Client.SetPublicFolderEmailAddresses(organizationId, folder, emailAddresses);
        }

        public async System.Threading.Tasks.Task SetPublicFolderEmailAddressesAsync(string organizationId, string folder, string[] emailAddresses)
        {
            await base.Client.SetPublicFolderEmailAddressesAsync(organizationId, folder, emailAddresses);
        }

        public void SetPublicFolderPrimaryEmailAddress(string organizationId, string folder, string emailAddress)
        {
            base.Client.SetPublicFolderPrimaryEmailAddress(organizationId, folder, emailAddress);
        }

        public async System.Threading.Tasks.Task SetPublicFolderPrimaryEmailAddressAsync(string organizationId, string folder, string emailAddress)
        {
            await base.Client.SetPublicFolderPrimaryEmailAddressAsync(organizationId, folder, emailAddress);
        }

        public SolidCP.Providers.HostedSolution.ExchangeItemStatistics[] GetPublicFoldersStatistics(string organizationId, string[] folders)
        {
            return base.Client.GetPublicFoldersStatistics(organizationId, folders);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeItemStatistics[]> GetPublicFoldersStatisticsAsync(string organizationId, string[] folders)
        {
            return await base.Client.GetPublicFoldersStatisticsAsync(organizationId, folders);
        }

        public string[] GetPublicFoldersRecursive(string organizationId, string parent)
        {
            return base.Client.GetPublicFoldersRecursive(organizationId, parent);
        }

        public async System.Threading.Tasks.Task<string[]> GetPublicFoldersRecursiveAsync(string organizationId, string parent)
        {
            return await base.Client.GetPublicFoldersRecursiveAsync(organizationId, parent);
        }

        public long GetPublicFolderSize(string organizationId, string folder)
        {
            return base.Client.GetPublicFolderSize(organizationId, folder);
        }

        public async System.Threading.Tasks.Task<long> GetPublicFolderSizeAsync(string organizationId, string folder)
        {
            return await base.Client.GetPublicFolderSizeAsync(organizationId, folder);
        }

        public string CreateOrganizationRootPublicFolder(string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain)
        {
            return base.Client.CreateOrganizationRootPublicFolder(organizationId, organizationDistinguishedName, securityGroup, organizationDomain);
        }

        public async System.Threading.Tasks.Task<string> CreateOrganizationRootPublicFolderAsync(string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain)
        {
            return await base.Client.CreateOrganizationRootPublicFolderAsync(organizationId, organizationDistinguishedName, securityGroup, organizationDomain);
        }

        public void CreateOrganizationActiveSyncPolicy(string organizationId)
        {
            base.Client.CreateOrganizationActiveSyncPolicy(organizationId);
        }

        public async System.Threading.Tasks.Task CreateOrganizationActiveSyncPolicyAsync(string organizationId)
        {
            await base.Client.CreateOrganizationActiveSyncPolicyAsync(organizationId);
        }

        public SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy GetActiveSyncPolicy(string organizationId)
        {
            return base.Client.GetActiveSyncPolicy(organizationId);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeActiveSyncPolicy> GetActiveSyncPolicyAsync(string organizationId)
        {
            return await base.Client.GetActiveSyncPolicyAsync(organizationId);
        }

        public void SetActiveSyncPolicy(string id, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInterval)
        {
            base.Client.SetActiveSyncPolicy(id, allowNonProvisionableDevices, attachmentsEnabled, maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired, passwordRecoveryEnabled, deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts, minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInterval);
        }

        public async System.Threading.Tasks.Task SetActiveSyncPolicyAsync(string id, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInterval)
        {
            await base.Client.SetActiveSyncPolicyAsync(id, allowNonProvisionableDevices, attachmentsEnabled, maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired, passwordRecoveryEnabled, deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts, minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInterval);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMobileDevice[] GetMobileDevices(string accountName)
        {
            return base.Client.GetMobileDevices(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice[]> GetMobileDevicesAsync(string accountName)
        {
            return await base.Client.GetMobileDevicesAsync(accountName);
        }

        public SolidCP.Providers.HostedSolution.ExchangeMobileDevice GetMobileDevice(string id)
        {
            return base.Client.GetMobileDevice(id);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.HostedSolution.ExchangeMobileDevice> GetMobileDeviceAsync(string id)
        {
            return await base.Client.GetMobileDeviceAsync(id);
        }

        public void WipeDataFromDevice(string id)
        {
            base.Client.WipeDataFromDevice(id);
        }

        public async System.Threading.Tasks.Task WipeDataFromDeviceAsync(string id)
        {
            await base.Client.WipeDataFromDeviceAsync(id);
        }

        public void CancelRemoteWipeRequest(string id)
        {
            base.Client.CancelRemoteWipeRequest(id);
        }

        public async System.Threading.Tasks.Task CancelRemoteWipeRequestAsync(string id)
        {
            await base.Client.CancelRemoteWipeRequestAsync(id);
        }

        public void RemoveDevice(string id)
        {
            base.Client.RemoveDevice(id);
        }

        public async System.Threading.Tasks.Task RemoveDeviceAsync(string id)
        {
            await base.Client.RemoveDeviceAsync(id);
        }

        public SolidCP.Providers.Common.ResultObject ExportMailBox(string organizationId, string accountName, string storagePath)
        {
            return base.Client.ExportMailBox(organizationId, accountName, storagePath);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> ExportMailBoxAsync(string organizationId, string accountName, string storagePath)
        {
            return await base.Client.ExportMailBoxAsync(organizationId, accountName, storagePath);
        }

        public SolidCP.Providers.Common.ResultObject SetMailBoxArchiving(string organizationId, string accountName, bool archive, long archiveQuotaKB, long archiveWarningQuotaKB, string RetentionPolicy)
        {
            return base.Client.SetMailBoxArchiving(organizationId, accountName, archive, archiveQuotaKB, archiveWarningQuotaKB, RetentionPolicy);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetMailBoxArchivingAsync(string organizationId, string accountName, bool archive, long archiveQuotaKB, long archiveWarningQuotaKB, string RetentionPolicy)
        {
            return await base.Client.SetMailBoxArchivingAsync(organizationId, accountName, archive, archiveQuotaKB, archiveWarningQuotaKB, RetentionPolicy);
        }

        public SolidCP.Providers.Common.ResultObject SetRetentionPolicyTag(string Identity, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagType Type, int AgeLimitForRetention, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagAction RetentionAction)
        {
            return base.Client.SetRetentionPolicyTag(Identity, Type, AgeLimitForRetention, RetentionAction);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRetentionPolicyTagAsync(string Identity, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagType Type, int AgeLimitForRetention, SolidCP.Providers.HostedSolution.ExchangeRetentionPolicyTagAction RetentionAction)
        {
            return await base.Client.SetRetentionPolicyTagAsync(Identity, Type, AgeLimitForRetention, RetentionAction);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRetentionPolicyTag(string Identity)
        {
            return base.Client.RemoveRetentionPolicyTag(Identity);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRetentionPolicyTagAsync(string Identity)
        {
            return await base.Client.RemoveRetentionPolicyTagAsync(Identity);
        }

        public SolidCP.Providers.Common.ResultObject SetRetentionPolicy(string Identity, string[] RetentionPolicyTagLinks)
        {
            return base.Client.SetRetentionPolicy(Identity, RetentionPolicyTagLinks);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetRetentionPolicyAsync(string Identity, string[] RetentionPolicyTagLinks)
        {
            return await base.Client.SetRetentionPolicyAsync(Identity, RetentionPolicyTagLinks);
        }

        public SolidCP.Providers.Common.ResultObject RemoveRetentionPolicy(string Identity)
        {
            return base.Client.RemoveRetentionPolicy(Identity);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> RemoveRetentionPolicyAsync(string Identity)
        {
            return await base.Client.RemoveRetentionPolicyAsync(Identity);
        }

        public SolidCP.Providers.Common.ResultObject SetPicture(string accountName, byte[] picture)
        {
            return base.Client.SetPicture(accountName, picture);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.Common.ResultObject> SetPictureAsync(string accountName, byte[] picture)
        {
            return await base.Client.SetPictureAsync(accountName, picture);
        }

        public SolidCP.Providers.ResultObjects.BytesResult GetPicture(string accountName)
        {
            return base.Client.GetPicture(accountName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.ResultObjects.BytesResult> GetPictureAsync(string accountName)
        {
            return await base.Client.GetPictureAsync(accountName);
        }
    }
}
#endif