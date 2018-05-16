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
using System.Net.Security;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.ServiceModel.Description;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using SolidCP.Server.Utils;
using SolidCP.Providers.HostedSolution;
using SolidCP.Providers;
using SolidCP.Providers.Utils;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Deployment;
using Microsoft.Xrm.Sdk.Messages;

namespace SolidCP.Providers.HostedSolution
{
    public class CRMBase : HostingServiceProviderBase, ICRM
    {
        #region Properties

        protected virtual string UserName
        {
            get { return ProviderSettings[Constants.UserName]; }
        }

        protected virtual string Password
        {
            get { return ProviderSettings[Constants.Password]; }
        }

        protected virtual string SqlServer
        {
            get { return ProviderSettings[Constants.SqlServer]; }
        }

        protected virtual string ReportingServer
        {
            get { return ProviderSettings[Constants.ReportingServer]; }
        }

        protected virtual string UrlSchema
        {
            get { return ProviderSettings[Constants.UrlSchema] == "http" ? "http://" : "https://"; }
        }

        protected virtual string CRMDeploymentUrl
        {
            get
            {
                string uri = ProviderSettings[Constants.DeploymentWebService];
                if (String.IsNullOrEmpty(uri)) uri = ProviderSettings[Constants.AppRootDomain];
                string cRMServiceUrl = UrlSchema + uri + ":" + ProviderSettings[Constants.Port] + "/XRMDeployment/2011/Deployment.svc";
                return cRMServiceUrl;
            }
        }

        protected virtual string CRMDiscoveryUrl
        {
            get
            {
                string uri = ProviderSettings[Constants.DiscoveryWebService];
                if (String.IsNullOrEmpty(uri)) uri = ProviderSettings[Constants.AppRootDomain];
                string cRMDiscoveryUri = UrlSchema + uri + ":" + ProviderSettings[Constants.Port] + "/XRMServices/2011/Discovery.svc";
                return cRMDiscoveryUri;
            }
        }

        private static string crmPath = null;
        protected static string CRMPath
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

        protected static string CRMDatabaseName = "MSCRM_CONFIG";

        #endregion

        #region Service

        protected virtual ClientCredentials GetUserLogonCredentials()
        {
            ClientCredentials credentials = new ClientCredentials();

            if (String.IsNullOrEmpty(UserName))
            {
                credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                credentials.UserName.UserName = UserName;
                credentials.UserName.Password = Password;
            }

            return credentials;
        }

        protected virtual DeploymentServiceClient GetDeploymentProxy()
        {
            Uri serviceUrl = new Uri(CRMDeploymentUrl);

            DeploymentServiceClient deploymentService = Microsoft.Xrm.Sdk.Deployment.Proxy.ProxyClientHelper.CreateClient(serviceUrl);
            if (!String.IsNullOrEmpty(UserName))
                deploymentService.ClientCredentials.Windows.ClientCredential = new NetworkCredential(UserName, Password);

            return deploymentService;
        }

        protected virtual DiscoveryServiceProxy GetDiscoveryProxy()
        {

            IServiceManagement<IDiscoveryService> serviceManagement =
                        ServiceConfigurationFactory.CreateManagement<IDiscoveryService>(
                        new Uri(CRMDiscoveryUrl));

            ClientCredentials Credentials = GetUserLogonCredentials();

            DiscoveryServiceProxy r = new DiscoveryServiceProxy(serviceManagement, Credentials);

            return r;
        }

        private OrganizationDetailCollection DiscoverOrganizations(IDiscoveryService service)
        {
            if (service == null) throw new ArgumentNullException("service");
            RetrieveOrganizationsRequest orgRequest = new RetrieveOrganizationsRequest();
            RetrieveOrganizationsResponse orgResponse =
                (RetrieveOrganizationsResponse)service.Execute(orgRequest);

            return orgResponse.Details;
        }

        public virtual string GetOrganizationUniqueName(string orgName)
        {
            return Regex.Replace(orgName, @"[^\dA-Za-z]", "-", RegexOptions.Compiled);
        }

        protected virtual Uri GetCRMOrganizationUrl(string orgName)
        {
            //string url = "https://" + ProviderSettings[Constants.AppRootDomain] + ":" + ProviderSettings[Constants.Port] + "/" + orgName + "/XRMServices/2011/Organization.svc";

            string url;

            orgName = GetOrganizationUniqueName(orgName);

            string organizationWebServiceUri = ProviderSettings[Constants.OrganizationWebService];

            if (String.IsNullOrEmpty(orgName))
                return null;

            if (!String.IsNullOrEmpty(organizationWebServiceUri))
            {
                url = UrlSchema + organizationWebServiceUri + ":" + ProviderSettings[Constants.Port] + "/" + orgName + "/XRMServices/2011/Organization.svc";
            }
            else
            {
                url = UrlSchema + orgName + "." + ProviderSettings[Constants.IFDWebApplicationRootDomain] + ":" + ProviderSettings[Constants.Port] + "/XRMServices/2011/Organization.svc";
            }

            try
            {

                using (DiscoveryServiceProxy serviceProxy = GetDiscoveryProxy())
                {
                    // Obtain organization information from the Discovery service. 
                    if (serviceProxy != null)
                    {
                        // Obtain information about the organizations that the system user belongs to.
                        OrganizationDetailCollection orgs = DiscoverOrganizations(serviceProxy);

                        for (int n = 0; n < orgs.Count; n++)
                        {
                            if (orgs[n].UniqueName == orgName)
                            {
                                // Return the organization Uri.
                                return new System.Uri(orgs[n].Endpoints[EndpointType.OrganizationService]);
                            }
                        }

                    }
                }
            }
            catch { }

            return new Uri(url);
        }

        private int getOrganizationProxyTryCount = 10;
        private int getOrganizationProxyTryTimeout = 30000;
        protected virtual OrganizationServiceProxy GetOrganizationProxy(string orgName)
        {
            return GetOrganizationProxy(orgName, getOrganizationProxyTryCount, getOrganizationProxyTryTimeout);
        }

