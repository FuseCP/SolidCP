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

#include "ChangeAdministratorPassword.h"

ExecutionResult ChangeAdministratorPassword::Run(ExecutionContext& context)
{
	ExecutionResult ret;
	ret.ResultCode = 0;
	ret.ErrorMessage = "";
	ret.RebootRequired = false;

	context.ActivityDescription = "Changing password for built-in local Administrator account...";
	context.Progress = 0;
	if (context.Parameters.count("Password") == 0)
	{
		ret.ResultCode = 2;
		ret.ErrorMessage = "Parameter 'Password' not found";
		Log::WriteError(ret.ErrorMessage);
		context.Progress = 100;
		return ret;
	}
	string password = context.Parameters["Password"];
	if (password.length() == 0)
	{
		ret.ResultCode = 2;
		ret.ErrorMessage = "Password is null or empty";
		Log::WriteError(ret.ErrorMessage);
		context.Progress = 100;
		return ret;
	}
	try
	{
		string userName = AppInfo::userName;
		ExecutionResult res;
		res = ShellHelper::ChangeUserPassword(userName, password);
		if (res.ResultCode == 1)
		{
			ret.ResultCode = 1;
			ret.ErrorMessage = res.ErrorMessage;
			context.Progress = 100;
			return ret;
		}
	}
	catch (exception ex)
	{
		ret.ResultCode = 2;
		ret.ErrorMessage = ex.what();
		Log::WriteError(ret.ErrorMessage);
	}
	if (ret.ResultCode == 0)
	{
		Log::WriteInfo("Password has been changed successfully");
	}
	context.Progress = 100;
	return ret;
}