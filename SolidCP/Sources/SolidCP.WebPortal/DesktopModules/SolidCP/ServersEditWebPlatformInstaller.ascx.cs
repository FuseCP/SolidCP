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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SolidCP.Server;
using SolidCP.Providers;
using System.Reflection;

namespace SolidCP.Portal
{

    public partial class ServersEditWebPlatformInstaller : SolidCPModuleBase
    {
        protected const string _wpiLogsDirViewStateKey = "WPI_LOGS_DIR";

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterClientScriptInclude("jquery", ResolveUrl("~/JavaScript/jquery-1.4.4.min.js"));

            if (!IsPostBack)
            {


                try
                {
                    if (!ES.Services.Servers.CheckLoadUserProfile(PanelRequest.ServerId))
                    {
                        CheckLoadUserProfilePanel.Visible = true;
                    }
                }
                catch (AmbiguousMatchException)
                {
                    CheckLoadUserProfilePanel.Visible = false;
                    ShowWarningMessage("Server application pool \"Load User Profile\" setting unavailable. IIS7 or higher is expected.");
                }
                catch (Exception ex)
                {
                    CheckLoadUserProfilePanel.Visible = false;
                    ProductsPanel.Visible = false;
                    keywordsList.Visible = false;
                    SearchPanel.Visible = false;
                    InstallButtons1.Visible = false;
                    InstallButtons2.Visible = false;

                    ShowErrorMessage("WPI_CHECK_LOAD_USER_PROFILE", ex);
                }


                try
                {
                    ES.Services.Servers.InitWPIFeeds(PanelRequest.ServerId);
                }
                catch
                {
                    ProductsPanel.Visible = false;
                    keywordsList.Visible = false;
                    SearchPanel.Visible = false;
                    InstallButtons1.Visible = false;
                    InstallButtons2.Visible = false;

                    ShowWarningMessage("No products available. Please enable Web Platfrom Installer feeds in System Settings.");

                    return;
                }

                

                ProgressAnimation.ImageUrl = PortalUtils.GetThemedImage("indicator_medium.gif");
                ViewState["SearchResultShown"] = false;
                BindWpiProducts();

                string message = UpdateProgress();
                if (!string.IsNullOrEmpty(message))
                {
                    ShowProgressPanel();
                }

                string gotoProducts = Request.QueryString["WPIProduct"];
                if (!string.IsNullOrEmpty(gotoProducts))
                {
                    ArrayList wpiProductsForInstall = GetProductsToInstallList();
                    wpiProductsForInstall.AddRange(gotoProducts.Split(','));
                    SetProductsToInstallList(wpiProductsForInstall);

                    btnInstall_Click(sender, e);
                }

            }
        }

        private void BindWpiProducts()
        {
            try
            {

                keywordsList.Items.Clear();
                List<WPIKeyword> keywords = new List<WPIKeyword>(ES.Services.Servers.GetWPIKeywords(PanelRequest.ServerId));
                keywords.Add(new WPIKeyword("Show installed", "Show Installed"));
                keywordsList.DataSource = keywords;
                keywordsList.DataTextField = "Text";
                keywordsList.DataValueField = "ID";
                keywordsList.DataBind();

                if (keywordsList.Items.Count > 0)
                {
                    keywordsList.Items[0].Attributes["class"] = "selected";
                }

                gvWpiProducts.DataSource = FilterInstaledProducts(ES.Services.Servers.GetWPIProducts(PanelRequest.ServerId, null, SelectedKeywordValue));
                gvWpiProducts.DataBind();

            }
            catch (Exception ex)
            {
                ShowErrorMessage("SERVER_GET_WIN_SERVICES", ex);
                return;
            }
        }

        private string SelectedKeywordValue
        {
            get
            {
                return IsShowInstalledPseudoKeywordSelected ? null : keywordsList.SelectedValue;
            }
        }

        private bool IsShowInstalledPseudoKeywordSelected
        {
            get { return keywordsList.SelectedIndex == keywordsList.Items.Count - 1; }
        }


        private ArrayList GetProductsToInstallList()
        {
            if (ViewState["wpiProductsForInstall"] != null)
            {
                return (ArrayList)ViewState["wpiProductsForInstall"];
            }
            return new ArrayList();
        }

        private void SetProductsToInstallList(ArrayList wpiProductsForInstall)
        {
            ViewState["wpiProductsForInstall"] = wpiProductsForInstall;
        }

