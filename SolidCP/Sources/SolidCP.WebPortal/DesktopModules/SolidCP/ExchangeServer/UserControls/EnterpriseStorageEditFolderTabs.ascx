<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnterpriseStorageEditFolderTabs.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.UserControls.EnterpriseStorageEditFolderTabs" %>
        
            <asp:DataList ID="esTabs" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" EnableViewState="false">
                <ItemStyle Wrap="False" />
                <ItemTemplate >
                    <asp:HyperLink ID="lnkTab" runat="server" CssClass="Tab" NavigateUrl='<%# Eval("Url") %>' OnClick="return tabClicked();">
                        <%# Eval("Name") %>
                    </asp:HyperLink>
                </ItemTemplate>
                <SelectedItemStyle Wrap="False" />
                <SelectedItemTemplate>
                    <asp:HyperLink ID="lnkSelTab" runat="server" CssClass="ActiveTab" NavigateUrl='<%# Eval("Url") %>' OnClick="return tabClicked;">
                        <%# Eval("Name") %>
                    </asp:HyperLink>
                </SelectedItemTemplate>                
            </asp:DataList>

<script type="text/javascript">
    function tabClicked() {
        ShowProgressDialog('Loading');
        ShowProgressDialogInternal();
        return true;
    }
</script>