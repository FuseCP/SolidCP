<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="SolidCP.Portal.Login" %>

<div class="panel-body form-group" runat="server" id="userPwdDiv">
    <div class="row">
        <div class="col-sm-12">
            <div class="input-group">
                <span class="input-group-addon"><i class="fa fa-user"></i></span>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <asp:RequiredFieldValidator ID="usernameValidator" runat="server" CssClass="form-control" ErrorMessage="*" ControlToValidate="txtUsername"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-12">
            <div class="input-group">
                <span class="input-group-addon"><i class="fa fa-lock"></i></span>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
            </div>
            <asp:RequiredFieldValidator ID="passwordValidator" runat="server" CssClass="NormalBold" ErrorMessage="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-6" style="padding-bottom: 15px;">
            <asp:CheckBox ID="chkRemember" runat="server" meta:resourcekey="chkRemember" Text="Remember me on this computer"></asp:CheckBox>
        </div>
        <div class="col-sm-6">
            <CPCC:StyleButton ID="btnLogin" runat="server" CssClass="btn btn-success pull-right" OnClick="btnLogin_Click">
                <asp:Localize runat="server" meta:resourcekey="btnLogin" />&nbsp;<i class="fa fa-sign-in" aria-hidden="true"></i></CPCC:StyleButton>
        </div>
    </div>
    <hr />
    <div class="row">
        <div class="col-sm-12 ">
            <h5><asp:Localize ID="forgotpass" runat="server" meta:resourcekey="forgotpass" /></h5>
            <p><asp:Localize ID="noworries" runat="server" meta:resourcekey="noworries" />
                <asp:LinkButton ID="cmdForgotPassword" runat="server" CssClass="color-green" CausesValidation="False" OnClick="cmdForgotPassword_Click"><asp:Localize runat="server" meta:resourcekey="cmdForgotPassword" /></asp:LinkButton>
                <asp:Localize ID="toresetyourpassword" runat="server" meta:resourcekey="toresetyourpassword" />.</p>
        </div>
    </div>
</div>
<div class="panel-body form-group" runat="server" id="tokenDiv" visible="false">
    <div class="row">
        <div class="col-sm-12"  style="padding-bottom: 15px;">
            <div class="input-group">
                <span class="input-group-addon"><i class="fa fa-lock"></i></span>
                <asp:TextBox ID="txtPin" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
     <div class="row">
         <div class="col-sm-6">
            <CPCC:StyleButton runat="server" id="StyleButton1" CssClass="btn btn-succsess" OnClick="btnResendPin_Click">
                <asp:Localize runat="server" meta:resourcekey="btResendPin" />
            </CPCC:StyleButton>
        </div>
        <div class="col-sm-6">
            <CPCC:StyleButton ID="StyleButton2" runat="server" CssClass="btn btn-success pull-right" OnClick="btnVerifyPin_Click">
                <asp:Localize runat="server" meta:resourcekey="btnLogin" />&nbsp;<i class="fa fa-sign-in" aria-hidden="true"></i></CPCC:StyleButton>
        </div>
         
    </div>
</div>
<div class="panel-footer">
    <div class="row">
        <div class="col-xs-6">
            <asp:Label ID="lblLanguage" runat="server" meta:resourcekey="lblLanguage" Text="Preferred Language:"></asp:Label>
            <asp:DropDownList ID="ddlLanguage" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="col-xs-6">
            <asp:Label ID="lblTheme" runat="server" meta:resourcekey="lblTheme" Text="Theme:"></asp:Label>
            <asp:DropDownList ID="ddlTheme" runat="server" Width="100%" DataValueField="LTRName" DataTextField="DisplayName" AutoPostBack="True" OnSelectedIndexChanged="ddlTheme_SelectedIndexChanged"></asp:DropDownList>
        </div>
    </div>
</div>
