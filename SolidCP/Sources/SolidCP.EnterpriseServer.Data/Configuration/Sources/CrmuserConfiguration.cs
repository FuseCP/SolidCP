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

public partial class CrmuserConfiguration: Extensions.EntityTypeConfiguration<Crmuser>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public CrmuserConfiguration(): base() { }
    public CrmuserConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        ToTable("CRMUsers");

        HasIndex(e => e.AccountId, "CRMUsersIdx_AccountID");

        Property(e => e.CrmuserId).HasColumnName("CRMUserID");
        Property(e => e.AccountId).HasColumnName("AccountID");
        Property(e => e.BusinessUnitId).HasColumnName("BusinessUnitID");
        Property(e => e.Caltype).HasColumnName("CALType");
        Property(e => e.ChangedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        Property(e => e.CrmuserGuid).HasColumnName("CRMUserGuid");

        HasOne(d => d.Account).WithMany(p => p.Crmusers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMUsers_ExchangeAccounts");
    }
#endif
}
