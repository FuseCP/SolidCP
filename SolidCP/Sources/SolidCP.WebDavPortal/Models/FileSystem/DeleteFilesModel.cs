using System.Collections.Generic;
using SolidCP.WebDavPortal.Models.Common;

namespace SolidCP.WebDavPortal.Models.FileSystem
{
    public class DeleteFilesModel : AjaxModel
    {
        public DeleteFilesModel()
        {
            DeletedFiles = new List<string>();
        }

        public List<string> DeletedFiles { get; set; }
    }
}