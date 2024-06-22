#if ScaffoldedEntities
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SolidCP.Providers.HostedSolution;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

[Table("SfBUserPlans")]
public partial class SfBUserPlan
{
    [Key]
    [Column("SfBUserPlanId")]
    public int SfBUserPlanId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [Column("SfBUserPlanName")]
    [StringLength(300)]
    public string SfBUserPlanName { get; set; }

    [Column("SfBUserPlanType")]
    public int? SfBUserPlanType { get; set; }

    [Column("IM")]
    public bool IM { get; set; }

    public bool Mobility { get; set; }

    public bool MobilityEnableOutsideVoice { get; set; }

    public bool Federation { get; set; }

    public bool Conferencing { get; set; }

    public bool EnterpriseVoice { get; set; }

    public SfBVoicePolicyType VoicePolicy { get; set; }

    public bool IsDefault { get; set; }

    public bool RemoteUserAccess { get; set; }

    [Column("PublicIMConnectivity")]
    public bool PublicIMConnectivity { get; set; }

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