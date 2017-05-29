using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Providers.OS;
using SolidCP.Providers.StorageSpaces;

namespace SolidCP.Portal.StorageSpaces
{
    public partial class EditStorageSpace : SolidCPModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var services = ES.Services.Servers.GetRawServicesByGroupId(EnterpriseServer.ServiceGroupIds.StorageSpace);

                ddlStorageService.DataSource = services.Tables[0];
                ddlStorageService.DataTextField = "ServiceName";
                ddlStorageService.DataValueField = "ServiceID";
                ddlStorageService.DataBind();

                var levels = ES.Services.StorageSpaces.GetStorageSpaceLevelsPaged(string.Empty, string.Empty, string.Empty, 0, int.MaxValue);

                foreach (var level in levels.Levels)
                {
                    ddlSsLevel.Items.Add(new ListItem(level.Name, level.Id.ToString()));
                }

                string path = string.Empty;
                var storage = ES.Services.StorageSpaces.GetStorageSpaceById(PanelRequest.StorageSpaceId);

                if (storage != null)
                {
                    txtName.Text = storage.Name;
                    txtStorageSize.Text = ConvertBytesToGB(storage.FsrmQuotaSizeBytes).ToString();
                    chkIsDisabled.Checked = storage.IsDisabled;

                    path = storage.Path;

                    switch (storage.FsrmQuotaType)
                    {
                        case QuotaType.Hard:
                            rbtnQuotaHard.Checked = true;
                            break;
                        case QuotaType.Soft:
                            rbtnQuotaSoft.Checked = true;
                            break;
                    }

                    ddlStorageService.SelectedValue = storage.ServiceId.ToString();
                    ddlStorageService.Enabled = false;
                    ddlSsLevel.SelectedValue = storage.LevelId.ToString();
                }

                var serviceId = Utils.ParseInt(ddlStorageService.SelectedValue);

                if (serviceId > 0)
                {
                    RefreshTreeView(serviceId, path);

                    FoldersTree.Enabled = PanelRequest.StorageSpaceId < 1;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            RenderValidationJavaScript();
        }

