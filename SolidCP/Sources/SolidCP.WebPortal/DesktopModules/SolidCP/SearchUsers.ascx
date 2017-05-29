<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchUsers.ascx.cs" Inherits="SolidCP.Portal.SearchUsers" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="UserControls/Comments.ascx" TagName="Comments" TagPrefix="uc4" %>
<%@ Register Src="UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="uc1" %>
<%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc2" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>

<script type="text/javascript">
    //<![CDATA[
    $(document).ready(function () {
        $("#tbSearch").autocomplete({
            zIndex: 100,
            source: function (request, response) {
                $.ajax({
                    type: "post",
                    dataType: "json",
                    data: {
                        term: request.term,
                        fullType: 'Users',
                        columnType: "'" + $("#ddlFilterColumn").val() + "'"
                    },
                    url: "AjaxHandler.ashx",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.TextSearch,
                                code: item
                            };
                        }));
                    }
                })
            },
            select: function (event, ui) {
                var item = ui.item;
                $("#ddlFilterColumn").val(item.code.ColumnType);
                $("#tbSearchFullType").val(item.code.FullType);
                $("#tbSearchText").val(item.code.TextSearch);
            }
        });
    });//]]>
</script>

<div class="FormButtonsBar">
    <asp:Panel ID="tblSearch" runat="server" CssClass="NormalBold" DefaultButton="ImageButton1">
    <asp:Label ID="lblSearch" runat="server" meta:resourcekey="lblSearch"></asp:Label>
    <div align="center">
    <table>
        <tr>
            <td>
                <asp:DropDownList ClientIDMode="Static" ID="ddlFilterColumn" runat="server" CssClass="form-control" resourcekey="ddlFilterColumn">
                    <asp:ListItem Value="Username">Username</asp:ListItem>
                    <asp:ListItem Value="Email">Email</asp:ListItem>
                    <asp:ListItem Value="FullName">FullName</asp:ListItem>
                    <asp:ListItem Value="CompanyName">CompanyName</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0" align="right">
                    <tr>
                        <td align="left" class="SearchQuery">
                            <div class="ui-widget">
                                <asp:TextBox
                                    ID="tbSearch"
                                    ClientIDMode="Static"
                                    runat="server"
                                    CssClass="form-control"
                                    Width="120px"
                                    style="vertical-align: middle; z-index: 100;"
                                >
                                </asp:TextBox>
                                <asp:TextBox
                                    ID="tbSearchFullType"
                                    ClientIDMode="Static"
                                    runat="server"
                                    type="hidden"
                                >
                                </asp:TextBox>
                                <asp:TextBox
                                    ID="tbSearchText"
                                    ClientIDMode="Static"
                                    runat="server"
                                    type="hidden"
                                >
                                </asp:TextBox>

                                <asp:ImageButton
                                    ID="ImageButton1"
                                    runat="server"
                                    SkinID="SearchButton"
                                    OnClick="cmdSearch_Click"
                                    CausesValidation="false"
                                    style="vertical-align: middle;"
                                />                 
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </div>
    </asp:Panel>
</div>

<asp:GridView id="gvUsers" runat="server" AutoGenerateColumns="False"
	AllowPaging="True" AllowSorting="True"
	CssSelectorClass="NormalGridView"
	DataSourceID="odsUsersPaged" EnableViewState="False"
	EmptyDataText="gvUsers">
	<Columns>
		<asp:TemplateField SortExpression="Username" HeaderText="gvUsersUsername" HeaderStyle-Wrap="false">
			<ItemTemplate>
				<asp:hyperlink id=lnkEdit runat="server"
				    NavigateUrl='<%# GetUserHomePageUrl((int)Eval("UserID")) %>'>
					<%# Eval("Username") %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="FullName" HtmlEncode="false" SortExpression="FullName" HeaderText="gvUsersName">
		    <HeaderStyle Wrap="false" />
        </asp:BoundField>
		<asp:BoundField DataField="Email" SortExpression="Email" HeaderText="gvUsersEmail"></asp:BoundField>
		<asp:TemplateField SortExpression="RoleID" HeaderText="gvUsersRole">
			<ItemStyle Wrap="False"></ItemStyle>
			<ItemTemplate>
				<%# PanelFormatter.GetUserRoleName((int)Eval("RoleID"))%>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="CompanyName" SortExpression="CompanyName" HeaderText="gvUsersCompanyName">
		    <HeaderStyle Wrap="false" />
        </asp:BoundField>
		<asp:TemplateField SortExpression="OwnerUsername" HeaderText="gvUsersReseller" HeaderStyle-Wrap="false">
			<ItemTemplate>
				<asp:hyperlink id=lnkEdit runat="server" NavigateUrl='<%# GetUserHomePageUrl((int)Eval("OwnerID")) %>'>
					<%# Eval("OwnerUsername") %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="PackagesNumber" SortExpression="PackagesNumber" HeaderText="gvUsersSpaces"></asp:BoundField>
		<asp:TemplateField SortExpression="StatusID" HeaderText="gvUsersStatus">
			<ItemStyle Wrap="False"></ItemStyle>
			<ItemTemplate>
				<%# PanelFormatter.GetAccountStatusName((int)Eval("StatusID"))%>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField ItemStyle-Width="20px" ItemStyle-Wrap="false">
			<ItemTemplate><uc4:Comments id="Comments1" runat="server"
				    Comments='<%# Eval("Comments") %>'>
                </uc4:Comments></ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsUsersPaged" runat="server" EnablePaging="True" SelectCountMethod="GetUsersPagedRecursiveCount"
    SelectMethod="GetUsersPagedRecursive" SortParameterName="sortColumn" TypeName="SolidCP.Portal.UsersHelper" OnSelected="odsUsersPaged_Selected">
    <SelectParameters>
        <asp:QueryStringParameter Name="filterColumn" QueryStringField="Criteria" />
        <asp:QueryStringParameter Name="filterValue" QueryStringField="Query" />
    </SelectParameters>
</asp:ObjectDataSource>
