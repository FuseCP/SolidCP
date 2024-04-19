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

public partial class SslcertificateConfiguration: Extensions.EntityTypeConfiguration<Sslcertificate>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public SslcertificateConfiguration(): base() { }
    public SslcertificateConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasNoKey().ToTable("SSLCertificates");

        Property(e => e.Certificate).HasColumnType("ntext");
        Property(e => e.Csr)
                .HasColumnType("ntext")
                .HasColumnName("CSR");
        Property(e => e.Csrlength).HasColumnName("CSRLength");
        Property(e => e.DistinguishedName).HasMaxLength(500);
        Property(e => e.ExpiryDate).HasColumnType("datetime");
        Property(e => e.FriendlyName).HasMaxLength(255);
        Property(e => e.Hash).HasColumnType("ntext");
        Property(e => e.Hostname).HasMaxLength(255);
        Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
        Property(e => e.Pfx).HasColumnType("ntext");
        Property(e => e.SerialNumber).HasMaxLength(250);
        Property(e => e.SiteId).HasColumnName("SiteID");
        Property(e => e.UserId).HasColumnName("UserID");
        Property(e => e.ValidFrom).HasColumnType("datetime");
    }
#endif
}
