using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace SolidCP.Providers.OS
{

	public class SystemdServiceController : ServiceController
	{
		public virtual string ServicesDirectory => "/lib/systemd/system";

		public override IEnumerable<OSService> All()
		{
			var text = Shell.Exec("systemctl --full --type=service --no-pager --all").Output().Result;
			var matches = Regex.Matches(text, @"^.?\s+(?<name>.*?).service\s+(?<loaded>[^\s$]+)\s+(?<active>[^\s$]+)\s+(?<state>[^\s$]+)\s+(?<desc>.*?)$", RegexOptions.Multiline);
			foreach (Match match in matches)
			{
				var name = Path.GetFileNameWithoutExtension(match.Groups["name"].Value);
				var desc = match.Groups["desc"].Value;
				var loaded = match.Groups["loaded"].Value;
				var active = match.Groups["active"].Value;
				var state = match.Groups["state"].Value;

				var srvc = new OSService()
				{
					Name = desc,
					Id = name,
					Description = desc,
					CanStop = loaded == "loaded" && active == "active" && state == "running",
					CanPauseAndContinue = loaded == "loaded" && active == "active" && state == "running",
					Status = state == "dead" || state == "exited" ? OSServiceStatus.Stopped :
						state == "running" ? OSServiceStatus.Running : OSServiceStatus.Stopped
				};
				yield return srvc;
			}
		}
		public override OSService Info(string serviceId)
		{
			var output = Shell.Exec($"systemctl status {serviceId}.service --no-pager --full").Output().Result;
			if (output == null) return null;

			var match = Regex.Match(output, @"^\s*Loaded:\s*(?<loaded>[^\s$]+).*?$(^.*$\r?\n)*\s*Active:\s*(?<active>[^\s$]+)\s+\((?<status>[^\)]+)\)", RegexOptions.Multiline);

			string loaded, active, status = null;
			if (match.Success)
			{
				loaded = match.Groups["loaded"].Value;
				active = match.Groups["active"].Value;
				status = match.Groups["status"].Value;
			}
			return new OSService()
			{
				Id = serviceId,
				Name = serviceId,
				Description = "",
				Status = status == "running" ? OSServiceStatus.Running : OSServiceStatus.Stopped
			};
		}
		public override void ChangeStatus(string serviceId, OSServiceStatus status)
		{
			var state = Info(serviceId);

			if (state == null) return;

			if (state.Status == OSServiceStatus.Running)
			{
				if (status == OSServiceStatus.PausePending || status == OSServiceStatus.Paused ||
					status == OSServiceStatus.Stopped || status == OSServiceStatus.StopPending)
				{
					var shell = Shell.Exec($"systemctl stop {serviceId}.service").Task().Result;
				}
			}
			else
			{
				if (status == OSServiceStatus.StartPending || status == OSServiceStatus.Running ||
					status == OSServiceStatus.ContinuePending)
				{
					var shell = Shell.Exec($"systemctl start {serviceId}.service").Task().Result;
				}
			}
		}
		public override void Restart(string serviceId) => Shell.Exec($"systemctl restart {serviceId}");
		public override void Reload(string serviceId) => Shell.Exec($"systemctl reload {serviceId}");
		public override void Disable(string serviceId)
		{
			Shell.Exec($"systemctl disable {serviceId}.service");
		}
		public override void Enable(string serviceId)
		{
			Shell.Exec($"systemctl enable {serviceId}.service");
		}

		public override void SystemReboot()
		{
			Shell.Exec("systemctl reboot");
		}

		public override ServiceManager Install(ServiceDescription description)
		{
			var desc = description as SystemdServiceDescription;
			if (desc == null) throw new ArgumentException("Service description is not a SystemdServiceDescription.");

			var srvcFile = @"[Unit]
Description=@description
@dependsOn
@StartLimitIntervalSec
@StartLimitBurst

[Service]
Type=simple
ExecStart=@exec
WorkingDirectory=@workdir
@environment
@Restart_
@RestartSec
@Syslog
@User
@Group

[Install]
WantedBy=multi-user.target
";

			var env = string.Join(Environment.NewLine, desc.EnvironmentVariables.Keys
				.OfType<string>()
				.Select(key => $"Environment=\"{key}={desc.EnvironmentVariables[key]}\"")
				.ToArray());

			var deps = string.Join(Environment.NewLine, desc.DependsOn
				.Select(dep => $"Requires={dep}{Environment.NewLine}After={dep}")
			.ToArray());

			var exe = description.Executable;
			string cmd = "";
			if (!string.IsNullOrEmpty(exe) && exe[0] == '"')
			{
				var indexOfQuote = exe.IndexOf('"', 1);
				if (indexOfQuote > 0)
				{
					cmd = exe.Substring(1, indexOfQuote - 1);
					if (!cmd.Contains(Path.DirectorySeparatorChar))
					{
						cmd = Shell.Find(cmd);
						if (cmd != null)
						{
							if (cmd.Contains(' ')) exe = @$"""{cmd}"" {exe.Substring(indexOfQuote + 1)}";
							else exe = $@"{cmd} {exe.Substring(indexOfQuote + 1)}";
						}
					}
				}
			}
			else
			{
				var indexOfSpace = exe.IndexOf(' ');
				if (indexOfSpace > 0)
				{
					cmd = exe.Substring(0, indexOfSpace);
					if (!cmd.Contains(Path.DirectorySeparatorChar))
					{
						cmd = Shell.Find(cmd);
						if (cmd != null)
						{
							if (cmd.Contains(' ')) exe = $@"""{cmd}"" {exe.Substring(indexOfSpace + 1)}";
							else exe = $@"{cmd} {exe.Substring(indexOfSpace + 1)}";
						}
					}
				}
			}

			srvcFile = srvcFile
				.Replace("@description", desc.Description)
				.Replace("@dependsOn", deps)
				.Replace("@exec", exe)
				.Replace("@workdir", desc.Directory)
				.Replace("@environment", env)
				.Replace("@Restart_", !string.IsNullOrEmpty(desc.Restart) ? $"Restart={desc.Restart}" : "")
				.Replace("@RestartSec", !string.IsNullOrEmpty(desc.RestartSec) ? $"RestartSec={desc.RestartSec}" : "")
				.Replace("@StartLimitIntervalSec", !string.IsNullOrEmpty(desc.StartLimitIntervalSec) ? $"StartLimitIntervalSec={desc.StartLimitIntervalSec}" : "")
				.Replace("@StartLimitBurst", !string.IsNullOrEmpty(desc.StartLimitBurst) ? $"StartLimitBurst={desc.StartLimitBurst}" : "")
				.Replace("@User", !string.IsNullOrEmpty(desc.User) ? $"User={desc.User}" : "")
				.Replace("@Group", !string.IsNullOrEmpty(desc.Group) ? $"Group={desc.Group}" : "")
				.Replace("@Syslog", !string.IsNullOrEmpty(desc.SyslogIdentifier) ?
					$"StandardOutput=journal{Environment.NewLine}StandardError=journal{Environment.NewLine}SyslogIdentifier={desc.SyslogIdentifier}" : "")
				.Replace("\r\n", "\n");
			srvcFile = Regex.Replace(srvcFile, @"^\s*$(?!^\[.*?\])", "", RegexOptions.Multiline); // remove empty lines

			File.WriteAllText(Path.Combine(ServicesDirectory, $"{desc.ServiceId}.service"), srvcFile);

			Shell.Exec($"systemctl daemon-reload");

			return this[desc.ServiceId];
		}

		public override void Remove(string serviceId)
		{
			var file = Path.Combine(ServicesDirectory, $"{serviceId}.service");
			if (File.Exists(file))
			{
				File.Delete(file);
				Shell.Exec($"systemctl reload {serviceId}.service");
			}
		}

		public override bool IsInstalled => OSInfo.IsSystemd;

		public Shell Shell => Shell.Default;
	}
}