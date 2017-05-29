<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcExternalNetwork.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VdcExternalNetwork" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PackageIPAddresses.ascx" TagName="PackageIPAddresses" TagPrefix="scp" %>


	    <div class="Content">
		    <div class="Center">
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
                                <td><scp:Quota ID="addressesQuota" runat="server" QuotaName="VPS2012.ExternalIPAddressesNumber" /></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locBandwidthQuota" runat="server" meta:resourcekey="locBandwidthQuota" Text="Bandwidth, GB:"></asp:Localize></td>
                                <td><scp:Quota ID="bandwidthQuota" runat="server" QuotaName="VPS2012.Bandwidth" /></td>
                            </tr>
                        </table>
                    
                    
                    </asp:Panel>

			    </div>
		    </div>
	    </div>