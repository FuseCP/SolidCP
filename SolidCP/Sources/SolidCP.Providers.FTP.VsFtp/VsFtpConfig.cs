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
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;

using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.FTP;

namespace SolidCP.Providers.FTP
{
	public class VsFtpConfig
	{
		public string ConfigFile;

		public VsFtpConfig(string path) { ConfigFile = path; }

		static string text = null;

		public string Text
		{
			get
			{
				if (text == null)
				{
					text = File.ReadAllText(ConfigFile);
				}
				return text;
			}
			set
			{
				text = value;
			}
		}
		public void Save() => File.WriteAllText(ConfigFile, Text);

		public string this[string name]
		{
			get
			{
				var matches = Regex.Matches(@$"(?<=^\s*{name}\s*=\s*).*?(?=\s*$)", Text, RegexOptions.Multiline);
				return matches.OfType<Match>().LastOrDefault()?.Value;
			}
			set
			{
				bool exists = false;
				Text = Regex.Replace(Text, @$"(?<=^\s*{name}\s*=\s*).*?(?=\s*$)", (Match m) =>
				{
					exists = true;
					return value;
				}, RegexOptions.Multiline);
				if (!exists)
				{
					Text = Text + $"{Environment.NewLine}{name}={value}";
				}
			}
		}

		public string UserListFile => this["userlist_file"];
		public bool UserListDeny => this["userlist_deny"] == "YES";
		public bool UserListEnable => this["userlist_enable"] == "YES";
		public bool LocalEnable => this["local_enable"] == "YES";

		public IEnumerable<string> UserList => UserListFile != null ? File.ReadLines(UserListFile).Select(u => u.Trim()) : new string[0];


	}
}

