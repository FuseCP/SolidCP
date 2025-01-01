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
using System.IO;
using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller
{
	[Serializable]
	public class AssemblyLoader : MarshalByRefObject
	{

		const bool UseLocalSetupDllForDebugging = true;
		public bool UseLocalSetupDll = false;

		public object RemoteRun(string fileName, string typeName, string methodName, object[] parameters)
		{
			Assembly assembly = null;
#if DEBUG
			if (UseLocalSetupDllForDebugging && fileName.EndsWith("Setup.dll", StringComparison.OrdinalIgnoreCase) &&
				(Debugger.IsAttached || UseLocalSetupDll))
			{
				var exe = Assembly.GetEntryAssembly();
				var path = Path.Combine(Path.GetDirectoryName(exe.Location), "Setup.dll");
				assembly = Assembly.LoadFrom(path);
			}
			else if (UseLocalSetupDllForDebugging && fileName.EndsWith("Setup2.dll", StringComparison.OrdinalIgnoreCase) &&
				(Debugger.IsAttached || UseLocalSetupDll))
			{
				var exe = Assembly.GetEntryAssembly();
				var path = Path.Combine(Path.GetDirectoryName(exe.Location), "Setup2.dll");
				assembly = Assembly.LoadFrom(path);
			}
			else assembly = Assembly.LoadFrom(fileName);
#else
			assembly = Assembly.LoadFrom(fileName);
#endif
			Type type = assembly.GetType(typeName);
			MethodInfo method = type.GetMethod(methodName, new Type[] { typeof(string) });
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
				if (OSInfo.IsNetFX)
				{
					/* Evidence securityInfo = AppDomain.CurrentDomain.Evidence;
					AppDomainSetup info = new AppDomainSetup();
					info.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

					domain = AppDomain.CreateDomain("Remote Domain", securityInfo, info); */
					object securityInfo;
					var evidenceProperty = typeof(AppDomain).GetProperty("Evidence");
					securityInfo = evidenceProperty.GetValue(AppDomain.CurrentDomain);
					var domainSetupType = Type.GetType("System.AppDomainSetup, mscorlib");
					object info = Activator.CreateInstance(domainSetupType);
					var appBaseProperty = domainSetupType.GetProperty("ApplicationBase");
					appBaseProperty.SetValue(info, AppDomain.CurrentDomain.BaseDirectory);

					var createDomainMethod = typeof(AppDomain).GetMethod("CreateDomain", new Type[] { typeof(string), Type.GetType("System.Security.Policy.Evidence, mscorlib"), domainSetupType });
					domain = createDomainMethod.Invoke(null, new object[] { "Remote Domain", securityInfo, info }) as AppDomain;

					domain.InitializeLifetimeService();
					//domain.UnhandledException += new UnhandledExceptionEventHandler(OnDomainUnhandledException);

					AssemblyLoader loader;

					if (!Debugger.IsAttached)
					{
						/* loader = (AssemblyLoader)domain.CreateInstanceAndUnwrap(
							typeof(AssemblyLoader).Assembly.FullName,
							typeof(AssemblyLoader).FullName); */
						var createInstanceAndUnwrapMethod = typeof(AppDomain).GetMethod("CreateInstanceAndUnwrap", new Type[] { typeof(string), typeof(string) });
						loader = (AssemblyLoader)createInstanceAndUnwrapMethod.Invoke(domain, new object[] {
							typeof(AssemblyLoader).Assembly.FullName,
							typeof(AssemblyLoader).FullName
						});

						foreach (TraceListener listener in Trace.Listeners)
						{
							loader.AddTraceListener(listener);
						}
					}
					else  // don't call in separate AppDomain when debugging
					{
						loader = new AssemblyLoader();
					}
					object ret = (bool)loader.RemoteRun(fileName, typeName, methodName, parameters);
					AppDomain.Unload(domain);
					return ret;
				}
				else
				{
					throw new NotImplementedException();
				}
			}
			catch (Exception ex)
			{
				if (domain != null) AppDomain.Unload(domain);

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