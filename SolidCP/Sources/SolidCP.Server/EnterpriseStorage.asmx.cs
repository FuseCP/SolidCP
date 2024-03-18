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
using System.Data;
using System.Web;
using System.Collections;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.EnterpriseStorage;
using SolidCP.Providers.OS;
using SolidCP.Server.Utils;
using SolidCP.Providers.Web;

namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for EnterpriseStorage
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class EnterpriseStorage : HostingServiceProviderWebService, IEnterpriseStorage
    {
        private IEnterpriseStorage EnterpriseStorageProvider
        {
            get { return (IEnterpriseStorage)Provider; }
        }


        [WebMethod, SoapHeader("settings")]
        public SystemFile[] GetFolders(string organizationId, WebDavSetting[] settings)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolders", ProviderSettings.ProviderName);
                SystemFile[] result = EnterpriseStorageProvider.GetFolders(organizationId, settings);
                Log.WriteEnd("'{0}' GetFolders", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolders", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile[] GetFoldersWithoutFrsm(string organizationId, WebDavSetting[] settings)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolders", ProviderSettings.ProviderName);
                SystemFile[] result = EnterpriseStorageProvider.GetFoldersWithoutFrsm(organizationId, settings);
                Log.WriteEnd("'{0}' GetFolders", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolders", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile GetFolder(string organizationId, string folder, WebDavSetting setting)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolder", ProviderSettings.ProviderName);
                SystemFile result = EnterpriseStorageProvider.GetFolder(organizationId, folder, setting);
                Log.WriteEnd("'{0}' GetFolder", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateFolder(string organizationId, string folder, WebDavSetting setting)
        {
            try
            {
                Log.WriteStart("'{0}' CreateFolder", ProviderSettings.ProviderName);
                EnterpriseStorageProvider.CreateFolder(organizationId, folder, setting);
                Log.WriteEnd("'{0}' CreateFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteFolder(string organizationId, string folder, WebDavSetting setting)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteFolder", ProviderSettings.ProviderName);
                EnterpriseStorageProvider.DeleteFolder(organizationId, folder, setting);
                Log.WriteEnd("'{0}' DeleteFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting, WebDavFolderRule[] rules)
        {
            try
            {
                Log.WriteStart("'{0}' SetFolderWebDavRules", ProviderSettings.ProviderName);
                bool bResult =  EnterpriseStorageProvider.SetFolderWebDavRules(organizationId, folder, setting, rules);
                Log.WriteEnd("'{0}' SetFolderWebDavRules", ProviderSettings.ProviderName);
                return bResult;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetFolderWebDavRules", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder, WebDavSetting setting)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolderWebDavRules", ProviderSettings.ProviderName);
                Providers.Web.WebDavFolderRule[]  webDavFolderRule =  EnterpriseStorageProvider.GetFolderWebDavRules(organizationId, folder, setting);
                Log.WriteEnd("'{0}' GetFolderWebDavRules", ProviderSettings.ProviderName);
                return webDavFolderRule;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolderWebDavRules", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool CheckFileServicesInstallation()
        {
            try
            {
                Log.WriteStart("'{0}' CheckFileServicesInstallation", ProviderSettings.ProviderName);
                bool bResult = EnterpriseStorageProvider.CheckFileServicesInstallation();
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
        public SystemFile[] Search(string organizationId, string[] searchPaths, string searchText, string userPrincipalName, bool recursive)
        {
            try
            {
                Log.WriteStart("'{0}' Search", ProviderSettings.ProviderName);
                var searchResults = EnterpriseStorageProvider.Search(organizationId, searchPaths, searchText, userPrincipalName, recursive);
                Log.WriteEnd("'{0}' Search", ProviderSettings.ProviderName);
                return searchResults;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' Search", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder, WebDavSetting setting)
        {
            try
            {
                Log.WriteStart("'{0}' RenameFolder", ProviderSettings.ProviderName);
                SystemFile systemFile = EnterpriseStorageProvider.RenameFolder(organizationId, originalFolder, newFolder, setting);
                Log.WriteEnd("'{0}' RenameFolder", ProviderSettings.ProviderName);
                return systemFile;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RenameFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile[] GetQuotasForOrganization(SystemFile[] folders)
        {
            try
            {
                Log.WriteStart("'{0}' GetQuotasForOrganization", ProviderSettings.ProviderName);
                var newFolders = EnterpriseStorageProvider.GetQuotasForOrganization(folders);
                Log.WriteEnd("'{0}' GetQuotasForOrganization", ProviderSettings.ProviderName);
                return newFolders;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetQuotasForOrganization", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void MoveFolder(string oldPath, string newPath)
        {
            try
            {
                Log.WriteStart("'{0}' MoveFolder", ProviderSettings.ProviderName);
                EnterpriseStorageProvider.MoveFolder(oldPath, newPath);
                Log.WriteEnd("'{0}' MoveFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' MoveFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

    }
}
