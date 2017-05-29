<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationUsers.ascx.cs" Inherits="SolidCP.Portal.HostedSolution.OrganizationUsers" %>

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

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<div class="panel-heading">
    <h3 class="panel-title">
        <asp:Image ID="Image1" SkinID="OrganizationUser48" runat="server" />
        <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Users"></asp:Localize>
    </h3>
</div>
<div class="FormButtonsBar right">
    <CPCC:StyleButton ID="btnCreateUser" CssClass="btn btn-primary" runat="server" OnClick="btnCreateUser_Click">
        <i class="fa fa-user-plus">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCreateUserText" />
    </CPCC:StyleButton>
</div>
<div class="panel-body form-horizontal">
    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <div class="row">
        <div class="col-md-3">
            <scp:UserActions ID="userActions" runat="server" GridViewID="gvUsers" CheckboxesName="chkSelectedUsersIds" />
        </div>
        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch" CssClass="col-md-9 text-right form-inline">
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem Selected="True">20</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="form-control">
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
</div>

<asp:Panel ID="UsersPanel" runat="server">
    <asp:UpdatePanel ID="UsersUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                Width="100%" EmptyDataText="gvUsers" CssSelectorClass="NormalGridView" DataKeyNames="AccountId,AccountType"
                OnRowCommand="gvUsers_RowCommand" AllowPaging="True" AllowSorting="True"
                DataSourceID="odsAccountsPaged" PageSize="20">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="selectAll" runat="server" onclick="checkAll(this);" CssClass="HeaderCheckbox"></asp:CheckBox>
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
                    <asp:TemplateField HeaderText="gvUsersDisplayName" SortExpression="DisplayName">
                        <ItemStyle Width="25%"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("AccountType"),(bool)Eval("IsVIP")) %>' ImageAlign="AbsMiddle" />
                            <asp:Hyperlink ID="lnk1" runat="server" NavigateUrl='<%# GetUserEditUrl(Eval("AccountId").ToString()) %>'>
                                <%# Eval("DisplayName") %>
                            </asp:Hyperlink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="gvUsersLogin" DataField="UserPrincipalName" SortExpression="UserPrincipalName" ItemStyle-Width="25%" />
                    <asp:TemplateField HeaderText="gvServiceLevel">
                        <ItemStyle Width="25%"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="lbServLevel" runat="server" ToolTip='<%# GetServiceLevel((int)Eval("LevelId")).LevelDescription%>'>
                                <%# GetServiceLevel((int)Eval("LevelId")).LevelName%>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="gvUsersEmail" DataField="PrimaryEmailAddress" SortExpression="PrimaryEmailAddress" ItemStyle-Width="25%" />
                    <asp:BoundField HeaderText="gvSubscriberNumber" DataField="SubscriberNumber" ItemStyle-Width="20%" />
                    <asp:TemplateField ItemStyle-Wrap="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="Image2" runat="server" Width="16px" Height="16px" ToolTip="Mail" ImageUrl='<%# GetMailImage((int)Eval("AccountType")) %>' CommandName="OpenMailProperties" CommandArgument='<%# Eval("AccountId") %>' Enabled='<%# EnableMailImageButton((int)Eval("AccountType")) %>' />
                            <asp:ImageButton ID="Image3" runat="server" Width="16px" Height="16px" ToolTip="UC" ImageUrl='<%# GetOCSImage((bool)Eval("IsOCSUser"),(bool)Eval("IsLyncUser"),(bool)Eval("IsSfBUser")) %>' CommandName="OpenUCProperties" CommandArgument='<%# GetOCSArgument((int)Eval("AccountId"),(bool)Eval("IsOCSUser"),(bool)Eval("IsLyncUser"),(bool)Eval("IsSfBUser")) %>' Enabled='<%# EnableOCSImageButton((bool)Eval("IsOCSUser"),(bool)Eval("IsLyncUser"),(bool)Eval("IsSfBUser")) %>' />
                            <asp:ImageButton ID="Image4" runat="server" Width="16px" Height="16px" ToolTip="BlackBerry" ImageUrl='<%# GetBlackBerryImage((bool)Eval("IsBlackBerryUser")) %>' CommandName="OpenBlackBerryProperties" CommandArgument='<%# Eval("AccountId") %>' Enabled='<%# EnableBlackBerryImageButton((bool)Eval("IsBlackBerryUser")) %>' />
                            <asp:Image ID="Image5" runat="server" Width="16px" Height="16px" ToolTip="CRM" ImageUrl='<%# GetCRMImage((Guid)Eval("CrmUserId")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <CPCC:StyleButton ID="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return ShowProgressDialog('Please wait...');">
                                &nbsp;
                                <i class="fa fa-trash-o"></i>&nbsp;
                            </CPCC:StyleButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsAccountsPaged" runat="server" EnablePaging="True"
                SelectCountMethod="GetOrganizationUsersPagedCount"
                SelectMethod="GetOrganizationUsersPaged"
                SortParameterName="sortColumn"
                TypeName="SolidCP.Portal.OrganizationsHelper"
                OnSelected="odsAccountsPaged_Selected">
                <SelectParameters>
                    <asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
                    <asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
                    <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:Panel ID="DeleteUserPanel" runat="server" Style="display: none">
                <div class="widget">
                    <div class="widget-header clearfix">
                        <h3><i class="fa fa-user"></i>  <asp:Localize ID="headerDeleteUser" runat="server" meta:resourcekey="headerDeleteUser"></asp:Localize></h3>
                    </div>
                    <div class="widget-content Popup">
                        <asp:UpdatePanel ID="DeleteUserUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdAccountId" runat="server" Value="0" />
                                <asp:Literal ID="litDeleteUser" runat="server" meta:resourcekey="litDeleteUser"></asp:Literal>
                                <br />
                                <asp:CheckBox ID="chkEnableForceArchiveMailbox" runat="server" meta:resourcekey="chkEnableForceArchiveMailbox" Visible="false" Checked="false" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
					<div class="popup-buttons text-right">
                        <CPCC:StyleButton ID="btnCancelDelete" CssClass="btn btn-warning" runat="server" CausesValidation="False">
                            <i class="fa fa-times">&nbsp;</i>&nbsp;
                            <asp:Localize runat="server" meta:resourcekey="btnCancelText" />
                        </CPCC:StyleButton>&nbsp;
			            <CPCC:StyleButton ID="btnDeleteUser" CssClass="btn btn-danger" runat="server" OnClick="btnDelete_Click" OnClientClick="return ShowProgressDialog('Deleting user...');">
                            <i class="fa fa-trash-o">&nbsp;</i>&nbsp;
                            <asp:Localize runat="server" meta:resourcekey="btnDeleteText" />
			            </CPCC:StyleButton>
                    </div>
                </div>
            </asp:Panel>
            <asp:Button ID="btnDeleteUserFake" runat="server" Style="display: none;" />
            <ajaxToolkit:ModalPopupExtender ID="DeleteUserModal" runat="server" TargetControlID="btnDeleteUserFake" EnableViewState="true"
                PopupControlID="DeleteUserPanel" BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelDelete" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDeleteUser" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Panel>
<div class="panel-footer">
    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Users Created:"></asp:Localize>
    &nbsp;&nbsp;&nbsp;
    <scp:QuotaViewer ID="usersQuota" runat="server" QuotaTypeId="2" />
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
                QuotaAvailable='<%# Eval("QuotaAvailable")%>' />
        </div>
    </ItemTemplate>
</asp:Repeater>
