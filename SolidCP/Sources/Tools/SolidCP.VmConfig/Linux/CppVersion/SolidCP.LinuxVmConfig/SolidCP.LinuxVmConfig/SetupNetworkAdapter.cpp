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

#include "SetupNetworkAdapter.h"

const string SetupNetworkAdapter::netplanFolder = "/etc/netplan/";
const string SetupNetworkAdapter::interfacesPath = "/etc/network/interfaces";
const string SetupNetworkAdapter::hostsPath = "/etc/hosts";
const string SetupNetworkAdapter::ifcfgBasePath = "/etc/sysconfig/network-scripts/ifcfg-";
const string SetupNetworkAdapter::rcConfPath = "/etc/rc.conf";
const string SetupNetworkAdapter::resolvConfPath = "/etc/resolv.conf";
const string SetupNetworkAdapter::configXmlPath = "/cf/conf/config.xml";

typedef struct SetupNetworkAdapter::Adapter {
	string Name;
	string MAC;
	Adapter(string name, string mac)
	{
		Name = name;
		MAC = mac;
	}
} Adapter;

ExecutionResult SetupNetworkAdapter::Run(ExecutionContext& context)
{
	ExecutionResult ret;
	string adapterName = "";
	ret.ResultCode = 0;
	ret.RebootRequired = false;

	context.ActivityDescription = "Configuring network adapter...";
	context.Progress = 0;
	if (!CheckParameter(context, "MAC"))
	{
		ProcessError(context, ret, 2, "Parameter 'MAC' is not specified");
		return ret;
	}
	string macAddress = context.Parameters["MAC"];
	if (!IsValidMACAddress(macAddress))
	{
		ProcessError(context, ret, 2, "Parameter 'MAC' has invalid format. It should be in 12:34:56:78:90:ab format.");
		return ret;
	}
	try
	{
		vector<Adapter> adapters = GetAdapters();
		if (adapters.empty())
		{
			ProcessError(context, ret, 2, "No adapters found.");
			return ret;
		}
		for (int i=0; i<adapters.size(); i++)
		{
			string mac = adapters[i].MAC;
			transform(macAddress.begin(), macAddress.end(), macAddress.begin(), ::tolower);
			transform(mac.begin(), mac.end(), mac.begin(), ::tolower);
			if (macAddress == mac)
			{
				adapterName = adapters[i].Name;
				break;
			}
		}
		if (adapterName == "")
		{
			ProcessError(context, ret, 2, "Adapter not found.");
			return ret;
		}
	}
	catch (exception ex)
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
		catch (exception ex)
		{
			ProcessError(context, ret, ex, 2, "DHCP error: ");
			return ret;
		}

	}
	else if (CheckParameter(context, "EnableDHCP", "False"))
	{
		if (!CheckParameter(context, "DefaultIPGateway"))
		{
			ProcessError(context, ret, 2, "Parameter 'DefaultIPGateway' is not specified");
			return ret;
		}
		if (!CheckParameter(context, "IPAddress"))
		{
			ProcessError(context, ret, 2, "Parameter 'IPAddresses' is not specified");
			return ret;
		}
		if (!CheckParameter(context, "SubnetMask"))
		{
			ProcessError(context, ret, 2, "Parameter 'SubnetMasks' is not specified");
			return ret;
		}
		try
		{
			DisableDHCP(context, adapterName, ret);
		}
		catch (exception ex)
		{
			ProcessError(context, ret, ex, 2, "Network adapter configuration error: ");
			return ret;
		}
	}
	context.Progress = 100;
	return ret;
}

vector<string> SetupNetworkAdapter::StrToList(string str)
{
	vector<string> result;
	int startIdx = 0;
	int idx;
	do
	{
		idx = str.find("\n", startIdx);
		if (idx != -1)
		{
			string subSstr = str.substr(startIdx, idx - startIdx);
			result.push_back(subSstr);
			startIdx = idx + 1;
		}
		else
		{
			string subSstr = str.substr(startIdx, str.length() - startIdx);
			result.push_back(subSstr);
		}
	} while (idx != -1);
	return result;
}

vector<Adapter> SetupNetworkAdapter::GetAdapters()
{
	vector<Adapter> adapters;
	ExecutionResult res;
	switch (OsVersion::GetOsVersion())
	{
	case Ubuntu:
		res = ShellHelper::RunCmd("ifconfig -s | awk '{print $1}'");
		break;
	case CentOS:
		res = ShellHelper::RunCmd("nmcli -p dev | grep \"ethernet\" | awk '{print $1}'");
		break;
	case FreeBSD:
		res = ShellHelper::RunCmd("ifconfig -l");
		if (res.Value != "") StrHelper::ReplaceAll(res.Value, " ", "\n");
		break;
	case PfSense:
		res = ShellHelper::RunCmd("ifconfig -l");
		if (res.Value != "") StrHelper::ReplaceAll(res.Value, " ", "\n");
		break;
	}
	if (res.Value == "" || res.ResultCode == 1) return adapters;
	StrHelper::Trim(res.Value);
	vector<string> adaptersStr = StrToList(res.Value);

	for (int i=0; i<adaptersStr.size(); i++)
	{
		string adapterName = adaptersStr[i];
		if (adapterName != "Iface" && adapterName != "lo")
		{
			string mac = GetAdapterMacAddress(adapterName);
			if (mac != "")
			{
				Adapter adapter(adapterName, mac);
				adapters.push_back(adapter);
			}
		}
	}
	return adapters;
}

