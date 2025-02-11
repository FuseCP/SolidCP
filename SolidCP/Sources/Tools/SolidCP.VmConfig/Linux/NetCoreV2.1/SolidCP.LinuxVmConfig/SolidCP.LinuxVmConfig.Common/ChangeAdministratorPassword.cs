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

namespace SolidCP.LinuxVmConfig
{
    public class ChangeAdministratorPassword
    {
        public static ExecutionResult Run(ref ExecutionContext context)
        {
            ExecutionResult ret = new ExecutionResult();
            ret.ResultCode = 0;
            ret.ErrorMessage = null;
            ret.RebootRequired = false;

            context.ActivityDescription = "Changing password for built-in local Administrator account...";
            context.Progress = 0;
            if (!context.Parameters.ContainsKey("Password"))
            {
                ret.ResultCode = 2;
                ret.ErrorMessage = "Parameter 'Password' not found";
                Log.WriteError(ret.ErrorMessage);
                context.Progress = 100;
                return ret;
            }
            string password = context.Parameters["Password"];
            if (string.IsNullOrEmpty(password))
            {
                ret.ResultCode = 2;
                ret.ErrorMessage = "Password is null or empty";
                Log.WriteError(ret.ErrorMessage);
                context.Progress = 100;
                return ret;
            }
            try
            {
                string userName = Environment.UserName;
                ExecutionResult res;
                res = ShellHelper.ChangeUserPassword(userName, password);
                if (res.ResultCode == 1)
                {
                    ret.ResultCode = 1;
                    ret.ErrorMessage = res.ErrorMessage;
                    context.Progress = 100;
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (IsPasswordPolicyException(ex))
                {
                    ret.ResultCode = 1;
                    ret.ErrorMessage = "The password does not meet the password policy requirements. Check the minimum password length, password complexity and password history requirements.";
                }
                else
                {
                    ret.ResultCode = 2;
                    ret.ErrorMessage = ex.ToString();
                }
                Log.WriteError(ret.ErrorMessage);
            }
            if (ret.ResultCode == 0)
            {
                Log.WriteInfo("Password has been changed successfully");
            }
            context.Progress = 100;
            return ret;
        }


        private static bool IsPasswordPolicyException(Exception ex)
        {
            //0x800708C5
            if (ex is System.Runtime.InteropServices.COMException &&
                ((System.Runtime.InteropServices.COMException)ex).ErrorCode == -2147022651)
            {
                return true;
            }
            if (ex.InnerException != null)
                return IsPasswordPolicyException(ex.InnerException);
            else
                return false;
        }
    }
}
