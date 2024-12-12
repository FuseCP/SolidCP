
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
				Installer.Current.RunMain();
				Installer.Current.UI.Init();
				Installer.Current.UI.PrintInstallerVersion();
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		}
	}
}

