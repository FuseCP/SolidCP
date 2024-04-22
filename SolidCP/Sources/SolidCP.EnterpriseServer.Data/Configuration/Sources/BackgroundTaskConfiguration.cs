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

using BackgroundTask = SolidCP.EnterpriseServer.Data.Entities.BackgroundTask;

public partial class BackgroundTaskConfiguration: Extensions.EntityTypeConfiguration<BackgroundTask>
{
    public BackgroundTaskConfiguration(): base() { }
    public BackgroundTaskConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Backgrou__3214EC27E22B574F");
    }
#endif
}
