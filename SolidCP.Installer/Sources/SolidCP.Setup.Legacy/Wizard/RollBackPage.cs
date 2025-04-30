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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using SolidCP.Setup.Web;
using SolidCP.Setup.Windows;
using SolidCP.Setup.Actions;

namespace SolidCP.Setup
{
	public partial class RollBackPage : BannerWizardPage
	{
		private Thread thread;
		
		public RollBackPage()
		{
			InitializeComponent();
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			string name = Wizard.SetupVariables.ComponentFullName;
			this.Text = "Rolling back";
			this.Description = string.Format("Please wait while {0} is being rolled back.", name);
			this.AllowMoveBack = false;
			this.AllowMoveNext = false;
			this.AllowCancel = false;
			thread = new Thread(new ThreadStart(this.Start));
			thread.Start();
		}

		/// <summary>
		/// Displays process progress.
		/// </summary>
		public void Start()
		{
			this.Update();
			try
			{
				this.progressBar.Value = 0;
				this.lblProcess.Text = "Rolling back...";
				Log.WriteStart("Rolling back");
				//
				Wizard.ActionManager.TotalProgressChanged += new EventHandler<ProgressEventArgs>((object sender, ProgressEventArgs e) =>
				{
					this.progressBar.Value = e.Value;
				});
				//
				Wizard.ActionManager.Rollback();
				//
				Log.WriteEnd("Rolled back");
				this.progressBar.Value = 100;
			}
			catch(Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				ShowError();
				this.Wizard.Close();
			}
			this.lblProcess.Text = "Rollback completed. Click Finish to exit setup.";
			this.AllowMoveNext = true;
			this.AllowCancel = true;
		}
	}
}
