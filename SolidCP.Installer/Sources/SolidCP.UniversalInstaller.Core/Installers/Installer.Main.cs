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

		public virtual bool CheckSecurityNetFX()
		{
			try
			{
				PermissionSet set = new PermissionSet(PermissionState.Unrestricted);
				set.Demand();
				return true;
			}
			catch
			{
				return false;
			}
		}
		public virtual bool CheckSecurity()
		{
			return !OSInfo.IsNetFX || CheckSecurityNetFX();
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

		public virtual void SetAppDomainUnhandledException()
		{
			AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
			{
				Log.WriteError($"Unhandled Exception:", args.ExceptionObject as Exception);
				UI.Current.ShowError(args.ExceptionObject as Exception);
			};
		}

		public virtual void StartMain()
		{
			SetAppDomainUnhandledException();

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
