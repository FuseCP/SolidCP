<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesAllocateIPAddresses.ascx.cs" Inherits="SolidCP.Portal.WebSitesAllocateIPAddresses" %>
<%@ Register Src="UserControls/AllocatePackageIPAddresses.ascx" TagName="AllocatePackageIPAddresses" TagPrefix="scp" %>

<div class="panel-body form-horizontal">

    <scp:AllocatePackageIPAddresses id="allocateAddresses" runat="server"
            Pool="WebSites"
            ResourceGroup="Web"
            ListAddressesControl="" />
            
</div>