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
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SolidCP.EnterpriseServer
{
    public class ScheduleInfo
    {
        private int scheduleId;
        private string taskId;
        private int packageId;
        private string scheduleName;
        private string scheduleTypeId;
        private int interval;
        private DateTime fromTime;
        private DateTime toTime;
        private DateTime startTime;
        private DateTime lastRun;
        private DateTime nextRun;
        private bool enabled;
        private string statusId;
        private ScheduleTaskParameterInfo[] parameters;
        private string priorityId;
        private int historiesNumber;
        private int maxExecutionTime;
        private int weekMonthDay;

        public int ScheduleId
        {
            get { return this.scheduleId; }
            set { this.scheduleId = value; }
        }

        public string TaskId
        {
            get { return this.taskId; }
            set { this.taskId = value; }
        }

        public int PackageId
        {
            get { return this.packageId; }
            set { this.packageId = value; }
        }

        public string ScheduleTypeId
        {
            get { return this.scheduleTypeId; }
            set { this.scheduleTypeId = value; }
        }

        public string ScheduleName
        {
            get { return this.scheduleName; }
            set { this.scheduleName = value; }
        }

        [XmlIgnore]
        public ScheduleType ScheduleType
        {
            get { return (ScheduleType)Enum.Parse(typeof(ScheduleType), scheduleTypeId, true); }
            set { scheduleTypeId = value.ToString(); }
        }

        public int Interval
        {
            get { return this.interval; }
            set { this.interval = value; }
        }

        public System.DateTime FromTime
        {
            get { return this.fromTime; }
            set { this.fromTime = value; }
        }

        public System.DateTime ToTime
        {
            get { return this.toTime; }
            set { this.toTime = value; }
        }

        public System.DateTime StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }

        public System.DateTime LastRun
        {
            get { return this.lastRun; }
            set { this.lastRun = value; }
        }

        public System.DateTime NextRun
        {
            get { return this.nextRun; }
            set { this.nextRun = value; }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        public string StatusId
        {
            get { return this.statusId; }
            set { this.statusId = value; }
        }

        [XmlIgnore]
        public ScheduleStatus Status
        {
            get { return (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), statusId, true); }
            set { statusId = value.ToString(); }
        }

        public ScheduleTaskParameterInfo[] Parameters
        {
            get { return this.parameters; }
            set { this.parameters = value; }
        }

        public int WeekMonthDay
        {
            get { return this.weekMonthDay; }
            set { this.weekMonthDay = value; }
        }

        public string PriorityId
        {
            get { return this.priorityId; }
            set { this.priorityId = value; }
        }

        [XmlIgnore]
        public SchedulePriority Priority
        {
            get { return (SchedulePriority)Enum.Parse(typeof(SchedulePriority), priorityId, true); }
            set { priorityId = value.ToString(); }
        }

        public int HistoriesNumber
        {
            get { return this.historiesNumber; }
            set { this.historiesNumber = value; }
        }

        public int MaxExecutionTime
        {
            get { return this.maxExecutionTime; }
            set { this.maxExecutionTime = value; }
        }
    }
}
