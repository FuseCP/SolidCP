using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM;
using SolidCP.Providers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers
{    
    public class VirtualizationUtils: ControllerBase
    {
        public const Int64 Size1G = 0x40000000;

        private const string SCP_HOSTNAME_PREFIX = "SCP-"; //min and max are 4 symbols! ([0-9][A-z] and -)

        public VirtualizationUtils(ControllerBase provider) : base(provider) { }

        public string GetHyperVConfigurationVersionFromSettings(StringDictionary settings)
        {
            return string.IsNullOrEmpty(settings["HyperVConfigurationVersion"]) ? "0.0" : settings["HyperVConfigurationVersion"];
        }

        public string GetCorrectVmRootFolderPath(StringDictionary settings, ServiceProviderItem vm)
        {
            string rootFolderPattern = settings["RootFolder"];
            if (rootFolderPattern.IndexOf("[") == -1)
            {
                // no pattern has been specified
                if (!rootFolderPattern.EndsWith("\\"))
                    rootFolderPattern += "\\";
                rootFolderPattern += "[username]\\[vps_hostname]";
            }

            string rootFolderPath = EvaluateItemVariables(rootFolderPattern, vm);
            if (!rootFolderPath.EndsWith(vm.Name))  //we must be sure that Path ends with vm.Name (hostname)!
            {
                rootFolderPath = Path.Combine(rootFolderPath, vm.Name);
            }
            return rootFolderPath;
        }

        public string[] GetCorrectVmVirtualHardDrivePaths(int hddNumber, string vmName, string rootFolderPath, string vhdExtension)
        {
            //var correctVhdPath = correctVhdPathOfTemplatesPathAndosTemplateFile;
            //string msHddHyperVFolderName = "Virtual Hard Disks\\" + vm.Name;
            //vm.VirtualHardDrivePath = new string[VMSettings.HddSize.Length];
            //for (int i = 0; i < VMSettings.HddSize.Length; i++)
            //{
            //    vm.VirtualHardDrivePath[i] = Path.Combine(vm.RootFolderPath, msHddHyperVFolderName + (i > 0 ? i.ToString() : "") + Path.GetExtension(correctVhdPath));
            //}
            string msHddHyperVFolderName = "Virtual Hard Disks\\" + vmName;
            string[] virtualHardDrivePath = new string[hddNumber];
            for (int i = 0; i < hddNumber; i++)
            {
                virtualHardDrivePath[i] = Path.Combine(rootFolderPath, msHddHyperVFolderName + (i > 0 ? i.ToString() : "") + vhdExtension);
            }
            return virtualHardDrivePath;
        }

        public string GetCorrectTemplateFilePath(string templatesPath, string osTemplateFile)
        {
            return Path.Combine(templatesPath, osTemplateFile);
        }

        public string EvaluateItemVariables(string str, ServiceProviderItem item)
        {
            str = Utils.ReplaceStringVariable(str, "vps_hostname", item.Name);

            return EvaluateSpaceVariables(str, item.PackageId);
        }

        public string EvaluateSpaceVariables(string str, int packageId)
        {
            // load package
            PackageInfo package = PackageController.GetPackage(packageId);
            UserInfo user = UserController.GetUser(package.UserId);
            // get 1 IP of VM
            PackageIPAddress[] ips = ServerController.GetPackageIPAddresses(packageId, 0,
                                IPAddressPool.VpsExternalNetwork, "", "", "", 0, 1, true).Items;

            str = Utils.ReplaceStringVariable(str, "space_id", packageId.ToString());
            str = Utils.ReplaceStringVariable(str, "space_name", package.PackageName);
            str = Utils.ReplaceStringVariable(str, "user_id", user.UserId.ToString());
            str = Utils.ReplaceStringVariable(str, "username", user.Username);
            str = Utils.ReplaceStringVariable(str, "ip_last_1_octect", GetIPv4LastOctetsFromPackage(1, packageId, ips));
            str = Utils.ReplaceStringVariable(str, "ip_last_2_octects", GetIPv4LastOctetsFromPackage(2, packageId, ips));
            str = Utils.ReplaceStringVariable(str, "ip_last_3_octects", GetIPv4LastOctetsFromPackage(3, packageId, ips));
            str = Utils.ReplaceStringVariable(str, "ip_last_4_octects", GetIPv4LastOctetsFromPackage(4, packageId, ips));

            return EvaluateRandomSymbolsVariables(str);
        }

        public string EvaluateRandomSymbolsVariables(string str)
        {
            str = Utils.ReplaceStringVariable(str, "guid", Guid.NewGuid().ToString("N"));
            str = Utils.ReplaceStringVariable(str, "mac", NetworkHelper.GenerateMacAddress());
            str = Utils.ReplaceStringVariable(str, "netbiosname", GenerateFakeNetBIOS());

            return str;
        }
        public string GenerateFakeNetBIOS()
        {
            return SCP_HOSTNAME_PREFIX + Utils.GetRandomString(11);
        }

        public string GetIPv4LastOctetsFromPackage(ushort octets, int packageId)
        {
            return GetIPv4LastOctetsFromPackage(octets, packageId, null);
        }

        public string GetIPv4LastOctetsFromPackage(ushort octets, int packageId, PackageIPAddress[] ips)
        {
            int maxItems = 1;
            string ExternalIP = "127.0.0.1"; //just a default IP
            if (ips == null || ips.Length == 0)
                ips = ServerController.GetPackageIPAddresses(packageId, 0,
                                IPAddressPool.VpsExternalNetwork, "", "", "", 0, maxItems, true).Items;
            if (ips.Length > 0)
                ExternalIP = ips[0].ExternalIP;

            byte[] Bytes = System.Net.IPAddress.Parse(ExternalIP).GetAddressBytes();
            StringBuilder sb = new StringBuilder();
            for (int i = 4 - octets; i < 4; i++)
                sb.AppendFormat("{0}-", Bytes[i]);
            sb.Length--; //delete the last symbol "-"

            return sb.ToString();
        }
    }    
}
