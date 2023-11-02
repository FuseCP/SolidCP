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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Ionic.Zip;
using SolidCP.Providers.OS;
using System.Management;

namespace SolidCP.Providers.Utils
{
    /// <summary>
    /// Defines a contract that a system command provider needs to implement.
    /// </summary>
    public interface ICommandLineProvider
    {
        /// <summary>
        /// Executes the file specifed as if you were executing it via command-line interface.
        /// </summary>
        /// <param name="filePath">Path to the executable file (eq. .exe, .bat, .cmd and etc).</param>
        /// <param name="args">Arguments to pass to the executable file</param>
        /// <param name="outputFile">Path to the output file if you want the output to be written somewhere.</param>
        /// <returns>Output of the command being executed.</returns>
        string Execute(string filePath, string args, string outputFile);
    }

    /// <summary>
    /// Provides a default implementation of running system commands.
    /// </summary>
    public sealed class DefaultCommandLineProvider : ICommandLineProvider
    {
        /// <summary>
        /// Creates a new process and executes the file specifed as if you were executing it via command-line interface.
        /// </summary>
        /// <param name="filePath">Path to the executable file (eq. .exe, .bat, .cmd and etc).</param>
        /// <param name="args">Arguments to pass to the executable file</param>
        /// <param name="outputFile">Path to the output file if you want the output to be written somewhere.</param>
        /// <returns>Output of the command being executed.</returns>
        public string Execute(string filePath, string args, string outputFile)
        {
			// when UseShellExecute is false, we CANNOT start .BAT/.CMD directly - handle this
			string ext = Path.GetExtension(filePath);
			string executable = String.Empty;
			if (".bat".Equals(ext, StringComparison.OrdinalIgnoreCase) 
				|| ".cmd".Equals(ext, StringComparison.OrdinalIgnoreCase))
			{
				// use cmd.exe as executable
				executable = "cmd.exe";
				string oldargs = args;
				string exearg = filePath;
				if (HasWhiteSpace(exearg))
				{
					exearg = "\"" + exearg + "\"";
				}
				args = "/c " + exearg;
				if (!String.IsNullOrEmpty(oldargs))
				{
					args += " " + oldargs;
				}
			}
			else
			{
				executable = filePath;
			}
			// launch system process
			ProcessStartInfo startInfo = new ProcessStartInfo(executable, args);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
			// when UseShellExecute is false, we CANNOT start .BAT/.CMD directly
			startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            // get working directory from executable path
            startInfo.WorkingDirectory = Path.GetDirectoryName(filePath);
            Process proc = Process.Start(startInfo);

            // analyze results
            StreamReader reader = proc.StandardOutput;
            string results = "";
            if (!String.IsNullOrEmpty(outputFile))
            {
                // stream to writer
                StreamWriter writer = new StreamWriter(outputFile);
                int BUFFER_LENGTH = 2048;
                int readBytes = 0;
                char[] buffer = new char[BUFFER_LENGTH];
                while ((readBytes = reader.Read(buffer, 0, BUFFER_LENGTH)) > 0)
                {
                    writer.Write(buffer, 0, readBytes);
                }
                writer.Close();
            }
            else
            {
                // return as string
                results = reader.ReadToEnd();
            }
            reader.Close();

            return results;
        }

		/// <summary>
		/// Determines whether the string has any white space.
		/// </summary>
		/// <param name="input">The string to test for white space.</param>
		/// <returns>
		///   <c>true</c> if the string has white space; otherwise, even for empty or NULL string <c>false</c>.
		/// </returns>
		private bool HasWhiteSpace(string input)
		{
			if (String.IsNullOrEmpty(input))
				return false;

			for (int i = 0; i < input.Length; i++)
			{
				if (char.IsWhiteSpace(input[i]))
					return true;
			}
			return false;
		}
	}

    /// <summary>
    /// Summary description for FileUtils.
    /// </summary>
    public class FileUtils
    {
        private static ICommandLineProvider CliProvider;

        static FileUtils()
        {
            SetDefaultCliProvider(new DefaultCommandLineProvider());
        }

        /// <summary>
        /// Initializes command-line provider for the utility class. Yet this is not a perfect way to inverse control over CLI processing 
        /// but it does its job for the testing purposes.
        /// </summary>
        /// <param name="provider">An instance of a command-line provider to initialize the utility with.</param>
        public static void SetDefaultCliProvider(ICommandLineProvider provider)
        {
            Debug.Assert(provider != null, "Command line provider is null");
            CliProvider = provider;
        }

