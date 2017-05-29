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
using SolidCP.Providers.HostedSolution;
using SolidCP.EnterpriseServer;

namespace SolidCP.Portal.ExchangeServer
{
    public partial class ExchangeMailboxMemberOf : SolidCPModuleBase
    {
        protected PackageContext cntx;

        protected PackageContext Cntx
        {
            get
            {
                if (cntx == null)
                {
                    cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                }

                return cntx;
            }
        }

        protected bool EnableSecurityGroups
        {
            get
            {
                return Utils.CheckQouta(Quotas.ORGANIZATION_SECURITYGROUPS, Cntx);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            groups.SecurityGroupsEnabled = EnableSecurityGroups;

            if (!IsPostBack)
            {
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                BindSettings();

                UserInfo user = UsersHelper.GetUser(PanelSecurity.EffectiveUserId);
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

                // title
                litDisplayName.Text = mailbox.DisplayName;

                List<ExchangeAccount> groupsList = new List<ExchangeAccount>();

                //Distribution Lists
                ExchangeAccount[] dLists = ES.Services.ExchangeServer.GetDistributionListsByMember(PanelRequest.ItemID, PanelRequest.AccountID);
                foreach (ExchangeAccount distList in dLists)
                {
                    groupsList.Add(distList);
                }

                if (EnableSecurityGroups)
                {
                    //Security Groups
                    ExchangeAccount[] securGroups = ES.Services.Organizations.GetSecurityGroupsByMember(PanelRequest.ItemID, PanelRequest.AccountID);

                    foreach (ExchangeAccount secGroup in securGroups)
                    {
                        groupsList.Add(secGroup);
                    }
                }

                groups.SetAccounts(groupsList.ToArray());

            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_GET_MAILBOX_SETTINGS", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                IList<ExchangeAccount> oldGroups = new List<ExchangeAccount>();

                if (EnableSecurityGroups)
                {
                    ExchangeAccount[] oldSecGroups = ES.Services.Organizations.GetSecurityGroupsByMember(PanelRequest.ItemID, PanelRequest.AccountID);

                    foreach (ExchangeAccount secGroup in oldSecGroups)
                    {
                        oldGroups.Add(secGroup);
                    }
                }

                ExchangeAccount[] oldDistLists = ES.Services.ExchangeServer.GetDistributionListsByMember(PanelRequest.ItemID, PanelRequest.AccountID);

                foreach (ExchangeAccount distList in oldDistLists)
                {
                    oldGroups.Add(distList);
                }

                IDictionary<string, ExchangeAccountType> newGroups = groups.GetFullAccounts();
                foreach (ExchangeAccount oldGroup in oldGroups)
                {
                    if (newGroups.ContainsKey(oldGroup.AccountName))
                    {
                        newGroups.Remove(oldGroup.AccountName);
                    }
                    else
                    {
                        switch (oldGroup.AccountType)
                        {
                            case ExchangeAccountType.DistributionList:
                                ES.Services.ExchangeServer.DeleteDistributionListMember(PanelRequest.ItemID, oldGroup.AccountName, PanelRequest.AccountID);
                                break;
                            case ExchangeAccountType.SecurityGroup:
                                ES.Services.Organizations.DeleteObjectFromSecurityGroup(PanelRequest.ItemID, PanelRequest.AccountID, oldGroup.AccountName);
                                break;
                        }
                    }
                }

                foreach (KeyValuePair<string, ExchangeAccountType> newGroup in newGroups)
                {
                    switch (newGroup.Value)
                    {
                        case ExchangeAccountType.DistributionList:
                            ES.Services.ExchangeServer.AddDistributionListMember(PanelRequest.ItemID, newGroup.Key, PanelRequest.AccountID);
                            break;
                        case ExchangeAccountType.SecurityGroup:
                            ES.Services.Organizations.AddObjectToSecurityGroup(PanelRequest.ItemID, PanelRequest.AccountID, newGroup.Key);
                            break;
                    }
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILBOX_SETTINGS");
                BindSettings();
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILBOX_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            SaveSettings();

            Response.Redirect(PortalUtils.EditUrl("ItemID", PanelRequest.ItemID.ToString(),
                "mailboxes",
                "SpaceID=" + PanelSecurity.PackageId));
        }

    }
}
