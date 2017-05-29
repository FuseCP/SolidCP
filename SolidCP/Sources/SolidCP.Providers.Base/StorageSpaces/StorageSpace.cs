using SolidCP.Providers.OS;

namespace SolidCP.Providers.StorageSpaces
{
    public class StorageSpace : StorageSpaceItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ServiceId { get; set; }
        public int ServerId { get; set; }
        public int LevelId { get; set; }
        public long ActuallyUsedInBytes { get; set; }
        public string Path { get; set; }
        public long DiskFreeSpaceInBytes { get; set; }
    }
}