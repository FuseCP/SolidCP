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
using System.IO;
using System.Collections.Generic;
using System.Text;

using SolidCP.Providers.Database;

namespace SolidCP.EnterpriseServer
{
    public class BackupDatabaseTask : SchedulerTask
    {
        public override void DoWork()
        {
            // Input parameters:
            //  - DATABASE_GROUP
            //  - DATABASE_NAME
            //  - BACKUP_FOLDER
            //  - BACKUP_NAME
            //  - ZIP_BACKUP

            BackgroundTask topTask = TaskManager.TopTask;

            string databaseGroup = (string)topTask.GetParamValue("DATABASE_GROUP");
            string databaseName = (string)topTask.GetParamValue("DATABASE_NAME");
            string backupFolder = (string)topTask.GetParamValue("BACKUP_FOLDER");
            string backupName = (string)topTask.GetParamValue("BACKUP_NAME");
            string strZipBackup = (string)topTask.GetParamValue("ZIP_BACKUP");

            // check input parameters
            if (String.IsNullOrEmpty(databaseName))
            {
                TaskManager.WriteWarning("Specify 'Database Name' task parameter.");
                return;
            }

            bool zipBackup = (strZipBackup.ToLower() == "true");

            if (String.IsNullOrEmpty(backupName))
            {
                backupName = databaseName + (zipBackup ? ".zip" : ".bak");
            }
            else
            {
                // check extension
                string ext = Path.GetExtension(backupName);
                if (zipBackup && String.Compare(ext, ".zip", true) != 0)
                {
                    // change extension to .zip
                    backupName = Path.GetFileNameWithoutExtension(backupName) + ".zip";
                }
            }

            // try to find database
            SqlDatabase item = (SqlDatabase)PackageController.GetPackageItemByName(topTask.PackageId, databaseGroup,
                databaseName, typeof(SqlDatabase));

            if (item == null)
            {
                TaskManager.WriteError("Database with the specified name was not found in the current hosting space.");
                return;
            }

            if (String.IsNullOrEmpty(backupFolder))
                backupFolder = "\\";

            // substitute parameters
            DateTime d = DateTime.Now;
            string date = d.ToString("yyyyMMdd");
            string time = d.ToString("HHmm");

            backupFolder = Utils.ReplaceStringVariable(backupFolder, "date", date);
            backupFolder = Utils.ReplaceStringVariable(backupFolder, "time", time);
            backupName = Utils.ReplaceStringVariable(backupName, "date", date);
            backupName = Utils.ReplaceStringVariable(backupName, "time", time);

            // backup database
            DatabaseServerController.BackupSqlDatabase(item.Id, backupName, zipBackup, false, backupFolder);
        }
    }
}
