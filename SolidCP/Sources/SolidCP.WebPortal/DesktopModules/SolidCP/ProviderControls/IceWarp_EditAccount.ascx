<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IceWarp_EditAccount.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.IceWarp_EditAccount" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register TagPrefix="scp" TagName="Calendar" Src="../UserControls/CalendarControl.ascx" %>

<table width="100%">
    <tr>
        <td colspan="2">
            <asp:CheckBox runat="server" meta:resourcekey="cbDomainAdmin" ID="cbDomainAdmin" Text="Domain Administrator" />
        </td>
    </tr>   
    <tr>
        <td class="SubHead" width="150">
            <asp:Label ID="lblAccountType" runat="server" meta:resourcekey="lblAccountType" Text="Account type:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:DropDownList ID="ddlAccountType" runat="server">
                <asp:ListItem Text="POP3" Value="0" />
                <asp:ListItem Text="POP3 & IMAP" Value="1" />
                <asp:ListItem Text="IMAP" Value="2" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <asp:Label ID="lblAccountState" runat="server" meta:resourcekey="lblAccountState" Text="Account state:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:DropDownList ID="ddlAccountState" runat="server">
                <asp:ListItem Text="Enabled" meta:resourcekey="ddlAccountStateEnabled" Value="0" />
                <asp:ListItem Text="Disabled (Login)" meta:resourcekey="ddlAccountStateDisabledLogin" Value="1" />
                <asp:ListItem Text="Disabled (Login, Receive)" meta:resourcekey="ddlAccountStateDisabledLoginReceive" Value="2" />
                <asp:ListItem Text="Disabled (Tarpitting)" meta:resourcekey="ddlAccountStateDisabledTarpitting" Value="3" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="SubHead">
            <asp:Label ID="lblFullName" runat="server" meta:resourcekey="lblFullName" Text="Full Name:"></asp:Label>
        </td>
        <td class="Normal">
            <asp:TextBox ID="txtFullName" runat="server" CssClass="TextBox400"></asp:TextBox>
        </td>
    </tr>
</table>


<scp:CollapsiblePanel id="secAutoresponder" runat="server" TargetControlID="AutoresponderPanel"
    meta:resourcekey="Autoresponder" Text="Autoresponder">
</scp:CollapsiblePanel>

<asp:Panel ID="AutoresponderPanel" runat="server">
    <asp:UpdatePanel ID="AutoresponderUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%">
            <tr>
                <td class="SubHead" width="150">
                    <asp:Label ID="lblResponderEnabled" runat="server" meta:resourcekey="lblResponderEnabled" Text="Responder type:"></asp:Label>
                </td>
                <td class="Normal">
                    <asp:DropDownList ID="ddlRespondType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRespondType_SelectedIndexChanged">
                        <asp:ListItem Text="Disabled" meta:resourcekey="ddlRespondDisabled" Value="0" />
                        <asp:ListItem Text="Respond always (can cause loop)" meta:resourcekey="ddlRespondAlways" Value="1" />
                        <asp:ListItem Text="Respond once" meta:resourcekey="ddlRespondOnce" Value="2" />
                        <asp:ListItem Text="Respond once in period" meta:resourcekey="ddlRespondOnceInPeriod" Value="3" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tbody runat="server" ID="RespondPeriod">
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblRespondPeriodInDays" runat="server" meta:resourcekey="lblRespondPeriodInDays" Text="Respond period in days:"></asp:Label>
                </td>
                <td class="Normal">
                    <asp:TextBox ID="txtRespondPeriodInDays" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRespondPeriodInDays" ErrorMessage="*"></asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="RespondPeriodInDaysValidator" runat="server" ControlToValidate="txtRespondPeriodInDays" MinimumValue="0" MaximumValue="63" Type="Integer" ErrorMessage="Respond days must be between 0 and 63 days" meta:resourcekey="RespondPeriodInDaysValidator"></asp:RangeValidator>
                </td>
            </tr>
            </tbody>
            <tbody runat="server" ID="RespondEnabled">
            <tr>
                <td colspan="2">
                    <asp:CheckBox Text="Only respond between these dates" ID="chkRespondOnlyBetweenDates" meta:resourcekey="chkRespondOnlyBetweenDates" runat="server" AutoPostBack="True" OnCheckedChanged="chkRespondOnlyBetweenDates_CheckedChanged" />
                </td>
            </tr>
            <tr runat="server" ID="RespondFrom">
                <td class="SubHead">
                    <asp:Label ID="lblRespondFrom" runat="server" meta:resourcekey="lblRespondFrom" Text="Respond from:"></asp:Label></td>
                <td class="Normal">
                    <scp:Calendar ID="calRespondFrom" runat="server"></scp:Calendar>
                </td>
            </tr>
            <tr runat="server" ID="RespondTo">
                <td class="SubHead">
                    <asp:Label ID="lblRespondTo" runat="server" meta:resourcekey="lblRespondTo" Text="Respond to:"></asp:Label></td>
                <td class="Normal">
                    <scp:Calendar ID="calRespondTo" runat="server"></scp:Calendar>
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblSubject" runat="server" meta:resourcekey="lblSubject" Text="Subject:"></asp:Label></td>
                <td class="Normal">
                    <asp:TextBox ID="txtSubject" runat="server" CssClass="TextBox400"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblRespondWithReplyFrom" runat="server" meta:resourcekey="lblRespondWithReplyFrom" Text="Respond with reply from:"></asp:Label>
                </td>
                <td class="Normal">
                    <asp:TextBox ID="txtRespondWithReplyFrom" runat="server" CssClass="TextBox400"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblMessage" runat="server" meta:resourcekey="lblMessage" Text="Message:"></asp:Label>
                </td>
                <td class="Normal">
                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="5" CssClass="TextBox400"></asp:TextBox>
                </td>
            </tr>
            </tbody>
        </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<scp:CollapsiblePanel id="secForwarding" runat="server" TargetControlID="ForwardingPanel" meta:resourcekey="Forwarding" Text="Mail Forwarding">
