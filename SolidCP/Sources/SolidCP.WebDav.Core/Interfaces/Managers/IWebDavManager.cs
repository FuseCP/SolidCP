using System.Collections.Generic;
using System.IO;
using System.Web;
using SolidCP.WebDav.Core.Client;

namespace SolidCP.WebDav.Core.Interfaces.Managers
{
    public interface IWebDavManager
    {
        IEnumerable<IHierarchyItem> OpenFolder(string path);
        bool IsFile(string path);
        bool FileExist(string path);
        byte[] GetFileBytes(string path);
        void UploadFile(string path, HttpPostedFileBase file);
        void UploadFile(string path, byte[] bytes);
        void UploadFile(string path, Stream stream);
        IEnumerable<IHierarchyItem> SearchFiles(int itemId, string pathPart, string searchValue, string uesrPrincipalName, bool recursive);
        IResource GetResource(string path);
        string GetFileUrl(string path);
        void DeleteResource(string path);
        void LockFile(string path);
        string GetFileFolderPath(string path);
    }
}