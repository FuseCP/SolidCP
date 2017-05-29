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
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Microsoft.Web.Services3;

using SolidCP.Providers.OS;
using SolidCP.Providers.SharePoint;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esSharePointServers : System.Web.Services.WebService
    {

        #region Sites
        [WebMethod]
        public DataSet GetRawSharePointSitesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SharePointServerController.GetRawSharePointSitesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<SharePointSite> GetSharePointSites(int packageId, bool recursive)
        {
            return SharePointServerController.GetSharePointSites(packageId, recursive);
        }

        [WebMethod]
        public SharePointSite GetSharePointSite(int itemId)
        {
            return SharePointServerController.GetSite(itemId);
        }

        [WebMethod]
        public int AddSharePointSite(SharePointSite item)
        {
            return SharePointServerController.AddSite(item);
        }

        [WebMethod]
        public int DeleteSharePointSite(int itemId)
        {
            return SharePointServerController.DeleteSite(itemId);
        }
        #endregion

        #region Backup/Restore
        [WebMethod]
        public string BackupVirtualServer(int itemId, string fileName,
            bool zipBackup, bool download, string folderName)
        {
            return SharePointServerController.BackupVirtualServer(itemId, fileName, zipBackup, download, folderName);
        }

        [WebMethod]
        public byte[] GetSharePointBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return SharePointServerController.GetSharePointBackupBinaryChunk(itemId, path, offset, length);
        }

        [WebMethod]
        public string AppendSharePointBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return SharePointServerController.AppendSharePointBackupBinaryChunk(itemId, fileName, path, chunk);
        }

        [WebMethod]
        public int RestoreVirtualServer(int itemId, string uploadedFile, string packageFile)
        {
            return SharePointServerController.RestoreVirtualServer(itemId, uploadedFile, packageFile);
        }
        #endregion

        #region Web Parts
        [WebMethod]
        public string[] GetInstalledWebParts(int itemId)
        {
            return SharePointServerController.GetInstalledWebParts(itemId);
        }

        [WebMethod]
        public int InstallWebPartsPackage(int itemId, string uploadedFile, string packageFile)
        {
            return SharePointServerController.InstallWebPartsPackage(itemId, uploadedFile, packageFile);
        }

        [WebMethod]
        public int DeleteWebPartsPackage(int itemId, string packageName)
        {
            return SharePointServerController.DeleteWebPartsPackage(itemId, packageName);
        }
        #endregion

        #region Users
        [WebMethod]
        public DataSet GetRawSharePointUsersPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SharePointServerController.GetRawSharePointUsersPaged(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<SystemUser> GetSharePointUsers(int packageId, bool recursive)
        {
            return SharePointServerController.GetSharePointUsers(packageId, recursive);
        }

        [WebMethod]
        public SystemUser GetSharePointUser(int itemId)
        {
            return SharePointServerController.GetSharePointUser(itemId);
        }

        [WebMethod]
        public int AddSharePointUser(SystemUser item)
        {
            return SharePointServerController.AddSharePointUser(item); ;
        }

        [WebMethod]
        public int UpdateSharePointUser(SystemUser item)
        {
            return SharePointServerController.UpdateSharePointUser(item);
        }

        [WebMethod]
        public int DeleteSharePointUser(int itemId)
        {
            return SharePointServerController.DeleteSharePointUser(itemId);
        }

        #endregion

        #region Groups
        [WebMethod]
        public DataSet GetRawSharePointGroupsPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SharePointServerController.GetRawSharePointGroupsPaged(packageId,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<SystemGroup> GetSharePointGroups(int packageId, bool recursive)
        {
            return SharePointServerController.GetSharePointGroups(packageId, recursive);
        }

        [WebMethod]
        public SystemGroup GetSharePointGroup(int itemId)
        {
            return SharePointServerController.GetSharePointGroup(itemId);
        }

        [WebMethod]
        public int AddSharePointGroup(SystemGroup item)
        {
            return SharePointServerController.AddSharePointGroup(item);
        }

        [WebMethod]
        public int UpdateSharePointGroup(SystemGroup item)
        {
            return SharePointServerController.UpdateSharePointGroup(item);
        }

        [WebMethod]
        public int DeleteSharePointGroup(int itemId)
        {
            return SharePointServerController.DeleteSharePointGroup(itemId);
        }
        #endregion
    }
}