string SetupNetworkAdapter::GetAdapterMacAddress(string adapterName)
{
	ExecutionResult res;
	if (OsVersion::IsFreeBsdOs())
	{
		res = ShellHelper::RunCmd("ifconfig " + adapterName + " ether | grep ether | awk '{print $2}'");
	}
	else
	{
		res = ShellHelper::RunCmd("cat /sys/class/net/" + adapterName + "/address");
	}
	if (res.ResultCode == 1 || res.Value == "") return "";
	string mac = res.Value;
	StrHelper::Trim(mac);
	if (mac != "")
	{
		StrHelper::ReplaceAll(mac, "\n", "");
	}
	else
	{
		return "";
	}
	if (IsValidMACAddress(mac))
	{
		return mac;
	}
	else
	{
		return "";
	}
}

vector<string> SetupNetworkAdapter::ParseArray(string array)
{
	if (array.length() == 0) throw invalid_argument("array");
	return StrHelper::Split(array, ';');
}

void SetupNetworkAdapter::ProcessError(ExecutionContext& context, ExecutionResult& ret, exception& ex, int errorCode, string errorPrefix)
{
	ret.ResultCode = errorCode;
	ret.ErrorMessage = errorPrefix;
	ret.ErrorMessage += ex.what();
	Log::WriteError(ret.ErrorMessage);
	context.Progress = 100;
}

void SetupNetworkAdapter::ProcessError(ExecutionContext& context, ExecutionResult& ret, int errorCode, string errorPrefix)
{
	ret.ResultCode = errorCode;
	ret.ErrorMessage = errorPrefix;
	Log::WriteError(ret.ErrorMessage);
	context.Progress = 100;
}

void SetupNetworkAdapter::EnableDHCP_Ubuntu(string adapter, ExecutionResult& ret)
{
	//netplan configuration (for Ubuntu 18.04+)
	string netplanPath = GetNetplanPath();
	int startPos = TxtHelper::GetStrPos(netplanPath, adapter + ":", 0, -1);
	if (startPos != -1)
	{
		string firstStr = TxtHelper::GetStr(netplanPath, adapter + ":", startPos, startPos);
		int spacesCount = GetNetplanSpacesCount(firstStr);

		startPos++;
		int endPos = GetNetplanEndPos(netplanPath, startPos, spacesCount);
		vector<string> config;
		config.push_back(string(spacesCount + 2, ' ') + "dhcp4: yes");
		TxtHelper::ReplaceLines(netplanPath, config, startPos, endPos);
		ShellHelper::RunCmd("netplan apply");
	}

	//iterfaces configuration (for Ubuntu 16.04-)
	startPos = TxtHelper::GetStrPos(interfacesPath, "iface " + adapter, 0, -1);
	if (startPos != -1)
	{
		int endPos = TxtHelper::GetStrPos(interfacesPath, "auto ", startPos, -1);
		if (endPos != -1) endPos--;
		vector<string> config;
		config.push_back("iface " + adapter + " inet dhcp");
		TxtHelper::ReplaceLines(interfacesPath, config, startPos, endPos);
		ret.RebootRequired = true;
	}
}

void SetupNetworkAdapter::EnableDHCP_CentOS(string adapter, ExecutionResult& ret)
{
	string adapterPath = ifcfgBasePath + adapter;
	TxtHelper::ReplaceStr(adapterPath, "BOOTPROTO=dhcp", TxtHelper::GetStrPos(adapterPath, "BOOTPROTO", 0, -1));
	TxtHelper::ReplaceStr(adapterPath, "ONBOOT=yes", TxtHelper::GetStrPos(adapterPath, "ONBOOT", 0, -1));
	ExecutionResult res = ShellHelper::RunCmd("systemctl restart network");
	if (res.ResultCode == 1)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = res.ErrorMessage;
	}
}

void SetupNetworkAdapter::EnableDHCP_FreeBSD(string adapter, ExecutionResult& ret)
{
	int pos = TxtHelper::GetStrPos(rcConfPath, "ifconfig_" + adapter + "=", 0, -1);
	if (pos == -1) pos = TxtHelper::GetStrPos(rcConfPath, "ifconfig_" + adapter + " =", 0, -1);
	TxtHelper::ReplaceStr(rcConfPath, "ifconfig_" + adapter + "=\"DHCP\"", pos);

	ExecutionResult res = ShellHelper::RunCmd("service netif restart");
	if (res.ResultCode == 1)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = res.ErrorMessage;
	}
	res = ShellHelper::RunCmd("service routing restart");
	if (res.ResultCode == 1)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = res.ErrorMessage;
	}
}

