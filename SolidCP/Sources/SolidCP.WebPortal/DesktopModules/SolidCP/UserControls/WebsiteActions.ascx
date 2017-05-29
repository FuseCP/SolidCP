<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebsiteActions.ascx.cs" Inherits="SolidCP.Portal.WebsiteActions" %>

<script type="text/javascript">
    function ShowProgress(btn) {
        var action = $(btn).prev().val();

        if (action === 1) {
            ShowProgressDialog("Stopping websites...");
        } else if (action == 2) {
            ShowProgressDialog("Starting websites...");
        } else if (action == 3) {
            ShowProgressDialog("Restarting App Pools...");
        }
    }
</script>
<asp:UpdatePanel ID="tblActions" runat="server" CssClass="NormalBold" UpdateMode="Conditional" ChildrenAsTriggers="true" >
    <ContentTemplate>
        <div class="input-group">
        <asp:DropDownList ID="ddlWebsiteActions" runat="server" CssClass="form-control" resourcekey="ddlWebsiteActions" AutoPostBack="True">
            <asp:ListItem Value="0">Actions</asp:ListItem>
            <asp:ListItem Value="1">Stop</asp:ListItem>
            <asp:ListItem Value="2">Start</asp:ListItem>
            <asp:ListItem Value="3">RestartAppPool</asp:ListItem>
        </asp:DropDownList>
        <div class="input-group-btn">
        <asp:Button ID="btnApply" runat="server" meta:resourcekey="btnApply"
        Text="Apply" CssClass="btn btn-primary" OnClick="btnApply_Click" OnClientClick="return ShowProgress(this);" />
        </div>
        </div>
    </ContentTemplate>
    
    <Triggers>
        <asp:PostBackTrigger ControlID="btnApply" />
    </Triggers>
</asp:UpdatePanel>
