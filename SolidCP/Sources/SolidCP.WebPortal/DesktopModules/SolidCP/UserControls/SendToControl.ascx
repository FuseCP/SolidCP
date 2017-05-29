<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendToControl.ascx.cs" Inherits="SolidCP.Portal.UserControls.SendToControl" %>
<div id="sendtoheader">
    <div class="container">
        <fieldset>
            <div class="row">
                <div class="col-md-12">
                    <div>
                        <div id="SendPasswordResetDisabledDiv" runat="server" class="alert alert-info" role="alert">
                            <strong>Heads up!</strong> You can send email automatically by enabling Webdav and enabling Organization Password Reset Module in system settings.
                        </div>
                        <div id="SendPasswordResetEmailDiv" class="form-group" runat="server">
                            <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="chkSendPasswordResetEmail">
                                <asp:Localize ID="Localize1" runat="server" meta:resourcekey="chkSendPasswordResetEmailLabel" Text="Send Password Request." />
                            </asp:Label>
                            <div class="col-sm-10">
                                <div class="input-group">
                                    <asp:CheckBox ID="chkSendPasswordResetEmail" runat="server" AutoPostBack="true" OnCheckedChanged="chkSendPasswordResetEmail_StateChanged" CssClass="btn button-primary" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
</div>
<div id="SendToBody" runat="server" visible="False">
    <div class="container">
        <fieldset>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group form-inline">
                        <label for="locSendTo" class="col-sm-2 control-label">
                            <asp:Localize ID="locSendTo" runat="server" meta:resourcekey="locSendTo" Text="Send to:" /></label>
                        </label> 
						<div class="col-sm-10">
                            <div class="input-group form-inline">
                                <div class="FormRBtnL">
                                    <label class="input-group-addon">
                                        <i class="fa fa-envelope-o" aria-hidden="true"></i>
                                        <asp:RadioButton ID="rbtnEmail" runat="server" meta:resourcekey="rbtnEmail" Text="Email" GroupName="SendToGroup" AutoPostBack="true" Checked="true" OnCheckedChanged="SendToGroupCheckedChanged" />
                                    </label>
                                    <label class="input-group-addon">
                                        <i class="fa fa-mobile" aria-hidden="true"></i>
                                        <asp:RadioButton ID="rbtnMobile" runat="server" meta:resourcekey="rbtnMobile" Text="Mobile" GroupName="SendToGroup" AutoPostBack="true" OnCheckedChanged="SendToGroupCheckedChanged" />
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="EmailDiv" runat="server" class="col-md-12">
                    <div class="form-group form-inline">
                        <label for="txtEmailAddress" class="col-sm-2 control-label">
                            <asp:Localize ID="locEmailAddress" runat="server" meta:resourcekey="locEmailAddress" />
                        </label>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <span class="input-group-addon"><i class="fa fa-envelope-o" aria-hidden="true"></i></span>
                                <asp:TextBox runat="server" ID="txtEmailAddress" CssClass="form-control" placeholder="Email Address" />
                            </div>
                            <asp:RequiredFieldValidator ID="valEmailAddress" runat="server" ErrorMessage="*" ControlToValidate="txtEmailAddress" ValidationGroup="ResetUserPassword"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="ResetUserPassword" ControlToValidate="txtEmailAddress" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="MobileDiv" runat="server" class="col-md-12">
                    <div class="form-group form-inline">
                        <label for="txtMobile" class="col-sm-2 control-label">
                            <asp:Localize ID="locMobile" runat="server" meta:resourcekey="locMobile" />
                        </label>
                        <div class="col-sm-10">
                            <div class="input-group">
                                <span class="input-group-addon"><i class="fa fa-mobile" aria-hidden="true"></i></span>
                                <asp:TextBox runat="server" ID="txtMobile" CssClass="form-control col-md-4" placeholder="Cell Phone" />
                            </div>
                            <asp:RequiredFieldValidator ID="valMobile" runat="server" ErrorMessage="*" ControlToValidate="txtMobile" ValidationGroup="ResetUserPassword"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexMobileValid" runat="server" ValidationExpression="^\+?\d+$" ValidationGroup="ResetUserPassword" ControlToValidate="txtMobile" ErrorMessage="Invalid Mobile Format"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
</div>