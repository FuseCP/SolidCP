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

// This file is auto generated, only edit when you add tables without scaffolding.
namespace SolidCP.EnterpriseServer.Context
{

    using Version = SolidCP.EnterpriseServer.Data.Entities.Version;
    using GlobalDnsRecord = SolidCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;
    using BackgroundTaskParameter = SolidCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

    public partial class SolidCPBaseContext : DbContext, Data.IGenericDbContext
    {

        public SolidCPBaseContext()
        {
        }

        Data.DbContext context;
        public SolidCPBaseContext(Data.DbContext context)
        {
            this.context = context;
        }

#if NetCore
        public SolidCPBaseContext(DbContextOptions<SolidCPBaseContext> options)
            : base(options)
        {
        }
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
            switch (context.Flavor)
            {
                case Data.DbFlavor.MsSql:
                    builder.UseSqlServer(context.ConnectionString);
                    break;
                case Data.DbFlavor.SqlLite:
                    builder.UseSqlite(context.ConnectionString);
                    break;
                case Data.DbFlavor.MySql:
                    builder.UseMySql(context.ConnectionString, ServerVersion.AutoDetect(context.ConnectionString));
                    break;
                case Data.DbFlavor.MariaDb:
                    builder.UseMariaDB(context.ConnectionString);
                    break;
                case Data.DbFlavor.PostgreSql:
                    builder.UseNpgsql(context.ConnectionString);
                    break;
                default: throw new NotSupportedException("This DB flavor is not supported");
            }
        }
#endif
#if NetCore
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessToken>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__AccessTo__3214EC27A32557FE");