        protected virtual OrganizationServiceProxy GetOrganizationProxy(string orgName, int TryCount, int TryTimeout)
        {

            Uri OrganizationUri = GetCRMOrganizationUrl(orgName);

            OrganizationServiceProxy r = null;

            bool success = false;
            int tryItem = 0;
            Exception exception = null;

            while (!success)
            {

                try
                {
                    // Set IServiceManagement for the current organization.
                    IServiceManagement<IOrganizationService> orgServiceManagement =
                            ServiceConfigurationFactory.CreateManagement<IOrganizationService>(
                            OrganizationUri);

                    r = new OrganizationServiceProxy(
                        orgServiceManagement,
                        GetUserLogonCredentials());

                    success = true;

                }
                catch (Exception exc)
                {
                    Thread.Sleep(TryTimeout);
                    tryItem++;
                    if (tryItem >= TryCount)
                    {
                        exception = exc;
                        success = true;
                    }
                }

            }

            if (exception != null)
                throw new ArgumentException(exception.ToString());

            r.EnableProxyTypes();

            return r;
        }

        #endregion
        
        #region Static constructor

        static CRMBase()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveCRMAssembly;
        }

        static Assembly ResolveCRMAssembly(object sender, ResolveEventArgs args)
        {
            // Ensure we load DLLs only.
            if (args.Name.ToLower().Contains("microsoft.crm") || args.Name.ToLower().Contains("antixsslibrary") || args.Name.ToLower().Contains("microsoft.xrm"))
            {
                string dllName = args.Name.Split(',')[0] + ".dll";

                List<string> paths = new List<string>();

                // assembly location
                paths.Add( Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().EscapedCodeBase).LocalPath) );
                // crm tools path
                paths.Add(CRMPath);

