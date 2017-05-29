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

namespace SolidCP.EnterpriseServer
{
    public class Quotas
    {
        /*
select 'public const string ' + UPPER(REPLACE(q.QuotaName, '.', '_')) + '			= "' + 
q.QuotaName + '";		// ' + q.QuotaDescription
from quotas as q
inner join ResourceGroups as rg on q.groupid = rg.groupid
order by rg.groupOrder
         * */

		public const string OS_ODBC = "OS.ODBC";  // ODBC DSNs
		public const string OS_BANDWIDTH = "OS.Bandwidth";  // Bandwidth, MB
		public const string OS_DISKSPACE = "OS.Diskspace";  // Disk space, MB
		public const string OS_DOMAINS = "OS.Domains";  // Domains
		public const string OS_SUBDOMAINS = "OS.SubDomains";  // Sub-Domains
        public const string OS_DOMAINPOINTERS = "OS.DomainPointers"; // Domain Pointers
		public const string OS_FILEMANAGER = "OS.FileManager";  // File Manager
		public const string OS_SCHEDULEDTASKS = "OS.ScheduledTasks";  // Scheduled Tasks
		public const string OS_SCHEDULEDINTERVALTASKS = "OS.ScheduledIntervalTasks";  // Interval Tasks Allowed
		public const string OS_MINIMUMTASKINTERVAL = "OS.MinimumTaskInterval";  // Minimum Tasks Interval, minutes
		public const string OS_APPINSTALLER = "OS.AppInstaller";  // Applications Installer
		public const string OS_EXTRAAPPLICATIONS = "OS.ExtraApplications";  // Extra Application Packs
        public const string OS_NOTALLOWTENANTCREATEDOMAINS = "OS.AllowTenantCreateDomains";  // Do not Allow tenant to create top level domains
		public const string WEB_SITES = "Web.Sites";  // Web Sites
		public const string WEB_ASPNET11 = "Web.AspNet11";  // ASP.NET 1.1
		public const string WEB_ASPNET20 = "Web.AspNet20";  // ASP.NET 2.0
		public const string WEB_ASPNET40 = "Web.AspNet40";  // ASP.NET 4.0
		public const string WEB_ASP = "Web.Asp";  // ASP
		public const string WEB_PHP4 = "Web.Php4";  // PHP 4.x
		public const string WEB_PHP5 = "Web.Php5";  // PHP 5.x
		public const string WEB_PERL = "Web.Perl";  // Perl
		public const string WEB_PYTHON = "Web.Python";  // Python
		public const string WEB_VIRTUALDIRS = "Web.VirtualDirs";  // Virtual Directories
		public const string WEB_FRONTPAGE = "Web.FrontPage";  // FrontPage
		public const string WEB_SECURITY = "Web.Security";  // Custom Security Settings
		public const string WEB_DEFAULTDOCS = "Web.DefaultDocs";  // Custom Default Documents
		public const string WEB_APPPOOLS = "Web.AppPools";  // Dedicated Application Pools
        public const string WEB_APPPOOLSRESTART = "Web.AppPoolsRestart";  // Application Pools Restart
        public const string WEB_HEADERS = "Web.Headers";  // Custom Headers
		public const string WEB_ERRORS = "Web.Errors";  // Custom Errors
		public const string WEB_MIME = "Web.Mime";  // Custom MIME Types
		public const string WEB_CGIBIN = "Web.CgiBin";  // CGI-BIN Folder
		public const string WEB_SECUREDFOLDERS = "Web.SecuredFolders";  // Secured Folders
        public const string WEB_HTACCESS = "Web.Htaccess";  // Htaccess
        public const string WEB_SHAREDSSL = "Web.SharedSSL";  // Shared SSL Folders
		public const string WEB_REDIRECTIONS = "Web.Redirections";  // Web Sites Redirection
		public const string WEB_HOMEFOLDERS = "Web.HomeFolders";  // Changing Sites Root Folders
        public const string WEB_IP_ADDRESSES = "Web.IPAddresses";  // Dedicated IP Addresses
        public const string WEB_COLDFUSION = "Web.ColdFusion"; // ColdFusion
        public const string WEB_CFVIRTUALDIRS = "Web.CFVirtualDirectories"; //ColdFusion Virtual Directories
		public const string WEB_REMOTEMANAGEMENT = "Web.RemoteManagement"; //IIS 7 Remote Management
        public const string WEB_SSL = "Web.SSL"; //SSL
        public const string WEB_ALLOWIPADDRESSMODESWITCH = "Web.AllowIPAddressModeSwitch"; //Allow to switch IP Address Mode
        public const string WEB_ENABLEHOSTNAMESUPPORT = "Web.EnableHostNameSupport"; //Enable to specify hostnames upon site creation
		public const string FTP_ACCOUNTS = "FTP.Accounts";  // FTP Accounts
		public const string MAIL_ACCOUNTS = "Mail.Accounts";  // Mail Accounts
		public const string MAIL_FORWARDINGS = "Mail.Forwardings";  // Mail Forwardings
		public const string MAIL_LISTS = "Mail.Lists";  // Mail Lists
		public const string MAIL_GROUPS = "Mail.Groups";  // Mail Groups
		public const string MAIL_MAXBOXSIZE = "Mail.MaxBoxSize";  // Max Mailbox Size
		public const string MAIL_MAXGROUPMEMBERS = "Mail.MaxGroupMembers";  // Max Group Recipients
		public const string MAIL_MAXLISTMEMBERS = "Mail.MaxListMembers";  // Max List Recipients
        public const string MAIL_DISABLESIZEEDIT = "Mail.DisableSizeEdit"; // Disable Mailbox Size Edit
		public const string EXCHANGE2007_ORGANIZATIONS = "Exchange2007.Organizations";  // Exchange 2007 Organizations
		public const string EXCHANGE2007_DISKSPACE = "Exchange2007.DiskSpace";  // Organization Disk Space, MB
		public const string EXCHANGE2007_MAILBOXES = "Exchange2007.Mailboxes";  // Mailboxes per Organization
		public const string EXCHANGE2007_CONTACTS = "Exchange2007.Contacts";  // Contacts per Organization
		public const string EXCHANGE2007_DISTRIBUTIONLISTS = "Exchange2007.DistributionLists";  // Distribution Lists per Organization
		public const string EXCHANGE2007_PUBLICFOLDERS = "Exchange2007.PublicFolders";  // Public Folders per Organization
		public const string EXCHANGE2007_DOMAINS = "Exchange2007.Domains";  // Domains per Organization
		public const string EXCHANGE2007_POP3ALLOWED = "Exchange2007.POP3Allowed";  // POP3 Access
		public const string EXCHANGE2007_IMAPALLOWED = "Exchange2007.IMAPAllowed";  // IMAP Access
		public const string EXCHANGE2007_OWAALLOWED = "Exchange2007.OWAAllowed";  // OWA/HTTP Access
		public const string EXCHANGE2007_MAPIALLOWED = "Exchange2007.MAPIAllowed";  // MAPI Access
		public const string EXCHANGE2007_ACTIVESYNCALLOWED = "Exchange2007.ActiveSyncAllowed";  // ActiveSync Access
		public const string EXCHANGE2007_MAILENABLEDPUBLICFOLDERS = "Exchange2007.MailEnabledPublicFolders";  // Mail Enabled Public Folders Allowed
		public const string EXCHANGE2007_POP3ENABLED = "Exchange2007.POP3Enabled";  // POP3 Enabled by default
		public const string EXCHANGE2007_IMAPENABLED = "Exchange2007.IMAPEnabled";  // IMAP Enabled by default
		public const string EXCHANGE2007_OWAENABLED = "Exchange2007.OWAEnabled";  // OWA  Enabled by default
		public const string EXCHANGE2007_MAPIENABLED = "Exchange2007.MAPIEnabled";  // MAPI  Enabled by default
		public const string EXCHANGE2007_ACTIVESYNCENABLED = "Exchange2007.ActiveSyncEnabled";  // ActiveSync Enabled by default
        public const string EXCHANGE2007_KEEPDELETEDITEMSDAYS = "Exchange2007.KeepDeletedItemsDays";  // Keep deleted items
        public const string EXCHANGE2007_MAXRECIPIENTS = "Exchange2007.MaxRecipients";  // Max Recipients
        public const string EXCHANGE2007_MAXSENDMESSAGESIZEKB = "Exchange2007.MaxSendMessageSizeKB";  // Max Send Message Size
        public const string EXCHANGE2007_MAXRECEIVEMESSAGESIZEKB = "Exchange2007.MaxReceiveMessageSizeKB";  // Max Receive Message Size
        public const string EXCHANGE2007_ISCONSUMER = "Exchange2007.IsConsumer";  // Is Consumer Organization
        public const string EXCHANGE2007_ENABLEDPLANSEDITING = "Exchange2007.EnablePlansEditing";  // Enabled plans editing
        public const string EXCHANGE2007_ALLOWLITIGATIONHOLD = "Exchange2007.AllowLitigationHold";
        public const string EXCHANGE2007_RECOVERABLEITEMSSPACE = "Exchange2007.RecoverableItemsSpace";
        public const string EXCHANGE2007_DISCLAIMERSALLOWED = "Exchange2007.DisclaimersAllowed";