		public static string EvaluateSystemVariables(string str)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            Regex re = new Regex("%([^\\s\\%]+)%", RegexOptions.IgnoreCase);
            return re.Replace(str, new MatchEvaluator(EvaluateSystemVariable));
        }

        public static string GetExecutablePathWithoutParameters(string path)
        {
            if (String.IsNullOrEmpty(path))
                return path;

            int idx = -1;
            int exeIdx = path.ToLower().IndexOf(".exe");
            int dllIdx = path.ToLower().IndexOf(".dll");

            if (exeIdx != -1)
                idx = exeIdx;

            if (dllIdx != -1)
                idx = dllIdx;

            if (exeIdx != -1 && dllIdx != -1)
            {
                idx = exeIdx;
                if (dllIdx < exeIdx)
                    idx = dllIdx;
            }

            string execPath = path;
            if (idx != -1)
            {
                execPath = path.Substring(0, idx + 4);
                if (execPath.StartsWith("\""))
                    execPath = execPath.Substring(1);
            }

            return execPath;
        }

        public static string CorrectRelativePath(string relativePath)
        {
            // clean path
            string correctedPath = Regex.Replace(relativePath.Replace("/", "\\"),
                    @"\.\\|\.\.|\\\\|\?|\:|\""|\<|\>|\||%|\$", "");
            if (correctedPath.StartsWith("\\"))
                correctedPath = correctedPath.Substring(1);
            return correctedPath;
        }

        private static string EvaluateSystemVariable(Match match)
        {
            string EnvVar = Environment.GetEnvironmentVariable(match.Groups[1].Value);
            if (string.IsNullOrEmpty(EnvVar))
            {
                return @"%" + match.Groups[1].Value + @"%";
            }
            else
            {
                return EnvVar;
            }
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static string CreatePackageFolder(string initialPath)
        {
            // substitute vars
            initialPath = FileUtils.EvaluateSystemVariables(initialPath);

            int i = 0;
            string path = null;
            while (true)
            {
                path = initialPath + ((i == 0) ? "" : i.ToString());
                if (!FileUtils.DirectoryExists(path))
                {
                    // create directory
                    FileUtils.CreateDirectory(path);
                    break;
                }

                i++;
            }

			// Set permissions
			// We decided to inherit NTFS permissions from the parent folder to comply with with the native security schema in Windows,
			// when a user decides on his own how to implement security practices for NTFS permissions schema and harden the server.
			SecurityUtils.GrantNtfsPermissionsBySid(path, SystemSID.ADMINISTRATORS, NTFSPermission.FullControl, true, true);
			SecurityUtils.GrantNtfsPermissionsBySid(path, SystemSID.SYSTEM, NTFSPermission.FullControl, true, true);
			//
            return path;
        }

        public static SystemFile GetFile(string path)
        {
            if (!File.Exists(path))
                return null;

            FileInfo file = new FileInfo(path);
            return new SystemFile(file.Name, file.FullName, false, file.Length,
                file.CreationTime, file.LastWriteTime);
        }

        public static SystemFile[] GetFiles(string path)
        {
            ArrayList items = new ArrayList();
            DirectoryInfo root = new DirectoryInfo(path);

            // get directories
            DirectoryInfo[] dirs = root.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                string fullName = System.IO.Path.Combine(path, dir.Name);
                SystemFile fi = new SystemFile(dir.Name, fullName, true, 0, dir.CreationTime, dir.LastWriteTime);
                items.Add(fi);

                // check if the directory is empty
                fi.IsEmpty = (Directory.GetFileSystemEntries(fullName).Length == 0);
            }

            // get files
            FileInfo[] files = root.GetFiles();
            foreach (FileInfo file in files)
            {
                string fullName = System.IO.Path.Combine(path, file.Name);
                SystemFile fi = new SystemFile(file.Name, fullName, false, file.Length, file.CreationTime, file.LastWriteTime);
                items.Add(fi);
            }

            return (SystemFile[])items.ToArray(typeof(SystemFile));
        }

        public static SystemFile[] GetFilesRecursive(string rootFolder, string path)
        {
            return GetFilesRecursiveByPattern(rootFolder, path, "*.*");
        }

        public static SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern)
        {
            // parse pattern
            string[] patterns = new string[] { pattern };
            if (pattern.IndexOf("|") != -1 || pattern.IndexOf(";") != -1)
                patterns = pattern.Split(new char[] { '|', ';' });

            // get files
            ArrayList files = new ArrayList();
            foreach (string ptrn in patterns)
                GetFilesList(files, rootFolder, path, ptrn);
            return (SystemFile[])files.ToArray(typeof(SystemFile));
        }

