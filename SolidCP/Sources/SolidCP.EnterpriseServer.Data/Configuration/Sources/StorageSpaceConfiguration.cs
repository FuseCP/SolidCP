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

public partial class StorageSpaceConfiguration: Extensions.EntityTypeConfiguration<StorageSpace>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public StorageSpaceConfiguration(): base() { }
    public StorageSpaceConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__StorageS__3214EC07B8B9A6D1");

        HasIndex(e => e.ServerId, "StorageSpacesIdx_ServerId");

        HasIndex(e => e.ServiceId, "StorageSpacesIdx_ServiceId");

        Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false);
        Property(e => e.Path)
                .IsRequired()
                .IsUnicode(false);
        Property(e => e.UncPath).IsUnicode(false);

        HasOne(d => d.Server).WithMany(p => p.StorageSpaces)
                .HasForeignKey(d => d.ServerId)
                .HasConstraintName("FK_StorageSpaces_ServerId");

        HasOne(d => d.Service).WithMany(p => p.StorageSpaces)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_StorageSpaces_ServiceId");
    }
#endif
}
