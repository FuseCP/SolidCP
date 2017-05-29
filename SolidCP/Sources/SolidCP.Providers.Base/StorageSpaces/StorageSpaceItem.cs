using SolidCP.Providers.OS;

namespace SolidCP.Providers.StorageSpaces
{
    public class StorageSpaceItem
    {
        public QuotaType FsrmQuotaType { get; set; }
        public long FsrmQuotaSizeBytes { get; set; }
        public long UsedSizeBytes { get; set; }

        public bool IsShared { get; set; }
        public bool IsDisabled { get; set; }
        public string UncPath { get; set; } 
    }
}