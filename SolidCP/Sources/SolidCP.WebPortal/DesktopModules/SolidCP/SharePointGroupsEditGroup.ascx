<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointGroupsEditGroup.ascx.cs" Inherits="SolidCP.Portal.SharePointGroupsEditGroup" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="scp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<div class="panel-body form-horizontal">
    <table cellSpacing="0" cellPadding="5" width="100%">
        <tr>
            <td class="SubHead" noWrap width="200"><asp:Label ID="lblGroupName" runat="server" meta:resourcekey="lblGroupName" Text="Group name:"></asp:Label></td>
            <td class="NormalBold" width="100%">
                <uc2:UsernameControl ID="usernameControl" runat="server" />
            </td>
        </tr>
    </table>
    
    <scp:CollapsiblePanel id="secUsers" runat="server"
        TargetControlID="UsersPanel" meta:resourcekey="secUsers" Text="Members">
    </scp:CollapsiblePanel>
    <asp:Panel ID="UsersPanel" runat="server" Height="0" style="overflow:hidden;">
        <table id="tblUsers" runat="server" cellSpacing="0" cellPadding="3" width="100%">
            <tr>
                <td colspan="2">
	                <asp:checkboxlist id="dlUsers" CellPadding="3" RepeatColumns="2" CssClass="NormalBold" Runat="server"
		                DataValueField="Name" DataTextField="Name"></asp:checkboxlist>
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