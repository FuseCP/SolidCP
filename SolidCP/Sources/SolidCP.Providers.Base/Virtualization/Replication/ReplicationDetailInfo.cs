using System;

namespace SolidCP.Providers.Virtualization
{
    public class ReplicationDetailInfo
    {
        public VmReplicationMode Mode { get; set; }
        public ReplicationState State { get; set; }
        public ReplicationHealth Health { get; set; }
        public string HealthDetails { get; set; }
        public string PrimaryServerName { get; set; }
        public string ReplicaServerName { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string AverageSize { get; set; }
        public string MaximumSize { get; set; }
        public TimeSpan AverageLatency { get; set; }
        public int Errors { get; set; }
        public int SuccessfulReplications { get; set; }
        public int MissedReplicationCount { get; set; }
        public string PendingSize { get; set; }
        public DateTime LastSynhronizedAt { get; set; }
    }
}