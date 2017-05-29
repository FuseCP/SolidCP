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
using System.Runtime.InteropServices;

using System.Collections.Generic;
using System.Text;


namespace SolidCP.VmConfig
{
	class ChangeComputerNameModule : IProvisioningModule
	{
		//P/Invoke signature
		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetComputerNameEx(COMPUTER_NAME_FORMAT nameType, [MarshalAs(UnmanagedType.LPTStr)] string lpBuffer);

		private enum COMPUTER_NAME_FORMAT : int
		{
			ComputerNameNetBIOS,
			ComputerNameDnsHostname,
			ComputerNameDnsDomain,
			ComputerNameDnsFullyQualified,
			ComputerNamePhysicalNetBIOS,
			ComputerNamePhysicalDnsHostname,
			ComputerNamePhysicalDnsDomain,
			ComputerNamePhysicalDnsFullyQualified,
			ComputerNameMax
		}

		#region IProvisioningModule Members

		public ExecutionResult Run(ref ExecutionContext context)
		{
			ExecutionResult ret = new ExecutionResult();
			ret.ResultCode = 0;
			ret.ErrorMessage = null;
			ret.RebootRequired = true;

			context.ActivityDescription = "Changing computer name...";
			context.Progress = 0;
            if (!context.Parameters.ContainsKey("FullComputerName"))
			{
				ret.ResultCode = 2;
                ret.ErrorMessage = "Parameter 'FullComputerName' not found";
				Log.WriteError(ret.ErrorMessage);
				context.Progress = 100;
				return ret;
			}
			// Call SetComputerEx
            string computerName = context.Parameters["FullComputerName"];
            string netBiosName = computerName;
            string primaryDnsSuffix = "";
            int idx = netBiosName.IndexOf(".");
            if (idx != -1)
            {
                netBiosName = computerName.Substring(0, idx);
                primaryDnsSuffix = computerName.Substring(idx + 1);
            }

			try
			{
                // set NetBIOS name
				bool res = SetComputerNameEx(COMPUTER_NAME_FORMAT.ComputerNamePhysicalDnsHostname, netBiosName);
				if (!res)
				{
					ret.ResultCode = 1;
					ret.ErrorMessage = "Unexpected error";
					Log.WriteError(ret.ErrorMessage);
				}

                // set primary DNS suffix
                res = SetComputerNameEx(COMPUTER_NAME_FORMAT.ComputerNamePhysicalDnsDomain, primaryDnsSuffix);
                if (!res)
                {
                    ret.ResultCode = 1;
                    ret.ErrorMessage = "Unexpected error";
                    Log.WriteError(ret.ErrorMessage);
                }
			}
			catch (Exception ex)
			{
				ret.ResultCode = 1;
				ret.ErrorMessage = ex.ToString();
				Log.WriteError(ret.ErrorMessage);
			}
			if (ret.ResultCode == 0)
			{
				Log.WriteInfo("Computer name has been changed successfully");
			}
			context.Progress = 100;
			return ret;
		}
		#endregion
		
	}
}
