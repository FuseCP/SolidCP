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

using System;
using System.ComponentModel;
using SolidCP.Web.Services;
using System.Collections.Generic;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Server.Utils;
using SolidCP.Providers.Common;

namespace SolidCP.Server
{
	/// <summary>
	/// Summary description for ExchangeServer
	/// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class ExchangeServer : HostingServiceProviderWebService
    {
        private IExchangeServer ES
        {
			get { return (IExchangeServer)Provider; }
		}
         
		[WebMethod, SoapHeader("settings")]
		public bool CheckAccountCredentials(string username, string password)
		{
			try
			{
				LogStart("CheckAccountCredentials");
				bool ret = ES.CheckAccountCredentials(username, password);
				LogEnd("CheckAccountCredentials");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("CheckAccountCredentials", ex);
				throw;
			}
		}

        #region Organizations

        [WebMethod, SoapHeader("settings")]
        public Organization ExtendToExchangeOrganization(string organizationId, string securityGroup, bool IsConsumer)
        {
            try
            {
                LogStart("ExtendToExchangeOrganization");
                Organization ret = ES.ExtendToExchangeOrganization(organizationId, securityGroup, IsConsumer);
                LogEnd("ExtendToExchangeOrganization");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("ExtendToExchangeOrganization", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string CreateMailEnableUser(string upn, string organizationId, string organizationDistinguishedName,
            string securityGroup, string organizationDomain,
            ExchangeAccountType accountType,
            string mailboxDatabase, string offlineAddressBook, string addressBookPolicy,
            string accountName, bool enablePOP, bool enableIMAP,
            bool enableOWA, bool enableMAPI, bool enableActiveSync,
            long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays,
            int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool hideFromAddressBook, bool isConsumer, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning)
        {
            try
            {
                LogStart("CreateMailEnableUser");
                string ret = ES.CreateMailEnableUser(upn, organizationId, organizationDistinguishedName, 
                                                           securityGroup, organizationDomain,
                                                           accountType,
                                                           mailboxDatabase, offlineAddressBook, addressBookPolicy,
                                                           accountName, enablePOP, enableIMAP,
                                                           enableOWA, enableMAPI, enableActiveSync,
                                                           issueWarningKB, prohibitSendKB, prohibitSendReceiveKB,
                                                           keepDeletedItemsDays,
                                                           maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, hideFromAddressBook, isConsumer, enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning);
                LogEnd("CreateMailEnableUser");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("ExtendToExchangeOrganization", ex);
                throw;
            }
        }

        /// <summary>
        /// Creates organization OAB
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="securityGroup"></param>
        /// <param name="oabVirtualDir"></param>
        /// <returns></returns>
        [WebMethod, SoapHeader("settings")]
        public Organization CreateOrganizationOfflineAddressBook(string organizationId, string securityGroup, string oabVirtualDir)
        {
            try
            {
                LogStart("CreateOrganizationOfflineAddressBook");
                Organization ret = ES.CreateOrganizationOfflineAddressBook(organizationId, securityGroup, oabVirtualDir);
                LogEnd("CreateOrganizationOfflineAddressBook");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("CreateOrganizationOfflineAddressBook", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates organization OAB
        /// </summary>
        /// <param name="id"></param>
        [WebMethod, SoapHeader("settings")]
        public void UpdateOrganizationOfflineAddressBook(string id)
        {
            try
            {
                LogStart("UpdateOrganizationOfflineAddressBook");
                ES.UpdateOrganizationOfflineAddressBook(id);
                LogEnd("UpdateOrganizationOfflineAddressBook");
            }
            catch (Exception ex)
            {
                LogError("UpdateOrganizationOfflineAddressBook", ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public string GetOABVirtualDirectory()
        {
            try
            {
                LogStart("GetOABVirtualDirectory");
                string ret = ES.GetOABVirtualDirectory();
                LogEnd("GetOABVirtualDirectory");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetOABVirtualDirectory", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public Organization CreateOrganizationAddressBookPolicy(string organizationId, string gal, string addressBook, string roomList, string oab)
        {
            try
            {
                LogStart("CCreateOrganizationAddressBookPolicy");
                Organization ret = ES.CreateOrganizationAddressBookPolicy(organizationId, gal, addressBook, roomList, oab);
                LogEnd("CreateOrganizationAddressBookPolicy");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("CreateOrganizationAddressBookPolicy", ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public bool DeleteOrganization(string organizationId, string distinguishedName, string globalAddressList, string addressList, string roomList, string offlineAddressBook, string securityGroup, string addressBookPolicy, List<ExchangeDomainName> acceptedDomains)
        {
            try
            {
                LogStart("DeleteOrganization");
                bool ret = ES.DeleteOrganization(organizationId, distinguishedName, globalAddressList, addressList, roomList, offlineAddressBook, securityGroup, addressBookPolicy, acceptedDomains);
                LogEnd("DeleteOrganization");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("DeleteOrganization", ex);
                throw;
            }
        }



        [WebMethod, SoapHeader("settings")]
        public void SetOrganizationStorageLimits(string organizationDistinguishedName, long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays)
        {
            try
            {
                LogStart("SetOrganizationStorageLimits");
                ES.SetOrganizationStorageLimits(organizationDistinguishedName, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays);
                LogEnd("SetOrganizationStorageLimits");
            }
            catch (Exception ex)
            {
                LogError("SetOrganizationStorageLimits", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeItemStatistics[] GetMailboxesStatistics(string organizationDistinguishedName)
        {
            try
            {
                LogStart("GetMailboxesStatistics");
                ExchangeItemStatistics[] ret = ES.GetMailboxesStatistics(organizationDistinguishedName);
                LogEnd("GetMailboxesStatistics");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetMailboxesStatistics", ex);
                throw;
            }
        }
        #endregion

        #region Domains
        [WebMethod, SoapHeader("settings")]
        public void AddAuthoritativeDomain(string domain)
        {
            try
            {
                LogStart("AddAuthoritativeDomain");
                ES.AddAuthoritativeDomain(domain);
                LogEnd("AddAuthoritativeDomain");
            }
            catch (Exception ex)
            {
                LogError("AddAuthoritativeDomain", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ChangeAcceptedDomainType(string domain, ExchangeAcceptedDomainType domainType)
        {
            try
            {
                LogStart("ChangeAcceptedDomainType");
                ES.ChangeAcceptedDomainType(domain, domainType);
                LogEnd("ChangeAcceptedDomainType");
            }
            catch (Exception ex)
            {
                LogError("ChangeAcceptedDomainType", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetAuthoritativeDomains()
        {
            try
            {
                LogStart("GetAuthoritativeDomains");
                string[] ret = ES.GetAuthoritativeDomains();
                LogEnd("GetAuthoritativeDomains");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetAuthoritativeDomain", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteAuthoritativeDomain(string domain)
        {
            try
            {
                LogStart("DeleteAuthoritativeDomain");
                ES.DeleteAuthoritativeDomain(domain);
                LogEnd("DeleteAuthoritativeDomain");
            }
            catch (Exception ex)
            {
                LogError("DeleteAuthoritativeDomain", ex);
                throw;
            }
        }
        #endregion

        #region Mailboxes
        [WebMethod, SoapHeader("settings")]
        public void DeleteMailbox(string accountName)
        {
            try
            {
                LogStart("DeleteMailbox");
                ES.DeleteMailbox(accountName);
                LogEnd("DeleteMailbox");
            }
            catch (Exception ex)
            {
                LogError("DeleteMailbox", ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public void DisableMailbox(string accountName)
        {
            try
            {
                LogStart("DisableMailbox");
                ES.DisableMailbox(accountName);
                LogEnd("DisableMailbox");
            }
            catch (Exception ex)
            {
                LogError("DisableMailbox", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeMailboxAutoReplySettings GetMailboxAutoReplySettings(string accountName)
        {
            try
            {
                LogStart("GetMailboxAutoReplySettings");
                ExchangeMailboxAutoReplySettings ret = ES.GetMailboxAutoReplySettings(accountName);
                LogEnd("GetMailboxAutoReplySettings");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetMailboxAutoReplySettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetMailboxAutoReplySettings(string accountName, ExchangeMailboxAutoReplySettings settings)
        {
            try
            {
                LogStart("SetMailboxAutoReplySettings");
                ES.SetMailboxAutoReplySettings(accountName, settings);
                LogEnd("SetMailboxAutoReplySettings");
            }
            catch (Exception ex)
            {
                LogError("SetMailboxAutoReplySettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeMailbox GetMailboxGeneralSettings(string accountName)
        {
            try
            {
                LogStart("GetMailboxGeneralSettings");
                ExchangeMailbox ret = ES.GetMailboxGeneralSettings(accountName);
                LogEnd("GetMailboxGeneralSettings");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetMailboxGeneralSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetMailboxGeneralSettings(string accountName, bool hideFromAddressBook, bool disabled)
        {
            try
            {
                LogStart("SetMailboxGeneralSettings");
                ES.SetMailboxGeneralSettings(accountName, hideFromAddressBook, disabled);
                LogEnd("SetMailboxGeneralSettings");
            }
            catch (Exception ex)
            {
                LogError("SetMailboxGeneralSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeMailbox GetMailboxMailFlowSettings(string accountName)
        {
            try
            {
                LogStart("GetMailboxMailFlowSettings");
                ExchangeMailbox ret = ES.GetMailboxMailFlowSettings(accountName);
                LogEnd("GetMailboxMailFlowSettings");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetMailboxMailFlowSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetMailboxMailFlowSettings(string accountName, bool enableForwarding, int saveSentItems, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            try
            {
                LogStart("SetMailboxMailFlowSettings");
                ES.SetMailboxMailFlowSettings(accountName, enableForwarding, saveSentItems, forwardingAccountName, forwardToBoth, sendOnBehalfAccounts, acceptAccounts, rejectAccounts, requireSenderAuthentication);
                LogEnd("SetMailboxMailFlowSettings");
            }
            catch (Exception ex)
            {
                LogError("SetMailboxMailFlowSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeMailbox GetMailboxAdvancedSettings(string accountName)
        {
            try
            {
                LogStart("GetMailboxAdvancedSettings");
                ExchangeMailbox ret = ES.GetMailboxAdvancedSettings(accountName);
                LogEnd("GetMailboxAdvancedSettings");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetMailboxAdvancedSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetMailboxAdvancedSettings(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync,
            long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB
            , bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning, string litigationHoldUrl, string litigationHoldMsg)
        {
            try
            {
                LogStart("SetMailboxAdvancedSettings");
                ES.SetMailboxAdvancedSettings(organizationId, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync,
                    issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB,
                    enabledLitigationHold, recoverabelItemsSpace, recoverabelItemsWarning, litigationHoldUrl, litigationHoldMsg);
                LogEnd("SetMailboxAdvancedSettings");
            }
            catch (Exception ex)
            {
                LogError("SetMailboxAdvancedSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeEmailAddress[] GetMailboxEmailAddresses(string accountName)
        {
            try
            {
                LogStart("GetMailboxEmailAddresses");
                ExchangeEmailAddress[] ret = ES.GetMailboxEmailAddresses(accountName);
                LogEnd("GetMailboxEmailAddresses");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetMailboxEmailAddresses", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetMailboxEmailAddresses(string accountName, string[] emailAddresses)
        {
            try
            {
                LogStart("SetMailboxEmailAddresses");
                ES.SetMailboxEmailAddresses(accountName, emailAddresses);
                LogEnd("SetMailboxEmailAddresses");
            }
            catch (Exception ex)
            {
                LogError("SetMailboxEmailAddresses", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetMailboxPrimaryEmailAddress(string accountName, string emailAddress)
        {
            try
            {
                LogStart("SetMailboxPrimaryEmailAddress");
                ES.SetMailboxPrimaryEmailAddress(accountName, emailAddress);
                LogEnd("SetMailboxPrimaryEmailAddress");
            }
            catch (Exception ex)
            {
                LogError("SetMailboxPrimaryEmailAddress", ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public void SetMailboxPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts, string[] onBehalfOfAccounts, ExchangeAccount[] calendarAccounts, ExchangeAccount[] contactAccounts)
        {
            try
            {
                LogStart("SetMailboxPermissions");
                ES.SetMailboxPermissions(organizationId, accountName, sendAsAccounts, fullAccessAccounts, onBehalfOfAccounts, calendarAccounts, contactAccounts);
                LogEnd("SetMailboxPermissions");
            }
            catch (Exception ex)
            {
                LogError("SetMailboxPermissions", ex);
                throw;
            }
        }



        [WebMethod, SoapHeader("settings")]
        public ExchangeMailbox GetMailboxPermissions(string organizationId, string accountName)
        {
            try
            {
                LogStart("GetMailboxPermissions");
                ExchangeMailbox ret = ES.GetMailboxPermissions(organizationId, accountName);
                LogEnd("GetMailboxPermissions");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetMailboxPermissions", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeMailboxStatistics GetMailboxStatistics(string accountName)
        {
            try
            {
                LogStart("GetMailboxStatistics");
                ExchangeMailboxStatistics ret = ES.GetMailboxStatistics(accountName);
                LogEnd("GetMailboxStatistics");
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError("GetMailboxStatistics", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] SetDefaultPublicFolderMailbox(string id, string organizationId, string organizationDistinguishedName)
        {
            try
            {
                LogStart("SetDefaultPublicFolderMailbox");
                string[] ret = ES.SetDefaultPublicFolderMailbox(id, organizationId, organizationDistinguishedName);
                LogEnd("SetDefaultPublicFolderMailbox");
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError("SetDefaultPublicFolderMailbox", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string CreateJournalRule(string journalEmail, string scope, string recipientEmail, bool enabled)
        {
            try
            {
                LogStart("CreateJournalRule");
                string ret = ES.CreateJournalRule(journalEmail, scope, recipientEmail, enabled);
                LogEnd("CreateJournalRule");
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError("CreateJournalRule", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeJournalRule GetJournalRule(string journalEmail)
        {
            try
            {
                LogStart("GetJournalRule");
                ExchangeJournalRule ret = ES.GetJournalRule(journalEmail);
                LogEnd("GetJournalRule");
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError("GetJournalRule", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetJournalRule(ExchangeJournalRule rule)
        {
            try
            {
                LogStart("SetJournalRule");
                ES.SetJournalRule(rule);
                LogEnd("SetJournalRule");
            }
            catch (Exception ex)
            {
                Log.WriteError("SetJournalRule", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void RemoveJournalRule(string journalEmail)
        {
            try
            {
                LogStart("RemoveJournalRule");
                ES.RemoveJournalRule(journalEmail);
                LogEnd("RemoveJournalRule");
            }
            catch (Exception ex)
            {
                Log.WriteError("RemoveJournalRule", ex);
            }
        }

        #endregion

        #region Contacts
        [WebMethod, SoapHeader("settings")]
        public void CreateContact(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain)
        {
            try
            {
                LogStart("CreateContact");
                ES.CreateContact(organizationId, organizationDistinguishedName, contactDisplayName, contactAccountName, contactEmail, defaultOrganizationDomain);
                LogEnd("CreateContact");
            }
            catch (Exception ex)
            {
                LogError("CreateContact", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteContact(string accountName)
        {
            try
            {
                LogStart("DeleteContact");
                ES.DeleteContact(accountName);
                LogEnd("DeleteContact");
            }
            catch (Exception ex)
            {
                LogError("DeleteContact", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeContact GetContactGeneralSettings(string accountName)
        {
            try
            {
                LogStart("GetContactGeneralSettings");
                ExchangeContact ret = ES.GetContactGeneralSettings(accountName);
                LogEnd("GetContactGeneralSettings");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetContactGeneralSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetContactGeneralSettings(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain)
        {
            try
            {
                LogStart("SetContactGeneralSettings");
                ES.SetContactGeneralSettings(accountName, displayName, email, hideFromAddressBook, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, useMapiRichTextFormat, defaultDomain);
                LogEnd("SetContactGeneralSettings");
            }
            catch (Exception ex)
            {
                LogError("SetContactGeneralSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeContact GetContactMailFlowSettings(string accountName)
        {
            try
            {
                LogStart("GetContactMailFlowSettings");
                ExchangeContact ret = ES.GetContactMailFlowSettings(accountName);
                LogEnd("GetContactMailFlowSettings");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetContactMailFlowSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetContactMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
        {
            try
            {
                LogStart("SetContactMailFlowSettings");
                ES.SetContactMailFlowSettings(accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication);
                LogEnd("SetContactMailFlowSettings");
            }
            catch (Exception ex)
            {
                LogError("SetContactMailFlowSettings", ex);
                throw;
            }
        }
        #endregion

        #region Distribution Lists
        [WebMethod, SoapHeader("settings")]
        public void CreateDistributionList(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy, string[] addressLists)
        {
            try
            {
                LogStart("CreateDistributionList");
                ES.CreateDistributionList(organizationId, organizationDistinguishedName, displayName, accountName, name, domain, managedBy, addressLists);
                LogEnd("CreateDistributionList");
            }
            catch (Exception ex)
            {
                LogError("CreateDistributionList", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteDistributionList(string accountName)
        {
            try
            {
                LogStart("DeleteDistributionList");
                ES.DeleteDistributionList(accountName);
                LogEnd("DeleteDistributionList");
            }
            catch (Exception ex)
            {
                LogError("DeleteDistributionList", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeDistributionList GetDistributionListGeneralSettings(string accountName)
        {
            try
            {
                LogStart("GetDistributionListGeneralSettings");
                ExchangeDistributionList ret = ES.GetDistributionListGeneralSettings(accountName);
                LogEnd("GetDistributionListGeneralSettings");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetDistributionListGeneralSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetDistributionListGeneralSettings(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] members, string notes, string[] addressLists)
        {
            try
            {
                LogStart("SetDistributionListGeneralSettings");
                ES.SetDistributionListGeneralSettings(accountName, displayName, hideFromAddressBook, managedBy, members, notes, addressLists);
                LogEnd("SetDistributionListGeneralSettings");
            }
            catch (Exception ex)
            {
                LogError("SetDistributionListGeneralSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeDistributionList GetDistributionListMailFlowSettings(string accountName)
        {
            try
            {
                LogStart("GetDistributionListMailFlowSettings");
                ExchangeDistributionList ret = ES.GetDistributionListMailFlowSettings(accountName);
                LogEnd("GetDistributionListMailFlowSettings");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetDistributionListMailFlowSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetDistributionListMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication, string[] addressLists)
        {
            try
            {
                LogStart("SetDistributionListMailFlowSettings");
                ES.SetDistributionListMailFlowSettings(accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication, addressLists);
                LogEnd("SetDistributionListMailFlowSettings");
            }
            catch (Exception ex)
            {
                LogError("SetDistributionListMailFlowSettings", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeEmailAddress[] GetDistributionListEmailAddresses(string accountName)
        {
            try
            {
                LogStart("GetDistributionListEmailAddresses");
                ExchangeEmailAddress[] ret = ES.GetDistributionListEmailAddresses(accountName);
                LogEnd("GetDistributionListEmailAddresses");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetDistributionListEmailAddresses", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetDistributionListEmailAddresses(string accountName, string[] emailAddresses, string[] addressLists)
        {
            try
            {
                LogStart("SetDistributionListEmailAddresses");
                ES.SetDistributionListEmailAddresses(accountName, emailAddresses, addressLists);
                LogEnd("SetDistributionListEmailAddresses");
            }
            catch (Exception ex)
            {
                LogError("SetDistributionListEmailAddresses", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetDistributionListPrimaryEmailAddress(string accountName, string emailAddress, string[] addressLists)
        {
            try
            {
                LogStart("SetDistributionListPrimaryEmailAddress");
                ES.SetDistributionListPrimaryEmailAddress(accountName, emailAddress, addressLists);
                LogEnd("SetDistributionListPrimaryEmailAddress");
            }
            catch (Exception ex)
            {
                LogError("SetDistributionListPrimaryEmailAddress", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetDistributionListPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts, string[] addressLists)
        {
            ES.SetDistributionListPermissions(organizationId, accountName, sendAsAccounts, sendOnBehalfAccounts, addressLists);
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeDistributionList GetDistributionListPermissions(string organizationId, string accountName)
        {
            return ES.GetDistributionListPermissions(organizationId, accountName);
        }

        #endregion

        #region Disclaimers

        [WebMethod, SoapHeader("settings")]
        public int SetDisclaimer(string name, string text)
        {
            return ES.SetDisclaimer(name, text);
        }

        [WebMethod, SoapHeader("settings")]
        public int RemoveDisclaimer(string name)
        {
            return ES.RemoveDisclaimer(name);
        }

        [WebMethod, SoapHeader("settings")]
        public int AddDisclamerMember(string name, string member)
        {
            return ES.AddDisclamerMember(name, member);
        }

        [WebMethod, SoapHeader("settings")]
        public int RemoveDisclamerMember(string name, string member)
        {
            return ES.RemoveDisclamerMember(name, member);
        }

        #endregion

        #region Public Folders
        [WebMethod, SoapHeader("settings")]
		public void CreatePublicFolder(string organizationDistinguishedName, string organizationId, string securityGroup, string parentFolder,
			string folderName, bool mailEnabled, string accountName, string name, string domain)
		{
			try
			{
				LogStart("CreatePublicFolder");
				ES.CreatePublicFolder(organizationDistinguishedName, organizationId, securityGroup, parentFolder, folderName,
					mailEnabled, accountName, name, domain);

				LogEnd("CreatePublicFolder");
			}
			catch (Exception ex)
			{
				LogError("CreatePublicFolder", ex);
				throw;
			}
		}
		
		[WebMethod, SoapHeader("settings")]
        public void DeletePublicFolder(string organizationId, string folder)
		{
			try
			{
				LogStart("DeletePublicFolder");
				ES.DeletePublicFolder(organizationId, folder);
				LogEnd("DeletePublicFolder");
			}
			catch (Exception ex)
			{
				LogError("DeletePublicFolder", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void EnableMailPublicFolder(string organizationId, string folder, string accountName,
			string name, string domain)
		{
			try
			{
				LogStart("EnableMailPublicFolder");
				ES.EnableMailPublicFolder(organizationId, folder, accountName, name, domain);
				LogEnd("EnableMailPublicFolder");
			}
			catch (Exception ex)
			{
				LogError("EnableMailPublicFolder", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public void DisableMailPublicFolder(string organizationId, string folder)
		{
			try
			{
				LogStart("DisableMailPublicFolder");
				ES.DisableMailPublicFolder(organizationId, folder);
				LogEnd("DisableMailPublicFolder");
			}
			catch (Exception ex)
			{
				LogError("DisableMailPublicFolder", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public ExchangePublicFolder GetPublicFolderGeneralSettings(string organizationId, string folder)
		{
			try
			{
				LogStart("GetPublicFolderGeneralSettings");
				ExchangePublicFolder ret = ES.GetPublicFolderGeneralSettings(organizationId, folder);
				LogEnd("GetPublicFolderGeneralSettings");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetPublicFolderGeneralSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public void SetPublicFolderGeneralSettings(string organizationId, string folder, string newFolderName,
			 bool hideFromAddressBook, ExchangeAccount[] accounts)
		{
			try
			{
				LogStart("SetPublicFolderGeneralSettings");
				ES.SetPublicFolderGeneralSettings(organizationId, folder, newFolderName, hideFromAddressBook,  accounts);
				LogEnd("SetPublicFolderGeneralSettings");
			}
			catch (Exception ex)
			{
				LogError("SetPublicFolderGeneralSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public ExchangePublicFolder GetPublicFolderMailFlowSettings(string organizationId, string folder)
		{
			try
			{
				LogStart("GetPublicFolderMailFlowSettings");
				ExchangePublicFolder ret = ES.GetPublicFolderMailFlowSettings(organizationId, folder);
				LogEnd("GetPublicFolderMailFlowSettings");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetPublicFolderMailFlowSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public void SetPublicFolderMailFlowSettings(string organizationId, string folder,
			string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
		{
			try
			{
				LogStart("SetPublicFolderMailFlowSettings");
				ES.SetPublicFolderMailFlowSettings(organizationId, folder, acceptAccounts, rejectAccounts, requireSenderAuthentication);
				LogEnd("SetPublicFolderMailFlowSettings");
			}
			catch (Exception ex)
			{
				LogError("SetPublicFolderMailFlowSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public ExchangeEmailAddress[] GetPublicFolderEmailAddresses(string organizationId, string folder)
		{
			try
			{
				LogStart("GetPublicFolderEmailAddresses");
				ExchangeEmailAddress[] ret = ES.GetPublicFolderEmailAddresses(organizationId, folder);
				LogEnd("GetPublicFolderEmailAddresses");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetPublicFolderEmailAddresses", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public void SetPublicFolderEmailAddresses(string organizationId, string folder, string[] emailAddresses)
		{
			try
			{
				LogStart("SetPublicFolderEmailAddresses");
				ES.SetPublicFolderEmailAddresses(organizationId, folder, emailAddresses);
				LogEnd("SetPublicFolderEmailAddresses");
			}
			catch (Exception ex)
			{
				LogError("SetPublicFolderEmailAddresses", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public void SetPublicFolderPrimaryEmailAddress(string organizationId, string folder, string emailAddress)
		{
			try
			{
				LogStart("SetPublicFolderPrimaryEmailAddress");
				ES.SetPublicFolderPrimaryEmailAddress(organizationId, folder, emailAddress);
				LogEnd("SetPublicFolderPrimaryEmailAddress");
			}
			catch (Exception ex)
			{
				LogError("SetPublicFolderPrimaryEmailAddress", ex);
				throw;
			}
		}
		
		[WebMethod, SoapHeader("settings")]
        public ExchangeItemStatistics[] GetPublicFoldersStatistics(string organizationId, string[] folders)
		{
			try
			{
				LogStart("GetPublicFoldersStatistics");
				ExchangeItemStatistics[] ret = ES.GetPublicFoldersStatistics(organizationId, folders);
				LogEnd("GetPublicFoldersStatistics");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetPublicFoldersStatistics", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public string[] GetPublicFoldersRecursive(string organizationId, string parent)
		{
			try
			{
				LogStart("GetPublicFoldersRecursive");
				string[] ret = ES.GetPublicFoldersRecursive(organizationId, parent);
				LogEnd("GetPublicFoldersRecursive");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetPublicFoldersRecursive", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public long GetPublicFolderSize(string organizationId, string folder)
		{
			try
			{
				LogStart("GetPublicFolderSize");
				long ret = ES.GetPublicFolderSize(organizationId, folder);
				LogEnd("GetPublicFolderSize");
				return ret;
			}
			catch (Exception ex)
			{
				Log.WriteError("GetPublicFolderSize", ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public string CreateOrganizationRootPublicFolder(string organizationId, string organizationDistinguishedName, string securityGroup, string organizationDomain)
        {
            try
            {
                LogStart("CreateOrganizationRootPublicFolder");
                string ret = ES.CreateOrganizationRootPublicFolder(organizationId, organizationDistinguishedName, securityGroup, organizationDomain);
                LogEnd("CreateOrganizationRootPublicFolder");
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError("CreateOrganizationRootPublicFolder", ex);
                throw;
            }
        }
		
        
        #endregion

        #region ActiveSync
        [WebMethod, SoapHeader("settings")]
        public void CreateOrganizationActiveSyncPolicy(string organizationId)
        {
            try
            {
                LogStart("CreateOrganizationActiveSyncPolicy");
                ES.CreateOrganizationActiveSyncPolicy(organizationId);
                LogEnd("CreateOrganizationActiveSyncPolicy");
            }
            catch (Exception ex)
            {
                LogError("CreateOrganizationActiveSyncPolicy", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeActiveSyncPolicy GetActiveSyncPolicy(string organizationId)
        {
            try
            {
                LogStart("GetActiveSyncPolicy");
                ExchangeActiveSyncPolicy ret = ES.GetActiveSyncPolicy(organizationId);
                LogEnd("GetActiveSyncPolicy");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetActiveSyncPolicy", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetActiveSyncPolicy(string id, bool allowNonProvisionableDevices, bool attachmentsEnabled,
            int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled,
            bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled,
            bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin,
            int passwordExpirationDays, int passwordHistory, int refreshInterval)
        {
            try
            {
                LogStart("SetActiveSyncPolicy");
                ES.SetActiveSyncPolicy(id, allowNonProvisionableDevices, attachmentsEnabled,
                    maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired, passwordRecoveryEnabled,
                    deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts,
                    minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInterval);
                LogEnd("SetActiveSyncPolicy");
            }
            catch (Exception ex)
            {
                LogError("SetActiveSyncPolicy", ex);
                throw;
            }
        }
        #endregion

        #region Mobile devices
        [WebMethod, SoapHeader("settings")]
        public ExchangeMobileDevice[] GetMobileDevices(string accountName)
        {
            try
            {
                LogStart("GetMobileDevices");
                ExchangeMobileDevice[] ret = ES.GetMobileDevices(accountName);
                LogEnd("GetMobileDevices");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetMobileDevices", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeMobileDevice GetMobileDevice(string id)
        {
            try
            {
                LogStart("GetMobileDevice");
                ExchangeMobileDevice ret = ES.GetMobileDevice(id);
                LogEnd("GetMobileDevice");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetMobileDevice", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void WipeDataFromDevice(string id)
        {
            try
            {
                LogStart("WipeDataFromDevice");
                ES.WipeDataFromDevice(id);
                LogEnd("WipeDataFromDevice");
            }
            catch (Exception ex)
            {
                LogError("WipeDataFromDevice", ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CancelRemoteWipeRequest(string id)
        {
            try
            {
                LogStart("CancelRemoteWipeRequest");
                ES.CancelRemoteWipeRequest(id);
                LogEnd("CancelRemoteWipeRequest");
            }
            catch (Exception ex)
            {
                LogError("CancelRemoteWipeRequest", ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public void RemoveDevice(string id)
        {
            try
            {
                LogStart("RemoveDevice");
                ES.RemoveDevice(id);
                LogEnd("RemoveDevice");
            }
            catch (Exception ex)
            {
                LogError("RemoveDevice", ex);
                throw;
            }
        }
        #endregion

        #region Archiving

        [WebMethod, SoapHeader("settings")]
        public ResultObject ExportMailBox(string organizationId, string accountName, string storagePath)
        {
            ResultObject res = null;
            try
            {
                LogStart("ExportMailBox");

                res = ES.ExportMailBox(organizationId, accountName, storagePath);
                
                LogEnd("ExportMailBox");
            }
            catch (Exception ex)
            {
                LogError("ExportMailBox", ex);
                throw;
            }

            return res;
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject SetMailBoxArchiving(string organizationId, string accountName, bool archive, long archiveQuotaKB, long archiveWarningQuotaKB, string RetentionPolicy)
        {
            ResultObject res = null;
            try
            {
                LogStart("SetMailBoxArchiving");
                res = ES.SetMailBoxArchiving(organizationId, accountName, archive, archiveQuotaKB, archiveWarningQuotaKB, RetentionPolicy);
                LogEnd("SetMailBoxArchiving");
            }
            catch (Exception ex)
            {
                LogError("SetMailBoxArchiving", ex);
                throw;
            }

            return res;
        }

        #endregion

        #region Retention policy
        [WebMethod, SoapHeader("settings")]
        public ResultObject SetRetentionPolicyTag(string Identity, ExchangeRetentionPolicyTagType Type, int AgeLimitForRetention, ExchangeRetentionPolicyTagAction RetentionAction)
        {
            ResultObject res = null;
            try
            {
                LogStart("SetRetentionPolicyTag");
                res = ES.SetRetentionPolicyTag(Identity, Type, AgeLimitForRetention, RetentionAction);
                LogEnd("SetRetentionPolicyTag");
            }
            catch (Exception ex)
            {
                LogError("SetRetentionPolicyTag", ex);
                throw;
            }
            return res;
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject RemoveRetentionPolicyTag(string Identity)
        {
            ResultObject res = null;
            try
            {
                LogStart("RemoveRetentionPolicyTag");
                res = ES.RemoveRetentionPolicyTag(Identity);
                LogEnd("RemoveRetentionPolicyTag");
            }
            catch (Exception ex)
            {
                LogError("RemoveRetentionPolicyTag", ex);
                throw;
            }
            return res;
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject SetRetentionPolicy(string Identity, string[] RetentionPolicyTagLinks)
        {
            ResultObject res = null;
            try
            {
                LogStart("SetRetentionPolicy");
                res = ES.SetRetentionPolicy(Identity, RetentionPolicyTagLinks);
                LogEnd("SetRetentionPolicy");
            }
            catch (Exception ex)
            {
                LogError("SetRetentionPolicy", ex);
                throw;
            }
            return res;
        }

        [WebMethod, SoapHeader("settings")]
        public ResultObject RemoveRetentionPolicy(string Identity)
        {
            ResultObject res = null;
            try
            {
                LogStart("RemoveRetentionPolicy");
                res = ES.RemoveRetentionPolicy(Identity);
                LogEnd("RemoveRetentionPolicy");
            }
            catch (Exception ex)
            {
                LogError("RemoveRetentionPolicy", ex);
                throw;
            }
            return res;
        }

        #endregion

        #region Picture

        [WebMethod, SoapHeader("settings")]
        public ResultObject SetPicture(string accountName, byte[] picture)
        {
            ResultObject res = null;
            try
            {
                LogStart("SetPicture");
                res = ES.SetPicture(accountName, picture);
                LogEnd("SetPicture");
            }
            catch (Exception ex)
            {
                LogError("SetPicture", ex);
                throw;
            }
            return res;
        }

        [WebMethod, SoapHeader("settings")]
        public BytesResult GetPicture(string accountName)
        {
            BytesResult res = null;
            try
            {
                LogStart("GetPicture");
                res = ES.GetPicture(accountName);
                LogEnd("SetPicture");
            }
            catch (Exception ex)
            {
                LogError("GetPicture", ex);
                throw;
            }
            return res;
        }


        #endregion
        protected void LogStart(string func)
		{
			Log.WriteStart("'{0}' {1}", ProviderSettings.ProviderName, func);
		}

		protected void LogEnd(string func)
		{
			Log.WriteEnd("'{0}' {1}", ProviderSettings.ProviderName, func);
		}

		protected void LogError(string func, Exception ex)
		{
			Log.WriteError(String.Format("'{0}' {1}", ProviderSettings.ProviderName, func), ex);
		}
 
	}
}
