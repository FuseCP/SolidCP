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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Web.Deployment;
using Microsoft.Web.PlatformInstaller;
using Installer = Microsoft.Web.PlatformInstaller.Installer;
using DeploymentParameterWPI = Microsoft.Web.PlatformInstaller.DeploymentParameter;

namespace SolidCP.Server.Code
{
    public class WpiUpdatedDeploymentParameter
    {
        public string Name;
        public string Value;
        public DeploymentWellKnownTag WellKnownTags;
    }

    public class WpiHelper
    {
        public const string DeafultLanguage = "en";

        #region private fields

        private List<string> _feeds;
        private string _webPIinstallersFolder;
        private const string IisChoiceProduct = "StaticContent";
        private const string WebMatrixChoiceProduct = "WebMatrix";
        private ProductManager _productManager;
        private bool _installCompleted;
        private InstallManager _installManager;
        private string _LogFileDirectory = string.Empty;
        private string _resourceLanguage = DeafultLanguage;
        private const DeploymentWellKnownTag databaseEngineTags =
                    DeploymentWellKnownTag.Sql |
                    DeploymentWellKnownTag.MySql |
                    DeploymentWellKnownTag.SqLite |
                    DeploymentWellKnownTag.VistaDB |
                    DeploymentWellKnownTag.FlatFile;

        #endregion private fields

        #region Public interface

        public WpiHelper(IEnumerable<string> feeds)
        {
            // check feeds is not empty
            if (null == feeds || !feeds.Any())
            {
                throw new Exception("WpiHelper error: empty feed list in constructor");
            }

            
            // by default feeds must contains main MS WPI feed url and Zoo feed url
            _feeds = new List<string>();
            _feeds.AddRange(feeds);

            Initialize();
        }

        public string GetLogFileDirectory()
        {
            return _LogFileDirectory;
        }


        
        #region Languages
        public List<Language> GetLanguages()
        {
            List<Language> languages = new List<Language>();

            foreach (Product product in GetProductsToInstall(null, null))
            {
                if (null!=product.Installers)
                {
                    foreach (Installer installer in product.Installers)
                    {
                        Language lang = installer.Language;
                        if (null!=lang && !languages.Contains(lang))
                        {
                            languages.Add(lang);
                        }
                    }
                }
            }

            return languages;
        }

        public void SetResourceLanguage(string resourceLanguage)
        {
            _resourceLanguage = resourceLanguage;
            _productManager.SetResourceLanguage(resourceLanguage);
        }

        #endregion

        #region Tabs
        public ReadOnlyCollection<Tab> GetTabs()
        {
            return _productManager.Tabs;
        }

        public Tab GetTab(string tabId)
        {
            return _productManager.GetTab(tabId);
        }
       #endregion

        #region Keywords
        public ReadOnlyCollection<Keyword> GetKeywords()
        {
            return _productManager.Keywords;
        }

        public bool IsKeywordApplication(Keyword keyword)
        {
            //if all products are Application
            foreach (Product product in keyword.Products)
            {
                if (!product.IsApplication)
                {
                    return false;
                }
            }

            return true;

        }

