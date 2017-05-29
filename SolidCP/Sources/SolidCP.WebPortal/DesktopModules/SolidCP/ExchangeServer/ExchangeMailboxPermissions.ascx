<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxPermissions.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.ExchangeMailboxPermissions" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="uc2" %>
<%@ Register Src="UserControls/AccountsListWithPermissions.ascx" TagName="AccountsListWithPermissions" TagPrefix="uc2" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="scp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="scp" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>


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
                    <scp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_permissions" />
                    </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />                    					
					
                    <scp:CollapsiblePanel id="secFullAccessPermission" runat="server"
                        TargetControlID="panelFullAccessPermission" meta:resourcekey="secFullAccessPermission" Text="Full Access Permission">
                    </scp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelFullAccessPermission">
                        <asp:Label runat="server" ID="Label1" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsList id="fullAccessPermission" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true">
                        </uc2:AccountsList>                                            
                    </asp:Panel>
                    
                    <scp:CollapsiblePanel id="secSendAsPermission" runat="server"
                        TargetControlID="panelSendAsPermission" meta:resourcekey="secSendAsPermission" Text="Send As Permission">
                    </scp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelSendAsPermission">
                        <asp:Label runat="server" ID="lblSendAsPermission" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsList id="sendAsPermission" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true"  >
                        </uc2:AccountsList>                                           
                    </asp:Panel>

                    <scp:CollapsiblePanel id="secOnBehalfOf" runat="server"
                        TargetControlID="panelOnBehalfOf" meta:resourcekey="secOnBehalfOf" Text="Send on Behalf">
                    </scp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelOnBehalfOf">
                        <asp:Label runat="server" ID="Label2" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsList id="onBehalfOfPermissions" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true">
                        </uc2:AccountsList>                                            
                    </asp:Panel>
                    
                    <scp:CollapsiblePanel id="secCalendarPermissions" runat="server"
                        TargetControlID="panelCalendarPermissions" meta:resourcekey="secCalendarPermissions" Text="Calendar access">
                    </scp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelCalendarPermissions">
                        <asp:Label runat="server" ID="Label3" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsListWithPermissions id="calendarPermissions" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true">
                        </uc2:AccountsListWithPermissions>                                            
                    </asp:Panel>
                    
                    <scp:CollapsiblePanel id="secContactsPermissions" runat="server"
                        TargetControlID="panelContactsPermissions" meta:resourcekey="secContactsPermissions" Text="Contacts access">
                    </scp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelContactsPermissions">
                        <asp:Label runat="server" ID="Label4" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsListWithPermissions id="contactsPermissions" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true">
                        </uc2:AccountsListWithPermissions>                                            
                    </asp:Panel>


                        </div>
				</div>
<div class="panel-footer text-right">
                        <scp:ItemButtonPanel id="buttonPanel" runat="server" ValidationGroup="EditMailbox" 
                            OnSaveClick="btnSave_Click" OnSaveExitClick="btnSaveExit_Click" />
			        </div>		