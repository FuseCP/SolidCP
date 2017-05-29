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
using System.Configuration;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SolidCP.EnterpriseServer
{
	/// <summary>
	/// Represents scheduler task that performs hosting space backup.
	/// </summary>
	public class BackupTask : SchedulerTask
	{
		/// <summary>
		/// Performs actual backup.
		/// </summary>
		public override void DoWork()
		{

			string backupFileName;
			int storePackageId;
			string storePackageFolder;
			string storeServerFolder;
			bool deleteTempBackup;

            BackgroundTask topTask = TaskManager.TopTask;

			try
			{
				backupFileName = (string)topTask.GetParamValue("BACKUP_FILE_NAME");
				storePackageId = Convert.ToInt32(topTask.GetParamValue("STORE_PACKAGE_ID"));
				storePackageFolder = (string)topTask.GetParamValue("STORE_PACKAGE_FOLDER");
				storeServerFolder = (string)topTask.GetParamValue("STORE_SERVER_FOLDER");
				deleteTempBackup = Convert.ToBoolean(topTask.GetParamValue("DELETE_TEMP_BACKUP"));
			}
			catch(Exception ex)
			{
				TaskManager.WriteError(ex, "Some parameters are absent or have incorrect value.");
				return;
			}

			try
			{
				PackageInfo package = PackageController.GetPackage(topTask.PackageId);
				// We do not take into account service id as long as scheduled tasks run against packages.
				BackupController.Backup(false, "BackupTask", package.UserId, package.PackageId, 0, 0,
                    backupFileName, storePackageId, storePackageFolder, storeServerFolder, deleteTempBackup);
			}
			catch(Exception ex)
			{
				TaskManager.WriteError(ex, "Failed to do backup.");
			}
		}
	}
}
