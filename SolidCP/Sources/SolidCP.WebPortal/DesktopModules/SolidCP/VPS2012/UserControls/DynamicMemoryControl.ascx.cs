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
using SolidCP.Providers.Virtualization;

namespace SolidCP.Portal.VPS2012.UserControls
{
    public partial class DynamicMemoryControl : SolidCPControlBase, IVirtualMachineSettingsControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ToggleControls();
        }

        private void ToggleControls()
        {
            tableDynamicMemory.Visible = chkDynamicMemoryEnabled.Checked;
        }

        public VirtualMachineSettingsMode Mode { get; set; }

        public void BindItem(VirtualMachine item)
        {
            if (item.DynamicMemory == null)
                item.DynamicMemory = new DynamicMemory();

            // set values
            chkDynamicMemoryEnabled.Checked = optionDymanicMemoryDisplay.Value = optionDymanicMemorySummary.Value = item.DynamicMemory.Enabled;
            txtMinimum.Text = litMinimumDisplay.Text = litMinimumSummary.Text = item.DynamicMemory.Minimum.ToString();
            txtMaximum.Text = litMaximumDisplay.Text = litMaximumSummary.Text = item.DynamicMemory.Maximum.ToString();
            txtBuffer.Text = litBufferDisplay.Text = litBufferSummary.Text = item.DynamicMemory.Buffer.ToString();
            txtPriority.Text = litPriorityDisplay.Text = litPrioritySummary.Text = item.DynamicMemory.Priority.ToString();

            // set visibilities
            trMinimumDisplay.Visible = trMaximumDisplay.Visible = trBufferDisplay.Visible = trPriorityDisplay.Visible = item.DynamicMemory.Enabled;
            trMinimumSummary.Visible = trMaximumSummary.Visible = trBufferSummary.Visible = trPrioritySummary.Visible = item.DynamicMemory.Enabled;
        }

        public void SaveItem(ref VirtualMachine item)
        {
            if (Mode != VirtualMachineSettingsMode.Edit)
                return;

            item.DynamicMemory = new DynamicMemory
            {
                Enabled = chkDynamicMemoryEnabled.Checked,
                Minimum = ParseInt(txtMinimum.Text),
                Maximum = ParseInt(txtMaximum.Text),
                Buffer = ParseInt(txtBuffer.Text),
                Priority = ParseInt(txtPriority.Text),
            };
        }

        private int ParseInt(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            return Int32.Parse(text);
        }
    }
}
