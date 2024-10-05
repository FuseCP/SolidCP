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

public partial class RdsCollectionConfiguration : EntityTypeConfiguration<RdsCollection>
{
    public override void Configure() {

        if (IsCore && IsSqlite) Property(e => e.Name).HasColumnType("TEXT COLLATE NOCASE");

        HasKey(e => e.Id).HasName("PK__RDSColle__3214EC27346D361D");
    }
}
