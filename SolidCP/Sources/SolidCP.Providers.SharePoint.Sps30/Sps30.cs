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
using System.IO;
using Microsoft.Win32;

namespace SolidCP.Providers.SharePoint
{
	public class Sps30 : Sps20
	{
        private static readonly string Wss3RegistryKey = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\12.0";
        
        #region Constants
		private const string SHAREPOINT_REGLOC = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\12.0";
		#endregion

		#region Sites
		public override void ExtendVirtualServer(SharePointSite site)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				wss.ExtendVirtualServer(site, ExclusiveNTLM);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}

		public override void UnextendVirtualServer(string url, bool deleteContent)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				wss.UnextendVirtualServer(url, deleteContent);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}
		#endregion

		#region Backup/Restore
		public override string BackupVirtualServer(string url, string fileName, bool zipBackup)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				return wss.BackupVirtualServer(url, fileName, zipBackup);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}

		public override void RestoreVirtualServer(string url, string fileName)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				wss.RestoreVirtualServer(url, fileName);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}
		#endregion

		#region Web Parts
		public override string[] GetInstalledWebParts(string url)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				return wss.GetInstalledWebParts(url);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}

		public override void InstallWebPartsPackage(string url, string fileName)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				wss.InstallWebPartsPackage(url, fileName);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}

		public override void DeleteWebPartsPackage(string url, string packageName)
		{
			AppDomain domain = null;
			try
			{
				domain = CreateAppDomain();
				Sps30Remote wss = CreateSps30Remote(domain);

				// call method
				wss.DeleteWebPartsPackage(url, packageName);
			}
			finally
			{
				if (domain != null)
					AppDomain.Unload(domain);
			}
		}
		#endregion

		#region Private Helpers
		protected override string GetAdminToolPath()
		{
			RegistryKey spKey = Registry.LocalMachine.OpenSubKey(SHAREPOINT_REGLOC);
			if (spKey == null)
				throw new Exception("SharePoint Services is not installed on the system");

			return ((string)spKey.GetValue("Location")) + @"\bin\stsadm.exe";
		}

		protected override bool IsSharePointInstalled()
		{
			RegistryKey spKey = Registry.LocalMachine.OpenSubKey(SHAREPOINT_REGLOC);
			if (spKey == null)
				return false;

			string spVal = (string)spKey.GetValue("SharePoint");
			return (String.Compare(spVal, "installed", true) == 0);
		}

		private AppDomain CreateAppDomain()
		{
			string binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");

			AppDomainSetup info = new AppDomainSetup();
			info.ApplicationBase = binPath;

			return AppDomain.CreateDomain("WSS30", null, info);
		}

		private Sps30Remote CreateSps30Remote(AppDomain domain)
		{
			return (Sps30Remote)domain.CreateInstanceAndUnwrap(
				typeof(Sps30Remote).Assembly.FullName,
				typeof(Sps30Remote).FullName);
		}
		#endregion

        public override bool IsInstalled()
        {
            RegistryKey spKey = Registry.LocalMachine.OpenSubKey(Wss3RegistryKey);
            if (spKey == null)
            {
                return false;
            }

            string spVal = (string)spKey.GetValue("SharePoint");
            return (String.Compare(spVal, "installed", true) == 0);
        }
	}
}
