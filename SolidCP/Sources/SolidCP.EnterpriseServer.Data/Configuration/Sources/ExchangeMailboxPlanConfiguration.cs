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

public partial class ExchangeMailboxPlanConfiguration: Extensions.EntityTypeConfiguration<ExchangeMailboxPlan>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ExchangeMailboxPlanConfiguration(): base() { }
    public ExchangeMailboxPlanConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.MailboxPlanId);

        HasIndex(e => e.ItemId, "ExchangeMailboxPlansIdx_ItemID");

        HasIndex(e => e.MailboxPlanId, "IX_ExchangeMailboxPlans").IsUnique();

        Property(e => e.ArchiveSizeMb).HasColumnName("ArchiveSizeMB");
        Property(e => e.EnableImap).HasColumnName("EnableIMAP");
        Property(e => e.EnableMapi).HasColumnName("EnableMAPI");
        Property(e => e.EnableOwa).HasColumnName("EnableOWA");
        Property(e => e.EnablePop).HasColumnName("EnablePOP");
        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.LitigationHoldMsg).HasMaxLength(512);
        Property(e => e.LitigationHoldUrl).HasMaxLength(256);
        Property(e => e.MailboxPlan)
                .IsRequired()
                .HasMaxLength(300);
        Property(e => e.MailboxSizeMb).HasColumnName("MailboxSizeMB");
        Property(e => e.MaxReceiveMessageSizeKb).HasColumnName("MaxReceiveMessageSizeKB");
        Property(e => e.MaxSendMessageSizeKb).HasColumnName("MaxSendMessageSizeKB");

        HasOne(d => d.Item).WithMany(p => p.ExchangeMailboxPlans)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_ExchangeMailboxPlans_ExchangeOrganizations");
    }
#endif
}
