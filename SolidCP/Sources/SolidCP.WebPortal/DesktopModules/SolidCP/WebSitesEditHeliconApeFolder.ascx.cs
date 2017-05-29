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
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SolidCP.Providers;
using SolidCP.Providers.Web;

namespace SolidCP.Portal
{
    public partial class WebSitesEditHeliconApeFolder : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // bind folder
                BindFolder();
            }
        }

        private void BindFolder()
        {
            HtaccessFolder folder;

            string name = Request.QueryString["Name"];
            string spaceId = Request.QueryString["SpaceID"];

            if ("httpd.conf" == name && !string.IsNullOrEmpty(spaceId))
            {
                // read httpd.conf
                folder = ES.Services.WebServers.GetHeliconApeHttpdFolder(int.Parse(spaceId));
                ButtonDebuggerStart.Visible = false;
                ButtonDebuggerStop.Visible = false;
            }
            else
            {
                // read web site
                WebSite site = GetWebSite();

                if (site == null)
                {
                    RedirectToBrowsePage();
                    return;
                }

                LabelWebSiteName.Text = site.Name;

                folderPath.RootFolder = site.ContentPath;
                folderPath.PackageId = site.PackageId;
                htaccessContent.Text = "# Helicon Ape\n";

                ButtonDebuggerStart.Visible = true;
                ButtonDebuggerStop.Visible = false;


                if (String.IsNullOrEmpty(PanelRequest.Name))
                    return;

                // read folder
                folder = ES.Services.WebServers.GetHeliconApeFolder(PanelRequest.ItemID, PanelRequest.Name);

            }

            if (folder == null)
            {
                ReturnBack();
            }

            folderPath.SelectedFile = folder.Path;
            folderPath.Enabled = false;
            contentPath.Value = folder.ContentPath;
            htaccessContent.Text = folder.HtaccessContent;
            if (string.IsNullOrEmpty( htaccessContent.Text ))
            {
                htaccessContent.Text = "# Helicon Ape\n";
            }

            /*
            DebuggerUrlField.Value = "";

            if ( RE_APE_DEBUGGER_ENABLED.IsMatch(htaccessContent.Text) )
            {
                btnApeDebug.Text = (string)GetLocalResourceObject("btnApeDebuggerStop.Text");
                GetDebuggerUrl();
            }
            */

        }

        private void SaveFolder()
        {
            HtaccessFolder folder = new HtaccessFolder();
            folder.Path = folderPath.SelectedFile;
            folder.ContentPath = contentPath.Value;
            folder.HtaccessContent = htaccessContent.Text;

            string spaceId = Request.QueryString["SpaceID"];


            try
            {
                if (folder.Path == HtaccessFolder.HTTPD_CONF_FILE && !string.IsNullOrEmpty(spaceId))
                {
                    int result = ES.Services.WebServers.UpdateHeliconApeHttpdFolder(int.Parse(spaceId), folder);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                else
                {
                    int result = ES.Services.WebServers.UpdateHeliconApeFolder(PanelRequest.ItemID, folder);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_HELICON_APE_FOLDER", ex);
                return;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveFolder();
            ReturnBack();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveFolder();
        }

        protected readonly Regex RE_APE_DEBUGGER_ENABLED = new Regex(@"^[ \t]*(SetEnv\s+mod_developer\s+secure-key-([\d]+))", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        protected readonly Regex RE_APE_DEBUGGER_DISABLED = new Regex(@"^[ \t]*#[ \t]*(SetEnv\s+mod_developer\s+secure-key-([\d]+))", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        
        protected string DebuggerSecureKey = "";
        protected string DebuggerSessionId = "";
        protected string DebuggerUrl = "";
        protected string DebuggingPageUrl = "";

        protected void DebugStartClick(object sender, EventArgs e)
        {
            var code = htaccessContent.Text;
            bool needUpdate = false;

            if ( RE_APE_DEBUGGER_DISABLED.IsMatch(code) )
            {
                // already disabled, enable it!
                DebuggerSecureKey = RE_APE_DEBUGGER_DISABLED.Match(code).Groups[2].Value;
                code = RE_APE_DEBUGGER_DISABLED.Replace(code, "$1");
                needUpdate = true;
            }
            else if (RE_APE_DEBUGGER_ENABLED.IsMatch(code))
            {
                // already enabled
                DebuggerSecureKey = RE_APE_DEBUGGER_ENABLED.Match(code).Groups[2].Value;
                needUpdate = false;
            }
            else
            {
                DebuggerSecureKey = new Random().Next(100000000, 999999999).ToString(CultureInfo.InvariantCulture);
                code = code + "\nSetEnv mod_developer secure-key-" + DebuggerSecureKey + "\n";
                needUpdate = true;
            }

            if (needUpdate)
            {
                htaccessContent.Text = code;
                SaveFolder();
            }

            StartDebugger();
        }

        protected void DebugStopClick(object sender, EventArgs e)
        {
            var code = htaccessContent.Text;
            if (RE_APE_DEBUGGER_ENABLED.IsMatch(code))
            {
                // alerdy enable, disable it!
                code = RE_APE_DEBUGGER_ENABLED.Replace(code, "# $1");
                htaccessContent.Text = code;
                SaveFolder();
            }


            StopDebugger();
        }

        private void GetDebuggerUrl()
        {
            // TODO: interactive binding selection

            if ( !string.IsNullOrEmpty(DebuggerSecureKey) )
            {
                WebSite site = GetWebSite();

                if ( null != site)
                {
                    if (site.Bindings.Length > 0)
                    {
                        ServerBinding serverBinding = site.Bindings[0];
                        DebuggerUrl = string.Format(
                            "{0}://{1}:{2}{3}/_ape_start_developer_session?ape_debug=secure-key-{4}_{5}", 
                            serverBinding.Protocol,
                            serverBinding.Host ?? serverBinding.IP,
                            serverBinding.Port,
                            folderPath.SelectedFile.Replace('\\', '/'),
                            DebuggerSecureKey,
                            DebuggerSessionId
                        );
                        DebuggerUrlField.Value = DebuggerUrl;
                    }
                }
            }

            // TODO: throw error if debugger url is empty
        }

        private WebSite GetWebSite()
        {
            WebSite webSite = ViewState["HtaccessWebSite"] as WebSite;
            if (null == webSite)
            {
                webSite = ES.Services.WebServers.GetWebSite(PanelRequest.ItemID);
                // TODO: ViewState["HtaccessWebSite"] = webSite;
            }

            return webSite;
        }

        private void GetDebuggingPageUrl()
        {
            if (!string.IsNullOrEmpty(DebuggerSecureKey))
            {
                WebSite site = GetWebSite();

                if (null != site)
                {
                    if (site.Bindings.Length > 0)
                    {
                        ServerBinding serverBinding = site.Bindings[0];

                        DebuggingPageUrl = string.Format(
                            "{0}://{1}:{2}{3}/?ape_debug=secure-key-{4}_{5}",
                            serverBinding.Protocol,
                            serverBinding.Host ?? serverBinding.IP,
                            serverBinding.Port,
                            folderPath.SelectedFile.Replace('\\', '/'),
                            DebuggerSecureKey,
                            DebuggerSessionId
                        );
                    }
                }
            }

            // TODO: throw error if url is empty
        }


        private void StartDebugger()
        {
            ButtonDebuggerStart.Visible = false;
            ButtonDebuggerStop.Visible = true;

            // session id
            DebuggerSessionId = new Random().Next(100000000, 999999999).ToString(CultureInfo.InvariantCulture);

            // debugger url
            GetDebuggerUrl();

            // debugging page url
            GetDebuggingPageUrl();

            // show debugger iframe
            DebuggerFramePanel.Visible = true;
            DebuggerFrame.Attributes["src"] = DebuggerUrl;

            // debugging page link
            LinkDebuggingPage.NavigateUrl = DebuggingPageUrl;
            LinkDebuggingPage.Text = DebuggingPageUrl;
            DebuggingPageLinkModal.Show();

        }

        private void StopDebugger()
        {
            ButtonDebuggerStart.Visible = true;
            ButtonDebuggerStop.Visible = false;

            DebuggerUrl = "";
            DebuggingPageUrl = "";
            DebuggerSessionId = "";

            // hide debugger iframe
            DebuggerFramePanel.Visible = false;
        }

        protected void BtnCancelClick(object sender, EventArgs e)
        {
            ReturnBack();
        }

        private void ReturnBack()
        {
            string returnUrlBase64 = Request.QueryString["ReturnUrlBase64"];
            if (!string.IsNullOrEmpty(returnUrlBase64))
            {

                Response.Redirect(Server.UrlDecode(DecodeFrom64(returnUrlBase64)));
            }
            else
            {
                Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_item",
                                          "MenuID=htaccessfolders",
                                          PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
            }
        }

        static public string DecodeFrom64(string encodedData)
        {
            return System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(encodedData));
        }
    }
}
