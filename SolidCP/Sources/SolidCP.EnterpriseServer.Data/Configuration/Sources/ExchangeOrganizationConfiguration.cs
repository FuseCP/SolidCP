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

public partial class ExchangeOrganizationConfiguration: EntityTypeConfiguration<ExchangeOrganization>
{
    public override void Configure() {
        Property(e => e.ItemId).ValueGeneratedNever();

        HasOne(d => d.Item).WithOne(p => p.ExchangeOrganization).HasConstraintName("FK_ExchangeOrganizations_ServiceItems");
    }
}
