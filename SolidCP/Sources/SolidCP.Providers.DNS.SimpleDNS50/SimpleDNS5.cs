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

﻿using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.IO;

using JHSoftware.SimpleDNSPlus;
using SolidCP.Server.Utils;
using Microsoft.Win32;
using System.Reflection;

namespace SolidCP.Providers.DNS
{
	public delegate void BuildDnsRecordDataEventHandler(string zoneName, ref string type,
		DnsRecord record, List<string> data);

	public class SimpleDNS5 : DNS.SimpleDNS
	{
		#region Static ctor & assembly resolver
		static SimpleDNS5()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			//
			if (!args.Name.Contains("SDNSAPI"))
				return null;
			//
			string connectorKeyName = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{33A002DC-D590-4527-B98E-3B9D6F4FC5DC}";
			string connectorLocation = String.Empty;
			//
			if (PInvoke.RegistryHive.HKLM.SubKeyExists_x86(connectorKeyName))
			{
				connectorLocation = PInvoke.RegistryHive.HKLM.GetSubKeyValue_x86(connectorKeyName, "InstallLocation");
			}
			//
			if (String.IsNullOrEmpty(connectorLocation))
			{
				Log.WriteInfo("SimpleDNS API library location is either null or empty");
				return null;
			}
			//
			string assemblyFile = Path.Combine(connectorLocation, args.Name.Split(',')[0] + ".dll");
			//
			Log.WriteInfo(assemblyFile);
			//
			if (!File.Exists(assemblyFile))
			{
				Log.WriteInfo("SimpleDNS API library could not be found or does not exist");
				return null;
			}
			//
			return Assembly.LoadFrom(assemblyFile);
		} 
		#endregion

		#region Records handlers
		private Dictionary<DnsRecordType, BuildDnsRecordDataEventHandler> SupportedDnsRecords = new Dictionary<DnsRecordType, BuildDnsRecordDataEventHandler>()
		{
			// A record
			{ DnsRecordType.A, new BuildDnsRecordDataEventHandler(BuildRecordData_ARecord) },
            // AAAA record
            { DnsRecordType.AAAA, new BuildDnsRecordDataEventHandler(BuildRecordData_AAAARecord) },
			// NS record
			{ DnsRecordType.NS, new BuildDnsRecordDataEventHandler(BuildRecordData_NSRecord) },
			// CNAME
			{ DnsRecordType.CNAME, new BuildDnsRecordDataEventHandler(BuildRecordData_CNAMERecord) },
			// MX
			{ DnsRecordType.MX, new BuildDnsRecordDataEventHandler(BuildRecordData_MXRecord) },
			// TXT
			{ DnsRecordType.TXT, new BuildDnsRecordDataEventHandler(BuildRecordData_TXTRecord) },
            // SRV
            { DnsRecordType.SRV, new BuildDnsRecordDataEventHandler(BuildRecordData_SRVRecord) },
			};
		#endregion

		public const int SOA_PRIMARY_NAME_SERVER = 0;
		public const int SOA_RESPONSIBLE_PERSON = 1;
		public const int SOA_SERIAL_NUMBER = 2;
		public const int SOA_REFRESH_INTERVAL = 3;
		public const int SOA_RETRY_DELAY = 4;
		public const int SOA_EXPIRE_LIMIT = 5;
		public const int SOA_MINIMUM_TTL = 6;
		public const int MX_RECORD_PRIORITY = 0;
		public const int MX_RECORD_NAMESERVER = 1;

		#region Service Methods

		protected string ConvertRecordNameToInternalFormat(string recordName, string zoneName)
		{
			if (String.IsNullOrEmpty(recordName))
				return recordName;
			else if (String.Compare(recordName, zoneName, true) == 0)
				return String.Empty;
			//
			else if (recordName.EndsWith("." + zoneName, StringComparison.InvariantCultureIgnoreCase))
				return recordName.Remove(recordName.Length - ("." + zoneName).Length);
			//
			return recordName;
		}

