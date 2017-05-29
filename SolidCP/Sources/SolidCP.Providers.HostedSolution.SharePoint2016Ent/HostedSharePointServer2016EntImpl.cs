// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
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
// - Neither  the  name  of  SolidCP  nor   the   names  of  its
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security.Principal;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SolidCP.Providers.SharePoint;
using SolidCP.Providers.Utils;

namespace SolidCP.Providers.HostedSolution
{
    public class HostedSharePointServer2016EntImpl : MarshalByRefObject
    {
        #region Fields

        private static RunspaceConfiguration runspaceConfiguration;

        #endregion

        #region Properties

        private string SharepointSnapInName
        {
            get { return "Microsoft.SharePoint.Powershell"; }
        }

        #endregion

        #region Methods

        /// <summary>Gets list of SharePoint collections within root web application.</summary>
        /// <param name="rootWebApplicationUri"> The root web application Uri. </param>
        /// <returns>List of SharePoint collections within root web application.</returns>
        public SharePointEnterpriseSiteCollection[] GetSiteCollections(Uri rootWebApplicationUri)
        {
            return GetSPSiteCollections(rootWebApplicationUri).Select(pair => NewSiteCollection(pair.Value)).ToArray();
        }

        /// <summary>Gets list of supported languages by this installation of SharePoint.</summary>
        /// <param name="rootWebApplicationUri"> The root web application Uri. </param>
        /// <returns>List of supported languages</returns>
        public int[] GetSupportedLanguages(Uri rootWebApplicationUri)
        {
            var languages = new List<int>();

            try
            {
                WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

                try
                {
                    languages.AddRange(from SPLanguage lang in SPRegionalSettings.GlobalInstalledLanguages select lang.LCID);                    
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

            return languages.ToArray();
        }

        /// <summary>Gets site collection size in bytes.</summary>
        /// <param name="rootWebApplicationUri">The root web application uri.</param>
        /// <param name="url">The site collection url.</param>
        /// <returns>Size in bytes.</returns>
        public long GetSiteCollectionSize(Uri rootWebApplicationUri, string url)
        {
            Dictionary<string, long> sizes = GetSitesCollectionSize(rootWebApplicationUri, new[] {url});

            if (sizes.Count() == 1)
            {
                return sizes.First().Value;
            }

            throw new ApplicationException(string.Format("SiteCollection {0} does not exist", url));
        }

        /// <summary>Gets sites disk space.</summary>
        /// <param name="rootWebApplicationUri">The root web application uri.</param>
        /// <param name="urls">The sites urls.</param>
        /// <returns>The disk space.</returns>
        public SharePointSiteDiskSpace[] CalculateSiteCollectionDiskSpace(Uri rootWebApplicationUri, string[] urls)
        {
            return GetSitesCollectionSize(rootWebApplicationUri, urls).Select(pair => new SharePointSiteDiskSpace {Url = pair.Key, DiskSpace = (long) Math.Round(pair.Value/1024.0/1024.0)}).ToArray();
        }

        /// <summary>Calculates size of the required seti collections.</summary>
        /// <param name="rootWebApplicationUri">The root web application uri.</param>
        /// <param name="urls">The sites urls.</param>
        /// <returns>Calculated sizes.</returns>
        private Dictionary<string, long> GetSitesCollectionSize(Uri rootWebApplicationUri, IEnumerable<string> urls)
        {
            Runspace runspace = null;
            var result = new Dictionary<string, long>();

            try
            {
                runspace = OpenRunspace();

                foreach (string url in urls)
                {
                    string siteCollectionUrl = String.Format("{0}:{1}", url, rootWebApplicationUri.Port);
                    var scripts = new List<string> {string.Format("$site=Get-SPSite -Identity \"{0}\"", siteCollectionUrl), "$site.RecalculateStorageUsed()", "$site.Usage.Storage"};
                    Collection<PSObject> scriptResult = ExecuteShellCommand(runspace, scripts);

                    if (scriptResult != null && scriptResult.Any())
                    {
                        result.Add(url, Convert.ToInt64(scriptResult.First().BaseObject));
                    }
                }
            }
            finally
            {
                CloseRunspace(runspace);
            }

            return result;
        }

        /// <summary>Sets people picker OU.</summary>
        /// <param name="site">The site.</param>
        /// <param name="ou">OU.</param>
        public void SetPeoplePickerOu(string site, string ou)
        {
            HostedSolutionLog.LogStart("SetPeoplePickerOu");
            HostedSolutionLog.LogInfo("  Site: {0}", site);
            HostedSolutionLog.LogInfo("  OU: {0}", ou);

            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();
                var cmd = new Command("Set-SPSite");
                cmd.Parameters.Add("Identity", site);
                cmd.Parameters.Add("UserAccountDirectoryPath", ou);
                ExecuteShellCommand(runspace, cmd);
            }
            finally
            {
                CloseRunspace(runspace);
            }

            HostedSolutionLog.LogEnd("SetPeoplePickerOu");
        }

        /// <summary>Gets SharePoint collection within root web application with given name.</summary>
        /// <param name="rootWebApplicationUri">Root web application uri.</param>
        /// <param name="url">Url that uniquely identifies site collection to be loaded.</param>
        /// <returns>SharePoint collection within root web application with given name.</returns>
        public SharePointEnterpriseSiteCollection GetSiteCollection(Uri rootWebApplicationUri, string url)
        {
            return NewSiteCollection(GetSPSiteCollection(rootWebApplicationUri, url));
        }

        /// <summary>Deletes quota.</summary>
        /// <param name="name">The quota name.</param>
        private static void DeleteQuotaTemplate(string name)
        {
            SPFarm farm = SPFarm.Local;

            var webService = farm.Services.GetValue<SPWebService>("");
            SPQuotaTemplateCollection quotaColl = webService.QuotaTemplates;
            quotaColl.Delete(name);
        }

        /// <summary>Updates site collection quota.</summary>
        /// <param name="root">The root uri.</param>
        /// <param name="url">The site collection url.</param>
        /// <param name="maxStorage">The max storage.</param>
        /// <param name="warningStorage">The warning storage value.</param>
        public void UpdateQuotas(Uri root, string url, long maxStorage, long warningStorage)
        {
            if (maxStorage != -1)
            {
                maxStorage = maxStorage*1024*1024;
            }
            else
            {
                maxStorage = 0;
            }

            if (warningStorage != -1 && maxStorage != -1)
            {
                warningStorage = Math.Min(warningStorage, maxStorage)*1024*1024;
            }
            else
            {
                warningStorage = 0;
            }

            Runspace runspace = null;

            try
            {
                runspace = OpenRunspace();
                GrantAccess(runspace, root);
                string siteCollectionUrl = String.Format("{0}:{1}", url, root.Port);
                var command = new Command("Set-SPSite");
                command.Parameters.Add("Identity", siteCollectionUrl);
                command.Parameters.Add("MaxSize", maxStorage);
                command.Parameters.Add("WarningSize", warningStorage);
                ExecuteShellCommand(runspace, command);
            }
            finally
            {
                CloseRunspace(runspace);
            }
        }

        /// <summary>Grants acces to current user.</summary>
        /// <param name="runspace">The runspace.</param>
        /// <param name="rootWebApplicationUri">The root web application uri.</param>
        private void GrantAccess(Runspace runspace, Uri rootWebApplicationUri)
        {
            ExecuteShellCommand(runspace, new List<string> {string.Format("$webApp=Get-SPWebApplication {0}", rootWebApplicationUri.AbsoluteUri), string.Format("$webApp.GrantAccessToProcessIdentity(\"{0}\")", WindowsIdentity.GetCurrent().Name)});
        }

        /// <summary>Deletes site collection.</summary>
        /// <param name="runspace">The runspace.</param>
        /// <param name="url">The site collection url.</param>
        /// <param name="deleteADAccounts">True - if active directory accounts should be deleted.</param>
        private void DeleteSiteCollection(Runspace runspace, string url, bool deleteADAccounts)
        {
            var command = new Command("Remove-SPSite");
            command.Parameters.Add("Identity", url);
            command.Parameters.Add("DeleteADAccounts", deleteADAccounts);
            ExecuteShellCommand(runspace, command);
        }

        /// <summary> Creates site collection within predefined root web application.</summary>
        /// <param name="rootWebApplicationUri">Root web application uri.</param>
        /// <param name="siteCollection">Information about site coolection to be created.</param>
        /// <exception cref="InvalidOperationException">Is thrown in case requested operation fails for any reason.</exception>
        public void CreateSiteCollection(Uri rootWebApplicationUri, SharePointEnterpriseSiteCollection siteCollection)
        {
            HostedSolutionLog.LogStart("CreateSiteCollection");
            WindowsImpersonationContext wic = null;
            Runspace runspace = null;

            try
            {
                wic = WindowsIdentity.GetCurrent().Impersonate();
                runspace = OpenRunspace();
                CreateCollection(runspace, rootWebApplicationUri, siteCollection);
            }
            finally
            {
                CloseRunspace(runspace);
                HostedSolutionLog.LogEnd("CreateSiteCollection");

                if (wic != null)
                {
                    wic.Undo();
                }
            }
        }

        /// <summary> Creates site collection within predefined root web application.</summary>
        /// <param name="runspace"> The runspace.</param>
        /// <param name="rootWebApplicationUri">Root web application uri.</param>
        /// <param name="siteCollection">Information about site coolection to be created.</param>
        /// <exception cref="InvalidOperationException">Is thrown in case requested operation fails for any reason.</exception>
        private void CreateCollection(Runspace runspace, Uri rootWebApplicationUri, SharePointEnterpriseSiteCollection siteCollection)
        {
            string siteCollectionUrl = String.Format("{0}:{1}", siteCollection.Url, rootWebApplicationUri.Port);
            HostedSolutionLog.DebugInfo("siteCollectionUrl: {0}", siteCollectionUrl);

            try
            {
                SPWebApplication rootWebApplication = SPWebApplication.Lookup(rootWebApplicationUri);
                rootWebApplication.Sites.Add(siteCollectionUrl, siteCollection.Title, siteCollection.Description, (uint) siteCollection.LocaleId, String.Empty, siteCollection.OwnerLogin, siteCollection.OwnerName, siteCollection.OwnerEmail, null, null, null, true);
                rootWebApplication.Update();
            }
            catch (Exception)
            {
                DeleteSiteCollection(runspace, siteCollectionUrl, true);
                throw;
            }

            try
            {
                GrantAccess(runspace, rootWebApplicationUri);
                var command = new Command("Set-SPSite");
                command.Parameters.Add("Identity", siteCollectionUrl);

                if (siteCollection.MaxSiteStorage != -1)
                {
                    command.Parameters.Add("MaxSize", siteCollection.MaxSiteStorage*1024*1024);
                }

                if (siteCollection.WarningStorage != -1 && siteCollection.MaxSiteStorage != -1)
                {
                    command.Parameters.Add("WarningSize", Math.Min(siteCollection.WarningStorage, siteCollection.MaxSiteStorage)*1024*1024);
                }

                ExecuteShellCommand(runspace, command);
            }
            catch (Exception)
            {
                DeleteQuotaTemplate(siteCollection.Title);
                DeleteSiteCollection(runspace, siteCollectionUrl, true);
                throw;
            }

            AddHostsRecord(siteCollection);
        }

        /// <summary>Deletes site collection under given url.</summary>
        /// <param name="rootWebApplicationUri">Root web application uri.</param>
        /// <param name="siteCollection">The site collection to be deleted.</param>
        /// <exception cref="InvalidOperationException">Is thrown in case requested operation fails for any reason.</exception>
        public void DeleteSiteCollection(Uri rootWebApplicationUri, SharePointEnterpriseSiteCollection siteCollection)
        {
            HostedSolutionLog.LogStart("DeleteSiteCollection");
            Runspace runspace = null;

            try
            {
                string siteCollectionUrl = String.Format("{0}:{1}", siteCollection.Url, rootWebApplicationUri.Port);
                HostedSolutionLog.DebugInfo("siteCollectionUrl: {0}", siteCollectionUrl);
                runspace = OpenRunspace();
                DeleteSiteCollection(runspace, siteCollectionUrl, false);
                RemoveHostsRecord(siteCollection);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to delete site collection.", ex);
            }
            finally
            {
                CloseRunspace(runspace);
                HostedSolutionLog.LogEnd("DeleteSiteCollection");
            }
        }

        /// <summary> Backups site collection under give url.</summary>
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
                string siteCollectionUrl = String.Format("{0}:{1}", url, rootWebApplicationUri.Port);
                HostedSolutionLog.LogStart("BackupSiteCollection");
                HostedSolutionLog.DebugInfo("siteCollectionUrl: {0}", siteCollectionUrl);

                if (String.IsNullOrEmpty(tempPath))
                {
                    tempPath = Path.GetTempPath();
                }

                string backupFileName = Path.Combine(tempPath, (zip ? StringUtils.CleanIdentifier(siteCollectionUrl) + ".bsh" : StringUtils.CleanIdentifier(filename)));
                HostedSolutionLog.DebugInfo("backupFilePath: {0}", backupFileName);
                Runspace runspace = null;

                try
                {
                    runspace = OpenRunspace();
                    var command = new Command("Backup-SPSite");
                    command.Parameters.Add("Identity", siteCollectionUrl);
                    command.Parameters.Add("Path", backupFileName);
                    ExecuteShellCommand(runspace, command);

                    if (zip)
                    {
                        string zipFile = Path.Combine(tempPath, filename);
                        string zipRoot = Path.GetDirectoryName(backupFileName);

                        FileUtils.ZipFiles(zipFile, zipRoot, new[] {Path.GetFileName(backupFileName)});
                        FileUtils.DeleteFile(backupFileName);

                        backupFileName = zipFile;
                    }

                    return backupFileName;
                }
                finally
                {
                    CloseRunspace(runspace);
                    HostedSolutionLog.LogEnd("BackupSiteCollection");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to backup site collection.", ex);
            }
        }

