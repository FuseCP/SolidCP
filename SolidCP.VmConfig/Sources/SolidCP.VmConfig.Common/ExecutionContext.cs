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
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace SolidCP.VmConfig
{
	[Serializable]
	public sealed class ExecutionContext
	{
		public string ActivityID { get; set; }
		public string ActivityName { get; set; }
		public string ActivityDefinition { get; set; }

		private string activityDescription;
		public string ActivityDescription
		{
			get
			{
				return activityDescription;
			}
			set
			{
				activityDescription = value;
				SaveState();
			}
		}
		
		private int progress = 0;
		public int Progress
		{
			get
			{
				return progress;
			}
			set
			{
				progress = value;
				if (progress < 0)
					progress = 0;
				if (progress > 100)
					progress = 100;
				SaveState();
			}
		}

		private Dictionary<string, string> parameters = new Dictionary<string, string>();
		public Dictionary<string, string> Parameters
		{
			get { return parameters; }
		}

		private void SaveState()
		{
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.CreateSubKey("SOFTWARE\\Microsoft\\Virtual Machine\\Guest");
			if (rk != null)
			{
				StringBuilder builder = new StringBuilder();
				builder.AppendFormat("ActivityDefinition={0}|", ActivityDefinition);
				builder.AppendFormat("ActivityDescription={0}|", ActivityDescription);
				builder.AppendFormat("Progress={0}", Progress);
				rk.SetValue("SCP-CurrentTask", builder.ToString());
			}
		}
	}
}
