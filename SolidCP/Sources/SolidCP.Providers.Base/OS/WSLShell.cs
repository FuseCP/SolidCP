using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolidCP.Providers.OS
{
	public class WSLShell : Shell
	{
		public enum Distro { Default, Ubuntu, Debian, Kali, Ubuntu18, Ubuntu20, Ubuntu22, Ubuntu24, Oracle7, Oracle8, Oracle9, openSUSELeap, SUSE15_4, SUSE15_5, openSUSEThumbleweed, FedoraRemix, Other };
		public override string ShellExe => CurrentDistro == Distro.Default ? "wsl" : $"wsl --distribution {CurrentDistroName}";

		public WSLShell() : base() { BaseShell = Shell.Default; }
		public WSLShell(string distro) : this() => Use(distro);
		public WSLShell(Distro distro) : this() => Use(distro);

		Shell baseShell = null;

		public Shell BaseShell
		{
			get => baseShell;
			set
			{
				if (baseShell != value)
				{
					if (baseShell != null)
					{
						baseShell.Log -= OnBaseLog;
						baseShell.LogCommandEnd -= OnBaseLogCommandEnd;
						baseShell.LogOutput -= OnBaseLogOutput;
						baseShell.LogError -= OnBaseLogError;
					}
					baseShell = value;
					if (value != null)
					{
						value.Log += OnBaseLog;
						value.LogCommandEnd += OnBaseLogCommandEnd;
						value.LogOutput += OnBaseLogOutput;
						value.LogError += OnBaseLogError;
					}
				}
			}
		}
		protected string DistroName(Distro distro)
		{
			switch (distro)
			{
				default:
				case Distro.Default: return "wsl";
				case Distro.Ubuntu: return "Ubuntu";
				case Distro.Debian: return "Debian";
				case Distro.Kali: return "kali-linux";
				case Distro.Ubuntu18: return "Ubuntu-18.04";
				case Distro.Ubuntu20: return "Ubuntu-20.04";
				case Distro.Ubuntu22: return "Ubuntu-22.04";
				case Distro.Ubuntu24: return "Ubuntu-24.04";
				case Distro.Oracle7: return "OracleLinux_7_9";
				case Distro.Oracle8: return "OracleLinux_8_7";
				case Distro.Oracle9: return "OracleLinux_9_1";
				case Distro.openSUSELeap: return "openSUSE-Leap-15.5";
				case Distro.SUSE15_4: return "SUSE-Linux-Enterprise-Server-15-SP4";
				case Distro.SUSE15_5: return "SUSE-Linux-Enterprise-15-SP5";
				case Distro.openSUSEThumbleweed: return "openSUSE-Tumbleweed";
				case Distro.FedoraRemix: return "fedoraremix";
				case Distro.Other: return OtherDistroName;
			}
		}

		public string OtherDistroName = null;

		public Distro CurrentDistro = Distro.Default;
		public string CurrentDistroName => DistroName(CurrentDistro);
		public void Use(Distro distro) => CurrentDistro = distro;
		public void Use(string distro)
		{
			CurrentDistro = Distro.Other;
			OtherDistroName = distro;
		}

		protected string WSLList => BaseShell.SilentClone.Exec($"wsl --list --verbose", Encoding.Unicode).Output().Result;
		public string[] InstalledDistros => base.Find("wsl") == null ? new string[0] : Regex.Matches(WSLList, @"(?<=\n\*?\s+)[^\s]+")
			.OfType<Match>()
			.Select(match => match.Value)
			.ToArray();
		public string DefaultDistro => base.Find("wsl") == null ? null : Regex.Match(WSLList, @"(?<=\n\*\s+)[^\s]+").Value;
		public bool IsInstalled(Distro distro) => IsInstalled(DistroName(distro));
		public bool IsInstalled(string distroName) => Regex.IsMatch(WSLList, $@"^\*?\s+{Regex.Escape(distroName)}\s", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		public bool IsInstalledAny() => base.Find("wsl") != null && InstalledDistros.Length > 0;
		public bool IsInstalled() => IsInstalled(CurrentDistroName);

		public string WSLPath(string path) => Regex.Replace(Path.GetFullPath(path), "^(?<drive>[A-Z]):",
			match => $"/mnt/{match.Groups["drive"].Value.ToLower()}", RegexOptions.IgnoreCase | RegexOptions.Singleline)
			.Replace(Path.DirectorySeparatorChar, '/');

		protected override string ToTempFile(string script)
		{
			script = script.Replace(Environment.NewLine, "\n");
			var localTmp = base.ToTempFile(script);
			return WSLPath(localTmp);
		}

		public override Shell ExecAsync(string command, Encoding encoding = null, StringDictionary environmentVariables = null)
		{
			LogCommand?.Invoke(command);

			return BaseShell.ExecAsync($"{ShellExe} {command}", encoding, environmentVariables);
		}
		public override Shell ExecScriptAsync(string script, Encoding encoding = null, StringDictionary environmentVariables = null)
		{
			LogCommand?.Invoke($"bash {script}");

			script = script.Trim();
			// adjust new lines to OS type
			script = Regex.Replace(script, @"\r?\n", Environment.NewLine);
			var file = ToTempFile(script.Trim());
			var shell = BaseShell.ExecAsync($"{ShellExe} \"{file}\"", encoding, environmentVariables);
			if (shell.Process != null)
			{
				shell.Process.Exited += (sender, args) =>
				{
					file = Regex.Replace(file, "^/mnt/(?<drive>[a-zA-Z])/", m => m.Groups["drive"].Value.ToUpper() + ":\\")
						.Replace('/', Path.DirectorySeparatorChar);
					File.Delete(file);
				};
			}
			return shell;
		}

		public override Shell Clone
		{
			get
			{
				var clone = (WSLShell)base.Clone;
				clone.CurrentDistro = CurrentDistro;
				clone.OtherDistroName = OtherDistroName;
				clone.Redirect = Redirect;
				clone.LogFile = LogFile;
				return clone;
			}
		}
		public override Shell SilentClone
		{
			get
			{
				var clone = (WSLShell)base.SilentClone;
				clone.BaseShell = BaseShell.SilentClone;
				clone.Redirect = false;
				return clone;
			}
		}
		public override string Find(string cmd)
		{
			var shell = (WSLShell)SilentClone;

			if (!shell.IsInstalled()) return null;

			var output = shell.Exec($"which {cmd}").Output().Result.Trim();
			if (string.IsNullOrEmpty(output)) return null;

			return output;
		}

		protected override void OnLogCommand(string text)
		{
			text = $"{CurrentDistroName}> {text}";
			if (Redirect) Console.WriteLine(text);
			if (LogFile != null) File.AppendAllText(LogFile, text);
		}
		protected void OnBaseLog(string msg) => Log?.Invoke(msg);
		protected void OnBaseLogCommandEnd() => LogCommandEnd?.Invoke();
		protected void OnBaseLogOutput(string msg) => LogOutput?.Invoke(msg);
		protected void OnBaseLogError(string msg) => LogError?.Invoke(msg);

		public new readonly static WSLShell Default = new WSLShell();
	}
}
