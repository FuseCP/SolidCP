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
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Services3;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.HostedSolution;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Security.Cryptography;

namespace SolidCP.WebDav.Core.Scp.Framework
{
    // SCP.Services

    public class SCP
    {
        private readonly ICryptography _cryptography;

        protected SCP()
        {
            _cryptography = DependencyResolver.Current.GetService<ICryptography>();
        }

        public static SCP Services
        {
            get
            {
                SCP services = (SCP)HttpContext.Current.Items["WebServices"];

                if (services == null)
                {
                    services = new SCP();
                    HttpContext.Current.Items["WebServices"] = services;
                }

                return services;
            }
        }

        public esCRM CRM
        {
            get
            {
                return GetCachedProxy<esCRM>();
            }
        }
        

        public esVirtualizationServer VPS
        {
            get { return GetCachedProxy<esVirtualizationServer>(); }
        }

        public esVirtualizationServerForPrivateCloud VPSPC
        {
            get { return GetCachedProxy<esVirtualizationServerForPrivateCloud>(); }
        }

        public esBlackBerry BlackBerry
        {
            get { return GetCachedProxy<esBlackBerry>(); }
        }
        
        public esOCS OCS
        {
            get { return GetCachedProxy<esOCS>(); }
        }


        public esLync Lync
        {
            get { return GetCachedProxy<esLync>(); }
        }

        public esSfB SfB
        {
            get { return GetCachedProxy<esSfB>(); }
        }


        public esOrganizations Organizations
        {
            get
            {
                return GetCachedProxy<esOrganizations>();
            }
        }

		public esSystem System
		{
			get { return GetCachedProxy<esSystem>(); }
		}

        public esApplicationsInstaller ApplicationsInstaller
        {
            get { return GetCachedProxy<esApplicationsInstaller>(); }
        }

        public esWebApplicationGallery WebApplicationGallery
        {
            get { return GetCachedProxy<esWebApplicationGallery>(); }
        }

        public esAuditLog AuditLog
        {
            get { return GetCachedProxy<esAuditLog>(); }
        }

        public esAuthentication Authentication
        {
            get { return GetCachedProxy<esAuthentication>(false); }
        }

        public esComments Comments
        {
            get { return GetCachedProxy<esComments>(); }
        }

        public esDatabaseServers DatabaseServers
        {
            get { return GetCachedProxy<esDatabaseServers>(); }
        }

        public esFiles Files
        {
            get { return GetCachedProxy<esFiles>(); }
        }

        public esFtpServers FtpServers
        {
            get { return GetCachedProxy<esFtpServers>(); }
        }

        public esMailServers MailServers
        {
            get { return GetCachedProxy<esMailServers>(); }
        }

        public esOperatingSystems OperatingSystems
        {
            get { return GetCachedProxy<esOperatingSystems>(); }
        }

        public esPackages Packages
        {
            get { return GetCachedProxy<esPackages>(); }
        }

        public esScheduler Scheduler
        {
            get { return GetCachedProxy<esScheduler>(); }
        }

        public esTasks Tasks
        {
            get { return GetCachedProxy<esTasks>(); }
        }

        public esServers Servers
        {
            get { return GetCachedProxy<esServers>(); }
        }

        public esStatisticsServers StatisticsServers
        {
            get { return GetCachedProxy<esStatisticsServers>(); }
        }

        public esUsers Users
        {
            get { return GetCachedProxy<esUsers>(); }
        }

        public esWebServers WebServers
        {
            get { return GetCachedProxy<esWebServers>(); }
        }

        public esSharePointServers SharePointServers
        {
            get { return GetCachedProxy<esSharePointServers>(); }
        }

		public esHostedSharePointServers HostedSharePointServers
		{
			get { return GetCachedProxy<esHostedSharePointServers>(); }
		}

        public esImport Import
        {
            get { return GetCachedProxy<esImport>(); }
        }

        public esBackup Backup
        {
            get { return GetCachedProxy<esBackup>(); }
        }

		public esExchangeServer ExchangeServer
		{
			get { return GetCachedProxy<esExchangeServer>(); }
		}


        public esHeliconZoo HeliconZoo
        {
            get { return GetCachedProxy<esHeliconZoo>(); }
        }


        public esEnterpriseStorage EnterpriseStorage
        {
            get { return GetCachedProxy<esEnterpriseStorage>(); }
        }

        public esRemoteDesktopServices RDS
        {
            get { return GetCachedProxy<esRemoteDesktopServices>(); }
        }
        
        protected virtual T GetCachedProxy<T>()
        {
            return GetCachedProxy<T>(true);
        }

        protected virtual T GetCachedProxy<T>(bool secureCalls)
        {
            Type t = typeof(T);
            string key = t.FullName + ".ServiceProxy";
            T proxy = (T)HttpContext.Current.Items[key];
            if (proxy == null)
            {
                proxy = (T)Activator.CreateInstance(t);
                HttpContext.Current.Items[key] = proxy;
            }

            object p = proxy;

            // configure proxy
			ConfigureEnterpriseServerProxy((WebServicesClientProtocol)p, secureCalls);

            return proxy;
        }

        public void ConfigureEnterpriseServerProxy(WebServicesClientProtocol proxy, bool applyPolicy)
        {
            // load ES properties
            string serverUrl = WebDavAppConfigManager.Instance.EnterpriseServerUrl;

            EnterpriseServerProxyConfigurator cnfg = new EnterpriseServerProxyConfigurator();
            cnfg.EnterpriseServerUrl = serverUrl;

            // create assertion
            if (applyPolicy)
            {

                cnfg.Username = WebDavAppConfigManager.Instance.SolidCPConstantUserParameters.Login;
                cnfg.Password = _cryptography.Decrypt(WebDavAppConfigManager.Instance.SolidCPConstantUserParameters.Password);
            }

            cnfg.Configure(proxy);
        }
    }
}
