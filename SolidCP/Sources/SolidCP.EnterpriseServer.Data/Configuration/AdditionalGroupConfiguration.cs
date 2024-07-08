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

public partial class AdditionalGroupConfiguration: EntityTypeConfiguration<AdditionalGroup>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Addition__3214EC272F1861EB");
    }
}
