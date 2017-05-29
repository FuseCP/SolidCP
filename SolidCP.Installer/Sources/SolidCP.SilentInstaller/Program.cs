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
using System.Collections.Generic;
using System.Text;
using SolidCP.Installer.Core;
using SolidCP.Installer.Common;
using System.Data;
using System.Threading;
using SolidCP.Installer.Configuration;
using System.IO;
using System.Collections;

namespace SolidCP.SilentInstaller
{
	class Program
	{
		public const string ComponentNameParam = "cname";

		public static readonly Hashtable ServiceCliParams = new Hashtable
		{
			{ Global.CLI.WebSiteIP, Global.Parameters.WebSiteIP },		// IP Address
			{ Global.CLI.ServiceAccountPassword, Global.Parameters.UserPassword },	// Service account password
			{ Global.CLI.ServiceAccountDomain, Global.Parameters.UserDomain },		// Service account domain (AD only)
			{ Global.CLI.ServiceAccountName, Global.Parameters.UserAccount },		// Service account name
			{ Global.CLI.WebSitePort, Global.Parameters.WebSitePort },	// TCP Port
			{ Global.CLI.WebSiteDomain, Global.Parameters.WebSiteDomain }	// Website domain (if any assigned)
		};

		public static readonly Hashtable ServerCliParams = new Hashtable
		{
			{ Global.Server.CLI.ServerPassword, Global.Parameters.ServerPassword },	// Server password
		};

		public static readonly Hashtable EntServerCliParams = new Hashtable
		{
			{ Global.EntServer.CLI.ServeradminPassword, Global.Parameters.ServerAdminPassword },		// serveradmin password
			{ Global.EntServer.CLI.DatabaseName, Global.Parameters.DatabaseName },			// Database name
			{ Global.EntServer.CLI.DatabaseServer, Global.Parameters.DatabaseServer },		// Database server
			{ Global.EntServer.CLI.DbServerAdmin, Global.Parameters.DbServerAdmin },			// Database server administrator login
			{ Global.EntServer.CLI.DbServerAdminPassword, Global.Parameters.DbServerAdminPassword }, // Database server administrator password
		};

		public static readonly Hashtable WebPortalCliParams = new Hashtable
		{
			{ Global.WebPortal.CLI.EnterpriseServerUrl, Global.Parameters.EnterpriseServerUrl },	// Enterprise Server URL
		};

        public static readonly Hashtable WebDavPortalCliParams = new Hashtable
        {
            
		};

        public static readonly Hashtable StdServerSetupCliParams = new Hashtable
		{
			{ Global.EntServer.CLI.DatabaseServer, Global.Parameters.DatabaseServer },
			{ Global.EntServer.CLI.DatabaseName, Global.Parameters.DatabaseName },
			{ Global.EntServer.CLI.DbServerAdmin , Global.Parameters.DbServerAdmin },
			{ Global.EntServer.CLI.DbServerAdminPassword, Global.Parameters.DbServerAdminPassword },
			{ Global.EntServer.CLI.ServeradminPassword, Global.Parameters.ServerAdminPassword }
		};

