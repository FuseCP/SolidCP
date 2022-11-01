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
using System.Data;
using System.Web;
using SolidCP.EnterpriseServer;
using SCP = SolidCP.EnterpriseServer;
 


namespace SolidCP.Portal
{
    public partial class Login : SolidCPModuleBase
    {
        string ipAddress;
        //private IMessageBoxControl messageBox;     --- compile warning - never used
         
        private bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            Uri absoluteUri;
            if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
            {
                return String.Equals(this.Request.Url.Host, absoluteUri.Host, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                bool isLocal = !url.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
                    && !url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
                    && Uri.IsWellFormedUriString(url, UriKind.Relative);
                return isLocal;
            }
        }

        private string RedirectUrl
        {
            get
            {
                string redirectUrl = "";
                if (Request["returnurl"] != null)
                {
                    // return to the url passed to signin
                    redirectUrl = HttpUtility.UrlDecode(Request["returnurl"]);
                    if (!IsLocalUrl(redirectUrl))
                    {
                        redirectUrl = PortalUtils.LoginRedirectUrl;
                    }
                }
                else
                {
                    redirectUrl = PortalUtils.LoginRedirectUrl;
                }
                return redirectUrl;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnsureSCPA();
                //
                BindControls();
            }

            // capture Enter key
            //DotNetNuke.UI.Utilities.ClientAPI.RegisterKeyCapture(this.Parent, btnLogin, 13);

            // get user IP
            if (Request.UserHostAddress != null)
                ipAddress = Request.UserHostAddress;

            // update password control
            txtPassword.Attributes["value"] = txtPassword.Text;

            // autologin
            string usr = Request["u"];
            if (String.IsNullOrEmpty(usr))
                usr = Request["user"];

            string psw = Request["p"];
            if (String.IsNullOrEmpty(psw))
                psw = Request["pwd"];
            if (String.IsNullOrEmpty(psw))
                psw = Request["password"];

            if (!String.IsNullOrEmpty(usr) && !String.IsNullOrEmpty(psw))
            {
                // perform login
                LoginUser(usr, psw, chkRemember.Checked, String.Empty, String.Empty);
            }
        }

        private void EnsureSCPA()
        {
            var enabledScpa = ES.Services.Authentication.GetSystemSetupMode();
            //
            if (enabledScpa == false)
            {
                return;
            }
            //
            Response.Redirect(EditUrl("scpa"), true);
        }

        private void BindControls()
        {
            // load languages
            PortalUtils.LoadCultureDropDownList(ddlLanguage);

            // load themes
            BindThemes();

            // try to get the last login name from cookie
            HttpCookie cookie = Request.Cookies["SolidCPLogin"];
            if (cookie != null)
            {
                txtUsername.Text = cookie.Value;
                txtPassword.Focus();
            }
            else
            {
                // set focus on username field
                txtUsername.Focus();
            }

            if (PortalUtils.GetHideThemeAndLocale())
            {
                ddlLanguage.Visible = false;
                lblLanguage.Visible = false;
                ddlTheme.Visible = false;
                lblTheme.Visible = false;
            }
        }

        private void BindThemes()
        {
            ddlTheme.DataSource = ES.Services.Authentication.GetLoginThemes();
            ddlTheme.DataBind();
            Utils.SelectListItem(ddlTheme, PortalUtils.CurrentTheme);
        }

        protected void cmdForgotPassword_Click(object sender, EventArgs e)
        {
           Response.Redirect(EditUrl("forgot_password"), true);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // validate input
            if (!Page.IsValid)
                return;

            // perform login
            LoginUser(txtUsername.Text.Trim(), txtPassword.Text, chkRemember.Checked,
                ddlLanguage.SelectedValue, ddlTheme.SelectedValue);
        }

        protected void btnVerifyPin_Click(object sender, EventArgs e)
        {
            // validate input
            if (!Page.IsValid)
                return;

            var isValid = PortalUtils.ValidatePin(txtUsername.Text, txtPin.Text);

            if (!isValid)
            {
                ShowErrorMessage("WrongPin");
                return;
            }

            var encryptedTicket = tokenDiv.Attributes["value"];
            PortalUtils.SetTicketAndCompleteLogin(encryptedTicket, txtUsername.Text.Trim(), chkRemember.Checked, ddlLanguage.SelectedValue, ddlTheme.SelectedValue);
            tokenDiv.Attributes["value"] = null;
            CompleteLogin(0);
        }


