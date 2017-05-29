<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="SolidCP.Portal.Proxmox.UserControls.Menu" %>
<div class="Menu">
    
	<asp:DataList runat="server" ID="MenuItems" EnableViewState="false" RepeatDirection="Horizontal" RepeatLayout="Flow">
	    <ItemTemplate>
	        <div class="MenuItem">
                <asp:HyperLink ID="lnkItem" runat="server" NavigateUrl='<%# Eval("Url") %>' Text='<%# Eval("Text") %>'/>			                
            </div>
	    </ItemTemplate>
	    <SelectedItemTemplate>
            <div class="SelectedMenuItem">
                <asp:HyperLink ID="lnkItem" runat="server"
	                NavigateUrl='<%# Eval("Url") %>'
	                Text='<%# Eval("Text") %>'/>			                
            </div>
        </SelectedItemTemplate>
	</asp:DataList>	
</div>