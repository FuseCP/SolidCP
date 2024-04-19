// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
#if NetCore
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#endif
#if NetFX
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Spatial;
using System.Data.Entity.Validation;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class CommentConfiguration: Extensions.EntityTypeConfiguration<Comment>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public CommentConfiguration(): base() { }
    public CommentConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasIndex(e => e.UserId, "CommentsIdx_UserID");

        Property(e => e.CommentId).HasColumnName("CommentID");
        Property(e => e.CommentText).HasMaxLength(1000);
        Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        Property(e => e.ItemId).HasColumnName("ItemID");
        Property(e => e.ItemTypeId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ItemTypeID");
        Property(e => e.SeverityId).HasColumnName("SeverityID");
        Property(e => e.UserId).HasColumnName("UserID");

        HasOne(d => d.User).WithMany(p => p.CommentsNavigation)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Comments_Users");
    }
#endif
}
