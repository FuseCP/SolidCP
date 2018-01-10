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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections.Specialized;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Web;
using SolidCP.Providers.Common;
using System.IO;
using System.Linq;
using SolidCP.Providers;

namespace SolidCP.Portal
{
    public partial class WebsitesSSL : SolidCPControlBase
    {
        public const string WEB_SSL_DELETE = "WEB_SSL_DELETE";
        public const string WEB_SSL_EXPORT = "WEB_SSL_EXPORT";
        public const string WEB_GEN_CSR = "WEB_GEN_CSR";
        public const string ERROR_CSR = "ERROR_CSR";


        public SSLCertificate InstalledCert
        {
            get { return (SSLCertificate)ViewState["InstalledCert"]; }
            set { ViewState["InstalledCert"] = value; }
        }
        public int SiteId
        {
            get { return (int)ViewState["SiteId"]; }
            set { ViewState["SiteId"] = value; }
        }

        public SSLCertificate PendingCert { get; set; }

        public string State
        {
            get
            {
                if (ddlStates.Visible)
                    return ddlStates.SelectedValue;
                else
                    return txtState.Text.Trim();
            }
            set
            {
                EnsureChildControls();
                //
                if (ddlStates.Visible)
                    ddlStates.SelectedValue = value;
                else
                    txtState.Text = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load countries
                BindCountries();
                BindStates();
            }

        }

        protected void lstCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindStates();
        }

        private void BindCountries()
        {
            PortalUtils.LoadCountriesDropDownList(lstCountries, null);
            lstCountries.Items.Insert(0, new ListItem(GetSharedLocalizedString("ListItem.NotSpecified"), ""));
        }

        private void BindStates()
        {
            ddlStates.Visible = false;
            txtState.Visible = true;

            if (lstCountries.SelectedValue != "")
            {
                // Load states using default mechanism
                PortalUtils.LoadStatesDropDownList(ddlStates, lstCountries.SelectedValue);
                // Correct list values because no abbreviations is allowed in state name
                foreach (ListItem li in ddlStates.Items)
                {
                    // Replace state abbreviation with its full name
                    li.Value = li.Text;
                }

                if (ddlStates.Items.Count > 0)
                {
                    ddlStates.Items.Insert(0, new ListItem(GetSharedLocalizedString("ListItem.NotSpecified")));
                    ddlStates.Visible = true;
                    txtState.Visible = false;
                }
            }
        }

        private void BindListOfAvailableSslDomains(string defaultBindingName)
        {
            var domains = ES.Services.WebServers.GetWebSitePointers(SiteId).ToList();

            // If no pointers at all, add website default domain
            if (domains.All(d => d.DomainName != defaultBindingName))
            {
                domains.Add(new DomainInfo() { DomainName = defaultBindingName, IsDomainPointer = false });
            }

            ddlbSiteCertificate.Items.AddRange(domains.Select(d => new ListItem(d.DomainName)).ToArray());
            ddlbSiteCertificate.Items.AddRange(domains.Where(d => !d.IsDomainPointer).Select(d => new ListItem("*." + d.DomainName)).ToArray());
        }

        public void BindWebItem(WebAppVirtualDirectory item)
        {
            //
            var webSite = item as WebSite;
            // Skip processing virtual directories, otherwise we will likely run into a trouble
            if (webSite == null)
                return;
            SiteId = item.Id;
            RefreshControlLayout();
        }

        protected void btnShowpnlCSR_click(object sender, EventArgs e)
        {
            SSLNotInstalled.Visible = false;
            pnlCSR.Visible = true;
        }

        protected void btnShowUpload_click(object sender, EventArgs e)
        {
            SSLNotInstalled.Visible = false;
            pnlShowUpload.Visible = true;
        }

