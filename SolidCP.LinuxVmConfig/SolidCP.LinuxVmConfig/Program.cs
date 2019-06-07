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
using System.Configuration;
using System.Collections.Generic;
using System.Timers;
using System.Text;

namespace SolidCP.LinuxVmConfig
{
    class Program
    {
        private static System.Threading.Thread mainThread;

        internal const string TaskPrefix = "SCP-";
        internal const string CurrentTaskName = "SCP-CurrentTask";

        internal const string DEFAULT_KVP_DIRECTORY = "/var/lib/hyperv/";
        internal const string KVP_BASEFILENAME = ".kvp_pool_";
        internal const string InputKVP = DEFAULT_KVP_DIRECTORY + KVP_BASEFILENAME + "0";
        internal const string OutputKVP = DEFAULT_KVP_DIRECTORY + KVP_BASEFILENAME + "1";

        private static Dictionary<string, string> provisioningModules;

        private static Timer idleTimer;
        private static int pollInterval = 60000;

        private static bool rebootRequired = false;

        static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                if (arg.Trim().Equals("install"))
                {
                    InstallService();
                    return;
                }
            }
            mainThread = new System.Threading.Thread(new System.Threading.ThreadStart(Start));
            mainThread.Start();
        }

        private static void InstallService()
        {
            const string serviceFileName = "SolidCP.service";
            const string serviceSystemPath = "/etc/systemd/system/";
            string userName = Environment.UserName;
            string path = Environment.CurrentDirectory;

            ShellHelper.RunCmd("sudo systemctl stop " + serviceFileName);
            ShellHelper.RunCmd("sudo systemctl disable " + serviceFileName);

            List<string> config = new List<string>();
            config.Add("[Unit]");
            config.Add("Description=SolidCP LinuxVmConfig Service");
            config.Add("[Service]");
            config.Add("User="+userName);
            config.Add("WorkingDirectory="+path);
            config.Add("ExecStart="+path+"/SolidCP.LinuxVmConfig");
            config.Add("SuccessExitStatus=0");
            config.Add("TimeoutStopSec=infinity");
            config.Add("Restart=on-failure");
            config.Add("RestartSec=5");
            config.Add("[Install]");
            config.Add("WantedBy=multi-user.target");
            File.WriteAllLines(serviceSystemPath + serviceFileName, config);

            ExecutionResult res = ShellHelper.RunCmd("sudo systemctl daemon-reload");
            if (res.ResultCode == 1)
            {
                ServiceLog.WriteError("Service install error: " + res.ErrorMessage);
                return;
            }
            res = ShellHelper.RunCmd("sudo systemctl enable "+ serviceFileName);
            if (res.ResultCode == 1)
            {
                ServiceLog.WriteError("Service install error: " + res.ErrorMessage);
                return;
            }
            res = ShellHelper.RunCmd("sudo systemctl start " + serviceFileName);
            if (res.ResultCode == 1)
            {
                ServiceLog.WriteError("Service install error: " + res.ErrorMessage);
                return;
            }
            ServiceLog.WriteInfo(serviceFileName + " successfully installed.");
        }

        private static void Start()
        {
            //log
            ServiceLog.WriteApplicationStart();

            // delay (for sync with KVP exchange service)
            DelayOnStart();

            //init
            InitializeProvisioningModules();
            InitializeTimers();

            //run tasks
            while (true)
            {
                ProcessTasks();
                System.Threading.Thread.Sleep(pollInterval);
            }
        }

        private static void DelayOnStart()
        {
            int startupDelay = 0;
            if (Int32.TryParse(ConfigurationManager.AppSettings["Service.StartupDelay"], out startupDelay)
                && startupDelay > 0)
            {
                ServiceLog.WriteStart("Delay on service start-up");
                System.Threading.Thread.Sleep(startupDelay);
                ServiceLog.WriteEnd("Delay on service start-up");
            }
        }

        private static void InitializeProvisioningModules()
        {
            ServiceLog.WriteStart("Loading provisioning modules...");
            provisioningModules = new Dictionary<string, string>();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ModuleSettingsSection section = config.Sections["moduleSettings"] as ModuleSettingsSection;
            if (section != null)
            {
                foreach (string key in section.Modules.AllKeys)
                {
                    provisioningModules.Add(key, section.Modules[key].Value);
                }
            }
            else
                ServiceLog.WriteError("Modules configuration section not found");
            ServiceLog.WriteEnd(string.Format("{0} module(s) loaded", provisioningModules.Count));
        }

        private static void InitializeTimers()
        {
            // idle timer
            idleTimer = new Timer();
            idleTimer.AutoReset = false;
            double idleInterval;
            if (!Double.TryParse(ConfigurationManager.AppSettings["Service.ExitIdleInterval"], out idleInterval))
            {
                ServiceLog.WriteError("Invalid configuration parameter: Service.ExitIdleInterval");
                idleInterval = 600000;
            }
            idleTimer.Interval = idleInterval;
            idleTimer.Enabled = false;
            idleTimer.Elapsed += new ElapsedEventHandler(OnIdleTimerElapsed);

            if (!Int32.TryParse(ConfigurationManager.AppSettings["Service.RegistryPollInterval"], out pollInterval))
            {
                ServiceLog.WriteError("Invalid configuration parameter: Service.RegistryPollInterval");
                pollInterval = 60000;
            }
        }

        private static void OnIdleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Environment.Exit(0);
        }

        private static void ProcessTasks()
        {
            // delete old results
            DeleteOldResults();

            //process all tasks
            while (true)
            {
                //load completed tasks results
                string[] strResults = KvpUtils.GetKvpKeys(OutputKVP);
                if (strResults == null) break;
                List<string> results = new List<string>();
                foreach (string strResult in strResults)
                {
                    if (!string.IsNullOrEmpty(strResult) && strResult.StartsWith(TaskPrefix) && strResult != CurrentTaskName)
                    {
                        //save only SolidCP tasks
                        results.Add(strResult);
                    }
                }

                // sorted list of tasks - will be sorted by the TaskID (time in ticks)
                SortedList<long, string> tasks = new SortedList<long, string>();

                //load task definitions from input registry key 
                string[] strTasks = KvpUtils.GetKvpKeys(InputKVP);
                foreach (string strTask in strTasks)
                {
                    if (results.Contains(strTask))
                        continue; // skip completed tasks

                    if (!string.IsNullOrEmpty(strTask) && strTask.StartsWith(TaskPrefix))
                    {
                        // extract Task ID parameter
                        int idx = strTask.LastIndexOf('-');
                        if (idx == -1)
                            continue;

                        string strTaskId = strTask.Substring(idx + 1);
                        long taskId = 0;
                        try
                        {
                            taskId = Int64.Parse(strTaskId);
                        }
                        catch
                        {
                            continue; // wrong task format
                        }

                        //save only SolidCP tasks
                        if (!tasks.ContainsKey(taskId))
                            tasks.Add(taskId, strTask);
                    }
                }
                if (tasks.Count == 0)
                {
                    if (rebootRequired)
                    {
                        ServiceLog.WriteInfo("Reboot required");
                        RebootSystem();
                        return;
                    }

                    StartIdleTimer(); //stops service if idle 
                                      //no tasks - exit! 
                    return;
                }
                else
                {
                    //stop idle timer as we need to process tasks
                    StopIdleTimer();
                }

                ExecutionContext context = null;
                foreach (long tid in tasks.Keys)
                {
                    //find first correct task 
                    string taskDefinition = tasks[tid];
                    string taskParameters = KvpUtils.GetKvpStringValue(InputKVP, taskDefinition);
                    if (taskDefinition.LastIndexOf("-") == -1 || taskDefinition.LastIndexOf('-') == taskDefinition.Length - 1)
                    {
                        ServiceLog.WriteError(string.Format("Task was deleted from queue as its definition is invalid : {0}", taskDefinition));
                        //go to next task
                        continue;

                    }
                    string taskName = taskDefinition.Substring(0, taskDefinition.LastIndexOf("-")).Substring(TaskPrefix.Length);
                    string taskId = taskDefinition.Substring(taskDefinition.LastIndexOf('-') + 1);

                    if (!provisioningModules.ContainsKey(taskName))
                    {
                        ServiceLog.WriteError(string.Format("Task was deleted from queue as its definition was not found : {0}", taskName));
                        //go to next task
                        continue;
                    }
                    //prepare execution context for correct task
                    context = new ExecutionContext();
                    context.ActivityID = taskId;
                    context.ActivityName = taskName;
                    ParseParameters(context.Parameters, taskParameters);
                    context.ActivityDefinition = taskDefinition;
                    break;
                }
                if (context != null)
                {
                    string type = provisioningModules[context.ActivityName];
                    ExecutionResult res = null;
                    DateTime start = DateTime.Now;

                    try
                    {
                        //load module and run task
                        ServiceLog.WriteStart(string.Format("Starting '{0}' module...", context.ActivityName));
                        context.Progress = 0;
                        switch (context.ActivityName)
                        {
                            case "ChangeComputerName":
                                res=ChangeComputerName.Run(ref context);
                                break;
                            case "ChangeAdministratorPassword":
                                res=ChangeAdministratorPassword.Run(ref context);
                                break;
                            case "SetupNetworkAdapter":
                                res=SetupNetworkAdapter.Run(ref context);
                                break;
                        }
                        context.Progress = 100;
                        ServiceLog.WriteEnd(string.Format("'{0}' module finished.", context.ActivityName));
                    }
                    catch (Exception ex)
                    {
                        ServiceLog.WriteError("Unhandled exception:", ex);
                        res = new ExecutionResult();
                        res.ResultCode = -1;
                        res.ErrorMessage = string.Format("Unhandled exception : {0}", ex);
                    }
                    DateTime end = DateTime.Now;
                    SaveExecutionResult(context.ActivityDefinition, res, start, end);

                    if (res.RebootRequired)
                        rebootRequired = true;
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        private static void DeleteOldResults()
        {
            // get the list of input tasks
            string[] strTasks = KvpUtils.GetKvpKeys(InputKVP);
            if (strTasks == null) return;
            List<string> tasks = new List<string>();
            foreach (string strTask in strTasks)
            {
                if (!string.IsNullOrEmpty(strTask) && strTask.StartsWith(TaskPrefix) && strTask != CurrentTaskName)
                {
                    //save only SolidCP tasks
                    tasks.Add(strTask);
                }
            }

            // get the list of task results
            int deletedResults = 0;
            string[] strResults = KvpUtils.GetKvpKeys(OutputKVP);
            foreach (string strResult in strResults)
            {
                if (!string.IsNullOrEmpty(strResult) && strResult.StartsWith(TaskPrefix) && strResult != CurrentTaskName)
                {
                    // check if task result exists in the input tasks
                    if (!tasks.Contains(strResult))
                    {
                        KvpUtils.DeleteKvpKey(OutputKVP, strResult);
                        ServiceLog.WriteInfo(string.Format("Deleted activity result: {0}", strResult));
                        deletedResults++;
                    }
                }
            }

            if (deletedResults > 0)
                ServiceLog.WriteEnd(string.Format("{0} result(s) deleted", deletedResults));
        }

        private static void ParseParameters(Dictionary<string, string> dictionary, string parameters)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            if (string.IsNullOrEmpty(parameters))
                return;

            string[] pairs = parameters.Split('|');
            foreach (string pair in pairs)
            {
                string[] parts = pair.Split(new char[] { '=' }, 2);
                if (parts.Length != 2)
                    continue;

                if (!dictionary.ContainsKey(parts[0]))
                    dictionary.Add(parts[0], parts[1]);
            }
        }

        private static void StopIdleTimer()
        {
            idleTimer.Stop();
        }

        private static void StartIdleTimer()
        {
            if (idleTimer.Interval > 1)
                idleTimer.Start();
        }

        private static void SaveExecutionResult(string name, ExecutionResult res, DateTime started, DateTime ended)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ResultCode={0}|", res.ResultCode);
            builder.AppendFormat("RebootRequired={0}|", res.RebootRequired);
            builder.AppendFormat("ErrorMessage={0}|", res.ErrorMessage);
            builder.AppendFormat("Value={0}|", res.Value);
            builder.AppendFormat("Started={0}|", started.ToString("yyyyMMddHHmmss"));
            builder.AppendFormat("Ended={0}", started.ToString("yyyyMMddHHmmss"));
            KvpUtils.SetKvpStringValue(OutputKVP, name, builder.ToString());
        }

        private static void RebootSystem()
        {
            try
            {
                ServiceLog.WriteStart("RebootSystem");
                ShellHelper.RunCmd("sudo reboot");
                ServiceLog.WriteEnd("RebootSystem");
            }
            catch (Exception ex)
            {
                ServiceLog.WriteError("Reboot System error:", ex);
            }
        }
    }
}
