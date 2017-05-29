<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoUpdateServers.ascx.cs" Inherits="SolidCP.Portal.AutoUpdateServers" %>
<%@ Register TagPrefix="scp" TagName="ProductVersion" Src="SkinControls/ProductVersion.ascx" %>
<%@ Import Namespace="SolidCP.Portal" %>

    <div class="buttons-in-panel-header">
<asp:Label ID="lblSelectVersion" runat="server">Select version</asp:Label>
<asp:DropDownList ID="ddlSelectVersion" runat="server"></asp:DropDownList>
</div>
<div class="panel-body">
<asp:DataList ID="dlServers" Runat="server" RepeatLayout="Flow"  RepeatDirection="Horizontal">

	<ItemTemplate>
            <div class="col-md-4">
        <div class=" panel panel-info server-panel matchHeight">
            <div class="panel-heading"
               <h3 class="panel-title">
                    <i class="fa fa-server" aria-hidden="true">&nbsp;</i>&nbsp;
        <asp:CheckBox ID="chkServer" AutoPostBack="true" runat="server" Checked="true" Value='<%# Eval("ServerID") %>' /> 
        <%# PortalAntiXSS.EncodeOld((string)Eval("ServerName")) %>
                   </h3>
            </div>  </div>
                </div>
    </ItemTemplate>
</asp:DataList>
</div>
<div class="panel-footer text-right">
    <div class="pull-left">
    <asp:Label ID="lblUpdateMessage" runat="server" CssClass="pull-left" Text="This will update all servers to version:" /> <scp:ProductVersion id="scpVersion" runat="server" AssemblyName="SolidCP.Portal.Modules"/><br />
	</div>
    <asp:Button ID="btnUpdateServers" runat="server" meta:resourcekey="btnUpdateServers" Text="Update Servers" CssClass="btn btn-success" OnClick="btnUpdateServers_Click" />
</div>

<asp:Panel CssClass="FailedList" ID="failedList" runat="server" Visible="false">
    <table class="failed">
        <asp:Repeater runat="server" ID="lstFailed">
            <HeaderTemplate>
                <thead><tr><th>Server</th><th>Message</th></tr></thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# getServerName(((KeyValuePair<int,string>)Container.DataItem).Key) %></td>
                    <td><%# ((KeyValuePair<int,string>)Container.DataItem).Value %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>

<table id="tblEmptyList" runat="server" cellpadding="10" cellspacing="0" width="100%">
    <tr>
        <td class="Normal" align="center">
            <asp:Label ID="lblEmptyList" runat="server" meta:resourcekey="lblEmptyList" Text="Empty list..."></asp:Label>
        </td>
    </tr>
</table>
