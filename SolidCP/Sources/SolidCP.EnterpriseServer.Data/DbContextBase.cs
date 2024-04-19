#if ScaffoldedDbContext
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Entities;
#if NetCore
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Context
{

    using Version = SolidCP.EnterpriseServer.Data.Entities.Version;
    using GlobalDnsRecord = SolidCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;
    using BackgroundTaskParameter = SolidCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

    public partial class DbContextBase : DbContext, Data.IGenericDbContext
    {

#if NetCore
        public DbContextBase(Data.DbContext context): this(new Data.Extensions.DbOptions<DbContextBase>(context)) { }
        public DbContextBase(DbContextOptions<DbContextBase> options): base(options) { }
#elif NetFX
        public DbContextBase(Data.DbContext context): base(context.ConnectionString) { }
#endif

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
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!(builder.Options is Data.Extensions.DbOptions<DbContextBase> options)) throw new NotSupportedException("This type of Options is not supported");
            switch (options.Flavor)
            {
                case Data.DbFlavor.MsSql:
                    builder.UseSqlServer(options.ConnectionString);
                    break;
                case Data.DbFlavor.SqlLite:
                    builder.UseSqlite(options.ConnectionString);
                    break;
                case Data.DbFlavor.MySql:
                    builder.UseMySql(options.ConnectionString, ServerVersion.AutoDetect(options.ConnectionString));
                    break;
                case Data.DbFlavor.MariaDb:
                    builder.UseMariaDB(options.ConnectionString);
                    break;
                case Data.DbFlavor.PostgreSql:
                    builder.UseNpgsql(options.ConnectionString);
                    break;
                default: throw new NotSupportedException("This DB flavor is not supported");
            }
        }
#endif

