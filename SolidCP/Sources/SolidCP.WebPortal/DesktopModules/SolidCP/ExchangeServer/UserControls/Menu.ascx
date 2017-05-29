<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.UserControls.Menu" %>
<div class="Menu">
	<asp:DataList ID="repMenu" runat="server" EnableViewState="false"
		RepeatLayout="Flow" RepeatDirection="Vertical">
		<ItemTemplate>			
			<div class="MenuGroup">				
				<asp:Image runat="server" ID="imgGroupIcon" BorderStyle="none" ImageAlign="AbsMiddle" ImageUrl ='<%# Eval("ImageUrl") %>' Width="16px" Height="16px" />
				<asp:Label runat="server" ID="lblGroupName" Text='<%# Eval("Text") %>' />
				<asp:DataList runat="server" ID="dlMenuItems" EnableViewState="false" RepeatDirection="Vertical" RepeatLayout="Flow" DataSource='<%#Eval("MenuItems") %>' >
				    <ItemTemplate>
				        <div class="MenuItem">
			                <asp:HyperLink ID="lnkItem" runat="server" NavigateUrl='<%# Eval("Url") %>' Text='<%# Eval("Text") %>'/>			                
		                </div>
				    </ItemTemplate>
				    <SelectedItemTemplate>
		                <div class="SelectedMenuItem">
			                <asp:HyperLink ID="lnkItem" runat="server"
				                NavigateUrl='<%# Eval("Url") %>'
				                Text='<%# Eval("Text") %>'/>			                
		                </div>
		            </SelectedItemTemplate>
				</asp:DataList>								
			</div>
		</ItemTemplate>		
	</asp:DataList>
</div>