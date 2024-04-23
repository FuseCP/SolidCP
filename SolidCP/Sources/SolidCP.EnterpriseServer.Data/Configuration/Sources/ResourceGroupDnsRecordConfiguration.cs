// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using SolidCP.EnterpriseServer.Data.Extensions;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class ResourceGroupDnsRecordConfiguration: EntityTypeConfiguration<ResourceGroupDnsRecord>
{

#if NetCore || NetFX
    public override void Configure() {
        Property(e => e.RecordOrder).HasDefaultValue(1);

        HasOne(d => d.Group).WithMany(p => p.ResourceGroupDnsRecords).HasConstraintName("FK_ResourceGroupDnsRecords_ResourceGroups");
    }
#endif
}
