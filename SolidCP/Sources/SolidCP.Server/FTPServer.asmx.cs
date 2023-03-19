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
using System.Collections;
using SolidCP.Web.Services;
using System.ComponentModel;
using SolidCP.Providers;
using SolidCP.Providers.FTP;
using SolidCP.Server.Utils;

namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for FTPServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class FTPServer : HostingServiceProviderWebService, IFtpServer
    {
        private IFtpServer FtpProvider
        {
            get { return (IFtpServer)Provider; }
        }

        #region Sites

        [WebMethod, SoapHeader("settings")]
        public void ChangeSiteState(string siteId, ServerState state)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeSiteState", ProviderSettings.ProviderName);
                FtpProvider.ChangeSiteState(siteId, state);
                Log.WriteEnd("'{0}' ChangeSiteState", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeSiteState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ServerState GetSiteState(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetSiteState", ProviderSettings.ProviderName);
                ServerState result = FtpProvider.GetSiteState(siteId);
                Log.WriteEnd("'{0}' GetSiteState", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSiteState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SiteExists(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' SiteIdExists", ProviderSettings.ProviderName);
                bool result = FtpProvider.SiteExists(siteId);
                Log.WriteEnd("'{0}' SiteIdExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SiteIdExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public FtpSite[] GetSites()
        {
            try
            {
                Log.WriteStart("'{0}' GetSites", ProviderSettings.ProviderName);
                FtpSite[] result = FtpProvider.GetSites();
                Log.WriteEnd("'{0}' GetSites", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSites", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public FtpSite GetSite(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetSite", ProviderSettings.ProviderName);
                FtpSite result = FtpProvider.GetSite(siteId);
                Log.WriteEnd("'{0}' GetSite", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string CreateSite(FtpSite site)
        {
            try
            {
                Log.WriteStart("'{0}' CreateSite", ProviderSettings.ProviderName);
                string result = FtpProvider.CreateSite(site);
                Log.WriteEnd("'{0}' CreateSite", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateSite(FtpSite site)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateSite", ProviderSettings.ProviderName);
                FtpProvider.UpdateSite(site);
                Log.WriteEnd("'{0}' UpdateSite", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteSite(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteSite", ProviderSettings.ProviderName);
                FtpProvider.DeleteSite(siteId);
                Log.WriteEnd("'{0}' DeleteSite", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion

        #region Accounts
        [WebMethod, SoapHeader("settings")]
        public bool AccountExists(string accountName)
        {
            try
            {
                Log.WriteStart("'{0}' AccountExists", ProviderSettings.ProviderName);
                bool result = FtpProvider.AccountExists(accountName);
                Log.WriteEnd("'{0}' AccountExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AccountExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public FtpAccount[] GetAccounts()
        {
            try
            {
                Log.WriteStart("'{0}' GetAccounts", ProviderSettings.ProviderName);
                FtpAccount[] result = FtpProvider.GetAccounts();
                Log.WriteEnd("'{0}' GetAccounts", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetAccounts", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public FtpAccount GetAccount(string accountName)
        {
            try
            {
                Log.WriteStart("'{0}' GetAccount", ProviderSettings.ProviderName);
                FtpAccount result = FtpProvider.GetAccount(accountName);
                Log.WriteEnd("'{0}' GetAccount", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetAccount", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateAccount(FtpAccount account)
        {
            try
            {
                Log.WriteStart("'{0}' CreateAccount", ProviderSettings.ProviderName);
                FtpProvider.CreateAccount(account);
                Log.WriteEnd("'{0}' CreateAccount", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateAccount", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateAccount(FtpAccount account)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateAccount", ProviderSettings.ProviderName);
                FtpProvider.UpdateAccount(account);
                Log.WriteEnd("'{0}' UpdateAccount", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateAccount", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteAccount(string accountName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteAccount", ProviderSettings.ProviderName);
                FtpProvider.DeleteAccount(accountName);
                Log.WriteEnd("'{0}' DeleteAccount", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteAccount", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion
    }
}
