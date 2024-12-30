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
using System.Text.RegularExpressions;

namespace SolidCP.LinuxVmConfig
{
    public class SetupNetworkAdapter
    {
        internal const string netplanFolder = "/etc/netplan/";
        internal const string interfacesPath = "/etc/network/interfaces";
        internal const string hosts = "/etc/hosts";
        internal const string ifcfgBasePath = "/etc/sysconfig/network-scripts/ifcfg-";
        internal const string rcConf = "/etc/rc.conf";
        internal const string resolvConf = "/etc/resolv.conf";
        internal const string compatLinux = "/compat/linux";

        public static ExecutionResult Run(ref ExecutionContext context)
        {
            ExecutionResult ret = new ExecutionResult();
            String adapterName = null;
            ret.ResultCode = 0;
            ret.ErrorMessage = null;
            ret.RebootRequired = false;

            context.ActivityDescription = "Configuring network adapter...";
            context.Progress = 0;
            if (!CheckParameter(context, "MAC"))
            {
                ProcessError(context, ret, null, 2, "Parameter 'MAC' is not specified");
                return ret;
            }
            string macAddress = context.Parameters["MAC"];
            if (!IsValidMACAddress(macAddress))
            {
                ProcessError(context, ret, null, 2, "Parameter 'MAC' has invalid format. It should be in 12:34:56:78:90:ab format.");
                return ret;
            }
            try
            {
                List<Adapter> adapters = getAdapters();
                if (adapters == null)
                {
                    ProcessError(context, ret, null, 2, "No adapters found.");
                    return ret;
                }

                Log.WriteInfo("Configuring adapter with MAC address: " + macAddress.ToUpper());

                foreach (Adapter adapter in adapters)
                {
                    if (macAddress.ToUpper().Equals(adapter.MAC.ToUpper()))
                    {
                        adapterName = adapter.Name;
                        break;
                    }
                }
                if (adapterName == null)
                {
                    ProcessError(context, ret, null, 2, "Adapter not found.");
                    return ret;
                }
            }
            catch (Exception ex)
            {
                ProcessError(context, ret, ex, 2, "Network adapter configuration error: ");
                return ret;
            }

            if (CheckParameter(context, "EnableDHCP", "True"))
            {
                try
                {
                    EnableDHCP(adapterName, ret);
                }
                catch (Exception ex)
                {
                    ProcessError(context, ret, ex, 2, "DHCP error: ");
                    return ret;
                }

            }
            else if (CheckParameter(context, "EnableDHCP", "False"))
            {

                if (!CheckParameter(context, "DefaultIPGateway"))
                {
                    // allow onlink only routes. ie private network when no gateway specified
                    Log.WriteInfo("No default gateway specified");
                    context.Parameters["DefaultIPGateway"] = ";";
                }
                if (!CheckParameter(context, "IPAddress"))
                {
                    ProcessError(context, ret, null, 2, "Parameter 'IPAddresses' is not specified");
                    return ret;
                }
                if (!CheckParameter(context, "SubnetMask"))
                {
                    ProcessError(context, ret, null, 2, "Parameter 'SubnetMasks' is not specified");
                    return ret;
                }
                try
                {
                    DisableDHCP(context, adapterName, ret);
                }
                catch (Exception ex)
                {
                    ProcessError(context, ret, ex, 2, "Network adapter configuration error: ");
                    return ret;
                }
            }
            context.Progress = 100;
            return ret;
        }

        private class Adapter
        {
            public Adapter(string name, string mac)
            {
                this.Name = name;
                this.MAC = mac;
            }
            public string Name { get; set; }
            public string MAC { get; set; }
        }

        private static List<string> StrToList(string str)
        {
            List<string> result = new List<string>();
            int startIdx = 0;
            int idx;
            do
            {
                idx = str.IndexOf("\n", startIdx);
                if (idx != -1)
                {
                    string subSstr = str.Substring(startIdx, idx - startIdx);
                    result.Add(subSstr);
                    startIdx = idx + 1;
                }
                else
                {
                    string subSstr = str.Substring(startIdx, str.Length - startIdx);
                    result.Add(subSstr);
                }
            } while (idx != -1);
            return result;
        }

