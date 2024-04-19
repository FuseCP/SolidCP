// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
#if NetCore
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#endif
#if NetFX
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class ExchangeAccountConfiguration: Extensions.EntityTypeConfiguration<ExchangeAccount>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ExchangeAccountConfiguration(): base() { }
    public ExchangeAccountConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.AccountId);

        HasIndex(e => e.ItemId, "ExchangeAccountsIdx_ItemID");

        HasIndex(e => e.MailboxPlanId, "ExchangeAccountsIdx_MailboxPlanId");

        HasIndex(e => e.AccountName, "IX_ExchangeAccounts_UniqueAccountName").IsUnique();

        Property(e => e.AccountId).HasColumnName("AccountID");
        Property(e => e.AccountName)
                .IsRequired()
                .HasMaxLength(300);
        Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(300);
        Property(e => e.IsVip).HasColumnName("IsVIP");
        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.LevelId).HasColumnName("LevelID");
        Property(e => e.MailboxManagerActions)
                .HasMaxLength(200)
                .IsUnicode(false);
        Property(e => e.PrimaryEmailAddress).HasMaxLength(300);
        Property(e => e.SamAccountName).HasMaxLength(100);
        Property(e => e.SubscriberNumber).HasMaxLength(32);
        Property(e => e.UserPrincipalName).HasMaxLength(300);

        HasOne(d => d.Item).WithMany(p => p.ExchangeAccounts)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ExchangeAccounts_ServiceItems");

        HasOne(d => d.MailboxPlan).WithMany(p => p.ExchangeAccounts)
                .HasForeignKey(d => d.MailboxPlanId)
                .HasConstraintName("FK_ExchangeAccounts_ExchangeMailboxPlans");
    }
#endif
}
