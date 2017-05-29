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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ServiceProcess;
using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;


namespace SolidCP.Providers.DNS
{
	public class IscBind : HostingServiceProviderBase, IDnsServer
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

		protected string BindConfigPath
		{
			get { return ProviderSettings["BindConfigPath"]; }
		}

		protected string ZonesFolderPath
		{
			get { return ProviderSettings["ZonesFolderPath"]; }
		}

		protected string ZoneFileNameTemplate
		{
			get { return ProviderSettings["ZoneFileNameTemplate"]; }
		}

		protected string BindReloadBatch
		{
			get { return ProviderSettings["BindReloadBatch"]; }
		}
		#endregion

		#region Zones
		public virtual bool ZoneExists(string zoneName)
		{
			foreach (string name in GetZones())
			{
				if (String.Compare(zoneName, name, true) == 0)
					return true;
			}
			return false;
		}

		public virtual string[] GetZones()
		{
			// open config file
			if (!File.Exists(BindConfigPath))
				throw new Exception(String.Format("BIND config \"{0}\" was not found.", BindConfigPath));

			List<string> zones = new List<string>();
			StreamReader reader = new StreamReader(BindConfigPath);
			string line = null;
			while ((line = reader.ReadLine()) != null)
			{
				line = line.Trim();
				if (line.ToLower().StartsWith("zone"))
				{
					int idx = line.IndexOf("\"");
					string zoneName = line.Substring(idx + 1, line.LastIndexOf("\"") - idx - 1);
					zones.Add(zoneName);
				}
			}
			reader.Close();
			return zones.ToArray();
		}

		public virtual void AddPrimaryZone(string zoneName, string[] secondaryServers)
		{
			// create zone record
			StringBuilder sb = new StringBuilder();
			sb.Append("\r\nzone \"").Append(zoneName).Append("\" in {\r\n");
			sb.Append("\ttype master;\r\n");
			sb.Append("\tfile \"").Append(GetZoneFileName(zoneName)).Append("\";\r\n");
			sb.Append("\tallow-transfer {");
			if (secondaryServers == null || secondaryServers.Length == 0)
			{
				sb.Append(" none;");
			}
			else
			{
				foreach (string ip in secondaryServers)
					sb.Append(" ").Append(ip).Append(";");
			}

			sb.Append(" };\r\n");
			sb.Append("\tallow-update { none; };\r\n");
			sb.Append("};\r\n");

			// append to config file
			File.AppendAllText(BindConfigPath, sb.ToString());

			// create zone file and fill it with initial info
			List<DnsRecord> records = new List<DnsRecord>();

			// add SOA record
			DnsSOARecord soa = new DnsSOARecord();
			soa.RecordType = DnsRecordType.SOA;
			soa.RecordName = "";
			soa.PrimaryNsServer = System.Net.Dns.GetHostEntry("LocalHost").HostName;
			soa.PrimaryPerson = "hostmaster";//"hostmaster." + zoneName;
			records.Add(soa);
            ReloadBIND("reconfig", "");
			// add DNS zone
			UpdateZone(zoneName, records);
		}

		public virtual void AddSecondaryZone(string zoneName, string[] masterServers)
		{
			// create zone record
			StringBuilder sb = new StringBuilder();
			sb.Append("\r\nzone \"").Append(zoneName).Append("\" in {\r\n");
			sb.Append("\ttype slave;\r\n");
			sb.Append("\tfile \"").Append(GetZoneFileName(zoneName)).Append("\";\r\n");
			sb.Append("\tmasters {");
			if (masterServers == null || masterServers.Length == 0)
			{
				sb.Append(" none;");
			}
			else
			{
				foreach (string ip in masterServers)
					sb.Append(" ").Append(ip).Append(";");
			}

			sb.Append(" };\r\n");
			sb.Append("};\r\n");

			// append to config file
			File.AppendAllText(BindConfigPath, sb.ToString());

			// create empty zone file
			File.Create(GetZoneFilePath(zoneName)).Close();

			// reload config
            ReloadBIND("reconfig", "");
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
			string zoneContent = LoadZoneFile(zoneName);

			// parse zone file
			return ParseZoneFileToArrayList(zoneName, zoneContent);
		}

