<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileLookup.ascx.cs" Inherits="SolidCP.Portal.FileLookup" %>
<asp:TextBox ID="txtFile" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
<asp:RequiredFieldValidator ID="valRequireFile" runat="server" meta:resourcekey="valRequireFile" ControlToValidate="txtFile"
    ErrorMessage="*"></asp:RequiredFieldValidator>
<br />
<asp:Panel ID="pnlLookup" runat="server" class="Toolbox" style="display: none;">
<div style="width: auto; height: 175px; overflow: auto; white-space: nowrap; background-color: #ffffff;padding:3px;border:solid 1px #909090;">
    <div style="float:right;">
	    <asp:UpdateProgress ID="treeProgress" runat="server"
	        AssociatedUpdatePanelID="TreeUpdatePanel" DynamicLayout="true">
	        <ProgressTemplate>
	            <asp:Image ID="imgIndicator" runat="server" SkinID="AjaxIndicator" />
	        </ProgressTemplate>
	    </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="TreeUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
		    <asp:treeview runat="server" id="DNNTree" populatenodesfromclient="true"
			    showexpandcollapse="true" expanddepth="0" ontreenodepopulate="DNNTree_TreeNodePopulate"
			    onselectednodechanged="DNNTree_SelectedNodeChanged" NodeIndent="10">
			    <rootnodestyle cssclass="FileManagerTreeNode" />
			    <nodestyle cssclass="FileManagerTreeNode" />
			    <leafnodestyle cssclass="FileManagerTreeNode" />
			    <parentnodestyle cssclass="FileManagerTreeNode" />
			    <selectednodestyle cssclass="FileManagerTreeNodeSelected" />
		    </asp:treeview>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Panel>

<ajaxToolkit:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtFile" PopupControlID="pnlLookup" Position="Bottom" />
<ajaxToolkit:DropShadowExtender  ID="DropShadowExtender1" runat="server" TargetControlID="pnlLookup" TrackPosition="true" Opacity="0.4" />