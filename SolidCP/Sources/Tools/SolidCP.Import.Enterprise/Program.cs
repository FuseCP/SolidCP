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
using System.Windows.Forms;
using System.Threading;

namespace SolidCP.Import.Enterprise
{
	static class Program
	{
		private static ApplicationForm appForm;
		private delegate void ExceptionDelegate(Exception ex);
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Log.WriteApplicationStart();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ApplicationExit += new EventHandler(OnApplicationExit);
			appForm = new ApplicationForm();
			Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnApplicationException);
			
			ConnectForm connectForm = new ConnectForm();
			if (connectForm.ShowDialog() == DialogResult.OK)
			{
				appForm.InitializeForm(connectForm.Username, connectForm.Password);
				Application.Run(appForm);
			}
		}

		/// <summary>
		/// Writes to log on application exit
		/// </summary>
		private static void OnApplicationExit(object sender, EventArgs e)
		{
			Log.WriteApplicationEnd();
		}

		/// <summary>
		/// Thread exception handler 
		/// </summary>
		private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Exception ex = e.Exception;
			PublishOnMainThread(ex);
		}

		/// <summary>
		/// Application domain exception handler 
		/// </summary>
		private static void OnApplicationException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			PublishOnMainThread(ex);
		}

		private static void PublishOnMainThread(Exception exception)
		{
			if (appForm.InvokeRequired)
			{
				appForm.Invoke(new ExceptionDelegate(HandleException), new object[] { exception });
			}
			else
			{
				HandleException(exception);
			}
		}

		private static void HandleException(Exception exception)
		{
			Log.WriteError("Fatal error occured.", exception);
			string message = "A fatal error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@SolidCP.net.\n\n" +
				"Make sure you include a copy of the log file from the \n" +
				"application home directory.";
			MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			Application.Exit();
			Environment.Exit(0);
		}

	}
}
