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
using SolidCP.Providers;

namespace SolidCP.Portal
{
    public partial class SpaceImportResources : SolidCPModuleBase
    {
        private static TreeNode rootNode;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tree.Attributes.Add("onClick", "TreeViewCheckBoxClicked(event)");

                PrepareTree();
            }
        }

        private void PrepareTree()
        {
            // prepare tree
            tree.CollapseImageUrl = PortalUtils.GetThemedImage("min.gif");
            tree.ExpandImageUrl = PortalUtils.GetThemedImage("max.gif");
            tree.NoExpandImageUrl = PortalUtils.GetThemedImage("empty.gif");
            tree.Nodes.Clear();

            rootNode = new TreeNode();
            rootNode.ImageUrl = PortalUtils.GetThemedImage("folder.png");
            rootNode.Text = GetLocalizedString("Text.Resources");
            rootNode.SelectAction = TreeNodeSelectAction.None;
            rootNode.Value = "Root";
            rootNode.Expanded = true;
            tree.Nodes.Add(rootNode);

            // populate root node
            TreeNode node;
            ServiceProviderItemType[] types = ES.Services.Import.GetImportableItemTypes(PanelSecurity.PackageId);
            foreach (ServiceProviderItemType type in types)
            {
                node = new TreeNode();
                node.Value = "-" + type.ItemTypeId.ToString();
                node.Text = GetSharedLocalizedString("ServiceItemType." + type.DisplayName);
                node.PopulateOnDemand = true;
                node.SelectAction = TreeNodeSelectAction.None;
                node.ImageUrl = PortalUtils.GetThemedImage("folder.png");
                rootNode.ChildNodes.Add(node);
            }

            // Add Import HostHeaders
            node = new TreeNode();
            node.Value = "+100";
            node.Text = GetSharedLocalizedString("ServiceItemType.HostHeader");
            node.PopulateOnDemand = true;
            node.SelectAction = TreeNodeSelectAction.None;
            node.ImageUrl = PortalUtils.GetThemedImage("folder.png");
            rootNode.ChildNodes.Add(node);



        }

        protected void tree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.Value.StartsWith("-"))
            {
                int itemTypeId = Utils.ParseInt(e.Node.Value.Substring(1), 0);
                string[] items = ES.Services.Import.GetImportableItems(PanelSecurity.PackageId, itemTypeId);

                foreach (string item in items)
                {
                    TreeNode node = new TreeNode();
                    node.Text = item;
                    node.Value = itemTypeId.ToString() + "|" + item;
                    node.ShowCheckBox = true;
                    node.SelectAction = TreeNodeSelectAction.None;
                    e.Node.ChildNodes.Add(node);
                }
            }

            if (e.Node.Value.StartsWith("+"))
            {
                int itemTypeId = Utils.ParseInt(e.Node.Value.Substring(1), 0);
                string[] items = ES.Services.Import.GetImportableItems(PanelSecurity.PackageId, itemTypeId * -1);

                switch (itemTypeId)
                {
                    case 100:

                        TreeNode headerNode = new TreeNode();
                        headerNode.Text = GetSharedLocalizedString("ServiceItemType.HostHeader");
                        headerNode.Value = "+" + itemTypeId.ToString();
                        headerNode.ShowCheckBox = true;
                        headerNode.SelectAction = TreeNodeSelectAction.None;
                        e.Node.ChildNodes.Add(headerNode);

                        foreach (string item in items)
                        {
                            string[] objectData = item.Split('|');

                            TreeNode userNode = null;
                            foreach (TreeNode n in headerNode.ChildNodes)
                            {
                                if (n.Value == "+" + itemTypeId.ToString() + "|" + objectData[1]) 
                                {
                                    userNode = n;
                                    break;
                                }
                            }

                            if (userNode == null)
                            {
                                userNode = new TreeNode();
                                userNode.Text = objectData[0];
                                userNode.Value = "+" + itemTypeId.ToString() + "|" + objectData[1];
                                userNode.ShowCheckBox = true;
                                userNode.SelectAction = TreeNodeSelectAction.None;
                                headerNode.ChildNodes.Add(userNode);
                            }

                            TreeNode siteNode = new TreeNode();
                            siteNode.Text = objectData[3];
                            siteNode.Value = "+" + itemTypeId.ToString() + "|" + item;
                            siteNode.ShowCheckBox = true;
                            userNode.SelectAction = TreeNodeSelectAction.None;
                            userNode.ChildNodes.Add(siteNode);
                        }

                        headerNode.Expand();
                        break;
                }

            }

        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            // collect data
            List<string> items = new List<string>();
            CollectNodesData(items, tree.Nodes);

            // import
            int result = ES.Services.Import.ImportItems(true, TaskID, PanelSecurity.PackageId, items.ToArray());

			if (result < 0)
			{
				ShowResultMessage(result);
				return;
			}

            // reset tree
            PrepareTree();

			// show progress dialog
			AsyncTaskID = TaskID;
			AsyncTaskTitle = GetLocalizedString("Text.ImportItems");
        }

        private void CollectNodesData(List<string> items, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                    items.Add(node.Value);

                // process children
                if(node.ChildNodes.Count > 0)
                    CollectNodesData(items, node.ChildNodes);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }


        void checkChildNodes(TreeNodeCollection ptnChildren, bool isChecked)
        {
            foreach (TreeNode childNode in ptnChildren)
            {
                childNode.Checked = isChecked;


                if (childNode.ChildNodes.Count > 0)
                {
                    this.checkChildNodes(childNode.ChildNodes, isChecked);
                }
            }
        }

        protected void tree_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            foreach (TreeNode childNode in e.Node.ChildNodes)
            {
                childNode.Checked = e.Node.Checked;

                if (childNode.ChildNodes.Count > 0)
                {
                    this.checkChildNodes(childNode.ChildNodes, e.Node.Checked);
                }
            }
        }

        protected void tree_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
        }
    }
}
