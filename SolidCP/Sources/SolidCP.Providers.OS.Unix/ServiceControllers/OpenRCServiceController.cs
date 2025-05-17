using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;

namespace SolidCP.Providers.OS;

public class OpenRCServiceController: ServiceController
{
	const string ScriptPath = "/etc/init.d";
	public override ServiceManager Install(ServiceDescription service)
	{
		var srvc = service as OpenRCServiceDescription;
		if (srvc == null) throw new ArgumentException("Service description is not of type OpenRCServiceDescription");

		var body = new StringBuilder();
		body.AppendLine("#!/sbin/openrc-run");

		if (!string.IsNullOrEmpty(srvc.ServiceId)) body.AppendLine($"name=\"{srvc.ServiceId}\"");
		if (!string.IsNullOrEmpty(srvc.Description)) body.AppendLine($"description=\"{srvc.Description}\"");
		if (srvc.DependsOn != null && srvc.DependsOn.Count > 0)
		{
			var deps = string.Join(" ", srvc.DependsOn);
			if (string.IsNullOrEmpty(srvc.Need)) srvc.Need = deps;
			else srvc.Need += " " + deps;
		}
		if (!string.IsNullOrEmpty(srvc.WorkingDirectory)) body.AppendLine($"directory=\"{srvc.WorkingDirectory}\"");

		if (!string.IsNullOrEmpty(srvc.Provide) ||
			!string.IsNullOrEmpty(srvc.Need) ||
			!string.IsNullOrEmpty(srvc.Use) ||
			!string.IsNullOrEmpty(srvc.Want) ||
			!string.IsNullOrEmpty(srvc.Before) ||
			!string.IsNullOrEmpty(srvc.After))
		{
			body.AppendLine("depend() {");
			if (!string.IsNullOrEmpty(srvc.Need)) body.AppendLine($"  need {srvc.Need}");
			if (!string.IsNullOrEmpty(srvc.Use)) body.AppendLine($"  use {srvc.Use}");
			if (!string.IsNullOrEmpty(srvc.Want)) body.AppendLine($"  want {srvc.Want}");
			if (!string.IsNullOrEmpty(srvc.Before)) body.AppendLine($"  before {srvc.Before}");
			if (!string.IsNullOrEmpty(srvc.After)) body.AppendLine($"  after {srvc.After}");
			if (!string.IsNullOrEmpty(srvc.Provide)) body.AppendLine($"  provide {srvc.Provide}");
			if (!string.IsNullOrEmpty(srvc.Keyword)) body.AppendLine($"  keyword {srvc.Keyword}");
			body.AppendLine("}");
		}

		if (!string.IsNullOrEmpty(srvc.Command)) body.AppendLine($"command=\"{srvc.Command}\"");
		if (!string.IsNullOrEmpty(srvc.CommandArgs)) body.AppendLine($"command_args=\"{srvc.CommandArgs}\"");
		if (!string.IsNullOrEmpty(srvc.CommandArgsBackground)) body.AppendLine($"command_args_background=\"{srvc.CommandArgsBackground}\"");
		if (!string.IsNullOrEmpty(srvc.PidFile)) body.AppendLine($"pidfile=\"{srvc.PidFile}\"");
		if (srvc.CommandBackground != null) body.AppendLine($"command_background={srvc.CommandBackground.ToString().ToLower()}");
		if (!string.IsNullOrEmpty(srvc.CommandUser)) body.AppendLine($"command_user=\"{srvc.CommandUser}\"");
		if (!string.IsNullOrEmpty(srvc.Capabilities)) body.AppendLine($"capabilities=\"{srvc.Capabilities}\"");
		if (!string.IsNullOrEmpty(srvc.Procname)) body.AppendLine($"procname=\"{srvc.Procname}\"");
		if (!string.IsNullOrEmpty(srvc.ExtraCommands)) body.AppendLine($"extra_commands=\"{srvc.ExtraCommands}\"");
		if (!string.IsNullOrEmpty(srvc.ExtraStartedCommands)) body.AppendLine($"extra_started_commands=\"{srvc.ExtraStartedCommands}\"");
		if (!string.IsNullOrEmpty(srvc.ExtraStoppedCommands)) body.AppendLine($"extra_stopped_commands=\"{srvc.ExtraStoppedCommands}\"");
		if (srvc.Environment != null && srvc.Environment.Count > 0)
		{
			foreach (var env in srvc.Environment) body.AppendLine($"export {env.Key}=\"{env.Value}\"");
		}

		if (srvc.StopTimeout != null && !string.IsNullOrEmpty(srvc.PidFile))
		{
			body.AppendLine($@"stop() {{
    ebegin ""Stopping myservice""

    # Send SIGTERM to process
    start-stop-daemon --stop --quiet --pidfile {srvc.PidFile}

    # Wait up to timeout seconds
    local timeout={srvc.StopTimeout}
    local count=0
    while [ -e {srvc.PidFile} ] && [ $count -lt $timeout ]; do
        sleep 1
        count=$((count + 1))
    done

    # Force kill if still running
    if [ -e {srvc.PidFile} ]; then
        start-stop-daemon --stop --quiet --pidfile {srvc.PidFile} --signal KILL
    fi

    eend $?
}}
");
		}
		if (!string.IsNullOrEmpty(srvc.Body))
		{
			body.AppendLine();
			body.AppendLine(srvc.Body);
		}
		
