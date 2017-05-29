<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InstallerInstallApplicationComplete.ascx.cs" Inherits="SolidCP.Portal.InstallerInstallApplicationComplete" %>
<%@ Register Src="installerapplicationheader.ascx" TagName="ApplicationHeader" TagPrefix="dnc" %>

<div class="panel-body form-horizontal">
    <dnc:applicationheader id="installerapplicationheader" runat="server"></dnc:applicationheader>
    <br />
    <br />
		<asp:PlaceHolder ID="completePanel" runat="server"></asp:PlaceHolder>
    <br />
    <br />
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnReturn" CssClass="btn btn-success" runat="server" OnClick="btnReturn_Click"> <i class="fa fa-arrow-left">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnReturnText"/> </CPCC:StyleButton>
</div>