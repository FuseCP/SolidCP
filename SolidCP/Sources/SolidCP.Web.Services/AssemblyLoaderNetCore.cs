#if !NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Loader;
using System.Diagnostics;
using System.Reflection;
using System.Security.Policy;
using System.IO;
using System.CodeDom;

namespace SolidCP.Web.Services
{
	public class AssemblyLoaderNetCore
	{
		public static void Init()
		{
			AssemblyLoadContext.Default.Resolving += Resolve;
		}

		static readonly string exepath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		static string[] paths = null;
		static string[] Paths => paths != null ? paths : paths =
			Configuration.ProbingPaths
				.Replace('\\', Path.DirectorySeparatorChar)
				.Split(';')
				.Concat(new string[] { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) })
				.ToArray();

		public static Assembly Resolve(AssemblyLoadContext context, AssemblyName name)
		{
			return Paths
				.Select(p =>
				{
					var relativename = Path.Combine(p, $"{name.Name}.dll");
					return new
					{
						FullName = new DirectoryInfo(Path.Combine(exepath, relativename)).FullName,
						Name = relativename
					};
				})
				.Where(p => File.Exists(p.FullName))
				.Select(p =>
				{
					var a = context.LoadFromAssemblyPath(p.FullName);
					if (a != null)
					{
						var msg = $"Loaded assembly {p.Name}";
						Console.WriteLine(msg);
						//if (Debugger.IsAttached) Debugger.Log(1, "info", $"{msg}\r\n");
					}
					return a;
				})
				.FirstOrDefault();
		}
	}
}
#endif