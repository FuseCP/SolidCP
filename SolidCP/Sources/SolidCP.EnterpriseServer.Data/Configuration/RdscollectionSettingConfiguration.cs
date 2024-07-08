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

public partial class RdsCollectionSettingConfiguration: EntityTypeConfiguration<RdsCollectionSetting>
{
    public override void Configure() {

#if NetCore
        HasOne(d => d.RdsCollection).WithMany(p => p.RdsCollectionSettings).HasConstraintName("FK_RDSCollectionSettings_RDSCollections");
#else
        HasRequired(d => d.RdsCollection).WithMany(p => p.RdsCollectionSettings);
#endif
    }
}