        private void LoginUser(string username, string password, bool rememberLogin,
            string preferredLocale, string theme)
        {
            // status
            int loginStatus = PortalUtils.AuthenticateUser(username, password, ipAddress);
            
            if (loginStatus < 0)
            {
                ShowWarningMessage("WrongLogin");
                return;
            }

            string encryptedTicket = PortalUtils.CreateEncyptedAuthenticationTicket(username, password, ipAddress, rememberLogin, preferredLocale, theme);

            if (loginStatus == BusinessSuccessCodes.SUCCESS_USER_MFA_ACTIVE)
            {
                userPwdDiv.Visible = false;
                tokenDiv.Visible = true;
                tokenDiv.Attributes["value"] = encryptedTicket;
                return;
            }

            PortalUtils.SetTicketAndCompleteLogin(encryptedTicket, txtUsername.Text.Trim(), chkRemember.Checked, ddlLanguage.SelectedValue, ddlTheme.SelectedValue);
            CompleteLogin(loginStatus);
        }

        private void CompleteLogin(int loginStatus)
        {
            // Access IP Settings
            SCP.SystemSettings settings = ES.Services.System.GetSystemSettings(SCP.SystemSettings.ACCESS_IP_SETTINGS);
            String AccessIps = String.Empty;
            String[] arAccessIps = null;
            if (settings != null)
            {
                AccessIps = settings.GetValueOrDefault(SCP.SystemSettings.ACCESS_IPs, string.Empty);
                arAccessIps = AccessIps.Split(',');
            }
           
            if (!String.IsNullOrEmpty(AccessIps))
            {
                String RequestIP = Request.ServerVariables["REMOTE_ADDR"];
               // String l_stSubnet = Knom.Helpers.Net.SubnetMask.ReturnSubnetmask(AccessIps);
                Boolean l_Mach = false;

                try {
                    foreach(String l_AccessIP in arAccessIps)
                    {
                        l_Mach = Knom.Helpers.Net.SubnetMask.IsInRange(RequestIP, l_AccessIP.Trim());
                        if (l_Mach)
                            break; // Once it passed then don't need to check for other access;
                    }
                } catch (Exception)
                { }
                if (!l_Mach)
                {
                    PortalUtils.UserSignOutOnly();
                    // messageBox.RenderMessage(MessageBoxType.Warning, "Unauthorized IP", "Unauthorized IP", null);
                    ShowWarningMessage("IPAccessProhibited");
                    return;
                }

            }

            if (loginStatus == BusinessSuccessCodes.SUCCESS_USER_ONETIMEPASSWORD)
            {
                // One time password should be changed after login
                Response.Redirect("Default.aspx?mid=1&ctl=change_onetimepassword&onetimepassword=true&UserID=" + PanelSecurity.LoggedUserId.ToString());
            }
            else
            {
                //Make Theme Cookies
                DataSet UserThemeSettingsData = ES.Services.Users.GetUserThemeSettings(PanelSecurity.LoggedUserId);
                if (UserThemeSettingsData.Tables.Count > 0)
                {
                    foreach (DataRow row in UserThemeSettingsData.Tables[0].Rows)
                    {
                        string RowPropertyName = row.Field<String>("PropertyName");
                        string RowPropertyValue = row.Field<String>("PropertyValue");

                        if (RowPropertyName == "Style")
                        {
                            string UserThemeStyle = RowPropertyValue;

                            HttpCookie UserThemeStyleCrumb = new HttpCookie("UserThemeStyle", UserThemeStyle);
                            UserThemeStyleCrumb.Expires = DateTime.Now.AddMonths(2);
                            HttpContext.Current.Response.Cookies.Add(UserThemeStyleCrumb);

                        }

                        if (RowPropertyName == "colorHeader")
                        {
                            string UserThemecolorHeader = RowPropertyValue;

                            HttpCookie UserThemecolorHeaderCrumb = new HttpCookie("UserThemecolorHeader", UserThemecolorHeader);
                            UserThemecolorHeaderCrumb.Expires = DateTime.Now.AddMonths(2);
                            HttpContext.Current.Response.Cookies.Add(UserThemecolorHeaderCrumb);

                        }

                        if (RowPropertyName == "colorSidebar")
                        {
                            string UserThemecolorSidebar = RowPropertyValue;

                            HttpCookie UserThemecolorSidebarCrumb = new HttpCookie("UserThemecolorSidebar", UserThemecolorSidebar);
                            UserThemecolorSidebarCrumb.Expires = DateTime.Now.AddMonths(2);
                            HttpContext.Current.Response.Cookies.Add(UserThemecolorSidebarCrumb);

                        }
                    }
                }

                // redirect by shortcut
                ShortcutRedirect();

                // standard redirect
                Response.Redirect(RedirectUrl, true);
            }
        }

