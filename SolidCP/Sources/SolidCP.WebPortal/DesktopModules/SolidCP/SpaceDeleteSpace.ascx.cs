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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace SolidCP.Portal
{
    public partial class SpaceDeleteSpace : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    BindPackageItems();
                    BindPackagePackages();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PACKAGE_GET_PACKAGE", ex);
                    return;
                }
            }
        }

        private void BindPackageItems()
        {
            try
            {
                gvItems.DataSource = ES.Services.Packages.GetRawPackageItems(PanelSecurity.PackageId);
                gvItems.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("INIT_SERVICE_ITEM_FORM", ex);
            }
        }

        private void BindPackagePackages()
        {
            gvPackages.DataSource = ES.Services.Packages.GetPackagePackages(PanelSecurity.PackageId);
            gvPackages.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(NavigateURL(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString()));
        }
        protected bool IsValidDomainName(string name)
        {
            return Uri.CheckHostName(name) != UriHostNameType.Unknown;
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int ownerId = PanelSecurity.SelectedUserId;
            DataSet l_oPackageData = ES.Services.Packages.GetRawPackageItems(PanelSecurity.PackageId);
            
            // delete package
            if (chkConfirm.Checked)
            {
                try
                {
                    int result = ES.Services.Packages.DeletePackage(PanelSecurity.PackageId);
                    if (result < 0)
                    {
                        ShowResultMessage(result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("PACKAGE_DELETE_PACKAGE", ex);
                    return;
                }
                var oPackageInfo = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                
                //Delete domain from Mail Cleaner
                foreach(DataRow l_oRow in l_oPackageData.Tables[0].Rows)
                { 
                    if(IsValidDomainName( Convert.ToString(l_oRow["ItemName"])))
                         Knom.Helpers.Net.APIMailCleanerHelper.DomainRemove(Convert.ToString(l_oRow["ItemName"]));
                }

                // return to the listgv
                Response.Redirect(PortalUtils.GetUserHomePageUrl(ownerId));
            }
            else
            {
                ShowWarningMessage("PACKAGE_DELETE_CONFIRM");
            }
        }

        private void RedirectBack(int spaceId)
        {
            
        }
    }
}
