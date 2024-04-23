#if ScaffoldedDbContext
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;

#if NetCore
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
#endif
#if NetFX
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
#endif

namespace SolidCP.EnterpriseServer.Context
{

	using Version = SolidCP.EnterpriseServer.Data.Entities.Version;
	using GlobalDnsRecord = SolidCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;
	using BackgroundTask = SolidCP.EnterpriseServer.Data.Entities.BackgroundTask;
	using BackgroundTaskParameter = SolidCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

	public partial class DbContextBase : DbContext, Data.IGenericDbContext
	{

#if NetCore
		public DbContextBase(Data.DbContext context) : this(new Data.DbOptions<DbContextBase>(context)) { }
		public DbContextBase(DbContextOptions<DbContextBase> options) : base(options)
		{
            if (options is Data.DbOptions<DbContextBase> opts)
            {
                DbType = opts.DbType;
                InitSeedData = opts.InitSeedData;
            }
		}
#elif NetFX
        public DbContextBase(Data.DbContext context): base(context.ConnectionString) { 
            DbType = context.DbType;
        }
#endif

		public Data.DbType DbType { get; set; } = Data.DbType.Unknown;
        public bool InitSeedData { get; set; } = false;

		public virtual DbSet<AccessToken> AccessTokens { get; set; }

		public virtual DbSet<AdditionalGroup> AdditionalGroups { get; set; }

		public virtual DbSet<AuditLog> AuditLogs { get; set; }

		public virtual DbSet<AuditLogSource> AuditLogSources { get; set; }

		public virtual DbSet<AuditLogTask> AuditLogTasks { get; set; }

		public virtual DbSet<BackgroundTask> BackgroundTasks { get; set; }

		public virtual DbSet<BackgroundTaskLog> BackgroundTaskLogs { get; set; }

		public virtual DbSet<BackgroundTaskParameter> BackgroundTaskParameters { get; set; }

		public virtual DbSet<BackgroundTaskStack> BackgroundTaskStacks { get; set; }

		public virtual DbSet<BlackBerryUser> BlackBerryUsers { get; set; }

		public virtual DbSet<Cluster> Clusters { get; set; }

		public virtual DbSet<Comment> Comments { get; set; }

		public virtual DbSet<Crmuser> Crmusers { get; set; }

		public virtual DbSet<Domain> Domains { get; set; }

		public virtual DbSet<DomainDnsRecord> DomainDnsRecords { get; set; }

		public virtual DbSet<EnterpriseFolder> EnterpriseFolders { get; set; }

		public virtual DbSet<EnterpriseFoldersOwaPermission> EnterpriseFoldersOwaPermissions { get; set; }

		public virtual DbSet<ExchangeAccount> ExchangeAccounts { get; set; }

		public virtual DbSet<ExchangeAccountEmailAddress> ExchangeAccountEmailAddresses { get; set; }

		public virtual DbSet<ExchangeDeletedAccount> ExchangeDeletedAccounts { get; set; }

		public virtual DbSet<ExchangeDisclaimer> ExchangeDisclaimers { get; set; }

		public virtual DbSet<ExchangeMailboxPlan> ExchangeMailboxPlans { get; set; }

		public virtual DbSet<ExchangeMailboxPlanRetentionPolicyTag> ExchangeMailboxPlanRetentionPolicyTags { get; set; }

		public virtual DbSet<ExchangeOrganization> ExchangeOrganizations { get; set; }

		public virtual DbSet<ExchangeOrganizationDomain> ExchangeOrganizationDomains { get; set; }

		public virtual DbSet<ExchangeOrganizationSetting> ExchangeOrganizationSettings { get; set; }

		public virtual DbSet<ExchangeOrganizationSsFolder> ExchangeOrganizationSsFolders { get; set; }

		public virtual DbSet<ExchangeRetentionPolicyTag> ExchangeRetentionPolicyTags { get; set; }

		public virtual DbSet<GlobalDnsRecord> GlobalDnsRecords { get; set; }

		public virtual DbSet<HostingPlan> HostingPlans { get; set; }

		public virtual DbSet<HostingPlanQuota> HostingPlanQuotas { get; set; }

		public virtual DbSet<HostingPlanResource> HostingPlanResources { get; set; }

		public virtual DbSet<Ipaddress> Ipaddresses { get; set; }

		public virtual DbSet<LyncUser> LyncUsers { get; set; }

		public virtual DbSet<LyncUserPlan> LyncUserPlans { get; set; }

		public virtual DbSet<Ocsuser> Ocsusers { get; set; }

