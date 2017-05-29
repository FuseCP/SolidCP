<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IceWarp_EditDomain.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.IceWarp_EditDomain" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>

<table width="100%">
    <tr>
        <td class="SubHead" width="150">
            <asp:Label ID="lblPostMaster" runat="server" meta:resourcekey="lblPostMaster"  Text="Postmaster Account:"></asp:Label></td>
        <td class="Normal">
            <asp:DropDownList ID="ddlPostMasterAccount" runat="server" DataTextField="Name" AppendDataBoundItems="True">
                <asp:ListItem Value="" Text="<Not Selected>" meta:resourcekey="liNotSelected"></asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="SubHead">
            <asp:Label ID="lblCatchAll" runat="server" meta:resourcekey="lblCatchAll" Text="Catch-All Account:"></asp:Label></td>
        <td class="Normal">
            <asp:DropDownList ID="ddlCatchAllAccount" runat="server" DataTextField="Name" AppendDataBoundItems="True">
                <asp:ListItem Value="" Text="None, reject mail for unknown users" meta:resourcekey="liRejectUnknown"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>

<asp:Panel runat="server" ID="AdvancedSettingsPanel">

<scp:CollapsiblePanel id="secLimits" runat="server" targetcontrolid="LimitsPanel"
    meta:resourcekey="Limits" text="Limits"></scp:CollapsiblePanel>

