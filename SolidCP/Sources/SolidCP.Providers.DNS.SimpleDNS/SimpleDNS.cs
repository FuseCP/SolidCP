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
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Web;

using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using Microsoft.Win32;

namespace SolidCP.Providers.DNS
{
	public class SimpleDNS : HostingServiceProviderBase, IDnsServer
    {
        #region Properties
        protected int ExpireLimit
		{
			get { return ProviderSettings.GetInt("ExpireLimit"); }
		}

		protected int MinimumTTL
		{
			get { return ProviderSettings.GetInt("MinimumTTL"); }
		}

		protected int RefreshInterval
		{
			get { return ProviderSettings.GetInt("RefreshInterval"); }
		}

		protected int RetryDelay
		{
			get { return ProviderSettings.GetInt("RetryDelay"); }
		}

		protected string SimpleDnsUrl
		{
			get { return ProviderSettings["SimpleDnsUrl"]; }
		}

	    protected string AdminLogin
	    {
            get { return ProviderSettings["AdminLogin"]; }
	    }

	    protected string SimpleDnsPassword
	    {
            get { return ProviderSettings["Password"]; }
	    }

	    #endregion

		#region Zones
		public virtual bool ZoneExists(string zoneName)
		{
            try
            {
                string response = ExecuteDnsQuery("getzone", "zone=" + zoneName);
                return true;
            }
            catch (WebException ex)
            {
                Log.WriteWarning(ex.ToString());
                return false;
            }
		}

		public virtual string[] GetZones()
		{
			string response = ExecuteDnsQuery("zonelist", null);
			string[] lines = response.Split('\r', '\n');

			List<string> zones = new List<string>();
			foreach (string line in lines)
			{
				// prepare line
				if (line.Trim() != "")
					zones.Add(RemoveTrailingDot(line));
			}

			return zones.ToArray();
		}

		public virtual void AddPrimaryZone(string zoneName, string[] secondaryServers)
		{
			// CREATE PRIMARY DNS ZONE
			List<DnsRecord> records = new List<DnsRecord>();

			// add "Zone transfers" record
			if (secondaryServers != null && secondaryServers.Length != 0)
			{
				DnsRecord zt = new DnsRecord();
				zt.RecordType = DnsRecordType.Other;
				zt.RecordName = "";
				if (secondaryServers.Length == 1 &&
					secondaryServers[0] == "*")
					zt.RecordText = ";$AllowZT *";
				else
					zt.RecordText = ";$AllowZT " + String.Join(" ", secondaryServers);

				records.Add(zt);
			}

			// add SOA record
			DnsSOARecord soa = new DnsSOARecord();
			soa.RecordType = DnsRecordType.SOA;
			soa.RecordName = "";
			soa.PrimaryNsServer = System.Net.Dns.GetHostEntry("LocalHost").HostName;
			soa.PrimaryPerson = "hostmaster";//"hostmaster." + zoneName;
			records.Add(soa);

			// add DNS zone
			UpdateZone(zoneName, records);
		}

		public virtual void AddSecondaryZone(string zoneName, string[] masterServers)
		{
			//;$PrimaryIP 127.0.0.1
			//;$MinimumTTL  0

			// CREATE SECONDARY DNS ZONE
			List<DnsRecord> records = new List<DnsRecord>();

			// add DNS zone
			UpdateZone(zoneName, records, masterServers);
		}

		public virtual DnsRecord[] GetZoneRecords(string zoneName)
		{
			List<DnsRecord> records = GetZoneRecordsArrayList(zoneName);
			List<DnsRecord> filteredRecords = new List<DnsRecord>();
			foreach (DnsRecord record in records)
			{
				if (record.RecordType != DnsRecordType.SOA
					&& record.RecordType != DnsRecordType.Other)
					filteredRecords.Add(record);
			}
			return filteredRecords.ToArray();
		}

		private List<DnsRecord> GetZoneRecordsArrayList(string zoneName)
		{
			string response = ExecuteDnsQuery("getzone", "zone=" + zoneName);
			if (response.IndexOf("(404) Not Found") != -1)
				return new List<DnsRecord>();

			// parse zone file
			return ParseZoneFileToArrayList(zoneName, response);
		}

		public virtual void DeleteZone(string zoneName)
		{
			ExecuteDnsQuery("removezone", "zone=" + zoneName);
		}
		#endregion

