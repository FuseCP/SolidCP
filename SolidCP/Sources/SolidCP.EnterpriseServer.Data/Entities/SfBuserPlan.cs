// This file is auto generated, do not edit.
#if ScaffoldedEntities
using System;
using System.Collections.Generic;

namespace SolidCP.EnterpriseServer.Data.Entities;

public partial class SfBuserPlan
{
    public int SfBuserPlanId { get; set; }

    public int ItemId { get; set; }

    public string SfBuserPlanName { get; set; }

    public int? SfBuserPlanType { get; set; }

    public bool Im { get; set; }

    public bool Mobility { get; set; }

    public bool MobilityEnableOutsideVoice { get; set; }

    public bool Federation { get; set; }

    public bool Conferencing { get; set; }

    public bool EnterpriseVoice { get; set; }

    public int VoicePolicy { get; set; }

    public bool IsDefault { get; set; }

    public bool RemoteUserAccess { get; set; }

    public bool PublicImconnectivity { get; set; }

    public bool AllowOrganizeMeetingsWithExternalAnonymous { get; set; }

    public int? Telephony { get; set; }

    public string ServerUri { get; set; }

    public string ArchivePolicy { get; set; }

    public string TelephonyDialPlanPolicy { get; set; }

    public string TelephonyVoicePolicy { get; set; }
}
#endif