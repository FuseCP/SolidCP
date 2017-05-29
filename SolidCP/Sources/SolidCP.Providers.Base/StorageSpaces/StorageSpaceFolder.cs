using System.IO;
using SolidCP.Providers.OS;

namespace SolidCP.Providers.StorageSpaces
{
    public class StorageSpaceFolder : StorageSpaceItem
    {
        public int Id { get; set; }
        public int StorageSpaceId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}