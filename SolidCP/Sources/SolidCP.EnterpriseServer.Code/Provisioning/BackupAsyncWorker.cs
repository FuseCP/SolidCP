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
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.EnterpriseServer
{
	public class BackupAsyncWorker
	{
		public int threadUserId = -1;
		public string taskId;
		public int userId;
		public int packageId;
		public int serviceId;
		public int serverId;
		public string backupFileName;
		public int storePackageId;
		public string storePackageFolder;
		public string storeServerFolder;
		public string storePackageBackupPath;
		public string storeServerBackupPath;
		public bool deleteTempBackup;

		#region Backup
		public void BackupAsync()
		{
			// start asynchronously
			Thread t = new Thread(new ThreadStart(Backup));
            t.Start();
		}

		public void Backup()
		{
			// impersonate thread
			if (threadUserId != -1)
				SecurityContext.SetThreadPrincipal(threadUserId);

			// perform backup
			BackupController.BackupInternal(taskId, userId, packageId, serviceId, serverId, backupFileName, storePackageId,
				storePackageFolder, storeServerFolder, deleteTempBackup);
		}
		#endregion

		#region Restore
		public void RestoreAsync()
		{
			// start asynchronously
			Thread t = new Thread(new ThreadStart(Restore));
			t.Start();
		}

		public void Restore()
		{
			// impersonate thread
			if (threadUserId != -1)
				SecurityContext.SetThreadPrincipal(threadUserId);

			// perform restore
			BackupController.RestoreInternal(taskId, userId, packageId, serviceId, serverId,
				storePackageId, storePackageBackupPath, storeServerBackupPath);
		}
		#endregion
	}
}
