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

public partial class ExchangeOrganizationSsFolderConfiguration: EntityTypeConfiguration<ExchangeOrganizationSsFolder>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Exchange__3214EC079B0946A0");

        HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationSsFolders).HasConstraintName("FK_ExchangeOrganizationSsFolders_ItemId");

        HasOne(d => d.StorageSpaceFolder).WithMany(p => p.ExchangeOrganizationSsFolders).HasConstraintName("FK_ExchangeOrganizationSsFolders_StorageSpaceFolderId");
    }
}
