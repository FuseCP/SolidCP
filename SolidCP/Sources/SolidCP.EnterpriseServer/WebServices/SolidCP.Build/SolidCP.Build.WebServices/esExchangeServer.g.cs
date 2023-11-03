#if !Client
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SolidCP.Web.Services;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.EnterpriseServer.Base.HostedSolution;
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
    public interface IesExchangeServer
    {
        [WebMethod]
        [OperationContract]
        DataSet GetRawExchangeOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        OrganizationsPaged GetExchangeOrganizationsPaged(int packageId, bool recursive, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows);
        [WebMethod]
        [OperationContract]
        List<Organization> GetExchangeOrganizations(int packageId, bool recursive);
        [WebMethod]
        [OperationContract]
        Organization GetOrganization(int itemId);
        [WebMethod]
        [OperationContract]
        OrganizationStatistics GetOrganizationStatistics(int itemId);
        [WebMethod]
        [OperationContract]
        OrganizationStatistics GetOrganizationStatisticsByOrganization(int itemId);
        [WebMethod]
        [OperationContract]
        int GetExchangeServiceID(int itemId);
        [WebMethod]
        [OperationContract]
        int DeleteOrganization(int itemId);
        [WebMethod]
        [OperationContract]
        Organization GetOrganizationStorageLimits(int itemId);
        [WebMethod]
        [OperationContract]
        int SetOrganizationStorageLimits(int itemId, int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays, bool applyToMailboxes);
        [WebMethod]
        [OperationContract]
        ExchangeItemStatistics[] GetMailboxesStatistics(int itemId);
        [WebMethod]
        [OperationContract]
        ExchangeMailboxStatistics GetMailboxStatistics(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int CalculateOrganizationDiskspace(int itemId);
        [WebMethod]
        [OperationContract]
        ExchangeActiveSyncPolicy GetActiveSyncPolicy(int itemId);
        [WebMethod]
        [OperationContract]
        int SetActiveSyncPolicy(int itemId, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInteval);
        [WebMethod]
        [OperationContract]
        int AddAuthoritativeDomain(int itemId, int domainId);
        [WebMethod]
        [OperationContract]
        int DeleteAuthoritativeDomain(int itemId, int domainId);
        [WebMethod]
        [OperationContract]
        ExchangeAccountsPaged GetAccountsPaged(int itemId, string accountTypes, string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows, bool archiving);
        [WebMethod]
        [OperationContract]
        List<ExchangeAccount> GetAccounts(int itemId, ExchangeAccountType accountType);
        [WebMethod]
        [OperationContract]
        List<ExchangeAccount> GetExchangeAccountByMailboxPlanId(int itemId, int mailboxPlanId);
        [WebMethod]
        [OperationContract]
        List<ExchangeAccount> SearchAccounts(int itemId, bool includeMailboxes, bool includeContacts, bool includeDistributionLists, bool includeRooms, bool includeEquipment, bool IncludeSharedMailbox, bool includeSecurityGroups, string filterColumn, string filterValue, string sortColumn);
        [WebMethod]
        [OperationContract]
        List<ExchangeAccount> SearchAccountsByTypes(int itemId, ExchangeAccountType[] types, string filterColumn, string filterValue, string sortColumn);
        [WebMethod]
        [OperationContract]
        ExchangeAccount GetAccount(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ExchangeAccount GetAccountByAccountNameWithoutItemId(string accountName);
        [WebMethod]
        [OperationContract]
        ExchangeAccount SearchAccount(ExchangeAccountType accountType, string primaryEmailAddress);
        [WebMethod]
        [OperationContract]
        bool CheckAccountCredentials(int itemId, string email, string password);
        [WebMethod]
        [OperationContract]
        int CreateMailbox(int itemId, int accountId, ExchangeAccountType accountType, string accountName, string displayName, string name, string domain, string password, bool sendSetupInstructions, string setupInstructionMailAddress, int mailboxPlanId, int archivedPlanId, string subscriberNumber, bool EnableArchiving);
        [WebMethod]
        [OperationContract]
        string CreateJournalRule(int itemId, string journalEmail, string scope, string recipientEmail, bool enabled);
        [WebMethod]
        [OperationContract]
        ExchangeJournalRule GetJournalRule(int itemId, string journalEmail);
        [WebMethod]
        [OperationContract]
        int SetJournalRule(int itemId, ExchangeJournalRule rule);
        [WebMethod]
        [OperationContract]
        int DeleteMailbox(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int DisableMailbox(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ExchangeMailbox GetMailboxAdvancedSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ExchangeMailboxAutoReplySettings GetMailboxAutoReplySettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetMailboxAutoReplySettings(int itemId, int accountId, ExchangeMailboxAutoReplySettings settings);
        [WebMethod]
        [OperationContract]
        ExchangeMailbox GetMailboxGeneralSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetMailboxGeneralSettings(int itemId, int accountId, bool hideAddressBook, bool disabled);
        [WebMethod]
        [OperationContract]
        ExchangeEmailAddress[] GetMailboxEmailAddresses(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int AddMailboxEmailAddress(int itemId, int accountId, string emailAddress);
        [WebMethod]
        [OperationContract]
        int SetMailboxPrimaryEmailAddress(int itemId, int accountId, string emailAddress);
        [WebMethod]
        [OperationContract]
        int DeleteMailboxEmailAddresses(int itemId, int accountId, string[] emailAddresses);
        [WebMethod]
        [OperationContract]
        ExchangeMailbox GetMailboxMailFlowSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetMailboxMailFlowSettings(int itemId, int accountId, bool enableForwarding, int SaveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [WebMethod]
        [OperationContract]
        int SetExchangeMailboxPlan(int itemId, int accountId, int mailboxPlanId, int archivePlanId, bool EnableArchiving);
        [WebMethod]
        [OperationContract]
        string GetMailboxSetupInstructions(int itemId, int accountId, bool pmm, bool emailMode, bool signup, string passwordResetUrl);
        [WebMethod]
        [OperationContract]
        int SendMailboxSetupInstructions(int itemId, int accountId, bool signup, string to, string cc);
        [WebMethod]
        [OperationContract]
        int SetMailboxManagerSettings(int itemId, int accountId, bool pmmAllowed, MailboxManagerActions action);
        [WebMethod]
        [OperationContract]
        ExchangeMailbox GetMailboxPermissions(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetMailboxPermissions(int itemId, int accountId, string[] sendAsaccounts, string[] fullAccessAcounts, string[] onBehalfOfAccounts, ExchangeAccount[] calendarAccounts, ExchangeAccount[] contactAccounts);
        [WebMethod]
        [OperationContract]
        int ExportMailBox(int itemId, int accountId, string path);
        [WebMethod]
        [OperationContract]
        int SetDeletedMailbox(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int CreateContact(int itemId, string displayName, string email);
        [WebMethod]
        [OperationContract]
        int DeleteContact(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ExchangeContact GetContactGeneralSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetContactGeneralSettings(int itemId, int accountId, string displayName, string emailAddress, bool hideAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat);
        [WebMethod]
        [OperationContract]
        ExchangeContact GetContactMailFlowSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetContactMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [WebMethod]
        [OperationContract]
        int CreateDistributionList(int itemId, string displayName, string name, string domain, int managerId);
        [WebMethod]
        [OperationContract]
        int DeleteDistributionList(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ExchangeDistributionList GetDistributionListGeneralSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetDistributionListGeneralSettings(int itemId, int accountId, string displayName, bool hideAddressBook, string managerAccount, string[] memberAccounts, string notes);
        [WebMethod]
        [OperationContract]
        ExchangeDistributionList GetDistributionListMailFlowSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetDistributionListMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [WebMethod]
        [OperationContract]
        ExchangeEmailAddress[] GetDistributionListEmailAddresses(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int AddDistributionListEmailAddress(int itemId, int accountId, string emailAddress);
        [WebMethod]
        [OperationContract]
        int SetDistributionListPrimaryEmailAddress(int itemId, int accountId, string emailAddress);
        [WebMethod]
        [OperationContract]
        int DeleteDistributionListEmailAddresses(int itemId, int accountId, string[] emailAddresses);
        [WebMethod]
        [OperationContract]
        ResultObject SetDistributionListPermissions(int itemId, int accountId, string[] sendAsAccounts, string[] sendOnBehalfAccounts);
        [WebMethod]
        [OperationContract]
        ExchangeDistributionListResult GetDistributionListPermissions(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ExchangeAccount[] GetDistributionListsByMember(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int AddDistributionListMember(int itemId, string distributionListName, int memberId);
        [WebMethod]
        [OperationContract]
        int DeleteDistributionListMember(int itemId, string distributionListName, int memberId);
        [WebMethod]
        [OperationContract]
        ExchangeMobileDevice[] GetMobileDevices(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ExchangeMobileDevice GetMobileDevice(int itemId, string deviceId);
        [WebMethod]
        [OperationContract]
        void WipeDataFromDevice(int itemId, string deviceId);
        [WebMethod]
        [OperationContract]
        void CancelRemoteWipeRequest(int itemId, string deviceId);
        [WebMethod]
        [OperationContract]
        void RemoveDevice(int itemId, string deviceId);
        [WebMethod]
        [OperationContract]
        List<ExchangeMailboxPlan> GetExchangeMailboxPlans(int itemId, bool archiving);
        [WebMethod]
        [OperationContract]
        ExchangeMailboxPlan GetExchangeMailboxPlan(int itemId, int mailboxPlanId);
        [WebMethod]
        [OperationContract]
        int AddExchangeMailboxPlan(int itemId, ExchangeMailboxPlan mailboxPlan);
        [WebMethod]
        [OperationContract]
        int UpdateExchangeMailboxPlan(int itemId, ExchangeMailboxPlan mailboxPlan);
        [WebMethod]
        [OperationContract]
        int DeleteExchangeMailboxPlan(int itemId, int mailboxPlanId);
        [WebMethod]
        [OperationContract]
        void SetOrganizationDefaultExchangeMailboxPlan(int itemId, int mailboxPlanId);
        [WebMethod]
        [OperationContract]
        List<ExchangeRetentionPolicyTag> GetExchangeRetentionPolicyTags(int itemId);
        [WebMethod]
        [OperationContract]
        ExchangeRetentionPolicyTag GetExchangeRetentionPolicyTag(int itemId, int tagId);
        [WebMethod]
        [OperationContract]
        IntResult AddExchangeRetentionPolicyTag(int itemId, ExchangeRetentionPolicyTag tag);
        [WebMethod]
        [OperationContract]
        ResultObject UpdateExchangeRetentionPolicyTag(int itemId, ExchangeRetentionPolicyTag tag);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteExchangeRetentionPolicyTag(int itemId, int tagId);
        [WebMethod]
        [OperationContract]
        List<ExchangeMailboxPlanRetentionPolicyTag> GetExchangeMailboxPlanRetentionPolicyTags(int policyId);
        [WebMethod]
        [OperationContract]
        IntResult AddExchangeMailboxPlanRetentionPolicyTag(int itemId, ExchangeMailboxPlanRetentionPolicyTag planTag);
        [WebMethod]
        [OperationContract]
        ResultObject DeleteExchangeMailboxPlanRetentionPolicyTag(int itemID, int policyId, int planTagId);
        [WebMethod]
        [OperationContract]
        int CreatePublicFolder(int itemId, string parentFolder, string folderName, bool mailEnabled, string accountName, string domain);
        [WebMethod]
        [OperationContract]
        int DeletePublicFolders(int itemId, int[] accountIds);
        [WebMethod]
        [OperationContract]
        int DeletePublicFolder(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int EnableMailPublicFolder(int itemId, int accountId, string name, string domain);
        [WebMethod]
        [OperationContract]
        int DisableMailPublicFolder(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        ExchangePublicFolder GetPublicFolderGeneralSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetPublicFolderGeneralSettings(int itemId, int accountId, string newName, bool hideAddressBook, ExchangeAccount[] accounts);
        [WebMethod]
        [OperationContract]
        ExchangePublicFolder GetPublicFolderMailFlowSettings(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int SetPublicFolderMailFlowSettings(int itemId, int accountId, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [WebMethod]
        [OperationContract]
        ExchangeEmailAddress[] GetPublicFolderEmailAddresses(int itemId, int accountId);
        [WebMethod]
        [OperationContract]
        int AddPublicFolderEmailAddress(int itemId, int accountId, string emailAddress);
        [WebMethod]
        [OperationContract]
        int SetPublicFolderPrimaryEmailAddress(int itemId, int accountId, string emailAddress);
        [WebMethod]
        [OperationContract]
        int DeletePublicFolderEmailAddresses(int itemId, int accountId, string[] emailAddresses);
        [WebMethod]
        [OperationContract]
        string SetDefaultPublicFolderMailbox(int itemId);
        [WebMethod]
        [OperationContract]
        int AddExchangeDisclaimer(int itemId, ExchangeDisclaimer disclaimer);
        [WebMethod]
        [OperationContract]
        int UpdateExchangeDisclaimer(int itemId, ExchangeDisclaimer disclaimer);
        [WebMethod]
        [OperationContract]
        int DeleteExchangeDisclaimer(int itemId, int exchangeDisclaimerId);
        [WebMethod]
        [OperationContract]
        ExchangeDisclaimer GetExchangeDisclaimer(int itemId, int exchangeDisclaimerId);
        [WebMethod]
        [OperationContract]
        List<ExchangeDisclaimer> GetExchangeDisclaimers(int itemId);
        [WebMethod]
        [OperationContract]
        int SetExchangeAccountDisclaimerId(int itemId, int AccountID, int ExchangeDisclaimerId);
        [WebMethod]
        [OperationContract]
        int GetExchangeAccountDisclaimerId(int itemId, int AccountID);
        [WebMethod]
        [OperationContract]
        ResultObject SetPicture(int itemId, int accountId, byte[] picture);
        [WebMethod]
        [OperationContract]
        BytesResult GetPicture(int itemId, int accountId);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class esExchangeServer : SolidCP.EnterpriseServer.esExchangeServer, IesExchangeServer
    {
    }
}
#endif