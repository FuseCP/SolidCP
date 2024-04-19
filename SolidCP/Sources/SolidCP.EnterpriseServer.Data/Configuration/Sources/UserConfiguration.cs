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

public partial class UserConfiguration: Extensions.EntityTypeConfiguration<User>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public UserConfiguration(): base() { }
    public UserConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasIndex(e => e.Username, "IX_Users_Username").IsUnique();

        HasIndex(e => e.OwnerId, "UsersIdx_OwnerID");

        Property(e => e.UserId).HasColumnName("UserID");
        Property(e => e.Address).HasMaxLength(200);
        Property(e => e.Changed).HasColumnType("datetime");
        Property(e => e.City).HasMaxLength(50);
        Property(e => e.Comments).HasColumnType("ntext");
        Property(e => e.CompanyName).HasMaxLength(100);
        Property(e => e.Country).HasMaxLength(50);
        Property(e => e.Created).HasColumnType("datetime");
        Property(e => e.Email).HasMaxLength(255);
        Property(e => e.Fax)
                .HasMaxLength(30)
                .IsUnicode(false);
        Property(e => e.FirstName).HasMaxLength(50);
        Property(e => e.HtmlMail).HasDefaultValue(true);
        Property(e => e.InstantMessenger)
                .HasMaxLength(100)
                .IsUnicode(false);
        Property(e => e.LastName).HasMaxLength(50);
        Property(e => e.OwnerId).HasColumnName("OwnerID");
        Property(e => e.Password).HasMaxLength(200);
        Property(e => e.PinSecret).HasMaxLength(255);
        Property(e => e.PrimaryPhone)
                .HasMaxLength(30)
                .IsUnicode(false);
        Property(e => e.RoleId).HasColumnName("RoleID");
        Property(e => e.SecondaryEmail).HasMaxLength(255);
        Property(e => e.SecondaryPhone)
                .HasMaxLength(30)
                .IsUnicode(false);
        Property(e => e.State).HasMaxLength(50);
        Property(e => e.StatusId).HasColumnName("StatusID");
        Property(e => e.SubscriberNumber).HasMaxLength(32);
        Property(e => e.Username).HasMaxLength(50);
        Property(e => e.Zip)
                .HasMaxLength(20)
                .IsUnicode(false);

        HasOne(d => d.Owner).WithMany(p => p.InverseOwner)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_Users_Users");
    }
#endif
}