		#region Resource records
		public virtual void AddZoneRecord(string zoneName, DnsRecord record)
		{
			try
			{
				if (record.RecordType == DnsRecordType.A)
					AddARecord(zoneName, record.RecordName, record.RecordData);
				else if (record.RecordType == DnsRecordType.AAAA)
					AddAAAARecord(zoneName, record.RecordName, record.RecordData);
				else if (record.RecordType == DnsRecordType.CNAME)
					AddCNameRecord(zoneName, record.RecordName, record.RecordData);
				else if (record.RecordType == DnsRecordType.MX)
					AddMXRecord(zoneName, record.RecordName, record.RecordData, record.MxPriority);
				else if (record.RecordType == DnsRecordType.NS)
					AddNsRecord(zoneName, record.RecordName, record.RecordData);
				else if (record.RecordType == DnsRecordType.TXT)
					AddTxtRecord(zoneName, record.RecordName, record.RecordData);
			}
			catch (Exception ex)
			{
				// log exception
				Log.WriteError(
					String.Format(
						"Simple DNS: Unable to create record (Name '{0}', Data '{1}', Text '{2}') to zone '{3}'"
						, record.RecordName
						, record.RecordData
						, record.RecordText
						, zoneName
						)
					, ex
				);
			}
		}

		public virtual void AddZoneRecords(string zoneName, DnsRecord[] records)
		{
			foreach (DnsRecord record in records)
				AddZoneRecord(zoneName, record);
		}

		public virtual void DeleteZoneRecord(string zoneName, DnsRecord record)
		{
			try
			{
				if (record.RecordType == DnsRecordType.A || record.RecordType == DnsRecordType.AAAA || record.RecordType == DnsRecordType.CNAME)
					record.RecordName = CorrectRecordName(zoneName, record.RecordName);

				// delete record
				DeleteRecord(zoneName, record.RecordType, record.RecordName, record.RecordData);
			}
			catch (Exception ex)
			{
				// log exception
				Log.WriteError(
					String.Format(
						"Simple DNS: Unable to delete record (Name '{0}', Data '{1}', Text '{2}') to zone '{3}'"
						, record.RecordName
						, record.RecordData
						, record.RecordText
						, zoneName
						)
					, ex
				);
			}
		}

		public virtual void DeleteZoneRecords(string zoneName, DnsRecord[] records)
		{
			foreach (DnsRecord record in records)
				DeleteZoneRecord(zoneName, record);
		}
		#endregion

		#region A & AAAA records
		private void AddARecord(string zoneName, string host, string ip)
		{
			// get all zone records
			List<DnsRecord> records = GetZoneRecordsArrayList(zoneName);

			// delete A record
			//DeleteARecordInternal(records, zoneName, host);

            //check if user tries to add existent zone record
            foreach (DnsRecord dnsRecord in records)
            {
                if ((String.Compare(dnsRecord.RecordName, host, StringComparison.OrdinalIgnoreCase) == 0)
                    && (String.Compare(dnsRecord.RecordData, ip, StringComparison.OrdinalIgnoreCase) == 0)
                    )


                    return;
            }

			// add new A record
			DnsRecord record = new DnsRecord();
			record.RecordType = DnsRecordType.A;
			record.RecordName = host;
			record.RecordData = ip;
			records.Add(record);

		    // update zone
			UpdateZone(zoneName, records);
		}

		private void AddAAAARecord(string zoneName, string host, string ip) {
			// get all zone records
			List<DnsRecord> records = GetZoneRecordsArrayList(zoneName);

			// delete AAAA record
			//DeleteARecordInternal(records, zoneName, host);

			//check if user tries to add existent zone record
			foreach (DnsRecord dnsRecord in records) {
				if ((String.Compare(dnsRecord.RecordName, host, StringComparison.OrdinalIgnoreCase) == 0)
                    && (String.Compare(dnsRecord.RecordData, ip, StringComparison.OrdinalIgnoreCase) == 0)
					)


					return;
			}

			// add new AAAA record
			DnsRecord record = new DnsRecord();
			record.RecordType = DnsRecordType.AAAA;
			record.RecordName = host;
			record.RecordData = ip;
			records.Add(record);

			// update zone
			UpdateZone(zoneName, records);
		}

		private void DeleteRecord(string zoneName, DnsRecordType recordType,
			string recordName, string recordData)
		{
			// get all zone records
			List<DnsRecord> records = GetZoneRecordsArrayList(zoneName);

			// delete record
			DeleteRecord(zoneName, records, recordType, recordName, recordData);

			// update zone
			UpdateZone(zoneName, records);
		}

