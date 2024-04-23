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

public partial class IpaddressConfiguration: EntityTypeConfiguration<Ipaddress>
{
#if NetCore || NetFX
    public override void Configure() {

#if NetCore
        HasOne(d => d.Server).WithMany(p => p.Ipaddresses)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_IPAddresses_Servers");
#else
        // TODO optional or required and cascade delete?
        HasOptional(d => d.Server).WithMany(p => p.Ipaddresses).WillCascadeOnDelete();
#endif
    }
#endif
}
