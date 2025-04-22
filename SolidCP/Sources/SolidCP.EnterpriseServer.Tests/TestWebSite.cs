using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace SolidCP.Tests
{
	public class TestWebSite : IDisposable
	{
		const string EnterpriseServerPath = @"..\..\..\..\SolidCP.EnterpriseServer";

		static object Lock = new object();

		static string path = null;
		public static string Path {
			get {
				string tmpPath = null;
				bool mustClone = false;
				lock (Lock) {
					if (path != null) return path;
					path = IO.Path.Combine(IO.Path.GetTempPath(), "SolidCP", "SolidCP.EnterpriseServer.Tests", Guid.NewGuid().ToString());
					mustClone = true;
					tmpPath = path;
				}
				if (mustClone) CloneTo(tmpPath);
				return path;
			}
		}
		public static void CloneTo(string path)
		{
			var exepath = IO.Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			var esserver = IO.Path.GetFullPath(IO.Path.Combine(exepath, "..", "..", "..", "..", "SolidCP.EnterpriseServer"));

			Console.WriteLine($"Cloning {IO.Path.GetFileName(EnterpriseServerPath)} ...");
			CopyDirectory(esserver, path, true);
		}

		public void Dispose() => Delete();

		public static void Delete()
		{
			string tmpPath = null;
			bool mustDelete = false;
			if (path != null)
			{
				path = null;
				mustDelete = true;
				tmpPath = path;
			}
			if (mustDelete) DeleteDirectory(tmpPath);
		}

		static void DeleteDirectory(string dir) => Directory.Delete(dir, true);
		
		static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
		{
			// Get information about the source directory
			var dir = new DirectoryInfo(sourceDir);

			// Check if the source directory exists
			if (!dir.Exists)
				throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

			// Cache directories before we start copying
			DirectoryInfo[] dirs = dir.GetDirectories();

			// Create the destination directory
			Directory.CreateDirectory(destinationDir);

			// Get the files in the source directory and copy to the destination directory
			foreach (FileInfo file in dir.GetFiles())
			{
				string targetFilePath = IO.Path.Combine(destinationDir, file.Name);
				file.CopyTo(targetFilePath);
			}

			// If recursive and copying subdirectories, recursively call this method
			if (recursive)
			{
				foreach (DirectoryInfo subDir in dirs)
				{
					string newDestinationDir = IO.Path.Combine(destinationDir, subDir.Name);
					CopyDirectory(subDir.FullName, newDestinationDir, true);
				}
			}
		}
	}
}
