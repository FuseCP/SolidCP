using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller
{

	public class ConsoleUI : UI
	{
		public override string GetRootPassword()
		{
			var rootUser = OSInfo.IsWindows ? "administartor" : "root";
			var form = new ConsoleForm(@$"
SolidCP Installer
=================

SolidCP Installer must run as {rootUser}.
Please enter {rootUser} password:

[?Password                                      ]

[*    Ok    ]
")
				.ShowDialog();
			return form["Password"].Text;
		}

		public override ServerSettings GetServerSettings()
		{

			var settings = Installer.ReadServerConfiguration();

			var form = new ConsoleForm(@"
Server Settings:
================

Server Urls: [?Urls http:\\localhost:9003                                                   ]

Server Password: [!ServerPassword                                         ]
Repeat Password: [!RepeatPassword                                         ]

[*    Ok    ]
")
				.Load(settings)
				.ShowDialog()
				.Save(settings);

			while (form["ServerPassword"].Text != form["RepeatPassword"].Text)
			{
				form.Parse(@"
Server Settings:
================

Server Urls: [?Urls http:\\localhost:9003                                                   ]

Server Password: [!ServerPassword                                         ]
Repeat Password: [!RepeatPassword                                         ]

Passwords don't match!

[*    Ok    ]
")
					.Load(settings)
					.ShowDialog()
					.Save(settings);
			}
			return settings;
		}
		public override EnterpriseServerSettings GetEnterpriseServerSettings()
		{
			throw new NotImplementedException();
		}
		public override WebPortalSettings GetWebPortalSettings()
		{
			throw new NotImplementedException();
		}
		public override Packages GetPackagesToInstall()
		{
			return Packages.Server;
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

	}
}