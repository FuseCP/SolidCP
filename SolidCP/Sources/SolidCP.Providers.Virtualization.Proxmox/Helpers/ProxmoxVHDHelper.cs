using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
				JsonElement jsonResponse = JsonDocument.Parse(Content).RootElement;
				JsonElement configvalue = jsonResponse.GetProperty("data");

				foreach (var property in configvalue.EnumerateObject())
				{
					string val = property.Value.GetString();
					if ((property.Name.Contains("ide") || property.Name.Contains("sata") || property.Name.Contains("virtio") || property.Name.Contains("scsi")) && val.Contains(":"))
					{
						VirtualHardDiskInfo disk = new VirtualHardDiskInfo();
						disk.ControllerNumber = 1;
						disk.ControllerLocation = 1;
						if (property.Name.Contains("ide") || property.Name.Contains("virtio"))
						{
							disk.VHDControllerType = ControllerType.IDE;
						}
						else
						{
							disk.VHDControllerType = ControllerType.SCSI;
						}
						disk.Path = parsepath(val);
						disk.Name = property.Name;
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
