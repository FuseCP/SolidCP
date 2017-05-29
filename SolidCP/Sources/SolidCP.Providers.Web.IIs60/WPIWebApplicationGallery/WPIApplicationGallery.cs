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
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Web.Deployment;
using Microsoft.Web.PlatformInstaller;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Utils;
using SolidCP.Providers.WebAppGallery;
using SolidCP.Server.Code;
using SolidCP.Server.Utils;
using System.Web;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using DeploymentParameter = SolidCP.Providers.WebAppGallery.DeploymentParameter;
using DeploymentParameterWPI = Microsoft.Web.PlatformInstaller.DeploymentParameter;

namespace SolidCP.Providers.Web.WPIWebApplicationGallery
{
    internal class WPIApplicationGallery
    {
        
        private CacheManager _cache;
        private const int LIVE_IN_CACHE_MINUTES = 20;
        private string _sufix="";

        public WPIApplicationGallery(string sufix)
        {
            _sufix = sufix;
            _cache = CacheFactory.GetCacheManager();
        }



        private string GetKey_Feeds(int UserId)
        {
            return "WPIHELPER_CACHE_FEEDS" + UserId.ToString();
        }

        private string GetKey_object(int UserId)
        {
            return "WPIHELPER_CACHE_OBJECTS" + UserId.ToString() + _sufix;
        }

        public void InitFeeds(int UserId, string[] feeds)
        {
            string CACHE_KEY = GetKey_Feeds(UserId);

            if (_cache.Contains(CACHE_KEY))
            {
                string[] oldfeeds = (string[])_cache[CACHE_KEY];

                if (!ArraysEqual<string>(feeds, oldfeeds))
                {
                    //Feeeds have been changed
                    ICacheItemExpiration exp = new SlidingTime(TimeSpan.FromMinutes(LIVE_IN_CACHE_MINUTES*2));
                    _cache.Add(CACHE_KEY, feeds, CacheItemPriority.Normal, null, exp );

                    DeleteWpiHelper(UserId);
                }

            }
            else
            {
                //add to cache
                ICacheItemExpiration exp = new SlidingTime(TimeSpan.FromMinutes(LIVE_IN_CACHE_MINUTES*2));
                _cache.Add(CACHE_KEY, feeds, CacheItemPriority.Normal, null, exp);

            }

        }

        private WpiHelper GetWpiHelper(int UserId)
        {
            string CACHE_KEY = GetKey_object(UserId);

            if (_cache.Contains(CACHE_KEY))
            {
                WpiHelper result = (WpiHelper)_cache[CACHE_KEY];

                if (result != null)
                {
                    return result;
                }
             
            }

            string[] feeds = (string[])_cache[GetKey_Feeds(UserId)];
            if (null == feeds)
            {
                throw new Exception("BUG:No feeds in cache.");
            }

            WpiHelper wpi = new WpiHelper(feeds);
            ICacheItemExpiration exp = new SlidingTime(TimeSpan.FromMinutes(LIVE_IN_CACHE_MINUTES));

            _cache.Add(CACHE_KEY, wpi, CacheItemPriority.Normal, null, exp);
            //Debug.WriteLine(string.Format("GetWpiHelper: put in cache. User {0}", UserId));

            return wpi;
        }

        public void DeleteWpiHelper(int UserId)
        {
            _cache.Remove(GetKey_object(UserId));
        }


        #region Public methods

        public void SetResourceLanguage(int UserId, string resourceLanguage)
        {
            WpiHelper wpi = GetWpiHelper(UserId);
            wpi.SetResourceLanguage(resourceLanguage);
        }


        public List<SettingPair> GetLanguages(int UserId)
        {
            List<SettingPair> langs = new List<SettingPair>();
            WpiHelper wpi = GetWpiHelper(UserId);
            foreach (Language lang in wpi.GetLanguages())
            {
                langs.Add(new SettingPair(lang.Id, lang.Name));
            }

            return langs;
        }

