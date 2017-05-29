namespace SolidCP.Providers.StorageSpaces
{
    public class StorageSpaceFolderSearchRequest
    {
        public int StorageSpaceId { get; set; }
        public int StorageSpaceFolderId { get; set; }
        public string SearchPath { get; set; }
        public string SearchValue { get; set; }
    }
}