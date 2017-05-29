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
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;

using SolidCP.Providers.SharePoint;
using SolidCP.Providers.Utils;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SolidCP.Providers.HostedSolution
{
    /// <summary>
    /// Represents SharePoint management functionality implementation.
    /// </summary>
    public class HostedSharePointServerImpl : MarshalByRefObject
    {
        /// <summary>
        /// Gets list of supported languages by this installation of SharePoint.
        /// </summary>
        /// <returns>List of supported languages</returns>
        public int[] GetSupportedLanguages(string languagePacksPath)
        {
            List<int> languages = new List<int>();

            try
            {
                WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

                try
                {
                    SPLanguageCollection installedLanguages = SPRegionalSettings.GlobalInstalledLanguages;

                    foreach (SPLanguage lang in installedLanguages)
                    {
                        languages.Add(lang.LCID);
                    }

                    return languages.ToArray();

                }
                finally
                {
                    wic.Undo();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create site collection.", ex);
            }
        }

        public long GetSiteCollectionSize(Uri root, string url)
        {
            WindowsImpersonationContext wic = null;

            try
            {
                wic = WindowsIdentity.GetCurrent().Impersonate();

                SPWebApplication rootWebApplication = SPWebApplication.Lookup(root);
                SPSite site = rootWebApplication.Sites[url];
                if (site != null)
                    site.RecalculateStorageUsed();
                else
                    throw new ApplicationException(string.Format("SiteCollection {0} does not exist", url));

                return site.Usage.Storage;
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(ex);
                throw;
            }
            finally
            {
                if (wic != null)
                    wic.Undo();
            }

        }

        public SharePointSiteDiskSpace[] CalculateSiteCollectionDiskSpace(Uri root, string[] urls)
        {
            WindowsImpersonationContext wic = null;

            try
            {
                wic = WindowsIdentity.GetCurrent().Impersonate();

                SPWebApplication rootWebApplication = SPWebApplication.Lookup(root);

                List<SharePointSiteDiskSpace> ret = new List<SharePointSiteDiskSpace>();
                foreach (string url in urls)
                {
                    SharePointSiteDiskSpace siteDiskSpace = new SharePointSiteDiskSpace();
                    rootWebApplication.Sites[url].RecalculateStorageUsed();
                    siteDiskSpace.Url = url;
                    siteDiskSpace.DiskSpace = (long)Math.Round(rootWebApplication.Sites[url].Usage.Storage / 1024.0 / 1024.0);
                    ret.Add(siteDiskSpace);
                }
                return ret.ToArray();
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(ex);
                throw;
            }
            finally
            {
                if (wic != null)
                    wic.Undo();
            }

        }


        /// <summary>
        /// Gets list of SharePoint collections within root web application.
        /// </summary>
        /// <param name="rootWebApplicationUri">Root web application uri.</param>
        /// <returns>List of SharePoint collections within root web application.</returns>
        public SharePointSiteCollection[] GetSiteCollections(Uri rootWebApplicationUri)
        {
            try
            {
                WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

                try
                {
                    SPWebApplication rootWebApplication = SPWebApplication.Lookup(rootWebApplicationUri);

                    List<SharePointSiteCollection> siteCollections = new List<SharePointSiteCollection>();

                    foreach (SPSite site in rootWebApplication.Sites)
                    {
                        SharePointSiteCollection loadedSiteCollection = new SharePointSiteCollection();
                        FillSiteCollection(loadedSiteCollection, site);
                        siteCollections.Add(loadedSiteCollection);
                    }

                    return siteCollections.ToArray();
                }
                finally
                {
                    wic.Undo();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create site collection.", ex);
            }
        }

        /// <summary>
        /// Gets SharePoint collection within root web application with given name.
        /// </summary>
        /// <param name="rootWebApplicationUri">Root web application uri.</param>
        /// <param name="url">Url that uniquely identifies site collection to be loaded.</param>
        /// <returns>SharePoint collection within root web application with given name.</returns>
        public SharePointSiteCollection GetSiteCollection(Uri rootWebApplicationUri, string url)
        {
            try
            {
                WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

                try
                {
                    SPWebApplication rootWebApplication = SPWebApplication.Lookup(rootWebApplicationUri);
                    string siteCollectionUrl = String.Format("{0}:{1}", url, rootWebApplicationUri.Port);

                    SPSite site = rootWebApplication.Sites[siteCollectionUrl];
                    if (site != null)
                    {
                        SharePointSiteCollection loadedSiteCollection = new SharePointSiteCollection();
                        FillSiteCollection(loadedSiteCollection, site);
                        return loadedSiteCollection;
                    }
                    return null;
                }
                finally
                {
                    wic.Undo();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create site collection.", ex);
            }
        }

        private static void DeleteQuotaTemplate(string name)
        {
            SPFarm farm = SPFarm.Local;

            SPWebService webService = farm.Services.GetValue<SPWebService>("");
            SPQuotaTemplateCollection quotaColl = webService.QuotaTemplates;
            quotaColl.Delete(name);
        }


        public void UpdateQuotas(Uri root, string url, long maxStorage, long warningStorage)
        {
            WindowsImpersonationContext wic = null;

            try
            {
                wic = WindowsIdentity.GetCurrent().Impersonate();

                SPWebApplication rootWebApplication = SPWebApplication.Lookup(root);

                SPQuota quota = new SPQuota();
                if (maxStorage != -1)
                    quota.StorageMaximumLevel = maxStorage * 1024 * 1024;
                else
                    quota.StorageMaximumLevel = 0;


                if (warningStorage != -1 && maxStorage != -1)
                    quota.StorageWarningLevel = Math.Min(warningStorage, maxStorage) * 1024 * 1024;
                else
                    quota.StorageWarningLevel = 0;

                rootWebApplication.GrantAccessToProcessIdentity(WindowsIdentity.GetCurrent().Name);
                rootWebApplication.Sites[url].Quota = quota;

            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(ex);
                throw;
            }
            finally
            {
                if (wic != null)
                    wic.Undo();
            }

        }

        /// <summary>
        /// Creates site collection within predefined root web application.
        /// </summary>
        /// <param name="rootWebApplicationUri">Root web application uri.</param>
        /// <param name="siteCollection">Information about site coolection to be created.</param>
        /// <exception cref="InvalidOperationException">Is thrown in case requested operation fails for any reason.</exception>
        public void CreateSiteCollection(Uri rootWebApplicationUri, SharePointSiteCollection siteCollection)
        {
            WindowsImpersonationContext wic = null;
            HostedSolutionLog.LogStart("CreateSiteCollection");

            try
            {
                wic = WindowsIdentity.GetCurrent().Impersonate();
                SPWebApplication rootWebApplication = SPWebApplication.Lookup(rootWebApplicationUri);
                string siteCollectionUrl = String.Format("{0}:{1}", siteCollection.Url, rootWebApplicationUri.Port);

                HostedSolutionLog.DebugInfo("rootWebApplicationUri: {0}", rootWebApplicationUri);
                HostedSolutionLog.DebugInfo("siteCollectionUrl: {0}", siteCollectionUrl);

                SPQuota spQuota;

                SPSite spSite = rootWebApplication.Sites.Add(siteCollectionUrl,
                                                             siteCollection.Title, siteCollection.Description,
                                                             (uint)siteCollection.LocaleId, String.Empty,
                                                             siteCollection.OwnerLogin, siteCollection.OwnerName,
                                                             siteCollection.OwnerEmail,
                                                             null, null, null, true);

                try
                {

                    spQuota = new SPQuota();

                    if (siteCollection.MaxSiteStorage != -1)
                        spQuota.StorageMaximumLevel = siteCollection.MaxSiteStorage * 1024 * 1024;

                    if (siteCollection.WarningStorage != -1 && siteCollection.MaxSiteStorage != -1)
                        spQuota.StorageWarningLevel = Math.Min(siteCollection.WarningStorage, siteCollection.MaxSiteStorage) * 1024 * 1024;

                }
                catch (Exception)
                {
                    rootWebApplication.Sites.Delete(siteCollectionUrl);
                    throw;
                }

                try
                {
                    rootWebApplication.GrantAccessToProcessIdentity(WindowsIdentity.GetCurrent().Name);
                    spSite.Quota = spQuota;
                }
                catch (Exception)
                {
                    rootWebApplication.Sites.Delete(siteCollectionUrl);
                    DeleteQuotaTemplate(siteCollection.Title);
                    throw;
                }

                rootWebApplication.Update(true);

                try
                {
                    if (siteCollection.RootWebApplicationInteralIpAddress != string.Empty)
                    {
                        string dirPath = FileUtils.EvaluateSystemVariables(@"%windir%\system32\drivers\etc");
                        string path = dirPath + "\\hosts";

                        if (FileUtils.FileExists(path))
                        {
                            string content = FileUtils.GetFileTextContent(path);
                            content = content.Replace("\r\n", "\n").Replace("\n\r", "\n");
                            string[] contentArr = content.Split(new char[] { '\n' });
                            bool bRecordExist = false;
                            foreach (string s in contentArr)
                            {
                                if (s != string.Empty)
                                {
                                    string IPAddr = string.Empty;
                                    string hostName = string.Empty;
                                    if (s[0] != '#')
                                    {
                                        bool bSeperator = false;
                                        foreach (char c in s)
                                        {
                                            if ((c != ' ') & (c != '\t'))
                                            {
                                                if (bSeperator)
                                                    hostName += c;
                                                else
                                                    IPAddr += c;
                                            }
                                            else
                                                bSeperator = true;
                                        }

                                        if (hostName.ToLower() == siteCollection.RootWebApplicationFQDN.ToLower())
                                        {
                                            bRecordExist = true;
                                            break;
                                        }

                                    }
                                }
                            }

                            if (!bRecordExist)
                            {
                                string outPut = string.Empty;
                                foreach (string o in contentArr)
                                {
                                    if (o != string.Empty)
                                        outPut += o + "\r\n";
                                }

                                outPut += siteCollection.RootWebApplicationInteralIpAddress + '\t' + siteCollection.RootWebApplicationFQDN + "\r\n";

                                FileUtils.UpdateFileTextContent(path, outPut);
                            }


                        }
                    }
                }
                catch (Exception ex)
                {
                    HostedSolutionLog.LogError(ex);

                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(ex);
                throw;
            }
            finally
            {
                if (wic != null)
                    wic.Undo();

                HostedSolutionLog.LogEnd("CreateSiteCollection");
            }
        }

        /// <summary>
        /// Deletes site collection under given url.
        /// </summary>
        /// <param name="rootWebApplicationUri">Root web application uri.</param>
        /// <param name="url">Url that uniquely identifies site collection to be deleted.</param>
        /// <exception cref="InvalidOperationException">Is thrown in case requested operation fails for any reason.</exception>
        public void DeleteSiteCollection(Uri rootWebApplicationUri, SharePointSiteCollection siteCollection)
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsImpersonationContext wic = identity.Impersonate();

                try
                {
                    SPWebApplication rootWebApplication = SPWebApplication.Lookup(rootWebApplicationUri);
                    string siteCollectionUrl = String.Format("{0}:{1}", siteCollection.Url, rootWebApplicationUri.Port);

                    //string args = String.Format("-o deletesite -url {0}", siteCollectionUrl);
                    //string stsadm = @"c:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN\STSADM.EXE";

                    //// launch system process
                    //ProcessStartInfo startInfo = new ProcessStartInfo(stsadm, args);
                    //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //startInfo.RedirectStandardOutput = true;
                    //startInfo.UseShellExecute = false;
                    //Process proc = Process.Start(startInfo);

                    //// analyze results
                    //StreamReader reader = proc.StandardOutput;
                    //string output = reader.ReadToEnd();
                    //int exitCode = proc.ExitCode;
                    //reader.Close();


                    rootWebApplication.Sites.Delete(siteCollectionUrl, true);
                    rootWebApplication.Update(true);

                    try
                    {
                        if (siteCollection.RootWebApplicationInteralIpAddress != string.Empty)
                        {
                            string dirPath = FileUtils.EvaluateSystemVariables(@"%windir%\system32\drivers\etc");
                            string path = dirPath + "\\hosts";

                            if (FileUtils.FileExists(path))
                            {
                                string content = FileUtils.GetFileTextContent(path);
                                content = content.Replace("\r\n", "\n").Replace("\n\r", "\n");
                                string[] contentArr = content.Split(new char[] { '\n' });
                                string outPut = string.Empty;
                                foreach (string s in contentArr)
                                {
                                    if (s != string.Empty)
                                    {
                                        string IPAddr = string.Empty;
                                        string hostName = string.Empty;
                                        if (s[0] != '#')
                                        {
                                            bool bSeperator = false;
                                            foreach (char c in s)
                                            {
                                                if ((c != ' ') & (c != '\t'))
                                                {
                                                    if (bSeperator)
                                                        hostName += c;
                                                    else
                                                        IPAddr += c;
                                                }
                                                else
                                                    bSeperator = true;
                                            }

                                            if (hostName.ToLower() != siteCollection.RootWebApplicationFQDN.ToLower())
                                            {
                                                outPut += s + "\r\n";
                                            }

                                        }
                                        else
                                            outPut += s + "\r\n";
                                    }
                                }

                                FileUtils.UpdateFileTextContent(path, outPut);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        HostedSolutionLog.LogError(ex);

                    }


                }
                finally
                {
                    wic.Undo();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete site collection.", ex);
            }
        }

        /// <summary>
        /// Backups site collection under give url.
        /// </summary>
        /// <param name="rootWebApplicationUri">Root web application uri.</param>
        /// <param name="url">Url that uniquely identifies site collection to be deleted.</param>
        /// <param name="filename">Resulting backup file name.</param>
        /// <param name="zip">A value which shows whether created backup must be archived.</param>
        /// <param name="tempPath">Custom temp path for backup</param>
        /// <returns>Full path to created backup.</returns>
        /// <exception cref="InvalidOperationException">Is thrown in case requested operation fails for any reason.</exception>
        public string BackupSiteCollection(Uri rootWebApplicationUri, string url, string filename, bool zip, string tempPath)
        {
            try
            {
                WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

                try
                {
                    SPWebApplication rootWebApplication = SPWebApplication.Lookup(rootWebApplicationUri);
                    string siteCollectionUrl = String.Format("{0}:{1}", url, rootWebApplicationUri.Port);

                    if (String.IsNullOrEmpty(tempPath))
                    {
                        tempPath = Path.GetTempPath();
                    }
                    string backupFileName = Path.Combine(tempPath, (zip ? StringUtils.CleanIdentifier(siteCollectionUrl) + ".bsh" : StringUtils.CleanIdentifier(filename)));
                    // Backup requested site.
                    rootWebApplication.Sites.Backup(siteCollectionUrl, backupFileName, true);

                    if (zip)
                    {
                        string zipFile = Path.Combine(tempPath, filename);
                        string zipRoot = Path.GetDirectoryName(backupFileName);

                        FileUtils.ZipFiles(zipFile, zipRoot, new string[] { Path.GetFileName(backupFileName) });
                        FileUtils.DeleteFile(backupFileName);

                        backupFileName = zipFile;
                    }
                    return backupFileName;
                }
                finally
                {
                    wic.Undo();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to backup site collection.", ex);
            }
        }

        /// <summary>
        /// Restores site collection under given url from backup.
        /// </summary>
        /// <param name="rootWebApplicationUri">Root web application uri.</param>
        /// <param name="siteCollection">Site collection to be restored.</param>
        /// <param name="filename">Backup file name to restore from.</param>
        /// <exception cref="InvalidOperationException">Is thrown in case requested operation fails for any reason.</exception>
        public void RestoreSiteCollection(Uri rootWebApplicationUri, SharePointSiteCollection siteCollection, string filename)
        {
            string url = siteCollection.Url;
            try
            {

                WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

                try
                {
                    SPWebApplication rootWebApplication = SPWebApplication.Lookup(rootWebApplicationUri);
                    string siteCollectionUrl = String.Format("{0}:{1}", url, rootWebApplicationUri.Port);

                    string tempPath = Path.GetTempPath();
                    // Unzip uploaded files if required.
                    string expandedFile = filename;
                    if (Path.GetExtension(filename).ToLower() == ".zip")
                    {
                        // Unpack file.
                        expandedFile = FileUtils.UnzipFiles(filename, tempPath)[0];

                        // Delete zip archive.
                        FileUtils.DeleteFile(filename);
                    }

                    // Delete existent site and restore new one.
                    rootWebApplication.Sites.Delete(siteCollectionUrl, false);
                    rootWebApplication.Sites.Restore(siteCollectionUrl, expandedFile, true, true);

                    SPSite restoredSite = rootWebApplication.Sites[siteCollectionUrl];
                    SPWeb web = restoredSite.OpenWeb();

                    SPUser owner = null;
                    try
                    {
                        owner = web.SiteUsers[siteCollection.OwnerLogin];
                    }
                    catch
                    {
                        // Ignore this error.
                    }
                    if (owner == null)
                    {
                        web.SiteUsers.Add(siteCollection.OwnerLogin, siteCollection.OwnerEmail, siteCollection.OwnerName, String.Empty);
                        owner = web.SiteUsers[siteCollection.OwnerLogin];
                    }

                    restoredSite.Owner = owner;
                    web.Close();

                    rootWebApplication.Update();

                    // Delete expanded file.
                    FileUtils.DeleteFile(expandedFile);
                }
                finally
                {
                    wic.Undo();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to restore site collection.", ex);
            }
        }


        /// <summary>
        /// Fills custom site collection with information from administration object.
        /// </summary>
        /// <param name="customSiteCollection">Custom site collection to fill.</param>
        /// <param name="site">Administration object.</param>
        private static void FillSiteCollection(SharePointSiteCollection customSiteCollection, SPSite site)
        {
            Uri siteUri = new Uri(site.Url);
            string url = (siteUri.Port > 0) ? site.Url.Replace(String.Format(":{0}", siteUri.Port), String.Empty) : site.Url;

            customSiteCollection.Url = url;
            customSiteCollection.OwnerLogin = site.Owner.LoginName;
            customSiteCollection.OwnerName = site.Owner.Name;
            customSiteCollection.OwnerEmail = site.Owner.Email;
            customSiteCollection.LocaleId = site.RootWeb.Locale.LCID;
            customSiteCollection.Title = site.RootWeb.Title;
            customSiteCollection.Description = site.RootWeb.Description;
            customSiteCollection.Bandwidth = site.Usage.Bandwidth;
            customSiteCollection.Diskspace = site.Usage.Storage;
            customSiteCollection.MaxSiteStorage = site.Quota.StorageMaximumLevel;
            customSiteCollection.WarningStorage = site.Quota.StorageWarningLevel;
        }
    }
}









