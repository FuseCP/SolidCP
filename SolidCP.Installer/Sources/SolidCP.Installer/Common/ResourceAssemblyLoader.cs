using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SolidCP.Installer
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

				var host = Assembly.GetExecutingAssembly();
				var path = AppDomain.CurrentDomain.BaseDirectory;
				var resources = host.GetManifestResourceNames();
				var fileName = Path.Combine(path, $"{name}.dll");
				var copyToFile = name == "SolidCP.Installer.Core"; // save installer core to a file, so the domain can find it
				var assName = resources.FirstOrDefault(res => res.EndsWith($"{name}.dll", StringComparison.OrdinalIgnoreCase));
				string pdbName = null;
				if (assName != null)
				{
					if (copyToFile == true)
					{ // load from file
						using (var assStream = host.GetManifestResourceStream(assName))
						using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
						{
							assStream.CopyTo(file);
						}
						if (Debugger.IsAttached)
						{
							pdbName = resources.FirstOrDefault(res => res.EndsWith($"{name}.pdb", StringComparison.OrdinalIgnoreCase));
							if (pdbName != null)
							{
								using (var pdbStream = host.GetManifestResourceStream(pdbName))
								using (var file = new FileStream(Path.Combine(path, $"{name}.pdb"), FileMode.Create, FileAccess.Write))
								{
									pdbStream.CopyTo(file);
								}
							}
						}
						var assembly = Assembly.LoadFrom(fileName);
						if (assembly != null) LoadedAssemblies.Add(name, assembly);
						return assembly;
					}
					else // load from resource stream
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
