<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSCollectionTabs.ascx.cs" Inherits="SolidCP.Portal.RDS.UserControls.RdsServerTabs" %>
<div class="nav nav-tabs" style="padding-bottom:7px !important;">
            <asp:DataList ID="rdsTabs" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" EnableViewState="false">
                <ItemStyle Wrap="False" />
                <ItemTemplate >
                    <asp:HyperLink ID="lnkTab" runat="server" CssClass="Tab" style="padding:10px 15px;" NavigateUrl='<%# Eval("Url") %>' OnClick="return tabClicked();">
                        <%# Eval("Name") %>
                    </asp:HyperLink>
                </ItemTemplate>
                <SelectedItemStyle Wrap="False" />
                <SelectedItemTemplate>
                    <asp:HyperLink ID="lnkSelTab" runat="server" CssClass="ActiveTab" style="padding:10px 15px;" NavigateUrl='<%# Eval("Url") %>' OnClick="return tabClicked;">
                        <%# Eval("Name") %>
                    </asp:HyperLink>
                </SelectedItemTemplate>                
            </asp:DataList>
</div>

<script type="text/javascript">
    function tabClicked() {
        ShowProgressDialog('Loading');
        ShowProgressDialogInternal();
        return true;
    }
</script>