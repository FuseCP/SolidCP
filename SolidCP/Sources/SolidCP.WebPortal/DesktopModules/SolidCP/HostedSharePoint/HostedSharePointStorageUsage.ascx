<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostedSharePointStorageUsage.ascx.cs" Inherits="SolidCP.Portal.HostedSharePointStorageUsage" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel"
    TagPrefix="scp" %>
<%@ Register Src="../ExchangeServer/UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="scp" %>
<%@ Register Src="../ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="scp" %>

<div id="ExchangeContainer">
	<div class="Module">
		<div class="Left">
        </div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="ExchangeStorageConfig48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Storage Usage"></asp:Localize>
				</div>
				<div class="panel-body form-horizontal">
				    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    
					<scp:CollapsiblePanel id="secSiteCollectionsReport" runat="server"
                        TargetControlID="siteCollectionsReport" meta:resourcekey="secSiteCollectionsReport" Text="Site Collections">
                    </scp:CollapsiblePanel>
                    <asp:Panel ID="siteCollectionsReport" runat="server" Height="0" style="overflow:hidden;">
				        <asp:GridView ID="gvStorageUsage" runat="server" AutoGenerateColumns="False" meta:resourcekey="gvStorageUsage"
					        Width="100%" EmptyDataText="gvSiteCollections" CssSelectorClass="NormalGridView">
					        <Columns>
						        <asp:BoundField meta:resourcekey="gvSiteCollectionName" DataField="Url" />
						        <asp:BoundField meta:resourcekey="gvSiteCollectionSize" DataField="DiskSpace" />						        
					        </Columns>
				        </asp:GridView>
				        <br />
			            <table cellpadding="2">
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="locTotalboxItems" runat="server" meta:resourcekey="locTotalMailboxItems" ></asp:Localize></td>
					            <td><asp:Label ID="lblTotalItems" runat="server" CssClass="NormalBold">177</asp:Label></td>
					        </tr>
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="locTotalMailboxesSize" runat="server" meta:resourcekey="locTotalMailboxesSize" ></asp:Localize></td>
					            <td><asp:Label ID="lblTotalSize" runat="server" CssClass="NormalBold">100</asp:Label></td>
					        </tr>
				        </table>
				        <br />
				    </asp:Panel>                   										                    								    
				
				
				<div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnRecalculateDiscSpace" CssClass="btn btn-success" runat="server" onclick="btnRecalculateDiscSpace_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnRecalculateDiscSpaceText"/> </CPCC:StyleButton>						
				    </div>
				</div>
			</div>
		</div>
	</div>
</div>