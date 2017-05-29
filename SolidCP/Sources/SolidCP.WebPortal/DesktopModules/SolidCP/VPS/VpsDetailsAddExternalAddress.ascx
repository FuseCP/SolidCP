<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsAddExternalAddress.ascx.cs" Inherits="SolidCP.Portal.VPS.VpsDetailsAddExternalAddress" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="scp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>   	
	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="Network48" runat="server" />
				    <scp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Network" />
			    </div>
			    <div class="panel-body form-horizontal">
                    <scp:Menu id="menu" runat="server" SelectedItem="" />
                <div class="panel panel-default tab-content">
                <div class="panel-body form-horizontal">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
			        <scp:ServerTabs id="tabs" runat="server" SelectedTab="vps_network" />	
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
                                    CssClass="NormalTextBox" Width="220" ></asp:ListBox>
                                <br />
                                <asp:Localize ID="locHoldCtrl" runat="server"
                                        meta:resourcekey="locHoldCtrl" Text="* Hold CTRL key to select multiple addresses"></asp:Localize>
                            </td>
                        </tr>
                    </table>
                    
                    <p>
                        <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>
                        <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" ValidationGroup="AddAddress"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAdd"/> </CPCC:StyleButton>&nbsp;
                    </p>
			    </div>
                </div>
                </div>
                </div>