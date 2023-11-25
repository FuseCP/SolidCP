
using SolidCP.UniversalInstaller;
using System.Diagnostics;

namespace SolidCP.UniversalInstaller
{

	public class Program
	{
		public static void Main(string[] args)
		{
			if (!Debugger.IsAttached) Debugger.Launch();
			ResourceAssemblyLoader.Init();
			Installer.Current.UI.PrintInstallerVersion();
			Installer.Current.InstallAll();
		}
	}
}

