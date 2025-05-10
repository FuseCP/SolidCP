using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using Claunia.PropertyList;

namespace SolidCP.Providers.OS;


public class LaunchdServiceController : ServiceController
{
	public const string ServicesDirectory = "/Library/LaunchDaemons";
	public override bool IsInstalled => OSInfo.IsMac;
	public Shell Shell => Shell.Standard;
	public override void SystemReboot() => Shell.Exec("launchctrl reboot");
	public override IEnumerable<OSService> All()
	{
		var servicesText = Shell.Exec("launchctl list").Output().Result;
		var matches = Regex.Matches(servicesText, @"^\s*(?<name>.+?)\s+(?<pid>\d+)\s*$", RegexOptions.Multiline);
		foreach (Match match in matches)
		{
			var name = match.Groups["name"].Value;
			var pid = match.Groups["pid"].Value;
			var script = Path.Combine(ServicesDirectory, name);
			bool running = !string.IsNullOrEmpty(pid);
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

	public override OSService Info(string serviceId)
	{
		var output = Shell.Exec($"launchctl list {serviceId}").Output().Result;
		if (output == null) return null;
		var match = Regex.Match(output, @"^\s*Loaded:\s*(?<loaded>[^\s$]+).*?$(^.*$\r?\n)*\s*Active:\s*(?<active>[^\s$]+)\s+\((?<status>[^\)]+)\)", RegexOptions.Multiline);
		string loaded, active, status = null;
		if (match.Success)
		{
			loaded = match.Groups["loaded"].Value;
			active = match.Groups["active"].Value;
			status = match.Groups["status"].Value;
		}
		else return null;

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
		var service = Info(serviceId);
		if (service == null) throw new ArgumentException($"Service {serviceId} not found");
		if (service.Status == OSServiceStatus.Running)
		{
			if (status == OSServiceStatus.PausePending || status == OSServiceStatus.Paused ||
				status == OSServiceStatus.Stopped || status == OSServiceStatus.StopPending)
			{
				Shell.Exec($"launchctl stop {serviceId}");
			}
		}
		else
		{
			if (status == OSServiceStatus.StartPending || status == OSServiceStatus.Running ||
				status == OSServiceStatus.ContinuePending)
			{
				Shell.Exec($"launchctl start {serviceId}");
			}
		}
	}
	public override void Remove(string serviceId)
	{
		Shell.Exec($"launchctl disable system {serviceId}");
		Shell.Exec($"launchctl unload system {serviceId}");
	}

	public override ServiceManager Install(ServiceDescription serviceDescription)
	{
		var srvc = serviceDescription as LaunchdServiceDescription;
		if (srvc == null) throw new ArgumentException("Service description is not of type LaunchdServiceDescription");

		var serviceId = srvc.ServiceId;
		var serviceFile = Path.Combine(ServicesDirectory, $"{serviceId}.plist");
		var dict = new NSDictionary();
		dict.Add("Label", serviceId);
		if (srvc.Program != null)
		{
			if (string.IsNullOrEmpty(srvc.Arguments))
			{
				dict.Add("Program", srvc.Program);
			}
			else
			{
				var args = new[] { srvc.Program }
					.Concat(
						Regex.Matches(srvc.Arguments, @"[^""\s]*(""[^""]*"")?[^""\s]*", RegexOptions.ExplicitCapture)
						.OfType<Match>()
						.Select(m => m.Value.Length > 2 && m.Value[0] == '"' && m.Value[m.Value.Length - 1] == '"' ?
							m.Value.Substring(1, m.Value.Length - 2) : m.Value))
						.ToArray();
				dict.Add("ProgramArguments", args);
			}
		}

		if (srvc.UserName != null) dict.Add("UserName", srvc.UserName);
		if (srvc.OnDemand != null) dict.Add("OnDemand", srvc.OnDemand);
		if (srvc.StartOnMount != null) dict.Add("StartOnMount", srvc.StartOnMount);
		if (srvc.OnDemand != null) dict.Add("OnDemand", srvc.OnDemand);
		if (srvc.QueueDirectories != null && srvc.QueueDirectories.Count > 0)
		{
			dict.Add("QueueDirectories", srvc.QueueDirectories);
		}
		if (srvc.WatchPaths != null && srvc.WatchPaths.Count > 0)
		{
			dict.Add("WatchPaths", srvc.WatchPaths);
		}
		if (srvc.StartInterval != null) dict.Add("StartInterval", srvc.StartInterval);
		if (srvc.RunAtLoad != null) dict.Add("RunAtLoad", srvc.RunAtLoad);
		if (srvc.RootDirectory != null) dict.Add("RootDirectory", srvc.RootDirectory);
		if (srvc.WorkingDirectory != null) dict.Add("WorkingDirectory", srvc.WorkingDirectory);
		if (srvc.StandardOutPath != null) dict.Add("StandardOutPath", srvc.StandardOutPath);
		if (srvc.StandardErrorPath != null) dict.Add("StandardErrorPath", srvc.StandardErrorPath);
		if (srvc.StandardInPath != null) dict.Add("StandardInPath", srvc.StandardInPath);
		if (srvc.LowPriorityIO != null) dict.Add("LowPriorityIO", srvc.LowPriorityIO);
		if (srvc.AbandonProcessGroup != null) dict.Add("AbandonProcessGroup", srvc.AbandonProcessGroup);
		if (srvc.SessionCreate != null) dict.Add("SessionCreate", srvc.SessionCreate);
		if (srvc.KeepAlive != null) dict.Add("KeepAlive", srvc.KeepAlive);
		if (srvc.ExitTimeout != null) dict.Add("ExitTimeout", srvc.ExitTimeout);
		if (srvc.Environment != null && srvc.Environment.Count > 0) dict.Add("EnvironmentVariables", srvc.Environment);

		using (var file = File.Create(serviceFile))
		{
			PropertyListParser.SaveAsXml(dict, file);
		}
		Shell.Exec($"launchctl load -w {serviceFile}");
		Shell.Exec($"launchctl enable system {serviceId}");

		return new ServiceManager(this, serviceId);
	}
}
