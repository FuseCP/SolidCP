using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.Search.Interop;
using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;
namespace SolidCP.Providers.StorageSpaces
{
    public class Windows2012 : SolidCP.Providers.OS.Windows2012, IStorageSpace
    {
        #region Properties
        internal string PrimaryDomainController
        {
            get { return ProviderSettings["PrimaryDomainController"]; }
        }

        internal string WebdavSiteAppPoolIdentity
        {
            get { return ProviderSettings["WebdavSiteAppPoolIdentity"]; }
        }

        #endregion Properties


        public override bool IsInstalled()
        {
            Server.Utils.OS.WindowsVersion version = SolidCP.Server.Utils.OS.GetVersion();
            return version == SolidCP.Server.Utils.OS.WindowsVersion.WindowsServer2012 ||
                   version == SolidCP.Server.Utils.OS.WindowsVersion.WindowsServer2012R2 ||
                   version == SolidCP.Server.Utils.OS.WindowsVersion.WindowsServer2016;
        }

        #region HostingServiceProvider methods

        public override string[] Install()
        {
            List<string> messages = new List<string>();

            try
            {

                if (!CheckFileServicesInstallation())
                {
                    Log.WriteStart(String.Format("Installing FSRM"));
                    InstallFsrmService();
                    Log.WriteEnd(String.Format("Installing FSRM"));
                }
                else
                {
                    Log.WriteInfo(String.Format("FSRM is Already Installed"));
                }

                if (!CheckWindowsFeatureInstallation("Search-Service"))
                {
                    Log.WriteStart(String.Format("Installing Search-Service"));
                    InstallWindwosFeature("Search-Service");
                    Log.WriteEnd(String.Format("Installing Search-Service"));
                }
                else
                {
                    Log.WriteInfo(String.Format("Search-Service is Already Installed"));
                }

            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                messages.Add(String.Format("Error installing Services for Storage Space Service: {0}", ex.Message));

            }

            return messages.ToArray();
        }

        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                try
                {
                    if (item is HomeFolder)
                        // delete home folder
                        FileUtils.DeleteFile(item.Name);
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                }
            }
        }

        public override ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
        {
            List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();
            foreach (ServiceProviderItem item in items)
            {
                if (item is HomeFolder)
                {
                    try
                    {
                        string path = item.Name;

                        Log.WriteStart(String.Format("Calculating '{0}' folder size", path));

                        // calculate disk space
                        ServiceProviderItemDiskSpace diskspace = new ServiceProviderItemDiskSpace();
                        diskspace.ItemId = item.Id;
                        diskspace.DiskSpace = FileUtils.CalculateFolderSize(path);
                        itemsDiskspace.Add(diskspace);

                        Log.WriteEnd(String.Format("Calculating '{0}' folder size", path));
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(ex);
                    }
                }
            }
            return itemsDiskspace.ToArray();
        }

        #endregion

        #region Storage Spaces

        public void UpdateStorageSettings(string fullPath, long qouteSizeBytes, QuotaType type)
        {
            UpdateFolderQuota(fullPath, qouteSizeBytes, type);

            AddPathToSearchIndex(fullPath);
        }

        public void ClearStorageSettings(string fullPath, string uncPath)
        {
            Log.WriteStart("ClearStorageSettings");
            Log.WriteInfo("FolderPath : {0}", fullPath);

            Runspace runSpace = null;

            try
            {
                runSpace = OpenRunspace();

                RemoveOldQuotaOnFolder(runSpace, fullPath);

                if (!string.IsNullOrEmpty(fullPath))
                {
                    RemoveShare(fullPath, runSpace);
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("ClearStorageSettings", ex);
                throw;
            }
            finally
            {
                CloseRunspace(runSpace);
                Log.WriteEnd("ClearStorageSettings");
            }

        }

        #endregion

        #region Storage Space Folders

        public void CreateFolder(string fullPath)
        {
            FileUtils.CreateDirectory(fullPath);
        }

        public void SetFolderNtfsPermissions(string fullPath, UserPermission[] permissions, bool isProtected, bool preserveInheritance)
        {
            Log.WriteStart("SetFolderNtfsPermissions");
            Log.WriteInfo("Full path : {0}", fullPath);

            try
            {
                if (preserveInheritance == false && permissions != null)
                {
                    if (permissions.All(x => !string.Equals(x.AccountName, "Domain Admins", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        permissions = permissions.Concat(new[]
                        {
                            new UserPermission {AccountName = "Domain Admins", Read = true, Write = true}
                        }).ToArray();
                    }

                    if (permissions.All(x => !string.Equals(x.AccountName, "System", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        permissions = permissions.Concat(new[]
                        {
                            new UserPermission {AccountName = "System", Read = true, Write = true}
                        }).ToArray();
                    }
                }

                SecurityUtils.ResetNtfsPermissions(fullPath);

                SecurityUtils.GrantGroupNtfsPermissions(fullPath, permissions, false, new RemoteServerSettings(), null, null, isProtected, preserveInheritance);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                Log.WriteEnd("SetFolderNtfsPermissions");
            }
        }

        public void DeleteFolder(string fullPath)
        {
            Log.WriteStart("DeleteFolder");
            Log.WriteInfo("Folder Path : {0}", fullPath);

            try
            {
                DirectoryInfo treeRoot = new DirectoryInfo(fullPath);

                if (treeRoot.Exists)
                {
                    DirectoryInfo[] dirs = treeRoot.GetDirectories();
                    while (dirs.Length > 0)
                    {
                        foreach (DirectoryInfo dir in dirs)
                        {
                            DeleteFolder(dir.FullName);
                        }

                        dirs = treeRoot.GetDirectories();
                    }

                    // DELETE THE FILES UNDER THE CURRENT ROOT
                    string[] files = Directory.GetFiles(treeRoot.FullName);
                    foreach (string file in files)
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }

                    Directory.Delete(treeRoot.FullName, true);

                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                Log.WriteEnd("DeleteFolder");
            }
        }

        public bool FileOrDirectoryExist(string fullPath)
        {
            Log.WriteStart("FolderExist");
            Log.WriteInfo("Folder Path : {0}", fullPath);

            try
            {
                return (Directory.Exists(fullPath) || File.Exists(fullPath));
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                Log.WriteEnd("FolderExist");
            }
        }

        public bool RenameFolder(string originalPath, string newName)
        {
            Log.WriteStart("RenameFolder");
            Log.WriteInfo("Folder Path : {0}", originalPath);
            Log.WriteInfo("New Name : {0}", newName);

            try
            {
                var newPath = Path.Combine(Directory.GetParent(originalPath).ToString(), newName);

                FileUtils.MoveFile(originalPath, newPath);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                Log.WriteEnd("RenameFolder");
            }

            return true;
        }

        public StorageSpaceFolderShare ShareFolder(string fullPath, string shareName)
        {
            Runspace runspace = null;

            shareName = shareName.Replace(" ", "");

            try
            {
                runspace = OpenRunspace();

                if (ShareExist(shareName, runspace))
                {
                    var share = GetShare(shareName, runspace);

                    if (String.Compare(share.Path, fullPath, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        return share;
                    }

                    //finding first not used share name
                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        var tmpShareName = shareName + i;

                        if (!ShareExist(tmpShareName, runspace))
                        {
                            shareName = tmpShareName;
                            break;
                        }
                    }
                }

                Log.WriteStart("ShareFolder");
                Log.WriteInfo("FolderPath : {0}", fullPath);

                var scripts = new List<string>
                {
                    string.Format("$LocalNetworkServiceName = (Get-WMIObject -Class Win32_Account -Filter \"LocalAccount=TRUE and SID='S-1-5-20'\").name"),
                    string.Format("$LocalEveryoneName = (Get-WMIObject -Class Win32_Account -Filter \"LocalAccount=TRUE and SID='S-1-1-0'\").name"),
                    string.Format("net share {0}=\"{1}\" \"/grant:$LocalNetworkServiceName,full\" /grant:\"$LocalEveryoneName,full\"",shareName, fullPath)
                };

                object[] errors = null;
                var result = ExecuteLocalScript(runspace, scripts, out errors);

                return GetShare(shareName, runspace);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
                Log.WriteEnd("ShareFolder");
            }
        }

        public void RemoveShare(string fullPath)
        {
            RemoveShare(fullPath, null);
        }

        public bool ShareExist(string shareName, Runspace runspace = null)
        {

            Log.WriteStart("ShareExist");
            Log.WriteInfo("Share Name : {0}", shareName);

            var closeRunspace = runspace == null;

            try
            {
                if (runspace == null)
                {
                    runspace = OpenRunspace();
                }

                var cmd = new Command("Get-WmiObject");
                cmd.Parameters.Add("Class", "Win32_Share");
                cmd.Parameters.Add("Filter", string.Format("name='{0}'", shareName));

                var result = ExecuteShellCommand(runspace, cmd, false);

                return result.Any();
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                if (closeRunspace)
                {
                    CloseRunspace(runspace);
                }

                Log.WriteEnd("ShareExist");
            }

            return false;
        }

        public StorageSpaceFolderShare GetShare(string shareName, Runspace runspace = null)
        {
            Log.WriteStart("GetShare");
            Log.WriteInfo("Share Name : {0}", shareName);

            var closeRunspace = runspace == null;

            try
            {
                if (runspace == null)
                {
                    runspace = OpenRunspace();
                }

                var cmd = new Command("Get-WmiObject");
                cmd.Parameters.Add("Class", "Win32_Share");
                cmd.Parameters.Add("Filter", string.Format("name='{0}'", shareName));

                var result = ExecuteShellCommand(runspace, cmd, false);

                if (!result.Any())
                {
                    return null;
                }

                return CreateShareEntity(result[0]);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                if (closeRunspace)
                {
                    CloseRunspace(runspace);
                }

                Log.WriteEnd("GetShare");
            }
        }

        public StorageSpaceFolderShare GetShareByPath(string path, Runspace runspace = null)
        {
            Log.WriteStart("GetShare");
            Log.WriteInfo("Path: {0}", path);

            var closeRunspace = runspace == null;

            try
            {
                if (runspace == null)
                {
                    runspace = OpenRunspace();
                }

                var cmd = new Command("Get-WmiObject");
                cmd.Parameters.Add("Class", "Win32_Share");
                cmd.Parameters.Add("Filter", string.Format("Path='{0}'", path).Replace("\\", "\\\\"));

                var result = ExecuteShellCommand(runspace, cmd, false);

                if (!result.Any())
                {
                    return null;
                }

                return CreateShareEntity(result[0]);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                if (closeRunspace)
                {
                    CloseRunspace(runspace);
                }

                Log.WriteEnd("GetShare");
            }
        }

        public bool RemoveShare(string shareName, Runspace runspace = null)
        {
            Log.WriteStart("RemoveShare");
            Log.WriteInfo("Share Name : {0}", shareName);

            var closeRunspace = runspace == null;

            try
            {
                if (runspace == null)
                {
                    runspace = OpenRunspace();
                }

                var scripts = new List<string>
                {
                    string.Format("net share \"{0}\" /Y /delete", shareName),
                };

                object[] errors = null;
                var result = ExecuteLocalScript(runspace, scripts, out errors);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                if (closeRunspace)
                {
                    CloseRunspace(runspace);
                }

                Log.WriteEnd("RemoveShare");
            }

            return false;
        }

        private StorageSpaceFolderShare CreateShareEntity(PSObject psObject)
        {
            var result = new StorageSpaceFolderShare();

            result.Name = Convert.ToString(GetPSObjectProperty(psObject, "Name"));
            result.Path = Convert.ToString(GetPSObjectProperty(psObject, "Path"));
            result.UncPath = string.Format("\\\\{0}\\{1}", GetFqdn(), result.Name);

            return result;
        }

        public void ShareSetAbeState(string path, bool enabled)
        {
            Log.WriteStart("ShareSetAbeState");
            Log.WriteInfo("Path: {0}", path);

            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();

                var share = GetShareByPath(path, runspace);

                if (share == null)
                {
                    throw new Exception(string.Format("Share by path '{0}' not found", path));
                }

                var cmd = new Command("Set-SmbShare");
                cmd.Parameters.Add("Name", share.Name);
                cmd.Parameters.Add("FolderEnumerationMode", enabled ? 0 : 1);

                ExecuteShellCommand(runspace, cmd, false);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);

                Log.WriteEnd("ShareSetAbeState");
            }
        }

        public bool ShareGetAbeState(string path)
        {
            Log.WriteStart("ShareGetAbeState");
            Log.WriteInfo("Path: {0}", path);

            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();

                var share = GetShareByPath(path, runspace);

                if (share == null)
                {
                    throw new Exception(string.Format("Share by path '{0}' not found", path));
                }

                var cmd = new Command("Get-SmbShare");
                cmd.Parameters.Add("Name", share.Name);

                var result = ExecuteShellCommand(runspace, cmd, false).FirstOrDefault();

                if (result == null)
                {
                    return false;
                }

                return GetPSObjectProperty(result, "FolderEnumerationMode").ToString() == "0";
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);

                Log.WriteEnd("ShareGetAbeState");
            }
        }

        public void ShareSetEncyptDataAccess(string path, bool enabled)
        {
            Log.WriteStart("ShareSetEncyptDataAccess");
            Log.WriteInfo("Path: {0}", path);

            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();

                var share = GetShareByPath(path, runspace);

                if (share == null)
                {
                    throw new Exception(string.Format("Share by path '{0}' not found", path));
                }

                var cmd = new Command("Set-SmbShare");
                cmd.Parameters.Add("Name", share.Name);
                cmd.Parameters.Add("EncryptData", enabled);
                cmd.Parameters.Add("Force", true);

                ExecuteShellCommand(runspace, cmd, false);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);

                Log.WriteEnd("ShareSetEncyptDataAccess");
            }
        }

        public bool ShareGetEncyptDataAccessStatus(string path)
        {
            Log.WriteStart("ShareGetEncyptDataAccessStatus");
            Log.WriteInfo("Path: {0}", path);

            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();

                var share = GetShareByPath(path, runspace);

                if (share == null)
                {
                    throw new Exception(string.Format("Share by path '{0}' not found", path));
                }

                var cmd = new Command("Get-SmbShare");
                cmd.Parameters.Add("Name", share.Name);

                var result = ExecuteShellCommand(runspace, cmd, false).FirstOrDefault();

                if (result == null)
                {
                    return false;
                }

                return (bool)GetPSObjectProperty(result, "EncryptData");
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);

                Log.WriteEnd("ShareGetEncyptDataAccessStatus");
            }
        }

        public SystemFile[] Search(string[] searchPaths, string searchText, bool recursive)
        {
            var result = new List<SystemFile>();

            using (var conn = new OleDbConnection("Provider=Search.CollatorDSO;Extended Properties='Application=Windows';"))
            {
                var wsSql = string.Format(
                        @"SELECT System.FileName, System.DateModified, System.Size, System.Kind, System.ItemPathDisplay, System.ItemType, System.Search.AutoSummary FROM SYSTEMINDEX WHERE System.FileName LIKE '%{0}%' AND ({1})",
                        searchText,
                        string.Join(" OR ", searchPaths.Select(x => string.Format("{0} = '{1}'", recursive ? "SCOPE" : "DIRECTORY", x)).ToArray()));

                conn.Open();

                var cmd = new OleDbCommand(wsSql, conn);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader != null && reader.Read())
                    {
                        var file = new SystemFile { Name = reader[0] as string };

                        file.Changed = file.CreatedDate = reader[1] is DateTime ? (DateTime)reader[1] : new DateTime();
                        file.Size = reader[2] is Decimal ? Convert.ToInt64((Decimal)reader[2]) : 0;

                        var kind = reader[3] is IEnumerable ? ((IEnumerable)reader[3]).Cast<string>().ToList() : null;
                        var itemType = reader[5] as string ?? string.Empty;

                        if (kind != null && kind.Any() && itemType.ToLowerInvariant() != ".zip")
                        {
                            file.IsDirectory = kind.Any(x => x == "folder");
                        }

                        file.FullName = (reader[4] as string ?? string.Empty);

                        file.Summary = SanitizeXmlString(reader[6] as string);

                        result.Add(file);
                    }
                }
            }

            return result.ToArray();
        }

        public void AddPathToSearchIndex(string fullPath)
        {
            Uri path = new Uri(fullPath);

            string indexingPath = path.ToString();

            CSearchManager csm = new CSearchManager();
            CSearchCrawlScopeManager manager = csm.GetCatalog("SystemIndex").GetCrawlScopeManager();

            if (manager.IncludedInCrawlScope(indexingPath) == 0)
            {
                manager.AddUserScopeRule(indexingPath, 1, 1, 0);
                manager.SaveAll();
            }
        }

        #endregion

        public void UpdateFolderQuota(string fullPath, long qouteSizeBytes, QuotaType quotaType)
        {
            var driveLetter = Path.GetPathRoot(fullPath);
            var pathWithoutDriveLetter = fullPath.Replace(driveLetter, string.Empty);
            driveLetter = driveLetter.Replace(":\\", string.Empty);

            SetQuotaLimitOnFolder(pathWithoutDriveLetter, driveLetter, quotaType, (qouteSizeBytes / (1024 * 1024)).ToString() + "MB", 0, String.Empty, String.Empty);
        }

        public Quota GetFolderQuota(string fullPath)
        {
            var quotas = GetQuotasForOrganization(Directory.GetParent(fullPath).ToString(), string.Empty, string.Empty);

            if (quotas.ContainsKey(fullPath) == false)
            {
                return null;
            }

            var quota = quotas[fullPath];

            if (quota != null)
            {
                if (quota.Usage == -1)
                {
                    quota.Usage = FileUtils.BytesToMb(FileUtils.CalculateFolderSize(fullPath));
                }

                quota.DiskFreeSpaceInBytes = FileUtils.GetTotalFreeSpace(Path.GetPathRoot(fullPath));
            }

            return quota;
        }

        public List<SystemFile> GetAllDriveLetters()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            var folders = new List<SystemFile>();

            foreach (var drive in drives)
            {
                var folder = new SystemFile();

                folder.Name = drive.Name;

                folders.Add(folder);
            }

            return folders;
        }

        public List<SystemFile> GetSystemSubFolders(string path)
        {
            DirectoryInfo rootDir = new DirectoryInfo(path);
            DirectoryInfo[] subdirs = rootDir.GetDirectories();

            var folders = new List<SystemFile>();

            foreach (var subdir in subdirs)
            {
                var folder = new SystemFile();

                folder.Name = subdir.FullName;

                folders.Add(folder);
            }

            return folders;
        }

        private string GetFqdn()
        {
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            if (!hostName.EndsWith(domainName))  // if hostname does not already include domain name
            {
                hostName += "." + domainName;   // add the domain name part
            }

            return hostName;                    // return the fully qualified name
        }

        public string SanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                return null;
            }

            var buffer = new StringBuilder(xml.Length);

            foreach (char c in xml.Where(c => IsLegalXmlChar(c)))
            {
                buffer.Append(c);
            }

            return buffer.ToString();
        }

        public bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

        public bool CheckWindowsFeatureInstallation(string featureName)
        {
            bool isInstalled = false;

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Get-WindowsFeature");
                cmd.Parameters.Add("Name", featureName);

                var feature = ExecuteShellCommand(runSpace, cmd, false).FirstOrDefault();

                if (feature != null)
                {
                    isInstalled = (bool)GetPSObjectProperty(feature, "Installed");
                }
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            return isInstalled;
        }


        public bool InstallWindwosFeature(string featureName)
        {
            Log.WriteStart("InstallWindowsFeature  {0}", featureName);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                Command cmd = new Command("Install-WindowsFeature");
                cmd.Parameters.Add("Name", featureName);
                cmd.Parameters.Add("IncludeManagementTools", true);

                ExecuteShellCommand(runSpace, cmd, false);
            }
            catch (Exception ex)
            {
                Log.WriteError(string.Format("InstallWindowsFeature  {0}", featureName), ex);

                return false;
            }
            finally
            {
                Log.WriteEnd("InstallWindowsFeature  {0}", featureName);

                CloseRunspace(runSpace);
            }

            return true;
        }
    }
}