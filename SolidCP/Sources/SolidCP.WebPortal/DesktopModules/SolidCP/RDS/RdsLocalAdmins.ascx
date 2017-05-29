<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSLocalAdmins.ascx.cs" Inherits="SolidCP.Portal.RDS.RdsLocalAdmins" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/RDSCollectionTabs.ascx" TagName="CollectionTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>
<%@ Register Src="UserControls/RDSCollectionUsers.ascx" TagName="CollectionUsers" TagPrefix="scp"%>
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
					<asp:Image ID="imgEditRDSCollection" SkinID="EnterpriseRDSCollections48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit RDS Collection"></asp:Localize>
                    -
					<asp:Literal ID="litCollectionName" runat="server" Text="" />
				</div>
				<div class="panel-body form-horizontal">
                <scp:CollectionTabs id="tabs" runat="server" SelectedTab="rds_collection_local_admins" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <div class="widget-content tab-content"> 
                    <scp:CollapsiblePanel id="secRdsLocalAdmins" runat="server"
                        TargetControlID="panelRdsLocalAdmins" meta:resourcekey="secRdsLocalAdmins" Text="">
                    </scp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelRdsLocalAdmins">                                                
                        <div style="padding: 10px;">
                            <scp:CollectionUsers id="users" runat="server" />
                        </div>                            
                    </asp:Panel>
				</div>
                    <div class="text-right">
                        <scp:ItemButtonPanel id="buttonPanel" runat="server" ValidationGroup="SaveRDSCollection" 
                            OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
			        </div>
			</div>
            </div>
                    </div>