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

public partial class OcsuserConfiguration: Extensions.EntityTypeConfiguration<Ocsuser>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public OcsuserConfiguration(): base() { }
    public OcsuserConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        ToTable("OCSUsers");

        Property(e => e.OcsuserId).HasColumnName("OCSUserID");
        Property(e => e.AccountId).HasColumnName("AccountID");
        Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        Property(e => e.InstanceId)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("InstanceID");
        Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
    }
#endif
}
