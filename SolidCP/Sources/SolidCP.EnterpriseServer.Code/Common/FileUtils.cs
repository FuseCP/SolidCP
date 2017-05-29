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
using System.Collections.Generic;
using Ionic.Zip;

namespace SolidCP.EnterpriseServer
{
    public class FileUtils
    {
        #region Zip/Unzip Methods

        public static void ZipFiles(string zipFile, string rootPath, string[] files)
        {
			using (ZipFile zip = new ZipFile())
			{
				//use unicode if necessary
				zip.UseUnicodeAsNecessary = true;
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

        public static List<string> UnzipFiles(string zipFile, string destFolder)
        {
			using (ZipFile zip = ZipFile.Read(zipFile))
			{
				foreach (ZipEntry e in zip)
				{
					e.Extract(destFolder, ExtractExistingFileAction.OverwriteSilently);
				}
			}

			// return extracted files names
			return GetFileNames(destFolder);
        }

        #endregion

        #region Copy
        public static void CopyDirectoryContentUNC(string sourceDirectory, string destinationDirectory) {
            foreach(string dir in Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories)) {
                string destinationPath = dir.Replace(sourceDirectory, destinationDirectory);
                if(!Directory.Exists(destinationPath)) { 
                    Directory.CreateDirectory(destinationPath);
                }
            }
            
            foreach(string file in Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories)) {
                string destinationPath = file.Replace(sourceDirectory, destinationDirectory);
                File.Copy(file, destinationPath, true);
            }
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// This function enumerates all directories and files of the <paramref name="direcrotyPath"/> specified.
        /// </summary>
        /// <param name="direcrotyPath">Path to the directory.</param>
        /// <returns>
        /// List of files and directories reside for the <paramref name="direcrotyPath"/> specified.
        /// Empty, when no files and directories are or path does not exists.
        /// </returns>
        public static List<string> GetFileNames(string direcrotyPath)
		{
			List<string> items = new List<string>();

			DirectoryInfo root = new DirectoryInfo(direcrotyPath);
			if (root.Exists)
			{
				// list directories
				foreach (DirectoryInfo dir in root.GetDirectories())
				{
					items.Add(
						System.IO.Path.Combine(direcrotyPath, dir.Name)
						);
				}

				// list files
				foreach (FileInfo file in root.GetFiles())
				{
					items.Add(
						System.IO.Path.Combine(direcrotyPath, file.Name)
						);
				}
			}

			return items;
		}

		#endregion
	}
}