        private static List<Adapter> getAdapters()
        {
            List<Adapter> adapters = new List<Adapter>();
            ExecutionResult res = null;
            switch (OsVersion.GetOsVersion())
            {
                case OsVersionEnum.Ubuntu:
                    // added -a since newly added adapters are shutdown by default and will not be returned without the switch.
                    res = ShellHelper.RunCmd("ifconfig -s -a | awk '{print $1}'"); 
                    break;
                case OsVersionEnum.CentOS:
                    res = ShellHelper.RunCmd("nmcli -p dev | grep \"ethernet\" | awk '{print $1}'");
                    break;
                case OsVersionEnum.FreeBSD:
                    res = ShellHelper.RunCmd("ifconfig -l");
                    if (res.Value != null) res.Value = res.Value.Replace(" ", "\n");
                    break;
            }
            if (res.Value == null || res.ResultCode == 1) return null;
            List<string> adaptersStr = StrToList(res.Value.Trim());
            
            foreach (string adapterName in adaptersStr)
            {
                if (!adapterName.Equals("Iface") && !adapterName.Equals("lo"))
                {
                    string mac = GetAdapterMacAddress(adapterName);
                    if (mac != null)
                    {
                        Adapter adapter = new Adapter(adapterName, mac);
                        adapters.Add(adapter);
                    }
                }
            }
            return adapters;
        }

        private static string GetAdapterMacAddress(string adapterName)
        {
            ExecutionResult res;
            if (OsVersion.GetOsVersion() == OsVersionEnum.FreeBSD)
            {
                res = ShellHelper.RunCmd("ifconfig " + adapterName + " ether | grep ether | awk '{print $2}'");
            }
            else
            {
                res = ShellHelper.RunCmd("cat /sys/class/net/" + adapterName + "/address");
            }
            if (res.ResultCode == 1 || res.Value == null) return null;
            string mac = res.Value.Trim();
            if (mac != null)
            {
                mac = mac.Replace("\n", "");
            }
            else
            {
                return null;
            }
            if (IsValidMACAddress(mac))
            {
                return mac;
            }
            else
            {
                return null;
            }
        }

        private static string[] ParseArray(string array)
        {
            if (string.IsNullOrEmpty(array))
                throw new ArgumentException("array");
            string[] ret = array.Split(';');
            return ret;
        }

        private static void ProcessError(ExecutionContext context, ExecutionResult ret, Exception ex, int errorCode, string errorPrefix)
        {
            ret.ResultCode = errorCode;
            ret.ErrorMessage = errorPrefix;
            if (ex != null) ret.ErrorMessage += ex.ToString();
            Log.WriteError(ret.ErrorMessage);
            context.Progress = 100;
        }

        private static void EnableDHCP_Ubuntu(String adapter, ExecutionResult ret)
        {
            //netplan configuration (for Ubuntu 18.04+)
            string netplanPath = GetNetplanPath();
            int startPos = TxtHelper.GetStrPos(netplanPath, adapter + ":", 0, -1);
            if (startPos != -1)
            {
                string firstStr = TxtHelper.GetStr(netplanPath, adapter + ":", startPos, startPos);
                int spacesCount = GetNetplanSpacesCount(firstStr);

                startPos++;
                int endPos = GetNetplanEndPos(netplanPath, startPos, spacesCount);
                List<string> config = new List<string>();
                config.Add(new String(' ', spacesCount + 2) + "dhcp4: yes");
                TxtHelper.ReplaceAllStr(netplanPath, config, startPos, endPos);
                ShellHelper.RunCmd("netplan apply");
            }
        }

        private static void EnableDHCP_CentOS(String adapter, ExecutionResult ret)
        {
            string adapterPath = ifcfgBasePath + adapter;
            TxtHelper.ReplaceStr(adapterPath, "BOOTPROTO=dhcp", TxtHelper.GetStrPos(adapterPath, "BOOTPROTO", 0, -1));
            TxtHelper.ReplaceStr(adapterPath, "ONBOOT=yes", TxtHelper.GetStrPos(adapterPath, "ONBOOT", 0, -1));
            ExecutionResult res = ShellHelper.RunCmd("systemctl restart network");
            if (res.ResultCode == 1)
            {
                ret.ResultCode = 1;
                ret.ErrorMessage = res.ErrorMessage;
            }
        }

