using System;
using System.Linq;
using System.Collections.Generic;

namespace SolidCP.Providers.OS
{

	public class ServiceDescription
	{
		public string Executable { get; set; }
		public string ServiceId { get; set; }
		public string Description { get; set; }
		public string Directory { get; set; }
		public string User { get; set; }
		public string StartLimitIntervalSec { get; set; }
		public string StartLimitBurst { get; set; }
		public string Restart { get; set; }
		public string RestartSec { get; set; }
		public Dictionary<string, string> EnvironmentVariables { get; set; } = new Dictionary<string, string>();
		public List<string> DependsOn { get; set; } = new List<string>();
	}

	public class WindowsServiceDescription: ServiceDescription { }
	public class UnixServiceDescription: ServiceDescription {
		public string Group { get; set; }
		public string SyslogIdentifier { get; set; }

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