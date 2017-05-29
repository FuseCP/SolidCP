using SolidCP.WebDav.Core.Attributes.Resources;
using SolidCP.WebDav.Core.Resources;

namespace SolidCP.WebDav.Core
{
    namespace Client
    {
        public enum ItemType
        {
            [LocalizedDescription(typeof(WebDavResources), "ItemTypeResource")]
            Resource,
            [LocalizedDescription(typeof(WebDavResources), "ItemTypeFolder")]
            Folder,
            Version,
            VersionHistory
        }
    }
}