using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class PackageServiceConfiguration: EntityTypeConfiguration<PackageService>
{
    public override void Configure() {
#if NetCore
		HasOne<Service>().WithMany()
			.HasForeignKey("ServiceId")
			.HasConstraintName("FK_PackageServices_Services");
		HasOne<Package>().WithMany()
			.HasForeignKey("PackageId")
			.HasConstraintName("FK_PackageServices_Packages");
#endif
	}
}
