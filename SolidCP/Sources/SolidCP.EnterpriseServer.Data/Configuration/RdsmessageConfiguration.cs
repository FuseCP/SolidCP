﻿using System;
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

public partial class RdsMessageConfiguration: EntityTypeConfiguration<RdsMessage>
{
#if NetCore || NetFX
    public override void Configure() {
        Property(e => e.UserName).IsFixedLength();

#if NetCore
        HasOne(d => d.RdsCollection).WithMany(p => p.RdsMessages).HasConstraintName("FK_RDSMessages_RDSCollections");
#else
        // TODO required or optional?
        HasRequired(d => d.RdsCollection).WithMany(p => p.RdsMessages);
#endif
    }
#endif
    }