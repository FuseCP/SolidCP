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

public partial class RdsmessageConfiguration: EntityTypeConfiguration<Rdsmessage>
{
#if NetCore || NetFX
    public override void Configure() {
        Property(e => e.UserName).IsFixedLength();

#if NetCore
        HasOne(d => d.Rdscollection).WithMany(p => p.Rdsmessages).HasConstraintName("FK_RDSMessages_RDSCollections");
#else
        // TODO required or optional?
        HasRequired(d => d.Rdscollection).WithMany(p => p.Rdsmessages);
#endif
    }
#endif
    }
