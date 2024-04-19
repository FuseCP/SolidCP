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

public partial class RdscertificateConfiguration: Extensions.EntityTypeConfiguration<Rdscertificate>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public RdscertificateConfiguration(): base() { }
    public RdscertificateConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        ToTable("RDSCertificates");

        Property(e => e.Id).HasColumnName("ID");
        Property(e => e.Content)
                .IsRequired()
                .HasColumnType("ntext");
        Property(e => e.ExpiryDate).HasColumnType("datetime");
        Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(255);
        Property(e => e.Hash)
                .IsRequired()
                .HasMaxLength(255);
        Property(e => e.ValidFrom).HasColumnType("datetime");
    }
#endif
}
