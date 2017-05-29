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


using SolidCP.Updater.Common;
using SolidCP.Updater.Services;
using Ionic.Zip;

namespace SolidCP.Updater
{
	internal partial class UpdaterForm : Form
	{
		private const int ChunkSize = 262144;
		private Thread thread;
		private InstallerService service;

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

				service = new InstallerService();
				service.Url = url;

				if (!String.IsNullOrEmpty(proxyServer))
				{
					IWebProxy proxy = new WebProxy(proxyServer);
					if (!String.IsNullOrEmpty(user))
						proxy.Credentials = new NetworkCredential(user, password);
					service.Proxy = proxy;
				}

				string destinationFile = Path.GetTempFileName();
				string baseDir = Path.GetDirectoryName(targetFile);
				// download file
				DownloadFile(fileToDownload, destinationFile, progressBar);
				progressBar.Value = 100;

				// unzip file
				lblProcess.Text = "Unzipping files...";
				progressBar.Value = 0;

				UnzipFile(destinationFile, baseDir, progressBar);
				progressBar.Value = 100;

				FileUtils.DeleteFile(destinationFile);
				ProcessStartInfo info = new ProcessStartInfo();
				info.FileName = targetFile;
				info.Arguments = "nocheck";
				//info.WindowStyle = ProcessWindowStyle.Normal;
				Process process = Process.Start(info);
				//activate window
				if (process.Handle != IntPtr.Zero)
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
				//calculate size
				long zipSize = 0;
				using (ZipFile zip = ZipFile.Read(zipFile))
				{
					foreach (ZipEntry entry in zip)
					{
						if ( !entry.IsDirectory)
							zipSize += entry.UncompressedSize;
					}
				}

				progressBar.Minimum = 0;
				progressBar.Maximum = 100;
				progressBar.Value = 0;

				long unzipped = 0;
				using (ZipFile zip = ZipFile.Read(zipFile))
				{
					foreach (ZipEntry entry in zip)
					{
						entry.Extract(destFolder, ExtractExistingFileAction.OverwriteSilently);  // overwrite == true
						if (!entry.IsDirectory)
							unzipped += entry.UncompressedSize;

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
			argName = "\\" + argName + ":";
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
				"Please contact Technical Support at support@solidcp.com";
			MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
