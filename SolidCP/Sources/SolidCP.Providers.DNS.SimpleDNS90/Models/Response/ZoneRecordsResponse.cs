// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace SolidCP.Providers.DNS.SimpleDNS90.Models.Response
{
    public partial class ZoneRecordsResponse
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("TTL")]
        public long? TTL { get; set; }

        [JsonProperty("Data")]
        public string Data { get; set; }

        [JsonProperty("Comment", NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }
    }

    public partial class ZoneRecordsResponse
    {
        public static List<ZoneRecordsResponse> FromJson(string json) => JsonConvert.DeserializeObject<List<ZoneRecordsResponse>>(json, Converter.Settings);
    }

    public static class ZoneRecordsResponseExtensions
    {
        public static DnsRecord[] ToDnsRecordArray(this List<ZoneRecordsResponse> records, string zoneName)
        {
            //Declare the result
            var dnsRecords = new List<DnsRecord>();

            //Loop through each record in the list
            foreach (var record in records)
            {
                //Convert the ZoneRecordsResponse to DnsRecord
                dnsRecords.Add(ZoneRecordResponseToDnsRecord(record, zoneName));
            }

            //Return the array of DnsRecords
            return dnsRecords.ToArray();
        }

        public static ZoneRecordsResponse ToZoneRecordsResponse(this DnsRecord record, string zoneName)
        {
            //Declare the result
            var response = new ZoneRecordsResponse();

            //Check that the record is in SDNS format
            if (!record.RecordName.Contains(zoneName) && record.RecordName.Length > 0)
                record.RecordName = $"{record.RecordName}.{zoneName}";

            if (record.RecordName == "")
                record.RecordName = $"{zoneName}";

            //Build up the response
            response.Name = record.RecordName;
            response.Type = record.RecordType.ToString();
            response.TTL = record.RecordTTL;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (record.RecordType)
            {
                case DnsRecordType.MX:
                    if (!record.RecordData.EndsWith(".") && record.RecordData.Length > 0)
                    {
                        response.Data = $"{record.MxPriority} {record.RecordData}.";
                        break;
                    }
                    response.Data = $"{record.MxPriority} {record.RecordData}";
                    break;
                case DnsRecordType.SRV:
                    if (!record.RecordData.EndsWith(".") && record.RecordData.Length > 0)
                    {
                        response.Data = $"{record.SrvPriority} {record.SrvWeight} {record.SrvPort} {record.RecordData}.";
                        break;
                    }
                    response.Data = $"{record.SrvPriority} {record.SrvWeight} {record.SrvPort} {record.RecordData}";
                    break;
                case DnsRecordType.CNAME:
                case DnsRecordType.NS:
                    if (!record.RecordData.EndsWith(".") && record.RecordData.Length > 0) {
                        response.Data = $"{record.RecordData}.";
                        break;
                    }
                    response.Data = $"{record.RecordData}";
                    break;
                case DnsRecordType.TXT:
                    if (!record.RecordData.StartsWith("\"") && record.RecordData.Length > 0)
                    {
                        response.Data = $"\"{record.RecordData}\"";
                        break;
                    }
                    response.Data = record.RecordData ?? "";
                    break;
                default:
                    response.Data = record.RecordData ?? "";
                    break;
            }
            //Return the response
            return response;
        }

        /// <summary>
        /// Method to convert <see cref="ZoneRecordsResponse"/> to <see cref="DnsRecord"/>
        /// </summary>
        /// <param name="record">DNS Record in <see cref="ZoneRecordsResponse"/> format</param>
        private static DnsRecord ZoneRecordResponseToDnsRecord(ZoneRecordsResponse record, string zoneName)
        {
            //Null checking
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            //Declare result variable
            var resultRecord = new DnsRecord();

            //Switch on the type
            switch (record.Type)
            {
                case "A":
                    resultRecord.RecordType = DnsRecordType.A;
                    break;
                case "AAAA":
                    resultRecord.RecordType = DnsRecordType.AAAA;
                    break;
                case "CAA":
                    resultRecord.RecordType = DnsRecordType.CAA;
                    break;
                case "CNAME":
                    resultRecord.RecordType = DnsRecordType.CNAME;
                    resultRecord.RecordData = record.Data.TrimEnd('.');
                    break;
                case "MX":
                    resultRecord.RecordType = DnsRecordType.MX;
                    resultRecord.MxPriority = Convert.ToInt32(record.Data.Split(' ')[0]);
                    resultRecord.RecordData = record.Data.Split(' ')[1].TrimEnd('.');
                    break;
                case "NS":
                    resultRecord.RecordType = DnsRecordType.NS;
                    resultRecord.RecordData = record.Data.TrimEnd('.');
                    break;
                case "SOA":
                    resultRecord.RecordType = DnsRecordType.SOA;
                    break;
                case "SRV":
                    resultRecord.RecordType = DnsRecordType.SRV;
                    resultRecord.SrvPriority = Convert.ToInt32(record.Data.Split(' ')[0]);
                    resultRecord.SrvWeight = Convert.ToInt32(record.Data.Split(' ')[1]);
                    resultRecord.SrvPort = Convert.ToInt32(record.Data.Split(' ')[2]);
                    resultRecord.RecordData = record.Data.Split(' ')[3].TrimEnd('.');
                    break;
                case "TXT":
                    resultRecord.RecordType = DnsRecordType.TXT;
                    break;
                default:
                    resultRecord.RecordType = DnsRecordType.Other;
                    break;
            }

            //Build up the rest of the record
            //If data is already set, don't change it
            if (string.IsNullOrWhiteSpace(resultRecord.RecordData))
                resultRecord.RecordData = record.Data;
            //Build the remaining fields of the record
            resultRecord.RecordName = record.Name;
            resultRecord.RecordText = $"{record.Name}\t{record.TTL}\t{record.Type}\t{record.Data}";
            resultRecord.RecordTTL = (int)record.TTL;

            //Return the result
            return resultRecord;
        }
    }
}
