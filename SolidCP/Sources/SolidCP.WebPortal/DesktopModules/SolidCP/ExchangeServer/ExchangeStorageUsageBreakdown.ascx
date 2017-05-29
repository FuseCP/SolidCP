<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeStorageUsageBreakdown.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeStorageUsageBreakdown" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Import Namespace="SolidCP.Portal" %>
				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeStorage48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Storage Usage Breakdown"></asp:Localize>
                    </h3>
				</div>
				
				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    
				    <scp:CollapsiblePanel id="secMailboxesReport" runat="server"
                        TargetControlID="MailboxesReport" meta:resourcekey="secMailboxesReport" Text="Mailboxes">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="MailboxesReport" runat="server" Height="0" style="overflow:hidden;">
				        <asp:GridView ID="gvMailboxes" runat="server" AutoGenerateColumns="False"
					        Width="100%" EmptyDataText="gvMailboxes" CssSelectorClass="NormalGridView">
					        <Columns>
						        <asp:BoundField HeaderText="gvMailboxesEmail" DataField="ItemName" />
						        <asp:BoundField HeaderText="gvMailboxesItems" DataField="TotalItems" />
						        <asp:BoundField HeaderText="gvMailboxesSize" DataField="TotalSizeMB" />
						        <asp:TemplateField HeaderText="gvMailboxesLastLogon">
									<ItemTemplate>&nbsp;<%# Utils.FormatDateTime((DateTime)Eval("LastLogon"))%></ItemTemplate>
						        </asp:TemplateField>
						        <asp:TemplateField HeaderText="gvMailboxesLastLogoff">
									<ItemTemplate>&nbsp;<%# Utils.FormatDateTime((DateTime)Eval("LastLogoff")) %></ItemTemplate>
						        </asp:TemplateField>
					        </Columns>
				        </asp:GridView>
				        <br />
			            <table cellpadding="2">
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="locTotalMailboxes" runat="server" meta:resourcekey="locTotalMailboxes" Text="Total Mailboxes:"></asp:Localize></td>
					            <td><asp:Label ID="lblTotalMailboxes" runat="server" CssClass="NormalBold">177</asp:Label></td>
					        </tr>

					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="locTotalMailboxItems" runat="server" meta:resourcekey="locTotalMailboxItems" Text="Total Items:"></asp:Localize></td>
					            <td><asp:Label ID="lblTotalMailboxItems" runat="server" CssClass="NormalBold">177</asp:Label></td>
					        </tr>
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="locTotalMailboxesSize" runat="server" meta:resourcekey="locTotalMailboxesSize" Text="Total Size (MB):"></asp:Localize></td>
					            <td><asp:Label ID="lblTotalMailboxSize" runat="server" CssClass="NormalBold">100</asp:Label></td>
					        </tr>
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="Localize1" runat="server" meta:resourcekey="locAverageMailboxSize" Text="Avg. Size (MB):"></asp:Localize></td>
					            <td><asp:Label ID="lblAverageMailboxSize" runat="server" CssClass="NormalBold">100</asp:Label></td>
					        </tr>

				        </table>
				        <br />
				    </asp:Panel>
			        <br />
                </div>