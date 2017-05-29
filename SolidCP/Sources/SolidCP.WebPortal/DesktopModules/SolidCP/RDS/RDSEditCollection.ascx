<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSEditCollection.ascx.cs" Inherits="SolidCP.Portal.RDS.RDSEditCollection" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/RDSCollectionServers.ascx" TagName="CollectionServers" TagPrefix="scp"%>
<%@ Register Src="UserControls/RDSCollectionTabs.ascx" TagName="CollectionTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
					<asp:Image ID="imgAddRDSServer" SkinID="EnterpriseRDSCollections48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Add Server To Organization"></asp:Localize>
                    -
					<asp:Literal ID="litCollectionName" runat="server" Text="" />
				</div>
				<div class="panel-body form-horizontal">			    
                    <scp:CollectionTabs id="tabs" runat="server" SelectedTab="rds_edit_collection" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <scp:CollapsiblePanel id="secRdsServers" runat="server"
                        TargetControlID="panelRdsServers" meta:resourcekey="secRdsServers" Text="">
                    </scp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelRdsServers">                                                
                        <div style="padding: 10px;">
                            <scp:CollectionServers id="servers" runat="server" />
                        </div>                             
                    </asp:Panel>
               
				</div>
                    
                    <div class="text-right">
                        <scp:ItemButtonPanel id="buttonPanel" runat="server" ValidationGroup="SaveRDSCollection" 
                            OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
			        </div>     </div>    </div>