		public virtual DbSet<Package> Packages { get; set; }

		public virtual DbSet<PackageAddon> PackageAddons { get; set; }

		public virtual DbSet<PackageIpaddress> PackageIpaddresses { get; set; }

		public virtual DbSet<PackageQuota> PackageQuotas { get; set; }

		public virtual DbSet<PackageResource> PackageResources { get; set; }

		public virtual DbSet<PackageSetting> PackageSettings { get; set; }

		public virtual DbSet<PackageVlan> PackageVlans { get; set; }

		public virtual DbSet<PackagesBandwidth> PackagesBandwidths { get; set; }

		public virtual DbSet<PackagesDiskspace> PackagesDiskspaces { get; set; }

		public virtual DbSet<PackagesTreeCache> PackagesTreeCaches { get; set; }

		public virtual DbSet<PrivateIpaddress> PrivateIpaddresses { get; set; }

		public virtual DbSet<PrivateNetworkVlan> PrivateNetworkVlans { get; set; }

		public virtual DbSet<Provider> Providers { get; set; }

		public virtual DbSet<Quota> Quotas { get; set; }

		public virtual DbSet<Rdscertificate> Rdscertificates { get; set; }

		public virtual DbSet<Rdscollection> Rdscollections { get; set; }

		public virtual DbSet<RdscollectionSetting> RdscollectionSettings { get; set; }

		public virtual DbSet<RdscollectionUser> RdscollectionUsers { get; set; }

		public virtual DbSet<Rdsmessage> Rdsmessages { get; set; }

		public virtual DbSet<Rdsserver> Rdsservers { get; set; }

		public virtual DbSet<RdsserverSetting> RdsserverSettings { get; set; }

		public virtual DbSet<ResourceGroup> ResourceGroups { get; set; }

		public virtual DbSet<ResourceGroupDnsRecord> ResourceGroupDnsRecords { get; set; }

		public virtual DbSet<Schedule> Schedules { get; set; }

		public virtual DbSet<ScheduleParameter> ScheduleParameters { get; set; }

		public virtual DbSet<ScheduleTask> ScheduleTasks { get; set; }

		public virtual DbSet<ScheduleTaskParameter> ScheduleTaskParameters { get; set; }

		public virtual DbSet<ScheduleTaskViewConfiguration> ScheduleTaskViewConfigurations { get; set; }

		public virtual DbSet<Server> Servers { get; set; }

		public virtual DbSet<Service> Services { get; set; }

		public virtual DbSet<ServiceDefaultProperty> ServiceDefaultProperties { get; set; }

		public virtual DbSet<ServiceItem> ServiceItems { get; set; }

		public virtual DbSet<ServiceItemProperty> ServiceItemProperties { get; set; }

		public virtual DbSet<ServiceItemType> ServiceItemTypes { get; set; }

		public virtual DbSet<ServiceProperty> ServiceProperties { get; set; }

		public virtual DbSet<SfBuser> SfBusers { get; set; }

		public virtual DbSet<SfBuserPlan> SfBuserPlans { get; set; }

		public virtual DbSet<Sslcertificate> Sslcertificates { get; set; }

		public virtual DbSet<StorageSpace> StorageSpaces { get; set; }

		public virtual DbSet<StorageSpaceFolder> StorageSpaceFolders { get; set; }

		public virtual DbSet<StorageSpaceLevel> StorageSpaceLevels { get; set; }

		public virtual DbSet<StorageSpaceLevelResourceGroup> StorageSpaceLevelResourceGroups { get; set; }

		public virtual DbSet<SupportServiceLevel> SupportServiceLevels { get; set; }

		public virtual DbSet<SystemSetting> SystemSettings { get; set; }

		public virtual DbSet<Theme> Themes { get; set; }

		public virtual DbSet<ThemeSetting> ThemeSettings { get; set; }

		public virtual DbSet<User> Users { get; set; }

		public virtual DbSet<UserSetting> UserSettings { get; set; }

		public virtual DbSet<UsersDetailed> UsersDetaileds { get; set; }

		public virtual DbSet<Version> Versions { get; set; }

		public virtual DbSet<VirtualGroup> VirtualGroups { get; set; }

		public virtual DbSet<VirtualService> VirtualServices { get; set; }

		public virtual DbSet<WebDavAccessToken> WebDavAccessTokens { get; set; }

		public virtual DbSet<WebDavPortalUsersSetting> WebDavPortalUsersSettings { get; set; }

#if NetCore
		static ConcurrentDictionary<string, ServerVersion> serverVersions = new ConcurrentDictionary<string, ServerVersion>();
		protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			if (!(builder.Options is Data.DbOptions<DbContextBase> options)) throw new NotSupportedException("This type of Options is not supported");

