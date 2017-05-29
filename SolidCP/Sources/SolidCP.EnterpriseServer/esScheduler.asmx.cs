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
using SolidCP.EnterpriseServer.Base.Scheduling;
using Microsoft.Web.Services3;

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esScheduler : System.Web.Services.WebService
    {
        [WebMethod]
        public DateTime GetSchedulerTime()
        {
            return SchedulerController.GetSchedulerTime();
        }

        [WebMethod]
        public List<ScheduleTaskInfo> GetScheduleTasks()
        {
            return SchedulerController.GetScheduleTasks();
        }

		//[WebMethod]
		//public ScheduleTaskInfo GetScheduleTask(string taskId)
		//{
		//    return SchedulerController.GetScheduleTask(taskId);
		//}

    	[WebMethod]
        public DataSet GetSchedules(int userId)
        {
            return SchedulerController.GetSchedules(userId);
        }

        [WebMethod]
        public DataSet GetSchedulesPaged(int packageId, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SchedulerController.GetSchedulesPaged(packageId,
                recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public ScheduleInfo GetSchedule(int scheduleId)
        {
            return SchedulerController.GetSchedule(scheduleId);
        }

        [WebMethod]
        public List<ScheduleTaskParameterInfo> GetScheduleParameters(string taskId, int scheduleId)
        {
            return SchedulerController.GetScheduleParameters(taskId, scheduleId);
        }

		[WebMethod]
		public List<ScheduleTaskViewConfiguration> GetScheduleTaskViewConfigurations(string taskId)
		{
			return SchedulerController.GetScheduleTaskViewConfigurations(taskId);
		}

		[WebMethod]
		public ScheduleTaskViewConfiguration GetScheduleTaskViewConfiguration(string taskId, string environment)
		{
			List<ScheduleTaskViewConfiguration> configurations = SchedulerController.GetScheduleTaskViewConfigurations(taskId);
			return configurations.Find(delegate(ScheduleTaskViewConfiguration configuration)
			                           	{
			                           		return configuration.Environment == environment;
			                           	});
		}


    	[WebMethod]
        public int StartSchedule(int scheduleId)
        {
            return SchedulerController.StartSchedule(scheduleId);
        }

        [WebMethod]
        public int StopSchedule(int scheduleId)
        {
            return SchedulerController.StopSchedule(scheduleId);
        }

        [WebMethod]
        public int AddSchedule(ScheduleInfo schedule)
        {
            return SchedulerController.AddSchedule(schedule);
        }

        [WebMethod]
        public int UpdateSchedule(ScheduleInfo schedule)
        {
            return SchedulerController.UpdateSchedule(schedule);
        }

        [WebMethod]
        public int DeleteSchedule(int scheduleId)
        {
            return SchedulerController.DeleteSchedule(scheduleId);
        }
    }
}