void SetupNetworkAdapter::EnableDHCP_PfSense(string adapter, ExecutionResult& ret)
{
	vector<int> adapterPos = GetPfSenseAdapterPos(adapter);
	int adapterStartPos = adapterPos[0];
	int adapterEndPos = adapterPos[1];
	if (adapterStartPos == -1 || adapterEndPos == -1)
	{
		string error = "Adapter \"" + adapter + "\" not found in config.xml";
		ret.ResultCode = 1;
		ret.ErrorMessage = error;
		Log::WriteError("EnableDHCP_PfSense error: " + error);
		return;
	}

	int startPos = TxtHelper::GetStrPos(configXmlPath, "<ipaddr>", adapterStartPos, adapterEndPos);
	if (startPos != -1) {
		int endPos = TxtHelper::GetStrPos(configXmlPath, "</ipaddr>", startPos, adapterEndPos);
		TxtHelper::DelLines(configXmlPath, startPos, endPos);
	}

	startPos = TxtHelper::GetStrPos(configXmlPath, "<subnet>", adapterStartPos, adapterEndPos);
	if (startPos != -1) {
		int endPos = TxtHelper::GetStrPos(configXmlPath, "</subnet>", startPos, adapterEndPos);
		TxtHelper::DelLines(configXmlPath, startPos, endPos);
	}

	startPos = TxtHelper::GetStrPos(configXmlPath, "<gateway>", adapterStartPos, adapterEndPos);
	if (startPos != -1) {
		int endPos = TxtHelper::GetStrPos(configXmlPath, "</gateway>", startPos, adapterEndPos);
		TxtHelper::DelLines(configXmlPath, startPos, endPos);
	}

	TxtHelper::InsertStr(configXmlPath, "\t\t\t<gateway></gateway>", adapterStartPos + 1);
	TxtHelper::InsertStr(configXmlPath, "\t\t\t<subnet></subnet>", adapterStartPos + 1);
	TxtHelper::InsertStr(configXmlPath, "\t\t\t<ipaddr>dhcp</ipaddr>", adapterStartPos + 1);

	ExecutionResult res = ShellHelper::RunCmd("/etc/rc.reload_interfaces");
	if (res.ResultCode == 1)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = res.ErrorMessage;
	}
}

vector<int> SetupNetworkAdapter::GetPfSenseAdapterPos(string adapter)
{
	vector<int> result;
	result.push_back(-1);
	result.push_back(-1);
	int interfacesStart = TxtHelper::GetStrPos(configXmlPath, "<interfaces>", 0, -1);
	int interfacesEnd = TxtHelper::GetStrPos(configXmlPath, "</interfaces>", interfacesStart, -1);
	int adapterNamePos = -1;
	int adapterStartPos = -1;
	int adapterEndPos = -1;
	int ifStart = interfacesStart;
	int ifEnd;
	do
	{
		ifStart = TxtHelper::GetStrPos(configXmlPath, "<if>", ifStart, interfacesEnd);
		if (ifStart != -1)
		{
			ifEnd = TxtHelper::GetStrPos(configXmlPath, "</if>", ifStart, interfacesEnd);
			adapterNamePos = TxtHelper::GetStrPos(configXmlPath, adapter, ifStart, ifEnd);
			if (adapterNamePos != -1) break;
			ifStart++;
		}
	} while (ifStart != -1);
	if (adapterNamePos == -1) return result;
	int wanStartPos = TxtHelper::GetStrPos(configXmlPath, "<wan>", interfacesStart, interfacesEnd);
	int wanEndPos = TxtHelper::GetStrPos(configXmlPath, "</wan>", wanStartPos, interfacesEnd);
	int lanStartPos = TxtHelper::GetStrPos(configXmlPath, "<lan>", interfacesStart, interfacesEnd);
	int lanEndPos = TxtHelper::GetStrPos(configXmlPath, "</lan>", lanStartPos, interfacesEnd);
	if (adapterNamePos >= wanStartPos && adapterNamePos <= wanEndPos)
	{
		adapterStartPos = wanStartPos;
		adapterEndPos = wanEndPos;
	}
	else if (adapterNamePos >= lanStartPos && adapterNamePos <= lanEndPos)
	{
		adapterStartPos = lanStartPos;
		adapterEndPos = lanEndPos;
	}
	result[0] = adapterStartPos;
	result[1] = adapterEndPos;
	return result;
}

