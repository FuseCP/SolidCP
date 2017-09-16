<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSettingsPreviewDomain.ascx.cs" Inherits="SolidCP.Portal.SpaceSettingsPreviewDomain" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel id="secPreviewDomain" runat="server"
    TargetControlID="PreviewDomainPanel" meta:resourcekey="secPreviewDomain" Text="Preview Domain"/>
<asp:Panel ID="PreviewDomainPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" nowrap>
                <asp:Label ID="lblPreviewDomain" runat="server" meta:resourcekey="lblPreviewDomain" Text="Preview Domain:"></asp:Label>
            </td>
            <td class="NormalBold">
                <div class="form-inline">
                domain.com.&nbsp;<asp:TextBox ID="txtPreviewDomain" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>