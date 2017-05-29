using System;

namespace SolidCP.WebDavPortal.Models.FileSystem
{
    public class UploadFileResult
    {
        private string _error;

        public string error
        {
            get { return _error; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _error = value;
                    deleteUrl = String.Empty;
                    thumbnailUrl = String.Empty;
                    url = String.Empty;
                }
            }
        }

        public string name { get; set; }

        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string deleteUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteType { get; set; }


        public string FullPath { get; set; }
        public string SavedFileName { get; set; }

        public string Title { get; set; }
    }
}