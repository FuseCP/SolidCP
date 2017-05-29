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
	public class MsDNS2012: HostingServiceProviderBase, IDnsServer
    {

        #region Properties
        protected TimeSpan ExpireLimit
		{
			get { return ProviderSettings.GetTimeSpan( "ExpireLimit" ); }
		}

        protected TimeSpan MinimumTTL
		{
            get { return ProviderSettings.GetTimeSpan("MinimumTTL"); }
		}

        protected TimeSpan RefreshInterval
		{
            get { return ProviderSettings.GetTimeSpan("RefreshInterval"); }
		}

        protected TimeSpan RetryDelay
		{
            get { return ProviderSettings.GetTimeSpan("RetryDelay"); }
		}

		protected bool AdMode
		{
			get { return ProviderSettings.GetBool( "AdMode" ); }
		}
        #endregion

        private PowerShellHelper ps = null;
		private bool bulkRecords;

		public MsDNS2012()
		{
			// Create PowerShell helper
			ps = new PowerShellHelper();
			if( !this.IsInstalled() )
				return;
		}

		#region Zones

		public virtual string[] GetZones()
		{
			return ps.Get_DnsServerZone_Names();
		}

		public virtual bool ZoneExists( string zoneName )
		{
			return ps.ZoneExists( zoneName );
		}

		public virtual DnsRecord[] GetZoneRecords( string zoneName )
		{
			return ps.GetZoneRecords( zoneName );
		}

		public virtual void AddPrimaryZone( string zoneName, string[] secondaryServers )
		{
			ps.Add_DnsServerPrimaryZone( zoneName, secondaryServers, AdMode);

            // remove ns records
            ps.Remove_DnsServerResourceRecords(zoneName, "NS");
		}

		public virtual void AddSecondaryZone( string zoneName, string[] masterServers )
		{
			ps.Add_DnsServerSecondaryZone( zoneName, masterServers );
        }

		public virtual void DeleteZone( string zoneName )
		{
			try
			{
				ps.Remove_DnsServerZone( zoneName );
			}
			catch( Exception ex )
			{
				Log.WriteError( ex );
			}
		}

		public virtual void AddZoneRecord( string zoneName, DnsRecord record )
		{
			try
			{
				string name = record.RecordName;
				if( String.IsNullOrEmpty( name ) )
					name = ".";

				if( record.RecordType == DnsRecordType.A )
					ps.Add_DnsServerResourceRecordA( zoneName, name, record.RecordData );
				else if( record.RecordType == DnsRecordType.AAAA )
					ps.Add_DnsServerResourceRecordAAAA( zoneName, name, record.RecordData );
				else if( record.RecordType == DnsRecordType.CNAME )
					ps.Add_DnsServerResourceRecordCName( zoneName, name, record.RecordData );
				else if( record.RecordType == DnsRecordType.MX )
					ps.Add_DnsServerResourceRecordMX( zoneName, name, record.RecordData, (ushort)record.MxPriority );
				else if( record.RecordType == DnsRecordType.NS )
					ps.Add_DnsServerResourceRecordNS( zoneName, name, record.RecordData );
				else if( record.RecordType == DnsRecordType.TXT )
					ps.Add_DnsServerResourceRecordTXT( zoneName, name, record.RecordData );
				else if( record.RecordType == DnsRecordType.SRV )
					ps.Add_DnsServerResourceRecordSRV( zoneName, name, record.RecordData, (ushort)record.SrvPort, (ushort)record.SrvPriority, (ushort)record.SrvWeight );
				else
					throw new Exception( "Unknown record type" );
			}
			catch( Exception ex )
			{
				// log exception
				Log.WriteError( ex );
			}
		}

		public virtual void AddZoneRecords( string zoneName, DnsRecord[] records )
		{
			bulkRecords = true;
			try
			{
				foreach( DnsRecord record in records )
					AddZoneRecord( zoneName, record );
			}
			finally
			{
				bulkRecords = false;
			}

			UpdateSoaRecord( zoneName );
		}

		public virtual void DeleteZoneRecord( string zoneName, DnsRecord record )
		{
			try
			{
				string rrType;
				if( !RecordTypes.rrTypeFromRecord.TryGetValue( record.RecordType, out rrType ) )
					throw new Exception( "Unknown record type" );
				ps.Remove_DnsServerResourceRecord( zoneName, record);
			}
			catch( Exception ex )
			{
				// log exception
				Log.WriteError( ex );
			}
		}

		public virtual void DeleteZoneRecords( string zoneName, DnsRecord[] records )
		{
			foreach( DnsRecord record in records )
				DeleteZoneRecord( zoneName, record );
		}

		#endregion

		#region SOA Record
		public virtual void UpdateSoaRecord( string zoneName, string host, string primaryNsServer, string primaryPerson )
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


		public override void DeleteServiceItems( ServiceProviderItem[] items )
		{
			foreach( ServiceProviderItem item in items )
			{
				if( item is DnsZone )
				{
					try
					{
						// delete DNS zone
						DeleteZone( item.Name );
					}
					catch( Exception ex )
					{
						Log.WriteError( String.Format( "Error deleting '{0}' MS DNS zone", item.Name ), ex );
					}
				}
			}
		}

		public override bool IsInstalled()
		{
			return ps.Test_DnsServer();
		}
	}
}
