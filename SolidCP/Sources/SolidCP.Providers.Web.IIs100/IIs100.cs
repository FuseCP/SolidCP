// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
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
// - Neither  the  name  of SolidCP  nor   the   names  of  its
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

using Microsoft.Web.Administration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using SolidCP.Providers.Common;
using SolidCP.Providers.Web.Iis;
using SolidCP.Providers.ResultObjects;

namespace SolidCP.Providers.Web
{
	public class IIs100 : IIs70
	{
		private SslFlags SSLFlags
		{
			get
			{
				return (UseSni ? SslFlags.Sni : SslFlags.None) | (UseCcs ? SslFlags.CentralCertStore : SslFlags.None);
			}
		}

		public string CCSUncPath
		{
			get { return ProviderSettings["SSLCCSUNCPath"]; }
		}

		public string CCSCommonPassword
		{
			get { return ProviderSettings["SSLCCSCommonPassword"]; }
		}

		public bool UseSni
		{
			get
			{
				try
				{
					return Convert.ToBoolean(ProviderSettings["SSLUseSNI"]);
				}
				catch
				{
					return false;
				}
			}
		}

		public bool UseCcs
		{
			get
			{
				try
				{
					return Convert.ToBoolean(ProviderSettings["SSLUseCCS"]);
				}
				catch
				{
					return false;
				}
			}
		}

		public override SettingPair[] GetProviderDefaultSettings()
		{
			var allSettings = new List<SettingPair>();
			allSettings.AddRange(base.GetProviderDefaultSettings());

			// Add these to get some default values in. These are also used a marker in the IIS70_Settings.ascx.cs to know that it is the IIS80 provider that is used
			allSettings.Add(new SettingPair("SSLUseCCS", false.ToString()));
			allSettings.Add(new SettingPair("SSLUseSNI", false.ToString()));
			allSettings.Add(new SettingPair("SSLCCSUNCPath", ""));
			allSettings.Add(new SettingPair("SSLCCSCommonPassword", ""));

			return allSettings.ToArray();
		}

		public override string[] Install()
		{
			var messages = new List<string>();

			messages.AddRange(base.Install());

			// TODO: Setup ccs

			return messages.ToArray();
		}

		public override bool IsIISInstalled()
		{
			int value = 0;
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey("SOFTWARE\\Microsoft\\InetStp");
			if (rk != null)
			{
				value = (int)rk.GetValue("MajorVersion", null);
				rk.Close();
			}

			return value >= 10;
		}

		public override bool IsInstalled()
		{
			return OS.OSInfo.IsWindows && IsIISInstalled();
		}

		public override bool CheckCertificate(WebSite webSite)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.CheckCertificate(webSite);
		}

		public override ResultObject DeleteCertificate(SSLCertificate certificate, WebSite website)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.DeleteCertificate(certificate, website);
		}

		public override SSLCertificate InstallPFX(byte[] certificate, string password, WebSite website)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.InstallPfx(certificate, password, website);
		}

		public override SSLCertificate ImportCertificate(WebSite website)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.ImportCertificate(website);
		}

		public override byte[] ExportCertificate(string serialNumber, string password)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.ExportPfx(serialNumber, password);
		}

		public override SSLCertificate GenerateCSR(SSLCertificate certificate)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			sslObjectService.GenerateCsr(certificate);

			return certificate;
		}

		public override List<SSLCertificate> GetServerCertificates()
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.GetServerCertificates();
		}

		public override SSLCertificate InstallCertificate(SSLCertificate certificate, WebSite website)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.InstallCertificate(certificate, website);
		}

		public override String LEInstallCertificate(WebSite website, string email)
		{
			var sslObjectService = new SSLModuleService100(SSLFlags, CCSUncPath, CCSCommonPassword);

			return sslObjectService.LEInstallCertificate(website, email);
		}

		public override WebSite GetSite(string siteId)
		{
			var site = base.GetSite(siteId);
			site.SniEnabled = UseSni;
			return site;
		}

		public override Version DefaultVersion => new Version(10, 0);
	}
}