		[STAThread]
		static int Main(string[] args)
		{
			//
			Utils.FixConfigurationSectionDefinition();
			//
			// Ensure arguments supplied for the application
			if (args.Length == 0)
			{
				Log.WriteError(Global.Messages.NoInputParametersSpecified);
				return Global.Messages.NoInputParametersSpecifiedError;
			}

			// Check user's security permissions
			if (!Utils.CheckSecurity())
			{
				ShowSecurityError();
				return Global.Messages.NotEnoughPermissionsErrorCode;
			}

			// Check administrator permissions
			if (!Utils.IsAdministrator())
			{
				ShowSecurityError();
				return Global.Messages.NotEnoughPermissionsErrorCode;
			}

			// Check for running instance
			if (!Utils.IsNewInstance())
			{
				ShowInstanceRunningErrorMessage();
				return Global.Messages.AnotherInstanceIsRunningError;
			}

			//
			var cname = GetCommandLineArgumentValue(ComponentNameParam);

			if (Utils.CheckForInstalledComponent(cname))
			{
				Log.WriteError(Global.Messages.ComponentIsAlreadyInstalled);
				//
				return Global.Messages.ComponentIsAlreadyInstalledError;
			}

			try
			{
				// Make sure no other installations could be run at the same time
				Utils.SaveMutex();
				//
				Log.WriteApplicationStart();

				//check OS version
				Log.WriteInfo("{0} detected", Global.OSVersion);

				//check IIS version
				if (Global.IISVersion.Major == 0)
					Log.WriteError("IIS not found.");
				else
					Log.WriteInfo("IIS {0} detected", Global.IISVersion);

				var service = ServiceProviderProxy.GetInstallerWebService();
				var record = default(DataRow);
				//
				var ds = service.GetAvailableComponents();
				//
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					string componentCode = Utils.GetDbString(row["ComponentCode"]);
					//
					if (!String.Equals(componentCode, cname, StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}
					//
					record = row;
					break;
				}
				//
				if (record == null)
				{
					Log.WriteError(String.Format("{0} => {1}", ComponentNameParam, cname));
					Log.WriteInfo("Incorrect component name specified");
					return Global.Messages.UnknownComponentCodeError;
				}
				//
				var cli_args = ParseInputFromCLI(cname);
				//
				StartInstaller(record, cli_args);
				//
				return Global.Messages.SuccessInstallation;
			}
			catch (Exception ex)
			{
				Log.WriteError("Failed to install the component", ex);
				//
				return Global.Messages.InstallationError;
			}
			finally
			{
				Log.WriteApplicationEnd();
			}
		}

		private static void ShowInstanceRunningErrorMessage()
		{
			Log.WriteError(Global.Messages.AnotherInstanceIsRunning);
		}

		private static void ShowSecurityError()
		{
			Log.WriteError(Global.Messages.NotEnoughPermissionsError);
		}

		/// <summary>
		/// Parses user input from the command-line for the component specified and returns it in the form of a Hashtable to pass run the installer with arguments then.
		/// </summary>
		/// <param name="cname">Component code whose input from the command-line is being parsed</param>
		/// <exception cref="System.Exception">Thrown when wrong component code is specified. See Global class for supported component codes.</exception>
		/// <returns></returns>
		static Hashtable ParseInputFromCLI(string cname)
		{
			//
			if (cname.Equals(Global.Server.ComponentCode, StringComparison.OrdinalIgnoreCase))
			{
				//
				return LoadInputFromCLI(
					ServerCliParams,
					ServiceCliParams,
					output: new Hashtable
						{
							{ Global.Parameters.WebSiteIP, Global.Server.DefaultIP },
							{ Global.Parameters.WebSiteDomain, String.Empty },
							{ Global.Parameters.WebSitePort, Global.Server.DefaultPort },
							{ Global.Parameters.UserAccount, Global.Server.ServiceAccount },
							{ Global.Parameters.UserDomain, String.Empty },
							{ Global.Parameters.UserPassword, String.Empty },
							{ Global.Parameters.ServerPassword, null }  // Required CLI parameters must be initialized with null value!
						}
				);
			}
			else if (cname.Equals(Global.EntServer.ComponentCode, StringComparison.OrdinalIgnoreCase))
			{
				//
				return LoadInputFromCLI(
					EntServerCliParams,
					ServiceCliParams,
					output: new Hashtable
						{
							{ Global.Parameters.WebSiteIP, Global.EntServer.DefaultIP },
							{ Global.Parameters.WebSiteDomain, String.Empty },
							{ Global.Parameters.WebSitePort, Global.EntServer.DefaultPort },
							{ Global.Parameters.UserAccount, Global.EntServer.ServiceAccount },
							{ Global.Parameters.UserDomain, String.Empty },
							{ Global.Parameters.UserPassword, String.Empty },
							{ Global.Parameters.DatabaseName, Global.EntServer.DefaultDatabase },
							{ Global.Parameters.DbServerAdmin, String.Empty },
							{ Global.Parameters.DbServerAdminPassword, String.Empty },
							{ Global.Parameters.DatabaseServer, Global.EntServer.DefaultDbServer },
							{ Global.Parameters.ServerAdminPassword, null }  // Required CLI parameters must be initialized with null value!
						}
				);
			}
			else if (cname.Equals(Global.WebPortal.ComponentCode, StringComparison.OrdinalIgnoreCase))
			{
				//
				return LoadInputFromCLI(
					WebPortalCliParams,
					ServiceCliParams,
					output: new Hashtable
						{
							{ Global.Parameters.WebSiteIP, Global.WebPortal.DefaultIP },
							{ Global.Parameters.WebSiteDomain, String.Empty },
							{ Global.Parameters.WebSitePort, Global.WebPortal.DefaultPort },
							{ Global.Parameters.UserAccount, Global.WebPortal.ServiceAccount },
							{ Global.Parameters.UserDomain, String.Empty },
							{ Global.Parameters.UserPassword, String.Empty },
							{ Global.Parameters.EnterpriseServerUrl, null } // Required CLI parameters must be initialized with null value!
						}
				);
			}
            else if (cname.Equals(Global.WebDavPortal.ComponentCode, StringComparison.OrdinalIgnoreCase))
            {
                //
                return LoadInputFromCLI(
                    WebDavPortalCliParams,
                    ServiceCliParams,
                    output: new Hashtable
                        {
                            { Global.Parameters.WebSiteIP, Global.WebDavPortal.DefaultIP },
                            { Global.Parameters.WebSiteDomain, String.Empty },
                            { Global.Parameters.WebSitePort, Global.WebDavPortal.DefaultPort },
                            { Global.Parameters.UserAccount, Global.WebDavPortal.ServiceAccount },
                            { Global.Parameters.UserDomain, String.Empty },
                            { Global.Parameters.UserPassword, String.Empty },
						}
                );
            }
            else if (cname.Equals(Global.StandaloneServer.ComponentCode, StringComparison.OrdinalIgnoreCase))
			{
				return LoadInputFromCLI(
					StdServerSetupCliParams,
					ServiceCliParams,
					output: new Hashtable
					{
						{ Global.Parameters.WebSiteIP, Global.WebPortal.DefaultIP },
						{ Global.Parameters.WebSiteDomain, String.Empty },
						{ Global.Parameters.WebSitePort, Global.WebPortal.DefaultPort },
						{ Global.Parameters.DatabaseName, Global.EntServer.DefaultDatabase },
						{ Global.Parameters.DatabaseServer, Global.EntServer.DefaultDbServer },
						{ Global.Parameters.DbServerAdmin, String.Empty },
						{ Global.Parameters.DbServerAdminPassword, String.Empty },
						{ Global.Parameters.ServerAdminPassword, String.Empty }
					}
				);
			}
			//
			throw new Exception("Wrong component code!");
		}

