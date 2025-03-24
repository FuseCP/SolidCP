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
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SolidCP.UniversalInstaller;
using OS = SolidCP.Providers.OS;

namespace SolidCP.Setup;

public class BaseSetup
{
	public bool IsJsonArguments => true;
	public InstallerSettings Settings => Installer.Current.Settings;

	static AssemblyLoader loader = null;
	static BaseSetup()
	{
		//loader = AssemblyLoader.Init();
		AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
	}
	public void InitCostura()
	{
#if Costura
		CosturaUtility.Initialize();
#endif
	}

	public void Unload()
	{
		AppDomain.CurrentDomain.UnhandledException -= OnDomainUnhandledException;
		loader?.Unload();
	}
	static void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		Log.WriteError("Remote domain error", (Exception)e.ExceptionObject);
	}

	public void AssertLoadContext()
	{
		if (AssemblyLoadContext.GetLoadContext(Installer.Current.GetType().Assembly) ==
			AssemblyLoadContext.Default) throw new NotSupportedException();
	}

	bool argsParsed = false;
	public bool ParseArgs(object args)
	{
		if (OS.OSInfo.IsCore) AssertLoadContext();

		if (!argsParsed)
		{
			argsParsed = true;
			string json;
			json = args as string;
			if (json == null)
			{
				var hashtable = args as Hashtable;
				if (hashtable.Contains("ParametersJson")) json = hashtable["ParametersJson"] as string;
			}
			if (json != null)
			{
				Installer.Current.Settings = JsonConvert.DeserializeObject<InstallerSettings>(json, new VersionConverter(), new StringEnumConverter());

				UI.SetCurrent(Installer.Current.Settings.Installer.UI);
			}
			else
			{
				UI.Current.ShowWarning("You need to upgrade the Installer to install this component.");
				return false;
			}
			if (args is Hashtable hash) UI.Current.ReadArguments(hash);
		}
		return true;
	}
	public virtual Version MinimalInstallerVersion => new Version("2.0.0");
	public virtual string VersionsToUpgrade => "";
	public bool CheckInstallerVersion()
	{
		if (Settings.Installer.Version < MinimalInstallerVersion)
		{
			UI.Current.ShowWarning("You need to upgrade the Installer to install this component.");
			return false;
		}
		else return true;
	}
	public virtual CommonSettings CommonSettings => null;
	public virtual ComponentSettings ComponentSettings => (CommonSettings as ComponentSettings) ?? Installer.Current.Settings.Standalone;
	public virtual ComponentInfo Component => null;

	public virtual bool IsServer => ComponentSettings is ServerSettings;
	public virtual bool IsEnterpriseServer => ComponentSettings is EnterpriseServerSettings;
	public virtual bool IsStandalone => ComponentSettings is StandaloneSettings;
	public virtual bool IsWebPortal => ComponentSettings is WebPortalSettings;
	public virtual bool IsWebDavPortal => ComponentSettings is WebDavPortalSettings;
	public virtual bool HasEnterpriseServerInstallation
		=> Directory.Exists(Path.Combine(Installer.Current.InstallWebRootPath, Installer.Current.EnterpriseServerFolder)) ||
			Directory.Exists(Path.Combine(Installer.Current.InstallWebRootPath, Installer.Current.PathWithSpaces(Installer.Current.EnterpriseServerFolder)));
	public virtual UI.SetupWizard Wizard(object args)
	{
		if (ParseArgs(args) && CheckInstallerVersion())
		{
			var wizard = UI.Current.Wizard
				.Introduction(ComponentSettings)
				.CheckPrerequisites()
				.LicenseAgreement();
			if (!IsStandalone)
			{
				wizard = wizard
					.InstallFolder(CommonSettings)
					.Web(CommonSettings)
					.InsecureHttpWarning(CommonSettings)
					.Certificate(CommonSettings)
					.UserAccount(CommonSettings);

				if (IsServer) wizard = wizard
					.ServerPassword();

				if (IsEnterpriseServer) wizard = wizard
					.ServerAdminPassword();

				if (IsWebPortal)
				{
					wizard = wizard.EnterpriseServerUrl();
				}
			}
			else
			{
				// Set EnterpriseServer setting for embedded EnterpriseServer
				Settings.EnterpriseServer.WebSiteDomain = "";
				Settings.EnterpriseServer.WebSitePort = 9002;
				Settings.EnterpriseServer.WebSiteIp = "";
				Settings.EnterpriseServer.Username = "";
				Settings.EnterpriseServer.Password = "";
				Settings.EnterpriseServer.Urls = "http://localhost:9002";
				Settings.EnterpriseServer.ConfigureCertificateManually = true;

				wizard = wizard
					.InstallFolder(Settings.Standalone)
					.Web(Settings.WebPortal)
					.InsecureHttpWarning(Settings.WebPortal)
					.Certificate(Settings.WebPortal)
					.UserAccount(Settings.WebPortal)
					.ServerAdminPassword()
					.Database()
					.Web(Settings.Server)
					.InsecureHttpWarning(Settings.Server)
					.Certificate(Settings.Server)
					.UserAccount(Settings.Server)
					.ServerPassword();
			}
			return wizard;
		}
		return null;
	}
	public virtual Result InstallOrSetup(object args, string title, Action installer, bool setup = false) {
		var wizard = Wizard(args);
		if (wizard == null) return Result.Abort;
		if (IsEnterpriseServer) wizard = wizard.Database();
		if (setup) Installer.Current.Settings.Installer.Action = SetupActions.Setup;
		else Installer.Current.Settings.Installer.Action = SetupActions.Install;
		var res = wizard
			.RunWithProgress(title, installer, ComponentSettings)
			.Finish()
			.Show() ? Result.OK : Result.Cancel;
		Unload();
		return res;
	}

	public bool CheckUpdate()
	{
		// Find out whether the version(s) are supported in that upgrade
		var upgradeSupported = VersionsToUpgrade.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
			.Any(x => Component?.Version == new Version(x.Trim()));
		// 
		if (!upgradeSupported)
		{
			Log.WriteInfo(
				String.Format("Could not find a suitable version to upgrade. Current version: {0}; Versions supported: {1};", Component?.Version.ToString() ?? "?", VersionsToUpgrade));
			//
			UI.Current.ShowWarning(
				"Your current software version either is not supported or could not be upgraded at this time. Please send log file from the installer to the software vendor for further research on the issue.");
			//
		}

		return upgradeSupported;
	}
	public virtual Result Update(object args, string title, Action installer)
	{
		Result res;
		if (ParseArgs(args))
		{
			Installer.Current.Settings.Installer.Action = SetupActions.Update;

			res = CheckUpdate() && Wizard(args)
				.RunWithProgress(title, installer, ComponentSettings)
				.Finish()
				.Show() ? Result.OK : Result.Cancel;
			Unload();
			return res;
		}
		Unload();
		return Result.Cancel;
	}
			
	public virtual Result Uninstall(object args, string title, Action installer)
	{
		if (ParseArgs(args))
		{
			Installer.Current.Settings.Installer.Action = SetupActions.Uninstall;

			if (CheckInstallerVersion())
			{
				var res = UI.Current.Wizard
					.Introduction(CommonSettings)
					.ConfirmUninstall(CommonSettings)
					.RunWithProgress(title, installer, ComponentSettings)
					.Finish()
					.Show() ? Result.OK : Result.Cancel;
				Unload();
				return res;
			}
		}
		Unload();
		return Result.Cancel;
	}
}
