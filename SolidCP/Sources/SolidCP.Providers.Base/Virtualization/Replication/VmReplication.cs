namespace SolidCP.Providers.Virtualization
{
    [Persistent]
    public class VmReplication
    {
        [Persistent]
        public string Thumbprint { get; set; }

        [Persistent]
        public string[] VhdToReplicate { get; set; }

        [Persistent]
        public ReplicaFrequency ReplicaFrequency { get; set; }

        [Persistent]
        public int AdditionalRecoveryPoints { get; set; }

        [Persistent]
        public int VSSSnapshotFrequencyHour { get; set; }
    }
}