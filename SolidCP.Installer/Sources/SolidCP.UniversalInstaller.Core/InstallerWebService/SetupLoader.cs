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
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;

namespace SolidCP.UniversalInstaller.Core
{
	public class LoaderEventArgs<T> : EventArgs
	{
		public string StatusMessage { get; set; }
		public T EventData { get; set; }
		public bool Cancellable { get; set; }
	}

	public static class SetupLoaderFactory
	{
		/// <summary>
		/// Instantiates either CodeplexLoader or InstallerServiceLoader based on remote file format.
		/// </summary>
		/// <param name="remoteFile"></param>
		/// <returns></returns>
		public static SetupLoader CreateFileLoader(RemoteFile remoteFile)
		{
			if (remoteFile.File.StartsWith("http://installer.solidcp.com/"))
			{
				return new CodeplexLoader(remoteFile);
			}
			else
			{
				return new SetupLoader(remoteFile);
			}
		}
	}

	public class CodeplexLoader : SetupLoader
	{
		public const string WEB_PI_USER_AGENT_HEADER = "PI-Integrator/3.0.0.0({0})";

		private WebClient fileLoader;

		internal CodeplexLoader(RemoteFile remoteFile)
			: base(remoteFile)
		{
			InitFileLoader();
		}

		private void InitFileLoader()
		{
			fileLoader = new WebClient();
			// Set HTTP header for Codeplex to allow direct downloads
			fileLoader.Headers.Add("User-Agent", String.Format(WEB_PI_USER_AGENT_HEADER, Assembly.GetExecutingAssembly().FullName));
		}

