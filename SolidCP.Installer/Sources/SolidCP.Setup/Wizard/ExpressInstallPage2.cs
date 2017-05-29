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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading;
using System.Windows.Forms;

using SolidCP.Setup.Common;
using SolidCP.Setup.Web;
using SolidCP.Setup.Windows;
using System.Data.SqlClient;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;

using System.Reflection;
using System.Collections.Specialized;
using SolidCP.Setup.Actions;

namespace SolidCP.Setup
{
	public partial class ExpressInstallPage2 : BannerWizardPage
	{
		private Thread thread;
		
		public ExpressInstallPage2()
		{
			InitializeComponent();
			//
			this.CustomCancelHandler = true;
		}

		delegate void StringCallback(string value);
		delegate void IntCallback(int value);

		private void SetProgressValue(int value)
		{
			//thread safe call
			if (InvokeRequired)
			{
				IntCallback callback = new IntCallback(SetProgressValue);
				Invoke(callback, new object[] { value });
			}
			else
			{
				progressBar.Value = value;
				Update();
			}
		}

		private void SetProgressText(string text)
		{
			//thread safe call
			if (InvokeRequired)
			{
				StringCallback callback = new StringCallback(SetProgressText);
				Invoke(callback, new object[] { text });
			}
			else
			{
				lblProcess.Text = text;
				Update();
			}
		}
		
		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);
			string name = Wizard.SetupVariables.ComponentFullName;
			this.Text = string.Format("Installing {0}", name);
			this.Description = string.Format("Please wait while {0} is being installed.", name);
			this.AllowMoveBack = false;
			this.AllowMoveNext = false;
			this.AllowCancel = false;
		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			thread = new Thread(new ThreadStart(this.Start));
			thread.Start();
		}

		/// <summary>
		/// Displays process progress.
		/// </summary>
		public void Start()
		{
			SetProgressValue(0);

			string component = Wizard.SetupVariables.ComponentFullName;
			string componentName = Wizard.SetupVariables.ComponentName;

			try
			{
				SetProgressText("Creating installation script...");

				Wizard.ActionManager.ActionProgressChanged += new EventHandler<ActionProgressEventArgs<int>>((object sender, ActionProgressEventArgs<int> e) =>
				{
					SetProgressText(e.StatusMessage);
				});

				Wizard.ActionManager.TotalProgressChanged += new EventHandler<ProgressEventArgs>((object sender, ProgressEventArgs e) =>
				{
					SetProgressValue(e.Value);
				});

				Wizard.ActionManager.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
				{
					ShowError();
					Rollback();
					//
					return;
				});

				Wizard.ActionManager.Start();
				
				//this.progressBar.EventData = 100;
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				return;
			}

			SetProgressText("Completed. Click Next to continue.");
			this.AllowMoveNext = true;
			this.AllowCancel = false;
			//unattended setup
			if (!string.IsNullOrEmpty(SetupVariables.SetupXml))
				Wizard.GoNext();
		}

		private void InitSetupVaribles(SetupVariables setupVariables)
		{
			try
			{
				Wizard.SetupVariables = setupVariables.Clone();
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
			}
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			if (this.Wizard != null)
			{
				this.Wizard.Cancel += new EventHandler(OnWizardCancel);
			}
			Form parentForm = FindForm();
			parentForm.FormClosing += new FormClosingEventHandler(OnFormClosing);
		}

		void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			AbortProcess();
		}

		private void OnWizardCancel(object sender, EventArgs e)
		{
			AbortProcess();
			this.CustomCancelHandler = false;
			Wizard.Close();
		}

		private void AbortProcess()
		{
			if (this.thread != null)
			{
				if (this.thread.IsAlive)
				{
					this.thread.Abort();
				}
				this.thread.Join();
			}
		}
	}
}
