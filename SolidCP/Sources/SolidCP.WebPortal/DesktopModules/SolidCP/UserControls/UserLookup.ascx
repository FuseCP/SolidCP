<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserLookup.ascx.cs" Inherits="SolidCP.Portal.UserLookup" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="UserDetails.ascx" TagName="UserDetails" TagPrefix="uc1" %>
<%@ Register Src="SearchBox.ascx" TagName="SearchBox" TagPrefix="uc1" %>
<asp:UpdatePanel ID="UserPanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
    
<table>
    <tr>
        <td class="NormalBold"><uc1:UserDetails ID="user" runat="server" /></td>
        <td>
			<asp:Menu ID="mnuActions" runat="server" CssSelectorClass="PopupMenu"
			    resourcekey="mnuActions" Orientation="Horizontal"
				OnMenuItemClick="mnuPackages_MenuItemClick" >
				<Items>
					<asp:MenuItem Text="" Selectable="false">
						<asp:MenuItem Value="select" Text="Select"></asp:MenuItem>
						<asp:MenuItem Value="switch_logged" Text="SwitchLogged"></asp:MenuItem>
						<asp:MenuItem Value="switch_empty" Text="ResetUser"></asp:MenuItem>
					</asp:MenuItem>
				</Items>
			</asp:Menu>
        </td>
    </tr>
</table>
<asp:Panel ID="SelectPanel" runat="server">
    <div class="FormButtonsBar">
        <div class="Left">
            <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>
        </div>
        <div class="Right">
            <uc1:SearchBox ID="searchBox" runat="server" />
        </div>
    </div>
    <asp:GridView id="gvUsers" runat="server" AutoGenerateColumns="False"
        CssSelectorClass="NormalGridView"
        AllowPaging="True" AllowSorting="True" DataSourceID="odsUsersPaged"
        OnRowCommand="gvUsers_RowCommand" EnableViewState="False" PageSize="5"
        EmptyDataText="gvUsers">
        <Columns>
            <asp:TemplateField SortExpression="Username" HeaderText="gvUsersUsername">
	            <ItemTemplate>
		            <CPCC:StyleButton id=cmdSelect runat="server" CommandName="select" CommandArgument='<%# Eval("UserID")%>' CausesValidation="false">
			            <%# Eval("Username") %>
		            </CPCC:StyleButton>
	            </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="FullName" SortExpression="FullName" HeaderText="gvUsersName">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="gvUsersEmail"></asp:BoundField>
            <asp:TemplateField SortExpression="RoleID" HeaderText="gvUsersRole">
	            <ItemStyle Wrap="False"></ItemStyle>
	            <ItemTemplate>
		            <%# PanelFormatter.GetUserRoleName((int)Eval("RoleID"))%>
	            </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsUsersPaged" runat="server" EnablePaging="True" SelectCountMethod="GetLoggedUsersPagedCount"
        SelectMethod="GetLoggedUsersPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.UsersHelper" OnSelected="odsUsersPaged_Selected">
        <SelectParameters>
            <asp:ControlParameter ControlID="searchBox" Name="filterColumn" PropertyName="FilterColumn" />
             <asp:ControlParameter ControlID="searchBox" Name="filterValue" PropertyName="FilterValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>