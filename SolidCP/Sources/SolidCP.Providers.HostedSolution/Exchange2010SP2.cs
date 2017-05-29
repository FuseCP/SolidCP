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
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Collections;

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

using Microsoft.Exchange.Data;
using Microsoft.Exchange.Data.Directory;
using Microsoft.Exchange.Data.Storage;

namespace SolidCP.Providers.HostedSolution
{
    public class Exchange2010SP2 : Exchange2010
    {
        #region Static constructor

        static private Hashtable htBbalancer = new Hashtable();

        static Exchange2010SP2()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveExchangeAssembly);
            ExchangeRegistryPath = "SOFTWARE\\Microsoft\\ExchangeServer\\v14\\Setup";
        }
        #endregion


        #region Organization
        /// <summary>
        /// Creates organization on Mail Server
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        internal override Organization ExtendToExchangeOrganizationInternal(string organizationId, string securityGroup, bool IsConsumer)
        {
            ExchangeLog.LogStart("CreateOrganizationInternal");
            ExchangeLog.DebugInfo("  Organization Id: {0}", organizationId);

            ExchangeTransaction transaction = StartTransaction();
            Organization info = new Organization();
            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                string server = GetServerName();
                string securityGroupPath = AddADPrefix(securityGroup);

                //Create mail enabled organization security group
                EnableMailSecurityDistributionGroup(runSpace, securityGroup, organizationId);
                transaction.RegisterMailEnabledDistributionGroup(securityGroup);
                UpdateSecurityDistributionGroup(runSpace, securityGroup, organizationId, IsConsumer);

                //create GAL
                string galId = CreateGlobalAddressList(runSpace, organizationId);
                transaction.RegisterNewGlobalAddressList(galId);
                ExchangeLog.LogInfo("  Global Address List: {0}", galId);
                UpdateGlobalAddressList(runSpace, galId, securityGroupPath);

                //create AL
                string alId = CreateAddressList(runSpace, organizationId);
                transaction.RegisterNewAddressList(alId);
                ExchangeLog.LogInfo("  Address List: {0}", alId);
                UpdateAddressList(runSpace, alId, securityGroupPath);

                //create RAL
                string ralId = CreateRoomsAddressList(runSpace, organizationId);
                transaction.RegisterNewRoomsAddressList(ralId);
                ExchangeLog.LogInfo("  Rooms Address List: {0}", ralId);
                UpdateAddressList(runSpace, ralId, securityGroupPath);

                //create ActiveSync policy
                string asId = CreateActiveSyncPolicy(runSpace, organizationId);
                transaction.RegisterNewActiveSyncPolicy(asId);
                ExchangeLog.LogInfo("  ActiveSync Policy: {0}", asId);

                info.AddressList = alId;
                info.GlobalAddressList = galId;
                info.RoomsAddressList = ralId;
                info.OrganizationId = organizationId;
            }
            catch (Exception ex)
            {
                ExchangeLog.LogError("CreateOrganizationInternal", ex);
                RollbackTransaction(transaction);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("CreateOrganizationInternal");
            return info;
        }


        internal override Organization CreateOrganizationAddressBookPolicyInternal(string organizationId, string gal, string addressBook, string roomList, string oab)
        {
            ExchangeLog.LogStart("CreateOrganizationAddressBookPolicyInternal");
            ExchangeLog.LogInfo("  Organization Id: {0}", organizationId);
            ExchangeLog.LogInfo("  GAL: {0}", gal);
            ExchangeLog.LogInfo("  AddressBook: {0}", addressBook);
            ExchangeLog.LogInfo("  RoomList: {0}", roomList);
            ExchangeLog.LogInfo("  OAB: {0}", oab);

            ExchangeTransaction transaction = StartTransaction();

            Organization info = new Organization();
            string policyName = GetAddressBookPolicyName(organizationId);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();
                Command cmd = new Command("New-AddressBookPolicy");
                cmd.Parameters.Add("Name", policyName);
                cmd.Parameters.Add("AddressLists", addressBook);
                cmd.Parameters.Add("RoomList", roomList);
                cmd.Parameters.Add("GlobalAddressList", gal);
                cmd.Parameters.Add("OfflineAddressBook", oab);

                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);
                info.AddressBookPolicy = GetResultObjectDN(result);

            }
            catch (Exception ex)
            {
                ExchangeLog.LogError("CreateOrganizationAddressBookPolicyInternal", ex);
                RollbackTransaction(transaction);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("CreateOrganizationAddressBookPolicyInternal");
            return info;
        }

        internal override bool DeleteOrganizationInternal(string organizationId, string distinguishedName,
                    string globalAddressList, string addressList, string roomList, string offlineAddressBook, string securityGroup, string addressBookPolicy, List<ExchangeDomainName> acceptedDomains)
        {
            ExchangeLog.LogStart("DeleteOrganizationInternal");
            bool ret = true;

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();


                string ou = ConvertADPathToCanonicalName(distinguishedName);

                if (!DeleteOrganizationMailboxes(runSpace, ou))
                    ret = false;

                if (!DeleteOrganizationContacts(runSpace, ou))
                    ret = false;

                if (!DeleteOrganizationDistributionLists(runSpace, ou))
                    ret = false;

                //delete AddressBookPolicy
                try
                {
                    if (!string.IsNullOrEmpty(addressBookPolicy))
                        DeleteAddressBookPolicy(runSpace, addressBookPolicy);
                }
                catch (Exception ex)
                {
                    ret = false;
                    ExchangeLog.LogError("Could not delete AddressBook Policy " + addressBookPolicy, ex);
                }

                //delete OAB
                try
                {
                    if (!string.IsNullOrEmpty(offlineAddressBook))
                        DeleteOfflineAddressBook(runSpace, offlineAddressBook);
                }
                catch (Exception ex)
                {
                    ret = false;
                    ExchangeLog.LogError("Could not delete Offline Address Book " + offlineAddressBook, ex);
                }

                //delete AL
                try
                {
                    if (!string.IsNullOrEmpty(addressList))
                        DeleteAddressList(runSpace, addressList);
                }
                catch (Exception ex)
                {
                    ret = false;
                    ExchangeLog.LogError("Could not delete Address List " + addressList, ex);
                }

                //delete RL
                try
                {
                    if (!string.IsNullOrEmpty(roomList))
                        DeleteAddressList(runSpace, roomList);
                }
                catch (Exception ex)
                {
                    ret = false;
                    ExchangeLog.LogError("Could not delete Address List " + roomList, ex);
                }


                //delete GAL
                try
                {
                    if (!string.IsNullOrEmpty(globalAddressList))
                        DeleteGlobalAddressList(runSpace, globalAddressList);
                }
                catch (Exception ex)
                {
                    ret = false;
                    ExchangeLog.LogError("Could not delete Global Address List " + globalAddressList, ex);
                }

                //delete ActiveSync policy
                try
                {
                    DeleteActiveSyncPolicy(runSpace, organizationId);
                }
                catch (Exception ex)
                {
                    ret = false;
                    ExchangeLog.LogError("Could not delete ActiveSyncPolicy " + organizationId, ex);
                }

                //disable mail security distribution group
                try
                {
                    DisableMailSecurityDistributionGroup(runSpace, securityGroup);
                }
                catch (Exception ex)
                {
                    ret = false;
                    ExchangeLog.LogError("Could not disable mail security distribution group " + securityGroup, ex);
                }

                if (!DeleteOrganizationAcceptedDomains(runSpace, acceptedDomains))
                    ret = false;
            }
            catch (Exception ex)
            {
                ret = false;
                ExchangeLog.LogError("DeleteOrganizationInternal", ex);
                throw;
            }
            finally
            {

                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("DeleteOrganizationInternal");
            return ret;
        }

        internal override void DeleteAddressBookPolicy(Runspace runSpace, string id)
        {
            ExchangeLog.LogStart("DeleteAddressBookPolicy");
            //if (id != "IsConsumer")
            //{
            Command cmd = new Command("Remove-AddressBookPolicy");
            cmd.Parameters.Add("Identity", id);
            cmd.Parameters.Add("Confirm", false);
            ExecuteShellCommand(runSpace, cmd);
            //}
            ExchangeLog.LogEnd("DeleteAddressBookPolicy");
        }

        #endregion

        #region Mailbox
        internal override string CreateMailEnableUserInternal(string upn, string organizationId, string organizationDistinguishedName,
            ExchangeAccountType accountType,
            string mailboxDatabase, string offlineAddressBook, string addressBookPolicy,
            string accountName, bool enablePOP, bool enableIMAP,
            bool enableOWA, bool enableMAPI, bool enableActiveSync,
            long issueWarningKB, long prohibitSendKB, long prohibitSendReceiveKB, int keepDeletedItemsDays,
            int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool hideFromAddressBook, bool IsConsumer, bool enabledLitigationHold, long recoverabelItemsSpace, long recoverabelItemsWarning)
        {

            ExchangeLog.LogStart("CreateMailEnableUserInternal");
            ExchangeLog.DebugInfo("Organization Id: {0}", organizationId);

            string ret = null;
            ExchangeTransaction transaction = StartTransaction();
            Runspace runSpace = null;

            int attempts = 0;
            string id = null;

            try
            {
                runSpace = OpenRunspace();
                Command cmd = null;
                Collection<PSObject> result = null;

                //try to enable mail user for 10 times
                while (true)
                {
                    try
                    {
                        //create mailbox
                        cmd = new Command("Enable-Mailbox");
                        cmd.Parameters.Add("Identity", upn);
                        cmd.Parameters.Add("Alias", accountName);
                        string database = GetDatabase(runSpace, PrimaryDomainController, mailboxDatabase);
                        ExchangeLog.DebugInfo("database: " + database);
                        if (database != string.Empty)
                        {
                            cmd.Parameters.Add("Database", database);
                        }
                        if (accountType == ExchangeAccountType.Equipment)
                            cmd.Parameters.Add("Equipment");
                        else if (accountType == ExchangeAccountType.Room)
                            cmd.Parameters.Add("Room");
                        else if (accountType == ExchangeAccountType.SharedMailbox)
                            cmd.Parameters.Add("Shared");

                        result = ExecuteShellCommand(runSpace, cmd);

                        id = CheckResultObjectDN(result);
                    }
                    catch (Exception ex)
                    {
                        ExchangeLog.LogError(ex);
                    }
                    if (id != null)
                        break;

                    if (attempts > 9)
                        throw new Exception(
                            string.Format("Could not enable mail user '{0}' ", upn));

                    attempts++;
                    ExchangeLog.LogWarning("Attempt #{0} to enable mail user failed!", attempts);
                    // wait 5 sec
                    System.Threading.Thread.Sleep(1000);
                }

                transaction.RegisterEnableMailbox(id);

                string windowsEmailAddress = ObjToString(GetPSObjectProperty(result[0], "WindowsEmailAddress"));

                //update mailbox
                cmd = new Command("Set-Mailbox");
                cmd.Parameters.Add("Identity", id);
                cmd.Parameters.Add("OfflineAddressBook", offlineAddressBook);
                cmd.Parameters.Add("EmailAddressPolicyEnabled", false);
                cmd.Parameters.Add("CustomAttribute1", organizationId);
                cmd.Parameters.Add("CustomAttribute3", windowsEmailAddress);
                cmd.Parameters.Add("PrimarySmtpAddress", upn);
                cmd.Parameters.Add("WindowsEmailAddress", upn);

                cmd.Parameters.Add("UseDatabaseQuotaDefaults", new bool?(false));
                cmd.Parameters.Add("UseDatabaseRetentionDefaults", false);
                cmd.Parameters.Add("IssueWarningQuota", ConvertKBToUnlimited(issueWarningKB));
                cmd.Parameters.Add("ProhibitSendQuota", ConvertKBToUnlimited(prohibitSendKB));
                cmd.Parameters.Add("ProhibitSendReceiveQuota", ConvertKBToUnlimited(prohibitSendReceiveKB));
                cmd.Parameters.Add("RetainDeletedItemsFor", ConvertDaysToEnhancedTimeSpan(keepDeletedItemsDays));
                cmd.Parameters.Add("RecipientLimits", ConvertInt32ToUnlimited(maxRecipients));
                cmd.Parameters.Add("MaxSendSize", ConvertKBToUnlimited(maxSendMessageSizeKB));
                cmd.Parameters.Add("MaxReceiveSize", ConvertKBToUnlimited(maxReceiveMessageSizeKB));
                if (IsConsumer) cmd.Parameters.Add("HiddenFromAddressListsEnabled", true);
                else
                    cmd.Parameters.Add("HiddenFromAddressListsEnabled", hideFromAddressBook);
                cmd.Parameters.Add("AddressBookPolicy", addressBookPolicy);

                if (enabledLitigationHold)
                {
                    cmd.Parameters.Add("LitigationHoldEnabled", true);
                    cmd.Parameters.Add("RecoverableItemsQuota", ConvertKBToUnlimited(recoverabelItemsSpace));
                    cmd.Parameters.Add("RecoverableItemsWarningQuota", ConvertKBToUnlimited(recoverabelItemsWarning));
                }

                ExecuteShellCommand(runSpace, cmd);

                //Client Access
                cmd = new Command("Set-CASMailbox");
                cmd.Parameters.Add("Identity", id);
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

                //calendar settings
                if (accountType == ExchangeAccountType.Equipment || accountType == ExchangeAccountType.Room)
                {
                    SetCalendarSettings(runSpace, id);
                }

                //add to the security group
                cmd = new Command("Add-DistributionGroupMember");
                cmd.Parameters.Add("Identity", organizationId);
                cmd.Parameters.Add("Member", id);
                cmd.Parameters.Add("BypassSecurityGroupManagerCheck", true);
                ExecuteShellCommand(runSpace, cmd);

                if (!IsConsumer)
                {
                    //Set-MailboxFolderPermission for calendar
                    cmd = new Command("Add-MailboxFolderPermission");
                    cmd.Parameters.Add("Identity", id + ":\\calendar");
                    cmd.Parameters.Add("AccessRights", "Reviewer");
                    cmd.Parameters.Add("User", organizationId);
                    ExecuteShellCommand(runSpace, cmd);
                }
                cmd = new Command("Set-MailboxFolderPermission");
                cmd.Parameters.Add("Identity", id + ":\\calendar");
                cmd.Parameters.Add("AccessRights", "None");
                cmd.Parameters.Add("User", "Default");
                ExecuteShellCommand(runSpace, cmd);

                ret = string.Format("{0}\\{1}", GetNETBIOSDomainName(), accountName);
                ExchangeLog.LogEnd("CreateMailEnableUserInternal");
                return ret;
            }
            catch (Exception ex)
            {
                ExchangeLog.LogError("CreateMailEnableUserInternal", ex);
                RollbackTransaction(transaction);
                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
            }
        }


        internal override void SetCalendarSettings(Runspace runspace, string id)
        {
            ExchangeLog.LogStart("SetCalendarSettings");
            Command cmd = new Command("Set-CalendarProcessing");
            cmd.Parameters.Add("Identity", id);
            cmd.Parameters.Add("AutomateProcessing", CalendarProcessingFlags.AutoAccept);
            ExecuteShellCommand(runspace, cmd);
            ExchangeLog.LogEnd("SetCalendarSettings");
        }

        internal override void DisableMailboxInternal(string id)
        {
            ExchangeLog.LogStart("DisableMailboxInternal");
            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Get-Mailbox");
                cmd.Parameters.Add("Identity", id);
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);

                if (result != null && result.Count > 0)
                {
                    string upn = ObjToString(GetPSObjectProperty(result[0], "UserPrincipalName"));

                    string addressbookPolicy = ObjToString(GetPSObjectProperty(result[0], "AddressBookPolicy"));

                    RemoveDevicesInternal(runSpace, id);

                    cmd = new Command("Disable-Mailbox");
                    cmd.Parameters.Add("Identity", id);
                    cmd.Parameters.Add("Confirm", false);
                    ExecuteShellCommand(runSpace, cmd);


                    if (addressbookPolicy == (upn + " AP"))
                    {
                        try
                        {
                            DeleteAddressBookPolicy(runSpace, upn + " AP");
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            DeleteGlobalAddressList(runSpace, upn + " GAL");
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            DeleteAddressList(runSpace, upn + " AL");
                        }
                        catch (Exception)
                        {
                        }
                    }


                }

            }

            finally
            {

                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("DisableMailboxInternal");
        }


        internal override void DeleteMailboxInternal(string accountName)
        {
            ExchangeLog.LogStart("DeleteMailboxInternal");
            ExchangeLog.DebugInfo("Account Name: {0}", accountName);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Get-Mailbox");
                cmd.Parameters.Add("Identity", accountName);
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);

                if (result != null && result.Count > 0)
                {
                    string upn = ObjToString(GetPSObjectProperty(result[0], "UserPrincipalName"));
                    string addressbookPolicy = ObjToString(GetPSObjectProperty(result[0], "AddressBookPolicy"));

                    RemoveDevicesInternal(runSpace, accountName);

                    RemoveMailbox(runSpace, accountName);

                    if (addressbookPolicy == (upn + " AP"))
                    {
                        try
                        {
                            DeleteAddressBookPolicy(runSpace, upn + " AP");
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            DeleteGlobalAddressList(runSpace, upn + " GAL");
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            DeleteAddressList(runSpace, upn + " AL");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }


            }
            finally
            {

                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("DeleteMailboxInternal");
        }


        internal string GetDatabase(Runspace runSpace, string primaryDomainController, string dagName)
        {
            string database = string.Empty;

            if (string.IsNullOrEmpty(dagName)) return string.Empty;

            ExchangeLog.LogStart("GetDatabase");
            ExchangeLog.LogInfo("DAG: " + dagName);

            // this part of code handles mailboxnames like in the old 2007 provider
            // check if DAG is in reality DAG\mailboxdatabase
            string dagNameDAG = string.Empty;
            string dagNameMBX = string.Empty;
            bool isFixedDatabase = false;
            if (dagName.Contains("\\"))
            {
                // split the two parts and extract DAG-Name and mailboxdatabase-name
                string[] parts = dagName.Split(new char[] { '\\' }, 2, StringSplitOptions.None);
                dagNameDAG = parts[0];
                dagNameMBX = parts[1];
                // check that we realy have a database name
                if (!String.IsNullOrEmpty(dagNameMBX))
                {
                    isFixedDatabase = true;
                }
            }
            else
            {
                // there is no mailboxdatabase-name use the loadbalancing-code
                dagNameDAG = dagName;
                isFixedDatabase = false;
            }

            //Get Dag Servers - with the name of the database availability group
            Collection<PSObject> dags = null;
            Command cmd = new Command("Get-DatabaseAvailabilityGroup");
            cmd.Parameters.Add("Identity", dagNameDAG);
            dags = ExecuteShellCommand(runSpace, cmd);

            if (htBbalancer == null)
                htBbalancer = new Hashtable();

            // use fully qualified dagName for loadbalancer. Thus if there are two services and one of them
            // contains only the DAG, the "fixed" database could also be used in loadbalancing. If you do not want this,
            // set either IsExcludedFromProvisioning or IsSuspendedFromProvisioning - it is not evaluated for fixed databases
            if (htBbalancer[dagName] == null)
                htBbalancer.Add(dagName, 0);

            if (dags != null && dags.Count > 0)
            {

                ADMultiValuedProperty<ADObjectId> servers = (ADMultiValuedProperty<ADObjectId>)GetPSObjectProperty(dags[0], "Servers");

                if (servers != null)
                {
                    System.Collections.Generic.List<string> lstDatabase = new System.Collections.Generic.List<string>();

                    if (!isFixedDatabase) // "old" loadbalancing code
                    {
                        foreach (object objServer in servers)
                        {
                            Collection<PSObject> databases = null;
                            cmd = new Command("Get-MailboxDatabase");
                            cmd.Parameters.Add("Server", ObjToString(objServer));
                            databases = ExecuteShellCommand(runSpace, cmd);

                            foreach (PSObject objDatabase in databases)
                            {
                                if (((bool)GetPSObjectProperty(objDatabase, "IsExcludedFromProvisioning") == false) &&
                                    ((bool)GetPSObjectProperty(objDatabase, "IsSuspendedFromProvisioning") == false))
                                {
                                    string db = ObjToString(GetPSObjectProperty(objDatabase, "Identity"));

                                    bool bAdd = true;
                                    foreach (string s in lstDatabase)
                                    {
                                        if (s.ToLower() == db.ToLower())
                                        {
                                            bAdd = false;
                                            break;
                                        }
                                    }

                                    if (bAdd)
                                    {
                                        lstDatabase.Add(db);
                                        ExchangeLog.LogInfo("AddDatabase: " + db);
                                    }
                                }
                            }
                        }
                    }
                    else // new fixed database code
                    {
                        Collection<PSObject> databases = null;
                        cmd = new Command("Get-MailboxDatabase");
                        cmd.Parameters.Add("Identity", dagNameMBX);
                        databases = ExecuteShellCommand(runSpace, cmd);

                        // do not check "IsExcludedFromProvisioning" or "IsSuspended", just check if it is a member of the DAG
                        foreach (PSObject objDatabase in databases)
                        {
                            string dagSetting = ObjToString(GetPSObjectProperty(objDatabase, "MasterServerOrAvailabilityGroup"));
                            if (dagNameDAG.Equals(dagSetting, StringComparison.OrdinalIgnoreCase))
                            {
                                lstDatabase.Add(dagNameMBX);
                                ExchangeLog.LogInfo("AddFixedDatabase: " + dagNameMBX);
                            }
                        }
                    }

                    int balancer = (int)htBbalancer[dagName];
                    balancer++;
                    if (balancer >= lstDatabase.Count) balancer = 0;
                    htBbalancer[dagName] = balancer;
                    if (lstDatabase.Count != 0) database = lstDatabase[balancer];

                }
            }

            ExchangeLog.LogEnd("GetDatabase");
            return database;
        }


        #endregion

        #region AddressBook
        internal override void AdjustADSecurity(string objPath, string securityGroupPath, bool isAddressBook)
        {
            ExchangeLog.LogStart("AdjustADSecurity");
            ExchangeLog.DebugInfo("  Active Direcory object: {0}", objPath);
            ExchangeLog.DebugInfo("  Security Group: {0}", securityGroupPath);

            if (isAddressBook)
            {
                ExchangeLog.DebugInfo("  Updating Security");
                //"Download Address Book" security permission for offline address book
                Guid openAddressBookGuid = new Guid("{bd919c7c-2d79-4950-bc9c-e16fd99285e8}");

                DirectoryEntry groupEntry = GetADObject(securityGroupPath);
                byte[] byteSid = (byte[])GetADObjectProperty(groupEntry, "objectSid");

                DirectoryEntry objEntry = GetADObject(objPath);
                ActiveDirectorySecurity security = objEntry.ObjectSecurity;

                // Create a SecurityIdentifier object for security group.
                SecurityIdentifier groupSid = new SecurityIdentifier(byteSid, 0);

                // Create an access rule to allow users in Security Group to open address book. 
                ActiveDirectoryAccessRule allowOpenAddressBook =
                    new ActiveDirectoryAccessRule(
                        groupSid,
                        ActiveDirectoryRights.ExtendedRight,
                        AccessControlType.Allow,
                        openAddressBookGuid);

                // Create an access rule to allow users in Security Group to read object. 
                ActiveDirectoryAccessRule allowRead =
                    new ActiveDirectoryAccessRule(
                        groupSid,
                        ActiveDirectoryRights.GenericRead,
                        AccessControlType.Allow);

                // Remove existing rules if exist
                security.RemoveAccessRuleSpecific(allowOpenAddressBook);
                security.RemoveAccessRuleSpecific(allowRead);

                // Add a new access rule to allow users in Security Group to open address book. 
                security.AddAccessRule(allowOpenAddressBook);
                // Add a new access rule to allow users in Security Group to read object. 
                security.AddAccessRule(allowRead);

                // Commit the changes.
                objEntry.CommitChanges();
            }

            ExchangeLog.LogEnd("AdjustADSecurity");
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
                    if ((value == 2) | (value == 3)) bResult = true;
                }
                rk.Close();
            }
            return bResult;
        }
    }
}

