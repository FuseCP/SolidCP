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
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Reflection;

using System.Security.Principal;
using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;

namespace SolidCP.Installer.Common
{
	public static class UiUtils
	{
		public static void ShowRunningInstance()
		{
			Process currentProcess = Process.GetCurrentProcess();
			foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
			{
				if (process.Id != currentProcess.Id)
				{
					//set focus
					User32.SetForegroundWindow(process.MainWindowHandle);
					break;
				}
			}
		}
	}

	public class ResourceUtils
	{
		public static void CreateDefaultAppConfig()
		{
			var path = AppDomain.CurrentDomain.BaseDirectory;
			var assembly = Assembly.GetEntryAssembly();
			var file = assembly.Location + ".config";
			if (!File.Exists(file))
			{
				var resources = assembly.GetManifestResourceNames();
				var resource = resources.FirstOrDefault(r => r.EndsWith("App.config") || r.EndsWith("App.Release.config"));
				if (resource != null)
				{
					using (var src = assembly.GetManifestResourceStream(resource))
					using (var reader = new StreamReader(src))
					{
						File.WriteAllText(file, reader.ReadToEnd());
					}
				}
			}
		}
	}
}
