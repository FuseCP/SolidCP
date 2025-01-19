
using SolidCP.Providers.OS;
using System.Diagnostics;

namespace SolidCP.UniversalInstaller
{

	public class Program
	{
		static AssemblyLoader Loader = null;
		static Program()
		{
			Loader = AssemblyLoader.Init();
		}

		[STAThread]
		public static void Main(string[] args)
		{
			try
			{
				//if (!Debugger.IsAttached && !OSInfo.IsMono) Debugger.Launch();
				StartMain(args);
			} catch { }
		}

		public static void StartMain(string[] args)
		{
			try
			{
				if (args.Any(arg => arg.Equals("-ui=winforms", StringComparison.OrdinalIgnoreCase))) UI.Current = UI.WinFormsUI;
				else if (args.Any(arg => arg.Equals("-ui=avalonia", StringComparison.OrdinalIgnoreCase))) UI.Current = UI.AvaloniaUI;
				else if (args.Any(arg => arg.Equals("-ui=console", StringComparison.OrdinalIgnoreCase))) UI.Current = UI.ConsoleUI;

				Installer.Current.UI.Init();
				Installer.Current.OnExit += Loader.Unload;
				Installer.Current.UI.PrintInstallerVersion();
				Installer.Current.StartMain();
			}
			catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
		}
	}
}

