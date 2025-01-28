using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SolidCP.UniversalInstaller
{

	[Flags]
	public enum Platforms { Undefined = 0, None = 0, Windows = 1, Unix = 2, All = 3 };

	public class ElementJson
	{
		public string application { get; set; }
		public string name { get; set; }
		public string code { get; set; }
		public string description { get; set; }
		public string platforms { get; set; }
		public int id { get; set; }
		public string fullFilePath { get; set; }
		public string upgradeFilePath { get; set; }
		public string installerPath { get; set; }
		public string installerType { get; set; }
		public string version { get; set; }
		public bool beta { get; set; }
	}

	public class ElementInfo
	{
		public ElementInfo() { }
		public ElementInfo(ElementJson json)
		{
			ReleaseFileId = json.id;
			ApplicationName = json.application;
			Component = $"{json.application} {json.name}";
			Version = json.version;
			Beta = json.beta;
			ComponentDescription = json.description;
			ComponentCode = json.code;
			ComponentName = json.name;
			FullFilePath = json.fullFilePath;
			UpgradeFilePath = json.upgradeFilePath;
			InstallerPath = json.installerPath;
			InstallerType = json.installerType;
			Platforms = json.platforms;
		}
		public int ReleaseFileId { get; set; }
		public string ApplicationName { get; set; }
		public string Component { get; set; }
		public string Version { get; set; }
		public bool Beta { get; set; }
		public string ComponentDescription { get; set; }
		public string ComponentCode { get; set; }
		public string ComponentName { get; set; }
		public string FullFilePath { get; set; }
		public string UpgradeFilePath { get; set; }
		public string InstallerPath { get; set; }
		public string InstallerType { get; set; }
        public string Platforms { get; set; }
	}

	public class ReleaseFileInfo
	{
		public ReleaseFileInfo() { }
		public ReleaseFileInfo(ElementInfo raw)
		{
			ReleaseFileId = raw.ReleaseFileId;
			FullFilePath = raw.FullFilePath;
			UpgradeFilePath = raw.UpgradeFilePath;
			InstallerPath = raw.InstallerPath;
			InstallerType = raw.InstallerType;
			if (string.IsNullOrEmpty(raw.Platforms)) Platforms = Platforms.Windows;
			else {
				Platforms = default;
				foreach (var platformToken in raw.Platforms.Split(','))
				{
					Platforms platform;
					if (Enum.TryParse(platformToken, out platform)) Platforms |= platform;
				}
			}
		}
		public int ReleaseFileId { get; set; }
		public string FullFilePath { get; set; }
		public string UpgradeFilePath { get; set; }
		public string InstallerPath { get; set; }
		public string InstallerType { get; set; }
		public Platforms Platforms { get; set; }
		public bool GitHub { get; set; }
	}

	public class RemoteFile
	{
		public RemoteFile(ComponentUpdateInfo release, bool fullFile)
		{
			Release = release;
			FullFile = fullFile;
		}
		public ComponentUpdateInfo Release { get; set; }
		public bool FullFile { get; set; }
		public string File => FullFile ? Release.FullFilePath : Release.UpgradeFilePath;
	}

	public class ComponentUpdateInfo: ReleaseFileInfo
	{
		public ComponentUpdateInfo() { }
		public ComponentUpdateInfo(ComponentInfo info)
		{
			this.Beta = info.Beta;
			this.FullFilePath = info.FullFilePath;
			this.InstallerPath = info.InstallerPath;
			this.InstallerType = info.InstallerType;
			this.Platforms = info.Platforms;
			this.ReleaseFileId = info.ReleaseFileId;
			this.UpgradeFilePath = info.UpgradeFilePath;
			this.Version = info.Version;
			this.VersionName = info.VersionName;
			this.GitHub = info.GitHub;
		}
		public ComponentUpdateInfo(ReleaseFileInfo info, string release)
		{
			Version version = default;
			var vm = Regex.Match(release, "[0-9][0-9.]+");
			if (vm.Success) Version.TryParse(vm.Value, out version);
			this.Beta = Regex.IsMatch(release, "beta|alpha", RegexOptions.IgnoreCase);
			this.FullFilePath = info.FullFilePath;
			this.InstallerPath = info.InstallerPath;
			this.InstallerType = info.InstallerType;
			this.Platforms = info.Platforms;
			this.ReleaseFileId = info.ReleaseFileId;
			this.UpgradeFilePath = info.UpgradeFilePath;
			this.Version = version;
			this.VersionName = release;
			this.GitHub = info.GitHub;
		}

		public ComponentUpdateInfo(ElementInfo raw): base(raw)
		{
			VersionName = raw.Version;
			Version version;
			var m = Regex.Match(raw.Version, "[0-9.]+");
			if (m.Success && Version.TryParse(m.Value, out version)) Version = version;
			else Version = default;
			Beta = raw.Beta;
		}
		public string VersionName { get; set; }
		public Version Version { get; set; }
		public bool Beta { get; set; }
	}
	public class ComponentInfo : ComponentUpdateInfo
	{
		public ComponentInfo(): base() { }
	
		public ComponentInfo(ComponentInfo component, ComponentUpdateInfo update): base(component)
		{
			this.Beta = update.Beta;
			this.FullFilePath = update.FullFilePath;
			this.InstallerPath = update.InstallerPath;
			this.InstallerType = update.InstallerType;
			this.Platforms = update.Platforms;
			this.ReleaseFileId = update.ReleaseFileId;
			this.UpgradeFilePath = update.UpgradeFilePath;
			this.Version = update.Version;
			this.VersionName = update.VersionName;
			this.GitHub = update.GitHub;
			this.ApplicationName = component.ApplicationName;
			this.Component = component.Component;
			this.ComponentCode = component.ComponentCode;
			this.ComponentDescription = component.ComponentDescription;
			this.ComponentName = component.ComponentName;
		}
		public ComponentInfo(ElementInfo raw): base(raw)
		{
			ApplicationName = raw.ApplicationName;
			Component = raw.Component;
			ComponentDescription = raw.ComponentDescription;
			ComponentCode = raw.ComponentCode;
			ComponentName = raw.ComponentName;
		}
		public string ApplicationName { get; set; }
		public string Component { get; set; }
		public string ComponentDescription { get; set; }
		public string ComponentCode { get; set; }
		public string ComponentName { get; set; }
		public bool IsAvailableOnPlatform
		{
			get
			{
				if (Platforms == Platforms.Undefined) Platforms = Platforms.Windows;

				return OSInfo.IsWindows && Platforms.HasFlag(Platforms.Windows) ||
					!OSInfo.IsWindows && Platforms.HasFlag(Platforms.Unix);
			}
		}

		public bool UsesNewInstaller => !InstallerPath.Equals("setup\\setup.dll", StringComparison.OrdinalIgnoreCase);
		public bool IsInstalled
		{
			get
			{
				var componentCode = ComponentCode;
				bool ret = false;
				var installedComponents = new HashSet<string>();
				foreach (var component in Installer.Current.Settings.Installer.InstalledComponents)
				{
					string code = component.ComponentCode;
					installedComponents.Add(code);
					if (code == componentCode)
					{
						ret = true;
						break;
					}
				}
				if (componentCode == "standalone")
				{
					if ((installedComponents.Contains("server") || installedComponents.Contains("serverunix")) &&
						installedComponents.Contains("enterprise server") &&
						installedComponents.Contains("portal"))
						ret = true;
				}
				return ret;
			}
		}
	}

}
