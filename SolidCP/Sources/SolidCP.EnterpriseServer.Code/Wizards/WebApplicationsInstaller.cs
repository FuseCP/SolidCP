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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using System.Net.Mail;
using System.Web;
using System.Web.Caching;
using SolidCP.Providers;
using SolidCP.Providers.Web;
using SolidCP.Providers.FTP;
using SolidCP.Providers.Mail;
using SolidCP.Providers.Database;
using SolidCP.Providers.OS;
using OS = SolidCP.Providers.OS;


namespace SolidCP.EnterpriseServer
{
    public class WebApplicationsInstaller
    {
        public const string PROPERTY_CONTENT_PATH = "installer.contentpath";
        public const string PROPERTY_ABSOLUTE_CONTENT_PATH = "installer.absolute.contentpath";
        public const string PROPERTY_VDIR_CREATED = "installer.virtualdircreated";
        public const string PROPERTY_DATABASE_CREATED = "installer.databasecreated";
        public const string PROPERTY_USER_CREATED = "installer.usercreated";
        public const string PROPERTY_INSTALLED_FILES = "installer.installedfiles";
        public const string PROPERTY_DELETE_FILES = "installer.deletefiles";
        public const string PROPERTY_DELETE_VDIR = "installer.deletevdir";
        public const string PROPERTY_DELETE_SQL = "installer.deletesql";
        public const string PROPERTY_DELETE_DATABASE = "installer.deletedatabase";
        public const string PROPERTY_DELETE_USER = "installer.deleteuser";

        public static int InstallApplication(InstallationInfo inst)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check package
            int packageCheck = SecurityContext.CheckPackage(inst.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            // install application
            WebApplicationsInstaller installer = new WebApplicationsInstaller();
            return installer.InstallWebApplication(inst);
        }

        ApplicationInfo app = null;
        private string contentPath = null;
        private string siteId = null;
        private OS.OperatingSystem os = null;
        private DatabaseServer sql = null;
        private string serverIpAddressExternal = null;
		private string serverIpAddressInternal = null;
        private string webSiteName = "";

