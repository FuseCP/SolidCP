<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesIPAddresses.ascx.cs" Inherits="SolidCP.Portal.WebSitesIPAddresses" %>
<%@ Register Src="UserControls/PackageIPAddresses.ascx" TagName="PackageIPAddresses" TagPrefix="scp" %>
<%@ Register Src="UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>

<div class="panel-body form-horizontal">
    <scp:PackageIPAddresses id="webAddresses" runat="server"
            Pool="WebSites"
            EditItemControl=""
            SpaceHomeControl=""
            AllocateAddressesControl="allocate_addresses"
            ManageAllowed="true" />
    
    <br />
    <scp:CollapsiblePanel id="secQuotas" runat="server"
        TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
    </scp:CollapsiblePanel>
    <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
    
    <table cellspacing="6">
        <tr>
            <td><asp:Localize ID="locIPQuota" runat="server" meta:resourcekey="locIPQuota" Text="Number of IP Addresses:"></asp:Localize></td>
            <td><scp:Quota ID="addressesQuota" runat="server" QuotaName="Web.IPAddresses" /></td>
        </tr>
    </table>
    
    
    </asp:Panel>
</div>

