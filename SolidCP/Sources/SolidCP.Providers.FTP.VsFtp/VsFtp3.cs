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
using System.Diagnostics;
using Mono.Posix;
using Mono.Unix;

using SolidCP.Providers;
using SolidCP.Providers.OS;
using SolidCP.Providers.FTP;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.FTP
{
	public class VsFtp3 : HostingServiceProviderBase, IFtpServer
	{
		public string ConfigPath => ProviderSettings[nameof(ConfigPath)];

		VsFtpConfig config = null;
		public VsFtpConfig Config => config ?? (config = new VsFtpConfig(ConfigPath));

		public Shell Shell => Shell.Default;

		#region Sites

		public virtual void ChangeSiteState(string siteId, ServerState state)
		{
			// not implemented
		}

		public virtual string CreateSite(FtpSite site)
		{
			// not implemented
			return null;
		}

		public virtual void DeleteSite(string siteId)
		{
			// not implemented
		}

		public virtual FtpSite GetSite(string siteId)
		{
			// not implemented
			return null;
		}

		public virtual FtpSite[] GetSites()
		{
			// not implemented
			return null;
		}

		public virtual bool SiteExists(string siteId)
		{
			// not implemented
			return false;
		}

		public virtual ServerState GetSiteState(string siteId)
		{
			// not implemented
			return ServerState.Started;
		}

		public virtual void UpdateSite(FtpSite site)
		{
			// not implemented
		}

		#endregion

		#region Accounts

		IEnumerable<string> Users {
			get
			{
				if (Config.UserListEnable)
				{
					if (!Config.UserListDeny)
					{
						return Config.UserList;
					}
					else
					{
						var denyUsers = Config.UserList;
						var users = UnixUserInfo.GetLocalUsers().Select(user => user.UserName);
						return users.Except(denyUsers);
					}
				}
				else if (Config.LocalEnable)
				{
					return UnixUserInfo.GetLocalUsers().Select(user => user.UserName);
				}
				return new string[0];

			}
		}
		public virtual bool AccountExists(string accountName) => Users.Any(user => user == accountName);

		public virtual FtpAccount GetAccount(string accountName)
		{
			try
			{
				var user = new UnixUserInfo(accountName);

				var ftp = new FtpAccount()
				{
					CanRead = true,
					CanWrite = true,
					CreatedDate = DateTime.MinValue,
					Enabled = true,
					Folder = user.HomeDirectory,
					Name = user.UserName,
					GroupName = user.GroupName,
					Id = Convert.ToInt32(user.UserId),
					Password = user.Password
				};
				return ftp;
			} catch
			{
				return null;
			}
		}

		public virtual FtpAccount[] GetAccounts() => Users.Select(user => GetAccount(user)).ToArray();

		public virtual void CreateAccount(FtpAccount account)
		{
			bool exists;
			try
			{
				var user = new UnixUserInfo(account.Name);
				exists = true;
			} catch
			{
				exists = false;
			}
			if (exists) throw new ArgumentException($"User {account.Name} already exists.");

			if (!Directory.Exists(account.Folder) && !string.IsNullOrEmpty(account.Folder)) Directory.CreateDirectory(account.Folder);
			// add user and set password
			Shell.Exec($"useradd -s /bin/nologin{(!string.IsNullOrEmpty(account.Folder) ? $" -d {account.Folder}" : "")} {account.Name}");
			Shell.Exec($"echo \"{account.Password}\" | passwd {account.Name} -stdin");

			// add to userlist file if necessary
			if ((!Config.LocalEnable || Config.UserListEnable) && !Config.UserListDeny && account.Enabled)
			{
				if (!Config.UserList.Any(user => user == account.Name))
				{
					File.AppendAllLines(Config.UserListFile, new string[1] { account.Name });
				}
			}
		}

		public virtual void UpdateAccount(FtpAccount account)
		{
			if (!Directory.Exists(account.Folder) && !string.IsNullOrEmpty(account.Folder)) Directory.CreateDirectory(account.Folder);

			Shell.Exec($"usermod -p {account.Folder} {account.Name}");
			Shell.Exec($"echo \"{account.Password}\" | passwd {account.Name} -stdin");

			// add to userlist file if necessary
			if ((!Config.LocalEnable || Config.UserListEnable) && !Config.UserListDeny && account.Enabled)
			{
				AddToUserList(account.Name);
			}
			if ((!Config.LocalEnable || Config.UserListEnable) && Config.UserListDeny && !account.Enabled)
			{
				AddToUserList(account.Name);
			}

		}

		public virtual void DeleteAccount(string accountName)
		{
			if (AccountExists(accountName))
			{
				Shell.Exec($"userdel {accountName}");
				// remove from userlist file if necessary
				if (!Config.LocalEnable || Config.UserListEnable)
				{
					RemoveFromUserList(accountName);
				}
			}
			else throw new ArgumentException($"User {accountName} does not exist.");
		}
		#endregion

		void AddToUserList(string userName)
		{
			if (Config.UserListFile != null && !Config.UserList.Any(user => user == userName))
			{
				File.AppendAllLines(Config.UserListFile, new string[1] { userName });
			}
		}

		void RemoveFromUserList(string userName)
		{
			if (Config.UserListFile != null && Config.UserList.Any(user => user == userName))
			{
				var list = Config.UserList.Where(user => user != userName).ToArray();
				File.WriteAllLines(Config.UserListFile, list);
			}
		}

		public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is FtpAccount)
				{
					try
					{
						// change FTP account state
						FtpAccount account = GetAccount(item.Name);
						account.Enabled = enabled;
						UpdateAccount(account);
					}
					catch (Exception ex)
					{
						Log.WriteError(String.Format("Error switching '{0}' {1}", item.Name, item.GetType().Name), ex);
					}
				}
			}
		}

		public override void DeleteServiceItems(ServiceProviderItem[] items)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is FtpAccount)
				{
					try
					{
						// delete FTP account
						DeleteAccount(item.Name);
					}
					catch (Exception ex)
					{
						Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
					}
				}
			}
		}

		public override bool IsInstalled()
		{
			if (!OSInfo.IsWindows)
			{
				var processes = Process.GetProcessesByName("vsftpd")
					.Select(p => p.MainModule.FileName)
					.Concat(new string[] { Shell.Default.Find("vsftpd") })
					.Where(exe => exe != null)
					.Distinct();
				foreach (var exe in processes)
				{
					if (File.Exists(exe))
					{
						var output = Shell.Default.Exec($"\"{exe}\" -v").Output().Result;
						var match = Regex.Match(output, @"[0-9][0-9.]+");
						if (match.Success)
						{
							try
							{
								var version = new Version(match.Value);
								if (version.Major == 3) return true;
							}
							catch { }
						}
					}
				}
			}
			return false;
		}

	}
}