		protected string ConvertRecordNameToSDNSFormat(string recordName, string zoneName)
		{
			if (!String.IsNullOrEmpty(recordName))
				return recordName + "." + zoneName;
			//
			return zoneName;
		}

        protected DnsRecord ConvertToNative(DNSRecord record, string zoneName)
        {
            string recordName = ConvertRecordNameToInternalFormat(record.Name, zoneName);
            //
            DnsRecord dnsRecord = null;
            switch (record.Type)
            {
                case "A":
                    dnsRecord = new DnsRecord
                    {
                        RecordName = recordName,
                        RecordType = DnsRecordType.A,
                        RecordData = record.DataFields[0]
                    };
                    break;
                case "AAAA":
                    dnsRecord = new DnsRecord
                    {
                        RecordName = recordName,
                        RecordType = DnsRecordType.AAAA,
                        RecordData = record.DataFields[0]
                    };
                    break;
                case "NS":
                    dnsRecord = new DnsRecord
                    {
                        RecordName = recordName,
                        RecordType = DnsRecordType.NS,
                        RecordData = record.DataFields[0]
                    };
                    break;
                case "CNAME":
                    dnsRecord = new DnsRecord
                    {
                        RecordName = recordName,
                        RecordType = DnsRecordType.CNAME,
                        RecordData = record.DataFields[0]
                    };
                    break;
                case "MX":
                    dnsRecord = new DnsRecord
                    {
                        RecordName = recordName,
                        RecordType = DnsRecordType.MX,
                        MxPriority = Convert.ToInt32(record.DataFields[MX_RECORD_PRIORITY]),
                        RecordData = record.DataFields[MX_RECORD_NAMESERVER]
                    };
                    break;
                case "TXT":
                    dnsRecord = new DnsRecord
                    {
                        RecordName = recordName,
                        RecordType = DnsRecordType.TXT,
                        RecordData = record.DataFields[0]
                    };
                    break;
                case "SRV":
                    dnsRecord = new DnsRecord
                    {
                        RecordName = recordName,
                        RecordType = DnsRecordType.SRV,
                        RecordData = record.DataFields[3],
                        SrvPriority = Convert.ToInt32(record.DataFields[0]),
                        SrvWeight = Convert.ToInt32(record.DataFields[1]),
                        SrvPort = Convert.ToInt32(record.DataFields[2])
                    };
                    break;
            }
            //
            return dnsRecord;
        }
        