        public const string EXCHANGE2013_ALLOWRETENTIONPOLICY = "Exchange2013.AllowRetentionPolicy"; // RetentionPolicy
        public const string EXCHANGE2013_ARCHIVINGSTORAGE = "Exchange2013.ArchivingStorage"; // Archiving
        public const string EXCHANGE2013_ARCHIVINGMAILBOXES = "Exchange2013.ArchivingMailboxes";

        public const string EXCHANGE2013_SHAREDMAILBOXES = "Exchange2013.SharedMailboxes"; // Shared and resource mailboxes
        public const string EXCHANGE2013_RESOURCEMAILBOXES = "Exchange2013.ResourceMailboxes";

        public const string MSSQL2000_DATABASES = "MsSQL2000.Databases";  // Databases
		public const string MSSQL2000_USERS = "MsSQL2000.Users";  // Users
		public const string MSSQL2000_MAXDATABASESIZE = "MsSQL2000.MaxDatabaseSize";  // Max Database Size
		public const string MSSQL2000_BACKUP = "MsSQL2000.Backup";  // Database Backups
		public const string MSSQL2000_RESTORE = "MsSQL2000.Restore";  // Database Restores
		public const string MSSQL2000_TRUNCATE = "MsSQL2000.Truncate";  // Database Truncate
		public const string MSSQL2005_DATABASES = "MsSQL2005.Databases";  // Databases
		public const string MSSQL2005_USERS = "MsSQL2005.Users";  // Users
		public const string MSSQL2005_MAXDATABASESIZE = "MsSQL2005.MaxDatabaseSize";  // Max Database Size
		public const string MSSQL2005_BACKUP = "MsSQL2005.Backup";  // Database Backups
		public const string MSSQL2005_RESTORE = "MsSQL2005.Restore";  // Database Restores
		public const string MSSQL2005_TRUNCATE = "MsSQL2005.Truncate";  // Database Truncate
		public const string MYSQL4_DATABASES = "MySQL4.Databases";  // Databases
		public const string MYSQL4_USERS = "MySQL4.Users";  // Users
		public const string MYSQL4_BACKUP = "MySQL4.Backup";  // Database Backups
        public const string MYSQL4_RESTORE = "MySQL4.Restore";  // Database Restores
        public const string MYSQL4_MAXDATABASESIZE = "MySQL4.MaxDatabaseSize";  // Max Database Size
		public const string MYSQL5_DATABASES = "MySQL5.Databases";  // Databases
		public const string MYSQL5_USERS = "MySQL5.Users";  // Users
		public const string MYSQL5_BACKUP = "MySQL5.Backup";  // Database Backups
        public const string MYSQL5_RESTORE = "MySQL5.Restore";  // Database Restores
        public const string MYSQL5_MAXDATABASESIZE = "MySQL5.MaxDatabaseSize";  // Max Database Size
        public const string MARIADB_DATABASES = "MariaDB.Databases";  // Databases
        public const string MARIADB_USERS = "MariaDB.Users";  // Users
        public const string MARIADB_BACKUP = "MariaDB.Backup";  // Database Backups
        public const string MARIADB_RESTORE = "MariaDB.Restore";  // Database Restores
        public const string MARIADB_MAXDATABASESIZE = "MariaDB.MaxDatabaseSize";  // Max Database Size
        public const string SHAREPOINT_USERS = "SharePoint.Users";  // SharePoint Users
		public const string SHAREPOINT_GROUPS = "SharePoint.Groups";  // SharePoint Groups
		public const string SHAREPOINT_SITES = "SharePoint.Sites";  // SharePoint Sites
		public const string HOSTED_SHAREPOINT_SITES = "HostedSharePoint.Sites";  // Hosted SharePoint Sites
        public const string HOSTED_SHAREPOINT_STORAGE_SIZE = "HostedSharePoint.MaxStorage"; // Hosted SharePoint storage size;
        public const string HOSTED_SHAREPOINT_USESHAREDSSL = "HostedSharePoint.UseSharedSSL"; // Hosted SharePoint Use Shared SSL Root
        public const string HOSTED_SHAREPOINT_ENTERPRISE_SITES = "HostedSharePointEnterprise.Sites";  // Hosted SharePoint Sites
        public const string HOSTED_SHAREPOINT_ENTERPRISE_STORAGE_SIZE = "HostedSharePointEnterprise.MaxStorage"; // Hosted SharePoint storage size;
        public const string HOSTED_SHAREPOINT_ENTERPRISE_USESHAREDSSL = "HostedSharePointEnterprise.UseSharedSSL"; // Hosted SharePoint Use Shared SSL Root
        public const string DNS_EDITOR = "DNS.Editor";  // DNS Editor
        public const string DNS_ZONES = "DNS.Zones";  // DNS Editor
        public const string DNS_PRIMARY_ZONES = "DNS.PrimaryZones";  // DNS Editor
        public const string DNS_SECONDARY_ZONES = "DNS.SecondaryZones";  // DNS Editor
		public const string STATS_SITES = "Stats.Sites";  // Statistics Sites
        public const string ORGANIZATIONS = "HostedSolution.Organizations";
        public const string ORGANIZATION_USERS = "HostedSolution.Users";
        public const string ORGANIZATION_DELETED_USERS = "HostedSolution.DeletedUsers";
        public const string ORGANIZATION_DELETED_USERS_BACKUP_STORAGE_SPACE = "HostedSolution.DeletedUsersBackupStorageSpace";
        public const string ORGANIZATION_DOMAINS = "HostedSolution.Domains";
        public const string ORGANIZATION_ALLOWCHANGEUPN = "HostedSolution.AllowChangeUPN";
        public const string ORGANIZATION_SECURITYGROUPS = "HostedSolution.SecurityGroups";

