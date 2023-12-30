#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Text;
using System.Text.RegularExpressions;


namespace SolidCP.Web.Services
{
	public class Paths
	{
		public static void Split(string path, out string directory, out string name)
		{
			path = Normalize(path);
			int i = path.LastIndexOf('/');
			if (i >= 0 && i < path.Length)
			{
				directory = path.Substring(0, i);
				if (i <= path.Length - 1) name = path.Substring(i + 1);
				else name = "";
			}
			else
			{
				directory = string.Empty;
				name = path;
			}
		}

		public static string Directory(string path)
		{
			if (string.IsNullOrEmpty(path) || path == "~") return "~";
			string dir, file;
			Split(path, out dir, out file);
			return dir;
		}

		public static string File(string path)
		{
			if (string.IsNullOrEmpty(path) || path == "~") return "";
			string dir, file;
			Split(path, out dir, out file);
			return file;
		}

		public static string Move(string file, string to) { return Combine(to, File(file)); }

		/// <summary>
		/// Returns the filename without path & extension 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string FileWithoutExtension(string path)
		{
			int i = path.LastIndexOf('.');
			int j = path.LastIndexOf('/') + 1;
			if (i <= j) i = path.Length;
			return path.Substring(j, i - j);
		}

		/// <summary>
		/// Combines two paths, and resolves '..' segments.
		/// </summary>
		/// <param name="path1">The root path.</param>
		/// <param name="path2">The relative path to the root path.</param>
		/// <returns>The combined path.</returns>
		public static string Combine(string path1, string path2)
		{
			string path = "";
			int slash;

			if (path2.StartsWith("/..")) path2 = path2.Substring(1);

			while (path2.StartsWith("../"))
			{ // resolve relative paths.
				if (path1.EndsWith("/"))
				{
					slash = path1.LastIndexOf('/', path1.Length - 1);
					if (slash <= 0) slash = 0;
					path1 = path1.Substring(0, slash);
				}
				else
				{
					slash = path1.LastIndexOf('/', path1.Length - 1);
					if (slash <= 0) slash = 0;
					slash = path1.LastIndexOf('/', slash - 1);
					if (slash <= 0) slash = 0;
					path1 = path1.Substring(0, slash);
				}
				path2 = path2.Substring(3);
			}

			if (path2.StartsWith("~")) path2 = path2.Substring(1);
			if (path1.EndsWith("/"))
			{
				if (path2.StartsWith("/")) path2 = path2.Substring(1);
				path = path1 + path2;
			}
			else if (path2.StartsWith("/"))
			{
				path = path1 + path2;
			}
			else
			{
				path = path1 + "/" + path2;
			}
			return path;
		}

		public static string Combine(params string[] paths)
		{
			if (paths == null || paths.Length == 0) return "";
			else if (paths.Length == 1) return paths[0];
			else if (paths.Length == 2) return Combine(paths[0], paths[1]);
			else return Combine(paths[0], Combine(paths.Skip(1).ToArray()));
		}

		public static string Relative(string file, string relativePath)
		{
			if (relativePath.Contains(':')) return relativePath;
			return Paths.Combine(Paths.Directory(file) + "/", relativePath);
		}

		public static string Normalize(string path)
		{
			if (path == null) return "~";
			if (path.EndsWith("/")) path = path.Substring(0, path.Length - 1);
			if (path.StartsWith("/"))
			{
				var app = HostingEnvironment.ApplicationVirtualPath;
				if (path.StartsWith(app, StringComparison.OrdinalIgnoreCase))
				{
					path = path.Substring(app.Length);
					if (path.StartsWith("/")) return "~" + path;
					else return "~/" + path;
				}
				else throw new NotSupportedException($"{path}: Absolute path with invalid ApplicationPath.");
			}
			else if (path.StartsWith("~")) return path;
			else return path;
		}

		public static bool IsNonVirtual(string path)
		{
			var lpath = Normalize(path).ToLower();
			return lpath.EndsWith("web.config") ||
				lpath.EndsWith("global.asax") ||
				lpath.StartsWith("~/bin") ||
				lpath.StartsWith("~/app_code") ||
				lpath.StartsWith("~/app_data") ||
				lpath.StartsWith("~/app_globalresources") ||
				lpath.StartsWith("~/app_localresources") ||
				lpath.EndsWith(".sitemap");
		}

		public static string Map(string path)
		{
			if (path.Contains(':')) return path;
			path = path.Replace('/', '\\');
			var appphyspath = HostingEnvironment.ApplicationPhysicalPath;
			if (appphyspath.EndsWith("\\")) appphyspath = appphyspath.Substring(0, appphyspath.Length - 1);
			if (path.StartsWith("~")) path = path.Substring(1);
			if (path.StartsWith("\\")) path = path.Substring(1);
			return appphyspath + "\\" + path;
			// return HostingEnvironment.MapPath(path);
		}

		public static string Unmap(string physicalPath)
		{
			var appphyspath = HostingEnvironment.ApplicationPhysicalPath;
			if (appphyspath.EndsWith("\\")) appphyspath = appphyspath.Substring(0, appphyspath.Length - 1);
			if (physicalPath.StartsWith(appphyspath)) return physicalPath.Replace(appphyspath, "~").Replace("\\", "/");
			return physicalPath;
		}

		public static string Absolute(string path)
		{
			if (path.StartsWith("~"))
			{
				path = Normalize(path).Substring(1);
				var app = HostingEnvironment.ApplicationVirtualPath;
				if (app.Length > 1) path = Combine(app, path);
				if (string.IsNullOrEmpty(path)) return "/";
				return path;
			}
			else
			{
				if (!path.StartsWith("/")) return "/" + path;
				return path;
			}
		}

		public static string AddSlash(string path) { return path.EndsWith("/") ? path : path + "/"; }
		public static string RemoveSlash(string path) { return path.EndsWith("/") ? path.Remove(path.Length - 1) : path; }

		public static string Extension(string path)
		{
			var name = File(path);
			int i = name.LastIndexOf('.');
			if (i > 0) return name.Substring(i + 1).ToLower();
			return string.Empty;
		}

		public static string WithoutExtension(string path)
		{
			int i = path.LastIndexOf('.');
			int j = path.LastIndexOf('/');
			if (i > 0 && i > j) return path.Substring(0, i);
			return path;
		}

		public static string ChangeExtension(string path, string ext)
		{
			if (ext.StartsWith(".")) return WithoutExtension(path) + ext;
			else return WithoutExtension(path) + "." + ext;
		}


	}
}
#endif