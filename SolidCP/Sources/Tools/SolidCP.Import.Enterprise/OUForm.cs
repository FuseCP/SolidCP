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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.DirectoryServices;

namespace SolidCP.Import.Enterprise
{
	public partial class OUForm : BaseForm
	{
		public const string OU = "organizationalUnit";
		public const string CONTAINER = "container";
		public const string COMPUTER = "computer";
		public const string USER = "user";
		public const string CONTACT = "contact";
		public const string GROUP = "group";
		public const string TMP = "Tmp";

		public OUForm()
		{
			InitializeComponent();
			
		}

		public void InitializeForm()
		{
			PopulateRootNode();
		}

		private void PopulateRootNode()
		{
			ouTree.Nodes.Clear();
			DirectoryEntry root = null;
			try
			{
				root = ADUtils.GetRootOU();
			}
			catch (Exception ex)
			{
				ShowError("Unable to load root OU.", ex);
			}
			if (root != null)
			{
				DataNode rootNode = AddTreeNode(null, root);
				rootNode.Expand();
			}
		}

		private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			DataNode node = e.Node as DataNode;
			ExpandNode(node);
		}

		private void ExpandNode(DataNode node)
		{
			if (node == null || node.Populated)
			   return;

			node.Nodes.Clear();
			ouTree.Update();
			try
			{
				DirectoryEntry parent = node.Tag as DirectoryEntry;

				foreach (DirectoryEntry child in parent.Children)
				{
					AddTreeNode(node, child);
				}
			}
			catch (Exception ex)
			{
				ShowError("Unable to load Active Directory data.", ex);
			}

			node.Populated = true;
			node.Expand();
		}

		private DataNode AddTreeNode(DataNode parentNode, DirectoryEntry entry)
		{
			bool hasChildren = true;
			DataNode node = new DataNode();
			node.Text = (string)entry.Properties["name"].Value;
			
			node.NodeType = entry.SchemaClassName;
			int imageIndex = 2;
			switch (entry.SchemaClassName)
			{
				case OU:
					imageIndex = 1;
					break;
				case CONTAINER:
					imageIndex = 2;
					break;
				case COMPUTER:
					imageIndex = 3;
					break;
				case USER:
					imageIndex = 4;
					hasChildren = false;
					break;
				case CONTACT:
					imageIndex = 8;
					hasChildren = false;
					break;
				case GROUP:
					imageIndex = 5;
					hasChildren = false;
					break;
				default:
					imageIndex = 6;
					break;
			}
			
			node.SelectedImageIndex = imageIndex;
			node.ImageIndex = imageIndex;
			node.Tag = entry;
			if (hasChildren)
			{
				node.Populated = false;
				DataNode tmpNode = new DataNode();
				tmpNode.Text = "Expanding...";
				tmpNode.SelectedImageIndex = 2;
				tmpNode.ImageIndex = 2;
				tmpNode.NodeType = TMP;
				node.Nodes.Add(tmpNode);
			}
			else
			{
				node.Populated = true;
			}
			if (parentNode != null)
				parentNode.Nodes.Add(node);
			else
				ouTree.Nodes.Add(node);
			return node;
		}

		private void OnSelectClick(object sender, EventArgs e)
		{
			DataNode node = ouTree.SelectedNode as DataNode;
			if (node == null || node.NodeType != OU)
			{
				ShowWarning("Please select Organizational Unit.");
				return;
			}

			this.directoryEntry = (DirectoryEntry)node.Tag;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private DirectoryEntry directoryEntry;

		public DirectoryEntry DirectoryEntry
		{
			get { return directoryEntry; }
		}



	}
}
