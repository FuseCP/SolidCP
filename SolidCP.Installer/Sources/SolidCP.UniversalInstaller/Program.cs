
using SolidCP.Providers.OS;
using System.Diagnostics;

namespace SolidCP.UniversalInstaller
{

	public class Program
	{
		public static void Main(string[] args)
		{
			//if (!Debugger.IsAttached && !OSInfo.IsMono) Debugger.Launch();
			ResourceAssemblyLoader.Init();
			Installer.Current.UI.PrintInstallerVersion();
			Installer.Current.InstallAll();
		}
	}
}

