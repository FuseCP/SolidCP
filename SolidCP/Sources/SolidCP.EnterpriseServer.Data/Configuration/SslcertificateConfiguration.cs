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

public partial class SslcertificateConfiguration: EntityTypeConfiguration<Sslcertificate>
{
#if NetCore || NetFX
    public override void Configure() {

        Property(e => e.Id).ValueGeneratedOnAdd();

    }
#endif
}