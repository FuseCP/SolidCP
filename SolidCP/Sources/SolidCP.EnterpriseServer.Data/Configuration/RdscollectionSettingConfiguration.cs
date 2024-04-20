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

public partial class RdscollectionSettingConfiguration: Extensions.EntityTypeConfiguration<RdscollectionSetting>
{
    public RdscollectionSettingConfiguration(): base() { }
    public RdscollectionSettingConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {

#if NetCore
        HasOne(d => d.Rdscollection).WithMany(p => p.RdscollectionSettings).HasConstraintName("FK_RDSCollectionSettings_RDSCollections");
#else
        HasRequired(d => d.Rdscollection).WithMany(p => p.RdscollectionSettings);
#endif
    }
#endif
    }
