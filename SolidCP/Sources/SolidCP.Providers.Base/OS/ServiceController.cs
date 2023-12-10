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
		public string StartLimitIntervalSec { get; set; }
		public string StartLimitBurst { get; set; }
		public string Restart { get; set; }
		public string RestartSec { get; set; }
		public string SyslogIdentifier { get; set; }
		public Dictionary<string, string> EnvironmentVariables { get; set; } = new Dictionary<string, string>();
		public List<string> DependsOn { get; set; } = new List<string>();
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

		public virtual void Enable(string serviceId)
		{

		}
		public virtual void Disable(string serviceId)
		{

		}
		public abstract void SystemReboot();
		public abstract void ChangeStatus(string serviceId, OSServiceStatus status);
		public abstract IEnumerable<OSService> All();
		public abstract OSService Info(string serviceId);
		public abstract void Install(ServiceDescription service);
		public abstract void Remove(string serviceId);
		public abstract bool IsInstalled { get; }
	}

}