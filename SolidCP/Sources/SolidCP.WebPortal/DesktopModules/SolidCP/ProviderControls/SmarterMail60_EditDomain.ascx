<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SmarterMail60_EditDomain.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.SmarterMail60_EditDomain" %>
<%@ Register Src="SmarterMail60_EditDomain_Features.ascx" TagName="SmarterMail60_EditDomain_Features"
    TagPrefix="uc4" %>
<%@ Register Src="SmarterMail60_EditDomain_Sharing.ascx" TagName="SmarterMail60_EditDomain_Sharing"
    TagPrefix="uc3" %>
<%@ Register Src="SmarterMail60_EditDomain_Throttling.ascx" TagName="SmarterMail60_EditDomain_Throttling"
    TagPrefix="uc5" %>

<%@ Register Src="../UserControls/QuotaEditor.ascx" TagName="QuotaEditor" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>


<table width="100%">
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label ID="lblCatchAll" runat="server" meta:resourcekey="lblCatchAll" Text="Catch-All Account:"></asp:Label></td>
        <td class="Normal" width="100%">
            <asp:DropDownList ID="ddlCatchAllAccount" runat="server" CssClass="form-control">
            </asp:DropDownList></td>
    </tr>
</table>
<asp:Panel runat="server" ID="AdvancedSettingsPanel">



<scp:collapsiblepanel id="secFeatures" runat="server" targetcontrolid="FeaturesPanel"
    meta:resourcekey="secFeatures" ></scp:collapsiblepanel>
<asp:Panel runat="server" ID="FeaturesPanel">
    <uc4:SmarterMail60_EditDomain_Features id="featuresSection" runat="server"></uc4:SmarterMail60_EditDomain_Features>

</asp:Panel>


<scp:collapsiblepanel id="secSharing" runat="server" targetcontrolid="SharingPanel"
    meta:resourcekey="secSharing" ></scp:collapsiblepanel>

<asp:Panel runat="server" ID="SharingPanel">
    <uc3:SmarterMail60_EditDomain_Sharing id="sharingSection" runat="server"></uc3:SmarterMail60_EditDomain_Sharing>
</asp:Panel>

<scp:collapsiblepanel id="secThrottling" runat="server" targetcontrolid="ThrottlingPanel"
    meta:resourcekey="secThrottling" ></scp:collapsiblepanel>

<asp:Panel runat="server" ID="ThrottlingPanel">
    <uc5:SmarterMail60_EditDomain_Throttling id="throttlingSection" runat="server"></uc5:SmarterMail60_EditDomain_Throttling>
</asp:Panel>


<scp:collapsiblepanel id="secLimits" runat="server" targetcontrolid="LimitsPanel"
    meta:resourcekey="secLimits" text="Limits"></scp:collapsiblepanel>

<asp:Panel runat="server" ID="LimitsPanel">
    <table width="100%">
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblDomainDiskSpace" runat="server" meta:resourcekey="lblDomainDiskSpace"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server"  ID="txtSize" Text="0" Width="80px" CssClass="form-control" />
                <asp:RangeValidator Type="Integer" ID="valDomainDiskSpace" MinimumValue="0" runat="server" ControlToValidate="txtSize"
                    Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValDiskSpace" runat="server" ControlToValidate="txtSize"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblDomainAliases" runat="server" meta:resourcekey="lblDomainAliases"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" ID="txtDomainAliases" Text="0" Width="80px" CssClass="form-control"/>
                <asp:RangeValidator  Type="Integer" ID="valDomainAliases" MinimumValue="0" runat="server" ControlToValidate="txtDomainAliases"
                    Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValDomainAliases" runat="server" ControlToValidate="txtDomainAliases"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblUserQuota" runat="server" meta:resourcekey="lblUserQuota"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" ID="txtUser" Width="80px" CssClass="form-control"/>
                <asp:RangeValidator Type="Integer" MinimumValue="0" ID="valUser" runat="server" ControlToValidate="txtUser"
                    Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValUser" runat="server" ControlToValidate="txtUser"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblUserAliasesQuota" runat="server" meta:resourcekey="lblUserAliasesQuota"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" ID="txtUserAliases" Width="80px" CssClass="form-control"/>
                <asp:RangeValidator Type="Integer" runat="server" ID="valUserAliases" ControlToValidate="txtUserAliases"
                    MinimumValue="0" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValUserAliases" runat="server" ControlToValidate="txtUserAliases"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblMailingListsQuota" runat="server" meta:resourcekey="lblMailingListsQuota"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" Width="80px" ID="txtMailingLists" CssClass="form-control"/>
                <asp:RangeValidator Type="Integer" runat="server" ID="valMailingLists" ControlToValidate="txtMailingLists"
                    MinimumValue="0" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValMailingLists" runat="server" ControlToValidate="txtMailingLists"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblPopRetreivalAccounts" runat="server" meta:resourcekey="lblPopRetreivalAccounts"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" Width="80px" ID="txtPopRetreivalAccounts" CssClass="form-control"/>
            <asp:RangeValidator Type="Integer" runat="server" ID="valPopRetreivalAccounts" ControlToValidate="txtPopRetreivalAccounts"
                    MinimumValue="0" Display="None"/>
            <asp:RequiredFieldValidator ID="reqPopRetreivalAccounts" runat="server" ControlToValidate="txtPopRetreivalAccounts"
                    Display="None"/>
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap  align="right">
                <asp:Label ID="lblMessageSizeQuota" runat="server" meta:resourcekey="lblMessageSizeQuota"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" ID="txtMessageSize" CssClass="form-control" Width="80px"/>
                <asp:RangeValidator Type="Integer" runat="server" ID="valMessageSize" ControlToValidate="txtMessageSize"
                    MinimumValue="0" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValMessageSize" runat="server" ControlToValidate="txtMessageSize"
                    Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="200" nowrap align="right">
                <asp:Label ID="lblRecipientsPerMessageQuota" runat="server" meta:resourcekey="lblRecipientsPerMessageQuota"></asp:Label></td>
            <td width="100%" align="left">
                <asp:TextBox runat="server" ID="txtRecipientsPerMessage" CssClass="form-control" Width="80px"/>
                <asp:RangeValidator Type="Integer" runat="server" ID="valRecipientsPerMessage" ControlToValidate="txtRecipientsPerMessage"
                    MinimumValue="0" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="reqValRecipientsPerMessage" runat="server" ControlToValidate="txtRecipientsPerMessage"
                    Display="Dynamic" />
            </td>
        </tr>
    </table>
</asp:Panel>

</asp:Panel>