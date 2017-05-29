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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using SolidCP.EnterpriseServer;

namespace SolidCP.Portal
{
    public partial class HostingPlansQuotas : SolidCPControlBase
    {
        DataSet dsQuotas = null;

        public bool IsPlan = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                ToggleQuotaControls();
            }
        }

        public void BindPackageQuotas(int packageId)
        {
            try
            {
                dsQuotas = ES.Services.Packages.GetPackageQuotasForEdit(packageId);
                dlGroups.DataSource = dsQuotas.Tables[0];
                dlGroups.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            ToggleQuotaControls();
        }

        public void BindPlanQuotas(int packageId, int planId, int serverId)
        {
            try
            {
                dsQuotas = ES.Services.Packages.GetHostingPlanQuotas(packageId, planId, serverId);
                dlGroups.DataSource = dsQuotas.Tables[0];
                dlGroups.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            ToggleQuotaControls();
        }

        private void ToggleQuotaControls()
        {
            foreach (RepeaterItem item in dlGroups.Items)
            {
                CheckBox chkEnabled = (CheckBox)item.FindControl("chkEnabled");

                CheckBox chkCountDiskspace = (CheckBox)item.FindControl("chkCountDiskspace");
                CheckBox chkCountBandwidth = (CheckBox)item.FindControl("chkCountBandwidth");
                chkCountDiskspace.Enabled = chkEnabled.Checked && IsPlan;
                chkCountBandwidth.Enabled = chkEnabled.Checked && IsPlan;

                // iterate quotas
                Control quotaPanel = item.FindControl("QuotaPanel");
                quotaPanel.Visible = chkEnabled.Checked;

                DataList dlQuotas = (DataList)item.FindControl("dlQuotas");
                foreach (DataListItem quotaItem in dlQuotas.Items)
                {
                    if (!chkEnabled.Checked)
                    {
                        QuotaEditor quotaEditor = (QuotaEditor)quotaItem.FindControl("quotaEditor");
                        quotaEditor.QuotaValue = 0;
                    }
                }

                // hide group if quotas == 0
                Control groupPanel = item.FindControl("GroupPanel");
                groupPanel.Visible = (IsPlan || dlQuotas.Items.Count > 0);
            }
        }

        List<HostingPlanGroupInfo> groups;
        List<HostingPlanQuotaInfo> quotas;

        public HostingPlanGroupInfo[] Groups
        {
            get
            {
                if (groups == null)
                    CollectFormData();

                return groups.ToArray();
            }
        }

        public HostingPlanQuotaInfo[] Quotas
        {
            get
            {
                if (quotas == null)
                    CollectFormData();

                return quotas.ToArray();
            }
        }

        public void CollectFormData()
        {
            groups = new List<HostingPlanGroupInfo>();
            quotas = new List<HostingPlanQuotaInfo>();

            // gather info
            foreach (RepeaterItem item in dlGroups.Items)
            {
                Literal litGroupId = (Literal)item.FindControl("groupId");
                CheckBox chkEnabled = (CheckBox)item.FindControl("chkEnabled");
                CheckBox chkCountDiskspace = (CheckBox)item.FindControl("chkCountDiskspace");
                CheckBox chkCountBandwidth = (CheckBox)item.FindControl("chkCountBandwidth");

                if (!chkEnabled.Checked)
                    continue; // disabled group

                HostingPlanGroupInfo group = new HostingPlanGroupInfo();
                group.GroupId = Utils.ParseInt(litGroupId.Text, 0);
                group.Enabled = chkEnabled.Checked;
                group.CalculateDiskSpace = chkCountDiskspace.Checked;
                group.CalculateBandwidth = chkCountBandwidth.Checked;
                groups.Add(group);

                // iterate quotas
                DataList dlQuotas = (DataList)item.FindControl("dlQuotas");
                foreach (DataListItem quotaItem in dlQuotas.Items)
                {
                    QuotaEditor quotaEditor = (QuotaEditor)quotaItem.FindControl("quotaEditor");
                 
                    HostingPlanQuotaInfo quota = new HostingPlanQuotaInfo();
                    quota.QuotaId = quotaEditor.QuotaId;
                    quota.QuotaValue = quotaEditor.QuotaValue;
                    quotas.Add(quota);
                }
            }
        }

        /*
        public void SavePlanQuotas(int planId)
        {
            CollectFormData();

            // update plan quotas
            ES.Packages.UpdateHostingPlanQuotas(planId, groups.ToArray(), quotas.ToArray());
        }

        public void SavePackageQuotas(int packageId)
        {
            CollectFormData();

            // update plan quotas
            ES.Packages.UpdatePackageQuotas(packageId, groups.ToArray(), quotas.ToArray());
        }
         * */

        public DataView GetGroupQuotas(int groupId)
        {
            return new DataView(dsQuotas.Tables[1], "GroupID=" + groupId.ToString(), "", DataViewRowState.CurrentRows);
        }

        public string GetSharedLocalizedStringNotEmpty(string resourceKey, object resourceDescription)
        {
            string result = GetSharedLocalizedString("Quota." + resourceKey);
            if (string.IsNullOrEmpty(result))
            {
                result = resourceKey;

                string resourceDescriptionString = resourceDescription as string;
                if (!string.IsNullOrEmpty(resourceDescriptionString))
                {
                    result = resourceDescriptionString;
                }
                else if (result.IndexOf('.') > 0 && result.Substring(result.IndexOf('.')).Length > 1)
                {
                    // drop Quota name prefix
                    // HeliconZoo.python -> python

                    result = result.Substring(result.IndexOf('.')+1);
                }
            }

            return result;
        }
    }
}
