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
using System.Xml;

namespace SolidCP.Setup
{
	public class Global
	{
		public const string SilentInstallerShell = "SilentInstallerShell";
		public const string DefaultInstallPathRoot = @"C:\SolidCP";
		public const string LoopbackIPv4 = "127.0.0.1";
		public const string InstallerProductCode = "cfg core";
        public const string DefaultProductName = "SolidCP";

		public abstract class Parameters
		{
			public const string ComponentId = "ComponentId";
			public const string EnterpriseServerUrl = "EnterpriseServerUrl";
			public const string ShellMode = "ShellMode";
			public const string ShellVersion = "ShellVersion";
			public const string IISVersion = "IISVersion";
			public const string BaseDirectory = "BaseDirectory";
			public const string Installer = "Installer";
			public const string InstallerType = "InstallerType";
			public const string InstallerPath = "InstallerPath";
			public const string InstallerFolder = "InstallerFolder";
			public const string Version = "Version";
			public const string ComponentDescription = "ComponentDescription";
			public const string ComponentCode = "ComponentCode";
			public const string ApplicationName = "ApplicationName";
			public const string ComponentName = "ComponentName";
			public const string WebSiteIP = "WebSiteIP";
			public const string WebSitePort = "WebSitePort";
			public const string WebSiteDomain = "WebSiteDomain";
			public const string ServerPassword = "ServerPassword";
			public const string UserDomain = "UserDomain";
			public const string UserAccount = "UserAccount";
			public const string UserPassword = "UserPassword";
			public const string CryptoKey = "CryptoKey";
			public const string ServerAdminPassword = "ServerAdminPassword";
			public const string SetupXml = "SetupXml";
			public const string ParentForm = "ParentForm";
			public const string Component = "Component";
			public const string FullFilePath = "FullFilePath";
			public const string DatabaseServer = "DatabaseServer";
			public const string DbServerAdmin = "DbServerAdmin";
			public const string DbServerAdminPassword = "DbServerAdminPassword";
			public const string DatabaseName = "DatabaseName";
			public const string ConnectionString = "ConnectionString";
			public const string InstallConnectionString = "InstallConnectionString";
		    public const string Release = "Release";
            public const string SchedulerServiceFileName = "SolidCP.SchedulerService.exe";
            public const string SchedulerServiceName = "SolidCP Scheduler";
            public const string DatabaseUser = "DatabaseUser";
            public const string DatabaseUserPassword = "DatabaseUserPassword";
		}

		public abstract class Messages
		{
			public const string NotEnoughPermissionsError = "You do not have the appropriate permissions to perform this operation. Make sure you are running the application from the local disk and you have local system administrator privileges.";
			public const string InstallerVersionIsObsolete = "SolidCP Installer {0} or higher required.";
			public const string ComponentIsAlreadyInstalled = "Component or its part is already installed.";
			public const string AnotherInstanceIsRunning = "Another instance of the installation process is already running.";
			public const string NoInputParametersSpecified = "No input parameters specified";
			public const int InstallationError = -1000;
			public const int UnknownComponentCodeError = -999;
			public const int SuccessInstallation = 0;
			public const int AnotherInstanceIsRunningError = -998;
			public const int NotEnoughPermissionsErrorCode = -997;
			public const int NoInputParametersSpecifiedError = -996;
			public const int ComponentIsAlreadyInstalledError = -995;
		}

		public abstract class Server
		{
			public abstract class CLI
			{
				public const string ServerPassword = "passw";
			};

			public const string ComponentName = "Server";
			public const string ComponentCode = "server";
			public const string ComponentDescription = "SolidCP Server is a set of services running on the remote server to be controlled. Server application should be reachable from Enterprise Server one.";
			public const string ServiceAccount = "SCPServer";
			public const string DefaultPort = "9003";
			public const string DefaultIP = "127.0.0.1";
			public const string SetupController = "Server";

			public static string[] ServiceUserMembership
			{
				get
				{
                    if (IISVersion.Major >= 7)
					{
						return new string[] { "AD:Domain Admins", "SID:" + SystemSID.ADMINISTRATORS, "IIS_IUSRS" };
					}
					//
					return new string[] { "AD:Domain Admins", "SID:" + SystemSID.ADMINISTRATORS, "IIS_WPG" };
				}
			}
		}

		public abstract class StandaloneServer
		{
			public const string SetupController = "StandaloneServerSetup";
			public const string ComponentCode = "standalone";
			public const string ComponentName = "Standalone Server Setup";
            public const string ComponentDescr = "SolidCP Stand alone server installs Portal, Enterprise, and Server onto a single server. ** Active Directory options are not possible during this setup **";

        }

