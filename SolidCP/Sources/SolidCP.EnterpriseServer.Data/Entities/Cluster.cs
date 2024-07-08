#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Cluster
{
    [Key]
    [Column("ClusterID")]
    public int ClusterId { get; set; }

    [Required]
    [StringLength(100)]
    public string ClusterName { get; set; }

    [InverseProperty("Cluster")]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
#endif