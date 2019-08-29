<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcAddPrivateNetworkVLAN.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VdcAddPrivateNetworkVLAN" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/AllocatePackageVLANs.ascx" TagName="AllocatePackageVLANs" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">

                <scp:AllocatePackageVLANs id="allocateVLANs" runat="server"
                        ResourceGroup="VPS2012"
                        ListAddressesControl="vdc_private_network" />
				    
			    </div>
		    </div>
	    </div>
