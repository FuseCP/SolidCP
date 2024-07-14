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

public partial class PackageConfiguration: EntityTypeConfiguration<Package>
{
    public override void Configure() {

		if (IsSqlServer)
		{
			Property(e => e.StatusIdChangeDate).HasColumnType("datetime");
			Property(e => e.PurchaseDate).HasColumnType("datetime");
			Property(e => e.BandwidthUpdated).HasColumnType("datetime");
            Property(e => e.PackageComments).HasColumnType("ntext");
        } else if (IsCore && (IsMySql || IsMariaDb || IsSqlite || IsPostgreSql))
        {
            Property(e => e.PackageComments).HasColumnType("TEXT");
        }

#if NetCore
        if (IsSqlServer) {
            Core.ToTable(tb => tb.HasTrigger("Update_StatusIDchangeDate"));
            Property(e => e.StatusIdChangeDate).HasDefaultValueSql("(getdate())");
        }

        HasOne(d => d.ParentPackage).WithMany(p => p.ChildPackages).HasConstraintName("FK_Packages_Packages");

        HasOne(d => d.HostingPlan).WithMany(p => p.Packages).HasConstraintName("FK_Packages_HostingPlans");

        HasOne(d => d.Server).WithMany(p => p.Packages).HasConstraintName("FK_Packages_Servers");

        HasOne(d => d.User).WithMany(p => p.Packages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Packages_Users");

        HasMany(d => d.Services).WithMany(p => p.Packages)
            .UsingEntity<PackageService>();
            /*  r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .HasConstraintName("FK_PackageServices_Services"),
                l => l.HasOne<Package>().WithMany()
                        .HasForeignKey("PackageId")
                        .HasConstraintName("FK_PackageServices_Packages"),
                j => {
                    j.HasKey("PackageId", "ServiceId");
                    j.ToTable("PackageServices");
                    j.IndexerProperty<int>("PackageId").HasColumnName("PackageID");
                    j.IndexerProperty<int>("ServiceId").HasColumnName("ServiceID");
                }); */
#else
		HasOptional(p => p.ParentPackage).WithMany(p => p.ChildPackages);
        HasOptional(p => p.HostingPlan).WithMany(p => p.Packages);
        HasOptional(p => p.Server).WithMany(p => p.Packages);
        HasRequired(p => p.User).WithMany(p => p.Packages);

        // TODO EF is this correct?
        HasMany(p => p.Services).WithMany(p => p.Packages)
            .Map(c => c.ToTable("PackageService")
                .MapRightKey("ServiceID")
                .MapLeftKey("PackageID"));
#endif

        #region Seed Data
		HasData(() => new Package[] {
			new Package() { PackageId = 1, PackageComments = "", PackageName = "System", StatusId = 1, StatusIdChangeDate = DateTime.Parse("2024-04-20T11:02:58.560000Z").ToUniversalTime(), UserId = 1 }
		});
        #endregion
    }
}
