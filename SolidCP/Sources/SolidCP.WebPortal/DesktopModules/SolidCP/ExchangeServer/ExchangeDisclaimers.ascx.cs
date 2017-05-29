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

using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeDisclaimers : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDisclaimers();

                BindStats();
            }
        }

        ExchangeDisclaimer[] disclaimerList = null;
        private void BindDisclaimers()
        {
            disclaimerList = ES.Services.ExchangeServer.GetExchangeDisclaimers(PanelRequest.ItemID);

            gvLists.DataSource = disclaimerList;
            gvLists.DataBind();
        }

        private void BindStats()
        {
            listsQuota.QuotaValue = -1;
            if (disclaimerList!=null)
                listsQuota.QuotaUsedValue = disclaimerList.Length;
        }

        protected void btnCreateList_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "disclaimers_settings",
                "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        public string GetListEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "disclaimers_settings",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID.ToString());
        }

        protected void odsAccountsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                messageBox.ShowErrorMessage("EXCHANGE_DISCLAIMERS_LISTS", e.Exception);
                e.ExceptionHandled = true;
            }
        }

        protected void gvLists_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {

                int accountId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                try
                {
                    int result = ES.Services.ExchangeServer.DeleteExchangeDisclaimer(PanelRequest.ItemID, accountId);
                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }

                    // rebind grid
                    BindDisclaimers();

                    BindStats();
                }
                catch (Exception ex)
                {
                    messageBox.ShowErrorMessage("EXCHANGE_DELETE_DISCLAIMER", ex);
                }
            }

        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)   
        {   
            //gvLists.PageSize = Convert.ToInt16(ddlPageSize.SelectedValue);   
       
            // rebind grid   
            BindDisclaimers();
       
            // bind stats   
            BindStats();   
       
        }  

    }
}
