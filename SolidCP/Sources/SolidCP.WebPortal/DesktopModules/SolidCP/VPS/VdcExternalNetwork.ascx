<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcExternalNetwork.ascx.cs" Inherits="SolidCP.Portal.VPS.VdcExternalNetwork" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PackageIPAddresses.ascx" TagName="PackageIPAddresses" TagPrefix="scp" %>
    	
	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="Image1" SkinID="Network48" runat="server" />
				    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="External Network"></asp:Localize>
			    </div>
            <div class="panel-body form-horizontal">
            <scp:Menu id="menu" runat="server" SelectedItem="vdc_external_network" />
            <div class="panel panel-default tab-content">
            <div class="panel-body form-horizontal">  
                    <scp:PackageIPAddresses id="packageAddresses" runat="server"
                            Pool="VpsExternalNetwork"
                            EditItemControl="vps_general"
                            SpaceHomeControl="vdc_external_network"
                            AllocateAddressesControl="vdc_allocate_external_ip" />
    				<br />
				    <scp:CollapsiblePanel id="secQuotas" runat="server"
                        TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
                        <table cellspacing="6">
                            <tr>
                                <td><asp:Localize ID="locIPQuota" runat="server" meta:resourcekey="locIPQuota" Text="Number of IP Addresses:"></asp:Localize></td>
                                <td><scp:Quota ID="addressesQuota" runat="server" QuotaName="VPS.ExternalIPAddressesNumber" /></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locBandwidthQuota" runat="server" meta:resourcekey="locBandwidthQuota" Text="Bandwidth, GB:"></asp:Localize></td>
                                <td><scp:Quota ID="bandwidthQuota" runat="server" QuotaName="VPS.Bandwidth" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
			    </div>
                </div>
                </div>
                </div>