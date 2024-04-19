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

public partial class ResourceGroupDnsRecordConfiguration: Extensions.EntityTypeConfiguration<ResourceGroupDnsRecord>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ResourceGroupDnsRecordConfiguration(): base() { }
    public ResourceGroupDnsRecordConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.RecordId);

        HasIndex(e => e.GroupId, "ResourceGroupDnsRecordsIdx_GroupID");

        Property(e => e.RecordId).HasColumnName("RecordID");
        Property(e => e.GroupId).HasColumnName("GroupID");
        Property(e => e.Mxpriority).HasColumnName("MXPriority");
        Property(e => e.RecordData)
                .IsRequired()
                .HasMaxLength(200);
        Property(e => e.RecordName)
                .IsRequired()
                .HasMaxLength(50);
        Property(e => e.RecordOrder).HasDefaultValue(1);
        Property(e => e.RecordType)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

        HasOne(d => d.Group).WithMany(p => p.ResourceGroupDnsRecords)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_ResourceGroupDnsRecords_ResourceGroups");
    }
#endif
}