		public abstract class WebPortal
		{
			public const string ComponentName = "Portal";
			public const string ComponentDescription = "SolidCP Portal is a control panel itself with user interface which allows managing user accounts, hosting spaces, web sites, FTP accounts, files, etc.";
			public const string ServiceAccount = "SCPPortal";
			public const string DefaultPort = "9001";
			public const string DefaultIP = "";
			public const string DefaultEntServURL = "http://127.0.0.1:9002";
			public const string ComponentCode = "portal";
			public const string SetupController = "Portal";

			public static string[] ServiceUserMembership
			{
				get
				{
                    if (IISVersion.Major >= 7)
					{
						return new string[] { "IIS_IUSRS" };
					}
					//
					return new string[] { "IIS_WPG" };
				}
			}

			public abstract class CLI
			{
				public const string EnterpriseServerUrl = "esurl";
			}
		}

        public abstract class WebDavPortal
        {
            public const string ComponentName = "WebDavPortal";
            public const string ComponentDescription = "SolidCP Cloud Storage Portal is a control panel itself with user interface which allows managing user accounts, hosting spaces, web sites, FTP accounts, files, etc.";
            public const string ServiceAccount = "SCPWebDav";
            public const string DefaultPort = "9004";
            public const string DefaultIP = "";
            public const string ComponentCode = "WebDavPortal";
            public const string SetupController = "WebDavPortal";
            public const string DefaultDbServer = @"localhost\sqlexpress";
            public const string DefaultDatabase = "SolidCP";
            public const string AspNetConnectionStringFormat = "server={0};database={1};uid={2};pwd={3};";

            public static string[] ServiceUserMembership
            {
                get
                {
                    if (IISVersion.Major >= 7)
                    {
                        return new string[] { "IIS_IUSRS" };
                    }
                    //
                    return new string[] { "IIS_WPG" };
                }
            }

            public abstract class CLI
            {

            }
        }

        public abstract class EntServer
		{
			public const string ComponentName = "Enterprise Server";
			public const string ComponentDescription = "Enterprise Server is the heart of SolidCP system. It includes all business logic of the application. Enterprise Server should have access to Server and be accessible from Portal applications.";
			public const string ServiceAccount = "SCPEnterprise";
			public const string DefaultPort = "9002";
			public const string DefaultIP = "127.0.0.1";
			public const string DefaultDbServer = @"localhost\sqlexpress";
			public const string DefaultDatabase = "SolidCP";
			public const string AspNetConnectionStringFormat = "server={0};database={1};uid={2};pwd={3};";
			public const string ComponentCode = "enterprise server";
			public const string SetupController = "EnterpriseServer";

			public static string[] ServiceUserMembership
			{
				get
				{
                    if (IISVersion.Major >= 7)
					{
						return new string[] { "IIS_IUSRS" };
					}
					//
					return new string[] { "IIS_WPG" };
				}
			}

			public abstract class CLI
			{
				public const string ServeradminPassword = "passw";
				public const string DatabaseName = "dbname";
				public const string DatabaseServer = "dbserver";
				public const string DbServerAdmin = "dbadmin";
				public const string DbServerAdminPassword = "dbapassw";
			}
		}

        public abstract class Scheduler
        {
            public const string ComponentName = "Scheduler Service";
            public const string ComponentCode = "scheduler service";
        }

		public abstract class CLI
		{
			public const string WebSiteIP = "webip";
			public const string ServiceAccountPassword = "upassw";
			public const string ServiceAccountDomain = "udomaim";
			public const string ServiceAccountName = "uname";
			public const string WebSitePort = "webport";
			public const string WebSiteDomain = "webdom";
		}

		private Global()
		{
		}

		private static Version iisVersion;
		//
		public static Version IISVersion
		{
			get
			{
				if (iisVersion == null)
				{
					iisVersion = RegistryUtils.GetIISVersion();
				}
				//
				return iisVersion;
			}
		}

		//
		private static OS.WindowsVersion osVersion = OS.WindowsVersion.Unknown;

		/// <summary>
		/// Represents Setup Control Panel Accounts system settings set (SCPA)
		/// </summary>
		public class SCPA
		{
			public const string SettingsKeyName = "EnabledSCPA";
		}

		public static OS.WindowsVersion OSVersion
		{
			get
			{
				if (osVersion == OS.WindowsVersion.Unknown)
				{
					osVersion = OS.GetVersion();
				}
				//
				return osVersion;
			}
		}

		public static XmlDocument SetupXmlDocument { get; set; }
	}

	public class SetupEventArgs<T> : EventArgs
	{
		public T EventData { get; set; }
		public string EventMessage { get; set; }
	}
}
