<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MSDNS_Settings.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.MSDNS_Settings" %>
<%@ Register Src="../UserControls/ScheduleInterval.ascx" TagName="ScheduleInterval" TagPrefix="uc4" %>
<%@ Register Src="Common_IPAddressesList.ascx" TagName="IPAddressesList" TagPrefix="uc1" %>
<%@ Register Src="Common_SecondaryDNSServers.ascx" TagName="SecondaryDNSServers" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/EditDomainsList.ascx" TagName="EditDomainsList" TagPrefix="uc5" %>
<table cellpadding="1" cellspacing="0" width="100%"> 
		<tr>
			<td class="SubHead" width="200" nowrap valign="top">
			    <asp:Label ID="lblIPAddresses" runat="server" meta:resourcekey="lblIPAddresses" Text="Listening IP Addresses:"></asp:Label>
			</td>
			<td width="100%" valign="top">
                <uc1:IPAddressesList id="iPAddressesList" runat="server">
                </uc1:IPAddressesList></td>
		</tr>
		<tr>
			<td class="SubHead" valign="top">
				<asp:Label ID="lblAllowZoneTransfers" runat="server" meta:resourcekey="lblAllowZoneTransfers" Text="Allow Zone Transfers: type IP of secondary DNS Server here."></asp:Label>
			</td>
			<td>
				<asp:TextBox ID="txtAllowZoneTransfers" runat="server" Rows="4" TextMode="MultiLine" Width="200px"></asp:TextBox>
			</td>
		</tr>
	    <tr>
		    <td class="SubHead" noWrap>
		        <asp:Label ID="lblAdMode" runat="server" meta:resourcekey="lblAdMode" Text="Create Zones in Active Directory:"></asp:Label>
		    </td>
		    <td class="Normal">
                <asp:CheckBox ID="chkAdMode" runat="server" meta:resourcekey="chkAdMode" Text="Yes" />
		    </td>
	    </tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblResponsiblePerson" runat="server" meta:resourcekey="lblResponsiblePerson" Text="Responsible Person:"></asp:Label>
			</td>
			<td>
				<asp:TextBox ID="txtResponsiblePerson" runat="server" Width="300px" CssClass="form-control"></asp:TextBox>
			</td>
		</tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Label ID="lblRefreshInterval" runat="server" meta:resourcekey="lblRefreshInterval" Text="Refresh Interval:"></asp:Label>
		    </td>
		    <td class="Normal">
                <uc4:ScheduleInterval ID="intRefresh" runat="server" />
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Label ID="lblRetryInterval" runat="server" meta:resourcekey="lblRetryInterval" Text="Retry Interval:"></asp:Label>
		    </td>
		    <td class="Normal">
                <uc4:ScheduleInterval ID="intRetry" runat="server" />
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead">
		        <asp:Label ID="lblExpireLimit" runat="server" meta:resourcekey="lblExpireLimit" Text="Expire Limit:"></asp:Label>
		    </td>
		    <td class="Normal">
                <uc4:ScheduleInterval ID="intExpire" runat="server" />
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead" noWrap>
		        <asp:Label ID="lblMinimumTtl" runat="server" meta:resourcekey="lblMinimumTtl" Text="Minimum TTL:"></asp:Label>
		    </td>
		    <td class="Normal">
                <uc4:ScheduleInterval ID="intTtl" runat="server" />
		    </td>
	    </tr>
		<tr>
		    <td class="SubHead" valign="top">
		        <asp:Label ID="lblSecondaryDNS" runat="server" meta:resourcekey="lblSecondaryDNS" Text="Secondary DNS Services:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <uc2:SecondaryDNSServers ID="secondaryDNSServers" runat="server" />
            </td>
		</tr>
		<tr>
		    <td class="SubHead" valign="top">
		        <asp:Label ID="lblNameServers" runat="server" meta:resourcekey="lblNameServers" Text="Name Servers:"></asp:Label>
		    </td>
		    <td class="Normal" valign="top">
                <uc5:EditDomainsList id="nameServers" runat="server">
                </uc5:EditDomainsList>
            </td>
		</tr>
</table>