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
using System.Security.Policy;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
#if NETFRAMEWORK
using System.Runtime.Remoting.Lifetime;
#else
using System.Runtime.Loader;
#endif

namespace SolidCP.VmConfig
{
	[Serializable]
	internal class ModuleLoader : MarshalByRefObject
	{

#if NETCOREAPP
		internal AssemblyLoadContext LoadContext { get; private set; }
#endif
		internal ExecutionResult RemoteRun(string typeName, ref ExecutionContext context)
		{
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");

			string[] parts = typeName.Split(',');
			if (parts == null || parts.Length != 2)
				throw new ArgumentException("Incorrect type name " + typeName);
 
			Assembly assembly = typeof(ModuleLoader).Assembly;
			string fileName = parts[1].Trim();
			if (!fileName.EndsWith(".dll"))
				fileName = fileName + ".dll"; 

			string path = Path.Combine(Path.GetDirectoryName(assembly.Location), fileName);
#if NETFRAMEWORK
			assembly = Assembly.LoadFrom(path);
#else
			assembly = LoadContext.LoadFromAssemblyPath(path);
#endif
			Type type = assembly.GetType(parts[0].Trim());
			//Type type = Type.GetType(typeName);
			if (type == null)
			{
				throw new Exception(string.Format("Type {0} not found", typeName));
			}

			IProvisioningModule module = Activator.CreateInstance(type) as IProvisioningModule;
			if (module == null)
			{
				throw new Exception(string.Format("Module {0} not found", typeName));
			}

			return module.Run(ref context);
		}

		internal void AddTraceListener(TraceListener traceListener)
		{
			Trace.Listeners.Add(traceListener);
		}

		internal static ExecutionResult Run(string typeName, ref ExecutionContext context)
		{
			AppDomain domain = null;
			ModuleLoader loader = null;
			try
			{
#if NETFRAMEWORK
				Evidence securityInfo = AppDomain.CurrentDomain.Evidence;
				AppDomainSetup info = new AppDomainSetup();
				info.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

				domain = AppDomain.CreateDomain("Remote Domain", securityInfo, info);
				ILease lease = domain.GetLifetimeService() as ILease;
				if (lease != null)
				{
					lease.InitialLeaseTime = TimeSpan.Zero;
				}
				domain.UnhandledException += new UnhandledExceptionEventHandler(OnDomainUnhandledException);
				loader = (ModuleLoader)domain.CreateInstanceAndUnwrap(
					typeof(ModuleLoader).Assembly.FullName,
					typeof(ModuleLoader).FullName);
#else
				loader = new ModuleLoader();
				loader.LoadContext = new AssemblyLoadContext("ModuleLoader", true);
#endif
				foreach (TraceListener listener in Trace.Listeners)
				{
					loader.AddTraceListener(listener);
				}

				ExecutionResult ret = loader.RemoteRun(typeName, ref context);
#if NETFRAMEWORK
				AppDomain.Unload(domain);
#else
				loader.LoadContext.Unload();
				loader.LoadContext = null;
#endif
				return ret;
			}
			catch (Exception)
			{
#if NETFRAMEWORK
				if (domain != null)
				{
					AppDomain.Unload(domain);
				}
#else
				loader.LoadContext.Unload();
				loader.LoadContext = null;
#endif
				throw;
			}
		}

		static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			ServiceLog.WriteError("Remote application domain error", (Exception)e.ExceptionObject);
		}
	}
}
