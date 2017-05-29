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
using System.Linq;
using System.Management.Automation;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Generic;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.DNS
{
	/// <summary>Copy fields from CimInstance#DnsServerResourceRecord into DnsRecord</summary>
	/// <remarks>It's also possible to access native CIM object, and use Mgmtclassgen.exe for that.</remarks>
	internal static class RecordConverter
	{
		internal static string RemoveTrailingDot( string str )
		{
			if( !str.EndsWith( "." ) )
				return str;
			return str.Substring( 0, str.Length - 1 );
		}

		internal static string CorrectHost( string zoneName, string host )
		{
			if( "@" == host || host.ToLower() == zoneName.ToLower() )
				return String.Empty;

			if( host.ToLower().EndsWith( "." + zoneName.ToLower() ) )
				return host.Substring( 0, ( host.Length - zoneName.Length - 1 ) );
			return host;
		}

		public static DnsRecord asDnsRecord( this PSObject obj, string zoneName )
		{
			// Here's what comes from Server 2012 in the TypeNames:
			// "Microsoft.Management.Infrastructure.CimInstance#root/Microsoft/Windows/DNS/DnsServerResourceRecord"
			// "Microsoft.Management.Infrastructure.CimInstance#ROOT/Microsoft/Windows/DNS/DnsDomain"
			// "Microsoft.Management.Infrastructure.CimInstance#DnsServerResourceRecord"
			// "Microsoft.Management.Infrastructure.CimInstance#DnsDomain"
			// "Microsoft.Management.Infrastructure.CimInstance"
			// "System.Object"	string

			if( !obj.TypeNames.Contains( "Microsoft.Management.Infrastructure.CimInstance#DnsServerResourceRecord" ) )
			{
				Log.WriteWarning( "asDnsRecord: wrong object type {0}", obj.TypeNames.FirstOrDefault() );
				return null;
			}

			string strRT = (string)obj.Properties[ "RecordType" ].Value;
			DnsRecordType tp;
			if( !RecordTypes.recordFromString.TryGetValue( strRT, out tp ) )
				return null;

			/*// Debug code below: 
			obj.dumpProperties();
			CimInstance rd = (CimInstance)obj.Properties[ "RecordData" ].Value;
			rd.dumpProperties(); //*/

			CimKeyedCollection<CimProperty> data = ( (CimInstance)obj.Properties[ "RecordData" ].Value ).CimInstanceProperties;
			string host = CorrectHost( zoneName, (string)obj.Properties[ "HostName" ].Value );

			switch( tp )
			{
				// The compiler should create a Dictionary<> from dis switch
				case DnsRecordType.A:
					{
						return new DnsRecord()
						{
							RecordType = tp,
							RecordName = host,
							RecordData = data[ "IPv4Address" ].Value as string,
						};
					}
				case DnsRecordType.AAAA:
					{
						return new DnsRecord()
						{
							RecordType = tp,
							RecordName = host,
							RecordData = data[ "IPv6Address" ].Value as string,
						};
					}
				case DnsRecordType.CNAME:
					{
						return new DnsRecord()
						{
							RecordType = tp,
							RecordName = host,
							RecordData = RemoveTrailingDot( data[ "HostNameAlias" ].Value as string ),
						};
					}
				case DnsRecordType.MX:
					{
						return new DnsRecord()
						{
							RecordType = tp,
							RecordName = host,
							RecordData = RemoveTrailingDot( data[ "MailExchange" ].Value as string ),
							MxPriority = (UInt16)data[ "Preference" ].Value,
						};
					}
				case DnsRecordType.NS:
					{
						return new DnsRecord()
						{
							RecordType = tp,
							RecordName = host,
							RecordData = RemoveTrailingDot( data[ "NameServer" ].Value as string ),
						};
					}
				case DnsRecordType.TXT:
					{
						return new DnsRecord()
						{
							RecordType = tp,
							RecordName = host,
							RecordData = data[ "DescriptiveText" ].Value as string,
						};
					}
				case DnsRecordType.SOA:
					{
						string PrimaryServer = data[ "PrimaryServer" ].Value as string;
						string ResponsiblePerson = data[ "ResponsiblePerson" ].Value as string;
						UInt32? sn = (UInt32?)data[ "SerialNumber" ].Value;
						return new DnsSOARecord()
						{
							RecordType = tp,
							RecordName = host,
							PrimaryNsServer = PrimaryServer,
							PrimaryPerson = ResponsiblePerson,
							SerialNumber = ( sn.HasValue ) ? sn.Value.ToString() : null,
						};
					}
				case DnsRecordType.SRV:
					{
						return new DnsRecord()
						{
							RecordType = tp,
							RecordName = host,
							RecordData = RemoveTrailingDot( data[ "DomainName" ].Value as string ),
							SrvPriority = (UInt16)data[ "Priority" ].Value,
							SrvWeight = (UInt16)data[ "Weight" ].Value,
							SrvPort = (UInt16)data[ "Port" ].Value,
						};
					}
			}
			return null;
		}
	}
}
