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

public partial class RdscollectionUserConfiguration: Extensions.EntityTypeConfiguration<RdscollectionUser>
{

    public RdscollectionUserConfiguration(): base() { }
    public RdscollectionUserConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__RDSColle__3214EC2780141EF7");

        HasOne(d => d.Account).WithMany(p => p.RdscollectionUsers).HasConstraintName("FK_RDSCollectionUsers_UserId");

        HasOne(d => d.Rdscollection).WithMany(p => p.RdscollectionUsers).HasConstraintName("FK_RDSCollectionUsers_RDSCollectionId");
    }
#endif
}
