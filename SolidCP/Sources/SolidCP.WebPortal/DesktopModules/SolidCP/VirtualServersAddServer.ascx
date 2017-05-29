<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VirtualServersAddServer.ascx.cs" Inherits="SolidCP.Portal.VirtualServersAddServer" %>
<div class="panel-body form-horizontal">
    <div class="form-group">
        <asp:Label ID="lblServerName" runat="server" CssClass="control-label col-sm-2" meta:resourcekey="lblServerName" style="font-weight:bold"></asp:Label>
        <div class="col-sm-10">
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control">New Server</asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireServerName" runat="server" ControlToValidate="txtName" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label ID="lblServerComments" runat="server" CssClass="control-label col-sm-2" meta:resourcekey="lblServerComments" style="font-weight:bold"></asp:Label>
        <div class="col-sm-10">
            <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" Rows="4" TextMode="MultiLine"></asp:TextBox>
        </div>
    </div>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
        <i class="fa fa-times">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCancel"/>
    </CPCC:StyleButton>
    &nbsp;
    <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click">
        <i class="fa fa-check">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnAdd"/>
    </CPCC:StyleButton>
    &nbsp;
</div>