        public const string CRM_USERS = "HostedCRM.Users";
        public const string CRM_ORGANIZATION = "HostedCRM.Organization";
        public const string CRM_LIMITEDUSERS = "HostedCRM.LimitedUsers";
        public const string CRM_ESSUSERS = "HostedCRM.ESSUsers";
        public const string CRM_MAXDATABASESIZE = "HostedCRM.MaxDatabaseSize";

        public const string CRM2013_ORGANIZATION = "HostedCRM2013.Organization";
        public const string CRM2013_MAXDATABASESIZE = "HostedCRM2013.MaxDatabaseSize";

        public const string CRM2013_ESSENTIALUSERS = "HostedCRM2013.EssentialUsers";
        public const string CRM2013_BASICUSERS = "HostedCRM2013.BasicUsers";
        public const string CRM2013_PROFESSIONALUSERS = "HostedCRM2013.ProfessionalUsers";


        public const string VPS_SERVERS_NUMBER = "VPS.ServersNumber";		// Number of VPS
        public const string VPS_MANAGING_ALLOWED = "VPS.ManagingAllowed";		// Allow user to create VPS
        public const string VPS_CPU_NUMBER = "VPS.CpuNumber";		// Number of CPU cores
        public const string VPS_BOOT_CD_ALLOWED = "VPS.BootCdAllowed";		// Boot from CD allowed
        public const string VPS_BOOT_CD_ENABLED = "VPS.BootCdEnabled";		// Boot from CD
        public const string VPS_RAM = "VPS.Ram";		// RAM size, MB
        public const string VPS_HDD = "VPS.Hdd";		// Hard Drive size, GB
        public const string VPS_DVD_ENABLED = "VPS.DvdEnabled";		// DVD drive
        public const string VPS_EXTERNAL_NETWORK_ENABLED = "VPS.ExternalNetworkEnabled";		// External Network
        public const string VPS_EXTERNAL_IP_ADDRESSES_NUMBER = "VPS.ExternalIPAddressesNumber";		// Number of External IP addresses
        public const string VPS_PRIVATE_NETWORK_ENABLED = "VPS.PrivateNetworkEnabled";		// Private Network
        public const string VPS_PRIVATE_IP_ADDRESSES_NUMBER = "VPS.PrivateIPAddressesNumber";		// Number of Private IP addresses per VPS
        public const string VPS_SNAPSHOTS_NUMBER = "VPS.SnapshotsNumber";		// Number of Snaphots
        public const string VPS_START_SHUTDOWN_ALLOWED = "VPS.StartShutdownAllowed";		// Allow user to Start, Turn off and Shutdown VPS
        public const string VPS_PAUSE_RESUME_ALLOWED = "VPS.PauseResumeAllowed";		// Allow user to Pause, Resume VPS
        public const string VPS_REBOOT_ALLOWED = "VPS.RebootAllowed";		// Allow user to Reboot VPS
        public const string VPS_RESET_ALOWED = "VPS.ResetAlowed";		// Allow user to Reset VPS
        public const string VPS_REINSTALL_ALLOWED = "VPS.ReinstallAllowed";		// Allow user to Re-install VPS
        public const string VPS_BANDWIDTH = "VPS.Bandwidth";		// Monthly bandwidth, GB

