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
using System.Globalization;
using System.Linq;

namespace SolidCP.Providers.DNS
{
	/// <summary>This static class holds 2 lookup tables, from/to DnsRecordType enum</summary>
	internal static class RecordTypes
	{
		static readonly Dictionary<string, DnsRecordType> s_lookup;
		static readonly Dictionary<DnsRecordType, string> s_lookupInv;

		static RecordTypes()
		{
			s_lookup = new Dictionary<string, DnsRecordType>()
			{
				{ "A",     DnsRecordType.A     },
				{ "AAAA",  DnsRecordType.AAAA  },
				{ "NS",    DnsRecordType.NS    },
				{ "MX",    DnsRecordType.MX    },
				{ "CNAME", DnsRecordType.CNAME },
				{ "SOA",   DnsRecordType.SOA   },
				{ "TXT",   DnsRecordType.TXT   },
				{ "SRV",   DnsRecordType.SRV   },
			};

			TextInfo ti = new CultureInfo( "en-US", false ).TextInfo;

			s_lookupInv = s_lookup
				.ToDictionary( kvp => kvp.Value, kvp => ti.ToTitleCase( kvp.Key ) );
		}

		/// <summary>The dictionary that maps string record types to DnsRecordType enum</summary>
		public static Dictionary<string, DnsRecordType> recordFromString { get { return s_lookup; } }

		/// <summary>the dictionary that maps DnsRecordType enum to strings, suitable for PowerShell </summary>
		public static Dictionary<DnsRecordType, string> rrTypeFromRecord { get { return s_lookupInv; } }
	}
}
