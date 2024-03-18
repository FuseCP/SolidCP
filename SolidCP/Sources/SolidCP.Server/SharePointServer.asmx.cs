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
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.SharePoint;
using SolidCP.Server.Utils;

namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for SharePointServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class SharePointServer : HostingServiceProviderWebService
    {
        private ISharePointServer SPS
        {
            get { return (ISharePointServer)Provider; }
        }

        #region Sites
        [WebMethod, SoapHeader("settings")]
        public void ExtendVirtualServer(SharePointSite site)
        {
            try
            {
                Log.WriteStart("'{0}' ExtendVirtualServer", ProviderSettings.ProviderName);
                SPS.ExtendVirtualServer(site);
                Log.WriteEnd("'{0}' ExtendVirtualServer", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't ExtendVirtualServer '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UnextendVirtualServer(string url, bool deleteContent)
        {
            try
            {
                Log.WriteStart("'{0}' GetProviderProperties", ProviderSettings.ProviderName);
                SPS.UnextendVirtualServer(url, deleteContent);
                Log.WriteEnd("'{0}' GetProviderProperties", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't GetProviderProperties '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Backup/Restore
        [WebMethod, SoapHeader("settings")]
        public string BackupVirtualServer(string url, string fileName, bool zipBackup)
        {
            try
            {
                Log.WriteStart("'{0}' BackupVirtualServer", ProviderSettings.ProviderName);
                string result = SPS.BackupVirtualServer(url, fileName, zipBackup);
                Log.WriteEnd("'{0}' BackupVirtualServer", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't BackupVirtualServer '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void RestoreVirtualServer(string url, string fileName)
        {
            try
            {
                Log.WriteStart("'{0}' RestoreVirtualServer", ProviderSettings.ProviderName);
                SPS.RestoreVirtualServer(url, fileName);
                Log.WriteEnd("'{0}' RestoreVirtualServer", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't RestoreVirtualServer '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public byte[] GetTempFileBinaryChunk(string path, int offset, int length)
        {
            try
            {
                Log.WriteStart("'{0}' GetTempFileBinaryChunk", ProviderSettings.ProviderName);
                byte[] result = SPS.GetTempFileBinaryChunk(path, offset, length);
                Log.WriteEnd("'{0}' GetTempFileBinaryChunk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't GetTempFileBinaryChunk '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string AppendTempFileBinaryChunk(string fileName, string path, byte[] chunk)
        {
            try
            {
                Log.WriteStart("'{0}' AppendTempFileBinaryChunk", ProviderSettings.ProviderName);
                string result = SPS.AppendTempFileBinaryChunk(fileName, path, chunk);
                Log.WriteEnd("'{0}' AppendTempFileBinaryChunk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't AppendTempFileBinaryChunk '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Web Parts
        [WebMethod, SoapHeader("settings")]
        public string[] GetInstalledWebParts(string url)
        {
            try
            {
                Log.WriteStart("'{0}' GetInstalledWebParts", ProviderSettings.ProviderName);
                string[] result = SPS.GetInstalledWebParts(url);
                Log.WriteEnd("'{0}' GetInstalledWebParts", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't GetInstalledWebParts '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void InstallWebPartsPackage(string url, string packageName)
        {
            try
            {
                Log.WriteStart("'{0}' InstallWebPartsPackage", ProviderSettings.ProviderName);
                SPS.InstallWebPartsPackage(url, packageName);
                Log.WriteEnd("'{0}' InstallWebPartsPackage", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't InstallWebPartsPackage '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteWebPartsPackage(string url, string packageName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteWebPartsPackage", ProviderSettings.ProviderName);
                SPS.DeleteWebPartsPackage(url, packageName);
                Log.WriteEnd("'{0}' DeleteWebPartsPackage", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't DeleteWebPartsPackage '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Users
        [WebMethod, SoapHeader("settings")]
        public bool UserExists(string username)
        {
            try
            {
                Log.WriteStart("'{0}' UserExists", ProviderSettings.ProviderName);
                bool result = SPS.UserExists(username);
                Log.WriteEnd("'{0}' UserExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UserExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetUsers()
        {
            try
            {
                Log.WriteStart("'{0}' GetUsers", ProviderSettings.ProviderName);
                string[] result = SPS.GetUsers();
                Log.WriteEnd("'{0}' GetUsers", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetUsers", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemUser GetUser(string username)
        {
            try
            {
                Log.WriteStart("'{0}' GetUser", ProviderSettings.ProviderName);
                SystemUser result = SPS.GetUser(username);
                Log.WriteEnd("'{0}' GetUser", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateUser(SystemUser user)
        {
            try
            {
                Log.WriteStart("'{0}' CreateUser", ProviderSettings.ProviderName);
                SPS.CreateUser(user);
                Log.WriteEnd("'{0}' CreateUser", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateUser(SystemUser user)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateUser", ProviderSettings.ProviderName);
                SPS.UpdateUser(user);
                Log.WriteEnd("'{0}' UpdateUser", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ChangeUserPassword(string username, string password)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeUserPassword", ProviderSettings.ProviderName);
                SPS.ChangeUserPassword(username, password);
                Log.WriteEnd("'{0}' ChangeUserPassword", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeUserPassword", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteUser(string username)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteUser", ProviderSettings.ProviderName);
                SPS.DeleteUser(username);
                Log.WriteEnd("'{0}' DeleteUser", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Groups
        [WebMethod, SoapHeader("settings")]
        public bool GroupExists(string groupName)
        {
            try
            {
                Log.WriteStart("'{0}' GroupExists", ProviderSettings.ProviderName);
                bool result = SPS.GroupExists(groupName);
                Log.WriteEnd("'{0}' GroupExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GroupExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetGroups()
        {
            try
            {
                Log.WriteStart("'{0}' GetGroups", ProviderSettings.ProviderName);
                string[] result = SPS.GetGroups();
                Log.WriteEnd("'{0}' GetGroups", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetGroups", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemGroup GetGroup(string groupName)
        {
            try
            {
                Log.WriteStart("'{0}' GetGroup", ProviderSettings.ProviderName);
                SystemGroup result = SPS.GetGroup(groupName);
                Log.WriteEnd("'{0}' GetGroup", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateGroup(SystemGroup group)
        {
            try
            {
                Log.WriteStart("'{0}' CreateGroup", ProviderSettings.ProviderName);
                SPS.CreateGroup(group);
                Log.WriteEnd("'{0}' CreateGroup", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateGroup(SystemGroup group)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateGroup", ProviderSettings.ProviderName);
                SPS.UpdateGroup(group);
                Log.WriteEnd("'{0}' UpdateGroup", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteGroup(string groupName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteGroup", ProviderSettings.ProviderName);
                SPS.DeleteGroup(groupName);
                Log.WriteEnd("'{0}' DeleteGroup", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteGroup", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion
    }
}
