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

public partial class DomainConfiguration: Extensions.EntityTypeConfiguration<Domain>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public DomainConfiguration(): base() { }
    public DomainConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasIndex(e => e.MailDomainId, "DomainsIdx_MailDomainID");

        HasIndex(e => e.PackageId, "DomainsIdx_PackageID");

        HasIndex(e => e.WebSiteId, "DomainsIdx_WebSiteID");

        HasIndex(e => e.ZoneItemId, "DomainsIdx_ZoneItemID");

        Property(e => e.DomainId).HasColumnName("DomainID");
        Property(e => e.CreationDate).HasColumnType("datetime");
        Property(e => e.DomainName)
                .IsRequired()
                .HasMaxLength(100);
        Property(e => e.ExpirationDate).HasColumnType("datetime");
        Property(e => e.LastUpdateDate).HasColumnType("datetime");
        Property(e => e.MailDomainId).HasColumnName("MailDomainID");
        Property(e => e.PackageId).HasColumnName("PackageID");
        Property(e => e.WebSiteId).HasColumnName("WebSiteID");
        Property(e => e.ZoneItemId).HasColumnName("ZoneItemID");

        HasOne(d => d.MailDomain).WithMany(p => p.DomainMailDomains)
                .HasForeignKey(d => d.MailDomainId)
                .HasConstraintName("FK_Domains_ServiceItems_MailDomain");

        HasOne(d => d.Package).WithMany(p => p.Domains)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("FK_Domains_Packages");

        HasOne(d => d.WebSite).WithMany(p => p.DomainWebSites)
                .HasForeignKey(d => d.WebSiteId)
                .HasConstraintName("FK_Domains_ServiceItems_WebSite");

        HasOne(d => d.ZoneItem).WithMany(p => p.DomainZoneItems)
                .HasForeignKey(d => d.ZoneItemId)
                .HasConstraintName("FK_Domains_ServiceItems_ZoneItem");
    }
#endif
}
