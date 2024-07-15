using SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.VM;
using SolidCP.Providers.Virtualization;
//using SolidCP.Providers.Virtualization2012;
using SolidCP.Server.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Code.Virtualization2012.Helpers.PS
{
    public class PowerShellScript: ControllerBase
    {
        public PowerShellScript(ControllerBase provider): base(provider) { }

        public void CheckCustomPsScript(PsScriptPoint point, VirtualMachine vm)
        {
            try
            {
                StringDictionary settings = ServerController.GetServiceSettings(vm.ServiceId);
                string xml = settings["PsScript"];
                var config = new ConfigFile(xml);
                LibraryItem[] scripts = config.LibraryItems;
                foreach (LibraryItem item in scripts)
                {
                    if (!String.IsNullOrEmpty(item.Name) && !String.IsNullOrEmpty(item.Description) && item.Name.Equals(point.ToString()))
                    {
                        string script = PreparePsScript(item.Description, vm);
                        VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
                        if (vs != null) vs.ExecuteCustomPsScript(script);
                    }
                }
            }
            catch (Exception) { }
        }

        private string PreparePsScript(string script, VirtualMachine vm)
        {
            string vars = "";
            try
            {
                if (script.Contains("$vmName")) vars += "$vmName = \"" + vm.Name + "\"" + Environment.NewLine;
                if (script.Contains("$vmId")) vars += "$vmId = \"" + vm.VirtualMachineId + "\"" + Environment.NewLine;
                if (script.Contains("$vmTemplateName")) vars += "$vmTemplateName = \"" + vm.OperatingSystemTemplate + "\"" + Environment.NewLine;
                if (script.Contains("$vmTemplatePath")) vars += "$vmTemplatePath = \"" + vm.OperatingSystemTemplatePath + "\"" + Environment.NewLine;
                if (script.Contains("$vmObject")) vars += "$vmObject = Get-VM -Id \"" + vm.VirtualMachineId + "\"" + Environment.NewLine;
                PrepareNetworkVariables(script, ref vars, vm, "ext");
                PrepareNetworkVariables(script, ref vars, vm, "priv");
                PrepareNetworkVariables(script, ref vars, vm, "mng");
                PrepareNetworkVariables(script, ref vars, vm, "dmz");
            }
            catch (Exception) { }
            return vars + script;
        }

        private void PrepareNetworkVariables(string script, ref string vars, VirtualMachine vm, string networkPrefix)
        {
            try
            {
                string vIps = "$" + networkPrefix + "IpAddresses";
                string vMasks = "$" + networkPrefix + "Masks";
                string vGateway = "$" + networkPrefix + "Gateway";
                string vAdapterName = "$" + networkPrefix + "AdapterName";
                string vAdapterMac = "$" + networkPrefix + "AdapterMac";
                if (script.Contains(vIps) || script.Contains(vGateway) || script.Contains(vMasks) || script.Contains(vAdapterName) || script.Contains(vAdapterMac))
                {
                    string ips = "";
                    string masks = "";
                    string gateway = "";
                    string adapterName = "";
                    string adapterMac = "";
                    NetworkAdapterDetails nic = null;
                    if (networkPrefix.Equals("ext")) nic = NetworkAdapterDetailsHelper.GetExternalNetworkAdapterDetails(vm.Id);
                    if (networkPrefix.Equals("priv")) nic = NetworkAdapterDetailsHelper.GetPrivateNetworkAdapterDetails(vm.Id);
                    if (networkPrefix.Equals("dmz")) nic = NetworkAdapterDetailsHelper.GetDmzNetworkAdapterDetails(vm.Id);
                    if (networkPrefix.Equals("mng")) nic = NetworkAdapterDetailsHelper.GetManagementNetworkAdapterDetails(vm.Id);
                    if (nic != null)
                    {
                        if (script.Contains(vAdapterName))
                        {
                            VirtualizationServer2012 vs = VirtualizationHelper.GetVirtualizationProxy(vm.ServiceId);
                            VirtualMachine vmex = vs.GetVirtualMachineEx(vm.VirtualMachineId);
                            if (vmex.Adapters != null)
                            {
                                foreach (var adapter in vmex.Adapters)
                                {
                                    if (adapter == null || String.IsNullOrEmpty(adapter.MacAddress) || String.IsNullOrEmpty(nic.MacAddress)) continue;
                                    string nicMac = nic.MacAddress.Replace("-", "");
                                    if (adapter.MacAddress.ToLower().Equals(nicMac.ToLower())) adapterName = "\"" + adapter.Name + "\"";
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(nic.MacAddress)) adapterMac = "\"" + nic.MacAddress.Replace("-", "") + "\"";
                        for (int i = 0; i < nic.IPAddresses.Length; i++)
                        {
                            string comma = ",";
                            if (i == nic.IPAddresses.Length - 1) comma = "";
                            ips += "\"" + nic.IPAddresses[i].IPAddress + "\"" + comma;
                            masks += "\"" + nic.IPAddresses[i].SubnetMask + "\"" + comma;
                            if (i == 0)
                            {
                                if (networkPrefix.Equals("priv") && !String.IsNullOrEmpty(vm.CustomPrivateGateway))
                                {
                                    gateway = "\"" + vm.CustomPrivateGateway + "\"";
                                }
                                else if (networkPrefix.Equals("dmz") && !String.IsNullOrEmpty(vm.CustomDmzGateway))
                                {
                                    gateway = "\"" + vm.CustomDmzGateway + "\"";
                                }
                                else
                                {
                                    gateway = "\"" + nic.IPAddresses[i].DefaultGateway + "\"";
                                }
                            }
                        }
                    }
                    vars += vIps + " = @(" + ips + ")" + Environment.NewLine;
                    vars += vMasks + " = @(" + masks + ")" + Environment.NewLine;
                    if (!String.IsNullOrEmpty(gateway)) vars += vGateway + " = " + gateway + Environment.NewLine;
                    if (!String.IsNullOrEmpty(adapterName)) vars += vAdapterName + " = " + adapterName + Environment.NewLine;
                    if (!String.IsNullOrEmpty(adapterMac)) vars += vAdapterMac + " = " + adapterMac + Environment.NewLine;
                }
            }
            catch (Exception) { }
        }
    }
}
