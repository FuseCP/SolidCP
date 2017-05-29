<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginForgotPassword.ascx.cs" Inherits="SolidCP.Portal.LoginForgotPassword" %>
<div class="panel-body form-horizontal">
    <div class="form-group">
	<asp:Label ID="lblUsername" runat="server" meta:resourcekey="lblUsername" Text="User name:" CssClass="col-sm-2"></asp:Label>
	<div class="col-sm-10">
				<asp:TextBox id="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
				<asp:RequiredFieldValidator id="usernameValidator" runat="server" ControlToValidate="txtUsername" ErrorMessage="*"
					CssClass="alert alert-warning"></asp:RequiredFieldValidator></div>
        </div>
    </div>
<div class="panel-footer text-right">
	<CPCC:StyleButton id="btnSend" CssClass="btn btn-success" runat="server" OnClick="btnSend_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSendtext"/> </CPCC:StyleButton>
	<CPCC:StyleButton id="cmdBack" runat="server" CssClass="btn btn-default pull-left" CausesValidation="False" OnClick="cmdBack_Click"><i class="fa fa-arrow-left" aria-hidden="true"></i>&nbsp;<asp:Localize runat="server" meta:resourcekey="cmdBack" /> </CPCC:StyleButton>
</div>