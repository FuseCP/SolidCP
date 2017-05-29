<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcExternalNetwork.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VdcExternalNetwork" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/PackageIPAddresses.ascx" TagName="PackageIPAddresses" TagPrefix="wsp" %>


	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">

                    
                    <wsp:PackageIPAddresses id="packageAddresses" runat="server"
                            Pool="VpsExternalNetwork"
                            EditItemControl="vps_general"
                            SpaceHomeControl="vdc_external_network"
                            AllocateAddressesControl="vdc_allocate_external_ip" />

    				
    				<br />
				    <wsp:CollapsiblePanel id="secQuotas" runat="server"
                        TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
                    
                        <table cellspacing="6">
                            <tr>
                                <td><asp:Localize ID="locIPQuota" runat="server" meta:resourcekey="locIPQuota" Text="Number of IP Addresses:"></asp:Localize></td>
                                <td><wsp:Quota ID="addressesQuota" runat="server" QuotaName="Proxmox.ExternalIPAddressesNumber" /></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locBandwidthQuota" runat="server" meta:resourcekey="locBandwidthQuota" Text="Bandwidth, GB:"></asp:Localize></td>
                                <td><wsp:Quota ID="bandwidthQuota" runat="server" QuotaName="Proxmox.Bandwidth" /></td>
                            </tr>
                        </table>
                    
                    
                    </asp:Panel>

			    </div>
		    </div>
	    </div>
