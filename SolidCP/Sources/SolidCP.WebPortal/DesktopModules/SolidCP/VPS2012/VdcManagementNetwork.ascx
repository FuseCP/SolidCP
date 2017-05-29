<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcManagementNetwork.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VdcManagementNetwork" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/PackageIPAddresses.ascx" TagName="PackageIPAddresses" TagPrefix="scp" %>


	    <div class="Content">
		    <div class="Center">
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
