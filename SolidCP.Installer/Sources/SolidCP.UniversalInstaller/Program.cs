
using System;
using System.Text.RegularExpressions;
using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller;
 
public class Program
{
	static AssemblyLoader Loader = null;
	static Program()
	{
		ShowWaitCursor();

		Loader = AssemblyLoader.Init();
	}

	public static void ImportDlls()
	{
#if NETCOREAPP
		// Import Mono.Posix.NETStandard
		var dummy = Mono.Unix.Native.FilePermissions.ACCESSPERMS;
#endif
		// Import SolidCP.Providers.Base
		SolidCP.Providers.Common.BoolResult dummy2 = null;
	}

	const string CancelFileName = "WaitCursor.cancel";
	static string CancelFile => Path.Combine(Environment.CurrentDirectory, CancelFileName);

	public static CancellationTokenSource CancelWaitCursor = new CancellationTokenSource();
	public static void ShowWaitCursor()
	{
		Console.CursorVisible = false;
		Console.Clear();
		var write = (string txt) =>
		{
			if (CancelWaitCursor.Token.IsCancellationRequested || File.Exists(CancelFile)) throw new Exception();
			Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
			Console.Write(txt);
			Thread.Sleep(333);
		};
		write("|");
		Task.Run(() =>
		{
			try
			{
				while (true)
				{
					write("/");
					write("-");
					write("\\");
					write("|");
				}
			}
			catch
			{
				CancelWaitCursor = new CancellationTokenSource();
				if (File.Exists(CancelFile)) File.Delete(CancelFile);
			}
		});
	}

	public static void EndWaitCursor()
	{
		File.WriteAllText(CancelFile, "");
		CancelWaitCursor.Cancel();
	}


	[STAThread]
	public static void Main(string[] args)
	{
		try
		{
			ImportDlls();
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

			var unattendedInstallPackages = args.Select(arg => Regex.Match(arg, @"(?<=-unattended=).*$"))
				.Where(match => match.Success)
				.Select(match => match.Value)
				.FirstOrDefault();
			var unattendedAction = args.Select(arg => Regex.Match(arg, @"(?<=-action=).*$"))
				.Where(match => match.Success)
				.Select(match => match.Value)
				.FirstOrDefault()
				?? "Install";

			if (unattendedInstallPackages != null)
			{
				Installer.Current.Settings.Installer.UnattendedInstallPackages = unattendedInstallPackages;
				SetupActions action;
				if (!Enum.TryParse<SetupActions>(unattendedAction, out action)) action = SetupActions.Install;
				Installer.Current.Settings.Installer.Action = action;
			}

			Installer.Current.UI.Init();

			if (args.Any(arg => arg.Equals("-update", StringComparison.OrdinalIgnoreCase)))
			{
				Installer.Current.UI.DownloadInstallerUpdate();
				return;
			}

			Installer.Current.OnExit += Loader.Unload;

			EndWaitCursor();
			Console.Clear();

			Installer.Current.UI.PrintInstallerVersion();
			Installer.Current.StartMain();
		}
		catch (Exception ex) {
			Console.WriteLine(ex.ToString());
		}
	}
}

