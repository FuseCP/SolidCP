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

		bool Bool(string txt) => txt.Equals("YES", StringComparison.OrdinalIgnoreCase);
		public bool Listen { get => Bool(this["listen"]); set => this["listen"] = value ? "YES" : "NO"; }
		public bool ListenIpV6 { get => Bool(this["listen_ipv6"]); set => this["listen_ipv6"] = value ? "YES" : "NO"; }
		public bool AnonymousEnable { get => Bool(this["anonymous_enable"]); set => this["anonymous_enable"] = value ? "YES" : "NO"; }
		public bool LocalEnable { get => Bool(this["local_enable"]); set => this["local_enable"] = value ? "YES" : "NO"; }
		public bool WriteEnable { get => Bool(this["write_enable"]); set => this["write_enable"] = value ? "YES" : "NO"; }
		public string LocalUmask { get => this["local_umask"]; set => this["local_umask"] = value; }
		public string LocalRoot { get => this["local_root"]; set => this["local_root"] = value; }
		public bool ChrootLocalUser { get => Bool(this["chroot_local_user"]); set => this["chroot_local_user"] = value ? "YES" : "NO"; }
		public bool AllowWritableChroot { get => Bool(this["allow_writable_chroot"]); set => this["allow_writable_chroot"] = value ? "YES" : "NO"; }
		public bool HideIds { get => Bool(this["hide_ids"]); set => this["hide_ids"] = value ? "YES" : "NO"; }
		public string UserConfigDir { get => this["user_config_dir"]; set => this["user_config_dir"] = value; }
		public bool GuestEnable { get => Bool(this["guest_enable"]); set => this["guest_enable"] = value ? "YES" : "NO"; }
		public bool VirtualUseLocalPrivs { get => Bool(this["virtual_use_local_privs"]); set => this["virtual_use_local_privs"] = value ? "YES" : "NO"; }
		public string PamServiceName { get => this["pam_service_name"]; set => this["pam_service_name"] = value; }
		public string NoprivUser { get => this["nopriv_user"]; set => this["nopriv_user"] = value; }
		public string GuestUsername { get => this["guest_username"]; set => this["guest_username"] = value; }
	}
}

