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
using System.Resources;
using System.IO;
using System.Drawing;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;
using SolidCP.Portal;

namespace SolidCP.WebPortal
{
    public partial class DefaultPage : System.Web.UI.Page
    {
        public const string DEFAULT_PAGE = "~/Default.aspx";
        public const string PAGE_ID_PARAM = "pid";
        public const string CONTROL_ID_PARAM = "ctl";
        public const string MODULE_ID_PARAM = "mid";
        public const string THEMES_FOLDER = "App_Themes";
        public const string IMAGES_FOLDER = "Images";
		public const string ICONS_FOLDER = "Icons";
        public const string SKINS_FOLDER = "App_Skins";
        public const string CONTAINERS_FOLDER = "App_Containers";
        public const string CONTENT_PANE_NAME = "ContentPane";
        public const string LEFT_PANE_NAME = "LeftPane";
        public const string MODULE_TITLE_CONTROL_ID = "lblModuleTitle";
        public const string MODULE_ICON_CONTROL_ID = "imgModuleIcon";
        public const string DESKTOP_MODULES_FOLDER = "DesktopModules";

		protected string CultureCookieName
		{
			get { return PortalConfiguration.SiteSettings["CultureCookieName"]; }
		}

        private string CurrentPageID
        {
            get
            {
                string pid = Request[PAGE_ID_PARAM];
                if (pid == null)
                {
                    // get default page
                    pid = PortalConfiguration.SiteSettings["DefaultPage"];
                }
				return pid.ToLower(CultureInfo.InvariantCulture);
            }
        }

        private string ModuleControlID
        {
            get
            {
                string ctl = Request[CONTROL_ID_PARAM];
                if (ctl == null)
                {
                    ctl = "";
                }
                return ctl.ToLower(CultureInfo.InvariantCulture);
            }
        }

        private int ModuleID
        {
            get
            {
                string smid = Request[MODULE_ID_PARAM];
                if (smid == null)
                {
                    smid = "0";
                }
                return Int32.Parse(smid);
            }
        }

        public static string GetPageUrl(string pid)
        {
            return DefaultPage.DEFAULT_PAGE + "?" + DefaultPage.PAGE_ID_PARAM + "=" + pid;
        }

