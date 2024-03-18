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
using System.ComponentModel;
using SolidCP.Web.Services;
using SolidCP.Providers;
using SolidCP.Providers.HostedSolution;
using SolidCP.Server.Utils;

namespace SolidCP.Server
{
    /// <summary>
    /// OCS Web Service
    /// </summary>
    [WebService(Namespace = "http://smbsaas/solidcp/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class LyncServer : HostingServiceProviderWebService
    {
        private ILyncServer Lync
        {
            get { return (ILyncServer)Provider; }
        }


        #region Organization
        [WebMethod, SoapHeader("settings")]
        public string CreateOrganization(string organizationId, string sipDomain, bool enableConferencing, bool enableConferencingVideo, int maxConferenceSize, bool enabledFederation, bool enabledEnterpriseVoice)
        {
            try
            {
                Log.WriteStart("{0}.CreateOrganization", ProviderSettings.ProviderName);
                string ret = Lync.CreateOrganization(organizationId, sipDomain, enableConferencing, enableConferencingVideo, maxConferenceSize, enabledFederation, enabledEnterpriseVoice);
                Log.WriteEnd("{0}.CreateOrganization", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.CreateOrganization", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string GetOrganizationTenantId(string organizationId)
        {
            try
            {
                Log.WriteStart("{0}.GetOrganizationTenantId", ProviderSettings.ProviderName);
                string ret = Lync.GetOrganizationTenantId(organizationId);
                Log.WriteEnd("{0}.GetOrganizationTenantId", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.GetOrganizationTenantId", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool DeleteOrganization(string organizationId, string sipDomain)
        {
            try
            {
                Log.WriteStart("{0}.DeleteOrganization", ProviderSettings.ProviderName);
                bool ret = Lync.DeleteOrganization(organizationId, sipDomain);
                Log.WriteEnd("{0}.DeleteOrganization", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.DeleteOrganization", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Users

        [WebMethod, SoapHeader("settings")]
        public bool CreateUser(string organizationId, string userUpn, LyncUserPlan plan)
        {
            try
            {
                Log.WriteStart("{0}.CreateUser", ProviderSettings.ProviderName);
                bool ret = Lync.CreateUser(organizationId, userUpn, plan);
                Log.WriteEnd("{0}.CreateUser", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.CreateUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public LyncUser GetLyncUserGeneralSettings(string organizationId, string userUpn)
        {
            try
            {
                Log.WriteStart("{0}.GetLyncUserGeneralSettings", ProviderSettings.ProviderName);
                LyncUser ret = Lync.GetLyncUserGeneralSettings(organizationId, userUpn);
                Log.WriteEnd("{0}.GetLyncUserGeneralSettings", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.GetLyncUserGeneralSettings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SetLyncUserGeneralSettings(string organizationId, string userUpn, LyncUser lyncUser)
        {
            try
            {
                Log.WriteStart("{0}.SetLyncUserGeneralSettings", ProviderSettings.ProviderName);
                bool ret = Lync.SetLyncUserGeneralSettings(organizationId, userUpn, lyncUser);
                Log.WriteEnd("{0}.SetLyncUserGeneralSettings", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.SetLyncUserGeneralSettings", ProviderSettings.ProviderName), ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings")]
        public bool SetLyncUserPlan(string organizationId, string userUpn, LyncUserPlan plan)
        {
            try
            {
                Log.WriteStart("{0}.SetLyncUserPlan", ProviderSettings.ProviderName);
                bool ret = Lync.SetLyncUserPlan(organizationId, userUpn, plan);
                Log.WriteEnd("{0}.SetLyncUserPlan", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.SetLyncUserPlan", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool DeleteUser(string userUpn)
        {
            try
            {
                Log.WriteStart("{0}.DeleteUser", ProviderSettings.ProviderName);
                bool ret = Lync.DeleteUser(userUpn);
                Log.WriteEnd("{0}.DeleteUser", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.DeleteUser", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion

        #region Federation
        [WebMethod, SoapHeader("settings")]
        public LyncFederationDomain[] GetFederationDomains(string organizationId)
        {
            try
            {
                Log.WriteStart("{0}.GetFederationDomains", ProviderSettings.ProviderName);
                LyncFederationDomain[] ret = Lync.GetFederationDomains(organizationId);
                Log.WriteEnd("{0}.GetFederationDomains", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.GetFederationDomains", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool AddFederationDomain(string organizationId, string domainName, string proxyFqdn)
        {
            try
            {
                Log.WriteStart("{0}.AddFederationDomain", ProviderSettings.ProviderName);
                bool ret = Lync.AddFederationDomain(organizationId, domainName, proxyFqdn);
                Log.WriteEnd("{0}.AddFederationDomain", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.AddFederationDomain", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool RemoveFederationDomain(string organizationId, string domainName)
        {
            try
            {
                Log.WriteStart("{0}.RemoveFederationDomain", ProviderSettings.ProviderName);
                bool ret = Lync.RemoveFederationDomain(organizationId, domainName);
                Log.WriteEnd("{0}.RemoveFederationDomain", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.RemoveFederationDomain", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        [WebMethod, SoapHeader("settings")]
        public void ReloadConfiguration()
        {
            try
            {
                Log.WriteStart("{0}.ReloadConfiguration", ProviderSettings.ProviderName);
                Lync.ReloadConfiguration();
                Log.WriteEnd("{0}.ReloadConfiguration", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.ReloadConfiguration", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetPolicyList(LyncPolicyType type, string name)
        {
            string[] ret = null;

            try
            {
                Log.WriteStart("{0}.GetPolicyList", ProviderSettings.ProviderName);
                ret = Lync.GetPolicyList(type, name);
                Log.WriteEnd("{0}.GetPolicyList", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Error: {0}.GetPolicyList", ProviderSettings.ProviderName), ex);
                throw;
            }

            return ret;
        }


    }
}