        public const string VPS2012_SERVERS_NUMBER = "VPS2012.ServersNumber";		// Number of VPS
        public const string VPS2012_MANAGING_ALLOWED = "VPS2012.ManagingAllowed";		// Allow user to create VPS
        public const string VPS2012_CPU_NUMBER = "VPS2012.CpuNumber";		// Number of CPU cores
        public const string VPS2012_BOOT_CD_ALLOWED = "VPS2012.BootCdAllowed";		// Boot from CD allowed
        public const string VPS2012_BOOT_CD_ENABLED = "VPS2012.BootCdEnabled";		// Boot from CD
        public const string VPS2012_RAM = "VPS2012.Ram";		// RAM size, MB
        public const string VPS2012_HDD = "VPS2012.Hdd";		// Hard Drive size, GB
        public const string VPS2012_DVD_ENABLED = "VPS2012.DvdEnabled";		// DVD drive
        public const string VPS2012_EXTERNAL_NETWORK_ENABLED = "VPS2012.ExternalNetworkEnabled";		// External Network
        public const string VPS2012_EXTERNAL_IP_ADDRESSES_NUMBER = "VPS2012.ExternalIPAddressesNumber";		// Number of External IP addresses
        public const string VPS2012_PRIVATE_NETWORK_ENABLED = "VPS2012.PrivateNetworkEnabled";		// Private Network
        public const string VPS2012_PRIVATE_IP_ADDRESSES_NUMBER = "VPS2012.PrivateIPAddressesNumber";		// Number of Private IP addresses per VPS
        public const string VPS2012_SNAPSHOTS_NUMBER = "VPS2012.SnapshotsNumber";		// Number of Snaphots
        public const string VPS2012_START_SHUTDOWN_ALLOWED = "VPS2012.StartShutdownAllowed";		// Allow user to Start, Turn off and Shutdown VPS
        public const string VPS2012_PAUSE_RESUME_ALLOWED = "VPS2012.PauseResumeAllowed";		// Allow user to Pause, Resume VPS
        public const string VPS2012_REBOOT_ALLOWED = "VPS2012.RebootAllowed";		// Allow user to Reboot VPS
        public const string VPS2012_RESET_ALOWED = "VPS2012.ResetAlowed";		// Allow user to Reset VPS
        public const string VPS2012_REINSTALL_ALLOWED = "VPS2012.ReinstallAllowed";		// Allow user to Re-install VPS
        public const string VPS2012_BANDWIDTH = "VPS2012.Bandwidth";		// Monthly bandwidth, GB
        public const string VPS2012_REPLICATION_ENABLED = "VPS2012.ReplicationEnabled";

