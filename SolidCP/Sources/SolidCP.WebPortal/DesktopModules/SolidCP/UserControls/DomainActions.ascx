<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainActions.ascx.cs" Inherits="SolidCP.Portal.DomainActions" %>
<script type="text/javascript">
    function ShowProgress(btn) {
        var action = $(btn).prev().val();

        if (action === 1) {
            ShowProgressDialog("Enabling Dns...");
        } else if (action == 2) {
            ShowProgressDialog("Disabling Dns...");
        } else if (action == 3) {
            ShowProgressDialog("Creating Instant Alias...");
        } else if (action == 4) {
            ShowProgressDialog("Removing Instant Alias...");
        }
    }
</script>
<asp:UpdatePanel ID="tblActions" runat="server" CssClass="NormalBold" UpdateMode="Conditional" ChildrenAsTriggers="true" >
    <ContentTemplate>
        <div class="input-group">
        <asp:DropDownList ID="ddlDomainActions" runat="server" CssClass="form-control" resourcekey="ddlDomainActions" AutoPostBack="True">
            <asp:ListItem Value="0">Actions</asp:ListItem>
            <asp:ListItem Value="1">EnableDns</asp:ListItem>
            <asp:ListItem Value="2">DisableDns</asp:ListItem>
            <asp:ListItem Value="3">CreateInstantAlias</asp:ListItem>
            <asp:ListItem Value="4">DeleteInstantAlias</asp:ListItem>
        </asp:DropDownList>
         <div class="input-group-btn">
             <CPCC:StyleButton id="btnApply" CssClass="btn btn-primary" runat="server" OnClick="btnApply_Click" OnClientClick="return ShowProgress(this);"><asp:Label runat="server" meta:resourcekey="btnApplyText"/></CPCC:StyleButton>
         </div>
       </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnApply" />
    </Triggers>
</asp:UpdatePanel>
