using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Text;
using SolidCP.Providers.OS;
using SolidCP.EnterpriseServer.Data;

namespace SolidCP.UniversalInstaller;


public class ConsoleUI : UI
{
	public override bool IsAvailable => true;

	public string NewLine => Environment.NewLine;
	public new class SetupWizard : UI.SetupWizard
	{

		List<Action> Pages = new();

		int CurrentPage = 0;
		public string NewLine => Environment.NewLine;

		protected void Next()
		{
			if (CurrentPage < Pages.Count) CurrentPage++;
		}
		protected void Back()
		{
			if (CurrentPage > 0) CurrentPage--;
		}
		protected void Exit() => CurrentPage = -1;
		protected bool HasExited => CurrentPage < 0 || CurrentPage >= Pages.Count;
		protected Action Current => CurrentPage >= 0 && CurrentPage < Pages.Count ? Pages[CurrentPage] : () => { };

		public override UI.SetupWizard Introduction(ComponentSettings settings)
		{
			Pages.Add(() =>
			{
				var form = new ConsoleForm($@"
Welcome to the SolidCP Setup Wizard
===================================

This wizard will guide you through the installation of the SolidCP product.

It is recommended that you close all other applications before starting Setup. " +
@"This will make it possible to update relevant system files without having " +
@"to reboot your computer.

[  Next  ]  [  Cancel  ]")
					.ShowDialog();
				if (form["Cancel"].Clicked)
				{
					UI.RunMainUI();
					Exit();
				}
				Next();
			});
			return this;
		}

		public override UI.SetupWizard LicenseAgreement()
		{
			Pages.Add(() =>
			{
				var form = new ConsoleForm();
				var license = @"
LICENSE AGREEMENT
=================

Copyright (c) 2016 - 2024, SolidCP. 
All rights Reserved 

This project is distributed under the Creative Commons Share-alike license. 

This project is originally based up on WebsitePanel: 
Copyright (c) 2012, Outercurve Foundation.
All rights reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
* Neither the name of the Outercurve Foundation nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ""AS IS"" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.".Trim();
				license = form.Wrap(license);
				var sr = new StringReader(license);
				List<string> lines = new();
				string line;
				while ((line = sr.ReadLine()) != null) lines.Add(line);

				int height = Console.WindowHeight - 2;
				int pages = lines.Count / height + 1;
				int page = 0;
				bool lastPage = false;
				bool firstPage = true;
				bool exit = false;
				do
				{
					var sb = new StringBuilder();
					for (int i = page * height; i < (page + 1) * height; i++)
					{
						if (i < lines.Count) sb.AppendLine(lines[i]);
					}
					sb.AppendLine();
					lastPage = page >= pages - 1;
					firstPage = page == 0;
					if (firstPage && lastPage) sb.Append("[  I Agree  ]  [  Cancel  ]");
					else if (firstPage) sb.Append("[  Next  ]  [  Cancel  ]");
					else if (lastPage) sb.Append("[  Previous  ]  [  I Agree  ]  [  Cancel  ]");
					else sb.Append("[  Previous  ]  [  Next  ]  [  Cancel  ]");
					form.Parse(sb.ToString());
					form.ShowDialog();
					if (form["Cancel"].Clicked)
					{
						UI.RunMainUI();
						Exit();
					}
					else if (lastPage && form["I Agree"].Clicked)
					{
						exit = true;
						Next();
					}
					else if (!firstPage && form["Previous"].Clicked) page--;
					else if (!lastPage && form["Next"].Clicked) page++;
				} while (!exit);
			});
			return this;
		}
		public override UI.SetupWizard CheckPrerequisites()
		{
			Pages.Add(() =>
			{
				((ConsoleUI)UI).CheckPrerequisites();
				Next();
			});
			return this;
		}
		public override UI.SetupWizard InstallFolder(ComponentSettings settings)
		{
			Pages.Add(() =>
			{
				var form = new ConsoleForm(@"
Install Folder:
===============

Install Component to:
[?InstallFolder                                                          ]

[  Next  ]  [  Back  ]")
				.Load(settings)
				.ShowDialog();
				if (form["Next"].Clicked)
				{
					form.Save(settings);
					Next();
				}
				else Back();
			});
			return this;
		}

		string warning = "";
		private bool CheckSettings(CommonSettings settings, string confirm)
		{
			string name = settings.Username;
			string password = settings.Password;
			var tokens = name.Split('\\');
			string domain = "";
			if (tokens.Length == 2) domain = tokens[0];

			if (settings.UseActiveDirectory)
			{
				if (domain.Trim().Length == 0)
				{
					warning = "Please enter domain name!";
					return false;
				}

				string users = Installer.Current.SecurityUtils.GetDomainUsersContainer(domain);
				if (string.IsNullOrEmpty(users))
				{
					warning = "Domain not found or access denied!";
					return false;
				}

				if (!Installer.Current.SecurityUtils.ADObjectExists(users))
				{
					warning = "Domain not found or access denied!";
					return false;
				}
			}

			if (name.Trim().Length == 0)
			{
				warning = "Please enter user name!";
				return false;
			}

			if (password.Trim().Length == 0)
			{
				warning = "Please enter password!";
				return false;
			}

			if (password != confirm)
			{
				warning = "The password was not correctly confirmed. Please ensure that the password and confirmation match exactly.";
				return false;
			}

			return true;
		}

		private bool ProcessSettings(CommonSettings settings, string confirm)
		{
			if (!CheckSettings(settings, confirm))
			{
				return false;
			}

			if (!CheckUserAccount(settings))
			{
				return false;
			}
			return true;
		}
		private bool CheckUserAccount(CommonSettings settings)
		{
			string userName = settings.Username;
			string password = settings.Password;
			var tokens = userName.Split('\\');
			string domain = "";
			if (tokens.Length == 2) domain = tokens[0];
			domain = settings.UseActiveDirectory ? domain : null;

			if (Installer.Current.SecurityUtils.UserExists(domain, userName))
			{
				warning = $"{userName} user account already exists!";
				return false;
			}

			bool created = false;
			try
			{
				// create account
				Log.WriteStart($"Creating temp user account \"{userName}\"");
				SystemUserItem user = new SystemUserItem();
				user.Name = userName;
				user.FullName = userName;
				user.Description = string.Empty;
				user.MemberOf = null;
				user.Password = password;
				user.PasswordCantChange = true;
				user.PasswordNeverExpires = true;
				user.AccountDisabled = false;
				user.System = true;
				user.Domain = domain;
				Installer.Current.SecurityUtils.CreateUser(user);
				//update log
				Log.WriteEnd("Created temp local user account");
				created = true;
			}
			catch (Exception ex)
			{
				System.Runtime.InteropServices.COMException e = ex.InnerException as System.Runtime.InteropServices.COMException;
				Log.WriteError("Create temp local user account error", ex);
				string errorMessage = "Unable to create Windows user account";
				if (e != null)
				{
					string errorCode = string.Format("{0:x}", e.ErrorCode);
					switch (errorCode)
					{
						case "8007089a":
							errorMessage = "Invalid username!";
							break;
						case "800708c5":
							errorMessage = "The password does not meet the password policy requirements. Check the minimum password length, password complexity and password history requirements!";
							break;
						case "800708b0":
							errorMessage = "The account already exists!";
							break;
					}
				}
				warning = errorMessage;
				return false;
			}

			if (created)
			{
				Log.WriteStart(string.Format("Deleting temp local user account \"{0}\"", userName));
				try
				{
					Installer.Current.SecurityUtils.DeleteUser(domain, userName);
				}
				catch (Exception ex)
				{
					Log.WriteError("Delete temp local user account error", ex);
				}
				Log.WriteEnd("Deleted temp local user account");
			}
			return true;
		}
		public override UI.SetupWizard UserAccount(CommonSettings settings)
		{
			if (OSInfo.IsWindows || !(settings is ServerSettings))
			{
				Pages.Add(() =>
				{
					bool passwordMatch = true, settingsValid = false;
					ConsoleForm form;
					do
					{
						//creating user account
						string userName = settings.ComponentName.Replace(" ", string.Empty);
						userName = userName.Replace("HostPanelPro", "HPP");

						var domain = "mydomain.com";

						if (Environment.UserDomainName != Environment.MachineName)
						{

							string domainName = Installer.Current.SecurityUtils.GetFullDomainName(Environment.UserDomainName);
							if (!string.IsNullOrEmpty(domainName))
							{
								settings.Username = $"{domainName}\\{userName}";
								settings.UseActiveDirectory = true;
							}
						}
						else
						{
							settings.UseActiveDirectory = false;
						}

						form = new ConsoleForm(@$"
Security Settings:
==================

Please specify a new user account for the website anonymous access and application pool identity.
{(OSInfo.IsWindows ? @"
[x] Create Active Directory account" : "")}

Username:        [?Username                                     ]
Password:        [!Password                                     ]
Repeat Password: [!RepeatPassword                                     ]
{(!passwordMatch ? @"
Passwords must match!
" : !settingsValid ? @$"
{warning}
" : "")}
[  Next  ]  [  Back  ]
")
							.Load(settings)
							.Apply(f =>
							{
								if (OSInfo.IsWindows)
								{
									f[0].Checked = settings.UseActiveDirectory;
								}
							})
							.ShowDialog();
						if (form["Back"].Clicked)
						{
							Back();
							break;
						}
						else
						{
							passwordMatch = form["Password"].Text == form["RepeatPassword"].Text;
							if (passwordMatch)
							{
								form.Save(settings);
								settings.UseActiveDirectory = OSInfo.IsWindows && form[0].Checked;

								settingsValid = ProcessSettings(settings, form["RepeatPassword"].Text);

								if (settingsValid) Next();
							}
						}
					} while (!passwordMatch || !settingsValid);
				});
			} else
			{
				settings.Username = settings.Password = null;
			}
				return this;
		}
		public override UI.SetupWizard Web(CommonSettings settings)
		{
			Pages.Add(() =>
			{
				var form = new ConsoleForm(@"
Web Site Settings:
==================

Specify the desired url of the website. You can specify multiple urls separated by a semicolon.

Urls: [?Urls                                                                       ]

[  Next  ]  [  Back  ]
")
					.Load(settings)
					.ShowDialog();
				settings.WebSiteDomain = null;
				settings.WebSiteIp = null;
				settings.WebSitePort = default;
				if (form["Back"].Clicked)
				{
					Back();
				} else
				{
					form.Save(settings);
					Next();
				}
			});
			return this;
		}
		public override UI.SetupWizard EnterpriseServerUrl()
		{
			var settings = Settings.WebPortal;

			Pages.Add(() =>
			{
				var form = new ConsoleForm(@"
EnterpriseServer Settings:
==========================

[x] Embed EnterpriseServer into WebPortal

EnterpriseServer URL: [?EnterpriseServerUrl                                          ]

[x] Expose EnterpriseServer Webservices

Path to EnterpriseServer: [?EnterpriseServerPath                                      ]

[  Next  ]  [  Back  ]
")
					.Load(settings)
					.Apply(f =>
					{
						f[0].Checked = settings.EmbedEnterpriseServer;
						f[2].Checked = settings.ExposeEnterpriseServerWebServices;
					})
					.ShowDialog();
				if (form["Back"].Clicked)
				{
					Back();
				} else
				{
					form.Save(settings);
					settings.EmbedEnterpriseServer = form[0].Checked;
					settings.ExposeEnterpriseServerWebServices = form[0].Checked;
					Next();
				}
			});
			return this;
		}
		public override UI.SetupWizard EmbedEnterpriseServer()
		{
			var settings = Settings.WebPortal;

			Pages.Add(() =>
			{
				var form = new ConsoleForm(@"
EnterpriseServer Settings:
==========================

[x] Embed EnterpriseServer into WebPortal

[x] Expose EnterpriseServer Webservices

[  Next  ]  [  Back  ]
")
					.Apply(f =>
					{
						f[0].Checked = settings.EmbedEnterpriseServer;
						f[2].Checked = settings.ExposeEnterpriseServerWebServices;
					})
					.ShowDialog();
				if (form["Back"].Clicked)
				{
					Back();
				}
				else
				{
					settings.EmbedEnterpriseServer = form[0].Checked;
					settings.ExposeEnterpriseServerWebServices = form[0].Checked;
					Next();
				}
			});
			return this;
		}

		bool IsIis7 => OSInfo.IsWindows && OSInfo.Windows?.WebServer.Version.Major >= 7;
		bool IsSecure(Uri uri) => uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) || (IsIis7 || !OSInfo.IsWindows) && Utils.IsHostLocal(uri.Host);
		bool IsSecure(string urls) => (urls ?? "").Split(',', ';')
			.Any(url => IsSecure(new Uri(url)));
		public override UI.SetupWizard InsecureHttpWarning(CommonSettings settings)
		{
			Pages.Add(() =>
			{
				if (!IsSecure(settings.Urls))
				{
					var form = new ConsoleForm(@"
Insecure HTTP Warning:
======================

You've choosen an insecure http protocol for the server. That way you will only be able to access the server from localhost, a LAN IP, via an SSH Tunnel or VPN. You can access the server via SSH Tunnel by specifying the url 
ssh://<username>:<password>@<host>/<remoteport>
when adding the server in SolidCP Portal.

[  Next  ]  [  Back  ]")
						.ShowDialog();
					if (form["Back"].Clicked) Back();
					else Next();
				}
				else Next();
			});
			return this;
		}

		public override UI.SetupWizard Database()
		{
			var settings = Settings.EnterpriseServer;
			Pages.Add(() =>
			{
				var dbType = settings.DatabaseType;
				ConsoleForm form = null;

				if (dbType == DbType.Unknown)
				{
					form = new ConsoleForm(@"
Database Settings:
==================

[  Use Microsoft SQL-Server Database  ]
[  Use MySQL/MariaDB Database  ]
[  Use SQLite Database  ]

[  Back  ]
")
					.ShowDialog();
					if (form["Back"].Clicked)
					{
						Back();
						return;
					}
					else
					{
						if (form[0].Clicked) dbType = DbType.SqlServer;
						else if (form[1].Clicked) dbType = DbType.MySql;
						else if (form[2].Clicked) dbType = DbType.Sqlite;
					}
				}

				settings.DatabaseType = dbType;

				switch (dbType)
				{
					default:
					case DbType.SqlServer:
						form = new ConsoleForm(@"
SQL Server Settings:
====================

Server:   [?DatabaseServer                               ]
Database: [?DatabaseName                               ]
          [x DatabaseWindowsAuthentication] Windows Authentication
User:     [?DatabaseUser                               ]
Password: [?DatabasePassword                               ]
Always Trust Server Certificate: [x DatabaseTrustServerCertificate]

[  Next  ]  [  Back  ]")
							.Load(settings)
							.ShowDialog();
						if (form["Next"].Clicked)
						{
							form.Save(settings);
							Next();
						}
						else
						{
							settings.DatabaseType = DbType.Unknown;
							Back();
						}
						break;
					case DbType.MySql:
					case DbType.MariaDb:
						form = new ConsoleForm(@"
MySQL/MariaDB Settings:
=======================

(Note that by using MySQL/MariaDB, SolidCP will run slower than with SQL Server)

Server:   [?DatabaseServer                               ]
Database: [?DatabaseName                               ]
Port:     [?DatabasePort                               ]
User:     [?DatabaseUser                               ]
Password: [?DatabasePassword                               ]

[  Next  ]  [  Back  ]")
							.Load(settings)
							.ShowDialog();
						if (form["Next"].Clicked)
						{
							form.Save(settings);
							Next();
						}
						else
						{
							settings.DatabaseType = DbType.Unknown;
							Back();
						}
						break;
					case DbType.Sqlite:
					case DbType.SqliteFX:
						form = new ConsoleForm(@"
SQLite Settings:
================

(Note that by using SQLite, SolidCP will run slower than with using SQL Server. It is not recommended to use SQLite on a production server.)

Database: [?DatabaseName                               ]

[  Next  ]  [  Back  ]")
							.Load(settings)
							.ShowDialog();
						if (form["Next"].Clicked)
						{
							form.Save(settings);
							Next();
						}
						else
						{
							settings.DatabaseType = DbType.Unknown;
							Back();
						}
						break;
				}
			});
			return this;
		}

		public override UI.SetupWizard ServerPassword()
		{
			Pages.Add(() =>
			{
				bool passwordMatch = true;
				do
				{
					var form = new ConsoleForm(@$"
Server Password:
================

Choose a password to secure access to the Server.

Password:        [!ServerPassword                             ]
Repeat Password: [!RepeatPassword                             ]
{(passwordMatch ? "" : @"
Passwords must match!
")}
[  Next  ]  [  Back  ]")
					.Load(Settings.Server)
					.ShowDialog();
					passwordMatch = form["ServerPassword"].Text == form["RepeatPassword"].Text;
					if (form["Next"].Clicked && passwordMatch)
					{
						form.Save(Settings.Server);
						Next();
					}
					else if (form["Back"].Clicked) Back();
				} while (!passwordMatch);

			});
			return this;
		}
		public override UI.SetupWizard ServerAdminPassword()
		{
			Pages.Add(() =>
			{
				bool passwordMatch = true;
				do
				{
					var form = new ConsoleForm(@$"
ServerAdmin Password:
================

Choose a password for the serveradmin user to log into WebPortal.

Password:        [!ServerAdminPassword                             ]
Repeat Password: [!RepeatPassword                             ]
{(passwordMatch ? "" : @"
Passwords must match!
")}
[  Next  ]  [  Back  ]")
					.Load(Settings.Server)
					.ShowDialog();
					passwordMatch = form["ServerAdminPassword"].Text == form["RepeatPassword"].Text;
					if (form["Next"].Clicked && passwordMatch)
					{
						form.Save(Settings.Server);
						Next();
					}
					else if (form["Back"].Clicked) Back();
				} while (!passwordMatch);

			});
			return this;
		}

		public override UI.SetupWizard Certificate(CommonSettings settings)
		{
			Pages.Add(() =>
			{
				var form = new ConsoleForm(@"
Certificate Settings:
=====================

[  Use a certificate from the store  ]
" + (!OSInfo.IsWindows ?
@"[   Use a certificate from a file    ]
[  Use a Let's Encrypt certificate   ]
" : "") + 
@"[ Configure the certificate manually ]

[  Back  ]")
				.ShowDialog();
				if (form["Back"].Clicked)
				{
					Back();
					if (IsSecure(settings.Urls)) Back(); // Go also back in InsecureHttpWarnings
				}
				else if (form[0].Clicked)
				{
					form = new ConsoleForm(@"
Certificate from Store:
=======================

Store Location:  [?CertificateStoreLocation                                     ]
Store Name:      [?CertificateStoreName                                     ]
Find Type:       [?CertificateFindType                                     ]
Find Value:      [?CertificateFindValue                                     ]

[  Next  ]  [  Back  ]")
						.Load(settings)
						.ShowDialog();
					if (form["Next"].Clicked)
					{
						form.Save(settings);
						Next();
					}
					else Back();
				} else if (form[1].Clicked && !OSInfo.IsWindows)
				{
					form = new ConsoleForm(@"
Certificate from File:
======================

File:     [?CertificateFile                                     ]
Password: [?CertificatePassword                                     ]

[  Next  ]  [  Back  ]")
						.Load(settings)
						.ShowDialog();
					if (form["Next"].Clicked)
					{
						form.Save(settings);
						Next();
					}
					else Back();
				} else if (form[2].Clicked && !OSInfo.IsWindows)
				{
					form = new ConsoleForm(@"
Let's Encrypt Certificate:
==========================

Email:   [?LetsEncryptCertificateEmail                                     ]
Domains: [?LetsEncryptCertificateDomains                                     ]

[  Next  ]  [  Back  ]")
.Load(settings)
.ShowDialog();
					if (form["Next"].Clicked)
					{
						form.Save(settings);
						Next();
					}
					else Back();
				} else
				{
					form = new ConsoleForm(@"
Configure Certificate Manually:
===============================

[x] Configure certificate manually

[  Next  ]  [  Back  ]")
						.Apply(f => f[0].Checked = settings.ConfigureCertificateManually)
						.ShowDialog();
					if (form["Next"].Clicked)
					{
						settings.ConfigureCertificateManually = form[0].Checked;
						Next();
					}
					else Back();
				}
			});
			return this;
		}
		public override UI.SetupWizard Finish()
		{
			Pages.Add(() =>
			{
				if (!Installer.Current.HasError)
				{
					var form = new ConsoleForm($@"
Successful Installation
=======================

SolidCP Installer successfully has:
{string.Join(NewLine, Installer.Current.InstallLogs
	.Select(line => $"- {line}"))}

[  Finish  ]")
					.ShowDialog();
				} else
				{
					var form = new ConsoleForm($@"
Installation Failed
===================

Installation of {Settings.Installer.Component.Component} failed. Any changes have been rolled back.
Exception Message: {Installer.Error.SourceException.Message}

[  Finish  ]")
					.ShowDialog();
				}
				Installer.Current.UpdateSettings();
				Installer.Current.Cleanup();

				UI.RunMainUI();
				UI.Exit();
			});
			return this;
		}

		private void SetProgressValue(int value)
		{
			float fvalue = 0;
			if (value > 0)
			{
				if (value < 100) fvalue = (ProgressMaximum * (float)value / (5f * 100f));
				else fvalue = (ProgressMaximum / 5f) + (float)(((float)ProgressMaximum - (float)ProgressMaximum / 5f) * (1 - Math.Exp(-2 * (double)(value - 100) / Installer.Current.EstimatedOutputLines)));
			}
			var ui = UI as ConsoleUI;
			fvalue = fvalue / ProgressMaximum;
			if (ui.InstallationProgress.Progress.Value != fvalue)
			{
				ui.InstallationProgress.Progress.Value = fvalue;
			}
		}

		private void SetProgressText(string text) {
			var ui = UI as ConsoleUI;
			ui.ShowInstallationProgress(null, text);
		}

		public override UI.SetupWizard RunWithProgress(string title, Action action, ComponentSettings settings)
		{
			Pages.Add(() =>
			{
				int n = 0;
				((ConsoleUI)UI).ShowInstallationProgress(title, null, ProgressMaximum);

				var reportProgress = () => SetProgressValue(n++);
				Installer.Current.Log.OnWrite += reportProgress;
				Installer.Current.OnInfo += SetProgressText;
				Installer.Current.OnError += UI.ShowError;

				Installer.Current.WaitForDownloadToComplete();

				action();

				Installer.Current.Log.OnWrite -= reportProgress;
				Installer.Current.OnInfo -= SetProgressText;
				Installer.Current.OnError -= UI.ShowError;

				Next();
			});
			return this;
		}
		public override UI.SetupWizard ConfirmUninstall(ComponentSettings settings)
		{
			Pages.Add(() =>
			{
				if (settings == null) settings = Settings.Standalone;
				var form = new ConsoleForm(@$"
Uninstall {settings.ComponentName}:
=========={new string('=', settings.ComponentName.Length)}=

If you proceed, the installer will completely uninstall {settings.ComponentName} from this computer.

[*  Cancel  ]  [  Uninstall  ]")
				.ShowDialog();
				if (form["Cancel"].Clicked)
				{
					UI.RunMainUI();
				}
				else Next();
			});
			return this;
		}
		public SetupWizard(UI ui) : base(ui) { }
		public override bool Show()
		{
			try
			{
				UI.EndWaitCursor();

				while (!HasExited) Current();

				Installer.Current.UpdateSettings();

				return true;
			} catch (Exception ex)
			{
				return false;
			}
		}
	}

	public override UI.SetupWizard Wizard => new SetupWizard(this);

	public override void RunMainUI()
	{
		var form = new ConsoleForm($@"
SolidCP Installer
=================

[ Application Settings ]
[ View Available Components ]
[ View Installed Components ]
[ Exit ]")
			.ShowDialog();
		if (form["Application Settings"].Clicked) ApplicationSettings();
		else if (form["View Available Components"].Clicked) AvailableComponents();
		else if (form["View Installed Components"].Clicked) InstalledComponents();
		else Installer.Current.Exit();
	}

	public void ApplicationSettings()
	{
		var form = new ConsoleForm($@"
Application Settings
====================

[x] Automatically check for Updates
[ Check for Updates ]

[x] Use Proxy
Address:  [?ProxyAddress                                             ]
Username: [?ProxyUsername                           ]
Password: [!ProxyPassword                           ]

[  Save  ]  [  Cancel  ]  [  View Log  ]")
			.Apply(f =>
			{
				f[0].Checked = Settings.Installer.CheckForUpdate;
				if (Settings.Installer.Proxy != null)
				{
					f[2].Checked = true;
					f["ProxyAddress"].Text = Settings.Installer.Proxy.Address;
					f["ProxyUsername"].Text = Settings.Installer.Proxy.Username;
					f["ProxyPassword"].Text = Settings.Installer.Proxy.Password;
				} else
				{
					f[2].Checked = false;
					f["ProxyAddress"].Text = f["ProxyUsername"].Text = f["ProxyPassword"].Text = "";
				}
			})
			.ShowDialog();

		if (form["Save"].Clicked)
		{
			Settings.Installer.CheckForUpdate = form[0].Checked;
			if (form[2].Checked)
			{
				var proxy = Settings.Installer.Proxy = new ProxySettings();
				proxy.Address = form["ProxyAddress"].Text;
				proxy.Username = form["ProxyUsername"].Text;
				proxy.Password = form["ProxyPassword"].Text;
			}
			else Settings.Installer.Proxy = null;
			Installer.Current.SaveSettings();
		}
		else if (form["View Log"].Clicked)
		{
			ShowLogFile();
		}
		else if (form["Check for Updates"].Clicked)
		{
			CheckForInstallerUpdate();
		}
		
		RunMainUI();
	}

	public void AvailableComponents()
	{
		var str = new StringBuilder();
		str.AppendLine("Available Components");
		str.AppendLine("====================");
		str.AppendLine();
		//load components via web service
		var releases = Installer.Current.Releases;
		
		ShowWaitCursor();
		var components = releases.GetAvailableComponents();
		EndWaitCursor();

		//remove already installed components or components not available on this platform
		foreach (var component in components.ToArray())
		{
			if (component.IsInstalled || !component.IsAvailableOnPlatform) components.Remove(component);
			else
			{
				str.AppendLine($"[  {component.ComponentName}, {component.Version}  ]");
			}
		}
		str.AppendLine();
		str.AppendLine("[  Back  ]");
		var form = new ConsoleForm(str.ToString())
			.ShowDialog();
		if (form["Back"].Clicked) RunMainUI();
		else
		{
			for (int i = 0; i < components.Count; i++)
			{
				if (form[i].Clicked)
				{
					ShowComponentDetails(components[i]);
					break;
				}
			}
		}
	}

	public void ShowComponentDetails(ComponentInfo component)
	{
		var str = new StringBuilder();
		var title = $"{component.ComponentName}, {component.Version}";
		str.AppendLine(title);
		str.AppendLine(new string('=', title.Length));
		str.AppendLine();
		str.AppendLine(component.ComponentDescription);
		str.AppendLine();
		str.AppendLine("[  Install  ]  [  Back  ]");
		var form = new ConsoleForm(str.ToString())
			.ShowDialog();
		if (form["Back"].Clicked) AvailableComponents();
		else
		{
			Installer.Current.Install(component);
		}
	}
	public void InstalledComponents()
	{
		var str = new StringBuilder();
		str.AppendLine("Installed Components");
		str.AppendLine("====================");
		str.AppendLine();
		//load components via web service.
		var components = Settings.Installer.InstalledComponents;

		//remove already installed components or components not available on this platform
		foreach (var component in components)
		{
			str.AppendLine($"[  {component.ComponentName}, {component.Version}  ]");
		}
		str.AppendLine();
		str.AppendLine("[  Back  ]");
		var form = new ConsoleForm(str.ToString())
			.ShowDialog();
		if (form["Back"].Clicked) RunMainUI();
		else
		{
			for (int i = 0; i < components.Count; i++)
			{
				if (form[i].Clicked)
				{
					ShowInstalledComponentDetails(components[i]);
					break;
				}
			}
		}
	}

	public void ShowInstalledComponentDetails(ComponentInfo component)
	{
		var str = new StringBuilder();
		var title = $"{component.ComponentName}, {component.Version}";
		str.AppendLine(title);
		str.AppendLine(new string('=', title.Length));
		str.AppendLine();
		str.AppendLine(component.ComponentDescription);
		str.AppendLine();
		str.AppendLine("[  Back  ]  [  Check for Updates  ]  [  Uninstall  ]  [  Settings  ]");
		var form = new ConsoleForm(str.ToString())
			.ShowDialog();
		if (form["Back"].Clicked) InstalledComponents();
		else if (form["Check for Updates"].Clicked)
		{
			CheckForUpdate(component);
		} else if (form["Uninstall"].Clicked)
		{
			Installer.Current.Uninstall(component);
		} else if (form["Settings"].Clicked)
		{
			Installer.Current.Setup(component);
		}
	}


	public void CheckForUpdate(ComponentInfo component)
	{
		var release = Installer.Current.Releases;
		var update = release.GetComponentUpdate(component.ComponentCode, component.VersionName);
		if (update == null)
		{
			var title = $"{component.ComponentName}, {component.Version}";
			var form = new ConsoleForm($@"{title}
{new string('=', title.Length)}

Component is already the newest version, there are no updates available;

[  Back  ]
")
				.ShowDialog();
			ShowInstalledComponentDetails(component);
		}
		else
		{
			var title = $"{component.ComponentName}, {update.Version}";
			var form = new ConsoleForm($@"{title}
{new string('=', title.Length)}

There is a new version {component.ComponentName}, {update.VersionName} available that can be installed.

[  Install  ]  [  Back  ]
")
				.ShowDialog();
			if (form["Back"].Clicked) ShowInstalledComponentDetails(component);
			else Installer.Current.Update(new ComponentInfo(component, update));
		}
	}

	public override bool CheckForInstallerUpdate(bool appStartup = false)
	{
		if (appStartup &&
			(!Settings.Installer.CheckForUpdate ||
			Environment.GetCommandLineArgs().Any(arg => arg.Equals("nockeck", StringComparison.OrdinalIgnoreCase))))
			return false;

		var release = Installer.Current.Releases;
		var version = Settings.Installer.Version;
		ComponentUpdateInfo update;
		var hasUpdate = Installer.Current.CheckForInstallerUpdate(out update) && update != null;
		if (!hasUpdate)
		{
			if (!appStartup)
			{
				var title = $"SolidCP Installer, {version}";
				var form = new ConsoleForm($@"{title}
{new string('=', title.Length)}

Component is already the newest version, there are no updates available;

[  Back  ]
")
					.ShowDialog();
				ApplicationSettings();
			}
		}
		else
		{
			var title = $"SolidCP Installer, {update.Version}";
			var form = new ConsoleForm($@"{title}
{new string('=', title.Length)}

There is a new version SolidCP Installer, {update.Version}, available that can be installed.

[  Install  ]  [  Back  ]
")
				.ShowDialog();
			if (form["Back"].Clicked)
			{
				if (!appStartup) ApplicationSettings();
			}
			else
			{
				try
				{
					Installer.Current.DownloadInstallerUpdate(update);
					Installer.Current.Exit();
				}
				catch (Exception ex)
				{
					form = new ConsoleForm($@"There was an error updating the installer: {ex.Message}

[  Ok  ]");
					form.ShowDialog();
				}
			}
		}
		return hasUpdate;
	}

	public override string GetRootPassword()
	{
		var rootUser = OSInfo.IsWindows ? "administrator" : "root";
		var form = new ConsoleForm(@$"
SolidCP Installer
=================

SolidCP Installer must run as {rootUser}.
Please restart SolidCP Installer as {rootUser}.

[    Exit    ]
")
			.ShowDialog();
		//return form["Password"].Text;
		Exit();
		return null;
	}

	public ServerSettings GetServerSettings()
	{
		var form = new ConsoleForm(@"
Server Settings:
================

Server Urls: [?Urls http://localhost:9003                                                   ]

Server Password: [!ServerPassword                                         ]
Repeat Password: [!RepeatPassword                                         ]

[    Ok    ]
")
			.Load(Settings.Server)
			.Apply(f => f["RepeatPassword"].Text = f["ServerPassword"].Text)
			.ShowDialog()
			.Save(Settings.Server);

		while (form["ServerPassword"].Text != form["RepeatPassword"].Text)
		{
			form.Parse(@"
Server Settings:
================

Server Urls: [?Urls http://localhost:9003                                                   ]

Server Password: [!ServerPassword                                         ]
Repeat Password: [!RepeatPassword                                         ]

Passwords don't match!

[    Ok    ]
")
				.Load(Settings.Server)
				.Apply(f => f["RepeatPassword"].Text = f["ServerPassword"].Text)
				.ShowDialog()
				.Save(Settings.Server);
		}

		if (!Settings.Server.Urls.Split(',', ';').Select(url => url.Trim()).Any(url => url.StartsWith("https://") || url.StartsWith("net.tcp://")))
		{
			form = new ConsoleForm(@"
You did not specify a secure protocol as url.
That way, you will only be able to access
the server via localhost or a LAN ip.
Do you want to proceed?

[    Back    ]  [*    Continue    ]")
				.ShowDialog();
			if (form["Back"].Clicked) GetServerSettings();
		}

		GetCommonSettings(Settings.Server);

		return Settings.Server;
	}
	public EnterpriseServerSettings GetEnterpriseServerSettings()
	{
		var form = new ConsoleForm(@"
Enterprise Server Settings
==========================

Urls:              [?Urls http://localhost:9002                                               ]
Database Server:   [?DatabaseServer localhost                                                           ]
Database User:     [?DatabaseUser sa                                                                  ]
Database Password: [!DatabasePassword                                                                     ]

[    Ok    ]
")
			.Load(Settings.EnterpriseServer)
			.ShowDialog()
			.Save(Settings.EnterpriseServer);

		GetCommonSettings(Settings.EnterpriseServer);
		return Settings.EnterpriseServer;
	}
	bool IsLocalHttp(string url)
	{
		var uri = new Uri(url.Trim());
		var host = uri.Host;

		return uri.Scheme == "http" && (host == "localhost" || host == "127.0.0.1" || host == "::1" || Regex.IsMatch(host, @"^192\.168\.[0-9]{1,3}\.[0-9]{1,3}$"));
	}
	bool IsLocalHttps(string url)
	{
		var uri = new Uri(url.Trim());
		var host = uri.Host;

		return uri.Scheme == "https" && (host == "localhost" || host == "127.0.0.1" || host == "::1" || Regex.IsMatch(host, @"^192\.168\.[0-9]{1,3}\.[0-9]{1,3}$"));
	}
	public WebPortalSettings GetWebPortalSettings()
	{
		var esurls = Settings.EnterpriseServer.Urls;
		if (!string.IsNullOrEmpty(esurls))
		{
			var urls = esurls!.Split(';', ',').Select(url => url.Trim());
			var esurl = urls.FirstOrDefault(url => IsLocalHttp(url));
			if (esurl == null) esurl = urls.FirstOrDefault(url => IsLocalHttps(url));
			if (esurl != null)
			{
				Settings.WebPortal.EnterpriseServerUrl = esurl;
			}
		}

		var form = new ConsoleForm(@"
Web Portal Settings
===================

Enterprise Server Url: [?EnterpriseServerUrl http://localhost:9002                                        ]

[    Ok    ]
")
			.Load(Settings.WebPortal)
			.ShowDialog()
			.Save(Settings.WebPortal);

		return Settings.WebPortal;
	}

	public override void ShowError(Exception ex)
	{
		Console.Clear();
		Console.WriteLine("Error:");
		Console.WriteLine(ex.ToString());
	}

	const int ProgressMaximum = 1000;
	public ConsoleForm InstallationProgress = null;
	public string ProgressTitle = null;
	public void ShowInstallationProgress(string title = null, string task = null, int maxProgress = ProgressMaximum)
	{
		float progress = InstallationProgress?.Progress.Value ?? 0;
		title ??= (ProgressTitle ??= "Installation Progress");
		var form = InstallationProgress = new ConsoleForm(@$"
{title}
{new string('=', title.Length)}

{task ?? ""}

[%Progress                                                                      ]
");
		form.Progress.Value = progress;
		form.Show();
	}

	public void CloseInstallationProgress()
	{
		if (InstallationProgress != null) InstallationProgress.Close();
	}

	public void GetCommonSettings(CommonSettings settings)
	{
		if (settings.Urls!.Split(';', ',').Any(url =>
			url.StartsWith("https:", StringComparison.OrdinalIgnoreCase) ||
			url.StartsWith("net.tcp:", StringComparison.OrdinalIgnoreCase)))
		{
			var template = OSInfo.IsWindows ? @"
Server Certificate Settings
===========================

Your server uses a secure protocol, for which it needs to provide a certificate.

Choose how to provide a certificate:

[    From the Certificate Store    ]
[    From a pfx file               ]
[    Use Let's Encrypt             ]" :

@"
Server Certificate Settings
===========================

Your server uses a secure protocol, for which it needs to provide a certificate.

Choose how to provide a certificate:

[    From the Certificate Store    ]
[    From a pfx file               ]
[    Use Let's Encrypt             ]";

			var form = new ConsoleForm(template).ShowDialog();
			if (form[0].Clicked)
			{
				bool repeat = false;
				do
				{
					var certStore = new ConsoleForm(@"
Server Certificate From Certificate Store
=========================================

Store Name:     [?CertificateStoreName My                                     ]
Store Location: [?CertificateStoreLocation LocalMachine                           ]
Find Value:     [?CertificateFindValue localhost                              ]
Find Type:      [?CertificateFindType Subject                                ]

[    Ok    ]
")
					.Load(settings)
					.ShowDialog()
					.Save(settings);

					StoreName storeName;
					StoreLocation storeLocation;
					X509FindType findType;

					repeat = !Enum.TryParse<StoreName>(settings.CertificateStoreName, out storeName);
					repeat |= !Enum.TryParse<StoreLocation>(settings.CertificateStoreLocation, out storeLocation);
					repeat |= !Enum.TryParse<X509FindType>(settings.CertificateFindType, out findType);

					if (!repeat)
					{

						var certificateStore = new X509Store(storeName, storeLocation);
						certificateStore.Open(OpenFlags.ReadOnly);
						var certificate = certificateStore.Certificates.Find(findType, settings.CertificateFindValue, true);
						repeat |= certificate == null;
					}
					if (repeat)
					{
						var goBack = new ConsoleForm(@"
The specified certificate was not found.

[*    Back    ]  [    Continue    ]
")
							.ShowDialog();
						repeat = goBack["Back"].Clicked;
					}
				} while (repeat);
			}
			else if (form[1].Clicked)
			{
				bool goBack = false;
				do
				{
					var certFile = new ConsoleForm(@"
Server Certificate From pfx File
================================

File Name:    [?CertificateFile ~/localhost.pfx                                              ]
Password:     [?CertificatePassword                                                              ]

[    Ok    ]
")
					.Load(settings)
					.ShowDialog()
					.Save(settings);
					settings.CertificateFile = new DirectoryInfo(settings.CertificateFile!.Replace("~", Environment.GetEnvironmentVariable("HOME"))).FullName;

					if (!File.Exists(settings.CertificateFile))
					{
						var fileNotFoundForm = new ConsoleForm(@$"
The specified file {settings.CertificateFile} was not found.

[*    Back    ]  [     Continue    ]
")
							.ShowDialog();
						goBack = fileNotFoundForm["Back"].Clicked;
					}
				} while (goBack);
			}
			else if (form[2].Clicked)
			{

				if (string.IsNullOrEmpty(settings.LetsEncryptCertificateDomains))
				{
					var hosts = string.Join(",", settings.Urls.Split(',', ';')
						.Select(url => new Uri(url.Trim()))
						.Where(url => url.Scheme == "https" || url.Scheme == "net.tcp")
						.Select(url => url.Host)
						.ToArray());
					settings.LetsEncryptCertificateDomains = hosts;
				}
				var leCert = new ConsoleForm(@"
Server Certificate From Let's Encrypt
=====================================

Domains:   [?LetsEncryptCertificateDomains                                                                ]
Email:     [?LetsEncryptCertificateEmail                                                                ]

[    Ok    ]
")
				.Load(settings)
				.ShowDialog()
				.Save(settings);
			}
			else throw new NotSupportedException("Internal error");
		}
	}
	public override void Init()
	{
		AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
		{
			//Console.Clear();
			Console.CursorVisible = true;
		};
	}
	bool exitCalled = false;
	public override void Exit()
	{
		if (!exitCalled)
		{
			exitCalled = true;
			//if (Installer.Error == null) Console.Clear();
			Console.CursorVisible = true;
			Installer.Exit();
		}
	}

	public void CheckPrerequisites()
	{
		bool ok = true;
		if (OSInfo.IsWindows)
		{
			var form = new ConsoleForm(@"
Checking System Requirements
============================

[x] Operating System:  [?OS                                  ]
[x] NET Framework:     [?NET                                  ]
[x] IIS:               [?IIS                                  ]

[    OK    ]
");
			form.SetFocus(form["OK"]);
			form.Show();
			if (Installer.CheckOSSupported()) form[0].Text = "x";
			else
			{
				form[0].Text = "!";
				ok = false;
			}
			form["OS"].Text = " "+Regex.Replace(OSInfo.Description, @"(?<=[0-9]+)\.[0-9.]*", "");
			form.Show();
			if (Installer.CheckNetVersionSupported()) form[2].Text = "x";
			else {
				form[2].Text = "!";
				ok = false;
			}
			form["NET"].Text = " "+OSInfo.NetDescription;
			form.Show();
			if (Installer.CheckIISVersionSupported()) form[4].Text = "x";
			else {
				form[4].Text = "!";
				ok = false;
			}
			form["IIS"].Text = $" IIS {OSInfo.Current.WebServer.Version}";
			form.Show();
		} else
		{
			var form = new ConsoleForm(@"
Checking System Requirements
============================

  [x] Operating System: [?OS                                ]
  [x] .NET 8 Runtime:   [?NET                                ]
  [x] Init System:      [?SysD                                ]

  [    OK    ]
");
			form["OK"].HasFocus = true;
			form.Show();
			if (Installer.CheckOSSupported()) form[0].Text = "x";
			else
			{
				form[0].Text = "!";
				ok = false;
			}
			form["OS"].Text = " "+OSInfo.Description;
			form.Show();
			if (Installer.CheckNet8RuntimeInstalled())
			{
				form[2].Text = "x";
				form["NET"].Text = " Installed";
			}
			else
			{
				form[2].Text = "!";
				form["NET"].Text = " Not Installed";
			}
			form.Show();
			if (Installer.CheckInitSystemSupported())
			{
				string system = "";
				if (Installer.IsSystemd) system = "SystemD";
				else if (Installer.IsOpenRC) system = "OpenRC";
				else if (Installer.IsMac) system = "LaunchD";
				else throw new NotSupportedException("Only SystemD, OpenRC & LaunchD are supported.");
				form[4].Text = "x";
				form["SysD"].Text = $" {system} Supported";
			}
			else
			{
				form[4].Text = "!";
				form["SysD"].Text = " Init System not supported";
				ok = false;
			}

			form.Show();
		}
		ConsoleKeyInfo key;
		do {
			key = Console.ReadKey();
		} while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Spacebar);

		if (!ok)
		{
			var form = new ConsoleForm(@"
Prerequisite Check Failed
=========================

SolidCP cannot be installed on this System.

[    Exit    ]
");
			form.ShowDialog();
			Exit();
		}
	}

	public override void ShowLogFile()
	{
		var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Log.File);

		string txt = "";

		if (File.Exists(path)) txt = File.ReadAllText(path);
		txt = new ConsoleForm().Wrap(txt);
		var reader = new StringReader(txt);
		var lines = new List<string>();
		string line = reader.ReadLine();
		while (line != null)
		{
			lines.Add(line);
			line = reader.ReadLine();
		}

		var height = Console.WindowHeight - 4;
		int Y = 0;
		ConsoleForm form;
		do
		{
			var template = string.Join(NewLine, new[] { "SolidCP Installer Log", "=====================" }
				.Concat(Enumerable.Repeat("", height + 1))
				.Concat(new[] { "  [  Down  ]  [  Up  ]  [  Exit  ]" }));
			form = new ConsoleForm(template);
			var window = lines
				.Skip(Y)
				.Take(height);
			var count = window.Count();
			if (count < height)
			{
				window = window.Concat(Enumerable.Repeat("", height - count));
			}
			form.Template = string.Join(NewLine, new[] { "SolidCP Installer Log", "=====================" }
				.Concat(window)
				.Concat(new[] { "", "  [  Down  ]  [  Up  ]  [  Exit  ]" }));
			form.ShowDialog();

			if (form["Up"].Clicked) Y = Math.Max(0, Y - height);
			else if (form["Down"].Clicked) Y = Math.Min(lines.Count, Y + height);
		} while (!form["Exit"].Clicked); 
	}

	public override void ShowWarning(string msg)
	{
		Console.Clear();
		Console.Write("Warning: ");
		Console.WriteLine(msg);
		Console.ReadKey();
		ConsoleForm.Current?.Show();
	}

	bool downloadComplete = false;
	int maxProgress = 200;
	public override bool DownloadSetup(RemoteFile file, bool setupOnly = false)
	{
		var loader = Core.SetupLoaderFactory.CreateFileLoader(file);
		loader.ProgressChanged += DownloadProgressChanged;
		ShowInstallationProgress("Download and Extract Component");
		loader.OperationCompleted += DownloadAndUnzipCompleted;
		loader.DownloadComplete += DownloadCompleted;
		loader.NoUnzipStatus += (sender, args) => maxProgress = 100;
		loader.SetupOnly = setupOnly;
		loader.LoadAppDistributive();

		while (!downloadComplete) Thread.Sleep(100);

		return true;
	}

	float delta = 0;
	void DownloadCompleted(object sender, EventArgs args)
	{
		delta = 0.5f;
	}
	void DownloadProgressChanged(object sender, Core.LoaderEventArgs<int> args)
	{
		InstallationProgress.Progress.Value = (float)args.EventData / maxProgress + delta;
	}

	void DownloadAndUnzipCompleted(object sender, EventArgs args)
	{
		InstallationProgress.Close();
		downloadComplete = true;
	}

	public override bool ExecuteSetup(string path, string installerType, string method, object[] args)
	{
		var res = (Result)Installer.Current.LoadContext.Execute(path, installerType, method, new object[] { args });

		return res == Result.OK;
	}

	const string CancelFileName = "WaitCursor.cancel";
	string CancelFile => Path.Combine(Settings.Installer.TempPath, CancelFileName);

	public CancellationTokenSource CancelWaitCursor = new CancellationTokenSource();
	private bool CursorVisibleAfterWaitCursor;
	public override void ShowWaitCursor()
	{
		if (!Directory.Exists(Settings.Installer.TempPath)) Directory.CreateDirectory(Settings.Installer.TempPath);
		Console.Clear();
		var write = async (string txt) =>
		{
			if (CancelWaitCursor.Token.IsCancellationRequested || File.Exists(CancelFile)) throw new Exception();
			Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
			Console.Write(txt);
			await Task.Delay(333);
		};
		CursorVisibleAfterWaitCursor = false; // Console.CursorVisible;
		Console.CursorVisible = false;
		Task.Run(async () =>
		{
			try
			{
				while (true)
				{
					await write("/");
					await write("-");
					await write("\\");
					await write("|");
				}
			}
			catch {
				CancelWaitCursor = new CancellationTokenSource();
				if (File.Exists(CancelFile)) File.Delete(CancelFile);
			}
		});
	}

	public override void EndWaitCursor()
	{
		File.WriteAllText(CancelFile, "");
		CancelWaitCursor.Cancel();
	}

	public override void DownloadInstallerUpdate()
	{
		new Updater().Update();
	}
}