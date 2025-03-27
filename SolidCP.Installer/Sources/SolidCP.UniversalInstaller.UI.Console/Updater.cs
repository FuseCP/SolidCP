using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace SolidCP.UniversalInstaller;  

public class Updater
{
	private const int ChunkSize = 262144;

	private IInstallerWebService service;

	public Updater()
	{
		var url = GetCommandLineArgument("url");
		Installer.Current.Settings.Installer.WebServiceUrl = url;
		service = Installer.Current.InstallerWebService;
	}

	public void Update()
	{
		try
		{
			string url = GetCommandLineArgument("url");
			string targetFile = GetCommandLineArgument("target");
			string fileToDownload = GetCommandLineArgument("file");
			string proxyServer = GetCommandLineArgument("proxy");
			string user = GetCommandLineArgument("user");
			string password = GetCommandLineArgument("password");

			if (!string.IsNullOrEmpty(proxyServer))
			{
				Installer.Current.Settings.Installer.Proxy = new ProxySettings();
				Installer.Current.Settings.Installer.Proxy.Address = proxyServer;
				if (!String.IsNullOrEmpty(user))
				{
					Installer.Current.Settings.Installer.Proxy.Username = user;
					Installer.Current.Settings.Installer.Proxy.Password = password;
				}
			}

			Installer.Current.Settings.Installer.WebServiceUrl = url;
			service = Installer.Current.InstallerWebService;

			string destinationFile = Path.GetTempFileName();
			string baseDir = Path.GetDirectoryName(targetFile);
			string tempDir = Path.Combine(baseDir, "Temp");

			DownloadFile(fileToDownload, destinationFile);

			UnzipFile(destinationFile, tempDir);

			if (Providers.OS.OSInfo.IsCore)
			{
				for (int ver = 20; ver >= 8; ver--)
				{
					var path = Path.Combine(tempDir, $"net{ver}.0");
					if (Directory.Exists(path))
					{
						CopyDirectory(path, baseDir, true);
						break;
					}
				}
			}
			else
			{
				CopyDirectory(Path.Combine(tempDir, "net48"), baseDir, true);
			}

			FileUtils.DeleteFile(destinationFile);
			Directory.Delete(tempDir, true);

			ProcessStartInfo info = new ProcessStartInfo();
			var isExe = Path.GetExtension(targetFile).Equals(".exe", StringComparison.OrdinalIgnoreCase);
			if (isExe)
			{
				if (UI.Current.IsConsole && Providers.OS.OSInfo.IsWindows)
				{
					info.FileName = Providers.OS.Shell.Standard.Find("cmd.exe");
					info.Arguments = $"/C \"{targetFile}\" nocheck";
					info.UseShellExecute = true;
					info.CreateNoWindow = false;
				} else {
					info.FileName = targetFile;
					info.Arguments = "nocheck";
					info.UseShellExecute = false;
					info.CreateNoWindow = false;
				}
			}
			else
			{
				if (UI.Current.IsConsole && Providers.OS.OSInfo.IsWindows)
				{
					info.FileName = Providers.OS.Shell.Standard.Find("cmd.exe"); 
					var dotnet = Providers.OS.Shell.Standard.Find(Providers.OS.OSInfo.IsWindows ? "dotnet.exe" : "dotnet");
					info.Arguments = $"/C \"{dotnet}\" \"{targetFile}\" nocheck";
				}
				else {
					info.FileName = Providers.OS.Shell.Standard.Find(Providers.OS.OSInfo.IsWindows ? "dotnet.exe" : "dotnet");
					info.Arguments = $"\"{targetFile}\" nocheck";
				}
			}

			//info.WindowStyle = ProcessWindowStyle.Normal;
			Process process = Process.Start(info);
			//activate window
			if (Providers.OS.OSInfo.IsWindows && process.Handle != IntPtr.Zero)
			{
				User32.SetForegroundWindow(process.Handle);
				/*if (User32.IsIconic(process.Handle))
				{
					User32.ShowWindowAsync(process.Handle, User32.SW_RESTORE);
				}
				else
				{
					User32.ShowWindowAsync(process.Handle, User32.SW_SHOWNORMAL);
				}*/
			}

			Installer.Current.Exit();
		}
		catch (Exception ex)
		{
			if (Utils.IsThreadAbortException(ex))
				return;
			string message = ex.ToString();

			return;
		}
	}

