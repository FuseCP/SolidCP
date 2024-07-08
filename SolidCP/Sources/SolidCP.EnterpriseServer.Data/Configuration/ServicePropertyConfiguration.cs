using SolidCP.EnterpriseServer.Data.Entities;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class ServicePropertyConfiguration: EntityTypeConfiguration<ServiceProperty>
{
    public override void Configure() {
        HasKey(e => new { e.ServiceId, e.PropertyName }).HasName("PK_ServiceProperties_1");

#if NetCore
        HasOne(d => d.Service).WithMany(p => p.ServiceProperties).HasConstraintName("FK_ServiceProperties_Services");
#else
        HasRequired(d => d.Service).WithMany(p => p.ServiceProperties);
#endif
    }
}
