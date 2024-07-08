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

public partial class ThemeConfiguration: EntityTypeConfiguration<Theme>
{
    public override void Configure() {

        #region Seed Data
        HasData(() => new Theme[] {
            new Theme() { ThemeId = 1, DisplayName = "SolidCP v1", DisplayOrder = 1, Enabled = 1, LTRName = "Default", RTLName = "Default" }
        });
        #endregion

    }
}