	private void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
	{
		// Get information about the source directory
		var dir = new DirectoryInfo(sourceDir);

		// Check if the source directory exists
		if (!dir.Exists)
			throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

		// Cache directories before we start copying
		DirectoryInfo[] dirs = dir.GetDirectories();

		// Create the destination directory
		if (!Directory.Exists(destinationDir)) Directory.CreateDirectory(destinationDir);

		// Get the files in the source directory and copy to the destination directory
		foreach (FileInfo file in dir.GetFiles())
		{
			string targetFilePath = Path.Combine(destinationDir, file.Name);
			file.CopyTo(targetFilePath, true);
		}

		// If recursive and copying subdirectories, recursively call this method
		if (recursive)
		{
			foreach (DirectoryInfo subDir in dirs)
			{
				string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
				CopyDirectory(subDir.FullName, newDestinationDir, true);
			}
		}
	}

	private void DownloadFile(string sourceFile, string destinationFile)
	{
		try
		{
			long downloaded = 0;
			long fileSize = service.GetFileSize(sourceFile);
			if (fileSize == 0)
			{
				throw new FileNotFoundException("Service returned empty file.", sourceFile);
			}

			byte[] content;

			while (downloaded < fileSize)
			{
				content = service.GetFileChunk(sourceFile, (int)downloaded, ChunkSize);
				if (content == null)
				{
					throw new FileNotFoundException("Service returned NULL file content.", sourceFile);
				}
				FileUtils.AppendFileContent(destinationFile, content);
				downloaded += content.Length;

				if (content.Length < ChunkSize)
					break;
			}
		}
		catch (Exception ex)
		{
			if (Utils.IsThreadAbortException(ex))
				return;

			throw;
		}
	}

	private static void UnzipFile(string zipFile, string destFolder)
	{
		try
		{
			destFolder = Path.GetFullPath(destFolder);
			if (!destFolder.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
				destFolder += Path.DirectorySeparatorChar;

			if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);

			//calculate size
			long zipSize = 0;
			using (ZipArchive zip = ZipFile.OpenRead(zipFile))
			{
				foreach (ZipArchiveEntry entry in zip.Entries)
				{
					zipSize += entry.Length;
				}
			}

			long unzipped = 0;
			using (ZipArchive zip = ZipFile.OpenRead(zipFile))
			{
				foreach (ZipArchiveEntry entry in zip.Entries)
				{
					// Gets the full path to ensure that relative segments are removed.
					string destinationPath = Path.GetFullPath(Path.Combine(destFolder, entry.FullName))
						.Replace('/', Path.DirectorySeparatorChar)
						.Replace('\\', Path.DirectorySeparatorChar);

					// Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
					// are case-insensitive.
					if (destinationPath.StartsWith(destFolder, StringComparison.Ordinal) &&
						!string.IsNullOrEmpty(entry.Name))
					{
						entry.ExtractToFile(destinationPath, true);
					}
					else
					{
						Directory.CreateDirectory(destinationPath.Substring(0, destinationPath.Length - 1));
					}

					unzipped += entry.Length;
				}
			}
		}
		catch (Exception ex)
		{
			if (Utils.IsThreadAbortException(ex))
				return;

			throw;
		}
	}

	private static string GetCommandLineArgument(string argName)
	{
		argName = "-" + argName + ":";
		string[] args = Environment.GetCommandLineArgs();
		for (int i = 1; i < args.Length; i++)
		{
			string arg = args[i];
			if (arg.StartsWith(argName))
			{
				string text = arg.Substring(argName.Length);
				if (text.StartsWith("\"") && text.EndsWith("\""))
				{
					text = text.Substring(1, text.Length - 2);
				}
				return text;
			}
		}
		return string.Empty;
	}

}
