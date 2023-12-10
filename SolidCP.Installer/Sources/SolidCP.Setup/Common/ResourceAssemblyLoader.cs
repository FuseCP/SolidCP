using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SolidCP.Setup
{
	public class ResourceAssemblyLoader
	{

		public static byte[] BytesFromStream(Stream input)
		{
			using (var m = new MemoryStream((int)input.Length))
			{
				input.CopyTo(m);
				return m.ToArray();
			}
		}

		static Dictionary<string, Assembly> LoadedAssemblies = new Dictionary<string, Assembly>();

		public static Assembly Resolve(object sender, ResolveEventArgs args)
		{
			var host = Assembly.GetExecutingAssembly();
			var resources = host.GetManifestResourceNames();
			var name = new AssemblyName(args.Name).Name;

			lock (LoadedAssemblies)
			{
				if (LoadedAssemblies.ContainsKey(name)) return LoadedAssemblies[name];

				var assName = resources.FirstOrDefault(res => res.EndsWith($"{name}.dll", StringComparison.OrdinalIgnoreCase));
				string pdbName = null;
				if (assName != null)
				{
					using (var assStream = host.GetManifestResourceStream(assName))
					{
						if (Debugger.IsAttached)
						{
							pdbName = resources.FirstOrDefault(res => res.EndsWith($"{name}.pdb", StringComparison.OrdinalIgnoreCase));
							if (pdbName != null)
							{
								using (var pdbStream = host.GetManifestResourceStream(pdbName))
								{
									if (assStream != null && pdbStream != null)
									{
										var assembly = Assembly.Load(BytesFromStream(assStream), BytesFromStream(pdbStream));
										if (assembly != null) LoadedAssemblies.Add(name, assembly);
										return assembly;
									}
								}
							}
							else
							{
								var assembly = Assembly.Load(BytesFromStream(assStream));
								if (assembly != null) LoadedAssemblies.Add(name, assembly);
								return assembly;
							}
						}
						else
						{
							var assembly = Assembly.Load(BytesFromStream(assStream));
							if (assembly != null) LoadedAssemblies.Add(name, assembly);
							return assembly;
						}
					}
				}
				return null;
			}
		}

		public static void Init()
		{
			AppDomain.CurrentDomain.AssemblyResolve += Resolve;
		}
	}
}
