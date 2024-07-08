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

public partial class ExchangeAccountEmailAddressConfiguration: EntityTypeConfiguration<ExchangeAccountEmailAddress>
{
    public override void Configure() {
        HasOne(d => d.Account).WithMany(p => p.ExchangeAccountEmailAddresses).HasConstraintName("FK_ExchangeAccountEmailAddresses_ExchangeAccounts");
    }
}
