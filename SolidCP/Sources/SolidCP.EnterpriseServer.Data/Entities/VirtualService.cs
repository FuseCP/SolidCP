#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

#if NetCore
[Index("ServerId", Name = "VirtualServicesIdx_ServerID")]
[Index("ServiceId", Name = "VirtualServicesIdx_ServiceID")]
#endif
public partial class VirtualService
{
    [Key]
    [Column("VirtualServiceID")]
    public int VirtualServiceId { get; set; }

    [Column("ServerID")]
    public int ServerId { get; set; }

    [Column("ServiceID")]
    public int ServiceId { get; set; }

    [ForeignKey("ServerId")]
    [InverseProperty("VirtualServices")]
    public virtual Server Server { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("VirtualServices")]
    public virtual Service Service { get; set; }
}
#endif