<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSSetupLetter.ascx.cs" Inherits="SolidCP.Portal.RDS.RDSSeupLetter" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/RDSCollectionTabs.ascx" TagName="CollectionTabs" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-heading">
					<asp:Image ID="imgEditRDSCollection" SkinID="EnterpriseRDSCollections48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Setup Instructions"></asp:Localize>                    
                </div>
				<div class="panel-body form-horizontal">     
                    <scp:CollectionTabs id="tabs" runat="server" SelectedTab="rds_collection_edit_users" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">  
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    <scp:CollapsiblePanel id="secEmail" runat="server" IsCollapsed="true"
                        TargetControlID="EmailPanel" meta:resourcekey="secEmail" Text="Send via E-Mail">
                    </scp:CollapsiblePanel>
	                <asp:Panel ID="EmailPanel" runat="server" Height="0" style="overflow:hidden;">
                        <table id="tblEmail" runat="server" cellpadding="2">
                            <tr>
                                <td class="SubHead" width="30" nowrap>
                                    <asp:Label ID="lblTo" runat="server" meta:resourcekey="lblTo" Text="To:"></asp:Label>
                                </td>
                                <td class="Normal">
                                    <asp:TextBox ID="txtTo" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valRequireEmail" runat="server" ControlToValidate="txtTo" Display="Dynamic"
                                        ErrorMessage="Enter e-mail" ValidationGroup="SendEmail" meta:resourcekey="valRequireEmail"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="SubHead">
                                    <asp:Label ID="lblCC" runat="server" meta:resourcekey="lblCC" Text="CC:"></asp:Label>
                                </td>
                                <td class="Normal">
                                    <asp:TextBox ID="txtCC" runat="server" CssClass="NormalTextBox" Width="300px"></asp:TextBox></td>
                            </tr>                            
                        </table>
                        <div class="panel-footer text-right">
                        <<CPCC:StyleButton id="btnExit" CssClass="btn btn-danger" runat="server" OnClick="btnExit_Click" OnClientClick="ShowProgressDialog('Loading ...');"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnExitText"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btnSend" CssClass="btn btn-success" runat="server" OnClick="btnSend_Click" ValidationGroup="SendEmail"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSendtext"/> </CPCC:StyleButton>
			        </div>
                    </asp:Panel>
					
                    <div class="PreviewArea">
                        <asp:Literal ID="litContent" runat="server"></asp:Literal>
                    </div>										
				</div>
			</div>
		</div>