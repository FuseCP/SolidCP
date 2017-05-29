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
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SolidCP.Providers.SharePoint;

namespace SolidCP.Portal
{
	public partial class HostedSharePointEnterpriseRestoreSiteCollection : SolidCPModuleBase
	{
		private int OrganizationId
		{
			get
			{
				return PanelRequest.GetInt("ItemID");
			}
		}

		private int SiteCollectionId
		{
			get
			{
				return PanelRequest.GetInt("SiteCollectionID");
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				BindSite();
			}
		}

		private void BindSite()
		{
			try
			{
				SharePointEnterpriseSiteCollection siteCollection = ES.Services.HostedSharePointServersEnt.Enterprise_GetSiteCollection(this.SiteCollectionId);
				litSiteCollectionName.Text = siteCollection.PhysicalAddress;
				fileLookup.SelectedFile = String.Empty;
				fileLookup.PackageId = siteCollection.PackageId;

				ToggleControls();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SHAREPOINT_GET_SITE", ex);
				return;
			}
		}

		private void ToggleControls()
		{
			cellFile.Visible = radioFile.Checked;
			cellUploadFile.Visible = radioUpload.Checked;
		}

		private void RestoreSiteCollection()
		{
			try
			{
				string uploadedFile = null;
				string packageFile = null;

				if (radioUpload.Checked)
				{
					if (uploadFile.PostedFile.FileName != "")
					{
						Stream stream = uploadFile.PostedFile.InputStream;

						// save uploaded file
						int FILE_BUFFER_LENGTH = 5000000;
						string path = null;
						int readBytes = 0;
						string fileName = Path.GetFileName(uploadFile.PostedFile.FileName);

						int offset = 0;
						do
						{
							// read input stream
							byte[] buffer = new byte[FILE_BUFFER_LENGTH];
							readBytes = stream.Read(buffer, 0, FILE_BUFFER_LENGTH);

							if (readBytes < FILE_BUFFER_LENGTH)
								Array.Resize<byte>(ref buffer, readBytes);

							// write remote backup file
							string tempPath = ES.Services.HostedSharePointServersEnt.Enterprise_AppendBackupBinaryChunk(this.SiteCollectionId, fileName, path, buffer);
							if (path == null)
								path = tempPath;

							offset += FILE_BUFFER_LENGTH;
						}
						while (readBytes == FILE_BUFFER_LENGTH);

						uploadedFile = path;
					}
				}
				else
				{
					// package files
					packageFile = fileLookup.SelectedFile;
				}

				int result = ES.Services.HostedSharePointServersEnt.Enterprise_RestoreSiteCollection(this.SiteCollectionId, uploadedFile, packageFile);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SHAREPOINT_RESTORE_SITE", ex);
				return;
			}

			RedirectBack();
		}

		protected void btnRestore_Click(object sender, EventArgs e)
		{
			RestoreSiteCollection();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			RedirectBack();
		}

		private void RedirectBack()
		{
            Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "sharepoint_enterprise_edit_sitecollection", "SiteCollectionID=" + this.SiteCollectionId, "ItemID=" + PanelRequest.ItemID.ToString()));
		}
		protected void radioUpload_CheckedChanged(object sender, EventArgs e)
		{
			ToggleControls();
		}
	}
}
