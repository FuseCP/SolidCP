<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail100x_EditGroup.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail100x_EditGroup" %>
<%@ Register TagPrefix="dnc" TagName="EditItemsList" Src="../MailEditItems.ascx" %>
<table cellSpacing="0" cellPadding="3" width="100%">
	<tr>
		<td class="SubHead" width="600" nowrap>
		    <asp:Label ID="lblDisplayName" runat="server" meta:resourcekey="lblDisplayName" Text="Group DisplayName:"></asp:Label>
		</td>
		<td class="normal" width="100%">
            <asp:TextBox ID="txtDisplayName" runat="server" Width="300px"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead" vAlign="top" >
		    <asp:Label ID="lblGroupMembers" runat="server" meta:resourcekey="lblGroupMembers" Text="Group e-mails:"></asp:Label>
		</td>
		<td vAlign="top" >
			<dnc:EditItemsList id="mailEditItems" runat="server"></dnc:EditItemsList>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblIncludeAllDomainUsers" runat="server" meta:resourcekey="lblIncludeAllDomainUsers" Text="Include all domain users:"></asp:Label>
		</td>
		<td class="normal">
			<asp:CheckBox ID="chkIncludeAllDomainUsers" runat="server" meta:resourcekey="chkIncludeAllDomainUsers" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblEnableGAL" runat="server" meta:resourcekey="lblEnableGAL" Text="Show in Global Address List:"></asp:Label>
		</td>
		<td class="normal">
			<asp:CheckBox ID="chkEnableGAL" runat="server" meta:resourcekey="chkEnableGAL" Checked="True"/>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblEnableChat" runat="server" meta:resourcekey="lblEnableChat" Text="Enable Group Chat:"></asp:Label>
		</td>
		<td class="normal">
			<asp:CheckBox ID="chkEnableChat" runat="server" meta:resourcekey="chkEnableChat" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblInternalOnly" runat="server" meta:resourcekey="lblInternalOnly" Text="Internal use only:"></asp:Label>
		</td>
		<td class="normal">
			<asp:CheckBox ID="chkInternalOnly" runat="server" meta:resourcekey="chkInternalOnly" />
		</td>
	</tr>
</table>