vector<int> SetupNetworkAdapter::GetPfSenseGatewayPos(string interfaceTyp)
{
	vector<int> result;
	result.push_back(-1);
	result.push_back(-1);
	int gatewaysStart = TxtHelper::GetStrPos(configXmlPath, "<gateways>", 0, -1);
	int gatewaysEnd = TxtHelper::GetStrPos(configXmlPath, "</gateways>", gatewaysStart, -1);
	int gatewayStartPos = -1;
	int gatewayEndPos = -1;

	if (gatewaysStart == -1 || gatewaysEnd == -1) return result;

	gatewayStartPos = gatewaysStart;
	do
	{
		gatewayStartPos = TxtHelper::GetStrPos(configXmlPath, "<gateway_item>", gatewayStartPos, gatewaysEnd);
		if (gatewayStartPos != -1)
		{
			gatewayEndPos = TxtHelper::GetStrPos(configXmlPath, "</gateway_item>", gatewayStartPos, gatewaysEnd);
			if (TxtHelper::GetStrPos(configXmlPath, "<interface>" + interfaceTyp + "</interface>", gatewayStartPos, gatewayEndPos) != -1)
			{
				result[0] = gatewayStartPos;
				result[1] = gatewayEndPos;
				return result;
			}
			gatewayStartPos = gatewayEndPos + 1;
		}
	} while (gatewayStartPos != -1);
	return result;
}

vector<int> SetupNetworkAdapter::GetPfSenseVipPos(string interfaceTyp)
{
	vector<int> result;
	result.push_back(-1);
	result.push_back(-1);
	int virtualipStart = TxtHelper::GetStrPos(configXmlPath, "<virtualip>", 0, -1);
	if (virtualipStart == -1)
	{
		int pfsenseEnd = TxtHelper::GetStrPos(configXmlPath, "</pfsense>", 0, -1);
		TxtHelper::InsertStr(configXmlPath, "\t<virtualip>\n\t</virtualip>", pfsenseEnd);
		virtualipStart = TxtHelper::GetStrPos(configXmlPath, "<virtualip>", 0, -1);
	}
	int virtualipEnd = TxtHelper::GetStrPos(configXmlPath, "</virtualip>", virtualipStart, -1);
	int vipStartPos = -1;
	int vipEndPos = -1;

	if (virtualipStart == -1 || virtualipEnd == -1) return result;

	vipStartPos = virtualipStart;
	do
	{
		vipStartPos = TxtHelper::GetStrPos(configXmlPath, "<vip>", vipStartPos, virtualipEnd);
		if (vipStartPos != -1)
		{
			vipEndPos = TxtHelper::GetStrPos(configXmlPath, "</vip>", vipStartPos, virtualipEnd);
			if (TxtHelper::GetStrPos(configXmlPath, "<interface>" + interfaceTyp + "</interface>", vipStartPos, vipEndPos) != -1)
			{
				result[0] = vipStartPos;
				result[1] = vipEndPos;
				return result;
			}
			vipStartPos = vipEndPos + 1;
		}
	} while (vipStartPos != -1);
	return result;
}

void SetupNetworkAdapter::EnableDHCP(string adapter, ExecutionResult& ret)
{
	Log::WriteStart("Enabling DHCP...");
	switch (OsVersion::GetOsVersion())
	{
	case Ubuntu:
		EnableDHCP_Ubuntu(adapter, ret);
		break;
	case CentOS:
		EnableDHCP_CentOS(adapter, ret);
		break;
	case FreeBSD:
		EnableDHCP_FreeBSD(adapter, ret);
		break;
	case PfSense:
		EnableDHCP_PfSense(adapter, ret);
		break;
	}
	Log::WriteEnd("DHCP enabled");
}

