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

using Version = SolidCP.EnterpriseServer.Data.Entities.Version;

public partial class VersionConfiguration: EntityTypeConfiguration<Version>
{
    public override void Configure() {

        Property(e => e.DatabaseVersion).IsUnicode(false);
        if (IsSqlServer) Property(e => e.BuildDate).HasColumnType("datetime");

        #region Seed Data
        HasData(() => new Version[] {
            new Version() { DatabaseVersion = "1.0", BuildDate = DateTime.Parse("2010-04-10T00:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.0.1.0", BuildDate = DateTime.Parse("2010-07-16T12:53:03.5630000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.0.2.0", BuildDate = DateTime.Parse("2010-09-03T00:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.1.0.9", BuildDate = DateTime.Parse("2010-11-16T00:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.1.2.13", BuildDate = DateTime.Parse("2011-04-15T00:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.2.0.38", BuildDate = DateTime.Parse("2011-07-13T00:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.2.1.6", BuildDate = DateTime.Parse("2012-03-29T00:00:00.0000000Z").ToUniversalTime() },
            new Version() { DatabaseVersion = "1.4.9", BuildDate = DateTime.Parse("2024-04-20T00:00:00.0000000Z").ToUniversalTime() },
        });
        #endregion
    }
}
