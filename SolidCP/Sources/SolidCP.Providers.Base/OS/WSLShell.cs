using System;
using System.Collections.Generic;
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
		public override string ShellExe => CurrentDistro == Distro.Default ? "wsl" : $"wsl --distribution {DistroName(CurrentDistro)}";

		public WSLShell() { }
		public WSLShell(string distro) => Use(distro);
		public WSLShell(Distro distro) => Use(distro);
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
		public void Use(Distro distro) => CurrentDistro = distro;
		public void Use(string distro)
		{
			CurrentDistro = Distro.Other;
			OtherDistroName = distro;
		}

		public string[] InstalledDistros => base.Find("wsl") == null ? new string[0] : Regex.Matches(base.Exec($"wsl --list --verbose").Output().Result, @"(?<=\n\*?\s*)[^\s]+")
			.OfType<Match>()
			.Select(match => match.Value)
			.ToArray();
		public string DefaultDistro => base.Find("wsl") == null ? null : Regex.Match(base.Exec($"wsl --list --verbose").Output().Result, @"(?<=\n\*\s*)[^\s]+").Value;
		public bool IsInstalled(Distro distro) => IsInstalled(DistroName(distro));
		public bool IsInstalled(string distroName) => Regex.IsMatch(base.Exec($"wsl --list --verbose").Output().Result, $@"^\*?\s*{Regex.Escape(distroName)}\s", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		public bool IsInstalledAny() => base.Find("wsl") != null && InstalledDistros.Length > 0;
		public bool IsInstalled() => IsInstalled(DistroName(CurrentDistro));

		public string WSLPath(string path) => Regex.Replace(Path.GetFullPath(path), "^(?<drive>[A-Z]):",
			match => $"/mnt/{match.Groups["drive"].Value.ToLower()}", RegexOptions.IgnoreCase | RegexOptions.Singleline)
			.Replace(Path.DirectorySeparatorChar, '/');

		protected override string ToTempFile(string script)
		{
			script = script.Replace(Environment.NewLine, "\n");
			var localTmp = base.ToTempFile(script);
			return WSLPath(localTmp);
		}

		public override Shell ExecAsync(string command)
		{
			return base.ExecAsync($"{ShellExe} {command}");
		}
		public override Shell ExecScriptAsync(string script)
		{
			script = script.Trim();
			// adjust new lines to OS type
			script = Regex.Replace(script, @"\r?\n", Environment.NewLine);
			var file = ToTempFile(script.Trim());
			var shell = base.ExecAsync($"{ShellExe} \"{file}\"");
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

		public override string Find(string cmd)
		{
			if (!IsInstalled()) return null;

			var output = Exec($"which {cmd}").Output().Result.Trim();
			if (string.IsNullOrEmpty(output)) return null;
			
			return output;
		}

		public new readonly static WSLShell Default = new WSLShell();
	}
}