        private void RenderValidationJavaScript()
        {
            if (!Page.ClientScript.IsClientScriptIncludeRegistered("wspStorageSpaceFunctions"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "wspStorageSpaceFunctions", @"
                            
                    function ClientValidateTreeView(source, arguments) {
                    var treeView = document.getElementById('"+FoldersTree.ClientID +@"');
                    var checkBoxes = treeView.getElementsByTagName('input');
                    var checkedCount = 0;
                    for (var i = 0; i < checkBoxes.length; i++) {
                      if (checkBoxes[i].checked) {
                        checkedCount++;
                      }
                    }
                    if (checkedCount > 0) {
                      arguments.IsValid = true;
                    } else {
                      arguments.IsValid = false;
                    }
                  } 

                    function client_OnTreeNodeChecked(event)
                    {
                     var treeNode = event.srcElement || event.target ;
                     if (treeNode.tagName == 'INPUT' && treeNode.type == 'checkbox')
                      {
                       if(treeNode.checked)
                        {
                         uncheckOthers(treeNode.id);
                        }
                      }
                    }

                    function uncheckOthers(id)
                     {
                      var elements = document.getElementsByTagName('input');
                      // loop through all input elements in form
                      for(var i = 0; i < elements.length; i++)
                       {
                        if(elements.item(i).type == 'checkbox')
                        {
                         if(elements.item(i).id!=id)
                         {
                          elements.item(i).checked=false;
                         }
                        }
                       }
                      }

                ", true);

                FoldersTree.Attributes.Add("OnClick", "client_OnTreeNodeChecked(event)");
            }

        }

        private void RefreshTreeView(int serviceId, string path = null)
        {
            BuildFoldersTree(serviceId);

            OpenAndCheckFolder(path);
        }

        private void BuildFoldersTree(int serviceId)
        {
            FoldersTree.Nodes.Clear();

            // get organization info
            var rootDrives = ES.Services.StorageSpaces.GetDriveLetters(serviceId);

            // add folders to the tree
            foreach (SystemFile drive in rootDrives)
            {
                string[] path = drive.Name.Split(new[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);

                var node = CreateNode(path[0], drive.Name, false, false, true);

                 FoldersTree.Nodes.Add(node);
            }
        }

        private void OpenAndCheckFolder(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return;
            }

            string[] path = fullPath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            TreeNodeCollection nodes = FoldersTree.Nodes;

            int i = 0;

            while (i < path.Length)
            {
                foreach (TreeNode childNode in nodes)
                {
                    if (String.Compare(childNode.Value, fullPath, StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        childNode.Checked = true;

                        return;
                    }

                    if (String.Compare(childNode.Text, path[i], StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        childNode.Expand();
                        nodes = childNode.ChildNodes;

                        break;
                    }
                }

                i++;
            }
        }

        protected void FoldersTree_OnTreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            TreeNode subHeadingNode = e.Node;
            string path = subHeadingNode.Value;

            var folders = ES.Services.StorageSpaces.GetSystemSubFolders(Utils.ParseInt(ddlStorageService.SelectedValue), path);

            foreach (var folder in folders)
            {
                var newNode = CreateNode(folder.Name.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last(), folder.Name, false, true, true);

                subHeadingNode.ChildNodes.Add(newNode);
            }
        }

        private TreeNode CreateNode(string name, string value, bool expanded, bool showCheckBox,
            bool onDemand)
        {
            var node = new TreeNode(name, value);
            node.Expanded = expanded;
            node.ShowCheckBox = showCheckBox;
            node.PopulateOnDemand = onDemand;
            node.ImageUrl = GetThemedImage("FileManager/OpenFolder.gif");
            node.SelectAction = TreeNodeSelectAction.None;

            return node;
        }

        private bool SaveStorageSpace(out int storageId)
        {
            StorageSpace storage = ES.Services.StorageSpaces.GetStorageSpaceById(PanelRequest.StorageSpaceId)
                                      ?? new StorageSpace();

            storage.Id = PanelRequest.StorageSpaceId;
            storage.Name = txtName.Text;
            storage.LevelId = Utils.ParseInt(ddlSsLevel.SelectedValue);

            if (PanelRequest.StorageSpaceId < 1)
            {
                storage.ServiceId = Utils.ParseInt(ddlStorageService.SelectedValue);
                storage.Path = GetCheckedNodeValue(FoldersTree.Nodes);
            }

            var serviceInfo = ES.Services.Servers.GetServiceInfo(storage.ServiceId);

            storage.ServerId = serviceInfo.ServerId;

            storage.FsrmQuotaType = rbtnQuotaSoft.Checked ? QuotaType.Soft : QuotaType.Hard;
            storage.FsrmQuotaSizeBytes = (long)(decimal.Parse(txtStorageSize.Text)*1024*1024*1024);

            storage.IsDisabled = chkIsDisabled.Checked;

            var result = ES.Services.StorageSpaces.SaveStorageSpace(storage);

            storageId = result.Value;

            messageBox.ShowMessage(result, "STORAGE_SPACE_SAVE", null);

            return result.IsSuccess;
        }

        private string GetCheckedNodeValue(TreeNodeCollection nodes)
        {
            if (nodes != null)
            {
                foreach (TreeNode node in nodes)
                {
                    if (node.Checked)
                    {
                        return node.Value;
                    }

                    var childResult = GetCheckedNodeValue(node.ChildNodes);

                    if (!string.IsNullOrEmpty(childResult))
                    {
                        return childResult;
                    }
                }
            }

            return string.Empty;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int spaceId;
            if (SaveStorageSpace(out spaceId) && PanelRequest.StorageSpaceId <= 0)
            {
                EditStorageSpaceRedirect(spaceId);
            }
        }

        protected void btnSaveExit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int spaceId;
            if (SaveStorageSpace(out spaceId))
            {
                Response.Redirect(EditUrl(null));
            }
        }

        private void EditStorageSpaceRedirect(int id)
        {
            Response.Redirect(EditUrl("StorageSpaceId", id.ToString(), "edit_storage_space"));
        }

        protected decimal ConvertBytesToGB(object size)
        {
            return Math.Round(Convert.ToDecimal(size) / (1024 * 1024 * 1024), 2);
        }

        protected void valRequireFolder_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrEmpty(GetCheckedNodeValue(FoldersTree.Nodes));
        }

        protected void ddlStorageService_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var serviceId = Utils.ParseInt(ddlStorageService.SelectedValue);

            if (serviceId > 0)
            {
                RefreshTreeView(serviceId);
            }
        }

        protected bool CheckStorageIsInUse(int storageId)
        {
            return ES.Services.StorageSpaces.GetStorageSpaceFoldersByStorageSpaceId(storageId).Any();
        }

        protected void valPathIsInUseFolder_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var service = ES.Services.Servers .GetServiceInfo(Utils.ParseInt(ddlStorageService.SelectedValue));
            var path = GetCheckedNodeValue(FoldersTree.Nodes);

            args.IsValid = !ES.Services.StorageSpaces.CheckIsStorageSpacePathInUse(service.ServerId, path, PanelRequest.StorageSpaceId);
        }
    }
}
