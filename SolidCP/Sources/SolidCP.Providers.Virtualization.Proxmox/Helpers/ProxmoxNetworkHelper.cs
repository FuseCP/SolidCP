using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace SolidCP.Providers.Virtualization.Proxmox
{
    public class ProxmoxNetworkHelper
    {
        private const int defaultvlan = 0;

        public static VirtualMachineNetworkAdapter[] Get(String Content)
            {
                List<VirtualMachineNetworkAdapter> adapters = new List<VirtualMachineNetworkAdapter>();
                try
                {
                    JsonObject jsonResponse = (JsonObject)SimpleJson.DeserializeObject(Content);
                    JsonObject configvalue = (JsonObject)SimpleJson.DeserializeObject(jsonResponse["data"].ToString());
                    foreach (var key in configvalue.Keys)
                    {
                        string val = configvalue[key].ToString();
                        if (key.Contains("net"))
                        {
                            VirtualMachineNetworkAdapter adapter = CreateAdapter(val);
                            adapter.Name = String.Format("{0} {1} VLAN {2}", key, adapter.Name, adapter.vlan);
                            adapters.Add(adapter);
                        }
                    }
                }
                catch
                {
                    adapters = null;
                }
                return adapters.ToArray();
        }

        private static VirtualMachineNetworkAdapter CreateAdapter(String adapterinfo)
        {
            VirtualMachineNetworkAdapter adapter = new VirtualMachineNetworkAdapter();
            try
            {
                adapter.vlan = defaultvlan;
                Array adapterarray = adapterinfo.Split(',');
                foreach (String adapterval in adapterarray)
                {
                    if (adapterval.Contains(":"))
                    {
                        adapter.MacAddress = adapterval.Split('=')[1];
                        adapter.MacAddress = adapter.MacAddress.Replace(":", "");
                        adapter.Name = adapterinfo.Split('=')[0];
                    }
                    else if (adapterval.Contains("bridge"))
                    {
                        adapter.SwitchName = adapterval.Split('=')[1];
                    }
                    else if (adapterval.Contains("tag"))
                    {
                        adapter.vlan = Convert.ToInt32(adapterval.Split('=')[1]);
                    }

                }
            }
            catch
            {
                adapter = null;
            }
            return adapter;
        }
    }
}
