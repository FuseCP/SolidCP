<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RDSCollectionApps.ascx.cs" Inherits="SolidCP.Portal.RDS.UserControls.RDSCollectionApps" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="../../UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="scp" %>

<asp:UpdatePanel ID="RDAppsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
	<div class="FormButtonsBarClean">
		<asp:LinkButton id="btnAdd" CssClass="btn btn-success" runat="server" OnClick="btnAdd_Click" OnClientClick="ShowProgressDialog('loading applications...');"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddText"/> </asp:LinkButton>&nbsp;
		<asp:LinkButton id="btnDelete" CssClass="btn btn-danger" runat="server" OnClick="btnDelete_Click"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </asp:LinkButton>      
	</div>
	<asp:GridView ID="gvApps" runat="server" meta:resourcekey="gvApps" AutoGenerateColumns="False"
		Width="600px" CssSelectorClass="NormalGridView" OnRowCommand="gvApps_RowCommand"
		DataKeyNames="Alias">
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
			<asp:TemplateField meta:resourcekey="gvAppName" HeaderText="gvAppName">
				<ItemStyle Width="90%" Wrap="false">
				</ItemStyle>
				<ItemTemplate>                    
                    <asp:LinkButton id="lnkDisplayName" meta:resourcekey="lnkDisplayName" runat="server" Text='<%# Eval("DisplayName")%>' CommandName="EditApplication" CommandArgument='<%# Eval("Alias") %>' OnClientClick="ShowProgressDialog('Loading ...');return true;"/>
                    <asp:HiddenField ID="hfFilePath" runat="server"  Value='<%# Eval("FilePath") %>'/>
                    <asp:HiddenField ID="hfRequiredCommandLine" runat="server"  Value='<%# Eval("RequiredCommandLine") %>'/>
                    <asp:HiddenField ID="hfUsers" runat="server"  Value='<%# Eval("Users") != null ? "New" : null %>'/>
				</ItemTemplate>
			</asp:TemplateField>  
            <asp:TemplateField>
                <ItemStyle Width="20px" />
                <ItemTemplate>
                    <asp:Image ID="UsersImage" ImageUrl='<%# PortalUtils.GetThemedImage("Exchange/accounting_mail_16.png")%>' runat="server" Visible='<%# Eval("Users") != null %>'/>
                </ItemTemplate>
            </asp:TemplateField>          
		</Columns>
	</asp:GridView>
    <br />


<asp:Panel ID="AddAppsPanel" runat="server" CssClass="Popup" style="display:none">
	<div class="widget">
             <div class="widget-header clearfix">
                           <h3><i class="fa fa-list"></i>  <asp:Localize ID="headerAddApps" runat="server" meta:resourcekey="headerAddApps"></asp:Localize></h3>
			</div>
                    <div class="widget-content">
<asp:UpdatePanel ID="AddAppsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>	            
                <div class="FormButtonsBarClean">
                </div>
                <div class="Popup-Scroll">
					<asp:GridView ID="gvPopupApps" runat="server" meta:resourcekey="gvPopupApps" AutoGenerateColumns="False"
						Width="100%" CssSelectorClass="NormalGridView"
						DataKeyNames="DisplayName">
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
							<asp:TemplateField meta:resourcekey="gvPopupAppName">
								<ItemStyle Width="70%"></ItemStyle>
								<ItemTemplate>
									<asp:Literal ID="litName" runat="server" Text='<%# Eval("DisplayName") %>'></asp:Literal>
                                    <asp:HiddenField ID="hfFilePathPopup" runat="server" Value='<%# Eval("FilePath") %>'/>
                                    <asp:HiddenField ID="hfRequiredCommandLinePopup" runat="server"  Value='<%# Eval("RequiredCommandLine") %>'/>
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
				</div>
	</ContentTemplate>
</asp:UpdatePanel>
		<div class="popup-buttons text-right">
			<CPCC:StyleButton id="btnCancelAdd" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClientClick="CloseProgressDialog();"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancelText"/> </CPCC:StyleButton>&nbsp;
            <CPCC:StyleButton id="btnAddSelected" CssClass="btn btn-success" runat="server" OnClick="btnAddSelected_Click" OnClientClick="CloseProgressDialog();"> <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddSelectedText"/> </CPCC:StyleButton>
		</div>
	</div>
</asp:Panel>

<asp:Button ID="btnAddAppsFake" runat="server" style="display:none;" />
<ajaxToolkit:ModalPopupExtender ID="AddAppsModal" runat="server"
	TargetControlID="btnAddAppsFake" PopupControlID="AddAppsPanel"
	BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelAdd" />

	</ContentTemplate>
</asp:UpdatePanel>