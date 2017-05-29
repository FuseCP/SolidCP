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
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Reflection;
using SolidCP.Providers.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;

namespace SolidCP.EnterpriseServer
{
    public class TaskManager
    {
        private static Hashtable eventHandlers = null;
        //using id instead of guid
        private static ConcurrentDictionary<int, Thread> _taskThreadsDictionary = new ConcurrentDictionary<int, Thread>();

        // purge timer, used for killing old tasks from the hash
        static Timer purgeTimer = new Timer(new TimerCallback(PurgeCompletedTasks),
                                            null,
                                            60000, // start from 1 minute
                                            60000); // invoke each minute

        public static Guid Guid
        {
            get
            {
                Guid? guid = (Guid?)Thread.GetData(Thread.GetNamedDataSlot("BackgroundTaskGuid"));
                if (!guid.HasValue)
                {
                    guid = Guid.NewGuid();
                    Thread.SetData(Thread.GetNamedDataSlot("BackgroundTaskGuid"), guid.Value);
                }

                return guid.Value;
            }
        }

        public static void StartTask(string source, string taskName)
        {
            StartTask(source, taskName, 0);
        }

        public static void StartTask(string source, string taskName, int itemId)
        {
            StartTask(source, taskName, itemId, new List<BackgroundTaskParameter>());
        }

        public static void StartTask(string source, string taskName, int itemId, BackgroundTaskParameter parameter)
        {
            StartTask(source, taskName, null, itemId, parameter);
        }

        public static void StartTask(string source, string taskName, int itemId, List<BackgroundTaskParameter> parameters)
        {
            StartTask(source, taskName, null, itemId, parameters);
        }

        public static void StartTask(string source, string taskName, object itemName)
        {
            StartTask(source, taskName, itemName, 0);
        }

        public static void StartTask(string source, string taskName, object itemName, int itemId)
        {
            StartTask(source, taskName, itemName, itemId, new List<BackgroundTaskParameter>());
        }

        public static void StartTask(string source, string taskName, object itemName, BackgroundTaskParameter parameter)
        {
            StartTask(source, taskName, itemName, 0, parameter);
        }

        public static void StartTask(string source, string taskName, object itemName, List<BackgroundTaskParameter> parameters)
        {
            StartTask(source, taskName, itemName, 0, parameters);
        }

        public static void StartTask(string source, string taskName, object itemName, int itemId, BackgroundTaskParameter parameter)
        {
            StartTask(source, taskName, itemName, itemId, 0, parameter);
        }

        public static void StartTask(string source, string taskName, object itemName, int itemId, List<BackgroundTaskParameter> parameters)
        {
            StartTask(source, taskName, itemName, itemId, 0, 0, -1, parameters);
        }

        public static void StartTask(string source, string taskName, object itemName, int itemId, int packageId, BackgroundTaskParameter parameter)
        {
            List<BackgroundTaskParameter> parameters = new List<BackgroundTaskParameter>();
            if (parameter != null)
            {
                parameters.Add(parameter);
            }

            StartTask(source, taskName, itemName, itemId, 0, packageId, -1, parameters);
        }

        public static void StartTask(string taskId, string source, string taskName, object itemName, int itemId)
        {
            StartTask(taskId, source, taskName, itemName, itemId, 0, 0, -1, new List<BackgroundTaskParameter>());
        }

        public static void StartTask(string taskId, string source, string taskName, object itemName, int itemId, int packageId)
        {
            StartTask(taskId, source, taskName, itemName, itemId, 0, packageId, -1, new List<BackgroundTaskParameter>());
        }

        public static void StartTask(string source, string taskName, object itemName, int itemId,
            int scheduleId, int packageId, int maximumExecutionTime, List<BackgroundTaskParameter> parameters)
        {
            StartTask(null, source, taskName, itemName, itemId, scheduleId, packageId, maximumExecutionTime, parameters);
        }

