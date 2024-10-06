using SolidCP.EnterpriseServer.Data.Entities;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class ExchangeOrganizationConfiguration: EntityTypeConfiguration<ExchangeOrganization>
{
    public override void Configure() {

        if (IsCore && IsSqlite) Property(e => e.OrganizationId).HasColumnType("TEXT COLLATE NOCASE");

#if NetCore
        Property(e => e.ItemId).ValueGeneratedNever();

        HasOne(d => d.Item).WithOne(p => p.ExchangeOrganization).HasConstraintName("FK_ExchangeOrganizations_ServiceItems");
#else
        HasRequired(d => d.Item); //.WithOptional(p => p.ExchangeOrganization);
#endif    
    }
}
