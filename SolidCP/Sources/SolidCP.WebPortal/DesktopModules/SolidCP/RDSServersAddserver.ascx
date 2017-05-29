<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSServersAddserver.ascx.cs" Inherits="SolidCP.Portal.RDSServersAddserver" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<script type="text/javascript" src="/JavaScript/jquery.min.js?v=1.4.4"></script>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-body form-horizontal">
    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <div class="form-group">
        <asp:Label runat="server" CssClass="control-label col-sm-4" AssociatedControlID="txtServerName">
            <asp:Localize ID="locServerName" runat="server" meta:resourcekey="locServerName" Text="Server Fully Qualified Domain Name:"></asp:Localize>
        </asp:Label>
        <div class="col-sm-8">
            <asp:TextBox ID="txtServerName" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valServerName" runat="server" ErrorMessage="*" ControlToValidate="txtServerName"></asp:RequiredFieldValidator>
        </div>
        <asp:Label runat="server" CssClass="control-label col-sm-4" AssociatedControlID="txtServerComments">
            <asp:Localize ID="locServerComments" runat="server" meta:resourcekey="locServerComments" Text="Server Comments:"></asp:Localize>
        </asp:Label>
        <div class="col-sm-8">
            <asp:TextBox ID="txtServerComments" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click">
        <i class="fa fa-times">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCancel"/>
    </CPCC:StyleButton>
    &nbsp;
	<CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" OnClientClick="ShowProgressDialog('Adding server...');">
        <i class="fa fa-check">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnAddRDSServer"/>
	</CPCC:StyleButton>
</div>