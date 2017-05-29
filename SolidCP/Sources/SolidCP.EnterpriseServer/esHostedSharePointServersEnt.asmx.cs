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
using System.ComponentModel;
using System.Web.Services;
using SolidCP.EnterpriseServer.Code.SharePoint;
using SolidCP.Providers.SharePoint;
using Microsoft.Web.Services3;

namespace SolidCP.EnterpriseServer
{
	/// <summary>
	/// Summary description for esHostedSharePointServers
	/// </summary>
	[WebService(Namespace = "http://smbsaas/solidcp/enterpriseserver")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[Policy("ServerPolicy")]
	[ToolboxItem(false)]
	public class esHostedSharePointServersEnt : WebService
	{
		/// <summary>
		/// Gets site collections in raw form.
		/// </summary>
		/// <param name="packageId">Package to which desired site collections belong.</param>
		/// <param name="organizationId">Organization to which desired site collections belong.</param>
		/// <param name="filterColumn">Filter column name.</param>
		/// <param name="filterValue">Filter value.</param>
		/// <param name="sortColumn">Sort column name.</param>
		/// <param name="startRow">Row index to start from.</param>
		/// <param name="maximumRows">Maximum number of rows to retrieve.</param>
        /// <param name="groupName">Resource group name.</param>
		/// <returns>Site collections in raw format.</returns>
		[WebMethod]
		public SharePointEnterpriseSiteCollectionListPaged Enterprise_GetSiteCollectionsPaged(int packageId, int organizationId,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
            return HostedSharePointServerEntController.GetSiteCollectionsPaged(packageId, organizationId, filterColumn, filterValue,
				sortColumn, startRow, maximumRows);
		}

		/// <summary>
		/// Gets list of supported languages by this installation of SharePoint.
		/// </summary>
		/// <returns>List of supported languages</returns>
		[WebMethod]
        public int[] Enterprise_GetSupportedLanguages(int packageId)
		{
            return HostedSharePointServerEntController.GetSupportedLanguages(packageId);            
		}

		/// <summary>
		/// Gets list of SharePoint site collections that belong to the package.
		/// </summary>
		/// <param name="packageId">Package that owns site collections.</param>
		/// <param name="recursive">A value which shows whether nested spaces must be searched as well.</param>
        /// <param name="groupName">Resource group name.</param>
		/// <returns>List of found site collections.</returns>
		[WebMethod]
        public List<SharePointEnterpriseSiteCollection> Enterprise_GetSiteCollections(int packageId, bool recursive)
		{
			return HostedSharePointServerEntController.GetSiteCollections(packageId, recursive);
		}

        [WebMethod]
        public int Enterprise_SetStorageSettings(int itemId, int maxStorage, int warningStorage, bool applyToSiteCollections)
        {
            return HostedSharePointServerEntController.SetStorageSettings(itemId, maxStorage, warningStorage, applyToSiteCollections );
        }

		/// <summary>
		/// Gets SharePoint site collection with given id.
		/// </summary>
		/// <param name="itemId">Site collection id within metabase.</param>
		/// <returns>Site collection.</returns>
		[WebMethod]
        public SharePointEnterpriseSiteCollection Enterprise_GetSiteCollection(int itemId)
		{
			return HostedSharePointServerEntController.GetSiteCollection(itemId);
		}

		/// <summary>
		/// Gets SharePoint site collection from package under organization with given domain.
		/// </summary>
		/// <param name="packageId">Package id.</param>
		/// <param name="organizationId">Organization id.</param>
		/// <param name="domain">Domain name.</param>
		/// <returns>SharePoint site collection or null.</returns>
		[WebMethod]
        public SharePointEnterpriseSiteCollection Enterprise_GetSiteCollectionByDomain(int organizationId, string domain)
		{
			DomainInfo domainInfo = ServerController.GetDomain(domain);
            SharePointEnterpriseSiteCollectionListPaged existentSiteCollections = this.Enterprise_GetSiteCollectionsPaged(domainInfo.PackageId, organizationId, "ItemName", String.Format("%{0}", domain), String.Empty, 0, Int32.MaxValue);
			foreach (SharePointEnterpriseSiteCollection existentSiteCollection in existentSiteCollections.SiteCollections)
			{
				Uri existentSiteCollectionUri = new Uri(existentSiteCollection.Name);
				if (existentSiteCollection.Name == String.Format("{0}://{1}", existentSiteCollectionUri.Scheme, domain))
				{
					return existentSiteCollection;
				}
			}

			return null;
		}

		/// <summary>
		/// Adds SharePoint site collection.
		/// </summary>
		/// <param name="item">Site collection description.</param>
		/// <returns>Created site collection id within metabase.</returns>
		[WebMethod]
        public int Enterprise_AddSiteCollection(SharePointEnterpriseSiteCollection item)
		{
			return HostedSharePointServerEntController.AddSiteCollection(item);
		}

		/// <summary>
		/// Deletes SharePoint site collection with given id.
		/// </summary>
		/// <param name="itemId">Site collection id within metabase.</param>
		/// <returns>?</returns>
		[WebMethod]
        public int Enterprise_DeleteSiteCollection(int itemId)
		{
			return HostedSharePointServerEntController.DeleteSiteCollection(itemId);
		}

		/// <summary>
		/// Deletes SharePoint site collections which belong to organization.
		/// </summary>
		/// <param name="organizationId">Site collection id within metabase.</param>
		/// <returns>?</returns>
		[WebMethod]
        public int Enterprise_DeleteSiteCollections(int organizationId)
		{
			HostedSharePointServerEntController.DeleteSiteCollections(organizationId);
			return 0;
		}


		/// <summary>
		/// Backups SharePoint site collection.
		/// </summary>
		/// <param name="itemId">Site collection id within metabase.</param>
		/// <param name="fileName">Backed up site collection file name.</param>
		/// <param name="zipBackup">A value which shows whether back up must be archived.</param>
		/// <param name="download">A value which shows whether created back up must be downloaded.</param>
		/// <param name="folderName">Local folder to store downloaded backup.</param>
		/// <returns>Created backup file name. </returns>
		[WebMethod]
        public string Enterprise_BackupSiteCollection(int itemId, string fileName, bool zipBackup, bool download, string folderName)
		{
			return HostedSharePointServerEntController.BackupSiteCollection(itemId, fileName, zipBackup, download, folderName);
		}

		/// <summary>
		/// Restores SharePoint site collection.
		/// </summary>
		/// <param name="itemId">Site collection id within metabase.</param>
		/// <param name="uploadedFile"></param>
		/// <param name="packageFile"></param>
		/// <returns></returns>
		[WebMethod]
        public int Enterprise_RestoreSiteCollection(int itemId, string uploadedFile, string packageFile)
		{
			return HostedSharePointServerEntController.RestoreSiteCollection(itemId, uploadedFile, packageFile);
		}

		/// <summary>
		/// Gets binary data chunk of specified size from specified offset.
		/// </summary>
		/// <param name="itemId">Item id to obtain realted service id.</param>
		/// <param name="path">Path to file to get bunary data chunk from.</param>
		/// <param name="offset">Offset from which to start data reading.</param>
		/// <param name="length">Binary data chunk length.</param>
		/// <returns>Binary data chunk read from file.</returns>
		[WebMethod]
        public byte[] Enterprise_GetBackupBinaryChunk(int itemId, string path, int offset, int length)
		{
			return HostedSharePointServerEntController.GetBackupBinaryChunk(itemId, path, offset, length);
		}

		/// <summary>
		/// Appends supplied binary data chunk to file.
		/// </summary>
		/// <param name="itemId">Item id to obtain realted service id.</param>
		/// <param name="fileName">Non existent file name to append to.</param>
		/// <param name="path">Full path to existent file to append to.</param>
		/// <param name="chunk">Binary data chunk to append to.</param>
		/// <returns>Path to file that was appended with chunk.</returns>
		[WebMethod]
        public string Enterprise_AppendBackupBinaryChunk(int itemId, string fileName, string path, byte[] chunk)
		{
			return HostedSharePointServerEntController.AppendBackupBinaryChunk(itemId, fileName, path, chunk);
		}

        [WebMethod]
        public SharePointSiteDiskSpace[] Enterprise_CalculateSharePointSitesDiskSpace(int itemId, out int errorCode)
        {
            return HostedSharePointServerEntController.CalculateSharePointSitesDiskSpace(itemId, out  errorCode);
        }


        [WebMethod]
        public void Enterprise_UpdateQuota(int itemId, int siteCollectionId, int maxSize, int warningSize)
        {
            HostedSharePointServerEntController.UpdateQuota(itemId, siteCollectionId, maxSize, warningSize);
        }
	}
}
