<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetupControlPanelAccounts.ascx.cs" Inherits="SolidCP.Portal.SetupControlPanelAccounts" %>
<%@ Register src="UserControls/PasswordControl.ascx" tagname="PasswordControl" TagPrefix="scp" %>
<div class="panel-body form-horizontal">
	<p class="SubHead" style="text-align: justify;"><asp:Localize runat="server" meta:resourcekey="ScpaProcedureDescription" /></p>
	<table cellpadding="3" cellspacing="0">
		<tr>
			<td class="SubHead" align="right" style="width:100px;"><asp:Localize runat="server" meta:resourcekey="lblUserNameA" Text="User Name:" /></td>
			<td class="NormalBold" align="left"><asp:Localize runat="server" Text="serveradmin" /></td>
		</tr>
		<tr>
			<td class="SubHead" align="right" valign="top" nowrap><asp:Localize runat="server" meta:resourcekey="lblPassword" Text="Password:" /></td>
			<td class="Normal" align="left" valign="middle">
				<scp:PasswordControl ID="PasswordControlA" runat="server" />
			</td>	
		</tr>
		<tr>
			<td class="SubHead" align="right" style="width:100px;"><asp:Localize runat="server" meta:resourcekey="lblUserNameA" Text="User Name:" /></td>
			<td class="NormalBold" align="left"><asp:Localize ID="Localize2" runat="server" Text="admin" /></td>
		</tr>
		<tr>
			<td class="SubHead" align="right" valign="top" nowrap><asp:Localize runat="server" meta:resourcekey="lblPassword" Text="Password:" /></td>
			<td class="Normal" align="left" valign="middle">
				<scp:PasswordControl ID="PasswordControlB" runat="server" />
			</td>	
		</tr>
		<tr>
			<td class="SubHead" nowrap></td>
			<td align="left">
				<asp:Button id="CompleteSetupButton" runat="server" meta:resourcekey="CompleteSetupButton" 
					Text="Complete Setup" CssClass="LoginButton" OnClick="CompleteSetupButton_Click" />
			</td>
		</tr>
	</table>
</div>