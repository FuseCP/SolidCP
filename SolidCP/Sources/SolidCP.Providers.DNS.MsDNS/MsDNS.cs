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
using System.Management;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;

namespace SolidCP.Providers.DNS
{
    public class MsDNS : HostingServiceProviderBase, IDnsServer
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

        protected bool AdMode
        {
            get { return ProviderSettings.GetBool("AdMode"); }
        }
        #endregion

        private WmiHelper wmi = null;
        private bool bulkRecords;

        public MsDNS()
        {
            if (IsDNSInstalled())
            {
                // creare WMI helper
                wmi = new WmiHelper("root\\MicrosoftDNS");
            }
        }


        #region Zones
        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetZones()
        {
            List<string> zones = new List<string>();
            using (ManagementObjectCollection objZones = wmi.GetClass("MicrosoftDNS_Zone").GetInstances())
            {
                foreach (ManagementObject objZone in objZones) using (objZone)
                    {
                        if ((uint)objZone.Properties["ZoneType"].Value == 1)
                            zones.Add((string)objZone.Properties["Name"].Value);
                    }
            }

            return zones.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneName"></param>
        /// <returns></returns>
        /// <remarks>Supports managed resources disposal</remarks>
        public virtual bool ZoneExists(string zoneName)
        {
            using (RegistryKey root = Registry.LocalMachine)
            {
                using (RegistryKey rk = root.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\DNS Server\\Zones\\" + zoneName))
                {
                    return (rk != null);
                }
            }
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        /// <returns></returns>

        public virtual DnsRecord[] GetZoneRecords(string zoneName)
        {
            //using (ManagementObjectCollection rrs = wmi.ExecuteQuery(
            //	String.Format("SELECT * FROM MicrosoftDNS_ResourceRecord WHERE DomainName='{0}'", zoneName)))
            //ManagementObjectCollection rrs = GetWmiObjects("MicrosoftDNS_ResourceRecord", "DomainName='{0}'",zoneName);

            ManagementObjectCollection rrsA = wmi.GetWmiObjects("MicrosoftDNS_AType", "DomainName='{0}'", zoneName);

			ManagementObjectCollection rrsAAAA = wmi.GetWmiObjects("MicrosoftDNS_AAAAType", "DomainName='{0}'", zoneName);

            ManagementObjectCollection rrsCNAME = wmi.GetWmiObjects("MicrosoftDNS_CNAMEType", "DomainName='{0}'", zoneName);

            ManagementObjectCollection rrsMX = wmi.GetWmiObjects("MicrosoftDNS_MXType", "DomainName='{0}'", zoneName);

            ManagementObjectCollection rrsNS = wmi.GetWmiObjects("MicrosoftDNS_NSType", "DomainName='{0}'", zoneName);

            ManagementObjectCollection rrsTXT = wmi.GetWmiObjects("MicrosoftDNS_TXTType", "DomainName='{0}'", zoneName);

            ManagementObjectCollection rrsSRV = wmi.GetWmiObjects("MicrosoftDNS_SRVType", "DomainName='{0}'", zoneName);

            ManagementObjectCollection rrsSRV_tcp = wmi.GetWmiObjects("MicrosoftDNS_SRVType", "DomainName='_tcp.{0}'", zoneName);

            ManagementObjectCollection rrsSRV_udp = wmi.GetWmiObjects("MicrosoftDNS_SRVType", "DomainName='_udp.{0}'", zoneName);

            ManagementObjectCollection rrsSRV_tls = wmi.GetWmiObjects("MicrosoftDNS_SRVType", "DomainName='_tls.{0}'", zoneName);

            List<DnsRecord> records = new List<DnsRecord>();
            DnsRecord record = new DnsRecord();

            foreach (ManagementObject rr in rrsA)
            {
                record = new DnsRecord();
                record.RecordType = DnsRecordType.A;
                record.RecordName = CorrectHost(zoneName, (string)rr.Properties["OwnerName"].Value);
                record.RecordData = (string)rr.Properties["RecordData"].Value;
                records.Add(record);
            }

			foreach (ManagementObject rr in rrsAAAA) {
				record = new DnsRecord();
				record.RecordType = DnsRecordType.AAAA;
				record.RecordName = CorrectHost(zoneName, (string)rr.Properties["OwnerName"].Value);
				record.RecordData = (string)rr.Properties["RecordData"].Value;
				records.Add(record);
			}

            foreach (ManagementObject rr in rrsCNAME)
            {
                record = new DnsRecord();
                record.RecordType = DnsRecordType.CNAME;
                record.RecordName = CorrectHost(zoneName, (string)rr.Properties["OwnerName"].Value);
                record.RecordData = RemoveTrailingDot((string)rr.Properties["RecordData"].Value);
                records.Add(record);
            }

            foreach (ManagementObject rr in rrsMX)
            {
                record = new DnsRecord();
                record.RecordType = DnsRecordType.MX;
                record.RecordName = CorrectHost(zoneName, (string)rr.Properties["OwnerName"].Value);
                record.RecordData = RemoveTrailingDot((string)rr.Properties["MailExchange"].Value);
                record.MxPriority = Convert.ToInt32(rr.Properties["Preference"].Value);
                records.Add(record);
            }

            foreach (ManagementObject rr in rrsNS)
            {
                record = new DnsRecord();
                record.RecordType = DnsRecordType.NS;
                record.RecordName = CorrectHost(zoneName, (string)rr.Properties["OwnerName"].Value);
                record.RecordData = RemoveTrailingDot((string)rr.Properties["NSHost"].Value);
                records.Add(record);
            }

            foreach (ManagementObject rr in rrsTXT)
            {
                record = new DnsRecord();
                record.RecordType = DnsRecordType.TXT;
                record.RecordName = CorrectHost(zoneName, (string)rr.Properties["OwnerName"].Value);
                string text = (string)rr.Properties["RecordData"].Value;
                record.RecordData = text.Substring(1, text.Length - 2);
                records.Add(record);
            }

            foreach (ManagementObject rr in rrsSRV)
            {
                record = new DnsRecord();
                record.RecordType = DnsRecordType.SRV;
                record.RecordName = CorrectHost(zoneName, (string)rr.Properties["OwnerName"].Value);
                record.SrvPriority = Convert.ToInt32(rr.Properties["Priority"].Value);
                record.SrvWeight = Convert.ToInt32(rr.Properties["Weight"].Value);
                record.SrvPort = Convert.ToInt32(rr.Properties["Port"].Value);
                record.RecordData = RemoveTrailingDot((string)rr.Properties["SRVDomainName"].Value);
                records.Add(record);
            }

            foreach (ManagementObject rr in rrsSRV_tcp)
            {
                record = new DnsRecord();
                record.RecordType = DnsRecordType.SRV;
                record.RecordName = CorrectHost(zoneName, (string)rr.Properties["OwnerName"].Value);
                record.SrvPriority = Convert.ToInt32(rr.Properties["Priority"].Value);
                record.SrvWeight = Convert.ToInt32(rr.Properties["Weight"].Value);
                record.SrvPort = Convert.ToInt32(rr.Properties["Port"].Value);
                record.RecordData = RemoveTrailingDot((string)rr.Properties["SRVDomainName"].Value);
                records.Add(record);
            }

            foreach (ManagementObject rr in rrsSRV_udp)
            {
                record = new DnsRecord();
                record.RecordType = DnsRecordType.SRV;
                record.RecordName = CorrectHost(zoneName, (string)rr.Properties["OwnerName"].Value);
                record.SrvPriority = Convert.ToInt32(rr.Properties["Priority"].Value);
                record.SrvWeight = Convert.ToInt32(rr.Properties["Weight"].Value);
                record.SrvPort = Convert.ToInt32(rr.Properties["Port"].Value);
                record.RecordData = RemoveTrailingDot((string)rr.Properties["SRVDomainName"].Value);
                records.Add(record);
            }

            foreach (ManagementObject rr in rrsSRV_tls)
            {
                record = new DnsRecord();
                record.RecordType = DnsRecordType.SRV;
                record.RecordName = CorrectHost(zoneName, (string)rr.Properties["OwnerName"].Value);
                record.SrvPriority = Convert.ToInt32(rr.Properties["Priority"].Value);
                record.SrvWeight = Convert.ToInt32(rr.Properties["Weight"].Value);
                record.SrvPort = Convert.ToInt32(rr.Properties["Port"].Value);
                record.RecordData = RemoveTrailingDot((string)rr.Properties["SRVDomainName"].Value);
                records.Add(record);
            }




            return records.ToArray();
        }


        private string RemoveTrailingDot(string str)
        {
            return (str.EndsWith(".")) ? str.Substring(0, str.Length - 1) : str;
        }

        private string CorrectHost(string zoneName, string host)
        {
            if (host.ToLower() == zoneName.ToLower())
                return "";
            else
                return host.Substring(0, (host.Length - zoneName.Length - 1));
        }

        private ManagementObject GetZone(string zoneName)
        {
            ManagementObject objZone = null;

            try
            {

                objZone = wmi.GetObject(String.Format(
                    "MicrosoftDNS_Zone.ContainerName='{0}',DnsServerName='{1}',Name='{2}'",
                    zoneName, System.Net.Dns.GetHostEntry("LocalHost").HostName, zoneName));
                objZone.Get();

                /*
                objZone = wmi.GetWmiObject("MicrosoftDNS_Zone", "ContainerName = '{0}' AND DnsServerName = '{1}' AND Name = '{2}'",
                    new object[] { zoneName, System.Net.Dns.GetHostEntry("LocalHost").HostName, zoneName });
                 */
            }
            catch (Exception ex)
            {
                objZone = null;
                Log.WriteError("Could not get DNS Zone", ex);
            }

            return objZone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="secondaryServers"></param>
        /// <remarks>Supports managed resources disposal</remarks>
        public virtual void AddPrimaryZone(string zoneName, string[] secondaryServers)
        {
            // check if zone exists
            if (ZoneExists(zoneName))
                return;

            // create a zone
            using (ManagementClass clsZone = wmi.GetClass("MicrosoftDNS_Zone"))
            {
                using (ManagementBaseObject inParams = clsZone.GetMethodParameters("CreateZone"))
                {
                    inParams["ZoneName"] = zoneName;
                    inParams["ZoneType"] = 0; // primary zone

                    // create zones in AD if required
                    if (AdMode)
                        inParams["DsIntegrated"] = true;

                    using (ManagementBaseObject outParams = clsZone.InvokeMethod("CreateZone", inParams, null))
                    {
                        // update created zone
                        using (ManagementObject objZone = wmi.GetObject(String.Format(
                            "MicrosoftDNS_Zone.ContainerName='{0}',DnsServerName='{1}',Name='{2}'",
                            zoneName, System.Net.Dns.GetHostEntry("LocalHost").HostName, zoneName)))
                        {
                            try
                            {
                                // invoke ResetSecondaries method
                                using (ManagementBaseObject inParams2 = objZone.GetMethodParameters("ResetSecondaries"))
                                {
                                    inParams2["SecondaryServers"] = new string[] { };
                                    inParams2["NotifyServers"] = new string[] { };

                                    if (secondaryServers == null || secondaryServers.Length == 0)
                                    {
                                        // transfers are not allowed
                                        inParams2["SecureSecondaries"] = 3;
                                        inParams2["Notify"] = 0;
                                    }
                                    else if (secondaryServers.Length == 1 &&
                                        secondaryServers[0] == "*")
                                    {
                                        // allowed transfer from all servers
                                        inParams2["SecureSecondaries"] = 0;
                                        inParams2["Notify"] = 1;
                                    }
                                    else
                                    {
                                        // allowed transfer from specified servers
                                        inParams2["SecureSecondaries"] = 2;
                                        inParams2["SecondaryServers"] = secondaryServers;
                                        inParams2["NotifyServers"] = secondaryServers;
                                        inParams2["Notify"] = 2;
                                    }

                                    objZone.InvokeMethod("ResetSecondaries", inParams2, null);
                                }
                            }
                            catch
                            {
                                Log.WriteWarning("Error resetting/notifying secondary name servers");
                            }
                        }
                    }
                }
            }
            // delete orphan NS records
            DeleteOrphanNsRecords(zoneName);
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="masterServers"></param>
        public virtual void AddSecondaryZone(string zoneName, string[] masterServers)
        {
            // check if zone exists
            using (ManagementObject objSecondary = GetZone(zoneName))
            {
                if (objSecondary != null)
                    return;
            }

            // create a zone
            using (ManagementClass clsZone = wmi.GetClass("MicrosoftDNS_Zone"))
            {
                using (ManagementBaseObject inParams = clsZone.GetMethodParameters("CreateZone"))
                {
                    inParams["ZoneName"] = zoneName;
                    inParams["ZoneType"] = 1; // secondary zone
                    inParams["IpAddr"] = masterServers;
                    inParams["DataFileName"] = zoneName + ".dns";

                    // create zones in AD if required
                    inParams["DsIntegrated"] = AdMode;

                    using (ManagementBaseObject outParams = clsZone.InvokeMethod("CreateZone", inParams, null))
                    {
                        try
                        {
                            // update created zone
                            /*ManagementObject objZone = wmi.GetObject(String.Format(
                                "MicrosoftDNS_Zone.ContainerName='{0}',DnsServerName='{1}',Name='{2}'",
                                zoneName, System.Net.Dns.GetHostEntry("LocalHost").HostName, zoneName));

                            objZone.InvokeMethod("ForceRefresh", null);*/
                            using (ManagementObject objZone = (ManagementObject)outParams["RR"])
                            {
                                objZone.InvokeMethod("ReloadZone", null);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteWarning("Error ReloadZone for secondary zone '{0}': {1}", zoneName, ex.Message);
                        }
                    }
                }
            }

            // delete orphan NS records
            DeleteOrphanNsRecords(zoneName);
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>

        public virtual void DeleteZone(string zoneName)
        {
            try
            {
                using (ManagementObject objZone = GetZone(zoneName))
                {
                    if (objZone != null)
                        objZone.Delete();
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
        }


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
                else if (record.RecordType == DnsRecordType.SRV)
                    AddSrvRecord(zoneName, record.RecordName, record.SrvPriority, record.SrvWeight, record.SrvPort, record.RecordData);

            }
            catch (Exception ex)
            {
                // log exception
                Log.WriteError(ex);
            }
        }

        public virtual void AddZoneRecords(string zoneName, DnsRecord[] records)
        {
            bulkRecords = true;
            foreach (DnsRecord record in records)
                AddZoneRecord(zoneName, record);
            UpdateSoaRecord(zoneName);
        }

        public virtual void DeleteZoneRecord(string zoneName, DnsRecord record)
        {
            try
            {
                if (record.RecordType == DnsRecordType.A)
                    DeleteARecord(zoneName, record.RecordName, record.RecordData);
				else if (record.RecordType == DnsRecordType.AAAA)
					DeleteAAAARecord(zoneName, record.RecordName, record.RecordData);
				else if (record.RecordType == DnsRecordType.CNAME)
                    DeleteCNameRecord(zoneName, record.RecordName, record.RecordData);
                else if (record.RecordType == DnsRecordType.MX)
                    DeleteMXRecord(zoneName, record.RecordName, record.RecordData);
                else if (record.RecordType == DnsRecordType.NS)
                    DeleteNsRecord(zoneName, record.RecordName, record.RecordData);
                else if (record.RecordType == DnsRecordType.TXT)
                    DeleteTxtRecord(zoneName, record.RecordName, record.RecordData);
                else if (record.RecordType == DnsRecordType.SRV)
                    DeleteSrvRecord(zoneName, record.RecordName, record.RecordData);

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

        public void AddZoneRecord(string zoneName, string recordText)
        {
            try
            {
                Log.WriteStart(string.Format("Adding MS DNS Server zone '{0}' record '{1}'", zoneName, recordText));
                AddDnsRecord(zoneName, recordText);
                Log.WriteEnd("Added MS DNS Server zone record");
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
                throw;
            }
        }
        #endregion

        #region SOA Record
        public virtual void UpdateSoaRecord(string zoneName, string host, string primaryNsServer,
            string primaryPerson)
        {
            host = CorrectHostName(zoneName, host);

            // delete record if exists
            DeleteSoaRecord(zoneName);

            // format record data
            string recordText = GetSoaRecordText(host, primaryNsServer, primaryPerson);

            // add record
            AddDnsRecord(zoneName, recordText);

            // update SOA record
            UpdateSoaRecord(zoneName);
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        private void DeleteSoaRecord(string zoneName)
        {
            string query = String.Format("SELECT * FROM MicrosoftDNS_SOAType " +
                "WHERE OwnerName = '{0}'",
                zoneName);
            using (ManagementObjectCollection objRRs = wmi.ExecuteQuery(query))
            {
                foreach (ManagementObject objRR in objRRs) using (objRR)
                        objRR.Delete();
            }
        }

        private string GetSoaRecordText(string host, string primaryNsServer,
            string primaryPerson)
        {
            return String.Format("{0} IN SOA {1} {2} 1 900 600 86400 3600", host,
                primaryNsServer, primaryPerson);
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>

        private void UpdateSoaRecord(string zoneName)
        {
            // get existing SOA record in order to read serial number

            try
            {

                //ManagementObject obj = GetWmiObject("MicrosoftDNS_Zone", "ContainerName = '{0}'", zoneName);
                //ManagementObject objSoa = GetRelatedWmiObject(obj, "MicrosoftDNS_SOAType");


                ManagementObject objSoa = wmi.GetWmiObject("MicrosoftDNS_SOAType", "ContainerName = '{0}'", RemoveTrailingDot(zoneName));

                if (objSoa != null)
                {
                    if (objSoa.Properties["OwnerName"].Value.Equals(zoneName))
                    {
                        string primaryServer = (string)objSoa.Properties["PrimaryServer"].Value;
                        string responsibleParty = (string)objSoa.Properties["ResponsibleParty"].Value;
                        UInt32 serialNumber = (UInt32)objSoa.Properties["SerialNumber"].Value;

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

                        // update SOA record
                        using (ManagementBaseObject methodParams = objSoa.GetMethodParameters("Modify"))
                        {
                            methodParams["ResponsibleParty"] = responsibleParty;
                            methodParams["PrimaryServer"] = primaryServer;
                            methodParams["SerialNumber"] = serialNumber;

                            methodParams["ExpireLimit"] = ExpireLimit;
                            methodParams["MinimumTTL"] = MinimumTTL;
                            methodParams["TTL"] = MinimumTTL;
                            methodParams["RefreshInterval"] = RefreshInterval;
                            methodParams["RetryDelay"] = RetryDelay;

                            ManagementBaseObject outParams = objSoa.InvokeMethod("Modify", methodParams, null);
                        }
                        //
                        objSoa.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
        }

        #endregion

        #region A Record
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="host"></param>
        /// <param name="ip"></param>
        /// <remarks>Supports managed resources disposal</remarks>
        private void AddARecord(string zoneName, string host, string ip)
        {
            // add record
            using (ManagementClass clsRR = wmi.GetClass("MicrosoftDNS_AType"))
            {
                clsRR.InvokeMethod("CreateInstanceFromPropertyData", new object[] {
					GetDnsServerName(),
					zoneName,
					CorrectHostName(zoneName, host),
					1,
					MinimumTTL,
					ip
				});
            }

            // update SOA record
            if (bulkRecords) return;
            UpdateSoaRecord(zoneName);
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="host"></param>
        /// <param name="ip"></param>
        private void DeleteARecord(string zoneName, string host, string ip)
        {
            string query = String.Format("SELECT * FROM MicrosoftDNS_AType " +
                "WHERE ContainerName = '{0}' AND OwnerName = '{1}'",
                zoneName, CorrectHostName(zoneName, host));

            if (ip != null)
                query += String.Format(" AND RecordData = '{0}'", ip);

            using (ManagementObjectCollection objRRs = wmi.ExecuteQuery(query))
            {
                foreach (ManagementObject objRR in objRRs) using (objRR)
                        objRR.Delete();
            }

            // update SOA record
            UpdateSoaRecord(zoneName);

        }
        #endregion

		#region AAAA Record
		/// <summary>
		/// 
		/// </summary>
		/// <param name="zoneName"></param>
		/// <param name="host"></param>
		/// <param name="ip"></param>
		/// <remarks>Supports managed resources disposal</remarks>
		private void AddAAAARecord(string zoneName, string host, string ip) {
			// add record
			using (ManagementClass clsRR = wmi.GetClass("MicrosoftDNS_AAAAType")) {
				clsRR.InvokeMethod("CreateInstanceFromPropertyData", new object[] {
					GetDnsServerName(),
					zoneName,
					CorrectHostName(zoneName, host),
					1,
					MinimumTTL,
					ip
				});
			}

			// update SOA record
			if (bulkRecords) return;
			UpdateSoaRecord(zoneName);
		}

		/// <summary>
		/// Supports managed resources disposal
		/// </summary>
		/// <param name="zoneName"></param>
		/// <param name="host"></param>
		/// <param name="ip"></param>
		private void DeleteAAAARecord(string zoneName, string host, string ip) {
			string query = String.Format("SELECT * FROM MicrosoftDNS_AAAAType " +
					"WHERE ContainerName = '{0}' AND OwnerName = '{1}'",
				  zoneName, CorrectHostName(zoneName, host));

			if (ip != null)
				query += String.Format(" AND RecordData = '{0}'", ip);

			using (ManagementObjectCollection objRRs = wmi.ExecuteQuery(query)) {
				foreach (ManagementObject objRR in objRRs) using (objRR)
						objRR.Delete();
			}

			// update SOA record
			UpdateSoaRecord(zoneName);

		}
		#endregion

        #region CNAME Record
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="alias"></param>
        /// <param name="targetHost"></param>
        /// <remarks>Supports managed resources disposal</remarks>
        private void AddCNameRecord(string zoneName, string alias, string targetHost)
        {
            // add record
            using (ManagementClass clsRR = wmi.GetClass("MicrosoftDNS_CNAMEType"))
            {
                clsRR.InvokeMethod("CreateInstanceFromPropertyData", new object[] {
					GetDnsServerName(),
					zoneName,
					CorrectHostName(zoneName, alias),
					1,
					MinimumTTL,
					targetHost
				});
            }

            // update SOA record
            if (bulkRecords) return;
            UpdateSoaRecord(zoneName);
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="alias"></param>
        /// <param name="targetHost"></param>
        private void DeleteCNameRecord(string zoneName, string alias, string targetHost)
        {
            string query = String.Format("SELECT * FROM MicrosoftDNS_CNAMEType " +
                "WHERE ContainerName = '{0}' AND OwnerName = '{1}'",
                zoneName, CorrectHostName(zoneName, alias));

            if (targetHost != null)
                query += String.Format(" AND RecordData='{0}.'", targetHost);

            using (ManagementObjectCollection objRRs = wmi.ExecuteQuery(query))
            {
                foreach (ManagementObject objRR in objRRs) using (objRR)
                        objRR.Delete();
            }

            // update SOA record
            UpdateSoaRecord(zoneName);
        }
        #endregion

        #region MX Record
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="host"></param>
        /// <param name="mailServer"></param>
        /// <param name="mailServerPriority"></param>
        /// <remarks>Supports managed resources disposal</remarks>
        private void AddMXRecord(string zoneName, string host, string mailServer, int mailServerPriority)
        {
            // add record
            using (ManagementClass clsRR = wmi.GetClass("MicrosoftDNS_MXType"))
            {
                clsRR.InvokeMethod("CreateInstanceFromPropertyData", new object[] {
					GetDnsServerName(),
					zoneName,
					CorrectHostName(zoneName, host),
					1,
					MinimumTTL,
					mailServerPriority,
					mailServer
				});
            }

            // update SOA record
            if (bulkRecords) return;
            UpdateSoaRecord(zoneName);
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="host"></param>
        /// <param name="mailServer"></param>
        private void DeleteMXRecord(string zoneName, string host, string mailServer)
        {
            string query = String.Format("SELECT * FROM MicrosoftDNS_MXType " +
                "WHERE ContainerName = '{0}' AND OwnerName ='{1}'",
                zoneName, CorrectHostName(zoneName, host));

            if (mailServer != null)
                query += String.Format(" AND MailExchange = '{0}.'", CorrectHostName(zoneName, mailServer));

            using (ManagementObjectCollection objRRs = wmi.ExecuteQuery(query))
            {
                foreach (ManagementObject objRR in objRRs) using (objRR)
                        objRR.Delete();
            }

            // update SOA record
            UpdateSoaRecord(zoneName);
        }
        #endregion

        #region NS Record
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="host"></param>
        /// <param name="nameServer"></param>
        /// <remarks>Supports managed resources disposal</remarks>
        private void AddNsRecord(string zoneName, string host, string nameServer)
        {
            // add record
            using (ManagementClass clsRR = wmi.GetClass("MicrosoftDNS_NSType"))
            {
                clsRR.InvokeMethod("CreateInstanceFromPropertyData", new object[] {
					GetDnsServerName(),
					zoneName,
					CorrectHostName(zoneName, host),
					1,
					MinimumTTL,
					nameServer
				});
            }
            // update SOA record
            if (bulkRecords) return;
            UpdateSoaRecord(zoneName);
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="host"></param>
        /// <param name="nameServer"></param>
        private void DeleteNsRecord(string zoneName, string host, string nameServer)
        {
            string query = String.Format("SELECT * FROM MicrosoftDNS_NSType " +
                "WHERE ContainerName = '{0}' AND OwnerName = '{1}'",
                zoneName, CorrectHostName(zoneName, host));

            if (nameServer != null)
                query += String.Format(" AND NSHost = '{0}.'", nameServer);

            using (ManagementObjectCollection objRRs = wmi.ExecuteQuery(query))
            {
                foreach (ManagementObject objRR in objRRs) using (objRR)
                        objRR.Delete();
            }

            // update SOA record
            UpdateSoaRecord(zoneName);
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        private void DeleteOrphanNsRecords(string zoneName)
        {
            string machineName = System.Net.Dns.GetHostEntry("LocalHost").HostName.ToLower();
            string computerName = Environment.MachineName.ToLower();

            using (ManagementObjectCollection objRRs = wmi.ExecuteQuery(
                String.Format("SELECT * FROM MicrosoftDNS_NSType WHERE DomainName = '{0}'", zoneName)))
            {
                foreach (ManagementObject objRR in objRRs)
                    using (objRR)
                    {
                        string ns = ((string)objRR.Properties["NSHost"].Value).ToLower();
                        if (ns.StartsWith(machineName) || ns.StartsWith(computerName))
                            objRR.Delete();
                    }
            }
        }
        #endregion

        #region TXT Record
        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="host"></param>
        /// <param name="text"></param>
        private void AddTxtRecord(string zoneName, string host, string text)
        {
            using (ManagementClass clsRR = wmi.GetClass("MicrosoftDNS_TXTType"))
            {
                clsRR.InvokeMethod("CreateInstanceFromPropertyData", new object[] {
					GetDnsServerName(),
					zoneName,
					CorrectHostName(zoneName, host),
					1,
					MinimumTTL,
					System.Text.RegularExpressions.Regex.Replace(text, @"""|\\", "") 
				});
            }

            // update SOA record
            if (bulkRecords) return;
            UpdateSoaRecord(zoneName);
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="host"></param>
        /// <param name="text"></param>
        private void DeleteTxtRecord(string zoneName, string host, string text)
        {
            string query = String.Format("SELECT * FROM MicrosoftDNS_TXTType " +
                "WHERE ContainerName = '{0}' AND OwnerName = '{1}'",
                zoneName, CorrectHostName(zoneName, host));

            if (text != null)
                query += String.Format(" AND RecordData = '\"{0}\"'", System.Text.RegularExpressions.Regex.Replace(text, @"""|\\", ""));

            using (ManagementObjectCollection objRRs = wmi.ExecuteQuery(query))
            {
                foreach (ManagementObject objRR in objRRs) using (objRR)
                    {
                        objRR.Delete();
                    }
            }

            // update SOA record
            UpdateSoaRecord(zoneName);
        }
        #endregion

        #region SRV Record
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="host"></param>
        /// <param name="mailServer"></param>
        /// <param name="mailServerPriority"></param>
        /// <remarks>Supports managed resources disposal</remarks>
        private void AddSrvRecord(string zoneName, string host, int priority, int weight, int port, string domainName)
        {
            // add record
            using (ManagementClass clsRR = wmi.GetClass("MicrosoftDNS_SRVType"))
            {

                clsRR.InvokeMethod("CreateInstanceFromPropertyData", new object[] {
					GetDnsServerName(),
					zoneName,
					CorrectHostName(zoneName, host),
					1,
					MinimumTTL,
					priority,
					weight,
                    port,
                    domainName
				});
            }

            // update SOA record
            if (bulkRecords) return;
            UpdateSoaRecord(zoneName);
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="host"></param>
        /// <param name="mailServer"></param>
        private void DeleteSrvRecord(string zoneName, string host, string domainName)
        {

            string query = string.Empty;
            if ((host.Contains("._tcp")) | (host.Contains("._udp")) | (host.Contains("._tls")))
            {
                query = String.Format("SELECT * FROM MicrosoftDNS_SRVType " +
                "WHERE ContainerName = '{0}' AND OwnerName ='{1}.{0}'",
                zoneName, CorrectHostName(zoneName, host));
            }
            else
            {
                query = String.Format("SELECT * FROM MicrosoftDNS_SRVType " +
                "WHERE ContainerName = '{0}' AND OwnerName ='{1}'",
                zoneName, CorrectHostName(zoneName, host));
            }



            if (domainName != null)
                query += String.Format(" AND SRVDomainName = '{0}.'", domainName);

            using (ManagementObjectCollection objRRs = wmi.ExecuteQuery(query))
            {
                foreach (ManagementObject objRR in objRRs) using (objRR)
                        objRR.Delete();
            }

            // update SOA record
            UpdateSoaRecord(zoneName);
        }
        #endregion

        #region private helper methods
        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <returns></returns>
        private string GetDnsServerName()
        {
            using (ManagementObject objServer = wmi.GetObject("MicrosoftDNS_Server.Name=\".\""))
            {
                return (string)objServer.Properties["Name"].Value;
            }
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="recordText"></param>
        /// <returns></returns>
        private string AddDnsRecord(string zoneName, string recordText)
        {
            // get the name of the server
            string serverName = GetDnsServerName();

            // add record
            using (ManagementClass clsRR = wmi.GetClass("MicrosoftDNS_ResourceRecord"))
            {
                object[] prms = new object[] { serverName, zoneName, recordText, null };
                clsRR.InvokeMethod("CreateInstanceFromTextRepresentation", prms);
                return (string)prms[3];
            }
        }

        private string CorrectHostName(string zoneName, string host)
        {
            // if host is empty or null
            if (host == null || host == "")
                return zoneName;

                // if there are not dot at all
            else if (host.IndexOf(".") == -1)
                return host + "." + zoneName;

                // if only one dot at the end
            else if (host[host.Length - 1] == '.' && host.IndexOf(".") == (host.Length - 1))
                return host + zoneName;

                // other cases
            else
                return host;
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
                        Log.WriteError(String.Format("Error deleting '{0}' MS DNS zone", item.Name), ex);
                    }
                }
            }
        }

        /// <summary>
        /// Supports managed resources disposal
        /// </summary>
        /// <returns></returns>
        protected bool IsDNSInstalled()
        {
            using (RegistryKey root = Registry.LocalMachine)
            {
                using (RegistryKey key = root.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\DNS"))
                {
                    bool res = key != null;
                    if (key != null)
                        key.Close();

                    return res;
                }
            }
        }

        public override bool IsInstalled()
        {
            return IsDNSInstalled();
        }

        #endregion
    }
}
