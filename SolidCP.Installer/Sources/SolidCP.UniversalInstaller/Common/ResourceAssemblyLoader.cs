using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace SolidCP.UniversalInstaller
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
		public static Assembly Resolve(object sender, ResolveEventArgs args)
		{
			var host = Assembly.GetExecutingAssembly();
			var resources = host.GetManifestResourceNames();
			var assName = resources.FirstOrDefault(res => res.EndsWith($"{args.Name}.dll"));
			string? pdbName = null;
			if (assName != null)
			{
				using (var assStream = host.GetManifestResourceStream(assName))
				{
					if (Debugger.IsAttached)
					{
						pdbName = resources.FirstOrDefault(res => res.EndsWith($"{args.Name}.pdb"));
						if (pdbName != null)
						{
							using (var pdbStream = host.GetManifestResourceStream(pdbName))
							{
								return Assembly.Load(BytesFromStream(assStream), BytesFromStream(pdbStream));
							}
						}
					}
					else
					{
						return Assembly.Load(BytesFromStream(assStream));
					}
				}
			}
			return null;
		}

		public static void Init()
		{
			AppDomain.CurrentDomain.AssemblyResolve += Resolve;
		}
	}
}
