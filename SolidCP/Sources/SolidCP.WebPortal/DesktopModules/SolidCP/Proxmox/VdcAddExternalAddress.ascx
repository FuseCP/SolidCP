<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcAddExternalAddress.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VdcAddExternalAddress" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/AllocatePackageIPAddresses.ascx" TagName="AllocatePackageIPAddresses" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">

                <wsp:AllocatePackageIPAddresses id="allocateAddresses" runat="server"
                        Pool="VPSExternalNetwork"
                        ResourceGroup="Proxmox"
                        ListAddressesControl="vdc_external_network" />
				    
			    </div>
		    </div>
	    </div>
