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

public partial class PackageQuotaConfiguration: EntityTypeConfiguration<PackageQuota>
{
    public override void Configure() {
        HasOne(d => d.Package).WithMany(p => p.PackageQuota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageQuotas_Packages");

        HasOne(d => d.Quota).WithMany(p => p.PackageQuota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackageQuotas_Quotas");
    }
}
