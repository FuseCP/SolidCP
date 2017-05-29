<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcManagementNetwork.ascx.cs" Inherits="SolidCP.Portal.VPS.VdcManagementNetwork" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PackageIPAddresses.ascx" TagName="PackageIPAddresses" TagPrefix="scp" %>


	    <div class="panel panel-default">
			    <div class="panel-heading">
				    <asp:Image ID="imgIcon" SkinID="Network48" runat="server" />
				    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Management Network"></asp:Localize>
			    </div>
            <div class="panel-body form-horizontal">
                    <scp:Menu id="menu" runat="server" SelectedItem="vdc_management_network" />
            <div class="panel panel-default tab-content">
            <div class="panel-body form-horizontal">  
                    <scp:PackageIPAddresses id="packageAddresses" runat="server"
                            Pool="VpsManagementNetwork"
                            EditItemControl="vps_general"
                            SpaceHomeControl="vdc_management_network"
                            AllocateAddressesControl=""
                            ManageAllowed="true" />
                            
			    </div>
                </div>
                </div>
                </div>