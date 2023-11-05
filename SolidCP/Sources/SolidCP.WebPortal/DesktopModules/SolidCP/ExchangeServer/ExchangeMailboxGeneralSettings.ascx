<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxGeneralSettings.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeMailboxGeneralSettings" %>
<%@ Register Src="UserControls/CountrySelector.ascx" TagName="CountrySelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxPlanSelector.ascx" TagName="MailboxPlanSelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="scp" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="ExchangeMailbox48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Mailbox"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
					<asp:Literal ID="litMailboxType" runat="server" Text="" Visible="false" />
                    <asp:Image ID="imgVipUser" SkinID="VipUser16" runat="server" tooltip="VIP user" Visible="false"/>
                    <asp:Label ID="litServiceLevel" runat="server" style="float:right;padding-right:8px;" Visible="false"></asp:Label>
              </h3>
                          </div>
				<div class="panel-body form-horizontal">
                    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <scp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_settings" />
                    </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
					<asp:UpdatePanel ID="GeneralUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    				    <ContentTemplate>

					        <scp:CollapsiblePanel id="secGeneral" runat="server" TargetControlID="General" meta:resourcekey="secGeneral" Text="General"></scp:CollapsiblePanel>
                            <asp:Panel ID="General" runat="server" Height="0" style="overflow:hidden;">
					            <table>
						            <tr>
						                <td></td>
						                <td>
						                    <asp:CheckBox ID="chkHideAddressBook" runat="server" meta:resourcekey="chkHideAddressBook" Text="Hide from Address Book" />
						                </td>
						            </tr>
						            <tr>
						                <td></td>
						                <td>
						                    <asp:CheckBox ID="chkDisable" runat="server" meta:resourcekey="chkDisable" Text="Disable Mailbox" />
						                    <br />
						                    <br />
						                </td>
						            </tr>
					                <tr>
					                    <td class="FormLabel150"><asp:Localize ID="Localize2" runat="server" meta:resourcekey="locMailboxplanName" Text="Mailbox plan: *"></asp:Localize></td>
					                    <td>                                
                                            <scp:MailboxPlanSelector ID="mailboxPlanSelector" runat="server" CssClass="form-control" OnChanged="mailboxPlanSelector_Changed" />
                                        </td>
					                </tr>
					                <tr>
					                    <td class="FormLabel150"><asp:Localize ID="locDisclaimer" runat="server" meta:resourcekey="locDisclaimer" Text="Disclaimer: "></asp:Localize></td>
					                    <td>                                
                                            <asp:DropDownList ID="ddDisclaimer" runat="server" CssClass="form-control" />
                                        </td>
					                </tr>
									<tr runat="server" id="trCapacity">
					                    <td class="FormLabel150"><asp:Localize ID="locCapacity" runat="server" meta:resourcekey="locCapacity" Text="Capacity:"></asp:Localize><br /><br /></td>
					                    <td>                                
                                            <asp:TextBox ID="txtCapacity" runat="server" CssClass="form-control" Width="125px"></asp:TextBox>
											<asp:RegularExpressionValidator ID="valCapacity"
												ControlToValidate="txtCapacity" runat="server" SetFocusOnError="true"
												ErrorMessage="Wrong number format" ValidationGroup="EditMailbox"
												ValidationExpression="\d+" meta:resourcekey="valCapacity">
											</asp:RegularExpressionValidator>
                                        </td>
					                </tr>
                                    <tr>
                                        <td class="FormLabel150"><asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Mailbox Size:"></asp:Localize></td>
					                    <td>                                
                                            <scp:QuotaViewer ID="mailboxSize" runat="server" QuotaTypeId="2" DisplayGauge="true" /> MB
                                        </td>
					                </tr>

					            </table>
					        </asp:Panel>

                            <scp:CollapsiblePanel id="secRetentionPolicy" runat="server" TargetControlID="RetentionPolicy" meta:resourcekey="secRetentionPolicy" Text="Retention policy"></scp:CollapsiblePanel>
                            <asp:Panel ID="RetentionPolicy" runat="server" Height="0" style="overflow:hidden;">
					            <table>
					                <tr runat="server">
					                    <td class="FormLabel150"><asp:Localize ID="locRetentionPolicyName" runat="server" meta:resourcekey="locRetentionPolicyName" Text="Retention policy: "></asp:Localize></td>
					                    <td>                                
                                            <scp:MailboxPlanSelector ID="mailboxRetentionPolicySelector" runat="server" Archiving="true" AddNone="true"/>
                                        </td>
					                </tr>
					            </table>
					        </asp:Panel>

                            <scp:CollapsiblePanel id="secLitigationHoldSettings" runat="server" TargetControlID="LitigationHoldSettings" meta:resourcekey="secLitigationHoldSettings" Text="Litigation Hold"></scp:CollapsiblePanel>
                            <asp:Panel ID="LitigationHoldSettings" runat="server" Height="0" style="overflow:hidden;">
					            <table>
