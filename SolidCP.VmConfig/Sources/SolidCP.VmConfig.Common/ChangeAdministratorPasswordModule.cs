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
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace SolidCP.VmConfig
{
	class ChangeAdministratorPasswordModule : IProvisioningModule
	{
		#region IProvisioningModule Members

		public ExecutionResult Run(ref ExecutionContext context)
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
                string userPath = string.Format("WinNT://{0}/{1}", System.Environment.MachineName, GetAdministratorName());
				DirectoryEntry userEntry = new DirectoryEntry(userPath);
				userEntry.Invoke("SetPassword", new object[] { password });
				userEntry.CommitChanges();
				userEntry.Close();
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

		#endregion


		private bool IsPasswordPolicyException(Exception ex)
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

        private string GetAdministratorName()
        {
            WmiUtils wmi = new WmiUtils("root\\cimv2");
            ManagementObjectCollection objUsers = wmi.ExecuteQuery("Select * From Win32_UserAccount Where LocalAccount = TRUE");

            foreach (ManagementObject objUser in objUsers)
            {
                string sid = (string)objUser["SID"];
                if (sid != null && sid.StartsWith("S-1-5-") && sid.EndsWith("-500"))
                    return (string)objUser["Name"];
            }

            return null;
        }
	}
}
