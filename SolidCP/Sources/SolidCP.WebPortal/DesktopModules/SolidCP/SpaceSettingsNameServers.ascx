<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSettingsNameServers.ascx.cs" Inherits="SolidCP.Portal.SpaceSettingsNameServers" %>
<%@ Register Src="UserControls/EditDomainsList.ascx" TagName="EditDomainsList" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="secNameServers" runat="server"
    TargetControlID="NameServersPanel" meta:resourcekey="secNameServers" Text="Name Servers"/>
<asp:Panel ID="NameServersPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblNameServers" runat="server" meta:resourcekey="lblNameServers" Text="Name Servers:"></asp:Label>
            </td>
            <td class="Normal">
                <uc1:EditDomainsList id="domainsList" runat="server">
                </uc1:EditDomainsList>
            </td>
        </tr>
    </table>
</asp:Panel>