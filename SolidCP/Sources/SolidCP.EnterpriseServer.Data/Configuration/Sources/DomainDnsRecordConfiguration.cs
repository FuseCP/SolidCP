﻿// This file is auto generated, do not edit.
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

public partial class DomainDnsRecordConfiguration: EntityTypeConfiguration<DomainDnsRecord>
{

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__DomainDn__3214EC2701612B5D");

        HasOne(d => d.Domain).WithMany(p => p.DomainDnsRecords).HasConstraintName("FK_DomainDnsRecords_DomainId");
    }
#endif
}