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

public partial class ExchangeOrganizationSsFolderConfiguration : EntityTypeConfiguration<ExchangeOrganizationSsFolder>
{
	public override void Configure()
	{
		HasKey(e => e.Id).HasName("PK__Exchange__3214EC072DDBA072");

		Property(e => e.Type).IsUnicode(false);

		if (IsCore && IsSqlite) Property(e => e.Type).HasColumnType("TEXT COLLATE NOCASE");

#if NetCore
        HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationSsFolders)
            .HasConstraintName("FK_ExchangeOrganizationSsFolders_ItemId");
        HasOne(d => d.StorageSpaceFolder).WithMany(p => p.ExchangeOrganizationSsFolders)
            .HasConstraintName("FK_ExchangeOrganizationSsFolders_StorageSpaceFolderId");
#else
		HasRequired(d => d.Item).WithMany(p => p.ExchangeOrganizationSsFolders);
		HasRequired(d => d.StorageSpaceFolder).WithMany(p => p.ExchangeOrganizationSsFolders);
#endif
    }
}
