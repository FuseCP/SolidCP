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

﻿using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using SolidCP.Providers;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Server.Utils;


namespace SolidCP.Providers.HostedSolution
{
    public class BlackBerryProvider : HostingServiceProviderBase, IBlackBerry
    {        
        public string UtilityPath
        {
            get
            {
                return ProviderSettings[Constants.UtilityPath];
            }
        }
        public string HandheldcleanupPath
        {
            get
            {
                return ProviderSettings[Constants.HandheldcleanupPath];
            }
        }
        public string MAPIProfile
        {
            get
            {
                return ProviderSettings[Constants.MAPIProfile];
            }
        }
        public string Password
        {
            get
            {
                return ProviderSettings[Constants.Password];
            }
        }

        public string AdministrationToolService
        {
            get
            {
                return ProviderSettings[Constants.AdministrationToolService];
            }
        }

        public string EnterpriseServer
        {
            get
            {
                return ProviderSettings[Constants.EnterpriseServer];
            }
        }

        public string EnterpriseServerFQDN
        {
            get
            {
                return ProviderSettings[Constants.EnterpriseServerFQDN];
            }
        }


        public ResultObject CreateBlackBerryUser(string primaryEmailAddress)
        {
            return CreateBlackBerryUserInternal(primaryEmailAddress);
        }        
        
        internal virtual ResultObject CreateBlackBerryUserInternal(string primaryEmailAddress)
        {
            ResultObject res = HostedSolutionLog.StartLog<ResultObject>("CreateBlackBerryUserInternal");

            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");
            
            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("CreateBlackBerryUserInternal", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                return res;
            }

            string arguments = string.Format("-add -u {0} -p {1} -b {2} -n {3}",
                                             primaryEmailAddress,
                                             Password,
                                             EnterpriseServer,
                                             AdministrationToolService);

            try
            {
                string output;
                int exitCode = Execute(file, arguments, out output);
                if (exitCode == 0)
                {
                    Log.WriteInfo(output);
                }
                else
                {
                    throw new ApplicationException(
                        string.Format("Excit code is not 0. {0}, ExitCode = {1}", output, exitCode));
                }
            }
            catch(Exception ex)
            {
                HostedSolutionLog.EndLog("CreateBlackBerryUserInternal", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND, ex);
                return res;
            }

            HostedSolutionLog.EndLog("CreateBlackBerryUserInternal");
            return res;
        }

        protected  int Execute(string file, string arguments, out string output, out string error)
        {
            string oldDir = Directory.GetCurrentDirectory();            
            Directory.SetCurrentDirectory(UtilityPath);            
            ProcessStartInfo startInfo = new ProcessStartInfo(file, arguments) ;
            
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process proc = Process.Start(startInfo);

            if (proc == null)
                throw new ApplicationException("Proc is null.");

            StreamReader outputReader = proc.StandardOutput;
            output= outputReader.ReadToEnd();

            StreamReader errorReader = proc.StandardError;
            error = errorReader.ReadToEnd();

            Directory.SetCurrentDirectory(oldDir);
            return proc.ExitCode;            
        }

        protected  int Execute(string file, string arguments, out string output)
        {
            Log.WriteInfo(file);
            Log.WriteInfo(arguments);
                
            string outputData;
            string errorData;
            int res = Execute(file, arguments, out outputData, out errorData);

            output = outputData.Length > 0 ? "Output stream:" + outputData : string.Empty;
            output += errorData.Length > 0 ? "Error stream:" + errorData : string.Empty;

            return res;
        }
        protected int Execute2(string file, string arguments, out string output, out string error)
        {
            string oldDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(HandheldcleanupPath);
            ProcessStartInfo startInfo = new ProcessStartInfo(file, arguments);

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process proc = Process.Start(startInfo);

            StreamWriter inputWriter = proc.StandardInput;
            inputWriter.WriteLine(EnterpriseServer);
            inputWriter.Flush();
            inputWriter.Close();

            if (proc == null)
                throw new ApplicationException("Proc is null.");

            StreamReader outputReader = proc.StandardOutput;
            output = outputReader.ReadToEnd();

            StreamReader errorReader = proc.StandardError;
            error = errorReader.ReadToEnd();

            Directory.SetCurrentDirectory(oldDir);
            return proc.ExitCode;
        }

        protected int Execute2(string file, string arguments, out string output)
        {
            Log.WriteInfo(file);
            Log.WriteInfo(arguments);

            string outputData;
            string errorData;
            int res = Execute2(file, arguments, out outputData, out errorData);

            output = outputData.Length > 0 ? "Output stream:" + outputData : string.Empty;
            output += errorData.Length > 0 ? "Error stream:" + errorData : string.Empty;

            return res;
        }

