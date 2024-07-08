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

public partial class SupportServiceLevelConfiguration: EntityTypeConfiguration<SupportServiceLevel>
{
    public override void Configure() {
        HasKey(e => e.LevelId).HasName("PK__SupportS__09F03C065BA08AFB");
    }
}
