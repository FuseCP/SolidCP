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

using SolidCP.Providers.Database;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esDatabaseServers : System.Web.Services.WebService
    {
        #region Databases
        [WebMethod]
        public DataSet GetRawSqlDatabasesPaged(int packageId, string groupName,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return DatabaseServerController.GetRawSqlDatabasesPaged(packageId, groupName, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<SqlDatabase> GetSqlDatabases(int packageId, string groupName, bool recursive)
        {
            return DatabaseServerController.GetSqlDatabases(packageId, groupName, recursive);
        }

        [WebMethod]
        public SqlDatabase GetSqlDatabase(int itemId)
        {
            return DatabaseServerController.GetSqlDatabase(itemId);
        }

        [WebMethod]
        public int AddSqlDatabase(SqlDatabase item, string groupName)
        {
            return DatabaseServerController.AddSqlDatabase(item, groupName);
        }

        [WebMethod]
        public int UpdateSqlDatabase(SqlDatabase item)
        {
            return DatabaseServerController.UpdateSqlDatabase(item);
        }

        [WebMethod]
        public int DeleteSqlDatabase(int itemId)
        {
            return DatabaseServerController.DeleteSqlDatabase(itemId);
        }

        [WebMethod]
        public string BackupSqlDatabase(int itemId, string backupName,
                bool zipBackup, bool download, string folderName)
        {
            return DatabaseServerController.BackupSqlDatabase(itemId, backupName, zipBackup,
                download, folderName);
        }

        [WebMethod]
        public byte[] GetSqlBackupBinaryChunk(int itemId, string path, int offset, int length)
        {
            return DatabaseServerController.GetSqlBackupBinaryChunk(itemId, path, offset, length);
        }

        [WebMethod]
        public string AppendSqlBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
        {
            return DatabaseServerController.AppendSqlBackupBinaryChunk(itemId, fileName, path, chunk);
        }

        [WebMethod]
        public int RestoreSqlDatabase(int itemId, string[] uploadedFiles, string[] packageFiles)
        {
            return DatabaseServerController.RestoreSqlDatabase(itemId, uploadedFiles, packageFiles);
        }

        [WebMethod]
        public int TruncateSqlDatabase(int itemId)
        {
            return DatabaseServerController.TruncateSqlDatabase(itemId);
        }

		[WebMethod]
		public DatabaseBrowserConfiguration GetDatabaseBrowserConfiguration(int packageId, string groupName)
		{
			return DatabaseServerController.GetDatabaseBrowserConfiguration(packageId, groupName);
		}

		[WebMethod]
		public DatabaseBrowserConfiguration GetDatabaseBrowserLogonScript(int packageId,
			string groupName, string username)
		{
			return DatabaseServerController.GetDatabaseBrowserLogonScript(packageId, groupName, username);
		}

        #endregion

        #region Users
        [WebMethod]
        public DataSet GetRawSqlUsersPaged(int packageId, string groupName,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return DatabaseServerController.GetRawSqlUsersPaged(packageId, groupName,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<SqlUser> GetSqlUsers(int packageId, string groupName, bool recursive)
        {
            return DatabaseServerController.GetSqlUsers(packageId, groupName, recursive);
        }

        [WebMethod]
        public SqlUser GetSqlUser(int itemId)
        {
            return DatabaseServerController.GetSqlUser(itemId);
        }

        [WebMethod]
        public int AddSqlUser(SqlUser item, string groupName)
        {
            return DatabaseServerController.AddSqlUser(item, groupName);
        }

        [WebMethod]
        public int UpdateSqlUser(SqlUser item)
        {
            return DatabaseServerController.UpdateSqlUser(item);
        }

        [WebMethod]
        public int DeleteSqlUser(int itemId)
        {
            return DatabaseServerController.DeleteSqlUser(itemId);
        }
        #endregion
    }
}