        public static bool IsAccessibleToUser(HttpContext context, IList roles)
        {
            foreach (string role in roles)
            {
                if (role == "?")
                    return true;

                if ((role == "*") || ((context.User != null) && context.User.IsInRole(role)))
                {
                    return true;
                }
            }
            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		protected void Page_PreInit(object sender, EventArgs e)
		{
			Theme = PortalThemeProvider.Instance.GetTheme();
		}

        protected void Page_Init(object sender, EventArgs e)
        {
            // get page info
            string pid = CurrentPageID;
            if(PortalConfiguration.Site.Pages[pid] == null)
            {
                ShowError(skinPlaceHolder, String.Format("Page with ID '{0}' is not found", HttpUtility.HtmlEncode(pid)));
                return;
            }

            PortalPage page = PortalConfiguration.Site.Pages[pid];

            // check if page is accessible to user
            if (!IsAccessibleToUser(Context, page.Roles))
            {
                // redirect to login page
                string returnUrl = Request.RawUrl;
                Response.Redirect(DEFAULT_PAGE + "?" + PAGE_ID_PARAM + "=" +
                    PortalConfiguration.SiteSettings["LoginPage"] + "&ReturnUrl=" + Server.UrlEncode(returnUrl));
            }

			Title = String.Format("{0} - {1}",
				PortalConfiguration.SiteSettings["PortalName"],
				PageTitleProvider.Instance.ProcessPageTitle(GetLocalizedPageTitle(page.Name)));

            // load skin
            bool editMode = (ModuleControlID != "" && ModuleID > 0);

            string skinName = page.SkinSrc;
            if(!editMode)
            {
                // browse skin
                if (String.IsNullOrEmpty(skinName))
                {
                    // load portal skin
                    skinName = PortalConfiguration.SiteSettings["PortalSkin"];
                }
            }
            else
            {
                // edit skin
				if (!String.IsNullOrEmpty(page.AdminSkinSrc))
					skinName = page.AdminSkinSrc;
				else
					skinName =  PortalConfiguration.SiteSettings["AdminSkin"];
            }

            // load skin control
            string skinPath = "~/" + SKINS_FOLDER + "/" + this.Theme + "/" + skinName;
            Control ctrlSkin = null;
            try
            {
                ctrlSkin = LoadControl(skinPath);
                skinPlaceHolder.Controls.Add(ctrlSkin);
            }
            catch (Exception ex)
            {
                ShowError(skinPlaceHolder, String.Format("Can't load {0} skin: {1}", skinPath, ex.ToString()));
                return;
            }

            // load page modules
            if (!editMode)
            {
                // browse mode
                foreach (string paneId in page.ContentPanes.Keys)
                {
                    // try to find content pane
                    Control ctrlPane = ctrlSkin.FindControl(paneId);
                    if (ctrlPane != null)
                    {
                        // insert modules
                        ContentPane pane = page.ContentPanes[paneId];
                        foreach (PageModule module in pane.Modules)
                        {
                            if (IsAccessibleToUser(Context, module.ViewRoles))
                            {
                                // add module
                                if (module.Settings.Contains("UseDefault"))
                                {
                                    string useDefault = Convert.ToString(module.Settings["UseDefault"]).ToLower(CultureInfo.InvariantCulture);
                                    AddModuleToContentPane(ctrlPane, module, useDefault, editMode);
                                }
                                else
                                    AddModuleToContentPane(ctrlPane, module, "", editMode);
                            }
                        }
                    }
                }
            }
            else
            {
                // edit mode
                // find ContentPane
                Control ctrlPane = ctrlSkin.FindControl(CONTENT_PANE_NAME);
                if (ctrlPane != null)
                {
                    // add "edit" module
                    if (PortalConfiguration.Site.Modules.ContainsKey(ModuleID))
                        AddModuleToContentPane(ctrlPane, PortalConfiguration.Site.Modules[ModuleID],
                            ModuleControlID, editMode);
                }
                // find LeftPane
                ctrlPane = ctrlSkin.FindControl(LEFT_PANE_NAME);
                if (ctrlPane != null && page.ContentPanes.ContainsKey(LEFT_PANE_NAME))
                {
                    ContentPane pane = page.ContentPanes[LEFT_PANE_NAME];
                     foreach (PageModule module in pane.Modules)
                        {
                            if (IsAccessibleToUser(Context, module.ViewRoles))
                            {
                                // add module
                                AddModuleToContentPane(ctrlPane, module, "", false);
                            }
                        }
                }
            }
        }

		protected void Page_PreRender(object sender, EventArgs e)
		{
			// Ensure the page's form action attribute is not empty
			if (Request.RawUrl.Equals("/") == false)
				return;
			// Assign default page to avoid ASP.NET 4 issue w/ Extensionless URL Module & Custom HTTP Modules
			Form.Action = Form.ResolveUrl(DEFAULT_PAGE);
		}

        private void AddModuleToContentPane(Control pane, PageModule module, string ctrlKey, bool editMode)
        {
            string defId = module.ModuleDefinitionID;
            if(!PortalConfiguration.ModuleDefinitions.ContainsKey(defId))
            {
                ShowError(pane, String.Format("Module definition '{0}' could not be found", defId));
                return;
            }

            ModuleDefinition definition = PortalConfiguration.ModuleDefinitions[defId];
            ModuleControl control = null;
            if(String.IsNullOrEmpty(ctrlKey))
                control = definition.DefaultControl;
            else
            {
                if(definition.Controls.ContainsKey(ctrlKey))
                    control = definition.Controls[ctrlKey];
            }

            if(control == null)
                return;

            // container
            string containerName = editMode ?
                PortalConfiguration.SiteSettings["AdminContainer"] : PortalConfiguration.SiteSettings["PortalContainer"];

            if (!editMode && !String.IsNullOrEmpty(module.ContainerSrc))
                containerName = module.ContainerSrc;

			if (editMode && !String.IsNullOrEmpty(module.AdminContainerSrc))
				containerName = module.AdminContainerSrc;

            // load container
            string containerPath = "~/" + CONTAINERS_FOLDER + "/" + this.Theme + "/" + containerName;
            Control ctrlContainer = null;
            try
            {
                ctrlContainer = LoadControl(containerPath);
            }
            catch (Exception ex)
            {
                ShowError(pane, String.Format("Container '{0}' could not be loaded: {1}", containerPath, ex.ToString()));
                return;
            }

            string title = module.Title;
            if (editMode || String.IsNullOrEmpty(title))
            {
                // get control title
                title = control.Title;
            }

            string iconFile = module.IconFile;
            if (editMode || String.IsNullOrEmpty(iconFile))
            {
                // get control icon
                iconFile = control.IconFile;
            }

            // set title
            Label lblModuleTitle = (Label)ctrlContainer.FindControl(MODULE_TITLE_CONTROL_ID);
			if (lblModuleTitle != null)
			{
				lblModuleTitle.Text = GetLocalizedModuleTitle(title);
			}

            // set icon
            System.Web.UI.WebControls.Image imgModuleIcon = (System.Web.UI.WebControls.Image)ctrlContainer.FindControl(MODULE_ICON_CONTROL_ID);
            if (imgModuleIcon != null)
            {
                if (String.IsNullOrEmpty(iconFile))
                {
                    imgModuleIcon.Visible = false;
                }
                else
                {
                    string iconPath = "~/" + THEMES_FOLDER + "/" + this.Theme + "/" + ICONS_FOLDER + "/" + iconFile;
                    imgModuleIcon.ImageUrl = iconPath;
                }
            }

            Control contentPane = ctrlContainer.FindControl(CONTENT_PANE_NAME);
            if (contentPane != null)
            {
                string controlName = control.Src;
                string controlPath = "~/" + DESKTOP_MODULES_FOLDER + "/" + controlName;
                if (!String.IsNullOrEmpty(controlName))
                {
                    PortalControlBase ctrlControl = null;
                    try
                    {
                        ctrlControl = (PortalControlBase)LoadControl(controlPath);
                        ctrlControl.Module = module;
                        ctrlControl.ContainerControl = ctrlContainer;
                        contentPane.Controls.Add(ctrlControl);
                    }
                    catch (Exception ex)
                    {
                        ShowError(contentPane, String.Format("Control '{0}' could not be loaded: {1}", controlPath, ex.ToString()));
                    }
                }
            }


            // add controls to the pane
            pane.Controls.Add(ctrlContainer);
        }

		protected override void InitializeCulture()
		{
			HttpCookie localeCrub = Request.Cookies[CultureCookieName];

			if (localeCrub != null)
			{
				string localeCode = localeCrub.Value;
				UICulture = localeCode;
				Culture = localeCode;

				System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture(localeCode);

				if (ci != null)
				{
					// Reset currency symbol to deal with the existing ISO currency symbol implementation
					ci.NumberFormat.CurrencySymbol = String.Empty;
					// Setting up culture
					System.Threading.Thread.CurrentThread.CurrentCulture = ci;
					System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
				}
			}

			base.InitializeCulture();
		}

        private void ShowError(Control placeholder, string message)
        {
            Label lbl = new Label();
            lbl.Text =
                PortalAntiXSS.Encode("<div style=\"height:300px;overflow:auto;\">" + message.Replace("\n", "<br>") +
                                   "</div>");
            lbl.ForeColor = Color.Red;
            lbl.Font.Bold = true;
            lbl.Font.Size = FontUnit.Point(8);
            placeholder.Controls.Add(lbl);
        }

		public static string GetLocalizedModuleTitle(string moduleName)
		{
            string localizedString = GetLocalizedResourceString("Modules", String.Concat("ModuleTitle.", moduleName));
            return localizedString != null ? localizedString : moduleName;
		}

		public static string GetLocalizedPageTitle(string tabName)
		{
            string localizedString = GetLocalizedResourceString("Pages", String.Concat("PageTitle.", tabName));
            return localizedString != null ? localizedString : tabName;
		}

		public static string GetLocalizedPageName(string tabName)
		{
			return GetLocalizedResourceString("Pages", String.Concat("PageName.", tabName));
		}

		private static string GetLocalizedResourceString(string suffix, string key)
		{
			List<string> list1 = null;
			if (suffix == "Pages")
			{
				list1 = GetResourceFiles("Pages", "SCPLocaleAdapterPages");
			}
			else
			{
				list1 = GetResourceFiles("Modules", "SCPLocaleAdapterModules");
			}

			string text1 = null;
			foreach (string text2 in list1)
			{
				text1 = GetGlobalLocalizedString(text2, key);

				if (!String.IsNullOrEmpty(text1))
				{
					return text1;
				}
			}
			return text1;
		}

		private static List<string> GetResourceFiles(string suffix, string cacheKey)
		{
			List<string> list1 = (List<string>)HttpContext.Current.Cache[cacheKey];
			if (list1 == null)
			{
				list1 = new List<string>();
				string text2 = HttpContext.Current.Server.MapPath("~/App_GlobalResources");

				FileInfo[] infoArray1 = new DirectoryInfo(text2).GetFiles("*_" + suffix + ".ascx.resx");

				foreach (FileInfo info1 in infoArray1)
				{
					list1.Add(info1.Name);
				}

				HttpContext.Current.Cache.Insert(cacheKey, list1, new CacheDependency(text2));
			}
			return list1;
		}

		public static string GetGlobalLocalizedString(string fileName, string resourceKey)
		{
			string className = fileName.Replace(".resx", "");
			return (string)HttpContext.GetGlobalResourceObject(className, resourceKey);
		}
    }
}
