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
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.Management.Infrastructure;
using System.Collections.Generic;

namespace SolidCP.Providers.DNS
{
    /// <summary>This class wraps MS DNS server PowerShell commands used by the SolidCP.</summary>
    internal static class DnsCommands
    {
        /// <summary>Add parameter to PS command</summary>
        /// <param name="cmd">command</param>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Same command</returns>
        private static Command addParam(this Command cmd, string name, object value)
        {
            cmd.Parameters.Add(name, value);
            return cmd;
        }

        /// <summary>Add parameter without value to the PS command</summary>
        /// <param name="cmd">command</param>
        /// <param name="name">Parameter name</param>
        /// <returns>Same command</returns>
        private static Command addParam(this Command cmd, string name)
        {
            // http://stackoverflow.com/a/10304080/126995
            cmd.Parameters.Add(name, true);
            return cmd;
        }

        /// <summary>Get all records, except the SOA</summary>
        /// <param name="ps"></param>
        /// <param name="zoneName">Name of the zone</param>
        /// <returns>Array of records</returns>
        public static DnsRecord[] GetZoneRecords(this PowerShellHelper ps, string zoneName)
        {
            // Get-DnsServerResourceRecord -ZoneName xxxx.com
            var allRecords = ps.RunPipeline(new Command("Get-DnsServerResourceRecord").addParam("ZoneName", zoneName));

            DnsRecord[] records = allRecords.Select(o => o.asDnsRecord(zoneName, true))
                .Where(r => null != r)
                .Where(r => r.RecordType != DnsRecordType.SOA)
                //	.Where( r => !( r.RecordName == "@" && DnsRecordType.NS == r.RecordType ) )
                .OrderBy(r => r.RecordName)
                .ThenBy(r => r.RecordType)
                .ThenBy(r => r.RecordData)
                .ToArray();
            List<DnsRecord> result = new List<DnsRecord>();
            foreach (DnsRecord record in records)
            {
                bool add = true;
                foreach (DnsRecord res in result)
                {
                    if (res.RecordName.Equals(record.RecordName) && res.RecordType.Equals(record.RecordType)
                        && res.RecordData.Equals(record.RecordData)) add = false;
                }
                if (add) result.Add(record);
            }
            return result.ToArray();
        }

        #region Records add / remove

        public static void Add_DnsServerResourceRecordA(this PowerShellHelper ps, string zoneName, string Name, string address, TimeSpan timetolive)
        {
            var cmd = new Command("Add-DnsServerResourceRecordA");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("Name", Name);
            cmd.addParam("IPv4Address", address);
            cmd.addParam("TimeToLive", timetolive);
            ps.RunPipeline(cmd);
        }

        public static void Add_DnsServerResourceRecordAAAA(this PowerShellHelper ps, string zoneName, string Name, string address, TimeSpan timetolive)
        {
            var cmd = new Command("Add-DnsServerResourceRecordAAAA");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("Name", Name);
            cmd.addParam("IPv6Address", address);
            cmd.addParam("TimeToLive", timetolive);
            ps.RunPipeline(cmd);
        }

        public static void Add_DnsServerResourceRecordCName(this PowerShellHelper ps, string zoneName, string Name, string alias, TimeSpan timetolive)
        {
            var cmd = new Command("Add-DnsServerResourceRecordCName");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("Name", Name);
            cmd.addParam("HostNameAlias", alias);
            cmd.addParam("TimeToLive", timetolive);
            ps.RunPipeline(cmd);
        }

        public static void Add_DnsServerResourceRecordCAA(this PowerShellHelper ps, string zoneName, string Name, string recordData, TimeSpan timetolive)
        {
            var cmd = new Command("Add-DnsServerResourceRecord");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("Name", Name);
            cmd.addParam("RecordData", recordData);
            cmd.addParam("Type", 257);
            cmd.addParam("TimeToLive", timetolive);
            ps.RunPipeline(cmd);
        }

        public static void Add_DnsServerResourceRecordMX(this PowerShellHelper ps, string zoneName, string Name, string mx, UInt16 pref, TimeSpan timetolive)
        {
            var cmd = new Command("Add-DnsServerResourceRecordMX");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("Name", Name);
            cmd.addParam("MailExchange", mx);
            cmd.addParam("Preference", pref);
            cmd.addParam("TimeToLive", timetolive);
            ps.RunPipeline(cmd);
        }

        public static void Add_DnsServerResourceRecordNS(this PowerShellHelper ps, string zoneName, string Name, string NameServer, TimeSpan timetolive)
        {
            var cmd = new Command("Add-DnsServerResourceRecord");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("Name", Name);
            cmd.addParam("NS");
            cmd.addParam("NameServer", NameServer);
            cmd.addParam("TimeToLive", timetolive);
            ps.RunPipeline(cmd);
        }

        public static void Add_DnsServerResourceRecordTXT(this PowerShellHelper ps, string zoneName, string Name, string txt, TimeSpan timetolive)
        {
            var cmd = new Command("Add-DnsServerResourceRecord");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("Name", Name);
            cmd.addParam("Txt");
            cmd.addParam("DescriptiveText", txt);
            cmd.addParam("TimeToLive", timetolive);
            ps.RunPipeline(cmd);
        }

