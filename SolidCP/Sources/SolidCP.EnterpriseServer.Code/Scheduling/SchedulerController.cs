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
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using SolidCP.EnterpriseServer.Base.Scheduling;

namespace SolidCP.EnterpriseServer
{
    public class SchedulerController
    {
        public static DateTime GetSchedulerTime()
        {
            return DateTime.Now;
        }

        public static List<ScheduleTaskInfo> GetScheduleTasks()
        {
            return ObjectUtils.CreateListFromDataReader<ScheduleTaskInfo>(
                DataProvider.GetScheduleTasks(SecurityContext.User.UserId));
        }

        public static ScheduleTaskInfo GetScheduleTask(string taskId)
        {
            return ObjectUtils.FillObjectFromDataReader<ScheduleTaskInfo>(
                DataProvider.GetScheduleTask(SecurityContext.User.UserId, taskId));
        }

        public static DataSet GetSchedules(int packageId)
        {
            DataSet ds = DataProvider.GetSchedules(SecurityContext.User.UserId, packageId);

            // set status to each returned schedule
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["StatusID"] = Scheduler.IsScheduleActive((int)dr["ScheduleID"])
                    ? ScheduleStatus.Running : ScheduleStatus.Idle;
            }
            return ds;
        }

        public static DataSet GetSchedulesPaged(int packageId, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            DataSet ds = DataProvider.GetSchedulesPaged(SecurityContext.User.UserId, packageId,
                recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);

            // set status to each returned schedule
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                dr["StatusID"] = Scheduler.IsScheduleActive((int)dr["ScheduleID"])
                    ? ScheduleStatus.Running : ScheduleStatus.Idle;
            }
            return ds;
        }

        public static ScheduleInfo GetSchedule(int scheduleId)
        {
            DataSet ds = DataProvider.GetSchedule(SecurityContext.User.UserId, scheduleId);
            ScheduleInfo si = ObjectUtils.FillObjectFromDataView<ScheduleInfo>(ds.Tables[0].DefaultView);
            return si;
        }

        /// <summary>
        /// Gets view configuration for a certain task.
        /// </summary>
        /// <param name="taskId">Task id for which view configuration is intended to be loeaded.</param>
        /// <returns>View configuration for the task with supplied id.</returns>
        public static List<ScheduleTaskViewConfiguration> GetScheduleTaskViewConfigurations(string taskId)
        {
            List<ScheduleTaskViewConfiguration> c = ObjectUtils.CreateListFromDataReader<ScheduleTaskViewConfiguration>(DataProvider.GetScheduleTaskViewConfigurations(taskId));
            return c;
        }

        internal static SchedulerJob GetScheduleComplete(int scheduleId)
        {
            DataSet ds = DataProvider.GetSchedule(SecurityContext.User.UserId, scheduleId);
            return CreateCompleteScheduleFromDataSet(ds);
        }

        internal static SchedulerJob GetNextSchedule()
        {
            DataSet ds = DataProvider.GetNextSchedule();
            return CreateCompleteScheduleFromDataSet(ds);
        }

        internal static SchedulerJob CreateCompleteScheduleFromDataSet(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count == 0)
                return null;

            SchedulerJob schedule = new SchedulerJob();

            // schedule info
            schedule.ScheduleInfo = ObjectUtils.FillObjectFromDataView<ScheduleInfo>(ds.Tables[0].DefaultView);

            // task info
            schedule.Task = ObjectUtils.FillObjectFromDataView<ScheduleTaskInfo>(ds.Tables[1].DefaultView);

            // parameters info
            List<ScheduleTaskParameterInfo> parameters = new List<ScheduleTaskParameterInfo>();
            ObjectUtils.FillCollectionFromDataView<ScheduleTaskParameterInfo>(
                parameters, ds.Tables[2].DefaultView);
            schedule.ScheduleInfo.Parameters = parameters.ToArray();

            return schedule;
        }

        public static ScheduleInfo GetScheduleInternal(int scheduleId)
        {
            return ObjectUtils.FillObjectFromDataReader<ScheduleInfo>(
                DataProvider.GetScheduleInternal(scheduleId));
        }

        public static List<ScheduleTaskParameterInfo> GetScheduleParameters(string taskId, int scheduleId)
        {
            return ObjectUtils.CreateListFromDataReader<ScheduleTaskParameterInfo>(
                DataProvider.GetScheduleParameters(SecurityContext.User.UserId,
                taskId, scheduleId));
        }

        public static int StartSchedule(int scheduleId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);

            if (accountCheck < 0)
                return accountCheck;

            SchedulerJob schedule = GetScheduleComplete(scheduleId);
            if (schedule == null)
                return 0;

            if (TaskController.GetScheduleTasks(scheduleId).Any(x => x.Status == BackgroundTaskStatus.Run
                                                                     || x.Status == BackgroundTaskStatus.Starting))
                return 0;

            var parameters = schedule.ScheduleInfo.Parameters.Select(
                prm => new BackgroundTaskParameter(prm.ParameterId, prm.ParameterValue)).ToList();

            var userInfo = PackageController.GetPackageOwner(schedule.ScheduleInfo.PackageId);

            var backgroundTask = new BackgroundTask(
                Guid.NewGuid(),
                Guid.NewGuid().ToString("N"),
                userInfo.OwnerId == 0 ? userInfo.UserId : userInfo.OwnerId,
                userInfo.UserId,
                "SCHEDULER",
                "RUN_SCHEDULE",
                schedule.ScheduleInfo.ScheduleName,
                schedule.ScheduleInfo.ScheduleId,
                schedule.ScheduleInfo.ScheduleId,
                schedule.ScheduleInfo.PackageId,
                schedule.ScheduleInfo.MaxExecutionTime, parameters)
                                     {
                                         Status = BackgroundTaskStatus.Starting
                                     };
            
            TaskController.AddTask(backgroundTask);

            // update next run (if required)
            CalculateNextStartTime(schedule.ScheduleInfo);
            
            // disable run once task
            if (schedule.ScheduleInfo.ScheduleType == ScheduleType.OneTime)
                schedule.ScheduleInfo.Enabled = false;

            schedule.ScheduleInfo.LastRun = DateTime.Now;
            UpdateSchedule(schedule.ScheduleInfo);

            return 0;
        }        

        public static int StopSchedule(int scheduleId)
        {
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);

            if (accountCheck < 0)
                return accountCheck;
            
            SchedulerJob schedule = GetScheduleComplete(scheduleId);
            if (schedule == null)
                return 0;

            foreach (BackgroundTask task in TaskController.GetScheduleTasks(scheduleId))
            {
                task.Status = BackgroundTaskStatus.Stopping;
                
                TaskController.UpdateTask(task);
            }
            
            return 0;

        }

        public static void CalculateNextStartTime(ScheduleInfo schedule)
        {
            if (schedule.ScheduleType == ScheduleType.OneTime)
            {
                // start time stay intact
                // we only disable this task for the next time
                schedule.NextRun = schedule.StartTime;
            }
            else if (schedule.ScheduleType == ScheduleType.Interval)
            {
                DateTime lastRun = schedule.LastRun;
                DateTime now = DateTime.Now;

                // the task is running first time by default
                DateTime nextStart = DateTime.Now;

                if (lastRun != DateTime.MinValue)
                {
                    // the task is running next times
                    nextStart = lastRun.AddSeconds(schedule.Interval);
                }

                if (nextStart < now)
                    nextStart = now; // run immediately

                // check if start time is in allowed interval
                DateTime fromTime = new DateTime(now.Year, now.Month, now.Day,
                    schedule.FromTime.Hour, schedule.FromTime.Minute, schedule.FromTime.Second);

                DateTime toTime = new DateTime(now.Year, now.Month, now.Day,
                    schedule.ToTime.Hour, schedule.ToTime.Minute, schedule.ToTime.Second);

                if (!(nextStart >= fromTime && nextStart <= toTime))
                {
                    // run task in the start of the interval, but only tomorrow
                    nextStart = fromTime.AddDays(1);
                }
                schedule.NextRun = nextStart;
            }
            else if (schedule.ScheduleType == ScheduleType.Daily)
            {
                DateTime now = DateTime.Now;
                DateTime startTime = schedule.StartTime;
                DateTime nextStart = new DateTime(now.Year, now.Month, now.Day,
                    startTime.Hour, startTime.Minute, startTime.Second);
                if (nextStart < now) // start time is in the past
                    nextStart = nextStart.AddDays(1); // run tomorrow
                schedule.NextRun = nextStart;
            }
            else if (schedule.ScheduleType == ScheduleType.Weekly)
            {
                DateTime now = DateTime.Now;
                DateTime startTime = schedule.StartTime;
                DateTime nextStart = new DateTime(now.Year, now.Month, now.Day,
                    startTime.Hour, startTime.Minute, startTime.Second);
                int todayWeekDay = (int)now.DayOfWeek;
                nextStart = nextStart.AddDays(schedule.WeekMonthDay - todayWeekDay);

                if (nextStart < now) // start time is in the past
                    nextStart = nextStart.AddDays(7); // run next week
                schedule.NextRun = nextStart;
            }
            else if (schedule.ScheduleType == ScheduleType.Monthly)
            {
                DateTime now = DateTime.Now;
                DateTime startTime = schedule.StartTime;
                DateTime nextStart = new DateTime(now.Year, now.Month, now.Day,
                    startTime.Hour, startTime.Minute, startTime.Second);
                int todayDay = now.Day;
                nextStart = nextStart.AddDays(schedule.WeekMonthDay - todayDay);

                if (nextStart < now) // start time is in the past
                    nextStart = nextStart.AddMonths(1); // run next month
                schedule.NextRun = nextStart;
            }
        }

        public static int AddSchedule(ScheduleInfo schedule)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check quota
            if (PackageController.GetPackageQuota(schedule.PackageId, Quotas.OS_SCHEDULEDTASKS).QuotaExhausted)
                return BusinessErrorCodes.ERROR_OS_SCHEDULED_TASK_QUOTA_LIMIT;

            CalculateNextStartTime(schedule);

            string xmlParameters = BuildParametersXml(schedule.Parameters);

            int scheduleId = DataProvider.AddSchedule(SecurityContext.User.UserId,
                schedule.TaskId, schedule.PackageId, schedule.ScheduleName, schedule.ScheduleTypeId,
                schedule.Interval, schedule.FromTime, schedule.ToTime, schedule.StartTime,
                schedule.NextRun, schedule.Enabled, schedule.PriorityId,
                schedule.HistoriesNumber, schedule.MaxExecutionTime, schedule.WeekMonthDay, xmlParameters);

            // re-schedule tasks
            //Scheduler.ScheduleTasks();

            return scheduleId;
        }

        public static int UpdateSchedule(ScheduleInfo schedule)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load original schedule
            ScheduleInfo originalSchedule = GetScheduleInternal(schedule.ScheduleId);

            schedule.LastRun = schedule.LastRun;
            CalculateNextStartTime(schedule);

            string xmlParameters = BuildParametersXml(schedule.Parameters);

            DataProvider.UpdateSchedule(SecurityContext.User.UserId,
                schedule.ScheduleId, schedule.TaskId, schedule.ScheduleName, schedule.ScheduleTypeId,
                schedule.Interval, schedule.FromTime, schedule.ToTime, schedule.StartTime,
                schedule.LastRun, schedule.NextRun, schedule.Enabled, schedule.PriorityId,
                schedule.HistoriesNumber, schedule.MaxExecutionTime, schedule.WeekMonthDay, xmlParameters);

            // re-schedule tasks
            //Scheduler.ScheduleTasks();

            return 0;
        }

        private static string BuildParametersXml(ScheduleTaskParameterInfo[] parameters)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement nodeProps = doc.CreateElement("parameters");
            if (parameters != null)
            {
                foreach (ScheduleTaskParameterInfo parameter in parameters)
                {
                    XmlElement nodeProp = doc.CreateElement("parameter");
                    nodeProp.SetAttribute("id", parameter.ParameterId);
                    nodeProp.SetAttribute("value", parameter.ParameterValue);
                    nodeProps.AppendChild(nodeProp);
                }
            }
            return nodeProps.OuterXml;
        }

        public static int DeleteSchedule(int scheduleId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // stop schedule if active
            StopSchedule(scheduleId);

            // delete schedule
            DataProvider.DeleteSchedule(SecurityContext.User.UserId, scheduleId);

            // re-schedule tasks
            //Scheduler.ScheduleTasks();

            return 0;
        }
    }
}
