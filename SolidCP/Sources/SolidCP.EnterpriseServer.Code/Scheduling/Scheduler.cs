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
using System.ServiceProcess;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace SolidCP.EnterpriseServer
{
    public delegate void ScheduleFinished(SchedulerJob schedule);

    public sealed class Scheduler
    {
        public static SchedulerJob nextSchedule = null;

        public static void Start()
        {
            ScheduleTasks();
        }

        public static bool IsScheduleActive(int scheduleId)
        {
            Dictionary<int, BackgroundTask> scheduledTasks = TaskManager.GetScheduledTasks();
            
            return scheduledTasks.ContainsKey(scheduleId);
        }

        public static void ScheduleTasks()
        {
            RunManualTasks();

            nextSchedule = SchedulerController.GetNextSchedule();

            if (nextSchedule != null)
            {
                if (nextSchedule.ScheduleInfo.NextRun <= DateTime.Now)
                {
                    RunNextSchedule(null);
                }
            }
        }

        private static void RunManualTasks()
        {
            var tasks = TaskController.GetProcessTasks(BackgroundTaskStatus.Stopping);

            foreach (var task in tasks)
            {
                TaskManager.StopTask(task.TaskId);
            }

            tasks = TaskController.GetProcessTasks(BackgroundTaskStatus.Starting);

            foreach (var task in tasks)
            {
                var taskThread = new Thread(() => RunBackgroundTask(task)) { Priority = ThreadPriority.Highest };
                taskThread.Start();
                TaskManager.AddTaskThread(task.Id, taskThread);
            }
        }

        private static void RunBackgroundTask(BackgroundTask backgroundTask)
        {
            UserInfo user = PackageController.GetPackageOwner(backgroundTask.PackageId);
            
            SecurityContext.SetThreadPrincipal(user.UserId);
            
            var schedule = SchedulerController.GetScheduleComplete(backgroundTask.ScheduleId);

            backgroundTask.Guid = TaskManager.Guid;
            backgroundTask.Status = BackgroundTaskStatus.Run;


            TaskController.UpdateTask(backgroundTask);
            
            try
            {
                var objTask = (SchedulerTask)Activator.CreateInstance(Type.GetType(schedule.Task.TaskType));

                objTask.DoWork();
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex, "Error executing scheduled task");
            }
            finally
            {
                try
                {
                    TaskManager.CompleteTask();
                }
                catch (Exception)
                {
                }
            }
        }

        // call back for the timer function
        static void RunNextSchedule(object obj) // obj ignored
        {            
            if (nextSchedule == null)
                return;

            RunSchedule(nextSchedule, true);

            // schedule next task
            ScheduleTasks();
        }

        static void RunSchedule(SchedulerJob schedule, bool changeNextRun)
        {
            try
            {
                // update next run (if required)
                if (changeNextRun)
                {
                    SchedulerController.CalculateNextStartTime(schedule.ScheduleInfo);
                }

                // disable run once task
                if (schedule.ScheduleInfo.ScheduleType == ScheduleType.OneTime)
                    schedule.ScheduleInfo.Enabled = false;

                Dictionary<int, BackgroundTask> scheduledTasks = TaskManager.GetScheduledTasks();
                if (!scheduledTasks.ContainsKey(schedule.ScheduleInfo.ScheduleId))
                    // this task should be run, so
                    // update its last run
                    schedule.ScheduleInfo.LastRun = DateTime.Now;

                // update schedule
                int MAX_RETRY_COUNT = 10;
                int counter = 0;
                while (counter < MAX_RETRY_COUNT)
                {
                    try
                    {
                        SchedulerController.UpdateSchedule(schedule.ScheduleInfo);
                        break;
                    }
                    catch (SqlException)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }

                    counter++;
                }
                if (counter == MAX_RETRY_COUNT)
                    return;

                // skip execution if the current task is still running
                scheduledTasks = TaskManager.GetScheduledTasks();
                if (!scheduledTasks.ContainsKey(schedule.ScheduleInfo.ScheduleId))
                {
                    // run the schedule in the separate thread
                    schedule.Run();
                }
            }
            catch (Exception Ex)
            {
                try
                {
                    TaskManager.WriteError(string.Format("RunSchedule Error : {0}", Ex.Message));
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
