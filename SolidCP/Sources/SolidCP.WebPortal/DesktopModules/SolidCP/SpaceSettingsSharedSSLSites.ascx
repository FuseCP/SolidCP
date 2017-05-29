<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSettingsSharedSSLSites.ascx.cs" Inherits="SolidCP.Portal.SpaceSettingsSharedSSLSites" %>
<%@ Register Src="UserControls/EditDomainsList.ascx" TagName="EditDomainsList" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="tblSharedSslSites" runat="server"
    TargetControlID="SharedSslSitesPanel" meta:resourcekey="secSharedSslSites" Text="Shared SSL Web Sites"/>
<asp:Panel ID="SharedSslSitesPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblSharedSslSites" runat="server" meta:resourcekey="lblSharedSslSites" Text="Shared SSL Sites:"></asp:Label>
            </td>
            <td class="Normal">
                <uc1:EditDomainsList id="domainsList" runat="server" DisplayNames="false">
                </uc1:EditDomainsList>
            </td>
        </tr>
    </table>
</asp:Panel>