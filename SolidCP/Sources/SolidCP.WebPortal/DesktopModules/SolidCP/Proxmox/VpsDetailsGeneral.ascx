<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsGeneral.ascx.cs" Inherits="SolidCP.Portal.Proxmox.VpsDetailsGeneral" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/Gauge.ascx" TagName="Gauge" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<asp:Timer runat="server" Interval="10000" ID="operationTimer" />

<script language="JavaScript" type="text/javascript">
function OpenRemoteDesktopWindow(resolution, width, height) {
    $find("RdpPopup").hidePopup();
    var rdpUrl = "<asp:literal id="litRdpPageUrl" runat="server" />";
    var left = (screen.width - width) / 2;
    var top = (screen.height - height) / 2;
    my_window = window.open(rdpUrl + resolution, "RDP", "status=0,width=" + width + ",height=" + height + ",top=" + top + ",left=" + left); 
}
</script>

	    <div class="Content">
		    <div class="Center">
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_general" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
				    <table id="DetailsTable" runat="server" style="width:100%;" cellspacing="10">
				        <tr>
				            <td valign="top" style="width:40%;">

                                <table cellspacing="2">
                                   <tr>
                                        <td><asp:Localize ID="locHostname" runat="server"
                                            meta:resourcekey="locHostname" Text="Host name:"/></td>
                                        <td>
                                            <b><asp:HyperLink ID="lnkHostname" runat="server" NavigateUrl="javascript:void(0);" Text="[hostname]"></asp:HyperLink><asp:Literal ID="litHostname" runat="server" Text="[hostname]"></asp:Literal></b>
                                            &nbsp;<asp:LinkButton ID="btnChangeHostnamePopup" runat="server"
                                                meta:resourcekey="btnChangeHostnamePopup" SkinID="EditSmall" Text="Edit"></asp:LinkButton>
                                                
                                            <asp:Panel ID="RdpPanel" runat="server" CssClass="PopupExtender" style="display:none;">
                                                <div style="padding-bottom:3px;">
                                                    <asp:Image ID="imgRdc" runat="server" SkinID="Rdc16" />&nbsp;
                                                    <asp:Localize ID="locRdpText" runat="server" meta:resourcekey="locRdpText" Text="Remote desktop"></asp:Localize><br />
                                                </div>
                                                <asp:HyperLink ID="lnkRdpFull" runat="server" NavigateUrl="javascript:OpenRemoteDesktopWindow(1, 800, 600);"
                                                    meta:resourcekey="lnkRdpFull" Text="Full screen"></asp:HyperLink><br />
                                                <asp:HyperLink ID="lnkRdp800" runat="server" NavigateUrl="javascript:OpenRemoteDesktopWindow(2, 800, 600);"
                                                    meta:resourcekey="lnkRdp800" Text="800 x 600"></asp:HyperLink><br />
                                                <asp:HyperLink ID="lnkRdp1024" runat="server" NavigateUrl="javascript:OpenRemoteDesktopWindow(3, 1024 , 768);"
                                                    meta:resourcekey="lnkRdp1024" Text="1024 x 768"></asp:HyperLink><br />
                                                <asp:HyperLink ID="lnkRdp1280" runat="server" NavigateUrl="javascript:OpenRemoteDesktopWindow(4, 1280, 1024);"
                                                    meta:resourcekey="lnkRdp1280" Text="1280 x 1024"></asp:HyperLink><br />
                                            </asp:Panel>
                                            
                                            <ajaxToolkit:PopupControlExtender ID="RdpPopup" BehaviorID="RdpPopup" runat="server" TargetControlID="lnkHostname"
                                                PopupControlID="RdpPanel" Position="Bottom" />
                                            <ajaxToolkit:DropShadowExtender  ID="RdpShadow" runat="server" TargetControlID="RdpPanel" TrackPosition="true" Opacity="0.4" Width="3" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Localize ID="locDomainTop" runat="server" meta:resourcekey="locDomainTop" Text="Domain:"/>
                                        </td>
                                        <td>
                                            <asp:Literal ID="litDomain" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </table>
                                
				                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="operationTimer" EventName="Tick" />
                                    </Triggers>
                                    <ContentTemplate>
                                    
                                <table cellspacing="2">
                                    <tr>
                                        <td><asp:Localize ID="locStatus" runat="server"
                                            meta:resourcekey="locStatus" Text="Status:"/></td>
                                        <td><asp:Literal ID="litStatus" runat="server" Text="[status]"/></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Localize ID="locUptime" runat="server"
                                            meta:resourcekey="locUptime" Text="Uptime:"/></td>
                                        <td><asp:Literal ID="litUptime" runat="server" Text="[uptime]"/></td>
                                    </tr>
                                    <tr>
                                        <td><asp:Localize ID="locCreated" runat="server"
                                            meta:resourcekey="locCreated" Text="Created:"/></td>
                                        <td><asp:Literal ID="litCreated" runat="server" Text="[date]"/></td>
                                    </tr>
                                </table>
				    				   </ContentTemplate>
				                 </asp:UpdatePanel>
				                 
				            </td>
				            <td valign="top" style="width:35%;">
				                
				                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="operationTimer" EventName="Tick" />
                                    </Triggers>
                                    <ContentTemplate>                
				                        <asp:Image ID="imgThumbnail" runat="server" Width="160" Height="120" style="border-style:ridge;border-width: 3px;border-color: #ffffff;" />
				                    </ContentTemplate>
				                 </asp:UpdatePanel>


				            </td>
				            <td rowspan="2" valign="top">
				                <ul class="ActionButtons">
				                    <asp:Repeater ID="repButtons" runat="server" 
                                        onitemcommand="repButtons_ItemCommand">
				                        <ItemTemplate>
				                            <li>
				                                <asp:LinkButton ID="btnAction" runat="server" CausesValidation="false"
				                                    Text='<%# Eval("Text") %>'
				                                    CommandName='<%# Eval("Command") %>'
				                                    style='<%# Eval("Style") %>'
				                                    OnClientClick='<%# Eval("OnClientClick") %>'
				                                    CssClass="ActionButton"></asp:LinkButton>
				                            </li>
				                        </ItemTemplate>
				                    </asp:Repeater>
				                </ul>
				            </td>
				        </tr>
				        <tr>
				            <td colspan="2">
				                <br />
				                
				                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="operationTimer" EventName="Tick" />
                                    </Triggers>
                                    <ContentTemplate>
                                    
				                        <table cellspacing="5">
                                            <tr>
                                                <td class="NormalBold">
                                                    <asp:Localize ID="locCpu" runat="server" meta:resourcekey="locCpu" Text="CPU:"/>
                                                </td>
				                                <td class="NormalBold" style="width:150px;">
				                                    <wsp:Gauge ID="cpuGauge" runat="server" Progress="0" Total="100" />
				                                    <asp:Literal ID="litCpuPercentage" runat="server" Text="0%"></asp:Literal>
				                                </td>
				                                <td>
				                                    <asp:Literal ID="litCpuUsage" runat="server" Text=""></asp:Literal>
				                                </td>
                                            </tr>
                                            <tr>
                                                <td class="NormalBold">
                                                    <asp:Localize ID="locRam" runat="server" meta:resourcekey="locRam" Text="RAM:"/>
                                                </td>
				                                <td class="NormalBold">
				                                    <wsp:Gauge ID="ramGauge" runat="server" Progress="0" Total="100" />
				                                    <asp:Literal ID="litRamPercentage" runat="server" Text="0%"></asp:Literal>
				                                </td>
				                                <td>
				                                    <asp:Literal ID="litRamUsage" runat="server" Text="Used: x MB, Total: x MB"></asp:Literal>
				                                </td>
                                            </tr>
                                            <tr id="HddRow" runat="server" visible="false">
                                                <td class="NormalBold">
                                                    <asp:Localize ID="locHdd" runat="server" meta:resourcekey="locHdd" Text="HDD:"/>
                                                </td>
				                                <td class="NormalBold">
				                                    <wsp:Gauge ID="hddGauge" runat="server" Progress="0" Total="100" />
				                                    <asp:Literal ID="litHddPercentage" runat="server" Text="0%"></asp:Literal>
				                                </td>
				                                <td>
				                                    <asp:Literal ID="litHddUsage" runat="server" Text="Free: x MB, Total: x MB on x drive(s)"></asp:Literal>
				                                </td>
                                            </tr>
				                        </table>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
				            </td>
				        </tr>
				    </table>
				    
			    </div>
		    </div>
	    </div>
    	

