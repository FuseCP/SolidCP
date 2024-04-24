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
using System.DirectoryServices;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

using SolidCP.Server.Client;

namespace SolidCP.EnterpriseServer
{
	public class ServiceProviderProxy: ControllerBase
	{
		public ServiceProviderProxy(ControllerBase provider) : base(provider) { }

		public SolidCP.Web.Clients.ClientBase Init(SolidCP.Web.Clients.ClientBase proxy, int serviceId, StringDictionary additionalSettings = null)
		{
			ServerProxyConfigurator cnfg = new ServerProxyConfigurator();

			// get service
			ServiceInfo service = ServerController.GetServiceInfo(serviceId);

			if (service == null)
				throw new Exception($"Service with ID {serviceId} was not found");

			// set service settings
			StringDictionary serviceSettings = ServerController.GetServiceSettings(serviceId);
			foreach (string key in serviceSettings.Keys)
				cnfg.ProviderSettings.Settings[key] = serviceSettings[key];
			// RB ADDED EMAIL SECURITY SE
			if (additionalSettings != null)
			{
				foreach (string str in additionalSettings.Keys)
				{
					cnfg.ProviderSettings.Settings[str] = additionalSettings[str];
				}
			}
			// get provider
			ProviderInfo provider = ServerController.GetProvider(service.ProviderId);
			cnfg.ProviderSettings.ProviderGroupID = provider.GroupId;
			cnfg.ProviderSettings.ProviderCode = provider.ProviderName;
			cnfg.ProviderSettings.ProviderName = provider.DisplayName;
			cnfg.ProviderSettings.ProviderType = provider.ProviderType;

			// init service on the server level
			return ServerInit(proxy, cnfg, service.ServerId);
		}

		public SolidCP.Web.Clients.ClientBase ServerInit(SolidCP.Web.Clients.ClientBase proxy, ServerProxyConfigurator cnfg, int serverId)
		{
			// get server info
			ServerInfo server = ServerController.GetServerByIdInternal(serverId);

			if (server == null)
				throw new Exception(String.Format("Server with ID {0} was not found", serverId));

			// set AD integration settings
			cnfg.ServerSettings.ADEnabled = server.ADEnabled;
			cnfg.ServerSettings.ADAuthenticationType = AuthenticationTypes.Secure;

			AuthenticationTypes adAuthenticationType;
			if (Enum.TryParse<AuthenticationTypes>(server.ADAuthenticationType, true, out adAuthenticationType))
				cnfg.ServerSettings.ADAuthenticationType = adAuthenticationType;

			cnfg.ServerSettings.ADRootDomain = server.ADRootDomain;
			cnfg.ServerSettings.ADUsername = server.ADUsername;
			cnfg.ServerSettings.ADPassword = server.ADPassword;
			cnfg.ServerSettings.ADParentDomain = server.ADParentDomain;
			cnfg.ServerSettings.ADParentDomainController = server.ADParentDomainController;

			// set timeout
			cnfg.Timeout = ConfigSettings.ServerRequestTimeout;
			
			// set if server is running on net core
			cnfg.IsCore = server.IsCore;
			cnfg.PasswordIsSHA256 = server.PasswordIsSHA256;

			return ServerInit(proxy, cnfg, server.ServerUrl, server.Password);
		}

		private static SolidCP.Web.Clients.ClientBase ServerInit(SolidCP.Web.Clients.ClientBase proxy,
			 ServerProxyConfigurator cnfg, string serverUrl, string serverPassword)
		{			
			// set URL & password
			cnfg.ServerUrl = CryptoUtils.DecryptServerUrl(serverUrl);
			if (proxy.IsAuthenticated)
			{
				cnfg.ServerPassword = cnfg.PasswordIsSHA256 ? CryptoUtils.SHA256(serverPassword) : CryptoUtils.SHA1(serverPassword);
			}

			// configure proxy!
			cnfg.Configure(proxy);

			return proxy;
		}

		public SolidCP.Web.Clients.ClientBase ServerInit(SolidCP.Web.Clients.ClientBase proxy,
			 string serverUrl, string serverPassword, bool sha256Password)
		{
			return ServerInit(proxy, new ServerProxyConfigurator() { PasswordIsSHA256 = sha256Password }, serverUrl, serverPassword);
		}

		public SolidCP.Web.Clients.ClientBase ServerInit(SolidCP.Web.Clients.ClientBase proxy, int serverId)
		{
			return ServerInit(proxy, new ServerProxyConfigurator(), serverId);
		}
	}
}