        public static void StartTask(string taskId, string source, string taskName, object itemName, int itemId,
            int scheduleId, int packageId, int maximumExecutionTime, List<BackgroundTaskParameter> parameters)
        {
            if (String.IsNullOrEmpty(taskId))
            {
                taskId = Guid.NewGuid().ToString("N");
            }

            var user = SecurityContext.User;

            int userId = user.OwnerId == 0
                             ? user.UserId
                             : user.OwnerId;

            int effectiveUserId = user.UserId;

            String itemNameStr = itemName != null
                ? itemName.ToString()
                : String.Empty; //: itemId > 0 ? "(Id = " + itemId + ")" : String.Empty;
            BackgroundTask task = new BackgroundTask(Guid, taskId, userId, effectiveUserId, source, taskName, itemNameStr,
                                                     itemId, scheduleId, packageId, maximumExecutionTime, parameters);


            List<BackgroundTask> tasks = TaskController.GetTasks(Guid);

            if (tasks.Count > 0)
            {
                BackgroundTask rootTask = tasks[0];

                BackgroundTaskLogRecord log = new BackgroundTaskLogRecord(
                    rootTask.Id,
                    tasks.Count - 1,
                    true,
                    String.Format("{0}_{1}", source, taskName),
                    new string[] { itemNameStr });


                TaskController.AddLog(log);
            }

            // call event handler
            CallTaskEventHandler(task, false);

            AddTaskThread(TaskController.AddTask(task), Thread.CurrentThread);
        }

        public static void WriteParameter(string parameterName, object parameterValue)
        {
            string val = parameterValue != null ? parameterValue.ToString() : "";
            WriteLogRecord(Guid, 0, parameterName + ": " + val, null, null);
        }

        public static void Write(string text, params string[] textParameters)
        {
            // INFO
            WriteLogRecord(Guid, 0, text, null, textParameters);
        }

        public static void WriteWarning(string text, params string[] textParameters)
        {
            WriteWarning(Guid, text, textParameters);
        }

        public static void WriteWarning(Guid guid, string text, params string[] textParameters)
        {
            // WARNING
            WriteLogRecord(guid, 1, text, null, textParameters);
        }

        public static Exception WriteError(Exception ex)
        {
            // ERROR
            WriteLogRecord(Guid, 2, ex.Message, ex.StackTrace);

            return new Exception((TopTask != null)
                                     ? String.Format("Error executing '{0}' task on '{1}' {2}",
                                                     TopTask.TaskName, TopTask.ItemName, TopTask.Source)
                                     : String.Format("Error executing task"), ex);
        }

        public static void WriteError(Exception ex, string text, params string[] textParameters)
        {
            // ERROR
            string[] prms = new string[] { ex.Message };
            if (textParameters != null && textParameters.Length > 0)
            {
                prms = new string[textParameters.Length + 1];
                Array.Copy(textParameters, 0, prms, 1, textParameters.Length);
                prms[0] = ex.Message;
            }

            WriteLogRecord(Guid, 2, text, ex.Message + "\n" + ex.StackTrace, prms);
        }

        public static void WriteError(string text, params string[] textParameters)
        {
            // ERROR
            WriteLogRecord(Guid, 2, text, null, textParameters);
        }

        private static void WriteLogRecord(Guid guid, int severity, string text, string stackTrace, params string[] textParameters)
        {
            List<BackgroundTask> tasks = TaskController.GetTasks(guid);

            if (tasks.Count > 0)
            {
                BackgroundTask rootTask = tasks[0];

                BackgroundTaskLogRecord log = new BackgroundTaskLogRecord(
                    rootTask.Id,
                    tasks.Count - 1,
                    false,
                    text,
                    stackTrace,
                    textParameters);

                TaskController.AddLog(log);

                if (severity > rootTask.Severity)
                {
                    rootTask.Severity = severity;

                    TaskController.UpdateTask(rootTask);
                }
            }
        }

        public static void CompleteTask()
        {
            List<BackgroundTask> tasks = TaskController.GetTasks(Guid);

            if (tasks.Count == 0)
                return;

            BackgroundTask topTask = tasks[tasks.Count - 1];

            // call event handler
            CallTaskEventHandler(topTask, true);

            // finish task
            topTask.FinishDate = DateTime.Now;
            topTask.Completed = true;

            // write task execution result to database
            if (tasks.Count == 1) // single task
            {
                // write to database
                AddAuditLog(topTask);
            }

            TaskController.UpdateTask(topTask);
            StopProcess(topTask);
        }

        public static void UpdateParam(String name, Object value)
        {
            BackgroundTask topTask = TopTask;

            if (topTask == null)
                return;

            topTask.UpdateParamValue(name, value);

            TaskController.UpdateTaskWithParams(topTask);
        }

        public static int ItemId
        {
            set
            {
                BackgroundTask topTask = TopTask;

                if (topTask == null)
                    return;

                topTask.ItemId = value;

                TaskController.UpdateTask(topTask);
            }
        }

        public static String ItemName
        {
            set
            {
                BackgroundTask topTask = TopTask;

                if (topTask == null)
                    return;

                topTask.ItemName = value;

                TaskController.UpdateTask(topTask);
            }
        }

