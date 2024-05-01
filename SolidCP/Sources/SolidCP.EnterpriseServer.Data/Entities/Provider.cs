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
[Index("GroupId", Name = "ProvidersIdx_GroupID")]
#endif
public partial class Provider
{
    [Key]
    [Column("ProviderID")]
    public int ProviderId { get; set; }

    [Column("GroupID")]
    public int GroupId { get; set; }

    [StringLength(100)]
    public string ProviderName { get; set; }

    [Required]
    [StringLength(200)]
    public string DisplayName { get; set; }

    [StringLength(400)]
    public string ProviderType { get; set; }

    [StringLength(100)]
    public string EditorControl { get; set; }

    public bool? DisableAutoDiscovery { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("Providers")]
    public virtual ResourceGroup Group { get; set; }

    [InverseProperty("Provider")]
    public virtual ICollection<ServiceDefaultProperty> ServiceDefaultProperties { get; set; } = new List<ServiceDefaultProperty>();

    [InverseProperty("Provider")]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
#endif