<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiskspaceReport.ascx.cs" Inherits="SolidCP.Portal.DiskspaceReport" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc2" %>
<%@ Register Src="UserControls/Comments.ascx" TagName="Comments" TagPrefix="uc4" %>
<div class="FormButtonsBar right">
	<asp:Button ID="btnExportReport" runat="server" Text="Export Report" meta:resourcekey="btnExportReport" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnExportReport_Click" />
</div>
<asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False"
    EmptyDataText="gvReport" CssSelectorClass="NormalGridView"
    AllowSorting="True" DataSourceID="odsReport" AllowPaging="True" OnRowDataBound="gvReport_RowDataBound">
    <Columns>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:hyperlink id="lnkEdit" runat="server" Target="_blank" NavigateUrl='<%# EditUrl("SpaceID", Eval("PackageID").ToString(), "edit") %>'>
					<asp:Image ID="imgDetails" runat="server" SkinID="ViewSmall" />
				</asp:hyperlink>
			</ItemTemplate>
            <HeaderStyle Wrap="False" />
		</asp:TemplateField>
		<asp:TemplateField SortExpression="P.PackageName" HeaderText="gvReportPackageName">
			<ItemTemplate>
				<asp:hyperlink id="lnkEdit" runat="server" Visible='<%# (int)Eval("PackagesNumber") > 0 %>'
				    NavigateUrl='<%# NavigateURL("SpaceID", Eval("PackageID").ToString()) %>'>
					<%# Eval("PackageName")%>
				</asp:hyperlink>
				<asp:Literal ID="litPackageName" runat="server" Visible='<%# (int)Eval("PackagesNumber") == 0 %>'
				    Text='<%# Eval("PackageName")%>'></asp:Literal>
			</ItemTemplate>
            <HeaderStyle Wrap="False" />
		</asp:TemplateField>
        <asp:TemplateField SortExpression="P.StatusID" HeaderText="gvReportPackageStatus">
            <ItemTemplate>
		         <%# PanelFormatter.GetPackageStatusName((int)Eval("StatusID"))%>
            </ItemTemplate>
            <HeaderStyle Wrap="False" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvReportAllocated" SortExpression="QuotaValue">
            <ItemTemplate>
                <%# GetAllocatedValue((int)Eval("QuotaValue")) %>
            </ItemTemplate>
            <HeaderStyle Wrap="False" />
        </asp:TemplateField>
		<asp:BoundField DataField="Diskspace" SortExpression="Diskspace" HeaderText="gvReportUsed">
            <HeaderStyle Wrap="False" />
        </asp:BoundField>
		<asp:BoundField DataField="UsagePercentage" SortExpression="UsagePercentage" HeaderText="gvReportUsage">
            <HeaderStyle Wrap="False" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="gvReportUser">
            <ItemStyle Wrap="False" />
            <ItemTemplate>
		         <uc2:UserDetails ID="userDetails" runat="server"
		            UserID='<%# Eval("UserID") %>'
		            Username='<%# Eval("Username") %>' />
		         <uc4:Comments id="userComments" runat="server"
				    Comments='<%# Eval("UserComments") %>'>
                </uc4:Comments>
            </ItemTemplate>
            <HeaderStyle Wrap="False" />
        </asp:TemplateField>
        <asp:BoundField DataField="PackagesNumber" SortExpression="PackagesNumber" HeaderText="gvReportTotalSpaces">
            <HeaderStyle Wrap="False" />
        </asp:BoundField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsReport" runat="server" EnablePaging="True" SelectCountMethod="GetPackagesDiskspacePagedCount"
    SelectMethod="GetPackagesDiskspacePaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.ReportsHelper" OnSelected="odsReport_Selected">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="-1" Name="packageId" QueryStringField="SpaceID" />
    </SelectParameters>
</asp:ObjectDataSource>