		var script = Path.Combine(ScriptPath, srvc.ServiceId);
		File.WriteAllText(script, body.ToString());

		Shell.Exec($"chmod +x {script}");
		Shell.Exec($"rc-update add {srvc.ServiceId} default");

		return new ServiceManager(this, srvc.ServiceId);
	}
	public override void SystemReboot() => Shell.Exec("openrc-shutdown reboot");
	public override void ChangeStatus(string serviceId, OSServiceStatus status)
	{
		var service = Info(serviceId);
		if (service == null)
		{
			if (Shell.Exec($"rc-service -e {serviceId}").ExitCode().Result == 0)
			{
				var output = Shell.Exec($"rc-service -C {serviceId} status").Output().Result;
				var match = Regex.Match(output, @"^.*status\s*:\s*(?<status>.+?)\s*$", RegexOptions.Multiline);
				var stat = OSServiceStatus.Stopped;
				switch (match.Groups["status"].Value)
				{
					case "started":
						stat = OSServiceStatus.Running;
						break;
					case "stopped":
					case "crashed":
					default:
						stat = OSServiceStatus.Stopped;
						break;
					case "stopping":
						stat = OSServiceStatus.StopPending;
						break;
					case "starting":
						stat = OSServiceStatus.StartPending;
						break;
				}
				service = new OSService() { Id = serviceId, Name = serviceId, Status = stat };
			}
			else throw new ArgumentException($"Service {serviceId} not found");
		}

		if (service.Status == OSServiceStatus.Running)
		{
			if (status == OSServiceStatus.PausePending || status == OSServiceStatus.Paused ||
				status == OSServiceStatus.Stopped || status == OSServiceStatus.StopPending)
			{
				Shell.Exec($"rc-service {serviceId} stop");
			}
		}
		else
		{
			if (status == OSServiceStatus.StartPending || status == OSServiceStatus.Running ||
				status == OSServiceStatus.ContinuePending)
			{
				Shell.Exec($"rc-service {serviceId} start");
			}
		}
	}
	public override IEnumerable<OSService> All()
	{
		var servicesText = Shell.Exec("rc-status -f ini -s").Output().Result;
		var matches = Regex.Matches(servicesText, @"^\s*(?<name>.+?)\s*=\s*(?<status>.+?)\s*$", RegexOptions.Multiline);
		foreach (Match match in matches)
		{
			var name = match.Groups["name"].Value;
			var status = match.Groups["status"].Value;
			var script = Shell.Exec($"rc-service -r {name}").Output().Result;
			bool running = status != "stopped" && status != "crashed";
			string description = "";
			if (File.Exists(script))
			{
				var scriptText = File.ReadAllText(script);
				description = Regex.Match(scriptText, @"^\s*description\s*=\s*""(?<description>.*?)""\s*$", RegexOptions.Multiline)?.Value ?? "";
			}

			var srvc = new OSService()
			{
				Name = name,
				Id = name,
				Description = description,
				CanStop = running,
				CanPauseAndContinue = false,
				Status = running ? OSServiceStatus.Running : OSServiceStatus.Stopped
			};
			yield return srvc;
		}
	}

	public override OSService Info(string serviceId) => All().FirstOrDefault(s => s.Id == serviceId);

	public override void Remove(string serviceId)
	{
		Shell.Exec($"rc-update del {serviceId}");
	}
	public override bool IsInstalled => OSInfo.IsOpenRC;
	public Shell Shell => Shell.Default;
}
