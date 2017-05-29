<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxes.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeMailboxes" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/UserActions.ascx" TagName="UserActions" TagPrefix="scp" %>

<script type="text/javascript">
    function checkAll(selectAllCheckbox) {
        //get all checkbox and select it
        $('td :checkbox').prop("checked", selectAllCheckbox.checked);
    }
    function unCheckSelectAll(selectCheckbox) {
        //if any item is unchecked, uncheck header checkbox as also
        if (!selectCheckbox.checked)
            $('th :checkbox').prop("checked", false);
    }
</script>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<div class="panel-heading">
    <h3 class="panel-title">
        <asp:Image ID="Image1" SkinID="ExchangeMailbox48" runat="server" />
        <asp:Localize ID="locTitle" runat="server" Text="Mailboxes"></asp:Localize>
    </h3>
</div>
<div class="FormButtonsBar right">
    <CPCC:StyleButton id="btnCreateMailbox" CssClass="btn btn-primary" runat="server" OnClick="btnCreateMailbox_Click" ValidationGroup="CreateMailbox">
        <i class="fa fa-envelope">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCreateMailbox"/>
    </CPCC:StyleButton>
</div>
<div class="panel-body form-horizontal">
    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <div class="row">
        <div class="col-md-4">
            <scp:UserActions ID="userActions" runat="server" GridViewID="gvMailboxes" CheckboxesName="chkSelectedUsersIds" ShowSetMailboxPlan="true" />
        </div>
        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch" CssClass="col-md-8 text-right form-inline">
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlPageSize" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem Selected="True">20</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlSearchColumn" runat="server" class="form-control">
                        <asp:ListItem Value="DisplayName" meta:resourcekey="ddlSearchColumnDisplayName">DisplayName</asp:ListItem>
                        <asp:ListItem Value="PrimaryEmailAddress" meta:resourcekey="ddlSearchColumnEmail">Email</asp:ListItem>
                        <asp:ListItem Value="AccountName" meta:resourcekey="ddlSearchColumnAccountName">AccountName</asp:ListItem>
                        <asp:ListItem Value="SubscriberNumber" meta:resourcekey="ddlSearchColumnSubscriberNumber">Account Number</asp:ListItem>
                        <asp:ListItem Value="UserPrincipalName" meta:resourcekey="ddlSearchColumnUserPrincipalName">Login</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <asp:TextBox ID="txtSearchValue" runat="server" CssClass="form-control"></asp:TextBox>
                    <div class="input-group-btn">
                        <CPCC:StyleButton ID="cmdSearch" runat="server" CausesValidation="false" CssClass="btn btn-primary">
                            <i class="fa fa-search" aria-hidden="true"></i>
                        </CPCC:StyleButton>      
                    </div>     
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="row">
        <div class="col-md-12 pull-right">
            <div class="pull-right" style="margin-top:5px;">
                <asp:CheckBox ID="chkMailboxes" runat="server" meta:resourcekey="chkMailboxes" Text="Mailboxes" AutoPostBack="true" OnCheckedChanged="chkMailboxes_CheckedChanged" CssClass="Small" />&emsp;
                <asp:CheckBox ID="chkResourceMailboxes" runat="server" meta:resourcekey="chkResourceMailboxes" Text="Resource Mailboxes" AutoPostBack="true" OnCheckedChanged="chkMailboxes_CheckedChanged" CssClass="Small" />&emsp;
                <asp:CheckBox ID="chkSharedMailboxes" runat="server" meta:resourcekey="chkSharedMailboxes" Text="Shared Mailboxes" AutoPostBack="true" OnCheckedChanged="chkMailboxes_CheckedChanged" CssClass="Small" />
            </div>
        </div>
    </div>
