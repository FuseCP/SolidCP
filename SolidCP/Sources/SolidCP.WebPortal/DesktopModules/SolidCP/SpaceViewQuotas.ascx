<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceViewQuotas.ascx.cs" Inherits="SolidCP.Portal.SpaceViewQuotas" %>
<%@ Import Namespace="SolidCP.Portal" %>
<%@ Register Src="SpaceDetailsHeaderControl.ascx" TagName="SpaceDetailsHeaderControl" TagPrefix="scp" %>
<%@ Register Src="SpaceQuotasControl.ascx" TagName="SpaceQuotasControl" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<div class="panel-body form-horizontal">
    <scp:SpaceDetailsHeaderControl id="spaceDetailsHeaderControl" runat="server">
    </scp:SpaceDetailsHeaderControl>

    <scp:CollapsiblePanel id="secAddons" runat="server"
        TargetControlID="AddonsPanel" meta:resourcekey="secAddons" Text="Space Add-Ons">
    </scp:CollapsiblePanel>
    <asp:Panel ID="AddonsPanel" runat="server" Height="0" style="overflow:hidden;">
        <asp:GridView ID="gvAddons" runat="server" AutoGenerateColumns="False"
                CssSelectorClass="NormalGridView"
                EmptyDataText="gvAddons">
            <Columns>
                <asp:TemplateField SortExpression="PlanName" HeaderText="gvAddonsName">
	                <ItemStyle Width="100%"></ItemStyle>
	                <ItemTemplate>
		                <b><%# Eval("PlanName") %></b><br />
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
        <scp:SpaceQuotasControl id="packagesQuotas" runat="server">
        </scp:SpaceQuotasControl>
    </asp:Panel>
</div>

<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>
</div>