        private static void EnableDHCP_FreeBSD(string adapter, ExecutionResult ret)
        {
            const string rcConfPath = compatLinux + rcConf;
            ShellHelper.RunCmd("cp -p " + rcConf + " " + rcConfPath);

            int pos = TxtHelper.GetStrPos(rcConfPath, "ifconfig_" + adapter + "=", 0, -1);
            if (pos == -1) pos = TxtHelper.GetStrPos(rcConfPath, "ifconfig_" + adapter + " =", 0, -1);
            TxtHelper.ReplaceStr(rcConfPath, "ifconfig_" + adapter + "=\"DHCP\"", pos);

            ShellHelper.RunCmd("cp -p " + rcConfPath + " " + rcConf);

            ExecutionResult res = ShellHelper.RunCmd("service netif restart");
            if (res.ResultCode == 1)
            {
                ret.ResultCode = 1;
                ret.ErrorMessage = res.ErrorMessage;
            }
            res = ShellHelper.RunCmd("service routing restart");
            if (res.ResultCode == 1)
            {
                ret.ResultCode = 1;
                ret.ErrorMessage = res.ErrorMessage;
            }
        }

        private static void EnableDHCP(String adapter, ExecutionResult ret)
        {
            Log.WriteStart("Enabling DHCP...");
            switch (OsVersion.GetOsVersion())
            {
                case OsVersionEnum.Ubuntu:
                    EnableDHCP_Ubuntu(adapter, ret);
                    break;
                case OsVersionEnum.CentOS:
                    EnableDHCP_CentOS(adapter, ret);
                    break;
                case OsVersionEnum.FreeBSD:
                    EnableDHCP_FreeBSD(adapter, ret);
                    break;
            }
            Log.WriteEnd("DHCP enabled");
        }

        private static void DisableDHCP_Ubuntu(ExecutionContext context, String adapter, ExecutionResult ret, string[] ipAddresses, string[] subnetMasksPrefix, string[] subnetMasks, string ipGateway)
        {
            //netplan configuration (for Ubuntu 18.04+)
            string netplanPath = GetNetplanPath();

            int startPos = TxtHelper.GetStrPos(netplanPath, "ethernets:", 0, -1);
            if (startPos != -1)
            {
                // newly attached hyper-v adapters might not exist in netplan so add whether it exists or not (ie addition of private network)
                string enetStr = TxtHelper.GetStr(netplanPath, "ethernets:", startPos, startPos);
                int spacesCount = GetNetplanSpacesCount(enetStr);

                List<string> config = new List<string>();

                string strAddresses = new String(' ', spacesCount + 4) + "addresses: [";
                for (int i = 0; i < ipAddresses.Length; i++)
                {
                    strAddresses += ipAddresses[i] + "/" + subnetMasksPrefix[i];
                    if (i + 1 < ipAddresses.Length) strAddresses += ",";
                }
                strAddresses += "]";

                config.Add(new String(' ', spacesCount + 2) + adapter + ":");
                config.Add(strAddresses);

                if (!string.IsNullOrEmpty(ipGateway))
                {
                    if (ipGateway != "0.0.0.0")
                    {
                        config.Add(new String(' ', spacesCount + 4) + "routes:");
                        config.Add(new String(' ', spacesCount + 4) + "- to: default");
                        config.Add(new String(' ', spacesCount + 6) + "via: " + ipGateway);
                    }
                }

                if (CheckParameter(context, "PreferredDNSServer"))
                {
                    bool validNS = false;
                    string[] dnsServers = ParseArray(context.Parameters["PreferredDNSServer"]);

                    string strDNS = new String(' ', spacesCount + 6) + "addresses: [";
                    for (int i = 0; i < dnsServers.Length; i++)
                    {
                        if (dnsServers[i] != "0.0.0.0")
                        {
                            validNS = true;
                            strDNS += dnsServers[i];
                            if (i + 1 < dnsServers.Length) strDNS += ",";
                        }
                    }
                    if (strDNS[^1] == ',') strDNS = strDNS.Remove(strDNS.Length - 1, 1);
                    strDNS += "]";

                    if (validNS)
                    {
                        config.Add(new String(' ', spacesCount + 4) + "nameservers:");
                        config.Add(strDNS);
                    }

                }

                config.Add(new String(' ', spacesCount + 4) + "dhcp4: no");
                config.Add(new String(' ', spacesCount + 4) + "dhcp6: no");

                // add or replace adapter
                int adapterPos = TxtHelper.GetStrPos(netplanPath, adapter + ":", 0, -1);
                if (adapterPos != -1)
                {
                    string firstStr = TxtHelper.GetStr(netplanPath, adapter + ":", adapterPos, adapterPos);
                    int existingSpacesCount = GetNetplanSpacesCount(firstStr);
                    int endPos = GetNetplanEndPos(netplanPath, adapterPos + 1, existingSpacesCount);

                    TxtHelper.ReplaceAllStr(netplanPath, config, adapterPos, endPos);
                    
                }
                else
                {
                    startPos++;
                    TxtHelper.ReplaceAllStr(netplanPath, config, startPos, startPos);
                }

                ShellHelper.RunCmd("netplan apply");

            }
        }

