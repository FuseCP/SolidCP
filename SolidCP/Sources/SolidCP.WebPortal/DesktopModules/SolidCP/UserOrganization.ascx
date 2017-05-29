<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserOrganization.ascx.cs" Inherits="SolidCP.Portal.UserOrganization" %>
<%@ Import Namespace="SolidCP.Portal" %>


<asp:Panel ID="UserOrgPanel" runat="server" Visible="false">

    <asp:Repeater ID="OrgList" runat="server" EnableViewState="false">
        <ItemTemplate>
          
                <h4><asp:Label ID="lblOrg" runat="server" meta:resourcekey="lblOrg" CssClass="LinkText" Text='<%# Eval("Text") %>' /></h4>
            


                <div class="IconsBlock">
                    <asp:DataList ID="OrgIcons" runat="server" DataSource='<%# GetIconMenuItems(Eval("ChildItems")) %>'
                        CellSpacing="1" RepeatColumns="5" RepeatDirection="Horizontal">
                        <ItemTemplate>
                            <asp:Panel ID="IconPanel" runat="server" CssClass="Icon">
                                <asp:HyperLink ID="imgLink" runat="server" NavigateUrl='<%# Eval("NavigateURL") %>'><asp:Image ID="imgIcon" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' /></asp:HyperLink>
                                <br />
                                <asp:HyperLink ID="lnkIcon" runat="server" NavigateUrl='<%# Eval("NavigateURL") %>'><%# Eval("Text") %></asp:HyperLink>
                            </asp:Panel>
                            <asp:Panel ID="IconMenu" runat="server" CssClass="IconMenu" Visible='<%# IsIconMenuVisible(Eval("ChildItems")) %>'>
                                <ul>
                                    <asp:Repeater ID="MenuItems" runat="server" DataSource='<%# GetIconMenuItems(Eval("ChildItems")) %>'>
                                        <ItemTemplate>
                                            <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("NavigateURL") %>'><%# Eval("Text") %></asp:HyperLink></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </asp:Panel>
                            <ajaxToolkit:HoverMenuExtender TargetControlID="IconPanel" PopupControlID="IconMenu" runat="server"
                                PopupPosition="Right" HoverCssClass="Icon Hover"></ajaxToolkit:HoverMenuExtender>
                        </ItemTemplate>
                    </asp:DataList>
                </div>

        </ItemTemplate>
    </asp:Repeater>

</asp:Panel>

