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
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.HeliconZoo;
using SolidCP.Providers.ResultObjects;
using SolidCP.Providers.Web;
using SolidCP.Providers.WebAppGallery;
using SolidCP.WebPortal;
using System.Reflection;

namespace SolidCP.Portal
{
    public class ShortHeliconZooEngineComparer:IComparer<ShortHeliconZooEngine>
    {
        public int Compare(ShortHeliconZooEngine x, ShortHeliconZooEngine y)
        {
            return string.Compare(x.DisplayName, y.DisplayName, StringComparison.OrdinalIgnoreCase);
        }
    }

    public partial class WebSitesHeliconZooControl : SolidCPControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindWebItem(WebSite site)
        {
            ViewState["WebSiteId"] = site.SiteId;
            ViewState["WebSitePackageId"] = site.PackageId;

            try
            {
                BindEngines(site);
                BindInstalledApplications();
            }
            catch (AmbiguousMatchException)
            {
                lblConsole.Text = "Zoo Module is not installed. Please ask your system administrator to install Zoo on the server to Configuration\\Server\\Web Application Engines.";
                lblConsole.ForeColor = Color.Red;
                lblConsole.Font.Size = 16;

                return; // Exit
            }
            
            BindApplications();
        }

        private void BindEngines(WebSite site)
        {
            // get allowed engines for current hosting plan
            ShortHeliconZooEngine[] allowedEngineArray =
                ES.Services.HeliconZoo.GetAllowedHeliconZooQuotasForPackage(site.PackageId);
            Array.Sort(allowedEngineArray, new ShortHeliconZooEngineComparer());


            // get enabled engines for this site from applicationHost.config
            string[] enabledEngineNames = ES.Services.HeliconZoo.GetEnabledEnginesForSite(site.SiteId, site.PackageId);
            ViewState["EnabledEnginesNames"] = enabledEngineNames;

            //console allowed in applicationHost.config
            ViewState["IsZooWebConsoleEnabled"] = enabledEngineNames.Contains("console", StringComparer.OrdinalIgnoreCase);


            List<ShortHeliconZooEngine> allowedEngines = new List<ShortHeliconZooEngine>(allowedEngineArray);

            // fix engine name and check is web console enabled
            foreach (ShortHeliconZooEngine engine in allowedEngines)
            {
                engine.Name = engine.Name.Replace("HeliconZoo.", "");
                //engine.Enabled = enabledEngineNames.Contains(engine.Name, StringComparer.OrdinalIgnoreCase);

                if (engine.Name == "console")
                {
                    //console allowed in hosting plan
                    ViewState["IsZooWebConsoleEnabled"] = engine.Enabled;
                }
            }

            ViewState["AllowedEngines"] = allowedEngines;

        }

        private void BindInstalledApplications()
        {
            ViewState["IsZooEnabled"] = false;
            var installedApplications = ES.Services.WebServers.GetZooApplications(PanelRequest.ItemID);
            ViewState["IsZooEnabled"] = true;

            if ((bool) ViewState["IsZooWebConsoleEnabled"])
            {
                gvInstalledApplications.DataSource = installedApplications;
                gvInstalledApplications.DataBind();

                
            }
            else
            {
                HideInstalledApplications();
            }
        }

        private void HideInstalledApplications()
        {
            gvInstalledApplications.Visible = false;
            lblConsole.Visible = false;
        }