        public List<GalleryCategory> GetCategories(int UserId)
        {
            WpiHelper wpi = GetWpiHelper(UserId);

            List<GalleryCategory> categories = new List<GalleryCategory>();

            foreach (Keyword keyword in wpi.GetKeywords())
            {

                if (wpi.IsKeywordApplication(keyword) && !wpi.IsHiddenKeyword(keyword))
                {
                    categories.Add(new GalleryCategory
                    {
                        Id = keyword.Id,
                        Name = keyword.Text
                    });

                }
            }

            return categories;
        }

        public List<GalleryApplication> GetApplications(int UserId, string categoryId)
        {
            WpiHelper wpi = GetWpiHelper(UserId);

            List<Product> products = wpi.GetApplications(categoryId);
            List<GalleryApplication> applications = new List<GalleryApplication>();

            try
            {
                foreach (Product product in products)
                {
                    applications.Add(MakeGalleryApplicationFromProduct(product));
                }
            }
            catch(Exception)
            {
                //
            }

            return applications;
        }



        public List<GalleryApplication> GetGalleryApplicationsFiltered(int UserId, string pattern)
        {
            WpiHelper wpi = GetWpiHelper(UserId);

            List<Product> products = wpi.GetApplications(null);
            List<GalleryApplication> applications = new List<GalleryApplication>();

            foreach (Product product in products)
            {
                if (product.Title.ToLower().Contains(pattern.ToLower()))
                {
                    applications.Add(MakeGalleryApplicationFromProduct(product));
                }
                
            }

            return applications;
        }



        public GalleryApplication GetApplicationByProductId(int UserId, string id)
        {
            WpiHelper wpi = GetWpiHelper(UserId);

            return MakeGalleryApplicationFromProduct(wpi.GetProduct(id));
        }

        public List<string> GetMissingDependenciesForApplicationById(int UserId, string id)
        {
            WpiHelper wpi = GetWpiHelper(UserId);

            List<string> missingDeps = new List<string>();
            foreach (Product product in wpi.GetProductsToInstallWithDependencies(new string[] { id }))
            {
                if (product.ProductId != id)
                {
                    missingDeps.Add(product.Title);
                }
            }

            return missingDeps;
        }

        public GalleryWebAppStatus DownloadAppAndGetStatus(int UserId, string id)
        {
            WpiHelper wpi = GetWpiHelper(UserId);
            wpi.InstallProducts(
                new[] { id }, 
                false, // do not install dependencies
                null, 
                null, 
                null
                );

            return GalleryWebAppStatus.Downloaded;
        }

        public List<DeploymentParameter> GetApplicationParameters(int UserId, string id)
        {
            WpiHelper wpi = GetWpiHelper(UserId);

            Product product = wpi.GetProduct(id);
            List<DeploymentParameter> deploymentParameters = new List<DeploymentParameter>();
            IList<DeploymentParameterWPI> appDeploymentWPIParameters = wpi.GetAppDecalredParameters(id);
            foreach (DeploymentParameterWPI deploymentParameter in appDeploymentWPIParameters)
            {
                deploymentParameters.Add(MakeDeploymentParameterFromDecalredParameter(deploymentParameter));
            }

            return deploymentParameters;

        }

        public void InstallApplication(
            int UserId, 
            string webAppId, 
            List<DeploymentParameter> updatedParameters, 
            string languageId, 
            ref StringResultObject result)
        {
            WpiHelper wpi = GetWpiHelper(UserId);

            // convert list of DeploymentParameter to list of WpiUpdatedDeploymentParameter
            List<WpiUpdatedDeploymentParameter> updatedWpiParameters = new List<WpiUpdatedDeploymentParameter>();
            foreach (DeploymentParameter updatedParameter in updatedParameters)
            {
                updatedWpiParameters.Add(
                    new WpiUpdatedDeploymentParameter
                    {
                        Name = updatedParameter.Name,
                        Value = updatedParameter.Value,
                        WellKnownTags = (DeploymentWellKnownTag)updatedParameter.WellKnownTags
                    }
                );
            }

            Log.WriteStart("Application installation starting");
            string log;
            string failedMessage;
            bool success = wpi.InstallApplication(
                webAppId, 
                updatedWpiParameters, 
                languageId, 
                InstallStatusUpdatedHandler, InstallCompleteHandler, 
                out log,
                out failedMessage);

            result.Value = log;
            result.IsSuccess = success;

            // add log files to result value
            try
            {
                StringBuilder sb = new StringBuilder();
                string[] filePaths = Directory.GetFiles(wpi.GetLogFileDirectory());
                foreach (string filePath in filePaths)
                {
                    using (StreamReader streamReader = new StreamReader(filePath))
                    {
                        string fileContent =
                            SecurityElement.Escape(StringUtils.CleanupASCIIControlCharacters(streamReader.ReadToEnd()));
                        sb.AppendLine().AppendLine(filePath).AppendLine(fileContent);
                    }

                }

                result.Value += sb.ToString();
            }
            catch(Exception)
            {
            }


            if (!success)
            {
                result.AddError(failedMessage, null);
            }
            
            // don`t reuse wpi helper after installation
            DeleteWpiHelper(UserId);
        }

