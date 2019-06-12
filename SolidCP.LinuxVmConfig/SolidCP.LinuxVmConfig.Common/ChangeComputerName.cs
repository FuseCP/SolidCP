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

namespace SolidCP.LinuxVmConfig
{
    public class ChangeComputerName
    {
        internal const string cloudCfg = "/etc/cloud/cloud.cfg";
        internal const string hosts = "/etc/hosts";
        internal const string hostname = "/etc/hostname";

        public static ExecutionResult Run(ref ExecutionContext context, OsVersion osVersion)
        {
            ExecutionResult ret = new ExecutionResult();
            ret.ResultCode = 0;
            ret.ErrorMessage = null;
            ret.RebootRequired = true;
            try
            {
                context.ActivityDescription = "Changing computer name...";
                context.Progress = 0;
                if (!context.Parameters.ContainsKey("FullComputerName"))
                {
                    ret.ResultCode = 2;
                    ret.ErrorMessage = "Parameter 'FullComputerName' not found";
                    Log.WriteError(ret.ErrorMessage);
                    context.Progress = 100;
                    return ret;
                }
                string computerName = context.Parameters["FullComputerName"].ToLower();
                string netBiosName = computerName;
                int idx = netBiosName.IndexOf(".");
                if (idx != -1)
                {
                    netBiosName = computerName.Substring(0, idx);
                }
                ExecutionResult res;
                res = ShellHelper.RunCmd("hostname -s");
                if (res.ResultCode == 1)
                {
                    ret.ResultCode = 1;
                    ret.ErrorMessage = res.ErrorMessage;
                    context.Progress = 100;
                    return ret;
                }
                string hostnameOld = res.Value.Trim();

                res = ShellHelper.RunCmd("hostname");
                if (res.ResultCode == 1)
                {
                    ret.ResultCode = 1;
                    ret.ErrorMessage = res.ErrorMessage;
                    context.Progress = 100;
                    return ret;
                }
                string fullnameOld = res.Value.Trim();

                TxtHelper.ReplaceStr(hostname, computerName, 0);
                TxtHelper.ReplaceStr(hosts, fullnameOld, computerName);
                TxtHelper.ReplaceStr(hosts, " " + hostnameOld, " " + netBiosName);
                TxtHelper.ReplaceStr(hosts, "\t" + hostnameOld, "\t" + netBiosName);

                if (osVersion == OsVersion.Ubuntu)
                {
                    TxtHelper.ReplaceStr(cloudCfg, "preserve_hostname: false", "preserve_hostname: true");
                    TxtHelper.ReplaceStr(cloudCfg, "# preserve_hostname: true", "preserve_hostname: true");
                    TxtHelper.ReplaceStr(cloudCfg, "#preserve_hostname: true", "preserve_hostname: true");
                }
            }catch(Exception ex)
            {
                ret.ResultCode = 1;
                ret.ErrorMessage = "ChangeComputerName error: " + ex.ToString();
                Log.WriteError(ret.ErrorMessage);
            }
            if (ret.ResultCode == 0)
            {
                Log.WriteInfo("Computer name has been changed successfully");
            }
            context.Progress = 100;
            return ret;
        }
    }
}
