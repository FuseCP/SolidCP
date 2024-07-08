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

public partial class ExchangeDeletedAccountConfiguration: EntityTypeConfiguration<ExchangeDeletedAccount>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Exchange__3214EC27D6F18B32");
    }
}