        private void ShortcutRedirect()
        {
            if (PanelSecurity.EffectiveUser.Role == UserRole.Administrator)
                return; // not for administrators

            string shortcut = Request["shortcut"];
            if ("vps".Equals(shortcut, StringComparison.InvariantCultureIgnoreCase))
            {
                // load hosting spaces
                PackageInfo[] packages = ES.Services.Packages.GetMyPackages(PanelSecurity.EffectiveUserId);
                if (packages.Length == 0)
                    return; // no spaces - exit

                // check if some package has VPS resource enabled
                foreach (PackageInfo package in packages)
                {
                    int packageId = package.PackageId;
                    PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);
                    if (cntx.Groups.ContainsKey(ResourceGroups.VPS))
                    {
                        // VPS resource found
                        // check created VPS
                        VirtualMachineMetaItemsPaged vms = ES.Services.VPS.GetVirtualMachines(packageId, "", "", "", 0, Int32.MaxValue, false);
                        if (vms.Items.Length == 1)
                        {
                            // one VPS - redirect to its properties screen
                            Response.Redirect(PortalUtils.NavigatePageURL("SpaceVPS", "SpaceID", packageId.ToString(),
                                "ItemID=" + vms.Items[0].ItemID.ToString(), "ctl=vps_general", "moduleDefId=VPS"));
                        }
                        else
                        {
                            // several VPS - redirect to VPS list page
                            Response.Redirect(PortalUtils.NavigatePageURL("SpaceVPS", "SpaceID", packageId.ToString(),
                                "ctl=", "moduleDefId=VPS"));
                        }
                    }

                    
                    if (cntx.Groups.ContainsKey(ResourceGroups.VPS2012))
                    {
                        // VPS resource found
                        // check created VPS
                        VirtualMachineMetaItemsPaged vms = ES.Services.VPS2012.GetVirtualMachines(packageId, "", "", "", 0, Int32.MaxValue, false);
                        if (vms.Items.Length == 1)
                        {
                            // one VPS - redirect to its properties screen
                            Response.Redirect(PortalUtils.NavigatePageURL("SpaceVPS2012", "SpaceID", packageId.ToString(),
                                "ItemID=" + vms.Items[0].ItemID.ToString(), "ctl=vps_general", "moduleDefId=VPS2012"));
                        }
                        else
                        {
                            // several VPS - redirect to VPS list page
                            Response.Redirect(PortalUtils.NavigatePageURL("SpaceVPS2012", "SpaceID", packageId.ToString(),
                                "ctl=", "moduleDefId=VPS2012"));
                        }
                    }


                    if (cntx.Groups.ContainsKey(ResourceGroups.VPSForPC))
                    {
                        // VPS resource found
                        // check created VPS
                        VirtualMachineMetaItemsPaged vms = ES.Services.VPSPC.GetVirtualMachines(packageId, "", "", "", 0, Int32.MaxValue, false);
                        if (vms.Items.Length == 1)
                        {
                            // one VPS - redirect to its properties screen
                            Response.Redirect(PortalUtils.NavigatePageURL("SpaceVPSForPC", "SpaceID", packageId.ToString(),
                                "ItemID=" + vms.Items[0].ItemID.ToString(), "ctl=vps_general", "moduleDefId=VPSForPC"));
                        }
                        else
                        {
                            // several VPS - redirect to VPS list page
                            Response.Redirect(PortalUtils.NavigatePageURL("SpaceVPSForPC", "SpaceID", packageId.ToString(),
                                "ctl=", "moduleDefId=VPSForPC"));
                        }
                    }

                }

                // no VPS resources found
                // redirect to space home
                if (packages.Length == 1)
                    Response.Redirect(PortalUtils.GetSpaceHomePageUrl(packages[0].PackageId));
            }
        }

        private void SetCurrentLanguage()
        {
            PortalUtils.SetCurrentLanguage(ddlLanguage.SelectedValue);
        }

        private void SetCurrentTheme()
        {
            string selectedTheme = ddlTheme.SelectedValue;

            HttpCookie UserRTLCrub = Request.Cookies["UserRTL"];
            if (UserRTLCrub != null)
            {
                if (HttpContext.Current.Response.Cookies["UserRTL"].Value == "1")
                {
                    DataSet themeData = ES.Services.Authentication.GetLoginThemes();
                    selectedTheme = themeData.Tables[0].Rows[ddlTheme.SelectedIndex]["RTLName"].ToString();
                }
            }

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

        protected void btnResendPin_Click(object sender, EventArgs e)
        {
            if (PortalUtils.SendPin(txtUsername.Text.Trim()) == 0)
                ShowSuccessMessage("PinSend");
            else
                ShowErrorMessage("PinSendError");
        }
    }
}
