
using SolidCP.Providers.OS;
using System.Diagnostics;

namespace SolidCP.UniversalInstaller
{

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
		private static bool CursorVisibleAfterWaitCursor;
		public static void ShowWaitCursor()
		{
			Console.Clear();
			var write = (string txt) =>
			{
				if (CancelWaitCursor.Token.IsCancellationRequested || File.Exists(CancelFile)) throw new Exception();
				Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
				Console.Write(txt);
				Thread.Sleep(333);
			};
			write("|");
			CursorVisibleAfterWaitCursor = false; // Console.CursorVisible;
			Console.CursorVisible = false;
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
				//if (!Debugger.IsAttached && !OSInfo.IsMono) Debugger.Launch();
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

				Installer.Current.UI.Init();
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
}

