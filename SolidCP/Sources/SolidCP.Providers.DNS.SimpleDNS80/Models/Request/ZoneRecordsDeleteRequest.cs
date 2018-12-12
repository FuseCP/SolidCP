using System;
using Newtonsoft.Json;
using SolidCP.Providers.DNS.SimpleDNS80.Models.Response;

namespace SolidCP.Providers.DNS.SimpleDNS80.Models.Request
{
    public class ZoneRecordsDeleteRequest : ZoneRecordsResponse
    {
        [JsonProperty("Remove")]
        public bool Remove { get; set; }
    }
}