        public ResultObject DeleteBlackBerryUser(string primaryEmailAddress)
        {
            return DeleteBlackBerryUserInternal(primaryEmailAddress);
        }

        internal virtual ResultObject DeleteBlackBerryUserInternal(string primaryEmailAddress)
        {
            ResultObject res = HostedSolutionLog.StartLog<ResultObject>("DeleteBlackBerryUserInternal");

            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("DeleteBlackBerryUser", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                return res;
            }

            string arguments = string.Format("-delete -u {0} -p {1} -b {2} -n {3}",
                                             primaryEmailAddress,
                                             Password,
                                             EnterpriseServer,
                                             AdministrationToolService);

            try
            {
                string output;
                int exitCode = Execute(file, arguments, out output);
                if (exitCode == 0)
                {
                    Log.WriteInfo(output);
                }
                else
                {
                    throw new ApplicationException(
                        string.Format("Exit code is not 0. {0}, ExitCode = {1}", output, exitCode));
                }

                for (int i = 0; i < 10; i++)
                {

                    BlackBerryUserDeleteState states = GetBlackBerryUserState(primaryEmailAddress);
                    if (states != BlackBerryUserDeleteState.Pending)
                    {
                        break;
                    }
                    Thread.Sleep(10000);

                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.EndLog("DeleteBlackBerryUserInternal", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND, ex);
                return res;
            }

            HostedSolutionLog.EndLog("DeleteBlackBerryUserInternal");
            return res;
                       
        }

        public BlackBerryUserStatsResult GetBlackBerryUserStats(string primaryEmailAddress)
        {
            return GetBlackBerryUserStatsInternal(primaryEmailAddress);
        }

        protected virtual ResultObject GetBlackBerryUserData(string primaryEmailAddress, out string []keys, out string []values)
        {
            BlackBerryUserStatsResult res =
                HostedSolutionLog.StartLog<BlackBerryUserStatsResult>("GetBlackBerryUserData");


            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("GetBlackBerryUserData", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                keys = null;
                values = null;
                
                return res;

            }
            string arguments = string.Format("-stats -u {0} -p {1} -b {2} -n {3}",
                                             primaryEmailAddress,
                                             Password,
                                             EnterpriseServer,
                                             AdministrationToolService);

            string output;
            string error;

            try
            {
                int exitCode = Execute(file, arguments, out output, out error);
                if (exitCode == 0)
                {
                    Log.WriteInfo(output);
                }
                else
                {
                    throw new ApplicationException(
                        string.Format("Exit code is not 0. {0}, ExitCode = {1}", error, exitCode));
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.EndLog("GetBlackBerryUserData", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND, ex);
                keys = null;
                values = null;
                return res;
            }

            try
            {
                string[] data = output.Split('\n');
                Regex regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                keys = regex.Split(data[0]);
                values = regex.Split(data[1]);                

            }
            catch (Exception ex)
            {
                HostedSolutionLog.EndLog("GetBlackBerryUserData", res, BlackBerryErrorsCodes.CANNOT_SPLIT_STATS, ex);
                keys = null;
                values = null;                
                return res;
            }

            HostedSolutionLog.EndLog("GetBlackBerryUserData");
            return res;            
        }

        internal virtual BlackBerryUserStatsResult GetBlackBerryUserStatsInternal(string primaryEmailAddress)
        {
            BlackBerryUserStatsResult res =
                HostedSolutionLog.StartLog<BlackBerryUserStatsResult>("GetBlackBerryUserStatsInternal");

            string[] keys; 
            string[] values;

            ResultObject tempRes = GetBlackBerryUserData(primaryEmailAddress, out keys, out values);
            res.ErrorCodes.AddRange(tempRes.ErrorCodes);
            if (!res.IsSuccess)
            {
                HostedSolutionLog.EndLog("GetBlackBerryUserStatsInternal", res);
                return res;
            }

            try
            {
                List<BlackBerryStatsItem> items = new List<BlackBerryStatsItem>();

                int []inds = new int[] {3, 4, 5, 7, 8, 9, 11, 12, 13, 14, 15, 16, 26};

                foreach(int i in inds)
                {
                    items.Add(new BlackBerryStatsItem() { Name = keys[i], Value = values[i] });                    
                }
                
                
                res.Value = items;
            }
            catch(Exception ex)
            {
                HostedSolutionLog.EndLog("GetBlackBerryUserStatsInternal", res, BlackBerryErrorsCodes.CANNOT_POPULATE_STATS, ex);
                return res;
            }

            HostedSolutionLog.EndLog("GetBlackBerryUserStatsInternal");
            return res;            
        }
        
        public override bool IsInstalled()
        {
            return true;
        }

        public override string[] Install()
        {
            return CheckSettings();
        }

        protected string[] CheckSettings()
        {
            List<string> ret = new List<string>();
            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                ret.Add(BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
            }

            string output;
            int res = Execute("Net", string.Format("View {0}", AdministrationToolService), out output);
            if (res != 0)
            {
                ret.Add(BlackBerryErrorsCodes.ADMINISTRATION_TOOL_SERVICE_IS_INVALID);
            }
            

            return ret.ToArray();
        }


        public ResultObject SetActivationPasswordWithExpirationTime(string primaryEmailAddress, string password, int time)
        {
            return SetActivationPasswordWithExpirationTimeInternal(primaryEmailAddress, password, time);                        
        }

        internal virtual ResultObject SetActivationPasswordWithExpirationTimeInternal(string primaryEmailAddress, string password, int time)
        {
            ResultObject res = HostedSolutionLog.StartLog<ResultObject>("SetActivationPasswordWithExpirationTimeInternal");

            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("SetActivationPasswordWithExpirationTimeInternal", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                return res;
            }

            string arguments = string.Format("-change  -u {0} -p {1} -b {2} -n {3} -w {4} -wt {5}",
                                             primaryEmailAddress,
                                             Password,
                                             EnterpriseServer,
                                             AdministrationToolService,
                                             password,
                                             time);

            try
            {
                string output;
                int exitCode = Execute(file, arguments, out output);
                if (exitCode == 0)
                {
                    Log.WriteInfo(output);
                }
                else
                {
                    throw new ApplicationException(
                        string.Format("Exit code is not 0. {0}, ExitCode = {1}", output, exitCode));
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.EndLog("SetActivationPasswordWithExpirationTimeInternal", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND, ex);
                return res;
            }

            HostedSolutionLog.EndLog("SetActivationPasswordWithExpirationTimeInternal");
            return res;
        }

        public ResultObject SetEmailActivationPassword(string primaryEmailAddress)
        {
            return SetEmailActivationPasswordInternal(primaryEmailAddress);
        }

        internal virtual ResultObject SetEmailActivationPasswordInternal(string primaryEmailAddress)
        {
            ResultObject res = HostedSolutionLog.StartLog<ResultObject>("SetEmailActivationPassword");

            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("SetEmailActivationPassword", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                return res;
            }

            string arguments = string.Format("-change -u {0} -p {1} -b {2} -n {3} -wrandom",
                                             primaryEmailAddress,
                                             Password,
                                             EnterpriseServer,
                                             AdministrationToolService);

            try
            {
                string output;
                int exitCode = Execute(file, arguments, out output);
                if (exitCode == 0)
                {
                    Log.WriteInfo(output);
                }
                else
                {
                    throw new ApplicationException(
                        string.Format("Exit code is not 0. {0}, ExitCode = {1}", output, exitCode));
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.EndLog("SetEmailActivationPassword", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND, ex);
                return res;
            }

            HostedSolutionLog.EndLog("SetEmailActivationPassword");
            return res;
        }

        public ResultObject DeleteDataFromBlackBerryDevice(string primaryEmailAddress)
        {
            return DeleteDataFromBlackBerryDeviceInternal(primaryEmailAddress);
        }

        internal virtual ResultObject DeleteDataFromBlackBerryDeviceInternal(string primaryEmailAddress)
        {
            ResultObject res = HostedSolutionLog.StartLog<ResultObject>("DeleteDataFromBlackBerryDevice");

            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("DeleteDataFromBlackBerryDevice", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                return res;
            }

            string arguments = string.Format("-kill_handheld -u {0} -p {1} -b {2} -n {3}",
                                             primaryEmailAddress,
                                             Password,
                                             EnterpriseServer,
                                             AdministrationToolService);

            try
            {
                string output;
                int exitCode = Execute(file, arguments, out output);
                if (exitCode == 0)
                {
                    Log.WriteInfo(output);
                }
                else
                {
                    throw new ApplicationException(
                        string.Format("Exit code is not 0. {0}, ExitCode = {1}", output, exitCode));
                }
            }
            catch (Exception ex)
            {
                HostedSolutionLog.EndLog("DeleteDataFromBlackBerryDevice", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND, ex);
                return res;
            }

            HostedSolutionLog.EndLog("DeleteDataFromBlackBerryDevice");
            return res;
        }

        protected BlackBerryUserDeleteState GetBlackBerryUserState(string primaryEmailAddress)
        {
                string[] keys;
                string[] values;

                GetBlackBerryUserData(primaryEmailAddress, out keys, out values);
            
                return  values != null && values.Length == 32 && !string.IsNullOrEmpty(values[values.Length - 1])
                                ? BlackBerryUserDeleteState.Pending
                                : BlackBerryUserDeleteState.None;
            
        }
    }
}
