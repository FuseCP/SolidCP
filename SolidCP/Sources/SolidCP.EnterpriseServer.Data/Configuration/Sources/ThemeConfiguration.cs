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

public partial class ThemeConfiguration: Extensions.EntityTypeConfiguration<Theme>
{
    public ThemeConfiguration(): base() { }
    public ThemeConfiguration(DbFlavor flavor): base(flavor) { }

#if NetCore || NetFX
    public override void Configure() {

        #region Seed Data
        HasData(() => new Theme[] {
            new Theme() { DisplayName = "SolidCP v1", DisplayOrder = 1, Enabled = 1, Ltrname = "Default", Rtlname = "Default", ThemeId = 1 }
        });
        #endregion

    }
#endif
}
