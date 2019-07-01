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

#pragma once
#include "ExecutionResult.h"
#include "ExecutionContext.h"
#include "ShellHelper.h"
#include "Log.h"
#include <regex>
#include <bitset>
#include <random>

using namespace std;

class SetupNetworkAdapter
{
public:
	static ExecutionResult Run(ExecutionContext& context);
private:
	static const string netplanFolder;
	static const string interfacesPath;
	static const string hostsPath;
	static const string ifcfgBasePath;
	static const string rcConfPath;
	static const string resolvConfPath;
	static const string configXmlPath;
	struct Adapter;
	static vector<string> StrToList(string str);
	static vector<Adapter> GetAdapters();
	static string GetAdapterMacAddress(string adapterName);
	static vector<string> ParseArray(string array);
	static void ProcessError(ExecutionContext& context, ExecutionResult& ret, exception& ex, int errorCode, string errorPrefix);
	static void ProcessError(ExecutionContext& context, ExecutionResult& ret, int errorCode, string errorPrefix);
	static void EnableDHCP_Ubuntu(string adapter, ExecutionResult& ret);
	static void EnableDHCP_CentOS(string adapter, ExecutionResult& ret);
	static void EnableDHCP_FreeBSD(string adapter, ExecutionResult& ret);
	static void EnableDHCP_PfSense(string adapter, ExecutionResult& ret);
	static void EnableDHCP(string adapter, ExecutionResult& ret);
	static vector<int> GetPfSenseAdapterPos(string adapter);
	static vector<int> GetPfSenseGatewayPos(string interfaceTyp);
	static vector<int> GetPfSenseVipPos(string interfaceTyp);
	static string GetUniqId(int length);
	static void DisableDHCP_Ubuntu(ExecutionContext& context, string adapter, ExecutionResult& ret, vector<string> ipAddresses, vector<string> subnetMasksPrefix, vector<string> subnetMasks, string ipGateway);
	static void DisableDHCP_CentOS(ExecutionContext& context, string adapter, ExecutionResult& ret, vector<string> ipAddresses, vector<string> subnetMasksPrefix, string ipGateway);
	static void DisableDHCP_FreeBSD(ExecutionContext& context, string adapter, ExecutionResult& ret, vector<string> ipAddresses, vector<string> subnetMasks, string ipGateway);
	static void DisableDHCP_PfSense(ExecutionContext& context, string adapter, ExecutionResult& ret, vector<string> ipAddresses, vector<string> subnetMasksPrefix, string ipGateway);
	static void DisableDHCP(ExecutionContext& context, string adapter, ExecutionResult& ret);
	static vector<string> GetAdapterIp(string adapterName);
	static int GetNetplanEndPos(string netplanPath, int startPos, int spacesCount);
	static int GetNetplanSpacesCount(string str);
	static string GetSubnetMaskPrefix(string subnetMask);
	static string GetNetplanPath();
	static bool IsValidMACAddress(string mac);
	static bool CheckParameter(ExecutionContext& context, string name);
	static bool CheckParameter(ExecutionContext& context, string name, string value);
};

