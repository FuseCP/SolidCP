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

public partial class ExchangeAccountEmailAddressConfiguration: Extensions.EntityTypeConfiguration<ExchangeAccountEmailAddress>
{
    public DbFlavor Flavor { get; set; } = DbFlavor.Unknown;

    public ExchangeAccountEmailAddressConfiguration(): base() { }
    public ExchangeAccountEmailAddressConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.AddressId);

        HasIndex(e => e.AccountId, "ExchangeAccountEmailAddressesIdx_AccountID");

        HasIndex(e => e.EmailAddress, "IX_ExchangeAccountEmailAddresses_UniqueEmail").IsUnique();

        Property(e => e.AddressId).HasColumnName("AddressID");
        Property(e => e.AccountId).HasColumnName("AccountID");
        Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(300);

        HasOne(d => d.Account).WithMany(p => p.ExchangeAccountEmailAddresses)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_ExchangeAccountEmailAddresses_ExchangeAccounts");
    }
#endif
}
