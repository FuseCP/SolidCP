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
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using SolidCP.EnterpriseServer.Data;
using SolidCP.Providers.OS;

namespace SolidCP.UniversalInstaller
{
	public interface IInstallerWebService
	{
		public ComponentUpdateInfo GetComponentUpdate(string componentCode, string release);
		public Task<ComponentUpdateInfo> GetComponentUpdateAsync(string componentCode, string release);
		public List<ComponentInfo> GetAvailableComponents();
		public Task<List<ComponentInfo>> GetAvailableComponentsAsync();
		public ComponentUpdateInfo GetLatestComponentUpdate(string componentCode);
		public Task<ComponentUpdateInfo> GetLatestComponentUpdateAsync(string componentCode);
		public ReleaseFileInfo GetReleaseFileInfo(string componentCode, string version);
		public Task<ReleaseFileInfo> GetReleaseFileInfoAsync(string componentCode, string version);
		public byte[] GetFileChunk(string file, int offset, int size);
		public Task<byte[]> GetFileChunkAsync(string file, int offset, int size);
		public long GetFileSize(string file);
		public Task<long> GetFileSizeAsync(string file);
	}

	public partial class Installer
	{
		public virtual IInstallerWebService InstallerWebService
		{
			get
			{
				string url = Settings.Installer.WebServiceUrl;
				if (string.IsNullOrEmpty(url)) url = "http://installer.solidcp.com/Services/InstallerService-1.0.asmx";

				var type = GetType($"SolidCP.UniversalInstaller.InstallerWebService, SolidCP.UniversalInstaller.Runtime.{
					(OSInfo.IsCore ? "NetCore" : "NetFX")}");

				var webService = Activator.CreateInstance(type, url) as IInstallerWebService;
				
				// check if we need to add a proxy to access Internet
				if (Settings.Installer.Proxy != null)
				{
					string proxyServer = Settings.Installer.Proxy.Address;
					if (!String.IsNullOrEmpty(proxyServer))
					{
						IWebProxy proxy = new WebProxy(proxyServer);
						var proxyUsername = Settings.Installer.Proxy.Username;
						var proxyPassword = Settings.Installer.Proxy.Password;
						if (!String.IsNullOrEmpty(proxyUsername))
							proxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
						WebRequest.DefaultWebProxy = proxy;
					} else
					{
						WebRequest.DefaultWebProxy = null;
					}
				}

				return webService;
			}
		}

		public const string GitHubUrl = "https://github.com/FuseCP/SolidCP";
 		public GitHubReleases GitHub => new GitHubReleases(Settings.Installer.GitHubUrl ?? GitHubUrl);
		public Releases Releases => new Releases();
	}
}
