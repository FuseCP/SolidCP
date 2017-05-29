using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class ProxmoxVHDHelper
    {

        public static VirtualHardDiskInfo[] Get(String Content)
        {
            List<VirtualHardDiskInfo> disks = new List<VirtualHardDiskInfo>();

            try
            {
                JsonObject jsonResponse = (JsonObject)SimpleJson.DeserializeObject(Content);
                JsonObject configvalue = (JsonObject)SimpleJson.DeserializeObject(jsonResponse["data"].ToString());

                foreach (var key in configvalue.Keys)
                {
                    string val = configvalue[key].ToString();
                    if ((key.Contains("ide") || key.Contains("sata") || key.Contains("virtio") || key.Contains("scsi")) && val.Contains(":"))
                    {
                        VirtualHardDiskInfo disk = new VirtualHardDiskInfo();
                        disk.ControllerNumber = 1;
                        disk.ControllerLocation = 1;
                        if (key.Contains("ide") || key.Contains("virtio"))
                        {
                            disk.VHDControllerType = ControllerType.IDE;
                        }
                        else
                        {
                            disk.VHDControllerType = ControllerType.SCSI;
                        }
                        disk.Path = parsepath(val);
                        disk.Name = key;
                        disks.Add(disk);
                    }
                }

            }
            catch
            {
                disks = null;
            }

            return disks.ToArray();
        }


        static String parsepath(String io)
        {
            String Path = "";
            try
            {
                Array ioarray = io.Split(',');
                foreach (String ioval in ioarray)
                {
                    if (ioval.Contains(':'))
                        Path = ioval;
                }
            }
            catch
            {
                Path = io;
            }
            return Path;
        }
    }
}
