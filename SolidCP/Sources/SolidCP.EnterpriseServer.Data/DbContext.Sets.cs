using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidCP.EnterpriseServer.Data.Entities;

namespace SolidCP.EnterpriseServer.Data
{
	using Version = SolidCP.EnterpriseServer.Data.Entities.Version;
	using GlobalDnsRecord = SolidCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;
	using BackgroundTask = SolidCP.EnterpriseServer.Data.Entities.BackgroundTask;
	using BackgroundTaskParameter = SolidCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

	public partial class DbContext
	{
		DbSet<AccessToken> accessTokens = null;
		public virtual DbSet<AccessToken> AccessTokens => accessTokens ?? (accessTokens = new DbSet<AccessToken>(BaseContext));

		DbSet<AdditionalGroup> additionalGroups = null;
		public virtual DbSet<AdditionalGroup> AdditionalGroups => additionalGroups ?? (additionalGroups = new DbSet<AdditionalGroup>(BaseContext));

		DbSet<AuditLog> auditLogs = null;
		public virtual DbSet<AuditLog> AuditLogs => auditLogs ?? (auditLogs = new DbSet<AuditLog>(BaseContext));

		DbSet<AuditLogSource> auditLogSources = null;
		public virtual DbSet<AuditLogSource> AuditLogSources => auditLogSources ?? (auditLogSources = new DbSet<AuditLogSource>(BaseContext));

		DbSet<AuditLogTask> auditLogTasks = null;
		public virtual DbSet<AuditLogTask> AuditLogTasks => auditLogTasks ?? (auditLogTasks = new DbSet<AuditLogTask>(BaseContext));

		DbSet<BackgroundTask> backgroundTasks = null;
		public virtual DbSet<BackgroundTask> BackgroundTasks => backgroundTasks ?? (backgroundTasks = new DbSet<BackgroundTask>(BaseContext));

		DbSet<BackgroundTaskLog> backgroundTaskLogs = null;
		public virtual DbSet<BackgroundTaskLog> BackgroundTaskLogs => backgroundTaskLogs ?? (backgroundTaskLogs = new DbSet<BackgroundTaskLog>(BaseContext));

		DbSet<BackgroundTaskParameter> backgroundTaskParameters = null;
		public virtual DbSet<BackgroundTaskParameter> BackgroundTaskParameters => backgroundTaskParameters ?? (backgroundTaskParameters = new DbSet<BackgroundTaskParameter>(BaseContext));

		DbSet<BackgroundTaskStack> backgroundTaskStacks = null;
		public virtual DbSet<BackgroundTaskStack> BackgroundTaskStacks => backgroundTaskStacks ?? (backgroundTaskStacks = new DbSet<BackgroundTaskStack>(BaseContext));

		DbSet<BlackBerryUser> blackBerryUsers = null;
		public virtual DbSet<BlackBerryUser> BlackBerryUsers => blackBerryUsers ?? (blackBerryUsers = new DbSet<BlackBerryUser>(BaseContext));

		DbSet<Cluster> clusters = null;
		public virtual DbSet<Cluster> Clusters => clusters ?? (clusters = new DbSet<Cluster>(BaseContext));

		DbSet<Comment> comments = null;
		public virtual DbSet<Comment> Comments => comments ?? (comments = new DbSet<Comment>(BaseContext));

		DbSet<CrmUser> crmusers = null;
		public virtual DbSet<CrmUser> Crmusers => crmusers ?? (crmusers = new DbSet<CrmUser>(BaseContext));

		DbSet<Domain> domains = null;
		public virtual DbSet<Domain> Domains => domains ?? (domains = new DbSet<Domain>(BaseContext));

		DbSet<DomainDnsRecord> domainDnsRecords = null;
		public virtual DbSet<DomainDnsRecord> DomainDnsRecords => domainDnsRecords ?? (domainDnsRecords = new DbSet<DomainDnsRecord>(BaseContext));

		DbSet<EnterpriseFolder> enterpriseFolders = null;
		public virtual DbSet<EnterpriseFolder> EnterpriseFolders => enterpriseFolders ?? (enterpriseFolders = new DbSet<EnterpriseFolder>(BaseContext));

		DbSet<EnterpriseFoldersOwaPermission> enterpriseFoldersOwaPermissions = null;
		public virtual DbSet<EnterpriseFoldersOwaPermission> EnterpriseFoldersOwaPermissions => enterpriseFoldersOwaPermissions ?? (enterpriseFoldersOwaPermissions = new DbSet<EnterpriseFoldersOwaPermission>(BaseContext));

