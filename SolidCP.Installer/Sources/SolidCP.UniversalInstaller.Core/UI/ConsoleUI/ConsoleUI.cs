using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Text;
using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller
{

	public class ConsoleUI : UI
	{
		public override bool IsAvailable => true;
		public new class SetupWizard : UI.SetupWizard
		{
			public override UI.SetupWizard BannerWizard()
			{
				return base.BannerWizard();
			}
			public SetupWizard(UI ui) : base(ui) { }
			public override void Show()
			{
				throw new NotImplementedException();
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
			else Environment.Exit(0);
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
						f[0].Checked = false;
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
			}
			else if (form["View Log"].Clicked)
			{
				ShowLogFile();
			}
			else if (form["Check for Updates"].Clicked)
			{
				CheckForUpdate();
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
			var webService = Installer.Current.InstallerWebService;
			var components = webService.GetAvailableComponents();

			//remove already installed components or components not available on this platform
			foreach (var component in components.ToArray())
			{
				if (component.CheckForInstalledComponent()) components.Remove(component);
				else if (!component.CheckIsAvailableOnPlatform()) components.Remove(component);
				else
				{
					str.AppendLine($"[  {component.ComponentName}, {component.Version}  ]");
				}
			}
			str.AppendLine();
			str.AppendLine("[  Cancel  ]");
			var form = new ConsoleForm(str.ToString())
				.ShowDialog();
			if (form["Cancel"].Clicked) RunMainUI();
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
			var desc = component.ComponentDescription;
			while (!string.IsNullOrWhiteSpace(desc))
			{
				if (desc.Length < Console.WindowWidth)
				{
					str.AppendLine(desc);
					desc = "";
				}
				else
				{
					var space = desc.LastIndexOf(' ', Console.WindowWidth - 1);
					str.AppendLine(desc.Substring(0, space));
					desc = desc.Substring(space + 1);
				}
			}
			str.AppendLine();
			str.AppendLine("[  Cancel  ]  [  Install  ]");
			var form = new ConsoleForm(str.ToString())
				.ShowDialog();
			if (form["Cancel"].Clicked) AvailableComponents();
			else
			{

			}
		}
		public void InstalledComponents()
		{

		}

		public void CheckForUpdate()
		{
		}

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
		public override WebPortalSettings GetWebPortalSettings()
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

[    Ok    ] [    Cancel    ]
")
					.ShowDialog();
				if (form[4].Clicked) Environment.Exit(0);
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

			Exit();
		}

		public override void Init()
		{
			AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
			{
				Exit();
			};
		}
		public override void Exit()
		{
			Console.Clear();
			Console.CursorVisible = true;
		}

		public override void CheckPrerequisites()
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

  [x] Operating System:  [?OS                                ]
  [x] .NET 8 Runtime:    [?NET                                ]
  [x] SystemD:           [?SysD                                ]

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
				if (Installer.CheckSystemdSupported())
				{
					form[4].Text = "x";
					form["SysD"].Text = " SystemD Supported";
				}
				else
				{
					form[4].Text = "!";
					form["SysD"].Text = " SystemD not Supported";
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
				Environment.Exit(0);
			}
		}

		public override void ShowLogFile()
		{
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Log.File);
			if (Shell.Standard.Find("nano") != null) Shell.Standard.Exec($"nano \"{path}\"");
			else if (Installer.IsUnix) Shell.Standard.Exec($"less \"{path}\"");
			else throw new NotImplementedException();
		}

		public override void ShowWarning(string msg) => throw new NotImplementedException();

		public override bool DownloadSetup(string fileName)
		{
			throw new NotImplementedException();
		}
		public override bool ExecuteSetup(string path, string installerType, string method, object[] args)
			=> throw new NotSupportedException();
	}
}