            InitSeedData = options.InitSeedData;

			switch (options.DbType)
			{
				case Data.DbType.MsSql:
					builder.UseSqlServer(options.ConnectionString);
					break;
				case Data.DbType.Sqlite:
					builder.UseSqlite(options.ConnectionString);
					break;
				case Data.DbType.MySql:
				case Data.DbType.MariaDb:
					ServerVersion serverVersion = serverVersions.GetOrAdd(options.ConnectionString, connectionString => ServerVersion.AutoDetect(connectionString));
					builder.UseMySql(options.ConnectionString, serverVersion);
					break;
				case Data.DbType.PostgreSql:
					builder.UseNpgsql(options.ConnectionString);
					break;
				default: throw new NotSupportedException("This DB flavor is not supported");
			}
		}
#endif

#if NetCore
		private void ApplyConfiguration<TEntity>(ModelBuilder model, Data.EntityTypeConfiguration<TEntity> configuration) where TEntity : class
#elif NetFX
        private void ApplyConfiguration<TEntity>(DbModelBuilder model, Data.EntityTypeConfiguration<TEntity> configuration) where TEntity: class
#else
        private void ApplyConfiguration<TEntity>(DummyModel model, Data.EntityTypeConfiguration<TEntity> configuration) where TEntity: class
#endif
		{
			configuration.DbType = DbType;
			configuration.InitSeedData = InitSeedData;
			model.ApplyConfiguration(configuration);
		}


#if NetCore
		protected override void OnModelCreating(ModelBuilder model)
		{
#elif NetFX
        protected override void OnModelCreating(DbModelBuilder model) {
#else
        protected void OnModelCreating(DummyModel model) {
#endif
			ApplyConfiguration(model, new AccessTokenConfiguration());
			ApplyConfiguration(model, new AdditionalGroupConfiguration());
			ApplyConfiguration(model, new AuditLogConfiguration());
			ApplyConfiguration(model, new AuditLogSourceConfiguration());
			ApplyConfiguration(model, new AuditLogTaskConfiguration());
			ApplyConfiguration(model, new BackgroundTaskConfiguration());
			ApplyConfiguration(model, new BackgroundTaskLogConfiguration());
			ApplyConfiguration(model, new BackgroundTaskParameterConfiguration());
			ApplyConfiguration(model, new BackgroundTaskStackConfiguration());
			ApplyConfiguration(model, new BlackBerryUserConfiguration());
			ApplyConfiguration(model, new ClusterConfiguration());
			ApplyConfiguration(model, new CommentConfiguration());
			ApplyConfiguration(model, new CrmuserConfiguration());
			ApplyConfiguration(model, new DomainConfiguration());
			ApplyConfiguration(model, new DomainDnsRecordConfiguration());
			ApplyConfiguration(model, new EnterpriseFolderConfiguration());
			ApplyConfiguration(model, new EnterpriseFoldersOwaPermissionConfiguration());
			ApplyConfiguration(model, new ExchangeAccountConfiguration());
			ApplyConfiguration(model, new ExchangeAccountEmailAddressConfiguration());
			ApplyConfiguration(model, new ExchangeDeletedAccountConfiguration());
			ApplyConfiguration(model, new ExchangeDisclaimerConfiguration());
			ApplyConfiguration(model, new ExchangeMailboxPlanConfiguration());
			ApplyConfiguration(model, new ExchangeMailboxPlanRetentionPolicyTagConfiguration());
			ApplyConfiguration(model, new ExchangeOrganizationConfiguration());
			ApplyConfiguration(model, new ExchangeOrganizationDomainConfiguration());
			ApplyConfiguration(model, new ExchangeOrganizationSettingConfiguration());
			ApplyConfiguration(model, new ExchangeOrganizationSsFolderConfiguration());
			ApplyConfiguration(model, new ExchangeRetentionPolicyTagConfiguration());
			ApplyConfiguration(model, new GlobalDnsRecordConfiguration());
			ApplyConfiguration(model, new HostingPlanConfiguration());
			ApplyConfiguration(model, new HostingPlanQuotaConfiguration());
			ApplyConfiguration(model, new HostingPlanResourceConfiguration());
			ApplyConfiguration(model, new IpaddressConfiguration());
			ApplyConfiguration(model, new LyncUserConfiguration());
			ApplyConfiguration(model, new LyncUserPlanConfiguration());
			ApplyConfiguration(model, new OcsuserConfiguration());
			ApplyConfiguration(model, new PackageConfiguration());
			ApplyConfiguration(model, new PackageAddonConfiguration());
			ApplyConfiguration(model, new PackageIpaddressConfiguration());
			ApplyConfiguration(model, new PackageQuotaConfiguration());
			ApplyConfiguration(model, new PackageResourceConfiguration());
			ApplyConfiguration(model, new PackageSettingConfiguration());
			ApplyConfiguration(model, new PackageVlanConfiguration());
			ApplyConfiguration(model, new PackagesBandwidthConfiguration());
			ApplyConfiguration(model, new PackagesDiskspaceConfiguration());
			ApplyConfiguration(model, new PackagesTreeCacheConfiguration());
			ApplyConfiguration(model, new PrivateIpaddressConfiguration());
			ApplyConfiguration(model, new PrivateNetworkVlanConfiguration());
			ApplyConfiguration(model, new ProviderConfiguration());
			ApplyConfiguration(model, new QuotaConfiguration());
			ApplyConfiguration(model, new RdscertificateConfiguration());
			ApplyConfiguration(model, new RdscollectionConfiguration());
			ApplyConfiguration(model, new RdscollectionSettingConfiguration());
			ApplyConfiguration(model, new RdscollectionUserConfiguration());
			ApplyConfiguration(model, new RdsmessageConfiguration());
			ApplyConfiguration(model, new RdsserverConfiguration());
			ApplyConfiguration(model, new RdsserverSettingConfiguration());
			ApplyConfiguration(model, new ResourceGroupConfiguration());
			ApplyConfiguration(model, new ResourceGroupDnsRecordConfiguration());
			ApplyConfiguration(model, new ScheduleConfiguration());
			ApplyConfiguration(model, new ScheduleParameterConfiguration());
			ApplyConfiguration(model, new ScheduleTaskConfiguration());
			ApplyConfiguration(model, new ScheduleTaskParameterConfiguration());
			ApplyConfiguration(model, new ScheduleTaskViewConfigurationConfiguration());
			ApplyConfiguration(model, new ServerConfiguration());
			ApplyConfiguration(model, new ServiceConfiguration());
			ApplyConfiguration(model, new ServiceDefaultPropertyConfiguration());
			ApplyConfiguration(model, new ServiceItemConfiguration());
			ApplyConfiguration(model, new ServiceItemPropertyConfiguration());
			ApplyConfiguration(model, new ServiceItemTypeConfiguration());
			ApplyConfiguration(model, new ServicePropertyConfiguration());
			ApplyConfiguration(model, new SfBuserConfiguration());
			ApplyConfiguration(model, new SfBuserPlanConfiguration());
			ApplyConfiguration(model, new SslcertificateConfiguration());
			ApplyConfiguration(model, new StorageSpaceConfiguration());
			ApplyConfiguration(model, new StorageSpaceFolderConfiguration());
			ApplyConfiguration(model, new StorageSpaceLevelConfiguration());
			ApplyConfiguration(model, new StorageSpaceLevelResourceGroupConfiguration());
			ApplyConfiguration(model, new SupportServiceLevelConfiguration());
			ApplyConfiguration(model, new SystemSettingConfiguration());
			ApplyConfiguration(model, new ThemeConfiguration());
			ApplyConfiguration(model, new ThemeSettingConfiguration());
			ApplyConfiguration(model, new UserConfiguration());
			ApplyConfiguration(model, new UserSettingConfiguration());
			ApplyConfiguration(model, new UsersDetailedConfiguration());
			ApplyConfiguration(model, new VersionConfiguration());
			ApplyConfiguration(model, new VirtualGroupConfiguration());
			ApplyConfiguration(model, new VirtualServiceConfiguration());
			ApplyConfiguration(model, new WebDavAccessTokenConfiguration());
			ApplyConfiguration(model, new WebDavPortalUsersSettingConfiguration());

			OnModelCreatingPartial(model);
		}
#if NetCore
		partial void OnModelCreatingPartial(ModelBuilder model);
#elif NetFX
        partial void OnModelCreatingPartial(DbModelBuilder model);
#else
        partial void OnModelCreatingPartial(DummyModel model);
#endif

	}
}

namespace SolidCP.EnterpriseServer.Data
{

