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
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using SolidCP.Server.Utils;
using Microsoft.Management.Infrastructure;


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
		private static Command addParam( this Command cmd, string name, object value )
		{
			cmd.Parameters.Add( name, value );
			return cmd;
		}

		/// <summary>Add parameter without value to the PS command</summary>
		/// <param name="cmd">command</param>
		/// <param name="name">Parameter name</param>
		/// <returns>Same command</returns>
		private static Command addParam( this Command cmd, string name )
		{
			// http://stackoverflow.com/a/10304080/126995
			cmd.Parameters.Add( name, true );
			return cmd;
		}

		/// <summary>Create "Where-Object -Property ... -eq -Value ..." command</summary>
		/// <param name="property"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private static Command where( string property, object value )
		{
			return new Command( "Where-Object" )
				.addParam( "Property", property )
				.addParam( "eq" )
				.addParam( "Value", value );
		}

		/// <summary>Test-DnsServer -IPAddress 127.0.0.1</summary>
		/// <param name="ps">PowerShell host to use</param>
		/// <returns>true if localhost is an MS DNS server</returns>
		public static bool Test_DnsServer( this PowerShellHelper ps )
		{
			if( null == ps )
				throw new ArgumentNullException( "ps" );

			var cmd = new Command( "Test-DnsServer" )
				.addParam( "IPAddress", IPAddress.Loopback );

			PSObject res = ps.RunPipeline( cmd ).FirstOrDefault();
			PSPropertyInfo p = res.Properties["Result"];
			if (null == res || null == res.Properties)
				return false;
			else
				return true;
		}

		#region Zones

		/// <summary>Get-DnsServerZone | Select-Object -Property ZoneName</summary>
		/// <remarks>Only primary DNS zones are returned</remarks>
		/// <returns>Array of zone names</returns>
		public static string[] Get_DnsServerZone_Names( this PowerShellHelper ps )
		{
			var allZones = ps.RunPipeline( new Command( "Get-DnsServerZone" ),
				where( "IsAutoCreated", false ) );

			string[] res = allZones
				.Select( pso => new
				{
					name = (string)pso.Properties[ "ZoneName" ].Value,
					type = (string)pso.Properties[ "ZoneType" ].Value
				} )
				.Where( obj => obj.type == "Primary" )
				.Select( obj => obj.name )
				.ToArray();

			Log.WriteInfo( "Get_DnsServerZone_Names: {{{0}}}", String.Join( ", ", res ) );
			return res;
		}

		/// <summary>Returns true if the specified zone exists.</summary>
		/// <remarks>The PS pipeline being run: Get-DnsServerZone | Where-Object -Property ZoneName -eq -Value "name"</remarks>
		/// <param name="ps"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool ZoneExists( this PowerShellHelper ps, string name )
		{
			Log.WriteStart( "ZoneExists {0}", name );
			bool res = ps.RunPipeline( new Command( "Get-DnsServerZone" ),
				where( "ZoneName", name ) )
				.Any();
			Log.WriteEnd( "ZoneExists: {0}", res );
			return res;
		}

		/* public enum eReplicationScope: byte
		{
			Custom, Domain, Forest, Legacy
		} */

		/// <summary></summary>
		/// <param name="ps"></param>
		/// <param name="zoneName"></param>
		/// <param name="replicationScope">Specifies a partition on which to store an Active Directory-integrated zone.</param>
		/// <returns></returns>
		public static void Add_DnsServerPrimaryZone( this PowerShellHelper ps, string zoneName, string[] secondaryServers, bool AdMode )
		{
			Log.WriteStart( "Add_DnsServerPrimaryZone {0} {{{1}}}", zoneName, String.Join( ", ", secondaryServers ) );

			// Add-DnsServerPrimaryZone -Name zzz.com -ZoneFile zzz.com.dns
			var cmd = new Command( "Add-DnsServerPrimaryZone" );
			cmd.addParam( "Name", zoneName );
            
            // Add AD zone if required
            if (AdMode)
                { cmd.addParam("ReplicationScope", "Forest"); }
            else
                { cmd.addParam("ZoneFile", zoneName + ".dns"); }


            ps.RunPipeline( cmd );

			// Set-DnsServerPrimaryZone -Name zzz.com -SecureSecondaries ... -Notify ... Servers ..
			cmd = new Command( "Set-DnsServerPrimaryZone" );
			cmd.addParam( "Name", zoneName );

			if( secondaryServers == null || secondaryServers.Length == 0 )
			{
				// transfers are not allowed
				// inParams2[ "SecureSecondaries" ] = 3;
				// inParams2[ "Notify" ] = 0;
				cmd.addParam( "SecureSecondaries", "NoTransfer" );
				cmd.addParam( "Notify", "NoNotify" );
			}
			else if( secondaryServers.Length == 1 && secondaryServers[ 0 ] == "*" )
			{
				// allowed transfer from all servers
				// inParams2[ "SecureSecondaries" ] = 0;
				// inParams2[ "Notify" ] = 1;
				cmd.addParam( "SecureSecondaries", "TransferAnyServer" );
				cmd.addParam( "Notify", "Notify" );
			}
			else
			{
				// allowed transfer from specified servers
				// inParams2[ "SecureSecondaries" ] = 2;
				// inParams2[ "SecondaryServers" ] = secondaryServers;
				// inParams2[ "NotifyServers" ] = secondaryServers;
				// inParams2[ "Notify" ] = 2;
				cmd.addParam( "SecureSecondaries", "TransferToSecureServers" );
				cmd.addParam( "Notify", "NotifyServers" );
				cmd.addParam( "SecondaryServers", secondaryServers );
				cmd.addParam( "NotifyServers", secondaryServers );
			}
			ps.RunPipeline( cmd );
			Log.WriteEnd( "Add_DnsServerPrimaryZone" );
		}

		/// <summary>Call Add-DnsServerSecondaryZone cmdlet</summary>
		/// <param name="ps"></param>
		/// <param name="zoneName">a name of a zone</param>
		/// <param name="masterServers">an array of IP addresses of the master servers of the zone. You can use both IPv4 and IPv6.</param>
		public static void Add_DnsServerSecondaryZone( this PowerShellHelper ps, string zoneName, string[] masterServers )
		{
			// Add-DnsServerSecondaryZone -Name zzz.com -ZoneFile zzz.com.dns -MasterServers ...
			var cmd = new Command( "Add-DnsServerSecondaryZone" );
			cmd.addParam( "Name", zoneName );
			cmd.addParam( "ZoneFile", zoneName + ".dns" );
			cmd.addParam( "MasterServers", masterServers );
			ps.RunPipeline( cmd );
		}

		public static void Remove_DnsServerZone( this PowerShellHelper ps, string zoneName )
		{
			var cmd = new Command( "Remove-DnsServerZone" );
			cmd.addParam( "Name", zoneName );
			cmd.addParam( "Force" );
			ps.RunPipeline( cmd );
		}
		#endregion

		/// <summary>Get all records, except the SOA</summary>
		/// <param name="ps"></param>
		/// <param name="zoneName">Name of the zone</param>
		/// <returns>Array of records</returns>
		public static DnsRecord[] GetZoneRecords( this PowerShellHelper ps, string zoneName )
		{
			// Get-DnsServerResourceRecord -ZoneName xxxx.com
			var allRecords = ps.RunPipeline( new Command( "Get-DnsServerResourceRecord" ).addParam( "ZoneName", zoneName ) );

			return allRecords.Select( o => o.asDnsRecord( zoneName ) )
				.Where( r => null != r )
				.Where( r => r.RecordType != DnsRecordType.SOA )
			//	.Where( r => !( r.RecordName == "@" && DnsRecordType.NS == r.RecordType ) )
                .OrderBy( r => r.RecordName )
                .ThenBy( r => r.RecordType )
                .ThenBy( r => r.RecordData )
				.ToArray();
		}

		#region Records add / remove

		public static void Add_DnsServerResourceRecordA( this PowerShellHelper ps, string zoneName, string Name, string address )
		{
			var cmd = new Command( "Add-DnsServerResourceRecordA" );
			cmd.addParam( "ZoneName", zoneName );
			cmd.addParam( "Name", Name );
			cmd.addParam( "IPv4Address", address );
			ps.RunPipeline( cmd );
		}

		public static void Add_DnsServerResourceRecordAAAA( this PowerShellHelper ps, string zoneName, string Name, string address )
		{
			var cmd = new Command( "Add-DnsServerResourceRecordAAAA" );
			cmd.addParam( "ZoneName", zoneName );
			cmd.addParam( "Name", Name );
			cmd.addParam( "IPv6Address", address );
			ps.RunPipeline( cmd );
		}

		public static void Add_DnsServerResourceRecordCName( this PowerShellHelper ps, string zoneName, string Name, string alias )
		{
			var cmd = new Command( "Add-DnsServerResourceRecordCName" );
			cmd.addParam( "ZoneName", zoneName );
			cmd.addParam( "Name", Name );
			cmd.addParam( "HostNameAlias", alias );
			ps.RunPipeline( cmd );
		}

		public static void Add_DnsServerResourceRecordMX( this PowerShellHelper ps, string zoneName, string Name, string mx, UInt16 pref )
		{
			var cmd = new Command( "Add-DnsServerResourceRecordMX" );
			cmd.addParam( "ZoneName", zoneName );
			cmd.addParam( "Name", Name );
			cmd.addParam( "MailExchange", mx );
			cmd.addParam( "Preference", pref );
			ps.RunPipeline( cmd );
		}

		public static void Add_DnsServerResourceRecordNS( this PowerShellHelper ps, string zoneName, string Name, string NameServer )
		{
			var cmd = new Command( "Add-DnsServerResourceRecord" );
			cmd.addParam( "ZoneName", zoneName );
			cmd.addParam( "Name", Name );
			cmd.addParam( "NS" );
			cmd.addParam( "NameServer", NameServer );
			ps.RunPipeline( cmd );
		}

		public static void Add_DnsServerResourceRecordTXT( this PowerShellHelper ps, string zoneName, string Name, string txt )
		{
			var cmd = new Command( "Add-DnsServerResourceRecord" );
			cmd.addParam( "ZoneName", zoneName );
			cmd.addParam( "Name", Name );
			cmd.addParam( "Txt" );
			cmd.addParam( "DescriptiveText", txt );
			ps.RunPipeline( cmd );
		}

		public static void Add_DnsServerResourceRecordSRV( this PowerShellHelper ps, string zoneName, string Name, string DomainName, UInt16 Port, UInt16 Priority, UInt16 Weight )
		{
			var cmd = new Command( "Add-DnsServerResourceRecord" );
			cmd.addParam( "ZoneName", zoneName );
			cmd.addParam( "Name", Name );
			cmd.addParam( "Srv" );
			cmd.addParam( "DomainName", DomainName );
			cmd.addParam( "Port", Port );
			cmd.addParam( "Priority", Priority );
			cmd.addParam( "Weight", Weight );
			ps.RunPipeline( cmd );
		}

		public static void Remove_DnsServerResourceRecord( this PowerShellHelper ps, string zoneName, DnsRecord record)
		{
            string type;
            if (!RecordTypes.rrTypeFromRecord.TryGetValue(record.RecordType, out type))
					throw new Exception( "Unknown record type" );

            string Name = record.RecordName;
            if (String.IsNullOrEmpty(Name)) Name = "@";

            var cmd = new Command("Get-DnsServerResourceRecord");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("Name", Name);
            cmd.addParam("RRType", type);
            Collection<PSObject> resourceRecords = ps.RunPipeline(cmd);

            object inputObject = null;
            foreach (PSObject resourceRecord in resourceRecords)
            {
                DnsRecord dnsResourceRecord = resourceRecord.asDnsRecord(zoneName);

                bool found = false;

                switch(dnsResourceRecord.RecordType)
                {
                    case DnsRecordType.A:
                    case DnsRecordType.AAAA:
                    case DnsRecordType.CNAME:
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
                            &&(dnsResourceRecord.SrvPriority == record.SrvPriority)
                            &&(dnsResourceRecord.SrvWeight == record.SrvWeight)
                            &&(dnsResourceRecord.SrvPort == record.SrvPort);
                        break;
                }

                if (found)
                {
                    inputObject = resourceRecord;
                    break;
                }
            }

			cmd = new Command( "Remove-DnsServerResourceRecord" );
			cmd.addParam( "ZoneName", zoneName );
            cmd.addParam("InputObject", inputObject);

			cmd.addParam( "Force" );
			ps.RunPipeline( cmd );
		}
        
        public static void Remove_DnsServerResourceRecords(this PowerShellHelper ps, string zoneName, string type)
        {
            var cmd = new Command("Get-DnsServerResourceRecord");
            cmd.addParam("ZoneName", zoneName);
            cmd.addParam("RRType", type);
            Collection<PSObject> resourceRecords = ps.RunPipeline(cmd);

            foreach (PSObject resourceRecord in resourceRecords)
            {
                cmd = new Command("Remove-DnsServerResourceRecord");
                cmd.addParam("ZoneName", zoneName);
                cmd.addParam("InputObject", resourceRecord);

                cmd.addParam("Force");
                ps.RunPipeline(cmd);
            }
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

            if (recordData==null) return;
            
            if (ExpireLimit!=null)
                recordData.CimInstanceProperties["ExpireLimit"].Value = ExpireLimit;

            if (MinimumTimeToLive!=null)
                recordData.CimInstanceProperties["MinimumTimeToLive"].Value = MinimumTimeToLive;

            if (PrimaryServer!=null)
                recordData.CimInstanceProperties["PrimaryServer"].Value = PrimaryServer;

            if (RefreshInterval!=null)
                recordData.CimInstanceProperties["RefreshInterval"].Value = RefreshInterval;

            if (ResponsiblePerson!=null)
                recordData.CimInstanceProperties["ResponsiblePerson"].Value = ResponsiblePerson;

            if (RetryDelay!=null)
                recordData.CimInstanceProperties["RetryDelay"].Value = RetryDelay;

            if (PSComputerName!=null)
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
