// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System.Collections.Generic;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Providers.HostedSolution
{
	public interface IExchangeServer
	{

        bool CheckAccountCredentials(string username, string password);
        // Organizations

        string CreateMailEnableUser(string upn, string organizationId, string organizationDistinguishedName,
                                    string securityGroup, string organizationDomain,
                                    ExchangeAccountType accountType,
                                    string mailboxDatabase, string offlineAddressBook, string addressBookPolicy,
                                    string accountName, bool enablePOP, bool enableIMAP,
                                    bool enableOWA, bool enableMAPI, bool enableActiveSync,
                                    long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB,
                                    int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool hideFromAddressBook, bool isConsumer, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning);

        Organization ExtendToExchangeOrganization(string organizationId, string securityGroup, bool IsConsumer);
        string GetOABVirtualDirectory();
        Organization CreateOrganizationOfflineAddressBook(string organizationId, string securityGroup, string oabVirtualDir);
        Organization CreateOrganizationAddressBookPolicy(string organizationId, string gal, string addressBook, string roomList, string oab);
        void UpdateOrganizationOfflineAddressBook(string id);
        bool DeleteOrganization(string organizationId, string distinguishedName, string globalAddressList, string addressList, string roomList, string offlineAddressBook, string securityGroup, string addressBookPolicy, List<ExchangeDomainName> acceptedDomains);
        void SetOrganizationStorageLimits(string organizationDistinguishedName, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays);
        ExchangeItemStatistics[] GetMailboxesStatistics(string organizationDistinguishedName);

        // Domains
        void AddAuthoritativeDomain(string domain);
        void DeleteAuthoritativeDomain(string domain);
        void ChangeAcceptedDomainType(string domain, ExchangeAcceptedDomainType domainType);
        string[] GetAuthoritativeDomains();

        // Mailboxes
        void DeleteMailbox(string accountName);
        void DisableMailbox(string id);
        ExchangeMailboxAutoReplySettings GetMailboxAutoReplySettings(string accountName);
        void SetMailboxAutoReplySettings(string accountName, ExchangeMailboxAutoReplySettings settings);
        ExchangeMailbox GetMailboxGeneralSettings(string accountName);
        void SetMailboxGeneralSettings(string accountName, bool hideFromAddressBook, bool disabled);
        ExchangeMailbox GetMailboxMailFlowSettings(string accountName);
        void SetMailboxMailFlowSettings(string accountName, bool enableForwarding, int saveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        ExchangeMailbox GetMailboxAdvancedSettings(string accountName);
        void SetMailboxAdvancedSettings(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg);
        ExchangeEmailAddress[] GetMailboxEmailAddresses(string accountName);
        void SetMailboxEmailAddresses(string accountName, string[] emailAddresses);
        void SetMailboxPrimaryEmailAddress(string accountName, string emailAddress);
        void SetMailboxPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts, string[] onBehalfOfAccounts, ExchangeAccount[] calendarAccounts, ExchangeAccount[] contactAccounts);
        ExchangeMailbox GetMailboxPermissions(string organizationId, string accountName);
        ExchangeMailboxStatistics GetMailboxStatistics(string accountName);
        string[] SetDefaultPublicFolderMailbox(string id, string organizationId, string organizationDistinguishedName);
        string CreateJournalRule(string journalEmail, string scope, string recipientEmail, bool enabled);
        ExchangeJournalRule GetJournalRule(string journalEmail);
        void SetJournalRule(ExchangeJournalRule rule);
        void RemoveJournalRule(string journalEmail);


        // Contacts
        void CreateContact(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain);
        void DeleteContact(string accountName);
        ExchangeContact GetContactGeneralSettings(string accountName);
        void SetContactGeneralSettings(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain);
        ExchangeContact GetContactMailFlowSettings(string accountName);
        void SetContactMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);

        // Distribution Lists
        void CreateDistributionList(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy, string[] addressLists);
        void DeleteDistributionList(string accountName);
        ExchangeDistributionList GetDistributionListGeneralSettings(string accountName);
        void SetDistributionListGeneralSettings(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] memebers, string notes, string[] addressLists);
        void AddDistributionListMembers(string accountName, string[] memberAccounts, string[] addressLists);
        void RemoveDistributionListMembers(string accountName, string[] memberAccounts, string[] addressLists);
        ExchangeDistributionList GetDistributionListMailFlowSettings(string accountName);
        void SetDistributionListMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication, string[] addressLists);
        ExchangeEmailAddress[] GetDistributionListEmailAddresses(string accountName);
        void SetDistributionListEmailAddresses(string accountName, string[] emailAddresses, string[] addressLists);
        void SetDistributionListPrimaryEmailAddress(string accountName, string emailAddress, string[] addressLists);
        ExchangeDistributionList GetDistributionListPermissions(string organizationId, string accountName);
        void SetDistributionListPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts, string[] addressLists);

		// Public Folders
		void CreatePublicFolder(string organizationDistinguishedName, string organizationId, string securityGroup, string parentFolder, string folderName, bool mailEnabled, string accountName, string name, string domain);
        void DeletePublicFolder(string organizationId, string folder);
		void EnableMailPublicFolder(string organizationId, string folder, string accountName, string name, string domain);
        void DisableMailPublicFolder(string organizationId, string folder);
        ExchangePublicFolder GetPublicFolderGeneralSettings(string organizationId, string folder);
        void SetPublicFolderGeneralSettings(string organizationId, string folder, string newFolderName, bool hideFromAddressBook, ExchangeAccount[] accounts);
        ExchangePublicFolder GetPublicFolderMailFlowSettings(string organizationId, string folder);
        void SetPublicFolderMailFlowSettings(string organizationId, string folder, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
        ExchangeEmailAddress[] GetPublicFolderEmailAddresses(string organizationId, string folder);
        void SetPublicFolderEmailAddresses(string organizationId, string folder, string[] emailAddresses);
        void SetPublicFolderPrimaryEmailAddress(string organizationId, string folder, string emailAddress);
        ExchangeItemStatistics[] GetPublicFoldersStatistics(string organizationId, string[] folders);
        string[] GetPublicFoldersRecursive(string organizationId, string parent);
        long GetPublicFolderSize(string organizationId, string folder);
        string CreateOrganizationRootPublicFolder(string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain);


        //ActiveSync
        void CreateOrganizationActiveSyncPolicy(string organizationId);
        ExchangeActiveSyncPolicy GetActiveSyncPolicy(string organizationId);
        void SetActiveSyncPolicy(string organizationId, bool allowNonProvisionableDevices, bool attachmentsEnabled,
            int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled,
            bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled,
            bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin,
            int passwordExpirationDays, int passwordHistory, int refreshInterval);

        //Mobile Devices
        ExchangeMobileDevice[] GetMobileDevices(string accountName);
        ExchangeMobileDevice GetMobileDevice(string id);
        void WipeDataFromDevice(string id);
        void CancelRemoteWipeRequest(string id);
        void RemoveDevice(string id);

        // Disclaimers
        int SetDisclaimer(string name, string text);
        int RemoveDisclaimer(string name);
        int AddDisclamerMember(string name, string member);
        int RemoveDisclamerMember(string name, string member);

        // Archiving
        ResultObject ExportMailBox(string organizationId, string accountName, string storagePath);
        ResultObject SetMailBoxArchiving(string organizationId, string accountName, bool archive, long archiveQuotaKB, long archiveWarningQuotaKB, string RetentionPolicy);

        // Retention policy
        ResultObject SetRetentionPolicyTag(string Identity, ExchangeRetentionPolicyTagType Type, int AgeLimitForRetention, ExchangeRetentionPolicyTagAction RetentionAction);
        ResultObject RemoveRetentionPolicyTag(string Identity);
        ResultObject SetRetentionPolicy(string Identity, string[] RetentionPolicyTagLinks);
        ResultObject RemoveRetentionPolicy(string Identity);

        // Picture
        ResultObject SetPicture(string accountName, byte[] picture);
        BytesResult GetPicture(string accountName);
    }
}
