﻿// Copyright (c) 2016, SolidCP
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
        internal const string rcConf = "/etc/rc.conf";
        internal const string compatLinux = "/compat/linux";

        public static ExecutionResult Run(ref ExecutionContext context)
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
                string hostNameOld = null;
                if (res.Value != null) hostNameOld = res.Value.Trim();

                res = ShellHelper.RunCmd("hostname");
                string fullNameOld = null;
                if (res.Value != null) fullNameOld = res.Value.Trim();

                string domain = computerName.Replace(netBiosName + ".", "");
                string domainOld = null;
                if (hostNameOld != null && hostNameOld.Length > 0 && fullNameOld != null && fullNameOld.Length > 0)
                {
                    domainOld = fullNameOld.Replace(hostNameOld + ".", "");
                }

                switch (OsVersion.GetOsVersion())
                {
                    case OsVersionEnum.Ubuntu:
                        TxtHelper.ReplaceStr(hostname, computerName, 0);
                        if (domainOld != null && domainOld.Length > 0) TxtHelper.ReplaceStr(hosts, domainOld, domain);
                        if (hostNameOld != null && hostNameOld.Length > 0) TxtHelper.ReplaceStr(hosts, hostNameOld, netBiosName);
                        TxtHelper.ReplaceStr(cloudCfg, "preserve_hostname: false", "preserve_hostname: true");
                        TxtHelper.ReplaceStr(cloudCfg, "# preserve_hostname: true", "preserve_hostname: true");
                        TxtHelper.ReplaceStr(cloudCfg, "#preserve_hostname: true", "preserve_hostname: true");
                        break;
                    case OsVersionEnum.CentOS:
                        TxtHelper.ReplaceStr(hostname, computerName, 0);
                        if (domainOld != null && domainOld.Length > 0) TxtHelper.ReplaceStr(hosts, domainOld, domain);
                        if (hostNameOld != null && hostNameOld.Length > 0) TxtHelper.ReplaceStr(hosts, hostNameOld, netBiosName);
                        break;
                    case OsVersionEnum.FreeBSD:
                        ShellHelper.RunCmd("cp -p " + rcConf + " " + compatLinux + rcConf);
                        ShellHelper.RunCmd("cp -p " + hosts + " " + compatLinux + hosts);
                        TxtHelper.ReplaceStr(compatLinux + rcConf, "hostname=\"" + computerName + "\"", TxtHelper.GetStrPos(compatLinux + rcConf, "hostname", 0, -1));
                        if (domainOld != null && domainOld.Length > 0) TxtHelper.ReplaceStr(compatLinux + hosts, domainOld, domain);
                        if (hostNameOld != null && hostNameOld.Length > 0) TxtHelper.ReplaceStr(compatLinux + hosts, hostNameOld, netBiosName);
                        ShellHelper.RunCmd("cp -p " + compatLinux + rcConf + " " + rcConf);
                        ShellHelper.RunCmd("cp -p " + compatLinux + hosts + " " + hosts);
                        break;
                    default:
                        TxtHelper.ReplaceStr(hostname, computerName, 0);
                        if (domainOld != null && domainOld.Length > 0) TxtHelper.ReplaceStr(hosts, domainOld, domain);
                        if (hostNameOld != null && hostNameOld.Length > 0) TxtHelper.ReplaceStr(hosts, hostNameOld, netBiosName);
                        break;
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