        public int InstallWebApplication(InstallationInfo inst)
        {
            // place log record
            TaskManager.StartTask("APP_INSTALLER", "INSTALL_APPLICATION", inst.PackageId);

            TaskManager.WriteParameter("Virtual directory", inst.VirtualDir);
            TaskManager.WriteParameter("Database group", inst.DatabaseGroup);
            
            try
            {
                // get application info
                app = GetApplication(inst.PackageId, inst.ApplicationId);

                BackgroundTask topTask = TaskManager.TopTask;

                topTask.ItemName = app.Name;

                TaskController.UpdateTask(topTask);

                // check web site for existance
                WebSite webSite = WebServerController.GetWebSite(inst.WebSiteId);

                if (webSite == null)
                    return BusinessErrorCodes.ERROR_WEB_INSTALLER_WEBSITE_NOT_EXISTS;

				TaskManager.WriteParameter("Web site", webSite.Name);

                webSiteName = webSite.Name;
                siteId = webSite.SiteId;

                // change web site properties if required
                if (String.IsNullOrEmpty(inst.VirtualDir))
                {
                    ChangeAppVirtualDirectoryProperties(webSite, app.WebSettings);
                    WebServerController.UpdateWebSite(webSite);
                }

                // get OS service
                int osId = PackageController.GetPackageServiceId(inst.PackageId, "os");
                os = new OS.OperatingSystem();
                ServiceProviderProxy.Init(os, osId);

                // get remote content path
                contentPath = webSite.ContentPath;

                // create virtual dir if required
                if (!String.IsNullOrEmpty(inst.VirtualDir))
                {
                    // check if the required virtual dir already exists
                    contentPath = Path.Combine(contentPath, inst.VirtualDir);

                    WebAppVirtualDirectory vdir = null;
                    int result = WebServerController.AddAppVirtualDirectory(inst.WebSiteId, inst.VirtualDir, contentPath);
                    if (result == BusinessErrorCodes.ERROR_VDIR_ALREADY_EXISTS)
                    {
                        // the directory alredy exists
                        vdir = WebServerController.GetAppVirtualDirectory(
                            inst.WebSiteId, inst.VirtualDir);

                        contentPath = vdir.ContentPath;
                    }
                    else
                    {
                        vdir = WebServerController.GetAppVirtualDirectory(
                            inst.WebSiteId, inst.VirtualDir);

                        inst[PROPERTY_VDIR_CREATED] = "True";
                    }

                    // change virtual directory properties if required
                    ChangeAppVirtualDirectoryProperties(vdir, app.WebSettings);
                    WebServerController.UpdateAppVirtualDirectory(inst.WebSiteId, vdir);
                }

                // deploy application codebase ZIP and then unpack it
                string codebasePath = app.Codebase;
                string remoteCodebasePath = Path.Combine(contentPath, Path.GetFileName(app.Codebase));

                // make content path absolute
                string absContentPath = FilesController.GetFullPackagePath(inst.PackageId, contentPath);

                // save content path
                inst[PROPERTY_CONTENT_PATH] = contentPath;
                inst[PROPERTY_ABSOLUTE_CONTENT_PATH] = absContentPath;

                // copy ZIP to the target server
                FileStream stream = File.OpenRead(codebasePath);
                int BUFFER_LENGTH = 5000000;

                byte[] buffer = new byte[BUFFER_LENGTH];
                int readBytes = 0;
                while (true)
                {
                    readBytes = stream.Read(buffer, 0, BUFFER_LENGTH);

                    if (readBytes < BUFFER_LENGTH)
                        Array.Resize<byte>(ref buffer, readBytes);

                    FilesController.AppendFileBinaryChunk(inst.PackageId, remoteCodebasePath, buffer);

                    if (readBytes < BUFFER_LENGTH)
                        break;
                }


                
                // unpack codebase
                inst[PROPERTY_INSTALLED_FILES] = String.Join(";",
                   FilesController.UnzipFiles(inst.PackageId, new string[] { remoteCodebasePath }));
                
                // delete codebase zip
                FilesController.DeleteFiles(inst.PackageId, new string[] { remoteCodebasePath });

                // check/create databases
                if (!String.IsNullOrEmpty(inst.DatabaseGroup) &&
                    String.Compare(inst.DatabaseGroup, "None", true) != 0)
                {
                    // database
                    if (inst.DatabaseId == 0)
                    {
                        TaskManager.WriteParameter("Database name", inst.DatabaseName);

                        // we should create a new database
                        SqlDatabase db = new SqlDatabase();
                        db.PackageId = inst.PackageId;
                        db.Name = inst.DatabaseName;
                        inst.DatabaseId = DatabaseServerController.AddSqlDatabase(db, inst.DatabaseGroup);
                        if (inst.DatabaseId < 0)
                        {
                            // rollback installation
                            RollbackInstallation(inst);

                            // return error
                            return inst.DatabaseId; // there was an error when creating database
                        }

                        inst[PROPERTY_DATABASE_CREATED] = "True";
                    }
                    else
                    {
                        // existing database
                        SqlDatabase db = DatabaseServerController.GetSqlDatabase(inst.DatabaseId);
                        inst.DatabaseName = db.Name;

                        TaskManager.WriteParameter("Database name", inst.DatabaseName);
                    }

                    SqlUser user = null;
                    // database user
                    if (inst.UserId == 0)
                    {
                        TaskManager.WriteParameter("Database user", inst.Username);

                        // NEW USER
                        user = new SqlUser();
                        user.PackageId = inst.PackageId;
                        user.Name = inst.Username;
                        user.Databases = new string[] { inst.DatabaseName };
                        user.Password = inst.Password;
                        inst.UserId = DatabaseServerController.AddSqlUser(user, inst.DatabaseGroup);
                        if (inst.UserId < 0)
                        {
                            // rollback installation
                            RollbackInstallation(inst);

                            // return error
                            return inst.UserId; // error while adding user
                        }

                        inst[PROPERTY_USER_CREATED] = "True";
                    }
                    else
                    {
                        // EXISTING USER
                        user = DatabaseServerController.GetSqlUser(inst.UserId);
                        inst.Username = user.Name;

                        TaskManager.WriteParameter("Database user", inst.Username);

                        List<string> databases = new List<string>();
                        databases.AddRange(user.Databases);

                        if (!databases.Contains(inst.DatabaseName))
                        {
                            databases.Add(inst.DatabaseName);

                            user.Databases = databases.ToArray();
                            DatabaseServerController.UpdateSqlUser(user);
                        }
                    }

                    // check connectivity with SQL Server and credentials provided
                    // load user item
                    int sqlServiceId = PackageController.GetPackageServiceId(inst.PackageId, inst.DatabaseGroup);
                    sql = new DatabaseServer();
                    ServiceProviderProxy.Init(sql, sqlServiceId);

                    if (!sql.CheckConnectivity(inst.DatabaseName, inst.Username,
                        inst.Password))
                    {
                        // can't connect to the database
                        RollbackInstallation(inst);

                        return BusinessErrorCodes.ERROR_WEB_INSTALLER_CANT_CONNECT_DATABASE;
                    }

                    // read SQL server settings
                    StringDictionary settings = ServerController.GetServiceSettings(sqlServiceId);
                    serverIpAddressExternal = settings["ExternalAddress"];
					if (settings.ContainsKey("InternalAddress"))
					{
						serverIpAddressInternal = settings["InternalAddress"];
					}
                }

                // ********* RUN INSTALL SCENARIO ***********
                int scriptResult = RunInstallScenario(inst);
                if (scriptResult < 0)
                {
                    // rollback installation
                    RollbackInstallation(inst);

                    // return error
                    return scriptResult;
                }

                // add new installation to the database
                return 0;
            }
            catch (Exception ex)
            {
                // rollback installation
                RollbackInstallation(inst);

                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        private void ChangeAppVirtualDirectoryProperties(WebAppVirtualDirectory vdir,
            ApplicationWebSetting[] settings)
        {
            if (settings == null)
                return;

            // get type properties
            Type vdirType = vdir.GetType();
            foreach (ApplicationWebSetting setting in settings)
            {
                PropertyInfo prop = vdirType.GetProperty(setting.Name,
                    BindingFlags.Public | BindingFlags.Instance);

                if (prop != null)
                {
                    prop.SetValue(vdir, ObjectUtils.Cast(setting.Value, prop.PropertyType), null);
                }
            }
        }

        private int RunInstallScenario(InstallationInfo inst)
        {
            string scenarioPath = Path.Combine(app.Folder, "Install.xml");
            return RunScenario(scenarioPath, inst, true);
        }

        private string GetFullPathToInstallFolder(int userId)
        {
            string userhomeFolder = String.Empty;
            string[] osSesstings = os.ServiceProviderSettingsSoapHeaderValue.Settings;
            foreach (string s in osSesstings)
            {
                if (s.Contains("usershome"))
                {
                    string[] split = s.Split(new char[] {'='});
                    userhomeFolder = split[1];
                }
            }
            UserInfo info = UserController.GetUser(userId);
            return Path.Combine(userhomeFolder, info.Username);
        }

        private int RunScenario(string scenarioPath, InstallationInfo inst, bool throwExceptions)
        {
            // load XML document
            XmlDocument docScenario = new XmlDocument();
            docScenario.Load(scenarioPath);

            // go through "check" section
            XmlNode nodeCheck = docScenario.SelectSingleNode("//check");
            if (nodeCheck != null)
            {
                foreach (XmlNode nodeStep in nodeCheck.ChildNodes)
                {
                    if (nodeStep.Name == "fileExists")
                    {
                        /*
                        // check if the specified file exists
                        string fileName = nodeStep.Attributes["path"].Value;
                        fileName = ExpandVariables(fileName, inst);
                        if (fileName.StartsWith("\\"))
                        {
                            fileName = fileName.Substring(1);
                        }
                        //get full path to instal folder
                        PackageInfo package = PackageController.GetPackage(inst.PackageId);
                        string fullPath = Path.Combine(GetFullPathToInstallFolder(package.UserId), fileName);
                        if (os.FileExists(fullPath))
                            return BusinessErrorCodes.ERROR_WEB_INSTALLER_TARGET_WEBSITE_UNSUITABLE;
                         */
                    }
                    else if (nodeStep.Name == "sql")
                    {
                        string cmdText = nodeStep.InnerText;
                        cmdText = ExpandVariables(cmdText, inst);

                        DataSet dsResults = sql.ExecuteSqlQuery(inst.DatabaseName, cmdText);
                        if (dsResults.Tables[0].Rows.Count > 0)
                            return BusinessErrorCodes.ERROR_WEB_INSTALLER_TARGET_DATABASE_UNSUITABLE;
                    }
                }
            }

            // go through "commands" section
            XmlNode nodeCommands = docScenario.SelectSingleNode("//commands");
            if (nodeCommands != null)
            {
                foreach (XmlNode nodeCommand in nodeCommands.ChildNodes)
                {
                    if (nodeCommand.Name == "processFile")
                    {
                        // process remote file
                        string fileName = nodeCommand.Attributes["path"].Value;
                        fileName = ExpandVariables(fileName, inst);

                        byte[] fileBinaryContent = FilesController.GetFileBinaryContent(inst.PackageId, fileName);
                        if (fileBinaryContent == null)
                            throw new Exception("Could not process scenario file: " + fileName);

                        string fileContent = Encoding.UTF8.GetString(fileBinaryContent);
                        fileContent = ExpandVariables(fileContent, inst);

                        FilesController.UpdateFileBinaryContent(inst.PackageId, fileName,
                            Encoding.UTF8.GetBytes(fileContent));
                    }
                    else if (nodeCommand.Name == "runSql")
                    {
                        string cmdText = nodeCommand.InnerText;
                        if (nodeCommand.Attributes["path"] != null)
                        {
                            // load SQL from file
                            string sqlPath = Path.Combine(app.Folder, nodeCommand.Attributes["path"].Value);

                            if (!File.Exists(sqlPath))
                                continue;

                            StreamReader reader = new StreamReader(sqlPath);
                            cmdText = reader.ReadToEnd();
                            reader.Close();
                        }

                        bool run = true;
                        if (nodeCommand.Attributes["dependsOnProperty"] != null)
                        {
                            string[] propNames = nodeCommand.Attributes["dependsOnProperty"].Value.Split(',');
                            foreach (string propName in propNames)
                            {
                                if (inst[propName.Trim()] == null)
                                {
                                    run = false;
                                    break;
                                }
                            }
                        }

                        if (run)
                        {
                            try
                            {
                                cmdText = ExpandVariables(cmdText, inst);
                                sql.ExecuteSqlNonQuerySafe(inst.DatabaseName, inst.Username, inst.Password, cmdText);
                            }
                            catch (Exception ex)
                            {
                                if (throwExceptions)
                                    throw ex;
                            }
                        }
                    }
                }
            }
            return 0;
        }

        string appUrls = null;
        private string ExpandVariables(string str, InstallationInfo inst)
        {
            str = ReplaceTemplateVariable(str, "installer.contentpath", inst[PROPERTY_CONTENT_PATH]);
            str = ReplaceTemplateVariable(str, "installer.website", webSiteName);
            str = ReplaceTemplateVariable(str, "installer.virtualdir", inst.VirtualDir);

            string fullWebPath = webSiteName;
            if (!String.IsNullOrEmpty(inst.VirtualDir))
                fullWebPath += "/" + inst.VirtualDir;

            // try to load domain info
            DomainInfo domain = ServerController.GetDomain(webSiteName);
            string fullWebPathPrefix = (domain != null && domain.IsSubDomain) ? "" : "www.";

            // app URLs
            if (appUrls == null)
            {
                // read web pointers
                List<DomainInfo> sitePointers = WebServerController.GetWebSitePointers(inst.WebSiteId);
                StringBuilder sb = new StringBuilder();
                sb.Append("<urls>");
                sb.Append("<url value=\"").Append(fullWebPath).Append("\"/>");
                foreach (DomainInfo pointer in sitePointers)
                {
                    string pointerWebPath = pointer.DomainName;
                    if (!String.IsNullOrEmpty(inst.VirtualDir))
                        pointerWebPath += "/" + inst.VirtualDir;
                    sb.Append("<url value=\"").Append(pointerWebPath).Append("\"/>");
                }
                sb.Append("</urls>");
                appUrls = sb.ToString();
            }
            str = ReplaceTemplateVariable(str, "installer.appurls", appUrls);

            string slashVirtualDir = "";
            if (!String.IsNullOrEmpty(inst.VirtualDir))
                slashVirtualDir = "/" + inst.VirtualDir;

            str = ReplaceTemplateVariable(str, "installer.slashvirtualdir", slashVirtualDir);
            str = ReplaceTemplateVariable(str, "installer.website.www", fullWebPathPrefix + webSiteName);
            str = ReplaceTemplateVariable(str, "installer.fullwebpath", fullWebPath);
            str = ReplaceTemplateVariable(str, "installer.fullwebpath.www", fullWebPathPrefix + fullWebPath);
            //Replace ObjectQualifierNormalized which is not defined on portal
            str = ReplaceTemplateVariable(str, "ObjectQualifierNormalized", "");
			
			/*
			 * Application installer variable 'installer.database.server' is obsolete 
			 * and should not be used to install Application Packs.
			 * Instead, please use the following two variables:
			 *  - installer.database.server.external - defines external database address
			 *  - installer.database.server.internal - defines internal database address
			 * 
			 * See TFS Issue 952 for details.
			 */
			//apply external database address
			str = ReplaceTemplateVariable(str, "installer.database.server",
                ((serverIpAddressExternal != null) ? serverIpAddressExternal : ""));
			str = ReplaceTemplateVariable(str, "installer.database.server.external",
				((serverIpAddressExternal != null) ? serverIpAddressExternal : String.Empty));

			//apply internal database address
			str = ReplaceTemplateVariable(str, "installer.database.server.internal",
				((serverIpAddressInternal != null) ? serverIpAddressInternal : String.Empty));

            str = ReplaceTemplateVariable(str, "installer.database", inst.DatabaseName);
            str = ReplaceTemplateVariable(str, "installer.database.user", inst.Username);
            str = ReplaceTemplateVariable(str, "installer.database.password",
                ((inst.Password != null) ? inst.Password : ""));
            foreach (string[] pair in inst.PropertiesArray)
                str = ReplaceTemplateVariable(str, pair[0], pair[1]);
            return str;
        }

        private string ReplaceTemplateVariable(string str, string varName, string varValue)
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrEmpty(varName))
                return str;

            str = Regex.Replace(str, "\\$\\{" + varName + "\\}+", varValue, RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "\\$\\{" + varName + ".mysql-escaped\\}+", EscapeMySql(varValue), RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "\\$\\{" + varName + ".mariadb-escaped\\}+", EscapeMySql(varValue), RegexOptions.IgnoreCase);
            return Regex.Replace(str, "\\$\\{" + varName + ".mssql-escaped\\}+", EscapeMsSql(varValue), RegexOptions.IgnoreCase);
        }