        protected void btnCSR_Click(object sender, EventArgs e)
        {
            //
            string distinguishedName = string.Format(@"CN={0},
                                                     O={1},
                                                     OU={2},                                                                                                  
                                                     L={3},
                                                     S={4},                                                
                                                     C={5}",
                                                    //rbSiteCertificate.Checked ? rbSiteCertificate.Text : rbDomainCertificate.Text,
                                                    ddlbSiteCertificate.SelectedValue,
                                                        txtCompany.Text,
                                                        txtOU.Text,
                                                        txtCity.Text,
                                                        State,
                                                        lstCountries.SelectedValue);

            SSLCertificate certificate = new SSLCertificate();
            certificate.Hostname = ddlbSiteCertificate.SelectedValue;   //rbSiteCertificate.Checked ? rbSiteCertificate.Text : rbDomainCertificate.Text;
            certificate.DistinguishedName = distinguishedName;
            certificate.CSRLength = Convert.ToInt32(lstBits.SelectedValue);
            certificate.Organisation = txtCompany.Text;
            certificate.OrganisationUnit = txtOU.Text;
            certificate.SiteID = PanelRequest.ItemID;
            certificate.State = State;
            certificate.City = txtCity.Text;
            certificate.Country = lstCountries.SelectedValue;
            certificate.IsRenewal = false;
            certificate = ES.Services.WebServers.CertificateRequest(certificate, certificate.SiteID);
            // Something is wrong
            if (certificate.CSR == "")
            {
                messageBox.ShowErrorMessage(ERROR_CSR);
                return;
            }
            // We are done
            SSLNotInstalled.Visible = false;
            pnlCSR.Visible = false;
            tabCSR.HeaderText = GetLocalizedString("tabPendingCertificate.HeaderText");
            ViewState["CSRID"] = certificate.id;
            pnlInstallCertificate.Visible = true;
            txtCSR.Text = certificate.CSR;
            txtCSR.Attributes.Add("onfocus", "this.select();");
        }

        protected void btnRegenCSR_Click(object sender, EventArgs e)
        {
            ResultObject result = new ResultObject { IsSuccess = true };
            try
            {
                result = ES.Services.WebServers.DeleteCertificateRequest(SiteId, (int)ViewState["CSRID"]);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage(WEB_SSL_DELETE, ex);
            }
            //
            if (!result.IsSuccess)
            {
                messageBox.ShowErrorMessage(WEB_SSL_DELETE);
                return;
            }
            //
            SSLNotInstalled.Visible = false;
            pnlCSR.Visible = true;
            pnlInstallCertificate.Visible = false;
        }

        protected void btnRenCSR_Click(object sender, EventArgs e)
        {
            string distinguishedName = string.Format(@"CN={0},
                                                     O={1},
                                                     OU={2},                                                                                                  
                                                     L={3},
                                                     S={4},                                                
                                                     C={5}", ddlbSiteCertificate.SelectedValue, //rbSiteCertificate.Checked ? rbSiteCertificate.Text : rbDomainCertificate.Text,
                                                             txtCompany.Text,
                                                             txtOU.Text,
                                                             txtCity.Text,
                                                             State,
                                                             lstCountries.SelectedValue);

            SSLCertificate certificate = new SSLCertificate();
            certificate.Hostname = ddlbSiteCertificate.SelectedValue;   //rbSiteCertificate.Checked ? rbSiteCertificate.Text : rbDomainCertificate.Text;
            certificate.DistinguishedName = distinguishedName;
            certificate.CSRLength = Convert.ToInt32(lstBits.SelectedValue);
            certificate.Organisation = txtCompany.Text;
            certificate.OrganisationUnit = txtOU.Text;
            certificate.SiteID = PanelRequest.ItemID;
            certificate.State = State;
            certificate.City = txtCity.Text;
            certificate.Country = lstCountries.SelectedValue;
            certificate.PreviousId = InstalledCert.id;
            certificate.IsRenewal = true;
            certificate = ES.Services.WebServers.CertificateRequest(certificate, certificate.SiteID);

            // Something is wrong
            if (certificate.CSR == "")
            {
                messageBox.ShowErrorMessage(WEB_GEN_CSR);
                return;
            }

            //
            pnlShowUpload.Visible = false;
            pnlCSR.Visible = false;
            ViewState["CSRID"] = certificate.id;
            txtCSR.Attributes.Add("onfocus", "this.select();");
            RefreshControlLayout();
            TabContainer1.ActiveTab = TabContainer1.Tabs[0];
            messageBox.ShowSuccessMessage(WEB_GEN_CSR);
        }

        protected void btnInstallCertificate_Click(object sender, EventArgs e)
        {
            InstallCertificate(PanelRequest.ItemID, txtCertificate.Text);
        }

        protected void LEInstallCertificate_Click(object sender, EventArgs e)
        {
            LEInstallCertificate(PanelRequest.ItemID);
        }

