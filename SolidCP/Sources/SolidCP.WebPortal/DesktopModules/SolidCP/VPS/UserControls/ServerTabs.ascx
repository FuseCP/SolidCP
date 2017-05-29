<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServerTabs.ascx.cs"
    Inherits="SolidCP.Portal.VPS.UserControls.ServerTabs" %>
<%@ Register Src="../../UserControls/Gauge.ascx" TagName="Gauge" TagPrefix="scp" %>
<%@ Register Src="../../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>

<asp:Timer runat="server" Interval="3000" ID="refreshTimer" />
<asp:UpdatePanel ID="TabsPanel" runat="server">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="refreshTimer" EventName="Tick" />
    </Triggers>
    <ContentTemplate>
    
        <table id="TaskTable" runat="server" visible="false">
            <tr>
                <td>
                    <asp:Literal ID="litTaskName" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>
                    <asp:Localize ID="locStarted" runat="server" Text="Started:" meta:resourcekey="locStarted"></asp:Localize>
                    <asp:Literal ID="litStarted" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Localize ID="locElapsed" runat="server" Text="Elapsed:" meta:resourcekey="locElapsed"></asp:Localize>
                    <asp:Literal ID="litElapsed" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>
                    <asp:Repeater ID="repRecords" runat="server" 
                        onitemdatabound="repRecords_ItemDataBound">
                        <ItemTemplate>
                            <div style="padding: 2px;">
                                <asp:Literal ID="litRecord" runat="server"></asp:Literal>
                                <scp:Gauge id="gauge" runat="server" OneColour="true" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>

        <table id="TabsTable" runat="server" width="100%" cellpadding="0" cellspacing="1" visible="false">
            <tr>
                <td class="Tabs">
                    <asp:DataList ID="dlTabs" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                        EnableViewState="false">
                        <ItemStyle Wrap="False" />
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkTab" runat="server" CssClass="Tab" NavigateUrl='<%# Eval("Url") %>'>
                                <%# Eval("Name") %>
                            </asp:HyperLink>
                        </ItemTemplate>
                        <SelectedItemStyle Wrap="False" />
                        <SelectedItemTemplate>
                            <asp:HyperLink ID="lnkSelTab" runat="server" CssClass="ActiveTab" NavigateUrl='<%# Eval("Url") %>'>
                                <%# Eval("Name") %>
                            </asp:HyperLink>
                        </SelectedItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
        
        <scp:SimpleMessageBox id="messageBox" runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>