void SetupNetworkAdapter::DisableDHCP_Ubuntu(ExecutionContext& context, string adapter, ExecutionResult& ret, vector<string> ipAddresses, vector<string> subnetMasksPrefix, vector<string> subnetMasks, string ipGateway)
{
	//netplan configuration (for Ubuntu 18.04+)
	string netplanPath = GetNetplanPath();
	int startPos = TxtHelper::GetStrPos(netplanPath, adapter + ":", 0, -1);
	if (startPos != -1)
	{
		string firstStr = TxtHelper::GetStr(netplanPath, adapter + ":", startPos, startPos);
		int spacesCount = GetNetplanSpacesCount(firstStr);

		startPos++;
		int endPos = GetNetplanEndPos(netplanPath, startPos, spacesCount);
		vector<string> config;
		string str = string(spacesCount + 2, ' ') + "addresses: [";
		for (int i = 0; i < ipAddresses.size(); i++)
		{
			str += ipAddresses[i] + "/" + subnetMasksPrefix[i];
			if (i + 1 < ipAddresses.size()) str += ",";
		}
		str += "]";
		config.push_back(str);
		config.push_back(string(spacesCount + 2, ' ') + "gateway4: " + ipGateway);
		config.push_back(string(spacesCount + 2, ' ') + "nameservers:");
		if (CheckParameter(context, "PreferredDNSServer"))
		{
			vector<string> dnsServers = ParseArray(context.Parameters["PreferredDNSServer"]);
			str = string(spacesCount + 4, ' ') + "addresses: [";
			for (int i = 0; i < dnsServers.size(); i++)
			{
				str += dnsServers[i];
				if (i + 1 < dnsServers.size()) str += ",";
			}
			str += "]";
			config.push_back(str);
		}
		config.push_back(string(spacesCount + 2, ' ') + "dhcp4: no");
		TxtHelper::ReplaceLines(netplanPath, config, startPos, endPos);
		ShellHelper::RunCmd("netplan apply");
	}
	//iterfaces configuration (for Ubuntu 16.04-)
	startPos = TxtHelper::GetStrPos(interfacesPath, "iface " + adapter, 0, -1);
	if (startPos != -1)
	{
		int endPos = TxtHelper::GetStrPos(interfacesPath, "auto ", startPos, -1);
		if (endPos != -1) endPos--;
		vector<string> config;
		string str;
		for (int i = 0; i < ipAddresses.size(); i++)
		{
			config.push_back("iface " + adapter + " inet static");
			config.push_back("address " + ipAddresses[i]);
			config.push_back("netmask " + subnetMasks[i]);
			config.push_back("");
		}
		config.push_back("gateway " + ipGateway);
		if (CheckParameter(context, "PreferredDNSServer"))
		{
			vector<string> dnsServers = ParseArray(context.Parameters["PreferredDNSServer"]);
			str = "dns-nameservers ";
			for (int t = 0; t < dnsServers.size(); t++)
			{
				str += dnsServers[t];
				if (t + 1 < dnsServers.size()) str += " ";
			}
			config.push_back(str);
			config.push_back("");
		}
		TxtHelper::ReplaceLines(interfacesPath, config, startPos, endPos);
		ret.RebootRequired = true;
	}
}

