using System;
using System.Linq;
using System.Collections.Generic;

namespace SolidCP.Providers.OS
{
	public enum WindowsServiceStartMode { Demand, Boot, System, Auto, Disabled, DelayedAuto }
	public enum WindowsServiceType { Own, Share, Interact, Kernel, Filesys, Rec, Userown, Usershare }
	public enum WindowsServiceErrorHandling { Normal, Severe, Critical, Ignore }

	public class ServiceDescription
	{
		public string Executable { get; set; }
		public string ServiceId { get; set; }
		public List<string> DependsOn { get; set; } = new List<string>();
	}

	public class WindowsServiceDescription: ServiceDescription {
		public WindowsServiceStartMode Start { get; set; }
		public WindowsServiceType Type { get; set; }
		public WindowsServiceErrorHandling Error { get; set; }
		public bool? Tag { get; set; }
		public string Group { get; set; }
		public string Object { get; set; }
		public string Password { get; set; }
		public string DisplayName { get; set; }
	}
	public class SystemdServiceDescription: ServiceDescription {
		public string Group { get; set; }
		public string SyslogIdentifier { get; set; }
		public string Description { get; set; }
		public string Directory { get; set; }
		public string User { get; set; }
		public string StartLimitIntervalSec { get; set; }
		public string StartLimitBurst { get; set; }
		public string Restart { get; set; }
		public string RestartSec { get; set; }
		public Dictionary<string, string> Environment { get; set; } = new Dictionary<string, string>();
	}
	public class OpenRCServiceDescription : ServiceDescription
	{
		public string Need { get; set; }
		public string Use { get; set; }
		public string Want { get; set; }
		public string Before { get; set; }
		public string After { get; set; }
		public string Provide { get; set; }
		public string Keyword { get; set; }
		public string Command { get; set; }
		public string CommandArgs { get; set; }
		public string CommandArgsBackground { get; set; }
		public string PidFile { get; set; }
		public bool? CommandBackground { get; set; }
		public string CommandUser { get; set; }
		public string Capabilities { get; set; }
		public string Procname { get; set; }
		public string ExtraCommands { get; set; }
		public string ExtraStartedCommands { get; set; }
		public string ExtraStoppedCommands { get; set; }
		public string WorkingDirectory { get; set; }
		public Dictionary<string, string> Environment { get; set; } = new Dictionary<string, string>();
		public string Body { get; set; }
		public string Description { get; set; }
		public int? StopTimeout { get; set; }
	}
	public class ServiceManager
	{
		public string ServiceId { get; private set; }
		public ServiceController Controller { get; private set; }

		public ServiceManager(ServiceController controller, string serviceId)
		{
			Controller = controller;
			ServiceId = serviceId;
		}
		public void Start() => Controller.Start(ServiceId);
		public void Stop() => Controller.Stop(ServiceId);
		public void Restart() => Controller.Restart(ServiceId);
		public void Reload() => Controller.Reload(ServiceId);
		public void Enable() => Controller.Enable(ServiceId);
		public void Disable() => Controller.Disable(ServiceId);
		public void ChangeStatus(OSServiceStatus status) => Controller.ChangeStatus(ServiceId, status);
		public OSService Info => Controller.Info(ServiceId);
		public void Remove() => Controller.Remove(ServiceId);
	}
	public abstract class ServiceController
	{
		public virtual void Start(string serviceId)
		{
			ChangeStatus(serviceId, OSServiceStatus.Running);
		}

		public virtual void Stop(string serviceId)
		{
			ChangeStatus(serviceId, OSServiceStatus.Stopped);
		}
		public virtual void Restart(string serviceId)
		{
			Stop(serviceId);
			Start(serviceId);
		}
		public virtual void Reload(string serviceId) => Restart(serviceId);
		public virtual void Enable(string serviceId) { }
		public virtual void Disable(string serviceId) { }
		public abstract void SystemReboot();
		public abstract void ChangeStatus(string serviceId, OSServiceStatus status);
		public abstract IEnumerable<OSService> All();
		public ServiceManager this[string serviceId] => new ServiceManager(this, serviceId);
		public abstract OSService Info(string serviceId);
		public abstract ServiceManager Install(ServiceDescription service);
		public abstract void Remove(string serviceId);
		public abstract bool IsInstalled { get; }
	}

}