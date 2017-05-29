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

using System.Linq;
using System.Collections.Generic;
using SolidCP.EnterpriseServer;
using SolidCP.EnterpriseServer.Base.HostedSolution;

namespace SolidCP.Portal
{
    public partial class SettingsExchangePolicy : SolidCPControlBase, IUserSettingsEditorControl
    {
        internal static AdditionalGroup[] additionalGroups;

        #region IUserSettingsEditorControl Members

        public void BindSettings(UserSettings settings)
        {
            // mailbox
            mailboxPasswordPolicy.Value = settings["MailboxPasswordPolicy"];
            orgIdPolicy.Value = settings["OrgIdPolicy"];

            additionalGroups = ES.Services.Organizations.GetAdditionalGroups(settings.UserId);
            
            orgPolicy.SetAdditionalGroups(additionalGroups);
            orgPolicy.Value = settings["OrgPolicy"];
        }

        public void SaveSettings(UserSettings settings)
        {
            settings["MailboxPasswordPolicy"] = mailboxPasswordPolicy.Value;
            settings["OrgIdPolicy"] = orgIdPolicy.Value;
            settings["OrgPolicy"] = orgPolicy.Value;

            if (Utils.ParseBool(orgPolicy.Value, false))
            {
                List<AdditionalGroup> newAdditionalGroups = orgPolicy.GetGridViewGroups();
 
                foreach (AdditionalGroup oldGroup in additionalGroups)
                {
                    AdditionalGroup upGroup = newAdditionalGroups.Where(x => x.GroupId == oldGroup.GroupId).FirstOrDefault();

                    if(upGroup != null && upGroup.GroupName != oldGroup.GroupName)
                    {
                        ES.Services.Organizations.UpdateAdditionalGroup(oldGroup.GroupId, upGroup.GroupName);

                        newAdditionalGroups.Remove(upGroup);
                    }
                    else
                    {
                        ES.Services.Organizations.DeleteAdditionalGroup(oldGroup.GroupId);
                    }
                }

                foreach (AdditionalGroup newGroup in newAdditionalGroups)
                {
                    ES.Services.Organizations.AddAdditionalGroup(settings.UserId, newGroup.GroupName);
                }
            }
        }

        #endregion
    }
}
