#if !Client
using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Services;
using System.Web.Services.Protocols;
using SolidCP.Providers;
using SolidCP.Providers.DNS;
using SolidCP.Server.Utils;
using Microsoft.Web.Services3;
using SolidCP.Server;
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Services
{
    /// <summary>
    /// Summary description for DNSServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract(Namespace = "http://smbsaas/solidcp/server/")]
    public interface IDNSServer
    {

        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        bool ZoneExists(string zoneName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        string[] GetZones();
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void AddPrimaryZone(string zoneName, string[] secondaryServers);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void AddSecondaryZone(string zoneName, string[] masterServers);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteZone(string zoneName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson);


        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        DnsRecord[] GetZoneRecords(string zoneName);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void AddZoneRecord(string zoneName, DnsRecord record);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteZoneRecord(string zoneName, DnsRecord record);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void AddZoneRecords(string zoneName, DnsRecord[] records);
        [WebMethod, SoapHeader("settings")]
        [OperationContract]
        void DeleteZoneRecords(string zoneName, DnsRecord[] records);
    }

    public class DNSServerService : SolidCP.Server.DNSServer, IDNSServer
    {
        public new bool ZoneExists(string zoneName)
        {
            return base.ZoneExists(zoneName);
        }

        public new string[] GetZones()
        {
            return base.GetZones();
        }

        public new void AddPrimaryZone(string zoneName, string[] secondaryServers)
        {
            base.AddPrimaryZone(zoneName, secondaryServers);
        }

        public new void AddSecondaryZone(string zoneName, string[] masterServers)
        {
            base.AddSecondaryZone(zoneName, masterServers);
        }

        public new void DeleteZone(string zoneName)
        {
            base.DeleteZone(zoneName);
        }

        public new void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson)
        {
            base.UpdateSoaRecord(zoneName, host, primaryNsServer, primaryPerson);
        }

        public new DnsRecord[] GetZoneRecords(string zoneName)
        {
            return base.GetZoneRecords(zoneName);
        }

        public new void AddZoneRecord(string zoneName, DnsRecord record)
        {
            base.AddZoneRecord(zoneName, record);
        }

        public new void DeleteZoneRecord(string zoneName, DnsRecord record)
        {
            base.DeleteZoneRecord(zoneName, record);
        }

        public new void AddZoneRecords(string zoneName, DnsRecord[] records)
        {
            base.AddZoneRecords(zoneName, records);
        }

        public new void DeleteZoneRecords(string zoneName, DnsRecord[] records)
        {
            base.DeleteZoneRecords(zoneName, records);
        }
    }
}
#endif