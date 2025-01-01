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
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SolidCP.Providers.OS;
using SolidCP.UniversalInstaller;
using SolidCP.UniversalInstaller.Core;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.UniversalInstaller.WinForms;

public partial class UninstallPage : BannerWizardPage
{
	private Thread thread;

	public CommonSettings Settings { get; set; }
	public Action Action { get; set; }
	public UninstallPage()
	{
		InitializeComponent();
	}

	protected override void InitializePageInternal()
	{
		string name = Settings.ComponentName;
		this.Text = string.Format("Uninstalling {0}", name);
		this.Description = string.Format("Please wait while {0} is being uninstalled.", name);
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
		this.progressBar.Value = 0;

		string component = Settings.ComponentName;
		string componentId = Settings.ComponentCode;
		Version iisVersion = OSInfo.IsWindows ? OSInfo.Windows.WebServer?.Version : null;
		bool iis7 = iisVersion?.Major >= 7;

		try
		{
			this.lblProcess.Text = "Creating uninstall script...";
			this.Update();

			//default actions

			//process actions
			//this.progressBar.Value = progress * 100 / actions.Count;
			this.Update();

			try
			{
				Action?.Invoke();
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Uninstall error", ex);
			}

			this.progressBar.Value = 100;

		}
		catch (Exception ex)
		{
			if (Utils.IsThreadAbortException(ex))
				return;

			ShowError();
			this.Wizard.Close();
		}

		this.lblProcess.Text = "Completed. Click Next to continue.";
		this.AllowMoveNext = true;
		this.AllowCancel = true;
	}
}
