<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BandwidthReportPackageDetails.ascx.cs" Inherits="SolidCP.Portal.BandwidthReportPackageDetails" %>
<%@ Register Src="SpaceDetailsHeaderControl.ascx" TagName="SpaceDetailsHeaderControl" TagPrefix="scp" %>
<%@ Register Src="UserControls/Gauge.ascx" TagName="Gauge" TagPrefix="uc1" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<div class="panel-body form-horizontal">
    <scp:SpaceDetailsHeaderControl ID="spaceDetails" runat="server" />
    
    <scp:CollapsiblePanel id="secSummary" runat="server"
        TargetControlID="SummaryPanel" meta:resourcekey="secSummary" Text="Bandwidth by Resources">
    </scp:CollapsiblePanel>
    <asp:Panel ID="SummaryPanel" runat="server" Height="0" style="overflow:hidden;">
        <div class="Big">
            <asp:Literal ID="litPeriod" runat="server"></asp:Literal>
        </div>
        <br />
        <asp:GridView ID="gvSummary" runat="server" AutoGenerateColumns="False"
            EmptyDataText="gvSummary" CssSelectorClass="NormalGridView">
            <Columns>
	            <asp:TemplateField HeaderText="gvSummaryGroupName" ItemStyle-Width="150">
		            <ItemTemplate>
			            <span class="Big"><%# GetSharedLocalizedString("ReportResourceGroup." + Eval("GroupName")) %></span>
		            </ItemTemplate>
	            </asp:TemplateField>
	            <asp:TemplateField HeaderText="gvSummaryBandwidthTotal">
		            <ItemTemplate>
                        <uc1:Gauge ID="gauge" runat="server" OneColour="true"
                            Progress='<%# Convert.ToInt32(Eval("MegaBytesTotal")) %>'
                            Total='<%# BandwidthTotal %>' DisplayText="false" />
                         <span class="Medium" title='<%# Eval("BytesTotal") + " " + GetLocalizedString("lblBytes.Text") %>'><%# Eval("MegaBytesTotal")%> <asp:Label ID="lblMB2" runat="server" meta:resourcekey="lblMB" Text="MB"></asp:Label></span>
		            </ItemTemplate>
	            </asp:TemplateField>
	            <asp:TemplateField HeaderText="gvSummaryBandwidthSent">
	                <ItemStyle CssClass="Normal" HorizontalAlign="Center" />
		            <ItemTemplate>
		                <span title='<%# Eval("BytesSent") + " " + GetLocalizedString("lblBytes.Text") %>'><%# Eval("MegaBytesSent")%> <asp:Label ID="lblMB2" runat="server" meta:resourcekey="lblMB" Text="MB"></asp:Label></span>
		            </ItemTemplate>
	            </asp:TemplateField>
	            <asp:TemplateField HeaderText="gvSummaryBandwidthReceived">
	                <ItemStyle CssClass="Normal" HorizontalAlign="Center" />
		            <ItemTemplate>
		                <span title='<%# Eval("BytesReceived") + " " + GetLocalizedString("lblBytes.Text") %>'><%# Eval("MegaBytesReceived")%> <asp:Label ID="lblMB2" runat="server" meta:resourcekey="lblMB" Text="MB"></asp:Label></span>
		            </ItemTemplate>
	            </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="GridFooter">
            <div class="Medium">
                <asp:Label ID="lblTotal" runat="server" meta:resourcekey="lblTotal" Text="Total:"></asp:Label>
                <asp:Literal ID="litTotal" runat="server"></asp:Literal> <asp:Label ID="lblMB1" runat="server" meta:resourcekey="lblMB" Text="MB"></asp:Label>
            </div>
        </div>
    </asp:Panel>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>
</div>