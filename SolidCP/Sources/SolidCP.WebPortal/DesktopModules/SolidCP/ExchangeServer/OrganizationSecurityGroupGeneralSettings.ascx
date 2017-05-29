<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationSecurityGroupGeneralSettings.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.OrganizationSecurityGroupGeneralSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/UsersList.ascx" TagName="UsersList" TagPrefix="scp"%>
<%@ Register Src="UserControls/SecurityGroupTabs.ascx" TagName="SecurityGroupTabs" TagPrefix="scp"%>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
    <div class="panel-heading">
        <h3 class="panel-title">
            <asp:Image ID="Image1" SkinID="OrganizationUser48" runat="server" />
            <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Security Group"></asp:Localize>
            <asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
        </h3>
    </div>
    <div class="panel-body form-horizontal">
        <div class="nav nav-tabs" style="padding-bottom:7px !important;">
            <scp:SecurityGroupTabs id="tabs" runat="server" SelectedTab="secur_group_settings" />
        </div>
        <div class="panel panel-default tab-content">
        <scp:SimpleMessageBox id="messageBox" runat="server" />
            <div class="form-group">
                <asp:Label runat="server" CssClass="control-label col-sm-3" AssociatedControlID="txtDisplayName">
                    <asp:Localize ID="locDisplayName" runat="server" meta:resourcekey="locDisplayName" Text="Display Name:"></asp:Localize>
                </asp:Label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" ValidationGroup="CreateMailbox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valRequireDisplayName" runat="server" meta:resourcekey="valRequireDisplayName" ControlToValidate="txtDisplayName" ErrorMessage="Enter Display Name" ValidationGroup="EditList" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" CssClass="control-label col-sm-3" AssociatedControlID="lblGroupName">
                    <asp:Localize ID="locGroupName" runat="server" meta:resourcekey="locGroupName" Text="Windows Group Name:"></asp:Localize>
                </asp:Label>
                <div class="col-sm-9">
                    <asp:TextBox ID="lblGroupName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" CssClass="control-label col-sm-3" AssociatedControlID="members">
                    <asp:Localize ID="locMembers" runat="server" meta:resourcekey="locMembers" Text="Members:"></asp:Localize>
                </asp:Label>
                <div class="col-sm-9">
                    <scp:UsersList id="members" runat="server" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" CssClass="control-label col-sm-3" AssociatedControlID="txtNotes">
                    <asp:Localize ID="locNotes" runat="server" meta:resourcekey="locNotes" Text="Notes:"></asp:Localize>
                </asp:Label>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" Rows="4" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="panel-footer text-right">
        <CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click" ValidationGroup="EditList">
             <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnSaveText"/>
        </CPCC:StyleButton>
        &nbsp;
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditList" />
    </div>