        protected void LEInstallCertificate(int webSiteId)
        {
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            UserInfo user = UsersHelper.GetUser(package.UserId);

            ResultObject result = ES.Services.WebServers.LEInstallCertificate(webSiteId, user.Email);
            // Check the operation status
            if (!result.IsSuccess)
            {
                messageBox.ShowErrorMessage("WEB_INSTALL_LE");
                return;
            }
            //
            //messageBox.ShowSuccessMessage("WEB_INSTALL_LE");
            tabLEInstalled.Visible = true;
            tabLEInstalled.Enabled = true;
            tabInstalled.HeaderText = "Installed Lets Encrypt Certificate";
            pnlInstallCertificate.Visible = false;
            SSLNotInstalled.Visible = false;
            //
            RefreshControlLayout();
            TabContainer1.ActiveTab = TabContainer1.Tabs[0];
            messageBox.ShowSuccessMessage("WEB_INSTALL_LE");
            TabContainer1.ActiveTab = tabInstalled;
        }

            protected void InstallCertificate(int webSiteId, string certText)
        {
            SSLCertificate certificate = ES.Services.WebServers.GetSSLCertificateByID((int)ViewState["CSRID"]);
            certificate.Certificate = certText;

            ResultObject result = ES.Services.WebServers.InstallCertificate(certificate, webSiteId);
            // Check the operation status
            if (!result.IsSuccess)
            {
                messageBox.ShowErrorMessage("WEB_INSTALL_CSR");
                return;
            }
            //
            messageBox.ShowSuccessMessage("WEB_INSTALL_CSR");
            tabInstalled.Visible = true;
            tabInstalled.Enabled = true;
            tabInstalled.HeaderText = "Installed Certificate";
            tabCSR.HeaderText = "New Certificate";
            pnlInstallCertificate.Visible = false;
            SSLNotInstalled.Visible = true;
            //
            TabContainer1.ActiveTab = tabInstalled;

            RefreshControlLayout();
        }

        protected void btnInstallPFX_Click(object sender, EventArgs e)
        {
            InstallPfxFromClient(PanelRequest.ItemID);
        }

        protected void InstallPfxFromClient(int webSiteId)
        {
            if (upPFX.HasFile.Equals(false))
            {
                messageBox.ShowErrorMessage("WEB_SSL_NOFILE");
                return;
            }

            byte[] pfx = upPFX.FileBytes;
            string certPassword = txtPFXInstallPassword.Text;

            ResultObject result = ES.Services.WebServers.InstallPfx(pfx, webSiteId, txtPFXInstallPassword.Text);

            // Check the operation status
            if (result.IsSuccess.Equals(false))
            {
                messageBox.ShowErrorMessage("WEB_INSTALL_CSR");
                RefreshControlLayout();
                return;
            }

            messageBox.ShowSuccessMessage("WEB_INSTALL_CSR");
            SSLNotInstalled.Visible = false;
            tabInstalled.Visible = true;
            RefreshControlLayout();
        }

        protected void BindCertificateFields()
        {
            // Decode certificate's fields
            Lookup<string, string> dnFields = DecodeDn(InstalledCert.DistinguishedName);
            // Set certificate's Hostname
            lblInstalledDomain.Text = InstalledCert.Hostname;
            // Set certificate's Expiration Date
            lblInstalledExpiration.Text = InstalledCert.ExpiryDate.ToShortDateString();
            // Set Organization
            lblInstalledOrganization.Text = InstalledCert.Organisation = dnFields["O"].SingleOrDefault();
            // Set Organizational Unit
            lblInstalledOU.Text = InstalledCert.OrganisationUnit = dnFields["OU"].LastOrDefault();
            // Set City
            lblInstalledCity.Text = InstalledCert.City = dnFields["L"].FirstOrDefault();
            // Set State
            lblinstalledState.Text = InstalledCert.State = dnFields["S"].FirstOrDefault();
            // Set Country
            lblInstalledCountry.Text = InstalledCert.Country = dnFields["C"].FirstOrDefault();
            // Set Certificate Strength
            lblInstalledBits.Text = InstalledCert.CSRLength.ToString();
        }

