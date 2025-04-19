<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailAccessEditAccess.ascx.cs" Inherits="SolidCP.Portal.MailAccessEditAccess" %>

<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="scp" %>


<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<div class="panel-body form-horizontal">
    <div class="Huge">
        <asp:Literal ID="litDomainName" runat="server"></asp:Literal>
    </div>
    <div class="panel-body form-horizontal">
        <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
    </div>
</div>

<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnUpdate" CssClass="btn btn-success" runat="server" OnClick="btnUpdate_Click" OnClientClick="ShowProgressDialog('Updating Domain Settings...');"> <i class="fa fa-refresh">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnUpdateText"/> </CPCC:StyleButton>
</div>