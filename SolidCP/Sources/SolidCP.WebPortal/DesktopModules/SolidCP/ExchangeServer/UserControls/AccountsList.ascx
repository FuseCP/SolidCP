<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountsList.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.UserControls.AccountsList" %>
<%@ Register Src="../../UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>

<asp:UpdatePanel ID="AccountsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
    
	<div class="FormButtonsBarClean">
		<CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" OnClick="btnDelete_Click"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
        <CPCC:StyleButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </CPCC:StyleButton>
	</div>
	<asp:GridView ID="gvAccounts" runat="server" meta:resourcekey="gvAccounts" AutoGenerateColumns="False"
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
			<asp:TemplateField meta:resourcekey="gvAccountsDisplayName" HeaderText="gvAccountsDisplayName">
				<HeaderStyle Wrap="false" />
				<ItemStyle Width="50%" Wrap="false"></ItemStyle>
				<ItemTemplate>
					<asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("AccountType")) %>' ImageAlign="AbsMiddle" />
					<asp:Literal ID="litDisplayName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Literal>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvAccountsEmail" HeaderText="gvAccountsEmail">
				<HeaderStyle Wrap="false" />
				<ItemStyle Width="50%" Wrap="false"></ItemStyle>
				<ItemTemplate>
					<asp:Literal ID="litPrimaryEmailAddress" runat="server" Text='<%# Eval("PrimaryEmailAddress") %>'></asp:Literal>
				</ItemTemplate>
			</asp:TemplateField>
            <asp:TemplateField meta:resourcekey="gvAccountsAccountType" HeaderText="gvAccountsAccountType">
				<HeaderStyle Wrap="false" />
				<ItemStyle Width="50%" Wrap="false"></ItemStyle>
				<ItemTemplate>
					<asp:Literal ID="litType" runat="server" Text='<%# GetType((int)Eval("AccountType")) %>'></asp:Literal>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>


<asp:Panel ID="AddAccountsPanel" runat="server" style="display:none">
	<div class="widget">
             <div class="widget-header clearfix">
				<h3><i class="fa fa-user"></i>  <asp:Localize ID="headerAddAccounts" runat="server" meta:resourcekey="headerAddAccounts"></asp:Localize></h3>
			</div>
                    <div class="widget-content Popup">
<asp:UpdatePanel ID="AddAccountsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
	            <div style="text-align:right;margin-bottom: 4px;">
					<asp:Localize ID="locIncludeSearch" runat="server" Text="Include in search:"></asp:Localize>
					<asp:CheckBox ID="chkIncludeMailboxes" runat="server" Text="Accounts" Checked="true"
							meta:resourcekey="chkIncludeMailboxes" AutoPostBack="true" CssClass="Normal" OnCheckedChanged="chkIncludeMailboxes_CheckedChanged" />
                    <asp:CheckBox ID="chkIncludeRooms" runat="server" Text="Rooms" Checked="true"
						    meta:resourcekey="chkIncludeRooms" AutoPostBack="true" CssClass="Normal" OnCheckedChanged="chkIncludeMailboxes_CheckedChanged" />
                    <asp:CheckBox ID="chkIncludeEquipment" runat="server" Text="Equipment" Checked="true"
							meta:resourcekey="chkIncludeEquipment" AutoPostBack="true" CssClass="Normal" OnCheckedChanged="chkIncludeMailboxes_CheckedChanged" />
							
					<asp:CheckBox ID="chkIncludeContacts" runat="server" Text="Contacts" Checked="true"
							meta:resourcekey="chkIncludeContacts" AutoPostBack="true" CssClass="Normal" OnCheckedChanged="chkIncludeMailboxes_CheckedChanged" />
					<asp:CheckBox ID="chkIncludeLists" runat="server" Text="Distribution Lists" Checked="true"
							meta:resourcekey="chkIncludeLists" AutoPostBack="true" CssClass="Normal" OnCheckedChanged="chkIncludeMailboxes_CheckedChanged" />
                    <asp:CheckBox ID="chkIncludeGroups" runat="server" Text="Groups" Checked="true"
							meta:resourcekey="chkIncludeGroups" AutoPostBack="true" CssClass="Normal" OnCheckedChanged="chkIncludeMailboxes_CheckedChanged" />
                    <asp:CheckBox ID="chkIncludeSharedMailbox" runat="server" Text="Shared Mailbox" Checked="true"
                        meta:resourcekey="chkIncludeSharedMailbox" AutoPostBack="true" CssClass="Normal" OnCheckedChanged="chkIncludeMailboxes_CheckedChanged" />
                </div>
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
									<asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("AccountType")) %>' ImageAlign="AbsMiddle" />
									<asp:Literal ID="litDisplayName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Literal>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField meta:resourcekey="gvAccountsEmail">
								<ItemStyle Width="50%"></ItemStyle>
								<ItemTemplate>
									<asp:Literal ID="litPrimaryEmailAddress" runat="server" Text='<%# Eval("PrimaryEmailAddress") %>'></asp:Literal>
								</ItemTemplate>
							</asp:TemplateField>
                            <asp:TemplateField meta:resourcekey="gvAccountsAccountType" HeaderText="gvAccountsAccountType">
				                <ItemStyle Width="50%"></ItemStyle>
				                <ItemTemplate>
					                <asp:Literal ID="litType" runat="server" Text='<%# GetType((int)Eval("AccountType")) %>'></asp:Literal>
				                </ItemTemplate>
			                </asp:TemplateField>
						</Columns>
					</asp:GridView>
				</div>
	</ContentTemplate>
</asp:UpdatePanel>
		<br /><br />
			<CPCC:StyleButton id="btnCancelAdd" CssClass="btn btn-warning" runat="server" CausesValidation="False"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnAddSelected" CssClass="btn btn-success" runat="server" OnClick="btnAddSelected_Click"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddSelectedText"/> </CPCC:StyleButton>&nbsp;
		</div>
	</div>
</asp:Panel>

<asp:Button ID="btnAddAccountsFake" runat="server" style="display:none;" />
<ajaxToolkit:ModalPopupExtender ID="AddAccountsModal" runat="server"
	TargetControlID="btnAddAccountsFake" PopupControlID="AddAccountsPanel"
	BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelAdd" />

	</ContentTemplate>
</asp:UpdatePanel>