<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BandwidthReport.ascx.cs" Inherits="SolidCP.Portal.BandwidthReport" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc2" %>
<%@ Register Src="UserControls/Comments.ascx" TagName="Comments" TagPrefix="uc4" %>
<div class="buttons-in-panel-header">
       <asp:Button ID="btnExportReport" runat="server" Text="Export Report" meta:resourcekey="btnExportReport" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnExportReport_Click" />
</div>
<div class="panel-body">
    <fieldset>
        <h4><asp:Localize ID="locSortData" runat="server" meta:resourcekey="locSortData" Text="Sort Data"></asp:Localize></h4>
        <hr />
        <div class="row">
            <div class="col-sm-6">
                <div class="form-inline">
                    <div class="form-group">
                        <asp:Label ID="lblFrom" runat="server" meta:resourcekey="lblFrom" Text="From:" CssClass="form-label"></asp:Label>
                        <uc3:CalendarControl ID="calStartDate" runat="server" Cssclass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblTo" runat="server" meta:resourcekey="lblTo" Text="To:" CssClass="form-label"></asp:Label>
                        <uc3:CalendarControl ID="calEndDate" runat="server" Cssclass="form-control" />
                    </div>
                      <div class="form-group">
                    <asp:Button ID="btnDisplay" runat="server" Text="Display Report" meta:resourcekey="btnDisplay" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnDisplay_Click" />
                </div>
                </div>
            </div>
            <div class="col-sm-6 text-right">
                <div class="form-inline">
                    <div class="form-group">
                        <CPCC:StyleButton ID="cmdPrevMonth" runat="server" meta:resourcekey="cmdPrevMonth" OnClick="cmdPrevMonth_Click" CssClass="btn btn-primary"></CPCC:StyleButton>&nbsp;
                        <asp:Literal ID="litStartDate" runat="server" Visible="false"></asp:Literal>
                        <asp:Literal ID="litPeriod" runat="server"></asp:Literal>
                        <asp:Literal ID="litEndDate" runat="server" Visible="false"></asp:Literal>&nbsp;
                        <CPCC:StyleButton ID="cmdNextMonth" runat="server" meta:resourcekey="cmdNextMonth" OnClick="cmdNextMonth_Click" CssClass="btn btn-primary"></CPCC:StyleButton>
                    </div>
                </div>
            </div>
        </div>
    </fieldset>
</div>




<div class="FormButtonsBar">
</div>
<asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False"
    EmptyDataText="gvReport"
    CssSelectorClass="NormalGridView"
    AllowSorting="True" DataSourceID="odsReport" AllowPaging="True" OnRowDataBound="gvReport_RowDataBound">
    <Columns>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:hyperlink id="lnkEdit" runat="server" Target="_blank" NavigateUrl='<%# EditUrl("SpaceID", Eval("PackageID").ToString(), "edit", "StartDate=" + DateTime.Parse(StartDate).Ticks.ToString(), "EndDate=" + DateTime.Parse(EndDate).Ticks.ToString()) %>'>
					<asp:Image ID="imgDetails" runat="server" SkinID="ViewSmall" />
				</asp:hyperlink>
			</ItemTemplate>
            <HeaderStyle Wrap="False" />
		</asp:TemplateField>
		<asp:TemplateField SortExpression="P.PackageName" HeaderText="gvReportPackageName">
			<ItemTemplate>
				<asp:hyperlink id="lnkEdit" runat="server" Visible='<%# (int)Eval("PackagesNumber") > 0 %>'
				    NavigateUrl='<%# GetNavigatePackageLink((int)Eval("PackageID")) %>'>
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
        <asp:BoundField DataField="Bandwidth" SortExpression="Bandwidth" HeaderText="gvReportUsed">
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
        <asp:BoundField SortExpression="PackagesNumber" DataField="PackagesNumber" HeaderText="gvReportTotalSpaces">
            <HeaderStyle Wrap="False" />
        </asp:BoundField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsReport" runat="server" EnablePaging="True" SelectCountMethod="GetPackagesBandwidthPagedCount"
    SelectMethod="GetPackagesBandwidthPaged" SortParameterName="sortColumn" TypeName="SolidCP.Portal.ReportsHelper" OnSelected="odsReport_Selected">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="-1" Name="packageId" QueryStringField="SpaceID" />
        <asp:ControlParameter ControlID="litStartDate" Name="sStartDate" PropertyName="Text" />
        <asp:ControlParameter ControlID="litEndDate" Name="sEndDate" PropertyName="Text" />
    </SelectParameters>
</asp:ObjectDataSource>