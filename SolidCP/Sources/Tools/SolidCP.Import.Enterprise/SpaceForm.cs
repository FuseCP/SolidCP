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
using SolidCP.EnterpriseServer;


namespace SolidCP.Import.Enterprise
{
	public partial class SpaceForm : BaseForm
	{
		public const string USER = "User";
		public const string CUSTOMERS = "Customers";
		public const string SPACES = "Spaces";
		public const string SPACE = "Space";
		public const string TMP = "Tmp";

		private string username;
		private string password;

		public SpaceForm()
		{
			InitializeComponent();
			
		}

		public void InitializeForm(string username, string password)
		{
			this.username = username;
			this.password = password;
			PopulateRootNode(username, password);
		}

		private void PopulateRootNode(string username, string password)
		{
			spaceTree.Nodes.Clear();
			UserInfo info = null;
			try
			{
				info = UserController.GetUser(username);
				SecurityContext.SetThreadPrincipal(info);
			}
			catch (Exception ex)
			{
				ShowError("Unable to load user.", ex);
			}
			if (info != null)
			{
				DataNode rootNode = AddTreeNode(null, info.Username, 0, USER, info, true);
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
			spaceTree.Update();

			switch (node.NodeType)
			{
				case USER:
					PopulateUser(node);
					break;
				case CUSTOMERS:
					PopulateCustomers(node);
					break;
				case SPACES:
					PopulateSpaces(node);
					break;
			}

			node.Populated = true;
			node.Expand();
		}

		private void PopulateSpaces(DataNode parentNode)
		{
			UserInfo info = parentNode.Tag as UserInfo;
			DataSet ds = null;
			try
			{
				ds = PackageController.GetRawMyPackages(info.UserId);
			}
			catch(Exception ex)
			{
				ShowError("Unable to load spaces.", ex);
			}
			if (ds != null)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					PackageInfo data = new PackageInfo();
					data.PackageId = Utils.GetDbInt32(dr["PackageId"]);
					data.PackageName = Utils.GetDbString(dr["PackageName"]);
					data.UserId = Utils.GetDbInt32(dr["UserId"]);
					AddTreeNode(parentNode, data.PackageName, 3, SPACE, data, false);
				}
			}

		}

		private void PopulateCustomers(DataNode parentNode)
		{
			UserInfo info = parentNode.Tag as UserInfo;
			DataSet ds = null;
			try
			{
				ds = UserController.GetRawUsers(info.UserId, false);
			}
			catch(Exception ex)
			{
				ShowError("Unable to load users.", ex);
			}
			if (ds != null)
			{

				foreach (DataRow dr in ds.Tables[0].Rows)
				{

					UserInfo user = new UserInfo();
					user.UserId = Utils.GetDbInt32(dr["UserId"]);
					user.Username = Utils.GetDbString(dr["Username"]);
					user.RoleId = Utils.GetDbInt32(dr["RoleId"]);
					AddTreeNode(parentNode, user.Username, 0, USER, user, true);
				}
			}

		}

		private void PopulateUser(DataNode parentNode)
		{
			UserInfo info = parentNode.Tag as UserInfo;
			if ((UserRole)info.RoleId != UserRole.User)
			{
				AddTreeNode(parentNode, CUSTOMERS, 1, CUSTOMERS, info, true);
			}
			AddTreeNode(parentNode, SPACES, 2, SPACES, info, true); 
		}

		private DataNode AddTreeNode(DataNode parentNode, string text, int imageIndex, string nodeType, object data, bool hasChildren)
		{
			DataNode node = new DataNode();
			node.Text = text;
			node.SelectedImageIndex = imageIndex;
			node.ImageIndex = imageIndex;
			node.NodeType = nodeType;
			node.Tag = data;
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
				spaceTree.Nodes.Add(node);
			return node;
		}

		private void OnSelectClick(object sender, EventArgs e)
		{
			DataNode node = spaceTree.SelectedNode as DataNode;
			if (node == null || node.NodeType != SPACE)
			{
				ShowWarning("Please select hosting space.");
				return;
			}
			PackageInfo data = node.Tag as PackageInfo;
			if (data == null || data.PackageId == 0 || data.PackageId == 1)
			{
				ShowWarning("Invalid hosting space. Please select hosting space with allowed Exchange organizations.");
				return;
			}

			PackageContext cntx = null;
			try
			{
				cntx = PackageController.GetPackageContext(data.PackageId);
			}
			catch (Exception ex)
			{
				ShowError("Unable to load space data", ex);
				return;
			}

			if (cntx == null)
			{
				ShowWarning("Invalid hosting space. Please select hosting space with allowed Exchange organizations.");
				return;
			}

			bool exchangeEnabled = false;
			bool orgEnabled = false;
			
			foreach (HostingPlanGroupInfo group in cntx.GroupsArray)
			{
				if (!group.Enabled)
					continue;
				if (group.GroupName == ResourceGroups.Exchange)
				{
					exchangeEnabled = true;
					continue;
				}
				else if (group.GroupName == ResourceGroups.HostedOrganizations)
				{
					orgEnabled = true;
					continue;
				}
			}
			if (!exchangeEnabled || !orgEnabled)
			{
				ShowWarning("Invalid hosting space. Please select hosting space with allowed Exchange organizations.");
				return;
			}
			this.selectedSpace = data;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private PackageInfo selectedSpace;

		public PackageInfo SelectedSpace
		{
			get { return selectedSpace; }
		}
	
	}
}
