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
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Principal;

using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Server.Utils;
using Microsoft.Crm;
using Microsoft.Crm.Admin.AdminService;
using Microsoft.Crm.ConfigurationDatabase;
using Microsoft.Crm.Data;
using Microsoft.Crm.Setup.Database;
using Microsoft.Crm.Setup.Server.Utility;
using Microsoft.Crm.Tools.Admin;
using Microsoft.Win32;
using Microsoft.Crm.Setup.Common.Update;

namespace SolidCP.Providers.HostedSolution
{
	public class CRMProvider : HostingServiceProviderBase, ICRM
	{
		private static string crmPath = null;

		#region Properties

		private string SqlServer
		{
			get { return ProviderSettings[Constants.SqlServer]; }
		}

		private string ReportingServer
		{
			get { return ProviderSettings[Constants.ReportingServer]; }
		}

		private string CRMServiceUrl
		{
			get
			{
				string url = string.Format("http://{0}/mscrmservices/2007/crmservice.asmx",
					ProviderSettings[Constants.AppRootDomain]);

				return url;
			}
		}


		private static string CrmPath
		{
			get
			{
				if (string.IsNullOrEmpty(crmPath))
				{
					RegistryKey root = Registry.LocalMachine;
					RegistryKey rk = root.OpenSubKey("SOFTWARE\\Microsoft\\MSCRM");
					if (rk != null)
					{
						crmPath = (string)rk.GetValue("CRM_Server_InstallDir", string.Empty);
					}
				}
				return crmPath;
			}
		}

		#endregion


		#region Static constructor

		static CRMProvider()
		{
			AppDomain.CurrentDomain.AssemblyResolve += ResolveCRMAssembly;
		}

		#endregion


		static Assembly ResolveCRMAssembly(object sender, ResolveEventArgs args)
		{
			var loadedAssembly = default(Assembly);
			// Ensure we load DLLs only.
			if (args.Name.ToLower().Contains("microsoft.crm") || args.Name.ToLower().Contains("antixsslibrary"))
			{
				//
				string crmToolsPath = Path.Combine(CrmPath, "tools");
				//
				string path = Path.Combine(crmToolsPath, args.Name.Split(',')[0] + ".dll");
				// Call to load an assembly only if its existence is confirmed.
				if (File.Exists(path))
				{
					loadedAssembly = Assembly.LoadFrom(path);
				}
			}
			//
			return loadedAssembly;
		}

