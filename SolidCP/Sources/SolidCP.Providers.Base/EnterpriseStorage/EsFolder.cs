using SolidCP.Providers.StorageSpaces;

namespace SolidCP.Providers.EnterpriseStorage
{
    public class EsFolder : StorageSpaceFolder
    {
        public int EnterpriseFolderID { get; set; }
        public int ItemID { get; set; }
        public string FolderName { get; set; }
        public int FolderQuota { get; set; }
        public string LocationDrive { get; set; }
        public string HomeFolder { get; set; }
        public string Domain { get; set; }
        public int? StorageSpaceFolderId { get; set; }
    }
}
