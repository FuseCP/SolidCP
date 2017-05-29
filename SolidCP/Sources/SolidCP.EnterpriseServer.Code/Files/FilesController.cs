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
using System.Threading;
using System.IO;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using SolidCP.Providers;
using OS = SolidCP.Providers.OS;
using SolidCP.Providers.OS;
using SolidCP.Providers.Web;

namespace SolidCP.EnterpriseServer
{
    public class FilesController
    {
        public static SystemSettings GetFileManagerSettings()
        {
            return SystemController.GetSystemSettingsInternal(SystemSettings.FILEMANAGER_SETTINGS, false);
        }

        public static OS.OperatingSystem GetOS(int packageId)
        {
            int sid = PackageController.GetPackageServiceId(packageId, ResourceGroups.Os);
            if (sid <= 0)
                return null;

            OS.OperatingSystem os = new OS.OperatingSystem();
            ServiceProviderProxy.Init(os, sid);

            return os;
        }

        public static string GetHomeFolder(int packageId)
        {
            // check context
            string key = "HomeFolder" + packageId.ToString();
            if (HttpContext.Current != null && HttpContext.Current.Items[key] != null)
                return (string)HttpContext.Current.Items[key];

            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(packageId, typeof(HomeFolder));
            string path = (items.Count > 0) ? items[0].Name : null;
            
            // place to context
            if (HttpContext.Current != null)
                HttpContext.Current.Items[key] = path;

            return path;
        }

        public static string GetFullPackagePath(int packageId, string path)
        {
            string homeFolder = GetHomeFolder(packageId);
            string correctedPath = CorrectRelativePath(path);
            return Path.Combine(homeFolder, correctedPath);
        }

		public static string GetFullUncPackagePath(int packageId, int serviceId, string path)
		{
			return ConvertToUncPath(serviceId, GetFullPackagePath(packageId, path));
		}

        public static string GetVirtualPackagePath(int packageId, string fullPath)
        {
			if (String.IsNullOrEmpty(fullPath))
				return fullPath;

			// check for UNC
			int signIdx = fullPath.IndexOf("$");
			if (signIdx > -1)
			{
				fullPath = fullPath.Substring(signIdx - 1).Replace("$", ":");
			}

            string homeFolder = GetHomeFolder(packageId);
            string path = "\\";
            if(fullPath.Length >= homeFolder.Length)
                path = fullPath.Substring(homeFolder.Length);
            if (path == "")
                path = "\\";
            return path;
        }