        private void ShowProductsGrid()
        {
            ProductsPanel.Visible = true;
            keywordsList.Visible = true;
            SearchPanel.Visible = true;
            InstallButtons1.Visible = true;
            InstallButtons2.Visible = true;

            NoProductsSelectedPanel.Visible = false;

            InstallPanel.Visible = false;
            AceptDEclineButtons.Visible = false;
            ProgressPanel.Visible = false;
            ProgressTimer.Enabled = false;
            InstallCompletedPanel.Visible = false;

            ProductsPanel.Update();
            InstallPanel.Update();
            ProgressPanel.Update();
        }

        private void ShowNoProductsSelectedPanel()
        {
            ProductsPanel.Visible = false;
            keywordsList.Visible = false;
            SearchPanel.Visible = false;
            InstallButtons1.Visible = false;
            InstallButtons2.Visible = false;

            NoProductsSelectedPanel.Visible = true;

            InstallPanel.Visible = false;
            AceptDEclineButtons.Visible = false;
            ProgressPanel.Visible = false;
            ProgressTimer.Enabled = false;
            InstallCompletedPanel.Visible = false;

            ProductsPanel.Update();
            InstallPanel.Update();
            ProgressPanel.Update();
        }

        private void ShowSelectedProducts()
        {
            ProductsPanel.Visible = false;
            keywordsList.Visible = false;
            SearchPanel.Visible = false;
            InstallButtons1.Visible = false;
            InstallButtons2.Visible = false;
            ProgressPanel.Visible = false;
            ProgressTimer.Enabled = false;
            InstallCompletedPanel.Visible = false;

            InstallPanel.Visible = true;
            AceptDEclineButtons.Visible = true;

            ProductsPanel.Update();
            InstallPanel.Update();
            ProgressPanel.Update();
        }

        private void ShowProgressPanel()
        {
            ProductsPanel.Visible = false;
            keywordsList.Visible = false;
            SearchPanel.Visible = false;
            InstallButtons1.Visible = false;
            InstallButtons2.Visible = false;
            InstallPanel.Visible = false;
            AceptDEclineButtons.Visible = false;
            InstallCompletedPanel.Visible = false;

            ProgressPanel.Visible = true;
            ProgressTimer.Enabled = true;

            ProductsPanel.Update();
            InstallPanel.Update();
            ProgressPanel.Update();
        }

