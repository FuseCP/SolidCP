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

public partial class DomainConfiguration: EntityTypeConfiguration<Domain>
{
    public DomainConfiguration(): base() { }
    public DomainConfiguration(DbType dbType, bool initSeedData = false) : base(dbType, initSeedData) { }

    public override void Configure() {

		if (IsSqlServer)
		{
			Property(e => e.CreationDate).HasColumnType("datetime");
			Property(e => e.ExpirationDate).HasColumnType("datetime");
			Property(e => e.LastUpdateDate).HasColumnType("datetime");
		}

#if NetCore
        HasOne(d => d.MailDomain).WithMany(p => p.DomainMailDomains).HasConstraintName("FK_Domains_ServiceItems_MailDomain");

        HasOne(d => d.Package).WithMany(p => p.Domains).HasConstraintName("FK_Domains_Packages");

        HasOne(d => d.WebSite).WithMany(p => p.DomainWebSites).HasConstraintName("FK_Domains_ServiceItems_WebSite");

        HasOne(d => d.Zone).WithMany(p => p.DomainZones).HasConstraintName("FK_Domains_ServiceItems_ZoneItem");
#else
		HasOptional(d => d.MailDomain).WithMany(p => p.DomainMailDomains);
        HasRequired(d => d.Package).WithMany(p => p.Domains);
        HasOptional(d => d.WebSite).WithMany(p => p.DomainWebSites);
        HasOptional(d => d.Zone).WithMany(p => p.DomainZones);
#endif
    }
}
