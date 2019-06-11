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
                    ProcessError(context, ret, null, 2, "Parameter 'DefaultIPGateway' is not specified");
                    return ret;
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

        private static List<Adapter> getAdapters()
        {
            List<Adapter> adapters = new List<Adapter>();
            ExecutionResult res = ShellHelper.RunCmd("ifconfig -s");
            if (res.ResultCode == 1) return null;
            string ifconfigRes = res.Value.Trim();
            List<string> ifconfigList = new List<string>();
            int startIdx = 0;
            int idx = -1;
            do
            {
                idx = ifconfigRes.IndexOf("\n", startIdx);
                if (idx != -1)
                {
                    string str = ifconfigRes.Substring(startIdx, idx - startIdx);
                    ifconfigList.Add(str);
                    startIdx = idx + 1;
                }
                else
                {
                    string str = ifconfigRes.Substring(startIdx, ifconfigRes.Length - startIdx);
                    ifconfigList.Add(str);
                }
            } while (idx != -1);
            foreach (string str in ifconfigList)
            {
                idx = str.IndexOf(" ");
                if (idx != -1)
                {
                    string adapterName = str.Substring(0, idx);
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
            }
            return adapters;
        }

        private static string GetAdapterMacAddress(string adapterName)
        {
            ExecutionResult res = ShellHelper.RunCmd("cat /sys/class/net/"+adapterName+"/address");
            if (res.ResultCode == 1) return null;
            string mac = res.Value;
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

        private static void EnableDHCP(String adapter, ExecutionResult ret)
        {
            Log.WriteStart("Enabling DHCP...");
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
                config.Add(new String(' ', spacesCount+2) + "dhcp4: yes");
                TxtHelper.ReplaceAllStr(netplanPath, config, startPos, endPos);
                ShellHelper.RunCmd("sudo netplan apply");
            }

            //iterfaces configuration (for Ubuntu 16.04-)
            startPos = TxtHelper.GetStrPos(interfacesPath, "iface " + adapter, 0, -1);
            if (startPos != -1)
            {
                int endPos = TxtHelper.GetStrPos(interfacesPath, "auto ", startPos, -1);
                if (endPos != -1) endPos--;
                List<string> config = new List<string>();
                config.Add("iface " + adapter + " inet dhcp");
                TxtHelper.ReplaceAllStr(interfacesPath, config, startPos, endPos);
                ret.RebootRequired = true;
            }
            Log.WriteEnd("DHCP enabled");
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
            //netplan configuration (for Ubuntu 18.04+)
            string netplanPath = GetNetplanPath();
            int startPos = TxtHelper.GetStrPos(netplanPath, adapter+":", 0, -1);
            if (startPos != -1)
            {
                string firstStr = TxtHelper.GetStr(netplanPath, adapter + ":", startPos, startPos);
                int spacesCount = GetNetplanSpacesCount(firstStr);                

                startPos++;
                int endPos = GetNetplanEndPos(netplanPath, startPos, spacesCount);
                List<string> config = new List<string>();
                string str = new String(' ', spacesCount+2) + "addresses: [";
                for (int i = 0; i < ipAddresses.Length; i++)
                {
                    str += ipAddresses[i] + "/" + subnetMasksPrefix[i];
                    if (i + 1 < ipAddresses.Length) str += ",";
                }
                str += "]";
                config.Add(str);
                config.Add(new String(' ', spacesCount + 2) + "gateway4: " + ipGateways[0]);
                config.Add(new String(' ', spacesCount + 2) + "nameservers:");
                if (CheckParameter(context, "PreferredDNSServer"))
                {
                    string[] dnsServers = ParseArray(context.Parameters["PreferredDNSServer"]);
                    str = new String(' ', spacesCount+4) + "addresses: [";
                    for (int i = 0; i < dnsServers.Length; i++)
                    {
                        str += dnsServers[i];
                        if (i + 1 < dnsServers.Length) str += ",";
                    }
                    str += "]";
                    config.Add(str);
                }
                config.Add(new String(' ', spacesCount + 2) + "dhcp4: no");
                TxtHelper.ReplaceAllStr(netplanPath, config, startPos, endPos);
                ShellHelper.RunCmd("sudo netplan apply");
            }
            //iterfaces configuration (for Ubuntu 16.04-)
            startPos = TxtHelper.GetStrPos(interfacesPath, "iface " + adapter, 0, -1);
            if (startPos != -1)
            {
                int endPos = TxtHelper.GetStrPos(interfacesPath, "auto ", startPos, -1);
                if (endPos != -1) endPos--;
                List<string> config = new List<string>();
                string str;
                for (int i = 0; i < ipAddresses.Length; i++)
                {
                    config.Add("iface " + adapter + " inet static");
                    config.Add("address " + ipAddresses[i]);
                    config.Add("netmask " + subnetMasks[i]);
                    config.Add("");
                }
                config.Add("gateway " + ipGateways[0]);
                if (CheckParameter(context, "PreferredDNSServer"))
                {
                    string[] dnsServers = ParseArray(context.Parameters["PreferredDNSServer"]);
                    str = "dns-nameservers ";
                    for (int t = 0; t < dnsServers.Length; t++)
                    {
                        str += dnsServers[t];
                        if (t + 1 < dnsServers.Length) str += " ";
                    }
                    config.Add(str);
                    config.Add("");
                }
                TxtHelper.ReplaceAllStr(interfacesPath, config, startPos, endPos);
                ret.RebootRequired = true;
            }

            foreach (string Ip in oldIpList)
            {
                TxtHelper.ReplaceStr(hosts, Ip, ipAddresses[0]);
            }
        }

        private static List<string> GetAdapterIp(string adapterName)
        {
            ExecutionResult res = ShellHelper.RunCmd("ip addr show "+ adapterName + " | grep \"inet \" | awk '{print $2}' | cut -d/ -f1");
            if (res.ResultCode == 1) return null;
            string strIps = res.Value.Trim();
            List<string> IpList = new List<string>();
            int startIdx = 0;
            int idx = -1;
            do
            {
                idx = strIps.IndexOf("\n", startIdx);
                if (idx != -1)
                {
                    string str = strIps.Substring(startIdx, idx - startIdx);
                    IpList.Add(str);
                    startIdx = idx + 1;
                }
                else
                {
                    string str = strIps.Substring(startIdx, strIps.Length - startIdx);
                    IpList.Add(str);
                }
            } while (idx != -1);
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
            int idx = -1;
            string binary = "";
            for (int i = 0; i < 4; i++)
            {
                int num;
                idx = subnetMask.IndexOf(".", startIdx);
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
