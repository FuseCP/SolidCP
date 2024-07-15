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
using System.Collections.Specialized;
using System.Text;
using System.Reflection;
using System.IO;
using System.Data;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Data;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Common;
using SolidCP.UniversalInstaller.Core;

namespace SolidCP.Setup.Actions
{
	public class SwapSetupVariablesBackAction : Action, IInstallAction, IUninstallAction, IPrepareDefaultsAction
	{
		public SetupVariables DataA { get; set; }

		void IInstallAction.Run(SetupVariables vars)
		{
			// Just replace setup variables suitable for next action set
			// B -> A
			CopySetupData(vars, DataA);
		}

		void IUninstallAction.Run(SetupVariables vars)
		{
			// Rollback scenario variables
			// A -> B
			CopySetupData(DataA, vars);
		}

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			// B -> A
			CopySetupData(vars, DataA);
		}

		private void CopySetupData(SetupVariables source, SetupVariables target)
		{
			var targetProperties = target.GetType().GetProperties();
			//
			foreach (var targetProperty in targetProperties)
			{
				if (targetProperty.CanWrite)
				{
					var sourceProperty = source.GetType().GetProperty(targetProperty.Name);
					//
					targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
				}
			}
		}
	}

	public class SwapSetupVariablesForthAction : Action, IInstallAction, IPrepareDefaultsAction
	{
		public SetupVariables DataA { get; set; }

		void IInstallAction.Run(SetupVariables vars)
		{
			// Just replace setup variables suitable for next action set
			// A -> B
			CopySetupData(DataA, vars);
		}

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			// A -> B
			CopySetupData(DataA, vars);
		}

		private void CopySetupData(SetupVariables source, SetupVariables target)
		{
			var targetProperties = target.GetType().GetProperties();
			//
			foreach (var targetProperty in targetProperties)
			{
				if (targetProperty.CanWrite)
				{
					var sourceProperty = source.GetType().GetProperty(targetProperty.Name);
					//
					targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
				}
			}
		}
	}

	public class SetupAssemblyResolverAction : Action, IInstallAction
	{
		void IInstallAction.Run(SetupVariables vars)
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler((object sender, ResolveEventArgs e) =>
			{
				Assembly ret = null;
				//
				string portalPath = AppConfig.GetComponentSettingStringValue(vars.PortalComponentId, "InstallFolder");
				string binPath = Path.Combine(portalPath, "bin");
				string path = Path.Combine(binPath, e.Name.Split(',')[0] + ".dll");
				//
				Log.WriteInfo("Assembly to resolve: " + path);
				//
				if (File.Exists(path))
				{
					ret = Assembly.LoadFrom(path);
					//
					Log.WriteInfo("Assembly resolved: " + path);
				}
				//
				return ret;
			});
		}
	}

	public class ConfigureStandaloneServerAction : Action, IInstallAction
	{
		public const string LogStartMessage = "Configuring server data...";
		public const string MyServerName = "My Server";
		public const string ServerAddFailedFormat = "Failed to add a server. Result code: {0}";
		public const string OSProviderAddFailedFormat = "Failed to add OS service provider. Reason code: {0}";
		public const string ConnectionFailedMessage = "Enterprise Server connection error";
		public const string WebServiceAddFailedFormat = "Failed to add Web service provider. Reason code: {0}";

		public SetupVariables PortalSetup { get; set; }
		public SetupVariables ServerSetup { get; set; }
		public SetupVariables EnterpriseServerSetup { get; set; }

		private Assembly ResolvePortalAssembly(object sender, ResolveEventArgs args)
		{
			Assembly ret = null;

			string portalPath = AppConfig.GetComponentSettingStringValue(PortalSetup.ComponentId, "InstallFolder");
			string binPath = Path.Combine(portalPath, "bin");
			string path = Path.Combine(binPath, args.Name.Split(',')[0] + ".dll");
			Log.WriteInfo("Assembly to resolve: " + path);
			if (File.Exists(path))
			{
				ret = Assembly.LoadFrom(path);
				Log.WriteInfo("Assembly resolved: " + path);
			}
			return ret;
		}

		private bool ConnectToEnterpriseServer(string url, string username, string password)
		{
			return ES.Connect(url, username, password);
		}

		private int AddServer(string url, string name, string password)
		{
			try
			{
				Log.WriteStart("Adding server");
				ServerInfo serverInfo = new ServerInfo()
				{
					ADAuthenticationType = null,
					ADPassword = null,
					ADEnabled = false,
					ADRootDomain = null,
					ADUsername = null,
					Comments = string.Empty,
					Password = password,
					ServerName = name,
					ServerUrl = url,
					VirtualServer = false
				};

				int serverId = ES.Services.Servers.AddServer(serverInfo, false);
				if (serverId > 0)
				{
					Log.WriteEnd("Added server");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serverId));
				}
				return serverId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Server configuration error", ex);
				return -1;
			}
		}

		private int AddIpAddress(string ip, int serverId)
		{
			try
			{
				Log.WriteStart("Adding IP address");
				IntResult res = ES.Services.Servers.AddIPAddress(IPAddressPool.General, serverId, ip, String.Empty, String.Empty, String.Empty, String.Empty, 0);
				if (res.IsSuccess && res.Value > 0)
				{
					Log.WriteEnd("Added IP address");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", res.Value));
				}
				return res.Value;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("IP address configuration error", ex);
				return -1;
			}
		}

		private int AddOSService(int serverId)
		{
			try
			{
				Log.WriteStart("Adding OS service");
				ServiceInfo serviceInfo = new ServiceInfo();
				serviceInfo.ServerId = serverId;
				serviceInfo.ServiceName = "OS";
				serviceInfo.Comments = string.Empty;

				//check OS version
				OS.WindowsVersion version = OS.GetVersion();
				if (version == OS.WindowsVersion.WindowsServer2003)
				{
					serviceInfo.ProviderId = 1;
				}
				else if (version == OS.WindowsVersion.WindowsServer2008 ||
					version == OS.WindowsVersion.WindowsServer2008R2 ||
					version == OS.WindowsVersion.Windows7)
				{
					serviceInfo.ProviderId = 100;
				}
				else if (version == OS.WindowsVersion.WindowsServer2012 ||
					version == OS.WindowsVersion.Windows8)
				{
					serviceInfo.ProviderId = 104;
				}
				else if (version == OS.WindowsVersion.WindowsServer2016 ||
					version == OS.WindowsVersion.Windows10)
				{
					serviceInfo.ProviderId = 111;
				}
				int serviceId = ES.Services.Servers.AddService(serviceInfo);
				if (serviceId > 0)
				{
					InstallService(serviceId);
					Log.WriteEnd("Added OS service");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("OS service configuration error", ex);
				return -1;
			}
		}

		private StringDictionary GetServiceSettings(int serviceId)
		{
			StringDictionary ret = null;
			try
			{
				if (serviceId > 0)
				{
					// load service properties and bind them
					string[] settings = ES.Services.Servers.GetServiceSettings(serviceId);
					ret = ConvertArrayToDictionary(settings);
				}
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Get service settings error", ex);
			}
			return ret;
		}

		private StringDictionary ConvertArrayToDictionary(string[] settings)
		{
			StringDictionary r = new StringDictionary();
			foreach (string setting in settings)
			{
				int idx = setting.IndexOf('=');
				r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
			}
			return r;
		}

		private int AddWebService(int serverId, int ipAddressId)
		{
			try
			{
				Log.WriteStart("Adding Web service");
				ServiceInfo serviceInfo = new ServiceInfo();
				serviceInfo.ServerId = serverId;
				serviceInfo.ServiceName = "Web";
				serviceInfo.Comments = string.Empty;

				//check IIS version
				if (ServerSetup.IISVersion.Major == 7)
				{
					serviceInfo.ProviderId = 101;
				}
				else if (ServerSetup.IISVersion.Major == 8)
				{
					serviceInfo.ProviderId = 105;
				}
				else if (ServerSetup.IISVersion.Major == 10)
				{
					serviceInfo.ProviderId = 112;
				}
				else if (ServerSetup.IISVersion.Major == 6)
				{
					serviceInfo.ProviderId = 2;
				}
				int serviceId = ES.Services.Servers.AddService(serviceInfo);
				if (serviceId > 0)
				{
					StringDictionary settings = GetServiceSettings(serviceId);
					if (settings != null)
					{
						// set ip address						
						if (ipAddressId > 0)
							settings["sharedip"] = ipAddressId.ToString();

						// settings for win2003 x64
						if (ServerSetup.IISVersion.Major == 6 &&
							Utils.IsWin64() && !Utils.IIS32Enabled())
						{
							settings["AspNet20Path"] = @"%SYSTEMROOT%\Microsoft.NET\Framework64\v2.0.50727\aspnet_isapi.dll";
							settings["Php4Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php.exe";
							settings["Php5Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php-cgi.exe";
						}
						// settings for win2008 x64
						if (ServerSetup.IISVersion.Major > 6 &&
							Utils.IsWin64())
						{
							settings["Php4Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php.exe";
							settings["Php5Path"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php-cgi.exe";
							settings["phppath"] = @"%SYSTEMDRIVE%\Program Files (x86)\PHP\php-cgi.exe";
						}

						// Ensure Web Deploy is installed and then enable it
						if (Utils.IsWebDeployInstalled())
						{
							settings["WDeployEnabled"] = Boolean.TrueString;
						}

						UpdateServiceSettings(serviceId, settings);
					}
					InstallService(serviceId);
					Log.WriteEnd("Added Web service");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Web service configuration error", ex);
				return -1;
			}
		}

		private void InstallService(int serviceId)
		{
			if (serviceId < 0)
				return;

			string[] installResults = null;

			try
			{
				installResults = ES.Services.Servers.InstallService(serviceId);
				foreach (string result in installResults)
				{
					Log.WriteInfo(result);
				}
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Install service error", ex);
			}
		}

		private string[] ConvertDictionaryToArray(StringDictionary settings)
		{
			List<string> r = new List<string>();
			foreach (string key in settings.Keys)
				r.Add(key + "=" + settings[key]);
			return r.ToArray();
		}

		private void UpdateServiceSettings(int serviceId, StringDictionary settings)
		{
			if (serviceId < 0 || settings == null)
				return;

			try
			{
				// save settings
				int result = ES.Services.Servers.UpdateServiceSettings(serviceId,
					ConvertDictionaryToArray(settings));

				if (result < 0)
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", result));
				}
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Update service settings error", ex);
			}
		}

		private SqlServerItem ParseConnectionString(string connectionString)
		{
			SqlServerItem ret = new SqlServerItem();

			ret.WindowsAuthentication = false;
			string[] pairs = connectionString.Split(';');
			foreach (string pair in pairs)
			{
				string[] keyValue = pair.Split('=');
				if (keyValue.Length == 2)
				{
					string key = keyValue[0].Trim().ToLower();
					string value = keyValue[1];
					switch (key)
					{
						case "server":
							ret.Server = value;
							break;
						case "database":
							ret.Database = value;
							break;
						case "integrated security":
							if (value.Trim().ToLower() == "sspi")
								ret.WindowsAuthentication = true;
							break;
						case "user":
						case "user id":
							ret.User = value;
							break;
						case "password":
							ret.Password = value;
							break;
					}
				}
			}
			return ret;
		}

		private int AddSqlService(int serverId)
		{
			int serviceId = -1;
			try
			{
				Log.WriteStart("Adding Sql service");

				SolidCP.EnterpriseServer.Data.DbType dbtype;
				string nativeConnectionString;
				DatabaseUtils.ParseConnectionString(EnterpriseServerSetup.DbInstallConnectionString, out dbtype, out nativeConnectionString);
				if (dbtype == SolidCP.EnterpriseServer.Data.DbType.SqlServer)
				{
					SqlServerItem item = ParseConnectionString(nativeConnectionString);
					string serverName = item.Server.ToLower();
					if (serverName.StartsWith("(local)") ||
						serverName.StartsWith("localhost") ||
						serverName.StartsWith(System.Environment.MachineName.ToLower()))
					{
						ServiceInfo serviceInfo = new ServiceInfo();
						serviceInfo.ServerId = serverId;
						serviceInfo.ServiceName = "SQL Server";
						serviceInfo.Comments = string.Empty;

						string connectionString = EnterpriseServerSetup.DbInstallConnectionString;
						//check SQL version
						if (DatabaseUtils.CheckSqlConnection(connectionString))
						{
							// check SQL server version
							string sqlVersion = DatabaseUtils.GetSqlServerVersion(connectionString);
							if (sqlVersion.StartsWith("9."))
							{
								serviceInfo.ProviderId = 16;
							}
							else if (sqlVersion.StartsWith("10."))
							{
								serviceInfo.ProviderId = 202;
							}
							else if (sqlVersion.StartsWith("11."))
							{
								serviceInfo.ProviderId = 209;
							}
							else if (sqlVersion.StartsWith("12."))
							{
								serviceInfo.ProviderId = 1203;
							}
							else if (sqlVersion.StartsWith("13."))
							{
								serviceInfo.ProviderId = 1701;
							}
							else if (sqlVersion.StartsWith("14."))
							{
								serviceInfo.ProviderId = 1704;
							}
							else if (sqlVersion.StartsWith("15."))
							{
								serviceInfo.ProviderId = 1705;
							}
							else if (sqlVersion.StartsWith("16."))
							{
								serviceInfo.ProviderId = 1706;
							}
							serviceId = ES.Services.Servers.AddService(serviceInfo);
						}
						else
							Log.WriteInfo("SQL Server connection error");
						//configure service
						if (serviceId > 0)
						{
							StringDictionary settings = GetServiceSettings(serviceId);
							if (settings != null)
							{
								settings["InternalAddress"] = item.Server;
								settings["ExternalAddress"] = string.Empty;
								settings["UseTrustedConnection"] = item.WindowsAuthentication.ToString();
								settings["SaLogin"] = item.User;
								settings["SaPassword"] = item.Password;
								UpdateServiceSettings(serviceId, settings);
							}
							InstallService(serviceId);
							Log.WriteEnd("Added Sql service");
						}
						else
						{
							Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
						}
					}
					else
					{
						Log.WriteError("Microsoft SQL Server was not found");
					}
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Sql service configuration error", ex);
				return -1;
			}
		}

		private int AddDnsService(int serverId, int ipAddressId)
		{
			try
			{
				Log.WriteStart("Adding DNS service");
				int providerId = 7;
				int serviceId = -1;
				BoolResult result = ES.Services.Servers.IsInstalled(serverId, providerId);
				if (result.IsSuccess && result.Value)
				{
					ServiceInfo serviceInfo = new ServiceInfo();
					serviceInfo.ServerId = serverId;
					serviceInfo.ServiceName = "DNS";
					serviceInfo.Comments = string.Empty;
					serviceInfo.ProviderId = providerId;
					serviceId = ES.Services.Servers.AddService(serviceInfo);
				}
				else
				{
					Log.WriteInfo("Microsoft DNS was not found");
					return -1;
				}

				if (serviceId > 0)
				{
					StringDictionary settings = GetServiceSettings(serviceId);
					if (settings != null)
					{
						if (ipAddressId > 0)
							settings["listeningipaddresses"] = ipAddressId.ToString();
						UpdateServiceSettings(serviceId, settings);
					}
					InstallService(serviceId);
					Log.WriteEnd("Added DNS service");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", serviceId));
				}
				return serviceId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("DNS service configuration error", ex);
				return -1;
			}
		}

		private int AddVirtualServer(string name, int serverId, int[] services)
		{
			Log.WriteStart("Adding virtual server");
			ServerInfo serverInfo = new ServerInfo()
			{
				Comments = string.Empty,
				ServerName = name,
				VirtualServer = true
			};

			int virtualServerId = ES.Services.Servers.AddServer(serverInfo, false);
			if (virtualServerId > 0)
			{
				List<int> allServices = new List<int>(services);
				List<int> validServices = new List<int>();
				foreach (int serviceId in allServices)
				{
					if (serviceId > 0)
						validServices.Add(serviceId);
				}
				ES.Services.Servers.AddVirtualServices(virtualServerId, validServices.ToArray());
				Log.WriteEnd("Added virtual server");
			}
			else
			{
				Log.WriteError(string.Format("Enterprise Server error: {0}", virtualServerId));
			}

			return virtualServerId;
		}

		private int AddUser(string loginName, string password, string firstName, string lastName, string email)
		{
			try
			{
				Log.WriteStart("Adding user account");
				UserInfo user = new UserInfo();
				user.UserId = 0;
				user.Role = UserRole.User;
				user.StatusId = 1;
				user.OwnerId = 1;
				user.IsDemo = false;
				user.IsPeer = false;
				user.HtmlMail = true;
				user.Username = loginName;
				user.FirstName = firstName;
				user.LastName = lastName;
				user.Email = email;

				int userId = ES.Services.Users.AddUser(user, false, password, new string[0]);
				if (userId > 0)
				{
					Log.WriteEnd("Added user account");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", userId));
				}
				return userId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("User configuration error", ex);
				return -1;
			}
		}

		private List<HostingPlanQuotaInfo> GetGroupQuotas(int groupId, DataView dvQuotas)
		{
			List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();
			//OS quotas
			if (groupId == 1)
				quotas = GetOSQuotas(dvQuotas);
			//Web quotas
			else if (groupId == 2)
				quotas = GetWebQuotas(dvQuotas);
			else
			{
				foreach (DataRowView quotaRow in dvQuotas)
				{
					int quotaTypeId = (int)quotaRow["QuotaTypeID"];
					HostingPlanQuotaInfo quota = new HostingPlanQuotaInfo();
					quota.QuotaId = (int)quotaRow["QuotaID"];
					quota.QuotaValue = (quotaTypeId == 1) ? 1 : -1;
					quotas.Add(quota);
				}
			}
			return quotas;
		}

		private List<HostingPlanQuotaInfo> GetOSQuotas(DataView dvQuotas)
		{
			List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();
			foreach (DataRowView quotaRow in dvQuotas)
			{
				int quotaTypeId = (int)quotaRow["QuotaTypeID"];
				string quotaName = (string)quotaRow["QuotaName"];
				HostingPlanQuotaInfo quota = new HostingPlanQuotaInfo();
				quota.QuotaId = (int)quotaRow["QuotaID"];
				quota.QuotaValue = (quotaTypeId == 1) ? 1 : -1;
				if (quotaName == "OS.AppInstaller" ||
					quotaName == "OS.ExtraApplications")
					quota.QuotaValue = 0;

				quotas.Add(quota);
			}
			return quotas;
		}

		private List<HostingPlanQuotaInfo> GetWebQuotas(DataView dvQuotas)
		{
			List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();
			foreach (DataRowView quotaRow in dvQuotas)
			{
				int quotaTypeId = (int)quotaRow["QuotaTypeID"];
				string quotaName = (string)quotaRow["QuotaName"];
				HostingPlanQuotaInfo quota = new HostingPlanQuotaInfo();
				quota.QuotaId = (int)quotaRow["QuotaID"];
				quota.QuotaValue = (quotaTypeId == 1) ? 1 : -1;
				if (quotaName == "Web.Asp" ||
					quotaName == "Web.AspNet11" ||
					quotaName == "Web.Php4" ||
					quotaName == "Web.Perl" ||
					quotaName == "Web.CgiBin" ||
					quotaName == "Web.SecuredFolders" ||
					quotaName == "Web.SharedSSL" ||
					quotaName == "Web.Python" ||
					quotaName == "Web.AppPools" ||
					quotaName == "Web.IPAddresses" ||
					quotaName == "Web.ColdFusion" ||
					quotaName == "Web.CFVirtualDirectories" ||
					quotaName == "Web.RemoteManagement")
					quota.QuotaValue = 0;

				quotas.Add(quota);
			}
			return quotas;
		}

		private int AddHostingPlan(string name, int serverId)
		{
			try
			{
				Log.WriteStart("Adding hosting plan");
				// gather form info
				HostingPlanInfo plan = new HostingPlanInfo();
				plan.UserId = 1;
				plan.PlanId = 0;
				plan.IsAddon = false;
				plan.PlanName = name;
				plan.PlanDescription = "";
				plan.Available = true; // always available

				plan.SetupPrice = 0;
				plan.RecurringPrice = 0;
				plan.RecurrenceLength = 1;
				plan.RecurrenceUnit = 2; // month

				plan.PackageId = 0;
				plan.ServerId = serverId;
				List<HostingPlanGroupInfo> groups = new List<HostingPlanGroupInfo>();
				List<HostingPlanQuotaInfo> quotas = new List<HostingPlanQuotaInfo>();

				DataSet ds = ES.Services.Packages.GetHostingPlanQuotas(-1, 0, serverId);

				foreach (DataRow groupRow in ds.Tables[0].Rows)
				{
					bool enabled = (bool)groupRow["ParentEnabled"];
					if (!enabled)
						continue; // disabled group

					int groupId = (int)groupRow["GroupId"]; ;

					HostingPlanGroupInfo group = new HostingPlanGroupInfo();
					group.GroupId = groupId;
					group.Enabled = true;
					group.GroupName = Convert.ToString(groupRow["GroupName"]);
					group.CalculateDiskSpace = (bool)groupRow["CalculateDiskSpace"];
					group.CalculateBandwidth = (bool)groupRow["CalculateBandwidth"];
					groups.Add(group);

					DataView dvQuotas = new DataView(ds.Tables[1], "GroupID=" + group.GroupId.ToString(), "", DataViewRowState.CurrentRows);
					List<HostingPlanQuotaInfo> groupQuotas = GetGroupQuotas(groupId, dvQuotas);
					quotas.AddRange(groupQuotas);
				}

				plan.Groups = groups.ToArray();
				plan.Quotas = quotas.ToArray();

				// Add Web Deploy publishing support if enabled by default
				if (Utils.IsWebDeployInstalled())
				{
					var resGroupWeb = Array.Find(plan.Groups, x => x.GroupName.Equals(ResourceGroups.Web, StringComparison.OrdinalIgnoreCase));
					//
					if (resGroupWeb != null)
					{
						EnableRemoteManagementQuota(quotas, new DataView(ds.Tables[1], String.Format("GroupID = {0}", resGroupWeb.GroupId), "", DataViewRowState.CurrentRows));
					}
				}

				int planId = ES.Services.Packages.AddHostingPlan(plan);
				if (planId > 0)
				{
					Log.WriteEnd("Added hosting plan");
				}
				else
				{
					Log.WriteError(string.Format("Enterprise Server error: {0}", planId));
				}
				return planId;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Hosting plan configuration error", ex);
				return -1;
			}
		}

		private void EnableRemoteManagementQuota(List<HostingPlanQuotaInfo> quotas, DataView dataView)
		{
			// Sort by quota name
			dataView.Sort = "QuotaName";
			// Try to find out the quota we are looking for...
			var indexOf = dataView.Find(Quotas.WEB_REMOTEMANAGEMENT);
			// Exit if nothing has been found
			if (indexOf == -1)
				return;
			// Retrieve QuotaID value from the row we have found
			var quotaId = Convert.ToInt32(dataView[indexOf]["QuotaID"]);
			// Look for the quota in quotas list
			var quotaInfo = quotas.Find(x => x.QuotaId.Equals(quotaId));
			// Exit if nothing has been found
			if (quotaInfo == default(HostingPlanQuotaInfo))
				return;
			// Enable quota if found
			quotaInfo.QuotaValue = 1;
		}

		private int AddPackage(string name, int userId, int planId)
		{
			try
			{
				Log.WriteStart("Adding hosting space");
				// gather form info
				PackageResult res = ES.Services.Packages.AddPackageWithResources(userId, planId,
					name, 1, false, false, string.Empty, false, false, false, null, false, string.Empty);
				if (res.Result > 0)
					Log.WriteEnd("Added hosting space");
				else
					Log.WriteError(string.Format("Enterprise Server error: {0}", planId));
				return res.Result;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Hosting space configuration error", ex);
				return -1;
			}
		}

		private void ConfigureWebPolicy(int userId)
		{
			try
			{
				Log.WriteStart("Configuring Web Policy");
				UserSettings settings = ES.Services.Users.GetUserSettings(userId, "WebPolicy");
				settings["AspNetInstalled"] = "2I";
				if (ServerSetup.IISVersion.Major == 6)
					settings["AspNetInstalled"] = "2";
				ES.Services.Users.UpdateUserSettings(settings);
				Log.WriteEnd("Configured Web Policy");
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Web policy configuration error", ex);
			}
		}

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				Begin(LogStartMessage);

				AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolvePortalAssembly);

				// Check connection
				if (!ConnectToEnterpriseServer(PortalSetup.EnterpriseServerURL, "serveradmin", EnterpriseServerSetup.ServerAdminPassword))
				{
					Log.WriteError("Enterprise Server connection error");
					return;
				}

				//
				OnInstallProgressChanged(LogStartMessage, 10);

				//Add server
				int serverId = AddServer(ServerSetup.RemoteServerUrl, "My Server", ServerSetup.ServerPassword);
				if (serverId < 0)
				{
					Log.WriteError(String.Format("Enterprise Server error: {0}", serverId));
					return;
				}

				//Add IP address
				string portalIP = PortalSetup.WebSiteIP;
				int ipAddressId = AddIpAddress(portalIP, serverId);

				OnInstallProgressChanged(LogStartMessage, 20);

				//Add OS service
				int osServiceId = AddOSService(serverId);
				OnInstallProgressChanged(LogStartMessage, 30);

				//Add Web service
				int webServiceId = AddWebService(serverId, ipAddressId);
				OnInstallProgressChanged(LogStartMessage, 40);

				//Add Sql service
				int sqlServiceId = AddSqlService(serverId);
				OnInstallProgressChanged(LogStartMessage, 50);

				//Add Dns service
				int dnsServiceId = AddDnsService(serverId, ipAddressId);
				OnInstallProgressChanged(LogStartMessage, 60);

				//Add virtual server
				int virtualServerId = AddVirtualServer("My Server Resources", serverId, new int[] { osServiceId, webServiceId, sqlServiceId, dnsServiceId });
				OnInstallProgressChanged(LogStartMessage, 70);

				//Add user
				int userId = AddUser("admin", EnterpriseServerSetup.PeerAdminPassword, "Server", "Administrator", "admin@myhosting.com");
				OnInstallProgressChanged(LogStartMessage, 80);

				//Add plan
				int planId = -1;
				if (virtualServerId > 0)
				{
					planId = AddHostingPlan("My Server", virtualServerId);
				}

				//Add package
				if (userId > 0 && planId > 0)
				{
					int packageId = AddPackage("My Server", userId, planId);
				}

				ConfigureWebPolicy(1);

				//
				EnterSystemIntoScpaMode();
				//
				Finish(LogStartMessage);
				Log.WriteEnd(LogStartMessage);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Server configuration error", ex);
			}
		}

		private void EnterSystemIntoScpaMode()
		{
			if (EnterpriseServerSetup.EnableScpaMode == false)
				return;
			//
			try
			{
				var scpaSysSettings = ES.Services.System.GetSystemSettings(SystemSettings.SETUP_SETTINGS);
				//
				scpaSysSettings[Global.SCPA.SettingsKeyName] = EnterpriseServerSetup.EnableScpaMode.ToString();
				//
				var resultCode = ES.Services.System.SetSystemSettings(SystemSettings.SETUP_SETTINGS, scpaSysSettings);
				//
				if (resultCode < 0)
				{
					Log.WriteError(String.Format("Failed to enter SCPA mode: {0}", resultCode));
					//
					return;
				}
				//
				Log.WriteInfo("The system has been switched to SCPA mode");
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("SCPA mode configuration error", ex);
			}
		}
	}

	public class StandaloneServerActionManager : BaseActionManager
	{
		private SetupVariables serverSetup;
		private SetupVariables esServerSetup;
		private SetupVariables portalSetup;

		public StandaloneServerActionManager(SetupVariables serverSetup, SetupVariables esServerSetup, SetupVariables portalSetup)
			: base(SetupVariables.Empty)
		{
			this.serverSetup = serverSetup;
			this.esServerSetup = esServerSetup;
			this.portalSetup = portalSetup;
			//
			Initialize += new EventHandler(StandaloneServerActionManager_Initialize);
		}

		void StandaloneServerActionManager_Initialize(object sender, EventArgs e)
		{
			switch (SessionVariables.SetupAction)
			{
				case SetupActions.Install:
					LoadInstallationScenario();
					break;
			}
		}

		private void LoadInstallationScenario()
		{
			// These actions before & after Server installation scenario are necessary to syncronize
			// changes in setup variables during the installation
			AddAction(new SwapSetupVariablesForthAction { DataA = serverSetup });
			CurrentScenario.AddRange(ServerActionManager.InstallScenario);
			AddAction(new SwapSetupVariablesBackAction { DataA = serverSetup });

			// These actions before & after EnterpriseServer installation scenario are necessary to syncronize
			// changes in setup variables during the installation
			AddAction(new SwapSetupVariablesForthAction { DataA = esServerSetup });
			CurrentScenario.AddRange(EntServerActionManager.InstallScenario);
			AddAction(new SwapSetupVariablesBackAction { DataA = esServerSetup });

			// These actions before & after WebPortal installation scenario are necessary to syncronize
			// changes in setup variables during the installation
			AddAction(new SwapSetupVariablesForthAction { DataA = portalSetup });
			CurrentScenario.AddRange(WebPortalActionManager.InstallScenario);
			AddAction(new SwapSetupVariablesBackAction { DataA = portalSetup });

			//
			AddAction(new ConfigureStandaloneServerAction
			{
				ServerSetup = serverSetup,
				EnterpriseServerSetup = esServerSetup,
				PortalSetup = portalSetup
			});
		}
	}
}
