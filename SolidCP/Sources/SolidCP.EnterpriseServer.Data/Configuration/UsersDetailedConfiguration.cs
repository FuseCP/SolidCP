using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class UsersDetailedConfiguration: EntityTypeConfiguration<UsersDetailed>
{
#if NetCore || NetFX
    public override void Configure() {

        if (IsMsSql)
        {
#if NetCore
            Core.ToView("UsersDetailed");
#else
            // TODO is this correct ? (The db will be cretaed with EF Core 8, not with EF6, so the view will be in
            // the database)
            ToTable("UsersDetailed");
#endif
        }
    }
#endif
}
