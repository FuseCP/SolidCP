<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcManagementNetwork.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VdcManagementNetwork" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/PackageIPAddresses.ascx" TagName="PackageIPAddresses" TagPrefix="wsp" %>


	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">
                    <wsp:PackageIPAddresses id="packageAddresses" runat="server"
                            Pool="VpsManagementNetwork"
                            EditItemControl="vps_general"
                            SpaceHomeControl="vdc_management_network"
                            AllocateAddressesControl=""
                            ManageAllowed="true" />
			    </div>
		    </div>
	    </div>
