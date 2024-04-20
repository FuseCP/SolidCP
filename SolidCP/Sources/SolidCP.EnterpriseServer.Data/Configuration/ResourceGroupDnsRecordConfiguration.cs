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
        Property(e => e.RecordOrder).HasDefaultValue(1);

        HasOne(d => d.Group).WithMany(p => p.ResourceGroupDnsRecords).HasConstraintName("FK_ResourceGroupDnsRecords_ResourceGroups");
    }
#endif
}
