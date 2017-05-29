<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsSnapshots.ascx.cs" Inherits="SolidCP.Portal.VPSForPC.VpsDetailsSnapshots" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="Snapshot48" runat="server" />
				    <scp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Snapshots" />
			    </div>
			    <div class="panel-body form-horizontal">
                    <scp:Menu id="menu" runat="server" SelectedItem="" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_snapshots" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
				    
				    <table style="width:100%;">
				        <tr>
				            <td valign="top">
				            
                                <div class="FormButtonsBarClean">
                                    <CPCC:StyleButton id="btnTakeSnapshot" CssClass="btn btn-success" runat="server" onclick="btnTakeSnapshot_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnTakeSnapshotText"/> </CPCC:StyleButton>
                                </div>
                                <br />
                                
				                <asp:TreeView ID="SnapshotsTree" runat="server" 
                                    onselectednodechanged="SnapshotsTree_SelectedNodeChanged" ShowLines="True">
                                    <SelectedNodeStyle CssClass="SelectedTreeNode" />
                                    <Nodes>
                                    </Nodes>
                                    <NodeStyle CssClass="TreeNode" />
				                </asp:TreeView>
				                
				                <div id="NoSnapshotsPanel" runat="server" style="padding: 5px;">
				                    <asp:Localize ID="locNoSnapshots" runat="server" meta:resourcekey="locNoSnapshots" Text="No snapshots"></asp:Localize>
				                </div>
                                
                                <br />
				                <br />
				                <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota"
				                    Text="Number of Snapshots:"></asp:Localize>
				                &nbsp;&nbsp;&nbsp;
				                <scp:QuotaViewer ID="snapshotsQuota" runat="server" QuotaTypeId="2" />
				    
				            </td>
				            <td valign="top" id="SnapshotDetailsPanel" runat="server">
				                <asp:Image ID="imgThumbnail" runat="server" Width="160" Height="120" />
				                <p>
				                    <asp:Localize ID="locCreated" runat="server" meta:resourcekey="locCreated"
				                        Text="Created:"></asp:Localize>
				                    <asp:Literal ID="litCreated" runat="server"></asp:Literal>
				                </p>
				                <ul class="ActionButtons">
				                    <li>
				                        <CPCC:StyleButton ID="btnApply" runat="server" CausesValidation="false" CssClass="ActionButtonApplySnapshot"
				                            meta:resourcekey="btnApply" Text="Apply" onclick="btnApply_Click"></CPCC:StyleButton>
				                    </li>
				                    <li>
				                        <CPCC:StyleButton ID="btnRename" runat="server" CausesValidation="false" CssClass="ActionButtonRename"
				                            meta:resourcekey="btnRename" Text="Rename"></CPCC:StyleButton>
				                    </li>
				                    <li>
				                        <CPCC:StyleButton ID="btnDelete" runat="server" CausesValidation="false" CssClass="ActionButtonDeleteSnapshot"
				                            meta:resourcekey="btnDelete" Text="Delete" onclick="btnDelete_Click"></CPCC:StyleButton>
				                    </li>
				                    <li>
				                        <CPCC:StyleButton ID="btnDeleteSubtree" runat="server" CausesValidation="false" CssClass="ActionButtonDeleteSnapshotTree"
				                            meta:resourcekey="btnDeleteSubtree" Text="Delete subtree" 
                                            onclick="btnDeleteSubtree_Click"></CPCC:StyleButton>
				                    </li>
				                </ul>
				            </td>
				        </tr>
				    </table>
				    
			    </div>
		    </div>
            </div>
    </div>
		    <div class="Right">
			    <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
		    </div>

<asp:Panel ID="RenamePanel" runat="server" style="display:none;">
	 <div class="widget">
             <div class="widget-header clearfix">
                           <h3><i class="fa fa-i-cursor"></i>  <asp:Localize ID="locRenameSnapshot" runat="server" Text="Rename snapshot" meta:resourcekey="locRenameSnapshot"></asp:Localize></h3>
			 </div>
                    <div class="widget-content Popup">
			<table cellspacing="10">
			    <tr>
			        <td>
			            <asp:TextBox ID="txtSnapshotName" runat="server" CssClass="NormalTextBox" Width="300"></asp:TextBox>
			            
			            <asp:RequiredFieldValidator ID="SnapshotNameValidator" runat="server" Text="*" Display="Dynamic"
                                ControlToValidate="txtSnapshotName" meta:resourcekey="SnapshotNameValidator" SetFocusOnError="true"
                                ValidationGroup="RenameSnapshot">*</asp:RequiredFieldValidator>
			        </td>
			    </tr>
			</table>                      
			</div>
					<div class="popup-buttons text-right">
		    <CPCC:StyleButton id="btnCancelRename" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelRenameText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnRenameSnapshot" CssClass="btn btn-success" runat="server" OnClick="btnRenameSnapshot_Click" ValidationGroup="RenameSnapshot"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRenameSnapshotText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="RenameSnapshotModal" runat="server" BehaviorID="RenameSnapshotModal"
	TargetControlID="btnRename" PopupControlID="RenamePanel"
	BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelRename" />