		DbSet<ExchangeAccount> exchangeAccounts = null;
		public virtual DbSet<ExchangeAccount> ExchangeAccounts => exchangeAccounts ?? (exchangeAccounts = new DbSet<ExchangeAccount>(BaseContext));

		DbSet<ExchangeAccountEmailAddress> exchangeAccountEmailAddresses = null;
		public virtual DbSet<ExchangeAccountEmailAddress> ExchangeAccountEmailAddresses => exchangeAccountEmailAddresses ?? (exchangeAccountEmailAddresses = new DbSet<ExchangeAccountEmailAddress>(BaseContext));

		DbSet<ExchangeDeletedAccount> exchangeDeletedAccounts = null;
		public virtual DbSet<ExchangeDeletedAccount> ExchangeDeletedAccounts => exchangeDeletedAccounts ?? (exchangeDeletedAccounts = new DbSet<ExchangeDeletedAccount>(BaseContext));

		DbSet<ExchangeDisclaimer> exchangeDisclaimers = null;
		public virtual DbSet<ExchangeDisclaimer> ExchangeDisclaimers => exchangeDisclaimers ?? (exchangeDisclaimers = new DbSet<ExchangeDisclaimer>(BaseContext));

		DbSet<ExchangeMailboxPlan> exchangeMailboxPlans = null;
		public virtual DbSet<ExchangeMailboxPlan> ExchangeMailboxPlans => exchangeMailboxPlans ?? (exchangeMailboxPlans = new DbSet<ExchangeMailboxPlan>(BaseContext));

		DbSet<ExchangeMailboxPlanRetentionPolicyTag> exchangeMailboxPlanRetentionPolicyTags = null;
		public virtual DbSet<ExchangeMailboxPlanRetentionPolicyTag> ExchangeMailboxPlanRetentionPolicyTags => exchangeMailboxPlanRetentionPolicyTags ?? (exchangeMailboxPlanRetentionPolicyTags = new DbSet<ExchangeMailboxPlanRetentionPolicyTag>(BaseContext));

		DbSet<ExchangeOrganization> exchangeOrganizations = null;
		public virtual DbSet<ExchangeOrganization> ExchangeOrganizations => exchangeOrganizations ?? (exchangeOrganizations = new DbSet<ExchangeOrganization>(BaseContext));

		DbSet<ExchangeOrganizationDomain> exchangeOrganizationDomains = null;
		public virtual DbSet<ExchangeOrganizationDomain> ExchangeOrganizationDomains => exchangeOrganizationDomains ?? (exchangeOrganizationDomains = new DbSet<ExchangeOrganizationDomain>(BaseContext));

		DbSet<ExchangeOrganizationSetting> exchangeOrganizationSettings = null;
		public virtual DbSet<ExchangeOrganizationSetting> ExchangeOrganizationSettings => exchangeOrganizationSettings ?? (exchangeOrganizationSettings = new DbSet<ExchangeOrganizationSetting>(BaseContext));

		DbSet<ExchangeOrganizationSsFolder> exchangeOrganizationSsFolders = null;
		public virtual DbSet<ExchangeOrganizationSsFolder> ExchangeOrganizationSsFolders => exchangeOrganizationSsFolders ?? (exchangeOrganizationSsFolders = new DbSet<ExchangeOrganizationSsFolder>(BaseContext));

		DbSet<ExchangeRetentionPolicyTag> exchangeRetentionPolicyTags = null;
		public virtual DbSet<ExchangeRetentionPolicyTag> ExchangeRetentionPolicyTags => exchangeRetentionPolicyTags ?? (exchangeRetentionPolicyTags = new DbSet<ExchangeRetentionPolicyTag>(BaseContext));

		DbSet<GlobalDnsRecord> globalDnsRecords = null;
		public virtual DbSet<GlobalDnsRecord> GlobalDnsRecords => globalDnsRecords ?? (globalDnsRecords = new DbSet<GlobalDnsRecord>(BaseContext));

		DbSet<HostingPlan> hostingPlans = null;
		public virtual DbSet<HostingPlan> HostingPlans => hostingPlans ?? (hostingPlans = new DbSet<HostingPlan>(BaseContext));

		DbSet<HostingPlanQuota> hostingPlanQuotas = null;
		public virtual DbSet<HostingPlanQuota> HostingPlanQuotas => hostingPlanQuotas ?? (hostingPlanQuotas = new DbSet<HostingPlanQuota>(BaseContext));

