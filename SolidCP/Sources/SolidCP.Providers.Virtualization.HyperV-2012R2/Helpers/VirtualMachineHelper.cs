﻿using Microsoft.Management.Infrastructure;
using SolidCP.Providers.HostedSolution;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.Virtualization
{
    public class VirtualMachineHelper
    {
        private PowerShellManager _powerShell;
        private MiManager _mi;

        public VirtualMachineHelper(PowerShellManager powerShellManager, MiManager mi)
        {
            _powerShell = powerShellManager;
            _mi = mi;
        }

        public VirtualMachine CreateVirtualMachineFromCimInstance(CimInstance cimInsVm)
        {
            if (cimInsVm == null || cimInsVm.CimInstanceProperties.Count == 0)
                return null;

            var objVm = cimInsVm.CimInstanceProperties;

            VirtualMachine vm = new VirtualMachine();
            vm.VirtualMachineId = (string)objVm["Name"].Value;
            vm.Name = (string)objVm["ElementName"].Value;
            vm.State = (VirtualMachineState)Convert.ToInt32(objVm["EnabledState"].Value);
            vm.Status = Convert.ToString(objVm["Status"].Value);
            vm.Uptime = Convert.ToInt64(objVm["OnTimeInMilliseconds"].Value);
            return vm;
        }

        public CimInstance GetVirtualMachineSettingsObject(string vmId)
        {
            using (CimInstance vmInstance = _mi.GetCimInstance("Msvm_ComputerSystem", "Name = '{0}'", vmId)){
                return _mi.GetAssociatedCimInstance(vmInstance, "Msvm_VirtualSystemSettingData", "Msvm_SettingsDefineState"); //Optimizated query
                //CimInstance settingData = GetCimInstance("Msvm_VirtualSystemSettingData", "InstanceID Like 'Microsoft:{0}%'", vmId); //this is slower ~6 times that two the above query
            }
        }

        public CimInstance GetSummaryInformation(CimInstance settingData, params SummaryInformationRequest[] requestedInformation)
        {
            if (requestedInformation == null || requestedInformation.Length == 0) 
                HostedSolutionLog.LogWarning("At least one SummaryInformationRequest must be provided.", nameof(requestedInformation));

            uint[] reqif = new uint[requestedInformation.Length];
            for (int i = 0; i < requestedInformation.Length; i++)
                reqif[i] = (uint)requestedInformation[i];

            using (CimInstance managementService = _mi.GetCimInstance("Msvm_VirtualSystemManagementService"))
            {
                if (managementService == null)
                    HostedSolutionLog.LogWarning("Failed to retrieve Msvm_VirtualSystemManagementService instance.");

                using (var inParams = new CimMethodParametersCollection
                {
                    CimMethodParameter.Create("SettingData", new CimInstance[] { settingData }, CimType.ReferenceArray, CimFlags.In),
                    CimMethodParameter.Create("RequestedInformation", reqif, CimFlags.In),
                })
                using (CimMethodResult result = _mi.InvokeMethod(managementService, "GetSummaryInformation", inParams)) 
                {
                    if (result.ReturnValue.Value is uint retVal && retVal != 0)
                        HostedSolutionLog.LogError(new Exception($"GetSummaryInformation returned error code: {retVal}"));

                    object summaryObj = result.OutParameters["SummaryInformation"].Value;
                    if (summaryObj is CimInstance[] summaryArray) {
                        return summaryArray.FirstOrDefault();
                    } else {
                        return null;
                    }
                }                
            }            
        }
                
        public PSObject GetVmPSObject(string vmId) //if any Powershell command accept VM object WE MUST use it as it extremely faster
        {
            Command cmd = new Command("Get-VM");
            cmd.Parameters.Add("Id", vmId);
            Collection<PSObject> result = _powerShell.Execute(cmd, true, true);
            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            return null;
        }

        public OperationalStatus GetVMHeartBeatStatusFromGetVmResult(PSObject result, string vmId)
        {
            OperationalStatus status = OperationalStatus.None;
            try
            {
                var statusString = TryToGetStatusString(result, "PrimaryOperationalStatus");
                status = (OperationalStatus)Enum.Parse(typeof(OperationalStatus), statusString);
            }
            catch
            {
                HostedSolution.HostedSolutionLog.LogWarning("GetVMHeartBeatStatus: can not get OperationalStatus from Get-VM result");
                status = GetVMHeartBeatStatus(vmId); //try to get it again, but from CIM/Mi
            }

            return status;

        }

        public OperationalStatus GetVMHeartBeatStatus(string vmId)
        {
            OperationalStatus status = OperationalStatus.None;
            try
            {
                using (CimInstance vmSettings = GetVirtualMachineSettingsObject(vmId))
                using (CimInstance cimSummary = GetSummaryInformation(vmSettings, SummaryInformationRequest.Heartbeat))
                {
                    status = (OperationalStatus)Convert.ToInt32(cimSummary.CimInstanceProperties["Heartbeat"].Value);
                }
            }
            catch
            {
                //Nothing to do here — the panel works so fast that we can even get an exception while fetching the VM Heartbeat :D
            }

            return status;
        }

        private string TryToGetStatusString(PSObject result, string propertyName, bool isLast = false) //burn in hell MS for your bugs!
        {
            string status = null;
            try
            {
                var statusString = result.GetProperty(propertyName);
                if (statusString != null)
                    status = statusString.ToString();
            }
            catch
            {
                if(!isLast)
                    status = ConvertToOperationalStatusTypeString(TryToGetStatusString(result, "PrimaryStatusDescription", true));
                else
                    HostedSolution.HostedSolutionLog.LogWarning("GetVMHeartBeatStatus: can not get OperationalStatus");
            }
            
            return status;
        }

        private string ConvertToOperationalStatusTypeString(string str)
        {
            switch (str)
            {
                case "OK":
                    {
                        str = "Ok";
                        break;
                    }
                case "No Contact":
                    {
                        str = "NoContact";
                        break;
                    }
                case "Lost Communication":
                    {
                        str = "LostCommunication";
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return str;
        }

        public int GetVMProcessors(string vmId)
        {
            int procs = 0;
            using(CimInstance vmSettings = GetVirtualMachineSettingsObject(vmId))
            using(CimInstance cimSummary = GetSummaryInformation(vmSettings, SummaryInformationRequest.NumberOfProcessors))
            {
                procs = Convert.ToInt32(cimSummary.CimInstanceProperties["NumberOfProcessors"].Value);                
            }
            return procs;
        }

        public int GetVMProcessorsPS(string name) //22 time slower than CIM GetVMProcessors
        {
            int procs = 0;

            Command cmd = new Command("Get-VMProcessor");

            cmd.Parameters.Add("VMName", name);

            Collection<PSObject> result = _powerShell.Execute(cmd, true);
            if (result != null && result.Count > 0)
            {
                procs = Convert.ToInt32(result[0].GetProperty("Count"));

            }
            return procs;
        }

        public void UpdateProcessors(VirtualMachineData vmData, int cpuCores, int cpuLimitSettings, int cpuReserveSettings, int cpuWeightSettings)
        {
            Command cmd = new Command("Set-VMProcessor");

            cmd.Parameters.Add("Count", cpuCores);
            cmd.Parameters.Add("Maximum", cpuLimitSettings);
            cmd.Parameters.Add("Reserve", cpuReserveSettings);
            cmd.Parameters.Add("RelativeWeight", cpuWeightSettings);

            _powerShell.ExecuteOnVm(cmd, vmData);
        }

        public void Delete(VirtualMachineData vmData, string vmId, string clusterName)
        {
            if (!String.IsNullOrEmpty(clusterName))
            {
                Command cmdCluster = new Command("Remove-ClusterGroup");
                cmdCluster.Parameters.Add("VMId", vmId);
                cmdCluster.Parameters.Add("RemoveResources");
                cmdCluster.Parameters.Add("Force");
                _powerShell.Execute(cmdCluster, false);
            }
            Command cmd = new Command("Remove-VM");
            cmd.Parameters.Add("Force");
            _powerShell.ExecuteOnVm(cmd, vmData, true);
        }

        public void Stop(VirtualMachineData vmData, bool force)
        {
            Command cmd = new Command("Stop-VM");

            if (force) cmd.Parameters.Add("Force");
            //if (!string.IsNullOrEmpty(reason)) cmd.Parameters.Add("Reason", reason);
            try
            {
                _powerShell.ExecuteOnVm(cmd, vmData, true);
            }
            catch
            {
                cmd = new Command("Stop-VM");
                cmd.Parameters.Add("TurnOff");
                _powerShell.ExecuteOnVm(cmd, vmData);
            }
            
        }

        protected VirtualMachine GetVirtualMachineCim(string vmId, bool extendedInfo) //examples of MI
        {
            CimInstance cimVm = _mi.GetCimInstanceWithSelect(
                "Msvm_ComputerSystem",
                "Name, ElementName, EnabledState, Status, OnTimeInMilliseconds",
                "Name = '{0}'", vmId); //Name = "GUID"
            VirtualMachine vm = this.CreateVirtualMachineFromCimInstance(cimVm);

            if (vm != null)
            {
                CimInstance cimSettings = _mi.GetAssociatedCimInstance(cimVm, "Msvm_VirtualSystemSettingData", "Msvm_SettingsDefineState");
                var objSettings = cimSettings.CimInstanceProperties;

                CimInstance cimSummary = GetSummaryInformation(cimSettings,
                    SummaryInformationRequest.Heartbeat,
                    SummaryInformationRequest.MemoryUsage,
                    SummaryInformationRequest.ProcessorLoad,
                    SummaryInformationRequest.NumberOfProcessors);
                var objSummary = cimSummary.CimInstanceProperties;

                vm.CpuUsage = Convert.ToInt32(objSummary["ProcessorLoad"].Value);
                vm.Version = Convert.ToString(objSettings["Version"].Value);

                if (Convert.ToDouble(vm.Version) > Convert.ToDouble(Constants.ConfigurationVersion))
                    vm.RamUsage = Convert.ToInt32(Convert.ToUInt64(objSummary["MemoryUsage"].Value) / Constants.Size1M);
                else
                    vm.RamUsage = Convert.ToInt32(Convert.ToUInt64(objSummary["MemoryAvailable"].Value) / Constants.Size1M);

                CimInstance cimRamInfo = _mi.GetCimInstanceWithSelect(
                    "Msvm_MemorySettingData",
                    "VirtualQuantity",
                    "InstanceID Like 'Microsoft:{0}%'", vmId);
                var ramInfo = cimRamInfo.CimInstanceProperties;
                vm.RamSize = Convert.ToInt32(ramInfo["VirtualQuantity"].Value);

                bool isDynamicMemoryEnabled = Convert.ToBoolean(ramInfo["DynamicMemoryEnabled"].Value);

                vm.Generation = Convert.ToString(objSettings["VirtualSystemSubType"].Value).EndsWith(":2") ? 2 : 1;
                vm.ProcessorCount = Convert.ToInt32(objSummary["NumberOfProcessors"].Value);

                string parentRelPath = Convert.ToString(objSettings["Parent"].Value);
                if (!string.IsNullOrEmpty(parentRelPath))
                {
                    vm.ParentSnapshotId = parentRelPath.Split(new[] { "InstanceID=\"" }, StringSplitOptions.None)[1].Split('"')[0].Split(':')[1];
                }

                vm.Heartbeat = (OperationalStatus)Convert.ToInt32(objSummary["Heartbeat"].Value);
                vm.CreatedDate = (System.DateTime)objSettings["CreationTime"].Value;

                vm.ReplicationState = (ReplicationState)Convert.ToInt32(objSummary["ReplicationState"].Value);

                //var commands = new List<Command>
                //{
                //    new Command("Get-VM") { Parameters = { { "Id", vmId } } },
                //    new Command("Select-Object") { Parameters = { { "Property", new string[] { "IsClustered" } } } }
                //};
                //Collection<PSObject> result = PowerShell.Execute(commands, true);
                //vm.IsClustered = result[0].GetBool("IsClustered");

                //if (extendedInfo)
                //{

                //}

                //if (isDynamicMemoryEnabled)
                //{
                //    vm.DynamicMemory = MemoryHelper.GetDynamicMemory(PowerShell, vm.Name);
                //}

            }

            return vm;
        }
    }
}
