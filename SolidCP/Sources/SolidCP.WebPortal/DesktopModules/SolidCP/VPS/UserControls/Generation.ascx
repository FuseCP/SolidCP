<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Generation.ascx.cs" Inherits="SolidCP.Portal.VPS.UserControls.Generation" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../../UserControls/CollapsiblePanel.ascx" %>

<scp:CollapsiblePanel ID="secGeneration" runat="server" TargetControlID="GenerationPanel" meta:ResourceKey="secGeneration" Text="Generation"></scp:CollapsiblePanel>
<asp:Panel ID="GenerationPanel" runat="server" Height="0" Style="overflow: hidden; padding: 5px;">
    <table>
        <tr>
            <td class="FormLabel150">
                <asp:Localize ID="locGeneration" runat="server" meta:resourcekey="locGeneration" Text="Generation:"></asp:Localize>
            </td>
            <td>
                <% if (IsEditMode)
                   { %>
                    <asp:DropDownList ID="ddlGeneration" runat="server" CssClass="NormalTextBox" resourcekey="ddlGeneration">
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                    </asp:DropDownList>
                <% } else { %>
                    <asp:Label runat="server" ID="lblGeneration"/>
                <% } %>
            </td>
        </tr>
    </table>
</asp:Panel>