        private static void DisableDHCP_CentOS(ExecutionContext context, String adapter, ExecutionResult ret, string[] ipAddresses, string[] subnetMasksPrefix, string ipGateway)
        {
            string adapterPath = ifcfgBasePath + adapter;

            //support for newly added adapters
            ExecutionResult ifcfg = ShellHelper.RunCmd("test -f " + adapterPath + " && echo \"true\" || echo \"false\"");
            if (ifcfg.Value != null)
            {

                bool exists = Convert.ToBoolean(ifcfg.Value);

                if (!exists) {

                    string uuid = "";
                    ExecutionResult uuidgen = ShellHelper.RunCmd("uuidgen " + adapter);
                    if (uuidgen.Value != null) { uuid = uuidgen.Value.Replace("\n", ""); }

                    List<string> config = new List<string>();

                    config.Add("TYPE=\"Ethernet\"");
                    config.Add("PROXY_METHOD=\"none\"");
                    config.Add("BROWSER_ONLY=\"no\"");
                    config.Add("BOOTPROTO=none");
                    config.Add("DEFROUTE=\"yes\"");
                    config.Add("IPV4_FAILURE_FATAL=\"no\"");
                    config.Add("NAME=\"" + adapter + "\"");
                    config.Add("UUID=\"" + uuid + "\"");
                    config.Add("DEVICE=\"" + adapter + "\"");
                    config.Add("ONBOOT=yes");

                    TxtHelper.AddAllStr(adapterPath, config);

                }

                if (exists)
                {
                    // remove all existing nameservers
                    int dnsPos;
                    do
                    {
                        dnsPos = TxtHelper.GetStrPos(adapterPath, "DNS", 0, -1);
                        if (dnsPos != -1) TxtHelper.DelStr(adapterPath, dnsPos);

                    } while (dnsPos != -1);

                    // remove all existing ips
                    int ipPos;
                    do
                    {
                        ipPos = TxtHelper.GetStrPos(adapterPath, "IPADDR", 0, -1);
                        if (ipPos != -1) TxtHelper.DelStr(adapterPath, ipPos);

                    } while (ipPos != -1);

                    // remove all existing prefixes
                    int prePos;
                    do
                    {
                        prePos = TxtHelper.GetStrPos(adapterPath, "PREFIX", 0, -1);
                        if (prePos != -1) TxtHelper.DelStr(adapterPath, prePos);

                    } while (prePos != -1);

                }

                TxtHelper.ReplaceStr(adapterPath, "BOOTPROTO=none", TxtHelper.GetStrPos(adapterPath, "BOOTPROTO", 0, -1));
                TxtHelper.ReplaceStr(adapterPath, "IPADDR=" + ipAddresses[0], TxtHelper.GetStrPos(adapterPath, "IPADDR", 0, -1));
                TxtHelper.ReplaceStr(adapterPath, "PREFIX=" + subnetMasksPrefix[0], TxtHelper.GetStrPos(adapterPath, "PREFIX", 0, -1));

                string gateway = "GATEWAY=";
                if (!string.IsNullOrEmpty(ipGateway))
                {
                    if (ipGateway != "0.0.0.0")
                    {
                        gateway += ipGateway;
                    }
                }
                TxtHelper.ReplaceStr(adapterPath, gateway, TxtHelper.GetStrPos(adapterPath, "GATEWAY", 0, -1));

                if (CheckParameter(context, "PreferredDNSServer"))
                {
                    int nsi = 1;
                    string[] dnsServers = ParseArray(context.Parameters["PreferredDNSServer"]);
                    for (int i = 0; i < dnsServers.Length; i++)
                    {
                        if (dnsServers[i] != "0.0.0.0")
                        {
                            TxtHelper.ReplaceStr(adapterPath, "DNS" + nsi.ToString() + "=" + dnsServers[i], -1);
                            nsi++;
                        }
                    }
                }

                if (ipAddresses.Length > 1)
                {
                    for (int i = 1; i < ipAddresses.Length; i++)
                    {
                        TxtHelper.ReplaceStr(adapterPath, "IPADDR" + i.ToString() + "=" + ipAddresses[i], -1);
                        TxtHelper.ReplaceStr(adapterPath, "PREFIX" + i.ToString() + "=" + subnetMasksPrefix[i], -1);
                    }
                }

                ExecutionResult res = ShellHelper.RunCmd("systemctl restart network");
                if (res.ResultCode == 1)
                {
                    ret.ResultCode = 1;
                    ret.ErrorMessage = res.ErrorMessage;
                }

            }

        }

