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

public partial class RdscollectionSettingConfiguration: EntityTypeConfiguration<RdscollectionSetting>
{
    public override void Configure() {
        HasOne(d => d.Rdscollection).WithMany(p => p.RdscollectionSettings).HasConstraintName("FK_RDSCollectionSettings_RDSCollections");
    }
}
