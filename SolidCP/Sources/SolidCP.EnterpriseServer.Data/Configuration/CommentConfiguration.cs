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

public partial class CommentConfiguration: EntityTypeConfiguration<Comment>
{
    public override void Configure() {

        Property(e => e.ItemTypeId).IsUnicode(false);
        if (IsSqlServer) Property(e => e.CreatedDate).HasColumnType("datetime");

#if NetCore
		if (IsSqlServer) Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

        HasOne(d => d.User).WithMany(p => p.CommentsNavigation).HasConstraintName("FK_Comments_Users");
#else
		HasRequired(d => d.User).WithMany(p => p.CommentsNavigation);
#endif
    }
}