	using Version = SolidCP.EnterpriseServer.Data.Entities.Version;
	using GlobalDnsRecord = SolidCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;
	using BackgroundTask = SolidCP.EnterpriseServer.Data.Entities.BackgroundTask;
	using BackgroundTaskParameter = SolidCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

#if ScaffoldDbContextEntities
    public partial class DbContext {
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

        DbSet<Crmuser> crmusers = null;
        public virtual DbSet<Crmuser> Crmusers => crmusers ?? (crmusers = new DbSet<Crmuser>(BaseContext));

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

        DbSet<Ipaddress> ipaddresses = null;
        public virtual DbSet<Ipaddress> Ipaddresses => ipaddresses ?? (ipaddresses = new DbSet<Ipaddress>(BaseContext));

        DbSet<LyncUser> lyncUsers = null;
        public virtual DbSet<LyncUser> LyncUsers => lyncUsers ?? (lyncUsers = new DbSet<LyncUser>(BaseContext));

        DbSet<LyncUserPlan> lyncUserPlans = null;
        public virtual DbSet<LyncUserPlan> LyncUserPlans => lyncUserPlans ?? (lyncUserPlans = new DbSet<LyncUserPlan>(BaseContext));

        DbSet<Ocsuser> ocsusers = null;
        public virtual DbSet<Ocsuser> Ocsusers => ocsusers ?? (ocsusers = new DbSet<Ocsuser>(BaseContext));

