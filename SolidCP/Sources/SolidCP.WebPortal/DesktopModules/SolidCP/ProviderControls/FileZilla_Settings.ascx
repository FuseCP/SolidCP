<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileZilla_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.FileZilla_Settings" %>
<table cellpadding="4" cellspacing="0" width="100%">
	<tr>
		<td class="SubHead" style="width:200px;">
		    <asp:Label ID="lblBuildUncFilesPath" runat="server" meta:resourcekey="lblBuildUncFilesPath" Text="Build UNC Path to Space Files:"></asp:Label>
		</td>
		<td>
			<asp:CheckBox ID="chkBuildUncFilesPath" runat="server" meta:resourcekey="chkBuildUncFilesPath" Text="Yes" />
		</td>
	</tr>
</table>