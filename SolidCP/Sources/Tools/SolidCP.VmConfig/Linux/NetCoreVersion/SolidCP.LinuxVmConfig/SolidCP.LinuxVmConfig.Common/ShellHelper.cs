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
using System.Diagnostics;

namespace SolidCP.LinuxVmConfig
{
    public class ShellHelper
    {
        internal const string Shell_Linux = "/bin/bash";
        private static readonly string Shell_FreeBSD = AppDomain.CurrentDomain.BaseDirectory + "sh";//use external shell to out from Linux-emulator

        public static ExecutionResult RunCmd(string cmd)
        {
            ExecutionResult ret = new ExecutionResult();
            ret.ResultCode = 0;
            ret.ErrorMessage = null;
            string shellPath = null;
            try
            {
                if (OsVersion.GetOsVersion() == OsVersionEnum.FreeBSD)
                {
                    shellPath = Shell_FreeBSD;
                }
                else
                {
                    shellPath = Shell_Linux;
                }
                string escapedArgs = cmd.Replace("\"", "\\\"");
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = shellPath,
                        Arguments = $"-c \"{escapedArgs}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };
                    process.Start();
                    ret.Value = process.StandardOutput.ReadToEnd();
                    ret.ErrorMessage = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.ResultCode = 1;
                ret.ErrorMessage = "ShellHelper error: " + ex.ToString();
                return ret;
            }
        }

        public static ExecutionResult ChangeUserPassword(string userName, string password)
        {
            ExecutionResult ret = new ExecutionResult();
            ret.ResultCode = 0;
            ret.ErrorMessage = null;

            try
            {
                string shellPath = null;
                string arguments = null;
                if (OsVersion.GetOsVersion() == OsVersionEnum.FreeBSD)
                {
                    shellPath = Shell_FreeBSD;
                    arguments = $"-c \"echo \"{password}\" | pw usermod {userName} -h 0\"";
                }
                else
                {
                    shellPath = Shell_Linux;
                    arguments = $"-c \"passwd {userName}\"";
                }
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = shellPath,
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    };
                    process.Start();
                    if (OsVersion.GetOsVersion() != OsVersionEnum.FreeBSD)//Input redirect dont work on FreeBSD
                    {
                        System.Threading.Thread.Sleep(1000);
                        process.StandardInput.WriteLine(password);
                        System.Threading.Thread.Sleep(1000);
                        process.StandardInput.WriteLine(password);
                    }
                    process.WaitForExit();
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.ResultCode = 1;
                ret.ErrorMessage = "ShellHelper error: " + ex.ToString();
                return ret;
            }
        }
    }
}
