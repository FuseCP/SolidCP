// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using Ionic.Zip;

namespace SolidCP.Setup
{
	/// <summary>
	/// File utils.
	/// </summary>
	public sealed class FileUtils
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private FileUtils()
		{
		}

		/// <summary>
		/// Returns current application path.
		/// </summary>
		/// <returns>Curent application path.</returns>
		public static string GetCurrentDirectory()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}

		/// <summary>
		/// Returns application temp directory.
		/// </summary>
		/// <returns>Application temp directory.</returns>
		public static string GetTempDirectory()
		{
			return Path.Combine(GetCurrentDirectory(), "Tmp");
		}

		/// <summary>
		/// Returns application data directory.
		/// </summary>
		/// <returns>Application data directory.</returns>
		public static string GetDataDirectory()
		{
			return Path.Combine(GetCurrentDirectory(), "Data");
		}

		internal static void UnzipFile(string targetDir, string zipFile)
		{
			using (ZipFile zip = ZipFile.Read(zipFile))
			{
				foreach (ZipEntry e in zip)
				{
					e.Extract(targetDir, ExtractExistingFileAction.OverwriteSilently);
				}
			}
		}

		/// <summary>
		/// Creates drectory with the specified directory.
		/// </summary>
		/// <param name="path">The directory path to create.</param>
		internal static void CreateDirectory(string path)
		{
			string dir = Path.GetDirectoryName(path);
			if (!Directory.Exists(dir))
			{
				// create directory structure
				Directory.CreateDirectory(dir);
			}
		}

		/// <summary>
		/// Saves file content.
		/// </summary>
		/// <param name="fileName">File name.</param>
		/// <param name="content">The array of bytes to write.</param>
		internal static void SaveFileContent(string fileName, byte[] content)
		{
			FileStream stream = new FileStream(fileName, FileMode.Create);
			stream.Write(content, 0, content.Length);
			stream.Close();
		}

		/// <summary>
		/// Saves file content.
		/// </summary>
		/// <param name="fileName">File name.</param>
		/// <param name="content">The array of bytes to write.</param>
		internal static void AppendFileContent(string fileName, byte[] content)
		{
			FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
			stream.Write(content, 0, content.Length);
			stream.Close();
		}

		/// <summary>
		/// Copies the specified file.
		/// </summary>
		internal static void CopyFile(FileInfo sourceFile, string destinationFile)
		{
			int attempts = 0;
			while (true)
			{
				try
				{
					CopyFileInternal(sourceFile, destinationFile);
					break;
				}
				catch (Exception)
				{
					if (attempts > 2)
						throw;

					attempts++;
					System.Threading.Thread.Sleep(1000);
				}
			}
		}

		/// <summary>
		/// Copies the specified file.
		/// </summary>
		private static void CopyFileInternal(FileInfo sourceFile, string destinationFile)
		{
			try
			{
				sourceFile.CopyTo(destinationFile, true);
			}
			catch (UnauthorizedAccessException)
			{
				//try to overwrite read-only file
				OverwriteReadOnlyFile(sourceFile, destinationFile);
			}
		}

		/// <summary>
		/// Copies the specified file.
		/// </summary>
		private static void OverwriteReadOnlyFile(FileInfo sourceFile, string destinationFile)
		{
			FileInfo fi = new FileInfo(destinationFile);
			if (fi.Exists)
			{
				fi.Attributes = FileAttributes.Normal;
			}
			sourceFile.CopyTo(fi.FullName, true);
		}

		/// <summary>
		/// Deletes the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file to be deleted.</param>
		internal static void DeleteFile(string fileName)
		{
			int attempts = 0;
			while (true)
			{
				try
				{
					DeleteFileInternal(fileName);
					break;
				}
				catch (Exception)
				{
					if (attempts > 2)
						throw;

					attempts++;
					System.Threading.Thread.Sleep(1000);
				}
			}
		}

		/// <summary>
		/// Deletes the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file to be deleted.</param>
		private static void DeleteReadOnlyFile(string fileName)
		{
			FileInfo info = new FileInfo(fileName);
			info.Attributes = FileAttributes.Normal;
			info.Delete();
		}

		/// <summary>
		/// Deletes the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file to be deleted.</param>
		private static void DeleteFileInternal(string fileName)
		{
			try
			{
				File.Delete(fileName);
			}
			catch (UnauthorizedAccessException)
			{
				DeleteReadOnlyFile(fileName);
			}
		}

		/// <summary>
		/// Deletes the specified directory.
		/// </summary>
		/// <param name="directory">The name of the directory to be deleted.</param>
		internal static void DeleteDirectory(string directory)
		{
			if (!Directory.Exists(directory))
				return;

			// iterate through child folders
			string[] dirs = Directory.GetDirectories(directory);
			foreach (string dir in dirs)
			{
				DeleteDirectory(dir);
			}

			// iterate through child files
			string[] files = Directory.GetFiles(directory);
			foreach (string file in files)
			{
				DeleteFile(file);
			}

			//try to delete dir for 3 times
			int attempts = 0;
			while (true)
			{
				try
				{
					DeleteDirectoryInternal(directory);
					break;
				}
				catch (Exception)
				{
					if (attempts > 2)
						throw;

					attempts++;
					System.Threading.Thread.Sleep(1000);
				}
			}
		}
        internal static void DeleteDirectoryFiles(string Dir, Func<string, bool> Predicate)
        {
            if (!Directory.Exists(Dir))
                return;
            string[] Dirs = Directory.GetDirectories(Dir);
            foreach (string Item in Dirs)
                DeleteDirectoryFiles(Item, Predicate);
            string[] Files = Directory.GetFiles(Dir);
            foreach (string Item in Files)
                if(Predicate(Item))
                    DeleteFile(Item);
        }
		/// <summary>
		/// Deletes the specified directory.
		/// </summary>
		/// <param name="directory">The name of the directory to be deleted.</param>
		private static void DeleteDirectoryInternal(string directory)
		{
			try
			{
				Directory.Delete(directory);
			}
			catch (IOException)
			{
				DeleteReadOnlyDirectory(directory);
			}
		}

		/// <summary>
		/// Deletes the specified directory.
		/// </summary>
		/// <param name="directory">The name of the directory to be deleted.</param>
		private static void DeleteReadOnlyDirectory(string directory)
		{
			DirectoryInfo info = new DirectoryInfo(directory);
			info.Attributes = FileAttributes.Normal;
			info.Delete();
		}


		/// <summary>
		/// Determines whether the specified file exists.
		/// </summary>
		/// <param name="fileName">The path to check.</param>
		/// <returns></returns>
		internal static bool FileExists(string fileName)
		{
			return File.Exists(fileName);
		}

		/// <summary>
		/// Determines whether the given path refers to an existing directory on disk.
		/// </summary>
		/// <param name="path">The path to test.</param>
		/// <returns></returns>
		internal static bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public static long CalculateFolderSize(string path)
		{
			int files = 0;
			int folders = 0;
			return CalculateFolderSize(path, out files, out folders);
		}

		public static int CalculateFiles(string path)
		{
			int files = 0;
			int folders = 0;
			CalculateFolderSize(path, out files, out folders);
			return files;
		}

		public static void CopyFileToFolder(string sourceFile, string destinationFolder, string destinationFileName = "")
		{
			string fileName = Path.GetFileName(sourceFile);
			string destinationFile = Path.Combine(destinationFolder, string.IsNullOrWhiteSpace(destinationFileName) ? fileName : destinationFileName);
			CopyFile(new FileInfo(sourceFile), destinationFile);
		}

		/// <summary>
		/// Copy source to the destination folder.
		/// </summary>
		public static void CopyDirectory(string source, string destination)
		{
			int i = 0;
			List<DirectoryInfo> folders = new List<DirectoryInfo>();
			List<FileInfo> files = new List<FileInfo>();

			DirectoryInfo sourceFolder = new DirectoryInfo(source);
			DirectoryInfo destFolder = new DirectoryInfo(destination);

			DirectoryInfo di = null;
			FileInfo fi = null;
			string path = null;

			// Part 1: Indexing
			folders.Add(sourceFolder);
			while (i < folders.Count)
			{
				foreach (DirectoryInfo info in folders[i].GetDirectories())
				{
					if (!folders.Contains(info))
						folders.Add(info);
				}
				foreach (FileInfo info in folders[i].GetFiles())
				{
					files.Add(info);
				}
				i++;
			}

			// Part 2: Destination Folders Creation
			///////////////////////////////////////////////////////
			for (i = 0; i < folders.Count; i++)
			{
				if (folders[i].Exists)
				{
					path = destFolder.FullName +
						Path.DirectorySeparatorChar +
						folders[i].FullName.Remove(0, sourceFolder.FullName.Length);

					di = new DirectoryInfo(path);

					// Prevent IOException
					if (!di.Exists)
						di.Create();
				}
			}

			// Part 3: Source to Destination File Copy
			///////////////////////////////////////////////////////
			for (i = 0; i < files.Count; i++)
			{
				if (files[i].Exists)
				{
					path = destFolder.FullName +
						Path.DirectorySeparatorChar +
						files[i].FullName.Remove(0, sourceFolder.FullName.Length + 1);
					fi = new FileInfo(path);
					files[i].CopyTo(fi.FullName, true);
				}
			}
		}

		public static long CalculateFolderSize(string path, out int files, out int folders)
		{
			files = 0;
			folders = 0;

			if (!Directory.Exists(path))
				return 0;

			IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
			long size = 0;
			FindData findData = new FindData();

			IntPtr findHandle;

			findHandle = Kernel32.FindFirstFile(@"\\?\" + path + @"\*", findData);
			if (findHandle != INVALID_HANDLE_VALUE)
			{

				do
				{
					if ((findData.fileAttributes & (int)FileAttributes.Directory) != 0)
					{

						if (findData.fileName != "." && findData.fileName != "..")
						{
							folders++;

							int subfiles, subfolders;
							string subdirectory = path + (path.EndsWith(@"\") ? "" : @"\") +
								findData.fileName;
							size += CalculateFolderSize(subdirectory, out subfiles, out subfolders);
							folders += subfolders;
							files += subfiles;
						}
					}
					else
					{
						// File
						files++;

						size += (long)findData.nFileSizeLow + (long)findData.nFileSizeHigh * 4294967296;
					}
				}
				while (Kernel32.FindNextFile(findHandle, findData));
				Kernel32.FindClose(findHandle);

			}

			return size;
		}

		public static string SizeToString(long size)
		{
			if (size >= 0x400 && size < 0x100000)
				// kilobytes
				return SizeToKB(size);
			else if (size >= 0x100000 && size < 0x40000000)
				// megabytes
				return SizeToMB(size);
			else if (size >= 0x40000000 && size < 0x10000000000)
				// gigabytes
				return SizeToGB(size);
			else
				return string.Format("{0:0.0}", size);
		}

		public static string SizeToKB(long size)
		{
			return string.Format("{0:0.0} KB", (float)size / 1024 );
		}

		public static string SizeToMB(long size)
		{
			return string.Format("{0:0.0} MB", (float)size / 1024 / 1024);
		}

		public static string SizeToGB(long size)
		{
			return string.Format("{0:0.0} GB", (float)size / 1024 / 1024 / 1024);
		}

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
		   out ulong lpFreeBytesAvailable,
		   out ulong lpTotalNumberOfBytes,
		   out ulong lpTotalNumberOfFreeBytes);
    
	}

	#region File Size Calculation helper classes
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	class FindData
	{
		public int fileAttributes;
		public int creationTime_lowDateTime;
		public int creationTime_highDateTime;
		public int lastAccessTime_lowDateTime;
		public int lastAccessTime_highDateTime;
		public int lastWriteTime_lowDateTime;
		public int lastWriteTime_highDateTime;
		public int nFileSizeHigh;
		public int nFileSizeLow;
		public int dwReserved0;
		public int dwReserved1;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public String fileName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
		public String alternateFileName;
	}

	class Kernel32
	{
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr FindFirstFile(String fileName, [In, Out] FindData findFileData);

		[DllImport("kernel32", CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FindNextFile(IntPtr hFindFile, [In, Out] FindData lpFindFileData);

		[DllImport("kernel32", CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FindClose(IntPtr hFindFile);
	}
	#endregion

}
