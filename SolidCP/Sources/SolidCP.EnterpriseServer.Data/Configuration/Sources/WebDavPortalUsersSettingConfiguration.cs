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

public partial class WebDavPortalUsersSettingConfiguration: EntityTypeConfiguration<WebDavPortalUsersSetting>
{
    public override void Configure() {
        HasKey(e => e.Id).HasName("PK__WebDavPo__3214EC279D0BC918");

        HasOne(d => d.Account).WithMany(p => p.WebDavPortalUsersSettings).HasConstraintName("FK_WebDavPortalUsersSettings_UserId");
    }
}
