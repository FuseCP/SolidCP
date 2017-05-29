<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeDistributionListGeneralSettings.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeDistributionListGeneralSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="scp" %>
<%@ Register Src="UserControls/DistributionListTabs.ascx" TagName="DistributionListTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeList48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Distribution List"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                        </h3>
                </div>
				<div class="panel-body form-horizontal">
                    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <scp:DistributionListTabs id="tabs" runat="server" SelectedTab="dlist_settings" />
                    </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
					<div class="form-group">
                        <asp:Label runat="server" CssClass="control-label col-sm-2" AssociatedControlID="txtDisplayName">
                            <asp:Localize ID="locDisplayName" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize>
                        </asp:Label>
                        <div class="col-sm-10">
                                        <div class="input-group">
								            <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" ValidationGroup="CreateMailbox"></asp:TextBox>
								            <asp:RequiredFieldValidator ID="valRequireDisplayName" runat="server" meta:resourcekey="valRequireDisplayName" ControlToValidate="txtDisplayName"
									        ErrorMessage="Enter Display Name" ValidationGroup="EditList" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                        </div>
                        </div>
                        <div class="form-group">
                        <div class="col-sm-10 col-sm-offset-2">
						        <asp:CheckBox ID="chkHideAddressBook" runat="server" meta:resourcekey="chkHideAddressBook" Text="Hide from Address Book" />
						</div>
                        </div>
                         <div class="form-group">
                        <asp:label runat="server" AssociatedControlID="txtDisplayName" CssClass="control-label col-sm-2">
						   <asp:Localize ID="locManager" runat="server" meta:resourcekey="locManager" Text="Manager:"></asp:Localize>
                        </asp:label>
                        <div class="col-sm-6">
                                <scp:MailboxSelector id="manager" runat="server"
                                            ShowOnlyMailboxes="true" 
											MailboxesEnabled="true"
											DistributionListsEnabled="true" />
											
								<asp:CustomValidator runat="server" 
                                     ValidationGroup="EditList"  meta:resourcekey="valManager" ID="valManager" 
                                     onservervalidate="valManager_ServerValidate" />
                         </div>
                        </div>
					    <br /><br />
					    <div class="form-group">
                        <asp:label runat="server" AssociatedControlID="members" CssClass="control-label col-sm-2">
                            <asp:Localize ID="locMembers" runat="server" meta:resourcekey="locMembers" Text="Members:"></asp:Localize>
                        </asp:label>
						<div class="col-sm-10">
                                 <div class="input-group">
                                            <scp:AccountsList id="members" runat="server" MailboxesEnabled="true" EnableMailboxOnly="true" ContactsEnabled="true" DistributionListsEnabled="true" SharedMailboxEnabled="true"  />
                                </div>
						</div>
                        </div>
						<div class="form-group">
                        <div class="col-sm-10 col-sm-offset-2">
							    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" Rows="4" TextMode="MultiLine"></asp:TextBox>
						</div>
                        </div>
                        </div>
                    </div>
				    <div class="panel-footer text-right">
					    <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="EditList"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSaveText"/> </CPCC:StyleButton>&nbsp;
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditList" />
				    </div>