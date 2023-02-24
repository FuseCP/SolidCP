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
#if NET6_0
using CoreWCF;
#endif
#if !NET6_0
using System.ServiceModel;
#endif

namespace SolidCP.Server.Client
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

    public class DNSServer
    {
        ChannelFactory<T> _Factory { get; set; }

        public Credentials Credentials { get; set; }

        public object SoapHeader { get; set; }

        void Test()
        {
            try
            {
                var client = _Factory.CreateChannel();
                client.MyServiceOperation();
                ((ICommunicationObject)client).Close();
                _Factory.Close();
            }
            catch
            {
                (client as ICommunicationObject)?.Abort();
            }
        }
    }
}
#endif