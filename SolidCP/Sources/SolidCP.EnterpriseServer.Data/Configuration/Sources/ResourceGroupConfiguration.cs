// This file is auto generated, do not edit.
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

public partial class ResourceGroupConfiguration: Extensions.EntityTypeConfiguration<ResourceGroup>
{

    public ResourceGroupConfiguration(): base() { }
    public ResourceGroupConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        Property(e => e.GroupId).ValueGeneratedNever();
        Property(e => e.GroupOrder).HasDefaultValue(1);
    }
#endif
}
