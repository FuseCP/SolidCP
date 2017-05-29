<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxMobileDetails.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeMailboxMobileDetails" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="scp" %>



				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeMailbox48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Mailbox"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                        </h3>
                </div>
				<div class="panel-body form-horizontal">
                    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <scp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_mobile" />
                    </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
				    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
                    <CPCC:StyleButton id="btnWipeAllData" CssClass="btn btn-danger" runat="server" OnClick="btnWipeAllData_Click"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnWipeAllDataText"/> </CPCC:StyleButton>
					<table>					    
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="loc" meta:resourcekey="locStatus"></asp:Localize></td>
					        <td><asp:Label runat="server" ID="lblStatus" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize1" meta:resourcekey="locDeviceModel"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceModel" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize2" meta:resourcekey="locDeviceType"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceType" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize3" meta:resourcekey="locFirstSyncTime"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblFirstSyncTime" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize4" meta:resourcekey="locDeviceWipeRequestTime"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceWipeRequestTime" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize5" meta:resourcekey="locDeviceAcnowledgeTime"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceAcnowledgeTime" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize6" meta:resourcekey="locLastSync"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblLastSync" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize7" meta:resourcekey="locLastUpdate"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblLastUpdate" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize10" meta:resourcekey="locLastPing"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblLastPing" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize11" meta:resourcekey="locDeviceFriendlyName"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceFriendlyName" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize12" meta:resourcekey="locDeviceId"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceId" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize13" meta:resourcekey="locDeviceUserAgent"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceUserAgent" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize14" meta:resourcekey="locDeviceOS"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceOS" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize15" meta:resourcekey="locDeviceOSLanguage"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceOSLanguage" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize16" meta:resourcekey="locDeviceIMEA"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceIMEA" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize17" meta:resourcekey="locDevicePassword"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDevicePassword" /></td>
					    </tr>
					</table>

                        </div>
				</div>
				    <div class="panel-footer text-right">
					   <CPCC:StyleButton id="btnBack" CssClass="btn btn-warning" runat="server" OnClick="btnBack_Click"> <i class="fa fa-arrow-left">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnBack"/> </CPCC:StyleButton>
				    </div>