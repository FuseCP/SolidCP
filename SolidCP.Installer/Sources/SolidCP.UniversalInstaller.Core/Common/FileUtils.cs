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
using System.Text;
using System.IO;

namespace SolidCP.UniversalInstaller
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
        /// Creates drectory with the specified directory.
        /// </summary>
        /// <param name="path">The directory path to create.</param>
        public static void CreateDirectory(string path)
        {
            string dir = Path.GetDirectoryName(path);
            if(!Directory.Exists(dir))
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
        public static void SaveFileContent(string fileName, byte[] content)
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
        public static void AppendFileContent(string fileName, byte[] content)
        {
            FileStream stream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            stream.Write(content, 0, content.Length);
            stream.Close();
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted.</param>
        public static void DeleteFile(string fileName)
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
        public static void DeleteDirectory(string directory)
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
        public static bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns></returns>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
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

        /// <summary>
        /// Deletes application's Data folder.
        /// </summary>
        public static void DeleteDataDirectory()
        {
            try
            {
                DeleteDirectory(GetDataDirectory());
            }
            catch (Exception ex)
            {
                Log.WriteError("IO Error", ex);
            }
        }

        /// <summary>
        /// Deletes application Tmp directory.
        /// </summary>
        public static void DeleteTempDirectory()
        {
            try
            {
                DeleteDirectory(GetTempDirectory());
            }
            catch (Exception ex)
            {
                Log.WriteError("IO Error", ex);
            }
        }
    }
}
