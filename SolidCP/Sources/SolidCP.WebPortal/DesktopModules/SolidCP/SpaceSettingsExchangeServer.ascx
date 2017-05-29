<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSettingsExchangeServer.ascx.cs" Inherits="SolidCP.Portal.SpaceSettingsExchangeServer" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="secTempDomain" runat="server"
    TargetControlID="TempDomainPanel" meta:resourcekey="secTempDomain" Text="Temporary Exchange Domain Name"/>
<asp:Panel ID="TempDomainPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" nowrap>
                <asp:Label ID="lblTempDomain" runat="server" meta:resourcekey="lblTempDomain" Text="Temporary Domain Name:"></asp:Label>
            </td>
            <td class="NormalBold">
                &lt;organization_id&gt;.&nbsp;<asp:TextBox ID="txtTempDomain" runat="server" CssClass="form-control" Width="200px"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Panel>