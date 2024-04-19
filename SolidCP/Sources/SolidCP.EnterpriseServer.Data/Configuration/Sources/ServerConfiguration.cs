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

public partial class ServerConfiguration: Extensions.EntityTypeConfiguration<Server>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ServerConfiguration(): base() { }
    public ServerConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasIndex(e => e.PrimaryGroupId, "ServersIdx_PrimaryGroupID");

        Property(e => e.ServerId).HasColumnName("ServerID");
        Property(e => e.AdParentDomain).HasMaxLength(200);
        Property(e => e.AdParentDomainController).HasMaxLength(200);
        Property(e => e.AdauthenticationType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ADAuthenticationType");
        Property(e => e.Adenabled)
                .HasDefaultValue(false)
                .HasColumnName("ADEnabled");
        Property(e => e.Adpassword)
                .HasMaxLength(100)
                .HasColumnName("ADPassword");
        Property(e => e.AdrootDomain)
                .HasMaxLength(200)
                .HasColumnName("ADRootDomain");
        Property(e => e.Adusername)
                .HasMaxLength(100)
                .HasColumnName("ADUsername");
        Property(e => e.Comments).HasColumnType("ntext");
        Property(e => e.InstantDomainAlias).HasMaxLength(200);
        Property(e => e.Osplatform).HasColumnName("OSPlatform");
        Property(e => e.Password).HasMaxLength(100);
        Property(e => e.PasswordIsSha256).HasColumnName("PasswordIsSHA256");
        Property(e => e.PrimaryGroupId).HasColumnName("PrimaryGroupID");
        Property(e => e.ServerName)
                .IsRequired()
                .HasMaxLength(100);
        Property(e => e.ServerUrl)
                .HasMaxLength(255)
                .HasDefaultValue("");

        HasOne(d => d.PrimaryGroup).WithMany(p => p.Servers)
                .HasForeignKey(d => d.PrimaryGroupId)
                .HasConstraintName("FK_Servers_ResourceGroups");
    }
#endif
}
