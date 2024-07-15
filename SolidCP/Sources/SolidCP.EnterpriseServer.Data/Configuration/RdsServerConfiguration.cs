using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class RdsServerConfiguration: EntityTypeConfiguration<RdsServer>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__RDSServe__3214EC27DBEBD4B5");

#if NetCore
        Property(e => e.ConnectionEnabled).HasDefaultValue(true);

        HasOne(d => d.RdsCollection).WithMany(p => p.RdsServers).HasConstraintName("FK_RDSServers_RDSCollectionId");
#else
        HasOptional(d => d.RdsCollection).WithMany(p => p.RdsServers);
#endif
    }
}
