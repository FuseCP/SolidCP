using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Security.Permissions;
using System.Security;
using System.Text;
using System.Threading;
using SolidCP.Providers.OS;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.IO;

namespace SolidCP.UniversalInstaller
{
	public partial class Installer
	{
		public virtual Mutex Mutex { get; private set; }
		public virtual bool IsNewInstance()
		{
			//check only one instance
			bool createdNew = true;
			Mutex = new Mutex(true, "SolidCP Installer", out createdNew);
			return createdNew;
		}
		public virtual void SaveMutex()
		{
			GC.KeepAlive(Mutex);
		}

		public virtual bool CheckSecurity()
		{
			if (OSInfo.IsNetFX)
			{
				try
				{
					PermissionSet set = new PermissionSet(PermissionState.Unrestricted);
					set.Demand();
				}
				catch
				{
					return false;
				}
			}
			return true;
		}
		public virtual void CheckWebServer()
		{
			if (!OSInfo.IsWindows) return;

			if (OSInfo.Current.WebServer.IsInstalled())
				Log.WriteError("IIS not found.");
			else
			{
				var version = OSInfo.Current.WebServer.Version;
				Log.WriteInfo("IIS {0} detected", version);
			}
		}

		public void LogOSVersion()
		{
			Log.WriteInfo($"{(OSInfo.WindowsVersion != WindowsVersion.NonWindows ? OSInfo.WindowsVersion : OSInfo.OSFlavor)} detected.");

		}
		public virtual void ShowLogFile() => UI.ShowLogFile();

		public virtual void StartMain()
		{
			LoadSettings();
			//
			//Utils.FixConfigurationSectionDefinition();

			//check security permissions && administrator permissions
			if (!CheckSecurity() || !IsRunningAsAdmin)
			{
				//ShowSecurityError();
				RestartAsAdmin();
				return;
			}

			//check for running instance
			if (!IsNewInstance())
			{
				UI.ShowRunningInstance();
				return;
			}

			Log.WriteApplicationStart();
			//AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);

			//check OS version
			LogOSVersion();

			//check web server version
			CheckWebServer();

			/*if (!CheckCommandLineArgument("/nocheck"))
			{
				//Check for new versions
				if (CheckForUpdate(mainForm))
				{
					return;
				}
			} */

			/* if (CheckCommandLineArgument("/uselocalsetupdll"))
			{

			} */

			// Load setup parameters from an XML file
			//LoadSetupXmlFile();
			//start application

			UI.Init();
			UI.RunMainUI();

			//
			SaveMutex();
		}

	}
}
