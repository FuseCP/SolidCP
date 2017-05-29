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
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Portal
{
    public partial class WebApplicationGallery : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));

			// Maintains appearance settings corresponding to user's display preferences
			gvApplications.PageSize = UsersHelper.GetDisplayItemsPerPage();

            try
            {
				GalleryCategoriesResult result = ES.Services.WebApplicationGallery.GetGalleryCategories(PanelSecurity.PackageId);
				//
				if (!result.IsSuccess)
				{
					rbsCategory.Visible = false;
					messageBox.ShowMessage(result, "WAG_NOT_AVAILABLE", "ModuleWAG");
					return;
				}

                if (!IsPostBack)
                {
                    //
                    SetLanguage();
                    BindLanguages();
                    BindCategories();
					BindApplications();
                    ViewState["IsSearchResults"] = false;
                }
            }
            catch(Exception ex)
            {
                ShowErrorMessage("GET_WEB_GALLERY_CATEGORIES", ex);             
            }
        }

		protected void gvApplications_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvApplications.PageIndex = e.NewPageIndex;
            if ((bool)ViewState["IsSearchResults"] == false)
            {
                // categorized app list
                BindApplications();
                if (null != rbsCategory.SelectedItem)
                {
                    rbsCategory.SelectedItem.Attributes["class"] = "selected";
                }
            }
            else
            {
                // search result app list
                SearchButton_Click(sender, null);
            }
		}

        private void BindLanguages()
        {
            GalleryLanguagesResult result = ES.Services.WebApplicationGallery.GetGalleryLanguages(PanelSecurity.PackageId);
            dropDownLanguages.DataSource = result.Value;
            //dropDownLanguages.SelectedIndex = 0;
            dropDownLanguages.SelectedValue = (string)Session["WebApplicationGaleryLanguage"];
            dropDownLanguages.DataTextField = "Value";
            dropDownLanguages.DataValueField = "Name";
            dropDownLanguages.DataBind();

        }

        private void BindCategories()
        {
			GalleryCategoriesResult result = ES.Services.WebApplicationGallery.GetGalleryCategories(PanelSecurity.PackageId);
			//
			rbsCategory.DataSource = result.Value;
            rbsCategory.DataTextField = "Name";
            rbsCategory.DataValueField = "Id";
            rbsCategory.DataBind();

            // add empty
            ListItem listItem = new ListItem("All", "");
            listItem.Attributes["class"] = "selected";
            rbsCategory.Items.Insert(0, listItem);
        }

		private void BindApplications()
		{
            ViewState["IsSearchResults"] = false;
            WebAppGalleryHelpers helper = new WebAppGalleryHelpers();
			//
			GalleryApplicationsResult result = helper.GetGalleryApplications(rbsCategory.SelectedValue, PanelSecurity.PackageId);
			//
			gvApplications.DataSource = result.Value;
			gvApplications.DataBind();
		}

		protected void CategorySelectedIndexChanged(object sender, EventArgs e)
		{
		    ViewState["IsSearchResults"] = false;
		    searchBox.Text = "";
            gvApplications.PageIndex = 0;
            rbsCategory.SelectedItem.Attributes["class"] = "selected";

            BindApplications();
		}

        protected void gvApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Install")
                Response.Redirect(EditUrl("ApplicationID", e.CommandArgument.ToString(), "edit",
                    "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if ((bool)ViewState["IsSearchResults"] == false)
            {
                gvApplications.PageIndex = 0;
            }
            ViewState["IsSearchResults"] = true;

            WebAppGalleryHelpers helper = new WebAppGalleryHelpers();
            GalleryApplicationsResult result = helper.GetGalleryApplicationsFiltered(searchBox.Text, PanelSecurity.PackageId);

            gvApplications.DataSource = result.Value;
            gvApplications.DataBind();
        }

        protected void dropDownLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["WebApplicationGaleryLanguage"] = dropDownLanguages.SelectedValue;

            SetLanguage();

            BindLanguages();
            BindCategories();
            BindApplications();

        }

        private void SetLanguage()
        {
            string lang = (string)Session["WebApplicationGaleryLanguage"];
            if (string.IsNullOrEmpty(lang))
            {
                lang = "en";
            }
            ES.Services.WebApplicationGallery.SetResourceLanguage(PanelSecurity.PackageId, lang);
        }

        protected string GetIconUrlOrDefault(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "/App_Themes/Default/icons/sphere_128.png";
            }
            
            return "~/DesktopModules/SolidCP/ResizeImage.ashx?width=120&height=120&url=" + Server.UrlEncode(url);
        }
    }
}
