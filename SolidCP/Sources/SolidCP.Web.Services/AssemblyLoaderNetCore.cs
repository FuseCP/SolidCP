#if !NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Loader;
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
		static readonly string[] paths = StartupCore.ProbingPaths.Split(';');

		public static Assembly Resolve(AssemblyLoadContext context, AssemblyName name)
		{
			return paths
				.Select(p => {
					var file = Path.Combine(new DirectoryInfo(Path.Combine(exepath, p)).FullName, $"{name.Name}.dll");
					return new {
						File = file,
						CodeBase = new Uri(file).AbsoluteUri
					};
				})
				.Where(p => File.Exists(p.File))
				.Select(p =>
				{
					var namewithfile = new AssemblyName(name.FullName);
					namewithfile.CodeBase = p.CodeBase;
					return context.LoadFromAssemblyName(namewithfile);
				})
				.FirstOrDefault();
		}
	}
}
#endif