        DbSet<Package> packages = null;
        public virtual DbSet<Package> Packages => packages ?? (packages = new DbSet<Package>(BaseContext));

        DbSet<PackageAddon> packageAddons = null;
        public virtual DbSet<PackageAddon> PackageAddons => packageAddons ?? (packageAddons = new DbSet<PackageAddon>(BaseContext));

        DbSet<PackageIpaddress> packageIpaddresses = null;
        public virtual DbSet<PackageIpaddress> PackageIpaddresses => packageIpaddresses ?? (packageIpaddresses = new DbSet<PackageIpaddress>(BaseContext));

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

        DbSet<PrivateIpaddress> privateIpaddresses = null;
        public virtual DbSet<PrivateIpaddress> PrivateIpaddresses => privateIpaddresses ?? (privateIpaddresses = new DbSet<PrivateIpaddress>(BaseContext));

        DbSet<PrivateNetworkVlan> privateNetworkVlans = null;
        public virtual DbSet<PrivateNetworkVlan> PrivateNetworkVlans => privateNetworkVlans ?? (privateNetworkVlans = new DbSet<PrivateNetworkVlan>(BaseContext));

        DbSet<Provider> providers = null;
        public virtual DbSet<Provider> Providers => providers ?? (providers = new DbSet<Provider>(BaseContext));

        DbSet<Quota> quotas = null;
        public virtual DbSet<Quota> Quotas => quotas ?? (quotas = new DbSet<Quota>(BaseContext));

        DbSet<Rdscertificate> rdscertificates = null;
        public virtual DbSet<Rdscertificate> Rdscertificates => rdscertificates ?? (rdscertificates = new DbSet<Rdscertificate>(BaseContext));

        DbSet<Rdscollection> rdscollections = null;
        public virtual DbSet<Rdscollection> Rdscollections => rdscollections ?? (rdscollections = new DbSet<Rdscollection>(BaseContext));

        DbSet<RdscollectionSetting> rdscollectionSettings = null;
        public virtual DbSet<RdscollectionSetting> RdscollectionSettings => rdscollectionSettings ?? (rdscollectionSettings = new DbSet<RdscollectionSetting>(BaseContext));

        DbSet<RdscollectionUser> rdscollectionUsers = null;
        public virtual DbSet<RdscollectionUser> RdscollectionUsers => rdscollectionUsers ?? (rdscollectionUsers = new DbSet<RdscollectionUser>(BaseContext));

        DbSet<Rdsmessage> rdsmessages = null;
        public virtual DbSet<Rdsmessage> Rdsmessages => rdsmessages ?? (rdsmessages = new DbSet<Rdsmessage>(BaseContext));

        DbSet<Rdsserver> rdsservers = null;
        public virtual DbSet<Rdsserver> Rdsservers => rdsservers ?? (rdsservers = new DbSet<Rdsserver>(BaseContext));

        DbSet<RdsserverSetting> rdsserverSettings = null;
        public virtual DbSet<RdsserverSetting> RdsserverSettings => rdsserverSettings ?? (rdsserverSettings = new DbSet<RdsserverSetting>(BaseContext));

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

        DbSet<Sslcertificate> sslcertificates = null;
        public virtual DbSet<Sslcertificate> Sslcertificates => sslcertificates ?? (sslcertificates = new DbSet<Sslcertificate>(BaseContext));

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
#endif
}
#endif
