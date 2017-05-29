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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;

//using DNNTV = DotNetNuke.UI.WebControls;
using SolidCP.Providers.OS;

namespace SolidCP.Portal
{
    public partial class FileLookup : SolidCPControlBase
    {

        public bool Enabled
        {
            get { return txtFile.Enabled; }
            set { txtFile.Enabled = value; }
        }

        public int PackageId
        {
            get { return (ViewState["PackageId"] != null) ? (int)ViewState["PackageId"] : PanelSecurity.PackageId; }
            set { ViewState["PackageId"] = value; InitTree(); }
        }

        public bool IncludeFiles
        {
            get { return (ViewState["IncludeFiles"] != null) ? (bool)ViewState["IncludeFiles"] : false; }
            set { ViewState["IncludeFiles"] = value; }
        }

        public string RootFolder
        {
            get { return (ViewState["RootFolder"] != null) ? (string)ViewState["RootFolder"] : ""; }
            set { ViewState["RootFolder"] = value; }
        }

        public string SelectedFile
        {
            get { return txtFile.Text; }
            set { txtFile.Text = value; }
        }

        public Unit Width
        {
            get { return txtFile.Width; }
            set { txtFile.Width = value; pnlLookup.Width = value; }
        }

        public string ValidationGroup
        {
            get { return valRequireFile.ValidationGroup; }
            set { valRequireFile.ValidationGroup = value; }
        }

        public bool ValidationEnabled
        {
            get { return valRequireFile.Enabled; }
            set { valRequireFile.Enabled = value; }
        }

        public bool DropShadow
        {
            get { return DropShadowExtender1.Opacity > 0.0F; }
            set { DropShadowExtender1.Opacity = value ? 0.6F : 0.0F; }
        }

        private bool treeInitialized = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(!treeInitialized)
                    InitTree();
            }
        }

        private void InitTree()
        {
            pnlLookup.Width = Width;

            // prepare tree
            DNNTree.CollapseImageUrl = ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/images/min.gif"));
            DNNTree.ExpandImageUrl = ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/images/max.gif"));
            DNNTree.NoExpandImageUrl = ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/images/empty.gif"));

            DNNTree.Nodes.Clear();

            TreeNode node = new TreeNode();
            node.ImageUrl = ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/images/folder.png"));
            node.Value = PackageId.ToString() + "," + RootFolder + "\\";
            node.Text = GetLocalizedString("Text.Root");
            node.PopulateOnDemand = true;
            DNNTree.Nodes.Add(node);

            // set flag
            treeInitialized = true;
        }

		protected void DNNTree_SelectedNodeChanged(object sender, EventArgs e)
		{
			if (DNNTree.SelectedNode != null)
			{
				string[] key = DNNTree.SelectedNode.Value.Split(',');
				string path = key[1];

				if (path.Length > 1 && path.EndsWith("\\"))
					path = path.Substring(0, path.Length - 1);

				path = path.Substring(RootFolder.Length);

                if (!String.IsNullOrEmpty(RootFolder) &&
                    !path.StartsWith("\\"))
                    path = "\\" + path;

                PopupControlExtender1.Commit(path);
			}
		}

		protected void DNNTree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
		{
			if (e.Node.ChildNodes.Count > 0)
				return;

			string[] key = e.Node.Value.Split(',');

			int packageId = Utils.ParseInt(key[0], 0);
			string path = key[1];

			// read child folders
            SystemFile[] files = null;

            try
            {
                files = ES.Services.Files.GetFiles(packageId, path, IncludeFiles);
            }
            catch (Exception ex)
            {
                // add error node
                TreeNode node = new TreeNode();
                node.Text = "Error: " + ex.Message;
                e.Node.ChildNodes.Add(node);
                return;
            }

			foreach (SystemFile file in files)
			{
				string fullPath = path + file.Name;
				if (file.IsDirectory)
					fullPath += "\\";

				TreeNode node = new TreeNode();
				node.Value = packageId.ToString() + "," + fullPath;
				node.Text = file.Name;
				node.PopulateOnDemand = (file.IsDirectory && !file.IsEmpty);
				
				node.ImageUrl = file.IsDirectory? ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/images/folder.png")) : 
					ResolveUrl(String.Concat("~/App_Themes/", Page.Theme, "/images/file.png"));

				e.Node.ChildNodes.Add(node);
			}
		}
    }
}
