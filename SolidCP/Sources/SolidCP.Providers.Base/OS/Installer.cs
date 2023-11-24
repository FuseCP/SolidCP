using System;
using System.Linq;
using System.Diagnostics;

namespace SolidCP.Providers.OS
{
    public abstract class Installer
    {
        public virtual Shell Shell { get; set; } = OSInfo.Shell;
        public abstract Shell Install(string apps);
        public abstract void AddSources(string sources);
       	public abstract bool IsInstallerInstalled { get; }
        public abstract bool IsInstalled(string apps);
	    public void CheckInstallerInstalled() {
            if (!IsInstallerInstalled) throw new PlatformNotSupportedException($"The installer type {this.GetType().Name} is not installed on this system.");
        }

        public static Installer Default => OSInfo.Current.DefaultInstaller;

    }

}