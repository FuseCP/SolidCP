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

public partial class RdsserverConfiguration: EntityTypeConfiguration<Rdsserver>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__RDSServe__3214EC277AE01BBB");

        Property(e => e.ConnectionEnabled).HasDefaultValue(true);

        HasOne(d => d.Rdscollection).WithMany(p => p.Rdsservers).HasConstraintName("FK_RDSServers_RDSCollectionId");
    }
}
