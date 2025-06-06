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
using SolidCP.Server.Utils;

namespace SolidCP.Providers.DNS
{
    public class MsDNS2016 : MsDNS2012, IDnsServer
    {
        private PowerShellHelper ps = null;
        private bool bulkRecords;

        public MsDNS2016()
        {
            // Create PowerShell helper
            ps = new PowerShellHelper();
            if (!this.IsInstalled())
                return;
        }

        #region Zones

        public override DnsRecord[] GetZoneRecords(string zoneName)
        {
            return ps.GetZoneRecords(zoneName);
        }

        public override void AddZoneRecord(string zoneName, DnsRecord record)
        {
            try
            {
                string name = record.RecordName;
                if (String.IsNullOrEmpty(name))
                    name = ".";

                if (record.RecordTTL == 0)
                    record.RecordTTL = DNSRecordDefaultTTL;

                if (record.RecordType == DnsRecordType.A)
                    ps.Add_DnsServerResourceRecordA(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.AAAA)
                    ps.Add_DnsServerResourceRecordAAAA(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.CNAME)
                    ps.Add_DnsServerResourceRecordCName(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.MX)
                    ps.Add_DnsServerResourceRecordMX(zoneName, name, record.RecordData, (ushort)record.MxPriority, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.NS)
                    ps.Add_DnsServerResourceRecordNS(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.TXT)
                    ps.Add_DnsServerResourceRecordTXT(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.SRV)
                    ps.Add_DnsServerResourceRecordSRV(zoneName, name, record.RecordData, (ushort)record.SrvPort, (ushort)record.SrvPriority, (ushort)record.SrvWeight, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.CAA)
                    ps.Add_DnsServerResourceRecordCAA(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else if (record.RecordType == DnsRecordType.PTR)
                    ps.Add_DnsServerResourceRecordPTR(zoneName, name, record.RecordData, TimeSpan.FromSeconds(record.RecordTTL));
                else
                    throw new Exception("Unknown record type");
            }
            catch (Exception ex)
            {
                // log exception
                Log.WriteError(ex);
            }
        }

        public override void AddZoneRecords(string zoneName, DnsRecord[] records)
        {
            bulkRecords = true;
            try
            {
                foreach (DnsRecord record in records)
                    AddZoneRecord(zoneName, record);
            }
            finally
            {
                bulkRecords = false;
            }

            UpdateSoaRecord(zoneName);
        }

        public override void DeleteZoneRecord(string zoneName, DnsRecord record)
        {
            try
            {
                string rrType;
                if (!RecordTypes.rrTypeFromRecord.TryGetValue(record.RecordType, out rrType))
                    throw new Exception("Unknown record type");
                ps.Remove_DnsServerResourceRecord(zoneName, record);
            }
            catch (Exception ex)
            {
                // log exception
                Log.WriteError(ex);
            }
        }

        public override void DeleteZoneRecords(string zoneName, DnsRecord[] records)
        {
            foreach (DnsRecord record in records)
                DeleteZoneRecord(zoneName, record);
        }
        #endregion


        #region SOA Record

        public override void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson)
        {
            try
            {
                ps.Update_DnsServerResourceRecordSOA(zoneName, ExpireLimit, MinimumTTL, primaryNsServer, RefreshInterval, primaryPerson, RetryDelay, null);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
        }

        private void UpdateSoaRecord(string zoneName)
        {
            if (bulkRecords)
                return;

            try
            {
                ps.Update_DnsServerResourceRecordSOA(zoneName, ExpireLimit, MinimumTTL, null, RefreshInterval, null, RetryDelay, null);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
        }
        #endregion
    }
}