		private void DeleteRecord(string zoneName, List<DnsRecord> records, DnsRecordType recordType,
			string recordName, string recordData)
		{
			// delete record from the array
			int i = 0;
			while (i < records.Count)
			{
				if (records[i].RecordType == recordType
					&& (recordName == null || String.Compare(records[i].RecordName, recordName, true) == 0)
					&& (recordData == null || String.Compare(records[i].RecordData, recordData, true) == 0))
				{
					records.RemoveAt(i);
					break;
				}
				i++;
			}
		}

		#endregion

		#region NS records
		private void AddNsRecord(string zoneName, string host, string nameServer)
		{
			// get all zone records
			List<DnsRecord> records = GetZoneRecordsArrayList(zoneName);

			// delete NS record
			//DeleteNsRecordInternal(records, zoneName, nameServer);

            //check if user tries to add existent zone record
            foreach (DnsRecord dnsRecord in records)
            {
                if ((String.Compare(dnsRecord.RecordName, host, StringComparison.OrdinalIgnoreCase) == 0)
                    && (String.Compare(dnsRecord.RecordData, nameServer, StringComparison.OrdinalIgnoreCase) == 0))
                    return;
            }

			// add new NS record
			DnsRecord record = new DnsRecord();
			record.RecordType = DnsRecordType.NS;
			record.RecordName = host;
			record.RecordData = nameServer;
			records.Add(record);

			// update zone
			UpdateZone(zoneName, records);
		}

		#endregion

		#region MX records
		private void AddMXRecord(string zoneName, string host, string mailServer, int mailServerPriority)
		{
			// get all zone records
			List<DnsRecord> records = GetZoneRecordsArrayList(zoneName);

			// delete MX record
			//DeleteMXRecordInternal(records, zoneName, mailServer);

            //check if user tries to add existent zone record
            foreach (DnsRecord dnsRecord in records)
            {
                if ((dnsRecord.RecordType == DnsRecordType.MX) &&
                    (String.Compare(dnsRecord.RecordName, host, StringComparison.OrdinalIgnoreCase) == 0) &&
                    (String.Compare(dnsRecord.RecordData, mailServer, StringComparison.OrdinalIgnoreCase) == 0) 
                    && dnsRecord.MxPriority == mailServerPriority)
                    return;
            }

			// add new MX record
			DnsRecord record = new DnsRecord();
			record.RecordType = DnsRecordType.MX;
			record.RecordName = host;
			record.MxPriority = mailServerPriority;
			record.RecordData = mailServer;
			records.Add(record);

			// update zone
			UpdateZone(zoneName, records);
		}

		#endregion

		#region CNAME records
		private void AddCNameRecord(string zoneName, string alias, string targetHost)
		{
			// get all zone records
			List<DnsRecord> records = GetZoneRecordsArrayList(zoneName);

			// Find existing CNAME record. This is needed in order not to have two records after web-site is created.
			//  - verify if this record already exists... if so, we update is, otherwise create a new one
			DnsRecord record = records.Find(
					delegate(DnsRecord r) 
					{
						bool isSameRecord = (r.RecordType == DnsRecordType.CNAME) && (String.Compare(r.RecordName, alias, StringComparison.OrdinalIgnoreCase) == 0);
						if (isSameRecord)
						{
							return true;
						}

						return false;
					}
				);

			//  - if no existing records were found - add one :)
			if (record == null)
			{
				// add new CNAME record
				record = new DnsRecord();
				record.RecordType = DnsRecordType.CNAME;
				record.RecordName = alias;
				records.Add(record);
			}

			record.RecordData = targetHost;

			// update zone
			UpdateZone(zoneName, records);
		}

		#endregion

		#region TXT records
		private void AddTxtRecord(string zoneName, string host, string text)
		{
			// get all zone records
			List<DnsRecord> records = GetZoneRecordsArrayList(zoneName);

			// delete TXT record
			//DeleteTxtRecordInternal(records, zoneName, text);

            //check if user tries to add existent zone record
            foreach (DnsRecord dnsRecord in records)
            {
                if ((String.Compare(dnsRecord.RecordName, host, StringComparison.OrdinalIgnoreCase) == 0)
                    && (String.Compare(dnsRecord.RecordData, text, StringComparison.OrdinalIgnoreCase) == 0))  
                    return;
            }

			// add new TXT record
			DnsRecord record = new DnsRecord();
			record.RecordType = DnsRecordType.TXT;
			record.RecordName = host;
			record.RecordData = text;
			records.Add(record);

			// update zone
			UpdateZone(zoneName, records);
		}

