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
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.EnterpriseServer
{
    public class SchedulerJob
    {
        private ScheduleInfo scheduleInfo;
        private ScheduleTaskInfo task;

        public ScheduleFinished ScheduleFinishedCallback;

        #region public properties
        public ScheduleInfo ScheduleInfo
        {
            get { return this.scheduleInfo; }
            set { this.scheduleInfo = value; }
        }

        public ScheduleTaskInfo Task
        {
            get { return this.task; }
            set { this.task = value; }
        }
        #endregion

        // Constructor
        public SchedulerJob()
        {
        }

        // Sets the next time this Schedule is kicked off and kicks off events on
        // a seperate thread, freeing the Scheduler to continue
        public void Run()
        {
            // create worker
            Thread worker = new Thread(new ThreadStart(RunSchedule));
            // set worker priority
            switch (scheduleInfo.Priority)
            {
                case SchedulePriority.Highest: worker.Priority = ThreadPriority.Highest; break;
                case SchedulePriority.AboveNormal: worker.Priority = ThreadPriority.AboveNormal; break;
                case SchedulePriority.Normal: worker.Priority = ThreadPriority.Normal; break;
                case SchedulePriority.BelowNormal: worker.Priority = ThreadPriority.BelowNormal; break;
                case SchedulePriority.Lowest: worker.Priority = ThreadPriority.Lowest; break;
            }

            // start worker!
            worker.Start();
        }

        // Implementation of ThreadStart delegate.
        // Used by Scheduler to kick off events on a seperate thread
        private void RunSchedule()
        {
            // impersonate thread
            UserInfo user = PackageController.GetPackageOwner(scheduleInfo.PackageId);
            SecurityContext.SetThreadPrincipal(user.UserId);

            List<BackgroundTaskParameter> parameters = new List<BackgroundTaskParameter>();
            foreach (ScheduleTaskParameterInfo prm in scheduleInfo.Parameters)
            {
                parameters.Add(new BackgroundTaskParameter(prm.ParameterId, prm.ParameterValue));
            }

            TaskManager.StartTask("SCHEDULER", "RUN_SCHEDULE", scheduleInfo.ScheduleName, scheduleInfo.ScheduleId,
                                  scheduleInfo.ScheduleId, scheduleInfo.PackageId, scheduleInfo.MaxExecutionTime,
                                  parameters);

            // run task
            try
            {
                // create scheduled task object
                SchedulerTask objTask = (SchedulerTask)Activator.CreateInstance(Type.GetType(task.TaskType));

                if (objTask != null)
                    objTask.DoWork();
                else
                    throw new Exception(String.Format("Could not create scheduled task of '{0}' type",
                        task.TaskType));      
               // Thread.Sleep(40000);
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex, "Error executing scheduled task");
            }
            finally
            {
                // complete task
                try
                {
                    TaskManager.CompleteTask();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