<!--
						            <tr>
						                <td></td>
						                <td>
						                    <asp:CheckBox ID="chkEnableLitigationHold" runat="server" meta:resourcekey="chkEnableLitigationHold" Text="Enable Litigation Hold" />
						                    <br />
						                    <br />
						                </td>
						            </tr>
-->
                                    <tr>
                                        <td class="FormLabel150"><asp:Localize ID="locLitigationHoldSpace" runat="server" meta:resourcekey="locLitigationHoldSpace" Text="Litigation Hold Space:"></asp:Localize></td>
					                    <td>                                
                                            <scp:QuotaViewer ID="litigationHoldSpace" runat="server" QuotaTypeId="2" DisplayGauge="true" /> MB
                                        </td>
					                </tr>
					            </table>
					        </asp:Panel>

                            <scp:CollapsiblePanel id="secArchiving" runat="server" TargetControlID="Archiving" meta:resourcekey="secArchiving" Text="Archiving"></scp:CollapsiblePanel>
                            <asp:Panel ID="Archiving" runat="server" Height="0" style="overflow:hidden;">
					            <table>
						            <tr>
						                <td class="FormLabel150"></td>
						                <td>
						                    <asp:CheckBox ID="chkEnableArchiving" runat="server" meta:resourcekey="chkEnableArchiving" Text="Enable archiving" />
						                    <br />
						                </td>
						            </tr>
                                    <tr id="rowArchiving" runat="server">
                                        <td class="FormLabel150"><asp:Localize ID="locArchivingQuotaViewer" runat="server" meta:resourcekey="locArchivingQuotaViewer" Text="Archive Size:"></asp:Localize></td>
					                    <td>                                
                                            <scp:QuotaViewer ID="archivingQuotaViewer" runat="server" QuotaTypeId="2" DisplayGauge="true" /> MB
                                        </td>
					                </tr>
					            </table>
					        </asp:Panel>

                        </ContentTemplate>
					</asp:UpdatePanel>

					<scp:CollapsiblePanel id="secBookingDelegates" runat="server" TargetControlID="BookingDelegates" meta:resourcekey="secBookingDelegates" Text="Booking delegates"></scp:CollapsiblePanel>
                    <asp:Panel ID="BookingDelegates" runat="server" Height="0" style="overflow:hidden;">
						<asp:UpdatePanel ID="upBookingDelegates" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
							<ContentTemplate>
					            <table>
									<tr>
										<td class="FormLabel150"><asp:Localize ID="locBookingRequests" runat="server" meta:resourcekey="locBookingRequests" Text="Booking requests:"></asp:Localize></td>
										<td></td>
									</tr>
									<tr>
										<td></td>
										<td>
											<asp:RadioButtonList ID="rblBookingRequests" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblBookingRequests_SelectedIndexChanged">
												<asp:ListItem Text="Use customized setting to accept or decline booking requests" meta:resourcekey="rblBookingRequestsCustom" Selected="True" Value="Custom"></asp:ListItem>
												<asp:ListItem Text="Accept or decline booking requests automatically" meta:resourcekey="rblBookingRequestsAuto" Value="Auto"></asp:ListItem>
												<asp:ListItem Text="Select delegates who can accept or decline booking requests" meta:resourcekey="rblBookingRequestsDelegates" Value="Delegates"></asp:ListItem>
											</asp:RadioButtonList>
										</td>
									</tr>
									<tr><td><br/></td><td><br/></td></tr>
									<tr>
										<td class="FormLabel150"><asp:Localize ID="locDelegates" runat="server" meta:resourcekey="locDelegates" Text="Delegates:"></asp:Localize><br /><br /></td>
										<td class="col-sm-10">
											<scp:MailboxSelector id="msDelegates" runat="server" ShowOnlyMailboxes="true" MailboxesEnabled="true" />
											<asp:CustomValidator runat="server" ValidationGroup="EditMailbox"  meta:resourcekey="valDelegates" ID="valDelegates" onservervalidate="valDelegates_ServerValidate" />
										</td>
									</tr>
					            </table>
							</ContentTemplate>
						</asp:UpdatePanel>
					</asp:Panel>
                    

					<scp:CollapsiblePanel id="secBookingOptions" runat="server" TargetControlID="BookingOptions" meta:resourcekey="secBookingOptions" Text="Booking options"></scp:CollapsiblePanel>
                    <asp:Panel ID="BookingOptions" runat="server" Height="0" style="overflow:hidden;">
						<asp:UpdatePanel ID="upBookingOptions" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
							<ContentTemplate>
					            <table>
									<tr>
						                <td></td>
						                <td>
						                    <asp:CheckBox ID="chkAllowRecurringMeetings" runat="server" meta:resourcekey="chkAllowRecurringMeetings" Text="Allow repeating meetings" />
						                </td>
						            </tr>
									<tr>
						                <td></td>
						                <td>
						                    <asp:CheckBox ID="chkScheduleOnlyDuringWorkHours" runat="server" meta:resourcekey="chkScheduleOnlyDuringWorkHours" Text="Allow scheduling only during working hours" />
						                </td>
						            </tr>
									<tr>
						                <td></td>
						                <td>
						                    <asp:CheckBox ID="chkEnforceSchedulingHorizon" runat="server" meta:resourcekey="chkEnforceSchedulingHorizon" Text="Always decline if the end date is beyond this limit" />
						                </td>
						            </tr>
									<tr>
										<td class="FormLabel150"><asp:Localize ID="locBookingWindowInDays" runat="server" meta:resourcekey="locBookingWindowInDays" Text="Maximum booking lead time (days):"></asp:Localize><br /><br /></td>
										<td>
											<asp:TextBox ID="txtBookingWindowInDays" runat="server" CssClass="form-control" Width="125px"></asp:TextBox>
											<asp:RegularExpressionValidator ID="valBookingWindowInDays"
												ControlToValidate="txtBookingWindowInDays" runat="server" SetFocusOnError="true"
												ErrorMessage="Wrong number format" ValidationGroup="EditMailbox"
												ValidationExpression="\d+" meta:resourcekey="valBookingWindowInDays">
											</asp:RegularExpressionValidator>
										</td>
									</tr>
									<tr>
										<td class="FormLabel150"><asp:Localize ID="locMaximumDuration" runat="server" meta:resourcekey="locMaximumDuration" Text="Maximum duration (hours):"></asp:Localize><br /><br /></td>
										<td>
											<asp:TextBox ID="txtMaximumDuration" runat="server" CssClass="form-control" Width="125px"></asp:TextBox>
											<asp:RegularExpressionValidator ID="valMaximumDuration"
												ControlToValidate="txtMaximumDuration" runat="server" SetFocusOnError="true"
												ErrorMessage="Wrong number format" ValidationGroup="EditMailbox"
												ValidationExpression="^(?:\d+(\.|\,)\d{1}|\d+)$" meta:resourcekey="valMaximumDuration">
											</asp:RegularExpressionValidator>
										</td>
									</tr>
									<tr>
										<td></td>
										<td><asp:Localize ID="locAdditionalResponse" runat="server" meta:resourcekey="locAdditionalResponse" Text="If you want the meeting organizer to receive a reply, enter the text below:"></asp:Localize></td>
									</tr>
									<tr>
						                <td></td>
						                <td>
						                    <asp:TextBox ID="txtAdditionalResponse" runat="server" Rows="4" TextMode="MultiLine" CssClass="form-control" />
						                </td>
						            </tr>
					            </table>
							</ContentTemplate>
						</asp:UpdatePanel>
					</asp:Panel>
					
					<table style="width:100%;margin-top:10px;">
					    <tr>
					        <td align="center">
					            <asp:CheckBox ID="chkPmmAllowed" Visible="false" runat="server" meta:resourcekey="chkPmmAllowed" AutoPostBack="true"
					                Text="Allow these settings to be managed from Personal Mailbox Manager" OnCheckedChanged="chkPmmAllowed_CheckedChanged" />
					        </td>
					    </tr>
					</table>

                    <scp:CollapsiblePanel id="secAdvancedInfo" runat="server" TargetControlID="AdvancedInfo" meta:resourcekey="secAdvancedInfo" Text="Advanced Information" IsCollapsed="true"></scp:CollapsiblePanel>
                    <asp:Panel ID="AdvancedInfo" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
						    <td class="FormLabel150"> <asp:Localize ID="locExchangeGuid" runat="server" meta:resourcekey="locExchangeGuid" Text="Exchange Guid:"></asp:Localize></td>
						    <td><asp:Label runat="server" ID="lblExchangeGuid" /></td>
						</tr>					   
					    </table>
					</asp:Panel>
					

                        </div>
				</div>
				    <div class="panel-footer text-right">
                        <scp:ItemButtonPanel id="buttonPanel" runat="server" ValidationGroup="EditMailbox" 
                            OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditMailbox" />
				    </div>