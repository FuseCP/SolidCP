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

public partial class QuotaConfiguration: Extensions.EntityTypeConfiguration<Quota>
{
    public QuotaConfiguration(): base() { }
    public QuotaConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {

#if NetCore
        Property(e => e.QuotaId).ValueGeneratedNever();
        Property(e => e.QuotaOrder).HasDefaultValue(1);
        Property(e => e.QuotaTypeId).HasDefaultValue(2);
        Property(e => e.ServiceQuota).HasDefaultValue(false);

        HasOne(d => d.Group).WithMany(p => p.Quota).HasConstraintName("FK_Quotas_ResourceGroups");

        HasOne(d => d.ItemType).WithMany(p => p.Quota).HasConstraintName("FK_Quotas_ServiceItemTypes");
#else
        HasRequired(d => d.Group).WithMany(p => p.Quota);
        HasRequired(d => d.ItemType).WithMany(p => p.Quota);
#endif
    }
#endif
    }