</scp:CollapsiblePanel>
<asp:Panel ID="ForwardingPanel" runat="server">
    <table width="100%">
        <tr>
            <td class="SubHead" width="150">
                <asp:Label ID="lblForwardTo" runat="server" meta:resourcekey="lblForwardTo" Text="Forward mail to address:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox ID="txtForward" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="cbDeleteOnForward" runat="server" meta:resourcekey="cbDeleteOnForward" Text="Delete Message on Forward"></asp:CheckBox>
            </td>
        </tr>
    </table>
</asp:Panel>
<scp:CollapsiblePanel id="secOlderMails" runat="server" TargetControlID="OlderMailsPanel"
    meta:resourcekey="OlderMails" Text="Handle older mails">
</scp:CollapsiblePanel>
<asp:Panel ID="OlderMailsPanel" runat="server">
    <asp:UpdatePanel ID="DeleteOlderUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <table width="100%">
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="cbDeleteOlder" runat="server" meta:resourcekey="cbDeleteOlder" AutoPostBack="True" OnCheckedChanged="cbDeleteOlder_CheckedChanged"
                    Text="Enable delete of older messages"></asp:CheckBox>
            </td>
        </tr>
        <tbody runat="server" id="DeleteOlderEnabled">
        <tr>
            <td class="SubHead" width="150">
                <asp:Label ID="lblDeleteOlderDays" runat="server" meta:resourcekey="lblDeleteOlderDays" Text="Delete older than (days):"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox ID="txtDeleteOlderDays" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        </tbody>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="ForwardOlderUpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <table width="100%">
        <tr>
            <td colspan="2">
                <asp:CheckBox ID="cbForwardOlder" runat="server" meta:resourcekey="cbForwardOlder" AutoPostBack="True" OnCheckedChanged="cbForwardOlder_CheckedChanged"
                    Text="Enable forwarding of older messages"></asp:CheckBox>
            </td>
        </tr>
        <tbody runat="server" id="ForwardOlderEnabled">
        <tr>
            <td class="SubHead" width="150">
                <asp:Label ID="lblForwardOlderDays" runat="server" meta:resourcekey="lblForwardOlderDays" Text="Forward older than (days):"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox ID="txtForwardOlderDays" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblForwardOlderTo" runat="server" meta:resourcekey="lblForwardOlderTo" Text="Forward to:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox ID="txtForwardOlderTo" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
        </tr>
        </tbody>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
