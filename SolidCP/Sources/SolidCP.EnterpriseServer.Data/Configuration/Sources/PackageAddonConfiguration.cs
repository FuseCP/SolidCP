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

public partial class PackageAddonConfiguration: EntityTypeConfiguration<PackageAddon>
{
    public override void Configure() {
        HasOne(d => d.Package).WithMany(p => p.PackageAddons)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PackageAddons_Packages");

        HasOne(d => d.Plan).WithMany(p => p.PackageAddons).HasConstraintName("FK_PackageAddons_HostingPlans");
    }
}
