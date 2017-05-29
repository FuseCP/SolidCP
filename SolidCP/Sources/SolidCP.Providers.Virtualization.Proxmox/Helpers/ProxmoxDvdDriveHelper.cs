using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization
{
    public static class ProxmoxDvdDriveHelper
    {
        public static DvdDriveInfo Get(ApiClient client, string vmId)
        {
            DvdDriveInfo info = null;

            var result = client.VMConfig(vmId);

            if (result != null)
            {
                if (result.Data.ide2 == null || !result.Data.ide2.Contains(",")) return info;
                info = new DvdDriveInfo();
                Array resultarr = result.Data.ide2.Split(',');
                foreach (String val in resultarr)
                {
                    if (val.Contains(":"))
                    {
                        info.Path = val;
                        string isoname = val.Split('/')[1];
                        if (isoname == "")
                            isoname = val.Split('/')[0];
                        info.Name = isoname.Replace(".iso", "");
                    }
                }
            }
            return info;
        }


        public static void Set(ApiClient client, string vmId, string path, int size)
        {
            if (path == null)
            {
                Proxmox.UpdateDVD configuration = new Proxmox.UpdateDVD { };
                configuration.ide2 = "none,media=cdrom";
                client.UpdateDVD(vmId, configuration);
            }
            else
            {
                Proxmox.UpdateDVD configuration = new Proxmox.UpdateDVD { };
                configuration.ide2 = String.Format("{0},media=cdrom,size={1}M", path, Convert.ToInt32(size / Constants.Size1M));
                client.UpdateDVD(vmId, configuration);
            }
        }

        public static void Update(ApiClient client, VirtualMachine vm, bool dvdDriveShouldBeInstalled)
        {
            if (!vm.DvdDriveInstalled && dvdDriveShouldBeInstalled)
                Add(client, vm.VirtualMachineId);
            else if (vm.DvdDriveInstalled && !dvdDriveShouldBeInstalled)
                Remove(client, vm.VirtualMachineId);
        }

        public static void Add(ApiClient client, string vmId)
        {
            Set(client, vmId, null, 0);
        }

        public static void Remove(ApiClient client, string vmId)
        {
            client.Unlink(vmId, "ide2");
        }
    }
}