</div>
<asp:GridView ID="gvMailboxes" runat="server" AutoGenerateColumns="False" EnableViewState="true" Width="100%" EmptyDataText="gvMailboxes" CssSelectorClass="NormalGridView" DataKeyNames="AccountId" OnRowCommand="gvMailboxes_RowCommand" AllowPaging="True" AllowSorting="True" DataSourceID="odsAccountsPaged" PageSize="20">
    <Columns>
    <asp:TemplateField>
        <HeaderTemplate>
            <asp:CheckBox ID="selectAll" Runat="server" onclick="checkAll(this);" CssClass="HeaderCheckbox"></asp:CheckBox>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:CheckBox runat="server" ID="chkSelectedUsersIds" onclick="unCheckSelectAll(this);" CssClass="GridCheckbox"></asp:CheckBox>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField>
        <ItemTemplate>
            <asp:Image ID="img2" runat="server" Width="16px" Height="16px" ImageUrl='<%# GetStateImage((bool)Eval("Locked"),(bool)Eval("Disabled")) %>' ImageAlign="AbsMiddle" />
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="gvMailboxesDisplayName" SortExpression="DisplayName">
        <ItemStyle Width="20%"></ItemStyle>
        <ItemTemplate>
            <asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("AccountType"),(bool)Eval("IsVIP")) %>' ImageAlign="AbsMiddle" />
            <asp:HyperLink id="lnk1" runat="server" NavigateUrl='<%# GetMailboxEditUrl(Eval("AccountId").ToString()) %>'>
                <%# Eval("DisplayName") %>
            </asp:HyperLink>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="gvUsersLogin" SortExpression="UserPrincipalName">
        <ItemStyle Width="20%"></ItemStyle>
        <ItemTemplate>							        
            <asp:HyperLink id="lnk2" runat="server" NavigateUrl='<%# GetOrganizationUserEditUrl(Eval("AccountId").ToString()) %>'>
                <%# Eval("UserPrincipalName") %>
            </asp:HyperLink>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="gvServiceLevel">
        <ItemStyle Width="15%"></ItemStyle>
        <ItemTemplate>
            <asp:Label id="lbServLevel" runat="server" ToolTip = '<%# GetServiceLevel((int)Eval("LevelId")).LevelDescription%>'>
                <%# GetServiceLevel((int)Eval("LevelId")).LevelName%>
            </asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField HeaderText="gvMailboxesEmail" DataField="PrimaryEmailAddress" SortExpression="PrimaryEmailAddress" ItemStyle-Width="25%" />
    <asp:BoundField HeaderText="gvSubscriberNumber" DataField="SubscriberNumber" ItemStyle-Width="10%" />
    <asp:BoundField HeaderText="gvMailboxesMailboxPlan" DataField="MailboxPlan" SortExpression="MailboxPlan" ItemStyle-Width="50%" />
    <asp:TemplateField>
        <ItemTemplate>
            <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("AccountId") %>' OnClientClick="if(!confirm('Are you sure you want to delete the Exchange mailbox?.\n\nThis will only delete the mailbox, the Active Directory account will remain (under Organization --> Users) .\n\nDo you want to delete mailbox?')) return false; ShowProgressDialog('Deleting Exchange Mailbox...');">
                &nbsp;<i class="fa fa-trash-o"></i>&nbsp;
            </CPCC:StyleButton>
        </ItemTemplate>
    </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsAccountsPaged" runat="server" EnablePaging="True"
    SelectCountMethod="GetExchangeAccountsPagedCount" SelectMethod="GetExchangeAccountsPaged"
    SortParameterName="sortColumn" TypeName="SolidCP.Portal.ExchangeHelper"
    OnSelecting="odsAccountsPaged_Selecting" OnSelected="odsAccountsPaged_Selected">
    <SelectParameters>
        <asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
        <asp:Parameter Name="accountTypes" DefaultValue="1,5,6,10" />
        <asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
        <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
        <asp:Parameter Name="archiving" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>
<div class="panel-footer">
    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Mailboxes Created:"></asp:Localize>
    &nbsp;&nbsp;&nbsp;
    <scp:QuotaViewer ID="mailboxesQuota" runat="server" QuotaTypeId="2" />
</div>
<asp:Repeater ID="dlServiceLevelQuotas" runat="server" EnableViewState="false">
    <ItemTemplate>
        <div>
            <asp:Localize ID="locServiceLevelQuota" runat="server" Text='<%# Eval("QuotaDescription") %>'></asp:Localize>
            &nbsp;&nbsp;&nbsp;
            <scp:QuotaViewer ID="serviceLevelQuota" runat="server"
                QuotaTypeId='<%# Eval("QuotaTypeId") %>'
                QuotaUsedValue='<%# Eval("QuotaUsedValue") %>'
                QuotaValue='<%# Eval("QuotaValue") %>'
                QuotaAvailable='<%# Eval("QuotaAvailable")%>'/>
        </div>
    </ItemTemplate>
</asp:Repeater>
