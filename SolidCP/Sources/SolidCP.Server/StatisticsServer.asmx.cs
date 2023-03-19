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
using SolidCP.Providers.Statistics;
using SolidCP.Server.Utils;

namespace SolidCP.Server
{
    /// <summary>
    /// Summary description for StatisticsServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class StatisticsServer : HostingServiceProviderWebService, IStatisticsServer
    {
        private IStatisticsServer StatsProvider
        {
            get { return (IStatisticsServer)Provider; }
        }

        #region Sites

        [WebMethod, SoapHeader("settings")]
        public StatsServer[] GetServers()
        {
            try
            {
                Log.WriteStart("'{0}' GetServers", ProviderSettings.ProviderName);
                StatsServer[] result = StatsProvider.GetServers();
                Log.WriteEnd("'{0}' GetServers", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetServers", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string GetSiteId(string siteName)
        {
            try
            {
                Log.WriteStart("'{0}' GetSiteId", ProviderSettings.ProviderName);
                string result = StatsProvider.GetSiteId(siteName);
                Log.WriteEnd("'{0}' GetSiteId", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSiteId", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetSites()
        {
            try
            {
                Log.WriteStart("'{0}' GetSites", ProviderSettings.ProviderName);
                string[] result = StatsProvider.GetSites();
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
        public StatsSite GetSite(string siteId)
        {
            try
            {
                Log.WriteStart("'{0}' GetSite", ProviderSettings.ProviderName);
                StatsSite result = StatsProvider.GetSite(siteId);
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
        public string AddSite(StatsSite site)
        {
            try
            {
                Log.WriteStart("'{0}' AddSite", ProviderSettings.ProviderName);
                string result = StatsProvider.AddSite(site);
                Log.WriteEnd("'{0}' AddSite", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateSite(StatsSite site)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateSite", ProviderSettings.ProviderName);
                StatsProvider.UpdateSite(site);
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
                StatsProvider.DeleteSite(siteId);
                Log.WriteEnd("'{0}' DeleteSite", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteSite", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion
    }
}
