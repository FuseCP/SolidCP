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

public partial class PrivateNetworkVlanConfiguration : EntityTypeConfiguration<PrivateNetworkVlan>
{
#if NetCore || NetFX
	public override void Configure()
	{
		HasKey(e => e.VlanId).HasName("PK__PrivateN__8348135581B53618");

#if NetCore
        HasOne(d => d.Server).WithMany(p => p.PrivateNetworkVlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ServerID");
#else
		// TODO optional or required?
		HasOptional(d => d.Server).WithMany(p => p.PrivateNetworkVlans);
#endif
	}
#endif
}