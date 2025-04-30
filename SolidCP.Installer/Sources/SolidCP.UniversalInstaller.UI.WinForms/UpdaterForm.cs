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
using System.Net;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Compression;

using SolidCP.Providers;
using SolidCP.UniversalInstaller;

namespace SolidCP.Updater
{
	internal partial class UpdaterForm : Form
	{
		private const int ChunkSize = 262144;
		private Thread thread;
		private IInstallerWebService service;

		public UpdaterForm()
		{
			CheckForIllegalCrossThreadCalls = false;
			InitializeComponent();
			this.DialogResult = DialogResult.Cancel;
			Start();
		}

		private void Start()
		{

			thread = new Thread(new ThreadStart(ShowProcess));
			thread.Start();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
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

		/// <summary>
		/// Displays process progress.
		/// </summary>
		public void ShowProcess()
		{
			progressBar.Value = 0;
			lblProcess.Text = "Downloading installation files...";
			Update();
			try
			{
				string url = GetCommandLineArgument("url");
				string targetFile = GetCommandLineArgument("target");
				string fileToDownload = GetCommandLineArgument("file");
				string proxyServer = GetCommandLineArgument("proxy");
				string user = GetCommandLineArgument("user");
				string password = GetCommandLineArgument("password");

				if (!String.IsNullOrEmpty(proxyServer))
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
				// download file
				DownloadFile(fileToDownload, destinationFile, progressBar);
				progressBar.Value = 100;

				// unzip file
				lblProcess.Text = "Unzipping files...";
				progressBar.Value = 0;

				UnzipFile(destinationFile, tempDir, progressBar);
				progressBar.Value = 100;

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
				var ui = $"-ui={UI.Current.GetType().Name.Replace("UI", "").ToLower()}";
				var isExe = Path.GetExtension(targetFile).Equals(".exe", StringComparison.OrdinalIgnoreCase);
				if (isExe)
				{
					info.FileName = targetFile;
					info.Arguments = $"{ui} nocheck";
				} else
				{
					info.FileName = Providers.OS.Shell.Standard.Find(Providers.OS.OSInfo.IsWindows ? "dotnet.exe" : "dotnet");
					info.Arguments = $"\"{targetFile}\" {ui} nocheck";
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
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				string message = ex.ToString();

				ShowError();
				return;
			}
		}

		private void DownloadFile(string sourceFile, string destinationFile, ProgressBar progressBar)
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
					//update progress bar
					progressBar.Value = Convert.ToInt32((downloaded * 100) / fileSize);

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

		private void UnzipFile(string zipFile, string destFolder, ProgressBar progressBar)
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

				progressBar.Minimum = 0;
				progressBar.Maximum = 100;
				progressBar.Value = 0;

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
						} else
						{
							Directory.CreateDirectory(destinationPath.Substring(0, destinationPath.Length - 1));
						}

						unzipped += entry.Length;

						if (zipSize != 0)
						{
							progressBar.Value = Convert.ToInt32(unzipped * 100 / zipSize);
						}
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

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DialogResult != DialogResult.OK && this.thread != null)
			{
				if (this.thread.IsAlive)
				{
					this.thread.Abort();
				}
				this.thread.Join();
			}
		}

		private string GetCommandLineArgument(string argName)
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

		/// <summary>
		/// Shows error message
		/// </summary>
		/// <param name="message">Message</param>
		private void ShowError(string message)
		{
			MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void ShowError()
		{
			string message = "An unexpected error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at info@hostpanelpro.com";
			MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
