// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities.Sources;

[Table("SfBUserPlans")]
public partial class SfBuserPlan
{
    [Key]
    [Column("SfBUserPlanId")]
    public int SfBuserPlanId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [Column("SfBUserPlanName")]
    [StringLength(300)]
    public string SfBuserPlanName { get; set; }

    [Column("SfBUserPlanType")]
    public int? SfBuserPlanType { get; set; }

    [Column("IM")]
    public bool Im { get; set; }

    public bool Mobility { get; set; }

    public bool MobilityEnableOutsideVoice { get; set; }

    public bool Federation { get; set; }

    public bool Conferencing { get; set; }

    public bool EnterpriseVoice { get; set; }

    public int VoicePolicy { get; set; }

    public bool IsDefault { get; set; }

    public bool RemoteUserAccess { get; set; }

    [Column("PublicIMConnectivity")]
    public bool PublicImconnectivity { get; set; }

    public bool AllowOrganizeMeetingsWithExternalAnonymous { get; set; }

    public int? Telephony { get; set; }

    [Column("ServerURI")]
    [StringLength(300)]
    public string ServerUri { get; set; }

    [StringLength(300)]
    public string ArchivePolicy { get; set; }

    [StringLength(300)]
    public string TelephonyDialPlanPolicy { get; set; }

    [StringLength(300)]
    public string TelephonyVoicePolicy { get; set; }
}
#endif