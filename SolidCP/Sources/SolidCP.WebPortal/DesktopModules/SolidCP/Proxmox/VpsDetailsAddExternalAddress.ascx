<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsAddExternalAddress.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VpsDetailsAddExternalAddress" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_network" />	
			        
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                        ValidationGroup="AddAddress" ShowMessageBox="True" ShowSummary="False" />
                    
		            <p class="SubTitle">
		                <asp:Localize ID="locSubTitle" runat="server" meta:resourcekey="locSubTitle"
		                    Text="Add External IP Addresses" />
		            </p>
		            
                     <div runat="server" ID="EmptyExternalAddressesMessage" style="padding: 5px;" visible="false">
                        <asp:Localize ID="locNotEnoughExternalAddresses" runat="server" Text="Not enough..."
                                meta:resourcekey="locNotEnoughExternalAddresses"></asp:Localize>
                     </div>
                    
                    <table id="ExternalAddressesTable" runat="server" cellspacing="5" style="width: 100%;">
                        <tr>
                            <td>
                                <asp:RadioButton ID="radioExternalRandom" runat="server" AutoPostBack="true"
                                    meta:resourcekey="radioExternalRandom" Text="Randomly select IP addresses from the pool" 
                                    Checked="True" GroupName="ExternalAddress" />
                            </td>
                        </tr>
                        <tr id="ExternalAddressesNumberRow" runat="server">
                            <td style="padding-left: 30px;">
                                <asp:Localize ID="locExternalAddresses" runat="server"
                                        meta:resourcekey="locExternalAddresses" Text="Number of IP addresses:"></asp:Localize>

                                <asp:TextBox ID="txtExternalAddressesNumber" runat="server" CssClass="NormalTextBox" Width="50" Text="1"></asp:TextBox>
                                
                                <asp:RequiredFieldValidator ID="ExternalAddressesValidator" runat="server" Text="*" Display="Dynamic"
                                        ControlToValidate="txtExternalAddressesNumber" meta:resourcekey="ExternalAddressesValidator" SetFocusOnError="true"
                                        ValidationGroup="AddAddress">*</asp:RequiredFieldValidator>
                                
                                <asp:Literal ID="litMaxExternalAddresses" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton ID="radioExternalSelected" runat="server" AutoPostBack="true"
                                    meta:resourcekey="radioExternalSelected" Text="Select IP addresses from the list" 
                                    GroupName="ExternalAddress" />
                            </td>
                        </tr>
                        <tr id="ExternalAddressesListRow" runat="server">
                            <td style="padding-left: 30px;">
                                <asp:ListBox ID="listExternalAddresses" SelectionMode="Multiple" runat="server" Rows="8"
                                    CssClass="_NormalTextBox" Width="300" ></asp:ListBox>
                                <br />
                                <asp:Localize ID="locHoldCtrl" runat="server"
                                        meta:resourcekey="locHoldCtrl" Text="* Hold CTRL key to select multiple addresses"></asp:Localize>
                            </td>
                        </tr>
                    </table>
                    
                    <p>
                        <asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd"
                            ValidationGroup="AddAddress" Text="Add" CssClass="Button1" 
                            onclick="btnAdd_Click" />
                        <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel"
                            CausesValidation="false" Text="Cancel" CssClass="Button1" 
                            onclick="btnCancel_Click" />
                    </p>

				    
			    </div>
		    </div>
	    </div>