		protected override Task GetDownloadFileTask(RemoteFile remoteFile, string tmpFile, CancellationToken ct)
		{
			var downloadFileTask = new Task(() =>
			{
				if (!File.Exists(tmpFile))
				{
					// Mimic synchronous file download operation because we need to track the download progress
					// and be able to cancel the operation in progress
					AutoResetEvent autoEvent = new AutoResetEvent(false);

					if (fileLoader.IsBusy.Equals(true))
					{
						return;
					}

					ct.Register(() =>
					{
						fileLoader.CancelAsync();
					});

					Log.WriteStart("Downloading file");
					Log.WriteInfo("Downloading file \"{0}\" to \"{1}\"", remoteFile, tmpFile);

					// Attach event handlers to track status of the download process
					fileLoader.DownloadProgressChanged += (obj, e) =>
					{
						if (ct.IsCancellationRequested)
							return;

						RaiseOnProgressChangedEvent(e.ProgressPercentage);
						RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage,
								String.Format(DownloadProgressMessage, e.BytesReceived / 1024, e.TotalBytesToReceive / 1024));
					};

					fileLoader.DownloadFileCompleted += (obj, e) =>
					{
						if (ct.IsCancellationRequested == false)
						{
							RaiseOnProgressChangedEvent(100);
							RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage, "100%");
						}

						if (e.Cancelled)
						{
							CancelDownload(tmpFile);
						}

						autoEvent.Set();
					};

					fileLoader.DownloadFileAsync(new Uri(remoteFile.File), tmpFile);
					RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage);

					autoEvent.WaitOne();
				}
			}, ct);

			return downloadFileTask;
		}
	}

	/// <summary>
	/// Loader form.
	/// </summary>
	public class SetupLoader
	{
		public const string ConnectingRemotServiceMessage = "Connecting...";
		public const string DownloadingSetupFilesMessage = "Downloading setup files...";
		public const string CopyingSetupFilesMessage = "Copying setup files...";
		public const string PreparingSetupFilesMessage = "Please wait while Setup prepares the necessary files...";
		public const string DownloadProgressMessage = "{0} KB of {1} KB";
		public const string PrepareSetupProgressMessage = "{0}%";

		public const int ChunkSize = 262144;
		private RemoteFile remoteFile;
		private CancellationTokenSource cts;

		public event EventHandler<LoaderEventArgs<String>> StatusChanged;
		public event EventHandler<LoaderEventArgs<Exception>> OperationFailed;
		public event EventHandler<LoaderEventArgs<Int32>> ProgressChanged;
		public event EventHandler<EventArgs> DownloadComplete;
		public event EventHandler<EventArgs> OperationCompleted;
		public event EventHandler<EventArgs> NoUnzipStatus;
		public event EventHandler<LoaderEventArgs<int>> SetMaximumProgress;
		internal SetupLoader(RemoteFile remoteFile)
		{
			this.remoteFile = remoteFile;
		}

		public bool SetupOnly { get; set; } = false;
		public void LoadAppDistributive()
		{
			ThreadPool.QueueUserWorkItem(q => LoadAppDistributiveInternal());
		}

		protected void RaiseOnStatusChangedEvent(string statusMessage)
		{
			RaiseOnStatusChangedEvent(statusMessage, String.Empty);
		}

		protected void RaiseOnStatusChangedEvent(string statusMessage, string eventData)
		{
			RaiseOnStatusChangedEvent(statusMessage, eventData, true);
		}

		protected void RaiseDownloadCompleteEvent() => DownloadComplete?.Invoke(this, EventArgs.Empty);
		protected void RaiseOnStatusChangedEvent(string statusMessage, string eventData, bool cancellable)
		{
			if (StatusChanged == null)
			{
				return;
			}
			// No event data for status updates
			StatusChanged(this, new LoaderEventArgs<String>
			{
				StatusMessage = statusMessage,
				EventData = eventData,
				Cancellable = cancellable
			});
		}

		protected void RaiseOnProgressChangedEvent(int eventData)
		{
			RaiseOnProgressChangedEvent(eventData, true);
		}

		protected void RaiseSetMaximumProgressEvent(int eventData)
		{
			SetMaximumProgress?.Invoke(eventData, new LoaderEventArgs<int>() { EventData = eventData });
		}

		protected void RaiseOnProgressChangedEvent(int eventData, bool cancellable)
		{
			if (ProgressChanged == null)
			{
				return;
			}
			//
			ProgressChanged(this, new LoaderEventArgs<int>
			{
				EventData = eventData,
				Cancellable = cancellable
			});
		}

		protected void RaiseOnOperationFailedEvent(Exception ex)
		{
			if (OperationFailed == null)
			{
				return;
			}
			//
			OperationFailed(this, new LoaderEventArgs<Exception> { EventData = ex });
		}

		protected void RaiseOnOperationCompletedEvent()
		{
			if (OperationCompleted == null)
			{
				return;
			}
			//
			OperationCompleted(this, EventArgs.Empty);
		}

		protected void RaiseNoUzipStatusEvent() => NoUnzipStatus?.Invoke(this, EventArgs.Empty);

		/// <summary>
		/// Executes a file download request asynchronously
		/// </summary>
		private void LoadAppDistributiveInternal()
		{
			try
			{
				string dataFolder;
				string tmpFolder;
				// Retrieve local storage configuration
				GetLocalStorageInfo(out dataFolder, out tmpFolder);
				// Initialize storage
				InitializeLocalStorage(dataFolder, tmpFolder);

				cts = new CancellationTokenSource();
				CancellationToken token = cts.Token;

				var fileToDownload = Path.GetFileName(remoteFile.File);
				var exeFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				var installerPath = remoteFile.Release.InstallerPath.Replace('/', Path.DirectorySeparatorChar);
				var setupFileName = Path.Combine(exeFolder,
					Path.GetFileName(installerPath));
				var destinationFile = Path.Combine(dataFolder, fileToDownload);
				var tmpFile = Path.Combine(tmpFolder, fileToDownload);

				if (File.Exists(setupFileName))
				{
					var assembly = Mono.Cecil.AssemblyDefinition.ReadAssembly(setupFileName);
					var version = assembly.Name.Version;
					if (version.Major == remoteFile.Release.Version.Major &&
						version.Minor == remoteFile.Release.Version.Minor &&
						version.Build == remoteFile.Release.Version.Build)
					{
						RaiseNoUzipStatusEvent();
						RaiseOnProgressChangedEvent(100);
						RaiseDownloadCompleteEvent();
						RaiseOnOperationCompletedEvent();

						if (!SetupOnly) StartDownloadAsyncRelease(remoteFile, tmpFile, destinationFile, tmpFolder, installerPath, token);

						return;
					}
				}

				// Check for setup file
				var info = Installer.Current.Releases.GetReleaseFileInfo(Path.GetFileNameWithoutExtension(setupFileName).ToLower(), remoteFile.Release.VersionName);
				if (info != null)
				{
					try
					{
						RaiseNoUzipStatusEvent();

						Task downloadSetupTask = GetDownloadFileTask(new RemoteFile(info, true), setupFileName, token);
						
						if (!SetupOnly)
						{
							var downloadComponentTask = downloadSetupTask.ContinueWith(async task =>
							{
								RaiseDownloadCompleteEvent();
								RaiseOnOperationCompletedEvent();

								StartDownloadAsyncRelease(remoteFile, tmpFile, destinationFile, tmpFolder, installerPath, token);
							}, token);
						}
						downloadSetupTask.Start();
					}
					catch (Exception ex) { }

					return;
				}

				try
				{
					// Download the file requested
					Task downloadFileTask = GetDownloadFileTask(remoteFile, tmpFile, token);
					// Move the file downloaded from temporary location to Data folder
					var moveFileTask = downloadFileTask.ContinueWith((t) =>
					{
						RaiseDownloadCompleteEvent();

						if (File.Exists(tmpFile))
						{
							// move downloaded file to data folder
							//RaiseOnStatusChangedEvent(CopyingSetupFilesMessage);
							//
							//RaiseOnProgressChangedEvent(0);

							// Ensure that the target does not exist.
							if (File.Exists(destinationFile))
								FileUtils.DeleteFile(destinationFile);
							File.Move(tmpFile, destinationFile);
							//
							//RaiseOnProgressChangedEvent(1000);
						}
					}, TaskContinuationOptions.NotOnCanceled);
					// Unzip file downloaded
					var unzipFileTask = moveFileTask.ContinueWith((t) =>
					{
						if (File.Exists(destinationFile))
						{
							RaiseOnStatusChangedEvent(PreparingSetupFilesMessage);
							//
							RaiseOnProgressChangedEvent(0);
							//
							UnzipFile(destinationFile, tmpFolder);
							//
							RaiseOnProgressChangedEvent(100);
						}
					}, token);
					//
					var notifyCompletionTask = unzipFileTask.ContinueWith((t) =>
					{
						RaiseOnOperationCompletedEvent();
					}, token);

					downloadFileTask.Start();
					downloadFileTask.Wait();
				}
				catch (AggregateException ae)
				{
					ae.Handle((e) =>
					{
						// We handle cancellation requests
						if (e is OperationCanceledException)
						{
							CancelDownload(tmpFile);
							Log.WriteInfo("Download has been cancelled by the user");
							return true;
						}
						// But other issues just being logged
						Log.WriteError("Could not download the file", e);
						return false;
					});
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException)
					return;

				Log.WriteError("Loader module error", ex);
				//
				RaiseOnOperationFailedEvent(ex);
			}
		}


		public const string DownloadProgressFile = "_downloadProgress";
		protected void StartDownloadAsyncRelease(RemoteFile remoteFile, string tmpFile, string destinationFile,
			string tmpFolder, string installerPath, CancellationToken token)
		{
			var progressFile = Path.Combine(tmpFolder, DownloadProgressFile);

			try
			{
				var installerPathDir = Path.GetDirectoryName(installerPath).Replace(Path.DirectorySeparatorChar, '/');
				if (!string.IsNullOrEmpty(installerPathDir) && installerPathDir != "/") installerPath = installerPathDir;
				var loader = new SetupLoader(remoteFile);
				var progressStream = File.Create(progressFile);

				// report progress in progress file so it can be read by setup
				loader.ProgressChanged += (sender, args) =>
				{
					try
					{
						while (args.EventData > progressStream.Length)
						{
							progressStream.WriteByte(0);
							progressStream.Flush();
						}
					}
					catch { }
				};

				// Download the file requested
				Task downloadFileTask = loader.GetDownloadFileTask(remoteFile, tmpFile, token);
				// Move the file downloaded from temporary location to Data folder
				var moveFileTask = downloadFileTask.ContinueWith((t) =>
				{
					if (File.Exists(tmpFile))
					{
						// Ensure that the target does not exist.
						if (File.Exists(destinationFile))
							FileUtils.DeleteFile(destinationFile);
						File.Move(tmpFile, destinationFile);
					}
				}, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.NotOnFaulted);
				var faultDowloadFile = downloadFileTask.ContinueWith(t =>
				{
					progressStream.Close();
					File.Delete(progressFile);
				}, TaskContinuationOptions.NotOnRanToCompletion);
				var faultMoveFileTask = moveFileTask.ContinueWith(t =>
				{
					progressStream.Close();
					File.Delete(progressFile);
				}, TaskContinuationOptions.NotOnRanToCompletion);
				// Unzip file downloaded
				var unzipFileTask = moveFileTask.ContinueWith((t) =>
				{
					if (File.Exists(destinationFile))
					{
						loader.UnzipFile(destinationFile, tmpFolder, name => !name.StartsWith(installerPath));
					}

					progressStream.Close();
					File.Delete(progressFile);
				}, token, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.NotOnFaulted, TaskScheduler.Current);
				var faultUnzipFile = unzipFileTask.ContinueWith(t =>
				{
					progressStream.Close();
					File.Delete(progressFile);
				}, TaskContinuationOptions.NotOnRanToCompletion);
				//

				try
				{
					downloadFileTask.Start();
				}
				catch { }
			}
			catch (AggregateException ae)
			{
				ae.Handle((e) =>
				{
					// We handle cancellation requests
					if (e is OperationCanceledException)
					{
						CancelDownload(tmpFile);
						Log.WriteInfo("Download has been cancelled by the user");
						return true;
					}
					// But other issues just being logged
					Log.WriteError("Could not download the file", e);
					return false;
				});
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException)
					return;

				Log.WriteError("Loader module error", ex);
				//
				RaiseOnOperationFailedEvent(ex);
			}
		}

		protected virtual Task GetDownloadFileTask(RemoteFile sourceFile, string tmpFile, CancellationToken ct)
		{
			var downloadFileTask = new Task(async () =>
			{
				if (!File.Exists(tmpFile))
				{
					RaiseOnProgressChangedEvent(0);
					RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage);

					Log.WriteStart("Downloading file");
					Log.WriteInfo(string.Format("Downloading file \"{0}\" to \"{1}\"", sourceFile.File, tmpFile));

					long downloadedSize = 0;

					await Installer.Current.Releases.GetFileAsync(remoteFile, tmpFile,
						(downloaded, fileSize) =>
						{
							downloadedSize = downloaded;
							// Update download progress
							RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage,
								string.Format(DownloadProgressMessage, downloaded / 1024, fileSize / 1024));

							RaiseOnProgressChangedEvent(Convert.ToInt32((downloaded * 100) / fileSize));
						});

					RaiseOnProgressChangedEvent(100);
					RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage, "100%");
					Log.WriteEnd(string.Format("Downloaded {0} bytes", downloadedSize));
				}
			}, ct);

			return downloadFileTask;
		}

		private static void InitializeLocalStorage(string dataFolder, string tmpFolder)
		{
			if (!Directory.Exists(dataFolder))
			{
				Directory.CreateDirectory(dataFolder);
				Log.WriteInfo("Data directory created");
			}

			if (Directory.Exists(tmpFolder))
			{
				Directory.Delete(tmpFolder, true);
			}

			if (!Directory.Exists(tmpFolder))
			{
				Directory.CreateDirectory(tmpFolder);
				Log.WriteInfo("Tmp directory created");
			}
		}

		private static void GetLocalStorageInfo(out string dataFolder, out string tmpFolder)
		{
			dataFolder = FileUtils.GetDataDirectory();
			tmpFolder = Installer.Current.ComponentTempPath; //FileUtils.GetTempDirectory(); + "\Component"
		}

		private void UnzipFile(string zipFile, string destFolder, Func<string, bool> filter = null)
		{
			try
			{
				if (filter == null) filter = name => true;

				int val = 0;
				// Negative value means no progress made yet
				int progress = -1;
				//
				Log.WriteStart("Unzipping file");
				Log.WriteInfo(string.Format("Unzipping file \"{0}\" to the folder \"{1}\"", zipFile, destFolder));

				using (var file = new FileStream(zipFile, FileMode.Open, FileAccess.Read))
				using (var zip = new ZipArchive(file))
				{
					long zipSize = file.Length;
					long unzipped = 0;

					int files = 0;

					foreach (var entry in zip.Entries)
					{
						if (filter(entry.FullName))
						{
							if (string.IsNullOrEmpty(entry.Name))
							{
								Directory.CreateDirectory(Path.Combine(destFolder, entry.FullName.Replace('/', Path.DirectorySeparatorChar)));
							}
							else
							{
								entry.ExtractToFile(Path.Combine(destFolder, entry.FullName.Replace('/', Path.DirectorySeparatorChar)), true);
								files++;
							}
						}
						else if (!string.IsNullOrEmpty(entry.Name)) files++;

						unzipped += entry.CompressedLength;

						if (zipSize != 0)
						{
							val = Convert.ToInt32(unzipped * 100 / zipSize);
							// Skip to raise the progress event change when calculated progress 
							// and the current progress value are even
							if (val == progress)
							{
								continue;
							}
							//
							RaiseOnStatusChangedEvent(
								PreparingSetupFilesMessage,
								String.Format(PrepareSetupProgressMessage, val),
								false);
							//
							RaiseOnProgressChangedEvent(val, false);
						}
					}

					Installer.Current.Settings.Installer.Files = files;

					// Notify client the operation can be cancelled at this time
					RaiseOnProgressChangedEvent(100);
					//
					Log.WriteEnd("Unzipped file");
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException)
					return;
				//
				RaiseOnOperationFailedEvent(ex);
			}
		}

		/// <summary>
		/// Cleans up temporary file if the download process has been cancelled.
		/// </summary>
		/// <param name="tmpFile">Path to the temporary file to cleanup</param>
		protected virtual void CancelDownload(string tmpFile)
		{
			if (File.Exists(tmpFile))
			{
				File.Delete(tmpFile);
			}
		}

		public void AbortOperation()
		{
			// Make sure we are in business
			if (cts != null)
			{
				cts.Cancel();
			}
		}
	}
}
