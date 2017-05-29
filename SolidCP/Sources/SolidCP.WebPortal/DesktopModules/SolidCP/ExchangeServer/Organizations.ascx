<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Organizations.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.Organizations" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="panel-heading">
    <h3 class="panel-title">
        <asp:Image ID="Image1" SkinID="Organization48" runat="server" />
        <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Organizations"></asp:Localize>
    </h3>
</div>
<div class="FormButtonsBar right">
    <CPCC:StyleButton id="btnCreate" CssClass="btn btn-primary" runat="server" OnClick="btnCreate_Click">
        <i class="fa fa-plus">&nbsp;</i>&nbsp;
        <asp:Localize runat="server" meta:resourcekey="btnCreateText"/>
    </CPCC:StyleButton>
</div>
<div class="panel-body form-horizontal">
    <scp:SimpleMessageBox id="messageBox" runat="server" />
    <div style="text-align:right;margin-bottom: 4px;">
        <asp:CheckBox ID="chkRecursive" runat="server" Text="Show Reseller Organizations" meta:resourcekey="chkRecursive" AutoPostBack="true" CssClass="Normal" />
    </div>
    <div class="row">
        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch" CssClass="col-md-7 col-md-offset-5 text-right form-inline">
            <div class="form-group">
                <div class="input-group">
                    <asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="form-control" style="vertical-align: middle;">
                        <asp:ListItem Value="ItemName" meta:resourcekey="ddlSearchColumnItemName">OrganizationName</asp:ListItem>
                        <asp:ListItem Value="Username" meta:resourcekey="ddlSearchColumnUsername">OwnerUsername</asp:ListItem>
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
<asp:GridView ID="gvOrgs" runat="server" AutoGenerateColumns="False" DataSourceID="odsOrgsPaged" Width="100%" meta:resourcekey="gvOrgs" CssSelectorClass="NormalGridView" OnRowCommand="gvOrgs_RowCommand" AllowPaging="True" AllowSorting="True" EnableViewState="false">
    <Columns>
        <asp:BoundField meta:resourcekey="gvOrgsID" DataField="OrganizationID" />
        <asp:TemplateField meta:resourcekey="gvOrgsName" SortExpression="ItemName">
            <ItemStyle Width="80%"></ItemStyle>
            <ItemTemplate>
                <div style="padding:7px;">
                    <asp:hyperlink id="lnk1" runat="server" EnableViewState="false" CssClass="NormalBold" NavigateUrl='<%# GetOrganizationEditUrl(Eval("ItemID").ToString()) %>'>
                        <%# Eval("ItemName") %>
                    </asp:hyperlink>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="PackageName" meta:resourcekey="gvOrgsSpace">
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
                <asp:hyperlink id="lnk4" runat="server" NavigateUrl='<%# GetSpaceHomePageUrl((int)Eval("PackageID")) %>'>
                    <%# Eval("PackageName") %>
                </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Username" meta:resourcekey="gvOrgsUser">
            <ItemStyle Wrap="False"></ItemStyle>
                <ItemTemplate>
                    <asp:hyperlink id="lnk3" runat="server" NavigateUrl='<%# GetUserHomePageUrl((int)Eval("UserID")) %>'>
                        <%# Eval("Username") %>
                    </asp:hyperlink>
                </ItemTemplate>
            <HeaderStyle Wrap="False" />
        </asp:TemplateField>
        <asp:TemplateField meta:resourcekey="gvOrgsDefault">
            <ItemTemplate>
                <div style="text-align:center">
                    <input type="radio" name="DefaultOrganization" value='<%# Eval("ItemID") %>' <%# IsChecked(Convert.ToString(Eval("IsDefault")), Eval("ItemID").ToString()) %>/>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Width="20px">
            <ItemTemplate>
                <CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("ItemID") %>' OnClientClick="return confirm('Remove this item?');">
                    &nbsp;
                    <i class="fa fa-trash-o"></i>&nbsp;
                </CPCC:StyleButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<div class="panel-body">
    <asp:ObjectDataSource ID="odsOrgsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetOrganizationsPagedCount" SelectMethod="GetOrganizationsPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.OrganizationsHelper" OnSelected="odsOrgsPaged_Selected">
        <SelectParameters>
            <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="-1" />
            <asp:ControlParameter Name="recursive" ControlID="chkRecursive" PropertyName="Checked" DefaultValue="False" />
            <asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
            <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
</div>
<div class="panel-footer">
    <div class="row">
        <div class="col-md-6">
            <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Organizations Created:"></asp:Localize>
            &nbsp;&nbsp;&nbsp;
            <scp:Quota ID="orgsQuota" runat="server" QuotaName="HostedSolution.Organizations" />
        </div>
        <div class="col-md-6 text-right">
            <CPCC:StyleButton id="btnSetDefaultOrganization" CssClass="btn btn-success" runat="server" OnClick="btnSetDefaultOrganization_Click">
                <i class="fa fa-check">&nbsp;</i>&nbsp;
                <asp:Localize runat="server" meta:resourcekey="btnSetDefaultOrganizationText"/>
            </CPCC:StyleButton>
            &nbsp;
        </div>
    </div>
</div>