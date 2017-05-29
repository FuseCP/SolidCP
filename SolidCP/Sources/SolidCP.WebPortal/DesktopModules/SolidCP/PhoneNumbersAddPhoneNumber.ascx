<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhoneNumbersAddPhoneNumber.ascx.cs" Inherits="SolidCP.Portal.PhoneNumbersAddPhoneNumber" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<div class="panel-body form-horizontal">

    <scp:SimpleMessageBox id="messageBox" runat="server" />
    
    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
            ValidationGroup="EditAddress" ShowMessageBox="True" ShowSummary="False" />

	<asp:CustomValidator ID="consistentAddresses" runat="server" ErrorMessage="You must not mix IPv4 and IPv6 addresses." ValidationGroup="EditAddress" Display="dynamic" ServerValidate="CheckIPAddresses" /> 
    
	<table cellspacing="0" cellpadding="3">
	    <tr>
		    <td style="width:150px;"><asp:Localize ID="locServer" runat="server" meta:resourcekey="locServer" Text="Server:"></asp:Localize></td>
		    <td>
		        <asp:dropdownlist id="ddlServer" CssClass="form-control" runat="server" DataTextField="ServerName" DataValueField="ServerID"></asp:dropdownlist>
		    </td>
	    </tr>
	    <tr id="PhoneNumbersRow" runat="server">
		    <td style="width:150px;"><asp:Localize ID="Localize1" runat="server" meta:resourcekey="lblPhoneNumbers" Text="Phone Numbers:"></asp:Localize></td>
		    <td>
		        <div class="form-inline">
		        <asp:TextBox id="startPhone" runat="server" Width="300px" MaxLength="45" CssClass="form-control"/>
                <asp:RequiredFieldValidator ID="requireStartPhoneValidator" runat="server" meta:resourcekey="requireStartPhoneValidator"
                    ControlToValidate="startPhone" SetFocusOnError="true" Text="*" Enabled="false" ValidationGroup="EditAddress" ErrorMessage="Enter Phone Number" />					            

			    &nbsp;<asp:Localize ID="Localize2" runat="server" meta:resourcekey="locTo" Text="to"></asp:Localize>&nbsp;

		        <asp:TextBox id="endPhone" runat="server" ValidationGroup="EditAddress"  Width="300px" MaxLength="45" CssClass="form-control"/>
		        </div>
		    </td>
	    </tr>
	    <tr>
		    <td style="width:150px;"><asp:Localize ID="lblComments" runat="server" meta:resourcekey="lblComments" Text="Comments:"></asp:Localize></td>
		    <td><asp:textbox id="txtComments" CssClass="form-control" runat="server" Rows="3" TextMode="MultiLine"></asp:textbox></td>
	    </tr>
    </table>
    
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" ValidationGroup="EditAddress"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAdd"/> </CPCC:StyleButton>
</div>