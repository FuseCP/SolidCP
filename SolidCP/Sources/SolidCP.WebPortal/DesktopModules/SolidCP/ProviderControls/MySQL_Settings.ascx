<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MySQL_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.MySQL_Settings" %>
<fieldset>
    <legend>
        <asp:Label runat="server" CssClass="NormalBold" meta:resourcekey="lblFtuNote" />
    </legend>
    <div runat="server" id="lblFirsttimeUserNote" />
</fieldset>

<fieldset>
    <legend>
        <asp:Label runat="server" CssClass="NormalBold" meta:resourcekey="lblServiceConfig" />
    </legend>
	<table cellpadding="4" cellspacing="0" width="100%">
		<tr>
			<td class="SubHead" width="200" nowrap>
				<asp:Label ID="lblInternalAddress" runat="server" meta:resourcekey="lblInternalAddress" Text="Internal Address:"></asp:Label>
			</td>
			<td width="100%"><asp:TextBox Width="200px" CssClass="form-control" Runat="server" ID="txtInternalAddress"></asp:TextBox></td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblExternalAddress" runat="server" meta:resourcekey="lblExternalAddress" Text="External Address:"></asp:Label>
			</td>
			<td>
				<asp:TextBox Width="200px" CssClass="form-control" Runat="server" ID="txtExternalAddress"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblFolder" runat="server" meta:resourcekey="lblFolder" Text="MySQL Installation Folder:"></asp:Label>
			</td>
			<td><asp:TextBox Runat="server" ID="txtBinFolder" CssClass="form-control" Width="300px"></asp:TextBox></td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblLogin" runat="server" meta:resourcekey="lblLogin" Text="Root Login:"></asp:Label>
			</td>
			<td><asp:TextBox Runat="server" ID="txtUserName" CssClass="form-control" Width="150px"></asp:TextBox></td>
		</tr>
		<tr id="rowPassword" runat="server">
			<td class="SubHead">
				<asp:Label ID="lblCurrPassword" runat="server" meta:resourcekey="lblCurrPassword" Text="Current Root Password:"></asp:Label>
			</td>
			<td class="Normal">******
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Root Password:"></asp:Label>
			</td>
			<td><asp:TextBox Runat="server" ID="txtPassword" CssClass="form-control" Width="150px" TextMode="Password"></asp:TextBox>
				</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblConfirmPassword" runat="server" meta:resourcekey="lblConfirmPassword" Text="Confirm Root Password:"></asp:Label>
			</td>
			<td class="NormalBold"><asp:TextBox Runat="server" ID="txtConfirmPassword" CssClass="form-control" Width="150px" TextMode="Password"></asp:TextBox>
				<asp:CompareValidator id="passwordIdentValidator" runat="server" meta:resourcekey="passwordIdentValidator" ErrorMessage="Both passwords should be identical"
					ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Display="Dynamic"></asp:CompareValidator></td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblOldPassword" runat="server" meta:resourcekey="lblOldPassword" Text="Use 'OLD_PASSWORD' Algorithm:"></asp:Label>
			</td>
			<td class="Normal">
				<asp:CheckBox ID="chkOldPassword" runat="server" meta:resourcekey="chkOldPassword" Text="Yes" />
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblBrowseUrl" runat="server" meta:resourcekey="lblBrowseUrl" Text="Database Browser Logon URL:"></asp:Label>
			</td>
			<td>
				<asp:TextBox Width="400px" CssClass="form-control" Runat="server" ID="txtBrowseUrl"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblBrowseMethod" runat="server" meta:resourcekey="lblBrowseMethod" Text="Database Browser Logon Method:"></asp:Label>
			</td>
			<td>
				<asp:DropDownList ID="ddlBrowseMethod" runat="server" CssClass="form-control">
					<asp:ListItem Value="POST">POST</asp:ListItem>
					<asp:ListItem Value="GET">GET</asp:ListItem>
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblBrowseParameters" runat="server" meta:resourcekey="lblBrowseParameters" Text="Database Browser Logon Parameters:"></asp:Label>
			</td>
			<td>
				<asp:TextBox Width="200px" CssClass="form-control" Runat="server" TextMode="MultiLine" Rows="5" ID="txtBrowseParameters"></asp:TextBox>
			</td>
		</tr>
	</table>
</fieldset>