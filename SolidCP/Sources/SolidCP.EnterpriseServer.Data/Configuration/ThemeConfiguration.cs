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

public partial class ThemeConfiguration: EntityTypeConfiguration<Theme>
{
    public override void Configure() {

		#region Seed Data
		HasData(() => new Theme[] {
			new Theme() { DisplayName = "SolidCP v1", DisplayOrder = 1, Enabled = 1, LTRName = "Default", RTLName = "Default", ThemeId = 1 }
		});
		#endregion
	}
}
