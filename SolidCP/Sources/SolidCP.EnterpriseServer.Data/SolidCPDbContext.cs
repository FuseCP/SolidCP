#if ScaffoldedDbContext
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
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

namespace SolidCP.EnterpriseServer.Data;

public partial class SolidCPDbContext : DbContext
{
    public SolidCPDbContext()
    {
    }

    public SolidCPDbContext(DbContextOptions<SolidCPDbContext> options)
        : base(options)
    {
    }

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database=SolidCP;uid=sa;pwd=Password12;TrustServerCertificate=true;Connection Timeout=300;command timeout=300");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccessTo__3214EC27A32557FE");

            entity.HasOne(d => d.Account).WithMany(p => p.AccessTokens).HasConstraintName("FK_AccessTokens_UserId");
        });

        modelBuilder.Entity<AdditionalGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addition__3214EC272F1861EB");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK_Log");
        });

        modelBuilder.Entity<AuditLogTask>(entity =>
        {
            entity.HasKey(e => new { e.SourceName, e.TaskName }).HasName("PK_LogActions");
        });

        modelBuilder.Entity<BackgroundTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Backgrou__3214EC271AFAB817");
        });

        modelBuilder.Entity<BackgroundTaskLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Backgrou__5E5499A830A1D5BF");

            entity.HasOne(d => d.Task).WithMany(p => p.BackgroundTaskLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__06ADD4BD");
        });

        modelBuilder.Entity<BackgroundTaskParameter>(entity =>
        {
            entity.HasKey(e => e.ParameterId).HasName("PK__Backgrou__F80C6297E2E5AF88");

            entity.HasOne(d => d.Task).WithMany(p => p.BackgroundTaskParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__03D16812");
        });

        modelBuilder.Entity<BackgroundTaskStack>(entity =>
        {
            entity.HasKey(e => e.TaskStackId).HasName("PK__Backgrou__5E44466F62E48BE6");

            entity.HasOne(d => d.Task).WithMany(p => p.BackgroundTaskStacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__098A4168");
        });

        modelBuilder.Entity<BlackBerryUser>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Account).WithMany(p => p.BlackBerryUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlackBerryUsers_ExchangeAccounts");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.CommentsNavigation).HasConstraintName("FK_Comments_Users");
        });

        modelBuilder.Entity<Crmuser>(entity =>
        {
            entity.Property(e => e.ChangedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Account).WithMany(p => p.Crmusers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMUsers_ExchangeAccounts");
        });

        modelBuilder.Entity<Domain>(entity =>
        {
            entity.HasOne(d => d.MailDomain).WithMany(p => p.DomainMailDomains).HasConstraintName("FK_Domains_ServiceItems_MailDomain");

            entity.HasOne(d => d.Package).WithMany(p => p.Domains).HasConstraintName("FK_Domains_Packages");

            entity.HasOne(d => d.WebSite).WithMany(p => p.DomainWebSites).HasConstraintName("FK_Domains_ServiceItems_WebSite");

            entity.HasOne(d => d.ZoneItem).WithMany(p => p.DomainZoneItems).HasConstraintName("FK_Domains_ServiceItems_ZoneItem");
        });

        modelBuilder.Entity<DomainDnsRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DomainDn__3214EC2758B0A6F1");

            entity.HasOne(d => d.Domain).WithMany(p => p.DomainDnsRecords).HasConstraintName("FK_DomainDnsRecords_DomainId");
        });

        modelBuilder.Entity<EnterpriseFolder>(entity =>
        {
            entity.HasOne(d => d.StorageSpaceFolder).WithMany(p => p.EnterpriseFolders)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EnterpriseFolders_StorageSpaceFolderId");
        });

        modelBuilder.Entity<EnterpriseFoldersOwaPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Enterpri__3214EC27D1B48691");

            entity.HasOne(d => d.Account).WithMany(p => p.EnterpriseFoldersOwaPermissions).HasConstraintName("FK_EnterpriseFoldersOwaPermissions_AccountId");

            entity.HasOne(d => d.Folder).WithMany(p => p.EnterpriseFoldersOwaPermissions).HasConstraintName("FK_EnterpriseFoldersOwaPermissions_FolderId");
        });

        modelBuilder.Entity<ExchangeAccount>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Item).WithMany(p => p.ExchangeAccounts).HasConstraintName("FK_ExchangeAccounts_ServiceItems");

            entity.HasOne(d => d.MailboxPlan).WithMany(p => p.ExchangeAccounts).HasConstraintName("FK_ExchangeAccounts_ExchangeMailboxPlans");
        });

        modelBuilder.Entity<ExchangeAccountEmailAddress>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.ExchangeAccountEmailAddresses).HasConstraintName("FK_ExchangeAccountEmailAddresses_ExchangeAccounts");
        });

        modelBuilder.Entity<ExchangeDeletedAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exchange__3214EC27EF1C22C1");
        });

        modelBuilder.Entity<ExchangeMailboxPlan>(entity =>
        {
            entity.HasOne(d => d.Item).WithMany(p => p.ExchangeMailboxPlans).HasConstraintName("FK_ExchangeMailboxPlans_ExchangeOrganizations");
        });

        modelBuilder.Entity<ExchangeMailboxPlanRetentionPolicyTag>(entity =>
        {
            entity.HasKey(e => e.PlanTagId).HasName("PK__Exchange__E467073C50CD805B");
        });

        modelBuilder.Entity<ExchangeOrganization>(entity =>
        {
            entity.Property(e => e.ItemId).ValueGeneratedNever();

            entity.HasOne(d => d.Item).WithOne(p => p.ExchangeOrganization).HasConstraintName("FK_ExchangeOrganizations_ServiceItems");
        });

        modelBuilder.Entity<ExchangeOrganizationDomain>(entity =>
        {
            entity.Property(e => e.IsHost).HasDefaultValue(false);

            entity.HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationDomains).HasConstraintName("FK_ExchangeOrganizationDomains_ServiceItems");
        });

        modelBuilder.Entity<ExchangeOrganizationSsFolder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exchange__3214EC072DDBA072");

            entity.HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationSsFolders).HasConstraintName("FK_ExchangeOrganizationSsFolders_ItemId");

            entity.HasOne(d => d.StorageSpaceFolder).WithMany(p => p.ExchangeOrganizationSsFolders).HasConstraintName("FK_ExchangeOrganizationSsFolders_StorageSpaceFolderId");
        });

        modelBuilder.Entity<ExchangeRetentionPolicyTag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__Exchange__657CFA4C02667D37");
        });

        modelBuilder.Entity<GlobalDnsRecord>(entity =>
        {
            entity.HasOne(d => d.Ipaddress).WithMany(p => p.GlobalDnsRecords).HasConstraintName("FK_GlobalDnsRecords_IPAddresses");

            entity.HasOne(d => d.Package).WithMany(p => p.GlobalDnsRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Packages");

            entity.HasOne(d => d.Server).WithMany(p => p.GlobalDnsRecords).HasConstraintName("FK_GlobalDnsRecords_Servers");

            entity.HasOne(d => d.Service).WithMany(p => p.GlobalDnsRecords)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Services");
        });

        modelBuilder.Entity<HostingPlan>(entity =>
        {
            entity.HasOne(d => d.Package).WithMany(p => p.HostingPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HostingPlans_Packages");

            entity.HasOne(d => d.Server).WithMany(p => p.HostingPlans).HasConstraintName("FK_HostingPlans_Servers");

            entity.HasOne(d => d.User).WithMany(p => p.HostingPlans).HasConstraintName("FK_HostingPlans_Users");
        });

        modelBuilder.Entity<HostingPlanQuota>(entity =>
        {
            entity.HasKey(e => new { e.PlanId, e.QuotaId }).HasName("PK_HostingPlanQuotas_1");

            entity.HasOne(d => d.Plan).WithMany(p => p.HostingPlanQuota).HasConstraintName("FK_HostingPlanQuotas_HostingPlans");

            entity.HasOne(d => d.Quota).WithMany(p => p.HostingPlanQuota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostingPlanQuotas_Quotas");
        });

        modelBuilder.Entity<HostingPlanResource>(entity =>
        {
            entity.HasOne(d => d.Group).WithMany(p => p.HostingPlanResources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostingPlanResources_ResourceGroups");

            entity.HasOne(d => d.Plan).WithMany(p => p.HostingPlanResources).HasConstraintName("FK_HostingPlanResources_HostingPlans");
        });

        modelBuilder.Entity<Ipaddress>(entity =>
        {
            entity.HasOne(d => d.Server).WithMany(p => p.Ipaddresses)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_IPAddresses_Servers");
        });

        modelBuilder.Entity<LyncUser>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.LyncUserPlan).WithMany(p => p.LyncUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LyncUsers_LyncUserPlans");
        });

        modelBuilder.Entity<LyncUserPlan>(entity =>
        {
            entity.HasOne(d => d.Item).WithMany(p => p.LyncUserPlans).HasConstraintName("FK_LyncUserPlans_ExchangeOrganizations");
        });

        modelBuilder.Entity<Ocsuser>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("Update_StatusIDchangeDate"));

            entity.Property(e => e.StatusIdchangeDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ParentPackage).WithMany(p => p.InverseParentPackage).HasConstraintName("FK_Packages_Packages");

            entity.HasOne(d => d.Plan).WithMany(p => p.Packages).HasConstraintName("FK_Packages_HostingPlans");

            entity.HasOne(d => d.Server).WithMany(p => p.Packages).HasConstraintName("FK_Packages_Servers");

            entity.HasOne(d => d.User).WithMany(p => p.Packages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Packages_Users");

            entity.HasMany(d => d.Services).WithMany(p => p.Packages)
                .UsingEntity<Dictionary<string, object>>(
                    "PackageService",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .HasConstraintName("FK_PackageServices_Services"),
                    l => l.HasOne<Package>().WithMany()
                        .HasForeignKey("PackageId")
                        .HasConstraintName("FK_PackageServices_Packages"),
                    j =>
                    {
                        j.HasKey("PackageId", "ServiceId");
                        j.ToTable("PackageServices");
                        j.IndexerProperty<int>("PackageId").HasColumnName("PackageID");
                        j.IndexerProperty<int>("ServiceId").HasColumnName("ServiceID");
                    });
        });

        modelBuilder.Entity<PackageAddon>(entity =>
        {
            entity.HasOne(d => d.Package).WithMany(p => p.PackageAddons)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PackageAddons_Packages");

            entity.HasOne(d => d.Plan).WithMany(p => p.PackageAddons).HasConstraintName("FK_PackageAddons_HostingPlans");
        });

        modelBuilder.Entity<PackageIpaddress>(entity =>
        {
            entity.HasOne(d => d.Address).WithMany(p => p.PackageIpaddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageIPAddresses_IPAddresses");

            entity.HasOne(d => d.Item).WithMany(p => p.PackageIpaddresses).HasConstraintName("FK_PackageIPAddresses_ServiceItems");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageIpaddresses).HasConstraintName("FK_PackageIPAddresses_Packages");
        });

        modelBuilder.Entity<PackageQuota>(entity =>
        {
            entity.HasOne(d => d.Package).WithMany(p => p.PackageQuota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageQuotas_Packages");

            entity.HasOne(d => d.Quota).WithMany(p => p.PackageQuota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageQuotas_Quotas");
        });

        modelBuilder.Entity<PackageResource>(entity =>
        {
            entity.HasKey(e => new { e.PackageId, e.GroupId }).HasName("PK_PackageResources_1");

            entity.HasOne(d => d.Group).WithMany(p => p.PackageResources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageResources_ResourceGroups");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageResources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageResources_Packages");
        });

        modelBuilder.Entity<PackageVlan>(entity =>
        {
            entity.HasKey(e => e.PackageVlanId).HasName("PK__PackageV__A9AABBF9C0C25CB3");

            entity.HasOne(d => d.Package).WithMany(p => p.PackageVlans).HasConstraintName("FK_PackageID");

            entity.HasOne(d => d.Vlan).WithMany(p => p.PackageVlans).HasConstraintName("FK_VlanID");
        });

        modelBuilder.Entity<PackagesBandwidth>(entity =>
        {
            entity.HasOne(d => d.Group).WithMany(p => p.PackagesBandwidths)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesBandwidth_ResourceGroups");

            entity.HasOne(d => d.Package).WithMany(p => p.PackagesBandwidths)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesBandwidth_Packages");
        });

        modelBuilder.Entity<PackagesDiskspace>(entity =>
        {
            entity.HasOne(d => d.Group).WithMany(p => p.PackagesDiskspaces)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesDiskspace_ResourceGroups");

            entity.HasOne(d => d.Package).WithMany(p => p.PackagesDiskspaces)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesDiskspace_Packages");
        });

        modelBuilder.Entity<PackagesTreeCache>(entity =>
        {
            entity.HasIndex(e => new { e.ParentPackageId, e.PackageId }, "PackagesTreeCacheIndex").IsClustered();

            entity.HasOne(d => d.Package).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesTreeCache_Packages1");

            entity.HasOne(d => d.ParentPackage).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesTreeCache_Packages");
        });

        modelBuilder.Entity<PrivateIpaddress>(entity =>
        {
            entity.HasOne(d => d.Item).WithMany(p => p.PrivateIpaddresses).HasConstraintName("FK_PrivateIPAddresses_ServiceItems");
        });

        modelBuilder.Entity<PrivateNetworkVlan>(entity =>
        {
            entity.HasKey(e => e.VlanId).HasName("PK__PrivateN__8348135581B53618");

            entity.HasOne(d => d.Server).WithMany(p => p.PrivateNetworkVlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ServerID");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PK_ServiceTypes");

            entity.Property(e => e.ProviderId).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithMany(p => p.Providers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Providers_ResourceGroups");
        });

        modelBuilder.Entity<Quota>(entity =>
        {
            entity.Property(e => e.QuotaId).ValueGeneratedNever();
            entity.Property(e => e.QuotaOrder).HasDefaultValue(1);
            entity.Property(e => e.QuotaTypeId).HasDefaultValue(2);
            entity.Property(e => e.ServiceQuota).HasDefaultValue(false);

            entity.HasOne(d => d.Group).WithMany(p => p.Quota).HasConstraintName("FK_Quotas_ResourceGroups");

            entity.HasOne(d => d.ItemType).WithMany(p => p.Quota).HasConstraintName("FK_Quotas_ServiceItemTypes");
        });

        modelBuilder.Entity<Rdscollection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RDSColle__3214EC27346D361D");
        });

        modelBuilder.Entity<RdscollectionSetting>(entity =>
        {
            entity.HasOne(d => d.Rdscollection).WithMany(p => p.RdscollectionSettings).HasConstraintName("FK_RDSCollectionSettings_RDSCollections");
        });

        modelBuilder.Entity<RdscollectionUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RDSColle__3214EC2780141EF7");

            entity.HasOne(d => d.Account).WithMany(p => p.RdscollectionUsers).HasConstraintName("FK_RDSCollectionUsers_UserId");

            entity.HasOne(d => d.Rdscollection).WithMany(p => p.RdscollectionUsers).HasConstraintName("FK_RDSCollectionUsers_RDSCollectionId");
        });

        modelBuilder.Entity<Rdsmessage>(entity =>
        {
            entity.Property(e => e.UserName).IsFixedLength();

            entity.HasOne(d => d.Rdscollection).WithMany(p => p.Rdsmessages).HasConstraintName("FK_RDSMessages_RDSCollections");
        });

        modelBuilder.Entity<Rdsserver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RDSServe__3214EC27DBEBD4B5");

            entity.Property(e => e.ConnectionEnabled).HasDefaultValue(true);

            entity.HasOne(d => d.Rdscollection).WithMany(p => p.Rdsservers).HasConstraintName("FK_RDSServers_RDSCollectionId");
        });

        modelBuilder.Entity<ResourceGroup>(entity =>
        {
            entity.Property(e => e.GroupId).ValueGeneratedNever();
            entity.Property(e => e.GroupOrder).HasDefaultValue(1);
        });

        modelBuilder.Entity<ResourceGroupDnsRecord>(entity =>
        {
            entity.Property(e => e.RecordOrder).HasDefaultValue(1);

            entity.HasOne(d => d.Group).WithMany(p => p.ResourceGroupDnsRecords).HasConstraintName("FK_ResourceGroupDnsRecords_ResourceGroups");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasOne(d => d.Package).WithMany(p => p.Schedules)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Schedule_Packages");

            entity.HasOne(d => d.Task).WithMany(p => p.Schedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_ScheduleTasks");
        });

        modelBuilder.Entity<ScheduleParameter>(entity =>
        {
            entity.HasOne(d => d.Schedule).WithMany(p => p.ScheduleParameters).HasConstraintName("FK_ScheduleParameters_Schedule");
        });

        modelBuilder.Entity<ScheduleTaskParameter>(entity =>
        {
            entity.HasOne(d => d.Task).WithMany(p => p.ScheduleTaskParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleTaskParameters_ScheduleTasks");
        });

        modelBuilder.Entity<ScheduleTaskViewConfiguration>(entity =>
        {
            entity.HasOne(d => d.Task).WithMany(p => p.ScheduleTaskViewConfigurations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleTaskViewConfiguration_ScheduleTaskViewConfiguration");
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.Property(e => e.Adenabled).HasDefaultValue(false);
            entity.Property(e => e.ServerUrl).HasDefaultValue("");

            entity.HasOne(d => d.PrimaryGroup).WithMany(p => p.Servers).HasConstraintName("FK_Servers_ResourceGroups");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasOne(d => d.Cluster).WithMany(p => p.Services).HasConstraintName("FK_Services_Clusters");

            entity.HasOne(d => d.Provider).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Providers");

            entity.HasOne(d => d.Server).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Servers");
        });

        modelBuilder.Entity<ServiceDefaultProperty>(entity =>
        {
            entity.HasKey(e => new { e.ProviderId, e.PropertyName }).HasName("PK_ServiceDefaultProperties_1");

            entity.HasOne(d => d.Provider).WithMany(p => p.ServiceDefaultProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceDefaultProperties_Providers");
        });

        modelBuilder.Entity<ServiceItem>(entity =>
        {
            entity.HasOne(d => d.ItemType).WithMany(p => p.ServiceItems).HasConstraintName("FK_ServiceItems_ServiceItemTypes");

            entity.HasOne(d => d.Package).WithMany(p => p.ServiceItems).HasConstraintName("FK_ServiceItems_Packages");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceItems).HasConstraintName("FK_ServiceItems_Services");
        });

        modelBuilder.Entity<ServiceItemProperty>(entity =>
        {
            entity.HasOne(d => d.Item).WithMany(p => p.ServiceItemProperties).HasConstraintName("FK_ServiceItemProperties_ServiceItems");
        });

        modelBuilder.Entity<ServiceItemType>(entity =>
        {
            entity.Property(e => e.ItemTypeId).ValueGeneratedNever();
            entity.Property(e => e.Backupable).HasDefaultValue(true);
            entity.Property(e => e.Importable).HasDefaultValue(true);
            entity.Property(e => e.TypeOrder).HasDefaultValue(1);

            entity.HasOne(d => d.Group).WithMany(p => p.ServiceItemTypes).HasConstraintName("FK_ServiceItemTypes_ResourceGroups");
        });

        modelBuilder.Entity<ServiceProperty>(entity =>
        {
            entity.HasKey(e => new { e.ServiceId, e.PropertyName }).HasName("PK_ServiceProperties_1");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceProperties).HasConstraintName("FK_ServiceProperties_Services");
        });

        modelBuilder.Entity<Sslcertificate>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<StorageSpace>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StorageS__3214EC07B8B9A6D1");

            entity.HasOne(d => d.Server).WithMany(p => p.StorageSpaces).HasConstraintName("FK_StorageSpaces_ServerId");

            entity.HasOne(d => d.Service).WithMany(p => p.StorageSpaces).HasConstraintName("FK_StorageSpaces_ServiceId");
        });

        modelBuilder.Entity<StorageSpaceFolder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StorageS__3214EC07AC0C9EB6");

            entity.HasOne(d => d.StorageSpace).WithMany(p => p.StorageSpaceFolders).HasConstraintName("FK_StorageSpaceFolders_StorageSpaceId");
        });

        modelBuilder.Entity<StorageSpaceLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StorageS__3214EC07B8D82363");
        });

        modelBuilder.Entity<StorageSpaceLevelResourceGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StorageS__3214EC07EBEBED98");

            entity.HasOne(d => d.Group).WithMany(p => p.StorageSpaceLevelResourceGroups).HasConstraintName("FK_StorageSpaceLevelResourceGroups_GroupId");

            entity.HasOne(d => d.Level).WithMany(p => p.StorageSpaceLevelResourceGroups).HasConstraintName("FK_StorageSpaceLevelResourceGroups_LevelId");
        });

        modelBuilder.Entity<SupportServiceLevel>(entity =>
        {
            entity.HasKey(e => e.LevelId).HasName("PK__SupportS__09F03C065BA08AFB");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.HtmlMail).HasDefaultValue(true);

            entity.HasOne(d => d.Owner).WithMany(p => p.InverseOwner).HasConstraintName("FK_Users_Users");
        });

        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.UserSettings).HasConstraintName("FK_UserSettings_Users");
        });

        modelBuilder.Entity<UsersDetailed>(entity =>
        {
            entity.ToView("UsersDetailed");
        });

        modelBuilder.Entity<VirtualGroup>(entity =>
        {
            entity.HasOne(d => d.Group).WithMany(p => p.VirtualGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VirtualGroups_ResourceGroups");

            entity.HasOne(d => d.Server).WithMany(p => p.VirtualGroups).HasConstraintName("FK_VirtualGroups_Servers");
        });

        modelBuilder.Entity<VirtualService>(entity =>
        {
            entity.HasOne(d => d.Server).WithMany(p => p.VirtualServices).HasConstraintName("FK_VirtualServices_Servers");

            entity.HasOne(d => d.Service).WithMany(p => p.VirtualServices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VirtualServices_Services");
        });

        modelBuilder.Entity<WebDavAccessToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WebDavAc__3214EC27B27DC571");

            entity.HasOne(d => d.Account).WithMany(p => p.WebDavAccessTokens).HasConstraintName("FK_WebDavAccessTokens_UserId");
        });

        modelBuilder.Entity<WebDavPortalUsersSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WebDavPo__3214EC278AF5195E");

            entity.HasOne(d => d.Account).WithMany(p => p.WebDavPortalUsersSettings).HasConstraintName("FK_WebDavPortalUsersSettings_UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
#endif