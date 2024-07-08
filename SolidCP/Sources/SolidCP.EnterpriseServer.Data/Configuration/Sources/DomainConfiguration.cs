// This file is auto generated, do not edit.
using System;
using System.Collections.Generic;
using SolidCP.EnterpriseServer.Data.Configuration;
using SolidCP.EnterpriseServer.Data.Entities;
using SolidCP.EnterpriseServer.Data.Extensions;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif
#if NetFX
using System.Data.Entity;
#endif

namespace SolidCP.EnterpriseServer.Data.Configuration;

public partial class DomainConfiguration: EntityTypeConfiguration<Domain>
{
    public override void Configure() {
        HasOne(d => d.MailDomain).WithMany(p => p.DomainMailDomains).HasConstraintName("FK_Domains_ServiceItems_MailDomain");

        HasOne(d => d.Package).WithMany(p => p.Domains).HasConstraintName("FK_Domains_Packages");

        HasOne(d => d.WebSite).WithMany(p => p.DomainWebSites).HasConstraintName("FK_Domains_ServiceItems_WebSite");

        HasOne(d => d.ZoneItem).WithMany(p => p.DomainZoneItems).HasConstraintName("FK_Domains_ServiceItems_ZoneItem");
    }
}
