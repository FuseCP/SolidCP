<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingPlans.ascx.cs" Inherits="SolidCP.Portal.HostingPlans" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="UserControls/ServerDetails.ascx" TagName="ServerDetails" TagPrefix="uc3" %>
<div class="FormButtonsBar right">
	<CPCC:StyleButton ID="btnAddItem" runat="server" CssClass="btn btn-primary" OnClick="btnAddItem_Click" >
        <i class="fa fa-plus">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnAddItem"/>
	</CPCC:StyleButton>
</div>
<asp:GridView id="gvPlans" runat="server" AutoGenerateColumns="False"
	DataSourceID="odsPlans" AllowPaging="True" AllowSorting="True" EmptyDataText="gvPlans"
	CssSelectorClass="NormalGridView">
	<Columns>
		<asp:TemplateField SortExpression="PlanName" HeaderText="gvPlansName">
			<ItemStyle Width="100%"></ItemStyle>
			<ItemTemplate>
				<b><asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# EditUrl("PlanID", Eval("PlanID").ToString(), "edit_plan", "UserID=" + Eval("UserID").ToString()) %>'>
					<%# PortalAntiXSS.EncodeOld((string)Eval("PlanName")) %>
				</asp:hyperlink></b><br />
				<%# PortalAntiXSS.EncodeOld((string)Eval("PlanDescription")) %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:hyperlink id="lnkCopy" runat="server" CssClass="btn btn-default btn-sm" NavigateUrl='<%# EditUrl("PlanID", Eval("PlanID").ToString(), "edit_plan", "UserID=" + Eval("UserID").ToString(), "TargetAction=Copy") %>'><i class="fa fa-clone">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="lnkCopy"/></asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
        <asp:TemplateField SortExpression="ServerName" HeaderText="gvPlansServer"
                HeaderStyle-Wrap="false">
            <ItemStyle Width="30%" Wrap="false"></ItemStyle>
            <ItemTemplate>
		         <uc3:ServerDetails ID="serverDetails" runat="server"
		            ServerID='<%# Eval("ServerID") %>'
		            ServerName='<%# Eval("ServerName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
		<asp:BoundField DataField="PackagesNumber" SortExpression="PackagesNumber" HeaderText="gvPlansSpaces"></asp:BoundField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsPlans" runat="server" SelectMethod="GetRawHostingPlans"
TypeName="SolidCP.Portal.HostingPlansHelper" OnSelected="odsPlans_Selected"></asp:ObjectDataSource>
