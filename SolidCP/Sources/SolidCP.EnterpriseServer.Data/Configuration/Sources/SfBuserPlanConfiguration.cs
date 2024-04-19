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

public partial class SfBuserPlanConfiguration: Extensions.EntityTypeConfiguration<SfBuserPlan>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public SfBuserPlanConfiguration(): base() { }
    public SfBuserPlanConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        ToTable("SfBUserPlans");

        Property(e => e.SfBuserPlanId).HasColumnName("SfBUserPlanId");
        Property(e => e.ArchivePolicy).HasMaxLength(300);
        Property(e => e.Im).HasColumnName("IM");
        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.PublicImconnectivity).HasColumnName("PublicIMConnectivity");
        Property(e => e.ServerUri)
                .HasMaxLength(300)
                .HasColumnName("ServerURI");
        Property(e => e.SfBuserPlanName)
                .IsRequired()
                .HasMaxLength(300)
                .HasColumnName("SfBUserPlanName");
        Property(e => e.SfBuserPlanType).HasColumnName("SfBUserPlanType");
        Property(e => e.TelephonyDialPlanPolicy).HasMaxLength(300);
        Property(e => e.TelephonyVoicePolicy).HasMaxLength(300);
    }
#endif
}
