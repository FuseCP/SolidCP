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
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using SolidCP.Providers.Common;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers.ResultObjects;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.HostedSolution
{
    public class BlackBerry5Provider : BlackBerryProvider
    {
        public string User
        {
            get
            {
                return ProviderSettings[Constants.UserName];
            }
        }

        public override string[] Install()
        {
            List<string> ret = new List<string>();
            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                ret.Add(BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
            }
            return ret.ToArray();
        }

        internal override ResultObject CreateBlackBerryUserInternal(string primaryEmailAddress)
        {
            ResultObject res = HostedSolutionLog.StartLog<ResultObject>("CreateBlackBerryUser5Internal");

            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");
            string file2 = Path.Combine(HandheldcleanupPath, "handheldcleanup.exe");

            //Add user to Blackberry Server
            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("CreateBlackBerry5UserInternal", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                return res;
            }

            string arguments = string.Format("-username {0} -password {1} -add -u {2} -b {3} -n {4}",
                                             User,
                                             Password,
                                             primaryEmailAddress,
                                             EnterpriseServer,
                                             EnterpriseServerFQDN);

            //run besuseradminclient.exe

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
                HostedSolutionLog.EndLog("CreateBlackBerry5UserInternal", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND, ex);
                return res;
            }

             //run handheldcleanup.exe           
            if (File.Exists(file2))
            {

            string arguments2 = string.Format("-u -p {0}",
                                 MAPIProfile);
            try
            {
                string output;

                int exitCode = Execute2(file2, arguments2, out output);
                if (exitCode == 0)
                {
                    Log.WriteInfo(output);

                }
                else
                {

                    throw new ApplicationException(
                        string.Format("Exit code is not 0. {0}, ExitCode = {1}", arguments2, output, exitCode));
                }
            }

            catch (Exception ex)
            {
                HostedSolutionLog.EndLog("CreateBlackBerry5UserInternal", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND_HANDHELDCLEANUP, ex);
                return res;
            }
            }
            HostedSolutionLog.EndLog("CreateBlackBerry5UserInternal");
            return res;
        
        }


        internal override ResultObject SetActivationPasswordWithExpirationTimeInternal(string primaryEmailAddress, string password, int time)
        {
            ResultObject res = HostedSolutionLog.StartLog<ResultObject>("SetActivationPasswordWithExpirationTimeInternal");

            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("SetActivationPasswordWithExpirationTimeInternal", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                return res;
            }

            string arguments = string.Format(" -username {0} -password {1} -change -u {2} -b {3} -w {4} -wt {5} -n {6}",
                                             User,
                                             Password,
                                             primaryEmailAddress,
                                             EnterpriseServer,                                             
                                             password,
                                             time,
                                             EnterpriseServerFQDN);

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

        private ResultObject RemoveEmailActivationPassword(string primaryEmailAddress)
        {
            ResultObject res = HostedSolutionLog.StartLog<ResultObject>("RemoveEmailActivationPassword");

            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("RemoveEmailActivationPassword", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                return res;
            }

            string arguments = string.Format("-username {0} -password {1}  -change -u {2}  -b {3} -cw -n {4}",
                                                User,
                                                Password,
                                                primaryEmailAddress,
                                                EnterpriseServer,
                                                EnterpriseServerFQDN);

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
                HostedSolutionLog.EndLog("RemoveEmailActivationPassword", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND, ex);
                return res;
            }

            HostedSolutionLog.EndLog("RemoveEmailActivationPassword");
            return res;
        }
        
        internal override ResultObject SetEmailActivationPasswordInternal(string primaryEmailAddress)
        {                        
            ResultObject res = HostedSolutionLog.StartLog<ResultObject>("SetEmailActivationPassword");

            ResultObject removeRes = RemoveEmailActivationPassword(primaryEmailAddress);
            res.ErrorCodes.AddRange(removeRes.ErrorCodes);
            
            if (!removeRes.IsSuccess)
            {
                HostedSolutionLog.EndLog("SetEmailActivationPassword", res);
                return res;
            }

            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("SetEmailActivationPassword", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                return res;
            }

            string arguments = string.Format("-username {0} -password {1}  -change -u {2}  -b {3} -wrandom -n {4}",
                                                User,
                                                Password,         
                                                primaryEmailAddress,                                             
                                                EnterpriseServer,
                                                EnterpriseServerFQDN);

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

        internal override ResultObject DeleteDataFromBlackBerryDeviceInternal(string primaryEmailAddress)
        {
            ResultObject res = HostedSolutionLog.StartLog<ResultObject>("DeleteDataFromBlackBerry5Device");

            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("DeleteDataFromBlackBerryDevice", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                return res;
            }

            string arguments = string.Format("-username {0} -password {1} -kill_handheld -u {2} -b {3} -n {4}",
                                             User,
                                             Password,
                                             primaryEmailAddress,
                                             EnterpriseServer,
                                             EnterpriseServerFQDN);
                

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
                HostedSolutionLog.EndLog("DeleteDataFromBlackBerry5Device", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND, ex);
                return res;
            }

            HostedSolutionLog.EndLog("DeleteDataFromBlackBerry5Device");
            return res;
        }

        internal override  BlackBerryUserStatsResult GetBlackBerryUserStatsInternal(string primaryEmailAddress)
        {
            BlackBerryUserStatsResult res =
                HostedSolutionLog.StartLog<BlackBerryUserStatsResult>("GetBlackBerry5UserStatsInternal");

            string[] keys;
            string[] values;

            ResultObject tempRes = GetBlackBerryUserData(primaryEmailAddress, out keys, out values);
            res.ErrorCodes.AddRange(tempRes.ErrorCodes);
            if (!res.IsSuccess)
            {
                HostedSolutionLog.EndLog("GetBlackBerry5UserStatsInternal", res);
                return res;
            }

            try
            {
                List<BlackBerryStatsItem> items = new List<BlackBerryStatsItem>();

                int[] inds = new int[] { 3, 4, 5, 7, 8, 10, 11, 12, 13, 14, 15, 25 };

                foreach (int i in inds)
                {                   
                    if (keys.Length > i && values.Length > i)
                        items.Add(new BlackBerryStatsItem() { Name = keys[i], Value = values[i] });
                }


                res.Value = items;
            }
            catch (Exception ex)
            {
                HostedSolutionLog.EndLog("GetBlackBerry5UserStatsInternal", res, BlackBerryErrorsCodes.CANNOT_POPULATE_STATS, ex);
                return res;
            }

            HostedSolutionLog.EndLog("GetBlackBerry5UserStatsInternal");
            return res;
        }
        

        protected override ResultObject GetBlackBerryUserData(string primaryEmailAddress, out string[] keys, out string[] values)
        {
            BlackBerryUserStatsResult res =
               HostedSolutionLog.StartLog<BlackBerryUserStatsResult>("GetBlackBerry5UserData");


            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("GetBlackBerry5UserData", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                keys = null;
                values = null;

                return res;

            }
            string arguments = string.Format(" -username {0} -password {1} -stats -u {2} -b {3} -n {4}",
                                             User,
                                             Password,
                                             primaryEmailAddress,
                                             EnterpriseServer,
                                             EnterpriseServerFQDN);

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
                HostedSolutionLog.EndLog("GetBlackBerry5UserData", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND, ex);
                keys = null;
                values = null;
                return res;
            }

            try
            {
                string[] data = output.Split('\n');
                Regex regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                int startRow = 0;

                if (data.Length > 0)
                {
                    for (startRow = data.Length - 1; startRow >= 0; startRow--)
                    {                       
                        if (!string.IsNullOrEmpty(data[startRow]) && !string.IsNullOrEmpty(data[startRow].Trim()))
                            break;
                    }
                }
                
                 
                keys = regex.Split(data[startRow -1]);
                values = regex.Split(data[startRow]);

            }
            catch (Exception ex)
            {
                HostedSolutionLog.EndLog("GetBlackBerry5UserData", res, BlackBerryErrorsCodes.CANNOT_SPLIT_STATS, ex);
                keys = null;
                values = null;
                return res;
            }

            HostedSolutionLog.EndLog("GetBlackBerry5UserData");
            return res;            
        }

        internal override ResultObject DeleteBlackBerryUserInternal(string primaryEmailAddress)
        {
            ResultObject res = HostedSolutionLog.StartLog<ResultObject>("DeleteBlackBerry5UserInternal");

            string file = Path.Combine(UtilityPath, "besuseradminclient.exe");

            if (!File.Exists(file))
            {
                HostedSolutionLog.EndLog("DeleteBlackBerry5User", res, BlackBerryErrorsCodes.FILE_PATH_IS_INVALID);
                return res;
            }

            string arguments = string.Format("-username {0} -password {1} -delete -u {2} -b {3} -n {4}",
                                             User, 
                                             Password,
                                             primaryEmailAddress,                                             
                                             EnterpriseServer,
                                             EnterpriseServerFQDN);

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
                HostedSolutionLog.EndLog("DeleteBlackBerry5UserInternal", res, BlackBerryErrorsCodes.CANNOT_EXECUTE_COMMAND, ex);
                return res;
            }

            HostedSolutionLog.EndLog("DeleteBlackBerry5UserInternal");
            return res;
        }
    }
}