void SetupNetworkAdapter::DisableDHCP_PfSense(ExecutionContext& context, string adapter, ExecutionResult& ret, vector<string> ipAddresses, vector<string> subnetMasksPrefix, string ipGateway)
{
	vector<int> adapterPos = GetPfSenseAdapterPos(adapter);
	int adapterStartPos = adapterPos[0];
	int adapterEndPos = adapterPos[1];
	if (adapterStartPos == -1 || adapterEndPos == -1)
	{
		string error = "Adapter \"" + adapter + "\" not found in config.xml";
		ret.ResultCode = 1;
		ret.ErrorMessage = error;
		Log::WriteError("EnableDHCP_PfSense error: " + error);
		return;
	}

	int startPos = TxtHelper::GetStrPos(configXmlPath, "<ipaddr>", adapterStartPos, adapterEndPos);
	if (startPos != -1) {
		int endPos = TxtHelper::GetStrPos(configXmlPath, "</ipaddr>", startPos, adapterEndPos);
		TxtHelper::DelLines(configXmlPath, startPos, endPos);
	}

	startPos = TxtHelper::GetStrPos(configXmlPath, "<subnet>", adapterStartPos, adapterEndPos);
	if (startPos != -1) {
		int endPos = TxtHelper::GetStrPos(configXmlPath, "</subnet>", startPos, adapterEndPos);
		TxtHelper::DelLines(configXmlPath, startPos, endPos);
	}

	string interfaceTyp = TxtHelper::GetStr(configXmlPath, adapterStartPos);
	StrHelper::Replace(interfaceTyp, "<", "");
	StrHelper::Replace(interfaceTyp, ">", "");
	StrHelper::ReplaceAll(interfaceTyp, "\t", "");
	StrHelper::ReplaceAll(interfaceTyp, "\n", "");
	StrHelper::ReplaceAll(interfaceTyp, " ", "");
	string gatewayName = "";

	if (interfaceTyp != "lan")//use template settings for private network
	{
		startPos = TxtHelper::GetStrPos(configXmlPath, "<gateway>", adapterStartPos, adapterEndPos);
		if (startPos != -1) {
			int endPos = TxtHelper::GetStrPos(configXmlPath, "</gateway>", startPos, adapterEndPos);
			TxtHelper::DelLines(configXmlPath, startPos, endPos);
		}

		vector<int> gatewayPos = GetPfSenseGatewayPos(interfaceTyp);
		int gatewayStartPos = gatewayPos[0];
		int gatewayEndPos = gatewayPos[1];
		if (gatewayStartPos != -1 && gatewayEndPos != -1)
		{
			int gIpStart = TxtHelper::GetStrPos(configXmlPath, "<gateway>", gatewayStartPos, gatewayEndPos);
			if (gIpStart != -1)
			{
				int gIpEnd = TxtHelper::GetStrPos(configXmlPath, "</gateway>", gIpStart, gatewayEndPos);
				TxtHelper::DelLines(configXmlPath, gIpStart, gIpEnd);
				TxtHelper::InsertStr(configXmlPath, "\t\t\t<gateway>" + ipGateway + "</gateway>", gIpStart);
			}
			else
			{
				TxtHelper::InsertStr(configXmlPath, "\t\t\t<gateway>" + ipGateway + "</gateway>", gatewayStartPos + 1);
			}

			int nameStart = TxtHelper::GetStrPos(configXmlPath, "<name>", gatewayStartPos, gatewayEndPos);
			if (nameStart != -1)
			{
				int nameEnd = TxtHelper::GetStrPos(configXmlPath, "</name>", nameStart, gatewayEndPos);
				for (int i = nameStart; i <= nameEnd; i++)
				{
					gatewayName.append(TxtHelper::GetStr(configXmlPath, i));
				}
				StrHelper::ReplaceAll(gatewayName, "<name>", "");
				StrHelper::ReplaceAll(gatewayName, "</name>", "");
				StrHelper::ReplaceAll(gatewayName, "\t", "");
				StrHelper::ReplaceAll(gatewayName, "\n", "");
				StrHelper::ReplaceAll(gatewayName, " ", "");
			}
		}
	}
	
	if (gatewayName.length() > 0) TxtHelper::InsertStr(configXmlPath, "\t\t\t<gateway>" + gatewayName + "</gateway>", adapterStartPos + 1);
	TxtHelper::InsertStr(configXmlPath, "\t\t\t<subnet>" + subnetMasksPrefix[0] + "</subnet>", adapterStartPos + 1);
	TxtHelper::InsertStr(configXmlPath, "\t\t\t<ipaddr>" + ipAddresses[0] + "</ipaddr>", adapterStartPos + 1);

	vector<int> vipPos = GetPfSenseVipPos(interfaceTyp);
	while (vipPos[0] != -1)
	{
		TxtHelper::DelLines(configXmlPath, vipPos[0], vipPos[1]);
		vipPos = GetPfSenseVipPos(interfaceTyp);
	}

	int virtualipEndPos = TxtHelper::GetStrPos(configXmlPath, "</virtualip>", 0, -1);
	vector<string> config;
	for (int i = 1; i < ipAddresses.size(); i++)
	{
		config.clear();
		config.push_back("\t\t<vip>");
		config.push_back("\t\t\t<mode>ipalias</mode>");
		config.push_back("\t\t\t<interface>" + interfaceTyp + "</interface>");
		config.push_back("\t\t\t<uniqid>" + GetUniqId(13) + "</uniqid>");
		config.push_back("\t\t\t<descr>SolidCP virtual IP</descr>");
		config.push_back("\t\t\t<type>single</type>");
		config.push_back("\t\t\t<subnet_bits>" + subnetMasksPrefix[i] + "</subnet_bits>");
		config.push_back("\t\t\t<subnet>" + ipAddresses[i] + "</subnet>");
		config.push_back("\t\t</vip>");
		TxtHelper::InsertLines(configXmlPath, config, virtualipEndPos);
	}

	if (interfaceTyp != "lan")//use template settings for private network
	{
		if (CheckParameter(context, "PreferredDNSServer"))
		{
			int systemStart = TxtHelper::GetStrPos(configXmlPath, "<system>", 0, -1);
			int systemEnd = TxtHelper::GetStrPos(configXmlPath, "</system>", systemStart, -1);
			vector<string> dnsServers = ParseArray(context.Parameters["PreferredDNSServer"]);
			for (int i = 0; i < dnsServers.size(); i++)
			{
				string dnsRecord = "<dnsserver>" + dnsServers[i] + "</dnsserver>";
				if (TxtHelper::GetStrPos(configXmlPath, dnsRecord, systemStart, systemEnd) == -1)
				{
					TxtHelper::InsertStr(configXmlPath, "\t\t" + dnsRecord, systemEnd);
					systemEnd++;
				}
			}
		}
	}

	ExecutionResult res = ShellHelper::RunCmd("/etc/rc.reload_interfaces");
	if (res.ResultCode == 1)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = res.ErrorMessage;
	}
}

string SetupNetworkAdapter::GetUniqId(int length)
{
	string result = "";
	random_device dev;
	mt19937 rng(dev());
	uniform_int_distribution<mt19937::result_type> distr(0, 15);
	for (int i = 0; i < length; i++)
	{
		stringstream sstream;
		sstream << hex << distr(rng);
		result.append(sstream.str());
	}
	return result;
}

