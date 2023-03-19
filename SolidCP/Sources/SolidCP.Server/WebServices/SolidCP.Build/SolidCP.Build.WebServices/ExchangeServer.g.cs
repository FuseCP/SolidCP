#if !Client
using System;
using System.ComponentModel;
using SolidCP.Web.Services;
using System.Collections.Generic;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Server.Utils;
using SolidCP.Providers.Common;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IExchangeServer
    {
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool CheckAccountCredentials(string username, string password);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        Organization ExtendToExchangeOrganization(string organizationId, string securityGroup, bool IsConsumer);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string CreateMailEnableUser(string upn, string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain, ExchangeAccountType accountType, string mailboxDatabase, string offlineAddressBook, string addressBookPolicy, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool hideFromAddressBook, bool isConsumer, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        Organization CreateOrganizationOfflineAddressBook(string organizationId, string securityGroup, string oabVirtualDir);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateOrganizationOfflineAddressBook(string id);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string GetOABVirtualDirectory();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        Organization CreateOrganizationAddressBookPolicy(string organizationId, string gal, string addressBook, string roomList, string oab);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool DeleteOrganization(string organizationId, string distinguishedName, string globalAddressList, string addressList, string roomList, string offlineAddressBook, string securityGroup, string addressBookPolicy, List<ExchangeDomainName> acceptedDomains);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetOrganizationStorageLimits(string organizationDistinguishedName, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeItemStatistics[] GetMailboxesStatistics(string organizationDistinguishedName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void AddAuthoritativeDomain(string domain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void ChangeAcceptedDomainType(string domain, ExchangeAcceptedDomainType domainType);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetAuthoritativeDomains();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteAuthoritativeDomain(string domain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteMailbox(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DisableMailbox(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeMailboxAutoReplySettings GetMailboxAutoReplySettings(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetMailboxAutoReplySettings(string accountName, ExchangeMailboxAutoReplySettings settings);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeMailbox GetMailboxGeneralSettings(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetMailboxGeneralSettings(string accountName, bool hideFromAddressBook, bool disabled);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeMailbox GetMailboxMailFlowSettings(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetMailboxMailFlowSettings(string accountName, bool enableForwarding, int saveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeMailbox GetMailboxAdvancedSettings(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetMailboxAdvancedSettings(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeEmailAddress[] GetMailboxEmailAddresses(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetMailboxEmailAddresses(string accountName, string[] emailAddresses);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetMailboxPrimaryEmailAddress(string accountName, string emailAddress);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetMailboxPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts, string[] onBehalfOfAccounts, ExchangeAccount[] calendarAccounts, ExchangeAccount[] contactAccounts);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeMailbox GetMailboxPermissions(string organizationId, string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeMailboxStatistics GetMailboxStatistics(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] SetDefaultPublicFolderMailbox(string id, string organizationId, string organizationDistinguishedName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string CreateJournalRule(string journalEmail, string scope, string recipientEmail, bool enabled);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeJournalRule GetJournalRule(string journalEmail);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetJournalRule(ExchangeJournalRule rule);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RemoveJournalRule(string journalEmail);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateContact(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteContact(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeContact GetContactGeneralSettings(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetContactGeneralSettings(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeContact GetContactMailFlowSettings(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetContactMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateDistributionList(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy, string[] addressLists);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteDistributionList(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeDistributionList GetDistributionListGeneralSettings(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetDistributionListGeneralSettings(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] members, string notes, string[] addressLists);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeDistributionList GetDistributionListMailFlowSettings(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetDistributionListMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication, string[] addressLists);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeEmailAddress[] GetDistributionListEmailAddresses(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetDistributionListEmailAddresses(string accountName, string[] emailAddresses, string[] addressLists);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetDistributionListPrimaryEmailAddress(string accountName, string emailAddress, string[] addressLists);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetDistributionListPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts, string[] addressLists);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeDistributionList GetDistributionListPermissions(string organizationId, string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int SetDisclaimer(string name, string text);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int RemoveDisclaimer(string name);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int AddDisclamerMember(string name, string member);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        int RemoveDisclamerMember(string name, string member);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreatePublicFolder(string organizationDistinguishedName, string organizationId, string securityGroup, string parentFolder, string folderName, bool mailEnabled, string accountName, string name, string domain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeletePublicFolder(string organizationId, string folder);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void EnableMailPublicFolder(string organizationId, string folder, string accountName, string name, string domain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DisableMailPublicFolder(string organizationId, string folder);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangePublicFolder GetPublicFolderGeneralSettings(string organizationId, string folder);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetPublicFolderGeneralSettings(string organizationId, string folder, string newFolderName, bool hideFromAddressBook, ExchangeAccount[] accounts);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangePublicFolder GetPublicFolderMailFlowSettings(string organizationId, string folder);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetPublicFolderMailFlowSettings(string organizationId, string folder, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeEmailAddress[] GetPublicFolderEmailAddresses(string organizationId, string folder);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetPublicFolderEmailAddresses(string organizationId, string folder, string[] emailAddresses);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetPublicFolderPrimaryEmailAddress(string organizationId, string folder, string emailAddress);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeItemStatistics[] GetPublicFoldersStatistics(string organizationId, string[] folders);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetPublicFoldersRecursive(string organizationId, string parent);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        long GetPublicFolderSize(string organizationId, string folder);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string CreateOrganizationRootPublicFolder(string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CreateOrganizationActiveSyncPolicy(string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeActiveSyncPolicy GetActiveSyncPolicy(string organizationId);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void SetActiveSyncPolicy(string id, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInterval);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeMobileDevice[] GetMobileDevices(string accountName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ExchangeMobileDevice GetMobileDevice(string id);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void WipeDataFromDevice(string id);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void CancelRemoteWipeRequest(string id);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void RemoveDevice(string id);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject ExportMailBox(string organizationId, string accountName, string storagePath);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject SetMailBoxArchiving(string organizationId, string accountName, bool archive, long archiveQuotaKB, long archiveWarningQuotaKB, string RetentionPolicy);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject SetRetentionPolicyTag(string Identity, ExchangeRetentionPolicyTagType Type, int AgeLimitForRetention, ExchangeRetentionPolicyTagAction RetentionAction);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject RemoveRetentionPolicyTag(string Identity);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject SetRetentionPolicy(string Identity, string[] RetentionPolicyTagLinks);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject RemoveRetentionPolicy(string Identity);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        ResultObject SetPicture(string accountName, byte[] picture);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        BytesResult GetPicture(string accountName);
    }

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class ExchangeServer : SolidCP.Server.ExchangeServer, IExchangeServer
    {
    }
}
#endif