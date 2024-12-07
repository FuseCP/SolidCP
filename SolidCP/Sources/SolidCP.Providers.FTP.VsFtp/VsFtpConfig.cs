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

		string text = null;

		public string Text
		{
			get
			{
				if (text == null && File.Exists(ConfigFile))
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
		public void Save() => File.WriteAllText(ConfigFile, Text+Environment.NewLine);

		public string this[string name]
		{
			get
			{
				var matches = Regex.Matches(Text ?? "", @$"(?<=^\s*{Regex.Escape(name)}\s*=\s*).*?(?=\s*$)", RegexOptions.Multiline);
				return matches.OfType<Match>().LastOrDefault()?.Value;
			}
			set
			{
				bool exists = false;
				Text = (Text ?? "").Trim();
				Text = Regex.Replace(Text, @$"(?<=^\s*{name}\s*=\s*).*?(?=\s*$)", (Match m) =>
				{
					exists = true;
					return value;
				}, RegexOptions.Multiline);
				if (!exists)
				{
					if (!string.IsNullOrWhiteSpace(Text)) Text += $"{Environment.NewLine}";
					Text = Text + $"{name}={value}";
				}
			}
		}

		bool Bool(string txt, bool defaultValue = false) => txt?.Equals("YES", StringComparison.OrdinalIgnoreCase) ?? defaultValue;

		// bool settings
		public bool AllowAnonSsl { get => Bool(this["allow_anon_ssl"], false); set => this["allow_anon_ssl"] = value ? "YES" : "NO"; }
		public bool AllowWriteableChroot { get => Bool(this["allow_writeable_chroot"], false); set => this["allow_writeable_chroot"] = value ? "YES" : "NO"; }
		public bool AnonMkdirWriteEnable { get => Bool(this["anon_mkdir_write_enable"], false); set => this["anon_mkdir_write_enable"] = value ? "YES" : "NO"; }
		public bool AnonOtherWriteEnable { get => Bool(this["anon_other_write_enable"], false); set => this["anon_other_write_enable"] = value ? "YES" : "NO"; }
		public bool AnonUploadEnable { get => Bool(this["anon_upload_enable"], false); set => this["anon_upload_enable"] = value ? "YES" : "NO"; }
		public bool AnonWorldReadableOnly { get => Bool(this["anon_world_readable_only"], true); set => this["anon_world_readable_only"] = value ? "YES" : "NO"; }
		public bool AnonymousEnable { get => Bool(this["anonymous_enable"], true); set => this["anonymous_enable"] = value ? "YES" : "NO"; }
		public bool AsciiDownloadEnable { get => Bool(this["ascii_download_enable"], false); set => this["ascii_download_enable"] = value ? "YES" : "NO"; }
		public bool AsciiUploadEnable { get => Bool(this["ascii_upload_enable"], false); set => this["ascii_upload_enable"] = value ? "YES" : "NO"; }
		public bool AsyncAborEnable { get => Bool(this["async_abor_enable"], false); set => this["async_abor_enable"] = value ? "YES" : "NO"; }
		public bool Background { get => Bool(this["background"], true); set => this["background"] = value ? "YES" : "NO"; }
		public bool CheckShell { get => Bool(this["check_shell"], true); set => this["check_shell"] = value ? "YES" : "NO"; }
		public bool ChmodEnable { get => Bool(this["chmod_enable"], true); set => this["chmod_enable"] = value ? "YES" : "NO"; }
		public bool ChownUploads { get => Bool(this["chown_uploads"], false); set => this["chown_uploads"] = value ? "YES" : "NO"; }
		public bool ChrootListEnable { get => Bool(this["chroot_list_enable"], false); set => this["chroot_list_enable"] = value ? "YES" : "NO"; }
		public bool ChrootLocalUser { get => Bool(this["chroot_local_user"], false); set => this["chroot_local_user"] = value ? "YES" : "NO"; }
		public bool ConnectFromPort20 { get => Bool(this["connect_from_port_20"], false); set => this["connect_from_port_20"] = value ? "YES" : "NO"; }
		public bool DebugSsl { get => Bool(this["debug_ssl"], false); set => this["debug_ssl"] = value ? "YES" : "NO"; }
		public bool DeleteFailedUploads { get => Bool(this["delete_failed_uploads"], false); set => this["delete_failed_uploads"] = value ? "YES" : "NO"; }
		public bool DenyEmailEnable { get => Bool(this["deny_email_enable"], false); set => this["deny_email_enable"] = value ? "YES" : "NO"; }
		public bool DirlistEnable { get => Bool(this["dirlist_enable"], true); set => this["dirlist_enable"] = value ? "YES" : "NO"; }
		public bool DirmessageEnable { get => Bool(this["dirmessage_enable"], false); set => this["dirmessage_enable"] = value ? "YES" : "NO"; }
		public bool DownloadEnable { get => Bool(this["download_enable"], true); set => this["download_enable"] = value ? "YES" : "NO"; }
		public bool DualLogEnable { get => Bool(this["dual_log_enable"], false); set => this["dual_log_enable"] = value ? "YES" : "NO"; }
		public bool ForceDotFiles { get => Bool(this["force_dot_files"], false); set => this["force_dot_files"] = value ? "YES" : "NO"; }
		public bool ForceAnonDataSsl { get => Bool(this["force_anon_data_ssl"], false); set => this["force_anon_data_ssl"] = value ? "YES" : "NO"; }
		public bool ForceAnonLoginsSsl { get => Bool(this["force_anon_logins_ssl"], false); set => this["force_anon_logins_ssl"] = value ? "YES" : "NO"; }
		public bool ForceLocalDataSsl { get => Bool(this["force_local_data_ssl"], true); set => this["force_local_data_ssl"] = value ? "YES" : "NO"; }
		public bool ForceLocalLoginsSsl { get => Bool(this["force_local_logins_ssl"], true); set => this["force_local_logins_ssl"] = value ? "YES" : "NO"; }
		public bool GuestEnable { get => Bool(this["guest_enable"], false); set => this["guest_enable"] = value ? "YES" : "NO"; }
		public bool HideIds { get => Bool(this["hide_ids"], false); set => this["hide_ids"] = value ? "YES" : "NO"; }
		public bool ImplicitSsl { get => Bool(this["implicit_ssl"], false); set => this["implicit_ssl"] = value ? "YES" : "NO"; }
		public bool Listen { get => Bool(this["listen"], false); set => this["listen"] = value ? "YES" : "NO"; }
		public bool ListenIpV6 { get => Bool(this["listen_ipv6"], false); set => this["listen_ipv6"] = value ? "YES" : "NO"; }
		public bool LocalEnable { get => Bool(this["local_enable"], false); set => this["local_enable"] = value ? "YES" : "NO"; }
		public bool LockUploadFiles { get => Bool(this["lock_upload_files"], true); set => this["lock_upload_files"] = value ? "YES" : "NO"; }
		public bool LogFtpProtocol { get => Bool(this["log_ftp_protocol"], false); set => this["log_ftp_protocol"] = value ? "YES" : "NO"; }
		public bool LsRecurseEnable { get => Bool(this["ls_recurse_enable"], false); set => this["ls_recurse_enable"] = value ? "YES" : "NO"; }
		public bool MdtmWrite { get => Bool(this["mdtm_write"], true); set => this["mdtm_write"] = value ? "YES" : "NO"; }
		public bool NoAnonPassword { get => Bool(this["no_anon_password"], false); set => this["no_anon_password"] = value ? "YES" : "NO"; }
		public bool NoLogLock { get => Bool(this["no_log_lock"], false); set => this["no_log_lock"] = value ? "YES" : "NO"; }
		public bool OneProcessModel { get => Bool(this["one_process_model"], false); set => this["one_process_model"] = value ? "YES" : "NO"; }
		public bool PasswdChrootEnable { get => Bool(this["passwd_chroot_enable"], false); set => this["passwd_chroot_enable"] = value ? "YES" : "NO"; }
		public bool PasvAddrResolve { get => Bool(this["pasv_addr_resolve"], false); set => this["pasv_addr_resolve"] = value ? "YES" : "NO"; }
		public bool PasvEnable { get => Bool(this["pasv_enable"], true); set => this["pasv_enable"] = value ? "YES" : "NO"; }
		public bool PasvPromiscuous { get => Bool(this["pasv_promiscuous"], false); set => this["pasv_promiscuous"] = value ? "YES" : "NO"; }
		public bool PortEnable { get => Bool(this["port_enable"], true); set => this["port_enable"] = value ? "YES" : "NO"; }
		public bool PortPromiscuous { get => Bool(this["port_promiscuous"], false); set => this["port_promiscuous"] = value ? "YES" : "NO"; }
		public bool RequireCert { get => Bool(this["require_cert"], false); set => this["require_cert"] = value ? "YES" : "NO"; }
		public bool RequireSslReuse { get => Bool(this["require_ssl_reuse"], true); set => this["require_ssl_reuse"] = value ? "YES" : "NO"; }
		public bool ReverseLookupEnable { get => Bool(this["reverse_lookup_enable"], true); set => this["reverse_lookup_enable"] = value ? "YES" : "NO"; }
		public bool RunAsLaunchingUser { get => Bool(this["run_as_launching_user"], false); set => this["run_as_launching_user"] = value ? "YES" : "NO"; }
		public bool SecureEmailListEnable { get => Bool(this["secure_email_list_enable"], false); set => this["secure_email_list_enable"] = value ? "YES" : "NO"; }
		public bool SessionSupport { get => Bool(this["session_support"], false); set => this["session_support"] = value ? "YES" : "NO"; }
		public bool SetproctitleEnable { get => Bool(this["setproctitle_enable"], false); set => this["setproctitle_enable"] = value ? "YES" : "NO"; }
		public bool SslEnable { get => Bool(this["ssl_enable"], false); set => this["ssl_enable"] = value ? "YES" : "NO"; }
		public bool SslRequestCert { get => Bool(this["ssl_request_cert"], true); set => this["ssl_request_cert"] = value ? "YES" : "NO"; }
		public bool SslSslv2 { get => Bool(this["ssl_sslv2"], false); set => this["ssl_sslv2"] = value ? "YES" : "NO"; }
		public bool SslSslv3 { get => Bool(this["ssl_sslv3"], false); set => this["ssl_sslv3"] = value ? "YES" : "NO"; }
		public bool SslTlsv1 { get => Bool(this["ssl_tlsv1"], true); set => this["ssl_tlsv1"] = value ? "YES" : "NO"; }
		public bool StrictSslReadEof { get => Bool(this["strict_ssl_read_eof"], false); set => this["strict_ssl_read_eof"] = value ? "YES" : "NO"; }
		public bool StrictSslWriteShutdown { get => Bool(this["strict_ssl_write_shutdown"], false); set => this["strict_ssl_write_shutdown"] = value ? "YES" : "NO"; }
		public bool SyslogEnable { get => Bool(this["syslog_enable"], false); set => this["syslog_enable"] = value ? "YES" : "NO"; }
		public bool TcpWrappers { get => Bool(this["tcp_wrappers"], false); set => this["tcp_wrappers"] = value ? "YES" : "NO"; }
		public bool TextUserdbNames { get => Bool(this["text_userdb_names"], false); set => this["text_userdb_names"] = value ? "YES" : "NO"; }
		public bool TildeUserEnable { get => Bool(this["tilde_user_enable"], false); set => this["tilde_user_enable"] = value ? "YES" : "NO"; }
		public bool UseLocaltime { get => Bool(this["use_localtime"], false); set => this["use_localtime"] = value ? "YES" : "NO"; }
		public bool UseSendfile { get => Bool(this["use_sendfile"], true); set => this["use_sendfile"] = value ? "YES" : "NO"; }
		public bool UserlistDeny { get => Bool(this["userlist_deny"], true); set => this["userlist_deny"] = value ? "YES" : "NO"; }
		public bool UserlistEnable { get => Bool(this["userlist_enable"], false); set => this["userlist_enable"] = value ? "YES" : "NO"; }
		public bool ValidateCert { get => Bool(this["validate_cert"], false); set => this["validate_cert"] = value ? "YES" : "NO"; }
		public bool UserlistLog { get => Bool(this["userlist_log"], false); set => this["userlist_log"] = value ? "YES" : "NO"; }
		public bool VirtualUseLocalPrivs { get => Bool(this["virtual_use_local_privs"], false); set => this["virtual_use_local_privs"] = value ? "YES" : "NO"; }
		public bool WriteEnable { get => Bool(this["write_enable"], false); set => this["write_enable"] = value ? "YES" : "NO"; }
		public bool XferlogEnable { get => Bool(this["xferlog_enable"], false); set => this["xferlog_enable"] = value ? "YES" : "NO"; }
		public bool XferlogStdFormat { get => Bool(this["xferlog_std_format"], false); set => this["xferlog_std_format"] = value ? "YES" : "NO"; }
		public bool IsolateNetwork { get => Bool(this["isolate_network"], true); set => this["isolate_network"] = value ? "YES" : "NO"; }
		public bool Isolate { get => Bool(this["isolate"], true); set => this["isolate"] = value ? "YES" : "NO"; }


		// int settings
		public int Int(string value, int defaultValue)
		{
			if (!string.IsNullOrEmpty(value))
			{
				int result;
				if (int.TryParse(value, out result)) return result;
			}
			return defaultValue;
		}
		public int AcceptTimeout { get => Int(this["accept_timeout"], 60); set => this["accept_timeout"] = value.ToString(); }
		public int AnonMaxRate { get => Int(this["accept_timeout"], 60); set => this["accept_timeout"] = value.ToString(); }
		public string AnonUmask { get => this["anon_umask"] ?? "077"; set => this["anon_umask"] = value; }
		public string ChownUploadMode { get => this["anon_umask"] ?? "0600"; set => this["anon_umask"] = value; }
		public int ConnectTimeout { get => Int(this["connect_timeout"], 60); set => this["connect_timeout"] = value.ToString(); }
		public int DataConnectionTimeout { get => Int(this["data_connection_timeout"], 300); set => this["data_connection_timeout"] = value.ToString(); }
		public int DelayFailedLogin { get => Int(this["delay_failed_login"], 1); set => this["delay_failed_login"] = value.ToString(); }
		public int DelaySuccessfulLogin { get => Int(this["delay_successful_login"], 0); set => this["delay_successful_login"] = value.ToString(); }
		public string FileOpenMode { get => this["file_open_mode"] ?? "0666"; set => this["file_open_mode"] = value; }
		public int FtpDataPort { get => Int(this["ftp_data_port"], 20); set => this["ftp_data_port"] = value.ToString(); }
		public int IdleSessionTimeout { get => Int(this["idle_session_timeout"], 300); set => this["idle_session_timeout"] = value.ToString(); }
		public int ListenPort { get => Int(this["listen_port"], 21); set => this["listen_port"] = value.ToString(); }
		public int LocalMaxRate { get => Int(this["local_max_rate"], 0); set => this["local_max_rate"] = value.ToString(); }
		public string LocalUmask { get => this["local_umask"] ?? "077"; set => this["local_umask"] = value; }
		public int MaxClients { get => Int(this["max_clients"], 2000); set => this["max_clients"] = value.ToString(); }
		public int MaxLoginFails { get => Int(this["max_login_fails"], 3); set => this["max_login_fails"] = value.ToString(); }
		public int MaxPerIp { get => Int(this["max_per_ip"], 50); set => this["max_per_ip"] = value.ToString(); }
		public int PasvMaxPort { get => Int(this["pasv_max_port"], 0); set => this["pasv_max_port"] = value.ToString(); }
		public int PasvMinPort { get => Int(this["pasv_min_port"], 0); set => this["pasv_min_port"] = value.ToString(); }
		public int TransChunkSize { get => Int(this["trans_chunk_size"], 0); set => this["trans_chunk_size"] = value.ToString(); }

		// string settings
		public string AnonRoot { get => this["anon_root"]; set => this["anon_root"] = value; }
		public string BannedEmailFile { get => this["banned_email_file"] ?? "/etc/vsftpd/banned_emails"; set => this["banned_email_file"] = value; }
		public string BannerFile { get => this["banner_file"]; set => this["banner_file"] = value; }
		public string CaCertsFile { get => this["ca_certs_file"]; set => this["ca_certs_file"] = value; }
		public string ChownUsername { get => this["chown_username"] ?? "root"; set => this["chown_username"] = value; }
		public string ChrootListFile { get => this["chroot_list_file"] ?? "/etvsftpd.confc/vsftpd.chroot_list"; set => this["chroot_list_file"] = value; }
		public string CmdsAllowed { get => this["cmds_allowed"]; set => this["cmds_allowed"] = value; }
		public string CmdsDenied { get => this["cmds_denied"]; set => this["cmds_denied"] = value; }
		public string DenyFile { get => this["deny_file"]; set => this["deny_file"] = value; }
		public string DsaCertFile { get => this["dsa_cert_file"]; set => this["dsa_cert_file"] = value; }
		public string DsaPrivateKeyFile { get => this["dsa_private_key_file"]; set => this["dsa_private_key_file"] = value; }
		public string EmailPasswordFile { get => this["email_password_file"] ?? "/etc/vsftpd/email_passwords"; set => this["email_password_file"] = value; }
		public string FtpUsername { get => this["ftp_username"] ?? "ftp"; set => this["ftp_username"] = value; }
		public string FtpBanner { get => this["ftpd_banner"]; set => this["ftpd_banner"] = value; }
		public string GuestUsername { get => this["guest_username"] ?? "ftp"; set => this["guest_username"] = value; }
		public string HideFile { get => this["hide_file"]; set => this["hide_file"] = value; }
		public string ListenAddress { get => this["listen_address"]; set => this["listen_address"] = value; }
		public string ListenAddress6 { get => this["listen_address6"]; set => this["listen_address6"] = value; }
		public string LocalRoot { get => this["local_root"]; set => this["local_root"] = value; }
		public string MessageFile { get => this["message_file"] ?? ".message"; set => this["message_file"] = value; }
		public string NoprivUser { get => this["nopriv_user"] ?? "nobody"; set => this["nopriv_user"] = value; }
		public string PamServiceName { get => this["pam_service_name"] ?? "ftp"; set => this["pam_service_name"] = value; }
		public string PasvAddress { get => this["pasv_address"]; set => this["pasv_address"] = value; }
		public string RsaCertFile { get => this["rsa_cert_file"] ?? "/usr/share/ssl/certs/vsftpd.pem"; set => this["rsa_cert_file"] = value; }
		public string RsaPrivateKeyFile { get => this["rsa_private_key_file"]; set => this["rsa_private_key_file"] = value; }
		public string SecureChrootDir { get => this["secure_chroot_dir"] ?? "/usr/share/empty"; set => this["secure_chroot_dir"] = value; }
		public string SslCiphers { get => this["ssl_ciphers"] ?? "DES-CBC3-SHA"; set => this["ssl_ciphers"] = value; }
		public string UserConfigDir { get => this["user_config_dir"]; set => this["user_config_dir"] = value; }
		public string UserSubToken { get => this["user_sub_token"]; set => this["user_sub_token"] = value; }
		public string UserlistFile { get => this["userlist_file"] ?? "/etc/vsftpd/user_list"; set => this["userlist_file"] = value; }
		public string VsftpdLogFile { get => this["vsftpd_log_file"] ?? "/var/log/vsftpd.log"; set => this["vsftpd_log_file"] = value; }
		public string XferlogFile { get => this["xferlog_file"] ?? "/var/log/xferlog"; set => this["xferlog_file"] = value; }

	}
}

