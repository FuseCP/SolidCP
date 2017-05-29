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

namespace SolidCP.Updater.Common
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
		internal static void CreateDirectory(string path)
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
		/// Deletes the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file to be deleted.</param>
		internal static void DeleteFile(string fileName)
		{
			File.Delete(fileName);
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
		
		/// <summary>
		/// Deletes a directory and its contents.
		/// </summary>
		/// <param name="path">The name of the directory to remove. </param>
		/// <param name="recursive">true to remove directories, subdirectories, and files in path; otherwise, false.</param>
		internal static void DeleteDirectory(string path, bool recursive)
		{
			Directory.Delete(path, recursive);
		}
	}
}