		public const string VPSForPC_SERVERS_NUMBER = "VPSForPC.ServersNumber";		// Number of VPS
		public const string VPSForPC_MANAGING_ALLOWED = "VPSForPC.ManagingAllowed";		// Allow user to create VPS
		public const string VPSForPC_CPU_NUMBER = "VPSForPC.CpuNumber";		// Number of CPU cores
		public const string VPSForPC_BOOT_CD_ALLOWED = "VPSForPC.BootCdAllowed";		// Boot from CD allowed
		public const string VPSForPC_BOOT_CD_ENABLED = "VPSForPC.BootCdEnabled";		// Boot from CD
		public const string VPSForPC_RAM = "VPSForPC.Ram";		// RAM size, MB
		public const string VPSForPC_HDD = "VPSForPC.Hdd";		// Hard Drive size, GB
		public const string VPSForPC_DVD_ENABLED = "VPSForPC.DvdEnabled";		// DVD drive
		public const string VPSForPC_EXTERNAL_NETWORK_ENABLED = "VPSForPC.ExternalNetworkEnabled";		// External Network
		public const string VPSForPC_EXTERNAL_IP_ADDRESSES_NUMBER = "VPSForPC.ExternalIPAddressesNumber";		// Number of External IP addresses
		public const string VPSForPC_PRIVATE_NETWORK_ENABLED = "VPSForPC.PrivateNetworkEnabled";		// Private Network
		public const string VPSForPC_PRIVATE_IP_ADDRESSES_NUMBER = "VPSForPC.PrivateIPAddressesNumber";		// Number of Private IP addresses per VPS
		public const string VPSForPC_SNAPSHOTS_NUMBER = "VPSForPC.SnapshotsNumber";		// Number of Snaphots
		public const string VPSForPC_START_SHUTDOWN_ALLOWED = "VPSForPC.StartShutdownAllowed";		// Allow user to Start, Turn off and Shutdown VPS
		public const string VPSForPC_PAUSE_RESUME_ALLOWED = "VPSForPC.PauseResumeAllowed";		// Allow user to Pause, Resume VPS
		public const string VPSForPC_REBOOT_ALLOWED = "VPSForPC.RebootAllowed";		// Allow user to Reboot VPS
		public const string VPSForPC_RESET_ALOWED = "VPSForPC.ResetAlowed";		// Allow user to Reset VPS
		public const string VPSForPC_REINSTALL_ALLOWED = "VPSForPC.ReinstallAllowed";		// Allow user to Re-install VPS
		public const string VPSForPC_BANDWIDTH = "VPSForPC.Bandwidth";		// Monthly bandwidth, GB

