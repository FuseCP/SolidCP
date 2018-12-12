using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SolidCP.Providers.DNS.SimpleDNS80.Models.Request;
using SolidCP.Providers.DNS.SimpleDNS80.Models.Response;

namespace SolidCP.Providers.DNS.SimpleDNS80.Models
{
    public static class Serialize
    {
        public static string ToJson(this StatisticsResponse self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this SecondaryZoneRequest self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this List<ZoneRecordsResponse> self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this ZoneRecordsResponse self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this ZoneRecordsDeleteRequest self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);

        public static string ToJson(this List<ZoneRecordsDeleteRequest> self) =>
            JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
