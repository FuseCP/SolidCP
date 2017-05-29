<%@ Control Language="C#" AutoEventWireup="true" Codebehind="HostedSharePointEnterpriseSiteCollections.ascx.cs"
	Inherits="SolidCP.Portal.HostedSharePointEnterpriseSiteCollections" %>
<%@ Register Src="../UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems"
	TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
	TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="scp" %>
	
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
	if (!confirm("Are you sure you want to delete Site Collection?")) return false; else ShowProgressDialog('Deleting SharePoint site collection...');	
}
</script>
	
<div id="ExchangeContainer">
	<div class="Module">
		<div class="Left">
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="SharePointSiteCollection48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="SharePoint Site Collections"></asp:Localize>
				</div>
				<div class="panel-body form-horizontal">
					<scp:SimpleMessageBox id="messageBox" runat="server" />
					<div class="FormButtonsBarClean">
						<div class="FormButtonsBarCleanLeft">
							<CPCC:StyleButton id="btnCreateSiteCollection" CssClass="btn btn-success" runat="server" OnClick="btnCreateSiteCollection_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCreateSiteCollectionText"/> </CPCC:StyleButton>
						</div>
						<div class="FormButtonsBarCleanRight">
							<asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch">
								<asp:Localize ID="locSearch" runat="server" meta:resourcekey="locSearch" Visible="false"></asp:Localize>
								<asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="NormalTextBox">
									<asp:ListItem Value="ItemName" meta:resourcekey="ddlSearchColumnUrl">Url</asp:ListItem>
								</asp:DropDownList><asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox"
									Width="100"></asp:TextBox><asp:ImageButton ID="cmdSearch" runat="server" meta:resourcekey="cmdSearch"
										SkinID="SearchButton" CausesValidation="false" />
							</asp:Panel>
						</div>
					</div>
					<asp:GridView ID="gvSiteCollections" runat="server" AutoGenerateColumns="False" EnableViewState="true"
						Width="100%" EmptyDataText="gvSiteCollection" CssSelectorClass="NormalGridView" OnRowCommand="gvSiteCollections_RowCommand"
						AllowPaging="True" AllowSorting="True" DataSourceID="odsSiteCollectionsPaged">
						<Columns>
							<asp:TemplateField meta:resourcekey="gvSiteCollectionUrl" SortExpression="ItemName">
								<ItemStyle Width="50%"></ItemStyle>
								<ItemTemplate>
									<asp:HyperLink ID="lnk1" runat="server" NavigateUrl='<%# GetSiteCollectionEditUrl(Eval("Id").ToString()) %>'>
									    <%# Eval("PhysicalAddress") %>
									</asp:HyperLink>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField meta:resourcekey="gvOwnerDisplayName" DataField="OwnerName"	ItemStyle-Width="50%" />
							<asp:TemplateField>
								<ItemTemplate>
									<CPCC:StyleButton id="cmdDelete" CssClass="btn btn-danger" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("Id") %>' OnClientClick="confirmation()"> &nbsp;<i class="fa fa-trash-o"></i>&nbsp; </CPCC:StyleButton>
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
					<asp:ObjectDataSource ID="odsSiteCollectionsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetSharePointEnterpriseSiteCollectionPagedCount"
						SelectMethod="GetSharePointEnterpriseSiteCollectionPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.HostedSharePointEnterpriseSiteCollectionsHelper"
						OnSelected="odsSharePointEnterpriseSiteCollectionPaged_Selected">
						<SelectParameters>
					        <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="-1" />
							<asp:QueryStringParameter Name="organizationId" QueryStringField="ItemID" DefaultValue="0" />
                            <asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
							<asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
						</SelectParameters>
					</asp:ObjectDataSource>
					<br />
					<asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Site Collections Created:"></asp:Localize>
					&nbsp;&nbsp;&nbsp;
					<%--<scp:Quota ID="siteCollectionsQuota1" runat="server" QuotaName="HostedSharePoint.Sites" />--%>
					<scp:QuotaViewer ID="siteCollectionsQuota" runat="server" QuotaTypeId="2" />
				</div>
			</div>
		</div>
	</div>
</div>
