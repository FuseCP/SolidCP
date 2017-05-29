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

﻿using System;
using System.Collections.Generic;
﻿using System.Collections.Specialized;
﻿using System.Linq;
﻿using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
﻿using SolidCP.EnterpriseServer;
﻿using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;

namespace SolidCP.Portal.VPS2012
{
    public partial class VpsDetailsReplications : SolidCPModuleBase
    {
        private const string DateFormat = "MM/dd/yyyy h:mm:ss tt";
        private const string na = "n/a";
        private const string cyclesTemplate = "{0} out of {1} ({2}%)";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPS2012_REPLICATION_ENABLED))
            {
                ShowErrorMessage("VPS_QUOTA_REPLICATION_ENABLED");
                ReplicationPanel.Visible = false;
                return;
            }

            if (!IsPostBack)
            {
                Bind();
            }

            Toogle();
        }

        private void Toogle()
        {
            ReplicaTable.Visible = chbEnable.Checked;

            switch (radRecoveryPoints.SelectedValue)
            {
                case "OnlyLast":
                    tabAdditionalRecoveryPoints.Visible = false;
                    break;
                case "Additional":
                    tabAdditionalRecoveryPoints.Visible = true;
                    VSSdiv.Visible = chbVSS.Checked;
                    break;
            }
        }

        private void Bind()
        {
            try
            {

                var packageVm = ES.Services.VPS2012.GetVirtualMachineItem(PanelRequest.ItemID);
                var vm = ES.Services.VPS2012.GetVirtualMachineExtendedInfo(packageVm.ServiceId, packageVm.VirtualMachineId);
                var serviceSettings = VirtualMachines2012Helper.ConvertArrayToDictionary(ES.Services.Servers.GetServiceSettings(packageVm.ServiceId));

                //var replicaMode = Enum.Parse(typeof(ReplicaMode), serviceSettings["ReplicaMode"]);
                var computerName = serviceSettings["ServerName"];

                var vmReplica = ES.Services.VPS2012.GetReplication(PanelRequest.ItemID);
                var vmReplicaInfo = ES.Services.VPS2012.GetReplicationInfo(PanelRequest.ItemID);
                
                // Enable checkpoint
                chbEnable.Checked = ReplicaTable.Visible = vmReplica != null;

                if (vmReplicaInfo != null)
                {
                    // General labels
                    labPrimaryServer.Text = vmReplicaInfo.PrimaryServerName;
                    labReplicaServer.Text = vmReplicaInfo.ReplicaServerName;
                    labLastSynchronized.Text = vmReplicaInfo.LastSynhronizedAt == DateTime.MinValue ? na : vmReplicaInfo.LastSynhronizedAt.ToString(DateFormat);

                    // Details
                    labHealth.Text = vmReplicaInfo.Health.ToString();
                }
                else
                {
                    labPrimaryServer.Text = labReplicaServer.Text = labLastSynchronized.Text = na;
                    labHealth.Text = "";
                }

                // Certificates list
                ddlCeritficate.Items.Clear();
                var certificates = ES.Services.VPS2012.GetCertificates(packageVm.ServiceId, computerName);
                if (certificates != null)
                {
                    foreach (var cert in certificates)
                    {
                        ddlCeritficate.Items.Add(new ListItem(cert.Title, cert.Thumbprint));
                    }
                }
                if (string.IsNullOrEmpty(computerName))
                {
                    ddlCeritficateDiv.Visible = true;
                    txtCeritficateDiv.Visible = false;
                }
                else
                {
                    ddlCeritficateDiv.Visible = false;
                    txtCeritficateDiv.Visible = true;
                }

                // VHDs editable
                trVHDEditable.Visible = true;
                trVHDReadOnly.Visible = false; 
                chlVHDs.Items.Clear();
                chlVHDs.Items.AddRange(vm.Disks.Select(d => new ListItem(d.Path) {Selected = true}).ToArray());

                if (vmReplica != null)
                {
                    // VHDs readonly
                    labVHDs.Text = "";
                    foreach (var disk in vm.Disks)
                    {
                        if (vmReplica.VhdToReplicate.Any(p=>string.Equals(p, disk.Path, StringComparison.OrdinalIgnoreCase)))
                            labVHDs.Text += disk.Path + "<br>";
                    }
                    trVHDEditable.Visible = false;
                    trVHDReadOnly.Visible = true;

                    // Certificates
                    ddlCeritficate.SelectedValue = txtCeritficate.Text = vmReplica.Thumbprint;

                    // Frequency
                    ddlFrequency.SelectedValue = ((int) vmReplica.ReplicaFrequency).ToString();

                    // Recovery points
                    if (vmReplica.AdditionalRecoveryPoints == 0)
                    {
                        radRecoveryPoints.SelectedValue = "OnlyLast";
                        tabAdditionalRecoveryPoints.Visible = false;
                    }
                    else
                    {
                        radRecoveryPoints.SelectedValue = "Additional";
                        tabAdditionalRecoveryPoints.Visible = true;
                        txtRecoveryPointsAdditional.Text = vmReplica.AdditionalRecoveryPoints.ToString();

                        // VSS
                        if (vmReplica.VSSSnapshotFrequencyHour == 0)
                        {
                            chbVSS.Checked = false;
                            VSSdiv.Visible = false;
                        }
                        else
                        {
                            chbVSS.Checked = true;
                            VSSdiv.Visible = true;
                            txtRecoveryPointsVSS.Text = vmReplica.VSSSnapshotFrequencyHour.ToString();
                        }
                    }

                    BindDetailsPopup(vmReplicaInfo);
                }

                // Details
                /*secReplicationDetails.Visible = */ ReplicationDetailsPanel.Visible = vmReplica != null;

                // Pause buttons
                if (vm.ReplicationState == ReplicationState.Suspended)
                {
                    btnResume.Visible = true;
                    btnPause.Visible = false;
                }
                else
                {
                    btnResume.Visible = false;
                    btnPause.Visible = true;
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_GET_VM_REPLICATION", ex);
            }
        }

        private void BindDetailsPopup(ReplicationDetailInfo vmReplicaInfo)
        {
            labDetailsState.Text = vmReplicaInfo.State.ToString();
            labDetailsMode.Text = vmReplicaInfo.Mode.ToString();
            labDetailsPrimary.Text = vmReplicaInfo.PrimaryServerName;
            labDetailsReplica.Text = vmReplicaInfo.ReplicaServerName;
            labDetailsHealth.Text = vmReplicaInfo.Health.ToString();
            labDetailsHealthDetails.Text = vmReplicaInfo.HealthDetails;

            // statistic
            StatisticCollapsiblePanel.Text = GetLocalizedString("secStatisticPanel.Text") +
                                             ToReadableString(vmReplicaInfo.ToTime - vmReplicaInfo.FromTime);
            labFromTime.Text = vmReplicaInfo.FromTime.ToString(DateFormat);
            labToTime.Text = vmReplicaInfo.ToTime.ToString(DateFormat);
            labAverageSize.Text = vmReplicaInfo.AverageSize;
            labMaximumSize.Text = vmReplicaInfo.MaximumSize;
            labAverageLatency.Text = vmReplicaInfo.AverageLatency.ToString("c");
            labErrorsEncountered.Text = vmReplicaInfo.Errors.ToString();

            var totalCycles = vmReplicaInfo.SuccessfulReplications + vmReplicaInfo.MissedReplicationCount;
            if (totalCycles > 0)
                labSuccessfulReplicationCycles.Text = string.Format(cyclesTemplate,
                    vmReplicaInfo.SuccessfulReplications, totalCycles,
                    Convert.ToInt32(100*vmReplicaInfo.SuccessfulReplications/totalCycles));
            else
                labSuccessfulReplicationCycles.Text = na;

            // pending replication
            labSizeData.Text = vmReplicaInfo.PendingSize;
            labLastSyncro.Text = vmReplicaInfo.LastSynhronizedAt == DateTime.MinValue ? na : vmReplicaInfo.LastSynhronizedAt.ToString(DateFormat);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                ShowErrorMessage("VPS_REQUIRED");
                return;
            }

            if (chbEnable.Checked)
                SetReplication();
            else
                DisableReplication();
        }

        private void SetReplication()
        {
            var vmReplica = new VmReplication();

            vmReplica.VhdToReplicate = chlVHDs.Items.Cast<ListItem>()
                .Where(li => li.Selected)
                .Select(li => li.Value)
                .ToArray();

            vmReplica.Thumbprint = ddlCeritficateDiv.Visible ? ddlCeritficate.SelectedValue : txtCeritficate.Text;
            vmReplica.ReplicaFrequency = (ReplicaFrequency) Convert.ToInt32(ddlFrequency.SelectedValue);
            vmReplica.AdditionalRecoveryPoints = radRecoveryPoints.SelectedValue == "OnlyLast" ? 0 : Convert.ToInt32(txtRecoveryPointsAdditional.Text);
            vmReplica.VSSSnapshotFrequencyHour = chbVSS.Checked ? Convert.ToInt32(txtRecoveryPointsVSS.Text) : 0;

            try
            {
                ResultObject res = ES.Services.VPS2012.SetVmReplication(PanelRequest.ItemID, vmReplica);

                if (res.IsSuccess)
                {
                    Bind();
                    ShowSuccessMessage("VPS_SET_REPLICATION_ERROR");
                }
                else
                    messageBox.ShowMessage(res, "VPS_SET_REPLICATION_ERROR", "VPS");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("VPS_SET_REPLICATION_ERROR", ex);
            }
        }

        private void DisableReplication()
        {
            try
            {
                ResultObject res = ES.Services.VPS2012.DisableVmReplication(PanelRequest.ItemID);

                if (res.IsSuccess)
                {
                    Bind();
                    ShowSuccessMessage("VPS_DISABLE_REPLICATION_ERROR");
                }
                else
                    messageBox.ShowMessage(res, "VPS_DISABLE_REPLICATION_ERROR", "VPS");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("VPS_DISABLE_REPLICATION_ERROR", ex);
            }
        }
        
        protected void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS2012.PauseReplication(PanelRequest.ItemID);

                if (res.IsSuccess)
                {
                    Bind();
                    ShowSuccessMessage("VPS_PAUSE_REPLICATION_ERROR");
                }
                else
                    messageBox.ShowMessage(res, "VPS_PAUSE_REPLICATION_ERROR", "VPS");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("VPS_PAUSE_REPLICATION_ERROR", ex);
            }
        }

        protected void btnResume_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS2012.ResumeReplication(PanelRequest.ItemID);

                if (res.IsSuccess)
                {
                    Bind();
                    ShowSuccessMessage("VPS_RESUME_REPLICATION_ERROR");
                }
                else
                    messageBox.ShowMessage(res, "VPS_RESUME_REPLICATION_ERROR", "VPS");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("VPS_RESUME_REPLICATION_ERROR", ex);
            }
        }

        public string ToReadableString(TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}",
                span.Duration().Days > 0 ? string.Format("{0:0} Day{1}, ", span.Days, span.Days == 1 ? String.Empty : "s") : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0} Hour{1}, ", span.Hours, span.Hours == 1 ? String.Empty : "s") : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0} Minute{1}, ", span.Minutes, span.Minutes == 1 ? String.Empty : "s") : string.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 Minutes";

            return formatted;
        }
    }
}
