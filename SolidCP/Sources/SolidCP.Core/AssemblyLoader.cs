using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel.Design.Serialization;
using System.Threading;
using SolidCP.Core;

namespace SolidCP.Core
{

	public class AssemblyLoader
	{
		public static void Init()
		{
			System.AppDomain.CurrentDomain.AssemblyResolve += LoadAssembly;
		}
		public static string ToFileUrl(string path)
		{
			return new UriBuilder("file", string.Empty)
			{
				Path = path
							.Replace(" ", $"%{(int)' ':X2}")
							.Replace("%", $"%{(int)'%':X2}")
							.Replace("[", $"%{(int)'[':X2}")
							.Replace("]", $"%{(int)']':X2}"),
			}
				 .Uri
				 .AbsoluteUri;
		}

		[ThreadStatic]
		static bool Resolving = false;

		public static Assembly LoadAssembly(object sender, ResolveEventArgs args)
		{
			if (!Resolving)
			{
				var name = new AssemblyName(args.Name);
				var basepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath ?? "");
				string path;

				if (Net.IsCore)
				{
					path = Path.Combine(basepath, "net7.0", $"{args.Name}.dll");
					if (Net.IsNet7 && File.Exists(path)) name.CodeBase = AssemblyLoader.ToFileUrl(path);
					else
					{
						path = Path.Combine(basepath, "net6.0", $"{args.Name}.dll");
						if (Net.IsNet6 && File.Exists(path)) name.CodeBase = AssemblyLoader.ToFileUrl(path);
						else
						{
							path = Path.Combine(basepath, "net5.0", $"{args.Name}.dll");
							if (Net.IsNet5 && File.Exists(path)) name.CodeBase = AssemblyLoader.ToFileUrl(path);
							else
							{
								path = Path.Combine(basepath, "netstandard2.0", $"{args.Name}.dll");
								if (File.Exists(path)) name.CodeBase = AssemblyLoader.ToFileUrl(path);
								else return null;
							}
						}
					}
				} else if (Net.IsMono) {
					path = Path.Combine(basepath, "mono", $"{args.Name}.dll");
					if (File.Exists(path)) name.CodeBase = AssemblyLoader.ToFileUrl(path);
					else return null;
				} else if (Net.IsFramework)
				{
					path = Path.Combine(basepath, "net40", $"{args.Name}.dll");
					if (Net.IsNet4 && File.Exists(path)) name.CodeBase = AssemblyLoader.ToFileUrl(path);
					else
					{
						path = Path.Combine(basepath, "net35", $"{args.Name}.dll");
						if (Net.IsNet35 && File.Exists(path)) name.CodeBase = AssemblyLoader.ToFileUrl(path);
						else
						{
							path = Path.Combine(basepath, "net20", $"{args.Name}.dll");
							if (Net.IsNet2 && File.Exists(path)) name.CodeBase = AssemblyLoader.ToFileUrl(path);
							return null;
						}
					}
				}
				Assembly a;
				try
				{
					Resolving = true;
					a = Assembly.Load(name);
				} finally
				{
					Resolving = false;
				}
				return a;
			}
			return null;
		}
	}
}