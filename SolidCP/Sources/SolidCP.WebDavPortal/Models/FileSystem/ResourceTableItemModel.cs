using System;
using SolidCP.WebDav.Core.Client;
using SolidCP.WebDavPortal.Models.Common.DataTable;

namespace SolidCP.WebDavPortal.Models.FileSystem
{
    public class ResourceTableItemModel : JqueryDataTableBaseEntity
    {
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public Uri Href { get; set; }
        public bool IsTargetBlank { get; set; }
        public bool IsFolder { get; set; }
        public long Size { get; set; }
        public bool IsRoot { get; set; }
        public long Quota { get; set; }
        public string Type { get; set; }
        public DateTime LastModified { get; set; }
        public string LastModifiedFormated { get; set; }
        public string IconHref { get; set; }
        public string FolderUrlAbsoluteString { get; set; }
        public string FolderUrlLocalString { get; set; }
        public string FolderName { get; set; }
        public string Summary { get; set; }

        public override dynamic this[int index]
        {
            get
            {
                switch (index)
                {
                    case 1 :
                    {
                        return Size;
                    }
                    case 2:
                    {
                        return LastModified;
                    }
                    case 3:
                    {
                        return Type;
                    }
                    default:
                    {
                        return DisplayName;
                    }
                }
            }
        }
    }
}