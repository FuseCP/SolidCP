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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ionic.Zip;

using SolidCP.Installer.Services;
using SolidCP.Installer.Common;
using SolidCP.Installer.Core;

namespace SolidCP.Installer.Controls
{
    public delegate void OperationProgressDelegate(int percentage);

    /// <summary>
    /// Loader form.
    /// </summary>
    internal partial class Loader : Form
    {
        private Core.Loader appLoader;

        public Loader()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        public Loader(string remoteFile, Action<Exception> callback)
            : this()
        {
            Start(remoteFile, callback);
        }

        public Loader(string localFile, string componentCode, string version, Action<Exception> callback)
            : this()
        {
            Start(componentCode, version, callback);
        }

        /// <summary>
        /// Resolves URL of the component's distributive and initiates download process.
        /// </summary>
        /// <param name="componentCode">Component code to resolve</param>
        /// <param name="version">Component version to resolve</param>
        private void Start(string componentCode, string version, Action<Exception> callback)
        {
            string remoteFile = Utils.GetDistributiveLocationInfo(componentCode, version);

            Start(remoteFile, callback);
        }

        /// <summary>
        /// Initializes and starts the app distributive download process.
        /// </summary>
        /// <param name="remoteFile">URL of the file to be downloaded</param>
        private void Start(string remoteFile, Action<Exception> callback)
        {
            appLoader = Core.LoaderFactory.CreateFileLoader(remoteFile);

            appLoader.OperationFailed += new EventHandler<Core.LoaderEventArgs<Exception>>(appLoader_OperationFailed);
            appLoader.OperationFailed += (object sender, Core.LoaderEventArgs<Exception> e) => {
                if (callback != null)
                {
                    try
                    {
                        callback(e.EventData);
                    }
                    catch
                    {
                        // Just swallow the exception as we have no interest in it.
                    }
                }
            };
            appLoader.ProgressChanged += new EventHandler<Core.LoaderEventArgs<Int32>>(appLoader_ProgressChanged);
            appLoader.StatusChanged += new EventHandler<Core.LoaderEventArgs<String>>(appLoader_StatusChanged);
            appLoader.OperationCompleted += new EventHandler<EventArgs>(appLoader_OperationCompleted);

            appLoader.LoadAppDistributive();
        }

        void appLoader_OperationCompleted(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        void appLoader_StatusChanged(object sender, Core.LoaderEventArgs<String> e)
        {
            lblProcess.Text = e.StatusMessage;
            lblValue.Text = e.EventData;
            // Adjust Cancel button availability for an operation being performed
            if (btnCancel.Enabled != e.Cancellable)
            {
                btnCancel.Enabled = e.Cancellable;
            }
            // This check allows to avoid extra form redrawing operations
            if (ControlBox != e.Cancellable)
            {
                ControlBox = e.Cancellable;
            }
        }

        void appLoader_ProgressChanged(object sender, Core.LoaderEventArgs<Int32> e)
        {
            bool updateControl = (progressBar.Value != e.EventData);
            progressBar.Value = e.EventData;
            // Adjust Cancel button availability for an operation being performed
            if (btnCancel.Enabled != e.Cancellable)
            {
                btnCancel.Enabled = e.Cancellable;
            }
            // This check allows to avoid extra form redrawing operations
            if (ControlBox != e.Cancellable)
            {
                ControlBox = e.Cancellable;
            }
            //
            if (updateControl)
            {
                progressBar.Update();
            }
        }

        void appLoader_OperationFailed(object sender, Core.LoaderEventArgs<Exception> e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DetachEventHandlers();
            Log.WriteInfo("Execution was canceled by user");
            Close();
        }

        private void DetachEventHandlers()
        {
            // Detach event handlers
            if (appLoader != null)
            {
                appLoader.OperationFailed -= new EventHandler<Core.LoaderEventArgs<Exception>>(appLoader_OperationFailed);
                appLoader.ProgressChanged -= new EventHandler<Core.LoaderEventArgs<Int32>>(appLoader_ProgressChanged);
                appLoader.StatusChanged -= new EventHandler<Core.LoaderEventArgs<String>>(appLoader_StatusChanged);
                appLoader.OperationCompleted -= new EventHandler<EventArgs>(appLoader_OperationCompleted);
            }
        }

        private void OnLoaderFormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
            {
                if (appLoader != null)
                {
                    appLoader.AbortOperation();
                    appLoader = null;
                }
            }
        }
    }
}
