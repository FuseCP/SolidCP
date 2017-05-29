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
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

using SolidCP.Server.Utils;
using SolidCP.Providers.Utils;
using System.IO;
using System.Reflection;

namespace SolidCP.Providers.DNS
{
    public class PowerDNS : HostingServiceProviderBase, IDnsServer
    {
        #region Constants

        //pdns mysql db settings
        const string PDNSDbServer = "PDNSDbServer";
        const string PDNSDbPort = "PDNSDbPort";
        const string PDNSDbName = "PDNSDbName";
        const string PDNSDbUser = "PDNSDbUser";
        const string PDNSDbPassword = "PDNSDbPassword";

        //soa record settings
        const string ResponsiblePerson = "ResponsiblePerson";
        const string RefreshInterval = "RefreshInterval";
        const string RetryDelay = "RetryDelay";
        const string ExpireLimit = "ExpireLimit";
        const string MinimumTTL = "MinimumTTL";

        //name servers
        const string NameServers = "NameServers";

        #endregion

		#region Static ctor

		static PowerDNS()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			//
			if (!args.Name.Contains("MySql.Data"))
				return null;
			//
			string connectorKeyName = "SOFTWARE\\MySQL AB\\MySQL Connector/Net";
			string connectorLocation = String.Empty;
			string connectorVersion = String.Empty;
			//
			if (PInvoke.RegistryHive.HKLM.SubKeyExists_x86(connectorKeyName))
			{
				connectorLocation = PInvoke.RegistryHive.HKLM.GetSubKeyValue_x86(connectorKeyName, "Location");
				connectorVersion = PInvoke.RegistryHive.HKLM.GetSubKeyValue_x86(connectorKeyName, "Version");
			}
			//
			if (String.IsNullOrEmpty(connectorLocation))
			{
				Log.WriteInfo("Connector location is either null or empty");
				return null;
			}
			//
			string assemblyFile = String.Empty;
			// Versions 5.x.x compatibility
			if (connectorVersion.StartsWith("5."))
				assemblyFile = Path.Combine(connectorLocation, @"Binaries\.NET 2.0\" + args.Name.Split(',')[0] + ".dll");
			// Newest versions compatibility
			else
				assemblyFile = Path.Combine(connectorLocation, @"Assemblies\" + args.Name.Split(',')[0] + ".dll");
			//
			Log.WriteInfo(assemblyFile);
			//
			if (!File.Exists(assemblyFile))
			{
				Log.WriteInfo("Connector assembly could not be found or does not exist");
				return null;
			}
			//
			return Assembly.LoadFrom(assemblyFile);
		}

		#endregion

        #region Provider Properties

        public string SOARefreshInterval
        {
            get { return ProviderSettings[RefreshInterval]; }
        }

        public string SOARetryDelay
        {
            get { return ProviderSettings[RetryDelay]; }
        }

        public string SOAExpireLimit
        {
            get { return ProviderSettings[ExpireLimit]; }
        }

        public string SOAMinimumTTL
        {
            get { return ProviderSettings[MinimumTTL]; }
        }

        public string DNSNameServers
        {
            get { return ProviderSettings[NameServers]; }
        }

        public string DbServer
        {
            get { return ProviderSettings[PDNSDbServer]; }
        }

        public string DbPort
        {
            get { return ProviderSettings[PDNSDbPort]; }
        }

        public string DbName
        {
            get { return ProviderSettings[PDNSDbName]; }
        }

        public string DbUser
        {
            get { return ProviderSettings[PDNSDbUser]; }
        }

        public string DbPassword
        {
            get { return ProviderSettings[PDNSDbPassword]; }
        }

        #endregion

        #region IDnsServer Members

        /// <summary>
        /// Used to check whether the particular zone exits.
        /// Return true if yes and false otherwise.
        /// </summary>
        /// <param name="zoneName">Name of zone to be found.</param>
        /// <returns>Bollean value. True if zone exists and false otherwise.</returns>
        public bool ZoneExists(string zoneName)
        {
            bool result = false;

            try
            {
                MySqlParameter ZoneName = new MySqlParameter("?ZoneName", MySqlDbType.VarString);
                ZoneName.Value = zoneName;

                IDataReader reader = ExecuteReader(
                      "SELECT name FROM domains WHERE name = ?ZoneName LIMIT 1"
                    , ZoneName
                );

                if (reader.Read())
                {
                    result = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error locating '{0}' Power DNS zone", zoneName), ex);
            }

            return result;
        }


        /// <summary>
        /// Returns the list of zones from Power DNS.
        /// </summary>
        /// <returns>Array of zone names</returns>
        public string[] GetZones()
        {
            List<string> zones = new List<string>();

            try
            {
                IDataReader reader = ExecuteReader(
                    "SELECT name FROM domains"
                );

                while (reader.Read())
                {
                    zones.Add(
                        reader["name"].ToString()
                    );
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Log.WriteError("Error getting the list of Power DNS zones.", ex);
            }

            return zones.ToArray();
        }


        /// <summary>
        /// This method is being called when Power DNS server is a primary DNS server.
        /// By primary we mean that Power DNS is the only DNS server, or it is primary (can have secondary) 
        /// server for this zone.
        /// </summary>
        /// <param name="zoneName">Domain name in the Power DNS database.</param>
        /// <param name="secondaryServers">IP Addresses of servers bound to Power DNS server</param>
        public void AddPrimaryZone(string zoneName, string[] secondaryServers)
        {
            PDNSAddZone(zoneName, "MASTER", secondaryServers);
        }


        /// <summary>
        /// This methodis is being called when Power DNS server is a secondary DNS server for some other DNS server.
        /// So, firstly, AddPrimaryZone is callled for those DNS server to which Power DNS is bound and only then, 
        /// this method is called to write zone data to Power DNS.
        /// </summary>
        /// <param name="zoneName">Domain name in Power DNS database.</param>
        /// <param name="masterServers">Primary DNS for current Power DNS instance.</param>
        public void AddSecondaryZone(string zoneName, string[] masterServers)
        {
            PDNSAddZone(zoneName, "SLAVE", masterServers);
        }

        /// <summary>
        /// Removes zone from Power DNS database by name.
        /// </summary>
        /// <param name="zoneName"></param>
        public void DeleteZone(string zoneName)
        {
            try
            {
                string domainId = GetDomainId(zoneName);

                if (string.IsNullOrEmpty(domainId))
                    return;

                MySqlParameter DomainId = new MySqlParameter("?DomainId", MySqlDbType.Int32);
                DomainId.Value = domainId;

                //clear records table
                ExecuteNonQuery(
                    "DELETE FROM records WHERE domain_id = ?DomainId"
                    , DomainId
                );

                //clear domains table
                ExecuteNonQuery(
                    "DELETE FROM domains WHERE id = ?DomainId"
                    , DomainId
                );
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error deleting '{0}' Power DNS zone", zoneName), ex);
            }
        }

        //partially tested
        public void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson)
        {
            string recordId = string.Empty;
            string recordContent = string.Empty;

            try
            {
                MySqlParameter ZoneName = new MySqlParameter("?Name", MySqlDbType.VarString);
                ZoneName.Value = zoneName;

                MySqlParameter RecordType = new MySqlParameter("?Type", MySqlDbType.VarString);
                RecordType.Value = ConvertDnsRecordTypeToString(DnsRecordType.SOA);

                IDataReader reader = ExecuteReader(
                     "SELECT id, content FROM records WHERE name = ?Name AND type = ?Type LIMIT 1"
                     , ZoneName
                     , RecordType
                 );

                if (reader.Read())
                {
                    recordId = reader["id"].ToString();
                    recordContent = reader["content"].ToString();
                }

                reader.Close();


                if (string.IsNullOrEmpty(recordId))
                    throw new ArgumentOutOfRangeException(string.Format("Not SOA record for Power DNS zone '{0}'", zoneName));

                PowerDnsSOARecordData soaData = PowerDnsSOARecordData.FromString(recordContent);

                soaData.PrimaryNameServer = primaryNsServer;
                soaData.HostMaster = primaryPerson;
                soaData.Refresh = SOARefreshInterval;
                soaData.Retry = SOARetryDelay;
                soaData.DefaultTTL = SOAMinimumTTL;
                soaData.Expire = SOAExpireLimit;

                soaData.IncrementSerial(
                    GetDomainNotifiedSerial(zoneName)
                );

                UpdateRecord(recordId, soaData.ToString());
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error updating SOA record for '{0}' Power DNS zone", zoneName), ex);
            }
        }

        //TESTED: partially
        /// <summary>
        /// The caller of this method is not waiting for SOA records. Assuming this, one should return all records except SOA.
        /// </summary>
        /// <param name="zoneName">Corresponds to the Domain name in the Power DNS domains table.</param>
        /// <returns>All DnsRecords except of SOA type.</returns>
        public DnsRecord[] GetZoneRecords(string zoneName)
        {
            List<DnsRecord> records = new List<DnsRecord>();

            try
            {
                string domainId = GetDomainId(zoneName);
                if (string.IsNullOrEmpty(domainId))
                    throw new ArgumentOutOfRangeException("Power DNS zone '{0}' does not exist.");

                MySqlParameter DomainId = new MySqlParameter("?DomainId", MySqlDbType.Int32);
                DomainId.Value = domainId;

                IDataReader reader = ExecuteReader(
                    "SELECT * FROM records WHERE domain_id = ?DomainId AND type != 'SOA'"
                    , DomainId
                );

                while (reader.Read())
                {
                    DnsRecord record = new DnsRecord();

                    record.RecordData = reader["content"].ToString();
                    record.RecordName = CorrectHost(zoneName, reader["name"].ToString());
                    record.RecordType = ConvertStringToDnsRecordType(reader["type"].ToString());

                    int mxPriority = 0;
                    if (!string.IsNullOrEmpty(reader["prio"].ToString()))
                    if (Int32.TryParse(reader["prio"].ToString(), out mxPriority))
                    {
                        record.MxPriority = mxPriority;
                    }

                    records.Add(record);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error getting records for '{0}' Power DNS zone", zoneName), ex);
            }

            return records.ToArray();
        }

        /// <summary>
        /// Adds zone record into Power DNS database
        /// </summary>
        /// <param name="zoneName">Domain name in Power DNS database.</param>
        /// <param name="record">Record in Power DNS database</param>
        public void AddZoneRecord(string zoneName, DnsRecord record)
        {
            PDNSAddZoneRecord(zoneName, record, true);          
        }

        public void AddZoneRecords(string zoneName, DnsRecord[] records)
        {
            foreach (DnsRecord record in records)
            {
                PDNSAddZoneRecord(zoneName, record, false);
            }

            PDNSUpdateSoaRecord(zoneName);
        }

        /// <summary>
        /// Delete zone record from Power DNS database.
        /// </summary>
        /// <param name="zoneName">Domain name from Power DNS database</param>
        /// <param name="record">Record from Power DNS database</param>
        public void DeleteZoneRecord(string zoneName, DnsRecord record)
        {
            PDNSDeleteZoneRecord(zoneName, record, true);
        }

        public void DeleteZoneRecords(string zoneName, DnsRecord[] records)
        {
            foreach (DnsRecord record in records)
            {
                PDNSDeleteZoneRecord(zoneName, record, false);
            }

            PDNSUpdateSoaRecord(zoneName);
        }

        #endregion

        #region Zone

        /// <summary>
        /// Adds zone in Power DNS domains table and creates a SOA record for it.
        /// </summary>
        /// <param name="zoneName">Domain name in Power DNS database.</param>
        /// <param name="zoneType"></param>
        /// <param name="nameServers"></param>
        protected void PDNSAddZone(string zoneName, string zoneType, string[] nameServers)
        {
            string domainId = AddDomainAndReturnDomainId(zoneName, zoneType);
            if (string.IsNullOrEmpty(domainId))
                throw new ArgumentOutOfRangeException(string.Format("Unable to add Power DNS zone '{0}'.", zoneName));

            //create SOA record
            PowerDnsSOARecordData data = new PowerDnsSOARecordData();

            data.DefaultTTL = SOAMinimumTTL;
            data.Expire = SOAExpireLimit;
            data.Refresh = SOARefreshInterval;
            data.Retry = SOARetryDelay;
            data.Serial = DateTime.Now.ToString("yyyyMMdd") + "01";

            //add SOA record
            AddRecord(
                  domainId
                , zoneName
                , data.ToString()
                , ConvertDnsRecordTypeToString(DnsRecordType.SOA)
                , GetDefaultRecordTTL(DnsRecordType.SOA)
                , "0"
            );


            //add NS records for secondary servers
            foreach (string server in nameServers)
            {
                AddRecord(
                      domainId
                    , zoneName
                    , server
                    , ConvertDnsRecordTypeToString(DnsRecordType.NS)
                    , GetDefaultRecordTTL(DnsRecordType.NS)
                    , "0"
                );
            }
        }

        #endregion

        #region Zone Record

        protected void PDNSAddZoneRecord(string zoneName, DnsRecord record, bool isNeedToUpdateSOA)
        {
            string domainId = GetDomainId(zoneName);
            if (domainId == string.Empty)
                throw new ArgumentOutOfRangeException("DomainId not found. Zone does not exist.");


            string recordType = ConvertDnsRecordTypeToString(record.RecordType);
            string ttl = GetDefaultRecordTTL(record.RecordType);


            //NS record
            if (record.RecordType == DnsRecordType.NS)
            {
                if (string.IsNullOrEmpty(record.RecordName))
                    record.RecordName = zoneName;
            }

            //widen record name for Power DNS 
            if (!string.IsNullOrEmpty(record.RecordName))
            {
                if (!record.RecordName.Contains(zoneName))
                {
                    record.RecordName = string.Format("{0}.{1}", record.RecordName, zoneName);
                }
            }
            else
            {
                record.RecordName = zoneName;
            }


            AddRecord(
                  domainId
                , record.RecordName
                , record.RecordData
                , recordType
                , ttl
                , record.MxPriority.ToString()
            );

            if (isNeedToUpdateSOA)
            {
                PDNSUpdateSoaRecord(zoneName);
            }
        }

        protected void PDNSDeleteZoneRecord(string zoneName, DnsRecord record, bool isNeedToUpdateSOA)
        {
            string recordName = zoneName;
            if (!String.IsNullOrEmpty(record.RecordName))
            {
                recordName = string.Format("{0}.{1}", record.RecordName, recordName);
            }

            RemoveRecord(
                  recordName
                , ConvertDnsRecordTypeToString(record.RecordType)
                , record.RecordData
                , record.MxPriority
            );

            if (isNeedToUpdateSOA)
            {
                PDNSUpdateSoaRecord(zoneName);
            }
        }

        #endregion

        #region SOA Record

        /// <summary>
        /// Updates SOA record of the corresponding zone
        /// </summary>
        /// <param name="zoneName">Domain name in Power DNS database</param>
        protected void PDNSUpdateSoaRecord(string zoneName)
        {
            try
            {
                string domainId = GetDomainId(zoneName);
                if (string.IsNullOrEmpty(domainId))
                    throw new ArgumentOutOfRangeException(string.Format("Power DNS zone '{0}' does not exist.", domainId));


                MySqlParameter DomainId = new MySqlParameter("?DomainId", MySqlDbType.Int32);
                DomainId.Value = domainId;

                IDataReader reader = ExecuteReader(
                      "SELECT id, content FROM records WHERE domain_id = ?DomainId AND type='SOA' LIMIT 1"
                      , DomainId
                );

                string
                    recordId = string.Empty,
                    recordContent = string.Empty;

                if (reader.Read())
                {
                    recordId = reader["id"].ToString();
                    recordContent = reader["content"].ToString();
                }

                if (string.IsNullOrEmpty(recordId))
                    throw new ArgumentOutOfRangeException(string.Format("SOA record for Power DNS zone '{0}' not found", zoneName));


                PowerDnsSOARecordData data = PowerDnsSOARecordData.FromString(recordContent);

                data.Refresh = SOARefreshInterval;
                data.Retry = SOARetryDelay;
                data.DefaultTTL = SOAMinimumTTL;
                data.Expire = SOAExpireLimit;

                data.IncrementSerial(GetDomainNotifiedSerial(zoneName));

                UpdateRecord(recordId, data.ToString());
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error updating SOA record for '{0}' Power DNS zone", zoneName), ex);
            }
        }

        #endregion

        #region Database Helper Methods

        protected string GetConnectionString()
        {
            return string.Format(
                      "server={0};port={1};database={2};uid={3};password={4}"
                    , DbServer
                    , DbPort
                    , DbName
                    , DbUser
                    , DbPassword
                );
        }

        public IDataReader ExecuteReader(string mySqlStatement, params MySqlParameter[] parameters)
        {
            MySqlConnection mysqlConnection = new MySqlConnection(GetConnectionString());
            mysqlConnection.Open();

            MySqlCommand command = new MySqlCommand(
                  mySqlStatement
                , mysqlConnection
            );

            foreach (MySqlParameter param in parameters)
            {
                command.Parameters.Add(param);
            }

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public int ExecuteNonQuery(string mySqlStatement, params MySqlParameter[] parameters)
        {
            MySqlConnection mysqlConnection = new MySqlConnection(GetConnectionString());
            mysqlConnection.Open();

            MySqlCommand command = new MySqlCommand(
                  mySqlStatement
                , mysqlConnection
            );

            foreach (MySqlParameter param in parameters)
            {
                command.Parameters.Add(param);
            }

            int returnValue = command.ExecuteNonQuery();

            mysqlConnection.Close();

            return returnValue;
        }


        #endregion

        #region Record Helper Methods


        /// <summary>
        /// Removes record by it's name and type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        void RemoveRecord(string name, string type, string content, int mxPriority)
        {
            try
            {
                MySqlParameter RecordName = new MySqlParameter("?RecordName", MySqlDbType.VarString);
                RecordName.Value = name;

                MySqlParameter RecordType = new MySqlParameter("?RecordType", MySqlDbType.VarString);
                RecordType.Value = type;

                MySqlParameter RecordContent = new MySqlParameter("?RecordContent", MySqlDbType.VarString);
                RecordContent.Value = "%" + content.Trim() + "%";

                MySqlParameter RecordPriority = new MySqlParameter("?RecordPriority", MySqlDbType.Int32);
                RecordPriority.Value = mxPriority;

                ExecuteNonQuery(
                    "DELETE FROM records WHERE name = ?RecordName AND type = ?RecordType AND prio = ?RecordPriority AND content like ?RecordContent "
                    , RecordName
                    , RecordType
                    , RecordContent
                    , RecordPriority
                );
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error removing record '{0}' of type '{1}' from Power DNS", name, type), ex);
            }
        }

        /// <summary>
        /// Add record to Power DNS database
        /// </summary>
        /// <param name="domainId"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <param name="ttl"></param>
        /// <param name="prio"></param>
        protected void AddRecord(string domainId, string name, string content, string type, string ttl, string prio)
        {
            try
            {
                MySqlParameter DomainId = new MySqlParameter("?DomainId", MySqlDbType.Int32);
                DomainId.Value = domainId;

                MySqlParameter RecordName = new MySqlParameter("?RecordName", MySqlDbType.VarString);
                RecordName.Value = name;

                MySqlParameter RecordContent = new MySqlParameter("?RecordContent", MySqlDbType.VarString);
                RecordContent.Value = content;

                MySqlParameter RecordType = new MySqlParameter("?RecordType", MySqlDbType.VarString);
                RecordType.Value = type;

                MySqlParameter RecordTtl = new MySqlParameter("?RecordTtl", MySqlDbType.Int32);
                RecordTtl.Value = ttl;

                MySqlParameter RecordPriority = new MySqlParameter("?RecordPriority", MySqlDbType.Int32);
                RecordPriority.Value = prio;

                ExecuteNonQuery(
                     "INSERT INTO records (domain_id, name, content, type, ttl, prio) " +
                     "VALUES (?DomainId, ?RecordName, ?RecordContent, ?RecordType, ?RecordTtl, ?RecordPriority)"
                     , DomainId
                     , RecordName
                     , RecordContent
                     , RecordType
                     , RecordTtl
                     , RecordPriority
                 );

            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error adding record '{0}' of type '{1}' in Power DNS", name, type), ex);
            }
        }

        /// <summary>
        /// Updates record's content field
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="content"></param>
        protected void UpdateRecord(string recordId, string content)
        {
            try
            {
                MySqlParameter RecordContent = new MySqlParameter("?RecordContent", MySqlDbType.VarString);
                RecordContent.Value = content;

                MySqlParameter RecordId = new MySqlParameter("?RecordId", MySqlDbType.Int32);
                RecordId.Value = recordId;

                ExecuteNonQuery(
                    "UPDATE records SET content = ?RecordContent " +
                    "WHERE id = ?RecordId LIMIT 1"
                    , RecordContent
                    , RecordId
                );
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error updating record with id '{0}' in Power DNS", recordId), ex);
            }
        }

        /// <summary>
        /// Provides a default TTLs for records.
        /// </summary>
        /// <param name="recordType"></param>
        /// <returns></returns>
        public string GetDefaultRecordTTL(DnsRecordType recordType)
        {
            string ttl = string.Empty;

            switch (recordType)
            {
                case DnsRecordType.SOA:
                    ttl = SOAMinimumTTL;
                    break;

                case DnsRecordType.NS:
                    ttl = "3600";
                    break;

                default:
                    ttl = "120";
                    break;
            }

            return ttl;
        }


        /// <summary>
        /// Represents SOA record contents in Power DNS database.
        /// Allows to perform Increment operation.
        /// </summary>
        internal class PowerDnsSOARecordData
        {
            public string PrimaryNameServer = "localhost";
            public string HostMaster = "hostmaster.yourdomain.com";
            public string Serial = "1";
            public string Refresh = "10800";
            public string Retry = "3600";
            public string Expire = "84600";
            public string DefaultTTL = "3600";

            /// <summary>
            /// Converts <see cref="PowerDnsSOARecordData"/> class instance to string representation.
            /// </summary>
            /// <returns>SOA record content according to the Power DNS SOA record content format</returns>
            public override string ToString()
            {
                return string.Format(
                    "{0} {1} {2} {3} {4} {5} {6}"
                    , PrimaryNameServer
                    , HostMaster
                    , Serial
                    , Refresh
                    , Retry
                    , Expire
                    , DefaultTTL
                ).Trim();
            }

            /// <summary>
            /// Creates <see cref="PowerDnsSOARecordData"/> class instance from string.
            /// </summary>
            /// <param name="content">Record content from Power DNS database.</param>
            /// <returns>Instance of <see cref="PowerDnsSOARecordData"/> class.</returns>
            public static PowerDnsSOARecordData FromString(string content)
            {
                PowerDnsSOARecordData data = new PowerDnsSOARecordData();

                string[] contentParts = content
                                        .Trim()
                                        .Split(' ');

                for (int i = 0; i < contentParts.Length; i++)
                {
                    data.UpdateProperty(i, contentParts[i]);
                }

                return data;
            }

            /// <summary>
            /// Increments Serial number of SOA record according to RFC for DNS format.
            /// </summary>
            /// <param name="notifiedSerial">Is a serial number of SOA record notified by primary server.</param>
            public void IncrementSerial(string notifiedSerial)
            {
                string todayDate = DateTime.Now.ToString("yyyyMMdd");

                if (notifiedSerial.Contains(todayDate))
                {
                    Serial = notifiedSerial;
                }

                if (Serial.Length < 10 || !Serial.StartsWith(todayDate))
                {
                    Serial = todayDate + "01";
                }
                else
                {
                    Serial = (UInt32.Parse(Serial) + 1).ToString();
                }
            }

            private void UpdateProperty(int index, string data)
            {
                switch (index)
                {
                    case 0:
                        PrimaryNameServer = data;
                        break;

                    case 1:
                        HostMaster = data;
                        break;

                    case 2:
                        Serial = data;
                        break;

                    case 3:
                        Refresh = data;
                        break;

                    case 4:
                        Retry = data;
                        break;

                    case 5:
                        Expire = data;
                        break;

                    case 6:
                        DefaultTTL = data;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("Unknown index.");
                }
            }
        }

        #endregion

        #region Domain Record Helpers

        /// <summary>
        /// Determines domain id by its name.
        /// </summary>
        /// <param name="domainName">Domain name in Power DNS database.</param>
        /// <returns>Domain Id</returns>
        protected string GetDomainId(string domainName)
        {
            string domainId = string.Empty;

            try
            {
                MySqlParameter DomainName = new MySqlParameter("?DomainName", MySqlDbType.VarString);
                DomainName.Value = domainName;

                IDataReader reader = ExecuteReader(
                    "SELECT id FROM domains WHERE name = ?DomainName LIMIT 1"
                    , DomainName
                );

                if (reader.Read())
                {
                    domainId = reader["id"].ToString();
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error getting domain '{0}' id from Power DNS.", domainName), ex);
            }

            return domainId;
        }


        /// <summary>
        /// Determines notified_serial value from Domains table in Power DNS database.
        /// </summary>
        /// <param name="domainName">Domain name in Power DNS database.</param>
        /// <returns>string represeting notified_serial field.</returns>
        protected string GetDomainNotifiedSerial(string domainName)
        {
            string domainId = string.Empty;

            try
            {
                MySqlParameter DomainName = new MySqlParameter("?DomainName", MySqlDbType.VarString);
                DomainName.Value = domainName;

                IDataReader reader = ExecuteReader(
                    "SELECT notified_serial FROM domains WHERE name = ?DomainName LIMIT 1"
                    , DomainName
                );

                if (reader.Read())
                {
                    domainId = reader["notified_serial"].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error getting notofied serial for domain '{0}' from Power DNS.", domainName), ex);
            }

            return domainId;
        }

        /// <summary>
        /// Inserts domain into Power DNS database and returns it's id.
        /// </summary>
        /// <param name="zoneName">Domain name in Power DNS database.</param>
        /// <param name="domainType">Can be Native or Slave</param>
        /// <returns>An id of the inserted domain</returns>
        protected string AddDomainAndReturnDomainId(string zoneName, string domainType)
        {
            AddDomain(zoneName, domainType);

            return GetDomainId(zoneName);
        }

        /// <summary>
        /// Add domain into Power DNS database.
        /// </summary>
        /// <param name="zoneName">Domain name in Power DNS database.</param>
        /// <param name="domainType">Can be Native or Slave</param>
        protected void AddDomain(string zoneName, string domainType)
        {
            try
            {
                MySqlParameter DomainName = new MySqlParameter("?DomainName", MySqlDbType.VarString);
                DomainName.Value = zoneName;

                MySqlParameter DomainType = new MySqlParameter("?DomainType", MySqlDbType.VarString);
                DomainType.Value = domainType;

                ExecuteNonQuery(
                    "INSERT INTO domains (name, type) VALUES (?DomainName, ?DomainType)"
                    , DomainName
                    , DomainType
                );
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error adding domain '{0}' of type '{1}' to Power DNS", zoneName, domainType), ex);
            }
        }

        #endregion

        #region Conversion Methods

        public DnsRecordType ConvertStringToDnsRecordType(string recordType)
        {
            DnsRecordType result = DnsRecordType.Other;

            switch (recordType)
            {
                case "A":
                    result = DnsRecordType.A;
                    break;

				case "AAAA":
					result = DnsRecordType.AAAA;
					break;

				case "CNAME":
                    result = DnsRecordType.CNAME;
                    break;

                case "MX":
                    result = DnsRecordType.MX;
                    break;

                case "NS":
                    result = DnsRecordType.NS;
                    break;

                case "TXT":
                    result = DnsRecordType.TXT;
                    break;

                case "SOA":
                    result = DnsRecordType.SOA;
                    break;

                default:
                    result = DnsRecordType.Other;
                    break;
            }

            return result;
        }

        public string ConvertDnsRecordTypeToString(DnsRecordType recordType)
        {
            string result = string.Empty;

            switch (recordType)
            {
                case DnsRecordType.A:
                    result = "A";
                    break;
				
				case DnsRecordType.AAAA:
					result = "AAAA";
					break;
                
                case DnsRecordType.CNAME:
                    result = "CNAME";
                    break;

                case DnsRecordType.MX:
                    result = "MX";
                    break;

                case DnsRecordType.NS:
                    result = "NS";
                    break;

                case DnsRecordType.SOA:
                    result = "SOA";
                    break;

                case DnsRecordType.TXT:
                    result = "TXT";
                    break;

                case DnsRecordType.Other:
                default:
                    throw new NotSupportedException("Record type not supported.");
            }

            return result;
        }

        #endregion

        /// <summary>
        /// If zoneName stored in Power DNS is the same as record name (host) for this zone the the return value is string.Empty.
        /// If record name different, then we delete zone name from record name ans return the rest.
        /// </summary>
        /// <param name="zoneName">Represents domain name in Power DNS database.</param>
        /// <param name="host">FQDN, record name in Power DNS database.</param>
        /// <returns>Third Level Domain name or empty string if zoneName == host.</returns>
        private string CorrectHost(string zoneName, string host)
        {
            if (host == String.Empty)
                return host;

            if (host.ToLower() == zoneName.ToLower())
                return string.Empty;
            else
                return host.Substring(0, (host.Length - zoneName.Length - 1));
        }

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
                        Log.WriteError(String.Format("Error deleting '{0}' Power DNS zone", item.Name), ex);
                    }
                }
            }
        }

        public override bool IsInstalled()
        {
            return false;
        }

    }
}
