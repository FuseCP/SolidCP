<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Generation.ascx.cs" Inherits="SolidCP.Portal.Proxmox.UserControls.Generation" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="../../UserControls/CollapsiblePanel.ascx" %>

<% if (Mode != VirtualMachineSettingsMode.Summary){ %>
    <wsp:CollapsiblePanel ID="secGeneration" runat="server" TargetControlID="GenerationPanel" meta:ResourceKey="secGeneration" Text="Generation"></wsp:CollapsiblePanel>
    <asp:Panel ID="GenerationPanel" runat="server" Height="0" Style="overflow: hidden; padding: 5px;">
        <table>
            <% if (Mode == VirtualMachineSettingsMode.Edit) { %>
            <tr>
                <td class="FormLabel150">
                    <asp:Localize ID="locGeneration" runat="server" meta:resourcekey="locGeneration" Text="Generation:"></asp:Localize>
                </td>
                <td>
                    <asp:DropDownList ID="ddlGeneration" runat="server" CssClass="NormalTextBox" resourcekey="ddlGeneration">
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <% } else { %>
            <tr>
                <td>
                    <asp:Localize ID="locGenerationDisplay" runat="server" meta:resourcekey="locGeneration" Text="Generation:"></asp:Localize>
                    <asp:Label runat="server" ID="lblGeneration"/>
                </td>
            </tr>
            <% } %>
        </table>
    </asp:Panel>
<% } else { %>
    <tr>
        <td><asp:Localize ID="locGeneration2" runat="server" meta:resourcekey="locGeneration" Text="Generation:" /></td>
        <td><asp:Literal ID="litGeneration" runat="server"></asp:Literal></td>
    </tr>
<% } %>