        public bool IsHiddenKeyword(Keyword keyword)
        {
            if (keyword.Id.StartsWith("ZooEngine", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Products
        public List<Product> GetProductsToInstall(string FeedLocation, string keywordId)
        {
            Keyword keyword = null;
            List<Product> products = new List<Product>();

            if (!string.IsNullOrEmpty(keywordId))
            {
                keyword = _productManager.GetKeyword(keywordId);
            }

            // if we do not find keyword object by keyword string
            // then return empty list
            if (null == keyword && !string.IsNullOrEmpty(keywordId))
            {
                WriteLog(string.Format("Keyword '{0}' not found, return empty product list", keywordId));
                return products;
            }


            foreach (Product product in _productManager.Products)
            {
                if (!string.IsNullOrEmpty(FeedLocation) && string.Compare(product.FeedLocation, FeedLocation, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    // if FeedLocation defined, then select products only from this feed location
                    continue;
                }

                if (null == product.Installers || product.Installers.Count == 0)
                {
                    // skip this product
                    // usually product without intsallers user as product detection
                    continue;
                }

                if (null == keyword)
                {
                    products.Add(product);
                }
                else if (product.Keywords.Contains(keyword))
                {
                    products.Add(product);
                }
            }

            //Sort by Title
            products.Sort(delegate(Product a, Product b)
            {
                return a.Title.CompareTo(b.Title);
            });

            return products;
        }

        public List<Product> GetProductsFiltered(string filter)
        {

            List<Product> products = new List<Product>();

            foreach (Product product in _productManager.Products)
            {
                if (null == product.Installers || product.Installers.Count == 0)
                {
                    // skip this product
                    // usually product without intsallers user as product detection
                    continue;
                }

                if (product.GetAttributeValue("searchExclude") != null)
                {
                    // skip it, this is internal not visible product 
                    continue;
                }

                if (string.IsNullOrEmpty(filter))
                {
                    products.Add(product);
                }
                else if (product.Title.ToLower().Contains(filter.ToLower()))
                {
                    products.Add(product);
                }
            }

            //Sort by Title
            products.Sort(delegate(Product a, Product b)
            {
                return a.Title.CompareTo(b.Title);
            });


            return products;
        }

        public Product GetWPIProductById(string productId)
        {
            foreach (Product product in _productManager.Products)
            {
                if (0 == String.Compare(product.ProductId, productId, StringComparison.OrdinalIgnoreCase))
                {
                    return product;
                }

            }

            return null; //not found
        }
        
        public List<Product> GetProductsToInstall(IEnumerable<string> productIdsToInstall)
        {
            List<Product> productsToInstall = new List<Product>();

            foreach (string productId in productIdsToInstall)
            {
                WriteLog(string.Format("Product {0} to be installed", productId));

                Product product = _productManager.GetProduct(productId);
                if (null == product)
                {
                    WriteLog(string.Format("Product {0} not found", productId));
                    continue;
                }
                if (product.IsInstalled(true))
                {
                    WriteLog(string.Format("Product {0} is installed", product.Title));
                }
                else
                {
                    WriteLog(string.Format("Adding product {0}", product.Title));

                    productsToInstall.Add(product);
                }
            }

            return productsToInstall;
        }
        
        public List<Product> GetProductsToInstallWithDependencies(IEnumerable<string> productIdsToInstall )
        {
            List<string> updatedProductIdsToInstall = new List<string>();
            // add iis chioce product to force iis (not-iisexpress/webmatrix) branch
            updatedProductIdsToInstall.Add(IisChoiceProduct);
            updatedProductIdsToInstall.AddRange(productIdsToInstall);

            List<Product> productsToInstall = new List<Product>();

            foreach (string productId in updatedProductIdsToInstall)
            {
                WriteLog(string.Format("Product {0} to be installed", productId));

                Product product = _productManager.GetProduct(productId);
                if (null == product)
                {
                    WriteLog(string.Format("Product {0} not found", productId));
                    continue;
                }
                if (product.IsInstalled(true))
                {
                    WriteLog(string.Format("Product {0} is installed", product.Title));
                }
                else
                {
                    WriteLog(string.Format("Adding product {0} with dependencies", product.Title));
                    // search and add dependencies but skip webmatrix/iisexpress branches
                    AddProductWithDependencies(product, productsToInstall, WebMatrixChoiceProduct);
                }
            }

            return productsToInstall;
        }

        public Product GetProduct(string productId)
        {
            return _productManager.GetProduct(productId);
        }

        public void InstallProducts(
            IEnumerable<string> productIdsToInstall,
            bool installDependencies,
            string languageId,
            EventHandler<InstallStatusEventArgs> installStatusUpdatedHandler,
            EventHandler<EventArgs> installCompleteHandler)
        {

            List<Product> productsToInstall = null;
            if (installDependencies)
            {
                // Get products & dependencies list to install
                productsToInstall = GetProductsToInstallWithDependencies(productIdsToInstall);
            }
            else
            {
                productsToInstall = GetProductsToInstall(productIdsToInstall);
            }



            // Get installers
            Language lang = GetLanguage(languageId);
            List<Installer> installersToUse = GetInstallers(productsToInstall, lang);


            // Prepare install manager & set event handlers
            _installManager = new InstallManager();
            _installManager.Load(installersToUse);


            if (null != installStatusUpdatedHandler)
            {
                _installManager.InstallerStatusUpdated += installStatusUpdatedHandler;
            }
            _installManager.InstallerStatusUpdated += InstallManager_InstallerStatusUpdated;

            if (null != installCompleteHandler)
            {
                _installManager.InstallCompleted += installCompleteHandler;
            }
            _installManager.InstallCompleted += InstallManager_InstallCompleted;

            // Download installer files
            foreach (InstallerContext installerContext in _installManager.InstallerContexts)
            {
                if (null != installerContext.Installer.InstallerFile)
                {
                    string failureReason;
                    if (!_installManager.DownloadInstallerFile(installerContext, out failureReason))
                    {
                        WriteLog(string.Format("DownloadInstallerFile '{0}' failed: {1}",
                                          installerContext.Installer.InstallerFile.InstallerUrl, failureReason));

                        throw new Exception(
                                          string.Format("Can't install {0}  DownloadInstallerFile '{1}' failed: {2}",
                                          installerContext.ProductName,
                                          installerContext.Installer.InstallerFile.InstallerUrl, 
                                          failureReason)
                                          );
                    }
                }
            }

            if (installersToUse.Count > 0)
            {
                // Start installation
                _installCompleted = false;
                _installManager.StartInstallation();

                while (!_installCompleted)
                {
                    Thread.Sleep(100);
                }

                //save logs
                SaveLogDirectory();


                _installCompleted = false;
            }
            else
            {
                //Log("Nothing to install");
            }

        }

        public void CancelInstallProducts()
        {
            if (_installManager != null)
            {
                _installManager.Cancel();
            }
        }

        #endregion

        #region Applications
        public List<Product> GetApplications(string keywordId)
        {

            Keyword keyword = null;
            if (!string.IsNullOrEmpty(keywordId))
            {
                keyword = _productManager.GetKeyword(keywordId);
            }



            List<Product> products = new List<Product>();

            Language lang = GetLanguage(_resourceLanguage);
            Language langDefault = GetLanguage(DeafultLanguage);

            foreach (Product product in _productManager.Products)
            {
                if (!product.IsApplication)
                {
                    // skip
                    continue;
                }

                //Check language
                if (
                    lang.AvailableProducts.Contains(product) ||
                    langDefault.AvailableProducts.Contains(product)
                    )
                {
                    if (null == keyword)
                    {
                        products.Add(product);
                    }
                    else if (product.Keywords.Contains(keyword))
                    {
                        products.Add(product);
                    }

                }

            }

            //Sort by Title
            products.Sort(delegate(Product a, Product b)
            {
                return a.Title.CompareTo(b.Title);
            });


            return products;
        }

        public IList<DeploymentParameterWPI> GetAppDecalredParameters(string productId)
        {
            Product app = _productManager.GetProduct(productId);
            Installer appInstaller = app.GetInstaller(GetLanguage(null));
            return appInstaller.MSDeployPackage.DeploymentParameters;
        }

        public bool InstallApplication(
            string appId,
            List<WpiUpdatedDeploymentParameter> updatedValues, 
            string languageId,
            EventHandler<InstallStatusEventArgs> installStatusUpdatedHandler,
            EventHandler<EventArgs> installCompleteHandler,
            out string log,
            out string failedMessage
            )
        {

            Product app = GetProduct(appId);
            Installer appInstaller = GetInstaller(languageId, app);
            WpiAppInstallLogger logger = new WpiAppInstallLogger();

            /*
            if (null == _installManager)
            {
                Debugger.Break();
            }
            */

            if (null != installStatusUpdatedHandler)
            {
                _installManager.InstallerStatusUpdated += installStatusUpdatedHandler;
            }
            _installManager.InstallerStatusUpdated += logger.HanlderInstallerStatusUpdated;

            if (null != installCompleteHandler)
            {
                _installManager.InstallCompleted += installCompleteHandler;
            }
            _installManager.InstallCompleted += logger.HandlerInstallCompleted;

            // set updated parameters
            foreach (WpiUpdatedDeploymentParameter parameter in updatedValues)
            {
                if (!string.IsNullOrEmpty(parameter.Value))
                {
                    appInstaller.MSDeployPackage.SetParameters[parameter.Name] = parameter.Value;
                }
            }

            DeploymentWellKnownTag dbTag = (DeploymentWellKnownTag)GetDbTag(updatedValues);

            // remove parameters with alien db tags
            foreach (DeploymentParameterWPI parameter in appInstaller.MSDeployPackage.DeploymentParameters)
            {
                if (IsAlienDbTaggedParameter(dbTag, parameter))
                {
                    appInstaller.MSDeployPackage.RemoveParameters.Add(parameter.Name);
                }
            }

            // skip alien directives
            RemoveUnusedProviders(appInstaller.MSDeployPackage, dbTag);

            _installCompleted = false;

            _installManager.StartApplicationInstallation();
            while (!_installCompleted)
            {
                Thread.Sleep(1000);
            }

            WriteLog("InstallApplication complete");

            //save logs
            SaveLogDirectory();

            _installCompleted = false;

            log = logger.GetLog();
            failedMessage = logger.FailedMessage;
            
            return !logger.IsFailed;
        }

        private Installer GetInstaller(string languageId, Product product)
        {
            Installer installer = product.GetInstaller(GetLanguage(languageId));
            if (null == installer)
            {
                installer = product.GetInstaller(GetLanguage(DeafultLanguage));
                if (null == installer)
                {
                    throw new Exception(
                        string.Format(
                        "Could not get installer for product '{0}', language: {1}, default language: {2}",
                        product.Title, languageId, DeafultLanguage)
                    );
                }
            }

            return installer;
        }

        #endregion

        #endregion Public interface


        #region private members

        private void Initialize()
        {
            // create cache folder if not exists
            _webPIinstallersFolder = Path.Combine(
                Environment.ExpandEnvironmentVariables("%SystemRoot%"),
                "Temp\\zoo.wpi\\AppData\\Local\\Microsoft\\Web Platform Installer\\installers");

            if (!Directory.Exists(_webPIinstallersFolder))
            {
                Directory.CreateDirectory(_webPIinstallersFolder);
            }

            LoadFeeds();

            WriteLog(string.Format("{0} products loaded", _productManager.Products.Count));

            //LogDebugInfo();
        }

        private void LoadFeeds()
        {
            if (null == _feeds || !_feeds.Any())
            {
                throw new Exception("WpiHelper: no feeds provided");
            }

            _productManager = new ProductManager();

            try
            {
                TryLoadFeeds();
                // ok, all feeds loaded
            }
            catch(Exception ex1)
            {
                // feed loading failed

                if (_feeds.Count > 1)
                {
                    // exclude feeds except first (default microsoft feed)
                    _feeds = new List<string> {_feeds[0]};

                    // re-create product manager
                    _productManager = new ProductManager();

                    try
                    {
                        // loaded first (default) feed only
                        TryLoadFeeds();
                    }
                    catch(Exception ex2)
                    {
                        throw new Exception(string.Format("WpiHelper: download first feed failed: {0}", ex2), ex2);
                    }
                }
                else
                {
                    throw new Exception(string.Format("WpiHelper: download all ({0}) feeds failed: {1}", _feeds.Count, ex1), ex1);
                }
            }
        }

        private void TryLoadFeeds()
        {
            string loadingFeed = null;

            try
            {
                foreach (string feed in _feeds)
                {
                    loadingFeed = feed;
                    WriteLog(string.Format("Loading feed {0}", feed));
                    if (feed.IndexOf("microsoft.com", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        // it is internal Microsoft feed
                        _productManager.Load(new Uri(feed), true, true, true, _webPIinstallersFolder);
                    }
                    else if (feed.IndexOf("windows.net", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        // it is internal Microsoft feed
                        _productManager.Load(new Uri(feed), true, true, true, _webPIinstallersFolder);
                    }
                    else if(feed.IndexOf("fusecp.com", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        // it is FuseCP feed
                        _productManager.Load(new Uri(feed), true, true, true, _webPIinstallersFolder);
                    }
                    else
                    {
                        _productManager.LoadExternalFile(new Uri(feed));
                    }
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(loadingFeed))
                {
                    // error occured on loading this feed
                    // log this
                    WriteLog(string.Format("Feed {0} loading error: {1}", loadingFeed, ex));
                }
                throw;
            }
        }

        private Language GetLanguage(string languageId)
        {
            if (!string.IsNullOrEmpty(languageId))
            {
                return _productManager.GetLanguage(languageId);
            }

            return _productManager.GetLanguage(DeafultLanguage);
        }


        private List<Installer> GetInstallers(List<Product> productsToInstall, Language lang)
        {
            Language defaultLang = GetLanguage(DeafultLanguage);
            List<Installer> installersToUse = new List<Installer>();
            foreach (Product product in productsToInstall)
            {
                Installer installer = product.GetInstaller(lang) ?? product.GetInstaller(defaultLang);
                if (null != installer)
                {
                    installersToUse.Add(installer);
                }
            }

            return installersToUse;
        }


        //private void LogDebugInfo()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("Products: ");

        //    sb.Append("Tabs: ").AppendLine();
        //    foreach (Tab tab in _productManager.Tabs)
        //    {
        //        sb.AppendFormat("\t{0}, FromCustomFeed = {1}", tab.Name, tab.FromCustomFeed).AppendLine();
        //        foreach (string f in tab.FeedList)
        //        {
        //            sb.AppendFormat("\t\t{0}", f).AppendLine();
        //        }
        //        sb.AppendLine();
        //    }
        //    sb.AppendLine();

        //    sb.Append("Keywords: ").AppendLine().Append("\t");
        //    foreach (Keyword keyword in _productManager.Keywords)
        //    {
        //        sb.Append(keyword.Id).Append(",");
        //    }
        //    sb.AppendLine();

        //    sb.Append("Languages: ").AppendLine().Append("\t");
        //    foreach (Language language in _productManager.Languages)
        //    {
        //        sb.Append(language.Name).Append(",");
        //    }
        //    sb.AppendLine();

        //    Log(sb.ToString());
        //}

        private static void WriteLog(string message)
        {
            Debug.WriteLine(string.Format("[{0}] WpiHelper: {1}", Process.GetCurrentProcess().Id, message));
        }

        private void InstallManager_InstallCompleted(object sender, EventArgs e)
        {
            if (null != _installManager)
            {
                /*
                try
                {
                    _installManager.Dispose();
                } catch(Exception ex)
                {
                    Log("InstallManager_InstallCompleted Exception: "+ex.ToString());
                }
                _installManager = null;
                */
            }
            _installCompleted = true;
        }

        private void InstallManager_InstallerStatusUpdated(object sender, InstallStatusEventArgs e)
        {
            //Log(string.Format("{0}: {1}. {2} Progress: {3}",
            //    e.InstallerContext.ProductName,
            //    e.InstallerContext.InstallationState,
            //    e.InstallerContext.ReturnCode.DetailedInformation,
            //    e.ProgressValue));
        }

        private static void AddProductWithDependencies(Product product, List<Product> productsToInstall, string skipProduct)
        {
            if (!productsToInstall.Contains(product))
            {
                productsToInstall.Add(product);
            }

            ICollection<Product> missingDependencies = product.GetMissingDependencies(productsToInstall);
            if (missingDependencies != null)
            {
                foreach (Product dependency in missingDependencies)
                {
                    if (string.Equals(dependency.ProductId, skipProduct, StringComparison.OrdinalIgnoreCase))
                    {
                        //Log(string.Format("Product {0} is iis express dependency, skip it", dependency.Title));
                        continue;
                    }

                    AddProductWithDependencies(dependency, productsToInstall, skipProduct);
                }
            }
        }

        private void SaveLogDirectory()
        {
            foreach (InstallerContext ctx in _installManager.InstallerContexts)
            {
                _LogFileDirectory = ctx.LogFileDirectory;
                break;
            }
        }

        private DeploymentWellKnownTag GetDbTag(List<WpiUpdatedDeploymentParameter> parameters)
        {
            foreach (WpiUpdatedDeploymentParameter parameter in parameters)
            {
                if ((parameter.WellKnownTags & databaseEngineTags) != 0)
                {
                    return (DeploymentWellKnownTag)Enum.Parse(
                        typeof(DeploymentWellKnownTag),
                        (parameter.WellKnownTags & databaseEngineTags).ToString().Split(',')[0]);
                }
            }

            return DeploymentWellKnownTag.None;
        }

        private static bool IsAlienDbTaggedParameter(DeploymentWellKnownTag dbTag, DeploymentParameterWPI parameter)
        {
            return parameter.HasTags((long)databaseEngineTags) && !parameter.HasTags((long)dbTag);
/*
#pragma warning disable 612,618
            return (parameter.Tags & databaseEngineTags) != DeploymentWellKnownTag.None 
                   && 
                   (parameter.Tags & dbTag) == DeploymentWellKnownTag.None;
#pragma warning restore 612,618
*/
        }

        private static void RemoveUnusedProviders(MSDeployPackage msDeployPackage, DeploymentWellKnownTag dbTag)
        {
            List<string> providersToRemove = new List<string>();

            switch (dbTag)
            {
                case DeploymentWellKnownTag.MySql:
                    providersToRemove.Add("dbFullSql");
                    providersToRemove.Add("DBSqlite");
                    break;
                case DeploymentWellKnownTag.Sql:
                    providersToRemove.Add("dbMySql");
                    providersToRemove.Add("DBSqlite");
                    break;
                case DeploymentWellKnownTag.FlatFile:
                    providersToRemove.Add("dbFullSql");
                    providersToRemove.Add("DBSqlite");
                    providersToRemove.Add("dbMySql");
                    break;
                case DeploymentWellKnownTag.SqLite:
                    providersToRemove.Add("dbFullSql");
                    providersToRemove.Add("dbMySql");
                    break;
                case DeploymentWellKnownTag.VistaDB:
                    providersToRemove.Add("dbFullSql");
                    providersToRemove.Add("DBSqlite");
                    providersToRemove.Add("dbMySql");
                    break;
                case DeploymentWellKnownTag.SqlCE:
                    providersToRemove.Add("dbFullSql");
                    providersToRemove.Add("DBSqlite");
                    providersToRemove.Add("dbMySql");
                    break;
                default:
                    break;
            }

            foreach (string provider in providersToRemove)
            {
                msDeployPackage.SkipDirectives.Add(string.Format("objectName={0}", provider));
            }
        }


        #endregion private members
    }

    class WpiAppInstallLogger
    {
        private StringBuilder sb;
        private InstallReturnCode _installReturnCode;
        private string _failedMessage = string.Empty;

        public WpiAppInstallLogger()
        {
            sb = new StringBuilder();
        }

        public InstallReturnCode ReturnCode
        {
            get { return _installReturnCode; }
        }

        public string FailedMessage
        {
            get { return _failedMessage; }
        }

        public bool IsFailed
        {
            get
            {
                if (null != _installReturnCode)
                {
                    return _installReturnCode.Status == InstallReturnCodeStatus.Failure ||
                           _installReturnCode.Status == InstallReturnCodeStatus.FailureRebootRequired;
                }
                return false;
            }
        }

        public void HanlderInstallerStatusUpdated(object sender, InstallStatusEventArgs e)
        {
            sb.AppendFormat("{0}: {1}. {2} Progress: {3}",
                            e.InstallerContext.ProductName,
                            e.InstallerContext.InstallationState,
                            e.InstallerContext.ReturnCode.DetailedInformation,
                            e.ProgressValue).AppendLine();
        }

        public void HandlerInstallCompleted(object sender, EventArgs e)
        {
            InstallManager installManager = sender as InstallManager;
            if (null != installManager)
            {
                InstallerContext installerContext;
                if (null != installManager.InstallerContexts && installManager.InstallerContexts.Count>0)
                {
                    installerContext = installManager.InstallerContexts[0];
                    _installReturnCode = installerContext.ReturnCode;
                }
            }

            if (null != _installReturnCode)
            {
                _failedMessage = string.Format("{0}: {1}",
                                               _installReturnCode.Status,
                                               _installReturnCode.DetailedInformation);
                sb.AppendFormat("Return Code: {0}", _failedMessage).AppendLine();
            }
            sb.AppendLine("Installation completed");
        }

        public string GetLog()
        {
            return sb.ToString();
        }
    }
}
