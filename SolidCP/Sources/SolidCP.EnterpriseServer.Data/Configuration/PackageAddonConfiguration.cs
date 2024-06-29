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

public partial class PackageAddonConfiguration: EntityTypeConfiguration<PackageAddon>
{
#if NetCore || NetFX
    public override void Configure() {

		if (IsMsSql)
		{
			Property(e => e.Comments).HasColumnType("ntext");
			Property(e => e.PurchaseDate).HasColumnType("datetime");
		}
		else if (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql)
		{
#if NetCore
			Property(e => e.Comments).HasColumnType("TEXT");
#endif
		}

#if NetCore
        HasOne(d => d.Package).WithMany(p => p.PackageAddons)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PackageAddons_Packages");

        HasOne(d => d.Plan).WithMany(p => p.PackageAddons).HasConstraintName("FK_PackageAddons_HostingPlans");
#else
		HasRequired(d => d.Package).WithMany(p => p.PackageAddons).WillCascadeOnDelete();
        HasRequired(d => d.Plan).WithMany(p => p.PackageAddons);
#endif
    }
#endif
		}
