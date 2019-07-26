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

#include "ShellHelper.h"

ExecutionResult ShellHelper::RunCmd(string cmd)
{
	ExecutionResult ret;
	try
	{
		string data = "";
		const int max_buffer = 1024;
		char buffer[max_buffer];
		cmd.append(" 2>/dev/null");

		FILE* stream = popen(cmd.c_str(), "r");
		if (stream) {
			while (!feof(stream)) if (fgets(buffer, max_buffer, stream) != NULL) data.append(buffer);
			pclose(stream);
			ret.Value = data;
		}
		else 
		{
			ret.ResultCode = 1;
			ret.ErrorMessage = "RunCmd error: popen() failed!";
		}
		return ret;
	}
	catch (exception ex)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = "RunCmd error: " + (string)ex.what();
		return ret;
	}
}

ExecutionResult ShellHelper::ChangeUserPassword_Linux(string userName, string password)
{
	ExecutionResult ret;
	try
	{
		string cmd = "passwd " + userName;
		cmd.append(" > /dev/null 2>&1");
		FILE* stream = popen(cmd.c_str(), "w");
		if (stream) {
			password.append("\n");
			fwrite(password.c_str(), 1, password.size(), stream);
			fwrite(password.c_str(), 1, password.size(), stream);
			fflush(stream);
			pclose(stream);
		}
		else
		{
			ret.ResultCode = 1;
			ret.ErrorMessage = "ChangeUserPassword error: popen() failed!";
		}
		return ret;
	}
	catch (exception ex)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = "ChangeUserPassword error: " + (string)ex.what();
		return ret;
	}
}

ExecutionResult ShellHelper::ChangeUserPassword_FreeBSD(string userName, string password)
{
	ExecutionResult ret;
	try
	{
		string cmd = "echo \"" + password + "\" | pw usermod " + userName + " -h 0";
		FILE* stream = popen(cmd.c_str(), "r");
		if (stream) {
			pclose(stream);
		}
		else
		{
			ret.ResultCode = 1;
			ret.ErrorMessage = "ChangeUserPassword error: popen() failed!";
		}
		return ret;
	}
	catch (exception ex)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = "ChangeUserPassword error: " + (string)ex.what();
		return ret;
	}
}

ExecutionResult ShellHelper::ChangeUserPassword_PfSense(string password)
{
	ExecutionResult ret;
	try
	{
		string cmd = "pfSsh.php playback changepassword " + AppInfo::pfSenseAdmin;
		cmd.append(" > /dev/null 2>&1");
		FILE* stream = popen(cmd.c_str(), "w");
		if (stream) {
			password.append("\n");
			fwrite(password.c_str(), 1, password.size(), stream);
			fwrite(password.c_str(), 1, password.size(), stream);
			fflush(stream);
			pclose(stream);
		}
		else
		{
			ret.ResultCode = 1;
			ret.ErrorMessage = "ChangeUserPassword error: popen() failed!";
		}
		return ret;
	}
	catch (exception ex)
	{
		ret.ResultCode = 1;
		ret.ErrorMessage = "ChangeUserPassword error: " + (string)ex.what();
		return ret;
	}
}

ExecutionResult ShellHelper::ChangeUserPassword(string userName, string password)
{
	switch (OsVersion::GetOsVersion())
	{
	case FreeBSD:
		return ChangeUserPassword_FreeBSD(userName, password);
	case PfSense:
		return ChangeUserPassword_PfSense(password);
	default:
		return ChangeUserPassword_Linux(userName, password);
	}
}