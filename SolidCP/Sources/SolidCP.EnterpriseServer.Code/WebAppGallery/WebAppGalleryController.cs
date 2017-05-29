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

﻿using System;
using System.Collections.Generic;
using System.Threading;
﻿using SolidCP.EnterpriseServer.Base.Common;
﻿using SolidCP.Providers.Web;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Database;
using SolidCP.Providers.WebAppGallery;
using System.Collections.Specialized;
using SolidCP.Providers.Common;
using System.Diagnostics;

namespace SolidCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for ConfigSettings.
	/// </summary>
	public class WebAppGalleryController
	{
		#region Constants
		public const string NO_DB_PARAMETER_MATCHES_MSG = "Could not match the parameter by either the following {0} {1}. Please check parameters.xml file within the application package.";
		public const string PARAMETER_IS_NULL_OR_EMPTY = "{0} parameter is either not set or empty";
		public const string CANNOT_SET_PARAMETER_VALUE = "Parameter '{0}' has an empty value. Please check the database service provider configuration.\r\n" +
			"- For Microsoft SQL database server ensure you do not use Trusted connection option.\r\n" +
			"- For MySQL database server ensure database administrator credentials are set.";
		public const string TAGS_MATCH = "tags";
		public const string NAMES_MATCH = "names";
		public const string TASK_MANAGER_SOURCE = "WAG_INSTALLER";
		public const string GET_APP_PARAMS_TASK = "GET_APP_PARAMS_TASK";
		public const string GET_GALLERY_APPS_TASK = "GET_GALLERY_APPS_TASK";
		public const string GET_SRV_GALLERY_APPS_TASK = "GET_SRV_GALLERY_APPS_TASK";
		public const string GET_GALLERY_APP_DETAILS_TASK = "GET_GALLERY_APP_DETAILS_TASK";
		public const string GET_GALLERY_CATEGORIES_TASK = "GET_GALLERY_CATEGORIES_TASK";
		#endregion

        private static string[] getFeedsFromSettings(int packageId)
        {
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);

            return getFeedsFromSettingsByServiceId(serviceId);

        }

        private static string[] getFeedsFromSettingsByServiceId(int serviceId)
        {
            var wpiSettings = SystemController.GetSystemSettingsInternal(SystemSettings.WPI_SETTINGS,false);

            List<string> feeds = new List<string>();

            // Microsoft feed
            string mainFeedUrl = wpiSettings[SystemSettings.WPI_MAIN_FEED_KEY];
            if (string.IsNullOrEmpty(mainFeedUrl))
            {
                mainFeedUrl = WebPlatformInstaller.MAIN_FEED_URL;
            }
            feeds.Add(mainFeedUrl);

            // Zoo Feed
            feeds.Add(WebPlatformInstaller.ZOO_FEED);


            // additional feeds
            string additionalFeeds = wpiSettings[SystemSettings.FEED_ULS_KEY];
            if (!string.IsNullOrEmpty(additionalFeeds))
            {
                feeds.AddRange(additionalFeeds.Split(';'));
            }

            return feeds.ToArray();
        }


        public static void InitFeedsByServiceId(int UserId, int serviceId)
        {
            string[] feeds = getFeedsFromSettingsByServiceId(serviceId);

            WebServer webServer = WebServerController.GetWebServer(serviceId);
            webServer.InitFeeds(UserId, feeds);
        }
        


        public static void InitFeeds(int UserId, int packageId)
        {
            string[] feeds = getFeedsFromSettings(packageId);
            
            // Set feeds
            WebServer webServer = GetAssociatedWebServer(packageId);
            webServer.InitFeeds(UserId, feeds);

        }

        public static void SetResourceLanguage(int packageId, string resourceLanguage)
        {
            GetAssociatedWebServer(packageId).SetResourceLanguage(SecurityContext.User.UserId,resourceLanguage);
        }

      
        public static GalleryLanguagesResult GetGalleryLanguages(int packageId)
        {
            GalleryLanguagesResult result;

            try
            {
                WebServer webServer = GetAssociatedWebServer(packageId);
                result = webServer.GetGalleryLanguages(SecurityContext.User.UserId);

                if (!result.IsSuccess)
                    return Error<GalleryLanguagesResult>(result, GalleryErrors.GetLanguagesError);
            }
            catch (Exception ex)
            {
                return Error<GalleryLanguagesResult>(GalleryErrors.GetLanguagesError, ex.Message);
            }
            finally
            {
            }
            //
            return result;

        }

		public static GalleryCategoriesResult GetGalleryCategories(int packageId)
		{
			GalleryCategoriesResult result;
			//
			try
			{
				TaskManager.StartTask(TASK_MANAGER_SOURCE, GET_GALLERY_CATEGORIES_TASK);
				
                // check if Web App Gallery is installed
				WebServer webServer = GetAssociatedWebServer(packageId);

                if (!webServer.IsMsDeployInstalled())
                {
                    TaskManager.WriteError("MsDeploy is not installed");
                    return Error<GalleryCategoriesResult>(GalleryErrors.MsDeployIsNotInstalled);
                }

                // get categories
                result = webServer.GetGalleryCategories(SecurityContext.User.UserId);
				
				if (!result.IsSuccess)
                    return Error<GalleryCategoriesResult>(result, GalleryErrors.GetCategoriesError);
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
                return Error<GalleryCategoriesResult>(GalleryErrors.GeneralError, ex.Message);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static GalleryApplicationsResult GetGalleryApplicationsByServiceId(int serviceId)
		{
			GalleryApplicationsResult result;
			//
			try
			{
				TaskManager.StartTask(TASK_MANAGER_SOURCE, GET_SRV_GALLERY_APPS_TASK);
				
                int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin);
                if (accountCheck < 0)
                    return Warning<GalleryApplicationsResult>((-accountCheck).ToString());
				
                // check if WAG is installed
				WebServer webServer = WebServerController.GetWebServer(serviceId);
				
				if (!webServer.IsMsDeployInstalled())
                    return Error<GalleryApplicationsResult>(GalleryErrors.MsDeployIsNotInstalled);

				// get applications
                result = webServer.GetGalleryApplications(SecurityContext.User.UserId,String.Empty);
				
				if (!result.IsSuccess)
					return Error<GalleryApplicationsResult>(result, GalleryErrors.GetApplicationsError);
			}
			catch (Exception ex)
			{
                TaskManager.WriteError(ex);
                return Error<GalleryApplicationsResult>(GalleryErrors.GeneralError, ex.Message);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

        public static GalleryApplicationsResult GetGalleryApplications(int packageId, string categoryId)
		{
			GalleryApplicationsResult result;
			//
			try
			{
				TaskManager.StartTask(TASK_MANAGER_SOURCE, GET_GALLERY_APPS_TASK);
				
                // check if WAG is installed
                WebServer webServer = GetAssociatedWebServer(packageId);
                if (!webServer.IsMsDeployInstalled())
                    return Error<GalleryApplicationsResult>(GalleryErrors.MsDeployIsNotInstalled);

				// get applications
                result = webServer.GetGalleryApplications(SecurityContext.User.UserId,categoryId);

                if (!result.IsSuccess)
                    return Error<GalleryApplicationsResult>(result, GalleryErrors.GetApplicationsError);

				// get space quotas
				PackageContext context = PackageController.GetPackageContext(packageId);

                //// filter applications
                //List<string> appsFilter = new List<string>();
                //// if either ASP.NET 1.1 or 2.0 enabled in the hosting plan
                //if (context.Quotas[Quotas.WEB_ASPNET11].QuotaAllocatedValue == 1 ||
                //    context.Quotas[Quotas.WEB_ASPNET20].QuotaAllocatedValue == 1 ||
                //    context.Quotas[Quotas.WEB_ASPNET40].QuotaAllocatedValue == 1)
                //{
                //    appsFilter.AddRange(SupportedAppDependencies.ASPNET_SCRIPTING);
                //}
                //// if either PHP 4 or 5 enabled in the hosting plan
                //if (context.Quotas[Quotas.WEB_PHP4].QuotaAllocatedValue == 1 ||
                //    context.Quotas[Quotas.WEB_PHP5].QuotaAllocatedValue == 1)
                //{
                //    appsFilter.AddRange(SupportedAppDependencies.PHP_SCRIPTING);
                //}
                //// if either MSSQL 2000, 2005, 2008 or 2012 enabled in the hosting plan
                //if (context.Groups.ContainsKey(ResourceGroups.MsSql2000) ||
                //    context.Groups.ContainsKey(ResourceGroups.MsSql2005) ||
                //    context.Groups.ContainsKey(ResourceGroups.MsSql2008) ||
                //    context.Groups.ContainsKey(ResourceGroups.MsSql2012) ||
                //    context.Groups.ContainsKey(ResourceGroups.MsSql2014))
                //{
                //    appsFilter.AddRange(SupportedAppDependencies.MSSQL_DATABASE);
                //}
                //// if either MySQL 4 or 5 enabled in the hosting plan
                //if (context.Groups.ContainsKey(ResourceGroups.MySql4) ||
                //    context.Groups.ContainsKey(ResourceGroups.MySql5))
                //{
                //    appsFilter.AddRange(SupportedAppDependencies.MYSQL_DATABASE);
                //}
                //// Match applications based on the hosting plan restrictions collected
                //result.Value = new List<GalleryApplication>(Array.FindAll<GalleryApplication>(result.Value.ToArray(),
                //    x => MatchGalleryAppDependencies(x.Dependency, appsFilter.ToArray())
                //        || MatchMenaltoGalleryApp(x, appsFilter.ToArray())));


				{
                    FilterResultApplications(packageId, result);

				}
			}
			catch (Exception ex)
			{
                TaskManager.WriteError(ex);
                return Error<GalleryApplicationsResult>(GalleryErrors.GeneralError, ex.Message);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

        private static void FilterResultApplications(int packageId, GalleryApplicationsResult result)
        {
            int userId = SecurityContext.User.UserId;
            //
            SecurityContext.SetThreadSupervisorPrincipal();

            //get filter mode
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);
            StringDictionary serviceSettings = ServerController.GetServiceSettings(serviceId);
            bool bExclude = (Utils.ParseInt(serviceSettings["GalleryAppsFilterMode"], 0) != 1);  //0 - Exclude apps, 1- Include apps

            //
            string[] filteredApps = GetServiceAppsCatalogFilter(packageId);
            //



            if (filteredApps != null)
            {
                if (bExclude)
                {
                    result.Value = new List<GalleryApplication>(Array.FindAll(result.Value.ToArray(),
                        x => !Array.Exists(filteredApps,
                            z => z.Equals(x.Id, StringComparison.InvariantCultureIgnoreCase))));
                }
                else
                {
                    result.Value = new List<GalleryApplication>(Array.FindAll(result.Value.ToArray(),
                        x => Array.Exists(filteredApps,
                            z => z.Equals(x.Id, StringComparison.InvariantCultureIgnoreCase))));

                }
            }
            //
            SecurityContext.SetThreadPrincipal(userId);
        }

        public static GalleryApplicationsResult GetGalleryApplicationsFiltered(int packageId, string pattern)
        {
            GalleryApplicationsResult result;
            //
            try
            {
                TaskManager.StartTask(TASK_MANAGER_SOURCE, GET_GALLERY_APPS_TASK);

                // check if WAG is installed
                WebServer webServer = GetAssociatedWebServer(packageId);
                if (!webServer.IsMsDeployInstalled())
                    return Error<GalleryApplicationsResult>(GalleryErrors.MsDeployIsNotInstalled);

                // get applications
                result = webServer.GetGalleryApplicationsFiltered(SecurityContext.User.UserId,pattern);

                FilterResultApplications(packageId, result);

                if (!result.IsSuccess)
                    return Error<GalleryApplicationsResult>(result, GalleryErrors.GetApplicationsError);
                
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
                return Error<GalleryApplicationsResult>(GalleryErrors.GeneralError, ex.Message);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
            //
            return result;

        }


		internal static bool MatchParticularAppDependency(Dependency dependency, string[] dependencyIds)
		{
			List<Dependency> nested = null;
			// Web PI ver. 0.2
			if (dependency.LogicalAnd.Count > 0)
				nested = dependency.LogicalAnd;
			else if (dependency.LogicalOr.Count > 0)
				nested = dependency.LogicalOr;
			// Web PI ver. 2.0.1.0
			else if (dependency.And.Count > 0)
				nested = dependency.And;
			else if (dependency.Or.Count > 0)
				nested = dependency.Or;

			if (nested != null)
			{
				// Check conditions
				foreach (Dependency ndep in nested)
					if (MatchGalleryAppDependencies(ndep, dependencyIds))
						return true;
				//
				return false;
			}
			// Non-empty dependencies should be filtered out if do not match
			if (!String.IsNullOrEmpty(dependency.ProductId))
				return Array.Exists<string>(dependencyIds, x => String.Equals(x, dependency.ProductId,
					StringComparison.InvariantCultureIgnoreCase));

			// Empty should not match everything when checking a certain dependencies
			return false;
		}

		internal static bool MatchGalleryAppDependencies(Dependency dependency, string[] dependencyIds)
		{
			List<Dependency> nested = null;
			// Web PI ver. 0.2
			if (dependency.LogicalAnd.Count > 0)
				nested = dependency.LogicalAnd;
			else if (dependency.LogicalOr.Count > 0)
				nested = dependency.LogicalOr;
			// Web PI ver. 2.0.1.0
			else if (dependency.And.Count > 0)
				nested = dependency.And;
			else if (dependency.Or.Count > 0)
				nested = dependency.Or;

			if (nested != null)
			{
				// Check LogicalAnd conditions
				if (nested == dependency.LogicalAnd || nested == dependency.And)
				{
					foreach (Dependency ndep in nested)
						if (!MatchGalleryAppDependencies(ndep, dependencyIds))
							return false;
					//
					return true;
				}
				//
				if (nested == dependency.LogicalOr || nested == dependency.Or)
				{
					bool matchOK = false;
					//
					foreach (Dependency ndep in nested)
						if (MatchGalleryAppDependencies(ndep, dependencyIds))
							matchOK = true;
					//
					return matchOK;
				}
			}
			// Non-empty dependencies should be filtered out if do not match
			if (!String.IsNullOrEmpty(dependency.ProductId))
				return Array.Exists<string>(dependencyIds, x => String.Equals(x, dependency.ProductId,
					StringComparison.InvariantCultureIgnoreCase));

			// Empty dependencies always match everything
			return true;
		}

		public static GalleryApplicationResult GetGalleryApplicationDetails(int packageId, string applicationId)
		{
			GalleryApplicationResult result;

			//
			try
			{
				TaskManager.StartTask(TASK_MANAGER_SOURCE, GET_GALLERY_APP_DETAILS_TASK);

                // check if WAG is installed
                WebServer webServer = GetAssociatedWebServer(packageId);
                if (!webServer.IsMsDeployInstalled())
                    return Error<GalleryApplicationResult>(GalleryErrors.MsDeployIsNotInstalled);
				
                // get application details
                result = webServer.GetGalleryApplication(SecurityContext.User.UserId,applicationId);
				
				if (!result.IsSuccess)
                    return Error<GalleryApplicationResult>(result, GalleryErrors.GetApplicationError);

                // check application requirements
                PackageContext context = PackageController.GetPackageContext(packageId);

                GalleryApplication app = result.Value;

                // ASP.NET 2.0
                if ((app.WellKnownDependencies & GalleryApplicationWellKnownDependency.AspNet20) == GalleryApplicationWellKnownDependency.AspNet20
                    && context.Quotas.ContainsKey(Quotas.WEB_ASPNET20) && context.Quotas[Quotas.WEB_ASPNET20].QuotaAllocatedValue < 1)
                    result.ErrorCodes.Add(GalleryErrors.AspNet20Required);

                // ASP.NET 4.0
                else if ((app.WellKnownDependencies & GalleryApplicationWellKnownDependency.AspNet40) == GalleryApplicationWellKnownDependency.AspNet40
                    && context.Quotas.ContainsKey(Quotas.WEB_ASPNET40) && context.Quotas[Quotas.WEB_ASPNET40].QuotaAllocatedValue < 1)
                    result.ErrorCodes.Add(GalleryErrors.AspNet40Required);

                // PHP
                else if ((app.WellKnownDependencies & GalleryApplicationWellKnownDependency.PHP) == GalleryApplicationWellKnownDependency.PHP
                    && context.Quotas.ContainsKey(Quotas.WEB_PHP4) && context.Quotas[Quotas.WEB_PHP4].QuotaAllocatedValue < 1
                    && context.Quotas.ContainsKey(Quotas.WEB_PHP5) && context.Quotas[Quotas.WEB_PHP5].QuotaAllocatedValue < 1)
                    result.ErrorCodes.Add(GalleryErrors.PhpRequired);

                // any database
                GalleryApplicationWellKnownDependency anyDatabaseFlag = GalleryApplicationWellKnownDependency.SQL | GalleryApplicationWellKnownDependency.MySQL;
                if ((app.WellKnownDependencies & anyDatabaseFlag) == anyDatabaseFlag &&
                    !(context.Groups.ContainsKey(ResourceGroups.MsSql2000)
                    || context.Groups.ContainsKey(ResourceGroups.MsSql2005)
                    || context.Groups.ContainsKey(ResourceGroups.MsSql2008)
                    || context.Groups.ContainsKey(ResourceGroups.MsSql2012)
                    || context.Groups.ContainsKey(ResourceGroups.MsSql2014)
                    || context.Groups.ContainsKey(ResourceGroups.MsSql2016)
                    || context.Groups.ContainsKey(ResourceGroups.MySql4)
                    || context.Groups.ContainsKey(ResourceGroups.MySql5)
                    || context.Groups.ContainsKey(ResourceGroups.MariaDB)))
                    result.ErrorCodes.Add(GalleryErrors.DatabaseRequired);

                // SQL Server
                else if ((app.WellKnownDependencies & GalleryApplicationWellKnownDependency.SQL) == GalleryApplicationWellKnownDependency.SQL
                    && !(context.Groups.ContainsKey(ResourceGroups.MsSql2000)
                    || context.Groups.ContainsKey(ResourceGroups.MsSql2005)
                    || context.Groups.ContainsKey(ResourceGroups.MsSql2008)
                    || context.Groups.ContainsKey(ResourceGroups.MsSql2012)
                    || context.Groups.ContainsKey(ResourceGroups.MsSql2014)
                    || context.Groups.ContainsKey(ResourceGroups.MsSql2016)))
                    result.ErrorCodes.Add(GalleryErrors.SQLRequired);

                // MySQL
                else if ((app.WellKnownDependencies & GalleryApplicationWellKnownDependency.MySQL) == GalleryApplicationWellKnownDependency.MySQL
                    && !(context.Groups.ContainsKey(ResourceGroups.MySql4)
                    || context.Groups.ContainsKey(ResourceGroups.MySql5)
                    || context.Groups.ContainsKey(ResourceGroups.MariaDB)))
                    result.ErrorCodes.Add(GalleryErrors.MySQLRequired);


                //show Dependency warning optionaly 
                int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);
                StringDictionary serviceSettings = ServerController.GetServiceSettings(serviceId);

                bool galleryAppsAlwaysIgnoreDependencies = Utils.ParseBool(serviceSettings["GalleryAppsAlwaysIgnoreDependencies"], false);

                if (galleryAppsAlwaysIgnoreDependencies)
                {
                     result.ErrorCodes.Clear();
                }

                if (result.ErrorCodes.Count > 0)
                {
                    GalleryApplicationResult warning = Warning<GalleryApplicationResult>(result, GalleryErrors.PackageDoesNotMeetRequirements);
                    warning.Value = app;
                    return warning;
                }
			}
			catch (Exception ex)
			{
                TaskManager.WriteError(ex);
                return Error<GalleryApplicationResult>(GalleryErrors.GeneralError, ex.Message);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static DeploymentParametersResult GetGalleryApplicationParams(int packageId, string webAppId)
		{
			DeploymentParametersResult result = null;
			//
			try
			{
				TaskManager.StartTask(TASK_MANAGER_SOURCE, GET_APP_PARAMS_TASK);

                // check if WAG is installed
                WebServer webServer = GetAssociatedWebServer(packageId);
                if (!webServer.IsMsDeployInstalled())
                    return Error<DeploymentParametersResult>(GalleryErrors.MsDeployIsNotInstalled);

				// get parameters
                result = webServer.GetGalleryApplicationParameters(SecurityContext.User.UserId,webAppId);
				
				if (!result.IsSuccess)
                    return Error<DeploymentParametersResult>(result, GalleryErrors.GetApplicationParametersError);
			}
			catch (Exception ex)
			{
                TaskManager.WriteError(ex);
                return Error<DeploymentParametersResult>(GalleryErrors.GeneralError, ex.Message);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			
			return result;
		}

		public static StringResultObject Install(int packageId, string webAppId, string siteName, string virtualDir, List<DeploymentParameter> parameters, string languageId )
		{
			StringResultObject result = new StringResultObject();
		    int originalUserId = SecurityContext.User.UserId;

            try
            {
                // database operation results
                int databaseResult = -1;
                int databaseUserResult = -1;

                // initialize task manager
				TaskManager.StartTask(TASK_MANAGER_SOURCE, "INSTALL_WEB_APP");
				TaskManager.WriteParameter("Package ID", packageId);
				TaskManager.WriteParameter("Site Name", siteName);

                #region Check Space and Account
                // Check account
                int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
                if (accountCheck < 0)
                    return Warning<StringResultObject>((-accountCheck).ToString());

                // Check space
                int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
                if (packageCheck < 0)
                    return Warning<StringResultObject>((-packageCheck).ToString());
                #endregion

                #region Check MS Deploy, web site and application pack
                // get target web server
				WebServer webServer = GetAssociatedWebServer(packageId);

				// Check if Web App Gallery is installed
				if (!webServer.IsMsDeployInstalled())
					return Error<StringResultObject>(GalleryErrors.MsDeployIsNotInstalled);

                // Check web site for existence
                WebSite webSite = WebServerController.GetWebSite(packageId, siteName);
                if (webSite == null)
                    return Error<StringResultObject>(GalleryErrors.WebSiteNotFound, siteName);

				// get application pack details
                GalleryApplicationResult app = webServer.GetGalleryApplication(SecurityContext.User.UserId,webAppId);
                if (!app.IsSuccess)
                    return Error<StringResultObject>(app, GalleryErrors.GeneralError);
                if (app.Value == null)
                    return Error<StringResultObject>(GalleryErrors.WebApplicationNotFound, webAppId);
                #endregion

				#region Trace app details

                // Assign web app pack title to the currently running task
                TaskManager.ItemName = app.Value.Title;

				// Trace additional details from the feed
                TaskManager.WriteParameter("Title", app.Value.Title);
                TaskManager.WriteParameter("Version", app.Value.Version);
                TaskManager.WriteParameter("Download URL", app.Value.DownloadUrl);
                TaskManager.WriteParameter("Author", app.Value.AuthorName);
                TaskManager.WriteParameter("Last Updated", app.Value.LastUpdated);

				// Trace out all deployment parameters
				Array.ForEach<DeploymentParameter>(parameters.ToArray(), p => TaskManager.WriteParameter(p.Name, p.Value));
                #endregion

                // elevate security context
                SecurityContext.SetThreadSupervisorPrincipal();

                #region Set AppPath
                // set correct application path
                DeploymentParameter appPath = FindParameterByTag(parameters, DeploymentParameterWellKnownTag.IisApp);
                if (appPath == null)
                    return Error<StringResultObject>(GalleryErrors.AppPathParameterNotFound);

                appPath.Value = String.IsNullOrEmpty(virtualDir) ? siteName : String.Format("{0}/{1}", siteName, virtualDir);
                #endregion

                // database context
                // find database resource parameter
                DeploymentParameter databaseResoure = parameters.Find( p =>
                {
                    return (p.Name == DeploymentParameter.ResourceGroupParameterName);
                });

                // database is required for this application
                if (databaseResoure != null)
                {
                    // try to get database service
                    int dbServiceId = PackageController.GetPackageServiceId(packageId, databaseResoure.Value);
                    if (dbServiceId == 0)
                        return Error<StringResultObject>(GalleryErrors.DatabaseServiceIsNotAvailable);

                    #region Setup Database server and DB Admin credentials
                    // get database service settings
                    StringDictionary dbSettings = ServerController.GetServiceSettingsAdmin(dbServiceId);

                    // database server
                    DeploymentParameter databaseServer = FindParameterByTag(parameters, DeploymentParameterWellKnownTag.DBServer);
                    if (databaseServer != null)
                    {
                        databaseServer.Value = dbSettings["ExternalAddress"];
                        if (String.IsNullOrEmpty(databaseServer.Value))
                            return Error<StringResultObject>(GalleryErrors.DatabaseServerExternalAddressIsEmpty);
                    }

                    // database admin
                    DeploymentParameter databaseAdminUsername = FindParameterByTag(parameters, DeploymentParameterWellKnownTag.DBAdminUserName);
                    if (databaseAdminUsername != null)
                    {
                        databaseAdminUsername.Value = dbSettings["RootLogin"];
                        if(String.IsNullOrEmpty(databaseAdminUsername.Value))
                            databaseAdminUsername.Value = dbSettings["SaLogin"];

                        // raise error if database service is in Integrated Security mode (for SQL Server)
                        // or DB Admin username is not provided
                        if (String.IsNullOrEmpty(databaseAdminUsername.Value))
                            return Error<StringResultObject>(GalleryErrors.DatabaseAdminUsernameNotSpecified);
                    }

                    // database admin password
                    DeploymentParameter databaseAdminPassword = FindParameterByTag(parameters, DeploymentParameterWellKnownTag.DBAdminPassword);
                    if (databaseAdminPassword != null)
                    {
                        databaseAdminPassword.Value = dbSettings["RootPassword"];
                        if (String.IsNullOrEmpty(databaseAdminPassword.Value))
                            databaseAdminPassword.Value = dbSettings["SaPassword"];

                        // raise error if database service is in Integrated Security mode (for SQL Server)
                        // or DB Admin password is not provided
                        if (String.IsNullOrEmpty(databaseAdminPassword.Value))
                            return Error<StringResultObject>(GalleryErrors.DatabaseAdminPasswordNotSpecified);
                    }
                    #endregion

                    #region Create database and db user account if new selected

                    // create database
                    DeploymentParameter databaseName = FindParameterByTag(parameters, DeploymentParameterWellKnownTag.DBName);
                    if (databaseName != null)
                    {
                        SqlDatabase db = PackageController.GetPackageItemByName(packageId, databaseResoure.Value, databaseName.Value, typeof(SqlDatabase)) as SqlDatabase;
                        
                        if (db == null)
                        {
                            try
                            {
                                db = new SqlDatabase();
                                db.PackageId = packageId;
                                db.Name = databaseName.Value;

                                // create
                                databaseResult = DatabaseServerController.AddSqlDatabase(db, databaseResoure.Value);
                                if (databaseResult < 0)
                                {
                                    result.ErrorCodes.Add((-databaseResult).ToString());
                                    return Error<StringResultObject>(result, GalleryErrors.DatabaseCreationError);
                                }
                            }
                            catch (Exception ex)
                            {
                                // log exception
                                TaskManager.WriteError(ex);

                                // return error
                                return Error<StringResultObject>(GalleryErrors.DatabaseCreationException); 
                            }
                        }
                    }

                    // create database user
                    DeploymentParameter databaseUsername = FindParameterByTag(parameters, DeploymentParameterWellKnownTag.DBUserName);
                    DeploymentParameter databaseUserPassword = FindParameterByTag(parameters, DeploymentParameterWellKnownTag.DBUserPassword);

                    if (databaseUsername != null && databaseUserPassword != null)
                    {
                        SqlUser user = PackageController.GetPackageItemByName(packageId, databaseResoure.Value, databaseUsername.Value, typeof(SqlUser)) as SqlUser;
                        //
                        if (user == null)
                        {
                            // create new user account
                            try
                            {
                                user = new SqlUser();
                                user.PackageId = packageId;
                                user.Name = databaseUsername.Value;
                                user.Databases = (databaseName != null) ? new string[] { databaseName.Value } : new string[0];
                                user.Password = databaseUserPassword.Value;

                                // create
                                databaseUserResult = DatabaseServerController.AddSqlUser(user, databaseResoure.Value);

                                // check results
                                if (databaseUserResult < 0)
                                {
                                    // Rollback and remove db if created
                                    if (databaseResult > 0)
                                        DatabaseServerController.DeleteSqlDatabase(databaseResult);

                                    // raise error
                                    result.ErrorCodes.Add((-databaseUserResult).ToString());
                                    return Error<StringResultObject>(result, GalleryErrors.DatabaseUserCreationError);
                                }
                            }
                            catch (Exception ex)
                            {
                                // log exception
                                TaskManager.WriteError(ex);

                                // return error
                                return Error<StringResultObject>(GalleryErrors.DatabaseUserCreationException, ex.Message);
                            }
                        }
                        else
                        {
                            // check existing user account
                            DatabaseServer databaseService = DatabaseServerController.GetDatabaseServer(dbServiceId);
                            if (!databaseService.CheckConnectivity(databaseName.Value, databaseUsername.Value, databaseUserPassword.Value))
                            {
                                return Error<StringResultObject>(GalleryErrors.DatabaseUserCannotAccessDatabase, databaseUsername.Value);
                            }
                        }
                    }
                    #endregion

                    // remove database resource parameter from the list
                    // before calling "install" method
                    parameters.Remove(databaseResoure);
                }
				
                // install application
                result = webServer.InstallGalleryApplication(originalUserId, webAppId, parameters.ToArray(), languageId);

				#region Rollback in case of failure
				// Rollback - remove resources have been created previously
				if (!result.IsSuccess)
				{
					// delete database
					if (databaseUserResult > 0)
						DatabaseServerController.DeleteSqlUser(databaseUserResult);

					// delete database user
					if (databaseResult > 0)
						DatabaseServerController.DeleteSqlDatabase(databaseResult);

                    // exit with errors
                    return Error<StringResultObject>(result, GalleryErrors.ApplicationInstallationError);
				}
				#endregion

                #region Update Web Application settings

                WebAppVirtualDirectory iisApp = null;
                    if (String.IsNullOrEmpty(virtualDir))
                        // load web site
                        iisApp = WebServerController.GetWebSite(packageId, siteName);
                    else
                    {
                        try
                        {
                            // load virtual directory
                            iisApp = WebServerController.GetAppVirtualDirectory(webSite.Id, virtualDir);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(
                                string.Format(
                                    "{0} on WebServerController.GetAppVirtualDirectory(\"{1}\", \"{2}\")", 
                                    ex.GetType(), webSite.Id, virtualDir), 
                                ex);
                        }
                    }

                // put correct extensions
                if ((app.Value.WellKnownDependencies & GalleryApplicationWellKnownDependency.AspNet20) == GalleryApplicationWellKnownDependency.AspNet20)
                {
                    // ASP.NET 2.0
                    iisApp.AspNetInstalled = (iisApp.IIs7) ? "2I" : "2";
                    AddDefaultDocument(iisApp, "default.aspx");
                }
                else if ((app.Value.WellKnownDependencies & GalleryApplicationWellKnownDependency.AspNet40) == GalleryApplicationWellKnownDependency.AspNet40)
                {
                    // ASP.NET 4.0
                    iisApp.AspNetInstalled = (iisApp.IIs7) ? "4I" : "4";
                    AddDefaultDocument(iisApp, "default.aspx");
                }
                else if ((app.Value.WellKnownDependencies & GalleryApplicationWellKnownDependency.PHP) == GalleryApplicationWellKnownDependency.PHP)
                {
                    // PHP 5
                    iisApp.PhpInstalled = "5";
                    AddDefaultDocument(iisApp, "index.php");
                }

                // update web site or virtual directory
                int updateResult = 0;
                if (String.IsNullOrEmpty(virtualDir))
                    // update web site
                    updateResult = WebServerController.UpdateWebSite(iisApp as WebSite);
                else
                    // update virtual directory
                    updateResult = WebServerController.UpdateAppVirtualDirectory(webSite.Id, iisApp);

                if(updateResult < 0)
                    TaskManager.WriteWarning("Cannot update website or virtual directory programming extensions and default documents. Result code: {0}", updateResult.ToString());

                #endregion

				return result;
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex);

                // exit with error code
                //return Error<StringResultObject>(GalleryErrors.GeneralError);
                
                result.AddError(GalleryErrors.GeneralError, ex);
                return result;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
		}

        private static void AddDefaultDocument(WebAppVirtualDirectory iisApp, string document)
        {
            // parse list
            List<string> documents = new List<string>();

            if (!String.IsNullOrEmpty(iisApp.DefaultDocs))
                documents.AddRange(iisApp.DefaultDocs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            // add document if required
            if (documents.Find(d => { return String.Compare(d, document, true) == 0; }) == null)
                documents.Add(document);

            iisApp.DefaultDocs = String.Join(",", documents.ToArray());
        }

		public static GalleryWebAppStatus GetGalleryApplicationStatus(int packageId, string webAppId)
		{
			try
			{
				WebServer webServer = GetAssociatedWebServer(packageId);
				//
				if (!webServer.IsMsDeployInstalled())
                    return Error<GalleryWebAppStatus>(GalleryErrors.MsDeployIsNotInstalled);
				//
                GalleryWebAppStatus appStatus = webServer.GetGalleryApplicationStatus(SecurityContext.User.UserId,webAppId);
				//
				if (appStatus == GalleryWebAppStatus.NotDownloaded)
				{
                    GalleryApplicationResult appResult = webServer.GetGalleryApplication(SecurityContext.User.UserId,webAppId);
					// Start app download in new thread
					WebAppGalleryAsyncWorker async = new WebAppGalleryAsyncWorker();
					async.GalleryApp = appResult.Value;
					async.WebAppId = webAppId;
					async.PackageId = packageId;
					async.UserId = SecurityContext.User.UserId;
					async.DownloadGalleryWebApplicationAsync();
					//
					return GalleryWebAppStatus.Downloading;
				}
				//
				return appStatus;
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.StackTrace);
				//
				return GalleryWebAppStatus.Failed;
			}
		}

		internal static WebServer GetAssociatedWebServer(int packageId)
		{
			int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);
			//
			return WebServerController.GetWebServer(serviceId);
		}

		internal static string[] GetServiceAppsCatalogFilter(int packageId)
		{
			int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);
			//
			StringDictionary serviceSettings = ServerController.GetServiceSettings(serviceId);
			//
			string galleryAppsFilterStr = serviceSettings["GalleryAppsFilter"];
			//
			if (String.IsNullOrEmpty(galleryAppsFilterStr))
				return null;
			//
			return galleryAppsFilterStr.Split(new string[] { "," }, 
				StringSplitOptions.RemoveEmptyEntries);
		}

		public static bool MatchParameterByNames(DeploymentParameter param, string[] namesMatches)
		{
			foreach (string nameMatch in namesMatches)
				if (MatchParameterName(param, nameMatch))
					return true;
			//
			return false;
		}

        private static DeploymentParameter FindParameterByTag(List<DeploymentParameter> parameters, DeploymentParameterWellKnownTag tag)
        {
            return parameters.Find( p => { return (p.WellKnownTags & tag) == tag; });
        }

        private static DeploymentParameter FindParameterByName(List<DeploymentParameter> parameters, string name)
        {
            return parameters.Find( p => { return String.Compare(p.Name, name, true) == 0; });
        }

		public static bool MatchParameterName(DeploymentParameter param, string nameMatch)
		{
			if (param == null || String.IsNullOrEmpty(nameMatch))
				return false;
			//
			if (String.IsNullOrEmpty(param.Name))
				return false;
			// Match parameter name
			return (param.Name.ToLowerInvariant() == nameMatch.ToLowerInvariant());
		}

        #region Result object routines
        private static T Warning<T>(params string[] messageParts)
        {
            return Warning<T>(null, messageParts);
        }

        private static T Warning<T>(ResultObject innerResult, params string[] messageParts)
        {
            return Result<T>(innerResult, false, messageParts);
        }

        private static T Error<T>(params string[] messageParts)
        {
            return Error<T>(null, messageParts);
        }

        private static T Error<T>(ResultObject innerResult, params string[] messageParts)
        {
            return Result<T>(innerResult, true, messageParts);
        }

        private static T Result<T>(ResultObject innerResult, bool isError, params string[] messageParts)
        {
            object obj = Activator.CreateInstance<T>();
            ResultObject result = (ResultObject)obj;

            // set error
            result.IsSuccess = !isError;

            // add message
            if (messageParts != null)
                result.ErrorCodes.Add(String.Join(":", messageParts));

            // copy errors from inner result
            if (innerResult != null)
                result.ErrorCodes.AddRange(innerResult.ErrorCodes);

            return (T)obj;
        }
        #endregion
	}

	public class WebAppGalleryAsyncWorker
	{
		public int PackageId { get; set; }
		public string WebAppId { get; set; }
		public GalleryApplication GalleryApp { get; set; }
		public int UserId { get; set; }

		public void DownloadGalleryWebApplicationAsync()
		{
			Thread t = new Thread(new ThreadStart(DownloadGalleryWebApplication));
			t.Start();
		}

		public void DownloadGalleryWebApplication()
		{
			SecurityContext.SetThreadPrincipal(UserId);
			//
			TaskManager.StartTask(WebAppGalleryController.TASK_MANAGER_SOURCE, "DOWNLOAD_WEB_APP", GalleryApp.Title);
			TaskManager.WriteParameter("Version", GalleryApp.Version);
			TaskManager.WriteParameter("Download URL", GalleryApp.DownloadUrl);
			TaskManager.WriteParameter("Author", GalleryApp.AuthorName);
			TaskManager.WriteParameter("Last Updated", GalleryApp.LastUpdated);
			TaskManager.WriteParameter("Web App ID", WebAppId);
			//
			try
			{
				//
				WebServer webServer = WebAppGalleryController.GetAssociatedWebServer(PackageId);
				//
				TaskManager.Write("Application package download has been started");
				//
                GalleryWebAppStatus appStatus = webServer.DownloadGalleryApplication(SecurityContext.User.UserId,WebAppId);
				//
				if (appStatus == GalleryWebAppStatus.Failed)
				{
					TaskManager.WriteError("Could not download application package requested");
					TaskManager.WriteError("Please check SolidCP Server log for further information on this issue");
					TaskManager.WriteParameter("Status returned", appStatus);
					return;
				}
				//
				TaskManager.Write("Application package download has been started successfully");
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
			finally
			{
				//
				TaskManager.CompleteTask();
			}
		}
	}
}
