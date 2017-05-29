<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhoneNumbersEditPhoneNumber.ascx.cs" Inherits="SolidCP.Portal.PhoneNumbersEditPhoneNumber" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>

<div class="panel-body form-horizontal">

    <scp:SimpleMessageBox id="messageBox" runat="server" />

    <asp:ValidationSummary ID="validatorsSummary" runat="server" 
            ValidationGroup="EditAddress" ShowMessageBox="True" ShowSummary="False" />

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
		    
		        <asp:TextBox id="Phone" runat="server" Width="300px" MaxLength="45" CssClass="form-control"/>
                <asp:RequiredFieldValidator ID="requireStartPhoneValidator" runat="server" meta:resourcekey="requireStartPhoneValidator"
                    ControlToValidate="Phone" SetFocusOnError="true" Text="*" Enabled="false" ValidationGroup="EditAddress" ErrorMessage="Enter Phone Number" />					            

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
    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" useSubmitBehavior="false" ValidationGroup="EditAddress"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdate"/> </CPCC:StyleButton>
</div>