<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PeersEditPeer.ascx.cs" Inherits="SolidCP.Portal.PeersEditPeer" %>
<%@ Register TagPrefix="dnc" TagName="UserContact" Src="UserControls/ContactDetails.ascx" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc2" %>
<%@ Register Src="UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="uc2" %>
<asp:BulletedList ID="blLog" runat="server" CssClass="ErrorText">
</asp:BulletedList>
<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate">
<div class="panel-body form-horizontal">
	<table id="tblAccount" runat="server" cellspacing="0" cellpadding="2" width="100%">
		<tr id="rowUsername" runat="server">
			<td class="SubHead" style="width:200px;">
				<asp:Label ID="lblUserName1" runat="server" meta:resourcekey="lblUserName" Text="User name:"></asp:Label>
			</td>
			<td class="NormalBold">
				<asp:TextBox id="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
				<asp:RequiredFieldValidator id="usernameValidator" runat="server" ErrorMessage="*" ControlToValidate="txtUsername"
					Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr id="rowUsernameReadonly" runat="server">
			<td class="SubHead" style="width:200px;">
				<asp:Label ID="lblUserNameReadonly" runat="server" meta:resourcekey="lblUserName" Text="User name:"></asp:Label>
			</td>
			<td class="Huge">
				<asp:Label ID="lblUsername" Runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td colspan="2" class="Normal">
			    <uc2:PasswordControl ID="userPassword" runat="server" />
			</td>
		</tr>
		<tr id="rowChangePassword" runat="server">
			<td class="SubHead">
			</td>
			<td class="Normal">
				<asp:Button id="cmdChangePassword" runat="server" meta:resourcekey="cmdChangePassword" CssClass="Button3" Text="Change Password" OnClick="cmdChangePassword_Click" ValidationGroup="NewPassword"></asp:Button>
			</td>
		</tr>
		<tr>
			<td class="Normal">&nbsp;</td>
		</tr>
		<tr id="rowRole" runat="server">
			<td class="SubHead" valign="top">
			    <asp:Label ID="lblRole" runat="server" meta:resourcekey="lblRole" Text="Role:"></asp:Label>
			</td>
			<td class="NormalBold" valign="top">
				<asp:DropDownList id="role" runat="server" resourcekey="role" AutoPostBack="true" CssClass="form-control">
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblFirstName" runat="server" meta:resourcekey="lblFirstName" Text="First name:"></asp:Label>
			</td>
			<td class="Normal">
				<asp:TextBox id="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
				<asp:RequiredFieldValidator id="firstNameValidator" runat="server" ErrorMessage="*" Display="Dynamic"
					ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblLastName" runat="server" meta:resourcekey="lblLastName" Text="Last name:"></asp:Label>
			</td>
			<td class="Normal">
				<asp:TextBox id="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator id="lastNameValidator" runat="server" ErrorMessage="*" Display="Dynamic"
					ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblEmail" runat="server" meta:resourcekey="lblEmail" Text="E-mail:"></asp:Label>
			</td>
			<td class="Normal">
                <uc2:EmailControl id="txtEmail" runat="server"></uc2:EmailControl>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblSecondaryEmail" runat="server" meta:resourcekey="lblSecondaryEmail" Text="Secondary e-mail:"></asp:Label>
			</td>
			<td class="Normal">
                <uc2:EmailControl id="txtSecondaryEmail" runat="server" RequiredEnabled="false"></uc2:EmailControl>
            </td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblMailFormat" runat="server" meta:resourcekey="lblMailFormat" Text="Mail Format:"></asp:Label>
			</td>
			<td class="Normal">
				<asp:DropDownList ID="ddlMailFormat" runat="server"
				    CssClass="form-control" resourcekey="ddlMailFormat">
				    <asp:ListItem Value="Text">PlainText</asp:ListItem>
				    <asp:ListItem Value="HTML" Selected="True">HTML</asp:ListItem>
				</asp:DropDownList>
			</td>
		</tr>
        <tr id="roleLoginStatus" runat="server">
			<td class="SubHead" valign="top">
			    <asp:Label ID="lblLoginStatus" runat="server" meta:resourcekey="lblLoginStatus" Text="Login Status:"></asp:Label>
			</td>
			<td class="NormalBold" valign="top">
				<asp:DropDownList id="loginStatus" runat="server" resourcekey="loginStatus"  CssClass="form-control">
			        <asp:ListItem Value="Enabled">Enabled</asp:ListItem>
			        <asp:ListItem Value="Disabled">Disabled</asp:ListItem>
				    <asp:ListItem Value="Locked Out">LockedOut</asp:ListItem>
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
                <asp:Label ID="lblDemoAccount" runat="server" meta:resourcekey="lblDemoAccount" Text="Demo Account:"></asp:Label>  
            </td>
			<td class="Normal">
				<asp:CheckBox id="chkDemo" runat="server" meta:resourcekey="chkDemo" Text="Yes"></asp:CheckBox>
			</td>
		</tr>
		<tr id="rowGoogleAuth" runat="server">
			<td class="SubHead">
				<asp:Label ID="lblUseMfa" runat="server" meta:resourcekey="lblUseMfa" Text="Use MFA:"  AssociatedControlID="cbxMfaEnabled"></asp:Label>
			</td>
			<td class="Normal">
				<asp:CheckBox id="cbxMfaEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="cbxMfaEnabled_CheckedChanged"></asp:CheckBox>
				<asp:Label ID="lblMfaEnabled" runat="server" meta:resourcekey="lblMfaEnabled" Text="When you log in, a validation code is sent to the primary email address. Enabling an authentication app stops the validation code from being sent by email."></asp:Label>
			</td>
		</tr>
	</table>
	
    <scp:CollapsiblePanel id="headContact" runat="server" IsCollapsed="true"
        TargetControlID="pnlContact" meta:resourcekey="secContact" Text="Contact"></scp:CollapsiblePanel>
	<asp:Panel ID="pnlContact" runat="server" Height="0" style="overflow:hidden;">
	    <dnc:usercontact id="contact" runat="server"></dnc:usercontact>
	</asp:Panel>

</div>
<div class="panel-footer text-right">
        <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete peer account?');"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
		<CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
	    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" useSubmitBehavior="false"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateText"/> </CPCC:StyleButton>
</div>
</asp:Panel>