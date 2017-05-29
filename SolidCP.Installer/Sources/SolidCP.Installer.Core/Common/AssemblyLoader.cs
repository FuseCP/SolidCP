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
using System.Security.Policy;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Lifetime;

namespace SolidCP.Installer.Common
{
	[Serializable]
	public class AssemblyLoader : MarshalByRefObject
	{
		public object RemoteRun(string fileName, string typeName, string methodName, object[] parameters)
		{
			Assembly assembly = Assembly.LoadFrom(fileName);
			Type type = assembly.GetType(typeName);
			MethodInfo method = type.GetMethod(methodName);
			return method.Invoke(Activator.CreateInstance(type), parameters);
		}

		public void AddTraceListener(TraceListener traceListener)
		{
			Trace.Listeners.Add(traceListener);
		}

		public static object Execute(string fileName, string typeName, string methodName, object[] parameters)
		{
			AppDomain domain = null;
			try
			{
				Evidence securityInfo = AppDomain.CurrentDomain.Evidence;
				AppDomainSetup info = new AppDomainSetup();
				info.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

				domain = AppDomain.CreateDomain("Remote Domain", securityInfo, info);
				domain.InitializeLifetimeService();
				domain.UnhandledException += new UnhandledExceptionEventHandler(OnDomainUnhandledException);
				AssemblyLoader loader = (AssemblyLoader)domain.CreateInstanceAndUnwrap(
					typeof(AssemblyLoader).Assembly.FullName,
					typeof(AssemblyLoader).FullName);

				foreach (TraceListener listener in Trace.Listeners)
				{
					loader.AddTraceListener(listener);
				}

				object ret = loader.RemoteRun(fileName, typeName, methodName, parameters);
				AppDomain.Unload(domain);
				return ret;
			}
			catch (Exception)
			{
				if (domain != null)
				{
					AppDomain.Unload(domain);
				}
				throw;
			}
		}

		static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Log.WriteError("Remote domain error", (Exception)e.ExceptionObject);
		}

		public static string GetShellVersion()
		{
			return Assembly.GetEntryAssembly().GetName().Version.ToString();
		}
	}
}
