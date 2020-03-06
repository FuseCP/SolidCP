<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteFooter.ascx.cs" Inherits="SolidCP.Portal.SkinControls.SiteFooter" %>
<%@ Register TagPrefix="scp" TagName="ProductVersion" Src="ProductVersion.ascx" %>
<div class="row">
     <div class="col-md-9 Copyright">
         	<asp:Localize ID="locPoweredBy" runat="server" meta:resourcekey="locPoweredBy" />
     </div>	
     <div class="col-md-3 Version">
            <asp:Localize ID="locVersion" runat="server" meta:resourcekey="locVersion" /> <scp:ProductVersion id="scpVersion" runat="server" AssemblyName="SolidCP.Portal.Modules"/>
     </div>
     </div>
 </div>