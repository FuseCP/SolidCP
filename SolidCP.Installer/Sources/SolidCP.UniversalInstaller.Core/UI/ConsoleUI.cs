using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Text;
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

Enterprise Server Crypto Key: [?CryptoKey                                                             ]

[    Ok    ]
")
				.Load(ServerSettings)
				.Apply(f => f["RepeatPassword"].Text = f["ServerPassword"].Text)
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

Enterprise Server Crypto Key: [?CryptoKey                                                             ]

Passwords don't match!

[    Ok    ]
")
					.Load(ServerSettings)
					.Apply(f => f["RepeatPassword"].Text = f["ServerPassword"].Text)
					.ShowDialog()
					.Save(ServerSettings);
			}

			if (!ServerSettings.Urls.Split(',', ';').Select(url => url.Trim()).Any(url => url.StartsWith("https://") || url.StartsWith("net.tcp://")))
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
CryptoKey:      [?CryptoKey                                                                              ]

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
		public override WebPortalSettings GetWebPortalSettings()
		{
			var esurls = EnterpriseServerSettings.Urls;
			if (!string.IsNullOrEmpty(esurls))
			{
				var urls = esurls!.Split(';', ',').Select(url => url.Trim());
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
			}
			else
			{
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

		ConsoleForm InstallationProgress = null;
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
		public override void ShowInstallationSuccess(Packages packages)
		{
			var pckgStr = new StringBuilder();
			if (packages.HasFlag(Packages.Server)) pckgStr.AppendLine("SolidCP Server");
			if (packages.HasFlag(Packages.EnterpriseServer)) pckgStr.AppendLine("SolidCP Enterprise Server");
			if (packages.HasFlag(Packages.WebPortal)) pckgStr.AppendLine("SolidCP Web Portal");
			var template = @"
Installation Successful
=======================

You have successfully installed the following components:

@

[    Ok    ]
";
			template = template.Replace("@", pckgStr.ToString());
			var form = new ConsoleForm(template)
				.ShowDialog();
		}
	}
}