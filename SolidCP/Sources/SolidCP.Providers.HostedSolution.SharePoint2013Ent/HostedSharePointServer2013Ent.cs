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
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Win32;
using SolidCP.Providers.SharePoint;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.HostedSolution
{
    /// <summary>
    /// Provides hosted SharePoint server functionality implementation.
    /// </summary>
    public class HostedSharePointServer2013Ent : HostingServiceProviderBase, IHostedSharePointServerEnt
    {
        #region Delegate

        private delegate TReturn SharePointAction<TReturn>(HostedSharePointServer2013EntImpl impl);

        #endregion

        #region Fields

        protected string LanguagePacksPath;
        protected string Wss3Registry32Key;
        protected string Wss3RegistryKey;

        #endregion

        #region Properties

        public string BackupTemporaryFolder
        {
            get { return ProviderSettings["BackupTemporaryFolder"]; }
        }

        public Uri Enterprise_RootWebApplicationUri
        {
            get { return new Uri(ProviderSettings["RootWebApplicationUri"]); }
        }

        #endregion

        #region Constructor

        public HostedSharePointServer2013Ent()
        {
            Wss3RegistryKey = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\15.0";
            Wss3Registry32Key = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools\Web Server Extensions\15.0";
            LanguagePacksPath = @"%commonprogramfiles%\microsoft shared\Web Server Extensions\15\HCCab\";
        }

        #endregion

        #region Methods

        /// <summary>Gets list of supported languages by this installation of SharePoint.</summary>
        /// <returns>List of supported languages</returns>
        public int[] Enterprise_GetSupportedLanguages()
        {
            var impl = new HostedSharePointServer2013EntImpl();
            return impl.GetSupportedLanguages(Enterprise_RootWebApplicationUri);
        }

        /// <summary>Gets list of SharePoint collections within root web application.</summary>
        /// <returns>List of SharePoint collections within root web application.</returns>
        public SharePointEnterpriseSiteCollection[] Enterprise_GetSiteCollections()
        {
            return ExecuteSharePointAction(impl => impl.GetSiteCollections(Enterprise_RootWebApplicationUri));
        }

        /// <summary>Gets SharePoint collection within root web application with given name.</summary>
        /// <param name="url">Url that uniquely identifies site collection to be loaded.</param>
        /// <returns>SharePoint collection within root web application with given name.</returns>
        public SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(string url)
        {
            return ExecuteSharePointAction(impl => impl.GetSiteCollection(Enterprise_RootWebApplicationUri, url));
        }

        /// <summary>Creates site collection within predefined root web application.</summary>
        /// <param name="siteCollection">Information about site coolection to be created.</param>
        public void Enterprise_CreateSiteCollection(SharePointEnterpriseSiteCollection siteCollection)
        {
            ExecuteSharePointAction<object>(delegate(HostedSharePointServer2013EntImpl impl)
            {
                impl.CreateSiteCollection(Enterprise_RootWebApplicationUri, siteCollection);
                return null;
            });
        }

        /// <summary>Deletes site collection under given url.</summary>
        /// <param name="siteCollection">The site collection to be deleted.</param>
        public void Enterprise_DeleteSiteCollection(SharePointEnterpriseSiteCollection siteCollection)
        {
            ExecuteSharePointAction<object>(delegate(HostedSharePointServer2013EntImpl impl)
            {
                impl.DeleteSiteCollection(Enterprise_RootWebApplicationUri, siteCollection);
                return null;
            });
        }

        /// <summary>Backups site collection under give url.</summary>
        /// <param name="url">Url that uniquely identifies site collection to be deleted.</param>
        /// <param name="filename">Resulting backup file name.</param>
        /// <param name="zip">A value which shows whether created backup must be archived.</param>
        /// <returns>Created backup full path.</returns>
        public string Enterprise_BackupSiteCollection(string url, string filename, bool zip)
        {
            return ExecuteSharePointAction(impl => impl.BackupSiteCollection(Enterprise_RootWebApplicationUri, url, filename, zip, BackupTemporaryFolder));
        }

        /// <summary>Restores site collection under given url from backup.</summary>
        /// <param name="siteCollection">Site collection to be restored.</param>
        /// <param name="filename">Backup file name to restore from.</param>
        public void Enterprise_RestoreSiteCollection(SharePointEnterpriseSiteCollection siteCollection, string filename)
        {
            ExecuteSharePointAction<object>(delegate(HostedSharePointServer2013EntImpl impl)
            {
                impl.RestoreSiteCollection(Enterprise_RootWebApplicationUri, siteCollection, filename);
                return null;
            });
        }

        /// <summary>Gets binary data chunk of specified size from specified offset.</summary>
        /// <param name="path">Path to file to get bunary data chunk from.</param>
        /// <param name="offset">Offset from which to start data reading.</param>
        /// <param name="length">Binary data chunk length.</param>
        /// <returns>Binary data chunk read from file.</returns>
        public virtual byte[] Enterprise_GetTempFileBinaryChunk(string path, int offset, int length)
        {
            byte[] buffer = FileUtils.GetFileBinaryChunk(path, offset, length);

            if (buffer.Length < length)
            {
                FileUtils.DeleteFile(path);
            }

            return buffer;
        }

        /// <summary>Appends supplied binary data chunk to file.</summary>
        /// <param name="fileName">Non existent file name to append to.</param>
        /// <param name="path">Full path to existent file to append to.</param>
        /// <param name="chunk">Binary data chunk to append to.</param>
        /// <returns>Path to file that was appended with chunk.</returns>
        public virtual string Enterprise_AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            if (path == null)
            {
                path = Path.Combine(Path.GetTempPath(), fileName);
                if (FileUtils.FileExists(path))
                {
                    FileUtils.DeleteFile(path);
                }
            }

            FileUtils.AppendFileBinaryContent(path, chunk);

            return path;
        }

        public void Enterprise_UpdateQuotas(string url, long maxStorage, long warningStorage)
        {
            ExecuteSharePointAction<object>(delegate(HostedSharePointServer2013EntImpl impl)
            {
                impl.UpdateQuotas(Enterprise_RootWebApplicationUri, url, maxStorage, warningStorage);
                return null;
            });
        }

        public SharePointSiteDiskSpace[] Enterprise_CalculateSiteCollectionsDiskSpace(string[] urls)
        {
            return ExecuteSharePointAction(impl => impl.CalculateSiteCollectionDiskSpace(Enterprise_RootWebApplicationUri, urls));
        }

        public long Enterprise_GetSiteCollectionSize(string url)
        {
            return ExecuteSharePointAction(impl => impl.GetSiteCollectionSize(Enterprise_RootWebApplicationUri, url));
        }

        public void Enterprise_SetPeoplePickerOu(string site, string ou)
        {
            ExecuteSharePointAction<object>(delegate(HostedSharePointServer2013EntImpl impl)
            {
                impl.SetPeoplePickerOu(site, ou);
                return null;
            });
        }


        public override bool IsInstalled()
        {
            return IsSharePointInstalled();
        }

        /// <summary>Deletes service items that represent SharePoint site collection.</summary>
        /// <param name="items">Items to be deleted.</param>
        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                var SharePointEnterpriseSiteCollection = item as SharePointEnterpriseSiteCollection;

                if (SharePointEnterpriseSiteCollection != null)
                {
                    try
                    {
                        Enterprise_DeleteSiteCollection(SharePointEnterpriseSiteCollection);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                    }
                }
            }
        }

        /// <summary>Calculates diskspace used by supplied service items.</summary>
        /// <param name="items">Service items to get diskspace usage for.</param>
        /// <returns>Calculated disk space usage statistics.</returns>
        public override ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
        {
            var itemsDiskspace = new List<ServiceProviderItemDiskSpace>();

            foreach (ServiceProviderItem item in items)
            {
                if (item is SharePointEnterpriseSiteCollection)
                {
                    try
                    {
                        Log.WriteStart(String.Format("Calculating '{0}' site logs size", item.Name));

                        SharePointEnterpriseSiteCollection site = Enterprise_GetSiteCollection(item.Name);
                        var diskspace = new ServiceProviderItemDiskSpace { ItemId = item.Id, DiskSpace = site.Diskspace };
                        itemsDiskspace.Add(diskspace);

                        Log.WriteEnd(String.Format("Calculating '{0}' site logs size", item.Name));
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(ex);
                    }
                }
            }

            return itemsDiskspace.ToArray();
        }

        /// <summary>Checks whether SharePoint 2013 is installed.</summary>
        /// <returns>true - if it is installed; false - otherwise.</returns>
        private bool IsSharePointInstalled()
        {
            RegistryKey spKey = Registry.LocalMachine.OpenSubKey(Wss3RegistryKey);
            RegistryKey spKey32 = Registry.LocalMachine.OpenSubKey(Wss3Registry32Key);

            if (spKey == null && spKey32 == null)
            {
                return false;
            }

            var spVal = (string)spKey.GetValue("SharePoint");

            return (String.Compare(spVal, "installed", true) == 0);
        }

        /// <summary>Executes supplied action within separate application domain.</summary>
        /// <param name="action">Action to be executed.</param>
        /// <returns>Any object that results from action execution or null if nothing is supposed to be returned.</returns>
        /// <exception cref="ArgumentNullException">Is thrown in case supplied action is null.</exception>
        private static TReturn ExecuteSharePointAction<TReturn>(SharePointAction<TReturn> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            AppDomain domain = null;

            try
            {
                Type type = typeof(HostedSharePointServer2013EntImpl);
                var info = new AppDomainSetup { ApplicationBase = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), PrivateBinPath = GetPrivateBinPath() };
                domain = AppDomain.CreateDomain("WSS30", null, info);
                var impl = (HostedSharePointServer2013EntImpl)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);

                return action(impl);
            }
            finally
            {
                if (domain != null)
                {
                    AppDomain.Unload(domain);
                }
            }

            throw new ArgumentNullException("action");
        }

        /// <summary> Getting PrivatePath from web.config. </summary>
        /// <returns> The PrivateBinPath.</returns>
        private static string GetPrivateBinPath()
        {
            var lines = new List<string> { "bin", "bin/debug" };
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "web.config");

            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {
                    string content = reader.ReadToEnd();
                    var pattern = new Regex(@"(?<=probing .*?privatePath\s*=\s*"")[^""]+(?="".*?>)");
                    Match match = pattern.Match(content);
                    lines.AddRange(match.Value.Split(';'));
                }
            }

            return string.Join(Path.PathSeparator.ToString(), lines.ToArray());
        }

        #endregion
    }
}
