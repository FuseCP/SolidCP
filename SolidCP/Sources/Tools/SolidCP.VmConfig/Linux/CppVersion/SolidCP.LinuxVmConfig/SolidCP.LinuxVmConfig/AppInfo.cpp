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

#include "AppInfo.h"

string AppInfo::appExeName = "SolidCP.LinuxVmConfig";
string AppInfo::appVersion = VERSION;
string AppInfo::appPath = "/SolidCP";
string AppInfo::userName = "root";
string AppInfo::pfSenseAdmin = "admin";

void AppInfo::InitAppInfo(int argcount, char* args[])
{
	if (argcount > 0) {
		string exe(args[0]);
		size_t pos = exe.find_last_of('/');
		if (pos != string::npos) {
			exe = exe.substr(pos + 1);
		}
		appExeName = exe;
	}
	InitAppPath();
	InitUserName();
}

void AppInfo::InitAppPath()
{
	if (OsVersion::IsFreeBsdOs())
	{
#ifdef __FreeBSD__
		char exePath[2048];
		int mib[4];
		mib[0] = CTL_KERN;
		mib[1] = KERN_PROC;
		mib[2] = KERN_PROC_PATHNAME;
		mib[3] = -1;
		size_t len = sizeof(exePath);
		if (sysctl(mib, 4, exePath, &len, NULL, 0) != 0) exePath[0] = '\0';
		string path(exePath);
		size_t pos = path.find_last_of('/');
		if (pos != string::npos) path = path.substr(0, pos);
		appPath = path;
#endif
}
	else
	{
		char buff[4096];
		ssize_t len = readlink("/proc/self/exe", buff, sizeof(buff) - 1);
		if (len != -1) {
			buff[len] = '\0';
			string path(buff);
			size_t pos = path.find_last_of('/');
			if (pos != string::npos) path = path.substr(0, pos);
			appPath = path;
		}
	}
}

void AppInfo::InitUserName()
{
	struct passwd* pass;
	pass = getpwuid(getuid());
	char* name = pass->pw_name;
	string sname(name);
	if (sname.length() > 0) userName = sname;
}