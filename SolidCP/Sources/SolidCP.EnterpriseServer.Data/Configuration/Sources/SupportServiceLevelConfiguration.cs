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

public partial class SupportServiceLevelConfiguration: EntityTypeConfiguration<SupportServiceLevel>
{
    public override void Configure() {
        HasKey(e => e.LevelId).HasName("PK__SupportS__09F03C061D1B6E9C");
    }
}
