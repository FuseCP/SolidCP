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
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Security;
using System.Security.Permissions;

using SolidCP.Installer.Common;
using SolidCP.Installer.Services;
using System.Xml;
using System.Runtime.Remoting.Lifetime;
using System.Security.Principal;
using SolidCP.Installer.Core;
using SolidCP.Installer.Configuration;
using System.Reflection;

namespace SolidCP.Installer
{
	/// <summary>
	/// Entry point class 
	/// </summary>
	static class Program
	{
		public const string SetupFromXmlFileParam = "setupxml";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//
			Utils.FixConfigurationSectionDefinition();

			//check security permissions
			if (!Utils.CheckSecurity())
			{
				ShowSecurityError();
				return;
			}

			//check administrator permissions
			if (!Utils.IsAdministrator())
			{
				ShowSecurityError(); 
				return;
			}

			//check for running instance
			if ( !Utils.IsNewInstance())
			{
				UiUtils.ShowRunningInstance();
				return;
			}

			Log.WriteApplicationStart();
			//AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
			Application.ApplicationExit += new EventHandler(OnApplicationExit);
			Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			//check OS version
			Log.WriteInfo("{0} detected", Global.OSVersion);

			//check IIS version
			if (Global.IISVersion.Major == 0)
				Log.WriteError("IIS not found.");
			else
				Log.WriteInfo("IIS {0} detected", Global.IISVersion);

			ApplicationForm mainForm = new ApplicationForm();

			if (!CheckCommandLineArgument("/nocheck"))
			{
				//Check for new versions
				if (CheckForUpdate(mainForm))
				{
					return;
				}
			}
			// Load setup parameters from an XML file
			LoadSetupXmlFile();
			//start application
			mainForm.InitializeApplication();
			Application.Run(mainForm);
			//
			Utils.SaveMutex();
		}

		private static void LoadSetupXmlFile()
		{
			string file = GetCommandLineArgumentValue(SetupFromXmlFileParam);
			if (!string.IsNullOrEmpty(file))
			{
				if (FileUtils.FileExists(file))
				{
					try
					{
						XmlDocument doc = new XmlDocument();
						doc.Load(file);
						Global.SetupXmlDocument = doc;
					}
					catch (Exception ex)
					{
						Log.WriteError("I/O error", ex);
					}
				}
			}
		}

		/// <summary>
		/// Application thread exception handler 
		/// </summary>
		static void OnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Log.WriteError("Fatal error occured.", e.Exception);
			string message = "A fatal error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@solidcp.com.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"SolidCP Installer home directory.";
			MessageBox.Show(message, "SolidCP Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Application.Exit();
		}

		/// <summary>
		/// Application exception handler
		/// </summary>
		static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Log.WriteError("Fatal error occured.", (Exception)e.ExceptionObject);
			string message = "A fatal error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@solidcp.com.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"SolidCP Installer home directory.";
			MessageBox.Show(message, "SolidCP Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Process.GetCurrentProcess().Kill();
		}

		private static void ShowSecurityError()
		{
			string message = "You do not have the appropriate permissions to perform this operation. Make sure you are running the application from the local disk and you have local system administrator privileges.";
			MessageBox.Show(message, "SolidCP Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// Writes to log on application exit
		/// </summary>
		private static void OnApplicationExit(object sender, EventArgs e)
		{
			Log.WriteApplicationEnd();
		}

		/// <summary>
		/// Check whether application is up-to-date
		/// </summary>
		private static bool CheckForUpdate(ApplicationForm mainForm)
		{
			if (!AppConfigManager.AppConfiguration.GetBooleanSetting(ConfigKeys.Web_AutoCheck))
				return false;

			string appName = mainForm.Text;
			string fileName;
			bool updateAvailable;
			try
			{
				updateAvailable = mainForm.CheckForUpdate(out fileName);
			}
			catch (Exception ex)
			{
				Log.WriteError("Update error", ex);
				mainForm.ShowServerError();
				return false;
			}

			if (updateAvailable)
			{
				string message = string.Format("An updated version of {0} is available now.\nWould you like to download and install the latest version?", appName);
				if (MessageBox.Show(message, appName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
				{
					return mainForm.StartUpdateProcess(fileName);
				}
			}
			return false;
		}

		/// <summary>
		/// Check for existing command line argument
		/// </summary>
		private static bool CheckCommandLineArgument(string argName)
		{
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 1; i < args.Length; i++)
			{
				string arg = args[i];
				if (string.Equals(arg, argName, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Check for existing command line argument
		/// </summary>
		private static string GetCommandLineArgumentValue(string argName)
		{
			string key = "/"+argName.ToLower()+":";
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 1; i < args.Length; i++)
			{
				string arg = args[i].ToLower();
				if (arg.StartsWith(key))
				{
					return arg.Substring(key.Length);
				}
			}
			return null;
		}

	}
}