        public static string CorrectRelativePath(string relativePath)
        {
            // clean path
            string correctedPath = Regex.Replace(relativePath.Replace("/", "\\"),
                    @"\.\\|\.\.|\\\\|\?|\:|\""|\<|\>|\||%|\$", "");
            if (correctedPath.StartsWith("\\"))
                correctedPath = correctedPath.Substring(1);
            return correctedPath;
        }

        public static List<SystemFile> GetFiles(int packageId, string path, bool includeFiles)
        {
            OS.OperatingSystem os = GetOS(packageId);

            string fullPath = GetFullPackagePath(packageId, path);
            List<SystemFile> filteredFiles = new List<SystemFile>();
            SystemFile[] files = os.GetFiles(fullPath);

            foreach (SystemFile file in files)
            {
                if (file.IsDirectory || includeFiles)
                    filteredFiles.Add(file);
            }
            
            return filteredFiles;
        }

        public static List<SystemFile> GetFilesByMask(int packageId, string path, string filesMask)
        {
            return null;
        }

        public static byte[] GetFileBinaryContent(int packageId, string path)
        {
            OS.OperatingSystem os = GetOS(packageId);
            string fullPath = GetFullPackagePath(packageId, path);

            // create file
            return os.GetFileBinaryContent(fullPath);
        }

		public static byte[] GetFileBinaryContentUsingEncoding(int packageId, string path, string encoding)
		{
			OS.OperatingSystem os = GetOS(packageId);
			string fullPath = GetFullPackagePath(packageId, path);

			// create file
			return os.GetFileBinaryContentUsingEncoding(fullPath, encoding);
		}

        public static int UpdateFileBinaryContent(int packageId, string path, byte[] content)
        {

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("FILES", "UPDATE_BINARY_CONTENT", path, packageId);

            try
            {
                OS.OperatingSystem os = GetOS(packageId);
                string fullPath = GetFullPackagePath(packageId, path);

                // create file
                os.UpdateFileBinaryContent(fullPath, content);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

		public static int UpdateFileBinaryContentUsingEncoding(int packageId, string path, byte[] content, string encoding)
		{

			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// check package
			int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			// place log record
            TaskManager.StartTask("FILES", "UPDATE_BINARY_CONTENT", path, packageId);

			try
			{
				OS.OperatingSystem os = GetOS(packageId);
				string fullPath = GetFullPackagePath(packageId, path);

				// create file
				os.UpdateFileBinaryContentUsingEncoding(fullPath, content, encoding);

				return 0;
			}
			catch (Exception ex)
			{
				throw TaskManager.WriteError(ex);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

        public static byte[] GetFileBinaryChunk(int packageId, string path, int offset, int length)
        {
            OS.OperatingSystem os = GetOS(packageId);
            string fullPath = GetFullPackagePath(packageId, path);

            return os.GetFileBinaryChunk(fullPath, offset, length);
        }

        public static int AppendFileBinaryChunk(int packageId, string path, byte[] chunk)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            OS.OperatingSystem os = GetOS(packageId);
            string fullPath = GetFullPackagePath(packageId, path);

            os.AppendFileBinaryContent(fullPath, chunk);

            return 0;
        }

        public static int DeleteFiles(int packageId, string[] files)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("FILES", "DELETE_FILES", packageId);

            if (files != null)
            {
                foreach (string file in files)
                    TaskManager.Write(file);
            }

            try
            {
                OS.OperatingSystem os = GetOS(packageId);
                for (int i = 0; i < files.Length; i++)
                    files[i] = GetFullPackagePath(packageId, files[i]);

                // delete files
                os.DeleteFiles(files);

                return 0;
            }
            catch (Exception ex)
            {
                //Log and return a generic error rather than throwing an exception
                TaskManager.WriteError(ex);
                return BusinessErrorCodes.ERROR_FILE_GENERIC_LOGGED;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int CreateFile(int packageId, string path)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("FILES", "CREATE_FILE", path, packageId);

            try
            {
                OS.OperatingSystem os = GetOS(packageId);
                string fullPath = GetFullPackagePath(packageId, path);

                // cannot create a file with the same name as a directory
                if (os.DirectoryExists(fullPath))
                    return BusinessErrorCodes.ERROR_FILE_CREATE_FILE_WITH_DIR_NAME;

                // create file
                os.CreateFile(fullPath);

                return 0;
            }
            catch (Exception ex)
            {
                //Log and return a generic error rather than throwing an exception
                TaskManager.WriteError(ex);
                return BusinessErrorCodes.ERROR_FILE_GENERIC_LOGGED;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static bool FileExists(int packageId, string path)
        {
            OS.OperatingSystem os = GetOS(packageId);
            string fullPath = GetFullPackagePath(packageId, path);
            return os.FileExists(fullPath);
        }

        public static bool DirectoryExists(int packageId, string path)
        {
            OS.OperatingSystem os = GetOS(packageId);
            string fullPath = GetFullPackagePath(packageId, path);
            return os.DirectoryExists(fullPath);
        }

        public static int CreateFolder(int packageId, string path)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("FILES", "CREATE_FOLDER", path, packageId);

            try
            {

                OS.OperatingSystem os = GetOS(packageId);
                string fullPath = GetFullPackagePath(packageId, path);

                // create folder
                os.CreateDirectory(fullPath);

                return 0;
            }
            catch (Exception ex)
            {
                //Log and return a generic error rather than throwing an exception
                TaskManager.WriteError(ex);
                return BusinessErrorCodes.ERROR_FILE_GENERIC_LOGGED;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int CopyFiles(int packageId, string[] files, string destFolder)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // check dest folder exists
            if (!DirectoryExists(packageId, destFolder)) return BusinessErrorCodes.ERROR_FILE_DEST_FOLDER_NONEXISTENT;

            // place log record
            TaskManager.StartTask("FILES", "COPY_FILES", packageId);
            TaskManager.WriteParameter("Destination folder", destFolder);
            if (files != null)
            {
                foreach (string file in files)
                    TaskManager.Write(file);
            }

            try
            {
                OS.OperatingSystem os = GetOS(packageId);
                string destFullFolder = GetFullPackagePath(packageId, destFolder);

                for (int i = 0; i < files.Length; i++)
                {
                    string srcFilePath = GetFullPackagePath(packageId, files[i]);
                    string destFilePath = Path.Combine(destFullFolder,
                        srcFilePath.Substring(srcFilePath.LastIndexOf("\\") + 1));

                    if (srcFilePath == destFilePath)
                    {
                        return BusinessErrorCodes.ERROR_FILE_COPY_TO_SELF;
                    }
                    //Check that we're not trying to copy a folder into its own subfolder
                    else if (destFilePath.StartsWith(srcFilePath + "\\"))
                    {
                        return BusinessErrorCodes.ERROR_FILE_COPY_TO_OWN_SUBFOLDER;
                    }
                    else
                    {
                        os.CopyFile(srcFilePath, destFilePath);
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                //Log and return a generic error rather than throwing an exception
                TaskManager.WriteError(ex);
                return BusinessErrorCodes.ERROR_FILE_GENERIC_LOGGED;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int MoveFiles(int packageId, string[] files, string destFolder)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // check dest folder exists
            if (!DirectoryExists(packageId, destFolder)) return BusinessErrorCodes.ERROR_FILE_DEST_FOLDER_NONEXISTENT;

            // place log record
            TaskManager.StartTask("FILES", "MOVE_FILES", packageId);

            TaskManager.WriteParameter("Destination folder", destFolder);
            if (files != null)
            {
                foreach (string file in files)
                    TaskManager.Write(file);
            }

            try
            {
                OS.OperatingSystem os = GetOS(packageId);
                string destFullFolder = GetFullPackagePath(packageId, destFolder);

                for (int i = 0; i < files.Length; i++)
                {
                    string srcFilePath = GetFullPackagePath(packageId, files[i]);
                    string destFilePath = Path.Combine(destFullFolder,
                        srcFilePath.Substring(srcFilePath.LastIndexOf("\\") + 1));
                    if (srcFilePath == destFilePath)
                    {
                        return BusinessErrorCodes.ERROR_FILE_COPY_TO_SELF;
                    }
                    //Check that we're not trying to copy a folder into its own subfolder
                    else if (destFilePath.StartsWith(srcFilePath + "\\"))
                    {
                        return BusinessErrorCodes.ERROR_FILE_COPY_TO_OWN_SUBFOLDER;
                    }
                    else if (os.FileExists(destFilePath) || os.DirectoryExists(destFilePath))
                    {
                        return BusinessErrorCodes.ERROR_FILE_MOVE_PATH_ALREADY_EXISTS;
                    }
                    else
                    {
                        os.MoveFile(srcFilePath, destFilePath);
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                //Log and return a generic error rather than throwing an exception
                TaskManager.WriteError(ex);
                return BusinessErrorCodes.ERROR_FILE_GENERIC_LOGGED;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int RenameFile(int packageId, string oldPath, string newPath)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("FILES", "RENAME_FILE", oldPath, packageId);

            TaskManager.WriteParameter("New name", newPath);

            try
            {
                OS.OperatingSystem os = GetOS(packageId);
                string oldFullPath = GetFullPackagePath(packageId, oldPath);
                string destFullPath = GetFullPackagePath(packageId, newPath);

                os.MoveFile(oldFullPath, destFullPath);

                return 0;
            }
            catch (Exception ex)
            {
                //Log and return a generic error rather than throwing an exception
                TaskManager.WriteError(ex);
                return BusinessErrorCodes.ERROR_FILE_GENERIC_LOGGED;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static string[] UnzipFiles(int packageId, string[] files)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return null;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return null;

            // place log record
            TaskManager.StartTask("FILES", "UNZIP_FILES", packageId);

            if (files != null)
            {
                foreach (string file in files)
                    TaskManager.Write(file);
            }

            try
            {

                List<string> unzippedFiles = new List<string>();

                OS.OperatingSystem os = GetOS(packageId);

                for (int i = 0; i < files.Length; i++)
                {
                    string zipFilePath = GetFullPackagePath(packageId, files[i]);
                    string destFolderPath = zipFilePath.Substring(0, zipFilePath.LastIndexOf("\\"));
                    unzippedFiles.AddRange(os.UnzipFiles(zipFilePath, destFolderPath));
                }

                return unzippedFiles.ToArray();
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int ZipFiles(int packageId, string[] files, string archivePath)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("FILES", "ZIP_FILES", archivePath, packageId);

            if (files != null)
            {
                foreach (string file in files)
                    TaskManager.Write(file);
            }

            try
            {

                OS.OperatingSystem os = GetOS(packageId);
                string zipFilePath = GetFullPackagePath(packageId, archivePath);

                List<string> archFiles = new List<string>();
                string rootFolder = "";
                foreach (string file in files)
                {
                    string archFile = GetFullPackagePath(packageId, file);
                    int idx = archFile.LastIndexOf("\\");
                    rootFolder = archFile.Substring(0, idx);
                    archFiles.Add(archFile.Substring(idx + 1));
                }

                os.ZipFiles(zipFilePath, rootFolder, archFiles.ToArray());

                return 0;
            }
            catch (Exception ex)
            {
                //Log and return a generic error rather than throwing an exception
                TaskManager.WriteError(ex);
                return BusinessErrorCodes.ERROR_FILE_GENERIC_LOGGED;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

		public static int ZipRemoteFiles(int packageId, string rootFolder, string[] files, string archivePath)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// check package
			int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
			if (packageCheck < 0) return packageCheck;

			// place log record
            TaskManager.StartTask("FILES", "ZIP_FILES", archivePath, packageId);

			if (files != null)
			{
				foreach (string file in files)
					TaskManager.Write(file);
			}

			try
			{

				OS.OperatingSystem os = GetOS(packageId);
				string zipFilePath = GetFullPackagePath(packageId, archivePath);

				List<string> archFiles = new List<string>();
				string root = String.IsNullOrEmpty(rootFolder) ? "" : GetFullPackagePath(packageId, rootFolder);
				foreach (string file in files)
				{
					string archFile = GetFullPackagePath(packageId, file);
					if (!String.IsNullOrEmpty(rootFolder))
					{

						archFiles.Add(archFile.Substring(root.Length + 1));
					}
					else
					{
						int idx = archFile.LastIndexOf("\\");
						root = archFile.Substring(0, idx);
						archFiles.Add(archFile.Substring(idx + 1));
					}
				}

				os.ZipFiles(zipFilePath, root, archFiles.ToArray());

				return 0;
			}
			catch (Exception ex)
			{
                //Log and return a generic error rather than throwing an exception
                TaskManager.WriteError(ex);
                return BusinessErrorCodes.ERROR_FILE_GENERIC_LOGGED;
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

        public static int CreateAccessDatabase(int packageId, string dbPath)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("FILES", "CREATE_ACCESS_DATABASE", dbPath, packageId);

            try
            {
                OS.OperatingSystem os = GetOS(packageId);
                string fullPath = GetFullPackagePath(packageId, dbPath);

                os.CreateAccessDatabase(fullPath);

                return 0;
            }
            catch (Exception ex)
            {
                //Log and return a generic error rather than throwing an exception
                TaskManager.WriteError(ex);
                return BusinessErrorCodes.ERROR_FILE_GENERIC_LOGGED;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int CalculatePackageDiskspace(int packageId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("SPACE", "CALCULATE_DISKSPACE", packageId);

            try
            {
                // create thread parameters
                ThreadStartParameters prms = new ThreadStartParameters();
                prms.UserId = SecurityContext.User.UserId;
                prms.Parameters = new object[] { packageId };

                Thread t = new Thread(new ParameterizedThreadStart(CalculatePackageDiskspaceAsync));
                t.Start(prms);
                return 0;

            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        private static void CalculatePackageDiskspaceAsync(object objPrms)
        {
            ThreadStartParameters prms = (ThreadStartParameters)objPrms;

            // impersonate thread
            SecurityContext.SetThreadPrincipal(prms.UserId);

            int packageId = (int)prms.Parameters[0];
            try
            {
                // calculate
                CalculatePackagesDiskspaceTask calc = new CalculatePackagesDiskspaceTask();
                calc.CalculatePackage(packageId);
            }
            catch (Exception ex)
            {
                // write to audit log
                TaskManager.WriteError(ex);
            }
        }

        public static UserPermission[] GetFilePermissions(int packageId, string path)
        {
            try
            {
                // get all accounts
                UserPermission[] users = GetAvailableSecurityAccounts(packageId);

                OS.OperatingSystem os = GetOS(packageId);
                string fullPath = GetFullPackagePath(packageId, path);

                // get users OU defined on web server
                string usersOU = WebServerController.GetWebUsersOU(packageId);

                users = os.GetGroupNtfsPermissions(fullPath, users, usersOU);

                return users;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
        }

        public static int SetFilePermissions(int packageId, string path, UserPermission[] users, bool resetChildPermissions)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("FILES", "SET_PERMISSIONS", path, packageId);

            try
            {
                OS.OperatingSystem os = GetOS(packageId);
                string fullPath = GetFullPackagePath(packageId, path);

                // get users OU defined on web server
                string usersOU = WebServerController.GetWebUsersOU(packageId);

                os.GrantGroupNtfsPermissions(fullPath, users, usersOU, resetChildPermissions);

                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        // Synchronizing
        public static FolderGraph GetFolderGraph(int packageId, string path)
        {
            OS.OperatingSystem os = GetOS(packageId);
            string fullPath = GetFullPackagePath(packageId, path);

            // get graph
            return os.GetFolderGraph(fullPath);
        }

        public static void ExecuteSyncActions(int packageId, FileSyncAction[] actions)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return;

            OS.OperatingSystem os = GetOS(packageId);

            // update actions
            foreach (FileSyncAction action in actions)
            {
                if (!String.IsNullOrEmpty(action.SrcPath))
                    action.SrcPath = GetFullPackagePath(packageId, action.SrcPath);
                if (!String.IsNullOrEmpty(action.DestPath))
                    action.DestPath = GetFullPackagePath(packageId, action.DestPath);
            }

            // perform sync
            os.ExecuteSyncActions(actions);
        }

		public static string ConvertToUncPath(int serviceId, string path)
		{
			// load web service info
			ServiceInfo svc = ServerController.GetServiceInfo(serviceId);
			// load web server info
			ServerInfo srv = ServerController.GetServerByIdInternal(svc.ServerId);

			return "\\\\" + srv.ServerName + "\\" + path.Replace(":", "$");
		}

        private static UserPermission[] GetAvailableSecurityAccounts(int packageId)
        {
            List<UserPermission> users = new List<UserPermission>();

            // all web sites
            List<WebSite> sites = WebServerController.GetWebSites(packageId, false);
            int webServiceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);
            if (webServiceId > 0)
            {
                List<string> siteIds = new List<string>();
                foreach (WebSite site in sites)
                    siteIds.Add(site.SiteId);

                WebServer web = WebServerController.GetWebServer(webServiceId);
                string[] siteAccounts = web.GetSitesAccounts(siteIds.ToArray());

                for (int i = 0; i < sites.Count; i++)
                {
                    UserPermission user = new UserPermission();
                    user.DisplayName = sites[i].Name;
                    user.AccountName = siteAccounts[i];
                    users.Add(user);
                }
            }

            // add "network service"
            UserPermission ns = new UserPermission();
            ns.DisplayName = "NETWORK SERVICE";
            ns.AccountName = "NETWORK SERVICE";
            users.Add(ns);

            return users.ToArray();
        }

        public static int SetFolderQuota(int packageId, string path, string driveName, string quotas)
        {

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("FILES", "SET_QUOTA_ON_FOLDER", path, packageId);

            try
            {

                // disk space quota
                // This gets all the disk space allocated for a specific customer
                // It includes the package Add Ons * Quatity + Hosting Plan System disk space value. //Quotas.OS_DISKSPACE
                QuotaValueInfo diskSpaceQuota = PackageController.GetPackageQuota(packageId, quotas);


                #region figure Quota Unit

                // Quota Unit
                string unit = String.Empty;
                if (diskSpaceQuota.QuotaDescription.ToLower().Contains("gb"))
                    unit = "GB";
                else if (diskSpaceQuota.QuotaDescription.ToLower().Contains("mb"))
                    unit = "MB";
                else
                    unit = "KB";

                #endregion

                OS.OperatingSystem os = GetOS(packageId);

                os.SetQuotaLimitOnFolder(path, driveName, QuotaType.Hard, diskSpaceQuota.QuotaAllocatedValue.ToString() + unit, 0, String.Empty, String.Empty);

                return 0;
            }
            catch (Exception ex)
            {
                //Log and return a generic error rather than throwing an exception
                TaskManager.WriteError(ex);
                return BusinessErrorCodes.ERROR_FILE_GENERIC_LOGGED;
            }
            finally
            {
                TaskManager.CompleteTask();
            }


        }
        
        public static int ApplyEnableHardQuotaFeature(int packageId)
        {
            if (SecurityContext.CheckAccount(DemandAccount.IsActive | DemandAccount.IsAdmin | DemandAccount.NotDemo) != 0)
                throw new Exception("This method could be called by serveradmin only.");

            // place log record
            TaskManager.StartTask("FILES", "APPLY_ENABLEHARDQUOTAFEATURE");

            try
            {

                // request OS service
                //int osId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Os);
                //if (osId == 0)
                //    return -1;

                //OS.OperatingSystem os = new OS.OperatingSystem();
                //ServiceProviderProxy.Init(os, osId);

                ////Get operating system settings
                // StringDictionary osSesstings = ServerController.GetServiceSettings(osId);
                //  bool diskQuotaEnabled = (osSesstings["EnableHardQuota"] != null) ? bool.Parse(osSesstings["EnableHardQuota"]) : false;
                //string driveName = osSesstings["LocationDrive"];

                //if (!diskQuotaEnabled)
                //    return -1;


                List<PackageInfo> allPackages = PackageController.GetPackagePackages(packageId, true);

                foreach (PackageInfo childPackage in allPackages)
                {
                    // request OS service
                    int osId = PackageController.GetPackageServiceId(childPackage.PackageId, ResourceGroups.Os);
                    if (osId == 0)
                        continue;

                    OS.OperatingSystem os = new OS.OperatingSystem();
                    ServiceProviderProxy.Init(os, osId);

                    //Get operating system settings
                    StringDictionary osSesstings = ServerController.GetServiceSettings(osId);
                    string driveName = osSesstings["LocationDrive"];

                    if (String.IsNullOrEmpty(driveName))
                        continue;

                    string homeFolder = FilesController.GetHomeFolder(childPackage.PackageId);
                    FilesController.SetFolderQuota(childPackage.PackageId, homeFolder, driveName, Quotas.OS_DISKSPACE);
                }
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }

            return 0;
        }

        public static int DeleteDirectoryRecursive(int packageId, string rootPath)
        {

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // place log record
            TaskManager.StartTask("FILES", "DELETE_DIRECTORY_RECURSIVE", rootPath, packageId);

            try
            {

                OS.OperatingSystem os = GetOS(packageId);
                os.DeleteDirectoryRecursive(rootPath);

                return 0;
            }
            catch (Exception ex)
            {
                //Log and return a generic error rather than throwing an exception
                TaskManager.WriteError(ex);
                return BusinessErrorCodes.ERROR_FILE_GENERIC_LOGGED;
            }
            finally
            {
                TaskManager.CompleteTask();
            }


        }
    }
}
