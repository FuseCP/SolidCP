using System.Management.Automation.Runspaces;

namespace SolidCP.Providers.Virtualization
{
    public static class ReplicaHelper
    {
        public static void SetReplicaServer(PowerShellManager powerShell, bool enabled, string remoteServer, string thumbprint, string storagePath)
        {
            Command cmd = new Command("Set-VMReplicationServer");
            cmd.Parameters.Add("ReplicationEnabled", enabled);

            if (!string.IsNullOrEmpty(remoteServer))
            {
                cmd.Parameters.Add("ComputerName", remoteServer);
            }

            if (!string.IsNullOrEmpty(thumbprint))
            {
                cmd.Parameters.Add("AllowedAuthenticationType", "Certificate");
                cmd.Parameters.Add("CertificateThumbprint", thumbprint);
            }

            if (!string.IsNullOrEmpty(storagePath))
            {
                cmd.Parameters.Add("ReplicationAllowedFromAnyServer", true);
                cmd.Parameters.Add("DefaultStorageLocation", storagePath);
            }

            powerShell.Execute(cmd, false);
        }

        public static void SetFirewallRule(PowerShellManager powerShell, bool enabled)
        {
            Command cmd = new Command("Enable-Netfirewallrule");
            cmd.Parameters.Add("DisplayName", "Hyper-V Replica HTTPS Listener (TCP-In)");

            powerShell.Execute(cmd, false);
        }

        public static void RemoveVmReplication(PowerShellManager powerShell, string vmName, string server)
        {
            Command cmd = new Command("Remove-VMReplication");
            cmd.Parameters.Add("VmName", vmName);
            if (!string.IsNullOrEmpty(server)) cmd.Parameters.Add("ComputerName", server);

            powerShell.Execute(cmd, false);
        }
    }
}
