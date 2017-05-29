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
using SolidCP.Providers.Common;
using SolidCP.Providers.Virtualization;

namespace SolidCP.Portal.VPS
{
    public partial class VpsDetailsSnapshots : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSnapshotsTree();
            }
        }

        private void BindSnapshotsTree()
        {
            VirtualMachineSnapshot[] snapshots = ES.Services.VPS.GetVirtualMachineSnapshots(PanelRequest.ItemID);
            
            // clear tree
            SnapshotsTree.Nodes.Clear();

            // fill tree by root nodes
            AddChildNodes(SnapshotsTree.Nodes, null, snapshots);

            // select first node
            if (SnapshotsTree.Nodes.Count > 0)
            {
                SnapshotsTree.Nodes[0].Selected = true;
            }

            // refresh
            BindSelectedNode();

            // quotas
            VirtualMachine vm = ES.Services.VPS.GetVirtualMachineItem(PanelRequest.ItemID);
            snapshotsQuota.QuotaUsedValue = snapshots.Length;
            snapshotsQuota.QuotaValue = vm.SnapshotsNumber;
            btnTakeSnapshot.Enabled = snapshots.Length < vm.SnapshotsNumber;
        }

        private void BindSelectedNode()
        {
            TreeNode node = SnapshotsTree.SelectedNode;

            btnApply.Enabled =
                btnRename.Enabled =
                btnDelete.Enabled =
                btnDeleteSubtree.Enabled =
                SnapshotDetailsPanel.Visible = (node != null);

            NoSnapshotsPanel.Visible = (SnapshotsTree.Nodes.Count == 0);

            if (node != null)
            {
                // set name
                txtSnapshotName.Text = node.Text;

                // load snapshot details
                VirtualMachineSnapshot snapshot = ES.Services.VPS.GetSnapshot(PanelRequest.ItemID, node.Value);
                if (snapshot != null)
                    litCreated.Text = snapshot.Created.ToString();

                // set image
                imgThumbnail.ImageUrl =
                    string.Format("~/DesktopModules/SolidCP/VPS/VirtualMachineSnapshotImage.ashx?ItemID={0}&SnapshotID={1}&rnd={2}",
                    PanelRequest.ItemID, HttpUtility.UrlEncode(node.Value), DateTime.Now.Ticks);
            }
        }

        private void AddChildNodes(TreeNodeCollection parent, string parentId, VirtualMachineSnapshot[] snapshots)
        {
            foreach (VirtualMachineSnapshot snapshot in snapshots)
            {
                if (snapshot.ParentId == parentId)
                {
                    // add node
                    TreeNode node = new TreeNode(snapshot.Name, snapshot.Id);
                    node.Expanded = true;
                    node.ImageUrl = PortalUtils.GetThemedImage("VPS/snapshot.png");
                    parent.Add(node);

                    // check if the current
                    if (snapshot.IsCurrent)
                    {
                        TreeNode nowNode = new TreeNode(GetLocalizedString("Now.Text"), "");
                        nowNode.ImageUrl = PortalUtils.GetThemedImage("VPS/start2.png");
                        nowNode.SelectAction = TreeNodeSelectAction.None;
                        node.ChildNodes.Add(nowNode);
                    }

                    // fill children
                    AddChildNodes(node.ChildNodes, snapshot.Id, snapshots);
                }
            }
        }

        protected void btnTakeSnapshot_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS.CreateSnapshot(PanelRequest.ItemID);

                if (res.IsSuccess)
                {
                    // bind tree
                    BindSnapshotsTree();
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_TAKE_SNAPSHOT", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_TAKE_SNAPSHOT", ex);
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS.ApplySnapshot(PanelRequest.ItemID, GetSelectedSnapshot());

                if (res.IsSuccess)
                {
                    // bind tree
                    BindSnapshotsTree();
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_APPLY_SNAPSHOT", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_APPLY_SNAPSHOT", ex);
            }
        }

        protected void btnRenameSnapshot_Click(object sender, EventArgs e)
        {
            try
            {
                string newName = txtSnapshotName.Text.Trim();
                ResultObject res = ES.Services.VPS.RenameSnapshot(PanelRequest.ItemID, GetSelectedSnapshot(), newName);

                if (res.IsSuccess)
                {
                    // bind tree
                    SnapshotsTree.SelectedNode.Text = newName;
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_RENAME_SNAPSHOT", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_RENAME_SNAPSHOT", ex);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS.DeleteSnapshot(PanelRequest.ItemID, GetSelectedSnapshot());

                if (res.IsSuccess)
                {
                    // bind tree
                    BindSnapshotsTree();
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETE_SNAPSHOT", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETE_SNAPSHOT", ex);
            }
        }

        protected void btnDeleteSubtree_Click(object sender, EventArgs e)
        {
            try
            {
                ResultObject res = ES.Services.VPS.DeleteSnapshotSubtree(PanelRequest.ItemID, GetSelectedSnapshot());

                if (res.IsSuccess)
                {
                    // bind tree
                    BindSnapshotsTree();
                    return;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_DELETE_SNAPSHOT_SUBTREE", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_DELETE_SNAPSHOT_SUBTREE", ex);
            }
        }

        private string GetSelectedSnapshot()
        {
            return SnapshotsTree.SelectedNode.Value;
        }

        protected void SnapshotsTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindSelectedNode();
        }
    }
}
