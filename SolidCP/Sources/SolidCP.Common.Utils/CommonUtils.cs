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
using System.Data;
using System.Collections;
using System.Xml;
using System.Web;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Common.Utils
{
	public class Utils
	{
		public static bool IsEmpty(string str)
		{
			return (str == null || str.Trim() == "");
		}

		public static bool IsNotEmpty(string str)
		{
			return !IsEmpty(str);
		}

		public static int ParseInt(string val, int defaultValue)
		{
			int result = defaultValue;
			try { result = Int32.Parse(val); }
			catch { /* do nothing */ }
			return result;
		}

		public static decimal ParseDecimal(string val, decimal defaultValue)
		{
			decimal result = defaultValue;
			try { result = Decimal.Parse(val); }
			catch { /* do nothing */ }
			return result;
		}

		public static string[] ParseDelimitedString(string str, params char[] delimiter)
		{
			string[] parts = str.Split(delimiter);
			ArrayList list = new ArrayList();
			foreach (string part in parts)
				if (part.Trim() != "" && !list.Contains(part.Trim()))
					list.Add(part);
			return (string[])list.ToArray(typeof(string));
		}

		public static string ReplaceStringVariable(string str, string variable, string value)
		{
			if (IsEmpty(str) || IsEmpty(value))
				return str;

			Regex re = new Regex("\\[" + variable + "\\]+", RegexOptions.IgnoreCase);
			return re.Replace(str, value);
		}

		public static string BuildIdentityXmlFromArray(int[] ids, string rootName, string childName)
		{
			XmlDocument doc = new XmlDocument();
			XmlElement nodeRoot = doc.CreateElement(rootName);
			foreach (int id in ids)
			{
				XmlElement nodeChild = doc.CreateElement(childName);
				nodeChild.SetAttribute("id", id.ToString());
				nodeRoot.AppendChild(nodeChild);
			}

			return nodeRoot.OuterXml;
		}
	}
}
