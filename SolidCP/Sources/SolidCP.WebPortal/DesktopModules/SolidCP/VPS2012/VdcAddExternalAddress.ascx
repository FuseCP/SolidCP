<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcAddExternalAddress.ascx.cs" Inherits="SolidCP.Portal.VPS2012.VdcAddExternalAddress" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="scp" %>
<%@ Register Src="../UserControls/AllocatePackageIPAddresses.ascx" TagName="AllocatePackageIPAddresses" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

	    <div class="Content">
		    <div class="Center">
			    <div class="panel-body form-horizontal">

                <scp:AllocatePackageIPAddresses id="allocateAddresses" runat="server"
                        Pool="VPSExternalNetwork"
                        ResourceGroup="VPS2012"
                        ListAddressesControl="vdc_external_network" />
				    
			    </div>
		    </div>
	    </div>