        private static void DisableDHCP_FreeBSD(ExecutionContext context, String adapter, ExecutionResult ret, string[] ipAddresses, string[] subnetMasks, string ipGateway)
        {
            const string rcConfPath = compatLinux + rcConf;
            const string resolvConfPath = compatLinux + resolvConf;
            ShellHelper.RunCmd("cp -p " + rcConf + " " + rcConfPath);
            int pos = TxtHelper.GetStrPos(rcConfPath, "ifconfig_" + adapter + "=", 0, -1);
            if (pos == -1) pos = TxtHelper.GetStrPos(rcConfPath, "ifconfig_" + adapter + " =", 0, -1);
            TxtHelper.ReplaceStr(rcConfPath, "ifconfig_" + adapter + "=\"inet " + ipAddresses[0] + " netmask " + subnetMasks[0] + "\"", pos);
            TxtHelper.ReplaceStr(rcConfPath, "defaultrouter=\"" + ipGateway + "\"", TxtHelper.GetStrPos(rcConfPath, "defaultrouter", 0, -1));

            if (CheckParameter(context, "PreferredDNSServer"))
            {
                ShellHelper.RunCmd("cp -p " + resolvConf + " " + resolvConfPath);
                string[] dnsServers = ParseArray(context.Parameters["PreferredDNSServer"]);
                do
                {
                    pos = TxtHelper.GetStrPos(resolvConfPath, "nameserver", 0, -1);
                    if (pos != -1) TxtHelper.DelStr(resolvConfPath, pos);
                } while (pos != -1);
                foreach (var dnsServer in dnsServers)
                {
                    TxtHelper.ReplaceStr(resolvConfPath, "nameserver " + dnsServer, -1);
                }
                ShellHelper.RunCmd("cp -p " + resolvConfPath + " " + resolvConf);
            }

            if (ipAddresses.Length > 1)
            {
                for (int i = 1; i < ipAddresses.Length; i++)
                {
                    pos = TxtHelper.GetStrPos(rcConfPath, "ifconfig_" + adapter + "_alias" + (i-1).ToString(), 0, -1);
                    TxtHelper.ReplaceStr(rcConfPath, "ifconfig_" + adapter + "_alias" + (i - 1).ToString() + "=\"inet " + ipAddresses[i] + " netmask " + subnetMasks[i] + "\"", pos);
                }
            }

            int interfaceNum = ipAddresses.Length;
            do
            {
                pos = TxtHelper.GetStrPos(rcConfPath, "ifconfig_" + adapter + "_alias" + (interfaceNum - 1).ToString(), 0, -1);
                if (pos != -1) TxtHelper.DelStr(rcConfPath, pos);
                interfaceNum++;
            } while (pos != -1);

            ShellHelper.RunCmd("cp -p " + rcConfPath + " " + rcConf);

            ExecutionResult res = ShellHelper.RunCmd("service netif restart");
            if (res.ResultCode == 1)
            {
                ret.ResultCode = 1;
                ret.ErrorMessage = res.ErrorMessage;
            }
            res = ShellHelper.RunCmd("service routing restart");
            if (res.ResultCode == 1)
            {
                ret.ResultCode = 1;
                ret.ErrorMessage = res.ErrorMessage;
            }
        }

