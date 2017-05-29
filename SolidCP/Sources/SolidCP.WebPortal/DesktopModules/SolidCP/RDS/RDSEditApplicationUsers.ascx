<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSEditApplicationUsers.ascx.cs" Inherits="SolidCP.Portal.RDS.RDSEditApplicationUsers" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/RDSCollectionUsers.ascx" TagName="CollectionUsers" TagPrefix="scp"%>
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
				    <scp:SimpleMessageBox id="SimpleMessageBox1" runat="server" />
                    <scp:CollapsiblePanel id="secRdsApplicationEdit" runat="server"
                        TargetControlID="panelRdsApplicationEdit" meta:resourcekey="secRdsApplicationEdit" Text="">
                    </scp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelRdsApplicationEdit">                                                
                        <div style="padding: 10px;">
                            <table>
                                <tr>
                                    <td class="FormLabel150" colspan="2" style="white-space:nowrap;">
                                        <asp:Localize ID="locLblApplicationName" runat="server" meta:resourcekey="locLblApplicationName" Text="Application Name"/>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtApplicationName" runat="server" CssClass="TextBox300" />
                                        <asp:RequiredFieldValidator ID="valApplicationName" runat="server" ErrorMessage="*" ControlToValidate="txtApplicationName" ValidationGroup="SaveRDSCollection"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:RadioButton ID="chNotAllow" GroupName="commandLineParameters" meta:resourcekey="chNotAllow" runat="server" Text=""/>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:RadioButton ID="chAllowAny" GroupName="commandLineParameters" meta:resourcekey="chAllowAny" runat="server" Text=""/>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td  style="width:40px;"/>
                                    <td colspan="2">
                                        <asp:Localize ID="locAllowAny" runat="server" meta:resourcekey="locAllowAny" Text=""/>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:RadioButton ID="chAllow" meta:resourcekey="chAllow" GroupName="commandLineParameters" runat="server" Text=""/>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="FormLabel150" colspan="2" style="white-space:nowrap;">
                                        <asp:Localize ID="locCommandLine" runat="server" meta:resourcekey="locCommandLine" Text="Command-line parameters"/>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCommandLine" runat="server" CssClass="TextBox300" Enabled="False" />                                        
                                    </td>
                                </tr>
                            </table>
                        </div>                            
                    </asp:Panel>
                    					
                    <scp:CollapsiblePanel id="secRdsApplicationUsers" runat="server"
                        TargetControlID="panelRdsApplicationUsers" meta:resourcekey="secRdsApplicationUsers" Text="">
                    </scp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelRdsApplicationUsers">                                                
                        <div style="padding: 10px;">
                            <scp:CollectionUsers id="users" runat="server" />
                        </div>                            
                    </asp:Panel>
                    <div class="text-right">
                        <CPCC:StyleButton id="btnExit" CssClass="btn btn-warning" runat="server" OnClick="btnExit_Click" OnClientClick="ShowProgressDialog('Loading ...');"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnExitText"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btnSaveExit" CssClass="btn btn-success" runat="server" OnClick="btnSaveExit_Click" OnClientClick="ShowProgressDialog('Updating ...');"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveExitText"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" OnClientClick="ShowProgressDialog('Updating ...');"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>
			        </div>
				</div>	
			</div>
		</div>