        public const string PROXMOX_SERVERS_NUMBER = "PROXMOX.ServersNumber";		// Number of VPS
        public const string PROXMOX_MANAGING_ALLOWED = "PROXMOX.ManagingAllowed";		// Allow user to create VPS
        public const string PROXMOX_CPU_NUMBER = "PROXMOX.CpuNumber";		// Number of CPU cores
        public const string PROXMOX_BOOT_CD_ALLOWED = "PROXMOX.BootCdAllowed";		// Boot from CD allowed
        public const string PROXMOX_BOOT_CD_ENABLED = "PROXMOX.BootCdEnabled";		// Boot from CD
        public const string PROXMOX_RAM = "PROXMOX.Ram";		// RAM size, MB
        public const string PROXMOX_HDD = "PROXMOX.Hdd";		// Hard Drive size, GB
        public const string PROXMOX_DVD_ENABLED = "PROXMOX.DvdEnabled";		// DVD drive
        public const string PROXMOX_EXTERNAL_NETWORK_ENABLED = "PROXMOX.ExternalNetworkEnabled";		// External Network
        public const string PROXMOX_EXTERNAL_IP_ADDRESSES_NUMBER = "PROXMOX.ExternalIPAddressesNumber";		// Number of External IP addresses
        public const string PROXMOX_PRIVATE_NETWORK_ENABLED = "PROXMOX.PrivateNetworkEnabled";		// Private Network
        public const string PROXMOX_PRIVATE_IP_ADDRESSES_NUMBER = "PROXMOX.PrivateIPAddressesNumber";		// Number of Private IP addresses per VPS
        public const string PROXMOX_SNAPSHOTS_NUMBER = "PROXMOX.SnapshotsNumber";		// Number of Snaphots
        public const string PROXMOX_START_SHUTDOWN_ALLOWED = "PROXMOX.StartShutdownAllowed";		// Allow user to Start, Turn off and Shutdown VPS
        public const string PROXMOX_PAUSE_RESUME_ALLOWED = "PROXMOX.PauseResumeAllowed";		// Allow user to Pause, Resume VPS
        public const string PROXMOX_REBOOT_ALLOWED = "PROXMOX.RebootAllowed";		// Allow user to Reboot VPS
        public const string PROXMOX_RESET_ALOWED = "PROXMOX.ResetAlowed";		// Allow user to Reset VPS
        public const string PROXMOX_REINSTALL_ALLOWED = "PROXMOX.ReinstallAllowed";		// Allow user to Re-install VPS
        public const string PROXMOX_BANDWIDTH = "PROXMOX.Bandwidth";		// Monthly bandwidth, GB
        public const string PROXMOX_REPLICATION_ENABLED = "PROXMOX.ReplicationEnabled";