		DbSet<HostingPlanResource> hostingPlanResources = null;
		public virtual DbSet<HostingPlanResource> HostingPlanResources => hostingPlanResources ?? (hostingPlanResources = new DbSet<HostingPlanResource>(BaseContext));

		DbSet<IpAddress> ipaddresses = null;
		public virtual DbSet<IpAddress> Ipaddresses => ipaddresses ?? (ipaddresses = new DbSet<IpAddress>(BaseContext));

		DbSet<LyncUser> lyncUsers = null;
		public virtual DbSet<LyncUser> LyncUsers => lyncUsers ?? (lyncUsers = new DbSet<LyncUser>(BaseContext));

		DbSet<LyncUserPlan> lyncUserPlans = null;
		public virtual DbSet<LyncUserPlan> LyncUserPlans => lyncUserPlans ?? (lyncUserPlans = new DbSet<LyncUserPlan>(BaseContext));

		DbSet<OcsUser> ocsusers = null;
		public virtual DbSet<OcsUser> Ocsusers => ocsusers ?? (ocsusers = new DbSet<OcsUser>(BaseContext));

		DbSet<Package> packages = null;
		public virtual DbSet<Package> Packages => packages ?? (packages = new DbSet<Package>(BaseContext));

		DbSet<PackageAddon> packageAddons = null;
		public virtual DbSet<PackageAddon> PackageAddons => packageAddons ?? (packageAddons = new DbSet<PackageAddon>(BaseContext));

		DbSet<PackageIpAddress> packageIpaddresses = null;
		public virtual DbSet<PackageIpAddress> PackageIpaddresses => packageIpaddresses ?? (packageIpaddresses = new DbSet<PackageIpAddress>(BaseContext));

		DbSet<PackageQuota> packageQuotas = null;
		public virtual DbSet<PackageQuota> PackageQuotas => packageQuotas ?? (packageQuotas = new DbSet<PackageQuota>(BaseContext));

		DbSet<PackageResource> packageResources = null;
		public virtual DbSet<PackageResource> PackageResources => packageResources ?? (packageResources = new DbSet<PackageResource>(BaseContext));

		DbSet<PackageSetting> packageSettings = null;
		public virtual DbSet<PackageSetting> PackageSettings => packageSettings ?? (packageSettings = new DbSet<PackageSetting>(BaseContext));

		DbSet<PackageVlan> packageVlans = null;
		public virtual DbSet<PackageVlan> PackageVlans => packageVlans ?? (packageVlans = new DbSet<PackageVlan>(BaseContext));

		DbSet<PackagesBandwidth> packagesBandwidths = null;
		public virtual DbSet<PackagesBandwidth> PackagesBandwidths => packagesBandwidths ?? (packagesBandwidths = new DbSet<PackagesBandwidth>(BaseContext));

		DbSet<PackagesDiskspace> packagesDiskspaces = null;
		public virtual DbSet<PackagesDiskspace> PackagesDiskspaces => packagesDiskspaces ?? (packagesDiskspaces = new DbSet<PackagesDiskspace>(BaseContext));

		DbSet<PackagesTreeCache> packagesTreeCaches = null;
		public virtual DbSet<PackagesTreeCache> PackagesTreeCaches => packagesTreeCaches ?? (packagesTreeCaches = new DbSet<PackagesTreeCache>(BaseContext));

		DbSet<PrivateIpAddress> privateIpaddresses = null;
		public virtual DbSet<PrivateIpAddress> PrivateIpaddresses => privateIpaddresses ?? (privateIpaddresses = new DbSet<PrivateIpAddress>(BaseContext));

		DbSet<PrivateNetworkVlan> privateNetworkVlans = null;
		public virtual DbSet<PrivateNetworkVlan> PrivateNetworkVlans => privateNetworkVlans ?? (privateNetworkVlans = new DbSet<PrivateNetworkVlan>(BaseContext));

		DbSet<Provider> providers = null;
		public virtual DbSet<Provider> Providers => providers ?? (providers = new DbSet<Provider>(BaseContext));

		DbSet<Quota> quotas = null;
		public virtual DbSet<Quota> Quotas => quotas ?? (quotas = new DbSet<Quota>(BaseContext));

		DbSet<RdsCertificate> rdscertificates = null;
		public virtual DbSet<RdsCertificate> Rdscertificates => rdscertificates ?? (rdscertificates = new DbSet<RdsCertificate>(BaseContext));

		DbSet<RdsCollection> rdscollections = null;
		public virtual DbSet<RdsCollection> Rdscollections => rdscollections ?? (rdscollections = new DbSet<RdsCollection>(BaseContext));

