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
    public class UserAsyncWorker
    {
        private int threadUserId = -1;
        private int userId;
        private string taskId;
        private UserInfo user;

        #region Public properties
        public int ThreadUserId
        {
            get { return this.threadUserId; }
            set { this.threadUserId = value; }
        }

        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        public string TaskId
        {
            get { return this.taskId; }
            set { this.taskId = value; }
        }

        public UserInfo User
        {
            get { return this.user; }
            set { this.user = value; }
        }
        #endregion

        #region Update User
        public void UpdateUserAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(UpdateUser));
            t.Start();
        }

        public void UpdateUser()
        {
            // impersonate thread
            if (threadUserId != -1)
                SecurityContext.SetThreadPrincipal(threadUserId);

            // update
            UserController.UpdateUser(taskId, user);
        }
        #endregion

        #region Delete User
        public void DeleteUserAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(DeleteUser));
            t.Start();
        }

        public void DeleteUser()
        {
            // impersonate thread
            if (threadUserId != -1)
                SecurityContext.SetThreadPrincipal(threadUserId);

            // get user details
            UserInfo user = UserController.GetUserInternally(userId);

            // place log record
            TaskManager.StartTask(taskId, "USER", "DELETE", user.Username, userId);

            try
            {
                // delete user packages
                List<PackageInfo> packages = PackageController.GetMyPackages(userId);

                // delete user packages synchronously
                if (packages.Count > 0)
                {
                    PackageAsyncWorker packageWorker = new PackageAsyncWorker();
                    packageWorker.UserId = SecurityContext.User.UserId;
                    packageWorker.Packages = packages;

                    // invoke worker
                    packageWorker.DeletePackagesServiceItems();
                }

                // delete user from database
                DataProvider.DeleteUser(SecurityContext.User.UserId, userId);
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        #endregion
    }
}
