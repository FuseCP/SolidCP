<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationDeletedUserMemberOf.ascx.cs" Inherits="SolidCP.Portal.HostedSolution.DeletedUserMemberOf" %>
<%@ Register Src="UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="scp" %>
<%@ Register Src="UserControls/CountrySelector.ascx" TagName="CountrySelector" TagPrefix="scp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>

<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="scp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="scp" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="scp" %>
<%@ Register Src="UserControls/GroupsList.ascx" TagName="GroupsList" TagPrefix="scp" %>



<%@ Register src="UserControls/DeletedUserTabs.ascx" tagname="UserTabs" tagprefix="uc1" %>
<%@ Register src="UserControls/MailboxTabs.ascx" tagname="MailboxTabs" tagprefix="uc1" %>

<%@ Register Src="../UserControls/ItemButtonPanel.ascx" TagName="ItemButtonPanel" TagPrefix="scp" %>


<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

				<div class="panel-heading">
                    <h3 class="panel-title">
					<asp:Image ID="Image1" SkinID="OrganizationUser48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit User"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                        </h3>
                </div>

				<div class="panel-body form-horizontal">
                    <div class="nav nav-tabs" style="padding-bottom:7px !important;">
                    <uc1:UserTabs ID="UserTabsId" runat="server" SelectedTab="deleted_user_memberof" />
                    <uc1:MailboxTabs ID="MailboxTabsId" runat="server" SelectedTab="deleted_user_memberof" />
                    </div>
                    <div class="panel panel-default tab-content">
                    <scp:SimpleMessageBox id="messageBox" runat="server" />
                    
					<scp:CollapsiblePanel id="secGroups" runat="server" TargetControlID="GroupsPanel" meta:resourcekey="secGroups" Text="Groups"></scp:CollapsiblePanel>
                    <asp:Panel ID="GroupsPanel" runat="server" Height="0" style="overflow:hidden;">
						<asp:UpdatePanel ID="GeneralUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
							<ContentTemplate>

                                <scp:AccountsList id="groups" runat="server"
                                            Disabled="true"
                                            MailboxesEnabled="false" 
                                            EnableMailboxOnly="true" 
										    ContactsEnabled="false"
										    DistributionListsEnabled="true"
                                            SecurityGroupsEnabled="true" />

							</ContentTemplate>
						</asp:UpdatePanel>
					</asp:Panel>
				</div>
                </div>