void SetupNetworkAdapter::DisableDHCP_CentOS(ExecutionContext& context, string adapter, ExecutionResult& ret, vector<string> ipAddresses, vector<string> subnetMasksPrefix, string ipGateway)
{
	string adapterPath = ifcfgBasePath + adapter;
	TxtHelper::ReplaceStr(adapterPath, "BOOTPROTO=none", TxtHelper::GetStrPos(adapterPath, "BOOTPROTO", 0, -1));
	TxtHelper::ReplaceStr(adapterPath, "ONBOOT=yes", TxtHelper::GetStrPos(adapterPath, "ONBOOT", 0, -1));
	TxtHelper::ReplaceStr(adapterPath, "IPADDR=" + ipAddresses[0], TxtHelper::GetStrPos(adapterPath, "IPADDR", 0, -1));
	TxtHelper::ReplaceStr(adapterPath, "PREFIX=" + subnetMasksPrefix[0], TxtHelper::GetStrPos(adapterPath, "PREFIX", 0, -1));
	TxtHelper::ReplaceStr(adapterPath, "GATEWAY=" + ipGateway, TxtHelper::GetStrPos(adapterPath, "GATEWAY", 0, -1));
	if (CheckParameter(context, "PreferredDNSServer"))
	{
		vector<string> dnsServers = ParseArray(context.Parameters["PreferredDNSServer"]);
		for (int i = 0; i < dnsServers.size(); i++)
		{
			TxtHelper::ReplaceStr(adapterPath, "DNS" + to_string(i + 1) + "=" + dnsServers[i], TxtHelper::GetStrPos(adapterPath, "DNS" + to_string(i + 1), 0, -1));
		}
	}
	if (ipAddresses.size() > 1)
	{
		for (int i = 1; i < ipAddresses.size(); i++)
		{
			TxtHelper::ReplaceStr(adapterPath, "IPADDR" + to_string(i) + "=" + ipAddresses[i], TxtHelper::GetStrPos(adapterPath, "IPADDR" + to_string(i), 0, -1));
			TxtHelper::ReplaceStr(adapterPath, "PREFIX" + to_string(i) + "=" + subnetMasksPrefix[i], TxtHelper::GetStrPos(adapterPath, "PREFIX" + to_string(i), 0, -1));
		}
	}
	int pos;
	int interfaceNum = ipAddresses.size();
	do
	{
		pos = TxtHelper::GetStrPos(adapterPath, "IPADDR" + to_string(interfaceNum), 0, -1);
		if (pos != -1) TxtHelper::DelStr(adapterPath, pos);
		pos = TxtHelper::GetStrPos(adapterPath, "PREFIX" + to_string(interfaceNum), 0, -1);
		if (pos != -1) TxtHelper::DelStr(adapterPath, pos);
		interfaceNum++;
	} while (pos != -1);
	ExecutionResult res = ShellHelper::RunCmd("systemctl restart network");
	if (res.ResultCode == 1)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = res.ErrorMessage;
	}
}

void SetupNetworkAdapter::DisableDHCP_FreeBSD(ExecutionContext& context, string adapter, ExecutionResult& ret, vector<string> ipAddresses, vector<string> subnetMasks, string ipGateway)
{
	int pos = TxtHelper::GetStrPos(rcConfPath, "ifconfig_" + adapter + "=", 0, -1);
	if (pos == -1) pos = TxtHelper::GetStrPos(rcConfPath, "ifconfig_" + adapter + " =", 0, -1);
	TxtHelper::ReplaceStr(rcConfPath, "ifconfig_" + adapter + "=\"inet " + ipAddresses[0] + " netmask " + subnetMasks[0] + "\"", pos);
	/*TxtHelper::ReplaceStr(rcConfPath, "defaultrouter=\"" + ipGateway + "\"", TxtHelper::GetStrPos(rcConfPath, "defaultrouter", 0, -1));

	if (CheckParameter(context, "PreferredDNSServer"))
	{
		vector<string> dnsServers = ParseArray(context.Parameters["PreferredDNSServer"]);
		do
		{
			pos = TxtHelper::GetStrPos(resolvConfPath, "nameserver", 0, -1);
			if (pos != -1) TxtHelper::DelStr(resolvConfPath, pos);
		} while (pos != -1);
		for (int i=0; i<dnsServers.size(); i++)
		{
			TxtHelper::ReplaceStr(resolvConfPath, "nameserver " + dnsServers[i], -1);
		}
	}*///use template DNS and Gateway settings

	if (ipAddresses.size() > 1)
	{
		for (int i = 1; i < ipAddresses.size(); i++)
		{
			pos = TxtHelper::GetStrPos(rcConfPath, "ifconfig_" + adapter + "_alias" + to_string(i - 1), 0, -1);
			TxtHelper::ReplaceStr(rcConfPath, "ifconfig_" + adapter + "_alias" + to_string(i - 1) + "=\"inet " + ipAddresses[i] + " netmask " + subnetMasks[i] + "\"", pos);
		}
	}

	int interfaceNum = ipAddresses.size();
	do
	{
		pos = TxtHelper::GetStrPos(rcConfPath, "ifconfig_" + adapter + "_alias" + to_string(interfaceNum - 1), 0, -1);
		if (pos != -1) TxtHelper::DelStr(rcConfPath, pos);
		interfaceNum++;
	} while (pos != -1);
	ExecutionResult res = ShellHelper::RunCmd("service netif restart");
	if (res.ResultCode == 1)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = res.ErrorMessage;
	}
	res = ShellHelper::RunCmd("service routing restart");
	if (res.ResultCode == 1)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = res.ErrorMessage;
	}
}

