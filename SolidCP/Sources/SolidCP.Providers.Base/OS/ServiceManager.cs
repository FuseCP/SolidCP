using System;
using System.Linq;
using System.Collections.Generic;

namespace SolidCP.Providers.OS {

    public abstract class ServiceManager {

        public virtual void Start(string serviceId) {
            ChangeStatus(serviceId, OSServiceStatus.Running);
        }

        public virtual void Stop(string serviceId) {
            ChangeStatus(serviceId, OSServiceStatus.Stopped);
        }

        public virtual void Enable(string serviceId) {

        }
        public virtual void Disable(string serviceId) {

        }
        public abstract void SystemReboot();
        public abstract void ChangeStatus(string serviceId, OSServiceStatus status);
        public abstract IEnumerable<OSService> All();
        public abstract OSService Info(string serviceId);
        public abstract void Install(string serviceId, string description, string exe);
        public abstract void Remove(string serviceId);
        public abstract bool IsInstalled { get; }
    }

}