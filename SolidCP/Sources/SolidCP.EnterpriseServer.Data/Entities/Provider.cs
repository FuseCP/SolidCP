#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class Provider
{
    public int ProviderId { get; set; }

    public int GroupId { get; set; }

    public string ProviderName { get; set; }

    public string DisplayName { get; set; }

    public string ProviderType { get; set; }

    public string EditorControl { get; set; }

    public bool? DisableAutoDiscovery { get; set; }

    public virtual ResourceGroup Group { get; set; }

    public virtual ICollection<ServiceDefaultProperty> ServiceDefaultProperties { get; set; } = new List<ServiceDefaultProperty>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
#endif