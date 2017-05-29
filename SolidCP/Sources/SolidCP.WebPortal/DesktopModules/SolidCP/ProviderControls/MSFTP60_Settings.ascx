<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MSFTP60_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.MSFTP60_Settings" %>
<%@ Register Src="Common_ActiveDirectoryIntegration.ascx" TagName="ActiveDirectoryIntegration" TagPrefix="uc1" %>
<table cellpadding="4" cellspacing="0" width="100%">
	<tr>
		<td class="SubHead" width="200" nowrap>
		    <asp:Label ID="lblSite" runat="server" meta:resourcekey="lblSite" Text="FTP Accounts Site:"></asp:Label>
		</td>
		<td width="100%">
            <asp:DropDownList ID="ddlFtpSite" runat="server" CssClass="form-control"
                    DataValueField="SiteId" DataTextField="Name">
            </asp:DropDownList></td>
	</tr>
	<tr>
	    <td class="SubHead" valign="top">
	        <asp:Label ID="lblGroupName" runat="server" meta:resourcekey="lblGroupName" Text="FTP Users Group Name:"></asp:Label>
	    </td>
	    <td class="Normal" valign="top">
            <asp:TextBox ID="txtFtpGroupName" runat="server" CssClass="form-control" Width="200px"></asp:TextBox></td>
	</tr>
	<tr>
		<td class="SubHead">
		    <asp:Label ID="lblBuildUncFilesPath" runat="server" meta:resourcekey="lblBuildUncFilesPath" Text="Build UNC Path to Space Files:"></asp:Label>
		</td>
		<td>
			<asp:CheckBox ID="chkBuildUncFilesPath" runat="server" meta:resourcekey="chkBuildUncFilesPath" Text="Yes" />
		</td>
	</tr>
	<tr>
	    <td class="SubHead" valign="top">
	        <asp:Label ID="lblADIntegration" runat="server" meta:resourcekey="lblADIntegration" Text="Active Directory Integration:"></asp:Label>    
	    </td>
	    <td class="Normal" valign="top">
            <uc1:ActiveDirectoryIntegration ID="ActiveDirectoryIntegration" runat="server" />

        </td>
	</tr>
</table>