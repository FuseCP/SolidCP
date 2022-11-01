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
			<tr id="rowGoogleAuth" runat="server">
				<td class="SubHead">
					<asp:Label ID="lblUseMfa" runat="server" meta:resourcekey="lblUseMfa" Text="Use MFA:"  AssociatedControlID="cbxMfaEnabled"></asp:Label>
				</td>
				<td class="Normal">
					<asp:CheckBox id="cbxMfaEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="cbxMfaEnabled_CheckedChanged"></asp:CheckBox>
					<asp:Label ID="lblMfaEnabled" runat="server" meta:resourcekey="lblMfaEnabled" Text="When you log in, a validation code is sent to the primary email address. Enabling an authentication app stops the validation code from being sent by email."></asp:Label>
				</td>
			</tr>
			<tr>
				<td class="SubHead">
				</td>
				<td colspan="2" class="Normal">
					<asp:Button id="btnGetQRCodeData" Visible="false" runat="server" meta:resourcekey="btnGetQRCodeData" Text="Get QR-Code" CssClass="btn btn-primary" OnClick="btnGetQRCodeData_Click"></asp:Button>
					<div id="qrData" visible="false" runat="server">
						<asp:Image ID="imgQrCode" runat="server"/>
						<asp:Label ID="lblManualAuth" runat="server" meta:resourcekey="lblManualAuth"></asp:Label>
						<asp:TextBox id="txtQrCodeActivationPin" runat="server" CssClass="form-control"></asp:TextBox>
						<asp:Button id="btnActivateQRCode" runat="server" meta:resourcekey="btnActivateQRCode" Text="Validate Activation Pin" CssClass="btn btn-primary" OnClick="btnActivateQRCode_Click"></asp:Button>
					</div>
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
		                <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged"></asp:DropDownList>
				    </td>
			    </tr>
			    <tr>
				    <td class="SubHead" style="width:200px;">
					    <asp:Label ID="lblItemsPerPage" runat="server" meta:resourcekey="lblItemsPerPage" Text="Items Per Page:"></asp:Label>
				    </td>
				    <td class="NormalBold">
                        <asp:TextBox ID="txtItemsPerPage" runat="server" CssClass="form-control" Width="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="valRequireGridItems" runat="server" ControlToValidate="txtItemsPerPage" meta:resourcekey="valRequireGridItems"
                            Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valCorrectGridItems" runat="server" ControlToValidate="txtItemsPerPage" meta:resourcekey="valCorrectGridItems"
                            Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}"></asp:RegularExpressionValidator>
				    </td>
			    </tr>
			    <tr>
				    <td class="SubHead"  style="width:200px;">
					    <asp:Label ID="lblTheme" runat="server" meta:resourcekey="lblTheme" Text="Theme:"></asp:Label>
				    </td>
				    <td class="NormalBold">
		                <asp:DropDownList ID="ddlTheme" runat="server" CssClass="form-control" DataValueField="LTRName" DataTextField="DisplayName" AutoPostBack="True" OnSelectedIndexChanged="ddlTheme_SelectedIndexChanged"></asp:DropDownList>
				    </td>
			    </tr>
				<tr>
				    <td class="SubHead"  style="width:200px;">
					    <asp:Label ID="lblThemeStyle" runat="server" meta:resourcekey="lblThemeStyle" Text="Style:"></asp:Label>
				    </td>
				    <td class="NormalBold">
		                <asp:DropDownList ID="ddlThemeStyle" runat="server" CssClass="form-control" DataValueField="PropertyValue" DataTextField="PropertyName" ></asp:DropDownList>
				    </td>
			    </tr>
			   <tr>
				    <td class="SubHead"  style="width:200px;">
					    <asp:Label ID="lblThemecolorHeader" runat="server" meta:resourcekey="lblThemecolorHeader" Text="Header Color:"></asp:Label>
				    </td>
					<td class="NormalBold">
						<asp:panel runat="server" CssClass="row row-cols-auto g-3">
							<asp:Repeater ID="ThemecolorHeaderRepeater1" runat="server">
								<ItemTemplate>
									<asp:panel ID="ThemecolorHeaderPanel" runat="server" Height="45" width="45" BorderWidth="10" BorderColor="Transparent" CssClass="col" >
										<asp:button ID="ThemecolorHeaderButton" runat="server" BorderWidth="0" Height="40" width="40" BackColor='<%# ConvertFromHexToColor( Eval("PropertyName").ToString() )%>' oncommand='ThemecolorHeader_Click' CommandArgument='<%# Eval("PropertyValue").ToString()%>' CssClass="indigator" />
									</asp:panel>
								</ItemTemplate>	
							</asp:Repeater>
						</asp:panel>
					</td>
				   </tr>
			   <tr>
				    <td class="SubHead"  style="width:200px;">
					    <asp:Label ID="lblThemecolorSidebar" runat="server" meta:resourcekey="lblThemecolorSidebar" Text="Sidebar Color:"></asp:Label>
				    </td>
				    <td class="NormalBold">
		                <asp:panel runat="server" CssClass="row row-cols-auto g-3">
							<asp:Repeater ID="ThemecolorSidebarRepeater1" runat="server">
								<ItemTemplate>
									<asp:panel ID="ThemecolorSidebarPanel" runat="server" Height="45" width="45" BorderWidth="10" BorderColor="Transparent" CssClass="col" >
										<asp:button ID="ThemecolorSidebarButton" runat="server" BorderWidth="0" Height="40" width="40" BackColor='<%# ConvertFromHexToColor( Eval("PropertyName").ToString() )%>' oncommand='ThemecolorSidebar_Click' CommandArgument='<%# Eval("PropertyValue").ToString()%>' CssClass="indigator" />
									</asp:panel>  
								</ItemTemplate>	
							</asp:Repeater>
						</asp:panel>
				    </td>
			    </tr>
				<tr>
					<td class="SubHead">
					</td>
					<td class="Normal">
						<asp:Button id="ResetDisplay" runat="server" meta:resourcekey="cmdResetDisplay" Text="Reset Display Settings" CssClass="btn btn-primary" OnClick="cmdResetDisplay_Click"></asp:Button>
					</td>
				</tr>
		    </table>
		</asp:Panel>	
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdate"/>  </CPCC:StyleButton>
</div>
</asp:Panel>