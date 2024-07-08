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

public partial class RdsmessageConfiguration: EntityTypeConfiguration<Rdsmessage>
{
    public override void Configure() {
        Property(e => e.UserName).IsFixedLength();

        HasOne(d => d.Rdscollection).WithMany(p => p.Rdsmessages).HasConstraintName("FK_RDSMessages_RDSCollections");
    }
}