        public static void Add_DnsServerResourceRecordSRV(this PowerShellHelper ps, string zoneName, string Name, string DomainName, UInt16 Port, UInt16 Priority, UInt16 Weight, TimeSpan timetolive)
        {
            var cmd = new Command("Add-DnsServerResourceRecord");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("Name", Name);
            cmd.addParam("Srv");
            cmd.addParam("DomainName", DomainName);
            cmd.addParam("Port", Port);
            cmd.addParam("Priority", Priority);
            cmd.addParam("Weight", Weight);
            cmd.addParam("TimeToLive", timetolive);
            ps.RunPipeline(cmd);
        }

        public static void Remove_DnsServerResourceRecord(this PowerShellHelper ps, string zoneName, DnsRecord record)
        {
            string type;
            if (!RecordTypes.rrTypeFromRecord.TryGetValue(record.RecordType, out type))
                throw new Exception("Unknown record type");

            string Name = record.RecordName;
            if (String.IsNullOrEmpty(Name)) Name = "@";

            var cmd = new Command("Get-DnsServerResourceRecord");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("Name", Name);
            if (!type.Equals("UNKNOWN") && !type.Equals("CAA"))
            {
                cmd.addParam("RRType", type);
            }
            else
            {
                cmd.addParam("Type", 257);
            }
            Collection<PSObject> resourceRecords = ps.RunPipeline(cmd);

            object inputObject = null;
            foreach (PSObject resourceRecord in resourceRecords)
            {
                DnsRecord dnsResourceRecord = resourceRecord.asDnsRecord(zoneName, false);

                bool found = false;

                switch (dnsResourceRecord.RecordType)
                {
                    case DnsRecordType.A:
                    case DnsRecordType.AAAA:
                    case DnsRecordType.CNAME:
                    case DnsRecordType.CAA:
                    case DnsRecordType.UNKNOWN:
                    case DnsRecordType.NS:
                    case DnsRecordType.TXT:
                        found = dnsResourceRecord.RecordData == record.RecordData;
                        break;
                    case DnsRecordType.SOA:
                        found = true;
                        break;
                    case DnsRecordType.MX:
                        found = (dnsResourceRecord.RecordData == record.RecordData) && (dnsResourceRecord.MxPriority == record.MxPriority);
                        break;
                    case DnsRecordType.SRV:
                        found = (dnsResourceRecord.RecordData == record.RecordData)
                            && (dnsResourceRecord.SrvPriority == record.SrvPriority)
                            && (dnsResourceRecord.SrvWeight == record.SrvWeight)
                            && (dnsResourceRecord.SrvPort == record.SrvPort);
                        break;
                }

                if (found)
                {
                    inputObject = resourceRecord;
                    break;
                }
            }

            cmd = new Command("Remove-DnsServerResourceRecord");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("InputObject", inputObject);

            cmd.addParam("Force");
            ps.RunPipeline(cmd);
        }

        public static void Update_DnsServerResourceRecordSOA(this PowerShellHelper ps, string zoneName,
            TimeSpan ExpireLimit, TimeSpan MinimumTimeToLive, string PrimaryServer,
            TimeSpan RefreshInterval, string ResponsiblePerson, TimeSpan RetryDelay,
            string PSComputerName)
        {

            var cmd = new Command("Get-DnsServerResourceRecord");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("RRType", "SOA");
            Collection<PSObject> soaRecords = ps.RunPipeline(cmd);

            if (soaRecords.Count < 1)
                return;

            PSObject oldSOARecord = soaRecords[0];
            PSObject newSOARecord = oldSOARecord.Copy();

            CimInstance recordData = newSOARecord.Properties["RecordData"].Value as CimInstance;

            if (recordData == null) return;

            if (ExpireLimit != null)
                recordData.CimInstanceProperties["ExpireLimit"].Value = ExpireLimit;

            if (MinimumTimeToLive != null)
                recordData.CimInstanceProperties["MinimumTimeToLive"].Value = MinimumTimeToLive;

            if (PrimaryServer != null)
                recordData.CimInstanceProperties["PrimaryServer"].Value = PrimaryServer;

            if (RefreshInterval != null)
                recordData.CimInstanceProperties["RefreshInterval"].Value = RefreshInterval;

            if (ResponsiblePerson != null)
                recordData.CimInstanceProperties["ResponsiblePerson"].Value = ResponsiblePerson;

            if (RetryDelay != null)
                recordData.CimInstanceProperties["RetryDelay"].Value = RetryDelay;

            if (PSComputerName != null)
                recordData.CimInstanceProperties["PSComputerName"].Value = PSComputerName;

            UInt32 serialNumber = (UInt32)recordData.CimInstanceProperties["SerialNumber"].Value;

            // update record's serial number
            string sn = serialNumber.ToString();
            string todayDate = DateTime.Now.ToString("yyyyMMdd");
            if (sn.Length < 10 || !sn.StartsWith(todayDate))
            {
                // build a new serial number
                sn = todayDate + "01";
                serialNumber = UInt32.Parse(sn);
            }
            else
            {
                // just increment serial number
                serialNumber += 1;
            }

            recordData.CimInstanceProperties["SerialNumber"].Value = serialNumber;

            cmd = new Command("Set-DnsServerResourceRecord");
            cmd.addParam("NewInputObject", newSOARecord);
            cmd.addParam("OldInputObject", oldSOARecord);
            cmd.addParam("ZoneName", zoneName);
            ps.RunPipeline(cmd);

        }

        #endregion
    }
}
