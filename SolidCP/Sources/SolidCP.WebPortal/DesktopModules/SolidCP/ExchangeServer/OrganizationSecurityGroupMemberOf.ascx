<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationSecurityGroupMemberOf.ascx.cs" Inherits="SolidCP.Portal.HostedSolution.OrganizationSecurityGroupMemberOf" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>

<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="scp" %>
<%@ Register Src="UserControls/SecurityGroupTabs.ascx" TagName="SecurityGroupTabs" TagPrefix="scp"%>

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
            <scp:SecurityGroupTabs id="tabs" runat="server" SelectedTab="secur_group_memberof" />
        </div>
        <div class="panel panel-default tab-content">
            <scp:SimpleMessageBox id="messageBox" runat="server" />
            <scp:CollapsiblePanel id="secGroups" runat="server" TargetControlID="GroupsPanel" meta:resourcekey="secGroups" Text="Groups"></scp:CollapsiblePanel>
            <asp:Panel ID="GroupsPanel" runat="server" Height="0" style="overflow:hidden;">
                <asp:UpdatePanel ID="GeneralUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <scp:AccountsList id="groups" runat="server" MailboxesEnabled="false" EnableMailboxOnly="true" ContactsEnabled="false" DistributionListsEnabled="true" SecurityGroupsEnabled="true" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
    </div>
    <div class="panel-footer text-right">
        <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="EditMailbox">
            <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnSaveText"/>
        </CPCC:StyleButton>
        &nbsp;
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditMailbox" />
    </div>