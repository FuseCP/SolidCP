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
using Ionic.Zip;

using SolidCP.Installer.Common;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace SolidCP.Installer.Core
{
    public class LoaderEventArgs<T> : EventArgs
    {
        public string StatusMessage { get; set; }
        public T EventData { get; set; }
        public bool Cancellable { get; set; }
    }

    public static class LoaderFactory
    {
        /// <summary>
        /// Instantiates either CodeplexLoader or InstallerServiceLoader based on remote file format.
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <returns></returns>
        public static Loader CreateFileLoader(string remoteFile)
        {
            Debug.Assert(!String.IsNullOrEmpty(remoteFile), "Remote file is empty");

            if (remoteFile.StartsWith("http://installer.solidcp.com/"))
            {
                return new CodeplexLoader(remoteFile);
            }
            else
            {
                return new Loader(remoteFile);
            }
        }
    }

    public class CodeplexLoader : Loader
    {
        public const string WEB_PI_USER_AGENT_HEADER = "PI-Integrator/3.0.0.0({0})";

        private WebClient fileLoader;

        internal CodeplexLoader(string remoteFile) 
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

        protected override Task GetDownloadFileTask(string remoteFile, string tmpFile, CancellationToken ct)
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

                    fileLoader.DownloadFileAsync(new Uri(remoteFile), tmpFile);
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
    public class Loader
    {
        public const string ConnectingRemotServiceMessage = "Connecting...";
        public const string DownloadingSetupFilesMessage = "Downloading setup files...";
        public const string CopyingSetupFilesMessage = "Copying setup files...";
        public const string PreparingSetupFilesMessage = "Please wait while Setup prepares the necessary files...";
        public const string DownloadProgressMessage = "{0} KB of {1} KB";
        public const string PrepareSetupProgressMessage = "{0}%";

        private const int ChunkSize = 262144;
        private string remoteFile;
        private CancellationTokenSource cts;

        public event EventHandler<LoaderEventArgs<String>> StatusChanged;
        public event EventHandler<LoaderEventArgs<Exception>> OperationFailed;
        public event EventHandler<LoaderEventArgs<Int32>> ProgressChanged;
        public event EventHandler<EventArgs> OperationCompleted;

        internal Loader(string remoteFile)
        {
            this.remoteFile = remoteFile;
        }

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

                string fileToDownload = Path.GetFileName(remoteFile);

                string destinationFile = Path.Combine(dataFolder, fileToDownload);
                string tmpFile = Path.Combine(tmpFolder, fileToDownload);

                cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                try
                {
                    // Download the file requested
                    Task downloadFileTask = GetDownloadFileTask(remoteFile, tmpFile, token);
                    // Move the file downloaded from temporary location to Data folder
                    var moveFileTask = downloadFileTask.ContinueWith((t) => 
                    {
                        if (File.Exists(tmpFile))
                        {
                            // copy downloaded file to data folder
                            RaiseOnStatusChangedEvent(CopyingSetupFilesMessage);
                            //
                            RaiseOnProgressChangedEvent(0);

                            // Ensure that the target does not exist.
                            if (File.Exists(destinationFile))
                                FileUtils.DeleteFile(destinationFile);
                            File.Move(tmpFile, destinationFile);
                            //
                            RaiseOnProgressChangedEvent(100);
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
                if (Utils.IsThreadAbortException(ex))
                    return;

                Log.WriteError("Loader module error", ex);
                //
                RaiseOnOperationFailedEvent(ex);
            }
        }

        protected virtual Task GetDownloadFileTask(string sourceFile, string tmpFile, CancellationToken ct)
        {
            var downloadFileTask = new Task(() =>
            {
                if (!File.Exists(tmpFile))
                {
                    var service = ServiceProviderProxy.GetInstallerWebService();

                    RaiseOnProgressChangedEvent(0);
                    RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage);

                    Log.WriteStart("Downloading file");
                    Log.WriteInfo(string.Format("Downloading file \"{0}\" to \"{1}\"", sourceFile, tmpFile));

                    long downloaded = 0;
                    long fileSize = service.GetFileSize(sourceFile);
                    if (fileSize == 0)
                    {
                        throw new FileNotFoundException("Service returned empty file.", sourceFile);
                    }

                    byte[] content;

                    while (downloaded < fileSize)
                    {
                        // Throw OperationCancelledException if there is an incoming cancel request
                        ct.ThrowIfCancellationRequested();

                        content = service.GetFileChunk(sourceFile, (int)downloaded, ChunkSize);
                        if (content == null)
                        {
                            throw new FileNotFoundException("Service returned NULL file content.", sourceFile);
                        }
                        FileUtils.AppendFileContent(tmpFile, content);
                        downloaded += content.Length;
                        // Update download progress
                        RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage,
                            string.Format(DownloadProgressMessage, downloaded / 1024, fileSize / 1024));

                        RaiseOnProgressChangedEvent(Convert.ToInt32((downloaded * 100) / fileSize));

                        if (content.Length < ChunkSize)
                            break;
                    }

                    RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage, "100%");
                    Log.WriteEnd(string.Format("Downloaded {0} bytes", downloaded));
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
            tmpFolder = FileUtils.GetTempDirectory();
        }

        private void UnzipFile(string zipFile, string destFolder)
        {
            try
            {
                int val = 0;
                // Negative value means no progress made yet
                int progress = -1;
                //
                Log.WriteStart("Unzipping file");
                Log.WriteInfo(string.Format("Unzipping file \"{0}\" to the folder \"{1}\"", zipFile, destFolder));

                long zipSize = 0;
                var zipInfo = ZipFile.Read(zipFile);
                try
                {
                    foreach (ZipEntry entry in zipInfo)
                    {
                        if (!entry.IsDirectory)
                            zipSize += entry.UncompressedSize;
                    }
                }
                finally
                {
                    if (zipInfo != null)
                    {
                        zipInfo.Dispose();
                    }
                }

                long unzipped = 0;
                //
                var zip = ZipFile.Read(zipFile);
                //
                try
                {
                    foreach (ZipEntry entry in zip)
                    {
                        //
                        entry.Extract(destFolder, ExtractExistingFileAction.OverwriteSilently);
                        //
                        if (!entry.IsDirectory)
                            unzipped += entry.UncompressedSize;

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
                    // Notify client the operation can be cancelled at this time
                    RaiseOnProgressChangedEvent(100);
                    //
                    Log.WriteEnd("Unzipped file");
                }
                finally
                {
                    if (zip != null)
                    {
                        zip.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                if (Utils.IsThreadAbortException(ex))
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
