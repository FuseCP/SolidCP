using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace SolidCP.Providers.OS
{

	public class SystemdServiceController : ServiceController
	{
		public virtual string ServicesDirectory => " /lib/systemd/system";

		public override IEnumerable<OSService> All()
		{
			var text = Shell.Exec("systemctl --full --type=service --no-pager --all").Output().Result;
			var matches = Regex.Matches(text, @"^\u25CF?\s*(?<name>.*?).service\s+(?<loaded>[^\s$]+)\s+(?<active>[^\s$]+)\s+(?<state>[^\s$]+)\s+(?<desc>.*?)$", RegexOptions.Multiline);
			foreach (Match match in matches)
			{
				var name = Path.GetFileNameWithoutExtension(match.Groups["name"].Value);
				var desc = match.Groups["desc"].Value;
				var loaded = match.Groups["loaded"].Value;
				var active = match.Groups["active"].Value;
				var state = match.Groups["state"].Value;

				var srvc = new OSService()
				{
					Name = name,
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
			var match = Regex.Match(output, $@"^\s*Loaded:\s*(?<loaded>[^\s$]+).*?$^\s*Active:\s*(?<active>[^\s$]+)\s+\((?<status>[^\)]+\)", RegexOptions.Multiline);

			if (match.Success)
			{
				var loaded = match.Groups["loaded"].Value;
				var active = match.Groups["active"].Value;
				var status = match.Groups["status"].Value;
				return new OSService()
				{
					Id = serviceId,
					Name = serviceId,
					Description = "",
					Status = status == "dead" || status == "exited" ? OSServiceStatus.Stopped :
							status == "running" ? OSServiceStatus.Running : OSServiceStatus.Stopped
				};
			}
			return null;
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

		public override void Install(ServiceDescription description)
		{
			var srvcFile = @"
[Unit]
Description=@description
@dependsOn

[Service]
Type=simple
ExecStart=@exec
WorkingDirectory=@workdir
@envirnonment
Restart=on-failure

[Install]
WantedBy=multi-user.target
";
			var env = string.Join(Environment.NewLine, description.EnvironmentVariables.Keys
				.OfType<string>()
				.Select(key => $"Environment=\"{key}={description.EnvironmentVariables[key]}\"")
				.ToArray());

			var deps = string.Join(Environment.NewLine, description.DependsOn
				.Select(dep => $"Requires={dep}{Environment.NewLine}After={dep}")
			.ToArray());

			srvcFile = srvcFile.Replace("@description", description.Description)
				.Replace("@dependsOn", deps)
				.Replace("@exec", description.Executable)
				.Replace("@workdir", description.Directory)
				.Replace("@environment", env);

			File.WriteAllText(Path.Combine(ServicesDirectory, $"{description.ServiceId}.service"), srvcFile);

			Shell.Exec($"systemctl reload {description.ServiceId}.service");
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

		public override bool IsInstalled
		{
			get
			{
				try
				{
					var proc0 = System.Diagnostics.Process.GetProcessById(0);
					if (proc0 != null) return proc0.ProcessName.StartsWith("systemd", StringComparison.OrdinalIgnoreCase);
				}
				catch { }

				if (Shell.Find("pstree") != null)
				{
					return Shell.Exec("pstree").Output().Result.StartsWith("systemd");
				}

				return Shell.Find("systemctl") != null;
			}
		}

		public Shell Shell => Providers.OS.OSInfo.Shell;
	}
}