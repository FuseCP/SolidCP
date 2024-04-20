// This file is auto generated, do not edit.
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

public partial class VirtualServiceConfiguration: Extensions.EntityTypeConfiguration<VirtualService>
{

    public VirtualServiceConfiguration(): base() { }
    public VirtualServiceConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasOne(d => d.Server).WithMany(p => p.VirtualServices).HasConstraintName("FK_VirtualServices_Servers");

        HasOne(d => d.Service).WithMany(p => p.VirtualServices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VirtualServices_Services");
    }
#endif
}
