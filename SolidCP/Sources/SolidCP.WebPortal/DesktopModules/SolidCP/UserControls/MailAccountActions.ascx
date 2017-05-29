<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailAccountActions.ascx.cs" Inherits="SolidCP.Portal.MailAccountActions" %>

<script type="text/javascript">
    function ShowProgress(btn) {
        var action = $(btn).prev().val();

        if (action === 1) {
            ShowProgressDialog("Enabling mail account...");
        } else if (action === 2) {
            ShowProgressDialog("Disabling mail account...");
        }
    }
</script>
<asp:UpdatePanel ID="tblActions" runat="server" CssClass="NormalBold" UpdateMode="Conditional" ChildrenAsTriggers="true" >
    <ContentTemplate>
        <div class="input-group">
        <asp:DropDownList ID="ddlMailAccountActions" runat="server" CssClass="form-control" resourcekey="ddlWebsiteActions" AutoPostBack="True">
            <asp:ListItem Value="0">Actions</asp:ListItem>
            <asp:ListItem Value="1">Disable</asp:ListItem>
            <asp:ListItem Value="2">Enable</asp:ListItem>
        </asp:DropDownList>
            <span class="input-group-btn">
                <CPCC:StyleButton id="btnApply" CssClass="btn btn-primary" runat="server" OnClick="btnApply_Click" OnClientClick="return ShowProgress(this);"><asp:Localize runat="server" meta:resourcekey="btnApplyText"/> </CPCC:StyleButton>
            </span>
    </ContentTemplate>
    
    <Triggers>
        <asp:PostBackTrigger ControlID="btnApply" />
    </Triggers>
</asp:UpdatePanel>
