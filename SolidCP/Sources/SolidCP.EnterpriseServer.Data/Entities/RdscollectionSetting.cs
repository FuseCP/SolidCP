#if ScaffoldedEntities
using System;
using System.Collections.Generic;

// This file is auto generated, do not edit.
namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class RdscollectionSetting
{
    public int Id { get; set; }

    public int RdscollectionId { get; set; }

    public int? DisconnectedSessionLimitMin { get; set; }

    public int? ActiveSessionLimitMin { get; set; }

    public int? IdleSessionLimitMin { get; set; }

    public string BrokenConnectionAction { get; set; }

    public bool? AutomaticReconnectionEnabled { get; set; }

    public bool? TemporaryFoldersDeletedOnExit { get; set; }

    public bool? TemporaryFoldersPerSession { get; set; }

    public string ClientDeviceRedirectionOptions { get; set; }

    public bool? ClientPrinterRedirected { get; set; }

    public bool? ClientPrinterAsDefault { get; set; }

    public bool? RdeasyPrintDriverEnabled { get; set; }

    public int? MaxRedirectedMonitors { get; set; }

    public string SecurityLayer { get; set; }

    public string EncryptionLevel { get; set; }

    public bool? AuthenticateUsingNla { get; set; }

    public virtual Rdscollection Rdscollection { get; set; }
}
#endif