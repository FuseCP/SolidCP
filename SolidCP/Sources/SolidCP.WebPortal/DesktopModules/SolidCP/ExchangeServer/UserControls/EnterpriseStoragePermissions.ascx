<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnterpriseStoragePermissions.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.UserControls.EnterpriseStoragePermissions" %>
<%@ Register Src="../../UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>

<asp:UpdatePanel ID="PermissionsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
	<div class="FormButtonsBarClean">
		<CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" OnClick="btnDelete_Click"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
        <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton>
	</div>
	<asp:GridView ID="gvPermissions" runat="server" meta:resourcekey="gvPermissions" AutoGenerateColumns="False"
		Width="600px" CssSelectorClass="NormalGridView"
		DataKeyNames="Account">
		<Columns>
			<asp:TemplateField>
				<HeaderTemplate>
					<asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
				</HeaderTemplate>
				<ItemTemplate>
					<asp:CheckBox ID="chkSelect" runat="server" />
				</ItemTemplate>
				<ItemStyle Width="10px" />
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvPermissionsAccount" HeaderText="gvPermissionsAccount">
				<ItemStyle Width="60%" Wrap="false">
				</ItemStyle>
				<ItemTemplate>
                    <asp:Literal ID="litAccount" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Literal>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvPermissionsAccess" HeaderText="gvPermissionsAccess">
				<ItemStyle Width="40%" Wrap="false">
				</ItemStyle>
				<ItemTemplate>
					<asp:Literal ID="litAccess" runat="server" Text='<%# Eval("Access") %>'></asp:Literal>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
    <br />
    <div class="FormButtonsBarClean">
		<asp:Button ID="btnSetReadOnly" runat="server" Text="Set Read-Only" CssClass="btn btn-primary"  OnClick="btn_UpdateAccess" CommandArgument="Read-Only" meta:resourcekey="btnSetReadOnly"  />
		<asp:Button ID="btnSetReadWrite" runat="server" Text="Set Read-Write" CssClass="btn btn-success" OnClick="btn_UpdateAccess" CommandArgument="Read-Write" meta:resourcekey="btnSetReadWrite"/>
	</div>


<asp:Panel ID="AddAccountsPanel" runat="server" style="display:none">
	<div class="widget">
             <div class="widget-header clearfix">
                           <h3><i class="fa fa-user"></i>  <asp:Localize ID="headerAddAccounts" runat="server" meta:resourcekey="headerAddAccounts"></asp:Localize></h3>
			 </div>
                    <div class="widget-content Popup">
<asp:UpdatePanel ID="AddAccountsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
	            
                <div class="FormButtonsBarClean">
                    <div class="FormButtonsBarCleanRight">
                        <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch">
                            <asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="NormalTextBox">
                                <asp:ListItem Value="DisplayName" meta:resourcekey="ddlSearchColumnDisplayName">DisplayName</asp:ListItem>
                                <asp:ListItem Value="PrimaryEmailAddress" meta:resourcekey="ddlSearchColumnEmail">Email</asp:ListItem>
                            </asp:DropDownList><asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox><asp:ImageButton ID="cmdSearch" Runat="server" meta:resourcekey="cmdSearch" SkinID="SearchButton"
	                            CausesValidation="false" OnClick="cmdSearch_Click"/>
                        </asp:Panel>
                    </div>
                </div>
                <div class="Popup-Scroll">
					<asp:GridView ID="gvPopupAccounts" runat="server" meta:resourcekey="gvPopupAccounts" AutoGenerateColumns="False"
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
							<asp:TemplateField meta:resourcekey="gvAccountsDisplayName">
								<ItemStyle Width="50%"></ItemStyle>
								<ItemTemplate>
									<asp:Image ID="imgAccount" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("AccountType")) %>' ImageAlign="AbsMiddle" />
									<asp:Literal ID="litDisplayName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Literal>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField meta:resourcekey="gvAccountsEmail">
								<ItemStyle Width="50%"></ItemStyle>
								<ItemTemplate>
									<asp:Literal ID="litPrimaryEmailAddress" runat="server" Text='<%# Eval("PrimaryEmailAddress") %>'></asp:Literal>
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
<ajaxToolkit:ModalPopupExtender ID="AddAccountsModal" runat="server"
	TargetControlID="btnAddAccountsFake" PopupControlID="AddAccountsPanel"
	BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelAdd" />

	</ContentTemplate>
</asp:UpdatePanel>