        protected void gvWpiProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvWpiProducts.PageIndex = ((System.Web.UI.WebControls.GridViewPageEventArgs)e).NewPageIndex;
            if ((bool)ViewState["SearchResultShown"])
            {
                gvWpiProducts.DataSource = GetWpiProductsFiltered();
            }
            else
            {
                gvWpiProducts.DataSource = FilterInstaledProducts(ES.Services.Servers.GetWPIProducts(PanelRequest.ServerId, null, SelectedKeywordValue));
            }
            gvWpiProducts.DataBind();

        }
  

        protected void gvWpiProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "WpiAdd")
            {
                ((Button)e.CommandSource).Text = "- remove";
                ((Button)e.CommandSource).CommandName = "WpiRemove";

                ArrayList wpiProductsForInstall = GetProductsToInstallList();
                wpiProductsForInstall.Add(e.CommandArgument.ToString());
                SetProductsToInstallList(wpiProductsForInstall);

            }

            if (e.CommandName == "WpiRemove")
            {
                ((Button)e.CommandSource).Text = "+ add";
                ((Button)e.CommandSource).CommandName = "WpiAdd";

                ArrayList wpiProductsForInstall = GetProductsToInstallList();
                wpiProductsForInstall.Remove(e.CommandArgument.ToString());
                SetProductsToInstallList(wpiProductsForInstall);


            }

        }

      
        private void UpdateProductsByKeyword()
        {
            gvWpiProducts.DataSource = FilterInstaledProducts(
                ES.Services.Servers.GetWPIProducts(PanelRequest.ServerId, null, SelectedKeywordValue)
            );
            gvWpiProducts.DataBind();
        }

        protected void keywordsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvWpiProducts.PageIndex = 0;
            
            keywordsList.SelectedItem.Attributes["class"] = "selected";
            searchBox.Text = "";

            UpdateProductsByKeyword();
        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
            string[] productsToInstall = (string[]) GetProductsToInstallList().ToArray(typeof (string));
            if (null == productsToInstall || productsToInstall.Length == 0)
            {
                ShowNoProductsSelectedPanel();
            }
            else
            {
                // get dependencies and show it
                gvWpiInstall.DataSource = ES.Services.Servers.GetWPIProductsWithDependencies(PanelRequest.ServerId, productsToInstall);
                gvWpiInstall.DataBind();
                ShowSelectedProducts();
            }
        }

        protected void btnDecline_Click(object sender, EventArgs e)
        {
            ShowProductsGrid();
        }

        private IAsyncResult WPIInstallationResult;
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            WPIInstallationResult = ES.Services.Servers.BeginInstallWPIProducts(
                PanelRequest.ServerId, 
                (string[])GetProductsToInstallList().ToArray(typeof(string)), 
                null,
                null
                );
            ShowProgressPanel();
            UpdateProgress();
        }

        private string UpdateProgress()
        {

            try
            {
                string wpiLogsDir = ViewState[_wpiLogsDirViewStateKey] as string;
                if (string.IsNullOrEmpty(wpiLogsDir))
                {
                    wpiLogsDir = ES.Services.Servers.WpiGetLogFileDirectory(PanelRequest.ServerId);
                    if (!string.IsNullOrEmpty(wpiLogsDir))
                    {
                        ViewState[_wpiLogsDirViewStateKey] = wpiLogsDir;
                    }
                }
            }
            catch 
            {
                //
            }
            

            string message = ES.Services.Servers.GetWPIStatus(PanelRequest.ServerId);
            if (!string.IsNullOrEmpty(message))
            {
                ProgressMessage.Text = message.Replace("\n", "<br/>");
            }
            else
            {
                //ProgressMessage.Text = "please wait...";
            }


            return message;
        }

        protected void ProgressTimerTick(object sender, EventArgs e)
        {
            string message = UpdateProgress();

            if (string.IsNullOrEmpty(message))
            {
                ShowInstallCompletedPanel();
                //ProgressTimer.Enabled = false;
            }
            else if (message.IndexOf("failed", StringComparison.OrdinalIgnoreCase) > 0)
            {
                // some installation failed
                ShowInstallFailedPanel();
            }
        }

        private void ShowInstallFailedPanel()
        {
            ProgressMessagePanel.Visible = true;
            InstallCompletedPanel.Visible = true;
            ProgressAnimation.Style["display"] = "none";
            CancelInstall.Visible = false;

            LabelInstallationFailed.Visible = true;
            LabelInstallationSuccess.Visible = false;

            ShowLogButton();
        }

        private void ShowInstallCompletedPanel()
        {
            if (!LabelInstallationFailed.Visible)
            {
                ProgressMessagePanel.Visible = false;
            }
            InstallCompletedPanel.Visible = true;
            ProgressAnimation.Style["display"] = "none";

            if (!InstalledProductsList.Visible)
            {
                string[] productsToInstall = (string[]) GetProductsToInstallList().ToArray(typeof (string));
                foreach (string product in productsToInstall)
                {
                    InstalledProductsList.Items.Add(new ListItem(product));
                }

                InstalledProductsList.Visible = true;
            }

            ShowLogButton();

            // fix btnBackToServer button text
            if (null != Request["returnurl"])
            {
                btnBackToServer.Text = "Ok";
                BackToGalleryButton.Visible = false;
            }
        }

        private void ShowLogButton()
        {
            string wpiLogsDir = ViewState[_wpiLogsDirViewStateKey] as string;
            if (!string.IsNullOrEmpty(wpiLogsDir))
            {
                ShowLogsButton.Visible = true;
            }
        }

        protected void gvWpiInstall_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            WPIProduct p = (WPIProduct)e.Row.DataItem;
            if (p != null)
            {
                Label labelFileSize = (Label)e.Row.FindControl("downloadSize");
                if (p.FileSize == 0 )
                {
                    if (labelFileSize != null)
                    {
                        labelFileSize.Visible = false;
                    }
                }
                else
                {
                    if (labelFileSize != null)
                    {
                        labelFileSize.Text += " " + p.FileSize + " Kb";
                    }
                }
                

                if (string.IsNullOrEmpty( p.EulaUrl ) )
                {
                    HyperLink hl = (HyperLink)e.Row.FindControl("eulaLink");
                    if (hl != null)
                    {
                        hl.Visible = false;
                    }
                }

                if (string.IsNullOrEmpty( p.DownloadedLocation ) )
                {
                    Label label = (Label)e.Row.FindControl("labelDownloaded");
                    if (label != null)
                    {
                        label.Visible = false;
                    }
                }
            }
        }


        protected void BackToGalleryButtonClick(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_platforminstaller"));
        }

        protected void CacnelInstallButtonClick(object sender, EventArgs e)
        {
            ES.Services.Servers.CancelInstallWPIProducts(PanelRequest.ServerId);
            ProgressMessage.Text = "Canceling...";
            LabelInstallationSuccess.Text = "Installation canceled";
        }

        protected void NoProductsBackButtonClick(object sender, EventArgs e)
        {
            ShowProductsGrid();
        }

        protected string IsAddedText(string productId)
        {
            if ( GetProductsToInstallList().Contains(productId) )
            {
                return "- remove";
            }
            else
            {
                return "+ add";
            }
        }

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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string redirectUrl = "";
            if (Request["returnurl"] != null)
            {
                redirectUrl = HttpUtility.UrlDecode(Request["returnurl"]);
                if (!IsLocalUrl(redirectUrl))
                {
                    redirectUrl = "";
                }
            }

            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = EditUrl("ServerID", PanelRequest.ServerId.ToString(), "edit_server");
            }

            Response.Redirect(redirectUrl);
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            keywordsList.ClearSelection();
            ViewState["SearchResultShown"] = true;
            gvWpiProducts.PageIndex = 0;
            gvWpiProducts.DataSource = GetWpiProductsFiltered();
            gvWpiProducts.DataBind();
        }

        private WPIProduct[] GetWpiProductsFiltered()
        {
            return ES.Services.Servers.GetWPIProductsFiltered(PanelRequest.ServerId, searchBox.Text);
        }

        protected ArrayList FilterInstaledProducts(WPIProduct[] products)
        {
            ViewState["SearchResultShown"] = false;

            ArrayList result = new ArrayList();

            foreach (WPIProduct product in products)
            {
                if (product.IsInstalled == IsShowInstalledPseudoKeywordSelected)
                {
                    result.Add(product);
                }
            }

            return result;
        }

        protected string FixDefaultLogo(string imgUrl)
        {
            if (string.IsNullOrEmpty(imgUrl))
            {
                return GetThemedImage("../icons/dvd_disc_48.png");
            }

            return imgUrl;
        }

        protected void ShowLogsButton_OnClick(object sender, EventArgs e)
        {
            //show logs

            string wpiLogsDir = null;
            object[] logsEntrys = null;
            try
            {
                wpiLogsDir = ViewState[_wpiLogsDirViewStateKey] as string;

                if (!string.IsNullOrEmpty(wpiLogsDir))
                {
                    //Get logs !!!
                    logsEntrys = ES.Services.Servers.WpiGetLogsInDirectory(PanelRequest.ServerId, wpiLogsDir);

                    if (null == logsEntrys)
                    {
                        WpiLogsPanel.Visible = true;
                        string msg = string.Format("Could not get logs files. Log files folder:\n{0}", wpiLogsDir);
                        WpiLogsPre.InnerText = msg;
                    }
                    else
                    {
                        WpiLogsPanel.Visible = true;

                        StringBuilder sb = new StringBuilder();

                        foreach (SettingPair entry in logsEntrys)
                        {
                            string fileName = (string) entry.Name;
                            string fileContent = (string) entry.Value;
                            sb.AppendLine().AppendFormat("<h3>{0}</h3>", fileName).AppendLine().Append(fileContent).
                                AppendLine();
                        }

                        WpiLogsPre.InnerHtml = sb.ToString();

                        ShowLogsButton.Visible = false;
                        ProgressTimer.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                WpiLogsPanel.Visible = true;
                string msg = string.Format("wpiLogsDir: {0}\nlogsEntrys is null: {1}\n{2}", wpiLogsDir, logsEntrys == null, ex);
                WpiLogsPre.InnerText = msg;
            }
        }

        protected void EnableLoadUserProfileButton_OnClick(object sender, EventArgs e)
        {
            ES.Services.Servers.EnableLoadUserProfile(PanelRequest.ServerId);
            CheckLoadUserProfilePanel.Visible = false;
        }
    }
}
