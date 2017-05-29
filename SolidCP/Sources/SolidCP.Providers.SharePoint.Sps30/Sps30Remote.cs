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
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Security.Principal;
using Microsoft.Win32;

using SolidCP.Providers.OS;
using SolidCP.Providers.Utils;
using SolidCP.Server.Utils;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;

namespace SolidCP.Providers.SharePoint
{
	public class Sps30Remote : MarshalByRefObject
	{
        public void ExtendVirtualServer(SharePointSite site, bool exclusiveNTLM)
		{
			try
			{
				WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

				string siteUrl = "http://" + site.Name;

				// check input parameters
				if (String.IsNullOrEmpty(site.RootFolder)
					|| !Directory.Exists(site.RootFolder))
					throw new Exception("Could not create SharePoint site, because web site root folder does not exist. Open web site properties and click \"Update\" button to re-create site folder.");

				SPWebApplication app = SPWebApplication.Lookup(new Uri(siteUrl));
				if (app != null)
					throw new Exception("SharePoint is already installed on this web site.");

				//SPFarm farm = SPFarm.Local;
				SPFarm farm = SPFarm.Local;
				SPWebApplicationBuilder builder = new SPWebApplicationBuilder(farm);
				builder.ApplicationPoolId = site.ApplicationPool;
				builder.DatabaseServer = site.DatabaseServer;
				builder.DatabaseName = site.DatabaseName;
				builder.DatabaseUsername = site.DatabaseUser;
				builder.DatabasePassword = site.DatabasePassword;

				builder.ServerComment = site.Name;
				builder.HostHeader = site.Name;
				builder.Port = 80;

				builder.RootDirectory = new DirectoryInfo(site.RootFolder);
				builder.DefaultZoneUri = new Uri(siteUrl);
                builder.UseNTLMExclusively = exclusiveNTLM;

				app = builder.Create();
				app.Name = site.Name;

				app.Sites.Add(siteUrl, null, null, (uint)site.LocaleID, null, site.OwnerLogin, null, site.OwnerEmail);

				app.Update();
				app.Provision();

				wic.Undo();
			}
			catch (Exception ex)
			{
				try
				{
					// try to delete app if it was created
					SPWebApplication app = SPWebApplication.Lookup(new Uri("http://" + site.Name));
					if (app != null)
						app.Delete();
				}
				catch { /* nop */ }

				throw new Exception("Error creating SharePoint site", ex);
			}
		}

		public void UnextendVirtualServer(string url, bool deleteContent)
		{
			try
			{
				WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

				Uri uri = new Uri("http://" + url);
				SPWebApplication app = SPWebApplication.Lookup(uri);
				if (app == null)
					return;

				SPGlobalAdmin adm = new SPGlobalAdmin();
				adm.UnextendVirtualServer(uri, false);

				//typeof(SPWebApplication).InvokeMember("UnprovisionIisWebSitesAsAdministrator",
				//    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
				//    null, null, new object[] { false, new string[] { url }, app.ApplicationPool });
				//app.Unprovision();
				app.Delete();

				wic.Undo();
			}
			catch (Exception ex)
			{
				throw new Exception("Could not uninstall SharePoint from the web site", ex);
			}
		}

		public string BackupVirtualServer(string url, string fileName, bool zipBackup)
		{
			try
			{
				WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

				string tempPath = Path.GetTempPath();
				string bakFile = Path.Combine(tempPath, (zipBackup
					? StringUtils.CleanIdentifier(url) + ".bsh"
					: StringUtils.CleanIdentifier(fileName)));

				SPWebApplication app = SPWebApplication.Lookup(new Uri("http://" + url));
				if (app == null)
					throw new Exception("SharePoint is not installed on the web site");

				// backup
				app.Sites.Backup("http://" + url, bakFile, true);

				// zip backup file
				if (zipBackup)
				{
					string zipFile = Path.Combine(tempPath, fileName);
					string zipRoot = Path.GetDirectoryName(bakFile);

					// zip files
					FileUtils.ZipFiles(zipFile, zipRoot, new string[] { Path.GetFileName(bakFile) });

					// delete data files
					FileUtils.DeleteFile(bakFile);

					bakFile = zipFile;
				}

				wic.Undo();

				return bakFile;
			}
			catch (Exception ex)
			{
				throw new Exception("Could not backup SharePoint site", ex);
			}
		}

