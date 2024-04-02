using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using SolidCP.Providers.OS;
using SolidCP.Providers.HostedSolution;
using System.IO;

namespace SolidCP.Providers.Virtualization
{
	public class ProxmoxvpsLocal : Proxmoxvps
	{
		protected override string ProxmoxClusterServerApiHost => "localhost";
		protected override string ProxmoxClusterNode => Environment.MachineName;
		public override VirtualMachine CreateVirtualMachine(VirtualMachine vm)
		{
			string sshcmd = String.Format("{0} {1} {2}", DeploySSHScriptSettings, DeploySSHScriptParamsSettings, vm.OperatingSystemTemplateDeployParams);

			sshcmd = sshcmd.Replace("[FQDN]", vm.Name);
			sshcmd = sshcmd.Replace("[CPUCORES]", vm.CpuCores.ToString());
			sshcmd = sshcmd.Replace("[RAMSIZE]", vm.RamSize.ToString());
			sshcmd = sshcmd.Replace("[HDDSIZE]", vm.HddSize[0].ToString());
			sshcmd = sshcmd.Replace("[OSTEMPLATENAME]", $"\"{vm.OperatingSystemTemplate}\"");
			sshcmd = sshcmd.Replace("[OSTEMPLATEFILE]", $"\"{vm.OperatingSystemTemplatePath}\"");
			sshcmd = sshcmd.Replace("[ADMINPASS]", $"\"{vm.AdministratorPassword}\"");
			sshcmd = sshcmd.Replace("[VLAN]", vm.DefaultAccessVlan.ToString());
			sshcmd = sshcmd.Replace("[MAC]", vm.ExternalNicMacAddress);
			if (vm.ExternalNetworkEnabled)
			{
				sshcmd = sshcmd.Replace("[IP]", vm.PrimaryIP.IPAddress);
				sshcmd = sshcmd.Replace("[NETMASK]", vm.PrimaryIP.SubnetMask);
				sshcmd = sshcmd.Replace("[GATEWAY]", vm.PrimaryIP.DefaultGateway);
			}
			else
			{
				sshcmd = sshcmd.Replace("[IP]", "\"\"");
				sshcmd = sshcmd.Replace("[NETMASK]", "\"\"");
				sshcmd = sshcmd.Replace("[GATEWAY]", "\"\"");
			}

			string error = "Error creating wirtual machine.";
			try
			{
				var output = Shell.Default.Exec(sshcmd).Output().Result;
				error = $"Error creating wirtual machine. VM deploy script output:\n{output}";

				// Get created machine Id
				vm.Name = vm.Name.Split('.')[0];
				var createdMachine = GetVirtualMachines().FirstOrDefault(m => m.Name == vm.Name);
				if (createdMachine == null)
				{
					error = $"Can't find created machine. VM deploy script output:\n{output}";
					var ex = new Exception(error);

					HostedSolutionLog.LogError(error, ex);
					throw ex;
				}
				vm.VirtualMachineId = createdMachine.VirtualMachineId;

				// Update common settings
				UpdateVirtualMachine(vm);
			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError(error, ex);
				throw;
			}

			return vm;
		}

        public override Stream GetFile(string vmId, string filename, bool delete = false)
        {
			var mem = new MemoryStream(); 
			using (var stream = File.OpenRead(filename)) stream.CopyTo(mem);
			if (delete) File.Delete(filename);
			mem.Seek(0, SeekOrigin.Begin);
			return mem;
        }

		public override int CreatePveApiSshTunnel()
		{
			return int.Parse(ProxmoxClusterServerPort);
		}

        public string GetInstalledVersion()
		{
			try
			{
				var output = Shell.Default.Exec($"pveversion -v").Output().Result;
				var match = Regex.Match(output, @"(?<=^proxmox-ve:\s*)(?<version>[0-9][0-9.]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				if (match.Success)
				{
					return match.Groups["version"].Value;
				}
			}
			catch { }

			return "";
		}
		public override bool IsInstalled() => !string.IsNullOrEmpty(GetInstalledVersion());
	}
}