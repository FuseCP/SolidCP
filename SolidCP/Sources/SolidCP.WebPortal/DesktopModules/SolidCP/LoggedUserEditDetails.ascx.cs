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
using SolidCP.EnterpriseServer;
using System.Web.Security;
using System.Data;
using System.Web;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;
using SolidCP.WebPortal;

namespace SolidCP.Portal
{
    public partial class LoggedUserEditDetails : SolidCPModuleBase
    {
        const int redirectTimeout = Utils.CHANGE_PASSWORD_REDIRECT_TIMEOUT;
        const string changePasswordWarningKey = "LoggedUserEditDetails.ChangePasswordWarning";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // bind form
                BindLanguages();
                BindThemes();
                BindUser();
                string changePasswordWarningText = GetSharedLocalizedString(changePasswordWarningKey);
                if (!String.IsNullOrEmpty(changePasswordWarningText)) 
                    lblChangePasswordWarning.Text = changePasswordWarningText;
            }
        }

        private void BindLanguages()
        {
			PortalUtils.LoadCultureDropDownList(ddlLanguage);
        }

        private void BindThemes()
        {
            //PortalUtils.LoadThemesDropDownList(ddlTheme);
            DataSet ThemeData = ES.Services.System.GetThemes();
            ddlTheme.DataSource = ThemeData;
            ddlTheme.DataBind();
            Utils.SelectListItem(ddlTheme, PortalUtils.CurrentTheme);

            BindThemeSettings(ThemeData);
        }

        private void BindThemeSettings(DataSet ThemeData)
        {
            DataSet ThemeStyleData = ES.Services.System.GetThemeSetting(int.Parse(ThemeData.Tables[0].Rows[ddlTheme.SelectedIndex]["ThemeID"].ToString()), "Style");
            if (ThemeStyleData != null)
            {
                ddlThemeStyle.DataSource = ThemeStyleData;
                ddlThemeStyle.DataBind();
            }

            DataSet ThemecolorHeaderData = ES.Services.System.GetThemeSetting(int.Parse(ThemeData.Tables[0].Rows[ddlTheme.SelectedIndex]["ThemeID"].ToString()), "color-Header");
            if (ThemecolorHeaderData != null)
            {
                //ddlThemecolorHeader.DataSource = ThemecolorHeaderData;
                //ddlThemecolorHeader.DataBind();
                ThemecolorHeaderRepeater1.DataSource = ThemecolorHeaderData;
                ThemecolorHeaderRepeater1.DataBind();
            }

            DataSet ThemecolorSidebarData = ES.Services.System.GetThemeSetting(int.Parse(ThemeData.Tables[0].Rows[ddlTheme.SelectedIndex]["ThemeID"].ToString()), "color-Sidebar");
            if (ThemecolorSidebarData != null)
            {
                //ddlThemecolorSidebar.DataSource = ThemecolorSidebarData;
                //ddlThemecolorSidebar.DataBind();
                ThemecolorSidebarRepeater1.DataSource = ThemecolorSidebarData;
                ThemecolorSidebarRepeater1.DataBind();
            }
        }

        private void BindUser()
        {
            UserInfo user = ES.Services.Users.GetUserById(PanelSecurity.LoggedUserId);

            if (user != null)
            {
                userPassword.SetUserPolicy(user.UserId, UserSettings.SolidCP_POLICY, "PasswordPolicy");

                // account info
                txtFirstName.Text = PortalAntiXSS.DecodeOld(user.FirstName);
                txtLastName.Text = PortalAntiXSS.DecodeOld(user.LastName);
                txtEmail.Text = user.Email;
                txtSecondaryEmail.Text = user.SecondaryEmail;
                lblUsername.Text = user.Username;
                ddlMailFormat.SelectedIndex = user.HtmlMail ? 1 : 0;
                cbxMfaEnabled.Checked = user.MfaMode > 0 ? true : false;
                btnGetQRCodeData.Visible = cbxMfaEnabled.Checked;

                // contact info
                contact.CompanyName = user.CompanyName;
                contact.Address = user.Address;
                contact.City = user.City;
                contact.Country = user.Country;
                contact.State = user.State;
                contact.Zip = user.Zip;
                contact.PrimaryPhone = user.PrimaryPhone;
                contact.SecondaryPhone = user.SecondaryPhone;
                contact.Fax = user.Fax;
                contact.MessengerId = user.InstantMessenger;

                // bind language
                /*DotNetNuke.Entities.Users.UserInfo dnnUser =
                    DnnUsers.GetUserByName(PortalSettings.PortalId, user.Username, false);

                if (dnnUser != null)
                    Utils.SelectListItem(ddlLanguage, dnnUser.Profile.PreferredLocale);*/

                // bind items per page
                
                txtItemsPerPage.Text = UsersHelper.GetDisplayItemsPerPage().ToString();

                string UserThemeStyle = "";
                DataSet UserThemeSettingsData = ES.Services.Users.GetUserThemeSettings(PanelSecurity.LoggedUserId);
                if (UserThemeSettingsData.Tables.Count > 0)
                {
                    foreach (DataRow row in UserThemeSettingsData.Tables[0].Rows)
                    {
                        string RowPropertyName = row.Field<String>("PropertyName");
                        string RowPropertyValue = row.Field<String>("PropertyValue");

                        if (RowPropertyName == "Style")
                        {
                            Utils.SelectListItem(ddlThemeStyle, RowPropertyValue);
                            UserThemeStyle = RowPropertyValue;

                        }

                        if (RowPropertyName == "color-Header")
                        {
                            //Utils.SelectListItem(ddlThemecolorHeader, RowPropertyValue);
                        }

                        if (RowPropertyName == "color-Sidebar")
                        {
                            //Utils.SelectListItem(ddlThemecolorSidebar, RowPropertyValue);
                        }
                    }
                }

                //TODO: Dynamically load the Theme Settings

            }
        }

        private void SaveUser(bool switchUser)
        {
            // get owner
            UserInfo user = ES.Services.Users.GetUserById(PanelSecurity.LoggedUserId);

            if (Page.IsValid)
            {
                // gather data from form
                // account info
                user.FirstName = txtFirstName.Text;
                user.LastName = txtLastName.Text;
                user.Email = txtEmail.Text;
                user.SecondaryEmail = txtSecondaryEmail.Text;
                user.HtmlMail = ddlMailFormat.SelectedIndex == 1;

                // contact info
				user.CompanyName = contact.CompanyName;
                user.Address = contact.Address;
                user.City = contact.City;
                user.Country = contact.Country;
                user.State = contact.State;
                user.Zip = contact.Zip;
                user.PrimaryPhone = contact.PrimaryPhone;
                user.SecondaryPhone = contact.SecondaryPhone;
                user.Fax = contact.Fax;
                user.InstantMessenger = contact.MessengerId;

                // update existing user
                try
                {
                    //int result = UsersHelper.UpdateUser(PortalId, user);
					int result = PortalUtils.UpdateUserAccount(user);

                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }

					// set language
					PortalUtils.SetCurrentLanguage(ddlLanguage.SelectedValue);

                    // set items per page
                    UsersHelper.SetDisplayItemsPerPage(Utils.ParseInt(txtItemsPerPage.Text.Trim(), 10));

                    if (ddlLanguage.SelectedValue != PortalUtils.CurrentUICulture.ToString())
                    {
                        SetCurrentLanguage();
                    }

                    if (ddlTheme.SelectedValue != PortalUtils.CurrentTheme)
                    {
                        SetCurrentTheme();
                    }

                    if (!string.IsNullOrEmpty(ddlThemeStyle.SelectedValue))
                    {
                        if (ddlThemeStyle.SelectedValue != PortalUtils.CurrentThemeStyle)
                        {
                            RemoveThemeOptions();
                        }
                        
                        HttpCookie UserThemeStyleCrum = new HttpCookie("UserThemeStyle", ddlThemeStyle.SelectedValue);
                        UserThemeStyleCrum.Expires = DateTime.Now.AddMonths(2);
                        HttpContext.Current.Response.Cookies.Add(UserThemeStyleCrum);

                        ES.Services.Users.UpdateUserThemeSetting(PanelSecurity.LoggedUserId, "Style", ddlThemeStyle.SelectedValue);

                    }

                    //if (!string.IsNullOrEmpty(ddlThemecolorHeader.SelectedValue))
                    //{
                    //    HttpCookie UserThemecolorHeaderCrum = new HttpCookie("UserThemecolorHeader", ddlThemecolorHeader.SelectedValue);
                    //    UserThemecolorHeaderCrum.Expires = DateTime.Now.AddMonths(2);
                    //    HttpContext.Current.Response.Cookies.Add(UserThemecolorHeaderCrum);

                    //    ES.Services.Users.UpdateUserThemeSetting(PanelSecurity.LoggedUserId, "color-Header", ddlThemecolorHeader.SelectedValue);
                    //}

                    //if (!string.IsNullOrEmpty(ddlThemecolorSidebar.SelectedValue))
                    //{
                    //    HttpCookie UserThemecolorSidebarCrum = new HttpCookie("UserThemecolorSidebar", ddlThemecolorSidebar.SelectedValue);
                    //    UserThemecolorSidebarCrum.Expires = DateTime.Now.AddMonths(2);
                    //    HttpContext.Current.Response.Cookies.Add(UserThemecolorSidebarCrum);

                    //    ES.Services.Users.UpdateUserThemeSetting(PanelSecurity.LoggedUserId, "color-Sidebar", ddlThemecolorSidebar.SelectedValue);
                    //}

                }
                catch (Exception ex)
                {
                    ShowErrorMessage("USER_UPDATE_USER", ex);
                    return;
                }

                // show message
                ShowSuccessMessage("USER_UPDATE_USER");
            }
        }

        private void ChangeUserPassword()
        {
            if (!Page.IsValid)
                return;

            try
            {
                //int result = UsersHelper.ChangeUserPassword(PortalId, PanelRequest.UserID, userPassword.Password);
                int result = PortalUtils.ChangeUserPassword(PanelSecurity.LoggedUserId, userPassword.Password);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
                pnlEdit.Visible = false;
                string loginClientUrl = Page.ResolveClientUrl(PortalUtils.LoginRedirectUrl);
                ShowSuccessMessage(Utils.ModuleName, "LOGGED_USER_CHANGE_PASSWORD", loginClientUrl, (redirectTimeout/1000).ToString());
                FormsAuthentication.SignOut();
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RedirectToLogin", String.Format("setTimeout(\"window.location='{0}'\",{1});", loginClientUrl, redirectTimeout), true); 
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USER_CHANGE_PASSWORD", ex);
                return;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveUser(false);
        }

        protected void cmdChangePassword_Click(object sender, EventArgs e)
        {
            // change password
            ChangeUserPassword();
        }

        protected void cbxMfaEnabled_CheckedChanged(object sender, EventArgs e)
        {
            UserInfo user = ES.Services.Users.GetUserById(PanelSecurity.LoggedUserId);
            PortalUtils.UpdateUserMfa(user.Username, cbxMfaEnabled.Checked);
            qrData.Visible = false;
            btnGetQRCodeData.Visible = cbxMfaEnabled.Checked;
        }

        private void SetCurrentLanguage()
        {
            PortalUtils.SetCurrentLanguage(ddlLanguage.SelectedValue);
        }

        private void SetCurrentTheme()
        {
            string selectedTheme = ddlTheme.SelectedValue;

            if (HttpContext.Current.Response.Cookies["UserRTL"].Value == "1")
            {
                DataSet themeData = ES.Services.Authentication.GetLoginThemes();
                selectedTheme = themeData.Tables[0].Rows[ddlTheme.SelectedIndex]["RTLName"].ToString();
            }

            RemoveThemeOptions();

            PortalUtils.SetCurrentTheme(selectedTheme);

        }

        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCurrentLanguage();

            if (!string.IsNullOrEmpty(HttpContext.Current.Response.Cookies["UserTheme"].Value))
            {
                SetCurrentTheme();
            }

            Response.Redirect(Request.Url.ToString());
        }

        protected void ddlTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCurrentTheme();
            Response.Redirect(Request.Url.ToString());

        }

        public Color ConvertFromHexToColor(string hex)
        {
            string colorcode = hex;
            int argb = Int32.Parse(colorcode.Replace("#", ""), NumberStyles.HexNumber);
            Color clr = Color.FromArgb(argb);
            return clr;
        }

        protected void ThemecolorHeader_Click(object sender, CommandEventArgs e)
        {
            ES.Services.Users.UpdateUserThemeSetting(PanelSecurity.LoggedUserId, "color-Header", e.CommandArgument.ToString());

            HttpCookie UserThemecolorCrum = new HttpCookie("UserThemecolorHeader", e.CommandArgument.ToString());
            UserThemecolorCrum.Expires = DateTime.Now.AddMonths(2);
            HttpContext.Current.Response.Cookies.Add(UserThemecolorCrum);

            Response.Redirect(Request.Url.ToString());
        }

        protected void ThemecolorSidebar_Click(object sender, CommandEventArgs e)
        {
            ES.Services.Users.UpdateUserThemeSetting(PanelSecurity.LoggedUserId, "color-Sidebar", e.CommandArgument.ToString());

            HttpCookie UserThemecolorCrum = new HttpCookie("UserThemecolorSidebar", e.CommandArgument.ToString());
            UserThemecolorCrum.Expires = DateTime.Now.AddMonths(2);
            HttpContext.Current.Response.Cookies.Add(UserThemecolorCrum);

            Response.Redirect(Request.Url.ToString());

        }

        protected void cmdResetDisplay_Click(object sender, EventArgs e)
        {
            RemoveThemeOptions();
            Response.Redirect(Request.Url.ToString());
        }

        protected void RemoveThemeOptions()
        {
            ES.Services.Users.DeleteUserThemeSetting(PanelSecurity.LoggedUserId, "Style"); 
            ES.Services.Users.DeleteUserThemeSetting(PanelSecurity.LoggedUserId, "color-Header");
            ES.Services.Users.DeleteUserThemeSetting(PanelSecurity.LoggedUserId, "color-Sidebar");

            HttpCookie UserThemeStyleCrum = new HttpCookie("UserThemeStyle", "");
            UserThemeStyleCrum.Expires = DateTime.Now.AddMonths(-1);
            HttpContext.Current.Response.Cookies.Add(UserThemeStyleCrum);

            HttpCookie UserThemecolorHeaderCrum = new HttpCookie("UserThemecolorHeader", "");
            UserThemecolorHeaderCrum.Expires = DateTime.Now.AddMonths(-1);
            HttpContext.Current.Response.Cookies.Add(UserThemecolorHeaderCrum);

            HttpCookie UserThemeSidebarCrum = new HttpCookie("UserThemecolorSidebar", "");
            UserThemeSidebarCrum.Expires = DateTime.Now.AddMonths(-1);
            HttpContext.Current.Response.Cookies.Add(UserThemeSidebarCrum);
        }

        protected void btnGetQRCodeData_Click(object sender, EventArgs e)
        {
            UserInfo user = ES.Services.Users.GetUserById(PanelSecurity.LoggedUserId);
            var qrCodeData = PortalUtils.GetQrCodeData(user.Username);
            if (cbxMfaEnabled.Checked && qrCodeData.Length == 2)
            {
                qrData.Visible = true;
                lblManualAuth.Text = qrCodeData[0];
                imgQrCode.ImageUrl = qrCodeData[1];
            }
        }

        protected void btnActivateQRCode_Click(object sender, EventArgs e)
        {
            UserInfo user = ES.Services.Users.GetUserById(PanelSecurity.LoggedUserId);
            var success = PortalUtils.ActivateQrCode(user.Username, txtQrCodeActivationPin.Text.Trim());
            if (!success)
            {
                ShowErrorMessage("QRCodeActivation");
                return;
            }

            qrData.Visible = false;
            ShowSuccessMessage("QRCodeActivation");
        }
    }
}
