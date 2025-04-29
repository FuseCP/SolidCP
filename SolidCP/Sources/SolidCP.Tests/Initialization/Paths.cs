using System;
using System.Collections.Generic;
using IO=System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SolidCP.Tests
{
	public class Paths
	{
		public const string App = "SolidCP";

		static string project = null;

		public static string Test
		{
			get
			{
				if (project == null)
				{
					var path = IO.Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath);
					var dir = IO.Path.GetFileName(path);
					while (Regex.IsMatch(dir, @"^((net[0-9][.0-9]*)|Debug|Release|bin|bin_dotnet)$"))
					{
						path = IO.Path.GetDirectoryName(path);
						dir = IO.Path.GetFileName(path);
					}
					project = path;
				}
				return project;
			}
		}

		public static string EnterpriseServer => IO.Path.GetFullPath(IO.Path.Combine(Test, $@"..\{App}.EnterpriseServer"));
		public static string Server => IO.Path.GetFullPath(IO.Path.Combine(Test, $@"..\{App}.Server"));
		public static string Portal => IO.Path.GetFullPath(IO.Path.Combine(Test, $@"..\{App}.WebPortal"));
		public static string WebDavPortal => IO.Path.GetFullPath(IO.Path.Combine(Test, $@"..\{App}.WebDavPortal"));
		public static string Installer => IO.Path.GetFullPath(IO.Path.Combine(Test, $@"..\..\..\{App}.WebSite\Sources\{App}.WebSite\Sources\"));
		public static string Wsl(string path)
		{
			if (path.Length > 1 && IO.Path.IsPathRooted(path)) path = char.ToLower(path[0]) + path.Substring(2);
			return "/mnt/" + path.Replace('\\', '/'); 
		}
		public static string Path(Component server)
		{
			switch (server)
			{
				case Component.Server:
					return Server;
				case Component.EnterpriseServer:
					return EnterpriseServer;
				case Component.Portal:
					return Portal;
				case Component.WebDavPortal:
					return WebDavPortal;
				case Component.Installer:
					return Installer;
				default:
					throw new ArgumentOutOfRangeException(nameof(server), server, null);
			}
		}
	}
}