		/// <summary>
		/// Loads an input from the command-line interface into a Hashtable instance to use for calls to the Installer and checks whether or not the required parameters are set.
		/// </summary>
		/// <param name="inputA">A collection with service-specific parameter names to match the input from the command-line interface (CLI)</param>
		/// <param name="inputB">A collection with service-generic parameter names to match the input from the command-line interface (CLI)</param>
		/// <param name="output">An output storage to put the input data into</param>
		/// <returns>This method</returns>
		static Hashtable LoadInputFromCLI(Hashtable inputA, Hashtable inputB, Hashtable output)
		{
			// Process service
			foreach (var item in inputA.Keys)
			{
				var cli_argv = GetCommandLineArgumentValue(item as String);
				//
				if (String.IsNullOrEmpty(cli_argv))
					continue;
				// Assign argument value from CLI
				output[inputA[item]] = cli_argv;
			}
			//
			foreach (var item in inputB.Keys)
			{
				var cli_argv = GetCommandLineArgumentValue(item as String);
				//
				if (String.IsNullOrEmpty(cli_argv))
					continue;
				// Assign argument value from CLI
				output[inputB[item]] = cli_argv;
			}
			// Ensure all required parameters are set
			foreach (var item in output.Keys)
			{
				// If a parameter is required, then you should assign null value
				if (output[item] == null)
				{
					throw new ArgumentNullException(item as String);
				}
			}
			//
			return output;
		}

