// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyrascx.resight notice, this
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
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Resources;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SolidCP.LocalizationToolkit
{
	static class Program
	{
		private static Resources dsResources = null;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//check security permissions
			if (!CheckSecurity())
			{
				MessageBox.Show("You cannot launch this application from a network share. Please try running it from a local directory.", "Localization Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}

			if (args != null && args.Length == 2 && args[0].ToUpper() == @"-L")
			{
				LoadResources(args[1]);
				return;
			}
			Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new ApplicationForm());
		}

		/// <summary>
		/// Application thread exception handler 
		/// </summary>
		static void OnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			string message = string.Format("A fatal error has occurred.\n" +
				"We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@SolidCP.net.\n{0}",
				e.Exception);
			MessageBox.Show(message, "Localization Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Application.Exit();
		}


		/// <summary>
		/// Check security permissions
		/// </summary>
		private static bool CheckSecurity()
		{
			try
			{
				new PermissionSet(PermissionState.Unrestricted).Demand();
			}
			catch (SecurityException)
			{
				return false;
			}
			return true;
		}

		private static void LoadResources(string source)
		{
			try
			{
				string destination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\en-US");
				if (!Directory.Exists(destination))
				{
					Directory.CreateDirectory(destination);
				}
				dsResources = new Resources();
				LoadFiles(source, source);
				dsResources.AcceptChanges();
				string fileName = Path.Combine(destination, "Resources.xml");
				dsResources.WriteXml(fileName);
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
		}

		/// <summary>
		/// Shows default error message
		/// </summary>
		internal static void ShowError(Exception ex)
		{
			string message = string.Format("An unexpected error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@SolidCP.net\n{0}", ex);
			MessageBox.Show(message, "Localization Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private static void CopyFiles(string source, string destination, string baseDir)
		{
			string[] dirs = Directory.GetDirectories(source);
			foreach (string dir in dirs)
			{
				CopyFiles(dir, destination, baseDir);
			}
			string[] files = Directory.GetFiles(source, "*.ascx.resx", SearchOption.TopDirectoryOnly);
			foreach (string file in files)
			{
				CopyFile(baseDir, destination, file);
			}
		}

		private static void CopyFile(string baseDir, string destination, string file)
		{
			string dirPath = Path.GetDirectoryName(file);
			string dir = destination + Path.DirectorySeparatorChar + dirPath.Remove(0, baseDir.Length);
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			string destFile = destination + Path.DirectorySeparatorChar + file.Remove(0, baseDir.Length + 1);
			File.Copy(file, destFile, true);
		}

		private static void LoadFiles(string sourceDir, string baseDir)
		{
			string[] dirs = Directory.GetDirectories(sourceDir);
			foreach (string dir in dirs)
			{
				LoadFiles(dir, baseDir);
				//break;
			}
			string[] files = Directory.GetFiles(sourceDir, "*.ascx.resx", SearchOption.TopDirectoryOnly);
			foreach (string file in files)
			{
				LoadFile(file, baseDir);
				//break;
			}
		}

		private static void LoadFile(string file, string baseDir)
		{
			string path = file.Substring(baseDir.Length);
			if ( path.StartsWith(Path.DirectorySeparatorChar.ToString()))
			{
				path = path.TrimStart(Path.DirectorySeparatorChar);
			}
			// Create a ResXResourceReader for the file.
			ResXResourceReader rsxr = new ResXResourceReader(file);

			// Create an IDictionaryEnumerator to iterate through the resources.
			IDictionaryEnumerator id = rsxr.GetEnumerator();

			// Iterate through the resources and display the contents to the console.
			foreach (DictionaryEntry d in rsxr)
			{
				string key = d.Key.ToString();
				string enValue = d.Value.ToString();
				dsResources.Resource.Rows.Add(new object[] { path, key, enValue, null });
			}
			//Close the reader.
			rsxr.Close();
		}
	}
}
