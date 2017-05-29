<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SfBEditUser.ascx.cs" Inherits="SolidCP.Portal.SfB.EditSfBUser" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register src="../ExchangeServer/UserControls/MailboxSelector.ascx" tagname="MailboxSelector" tagprefix="uc1" %>
<%@ Register Src="UserControls/SfBUserPlanSelector.ascx" TagName="SfBUserPlanSelector" TagPrefix="scp" %>
<%@ Register Src="UserControls/SfBUserSettings.ascx" TagName="SfBUserSettings" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<div id="ExchangeContainer">
    <div class="Module">
        <div class="Left">
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="Image1" SkinID="SfBLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                    -
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                    <asp:Image ID="imgVipUser" SkinID="VipUser16" runat="server" tooltip="VIP user" Visible="false"/>
                    <asp:Label ID="litServiceLevel" runat="server" style="float:right;padding-right:8px;" Visible="false"></asp:Label>
                </div>
                <div class="panel-body form-horizontal">
                    
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <table>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locPlanName" runat="server" meta:resourcekey="locPlanName" Text="Plan Name: *"></asp:Localize>
                            </td>
                            <td>                                
                                <scp:SfBUserPlanSelector ID="planSelector" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locSipAddress" runat="server" meta:resourcekey="locSipAddress" Text="SIP Address: *"></asp:Localize>
                            </td>
                            <td>                                
                                <scp:SfBUserSettings ID="sfbUserSettings" runat="server" />
                            </td>
                        </tr>
                    </table>

                    <asp:Panel runat="server" ID="pnEnterpriseVoice">
                    <table>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize runat="server" ID="locPhoneNumber" meta:resourcekey="locPhoneNumber" Text="Phone Number:" />
                            </td>
                            <td>
                                <!-- <asp:TextBox runat="server" ID="tb_PhoneNumber" /> -->
                                <asp:dropdownlist id="ddlPhoneNumber" Runat="server" CssClass="NormalTextBox"></asp:dropdownlist>
                                <asp:RegularExpressionValidator ID="PhoneFormatValidator" runat="server"
		                        ControlToValidate="ddlPhoneNumber" Display="Dynamic" ValidationGroup="Validation1" SetFocusOnError="true"
		                        ValidationExpression="^([0-9])*$"
                                ErrorMessage="Must contain only numbers.">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize runat="server" ID="locSfBPin" meta:resourcekey="locSfBPin" Text="SfB Pin:" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbPin" />
                                <asp:RegularExpressionValidator ID="PinRegularExpressionValidator" runat="server"
		                        ControlToValidate="tbPin" Display="Dynamic" ValidationGroup="Validation1" SetFocusOnError="true"
		                        ValidationExpression="^([0-9])*$"
                                ErrorMessage="Must contain only numbers.">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
					</table>
                    </asp:Panel>
                        
					<div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnSaveExit" CssClass="btn btn-warning" runat="server" OnClick="btnSaveExit_Click" ValidationGroup="Validation1"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveExitText"/> </CPCC:StyleButton>&nbsp;
                        <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="Validation1"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>
				    </div>			
                </div>
            </div>
        </div>
    </div>
</div>
