<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LyncEditUser.ascx.cs" Inherits="SolidCP.Portal.Lync.EditLyncUser" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register src="../ExchangeServer/UserControls/MailboxSelector.ascx" tagname="MailboxSelector" tagprefix="uc1" %>
<%@ Register Src="UserControls/LyncUserPlanSelector.ascx" TagName="LyncUserPlanSelector" TagPrefix="scp" %>
<%@ Register Src="UserControls/LyncUserSettings.ascx" TagName="LyncUserSettings" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
			<div class="panel-heading">
                    <h3 class="panel-title">
                    <asp:Image ID="Image1" SkinID="LyncLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                    -
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                    <asp:Image ID="imgVipUser" SkinID="VipUser16" runat="server" tooltip="VIP user" Visible="false"/>
                    <asp:Label ID="litServiceLevel" runat="server" style="float:right;padding-right:8px;" Visible="false"></asp:Label>
                </h3>
                        </div>
                <div class="panel-body form-horizontal">
                    
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <table>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locPlanName" runat="server" meta:resourcekey="locPlanName" Text="Plan Name: *"></asp:Localize>
                            </td>
                            <td>                                
                                <scp:LyncUserPlanSelector ID="planSelector" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="FormLabel150">
                                <asp:Localize ID="locSipAddress" runat="server" meta:resourcekey="locSipAddress" Text="SIP Address: *"></asp:Localize>
                            </td>
                            <td>                                
                                <scp:LyncUserSettings ID="lyncUserSettings" runat="server" />
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
                                <asp:Localize runat="server" ID="locLyncPin" meta:resourcekey="locLyncPin" Text="Lync Pin:" />
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
                        
	
                </div>
					<div class="panel-footer text-right">
					 <CPCC:StyleButton id="btnSaveExit" CssClass="btn btn-warning" runat="server" OnClick="btnSaveExit_Click" ValidationGroup="Validation1"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveExitText"/> </CPCC:StyleButton>&nbsp;
                     <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="Validation1"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>				 					                                                
				    </div>		