		public virtual void DeleteZone(string zoneName)
		{
			// open config file and find required lines
			List<string> lines = new List<string>();
			StreamReader reader = new StreamReader(BindConfigPath);
			string line = null;
			bool zoneStarted = false;
			while ((line = reader.ReadLine()) != null)
			{
				if (!zoneStarted && line.Trim().ToLower().StartsWith("zone"))
				{
					int idx = line.IndexOf("\"");
					string zName = line.Substring(idx + 1, line.LastIndexOf("\"") - idx - 1);
					if (String.Compare(zName, zoneName, true) == 0)
					{
						zoneStarted = true;
						continue;
					}
					else
					{
						lines.Add(line);
					}
				}
				else if (zoneStarted && line.Trim().StartsWith("};"))
				{
					zoneStarted = false;
					continue;
				}
				else if (!zoneStarted)
				{
					lines.Add(line); // add the line
				}
			}
			reader.Close();

			// write updated lines back to file
			StreamWriter writer = new StreamWriter(BindConfigPath);
			foreach (string l in lines)
				writer.WriteLine(l);
			writer.Close();

			// delete zone file
			string zonePath = GetZoneFilePath(zoneName);
			if (File.Exists(zonePath))
				File.Delete(zonePath);

			// reload named.conf
            ReloadBIND("reconfig", "");
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
				Log.WriteError(ex);
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
				Log.WriteError(ex);
			}
		}

		public virtual void DeleteZoneRecords(string zoneName, DnsRecord[] records)
		{
			foreach (DnsRecord record in records)
				DeleteZoneRecord(zoneName, record);
		}
		#endregion

		#region A records
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

		#region AAAA records
		private void AddAAAARecord(string zoneName, string host, string ip) {
			// get all zone records
			List<DnsRecord> records = GetZoneRecordsArrayList(zoneName);

			// delete A record
			//DeleteARecordInternal(records, zoneName, host);

			//check if user tries to add existent zone record
			foreach (DnsRecord dnsRecord in records) {
				if ((String.Compare(dnsRecord.RecordName, host, StringComparison.OrdinalIgnoreCase) == 0)
					 && (String.Compare(dnsRecord.RecordData, ip, StringComparison.OrdinalIgnoreCase) == 0)
					 )


					return;
			}

			// add new A record
			DnsRecord record = new DnsRecord();
			record.RecordType = DnsRecordType.AAAA;
			record.RecordName = host;
			record.RecordData = ip;
			records.Add(record);

			// update zone
			UpdateZone(zoneName, records);
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

			// delete CNAME record
			//DeleteCNameRecordInternal(records, zoneName, alias);

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

			// add new CNAME record
            if (record == null)
            {
                record = new DnsRecord {RecordType = DnsRecordType.CNAME, RecordName = alias, RecordData = targetHost};
                records.Add(record);
            }
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

		#region private methods
		private DnsRecord[] ParseZoneFile(string zoneName, string zf)
		{
			return ParseZoneFileToArrayList(zoneName, zf).ToArray();
		}

		private List<DnsRecord> ParseZoneFileToArrayList(string zoneName, string zf)
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
					r.RecordData = CorrectRecordData(zoneName, recordData);
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

				//Debug.WriteLine(zfLine);
			}
			return records;
		}


		private void UpdateZone(string zoneName, List<DnsRecord> records)
		{
			UpdateZone(zoneName, records, null);
		}

		private void UpdateZone(string zoneName, List<DnsRecord> records, string[] masterServers)
		{
			// build zone file
			StringBuilder sb = new StringBuilder();

			// add SolidCP comment
			sb.Append("; Updated with SolidCP DNS API ").Append(DateTime.Now).Append("\r\n\r\n");

			// TTL
			sb.Append("$TTL ").Append(MinimumTTL).Append("\r\n\r\n");

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
					sb.Append("\r\n");
				}
			}

			// render all other records
			foreach (DnsRecord rr in records)
			{
				string host = "";
				string type = "";
				string data = "";

				if (rr.RecordType == DnsRecordType.A)
				{
					type = "A";
					host = rr.RecordName;
					data = rr.RecordData;
				}
				else if (rr.RecordType == DnsRecordType.AAAA)
				{
					type = "AAAA";
					host = rr.RecordName;
					data = rr.RecordData;
				}
				else if (rr.RecordType == DnsRecordType.NS)
				{
					type = "NS";
					host = rr.RecordName;
					data = BuildRecordData(zoneName, rr.RecordData);
				}
				else if (rr.RecordType == DnsRecordType.CNAME)
				{
					type = "CNAME";
					host = rr.RecordName;
					data = BuildRecordData(zoneName, rr.RecordData);
				}
				else if (rr.RecordType == DnsRecordType.MX)
				{
					type = "MX";
					host = rr.RecordName;
					data = String.Format("{0} {1}",
						rr.MxPriority,
						BuildRecordData(zoneName, rr.RecordData));
				}
				else if (rr.RecordType == DnsRecordType.TXT)
				{
					type = "TXT";
					host = rr.RecordName;
					data = "\"" + rr.RecordData + "\"";
				}

				// add line to the zone file
				if (type != "")
				{
					sb.Append(BuildRecordName(zoneName, host)).Append("\t");
					if (type == "NS")
						sb.Append(MinimumTTL);
					sb.Append("\t");
					sb.Append(type).Append("\t");
					sb.Append(data);

					// add line break
					sb.Append("\r\n");
				}
			}

			// update zone file
			UpdateZoneFile(zoneName, sb.ToString());
            ReloadBIND("reload", zoneName);
		}

		private string CorrectRecordName(string zoneName, string host)
		{
			if (host == "" || host == "@")
				return "";
			else if (host.EndsWith("."))
				return RemoveTrailingDot(host);
			else
				return host;
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

		private string BuildRecordName(string zoneName, string host)
		{
			if (host == "")
				return "@";
			else if (host.EndsWith(zoneName))
				return host.Substring(0, host.Length - zoneName.Length - 1);
			else
				return host;
		}

		private string BuildRecordData(string zoneName, string host)
		{
			if (host == "")
				return "@";
			else if (host.EndsWith(zoneName))
				return host.Substring(0, host.Length - zoneName.Length - 1);
			else
				return host + ".";
		}

		private string CorrectSOARecord(string zoneName, string data)
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

		private string UpdateSerialNumber(string serialNumber)
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

		private string LoadZoneFile(string zoneName)
		{
			string path = GetZoneFilePath(zoneName);
			if (!File.Exists(path))
				return "";
			return File.ReadAllText(path);
		}

		private void UpdateZoneFile(string zoneName, string zoneContent)
		{
			string path = GetZoneFilePath(zoneName);
			File.WriteAllText(path, zoneContent);
        }

		private string GetZoneFilePath(string zoneName)
		{
			return Path.Combine(ZonesFolderPath, GetZoneFileName(zoneName));
		}

		private string GetZoneFileName(string zoneName)
		{
			return StringUtils.ReplaceStringVariable(ZoneFileNameTemplate, "domain_name", zoneName);
		}

		private void ReloadBIND(string Args, string zoneName)
		{

            // Do we have a rndc.exe? if so use it - improves handling
            if (BindReloadBatch.IndexOf("rndc.exe") > 0)
            {
                Process rndc = new Process();
                rndc.StartInfo.FileName = BindReloadBatch;
                string rndcArguments = Args;
                if (zoneName.Length > 0)
                {
                    rndcArguments += " " + zoneName;
                }
                rndc.StartInfo.Arguments = rndcArguments;
                rndc.StartInfo.CreateNoWindow = true;
                rndc.Start();

                /*
                 * Can't figure out how to log the output of the process to auditlog. If someone could be of assistans and fix this.
                 * it's rndcOutput var that should be written to audit log.s
                 * 
                string rndcOutput = "";
                while (!rndc.StandardOutput.EndOfStream)
                {
                    rndcOutput += rndc.StandardOutput.ReadLine();
                } 
                */

            }
            else
            {
                FileUtils.ExecuteSystemCommand(BindReloadBatch, "");
            }
		}

		#endregion

        public override bool IsInstalled()
        {
            ServiceController[] services = null;
            services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (service.DisplayName.Contains("ISC BIND"))

                    return true;
            }

            return false;
        }

	}
}
