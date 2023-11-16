using System;
using System.Collections.Generic;

namespace SolidCP.Providers.OS
{

    public class SystemdServiceController: ServiceController {

        public override IEnumerable<OSService> All() {
            var text = Shell.ExecAsync("systemctl").Output().Result;
            throw new NotImplementedException();
        }
        public override OSService Info(string serviceId) {
            throw new NotImplementedException();
        }
        public override void ChangeStatus(string serviceId, OSServiceStatus status) {

        }


        public override void SystemReboot() {
            Shell.ExecAsync("systemctl reboot");
        }

        public override void Install(ServiceDescription description) {

        }

        public override void Remove(string serviceId) {
            
        }

        public override bool IsInstalled => Shell.Find("systemctl") != null;

        public Shell Shell => Providers.OS.OSInfo.Shell;
    }
}