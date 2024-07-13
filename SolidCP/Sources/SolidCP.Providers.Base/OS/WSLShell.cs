using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace SolidCP.Providers.OS
{
	public enum WSLNetworkingMode { NAT, mirrored }
	public class WSLShell : Shell
	{
		private static string ToCamelCase(string name)
		{
			if (name != null && name.Length > 0 && char.IsUpper(name[0])) return char.ToLower(name[0]) + name.Substring(1);
			return name;
		}
		private static string ToTitleCase(string name)
		{
			if (name != null && name.Length > 0 && char.IsLower(name[0])) return char.ToUpper(name[0]) + name.Substring(1);
			return name;
		}

		public class ConfigurationSection : OrderedNameDictionary<string?>
		{
			public ConfigurationSection() : base(StringComparer.OrdinalIgnoreCase) { }
			public bool Exists => base.Count > 0;
			public virtual string Title { get; }
			public bool? ParseBool(string? setting)
			{
				var value = this[setting];
				if (string.IsNullOrEmpty(value)) return null;
				bool boolean;
				if (bool.TryParse(value, out boolean)) return boolean;
				return null;
			}

			public int? ParseInt(string? setting)
			{
				var value = this[setting];
				if (string.IsNullOrEmpty(value)) return null;
				int integer;
				if (int.TryParse(value, out integer)) return integer;
				return null;
			}
			int ntrivia = 0;
			public void AddTrivia(string text) => this[$"\"{ntrivia++}"] = text;
			public void Write(StringBuilder sb, string eol = null)
			{
				eol = eol ?? "\n";
				sb.Append($"[{ToCamelCase(Title)}]{eol}");
				foreach (var item in this)
				{
					if (item.Value != null)
					{
						var key = item.Key;
						if (key.StartsWith("\"")) sb.Append(item.Value);
						else
						{
							sb.Append(ToCamelCase(item.Key));
							sb.Append("=");
							sb.Append(ToCamelCase(item.Value));
						}
						sb.Append(eol);
					}
				}
			}

			public override string ToString()
			{
				var sb = new StringBuilder();
				Write(sb);
				return sb.ToString();
			}
		}

		public class BootSection : ConfigurationSection
		{
			public override string Title => "Boot";

			public bool? Systemd
			{
				get => ParseBool(this[nameof(Systemd)]);
				set => this[nameof(Systemd)] = value?.ToString();
			}
			public string? Command
			{
				get => this[nameof(Command)];
				set => this[nameof(Command)] = value;
			}
		}

		public class Wsl2Section : ConfigurationSection
		{
			public override string Title => "Wsl2";
			public string? Kernel
			{
				get => this[nameof(Kernel)];
				set => this[nameof(Kernel)] = value;
			}
			public string? Memory
			{
				get => this[nameof(Memory)];
				set => this[nameof(Memory)] = value;
			}
			public int? Processors
			{
				get => ParseInt(this[nameof(Processors)]);
				set => this[nameof(Processors)] = value?.ToString();
			}
			public bool? LocalhostForwarding
			{
				get => ParseBool(this[nameof(LocalhostForwarding)]);
				set => this[nameof(LocalhostForwarding)] = value?.ToString();
			}
			public string? KernelCommandLine
			{
				get => this[nameof(KernelCommandLine)];
				set => this[nameof(KernelCommandLine)] = value;
			}
			public bool? SafeMode
			{
				get => ParseBool(this[nameof(SafeMode)]);
				set => this[nameof(SafeMode)] = value?.ToString();
			}
			public string? Swap
			{
				get => this[nameof(Swap)];
				set => this[nameof(Swap)] = value;
			}
			public string? SwapFile
			{
				get => this[nameof(SwapFile)];
				set => this[nameof(SwapFile)] = value;
			}
			public bool? PageReporting
			{
				get => ParseBool(this[nameof(PageReporting)]);
				set => this[nameof(PageReporting)] = value?.ToString();
			}
			public bool? GuiApplications
			{
				get => ParseBool(this[nameof(GuiApplications)]);
				set => this[nameof(GuiApplications)] = value?.ToString();
			}
			public bool? DebugConsole
			{
				get => ParseBool(this[nameof(DebugConsole)]);
				set => this[nameof(DebugConsole)] = value?.ToString();
			}
			public bool? NestedVirtualization
			{
				get => ParseBool(this[nameof(NestedVirtualization)]);
				set => this[nameof(NestedVirtualization)] = value?.ToString();
			}
			public int? VmIdleTimeout
			{
				get => ParseInt(this[nameof(VmIdleTimeout)]);
				set => this[nameof(VmIdleTimeout)] = value?.ToString();
			}
			public bool? DnsProxy
			{
				get => ParseBool(this[nameof(DnsProxy)]);
				set => this[nameof(DnsProxy)] = value?.ToString();
			}
			public WSLNetworkingMode? NetworkingMode
			{
				get
				{
					var setting = this[nameof(NetworkingMode)];
					if (string.IsNullOrEmpty(setting)) return null;
					WSLNetworkingMode mode;
					if (Enum.TryParse<WSLNetworkingMode>(setting, true, out mode)) return mode;
					else return null;
				}
				set => this[nameof(NetworkingMode)] = ToCamelCase(value?.ToString());
			}
			public bool? Firewall
			{
				get => ParseBool(this[nameof(Firewall)]);
				set => this[nameof(Firewall)] = value?.ToString();
			}
			public bool? DnsTunneling
			{
				get => ParseBool(this[nameof(DnsTunneling)]);
				set => this[nameof(DnsTunneling)] = value?.ToString();
			}
			public bool? AutoProxy
			{
				get => ParseBool(this[nameof(AutoProxy)]);
				set => this[nameof(AutoProxy)] = value?.ToString();
			}
		}

		public class ExperimentalSection : ConfigurationSection
		{
			public override string Title => "Experimental";
			public string? AutoMemoryReclaim
			{
				get => this[nameof(AutoMemoryReclaim)];
				set => this[nameof(AutoMemoryReclaim)] = value;
			}
			public bool? SparseVhd
			{
				get => ParseBool(this[nameof(SparseVhd)]);
				set => this[nameof(SparseVhd)] = value?.ToString();
			}
			public bool? UseWindowsDnsCache
			{
				get => ParseBool(this[nameof(UseWindowsDnsCache)]);
				set => this[nameof(UseWindowsDnsCache)] = value?.ToString();
			}
			public bool? BestEffortDnsParsing
			{
				get => ParseBool(this[nameof(BestEffortDnsParsing)]);
				set => this[nameof(BestEffortDnsParsing)] = value?.ToString();
			}
			public int? InitialAutoProxyTimeout
			{
				get => ParseInt(this[nameof(InitialAutoProxyTimeout)]);
				set => this[nameof(InitialAutoProxyTimeout)] = value?.ToString();
			}
			public string? IgnorePorts
			{
				get => this[nameof(IgnorePorts)];
				set => this[nameof(IgnorePorts)] = value;
			}
			public bool? HostAddressLoopback
			{
				get => ParseBool(this[nameof(HostAddressLoopback)]);
				set => this[nameof(HostAddressLoopback)] = value?.ToString();
			}
		}
		public class AutomountSection : ConfigurationSection
		{
			public override string Title => "Automount";

			public bool? Enabled
			{
				get => ParseBool(this[nameof(Enabled)]);
				set => this[nameof(Enabled)] = value?.ToString();
			}
			public bool? MountFsTab
			{
				get => ParseBool(this[nameof(MountFsTab)]);
				set => this[nameof(MountFsTab)] = value?.ToString();
			}
			public string? Root
			{
				get => this[nameof(Root)];
				set => this[nameof(Root)] = value;
			}
			public string? Options
			{
				get => this[nameof(Options)];
				set => this[nameof(Options)] = value;
			}
		}

		public class NetworkSection : ConfigurationSection
		{
			public override string Title => "Network";

			public bool? GenerateHosts
			{
				get => ParseBool(this[nameof(GenerateHosts)]);
				set => this[nameof(GenerateHosts)] = value?.ToString();
			}
			public bool? GenerateResolvConf
			{
				get => ParseBool(this[nameof(GenerateResolvConf)]);
				set => this[nameof(GenerateResolvConf)] = value?.ToString();
			}

			public string? Hostname
			{
				get => this[nameof(Hostname)];
				set => this[nameof(Hostname)] = value;
			}
		}

		public class InteropSection : ConfigurationSection
		{
			public override string Title => "Interop";

			public bool? Enabled
			{
				get => ParseBool(this[nameof(Enabled)]);
				set => this[nameof(Enabled)] = value?.ToString();
			}
			public bool? AppendWindowsPath
			{
				get => ParseBool(this[nameof(AppendWindowsPath)]);
				set => this[nameof(AppendWindowsPath)] = value?.ToString();
			}
		}

		public class UserSection : ConfigurationSection
		{
			public override string Title => "User";
			public string? Default
			{
				get => this[nameof(Default)];
				set => this[nameof(Default)] = value;
			}
		}

		public class ConfigurationBase : OrderedNameDictionary<ConfigurationSection>
		{
			public virtual string File => null;
			string leadingTrivia;
			public WSLShell Shell { get; protected set; }

			public ConfigurationBase() { }

			public ConfigurationBase(WSLShell shell)
			{
				Shell = shell;
				Open();
			}

			protected virtual bool IsWslFile => !File.Contains('\\');

			public void Open()
			{
				var configTxt = Shell.ReadTextFile(File);
				var match = Regex.Match(configTxt, @"(?<trivia>^.*?)(?<body>(?<=^|\n)\[[^]\n]\].*$)", RegexOptions.Singleline);
				leadingTrivia = match.Groups["trivia"].Value;
				var sections = match.Groups["body"].Value;
				var reader = new StringReader(sections);
				var line = reader.ReadLine();
				ConfigurationSection section = null;
				while (line != null)
				{
					line = line.Trim();

					if (line.StartsWith("["))
					{
						var title = Regex.Match(line, @"(?<=^\[).*?(?=\]|$)", RegexOptions.Singleline).Value;
						section = this[title];
					}
					else if (section == null) throw new ArgumentException("Not supported");
					else if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line) || !line.Contains("="))
					{
						section.AddTrivia(line);
					}
					else
					{
						var tokens = line.Split('=').Select(token => token.Trim()).ToArray();
						if (tokens.Length != 2) section.AddTrivia(line);
						else
						{
							section[tokens[0]] = tokens[1];
						}
					}
					line = reader.ReadLine();
				}
			}

			public void Save()
			{
				var sb = new StringBuilder();
				sb.Append(leadingTrivia);
				foreach (var section in this)
				{
					section.Value.Write(sb);
				}
				if (IsWslFile) Shell.WriteTextFile(sb.ToString(), File);
			}
			public override ConfigurationSection this[string section]
			{
				get
				{
					section = ToTitleCase(section);
					var config = base[section];
					if (section == null)
					{
						var sectionType = Type.GetType($"SolidCP.Providers.OS.WSLShell.{section}Section, SolidCP.Providers.Base");
						if (sectionType != null) config = Activator.CreateInstance(sectionType) as ConfigurationSection;
						else config = new ConfigurationSection();
						base[section] = config;
					}
					return config;
				}
				set => base[ToTitleCase(section)] = value;
			}
		}

		public class WSLConfiguration : ConfigurationBase
		{
			public WSLConfiguration(WSLShell shell) : base(shell) { }
			public override string File => "/etc/wsl.conf";
			protected override bool IsWslFile => true;
			public BootSection Boot => (BootSection)this[nameof(Boot)];
			public AutomountSection Automount => (AutomountSection)this[nameof(Automount)];
			public NetworkSection Network => (NetworkSection)this[nameof(Network)];
			public InteropSection Interop => (InteropSection)this[nameof(Interop)];
			public UserSection User => (UserSection)this[nameof(User)];
		}

		public class WSLGlobalConfiguration : ConfigurationBase
		{
			public WSLGlobalConfiguration(WSLShell shell) : base(shell) { }
			public WSLGlobalConfiguration(WSLShell shell, string user) { User = user; Shell = shell; Open(); }
			public virtual string User { get; set; } = null;
			public override string File => User == null ?
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".wslconfig") :
				Path.Combine(Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)), User, ".wslconfig");
			protected override bool IsWslFile => false;
			public Wsl2Section Wsl2 => (Wsl2Section)this[nameof(Wsl2)];
			public ExperimentalSection Experimental => (ExperimentalSection)this[nameof(Experimental)];
		}

		public class WSLDistro
		{
			public Distro Distro { get; set; }
			public string OtherDistroName { get; set; }

			public static implicit operator string(WSLDistro distro) => GetDistroName(distro) ?? distro.OtherDistroName;
			public static implicit operator Distro(WSLDistro distro) => distro.Distro;
			public static implicit operator WSLDistro(string distro)
			{
				var wslDistro = new WSLDistro() { OtherDistroName = null };
				for (Distro d = Distro.Default; d <= Distro.Other; d++)
				{
					wslDistro.Distro = d;
					if (GetDistroName(wslDistro) == distro) return new WSLDistro() { Distro = d, OtherDistroName = null };
				}
				return new WSLDistro() { Distro = Distro.Other, OtherDistroName = distro };
			}
			public static implicit operator WSLDistro(Distro distro) => new WSLDistro() { Distro = distro, OtherDistroName = null };
			public override string ToString() => GetDistroName(this) ?? OtherDistroName;
		}
		public bool Debug { get; set; }
		public enum Distro { Default, Ubuntu, Debian, Kali, Ubuntu18, Ubuntu20, Ubuntu22, Ubuntu24, Oracle7, Oracle8, Oracle9, openSUSELeap, SUSE15_4, SUSE15_5, openSUSEThumbleweed, FedoraRemix, Native, Other };
		public override string ShellExe => IsWindows ?
			(CurrentDistro == Distro.Default ? "wsl" : $"wsl --distribution {CurrentDistroName}") :
			"bash";

		public WSLShell() : base() { BaseShell = Shell.Default; }
		public WSLShell(WSLDistro distro) : this() => Use(distro);

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
		public static string GetDistroName(WSLDistro distro)
		{
			if (!IsWindows) return "unix";

			switch (distro.Distro)
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
				case Distro.Native: return "unix";
				case Distro.Other: return distro.OtherDistroName;
			}
		}
		protected string DistroName(WSLDistro distro) => GetDistroName(distro);
		public WSLDistro CurrentDistro { get; set; } = IsWindows ? Distro.Default : Distro.Native;
		public string CurrentDistroName => DistroName(CurrentDistro);
		public void Use(WSLDistro distro) => CurrentDistro = distro;
		public WSLShell For(WSLDistro distro)
		{
			var clone = (WSLShell)Clone;
			clone.CurrentDistro = distro;
			return clone;
		}
		protected string WSLList => IsWindows ? BaseShell.SilentClone.Exec($"wsl --list --verbose", Encoding.Unicode).Output().Result : "";
		public WSLDistro[] InstalledDistros => IsWindows ? base.Find("wsl") == null ? new WSLDistro[0] : Regex.Matches(WSLList, @"(?<=\n\*?\s+)[^\s]+")
			.OfType<Match>()
			.Select(match => (WSLDistro)match.Value)
			.ToArray() :
			new[] { (WSLDistro)"unix" };
		public bool IsWslInstalled => IsWindows && base.Find("wsl") != null;
		public WSLDistro DefaultDistro => IsWindows ?
			(!IsWslInstalled ? null : Regex.Match(WSLList, @"(?<=\n\*\s+)[^\s]+").Value) :
			"unix";
		public bool IsInstalled(WSLDistro distro) => !IsWindows || Regex.IsMatch(WSLList, $@"^\*?\s+{Regex.Escape(distro)}\s", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		public bool IsInstalledAny() => !IsWindows || IsWslInstalled && InstalledDistros.Length > 0;
		public bool IsInstalled() => !IsWindows || IsInstalled(CurrentDistroName);
		public void UpdateWsl() => BaseShell.Exec("wsl --update");
		public void ShutdownAll()
		{
			if (IsWindows) base.Exec("wsl --shutdown");
		}
		public void Terminate(WSLDistro distro)
		{
			if (IsWindows) base.Exec($"wsl --terminate {distro}");
		}
		public void Install(WSLDistro distro)
		{
			if (IsWindows) base.Exec($"wsl --install {distro}");
		}
		public void Uninstall(WSLDistro distro)
		{
			if (IsWindows) base.Exec($"wsl --unregister {distro}");
		}

		public void Import(WSLDistro distro, string file)
		{
			if (IsWindows) base.Exec($"wsl --import {distro} {file}{(file.EndsWith(".vhdx", StringComparison.OrdinalIgnoreCase) ? " --vhd" : "")}");
		}
		public void Export(WSLDistro distro, string file)
		{
			if (IsWindows) base.Exec($"wsl --export {distro} {file}{(file.EndsWith(".vhdx", StringComparison.OrdinalIgnoreCase) ? " --vhd" : "")}");
		}
		public string ReadTextFile(string path)
		{
			if (IsWindows) return Exec($"cat {path}").Output().Result;
			else return File.ReadAllText(path);
		}
		public void WriteTextFile(string content, string path)
		{
			if (IsWindows)
			{
				var shell = ExecAsync($"cat > {path}");
				shell.StandardInput.Write(content);
				shell.StandardInput.Close();
				if (!shell.IsCompleted) shell.Task().Wait();
			}
			else File.WriteAllText(path, content);
		}

		WSLConfiguration wslConfiguration = null;
		WSLGlobalConfiguration wslGlobalConfiguration = null;
		public WSLConfiguration Configuration
		{
			get => IsWindows ?
				wslConfiguration ?? (wslConfiguration = new WSLConfiguration(this)) :
				throw new PlatformNotSupportedException("Configuration is only available on Windows");
			private set
			{
				if (IsWindows) wslConfiguration = value;
				else throw new PlatformNotSupportedException("Configuration is only available on Windows");
			}
		}
		public WSLGlobalConfiguration GlobalConfiguration => IsWindows ?
			wslGlobalConfiguration ?? (wslGlobalConfiguration = new WSLGlobalConfiguration(this)) :
			throw new PlatformNotSupportedException("GlobalConfiguration is only available on Windows");
		public WSLGlobalConfiguration GlobalConfigurationFor(string user) => IsWindows ? new WSLGlobalConfiguration(this, user) :
			throw new PlatformNotSupportedException("GlobalConfiguration is only available on Windows");
		public string WSLPath(string path) => IsWindows ? Regex.Replace(Path.GetFullPath(path), "^(?<drive>[A-Z]):",
			match => $"/mnt/{match.Groups["drive"].Value.ToLower()}", RegexOptions.IgnoreCase | RegexOptions.Singleline)
			.Replace(Path.DirectorySeparatorChar, '/') :
			path;

		protected override string ToTempFile(string script)
		{
			script = script.Replace(Environment.NewLine, "\n");
			var localTmp = base.ToTempFile(script);
			return WSLPath(localTmp);
		}

		public override Shell ExecAsync(string command, Encoding encoding = null, StringDictionary environmentVariables = null)
		{
			LogCommand?.Invoke(command);

			if (IsWindows)
			{
				return BaseShell.ExecAsync($"{ShellExe} {command}", encoding, environmentVariables);
			}
			else // System is already unix, do not use WSL
			{
				return BaseShell.ExecAsync(command, encoding, environmentVariables);
			}
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
				clone.Redirect = Redirect;
				clone.LogFile = LogFile;
				clone.Debug = Debug;
				return clone;
			}
		}
		public override Shell SilentClone
		{
			get
			{
				var clone = (WSLShell)base.SilentClone;
				clone.BaseShell = BaseShell.SilentClone;
				clone.Redirect = Debug;
				return clone;
			}
		}
		public override string Find(string cmd)
		{
			if (IsWindows)
			{
				var shell = (WSLShell)SilentClone;

				if (!shell.IsInstalled()) return null;

				var output = shell.Exec($"which {cmd}").Output().Result.Trim();
				if (string.IsNullOrEmpty(output)) return null;

				return output;
			}
			else return base.Find(cmd);

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

#if wpkg
		public static new bool IsWindows => RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
#else
		public static new bool IsWindows => OSInfo.IsWindows;
#endif

	}
}