void SetupNetworkAdapter::DisableDHCP(ExecutionContext& context, string adapter, ExecutionResult& ret)
{
	vector<string> oldIpList = GetAdapterIp(adapter);

	vector<string> ipGateways = ParseArray(context.Parameters["DefaultIPGateway"]);
	vector<string> ipAddresses = ParseArray(context.Parameters["IPAddress"]);
	vector<string> subnetMasks = ParseArray(context.Parameters["SubnetMask"]);
	if (subnetMasks.size() != ipAddresses.size())
	{
		throw invalid_argument("Number of Subnet Masks should be equal to IP Addresses");
	}
	vector<string> subnetMasksPrefix;
	for (int i = 0; i < subnetMasks.size(); i++)
	{
		subnetMasksPrefix.push_back(GetSubnetMaskPrefix(subnetMasks[i]));
	}

	switch (OsVersion::GetOsVersion())
	{
	case Ubuntu:
		DisableDHCP_Ubuntu(context, adapter, ret, ipAddresses, subnetMasksPrefix, subnetMasks, ipGateways[0]);
		break;
	case CentOS:
		DisableDHCP_CentOS(context, adapter, ret, ipAddresses, subnetMasksPrefix, ipGateways[0]);
		break;
	case FreeBSD:
		DisableDHCP_FreeBSD(context, adapter, ret, ipAddresses, subnetMasks, ipGateways[0]);
		break;
	case PfSense:
		DisableDHCP_PfSense(context, adapter, ret, ipAddresses, subnetMasksPrefix, ipGateways[0]);
		break;
	}

	if (!oldIpList.empty())//update ip in hosts
	{
		for (int i=0; i<oldIpList.size(); i++)
		{
			string ip = oldIpList[i];
			if (ip.length() > 0) TxtHelper::ReplaceStr(hostsPath, ip, ipAddresses[0]);
		}
	}
}

vector<string> SetupNetworkAdapter::GetAdapterIp(string adapterName)
{
	ExecutionResult res;
	vector<string> IpList;
	if (OsVersion::IsFreeBsdOs())
	{
		res = ShellHelper::RunCmd("ifconfig " + adapterName + " inet | grep inet | awk '{print $2}'");
	}
	else
	{
		res = ShellHelper::RunCmd("ip addr show " + adapterName + " | grep \"inet \" | awk '{print $2}' | cut -d/ -f1");
	}
	if (res.ResultCode == 1) return IpList;
	StrHelper::Trim(res.Value);
	IpList = StrToList(res.Value);
	return IpList;
}

int SetupNetworkAdapter::GetNetplanEndPos(string netplanPath, int startPos, int spacesCount)
{
	int tmpPos = startPos;
	int endPos = tmpPos;
	do
	{
		tmpPos = TxtHelper::GetStrPos(netplanPath, string(spacesCount + 2, ' '), tmpPos, tmpPos);
		if (tmpPos != -1)
		{
			endPos = tmpPos;
			tmpPos++;
		}
	} while (tmpPos != -1);
	return endPos;
}

int SetupNetworkAdapter::GetNetplanSpacesCount(string str)
{
	int spacesCount = 0;
	if (str != "")
	{
		for (int i = 0; i < str.length(); i++)
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

string SetupNetworkAdapter::GetSubnetMaskPrefix(string subnetMask)
{
	int startIdx = 0;
	string binary = "";
	for (int i = 0; i < 4; i++)
	{
		int num;
		int idx = subnetMask.find(".", startIdx);
		if (idx != string::npos)
		{
			string str = subnetMask.substr(startIdx, idx - startIdx);
			num = stoi(str);
			startIdx = idx + 1;
		}
		else
		{
			string str = subnetMask.substr(startIdx, subnetMask.length() - startIdx);
			num = stoi(str);
		}
		string bin = bitset<8>(num).to_string();
		binary += bin;
	}
	int prefix = 0;
	for (int i = 0; i < binary.length(); i++)
	{
		if (binary[i] == '1') prefix++;
	}
	return to_string(prefix);
}

string SetupNetworkAdapter::GetNetplanPath()
{
	ExecutionResult res = ShellHelper::RunCmd("ls " + netplanFolder + " | grep yaml");
	string defaultPath = netplanFolder + "50-cloud-init.yaml";
	if (res.ResultCode == 1) return defaultPath;
	string fileName = res.Value;
	if (fileName != "")
	{
		StrHelper::ReplaceAll(fileName, "\n", "");
		if (fileName.find(" ") != string::npos)
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

bool SetupNetworkAdapter::IsValidMACAddress(string mac)
{
	regex rex;
	rex = "([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})";
	return regex_match(mac, rex);
}

bool SetupNetworkAdapter::CheckParameter(ExecutionContext& context, string name)
{
	return (context.Parameters.count(name) > 0 && context.Parameters[name].length() > 0);
}

bool SetupNetworkAdapter::CheckParameter(ExecutionContext& context, string name, string value)
{
	return (context.Parameters.count(name) > 0 && context.Parameters[name] == value);
}