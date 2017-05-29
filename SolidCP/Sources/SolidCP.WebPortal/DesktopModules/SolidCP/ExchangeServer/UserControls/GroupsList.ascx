<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupsList.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.UserControls.GroupsList" %>
<%@ Register Src="../../UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>

<asp:UpdatePanel ID="GroupsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
    
	<div class="FormButtonsBarClean">
		<CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" OnClick="btnDelete_Click"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
        <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton>
	</div>
	<asp:GridView ID="gvGroups" runat="server" meta:resourcekey="gvAccounts" AutoGenerateColumns="False"
		Width="600px" CssSelectorClass="NormalGridView"
		DataKeyNames="AccountName">
		<Columns>
			<asp:TemplateField>
				<HeaderTemplate>
					<asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
				</HeaderTemplate>
				<ItemTemplate>
					<asp:CheckBox ID="chkSelect" runat="server" />
					<asp:Literal ID="litAccountType" runat="server" Visible="false" Text='<%# Eval("AccountType") %>'></asp:Literal>
				</ItemTemplate>
				<ItemStyle Width="10px" />
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvGroupsDisplayName" HeaderText="gvGroupsDisplayName">
				<HeaderStyle Wrap="false" />
				<ItemStyle Width="100%" Wrap="false"></ItemStyle>
				<ItemTemplate>
					<asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("AccountType")) %>' ImageAlign="AbsMiddle" />
					<asp:Literal ID="litDisplayName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Literal>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>


<asp:Panel ID="AddGroupsPanel" runat="server" style="display:none">
	<div class="widget">
             <div class="widget-header clearfix">
                           <h3><i class="fa fa-user"></i>  <asp:Localize ID="headerAddGroups" runat="server" meta:resourcekey="headerAddGroups"></asp:Localize></h3>
			</div>
                    <div class="widget-content Popup">
    <asp:UpdatePanel ID="AddGroupsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="FormButtonsBarClean">
                <div class="FormButtonsBarCleanRight">
                    <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch">
                        <asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="NormalTextBox">
                            <asp:ListItem Value="DisplayName" meta:resourcekey="ddlSearchColumnDisplayName">DisplayName</asp:ListItem>
                        </asp:DropDownList><asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox><asp:ImageButton ID="cmdSearch" Runat="server" meta:resourcekey="cmdSearch" SkinID="SearchButton"
	                        CausesValidation="false" OnClick="cmdSearch_Click"/>
                    </asp:Panel>
                </div>
            </div>
            <div class="Popup-Scroll">
				<asp:GridView ID="gvPopupGroups" runat="server" meta:resourcekey="gvPopupGroups" AutoGenerateColumns="False"
					Width="100%" CssSelectorClass="NormalGridView"
					DataKeyNames="AccountName">
					<Columns>
						<asp:TemplateField>
							<HeaderTemplate>
								<asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
							</HeaderTemplate>
							<ItemTemplate>
								<asp:CheckBox ID="chkSelect" runat="server" />
								<asp:Literal ID="litAccountType" runat="server" Visible="false" Text='<%# Eval("AccountType") %>'></asp:Literal>
							</ItemTemplate>
							<ItemStyle Width="10px" />
						</asp:TemplateField>
						<asp:TemplateField meta:resourcekey="gvGroupsDisplayName">
							<ItemStyle Width="100%"></ItemStyle>
							<ItemTemplate>
								<asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("AccountType")) %>' ImageAlign="AbsMiddle" />
								<asp:Literal ID="litDisplayName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Literal>
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
				</asp:GridView>
			</div>
	    </ContentTemplate>
    </asp:UpdatePanel>
			<br /><br />
			<CPCC:StyleButton id="btnCancelAdd" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnAddSelected" CssClass="btn btn-success" runat="server" OnClick="btnAddSelected_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddSelectedText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>

<asp:Button ID="btnAddAccountsFake" runat="server" style="display:none;" />
<ajaxToolkit:ModalPopupExtender ID="AddGroupsModal" runat="server"
	TargetControlID="btnAddAccountsFake" PopupControlID="AddGroupsPanel"
	BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelAdd" />

	</ContentTemplate>
</asp:UpdatePanel>