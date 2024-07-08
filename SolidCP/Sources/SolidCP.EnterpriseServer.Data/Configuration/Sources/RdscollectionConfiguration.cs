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

public partial class RdscollectionConfiguration: EntityTypeConfiguration<Rdscollection>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__RDSColle__3214EC27F7A7E04B");
    }
}
