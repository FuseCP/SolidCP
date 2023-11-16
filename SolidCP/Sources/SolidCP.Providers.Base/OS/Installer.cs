using System;
using System.Linq;
using System.Diagnostics;

namespace SolidCP.Providers.OS
{
    public abstract class Installer
    {
        public virtual Shell Shell { get; set; } = OSInfo.Shell;
        public abstract Shell InstallAsync(string apps);
        public abstract void AddSources(string sources);
       	public abstract bool IsInstalled { get; }
	    protected void CheckInstalled() {
            if (!IsInstalled) throw new NotSupportedException($"The installer type {this.GetType().Name} is not installed on this system.");
        }

        public static Installer Default => OSInfo.Current.DefaultInstaller;

    }

}