#if Client
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
using System.ServiceModel;

namespace SolidCP.Server.Client
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [ServiceContract]
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
}
#endif