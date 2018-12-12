using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.Providers.DNS.SimpleDNS80.Models.Request
{
    using Newtonsoft.Json;

    public partial class SecondaryZoneRequest
    {
        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("PrimaryIP")]
        public string PrimaryIp { get; set; }
    }

    public partial class SecondaryZoneRequest
    {
        public static SecondaryZoneRequest FromJson(string json) => JsonConvert.DeserializeObject<SecondaryZoneRequest>(json, Converter.Settings);
    }
}
