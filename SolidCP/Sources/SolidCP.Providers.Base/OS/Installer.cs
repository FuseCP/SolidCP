using System;
using System.Linq;
using System.Diagnostics;

namespace SolidCP.Providers.OS
{
	public abstract class Installer
	{
		public virtual Shell Shell { get; set; } = OSInfo.Current.DefaultShell;
		public virtual Shell Install(string apps) => InstallAsync(apps).Task().Result;
		public abstract Shell InstallAsync(string apps);
		public virtual Shell AddSources(string sources) => AddSourcesAsync(sources).Task().Result;
		public abstract Shell AddSourcesAsync(string sources);
		public abstract bool IsInstallerInstalled { get; }
		public abstract bool IsInstalled(string apps);
		public void CheckInstallerInstalled()
		{
			if (!IsInstallerInstalled) throw new PlatformNotSupportedException($"The installer type {this.GetType().Name} is not installed on this system.");
		}
		public virtual Shell Remove(string apps) => RemoveAsync(apps).Task().Result;
		public abstract Shell RemoveAsync(string apps);
		public virtual Shell Update() => UpdateAsync().Task().Result;
		public abstract Shell UpdateAsync();
		public static Installer Default => OSInfo.Current.DefaultInstaller;

	}

}