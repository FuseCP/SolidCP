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

#include "OsVersion.h"
#include "ShellHelper.h"
#include <algorithm>

OsVersionEnum OsVersion::osVersion = NA;

OsVersionEnum OsVersion::GetOsVersion()
{
	if (osVersion != NA) return osVersion;

	ifstream f("/bin/freebsd-version");
	bool exists = f.good();
	f.close();
	if (exists)
	{
		osVersion = FreeBSD;
	}
	else
	{
		osVersion = Linux_NA;
	}
	ExecutionResult res;
	switch (osVersion)
	{
	case Linux_NA:
		res = ShellHelper::RunCmd("cat /etc/os-release | grep \"ID=\"");
		if (res.ResultCode != 1) osVersion = FindOsVersion(res.Value);
		break;
	case FreeBSD:
		const string configXml = "/cf/conf/config.xml";
		ifstream f(configXml);
		bool exists = f.good();
		f.close();
		if (exists)
		{
			if (TxtHelper::GetStrPos(configXml, "<pfsense>", 0, -1) != -1) osVersion = PfSense;
		}
		break;
	}
	return osVersion;
}

bool OsVersion::IsFreeBsdOs() 
{
	if (osVersion == NA) GetOsVersion();
	if (osVersion == FreeBSD || osVersion == PfSense) 
	{
		return true;
	}
	else
	{
		return false;
	}
}

OsVersionEnum OsVersion::FindOsVersion(string result)
{
	transform(result.begin(), result.end(), result.begin(), ::tolower);
	if (result.find("freebsd") != string::npos)
	{
		return FreeBSD;
	}
	else if (result.find("ubuntu") != string::npos)
	{
		return Ubuntu;
	}
	else if (result.find("centos") != string::npos)
	{
		return CentOS;
	}
	else if (result.find("linux") != string::npos)
	{
		return Linux_NA;
	}
	return osVersion;
}