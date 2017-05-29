<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SecurityGroupTabs.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.UserControls.SecurityGroupTabs" %>
            <asp:DataList ID="dlTabs" runat="server" RepeatDirection="Horizontal"
                RepeatLayout="Flow" EnableViewState="false">
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