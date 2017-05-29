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
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.HostedSolution;
using System.Reflection;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeCheckDomainName : SolidCPModuleBase
    {
        private static string EXCHANGEACCOUNTEMAILADDRESSES = "ExchangeAccountEmailAddresses";
        private static string EXCHANGEACCOUNTS = "ExchangeAccounts";
        private static string LYNCUSERS = "LyncUsers";
        private static string SFBUSERS = "SfBUsers";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // save return URL
                if (Request.UrlReferrer!=null)
                    ViewState["ReturnUrl"] = Request.UrlReferrer.ToString();

                // domain name
                DomainInfo domain = ES.Services.Servers.GetDomain(PanelRequest.DomainID);
                litDomainName.Text = domain.DomainName;

                Bind();
            }
        }

        public string GetObjectType(string objectName, int objectType)
        {
            if (objectName == EXCHANGEACCOUNTS)
            {
                ExchangeAccountType accountType = (ExchangeAccountType)objectType;
                objectName = accountType.ToString();
            }

            string res = GetLocalizedString(objectName+".Text");

            if (string.IsNullOrEmpty(res))
                res = objectName;

            return res;
        }

        public bool AllowDelete(string objectName, int objectType)
        {
            if (objectName == EXCHANGEACCOUNTEMAILADDRESSES)
            {
                ExchangeAccountType accountType = (ExchangeAccountType)objectType;
                switch (accountType)
                {
                    case ExchangeAccountType.Room:
                    case ExchangeAccountType.Equipment:
                    case ExchangeAccountType.SharedMailbox:
                    case ExchangeAccountType.Mailbox:
                    case ExchangeAccountType.DistributionList:
                    case ExchangeAccountType.PublicFolder:
                        return true;
                }

            }
            return false;
        }


        public string GetObjectImage(string objectName, int objectType)
        {
            string imgName = "blank16.gif";

            if (objectName == EXCHANGEACCOUNTS)
            {
                ExchangeAccountType accountType = (ExchangeAccountType)objectType;

                imgName = "mailbox_16.gif";
                switch(accountType)
                {
                    case ExchangeAccountType.Contact:
                        imgName = "contact_16.gif";
                        break;
                    case ExchangeAccountType.DistributionList:
                        imgName = "dlist_16.gif";
                        break;
                    case ExchangeAccountType.Room:
                        imgName = "room_16.gif";
                        break;
                    case ExchangeAccountType.Equipment:
                        imgName = "equipment_16.gif";
                        break;
                    case ExchangeAccountType.SharedMailbox:
                        imgName = "shared_16.gif";
                        break;
                }

            }
            else if (objectName == EXCHANGEACCOUNTEMAILADDRESSES)
            {
                imgName = "mailbox_16.gif";
            }

            return GetThemedImage("Exchange/" + imgName);
        }

        public string GetEditUrl(string objectName, int objectType, string objectId, string ownerId)
        {
            if (objectName == EXCHANGEACCOUNTS)
            {
                string key = "";

                ExchangeAccountType accountType = (ExchangeAccountType)objectType;

                switch (accountType)
                {
                    case ExchangeAccountType.User:
                        key = "edit_user";
                        return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), key,
                            "AccountID=" + objectId,
                            "ItemID=" + PanelRequest.ItemID, "context=user");

                    case ExchangeAccountType.Mailbox:
                    case ExchangeAccountType.Room:
                    case ExchangeAccountType.Equipment:
                    case ExchangeAccountType.SharedMailbox:
                        key = "mailbox_settings";
                        break;
                    case ExchangeAccountType.DistributionList:
                        key = "dlist_settings";
                        break;
                    case ExchangeAccountType.PublicFolder:
                        key = "public_folder_settings";
                        break;
                    case ExchangeAccountType.SecurityGroup:
                    case ExchangeAccountType.DefaultSecurityGroup:
                        key = "secur_group_settings";
                        break;
                }

                if (!string.IsNullOrEmpty(key))
                {
                    return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), key,
                            "AccountID=" + objectId,
                            "ItemID=" + PanelRequest.ItemID);
                }
            }

            if (objectName == EXCHANGEACCOUNTEMAILADDRESSES)
            {
                string key = "";

                ExchangeAccountType accountType = (ExchangeAccountType)objectType;

                switch (accountType)
                {
                    case ExchangeAccountType.Mailbox:
                    case ExchangeAccountType.Room:
                    case ExchangeAccountType.Equipment:
                    case ExchangeAccountType.SharedMailbox:
                        key = "mailbox_addresses";
                        break;
                    case ExchangeAccountType.DistributionList:
                        key = "dlist_addresses";
                        break;
                    case ExchangeAccountType.PublicFolder:
                        key = "public_folder_addresses";
                        break;
                }

                if (!string.IsNullOrEmpty(key))
                {
                    return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), key,
                                "AccountID=" + ownerId,
                                "ItemID=" + PanelRequest.ItemID);
                }
            }

            if (objectName == LYNCUSERS)
            {
                return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_lync_user",
                    "AccountID=" + objectId,
                    "ItemID=" + PanelRequest.ItemID);
            }
            if (objectName == SFBUSERS)
            {
                return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "edit_sfb_user",
                    "AccountID=" + objectId,
                    "ItemID=" + PanelRequest.ItemID);
            }

            return "";
        }

        private void Bind()
        {
            DomainInfo domain = ES.Services.Servers.GetDomain(PanelRequest.DomainID);

            gvObjects.DataSource =
                ES.Services.Organizations.GetOrganizationObjectsByDomain(PanelRequest.ItemID, domain.DomainName);
            gvObjects.DataBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (ViewState["ReturnUrl"] != null)
                Response.Redirect((string)ViewState["ReturnUrl"]);
        }

        protected void gvObjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                try
                {
                    string[] arg = e.CommandArgument.ToString().Split(',');
                    if (arg.Length != 3) return;

                    string[] emails = { arg[2] };

                    int accountID = 0;
                    if (!int.TryParse(arg[0], out accountID))
                        return;

                    int accountTypeID = 0;
                    if (!int.TryParse(arg[1], out accountTypeID))
                        return;

                    ExchangeAccountType accountType = (ExchangeAccountType)accountTypeID;

                    int result;

                    switch(accountType)
                    {
                        case ExchangeAccountType.Room:
                        case ExchangeAccountType.Equipment:
                        case ExchangeAccountType.SharedMailbox:
                        case ExchangeAccountType.Mailbox:
                            result = ES.Services.ExchangeServer.DeleteMailboxEmailAddresses(
                                PanelRequest.ItemID, accountID, emails);
                            break;
                        case ExchangeAccountType.DistributionList:
                            result = ES.Services.ExchangeServer.DeleteDistributionListEmailAddresses(
                                PanelRequest.ItemID, accountID, emails);
                            break;
                        case ExchangeAccountType.PublicFolder:
                            result = ES.Services.ExchangeServer.DeletePublicFolderEmailAddresses(
                                PanelRequest.ItemID, accountID, emails);
                            break;
                    }

                    Bind();
                }
                catch (AmbiguousMatchException)
                {
                }
            }
        }



    }
}
