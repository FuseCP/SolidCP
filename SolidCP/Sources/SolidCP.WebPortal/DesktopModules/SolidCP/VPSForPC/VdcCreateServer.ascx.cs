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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.EnterpriseServer;
using SolidCP.Providers.Virtualization;
using SolidCP.Providers.Common;
using SolidCP.Providers.ResultObjects;
using System.Resources;

namespace SolidCP.Portal.VPSForPC
{
	public partial class VdcCreate : SolidCPModuleBase
	{
		protected PackageContext cntx = null;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				BindFormControls();
			}

			// remove non-required steps
			ToggleWizardSteps();

			// toggle
			ToggleControls();
		}

		private void ToggleWizardSteps()
		{
			//CPU Core
			if (cntx == null)
				cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId); //Load package context

			if (ddlCpu.Items.Count == 0 || String.IsNullOrWhiteSpace(ddlCpu.SelectedValue))
			{
				int maxCores = 0;
				if (!String.IsNullOrWhiteSpace(listOperatingSystems.SelectedValue))
				{
					maxCores = ES.Services.VPSPC.GetMaximumCpuCoresNumber(PanelSecurity.PackageId, listOperatingSystems.SelectedItem.Value);
				}

				if (cntx.Quotas.ContainsKey(Quotas.VPSForPC_CPU_NUMBER))
				{
					QuotaValueInfo cpuQuota = cntx.Quotas[Quotas.VPSForPC_CPU_NUMBER];

					if (cpuQuota.QuotaAllocatedValue != -1
						&& maxCores > cpuQuota.QuotaAllocatedValue)
						maxCores = cpuQuota.QuotaAllocatedValue;
				}

				for (int i = 1; i < maxCores + 1; i++)
					ddlCpu.Items.Add(i.ToString());

				ddlCpu.SelectedIndex = (ddlCpu.Items.Count > 0 ? 0 : -1); // select last (maximum) item
			}

			// external network
			if (!PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPSForPC_EXTERNAL_NETWORK_ENABLED))
			{
				wizard.WizardSteps.Remove(stepExternalNetwork);
				chkExternalNetworkEnabled.Checked = false;
			}

			// private network
			if (!PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPSForPC_PRIVATE_NETWORK_ENABLED))
			{
				wizard.WizardSteps.Remove(stepPrivateNetwork);
				chkPrivateNetworkEnabled.Checked = false;
			}
		}

		private void BindFormControls()
		{
			try
			{
				// OS templates
				listOperatingSystems.DataSource = ES.Services.VPSPC.GetOperatingSystemTemplatesPC(PanelSecurity.PackageId);
				listOperatingSystems.DataBind();
			}
			catch (Exception ex)
			{
				listOperatingSystems.Items.Add(new ListItem(GetLocalizedString("SelectOsTemplate.Text"), ""));
				listOperatingSystems.Enabled = false;
				txtVmName.Enabled = false;

				Button btn;

				btn = ((Button)wizard.FindControl("StepNavigationTemplateContainerID").FindControl("btnNext"));
				if (btn != null)
				{
					btn.Enabled = false;
				}

				btn = ((Button)wizard.FindControl("StartNavigationTemplateContainerID").FindControl("btnNext"));
				if (btn != null)
				{
					btn.Enabled = false;
				}

				messageBox.ShowErrorMessage("VPS_ERROR_CREATE", new Exception("no templates", ex));
			}
			// summary letter e-mail
			PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
			if (package != null)
			{
				UserInfo user = ES.Services.Users.GetUserById(package.UserId);
				if (user != null)
				{
					chkSendSummary.Checked = true;
					txtSummaryEmail.Text = user.Email;
				}
			}

			// load package context
			if (cntx == null)
				cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

			// bind CPU cores
			int maxCores = 0;
			if (!String.IsNullOrWhiteSpace(listOperatingSystems.SelectedValue))
			{
				maxCores = ES.Services.VPSPC.GetMaximumCpuCoresNumber(PanelSecurity.PackageId, listOperatingSystems.SelectedValue);
			}

			if (cntx.Quotas.ContainsKey(Quotas.VPSForPC_CPU_NUMBER))
			{
				QuotaValueInfo cpuQuota = cntx.Quotas[Quotas.VPSForPC_CPU_NUMBER];

				if (cpuQuota.QuotaAllocatedValue != -1
					&& maxCores > cpuQuota.QuotaAllocatedValue)
					maxCores = cpuQuota.QuotaAllocatedValue;
			}

			for (int i = 1; i < maxCores + 1; i++)
				ddlCpu.Items.Add(i.ToString());

			ddlCpu.SelectedIndex = (ddlCpu.Items.Count > 0 ? 0 : -1); // select last (maximum) item

			#region Network
			// external network details
			if (PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPSForPC_EXTERNAL_NETWORK_ENABLED))
			{
			}

			// private network
			if (PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPSForPC_PRIVATE_NETWORK_ENABLED))
			{
				//Fill VLanID list
				UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);
				chkPrivateNetworkEnabled.Checked = chkPrivateNetworkEnabled.Enabled = tablePrivateNetwork.Visible = user.Vlans.Count > 0;
				pVLanListIsEmptyMessage.Visible = user.Vlans.Count == 0;

				ddlPrivateVLanID.DataSource = user.Vlans;
				ddlPrivateVLanID.DataBind();
			}
			#endregion

			// RAM size
			if (cntx.Quotas.ContainsKey(Quotas.VPSForPC_RAM))
			{
				QuotaValueInfo ramQuota = cntx.Quotas[Quotas.VPSForPC_RAM];
				if (ramQuota.QuotaAllocatedValue == -1)
				{
					// unlimited RAM
					txtRam.Text = "";
				}
				else
				{
					int availSize = ramQuota.QuotaAllocatedValue - ramQuota.QuotaUsedValue;
					txtRam.Text = availSize < 0 ? "" : availSize.ToString();
				}

				txtRam.Text = "";
			}

			// HDD size
			if (cntx.Quotas.ContainsKey(Quotas.VPSForPC_HDD))
			{
				QuotaValueInfo hddQuota = cntx.Quotas[Quotas.VPSForPC_HDD];
				if (hddQuota.QuotaAllocatedValue == -1)
				{
					// unlimited HDD
					txtHdd.Text = "";
				}
				else
				{
					int availSize = hddQuota.QuotaAllocatedValue - hddQuota.QuotaUsedValue;

					txtHdd.Text = availSize < 0 ? "" : availSize.ToString();
				}

				txtHdd.Text = "";
			}

			// snapshots number
			if (cntx.Quotas.ContainsKey(Quotas.VPSForPC_SNAPSHOTS_NUMBER))
			{
				int snapsNumber = cntx.Quotas[Quotas.VPSForPC_SNAPSHOTS_NUMBER].QuotaAllocatedValue;
				//txtSnapshots.Text = (snapsNumber != -1) ? snapsNumber.ToString() : "";
				//txtSnapshots.Enabled = (snapsNumber != 0);
			}

			// toggle controls
			//BindCheckboxOption(chkDvdInstalled, Quotas.VPS_DVD_ENABLED);
			//chkBootFromCd.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, Quotas.VPSForPC_BOOT_CD_ALLOWED);

			BindCheckboxOption(chkStartShutdown, Quotas.VPSForPC_START_SHUTDOWN_ALLOWED);
			BindCheckboxOption(chkPauseResume, Quotas.VPSForPC_PAUSE_RESUME_ALLOWED);
			BindCheckboxOption(chkReset, Quotas.VPSForPC_RESET_ALOWED);
			BindCheckboxOption(chkReboot, Quotas.VPSForPC_REBOOT_ALLOWED);
			BindCheckboxOption(chkReinstall, Quotas.VPSForPC_REINSTALL_ALLOWED);
		}

		private void BindCheckboxOption(CheckBox chk, string quotaName)
		{
			chk.Enabled = PackagesHelper.IsQuotaEnabled(PanelSecurity.PackageId, quotaName);
			chk.Checked = chk.Enabled;
		}

		private void ToggleControls()
		{
			// send letter
			txtSummaryEmail.Enabled = chkSendSummary.Checked;
			SummaryEmailValidator.Enabled = chkSendSummary.Checked;

			// private network
			tablePrivateNetwork.Visible = chkPrivateNetworkEnabled.Checked;
		}

		private void BindSummary()
		{
			// general
			litHostname.Text = txtVmName.Text.Trim();
			//            litHostname.Text =  PortalAntiXSS.Decode(String.Format("{0}.{1}", txtHostname.Text.Trim(), txtDomain.Text.Trim()));
			litOperatingSystem.Text = listOperatingSystems.SelectedItem.Text;

			litSummaryEmail.Text = PortalAntiXSS.Encode(txtSummaryEmail.Text.Trim());
			SummSummaryEmailRow.Visible = chkSendSummary.Checked;

			// config
            litCpu.Text = PortalAntiXSS.Encode(ddlCpu.SelectedValue);
            litRam.Text = PortalAntiXSS.Encode(txtRam.Text.Trim());
            litHdd.Text = PortalAntiXSS.Encode(txtHdd.Text.Trim());
			//litSnapshots.Text = PortalAntiXSS.Decode(txtSnapshots.Text.Trim());
			//optionDvdInstalled.Value = chkDvdInstalled.Checked;
			//optionBootFromCd.Value = chkBootFromCd.Checked;
			//optionNumLock.Value = chkNumLock.Checked;
			optionStartShutdown.Value = chkStartShutdown.Checked;
			optionPauseResume.Value = chkPauseResume.Checked;
			optionReboot.Value = chkReboot.Checked;
			optionReset.Value = chkReset.Checked;
			optionReinstall.Value = chkReinstall.Checked;

			// external network
			optionExternalNetwork.Value = chkExternalNetworkEnabled.Checked;

			// private network
			optionPrivateNetwork.Value = chkPrivateNetworkEnabled.Checked;
			litPrivateNetworkVLanID.Text = ddlPrivateVLanID.SelectedValue;
		}

		protected void wizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
		{
			if (!Page.IsValid)
				return;

			IntResult res = null;

			try
			{
				string adminPassword = (string)ViewState["Password"];

				string summaryEmail = chkSendSummary.Checked ? txtSummaryEmail.Text.Trim() : null;

				res = ES.Services.VPSPC.CreateVirtualMachine(PanelSecurity.PackageId,
					String.Empty, String.Empty, listOperatingSystems.SelectedValue, txtVmName.Text.Trim(), adminPassword, summaryEmail,
					Utils.ParseInt(ddlCpu.SelectedValue), Utils.ParseInt(txtRam.Text.Trim()),
					Utils.ParseInt(txtHdd.Text.Trim()), 0,
					false, false, false,
					chkStartShutdown.Checked, chkPauseResume.Checked, chkReboot.Checked, chkReset.Checked, chkReinstall.Checked,
					chkExternalNetworkEnabled.Checked, null, null, string.Empty,
					chkPrivateNetworkEnabled.Checked, null, null, string.Empty, (String.IsNullOrEmpty(ddlPrivateVLanID.SelectedValue) ? ((ushort)0) : ushort.Parse(ddlPrivateVLanID.SelectedValue)));


				if (res.IsSuccess == false)
				{
					messageBox.ShowMessage(res, "VPS_ERROR_CREATE", "VPS");
					//
					return;
				}
				//
				Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), String.Empty));
				
			}
			catch (Exception ex)
			{
				messageBox.ShowErrorMessage("VPS_ERROR_CREATE", ex);
				//Response.Redirect(EditUrl("SpaceID",PanelSecurity.PackageId.ToString(), null));
			}
		}

		protected void wizard_SideBarButtonClick(object sender, WizardNavigationEventArgs e)
		{
			if (e.NextStepIndex < e.CurrentStepIndex)
				return;

			Page.Validate("VpsWizard");

			if (!Page.IsValid)
				e.Cancel = true;
		}

		protected void wizard_ActiveStepChanged(object sender, EventArgs e)
		{
			if (cntx == null)
				cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

			if ((wizard.ActiveStepIndex == 1)
			 && (cntx.Quotas.ContainsKey(Quotas.VPSForPC_HDD)))
			{
				QuotaValueInfo hddQuota = cntx.Quotas[Quotas.VPSForPC_HDD];
				if (hddQuota.QuotaAllocatedValue == -1)
				{
					// unlimited HDD
					txtHdd.Text = "";
				}
				else
				{
					int availSize = hddQuota.QuotaAllocatedValue - hddQuota.QuotaUsedValue;

					if (availSize <= 0)
					{
						txtHdd.Text = "0";
						txtHdd.Enabled = false;

						Button btn = ((Button)wizard.FindControl("StepNavigationTemplateContainerID").FindControl("btnNext"));
						if (btn != null)
						{
							btn.Enabled = false;
						}

						messageBox.ShowErrorMessage("VPS_ERROR_CREATE", new Exception("The HDD quota has been exhausted."));
						return;
					}
					else
					{
						txtHdd.Text = availSize.ToString();
					}

					txtHdd.Text = "";
				}
			}

			BindSummary();
		}

		protected void wizard_NextButtonClick(object sender, WizardNavigationEventArgs e)
		{
		}
	}
}