        public static void UpdateParams(Hashtable parameters)
        {
            BackgroundTask topTask = TopTask;

            if (topTask == null)
                return;

            foreach (string key in parameters.Keys)
            {
                topTask.UpdateParamValue(key, parameters[key]);
            }

            TaskController.UpdateTaskWithParams(topTask);
        }

        static string FormatExecutionLog(BackgroundTask task)
        {
            StringWriter sw = new StringWriter();
            XmlWriter writer = new XmlTextWriter(sw);

            writer.WriteStartElement("log");

            // parameters
            writer.WriteStartElement("parameters");
            foreach (BackgroundTaskParameter param in task.Params)
            {
                writer.WriteStartElement("parameter");
                writer.WriteAttributeString("name", param.Name);
                writer.WriteString(param.Value.ToString());

                writer.WriteEndElement();
            }
            writer.WriteEndElement(); // parameters

            // records
            writer.WriteStartElement("records");
            foreach (BackgroundTaskLogRecord record in task.Logs)
            {
                writer.WriteStartElement("record");
                writer.WriteAttributeString("severity", record.Severity.ToString());
                writer.WriteAttributeString("date", record.Date.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteAttributeString("ident", record.TextIdent.ToString());

                // text
                writer.WriteElementString("text", record.Text);

                // text parameters
                if (record.TextParameters != null && record.TextParameters.Length > 0)
                {
                    writer.WriteStartElement("textParameters");
                    foreach (string prm in record.TextParameters)
                    {
                        writer.WriteElementString("value", prm);
                    }
                    writer.WriteEndElement(); // textParameters
                }

                // stack trace
                writer.WriteElementString("stackTrace", record.ExceptionStackTrace);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();

            return sw.ToString();
        }

        static void PurgeCompletedTasks(object obj)
        {
            List<BackgroundTask> tasks = TaskController.GetProcessTasks(BackgroundTaskStatus.Run);

            foreach (BackgroundTask task in tasks)
            {
                if (task.MaximumExecutionTime != -1
                    && ((TimeSpan)(DateTime.Now - task.StartDate)).TotalSeconds > task.MaximumExecutionTime)
                {
                    task.Status = BackgroundTaskStatus.Stopping;

                    TaskController.UpdateTask(task);
                }
            }
        }

        public static int IndicatorMaximum
        {
            set
            {
                BackgroundTask topTask = TopTask;

                if (topTask == null)
                {
                    return;
                }

                topTask.IndicatorMaximum = value;

                TaskController.UpdateTask(topTask);
            }
        }

        public static int IndicatorCurrent
        {
            get
            {
                return TopTask.IndicatorCurrent;
            }
            set
            {
                BackgroundTask topTask = TopTask;

                if (topTask == null)
                {
                    return;
                }

                topTask.IndicatorCurrent = value;

                TaskController.UpdateTask(topTask);
            }
        }

        public static int MaximumExecutionTime
        {
            get
            {
                return TopTask.MaximumExecutionTime;
            }
            set
            {
                BackgroundTask topTask = TopTask;

                if (topTask == null)
                {
                    return;
                }

                topTask.MaximumExecutionTime = value;

                TaskController.UpdateTask(topTask);
            }
        }

        public static bool HasErrors(BackgroundTask task)
        {
            return task.Severity == 2;
        }

        public static BackgroundTask TopTask
        {
            get { return TaskController.GetTopTask(Guid); }
        }

        public static BackgroundTask GetTask(string taskId)
        {
            BackgroundTask task = TaskController.GetTask(taskId);

            if (task == null)
                return null;

            return task;
        }

        public static BackgroundTask GetTaskWithLogRecords(string taskId, DateTime startLogTime)
        {
            BackgroundTask task = GetTask(taskId);

            if (task == null)
                return null;

            task.Logs = TaskController.GetLogs(task, startLogTime);

            return task;
        }

        public static Dictionary<int, BackgroundTask> GetScheduledTasks()
        {
            Dictionary<int, BackgroundTask> scheduledTasks = new Dictionary<int, BackgroundTask>();
            try
            {
                foreach (BackgroundTask task in TaskController.GetTasks())
                {
                    if (task.ScheduleId > 0
                        && !task.Completed
                        && (task.Status == BackgroundTaskStatus.Run || task.Status == BackgroundTaskStatus.Starting)
                        && !scheduledTasks.ContainsKey(task.ScheduleId))
                        scheduledTasks.Add(task.ScheduleId, task);
                }
            }
            catch { }

            return scheduledTasks;
        }

        public static void SetTaskNotifyOnComplete(string taskId)
        {
            BackgroundTask task = GetTask(taskId);

            if (task == null)
                return;

            task.NotifyOnComplete = true;
        }

        internal static void AddTaskThread(int taskId, Thread taskThread)
        {
            if (_taskThreadsDictionary.ContainsKey(taskId))
                _taskThreadsDictionary[taskId] = taskThread;
            else
                _taskThreadsDictionary.AddOrUpdate(taskId, taskThread, (key, oldValue) => taskThread);
        }

        public static void StopTask(string taskId)
        {
            BackgroundTask task = GetTask(taskId);

            if (task == null)
            {
                return;
            }

            task.Status = BackgroundTaskStatus.Abort;

            StopProcess(task);

            if (!HasErrors(task))
            {
                task.Severity = 1;
            }

            task.FinishDate = DateTime.Now;

            WriteWarning(task.Guid, "Task aborted by user");

            AddAuditLog(task);

            TaskController.UpdateTask(task);
        }

        private static void StopProcess(BackgroundTask task)
        {
                if (_taskThreadsDictionary.ContainsKey(task.Id))
                {
                    if (_taskThreadsDictionary[task.Id] != null)
                        if (_taskThreadsDictionary[task.Id].IsAlive)
                        {
                            if (!task.Completed)
                                _taskThreadsDictionary[task.Id].Abort();
                            _taskThreadsDictionary[task.Id] = null;
                        }
                    Thread deleted;
                    _taskThreadsDictionary.TryRemove(task.Id, out deleted);
                }
        }

        private static void AddAuditLog(BackgroundTask task)
        {
            task.Logs = TaskController.GetLogs(task, task.StartDate);

            string executionLog = FormatExecutionLog(task);

            UserInfo user = UserController.GetUserInternally(task.EffectiveUserId);
            string username = user != null ? user.Username : null;

            AuditLog.AddAuditLogRecord(task.TaskId, task.Severity, task.EffectiveUserId,
                                       username, task.PackageId, task.ItemId,
                                       task.ItemName, task.StartDate, task.FinishDate, task.Source,
                                       task.TaskName, executionLog);
        }

        public static List<BackgroundTask> GetUserTasks(int userId)
        {
            List<BackgroundTask> list = new List<BackgroundTask>();

            // try to get user first
            UserInfo user = UserController.GetUser(userId);
            if (user == null)
                return list; // prohibited user

            // get user tasks
            foreach (BackgroundTask task in TaskController.GetTasks(user.IsPeer ? user.OwnerId : user.UserId))
            {
                if (task.UserId == userId && !task.Completed
                    && task.Status == BackgroundTaskStatus.Run)
                {
                    list.Add(task);
                }
            }
            return list;
        }

        public static List<BackgroundTask> GetUserCompletedTasks(int userId)
        {
            return new List<BackgroundTask>();
        }

        public static int GetTasksNumber()
        {
            return TaskController.GetTasks().Count;
        }

        #region Private Helpers

        private static void CallTaskEventHandler(BackgroundTask task, bool onComplete)
        {
            string[] taskHandlers = GetTaskEventHandlers(task.Source, task.TaskName);
            if (taskHandlers != null)
            {
                foreach (string taskHandler in taskHandlers)
                {
                    try
                    {
                        Type handlerType = Type.GetType(taskHandler);
                        TaskEventHandler handler = (TaskEventHandler)Activator.CreateInstance(handlerType);

                        if (handler != null)
                        {
                            if (onComplete)
                                handler.OnComplete();
                            else
                                handler.OnStart();
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteError(ex, "Error executing task event handler: {0}", ex.Message);
                    }
                }
            }
        }

        private static string[] GetTaskEventHandlers(string source, string taskName)
        {
            // load configuration
            string appRoot = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(appRoot, "TaskEventHandlers.config");

            if (eventHandlers == null)
            {
                eventHandlers = Hashtable.Synchronized(new Hashtable());

                // load from XML
                if (File.Exists(path))
                {
                    List<XmlDocument> xmlConfigs = new List<XmlDocument>();
                    xmlConfigs.Add(new XmlDocument());
                    xmlConfigs[0].Load(path);
                    // Lookup for external references first
                    XmlNodeList xmlReferences = xmlConfigs[0].SelectNodes("//reference");
                    foreach (XmlElement xmlReference in xmlReferences)
                    {
                        string referencePath = Path.Combine(appRoot, xmlReference.GetAttribute("src"));
                        if (File.Exists(referencePath))
                        {
                            XmlDocument xmldoc = new XmlDocument();
                            xmldoc.Load(referencePath);
                            xmlConfigs.Add(xmldoc);
                        }
                    }

                    // parse XML
                    foreach (XmlDocument xml in xmlConfigs)
                    {
                        XmlNodeList xmlHandlers = xml.SelectNodes("//handler");
                        foreach (XmlNode xmlHandler in xmlHandlers)
                        {
                            string keyName = xmlHandler.ParentNode.Attributes["source"].Value
                                + xmlHandler.ParentNode.Attributes["name"].Value;

                            // get handlers collection
                            List<string> taskHandlers = (List<string>)eventHandlers[keyName];
                            if (taskHandlers == null)
                            {
                                taskHandlers = new List<string>();
                                eventHandlers[keyName] = taskHandlers;
                            }

                            string handlerType = xmlHandler.Attributes["type"].Value;
                            taskHandlers.Add(handlerType);
                        }
                    }
                }
            }

            string fullTaskName = source + taskName;
            List<string> handlersList = (List<string>)eventHandlers[fullTaskName];
            return handlersList == null ? null : handlersList.ToArray();
        }

        #endregion


        #region ResultTasks

        public static void CompleteResultTask(ResultObject res, string errorCode, Exception ex, string errorMessage)
        {
            if (res != null)
            {
                res.IsSuccess = false;
                if (!string.IsNullOrEmpty(errorCode))
                    res.ErrorCodes.Add(errorCode);
            }

            if (ex != null)
                TaskManager.WriteError(ex);

            if (!string.IsNullOrEmpty(errorMessage))
                TaskManager.WriteError(errorMessage);

            //LogRecord.
            CompleteTask();


        }

        public static void CompleteResultTask(ResultObject res, string errorCode, Exception ex)
        {
            CompleteResultTask(res, errorCode, ex, null);
        }

        public static void CompleteResultTask(ResultObject res, string errorCode)
        {
            CompleteResultTask(res, errorCode, null, null);
        }

        public static void CompleteResultTask(ResultObject res)
        {
            CompleteResultTask(res, null);
        }

        public static void CompleteResultTask()
        {
            CompleteResultTask(null);
        }

        public static T StartResultTask<T>(string source, string taskName) where T : ResultObject, new()
        {
            StartTask(source, taskName);
            T res = new T();
            res.IsSuccess = true;
            return res;
        }

        public static T StartResultTask<T>(string source, string taskName, object itemName) where T : ResultObject, new()
        {
            StartTask(source, taskName, itemName);
            T res = new T();
            res.IsSuccess = true;
            return res;
        }

        public static T StartResultTask<T>(string source, string taskName, object itemName, int packageId) where T : ResultObject, new()
        {
            StartTask(source, taskName, itemName, 0, packageId, null);
            T res = new T();
            res.IsSuccess = true;
            return res;
        }

        public static T StartResultTask<T>(string source, string taskName, int packageId) where T : ResultObject, new()
        {
            StartTask(source, taskName, null, 0, packageId, null);
            T res = new T();
            res.IsSuccess = true;
            return res;
        }

        public static T StartResultTask<T>(string source, string taskName, int itemId, BackgroundTaskParameter parameter) where T : ResultObject, new()
        {
            StartTask(source, taskName, itemId, parameter);
            T res = new T();
            res.IsSuccess = true;
            return res;
        }

        public static T StartResultTask<T>(string source, string taskName, int itemId, List<BackgroundTaskParameter> parameters) where T : ResultObject, new()
        {
            StartTask(source, taskName, itemId, parameters);
            T res = new T();
            res.IsSuccess = true;
            return res;
        }

        public static T StartResultTask<T>(string source, string taskName, int itemId, object itemName, int packageId) where T : ResultObject, new()
        {
            StartTask(source, taskName, itemName, itemId, packageId, null);
            T res = new T();
            res.IsSuccess = true;
            return res;
        }

        public static T StartResultTask<T>(string source, string taskName, Guid taskId, object itemName, int packageId) where T : ResultObject, new()
        {
            StartTask(taskId.ToString(), source, taskName, itemName, 0, packageId);
            T res = new T();
            res.IsSuccess = true;
            return res;
        }

        public static T StartResultTask<T>(string source, string taskName, Guid taskId, int itemId, object itemName, int packageId) where T : ResultObject, new()
        {
            StartTask(taskId.ToString(), source, taskName, itemName, itemId, packageId);
            T res = new T();
            res.IsSuccess = true;
            return res;
        }

        #endregion

        #region Log Advanced

        
        #endregion

    }
}

