
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
				StartMain();
			} catch { }
		}

		public static void StartMain() {
			try
			{
				Installer.Current.UI.Init();
				Installer.Current.UI.PrintInstallerVersion();
				Installer.Current.StartMain();
			}
			catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		}
	}
}

