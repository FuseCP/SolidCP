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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SolidCP.EnterpriseServer
{

    public class BackgroundTask
    {
        #region Fields

        public List<BackgroundTaskParameter> Params = new List<BackgroundTaskParameter>();
        
        public List<BackgroundTaskLogRecord> Logs = new List<BackgroundTaskLogRecord>();

        #endregion

        #region Properties

        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string TaskId { get; set; }

        public int ScheduleId { get; set; }

        public int PackageId { get; set; }

        public int UserId { get; set; }

        public int EffectiveUserId { get; set; }

        public string TaskName { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public int IndicatorCurrent { get; set; }

        public int IndicatorMaximum { get; set; }

        public int MaximumExecutionTime { get; set; }

        public string Source { get; set; }

        public int Severity { get; set; }

        public bool Completed { get; set; }

        public bool NotifyOnComplete { get; set; }

        public BackgroundTaskStatus Status { get; set; }

        #endregion

        #region Constructors

        public BackgroundTask()
        {
            StartDate = DateTime.Now;
            Severity = 0;
            IndicatorCurrent = 0;
            IndicatorMaximum = 0;
            Status = BackgroundTaskStatus.Run;

            Completed = false;
            NotifyOnComplete = false;
        }

        public BackgroundTask(Guid guid, String taskId, int userId, int effectiveUserId, String source, String taskName, String itemName,
            int itemId, int scheduleId, int packageId, int maximumExecutionTime, List<BackgroundTaskParameter> parameters)
            : this()
        {
            Guid = guid;
            TaskId = taskId;
            UserId = userId;
            EffectiveUserId = effectiveUserId;
            Source = source;
            TaskName = taskName;
            ItemName = itemName;
            ItemId = itemId;
            ScheduleId = scheduleId;
            PackageId = packageId;
            MaximumExecutionTime = maximumExecutionTime;
            Params = parameters;
        }

        #endregion

        #region Methods

        public List<BackgroundTaskLogRecord> GetLogs()
        {
            return Logs;
        }

        public Object GetParamValue(String name)
        {
            foreach(BackgroundTaskParameter param in Params)
            {
                if (param.Name == name)
                    return param.Value;
            }

            return null;
        }

        public void UpdateParamValue(String name, object value)
        {
            foreach (BackgroundTaskParameter param in Params)
            {
                if (param.Name == name)
                {
                    param.Value = value;

                    return;

                }
            }

            Params.Add(new BackgroundTaskParameter(name, value));
        }

        public bool ContainsParam(String name)
        {
            foreach (BackgroundTaskParameter param in Params)
            {
                if (param.Name == name)
                    return true;
            }

            return false;
        }

        #endregion
    }

    public class BackgroundTaskParameter
    {
        #region Properties

        public int ParameterId { get; set; }

        public int TaskId { get; set; }

        public String Name { get; set; }

        public Object Value { get; set; }

        public String TypeName { get; set; }

        public String SerializerValue { get; set; }

        #endregion

        #region Constructors

        public BackgroundTaskParameter() { }

        public BackgroundTaskParameter(String name, Object value)
        {
            Name = name;
            Value = value;
        }

        #endregion
    }
}
