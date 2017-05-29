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
using System.Globalization;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;

namespace SolidCP.Import.Enterprise
{
	public class ADUtils
	{
		public static string ADUsername
		{
			get
			{
				return ConfigurationManager.AppSettings["AD.Username"];
			}
		}

		public static string ADPassword
		{
			get
			{
				return ConfigurationManager.AppSettings["AD.Password"];
			}
		}

		public static string ADRootDomain
		{
			get { return Global.ADRootDomain; }
		}

		public static string PrimaryDomainController
		{
			get { return Global.PrimaryDomainController; }
		}

		public static string RootOU
		{
			get { return Global.RootOU; }
		}

		public static DirectoryEntry GetRootOU()
		{
			StringBuilder sb = new StringBuilder();
			// append provider
			AppendProtocol(sb);
			AppendDomainController(sb);
			AppendOUPath(sb, RootOU);
			AppendDomainPath(sb, ADRootDomain);

			DirectoryEntry de = GetADObject(sb.ToString());
			//ExchangeLog.LogEnd("GetRootOU");
			return de;
		}

		private static void AppendProtocol(StringBuilder sb)
		{
			sb.Append("LDAP://");
		}

		private static void AppendDomainController(StringBuilder sb)
		{
			string dc = PrimaryDomainController;
			if (dc.IndexOf(".") != -1)
				dc = dc.Substring(0, dc.IndexOf("."));
			sb.Append(dc + "/");
		}

		private static void AppendOUPath(StringBuilder sb, string ou)
		{
			if (string.IsNullOrEmpty(ou))
				return;

			string path = ou.Replace("/", "\\");
			string[] parts = path.Split('\\');
			for (int i = parts.Length - 1; i != -1; i--)
				sb.Append("OU=").Append(parts[i]).Append(",");
		}

		private static void AppendDomainPath(StringBuilder sb, string domain)
		{
			if (string.IsNullOrEmpty(domain))
				return;

			string[] parts = domain.Split('.');
			for (int i = 0; i < parts.Length; i++)
			{
				sb.Append("DC=").Append(parts[i]);

				if (i < (parts.Length - 1))
					sb.Append(",");
			}
		}

		private static DirectoryEntry GetADObject(string path)
		{
			DirectoryEntry de = null;
			de = new DirectoryEntry(path, ADUsername, ADPassword);
			de.RefreshCache();
			return de;
		}

		public static string RemoveADPrefix(string path)
		{
			string dn = path;
			if (dn.ToUpper().StartsWith("LDAP://"))
			{
				dn = dn.Substring(7);
			}
			int index = dn.IndexOf("/");

			if (index != -1)
			{
				dn = dn.Substring(index + 1);
			}
			return dn;
		}

		public static string GetAddressListsContainer()
		{
			StringBuilder sb = new StringBuilder();
			AppendProtocol(sb);
			AppendDomainController(sb);
			sb.Append("CN=Microsoft Exchange,CN=Services,CN=Configuration,");
			AppendDomainPath(sb, ADRootDomain);
			DirectoryEntry exchEntry = GetADObject(sb.ToString());
			DirectoryEntry orgEntry = null;
			foreach (DirectoryEntry child in exchEntry.Children)
			{
				orgEntry = child;
				break;
			}
			string ret = "CN=Address Lists Container," + RemoveADPrefix(orgEntry.Path);
			return ret;
		}
	}
}