        private string EscapeMySql(string str)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            return str.Replace("'", "\\'")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t")
            .Replace("\\", "\\\\")
            .Replace("\0", "\\0");
        }
        private string EscapeMariaDB(string str)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            return str.Replace("'", "\\'")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t")
            .Replace("\\", "\\\\")
            .Replace("\0", "\\0");
        }

        private string EscapeMsSql(string str)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            return str.Replace("'", "''");
        }

        private void RollbackInstallation(InstallationInfo inst)
        {
            // remove virtual dir
            if (inst[PROPERTY_VDIR_CREATED] != null)
            {
                // delete virtual directory
                WebServerController.DeleteAppVirtualDirectory(inst.WebSiteId, inst.VirtualDir);

                // delete folder
                FilesController.DeleteFiles(inst.PackageId, new string[] { inst[PROPERTY_CONTENT_PATH] });
            }

            // remove database
            if (inst[PROPERTY_DATABASE_CREATED] != null)
                DatabaseServerController.DeleteSqlDatabase(inst.DatabaseId);

            // remove database user
            if (inst[PROPERTY_USER_CREATED] != null)
                    DatabaseServerController.DeleteSqlUser(inst.UserId);
        }

        public static List<ApplicationCategory> GetCategories()
        {
            List<ApplicationCategory> categories = null;

            string key = "WebApplicationCategories";

            // look up in the cache
            if (HttpContext.Current != null)
                categories = (List<ApplicationCategory>)HttpContext.Current.Cache[key];

            if (categories == null)
            {
                string catsPath = Path.Combine(ConfigSettings.WebApplicationsPath, "Applications.xml");
                if (File.Exists(catsPath))
                {
                    categories = new List<ApplicationCategory>();

                    // parse file
                    XmlDocument doc = new XmlDocument();
                    doc.Load(catsPath);

                    XmlNodeList nodesCategories = doc.SelectNodes("categories/category");
                    foreach (XmlNode nodeCategory in nodesCategories)
                    {
                        ApplicationCategory category = new ApplicationCategory();
                        category.Id = nodeCategory.Attributes["id"].Value;
                        category.Name = GetNodeValue(nodeCategory, "name", category.Id);
                        categories.Add(category);

                        // read applications
                        List<string> catApps = new List<string>();
                        XmlNodeList nodesApps = nodeCategory.SelectNodes("applications/application");
                        foreach (XmlNode nodeApp in nodesApps)
                            catApps.Add(nodeApp.Attributes["name"].Value);
                        category.Applications = catApps.ToArray();
                    }
                }

                // place to the cache
                if (HttpContext.Current != null)
                    HttpContext.Current.Cache.Insert(key, categories, new CacheDependency(catsPath));
            }

            return categories;
        }

        public static List<ApplicationInfo> GetApplications(int packageId)
        {
            return GetApplications(packageId, null);
        }

        public static List<ApplicationInfo> GetApplications(int packageId, string categoryId)
        {
            string key = "WebApplicationsList";

            Dictionary<string, ApplicationInfo> apps = null;

            // look up in the cache
            if(HttpContext.Current != null)
                apps = (Dictionary<string, ApplicationInfo>)HttpContext.Current.Cache[key];

            if (apps == null)
            {
                // create apps list
                apps = new Dictionary<string, ApplicationInfo>();

                string appsRoot = ConfigSettings.WebApplicationsPath;
                string[] dirs = Directory.GetDirectories(appsRoot);
                foreach (string dir in dirs)
                {
                    string appFile = Path.Combine(dir, "Application.xml");

                    if (!File.Exists(appFile))
                        continue;

                    // read and parse web applications xml file
                    XmlDocument doc = new XmlDocument();
                    doc.Load(appFile);

                    XmlNode nodeApp = doc.SelectSingleNode("//application");

                    string appFolder = dir;

                    // parse node
                    ApplicationInfo app = CreateApplicationInfoFromXml(appFolder, nodeApp);

                    // add to the collection
                    apps.Add(app.Id, app);
                }

                // place to the cache
                if (HttpContext.Current != null)
                    HttpContext.Current.Cache.Insert(key, apps, new CacheDependency(appsRoot));
            }

            // filter applications based on category
            List<ApplicationInfo> categoryApps = new List<ApplicationInfo>();

            // check if the application fits requirements
            PackageContext cntx = PackageController.GetPackageContext(packageId);

            List<ApplicationCategory> categories = GetCategories();
            foreach (ApplicationCategory category in categories)
            {
                // skip category if required
                if (!String.IsNullOrEmpty(categoryId)
                    && String.Compare(category.Id, categoryId, true) != 0)
                    continue;

                // iterate through applications
                foreach (string appId in category.Applications)
                {
                    if (apps.ContainsKey(appId)
                        && IsApplicattionFitsRequirements(cntx, apps[appId]))
                        categoryApps.Add(apps[appId]);
                }
            }

            return categoryApps;
        }

        public static ApplicationInfo GetApplication(int packageId, string applicationId)
        {
            // get all applications
            List<ApplicationInfo> apps = GetApplications(packageId);

            // check if the application fits requirements
            PackageContext cntx = PackageController.GetPackageContext(packageId);

            // find the application
            foreach (ApplicationInfo app in apps)
            {
                if (app.Id.ToLower() == applicationId.ToLower())
                {
                    return IsApplicattionFitsRequirements(cntx, app) ? app : null;
                }
            }
            return null;
        }

        public static bool IsApplicattionFitsRequirements(PackageContext cntx, ApplicationInfo app)
        {
            if (app.Requirements == null)
                return true; // empty requirements

            foreach (ApplicationRequirement req in app.Requirements)
            {
                // check if this is a group
                if (req.Groups != null)
                {
                    bool groupFits = false;
                    foreach (string group in req.Groups)
                    {
                        if (cntx.Groups.ContainsKey(group))
                        {
                            groupFits = true;
                            break;
                        }
                    }

                    if (!groupFits)
                        return false;
                }

                // check if this is a quota
                if (req.Quotas != null)
                {
                    bool quotaFits = false;
                    foreach (string quota in req.Quotas)
                    {
                        if (cntx.Quotas.ContainsKey(quota) &&
                            !cntx.Quotas[quota].QuotaExhausted)
                        {
                            quotaFits = true;
                            break;
                        }
                    }

                    if (!quotaFits)
                        return false;
                }
            }

            return true;
        }

        #region private helper methods
        private static ApplicationInfo CreateApplicationInfoFromXml(string appFolder, XmlNode nodeApp)
        {
            ApplicationInfo app = new ApplicationInfo();

            // category name
            app.CategoryName = GetNodeValue(nodeApp, "category", "");

            // attributes
            app.Id = nodeApp.Attributes["id"].Value;
            app.Codebase = nodeApp.Attributes["codebase"].Value;
            app.SettingsControl = nodeApp.Attributes["settingsControl"].Value;
            app.Folder = appFolder;

            // child nodes
            app.Name = GetNodeValue(nodeApp, "name", "");
            app.ShortDescription = GetNodeValue(nodeApp, "shortDescription", "");
            app.FullDescription = GetNodeValue(nodeApp, "fullDescription", "");
            app.Logo = GetNodeValue(nodeApp, "logo", "");
            app.Version = GetNodeValue(nodeApp, "version", "");
            app.Size = Int32.Parse(GetNodeValue(nodeApp, "size", "0"));
            app.HomeSite = GetNodeValue(nodeApp, "homeSite", "");
            app.SupportSite = GetNodeValue(nodeApp, "supportSite", "");
            app.DocsSite = GetNodeValue(nodeApp, "docSite", "");

            app.Manufacturer = GetNodeValue(nodeApp, "manufacturer", "");
            app.License = GetNodeValue(nodeApp, "license", "");

            // process codebase path
            app.Codebase = Path.Combine(appFolder, app.Codebase);

            // web settings
            List<ApplicationWebSetting> settings = new List<ApplicationWebSetting>();
            XmlNodeList nodesWebSettings = nodeApp.SelectNodes("webSettings/add");
            foreach (XmlNode nodeSetting in nodesWebSettings)
            {
                ApplicationWebSetting setting = new ApplicationWebSetting();
                setting.Name = nodeSetting.Attributes["name"].Value;
                setting.Value = nodeSetting.Attributes["value"].Value;
                settings.Add(setting);
            }
            app.WebSettings = settings.ToArray();

            // requirements
            List<ApplicationRequirement> requirements = new List<ApplicationRequirement>();
            XmlNodeList nodesRequirements = nodeApp.SelectNodes("requirements/add");
            foreach (XmlNode nodesRequirement in nodesRequirements)
            {
                ApplicationRequirement req = new ApplicationRequirement();

                if (nodesRequirement.Attributes["group"] != null)
                    req.Groups = nodesRequirement.Attributes["group"].Value.Split('|');

                if (nodesRequirement.Attributes["quota"] != null)
                    req.Quotas = nodesRequirement.Attributes["quota"].Value.Split('|');

                req.Display = true;
                if (nodesRequirement.Attributes["display"] != null)
                    req.Display = Utils.ParseBool(nodesRequirement.Attributes["display"].Value, true);

                requirements.Add(req);
            }
            app.Requirements = requirements.ToArray();

            return app;
        }

        private static string GetNodeValue(XmlNode parentNode, string nodeName, string defaultValue)
        {
            XmlNode node = parentNode.SelectSingleNode(nodeName);
            if (node != null)
            {
                return node.InnerText.Trim();
            }

            // return default value
            return defaultValue;
        }
        #endregion
    }
}