<asp:Panel ID="ChangeHostnamePanel" runat="server" CssClass="Popup" style="display:none;">
	<table class="Popup-Header" cellpadding="0" cellspacing="0">
		<tr>
			<td class="Popup-HeaderLeft"></td>
			<td class="Popup-HeaderTitle">
				<asp:Localize ID="locChangeHostname" runat="server" Text="Change VPS host name"
				    meta:resourcekey="locChangeHostname"></asp:Localize>
			</td>
			<td class="Popup-HeaderRight"></td>
		</tr>
	</table>
	<div class="Popup-Content">
		<div class="Popup-Body">
			<br />
			
			<table cellspacing="5">
			    <tr>
			        <td colspan="2">
			            <asp:ValidationSummary ID="validatorsSummary" runat="server" 
                            ValidationGroup="ChangeHostname" />
			        </td>
			    </tr>
			    <tr>
			        <td>
			            <asp:Localize ID="locHostname1" runat="server" Text="Host name:"
				            meta:resourcekey="locHostname1"></asp:Localize>
			        </td>
			        <td>
			            <asp:TextBox ID="txtHostname" runat="server" CssClass="NormalTextBox" Width="200"></asp:TextBox>
			            
			            <asp:RequiredFieldValidator ID="HostnameValidator" runat="server" Text="*" Display="Dynamic"
                                ControlToValidate="txtHostname" meta:resourcekey="HostnameValidator" SetFocusOnError="true"
                                ValidationGroup="ChangeHostname">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator id="valCorrectHostname" runat="server" Text="*" meta:resourcekey="valCorrectHostname"
                                ValidationExpression="^[a-zA-Z]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?$"
                                ControlToValidate="txtHostname" Display="Dynamic" SetFocusOnError="true" ValidationGroup="ChangeHostname">
                            </asp:RegularExpressionValidator>
			        </td>
			    </tr>
			    <tr>
			        <td>
			            <asp:Localize ID="locDomain" runat="server" Text="Domain:"
				            meta:resourcekey="locDomain"></asp:Localize>
			        </td>
			        <td>
			            <asp:TextBox ID="txtDomain" runat="server" CssClass="NormalTextBox" Width="200"></asp:TextBox>
			            
			            <asp:RequiredFieldValidator ID="DomainValidator" runat="server" Text="*" Display="Dynamic"
                                ControlToValidate="txtDomain" meta:resourcekey="DomainValidator" SetFocusOnError="true"
                                ValidationGroup="ChangeHostname">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator id="valNewDomainFormat" runat="server" Text="*" meta:resourcekey="valNewDomainFormat"
                                ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,6}$"
                                ControlToValidate="txtDomain" Display="Dynamic" SetFocusOnError="true" ValidationGroup="ChangeHostname">
                            </asp:RegularExpressionValidator>
			        </td>
			    </tr>
			    <tr>

			    </tr>
			</table>
			
                                                
			<br />
		</div>
		
		<div class="FormFooter">
		    <asp:Button ID="btnChangeHostname" runat="server" CssClass="Button1"
		        meta:resourcekey="btnChangeHostname" Text="Change" 
                ValidationGroup="ChangeHostname" onclick="btnChangeHostname_Click" />
		        
			<asp:Button ID="btnCancelHostname" runat="server" CssClass="Button1"
			    meta:resourcekey="btnCancelHostname" Text="Cancel" CausesValidation="false" />
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="ChangeHostnameModal" runat="server" BehaviorID="ChangeHostnameModal"
	TargetControlID="btnChangeHostnamePopup" PopupControlID="ChangeHostnamePanel"
	BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelHostname" />