                entity.HasIndex(e => e.AccountId, "AccessTokensIdx_AccountID");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
                entity.Property(e => e.SmsResponse)
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.HasOne(d => d.Account).WithMany(p => p.AccessTokens)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AccessTokens_UserId");
            });

            modelBuilder.Entity<AdditionalGroup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Addition__3214EC272F1861EB");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.GroupName).HasMaxLength(255);
                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.RecordId).HasName("PK_Log");

                entity.ToTable("AuditLog");

                entity.Property(e => e.RecordId)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("RecordID");
                entity.Property(e => e.ExecutionLog).HasColumnType("ntext");
                entity.Property(e => e.FinishDate).HasColumnType("datetime");
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.ItemName).HasMaxLength(100);
                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.SeverityId).HasColumnName("SeverityID");
                entity.Property(e => e.SourceName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.StartDate).HasColumnType("datetime");
                entity.Property(e => e.TaskName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<AuditLogSource>(entity =>
            {
                entity.HasKey(e => e.SourceName);

                entity.Property(e => e.SourceName)
                .HasMaxLength(100)
                .IsUnicode(false);
            });

            modelBuilder.Entity<AuditLogTask>(entity =>
            {
                entity.HasKey(e => new { e.SourceName, e.TaskName }).HasName("PK_LogActions");

                entity.Property(e => e.SourceName)
                .HasMaxLength(100)
                .IsUnicode(false);
                entity.Property(e => e.TaskName)
                .HasMaxLength(100)
                .IsUnicode(false);
                entity.Property(e => e.TaskDescription).HasMaxLength(100);
            });

            modelBuilder.Entity<BackgroundTask>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Backgrou__3214EC271AFAB817");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.EffectiveUserId).HasColumnName("EffectiveUserID");
                entity.Property(e => e.FinishDate).HasColumnType("datetime");
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.ItemName).HasMaxLength(255);
                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
                entity.Property(e => e.StartDate).HasColumnType("datetime");
                entity.Property(e => e.TaskId)
                .HasMaxLength(255)
                .HasColumnName("TaskID");
                entity.Property(e => e.TaskName).HasMaxLength(255);
                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<BackgroundTaskLog>(entity =>
            {
                entity.HasKey(e => e.LogId).HasName("PK__Backgrou__5E5499A830A1D5BF");

                entity.HasIndex(e => e.TaskId, "BackgroundTaskLogsIdx_TaskID");

                entity.Property(e => e.LogId).HasColumnName("LogID");
                entity.Property(e => e.Date).HasColumnType("datetime");
                entity.Property(e => e.ExceptionStackTrace).HasColumnType("ntext");
                entity.Property(e => e.TaskId).HasColumnName("TaskID");
                entity.Property(e => e.Text).HasColumnType("ntext");
                entity.Property(e => e.XmlParameters).HasColumnType("ntext");

                entity.HasOne(d => d.Task).WithMany(p => p.BackgroundTaskLogs)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__06ADD4BD");
            });

            modelBuilder.Entity<BackgroundTaskParameter>(entity =>
            {
                entity.HasKey(e => e.ParameterId).HasName("PK__Backgrou__F80C6297E2E5AF88");

                entity.HasIndex(e => e.TaskId, "BackgroundTaskParametersIdx_TaskID");

                entity.Property(e => e.ParameterId).HasColumnName("ParameterID");
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.SerializerValue).HasColumnType("ntext");
                entity.Property(e => e.TaskId).HasColumnName("TaskID");
                entity.Property(e => e.TypeName).HasMaxLength(255);

                entity.HasOne(d => d.Task).WithMany(p => p.BackgroundTaskParameters)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__03D16812");
            });

            modelBuilder.Entity<BackgroundTaskStack>(entity =>
            {
                entity.HasKey(e => e.TaskStackId).HasName("PK__Backgrou__5E44466F62E48BE6");

                entity.ToTable("BackgroundTaskStack");

                entity.HasIndex(e => e.TaskId, "BackgroundTaskStackIdx_TaskID");

                entity.Property(e => e.TaskStackId).HasColumnName("TaskStackID");
                entity.Property(e => e.TaskId).HasColumnName("TaskID");

                entity.HasOne(d => d.Task).WithMany(p => p.BackgroundTaskStacks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Backgroun__TaskI__098A4168");
            });

            modelBuilder.Entity<BlackBerryUser>(entity =>
            {
                entity.HasIndex(e => e.AccountId, "BlackBerryUsersIdx_AccountId");

                entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Account).WithMany(p => p.BlackBerryUsers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlackBerryUsers_ExchangeAccounts");
            });

            modelBuilder.Entity<Cluster>(entity =>
            {
                entity.Property(e => e.ClusterId).HasColumnName("ClusterID");
                entity.Property(e => e.ClusterName)
                .IsRequired()
                .HasMaxLength(100);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasIndex(e => e.UserId, "CommentsIdx_UserID");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");
                entity.Property(e => e.CommentText).HasMaxLength(1000);
                entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.ItemTypeId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ItemTypeID");
                entity.Property(e => e.SeverityId).HasColumnName("SeverityID");
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User).WithMany(p => p.CommentsNavigation)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Comments_Users");
            });

            modelBuilder.Entity<Crmuser>(entity =>
            {
                entity.ToTable("CRMUsers");

                entity.HasIndex(e => e.AccountId, "CRMUsersIdx_AccountID");

                entity.Property(e => e.CrmuserId).HasColumnName("CRMUserID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");
                entity.Property(e => e.Caltype).HasColumnName("CALType");
                entity.Property(e => e.ChangedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
                entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
                entity.Property(e => e.CrmuserGuid).HasColumnName("CRMUserGuid");

                entity.HasOne(d => d.Account).WithMany(p => p.Crmusers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMUsers_ExchangeAccounts");
            });

            modelBuilder.Entity<Domain>(entity =>
            {
                entity.HasIndex(e => e.MailDomainId, "DomainsIdx_MailDomainID");

                entity.HasIndex(e => e.PackageId, "DomainsIdx_PackageID");

                entity.HasIndex(e => e.WebSiteId, "DomainsIdx_WebSiteID");

                entity.HasIndex(e => e.ZoneItemId, "DomainsIdx_ZoneItemID");

                entity.Property(e => e.DomainId).HasColumnName("DomainID");
                entity.Property(e => e.CreationDate).HasColumnType("datetime");
                entity.Property(e => e.DomainName)
                .IsRequired()
                .HasMaxLength(100);
                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.MailDomainId).HasColumnName("MailDomainID");
                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.WebSiteId).HasColumnName("WebSiteID");
                entity.Property(e => e.ZoneItemId).HasColumnName("ZoneItemID");

                entity.HasOne(d => d.MailDomain).WithMany(p => p.DomainMailDomains)
                .HasForeignKey(d => d.MailDomainId)
                .HasConstraintName("FK_Domains_ServiceItems_MailDomain");

                entity.HasOne(d => d.Package).WithMany(p => p.Domains)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_Domains_Packages");

                entity.HasOne(d => d.WebSite).WithMany(p => p.DomainWebSites)
                .HasForeignKey(d => d.WebSiteId)
                .HasConstraintName("FK_Domains_ServiceItems_WebSite");

                entity.HasOne(d => d.ZoneItem).WithMany(p => p.DomainZoneItems)
                .HasForeignKey(d => d.ZoneItemId)
                .HasConstraintName("FK_Domains_ServiceItems_ZoneItem");
            });

            modelBuilder.Entity<DomainDnsRecord>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__DomainDn__3214EC2758B0A6F1");

                entity.HasIndex(e => e.DomainId, "DomainDnsRecordsIdx_DomainId");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Date).HasColumnType("datetime");
                entity.Property(e => e.DnsServer).HasMaxLength(255);
                entity.Property(e => e.Value).HasMaxLength(255);

                entity.HasOne(d => d.Domain).WithMany(p => p.DomainDnsRecords)
                .HasForeignKey(d => d.DomainId)
                .HasConstraintName("FK_DomainDnsRecords_DomainId");
            });

            modelBuilder.Entity<EnterpriseFolder>(entity =>
            {
                entity.HasIndex(e => e.StorageSpaceFolderId, "EnterpriseFoldersIdx_StorageSpaceFolderId");

                entity.Property(e => e.EnterpriseFolderId).HasColumnName("EnterpriseFolderID");
                entity.Property(e => e.Domain).HasMaxLength(255);
                entity.Property(e => e.FolderName)
                .IsRequired()
                .HasMaxLength(255);
                entity.Property(e => e.HomeFolder).HasMaxLength(255);
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.LocationDrive).HasMaxLength(255);

                entity.HasOne(d => d.StorageSpaceFolder).WithMany(p => p.EnterpriseFolders)
                .HasForeignKey(d => d.StorageSpaceFolderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_EnterpriseFolders_StorageSpaceFolderId");
            });

            modelBuilder.Entity<EnterpriseFoldersOwaPermission>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Enterpri__3214EC27D1B48691");

                entity.HasIndex(e => e.AccountId, "EnterpriseFoldersOwaPermissionsIdx_AccountID");

                entity.HasIndex(e => e.FolderId, "EnterpriseFoldersOwaPermissionsIdx_FolderID");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.FolderId).HasColumnName("FolderID");
                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.HasOne(d => d.Account).WithMany(p => p.EnterpriseFoldersOwaPermissions)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_EnterpriseFoldersOwaPermissions_AccountId");

                entity.HasOne(d => d.Folder).WithMany(p => p.EnterpriseFoldersOwaPermissions)
                .HasForeignKey(d => d.FolderId)
                .HasConstraintName("FK_EnterpriseFoldersOwaPermissions_FolderId");
            });

            modelBuilder.Entity<ExchangeAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.HasIndex(e => e.ItemId, "ExchangeAccountsIdx_ItemID");

                entity.HasIndex(e => e.MailboxPlanId, "ExchangeAccountsIdx_MailboxPlanId");

                entity.HasIndex(e => e.AccountName, "IX_ExchangeAccounts_UniqueAccountName").IsUnique();

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.AccountName)
                .IsRequired()
                .HasMaxLength(300);
                entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
                entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(300);
                entity.Property(e => e.IsVip).HasColumnName("IsVIP");
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.LevelId).HasColumnName("LevelID");
                entity.Property(e => e.MailboxManagerActions)
                .HasMaxLength(200)
                .IsUnicode(false);
                entity.Property(e => e.PrimaryEmailAddress).HasMaxLength(300);
                entity.Property(e => e.SamAccountName).HasMaxLength(100);
                entity.Property(e => e.SubscriberNumber).HasMaxLength(32);
                entity.Property(e => e.UserPrincipalName).HasMaxLength(300);

                entity.HasOne(d => d.Item).WithMany(p => p.ExchangeAccounts)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ExchangeAccounts_ServiceItems");

                entity.HasOne(d => d.MailboxPlan).WithMany(p => p.ExchangeAccounts)
                .HasForeignKey(d => d.MailboxPlanId)
                .HasConstraintName("FK_ExchangeAccounts_ExchangeMailboxPlans");
            });

            modelBuilder.Entity<ExchangeAccountEmailAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId);

                entity.HasIndex(e => e.AccountId, "ExchangeAccountEmailAddressesIdx_AccountID");

                entity.HasIndex(e => e.EmailAddress, "IX_ExchangeAccountEmailAddresses_UniqueEmail").IsUnique();

                entity.Property(e => e.AddressId).HasColumnName("AddressID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(300);

                entity.HasOne(d => d.Account).WithMany(p => p.ExchangeAccountEmailAddresses)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_ExchangeAccountEmailAddresses_ExchangeAccounts");
            });

            modelBuilder.Entity<ExchangeDeletedAccount>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Exchange__3214EC27EF1C22C1");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
                entity.Property(e => e.FileName).HasMaxLength(128);
                entity.Property(e => e.FolderName).HasMaxLength(128);
                entity.Property(e => e.OriginAt).HasColumnName("OriginAT");
                entity.Property(e => e.StoragePath).HasMaxLength(255);
            });

            modelBuilder.Entity<ExchangeDisclaimer>(entity =>
            {
                entity.Property(e => e.DisclaimerName)
                .IsRequired()
                .HasMaxLength(300);
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
            });

            modelBuilder.Entity<ExchangeMailboxPlan>(entity =>
            {
                entity.HasKey(e => e.MailboxPlanId);

                entity.HasIndex(e => e.ItemId, "ExchangeMailboxPlansIdx_ItemID");

                entity.HasIndex(e => e.MailboxPlanId, "IX_ExchangeMailboxPlans").IsUnique();

                entity.Property(e => e.ArchiveSizeMb).HasColumnName("ArchiveSizeMB");
                entity.Property(e => e.EnableImap).HasColumnName("EnableIMAP");
                entity.Property(e => e.EnableMapi).HasColumnName("EnableMAPI");
                entity.Property(e => e.EnableOwa).HasColumnName("EnableOWA");
                entity.Property(e => e.EnablePop).HasColumnName("EnablePOP");
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.LitigationHoldMsg).HasMaxLength(512);
                entity.Property(e => e.LitigationHoldUrl).HasMaxLength(256);
                entity.Property(e => e.MailboxPlan)
                .IsRequired()
                .HasMaxLength(300);
                entity.Property(e => e.MailboxSizeMb).HasColumnName("MailboxSizeMB");
                entity.Property(e => e.MaxReceiveMessageSizeKb).HasColumnName("MaxReceiveMessageSizeKB");
                entity.Property(e => e.MaxSendMessageSizeKb).HasColumnName("MaxSendMessageSizeKB");

                entity.HasOne(d => d.Item).WithMany(p => p.ExchangeMailboxPlans)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ExchangeMailboxPlans_ExchangeOrganizations");
            });

            modelBuilder.Entity<ExchangeMailboxPlanRetentionPolicyTag>(entity =>
            {
                entity.HasKey(e => e.PlanTagId).HasName("PK__Exchange__E467073C50CD805B");

                entity.Property(e => e.PlanTagId).HasColumnName("PlanTagID");
                entity.Property(e => e.TagId).HasColumnName("TagID");
            });

            modelBuilder.Entity<ExchangeOrganization>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.HasIndex(e => e.OrganizationId, "IX_ExchangeOrganizations_UniqueOrg").IsUnique();

                entity.Property(e => e.ItemId)
                .ValueGeneratedNever()
                .HasColumnName("ItemID");
                entity.Property(e => e.ExchangeMailboxPlanId).HasColumnName("ExchangeMailboxPlanID");
                entity.Property(e => e.LyncUserPlanId).HasColumnName("LyncUserPlanID");
                entity.Property(e => e.OrganizationId)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("OrganizationID");
                entity.Property(e => e.SfBuserPlanId).HasColumnName("SfBUserPlanID");

                entity.HasOne(d => d.Item).WithOne(p => p.ExchangeOrganization)
                .HasForeignKey<ExchangeOrganization>(d => d.ItemId)
                .HasConstraintName("FK_ExchangeOrganizations_ServiceItems");
            });

            modelBuilder.Entity<ExchangeOrganizationDomain>(entity =>
            {
                entity.HasKey(e => e.OrganizationDomainId);

                entity.HasIndex(e => e.ItemId, "ExchangeOrganizationDomainsIdx_ItemID");

                entity.HasIndex(e => e.DomainId, "IX_ExchangeOrganizationDomains_UniqueDomain").IsUnique();

                entity.Property(e => e.OrganizationDomainId).HasColumnName("OrganizationDomainID");
                entity.Property(e => e.DomainId).HasColumnName("DomainID");
                entity.Property(e => e.DomainTypeId).HasColumnName("DomainTypeID");
                entity.Property(e => e.IsHost).HasDefaultValue(false);
                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationDomains)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ExchangeOrganizationDomains_ServiceItems");
            });

            modelBuilder.Entity<ExchangeOrganizationSetting>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.ItemId, "ExchangeOrganizationSettingsIdx_ItemId");

                entity.Property(e => e.SettingsName)
                .IsRequired()
                .HasMaxLength(100);
                entity.Property(e => e.Xml).IsRequired();

                entity.HasOne(d => d.Item).WithMany().HasForeignKey(d => d.ItemId);
            });

            modelBuilder.Entity<ExchangeOrganizationSsFolder>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Exchange__3214EC072DDBA072");

                entity.HasIndex(e => e.ItemId, "ExchangeOrganizationSsFoldersIdx_ItemId");

                entity.HasIndex(e => e.StorageSpaceFolderId, "ExchangeOrganizationSsFoldersIdx_StorageSpaceFolderId");

                entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationSsFolders)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ExchangeOrganizationSsFolders_ItemId");

                entity.HasOne(d => d.StorageSpaceFolder).WithMany(p => p.ExchangeOrganizationSsFolders)
                .HasForeignKey(d => d.StorageSpaceFolderId)
                .HasConstraintName("FK_ExchangeOrganizationSsFolders_StorageSpaceFolderId");
            });

            modelBuilder.Entity<ExchangeRetentionPolicyTag>(entity =>
            {
                entity.HasKey(e => e.TagId).HasName("PK__Exchange__657CFA4C02667D37");

                entity.Property(e => e.TagId).HasColumnName("TagID");
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.TagName).HasMaxLength(255);
            });

            modelBuilder.Entity<GlobalDnsRecord>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.HasIndex(e => e.IpaddressId, "GlobalDnsRecordsIdx_IPAddressID");

                entity.HasIndex(e => e.PackageId, "GlobalDnsRecordsIdx_PackageID");

                entity.HasIndex(e => e.ServerId, "GlobalDnsRecordsIdx_ServerID");

                entity.HasIndex(e => e.ServiceId, "GlobalDnsRecordsIdx_ServiceID");

                entity.Property(e => e.RecordId).HasColumnName("RecordID");
                entity.Property(e => e.IpaddressId).HasColumnName("IPAddressID");
                entity.Property(e => e.Mxpriority).HasColumnName("MXPriority");
                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.RecordData)
                .IsRequired()
                .HasMaxLength(500);
                entity.Property(e => e.RecordName)
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.RecordType)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);
                entity.Property(e => e.ServerId).HasColumnName("ServerID");
                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.Ipaddress).WithMany(p => p.GlobalDnsRecords)
                .HasForeignKey(d => d.IpaddressId)
                .HasConstraintName("FK_GlobalDnsRecords_IPAddresses");

                entity.HasOne(d => d.Package).WithMany(p => p.GlobalDnsRecords)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Packages");

                entity.HasOne(d => d.Server).WithMany(p => p.GlobalDnsRecords)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("FK_GlobalDnsRecords_Servers");

                entity.HasOne(d => d.Service).WithMany(p => p.GlobalDnsRecords)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_GlobalDnsRecords_Services");
            });

            modelBuilder.Entity<HostingPlan>(entity =>
            {
                entity.HasKey(e => e.PlanId);

                entity.HasIndex(e => e.PackageId, "HostingPlansIdx_PackageID");

                entity.HasIndex(e => e.ServerId, "HostingPlansIdx_ServerID");

                entity.HasIndex(e => e.UserId, "HostingPlansIdx_UserID");

                entity.Property(e => e.PlanId).HasColumnName("PlanID");
                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.PlanDescription).HasColumnType("ntext");
                entity.Property(e => e.PlanName)
                .IsRequired()
                .HasMaxLength(200);
                entity.Property(e => e.RecurringPrice).HasColumnType("money");
                entity.Property(e => e.ServerId).HasColumnName("ServerID");
                entity.Property(e => e.SetupPrice).HasColumnType("money");
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Package).WithMany(p => p.HostingPlans)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HostingPlans_Packages");

                entity.HasOne(d => d.Server).WithMany(p => p.HostingPlans)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("FK_HostingPlans_Servers");

                entity.HasOne(d => d.User).WithMany(p => p.HostingPlans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_HostingPlans_Users");
            });

            modelBuilder.Entity<HostingPlanQuota>(entity =>
            {
                entity.HasKey(e => new { e.PlanId, e.QuotaId }).HasName("PK_HostingPlanQuotas_1");

                entity.Property(e => e.PlanId).HasColumnName("PlanID");
                entity.Property(e => e.QuotaId).HasColumnName("QuotaID");

                entity.HasOne(d => d.Plan).WithMany(p => p.HostingPlanQuota)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_HostingPlanQuotas_HostingPlans");

                entity.HasOne(d => d.Quota).WithMany(p => p.HostingPlanQuota)
                .HasForeignKey(d => d.QuotaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostingPlanQuotas_Quotas");
            });

            modelBuilder.Entity<HostingPlanResource>(entity =>
            {
                entity.HasKey(e => new { e.PlanId, e.GroupId });

                entity.Property(e => e.PlanId).HasColumnName("PlanID");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Group).WithMany(p => p.HostingPlanResources)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HostingPlanResources_ResourceGroups");

                entity.HasOne(d => d.Plan).WithMany(p => p.HostingPlanResources)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_HostingPlanResources_HostingPlans");
            });

            modelBuilder.Entity<Ipaddress>(entity =>
            {
                entity.HasKey(e => e.AddressId);

                entity.ToTable("IPAddresses");

                entity.HasIndex(e => e.ServerId, "IPAddressesIdx_ServerID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");
                entity.Property(e => e.Comments).HasColumnType("ntext");
                entity.Property(e => e.DefaultGateway)
                .HasMaxLength(15)
                .IsUnicode(false);
                entity.Property(e => e.ExternalIp)
                .IsRequired()
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("ExternalIP");
                entity.Property(e => e.InternalIp)
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("InternalIP");
                entity.Property(e => e.PoolId).HasColumnName("PoolID");
                entity.Property(e => e.ServerId).HasColumnName("ServerID");
                entity.Property(e => e.SubnetMask)
                .HasMaxLength(15)
                .IsUnicode(false);
                entity.Property(e => e.Vlan).HasColumnName("VLAN");

                entity.HasOne(d => d.Server).WithMany(p => p.Ipaddresses)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_IPAddresses_Servers");
            });

            modelBuilder.Entity<LyncUser>(entity =>
            {
                entity.HasIndex(e => e.LyncUserPlanId, "LyncUsersIdx_LyncUserPlanID");

                entity.Property(e => e.LyncUserId).HasColumnName("LyncUserID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
                entity.Property(e => e.LyncUserPlanId).HasColumnName("LyncUserPlanID");
                entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
                entity.Property(e => e.SipAddress).HasMaxLength(300);

                entity.HasOne(d => d.LyncUserPlan).WithMany(p => p.LyncUsers)
                .HasForeignKey(d => d.LyncUserPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LyncUsers_LyncUserPlans");
            });

            modelBuilder.Entity<LyncUserPlan>(entity =>
            {
                entity.HasIndex(e => e.LyncUserPlanId, "IX_LyncUserPlans").IsUnique();

                entity.HasIndex(e => e.ItemId, "LyncUserPlansIdx_ItemID");

                entity.Property(e => e.ArchivePolicy).HasMaxLength(300);
                entity.Property(e => e.Im).HasColumnName("IM");
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.LyncUserPlanName)
                .IsRequired()
                .HasMaxLength(300);
                entity.Property(e => e.PublicImconnectivity).HasColumnName("PublicIMConnectivity");
                entity.Property(e => e.ServerUri)
                .HasMaxLength(300)
                .HasColumnName("ServerURI");
                entity.Property(e => e.TelephonyDialPlanPolicy).HasMaxLength(300);
                entity.Property(e => e.TelephonyVoicePolicy).HasMaxLength(300);

                entity.HasOne(d => d.Item).WithMany(p => p.LyncUserPlans)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_LyncUserPlans_ExchangeOrganizations");
            });

            modelBuilder.Entity<Ocsuser>(entity =>
            {
                entity.ToTable("OCSUsers");

                entity.Property(e => e.OcsuserId).HasColumnName("OCSUserID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
                entity.Property(e => e.InstanceId)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("InstanceID");
                entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.ToTable(tb => tb.HasTrigger("Update_StatusIDchangeDate"));

                entity.HasIndex(e => e.ParentPackageId, "PackageIndex_ParentPackageID");

                entity.HasIndex(e => e.PlanId, "PackageIndex_PlanID");

                entity.HasIndex(e => e.ServerId, "PackageIndex_ServerID");

                entity.HasIndex(e => e.UserId, "PackageIndex_UserID");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.BandwidthUpdated).HasColumnType("datetime");
                entity.Property(e => e.PackageComments).HasColumnType("ntext");
                entity.Property(e => e.PackageName).HasMaxLength(300);
                entity.Property(e => e.ParentPackageId).HasColumnName("ParentPackageID");
                entity.Property(e => e.PlanId).HasColumnName("PlanID");
                entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
                entity.Property(e => e.ServerId).HasColumnName("ServerID");
                entity.Property(e => e.StatusId).HasColumnName("StatusID");
                entity.Property(e => e.StatusIdchangeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("StatusIDchangeDate");
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.ParentPackage).WithMany(p => p.InverseParentPackage)
                .HasForeignKey(d => d.ParentPackageId)
                .HasConstraintName("FK_Packages_Packages");

                entity.HasOne(d => d.Plan).WithMany(p => p.Packages)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_Packages_HostingPlans");

                entity.HasOne(d => d.Server).WithMany(p => p.Packages)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("FK_Packages_Servers");

                entity.HasOne(d => d.User).WithMany(p => p.Packages)
                .HasForeignKey(d => d.UserId)
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
                entity.HasIndex(e => e.PackageId, "PackageAddonsIdx_PackageID");

                entity.HasIndex(e => e.PlanId, "PackageAddonsIdx_PlanID");

                entity.Property(e => e.PackageAddonId).HasColumnName("PackageAddonID");
                entity.Property(e => e.Comments).HasColumnType("ntext");
                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.PlanId).HasColumnName("PlanID");
                entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Package).WithMany(p => p.PackageAddons)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PackageAddons_Packages");

                entity.HasOne(d => d.Plan).WithMany(p => p.PackageAddons)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("FK_PackageAddons_HostingPlans");
            });

            modelBuilder.Entity<PackageIpaddress>(entity =>
            {
                entity.HasKey(e => e.PackageAddressId);

                entity.ToTable("PackageIPAddresses");

                entity.HasIndex(e => e.AddressId, "PackageIPAddressesIdx_AddressID");

                entity.HasIndex(e => e.ItemId, "PackageIPAddressesIdx_ItemID");

                entity.HasIndex(e => e.PackageId, "PackageIPAddressesIdx_PackageID");

                entity.Property(e => e.PackageAddressId).HasColumnName("PackageAddressID");
                entity.Property(e => e.AddressId).HasColumnName("AddressID");
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.OrgId).HasColumnName("OrgID");
                entity.Property(e => e.PackageId).HasColumnName("PackageID");

                entity.HasOne(d => d.Address).WithMany(p => p.PackageIpaddresses)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageIPAddresses_IPAddresses");

                entity.HasOne(d => d.Item).WithMany(p => p.PackageIpaddresses)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_PackageIPAddresses_ServiceItems");

                entity.HasOne(d => d.Package).WithMany(p => p.PackageIpaddresses)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_PackageIPAddresses_Packages");
            });

            modelBuilder.Entity<PackageQuota>(entity =>
            {
                entity.HasKey(e => new { e.PackageId, e.QuotaId });

                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.QuotaId).HasColumnName("QuotaID");

                entity.HasOne(d => d.Package).WithMany(p => p.PackageQuota)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageQuotas_Packages");

                entity.HasOne(d => d.Quota).WithMany(p => p.PackageQuota)
                .HasForeignKey(d => d.QuotaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageQuotas_Quotas");
            });

            modelBuilder.Entity<PackageResource>(entity =>
            {
                entity.HasKey(e => new { e.PackageId, e.GroupId }).HasName("PK_PackageResources_1");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Group).WithMany(p => p.PackageResources)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageResources_ResourceGroups");

                entity.HasOne(d => d.Package).WithMany(p => p.PackageResources)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageResources_Packages");
            });

            modelBuilder.Entity<PackageSetting>(entity =>
            {
                entity.HasKey(e => new { e.PackageId, e.SettingsName, e.PropertyName });

                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.SettingsName).HasMaxLength(50);
                entity.Property(e => e.PropertyName).HasMaxLength(50);
                entity.Property(e => e.PropertyValue).HasColumnType("ntext");
            });

            modelBuilder.Entity<PackageVlan>(entity =>
            {
                entity.HasKey(e => e.PackageVlanId).HasName("PK__PackageV__A9AABBF9C0C25CB3");

                entity.ToTable("PackageVLANs");

                entity.HasIndex(e => e.PackageId, "PackageVLANsIdx_PackageID");

                entity.HasIndex(e => e.VlanId, "PackageVLANsIdx_VlanID");

                entity.Property(e => e.PackageVlanId).HasColumnName("PackageVlanID");
                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.VlanId).HasColumnName("VlanID");

                entity.HasOne(d => d.Package).WithMany(p => p.PackageVlans)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_PackageID");

                entity.HasOne(d => d.Vlan).WithMany(p => p.PackageVlans)
                .HasForeignKey(d => d.VlanId)
                .HasConstraintName("FK_VlanID");
            });

            modelBuilder.Entity<PackagesBandwidth>(entity =>
            {
                entity.HasKey(e => new { e.PackageId, e.GroupId, e.LogDate });

                entity.ToTable("PackagesBandwidth");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.LogDate).HasColumnType("datetime");

                entity.HasOne(d => d.Group).WithMany(p => p.PackagesBandwidths)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesBandwidth_ResourceGroups");

                entity.HasOne(d => d.Package).WithMany(p => p.PackagesBandwidths)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesBandwidth_Packages");
            });

            modelBuilder.Entity<PackagesDiskspace>(entity =>
            {
                entity.HasKey(e => new { e.PackageId, e.GroupId });

                entity.ToTable("PackagesDiskspace");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Group).WithMany(p => p.PackagesDiskspaces)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesDiskspace_ResourceGroups");

                entity.HasOne(d => d.Package).WithMany(p => p.PackagesDiskspaces)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesDiskspace_Packages");
            });

            modelBuilder.Entity<PackagesTreeCache>(entity =>
            {
                entity
                .HasNoKey()
                .ToTable("PackagesTreeCache");

                entity.HasIndex(e => new { e.ParentPackageId, e.PackageId }, "PackagesTreeCacheIndex").IsClustered();

                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.ParentPackageId).HasColumnName("ParentPackageID");

                entity.HasOne(d => d.Package).WithMany()
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesTreeCache_Packages1");

                entity.HasOne(d => d.ParentPackage).WithMany()
                .HasForeignKey(d => d.ParentPackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagesTreeCache_Packages");
            });

            modelBuilder.Entity<PrivateIpaddress>(entity =>
            {
                entity.HasKey(e => e.PrivateAddressId);

                entity.ToTable("PrivateIPAddresses");

                entity.HasIndex(e => e.ItemId, "PrivateIPAddressesIdx_ItemID");

                entity.Property(e => e.PrivateAddressId).HasColumnName("PrivateAddressID");
                entity.Property(e => e.Ipaddress)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.HasOne(d => d.Item).WithMany(p => p.PrivateIpaddresses)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_PrivateIPAddresses_ServiceItems");
            });

            modelBuilder.Entity<PrivateNetworkVlan>(entity =>
            {
                entity.HasKey(e => e.VlanId).HasName("PK__PrivateN__8348135581B53618");

                entity.ToTable("PrivateNetworkVLANs");

                entity.HasIndex(e => e.ServerId, "PrivateNetworkVLANsIdx_ServerID");

                entity.Property(e => e.VlanId).HasColumnName("VlanID");
                entity.Property(e => e.Comments).HasColumnType("ntext");
                entity.Property(e => e.ServerId).HasColumnName("ServerID");

                entity.HasOne(d => d.Server).WithMany(p => p.PrivateNetworkVlans)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ServerID");
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(e => e.ProviderId).HasName("PK_ServiceTypes");

                entity.HasIndex(e => e.GroupId, "ProvidersIdx_GroupID");

                entity.Property(e => e.ProviderId)
                .ValueGeneratedNever()
                .HasColumnName("ProviderID");
                entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(200);
                entity.Property(e => e.EditorControl).HasMaxLength(100);
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.ProviderName).HasMaxLength(100);
                entity.Property(e => e.ProviderType).HasMaxLength(400);

                entity.HasOne(d => d.Group).WithMany(p => p.Providers)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Providers_ResourceGroups");
            });

            modelBuilder.Entity<Quota>(entity =>
            {
                entity.HasIndex(e => e.GroupId, "QuotasIdx_GroupID");

                entity.HasIndex(e => e.ItemTypeId, "QuotasIdx_ItemTypeID");

                entity.Property(e => e.QuotaId)
                .ValueGeneratedNever()
                .HasColumnName("QuotaID");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");
                entity.Property(e => e.QuotaDescription).HasMaxLength(200);
                entity.Property(e => e.QuotaName)
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.QuotaOrder).HasDefaultValue(1);
                entity.Property(e => e.QuotaTypeId)
                .HasDefaultValue(2)
                .HasColumnName("QuotaTypeID");
                entity.Property(e => e.ServiceQuota).HasDefaultValue(false);

                entity.HasOne(d => d.Group).WithMany(p => p.Quota)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_Quotas_ResourceGroups");

                entity.HasOne(d => d.ItemType).WithMany(p => p.Quota)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK_Quotas_ServiceItemTypes");
            });

            modelBuilder.Entity<Rdscertificate>(entity =>
            {
                entity.ToTable("RDSCertificates");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Content)
                .IsRequired()
                .HasColumnType("ntext");
                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
                entity.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(255);
                entity.Property(e => e.Hash)
                .IsRequired()
                .HasMaxLength(255);
                entity.Property(e => e.ValidFrom).HasColumnType("datetime");
            });

            modelBuilder.Entity<Rdscollection>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__RDSColle__3214EC27346D361D");

                entity.ToTable("RDSCollections");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.DisplayName).HasMaxLength(255);
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<RdscollectionSetting>(entity =>
            {
                entity.ToTable("RDSCollectionSettings");

                entity.HasIndex(e => e.RdscollectionId, "RDSCollectionSettingsIdx_RDSCollectionId");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AuthenticateUsingNla).HasColumnName("AuthenticateUsingNLA");
                entity.Property(e => e.BrokenConnectionAction).HasMaxLength(20);
                entity.Property(e => e.ClientDeviceRedirectionOptions).HasMaxLength(250);
                entity.Property(e => e.EncryptionLevel).HasMaxLength(20);
                entity.Property(e => e.RdeasyPrintDriverEnabled).HasColumnName("RDEasyPrintDriverEnabled");
                entity.Property(e => e.RdscollectionId).HasColumnName("RDSCollectionId");
                entity.Property(e => e.SecurityLayer).HasMaxLength(20);

                entity.HasOne(d => d.Rdscollection).WithMany(p => p.RdscollectionSettings)
                .HasForeignKey(d => d.RdscollectionId)
                .HasConstraintName("FK_RDSCollectionSettings_RDSCollections");
            });

            modelBuilder.Entity<RdscollectionUser>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__RDSColle__3214EC2780141EF7");

                entity.ToTable("RDSCollectionUsers");

                entity.HasIndex(e => e.AccountId, "RDSCollectionUsersIdx_AccountID");

                entity.HasIndex(e => e.RdscollectionId, "RDSCollectionUsersIdx_RDSCollectionId");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.RdscollectionId).HasColumnName("RDSCollectionId");

                entity.HasOne(d => d.Account).WithMany(p => p.RdscollectionUsers)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_RDSCollectionUsers_UserId");

                entity.HasOne(d => d.Rdscollection).WithMany(p => p.RdscollectionUsers)
                .HasForeignKey(d => d.RdscollectionId)
                .HasConstraintName("FK_RDSCollectionUsers_RDSCollectionId");
            });

            modelBuilder.Entity<Rdsmessage>(entity =>
            {
                entity.ToTable("RDSMessages");

                entity.HasIndex(e => e.RdscollectionId, "RDSMessagesIdx_RDSCollectionId");

                entity.Property(e => e.Date).HasColumnType("datetime");
                entity.Property(e => e.MessageText)
                .IsRequired()
                .HasColumnType("ntext");
                entity.Property(e => e.RdscollectionId).HasColumnName("RDSCollectionId");
                entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(250)
                .IsFixedLength();

                entity.HasOne(d => d.Rdscollection).WithMany(p => p.Rdsmessages)
                .HasForeignKey(d => d.RdscollectionId)
                .HasConstraintName("FK_RDSMessages_RDSCollections");
            });

            modelBuilder.Entity<Rdsserver>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__RDSServe__3214EC27DBEBD4B5");

                entity.ToTable("RDSServers");

                entity.HasIndex(e => e.RdscollectionId, "RDSServersIdx_RDSCollectionId");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ConnectionEnabled).HasDefaultValue(true);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.FqdName).HasMaxLength(255);
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.RdscollectionId).HasColumnName("RDSCollectionId");

                entity.HasOne(d => d.Rdscollection).WithMany(p => p.Rdsservers)
                .HasForeignKey(d => d.RdscollectionId)
                .HasConstraintName("FK_RDSServers_RDSCollectionId");
            });

            modelBuilder.Entity<RdsserverSetting>(entity =>
            {
                entity.HasKey(e => new { e.RdsServerId, e.SettingsName, e.PropertyName });

                entity.ToTable("RDSServerSettings");

                entity.Property(e => e.SettingsName).HasMaxLength(50);
                entity.Property(e => e.PropertyName).HasMaxLength(50);
                entity.Property(e => e.PropertyValue).HasColumnType("ntext");
            });

            modelBuilder.Entity<ResourceGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.Property(e => e.GroupId)
                .ValueGeneratedNever()
                .HasColumnName("GroupID");
                entity.Property(e => e.GroupController).HasMaxLength(1000);
                entity.Property(e => e.GroupName)
                .IsRequired()
                .HasMaxLength(100);
                entity.Property(e => e.GroupOrder).HasDefaultValue(1);
            });

            modelBuilder.Entity<ResourceGroupDnsRecord>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.HasIndex(e => e.GroupId, "ResourceGroupDnsRecordsIdx_GroupID");

                entity.Property(e => e.RecordId).HasColumnName("RecordID");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.Mxpriority).HasColumnName("MXPriority");
                entity.Property(e => e.RecordData)
                .IsRequired()
                .HasMaxLength(200);
                entity.Property(e => e.RecordName)
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.RecordOrder).HasDefaultValue(1);
                entity.Property(e => e.RecordType)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

                entity.HasOne(d => d.Group).WithMany(p => p.ResourceGroupDnsRecords)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_ResourceGroupDnsRecords_ResourceGroups");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.HasIndex(e => e.PackageId, "ScheduleIdx_PackageID");

                entity.HasIndex(e => e.TaskId, "ScheduleIdx_TaskID");

                entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
                entity.Property(e => e.FromTime).HasColumnType("datetime");
                entity.Property(e => e.LastRun).HasColumnType("datetime");
                entity.Property(e => e.NextRun).HasColumnType("datetime");
                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.PriorityId)
                .HasMaxLength(50)
                .HasColumnName("PriorityID");
                entity.Property(e => e.ScheduleName).HasMaxLength(100);
                entity.Property(e => e.ScheduleTypeId)
                .HasMaxLength(50)
                .HasColumnName("ScheduleTypeID");
                entity.Property(e => e.StartTime).HasColumnType("datetime");
                entity.Property(e => e.TaskId)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("TaskID");
                entity.Property(e => e.ToTime).HasColumnType("datetime");

                entity.HasOne(d => d.Package).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Schedule_Packages");

                entity.HasOne(d => d.Task).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_ScheduleTasks");
            });

            modelBuilder.Entity<ScheduleParameter>(entity =>
            {
                entity.HasKey(e => new { e.ScheduleId, e.ParameterId });

                entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
                entity.Property(e => e.ParameterId)
                .HasMaxLength(100)
                .HasColumnName("ParameterID");
                entity.Property(e => e.ParameterValue).HasMaxLength(1000);

                entity.HasOne(d => d.Schedule).WithMany(p => p.ScheduleParameters)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK_ScheduleParameters_Schedule");
            });

            modelBuilder.Entity<ScheduleTask>(entity =>
            {
                entity.HasKey(e => e.TaskId);

                entity.Property(e => e.TaskId)
                .HasMaxLength(100)
                .HasColumnName("TaskID");
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.TaskType)
                .IsRequired()
                .HasMaxLength(500);
            });

            modelBuilder.Entity<ScheduleTaskParameter>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.ParameterId });

                entity.Property(e => e.TaskId)
                .HasMaxLength(100)
                .HasColumnName("TaskID");
                entity.Property(e => e.ParameterId)
                .HasMaxLength(100)
                .HasColumnName("ParameterID");
                entity.Property(e => e.DataTypeId)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("DataTypeID");

                entity.HasOne(d => d.Task).WithMany(p => p.ScheduleTaskParameters)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleTaskParameters_ScheduleTasks");
            });

            modelBuilder.Entity<ScheduleTaskViewConfiguration>(entity =>
            {
                entity.HasKey(e => new { e.ConfigurationId, e.TaskId });

                entity.ToTable("ScheduleTaskViewConfiguration");

                entity.Property(e => e.ConfigurationId)
                .HasMaxLength(100)
                .HasColumnName("ConfigurationID");
                entity.Property(e => e.TaskId)
                .HasMaxLength(100)
                .HasColumnName("TaskID");
                entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(100);
                entity.Property(e => e.Environment)
                .IsRequired()
                .HasMaxLength(100);

                entity.HasOne(d => d.Task).WithMany(p => p.ScheduleTaskViewConfigurations)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ScheduleTaskViewConfiguration_ScheduleTaskViewConfiguration");
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasIndex(e => e.PrimaryGroupId, "ServersIdx_PrimaryGroupID");

                entity.Property(e => e.ServerId).HasColumnName("ServerID");
                entity.Property(e => e.AdParentDomain).HasMaxLength(200);
                entity.Property(e => e.AdParentDomainController).HasMaxLength(200);
                entity.Property(e => e.AdauthenticationType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ADAuthenticationType");
                entity.Property(e => e.Adenabled)
                .HasDefaultValue(false)
                .HasColumnName("ADEnabled");
                entity.Property(e => e.Adpassword)
                .HasMaxLength(100)
                .HasColumnName("ADPassword");
                entity.Property(e => e.AdrootDomain)
                .HasMaxLength(200)
                .HasColumnName("ADRootDomain");
                entity.Property(e => e.Adusername)
                .HasMaxLength(100)
                .HasColumnName("ADUsername");
                entity.Property(e => e.Comments).HasColumnType("ntext");
                entity.Property(e => e.InstantDomainAlias).HasMaxLength(200);
                entity.Property(e => e.Osplatform).HasColumnName("OSPlatform");
                entity.Property(e => e.Password).HasMaxLength(100);
                entity.Property(e => e.PasswordIsSha256).HasColumnName("PasswordIsSHA256");
                entity.Property(e => e.PrimaryGroupId).HasColumnName("PrimaryGroupID");
                entity.Property(e => e.ServerName)
                .IsRequired()
                .HasMaxLength(100);
                entity.Property(e => e.ServerUrl)
                .HasMaxLength(255)
                .HasDefaultValue("");

                entity.HasOne(d => d.PrimaryGroup).WithMany(p => p.Servers)
                .HasForeignKey(d => d.PrimaryGroupId)
                .HasConstraintName("FK_Servers_ResourceGroups");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasIndex(e => e.ClusterId, "ServicesIdx_ClusterID");

                entity.HasIndex(e => e.ProviderId, "ServicesIdx_ProviderID");

                entity.HasIndex(e => e.ServerId, "ServicesIdx_ServerID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
                entity.Property(e => e.ClusterId).HasColumnName("ClusterID");
                entity.Property(e => e.Comments).HasColumnType("ntext");
                entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
                entity.Property(e => e.ServerId).HasColumnName("ServerID");
                entity.Property(e => e.ServiceName)
                .IsRequired()
                .HasMaxLength(50);

                entity.HasOne(d => d.Cluster).WithMany(p => p.Services)
                .HasForeignKey(d => d.ClusterId)
                .HasConstraintName("FK_Services_Clusters");

                entity.HasOne(d => d.Provider).WithMany(p => p.Services)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Providers");

                entity.HasOne(d => d.Server).WithMany(p => p.Services)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Services_Servers");
            });

            modelBuilder.Entity<ServiceDefaultProperty>(entity =>
            {
                entity.HasKey(e => new { e.ProviderId, e.PropertyName }).HasName("PK_ServiceDefaultProperties_1");

                entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
                entity.Property(e => e.PropertyName).HasMaxLength(50);
                entity.Property(e => e.PropertyValue).HasMaxLength(1000);

                entity.HasOne(d => d.Provider).WithMany(p => p.ServiceDefaultProperties)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceDefaultProperties_Providers");
            });

            modelBuilder.Entity<ServiceItem>(entity =>
            {
                entity.HasKey(e => e.ItemId);

                entity.HasIndex(e => e.ItemTypeId, "ServiceItemsIdx_ItemTypeID");

                entity.HasIndex(e => e.PackageId, "ServiceItemsIdx_PackageID");

                entity.HasIndex(e => e.ServiceId, "ServiceItemsIdx_ServiceID");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ItemName).HasMaxLength(500);
                entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");
                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.ItemType).WithMany(p => p.ServiceItems)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK_ServiceItems_ServiceItemTypes");

                entity.HasOne(d => d.Package).WithMany(p => p.ServiceItems)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_ServiceItems_Packages");

                entity.HasOne(d => d.Service).WithMany(p => p.ServiceItems)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_ServiceItems_Services");
            });

            modelBuilder.Entity<ServiceItemProperty>(entity =>
            {
                entity.HasKey(e => new { e.ItemId, e.PropertyName });

                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.PropertyName).HasMaxLength(50);

                entity.HasOne(d => d.Item).WithMany(p => p.ServiceItemProperties)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ServiceItemProperties_ServiceItems");
            });

            modelBuilder.Entity<ServiceItemType>(entity =>
            {
                entity.HasKey(e => e.ItemTypeId);

                entity.HasIndex(e => e.GroupId, "ServiceItemTypesIdx_GroupID");

                entity.Property(e => e.ItemTypeId)
                .ValueGeneratedNever()
                .HasColumnName("ItemTypeID");
                entity.Property(e => e.Backupable).HasDefaultValue(true);
                entity.Property(e => e.DisplayName).HasMaxLength(50);
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.Importable).HasDefaultValue(true);
                entity.Property(e => e.TypeName).HasMaxLength(200);
                entity.Property(e => e.TypeOrder).HasDefaultValue(1);

                entity.HasOne(d => d.Group).WithMany(p => p.ServiceItemTypes)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_ServiceItemTypes_ResourceGroups");
            });

            modelBuilder.Entity<ServiceProperty>(entity =>
            {
                entity.HasKey(e => new { e.ServiceId, e.PropertyName }).HasName("PK_ServiceProperties_1");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
                entity.Property(e => e.PropertyName).HasMaxLength(50);

                entity.HasOne(d => d.Service).WithMany(p => p.ServiceProperties)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_ServiceProperties_Services");
            });

            modelBuilder.Entity<SfBuser>(entity =>
            {
                entity.ToTable("SfBUsers");

                entity.Property(e => e.SfBuserId).HasColumnName("SfBUserID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.SfBuserPlanId).HasColumnName("SfBUserPlanID");
                entity.Property(e => e.SipAddress).HasMaxLength(300);
            });

            modelBuilder.Entity<SfBuserPlan>(entity =>
            {
                entity.ToTable("SfBUserPlans");

                entity.Property(e => e.SfBuserPlanId).HasColumnName("SfBUserPlanId");
                entity.Property(e => e.ArchivePolicy).HasMaxLength(300);
                entity.Property(e => e.Im).HasColumnName("IM");
                entity.Property(e => e.ItemId).HasColumnName("ItemID");
                entity.Property(e => e.PublicImconnectivity).HasColumnName("PublicIMConnectivity");
                entity.Property(e => e.ServerUri)
                .HasMaxLength(300)
                .HasColumnName("ServerURI");
                entity.Property(e => e.SfBuserPlanName)
                .IsRequired()
                .HasMaxLength(300)
                .HasColumnName("SfBUserPlanName");
                entity.Property(e => e.SfBuserPlanType).HasColumnName("SfBUserPlanType");
                entity.Property(e => e.TelephonyDialPlanPolicy).HasMaxLength(300);
                entity.Property(e => e.TelephonyVoicePolicy).HasMaxLength(300);
            });

            modelBuilder.Entity<Sslcertificate>(entity =>
            {
                entity
                .HasNoKey()
                .ToTable("SSLCertificates");

                entity.Property(e => e.Certificate).HasColumnType("ntext");
                entity.Property(e => e.Csr)
                .HasColumnType("ntext")
                .HasColumnName("CSR");
                entity.Property(e => e.Csrlength).HasColumnName("CSRLength");
                entity.Property(e => e.DistinguishedName).HasMaxLength(500);
                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
                entity.Property(e => e.FriendlyName).HasMaxLength(255);
                entity.Property(e => e.Hash).HasColumnType("ntext");
                entity.Property(e => e.Hostname).HasMaxLength(255);
                entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
                entity.Property(e => e.Pfx).HasColumnType("ntext");
                entity.Property(e => e.SerialNumber).HasMaxLength(250);
                entity.Property(e => e.SiteId).HasColumnName("SiteID");
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.ValidFrom).HasColumnType("datetime");
            });

            modelBuilder.Entity<StorageSpace>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__StorageS__3214EC07B8B9A6D1");

                entity.HasIndex(e => e.ServerId, "StorageSpacesIdx_ServerId");

                entity.HasIndex(e => e.ServiceId, "StorageSpacesIdx_ServiceId");

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false);
                entity.Property(e => e.Path)
                .IsRequired()
                .IsUnicode(false);
                entity.Property(e => e.UncPath).IsUnicode(false);

                entity.HasOne(d => d.Server).WithMany(p => p.StorageSpaces)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("FK_StorageSpaces_ServerId");

                entity.HasOne(d => d.Service).WithMany(p => p.StorageSpaces)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_StorageSpaces_ServiceId");
            });

            modelBuilder.Entity<StorageSpaceFolder>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__StorageS__3214EC07AC0C9EB6");

                entity.HasIndex(e => e.StorageSpaceId, "StorageSpaceFoldersIdx_StorageSpaceId");

                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false);
                entity.Property(e => e.Path)
                .IsRequired()
                .IsUnicode(false);
                entity.Property(e => e.UncPath).IsUnicode(false);

                entity.HasOne(d => d.StorageSpace).WithMany(p => p.StorageSpaceFolders)
                .HasForeignKey(d => d.StorageSpaceId)
                .HasConstraintName("FK_StorageSpaceFolders_StorageSpaceId");
            });

            modelBuilder.Entity<StorageSpaceLevel>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__StorageS__3214EC07B8D82363");

                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(300);
            });

            modelBuilder.Entity<StorageSpaceLevelResourceGroup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__StorageS__3214EC07EBEBED98");

                entity.HasIndex(e => e.GroupId, "StorageSpaceLevelResourceGroupsIdx_GroupId");

                entity.HasIndex(e => e.LevelId, "StorageSpaceLevelResourceGroupsIdx_LevelId");

                entity.HasOne(d => d.Group).WithMany(p => p.StorageSpaceLevelResourceGroups)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_StorageSpaceLevelResourceGroups_GroupId");

                entity.HasOne(d => d.Level).WithMany(p => p.StorageSpaceLevelResourceGroups)
                .HasForeignKey(d => d.LevelId)
                .HasConstraintName("FK_StorageSpaceLevelResourceGroups_LevelId");
            });

            modelBuilder.Entity<SupportServiceLevel>(entity =>
            {
                entity.HasKey(e => e.LevelId).HasName("PK__SupportS__09F03C065BA08AFB");

                entity.Property(e => e.LevelId).HasColumnName("LevelID");
                entity.Property(e => e.LevelDescription).HasMaxLength(1000);
                entity.Property(e => e.LevelName)
                .IsRequired()
                .HasMaxLength(100);
            });

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.HasKey(e => new { e.SettingsName, e.PropertyName });

                entity.Property(e => e.SettingsName).HasMaxLength(50);
                entity.Property(e => e.PropertyName).HasMaxLength(50);
                entity.Property(e => e.PropertyValue).HasColumnType("ntext");
            });

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.DisplayName).HasMaxLength(255);
                entity.Property(e => e.Ltrname)
                .HasMaxLength(255)
                .HasColumnName("LTRName");
                entity.Property(e => e.Rtlname)
                .HasMaxLength(255)
                .HasColumnName("RTLName");
                entity.Property(e => e.ThemeId).HasColumnName("ThemeID");
            });

            modelBuilder.Entity<ThemeSetting>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.PropertyName)
                .IsRequired()
                .HasMaxLength(255);
                entity.Property(e => e.PropertyValue)
                .IsRequired()
                .HasMaxLength(255);
                entity.Property(e => e.SettingsName)
                .IsRequired()
                .HasMaxLength(255);
                entity.Property(e => e.ThemeId).HasColumnName("ThemeID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();

                entity.HasIndex(e => e.OwnerId, "UsersIdx_OwnerID");

                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.Changed).HasColumnType("datetime");
                entity.Property(e => e.City).HasMaxLength(50);
                entity.Property(e => e.Comments).HasColumnType("ntext");
                entity.Property(e => e.CompanyName).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(50);
                entity.Property(e => e.Created).HasColumnType("datetime");
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.Fax)
                .HasMaxLength(30)
                .IsUnicode(false);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.HtmlMail).HasDefaultValue(true);
                entity.Property(e => e.InstantMessenger)
                .HasMaxLength(100)
                .IsUnicode(false);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
                entity.Property(e => e.Password).HasMaxLength(200);
                entity.Property(e => e.PinSecret).HasMaxLength(255);
                entity.Property(e => e.PrimaryPhone)
                .HasMaxLength(30)
                .IsUnicode(false);
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.SecondaryEmail).HasMaxLength(255);
                entity.Property(e => e.SecondaryPhone)
                .HasMaxLength(30)
                .IsUnicode(false);
                entity.Property(e => e.State).HasMaxLength(50);
                entity.Property(e => e.StatusId).HasColumnName("StatusID");
                entity.Property(e => e.SubscriberNumber).HasMaxLength(32);
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.Zip)
                .HasMaxLength(20)
                .IsUnicode(false);

                entity.HasOne(d => d.Owner).WithMany(p => p.InverseOwner)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_Users_Users");
            });

            modelBuilder.Entity<UserSetting>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.SettingsName, e.PropertyName });

                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.SettingsName).HasMaxLength(50);
                entity.Property(e => e.PropertyName).HasMaxLength(50);
                entity.Property(e => e.PropertyValue).HasColumnType("ntext");

                entity.HasOne(d => d.User).WithMany(p => p.UserSettings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserSettings_Users");
            });

            modelBuilder.Entity<UsersDetailed>(entity =>
            {
                entity
                .HasNoKey()
                .ToView("UsersDetailed");

                entity.Property(e => e.Changed).HasColumnType("datetime");
                entity.Property(e => e.Comments).HasColumnType("ntext");
                entity.Property(e => e.CompanyName).HasMaxLength(100);
                entity.Property(e => e.Created).HasColumnType("datetime");
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.FullName).HasMaxLength(101);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.OwnerEmail).HasMaxLength(255);
                entity.Property(e => e.OwnerFirstName).HasMaxLength(50);
                entity.Property(e => e.OwnerFullName).HasMaxLength(101);
                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");
                entity.Property(e => e.OwnerLastName).HasMaxLength(50);
                entity.Property(e => e.OwnerRoleId).HasColumnName("OwnerRoleID");
                entity.Property(e => e.OwnerUsername).HasMaxLength(50);
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.StatusId).HasColumnName("StatusID");
                entity.Property(e => e.SubscriberNumber).HasMaxLength(32);
                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<Version>(entity =>
            {
                entity.HasKey(e => e.DatabaseVersion);

                entity.Property(e => e.DatabaseVersion)
                .HasMaxLength(50)
                .IsUnicode(false);
                entity.Property(e => e.BuildDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VirtualGroup>(entity =>
            {
                entity.HasIndex(e => e.GroupId, "VirtualGroupsIdx_GroupID");

                entity.HasIndex(e => e.ServerId, "VirtualGroupsIdx_ServerID");

                entity.Property(e => e.VirtualGroupId).HasColumnName("VirtualGroupID");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.ServerId).HasColumnName("ServerID");

                entity.HasOne(d => d.Group).WithMany(p => p.VirtualGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VirtualGroups_ResourceGroups");

                entity.HasOne(d => d.Server).WithMany(p => p.VirtualGroups)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("FK_VirtualGroups_Servers");
            });

            modelBuilder.Entity<VirtualService>(entity =>
            {
                entity.HasIndex(e => e.ServerId, "VirtualServicesIdx_ServerID");

                entity.HasIndex(e => e.ServiceId, "VirtualServicesIdx_ServiceID");

                entity.Property(e => e.VirtualServiceId).HasColumnName("VirtualServiceID");
                entity.Property(e => e.ServerId).HasColumnName("ServerID");
                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.Server).WithMany(p => p.VirtualServices)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("FK_VirtualServices_Servers");

                entity.HasOne(d => d.Service).WithMany(p => p.VirtualServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VirtualServices_Services");
            });

            modelBuilder.Entity<WebDavAccessToken>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__WebDavAc__3214EC27B27DC571");

                entity.HasIndex(e => e.AccountId, "WebDavAccessTokensIdx_AccountID");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.AuthData).IsRequired();
                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
                entity.Property(e => e.FilePath).IsRequired();

                entity.HasOne(d => d.Account).WithMany(p => p.WebDavAccessTokens)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_WebDavAccessTokens_UserId");
            });

            modelBuilder.Entity<WebDavPortalUsersSetting>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__WebDavPo__3214EC278AF5195E");

                entity.HasIndex(e => e.AccountId, "WebDavPortalUsersSettingsIdx_AccountId");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.Account).WithMany(p => p.WebDavPortalUsersSettings)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_WebDavPortalUsersSettings_UserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
#endif
    }
}

namespace SolidCP.EnterpriseServer.Data {

    using Version = SolidCP.EnterpriseServer.Data.Entities.Version;
    using GlobalDnsRecord = SolidCP.EnterpriseServer.Data.Entities.GlobalDnsRecord;
    using BackgroundTaskParameter = SolidCP.EnterpriseServer.Data.Entities.BackgroundTaskParameter;

#if ScaffoldDbContextProperties
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
