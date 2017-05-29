<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointUsersEditUser.ascx.cs" Inherits="SolidCP.Portal.SharePointUsersEditUser" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<div class="panel-body form-horizontal">
    <table cellSpacing="0" cellPadding="5" width="100%">
        <tr>
            <td class="SubHead" noWrap width="200"><asp:Label ID="lblUserName" runat="server" meta:resourcekey="lblUserName" Text="User Name:"></asp:Label></td>
            <td class="NormalBold" width="100%">
                <uc3:UsernameControl ID="usernameControl" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" valign="top">
                <asp:Label ID="lblUserPassword" runat="server" meta:resourcekey="lblUserPassword" Text="User Password:"></asp:Label></td>
            <td class="Normal" valign="top">
                <uc2:PasswordControl ID="passwordControl" runat="server" />
            </td>
        </tr>
    </table>
    
    <scp:CollapsiblePanel id="secGroups" runat="server"
        TargetControlID="GroupsPanel" meta:resourcekey="secGroups" Text="Member Of">
    </scp:CollapsiblePanel>
    <asp:Panel ID="GroupsPanel" runat="server" Height="0" style="overflow:hidden;">
        <table id="tblGroups" runat="server" cellSpacing="0" cellPadding="3" width="100%">
            <tr>
                <td colspan="2">
	                <asp:checkboxlist id="dlGroups" CellPadding="3" RepeatColumns="2" CssClass="NormalBold" DataTextField="Name"
		                DataValueField="Name" Runat="server"></asp:checkboxlist>
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
<div class="panel-footer text-right">
    <CPCC:StyleButton id="btnDelete" CssClass="btn btn-danger" runat="server" CausesValidation="False" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete this user?');"> <i class="fa fa-trash-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnDeleteText"/> </CPCC:StyleButton>&nbsp;
    <CPCC:StyleButton id="btnCancel" CssClass="btn btn-warning" runat="server" CausesValidation="False" OnClick="btnCancel_Click"> <i class="fa fa-times">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnCancel"/> </CPCC:StyleButton>&nbsp;
	<CPCC:StyleButton id="btnSave" CssClass="btn btn-success" runat="server" OnClick="btnSave_Click"> <i class="fa fa-floppy-o">&nbsp;</i>&nbsp;<asp:Localize runat="server" meta:resourcekey="btnSave"/> </CPCC:StyleButton>
</div>