		static void StartInstaller(DataRow row, Hashtable args)
		{
			string applicationName = Utils.GetDbString(row["ApplicationName"]);
			string componentName = Utils.GetDbString(row["ComponentName"]);
			string componentCode = Utils.GetDbString(row["ComponentCode"]);
			string componentDescription = Utils.GetDbString(row["ComponentDescription"]);
			string component = Utils.GetDbString(row["Component"]);
			string version = Utils.GetDbString(row["Version"]);
			string fileName = row["FullFilePath"].ToString();
			string installerPath = Utils.GetDbString(row["InstallerPath"]);
			string installerType = Utils.GetDbString(row["InstallerType"]);

            // Get appropriate loader to download the app distributive
            var loader = LoaderFactory.CreateFileLoader(fileName);
            // Mimic synchronous download process for the console app
            AutoResetEvent autoEvent = new AutoResetEvent(false);

			try
			{
				//
				loader.OperationCompleted += new EventHandler<EventArgs>((object sender, EventArgs e) =>
				{
					Log.WriteInfo("Download completed!");
					//
					string tmpFolder = FileUtils.GetTempDirectory();
					string path = Path.Combine(tmpFolder, installerPath);
					//Update();
					string method = "Install";
					Log.WriteStart(string.Format("Running installer {0}.{1} from {2}", installerType, method, path));

					//prepare installer args
					args[Global.Parameters.ComponentName] = componentName;
					args[Global.Parameters.ApplicationName] = applicationName;
					args[Global.Parameters.ComponentCode] = componentCode;
					args[Global.Parameters.ComponentDescription] = componentDescription;
					args[Global.Parameters.Version] = version;
					args[Global.Parameters.InstallerFolder] = tmpFolder;
					args[Global.Parameters.InstallerPath] = installerPath;
					args[Global.Parameters.InstallerType] = installerType;
					args[Global.Parameters.Installer] = Path.GetFileName(fileName);
					args[Global.Parameters.BaseDirectory] = FileUtils.GetCurrentDirectory();
					args[Global.Parameters.IISVersion] = Global.IISVersion;
					args[Global.Parameters.ShellVersion] = AssemblyLoader.GetShellVersion();
					args[Global.Parameters.ShellMode] = Global.SilentInstallerShell;
					args[Global.Parameters.SetupXml] = String.Empty;

					// Run the installer
					var res = AssemblyLoader.Execute(path, installerType, method, new object[] { args });
					Log.WriteInfo(string.Format("Installer returned {0}", res));
					Log.WriteEnd("Installer finished");
					// Remove temporary directory
					FileUtils.DeleteTempDirectory();
                    // We are done
                    autoEvent.Set();
				});
                // TODO: Add cleanup for events.
				loader.OperationFailed += new EventHandler<LoaderEventArgs<Exception>>(loader_OperationFailed);
				loader.ProgressChanged += new EventHandler<LoaderEventArgs<int>>(loader_ProgressChanged);
				loader.StatusChanged += new EventHandler<LoaderEventArgs<string>>(loader_StatusChanged);
				//
				loader.LoadAppDistributive();
                // Wait until the download is complete
                autoEvent.WaitOne();
			}
			catch (Exception ex)
			{
				Log.WriteError("Installer error", ex);
			}
			finally
			{
                autoEvent.Set();
			}
		}

		static void loader_StatusChanged(object sender, LoaderEventArgs<string> e)
		{
			if (String.IsNullOrEmpty(e.EventData))
				Log.WriteInfo(e.StatusMessage);
			else
				Log.WriteInfo("{0} {1}", e.StatusMessage, e.EventData);
		}

		static void loader_ProgressChanged(object sender, LoaderEventArgs<int> e)
		{
			if (!String.IsNullOrEmpty(e.StatusMessage))
				Log.WriteInfo("{0} {1}%", e.StatusMessage, e.EventData);
			else
				Log.WriteInfo("Current progress is {0}%", e.EventData);
		}

		static void loader_OperationFailed(object sender, LoaderEventArgs<Exception> e)
		{
			Log.WriteInfo(e.EventData.ToString());
		}

		/// <summary>
		/// Check for existing command line argument
		/// </summary>
		private static bool CheckCommandLineArgument(string argName)
		{
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 1; i < args.Length; i++)
			{
				string arg = args[i];
				if (string.Equals(arg, argName, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Check for existing command line argument
		/// </summary>
		private static string GetCommandLineArgumentValue(string argName)
		{
			string key = "/" + argName.ToLower() + ":";
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 1; i < args.Length; i++)
			{
				string arg = args[i].ToLower();
				if (arg.StartsWith(key))
				{
					// Remove leading and trailing double quotes if any
					return args[i].Substring(key.Length).TrimStart('"').TrimEnd('"');
				}
			}
			return null;
		}
	}
}
