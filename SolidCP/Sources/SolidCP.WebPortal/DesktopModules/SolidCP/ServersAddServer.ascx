<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersAddServer.ascx.cs"
    Inherits="SolidCP.Portal.ServersAddServer" %>
<%@ Register Src="UserControls/ServerPasswordControl.ascx" TagName="ServerPasswordControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<asp:Panel ID="ServersAddServerPanel" runat="server" DefaultButton="btnAdd">
    <div class="panel-body form-horizontal">
        <div class="form-group">
            <asp:Label ID="lblServerName" runat="server" CssClass="control-label col-sm-2" meta:resourcekey="lblServerName" style="font-weight:bold"></asp:Label>
            <div class="col-sm-10">
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control">New Server</asp:TextBox>
                <asp:RequiredFieldValidator ID="valServerName" runat="server" ErrorMessage="*" ControlToValidate="txtName"></asp:RequiredFieldValidator>
            </div>
            <asp:Label ID="lblServerUrl" runat="server" CssClass="control-label col-sm-2" meta:resourcekey="lblServerUrl" style="font-weight:bold"></asp:Label>
            <div class="col-sm-10">
                <asp:TextBox ID="txtUrl" runat="server" CssClass="form-control">http://127.0.0.1:9003</asp:TextBox>
                <asp:RequiredFieldValidator ID="valServerUrl" runat="server" ErrorMessage="*" ControlToValidate="txtUrl"></asp:RequiredFieldValidator>
            </div>
            <asp:Label ID="lblServerPassword" runat="server" CssClass="control-label col-sm-2" meta:resourcekey="lblServerPassword" style="font-weight:bold"></asp:Label>
            <div class="col-sm-10">
                <uc1:ServerPasswordControl id="serverPassword" runat="server"></uc1:ServerPasswordControl>
            </div>
            <div class="col-sm-10 col-md-offset-2">
                <asp:CheckBox runat="server" ID="cbAutoDiscovery" Checked="false" meta:resourcekey="cbAutoDiscovery" />
            </div>
        </div>
    </div>
    <div class="panel-footer text-right">
        <CPCC:StyleButton id="btnCancel" runat="server" CausesValidation="False" CssClass="btn btn-warning" OnClick="btnCancel_Click">
            <i class="fa fa-times">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnCancel"/>
        </CPCC:StyleButton>
        &nbsp;
        <CPCC:StyleButton id="btnAdd" runat="server" ValidationGroup="Server" CssClass="btn btn-success" OnClick="btnAdd_Click" OnClientClick="ShowProgressDialog('Adding New server...');">
            <i class="fa fa-plus">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnAddText"/>
        </CPCC:StyleButton>
    </div>
</asp:Panel>