        public Lookup<string, string> DecodeDn(string distinguisedName)
        {
            return (Lookup<string, string>)new Regex(@"(\w{1,2})=([a-zA-Z0-9\.\-\s\(\)]+),*").Matches(distinguisedName).Cast<Match>().ToLookup(x => x.Groups[1].Value, x => x.Groups[2].Value);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ExportCertificate(PanelRequest.ItemID, InstalledCert.Hostname,
                InstalledCert.SerialNumber, txtPFXPassConfirm.Text);
        }

        protected void ExportCertificate(int webSiteId, string certHostname, string certSerialNumber, string certPassword)
        {
            try
            {
                byte[] pfx = ES.Services.WebServers.ExportCertificate(webSiteId, certSerialNumber, certPassword);
                //
                modalPfxPass.Hide();
                //
                Response.Clear();
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.pfx", certHostname));
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(pfx);
                Response.End();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage(WEB_SSL_EXPORT, ex);
            }
            //
            messageBox.ShowSuccessMessage(WEB_SSL_EXPORT);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteCertificate(PanelRequest.ItemID, InstalledCert);
        }

        protected void DeleteCertificate(int webSiteId, SSLCertificate siteCert)
        {
            ResultObject result = ES.Services.WebServers.DeleteCertificate(webSiteId, siteCert);
            if (!result.IsSuccess)
            {
                // Show error message
                messageBox.ShowErrorMessage(WEB_SSL_DELETE);
                return;
            }
            // Show success message
            messageBox.ShowSuccessMessage(WEB_SSL_DELETE);
            //
            tabInstalled.Visible = false;
            tabInstalled.Enabled = false;
            tabInstalled.HeaderText = "";
            InstalledCert = null;
        }

        protected void btnRenew_Click(object sender, EventArgs e)
        {
            RenewCertificate(InstalledCert);
        }

        protected void RenewCertificate(SSLCertificate cert)
        {
            TabContainer1.ActiveTab = TabContainer1.Tabs[1];
            SSLNotInstalled.Visible = false;
            pnlCSR.Visible = true;
            tabCSR.HeaderText = GetLocalizedString("SSLGenereateRenewal.HeaderText");

            string hostname = cert.Hostname;

            // Assign hostname
            SetCertHostnameSelection(hostname);
            // Assign state
            SetCertCountrySelection(cert.Country);
            // Assign country
            SetCertStateSelection(cert.State);
            // Assign certificate strength
            MakeSafeListSelection(lstBits, cert.CSRLength.ToString());
            //
            txtCompany.Text = cert.Organisation;
            txtOU.Text = lblInstalledOU.Text;
            txtCity.Text = cert.City;

            // Render button controls appropriately
            btnCSR.Visible = false;
            btnRenCSR.Visible = true;
        }

        protected void btnImport_click(object sender, EventArgs e)
        {
            ImportCertificate(PanelRequest.ItemID);
        }

        protected void ImportCertificate(int webSiteId)
        {
            // Try to import the certificate
            ResultObject result = ES.Services.WebServers.ImportCertificate(webSiteId);
            // Display error message
            if (!result.IsSuccess)
            {
                messageBox.ShowErrorMessage("WEB_INSTALL_CSR");
                RefreshControlLayout();
                return;
            }
            // Show success message and display appropriate controls
            messageBox.ShowSuccessMessage("WEB_INSTALL_CSR");

            RefreshControlLayout();
        }

