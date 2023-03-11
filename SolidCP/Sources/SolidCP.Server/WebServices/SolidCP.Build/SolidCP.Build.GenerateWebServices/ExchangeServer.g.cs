#if !Client
using System;
using System.ComponentModel;
using System.Web.Services;
//using System.Web.Services.Protocols;
using System.Collections.Generic;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Server.Utils;
using Microsoft.Web.Services3;
using SolidCP.Providers.Common;
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
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExchangeServer : SolidCP.Server.ExchangeServer, IExchangeServer
    {
        public new bool CheckAccountCredentials(string username, string password)
        {
            return base.CheckAccountCredentials(username, password);
        }

        public new Organization ExtendToExchangeOrganization(string organizationId, string securityGroup, bool IsConsumer)
        {
            return base.ExtendToExchangeOrganization(organizationId, securityGroup, IsConsumer);
        }

        public new string CreateMailEnableUser(string upn, string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain, ExchangeAccountType accountType, string mailboxDatabase, string offlineAddressBook, string addressBookPolicy, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool hideFromAddressBook, bool isConsumer, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning)
        {
            return base.CreateMailEnableUser(upn, organizationId, organizationDistinguishedName, securityGroup, organizationDomain, accountType, mailboxDatabase, offlineAddressBook, addressBookPolicy, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, hideFromAddressBook, isConsumer, enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning);
        }

        public new Organization CreateOrganizationOfflineAddressBook(string organizationId, string securityGroup, string oabVirtualDir)
        {
            return base.CreateOrganizationOfflineAddressBook(organizationId, securityGroup, oabVirtualDir);
        }

        public new void UpdateOrganizationOfflineAddressBook(string id)
        {
            base.UpdateOrganizationOfflineAddressBook(id);
        }

        public new string GetOABVirtualDirectory()
        {
            return base.GetOABVirtualDirectory();
        }

        public new Organization CreateOrganizationAddressBookPolicy(string organizationId, string gal, string addressBook, string roomList, string oab)
        {
            return base.CreateOrganizationAddressBookPolicy(organizationId, gal, addressBook, roomList, oab);
        }

        public new bool DeleteOrganization(string organizationId, string distinguishedName, string globalAddressList, string addressList, string roomList, string offlineAddressBook, string securityGroup, string addressBookPolicy, List<ExchangeDomainName> acceptedDomains)
        {
            return base.DeleteOrganization(organizationId, distinguishedName, globalAddressList, addressList, roomList, offlineAddressBook, securityGroup, addressBookPolicy, acceptedDomains);
        }

        public new void SetOrganizationStorageLimits(string organizationDistinguishedName, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays)
        {
            base.SetOrganizationStorageLimits(organizationDistinguishedName, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays);
        }

        public new ExchangeItemStatistics[] GetMailboxesStatistics(string organizationDistinguishedName)
        {
            return base.GetMailboxesStatistics(organizationDistinguishedName);
        }

        public new void AddAuthoritativeDomain(string domain)
        {
            base.AddAuthoritativeDomain(domain);
        }

        public new void ChangeAcceptedDomainType(string domain, ExchangeAcceptedDomainType domainType)
        {
            base.ChangeAcceptedDomainType(domain, domainType);
        }

        public new string[] GetAuthoritativeDomains()
        {
            return base.GetAuthoritativeDomains();
        }

        public new void DeleteAuthoritativeDomain(string domain)
        {
            base.DeleteAuthoritativeDomain(domain);
        }

        public new void DeleteMailbox(string accountName)
        {
            base.DeleteMailbox(accountName);
        }

        public new void DisableMailbox(string accountName)
        {
            base.DisableMailbox(accountName);
        }

        public new ExchangeMailboxAutoReplySettings GetMailboxAutoReplySettings(string accountName)
        {
            return base.GetMailboxAutoReplySettings(accountName);
        }

        public new void SetMailboxAutoReplySettings(string accountName, ExchangeMailboxAutoReplySettings settings)
        {
            base.SetMailboxAutoReplySettings(accountName, settings);
        }

        public new ExchangeMailbox GetMailboxGeneralSettings(string accountName)
        {
            return base.GetMailboxGeneralSettings(accountName);
        }

        public new void SetMailboxGeneralSettings(string accountName, bool hideFromAddressBook, bool disabled)
        {
            base.SetMailboxGeneralSettings(accountName, hideFromAddressBook, disabled);
        }

        public new ExchangeMailbox GetMailboxMailFlowSettings(string accountName)
        {
            return base.GetMailboxMailFlowSettings(accountName);
        }

        public new void SetMailboxMailFlowSettings(string accountName, bool enableForwarding, int saveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            base.SetMailboxMailFlowSettings(accountName, enableForwarding, saveSentItems, forwardingAccountName, forwardToBoth, sendOnBehalfAccounts, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public new ExchangeMailbox GetMailboxAdvancedSettings(string accountName)
        {
            return base.GetMailboxAdvancedSettings(accountName);
        }

        public new void SetMailboxAdvancedSettings(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg)
        {
            base.SetMailboxAdvancedSettings(organizationId, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning, litigationHoldUrl, litigationHoldMsg);
        }

        public new ExchangeEmailAddress[] GetMailboxEmailAddresses(string accountName)
        {
            return base.GetMailboxEmailAddresses(accountName);
        }

        public new void SetMailboxEmailAddresses(string accountName, string[] emailAddresses)
        {
            base.SetMailboxEmailAddresses(accountName, emailAddresses);
        }

        public new void SetMailboxPrimaryEmailAddress(string accountName, string emailAddress)
        {
            base.SetMailboxPrimaryEmailAddress(accountName, emailAddress);
        }

        public new void SetMailboxPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts, string[] onBehalfOfAccounts, ExchangeAccount[] calendarAccounts, ExchangeAccount[] contactAccounts)
        {
            base.SetMailboxPermissions(organizationId, accountName, sendAsAccounts, fullAccessAccounts, onBehalfOfAccounts, calendarAccounts, contactAccounts);
        }

        public new ExchangeMailbox GetMailboxPermissions(string organizationId, string accountName)
        {
            return base.GetMailboxPermissions(organizationId, accountName);
        }

        public new ExchangeMailboxStatistics GetMailboxStatistics(string accountName)
        {
            return base.GetMailboxStatistics(accountName);
        }

        public new string[] SetDefaultPublicFolderMailbox(string id, string organizationId, string organizationDistinguishedName)
        {
            return base.SetDefaultPublicFolderMailbox(id, organizationId, organizationDistinguishedName);
        }

        public new string CreateJournalRule(string journalEmail, string scope, string recipientEmail, bool enabled)
        {
            return base.CreateJournalRule(journalEmail, scope, recipientEmail, enabled);
        }

        public new ExchangeJournalRule GetJournalRule(string journalEmail)
        {
            return base.GetJournalRule(journalEmail);
        }

        public new void SetJournalRule(ExchangeJournalRule rule)
        {
            base.SetJournalRule(rule);
        }

        public new void RemoveJournalRule(string journalEmail)
        {
            base.RemoveJournalRule(journalEmail);
        }

        public new void CreateContact(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain)
        {
            base.CreateContact(organizationId, organizationDistinguishedName, contactDisplayName, contactAccountName, contactEmail, defaultOrganizationDomain);
        }

        public new void DeleteContact(string accountName)
        {
            base.DeleteContact(accountName);
        }

        public new ExchangeContact GetContactGeneralSettings(string accountName)
        {
            return base.GetContactGeneralSettings(accountName);
        }

        public new void SetContactGeneralSettings(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain)
        {
            base.SetContactGeneralSettings(accountName, displayName, email, hideFromAddressBook, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, useMapiRichTextFormat, defaultDomain);
        }

        public new ExchangeContact GetContactMailFlowSettings(string accountName)
        {
            return base.GetContactMailFlowSettings(accountName);
        }

        public new void SetContactMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            base.SetContactMailFlowSettings(accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public new void CreateDistributionList(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy, string[] addressLists)
        {
            base.CreateDistributionList(organizationId, organizationDistinguishedName, displayName, accountName, name, domain, managedBy, addressLists);
        }

        public new void DeleteDistributionList(string accountName)
        {
            base.DeleteDistributionList(accountName);
        }

        public new ExchangeDistributionList GetDistributionListGeneralSettings(string accountName)
        {
            return base.GetDistributionListGeneralSettings(accountName);
        }

        public new void SetDistributionListGeneralSettings(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] members, string notes, string[] addressLists)
        {
            base.SetDistributionListGeneralSettings(accountName, displayName, hideFromAddressBook, managedBy, members, notes, addressLists);
        }

        public new ExchangeDistributionList GetDistributionListMailFlowSettings(string accountName)
        {
            return base.GetDistributionListMailFlowSettings(accountName);
        }

        public new void SetDistributionListMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication, string[] addressLists)
        {
            base.SetDistributionListMailFlowSettings(accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication, addressLists);
        }

        public new ExchangeEmailAddress[] GetDistributionListEmailAddresses(string accountName)
        {
            return base.GetDistributionListEmailAddresses(accountName);
        }

        public new void SetDistributionListEmailAddresses(string accountName, string[] emailAddresses, string[] addressLists)
        {
            base.SetDistributionListEmailAddresses(accountName, emailAddresses, addressLists);
        }

        public new void SetDistributionListPrimaryEmailAddress(string accountName, string emailAddress, string[] addressLists)
        {
            base.SetDistributionListPrimaryEmailAddress(accountName, emailAddress, addressLists);
        }

        public new void SetDistributionListPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts, string[] addressLists)
        {
            base.SetDistributionListPermissions(organizationId, accountName, sendAsAccounts, sendOnBehalfAccounts, addressLists);
        }

        public new ExchangeDistributionList GetDistributionListPermissions(string organizationId, string accountName)
        {
            return base.GetDistributionListPermissions(organizationId, accountName);
        }

        public new int SetDisclaimer(string name, string text)
        {
            return base.SetDisclaimer(name, text);
        }

        public new int RemoveDisclaimer(string name)
        {
            return base.RemoveDisclaimer(name);
        }

        public new int AddDisclamerMember(string name, string member)
        {
            return base.AddDisclamerMember(name, member);
        }

        public new int RemoveDisclamerMember(string name, string member)
        {
            return base.RemoveDisclamerMember(name, member);
        }

        public new void CreatePublicFolder(string organizationDistinguishedName, string organizationId, string securityGroup, string parentFolder, string folderName, bool mailEnabled, string accountName, string name, string domain)
        {
            base.CreatePublicFolder(organizationDistinguishedName, organizationId, securityGroup, parentFolder, folderName, mailEnabled, accountName, name, domain);
        }

        public new void DeletePublicFolder(string organizationId, string folder)
        {
            base.DeletePublicFolder(organizationId, folder);
        }

        public new void EnableMailPublicFolder(string organizationId, string folder, string accountName, string name, string domain)
        {
            base.EnableMailPublicFolder(organizationId, folder, accountName, name, domain);
        }

        public new void DisableMailPublicFolder(string organizationId, string folder)
        {
            base.DisableMailPublicFolder(organizationId, folder);
        }

        public new ExchangePublicFolder GetPublicFolderGeneralSettings(string organizationId, string folder)
        {
            return base.GetPublicFolderGeneralSettings(organizationId, folder);
        }

        public new void SetPublicFolderGeneralSettings(string organizationId, string folder, string newFolderName, bool hideFromAddressBook, ExchangeAccount[] accounts)
        {
            base.SetPublicFolderGeneralSettings(organizationId, folder, newFolderName, hideFromAddressBook, accounts);
        }

        public new ExchangePublicFolder GetPublicFolderMailFlowSettings(string organizationId, string folder)
        {
            return base.GetPublicFolderMailFlowSettings(organizationId, folder);
        }

        public new void SetPublicFolderMailFlowSettings(string organizationId, string folder, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            base.SetPublicFolderMailFlowSettings(organizationId, folder, acceptAccounts, rejectAccounts, requireSenderAuthentication);
        }

        public new ExchangeEmailAddress[] GetPublicFolderEmailAddresses(string organizationId, string folder)
        {
            return base.GetPublicFolderEmailAddresses(organizationId, folder);
        }

        public new void SetPublicFolderEmailAddresses(string organizationId, string folder, string[] emailAddresses)
        {
            base.SetPublicFolderEmailAddresses(organizationId, folder, emailAddresses);
        }

        public new void SetPublicFolderPrimaryEmailAddress(string organizationId, string folder, string emailAddress)
        {
            base.SetPublicFolderPrimaryEmailAddress(organizationId, folder, emailAddress);
        }

        public new ExchangeItemStatistics[] GetPublicFoldersStatistics(string organizationId, string[] folders)
        {
            return base.GetPublicFoldersStatistics(organizationId, folders);
        }

        public new string[] GetPublicFoldersRecursive(string organizationId, string parent)
        {
            return base.GetPublicFoldersRecursive(organizationId, parent);
        }

        public new long GetPublicFolderSize(string organizationId, string folder)
        {
            return base.GetPublicFolderSize(organizationId, folder);
        }

        public new string CreateOrganizationRootPublicFolder(string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain)
        {
            return base.CreateOrganizationRootPublicFolder(organizationId, organizationDistinguishedName, securityGroup, organizationDomain);
        }

        public new void CreateOrganizationActiveSyncPolicy(string organizationId)
        {
            base.CreateOrganizationActiveSyncPolicy(organizationId);
        }

        public new ExchangeActiveSyncPolicy GetActiveSyncPolicy(string organizationId)
        {
            return base.GetActiveSyncPolicy(organizationId);
        }

        public new void SetActiveSyncPolicy(string id, bool allowNonProvisionableDevices, bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInterval)
        {
            base.SetActiveSyncPolicy(id, allowNonProvisionableDevices, attachmentsEnabled, maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired, passwordRecoveryEnabled, deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts, minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInterval);
        }

        public new ExchangeMobileDevice[] GetMobileDevices(string accountName)
        {
            return base.GetMobileDevices(accountName);
        }

        public new ExchangeMobileDevice GetMobileDevice(string id)
        {
            return base.GetMobileDevice(id);
        }

        public new void WipeDataFromDevice(string id)
        {
            base.WipeDataFromDevice(id);
        }

        public new void CancelRemoteWipeRequest(string id)
        {
            base.CancelRemoteWipeRequest(id);
        }

        public new void RemoveDevice(string id)
        {
            base.RemoveDevice(id);
        }

        public new ResultObject ExportMailBox(string organizationId, string accountName, string storagePath)
        {
            return base.ExportMailBox(organizationId, accountName, storagePath);
        }

        public new ResultObject SetMailBoxArchiving(string organizationId, string accountName, bool archive, long archiveQuotaKB, long archiveWarningQuotaKB, string RetentionPolicy)
        {
            return base.SetMailBoxArchiving(organizationId, accountName, archive, archiveQuotaKB, archiveWarningQuotaKB, RetentionPolicy);
        }

        public new ResultObject SetRetentionPolicyTag(string Identity, ExchangeRetentionPolicyTagType Type, int AgeLimitForRetention, ExchangeRetentionPolicyTagAction RetentionAction)
        {
            return base.SetRetentionPolicyTag(Identity, Type, AgeLimitForRetention, RetentionAction);
        }

        public new ResultObject RemoveRetentionPolicyTag(string Identity)
        {
            return base.RemoveRetentionPolicyTag(Identity);
        }

        public new ResultObject SetRetentionPolicy(string Identity, string[] RetentionPolicyTagLinks)
        {
            return base.SetRetentionPolicy(Identity, RetentionPolicyTagLinks);
        }

        public new ResultObject RemoveRetentionPolicy(string Identity)
        {
            return base.RemoveRetentionPolicy(Identity);
        }

        public new ResultObject SetPicture(string accountName, byte[] picture)
        {
            return base.SetPicture(accountName, picture);
        }

        public new BytesResult GetPicture(string accountName)
        {
            return base.GetPicture(accountName);
        }
    }
}
#endif