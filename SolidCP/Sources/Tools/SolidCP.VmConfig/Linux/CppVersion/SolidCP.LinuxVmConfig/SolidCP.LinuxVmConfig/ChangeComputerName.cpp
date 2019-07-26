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

#include "ChangeComputerName.h"

const string ChangeComputerName::cloudCfgPath = "/etc/cloud/cloud.cfg";
const string ChangeComputerName::hostsPath = "/etc/hosts";
const string ChangeComputerName::hostnamePath = "/etc/hostname";
const string ChangeComputerName::rcConfPath = "/etc/rc.conf";
const string ChangeComputerName::configXmlPath = "/cf/conf/config.xml";

ExecutionResult ChangeComputerName::Run(ExecutionContext& context)
{
	ExecutionResult ret;
	ret.ResultCode = 0;
	ret.ErrorMessage = "";
	ret.RebootRequired = true;
	try
	{
		context.ActivityDescription = "Changing computer name...";
		context.Progress = 0;
		if (context.Parameters.count("FullComputerName") == 0)
		{
			ret.ResultCode = 2;
			ret.ErrorMessage = "Parameter 'FullComputerName' not found";
			Log::WriteError(ret.ErrorMessage);
			context.Progress = 100;
			return ret;
		}
		string fullName = context.Parameters["FullComputerName"];
		transform(fullName.begin(), fullName.end(), fullName.begin(), ::tolower);
		string hostName = fullName;
		size_t idx = hostName.find_first_of('.');
		if (idx != string::npos)
		{
			hostName = fullName.substr(0, idx);
		}
		ExecutionResult res;
		res = ShellHelper::RunCmd("hostname -s");
		string hostNameOld = "";
		if (res.Value.length() > 0) hostNameOld = res.Value;
		StrHelper::Trim(hostNameOld);

		res = ShellHelper::RunCmd("hostname");
		string fullNameOld = "";
		if (res.Value.length() > 0) fullNameOld = res.Value;
		StrHelper::Trim(fullNameOld);

		string domain = fullName;
		StrHelper::Replace(domain, hostName + ".", "");

		string domainOld = "";
		if (hostNameOld.length() > 0 && fullNameOld.length() > 0)
		{
			domainOld = fullNameOld;
			StrHelper::Replace(domainOld, hostNameOld + ".", "");
		}

		vector<string> config;
		int startPos;
		int endPos;

		switch (OsVersion::GetOsVersion())
		{
		case Ubuntu:
			TxtHelper::ReplaceStr(hostnamePath, fullName, 0);
			if (domainOld.length() > 0) TxtHelper::ReplaceStr(hostsPath, domainOld, domain);
			if (hostNameOld.length() > 0) TxtHelper::ReplaceStr(hostsPath, hostNameOld, hostName);
			TxtHelper::ReplaceStr(cloudCfgPath, "preserve_hostname: false", "preserve_hostname: true");
			TxtHelper::ReplaceStr(cloudCfgPath, "# preserve_hostname: true", "preserve_hostname: true");
			TxtHelper::ReplaceStr(cloudCfgPath, "#preserve_hostname: true", "preserve_hostname: true");
			break;
		case CentOS:
			TxtHelper::ReplaceStr(hostnamePath, fullName, 0);
			if (domainOld.length() > 0) TxtHelper::ReplaceStr(hostsPath, domainOld, domain);
			if (hostNameOld.length() > 0) TxtHelper::ReplaceStr(hostsPath, hostNameOld, hostName);
			break;
		case FreeBSD:
			TxtHelper::ReplaceStr(rcConfPath, "hostname=\"" + fullName + "\"", TxtHelper::GetStrPos(rcConfPath, "hostname", 0, -1));
			if (domainOld.length() > 0) TxtHelper::ReplaceStr(hostsPath, domainOld, domain);
			if (hostNameOld.length() > 0) TxtHelper::ReplaceStr(hostsPath, hostNameOld, hostName);
			break;
		case PfSense:
			startPos = TxtHelper::GetStrPos(configXmlPath, "<hostname>", 0, -1);
			endPos = TxtHelper::GetStrPos(configXmlPath, "</hostname>", startPos, -1);
			config.push_back("\t\t<hostname>" + hostName + "</hostname>");
			TxtHelper::ReplaceLines(configXmlPath, config, startPos, endPos);
			startPos = TxtHelper::GetStrPos(configXmlPath, "<domain>", 0, -1);
			endPos = TxtHelper::GetStrPos(configXmlPath, "</domain>", startPos, -1);
			config.clear();
			config.push_back("\t\t<domain>" + domain + "</domain>");
			TxtHelper::ReplaceLines(configXmlPath, config, startPos, endPos);
			break;
		default:
			TxtHelper::ReplaceStr(hostnamePath, fullName, 0);
			if (domainOld.length() > 0) TxtHelper::ReplaceStr(hostsPath, domainOld, domain);
			if (hostNameOld.length() > 0) TxtHelper::ReplaceStr(hostsPath, hostNameOld, hostName);
			break;
		}
	}
	catch (exception ex)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = "ChangeComputerName error: " + (string)ex.what();
		Log::WriteError(ret.ErrorMessage);
	}
	if (ret.ResultCode == 0)
	{
		Log::WriteInfo("Computer name has been changed successfully");
	}
	context.Progress = 100;
	return ret;
}