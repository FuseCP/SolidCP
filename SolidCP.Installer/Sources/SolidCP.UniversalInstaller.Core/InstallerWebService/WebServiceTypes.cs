using SolidCP.Providers.OS;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{

	[Flags]
	public enum Platforms { Undefined = 0, None = 0, Windows = 1, Unix = 2, All = 3 };

	public class ElementInfo
	{
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
	}

	public class ComponentUpdateInfo: ReleaseFileInfo
	{
		public ComponentUpdateInfo() { }
		public ComponentUpdateInfo(ElementInfo raw): base(raw)
		{
			Version version;
			if (Version.TryParse(raw.Version, out version)) Version = version;
			else Version = default;
			Beta = raw.Beta;
		}
		public Version Version { get; set; }
		public bool Beta { get; set; }
	}
	public class ComponentInfo : ComponentUpdateInfo
	{
		public ComponentInfo(): base() { }
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