        private static void GetFilesList(ArrayList files, string rootFolder, string folder, string pattern)
        {
            string fullPath = System.IO.Path.Combine(rootFolder, folder);

            // add files in the current folder
            FileInfo[] dirFiles = new DirectoryInfo(fullPath).GetFiles(pattern);
            foreach (FileInfo file in dirFiles)
            {
                SystemFile fi = new SystemFile(folder + "\\" + file.Name, file.Name, false, file.Length,
                    file.CreationTime, file.LastWriteTime);
                files.Add(fi);
            }

            // add children folders

            DirectoryInfo[] dirs = new DirectoryInfo(fullPath).GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                GetFilesList(files, rootFolder, System.IO.Path.Combine(folder, dir.Name), pattern);
            }
        }

        public static SystemFile[] GetDirectoriesRecursive(string rootFolder, string path)
        {
            ArrayList folders = new ArrayList();
            GetDirectoriesRecursive(folders, rootFolder, path);
            return (SystemFile[])folders.ToArray(typeof(SystemFile));
        }

        private static void GetDirectoriesRecursive(ArrayList folders, string rootFolder, string folder)
        {
            // add the current folder
            SystemFile fi = new SystemFile("\\" + folder, folder, true, 0, DateTime.Now, DateTime.Now);
            folders.Add(fi);

            // add children folders
            string fullPath = System.IO.Path.Combine(rootFolder, folder);
            DirectoryInfo dir = new DirectoryInfo(fullPath);
            fi.Created = dir.CreationTime;
            fi.Changed = dir.LastWriteTime;
            DirectoryInfo[] subDirs = dir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                GetDirectoriesRecursive(folders, rootFolder, System.IO.Path.Combine(folder, subDir.Name));
            }
        }

        public static byte[] GetFileBinaryContent(string path)
        {
            if (!File.Exists(path))
                return null;

            FileStream stream = new FileStream(path, FileMode.Open);
            if (stream == null)
                return null;

            long length = stream.Length;
            byte[] content = new byte[length];
            stream.Read(content, 0, (int)length);
            stream.Close();
            return content;
        }

		/// <summary>
		/// Returns file contents trying to read according to file contents
		/// </summary>
		/// <param name="path">Path to the file.</param>
		/// <param name="encoding">Current file encoding.</param>
		/// <returns>Array of bytes.</returns>
		/// <remarks>
		/// It uses UTF8 by default, so in case incorrect or not supported encoding name
		/// UTF8 will be used to read file contents and convert it to the byte array.
		/// </remarks>
		public static byte[] GetFileBinaryContent(string path, string encoding)
		{
			if (!File.Exists(path))
				return null;

			Encoding fileEncoding = GetEncodingByNameOrDefault(encoding, Encoding.UTF8);

			string fileContent = String.Empty;
			using (StreamReader sr = new StreamReader(path, fileEncoding))
			{
				fileContent = sr.ReadToEnd();
			}

			return fileEncoding.GetBytes(fileContent).Clone() as byte[];
		}

		/// <summary>
		/// Returns <see cref="Encoding"/> from <paramref name="encoding"/> name specified.
		/// If cannot find <see cref="Encoding"/>, returns <paramref name="defaultEncoding"/>.
		/// </summary>
		/// <param name="encoding">The name of the encoding to return.</param>
		/// <param name="defaultEncoding"><see cref="Encoding"/> that will be returned if no <paramref name="encoding"/> will be found.</param>
		/// <returns>
		/// <see cref="Encoding"/> from the <paramref name="encoding"/> name specified, 
		/// otherwise <paramref name="defaultEncoding"/>.
		/// </returns>
		private static Encoding GetEncodingByNameOrDefault(string encoding, Encoding defaultEncoding)
		{
			Encoding currentEncoding = defaultEncoding;

			try
			{
				currentEncoding = Encoding.GetEncoding(encoding);
			}
			catch(ArgumentException)
			{
				// this encoding is either no supported or incorrect
				// set to default encoding
				currentEncoding = defaultEncoding;
			}

			return currentEncoding;
		}

        public static Stream GetFileBinaryContentStream(string path)
        {
            if (!File.Exists(path))
                return null;

            return new FileStream(path, FileMode.Open);
        }

        public static byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            if (!File.Exists(path))
                return null;

            FileStream stream = new FileStream(path, FileMode.Open);
            if (stream == null)
                return null;

            if (offset > 0)
                stream.Seek(offset, SeekOrigin.Begin);

            byte[] content = new byte[length];
            int readBytes = stream.Read(content, 0, length);
            stream.Close();

            if (readBytes < length)
            {
                byte[] lastContent = new byte[readBytes];
                if (readBytes > 0)
                {
                    Array.Copy(content, 0, lastContent, 0, readBytes);
                }
                // avoiding of getting empty content
                if (lastContent.Length == 0)
                {
                    lastContent = new byte[] { 1 };
                }
                return lastContent;
            }
            else
            {
                return content;
            }
        }

        public static string GetFileTextContent(string path)
        {
            StreamReader reader = new StreamReader(path);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
        }

        public static void UpdateFileBinaryContent(string path, byte[] content)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            stream.Write(content, 0, content.Length);
            stream.Close();
        }

		/// <summary>
		/// Updates file contents using encoding.
		/// </summary>
		/// <param name="path">Path to the file.</param>
		/// <param name="content">File contents.</param>
		/// <param name="encoding">Current file encoding.</param>
		/// <remarks>
		/// It uses UTF8 by default, so in case incorrect or not supported encoding name is submitted, 
		/// then UTF8 will be used to convert bytes to file contents.
		/// </remarks>
		public static void UpdateFileBinaryContent(string path, byte[] content, string encoding)
		{
			Encoding fileEncoding = GetEncodingByNameOrDefault(encoding, Encoding.UTF8);

			using (StreamWriter sw = new StreamWriter(File.Create(path), fileEncoding))
			{
				sw.Write(
					fileEncoding.GetString(content)
					);
			}
		}

        public static void AppendFileBinaryContent(string path, byte[] chunk)
        {
            FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
            stream.Write(chunk, 0, chunk.Length);
            stream.Close();
        }

        public static void UpdateFileTextContent(string path, string content)
        {
            StreamWriter stream = new StreamWriter(path);
            stream.Write(content);
            stream.Close();
        }

        public static void CreateFile(string path)
        {
            FileStream stream = File.Create(path);
            stream.Close();
        }

        public static void ChangeFileAttributes(string path, DateTime createdTime, DateTime changedTime)
        {
            if (Directory.Exists(path))
            {
                Directory.SetCreationTime(path, createdTime);
                Directory.SetLastWriteTime(path, changedTime);
            }
            else if (File.Exists(path))
            {
                File.SetCreationTime(path, createdTime);
                File.SetLastWriteTime(path, changedTime);
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                  File.Delete(path);
            else
                if (Directory.Exists(path))
                Directory.Delete(path, true);
            
        }

        public static void DeleteFiles(string[] files)
        {
            foreach (string file in files)
                DeleteFile(file);
        }

        public static void DeleteEmptyDirectories(string[] directories)
        {
            foreach (string directory in directories)
                DeleteEmptyDirectoryRecursive(directory);
        }

        private static bool DeleteEmptyDirectoryRecursive(string directory)
        {
            if (!Directory.Exists(directory))
                return true;

            // iterate through child folders
            bool empty = true;
            string[] dirs = Directory.GetDirectories(directory);
            foreach (string dir in dirs)
            {
                if (!DeleteEmptyDirectoryRecursive(dir))
                    empty = false;
            }

            string[] files = Directory.GetFiles(directory);
            empty = empty && (files.Length == 0);

            // try to delete directory
            if (empty)
                Directory.Delete(directory);

            return empty;
        }

        public static void MoveFile(string sourcePath, string destinationPath)
        {
            if (File.Exists(sourcePath))
            {
                File.Move(sourcePath, destinationPath);
            }
            else if (Directory.Exists(sourcePath))
            {
                Directory.Move(sourcePath, destinationPath);
            }
            else
            {
                throw new Exception("Specified file is not found!");
            }

            // reset NTFS permissions on destination file/folder
            SecurityUtils.ResetNtfsPermissions(destinationPath);
        }

        public static void CopyFile(string sourcePath, string destinationPath)
        {
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destinationPath, true);
            }
            else if (Directory.Exists(sourcePath))
            {
                CopyDirectory(sourcePath, destinationPath);
            }
            else
            {
                throw new Exception("Specified file is not found!");
            }

            // reset NTFS permissions on destination file/folder
            SecurityUtils.ResetNtfsPermissions(destinationPath);
        }

        private static void CopyDirectory(string sourceDir, string destinationDir)
        {
            // create directory
            DirectoryInfo srcDir = new DirectoryInfo(sourceDir);
            if(!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);

            // create subdirectories
            DirectoryInfo[] dirs = srcDir.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                CopyDirectory(System.IO.Path.Combine(sourceDir, dir.Name),
                    System.IO.Path.Combine(destinationDir, dir.Name));
            }

            // copy files
            FileInfo[] files = srcDir.GetFiles();
            foreach (FileInfo file in files)
            {
                // copy file
                file.CopyTo(System.IO.Path.Combine(destinationDir, file.Name), true);
            }
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                // create directory structure
                Directory.CreateDirectory(path);
            }
        }
		

		public static void ZipFiles(string zipFile, string rootPath, string[] files)
		{
			using (ZipFile zip = new ZipFile())
			{
				//use unicode if necessary
				//zip.UseUnicodeAsNecessary = true;
                zip.UseZip64WhenSaving = Zip64Option.AsNecessary;

				//skip locked files
				zip.ZipErrorAction = ZipErrorAction.Skip;
				foreach (string file in files)
				{
					string fullPath = Path.Combine(rootPath, file);
					if (Directory.Exists(fullPath))
					{
						//add directory with the same directory name
						zip.AddDirectory(fullPath, file);
					}
					else if (File.Exists(fullPath))
					{
						//add file to the root folder
						zip.AddFile(fullPath, "");
					}
				}
				zip.Save(zipFile);
			}
		}

        /*** CreateBackupZip
 * params:
 * zipFile: Destination file of zipped backup
 * rootpath: Path of Folder to Backup Example: C:\HostingSpace\243423
 * 
 * Creates a zipped backup of Folder rootPath, excluding .wspak or .scpak Backup Files
 * */
        public static void CreateBackupZip(string zipFile, string rootpath)
        {
            using (ZipFile zip = new ZipFile())
            {
                //use unicode if necessary
                //zip.UseUnicodeAsNecessary = true;
                zip.UseZip64WhenSaving = Zip64Option.AsNecessary;

                //skip locked files
                zip.ZipErrorAction = ZipErrorAction.Skip;
                string[] zipfiles = BackupFileNames(rootpath, "").ToArray();
                foreach (string file in zipfiles)
                {
                    string fullPath = Path.Combine(rootpath, file);
                    if (Directory.Exists(fullPath))
                    {
                        //add empty Directory
                        zip.AddDirectoryByName(file);
                    }
                    else if (File.Exists(fullPath))
                    {
                        string path = "";
                        try
                        {
                            int idx = file.LastIndexOf("\\");
                            path = file.Substring(0, idx);
                        }
                        catch { }
                        //add file to relative folder
                        zip.AddFile(fullPath, path);
                    }
                }
                zip.Save(zipFile);
            }
        }


        /*** BackupFileNames
         * params:
         * rootpath: Directory to get files and folders recursively
         * folder: "" Empty String (for recursively function)
         * 
         * get all files and folders in rootpath excluded the backup files ending with .scpak or .wspak
         */
        public static List<string> BackupFileNames(string rootpath, string folder)
        {
            // get the list of files
            SystemFile[] files = GetFiles(Path.Combine(rootpath, folder));

            List<String> list_files = new List<String>();
            foreach (SystemFile file_i in files)
            {
                try
                {
                    if (file_i.IsDirectory)
                    {
                        string filename = Path.Combine(folder, file_i.Name);
                        list_files.Add(filename);
                        list_files.AddRange(BackupFileNames(rootpath, filename));
                    }
                    else
                    {
                        string filename = Path.Combine(folder, file_i.Name);
                        // Ignore scpak and old wspak Backup Files in Backup Folder
                        if (!(filename.EndsWith(".scpak")) && !(filename.EndsWith(".wspak")))
                        {
                            list_files.Add(filename);
                        }
                    }
                }
                catch { }

            }
            return list_files;
        }

        public static string[] UnzipFiles(string zipFile, string destFolder)
        {
			using (ZipFile zip = ZipFile.Read(zipFile))
			{
				foreach (ZipEntry e in zip)
				{
					// Remove Read-Only attribute from a zip entry
					if ((e.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
						e.Attributes ^= FileAttributes.ReadOnly;
					//
					e.Extract(destFolder, ExtractExistingFileAction.OverwriteSilently);
				}
			}
			
			List<string> files = new List<string>();
			foreach(SystemFile systemFile in GetFiles(destFolder))
			{
				files.Add(systemFile.FullName);
			}			

			return files.ToArray();
        }

        private static void CreateDirectoriesStructure(string path)
        {
            string dir = System.IO.Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                // create directory structure
                Directory.CreateDirectory(dir);
            }
        }

        public static string ExecuteSystemCommand(string cmd, string args)
        {
            return ExecuteSystemCommand(cmd, args, null);
        }

        /// <summary>
        /// Executes the file specifed as if you were executing it via command-line interface.
        /// </summary>
        /// <param name="filePath">Path to the executable file (eq. .exe, .bat, .cmd and etc).</param>
        /// <param name="args">Arguments to pass to the executable file</param>
        /// <param name="outputFile">Path to the output file if you want the output to be written somewhere.</param>
        /// <returns>Output of the command being executed.</returns>
        public static string ExecuteSystemCommand(string cmd, string args, string outputFile)
        {
            // launch system process
            return CliProvider.Execute(cmd, args, outputFile);
        }

        public static void ExecuteCmdCommand(string command)
        {
            ProcessStartInfo ProcessInfo;
            Process process;

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/C " + command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = false;
            process = Process.Start(ProcessInfo);
            if (process != null)
            {
                process.WaitForExit(500);
                process.Close();
            }
        }

        public static void SaveStreamToFile(Stream stream, string path)
        {
            try
            {
                //Create a file to save to
                Stream toStream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);

                //use the binary reader & writer because
                //they can work with all formats
                //i.e images, text files ,avi,mp3..
                BinaryReader br = new BinaryReader(stream);
                BinaryWriter bw = new BinaryWriter(toStream);

                //copy data from the FromStream to the outStream
                //convert from long to int 
                bw.Write(br.ReadBytes((int)stream.Length));

                //save
                bw.Flush();

                //clean up 
                bw.Close();
                br.Close();
            }
            //use Exception e as it can handle any exception 
            catch
            {
                //code if u like 
            }
        }

        public static long CalculateFolderSize(string path)
        {
            int files = 0;
            int folders = 0;
			// check directory exists
			if (!Directory.Exists(path))
				return 0;
			// normalize path
			path = path.Replace("/", "\\");
			// remove trailing slash
			if (path.EndsWith(@"\"))
				path = path.Substring(0, path.Length - 1);
			// calculate folder size
			return CalculateFolderSize(path, out files, out folders);
        }

        public static int BytesToMb(long bytes)
        {
            return (int)bytes / (1024 * 1024);
        }

        public static long GetTotalFreeSpace(string driveName)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == driveName)
                {
                    return drive.TotalFreeSpace;
                }
            }

            return -1;
        }

        private static long CalculateFolderSize(string path, out int files, out int folders)
        {
            files = 0;
            folders = 0;

            IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
            long size = 0;
            FindData findData = new FindData();

            IntPtr findHandle;

            findHandle = path.StartsWith("\\\\")
                             ? Kernel32.FindFirstFile(path + @"\*", findData)
                             : Kernel32.FindFirstFile(@"\\?\" + path + @"\*", findData);
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

        public static void CreateAccessDatabase(string databasePath)
        {
            if (String.IsNullOrEmpty(databasePath))
                throw new ArgumentException("databasePath");

            string connectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Engine Type=5;Mode=Share Deny None;Jet OLEDB:Database Locking Mode=0",
                databasePath);

            Type adoxType = Type.GetTypeFromProgID("ADOX.Catalog");
            object cat = Activator.CreateInstance(adoxType);
            adoxType.InvokeMember("Create", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod,
                null, cat, new object[] { connectionString });
            object conn = adoxType.InvokeMember("ActiveConnection", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty,
                null, cat, null);
            adoxType.InvokeMember("Close", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null,
                conn, null);
            cat = null;
        }

        public static void DeleteDirectoryRecursive(string rootPath)
        {
            // This code is done this way to force folder deletion 
            // even if the folder was opened

            DirectoryInfo treeRoot = new DirectoryInfo(rootPath);
            if (treeRoot.Exists)
            {

                DirectoryInfo[] dirs = treeRoot.GetDirectories();
                while (dirs.Length > 0)
                {
                    foreach (DirectoryInfo dir in dirs)
                        DeleteDirectoryRecursive(dir.FullName);

                    dirs = treeRoot.GetDirectories();
                }

                // DELETE THE FILES UNDER THE CURRENT ROOT
                string[] files = Directory.GetFiles(treeRoot.FullName);
                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                Directory.Delete(treeRoot.FullName, true);
            }
        }

        public static void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
        {

            try
            {
                string[] splits = folderPath.Split('\\');
                if (splits.Length > 0)
                {
                    // bat file pat
                    string cmdFilePath = @"\\" + splits[2] + @"\" + splits[3] + @"\" + "Process.bat";

                    // Creating the BAT file
                    FileStream fs = File.Create(cmdFilePath);
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }

                    StreamWriter swr = new StreamWriter(cmdFilePath);

                    if (swr != null)
                    {
                        swr.WriteLine(@"cd c:\windows\system32");

                        string sharePath = String.Empty;

                        if (splits.Length > 4)
                        {
                            // Form the share physicalPath
                            sharePath = shareNameDrive + @":\" + splits[3];

                            if (splits.Length == 5 && !quotaLimit.Equals(String.Empty))
                            {

                                switch (mode)
                                {
                                    // Set
                                    case 0:
                                        // Delete the quota in case one exists
                                        swr.WriteLine(@"dirquota quota delete /path:" + sharePath + @"\" + splits[4] + @" /remote:" + splits[2] + @" /quiet");
                                        swr.WriteLine(@"dirquota quota add /path:" + sharePath + @"\" + splits[4] + @" /limit:" + quotaLimit + @" /remote:" + splits[2]);
                                        break;

                                    // Modify
                                    case 1: swr.WriteLine(@"dirquota quota modify /path:" + sharePath + @"\" + splits[4] + @" /limit:" + quotaLimit + @" /remote:" + splits[2]);
                                        break;
                                }
                            }


                        }
                        swr.Flush();
                        swr.Close();
                        swr.Dispose();
                    }

                    ConnectionOptions connOptions = new ConnectionOptions();

                    if (wmiUserName.Length > 0)
                    {
                        connOptions.Username = wmiUserName;
                        connOptions.Password = wmiPassword;
                    }

                    connOptions.Impersonation = ImpersonationLevel.Impersonate;

                    connOptions.EnablePrivileges = true;

                    ManagementScope manScope =
                        new ManagementScope(String.Format(@"\\{0}\ROOT\CIMV2", splits[2]), connOptions);
                    manScope.Connect();

                    ObjectGetOptions objectGetOptions = new ObjectGetOptions();
                    ManagementPath managementPath = new ManagementPath("Win32_Process");
                    ManagementClass processClass = new ManagementClass(manScope, managementPath, objectGetOptions);

                    ManagementBaseObject inParams = processClass.GetMethodParameters("Create");
                    inParams["CommandLine"] = cmdFilePath;
                    ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);

                }
            }
            catch
            { }
        }
       #region Advanced Delete
		/// <summary>
		/// Deletes the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file to be deleted.</param>
		public static void DeleteFileAdvanced(string fileName)
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
		public static void DeleteDirectoryAdvanced(string directory)
		{
			if (!Directory.Exists(directory))
				return;

			// iterate through child folders
			string[] dirs = Directory.GetDirectories(directory);
			foreach (string dir in dirs)
			{
				DeleteDirectoryAdvanced(dir);
			}

			// iterate through child files
			string[] files = Directory.GetFiles(directory);
			foreach (string file in files)
			{
				DeleteFileAdvanced(file);
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

		/// <summary>
		/// Deletes the specified directory.
		/// </summary>
		/// <param name="directory">The name of the directory to be deleted.</param>
		public static void DeleteDirectoryInternal(string directory)
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
		#endregion

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
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
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
