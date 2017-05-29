<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationCreateSecurityGroup.ascx.cs" Inherits="SolidCP.Portal.ExchangeServer.OrganizationCreateSecurityGroup" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="scp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="scp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="scp" %>

<scp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
    <div class="panel-heading">
        <h3 class="panel-title">
            <asp:Image ID="Image1" SkinID="OrganizationUser48" runat="server" />
            <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Create Group"></asp:Localize>
        </h3>
    </div>
    <div class="panel-body form-horizontal">
        <scp:SimpleMessageBox id="messageBox" runat="server" />
        <div class="form-group">
            <asp:Label runat="server" CssClass="control-label col-sm-3" AssociatedControlID="txtDisplayName">
                <asp:Localize ID="locDisplayName" runat="server" meta:resourcekey="locDisplayName" Text="Display Name:"></asp:Localize>
            </asp:Label>
            <div class="col-sm-9">
                <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireDisplayName" runat="server" meta:resourcekey="valRequireDisplayName" ControlToValidate="txtDisplayName" ErrorMessage="Enter Display Name" ValidationGroup="CreateGroup" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <div class="panel-footer text-right">
        <CPCC:StyleButton id="btnCreate" CssClass="btn btn-success" runat="server" OnClick="btnCreate_Click" ValidationGroup="CreateGroup">
            <i class="fa fa-plus">&nbsp;</i>&nbsp;
            <asp:Localize runat="server" meta:resourcekey="btnCreateText"/>
        </CPCC:StyleButton>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateGroup" />
    </div>