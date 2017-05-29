<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceEditDetails.ascx.cs" Inherits="SolidCP.Portal.SpaceEditDetails" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="UserControls/ServerDetails.ascx" TagName="ServerDetails" TagPrefix="scp" %>
<%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="HostingPlansQuotas.ascx" TagName="HostingPlansQuotas" TagPrefix="scp" %>
<%@ Register Src="SpaceQuotasControl.ascx" TagName="SpaceQuotasControl" TagPrefix="scp" %>
<%@ Register Src="UserControls/CalendarControl.ascx" TagName="CalendarControl" TagPrefix="scp" %>

<div class="panel-body form-horizontal">
<asp:Label ID="lblMessage" runat="server" CssClass="NormalBold" ForeColor="red"></asp:Label>
<table id="tblEditPackage" runat="server" cellspacing="0" cellpadding="3" width="100%">
	<tr>
		<td class="SubHead">
            <asp:Label ID="lblSpaceName" runat="server" meta:resourcekey="lblSpaceName" Text="Space Name:"></asp:Label></td>
		<td class="Normal">
			<asp:TextBox id="txtName" runat="server" CssClass="form-control" Width="400px"></asp:TextBox>
			<asp:RequiredFieldValidator id="nameValidator" CssClass="NormalBold" runat="server" ErrorMessage="*"
				Display="Dynamic" ControlToValidate="txtName" ValidationGroup="EditSpace"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="SubHead"><asp:Label ID="lblComments" runat="server" meta:resourcekey="lblComments" Text="Comments:"></asp:Label></td>
		<td class="Normal">
			<asp:TextBox id="txtComments" runat="server" CssClass="NormalTextBox" Columns="40" Width="400px"
				Rows="3" TextMode="MultiLine"></asp:TextBox>
		</td>
	</tr>
	<tr>
		<td class="SubHead">
            <asp:Label ID="lblCreationDate" runat="server" meta:resourcekey="lblCreationDate" Text="Creation Date:"></asp:Label></td>
		<td class="Normal">
		    <scp:CalendarControl id="PurchaseDate" runat="server" ValidationEnabled="true" ValidationGroup="EditSpace" />

		</td>
	</tr>
	<tr>
	    <td colspan="2"><hr /></td>
	</tr>
	<tr>
		<td class="SubHead"><asp:Label ID="lblHostingPlan" runat="server" meta:resourcekey="lblHostingPlan" Text="Hosting Plan:"></asp:Label></td>
		<td class="Normal">
			<asp:DropDownList id="ddlPlan" runat="server" CssClass="NormalTextBox" DataValueField="PlanID" DataTextField="PlanName"></asp:DropDownList>
			<asp:RequiredFieldValidator id="planValidator" CssClass="NormalBold" runat="server" ErrorMessage="*"
				Display="Dynamic" ControlToValidate="ddlPlan" ValidationGroup="EditSpace"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="SubHead" height="26px"><asp:Label ID="lblTargetServer" runat="server" meta:resourcekey="lblTargetServer" Text="Target Server:"></asp:Label></td>
		<td class="Normal">
            <scp:ServerDetails ID="serverDetails" runat="server" />
		</td>
	</tr>
</table>

<scp:CollapsiblePanel id="secAddons" runat="server"
    TargetControlID="AddonsPanel" meta:resourcekey="secAddons" Text="Space Add-Ons">
</scp:CollapsiblePanel>
<asp:Panel ID="AddonsPanel" runat="server" Height="0" style="overflow:hidden;">
    <div class="FormButtonsBar">
        <asp:Button ID="btnAddAddon" runat="server" meta:resourcekey="btnAddAddon" Text="Add Add-on" CssClass="Button2" OnClick="btnAddAddon_Click" />
    </div>
    <asp:GridView ID="gvAddons" runat="server" AutoGenerateColumns="False"
        CssSelectorClass="NormalGridView"
        EmptyDataText="gvAddons">
        <Columns>
            <asp:TemplateField SortExpression="PlanName" HeaderText="gvAddonsName">
                <ItemStyle Width="100%"></ItemStyle>
                <ItemTemplate>
                    <b><asp:hyperlink id=lnkEdit runat="server" NavigateUrl='<%# EditUrl("PackageAddonID", Eval("PackageAddonID").ToString(), "edit_addon", "SpaceID=" + Eval("PackageID").ToString()) %>'>
                        <%# Eval("PlanName") %>
                    </asp:hyperlink></b><br />
                    <%# Eval("PlanDescription") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Quantity" HeaderText="gvAddonsQuantity" />
            <asp:BoundField SortExpression="PurchaseDate" DataField="PurchaseDate" HeaderText="gvAddonsCreationDate" DataFormatString="{0:d}" >
                <ItemStyle Wrap="False" />
                <HeaderStyle Wrap="False" />
            </asp:BoundField>
            <asp:TemplateField SortExpression="StatusID" HeaderText="gvAddonsStatus">
                <ItemTemplate>
                     <%# PanelFormatter.GetPackageStatusName((int)Eval("StatusID"))%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
</asp:Panel>


<scp:CollapsiblePanel id="secQuotas" runat="server"
    TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Space Quotas">
</scp:CollapsiblePanel>
<asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
    <table id="tblQuotas" runat="server" width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table id="tblOverrideQuotas" runat="server" border="0" cellpadding="2" width="100%">
                    <tr>
                        <td class="SubHead" width="200" nowrap rowspan="2">
                        </td>
                        <td class="NormalBold" width="100%">
                            <asp:RadioButton ID="rbPlanQuotas" runat="server" GroupName="OverrideQuotas" AutoPostBack="true"
                                meta:resourcekey="rbPlanQuotas" Text="Use quotas defined on plan level" OnCheckedChanged="rbPlanQuotas_CheckedChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td class="NormalBold">
                            <asp:RadioButton ID="rbPackageQuotas" runat="server" GroupName="OverrideQuotas" AutoPostBack="true"
                                meta:resourcekey="rbPackageQuotas" Text="Override quotas on space level" OnCheckedChanged="rbPlanQuotas_CheckedChanged" />                        
                        </td>
                    </tr>
                </table>
                <scp:SpaceQuotasControl id="packageQuotas" runat="server">
                </scp:SpaceQuotasControl>
                <scp:HostingPlansQuotas id="editPackageQuotas" runat="server">
                </scp:HostingPlansQuotas>
                <br />
            </td>
        </tr>
    </table>
</asp:Panel>

</div>

<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="EditSpace"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSave"/> </CPCC:StyleButton>
</div>