        public void RefreshControlLayout()
        {
            //
            bool hasactive = false;
            bool haspending = false;

            try
            {
                var webSite = ES.Services.WebServers.GetWebSite(SiteId);

                // Get all certificate infos stored in database
                SSLCertificate[] certificates = ES.Services.WebServers.GetCertificatesForSite(SiteId);

                // Set some default visible values, states and texts
                tabInstalled.Visible = false;
                tabInstalled.Enabled = false;
                SSLNotInstalled.Visible = true;
                SSLImport.Visible = false;
                pnlCSR.Visible = false;
                pnlShowUpload.Visible = false;
                pnlInstallCertificate.Visible = false;

                btnShowpnlCSR.Attributes.Remove("OnClientClick");
                btnShowUpload.Attributes.Remove("OnClientClick");
                SSLNotInstalledHeading.Text = GetLocalizedString("SSLNotInstalledHeading.Text");
                SSLNotInstalledDescription.Text = GetLocalizedString("SSLNotInstalledDescription.Text");

                BindListOfAvailableSslDomains(webSite.Name);

                if (certificates.Length > 0)
                {
                    foreach (SSLCertificate cert in certificates)
                    {
                        if (cert.Installed)
                        {
                            hasactive = true;
                        }
                        else
                        {
                            haspending = true;
                        }
                    }
                }

                // Web site has active certificate
                if (hasactive)
                {
                    tabInstalled.Visible = true;
                    tabInstalled.Enabled = true;
                    tabInstalled.HeaderText = GetLocalizedString("tabInstalled.Text");

                    InstalledCert = (from c in certificates
                                     where c.Installed == true
                                     select c).SingleOrDefault();
                    //
                    BindCertificateFields();
                    // Attention please, the certificate is about to expire!
                    TimeSpan daystoexp = InstalledCert.ExpiryDate - DateTime.Now;
                    if (daystoexp.Days < 30)
                    {
                        lblInstalledExpiration.ForeColor = System.Drawing.Color.Red;
                    }
                    // Put some data to the ViewState
                    ViewState["SSLID"] = InstalledCert.id;
                    ViewState["SSLSerial"] = InstalledCert.SerialNumber;
                    //
                    if (!haspending)
                    {
                        btnShowpnlCSR.Attributes.Add("OnClientClick", "return confirm('" + GetLocalizedString("btnInstallConfirm.Text") + "');");
                        btnShowUpload.Attributes.Add("OnClientClick", "return confirm('" + GetLocalizedString("btnInstallConfirm.Text") + "');");
                        SSLNotInstalledHeading.Text = GetLocalizedString("SSLInstalledNewHeading.Text");
                        SSLNotInstalledDescription.Text = GetLocalizedString("SSLInstalledNewDescription.Text");
                    }
                }

                // Web site has pending certificate
                if (haspending)
                {
                    tabCSR.HeaderText = GetLocalizedString("tabPendingCertificate.HeaderText");//"Pending Certificate";
                    SSLNotInstalled.Visible = false;
                    pnlInstallCertificate.Visible = true;
                    SSLCertificate pending = (from c in certificates
                                              where c.Installed == false
                                              select c).Single();
                    ViewState["CSRID"] = pending.id;
                    txtCSR.Text = pending.CSR;
                    txtCSR.Attributes.Add("onfocus", "this.select();");
                    if (InstalledCert != null)
                    {
                        btnInstallCertificate.Attributes.Add("OnClientClick", "return confirm('" + GetLocalizedString("btnInstallConfirm.Text") + "');");
                    }
                }

                if (!hasactive && ES.Services.WebServers.CheckCertificate(SiteId).IsSuccess)
                {
                    SSLNotInstalled.Visible = false;
                    SSLImport.Visible = true;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("WEB_GET_SSL", ex);
            }
        }

        protected void SetCertHostnameSelection(string hostname)
        {
            if (ddlbSiteCertificate.Items.Contains(new ListItem(hostname)))
            {
                ddlbSiteCertificate.SelectedValue = hostname;
            }
        }

        protected void SetCertCountrySelection(string country)
        {
            MakeSafeListSelection(lstCountries, country);
            BindStates();
        }

        protected void SetCertStateSelection(string state)
        {
            if (ddlStates.Visible)
            {
                MakeSafeListSelection(ddlStates, state);
                return;
            }
            //
            txtState.Text = state;
        }

        protected void MakeSafeListSelection(DropDownList listCtl, string valueSelected)
        {
            if (listCtl == null)
                return;
            //
            var li = listCtl.Items.FindByValue(valueSelected);
            //
            if (li == null)
                return;
            //
            listCtl.ClearSelection();
            li.Selected = true;
        }

        protected void btnCancelRequest_Click(object sender, EventArgs e)
        {
            ResultObject result = null;
            try
            {
                result = ES.Services.WebServers.DeleteCertificateRequest(SiteId, (int)ViewState["CSRID"]);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage(WEB_SSL_DELETE, ex);
            }
            //
            if (!result.IsSuccess)
            {
                messageBox.ShowErrorMessage(WEB_SSL_DELETE);
                return;
            }
            //
            SSLNotInstalled.Visible = true;
            pnlCSR.Visible = false;
            pnlInstallCertificate.Visible = false;
        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            DeleteCertificate(SiteId, new SSLCertificate());
        }
    }
}
