
using SolidCP.Providers.OS;
using System.Diagnostics;

namespace SolidCP.UniversalInstaller
{

	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				//if (!Debugger.IsAttached && !OSInfo.IsMono) Debugger.Launch();
				ResourceAssemblyLoader.Init();
				SetupCore();
			} catch { }
		}

		public static void SetupCore() {
			try
			{
				Installer.Current.UI.Init();
				Installer.Current.UI.PrintInstallerVersion();
				Installer.Current.InstallAll();
			} catch (Exception ex) {
			}
		}
	}
}

