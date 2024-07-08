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

public partial class ExchangeAccountEmailAddressConfiguration : EntityTypeConfiguration<ExchangeAccountEmailAddress>
{
	public override void Configure()
	{

#if NetCore
        HasOne(d => d.Account).WithMany(p => p.ExchangeAccountEmailAddresses).HasConstraintName("FK_ExchangeAccountEmailAddresses_ExchangeAccounts");
#else
		HasRequired(d => d.Account).WithMany(p => p.ExchangeAccountEmailAddresses);
#endif
    }
}
