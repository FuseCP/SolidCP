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

public partial class RdsCollectionUserConfiguration: EntityTypeConfiguration<RdsCollectionUser>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__RDSColle__3214EC2780141EF7");

#if NetCore
        HasOne(d => d.Account).WithMany(p => p.RdsCollectionUsers).HasConstraintName("FK_RDSCollectionUsers_UserId");
        HasOne(d => d.RdsCollection).WithMany(p => p.RdsCollectionUsers).HasConstraintName("FK_RDSCollectionUsers_RDSCollectionId");
#else
        HasRequired(d => d.Account).WithMany(p => p.RdsCollectionUsers);
        HasRequired(d => d.RdsCollection).WithMany(p => p.RdsCollectionUsers);
#endif
    }
}
