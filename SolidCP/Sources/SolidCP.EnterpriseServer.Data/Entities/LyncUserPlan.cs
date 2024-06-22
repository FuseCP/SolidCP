#if ScaffoldedEntities
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SolidCP.Providers.HostedSolution;
#if NetCore
using Microsoft.EntityFrameworkCore;
#endif

namespace SolidCP.EnterpriseServer.Data.Entities;

#if NetCore
[Index("LyncUserPlanId", Name = "IX_LyncUserPlans", IsUnique = true)]
[Index("ItemId", Name = "LyncUserPlansIdx_ItemID")]
#endif
public partial class LyncUserPlan
{
    [Key]
    public int LyncUserPlanId { get; set; }

    [Column("ItemID")]
    public int ItemId { get; set; }

    [Required]
    [StringLength(300)]
    public string LyncUserPlanName { get; set; }

    public int? LyncUserPlanType { get; set; }

    [Column("IM")]
    public bool IM { get; set; }

    public bool Mobility { get; set; }

    public bool MobilityEnableOutsideVoice { get; set; }

    public bool Federation { get; set; }

    public bool Conferencing { get; set; }

    public bool EnterpriseVoice { get; set; }

    public LyncVoicePolicyType VoicePolicy { get; set; }

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

    [ForeignKey("ItemId")]
    [InverseProperty("LyncUserPlans")]
    public virtual ExchangeOrganization Item { get; set; }

    [InverseProperty("LyncUserPlan")]
    public virtual ICollection<LyncUser> LyncUsers { get; set; } = new List<LyncUser>();
}
#endif