		private bool CheckCRMWebServicesAccess()
		{
			Log.WriteStart("CheckCRMWebServicesAccess");
			bool ret = false;
			HttpWebResponse response = null;
			HttpWebRequest request;

			try
			{
				WindowsIdentity.GetCurrent();

				request = WebRequest.Create(CRMServiceUrl) as HttpWebRequest;

				if (request != null)
				{
					request.UseDefaultCredentials = true;
					request.Credentials = CredentialCache.DefaultCredentials;
					response = request.GetResponse() as HttpWebResponse;

				}
				if (response != null)
					ret = (response.StatusCode == HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				ret = false;
			}

			Log.WriteEnd("CheckCRMWebServicesAccess");
			return ret;
		}

		private static bool CheckPermissions()
		{
			Log.WriteStart("CheckPermissions");
			bool res = false;
			try
			{
				string group = "PrivUserGroup";
				string user = WindowsIdentity.GetCurrent().Name.Split(new char[] { '\\' })[1];
				res = ActiveDirectoryUtils.IsUserInGroup(user, group);
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				res = false;
			}

			Log.WriteEnd("CheckPermissions");
			return res;
		}

		private bool CheckOrganizationUnique(string databaseName, string orgName)
		{
			Log.WriteStart("CheckOrganizationUnique");
			bool res = false;

			SqlConnection connection = null;
			try
			{
				connection = new SqlConnection();
				connection.ConnectionString =
				   string.Format("Server={1};Initial Catalog={0};Integrated Security=SSPI",
								  databaseName, SqlServer);

				connection.Open();

				string commandText = string.Format("SELECT COUNT(*) FROM dbo.Organization WHERE UniqueName = '{0}'", orgName);
				SqlCommand command = new SqlCommand(commandText, connection);
				int count = (int)command.ExecuteScalar();
				res = count == 0;


			}
			catch (Exception ex)
			{
				res = false;
				Log.WriteError(ex);
			}
			finally
			{
				if (connection != null)
					connection.Dispose();

			}

			Log.WriteEnd("CheckOrganizationUnique");
			return res;
		}

		private bool CheckSqlServerConnection()
		{
			Log.WriteStart("CheckSqlServerConnection");
			bool res = false;
			SqlConnection connection = null;
			try
			{
				connection = new SqlConnection();
				connection.ConnectionString =
					string.Format("server={0}; Integrated Security=SSPI",
								  SqlServer);

				connection.Open();
				res = true;
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				res = false;
			}
			finally
			{
				if (connection != null)
					connection.Dispose();
			}

			Log.WriteEnd("CheckSqlServerConnection");

			return res;
		}

		private bool CheckReportServerConnection()
		{
			Log.WriteStart("CheckReportServerConnection");
			bool ret = false;
			HttpWebResponse response = null;
			HttpWebRequest request;

			try
			{
				WindowsIdentity.GetCurrent();

				request = WebRequest.Create(ReportingServer) as HttpWebRequest;

				if (request != null)
				{
					request.UseDefaultCredentials = true;
					request.Credentials = CredentialCache.DefaultCredentials;
					response = request.GetResponse() as HttpWebResponse;

				}
				if (response != null)
					ret = (response.StatusCode == HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				ret = false;
			}

			Log.WriteEnd("CheckReportServerConnection");
			return ret;
		}

		private OrganizationResult CheckCrmEnvironment(string strDataBaseName, string organizationUniqueName)
		{
			OrganizationResult retOrganization = StartLog<OrganizationResult>("CheckCrmEnvironment");
			bool res = CheckSqlServerConnection();

			if (!res)
			{
				EndLog("CheckCrmEnvironment", retOrganization, CrmErrorCodes.CRM_SQL_SERVER_ERROR);
				return retOrganization;
			}

			res = CheckOrganizationUnique(strDataBaseName, organizationUniqueName);
			if (!res)
			{
				EndLog("CheckCrmEnvironment", retOrganization, CrmErrorCodes.CRM_ORGANIZATION_ALREADY_EXISTS);
				return retOrganization;
			}

			res = CheckReportServerConnection();
			if (!res)
			{
				EndLog("CheckCrmEnvironment", retOrganization, CrmErrorCodes.CRM_REPORT_SERVER_ERROR);
				return retOrganization;
			}

			res = CheckPermissions();
			if (!res)
			{
				EndLog("CheckCrmEnvironment", retOrganization, CrmErrorCodes.CRM_PERMISSIONS_ERROR);
				return retOrganization;
			}

			res = CheckCRMWebServicesAccess();
			if (!res)
			{
				EndLog("CheckCrmEnvironment", retOrganization, CrmErrorCodes.CRM_WEB_SERVICE_ERROR);
				return retOrganization;
			}


			EndLog("CheckCrmEnvironment");
			return retOrganization;
		}

        public OrganizationResult CreateOrganization(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, int baseLanguageCode, string ou, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation, long maxSize)
		{
			return CreateOrganizationInternal(organizationId, organizationUniqueName, organizationFriendlyName, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, organizationCollation);
		}

		internal OrganizationResult CreateOrganizationInternal(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string organizationCollation)
		{
			OrganizationResult ret = StartLog<OrganizationResult>("CreateOrganizationInternal");
			if (organizationId == Guid.Empty)
				throw new ArgumentException("OrganizationId is Guid.Empty");

			if (string.IsNullOrEmpty(organizationFriendlyName))
				throw new ArgumentNullException("organizationFriendlyName");

			if (string.IsNullOrEmpty(baseCurrencyCode))
				throw new ArgumentNullException("baseCurrencyCode");

			if (string.IsNullOrEmpty(baseCurrencySymbol))
				throw new ArgumentNullException("baseCurrencySymbol");

			if (string.IsNullOrEmpty(initialUserDomainName))
				throw new ArgumentNullException("initialUserDomainName");

			string strDataBaseName = "MSCRM_CONFIG";

			OrganizationResult retCheckEn = CheckCrmEnvironment(strDataBaseName, organizationUniqueName);

			if (!retCheckEn.IsSuccess)
			{
				ret.ErrorCodes.AddRange(retCheckEn.ErrorCodes);
				EndLog("CreateOrganizationInternal", ret, null, null);
				return ret;
			}

			Uri reportServerUrl = new Uri(ReportingServer);

			try
			{
				SecurityGroupsData securityGroups = new CrmConfigSettingsService().GetSecurityGroups();
				string privilegedUserGroupName = string.Format(CultureInfo.InvariantCulture, "<GUID={0}>", new object[] { securityGroups.PrivilegeUserGroupId.ToString("D") });
				string sqlAccessGroupName = string.Format(CultureInfo.InvariantCulture, "<GUID={0}>", new object[] { securityGroups.SqlAccessGroupId.ToString("D") });
				string userGroupName = string.Format(CultureInfo.InvariantCulture, "<GUID={0}>", new object[] { securityGroups.UserGroupId.ToString("D") });
				string reportingGroupName = string.Format(CultureInfo.InvariantCulture, "<GUID={0}>", new object[] { securityGroups.ReportingGroupId.ToString("D") });
				string privilegedReportingGroupName = string.Format(CultureInfo.InvariantCulture, "<GUID={0}>", new object[] { securityGroups.PrivilegeReportGroupId.ToString("D") });


				ResultObject resCreateOrg = Create(organizationId, organizationUniqueName, organizationFriendlyName, baseCurrencyCode,
					   baseCurrencyName,
					   baseCurrencySymbol, initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail,
					   SqlServer, reportServerUrl, privilegedUserGroupName, sqlAccessGroupName, userGroupName,
					   reportingGroupName, privilegedReportingGroupName,
					   CrmPath, false, organizationCollation);

				ret.ErrorCodes.AddRange(resCreateOrg.ErrorCodes);

				if (!resCreateOrg.IsSuccess)
				{
					EndLog("CreateOrganizationInternal", ret, null, null);
					return ret;

				}

			}
			catch (Exception ex)
			{
				HostedSolutionLog.LogError(ex);
				EndLog("CheckCrmEnvironment", ret, CrmErrorCodes.CREATE_CRM_ORGANIZATION_GENERAL_ERROR, ex);
				return ret;

			}

			EndLog("CheckCrmEnvironment");

			return ret;
		}


		private static ResultObject CreateCrmOrganizationDataBase(Guid organizationId, string databaseName, string sqlServerName, string sqlAccessGroupName, string reportingGroupName, string privilegedReportingGroupName)
		{

			ResultObject res = StartLog<ResultObject>("CreateCrmOrganizationDataBase");

			try
			{
				ServerDatabaseInstaller installer;
				string languageId = (string)RegistryCache.GetValue("LanguageID", "1033");
				int languageCode = Convert.ToInt32(languageId, CultureInfo.InvariantCulture);
				string configurationFilePath = Path.Combine(CrmPath, @"Tools\Sql\install.xml");
				string metadataDatabaseName = "MSCRM_CONFIG";

				installer = new ServerDatabaseInstaller(organizationId);

				installer.CreateDatabase(languageCode, configurationFilePath, sqlServerName, metadataDatabaseName,
										 databaseName, sqlAccessGroupName, reportingGroupName,
										 privilegedReportingGroupName);
			}
			catch (Exception ex)
			{
				EndLog("CreateCrmOrganizationDataBase", res, CrmErrorCodes.CANNOT_CREATE_CRM_ORGANIZATION_DATABASE, ex);
				return res;
			}

			EndLog("CreateCrmOrganizationDataBase");
			return res;
		}

		private static ResultObject SetDataBaseCollation(Guid organizationId, string databaseName, string sqlServerName, string applicationPath, string organizationCollation)
		{
			ResultObject res = StartLog<ResultObject>("SetDataBaseCollation");

			try
			{
				string sqlScriptRoot = Path.Combine(applicationPath, "Tools");
				NewOrgUtility.SetDatabaseCollation(organizationId, organizationCollation, databaseName,
												   OrganizationController.Instance.GetLocaleIdForCollation(
													   organizationCollation), sqlServerName, sqlScriptRoot);
			}
			catch (Exception ex)
			{
				EndLog("SetDataBaseCollation", res, CrmErrorCodes.CANNOT_SET_DATABASE_COLLATION, ex);
				return res;
			}

			EndLog("SetDataBaseCollation");
			return res;
		}

		private string GetDomainName(string username)
		{
			string domain = ActiveDirectoryUtils.GetNETBIOSDomainName(ServerSettings.ADRootDomain);
			string ret = string.Format(@"{0}\{1}", domain, username);
			return ret;
		}

		private ResultObject ConfigureOrganization(Guid organizationId, string languageId, string organizationFriendlyName, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string privilegedUserGroupName, string sqlAccessGroupName, string userGroupName, string reportingGroupName, string privilegedReportingGroupName,
			  bool sqmOption)
		{
			ResultObject res = StartLog<ResultObject>("ConfigureOrganization");
			res.IsSuccess = true;

			try
			{
				//seems to be a MS bug
				//string strDomainUserName = ActiveDirectoryHelper.GetDomainName(initialUserDomainName);
				string strDomainUserName = GetDomainName(initialUserDomainName);

				string importFileLocation = Path.Combine(CrmPath, "Tools");
				NewOrgUtility.ConfigureOrganization(organizationId.ToString("B"), organizationFriendlyName,
													strDomainUserName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail,
													languageId, privilegedUserGroupName, sqlAccessGroupName,
													userGroupName, reportingGroupName, privilegedReportingGroupName,
													true, NewOrgUtility.OrganizationGetAutoManagement(),
													importFileLocation, sqmOption);
			}
			catch (Exception ex)
			{
				EndLog("ConfigureOrganization", res, CrmErrorCodes.CANNOT_CONFIGURE_CRM_ORGANIZATION, ex);
				return res;
			}

			EndLog("ConfigureOrganization");
			return res;
		}

		private static ResultObject SetBaseCurrency(Guid organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol)
		{
			ResultObject res = StartLog<ResultObject>("SetBaseCurrency");
			res.IsSuccess = true;

			try
			{
				NewOrgUtility.OrganizationSetBaseCurrency(organizationId, baseCurrencyCode, baseCurrencyName,
														  baseCurrencySymbol);
			}
			catch (Exception ex)
			{
				EndLog("SetBaseCurrency", res, CrmErrorCodes.CANNOT_SET_ORGANIZATION_CURRENCY, ex);
				return res;
			}

			EndLog("SetBaseCurrency");
			return res;
		}

		public const string APPLY_VERSION_SPECIFIC_UPDATES = "ApplyVersionSpecificUpdates";

		private static ResultObject ApplyVersionSpecificUpdatesIfAny(Guid organizationId, int languageCode)
		{
			var result = StartLog<ResultObject>(APPLY_VERSION_SPECIFIC_UPDATES);
			//
			try
			{
				// Instantiate a tool class to works with a CRM organization's database
				var dbInstaller = new ServerDatabaseInstaller(organizationId);
				// Retrieve database version
				var dbVersion = dbInstaller.GetVersion();
				// Instantiate DBUpdateDatabaseInstaller to retrieve a list of updates available for CRM Server component
				var dbUpdateInstaller = new DBUpdateDatabaseInstaller(dbVersion.Revision, (int)CrmComponent.Server);
				// Check for updates available for th
				if (dbUpdateInstaller.IsDatabaseUpdateNeeded() == true)
				{
					// Assign corresponding language id before starting the update procedure
					CrmResourceManager.LanguageId = languageCode;
					// Apply corresponding organization database updates
					DBUpdateDatabaseInstaller.ApplyDBUpdates(organizationId);
				}
				// The update operation has been successfully completed
				result.IsSuccess = true;
			}
			catch (Exception ex)
			{
				EndLog(APPLY_VERSION_SPECIFIC_UPDATES, result, CrmErrorCodes.CANNOT_APPLY_CRM_UPDATES, ex);
				//
				return result;
			}
			//
			EndLog(APPLY_VERSION_SPECIFIC_UPDATES);
			//
			return result;
		}

		private static ResultObject CreateOrganizationReport(string organizationUniqueName, string applicationPath, Uri reportServerUrl, int languageCode)
		{
			ResultObject res = StartLog<ResultObject>("CreateOrganizationReport");
			res.IsSuccess = true;
			try
			{
				string reportFilesLocation = Path.Combine(applicationPath, "Reports");
				bool grantNetworkService = IIsUtility.IsLocalUrl(reportServerUrl.OriginalString);
				ReportsUtility.CreateReports(organizationUniqueName, grantNetworkService, reportServerUrl.OriginalString,
											 reportFilesLocation, languageCode);
			}
			catch (Exception ex)
			{
				EndLog("CreateOrganizationReport", res, CrmErrorCodes.CANNOT_CREATE_CRM_REPORT, ex);
				return res;
			}
			EndLog("CreateOrganizationReport");
			return res;
		}


		private ResultObject Create(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, string sqlServerName, Uri reportServerUrl, string privilegedUserGroupName, string sqlAccessGroupName, string userGroupName, string reportingGroupName, string privilegedReportingGroupName,
			 string applicationPath, bool sqmOption, string organizationCollation)
		{
			ResultObject res = StartLog<ResultObject>("Create");
			res.IsSuccess = true;

			OrganizationData data;
			string languageId = (string)RegistryCache.GetValue("LanguageID", "1033");
			int languageCode = Convert.ToInt32(languageId, CultureInfo.InvariantCulture);


			string databaseName = organizationUniqueName + "_MSCRM";
			string connectionString =
				string.Format("Provider=SQLOLEDB;Data Source={1};Initial Catalog={0};Integrated Security=SSPI",
							  databaseName, sqlServerName);


			CrmOrganizationService service = new CrmOrganizationService();
			bool orgCreated = false;
			try
			{
				data =
					service.Create(organizationId, organizationUniqueName, organizationFriendlyName, sqlServerName,
								   databaseName, connectionString, reportServerUrl.OriginalString, 0,
								   OrganizationState.Pending);

				orgCreated = true;
				if (data.UniqueName != organizationUniqueName)
				{
					throw new CrmArgumentException(
						string.Format(CultureInfo.InvariantCulture, "Organization name '{0}' and '{1}' should be equal",
									  new object[] { data.UniqueName, organizationUniqueName }));
				}
				if (data.Id != organizationId)
				{
					throw new CrmArgumentException(
						string.Format(CultureInfo.InvariantCulture, "Organization Id '{0}' and '{1}' should be equal",
									  new object[] { data.Id, organizationId }));
				}
			}
			catch (Exception ex)
			{
				EndLog("Create", res, CrmErrorCodes.CANNOT_CREATE_CRM_ORGANIZATION, ex);
				if (orgCreated)
					service.Delete(organizationId);
				return res;
			}


			//CreateDataBase
			ResultObject resCreateDataBase = CreateCrmOrganizationDataBase(organizationId, databaseName, sqlServerName, sqlAccessGroupName,
												reportingGroupName, privilegedReportingGroupName);
			res.ErrorCodes.AddRange(resCreateDataBase.ErrorCodes);
			if (!resCreateDataBase.IsSuccess)
			{
				EndLog("Create", res);
				return res;
			}


			//Set collation
			ResultObject resSetCollation = SetDataBaseCollation(organizationId, databaseName, sqlServerName, applicationPath, organizationCollation);
			res.ErrorCodes.AddRange(resSetCollation.ErrorCodes);


			//Configure organization
			ResultObject resConfigure = ConfigureOrganization(organizationId, languageId, organizationFriendlyName, initialUserDomainName,
								   initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, privilegedUserGroupName,
								   sqlAccessGroupName, userGroupName, reportingGroupName, privilegedReportingGroupName,
								   sqmOption);

			res.ErrorCodes.AddRange(resConfigure.ErrorCodes);


			//Set Currency
			ResultObject resSetCurrency = SetBaseCurrency(organizationId, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol);
			res.ErrorCodes.AddRange(resSetCurrency.ErrorCodes);


			//Create organiztion report
			ResultObject resCreateReport = CreateOrganizationReport(organizationUniqueName, applicationPath, reportServerUrl, languageCode);
			res.ErrorCodes.AddRange(resCreateReport.ErrorCodes);

			// Apply version-specific updates if any
			var typeInfo = Type.GetType("Microsoft.Crm.Setup.Common.Update.DBUpdateDatabaseInstaller, Microsoft.Crm.Setup.Common", false);
			// Ensure the environment we run on is DBUpdateDatabaseInstaller-compatible
			if (typeInfo != null)
			{
				var runDbUpdates = false;
				// Ensure DBUpdateDatabaseInstaller class carries ApplyDBUpdates method
				try
				{
					var methodInfo = typeInfo.GetMethod("ApplyDBUpdates");
					//
					if (methodInfo != null)
					{
						runDbUpdates = true;
					}
				}
				catch (AmbiguousMatchException)
				{
					Log.WriteWarning("Found a couple of DBUpdateDatabaseInstaller.ApplyDBUpdates methods.");
					// We are still ok to continue to run the update procedure.
					runDbUpdates = true;
				}
				//
				if (runDbUpdates)
				{
					var resVersionUpd = ApplyVersionSpecificUpdatesIfAny(organizationId, languageCode);
					//
					res.ErrorCodes.AddRange(resVersionUpd.ErrorCodes);
				}
			}

			//Enable Organization
			try
			{
				service.SetState(organizationId, OrganizationState.Enabled);
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				EndLog("Create", res, CrmErrorCodes.CANNOT_ENABLE_CRM_ORGANIZATION, ex);
				return res;
			}

			try
			{
				using (ConfigurationDatabaseService service2 = new ConfigurationDatabaseService())
				{
					using (CrmDbConnection connection = service2.CreateConnection())
					{
						connection.Open();
						Guid id = GetFetureId("CreateUser");
						UpdateOrgFeatureState(service2, organizationId, id, "False");
					}
				}
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				EndLog("Create", res, CrmErrorCodes.CANNOT_DISABLE_USER_FEATURES, ex);
			}

			EndLog("Create");
			return res;
		}

		public string[] GetSupportedCollationNames()
		{
			return GetSupportedCollationNamesInternal();
		}

		internal static string[] GetSupportedCollationNamesInternal()
		{
			HostedSolutionLog.LogStart("GetSupportedCollationNamesInternal");

			List<string> ret = new List<string>();
			foreach (string str in DatabaseUtility.CollationMap.CollationNames)
			{
				if (!ret.Contains(str))
					ret.Add(str);
			}

			HostedSolutionLog.LogEnd("GetSupportedCollationNamesInternal");
			return ret.ToArray();
		}

		public Currency[] GetCurrencyList()
		{
			return GetCurrencyListInternal();
		}

		internal static Currency[] GetCurrencyListInternal()
		{
			HostedSolutionLog.LogStart("GetCurrencyListInternal");

			DataTable dt = CrmUtility.RetrieveCurrencyList();
			List<Currency> retList = new List<Currency>();

			foreach (DataRow row in dt.Rows)
			{
				Currency currency = new Currency();
				currency.RegionName = row["regionname"].ToString();
				currency.CurrencyName = row["currencyname"].ToString();
				currency.CurrencyCode = row["currencycode"].ToString();
				currency.CurrencySymbol = row["currencysymbol"].ToString();
				retList.Add(currency);
			}

			HostedSolutionLog.LogEnd("GetCurrencyListInternal");
			return retList.ToArray();
		}


		public ResultObject DeleteOrganization(Guid orgId)
		{
			return DeleteOrganizationInternal(orgId);
		}

		internal static ResultObject DeleteOrganizationInternal(Guid orgId)
		{
			ResultObject res = StartLog<ResultObject>("DeleteOrganizationInternal");
			res.IsSuccess = true;
			try
			{
				CrmOrganizationService service = new CrmOrganizationService();
				try
				{
					service.SetState(orgId, OrganizationState.Disabled);
				}
				catch (Exception ex)
				{
					EndLog("DeleteOrganizationInternal", res, CrmErrorCodes.CANNOT_CHANGE_CRM_ORGANIZATION_STATE, ex);
					return res;
				}

				try
				{
					service.Delete(orgId);
				}
				catch (Exception ex)
				{
					EndLog("DeleteOrganizationInternal", res, CrmErrorCodes.CANNOT_DELETE_CRM_ORGANIZATION, ex);
					return res;
				}
			}
			catch (Exception ex)
			{
				EndLog("DeleteOrganizationInternal", res, CrmErrorCodes.DELETE_CRM_ORGANIZATION_GENERAL_ERROR, ex);
				return res;

			}

			EndLog("DeleteOrganizationInternal");
			return res;
		}

		public override void DeleteServiceItems(ServiceProviderItem[] items)
		{
			foreach (ServiceProviderItem item in items)
			{
				try
				{
					if (item is Organization)
					{
						Organization org = item as Organization;
						DeleteOrganization(org.CrmOrganizationId);
					}

				}
				catch (Exception ex)
				{
					Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
				}
			}
			base.DeleteServiceItems(items);
		}

		private static void EndLog(string message, ResultObject res, string errorCode, Exception ex)
		{
			if (res != null)
			{
				res.IsSuccess = false;

				if (!string.IsNullOrEmpty(errorCode))
					res.ErrorCodes.Add(errorCode);
			}

			if (ex != null)
				HostedSolutionLog.LogError(ex);


			//LogRecord.
			HostedSolutionLog.LogEnd(message);


		}

		private static void EndLog(string message, ResultObject res, string errorCode)
		{
			EndLog(message, res, errorCode, null);
		}

		private static void EndLog(string message, ResultObject res)
		{
			EndLog(message, res, null);
		}

		private static void EndLog(string message)
		{
			EndLog(message, null);
		}

		private static T StartLog<T>(string message) where T : ResultObject, new()
		{
			HostedSolutionLog.LogStart(message);
			T res = new T();
			res.IsSuccess = true;
			return res;
		}

		public UserResult CreateCRMUser(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType)
		{
			return CreateCRMUserInternal(user, orgName, organizationId, baseUnitId);
		}

		internal UserResult CreateCRMUserInternal(OrganizationUser user, string orgName, Guid organizationId, Guid businessUnitId)
		{
			UserResult res = StartLog<UserResult>("CreateCRMUserInternal");

			try
			{
				if (user == null)
					throw new ArgumentNullException("user");

				if (string.IsNullOrEmpty(orgName))
					throw new ArgumentNullException("orgName");

				if (organizationId == Guid.Empty)
					throw new ArgumentException("organizationId");

				if (businessUnitId == Guid.Empty)
					throw new ArgumentException("businessUnitId");

				try
				{
					CrmService service = GetCRMService(orgName);

					systemuser systemUser = new systemuser();

					Picklist accessMode = new Picklist();
					accessMode.Value = 0;

					UniqueIdentifier orgId = new UniqueIdentifier();
					orgId.Value = organizationId;

					systemUser.firstname = user.FirstName;
					systemUser.lastname = user.LastName;

					Lookup businessId = new Lookup();
					businessId.Value = businessUnitId;
					businessId.type = EntityName.businessunit.ToString();

					systemUser.businessunitid = businessId;
					systemUser.organizationid = orgId;
					systemUser.accessmode = accessMode;
					systemUser.domainname = user.DomainUserName;
					systemUser.personalemailaddress = user.ExternalEmail;
					systemUser.internalemailaddress = user.PrimaryEmailAddress;

					Guid guid = service.Create(systemUser);

					user.CrmUserId = guid;
					res.Value = user;

				}
				catch (Exception ex)
				{
					EndLog("CreateCRMUserInternal", res, CrmErrorCodes.CANNOT_CREATE_CRM_USER, ex);
					return res;
				}
			}
			catch (Exception ex)
			{
				EndLog("CreateCRMUserInternal", res, CrmErrorCodes.CANNOT_CREATE_CRM_USER_GENERAL_ERROR, ex);
				return res;

			}

			EndLog("CreateCRMUserInternal");
			return res;
		}

		private CrmService GetCRMService(string orgName)
		{
			CrmAuthenticationToken token = new CrmAuthenticationToken();
			token.AuthenticationType = 0;
			token.OrganizationName = orgName;

			CrmService service = new CrmService();
			service.Url = CRMServiceUrl;

			service.CrmAuthenticationTokenValue = token;

			service.Credentials = CredentialCache.DefaultCredentials;

			return service;
		}

		internal CRMBusinessUnitsResult GetOrganizationBusinessUnitsInternal(Guid organizationId, string orgName)
		{
			CRMBusinessUnitsResult res = StartLog<CRMBusinessUnitsResult>("GetOrganizationBusinessUnitsInternal");

			try
			{
				if (organizationId == Guid.Empty)
					throw new ArgumentException("organizationId");

				if (string.IsNullOrEmpty(orgName))
					throw new ArgumentNullException("orgName");

				CrmService service;
				RetrieveMultipleResponse baseUnits;

				try
				{
					service = GetCRMService(orgName);
				}
				catch (Exception ex)
				{
					EndLog("GetOrganizationBusinessUnitsInternal", res, CrmErrorCodes.CANNOT_GET_CRM_SERVICE, ex);
					return res;
				}

				try
				{
					ColumnSet cols = new ColumnSet();
					cols.Attributes = new string[] { "name", "businessunitid" };

					ConditionExpression condition = new ConditionExpression();

					condition.AttributeName = "organizationid";
					condition.Operator = ConditionOperator.Equal;
					condition.Values = new object[1];
					condition.Values[0] = organizationId;

					// Create the FilterExpression.
					FilterExpression filter = new FilterExpression();

					filter.Conditions = new ConditionExpression[] { condition };

					QueryExpression query = new QueryExpression();

					query.EntityName = EntityName.businessunit.ToString();
					query.ColumnSet = cols;
					query.Criteria = filter;

					// Create the request object.
					RetrieveMultipleRequest retrieveUnitsInOrganization = new RetrieveMultipleRequest();

					// Set the properties of the request object.
					retrieveUnitsInOrganization.Query = query;

					baseUnits =
						(RetrieveMultipleResponse)service.Execute(retrieveUnitsInOrganization);
				}
				catch (Exception ex)
				{
					EndLog("GetOrganizationBusinessUnitsInternal", res, CrmErrorCodes.CANNOT_GET_CRM_BUSINESS_UNITS, ex);
					return res;
				}

				List<CRMBusinessUnit> businessUnits = new List<CRMBusinessUnit>();

				try
				{
					foreach (businessunit bu in baseUnits.BusinessEntityCollection.BusinessEntities)
					{
						CRMBusinessUnit unit = new CRMBusinessUnit();
						unit.BusinessUnitId = bu.businessunitid.Value;
						unit.BusinessUnitName = bu.name;

						businessUnits.Add(unit);
					}

					res.Value = businessUnits;
				}
				catch (Exception ex)
				{
					EndLog("GetOrganizationBusinessUnitsInternal", res, CrmErrorCodes.CANNOT_FILL_BASE_UNITS_COLLECTION,
						   ex);
					return res;
				}
			}
			catch (Exception ex)
			{
				EndLog("GetOrganizationBusinessUnitsInternal", res, CrmErrorCodes.GET_ORGANIZATION_BUSINESS_UNITS_GENERAL_ERROR,
				   ex);
				return res;

			}

			EndLog("GetOrganizationBusinessUnitsInternal");
			return res;

		}

		public CRMBusinessUnitsResult GetOrganizationBusinessUnits(Guid organizationId, string orgName)
		{
			return GetOrganizationBusinessUnitsInternal(organizationId, orgName);
		}

		public CrmRolesResult GetAllCrmRoles(string orgName, Guid businessUnitId)
		{
			return GetAllCrmRolesInternal(orgName, businessUnitId);
		}

		public CrmRolesResult GetCrmUserRoles(string orgName, Guid userId)
		{
			return GetCrmUserRolesInternal(userId, orgName);
		}

		internal CrmRolesResult GetCrmUserRolesInternal(Guid userId, string orgName)
		{
			CrmRolesResult res = StartLog<CrmRolesResult>("GetCrmUserRolesInternal");

			try
			{
				BusinessEntityCollection relations;

				if (userId == Guid.Empty)
					throw new ArgumentException("userId");

				if (string.IsNullOrEmpty(orgName))
					throw new ArgumentNullException("orgName");

				try
				{
					// Set up the condition for the returned records.
					ConditionExpression condition = new ConditionExpression();
					condition.AttributeName = "systemuserid";
					condition.Operator = ConditionOperator.Equal;
					condition.Values = new string[] { userId.ToString() };

					// Build the filter based on the condition.
					FilterExpression filter = new FilterExpression();
					filter.FilterOperator = LogicalOperator.And;
					filter.Conditions = new ConditionExpression[] { condition };

					// Link the user information.
					LinkEntity link = new LinkEntity();

					link.LinkFromEntityName = EntityName.role.ToString();

					link.LinkFromAttributeName = "roleid";
					link.LinkToEntityName = "systemuserroles";
					link.LinkToAttributeName = "roleid";
					link.LinkCriteria = filter;

					// Build the query for the retrieval.

					QueryExpression query = new QueryExpression();
					query.EntityName = EntityName.role.ToString();
					query.ColumnSet = new AllColumns();
					query.LinkEntities = new LinkEntity[] { link };

					CrmService service = GetCRMService(orgName);

					relations = service.RetrieveMultiple(query);
				}
				catch (Exception ex)
				{
					EndLog("GetCrmUserRolesInternal", res, CrmErrorCodes.CANNOT_GET_CRM_USER_ROLES, ex);
					return res;
				}

				try
				{
					res.Value = FillCrmRoles(relations, true, Guid.Empty);
				}
				catch (Exception ex)
				{
					EndLog("GetCrmUserRolesInternal", res, CrmErrorCodes.CANNOT_FILL_ROLES_COLLECTION, ex);
					return res;
				}
			}
			catch (Exception ex)
			{
				EndLog("GetCrmUserRolesInternal", res, CrmErrorCodes.GET_CRM_USER_ROLE_GENERAL_ERROR, ex);
				return res;

			}
			EndLog("GetCrmUserRolesInternal");
			return res;
		}

		private static List<CrmRole> FillCrmRoles(BusinessEntityCollection entities, bool isUserRole, Guid businessUnitId)
		{
			List<CrmRole> res = new List<CrmRole>();

			foreach (role current in entities.BusinessEntities)
			{
				if (businessUnitId == Guid.Empty || businessUnitId == current.businessunitid.Value)
				{
					if (current.name.Trim().ToLower() == "support user")
						continue;

					CrmRole crmRole = new CrmRole();
					crmRole.IsCurrentUserRole = isUserRole;
					crmRole.RoleId = current.roleid.Value;
					crmRole.RoleName = current.name;

					res.Add(crmRole);
				}
			}
			return res;
		}

		private static List<CrmRole> FillCrmRoles(BusinessEntityCollection entities, Guid businessUnitId)
		{
			return FillCrmRoles(entities, false, businessUnitId);
		}

		internal CrmRolesResult GetAllCrmRolesInternal(string orgName, Guid businessUnitId)
		{
			CrmRolesResult res = StartLog<CrmRolesResult>("GetAllCrmRoles");

			try
			{
				if (string.IsNullOrEmpty(orgName))
					throw new ArgumentNullException("orgName");

				BusinessEntityCollection roles;
				try
				{
					CrmService service = GetCRMService(orgName);

					QueryExpression query = new QueryExpression();

					query.EntityName = EntityName.role.ToString();
					query.ColumnSet = new AllColumns();

					roles = service.RetrieveMultiple(query);
				}
				catch (Exception ex)
				{
					EndLog("GetAllCrmRoles", res, CrmErrorCodes.CANNOT_GET_ALL_CRM_ROLES, ex);
					return res;
				}

				try
				{
					List<CrmRole> crmRoles = FillCrmRoles(roles, businessUnitId);
					res.Value = crmRoles;
				}
				catch (Exception ex)
				{
					EndLog("GetAllCrmRoles", res, CrmErrorCodes.CANNOT_FILL_ROLES_COLLECTION, ex);
					return res;
				}
			}
			catch (Exception ex)
			{
				EndLog("GetAllCrmRoles", res, CrmErrorCodes.GET_ALL_CRM_ROLES_GENERAL_ERROR, ex);
				return res;
			}
			EndLog("GetAllCrmRoles");
			return res;
		}

		public ResultObject SetUserRoles(string orgName, Guid userId, Guid[] roles)
		{
			return SetUserRolesInternal(orgName, userId, roles);
		}

		internal ResultObject SetUserRolesInternal(string orgName, Guid userId, Guid[] roles)
		{
			CrmRolesResult res = StartLog<CrmRolesResult>("SetUserRolesInternal");

			try
			{
				if (string.IsNullOrEmpty(orgName))
					throw new ArgumentNullException("orgName");

				if (userId == Guid.Empty)
					throw new ArgumentException("userId");

				if (roles == null)
					throw new ArgumentNullException("roles");

				CrmService service = GetCRMService(orgName);


				CrmRolesResult tmpRoles = GetCrmUserRoles(orgName, userId);
				res.ErrorCodes.AddRange(tmpRoles.ErrorCodes);

				if (!tmpRoles.IsSuccess)
					return res;

				List<Guid> remRoles = new List<Guid>();

				for (int i = 0; i < tmpRoles.Value.Count; i++)
				{
					if (Array.Find(roles, delegate(Guid current) { return current == tmpRoles.Value[i].RoleId; }) == Guid.Empty)
					{
						remRoles.Add(tmpRoles.Value[i].RoleId);
					}
				}

				try
				{
					RemoveUserRolesRoleRequest removeRole = new RemoveUserRolesRoleRequest();
					removeRole.RoleIds = remRoles.ToArray();
					removeRole.UserId = userId;
					service.Execute(removeRole);
				}
				catch (Exception ex)
				{
					EndLog("SetUserRolesInternal", res, CrmErrorCodes.CANNOT_REMOVE_CRM_USER_ROLES, ex);
					return res;
				}


				try
				{
					AssignUserRolesRoleRequest request = new AssignUserRolesRoleRequest();
					request.RoleIds = roles;
					request.UserId = userId;
					service.Execute(request);
				}
				catch (Exception ex)
				{
					EndLog("SetUserRolesInternal", res, CrmErrorCodes.CANNOT_ASSIGN_CRM_USER_ROLES, ex);
					return res;
				}

			}
			catch (Exception ex)
			{
				EndLog("SetUserRolesInternal", res, CrmErrorCodes.CANNOT_SET_CRM_USER_ROLES_GENERAL_ERROR, ex);
				return res;
			}

			EndLog("SetUserRolesInternal");
			return res;

		}


		public CrmUserResult GetCrmUserById(Guid crmUserId, string orgName)
		{
			return GetCrmUserByIdInternal(crmUserId, orgName);
		}

		internal CrmUserResult GetCrmUserByIdInternal(Guid crmUserId, string orgName)
		{
			CrmUserResult ret = StartLog<CrmUserResult>("GetCrmUserByIdInternal");
			try
			{
				if (crmUserId == Guid.Empty)
					throw new ArgumentNullException("crmUserId");

				if (string.IsNullOrEmpty(orgName))
					throw new ArgumentNullException("orgName");

				CrmService service = GetCRMService(orgName);

				ConditionExpression condition = new ConditionExpression();
				condition.AttributeName = "systemuserid";
				condition.Operator = ConditionOperator.Equal;
				condition.Values = new object[] { crmUserId };

				// Build the filter based on the condition.
				FilterExpression filter = new FilterExpression();
				filter.FilterOperator = LogicalOperator.And;
				filter.Conditions = new ConditionExpression[] { condition };


				QueryExpression query = new QueryExpression();

				query.EntityName = EntityName.systemuser.ToString();
				query.ColumnSet = new AllColumns();
				query.Criteria = filter;


				BusinessEntityCollection res = service.RetrieveMultiple(query);
				CrmUser user = null;

				if (res != null && res.BusinessEntities != null && res.BusinessEntities.Length > 0)
				{
					systemuser sysuser = res.BusinessEntities[0] as systemuser;
					if (sysuser != null)
					{
						user = new CrmUser();
						user.BusinessUnitId = sysuser.businessunitid.Value;
						user.CRMUserId = sysuser.systemuserid.Value;
						user.ClientAccessMode = (CRMUserAccessMode)sysuser.accessmode.Value;
						user.IsDisabled = sysuser.isdisabled.Value;
						ret.Value = user;
					}
				}
			}
			catch (Exception ex)
			{
				EndLog("GetCrmUserByIdInternal", ret, CrmErrorCodes.CANONT_GET_CRM_USER_BY_ID, ex);
				return ret;
			}

			EndLog("GetCrmUserByIdInternal");
			return ret;
		}


		internal CrmUserResult GetCrmUserByDomainNameInternal(string domainName, string orgName)
		{
			CrmUserResult ret = StartLog<CrmUserResult>("GetCrmUserByDomainNameInternal");
			try
			{
				if (string.IsNullOrEmpty(domainName))
					throw new ArgumentNullException("domainName");

				if (string.IsNullOrEmpty(orgName))
					throw new ArgumentNullException("orgName");

				CrmService service = GetCRMService(orgName);

				ConditionExpression condition = new ConditionExpression();
				condition.AttributeName = "domainname";
				condition.Operator = ConditionOperator.Equal;
				condition.Values = new string[] { domainName };

				// Build the filter based on the condition.
				FilterExpression filter = new FilterExpression();
				filter.FilterOperator = LogicalOperator.And;
				filter.Conditions = new ConditionExpression[] { condition };


				QueryExpression query = new QueryExpression();

				query.EntityName = EntityName.systemuser.ToString();
				query.ColumnSet = new AllColumns();
				query.Criteria = filter;


				BusinessEntityCollection res = service.RetrieveMultiple(query);
				CrmUser user = null;

				if (res != null && res.BusinessEntities != null && res.BusinessEntities.Length > 0)
				{
					systemuser sysuser = res.BusinessEntities[0] as systemuser;
					if (sysuser != null)
					{
						user = new CrmUser();
						user.BusinessUnitId = sysuser.businessunitid.Value;
						user.CRMUserId = sysuser.systemuserid.Value;
						user.ClientAccessMode = (CRMUserAccessMode)sysuser.accessmode.Value;
						user.IsDisabled = sysuser.isdisabled.Value;
						ret.Value = user;
					}
				}
			}
			catch (Exception ex)
			{
				EndLog("GetCrmUserByDomainNameInternal", ret, CrmErrorCodes.CANONT_GET_CRM_USER_BY_DOMAIN_NAME, ex);
				return ret;
			}

			EndLog("GetCrmUserByDomainNameInternal");
			return ret;
		}

		public CrmUserResult GetCrmUserByDomainName(string domainName, string orgName)
		{
			return GetCrmUserByDomainNameInternal(domainName, orgName);
		}


		private static Guid GetFetureId(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			Dictionary<string, Guid> dictionary = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
			using (ConfigurationDatabaseService service = new ConfigurationDatabaseService())
			{
				PropertyBagCollection bags = service.Retrieve("Feature", null, null);
				if (bags == null)
				{
					throw new CrmException("No features defined");
				}
				foreach (PropertyBag bag in bags.Values)
				{
					string str = (string)bag["Name"];
					if (str.ToLower().Trim() == name.ToLower().Trim())
					{
						return (Guid)bag["Id"];
					}

				}
			}
			return Guid.Empty;
		}

		private static bool UpdateOrgFeatureState(ConfigurationDatabaseService cfg, Guid organizationId, Guid featureId, string newOrgFeatureState)
		{
			bool flag = false;
			string[] columns = new string[] { "Id", "Enabled" };
			PropertyBag bag = new PropertyBag();
			bag["OrganizationId"] = organizationId;
			bag["FeatureId"] = featureId;
			PropertyBag[] conditions = new PropertyBag[] { bag };
			PropertyBagCollection bags = cfg.Retrieve("OrganizationFeatureMap", columns, conditions);
			if ((bags == null) || (bags.Count == 0))
			{
				throw ExceptionsHelper.BuildConfigDBObjectDoesNotExist("OrganizationFeature", featureId);
			}
			if (1 < bags.Count)
			{
				throw ExceptionsHelper.BuildConfigDBDuplicateRecord("OrganizationFeature", featureId);
			}
			SortedDictionary<object, PropertyBag>.Enumerator enumerator = bags.GetEnumerator();
			enumerator.MoveNext();
			KeyValuePair<object, PropertyBag> current = enumerator.Current;
			PropertyBag columnSet = current.Value;
			flag = (bool)columnSet["Enabled"];
			columnSet["Enabled"] = newOrgFeatureState;
			cfg.Update("OrganizationFeatureMap", columnSet["Id"], columnSet);
			return flag;
		}


		public ResultObject ChangeUserState(bool disable, string orgName, Guid crmUserId)
		{
			return ChangeUserStateInternal(disable, orgName, crmUserId);
		}


		internal ResultObject ChangeUserStateInternal(bool disable, string orgName, Guid crmUserId)
		{
			ResultObject res = StartLog<ResultObject>("ChangeUserStateInternal");
			res.IsSuccess = true;
			try
			{
				if (string.IsNullOrEmpty(orgName))
					throw new ArgumentNullException("orgName");

				if (crmUserId == Guid.Empty)
					throw new ArgumentException("crmUserId");

				CrmService service = GetCRMService(orgName);

				SetStateSystemUserRequest request = new SetStateSystemUserRequest();
				request.EntityId = crmUserId;
				request.SystemUserState = disable ? SystemUserState.Inactive : SystemUserState.Active;
				request.SystemUserStatus = -1;

				service.Execute(request);
			}
			catch (Exception ex)
			{
				EndLog("ChangeUserStateInternal", res, CrmErrorCodes.CANNOT_CHANGE_USER_STATE, ex);
				return res;
			}

			EndLog("ChangeUserStateInternal");
			return res;
		}


		public override bool IsInstalled()
		{
			string value = string.Empty;
			try
			{
				RegistryKey root = Registry.LocalMachine;
				RegistryKey rk = root.OpenSubKey("SOFTWARE\\Microsoft\\MSCRM");

				if (rk == null)
					rk = root.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\MSCRM");

				if (rk != null)
				{
					value = (string)rk.GetValue("CRM_Server_Version", null);
					rk.Close();
				}
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
			}

			return value.StartsWith("4.");
		}

        public long GetUsedSpace(Guid organizationId)
        {
            return 0;
        }

        public ResultObject SetUserCALType(string orgName, Guid userId, int CALType)
        {
            ResultObject ret = new ResultObject();
            ret.IsSuccess = false;
            return ret;
        }

        public long GetDBSize(Guid organizationId)
        {
            return -1;
        }

        public long GetMaxDBSize(Guid organizationId)
        {
            return -1;
        }

        public ResultObject SetMaxDBSize(Guid organizationId, long maxSize)
        {
            ResultObject ret = new ResultObject();
            ret.IsSuccess = false;
            return ret;
        }

        public int[] GetInstalledLanguagePacks()
        {
            return null;
        }
	}

}
