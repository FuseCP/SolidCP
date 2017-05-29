<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InstallerApplications.ascx.cs" Inherits="SolidCP.Portal.InstallerApplications" %>
<%@ Import Namespace="SolidCP.Portal" %>
<div class="FormButtonsBar">
    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"
        DataValueField="Id" DataTextField="Name" AutoPostBack="True">
    </asp:DropDownList>
</div>
<asp:GridView id="gvApplications" runat="server" AutoGenerateColumns="False"
	ShowHeader="false" CssSelectorClass="NormalGridView"
	EnableViewState="False" DataSourceID="odsApplications"
	EmptyDataText="gvApplications" OnRowCommand="gvApplications_RowCommand">
	<Columns>
		<asp:TemplateField HeaderText="gvApplicationsApplication">
		    <ItemStyle HorizontalAlign="Center" />
			<ItemTemplate>
				<div style="text-align:center;">
					<asp:hyperlink NavigateUrl='<%# EditUrl("ApplicationID", Eval("Id").ToString(), "edit", "SpaceID=" + PanelSecurity.PackageId.ToString()) %>'
							runat="server" ID="Hyperlink3" ToolTip='<%# Eval("Name") %>'>
						<asp:Image runat=server ID="Image1" ImageUrl='<%# "ApplicationInstallerControls/" + (string)Eval("Logo") %>'
							AlternateText='<%# Eval("Name") %>'>
						</asp:Image>
					</asp:hyperlink>
				</div>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<div class="MediumBold" style="padding:4px;">
					<asp:hyperlink CssClass=MediumBold NavigateUrl='<%# EditUrl("ApplicationID", Eval("Id").ToString(), "edit", "SpaceID=" + PanelSecurity.PackageId.ToString()) %>'
					    runat="server" ID="lnkAppDetails" ToolTip='<%# Eval("Name") %>'>
						<%# Eval("Name") %>
					</asp:hyperlink>
				</div>
				<div class="Normal" style="padding:4px;">
					<%# Eval("ShortDescription") %>
				</div>
				<div class="Normal" style="padding:4px;">
			        <b><asp:Label ID="lblRequires" runat="server" Text='<%# GetLocalizedString("lblRequires.Text") %>'></asp:Label></b>
			        <%# FormatRequirements(Container.DataItem) %>
				</div>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="gvApplicationsApplication">
		    <ItemStyle HorizontalAlign="Center" />
			<ItemTemplate>
			    <CPCC:StyleButton id="btnInstall" CssClass="btn btn-success" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Install"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnInstallText"/> </CPCC:StyleButton>
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsApplications" runat="server"
    SelectMethod="GetApplications" TypeName="SolidCP.Portal.AppInstallerHelpers" OnSelected="odsApplications_Selected">
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlCategory" Name="categoryId" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:ObjectDataSource>