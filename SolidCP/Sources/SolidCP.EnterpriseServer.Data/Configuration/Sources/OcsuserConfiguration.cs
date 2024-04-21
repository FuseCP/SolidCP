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

public partial class OcsuserConfiguration: Extensions.EntityTypeConfiguration<Ocsuser>
{
    public OcsuserConfiguration(): base() { }
    public OcsuserConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");
    }
#endif
}
