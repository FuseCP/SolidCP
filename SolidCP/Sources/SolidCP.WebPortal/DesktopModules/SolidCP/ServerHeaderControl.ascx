<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServerHeaderControl.ascx.cs" Inherits="SolidCP.Portal.ServerHeaderControl" %>
<div class="panel-body form-horizontal">
	<div class="Huge">
		<asp:Literal ID="litServerName" runat="server"></asp:Literal>
	</div>
	<div class="Normal">
		<asp:Literal ID="litServerComments" runat="server" Visible="false"></asp:Literal>
	</div>
</div>