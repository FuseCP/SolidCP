// This file is auto generated, do not edit.
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

public partial class ExchangeOrganizationSsFolderConfiguration: Extensions.EntityTypeConfiguration<ExchangeOrganizationSsFolder>
{

    public ExchangeOrganizationSsFolderConfiguration(): base() { }
    public ExchangeOrganizationSsFolderConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__Exchange__3214EC072DDBA072");

        HasOne(d => d.Item).WithMany(p => p.ExchangeOrganizationSsFolders).HasConstraintName("FK_ExchangeOrganizationSsFolders_ItemId");

        HasOne(d => d.StorageSpaceFolder).WithMany(p => p.ExchangeOrganizationSsFolders).HasConstraintName("FK_ExchangeOrganizationSsFolders_StorageSpaceFolderId");
    }
#endif
}
