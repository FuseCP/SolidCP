<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsHelp.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VpsDetailsHelp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_help" />	
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <scp:CollapsiblePanel id="secEmail" runat="server" IsCollapsed="true"
                        TargetControlID="EmailPanel" meta:resourcekey="secEmail" Text="Send instructions by E-Mail">
                    </scp:CollapsiblePanel>
	                <asp:Panel ID="EmailPanel" runat="server" Height="0" style="overflow:hidden;">
                        <table id="tblEmail" runat="server" cellpadding="2">
                            <tr>
                                <td class="SubHead" width="30" nowrap>
                                    <asp:Label ID="lblTo" runat="server" meta:resourcekey="lblTo" Text="To:"></asp:Label>
                                </td>
                                <td class="Normal">
                                    <asp:TextBox ID="txtTo" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="valRequireEmail" runat="server" ControlToValidate="txtTo" Display="Dynamic"
                                        ErrorMessage="Enter e-mail" ValidationGroup="SendEmail" meta:resourcekey="valRequireEmail"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="SubHead">
                                    <asp:Label ID="lblBCC" runat="server" meta:resourcekey="lblBCC" Text="BCC:"></asp:Label>
                                </td>
                                <td class="Normal">
                                    <asp:TextBox ID="txtBCC" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSend" runat="server" CssClass="btn btn-success"
                                        meta:resourcekey="btnSend" Text="Send" ValidationGroup="SendEmail" 
                                        onclick="btnSend_Click" /></td>
                            </tr>
                        </table>
                        <br />
                    </asp:Panel>
                    
					
                    <div class="PreviewArea">
                        <asp:Literal ID="litContent" runat="server" Text="[content]"></asp:Literal>
                    </div>
                    
			    </div>
		    </div>
	    </div>