<asp:Panel runat="server" ID="LimitsPanel">
    <table width="100%">
        <tr runat="server" ID="rowMaxDomainDiskSpace">
            <td class="SubHead" width="150">
                <asp:Label ID="lblMaxDomainDiskSpace" runat="server" meta:resourcekey="lblMaxDomainDiskSpace" Text="Domain Disk Space, MB:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox runat="server"  ID="txtMaxDomainDiskSpace" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" ID="MaxDomainDiskSpaceValidator" MinimumValue="0" runat="server" ControlToValidate="txtMaxDomainDiskSpace"
                    Display="Dynamic" meta:resourcekey="ValueMustBeZeroOrGreater" ErrorMessage="Value must be zero or greater" />
                <asp:RequiredFieldValidator runat="server" ID="MaxDomainDiskSpaceRequiredValidator" ControlToValidate="txtMaxDomainDiskSpace"
                    Display="Dynamic" Text="*" />
                <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="150">
                <asp:Label ID="lblMaxDomainUsers" runat="server" meta:resourcekey="lblMaxDomainUsers" Text="Max users:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox runat="server"  ID="txtMaxDomainUsers" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" ID="MaxDomainUsersValidator" MinimumValue="0" runat="server" ControlToValidate="txtMaxDomainUsers"
                    Display="Dynamic" meta:resourcekey="ValueMustBeZeroOrGreater" ErrorMessage="Value must be zero or greater" />
                <asp:RequiredFieldValidator runat="server" ID="MaxDomainUsersRequiredValidator" ControlToValidate="txtMaxDomainUsers" Display="Dynamic" Text="*" />
                <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
            </td>
        </tr>
        <tbody runat="server" ID="rowDomainLimits">
        <tr>
            <td class="SubHead" width="150">
                <asp:Label ID="lblLimitVolume" runat="server" meta:resourcekey="lblLimitVolume" Text="Send out data limit per day, MB:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox runat="server"  ID="txtLimitVolume" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" ID="txtLimitVolumeValidator" MinimumValue="0" runat="server" ControlToValidate="txtLimitVolume"
                    Display="Dynamic" meta:resourcekey="ValueMustBeZeroOrGreater" ErrorMessage="Value must be zero or greater" />
                <asp:RequiredFieldValidator runat="server" ID="txtLimitVolumeRequiredValidator" ControlToValidate="txtLimitVolume"
                    Display="Dynamic" ErrorMessage="*" />
                <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblLimitNumber" runat="server" meta:resourcekey="lblLimitNumber" Text="Send out messages limit (#/Day):"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox runat="server"  ID="txtLimitNumber" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" ID="txtLimitNumberValidator" MinimumValue="0" runat="server" ControlToValidate="txtLimitNumber"
                    Display="Dynamic" meta:resourcekey="ValueMustBeZeroOrGreater" ErrorMessage="Value must be zero or greater" />
                <asp:RequiredFieldValidator runat="server" ID="txtLimitNumberRequiredValidator" ControlToValidate="txtLimitNumber" Display="Dynamic" Text="*" />
                <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
            </td>
        </tr>
        </tbody>
        <tbody runat="server" ID="rowUserLimits">
        <tr>
            <td class="SubHead" width="150">
                <asp:Label ID="lblDefaultUserQuotaInMB" runat="server" meta:resourcekey="lblDefaultUserQuotaInMB" Text="Default user quota, MB:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox runat="server" ID="txtDefaultUserQuotaInMB" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" ID="txtDefaultUserQuotaInMBValidator" MinimumValue="0" runat="server" ControlToValidate="txtDefaultUserQuotaInMB"
                    Display="Dynamic" meta:resourcekey="ValueMustBeZeroOrGreater" ErrorMessage="Value must be zero or greater" />
                <asp:RequiredFieldValidator runat="server" ID="txtDefaultUserQuotaInMBRequiredValidator" ControlToValidate="txtDefaultUserQuotaInMB"
                    Display="Dynamic" ErrorMessage="*" />
                <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblDefaultUserMaxMessageSizeMegaByte" runat="server" meta:resourcekey="lblDefaultUserMaxMessageSizeMegaByte" Text="Default user max message size, MB:"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox runat="server"  ID="txtDefaultUserMaxMessageSizeMegaByte" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" ID="txtDefaultUserMaxMessageSizeMegaByteValidator" MinimumValue="0" runat="server" ControlToValidate="txtDefaultUserMaxMessageSizeMegaByte"
                    Display="Dynamic" meta:resourcekey="ValueMustBeZeroOrGreater" ErrorMessage="Value must be zero or greater" />
                <asp:RequiredFieldValidator runat="server" ID="txtDefaultUserMaxMessageSizeMegaByteRequiredValidator" ControlToValidate="txtDefaultUserMaxMessageSizeMegaByte"
                    Display="Dynamic" ErrorMessage="*" />
                <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblDefaultUserMegaByteSendLimit" runat="server" meta:resourcekey="lblDefaultUserMegaByteSendLimit" Text="Default user send out data limit per day, MB:"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox runat="server"  ID="txtDefaultUserMegaByteSendLimit" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" ID="txtDefaultUserMegaByteSendLimitValidator" MinimumValue="0" runat="server" ControlToValidate="txtDefaultUserMegaByteSendLimit"
                    Display="Dynamic" meta:resourcekey="ValueMustBeZeroOrGreater" ErrorMessage="Value must be zero or greater" />
                <asp:RequiredFieldValidator runat="server" ID="txtDefaultUserMegaByteSendLimitRequiredValidator" ControlToValidate="txtDefaultUserMegaByteSendLimit"
                    Display="Dynamic" ErrorMessage="*" />
                <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
            </td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblDefaultUserNumberSendLimit" runat="server" meta:resourcekey="lblDefaultUserNumberSendLimit" Text="Default user send out messages limit (#/Day):"></asp:Label></td>
            <td class="Normal">
                <asp:TextBox runat="server"  ID="txtDefaultUserNumberSendLimit" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" ID="txtDefaultUserNumberSendLimitValidator" MinimumValue="0" runat="server" ControlToValidate="txtDefaultUserNumberSendLimit"
                    Display="Dynamic" meta:resourcekey="ValueMustBeZeroOrGreater" ErrorMessage="Value must be zero or greater" />
                <asp:RequiredFieldValidator runat="server" ID="txtDefaultUserNumberSendLimitRequiredValidator" ControlToValidate="txtDefaultUserNumberSendLimit" Display="Dynamic" Text="*" />
                <asp:Label runat="server" meta:resourcekey="lblZeroIsUnlimited" />
            </td>
        </tr>
        </tbody>
    </table>
</asp:Panel>

</asp:Panel>