		#endregion

		#region SOA record
		public virtual void UpdateSoaRecord(string zoneName, string host, string primaryNsServer,
			string primaryPerson)
		{
			// get all zone records
			List<DnsRecord> records = GetZoneRecordsArrayList(zoneName);

			// delete SOA record
			DeleteRecord(zoneName, records, DnsRecordType.SOA, null, null);

			// add new TXT record
			DnsSOARecord soa = new DnsSOARecord();
			soa.RecordType = DnsRecordType.SOA;
			soa.RecordName = "";
			soa.PrimaryNsServer = primaryNsServer;
			soa.PrimaryPerson = primaryPerson;
			records.Add(soa);

			// update primary person contact
			//if (soa.PrimaryPerson.ToLower().EndsWith(zoneName.ToLower()))
			//    soa.PrimaryPerson = soa.PrimaryPerson.Substring(0, (soa.PrimaryPerson.Length - zoneName.Length) - 1);

			// update zone
			UpdateZone(zoneName, records);
		}

		#endregion

		#region IHostingServiceProvier methods
		public override void DeleteServiceItems(ServiceProviderItem[] items)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is DnsZone)
				{
					try
					{
						// delete DNS zone
						DeleteZone(item.Name);
					}
					catch (Exception ex)
					{
						Log.WriteError(String.Format("Error deleting '{0}' SimpleDNS zone", item.Name), ex);
					}
				}
			}
		}
		#endregion

		#region Protected methods
		protected virtual DnsRecord[] ParseZoneFile(string zoneName, string zf)
		{
			return ParseZoneFileToArrayList(zoneName, zf).ToArray();
		}

		protected virtual List<DnsRecord> ParseZoneFileToArrayList(string zoneName, string zf)
		{
			List<DnsRecord> records = new List<DnsRecord>();
			StringReader reader = new StringReader(zf);
			string zfLine = null;

			DnsSOARecord soa = null;
			while ((zfLine = reader.ReadLine()) != null)
			{
				//string line = Regex.Replace(zfLine, "\\s+", " ").Trim();

				string[] columns = zfLine.Split('\t');

				string recordName = "";
				string recordTTL = "";
				string recordType = "";
				string recordData = "";
				string recordData2 = "";

				recordName = columns[0];
				if (columns.Length > 1)
					recordTTL = columns[1];
				if (columns.Length > 2)
					recordType = columns[2];
				if (columns.Length > 3)
					recordData = columns[3];
				if (columns.Length > 4)
					recordData2 = columns[4].Trim();

				if (recordType == "IN SOA")
				{
					string[] dataColumns = recordData.Split(' ');

					// parse SOA record
					soa = new DnsSOARecord();
					soa.RecordType = DnsRecordType.SOA;
					soa.RecordName = "";
					soa.PrimaryNsServer = RemoveTrailingDot(dataColumns[0]);
					soa.PrimaryPerson = RemoveTrailingDot(dataColumns[1]);
					soa.RecordText = zfLine;
					if (dataColumns[2] != "(")
						soa.SerialNumber = dataColumns[2];

					// add to the collection
					records.Add(soa);
				}
				else if (recordData2.IndexOf("; Serial number") != -1)
				{
					string[] dataColumns = recordData2.Split(' ');

					// append soa serial number
					soa.SerialNumber = dataColumns[0];
				}
				else if (recordType == "NS") // NS record with empty host
				{
					DnsRecord r = new DnsRecord();
					r.RecordType = DnsRecordType.NS;
					r.RecordName = CorrectRecordName(zoneName, recordName);
					r.RecordData = CorrectRecordData(zoneName, recordData);
					r.RecordText = zfLine;
					records.Add(r);
				}
				else if (recordType == "A") // A record
				{
					DnsRecord r = new DnsRecord();
					r.RecordType = DnsRecordType.A;
					r.RecordName = CorrectRecordName(zoneName, recordName);
					r.RecordData = recordData;
					r.RecordText = zfLine;
					records.Add(r);
				}
				else if (recordType == "AAAA") // A record
				{
					DnsRecord r = new DnsRecord();
					r.RecordType = DnsRecordType.AAAA;
					r.RecordName = CorrectRecordName(zoneName, recordName);
					r.RecordData = recordData;
					r.RecordText = zfLine;
					records.Add(r);
				}
				else if (recordType == "CNAME") // CNAME record
				{
					DnsRecord r = new DnsRecord();
					r.RecordType = DnsRecordType.CNAME;
					r.RecordName = CorrectRecordName(zoneName, recordName);
					r.RecordData = CorrectRecordData(zoneName, recordData, recordData);
					r.RecordText = zfLine;
					records.Add(r);
				}
				else if (recordType == "MX") // MX record
				{
					string[] dataColumns = recordData.Split(' ');

					DnsRecord r = new DnsRecord();
					r.RecordType = DnsRecordType.MX;
					r.RecordName = CorrectRecordName(zoneName, recordName);
					r.MxPriority = Int32.Parse(dataColumns[0]);
					r.RecordData = CorrectRecordData(zoneName, dataColumns[1]);
					r.RecordText = zfLine;
					records.Add(r);
				}
				else if (recordType == "TXT") // TXT record
				{
					DnsRecord r = new DnsRecord();
					r.RecordType = DnsRecordType.TXT;
					r.RecordName = CorrectRecordName(zoneName, recordName);
					r.RecordData = recordData.Substring(1, recordData.Length - 2);
					r.RecordText = zfLine;
					records.Add(r);
				}
				else if (zfLine.StartsWith(";$AllowZT")
					|| zfLine.StartsWith(";$PrimaryIP")
					|| zfLine.StartsWith(";$MinimumTTL"))
				{
					// unknown record just keep it line
					DnsRecord r = new DnsRecord();
					r.RecordType = DnsRecordType.Other;
					r.RecordName = "";
					r.RecordText = zfLine;
					records.Add(r);
				}

				//Debug.WriteLine(zfLine);
			}
			return records;
		}


		protected virtual void UpdateZone(string zoneName, List<DnsRecord> records)
		{
			UpdateZone(zoneName, records, null);
		}

		protected virtual void UpdateZone(string zoneName, List<DnsRecord> records, string[] masterServers)
		{
			// build zone file
			StringBuilder sb = new StringBuilder();

			// add SolidCP comment
			sb.Append(";$; Updated with SolidCP DNS API ").Append(DateTime.Now).Append("\r");

			// render comment/service records
			foreach (DnsRecord rr in records)
			{
				if (rr.RecordText != null && rr.RecordText.StartsWith(";") && !(rr.RecordType == DnsRecordType.TXT))
				{
					sb.Append(rr.RecordText);

					// add line break
					sb.Append("\r");
				}
			}

			// render SOA record
			foreach (DnsRecord rr in records)
			{
				string host = "";
				string type = "";
				string data = "";

				if (rr is DnsSOARecord)
				{
					type = "IN SOA";
					DnsSOARecord soa = (DnsSOARecord)rr;
					host = soa.RecordName;
					data = String.Format("{0} {1} {2} {3} {4} {5} {6}",
						CorrectSOARecord(zoneName, soa.PrimaryNsServer),
						CorrectSOARecord(zoneName, soa.PrimaryPerson),
						UpdateSerialNumber(soa.SerialNumber),
						RefreshInterval,
						RetryDelay,
						ExpireLimit,
						MinimumTTL);

					// add line to the zone file
					sb.Append(BuildRecordName(zoneName, host)).Append("\t");
					sb.Append("\t");
					sb.Append(type).Append("\t");
					sb.Append(data);

					// add line break
					sb.Append("\r");
				}
			}

			// render all other records
			foreach (DnsRecord rr in records)
			{
				string host = String.Empty;
				string type = String.Empty;
				string data = String.Empty;
				string name = String.Empty;

				if (rr.RecordType == DnsRecordType.A)
				{
					type = "A";
					host = rr.RecordName;
					data = rr.RecordData;
					name = BuildRecordName(zoneName, host);
				}
				else if (rr.RecordType == DnsRecordType.AAAA) {
					type = "AAAA";
					host = rr.RecordName;
					data = rr.RecordData;
					name = BuildRecordName(zoneName, host);
				}
				else if (rr.RecordType == DnsRecordType.NS)
				{
					type = "NS";
					host = rr.RecordName;
					data = BuildRecordData(zoneName, rr.RecordData);
					name = BuildRecordName(zoneName, host);
				}
				else if (rr.RecordType == DnsRecordType.CNAME)
				{
					type = "CNAME";
					host = rr.RecordName;
					data = BuildRecordData(zoneName, rr.RecordData, rr.RecordData);
					name = host;
				}
				else if (rr.RecordType == DnsRecordType.MX)
				{
					type = "MX";
					host = rr.RecordName;
					data = String.Format("{0} {1}",
						rr.MxPriority,
						BuildRecordData(zoneName, rr.RecordData));
					name = BuildRecordName(zoneName, host);
				}
				else if (rr.RecordType == DnsRecordType.TXT)
				{
					type = "TXT";
					host = rr.RecordName;
					data = "\"" + rr.RecordData + "\"";
					name = BuildRecordName(zoneName, host);
				}

				// add line to the zone file
				if (type != "")
				{
					sb.Append(name).Append("\t");
					if (type == "NS")
						sb.Append(MinimumTTL);
					sb.Append("\t");
					sb.Append(type).Append("\t");
					sb.Append(data);

					// add line break
					sb.Append("\r");
				}
			}

			string zoneFile = sb.ToString();

			// update zone file
			string queryParams = "zone=" + zoneName + "&data=" + zoneFile;
			if (masterServers != null && masterServers.Length > 0)
				queryParams += "&masterip=" + masterServers[0];

			// execute query
			string result = ExecuteDnsQuery("updatezone", queryParams);
		}
		#endregion

		#region private methods
		private string CorrectRecordName(string zoneName, string host)
		{
			if (host == "" || host == "@")
				return "";
			else if (host.EndsWith("."))
				return RemoveTrailingDot(host);
			else
				return host;
		}

		private string CorrectRecordData(string zoneName, string host, string recordData)
		{
			if (recordData == "@")
			{
				return zoneName;
			}

			return CorrectRecordData(zoneName, host);
		}

		private string CorrectRecordData(string zoneName, string host)
		{
			if (host == "" || host == "@")
				return "";
			else if (host.EndsWith("."))
				return RemoveTrailingDot(host);
			else
				return host + "." + zoneName;
		}

		protected string BuildRecordName(string zoneName, string host)
		{
			if (host == "")
				return "@";
			else if (host.EndsWith(zoneName))
				return host.Substring(0, host.Length - zoneName.Length - 1);
			else
				return host;
		}

		protected string BuildRecordData(string zoneName, string host, string recordData)
		{
			if (String.Compare(zoneName, recordData, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "@";
			}

			return BuildRecordData(zoneName, host);
		}

		protected string BuildRecordData(string zoneName, string host)
		{
			try
			{

				if (host == "")
					return "@";
				else if (host.EndsWith("." + zoneName))
					return host.Substring(0, host.Length - zoneName.Length - 1);
				else
					return host + ".";

			}
			catch (ArgumentOutOfRangeException ex)
			{
				Log.WriteError(
					  String.Format(
						"Simple DNS: Cannot build record data. Zone name: {0}, Host {1}."
						, zoneName
						, host
					  )
					, ex
				);

				throw ex;
			}
		}

		protected string CorrectSOARecord(string zoneName, string data)
		{
			if (data == "")
				return "@";
			else if (data.EndsWith("." + zoneName))
				return data.Substring(0, data.Length - zoneName.Length - 1);
			else if (data.IndexOf(".") == -1)
				return data;
			else
				return data + ".";
		}

		protected string UpdateSerialNumber(string serialNumber)
		{
			// update record's serial number
			string sn = serialNumber;
			string todayDate = DateTime.Now.ToString("yyyyMMdd");
			if (sn == null || sn.Length < 10 || !sn.StartsWith(todayDate))
			{
				// build a new serial number
				return todayDate + "01";
			}
			else
			{
				// just increment serial number
				int newSerialNumber = Int32.Parse(serialNumber);
				newSerialNumber += 1;
				return newSerialNumber.ToString();
			}
		}

		private string RemoveTrailingDot(string str)
		{
			if (str.Length == 0 || str[str.Length - 1] != '.')
				return str;
			else
				return str.Substring(0, str.Length - 1);
		}


        private string ExecuteDnsQuery(string command, string postData)
		{
			HttpWebResponse result = null;
			StringBuilder sb = new StringBuilder();

			try
			{
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(SimpleDnsUrl + "/" + command);
				req.Method = (postData == null) ? "GET" : "POST";
				req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0; .NET CLR 1.0.3705)";
				req.ContentType = "application/x-www-form-urlencoded";
                if (!String.IsNullOrEmpty(SimpleDnsPassword))
                {
                    CredentialCache myCache = new CredentialCache();
                    myCache.Add(new Uri(SimpleDnsUrl + "/" + command), "Basic",
                                new NetworkCredential(AdminLogin, SimpleDnsPassword));
                    req.Credentials = myCache;
                }
			    StringBuilder UrlEncoded = new StringBuilder();

				Char[] reserved = { '?', '=', '&' };
				byte[] SomeBytes = null;

				if (postData != null)
				{
					int i = 0, j;
					while (i < postData.Length)
					{
						j = postData.IndexOfAny(reserved, i);
						if (j == -1)
						{
							UrlEncoded.Append(HttpUtility.UrlEncode(postData.Substring(i, postData.Length - i)));
							break;
						}
						UrlEncoded.Append(HttpUtility.UrlEncode(postData.Substring(i, j - i)));
						UrlEncoded.Append(postData.Substring(j, 1));
						i = j + 1;
					}
					SomeBytes = Encoding.ASCII.GetBytes(UrlEncoded.ToString());
					req.ContentLength = SomeBytes.Length;
					Stream newStream = req.GetRequestStream();
					newStream.Write(SomeBytes, 0, SomeBytes.Length);
					newStream.Close();
				}

				// load document
				result = (HttpWebResponse)req.GetResponse();
				Stream ReceiveStream = result.GetResponseStream();
				Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
				StreamReader sr = new StreamReader(ReceiveStream, encode);

				//Console.WriteLine("\r\nResponse stream received");
				Char[] read = new Char[256];
				int count = sr.Read(read, 0, 256);

				//Console.WriteLine("HTML...\r\n");
				while (count > 0)
				{
					String str = new String(read, 0, count);
					sb.Append(str);
					count = sr.Read(read, 0, 256);
				}

			}
            //catch (WebException ex)
            //{
            //    response.Status = -1;
            //    HttpWebResponse errorResp = (HttpWebResponse)ex.Response;
            //    if (errorResp != null)
            //    {
            //        response.Status = (int)errorResp.StatusCode;
            //    }
            //    response.Content = ex.Status.ToString();
            //    return response;
            //}
            //catch (Exception ex)
            //{
            //    //Debug.WriteLine(ex);
            //    response.Status = -1;
            //    response.Content = ex.ToString();
            //    return response;
            //}
			finally
			{
				if (result != null)
				{
					result.Close();
				}
			}

            return sb.ToString();
		}
		#endregion

        public override bool IsInstalled()
        {
            string productName = null;
            string productVersion = null;

            RegistryKey HKLM = Registry.LocalMachine;

            RegistryKey key = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

            if (key != null)
            {
                String[] names = key.GetSubKeyNames();

                foreach (string s in names)
                {
                    RegistryKey subkey = HKLM.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + s);
                    if (subkey != null)
                        if (!String.IsNullOrEmpty((string)subkey.GetValue("DisplayName")))
                        {
                            productName = (string)subkey.GetValue("DisplayName");
                        }
                    if (productName != null)
                        if (productName.Equals("Simple DNS Plus"))
                        {
                            if (subkey != null) productVersion = (string)subkey.GetValue("DisplayVersion");
                            break;
                        }
                }

                if (!String.IsNullOrEmpty(productVersion))
                {
                    string[] split = productVersion.Split(new char[] { '.' });
                    return split[0].Equals("4");
                }

                //checking x64 platform
                key = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
                
                if (key == null)
                {
                    return false;
                }

                names = key.GetSubKeyNames();

                foreach (string s in names)
                {
                    RegistryKey subkey = HKLM.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" + s);
                    if (subkey != null)
                        if (!String.IsNullOrEmpty((string)subkey.GetValue("DisplayName")))
                        {
                            productName = (string)subkey.GetValue("DisplayName");
                        }
                    if (productName != null)
                        if (productName.Equals("Simple DNS Plus"))
                        {
                            if (subkey != null) productVersion = (string)subkey.GetValue("DisplayVersion");
                            break;
                        }
                }

                if (!String.IsNullOrEmpty(productVersion))
                {
                    string[] split = productVersion.Split(new[] { '.' });
                    return split[0].Equals("4");
                }
            }
            return false;
        }

       
	}
}