		DbSet<RdsCollectionSetting> rdscollectionSettings = null;
		public virtual DbSet<RdsCollectionSetting> RdscollectionSettings => rdscollectionSettings ?? (rdscollectionSettings = new DbSet<RdsCollectionSetting>(BaseContext));

		DbSet<RdsCollectionUser> rdscollectionUsers = null;
		public virtual DbSet<RdsCollectionUser> RdscollectionUsers => rdscollectionUsers ?? (rdscollectionUsers = new DbSet<RdsCollectionUser>(BaseContext));

		DbSet<RdsMessage> rdsmessages = null;
		public virtual DbSet<RdsMessage> Rdsmessages => rdsmessages ?? (rdsmessages = new DbSet<RdsMessage>(BaseContext));

		DbSet<RdsServer> rdsservers = null;
		public virtual DbSet<RdsServer> Rdsservers => rdsservers ?? (rdsservers = new DbSet<RdsServer>(BaseContext));

		DbSet<RdsServerSetting> rdsserverSettings = null;
		public virtual DbSet<RdsServerSetting> RdsserverSettings => rdsserverSettings ?? (rdsserverSettings = new DbSet<RdsServerSetting>(BaseContext));

		DbSet<ResourceGroup> resourceGroups = null;
		public virtual DbSet<ResourceGroup> ResourceGroups => resourceGroups ?? (resourceGroups = new DbSet<ResourceGroup>(BaseContext));

		DbSet<ResourceGroupDnsRecord> resourceGroupDnsRecords = null;
		public virtual DbSet<ResourceGroupDnsRecord> ResourceGroupDnsRecords => resourceGroupDnsRecords ?? (resourceGroupDnsRecords = new DbSet<ResourceGroupDnsRecord>(BaseContext));

		DbSet<Schedule> schedules = null;
		public virtual DbSet<Schedule> Schedules => schedules ?? (schedules = new DbSet<Schedule>(BaseContext));

		DbSet<ScheduleParameter> scheduleParameters = null;
		public virtual DbSet<ScheduleParameter> ScheduleParameters => scheduleParameters ?? (scheduleParameters = new DbSet<ScheduleParameter>(BaseContext));

		DbSet<ScheduleTask> scheduleTasks = null;
		public virtual DbSet<ScheduleTask> ScheduleTasks => scheduleTasks ?? (scheduleTasks = new DbSet<ScheduleTask>(BaseContext));

		DbSet<ScheduleTaskParameter> scheduleTaskParameters = null;
		public virtual DbSet<ScheduleTaskParameter> ScheduleTaskParameters => scheduleTaskParameters ?? (scheduleTaskParameters = new DbSet<ScheduleTaskParameter>(BaseContext));

		DbSet<ScheduleTaskViewConfiguration> scheduleTaskViewConfigurations = null;
		public virtual DbSet<ScheduleTaskViewConfiguration> ScheduleTaskViewConfigurations => scheduleTaskViewConfigurations ?? (scheduleTaskViewConfigurations = new DbSet<ScheduleTaskViewConfiguration>(BaseContext));

		DbSet<Server> servers = null;
		public virtual DbSet<Server> Servers => servers ?? (servers = new DbSet<Server>(BaseContext));

		DbSet<Service> services = null;
		public virtual DbSet<Service> Services => services ?? (services = new DbSet<Service>(BaseContext));

		DbSet<ServiceDefaultProperty> serviceDefaultProperties = null;
		public virtual DbSet<ServiceDefaultProperty> ServiceDefaultProperties => serviceDefaultProperties ?? (serviceDefaultProperties = new DbSet<ServiceDefaultProperty>(BaseContext));

		DbSet<ServiceItem> serviceItems = null;
		public virtual DbSet<ServiceItem> ServiceItems => serviceItems ?? (serviceItems = new DbSet<ServiceItem>(BaseContext));

		DbSet<ServiceItemProperty> serviceItemProperties = null;
		public virtual DbSet<ServiceItemProperty> ServiceItemProperties => serviceItemProperties ?? (serviceItemProperties = new DbSet<ServiceItemProperty>(BaseContext));

		DbSet<ServiceItemType> serviceItemTypes = null;
		public virtual DbSet<ServiceItemType> ServiceItemTypes => serviceItemTypes ?? (serviceItemTypes = new DbSet<ServiceItemType>(BaseContext));

