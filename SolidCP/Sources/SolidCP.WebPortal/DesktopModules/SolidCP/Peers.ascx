<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Peers.ascx.cs" Inherits="SolidCP.Portal.Peers" %>
<%@ Import Namespace="SolidCP.Portal" %>
<div class="FormButtonsBar right">
	<CPCC:StyleButton ID="btnAddItem" runat="server" CssClass="btn btn-primary" OnClick="btnAddItem_Click" >
         <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddItem"/>
	</CPCC:StyleButton>
</div>
<asp:GridView id="usersList" runat="server" AutoGenerateColumns="False"
	AllowPaging="True" AllowSorting="True" DataSourceID="odsUserPeers"
	EnableViewState="False" EmptyDataText="usersList"
	CssSelectorClass="NormalGridView">
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>							        
				<asp:Image ID="img2" runat="server" Width="16px" Height="16px" ImageUrl='<%# GetStateImage(Eval("LoginStatusId")) %>' ImageAlign="AbsMiddle" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField SortExpression="Username" HeaderText="usersListUsername">
			<ItemTemplate>
				<asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# EditUrl("PeerID", Eval("UserID").ToString(), "edit_peer", "UserID=" + PanelSecurity.SelectedUserId.ToString()) %>'>
					<%# Eval("Username") %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="usersListRole" SortExpression="RoleID">
			<ItemTemplate>							        
                <asp:Label runat="server" ID="lblRole" Text='<%# GetRoleName((int) Eval("RoleID")) %>' />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="FullName" SortExpression="FullName" HeaderText="usersListName">
            <ItemStyle Wrap="False" Width="50%" />
        </asp:BoundField>
		<asp:BoundField DataField="Email" SortExpression="Email" HeaderText="usersListEmail">
		    <HeaderStyle Wrap="False" />
		     <ItemStyle Wrap="False" Width="50%" />
		</asp:BoundField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsUserPeers" runat="server"
    SelectMethod="GetUserPeers" TypeName="SolidCP.Portal.UsersHelper" OnSelected="odsUserPeers_Selected" MaximumRowsParameterName="" StartRowIndexParameterName="">
</asp:ObjectDataSource>