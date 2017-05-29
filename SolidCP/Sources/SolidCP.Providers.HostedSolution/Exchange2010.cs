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
using System.Collections;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;

using System.DirectoryServices;
using System.Security;
using System.Security.Principal;
using System.Security.AccessControl;

using System.Management.Automation;
using System.Management.Automation.Runspaces;

using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;
using Microsoft.Exchange.Data.Directory.Recipient;
using Microsoft.Win32;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;

using Microsoft.Exchange.Data;
using Microsoft.Exchange.Data.Directory;
using Microsoft.Exchange.Data.Storage;

namespace SolidCP.Providers.HostedSolution
{
    public class Exchange2010 : Exchange2007
    {
        #region Static constructor
        static Exchange2010()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveExchangeAssembly);
            ExchangeRegistryPath = "SOFTWARE\\Microsoft\\ExchangeServer\\v14\\Setup";
        }
        #endregion

        #region Mailboxes

        internal override void SetCalendarSettings(Runspace runspace, string id)
        {
            ExchangeLog.LogStart("SetCalendarSettings");
            Command cmd = new Command("Set-CalendarProcessing");
            cmd.Parameters.Add("Identity", id);
            cmd.Parameters.Add("AutomateProcessing", CalendarProcessingFlags.AutoAccept);
            ExecuteShellCommand(runspace, cmd);
            ExchangeLog.LogEnd("SetCalendarSettings");
        }


        internal override ExchangeMailbox GetMailboxGeneralSettingsInternal(string accountName)
        {
            ExchangeLog.LogStart("GetMailboxGeneralSettingsInternal");
            ExchangeLog.DebugInfo("Account: {0}", accountName);

            ExchangeMailbox info = new ExchangeMailbox();
            info.AccountName = accountName;
            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Collection<PSObject> result = GetMailboxObject(runSpace, accountName);
                PSObject mailbox = result[0];

                string id = GetResultObjectDN(result);
                string path = AddADPrefix(id);
                DirectoryEntry entry = GetADObject(path);


                //ADAccountOptions userFlags = (ADAccountOptions)entry.Properties["userAccountControl"].Value;
                //info.Disabled = ((userFlags & ADAccountOptions.UF_ACCOUNTDISABLE) != 0);
                info.Disabled = (bool)entry.InvokeGet("AccountDisabled");

                info.DisplayName = (string)GetPSObjectProperty(mailbox, "DisplayName");
                info.HideFromAddressBook = (bool)GetPSObjectProperty(mailbox, "HiddenFromAddressListsEnabled");
                info.EnableLitigationHold = (bool)GetPSObjectProperty(mailbox, "LitigationHoldEnabled");

                Command cmd = new Command("Get-User");
                cmd.Parameters.Add("Identity", accountName);
                result = ExecuteShellCommand(runSpace, cmd);
                PSObject user = result[0];

                info.FirstName = (string)GetPSObjectProperty(user, "FirstName");
                info.Initials = (string)GetPSObjectProperty(user, "Initials");
                info.LastName = (string)GetPSObjectProperty(user, "LastName");

                info.Address = (string)GetPSObjectProperty(user, "StreetAddress");
                info.City = (string)GetPSObjectProperty(user, "City");
                info.State = (string)GetPSObjectProperty(user, "StateOrProvince");
                info.Zip = (string)GetPSObjectProperty(user, "PostalCode");
                info.Country = CountryInfoToString((CountryInfo)GetPSObjectProperty(user, "CountryOrRegion"));
                info.JobTitle = (string)GetPSObjectProperty(user, "Title");
                info.Company = (string)GetPSObjectProperty(user, "Company");
                info.Department = (string)GetPSObjectProperty(user, "Department");
                info.Office = (string)GetPSObjectProperty(user, "Office");


                info.ManagerAccount = GetManager(entry); //GetExchangeAccount(runSpace, ObjToString(GetPSObjectProperty(user, "Manager")));
                info.BusinessPhone = (string)GetPSObjectProperty(user, "Phone");
                info.Fax = (string)GetPSObjectProperty(user, "Fax");
                info.HomePhone = (string)GetPSObjectProperty(user, "HomePhone");
                info.MobilePhone = (string)GetPSObjectProperty(user, "MobilePhone");
                info.Pager = (string)GetPSObjectProperty(user, "Pager");
                info.WebPage = (string)GetPSObjectProperty(user, "WebPage");
                info.Notes = (string)GetPSObjectProperty(user, "Notes");

            }
            finally
            {

                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("GetMailboxGeneralSettingsInternal");
            return info;
        }


        internal override ExchangeMailbox GetMailboxAdvancedSettingsInternal(string accountName)
        {
            ExchangeLog.LogStart("GetMailboxAdvancedSettingsInternal");
            ExchangeLog.DebugInfo("Account: {0}", accountName);

            ExchangeMailbox info = new ExchangeMailbox();
            info.AccountName = accountName;
            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Collection<PSObject> result = GetMailboxObject(runSpace, accountName);
                PSObject mailbox = result[0];

                info.IssueWarningKB =
                    ConvertUnlimitedToKB((Unlimited<ByteQuantifiedSize>)GetPSObjectProperty(mailbox, "IssueWarningQuota"));
                info.ProhibitSendKB =
                    ConvertUnlimitedToKB((Unlimited<ByteQuantifiedSize>)GetPSObjectProperty(mailbox, "ProhibitSendQuota"));
                info.ProhibitSendReceiveKB =
                    ConvertUnlimitedToKB((Unlimited<ByteQuantifiedSize>)GetPSObjectProperty(mailbox, "ProhibitSendReceiveQuota"));
                info.KeepDeletedItemsDays =
                    ConvertEnhancedTimeSpanToDays((EnhancedTimeSpan)GetPSObjectProperty(mailbox, "RetainDeletedItemsFor"));

                info.EnableLitigationHold = (bool)GetPSObjectProperty(mailbox, "LitigationHoldEnabled");

                info.RecoverabelItemsSpace =
                    ConvertUnlimitedToKB((Unlimited<ByteQuantifiedSize>)GetPSObjectProperty(mailbox, "RecoverableItemsQuota"));
                info.RecoverabelItemsWarning =
                    ConvertUnlimitedToKB((Unlimited<ByteQuantifiedSize>)GetPSObjectProperty(mailbox, "RecoverableItemsWarningQuota"));


                //Client Access
                Command cmd = new Command("Get-CASMailbox");
                cmd.Parameters.Add("Identity", accountName);
                result = ExecuteShellCommand(runSpace, cmd);
                mailbox = result[0];

                info.EnableActiveSync = (bool)GetPSObjectProperty(mailbox, "ActiveSyncEnabled");
                info.EnableOWA = (bool)GetPSObjectProperty(mailbox, "OWAEnabled");
                info.EnableMAPI = (bool)GetPSObjectProperty(mailbox, "MAPIEnabled");
                info.EnablePOP = (bool)GetPSObjectProperty(mailbox, "PopEnabled");
                info.EnableIMAP = (bool)GetPSObjectProperty(mailbox, "ImapEnabled");

                //Statistics
                cmd = new Command("Get-MailboxStatistics");
                cmd.Parameters.Add("Identity", accountName);
                result = ExecuteShellCommand(runSpace, cmd);
                if (result.Count > 0)
                {
                    PSObject statistics = result[0];
                    Unlimited<ByteQuantifiedSize> totalItemSize =
                        (Unlimited<ByteQuantifiedSize>)GetPSObjectProperty(statistics, "TotalItemSize");
                    info.TotalSizeMB = ConvertUnlimitedToMB(totalItemSize);
                    uint? itemCount = (uint?)GetPSObjectProperty(statistics, "ItemCount");
                    info.TotalItems = ConvertNullableToInt32(itemCount);
                    DateTime? lastLogoffTime = (DateTime?)GetPSObjectProperty(statistics, "LastLogoffTime"); ;
                    DateTime? lastLogonTime = (DateTime?)GetPSObjectProperty(statistics, "LastLogonTime"); ;
                    info.LastLogoff = ConvertNullableToDateTime(lastLogoffTime);
                    info.LastLogon = ConvertNullableToDateTime(lastLogonTime);
                }
                else
                {
                    info.TotalSizeMB = 0;
                    info.TotalItems = 0;
                    info.LastLogoff = DateTime.MinValue;
                    info.LastLogon = DateTime.MinValue;
                }

                //domain
                info.Domain = GetNETBIOSDomainName();
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("GetMailboxAdvancedSettingsInternal");
            return info;
        }


        internal override ExchangeMailboxStatistics GetMailboxStatisticsInternal(string id)
        {
            ExchangeLog.LogStart("GetMailboxStatisticsInternal");
            ExchangeLog.DebugInfo("Account: {0}", id);

            ExchangeMailboxStatistics info = new ExchangeMailboxStatistics();
            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Collection<PSObject> result = GetMailboxObject(runSpace, id);
                PSObject mailbox = result[0];

                string dn = GetResultObjectDN(result);
                string path = AddADPrefix(dn);
                DirectoryEntry entry = GetADObject(path);
                info.Enabled = !(bool)entry.InvokeGet("AccountDisabled");
                info.LitigationHoldEnabled = (bool)GetPSObjectProperty(mailbox, "LitigationHoldEnabled");

                info.DisplayName = (string)GetPSObjectProperty(mailbox, "DisplayName");
                SmtpAddress smtpAddress = (SmtpAddress)GetPSObjectProperty(mailbox, "PrimarySmtpAddress");
                if (smtpAddress != null)
                    info.PrimaryEmailAddress = smtpAddress.ToString();

                info.MaxSize = ConvertUnlimitedToBytes((Unlimited<ByteQuantifiedSize>)GetPSObjectProperty(mailbox, "ProhibitSendReceiveQuota"));
                info.LitigationHoldMaxSize = ConvertUnlimitedToBytes((Unlimited<ByteQuantifiedSize>)GetPSObjectProperty(mailbox, "RecoverableItemsQuota"));

                DateTime? whenCreated = (DateTime?)GetPSObjectProperty(mailbox, "WhenCreated");
                info.AccountCreated = ConvertNullableToDateTime(whenCreated);
                //Client Access
                Command cmd = new Command("Get-CASMailbox");
                cmd.Parameters.Add("Identity", id);
                result = ExecuteShellCommand(runSpace, cmd);
                mailbox = result[0];

                info.ActiveSyncEnabled = (bool)GetPSObjectProperty(mailbox, "ActiveSyncEnabled");
                info.OWAEnabled = (bool)GetPSObjectProperty(mailbox, "OWAEnabled");
                info.MAPIEnabled = (bool)GetPSObjectProperty(mailbox, "MAPIEnabled");
                info.POPEnabled = (bool)GetPSObjectProperty(mailbox, "PopEnabled");
                info.IMAPEnabled = (bool)GetPSObjectProperty(mailbox, "ImapEnabled");

                //Statistics
                cmd = new Command("Get-MailboxStatistics");
                cmd.Parameters.Add("Identity", id);
                result = ExecuteShellCommand(runSpace, cmd);
                if (result.Count > 0)
                {
                    PSObject statistics = result[0];
                    Unlimited<ByteQuantifiedSize> totalItemSize = (Unlimited<ByteQuantifiedSize>)GetPSObjectProperty(statistics, "TotalItemSize");
                    info.TotalSize = ConvertUnlimitedToBytes(totalItemSize);

                    uint? itemCount = (uint?)GetPSObjectProperty(statistics, "ItemCount");
                    info.TotalItems = ConvertNullableToInt32(itemCount);
                    
                    DateTime? lastLogoffTime = (DateTime?)GetPSObjectProperty(statistics, "LastLogoffTime");
                    DateTime? lastLogonTime = (DateTime?)GetPSObjectProperty(statistics, "LastLogonTime");
                    info.LastLogoff = ConvertNullableToDateTime(lastLogoffTime);
                    info.LastLogon = ConvertNullableToDateTime(lastLogonTime);
                }
                else
                {
                    info.TotalSize = 0;
                    info.TotalItems = 0;
                    info.LastLogoff = DateTime.MinValue;
                    info.LastLogon = DateTime.MinValue;
                }

                if (info.LitigationHoldEnabled)
                {
                    cmd = new Command("Get-MailboxFolderStatistics");
                    cmd.Parameters.Add("FolderScope", "RecoverableItems");
                    cmd.Parameters.Add("Identity", id);
                    result = ExecuteShellCommand(runSpace, cmd);
                    if (result.Count > 0)
                    {
                        PSObject statistics = result[0];
                        ByteQuantifiedSize totalItemSize = (ByteQuantifiedSize)GetPSObjectProperty(statistics, "FolderAndSubfolderSize");
                        info.LitigationHoldTotalSize = (totalItemSize == null) ? 0 : ConvertUnlimitedToBytes(totalItemSize);

                        Int32 itemCount = (Int32)GetPSObjectProperty(statistics, "ItemsInFolder");
                        info.LitigationHoldTotalItems = (itemCount == 0) ? 0 : itemCount;
                    }
                }
                else
                {
                    info.LitigationHoldTotalSize = 0;
                    info.LitigationHoldTotalItems = 0;
                }

            }
            finally
            {
                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("GetMailboxStatisticsInternal");
            return info;
        }



        internal override void SetMailboxAdvancedSettingsInternal(string organizationId, string accountName, bool enablePOP, bool enableIMAP,
            bool enableOWA, bool enableMAPI, bool enableActiveSync, long issueWarningKB, long prohibitSendKB,
            long prohibitSendReceiveKB, int keepDeletedItemsDays, int maxRecipients, int maxSendMessageSizeKB,
            int maxReceiveMessageSizeKB, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning,
            string litigationHoldUrl, string litigationHoldMsg)
        {
            ExchangeLog.LogStart("SetMailboxAdvancedSettingsInternal");
            ExchangeLog.DebugInfo("Account: {0}", accountName);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();


                Command cmd = new Command("Set-Mailbox");
                cmd.Parameters.Add("Identity", accountName);
                cmd.Parameters.Add("IssueWarningQuota", ConvertKBToUnlimited(issueWarningKB));
                cmd.Parameters.Add("ProhibitSendQuota", ConvertKBToUnlimited(prohibitSendKB));
                cmd.Parameters.Add("ProhibitSendReceiveQuota", ConvertKBToUnlimited(prohibitSendReceiveKB));
                cmd.Parameters.Add("RetainDeletedItemsFor", ConvertDaysToEnhancedTimeSpan(keepDeletedItemsDays));
                cmd.Parameters.Add("RecipientLimits", ConvertInt32ToUnlimited(maxRecipients));
                cmd.Parameters.Add("MaxSendSize", ConvertKBToUnlimited(maxSendMessageSizeKB));
                cmd.Parameters.Add("MaxReceiveSize", ConvertKBToUnlimited(maxReceiveMessageSizeKB));

                cmd.Parameters.Add("LitigationHoldEnabled", enabledLitigationHold);
                cmd.Parameters.Add("RecoverableItemsQuota", ConvertKBToUnlimited(recoverabelItemsSpace));

                cmd.Parameters.Add("RetentionUrl", litigationHoldUrl);
                cmd.Parameters.Add("RetentionComment", litigationHoldMsg);

                if (recoverabelItemsSpace != -1) cmd.Parameters.Add("RecoverableItemsWarningQuota", ConvertKBToUnlimited(recoverabelItemsWarning));

                ExecuteShellCommand(runSpace, cmd);

                //Client Access
                cmd = new Command("Set-CASMailbox");
                cmd.Parameters.Add("Identity", accountName);
                cmd.Parameters.Add("ActiveSyncEnabled", enableActiveSync);
                if (enableActiveSync)
                {
                    cmd.Parameters.Add("ActiveSyncMailboxPolicy", organizationId);
                }
                cmd.Parameters.Add("OWAEnabled", enableOWA);
                cmd.Parameters.Add("MAPIEnabled", enableMAPI);
                cmd.Parameters.Add("PopEnabled", enablePOP);
                cmd.Parameters.Add("ImapEnabled", enableIMAP);
                ExecuteShellCommand(runSpace, cmd);
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("SetMailboxAdvancedSettingsInternal");
        }


        protected override ExchangeAccount[] GetMailboxCalendarAccounts(Runspace runSpace, string organizationId, string accountName)
        {
            ExchangeLog.LogStart("GetMailboxCalendarAccounts");

            string cn = GetMailboxCommonName(runSpace, accountName);
            ExchangeAccount[] ret = GetCalendarAccounts(runSpace, organizationId, cn);

            ExchangeLog.LogEnd("GetMailboxCalendarAccounts");
            return ret;
        }

        protected override ExchangeAccount[] GetMailboxContactAccounts(Runspace runSpace, string organizationId, string accountName)
        {
            ExchangeLog.LogStart("GetMailboxContactAccounts");

            string cn = GetMailboxCommonName(runSpace, accountName);
            ExchangeAccount[] ret = GetContactAccounts(runSpace, organizationId, cn);

            ExchangeLog.LogEnd("GetMailboxContactAccounts");
            return ret;
        }

        private ExchangeAccount[] GetCalendarAccounts(Runspace runSpace, string organizationId, string accountId)
        {
            ExchangeLog.LogStart("GetContactAccounts");

            var folderPath = GetMailboxFolderPath(accountId, ExchangeFolders.Calendar);

            var accounts = GetMailboxFolderPermissions(runSpace, organizationId, folderPath);

            ExchangeLog.LogEnd("GetContactAccounts");
            return accounts.ToArray();
        }

        private ExchangeAccount[] GetContactAccounts(Runspace runSpace, string organizationId, string accountId)
        {
            ExchangeLog.LogStart("GetContactAccounts");

            var folderPath = GetMailboxFolderPath(accountId, ExchangeFolders.Contacts);

            var accounts = GetMailboxFolderPermissions(runSpace, organizationId, folderPath);

            ExchangeLog.LogEnd("GetContactAccounts");
            return accounts.ToArray();
        }

        private List<ExchangeAccount> GetMailboxFolderPermissions(Runspace runSpace, string organizationId, string folderPath)
        {
            Command cmd = new Command("Get-MailboxFolderPermission");
            cmd.Parameters.Add("Identity", folderPath);
            Collection<PSObject> results = ExecuteShellCommand(runSpace, cmd);

            List<ExchangeAccount> accounts = new List<ExchangeAccount>();

            foreach (PSObject current in results)
            {
                string user = GetPSObjectProperty(current, "User").ToString();

                var accessRights = GetPSObjectProperty(current, "AccessRights") as IEnumerable;

                if (accessRights == null)
                {
                    continue;
                }

                ExchangeAccount account = GetOrganizationAccount(runSpace, organizationId, user);

                if (account != null)
                {
                    account.PublicFolderPermission = accessRights.Cast<object>().First().ToString();
                    accounts.Add(account);
                }
            }

            return accounts;
        }

        protected override void SetMailboxFolderPermissions(Runspace runSpace, ExchangeAccount[] existingAccounts, string folderPath, ExchangeAccount[] accounts)
        {
            ExchangeLog.LogStart("SetMailboxFolderPermissions");

            if (string.IsNullOrEmpty(folderPath))
                throw new ArgumentNullException("folderPath");

            if (accounts == null)
                throw new ArgumentNullException("accounts");

            ExchangeTransaction transaction = StartTransaction();

            try
            {
                SetMailboxFolderPermissions(runSpace, folderPath, existingAccounts, accounts, transaction);
            }
            catch (Exception)
            {
                RollbackTransaction(transaction);
                throw;
            }

            ExchangeLog.LogEnd("SetMailboxFolderPermissions");
        }

        private void SetMailboxFolderPermissions(Runspace runSpace, string folderPath, ExchangeAccount[] existingAccounts, ExchangeAccount[] newAccounts, ExchangeTransaction transaction)
        {
            ResetMailboxFolderPermissions(runSpace, folderPath, existingAccounts, transaction);

            AddMailboxFolderPermissions(runSpace, folderPath, newAccounts, transaction);
        }


        private void AddMailboxFolderPermissions(Runspace runSpace, string folderPath, ExchangeAccount[] accounts, ExchangeTransaction transaction)
        {
            ExchangeLog.LogStart("SetMailboxCalendarPermissions");

            foreach (var account in accounts)
            {
                AddMailboxFolderPermission(runSpace, folderPath, account);

                transaction.AddMailboxFolderPermission(folderPath, account);
            }

            ExchangeLog.LogEnd("SetMailboxCalendarPermissions");
        }

        private void ResetMailboxFolderPermissions(Runspace runSpace, string folderPath, ExchangeAccount[] accounts, ExchangeTransaction transaction)
        {
            ExchangeLog.LogStart("ResetMailboxFolderPermissions");

            foreach (var account in accounts)
            {
                RemoveMailboxFolderPermission(runSpace, folderPath, account);

                transaction.RemoveMailboxFolderPermissions(folderPath, account);
            }

            ExchangeLog.LogEnd("ResetMailboxFolderPermissions");
        }

        protected override void AddMailboxFolderPermission(Runspace runSpace, string folderPath, ExchangeAccount account)
        {
            Command cmd = new Command("Add-MailboxFolderPermission");
            cmd.Parameters.Add("Identity", folderPath);
            cmd.Parameters.Add("User", account.AccountName);
            cmd.Parameters.Add("AccessRights", account.PublicFolderPermission);

            ExecuteShellCommand(runSpace, cmd);
        }

        protected override void RemoveMailboxFolderPermission(Runspace runSpace, string folderPath, ExchangeAccount account)
        {
            Command cmd = new Command("Remove-MailboxFolderPermission");
            cmd.Parameters.Add("Identity", folderPath);
            cmd.Parameters.Add("User", account.AccountName);
            ExecuteShellCommand(runSpace, cmd);
        }

        #endregion

        #region Distribution Lists
        internal override string GetGroupManager(PSObject group)
        {
            string ret = null;
            MultiValuedProperty<ADObjectId> ids =
                (MultiValuedProperty<ADObjectId>)GetPSObjectProperty(group, "ManagedBy");
            if (ids.Count > 0)
                ret = ObjToString(ids[0]);
            return ret;
        }

        internal override void RemoveDistributionGroup(Runspace runSpace, string id)
        {
            ExchangeLog.LogStart("RemoveDistributionGroup");
            Command cmd = new Command("Remove-DistributionGroup");
            cmd.Parameters.Add("Identity", id);
            cmd.Parameters.Add("Confirm", false);
            cmd.Parameters.Add("BypassSecurityGroupManagerCheck");
            ExecuteShellCommand(runSpace, cmd);
            ExchangeLog.LogEnd("RemoveDistributionGroup");
        }

        internal override void SetDistributionGroup(Runspace runSpace, string id, string displayName, bool hideFromAddressBook)
        {
            Command cmd = new Command("Set-DistributionGroup");
            cmd.Parameters.Add("Identity", id);
            cmd.Parameters.Add("DisplayName", displayName);
            cmd.Parameters.Add("HiddenFromAddressListsEnabled", hideFromAddressBook);
            cmd.Parameters.Add("BypassSecurityGroupManagerCheck");
            ExecuteShellCommand(runSpace, cmd);
        }

        internal override void SetGroup(Runspace runSpace, string id, string managedBy, string notes)
        {
            Command cmd = new Command("Set-Group");
            cmd.Parameters.Add("Identity", id);
            cmd.Parameters.Add("ManagedBy", managedBy);
            cmd.Parameters.Add("Notes", notes);
            cmd.Parameters.Add("BypassSecurityGroupManagerCheck");
            ExecuteShellCommand(runSpace, cmd);
        }

        internal override void RemoveDistributionGroupMember(Runspace runSpace, string group, string member)
        {
            Command cmd = new Command("Remove-DistributionGroupMember");
            cmd.Parameters.Add("Identity", group);
            cmd.Parameters.Add("Member", member);
            cmd.Parameters.Add("Confirm", false);
            cmd.Parameters.Add("BypassSecurityGroupManagerCheck");
            ExecuteShellCommand(runSpace, cmd);
        }

        internal override void AddDistributionGroupMember(Runspace runSpace, string group, string member)
        {
            Command cmd = new Command("Add-DistributionGroupMember");
            cmd.Parameters.Add("Identity", group);
            cmd.Parameters.Add("Member", member);
            cmd.Parameters.Add("BypassSecurityGroupManagerCheck");
            ExecuteShellCommand(runSpace, cmd);
        }

        internal override void SetDistributionListSendOnBehalfAccounts(Runspace runspace, string accountName, string[] sendOnBehalfAccounts)
        {
            ExchangeLog.LogStart("SetDistributionListSendOnBehalfAccounts");
            Command cmd = new Command("Set-DistributionGroup");
            cmd.Parameters.Add("Identity", accountName);
            cmd.Parameters.Add("GrantSendOnBehalfTo", SetSendOnBehalfAccounts(runspace, sendOnBehalfAccounts));
            cmd.Parameters.Add("BypassSecurityGroupManagerCheck");
            ExecuteShellCommand(runspace, cmd);
            ExchangeLog.LogEnd("SetDistributionListSendOnBehalfAccounts");
        }
        #endregion

        #region PowerShell integration
        internal override string ExchangeSnapInName
        {
            get { return "Microsoft.Exchange.Management.PowerShell.E2010"; }
        }

        internal override Runspace OpenRunspace()
        {
            Runspace runspace = base.OpenRunspace();
            Command cmd = new Command("Set-ADServerSettings");
            cmd.Parameters.Add("PreferredServer", PrimaryDomainController);
            ExecuteShellCommand(runspace, cmd, false);
            return runspace;
        }

        internal static Assembly ResolveExchangeAssembly(object p, ResolveEventArgs args)
        {
            //Add path for the Exchange 2007 DLLs
            if (args.Name.Contains("Microsoft.Exchange"))
            {
                string exchangePath = GetExchangePath();
                if (string.IsNullOrEmpty(exchangePath))
                    return null;

                string path = Path.Combine(exchangePath, args.Name.Split(',')[0] + ".dll");
                if (!File.Exists(path))
                    return null;

                ExchangeLog.DebugInfo("Resolved assembly: {0}", path);

                return Assembly.LoadFrom(path);
            }
            else
            {
                return null;
            }
        }

        internal Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, ResultObject res, bool setIsSuccess)
        {
            object[] errors;
            Collection<PSObject> ret = ExecuteShellCommand(runSpace, cmd, out errors);
            if (errors.Length > 0)
            {
                foreach (object error in errors)
                    res.ErrorCodes.Add(error.ToString());
                if (setIsSuccess)
                    res.IsSuccess = false;
            }
            return ret;
        }


        #endregion

        #region Storage
        internal override string CreateStorageGroup(Runspace runSpace, string name, string server)
        {
            return string.Empty;
        }

        internal override string CreateMailboxDatabase(Runspace runSpace, string name, string storageGroup)
        {
            ExchangeLog.LogStart("CreateMailboxDatabase");
            string id;
            if (name != "*")
            {
                Command cmd = new Command("Get-MailboxDatabase");
                cmd.Parameters.Add("Identity", name);
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);
                if (result != null && result.Count > 0)
                {
                    id = GetResultObjectIdentity(result);
                }
                else
                {
                    throw new Exception(string.Format("Mailbox database {0} not found", name));
                }
            }
            else
            {
                id = "*";
            }
            ExchangeLog.LogEnd("CreateMailboxDatabase");
            return id;
        }
        #endregion

        #region Picture
        public override ResultObject SetPicture(string accountName, byte[] picture)
        {
            ExchangeLog.LogStart("SetPicture");

            ResultObject res = new ResultObject() { IsSuccess = true };

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();
                Command cmd;

                if (picture == null)
                {
                    cmd = new Command("Set-Mailbox");
                    cmd.Parameters.Add("Identity", accountName);
                    cmd.Parameters.Add("RemovePicture", true);
                }
                else
                {
                    cmd = new Command("Import-RecipientDataProperty");
                    cmd.Parameters.Add("Identity", accountName);
                    cmd.Parameters.Add("Picture", true);
                    cmd.Parameters.Add("FileData", picture);
                }
                ExecuteShellCommand(runSpace, cmd, res, true);
            }
            finally
            {
                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("SetPicture");

            return res;
        }
        public override BytesResult GetPicture(string accountName)
        {
            ExchangeLog.LogStart("GetPicture");

            BytesResult res = new BytesResult() { IsSuccess = true };

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd;

                cmd = new Command("Export-RecipientDataProperty");
                cmd.Parameters.Add("Identity", accountName);
                cmd.Parameters.Add("Picture", true);
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd, res, true);

                if (result.Count > 0)
                {
                    res.Value =
                    ((Microsoft.Exchange.Data.BinaryFileDataObject)
                     (result[0].ImmediateBaseObject)).FileData;
                }
            }
            finally
            {
                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("GetPicture");

            return res;
        }
        #endregion

        public override bool IsInstalled()
        {
            int value = 0;
            bool bResult = false;
            RegistryKey root = Registry.LocalMachine;
            RegistryKey rk = root.OpenSubKey(ExchangeRegistryPath);
            if (rk != null)
            {
                value = (int)rk.GetValue("MsiProductMajor", null);
                if (value == 14)
                {
                    value = (int)rk.GetValue("MsiProductMinor", null);
                    if ((value == 0) | (value == 1)) bResult = true;
                }

                rk.Close();
            }
            return bResult;
        }
    }
}