		DbSet<ServiceProperty> serviceProperties = null;
		public virtual DbSet<ServiceProperty> ServiceProperties => serviceProperties ?? (serviceProperties = new DbSet<ServiceProperty>(BaseContext));

		DbSet<SfBuser> sfBusers = null;
		public virtual DbSet<SfBuser> SfBusers => sfBusers ?? (sfBusers = new DbSet<SfBuser>(BaseContext));

		DbSet<SfBuserPlan> sfBuserPlans = null;
		public virtual DbSet<SfBuserPlan> SfBuserPlans => sfBuserPlans ?? (sfBuserPlans = new DbSet<SfBuserPlan>(BaseContext));

		DbSet<SslCertificate> sslcertificates = null;
		public virtual DbSet<SslCertificate> Sslcertificates => sslcertificates ?? (sslcertificates = new DbSet<SslCertificate>(BaseContext));

		DbSet<StorageSpace> storageSpaces = null;
		public virtual DbSet<StorageSpace> StorageSpaces => storageSpaces ?? (storageSpaces = new DbSet<StorageSpace>(BaseContext));

		DbSet<StorageSpaceFolder> storageSpaceFolders = null;
		public virtual DbSet<StorageSpaceFolder> StorageSpaceFolders => storageSpaceFolders ?? (storageSpaceFolders = new DbSet<StorageSpaceFolder>(BaseContext));

		DbSet<StorageSpaceLevel> storageSpaceLevels = null;
		public virtual DbSet<StorageSpaceLevel> StorageSpaceLevels => storageSpaceLevels ?? (storageSpaceLevels = new DbSet<StorageSpaceLevel>(BaseContext));

		DbSet<StorageSpaceLevelResourceGroup> storageSpaceLevelResourceGroups = null;
		public virtual DbSet<StorageSpaceLevelResourceGroup> StorageSpaceLevelResourceGroups => storageSpaceLevelResourceGroups ?? (storageSpaceLevelResourceGroups = new DbSet<StorageSpaceLevelResourceGroup>(BaseContext));

		DbSet<SupportServiceLevel> supportServiceLevels = null;
		public virtual DbSet<SupportServiceLevel> SupportServiceLevels => supportServiceLevels ?? (supportServiceLevels = new DbSet<SupportServiceLevel>(BaseContext));

		DbSet<SystemSetting> systemSettings = null;
		public virtual DbSet<SystemSetting> SystemSettings => systemSettings ?? (systemSettings = new DbSet<SystemSetting>(BaseContext));

		DbSet<Theme> themes = null;
		public virtual DbSet<Theme> Themes => themes ?? (themes = new DbSet<Theme>(BaseContext));

		DbSet<ThemeSetting> themeSettings = null;
		public virtual DbSet<ThemeSetting> ThemeSettings => themeSettings ?? (themeSettings = new DbSet<ThemeSetting>(BaseContext));

		DbSet<User> users = null;
		public virtual DbSet<User> Users => users ?? (users = new DbSet<User>(BaseContext));

		DbSet<UserSetting> userSettings = null;
		public virtual DbSet<UserSetting> UserSettings => userSettings ?? (userSettings = new DbSet<UserSetting>(BaseContext));

		DbSet<UsersDetailed> usersDetaileds = null;
		public virtual DbSet<UsersDetailed> UsersDetaileds => usersDetaileds ?? (usersDetaileds = new DbSet<UsersDetailed>(BaseContext));

		DbSet<Version> versions = null;
		public virtual DbSet<Version> Versions => versions ?? (versions = new DbSet<Version>(BaseContext));

		DbSet<VirtualGroup> virtualGroups = null;
		public virtual DbSet<VirtualGroup> VirtualGroups => virtualGroups ?? (virtualGroups = new DbSet<VirtualGroup>(BaseContext));

		DbSet<VirtualService> virtualServices = null;
		public virtual DbSet<VirtualService> VirtualServices => virtualServices ?? (virtualServices = new DbSet<VirtualService>(BaseContext));

		DbSet<WebDavAccessToken> webDavAccessTokens = null;
		public virtual DbSet<WebDavAccessToken> WebDavAccessTokens => webDavAccessTokens ?? (webDavAccessTokens = new DbSet<WebDavAccessToken>(BaseContext));

		DbSet<WebDavPortalUsersSetting> webDavPortalUsersSettings = null;
		public virtual DbSet<WebDavPortalUsersSetting> WebDavPortalUsersSettings => webDavPortalUsersSettings ?? (webDavPortalUsersSettings = new DbSet<WebDavPortalUsersSetting>(BaseContext));
	}
}
