<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsMoveServer.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VpsMoveServer" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/CheckBoxOption.ascx" TagName="CheckBoxOption" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">
    			    	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="MoveWizard" ShowMessageBox="True" ShowSummary="False" />
                        
                    
                    <table cellpadding="3">
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locSourceService" runat="server" meta:resourcekey="locSourceService" Text="Source Service:"></asp:Localize>
                            </td>
                            <td>
                                <asp:Literal ID="SourceHyperVService" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locDestinationService" runat="server" meta:resourcekey="locDestinationService" Text="Destination Service:"></asp:Localize>
                            </td>
                            <td>
                                <asp:DropDownList ID="HyperVServices" runat="server" CssClass="NormalTextBox"
                                    DataValueField="ServiceId" DataTextField="FullServiceName"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredHyperVService" runat="server"
                                    ControlToValidate="HyperVServices" ValidationGroup="MoveWizard" meta:resourcekey="RequiredHyperVService"
                                    Display="Dynamic" SetFocusOnError="true" Text="*">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    
                    <p>
                        <asp:Button ID="btnMove" runat="server" meta:resourcekey="btnMove"
                            ValidationGroup="MoveWizard" Text="Move" CssClass="Button1" 
                            onclick="btnMove_Click" />
                        <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel"
                            CausesValidation="false" Text="Cancel" CssClass="Button1" 
                            onclick="btnCancel_Click" />
                    </p>
                        
			    </div>
		    </div>
	    </div>