        #endregion

        #region installaton events

        private void InstallStatusUpdatedHandler(object sender, InstallStatusEventArgs installStatusEventArgs)
        {
            Log.WriteInfo("Application {0} installation status: {1}, return code: {0}", 
                installStatusEventArgs.InstallerContext.ProductName, 
                installStatusEventArgs.InstallerContext.InstallationState,
                installStatusEventArgs.InstallerContext.ReturnCode);
        }

        private void InstallCompleteHandler(object sender, EventArgs eventArgs)
        {
           Log.WriteEnd("Application installation completed");
        }

        #endregion 


        #region static helpers

        protected static GalleryApplication MakeGalleryApplicationFromProduct(Product product)
        {
            if (null == product)
            {
                return null;
            }

            int size = 0;
            if (null != product.Installers && product.Installers.Count > 0 && null != product.Installers[0].InstallerFile)
            {
                size = product.Installers[0].InstallerFile.FileSize;
            }

            return new GalleryApplication
                       {
                           Id = product.ProductId,
                           Title = product.Title,
                           Author = new Author {Name = product.Author, Uri = product.AuthorUri.ToString()},
                           IconUrl = null == product.IconUrl ? "" : product.IconUrl.ToString(),
                           Version = product.Version,
                           Description = product.LongDescription,
                           Summary = product.Summary,
                           LastUpdated = product.Published,
                           Published = product.Published,
                           Link = (null==product.Link) ? "" :product.Link.ToString(),
                           InstallerFileSize = size,
                           Keywords = product.Keywords.Select(keyword => keyword.Id).ToList()
                       };
        }

        protected static DeploymentParameter MakeDeploymentParameterFromDecalredParameter(DeploymentParameterWPI d)
        {
            DeploymentParameter r = new DeploymentParameter();
            r.Name = d.Name;
            r.FriendlyName = d.FriendlyName;
            r.DefaultValue = d.DefaultValue;
            r.Description = d.Description;

            r.SetWellKnownTagsFromRawString(d.RawTags);
            if (!string.IsNullOrEmpty(d.ValidationString))
            {
                // synchronized with Microsoft.Web.Deployment.DeploymentSyncParameterValidationKind
                if (d.HasValidation((int)DeploymentParameterValidationKind.AllowEmpty))
                {
                    r.ValidationKind |= DeploymentParameterValidationKind.AllowEmpty;
                }
                if (d.HasValidation((int)DeploymentParameterValidationKind.RegularExpression))
                {
                    r.ValidationKind |= DeploymentParameterValidationKind.RegularExpression;
                }
                if (d.HasValidation((int)DeploymentParameterValidationKind.Enumeration))
                {
                    r.ValidationKind |= DeploymentParameterValidationKind.Enumeration;
                }
                if (d.HasValidation((int)DeploymentParameterValidationKind.Boolean))
                {
                    r.ValidationKind |= DeploymentParameterValidationKind.Boolean;
                }

                r.ValidationString = d.ValidationString;
            }
            else
            {
                r.ValidationKind = DeploymentParameterValidationKind.None;
            }

            return r;
        }

        protected static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }


        #endregion
    }
}
