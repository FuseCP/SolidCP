// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;

using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Server.Utils;
using SolidCP.Providers.DNS;
using SolidCP.Providers.DomainLookup;
using System.Collections.Generic;

namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for OperatingSystem
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class OperatingSystem : HostingServiceProviderWebService, IOperatingSystem
    {
        private IOperatingSystem OsProvider
        {
            get { return (IOperatingSystem)Provider; }
        }

        #region Files
        [WebMethod, SoapHeader("settings")]
        public string CreatePackageFolder(string initialPath)
        {
            try
            {
                Log.WriteStart("'{0}' CreatePackageFolder", ProviderSettings.ProviderName);
                string result = OsProvider.CreatePackageFolder(initialPath);
                Log.WriteEnd("'{0}' CreatePackageFolder", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreatePackageFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool FileExists(string path)
        {
            try
            {
                Log.WriteStart("'{0}' FileExists", ProviderSettings.ProviderName);
                bool result = OsProvider.FileExists(path);
                Log.WriteEnd("'{0}' FileExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' FileExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool DirectoryExists(string path)
        {
            try
            {
                Log.WriteStart("'{0}' DirectoryExists", ProviderSettings.ProviderName);
                bool result = OsProvider.DirectoryExists(path);
                Log.WriteEnd("'{0}' DirectoryExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DirectoryExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile GetFile(string path)
        {
            try
            {
                Log.WriteStart("'{0}' GetFile", ProviderSettings.ProviderName);
                SystemFile result = OsProvider.GetFile(path);
                Log.WriteEnd("'{0}' GetFile", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFile", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile[] GetFiles(string path)
        {
            try
            {
                Log.WriteStart("'{0}' GetFiles", ProviderSettings.ProviderName);
                SystemFile[] result = OsProvider.GetFiles(path);
                Log.WriteEnd("'{0}' GetFiles", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFiles", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile[] GetDirectoriesRecursive(string rootFolder, string path)
        {
            try
            {
                Log.WriteStart("'{0}' GetDirectoriesRecursive", ProviderSettings.ProviderName);
                SystemFile[] result = OsProvider.GetDirectoriesRecursive(rootFolder, path);
                Log.WriteEnd("'{0}' GetDirectoriesRecursive", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetDirectoriesRecursive", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile[] GetFilesRecursive(string rootFolder, string path)
        {
            try
            {
                Log.WriteStart("'{0}' GetFilesRecursive", ProviderSettings.ProviderName);
                SystemFile[] result = OsProvider.GetFilesRecursive(rootFolder, path);
                Log.WriteEnd("'{0}' GetFilesRecursive", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFilesRecursive", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern)
        {
            try
            {
                Log.WriteStart("'{0}' GetFilesRecursiveByPattern", ProviderSettings.ProviderName);
                SystemFile[] result = OsProvider.GetFilesRecursiveByPattern(rootFolder, path, pattern);
                Log.WriteEnd("'{0}' GetFilesRecursiveByPattern", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFilesRecursiveByPattern", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public byte[] GetFileBinaryContent(string path)
        {
            try
            {
                Log.WriteStart("'{0}' GetFileBinaryContent", ProviderSettings.ProviderName);
                byte[] result = OsProvider.GetFileBinaryContent(path);
                Log.WriteEnd("'{0}' GetFileBinaryContent", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFileBinaryContent", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

		[WebMethod, SoapHeader("settings")]
		public byte[] GetFileBinaryContentUsingEncoding(string path, string encoding)
		{
			try
			{
				Log.WriteStart("'{0}' GetFileBinaryContentUsingEncoding", ProviderSettings.ProviderName);
				byte[] result = OsProvider.GetFileBinaryContentUsingEncoding(path, encoding);
				Log.WriteEnd("'{0}' GetFileBinaryContentUsingEncoding", ProviderSettings.ProviderName);
				return result;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("'{0}' GetFileBinaryContentUsingEncoding", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            try
            {
                Log.WriteStart("'{0}' GetFileBinaryChunk", ProviderSettings.ProviderName);
                byte[] result = OsProvider.GetFileBinaryChunk(path, offset, length);
                Log.WriteEnd("'{0}' GetFileBinaryChunk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFileBinaryContent", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string GetFileTextContent(string path)
        {
            try
            {
                Log.WriteStart("'{0}' GetFileTextContent", ProviderSettings.ProviderName);
                string result = OsProvider.GetFileTextContent(path);
                Log.WriteEnd("'{0}' GetFileTextContent", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFileTextContent", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateFile(string path)
        {
            try
            {
                Log.WriteStart("'{0}' CreateFile", ProviderSettings.ProviderName);
                OsProvider.CreateFile(path);
                Log.WriteEnd("'{0}' CreateFile", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateFile", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateDirectory(string path)
        {
            try
            {
                Log.WriteStart("'{0}' CreateDirectory", ProviderSettings.ProviderName);
                OsProvider.CreateDirectory(path);
                Log.WriteEnd("'{0}' CreateDirectory", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateDirectory", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ChangeFileAttributes(string path, DateTime createdTime, DateTime changedTime)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeFileAttributes", ProviderSettings.ProviderName);
                OsProvider.ChangeFileAttributes(path, createdTime, changedTime);
                Log.WriteEnd("'{0}' ChangeFileAttributes", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeFileAttributes", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteFile(string path)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteFile", ProviderSettings.ProviderName);
                OsProvider.DeleteFile(path);
                Log.WriteEnd("'{0}' DeleteFile", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteFile", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteFiles(string[] files)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteFiles", ProviderSettings.ProviderName);
                OsProvider.DeleteFiles(files);
                Log.WriteEnd("'{0}' DeleteFiles", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteFiles", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteEmptyDirectories(string[] directories)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteEmptyDirectories", ProviderSettings.ProviderName);
                OsProvider.DeleteEmptyDirectories(directories);
                Log.WriteEnd("'{0}' DeleteEmptyDirectories", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteEmptyDirectories", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateFileBinaryContent(string path, byte[] content)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateFileBinaryContent", ProviderSettings.ProviderName);
                OsProvider.UpdateFileBinaryContent(path, content);
                Log.WriteEnd("'{0}' UpdateFileBinaryContent", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateFileBinaryContent", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding)
        {
            try
            {
				Log.WriteStart("'{0}' UpdateFileBinaryContentUsingEncoding", ProviderSettings.ProviderName);
                OsProvider.UpdateFileBinaryContentUsingEncoding(path, content, encoding);
				Log.WriteEnd("'{0}' UpdateFileBinaryContentUsingEncoding", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
				Log.WriteError(String.Format("'{0}' UpdateFileBinaryContentUsingEncoding", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void AppendFileBinaryContent(string path, byte[] chunk)
        {
            try
            {
                Log.WriteStart("'{0}' AppendFileBinaryContent", ProviderSettings.ProviderName);
                OsProvider.AppendFileBinaryContent(path, chunk);
                Log.WriteEnd("'{0}' AppendFileBinaryContent", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AppendFileBinaryContent", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateFileTextContent(string path, string content)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateFileTextContent", ProviderSettings.ProviderName);
                OsProvider.UpdateFileTextContent(path, content);
                Log.WriteEnd("'{0}' UpdateFileTextContent", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateFileTextContent", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void MoveFile(string sourcePath, string destinationPath)
        {
            try
            {
                Log.WriteStart("'{0}' MoveFile", ProviderSettings.ProviderName);
                OsProvider.MoveFile(sourcePath, destinationPath);
                Log.WriteEnd("'{0}' MoveFile", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' MoveFile", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CopyFile(string sourcePath, string destinationPath)
        {
            try
            {
                Log.WriteStart("'{0}' CopyFile", ProviderSettings.ProviderName);
                OsProvider.CopyFile(sourcePath, destinationPath);
                Log.WriteEnd("'{0}' CopyFile", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CopyFile", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ZipFiles(string zipFile, string rootPath, string[] files)
        {
            try
            {
                Log.WriteStart("'{0}' ZipFiles", ProviderSettings.ProviderName);
                OsProvider.ZipFiles(zipFile, rootPath, files);
                Log.WriteEnd("'{0}' ZipFiles", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ZipFiles", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] UnzipFiles(string zipFile, string destFolder)
        {
            try
            {
                Log.WriteStart("'{0}' UnzipFiles", ProviderSettings.ProviderName);
                string[] result = OsProvider.UnzipFiles(zipFile, destFolder);
                Log.WriteEnd("'{0}' UnzipFiles", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UnzipFiles", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateAccessDatabase(string databasePath)
        {
            try
            {
                Log.WriteStart("'{0}' CreateAccessDatabase", ProviderSettings.ProviderName);
                OsProvider.CreateAccessDatabase(databasePath);
                Log.WriteEnd("'{0}' CreateAccessDatabase", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateAccessDatabase", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public UserPermission[] GetGroupNtfsPermissions(string path, UserPermission[] users, string usersOU)
        {
            try
            {
                Log.WriteStart("'{0}' GetGroupNtfsPermissions", ProviderSettings.ProviderName);
                UserPermission[] result = OsProvider.GetGroupNtfsPermissions(path, users, usersOU);
                Log.WriteEnd("'{0}' GetGroupNtfsPermissions", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetGroupNtfsPermissions", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void GrantGroupNtfsPermissions(string path, UserPermission[] users, string usersOU, bool resetChildPermissions)
        {
            try
            {
                Log.WriteStart("'{0}' GrantGroupNtfsPermissions", ProviderSettings.ProviderName);
                OsProvider.GrantGroupNtfsPermissions(path, users, usersOU, resetChildPermissions);
                Log.WriteEnd("'{0}' GrantGroupNtfsPermissions", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GrantGroupNtfsPermissions", ProviderSettings.ProviderName), ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
        {
            try
            {
                Log.WriteStart("'{0}' SetQuotaLimitOnFolder", ProviderSettings.ProviderName);
                OsProvider.SetQuotaLimitOnFolder(folderPath, shareNameDrive, quotaType, quotaLimit, mode, wmiUserName, wmiPassword);
                Log.WriteEnd("'{0}' SetQuotaLimitOnFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetQuotaLimitOnFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword)
        {
            try
            {
                Log.WriteStart("'{0}' GetQuotaOnFolder", ProviderSettings.ProviderName);
                var result = OsProvider.GetQuotaOnFolder(folderPath, wmiUserName, wmiPassword);
                Log.WriteEnd("'{0}' GetQuotaOnFolder", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetQuotaOnFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteDirectoryRecursive(string rootPath)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteDirectoryRecursive", ProviderSettings.ProviderName);
                OsProvider.DeleteDirectoryRecursive(rootPath);
                Log.WriteEnd("'{0}' DeleteDirectoryRecursive", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteDirectoryRecursive", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool CheckFileServicesInstallation()
        {
            try
            {
                Log.WriteStart("'{0}' CheckFileServicesInstallation", ProviderSettings.ProviderName);
                bool bResult =  OsProvider.CheckFileServicesInstallation();
                Log.WriteEnd("'{0}' CheckFileServicesInstallation", ProviderSettings.ProviderName);
                return bResult;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CheckFileServicesInstallation", ProviderSettings.ProviderName), ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public bool InstallFsrmService()
        {
            try
            {
                Log.WriteStart("'{0}' InstallFsrmService", ProviderSettings.ProviderName);
                bool bResult = OsProvider.InstallFsrmService();
                Log.WriteEnd("'{0}' InstallFsrmService", ProviderSettings.ProviderName);
                return bResult;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' InstallFsrmService", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Synchronizing
        [WebMethod, SoapHeader("settings")]
        public FolderGraph GetFolderGraph(string path)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolderGraph", ProviderSettings.ProviderName);
                FolderGraph result = OsProvider.GetFolderGraph(path);
                Log.WriteEnd("'{0}' GetFolderGraph", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolderGraph", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ExecuteSyncActions(FileSyncAction[] actions)
        {
            try
            {
                Log.WriteStart("'{0}' ExecuteSyncActions", ProviderSettings.ProviderName);
                OsProvider.ExecuteSyncActions(actions);
                Log.WriteEnd("'{0}' ExecuteSyncActions", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ExecuteSyncActions", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region ODBC DSNs
        [WebMethod, SoapHeader("settings")]
        public string[] GetInstalledOdbcDrivers()
        {
            try
            {
                Log.WriteStart("'{0}' GetInstalledOdbcDrivers", ProviderSettings.ProviderName);
                string[] result = OsProvider.GetInstalledOdbcDrivers();
                Log.WriteEnd("'{0}' GetInstalledOdbcDrivers", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetInstalledOdbcDrivers", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetDSNNames()
        {
            try
            {
                Log.WriteStart("'{0}' GetDSNNames", ProviderSettings.ProviderName);
                string[] result = OsProvider.GetDSNNames();
                Log.WriteEnd("'{0}' GetDSNNames", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetDSNNames", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemDSN GetDSN(string dsnName)
        {
            try
            {
                Log.WriteStart("'{0}' GetDSN", ProviderSettings.ProviderName);
                SystemDSN result = OsProvider.GetDSN(dsnName);
                Log.WriteEnd("'{0}' GetDSN", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetDSN", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateDSN(SystemDSN dsn)
        {
            try
            {
                Log.WriteStart("'{0}' CreateDSN", ProviderSettings.ProviderName);
                OsProvider.CreateDSN(dsn);
                Log.WriteEnd("'{0}' CreateDSN", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateDSN", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateDSN(SystemDSN dsn)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateDSN", ProviderSettings.ProviderName);
                OsProvider.UpdateDSN(dsn);
                Log.WriteEnd("'{0}' UpdateDSN", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateDSN", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteDSN(string dsnName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteDSN", ProviderSettings.ProviderName);
                OsProvider.DeleteDSN(dsnName);
                Log.WriteEnd("'{0}' DeleteDSN", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteDSN", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

    }
}
