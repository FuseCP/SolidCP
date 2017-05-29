<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSEditCollectionApps.ascx.cs" Inherits="SolidCP.Portal.RDS.RDSEditCollectionApps" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/RDSCollectionApps.ascx" TagName="CollectionApps" TagPrefix="scp"%>
<%@ Register Src="UserControls/RDSCollectionTabs.ascx" TagName="CollectionTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
					<asp:Image ID="imgEditRDSCollection" SkinID="EnterpriseRDSCollections48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit RDS Collection"></asp:Localize>
                    -
					<asp:Literal ID="litCollectionName" runat="server" Text="" />
				</div>
				<div class="panel-body form-horizontal">
                    <scp:CollectionTabs id="tabs" runat="server" SelectedTab="rds_collection_edit_apps" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />                    

                    <scp:CollapsiblePanel id="secRdsApplications" runat="server"
                        TargetControlID="panelRdsApplications" meta:resourcekey="secRdsApplications" Text="">
                    </scp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelRdsApplications">                                                
                        <div style="padding: 10px;">
                            <scp:CollectionApps id="remoreApps" runat="server" />
                        </div>                            
                    </asp:Panel>
                
				</div>
                            <div class="text-right">
                        <scp:ItemButtonPanel id="buttonPanel" runat="server" ValidationGroup="SaveRDSCollection" OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
			        </div>   
                </div>
                    </div>
