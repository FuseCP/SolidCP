﻿using System;
using System.Linq;
using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mono.Unix;
using SolidCP.Server;
using System.IO.Compression;
using SolidCP.Providers.Virtualization;

namespace SolidCP.Providers.OS { 

	public class Unix: HostingServiceProviderBase, IUnixOperatingSystem
	{

		#region Properties
		protected string UsersHome
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["UsersHome"]); }
		}

		protected string LogDir
		{
			get { return FileUtils.EvaluateSystemVariables(ProviderSettings["LogDir"] ?? "/var/log"); }
		}

		#endregion

		#region Files
		public virtual string CreatePackageFolder(string initialPath)
		{
			return FileUtils.CreatePackageFolder(initialPath);
		}

		public virtual bool FileExists(string path)
		{
			return FileUtils.FileExists(path);
		}

		public virtual bool DirectoryExists(string path)
		{
			return FileUtils.DirectoryExists(path);
		}

		public virtual SystemFile GetFile(string path)
		{
			return FileUtils.GetFile(path);
		}

		public virtual SystemFile[] GetFiles(string path)
		{
			return FileUtils.GetFiles(path);
		}

		public virtual SystemFile[] GetDirectoriesRecursive(string rootFolder, string path)
		{
			return FileUtils.GetDirectoriesRecursive(rootFolder, path);
		}

		public virtual SystemFile[] GetFilesRecursive(string rootFolder, string path)
		{
			return FileUtils.GetFilesRecursive(rootFolder, path);
		}

		public virtual SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern)
		{
			return FileUtils.GetFilesRecursiveByPattern(rootFolder, path, pattern);
		}

		public virtual byte[] GetFileBinaryContent(string path)
		{
			return FileUtils.GetFileBinaryContent(path);
		}

		public virtual byte[] GetFileBinaryContentUsingEncoding(string path, string encoding)
		{
			return FileUtils.GetFileBinaryContent(path, encoding);
		}

		public virtual byte[] GetFileBinaryChunk(string path, int offset, int length)
		{
			return FileUtils.GetFileBinaryChunk(path, offset, length);
		}

		public virtual string GetFileTextContent(string path)
		{
			return FileUtils.GetFileTextContent(path);
		}

		public virtual void CreateFile(string path)
		{
			FileUtils.CreateFile(path);
		}

		public virtual void CreateDirectory(string path)
		{
			FileUtils.CreateDirectory(path);
		}

		public virtual void ChangeFileAttributes(string path, DateTime createdTime, DateTime changedTime)
		{
			FileUtils.ChangeFileAttributes(path, createdTime, changedTime);
		}

		public virtual void DeleteFile(string path)
		{
			FileUtils.DeleteFile(path);
		}

		public virtual void DeleteFiles(string[] files)
		{
			FileUtils.DeleteFiles(files);
		}

		public virtual void DeleteEmptyDirectories(string[] directories)
		{
			FileUtils.DeleteEmptyDirectories(directories);
		}

		public virtual void UpdateFileBinaryContent(string path, byte[] content)
		{
			FileUtils.UpdateFileBinaryContent(path, content);
		}

		public virtual void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding)
		{
			FileUtils.UpdateFileBinaryContent(path, content, encoding);
		}

		public virtual void AppendFileBinaryContent(string path, byte[] chunk)
		{
			FileUtils.AppendFileBinaryContent(path, chunk);
		}

		public virtual void UpdateFileTextContent(string path, string content)
		{
			FileUtils.UpdateFileTextContent(path, content);
		}

		public virtual void MoveFile(string sourcePath, string destinationPath)
		{
			FileUtils.MoveFile(sourcePath, destinationPath);
		}

		public virtual void CopyFile(string sourcePath, string destinationPath)
		{
			FileUtils.CopyFile(sourcePath, destinationPath);
		}

		public virtual void ZipFiles(string zipFile, string rootPath, string[] files)
		{
			FileUtils.ZipFiles(zipFile, rootPath, files);
		}

		public virtual string[] UnzipFiles(string zipFile, string destFolder)
		{
			return FileUtils.UnzipFiles(zipFile, destFolder);
		}

		public virtual void CreateBackupZip(string zipFile, string rootPath)
		{
			FileUtils.CreateBackupZip(zipFile, rootPath);
		}
		#endregion

		#region Synchronizing
		public FolderGraph GetFolderGraph(string path)
		{
			if (!path.EndsWith("\\"))
				path += "\\";

			FolderGraph graph = new FolderGraph();
			graph.Hash = CalculateFileHash(path, path, graph.CheckSums);

			// copy hash to arrays
			graph.CheckSumKeys = new uint[graph.CheckSums.Count];
			graph.CheckSumValues = new FileHash[graph.CheckSums.Count];
			graph.CheckSums.Keys.CopyTo(graph.CheckSumKeys, 0);
			graph.CheckSums.Values.CopyTo(graph.CheckSumValues, 0);

			return graph;
		}

		public void ExecuteSyncActions(FileSyncAction[] actions)
		{
			// perform all operations but not delete ones
			foreach (FileSyncAction action in actions)
			{
				if (action.ActionType == SyncActionType.Create)
				{
					FileUtils.CreateDirectory(action.DestPath);
					continue;
				}
				else if (action.ActionType == SyncActionType.Copy)
				{
					FileUtils.CopyFile(action.SrcPath, action.DestPath);
				}
				else if (action.ActionType == SyncActionType.Move)
				{
					FileUtils.MoveFile(action.SrcPath, action.DestPath);
				}
			}

			// unzip file
			// ...after delete

			// delete files
			foreach (FileSyncAction action in actions)
			{
				if (action.ActionType == SyncActionType.Delete)
				{
					FileUtils.DeleteFile(action.DestPath);
				}
			}
		}

		private FileHash CalculateFileHash(string rootFolder, string path, Dictionary<uint, FileHash> checkSums)
		{
			CRC32 crc32 = new CRC32();

			// check if this is a folder
			if (Directory.Exists(path))
			{
				FileHash folder = new FileHash();
				folder.IsFolder = true;
				folder.Name = Path.GetFileName(path);
				folder.FullName = path.Substring(rootFolder.Length - 1);

				// process child folders and files
				List<string> childFiles = new List<string>();
				childFiles.AddRange(Directory.GetDirectories(path));
				childFiles.AddRange(Directory.GetFiles(path));

				foreach (string childFile in childFiles)
				{
					FileHash childHash = CalculateFileHash(rootFolder, childFile, checkSums);
					folder.Files.Add(childHash);

					// check sum
					folder.CheckSum += childHash.CheckSum;
					folder.CheckSum += ConvertCheckSumToInt(crc32.ComputeHash(Encoding.UTF8.GetBytes(childHash.Name)));

					//Debug.WriteLine(folder.CheckSum + " : " + folder.FullName);
				}

				// move list to array
				folder.FilesArray = folder.Files.ToArray();

				if (!checkSums.ContainsKey(folder.CheckSum))
					checkSums.Add(folder.CheckSum, folder);

				return folder;
			}

			FileHash file = new FileHash();
			file.Name = Path.GetFileName(path);
			file.FullName = path.Substring(rootFolder.Length - 1);

			// calculate CRC32
			using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				file.CheckSum = ConvertCheckSumToInt(
					crc32.ComputeHash(fs));
			}

			if (!checkSums.ContainsKey(file.CheckSum))
				checkSums.Add(file.CheckSum, file);

			//Debug.WriteLine(file.CheckSum + " : " + file.FullName);

			return file;
		}

		private uint ConvertCheckSumToInt(byte[] sumBytes)
		{
			uint checkSum = (uint)sumBytes[0] << 24;
			checkSum |= (uint)sumBytes[1] << 16;
			checkSum |= (uint)sumBytes[2] << 8;
			checkSum |= (uint)sumBytes[3] << 0;
			return checkSum;
		}
		#endregion

		public UnixFileMode GetUnixPermissions(string path)
		{
			var info = UnixFileInfo.GetFileSystemEntry(path);
			if (info != null && info.Exists) return (UnixFileMode)info.FileAccessPermissions;
			throw new FileNotFoundException(path);
		}

		public void GrantUnixPermissions(string path, UnixFileMode mode, bool resetChildPermissions = false)
		{
			if (!resetChildPermissions) {
				var info = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry(path);
				if (info != null && info.Exists)
				{
					info.FileAccessPermissions = (FileAccessPermissions)mode;
					info.Refresh();
				}
				else throw new FileNotFoundException(path);
			} else
			{
				foreach (var e in Directory.EnumerateFileSystemEntries(path))
				{
					var info = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry(e);
					if (info != null && info.Exists) { 
						info.FileAccessPermissions = (FileAccessPermissions)mode;
						info.Refresh();
					} else throw new FileNotFoundException(e);
				}
			} 
		}

		public void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
		{
			throw new NotImplementedException();
		}

		public Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword)
		{
			throw new NotImplementedException();
		}

		public void DeleteDirectoryRecursive(string rootPath)
		{
			FileUtils.DeleteDirectoryRecursive(rootPath);
		}

		public override bool IsInstalled()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
		}

		public List<string> GetLogNames()
		{
			var logs = Directory.EnumerateFiles(LogDir, "*.*", SearchOption.AllDirectories);
			return logs
				.Select(log => Path.GetFileName(log))
				.ToList();
		}

		private IEnumerable<SystemLogEntry> GetLogEntriesAsEnumerable(string logName)
		{
			var logs = Directory.EnumerateFiles(LogDir, "*.*", SearchOption.AllDirectories);
			var log = logs.FirstOrDefault(l => Path.GetFileName(l) == logName);
			if (log != null)
			{
				Stream file;
				file = new FileStream(log, FileMode.Open, FileAccess.Read);
				if (Path.GetExtension(log) == ".gz") file = new GZipStream(file, CompressionMode.Decompress);
				var text = new StreamReader(file, Encoding.UTF8, true).ReadToEnd();

				//TODO: parse log file
				throw new NotImplementedException();
			}
			else yield break;
		}

		public List<SystemLogEntry> GetLogEntries(string logName)
		{
			return GetLogEntriesAsEnumerable(logName).ToList();
		}

		public SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
		{
			var entries = GetLogEntriesAsEnumerable(logName)
				.Skip(startRow - 1)
				.Take(maximumRows)
				.ToArray();

			return new SystemLogEntriesPaged()
			{
				Entries = entries,
				Count = entries.Length
			};
		}

		public void ClearLog(string logName)
		{
			var logs = Directory.EnumerateFiles(LogDir, "*.*", SearchOption.AllDirectories);
			var log = logs.FirstOrDefault(l => Path.GetFileName(l) == logName);
			File.WriteAllText(log, "");
		}

		public OSProcess[] GetOSProcesses()
		{
			var processes = System.Diagnostics.Process.GetProcesses();
			//TODO username & cpu usage
			return processes
				.Select(p => new OSProcess()
				{
					Pid = p.Id,
					Name = p.ProcessName,
					Username = "",
					MemUsage = p.WorkingSet64,
					CpuUsage = 0
				})
				.ToArray();
		}

		public void TerminateOSProcess(int pid)
		{
			var process = System.Diagnostics.Process.GetProcessById(pid);
			process.Kill();
		}

		public OSService[] GetOSServices()
		{
			throw new NotImplementedException();
		}

		public void ChangeOSServiceStatus(string id, OSServiceStatus status)
		{
			throw new NotImplementedException();
		}

		public void RebootSystem()
		{
			throw new NotImplementedException();
		}

		public Memory GetMemory()
		{
			throw new NotImplementedException();
		}

		public string ExecuteSystemCommand(string path, string args)
		{
			try
			{
				string result = FileUtils.ExecuteSystemCommand(path, args);
				return result;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public bool IsUnix() => true;

		Shell bash, sh;
		
		Installer apt, yum, brew;
		public Shell Bash => bash != null ? bash : bash = new Bash();
		public Shell Sh => sh != null ? sh : sh = new Sh();
		public Installer Apt => new Apt();
		public Installer Yum => new Yum();
		public Installer Brew => new Brew();

		public virtual Shell DefaultShell => Bash;
		public virtual Installer DefaultInstaller => Apt;
	}
}