        private static void DisableDHCP(ExecutionContext context, String adapter, ExecutionResult ret)
        {
            List<string> oldIpList = GetAdapterIp(adapter);

            string[] ipGateways = ParseArray(context.Parameters["DefaultIPGateway"]);
            string[] ipAddresses = ParseArray(context.Parameters["IPAddress"]);
            string[] subnetMasks = ParseArray(context.Parameters["SubnetMask"]);
            if (subnetMasks.Length != ipAddresses.Length)
            {
                throw new ArgumentException("Number of Subnet Masks should be equal to IP Addresses");
            }
            string[] subnetMasksPrefix = new string[subnetMasks.Length];
            for (int i = 0; i < subnetMasks.Length; i++)
            {
                subnetMasksPrefix[i] = GetSubnetMaskPrefix(subnetMasks[i]);
            }

            switch (OsVersion.GetOsVersion())
            {
                case OsVersionEnum.Ubuntu:
                    DisableDHCP_Ubuntu(context, adapter, ret, ipAddresses, subnetMasksPrefix, subnetMasks, ipGateways[0]);
                    break;
                case OsVersionEnum.CentOS:
                    DisableDHCP_CentOS(context, adapter, ret, ipAddresses, subnetMasksPrefix, ipGateways[0]);
                    break;
                case OsVersionEnum.FreeBSD:
                    DisableDHCP_FreeBSD(context, adapter, ret, ipAddresses, subnetMasks, ipGateways[0]);
                    break;
            }

            if (oldIpList != null)//update ip in hosts
            {
                foreach (string ip in oldIpList)
                {
                    if (ip.Length > 0)
                    {
                        if (OsVersion.GetOsVersion() == OsVersionEnum.FreeBSD)
                        {
                            ShellHelper.RunCmd("cp -p " + hosts + " " + compatLinux + hosts);
                            TxtHelper.ReplaceStr(compatLinux + hosts, ip, ipAddresses[0]);
                            ShellHelper.RunCmd("cp -p " + compatLinux + hosts + " " + hosts);
                        }
                        else
                        {
                            TxtHelper.ReplaceStr(hosts, ip, ipAddresses[0]);
                        }
                    }
                }
            }
        }

        private static List<string> GetAdapterIp(string adapterName)
        {
            ExecutionResult res;
            if (OsVersion.GetOsVersion() == OsVersionEnum.FreeBSD)
            {
                res = ShellHelper.RunCmd("ifconfig " + adapterName + " inet | grep inet | awk '{print $2}'");
            }
            else
            {
                res = ShellHelper.RunCmd("ip addr show " + adapterName + " | grep \"inet \" | awk '{print $2}' | cut -d/ -f1");
            }
            if (res.ResultCode == 1) return null;
            List<string> IpList = StrToList(res.Value.Trim());
            return IpList;
        }

        private static int GetNetplanEndPos(string netplanPath, int startPos, int spacesCount)
        {
            int tmpPos = startPos;
            int endPos = tmpPos;
            do
            {
                tmpPos = TxtHelper.GetStrPos(netplanPath, new String(' ', spacesCount + 2), tmpPos, tmpPos);
                if (tmpPos != -1)
                {
                    endPos = tmpPos;
                    tmpPos++;
                }
            } while (tmpPos != -1);
            return endPos;
        }

        private static int GetNetplanSpacesCount(string str)
        {
            int spacesCount = 0;
            if (str != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == ' ')
                    {
                        spacesCount++;
                    }
                    else
                    {
                        return spacesCount;
                    }
                }
            }
            return 8;//default
        }

        private static string GetSubnetMaskPrefix(string subnetMask)
        {
            int startIdx = 0;
            string binary = "";
            for (int i = 0; i < 4; i++)
            {
                int num;
                int idx = subnetMask.IndexOf(".", startIdx);
                if (idx != -1)
                {
                    string str = subnetMask.Substring(startIdx, idx - startIdx);
                    num = Int32.Parse(str);
                    startIdx = idx + 1;
                }
                else
                {
                    string str = subnetMask.Substring(startIdx, subnetMask.Length - startIdx);
                    num = Int32.Parse(str);
                }
                string bin = Convert.ToString(num, 2);
                binary += bin;
            }
            int prefix = 0;
            for (int i = 0; i < binary.Length; i++)
            {
                if (binary[i] == '1') prefix++;
            }
            return prefix.ToString();
        }

        private static string GetNetplanPath()
        {
            ExecutionResult res = ShellHelper.RunCmd("ls " + netplanFolder + " | grep yaml");
            string defaultPath = netplanFolder + "50-cloud-init.yaml";
            if (res.ResultCode == 1) return defaultPath;
            string fileName = res.Value;
            if (fileName != null)
            {
                fileName = fileName.Replace("\n", "");
                if (fileName.IndexOf(" ") != -1)
                {
                    return defaultPath;
                }
                else
                {
                    return netplanFolder + fileName;
                }
            }
            else
            {
                return defaultPath;
            }
        }

        private static bool IsValidMACAddress(string mac)
        {
            return Regex.IsMatch(mac, "^([0-9a-fA-F]{2}:){5}[0-9a-fA-F]{2}$");
        }

        private static bool CheckParameter(ExecutionContext context, string name)
        {
            return (context.Parameters.ContainsKey(name) && !string.IsNullOrEmpty(context.Parameters[name]));
        }

        private static bool CheckParameter(ExecutionContext context, string name, string value)
        {
            return (context.Parameters.ContainsKey(name) && context.Parameters[name] == value);
        }
    }
}