                foreach(string path in paths)
                {
                    string filename = Path.Combine(path, dllName);
                    if (File.Exists(filename))
                        return Assembly.LoadFrom(filename);
                }
            }
            
            return default(Assembly);
        }

        #endregion

        #region Check environment

        protected virtual bool CheckCRMWebServicesAccess()
        {
            Log.WriteStart("CheckCRMWebServicesAccess");
            bool ret = false;
            HttpWebResponse response = null;
            HttpWebRequest request;

            try
            {
                WindowsIdentity.GetCurrent();

                request = WebRequest.Create(CRMDeploymentUrl) as HttpWebRequest;

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

        protected virtual bool CheckPermissions()
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

        protected virtual bool CheckOrganizationUnique(string databaseName, string orgName)
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
                    connection.Close();

            }

            Log.WriteEnd("CheckOrganizationUnique");
            return res;
        }

        protected virtual bool CheckSqlServerConnection()
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
                    connection.Close();
            }

            Log.WriteEnd("CheckSqlServerConnection");

            return res;
        }

        protected virtual bool CheckReportServerConnection()
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

        protected virtual OrganizationResult CheckCrmEnvironment(string strDataBaseName, string organizationUniqueName)
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

        #endregion

        #region DataBase

        protected virtual string GetDataBaseName(Guid organizationId)
        {
            string databasename = null;

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString =
                   string.Format("Server={1};Initial Catalog={0};Integrated Security=SSPI",
                                  CRMDatabaseName, SqlServer);

                connection.Open();

                string commandText = string.Format("SELECT DatabaseName FROM dbo.Organization where id = '{0}'", organizationId);
                SqlCommand command = new SqlCommand(commandText, connection);
                object result = command.ExecuteScalar();
                if (result!=null)
                    databasename = String.Concat(result.ToString());

            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }

            return databasename;
        }

        public virtual long GetDBSize(Guid organizationId)
        {
            StartLog("GetDBSize");
            long res = 0;

            string databasename = GetDataBaseName(organizationId);
            if (databasename == null) return 0;

            if (databasename == null) return 0;

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString =
                   string.Format("Server={1};Initial Catalog={0};Integrated Security=SSPI",
                                  databasename, SqlServer);

                connection.Open();

                string commandText = "SELECT ((dbsize ) * 8192 ) size FROM " + // + logsize
                    "( " +
                    "SELECT SUM(CONVERT(BIGINT,CASE WHEN status & 64 = 0 THEN size ELSE 0 END)) dbsize " +
                    ",      SUM(CONVERT(BIGINT,CASE WHEN status & 64 <> 0 THEN size ELSE 0 END)) logsize " +
                    "FROM dbo.sysfiles " +
                    ") big";

                SqlCommand command = new SqlCommand(commandText, connection);
                res = (long)command.ExecuteScalar();

            }
            catch (Exception ex)
            {
                EndLog("GetDBSize", null, null, ex);
            }
            finally
            {
                if (connection != null)
                    connection.Close();

            }

            EndLog("GetDBSize");
            return res;

        }

        public virtual long GetMaxDBSize(Guid organizationId)
        {
            StartLog("GetMaxDBSize");
            long res = 0;

            string databasename = GetDataBaseName(organizationId);
            if (databasename == null) return 0;

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString =
                   string.Format("Server={1};Initial Catalog={0};Integrated Security=SSPI",
                                  databasename, SqlServer);

                connection.Open();

                string commandText = "SELECT SUM(CONVERT(BIGINT,CASE WHEN status & 64 = 0 THEN maxsize ELSE 0 END)) dbsize FROM dbo.sysfiles";

                SqlCommand command = new SqlCommand(commandText, connection);
                res = (long)command.ExecuteScalar();
                if (res > 0) res = res * 8192;

            }
            catch (Exception ex)
            {
                EndLog("GetMaxDBSize", null, null, ex);
            }
            finally
            {
                if (connection != null)
                    connection.Close();

            }

            EndLog("GetMaxDBSize");
            return res;
        }

        public virtual ResultObject SetMaxDBSize(Guid organizationId, long maxSize)
        {
            ResultObject res = StartLog<ResultObject>("SetMaxDBSize");

            SqlConnection connection = null;
            try
            {
                string databasename = GetDataBaseName(organizationId);
                if (databasename == null) throw new Exception("Can not get database name");

                connection = new SqlConnection();
                connection.ConnectionString =
                   string.Format("Server={1};Initial Catalog={0};Integrated Security=SSPI",
                                  databasename, SqlServer);

                connection.Open();

                string maxSizeStr = maxSize == -1 ? "UNLIMITED" : (maxSize / (1024 * 1024)).ToString() + " MB";

                string commandText = "ALTER DATABASE [" + databasename + "] MODIFY FILE ( NAME = N'mscrm', MAXSIZE = " + maxSizeStr + " )";

                SqlCommand command = new SqlCommand(commandText, connection);
                command.ExecuteNonQuery();

                res.IsSuccess = true;

            }
            catch (Exception ex)
            {
                EndLog("SetMaxDBSize", res, CrmErrorCodes.CANNOT_CHANGE_CRM_ORGANIZATION_STATE, ex);
            }
            finally
            {
                if (connection != null)
                    connection.Close();

            }

            EndLog("SetMaxDBSize");
            return res;
        }

        #endregion

        #region Organization

        public virtual OrganizationResult CreateOrganization(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, 
            int baseLanguageCode,
            string ou, 
            string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, 
            string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, 
            string organizationCollation, 
            long maxSize)
        {
            return CreateOrganizationInternal(organizationId, organizationUniqueName, organizationFriendlyName, 
                baseLanguageCode, 
                ou , baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, 
                initialUserDomainName, initialUserFirstName, initialUserLastName, initialUserPrimaryEmail, 
                organizationCollation, 
                maxSize);
        }

        protected const string CRMSysAdminRoleStr = "?????????????????? ??????????????????????????;System Administrator";

        internal virtual OrganizationResult CreateOrganizationInternal(Guid organizationId, string organizationUniqueName, string organizationFriendlyName, 
            int baseLanguageCode,
            string ou, 
            string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, 
            string initialUserDomainName, string initialUserFirstName, string initialUserLastName, string initialUserPrimaryEmail, 
            string organizationCollation, 
            long maxSize)
        {
 
            OrganizationResult ret = StartLog<OrganizationResult>("CreateOrganizationInternal");

            organizationUniqueName = GetOrganizationUniqueName(organizationUniqueName);

            // calculate UserRootPath
            string ldapstr = "";

            string[] ouItems = ou.Split('.');
            foreach (string ouItem in ouItems)
            {
                if (ldapstr.Length != 0) ldapstr += ",";
                ldapstr += "OU=" + ouItem;
            }

            string rootDomain = ServerSettings.ADRootDomain;
            string[] domainItems = rootDomain.Split('.');
            foreach (string domainItem in domainItems)
                ldapstr += ",DC=" + domainItem;

            ldapstr = @"LDAP://" + rootDomain + "/" + ldapstr;

            

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

            OrganizationResult retCheckEn = CheckCrmEnvironment(CRMDatabaseName, organizationUniqueName);

            if (!retCheckEn.IsSuccess)
            {
                ret.ErrorCodes.AddRange(retCheckEn.ErrorCodes);
                EndLog("CreateOrganizationInternal", ret, null, null);
                return ret;
            }

            try
            {

                DeploymentServiceClient deploymentService = GetDeploymentProxy();

                Microsoft.Xrm.Sdk.Deployment.Organization org = new Microsoft.Xrm.Sdk.Deployment.Organization
                {
                    Id = organizationId,
                    UniqueName = organizationUniqueName,
                    FriendlyName = organizationFriendlyName,
                    SqlServerName = SqlServer,
                    SrsUrl = ReportingServer,
                    BaseCurrencyCode = baseCurrencyCode,
                    BaseCurrencyName = baseCurrencyName,
                    BaseCurrencySymbol = baseCurrencySymbol,
                    SqlCollation = organizationCollation,
                    State = Microsoft.Xrm.Sdk.Deployment.OrganizationState.Enabled
                };

                if (baseLanguageCode > 0)
                    org.BaseLanguageCode = baseLanguageCode;

                BeginCreateOrganizationRequest req = new BeginCreateOrganizationRequest
                {
                    Organization = org
                };

                if (!String.IsNullOrEmpty(UserName))
                {
                    req.SysAdminName = UserName;
                }

                BeginCreateOrganizationResponse resp = deploymentService.Execute(req) as BeginCreateOrganizationResponse;

                if (resp == null)
                    throw new ArgumentException("BeginCreateOrganizationResponse is Null");

                EntityInstanceId id = new EntityInstanceId();
                id.Name = org.UniqueName;

                Microsoft.Xrm.Sdk.Deployment.OrganizationState OperationState = Microsoft.Xrm.Sdk.Deployment.OrganizationState.Pending;

                int timeout = 30000;

                do
                {
                    Thread.Sleep(timeout);
                    try
                    {
                        Microsoft.Xrm.Sdk.Deployment.Organization getorg
                                = (Microsoft.Xrm.Sdk.Deployment.Organization)deploymentService.Retrieve(DeploymentEntityType.Organization, id);
                        OperationState = getorg.State;
                    }
                    catch { }
                } while ((OperationState != Microsoft.Xrm.Sdk.Deployment.OrganizationState.Enabled) &&
                        (OperationState != Microsoft.Xrm.Sdk.Deployment.OrganizationState.Failed));

                if (OperationState == Microsoft.Xrm.Sdk.Deployment.OrganizationState.Failed)
                    throw new ArgumentException("Create organization failed.");

                // update UserRootPath setting
                Microsoft.Xrm.Sdk.Deployment.ConfigurationEntity orgSettings = new Microsoft.Xrm.Sdk.Deployment.ConfigurationEntity
                {
                    Id = org.Id,
                    LogicalName = "Organization"
                };
                orgSettings.Attributes = new Microsoft.Xrm.Sdk.Deployment.AttributeCollection();
                orgSettings.Attributes.Add(new KeyValuePair<string, object>("UserRootPath", ldapstr));
                Microsoft.Xrm.Sdk.Deployment.UpdateAdvancedSettingsRequest reqUpdateSettings = new Microsoft.Xrm.Sdk.Deployment.UpdateAdvancedSettingsRequest
                {
                    Entity = orgSettings
                };
                Microsoft.Xrm.Sdk.Deployment.UpdateAdvancedSettingsResponse respUpdateSettings = (Microsoft.Xrm.Sdk.Deployment.UpdateAdvancedSettingsResponse) deploymentService.Execute(reqUpdateSettings);

                // DB size limit
                if (maxSize!=-1)
                    SetMaxDBSize(organizationId, maxSize);

                int tryTimeout = 30000;
                int tryCount = 10;

                bool success = false;
                int tryItem = 0;

                while (!success)
                {

                    try
                    {
                        Thread.Sleep(tryTimeout);

                        OrganizationServiceProxy _serviceProxy = GetOrganizationProxy(organizationUniqueName, 0, 0);

                        string ldap = "";

                        Guid SysAdminGuid = RetrieveSystemUser(GetDomainName(initialUserDomainName), initialUserFirstName, initialUserLastName, CRMSysAdminRoleStr, _serviceProxy, ref ldap, 0);

                        success = true;

                    }
                    catch
                    {
                        tryItem++;
                        if (tryItem >= tryCount)
                            success = true;
                    }
                }


            }
            catch (Exception ex)
            {
                EndLog("CheckCrmEnvironment", ret, CrmErrorCodes.CREATE_CRM_ORGANIZATION_GENERAL_ERROR, ex);
                return ret;

            }

            EndLog("CheckCrmEnvironment");

            return ret;
        }

        protected string GetDomainName(string username)
        {
            string domain = ActiveDirectoryUtils.GetNETBIOSDomainName(ServerSettings.ADRootDomain);
            string ret = string.Format(@"{0}\{1}", domain, username);
            return ret;
        }

        public virtual string[] GetSupportedCollationNames()
        {
            return GetSupportedCollationNamesInternal(SqlServer);
        }

        internal virtual string[] GetSupportedCollationNamesInternal(string SqlServer)
        {
            StartLog("GetSupportedCollationNamesInternal");

            List<string> ret = new List<string>();

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString =
                   string.Format("Server={1};Initial Catalog={0};Integrated Security=SSPI",
                                  CRMDatabaseName, SqlServer);

                connection.Open();

                string commandText = "select * from fn_helpcollations() where " +
                                     "(name not like '%_WS') AND (name not like '%_KS') AND (name not like '%_100_%') " +
                                     " AND (name not like 'SQL_%') " +
                                     " order by name";
                SqlCommand command = new SqlCommand(commandText, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader["name"] as string;
                    ret.Add(name);
                }

            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
            finally
            {
                if (connection != null)
                    connection.Close();

            }

            EndLog("GetSupportedCollationNamesInternal");
            return ret.ToArray();
        }

        public virtual Currency[] GetCurrencyList()
        {
            return GetCurrencyListInternal();
        }

        internal virtual Currency[] GetCurrencyListInternal()
        {
            StartLog("GetCurrencyListInternal");
            List<Currency> retList = new List<Currency>();

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);

            foreach (CultureInfo culture in cultures)
            {
                if (culture.IsNeutralCulture) continue;

                try
                {
                    RegionInfo Region = new RegionInfo(culture.LCID);

                    Currency currency = new Currency();
                    currency.RegionName = Region.NativeName;
                    currency.CurrencyName = Region.CurrencyNativeName;
                    currency.CurrencyCode = Region.ISOCurrencySymbol;
                    currency.CurrencySymbol = Region.CurrencySymbol;
                    retList.Add(currency);

                }
                catch
                {
                    continue;
                }
            }

            retList.Sort(delegate(Currency a, Currency b) { return a.RegionName.CompareTo(b.RegionName); });

            EndLog("GetCurrencyListInternal");
            return retList.ToArray();
        }

        public virtual int[] GetInstalledLanguagePacks()
        {
            return GetInstalledLanguagePacksInternal();
        }

        internal virtual int[] GetInstalledLanguagePacksInternal()
        {
            StartLog("GetInstalledLanguagePacks");
            List<int> res = new List<int>();

            try
            {
                RegistryKey root = Registry.LocalMachine;
                RegistryKey rk = root.OpenSubKey("SOFTWARE\\Microsoft\\MSCRM");

                if (rk == null)
                    rk = root.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\MSCRM");

                if (rk == null) throw new Exception("Can't open SOFTWARE\\Microsoft\\MSCRM");

                RegistryKey langPacksKey = rk.OpenSubKey("LangPacks");
                if (langPacksKey == null) throw new Exception("Can't open SOFTWARE\\Microsoft\\MSCRM\\LangPacks");

                string[] langPacksId = langPacksKey.GetSubKeyNames();

                foreach (string strLangId in langPacksId)
                {
                    int langId = 0;
                    if (int.TryParse(strLangId, out langId))
                        res.Add(langId);
                }

            }
            catch (Exception ex)
            {
                EndLog("GetInstalledLanguagePacks", null, null, ex);
                return null;
            }

            EndLog("GetInstalledLanguagePacks");
            return res.ToArray();
        }

        public virtual ResultObject DeleteOrganization(Guid orgId)
        {
            return DeleteOrganizationInternal(orgId);
        }

        internal virtual ResultObject DeleteOrganizationInternal(Guid orgId)
        {
            ResultObject res = StartLog<ResultObject>("DeleteOrganizationInternal");


            res.IsSuccess = true;
            try
            {
                DeploymentServiceClient deploymentService = GetDeploymentProxy();

                EntityInstanceId i = new EntityInstanceId();
                i.Id = orgId; //Organisation Id

                Microsoft.Xrm.Sdk.Deployment.Organization org = (Microsoft.Xrm.Sdk.Deployment.Organization)deploymentService.Retrieve(DeploymentEntityType.Organization, i);

                org.State = Microsoft.Xrm.Sdk.Deployment.OrganizationState.Disabled;

                Microsoft.Xrm.Sdk.Deployment.UpdateRequest updateRequest = new Microsoft.Xrm.Sdk.Deployment.UpdateRequest();
                updateRequest.Entity = org;

                deploymentService.Execute(updateRequest);

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

        #endregion

        #region Log

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

        private static void StartLog(string message)
        {
            HostedSolutionLog.LogStart(message);
        }

        #endregion

        #region User

        public virtual UserResult CreateCRMUser(OrganizationUser user, string orgName, Guid organizationId, Guid baseUnitId, int CALType)
        {
            return CreateCRMUserInternal(user, orgName, organizationId, baseUnitId, CALType);
        }

        internal virtual UserResult CreateCRMUserInternal(OrganizationUser user, string orgName, Guid organizationId, Guid businessUnitId, int CALType)
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
                    OrganizationServiceProxy _serviceProxy = GetOrganizationProxy(orgName);


                    string ldap = "";
                    Guid guid = RetrieveSystemUser(user.DomainUserName, user.FirstName, user.LastName, CRMSysAdminRoleStr, _serviceProxy, ref ldap, CALType);

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

        protected virtual Guid CreateSystemUser(String userName, String firstName,
            String lastName, String domain, String roleStr,
            OrganizationServiceProxy serviceProxy, ref String ldapPath, int CALType)
        {

            if (serviceProxy.ServiceConfiguration.AuthenticationType == AuthenticationProviderType.LiveId ||
                serviceProxy.ServiceConfiguration.AuthenticationType == AuthenticationProviderType.OnlineFederation)
                throw new Exception(String.Format("To run this sample, {0} {1} must be an active system user in your Microsoft Dynamics CRM Online organization.", firstName, lastName));

            Guid userId = Guid.Empty;

            Microsoft.Xrm.Sdk.Query.QueryExpression businessUnitQuery = new Microsoft.Xrm.Sdk.Query.QueryExpression
            {
                EntityName = BusinessUnit.EntityLogicalName,
                ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("businessunitid"),
                Criteria =
                {
                    Conditions =
                    {
                        new Microsoft.Xrm.Sdk.Query.ConditionExpression("parentbusinessunitid", 
                            Microsoft.Xrm.Sdk.Query.ConditionOperator.Null)
                    }
                }
            };

            BusinessUnit defaultBusinessUnit = serviceProxy.RetrieveMultiple(
                businessUnitQuery).Entities[0].ToEntity<BusinessUnit>();

            // Retrieve the specified security role.
            Role role = RetrieveRoleByName(serviceProxy, roleStr);

            // CALType and AccessMode
            int accessmode = CALType / 10;
            int caltype = CALType % 10;

            //Create a new system user.
            SystemUser user = new SystemUser
            {
                DomainName = userName,
                FirstName = firstName,
                LastName = lastName,
                BusinessUnitId = new EntityReference
                {
                    LogicalName = BusinessUnit.EntityLogicalName,
                    Name = BusinessUnit.EntityLogicalName,
                    Id = defaultBusinessUnit.Id
                },
                CALType = new OptionSetValue(caltype),
                AccessMode = new OptionSetValue(accessmode)
            };
            userId = serviceProxy.Create(user);

            // Assign the security role to the newly created Microsoft Dynamics CRM user.
            AssociateRequest associate = new AssociateRequest()
            {
                Target = new EntityReference(SystemUser.EntityLogicalName, userId),
                RelatedEntities = new EntityReferenceCollection()
                {
                    new EntityReference(Role.EntityLogicalName, role.Id),
                },
                Relationship = new Relationship("systemuserroles_association")
            };
            serviceProxy.Execute(associate);

            return userId;
        }


        protected virtual Guid RetrieveSystemUser(String userName, String firstName,
            String lastName, String roleStr, OrganizationServiceProxy serviceProxy,
            ref String ldapPath,
            int CALType)
        {
            String domain;
            Guid userId = Guid.Empty;

            if (serviceProxy == null)
                throw new ArgumentNullException("serviceProxy");

            if (String.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("UserName");

            if (String.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException("FirstName");

            if (String.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException("LastName");

            if (String.IsNullOrWhiteSpace(roleStr))
                throw new ArgumentNullException("Role");

            // Obtain the current user's information.
            Microsoft.Crm.Sdk.Messages.WhoAmIRequest who = new Microsoft.Crm.Sdk.Messages.WhoAmIRequest();
            Microsoft.Crm.Sdk.Messages.WhoAmIResponse whoResp = (Microsoft.Crm.Sdk.Messages.WhoAmIResponse)serviceProxy.Execute(who);
            Guid currentUserId = whoResp.UserId;

            SystemUser currentUser =
                serviceProxy.Retrieve(SystemUser.EntityLogicalName, currentUserId, new Microsoft.Xrm.Sdk.Query.ColumnSet("domainname")).ToEntity<SystemUser>();

            // Extract the domain and create the LDAP object.
            String[] userPath = currentUser.DomainName.Split(new char[] { '\\' });
            if (userPath.Length > 1)
                domain = userPath[0] + "\\";
            else
                domain = String.Empty;

            // Create the system user in Microsoft Dynamics CRM if the user doesn't 
            // already exist.
            Microsoft.Xrm.Sdk.Query.QueryExpression userQuery = new Microsoft.Xrm.Sdk.Query.QueryExpression
            {
                EntityName = SystemUser.EntityLogicalName,
                ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("systemuserid"),
                Criteria =
                {
                    FilterOperator = Microsoft.Xrm.Sdk.Query.LogicalOperator.Or,
                    Filters =
                    {   
                        new Microsoft.Xrm.Sdk.Query.FilterExpression
                        {
                            FilterOperator = Microsoft.Xrm.Sdk.Query.LogicalOperator.And,
                            Conditions =
                            {
                                new Microsoft.Xrm.Sdk.Query.ConditionExpression("domainname", Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, domain + userName)
                            }
                        },
                        new Microsoft.Xrm.Sdk.Query.FilterExpression
                        {
                            FilterOperator = Microsoft.Xrm.Sdk.Query.LogicalOperator.And,
                            Conditions = 
                            {
                                new Microsoft.Xrm.Sdk.Query.ConditionExpression("firstname", Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, firstName),
                                new Microsoft.Xrm.Sdk.Query.ConditionExpression("lastname", Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, lastName)
                            }
                        }
                    }

                }
            };

            DataCollection<Entity> existingUsers = (DataCollection<Entity>)serviceProxy.RetrieveMultiple(userQuery).Entities;

            SystemUser existingUser = null;
            if (existingUsers.Count > 0)
                existingUser = existingUsers[0].ToEntity<SystemUser>();

            if (existingUser != null)
            {
                userId = existingUser.SystemUserId.Value;

                // Check to make sure the user is assigned the correct role.
                Role role = RetrieveRoleByName(serviceProxy, roleStr);

                // Associate the user with the role when needed.
                if (!UserInRole(serviceProxy, userId, role.Id))
                {
                    AssociateRequest associate = new AssociateRequest()
                    {
                        Target = new EntityReference(SystemUser.EntityLogicalName, userId),
                        RelatedEntities = new EntityReferenceCollection()
                        {
                            new EntityReference(Role.EntityLogicalName, role.Id)
                        },
                        Relationship = new Relationship("systemuserroles_association")
                    };
                    serviceProxy.Execute(associate);
                }

            }
            else
            {
                userId = CreateSystemUser(userName, firstName, lastName, domain,
                    roleStr, serviceProxy, ref ldapPath, CALType);
            }

            return userId;
        }

        public virtual CrmUserResult GetCrmUserById(Guid crmUserId, string orgName)
        {
            return GetCrmUserByIdInternal(crmUserId, orgName);
        }

        internal virtual CrmUserResult GetCrmUserByIdInternal(Guid crmUserId, string orgName)
        {
            CrmUserResult ret = StartLog<CrmUserResult>("GetCrmUserByIdInternal");

            try
            {
                if (crmUserId == Guid.Empty)
                    throw new ArgumentNullException("crmUserId");

                if (string.IsNullOrEmpty(orgName))
                    throw new ArgumentNullException("orgName");

                OrganizationServiceProxy serviceProxy = GetOrganizationProxy(orgName);

                SystemUser retruveUser =
                    serviceProxy.Retrieve(SystemUser.EntityLogicalName, crmUserId, new Microsoft.Xrm.Sdk.Query.ColumnSet("domainname", "businessunitid", "accessmode", "isdisabled", "caltype")).ToEntity<SystemUser>();

                CrmUser user = null;

                if (retruveUser != null)
                {
                    user = new CrmUser();
                    user.BusinessUnitId = retruveUser.BusinessUnitId.Id;
                    user.CRMUserId = retruveUser.SystemUserId.Value;
                    user.ClientAccessMode = (CRMUserAccessMode)retruveUser.AccessMode.Value;
                    user.IsDisabled = (bool)retruveUser.IsDisabled;
                    user.CALType = retruveUser.CALType.Value;

                    ret.Value = user;
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

        internal virtual CrmUserResult GetCrmUserByDomainNameInternal(string domainName, string orgName)
        {
            CrmUserResult ret = StartLog<CrmUserResult>("GetCrmUserByDomainNameInternal");

            try
            {
                if (string.IsNullOrEmpty(domainName))
                    throw new ArgumentNullException("domainName");

                if (string.IsNullOrEmpty(orgName))
                    throw new ArgumentNullException("orgName");


                OrganizationServiceProxy serviceProxy = GetOrganizationProxy(orgName);

                Microsoft.Xrm.Sdk.Query.QueryExpression usereQuery = new Microsoft.Xrm.Sdk.Query.QueryExpression
                {
                    EntityName = SystemUser.EntityLogicalName,
                    ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("domainname", "businessunitid", "accessmode", "isdisabled", "systemuserid", "caltype"),
                };

                EntityCollection users = serviceProxy.RetrieveMultiple(usereQuery);

                foreach (Entity entityuser in users.Entities)
                {
                    SystemUser sysuser = entityuser.ToEntity<SystemUser>();

                    if (sysuser == null) continue;
                    if (sysuser.DomainName != domainName) continue;

                    CrmUser user = new CrmUser();
                    user.BusinessUnitId = sysuser.BusinessUnitId.Id;
                    user.CRMUserId = sysuser.SystemUserId.Value;
                    user.ClientAccessMode = (CRMUserAccessMode)sysuser.AccessMode.Value;
                    user.IsDisabled = sysuser.IsDisabled.Value;
                    user.CALType = sysuser.CALType.Value;
                    ret.Value = user;
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

        public virtual CrmUserResult GetCrmUserByDomainName(string domainName, string orgName)
        {
            return GetCrmUserByDomainNameInternal(domainName, orgName);
        }

        public virtual ResultObject ChangeUserState(bool disable, string orgName, Guid crmUserId)
        {
            return ChangeUserStateInternal(disable, orgName, crmUserId);
        }

        internal virtual ResultObject ChangeUserStateInternal(bool disable, string orgName, Guid crmUserId)
        {
            ResultObject res = StartLog<ResultObject>("ChangeUserStateInternal");

            res.IsSuccess = true;
            try
            {
                if (crmUserId == Guid.Empty)
                    throw new ArgumentNullException("crmUserId");

                if (string.IsNullOrEmpty(orgName))
                    throw new ArgumentNullException("orgName");

                OrganizationServiceProxy serviceProxy = GetOrganizationProxy(orgName);

                // Retrieve a user.
                SystemUser user = serviceProxy.Retrieve(SystemUser.EntityLogicalName,
                    crmUserId, new Microsoft.Xrm.Sdk.Query.ColumnSet(new String[] { "systemuserid", "firstname", "lastname" })).ToEntity<SystemUser>();

                if (user != null)
                {
                    Microsoft.Crm.Sdk.Messages.SetStateRequest request = new Microsoft.Crm.Sdk.Messages.SetStateRequest()
                    {
                        EntityMoniker = user.ToEntityReference(),

                        // Required by request but always valued at -1 in this context.
                        Status = new OptionSetValue(-1),

                        // Sets the user to disabled.
                        State = disable ? new OptionSetValue(-1) : new OptionSetValue(0)
                    };

                    serviceProxy.Execute(request);

                }
            }
            catch (Exception ex)
            {
                EndLog("ChangeUserStateInternal", res, CrmErrorCodes.CANNOT_CHANGE_USER_STATE, ex);
                return res;
            }


            EndLog("ChangeUserStateInternal");
            return res;
        }

        public virtual ResultObject SetUserCALType(string orgName, Guid userId, int CALType)
        {
            return SetUserCALTypeInternal(orgName, userId, CALType);
        }

        internal virtual ResultObject SetUserCALTypeInternal(string orgName, Guid userId, int CALType)
        {
            ResultObject ret = StartLog<CrmUserResult>("SetUserCALTypeInternal");

            try
            {
                if (userId == Guid.Empty)
                    throw new ArgumentNullException("crmUserId");

                if (string.IsNullOrEmpty(orgName))
                    throw new ArgumentNullException("orgName");

                OrganizationServiceProxy serviceProxy = GetOrganizationProxy(orgName);

                SystemUser user =
                    serviceProxy.Retrieve(SystemUser.EntityLogicalName, userId, new Microsoft.Xrm.Sdk.Query.ColumnSet("domainname", "businessunitid", "accessmode", "isdisabled", "caltype")).ToEntity<SystemUser>();

                // CALType and AccessMode
                int accessmode = CALType / 10;
                int caltype = CALType % 10;

                user.CALType = new OptionSetValue(caltype);
                user.AccessMode = new OptionSetValue(accessmode);

                serviceProxy.Update(user);

            }
            catch (Exception ex)
            {
                EndLog("SetUserCALTypeInternal", ret, CrmErrorCodes.CANONT_GET_CRM_USER_BY_ID, ex);
                return ret;
            }

            EndLog("SetUserCALTypeInternal");
            return ret;
        }

        #endregion

        #region Role

        protected virtual Role RetrieveRoleByName(OrganizationServiceProxy serviceProxy,
            String roleSplitStr)
        {
            string[] RolesStr = roleSplitStr.Split(';');

            foreach (string roleStr in RolesStr)
            {

                Microsoft.Xrm.Sdk.Query.QueryExpression roleQuery = new Microsoft.Xrm.Sdk.Query.QueryExpression
                {
                    EntityName = Role.EntityLogicalName,
                    ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("roleid"),
                    Criteria =
                    {
                        Conditions =
                {
                    new Microsoft.Xrm.Sdk.Query.ConditionExpression("name", Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, roleStr)
                }
                    }
                };

                DataCollection<Entity> roles = serviceProxy.RetrieveMultiple(roleQuery).Entities;

                if (roles.Count > 0) return roles[0].ToEntity<Role>();
            }

            return null;
        }

        protected virtual bool UserInRole(OrganizationServiceProxy serviceProxy,
            Guid userId, Guid roleId)
        {
            // Establish a SystemUser link for a query.
            Microsoft.Xrm.Sdk.Query.LinkEntity systemUserLink = new Microsoft.Xrm.Sdk.Query.LinkEntity()
            {
                LinkFromEntityName = SystemUserRoles.EntityLogicalName,
                LinkFromAttributeName = "systemuserid",
                LinkToEntityName = SystemUser.EntityLogicalName,
                LinkToAttributeName = "systemuserid",
                LinkCriteria =
                {
                    Conditions = 
                    {
                        new Microsoft.Xrm.Sdk.Query.ConditionExpression(
                            "systemuserid", Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, userId)
                    }
                }
            };

            // Build the query.
            Microsoft.Xrm.Sdk.Query.QueryExpression query = new Microsoft.Xrm.Sdk.Query.QueryExpression()
            {
                EntityName = Role.EntityLogicalName,
                ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("roleid"),
                LinkEntities = 
                {
                    new Microsoft.Xrm.Sdk.Query.LinkEntity()
                    {
                        LinkFromEntityName = Role.EntityLogicalName,
                        LinkFromAttributeName = "roleid",
                        LinkToEntityName = SystemUserRoles.EntityLogicalName,
                        LinkToAttributeName = "roleid",
                        LinkEntities = {systemUserLink}
                    }
                },
                Criteria =
                {
                    Conditions = 
                    {
                        new Microsoft.Xrm.Sdk.Query.ConditionExpression("roleid", Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, roleId)
                    }
                }
            };

            // Retrieve matching roles.
            EntityCollection ec = serviceProxy.RetrieveMultiple(query);

            if (ec.Entities.Count > 0)
                return true;

            return false;
        }

        public virtual CrmRolesResult GetAllCrmRoles(string orgName, Guid businessUnitId)
        {
            return GetAllCrmRolesInternal(orgName, businessUnitId);
        }

        public virtual CrmRolesResult GetCrmUserRoles(string orgName, Guid userId)
        {
            return GetCrmUserRolesInternal(userId, orgName);
        }

        public virtual EntityCollection GetUserRoles(Guid userId, string orgName)
        {
            OrganizationServiceProxy serviceProxy = GetOrganizationProxy(orgName);

            // Establish a SystemUser link for a query.
            Microsoft.Xrm.Sdk.Query.LinkEntity systemUserLink = new Microsoft.Xrm.Sdk.Query.LinkEntity()
            {
                LinkFromEntityName = SystemUserRoles.EntityLogicalName,
                LinkFromAttributeName = "systemuserid",
                LinkToEntityName = SystemUser.EntityLogicalName,
                LinkToAttributeName = "systemuserid",
                LinkCriteria =
                {
                    Conditions = 
                    {
                        new Microsoft.Xrm.Sdk.Query.ConditionExpression(
                            "systemuserid", Microsoft.Xrm.Sdk.Query.ConditionOperator.Equal, userId)
                    }
                }
            };

            // Build the query.
            Microsoft.Xrm.Sdk.Query.QueryExpression query = new Microsoft.Xrm.Sdk.Query.QueryExpression()
            {
                EntityName = Role.EntityLogicalName,
                ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("roleid"),
                LinkEntities = 
                        {
                            new Microsoft.Xrm.Sdk.Query.LinkEntity()
                            {
                                LinkFromEntityName = Role.EntityLogicalName,
                                LinkFromAttributeName = "roleid",
                                LinkToEntityName = SystemUserRoles.EntityLogicalName,
                                LinkToAttributeName = "roleid",
                                LinkEntities = {systemUserLink}
                            }
                        }
            };

            // Retrieve matching roles.
            EntityCollection relations = serviceProxy.RetrieveMultiple(query);

            return relations;
        }

        internal virtual CrmRolesResult GetCrmUserRolesInternal(Guid userId, string orgName)
        {
            CrmRolesResult res = StartLog<CrmRolesResult>("GetCrmUserRolesInternal");

            try
            {
                EntityCollection relations;

                if (userId == Guid.Empty)
                    throw new ArgumentException("userId");

                if (string.IsNullOrEmpty(orgName))
                    throw new ArgumentNullException("orgName");

                try
                {
                    relations = GetUserRoles(userId, orgName);
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

        protected static string excludedRolesStr = ";???????????????????????? ??????????????????;support user;";

        protected virtual List<CrmRole> FillCrmRoles(EntityCollection entities, bool isUserRole, Guid businessUnitId)
        {
            List<CrmRole> res = new List<CrmRole>();

            foreach (Entity current in entities.Entities)
            {
                Role role = current.ToEntity<Role>();

                if (role == null) continue;

                if (businessUnitId != Guid.Empty)
                {
                    if (businessUnitId != role.BusinessUnitId.Id)
                        continue;
                }

                string roleName = role.Name;

                if (roleName != null)
                    if (excludedRolesStr.IndexOf(";" + roleName.ToLower() + ";") != -1)
                        continue;

                CrmRole crmRole = new CrmRole();
                crmRole.IsCurrentUserRole = isUserRole;
                crmRole.RoleId = (Guid)role.RoleId;
                crmRole.RoleName = roleName;

                res.Add(crmRole);
            }

            return res;
        }

        protected virtual List<CrmRole> FillCrmRoles(EntityCollection entities, Guid businessUnitId)
        {
            return FillCrmRoles(entities, false, businessUnitId);
        }

        internal virtual CrmRolesResult GetAllCrmRolesInternal(string orgName, Guid businessUnitId)
        {
            CrmRolesResult res = StartLog<CrmRolesResult>("GetAllCrmRoles");

            try
            {
                if (string.IsNullOrEmpty(orgName))
                    throw new ArgumentNullException("orgName");

                EntityCollection roles;
                try
                {
                    OrganizationServiceProxy serviceProxy = GetOrganizationProxy(orgName);

                    Microsoft.Xrm.Sdk.Query.QueryExpression roleQuery = new Microsoft.Xrm.Sdk.Query.QueryExpression
                    {
                        EntityName = Role.EntityLogicalName,
                        ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(new string[] { "roleid", "name", "businessunitid" }),
                    };

                    roles = serviceProxy.RetrieveMultiple(roleQuery);


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

        public virtual ResultObject SetUserRoles(string orgName, Guid userId, Guid[] roles)
        {
            return SetUserRolesInternal(orgName, userId, roles);
        }

        internal virtual ResultObject SetUserRolesInternal(string orgName, Guid userId, Guid[] roles)
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

                OrganizationServiceProxy serviceProxy = GetOrganizationProxy(orgName);


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
                    DisassociateRequest removeRole = new DisassociateRequest()
                    {
                        Target = new EntityReference(SystemUser.EntityLogicalName, userId),
                        RelatedEntities = new EntityReferenceCollection(),
                        Relationship = new Relationship("systemuserroles_association")
                    };

                    for (int i = 0; i < remRoles.Count; i++)
                        removeRole.RelatedEntities.Add(new EntityReference(Role.EntityLogicalName, remRoles[i]));

                    serviceProxy.Execute(removeRole);

                }
                catch (Exception ex)
                {
                    EndLog("SetUserRolesInternal", res, CrmErrorCodes.CANNOT_REMOVE_CRM_USER_ROLES, ex);
                    return res;
                }


                try
                {
                    // Assign the security role to the newly created Microsoft Dynamics CRM user.
                    AssociateRequest associate = new AssociateRequest()
                    {
                        Target = new EntityReference(SystemUser.EntityLogicalName, userId),
                        RelatedEntities = new EntityReferenceCollection(),
                        Relationship = new Relationship("systemuserroles_association")
                    };

                    for (int i = 0; i < roles.Length; i++)
                    {
                        bool find = false;
                        foreach (CrmRole current in tmpRoles.Value)
                        {
                            if (current.RoleId == roles[i])
                                find = true;
                        }
                        if (find) continue;

                        associate.RelatedEntities.Add(new EntityReference(Role.EntityLogicalName, roles[i]));
                    }

                    serviceProxy.Execute(associate);
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

        #endregion

        #region Business Units

        internal virtual CRMBusinessUnitsResult GetOrganizationBusinessUnitsInternal(Guid organizationId, string orgName)
        {
            CRMBusinessUnitsResult res = StartLog<CRMBusinessUnitsResult>("GetOrganizationBusinessUnitsInternal");

            try
            {
                if (organizationId == Guid.Empty)
                    throw new ArgumentException("organizationId");

                if (string.IsNullOrEmpty(orgName))
                    throw new ArgumentNullException("orgName");

                OrganizationServiceProxy serviceProxy;

                try
                {
                    serviceProxy = GetOrganizationProxy(orgName);
                }
                catch (Exception ex)
                {
                    EndLog("GetOrganizationBusinessUnitsInternal", res, CrmErrorCodes.CANNOT_GET_CRM_SERVICE, ex);
                    return res;
                }

                DataCollection<Entity> BusinessUnits;

                try
                {

                    Microsoft.Xrm.Sdk.Query.QueryExpression businessUnitQuery = new Microsoft.Xrm.Sdk.Query.QueryExpression
                    {
                        EntityName = BusinessUnit.EntityLogicalName,
                        ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(new string[] { "businessunitid", "name" }),
                        Criteria =
                        {
                            Conditions =
                            {
                                new Microsoft.Xrm.Sdk.Query.ConditionExpression("parentbusinessunitid", 
                                    Microsoft.Xrm.Sdk.Query.ConditionOperator.Null)
                            }
                        }
                    };

                    BusinessUnits = serviceProxy.RetrieveMultiple(
                        businessUnitQuery).Entities;

                }
                catch (Exception ex)
                {
                    EndLog("GetOrganizationBusinessUnitsInternal", res, CrmErrorCodes.CANNOT_GET_CRM_BUSINESS_UNITS, ex);
                    return res;
                }

                List<CRMBusinessUnit> businessUnits = new List<CRMBusinessUnit>();

                try
                {
                    for (int i = 0; i < BusinessUnits.Count; i++)
                    {
                        BusinessUnit bu = BusinessUnits[i].ToEntity<BusinessUnit>();

                        CRMBusinessUnit unit = new CRMBusinessUnit();
                        unit.BusinessUnitId = (Guid)bu.BusinessUnitId;
                        unit.BusinessUnitName = bu.Name;

                        if (unit.BusinessUnitName == null)
                            unit.BusinessUnitName = "default";

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

        public virtual CRMBusinessUnitsResult GetOrganizationBusinessUnits(Guid organizationId, string orgName)
        {
            return GetOrganizationBusinessUnitsInternal(organizationId, orgName);
        }

        #endregion

        #region Version

        public virtual string CRMServerVersion
        {
            get
            {
                string value = "";
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

                return value;
            }

        }

        public override bool IsInstalled()
        {
            return false;
        }

        #endregion

    }
}
