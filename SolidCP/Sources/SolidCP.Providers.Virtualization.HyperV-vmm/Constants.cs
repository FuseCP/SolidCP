using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization
{
    public static class Constants
    {
        public const string CONFIG_USE_DISKPART_TO_CLEAR_READONLY_FLAG = "SolidCP.HyperV.UseDiskPartClearReadOnlyFlag";
        public const string WMI_VIRTUALIZATION_NAMESPACE = @"root\scvmm";
        public const string WMI_CIMV2_NAMESPACE = @"root\cimv2";

        public const string LIBRARY_INDEX_FILE_NAME = "index.xml";

        public const string EXTERNAL_NETWORK_ADAPTER_NAME = "External Network Adapter";
        public const string PRIVATE_NETWORK_ADAPTER_NAME = "Private Network Adapter";
        public const string MANAGEMENT_NETWORK_ADAPTER_NAME = "Management Network Adapter";

        public const Int64 Size1G = 0x40000000;
        public const Int64 Size1M = 0x100000;
        public const Int64 Size1K = 1024;

        public const string KVP_RAM_SUMMARY_KEY = "VM-RAM-Summary";
        public const string KVP_HDD_SUMMARY_KEY = "VM-HDD-Summary";
    }
}