        private void BindApplications()
        {


            WebAppGalleryHelpers helper = new WebAppGalleryHelpers();

            GalleryApplicationsResult result = helper.GetGalleryApplications("ZooTemplate", PanelSecurity.PackageId);

            List<GalleryApplication> applications = result.Value as List<GalleryApplication>;
            List<GalleryApplication> filteredApplications = new List<GalleryApplication>();

            List<ShortHeliconZooEngine> allowedEngines = (List<ShortHeliconZooEngine>)ViewState["AllowedEngines"];
            if (null != allowedEngines)
            {
                foreach (GalleryApplication application in applications)
                {
                    

                    foreach (string keyword in application.Keywords)
                    {
                        bool appAlreadyAdded = false;
                        if (keyword.StartsWith("ZooEngine", StringComparison.OrdinalIgnoreCase))
                        {
                            string appEngine = keyword.Substring("ZooEngine".Length);
                            
                            foreach (ShortHeliconZooEngine engine in allowedEngines)
                            {
                                if (!engine.Enabled)
                                {
                                    continue; //skip
                                }

                                if (
                                    string.Equals(appEngine, engine.KeywordedName, StringComparison.OrdinalIgnoreCase)
                                    ||
                                    engine.Name == "*"
                                    )
                                {
                                    
                                    filteredApplications.Add(application);
                                    appAlreadyAdded = true;
                                    break;
                                }
                            }
                            if (appAlreadyAdded)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                filteredApplications.AddRange(applications);
            }


            gvApplications.DataSource = filteredApplications;
            gvApplications.DataBind();
        }

        public void SaveWebItem(WebSite site)
        {
            UpdatedAllowedEngines();
        }

        protected void gvApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Install")
            {
                UpdatedAllowedEngines();
                Response.Redirect(GetWebAppInstallUrl(e.CommandArgument.ToString()));
            }
        }

        private void UpdatedAllowedEngines()
        {
            if (!(bool) ViewState["IsZooEnabled"])
            {
                return; // exit;
            }

            List<ShortHeliconZooEngine> allowedEngines = (List<ShortHeliconZooEngine>)ViewState["AllowedEngines"];
            string[] enabledEngineNames = (string[])ViewState["EnabledEnginesNames"];

            // check that all allowed engines are enabled
            bool allAllowedAreEnabled = true;

            if (allowedEngines.Count != enabledEngineNames.Length)
            {
                allAllowedAreEnabled = false;
            }
            else
            {
                foreach (ShortHeliconZooEngine allowedEngine in allowedEngines)
                {
                    if (!enabledEngineNames.Contains(allowedEngine.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        allAllowedAreEnabled = false;
                    }
                }
            }

            if (!allAllowedAreEnabled)
            {
                List<string> updateEnabledEngineNames = new List<string>();

                // by default allow for site all engines allowed by hosting plan
                foreach (ShortHeliconZooEngine heliconZooEngine in allowedEngines)
                {
                    updateEnabledEngineNames.Add(heliconZooEngine.Name);
                }

                string siteId = ViewState["WebSiteId"] as string;
                int packageId = (int) ViewState["WebSitePackageId"];

                ES.Services.HeliconZoo.SetEnabledEnginesForSite(siteId, packageId, updateEnabledEngineNames.ToArray());
            }

        }

        protected void gvApplications_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvApplications.PageIndex = e.NewPageIndex;
            // categorized app list
            BindApplications();
        }

        protected string GetIconUrlOrDefault(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "/App_Themes/Default/icons/sphere_128.png";
            }

            return "~/DesktopModules/SolidCP/ResizeImage.ashx?width=120&height=120&url=" + Server.UrlEncode(url);
        }

        protected string GetWebAppInstallUrl(string appId)
        {
            //http://localhost:9001/Default.aspx?pid=SpaceWebApplicationsGallery&mid=122&ctl=edit&ApplicationID=DotNetNuke&SpaceID=7

            var mid = GetWebAppGaleryModuleId();

            List<string> url = new List<string>();
            url.Add("pid=SpaceWebApplicationsGallery");
            url.Add(string.Format("{0}={1}", DefaultPage.MODULE_ID_PARAM, mid));
            url.Add("ctl=edit");
            url.Add("SpaceID="+PanelSecurity.PackageId.ToString(CultureInfo.InvariantCulture));
            url.Add("ApplicationID=" + appId);
            string siteId = ViewState["WebSiteId"] as string;
            if (!string.IsNullOrEmpty(siteId))
            {
                url.Add("SiteId="+siteId);
            }
            url.Add("ReturnUrl=" + Server.UrlEncode(Request.RawUrl));

            return "~/Default.aspx?" + String.Join("&", url.ToArray());
        }

        private static int GetWebAppGaleryModuleId()
        {
            // default value, valid in 2.1.0.166
            int mid = 124;

            foreach (KeyValuePair<int, PageModule> pair in PortalConfiguration.Site.Modules)
            {
                if (string.Equals(pair.Value.ModuleDefinitionID, "webapplicationsgallery", StringComparison.OrdinalIgnoreCase))
                {
                    mid = pair.Value.ModuleId;
                    break;
                }
            }
            return mid;
        }

        protected void gvInstalledApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EnableConsole")
            {
                UpdatedAllowedEngines();

                string appName = e.CommandArgument.ToString();

                ES.Services.WebServers.SetZooConsoleEnabled(PanelRequest.ItemID, appName);

                BindInstalledApplications();
            }

            if (e.CommandName == "DisableConsole")
            {
                UpdatedAllowedEngines();

                string appName = e.CommandArgument.ToString();

                ES.Services.WebServers.SetZooConsoleDisabled(PanelRequest.ItemID, appName);

                BindInstalledApplications();
            }

          
        }


        protected bool IsNullOrEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        protected string GetConsoleFullUrl(string consoleUrl)
        {
            WebSite site = ES.Services.WebServers.GetWebSite(PanelRequest.ItemID);
            return "http://" + site.Name + consoleUrl;
        }
    }
}