        public const string BLACKBERRY_USERS = "BlackBerry.Users";

        public const string OCS_USERS = "OCS.Users";
        
        public const string OCS_Federation = "OCS.Federation";
        public const string OCS_FederationByDefault = "OCS.FederationByDefault";
        
        public const string OCS_PublicIMConnectivity = "OCS.PublicIMConnectivity";
        public const string OCS_PublicIMConnectivityByDefault = "OCS.PublicIMConnectivityByDefault";
        
        public const string OCS_ArchiveIMConversation = "OCS.ArchiveIMConversation";
        public const string OCS_ArchiveIMConversationByDefault = "OCS.ArchiveIMConvervationByDefault";

        public const string OCS_ArchiveFederatedIMConversationByDefault = "OCS.ArchiveFederatedIMConversationByDefault";
        public const string OCS_ArchiveFederatedIMConversation = "OCS.ArchiveFederatedIMConversation";
        
        public const string OCS_PresenceAllowed = "OCS.PresenceAllowed";
        public const string OCS_PresenceAllowedByDefault = "OCS.PresenceAllowedByDefault";


        public const string LYNC_USERS = "Lync.Users";
        public const string LYNC_FEDERATION = "Lync.Federation";
        public const string LYNC_CONFERENCING = "Lync.Conferencing";
        public const string LYNC_MAXPARTICIPANTS = "Lync.MaxParticipants";
        public const string LYNC_ALLOWVIDEO = "Lync.AllowVideo";
        public const string LYNC_ENTERPRISEVOICE = "Lync.EnterpriseVoice";
        public const string LYNC_EVUSERS = "Lync.EVUsers";
        public const string LYNC_EVNATIONAL = "Lync.EVNational";
        public const string LYNC_EVMOBILE = "Lync.EVMobile";
        public const string LYNC_EVINTERNATIONAL = "Lync.EVInternational";
        public const string LYNC_ENABLEDPLANSEDITING = "Lync.EnablePlansEditing";
        public const string LYNC_PHONE = "Lync.PhoneNumbers";

        public const string SFB_USERS = "SfB.Users";
        public const string SFB_FEDERATION = "SfB.Federation";
        public const string SFB_CONFERENCING = "SfB.Conferencing";
        public const string SFB_MAXPARTICIPANTS = "SfB.MaxParticipants";
        public const string SFB_ALLOWVIDEO = "SfB.AllowVideo";
        public const string SFB_ENTERPRISEVOICE = "SfB.EnterpriseVoice";
        public const string SFB_EVUSERS = "SfB.EVUsers";
        public const string SFB_EVNATIONAL = "SfB.EVNational";
        public const string SFB_EVMOBILE = "SfB.EVMobile";
        public const string SFB_EVINTERNATIONAL = "SfB.EVInternational";
        public const string SFB_ENABLEDPLANSEDITING = "SfB.EnablePlansEditing";
        public const string SFB_PHONE = "SfB.PhoneNumbers";

        public const string HELICON_ZOO = "HeliconZoo.*";

        public const string ENTERPRISESTORAGE_DISKSTORAGESPACE = "EnterpriseStorage.DiskStorageSpace"; 
        public const string ENTERPRISESTORAGE_FOLDERS = "EnterpriseStorage.Folders";
        public const string ENTERPRICESTORAGE_DRIVEMAPS = "EnterpriseStorage.DriveMaps";

        public const string SERVICE_LEVELS = "ServiceLevel.";

        public const string RDS_USERS = "RDS.Users";
        public const string RDS_SERVERS = "RDS.Servers";
        public const string RDS_COLLECTIONS = "RDS.Collections";
    }
}
