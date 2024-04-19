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

public partial class UsersDetailedConfiguration: Extensions.EntityTypeConfiguration<UsersDetailed>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public UsersDetailedConfiguration(): base() { }
    public UsersDetailedConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasNoKey().ToView("UsersDetailed");

        Property(e => e.Changed).HasColumnType("datetime");
        Property(e => e.Comments).HasColumnType("ntext");
        Property(e => e.CompanyName).HasMaxLength(100);
        Property(e => e.Created).HasColumnType("datetime");
        Property(e => e.Email).HasMaxLength(255);
        Property(e => e.FirstName).HasMaxLength(50);
        Property(e => e.FullName).HasMaxLength(101);
        Property(e => e.LastName).HasMaxLength(50);
        Property(e => e.OwnerEmail).HasMaxLength(255);
        Property(e => e.OwnerFirstName).HasMaxLength(50);
        Property(e => e.OwnerFullName).HasMaxLength(101);
        Property(e => e.OwnerId).HasColumnName("OwnerID");
        Property(e => e.OwnerLastName).HasMaxLength(50);
        Property(e => e.OwnerRoleId).HasColumnName("OwnerRoleID");
        Property(e => e.OwnerUsername).HasMaxLength(50);
        Property(e => e.RoleId).HasColumnName("RoleID");
        Property(e => e.StatusId).HasColumnName("StatusID");
        Property(e => e.SubscriberNumber).HasMaxLength(32);
        Property(e => e.UserId).HasColumnName("UserID");
        Property(e => e.Username).HasMaxLength(50);
    }
#endif
}
