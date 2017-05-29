<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserActions.ascx.cs" Inherits="SolidCP.Portal.UserActions" %>
<%@ Register Src="../ExchangeServer/UserControls/MailboxPlanSelector.ascx" TagName="MailboxPlanSelector" TagPrefix="scp" %>

<style type="text/css">
    .accounts-without-phone-list li {
        list-style-type: circle;
        margin-left: 10px;
    }
</style>

<script type="text/javascript">
    function CloseAndShowProgressDialog(text) {
        $(".Popup").hide();
        return ShowProgressDialog(text);
    }

    function ShowProgress(btn) {
        var action = $(btn).prev().val();

        if (action == 1) {
            ShowProgressDialog('Disabling users...');
        } else if (action == 2) {
            ShowProgressDialog('Enabling users...');
        } else if (action == 3) {
            ShowProgressDialog('Prepare...');
        } else if (action == 4) {
            ShowProgressDialog('Setting VIP...');
        } else if (action == 5) {
            ShowProgressDialog('Unsetting VIP...');
        } else if (action == 7) {
            ShowProgressDialog('Sending password reset notification...');
        } else if (action == 8) {
            ShowProgressDialog('Sending password reset notification...');
        }

    }
</script>
<asp:UpdatePanel ID="tblActions" runat="server" CssClass="NormalBold" UpdateMode="Conditional" ChildrenAsTriggers="true" >
    <ContentTemplate>
        <div class="input-group">
        <asp:DropDownList ID="ddlUserActions" runat="server" CssClass="form-control" resourcekey="ddlUserActions" 
            AutoPostBack="True">
            <asp:ListItem Value="0">Actions</asp:ListItem>
            <asp:ListItem Value="1">Disable</asp:ListItem>
            <asp:ListItem Value="2">Enable</asp:ListItem>
            <asp:ListItem Value="3">SetServiceLevel</asp:ListItem>
            <asp:ListItem Value="4">SetVIP</asp:ListItem>
            <asp:ListItem Value="5">UnsetVIP</asp:ListItem>
            <asp:ListItem Value="6">SetMailboxPlan</asp:ListItem>
            <asp:ListItem Value="7">SendBySms</asp:ListItem>
            <asp:ListItem Value="8">SendByEmail</asp:ListItem>
        </asp:DropDownList>
            <div class="input-group-btn">
        <CPCC:StyleButton id="btnApply" CssClass="btn btn-primary" runat="server" OnClick="btnApply_Click" OnClientClick="return ShowProgress(this);"><asp:Localize runat="server" meta:resourcekey="btnApplyText"/> </CPCC:StyleButton>
        </div></div>

        
        <ajaxToolkit:ModalPopupExtender ID="Modal" runat="server" EnableViewState="true" TargetControlID="FakeModalPopupTarget"
             PopupControlID="FakeModalPopupTarget" BackgroundCssClass="modalBackground" DropShadow="false" />
        
        <%--Set Service Level--%>
        <asp:Panel ID="ServiceLevelPanel" runat="server" Style="display: none">
            <div class="widget">
             <div class="widget-header clearfix">
                           <h3><i class="fa fa-shield"></i>  <asp:Localize ID="headerServiceLevel" runat="server" meta:resourcekey="headerServiceLevel"></asp:Localize></h3>
                 </div>
                    <div class="widget-content Popup">
                    <asp:Literal ID="litServiceLevel" runat="server" meta:resourcekey="litServiceLevel"></asp:Literal>
                    <br/>
                    <asp:DropDownList ID="ddlServiceLevels" runat="server" CssClass="form-control" />
                    </div>
					<div class="popup-buttons text-right">
                    <CPCC:StyleButton id="btnServiceLevelCancel" CssClass="btn btn-warning" runat="server" OnClick="btnModalCancel_OnClick" CausesValidation="false"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnServiceLevelCancelText"/> </CPCC:StyleButton>&nbsp;
                    <CPCC:StyleButton id="btnServiceLevelOk" CssClass="btn btn-success" runat="server" OnClick="btnModalOk_Click" OnClientClick="return CloseAndShowProgressDialog('Setting Service Level...')"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnServiceLevelOkText"/> </CPCC:StyleButton>
                </div>
            </div>
        </asp:Panel>

        <%--Set MailboxPlan--%>
        <asp:Panel ID="MailboxPlanPanel" runat="server" Style="display: none">
            <div class="widget">
             <div class="widget-header clearfix">
                           <h3><i class="fa fa-envelope-o"></i>  <asp:Localize ID="headerMailboxPlanLabel" runat="server" meta:resourcekey="headerMailboxPlanLabel"></asp:Localize></h3>
                   </div>
                    <div class="widget-content Popup">
                    <asp:Literal ID="litMailboxPlan" runat="server" meta:resourcekey="litMailboxPlan"></asp:Literal>
                    <br/>
                    <scp:MailboxPlanSelector ID="mailboxPlanSelector" runat="server" />
                    </div>
					<div class="popup-buttons text-right">
                    <CPCC:StyleButton id="btnMailboxPlanCancel" CssClass="btn btn-warning" runat="server" OnClick="btnModalCancel_OnClick" CausesValidation="false"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnMailboxPlanCancelText"/> </CPCC:StyleButton>&nbsp;
                    <CPCC:StyleButton id="btnMailboxPlanOk" CssClass="btn btn-success" runat="server" OnClick="btnModalOk_Click" OnClientClick="return CloseAndShowProgressDialog('Setting Mailbox Plan ...')"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnMailboxPlanOkText"/> </CPCC:StyleButton>
                </div>
            </div>
        </asp:Panel>
        
        <%--Send password reset notification--%>
        <asp:Panel ID="PasswordResetNotificationPanel" runat="server" Style="display: none">
            <div class="widget">
             <div class="widget-header clearfix">
                           <h3><i class="fa fa-key"></i>  <asp:Localize ID="headerPasswordResetSendBySms" runat="server" meta:resourcekey="headerPasswordResetSendBySms"></asp:Localize><h3 />
                    </div>
                    <div class="widget-content Popup">
                    <asp:Literal ID="litAccountsWithoutPhone" runat="server" meta:resourcekey="litAccountsWithoutPhone"></asp:Literal>
                    <br/>
                    <ul class="accounts-without-phone-list">
                        <asp:Repeater runat="server" ID="repAccountsWithoutPhone">
                            <ItemTemplate>
                                <li> <%# Eval("DisplayName") %> </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                     </div>
					<div class="popup-buttons text-right">
                    <CPCC:StyleButton id="btnPasswordResetNotificationSendCancel" CssClass="btn btn-warning" runat="server" OnClick="btnModalCancel_OnClick" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnPasswordResetNotificationSendCancelText"/> </CPCC:StyleButton>&nbsp;
                    <CPCC:StyleButton id="btnPasswordResetNotificationSend" CssClass="btn btn-success" runat="server" OnClick="btnModalOk_Click" OnClientClick="return CloseAndShowProgressDialog('Sending password reset notification ...')"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnPasswordResetNotificationText"/> </CPCC:StyleButton>
                </div>
            </div>
        </asp:Panel>

        
        <asp:Button ID="FakeModalPopupTarget" runat="server" Style="display: none;" />
    </ContentTemplate>
    
    <Triggers>
        <asp:PostBackTrigger ControlID="btnServiceLevelOk" />
        <asp:PostBackTrigger ControlID="btnMailboxPlanOk" />
        <asp:PostBackTrigger ControlID="btnPasswordResetNotificationSend" />
        <asp:PostBackTrigger ControlID="btnApply" />
    </Triggers>
</asp:UpdatePanel>
