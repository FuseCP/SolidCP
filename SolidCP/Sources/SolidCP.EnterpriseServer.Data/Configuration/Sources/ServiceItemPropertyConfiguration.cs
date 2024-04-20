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

public partial class ServiceItemPropertyConfiguration: Extensions.EntityTypeConfiguration<ServiceItemProperty>
{

    public ServiceItemPropertyConfiguration(): base() { }
    public ServiceItemPropertyConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasOne(d => d.Item).WithMany(p => p.ServiceItemProperties).HasConstraintName("FK_ServiceItemProperties_ServiceItems");
    }
#endif
}
