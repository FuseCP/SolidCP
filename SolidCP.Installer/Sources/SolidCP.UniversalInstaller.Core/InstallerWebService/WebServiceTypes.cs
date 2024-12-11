using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.UniversalInstaller
{

	[Flags]
	public enum Platforms { Windows = 1, Unix = 2 };

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
			Version = raw.Version;
			Beta = raw.Beta;
		}
		public string Version { get; set; }
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
	}

}
