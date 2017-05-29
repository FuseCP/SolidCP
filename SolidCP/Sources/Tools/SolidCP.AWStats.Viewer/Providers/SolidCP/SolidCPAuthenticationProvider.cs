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
using System.Configuration;

using SolidCP.EnterpriseServer;
using SolidCP.Providers.Statistics;

namespace SolidCP.AWStats.Viewer
{
	/// <summary>
	/// Summary description for SolidCPAuthenticationProvider.
	/// </summary>
	public class SolidCPAuthenticationProvider : AuthenticationProvider
	{
		public override AuthenticationResult AuthenticateUser(string domain, string username, string password)
		{
            try
            {
                // authentication
                esAuthentication auth = new esAuthentication();
                SetupProxy(auth);

                int result = auth.AuthenticateUser(username, password, "");

                if (result == -109)
                    return AuthenticationResult.WrongUsername;
                else if (result == -110)
                    return AuthenticationResult.WrongPassword;

                // load user account
                UserInfo user = auth.GetUserByUsernamePassword(username, password, "");
                if (user == null)
                    return AuthenticationResult.WrongUsername;

                // get all packages
                esPackages packagesProxy = new esPackages();
                SetupProxy(packagesProxy, username, password);
                esStatisticsServers statsServers = new esStatisticsServers();
                SetupProxy(statsServers, username, password);
                PackageInfo[] packages = packagesProxy.GetMyPackages(user.UserId);

                // load all statistics sites from all packages
                foreach (PackageInfo package in packages)
                {
                    StatsSite[] sites = statsServers.GetStatisticsSites(package.PackageId, false);

                    foreach (StatsSite site in sites)
                    {
                        if (String.Compare(site.Name, domain, true) == 0)
                            return AuthenticationResult.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.ToString());
            }

            return AuthenticationResult.DomainNotFound;
		}

        private void SetupProxy(Microsoft.Web.Services3.WebServicesClientProtocol proxy)
        {
            SetupProxy(proxy, null, null);
        }

        private void SetupProxy(Microsoft.Web.Services3.WebServicesClientProtocol proxy,
            string username, string password)
        {
            // create ES configurator
            string serverUrl = ConfigurationManager.AppSettings["AWStats.SolidCPAuthenticationProvider.EnterpriseServer"];
            if (String.IsNullOrEmpty(serverUrl))
                throw new Exception("Enterprise Server URL could not be empty");

            EnterpriseServerProxyConfigurator cnfg = new EnterpriseServerProxyConfigurator();
            cnfg.EnterpriseServerUrl = serverUrl;
            cnfg.Username = username;
            cnfg.Password = password;
            cnfg.Configure(proxy);
        }
	}
}
