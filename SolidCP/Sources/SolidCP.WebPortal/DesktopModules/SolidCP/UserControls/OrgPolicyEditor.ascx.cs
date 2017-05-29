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
using System.Text;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer.Base.HostedSolution;

namespace SolidCP.Portal.UserControls
{
    public partial class OrgPolicyEditor : UserControl
    {
        #region Properties

        public string Value
        {
            get
            {
                return chkEnablePolicy.Checked.ToString();
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    chkEnablePolicy.Checked = false;
                }
                else
                {
                    try
                    {
                        chkEnablePolicy.Checked = Utils.ParseBool(value, true);
                    }
                    catch {}
                }

                ToggleControls();
            }
        }

        #endregion

        #region Methods

        public void SetAdditionalGroups(AdditionalGroup[] additionalGroups)
        {
            BindAdditionalGroups(additionalGroups);
        }

        public List<AdditionalGroup> GetGridViewGroups()
        {
            List<AdditionalGroup> additionalGroups = new List<AdditionalGroup>();
            for (int i = 0; i < gvAdditionalGroups.Rows.Count; i++)
            {
                GridViewRow row = gvAdditionalGroups.Rows[i];
                ImageButton cmdEdit = (ImageButton)row.FindControl("cmdEdit");
                if (cmdEdit == null)
                    continue;

                AdditionalGroup group = new AdditionalGroup();
                group.GroupId = (int)gvAdditionalGroups.DataKeys[i][0];
                group.GroupName = ((Literal)row.FindControl("litDisplayAdditionalGroup")).Text;

                additionalGroups.Add(group);
            }

            return additionalGroups;
        }

        protected void ToggleControls()
        {
            PolicyBlock.Visible = chkEnablePolicy.Checked;
        }

        protected void BindAdditionalGroups(AdditionalGroup[] additionalGroups)
        {
            gvAdditionalGroups.DataSource = additionalGroups;
            gvAdditionalGroups.DataBind();
        }

        protected int GetRowIndexByDataKey(int dataKey)
        {
            int index = 0;
            foreach (DataKey key in gvAdditionalGroups.DataKeys)
            {
                if (Utils.ParseInt(key.Value, 0) == dataKey)
                    break;

                index++;
            }

            return index >= gvAdditionalGroups.DataKeys.Count ? -1 : index;
        }

        #endregion

        #region Event Handlers

        protected void chkEnablePolicy_CheckedChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void btnAddAdditionalGroup_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            List<AdditionalGroup> additionalGroups = GetGridViewGroups();

            AdditionalGroup additionalGroup = new AdditionalGroup();

            additionalGroup.GroupId = additionalGroups.Count != 0
                ? additionalGroups.Select(x => x.GroupId).Max() + 1
                : 1;

            additionalGroup.GroupName = txtAdditionalGroup.Text;

            additionalGroups.Add(additionalGroup);

            BindAdditionalGroups(additionalGroups.ToArray());

            txtAdditionalGroup.Text = string.Empty;
        }

        protected void btnUpdateAdditionalGroup_Click(object sender, EventArgs e)
        {
            if (ViewState["AdditionalGroupID"] == null || !Page.IsValid)
                return;

            List<AdditionalGroup> additionalGroups = GetGridViewGroups();

            additionalGroups
                .Where(x => x.GroupId == (int)ViewState["AdditionalGroupID"])
                .First().GroupName = txtAdditionalGroup.Text;

            BindAdditionalGroups(additionalGroups.ToArray());

            txtAdditionalGroup.Text = string.Empty;
        }

        protected void gvAdditionalGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int additionalGroupId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

            List<AdditionalGroup> additionalGroups = GetGridViewGroups();

            int rowIndex = GetRowIndexByDataKey(additionalGroupId);

            if (rowIndex != -1)
            {
                switch (e.CommandName)
                {
                    case "DeleteItem":
                        BindAdditionalGroups(
                            additionalGroups
                                .Where(x => x.GroupId != additionalGroupId).ToArray());
                        
                        break;
                    case "EditItem":
                        ViewState["AdditionalGroupID"] = additionalGroupId;

                        txtAdditionalGroup.Text = additionalGroups
                            .Where(x => x.GroupId == additionalGroupId)
                            .Select(y => y.GroupName).First();

                        break;
                }
            }
        }

        protected void DuplicateName_Validation(object source, ServerValidateEventArgs arguments)
        {
            List<AdditionalGroup> additionalGroups = GetGridViewGroups();

            arguments.IsValid = (additionalGroups.Where(x => x.GroupName.Trim() == arguments.Value).Count() == 0);
        }

        #endregion
    }
}