        /// <summary>Restores site collection under given url from backup.</summary>
        /// <param name="rootWebApplicationUri">Root web application uri.</param>
        /// <param name="siteCollection">Site collection to be restored.</param>
        /// <param name="filename">Backup file name to restore from.</param>
        /// <exception cref="InvalidOperationException">Is thrown in case requested operation fails for any reason.</exception>
        public void RestoreSiteCollection(Uri rootWebApplicationUri, SharePointEnterpriseSiteCollection siteCollection, string filename)
        {
            string url = siteCollection.Url;

            try
            {
                string siteCollectionUrl = String.Format("{0}:{1}", url, rootWebApplicationUri.Port);
                HostedSolutionLog.LogStart("RestoreSiteCollection");
                HostedSolutionLog.DebugInfo("siteCollectionUrl: {0}", siteCollectionUrl);

                HostedSolutionLog.DebugInfo("backupFilePath: {0}", filename);
                Runspace runspace = null;

                try
                {
                    string tempPath = Path.GetTempPath();
                    string expandedFile = filename;

                    if (Path.GetExtension(filename).ToLower() == ".zip")
                    {
                        expandedFile = FileUtils.UnzipFiles(filename, tempPath)[0];

                        // Delete zip archive.
                        FileUtils.DeleteFile(filename);
                    }

                    runspace = OpenRunspace();
                    DeleteSiteCollection(runspace, siteCollectionUrl, false);
                    var command = new Command("Restore-SPSite");
                    command.Parameters.Add("Identity", siteCollectionUrl);
                    command.Parameters.Add("Path", filename);
                    ExecuteShellCommand(runspace, command);

                    command = new Command("Set-SPSite");
                    command.Parameters.Add("Identity", siteCollectionUrl);
                    command.Parameters.Add("OwnerAlias", siteCollection.OwnerLogin);
                    ExecuteShellCommand(runspace, command);

                    command = new Command("Set-SPUser");
                    command.Parameters.Add("Identity", siteCollection.OwnerLogin);
                    command.Parameters.Add("Email", siteCollection.OwnerEmail);
                    command.Parameters.Add("DisplayName", siteCollection.Name);
                    ExecuteShellCommand(runspace, command);

                    FileUtils.DeleteFile(expandedFile);
                }
                finally
                {
                    CloseRunspace(runspace);
                    HostedSolutionLog.LogEnd("RestoreSiteCollection");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to restore site collection.", ex);
            }
        }

        /// <summary>Creates new site collection with information from administration object.</summary>
        /// <param name="site">Administration object.</param>
        private static SharePointEnterpriseSiteCollection NewSiteCollection(SPSite site)
        {
            var siteUri = new Uri(site.Url);
            string url = (siteUri.Port > 0) ? site.Url.Replace(String.Format(":{0}", siteUri.Port), String.Empty) : site.Url;

            return new SharePointEnterpriseSiteCollection {Url = url, OwnerLogin = site.Owner.LoginName, OwnerName = site.Owner.Name, OwnerEmail = site.Owner.Email, LocaleId = site.RootWeb.Locale.LCID, Title = site.RootWeb.Title, Description = site.RootWeb.Description, Bandwidth = site.Usage.Bandwidth, Diskspace = site.Usage.Storage, MaxSiteStorage = site.Quota.StorageMaximumLevel, WarningStorage = site.Quota.StorageWarningLevel};
        }

        /// <summary>Gets SharePoint sites collection.</summary>
        /// <param name="rootWebApplicationUri">The root web application uri.</param>
        /// <returns>The SharePoint sites.</returns>
        private Dictionary<string, SPSite> GetSPSiteCollections(Uri rootWebApplicationUri)
        {
            Runspace runspace = null;
            var collections = new Dictionary<string, SPSite>();

            try
            {
                runspace = OpenRunspace();
                var cmd = new Command("Get-SPSite");
                cmd.Parameters.Add("WebApplication", rootWebApplicationUri.AbsoluteUri);
                Collection<PSObject> result = ExecuteShellCommand(runspace, cmd);

                if (result != null)
                {
                    foreach (PSObject psObject in result)
                    {
                        var spSite = psObject.BaseObject as SPSite;

                        if (spSite != null)
                        {
                            collections.Add(spSite.Url, spSite);
                        }
                    }
                }
            }
            finally
            {
                CloseRunspace(runspace);
            }

            return collections;
        }

        /// <summary>Gets SharePoint site collection.</summary>
        /// <param name="rootWebApplicationUri">The root web application uri.</param>
        /// <param name="url">The required site url.</param>
        /// <returns>The SharePoint sites.</returns>
        private SPSite GetSPSiteCollection(Uri rootWebApplicationUri, string url)
        {
            Runspace runspace = null;

            try
            {
                string siteCollectionUrl = String.Format("{0}:{1}", url, rootWebApplicationUri.Port);
                runspace = OpenRunspace();
                var cmd = new Command("Get-SPSite");
                cmd.Parameters.Add("Identity", siteCollectionUrl);
                Collection<PSObject> result = ExecuteShellCommand(runspace, cmd);

                if (result != null && result.Count() == 1)
                {
                    var spSite = result.First().BaseObject as SPSite;

                    if (spSite == null)
                    {
                        throw new ApplicationException(string.Format("SiteCollection {0} does not exist", url));
                    }

                    return result.First().BaseObject as SPSite;
                }
                else
                {
                    throw new ApplicationException(string.Format("SiteCollection {0} does not exist", url));
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError(ex);
                throw;
            }
            finally
            {
                CloseRunspace(runspace);
            }
        }

        /// <summary>Opens PowerShell runspace.</summary>
        /// <returns>The runspace.</returns>
        private Runspace OpenRunspace()
        {
            HostedSolutionLog.LogStart("OpenRunspace");

            if (runspaceConfiguration == null)
            {
                runspaceConfiguration = RunspaceConfiguration.Create();
                PSSnapInException exception;
                runspaceConfiguration.AddPSSnapIn(SharepointSnapInName, out exception);
                HostedSolutionLog.LogInfo("Sharepoint snapin loaded");

                if (exception != null)
                {
                    HostedSolutionLog.LogWarning("SnapIn error", exception);
                }
            }

            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();
            runspace.SessionStateProxy.SetVariable("ConfirmPreference", "none");
            HostedSolutionLog.LogEnd("OpenRunspace");

            return runspace;
        }

        /// <summary>Closes runspace.</summary>
        /// <param name="runspace">The runspace.</param>
        private void CloseRunspace(Runspace runspace)
        {
            try
            {
                if (runspace != null && runspace.RunspaceStateInfo.State == RunspaceState.Opened)
                {
                    runspace.Close();
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.LogError("Runspace error", ex);
            }
        }

        /// <summary>Executes shell command.</summary>
        /// <param name="runspace">The runspace.</param>
        /// <param name="cmd">The command to be executed.</param>        
        /// <returns>PSobjecs collection.</returns>
        private Collection<PSObject> ExecuteShellCommand(Runspace runspace, object cmd)
        {
            object[] errors;
            var command = cmd as Command;

            if (command != null)
            {
                return ExecuteShellCommand(runspace, command, out errors);
            }

            return ExecuteShellCommand(runspace, cmd as List<string>, out errors);
        }

        /// <summary>Executes shell command.</summary>
        /// <param name="runspace">The runspace.</param>
        /// <param name="cmd">The command to be executed.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>PSobjecs collection.</returns>
        private Collection<PSObject> ExecuteShellCommand(Runspace runspace, Command cmd, out object[] errors)
        {
            HostedSolutionLog.LogStart("ExecuteShellCommand");
            var errorList = new List<object>();
            Collection<PSObject> results;

            using (Pipeline pipeLine = runspace.CreatePipeline())
            {
                pipeLine.Commands.Add(cmd);
                results = pipeLine.Invoke();

                if (pipeLine.Error != null && pipeLine.Error.Count > 0)
                {
                    foreach (object item in pipeLine.Error.ReadToEnd())
                    {
                        errorList.Add(item);
                        string errorMessage = string.Format("Invoke error: {0}", item);
                        HostedSolutionLog.LogWarning(errorMessage);
                    }
                }
            }

            errors = errorList.ToArray();
            HostedSolutionLog.LogEnd("ExecuteShellCommand");

            return results;
        }

        /// <summary>Executes shell command.</summary>
        /// <param name="runspace">The runspace.</param>
        /// <param name="scripts">The scripts to be executed.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>PSobjecs collection.</returns>
        private Collection<PSObject> ExecuteShellCommand(Runspace runspace, List<string> scripts, out object[] errors)
        {
            HostedSolutionLog.LogStart("ExecuteShellCommand");
            var errorList = new List<object>();
            Collection<PSObject> results;

            using (Pipeline pipeLine = runspace.CreatePipeline())
            {
                foreach (string script in scripts)
                {
                    pipeLine.Commands.AddScript(script);
                }

                results = pipeLine.Invoke();

                if (pipeLine.Error != null && pipeLine.Error.Count > 0)
                {
                    foreach (object item in pipeLine.Error.ReadToEnd())
                    {
                        errorList.Add(item);
                        string errorMessage = string.Format("Invoke error: {0}", item);
                        HostedSolutionLog.LogWarning(errorMessage);

                        throw new ArgumentException(scripts.First());
                    }
                }
            }

            errors = errorList.ToArray();
            HostedSolutionLog.LogEnd("ExecuteShellCommand");

            return results;
        }

        /// <summary>Adds record to hosts file.</summary>
        /// <param name="siteCollection">The site collection object.</param>
        public void AddHostsRecord(SharePointEnterpriseSiteCollection siteCollection)
        {
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
                        string[] contentArr = content.Split(new[] {'\n'});
                        bool bRecordExist = false;

                        foreach (string s in contentArr)
                        {
                            if (s != string.Empty)
                            {
                                string hostName = string.Empty;

                                if (s[0] != '#')
                                {
                                    bool bSeperator = false;

                                    foreach (char c in s)
                                    {
                                        if ((c != ' ') & (c != '\t'))
                                        {
                                            if (bSeperator)
                                            {
                                                hostName += c;
                                            }
                                        }
                                        else
                                        {
                                            bSeperator = true;
                                        }
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
                            string outPut = contentArr.Where(o => o != string.Empty).Aggregate(string.Empty, (current, o) => current + (o + "\r\n"));
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

        /// <summary>Removes record from hosts file.</summary>
        /// <param name="siteCollection">The site collection object.</param>
        private void RemoveHostsRecord(SharePointEnterpriseSiteCollection siteCollection)
        {
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
                        string[] contentArr = content.Split(new[] {'\n'});
                        string outPut = string.Empty;

                        foreach (string s in contentArr)
                        {
                            if (s != string.Empty)
                            {
                                string hostName = string.Empty;

                                if (s[0] != '#')
                                {
                                    bool bSeperator = false;

                                    foreach (char c in s)
                                    {
                                        if ((c != ' ') & (c != '\t'))
                                        {
                                            if (bSeperator)
                                            {
                                                hostName += c;
                                            }
                                        }
                                        else
                                        {
                                            bSeperator = true;
                                        }
                                    }

                                    if (hostName.ToLower() != siteCollection.RootWebApplicationFQDN.ToLower())
                                    {
                                        outPut += s + "\r\n";
                                    }
                                }
                                else
                                {
                                    outPut += s + "\r\n";
                                }
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

        #endregion
    }
}