        		/// <summary>
		/// Setups connection with the Simple DNS instance
		/// </summary>
		/// <returns></returns>
		protected Connection SetupProviderConnection()
		{
			try
			{
				Uri uri = new Uri(SimpleDnsUrl);

				if (String.IsNullOrEmpty(SimpleDnsPassword))
					return new Connection(uri.Host, uri.Port);
				else
					return new Connection(uri.Host, uri.Port, SimpleDnsPassword);
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);

				throw;
			}
		}

		#endregion

		public override void AddPrimaryZone(string zoneName, string[] secondaryServers)
		{
			Connection cn = SetupProviderConnection();

			// CREATE PRIMARY DNS ZONE
			string primaryNameServer = System.Net.Dns.GetHostEntry("LocalHost").HostName;
			DNSZone zoneObj = cn.CreateZone(zoneName, primaryNameServer, "hostmaster");
			zoneObj.Comments = String.Concat("Created with SolidCP DNS API at ", DateTime.Now);

			// Setup zone data transfer
			if (secondaryServers != null && secondaryServers.Length != 0)
			{
				if (secondaryServers.Length == 1 && secondaryServers[0] == "*")
					zoneObj.AllowZoneTransfer = "*";
				else
					zoneObj.AllowZoneTransfer = String.Join(" ", secondaryServers);
			}

			// Update SOA record
			DNSRecord soaRecord = zoneObj.Records[0];
			soaRecord.DataFields[SOA_PRIMARY_NAME_SERVER] = CorrectSOARecord(zoneName, primaryNameServer);
			soaRecord.DataFields[SOA_RESPONSIBLE_PERSON] = CorrectSOARecord(zoneName, "hostmaster");
			// Issue new serial number for the zone
			soaRecord.DataFields[SOA_SERIAL_NUMBER] = UpdateSerialNumber(null);
			soaRecord.DataFields[SOA_REFRESH_INTERVAL] = RefreshInterval.ToString();
			soaRecord.DataFields[SOA_RETRY_DELAY] = RetryDelay.ToString();
			soaRecord.DataFields[SOA_EXPIRE_LIMIT] = ExpireLimit.ToString();
			soaRecord.DataFields[SOA_MINIMUM_TTL] = MinimumTTL.ToString();

			// Publish updates are made
			cn.UpdateZone(zoneObj, false);
		}

		public override void AddSecondaryZone(string zoneName, string[] masterServers)
		{
			Connection cn = SetupProviderConnection();

			IPAddress ipAddress = System.Net.Dns.GetHostEntry(masterServers[0]).AddressList[0];

			DNSZone dnsZone = cn.CreateZone(zoneName, masterServers[0], "hostmaster");
			dnsZone.PrimaryIP = ipAddress.ToString();

			cn.UpdateZone(dnsZone, false);
		}

		public override void AddZoneRecords(string zoneName, DnsRecord[] records)
		{
			// load zone
			Connection cn = SetupProviderConnection();
			DNSZone dnsZone = cn.GetZone(zoneName);

			// add zone records
			foreach (DnsRecord record in records)
			{
				if (!SupportedDnsRecords.ContainsKey(record.RecordType))
					continue;

				string m_strRecordName = ConvertRecordNameToSDNSFormat(record.RecordName, zoneName);

				//
				List<string> m_strRecordData = new List<string>();
				String m_strRecordType = String.Empty;
				// build record data
				SupportedDnsRecords[record.RecordType](zoneName, ref m_strRecordType, record, m_strRecordData);

				// skip if already added
				if (dnsZone.Records.Contains(m_strRecordName, m_strRecordType, m_strRecordData.ToArray()))
					continue;

				// add records
				dnsZone.Records.Add(m_strRecordName, m_strRecordType, m_strRecordData.ToArray());
			}

			// update zone
			cn.UpdateZone(dnsZone, false);

			// update SOA
			UpdateSoaRecord(zoneName);
		}

		public override void AddZoneRecord(string zoneName, DnsRecord record)
		{
			if (ZoneExists(zoneName))
			{
				//
				if (SupportedDnsRecords.ContainsKey(record.RecordType))
				{
					string m_strRecordName = ConvertRecordNameToSDNSFormat(record.RecordName, zoneName);
					//
					Connection cn = SetupProviderConnection();
					DNSZone dnsZone = cn.GetZone(zoneName);
					//
					List<string> m_strRecordData = new List<string>();
					String m_strRecordType = String.Empty;
					// build record data
					SupportedDnsRecords[record.RecordType](zoneName, ref m_strRecordType, record, m_strRecordData);

					// skip if already added
					if (dnsZone.Records.Contains(m_strRecordName, m_strRecordType, m_strRecordData.ToArray()))
						return;

					//
					dnsZone.Records.Add(m_strRecordName, m_strRecordType, m_strRecordData.ToArray());
					//
					cn.UpdateZone(dnsZone, false);
					//
					UpdateSoaRecord(zoneName);
				}
			}
		}

		public override void DeleteZoneRecord(string zoneName, DnsRecord record)
		{
			if (ZoneExists(zoneName))
			{
				string m_strRecordName = ConvertRecordNameToSDNSFormat(record.RecordName, zoneName);
				//
				Connection cn = SetupProviderConnection();
				DNSZone dnsZone = cn.GetZone(zoneName);
				//
				List<string> m_strRecordData = new List<string>();
				String m_strRecordType = String.Empty;
				// build record data
				SupportedDnsRecords[record.RecordType](zoneName, ref m_strRecordType, record, m_strRecordData);
				//
				dnsZone.Records.Remove(m_strRecordName, m_strRecordType, m_strRecordData.ToArray());
				//
				cn.UpdateZone(dnsZone, false);
				//
				UpdateSoaRecord(zoneName);
			}
		}

		public override void DeleteZoneRecords(string zoneName, DnsRecord[] records)
		{
			// load zone
			Connection cn = SetupProviderConnection();
			DNSZone dnsZone = cn.GetZone(zoneName);

			foreach (DnsRecord record in records)
			{
				if (ZoneExists(zoneName))
				{
					string m_strRecordName = ConvertRecordNameToSDNSFormat(record.RecordName, zoneName);
					//
					List<string> m_strRecordData = new List<string>();
					String m_strRecordType = String.Empty;
					// build record data
					SupportedDnsRecords[record.RecordType](zoneName, ref m_strRecordType, record, m_strRecordData);
					//
					dnsZone.Records.Remove(m_strRecordName, m_strRecordType, m_strRecordData.ToArray());
				}
			}

			// update zone
			cn.UpdateZone(dnsZone, false);
			// update SOA
			UpdateSoaRecord(zoneName);
		}

		public override void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson)
		{
			if (ZoneExists(zoneName))
			{
				//
				if (String.IsNullOrEmpty(primaryPerson))
					primaryPerson = "hostmaster";
				//
				Connection cn = SetupProviderConnection();

				DNSZone dnsZone = cn.GetZone(zoneName);
				dnsZone.Comments = "Updated by SolidCP DNS API at " + DateTime.Now.ToString();

				DNSRecord soaRecord = (dnsZone.Records.Count == 0) ? dnsZone.Records.Add("@", "SOA") : dnsZone.Records[0];
				// Fill record fields with the data
				soaRecord.DataFields[SOA_PRIMARY_NAME_SERVER] = CorrectSOARecord(zoneName, primaryNsServer);
				soaRecord.DataFields[SOA_RESPONSIBLE_PERSON] = primaryPerson;
				soaRecord.DataFields[SOA_SERIAL_NUMBER] = UpdateSerialNumber(soaRecord.DataFields[SOA_SERIAL_NUMBER]);
				soaRecord.DataFields[SOA_REFRESH_INTERVAL] = RefreshInterval.ToString();
				soaRecord.DataFields[SOA_RETRY_DELAY] = RetryDelay.ToString();
				soaRecord.DataFields[SOA_EXPIRE_LIMIT] = ExpireLimit.ToString();
				soaRecord.DataFields[SOA_MINIMUM_TTL] = MinimumTTL.ToString();

                // remove "dumb" internal Name Server
                string internalNameServer = System.Net.Dns.GetHostEntry("LocalHost").HostName;
                if (internalNameServer != primaryNsServer)
                {
                    dnsZone.Records.Remove(zoneName, "NS", internalNameServer);
                }

                // save zone
                cn.UpdateZone(dnsZone, false);
			}
		}

		public override bool ZoneExists(string zoneName)
		{
			try
			{
				Connection cn = SetupProviderConnection();
				//
				DNSZone dnsZone = cn.GetZone(zoneName);
				//
				return true;
			}
			catch (WebException ex)
			{
				Log.WriteWarning(ex.ToString());

				return false;
			}
		}

		public override string[] GetZones()
		{
			List<string> dnsZones = new List<string>();
			// Setup connection
			Connection cn = SetupProviderConnection();
			// Iterate through all zones on SimpleDNS side
			foreach (ZoneListItem li in cn.GetZoneList())
				dnsZones.Add(li.ZoneName);
			//
			return dnsZones.ToArray();
		}

		public override void DeleteZone(string zoneName)
		{
			Connection cn = SetupProviderConnection();

			cn.RemoveZone(zoneName);
		}

		public override DnsRecord[] GetZoneRecords(string zoneName)
		{
			List<DnsRecord> dnsRecords = new List<DnsRecord>();

			if (ZoneExists(zoneName))
			{
				Connection cn = SetupProviderConnection();
				//
				DNSZone dnsZone = cn.GetZone(zoneName);
				//
				foreach (DNSRecord record in dnsZone.Records)
				{
					DnsRecord zoneRecord = ConvertToNative(record, zoneName);
					if (zoneRecord != null && zoneRecord.RecordType != DnsRecordType.SOA
						&& zoneRecord.RecordType != DnsRecordType.Other)
						dnsRecords.Add(zoneRecord);
				}
			}

			return dnsRecords.ToArray();
		}

		#region Protected methods

		protected void UpdateSoaRecord(string zoneName)
		{
			if (ZoneExists(zoneName))
			{
				Connection cn = SetupProviderConnection();
				//
				DNSZone dnsZone = cn.GetZone(zoneName);
				DNSRecord soaRecord = dnsZone.Records[0];
				//
				UpdateSoaRecord(zoneName, "", soaRecord.DataFields[SOA_PRIMARY_NAME_SERVER], soaRecord.DataFields[SOA_RESPONSIBLE_PERSON]);
			}
		}

		/*protected override void UpdateZone(string zoneName, List<DnsRecord> records)
		{
			UpdateZone(zoneName, records, null);
		}

		protected override void UpdateZone(string zoneName, List<DnsRecord> records, string[] masterServers)
		{
			// SOA Record
			DnsSOARecord soa = null;
			foreach (DnsRecord record in records)
			{
				if (record is DnsSOARecord)
				{
					soa = (DnsSOARecord)record;
					break;
				}
			}
			//
			Connection cn = SetupProviderConnection();
			//
			DNSZone dnsZone = null;
			// Create zone if it doesn't exist
			if (!ZoneExists(zoneName))
				dnsZone = cn.CreateZone(zoneName, soa.PrimaryNsServer, soa.PrimaryPerson);
			else
				dnsZone = cn.GetZone(zoneName);
			//
			#region Manipulate with zone SOA record
			// Obtain SOA record
			DNSRecord soaRecord = (dnsZone.Records.Count == 0) ? dnsZone.Records.Add("@", "SOA") : dnsZone.Records[0];
			// Fill record fields with data
			soaRecord.DataFields[0] = CorrectSOARecord(zoneName, soa.PrimaryNsServer);
			soaRecord.DataFields[1] = CorrectSOARecord(zoneName, soa.PrimaryPerson);
			soaRecord.DataFields[2] = UpdateSerialNumber(soa.SerialNumber);
			soaRecord.DataFields[3] = RefreshInterval.ToString();
			soaRecord.DataFields[4] = RetryDelay.ToString();
			soaRecord.DataFields[5] = ExpireLimit.ToString();
			soaRecord.DataFields[6] = MinimumTTL.ToString();

			#endregion

			// add or update all other records
			foreach (DnsRecord rr in records)
			{
				string m_strRecordName = String.IsNullOrEmpty(rr.RecordName) ? zoneName : rr.RecordName;
				//
				switch (rr.RecordType)
				{
					case DnsRecordType.A:
						// cleanup DNS record if exists
						dnsZone.Records.Remove(m_strRecordName, "A");
						//
						dnsZone.Records.Add(m_strRecordName, "A", rr.RecordData);
						break;
					case DnsRecordType.AAAA:
						// cleanup DNS record if exists
						dnsZone.Records.Remove(m_strRecordName, "AAAA");
						//
						dnsZone.Records.Add(m_strRecordName, "AAAA", rr.RecordData);
						break;
					case DnsRecordType.NS:
						// cleanup DNS record if exists
						dnsZone.Records.Remove(m_strRecordName, "NS");
						//
						DNSRecord dnsRecord = dnsZone.Records.Add(m_strRecordName, "NS", 
							BuildRecordData(zoneName, rr.RecordData));
						dnsRecord.TTL = MinimumTTL;
						break;
					case DnsRecordType.CNAME:
						// cleanup DNS record if exists
						dnsZone.Records.Remove(m_strRecordName, "CNAME");
						//
						dnsZone.Records.Add(m_strRecordName, "CNAME", 
							BuildRecordData(zoneName, rr.RecordData, rr.RecordData));
						break;
					case DnsRecordType.MX:
						// cleanup DNS record if exists
						dnsZone.Records.Remove(m_strRecordName, "MX");
						//
						dnsZone.Records.Add(m_strRecordName, "MX", rr.MxPriority.ToString(),
							BuildRecordData(zoneName, rr.RecordData));
						break;
					case DnsRecordType.TXT:
						// cleanup DNS record if exists
						dnsZone.Records.Remove(m_strRecordName, "TXT");
						//
						dnsZone.Records.Add(m_strRecordName, "MX", "\"" + rr.RecordData + "\"");
						break;
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
			//
			cn.UpdateZone(dnsZone, false);
		}
		 
		 * private string GetDnsRecordType(DnsRecordType r_type)
		{
			if (r_type == DnsRecordType.A)
				return "A";
			else if (r_type == DnsRecordType.AAAA)
				return "AAAA";
			else if (r_type == DnsRecordType.CNAME)
				return "CNAME";
			else if (r_type == DnsRecordType.MX)
				return "MX";
			else if (r_type == DnsRecordType.NS)
				return "NS";
			else if (r_type == DnsRecordType.SOA)
				return "SOA";
			else if (r_type == DnsRecordType.TXT)
				return "TXT";
			//
			return String.Empty;
		}
		 
		 */

		#endregion

		#region Zone records action handlers

		static void BuildRecordData_ARecord(string zoneName, ref string type,
			DnsRecord record, List<string> data)
		{
			type = "A";
			data.Add(record.RecordData);
		}

		static void BuildRecordData_AAAARecord(string zoneName, ref string type,
			DnsRecord record, List<string> data)
		{
			type = "AAAA";
			data.Add(record.RecordData);
		}

		static void BuildRecordData_NSRecord(string zoneName, ref string type,
			DnsRecord record, List<string> data)
		{
			type = "NS";
			data.Add(record.RecordData);
		}

		static void BuildRecordData_CNAMERecord(string zoneName, ref string type,
			DnsRecord record, List<string> data)
		{
			type = "CNAME";
			data.Add(record.RecordData);
		}

		static void BuildRecordData_MXRecord(string zoneName, ref string type,
			DnsRecord record, List<string> data)
		{
			type = "MX";
			data.Add(record.MxPriority.ToString());
			data.Add(record.RecordData);
		}

		static void BuildRecordData_TXTRecord(string zoneName, ref string type,
			DnsRecord record, List<string> data)
		{
			type = "TXT";
			data.Add(record.RecordData);
		}

        	static void BuildRecordData_SRVRecord(string zoneName, ref string type, DnsRecord record, List<string> data)
        	{
            		type = "SRV";
            		data.Add(Convert.ToString(record.SrvPriority));
            		data.Add(Convert.ToString(record.SrvWeight));
            		data.Add(Convert.ToString(record.SrvPort));
            		data.Add(record.RecordData);
        	}

		#endregion

		new static string BuildRecordData(string zoneName, string host, string recordData)
		{
			if (String.Compare(zoneName, recordData, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "@";
			}

			return BuildRecordData(zoneName, host);
		}

		new static string BuildRecordData(string zoneName, string host)
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

        public override bool IsInstalled()
        {
			//
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
                    return split[0].Equals("5");
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
                    return split[0].Equals("5");
                }
            }
            return false;
        }

	}
}
