﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Security.Policy;
using System.IO;
using System.CodeDom;
using System.Collections.Concurrent;

namespace SolidCP.WebPortal
{
	public class AssemblyLoader
	{
		public static void Init()
		{
			ProbingPaths = ConfigurationManager.AppSettings["ExternalProbingPaths"];
			if (!string.IsNullOrEmpty(ProbingPaths))
			{
				AppDomain.CurrentDomain.AssemblyResolve += Resolve;

				try
				{
					var eserver = Assembly.Load("SolidCP.EnterpriseServer");
					if (eserver != null)
					{
						var validatorType = eserver.GetType("SolidCP.EnterpriseServer.UsernamePasswordValidator");
						var init = validatorType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
						init.Invoke(null, new object[0]);
					}
				}
				catch { }

				try
				{
					var server = Assembly.Load("SolidCP.Server");
					if (server != null)
					{
						var validatorType = server.GetType("SolidCP.Server.PasswordValidator");
						var init = validatorType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
						init.Invoke(null, new object[0]);
					}
				}
				catch { }

				var exposeWebServices = string.Equals(ConfigurationManager.AppSettings["ExposeWebServices"], "true", StringComparison.OrdinalIgnoreCase);
				if (exposeWebServices) StartWebServices();
			}
		}

		static void StartWebServices()
		{
			var assembly = Assembly.Load("SolidCP.Web.Services");
			var StartupNetFX = assembly?.GetType("SolidCP.Web.Services.StartupNetFX");
			var method = StartupNetFX?.GetMethod("Start", BindingFlags.Public | BindingFlags.Static);
			method?.Invoke(null, new object[0]);


		}

		static readonly string exepath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
		static string[] paths = null;
		static string ProbingPaths = null;
		static string[] Paths => paths != null ? paths : paths =
			(string.IsNullOrEmpty(ProbingPaths) ? new string[0] :
				ProbingPaths.Replace('\\', Path.DirectorySeparatorChar)
				.Split(';'));

		static Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();
		static ConcurrentDictionary<string, object> Locks = new ConcurrentDictionary<string, object>();

		public static Assembly Resolve(object sender, ResolveEventArgs args)
		{
			var name = new AssemblyName(args.Name).Name;

			var lockobj = Locks.GetOrAdd(name, new object());

			lock (lockobj)
			{
				if (LoadedAssemblies.ContainsKey(name)) return LoadedAssemblies[name];

				var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
				if (loadedAssembly != null)
				{
					LoadedAssemblies.Add(name, loadedAssembly);
					return loadedAssembly;
				}

				var dlls = Paths
				.Select(p =>
				{
					var relativename = Path.Combine(p, $"{name}.dll");
					return new
					{
						FullName = new DirectoryInfo(Path.Combine(exepath, relativename)).FullName,
						Name = relativename
					};
				})
				.Where(p => File.Exists(p.FullName));

				foreach (var p in dlls)
				{
					var a = Assembly.LoadFrom(p.FullName);
					if (a != null)
					{
						LoadedAssemblies.Add(name, a);

						var msg = $"Loaded assembly {p.Name}";
						Console.WriteLine(msg);
						//if (Debugger.IsAttached) Debugger.Log(1, "info", $"{msg}\r\n");
					}
					return a;
				};

				return null;
			}
		}
	}
}