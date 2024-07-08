#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("RDSCollectionSettings")]
#if NetCore
[Index("RdsCollectionId", Name = "RDSCollectionSettingsIdx_RDSCollectionId")]
#endif
public partial class RdsCollectionSetting
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("RDSCollectionId")]
    public int RdsCollectionId { get; set; }

    public int? DisconnectedSessionLimitMin { get; set; }

    public int? ActiveSessionLimitMin { get; set; }

    public int? IdleSessionLimitMin { get; set; }

    [StringLength(20)]
    public string BrokenConnectionAction { get; set; }

    public bool? AutomaticReconnectionEnabled { get; set; }

    public bool? TemporaryFoldersDeletedOnExit { get; set; }

    public bool? TemporaryFoldersPerSession { get; set; }

    [StringLength(250)]
    public string ClientDeviceRedirectionOptions { get; set; }

    public bool? ClientPrinterRedirected { get; set; }

    public bool? ClientPrinterAsDefault { get; set; }

    [Column("RDEasyPrintDriverEnabled")]
    public bool? RdEasyPrintDriverEnabled { get; set; }

    public int? MaxRedirectedMonitors { get; set; }

    [StringLength(20)]
    public string SecurityLayer { get; set; }

    [StringLength(20)]
    public string EncryptionLevel { get; set; }

    [Column("AuthenticateUsingNLA")]
    public bool? AuthenticateUsingNla { get; set; }

    [ForeignKey("RdsCollectionId")]
    [InverseProperty("RdsCollectionSettings")]
    public virtual RdsCollection RdsCollection { get; set; }
}
#endif