<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangePublicFolderGeneralSettings.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangePublicFolderGeneralSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="scp" %>
<%@ Register Src="UserControls/PublicFolderTabs.ascx" TagName="PublicFolderTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register src="UserControls/AccountsListWithPermissions.ascx" tagname="AccountsListWithPermissions" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangePublicFolder48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Distribution List"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                        </h3>
                </div>
				<div class="panel-body form-horizontal">
                    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <scp:PublicFolderTabs id="tabs" runat="server" SelectedTab="public_folder_settings" />
                    </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
					<table>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locName" runat="server" meta:resourcekey="locName" Text="Folder Name: *"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valDisplayName" runat="server" meta:resourcekey="valRequireName" ControlToValidate="txtName"
									ErrorMessage="Enter Folder Name" ValidationGroup="EditPublicFolder" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
							</td>
						</tr>

						<tr>
						    <td></td>
							<td>
							    <br />
							    <CPCC:StyleButton id="btnMailDisable" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnMailDisable_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnMailDisable"/> </CPCC:StyleButton>&nbsp;
                                <CPCC:StyleButton id="btnMailEnable" CssClass="btn btn-success" runat="server" OnClick="btnMailEnable_Click" CausesValidation="false"> <i class="fa fa-check">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnMailEnable"/> </CPCC:StyleButton>

							</td>
						</tr>
					    
					    <tr><td>&nbsp;</td></tr>
                        					    <tr><td>&nbsp;</td></tr>
						<tr>
							<td colspan="2"><asp:Localize ID="locAllAccounts" runat="server" meta:resourcekey="locAllAccounts" Text="Accounts:"></asp:Localize></td>
						</tr>
						<tr>
						    <td colspan="2">                                
                            	<scp:AccountsListWithPermissions ID="allAccounts" runat="server" MailboxesEnabled="true" EnableMailboxOnly="true" DistributionListsEnabled="true"/>
                                
                            </td>
						</tr>
						<tr><td>&nbsp;</td></tr>
						
						<tr>
						    <td style="white-space:nowrap;">
						        <br />
						        <asp:CheckBox ID="chkHideAddressBook" runat="server" meta:resourcekey="chkHideAddressBook" Text="Hide from Address Book" />
						    </td>
						</tr>
					</table>
					

				</div>
                    </div>
				    <div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="EditPublicFolder"> <i class="fa fa-folder-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditPublicFolder" />
				    </div>