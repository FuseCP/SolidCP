#if Client
using System.Linq;
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf client contract
    [SolidCP.Web.Client.HasPolicy("ServerPolicy")]
    [SolidCP.Providers.SoapHeader]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    [ServiceContract(ConfigurationName = "IDNSServer", Namespace = "http://smbsaas/solidcp/server/")]
    public interface IDNSServer
    {
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/ZoneExists", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/ZoneExistsResponse")]
        bool ZoneExists(string zoneName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/ZoneExists", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/ZoneExistsResponse")]
        System.Threading.Tasks.Task<bool> ZoneExistsAsync(string zoneName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/GetZones", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/GetZonesResponse")]
        string[] GetZones();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/GetZones", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/GetZonesResponse")]
        System.Threading.Tasks.Task<string[]> GetZonesAsync();
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/AddPrimaryZone", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/AddPrimaryZoneResponse")]
        void AddPrimaryZone(string zoneName, string[] secondaryServers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/AddPrimaryZone", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/AddPrimaryZoneResponse")]
        System.Threading.Tasks.Task AddPrimaryZoneAsync(string zoneName, string[] secondaryServers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/AddSecondaryZone", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/AddSecondaryZoneResponse")]
        void AddSecondaryZone(string zoneName, string[] masterServers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/AddSecondaryZone", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/AddSecondaryZoneResponse")]
        System.Threading.Tasks.Task AddSecondaryZoneAsync(string zoneName, string[] masterServers);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/DeleteZone", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/DeleteZoneResponse")]
        void DeleteZone(string zoneName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/DeleteZone", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/DeleteZoneResponse")]
        System.Threading.Tasks.Task DeleteZoneAsync(string zoneName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/UpdateSoaRecord", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/UpdateSoaRecordResponse")]
        void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/UpdateSoaRecord", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/UpdateSoaRecordResponse")]
        System.Threading.Tasks.Task UpdateSoaRecordAsync(string zoneName, string host, string primaryNsServer, string primaryPerson);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/GetZoneRecords", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/GetZoneRecordsResponse")]
        SolidCP.Providers.DNS.DnsRecord[] GetZoneRecords(string zoneName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/GetZoneRecords", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/GetZoneRecordsResponse")]
        System.Threading.Tasks.Task<SolidCP.Providers.DNS.DnsRecord[]> GetZoneRecordsAsync(string zoneName);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/AddZoneRecord", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/AddZoneRecordResponse")]
        void AddZoneRecord(string zoneName, SolidCP.Providers.DNS.DnsRecord record);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/AddZoneRecord", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/AddZoneRecordResponse")]
        System.Threading.Tasks.Task AddZoneRecordAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord record);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/DeleteZoneRecord", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/DeleteZoneRecordResponse")]
        void DeleteZoneRecord(string zoneName, SolidCP.Providers.DNS.DnsRecord record);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/DeleteZoneRecord", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/DeleteZoneRecordResponse")]
        System.Threading.Tasks.Task DeleteZoneRecordAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord record);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/AddZoneRecords", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/AddZoneRecordsResponse")]
        void AddZoneRecords(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/AddZoneRecords", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/AddZoneRecordsResponse")]
        System.Threading.Tasks.Task AddZoneRecordsAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/DeleteZoneRecords", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/DeleteZoneRecordsResponse")]
        void DeleteZoneRecords(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records);
        [OperationContract(Action = "http://smbsaas/solidcp/server/IDNSServer/DeleteZoneRecords", ReplyAction = "http://smbsaas/solidcp/server/IDNSServer/DeleteZoneRecordsResponse")]
        System.Threading.Tasks.Task DeleteZoneRecordsAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records);
    }

    // wcf client assembly proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class DNSServerAssemblyClient : SolidCP.Web.Client.ClientAssemblyBase, IDNSServer
    {
        public bool ZoneExists(string zoneName)
        {
            return Invoke<bool>("SolidCP.Server.DNSServer", "ZoneExists", zoneName);
        }

        public async System.Threading.Tasks.Task<bool> ZoneExistsAsync(string zoneName)
        {
            return await InvokeAsync<bool>("SolidCP.Server.DNSServer", "ZoneExists", zoneName);
        }

        public string[] GetZones()
        {
            return Invoke<string[]>("SolidCP.Server.DNSServer", "GetZones");
        }

        public async System.Threading.Tasks.Task<string[]> GetZonesAsync()
        {
            return await InvokeAsync<string[]>("SolidCP.Server.DNSServer", "GetZones");
        }

        public void AddPrimaryZone(string zoneName, string[] secondaryServers)
        {
            Invoke("SolidCP.Server.DNSServer", "AddPrimaryZone", zoneName, secondaryServers);
        }

        public async System.Threading.Tasks.Task AddPrimaryZoneAsync(string zoneName, string[] secondaryServers)
        {
            await InvokeAsync("SolidCP.Server.DNSServer", "AddPrimaryZone", zoneName, secondaryServers);
        }

        public void AddSecondaryZone(string zoneName, string[] masterServers)
        {
            Invoke("SolidCP.Server.DNSServer", "AddSecondaryZone", zoneName, masterServers);
        }

        public async System.Threading.Tasks.Task AddSecondaryZoneAsync(string zoneName, string[] masterServers)
        {
            await InvokeAsync("SolidCP.Server.DNSServer", "AddSecondaryZone", zoneName, masterServers);
        }

        public void DeleteZone(string zoneName)
        {
            Invoke("SolidCP.Server.DNSServer", "DeleteZone", zoneName);
        }

        public async System.Threading.Tasks.Task DeleteZoneAsync(string zoneName)
        {
            await InvokeAsync("SolidCP.Server.DNSServer", "DeleteZone", zoneName);
        }

        public void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson)
        {
            Invoke("SolidCP.Server.DNSServer", "UpdateSoaRecord", zoneName, host, primaryNsServer, primaryPerson);
        }

        public async System.Threading.Tasks.Task UpdateSoaRecordAsync(string zoneName, string host, string primaryNsServer, string primaryPerson)
        {
            await InvokeAsync("SolidCP.Server.DNSServer", "UpdateSoaRecord", zoneName, host, primaryNsServer, primaryPerson);
        }

        public SolidCP.Providers.DNS.DnsRecord[] GetZoneRecords(string zoneName)
        {
            return Invoke<SolidCP.Providers.DNS.DnsRecord[]>("SolidCP.Server.DNSServer", "GetZoneRecords", zoneName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.DNS.DnsRecord[]> GetZoneRecordsAsync(string zoneName)
        {
            return await InvokeAsync<SolidCP.Providers.DNS.DnsRecord[]>("SolidCP.Server.DNSServer", "GetZoneRecords", zoneName);
        }

        public void AddZoneRecord(string zoneName, SolidCP.Providers.DNS.DnsRecord record)
        {
            Invoke("SolidCP.Server.DNSServer", "AddZoneRecord", zoneName, record);
        }

        public async System.Threading.Tasks.Task AddZoneRecordAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord record)
        {
            await InvokeAsync("SolidCP.Server.DNSServer", "AddZoneRecord", zoneName, record);
        }

        public void DeleteZoneRecord(string zoneName, SolidCP.Providers.DNS.DnsRecord record)
        {
            Invoke("SolidCP.Server.DNSServer", "DeleteZoneRecord", zoneName, record);
        }

        public async System.Threading.Tasks.Task DeleteZoneRecordAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord record)
        {
            await InvokeAsync("SolidCP.Server.DNSServer", "DeleteZoneRecord", zoneName, record);
        }

        public void AddZoneRecords(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records)
        {
            Invoke("SolidCP.Server.DNSServer", "AddZoneRecords", zoneName, records);
        }

        public async System.Threading.Tasks.Task AddZoneRecordsAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records)
        {
            await InvokeAsync("SolidCP.Server.DNSServer", "AddZoneRecords", zoneName, records);
        }

        public void DeleteZoneRecords(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records)
        {
            Invoke("SolidCP.Server.DNSServer", "DeleteZoneRecords", zoneName, records);
        }

        public async System.Threading.Tasks.Task DeleteZoneRecordsAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records)
        {
            await InvokeAsync("SolidCP.Server.DNSServer", "DeleteZoneRecords", zoneName, records);
        }
    }

    // wcf client proxy class
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
    public class DNSServer : SolidCP.Web.Client.ClientBase<IDNSServer, DNSServerAssemblyClient>, IDNSServer
    {
        public bool ZoneExists(string zoneName)
        {
            return base.Client.ZoneExists(zoneName);
        }

        public async System.Threading.Tasks.Task<bool> ZoneExistsAsync(string zoneName)
        {
            return await base.Client.ZoneExistsAsync(zoneName);
        }

        public string[] GetZones()
        {
            return base.Client.GetZones();
        }

        public async System.Threading.Tasks.Task<string[]> GetZonesAsync()
        {
            return await base.Client.GetZonesAsync();
        }

        public void AddPrimaryZone(string zoneName, string[] secondaryServers)
        {
            base.Client.AddPrimaryZone(zoneName, secondaryServers);
        }

        public async System.Threading.Tasks.Task AddPrimaryZoneAsync(string zoneName, string[] secondaryServers)
        {
            await base.Client.AddPrimaryZoneAsync(zoneName, secondaryServers);
        }

        public void AddSecondaryZone(string zoneName, string[] masterServers)
        {
            base.Client.AddSecondaryZone(zoneName, masterServers);
        }

        public async System.Threading.Tasks.Task AddSecondaryZoneAsync(string zoneName, string[] masterServers)
        {
            await base.Client.AddSecondaryZoneAsync(zoneName, masterServers);
        }

        public void DeleteZone(string zoneName)
        {
            base.Client.DeleteZone(zoneName);
        }

        public async System.Threading.Tasks.Task DeleteZoneAsync(string zoneName)
        {
            await base.Client.DeleteZoneAsync(zoneName);
        }

        public void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson)
        {
            base.Client.UpdateSoaRecord(zoneName, host, primaryNsServer, primaryPerson);
        }

        public async System.Threading.Tasks.Task UpdateSoaRecordAsync(string zoneName, string host, string primaryNsServer, string primaryPerson)
        {
            await base.Client.UpdateSoaRecordAsync(zoneName, host, primaryNsServer, primaryPerson);
        }

        public SolidCP.Providers.DNS.DnsRecord[] GetZoneRecords(string zoneName)
        {
            return base.Client.GetZoneRecords(zoneName);
        }

        public async System.Threading.Tasks.Task<SolidCP.Providers.DNS.DnsRecord[]> GetZoneRecordsAsync(string zoneName)
        {
            return await base.Client.GetZoneRecordsAsync(zoneName);
        }

        public void AddZoneRecord(string zoneName, SolidCP.Providers.DNS.DnsRecord record)
        {
            base.Client.AddZoneRecord(zoneName, record);
        }

        public async System.Threading.Tasks.Task AddZoneRecordAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord record)
        {
            await base.Client.AddZoneRecordAsync(zoneName, record);
        }

        public void DeleteZoneRecord(string zoneName, SolidCP.Providers.DNS.DnsRecord record)
        {
            base.Client.DeleteZoneRecord(zoneName, record);
        }

        public async System.Threading.Tasks.Task DeleteZoneRecordAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord record)
        {
            await base.Client.DeleteZoneRecordAsync(zoneName, record);
        }

        public void AddZoneRecords(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records)
        {
            base.Client.AddZoneRecords(zoneName, records);
        }

        public async System.Threading.Tasks.Task AddZoneRecordsAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records)
        {
            await base.Client.AddZoneRecordsAsync(zoneName, records);
        }

        public void DeleteZoneRecords(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records)
        {
            base.Client.DeleteZoneRecords(zoneName, records);
        }

        public async System.Threading.Tasks.Task DeleteZoneRecordsAsync(string zoneName, SolidCP.Providers.DNS.DnsRecord[] records)
        {
            await base.Client.DeleteZoneRecordsAsync(zoneName, records);
        }
    }
}
#endif