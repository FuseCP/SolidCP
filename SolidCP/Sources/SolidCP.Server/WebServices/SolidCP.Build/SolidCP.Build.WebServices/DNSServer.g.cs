#if !Client
using System;
using System.ComponentModel;
using System.Globalization;
using SolidCP.Web.Services;
using SolidCP.Providers;
using SolidCP.Providers.DNS;
using SolidCP.Server.Utils;
using SolidCP.Server;
#if NETFRAMEWORK
using System.ServiceModel;
#else
using CoreWCF;
#endif

namespace SolidCP.Server.Services
{
    // wcf service contract
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
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

    // wcf service
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SolidCP.Build", "1.0")]
#if NETFRAMEWORK
[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
#endif
    public class DNSServer : SolidCP.Server.DNSServer, IDNSServer
    {
    }
}
#endif