		public void RestoreVirtualServer(string url, string fileName)
		{
			try
			{
				WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

				SPWebApplication app = SPWebApplication.Lookup(new Uri("http://" + url));
				if (app == null)
					throw new Exception("SharePoint is not installed on the web site");

				string tempPath = Path.GetTempPath();

				// unzip uploaded files if required
				string expandedFile = fileName;
				if (Path.GetExtension(fileName).ToLower() == ".zip")
				{
					// unpack file
					expandedFile = FileUtils.UnzipFiles(fileName, tempPath)[0];

					// delete zip archive
					FileUtils.DeleteFile(fileName);
				}

				// delete site
				SPSiteAdministration site = new SPSiteAdministration("http://" + url);
				site.Delete(false);

				// restore from backup
				app.Sites.Restore("http://" + url, expandedFile, true);

				// delete expanded file
				FileUtils.DeleteFile(expandedFile);

				wic.Undo();
			}
			catch (Exception ex)
			{
				throw new Exception("Could not restore SharePoint site", ex);
			}
		}

		public string[] GetInstalledWebParts(string url)
		{
			try
			{
				WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

				SPGlobalAdmin adm = new SPGlobalAdmin();
				string lines = adm.EnumWPPacks(null, "http://" + url, false);

				List<string> list = new List<string>();

				if(!String.IsNullOrEmpty(lines))
				{
					string line = null;
					StringReader reader = new StringReader(lines);
					while ((line = reader.ReadLine()) != null)
					{
						line = line.Trim();
						int commaIdx = line.IndexOf(",");
						if (!String.IsNullOrEmpty(line) && commaIdx != -1)
							list.Add(line.Substring(0, commaIdx));
					}
				}

				wic.Undo();

				return list.ToArray();
			}
			catch (Exception ex)
			{
				throw new Exception("Error reading web parts packages", ex);
			}
		}

		public void InstallWebPartsPackage(string url, string fileName)
		{
			try
			{
				WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

				string tempPath = Path.GetTempPath();

				// unzip uploaded files if required
				string expandedFile = fileName;
				if (Path.GetExtension(fileName).ToLower() == ".zip")
				{
					// unpack file
					expandedFile = FileUtils.UnzipFiles(fileName, tempPath)[0];

					// delete zip archive
					FileUtils.DeleteFile(fileName);
				}

				StringWriter errors = new StringWriter();

				SPGlobalAdmin adm = new SPGlobalAdmin();
				int result = adm.AddWPPack(expandedFile, null, 0, "http://" + url, false, true, errors);
				if (result > 1)
					throw new Exception("Error installing web parts package: " + errors.ToString());

				// delete expanded file
				FileUtils.DeleteFile(expandedFile);

				wic.Undo();

			}
			catch (Exception ex)
			{
				throw new Exception("Could not install web parts package", ex);
			}
		}

		public void DeleteWebPartsPackage(string url, string packageName)
		{
			try
			{
				WindowsImpersonationContext wic = WindowsIdentity.GetCurrent().Impersonate();

				StringWriter errors = new StringWriter();

				SPGlobalAdmin adm = new SPGlobalAdmin();
				int result = adm.RemoveWPPack(packageName, 0, "http://" + url, errors);
				if (result > 1)
					throw new Exception("Error uninstalling web parts package: " + errors.ToString());

				wic.Undo();
			}
			catch (Exception ex)
			{
				throw new Exception("Could not uninstall web parts package", ex);
			}
		}
	}
}
