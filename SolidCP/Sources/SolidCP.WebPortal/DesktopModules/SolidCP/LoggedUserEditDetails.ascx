<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoggedUserEditDetails.ascx.cs" Inherits="SolidCP.Portal.LoggedUserEditDetails" %>
<%@ Register TagPrefix="dnc" TagName="UserContact" Src="UserControls/ContactDetails.ascx" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc2" %>
<%@ Register Src="UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="uc2" %>
<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate">
<div class="panel-body form-horizontal">

        <asp:Panel ID="pnlAccount" runat="server" DefaultButton="cmdChangePassword">
		<table cellSpacing="0" cellPadding="2" width="100%">
			<tr id="rowUsernameReadonly" runat="server" height="25">
				<td class="SubHead" style="width:200px;">
					<asp:Label ID="lblUserNameText" runat="server" meta:resourcekey="lblUserNameText" Text="User name:"></asp:Label>
				</td>
				<td class="Huge">
					<asp:Label ID="lblUsername" Runat="server"></asp:Label>
				</td>
			</tr>
			<tr>
				<td colspan="2" class="NormalBold">
				    <uc2:PasswordControl ID="userPassword" runat="server" ValidationGroup="NewPassword" />
				</td>
			</tr>
			<tr id="rowChangePassword" runat="server">
				<td class="SubHead">
				</td>
				<td class="Normal">
					<asp:Button id="cmdChangePassword" runat="server" meta:resourcekey="cmdChangePassword" Text="Change Password" CssClass="btn btn-primary" OnClick="cmdChangePassword_Click" ValidationGroup="NewPassword"></asp:Button>
                    <asp:Label ID="lblChangePasswordWarning" runat="server" CssClass="ErrorText">Warning: This will end the current session.</asp:Label>
				</td>
			</tr>
			<tr>
				<td class="Normal">&nbsp;</td>
			</tr>
		</table>
		</asp:Panel>
		
		<table cellSpacing="0" cellPadding="2" width="100%">
			<tr>
				<td class="SubHead" style="width:200px;">
					<asp:Label ID="lblFirstName" runat="server" meta:resourcekey="lblFirstName" Text="First name:"></asp:Label>
				</td>
				<td class="NormalBold">
					<asp:TextBox id="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
					<asp:RequiredFieldValidator id="firstNameValidator" runat="server" ErrorMessage="*" Display="Dynamic"
						ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td class="SubHead">
					<asp:Label ID="lblLastName" runat="server" meta:resourcekey="lblLastName" Text="Last name:"></asp:Label>
				</td>
				<td class="NormalBold">
					<asp:TextBox id="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator id="lastNameValidator" runat="server" ErrorMessage="*" Display="Dynamic"
						ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td class="SubHead">
					<asp:Label ID="lblEmail" runat="server" meta:resourcekey="lblEmail" Text="E-mail:"></asp:Label>
				</td>
				<td class="NormalBold">
                    <uc2:EmailControl id="txtEmail" runat="server">
                    </uc2:EmailControl>
				</td>
			</tr>
			<tr>
				<td class="SubHead">
					<asp:Label ID="lblSecondaryEmail" runat="server" meta:resourcekey="lblSecondaryEmail" Text="Secondary e-mail:"></asp:Label>
				</td>
				<td class="NormalBold">
                    <uc2:EmailControl id="txtSecondaryEmail" runat="server" RequiredEnabled="false">
                    </uc2:EmailControl>
                </td>
			</tr>
			<tr>
				<td class="SubHead">
					<asp:Label ID="lblMailFormat" runat="server" meta:resourcekey="lblMailFormat" Text="Mail Format:"></asp:Label>
				</td>
				<td class="NormalBold">
					<asp:DropDownList ID="ddlMailFormat" runat="server"
					    CssClass="form-control" resourcekey="ddlMailFormat">
					    <asp:ListItem Value="Text">PlainText</asp:ListItem>
					    <asp:ListItem Value="HTML">HTML</asp:ListItem>
					</asp:DropDownList>
				</td>
			</tr>
		</table>
		<br/>
		
        <scp:CollapsiblePanel id="headContact" runat="server" IsCollapsed="true"
            TargetControlID="pnlContact" meta:resourcekey="secContact" Text="Contact">
        </scp:CollapsiblePanel>
		<asp:Panel ID="pnlContact" runat="server" Height="0" style="overflow:hidden;">
		    <dnc:usercontact id="contact" runat="server"></dnc:usercontact>
		</asp:Panel>		
        <scp:CollapsiblePanel id="secDisplay" runat="server" IsCollapsed="true"
            TargetControlID="DisplayPanel" meta:resourcekey="secDisplay" Text="Display Preferences">
        </scp:CollapsiblePanel>
		<asp:Panel ID="DisplayPanel" runat="server" Height="0" style="overflow:hidden;">
		    <table id="tblDisplay" cellSpacing="0" cellPadding="5" runat="server">
			    <tr>
				    <td class="SubHead"  style="width:200px;">
					    <asp:Label ID="lblLanguage" runat="server" meta:resourcekey="lblLanguage" Text="Interface Language:"></asp:Label>
				    </td>
				    <td class="NormalBold">
		                <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control"></asp:DropDownList>
				    </td>
			    </tr>
			    <tr>
				    <td class="SubHead">
					    <asp:Label ID="lblItemsPerPage" runat="server" meta:resourcekey="lblItemsPerPage" Text="Items Per Page:"></asp:Label>
				    </td>
				    <td class="NormalBold">
                        <asp:TextBox ID="txtItemsPerPage" runat="server" CssClass="form-control" Width="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valRequireGridItems" runat="server" ControlToValidate="txtItemsPerPage" meta:resourcekey="valRequireGridItems"
                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valCorrectGridItems" runat="server" ControlToValidate="txtItemsPerPage" meta:resourcekey="valCorrectGridItems"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}"></asp:RegularExpressionValidator>
				    </td>
			    </tr>
		    </table>
		</asp:Panel>	
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdate"/>  </CPCC:StyleButton>
</div>
</asp:Panel>