#if NetCore
        protected override void OnModelCreating(ModelBuilder model) {
#elif NetFX
        protected override void OnModelCreating(DbModelBuilder model) {
#else
        protected void OnModelCreating(DummyModel model) {
#endif

#if ScaffoldDbContextEntities
              model.ApplyConfiguration(new AccessTokenConfiguration(Flavor));
              model.ApplyConfiguration(new AdditionalGroupConfiguration(Flavor));
              model.ApplyConfiguration(new AuditLogConfiguration(Flavor));
              model.ApplyConfiguration(new AuditLogSourceConfiguration(Flavor));
              model.ApplyConfiguration(new AuditLogTaskConfiguration(Flavor));
              model.ApplyConfiguration(new BackgroundTaskConfiguration(Flavor));
              model.ApplyConfiguration(new BackgroundTaskLogConfiguration(Flavor));
              model.ApplyConfiguration(new BackgroundTaskParameterConfiguration(Flavor));
              model.ApplyConfiguration(new BackgroundTaskStackConfiguration(Flavor));
              model.ApplyConfiguration(new BlackBerryUserConfiguration(Flavor));
              model.ApplyConfiguration(new ClusterConfiguration(Flavor));
              model.ApplyConfiguration(new CommentConfiguration(Flavor));
              model.ApplyConfiguration(new CrmuserConfiguration(Flavor));
              model.ApplyConfiguration(new DomainConfiguration(Flavor));
              model.ApplyConfiguration(new DomainDnsRecordConfiguration(Flavor));
              model.ApplyConfiguration(new EnterpriseFolderConfiguration(Flavor));
              model.ApplyConfiguration(new EnterpriseFoldersOwaPermissionConfiguration(Flavor));
              model.ApplyConfiguration(new ExchangeAccountConfiguration(Flavor));
              model.ApplyConfiguration(new ExchangeAccountEmailAddressConfiguration(Flavor));
              model.ApplyConfiguration(new ExchangeDeletedAccountConfiguration(Flavor));
              model.ApplyConfiguration(new ExchangeDisclaimerConfiguration(Flavor));
              model.ApplyConfiguration(new ExchangeMailboxPlanConfiguration(Flavor));
              model.ApplyConfiguration(new ExchangeMailboxPlanRetentionPolicyTagConfiguration(Flavor));
              model.ApplyConfiguration(new ExchangeOrganizationConfiguration(Flavor));
              model.ApplyConfiguration(new ExchangeOrganizationDomainConfiguration(Flavor));
              model.ApplyConfiguration(new ExchangeOrganizationSettingConfiguration(Flavor));
              model.ApplyConfiguration(new ExchangeOrganizationSsFolderConfiguration(Flavor));
              model.ApplyConfiguration(new ExchangeRetentionPolicyTagConfiguration(Flavor));
              model.ApplyConfiguration(new GlobalDnsRecordConfiguration(Flavor));
              model.ApplyConfiguration(new HostingPlanConfiguration(Flavor));
              model.ApplyConfiguration(new HostingPlanQuotaConfiguration(Flavor));
              model.ApplyConfiguration(new HostingPlanResourceConfiguration(Flavor));
              model.ApplyConfiguration(new IpaddressConfiguration(Flavor));
              model.ApplyConfiguration(new LyncUserConfiguration(Flavor));
              model.ApplyConfiguration(new LyncUserPlanConfiguration(Flavor));
              model.ApplyConfiguration(new OcsuserConfiguration(Flavor));
              model.ApplyConfiguration(new PackageConfiguration(Flavor));
              model.ApplyConfiguration(new PackageAddonConfiguration(Flavor));
              model.ApplyConfiguration(new PackageIpaddressConfiguration(Flavor));
              model.ApplyConfiguration(new PackageQuotaConfiguration(Flavor));
              model.ApplyConfiguration(new PackageResourceConfiguration(Flavor));
              model.ApplyConfiguration(new PackageSettingConfiguration(Flavor));
              model.ApplyConfiguration(new PackageVlanConfiguration(Flavor));
              model.ApplyConfiguration(new PackagesBandwidthConfiguration(Flavor));
              model.ApplyConfiguration(new PackagesDiskspaceConfiguration(Flavor));
              model.ApplyConfiguration(new PackagesTreeCacheConfiguration(Flavor));
              model.ApplyConfiguration(new PrivateIpaddressConfiguration(Flavor));
              model.ApplyConfiguration(new PrivateNetworkVlanConfiguration(Flavor));
              model.ApplyConfiguration(new ProviderConfiguration(Flavor));
              model.ApplyConfiguration(new QuotaConfiguration(Flavor));
              model.ApplyConfiguration(new RdscertificateConfiguration(Flavor));
              model.ApplyConfiguration(new RdscollectionConfiguration(Flavor));
              model.ApplyConfiguration(new RdscollectionSettingConfiguration(Flavor));
              model.ApplyConfiguration(new RdscollectionUserConfiguration(Flavor));
              model.ApplyConfiguration(new RdsmessageConfiguration(Flavor));
              model.ApplyConfiguration(new RdsserverConfiguration(Flavor));
              model.ApplyConfiguration(new RdsserverSettingConfiguration(Flavor));
              model.ApplyConfiguration(new ResourceGroupConfiguration(Flavor));
              model.ApplyConfiguration(new ResourceGroupDnsRecordConfiguration(Flavor));
              model.ApplyConfiguration(new ScheduleConfiguration(Flavor));
              model.ApplyConfiguration(new ScheduleParameterConfiguration(Flavor));
              model.ApplyConfiguration(new ScheduleTaskConfiguration(Flavor));
              model.ApplyConfiguration(new ScheduleTaskParameterConfiguration(Flavor));
              model.ApplyConfiguration(new ScheduleTaskViewConfigurationConfiguration(Flavor));
              model.ApplyConfiguration(new ServerConfiguration(Flavor));
              model.ApplyConfiguration(new ServiceConfiguration(Flavor));
              model.ApplyConfiguration(new ServiceDefaultPropertyConfiguration(Flavor));
              model.ApplyConfiguration(new ServiceItemConfiguration(Flavor));
              model.ApplyConfiguration(new ServiceItemPropertyConfiguration(Flavor));
              model.ApplyConfiguration(new ServiceItemTypeConfiguration(Flavor));
              model.ApplyConfiguration(new ServicePropertyConfiguration(Flavor));
              model.ApplyConfiguration(new SfBuserConfiguration(Flavor));
              model.ApplyConfiguration(new SfBuserPlanConfiguration(Flavor));
              model.ApplyConfiguration(new SslcertificateConfiguration(Flavor));
              model.ApplyConfiguration(new StorageSpaceConfiguration(Flavor));
              model.ApplyConfiguration(new StorageSpaceFolderConfiguration(Flavor));
              model.ApplyConfiguration(new StorageSpaceLevelConfiguration(Flavor));
              model.ApplyConfiguration(new StorageSpaceLevelResourceGroupConfiguration(Flavor));
              model.ApplyConfiguration(new SupportServiceLevelConfiguration(Flavor));
              model.ApplyConfiguration(new SystemSettingConfiguration(Flavor));
              model.ApplyConfiguration(new ThemeConfiguration(Flavor));
              model.ApplyConfiguration(new ThemeSettingConfiguration(Flavor));
              model.ApplyConfiguration(new UserConfiguration(Flavor));
              model.ApplyConfiguration(new UserSettingConfiguration(Flavor));
              model.ApplyConfiguration(new UsersDetailedConfiguration(Flavor));
              model.ApplyConfiguration(new VersionConfiguration(Flavor));
              model.ApplyConfiguration(new VirtualGroupConfiguration(Flavor));
              model.ApplyConfiguration(new VirtualServiceConfiguration(Flavor));
              model.ApplyConfiguration(new WebDavAccessTokenConfiguration(Flavor));
              model.ApplyConfiguration(new WebDavPortalUsersSettingConfiguration(Flavor));
#endif

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

namespace SolidCP.EnterpriseServer.Data {

    using Version = SolidCP.EnterpriseServer.Data.Entities.Version;
    using GlobalDnsRecord = SolidCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;
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
