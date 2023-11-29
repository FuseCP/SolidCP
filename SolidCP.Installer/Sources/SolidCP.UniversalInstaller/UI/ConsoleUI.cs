using System.Text.RegularExpressions;
using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller
{

	public class ConsoleUI : UI
	{
		public override string GetRootPassword()
		{
			var rootUser = OSInfo.IsWindows ? "administrator" : "root";
			var form = new ConsoleForm(@$"
SolidCP Installer
=================

SolidCP Installer must run as {rootUser}.
Please enter {rootUser} password:

[!Password                                      ]

[    Ok    ]
")
				.ShowDialog();
			return form["Password"].Text;
		}

		public override ServerSettings GetServerSettings()
		{
			var form = new ConsoleForm(@"
Server Settings:
================

Server Urls: [?Urls http://localhost:9003                                                   ]

Server Password: [!ServerPassword                                         ]
Repeat Password: [!RepeatPassword                                         ]

[    Ok    ]
")
				.Load(ServerSettings)
				.ShowDialog()
				.Save(ServerSettings);

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
					.Load(ServerSettings)
					.ShowDialog()
					.Save(ServerSettings);
			}

			GetCommonSettings(ServerSettings);

			return ServerSettings;
		}
		public override EnterpriseServerSettings GetEnterpriseServerSettings()
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
				.Load(EnterpriseServerSettings)
				.ShowDialog()
				.Save(EnterpriseServerSettings);

			GetCommonSettings(EnterpriseServerSettings);
			return EnterpriseServerSettings;
		}
		bool IsLocalHttp(string url)
		{
			var uri = new Uri(url);
			var host = uri.Host;

			return uri.Scheme == "http" && (host == "localhost" || host == "127.0.0.1" || host == "::1" || Regex.IsMatch(host, @"^192\.168\.[0-9]{1,3}\.[0-9]{1,3}$"));
		}
		bool IsLocalHttps(string url)
		{
			var uri = new Uri(url);
			var host = uri.Host;

			return uri.Scheme == "https" && (host == "localhost" || host == "127.0.0.1" || host == "::1" || Regex.IsMatch(host, @"^192\.168\.[0-9]{1,3}\.[0-9]{1,3}$"));
		}
		public override WebPortalSettings GetWebPortalSettings()
		{
			var esurls = EnterpriseServerSettings.Urls;
			if (!string.IsNullOrEmpty(esurls))
			{
				var urls = esurls!.Split(';', ',');
				var esurl = urls.FirstOrDefault(url => IsLocalHttp(url));
				if (esurl == null) esurl = urls.FirstOrDefault(url => IsLocalHttps(url));
				if (esurl != null)
				{
					WebPortalSettings.EnterpriseServerUrl = esurl;
				}
			}

			var form = new ConsoleForm(@"
Web Portal Settings
===================

Enterprise Server Url: [?EnterpriseServerUrl http://localhost:9002                                        ]

[    Ok    ]
")
				.Load(WebPortalSettings)
				.ShowDialog()
				.Save(WebPortalSettings);

			return WebPortalSettings;
		}
		public override Packages GetPackagesToInstall()
		{
			Packages packages = 0;
			if (OSInfo.IsWindows)
			{
				var form = new ConsoleForm(@"
Components to Install
=====================

[x] Install Server
[x] Install Enterprise Server
[x] Install Web Portal   

[    Ok    ]
")
					.ShowDialog();
				if (form[0].Checked) packages |= Packages.Server;
				if (form[1].Checked) packages |= Packages.EnterpriseServer;
				if (form[2].Checked) packages |= Packages.WebPortal;
			} else {
				packages |= Packages.Server;
			}
			return packages;
		}
		public override void ShowError(Exception ex)
		{
			Console.Clear();
			Console.WriteLine("Error:");
			Console.WriteLine(ex.ToString());
		}

		ConsoleForm? InstallationProgress = null;
		public override void ShowInstallationProgress()
		{
			InstallationProgress = new ConsoleForm(@"
Installation Progress:
======================

[%Progress                                                                      ]
")
			.ShowProgress(Installer.Shell, Installer.EstimatedOutputLines)
			.Show();
		}

		public override void CloseInstallationProgress()
		{
			if (InstallationProgress != null) InstallationProgress.Close();
		}

		public override void GetCommonSettings(CommonSettings settings)
		{
			if (settings.Urls!.Split(';', ',').Any(url => url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)))
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
				}
				else if (form[1].Clicked)
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
				}
				else if (form[2].Clicked)
				{
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
	}
}